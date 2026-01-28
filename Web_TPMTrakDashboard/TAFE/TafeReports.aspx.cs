using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.TAFE
{
    public partial class TafeReports : System.Web.UI.Page
    {
        public string Generated = "Generated";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                SessionClear.ClearSession();
                LoadPageByReportType();
                BindPlantID();
                ddlReportType_SelectedIndexChanged(null, null);
            }
        }

        private void LoadPageByReportType()
        {
            try
            {
                trToDate.Visible = false;
                trFromDate.Visible = true;
                trLine.Visible = false;
                trPlant.Visible = false;
                trMachine.Visible = true;
                trmonthlydate.Visible = false;
                trfromdatetimeconsolidate.Visible = false;
                trtodatetimeconsolidate.Visible = false;
                trmonthlytodate.Visible = false;
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog("Error!! " + ex.Message);
                throw;
            }
        }

        private void BindPlantID()
        {
            try
            {
                List<string> plantIDs = TafeDataBaseAccess.ViewPlantToDisplay();
                ddlPlantId.DataSource = plantIDs;
                ddlPlantId.DataBind();
                ddlPlantId.SelectedIndex = 0;
                string PlantId = ddlPlantId.SelectedItem != null ? ddlPlantId.SelectedItem.ToString() : "";
                if (!string.IsNullOrEmpty(PlantId))
                {
                    BindCellID(PlantId);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog("Error!! " + ex.Message);
                throw;
            }
        }

        private void BindCellID(string PlantID)
        {
            try
            {
                PlantID = PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID;
                List<string> lineIDs = TafeDataBaseAccess.GetLineIDsForPlant(PlantID);
                ddlLineId.DataSource = lineIDs;
                ddlLineId.DataBind();
                ddlLineId.SelectedIndex = 0;
                string LineId = ddlLineId.SelectedItem != null ? ddlLineId.SelectedItem.ToString() : "";
                if (!string.IsNullOrEmpty(LineId))
                {
                    BindMachineID(PlantID, LineId);
                    BindComponentID(PlantID, LineId);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog("Error!! " + ex.Message);
                throw;
            }
        }

        private void BindMachineID(string plantID, string LineID)
        {
            try
            {
                plantID = "";
                LineID = "";
                List<string> machineIds = TafeDataBaseAccess.GetAllMachinesForPlantAndLine(plantID, LineID);
                ddlMachineId.DataSource = machineIds;
                ddlMachineId.DataBind();
                ddlMachineId.SelectedIndex = 0;
                string MachineID = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedItem.ToString() : "";
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog("Error!! " + ex.Message);
                throw;
            }
        }

        private void BindComponentID(string PlantID, string Line)
        {
            PlantID = PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID;
            Line = Line.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Line;
            List<string> componentid = DataBaseAccess.GetComponentsForMachine(PlantID, Line);
            if (componentid != null && componentid.Count > 0)
            {
                ddlComponent.DataSource = componentid;
                ddlComponent.DataBind();
            }
            else
            {
                ddlComponent.DataSource = new List<string>();
                ddlComponent.DataBind();
            }

        }

        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideEverything();
            if (ddlReportType.SelectedValue.Equals("CategoryWiseOEEAndLossTimeReport", StringComparison.OrdinalIgnoreCase))
            {
                trFromDate.Visible = true;
                trMachine.Visible = true;
            }
            else if (ddlReportType.SelectedValue.Equals("RejectionReport", StringComparison.OrdinalIgnoreCase))
            {
                trPlant.Visible = true;
                trLine.Visible = true;
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trCategory.Visible = true;
            }
            else if (ddlReportType.SelectedValue.Equals("BatchWiseReport", StringComparison.OrdinalIgnoreCase))
            {
                trPlant.Visible = true;
                trLine.Visible = true;
                trmonthlydate.Visible = true;
                trCategory.Visible = true;
                trPartID.Visible = true;
            }
            else if (ddlReportType.SelectedValue.Equals("HoldReport", StringComparison.OrdinalIgnoreCase))
            {
                trPlant.Visible = true;
                trLine.Visible = true;
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trMachine.Visible = true;
            }
            else if (ddlReportType.SelectedValue.Equals("LineMeterReport", StringComparison.OrdinalIgnoreCase))
            {
                trPlant.Visible = true;
                trLine.Visible = true;
                trmonthlydate.Visible = true;
                trMachine.Visible = false;
            }
            else if (ddlReportType.SelectedValue.Equals("PDIReport", StringComparison.OrdinalIgnoreCase))
            {
                trSerialNumber.Visible = true;
                lnkSlnoSearch_Click(null, null);
            }
        }

        private void HideEverything()
        {
            try
            {
                trFromDate.Visible = false;
                trToDate.Visible = false;
                trfromdatetimeconsolidate.Visible = false;
                trtodatetimeconsolidate.Visible = false;
                trPlant.Visible = false;
                trLine.Visible = false;
                trMachine.Visible = false;
                trmonthlytodate.Visible = false;
                trmonthlydate.Visible = false;
                trCategory.Visible = false;
                trPartID.Visible = false;
                trSerialNumber.Visible = false;
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog("Error!! " + ex.Message);
                throw;
            }
        }

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string PlantId = ddlPlantId.SelectedItem != null ? ddlPlantId.SelectedItem.ToString() : "";
                if (!string.IsNullOrEmpty(PlantId))
                    BindCellID(PlantId);
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog("Error!! " + ex.Message);
                throw;
            }
        }

        protected void ddlLineId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string PlantId = ddlPlantId.SelectedItem != null ? ddlPlantId.SelectedItem.ToString() : "";
                string LineId = ddlLineId.SelectedItem != null ? ddlLineId.SelectedItem.ToString() : "";
                if (!string.IsNullOrEmpty(PlantId) && !string.IsNullOrEmpty(LineId))
                {
                    BindMachineID(PlantId, LineId);
                    BindComponentID(PlantId, LineId);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog("Error!! " + ex.Message);
                throw;
            }
        }

        protected void ddlMachineId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string MachineId = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedItem.ToString() : "";

            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog("Error!! " + ex.Message);
                throw;
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            string plantId = ddlPlantId.SelectedValue == "All" ? string.Empty : ddlPlantId.SelectedValue;
            string lineId = ddlLineId.SelectedValue == "All" ? string.Empty : ddlLineId.SelectedValue;
            string machineId = ddlMachineId.SelectedValue == "All" ? string.Empty : ddlMachineId.SelectedValue;
            DateTime fromDate = DateTime.Now.Date;
            DateTime toDate = DateTime.Now.Date;

            string startDate = VDGDataBaseAccess.GetLogicalDayStart(txtFromDate.Text);
            string endDate = VDGDataBaseAccess.GetLogicalDayEnd(txtToDate.Text);
            fromDate = Util.GetDateTime(startDate);
            toDate = Util.GetDateTime(endDate);
            if (ddlReportType.SelectedValue.Equals("HoldReport", StringComparison.OrdinalIgnoreCase))
            {
                Generated = TAFEGenerateReports.GenerateHoldReport(fromDate, toDate, lineId, machineId);
            }
            else if (ddlReportType.SelectedValue.Equals("RejectionReport", StringComparison.OrdinalIgnoreCase))
            {
                Generated = TAFEGenerateReports.RejectionReport(fromDate, toDate, ddlPlantId.SelectedValue, ddlLineId.SelectedValue, ddlcategory.SelectedValue);
            }
            else if (ddlReportType.SelectedValue.Equals("BatchWiseReport", StringComparison.OrdinalIgnoreCase))
            {
                string Date = txtYear.Text + "-" + txtMonth.Text + "-" + "01";
                fromDate = Util.GetDateTime(Date);
                Generated = TAFEGenerateReports.BatchWiseReport(fromDate, ddlPlantId.SelectedValue, ddlLineId.SelectedValue, ddlComponent.SelectedValue, ddlcategory.SelectedValue);
            }
            else if (ddlReportType.SelectedValue.Equals("CategoryWiseOEEAndLossTimeReport", StringComparison.OrdinalIgnoreCase))
            {
                Generated = TAFEGenerateReports.GenerateOEEAndLossTimeReport(fromDate, machineId);
            }
            else if (ddlReportType.SelectedValue.Equals("LineMeterReport", StringComparison.OrdinalIgnoreCase))
            {
                string Date = txtYear.Text + "-" + txtMonth.Text + "-" + "01";
                fromDate = Util.GetDateTime(Date);
                Generated = TAFEGenerateReports.LineMeter(fromDate, ddlLineId.SelectedValue.ToString());
            }
            else if (ddlReportType.SelectedValue.Equals("PDIReport", StringComparison.OrdinalIgnoreCase))
            {

                Generated = Web_TPMTrakDashboard.Models.TMPTrakGenerateReport.PDIReportTafeChennai(ddlSlno.SelectedValue);
            }

            if (Generated.Equals("", StringComparison.OrdinalIgnoreCase))
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);
            else if (Generated.Equals("NoDataFound", StringComparison.OrdinalIgnoreCase))
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNodata", "messageNodata();", true);
            else if (Generated.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "messageOk();", true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LoadPageByReportType();
        }

        protected void lnkSlnoSearch_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> list = new List<string>();
                if (txtSlnoSearch.Text.Trim() == "")
                {
                    list = new List<string>();
                }
                else
                {
                    //list = TafeDataBaseAccess.getSerialNoForPDIReport(txtSlnoSearch.Text.Trim());
                }
                ddlSlno.DataSource = list;
                ddlSlno.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}