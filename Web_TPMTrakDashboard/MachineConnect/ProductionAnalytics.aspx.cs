using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.MachineConnect.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.MachineConnect
{
    public partial class ProductionAnalytics : System.Web.UI.Page
    {
        static string BindChartFrom = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    HttpContext.Current.Session["HourlyRunTimeChartData"] = null;
                    HttpContext.Current.Session["TimeAnalysisChartData"] = null;
                    HttpContext.Current.Session["StoppageReasonData"] = null;
                    HttpContext.Current.Session["HourlyPartCountChartData"] = null;
                    HttpContext.Current.Session["RunTimeChartData"] = null;
                    ViewState["NextStartPoint"] = 1;
                    ViewState["PreviousStartPoint"] = 1;
                    txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    txtExportFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                    txtExportToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    rblLeftSelction.Items[0].Selected = true;
                    rblRightSelction.Items[0].Selected = true;
                    BindShift();
                    BindPlant();
                    BindExportMachine();
                    GetCurrentShift();
                    lbNext_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void GetCurrentShift()
        {
            try
            {
                var currShiftVals = DataBaseAccess.GetCurrentOrPreviousShiftVals("[s_GetCurrentShift]");
                if (currShiftVals != null)
                {
                    ddlShift.SelectedValue= currShiftVals[3];
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void BindShift()
        {
            try
            {
                List<ListItem> list = DataBaseAccess.GetAllShiftIds();
                ddlShift.DataSource = list;
                ddlShift.DataTextField = "Text";
                ddlShift.DataValueField = "Text";
                ddlShift.DataBind();
                ddlShift.Items.Add("Day");

                lbExportShift.DataSource = list;
                lbExportShift.DataTextField = "Text";
                lbExportShift.DataValueField = "Text";
                lbExportShift.DataBind();
                foreach (ListItem item in lbExportShift.Items)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindPlant()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlantsForPlantInfo();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();
                ddlPlant_SelectedIndexChanged(null, null);

                ddlExportPlant.DataSource = list;
                ddlExportPlant.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = DataBaseAccess.GetMachineInfo(ddlPlant.SelectedValue);
                if (list.Count > 1)
                {
                    list.Insert(0,"All");
                }
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindExportMachine()
        {
            try
            {
                List<string> list = DataBaseAccess.GetMachineInfo(ddlExportPlant.SelectedValue);
                lbExporMachine.DataSource = list;
                lbExporMachine.DataBind();
                if (lbExporMachine.Items.Count > 0)
                {
                    lbExporMachine.Items[0].Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachine();
        }
        private void BindDetails()
        {
            try
            {
                HttpContext.Current.Session["HourlyRunTimeChartData"] = null;
                HttpContext.Current.Session["TimeAnalysisChartData"] = null;
                HttpContext.Current.Session["StoppageReasonData"] = null;
                HttpContext.Current.Session["HourlyPartCountChartData"] = null;
                HttpContext.Current.Session["RunTimeChartData"] = null;
                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(txtDate.Text) && ((txtDate.Text.Equals(DateTime.Now.Date.ToString("dd-MM-yyyy")) || cbAutoRefresh.Checked)))
                {
                    dt = MachineConnectDBAccess.getProductionAnalyticsData(txtDate.Text, ddlShift.SelectedValue, ddlPlant.SelectedValue, ddlMachine.SelectedValue, "s_GetFocasLiveDetailsForMultipleMac");
                }

                else
                {
                    dt = MachineConnectDBAccess.getProductionAnalyticsData(txtDate.Text, ddlShift.SelectedValue.Equals("Day") ? "" : ddlShift.SelectedValue, ddlPlant.SelectedValue, ddlMachine.SelectedValue, "s_GetAggFocasDetailsForMultipleMac");
                }

                if (cbAutoRefresh.Checked)
                {
                    timerToAutoRefresh.Enabled = true;
                    timerToAutoRefresh.Interval = 30000;
                    // BindChartFrom = "AutoRefresh";
                }
                else
                {
                    timerToAutoRefresh.Enabled = false;
                    //BindChartFrom = "";
                }
                List<ProductionAnalyticsEntity> list = new List<ProductionAnalyticsEntity>();
                ProductionAnalyticsEntity data = new ProductionAnalyticsEntity();
                //data.MachineID = "Machine ID";
                //data.Status = "Status";
                //data.RunningProgram = "Running Program";
                //data.TotalTime = "Total Time";
                //data.PowerOnTime = "Power On Time";
                //data.OperatingTime = "Operating Time";
                //data.CuttingTime = "Cutting Time";
                //data.DownTime = "Down Time";
                //data.TimeHeaderVisibility = "table-row";
                //data.Program1 = "Prog 1(#)";
                //data.Program2 = "Prog 2(#)";
                //data.Program3 = "Prog 3(#)";
                //data.Program4 = "Prog 4(#)";
                //data.TotalPartCount = "Total";
                //data.PartCountHeaderVisibility = "table-row";
                //list.Add(data);

                foreach (DataRow row in dt.Rows)
                {
                    data = new ProductionAnalyticsEntity();
                    data.MachineID = row["Machineid"].ToString();
                    data.Status = row["MachineStatus"].ToString();
                    if (data.Status.Equals("Feed Hold", StringComparison.OrdinalIgnoreCase) || data.Status.Equals("Idle", StringComparison.OrdinalIgnoreCase))
                    {
                        data.StatusBackColor = "#f2f20f";
                    }
                    else if (data.Status.Equals("In Cycle", StringComparison.OrdinalIgnoreCase) || data.Status.Equals("In Progress", StringComparison.OrdinalIgnoreCase))
                    {
                        data.StatusBackColor = "#40fc40";
                    }
                    else
                    {
                        data.StatusBackColor = "#f77777";

                    }
                    data.RunningProgram = row["RunningProgram"].ToString();
                    data.OEE = row["AvgOEE"].ToString();
                    data.TotalTime = row["TotalTime"].ToString();
                    data.PowerOnTime = row["Powerontime"].ToString();
                    data.OperatingTime = row["Operating time"].ToString();
                    data.CuttingTime = row["Cutting time"].ToString();
                    data.Program1 = row["ProgramNo1"].ToString();
                    data.Program2 = row["ProgramNo2"].ToString();
                    data.Program3 = row["ProgramNo3"].ToString();
                    data.Program4 = row["ProgramNo4"].ToString();
                    data.TotalPartCount = row["TotalPartsCount"].ToString();
                    list.Add(data);
                }
                lvProductionDetails.DataSource = list;
                lvProductionDetails.DataBind();
                Session["ProductionAnalyticsGridData"] = list;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "bindChart", "BindChart('" + BindChartFrom + "');", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            ViewState["NextStartPoint"] = 1;
            ViewState["PreviousStartPoint"] = 1;
            //BindDetails();
            lbNext_Click(null, null);
        }
        protected void lbPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                int previousStartPoint = (int)ViewState["PreviousStartPoint"];
                int noOfMachines = Convert.ToInt32(hfNoOfMachines.Value);
                string machines = "";
                if (ddlMachine.SelectedValue == "All")
                {
                    int uptoMachineCount = (previousStartPoint + noOfMachines);
                    if (uptoMachineCount > ddlMachine.Items.Count - 1)
                    {
                        uptoMachineCount = ddlMachine.Items.Count;
                    }
                    for (int i = previousStartPoint; i < uptoMachineCount; i++)
                    {
                        if (machines == "")
                        {
                            machines = ddlMachine.Items[i].Value;
                        }
                        else
                        {
                            machines += "," + ddlMachine.Items[i].Value;
                        }
                    }
                    if (previousStartPoint > 1)
                    {
                        int latestPreviousPointCount = previousStartPoint - noOfMachines;
                        ViewState["PreviousStartPoint"] = latestPreviousPointCount;
                        // ViewState["NextStartPoint"] = previousStartPoint;
                        lbPrevious.Visible = true;
                    }
                    else
                    {
                        ViewState["PreviousStartPoint"] = 1;
                        // ViewState["NextStartPoint"] = 1;
                        lbPrevious.Visible = false;
                    }
                    //if (previousStartPoint + noOfMachines <= ddlMachine.Items.Count)
                    //{
                    ViewState["NextStartPoint"] = previousStartPoint + noOfMachines;
                    //}
                    //if (previousStartPoint + noOfMachine <= 1)
                    //{
                    //    ViewState["PreviousStartPoint"] = 1;
                    //}
                    //else
                    //{
                    //    ViewState["PreviousStartPoint"] = nextStartPoint - noOfMachines;
                    //}
                    if ((previousStartPoint + noOfMachines) <= ddlMachine.Items.Count - 1)
                    {
                        lbNext.Visible = true;
                    }
                    else
                    {
                        lbNext.Visible = false;
                    }
                }
                else
                {
                    machines = ddlMachine.SelectedValue;

                    lbPrevious.Visible = false;
                    lbNext.Visible = false;

                }
                ViewState["PAMachines"] = machines;
                BindChartFrom = "";
                BindDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void lbNext_Click(object sender, EventArgs e)
        {
            try
            {
                int nextStartPoint = (int)ViewState["NextStartPoint"];
                int noOfMachines = Convert.ToInt32(hfNoOfMachines.Value);
                string machines = "";
                if (ddlMachine.SelectedValue == "All")
                {
                    int uptoMachineCount = (nextStartPoint + noOfMachines);
                    if (uptoMachineCount > ddlMachine.Items.Count - 1)
                    {
                        uptoMachineCount = ddlMachine.Items.Count;
                    }
                    for (int i = nextStartPoint; i < uptoMachineCount; i++)
                    {
                        if (machines == "")
                        {
                            machines = ddlMachine.Items[i].Value;
                        }
                        else
                        {
                            machines += "," + ddlMachine.Items[i].Value;
                        }
                    }
                    int latestNextPointCount = nextStartPoint + noOfMachines;
                    if (latestNextPointCount > ddlMachine.Items.Count - 1)
                    {
                        ViewState["NextStartPoint"] = 1;
                        //ViewState["PreviousStartPoint"] = 1;
                        lbNext.Visible = false;
                    }
                    else
                    {


                        ViewState["NextStartPoint"] = latestNextPointCount;
                        lbNext.Visible = true;
                    }
                    if (nextStartPoint - noOfMachines <= 1)
                    {
                        ViewState["PreviousStartPoint"] = 1;
                    }
                    else
                    {
                        ViewState["PreviousStartPoint"] = nextStartPoint - noOfMachines;
                    }
                    if (nextStartPoint <= 1)
                    {
                        lbPrevious.Visible = false;
                    }
                    else
                    {
                        lbPrevious.Visible = true;
                    }
                }
                else
                {

                    machines = ddlMachine.SelectedValue;
                    lbPrevious.Visible = false;
                    lbNext.Visible = false;
                }
                ViewState["PAMachines"] = machines;
                BindChartFrom = "";
                BindDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void timerToAutoRefresh_Tick(object sender, EventArgs e)
        {
            //btnOK_Click(null, null);
            BindChartFrom = "AutoRefresh";
            BindDetails();
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void makeSessionNull()
        {
            HttpContext.Current.Session["HourlyRunTimeChartData"] = null;
            HttpContext.Current.Session["TimeAnalysisChartData"] = null;
            HttpContext.Current.Session["StoppageReasonData"] = null;
            HttpContext.Current.Session["HourlyPartCountChartData"] = null;
            HttpContext.Current.Session["RunTimeChartData"] = null;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static HourlyRunTimeChartEntity getHourlyRunTimeChartData(string plant, string machine, string shift, string date, bool isautorefresh)
        {
            HourlyRunTimeChartEntity chartData = new HourlyRunTimeChartEntity();
            HourlyRunTimeChartEntity summaryData = new HourlyRunTimeChartEntity();
            try
            {

                if (!string.IsNullOrEmpty(date) && (date.Equals(DateTime.Now.Date.ToString("dd-MM-yyyy")) || isautorefresh))
                {
                    chartData = MachineConnectDBAccess.GetProductionDataDayWise(date, shift, plant, machine, "s_GetFocasLiveDetailsForMultipleMac", out summaryData);
                    //var shift = GetShiftString(shiftVal);
                    //partCountArrays = DatabaseAccessForProductionAnalytics.GetPartsCountData(dateVal, shift, plantId, machineId, "s_GetFocasHourShiftwiseLiveDetails");
                    //machineStatusArray = DatabaseAccessForProductionAnalytics.GetRuntimeDowntimeData(dateVal, shiftVal, plantId, machineId, "runtimeanddowntime", "s_GetFocasLiveDetailsForMultipleMac");
                }
                else
                {

                    chartData = MachineConnectDBAccess.GetProductionDataDayWise(date, shift.Equals("Day") ? "" : shift, plant, machine, "s_GetAggFocasDetailsForMultipleMac", out summaryData);
                    //partCountArrays = DatabaseAccessForProductionAnalytics.GetPartsCountData(dateVal, shift, plantId, machineId, "s_GetAggFocasDetailsForMultipleMac");
                    //machineStatusArray = DatabaseAccessForProductionAnalytics.GetRuntimeDowntimeData(dateVal, shift, plantId, machineId, "runtimeanddowntime", "s_GetAggFocasDetailsForMultipleMac");
                }
                HttpContext.Current.Session["HourlyRunTimeChartData"] = chartData;
                HttpContext.Current.Session["TimeAnalysisData"] = summaryData;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getHourlyRunTimeChartData: " + ex.Message);
            }
            return chartData;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<PARunChartEntity> getRunTimeChartData(string plant, string machine, string shift, string date, bool isautorefresh)
        {
            List<PARunChartEntity> data = new List<PARunChartEntity>();
            try
            {

                if (!string.IsNullOrEmpty(date) && (date.Equals(DateTime.Now.Date.ToString("dd-MM-yyyy")) || isautorefresh))
                {
                    //var shift = GetShiftString(shiftVal);
                    //partCountArrays = DatabaseAccessForProductionAnalytics.GetPartsCountData(dateVal, shift, plantId, machineId, "s_GetFocasHourShiftwiseLiveDetails");
                    //data = MachineConnectDBAccess.GetRuntimeDowntimeData(date, shift, plant, machine, "runtimeanddowntime", "s_GetFocasLiveDetailsForMultipleMac");
                    data = MachineConnectDBAccess.GetRuntimeDowntimeData(date, shift, plant, machine, "runtimeanddowntime", "s_GetFocasLiveDetailsForMultipleMac");
                }
                else
                {

                    //partCountArrays = DatabaseAccessForProductionAnalytics.GetPartsCountData(dateVal, shift, plantId, machineId, "s_GetAggFocasDetailsForMultipleMac");
                    //data = MachineConnectDBAccess.GetRuntimeDowntimeData(date, shift, plant, machine, "runtimeanddowntime", "s_GetAggFocasDetailsForMultipleMac");
                    data = MachineConnectDBAccess.GetRuntimeDowntimeData(date, shift.Equals("Day") ? "" : shift, plant, machine, "runtimeanddowntime", "s_GetAggFocasDetailsForMultipleMac");
                }

                HttpContext.Current.Session["RunTimeChartData"] = data;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getRunTimeChartData: " + ex.Message);
            }
            return data;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static StoppageReasonEntity getStoppageReasonData(string plant, string machine, string shift, string date, bool isautorefresh)
        {
            StoppageReasonEntity data = new StoppageReasonEntity();
            try
            {
                List<StoppageReasonDataEntity> datalist = new List<StoppageReasonDataEntity>();
                StoppageReasonDataEntity griddata = null;
                DataTable dt = new DataTable();
                string TotalStoppage = string.Empty;
                if (!string.IsNullOrEmpty(date) && (date.Equals(DateTime.Now.Date.ToString("dd-MM-yyyy")) || isautorefresh))
                    dt = MachineConnectDBAccess.GetStopagedata(date, shift.Equals("Day") ? "" : shift, plant, machine, "s_GetFocasLiveDetailsForMultipleMac");
                else
                    dt = MachineConnectDBAccess.GetStopagedata(date, shift.Equals("Day") ? "" : shift, plant, machine, "s_GetAggFocasDetailsForMultipleMac");
                string defaultThreshold = MachineConnectDBAccess.GetDefaultThreshold();
                if (dt != null)//vas
                {
                    if (dt.Rows.Count > 0)
                    {
                        TotalStoppage = dt.Rows[0]["TotalStoppage"].ToString();
                        TotalStoppage = " [ " + TotalStoppage + " ]";
                        foreach (DataRow row in dt.Rows)
                        {
                            griddata = new StoppageReasonDataEntity();
                            griddata.StartDate = row["Batchstart"].ToString() == "" ? "" : Convert.ToDateTime(row["Batchstart"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                            griddata.EndDate = row["BatchEnd"].ToString() == "" ? "" : Convert.ToDateTime(row["BatchEnd"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                            griddata.Value = row["Stoppagetime"].ToString();
                            //griddata.AlarmNo = row["LiveAlarmsNo"].ToString();
                            datalist.Add(griddata);
                        }

                    }
                }
                data.Title = machine + " : Stoppage ( > " + defaultThreshold + " Secs. )" + TotalStoppage;
                data.stoppageReasonDatas = datalist;
                HttpContext.Current.Session["StoppageReasonData"] = data;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getStoppageReasonData: " + ex.Message);
            }
            return data;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static HourlyRunTimeChartEntity getTimeAnalysisChartData(string plant, string machine, string shift, string date, bool isautorefresh)
        {
            HourlyRunTimeChartEntity summaryData = new HourlyRunTimeChartEntity();
            try
            {
                HourlyRunTimeChartEntity chartData = new HourlyRunTimeChartEntity();

                //if (HttpContext.Current.Session["TimeAnalysisData"] == null)
                //{
                if (!string.IsNullOrEmpty(date) && (date.Equals(DateTime.Now.Date.ToString("dd-MM-yyyy")) || isautorefresh))
                {
                    chartData = MachineConnectDBAccess.GetProductionDataDayWise(date, shift, plant, machine, "s_GetFocasLiveDetailsForMultipleMac", out summaryData);
                }
                else
                {

                    chartData = MachineConnectDBAccess.GetProductionDataDayWise(date, shift.Equals("Day") ? "" : shift, plant, machine, "s_GetAggFocasDetailsForMultipleMac", out summaryData);
                }
                //}
                //else
                //{
                //    summaryData = (HourlyRunTimeChartDetails)HttpContext.Current.Session["TimeAnalysisData"];
                //}
                HttpContext.Current.Session["TimeAnalysisChartData"] = summaryData;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getTimeAnalysisChartData: " + ex.Message);
            }
            return summaryData;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static PartCountChartEntity getHourlyPartCountChartData(string plant, string machine, string shift, string date, string shiftforhour, bool isautorefresh)
        {
            PartCountChartEntity partcountData = new PartCountChartEntity();
            try
            {
                HourlyRunTimeChartEntity chartData = new HourlyRunTimeChartEntity();

                if (!string.IsNullOrEmpty(date) && (date.Equals(DateTime.Now.Date.ToString("dd-MM-yyyy")) || isautorefresh))
                {
                    string shiftval = "";

                    if (shift == "Day")
                    {
                        string[] allshift = shiftforhour.Split(':');
                        for (int i = 0; i < allshift.Length; i++)
                        {
                            if (shiftval == "")
                            {
                                shiftval = "'" + allshift[i] + "'";
                            }
                            else
                            {
                                if (allshift[i] != "Day")
                                {
                                    shiftval = shiftval + "," + "N'" + allshift[i] + "'";
                                }
                            }
                        }
                    }
                    else
                    {
                        shiftval = "'" + shift + "'";
                    }
                    partcountData = MachineConnectDBAccess.GetPartsCountData(date, shiftval, plant, machine, "s_GetFocasHourShiftwiseLiveDetails");
                }
                else
                {
                    var shiftname = shift.Equals("Day") ? "" : shift;
                    partcountData = MachineConnectDBAccess.GetPartsCountData(date, shiftname, plant, machine, "s_GetAggFocasDetailsForMultipleMac");
                }
                HttpContext.Current.Session["HourlyPartCountChartData"] = partcountData;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getHourlyPartCountChartData: " + ex.Message);
            }
            return partcountData;
        }



        protected void btnExport_Click(object sender, EventArgs e)
        {
            HelperClassGeneric.openModal(this, "exportModal", false);
        }

        protected void ddlExportPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindExportMachine();
            HelperClassGeneric.openModal(this, "exportModal", false);
        }
        protected void btnExportConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                #region
                string shiftIds = string.Empty;
                string machineids = string.Empty;
                string selectedPlant = string.Empty;


                foreach (ListItem item in lbExporMachine.Items)
                {
                    if (item.Selected)
                    {
                        if (machineids == string.Empty)
                        {
                            machineids = "'" + item.Text + "'";
                        }
                        else
                        {
                            machineids = machineids + ",'" + item.Text.ToString().Trim() + "'";
                        }
                    }
                }

                if (machineids == string.Empty)
                {
                    HelperClassGeneric.openWarningModal(this, "Please select Machine ID");
                    HelperClassGeneric.openModal(this, "exportModal", false);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "bindChart", "BindChart('" + BindChartFrom + "');", true);
                    return;
                }


                foreach (ListItem item in lbExportShift.Items)
                {
                    if (item.Selected)
                    {
                        if (shiftIds == string.Empty)
                        {
                            shiftIds = "'" + item.Text + "'";
                        }
                        else
                        {
                            shiftIds = shiftIds + "," + "'" + item.Text + "'";
                        }
                    }

                }
                if (shiftIds == string.Empty)
                {
                    HelperClassGeneric.openWarningModal(this, "Please select shift");
                    HelperClassGeneric.openModal(this, "exportModal", false);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "bindChart", "BindChart('" + BindChartFrom + "');", true);
                    return;
                }
                DateTime fromDateInDate = Util.GetDateTime(txtExportFromDate.Text);
                DateTime toDateInDate = Util.GetDateTime(txtExportToDate.Text);

                double totdays = (toDateInDate - fromDateInDate).TotalDays;
                if (ddlExportReportBy.SelectedValue.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                {
                    if (totdays > 31)
                    {
                        HelperClassGeneric.openWarningModal(this, "Date should not be geater than 31 day");
                        HelperClassGeneric.openModal(this, "exportModal", false);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "bindChart", "BindChart('" + BindChartFrom + "');", true);
                        return;
                    }
                }
                else if (ddlExportReportBy.SelectedValue.Equals("Hour", StringComparison.OrdinalIgnoreCase))
                {
                    if (totdays > 7)
                    {
                        HelperClassGeneric.openWarningModal(this, "Date should not be geater than 7 day");
                        HelperClassGeneric.openModal(this, "exportModal", false);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "bindChart", "BindChart('" + BindChartFrom + "');", true);
                        return;
                    }
                }

                else if (ddlExportReportBy.SelectedValue.Equals("Day", StringComparison.OrdinalIgnoreCase))
                {
                    if (totdays > 31)
                    {

                        HelperClassGeneric.openWarningModal(this, "Date should not be geater than 31 day");
                        HelperClassGeneric.openModal(this, "exportModal", false);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "bindChart", "BindChart('" + BindChartFrom + "');", true);
                        return;
                    }
                }
                selectedPlant = ddlExportPlant.SelectedValue;
                #endregion

                string filePath = string.Empty;
                string fromDate = txtExportFromDate.Text;
                string toDate = txtExportToDate.Text;
                List<ProductionData> pDatas = new List<ProductionData>();
                List<StoppageDataEntity> sDatas = new List<StoppageDataEntity>();
                List<string> programnumber = new List<string>();
                List<string> partsCount = new List<string>();
                List<string> OEE = new List<string>();
                List<string> StoppageDuration = new List<string>();

                ProductionAnalysisForExcelSummary summaryData = new ProductionAnalysisForExcelSummary();
                string ReportStatus = "";
                if (ddlReportType.SelectedValue.Equals("ProducionAnalysis", StringComparison.OrdinalIgnoreCase))
                {
                    if (ddlExportReportBy.SelectedValue.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                    {
                        pDatas = MachineConnectDBAccess.GetProductionData(fromDate, toDate, shiftIds, selectedPlant, machineids, "Shift", out summaryData, out programnumber, out partsCount, out OEE);
                        ReportStatus = MachineConnectGenerateReport.ExportProductionReportEpplus(pDatas, summaryData, programnumber, partsCount, fromDate, toDate, ddlPlant.SelectedValue, "Shift", OEE);
                    }
                    else if (ddlExportReportBy.SelectedValue.Equals("Day", StringComparison.OrdinalIgnoreCase))
                    {
                        pDatas = MachineConnectDBAccess.GetProductionData(fromDate, toDate, shiftIds, selectedPlant, machineids, "Day", out summaryData, out programnumber, out partsCount, out OEE);
                        ReportStatus = MachineConnectGenerateReport.ExportProductionReportEpplus(pDatas, summaryData, programnumber, partsCount, fromDate, toDate, ddlPlant.SelectedValue, "Day", OEE);
                    }
                    else
                    {
                        pDatas = MachineConnectDBAccess.GetProductionData(fromDate, toDate, shiftIds, selectedPlant, machineids, "Hour", out summaryData, out programnumber, out partsCount, out OEE);
                        ReportStatus = MachineConnectGenerateReport.ExportProductionReportEpplus(pDatas, summaryData, programnumber, partsCount, fromDate, toDate, ddlPlant.SelectedValue, "Hour", OEE);
                    }
                }

                if (ddlReportType.SelectedValue.Equals("Stoppages", StringComparison.OrdinalIgnoreCase))
                {
                    if (ddlExportReportBy.SelectedValue.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                    {
                        sDatas = MachineConnectDBAccess.GetStoppageData(fromDate, toDate, shiftIds, selectedPlant, machineids, "Stoppages", "Shift");
                        ReportStatus = MachineConnectGenerateReport.ExportStoppageReport(sDatas, fromDate, toDate, ddlPlant.SelectedValue, "Shift");
                    }
                    else if (ddlExportReportBy.SelectedValue.Equals("Day", StringComparison.OrdinalIgnoreCase))
                    {
                        sDatas = MachineConnectDBAccess.GetStoppageData(fromDate, toDate, shiftIds, selectedPlant, machineids, "Stoppages", "DAY");
                        ReportStatus = MachineConnectGenerateReport.ExportStoppageReport(sDatas, fromDate, toDate, ddlPlant.SelectedValue, "Day");
                    }
                    else
                    {
                        sDatas = MachineConnectDBAccess.GetStoppageData(fromDate, toDate, shiftIds, selectedPlant, machineids, "Stoppages", "Hour");
                        ReportStatus = MachineConnectGenerateReport.ExportStoppageReport(sDatas, fromDate, toDate, ddlPlant.SelectedValue, "Hour");
                    }
                }
                if (ReportStatus.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openSuccessModal(this, "Report Generated");
                    HelperClassGeneric.openModal(this, "exportModal", false);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "bindChart", "BindChart('" + BindChartFrom + "');", true);
                }
                else if (ReportStatus.Equals("NoData", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openWarningToastrModal(this, "No data found");
                    HelperClassGeneric.openModal(this, "exportModal", false);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "bindChart", "BindChart('" + BindChartFrom + "');", true);
                }
                else
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Try again");
                    HelperClassGeneric.openModal(this, "exportModal", false);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "bindChart", "BindChart('" + BindChartFrom + "');", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}