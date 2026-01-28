using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class ItemStdCycleTimeMasterScreen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindMachine();
                BindUpdatedBy();

                if (Request.QueryString["NavigateFrom"] != null)
                {
                    if (Request.QueryString["NavigateFrom"].ToString() == "PlanEntry")
                    {
                        txtFromDate.Text = DataBaseAccess.GetItemStdCycleTimeFromDate().ToString("dd-MM-yyyy");
                        if (ddlUpdatedBy.Items.FindByValue("ERP") != null)
                        {
                            ddlUpdatedBy.SelectedValue = "ERP";
                        }
                    }
                }

                BindDetails();
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllMachinedByLineandGroup("", "");
                list.Insert(0, "All");
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachine: " + ex.Message);
            }
        }
        private void BindUpdatedBy()
        {
            try
            {
                List<string> list = DataBaseAccess.GetItemStdCycleTimeUpdatedByDetails();
                list.Insert(0, "All");
                ddlUpdatedBy.DataSource = list;
                ddlUpdatedBy.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindUpdatedBy: " + ex.Message);
            }
        }
        private void BindDetails()
        {
            try
            {
                List<ItemStdCycleTimeDetails> list = DataBaseAccess.GetItemStdCycleTimeDetails(txtFromDate.Text, txtToDate.Text, ddlMachine.SelectedValue, txtItemCode.Text, ddlUpdatedBy.SelectedValue, "View"); ;
                lvDetails.DataSource = list;
                lvDetails.DataBind();

                Session["ItemStdCycleTimeDetails"] = list;
                ViewState["FromDate"] = txtFromDate.Text;
                ViewState["ToDate"] = txtToDate.Text;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindDetails: " + ex.Message);
            }
        }
      

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindDetails();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isOK = true;
                for (int i = 0; i < lvDetails.Items.Count; i++)
                {
                    ItemStdCycleTimeDetails data = new ItemStdCycleTimeDetails();
                    data.MachineId = (lvDetails.Items[i].FindControl("lblMachineId") as Label).Text;
                    data.ItemCode = (lvDetails.Items[i].FindControl("lblItemCode") as Label).Text;
                    data.OperationNo = (lvDetails.Items[i].FindControl("hfOperationNo") as HiddenField).Value;
                    data.StdMachiningTime = (lvDetails.Items[i].FindControl("txtStdMachiningTime") as TextBox).Text;
                    data.StdLoadUnloadTime = (lvDetails.Items[i].FindControl("txtStdLoadUnloadTime") as TextBox).Text;
                    string machiningTime = (lvDetails.Items[i].FindControl("hfStdMachiningTime") as HiddenField).Value;
                    string loadunloadTime = (lvDetails.Items[i].FindControl("hfStdLoadUnloadTime") as HiddenField).Value;
                    data.Param = "Update";
                    if (machiningTime != data.StdMachiningTime || loadunloadTime != data.StdLoadUnloadTime)
                    {
                        string success = DataBaseAccess.SaveItemStdCycleTimeDetails(data);
                        if (success == "")
                        {
                            isOK = false;
                            break;
                        }
                    }
                }
                if (isOK)
                {
                    HelperClass.openInsertSuccessModal(this);
                    BindDetails();
                }
                else
                {
                    HelperClass.openInsertErrorModal(this);
               
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnSave_Click: " + ex.Message);
            }
        }
        protected void lbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["DeleteRowIndex"] = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewDataItem).DataItemIndex);
                string deletemsg = "Are you sure, you want to delete the record";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmModal", "openDeleteConfirmModal('" + deletemsg + "')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lbDelete_Click: " + ex.Message);
            }
        }
        protected void btnDeleteConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                ItemStdCycleTimeDetails data = new ItemStdCycleTimeDetails();
                data.MachineId = (lvDetails.Items[(int)ViewState["DeleteRowIndex"]].FindControl("lblMachineId") as Label).Text;
                data.ItemCode = (lvDetails.Items[(int)ViewState["DeleteRowIndex"]].FindControl("lblItemCode") as Label).Text;
                data.OperationNo = (lvDetails.Items[(int)ViewState["DeleteRowIndex"]].FindControl("hfOperationNo") as HiddenField).Value;
                data.Param = "Delete";
                string result = DataBaseAccess.SaveItemStdCycleTimeDetails(data);
                if (result == "Deleted")
                {

                    HelperClass.openDeleteSuccessModal(this);
                    HelperClass.clearModal(this);
                }
                else if (result.Contains("Cannot"))
                {
                    HelperClass.openWarningModal(this, "Production has happaned for selected record.");
                    return;
                }
                else
                {
                    HelperClass.clearModal(this);
                    HelperClass.openDeleteErrorModal(this);
                    return;
                }
                BindDetails();
                ViewState["DeleteRowIndex"] = -1;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnDeleteConfirm_Click: " + ex.Message);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                List<ItemStdCycleTimeDetails> list = new List<ItemStdCycleTimeDetails>();
                if (Session["ItemStdCycleTimeDetails"]==null)
                {
                    list = DataBaseAccess.GetItemStdCycleTimeDetails(txtFromDate.Text, txtToDate.Text, ddlMachine.SelectedValue, txtItemCode.Text, ddlUpdatedBy.SelectedValue,"View"); ;
                }
                else
                {
                    list = (List<ItemStdCycleTimeDetails>)Session["ItemStdCycleTimeDetails"];
                }
                string Generated = TMPTrakGenerateReport.GenerateItemStdCycleTimeDetails(ViewState["FromDate"].ToString(), ViewState["ToDate"].ToString(), list, true);
                if (Generated.Equals("", StringComparison.OrdinalIgnoreCase))
                {
                    // HelperClass.opene(this, "No Data Found");
                }
                else if (Generated.Equals("NodataFound", StringComparison.OrdinalIgnoreCase))
                {
                    //
                }
                else if (Generated.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                {
                    //HelperClass.ope(this, "No Data Found");
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnExport_Click: " + ex.Message);
            }
        }
    }
}