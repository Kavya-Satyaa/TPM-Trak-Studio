using BusinessClassLibrary;
using Elmah;
using ModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Xceed.Document.NET;

namespace Web_TPMTrakDashboard
{

    public partial class AggregatedReports : System.Web.UI.Page
    {
        bool losses = Util.show16losses;
        public string Generated = "Generated";
        public List<UserAccessModel> useAccessData = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                SessionClear.ClearSession();
                if (Session["UserAccessData"] == null)
                    Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"] != null ? Session["UserName"].ToString() : "PCT");
                else
                    useAccessData = Session["UserAccessData"] as List<UserAccessModel>;

                lblNOte.Text = System.Web.Configuration.WebConfigurationManager.AppSettings["AggregatedReportsNote"].ToString();
                PageLoad();
            }
        }

        private void PageLoad()
        {
            trNodeID.Visible = false;
            trBreakDown.Visible = false;
            trSubsystem.Visible = false;
            trview.Visible = false;
            ddlReportType.Items.Add(new ListItem("Production Report - Machinewise", "ProductionReportMachinewise"));
            ddlReportType.Items.Add(new ListItem("Production Report - Componentwise", "ProductionReportComponentwise"));
            ddlReportType.Items.Add(new ListItem("Downtime Report", "DowntimeReport"));
            ddlReportType.Items.Add(new ListItem("Daily Rejection Report", "DailyRejectionReport"));
            ddlReportType.Items.Add(new ListItem("Rejection Analysis Report", "RejectionAnalysisReport"));
            ddlReportType.Items.Add(new ListItem("Comparison Report", "ComparisonReport"));
            if(ConfigurationManager.AppSettings["SeyoonReport"].ToString()=="1")
            {
                ddlReportType.Items.Add(new ListItem("Operator Performance Report - Shiftwise", "OperatorPerformanceReport_Seyoon"));
            }
            if (ConfigurationManager.AppSettings["DantalHydraulicsPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Production Report - Dantal", "ProductionPlantwiseReport_Dantal"));
            }
            if (ConfigurationManager.AppSettings["KunAeroPages"].ToString() == "1")
                ddlReportType.Items.Add(new ListItem("Daily Production Report-KunAero", "DailyProductionReport_KunAero"));
           
            if (ConfigurationManager.AppSettings["MivinPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Mivin First Pass Yield Report", "MivinOEEReport"));
            }
            if (ConfigurationManager.AppSettings["PatelbrassPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Cycle Analysis MonthWise Report", "CycleAnalysis"));
                ddlReportType.Items.Add(new ListItem("Cycle Analysis DayWise Report", "CycleAnalysisDayWise"));
                ddlReportType.Items.Add(new ListItem("Cycle Analysis Report", "CycleAnalysisVariant"));
            }

            if (ConfigurationManager.AppSettings["ShowPMTReport"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("PMT Report", "PMTReport"));
            }
            if (ConfigurationManager.AppSettings["AdvikPages"].ToString().Equals("1"))
            {
                ddlReportType.Items.Add(new ListItem("SAP-OEE Report", "SAPOEEReport"));
            }
            if (ConfigurationManager.AppSettings["JagdevPage"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Jagadeva Week Summary Chart Report", "JagdevWeekSummaryChartReport"));
                ddlReportType.Items.Add(new ListItem("Jagadeva Rejection Analysis Report", "JagdevRejectionAnalysisReport"));
            }
            if (ConfigurationManager.AppSettings["VulkanPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Production And Down Time Report", "VulkanProdandDowntimeReport"));
            }
            if (ConfigurationManager.AppSettings["ConfidentalPage"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Operator Efficiency Report", "OperatorEfficiencyReport"));
            }
            if (ConfigurationManager.AppSettings["AnjaliPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Operator Incentive Report", "OperatorIncentiveReport"));
            }
            if (ConfigurationManager.AppSettings["IndiaNippon"].ToString() == "1")
            {
                ddlType.Items.Add(new ListItem("OEE Daily Report", "OEEDailyReportNippon"));
            }
            if (ConfigurationManager.AppSettings["GEAPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("OEE Trend Report", "OEETrendReport"));
            }
            if (ConfigurationManager.AppSettings["RTPLPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Production Analysis Report", "ProductionAnalysisReportRTPL"));
            }
            if (ConfigurationManager.AppSettings["AutoTechPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Shiftwise DownTime Details", "ShiftwiseDownTimeDetailsAutoTech"));
            }
            if (ConfigurationManager.AppSettings["LnTOdishaPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Spindle Runtime Report", "SpindleRuntimeReportLnTOdisha"));
            }

            if (ConfigurationManager.AppSettings["KTASpindlePages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Operator Production Login & Logout Report", "OperatorProductionLoginLogoutReport"));
                ddlReportType.Items.Add(new ListItem("Component Standard Cycltime Comparison", "ComponentStandardCycltimeComparison"));
            }
            if(ConfigurationManager.AppSettings["GKReport"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Operator Incentive Report", "OperatorIncentiveReportGK"));
            }
            if(ConfigurationManager.AppSettings["SeyoonReport"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Production & Rejection Qty Report", "SeyoonProductionrejectionReportQty"));
            }
            if (ConfigurationManager.AppSettings["DynamicFlowReport"].ToString() == "1")
                ddlReportType.Items.Add(new ListItem("Custom Production Report", "ProductionReportDynamicFlow"));
            if (ConfigurationManager.AppSettings["RNGupta"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Daily Production Report - RNGupta", "DailyProductionReport_RNGupta"));
                ddlReportType.Items.Add(new ListItem("Incentive Report - RNGupta", "IncentiveReport_RNGupta"));
                ddlReportType.Items.Add(new ListItem("Monthly Production Report - RNGupta", "MonthlyProductionReport_RNGupta"));
            }
           
            //trComponentId.Visible = false;
            //trOperationID.Visible = false;
            trType.Visible = false;
            trFormat.Visible = false;
            trCellId.Visible = false;
            trDownId.Visible = true;
            trsubsystemoneorall.Visible = false;
            trmonthlydate.Visible = false;
            trfromdatetimeconsolidate.Visible = false;
            trtodatetimeconsolidate.Visible = false;
            trToDate.Visible = true;
            trFromDate.Visible = true;
            trTimeFormat.Visible = false;
            trCategory.Visible = false;
            trRejectionID.Visible = false;
            trComponentId.Visible = false;
            trOperationID.Visible = false;
            trOperatorName.Visible = false;
            trBreakdownReason.Visible = false;
            trReportFormat.Visible = false;
            ddlReportFormat.Visible = false;
            ddlMultiMachineId.Visible = false;
            trmonthlytodate.Visible = false;
            trDownReason.Visible = true;
            trShiftAll.Visible = false;
            tryearlywithweek.Visible = false;
            trOperatorID.Visible = false;
            trCycleTimeType.Visible = false;
            ddlMultiComponentID.Visible = false;
            ddlMultiOperationID.Visible = false;
            ddlType.Items.FindByValue("Shift").Selected = true;
            ddlTopDownReasons.SelectedValue = "10";
            trShift.Visible = false;
            ddlTopBreakDownreason.SelectedValue = "5";
            bindNode(); DownTimeContent();
            ddlFormat.Items.FindByValue("Format1").Selected = true;
            //ddlType.Items.FindByValue("MachineDownTimeMatrix").Enabled = false;
            ddlFormat.Items.FindByValue("Format1CockpitDataReport").Enabled = false;
            ddlFormat.Items.FindByValue("Format1EXCEL").Enabled = false;
            //ddlType.Items.FindByValue("MachinewiseDownTimeDetails").Enabled = false;
            ddlType.Items.FindByValue("Hour").Enabled = false;
            trEfficiency.Visible = false;
            trMultiOperator.Visible = false;

            if (!useAccessData.Where(ss => ss.Code.Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("ProductionReportMachinewise"));
            if (!useAccessData.Where(ss => ss.Code.Equals("ProductionReportComponentwise", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("ProductionReportComponentwise"));
            if (!useAccessData.Where(ss => ss.Code.Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("DowntimeReport"));
            if (!useAccessData.Where(ss => ss.Code.Equals("ProductionandDowntimeReportMachinewise", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("ProductionAndDowntimeReportMachinewise"));
            if (!useAccessData.Where(ss => ss.Code.Equals("ProductionandDowntimeReportDailyByHour", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("ProductionAndDowntimeReportDailyByHour"));

            BindPlantId();
            BindCellId();
            BindMachineIDListBox();
            BindShiftData();
            BindComponentID();
            BindOperationID();
            BindCategory();
            BindRejectionID();
            ddlReportType_SelectedIndexChanged(null, null);
            // BindWeeksForYear();
            txtYearforWeek.Text = DateTime.Now.ToString("yyyy");
            txtYearforWeek_TextChanged(null, null);
        }

        private void BindOperatorName()
        {
            try
            {
                var allOperation = BindCockpitView.GetOperatorNameDataForPlant(ddlPlantId.SelectedValue != null ? ddlPlantId.SelectedValue.ToString() : "");
                if (allOperation.Count > 0)
                {
                    allOperation.Insert(0, "ALL");
                }
                ddlOperatorName.DataSource = allOperation;
                ddlOperatorName.DataBind();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void BindWeeksForYear()
        {
            try
            {
                List<ListItem> listItems = new List<ListItem>();
                for (int i = 1; i <= 52; i++)
                {
                    listItems.Add(new ListItem
                    {
                        Text = "Week" + i,
                        Value = i.ToString()
                    });
                }
                ddlWeekOfYear.DataSource = listItems;
                ddlWeekOfYear.DataTextField = "Text";
                ddlWeekOfYear.DataValueField = "Value";
                ddlWeekOfYear.DataBind();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        private void BindMachineIDListBox()
        {
            try
            {
                string plantId = ddlPlantId.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantId.SelectedValue;
                string cellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                lbMachineID.DataSource = DataBaseAccess.getMachineIDListForScreen(plantId, cellId, "ReportLive");
                lbMachineID.DataBind();
                foreach (ListItem item in lbMachineID.Items)
                {
                    item.Selected = true;
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ComponentStandardCycltimeComparison", StringComparison.OrdinalIgnoreCase))
                {
                    //if (lbCellID.SelectedIndex == 0)
                    //{
                    //    cellId = "";
                    //}
                    //else
                    //{
                    //    cellId = HelperClassGeneric.getListBoxValueWithCommaSeparator(lbCellID);
                    //}
                    cellId = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbCellID);
                    List<string> Machines = BindCockpitView.GetMachinesByCell(cellId);
                    ddlMultiMachineId.DataSource = Machines;
                    ddlMultiMachineId.DataBind();
                }
                if(ddlReportType.SelectedValue.ToString().Equals("ProductionPlantwiseReport_Dantal", StringComparison.OrdinalIgnoreCase))
                {
                    ddlMultiMachineId.DataSource = DataBaseAccess.getMachineIDListForScreen(plantId, cellId, "ReportLive");
                    ddlMultiMachineId.DataBind();
                    foreach (ListItem item in ddlMultiMachineId.Items)
                    {
                        item.Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachineIDListBox = " + ex.Message);
            }
        }
        private bool isMachineMultiSelect()
        {
            bool isMultiSelect = false;
            if ((ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && ((ddlType.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase) && ddlReportFormat.SelectedValue.ToString().Equals("Format1", StringComparison.OrdinalIgnoreCase)) || (ddlType.SelectedValue.ToString().Equals("Shift", StringComparison.OrdinalIgnoreCase) && ddlReportFormat.SelectedValue.ToString().Equals("Format1", StringComparison.OrdinalIgnoreCase)) || (ddlType.SelectedValue.ToString().Equals("Time-Consolidated", StringComparison.OrdinalIgnoreCase)))) || ddlReportType.SelectedValue.ToString().Equals("ComparisonReport", StringComparison.OrdinalIgnoreCase) ||
              (ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase) && (ddlFormat.SelectedValue.ToString().Equals("MachinewiseDownTimeDetails", StringComparison.OrdinalIgnoreCase) || ddlFormat.SelectedValue.ToString().Equals("TimeWise", StringComparison.OrdinalIgnoreCase) || ddlFormat.SelectedValue.ToString().Equals("TimeAndFreqWise", StringComparison.OrdinalIgnoreCase)))

              || ddlReportType.SelectedValue.ToString().Equals("RejectionAnalysisReport", StringComparison.OrdinalIgnoreCase)
              || ddlReportType.SelectedValue.ToString().Equals("DailyRejectionReport", StringComparison.OrdinalIgnoreCase)
               || ddlReportType.SelectedValue.ToString().Equals("ProductionAnalysisReportRTPL", StringComparison.OrdinalIgnoreCase)
              || ddlReportType.SelectedValue.ToString().Equals("ShiftwiseDownTimeDetailsAutoTech", StringComparison.OrdinalIgnoreCase)
              || ddlReportType.SelectedValue.ToString().Equals("SpindleRuntimeReportLnTOdisha", StringComparison.OrdinalIgnoreCase)
              || ddlReportType.SelectedValue.ToString().Equals("OperatorProductionLoginLogoutReport", StringComparison.OrdinalIgnoreCase)
              || ddlReportType.SelectedValue.ToString().Equals("ComponentStandardCycltimeComparison", StringComparison.OrdinalIgnoreCase)
              || ddlReportType.SelectedValue.ToString().Equals("SeyoonProductionrejectionReportQty", StringComparison.OrdinalIgnoreCase)
              ||ddlReportType.SelectedValue.ToString().Equals("DailyProductionReport_KunAero", StringComparison.OrdinalIgnoreCase)
              ||ddlReportType.SelectedValue.ToString().Equals("ProductionReportDynamicFlow", StringComparison.OrdinalIgnoreCase)
              ||ddlReportType.SelectedValue.ToString().Equals("DailyProductionReport_RNGupta",StringComparison.OrdinalIgnoreCase)
              || ddlReportType.SelectedValue.ToString().Equals("IncentiveReport_RNGupta",StringComparison.OrdinalIgnoreCase)
              || ddlReportType.SelectedValue.ToString().Equals("ProductionPlantwiseReport_Dantal", StringComparison.OrdinalIgnoreCase)
              || ddlReportType.SelectedValue.ToString().Equals("OperatorPerformanceReport_Seyoon",StringComparison.OrdinalIgnoreCase)
              )
            {
                isMultiSelect = true;
            }
            return isMultiSelect;
        }
        #region ---Rejection Analysis Report ----
        private void BindComponentID()
        {
            try
            {
                var allComponent = BindCockpitView.GetAllComponent();
                if (allComponent != null && allComponent.Count > 0)
                {
                    ddlComponentId.DataSource = allComponent;
                    ddlComponentId.DataBind();
                    ddlMultiComponentID.DataSource = allComponent;
                    ddlMultiComponentID.DataBind();
                    

                    if (ConfigurationManager.AppSettings["JagdevPage"].ToString() == "1" && ddlReportType.SelectedValue == "JagdevWeekSummaryChartReport")
                    {
                    }
                    else
                    {
                        //    if (ddlComponentId.Items.FindByValue("ALL") == null)
                        //    {
                        ddlComponentId.Items.Insert(0, new ListItem
                        {
                            Text = GetGlobalResourceObject("CommanResource", "ComponentAll").ToString(),
                            Value = "ALL"
                        });
                        //    }

                    }
                    if (ddlReportType.SelectedValue.ToString().Equals("ComponentStandardCycltimeComparison", StringComparison.OrdinalIgnoreCase))
                    {
                        string machines = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMachineID);
                        //if (lbMachineID.SelectedValue == "Select All")
                        //{
                        //    machines = "";
                        //}
                        //else
                        //{
                        //    machines = HelperClassGeneric.getListBoxValueWithCommaSeparator(lbMachineID);
                        //}
                        allComponent = BindCockpitView.GetComponentsByMachine(machines);
                        ddlMultiComponentID.DataSource = allComponent;
                        ddlMultiComponentID.DataBind();
                        foreach (ListItem item in ddlMultiComponentID.Items)
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        private void BindOperationID()
        {
            try
            {
                var allOperation = BindCockpitView.GetAllOperationData(ddlComponentId.SelectedValue != null ? ddlComponentId.SelectedValue.ToString() : "");
                if (allOperation != null && allOperation.Count > 0)
                {
                    ddlOperationID.DataSource = allOperation;
                    ddlOperationID.DataBind();
                    ddlMultiOperationID.DataSource = allOperation;
                    ddlMultiOperationID.DataBind();
                    foreach (ListItem item in ddlMultiOperationID.Items)
                    {
                        item.Selected = true;
                    }
                    ddlOperationID.Items.Insert(0, new ListItem
                    {
                        Text = "Operation All",
                        Value = "ALL"
                    });
                    if (ddlReportType.SelectedValue.ToString().Equals("ComponentStandardCycltimeComparison", StringComparison.OrdinalIgnoreCase))
                    {
                        string components = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(ddlMultiComponentID);
                        //if (ddlMultiComponentID.SelectedIndex == 0)
                        //{
                        //    components = "";
                        //}
                        //else
                        //{
                        //    components = HelperClassGeneric.getListBoxValueWithCommaSeparator(ddlMultiComponentID);
                        //}
                        allOperation = BindCockpitView.GetOperationsByComponent(ddlMultiComponentID.SelectedValue);
                        ddlMultiOperationID.DataSource = allOperation;
                        ddlMultiOperationID.DataBind();
                        foreach(ListItem item in ddlMultiOperationID.Items)
                        {
                            item.Selected = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        private void BindCategory()
        {
            try
            {
                var allCategory = BindCockpitView.GetAllCategory();
                if (allCategory != null && allCategory.Count > 0)
                {
                    ddlCatogery.DataSource = allCategory;
                    ddlCatogery.DataBind();
                    ddlCatogery.Items.Insert(0, new ListItem
                    {
                        Text = "Category All",
                        Value = "ALL"
                    });

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        private void BindRejectionID()
        {
            try
            {
                var allRejection = BindCockpitView.GetAllRejectionData(ddlCatogery.SelectedValue != null ? ddlCatogery.SelectedValue.ToString() : "");
                if (allRejection != null && allRejection.Count > 0)
                {
                    ddlMultiRejectionID.DataSource = allRejection;
                    ddlMultiRejectionID.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        private void BindOperatorID()
        {
            try
            {
                var allOperation = BindCockpitView.GetOperatorDataForPlant(ddlPlantId.SelectedValue != null ? ddlPlantId.SelectedValue.ToString() : "");
                if (allOperation.Count > 0)
                {
                    if (ddlReportType.SelectedValue.ToString().Equals("OperatorIncentiveReport", StringComparison.OrdinalIgnoreCase) || (ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.Equals("DownTimeFormat3", StringComparison.OrdinalIgnoreCase)) || ddlReportType.SelectedValue.ToString().Equals("DailyRejectionReport", StringComparison.OrdinalIgnoreCase)
                        || ddlReportType.SelectedValue.ToString().Equals("OperatorPerformanceReport_Seyoon",StringComparison.OrdinalIgnoreCase))
                    {
                        allOperation.Insert(0, "ALL");
                    }
                }
                ddlOperatorID.DataSource = allOperation;
                ddlOperatorID.DataBind();

                ddlMultiOperator.DataSource = allOperation;
                ddlMultiOperator.DataBind();
                foreach (ListItem item in ddlMultiOperator.Items)
                {
                    item.Selected = true;
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        protected void ddlComponentId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOperationID();
        }
        protected void ddlCatogery_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRejectionID();
        }
        #endregion

        #region binding
        private void BindShiftData()
        {
            try
            {
                var allShift = BindCockpitView.GetAllShift();
                if (allShift != null && allShift.Count > 0)
                {
                    ddlShift.DataSource = allShift;
                    ddlShift.DataBind();
                    ddlShift.Items.Insert(0, new ListItem
                    {
                        Text = GetGlobalResourceObject("CommanResource", "ShiftAll").ToString(),
                        Value = "All"
                    });
                    drpShiftAll.DataSource = allShift;
                    drpShiftAll.DataBind();
                    drpShiftAll.Items.Insert(0, new ListItem
                    {
                        Text = GetGlobalResourceObject("CommanResource", "ShiftAll").ToString(),
                        Value = "All"
                    });
                    lbShiftMultiSelect.DataSource = allShift;
                    lbShiftMultiSelect.DataBind();
                    foreach (ListItem item in lbShiftMultiSelect.Items)
                    {
                        item.Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void BindPlantId()
        {
            try
            {
                List<string> lstPlantData = BindCockpitView.ViewPlantToDisplay();
                ddlPlantId.DataSource = lstPlantData;
                ddlPlantId.DataBind();
                if (!ddlReportType.SelectedValue.ToString().Equals("MivinOEEReport", StringComparison.OrdinalIgnoreCase) && !ddlReportType.SelectedValue.ToString().Equals("ProductionAnalysisReportRTPL", StringComparison.OrdinalIgnoreCase))
                {
                    if (!ddlType.SelectedValue.ToString().Equals("Hour", StringComparison.OrdinalIgnoreCase) ||
                    ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportDailyByHour", StringComparison.OrdinalIgnoreCase)
                    || ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.ToString().Equals("OperatorIncentiveReport", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.ToString().Equals("OEETrendReport", StringComparison.OrdinalIgnoreCase))
                    {
                        ddlPlantId.Items.Insert(0, new ListItem
                        {
                            Text = GetGlobalResourceObject("CommanResource", "PlantAll").ToString(),
                            Value = "ALL"
                        });
                    }
                    ddlPlantId_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void BindCellId()
        {
            try
            {
                string plantid = (ddlPlantId.SelectedItem == null || ddlPlantId.SelectedItem.Text.Contains("All") || ddlPlantId.SelectedItem.Text.Contains("ALL")) ? "" : ddlPlantId.SelectedItem.Text;
                List<string> lstCellId = BindCockpitView.ViewCellsToDisplay(plantid);
                ddlCellID.DataSource = lstCellId;
                ddlCellID.DataBind();
                lbCellID.DataSource = lstCellId;
                lbCellID.DataBind();
                foreach (ListItem item in lbCellID.Items)
                {
                    item.Selected = true;
                }
                ddlCellID.Items.Insert(0, new ListItem
                {
                    Text = GetGlobalResourceObject("CommanResource", "CellAll").ToString(),
                    Value = "All"
                });

                BindMachinesForPlantCell();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        private void BindMachinesForPlantCell()
        {
            try
            {
                string plant = ddlPlantId.SelectedValue == null ? "" : ddlPlantId.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantId.SelectedValue.ToString();
                string Cell = ddlCellID.SelectedValue == null ? "" : ddlCellID.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCellID.SelectedValue.ToString();
                List<string> lstMachineId = new List<string>();
                ddlMachineId.Items.Clear();// = null;
                var allMachineName = VDGDataBaseAccess.GetMachinesbyPlantCell(plant, Cell);// AccessReportData.GetAllMachines(ddlPlantId.SelectedItem.ToString());
                if (allMachineName != null && allMachineName.Count > 0)
                {
                    if (ddlReportType.SelectedValue.ToString().Equals("OEETrendReport", StringComparison.OrdinalIgnoreCase))
                    {
                        ddlMultiMachineId.DataSource = allMachineName;
                        ddlMultiMachineId.DataBind();
                    }

                    allMachineName.Insert(0, "ALL");
                    ddlMachineId.DataSource = allMachineName;
                    ddlMachineId.DataBind();
                    //if (ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase))
                    //{
                    // ddlMultiMachineId.Visible = true;
                    // ddlMachineId.Visible = false;
                    // ddlMultiMachineId.DataSource = allMachineName;
                    // ddlMultiMachineId.DataBind();
                    //}
                    //if (!ddlType.SelectedValue.ToString().Equals("Hour", StringComparison.OrdinalIgnoreCase) ||
                    // ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportDailyByHour", StringComparison.OrdinalIgnoreCase) ||
                    // ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase))
                    // ddlMachineId.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommanResource", "MachineAll").ToString(), "ALL"));
                }
                

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        private void bindNode()
        {
            if (ddlReportType.SelectedValue.ToString().Equals("EnergyReport", StringComparison.OrdinalIgnoreCase))
            {
                string Machine = ddlMachineId.SelectedValue.ToString() == "ALL" ? "" : ddlMachineId.SelectedValue.ToString();
                List<string> BindNodes = DataBaseAccess.GetAllNodesForMachine(Machine);
                if (BindNodes != null && BindNodes.Count > 0)
                {
                    BindNodes.Insert(0, "ALL");
                    ddlNodeid.DataSource = BindNodes;
                    ddlNodeid.DataBind();
                    trNodeID.Visible = true;
                }
                else
                {
                    trNodeID.Visible = false;
                }
            }
        }

        private void BindMachines()
        {
            try
            {
                ddlMachineId.Items.Clear();// = null;
                var allMachineName = VDGDataBaseAccess.GetAllMachines(ddlPlantId.SelectedValue.ToString());// AccessReportData.GetAllMachines(ddlPlantId.SelectedItem.ToString());
                if (allMachineName != null && allMachineName.Count > 0)
                {
                    allMachineName.Insert(0, "ALL");
                    ddlMachineId.DataSource = allMachineName;
                    ddlMachineId.DataBind();

                    allMachineName.Remove("ALL");
                    ddlMultiMachineId.DataSource = allMachineName;
                    ddlMultiMachineId.DataBind();

                    //if (ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase))
                    //{
                    //	ddlMultiMachineId.Visible = true;
                    //	ddlMachineId.Visible = false;
                    //	ddlMultiMachineId.DataSource = allMachineName;
                    //	ddlMultiMachineId.DataBind();
                    //}
                    //if (!ddlType.SelectedValue.ToString().Equals("Hour", StringComparison.OrdinalIgnoreCase) ||
                    //	ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportDailyByHour", StringComparison.OrdinalIgnoreCase) ||
                    //	ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase))
                    //	ddlMachineId.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommanResource", "MachineAll").ToString(), "ALL"));
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region Selection change events
        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindPlantId();
            trShiftMultiSelect.Visible = false;
            trToDate.Visible = false;
            trShift.Visible = false;
            trmonthlydate.Visible = false;
            trType.Visible = false;
            trNodeID.Visible = false;
            trFormat.Visible = false;
            trPlant.Visible = false;
            trCellId.Visible = false;
            ddlCellID.Visible = true;
            trMachine.Visible = false;
            trview.Visible = false;
            //trComponentId.Visible = false;
            //trOperationID.Visible = false;
            trDownId.Visible = false;
            trsubsystemoneorall.Visible = false;
            trTimeFormat.Visible = false;
            trReportFormat.Visible = false;
            ddlReportFormat.Visible = false;
            trShiftAll.Visible = false;
            ddlMachineId.Visible = true;
            lbMachineID.Visible = false;
            ddlMultiMachineId.Visible = false;
            trDownReason.Visible = false;
            trfromdatetimeconsolidate.Visible = false;
            trtodatetimeconsolidate.Visible = false;
            trToDate.Visible = false;
            trFromDate.Visible = false;
            ddlType.SelectedValue = "Shift";
            trShift.Visible = false;
            trCategory.Visible = false;
            trRejectionID.Visible = false;
            trComponentId.Visible = false;
            trOperationID.Visible = false;
            trBreakDown.Visible = false;
            trDownReason.Visible = false;
            trmonthlytodate.Visible = false;
            trSubsystem.Visible = false;
            trBreakdownReason.Visible = false;
            tryearlywithweek.Visible = false;
            trOperatorID.Visible = false;
            trCycleTimeType.Visible = false;
            trOperatorName.Visible = false;
            //ddlType.Items.FindByValue("MachineDownTimeMatrix").Enabled = false;
            //ddlType.Items.FindByValue("MachinewiseDownTimeDetails").Enabled = false;
            //ddlType.Items.FindByValue("ProdAndDownPie").Enabled = false;
            ddlType.Items.FindByValue("Hour").Enabled = false;
            ddlType.Items.FindByValue("Shift").Enabled = false;
            ddlType.Items.FindByValue("Daily").Enabled = false;
            ddlType.Items.FindByValue("Month").Enabled = false;
            ddlType.Items.FindByValue("Time-Consolidated").Enabled = false;
            ddlType.Items.FindByValue("OEEReportKiswok").Enabled = false;
            ddlFormat.Items.FindByValue("MachinewiseDownTimeDetails").Enabled = false;
            ddlFormat.Items.FindByValue("TimeWise").Enabled = false;
            ddlFormat.Items.FindByValue("TimeAndFreqWise").Enabled = false;
            ddlFormat.Items.FindByValue("DownTimeFormat3").Enabled = false;
            ddlReportFormat.Items.FindByValue("Format2").Enabled = false;
            trEfficiency.Visible = false;
            trMultiOperator.Visible = false;
            if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase))
            {
                ddlMachineId.Visible = true;
                trType.Visible = true;
                ddlType.Items.FindByValue("Shift").Enabled = true;
                ddlType.Items.FindByValue("Daily").Enabled = true;
                ddlType.Items.FindByValue("Time-Consolidated").Enabled = true;
                if (ConfigurationManager.AppSettings["AggProductionMachinewiseOEEReport"].ToString() == "1")
                {
                    ddlType.Items.FindByValue("OEEReportKiswok").Enabled = true;
                }
                ddlMultiMachineId.Visible = false;
                trReportFormat.Visible = true;
                ddlReportFormat.Visible = true;
                if (ddlType.SelectedValue.ToString().Equals("Time-Consolidated", StringComparison.OrdinalIgnoreCase))
                {
                    trfromdatetimeconsolidate.Visible = true;
                    trtodatetimeconsolidate.Visible = true;
                    trToDate.Visible = false;
                    trFromDate.Visible = false;
                }
                else if (ddlType.SelectedValue.ToString().Equals("Shift", StringComparison.OrdinalIgnoreCase))
                {
                    trToDate.Visible = true;
                    trFromDate.Visible = true;
                    trPlant.Visible = true;
                    trMachine.Visible = true;
                    trShift.Visible = true;
                   ddlShift.Visible = true;
                    //ddlShift.Items.FindByValue("All").Enabled = false;
                }
                else if (ddlType.SelectedValue.ToString().Equals("OEEReportKiswok", StringComparison.OrdinalIgnoreCase))
                {
                    trFormat.Visible = false;
                    trReportFormat.Visible = false;
                    ddlReportFormat.Visible = false;
                    trShift.Visible = false;
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trfromdatetimeconsolidate.Visible = false;
                    trtodatetimeconsolidate.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    ddlMultiMachineId.Visible = false;
                    trCellId.Visible = true;
                    trPlant.Visible = true;
                }
                else if (ddlType.SelectedValue.ToString().Equals("OEEDailyReportNippon", StringComparison.OrdinalIgnoreCase))
                {
                    trFormat.Visible = false;
                    trReportFormat.Visible = false;
                    ddlReportFormat.Visible = false;
                    trShift.Visible = false;
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trfromdatetimeconsolidate.Visible = false;
                    trtodatetimeconsolidate.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    ddlMultiMachineId.Visible = false;
                    trCellId.Visible = true;
                    trPlant.Visible = true;
                }
                else
                {
                    trToDate.Visible = true;
                    trFromDate.Visible = true;
                    trPlant.Visible = true;
                    trMachine.Visible = true;
                    trShift.Visible = false;
                }
                if (isMachineMultiSelect())
                {
                    ddlMachineId.Visible = false;
                    lbMachineID.Visible = true;
                    trCellId.Visible = true;
                    ddlCellID.Visible = false;
                    lbCellID.Visible = true;
                }
                else
                {
                    ddlMachineId.Visible = true;
                    lbMachineID.Visible = false;
                }
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("ComparisonReport", StringComparison.OrdinalIgnoreCase))
            {
                trType.Visible = true;
                trToDate.Visible = false;
                trFromDate.Visible = false;
                trPlant.Visible = true;
                trMachine.Visible = true;
                trmonthlydate.Visible = true;
                ddlType.Items.FindByValue("Daily").Enabled = true;
                ddlType.Items.FindByValue("Month").Enabled = true;
                ddlType.SelectedValue = "Daily";
                if (isMachineMultiSelect())
                {
                    ddlMachineId.Visible = false;
                    lbMachineID.Visible = true;

                    trCellId.Visible = true;
                    ddlCellID.Visible = false;
                    lbCellID.Visible = true;
                }
                else
                {
                    ddlMachineId.Visible = true;
                    lbMachineID.Visible = false;
                }
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase))
            {
                trFormat.Visible = true;
                trToDate.Visible = true;
                trFromDate.Visible = true;
                trPlant.Visible = true;
                trMachine.Visible = true;
                ddlFormat.Items.FindByValue("Format1").Enabled = false;
                ddlFormat.Items.FindByValue("OEEGraphicalReport").Enabled = false;
                ddlFormat.Items.FindByValue("Format3").Enabled = false;
                ddlFormat.Items.FindByValue("MachinewiseDownTimeDetails").Enabled = true;
                ddlFormat.Items.FindByValue("TimeWise").Enabled = true;
                ddlFormat.Items.FindByValue("TimeAndFreqWise").Enabled = true;
                ddlFormat.SelectedValue = "MachinewiseDownTimeDetails";
                //if (ConfigurationManager.AppSettings["ConfidentalPage"].ToString() == "1")
                //{
                ddlFormat.Items.FindByValue("DownTimeFormat3").Enabled = true;
                if (ddlFormat.SelectedValue.ToString().Equals("DownTimeFormat3", StringComparison.OrdinalIgnoreCase))
                {
                    trOperatorID.Visible = true;
                    BindOperatorID();
                }
                // }
                DownTimeContent();
                ddlMachineId.Visible = true;
                ddlMultiMachineId.Visible = false;
                trDownReason.Visible = false;
                trDownReason.Visible = true;
                trShift.Visible = false;
                trBreakdownReason.Visible = false;
                if (isMachineMultiSelect())
                {
                    ddlMachineId.Visible = false;
                    lbMachineID.Visible = true;
                    trCellId.Visible = true;
                    ddlCellID.Visible = false;
                    lbCellID.Visible = true;
                }
                else
                {
                    ddlMachineId.Visible = true;
                    lbMachineID.Visible = false;
                }
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("EnergyReport", StringComparison.OrdinalIgnoreCase))
            {
                trview.Visible = true;
                trType.Visible = true;
                trFormat.Visible = false;
                trFromDate.Visible = true;
                bindNode();
                ddlType.Items.FindByValue("Shift").Enabled = true;
                ddlType.Items.FindByValue("Daily").Enabled = true;
                ddlType.Items.FindByValue("Time-Consolidated").Enabled = true;
                trToDate.Visible = true;
                trPlant.Visible = true;
                trMachine.Visible = true;
                trShift.Visible = false;
                //trComponentId.Visible = false;
                //trOperationID.Visible = false;
                trDownId.Visible = false;
                trDownReason.Visible = false;
                trNodeID.Visible = true;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("PMTReport", StringComparison.OrdinalIgnoreCase))
            {
                ddlType.Visible = false;
                trType.Visible = false;
                trShift.Visible = false;
                trFormat.Visible = false;
                trToDate.Visible = false;
                trFromDate.Visible = false;
                trDownId.Visible = false;
                trDownReason.Visible = false;
                trTimeFormat.Visible = false;
                trPlant.Visible = false;
                trMachine.Visible = false;
                trmonthlydate.Visible = true;
            }

            else if (ddlReportType.SelectedValue.ToString().Equals("RejectionAnalysisReport", StringComparison.OrdinalIgnoreCase))
            {
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trPlant.Visible = true;
                trMachine.Visible = true;
                trComponentId.Visible = true;
                trOperationID.Visible = true;
                trCategory.Visible = true;
                trRejectionID.Visible = true;
                if (isMachineMultiSelect())
                {
                    ddlMachineId.Visible = false;
                    lbMachineID.Visible = true;
                    trCellId.Visible = true;
                    ddlCellID.Visible = false;
                    lbCellID.Visible = true;
                }
                else
                {
                    ddlMachineId.Visible = true;
                    lbMachineID.Visible = false;
                }
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("CycleAnalysis", StringComparison.OrdinalIgnoreCase))
            {
                trview.Visible = true;
                //ddlview.Items.Clear();
                //ddlview.Items.Add(new ListItem("Cycle Time", "CycleTime"));
                //ddlview.Items.Add(new ListItem("Machining Time", "MachiningTime"));
                //ddlview.Items.Add(new ListItem("LoadUnload Time", "LoadUnLoadTime"));

                var abc = ddlview.SelectedValue.ToString();
                trMachine.Visible = true;
                trFromDate.Visible = false;
                trToDate.Visible = false;
                trComponentId.Visible = true;
                trOperationID.Visible = true;
                trmonthlydate.Visible = true;
                txtMonth.Visible = false;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("CycleAnalysisDayWise", StringComparison.OrdinalIgnoreCase))
            {
                trview.Visible = true;
                //ddlview.Items.Clear();
                //ddlview.Items.Add(new ListItem("Cycle Time", "CycleTime"));
                //ddlview.Items.Add(new ListItem("Machining Time", "MachiningTime"));
                //ddlview.Items.Add(new ListItem("LoadUnload Time", "LoadUnLoadTime"));

                trMachine.Visible = true;
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trComponentId.Visible = true;
                trOperationID.Visible = true;
                trmonthlydate.Visible = false;
                txtMonth.Visible = false;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("CycleAnalysisVariant", StringComparison.OrdinalIgnoreCase))
            {
                trview.Visible = false;
                trMachine.Visible = true;
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trComponentId.Visible = true;
                trOperationID.Visible = true;
                trmonthlydate.Visible = false;
                txtMonth.Visible = false;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("SAPOEEReport", StringComparison.OrdinalIgnoreCase))
            {
                trPlant.Visible = true;
                trCellId.Visible = true;
                trMachine.Visible = true;
                trShift.Visible = true;
                trFromDate.Visible = true;
                trToDate.Visible = true;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("JagdevRejectionAnalysisReport", StringComparison.OrdinalIgnoreCase))
            {
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trPlant.Visible = true;
                trMachine.Visible = true;
                trComponentId.Visible = true;
                trOperationID.Visible = true;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("JagdevWeekSummaryChartReport", StringComparison.OrdinalIgnoreCase))
            {
                //trPlant.Visible = true;
                //trMachine.Visible = true;
                trComponentId.Visible = true;
                tryearlywithweek.Visible = true;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("MivinOEEReport", StringComparison.OrdinalIgnoreCase))
            {
                //trPlant.Visible = true;
                //trMachine.Visible = true;
                trPlant.Visible = true;
                trmonthlydate.Visible = true;
                BindPlantId();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("VulkanProdandDowntimeReport", StringComparison.OrdinalIgnoreCase))
            {
                //trPlant.Visible = true;
                trShiftAll.Visible = true;
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trPlant.Visible = true;
                trMachine.Visible = true;
                trCellId.Visible = true;
                BindPlantId();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("OperatorEfficiencyReport", StringComparison.OrdinalIgnoreCase))
            {
                trToDate.Visible = true;
                trFromDate.Visible = true;
                trPlant.Visible = true;
                trOperatorID.Visible = true;
                BindPlantId();
                BindOperatorID();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("OperatorIncentiveReport", StringComparison.OrdinalIgnoreCase))
            {
                trCellId.Visible = true;
                trToDate.Visible = true;
                trFromDate.Visible = true;
                trMachine.Visible = true;
                trOperatorID.Visible = true;
                trPlant.Visible = true;
                trCycleTimeType.Visible = true;
                BindPlantId();
                BindOperatorID();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("OEETrendReport", StringComparison.OrdinalIgnoreCase))
            {
                trCellId.Visible = true;
                trFormat.Visible = false;
                trToDate.Visible = true;
                trFromDate.Visible = true;
                trPlant.Visible = true;
                trMachine.Visible = true;
                ddlMachineId.Visible = false;
                ddlMultiMachineId.Visible = true;
                trShift.Visible = true;
                ddlShift.Visible = true;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("DailyRejectionReport", StringComparison.OrdinalIgnoreCase))
            {
                trToDate.Visible = true;
                trFromDate.Visible = true;
                trMachine.Visible = true;
                trOperatorID.Visible = true;
                trOperatorName.Visible = false;
                trPlant.Visible = true;
                trComponentId.Visible = true;
                trCycleTimeType.Visible = false;
                BindPlantId();
                BindOperatorID();

                if (isMachineMultiSelect())
                {
                    ddlMachineId.Visible = false;
                    lbMachineID.Visible = true;
                    trCellId.Visible = true;
                    ddlCellID.Visible = false;
                    lbCellID.Visible = true;
                }
                else
                {
                    ddlMachineId.Visible = true;
                    lbMachineID.Visible = false;
                }
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("ProductionAnalysisReportRTPL", StringComparison.OrdinalIgnoreCase))
            {
                trToDate.Visible = true;
                trFromDate.Visible = true;
                trPlant.Visible = true;
                trMachine.Visible = true;
                trShiftMultiSelect.Visible = true;
                BindPlantId();
                ddlPlantId_SelectedIndexChanged(null, null);
                if (isMachineMultiSelect())
                {
                    ddlMachineId.Visible = false;
                    lbMachineID.Visible = true;
                    trCellId.Visible = true;
                    ddlCellID.Visible = false;
                    lbCellID.Visible = true;
                }
                else
                {
                    ddlMachineId.Visible = true;
                    lbMachineID.Visible = false;
                }
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("ShiftwiseDownTimeDetailsAutoTech", StringComparison.OrdinalIgnoreCase))
            {

                trToDate.Visible = true;
                trFromDate.Visible = true;
                trPlant.Visible = true;
                trMachine.Visible = true;
                ddlMachineId.Visible = true;
                ddlMultiMachineId.Visible = false;
                if (isMachineMultiSelect())
                {
                    ddlMachineId.Visible = false;
                    lbMachineID.Visible = true;
                    trCellId.Visible = true;
                    ddlCellID.Visible = false;
                    lbCellID.Visible = true;
                }
                else
                {
                    ddlMachineId.Visible = true;
                    lbMachineID.Visible = false;
                }
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("SpindleRuntimeReportLnTOdisha", StringComparison.OrdinalIgnoreCase))
            {
                ddlMachineId.Visible = true;
                ddlMultiMachineId.Visible = false;
                trToDate.Visible = true;
                trFromDate.Visible = true;
                trPlant.Visible = true;
                trMachine.Visible = true;
                trShift.Visible = true;
                if (isMachineMultiSelect())
                {
                    ddlMachineId.Visible = false;
                    lbMachineID.Visible = true;
                    trCellId.Visible = true;
                    ddlCellID.Visible = false;
                    lbCellID.Visible = true;
                }
                else
                {
                    ddlMachineId.Visible = true;
                    lbMachineID.Visible = false;
                }
            }
            else if(ddlReportType.SelectedValue.ToString().Equals("OperatorProductionLoginLogoutReport", StringComparison.OrdinalIgnoreCase))
            {
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trPlant.Visible = true;
                trCellId.Visible = true;
                trMachine.Visible = true;
                trShiftAll.Visible = true;
                if (isMachineMultiSelect())
                {
                    ddlMachineId.Visible = false;
                    lbMachineID.Visible = true;
                }
                else
                {
                    ddlMachineId.Visible = true;
                    lbMachineID.Visible = false;
                }
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("OperatorIncentiveReportGK", StringComparison.OrdinalIgnoreCase))
            {
                BindOperatorID();
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trMultiOperator.Visible = true;
            }
            else if(ddlReportType.SelectedValue.ToString().Equals("SeyoonProductionrejectionReportQty", StringComparison.OrdinalIgnoreCase))
            {
                trPlant.Visible = true;
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trCellId.Visible = true;
                trMachine.Visible = true;
                if (isMachineMultiSelect())
                {
                    ddlMachineId.Visible = false;
                    lbMachineID.Visible = true;
                }
                else
                {
                    ddlMachineId.Visible = true;
                    lbMachineID.Visible = false;
                }
                trShiftMultiSelect.Visible = true;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("ComponentStandardCycltimeComparison", StringComparison.OrdinalIgnoreCase))
            {
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trPlant.Visible = true;
                trCellId.Visible = true;
                trMachine.Visible = true;
                trComponentId.Visible = true;
                trOperationID.Visible = true;
                ddlMultiComponentID.Visible = true;
                ddlMultiOperationID.Visible = true;
                ddlComponentId.Visible = false;
                ddlOperationID.Visible = false;
                BindComponentID();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("DailyProductionReport_KunAero", StringComparison.OrdinalIgnoreCase))
            {
                trFromDate.Visible = true;
                trToDate.Visible = false;
                trPlant.Visible = false;
                trCellId.Visible = false;
                trMachine.Visible = true;
                trComponentId.Visible = true;
                trOperationID.Visible = false;
                ddlMultiComponentID.Visible = true;
                ddlMultiOperationID.Visible = false;
                ddlComponentId.Visible = false;
                ddlOperationID.Visible = false;
                BindComponentID();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportComponentwise", StringComparison.OrdinalIgnoreCase))
            {
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trShift.Visible = false;
                trType.Visible = false;
                trFormat.Visible = false;
                trPlant.Visible = false;
                trCellId.Visible = false;
                trMachine.Visible = false;
                trComponentId.Visible = true;
                trOperationID.Visible = true;
                trfromdatetimeconsolidate.Visible = false;
                trtodatetimeconsolidate.Visible = false;
                BindComponentID();
                ddlComponentId_SelectedIndexChanged(null, null);
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportDynamicFlow", StringComparison.OrdinalIgnoreCase))
            {
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trShiftMultiSelect.Visible = true;
                trShift.Visible = false;
                trType.Visible = false;
                trFormat.Visible = false;
                trPlant.Visible = true;
                trCellId.Visible = true;
                trMachine.Visible = true;
                trComponentId.Visible = false;
                trOperationID.Visible = false;
                trfromdatetimeconsolidate.Visible = false;
                trtodatetimeconsolidate.Visible = false;

                //Cell, Machine, Shift Multiselect
                ddlShift.Visible = false;
                lbShiftMultiSelect.Visible = true;

                ddlCellID.Visible = false;
                lbCellID.Visible = true;

                ddlMachineId.Visible = false;
                lbMachineID.Visible = true;
            }
            else if(ddlReportType.SelectedValue.ToString().Equals("DailyProductionReport_RNGupta",StringComparison.OrdinalIgnoreCase))
            {
                trShift.Visible = false;
                trType.Visible = false;
                trFormat.Visible = false;
                trComponentId.Visible = false;
                trOperationID.Visible = false;
                trfromdatetimeconsolidate.Visible = false;
                trtodatetimeconsolidate.Visible = false;

                trShiftMultiSelect.Visible = true;
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trPlant.Visible = true;
                trCellId.Visible = true;
                trMachine.Visible = true;

                //Cell, Machine, Shift Multiselect
                ddlShift.Visible = false;
                lbShiftMultiSelect.Visible = true;
                ddlCellID.Visible = false;
                lbCellID.Visible = true;
                ddlMachineId.Visible = false;
                lbMachineID.Visible = true;
            }
            else if(ddlReportType.SelectedValue.ToString().Equals("IncentiveReport_RNGupta", StringComparison.OrdinalIgnoreCase))
            {
                trShift.Visible = false;
                trType.Visible = false;
                trFormat.Visible = false;
                trComponentId.Visible = false;
                trOperationID.Visible = false;
                trfromdatetimeconsolidate.Visible = false;
                trtodatetimeconsolidate.Visible = false;

                trShiftMultiSelect.Visible = true;
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trPlant.Visible = true;
                trCellId.Visible = true;
                trMachine.Visible = true;

                //Cell, Machine, Shift Multiselect
                ddlShift.Visible = false;
                lbShiftMultiSelect.Visible = true;
                ddlCellID.Visible = false;
                lbCellID.Visible = true;
                ddlMachineId.Visible = false;
                lbMachineID.Visible = true;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("MonthlyProductionReport_RNGupta", StringComparison.OrdinalIgnoreCase))
            {
                trShift.Visible = false;
                trType.Visible = false;
                trFormat.Visible = false;
                trComponentId.Visible = false;
                trOperationID.Visible = false;
                trfromdatetimeconsolidate.Visible = false;
                trtodatetimeconsolidate.Visible = false;

                trShiftMultiSelect.Visible = false;
                trFromDate.Visible = false;
                trToDate.Visible = false;
                trPlant.Visible = false;
                trCellId.Visible = false;
                trMachine.Visible = false;

                //Cell, Machine, Shift Multiselect
                ddlShift.Visible = false;
                lbShiftMultiSelect.Visible = false;
                ddlCellID.Visible = false;
                lbCellID.Visible = false;
                ddlMachineId.Visible = false;
                lbMachineID.Visible = false;
                trmonthlydate.Visible = true;
                txtYear.Text = DateTime.Now.ToString("yyyy");
                txtMonth.Text = DateTime.Now.ToString("MM");
            }
            else if(ddlReportType.SelectedValue.ToString().Equals("ProductionPlantwiseReport_Dantal", StringComparison.OrdinalIgnoreCase))
            {
                trShift.Visible = false;
                trType.Visible = false;
                trFormat.Visible = false;
                trComponentId.Visible = false;
                trOperationID.Visible = false;
                trfromdatetimeconsolidate.Visible = false;
                trtodatetimeconsolidate.Visible = false;

                trFromDate.Visible = true;
                trToDate.Visible = true;
                trPlant.Visible = true;
                trCellId.Visible = true;
                trMachine.Visible = true;

                lbShiftMultiSelect.Visible = false;
                ddlCellID.Visible = false;
                lbCellID.Visible = true;
                ddlMachineId.Visible = false;
                lbMachineID.Visible = true;
                //ddlMultiMachineId.Visible = true;
               // BindMachineIDListBox();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("OperatorPerformanceReport_Seyoon", StringComparison.OrdinalIgnoreCase))
            {
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trPlant.Visible = true;
                trCellId.Visible = false;
                trMachine.Visible = true;
                ddlMachineId.Visible = false;
                //trOperator.Visible = true;
                trMultiOperator.Visible = false;
                BindComponentID();
                //trMultiComponent.Visible = false;
                trShift.Visible = true;
                ddlShift.Visible = true;
                trType.Visible = false;
               // trMoID.Visible = false;
                trNodeID.Visible = false;
                trFormat.Visible = false;
                trview.Visible = false;
                trComponentId.Visible = false;
                trOperationID.Visible = false;
                trDownId.Visible = false;
                trTimeFormat.Visible = false;
                ddlMultiMachineId.Visible = false;
                trDownReason.Visible = false;
                trfromdatetimeconsolidate.Visible = false;
                trtodatetimeconsolidate.Visible = false;
                trmonthlydate.Visible = false;
                trOperatorID.Visible = true;
                ddlOperatorID.Visible = true;
                lbCellID.Visible = false;
                BindOperatorID();
                if (isMachineMultiSelect())
                {
                    ddlMachineId.Visible = false;
                    lbMachineID.Visible = true;
                    ddlCellID.Visible = false;
                    lbCellID.Visible = false;
                }
                else
                {
                    ddlMachineId.Visible = true;
                    lbMachineID.Visible = false;
                    ddlCellID.Visible = true;
                    lbCellID.Visible = false;
                }
            }
            ddlReportFormat_SelectedIndexChanged(null, null);
            ddlFormat_SelectedIndexChanged(null, null);
            BindComponentID();
        }

        protected void ddlview_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlview.SelectedValue.ToString().Equals("NodeView", StringComparison.OrdinalIgnoreCase))
                {
                    trType.Visible = true;

                }
                if (ddlview.SelectedValue.ToString().Equals("MachineView", StringComparison.OrdinalIgnoreCase))
                {
                    trType.Visible = true;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            trmonthlydate.Visible = false;
            trmonthlytodate.Visible = false;
            trCategory.Visible = false;
            trRejectionID.Visible = false;
            trComponentId.Visible = false;
            trOperationID.Visible = false;
            trFromDate.Visible = true;
            trShiftAll.Visible = false;
            trToDate.Visible = true;
            trCellId.Visible = false;
            ddlCellID.Visible = true;
            trOperatorID.Visible = false;
            ddlReportFormat.Items.FindByValue("Format2").Enabled = false;
            if (ddlReportType.SelectedValue.ToString().Equals("BreakdownReportSona", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Month", StringComparison.OrdinalIgnoreCase))
            {
                trmonthlydate.Visible = true;
                trmonthlytodate.Visible = true;
                trFromDate.Visible = false;
                trToDate.Visible = false;
            }
            if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Time-Consolidated", StringComparison.OrdinalIgnoreCase))
            {
                trReportFormat.Visible = false;
                ddlReportFormat.Visible = false;
                trShift.Visible = false;
            }
            if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Shift", StringComparison.OrdinalIgnoreCase))
            {
                trReportFormat.Visible = true;
                trToDate.Visible = true;
                ddlReportFormat.Visible = true;
                trShift.Visible = true;
            }
            if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase))
            {
                trReportFormat.Visible = true;
                ddlReportFormat.Visible = true;
                trShift.Visible = false;
                trToDate.Visible = true;
                if (ConfigurationManager.AppSettings["ConfidentalPage"].ToString() == "1")
                {
                    ddlReportFormat.Items.FindByValue("Format2").Enabled = true;
                }
                ddlReportFormat_SelectedIndexChanged(null, null);
            }
            if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("OEEReportKiswok", StringComparison.OrdinalIgnoreCase))
            {
                trFormat.Visible = false;
                trReportFormat.Visible = false;
                ddlReportFormat.Visible = false;
                trShift.Visible = false;
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trfromdatetimeconsolidate.Visible = false;
                trtodatetimeconsolidate.Visible = false;
                trMachine.Visible = true;
                ddlMachineId.Visible = true;
                ddlMultiMachineId.Visible = false;
                trCellId.Visible = true;
            }
            if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("OEEDailyReportNippon", StringComparison.OrdinalIgnoreCase))
            {
                trFormat.Visible = false;
                trReportFormat.Visible = false;
                ddlReportFormat.Visible = false;
                trShift.Visible = false;
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trfromdatetimeconsolidate.Visible = false;
                trtodatetimeconsolidate.Visible = false;
                trMachine.Visible = true;
                ddlMachineId.Visible = true;
                ddlMultiMachineId.Visible = false;
                trCellId.Visible = true;
            }
            if (ddlReportType.SelectedValue.ToString().Equals("ComparisonReport", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase))
            {
                trShift.Visible = false;
                trToDate.Visible = false;
                trReportFormat.Visible = false;
                ddlReportFormat.Visible = false;
                trFromDate.Visible = false;
                trmonthlydate.Visible = true;
                trEfficiency.Visible = false;
            }
            if (ddlReportType.SelectedValue.ToString().Equals("ComparisonReport", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Month", StringComparison.OrdinalIgnoreCase))
            {
                trShift.Visible = false;
                trToDate.Visible = false;
                trReportFormat.Visible = false;
                ddlReportFormat.Visible = false;
                trFromDate.Visible = false;
                trmonthlydate.Visible = true;
                trmonthlytodate.Visible = true;
                trEfficiency.Visible = true;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("RejectionAnalysisReport", StringComparison.OrdinalIgnoreCase))
            {
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trPlant.Visible = true;
                trMachine.Visible = true;
                trComponentId.Visible = true;
                trOperationID.Visible = true;
                trCategory.Visible = true;
                trRejectionID.Visible = true;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("CycleAnalysis", StringComparison.OrdinalIgnoreCase))
            {
                trview.Visible = true;
                //ddlview.Items.Clear();
                //ddlview.Items.Add(new ListItem("Cycle Time", "CycleTime"));
                //ddlview.Items.Add(new ListItem("Machining Time", "MachiningTime"));
                //ddlview.Items.Add(new ListItem("LoadUnload Time", "LoadUnLoadTime"));

                var abc = ddlview.SelectedValue.ToString();
                trMachine.Visible = true;
                trFromDate.Visible = false;
                trToDate.Visible = false;
                trComponentId.Visible = true;
                trOperationID.Visible = true;
                trmonthlydate.Visible = true;
                txtMonth.Visible = false;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("CycleAnalysisDayWise", StringComparison.OrdinalIgnoreCase))
            {
                trview.Visible = true;
                //ddlview.Items.Clear();
                //ddlview.Items.Add(new ListItem("Cycle Time", "CycleTime"));
                //ddlview.Items.Add(new ListItem("Machining Time", "MachiningTime"));
                //ddlview.Items.Add(new ListItem("LoadUnload Time", "LoadUnLoadTime"));

                trMachine.Visible = true;
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trComponentId.Visible = true;
                trOperationID.Visible = true;
                trmonthlydate.Visible = false;
                txtMonth.Visible = false;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("CycleAnalysisVariant", StringComparison.OrdinalIgnoreCase))
            {
                trview.Visible = false;

                trMachine.Visible = true;
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trComponentId.Visible = true;
                trOperationID.Visible = true;
                trmonthlydate.Visible = false;
                txtMonth.Visible = false;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("SAPOEEReport", StringComparison.OrdinalIgnoreCase))
            {
                trPlant.Visible = true;
                trCellId.Visible = true;
                trMachine.Visible = true;
                trShift.Visible = true;
                trFromDate.Visible = true;
                trToDate.Visible = true;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("JagdevRejectionAnalysisReport", StringComparison.OrdinalIgnoreCase))
            {
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trPlant.Visible = true;
                trMachine.Visible = true;
                trComponentId.Visible = true;
                trOperationID.Visible = true;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("JagdevWeekSummaryChartReport", StringComparison.OrdinalIgnoreCase))
            {
                //trPlant.Visible = true;
                //trMachine.Visible = true;
                trComponentId.Visible = true;
                tryearlywithweek.Visible = true;
                BindComponentID();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("MivinOEEReport", StringComparison.OrdinalIgnoreCase))
            {
                //trPlant.Visible = true;
                //trMachine.Visible = true;
                trPlant.Visible = true;
                trmonthlydate.Visible = true;
                BindPlantId();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("VulkanProdandDowntimeReport", StringComparison.OrdinalIgnoreCase))
            {

                trShiftAll.Visible = true;
                trFromDate.Visible = true;
                trToDate.Visible = true;
                trPlant.Visible = true;
                trMachine.Visible = true;
                trCellId.Visible = true;
                BindPlantId();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("OperatorEfficiencyReport", StringComparison.OrdinalIgnoreCase))
            {
                trOperatorID.Visible = true;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("OperatorIncentiveReport", StringComparison.OrdinalIgnoreCase))
            {
                trOperatorID.Visible = true;
            }
            if (isMachineMultiSelect())
            {
                ddlMachineId.Visible = false;
                lbMachineID.Visible = true;
                trCellId.Visible = true;
                ddlCellID.Visible = false;
                lbCellID.Visible = true;
            }
            else
            {
                ddlMachineId.Visible = true;
                lbMachineID.Visible = false;
                ddlCellID.Visible = true;
                lbCellID.Visible = false;
            }
        }

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellId();
            if (!(ddlReportType.SelectedValue.ToString().Equals("SAPOEEReport", StringComparison.OrdinalIgnoreCase) || (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && (ddlType.SelectedValue.ToString().Equals("OEEReportKiswok", StringComparison.OrdinalIgnoreCase) || ddlType.SelectedValue.ToString().Equals("OEEDailyReportNippon", StringComparison.OrdinalIgnoreCase)))))
            {
                if (isMachineMultiSelect())
                {
                    BindMachineIDListBox();
                }
                else
                {
                    BindMachines();
                }
            }
            if (ddlReportType.SelectedValue.ToString().Equals("OperatorEfficiencyReport", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.ToString().Equals("OperatorIncentiveReport", StringComparison.OrdinalIgnoreCase) || (ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.Equals("DownTimeFormat3", StringComparison.OrdinalIgnoreCase)))
            {
                BindOperatorID();
            }
            //if (ddlReportType.SelectedValue.ToString().Equals("EnergyReport", StringComparison.OrdinalIgnoreCase))
            //{

            // bindNode();
            //}
        }

        protected void ddlMachineId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["sonapages"].Equals("1") && ddlReportType.SelectedValue.ToString().Equals("EnergyReport", StringComparison.OrdinalIgnoreCase))
            {
                bindNode();
                trview.Visible = true;
            }
            if (ConfigurationManager.AppSettings["sonapages"].Equals("1") && ddlReportType.SelectedValue.ToString().Equals("BreakdownSubsystem", StringComparison.OrdinalIgnoreCase))
            {
                BindBreakDownSubsystemIdInfo();
            }
        }

        protected void ddlReportFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase))
            {
                if (ddlReportFormat.SelectedValue.ToString().Equals("SonaReport", StringComparison.OrdinalIgnoreCase) && ConfigurationManager.AppSettings["sonapages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    trToDate.Visible = true;
                //if (ddlReportFormat.SelectedValue.ToString().Equals("Format1", StringComparison.OrdinalIgnoreCase))
                //trToDate.Visible = false;
                if (ConfigurationManager.AppSettings["ConfidentalPage"].ToString() == "1")
                {
                    if (ddlReportFormat.SelectedValue.ToString().Equals("Format2", StringComparison.OrdinalIgnoreCase))
                    {
                        trToDate.Visible = true;
                    }
                }
            }
            if (isMachineMultiSelect())
            {
                ddlMachineId.Visible = false;
                lbMachineID.Visible = true;
                trCellId.Visible = true;
                ddlCellID.Visible = false;
                lbCellID.Visible = true;
            }
            else if(ConfigurationManager.AppSettings["SeyoonReport"].ToString() == "1")
            {
                if(ddlReportType.SelectedValue.ToString().Equals("OperatorPerformanceReport_Seyoon", StringComparison.OrdinalIgnoreCase))
                {
                    trCellId.Visible = false;
                    ddlCellID.Visible = false;
                    lbCellID.Visible = false;
                }
            }
            else
            {
                ddlMachineId.Visible = true;
                if (ddlReportType.SelectedValue.ToString().Equals("OEETrendReport", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.ToString().Equals("OEETrendReport", StringComparison.OrdinalIgnoreCase))
                {
                    ddlMachineId.Visible = false;
                }
                lbMachineID.Visible = false;
                ddlCellID.Visible = true;
                lbCellID.Visible = false;
            }
        }
        #endregion

        private void DownTimeContent()
        {
            //ddlType.Items.FindByValue("MachineDownTimeMatrix").Enabled = true;
            //ddlType.Items.FindByValue("MachinewiseDownTimeDetails").Enabled = true;
            //ddlType.Items.FindByValue("ProdAndDownPie").Enabled = true;
            ddlType.Items.FindByValue("Hour").Enabled = false;
            ddlType.Items.FindByValue("Shift").Enabled = false;
            ddlType.Items.FindByValue("Daily").Enabled = false;
            ddlType.Items.FindByValue("Time-Consolidated").Enabled = false;

            trDownId.Visible = true;
            trTimeFormat.Visible = false;
            BindDownIdInfo();
        }

        private void BreakDownTimeContent()
        {
            //ddlType.Items.FindByValue("MachineDownTimeMatrix").Enabled = true;
            //ddlType.Items.FindByValue("MachinewiseDownTimeDetails").Enabled = true;
            //ddlType.Items.FindByValue("ProdAndDownPie").Enabled = true;
            ddlType.Items.FindByValue("Hour").Enabled = false;
            ddlType.Items.FindByValue("Shift").Enabled = false;
            ddlType.Items.FindByValue("Daily").Enabled = false;
            ddlType.Items.FindByValue("Time-Consolidated").Enabled = false;

            trDownId.Visible = false;
            trBreakDown.Visible = true;
            trTimeFormat.Visible = false;
            BindBreakDownIdInfo();
        }

        private void BreakDownTimeSubsystemContent()
        {
            //ddlType.Items.FindByValue("MachineDownTimeMatrix").Enabled = true;
            //ddlType.Items.FindByValue("MachinewiseDownTimeDetails").Enabled = true;
            //ddlType.Items.FindByValue("ProdAndDownPie").Enabled = true;
            ddlType.Items.FindByValue("Hour").Enabled = false;
            ddlType.Items.FindByValue("Shift").Enabled = false;
            ddlType.Items.FindByValue("Daily").Enabled = false;
            ddlType.Items.FindByValue("Month").Enabled = false;
            ddlType.Items.FindByValue("Time-Consolidated").Enabled = false;

            trDownId.Visible = false;
            trBreakDown.Visible = false;
            trSubsystem.Visible = true;
            trTimeFormat.Visible = false;
            BindBreakDownSubsystemIdInfo();
        }

        #region "Bind BreakDownSubsystemId Information"
        private void BindBreakDownSubsystemIdInfo()
        {
            try
            {
                var DownId = TMPTrakDataBase.GetBreakDownIdInfo("Subsystem", ddlMachineId.SelectedValue.ToString() == "ALL" ? "" : ddlMachineId.SelectedValue.ToString());
                if (DownId != null && DownId.Count > 0)
                {
                    ddlMultisubsystem.DataSource = DownId;
                    ddlMultisubsystem.DataBind();
                }
                else
                {
                    ddlMultisubsystem.DataSource = new List<string>();
                    ddlMultisubsystem.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind BreakDownId Information"
        private void BindBreakDownIdInfo()
        {
            try
            {
                var DownId = TMPTrakDataBase.GetBreakDownIdInfo("BreakdownID", "");
                if (DownId != null && DownId.Count > 0)
                {
                    ddlMultiBreakDownID.DataSource = DownId;
                    ddlMultiBreakDownID.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        private void bindsubsystem()
        {
            try
            {
                var DownId = TMPTrakDataBase.GetBreakDownIdInfo("Subsystem", ddlMachineId.SelectedValue.ToString() == "ALL" ? "" : ddlMachineId.SelectedValue.ToString());
                DownId.Insert(0, "ALL");
                if (DownId != null && DownId.Count > 0)
                {
                    ddlsubsystem.DataSource = DownId;
                    ddlsubsystem.DataBind();
                }
                else
                {
                    ddlsubsystem.DataSource = new List<string>();
                    ddlsubsystem.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        #region "Bind DownId Information"
        private void BindDownIdInfo()
        {
            try
            {
                var DownId = TMPTrakDataBase.GetDownIdInfo(losses);
                if (DownId != null && DownId.Count > 0)
                {
                    ddlMultiDownID.DataSource = DownId;
                    ddlMultiDownID.DataBind();
                }
                if (ConfigurationManager.AppSettings["SSWLPages"].ToString() == "1")
                {
                    foreach (ListItem item in ddlMultiDownID.Items)
                    {
                        if (item.Value.ToUpper().Contains("NO_DATA"))
                        {
                            item.Selected = true;
                            chkExclude.Checked = true;
                            break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PageLoad();
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            //HttpContext.Current.Session["ReportGenerated"] = "Started";
            try
            {


                string machineId = ddlMachineId.SelectedIndex >= 0 ? ddlMachineId.SelectedValue : string.Empty;// ddlMachineId.SelectedItem.ToString();
                string multiSelectMachineId = "";
                string multiselectCellId = "";
                string cellId = ddlCellID.SelectedItem != null ? ddlCellID.SelectedValue : string.Empty;
                string listMachine = "", machineSelection = "";
                #region "Machine selection"
                foreach (ListItem item in ddlMultiMachineId.Items)
                {
                    if (item.Selected)
                    {
                        listMachine += item.Value + "$@#";
                    }
                }
                if (listMachine != "")
                {
                    string[] result = listMachine.Split(new string[] { "$@#" }, StringSplitOptions.None);
                    result = result.Take(result.Count() - 1).ToArray();
                    machineSelection = string.Join(",", result.ToArray());
                }
                #endregion
                string plantId = ddlPlantId.SelectedValue == "ALL" ? string.Empty : ddlPlantId.SelectedValue;
                //string //Ma = ddlPlantId.SelectedValue == "ALL" ? string.Empty : ddlPlantId.SelectedValue;
                DateTime fromDate = DateTime.Now.Date;
                DateTime toDate = DateTime.Now.Date;

                if (trtodatetimeconsolidate.Visible || trfromdatetimeconsolidate.Visible)
                {
                    fromDate = Util.GetDateTime(txttimeconsolidate_fromdate.Text);
                    toDate = Util.GetDateTime(txttimeconsolidate_todate.Text);
                }

                //else if (ddlReportType.SelectedValue.ToString().Equals("EnergyReport", StringComparison.OrdinalIgnoreCase))
                //{
                //	fromDate = Convert.ToDateTime(VDGDataBaseAccess.GetLogicalDayStart(txtFromDate.Text));
                //	toDate = Convert.ToDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtToDate.Text));
                //}
                else if (ddlReportType.SelectedValue.ToString().Equals("BreakdownReportSona", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Month", StringComparison.OrdinalIgnoreCase))
                {
                    string date = "01-" + txtMonth.Text + "-" + txtYear.Text;
                    fromDate = Util.GetDateTime(date);
                    date = "01-" + txttomonth.Text + "-" + txttoyear.Text;
                    toDate = Util.GetDateTime(date);
                }
                else
                {
                    fromDate = Util.GetDateTime(txtFromDate.Text);
                    toDate = Util.GetDateTime(txtToDate.Text);
                }
                if ((ddlReportType.SelectedValue.ToString().Equals("CycleAnalysisDayWise", StringComparison.OrdinalIgnoreCase)) && (toDate - fromDate).Days > 31)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "Alert('Days cannot be greater than 31');", true);
                    return;
                }
                if (ddlReportType.SelectedValue.ToString().Equals("MISReport", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.SONA_MIS_REPORT(fromDate, toDate, ddlMachineId.SelectedValue.ToString(), ddlPlantId.SelectedValue.ToString(), "aggregate");
                }
                if (ddlReportType.SelectedValue.ToString().Equals("EnergyReport", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.EnergyReport(fromDate, toDate, ddlPlantId.SelectedValue.ToString(), ddlMachineId.SelectedValue.ToString(), ddlType.SelectedValue.ToString(), ddlview.SelectedValue.ToString(), ddlNodeid.SelectedValue.ToString(), "aggregate");
                }
                if (ddlReportType.SelectedValue.ToString().Equals("EnergyReportSona", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.EnergyReportSona(fromDate, toDate, "aggregate");
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase))
                {
                    if (ddlReportFormat.SelectedValue.ToString().Equals("SonaReport", StringComparison.OrdinalIgnoreCase))
                    {
                        toDate = toDate = Convert.ToDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtFromDate.Text));
                        Generated = TMPTrakGenerateReport.SONA_BFW_REPORT(fromDate, toDate, ddlPlantId.SelectedValue.ToString(), ddlMachineId.SelectedValue.ToString(), "aggregate");
                    }
                    if (ddlType.SelectedValue.ToString().Equals("Shift", StringComparison.OrdinalIgnoreCase) && ddlReportFormat.SelectedValue.ToString().Equals("Format1", StringComparison.OrdinalIgnoreCase))
                    {
                        Logger.WriteDebugLog("Production Report Machinewise - Shift Web Start : " + DateTime.Now.ToString());
                        machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                        cellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                        Generated = TMPTrakGenerateReport.ProdReportMachinewiseFormat1Shift(fromDate, toDate, ddlShift.SelectedValue.ToString(), ddlPlantId.SelectedValue.ToString(), machineId, cellId);
                        Logger.WriteDebugLog("Production Report Machinewise - Shift Web End : " + DateTime.Now.ToString());
                    }
                    if (ddlType.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase) && ddlReportFormat.SelectedValue.ToString().Equals("Format1", StringComparison.OrdinalIgnoreCase))
                    {
                        Logger.WriteDebugLog("Production Report Machinewise - Daily Web Start : " + DateTime.Now.ToString());
                        machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                        cellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                        Generated = TMPTrakGenerateReport.ProdReportMachinewiseFormat1Daily(fromDate, toDate, ddlShift.SelectedValue.ToString(), ddlPlantId.SelectedValue.ToString(), machineId, cellId);
                        Logger.WriteDebugLog("Production Report Machinewise - Daily Web End : " + DateTime.Now.ToString());
                    }
                    if (ddlType.SelectedValue.ToString().Equals("Time-Consolidated", StringComparison.OrdinalIgnoreCase))
                    {
                        machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                        cellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                        Generated = TMPTrakGenerateReport.ProdReportMachinewiseTimeConsolidated(fromDate, toDate, ddlShift.SelectedValue.ToString(), ddlPlantId.SelectedValue.ToString(), machineId, cellId);
                    }
                    if (ddlType.SelectedValue.ToString().Equals("OEEReportKiswok", StringComparison.OrdinalIgnoreCase))
                    {
                        Generated = TMPTrakGenerateReport.ProdReportMachinewiseOEEReportKiswok(fromDate, toDate, "All", ddlMachineId.SelectedValue.ToString(), ddlPlantId.SelectedValue, ddlCellID.SelectedValue);
                    }
                    if (ddlType.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase) && ddlReportFormat.SelectedValue.ToString().Equals("Format2", StringComparison.OrdinalIgnoreCase))
                    {
                        Generated = TMPTrakGenerateReport.ProdReportMachinewiseFormat2DailyForConfidental(fromDate, toDate, ddlShift.SelectedValue.ToString(), ddlPlantId.SelectedValue.ToString(), ddlMachineId.SelectedValue.ToString());
                    }
                    if (ddlType.SelectedValue.ToString().Equals("OEEDailyReportNippon", StringComparison.OrdinalIgnoreCase))
                    {
                        Generated = TMPTrakGenerateReport.ProdReportMachinewiseOEEDailyReportNippon(fromDate, toDate, "All", ddlMachineId.SelectedValue.ToString(), ddlPlantId.SelectedValue, ddlCellID.SelectedValue);
                    }
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ComparisonReport", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase))
                {
                    Logger.WriteDebugLog("ComparisonReport Web Exec Start :" + DateTime.Now.ToString());
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    cellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    Generated = TMPTrakGenerateReport.ProdReportAggregatedComparisonDaily(txtYear.Text, txtMonth.Text, ddlPlantId.SelectedValue.ToString(), machineId, ddlType.SelectedValue.ToString(), cellId);
                    Logger.WriteDebugLog("ComparisonReport Web Exec End :" + DateTime.Now.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ComparisonReport", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Month", StringComparison.OrdinalIgnoreCase))
                {
                    DateTime fromDate1 = new DateTime(Convert.ToInt32(txtYear.Text), Convert.ToInt32(txtMonth.Text), 1);
                    DateTime toDate1 = new DateTime(Convert.ToInt32(txttoyear.Text), Convert.ToInt32(txttomonth.Text), 1);
                    toDate = toDate.AddMonths(1).AddDays(-1);
                    int dateSpan = ((toDate1.Year - fromDate1.Year) * 12) + toDate1.Month - fromDate1.Month;
                    if (dateSpan >= 12)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "alert('Month cannot be greater than 12');", true);
                        return;
                    }
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    cellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    List<ListItem> efficiencyList = new List<ListItem>();
                    foreach (ListItem item in cblEfficiency.Items)
                    {
                        efficiencyList.Add(new ListItem() { Text = item.Value, Value = item.Selected ? "1" : "0" });
                    }
                    Logger.WriteDebugLog("ComparisonReport-Month Web Exec Start :" + DateTime.Now.ToString());
                    Generated = TMPTrakGenerateReport.ProdReportAggregatedComparisonMonthly(txtYear.Text, txtMonth.Text, txttoyear.Text, txttomonth.Text, ddlPlantId.SelectedValue.ToString(), machineId, ddlType.SelectedValue.ToString(), cellId, efficiencyList);
                    Logger.WriteDebugLog("ComparisonReport-Month Web Exec Start :" + DateTime.Now.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase))
                {
                    multiSelectMachineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    multiselectCellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    int Exclude = 0;
                    string downId = "", listDownId = "";

                    #region "DownId Selection "
                    foreach (ListItem item in ddlMultiDownID.Items)
                    {
                        if (item.Selected)
                        {
                            listDownId += item.Value + "$@#";
                        }
                    }
                    if (listDownId != "")
                    {
                        string[] result = listDownId.Split(new string[] { "$@#" }, StringSplitOptions.None);
                        result = result.Take(result.Count() - 1).ToArray();
                        downId = string.Join(",", result.ToArray());
                    }
                    if (chkExclude.Checked)
                        Exclude = 1;
                    else
                        Exclude = 0;

                    #endregion
                    var TheBrowserWidth = width.Value;
                    var TheBrowserHeight = height.Value;
                    machineSelection = ddlMachineId.SelectedValue == "ALL" ? string.Empty : ddlMachineId.SelectedValue.ToString();
                    //Generated = TMPTrakGenerateReport.MachineDownTimeMatrix(fromDate, ddlPlantId.SelectedValue.ToString(), machineSelection, toDate, downId, Exclude, TheBrowserWidth, TheBrowserHeight, ddlTopDownReasons.SelectedValue.ToString(), "", "aggregate");
                    if (ddlFormat.SelectedValue.ToString().Equals("TimeWise", StringComparison.OrdinalIgnoreCase))
                    {
                        Logger.WriteDebugLog("Down Time Report - Time Wise Web Start : " + DateTime.Now.ToString());
                        Generated = TPMTrakGenerateReportNewDll.MachineDownTimeMatrix(fromDate, ddlPlantId.SelectedValue.ToString(), multiSelectMachineId, toDate, downId, Exclude, TheBrowserWidth, TheBrowserHeight, ddlTopDownReasons.SelectedValue.ToString(), multiselectCellId, "aggregate", "TimeWise");
                        Logger.WriteDebugLog("Down Time Report - Time Wise Web Start : " + DateTime.Now.ToString());
                    }
                    if (ddlFormat.SelectedValue.ToString().Equals("TimeAndFreqWise", StringComparison.OrdinalIgnoreCase))
                    {
                        Logger.WriteDebugLog("Down Time Report - Time & Freq Wise Web Start : " + DateTime.Now.ToString());
                        Generated = TPMTrakGenerateReportNewDll.MachineDownTimeMatrix(fromDate, ddlPlantId.SelectedValue.ToString(), multiSelectMachineId, toDate, downId, Exclude, TheBrowserWidth, TheBrowserHeight, ddlTopDownReasons.SelectedValue.ToString(), multiselectCellId, "aggregate", "TimeAndFreqWise");
                        Logger.WriteDebugLog("Down Time Report - Time & Freq Wise Web End : " + DateTime.Now.ToString());
                    }
                    if (ddlFormat.SelectedValue.ToString().Equals("DownTimeFormat3", StringComparison.OrdinalIgnoreCase))
                    {
                        Generated = TMPTrakGenerateReport.MachineDownTimeMatrixFormat3(fromDate, ddlPlantId.SelectedValue.ToString(), machineSelection, toDate, downId, Exclude, TheBrowserWidth, TheBrowserHeight, ddlTopDownReasons.SelectedValue.ToString(), "", "aggregate", "TimeAndFreqWise", ddlOperatorID.SelectedValue);
                    }
                    if (ddlFormat.SelectedValue.ToString().Equals("MachinewiseDownTimeDetails", StringComparison.OrdinalIgnoreCase))
                    {
                        Generated = TMPTrakGenerateReport.MachineWiseDownTimeDetailsAgg(fromDate, ddlPlantId.SelectedValue.ToString(), multiSelectMachineId, toDate, downId, Exclude, false, multiselectCellId);
                    }
                }
                if (ddlReportType.SelectedValue.ToString().Equals("CycleAnalysis", StringComparison.OrdinalIgnoreCase))
                {
                    string selectedDate = "01-01-" + txtYear.Text;
                    string SelDate = "01-01-" + txtMonth.Text;
                    DateTime SelectedDate = Util.GetDateTime(selectedDate);
                    Generated = TMPTrakGenerateReport.CycleAnalysis(SelectedDate, ddlMachineId.SelectedValue.ToString(), ddlComponentId.SelectedValue.ToString(), ddlOperationID.SelectedValue.ToString(), ddlview.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("CycleAnalysisDayWise", StringComparison.OrdinalIgnoreCase))
                {
                    //string selectedDate = "01-01-" + txtYear.Text;
                    string fDate = txtFromDate.Text;
                    string tDate = txtToDate.Text;
                    DateTime FromDate = Util.GetDateTime(fDate);
                    DateTime ToDate = Util.GetDateTime(tDate);

                    Generated = TMPTrakGenerateReport.CycleAnalysisDayWise(FromDate, ToDate, ddlMachineId.SelectedValue.ToString(), ddlComponentId.SelectedValue.ToString(), ddlOperationID.SelectedValue.ToString(), ddlview.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("CycleAnalysisVariant", StringComparison.OrdinalIgnoreCase))
                {
                    string fDate = txtFromDate.Text;
                    string tDate = txtToDate.Text;
                    DateTime FromDate = Util.GetDateTime(fDate);
                    DateTime ToDate = Util.GetDateTime(tDate);

                    Generated = TMPTrakGenerateReport.CycleAnalysisVariant(FromDate, ToDate, ddlMachineId.SelectedValue.ToString(), ddlComponentId.SelectedValue.ToString(), ddlOperationID.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("BreakdownPhenomena", StringComparison.OrdinalIgnoreCase))
                {
                    int Exclude = 0;
                    string downId = "", listDownId = "";

                    #region "DownId Selection "
                    foreach (ListItem item in ddlMultiBreakDownID.Items)
                    {
                        if (item.Selected)
                        {
                            listDownId += item.Value + "$@#";
                        }
                    }
                    if (listDownId != "")
                    {
                        string[] result = listDownId.Split(new string[] { "$@#" }, StringSplitOptions.None);
                        result = result.Take(result.Count() - 1).ToArray();
                        downId = string.Join(",", result.ToArray());
                    }
                    if (BreakDownchkExclude.Checked)
                        Exclude = 1;
                    else
                        Exclude = 0;
                    #endregion
                    var TheBrowserWidth = width.Value;
                    var TheBrowserHeight = height.Value;
                    machineSelection = ddlMachineId.SelectedValue == "ALL" ? string.Empty : ddlMachineId.SelectedValue.ToString();
                    Generated = TMPTrakGenerateReport.MachineBreakDownTimeMatrix(fromDate, ddlPlantId.SelectedValue.ToString(), machineSelection, toDate, downId, Exclude, TheBrowserWidth, TheBrowserHeight, ddlTopBreakDownreason.SelectedValue.ToString(), "", "aggregate");

                }
                if (ddlReportType.SelectedValue.ToString().Equals("BreakdownSubsystem", StringComparison.OrdinalIgnoreCase))
                {
                    int Exclude = 0;
                    string downId = "", listDownId = "";


                    #region "DownId Selection "
                    foreach (ListItem item in ddlMultisubsystem.Items)
                    {
                        if (item.Selected)
                        {
                            listDownId += item.Value + "$@#";
                        }
                    }
                    if (listDownId != "")
                    {
                        string[] result = listDownId.Split(new string[] { "$@#" }, StringSplitOptions.None);
                        result = result.Take(result.Count() - 1).ToArray();
                        downId = string.Join(",", result.ToArray());
                    }
                    if (BreakDownchkExclude.Checked)
                        Exclude = 1;
                    else
                        Exclude = 0;
                    #endregion
                    var TheBrowserWidth = width.Value;
                    var TheBrowserHeight = height.Value;
                    machineSelection = ddlMachineId.SelectedValue == "ALL" ? string.Empty : ddlMachineId.SelectedValue.ToString();
                    TMPTrakGenerateReport.MachineBreakDownSubsystemTimeMatrix(fromDate, ddlPlantId.SelectedValue.ToString(), machineSelection, toDate, downId, Exclude, TheBrowserWidth, TheBrowserHeight, ddlTopBreakDownreason.SelectedValue.ToString(), "", "aggregate");

                }
                if (ddlReportType.SelectedValue.ToString().Equals("PMTReport", StringComparison.OrdinalIgnoreCase))
                {
                    string date = "01-" + txtMonth.Text + "-" + txtYear.Text;
                    fromDate = Util.GetDateTime(date);
                    Generated = TMPTrakGenerateReport.GeneratePMTReport(fromDate);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("BreakdownReportSona", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.BreakDownSona(fromDate, toDate, ddlPlantId.SelectedValue.ToString(), ddlMachineId.SelectedValue.ToString(), ddlsubsystem.SelectedValue.ToString(), ddlTimeFormat.SelectedValue.ToString(), ddlType.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("RejectionAnalysisReport", StringComparison.OrdinalIgnoreCase))
                {
                    string rejectionID = "";
                    int count = 0;
                    foreach (ListItem data in ddlMultiRejectionID.Items)
                    {
                        if (data.Selected)
                        {
                            if (count == 0)
                            {
                                rejectionID += "'" + data.Value + "'";
                                count++;
                            }
                            else
                            {
                                rejectionID += ",'" + data.Value + "'";
                            }

                        }
                    }
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    cellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    Generated = TMPTrakGenerateReport.RejectionAnalysis(Util.GetDateTime(txtFromDate.Text), Util.GetDateTime(txtToDate.Text), ddlPlantId.SelectedValue == "ALL" ? string.Empty : ddlPlantId.SelectedValue.ToString(), machineId, ddlComponentId.SelectedValue == "ALL" ? string.Empty : ddlComponentId.SelectedValue.ToString(), ddlOperationID.SelectedValue == "ALL" ? string.Empty : ddlOperationID.SelectedValue.ToString(), ddlCatogery.SelectedValue == "ALL" ? string.Empty : ddlCatogery.SelectedValue.ToString(), rejectionID, cellId);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("JagdevRejectionAnalysisReport", StringComparison.OrdinalIgnoreCase))
                {
                    string rejectionID = "";
                    int count = 0;
                    foreach (ListItem data in ddlMultiRejectionID.Items)
                    {
                        if (data.Selected)
                        {
                            if (count == 0)
                            {
                                rejectionID += "'" + data.Value + "'";
                                count++;
                            }
                            else
                            {
                                rejectionID += ",'" + data.Value + "'";
                            }

                        }
                    }
                    Generated = TMPTrakGenerateReport.JagdevRejectionAnalysis(Util.GetDateTime(txtFromDate.Text), Util.GetDateTime(txtToDate.Text), ddlPlantId.SelectedValue == "ALL" ? string.Empty : ddlPlantId.SelectedValue.ToString(), ddlMachineId.SelectedValue == "ALL" ? string.Empty : ddlMachineId.SelectedValue.ToString(), ddlComponentId.SelectedValue == "ALL" ? string.Empty : ddlComponentId.SelectedValue.ToString(), ddlOperationID.SelectedValue == "ALL" ? string.Empty : ddlOperationID.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("JagdevWeekSummaryChartReport", StringComparison.OrdinalIgnoreCase))
                {
                    if (txtYearforWeek.Text == "" || ddlWeekOfYear.SelectedValue == "")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "alert('Year and Week fields are required.');", true);
                        return;
                    }
                    Generated = TMPTrakGenerateReport.JagdevWeeklySummeryChartReport(txtYearforWeek.Text, ddlWeekOfYear.SelectedValue == null ? "" : ddlWeekOfYear.SelectedValue.ToString(), ddlComponentId.SelectedValue == "ALL" ? string.Empty : ddlComponentId.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("SAPOEEReport", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.SAPOEEReportAdvik(fromDate, toDate, ddlShift.SelectedValue.ToString(), ddlPlantId.SelectedValue.ToString(), ddlMachineId.SelectedValue.ToString(), cellId);
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("VulkanProdandDowntimeReport", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.VulkanProdAndDownTimeReport(fromDate, toDate, drpShiftAll.SelectedValue.ToString(), ddlPlantId.SelectedValue.ToString(), ddlMachineId.SelectedValue.ToString(), cellId);
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("MivinOEEReport", StringComparison.OrdinalIgnoreCase))
                {
                    string date = "01-" + txtMonth.Text + "-" + txtYear.Text;
                    fromDate = Util.GetDateTime(date);
                    Generated = TMPTrakGenerateReport.MivinOEEReport(fromDate, ddlPlantId.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("OperatorEfficiencyReport", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.OperatorEfficiencyData(fromDate, toDate, ddlPlantId.SelectedValue.ToString(), ddlOperatorID.SelectedValue.ToString());

                }
                if (ddlReportType.SelectedValue.ToString().Equals("OperatorIncentiveReport", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.OperatorIncentiveReportData(fromDate, toDate, ddlOperatorID.SelectedValue.ToString(), ddlMachineId.SelectedValue, ddlCycleTimeType.SelectedValue);

                }
                if (ddlReportType.SelectedValue.ToString().Equals("OEETrendReport", StringComparison.OrdinalIgnoreCase))
                {
                    string machine = "";
                    foreach (ListItem item in ddlMultiMachineId.Items)
                    {
                        if (item.Selected)
                        {
                            if (machine == "")
                                machine += "'" + item.Value + "'";
                            else
                                machine += ",'" + item.Value + "'";
                        }
                    }
                    if (machine == "")
                    {
                        foreach (ListItem item in ddlMultiMachineId.Items)
                        {
                            if (machine == "")
                                machine += "'" + item.Value + "'";
                            else
                                machine += ",'" + item.Value + "'";
                        }
                    }
                    Generated = TPMTrakGenerateReportNewDll.OEETrendReportData(fromDate, toDate, ddlShift.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlShift.SelectedValue, ddlPlantId.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantId.SelectedValue, machine);

                }
                if (ddlReportType.SelectedValue.ToString().Equals("DailyRejectionReport", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    cellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    Generated = TMPTrakGenerateReport.DailyRejectionReport_Agg(fromDate, toDate, ddlPlantId.SelectedValue.ToString(), ddlOperatorID.SelectedValue, machineId, ddlComponentId.SelectedValue.ToString(), cellId);

                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionAnalysisReportRTPL", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getListBoxValueWithSingleQuote(lbMachineID);
                    cellId = DataBaseAccess.getListBoxValueWithSingleQuote(lbCellID);
                    string shiftMultiSelect = DataBaseAccess.getListBoxValueWithSingleQuote(lbShiftMultiSelect);
                    Generated = TMPTrakGenerateReport.ProductionAnalysisReportRTPL(fromDate, toDate, ddlPlantId.SelectedValue.ToString(), machineId, shiftMultiSelect);

                }
                if (ddlReportType.SelectedValue.ToString().Equals("ShiftwiseDownTimeDetailsAutoTech", StringComparison.OrdinalIgnoreCase))
                {
                    multiSelectMachineId = DataBaseAccess.getListBoxValueWithSingleQuote(lbMachineID);
                    multiselectCellId = DataBaseAccess.getListBoxValueWithSingleQuote(lbCellID);
                    Generated = TPMTrakGenerateReportNewDll.ShiftwiseDownTimeDetailsAutoTech(fromDate, toDate, ddlPlantId.SelectedValue.ToString(), multiselectCellId, multiSelectMachineId);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("SpindleRuntimeReportLnTOdisha", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getListBoxValueWithSingleQuote(lbMachineID);
                    cellId = DataBaseAccess.getListBoxValueWithSingleQuote(lbCellID);
                    Generated = TMPTrakGenerateReport.GenerateSpindleRuntimeReportLnTOdisha(fromDate, toDate, ddlShift.SelectedValue.ToString(), ddlPlantId.SelectedValue.ToString(), cellId, machineId);
                }
                if(ddlReportType.SelectedValue.ToString().Equals("OperatorProductionLoginLogoutReport", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMachineID);
                    Generated = KTASpindle.KTAGenerateReport.GenerateProductionReport(fromDate, toDate, drpShiftAll.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : drpShiftAll.SelectedValue.ToString().Trim(),  machineId);

                }
                if (ddlReportType.SelectedValue.ToString().Equals("OperatorIncentiveReportGK", StringComparison.OrdinalIgnoreCase))
                {
                    string Operator = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(ddlMultiOperator);
                    Generated = TPMTrakGenerateReportNewDll.GenerateOperatorIncentiveReportGK(fromDate, toDate, Operator);
                }
                if(ddlReportType.SelectedValue.ToString().Equals("SeyoonProductionrejectionReportQty", StringComparison.OrdinalIgnoreCase))
                {
                    string Shift = HelperClassGeneric.getListBoxValueWithCommaSeparator(lbShiftMultiSelect);
                    string MachineID = HelperClassGeneric.getListBoxValueWithCommaSeparator(lbMachineID);
                    Generated = TMPTrakGenerateReport.GenerateSeyoonProductionReport(txtFromDate.Text, txtToDate.Text, MachineID, Shift);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ComponentStandardCycltimeComparison", StringComparison.OrdinalIgnoreCase))
                {
                    fromDate = Util.GetDateTime(txtFromDate.Text);
                    toDate = Util.GetDateTime(txtToDate.Text);
                    machineId = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMachineID);
                    //machineId = ddlMachineId.SelectedValue == null ? "" : ddlMachineId.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlMachineId.SelectedValue.ToString();
                    string ComponentID = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(ddlMultiComponentID);
                    string OperationNo = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(ddlMultiOperationID);
                    Generated = TMPTrakGenerateReport.GetComponentStandardCycleTimeComparisonData_KTA(fromDate, toDate, machineId, ComponentID, OperationNo);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("DailyProductionReport_KunAero", StringComparison.OrdinalIgnoreCase))
                {
                    fromDate = Util.GetDateTime(txtFromDate.Text);
                    machineId = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMachineID);
                    string ComponentID = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(ddlMultiComponentID);
                    Generated = TMPTrakGenerateReport.GetDailyProductionReport_KunAero(fromDate, machineId,ComponentID);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportComponentwise", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.ComponentwiseReport_Agg(fromDate, ddlComponentId.SelectedValue.ToString(), ddlOperationID.SelectedValue.ToString(), toDate);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportDynamicFlow", StringComparison.OrdinalIgnoreCase))
                {
                    fromDate = Util.GetDateTime(txtFromDate.Text);
                    toDate = Util.GetDateTime(txtToDate.Text);
                    cellId = HelperClassGeneric.getListBoxValueWithCommaSeparator(lbCellID);
                    machineId = HelperClassGeneric.getListBoxValueWithCommaSeparator(lbMachineID);
                    string shift = HelperClassGeneric.getListBoxValueWithCommaSeparator(lbShiftMultiSelect);
                    Generated = TMPTrakGenerateReport.ProductionReportDynamicFlow_Agg(cellId, machineId, shift, fromDate, toDate);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("DailyProductionReport_RNGupta", StringComparison.OrdinalIgnoreCase))
                {
                    fromDate = Util.GetDateTime(txtFromDate.Text);
                    toDate = Util.GetDateTime(txtToDate.Text);
                    cellId = HelperClassGeneric.getListBoxValueWithCommaSeparator(lbCellID);
                    machineId = HelperClassGeneric.getListBoxValueWithCommaSeparator(lbMachineID);
                    string shift = HelperClassGeneric.getListBoxValueWithCommaSeparator(lbShiftMultiSelect);
                    Generated = TMPTrakGenerateReport.DailyProductionReport_RNGupta(fromDate,toDate, shift, ddlPlantId.SelectedValue.ToString(),cellId,machineId);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("IncentiveReport_RNGupta", StringComparison.OrdinalIgnoreCase))
                {
                    fromDate = Util.GetDateTime(txtFromDate.Text);
                    toDate = Util.GetDateTime(txtToDate.Text);
                    cellId = HelperClassGeneric.getListBoxValueWithCommaSeparator(lbCellID);
                    machineId = HelperClassGeneric.getListBoxValueWithCommaSeparator(lbMachineID);
                    string shift = HelperClassGeneric.getListBoxValueWithCommaSeparator(lbShiftMultiSelect);
                    Generated = TMPTrakGenerateReport.IncentiveReport_RNGupta(fromDate, toDate, shift, ddlPlantId.SelectedValue.ToString(), cellId, machineId);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("MonthlyProductionReport_RNGupta", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.MonthlyProductionReport_RNGupta(txtYear.Text,txtMonth.Text);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionPlantwiseReport_Dantal", StringComparison.OrdinalIgnoreCase))
                {
                    fromDate = Util.GetDateTime(txtFromDate.Text);
                    toDate = Util.GetDateTime(txtToDate.Text);
                    cellId = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbCellID);
                    machineId = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMachineID);
                    string plant = HelperClassGeneric.getDropdownValueWithCommaSeparator(ddlPlantId);
                   Generated= TMPTrakGenerateReport.ProductionDantalReport_agg(fromDate, toDate,  ddlPlantId.SelectedValue.ToString(), cellId, machineId,plant);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("OperatorPerformanceReport_Seyoon", StringComparison.OrdinalIgnoreCase))
                {
                    fromDate = Util.GetDateTime(txtFromDate.Text);
                    toDate = Util.GetDateTime(txtToDate.Text);
                    //cellId = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbCellID);
                    machineId = HelperClassGeneric.getListBoxValueWithCommaSeparator(lbMachineID);
                    string plantD = ddlPlantId.SelectedValue == "ALL" ? string.Empty : ddlPlantId.SelectedValue;
                    string shiftID = ddlShift.SelectedValue == "All" ? string.Empty : ddlShift.SelectedValue;
                    string operatorID = ddlOperatorID.SelectedValue == "ALL" ? string.Empty : ddlOperatorID.SelectedValue;
                    //Generated = TMPTrakGenerateReport.ProductionOperatorShiftwise_agg(fromDate, toDate, ddlPlantId.SelectedValue.ToString(), machineId);
                    Generated = TMPTrakGenerateReport.ShiftwiseOperatorPerformanceReport_Seyoon_Agg(fromDate, toDate, plantD,shiftID, machineId,operatorID);
                }

                if (Generated.Equals("", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);
                else if (Generated.Equals("NodataFound", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNodata", "messageNodata();", true);
                else if (Generated.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "messageOk();", true);
            }
            catch (Exception ex)
            {
                // HttpContext.Current.Session["ReportGenerated"] = "Ended";
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetVal()
        {
            if (HttpContext.Current.Session["ReportGenerated"] != null)
                return HttpContext.Current.Session["ReportGenerated"].ToString();
            else
                return "Ended";

        }

        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isMachineMultiSelect())
                {
                    BindMachineIDListBox();
                }
                else
                {
                    BindMachinesForPlantCell();
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void lbCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isMachineMultiSelect())
            {
                BindMachineIDListBox();
            }
            else
            {
                BindMachinesForPlantCell();
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "openlistbox", "stayMultiselectedList('cell');", true);
        }
        protected void txtYearforWeek_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtYearforWeek.Text != "")
                {
                    List<ListItem> listItems = new List<ListItem>();
                    for (int i = 1; i <= 52; i++)
                    {
                        listItems.Add(new ListItem
                        {
                            Text = "Week" + i,
                            Value = i.ToString()
                        });
                    }
                    ddlWeekOfYear.DataSource = listItems;
                    ddlWeekOfYear.DataTextField = "Text";
                    ddlWeekOfYear.DataValueField = "Value";
                    ddlWeekOfYear.DataBind();
                }
                else
                {
                    List<ListItem> listItems = new List<ListItem>();
                    ddlWeekOfYear.DataSource = listItems;
                    ddlWeekOfYear.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase))
                {
                    if (ddlFormat.SelectedValue.ToString().Equals("DownTimeFormat3", StringComparison.OrdinalIgnoreCase))
                    {
                        trCellId.Visible = false;
                        ddlMachineId.Visible = true;
                        lbMachineID.Visible = false;
                        trDownId.Visible = false;
                        trDownReason.Visible = false;
                        trOperatorID.Visible = true;
                        BindOperatorID();
                    }
                    else if (ddlFormat.SelectedValue.ToString().Equals("TimeAndFreqWise", StringComparison.OrdinalIgnoreCase))
                    {
                        trDownReason.Visible = true;
                    }
                    else
                    {
                        trDownId.Visible = true;
                        trDownReason.Visible = false;
                        trOperatorID.Visible = false;
                    }
                }
                if (isMachineMultiSelect())
                {
                    trCellId.Visible = true;
                    ddlCellID.Visible = false;
                    lbCellID.Visible = true;
                }
                 if (ConfigurationManager.AppSettings["SeyoonReport"].ToString() == "1")
                {
                    if (ddlReportType.SelectedValue.ToString().Equals("OperatorPerformanceReport_Seyoon", StringComparison.OrdinalIgnoreCase))
                    {
                        trCellId.Visible = false;
                        ddlCellID.Visible = false;
                        lbCellID.Visible = false;
                    }
                }

            }
            catch (Exception ex)
            { }
        }

        protected void ddlMultiComponentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOperationID();
        }

        protected void lbMachineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindComponentID();
            BindOperationID();
            if (ConfigurationManager.AppSettings["SeyoonReport"].ToString() == "1"|| ConfigurationManager.AppSettings["DantalHydraulicsPages"].ToString() == "1")
            {
                if (ddlReportType.SelectedValue.ToString().Equals("OperatorPerformanceReport_Seyoon", StringComparison.OrdinalIgnoreCase)|| ddlReportType.SelectedValue.ToString().Equals("ProductionPlantwiseReport_Dantal", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "openlistbox", "stayMultiselectedList('machine');", true);
                }
            }
            
        }

        protected void btnComponent_Click(object sender, EventArgs e)
        {
            List<string> Component = new List<string>();
            if (txtComponent != null)
            {
                if (!(string.IsNullOrEmpty(txtComponent.Text)) || !(string.IsNullOrEmpty(txttimeconsolidate_fromdate.Text)))
                {
                    if (string.IsNullOrEmpty(txttimeconsolidate_fromdate.Text) || string.IsNullOrEmpty(txttimeconsolidate_todate.Text))
                        Component = DataBaseAccess.Getcomponentfromtxt(txtComponent.Text.ToString());
                    else
                    {
                        DateTime fromdate = Util.GetDateTime(txttimeconsolidate_fromdate.Text);
                        DateTime todate = Util.GetDateTime(txttimeconsolidate_todate.Text);
                        Component = DataBaseAccess.getcomponentbydatecomponent(fromdate.ToString("yyyy-MM-dd HH:mm:ss"), todate.ToString("yyyy-MM-dd HH:mm:ss"), txtComponent.Text.ToString());
                    }
                    if (Component != null && Component.Count > 0)
                    {
                        ddlComponentId.DataSource = Component;
                        ddlComponentId.DataBind();
                        ddlMultiComponentID.DataSource = Component;
                        ddlMultiComponentID.DataBind();
                        if (string.IsNullOrEmpty(txtComponent.Text))
                            ddlComponentId.Items.Insert(0, new ListItem
                            {
                                Text = GetGlobalResourceObject("CommanResource", "ComponentAll").ToString(),
                                Value = "All"
                            });
                        ddlComponentId_SelectedIndexChanged(null, null);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Component Found for the Given Search')", true);
                        txtComponent.Text = "";
                        txttimeconsolidate_fromdate.Text = ""; txttimeconsolidate_todate.Text = "";
                        BindComponentID();
                    }
                }
                else
                {
                    BindComponentID();
                }
            }
        }
    }
}