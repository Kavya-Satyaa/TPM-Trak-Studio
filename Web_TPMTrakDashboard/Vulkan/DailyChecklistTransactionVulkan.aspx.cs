using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Vulkan.Models;

namespace Web_TPMTrakDashboard.Vulkan
{
    public partial class DailyChecklistTransactionVulkan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtYear.Text = DateTime.Now.ToString("yyyy");
                txtMonth.Text = DateTime.Now.ToString("MMM");
                BindPlantID();
                BindMachines();
                BindTransactionData();
            }
        }

        private void BindTransactionData()
        {
            int Year = 0, Month = 0;
            try
            {
                List<DailyCLTransactionEntity> list = new List<DailyCLTransactionEntity>();
                int.TryParse(txtYear.Text.Trim(), out Year);
                int.TryParse(HelperClassGeneric.getMonthNumberFromAbbMonthName(txtMonth.Text.Trim()), out Month);
                list = DataBaseAccessVulkan.GetAMTransctionData(ddlPlantID.SelectedValue, ddlMachineID.SelectedValue, Year, Month);

                if (!(list.Count > 0))
                    HelperClassGeneric.openWarningToastrModal(this, "No Data Found.");
                lvDailyCheckListTransaction.DataSource = list;
                lvDailyCheckListTransaction.DataBind();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("BindTransactionData: " + ex.ToString());
            }
        }

        private void BindPlantID()
        {
            try
            {
                List<string> list = DataBaseAccessVulkan.GetPlantID();

                ddlPlantID.DataSource = list;
                ddlPlantID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindMachines()
        {
            try
            {
                List<string> list = new List<string>();
                list = DataBaseAccessVulkan.GetMachineIDs(ddlPlantID.SelectedValue.Trim());
                ddlMachineID.DataSource = list;
                ddlMachineID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }



        protected void btnView_Click(object sender, EventArgs e)
        {
            BindTransactionData();
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            string result = string.Empty;
            try
            {
                var ApprovedDay = ((sender as Button).Parent.FindControl("hdnDayValue") as HiddenField).Value;
                string MachineID = ddlMachineID.SelectedValue;
                string PlantID = ddlPlantID.SelectedValue;
                string Month = txtMonth.Text.Trim();
                string Year = txtYear.Text.Trim();

                ApprovedDay = ApprovedDay.Length == 1 ? ("0" + ApprovedDay) : ApprovedDay;

                string ApprovedDate = ApprovedDay + "-" + HelperClassGeneric.getMonthNumberFromAbbMonthName(Month) + "-" + Year;
                result = DataBaseAccessVulkan.SubmitApprovedDetail(MachineID, Session["UserName"].ToString(), ApprovedDate);

                if (result.Equals("Approved", StringComparison.OrdinalIgnoreCase)) 
                    HelperClassGeneric.openSuccessModal(this, "Approved Successfuly.");
                else if (result.Equals("NoTransactionFound", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openWarningToastrModal(this, $"No Transaction Found for the Date : {ApprovedDate}");
                else
                    HelperClassGeneric.openWarningToastrModal(this, "Approve Failed! Try Again.");
                BindTransactionData();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        
    }
}