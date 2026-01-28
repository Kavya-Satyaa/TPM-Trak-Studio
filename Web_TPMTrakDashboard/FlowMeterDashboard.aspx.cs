using BusinessClassLibrary;
using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class FlowMeterDashboard : System.Web.UI.Page
    {
        string DefaultShift = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                if (!IsPostBack)
                {
                    SessionClear.ClearSession();
                    if (Session["FromDate"] != null && Session["ToDate"] != null)
                    {
                        txtFromDate.Text = Convert.ToDateTime(Session["FromDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        txtToDate.Text = Convert.ToDateTime(Session["ToDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        GetTimings();
                    }
                    BindPlantId();
                    BindMachineID();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void GetTimings()
        {
            DefaultShift = DataBaseAccess.GetDefaultCockpitDefaultShift();
            if (DefaultShift.Equals("PreviousShift"))
            {
                var prevShiftVals = DataBaseAccess.GetCurrentOrPreviousShiftVals("[s_GetPreviousShift]");
                if (prevShiftVals != null)
                {
                    txtFromDate.Text = Convert.ToDateTime(prevShiftVals[0]).ToString("yyyy-MM-dd HH:mm:ss");
                    txtToDate.Text = Convert.ToDateTime(prevShiftVals[1]).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            else if (DefaultShift.Equals("CurrentShift"))
            {
                var currShiftVals = DataBaseAccess.GetCurrentOrPreviousShiftVals("[s_GetCurrentShift]");
                if (currShiftVals != null)
                {
                    txtFromDate.Text = Convert.ToDateTime(currShiftVals[0]).ToString("yyyy-MM-dd HH:mm:ss");
                    txtToDate.Text = Convert.ToDateTime(currShiftVals[1]).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            else if (DefaultShift.Equals("Last 24Hrs"))
            {
                string fromdate = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                txtFromDate.Text = fromdate + DateTime.Now.ToString("HH:mm:ss");
                string todate = DateTime.Today.ToString("yyyy-MM-dd");
                txtToDate.Text = todate + DateTime.Now.ToString("HH:mm:ss");
            }
        }

        #region "Bind Plant Id"
        private void BindPlantId()
        {
            try
            {
                List<string> lstPlantData = BindCockpitView.ViewPlantToDisplay();
                ddlPlantId.DataSource = lstPlantData;
                ddlPlantId.DataBind();
                ddlPlantId.Items.Insert(0, new ListItem
                {
                    Text = GetGlobalResourceObject("CommanResource", "PlantAll").ToString(),
                    Value = "All"
                });
                if(lstPlantData.Count>0)
                {
                    ddlPlantId.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Machine ID"
        private void BindMachineID()
        {
            try
            {
                string PlantID = ddlPlantId.SelectedItem != null ? ddlPlantId.SelectedItem.ToString() : "";
                if (!string.IsNullOrEmpty(PlantID))
                {
                    List<string> listMachineID = new List<string>();
                    listMachineID = VDGDataBaseAccess.GetMachineIDsLst(PlantID); 
                    if (listMachineID != null && listMachineID.Count > 0)
                    {
                        if (listMachineID.Contains("All")) listMachineID.Remove("All");
                        ddlMachineId.DataSource = listMachineID;
                        ddlMachineId.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineID();
        }

        protected void btnGetComponents_Click(object sender, EventArgs e)
        {
            try
            {
                string MachineID = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedItem.ToString() : "";
                if (!string.IsNullOrEmpty(MachineID))
                {
                    DateTime FromDate = Util.GetDateTime(txtFromDate.Text.Trim());
                    DateTime ToDate = Util.GetDateTime(txtToDate.Text.Trim());
                    if (DateTime.Compare(FromDate, ToDate) > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('From date cannot be greater than To date.')", true);
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "hideloader", "HideLoader()", true);
                        FromDate = DateTime.Now;
                        ToDate = DateTime.Now.AddDays(1);
                        txtFromDate.Text = FromDate.ToString("yyyy-MM-dd HH:mm:ss");
                        txtToDate.Text = ToDate.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        if ((ToDate - FromDate).TotalDays > 30)
                        {
                            lblMessages.Text = "Difference between to date and from date cannot be more than 30 days.";
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "hideloader", "HideLoader()", true);
                            return;
                        }
                        else
                        {
                            string strDate = FromDate.ToSQLDateTimeFormat();
                            string endDate = ToDate.ToSQLDateTimeFormat();
                            List<string> listComponent = DataBaseAccess.GetComponentsForPlant(strDate, endDate, MachineID);
                            //List<string> listComponent = new List<string>() { "SHFT-2-106- R", "SHFT-3-138-5164", "SHFT-3-E781-0", "SHFT-3-F002A13032" };
                            if (listComponent != null && listComponent.Count > 0)
                            {
                                if (listComponent.Contains("All")) listComponent.Remove("All");
                                ddlMultiComponentId.DataSource = listComponent;
                                ddlMultiComponentId.DataBind();
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No components found for selected time range and machine.')", true);
                            }
                        }
                    }
                }
                else
                {
                    lblMessages.Text = "Please select Machine ID.";
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "hideloader", "HideLoader()", true);
        }

        protected void ddlMultiComponentId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int count = ddlMultiComponentId.Items.Cast<ListItem>().Where(li => li.Selected).Count();
                if (count > 4)
                {
                    ddlMultiComponentId.Items.Cast<ListItem>().Where(li => li.Selected).Skip(4).All(i => i.Selected = false);
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "keepModalOpen", "openComponentIdModal();", true);
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        #region "Refresh Machine Status"
        [WebMethod]
        public static FlowMeterChartModel GetFlowMeterData(string fromDate, string toDate, string machineId, string componentId)
        {
            return DataBaseAccess.GetFlowMeterData(fromDate, toDate, machineId, componentId);
        }

        [WebMethod]
        public static string GetMachineDescription(string machineId)
        {
            return DataBaseAccess.GetMachineDescription(machineId);
        }
        #endregion
    }
}