using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessClassLibrary;
using Elmah;
using Web_TPMTrakDashboard.Models;
using ModelClassLibrary;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Services;
using Web_TPMTrakDashboard.KTASpindle;
using Web_TPMTrakDashboard.HighWay;
using System.Data;

namespace Web_TPMTrakDashboard
{
    public partial class ReportForm : System.Web.UI.Page
    {
        string Generated = "";
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
                PageLoad();
            }
        }

        #region PageLoad
        private void PageLoad()
        {
            //trToDate.Visible = false;
            trNodeID.Visible = false;
            trmonthlydate.Visible = false;
            trBreakDown.Visible = false;
            trview.Visible = false;
            trmivintype.Visible = false;
            trProcessType.Visible = false;
            reportTypeForVulkanAM.Visible = false;
            if (ConfigurationManager.AppSettings["DantalHydraulicsPages"] == "1")
            {
                ddlReportType.Items.Add(new ListItem("Production Report - Dantal", "ProductionPlantwiseReport_Dantal")); 
            }
            if (ConfigurationManager.AppSettings["HighwayPages"] == "1")
            {
                //ddlReportType.Items.Add(new ListItem("Checksheet Approval Report_Highway", "ChecksheetApprovalReport_Highway"));
                //ddlReportType.Items.Add(new ListItem("Inspection Approval Report_Highway", "InspectionApprovalReport_Highway"));
            }
            if (ConfigurationManager.AppSettings["ShowMOwiseReport"] == "1")
            {   
                ddlReportType.Items.Add(new ListItem("MO Wise Report", "MOWISEREPORT"));
            }
            ddlReportType.Items.Add(new ListItem("Operator Efficiency Report", "OperatorEffeciencyReport"));
            if (ConfigurationManager.AppSettings["sonapages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("MIS Report", "MISReport"));
                //ddlReportType.Items.Add(new ListItem("Energy Report", "EnergyReport"));
                //ddlReportType.Items.Add(new ListItem("Energy Report Sona", "EnergyReportSona"));
                ddlReportType.Items.Add(new ListItem("Breakdown Phenomena Report", "BreakdownPhenomena"));
                ddlFormat.Items.Add(new ListItem("SONA BLW", "SONABLW"));
                ddlFormat.Items.FindByValue("SONABLW").Enabled = false;
            }
            if (ConfigurationManager.AppSettings["MivinPages"].ToString().Equals("1"))
            {
                ddlReportType.Items.Add(new ListItem("Mivin Inspection Report", "MivinInspectionReport"));
            }
            if (ConfigurationManager.AppSettings["Toolsequence"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase) || ConfigurationManager.AppSettings["ShantiIronPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
            {
                ddlReportType.Items.Add(new ListItem("Tool Change Report", "ToolChangeReport"));
            }
            if (ConfigurationManager.AppSettings["ShantiIronPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
            {
                //ddlReportType.Items.Add(new ListItem("Tool Change Frequency Report", "ToolChangeFrequencyReport"));
                ddlReportType.Items.Add(new ListItem("Average Tool Life Report", "AVGToolChangeReport"));

                ddlReportType.Items.Add(new ListItem("Rejection/Rework Report Shanti Format", "ShanthiProdReport"));
                ddlReportType.Items.Add(new ListItem("PM Transaction Report", "PMReport"));
                //ddlReportType.Items.Add(new ListItem("Shanthi Production Report", "ShanthiProdReport"));

            }
            ddlReportType.Items.Add(new ListItem("Monthly Oee Report ", "MonthlyOeeReportShantiFormat"));
            if (ConfigurationManager.AppSettings["ShowHytechReport"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
            {
                ddlReportType.Items.Add(new ListItem("Hy-Tech Report", "Hytechreport"));
                trmonthlydate.Visible = true;
            }
            if (ConfigurationManager.AppSettings["ShowFlowMeterReport"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
            {
                ddlReportType.Items.Add(new ListItem("Flow Meter Report", "FlowMeterReport"));
            }
            if (ConfigurationManager.AppSettings["PatelbrassPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Hourly Part Count ", "HourlyPartCount"));
            }
            if (ConfigurationManager.AppSettings["AAAPL"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("PM Report ", "PMReportAAAPL"));
            }
            if (ConfigurationManager.AppSettings["Trelleborg"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Trelleborg OEE Report ", "TrelleborgOEEReport"));
            }
            if (ConfigurationManager.AppSettings["EnableDrawingNoChangeHistoryReport"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Drawing No. Report ", "OperatorDrawingNoReport"));
            }
            if (ConfigurationManager.AppSettings["ShowMachineReworkReport"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Machine Rework Report", "MachineReworkReport"));
            }
            if (ConfigurationManager.AppSettings["AmararagaMangalPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Machinewise Scrap Report", "MachinewiseScrapReport"));
            }
            if (ConfigurationManager.AppSettings["KTASpindlePages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Component Setup Report", "ComponentSetupReport"));
                //ddlReportType.Items.Add(new ListItem("Component Standard Cycltime Comparison", "ComponentStandardCycltimeComparison"));
            }
            //if (ConfigurationManager.AppSettings["GEAPages"].ToString() == "1")
            //{
            //    ddlReportType.Items.Add(new ListItem("Proddution Report GEA", "ProddutionReportGEA"));
            //}
            if (ConfigurationManager.AppSettings["LeoninePages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Production Hourly Report - Leonine", "ProductionHourlyReportLeonine"));
            }
            if (ConfigurationManager.AppSettings["HarshaReport"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("KKPillar Report", "KKPillarReport"));
            }
            if (ConfigurationManager.AppSettings["AlliedPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Hydro Test Report", "HydroTestReport"));
            }
            if (ConfigurationManager.AppSettings["LnTOdishaPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Spindle Idle Time Analysis", "SpindleIdleTimeAnalysisReportLnTOdisha"));
                ddlReportType.Items.Add(new ListItem("PM Transaction Report", "PMTransactionReportLnTOdisha"));
            }
            if (ConfigurationManager.AppSettings["AceDesignersPage"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Machine Utilization report- ACE", "MachineUtilizationReportACE"));
            }
            if (ConfigurationManager.AppSettings["TAFEChennaiPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("PDI Report", "PDIReportTafeChennai"));
            }
            if (ConfigurationManager.AppSettings["VulkanMachineShopPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Production Cum Inspection Report", "InspectionReportVulkan"));
                ddlReportType.Items.Add(new ListItem("PM Transaction Report", "PMTransactionReport"));
                ddlReportType.Items.Add(new ListItem("Cycle Time Report", "CycleTimeReport"));
                ddlReportType.Items.Add(new ListItem("Tool Life Report", "ToolLifeReport"));
            }
            if (ConfigurationManager.AppSettings["VulkanPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Daily Checklist Report", "AMTransactionReport"));
            }
            if (ConfigurationManager.AppSettings["SKSPages"].ToString() == "1")
                ddlReportType.Items.Add(new ListItem("ERP Performance Report", "ERPPerformanceReportSKS"));
            if (ConfigurationManager.AppSettings["RexnordPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Inspection Report - Rexnord", "InspectionReport_Rexnord"));
            }
            if (ConfigurationManager.AppSettings["PrecisionEngPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Non Machining Production Report - Machinewise", "ProductionReportMachinewise_PrecisionEngg"));
                ddlReportType.Items.Add(new ListItem("Non Machining Production Report - Componentwise", "ProductionReportComponentwise_PrecisionEngg"));
                //ddlReportType.Items.Add(new ListItem("Non Machining Daily Rejection Report", "DailyRejectionReport_PrecisionEngg"));
                ddlReportType.Items.Add(new ListItem("Non Machining Operator Efficiency Report", "OperatorEfficiencyReport_PrecisionEngg"));
                //ddlReportType.Items.Add(new ListItem("Daily Cleaning & Maintenance Report", "DailyCleaningandMaintenanceReport_PrecisionEngg"));
                ddlReportType.Items.Add(new ListItem("Maintenance Particular Report", "MaintenanceParticularReport_PrecisionEngg"));
                BindGroup_Precision();
            }
            trmonthlydate.Visible = false;
            trMoID.Visible = false;
            trComponentId.Visible = false;
            trOperationID.Visible = false;
            trDownId.Visible = false;
            trfromdatetimeconsolidate.Visible = false;

            txttimeconsolidate_fromdate.Text = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(txttimeconsolidate_fromdate.Text)).ToString("dd-MM-yyyy HH:mm:ss");
            txttimeconsolidate_todate.Text = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txttimeconsolidate_todate.Text)).ToString("dd-MM-yyyy HH:mm:ss");

            trtodatetimeconsolidate.Visible = false;
            trToDate.Visible = true;
            trFromDate.Visible = true;
            trCellId.Visible = true;
            trToolNumber.Visible = false;
            trTimeFormat.Visible = false;
            ddlMultiMachineId.Visible = false;
            ddlMultiOperationID.Visible = false;
            trDownReason.Visible = false;
            ddlType.Items.FindByValue("Shift").Selected = true;
            trToolNumber.Visible = false;
            ddlTopDownReasons.SelectedValue = "10";
            ddlType.SelectedValue = "Shift";
            ddlFormat.Items.FindByValue("Format1").Selected = true;
            ddlFormat.Items.FindByValue("OEEGraphicalReport").Enabled = true;
            trGroupID.Visible = false;
            if (ConfigurationManager.AppSettings["VulkanPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Tool Change Frequency Record", "ToolChangeFrequencyRecord"));
            }
            if (ConfigurationManager.AppSettings["GEAPages"].ToString().Equals("1"))
            {
                ddlFormat.Items.FindByValue("Format3").Enabled = false;
            }
            else
            {
                ddlFormat.Items.FindByValue("Format3").Enabled = true;
            }
            ddlType.Items.FindByValue("MachineDownTimeMatrix").Enabled = false;
            ddlFormat.Items.FindByValue("Format1CockpitDataReport").Enabled = false;
            ddlFormat.Items.FindByValue("Format1EXCEL").Enabled = false;
            ddlFormat.Items.FindByValue("Format2").Enabled = false;
            if (ConfigurationManager.AppSettings["KiswokPage"].ToString() == "1")
            {
                ddlFormat.Items.FindByValue("Format4").Enabled = true;
            }
            else
            {
                ddlFormat.Items.FindByValue("Format4").Enabled = false;
            }
            ddlType.Items.FindByValue("MachinewiseDownTimeDetails").Enabled = false;
            ddlType.Items.FindByValue("Hour").Enabled = false;
            ddlType.Items.FindByValue("TimeWise").Enabled = false;
            ddlType.Items.FindByValue("TimeAndFreqWise").Enabled = false;
            trDieNo.Visible = false;
            trHeatNo.Visible = false;
            trrevid.Visible = false;
            trReportType.Visible = false;
            trCheckpoints.Visible = false;
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
            if (!useAccessData.Where(ss => ss.Code.Equals("DailyRejectionReport", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("DailyRejectionReport"));
            if (!useAccessData.Where(ss => ss.Code.Equals("HelpRequestReport", StringComparison.OrdinalIgnoreCase) && (ss.Domain.Equals("HelpRequset", StringComparison.OrdinalIgnoreCase) || ss.Domain.Equals("HR", StringComparison.OrdinalIgnoreCase))).Select(ss => ss.Selected).FirstOrDefault())
                ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("HelpRequest"));
            if (ConfigurationManager.AppSettings["AnjaliPages"].ToString() == "1")
            {
                ddlReportType.Items.Add(new ListItem("Operator Incentive Report - Hourwise", "HourwiseOperatorIncentiveReport"));
            }
            //if (!useAccessData.Where(ss => ss.Code.Equals("NewOEEReport", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
            //    ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("NewOEEReport"));
            if (ConfigurationManager.AppSettings["GEAPages"].ToString() == "1")
            {
                //ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("MonthlyOeeReportShantiFormat"));
                //if (ddlReportType.Items.FindByValue("ProductionAndDowntimeReportDailyByHour") != null)
                //{
                ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("ProductionAndDowntimeReportDailyByHour"));
                //}
                //if (ddlReportType.Items.FindByValue("HelpRequestReport") != null)
                //{
                ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("HelpRequest"));
                //}
                //if (ddlReportType.Items.FindByValue("MonthlyOeeReportShantiFormat") != null)
                //{

                //}
            }
            if (ConfigurationManager.AppSettings["Advik184Pages"].ToString() == "1")
            {
                BindModel();
                ddlReportType.Items.Add(new ListItem("Data Traceability Report", "DataTraceabilityReportAdvikPanth"));
            }
            BindPlantId();
            BindCellId();
            BindMOIDS("", "");
            BindShiftData();
            BindOperator();


            if (ddlReportType.SelectedValue != null)
            {
                ddlReportType_SelectedIndexChanged(null, null);
            }
            if (ConfigurationManager.AppSettings["SSWLPages"].ToString() == "1")
            {
                chkExclude.Checked = true;
            }
        }
        #endregion

        #region Group ID - Precision--
        private void BindGroup_Precision()
        {
            try
            {
                List<GroupDefintion> list = DataBaseAccess.GetMaintenanceGroupDetails_Precision();
                if(list.Count>0)
                {
                    list.Insert(0, new GroupDefintion { GroupID = "All",GroupDesc="All" });
                }
                ddlGroupID.DataSource = list.AsEnumerable().Select(x => x.GroupID).Distinct().ToList();
                ddlGroupID.DataBind();
                ddlGroupID_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GroupID_precision "+ex.ToString());
            }
        }

        #endregion

        #region Bind Model 
        private void BindModel()
        {
            try
            {
                List<string> list = DataBaseAccess.getAdvikPanthModel();
                if (list.Count > 0)
                {
                    list.Insert(0, "All");
                }
                ddlModel.DataSource = list;
                ddlModel.DataBind();

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Shift Data"
        private void BindShiftData()
        {
            try
            {
                var allShift = BindCockpitView.GetAllShift();

                if (allShift != null && allShift.Count > 0)
                {
                    ddlShift.DataSource = allShift;
                    ddlShift.DataBind();

                    if (!ddlReportType.SelectedValue.Equals("InspectionReportVulkan", StringComparison.OrdinalIgnoreCase))
                        ddlShift.Items.Insert(0, new ListItem
                        {
                            Text = GetGlobalResourceObject("CommanResource", "ShiftAll").ToString(),
                            Value = "All"
                        });
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region BindOperator
        private void BindOperator()
        {
            try
            {
                ddlOperator.DataSource = DataBaseAccess.GetAllEmployeesWithName();
                ddlOperator.DataTextField = "Text";
                ddlOperator.DataValueField = "Value";
                ddlOperator.DataBind();

                ddlMultiOperator.DataSource = DataBaseAccess.GetAllEmployeesID();
                ddlMultiOperator.DataBind();
                foreach (ListItem item in ddlMultiOperator.Items)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        #endregion

        #region "Bind Plant Id"
        private void BindPlantId()
        {
            try
            {

                List<string> lstPlantData = BindCockpitView.ViewPlantToDisplay();
                ddlPlantId.DataSource = lstPlantData;
                ddlPlantId.DataBind();

                if (!ddlType.SelectedValue.ToString().Equals("Hour", StringComparison.OrdinalIgnoreCase) ||
                    ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportDailyByHour", StringComparison.OrdinalIgnoreCase)
                    || ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.ToString().Equals("TrelleborgOEEReport", StringComparison.OrdinalIgnoreCase))
                {
                    ddlPlantId.Items.Insert(0, new ListItem
                    {
                        Text = GetGlobalResourceObject("CommanResource", "PlantAll").ToString(),
                        Value = "ALL"
                    });
                }
               
                ddlPlantId_SelectedIndexChanged(null, null);

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region Bindcell
        private void BindCellId()
        {
            try
            {
                string plantid = (ddlPlantId.SelectedItem == null || ddlPlantId.SelectedItem.Text.Contains("All") || ddlPlantId.SelectedItem.Text.Contains("ALL")) ? "" : ddlPlantId.SelectedItem.Text;
                List<string> lstCellId = BindCockpitView.ViewCellsToDisplay(plantid);

                if (ddlReportType.SelectedValue.ToString().Equals("PMReport"))
                {
                    lstCellId.Remove("Shared");
                    ddlCellID.Items.Clear();
                    ddlCellID.DataSource = lstCellId;
                    ddlCellID.DataBind();
                }
                //else if (ddlReportType.SelectedValue.ToString().Equals("ProductionPlantwiseReport_Dantal", StringComparison.OrdinalIgnoreCase))
                //{
                //    lbCellID.DataSource = lstCellId;
                //    lbCellID.DataBind();
                //    foreach (ListItem item in lbCellID.Items)
                //    {
                //        item.Selected = true;
                //    }
                //    BindMachineIDListBox();
                //    ddlCellID.DataSource = lstCellId;
                //    ddlCellID.DataBind();
                //    ddlCellID.Items.Insert(0, new ListItem
                //    {
                //        Text = GetGlobalResourceObject("CommanResource", "CellAll").ToString(),
                //        Value = "All"
                //    });
                //}
                else
                {
                    lbCellID.DataSource = lstCellId;
                    lbCellID.DataBind();
                    foreach (ListItem item in lbCellID.Items)
                    {
                        item.Selected = true;
                    }
                    BindMachineIDListBox();
                    ddlCellID.DataSource = lstCellId;
                    ddlCellID.DataBind();
                    ddlCellID.Items.Insert(0, new ListItem
                    {
                        Text = GetGlobalResourceObject("CommanResource", "CellAll").ToString(),
                        Value = "All"
                    });

                }
                if (ddlReportType.SelectedValue.ToString().Equals("AVGToolChangeReport", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.ToString().Equals("PMReport", StringComparison.OrdinalIgnoreCase))
                {
                    BindMachinesForPlantCell();
                }
                
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region BindMachinesForPlantCell
        private void BindMachinesForPlantCell()
        {
            try
            {
                string plant = ddlPlantId.SelectedValue == null ? "" : ddlPlantId.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantId.SelectedValue.ToString();
                string Cell = ddlCellID.SelectedValue == null ? "" : ddlCellID.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCellID.SelectedValue.ToString();
                List<string> lstMachineId = new List<string>();
                if (ddlReportType.SelectedValue.ToString().Equals("PMReport", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.ToString().Equals("MivinInspectionReport") || (ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format1", StringComparison.OrdinalIgnoreCase)) || (ddlReportType.SelectedValue.ToString().Equals("SpindleIdleTimeAnalysisReportLnTOdisha", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format1", StringComparison.OrdinalIgnoreCase)))
                {
                    lstMachineId = VDGDataBaseAccess.GetMachinesbyPlantCell(plant, Cell);
                }
                else
                {
                    lstMachineId = VDGDataBaseAccess.GetMachinesbyPlantCell(plant, Cell);
                    lstMachineId.Insert(0, "All");
                }

                ddlMachineId.DataSource = lstMachineId;
                ddlMachineId.DataBind();

                //ddlCellID.Items.Insert(0, new ListItem
                //{
                //    Text = GetGlobalResourceObject("CommanResource", "CellAll").ToString(),
                //    Value = "All"
                //});

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region "Bind Machine Id"
        private void BindMachines()
        {
            try
            {
                ddlMachineId.Items.Clear();// = null;
                if (ddlReportType.SelectedValue.Equals("PMTransactionReportLnTOdisha", StringComparison.OrdinalIgnoreCase))
                { //Show All the machines irrespective of tpmtrakenabled flag for PM Report.
                    var AllMachines = LnTOdisha.Model.LnTOdishaDBAccess.GetMachineInfoForPM();
                    ddlMachineId.DataSource = AllMachines;
                    ddlMachineId.DataBind();
                }
                else
                {
                    var allMachineName = VDGDataBaseAccess.GetAllMachines(ddlPlantId.SelectedValue.ToString());// AccessReportData.GetAllMachines(ddlPlantId.SelectedItem.ToString());
                if (allMachineName != null && allMachineName.Count > 0)
                {
                    if (ddlReportType.SelectedValue.ToString().Equals("FlowMeterReport", StringComparison.OrdinalIgnoreCase))
                    {
                        var FlowMeterMachLst = VDGDataBaseAccess.GetMachineIDsLst(ddlPlantId.SelectedValue.ToString());
                        ddlMachineId.DataSource = FlowMeterMachLst;
                        ddlMachineId.DataBind();
                    }
                    else if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise_PrecisionEngg", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.ToString().Equals("DailyRejectionReport_PrecisionEngg", StringComparison.OrdinalIgnoreCase))
                    {
                        var NonmachiningLst = VDGDataBaseAccess.GetNonMachineLst_PrecisionEngg(ddlPlantId.SelectedValue.ToString());
                        ddlMachineId.DataSource = NonmachiningLst;
                        ddlMachineId.DataBind();
                    }
                    else
                    {
                        ddlMachineId.DataSource = allMachineName;
                        ddlMachineId.DataBind();
                    }

                    ddlMultiMachineId.DataSource = allMachineName;
                    ddlMultiMachineId.DataBind();
                    foreach (ListItem item in ddlMultiMachineId.Items)
                    {
                        item.Selected = true;
                    }
                    if (ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("MachineDownTimeMatrix", StringComparison.OrdinalIgnoreCase))
                    {
                        ddlMultiMachineId.Visible = true;
                        ddlMachineId.Visible = false;
                        ddlMultiMachineId.DataSource = allMachineName;
                        ddlMultiMachineId.DataBind();
                    }
                   

                    //if (ddlType.SelectedValue.ToString().Equals("Hour", StringComparison.OrdinalIgnoreCase) ||
                    //	ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportDailyByHour", StringComparison.OrdinalIgnoreCase) ||
                    //	ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase) ||(ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportMachinewise", StringComparison.OrdinalIgnoreCase)))
                    //	ddlMachineId.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommanResource", "MachineAll").ToString(), "ALL"));

                    if ((ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportDailyByHour", StringComparison.OrdinalIgnoreCase)))
                    {
                        ddlMachineId.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommanResource", "MachineAll").ToString(), "ALL"));
                    }
                    else
                    if (ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportMachinewise", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.ToString().Equals("AVGToolChangeReport", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.ToString().Equals("SpindleIdleTimeAnalysisReportLnTOdisha", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.ToString().Equals("ToolLifeReport", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.Equals("CycleTimeReport", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.Equals("ERPPerformanceReportSKS", StringComparison.OrdinalIgnoreCase))
                    {
                        ddlMachineId.Items.Remove("MachineAll");
                    }
                    else
                    {
                        ddlMachineId.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommanResource", "MachineAll").ToString(), "ALL"));
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
        private void BindMachineIDListBox()
        {
            try
            {
                string plantId = ddlPlantId.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantId.SelectedValue;
                string cellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                if (ddlReportType.SelectedValue.ToString().Equals("HydroTestReport", StringComparison.OrdinalIgnoreCase))
                {
                    var Machinelst = VDGDataBaseAccess.GetAllHydroStaticMachineLst(ddlPlantId.SelectedValue.ToString());
                    lbMachineID.DataSource = Machinelst;
                    lbMachineID.DataBind();
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("DailyCleaningandMaintenanceReport_PrecisionEngg", StringComparison.OrdinalIgnoreCase))
                {
                    List<string> list = DataBaseAccess.GetMachineforGroup_Precision(ddlGroupID.SelectedValue.ToString());
                    lbMachineID.DataSource = list;
                    lbMachineID.DataBind();
                }
                //else if(ddlReportType.SelectedValue.ToString().Equals("ProductionPlantwiseReport_Dantal", StringComparison.OrdinalIgnoreCase))
                //{
                //    lbMachineID.DataSource = DataBaseAccess.getMachineIDListForScreen(plantId, cellId, "ReportLive");
                //    lbMachineID.DataBind();
                //}
                else
                {
                    lbMachineID.DataSource = DataBaseAccess.getMachineIDListForScreen(plantId, cellId, "ReportLive");
                    lbMachineID.DataBind();
                }
                foreach (ListItem item in lbMachineID.Items)
                {
                    item.Selected = true;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachineIDListBox = " + ex.Message);
            }
        }
        #endregion

        #region "Bind Component ID"
        private void BindComponentID()
        {
            try
            {
                var lstPlantData = BindCockpitView.ViewComponentInfo();
                Session["ComponentList"] = lstPlantData;
                ddlComponentId.DataSource = lstPlantData;
                ddlComponentId.DataBind();
                ddlComponentId.Items.Insert(0, new ListItem
                {
                    Text = GetGlobalResourceObject("CommanResource", "ComponentAll").ToString(),
                    Value = "All"
                });
                ddlComponentId_SelectedIndexChanged(null, null);

                lstComponentID.DataSource = lstPlantData;
                lstComponentID.DataBind();
                foreach (ListItem item in lstComponentID.Items)
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
        #endregion

        #region "Bind Tools Life Data"
        private void BindToolsLifeData()
        {
            try
            {
                ddlToolNumber.Items.Clear();// = null;
                                            //  var allMachineName = BindCockpitView.FocusToolLifeData("");
                var allMachineName = BindCockpitView.GetToolNumberForMachine(ddlMachineId.SelectedValue == "ALL" ? string.Empty : ddlMachineId.SelectedValue);
                if (allMachineName != null && allMachineName.Count > 0)
                {
                    ddlToolNumber.DataSource = allMachineName;
                    ddlToolNumber.DataBind();
                    ddlToolNumber.Items.Insert(0, new ListItem
                    {
                        Text = "Tool No. All",
                        Value = ""
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Operation No"
        private void BindOperationNo(string componentId)
        {
            try
            {
                List<string> ComponetId = new List<string>();
                if (ConfigurationManager.AppSettings["RexnordPages"].ToString() == "1")
                {
                    ComponetId = DataBaseAccess.GetOperaionNo_Rexnord(ddlSlno.SelectedValue.ToString());
                }
                else
                {
                    ComponetId = TMPTrakDataBase.GetOperationNo(componentId);
                }
                if (ComponetId != null && ComponetId.Count > 0)
                {
                    ddlOperationID.DataSource = ComponetId;
                    ddlOperationID.DataBind();
                    ddlMultiOperationID.DataSource = ComponetId;
                    ddlMultiOperationID.DataBind();
                    ddlOperationID.Items.Insert(0, new ListItem
                    {
                        Text = GetGlobalResourceObject("CommanResource", "ALL").ToString(),
                        Value = "All"
                    });
                    foreach (ListItem item in ddlMultiOperationID.Items)
                    {
                        item.Selected = true;
                    }
                    //ddlMultiOperationID.Items.Insert(0, new ListItem
                    //{
                    //    Text = GetGlobalResourceObject("CommanResource", "ALL").ToString(),
                    //    Value = "All"
                    //});
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region BindNode
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
                }
                else
                {
                    trNodeID.Visible = false;
                }
            }
        }
        #endregion

        #region BreakDownTimeContent
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

        #region "Bind DownId Information"
        private void BindDownIdInfo()
        {
            try
            {
                var DownId = TMPTrakDataBase.GetDownIdInfo(Util.show16losses);
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

        #region BindMo
        private void BindMOIDS(string MOText, string type)
        {
            List<string> MOID = new List<string>();
            MOID = DataBaseAccess.GetMoIDS(MOText, type);
            if (MOID != null)
            {
                if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(MOText))
                {
                    MOID.Insert(0, "ALL");
                }
                ddlMoWise.DataSource = MOID;
                ddlMoWise.DataBind();
            }
        }
        #endregion

        #region BindDownTimeContent
        private void DownTimeContent()
        {
            ddlType.Items.FindByValue("MachineDownTimeMatrix").Enabled = true;
            ddlType.Items.FindByValue("MachinewiseDownTimeDetails").Enabled = true;
            ddlType.Items.FindByValue("ProdAndDownPie").Enabled = true;
            ddlType.Items.FindByValue("Hour").Enabled = false;
            ddlType.Items.FindByValue("Shift").Enabled = false;
            ddlType.Items.FindByValue("Daily").Enabled = false;
            ddlType.Items.FindByValue("Time-Consolidated").Enabled = false;
            ddlType.SelectedValue = "MachinewiseDownTimeDetails";
            trDownId.Visible = true;
            trTimeFormat.Visible = false;
            BindDownIdInfo();
        }
        #endregion

        #region PlantSelectionChange
        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlReportType.SelectedValue.ToString().Equals("EnergyReport", StringComparison.OrdinalIgnoreCase))
            {
                BindMachines();
                bindNode();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("AVGToolChangeReport", StringComparison.OrdinalIgnoreCase))
            {
                BindCellId();
                // BindMachines();
                BindToolsLifeData();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("ShiftwiseOperatorPerformance", StringComparison.OrdinalIgnoreCase))
            {
                BindMachines();
            }
            else
            if (ConfigurationManager.AppSettings["ShantiIronPages"].ToString().Equals("1") || ddlReportType.SelectedValue.ToString().Equals("TrelleborgOEEReport", StringComparison.OrdinalIgnoreCase))
            {
                BindCellId();
                // BindMachines();
                BindMachinesForPlantCell();
            }
            else if (isMachineMultiSelect())
            {
                BindCellId();
                //BindMachineIDListBox();
            }
            else if(ddlReportType.SelectedValue.ToString().Equals("ProductionPlantwiseReport_Dantal", StringComparison.OrdinalIgnoreCase))
            {
                BindCellId();
            }
            else
            {
                BindMachines();
            }
            
        }
        private bool isMachineMultiSelect()
        {
            bool isMultiSelect = false;
            if ((ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && ((ddlType.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format1EXCEL", StringComparison.OrdinalIgnoreCase))
                || (ddlType.SelectedValue.ToString().Equals("Shift", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format1", StringComparison.OrdinalIgnoreCase))
                || (ddlType.SelectedValue.ToString().Equals("Shift", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("OEEGraphicalReport", StringComparison.OrdinalIgnoreCase))
                || (ddlType.SelectedValue.ToString().Equals("Shift", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format3", StringComparison.OrdinalIgnoreCase))
                || (ddlType.SelectedValue.ToString().Equals("Time-Consolidated", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format1CockpitDataReport", StringComparison.OrdinalIgnoreCase))))

                ||

                (ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase) && (ddlType.SelectedValue.ToString().Equals("MachinewiseDownTimeDetails", StringComparison.OrdinalIgnoreCase) || ddlType.SelectedValue.ToString().Equals("MachineDownTimeMatrix", StringComparison.OrdinalIgnoreCase) || ddlType.SelectedValue.ToString().Equals("ProdAndDownPie", StringComparison.OrdinalIgnoreCase) ||
                  ddlType.SelectedValue.ToString().Equals("TimeAndFreqWise", StringComparison.OrdinalIgnoreCase)))

               || ddlReportType.SelectedValue.ToString().Equals("MachinewiseScrapReport", StringComparison.OrdinalIgnoreCase)
               || (ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportMachinewise", StringComparison.OrdinalIgnoreCase) && (ddlFormat.SelectedValue.ToString().Equals("Format2", StringComparison.OrdinalIgnoreCase)))

               || ddlReportType.SelectedValue.ToString().Equals("MonthlyOeeReportShantiFormat", StringComparison.OrdinalIgnoreCase)
               || ddlReportType.SelectedValue.ToString().Equals("DailyRejectionReport", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.ToString().Equals("ProductionHourlyReportLeonine", StringComparison.OrdinalIgnoreCase)
               || ddlReportType.SelectedValue.ToString().Equals("KKPillarReport", StringComparison.OrdinalIgnoreCase)
               || ddlReportType.SelectedValue.ToString().Equals("ShiftwiseOperatorPerformance", StringComparison.OrdinalIgnoreCase)
               || ddlReportType.SelectedValue.ToString().Equals("HydroTestReport", StringComparison.OrdinalIgnoreCase)|| ddlReportType.SelectedValue.ToString().Equals("ProductionPlantwiseReport_Dantal", StringComparison.OrdinalIgnoreCase))
            {
                isMultiSelect = true;
            }
            return isMultiSelect;
        }
        #endregion

        #region ReportTypeSelectionChange
        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //BindPlantId();
                trGroupID.Visible = false;
                trSerialNumber.Visible = false;
                trModel.Visible = false;
                trQRCode.Visible = false;
                lbCellID.Visible = false;
                lbMachineID.Visible = false;
                trSeperateSheet.Visible = false;
                trOperator.Visible = false;
                trMultiOperator.Visible = false;
                trToDate.Visible = true;
                trShift.Visible = true;
                ddlShift.Visible = true;
                trType.Visible = true;
                trMoID.Visible = false;
                trNodeID.Visible = false;
                trFormat.Visible = true;
                trPlant.Visible = true;
                trCellId.Visible = false;
                trMachine.Visible = true;
                trview.Visible = false;
                trComponentId.Visible = false;
                trOperationID.Visible = false;
                trDownId.Visible = false;
                trTimeFormat.Visible = false;
                ddlMachineId.Visible = true;
                ddlMultiMachineId.Visible = false;
                trDownReason.Visible = false;
                trfromdatetimeconsolidate.Visible = false;
                trtodatetimeconsolidate.Visible = false;
                trToDate.Visible = true;
                trmonthlydate.Visible = false;
                trToolNumber.Visible = false;
                txtMonth.Visible = false;
                trFromDate.Visible = true;
                trYear.Visible = false;
                trMultiComponent.Visible = false;
                BindMachines();
                BindMOIDS("", "");
                trmivintype.Visible = false;
                trProcessType.Visible = false;
                reportTypeForVulkanAM.Visible = false;
                ddlFormat.Items.FindByValue("Format2").Enabled = false;
                ddlType.Items.FindByValue("MachineDownTimeMatrix").Enabled = false;
                ddlType.Items.FindByValue("MachinewiseDownTimeDetails").Enabled = false;
                ddlType.Items.FindByValue("ProdAndDownPie").Enabled = false;
                ddlType.Items.FindByValue("Hour").Enabled = false;
                ddlType.Items.FindByValue("Shift").Enabled = true;
                ddlType.Items.FindByValue("Daily").Enabled = true;
                ddlType.Items.FindByValue("TimeWise").Enabled = false;
                ddlType.Items.FindByValue("TimeAndFreqWise").Enabled = false;
                trBreakDown.Visible = false;
                ddlType.Items.FindByValue("Time-Consolidated").Enabled = true;
                if (ddlCellID.Items.FindByValue("All") == null)
                    ddlCellID.Items.Insert(0, "All");
                BindCellId();
                BindMachineIDListBox();
                ddlCellID.Visible = true;
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase))
                {
                    ddlType.SelectedValue = "Shift";
                    trCellId.Visible = true;
                    ddlFormat.Items.FindByValue("Format1").Enabled = true;
                    ddlFormat.SelectedValue = "Format1";
                    ddlFormat.Items.FindByValue("Format1CockpitDataReport").Enabled = false;
                    ddlFormat.Items.FindByValue("Format1EXCEL").Enabled = false;
                    if (ConfigurationManager.AppSettings["GEAPages"].ToString().Equals("1"))
                    {
                        ddlFormat.Items.FindByValue("Format3").Enabled = false;
                    }
                    else
                    {
                        ddlFormat.Items.FindByValue("Format3").Enabled = true;
                    }
                    ddlFormat.Items.FindByValue("Format4").Enabled = false;
                    ddlFormat.Items.FindByValue("OEEGraphicalReport").Enabled = true;
                    ddlFormat.Items.FindByValue("Format2").Enabled = false;
                    ddlType.Visible = true;
                    ddlMultiMachineId.Visible = false;
                    if (ddlType.SelectedValue.ToString().Equals("Time-Consolidated", StringComparison.OrdinalIgnoreCase))
                    {
                        ddlFormat.Items.FindByValue("Format1").Enabled = false;
                        ddlFormat.Items.FindByValue("Format3").Enabled = false;
                        ddlFormat.Items.FindByValue("Format1CockpitDataReport").Enabled = true;
                        ddlFormat.Items.FindByValue("OEEGraphicalReport").Enabled = false;
                        trfromdatetimeconsolidate.Visible = true;
                        trtodatetimeconsolidate.Visible = true;
                        trCellId.Visible = true;
                        trToDate.Visible = false;
                        trFromDate.Visible = false;
                        trShift.Visible = false;
                    }
                    else
                    {
                        trfromdatetimeconsolidate.Visible = false;
                        trtodatetimeconsolidate.Visible = false;
                        trToDate.Visible = true;
                        trFromDate.Visible = true;
                        if (ddlType.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase) || ddlFormat.SelectedValue.ToString().Equals("Format4", StringComparison.OrdinalIgnoreCase))
                        {
                            trCellId.Visible = true;
                            ddlFormat.Items.FindByValue("Format1").Enabled = false;
                            ddlFormat.Items.FindByValue("Format3").Enabled = false;
                            ddlFormat.Items.FindByValue("OEEGraphicalReport").Enabled = false;
                            ddlFormat.Items.FindByValue("Format1EXCEL").Enabled = true;
                            trShift.Visible = false;

                        }
                    }
                    if (isMachineMultiSelect())
                    {
                        ddlMachineId.Visible = false;
                        lbMachineID.Visible = true;
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
                else if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportComponentwise", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = false;
                    trToDate.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trFormat.Visible = true;
                    ddlFormat.Items.FindByValue("Format1").Enabled = true;
                    ddlFormat.Items.FindByValue("Format1CockpitDataReport").Enabled = false;
                    ddlFormat.Items.FindByValue("Format1EXCEL").Enabled = false;
                    ddlFormat.Items.FindByValue("Format3").Enabled = false;
                    ddlFormat.Items.FindByValue("Format4").Enabled = false;
                    ddlFormat.Items.FindByValue("OEEGraphicalReport").Enabled = false;
                    ddlFormat.Items.FindByValue("Format2").Enabled = true;

                    trPlant.Visible = false;
                    trCellId.Visible = false;
                    trMachine.Visible = false;
                    trComponentId.Visible = true;
                    trOperationID.Visible = true;
                    trfromdatetimeconsolidate.Visible = true;
                    trtodatetimeconsolidate.Visible = true;
                    BindComponentID();
                    ddlComponentId_SelectedIndexChanged(null, null);
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportMachinewise", StringComparison.OrdinalIgnoreCase))
                {
                    trType.Visible = false;
                    trFormat.Visible = true;
                    trToDate.Visible = false;
                    trComponentId.Visible = true;
                    trShift.Visible = true;
                    trOperationID.Visible = true;
                    ddlOperationID.Visible = false;
                    ddlMultiOperationID.Visible = true;
                    BindComponentID();
                    ddlComponentId_SelectedIndexChanged(null, null);
                    trType.Visible = false;
                    ddlMachineId.Items.Remove("MachineAll");

                    ddlFormat.Items.FindByValue("Format1").Enabled = true;
                    ddlFormat.Items.FindByValue("Format1CockpitDataReport").Enabled = false;
                    ddlFormat.Items.FindByValue("Format1EXCEL").Enabled = false;
                    ddlFormat.Items.FindByValue("Format3").Enabled = false;
                    ddlFormat.Items.FindByValue("Format4").Enabled = false;
                    ddlFormat.Items.FindByValue("OEEGraphicalReport").Enabled = false;
                    ddlFormat.Items.FindByValue("Format2").Enabled = true;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportDailyByHour", StringComparison.OrdinalIgnoreCase))
                {
                    trType.Visible = false;
                    trShift.Visible = false;
                    trFormat.Visible = false;
                    trComponentId.Visible = false;
                    trOperationID.Visible = false;
                    //BindComponentID();
                    //ddlComponentId_SelectedIndexChanged(null, null);
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("MISReport", StringComparison.OrdinalIgnoreCase))
                {
                    trType.Visible = false;
                    trShift.Visible = false;
                    trComponentId.Visible = false;
                    trOperationID.Visible = false;
                    trDownId.Visible = false;
                    trDownReason.Visible = false;
                    trTimeFormat.Visible = false;
                    trFormat.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFormat.Visible = true;
                    trFormat.Visible = false;
                    ddlType.Visible = true;
                    ddlFormat.Items.FindByValue("Format1").Enabled = false;
                    ddlFormat.Items.FindByValue("OEEGraphicalReport").Enabled = false;
                    ddlFormat.Items.FindByValue("Format3").Enabled = false;
                    ddlFormat.Items.FindByValue("Format4").Enabled = false;
                    ddlFormat.Items.FindByValue("Format2").Enabled = false;
                    ddlType.Items.FindByValue("TimeWise").Enabled = false;
                    ddlType.Items.FindByValue("TimeAndFreqWise").Enabled = true;
                    trShift.Visible = false;
                    DownTimeContent();
                    ddlMachineId.Visible = true;
                    ddlMultiMachineId.Visible = false;
                    trDownReason.Visible = false;
                    trFromDate.Visible = false;
                    trfromdatetimeconsolidate.Visible = true;
                    trToDate.Visible = false;
                    trtodatetimeconsolidate.Visible = true;
                    // if (ddlType.SelectedValue.ToString().Equals("MachinewiseDownTimeDetails", StringComparison.OrdinalIgnoreCase))
                    // {
                    if (ConfigurationManager.AppSettings["IndiaNippon"].ToString() != "1")
                    {
                        trSeperateSheet.Visible = true;
                    }
                    // }
                    trCellId.Visible = true;
                    if (isMachineMultiSelect())
                    {
                        ddlMachineId.Visible = false;
                        lbMachineID.Visible = true;
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
                else if (ddlReportType.SelectedValue.ToString().Equals("ToolChangeFrequencyReport", StringComparison.OrdinalIgnoreCase))
                {
                    trDownId.Visible = false;
                    trComponentId.Visible = false;
                    trType.Visible = false;
                    trShift.Visible = false;
                    trOperationID.Visible = false;
                    trFormat.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("MOWISEREPORT", StringComparison.OrdinalIgnoreCase))
                {
                    ddlType.Visible = false;
                    trType.Visible = false;
                    trShift.Visible = false;
                    trComponentId.Visible = false;
                    trMoID.Visible = true;
                    trOperationID.Visible = false;
                    trDownId.Visible = false;
                    trDownReason.Visible = false;
                    trTimeFormat.Visible = false;
                    trFormat.Visible = false;
                    BindMOIDS("", "");
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("EnergyReport", StringComparison.OrdinalIgnoreCase))
                {
                    trview.Visible = true;
                    trType.Visible = true;
                    trFormat.Visible = false;
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trPlant.Visible = true;
                    trCellId.Visible = false;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    trShift.Visible = false;
                    trComponentId.Visible = false;
                    trOperationID.Visible = false;
                    trDownId.Visible = false;
                    trDownReason.Visible = false;
                    trNodeID.Visible = true;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("EnergyReportSona", StringComparison.OrdinalIgnoreCase))
                {
                    trview.Visible = false;
                    trType.Visible = false;
                    trFormat.Visible = false;
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trPlant.Visible = false;
                    trCellId.Visible = false;
                    trMachine.Visible = false;
                    trShift.Visible = false;
                    trComponentId.Visible = false;
                    trOperationID.Visible = false;
                    trDownId.Visible = false;
                    trDownReason.Visible = false;
                    trNodeID.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("ToolChangeReport", StringComparison.OrdinalIgnoreCase))
                {
                    trview.Visible = false;
                    trType.Visible = false;
                    trFormat.Visible = false;
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trPlant.Visible = true;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    trShift.Visible = false;
                    trComponentId.Visible = false;
                    trOperationID.Visible = false;
                    trDownId.Visible = false;
                    trDownReason.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("BreakdownPhenomena", StringComparison.OrdinalIgnoreCase))
                {
                    //trType.Visible = false;
                    trFormat.Visible = false;
                    //ddlType.Visible = true;
                    trType.Visible = false;
                    //ddlFormat.Visible = false;
                    //ddlShift.Visible = false;
                    BreakDownTimeContent();
                    ddlMachineId.Visible = true;
                    ddlMultiMachineId.Visible = false;
                    trDownReason.Visible = false;
                    trType.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("Hytechreport", StringComparison.OrdinalIgnoreCase))
                {
                    ddlType.Visible = false;
                    trType.Visible = false;
                    trToDate.Visible = false;
                    trFromDate.Visible = false;
                    trShift.Visible = false;
                    trComponentId.Visible = false;
                    trOperationID.Visible = false;
                    trDownId.Visible = false;
                    trDownReason.Visible = false;
                    trTimeFormat.Visible = false;
                    trFormat.Visible = false;
                    trmonthlydate.Visible = true;
                    txtMonth.Visible = true;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("FlowMeterReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trPlant.Visible = true;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;

                    trShift.Visible = true;
                    trType.Visible = false;
                    trMoID.Visible = false;
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

                }
                else if (ddlReportType.SelectedValue.ToString().Equals("ShanthiProdReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trPlant.Visible = true;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;

                    trShift.Visible = true;
                    trType.Visible = false;
                    trMoID.Visible = false;
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

                }
                else if (ddlReportType.SelectedValue.ToString().Equals("DailyRejectionReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trPlant.Visible = true;
                    trCellId.Visible = true;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;

                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
                    trNodeID.Visible = false;
                    trFormat.Visible = false;
                    trview.Visible = false;
                    trComponentId.Visible = true;
                    BindComponentID();
                    trOperationID.Visible = false;
                    trDownId.Visible = false;
                    trTimeFormat.Visible = false;
                    ddlMultiMachineId.Visible = false;
                    trDownReason.Visible = false;
                    trfromdatetimeconsolidate.Visible = false;
                    trtodatetimeconsolidate.Visible = false;
                    trmonthlydate.Visible = false;

                    if (isMachineMultiSelect())
                    {
                        ddlMachineId.Visible = false;
                        lbMachineID.Visible = true;
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
                else if (ddlReportType.SelectedValue.ToString().Equals("HelpRequest", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trPlant.Visible = true;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;

                    trShift.Visible = true;
                    trType.Visible = false;
                    trMoID.Visible = false;
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

                }
                else if (ddlReportType.SelectedValue.ToString().Equals("AVGToolChangeReport", StringComparison.OrdinalIgnoreCase))
                {
                    BindPlantId();
                    //BindToolsLifeData();
                    // ddlMachineId.Items.RemoveAt(0);
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trPlant.Visible = true;
                    trCellId.Visible = true;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    trToolNumber.Visible = true;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
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

                }
                else if (ddlReportType.SelectedValue.ToString().Equals("HourlyPartCount", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = false;
                    trPlant.Visible = false;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    trToolNumber.Visible = false;
                    trShift.Visible = true;
                    trType.Visible = false;
                    trMoID.Visible = false;
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
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("OperatorEffeciencyReport", StringComparison.OrdinalIgnoreCase))
                {
                    trPlant.Visible = true;
                    trCellId.Visible = false;
                    trFromDate.Visible = false;
                    trToDate.Visible = false;
                    trOperator.Visible = true;
                    trMachine.Visible = false;
                    trMachine.Visible = false;
                    ddlMachineId.Visible = false;
                    trToolNumber.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
                    trNodeID.Visible = false;
                    trFormat.Visible = false;
                    trview.Visible = false;
                    trComponentId.Visible = false;
                    trOperationID.Visible = false;
                    trDownId.Visible = false;
                    trTimeFormat.Visible = false;
                    ddlMultiMachineId.Visible = false;
                    trDownReason.Visible = false;
                    trfromdatetimeconsolidate.Visible = true;
                    trtodatetimeconsolidate.Visible = true;
                    trmonthlydate.Visible = false;

                    if (ConfigurationManager.AppSettings["KachMotors"].ToString().Equals("1"))
                    {
                        trFormat.Visible = true;
                        ddlFormat.Items.FindByValue("Format1").Enabled = true;
                        ddlFormat.Items.FindByValue("Format1CockpitDataReport").Enabled = false;
                        ddlFormat.Items.FindByValue("Format1EXCEL").Enabled = false;
                        ddlFormat.Items.FindByValue("Format3").Enabled = false;
                        ddlFormat.Items.FindByValue("Format4").Enabled = false;
                        ddlFormat.Items.FindByValue("OEEGraphicalReport").Enabled = false;
                        ddlFormat.Items.FindByValue("Format2").Enabled = true;
                    }
                    else
                        ddlFormat.SelectedValue = "Format1";
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("MonthlyOeeReportShantiFormat", StringComparison.OrdinalIgnoreCase))
                {
                    trPlant.Visible = true;
                    trCellId.Visible = true;
                    trFromDate.Visible = false;
                    trToDate.Visible = false;
                    trOperator.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    trToolNumber.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
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
                    trmonthlydate.Visible = true;
                    txtMonth.Visible = true;

                    if (isMachineMultiSelect())
                    {
                        ddlMachineId.Visible = false;
                        lbMachineID.Visible = true;
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
                else if (ddlReportType.SelectedValue.ToString().Equals("RejectionORReworkReportShantiFormat", StringComparison.OrdinalIgnoreCase))
                {
                    trPlant.Visible = true;
                    trCellId.Visible = true;
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trOperator.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    trToolNumber.Visible = false;
                    trShift.Visible = true;
                    trType.Visible = false;
                    trMoID.Visible = false;
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
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("PMReport", StringComparison.OrdinalIgnoreCase))
                {
                    trPlant.Visible = true;
                    trCellId.Visible = true;
                    trFromDate.Visible = false;
                    trToDate.Visible = false;
                    trOperator.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    trToolNumber.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
                    BindMachinesForPlantCell();
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
                    trYear.Visible = true;

                }
                else if (ddlReportType.SelectedValue.ToString().Equals("ProddutionReportGEA", StringComparison.OrdinalIgnoreCase))
                {
                    trPlant.Visible = true;
                    trCellId.Visible = true;
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trOperator.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    trToolNumber.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
                    BindMachinesForPlantCell();
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
                    trYear.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("MivinInspectionReport"))
                {
                    trPlant.Visible = false;
                    trCellId.Visible = false;
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trOperator.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    trToolNumber.Visible = false;
                    trShift.Visible = true;
                    trType.Visible = false;
                    trMoID.Visible = false;
                    BindMachinesForPlantCell();
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
                    trYear.Visible = false;
                    trmivintype.Visible = true;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("PMReportAAAPL"))
                {
                    trPlant.Visible = true;
                    trCellId.Visible = true;
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trOperator.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    trToolNumber.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
                    BindMachinesForPlantCell();
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
                    trYear.Visible = false;
                    trmivintype.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("TrelleborgOEEReport", StringComparison.OrdinalIgnoreCase))
                {
                    ddlType.Visible = false;
                    ddlShift.Visible = false;
                    trPlant.Visible = true;
                    trCellId.Visible = true;
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trOperator.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    trToolNumber.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
                    BindMachinesForPlantCell();
                    trNodeID.Visible = false;
                    trFormat.Visible = true;
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
                    trYear.Visible = false;
                    trmivintype.Visible = false;
                    ddlFormat.Items.FindByValue("Format1").Enabled = true;
                    ddlFormat.Items.FindByValue("Format1CockpitDataReport").Enabled = false;
                    ddlFormat.Items.FindByValue("Format1EXCEL").Enabled = false;
                    ddlFormat.Items.FindByValue("Format3").Enabled = false;
                    ddlFormat.Items.FindByValue("Format4").Enabled = false;
                    ddlFormat.Items.FindByValue("OEEGraphicalReport").Enabled = false;
                    ddlFormat.Items.FindByValue("Format2").Enabled = true;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("HourwiseOperatorIncentiveReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trPlant.Visible = false;
                    trCellId.Visible = false;
                    trMachine.Visible = false;
                    ddlMachineId.Visible = false;
                    trOperator.Visible = true;

                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
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

                }
                else if (ddlReportType.SelectedValue.ToString().Equals("OperatorDrawingNoReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = false;
                    trToDate.Visible = false;
                    trPlant.Visible = false;
                    trCellId.Visible = false;
                    trMachine.Visible = false;
                    ddlMachineId.Visible = false;
                    trOperator.Visible = false;
                    trMultiOperator.Visible = true;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
                    trNodeID.Visible = false;
                    trFormat.Visible = false;
                    trview.Visible = false;
                    trComponentId.Visible = false;
                    trOperationID.Visible = false;
                    trDownId.Visible = false;
                    trTimeFormat.Visible = false;
                    ddlMultiMachineId.Visible = false;
                    trDownReason.Visible = false;
                    trfromdatetimeconsolidate.Visible = true;
                    trtodatetimeconsolidate.Visible = true;
                    trmonthlydate.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("DataTraceabilityReportAdvikPanth", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trModel.Visible = true;
                    trQRCode.Visible = true;
                    trPlant.Visible = false;
                    trCellId.Visible = false;
                    trMachine.Visible = false;
                    ddlMachineId.Visible = false;
                    trOperator.Visible = false;
                    trMultiOperator.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
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
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("MachineReworkReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trModel.Visible = false;
                    trQRCode.Visible = false;
                    trPlant.Visible = false;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    trOperator.Visible = false;
                    trMultiOperator.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
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
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("MachinewiseScrapReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trModel.Visible = false;
                    trQRCode.Visible = false;
                    trPlant.Visible = false;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    trOperator.Visible = false;
                    trMultiOperator.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
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

                    if (isMachineMultiSelect())
                    {
                        ddlMachineId.Visible = false;
                        lbMachineID.Visible = true;
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
                else if (ddlReportType.SelectedValue.ToString().Equals("ComponentSetupReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trModel.Visible = false;
                    trQRCode.Visible = false;
                    trPlant.Visible = true;
                    trCellId.Visible = true;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    trOperator.Visible = false;
                    trMultiOperator.Visible = true;
                    BindComponentID();
                    trMultiComponent.Visible = true;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
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

                }
                else if (ddlReportType.SelectedValue.ToString().Equals("ProductionHourlyReportLeonine", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trModel.Visible = false;
                    trQRCode.Visible = false;
                    trPlant.Visible = false;
                    trCellId.Visible = true;
                    trMachine.Visible = true;
                    // ddlMachineId.Visible = false;
                    trOperator.Visible = false;
                    trMultiOperator.Visible = false;
                    trShift.Visible = true;
                    trType.Visible = false;
                    trMoID.Visible = false;
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
                    //lbCellID.Visible = true;
                    //ddlCellID.Visible = false;


                    if (isMachineMultiSelect())
                    {
                        ddlMachineId.Visible = false;
                        lbMachineID.Visible = true;
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
                else if (ddlReportType.SelectedValue.ToString().Equals("KKPillarReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trModel.Visible = false;
                    trQRCode.Visible = false;
                    trPlant.Visible = true;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    trOperator.Visible = false;
                    trMultiOperator.Visible = false;
                    BindComponentID();
                    trMultiComponent.Visible = false;
                    trShift.Visible = true;
                    ddlShift.Visible = true;
                    trType.Visible = false;
                    trMoID.Visible = false;
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

                    if (isMachineMultiSelect())
                    {
                        ddlMachineId.Visible = false;
                        lbMachineID.Visible = true;
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
                else if (ddlReportType.SelectedValue.ToString().Equals("ShiftwiseOperatorPerformance", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trModel.Visible = false;
                    trQRCode.Visible = false;
                    trPlant.Visible = true;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = false;
                    trOperator.Visible = true;
                    trMultiOperator.Visible = false;
                    BindComponentID();
                    trMultiComponent.Visible = false;
                    trShift.Visible = true;
                    ddlShift.Visible = true;
                    trType.Visible = false;
                    trMoID.Visible = false;
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

                    if (isMachineMultiSelect())
                    {
                        ddlMachineId.Visible = false;
                        lbMachineID.Visible = true;
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
               

                else if (ddlReportType.SelectedValue.ToString().Equals("HydroTestReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trModel.Visible = false;
                    trQRCode.Visible = false;
                    trPlant.Visible = false;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = false;
                    trOperator.Visible = false;
                    trMultiOperator.Visible = false;
                    BindComponentID();
                    trMultiComponent.Visible = false;
                    trShift.Visible = false;
                    ddlShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
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
                    trProcessType.Visible = true;

                    if (isMachineMultiSelect())
                    {
                        ddlMachineId.Visible = false;
                        lbMachineID.Visible = true;
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
                else if (ddlReportType.SelectedValue.ToString().Equals("SpindleIdleTimeAnalysisReportLnTOdisha", StringComparison.OrdinalIgnoreCase))
                {
                    trFormat.Visible = false;
                    trType.Visible = false;
                    trToDate.Visible = false;
                    trShift.Visible = true;
                    ddlMachineId.Items.Remove("MachineAll");
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("MachineUtilizationReportACE", StringComparison.OrdinalIgnoreCase))
                {
                    txtFromDate.Text = DateTime.Now.AddDays(-6).ToString("dd-MM-yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    trFormat.Visible = false;
                    trType.Visible = false;
                    trToDate.Visible = false;
                    trShift.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Items.RemoveAt(0);
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("PDIReportTafeChennai", StringComparison.OrdinalIgnoreCase))
                {
                    trFormat.Visible = false;
                    trType.Visible = false;
                    trToDate.Visible = false;
                    trFromDate.Visible = false;
                    trShift.Visible = false;
                    trMachine.Visible = false;
                    trPlant.Visible = false;
                    trSerialNumber.Visible = true;
                    lnkSlnoSearch_Click(null, null);
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("ToolChangeFrequencyRecord", StringComparison.OrdinalIgnoreCase))
                {
                    trToDate.Visible = true;
                    trFromDate.Visible = true;
                    trMachine.Visible = true;
                    ddlMultiMachineId.Visible = true;
                    ddlMachineId.Visible = false;
                    trType.Visible = false;
                    trShift.Visible = false;
                    trFormat.Visible = false;
                    trPlant.Visible = true;
                }
                else if (ddlReportType.SelectedValue.ToString().Trim().Equals("InspectionReportVulkan", StringComparison.OrdinalIgnoreCase))
                {
                    trToDate.Visible = false;
                    trFromDate.Visible = true;
                    ddlMultiMachineId.Visible = true;
                    trMachine.Visible = true;
                    trShift.Visible = true;
                    trType.Visible = false;
                    trFormat.Visible = false;
                    ddlMachineId.Visible = false;
                    trPlant.Visible = false;
                    BindShiftData();
                }
                else if (ddlReportType.SelectedValue.ToString().Trim().Equals("PMTransactionReport", StringComparison.OrdinalIgnoreCase))
                {
                    trYear.Visible = false;
                    trmonthlydate.Visible = true;
                    txtMonth.Visible = true;
                    ddlMultiMachineId.Visible = true;
                    trMachine.Visible = true;
                    trToDate.Visible = false;
                    trFromDate.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trFormat.Visible = false;
                    ddlMachineId.Visible = false;
                    trPlant.Visible = true;
                }
                else if (ddlReportType.SelectedValue.ToString().Trim().Equals("CycleTimeReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trMachine.Visible = true;
                    trType.Visible = false;
                    trShift.Visible = false;
                    lbMachineID.Visible = false;
                    ddlMachineId.Visible = true;
                    trFormat.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Trim().Equals("ToolLifeReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    lbMachineID.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trFormat.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Trim().Equals("AMTransactionReport", StringComparison.OrdinalIgnoreCase))
                {
                    trYear.Visible = false;
                    trmonthlydate.Visible = true;
                    txtMonth.Visible = true;
                    ddlMultiMachineId.Visible = true;
                    trMachine.Visible = true;
                    trToDate.Visible = false;
                    trFromDate.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trFormat.Visible = false;
                    ddlMachineId.Visible = false;
                    trPlant.Visible = true;
                    reportTypeForVulkanAM.Visible = true;
                }
                else if (ddlReportType.SelectedValue.ToString().Trim().Equals("ERPPerformanceReportSKS", StringComparison.OrdinalIgnoreCase))
                {
                    ddlMultiMachineId.Visible = false;
                    trFromDate.Visible = false;
                    trPlant.Visible = true;
                    ddlMachineId.Visible = true;
                    trToDate.Visible = true;
                    trType.Visible = false;
                    trShift.Visible = false;
                    trFormat.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Trim().Equals("PMTransactionReportLnTOdisha", StringComparison.OrdinalIgnoreCase))
                {
                    trYear.Visible = true;
                    trmonthlydate.Visible = false;
                    ddlMultiMachineId.Visible = false;
                    trMachine.Visible = true;
                    trToDate.Visible = false;
                    trFromDate.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trFormat.Visible = false;
                    ddlMachineId.Visible = true;
                    trPlant.Visible = true;
                    BindMachines();
                }
                else if (ddlReportType.SelectedValue.ToString().Trim().Equals("InspectionReport_Rexnord", StringComparison.OrdinalIgnoreCase))
                {
                    trSerialNumber.Visible = true;
                    txtSlnoSearch.Visible = false;
                    lnkSlnoSearch.Visible = false;
                    trOperationID.Visible = true;
                    trToDate.Visible = false;
                    trFromDate.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trFormat.Visible = false;
                    trMachine.Visible = false;
                    trPlant.Visible = false;
                    ddlOperationID.Visible = false;
                    ddlMultiOperationID.Visible = true;
                    BindSlNo();
                    BindOperationNo("");
                }
                else if (ddlReportType.SelectedValue.ToString().Trim().Equals("ProductionReportMachinewise_PrecisionEngg", StringComparison.OrdinalIgnoreCase))
                {
                    trType.Visible = false;
                    trFormat.Visible = false;
                    trComponentId.Visible = false;
                    trShift.Visible = true;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    ddlMultiMachineId.Visible = false;
                    trOperationID.Visible = false;
                    ddlOperationID.Visible = false;
                    ddlMultiOperationID.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Trim().Equals("ProductionReportComponentwise_PrecisionEngg", StringComparison.OrdinalIgnoreCase))
                {
                    trType.Visible = false;
                    trFormat.Visible = false;
                    trComponentId.Visible = true;
                    trShift.Visible = false;
                    trOperationID.Visible = false;
                    ddlOperationID.Visible = false;
                    ddlMultiOperationID.Visible = false;
                    trPlant.Visible = false;
                    trMachine.Visible = false;
                    trCellId.Visible = false;
                    BindComponentID();
                }
                else if (ddlReportType.SelectedValue.ToString().Trim().Equals("DailyRejectionReport_PrecisionEngg", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trPlant.Visible = true;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;

                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
                    trNodeID.Visible = false;
                    trFormat.Visible = false;
                    trview.Visible = false;
                    trComponentId.Visible = true;
                    BindComponentID();
                    trOperationID.Visible = false;
                    trDownId.Visible = false;
                    trTimeFormat.Visible = false;
                    ddlMultiMachineId.Visible = false;
                    trDownReason.Visible = false;
                    trfromdatetimeconsolidate.Visible = false;
                    trtodatetimeconsolidate.Visible = false;
                    trmonthlydate.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Trim().Equals("OperatorEfficiencyReport_PrecisionEngg", StringComparison.OrdinalIgnoreCase))
                {
                    trType.Visible = false;
                    trFormat.Visible = false;
                    trComponentId.Visible = false;
                    trShift.Visible = false;
                    trOperator.Visible = true;
                    trPlant.Visible = false;
                    trMachine.Visible = false;
                    trCellId.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Trim().Equals("DailyCleaningandMaintenanceReport_PrecisionEngg", StringComparison.OrdinalIgnoreCase))
                {
                    trType.Visible = false;
                    trFormat.Visible = false;
                    trComponentId.Visible = false;
                    trShift.Visible = false;
                    trOperator.Visible = false;
                    trPlant.Visible = false;
                    trMachine.Visible = true;
                    ddlMultiMachineId.Visible = false;
                    lbMachineID.Visible = true;
                    ddlMachineId.Visible = false;
                    trCellId.Visible = false;
                    trGroupID.Visible = true;
                    trFromDate.Visible = true;
                    trToDate.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Trim().Equals("InspectionApprovalReport_Highway", StringComparison.OrdinalIgnoreCase))
                {
                    //trDieNo.Visible = true;
                    //ddlRevID.DataSource = DataBaseAccess.GetRevID(ddlComponentId.SelectedValue.ToString(), ddlOperationID.SelectedValue.ToString());
                    //ddlRevID.DataBind();
                    //trHeatNo.Visible = true;
                    //ddlHeatNo.DataSource = DataBaseAccess.GetHeatNo(ddlMachineId.SelectedValue.ToString(),ddlComponentId.SelectedValue.ToString(), ddlOperationID.SelectedValue.ToString(),ddlShift.SelectedValue.ToString(),);
                    //ddlHeatNo.DataBind();
                    //trrevid.Visible = true;
                    //ddlDieNo.DataSource = DataBaseAccess.GetDieNo(ddlComponentId.SelectedValue.ToString(), ddlOperationID.SelectedValue.ToString());
                    //ddlDieNo.DataBind();
                }
                else if (ddlReportType.SelectedValue.ToString().Trim().Equals("ChecksheetApprovalReport_Highway", StringComparison.OrdinalIgnoreCase))
                {
                    trDieNo.Visible = true;
                    trHeatNo.Visible = true;
                    trrevid.Visible = true;
                }
                //else if (ddlReportType.SelectedValue.ToString().Equals("ComponentStandardCycltimeComparison", StringComparison.OrdinalIgnoreCase))
                //{
                //    trToDate.Visible = true;
                //    trFromDate.Visible = true;
                //    trMachine.Visible = true;
                //    ddlMultiMachineId.Visible = false;
                //    ddlMachineId.Visible = true;
                //    trMultiComponent.Visible = false;
                //    trComponentId.Visible = true;
                //    txtComponent.Visible = false;
                //    btnComponent.Visible = false;
                //    trOperationID.Visible = true;
                //    ddlOperationID.Visible = true;
                //    ddlMultiOperationID.Visible = false;
                //    trType.Visible = false;
                //    trShift.Visible = false;
                //    trFormat.Visible = false;
                //    trPlant.Visible = true;
                //    BindComponentID();
                //}
                else if (ddlReportType.SelectedValue.ToString().Trim().Equals("MaintenanceParticularReport_PrecisionEngg", StringComparison.OrdinalIgnoreCase))
                {
                    trType.Visible = false;
                    trFormat.Visible = false;
                    trReportType.Visible = true;
                    trComponentId.Visible = false;
                    trShift.Visible = false;
                    trOperator.Visible = false;
                    trPlant.Visible = false;
                    trMachine.Visible = true;
                    ddlMultiMachineId.Visible = false;
                    ddlMachineId.Visible = false;
                    lbMachineID.Visible = true;
                    trCellId.Visible = false;
                    //trToDate.Visible = false;
                }

                else if (ddlReportType.SelectedValue.ToString().Equals("ProductionPlantwiseReport_Dantal"))
                {
                    trPlant.Visible = true;
                    trCellId.Visible = true;
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trOperator.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = false;
                    trToolNumber.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
                    trNodeID.Visible = false;
                    trFormat.Visible = false;
                    trview.Visible = false;
                    trComponentId.Visible = false;
                    trOperationID.Visible = false;
                    trDownId.Visible = false;
                    trTimeFormat.Visible = false;
                    //ddlMultiMachineId.Visible = true;
                    trDownReason.Visible = false;
                    trfromdatetimeconsolidate.Visible = false;
                    trtodatetimeconsolidate.Visible = false;
                    trmonthlydate.Visible = false;
                    trYear.Visible = false;
                    trmivintype.Visible = false;
                    BindPlantId();
                    BindCellId();
                    BindMachineIDListBox();
                    if (isMachineMultiSelect())
                    {
                        ddlMachineId.Visible = false;
                        lbMachineID.Visible = true;
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
                setPlantID();
                setFromDateLabel();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region ViewSelectionChange
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
        #endregion

        #region ComponentSelectionChange
        protected void ddlComponentId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindOperationNo(ddlComponentId.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region TypeSelectionChange
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                trSeperateSheet.Visible = false;
                trOperator.Visible = false;
                trToDate.Visible = true;
                trShift.Visible = true;
                trType.Visible = true;
                trFormat.Visible = true;
                ddlMachineId.Visible = true;
                trDownReason.Visible = false;
                ddlMultiMachineId.Visible = false;
                txtMonth.Visible = true;
                trfromdatetimeconsolidate.Visible = false;
                trtodatetimeconsolidate.Visible = false;
                trMoID.Visible = false;
                trToDate.Visible = true;
                trmivintype.Visible = false;
                if (ddlCellID.Items.FindByValue("All") == null)
                    ddlCellID.Items.Insert(0, "All");
                BindCellId();
                trFromDate.Visible = true;
                trToolNumber.Visible = false;
                ddlFormat.Items.FindByValue("Format1").Enabled = false;
                ddlFormat.Items.FindByValue("OEEGraphicalReport").Enabled = false;
                ddlFormat.Items.FindByValue("Format1EXCEL").Enabled = false;
                ddlFormat.Items.FindByValue("Format2").Enabled = false;
                ddlFormat.Items.FindByValue("Format3").Enabled = false;
                if (ConfigurationManager.AppSettings["sonapages"].ToString() == "1")
                    ddlFormat.Items.FindByValue("SONABLW").Enabled = false;
                //BindPlantId();
                ddlFormat.Items.FindByValue("Format1CockpitDataReport").Enabled = false;
                ddlFormat.Items.FindByValue("Format4").Enabled = false;
                if (ddlType.SelectedValue.ToString().Equals("Hour", StringComparison.OrdinalIgnoreCase))
                    trToDate.Visible = false;
                if (ddlType.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase))
                {

                    if (ddlFormat.Items.FindByValue("TimeConsolidatedShanthiFormat") != null)
                    {
                        ddlFormat.Items.FindByValue("TimeConsolidatedShanthiFormat").Enabled = false;
                    }
                    trShift.Visible = false;
                    ddlFormat.Items.FindByValue("Format1").Enabled = false;
                    ddlFormat.Items.FindByValue("OEEGraphicalReport").Enabled = false;
                    ddlFormat.Items.FindByValue("Format1EXCEL").Enabled = true;
                    if (ddlFormat.Items.Count > 0)
                    {
                        if (ddlFormat.Items.FindByValue("Format1EXCEL") != null)
                        {
                            ddlFormat.SelectedValue = "Format1EXCEL";
                        }
                    }
                    if (ddlFormat.SelectedValue.ToString().Equals("Format1")) trCellId.Visible = true;
                    if (ddlReportType.SelectedValue == "ProductionReportMachinewise")
                    {
                        ddlFormat.Items.FindByValue("Format3").Enabled = false;
                        trCellId.Visible = true;
                    }
                    if (ConfigurationManager.AppSettings["sonapages"].ToString() == "1")
                        ddlFormat.Items.FindByValue("SONABLW").Enabled = true;
                    if (ConfigurationManager.AppSettings["KiswokPage"].ToString() == "1")
                    {
                        ddlFormat.Items.FindByValue("Format4").Enabled = true;
                    }
                    if (isMachineMultiSelect())
                    {
                        ddlMachineId.Visible = false;
                        lbMachineID.Visible = true;
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
                if (ddlType.SelectedValue.ToString().Equals("Shift", StringComparison.OrdinalIgnoreCase))
                {
                    if (ddlFormat.Items.FindByValue("TimeConsolidatedShanthiFormat") != null)
                    {
                        ddlFormat.Items.FindByValue("TimeConsolidatedShanthiFormat").Enabled = false;
                    }
                    ddlFormat.Items.FindByValue("Format1").Enabled = true;
                    ddlFormat.Items.FindByValue("OEEGraphicalReport").Enabled = true;
                    ddlFormat.Items.FindByValue("Format1EXCEL").Enabled = false;
                    ddlFormat.Items.FindByValue("Format1EXCEL").Enabled = false;
                    if (ddlFormat.Items.Count > 0)
                    {
                        if (ddlFormat.Items.FindByValue("Format1") != null)
                        {
                            ddlFormat.SelectedValue = "Format1";
                        }
                    }
                    if (ddlFormat.SelectedValue.ToString().Equals("Format1"))
                    {
                        trCellId.Visible = true;
                    }
                    if (ddlReportType.SelectedValue == "ProductionReportMachinewise")
                    {
                        trCellId.Visible = true;
                        if (ConfigurationManager.AppSettings["GEAPages"].ToString().Equals("1"))
                        {
                            ddlFormat.Items.FindByValue("Format3").Enabled = false;
                        }
                        else
                        {
                            ddlFormat.Items.FindByValue("Format3").Enabled = true;
                        }
                    }
                    if (ConfigurationManager.AppSettings["KiswokPage"].ToString() == "1")
                    {
                        ddlFormat.Items.FindByValue("Format4").Enabled = true;
                    }
                    if (isMachineMultiSelect())
                    {
                        ddlMachineId.Visible = false;
                        lbMachineID.Visible = true;
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
                if (ddlType.SelectedValue.ToString().Equals("Time-Consolidated", StringComparison.OrdinalIgnoreCase))
                {
                    if (ConfigurationManager.AppSettings["ShantiIronPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        if (ddlFormat.Items.FindByValue("TimeConsolidatedShanthiFormat") == null)
                        {
                            ddlFormat.Items.Add(new ListItem("Time Consolidated Shanthi-Format", "TimeConsolidatedShanthiFormat"));
                            ddlFormat.Items.FindByValue("TimeConsolidatedShanthiFormat").Enabled = true;
                        }
                        else
                            ddlFormat.Items.FindByValue("TimeConsolidatedShanthiFormat").Enabled = true;
                    }

                    ddlFormat.Items.FindByValue("Format1CockpitDataReport").Enabled = true;
                    ddlFormat.SelectedValue = "Format1CockpitDataReport";
                    ddlFormat.Items.FindByValue("Format1").Enabled = false;
                    ddlFormat.Items.FindByValue("Format1EXCEL").Enabled = false;
                    ddlFormat.Items.FindByValue("OEEGraphicalReport").Enabled = false;
                    trCellId.Visible = true;
                    if (ddlReportType.SelectedValue == "ProductionReportMachinewise")
                    {
                        trCellId.Visible = true;
                        ddlFormat.Items.FindByValue("Format3").Enabled = false;
                    }
                    trShift.Visible = false;
                    trfromdatetimeconsolidate.Visible = true;
                    trtodatetimeconsolidate.Visible = true;
                    trToDate.Visible = false;
                    trFromDate.Visible = false;
                    if (isMachineMultiSelect())
                    {
                        ddlMachineId.Visible = false;
                        lbMachineID.Visible = true;
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
                if (ddlType.SelectedValue.ToString().Equals("MachineDownTimeMatrix", StringComparison.OrdinalIgnoreCase))
                {
                    trShift.Visible = false;
                    trFormat.Visible = false;
                    ddlMachineId.Visible = false;
                    ddlMultiMachineId.Visible = false;
                    trDownReason.Visible = true;
                    if (isMachineMultiSelect())
                    {
                        ddlMachineId.Visible = false;
                        lbMachineID.Visible = true;
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
                if (ddlType.SelectedValue.ToString().Equals("MachinewiseDownTimeDetails", StringComparison.OrdinalIgnoreCase) || ddlType.SelectedValue.ToString().Equals("TimeWise", StringComparison.OrdinalIgnoreCase) || ddlType.SelectedValue.ToString().Equals("TimeAndFreqWise", StringComparison.OrdinalIgnoreCase))
                {
                    trShift.Visible = false;
                    trFormat.Visible = false;
                    ddlMachineId.Visible = true;
                    ddlMultiMachineId.Visible = false;
                    trDownReason.Visible = true;
                    if (isMachineMultiSelect())
                    {
                        ddlMachineId.Visible = false;
                        lbMachineID.Visible = true;
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
                if (ddlType.SelectedValue.ToString().Equals("MachinewiseDownTimeDetails", StringComparison.OrdinalIgnoreCase))
                {
                    if (ConfigurationManager.AppSettings["IndiaNippon"].ToString() != "1")
                    {
                        trSeperateSheet.Visible = true;
                    }
                }
                if (ddlType.SelectedValue.ToString().Equals("ProdAndDownPie", StringComparison.OrdinalIgnoreCase))
                {
                    trShift.Visible = false;
                    trFormat.Visible = false;
                    ddlMachineId.Visible = false;
                    ddlMultiMachineId.Visible = false;
                    if (isMachineMultiSelect())
                    {
                        ddlMachineId.Visible = false;
                        lbMachineID.Visible = true;
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
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportComponentwise", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = false;
                    trToDate.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trType.Visible = false;
                    trFormat.Visible = true;
                    ddlFormat.Items.FindByValue("Format1").Enabled = true;
                    ddlFormat.Items.FindByValue("Format1CockpitDataReport").Enabled = false;
                    ddlFormat.Items.FindByValue("Format1EXCEL").Enabled = false;
                    ddlFormat.Items.FindByValue("Format3").Enabled = false;
                    ddlFormat.Items.FindByValue("Format4").Enabled = false;
                    ddlFormat.Items.FindByValue("OEEGraphicalReport").Enabled = false;
                    ddlFormat.Items.FindByValue("Format2").Enabled = true;
                    trPlant.Visible = false;
                    trCellId.Visible = false;
                    trMachine.Visible = false;
                    trComponentId.Visible = true;
                    trOperationID.Visible = true;
                    trfromdatetimeconsolidate.Visible = true;
                    trtodatetimeconsolidate.Visible = true;
                    BindComponentID();
                    ddlComponentId_SelectedIndexChanged(null, null);
                }


                if (ddlReportType.SelectedValue.ToString().Equals("EnergyReport", StringComparison.OrdinalIgnoreCase))
                {
                    trview.Visible = true;
                    trType.Visible = true;
                    trFormat.Visible = false;
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trPlant.Visible = true;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    trShift.Visible = false;
                    trComponentId.Visible = false;
                    trOperationID.Visible = false;
                    trDownId.Visible = false;
                    trDownReason.Visible = false;
                    trNodeID.Visible = true;
                    trfromdatetimeconsolidate.Visible = false;
                    trtodatetimeconsolidate.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("BreakdownPhenomena", StringComparison.OrdinalIgnoreCase))
                {

                    trFormat.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    ddlMachineId.Visible = true;
                    ddlMultiMachineId.Visible = false;
                    trDownReason.Visible = false;
                    trType.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportMachinewise", StringComparison.OrdinalIgnoreCase))
                {
                    trType.Visible = false;
                    trFormat.Visible = false;
                    trToDate.Visible = false;
                    trComponentId.Visible = true;
                    trOperationID.Visible = true;
                    BindComponentID();
                    ddlComponentId_SelectedIndexChanged(null, null);
                    trType.Visible = false;
                    ddlMachineId.Items.Remove("MachineAll");

                    ddlFormat.Items.FindByValue("Format1").Enabled = true;
                    ddlFormat.Items.FindByValue("Format1CockpitDataReport").Enabled = false;
                    ddlFormat.Items.FindByValue("Format1EXCEL").Enabled = false;
                    ddlFormat.Items.FindByValue("Format3").Enabled = false;
                    ddlFormat.Items.FindByValue("Format4").Enabled = false;
                    ddlFormat.Items.FindByValue("OEEGraphicalReport").Enabled = false;
                    ddlFormat.Items.FindByValue("Format2").Enabled = true;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportDailyByHour", StringComparison.OrdinalIgnoreCase))
                {
                    trType.Visible = false;
                    trShift.Visible = false;
                    trFormat.Visible = false;
                    trComponentId.Visible = false;
                    trOperationID.Visible = false;
                    //BindComponentID();
                    //ddlComponentId_SelectedIndexChanged(null, null);
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("MISReport", StringComparison.OrdinalIgnoreCase))
                {
                    trType.Visible = false;
                    trShift.Visible = false;
                    trComponentId.Visible = false;
                    trOperationID.Visible = false;
                    trDownId.Visible = false;
                    trDownReason.Visible = false;
                    trTimeFormat.Visible = false;
                    trFormat.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("MOWISEREPORT", StringComparison.OrdinalIgnoreCase))
                {
                    ddlType.Visible = false;
                    trType.Visible = false;
                    trShift.Visible = false;
                    trComponentId.Visible = false;
                    trMoID.Visible = true;
                    trOperationID.Visible = false;
                    trDownId.Visible = false;
                    trDownReason.Visible = false;
                    trTimeFormat.Visible = false;
                    trFormat.Visible = false;
                    BindMOIDS("", "");
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("ShanthiProdReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trPlant.Visible = true;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;

                    trShift.Visible = true;
                    trType.Visible = false;
                    trMoID.Visible = false;
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

                }
                else if (ddlReportType.SelectedValue.ToString().Equals("FlowMeterReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trPlant.Visible = true;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;

                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
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

                }
                else if (ddlReportType.SelectedValue.ToString().Equals("DailyRejectionReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trPlant.Visible = true;
                    trCellId.Visible = true;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;

                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
                    trNodeID.Visible = false;
                    trFormat.Visible = false;
                    trview.Visible = false;
                    trComponentId.Visible = true;
                    BindComponentID();
                    trOperationID.Visible = false;
                    trDownId.Visible = false;
                    trTimeFormat.Visible = false;
                    ddlMultiMachineId.Visible = false;
                    trDownReason.Visible = false;
                    trfromdatetimeconsolidate.Visible = false;
                    trtodatetimeconsolidate.Visible = false;
                    trmonthlydate.Visible = false;

                }
                else if (ddlReportType.SelectedValue.ToString().Equals("HelpRequest", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trPlant.Visible = true;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;

                    trShift.Visible = true;
                    trType.Visible = false;
                    trMoID.Visible = false;
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

                }
                else if (ddlReportType.SelectedValue.ToString().Equals("AVGToolChangeReport", StringComparison.OrdinalIgnoreCase))
                {
                    BindPlantId();
                    // BindToolsLifeData();
                    //ddlMachineId.Items.RemoveAt(0);
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trPlant.Visible = true;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    trToolNumber.Visible = true;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
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

                }
                else if (ddlReportType.SelectedValue.ToString().Equals("HourlyPartCount", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = false;
                    trPlant.Visible = false;
                    trCellId.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    trToolNumber.Visible = false;
                    trShift.Visible = true;
                    trType.Visible = false;
                    trMoID.Visible = false;
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
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("OperatorEffeciencyReport", StringComparison.OrdinalIgnoreCase))
                {
                    trPlant.Visible = true;
                    trCellId.Visible = false;
                    trFromDate.Visible = false;
                    trToDate.Visible = false;
                    trOperator.Visible = true;
                    trMachine.Visible = false;
                    trMachine.Visible = false;
                    ddlMachineId.Visible = false;
                    trToolNumber.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
                    trNodeID.Visible = false;
                    trFormat.Visible = false;
                    trview.Visible = false;
                    trComponentId.Visible = false;
                    trOperationID.Visible = false;
                    trDownId.Visible = false;
                    trTimeFormat.Visible = false;
                    ddlMultiMachineId.Visible = false;
                    trDownReason.Visible = false;
                    trfromdatetimeconsolidate.Visible = true;
                    trtodatetimeconsolidate.Visible = true;
                    trmonthlydate.Visible = false;

                    if (ConfigurationManager.AppSettings["KachMotors"].ToString().Equals("1"))
                    {
                        trFormat.Visible = true;
                        ddlFormat.Items.FindByValue("Format1").Enabled = true;
                        ddlFormat.Items.FindByValue("Format1CockpitDataReport").Enabled = false;
                        ddlFormat.Items.FindByValue("Format1EXCEL").Enabled = false;
                        ddlFormat.Items.FindByValue("Format3").Enabled = false;
                        ddlFormat.Items.FindByValue("Format4").Enabled = false;
                        ddlFormat.Items.FindByValue("OEEGraphicalReport").Enabled = false;
                        ddlFormat.Items.FindByValue("Format2").Enabled = true;

                    }
                    else
                        ddlFormat.SelectedValue = "Format1";
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("MonthlyOeeReportShantiFormat", StringComparison.OrdinalIgnoreCase))
                {
                    trPlant.Visible = true;
                    trCellId.Visible = true;
                    trFromDate.Visible = false;
                    trToDate.Visible = false;
                    trOperator.Visible = true;
                    trMachine.Visible = true;
                    trMachine.Visible = false;
                    ddlMachineId.Visible = false;
                    trToolNumber.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
                    trNodeID.Visible = false;
                    trFormat.Visible = false;
                    trview.Visible = false;
                    trComponentId.Visible = false;
                    trOperationID.Visible = false;
                    trDownId.Visible = false;
                    trTimeFormat.Visible = false;
                    ddlMultiMachineId.Visible = false;
                    trDownReason.Visible = false;
                    trfromdatetimeconsolidate.Visible = true;
                    trtodatetimeconsolidate.Visible = true;
                    trmonthlydate.Visible = true;

                    if (isMachineMultiSelect())
                    {
                        ddlMachineId.Visible = false;
                        lbMachineID.Visible = true;
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
                else if (ddlReportType.SelectedValue.ToString().Equals("RejectionORReworkReportShantiFormat", StringComparison.OrdinalIgnoreCase))
                {
                    trPlant.Visible = true;
                    trCellId.Visible = true;
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trOperator.Visible = true;
                    trMachine.Visible = true;
                    trMachine.Visible = false;
                    ddlMachineId.Visible = false;
                    trToolNumber.Visible = false;
                    trShift.Visible = true;
                    trType.Visible = false;
                    trMoID.Visible = false;
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
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("PMReport", StringComparison.OrdinalIgnoreCase))
                {
                    trPlant.Visible = true;
                    trCellId.Visible = true;
                    trFromDate.Visible = false;
                    trToDate.Visible = false;
                    trOperator.Visible = true;
                    trMachine.Visible = true;
                    trMachine.Visible = false;
                    ddlMachineId.Visible = false;
                    BindMachinesForPlantCell();
                    trmonthlydate.Visible = true;
                    txtMonth.Visible = false;
                    trToolNumber.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
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
                    //trmonthlydate.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("ProddutionReportGEA", StringComparison.OrdinalIgnoreCase))
                {
                    trPlant.Visible = true;
                    trCellId.Visible = true;
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trOperator.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    trToolNumber.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
                    BindMachinesForPlantCell();
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
                    trYear.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("MivinInspectionReport"))
                {
                    trPlant.Visible = false;
                    trCellId.Visible = false;
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trOperator.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    trToolNumber.Visible = false;
                    trShift.Visible = true;
                    trType.Visible = false;
                    trMoID.Visible = false;
                    BindMachinesForPlantCell();
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
                    trYear.Visible = false;
                    trmivintype.Visible = true;
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("PMReportAAAPL"))
                {
                    trPlant.Visible = true;
                    trCellId.Visible = true;
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trOperator.Visible = false;
                    trMachine.Visible = true;
                    ddlMachineId.Visible = true;
                    trToolNumber.Visible = false;
                    trShift.Visible = false;
                    trType.Visible = false;
                    trMoID.Visible = false;
                    BindMachinesForPlantCell();
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
                    trYear.Visible = false;
                    trmivintype.Visible = false;
                }
               


                setPlantID();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion


        #region MOSelectionChange
        protected void btmMosearch_Click(object sender, EventArgs e)
        {
            BindMOIDS(txtMOID.Text, "MOsearchwithlike");
        }

        protected void btnMoExactSearch_Click(object sender, EventArgs e)
        {
            BindMOIDS(txtMOID.Text, "MOExactSearch");
        }
        #endregion

        #region CellSelectionChange
        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachinesForPlantCell();
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
        #endregion

        private void setFromDateLabel()
        {
            try
            {
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportMachinewise", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.ToString().Equals("SpindleIdleTimeAnalysisReportLnTOdisha", StringComparison.OrdinalIgnoreCase))
                {
                    lblFromDate.Text = "Select Date";
                }
                else
                {
                    lblFromDate.Text = "From Date";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("setFromDateLabel = " + ex.Message);
            }
        }
        #region FormatSelectionChange
        protected void ddlFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            trCellId.Visible = false;
            if (ddlFormat.SelectedValue.ToString().Equals("Format1EXCEL") || ddlFormat.SelectedValue.ToString().Equals("Format1") || ddlType.SelectedValue.ToString().Equals("Time-Consolidated") || ddlType.SelectedValue.ToString().Equals("Daily") || ddlFormat.SelectedValue.ToString().Equals("Format4") || (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format3")) || ddlFormat.SelectedValue.ToString().Equals("OEEGraphicalReport") || (ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format2")))
            {
                trCellId.Visible = true;
                if (isMachineMultiSelect())
                {
                    ddlMachineId.Visible = false;
                    lbMachineID.Visible = true;
                    ddlCellID.Visible = false;
                    lbCellID.Visible = true;
                    BindMachineIDListBox();
                }
                else
                {
                    ddlMachineId.Visible = true;
                    lbMachineID.Visible = false;
                    ddlCellID.Visible = true;
                    lbCellID.Visible = false;
                    BindMachinesForPlantCell();
                }

            }
            else
            {
                BindMachines();
            }

            if (ddlReportType.SelectedValue.ToString().Equals("TrelleborgOEEReport", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format2"))
            {
                trCellId.Visible = true;
                ddlMachineId.Visible = true;
                lbMachineID.Visible = false;
                ddlCellID.Visible = true;
                lbCellID.Visible = false;
                BindMachinesForPlantCell();
                trShift.Visible = true;
                ddlShift.Visible = true;
            }
            //else
            //{
            //    trShift.Visible = false;
            //    ddlShift.Visible = false;
            //}
            if ((ddlReportType.SelectedValue.ToString().Equals("ProductionReportComponentwise", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.ToString().Equals("OperatorEffeciencyReport",StringComparison.OrdinalIgnoreCase)) && ddlFormat.SelectedValue.ToString().Equals("Format1"))
            {
                trCellId.Visible = false;
            }

            if (ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format1"))
            {
                trCellId.Visible = false;
                trComponentId.Visible = true;
                trOperationID.Visible = true;
                ddlMachineId.Items.Remove("MachineAll");
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format2"))
            {
                trCellId.Visible = true;
                trComponentId.Visible = false;
                trOperationID.Visible = false;
            }

            if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && (ddlType.SelectedValue.ToString().Equals("Shift", StringComparison.OrdinalIgnoreCase) || ddlType.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase)))
            {
                if (ddlFormat.SelectedValue.ToString().Equals("Format4"))
                {
                    trToDate.Visible = false;
                }
                else
                {
                    trToDate.Visible = true;
                }
            }
            setPlantID();
        }
        #endregion
        private void setPlantID()
        {
            try
            {
                if (trPlant.Visible)
                {
                    if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase))
                    {
                        if ((ddlType.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase) || ddlType.SelectedValue.ToString().Equals("Shift", StringComparison.OrdinalIgnoreCase)) && ddlFormat.SelectedValue.ToString().Equals("Format4", StringComparison.OrdinalIgnoreCase))
                        {
                            if (ddlPlantId.Items.FindByValue("ALL") != null)
                            {
                                ddlPlantId.Items.Remove(ddlPlantId.Items.FindByValue("ALL"));
                                BindCellId();
                                BindMachinesForPlantCell();
                            }
                        }
                        else
                        {
                            if (ddlPlantId.Items.FindByValue("ALL") == null)
                            {
                                ddlPlantId.Items.Insert(0, new ListItem
                                {
                                    Text = GetGlobalResourceObject("CommanResource", "PlantAll").ToString(),
                                    Value = "ALL"
                                });
                            }
                        }
                    }
                    else if (ddlReportType.SelectedValue.ToString().Equals("MachineUtilizationReportACE", StringComparison.OrdinalIgnoreCase))
                    {
                        ddlPlantId.Items.Remove(ddlPlantId.Items.FindByValue("ALL"));
                    }
                    else
                    {
                        if (ddlPlantId.Items.FindByValue("ALL") == null)
                        {
                            ddlPlantId.Items.Insert(0, new ListItem
                            {
                                Text = GetGlobalResourceObject("CommanResource", "PlantAll").ToString(),
                                Value = "ALL"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #region "On Button Generat Click Event "
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
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


                string machineId = ddlMachineId.SelectedValue == "ALL" ? string.Empty : ddlMachineId.SelectedValue;// ddlMachineId.SelectedItem.ToString();              
                string plantId = ddlPlantId.SelectedValue == "ALL" ? string.Empty : ddlPlantId.SelectedValue;
                string CellID = ddlCellID.SelectedValue == "ALL" ? string.Empty : ddlCellID.SelectedValue;
                DateTime fromDate = DateTime.Now.Date;
                DateTime toDate = DateTime.Now.Date;
                Logger.WriteDebugLog(txtFromDate.Text);
                if (ddlReportType.SelectedValue.Equals("HourlyPartCount", StringComparison.OrdinalIgnoreCase))
                {
                    Logger.WriteDebugLog("before vdgcall: " + txtFromDate.Text);
                    fromDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(txtFromDate.Text));
                    Logger.WriteDebugLog("After vdgcall: " + fromDate);
                }
                else
                {
                    if (trtodatetimeconsolidate.Visible || trfromdatetimeconsolidate.Visible)
                    {
                        fromDate = Util.GetDateTime(txttimeconsolidate_fromdate.Text);
                        toDate = Util.GetDateTime(txttimeconsolidate_todate.Text);
                    }
                    else
                    {
                        fromDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(txtFromDate.Text));
                        toDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtToDate.Text));
                    }
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Shift", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format1", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    CellID = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    Generated = TMPTrakGenerateReport.AnalysisMachinewiseShiftFormat1Report(fromDate, ddlShift.SelectedValue.ToString(), machineId, "", "", plantId, CellID, toDate.AddDays(-1));
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Shift", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format4", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.AnalysisMachinewiseShiftFormat4Report(fromDate, ddlShift.SelectedValue.ToString(), machineId, "", "", plantId, CellID, fromDate);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Shift", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format3", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    CellID = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    Generated = TMPTrakGenerateReport.AnalysisMachinewiseShiftFormat3Report(fromDate, ddlShift.SelectedValue.ToString(), machineId, "", "", plantId, toDate.AddDays(-1), CellID);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format1EXCEL", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    CellID = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    Generated = TMPTrakGenerateReport.AnalysisMachinewiseDailyFormat1Report(fromDate, machineId, "", "", plantId, CellID, toDate.AddDays(-1));
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format4", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.AnalysisMachinewiseDailyFormat4Report(fromDate, machineId, "", "", plantId, CellID, fromDate);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Shift", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("OEEGraphicalReport", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    CellID = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    Generated = TMPTrakGenerateReport.AnalysisMachinewiseShiftOEEGraphicalReport(fromDate, ddlShift.SelectedValue.ToString(), machineId, plantId, toDate.AddDays(-1), CellID);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportComponentwise", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format1", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.ComponentwiseReport(fromDate, ddlComponentId.SelectedValue.ToString(), ddlOperationID.SelectedValue.ToString(), toDate);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportComponentwise", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format2", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.ComponentBatchwiseReport(fromDate, ddlComponentId.SelectedValue.ToString(), ddlOperationID.SelectedValue.ToString(), toDate);
                }

                if (ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportDailyByHour", StringComparison.OrdinalIgnoreCase))
                {
                    TMPTrakGenerateReport.DailyProdandDownLogbyDayReport(fromDate, ddlPlantId.SelectedValue.ToString(), ddlMachineId.SelectedValue.ToString(), toDate);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("TrelleborgOEEReport", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format1", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.TrelleborgOEEReport(fromDate, ddlPlantId.SelectedValue.ToString(), ddlMachineId.SelectedValue.ToString(), toDate, ddlCellID.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("TrelleborgOEEReport", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format2", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.TrelleborgOEEReport_withShiftwise(fromDate, ddlPlantId.SelectedValue.ToString(), ddlMachineId.SelectedValue.ToString(), toDate, ddlCellID.SelectedValue.ToString(), ddlShift.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format1", StringComparison.OrdinalIgnoreCase))
                {
                    toDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtFromDate.Text));
                    Generated = TMPTrakGenerateReport.ProductionandDownTimeReport_Machinewise(fromDate, ddlPlantId.SelectedValue.ToString(), ddlMachineId.SelectedValue.ToString(), ddlShift.SelectedValue.ToString(), toDate, ddlComponentId.SelectedValue.ToString(), ddlOperationID.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionAndDowntimeReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format2", StringComparison.OrdinalIgnoreCase))
                {
                    if (ddlShift.SelectedValue.ToString().Equals("All"))
                    {
                        toDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtFromDate.Text));
                    }
                    else
                    {
                        var shift = CockpitDataBaseAccess.GetShiftTime(ddlShift.SelectedValue.ToString(), txtFromDate.Text);
                        if (shift != null)
                        {
                            fromDate = Util.GetDateTime(shift[0]);
                            toDate = Util.GetDateTime(shift[1]);
                        }
                    }

                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    CellID = DataBaseAccess.getCellIDWithSeparator(lbCellID);

                    Generated = TPMTrakGenerateReportNewDll.ProductionandDownTimeReport_MachinewiseGraph(fromDate, ddlPlantId.SelectedValue.ToString(), machineId, ddlShift.SelectedValue.ToString(), toDate, CellID);
                }

                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Time-Consolidated", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format1CockpitDataReport", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    CellID = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    Generated = TMPTrakGenerateReport.SM_MachinewiseProdReportFromAutoData(fromDate, machineId, plantId, CellID, toDate);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Time-Consolidated", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("TimeConsolidatedShanthiFormat", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.TimeConsolidatedShanthiProductionReport(fromDate, machineId, plantId, CellID, toDate);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("MachineDownTimeMatrix", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    CellID = DataBaseAccess.getCellIDWithSeparator(lbCellID);
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
                    Generated = TPMTrakGenerateReportNewDll.MachineDownTimeMatrix(fromDate, ddlPlantId.SelectedValue.ToString(), machineId, toDate, downId, Exclude, TheBrowserWidth, TheBrowserHeight, ddlTopDownReasons.SelectedValue.ToString(), CellID, "standard", "TimeWise");

                }
                if (ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("TimeWise", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    CellID = DataBaseAccess.getCellIDWithSeparator(lbCellID);
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
                    Generated = TPMTrakGenerateReportNewDll.MachineDownTimeMatrix(fromDate, ddlPlantId.SelectedValue.ToString(), machineSelection, toDate, downId, Exclude, TheBrowserWidth, TheBrowserHeight, ddlTopDownReasons.SelectedValue.ToString(), "", "standard", "TimeWise");
                }
                if (ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("TimeAndFreqWise", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    CellID = DataBaseAccess.getCellIDWithSeparator(lbCellID);
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
                    Generated = TPMTrakGenerateReportNewDll.MachineDownTimeMatrix(fromDate, ddlPlantId.SelectedValue.ToString(), machineId, toDate, downId, Exclude, TheBrowserWidth, TheBrowserHeight, ddlTopDownReasons.SelectedValue.ToString(), CellID, "standard", "TimeAndFreqWise");
                }
                if (ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("ProdAndDownPie", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    CellID = DataBaseAccess.getCellIDWithSeparator(lbCellID);
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
                    Exclude = 0;
                    if (machineSelection != null && machineSelection != "")
                        TMPTrakGenerateReport.ProductionDownPieDownTimeChartReport(fromDate, ddlPlantId.SelectedValue.ToString(), machineId, toDate, downId, Exclude, CellID);
                    else
                        TMPTrakGenerateReport.ProductionDownPieDownTimeChartReport(fromDate, ddlPlantId.SelectedValue.ToString(), machineId, toDate, downId, Exclude, CellID);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("DowntimeReport", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("MachinewiseDownTimeDetails", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    CellID = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    int Exclude = 0;
                    string downId = "", listDownId = "";
                    foreach (ListItem item in ddlMultiDownID.Items)
                    {
                        if (item.Selected)
                        {
                            //listDownId += item.Value + "$@#";
                            listDownId += @"'" + item.Value + @"',";
                        }
                    }
                    if (listDownId != "")
                    {
                        //string[] result = listDownId.Split(new string[] { "$@#" }, StringSplitOptions.None);
                        //result = result.Take(result.Count() - 1).ToArray();
                        //downId = string.Join(",", result.ToArray());
                        downId = listDownId.TrimEnd(',');
                    }
                    if (chkExclude.Checked)
                        Exclude = 1;
                    else
                        Exclude = 0;
                    if (machineSelection != null && machineSelection != "")
                        Generated = TMPTrakGenerateReport.MachineWiseDownTimeDetails(fromDate, ddlPlantId.SelectedValue.ToString(), machineId, toDate, downId, Exclude, chkSeperateSheet.Checked, CellID);
                    else
                        Generated = TMPTrakGenerateReport.MachineWiseDownTimeDetails(fromDate, ddlPlantId.SelectedValue.ToString(), machineId, toDate, downId, Exclude, chkSeperateSheet.Checked, CellID);

                }
                if (ddlReportType.SelectedValue.ToString().Equals("ToolChangeFrequencyReport", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.ToolChangeFrequencyTool(fromDate, toDate, ddlPlantId.SelectedValue.ToString(), ddlMachineId.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("MOWISEREPORT", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.MOWISEREPORTGENERATE(fromDate, toDate, ddlPlantId.SelectedValue.ToString(), ddlMachineId.SelectedValue.ToString(), ddlMoWise.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise", StringComparison.OrdinalIgnoreCase) && ddlType.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("SONABLW", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.SONA_BFW_REPORT(fromDate, toDate, ddlPlantId.SelectedValue.ToString(), ddlMachineId.SelectedValue.ToString(), "standard");
                }
                if (ddlReportType.SelectedValue.ToString().Equals("MISReport", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.SONA_MIS_REPORT(fromDate, toDate.AddDays(-1), ddlMachineId.SelectedValue.ToString(), ddlPlantId.SelectedValue.ToString(), "standard");
                }
                if (ddlReportType.SelectedValue.ToString().Equals("EnergyReport", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.EnergyReport(fromDate, toDate, ddlPlantId.SelectedValue.ToString(), ddlMachineId.SelectedValue.ToString(), ddlType.SelectedValue.ToString(), ddlview.SelectedValue.ToString(), ddlNodeid.SelectedValue.ToString(), "standard");
                }
                if (ddlReportType.SelectedValue.ToString().Equals("EnergyReportSona", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.EnergyReportSona(fromDate, toDate, "standard");
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ToolChangeReport", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.ToolChangeReport(fromDate, toDate, ddlMachineId.SelectedValue.ToString(), ddlPlantId.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("Hytechreport", StringComparison.OrdinalIgnoreCase))
                {
                    string date = "01-" + txtMonth.Text + "-" + txtYear.Text;
                    fromDate = Util.GetDateTime(date);
                    TMPTrakGenerateReport.HytechReport(fromDate, ddlMachineId.SelectedValue.ToString(), ddlPlantId.SelectedValue.ToString());
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
                    if (chkExclude.Checked)
                        Exclude = 1;
                    else
                        Exclude = 0;
                    #endregion
                    var TheBrowserWidth = width.Value;
                    var TheBrowserHeight = height.Value;
                    Generated = TMPTrakGenerateReport.MachineBreakDownTimeMatrix(fromDate, ddlPlantId.SelectedValue.ToString(), machineSelection, toDate, downId, Exclude, TheBrowserWidth, TheBrowserHeight, ddlTopDownReasons.SelectedValue.ToString(), "", "standard");
                }
                if (ddlReportType.SelectedValue.ToString().Equals("DailyRejectionReport", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    CellID = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    Generated = TMPTrakGenerateReport.DailyRejectionReport(fromDate, toDate, plantId, machineId, ddlComponentId.SelectedValue.ToString(), CellID);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("FlowMeterReport", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.FlowMeterReport(fromDate, toDate, plantId, machineId, ddlShift.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("HelpRequest", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.HelpRequestReport(fromDate, toDate, plantId, machineId, ddlShift.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("AVGToolChangeReport", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.AverageToolLifeReports(machineId, plantId, fromDate, toDate, ddlToolNumber.SelectedValue.ToString(), ddlCellID.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("HourlyPartCount", StringComparison.OrdinalIgnoreCase))
                {
                    Logger.WriteDebugLog("Inside hourly partCount reportCall");

                    Generated = TMPTrakGenerateReport.HourlyPartCountReports(machineId, fromDate, ddlShift.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("MonthlyOeeReportShantiFormat", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    CellID = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    fromDate = Util.GetDateTime("01-" + txtMonth.Text + "-" + txtYear.Text);
                    //fromDate = new DateTime(Convert.ToInt32(txtYear.Text), DateTime.Now.Month, 1);
                    Generated = TMPTrakGenerateReport.MonthlyOeeReportShantiFormat(plantId, CellID, machineId, fromDate);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("RejectionORReworkReportShantiFormat", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.RejectionORReworkReportShantiFormat(plantId, ddlCellID.SelectedValue.ToString(), machineId, fromDate, toDate, ddlShift.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ShanthiProdReport", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.ShanthiProdReport(plantId, ddlShift.SelectedValue.ToString(), machineId, fromDate, toDate);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("PMReport", StringComparison.OrdinalIgnoreCase))
                {
                    fromDate = Util.GetDateTime("01-" + "01-" + txtYearOnly.Text);
                    Generated = TMPTrakGenerateReport.PMReport(plantId, ddlCellID.SelectedValue.ToString(), machineId, fromDate, toDate);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProddutionReportGEA", StringComparison.OrdinalIgnoreCase))
                {
                    //Generated = TMPTrakGenerateReport.ProductionReportGEA(ddlPlantId.SelectedValue.ToString(), ddlCellID.SelectedValue.ToString(), fromDate, toDate);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("OperatorEffeciencyReport", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format1",StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.OperatorWiseReport(plantId, fromDate, toDate, ddlOperator.SelectedValue.ToString());
                }
                else if(ddlReportType.SelectedValue.ToString().Equals("OperatorEffeciencyReport", StringComparison.OrdinalIgnoreCase) && ddlFormat.SelectedValue.ToString().Equals("Format2", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.OperatorWiseReport_KachMotors(plantId, fromDate, toDate, ddlOperator.SelectedValue.ToString());
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("MivinInspectionReport"))
                {
                    toDate = Convert.ToDateTime(VDGDataBaseAccess.GetLogicalDayStart(toDate.ToString("yyyy-MM-dd HH:mm:ss")));
                    Generated = TMPTrakGenerateReport.GetMivinInspectionReport(fromDate, toDate, ddlMachineId.SelectedValue.ToString(), ddlShift.SelectedValue.ToString(), ddlMivinType.SelectedValue.ToString(), ddlMivinType.SelectedItem.Text.ToString());
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("PMReportAAAPL"))
                {
                    Generated = TMPTrakGenerateReport.AAAPLPMReport(fromDate, toDate, ddlMachineId.SelectedValue.ToString());
                }

                if (ddlReportType.SelectedValue.ToString().Equals("HourwiseOperatorIncentiveReport", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.HourwiseOperatorIncentiveReport(fromDate, toDate, ddlOperator.SelectedValue);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("OperatorDrawingNoReport", StringComparison.OrdinalIgnoreCase))
                {
                    string OpId = "", listOpId = "";
                    foreach (ListItem item in ddlMultiOperator.Items)
                    {
                        if (item.Selected)
                        {
                            listOpId += @"'" + item.Value + @"',";
                        }
                    }
                    if (listOpId != "")
                    {
                        OpId = listOpId.TrimEnd(',');

                    }
                    Generated = TMPTrakGenerateReport.GetOperatorDrawingNoDetails(fromDate, toDate, OpId);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("DataTraceabilityReportAdvikPanth", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.DataTraceabilityReportAdvikPanth(fromDate, toDate, ddlModel.SelectedValue, txtQRCodeSearch.Text);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("MachineReworkReport", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.MachineReworkReport(fromDate, toDate, ddlMachineId.SelectedValue);
                }

                if (ddlReportType.SelectedValue.ToString().Equals("MachinewiseScrapReport", StringComparison.OrdinalIgnoreCase))
                {
                    string Machine = string.Empty;
                    foreach (System.Web.UI.WebControls.ListItem Item in lbMachineID.Items)
                    {

                        Machine += Item.Selected ? "'" + Item.Value + "'," : "";
                    }
                    Machine = Machine.Trim(',');
                    Generated = TMPTrakGenerateReport.MachinewiseScrapReport(fromDate, toDate, Machine);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ComponentSetupReport", StringComparison.OrdinalIgnoreCase))
                {
                    string OpId = "", listOpId = "";
                    foreach (ListItem item in ddlMultiOperator.Items)
                    {
                        if (item.Selected)
                        {
                            listOpId += @"'" + item.Value + @"',";
                        }
                    }
                    if (listOpId != "")
                    {
                        OpId = listOpId.TrimEnd(',');

                    }

                    string CompId = "", listCompId = "";
                    foreach (ListItem item in lstComponentID.Items)
                    {
                        if (item.Selected)
                        {
                            listCompId += @"'" + item.Value + @"',";
                        }
                    }
                    if (listCompId != "")
                    {
                        CompId = listCompId.TrimEnd(',');

                    }
                    Generated = TPMTrakGenerateReportNewDll.ComponentSetupReport(fromDate, toDate, ddlPlantId.SelectedValue.ToString(), ddlCellID.SelectedValue.ToString(), ddlMachineId.SelectedValue.ToString(), OpId, CompId);

                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionHourlyReportLeonine", StringComparison.OrdinalIgnoreCase))
                {
                    fromDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(txtFromDate.Text));
                    toDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtToDate.Text));
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    CellID = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    Generated = TPMTrakGenerateReportNewDll.GenerateLeonineReport(ddlShift.SelectedValue, fromDate, toDate, CellID, machineId);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("KKPillarReport", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    Generated = TPMTrakGenerateReportNewDll.KKPillarReport(fromDate, toDate, ddlPlantId.SelectedValue.ToString(), ddlShift.SelectedValue, machineId);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ShiftwiseOperatorPerformance", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    Generated = TPMTrakGenerateReportNewDll.ShiftwiseOperatorPerformanceReport(fromDate, toDate, ddlPlantId.SelectedValue.ToString(), ddlShift.SelectedValue, machineId, ddlOperator.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("HydroTestReport", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    Generated = TPMTrakGenerateReportNewDll.HydroTestReport(fromDate, toDate, ddlProcess.SelectedValue.ToString(), machineId);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("SpindleIdleTimeAnalysisReportLnTOdisha", StringComparison.OrdinalIgnoreCase))
                {
                    fromDate = Util.GetDateTime(txtFromDate.Text);
                    Generated = TMPTrakGenerateReport.GenerateSpindleIdleTimeAnalysisReportLnTOdisha(fromDate, ddlMachineId.SelectedValue.ToString(), ddlShift.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("MachineUtilizationReportACE", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.GenerateProductionandDownReportAce(fromDate, toDate, ddlMachineId.SelectedValue.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Equals("PDIReportTafeChennai", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.PDIReportTafeChennai(ddlSlno.SelectedValue);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ToolChangeFrequencyRecord", StringComparison.OrdinalIgnoreCase))
                {
                    fromDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(txtFromDate.Text));
                    toDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtToDate.Text));
                    //fromDate = Util.GetDateTime(txtFromDate.Text);
                    //toDate = Util.GetDateTime(txtToDate.Text);
                    if (ddlMultiMachineId.SelectedIndex == 0)
                    {
                        machineId = "";
                    }
                    else
                    {
                        machineId = DataBaseAccess.getMachineIDWithSeparator(ddlMultiMachineId);
                    }
                    Generated = TMPTrakGenerateReport.ToolChangeFrequencyReportVulkan(fromDate, toDate, machineId);
                }

                if (ddlReportType.SelectedValue.ToString().Trim().Equals("InspectionReportVulkan", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = HelperClassGeneric.getListBoxValueWithCommaSeparator(ddlMultiMachineId);
                    string MachineIDForOEE = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(ddlMultiMachineId);
                    Generated = TMPTrakGenerateReport.GenerateInspectionReport(txtFromDate.Text, ddlShift.SelectedValue, machineId, MachineIDForOEE);
                }
                if (ddlReportType.SelectedValue.ToString().Trim().Equals("PMTransactionReport", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(ddlMultiMachineId);
                    Generated = TMPTrakGenerateReport.GeneratePMReport(machineId, txtYear.Text.Trim(), txtMonth.Text.Trim());
                }
                if (ddlReportType.SelectedValue.ToString().Trim().Equals("CycleTimeReport", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.GenerateCycleTimeReportVulkanMS(txtFromDate.Text.Trim(), txtToDate.Text.Trim(), ddlMachineId.SelectedValue);
                }
                if (ddlReportType.SelectedValue.ToString().Trim().Equals("ToolLifeReport", StringComparison.OrdinalIgnoreCase))
                {
                    var days = Util.GetDateTime(txtToDate.Text.Trim()) - Util.GetDateTime(txtFromDate.Text.Trim());
                    if (days.TotalDays > 7)
                    {
                        HelperClassGeneric.openWarningModal(this, "From and To date difference cannot be more than 7 days");
                        return;
                    }
                    Generated = TMPTrakGenerateReport.GenerateToolChangeReportVulkanMS(txtFromDate.Text, txtToDate.Text, ddlMachineId.SelectedValue);
                }
                if (ddlReportType.SelectedValue.ToString().Trim().Equals("AMTransactionReport", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(ddlMultiMachineId);
                    Generated = TMPTrakGenerateReport.GenerateAMReport(machineId, txtYear.Text.Trim(), txtMonth.Text.Trim(), rdnReportType.SelectedValue);
                }
                if (ddlReportType.SelectedValue.ToString().Trim().Equals("ERPPerformanceReportSKS", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.GenerateProductionreportSKS(ddlMachineId.SelectedValue, txtToDate.Text.ToString());
                }
                if (ddlReportType.SelectedValue.ToString().Trim().Equals("PMTransactionReportLnTOdisha", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = LnTOdisha.Model.LnTGenerateReport.GeneratePMReport(ddlMachineId.SelectedValue, txtYearOnly.Text.Trim(),"","");
                }
                if (ddlReportType.SelectedValue.ToString().Trim().Equals("InspectionReport_Rexnord", StringComparison.OrdinalIgnoreCase))
                {
                    string OperationNo = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(ddlMultiOperationID);
                    Generated = TMPTrakGenerateReport.GetInspectionReport(ddlSlno.SelectedValue.ToString(), OperationNo);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportMachinewise_PrecisionEngg", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.GetProductionMachinewise_PrecisionEnggReport(fromDate, ddlShift.SelectedValue.ToString(), ddlMachineId.SelectedValue, plantId, toDate);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionReportComponentwise_PrecisionEngg", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.GetProductionComponentwise_PrecisionEnggReport(fromDate, ddlComponentId.SelectedValue.ToString(), toDate);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("DailyRejectionReport_PrecisionEngg", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.GetDailyRejection_PrecisionEnggReport(fromDate, ddlComponentId.SelectedValue.ToString(), ddlPlantId.SelectedValue, ddlMachineId.SelectedValue, toDate);
                }
                if (ddlReportType.SelectedValue.ToString().Equals("OperatorEfficiencyReport_PrecisionEngg", StringComparison.OrdinalIgnoreCase))
                {
                    Generated = TMPTrakGenerateReport.GetOperatorEfficiency_PrecisionEnggReport(fromDate, ddlOperator.SelectedValue.ToString(), toDate);
                }
                if (ddlReportType.SelectedValue.ToString().Trim().Equals("DailyCleaningandMaintenanceReport_PrecisionEngg", StringComparison.OrdinalIgnoreCase))
                {
                    machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                    Generated = TMPTrakGenerateReport.GetDailyCleaningAndMaintenance_PrecisionEnggReport(fromDate, ddlGroupID.SelectedValue.ToString(), machineId);
                }
                if (ddlReportType.SelectedValue.ToString().Trim().Equals("InspectionApprovalReport_Highway", StringComparison.OrdinalIgnoreCase))
                {
                    DataTable dt = new DataTable();
                    DataTable dtOperator = new DataTable();
                    Generated = HighwayGenerateReports.GenerateInspectionReportOfShaft(dt, dtOperator, ddlComponentId.SelectedValue.ToString(), ddlShift.SelectedValue, txtFromDate.Text, ddlOperationID.SelectedValue, ddlRevID.SelectedValue, ddlHeatNo.SelectedValue, ddlDieNo.SelectedValue);
                }
                if (ddlReportType.SelectedValue.ToString().Trim().Equals("ChecksheetApprovalReport_Highway", StringComparison.OrdinalIgnoreCase))
                {

                }
                //if (ddlReportType.SelectedValue.ToString().Equals("ComponentStandardCycltimeComparison", StringComparison.OrdinalIgnoreCase))
                //{
                //    fromDate = Util.GetDateTime(txtFromDate.Text);
                //    toDate = Util.GetDateTime(txtToDate.Text);
                //    machineId = ddlMachineId.SelectedValue == null ? "" : ddlMachineId.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlMachineId.SelectedValue.ToString();
                //    string ComponentID = ddlComponentId.SelectedValue == null ? "" : ddlComponentId.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlComponentId.SelectedValue.ToString();
                //    string OperationNo = ddlOperationID.SelectedValue == null ? "" : ddlOperationID.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlOperationID.SelectedValue.ToString();
                //    Generated = TMPTrakGenerateReport.GetComponentStandardCycleTimeComparisonData_KTA(fromDate, toDate, machineId, ComponentID, OperationNo);
                //}
                if (ddlReportType.SelectedValue.ToString().Equals("MaintenanceParticularReport_PrecisionEngg", StringComparison.OrdinalIgnoreCase))
                {
                    string Machine = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMachineID);
                    string Operator = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(ddlMultiOperator);
                    string Checkpoints = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbCheckpointID);
                    string Type = ddlTypee.SelectedValue.ToString();
                    if (ddlTypee.SelectedValue.ToString() == "Machinewise")
                    {
                        Generated = TMPTrakGenerateReport.GetMaintenanceParticularReport_PrecisionEngg(fromDate, toDate, Machine,Type);
                    }
                    else if (ddlTypee.SelectedValue.ToString() == "Operatorwise")
                    {
                        Generated = TMPTrakGenerateReport.GetMaintenanceParticularReport_PrecisionEngg(fromDate, toDate, Operator,Type);
                    }
                    else if (ddlTypee.SelectedValue.ToString() == "Particularwise")
                    {
                        Generated = TMPTrakGenerateReport.GetMaintenanceParticularReport_PrecisionEngg(fromDate, toDate, Checkpoints,Type);
                    }
                }
                if (ddlReportType.SelectedValue.ToString().Equals("ProductionPlantwiseReport_Dantal", StringComparison.OrdinalIgnoreCase))
                {
                    fromDate = Util.GetDateTime(txtFromDate.Text);
                    toDate = Util.GetDateTime(txtToDate.Text);
                    if ((ddlReportType.SelectedValue.ToString().Equals("ProductionPlantwiseReport_Dantal", StringComparison.OrdinalIgnoreCase)) && (toDate - fromDate).Days > 15)
                    {
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "Alert('Days cannot be greater than 15');", true);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Days cannot be greater than 15')", true);
                        return;
                    }
                    else
                    {
                        plantId = ddlPlantId.SelectedValue.ToString();
                        machineId = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                        CellID = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                        string plant = HelperClassGeneric.getDropdownValueWithCommaSeparator(ddlPlantId);
                        Generated = TMPTrakGenerateReport.ProductionDantalReports(plantId, machineId, CellID, fromDate, toDate,plant);
                    }
                    
                }

                if (Generated.Equals("", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "messageNotOk();", true);
                else if (Generated.Equals("NodataFound", StringComparison.OrdinalIgnoreCase))
                {
                    if ((ddlMoWise.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase)))
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNodata", "messageNodata();", true);
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNodataformo", "messageNodataformo();", true);
                        //ddlMoWise.SelectedValue = "ALL";
                    }
                }
                else if (Generated.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "messageOk();", true);
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

        #region MachineSelectionChange
        protected void ddlMachineId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["sonapages"].Equals("1") && ddlReportType.SelectedValue.ToString().Equals("EnergyReport", StringComparison.OrdinalIgnoreCase))
            {
                bindNode();
            }
            if (ConfigurationManager.AppSettings["ShantiIronPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
            {
                BindToolsLifeData();
            }

        }
        #endregion

        #region buttoncompClick
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
        #endregion
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
                    list = DataBaseAccess.getSerialNoForPDIReportTafeChennai(txtSlnoSearch.Text.Trim());
                }
                ddlSlno.DataSource = list;
                ddlSlno.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindSlNo()
        {
            try
            {
                List<string> list = DataBaseAccess.GetSlNo_Rexnord();
                ddlSlno.DataSource = list;
                ddlSlno.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
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

        protected void lbMachineID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static int getReportDateInterval()
        {
            int dateInterval = 15;
            try
            {
                dateInterval = Convert.ToInt32(ConfigurationManager.AppSettings["LiveReportDateInterval"].ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return dateInterval;
        }

        protected void ddlSlno_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOperationNo("");
        }

        protected void ddlGroupID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineIDListBox();
        }
        private void BindCheckpoints()
        {
            List<string> list = DataBaseAccess.GetCheckpointIDs_Precision();
            lbCheckpointID.DataSource = list;
            lbCheckpointID.DataBind();
            foreach(ListItem item in lbCheckpointID.Items)
            {
                item.Selected = true;
            }
        }
        protected void ddlTypee_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlTypee.SelectedValue.ToString() == "Machinewise")
                {
                    trMachine.Visible = true;
                    ddlMultiMachineId.Visible = false;
                    ddlMachineId.Visible = false;
                    lbMachineID.Visible = true;
                    trMultiOperator.Visible = false;
                    trCheckpoints.Visible = false;
                }
                else if(ddlTypee.SelectedValue.ToString() == "Operatorwise")
                {
                    trMachine.Visible = false;
                    trCheckpoints.Visible = false;
                    trMultiOperator.Visible = true;
                    //ddlOperator.Visible = true;
                    ddlMultiOperator.Visible = true;
                    BindOperator();
                }
                else if (ddlTypee.SelectedValue.ToString() == "Particularwise")
                {
                    trMachine.Visible = false;
                    trMultiOperator.Visible = false;
                    trCheckpoints.Visible = true;
                    BindCheckpoints();
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}