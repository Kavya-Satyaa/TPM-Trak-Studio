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
    public partial class InspectionTransactionVulkan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindShift();
                BindMachines();
                txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindDataGrid();
            }
        }

        private void BindShift()
        {
            try
            {
                List<string> list = new List<string>();
                list = VulkanMSDBAccess.GetShiftDetails();
                ddlShift.DataSource = list;
                ddlShift.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindShift : {ex.Message}");
            }
        }
        private void BindMachines()
        {
            try
            {
                List<string> list = new List<string>();
                list = VulkanMSDBAccess.GetMachineID();
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindShift : {ex.Message}");
            }
        }

        private void BindDataGrid()
        {
            List<InspectionTransactionEntity> list = new List<InspectionTransactionEntity>();
            try
            {
                string result = "";
                list = VulkanMSDBAccess.GetInspectionTransactionDetails(ddlMachine.SelectedValue.Trim(),txtDate.Text,ddlShift.SelectedValue, out result);
                if (!string.IsNullOrEmpty(result))
                    HelperClassGeneric.openWarningToastrModal(this, result);
                lvInspectionApproval.DataSource = list;
                lvInspectionApproval.DataBind();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog($"BindDataGrid: {ex.Message}");
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                BindDataGrid();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog($"btnView_Click: {ex.Message}");
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                string result = VulkanMSDBAccess.SaveApprovalDetails(ddlMachine.SelectedValue, txtDate.Text, ddlShift.SelectedValue);
               if(result.Equals("Inserted", StringComparison.OrdinalIgnoreCase) || result.Equals("Updated", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openSuccessModal(this, "Approved");
                else
                    HelperClassGeneric.openWarningModal(this, string.IsNullOrEmpty(result) ? "ERROR! Try again." : result);
                BindDataGrid();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog($"btnApprove_Click: {ex.Message}");
            }
        }
    }
}