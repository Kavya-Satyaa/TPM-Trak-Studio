using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.VulkanMachineShop.Model;

namespace Web_TPMTrakDashboard.VulkanMachineShop
{
    public partial class VulkanPMApprovalDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachineID();
                txtYear.Text = DateTime.Now.Year.ToString();
                txtMonth.Text = DateTime.Now.ToString("MMM");
                BindApprovalGrid();
            }
        }

        private void BindMachineID()
        {
            try
            {
                List<string> list = new List<string>();
                list = VulkanMSDBAccess.GetMachineID();
                ddlMachineID.DataSource = list;
                ddlMachineID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindMachineID: {ex.Message}");
            }
        }

        private void BindApprovalGrid()
        {
            try
            {
                List<PMTransactionEntity> list = new List<PMTransactionEntity>();
                list = VulkanMSDBAccess.GetPMTransactionDetails(ddlMachineID.SelectedValue, txtYear.Text.Trim(), ddlFrequency.SelectedValue.ToString().Equals("monthly", StringComparison.OrdinalIgnoreCase) ? "" : HelperClassGeneric.getMonthNumberFromAbbMonthName(txtMonth.Text.Trim()), ddlFrequency.SelectedValue, out string Message);
                if (Message.Trim().Equals("NoRecordsFound", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openWarningToastrModal(this, "No Data Found");
                lvApprovalGrid.DataSource = list;
                lvApprovalGrid.DataBind();

                if (ddlFrequency.SelectedValue.ToString().Equals("monthly", StringComparison.OrdinalIgnoreCase))
                    txtMonth.Text = "";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindApprovalGrid: {ex.Message}");
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                string WeekNo = ((sender as Button).Parent.FindControl("hdnValue") as HiddenField).Value;
                if (ddlFrequency.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase))
                {
                    if (DateTime.Now.Day < Convert.ToInt32(WeekNo) && Convert.ToInt32(HelperClassGeneric.getMonthNumberFromAbbMonthName(txtMonth.Text)) == DateTime.Now.Month)
                    {
                        HelperClassGeneric.openWarningModal(this, "Approval is allowed only for current or Previous dates");
                        return;
                    }
                }
                Session["WeekNo"] = WeekNo;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ConfirmationModal", "OpenConfirmModal();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"btnApprove_Click: {ex.Message}");
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindApprovalGrid();
        }

        protected void btnApprove_Click1(object sender, EventArgs e)
        {
            try
            {
                string WeekNo = Session["WeekNo"].ToString();
                if(ddlFrequency.SelectedValue.Equals("Monthly", StringComparison.OrdinalIgnoreCase))
                {
                    if (Convert.ToInt32(WeekNo) > DateTime.Now.Month)
                    {
                        HelperClassGeneric.openWarningModal(this, "Approval is not allowed for future months.");
                        return;
                    }
                }
                if (ddlFrequency.SelectedValue.Equals("Daily", StringComparison.OrdinalIgnoreCase) && Convert.ToInt32(HelperClassGeneric.getMonthNumberFromAbbMonthName(txtMonth.Text)) == DateTime.Now.Month)
                {
                    if (Convert.ToInt32(WeekNo) > DateTime.Now.Day)
                    {
                        HelperClassGeneric.openWarningModal(this, "Approval is not allowed for future days.");
                        return;
                    }
                }
                string res = VulkanMSDBAccess.SaveapprovedDetails(ddlMachineID.SelectedValue, txtYear.Text.Trim(), HelperClassGeneric.getMonthNumberFromAbbMonthName(txtMonth.Text.Trim()), ddlFrequency.SelectedValue, WeekNo);

                if (res.Equals("Inserted", StringComparison.OrdinalIgnoreCase) || res.Equals("Updated", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openSuccessModal(this, "Succesfully Approved");
                else if (res.Equals("Invalid WeekNo", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openWarningModal(this, "Approval is not allowed for Future Weeks.");
                else if (res.Equals("Checksheet is not done!", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openWarningModal(this, res);
                else
                    HelperClassGeneric.openWarningToastrModal(this, "Approve failed. Try Again!");
                BindApprovalGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"btnApprove_Click1: {ex.Message}");
            }
        }
    }
}