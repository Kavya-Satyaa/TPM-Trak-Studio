using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.PradeepMetals
{
    public partial class PradeepMetalsReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPlantID();
                BindCellID();
                BindMachineID();
                BindDownCategory();
                trShift.Visible = false;
            }
        }

        private void BindDownCategory()
        {
            try
            {
                List<string> list = DBAccessPradeepMetals.GetDownCategoryList();
                //if (list.Count > 0)
                //{
                //    list.Insert(0, "All");
                //}
                ddlMultiDownCategory.DataSource = list;
                ddlMultiDownCategory.DataBind();
                foreach (ListItem item in ddlMultiDownCategory.Items)
                {
                    item.Selected = true;
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("BindDownCategory: " + ex.Message);
            }
        }
        private void BindPlantID()
        {
            try
            {
                List<string> list = DBAccessPradeepMetals.GetPlantID();
                if (list.Count > 0)
                {
                    list.Insert(0, "Plant All");
                }
                ddlPlantID.DataSource = list;
                ddlPlantID.DataBind();
                
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindCellID()
        {
            try
            {
                List<string> list = new List<string>();
                if (ddlPlantID.SelectedValue.ToString() == "Plant All")
                {
                     list = DBAccessPradeepMetals.GetCellID("");
                }
                else
                {
                    list = DBAccessPradeepMetals.GetCellID(ddlPlantID.SelectedValue);
                }
                //List<string> list = DBAccessPradeepMetals.GetCellID(ddlPlantID.SelectedValue);
                if(list.Count>0)
                {
                    list.Remove("All");
                }
                ddlMultiCellID.DataSource = list;
                ddlMultiCellID.DataBind();
                foreach (ListItem item in ddlMultiCellID.Items)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindMachineID()
        {
            try
            {
                //List<string> list = DBAccessPradeepMetals.GetMachineIDs(ddlCellID.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCellID.SelectedValue);
                //ddlMultiMachineId.DataSource = list;
                //ddlMultiMachineId.DataBind();

                string cellID = ""; 
                cellID = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(ddlMultiCellID);

                List<string> list = DBAccessPradeepMetals.GetMachineIDs(cellID);
                ddlMultiMachineId.DataSource = list;
                ddlMultiMachineId.DataBind();

                foreach (ListItem item in ddlMultiMachineId.Items)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlReportType.SelectedValue.Equals("MaintenanceDowntimePareto", StringComparison.OrdinalIgnoreCase))
            {
                trMaintenanceReporttype.Visible = true;
                trDownCategory.Visible = true;
            }
            else
            {
                trMaintenanceReporttype.Visible = false;
                trDownCategory.Visible = false;
            }

            if (ddlReportType.SelectedValue.ToString().Equals("CNCProductionReport", StringComparison.OrdinalIgnoreCase))
            {
                trShift.Visible = true;
            }
            else
            {
                trShift.Visible = false; 
            }
            
           
        }

        protected void ddlFormat_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellID();
            BindMachineID();
        }

        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineID();
        }
        private void CheckDateDifference(DateTime fromDate, DateTime toDate)
        {
            try
            {
                TimeSpan difference = toDate - fromDate;
                int monthsDifference = difference.Days / 30;
                if (monthsDifference > 12)
                {
                    HelperClassGeneric.openWarningModal(this, "the difference between the dates should not be more than 12 months.");
                    ScriptManager.RegisterStartupScript(txtToDate, txtToDate.GetType(), "FocusScript", "document.getElementById('" + txtToDate.ClientID + "').focus();", true);
                    //Console.WriteLine("Warning: the difference between the dates should not be more than 12 months.");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            string ReportStatus = "Generated";
            try
            {
                if (ddlReportType.SelectedValue.ToString().Equals("MaintenanceDowntimePareto", StringComparison.OrdinalIgnoreCase))
                {
                    DateTime fromDate = Util.GetDateTime(txtFromDate.Text);
                    DateTime toDate = Util.GetDateTime(txtToDate.Text);

                    string machineId = "";
                    foreach (ListItem item in ddlMultiMachineId.Items)
                    {
                        if (item.Selected)
                        {
                            if (machineId == "")
                                machineId += item.Value;
                            else
                                machineId += "," + item.Value;
                        }
                    }

                    string DownCategory = "";
                    foreach (ListItem item in ddlMultiDownCategory.Items)
                    {
                        if (item.Selected)
                        {
                            if (DownCategory == "")
                                DownCategory += item.Value;
                            else
                                DownCategory += "," + item.Value;
                        }
                    }
                    //string DownCategory = ddlDownCategory.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlDownCategory.SelectedValue.Trim();

                    if(ddlMaintenaceReport.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase))
                        ReportStatus = PradeepMetalsGenerateReport.GenerateMaintenanceMachineLevelDowntimePareto(machineId, fromDate, toDate, "MaintenanceMachineLevelDowntimePareto,MaintenanceSubSystemLevelDowntimePareto", DownCategory);
                    else if (ddlMaintenaceReport.SelectedValue.Equals("MaintenanceMachineLevelDowntimePareto", StringComparison.OrdinalIgnoreCase))
                        ReportStatus = PradeepMetalsGenerateReport.GenerateMaintenanceMachineLevelDowntimePareto(machineId, fromDate, toDate, "MaintenanceMachineLevelDowntimePareto", DownCategory);
                    else if (ddlMaintenaceReport.SelectedValue.Equals("MaintenanceSubSystemLevelDowntimePareto", StringComparison.OrdinalIgnoreCase))
                        ReportStatus = PradeepMetalsGenerateReport.GenerateMaintenanceMachineLevelDowntimePareto(machineId, fromDate, toDate, "MaintenanceSubSystemLevelDowntimePareto", DownCategory);
                }

                //else if (ddlReportType.SelectedValue.ToString().Equals("ParetoOverallDowntimeReasonsReport", StringComparison.OrdinalIgnoreCase))
                //{
                //    DateTime fromDate = Util.GetDateTime(txtFromDate.Text);
                //    string year1 = Util.GetDateTime(txtFromDate.Text).ToString("yyyy");
                //    string month1 = Util.GetDateTime(txtFromDate.Text).ToString("MM");
                //    DateTime toDate = Util.GetDateTime(txtToDate.Text);
                //    string year2 = Util.GetDateTime(txtToDate.Text).ToString("yyyy");
                //    string month2 = Util.GetDateTime(txtToDate.Text).ToString("MM");
                //    string machineId = "";
                //    foreach (ListItem item in ddlMultiMachineId.Items)
                //    {
                //        if (item.Selected)
                //        {
                //            if (machineId == "")
                //                machineId += item.Value;
                //            else
                //                machineId += "," + item.Value;
                //        }
                //    }
                //    ReportStatus = PradeepMetalsGenerateReport.GenerateParetoDownTimeReasonsReport(machineId, year1, month1, year2, month2);
                //}

                else if (ddlReportType.SelectedValue.ToString().Equals("OVERALLPMLOEETREND", StringComparison.OrdinalIgnoreCase))
                {
                    string cellID = "";
                    cellID = DBAccessPradeepMetals.getListBoxValueWithSingleQuote(ddlMultiCellID);
                    DateTime fromDate = Util.GetDateTime(txtFromDate.Text.Trim());
                    DateTime toDate = Util.GetDateTime(txtToDate.Text.Trim());
                    string machineId = DBAccessPradeepMetals.getListBoxValueWithSingleQuote(ddlMultiMachineId);
                    //string cellID = DBAccessPradeepMetals.getDropdownValueWithSingleQuote(ddlMultiCellID);
                    ReportStatus = PradeepMetalsGenerateReport.GenerateOverAllOEETrendReport(machineId, fromDate, toDate, cellID);
                }

                else if (ddlReportType.SelectedValue.ToString().Equals("CNCProductionReport", StringComparison.OrdinalIgnoreCase))
                {
                    trShift.Visible = true;
                    string cellID = "";
                    cellID = DBAccessPradeepMetals.getListBoxValueWithSingleQuote(ddlMultiCellID);
                    DateTime fromDate = Util.GetDateTime(txtFromDate.Text);
                    DateTime toDate = Util.GetDateTime(txtToDate.Text);
                    string machineID = DBAccessPradeepMetals.getListBoxValueWithSingleQuote(ddlMultiMachineId);
                   // string cellID = ddlCellID.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : $"'{ddlCellID.SelectedValue.Trim()}'";
                    string Shift = ddlShift.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : $"'{ddlShift.SelectedValue.Trim()}'";
                    ReportStatus = PradeepMetalsGenerateReport.GenerateCNCProductionReport(fromDate, toDate, machineID, cellID, Shift);
                }

                if (ReportStatus.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openSuccessModal(this, "Succesfully Generated");
                }
                else
                {
                    HelperClassGeneric.openErrorModal(this, "Error Generating Report");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void txtToDate_TextChanged(object sender, EventArgs e)
        {
            //CheckDateDifference(Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));
            CheckDateDifference(DateTime.ParseExact(txtFromDate.Text, "dd-MM-yyyy", null), DateTime.ParseExact(txtToDate.Text, "dd-MM-yyyy", null));
        }

        protected void ddlMultiCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineID();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "openlistbox", "stayMultiselectedList('cell');", true);
        }
    }
}