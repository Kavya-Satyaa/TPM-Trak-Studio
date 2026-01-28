using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.AceDesigners.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.AceDesigners
{
    public partial class ScheduleDataAce : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindMachineId();
                BindMRPController();
                btnDateView_Click(null, null);
            }
        }
        private void BindMachineId()
        {
            try
            {
                List<string> list = DataBaseAccess.GetMachineIDForPlant("");
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void BindMRPController()
        {
            try
            {
                List<string> list = DataBaseAccess.GetMRPController();
                if(list.Count>0)
                {
                    list.Insert(0, "None");
                    ddlMRPController.DataSource = list;
                    ddlMRPController.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void BindData()
        {
            try
            {
                string status = "";
                foreach (ListItem item in lbStatus.Items)
                {
                    if (item.Selected)
                    {
                        if (status == "") status += "'" + item.Value + "'"; else status += ",'" + item.Value + "'";
                    }
                }
                string sapStatus = "";
                foreach (ListItem item in lbSAPStatus.Items)
                {
                    if (item.Selected)
                    {
                        if (sapStatus == "") sapStatus += "'" + item.Value + "'"; else sapStatus += ",'" + item.Value + "'";
                    }
                }
                DataTable dt = new DataTable();
                List<ScheduleEntity> list = AceDatabaseAccess.getScheduleDetails(ddlMachine.SelectedValue, status, txtFromDate.Text, txtToDate.Text, txtCompSearch.Text, txtPOSearch.Text, hdnViewType.Value, sapStatus, ddlMRPController.SelectedValue,out dt);
                Session["PriorityValidation"] = dt;
                if (list.Count > 0)
                {
                    btnSave.Visible = true;
                    btnSendToHMI.Visible = true;
                }
                else
                {
                    btnSave.Visible = false;
                    btnSendToHMI.Visible = false;
                }
                lvScheduleData.DataSource = list;
                lvScheduleData.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnDateView_Click(object sender, EventArgs e)
        {
            hdnViewType.Value = "Date";
            BindData();
        }

        protected void btnCompPOView_Click(object sender, EventArgs e)
        {
            hdnViewType.Value = "CompPO";
            BindData();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                for (int i = 0; i < lvScheduleData.Items.Count; i++)
                {
                    var row = lvScheduleData.Items[i];
                    if ((row.FindControl("hdnUpdate") as HiddenField).Value == "update")
                    {
                        ScheduleEntity data = new ScheduleEntity();
                        data.ID = (row.FindControl("hdnIDD") as HiddenField).Value;
                        data.SchedulePriority = (row.FindControl("txtSchedulePriority") as TextBox).Text;
                        data.ScheduleDate = (row.FindControl("txtScheduleDate") as TextBox).Text;
                        data.Sequence = (row.FindControl("txtSequence") as TextBox).Text;
                        data.UpdatedBy = Session["UserName"].ToString();
                        data.ChkJobType= (row.FindControl("ChkJobType") as CheckBox).Checked;
                        if (data.ChkJobType)
                            data.JobType = "Development";
                        else
                            data.JobType = "";

                        //if (!string.IsNullOrEmpty(data.ScheduleDate) && !string.IsNullOrEmpty(data.SchedulePriority))
                        //{
                        result += AceDatabaseAccess.saveScheduleDetails(data);
                        //}
                    }
                }
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Saved Successfully.')", true);
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnSendToHMI_Click(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                //string idd = "";
                //for (int i = 0; i < lvScheduleData.Items.Count; i++)
                //{
                //    var row = lvScheduleData.Items[i];
                //    if ((row.FindControl("chkSelect") as CheckBox).Checked)
                //    {
                //        if ((row.FindControl("hdnOldSequence") as HiddenField).Value != "")
                //        {
                //            if (idd == "")
                //                idd += (row.FindControl("hdnIDD") as HiddenField).Value;
                //            else
                //                idd += "," + (row.FindControl("hdnIDD") as HiddenField).Value;
                //        }
                //    }
                //}
                if (hdnSendToHMISelctedIDD.Value != "")
                {
                    result = AceDatabaseAccess.sendScheduleDataToHMI(hdnSendToHMISelctedIDD.Value);
                    hdnSendToHMISelctedIDD.Value = "";
                }
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Saved Successfully.')", true);
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static PrioritySequenceExistenceEntity isPriorityExists(string priority, string sequence, string idd, string scheduleDate)
        {
            PrioritySequenceExistenceEntity data = new PrioritySequenceExistenceEntity();
            bool priorityExists = false;
            try
            {
                DataTable dt = new DataTable();
                if (HttpContext.Current.Session["PriorityValidation"] != null)
                {
                    dt = HttpContext.Current.Session["PriorityValidation"] as DataTable;
                }
                if (dt.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(priority))
                    {
                        Int64 iddInt = Convert.ToInt64(idd);
                        int priorityInt = Convert.ToInt32(priority);
                        DateTime scheduleDateInDateTime = Util.GetDateTime(scheduleDate);
                        var row = dt.AsEnumerable().Where(k => k.Field<int?>("SchedulePriority") == priorityInt && k.Field<DateTime?>("ScheduleDate") == scheduleDateInDateTime && k.Field<Int64>("IDD") != iddInt).FirstOrDefault();
                        if (row != null)
                        {
                            data.PriorityExists = true;
                        }
                    }
                    else if (!string.IsNullOrEmpty(sequence))
                    {
                        int sequenceInt = Convert.ToInt32(sequence);
                        var row = dt.AsEnumerable().Where(k => k.Field<int?>("Sequence") == sequenceInt && k.Field<string>("SCHStatus").Equals("Running", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (row != null)
                        {
                            data.SequenceExists = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return data;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void lvScheduleData_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if(e.Item.ItemType== ListViewItemType.DataItem)
                {
                    string value = (e.Item.FindControl("hdnStatus") as HiddenField).Value;
                    (e.Item.FindControl("ddlStatus") as DropDownList).SelectedValue = value;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblmsg.Text = string.Empty;
                int rowIndex = Convert.ToInt32(((sender as DropDownList).NamingContainer as ListViewDataItem).DataItemIndex);
                hdnStatusIDD.Value = (lvScheduleData.Items[rowIndex].FindControl("hdnIDD") as HiddenField).Value;
                hdnStatus.Value = (lvScheduleData.Items[rowIndex].FindControl("ddlStatus") as DropDownList).SelectedValue;
               
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openModal", "openModals('" + "newLoginModal" + "');", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string userame = "";
                string password = "";
                userame = System.Web.Configuration.WebConfigurationManager.AppSettings["AceScheduleUsername"].ToString();
                password = System.Web.Configuration.WebConfigurationManager.AppSettings["AceSchedulePassword"].ToString();

                if(!string.IsNullOrEmpty(txtUserName.Text) && !string.IsNullOrEmpty(txtPassword.Text))
                {
                    if(txtUserName.Text.Equals(userame) && txtPassword.Text.Equals(password))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmModal", "clearAllModalScreen();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "successMsg", "openSuccessModal('LogIn Successfull.','');", true);
                        SaveStatusData(hdnStatusIDD.Value,hdnStatus.Value);
                    }
                    else
                    {
                        //lblmsg.Text = "Wrong UserName and Password";
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "openModal", "openModals('" + "newLoginModal" + "');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmModal", "clearAllModalScreen();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "successMsg", "showWarningMsg('LogIn Failed.','');", true);
                    }
                    BindData();
                }
                else
                {
                    lblmsg.Text = "Please Enter UserName and Password";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "openModal", "openModals('" + "newLoginModal" + "');", true);
                }
               
                txtUserName.Text = string.Empty;
                txtPassword.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnSave_Click: " + ex.Message);
            }
        }

        private void SaveStatusData(string IDD,string Status)
        {
            try
            {
                int result = 0;
                ScheduleEntity data = new ScheduleEntity();
                data.ID = IDD;
                data.Status = Status;
                data.UpdatedBy= Session["UserName"].ToString();
                result += AceDatabaseAccess.saveScheduleStatusDetails(data);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmModal", "clearAllModalScreen();", true);
            BindData();
        }
    }
}