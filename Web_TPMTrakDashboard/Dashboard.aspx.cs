using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModelClassLibrary;
using BusinessClassLibrary;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data;
using Web_TPMTrakDashboard.Models;
using System.Threading;
using System.Globalization;
using System.Diagnostics;
using Elmah;
using System.Configuration;

namespace Web_TPMTrakDashboard
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public static string parameter { get; set; }
        public static List<ColumnViewSetting> setting = null;
        public static AppUISettings model = null;
        public int fontSize = 20;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Language"] == null || Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (Request.QueryString["dbname"] != null)
            {
                Session["connectionString"] = Request.QueryString["dbname"].ToString();
            }
            if (!IsPostBack)
            {
                //Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Language"] == null ? Thread.CurrentThread.CurrentCulture.Name : Session["Language"].ToString());
                //Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["Language"] == null ? Thread.CurrentThread.CurrentCulture.Name : Session["Language"].ToString());
                SessionClear.ClearSession();
                Session["QEVisibility"] = DataBaseAccess.isQERequired();
                BindPlantId();
                BindCellId(ddlPlantId.SelectedItem == null ? "" : ddlPlantId.SelectedItem.Text);
                BindComponentInfo();
                BindEmployeeInfo();
                BindShiftData();
                if (ConfigurationManager.AppSettings["sonapages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    txtYear.Text = DateTime.Now.Year.ToString();
                    txtMonth.Text = DateTime.Now.Month.ToString("00");
                }

                setting = DataBaseAccess.BindSettingPage("WebTPMTrak", Session["Language"] == null ? "en" : Session["Language"].ToString());
                model = DataBaseAccess.ViewAppUISettings();
                if (model.DownTime == "mm")
                    model.DownTime = GetGlobalResourceObject("CommanResource", "mm").ToString();
                else
                    model.DownTime = GetGlobalResourceObject("CommanResource", "hh").ToString();
                ddlViewType.SelectedValue = DataBaseAccess.AppDefaultView;
            }
            fontSize = Convert.ToInt32(Session["fontSize"]);
            setdate();
        }

        private void setdate()
        {
            DateTime SetDate = DateTime.Now;
            string getyeartype = DataBaseAccess.GetType("DashboadYearType");
            if (getyeartype.Equals("LastYear", StringComparison.OrdinalIgnoreCase))
            {
                txtYear.Text = DateTime.Now.AddYears(-1).Year.ToString();
            }
            else if (getyeartype.Equals("ThisYear", StringComparison.OrdinalIgnoreCase))
            {
                txtYear.Text = DateTime.Now.Year.ToString();
            }
            else if (getyeartype.Equals("None", StringComparison.OrdinalIgnoreCase))
            {

                getyeartype = DataBaseAccess.GetType("DashboadMonthType");
                if (getyeartype.Equals("LastMonth", StringComparison.OrdinalIgnoreCase))
                {
                    txtMonth.Text = DateTime.Now.AddMonths(-1).Month.ToString("00");
                    txtYear.Text = DateTime.Now.AddMonths(-1).Year.ToString();
                }
                else if (getyeartype.Equals("ThisMonth", StringComparison.OrdinalIgnoreCase))
                {
                    txtMonth.Text = DateTime.Now.Month.ToString("00");
                    txtYear.Text = DateTime.Now.Year.ToString();
                }
                else if ((getyeartype.Equals("None", StringComparison.OrdinalIgnoreCase)))
                {

                    getyeartype = DataBaseAccess.GetType("DashboadDateType");
                    if (getyeartype.Equals("Yesterday", StringComparison.OrdinalIgnoreCase))
                    {
                        txtDay.Text = DateTime.Now.AddDays(-1).Day.ToString("00");
                        txtMonth.Text = DateTime.Now.AddDays(-1).Month.ToString("00");
                        txtYear.Text = DateTime.Now.AddDays(-1).Year.ToString();
                    }
                    else if (getyeartype.Equals("Daybeforeyesterday", StringComparison.OrdinalIgnoreCase))
                    {
                        txtDay.Text = DateTime.Now.AddDays(-2).Day.ToString("00");
                        txtMonth.Text = DateTime.Now.AddDays(-2).Month.ToString("00");
                        txtYear.Text = DateTime.Now.AddDays(-2).Year.ToString();
                    }
                    else if (getyeartype.Equals("Today", StringComparison.OrdinalIgnoreCase))
                    {
                        txtDay.Text = DateTime.Now.Day.ToString("00");
                        txtMonth.Text = DateTime.Now.Month.ToString("00");
                        txtYear.Text = DateTime.Now.Year.ToString();
                    }
                    else
                    {
                        txtYear.Text = DateTime.Now.Year.ToString();
                        txtMonth.Text = DateTime.Now.Month.ToString("00");
                        txtDay.Text = DateTime.Now.Day.ToString("00");
                    }
                }
                else
                {
                    txtYear.Text = DateTime.Now.Year.ToString();
                    txtMonth.Text = DateTime.Now.Month.ToString("00");
                }
            }
            else
            {
                txtYear.Text = DateTime.Now.Year.ToString();
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
            }
            catch (Exception ex)
            {
                // ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Cell Id"
        private void BindCellId(string plantId)
        {
            try
            {
                List<string> lstCellId = BindCockpitView.ViewCellsToDisplay(plantId == "Plant All" ? "" : plantId);
                ddlCellId.DataSource = lstCellId;
                ddlCellId.DataBind();
                ddlCellId.Items.Insert(0, new ListItem
                {
                    Text = GetGlobalResourceObject("CommanResource", "CellAll").ToString(),
                    Value = "All"
                });
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Component Info"
        private void BindComponentInfo()
        {
            try
            {
                List<string> lstPlantData = BindCockpitView.ViewComponentInfo();
                ddlComponentInfo.DataSource = lstPlantData;
                ddlComponentInfo.DataBind();
                ddlComponentInfo.Items.Insert(0, new ListItem
                {
                    Text = GetGlobalResourceObject("CommanResource", "ComponentAll").ToString(),
                    Value = "All"
                });
            }
            catch (Exception ex)
            {
                // ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Employee Info"
        private void BindEmployeeInfo()
        {
            try
            {
                List<EmployeeInfo> lstPlantData = BindCockpitView.ViewEmployeeInfo();
                ddlEmployeeInfo.DataSource = lstPlantData;
                ddlEmployeeInfo.DataValueField = "EmpID";
                ddlEmployeeInfo.DataTextField = "EmpName";
                ddlEmployeeInfo.DataBind();
                ddlEmployeeInfo.Items.Insert(0, new ListItem
                {
                    Text = GetGlobalResourceObject("CommanResource", "EmployeeAll").ToString(),
                    //"Employee All",
                    Value = "All"
                });
            }
            catch (Exception ex)
            {
                // ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Shift Data------------------------"
        private void BindShiftData()
        {
            try
            {
                var allShift = BindCockpitView.GetAllShift();
                if (allShift != null && allShift.Count > 0)
                {
                    ddlShift.DataSource = allShift;
                    ddlShift.DataBind();
                    //for (int i = 0; i < allShift.Count; i++)
                    //{
                    //    ddlShift.Items.Insert(0, new ListItem(GetLocalResourceObject("ddlShiftID").ToString(), allShift[i]));
                    //}

                    ddlShift.Items.Insert(0, new ListItem
                    {
                        Text = GetGlobalResourceObject("CommanResource", "ShiftAll").ToString(),
                        Value = "All"
                    });

                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Dashboard Details"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<DashboardDetails> BindDashboardData(string plantId, string strYear, string strMonth, string strDay, string strShift, string componentId, string employeeId, string cellId, string windowSize, string viewType)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<DashboardDetails> obj = new List<DashboardDetails>();
            if (strShift.Equals("All", StringComparison.OrdinalIgnoreCase))
                strShift = "";
            if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
                plantId = "";

            if (componentId.Equals("All", StringComparison.OrdinalIgnoreCase))
                componentId = "";
            if (employeeId.Equals("All", StringComparison.OrdinalIgnoreCase))
                employeeId = "";
            if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase))
                cellId = "";
            HttpContext.Current.Session["WindowSize"] = windowSize;

            #region "Month condition------------------"
            if (strMonth.Equals("Jan", StringComparison.OrdinalIgnoreCase))
                strMonth = "01";
            else if (strMonth.Equals("Feb", StringComparison.OrdinalIgnoreCase))
                strMonth = "02";
            else if (strMonth.Equals("Mar", StringComparison.OrdinalIgnoreCase))
                strMonth = "03";
            else if (strMonth.Equals("Apr", StringComparison.OrdinalIgnoreCase))
                strMonth = "04";
            else if (strMonth.Equals("May", StringComparison.OrdinalIgnoreCase))
                strMonth = "05";
            else if (strMonth.Equals("Jun", StringComparison.OrdinalIgnoreCase))
                strMonth = "06";
            else if (strMonth.Equals("Jul", StringComparison.OrdinalIgnoreCase))
                strMonth = "07";
            else if (strMonth.Equals("Aug", StringComparison.OrdinalIgnoreCase))
                strMonth = "08";
            else if (strMonth.Equals("Sep", StringComparison.OrdinalIgnoreCase))
                strMonth = "09";
            else if (strMonth.Equals("Oct", StringComparison.OrdinalIgnoreCase))
                strMonth = "10";
            else if (strMonth.Equals("Nov", StringComparison.OrdinalIgnoreCase))
                strMonth = "11";
            else if (strMonth.Equals("Dec", StringComparison.OrdinalIgnoreCase))
                strMonth = "12";
            #endregion

            if (strYear != "" && strMonth != "" && strDay != "")
            {
                strYear = strYear + "-" + strMonth + "-" + strDay;
                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase) || viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase) || viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                    obj = BindCockpitView.BindTableData(strYear, strShift, plantId, "", "DAY", "", componentId, employeeId, cellId, viewType);
                else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    obj = BindCockpitView.BindTableComponentAndOperatorData(strYear, strShift, plantId, employeeId, "DAY", "", "s_GetAggDrilldownTPMTrakComponentData_Grid", cellId, viewType, componentId);
                else
                    obj = BindCockpitView.BindTableComponentAndOperatorData(strYear, strShift, plantId, employeeId, "DAY", "", "s_GetAggDrilldownTPMTrakOperatorData_Grid", cellId, viewType, componentId);
            }
            else if (strYear != "" && strMonth != "")
            {
                strYear = strYear + "-" + strMonth + "-" + "01";
                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase) || viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase) || viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                    obj = BindCockpitView.BindTableData(strYear, strShift, plantId, "", "MONTH", "", componentId, employeeId, cellId, viewType);
                else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    obj = BindCockpitView.BindTableComponentAndOperatorData(strYear, strShift, plantId, employeeId, "MONTH", "", "s_GetAggDrilldownTPMTrakComponentData_Grid", cellId, viewType, componentId);
                else
                    obj = BindCockpitView.BindTableComponentAndOperatorData(strYear, strShift, plantId, employeeId, "MONTH", "", "s_GetAggDrilldownTPMTrakOperatorData_Grid", cellId, viewType, componentId);
            }
            else if (strYear != "")
            {
                strYear = strYear + "-" + "01" + "-" + "01";
                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase) || viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase) || viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                    obj = BindCockpitView.BindTableData(strYear, strShift, plantId, "", "YEAR", "", componentId, employeeId, cellId, viewType);
                else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    obj = BindCockpitView.BindTableComponentAndOperatorData(strYear, strShift, plantId, employeeId, "YEAR", "", "s_GetAggDrilldownTPMTrakComponentData_Grid", cellId, viewType, componentId);
                else
                    obj = BindCockpitView.BindTableComponentAndOperatorData(strYear, strShift, plantId, employeeId, "YEAR", "", "s_GetAggDrilldownTPMTrakOperatorData_Grid", cellId, viewType, componentId);
            }
            ICockpitStyle colorSetting = CockpitDataBaseAccess.GetCockpitBackColorValues();
            try
            {
                foreach (DashboardDetails entity in obj)
                {
                    if (string.IsNullOrEmpty(entity.AEColor))
                    {
                        entity.AEColor = "white";
                    }
                    else if (entity.AEColor.Equals("Green", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.AEColor = colorSetting.GoodRunning.Remove(1, 2);
                    }
                    else if (entity.AEColor.Equals("Red", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.AEColor = colorSetting.BadlyRunning.Remove(1, 2);
                    }
                    else if (entity.AEColor.Equals("yellow", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.AEColor = colorSetting.ModeratelyRunning.Remove(1, 2);
                    }

                    if (string.IsNullOrEmpty(entity.PEColor))
                    {
                        entity.PEColor = "white";
                    }
                    else if (entity.PEColor.Equals("Green", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.PEColor = colorSetting.GoodRunning.Remove(1, 2);
                    }
                    else if (entity.PEColor.Equals("Red", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.PEColor = colorSetting.BadlyRunning.Remove(1, 2);
                    }
                    else if (entity.PEColor.Equals("yellow", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.PEColor = colorSetting.ModeratelyRunning.Remove(1, 2);
                    }

                    if (string.IsNullOrEmpty(entity.QEColor))
                    {
                        entity.QEColor = "white";
                    }
                    else if (entity.QEColor.Equals("Green", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.QEColor = colorSetting.GoodRunning.Remove(1, 2);
                    }
                    else if (entity.QEColor.Equals("Red", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.QEColor = colorSetting.BadlyRunning.Remove(1, 2);
                    }
                    else if (entity.QEColor.Equals("yellow", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.QEColor = colorSetting.ModeratelyRunning.Remove(1, 2);
                    }

                    if (string.IsNullOrEmpty(entity.OEEColor))
                    {
                        entity.OEEColor = "white";
                    }
                    else if (entity.OEEColor.Equals("Green", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.OEEColor = colorSetting.GoodRunning.Remove(1, 2);
                    }
                    else if (entity.OEEColor.Equals("Red", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.OEEColor = colorSetting.BadlyRunning.Remove(1, 2);
                    }
                    else if (entity.OEEColor.Equals("yellow", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.OEEColor = colorSetting.ModeratelyRunning.Remove(1, 2);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                //throw;
            }

            stopwatch.Stop();
            Logger.WriteDebugLog("BindDashboardTableData : " + stopwatch.Elapsed.TotalSeconds);
            return obj;
        }
        #endregion

        #region "Shift Wise Chart Data---------"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Chart<Series_VDG> GetShiftChartData(string plantId, string strYear, string strMonth, string strDay, string param, string machineId, string type, string strShift, string componentId, string employeeId, string cellId, string paramText, string viewType)
        {
            string date = string.Empty;
            Chart<Series_VDG> chartData = null;
            bool isCombined = BindCockpitView.IsChartCombined_OEEDAshboard();
            if (isCombined)
                paramText = "Efficiency (%)";
            string CombinedCharts = isCombined ? BindCockpitView.GetCombinedChartNames_OEEDAshboard() : param;
            var distCharts = CombinedCharts.Split(',');
            if (!distCharts.Contains(param) )//|| viewType == "ComponentwiseView" || viewType == "OperatorwiseView")
            {
                distCharts = new string[0];
                distCharts = new string[] { param };
            }
            chartData = new Chart<Series_VDG>
            {
                Title = isCombined ? "Efficiency (%)" : paramText,
                Subtitle = "SubTitle",
                XAxisTitle = paramText + " : " + machineId + " (" + strYear + "-" + strMonth + "-" + strDay + ")",
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };
            List<string> shiftData = new List<string>();
            DataTable dtYear = new DataTable();
            DataTable dtHours = new DataTable();


            if (strShift.Equals("All", StringComparison.OrdinalIgnoreCase))
                strShift = "";
            if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
                plantId = "";
            if (componentId.Equals("All", StringComparison.OrdinalIgnoreCase))
                componentId = "";
            if (employeeId.Equals("All", StringComparison.OrdinalIgnoreCase))
                employeeId = "";
            if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase))
                cellId = "";

            date = strYear + "-" + strMonth + "-" + strDay;

            string sortOrder = "Machine";
            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                sortOrder = "Machineid";
            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
            {
                sortOrder = "plantid";
                plantId = machineId;
                machineId = string.Empty;
            }
            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
            {
                sortOrder = "cellid";
                cellId = machineId;
                machineId = string.Empty;
            }
            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
            {
                sortOrder = "Componentid";
                componentId = machineId;
                machineId = string.Empty;
            }
            else if (viewType.Equals("OperatorwiseView", StringComparison.OrdinalIgnoreCase))
            {
                sortOrder = "operatorid";
                employeeId = machineId;
                machineId = string.Empty;
            }
            try
            {
                chartData.series = new List<Series_VDG>();
                Series_VDG charttdataaa = null;
                foreach (var chartt in distCharts)
                {
                    param = chartt;
                    dtYear = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machineId, "CurrentSHIFT", param, componentId, employeeId, cellId, sortOrder, "", viewType);

                    //------------------------Shift Wise Start--------------------------
                    charttdataaa = new Series_VDG();
                    charttdataaa.type = type;
                    //charttdataaa.name = "Shift";
                    charttdataaa.name = param;
                    //charttdataaa.data = new List<Data>();
                    charttdataaa.data = new List<double>();
                    charttdataaa.Category = new List<string>();
                    charttdataaa.drilldown = new List<string>();
                    //Data dataa = null;
                    foreach (DataRow item in dtYear.Rows)
                    {
                        if (!DataBaseAccess.HourlyLevelDrillDown)
                        {
                            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                            {
                                //dataa = new Data();
                                //dataa.name = item["Shift"].ToString();
                                //dataa.y = Convert.IsDBNull(item["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0));
                                //dataa.drilldown = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["PlantID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + "/" + item["Month"].ToString() + "/" + item["Day"].ToString() + "/" + item["Shift"].ToString();
                                //dataa.afterTitel = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["PlantID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + " (" + strYear + "-" + item["Month"].ToString() + "-" + item["Day"].ToString() + "-" + item["Shift"].ToString() + ")";
                                //dataa.beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["PlantID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + " (" + strYear + "-" + item["Month"].ToString() + "-" + item["Day"].ToString() + "-" + item["Shift"].ToString() + ")";
                                //charttdataaa.data.Add(dataa);
                                charttdataaa.data.Add(Convert.IsDBNull(item["Parameter"]) ? 0 : Convert.IsDBNull(item["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0)));
                                charttdataaa.Category.Add(item["Shift"].ToString());
                                charttdataaa.drilldown.Add((viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["PlantID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + "/" + item["Month"].ToString() + "/" + item["Day"].ToString() + "/" + item["Shift"].ToString());
                            }
                            else
                            {
                                //dataa = new Data();
                                //dataa.name = item["Shift"].ToString();
                                //if (!Convert.IsDBNull(item["Parameter"]))
                                //{
                                //    dataa.y = Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0));
                                //}
                                //charttdataaa.data.Add(dataa);
                                charttdataaa.data.Add(Convert.IsDBNull(item["Parameter"]) ? 0 : Convert.IsDBNull(item["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0)));
                                charttdataaa.Category.Add(item["Shift"].ToString());

                            }
                        }
                        else
                        {
                            //dataa = new Data();
                            //dataa.name = item["Shift"].ToString();
                            //if (!Convert.IsDBNull(item["Parameter"]))
                            //{
                            //    dataa.y = Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0));
                            //}
                            //dataa.drilldown = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["PlantID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + "/" + item["Month"].ToString() + "/" + item["Day"].ToString() + "/" + item["Shift"].ToString();
                            //dataa.afterTitel = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["PlantID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + " (" + strYear + "-" + item["Month"].ToString() + "-" + item["Day"].ToString() + "-" + item["Shift"].ToString() + ")";
                            //dataa.beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["PlantID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + " (" + strYear + "-" + item["Month"].ToString() + "-" + item["Day"].ToString() + "-" + item["Shift"].ToString() + ")";
                            //charttdataaa.data.Add(dataa);
                            charttdataaa.data.Add(Convert.IsDBNull(item["Parameter"]) ? 0 : Convert.IsDBNull(item["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0)));
                            charttdataaa.Category.Add(item["Shift"].ToString());
                            charttdataaa.drilldown.Add((viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["PlantID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + "/" + item["Month"].ToString() + "/" + item["Day"].ToString() + "/" + item["Shift"].ToString());
                        }
                        shiftData.Add(item["Shift"].ToString());
                    }
                    chartData.series.Add(charttdataaa);
                }
                //-------------------------------Hours Wise Info-----------------------------------
                if (DataBaseAccess.HourlyLevelDrillDown)
                {
                    #region Cmntd
                    //chart.drilldown = new List<DrildownSeries>();
                    //int i = 0;
                    //shiftData = (from w in shiftData select w).Distinct().ToList();
                    //dtHours = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machineId, "CurrentDAY", param, componentId, employeeId, sortOrder, "", viewType);

                    ////if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    ////	dtHours = BindCockpitView.BindDashBoardHourGraph(date, strShift, plantId, machineId, "CurrentDAY", param, componentId, employeeId, (viewType == "MachinewiseView" ? "MachineID" : "Plantid"), "", viewType);
                    ////else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    ////	dtHours = BindCockpitView.BindDashBoardHourGraph(date, strShift, machineId, "", "CurrentDAY", param, componentId, employeeId, (viewType == "MachinewiseView" ? "MachineID" : "Plantid"), "", viewType);
                    ////else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    ////	dtHours = BindCockpitView.BindDashBoardHourGraphComponetAndOperator(date, strShift, plantId, "", "CurrentDAY", param, machineId, employeeId, "componentid", "", "s_GetHourAggDrilldownTPMTrakData_Component_Graph");
                    ////else
                    ////	dtHours = BindCockpitView.BindDashBoardHourGraphComponetAndOperator(date, strShift, plantId, "", "CurrentDAY", param, componentId, machineId, "OperatorID", "", "s_GetHourAggDrilldownTPMTrakData_Operator_Graph");

                    //foreach (var shift in shiftData)
                    //{
                    //	chart.drilldown.Add(new DrildownSeries
                    //	{
                    //		type = type,
                    //		//name = machineId,
                    //		id = machineId + "/" + strMonth + "/" + strDay + "/" + shift,
                    //		data = new List<DrildownData>(),
                    //	});
                    //	DataRow[] results = null;
                    //	if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //		results = dtHours.Select("Month = " + strMonth + " AND MachineID ='" + machineId + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                    //	else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //		results = dtHours.Select("Month = " + strMonth + " AND PlantID ='" + machineId + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                    //	else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    //		results = dtHours.Select("Month = " + strMonth + " AND ComponentID ='" + machineId + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                    //	else
                    //		results = dtHours.Select("Month = " + strMonth + " AND OperatorID ='" + machineId + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");

                    //	foreach (DataRow drshift in results)
                    //	{
                    //		if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //		{
                    //			chart.drilldown[i].data.Add(new DrildownData
                    //			{
                    //				name = drshift["HourID"].ToString(),
                    //				y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                    //				drilldown = drshift["MachineID"].ToString() + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["ShiftName"].ToString(),
                    //				afterTitel = paramText + " : " + drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["ShiftName"].ToString() + ")",
                    //				beforeTitle = paramText + " : " + drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["ShiftName"].ToString() + ")"
                    //			});
                    //		}
                    //		else
                    //		{
                    //			chart.drilldown[i].data.Add(new DrildownData
                    //			{
                    //				name = drshift["HourID"].ToString(),
                    //				y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                    //			});
                    //		}
                    //	}
                    //	i++;
                    //} 
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
                //throw ex;
            }
            //_r.Next(100)
            return chartData;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Chart<Series> GetShiftChartData_old(string plantId, string strYear, string strMonth, string strDay, string param, string machineId, string type, string strShift, string componentId, string employeeId, string cellId, string paramText, string viewType)
        {
            string date = string.Empty;
            Chart<Series> chartData = null;
            chartData = new Chart<Series>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = paramText + " : " + machineId + " (" + strYear + "-" + strMonth + "-" + strDay + ")",
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };
            List<string> shiftData = new List<string>();
            DataTable dtYear = new DataTable();
            DataTable dtHours = new DataTable();
            bool isCombined = BindCockpitView.IsChartCombined_OEEDAshboard();
            string CombinedCharts = isCombined ? BindCockpitView.GetCombinedChartNames_OEEDAshboard() : param;
            var distCharts = CombinedCharts.Split(',');

            if (strShift.Equals("All", StringComparison.OrdinalIgnoreCase))
                strShift = "";
            if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
                plantId = "";
            if (componentId.Equals("All", StringComparison.OrdinalIgnoreCase))
                componentId = "";
            if (employeeId.Equals("All", StringComparison.OrdinalIgnoreCase))
                employeeId = "";
            if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase))
                cellId = "";

            date = strYear + "-" + strMonth + "-" + strDay;

            string sortOrder = "Machine";
            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                sortOrder = "Machineid";
            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
            {
                sortOrder = "plantid";
                plantId = machineId;
                machineId = string.Empty;
            }
            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
            {
                sortOrder = "cellid";
                cellId = machineId;
                machineId = string.Empty;
            }
            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
            {
                sortOrder = "Componentid";
                componentId = machineId;
                machineId = string.Empty;
            }
            else if (viewType.Equals("OperatorwiseView", StringComparison.OrdinalIgnoreCase))
            {
                sortOrder = "operatorid";
                employeeId = machineId;
                machineId = string.Empty;
            }


            try
            {
                chartData.series = new List<Series>();
                Series charttdataaa = null;
                foreach (var chartt in distCharts)
                {
                    param = chartt;
                    dtYear = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machineId, "CurrentSHIFT", param, componentId, employeeId, cellId, sortOrder, "", viewType);

                    //------------------------Shift Wise Start--------------------------
                    charttdataaa = new Series();
                    charttdataaa.type = type;
                    charttdataaa.name = "Shift";
                    charttdataaa.data = new List<Data>();
                    Data dataa = null;
                    foreach (DataRow item in dtYear.Rows)
                    {
                        if (!DataBaseAccess.HourlyLevelDrillDown)
                        {
                            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                            {
                                dataa = new Data();
                                dataa.name = item["Shift"].ToString();
                                if (!Convert.IsDBNull(item["Parameter"]))
                                {
                                    dataa.y = Convert.ToInt32(Convert.ToDecimal(item["Parameter"].ToString()));
                                }
                                dataa.drilldown = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["PlantID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + "/" + item["Month"].ToString() + "/" + item["Day"].ToString() + "/" + item["Shift"].ToString();
                                dataa.afterTitel = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["PlantID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + " (" + strYear + "-" + item["Month"].ToString() + "-" + item["Day"].ToString() + "-" + item["Shift"].ToString() + ")";
                                dataa.beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["PlantID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + " (" + strYear + "-" + item["Month"].ToString() + "-" + item["Day"].ToString() + "-" + item["Shift"].ToString() + ")";
                            }
                            else
                            {
                                dataa = new Data();
                                dataa.name = item["Shift"].ToString();
                                if (!Convert.IsDBNull(item["Parameter"]))
                                {
                                    dataa.y = Convert.ToInt32(Convert.ToDecimal(item["Parameter"].ToString()));
                                }
                            }
                        }
                        else
                        {
                            dataa = new Data();
                            dataa.name = item["Shift"].ToString();
                            if (!Convert.IsDBNull(item["Parameter"]))
                            {
                                dataa.y = Convert.ToInt32(Convert.ToDecimal(item["Parameter"].ToString()));
                            }
                            dataa.drilldown = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["PlantID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + "/" + item["Month"].ToString() + "/" + item["Day"].ToString() + "/" + item["Shift"].ToString();
                            dataa.afterTitel = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["PlantID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + " (" + strYear + "-" + item["Month"].ToString() + "-" + item["Day"].ToString() + "-" + item["Shift"].ToString() + ")";
                            dataa.beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["PlantID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + " (" + strYear + "-" + item["Month"].ToString() + "-" + item["Day"].ToString() + "-" + item["Shift"].ToString() + ")";
                        }
                        shiftData.Add(item["Shift"].ToString());
                    }
                }
                //-------------------------------Hours Wise Info-----------------------------------
                if (DataBaseAccess.HourlyLevelDrillDown)
                {
                    #region Cmntd
                    //chart.drilldown = new List<DrildownSeries>();
                    //int i = 0;
                    //shiftData = (from w in shiftData select w).Distinct().ToList();
                    //dtHours = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machineId, "CurrentDAY", param, componentId, employeeId, sortOrder, "", viewType);

                    ////if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    ////	dtHours = BindCockpitView.BindDashBoardHourGraph(date, strShift, plantId, machineId, "CurrentDAY", param, componentId, employeeId, (viewType == "MachinewiseView" ? "MachineID" : "Plantid"), "", viewType);
                    ////else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    ////	dtHours = BindCockpitView.BindDashBoardHourGraph(date, strShift, machineId, "", "CurrentDAY", param, componentId, employeeId, (viewType == "MachinewiseView" ? "MachineID" : "Plantid"), "", viewType);
                    ////else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    ////	dtHours = BindCockpitView.BindDashBoardHourGraphComponetAndOperator(date, strShift, plantId, "", "CurrentDAY", param, machineId, employeeId, "componentid", "", "s_GetHourAggDrilldownTPMTrakData_Component_Graph");
                    ////else
                    ////	dtHours = BindCockpitView.BindDashBoardHourGraphComponetAndOperator(date, strShift, plantId, "", "CurrentDAY", param, componentId, machineId, "OperatorID", "", "s_GetHourAggDrilldownTPMTrakData_Operator_Graph");

                    //foreach (var shift in shiftData)
                    //{
                    //	chart.drilldown.Add(new DrildownSeries
                    //	{
                    //		type = type,
                    //		//name = machineId,
                    //		id = machineId + "/" + strMonth + "/" + strDay + "/" + shift,
                    //		data = new List<DrildownData>(),
                    //	});
                    //	DataRow[] results = null;
                    //	if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //		results = dtHours.Select("Month = " + strMonth + " AND MachineID ='" + machineId + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                    //	else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //		results = dtHours.Select("Month = " + strMonth + " AND PlantID ='" + machineId + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                    //	else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    //		results = dtHours.Select("Month = " + strMonth + " AND ComponentID ='" + machineId + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                    //	else
                    //		results = dtHours.Select("Month = " + strMonth + " AND OperatorID ='" + machineId + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");

                    //	foreach (DataRow drshift in results)
                    //	{
                    //		if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //		{
                    //			chart.drilldown[i].data.Add(new DrildownData
                    //			{
                    //				name = drshift["HourID"].ToString(),
                    //				y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                    //				drilldown = drshift["MachineID"].ToString() + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["ShiftName"].ToString(),
                    //				afterTitel = paramText + " : " + drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["ShiftName"].ToString() + ")",
                    //				beforeTitle = paramText + " : " + drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["ShiftName"].ToString() + ")"
                    //			});
                    //		}
                    //		else
                    //		{
                    //			chart.drilldown[i].data.Add(new DrildownData
                    //			{
                    //				name = drshift["HourID"].ToString(),
                    //				y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                    //			});
                    //		}
                    //	}
                    //	i++;
                    //} 
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
                //throw ex;
            }
            //_r.Next(100)
            return chartData;
        }
        #endregion

        #region "Get Column and Line Chart Data--------"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Chart<Series_VDG> GetColumnOEEChartData(string plantId, string strYear, string strMonth, string strDay, string param, string strShift, string componentId, string employeeId, string cellId, string SortColumn, string chartOrder, string paramText, string viewType)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string date = string.Empty;
            bool isCombined = BindCockpitView.IsChartCombined_OEEDAshboard();
            string CombinedCharts = isCombined ? BindCockpitView.GetCombinedChartNames_OEEDAshboard() : param;

            var distCharts = CombinedCharts.Split(',');
            if (!distCharts.Contains(param))//||viewType== "ComponentwiseView"||viewType== "OperatorwiseView")
            {
                distCharts = new string[0];
                distCharts = new string[] { param };
            }
            Chart<Series_VDG> chartData = null;
            chartData = new Chart<Series_VDG>
            {
                Title = (distCharts.Length > 1) ? "Efficiency (%)" : paramText,
                Subtitle = "SubTitle",
                XAxisTitle = paramText,
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };
            DataTable dtYear = new DataTable();
            DataTable dtMonth = new DataTable();
            DataTable dtDay = new DataTable();
            DataTable dtShift = new DataTable();
            DataTable dtHours = new DataTable();
            List<string> shiftData = new List<string>();
            List<string> machineData = new List<string>();
            List<string> monthData = new List<string>();

            string[] monthName = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };

            if (strShift.Equals("All", StringComparison.OrdinalIgnoreCase))
                strShift = "";
            if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
                plantId = "";
            if (componentId.Equals("All", StringComparison.OrdinalIgnoreCase))
                componentId = "";
            if (employeeId.Equals("All", StringComparison.OrdinalIgnoreCase))
                employeeId = "";
            if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase))
                cellId = "";

            #region "Month Condition------------"
            if (strMonth.Equals("Jan", StringComparison.OrdinalIgnoreCase))
                strMonth = "01";
            else if (strMonth.Equals("Feb", StringComparison.OrdinalIgnoreCase))
                strMonth = "02";
            else if (strMonth.Equals("Mar", StringComparison.OrdinalIgnoreCase))
                strMonth = "03";
            else if (strMonth.Equals("Apr", StringComparison.OrdinalIgnoreCase))
                strMonth = "04";
            else if (strMonth.Equals("May", StringComparison.OrdinalIgnoreCase))
                strMonth = "05";
            else if (strMonth.Equals("Jun", StringComparison.OrdinalIgnoreCase))
                strMonth = "06";
            else if (strMonth.Equals("Jul", StringComparison.OrdinalIgnoreCase))
                strMonth = "07";
            else if (strMonth.Equals("Aug", StringComparison.OrdinalIgnoreCase))
                strMonth = "08";
            else if (strMonth.Equals("Sep", StringComparison.OrdinalIgnoreCase))
                strMonth = "09";
            else if (strMonth.Equals("Oct", StringComparison.OrdinalIgnoreCase))
                strMonth = "10";
            else if (strMonth.Equals("Nov", StringComparison.OrdinalIgnoreCase))
                strMonth = "11";
            else if (strMonth.Equals("Dec", StringComparison.OrdinalIgnoreCase))
                strMonth = "12";
            #endregion
            try
            {
                if (strYear != "" && strMonth != "" && strDay != "")
                {
                    chartData.series = new List<Series_VDG>();
                    Series_VDG charttdataaa = null;
                    #region "Day Wise Information-------------"
                    foreach (var chartt in distCharts)
                    {
                        param = chartt;
                        date = strYear + "-" + strMonth + "-" + strDay;
                        dtMonth = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "currentDAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);
                        charttdataaa = new Series_VDG();
                        charttdataaa.name = param;
                        charttdataaa.type = "column";
                        if (distCharts.Length == 1)
                        {
                            charttdataaa.colorByPoint = true;
                        }
                        charttdataaa.currentParam = "day";
                        charttdataaa.nextParam = "shift";
                        charttdataaa.machine = "";
                        charttdataaa.day = "";
                        charttdataaa.month = strMonth;
                        charttdataaa.btnVisible = "hidden";
                        charttdataaa.previousParam = "";
                        //charttdataaa.data = new List<Data_VDG>();
                        charttdataaa.data = new List<double>();
                        charttdataaa.Category = new List<string>();
                        //Data_VDG dataa = null;
                        //charttdataaa.data = new List<Data_VDG>();
                        foreach (DataRow item in dtMonth.Rows)
                        {
                            machineData.Add(viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                            //dataa = new Data_VDG();
                            //dataa.name = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                            //dataa.y = Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0));
                            //dataa.drilldown = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                            //dataa.afterTitel = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + " (" + strYear + "-" + strMonth + ")";
                            //dataa.beforeTitle = "";
                            //charttdataaa.data.Add(dataa);
                            charttdataaa.Category.Add((viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()));
                            charttdataaa.data.Add(Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0)));
                        }
                        chartData.series.Add(charttdataaa);
                    }
                    #region-------------------------------Shift Wise Info-----------------------------------
                    //if (SortColumn == "MachineId")
                    //    SortColumn = "OEffy";
                    //chartData.drilldown = new List<DrildownSeries>();
                    //int i = 0;
                    //dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    //foreach (var machine in machineData)
                    //{
                    //    chartData.drilldown.Add(new DrildownSeries
                    //    {
                    //        name = machine,
                    //        id = machine + "-" + strMonth + "-" + strDay,
                    //        data = new List<DrildownData>(),
                    //    });
                    //    DataRow[] results = null;
                    //    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtShift.Select("Month = " + strMonth + " AND MachineID ='" + machine + "' AND Day ='" + strDay + "'");
                    //    else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtShift.Select("Month = " + strMonth + " AND PlantID ='" + machine + "' AND Day ='" + strDay + "'");
                    //    else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtShift.Select("Month = " + strMonth + " AND GroupID ='" + machine + "' AND Day ='" + strDay + "'");
                    //    else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtShift.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "' AND Day ='" + strDay + "'");
                    //    else
                    //        results = dtShift.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "' AND Day ='" + strDay + "'");

                    //    foreach (DataRow drshift in results)
                    //    {
                    //        if (!DataBaseAccess.HourlyLevelDrillDown)
                    //        {
                    //            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //            {
                    //                chartData.drilldown[i].data.Add(new DrildownData
                    //                {
                    //                    name = drshift["Shift"].ToString(),
                    //                    y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                    //                    drilldown = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString(),
                    //                    afterTitel = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["Shift"].ToString() + ")",
                    //                    beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")"
                    //                });
                    //            }
                    //            else
                    //            {
                    //                chartData.drilldown[i].data.Add(new DrildownData
                    //                {
                    //                    name = drshift["Shift"].ToString(),
                    //                    y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                    //                });
                    //            }
                    //        }
                    //        else
                    //        {
                    //            chartData.drilldown[i].data.Add(new DrildownData
                    //            {
                    //                name = drshift["Shift"].ToString(),
                    //                y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                    //            });
                    //        }
                    //        shiftData.Add(drshift["Shift"].ToString());
                    //    }
                    //    i++;
                    //}
                    #endregion

                    if (DataBaseAccess.HourlyLevelDrillDown)
                    {
                        #region-------------------------------Hours Wise Info-----------------------------------
                        //shiftData = (from w in shiftData select w).Distinct().ToList();
                        //dtHours = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "CurrentDAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                        //foreach (var machine in machineData)
                        //{
                        //    foreach (var shift in shiftData)
                        //    {
                        //        chartData.drilldown.Add(new DrildownSeries
                        //        {
                        //            name = machine,
                        //            id = machine + "/" + strMonth + "/" + strDay + "/" + shift,
                        //            data = new List<DrildownData>(),
                        //        });
                        //        DataRow[] results = null;
                        //        if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                        //            results = dtHours.Select("Month = " + strMonth + " AND MachineID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                        //        else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                        //            results = dtHours.Select("Month = " + strMonth + " AND PlantID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                        //        else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                        //            results = dtShift.Select("Month = " + strMonth + " AND GroupID ='" + machine + "' AND Day ='" + strDay + "'");
                        //        else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                        //            results = dtHours.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                        //        else
                        //            results = dtHours.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");

                        //        foreach (DataRow drshift in results)
                        //        {
                        //            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                        //            {
                        //                chartData.drilldown[i].data.Add(new DrildownData
                        //                {
                        //                    name = drshift["HourID"].ToString(),
                        //                    y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                        //                    drilldown = drshift["MachineID"].ToString() + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["ShiftName"].ToString(),
                        //                    afterTitel = drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["ShiftName"].ToString() + ")",
                        //                    beforeTitle = paramText + " : " + drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")"
                        //                });
                        //            }
                        //            else
                        //            {
                        //                chartData.drilldown[i].data.Add(new DrildownData
                        //                {
                        //                    name = drshift["HourID"].ToString(),
                        //                    y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                        //                });
                        //            }
                        //        }
                        //        i++;
                        //    }
                        //}
                        #endregion
                    }
                    #endregion
                }
                else if (strYear != "" && strMonth != "")
                {
                    #region "month Wise Information ------------"
                    chartData.series = new List<Series_VDG>();
                    Series_VDG charttdataaa = null;
                    foreach (var chartt in distCharts)
                    {
                        param = chartt;
                        date = strYear + "-" + strMonth + "-" + "01";
                        dtMonth = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "currentMONTH", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                        charttdataaa = new Series_VDG();
                        charttdataaa.name = param;
                        charttdataaa.type = "column";
                        if (distCharts.Length == 1)
                        {
                            charttdataaa.colorByPoint = true;
                        }
                        charttdataaa.currentParam = "month";
                        charttdataaa.nextParam = "day";
                        charttdataaa.machine = "";
                        charttdataaa.day = "";
                        charttdataaa.month = strMonth;
                        charttdataaa.btnVisible = "hidden";
                        charttdataaa.previousParam = "";
                        //charttdataaa.data = new List<Data_VDG>();
                        charttdataaa.data = new List<double>();
                        charttdataaa.Category = new List<string>();
                        //Data_VDG dataa = null;
                        foreach (DataRow item in dtMonth.Rows)
                        {
                            machineData.Add(viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                            //dataa = new Data_VDG();
                            //dataa.name = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                            //if (!Convert.IsDBNull(item["Parameter"]))
                            //{
                            //    dataa.y = Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0));
                            //}
                            //dataa.drilldown = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                            //dataa.afterTitel = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + " (" + strYear + "-" + strMonth + ")";
                            //dataa.beforeTitle = "";
                            //charttdataaa.data.Add(dataa);
                            charttdataaa.Category.Add((viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()));
                            charttdataaa.data.Add(Convert.IsDBNull(item["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0)));
                        }
                        chartData.series.Add(charttdataaa);
                    }
                    #region---------------------------Day Wise Start---------------------------------
                    //if (SortColumn == "MachineId")
                    //    SortColumn = "OEffy";
                    //chart.drilldown = new List<DrildownSeries>();
                    //int i = 0;
                    //dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "DAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    //foreach (var machine in machineData)
                    //{
                    //    chart.drilldown.Add(new DrildownSeries
                    //    {
                    //        name = machine,
                    //        id = machine,
                    //        data = new List<DrildownData>(),
                    //    });
                    //    DataRow[] results = null;
                    //    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtDay.Select("Month = " + strMonth + " AND MachineID ='" + machine + "'");
                    //    else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtDay.Select("Month = " + strMonth + " AND PlantID ='" + machine + "'");
                    //    else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtDay.Select("Month = " + strMonth + " AND GroupID ='" + machine + "'");
                    //    else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtDay.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "'");
                    //    else
                    //        results = dtDay.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "'");

                    //    foreach (DataRow dr in results)
                    //    {
                    //        chart.drilldown[i].data.Add(new DrildownData
                    //        {
                    //            name = "Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                    //            y = Convert.ToDecimal(dr["Parameter"].ToString()),
                    //            drilldown = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + "/" + strMonth + "/" + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                    //            afterTitel = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + strMonth + "-" + Convert.ToDateTime(dr["PDate"]).ToString("dd") + ")",
                    //            beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + strMonth + ")"
                    //        });
                    //        monthData.Add(Convert.ToDateTime(dr["PDate"]).ToString("dd"));
                    //    }
                    //    i++;
                    //}
                    #endregion
                    #region--------------------------Shift Wise Information-----------------------------
                    //monthData = (from w in monthData select w).Distinct().ToList();
                    //dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    //foreach (var machine in machineData)
                    //{
                    //    foreach (var day in monthData)
                    //    {
                    //        chart.drilldown.Add(new DrildownSeries
                    //        {
                    //            name = machine,
                    //            id = machine + "/" + strMonth + "/" + day,
                    //            data = new List<DrildownData>(),
                    //        });
                    //        DataRow[] results = null;
                    //        if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //            results = dtShift.Select("Month = " + strMonth + " AND MachineID ='" + machine + "' AND Day ='" + day + "'");
                    //        else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //            results = dtShift.Select("Month = " + strMonth + " AND PlantID ='" + machine + "' AND Day ='" + day + "'");
                    //        else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                    //            results = dtShift.Select("Month = " + strMonth + " AND GroupID ='" + machine + "' AND Day ='" + day + "'");
                    //        else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    //            results = dtShift.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "' AND Day ='" + day + "'");
                    //        else
                    //            results = dtShift.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "' AND Day ='" + day + "'");

                    //        foreach (DataRow drshift in results)
                    //        {

                    //            if (!DataBaseAccess.HourlyLevelDrillDown)
                    //            {
                    //                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //                {
                    //                    chart.drilldown[i].data.Add(new DrildownData
                    //                    {
                    //                        name = drshift["Shift"].ToString(),
                    //                        y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                    //                        drilldown = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString(),
                    //                        afterTitel = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["Shift"].ToString() + ")",
                    //                        beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")",
                    //                    });
                    //                }
                    //                else
                    //                {
                    //                    chart.drilldown[i].data.Add(new DrildownData
                    //                    {
                    //                        name = drshift["Shift"].ToString(),
                    //                        y = Convert.ToDecimal(drshift["Parameter"].ToString())
                    //                    });
                    //                }
                    //            }
                    //            else
                    //            {
                    //                chart.drilldown[i].data.Add(new DrildownData
                    //                {
                    //                    name = drshift["Shift"].ToString(),
                    //                    y = Convert.ToDecimal(drshift["Parameter"].ToString())
                    //                });
                    //            }
                    //            shiftData.Add(drshift["Shift"].ToString());
                    //        }
                    //        i++;
                    //    }
                    //}
                    #endregion

                    if (DataBaseAccess.HourlyLevelDrillDown)
                    {
                        #region-------------------------------Hours Wise Info-----------------------------------
                        //shiftData = (from w in shiftData select w).Distinct().ToList();
                        //dtHours = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "Currentmonth", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                        //foreach (var machine in machineData)
                        //{
                        //    foreach (var day in monthData)
                        //    {
                        //        foreach (var shift in shiftData)
                        //        {
                        //            chart.drilldown.Add(new DrildownSeries
                        //            {
                        //                name = machine,
                        //                id = machine + "/" + strMonth + "/" + day + "/" + shift,
                        //                data = new List<DrildownData>(),
                        //            });
                        //            DataRow[] results = null;
                        //            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                        //                results = dtHours.Select("Month = " + strMonth + " AND MachineID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                        //            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                        //                results = dtHours.Select("Month = " + strMonth + " AND PlantID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                        //            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                        //                results = dtHours.Select("Month = " + strMonth + " AND GroupID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                        //            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                        //                results = dtHours.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                        //            else
                        //                results = dtHours.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                        //            foreach (DataRow drshift in results)
                        //            {
                        //                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                        //                {
                        //                    chart.drilldown[i].data.Add(new DrildownData
                        //                    {
                        //                        name = drshift["HourID"].ToString(),
                        //                        y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                        //                        drilldown = drshift["MachineID"].ToString() + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["ShiftName"].ToString(),
                        //                        afterTitel = drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["ShiftName"].ToString() + ")",
                        //                        beforeTitle = paramText + " : " + drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")"
                        //                    });
                        //                }
                        //                else
                        //                {
                        //                    chart.drilldown[i].data.Add(new DrildownData
                        //                    {
                        //                        name = drshift["HourID"].ToString(),
                        //                        y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                        //                    });
                        //                }
                        //            }
                        //            i++;
                        //        }

                        //    }
                        //}
                        #endregion
                    }
                    #endregion
                }
                else if (strYear != "")
                {
                    #region "Year Wise Information -----------"
                    chartData.series = new List<Series_VDG>();
                    Series_VDG charttdataaa = null;
                    foreach (var chartt in distCharts)
                    {
                        param = chartt;
                        date = strYear + "-" + "01" + "-" + "01";
                        dtYear = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "YEAR", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);
                        charttdataaa = new Series_VDG();
                        charttdataaa.name = HttpContext.GetLocalResourceObject("~/Dashboard.aspx", param).ToString();
                        charttdataaa.type = "column";
                        if (distCharts.Length == 1)
                        {
                            charttdataaa.colorByPoint = true;
                        }
                        charttdataaa.currentParam = "year";
                        charttdataaa.nextParam = "month";
                        charttdataaa.machine = "";
                        charttdataaa.day = "";
                        charttdataaa.month = "";
                        charttdataaa.btnVisible = "hidden";
                        charttdataaa.previousParam = "";
                        //charttdataaa.colorByPoint = true;
                        charttdataaa.data = new List<double>();
                        charttdataaa.Category = new List<string>();
                        //charttdataaa.data = new List<Data_VDG>();
                        //charttdataaa.data = new List<Data>();
                        //Data_VDG dataa = null;
                        foreach (DataRow item in dtYear.Rows)
                        {
                            machineData.Add(viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                            //dataa = new Data_VDG();
                            //dataa.name = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                            //if (!Convert.IsDBNull(item["Parameter"]))
                            //{
                            //    dataa.y = Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0));
                            //}
                            //dataa.turboThreshold = 1500;
                            //dataa.drilldown = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                            //dataa.afterTitel = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + " (" + strYear + "-" + strMonth + ")";
                            //dataa.beforeTitle = "";
                            //charttdataaa.data.Add(dataa);
                            charttdataaa.Category.Add((viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()));
                            charttdataaa.data.Add(Convert.IsDBNull(item["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0)));
                            //charttdataaa.drilldown = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                        }
                        chartData.series.Add(charttdataaa);
                    }

                    #region---------------------------Month Wise Start----------------------------

                    //chartData.drilldown = new List<DrildownSeries>();
                    //int i = 0;
                    //foreach (var machine in machineData)
                    //{
                    //    chartData.drilldown.Add(new DrildownSeries
                    //    {
                    //        name = machine,
                    //        id = machine,
                    //        data = new List<DrildownData>(),
                    //    });
                    //    DataRow[] results = null;
                    //    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtMonth.Select("MachineID = '" + machine + "'");
                    //    else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtMonth.Select("PlantID = '" + machine + "'");
                    //    else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtMonth.Select("GroupID = '" + machine + "'");
                    //    else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtMonth.Select("ComponentID = '" + machine + "'");
                    //    else
                    //        results = dtMonth.Select("OperatorID = '" + machine + "'");

                    //    if (results != null)
                    //    {
                    //        foreach (DataRow drmonth in results)
                    //        {
                    //            chartData.drilldown[i].data.Add(new DrildownData
                    //            {
                    //                name = drmonth["NameOftheMonth"].ToString(),
                    //                y = Convert.ToDecimal(drmonth["Parameter"].ToString()),
                    //                drilldown = (viewType == "MachinewiseView" ? drmonth["MachineID"].ToString() : viewType == "CellWiseView" ? drmonth["GroupID"].ToString() : viewType == "PlantwiseView" ? drmonth["PlantID"].ToString() : viewType == "ComponentwiseView" ? drmonth["ComponentID"].ToString() : drmonth["OperatorID"].ToString()) + "-" + Convert.ToDateTime(drmonth["Startdate"]).ToString("MM"),
                    //                afterTitel = (viewType == "MachinewiseView" ? drmonth["MachineID"].ToString() : viewType == "CellWiseView" ? drmonth["GroupID"].ToString() : viewType == "PlantwiseView" ? drmonth["PlantID"].ToString() : viewType == "ComponentwiseView" ? drmonth["ComponentID"].ToString() : drmonth["OperatorID"].ToString()) + " (" + strYear + "-" + Convert.ToDateTime(drmonth["Startdate"]).ToString("MM") + ")",
                    //                beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? drmonth["MachineID"].ToString() : viewType == "CellWiseView" ? drmonth["GroupID"].ToString() : viewType == "PlantwiseView" ? drmonth["PlantID"].ToString() : viewType == "ComponentwiseView" ? drmonth["ComponentID"].ToString() : drmonth["OperatorID"].ToString()) + " (" + strYear + ")"
                    //            });
                    //        }
                    //        i++;
                    //    }
                    //}
                    #endregion
                    #region---------------------------Day Wise Start---------------------------------
                    //dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "DAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    //foreach (var machine in machineData)
                    //{
                    //    foreach (var month in monthName)
                    //    {

                    //        chartData.drilldown.Add(new DrildownSeries
                    //        {
                    //            name = machine + "(" + month + ")",
                    //            id = machine + "-" + month,
                    //            data = new List<DrildownData>(),
                    //        });
                    //        DataRow[] results = null;
                    //        if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //            results = dtDay.Select("Month = " + month + " AND MachineID ='" + machine + "'");
                    //        else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //            results = dtDay.Select("Month = " + month + " AND PlantID ='" + machine + "'");
                    //        else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                    //            results = dtDay.Select("Month = " + month + " AND GroupID ='" + machine + "'");
                    //        else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    //            results = dtDay.Select("Month = " + month + " AND ComponentID ='" + machine + "'");
                    //        else
                    //            results = dtDay.Select("Month = " + month + " AND OperatorID ='" + machine + "'");

                    //        if (results != null)
                    //        {
                    //            foreach (DataRow dr in results)
                    //            {
                    //                chartData.drilldown[i].data.Add(new DrildownData
                    //                {
                    //                    name = "Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                    //                    y = Convert.ToDecimal(dr["Parameter"].ToString()),
                    //                    drilldown = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + "/" + month + "/" + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                    //                    afterTitel = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + month + ")",//"-" + Convert.ToDateTime(dr["PDate"]).ToString("dd") +
                    //                    beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + month + ")"
                    //                });
                    //            }
                    //            i++;
                    //        }
                    //    }
                    //}
                    #endregion
                    #region--------------------------------------------------Shift Wise Start---------------------------------------------
                    //if (DataBaseAccess.ThirdLevelDrillDown)
                    //{
                    //    //date = string.Empty;
                    //    dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    //    foreach (var machine in machineData)
                    //    {
                    //        foreach (var month in monthName)
                    //        {
                    //            date = strYear + "-" + month + "-" + "01";
                    //            // dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "DAY", param, componentId, employeeId, SortColumn, "");
                    //            DataRow[] results = null;
                    //            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //                results = dtDay.Select("Month = '" + month + "' AND MachineID ='" + machine + "'");//"MachineID = '" + machine + "'" ///"PDate = '" + date + "' AND MachineID ='" + machine + "'"
                    //            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //                results = dtDay.Select("Month = '" + month + "' AND PlantID ='" + machine + "'");
                    //            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                    //                results = dtDay.Select("Month = '" + month + "' AND GroupID ='" + machine + "'");
                    //            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    //                results = dtDay.Select("Month = '" + month + "' AND ComponentID ='" + machine + "'");
                    //            else
                    //                results = dtDay.Select("Month = '" + month + "' AND OperatorID ='" + machine + "'");

                    //            foreach (DataRow day in results)
                    //            {
                    //                chartData.drilldown.Add(new DrildownSeries
                    //                {
                    //                    id = machine + "/" + month + "/" + Convert.ToDateTime(day["PDate"]).ToString("dd"),
                    //                    name = machine,//+ "/" + strYear + "/" + month + "/" + Convert.ToDateTime(day["PDate"]).ToString("dd"),
                    //                                   //drilldown = machine + "/" + strYear + "/" + month + "/" + Convert.ToDateTime(day["PDate"]).ToString("dd"),
                    //                                   //afterTitel = machine + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd") + ")",
                    //                                   //beforeTitle = paramText + " : " + machine + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd") + " )",
                    //                    data = new List<DrildownData>(),
                    //                });
                    //                date = strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd");
                    //                // dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "SHIFT", param, componentId, employeeId, SortColumn, "");
                    //                DataRow[] results1 = null;
                    //                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //                    results1 = dtShift.Select("PDate = '" + date + "' AND MachineID ='" + machine + "'");
                    //                else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //                    results1 = dtShift.Select("PDate = '" + date + "' AND PlantID ='" + machine + "'");
                    //                else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                    //                    results1 = dtShift.Select("PDate = '" + date + "' AND GroupID ='" + machine + "'");
                    //                else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    //                    results1 = dtShift.Select("PDate = '" + date + "' AND ComponentID ='" + machine + "'");
                    //                else
                    //                    results1 = dtShift.Select("PDate = '" + date + "' AND OperatorID ='" + machine + "'");

                    //                foreach (DataRow drshift in results1)
                    //                {
                    //                    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //                    {
                    //                        chartData.drilldown[i].data.Add(new DrildownData
                    //                        {
                    //                            name = drshift["Shift"].ToString(),
                    //                            y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                    //                            drilldown = machine + "/" + month + "/" + Convert.ToDateTime(drshift["PDate"]).ToString("dd") + "/" + drshift["Shift"].ToString(),
                    //                            afterTitel = machine + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd") + "-" + drshift["Shift"].ToString() + ")", //+ "/" + drshift["Shift"].ToString(),
                    //                                                                                                                                                                                   //beforeTitle = paramText + " : " + machine + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd") + " )",
                    //                        });
                    //                    }
                    //                    else
                    //                    {
                    //                        chartData.drilldown[i].data.Add(new DrildownData
                    //                        {
                    //                            name = drshift["Shift"].ToString(),
                    //                            y = Convert.ToDecimal(drshift["Parameter"].ToString())
                    //                        });
                    //                    }
                    //                }
                    //                i++;
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
                //throw ex;
            }
            stopwatch.Stop();
            Logger.WriteDebugLog("BindDashboardChartData : " + stopwatch.Elapsed.TotalSeconds);
            HttpContext.Current.Session["ChartDataForBackFun"] = null;
            List<Chart<Series_VDG>> chartList = new List<Chart<Series_VDG>>();
            chartList.Add(chartData);
            HttpContext.Current.Session["ChartDataForBackFun"] = chartList;
            return chartData;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Chart<Series> GetColumnOEEChartData_old(string plantId, string strYear, string strMonth, string strDay, string param, string strShift, string componentId, string employeeId, string cellId, string SortColumn, string chartOrder, string paramText, string viewType)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string date = string.Empty;

            var chart = new Chart<Series>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = paramText,
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };
            DataTable dtYear = new DataTable();
            DataTable dtMonth = new DataTable();
            DataTable dtDay = new DataTable();
            DataTable dtShift = new DataTable();
            DataTable dtHours = new DataTable();
            List<string> shiftData = new List<string>();
            List<string> machineData = new List<string>();
            List<string> monthData = new List<string>();

            string[] monthName = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };

            if (strShift.Equals("All", StringComparison.OrdinalIgnoreCase))
                strShift = "";
            if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
                plantId = "";
            if (componentId.Equals("All", StringComparison.OrdinalIgnoreCase))
                componentId = "";
            if (employeeId.Equals("All", StringComparison.OrdinalIgnoreCase))
                employeeId = "";
            if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase))
                cellId = "";

            #region "Month Condition------------"
            if (strMonth.Equals("Jan", StringComparison.OrdinalIgnoreCase))
                strMonth = "01";
            else if (strMonth.Equals("Feb", StringComparison.OrdinalIgnoreCase))
                strMonth = "02";
            else if (strMonth.Equals("Mar", StringComparison.OrdinalIgnoreCase))
                strMonth = "03";
            else if (strMonth.Equals("Apr", StringComparison.OrdinalIgnoreCase))
                strMonth = "04";
            else if (strMonth.Equals("May", StringComparison.OrdinalIgnoreCase))
                strMonth = "05";
            else if (strMonth.Equals("Jun", StringComparison.OrdinalIgnoreCase))
                strMonth = "06";
            else if (strMonth.Equals("Jul", StringComparison.OrdinalIgnoreCase))
                strMonth = "07";
            else if (strMonth.Equals("Aug", StringComparison.OrdinalIgnoreCase))
                strMonth = "08";
            else if (strMonth.Equals("Sep", StringComparison.OrdinalIgnoreCase))
                strMonth = "09";
            else if (strMonth.Equals("Oct", StringComparison.OrdinalIgnoreCase))
                strMonth = "10";
            else if (strMonth.Equals("Nov", StringComparison.OrdinalIgnoreCase))
                strMonth = "11";
            else if (strMonth.Equals("Dec", StringComparison.OrdinalIgnoreCase))
                strMonth = "12";
            #endregion

            try
            {
                if (strYear != "" && strMonth != "" && strDay != "")
                {
                    #region "Day Wise Information-------------"
                    date = strYear + "-" + strMonth + "-" + strDay;

                    dtMonth = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "currentDAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);


                    chart.series = new List<Series>();
                    chart.series.Add(new Series
                    {
                        name = HttpContext.GetLocalResourceObject("~/Dashboard.aspx", param).ToString(),
                        type = "column",
                        colorByPoint = true,
                        data = new List<Data>()
                    });
                    foreach (DataRow item in dtMonth.Rows)
                    {
                        machineData.Add((viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()));
                        chart.series[0].data.Add(new Data
                        {
                            name = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()),
                            y = Convert.ToInt32(Convert.ToDecimal(item["Parameter"].ToString())),
                            drilldown = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + "-" + Convert.ToDateTime(item["PDate"]).ToString("MM") + "-" + strDay,
                            afterTitel = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + " (" + strYear + "-" + strMonth + "-" + strDay + ")",
                            beforeTitle = ""
                        });
                    }

                    //-------------------------------Shift Wise Info-----------------------------------
                    if (SortColumn == "MachineId")
                        SortColumn = "OEffy";
                    chart.drilldown = new List<DrildownSeries>();
                    int i = 0;
                    dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    foreach (var machine in machineData)
                    {
                        chart.drilldown.Add(new DrildownSeries
                        {
                            name = machine,
                            id = machine + "-" + strMonth + "-" + strDay,
                            data = new List<DrildownData>(),
                        });
                        DataRow[] results = null;
                        if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtShift.Select("Month = " + strMonth + " AND MachineID ='" + machine + "' AND Day ='" + strDay + "'");
                        else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtShift.Select("Month = " + strMonth + " AND PlantID ='" + machine + "' AND Day ='" + strDay + "'");
                        else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtShift.Select("Month = " + strMonth + " AND GroupID ='" + machine + "' AND Day ='" + strDay + "'");
                        else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtShift.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "' AND Day ='" + strDay + "'");
                        else
                            results = dtShift.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "' AND Day ='" + strDay + "'");

                        foreach (DataRow drshift in results)
                        {
                            if (!DataBaseAccess.HourlyLevelDrillDown)
                            {
                                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                {
                                    chart.drilldown[i].data.Add(new DrildownData
                                    {
                                        name = drshift["Shift"].ToString(),
                                        y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                                        drilldown = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString(),
                                        afterTitel = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["Shift"].ToString() + ")",
                                        beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")"
                                    });
                                }
                                else
                                {
                                    chart.drilldown[i].data.Add(new DrildownData
                                    {
                                        name = drshift["Shift"].ToString(),
                                        y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                                    });
                                }
                            }
                            else
                            {
                                chart.drilldown[i].data.Add(new DrildownData
                                {
                                    name = drshift["Shift"].ToString(),
                                    y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                                });
                            }
                            shiftData.Add(drshift["Shift"].ToString());
                        }
                        i++;
                    }


                    if (DataBaseAccess.HourlyLevelDrillDown)
                    {
                        //-------------------------------Hours Wise Info-----------------------------------
                        shiftData = (from w in shiftData select w).Distinct().ToList();
                        dtHours = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "CurrentDAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                        foreach (var machine in machineData)
                        {
                            foreach (var shift in shiftData)
                            {
                                chart.drilldown.Add(new DrildownSeries
                                {
                                    name = machine,
                                    id = machine + "/" + strMonth + "/" + strDay + "/" + shift,
                                    data = new List<DrildownData>(),
                                });
                                DataRow[] results = null;
                                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                    results = dtHours.Select("Month = " + strMonth + " AND MachineID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                                else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                    results = dtHours.Select("Month = " + strMonth + " AND PlantID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                                else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                    results = dtShift.Select("Month = " + strMonth + " AND GroupID ='" + machine + "' AND Day ='" + strDay + "'");
                                else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                    results = dtHours.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                                else
                                    results = dtHours.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");

                                foreach (DataRow drshift in results)
                                {
                                    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                    {
                                        chart.drilldown[i].data.Add(new DrildownData
                                        {
                                            name = drshift["HourID"].ToString(),
                                            y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                                            drilldown = drshift["MachineID"].ToString() + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["ShiftName"].ToString(),
                                            afterTitel = drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["ShiftName"].ToString() + ")",
                                            beforeTitle = paramText + " : " + drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")"
                                        });
                                    }
                                    else
                                    {
                                        chart.drilldown[i].data.Add(new DrildownData
                                        {
                                            name = drshift["HourID"].ToString(),
                                            y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                                        });
                                    }
                                }
                                i++;
                            }
                        }
                    }
                    #endregion
                }
                else if (strYear != "" && strMonth != "")
                {
                    #region "month Wise Information ------------"
                    date = strYear + "-" + strMonth + "-" + "01";
                    dtMonth = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "currentMONTH", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    chart.series = new List<Series>();
                    chart.series.Add(new Series
                    {
                        name = param,
                        type = "column",
                        colorByPoint = true,
                        data = new List<Data>()
                    });
                    foreach (DataRow item in dtMonth.Rows)
                    {
                        machineData.Add(viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                        chart.series[0].data.Add(new Data
                        {
                            name = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()),
                            y = Convert.ToInt32(Convert.ToDecimal(item["Parameter"].ToString())),
                            drilldown = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()),
                            afterTitel = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + " (" + strYear + "-" + strMonth + ")",
                            beforeTitle = ""
                        });
                    }

                    //---------------------------Day Wise Start---------------------------------
                    if (SortColumn == "MachineId")
                        SortColumn = "OEffy";
                    chart.drilldown = new List<DrildownSeries>();
                    int i = 0;
                    dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "DAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    foreach (var machine in machineData)
                    {
                        chart.drilldown.Add(new DrildownSeries
                        {
                            name = machine,
                            id = machine,
                            data = new List<DrildownData>(),
                        });
                        DataRow[] results = null;
                        if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtDay.Select("Month = " + strMonth + " AND MachineID ='" + machine + "'");
                        else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtDay.Select("Month = " + strMonth + " AND PlantID ='" + machine + "'");
                        else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtDay.Select("Month = " + strMonth + " AND GroupID ='" + machine + "'");
                        else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtDay.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "'");
                        else
                            results = dtDay.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "'");

                        foreach (DataRow dr in results)
                        {
                            chart.drilldown[i].data.Add(new DrildownData
                            {
                                name = "Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                                y = Convert.ToDecimal(dr["Parameter"].ToString()),
                                drilldown = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + "/" + strMonth + "/" + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                                afterTitel = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + strMonth + "-" + Convert.ToDateTime(dr["PDate"]).ToString("dd") + ")",
                                beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + strMonth + ")"
                            });
                            monthData.Add(Convert.ToDateTime(dr["PDate"]).ToString("dd"));
                        }
                        i++;
                    }
                    //--------------------------Shift Wise Information-----------------------------
                    monthData = (from w in monthData select w).Distinct().ToList();
                    dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    foreach (var machine in machineData)
                    {
                        foreach (var day in monthData)
                        {
                            chart.drilldown.Add(new DrildownSeries
                            {
                                name = machine,
                                id = machine + "/" + strMonth + "/" + day,
                                data = new List<DrildownData>(),
                            });
                            DataRow[] results = null;
                            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND MachineID ='" + machine + "' AND Day ='" + day + "'");
                            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND PlantID ='" + machine + "' AND Day ='" + day + "'");
                            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND GroupID ='" + machine + "' AND Day ='" + day + "'");
                            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "' AND Day ='" + day + "'");
                            else
                                results = dtShift.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "' AND Day ='" + day + "'");

                            foreach (DataRow drshift in results)
                            {

                                if (!DataBaseAccess.HourlyLevelDrillDown)
                                {
                                    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                    {
                                        chart.drilldown[i].data.Add(new DrildownData
                                        {
                                            name = drshift["Shift"].ToString(),
                                            y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                                            drilldown = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString(),
                                            afterTitel = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["Shift"].ToString() + ")",
                                            beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")",
                                        });
                                    }
                                    else
                                    {
                                        chart.drilldown[i].data.Add(new DrildownData
                                        {
                                            name = drshift["Shift"].ToString(),
                                            y = Convert.ToDecimal(drshift["Parameter"].ToString())
                                        });
                                    }
                                }
                                else
                                {
                                    chart.drilldown[i].data.Add(new DrildownData
                                    {
                                        name = drshift["Shift"].ToString(),
                                        y = Convert.ToDecimal(drshift["Parameter"].ToString())
                                    });
                                }
                                shiftData.Add(drshift["Shift"].ToString());
                            }
                            i++;
                        }
                    }

                    if (DataBaseAccess.HourlyLevelDrillDown)
                    {
                        //-------------------------------Hours Wise Info-----------------------------------
                        shiftData = (from w in shiftData select w).Distinct().ToList();
                        dtHours = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "Currentmonth", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                        foreach (var machine in machineData)
                        {
                            foreach (var day in monthData)
                            {
                                foreach (var shift in shiftData)
                                {
                                    chart.drilldown.Add(new DrildownSeries
                                    {
                                        name = machine,
                                        id = machine + "/" + strMonth + "/" + day + "/" + shift,
                                        data = new List<DrildownData>(),
                                    });
                                    DataRow[] results = null;
                                    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                        results = dtHours.Select("Month = " + strMonth + " AND MachineID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                                    else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                        results = dtHours.Select("Month = " + strMonth + " AND PlantID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                                    else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                        results = dtHours.Select("Month = " + strMonth + " AND GroupID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                                    else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                        results = dtHours.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                                    else
                                        results = dtHours.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                                    foreach (DataRow drshift in results)
                                    {
                                        if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                        {
                                            chart.drilldown[i].data.Add(new DrildownData
                                            {
                                                name = drshift["HourID"].ToString(),
                                                y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                                                drilldown = drshift["MachineID"].ToString() + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["ShiftName"].ToString(),
                                                afterTitel = drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["ShiftName"].ToString() + ")",
                                                beforeTitle = paramText + " : " + drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")"
                                            });
                                        }
                                        else
                                        {
                                            chart.drilldown[i].data.Add(new DrildownData
                                            {
                                                name = drshift["HourID"].ToString(),
                                                y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                                            });
                                        }
                                    }
                                    i++;
                                }

                            }
                        }
                    }
                    #endregion
                }
                else if (strYear != "")
                {
                    #region "Year Wise Information -----------"
                    date = strYear + "-" + "01" + "-" + "01";
                    dtYear = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "YEAR", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    //------------------------Year Wise Start--------------------------
                    if (SortColumn == "MachineId")
                        SortColumn = "OEffy";

                    dtMonth = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "MONTH", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    chart.series = new List<Series>();
                    chart.series.Add(new Series
                    {
                        name = HttpContext.GetLocalResourceObject("~/Dashboard.aspx", param).ToString(),
                        type = "column",
                        colorByPoint = true,
                        data = new List<Data>()
                    });
                    foreach (DataRow item in dtYear.Rows)
                    {
                        machineData.Add(viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                        chart.series[0].data.Add(new Data
                        {
                            name = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()),
                            y = Convert.ToInt32(Convert.ToDecimal(item["Parameter"].ToString())),
                            drilldown = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()),
                            afterTitel = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + " (" + strYear + ")",
                            beforeTitle = ""
                        });
                    }
                    //---------------------------Month Wise Start----------------------------

                    chart.drilldown = new List<DrildownSeries>();
                    int i = 0;
                    foreach (var machine in machineData)
                    {
                        chart.drilldown.Add(new DrildownSeries
                        {
                            name = machine,
                            id = machine,
                            data = new List<DrildownData>(),
                        });
                        DataRow[] results = null;
                        if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtMonth.Select("MachineID = '" + machine + "'");
                        else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtMonth.Select("PlantID = '" + machine + "'");
                        else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtMonth.Select("GroupID = '" + machine + "'");
                        else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtMonth.Select("ComponentID = '" + machine + "'");
                        else
                            results = dtMonth.Select("OperatorID = '" + machine + "'");

                        if (results != null)
                        {
                            foreach (DataRow drmonth in results)
                            {
                                chart.drilldown[i].data.Add(new DrildownData
                                {
                                    name = drmonth["NameOftheMonth"].ToString(),
                                    y = Convert.ToDecimal(drmonth["Parameter"].ToString()),
                                    drilldown = (viewType == "MachinewiseView" ? drmonth["MachineID"].ToString() : viewType == "CellWiseView" ? drmonth["GroupID"].ToString() : viewType == "PlantwiseView" ? drmonth["PlantID"].ToString() : viewType == "ComponentwiseView" ? drmonth["ComponentID"].ToString() : drmonth["OperatorID"].ToString()) + "-" + Convert.ToDateTime(drmonth["Startdate"]).ToString("MM"),
                                    afterTitel = (viewType == "MachinewiseView" ? drmonth["MachineID"].ToString() : viewType == "CellWiseView" ? drmonth["GroupID"].ToString() : viewType == "PlantwiseView" ? drmonth["PlantID"].ToString() : viewType == "ComponentwiseView" ? drmonth["ComponentID"].ToString() : drmonth["OperatorID"].ToString()) + " (" + strYear + "-" + Convert.ToDateTime(drmonth["Startdate"]).ToString("MM") + ")",
                                    beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? drmonth["MachineID"].ToString() : viewType == "CellWiseView" ? drmonth["GroupID"].ToString() : viewType == "PlantwiseView" ? drmonth["PlantID"].ToString() : viewType == "ComponentwiseView" ? drmonth["ComponentID"].ToString() : drmonth["OperatorID"].ToString()) + " (" + strYear + ")"
                                });
                            }
                            i++;
                        }
                    }
                    //---------------------------Day Wise Start---------------------------------
                    dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "DAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    foreach (var machine in machineData)
                    {
                        foreach (var month in monthName)
                        {

                            chart.drilldown.Add(new DrildownSeries
                            {
                                name = machine + "(" + month + ")",
                                id = machine + "-" + month,
                                data = new List<DrildownData>(),
                            });
                            DataRow[] results = null;
                            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + month + " AND MachineID ='" + machine + "'");
                            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + month + " AND PlantID ='" + machine + "'");
                            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + month + " AND GroupID ='" + machine + "'");
                            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + month + " AND ComponentID ='" + machine + "'");
                            else
                                results = dtDay.Select("Month = " + month + " AND OperatorID ='" + machine + "'");

                            if (results != null)
                            {
                                foreach (DataRow dr in results)
                                {
                                    chart.drilldown[i].data.Add(new DrildownData
                                    {
                                        name = "Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                                        y = Convert.ToDecimal(dr["Parameter"].ToString()),
                                        drilldown = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + "/" + month + "/" + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                                        afterTitel = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + month + ")",//"-" + Convert.ToDateTime(dr["PDate"]).ToString("dd") +
                                        beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + month + ")"
                                    });
                                }
                                i++;
                            }
                        }
                    }

                    //--------------------------------------------------Shift Wise Start---------------------------------------------
                    if (DataBaseAccess.ThirdLevelDrillDown)
                    {
                        //date = string.Empty;
                        dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                        foreach (var machine in machineData)
                        {
                            foreach (var month in monthName)
                            {
                                date = strYear + "-" + month + "-" + "01";
                                // dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "DAY", param, componentId, employeeId, SortColumn, "");
                                DataRow[] results = null;
                                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                    results = dtDay.Select("Month = '" + month + "' AND MachineID ='" + machine + "'");//"MachineID = '" + machine + "'" ///"PDate = '" + date + "' AND MachineID ='" + machine + "'"
                                else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                    results = dtDay.Select("Month = '" + month + "' AND PlantID ='" + machine + "'");
                                else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                    results = dtDay.Select("Month = '" + month + "' AND GroupID ='" + machine + "'");
                                else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                    results = dtDay.Select("Month = '" + month + "' AND ComponentID ='" + machine + "'");
                                else
                                    results = dtDay.Select("Month = '" + month + "' AND OperatorID ='" + machine + "'");

                                foreach (DataRow day in results)
                                {
                                    chart.drilldown.Add(new DrildownSeries
                                    {
                                        id = machine + "/" + month + "/" + Convert.ToDateTime(day["PDate"]).ToString("dd"),
                                        name = machine,//+ "/" + strYear + "/" + month + "/" + Convert.ToDateTime(day["PDate"]).ToString("dd"),
                                                       //drilldown = machine + "/" + strYear + "/" + month + "/" + Convert.ToDateTime(day["PDate"]).ToString("dd"),
                                                       //afterTitel = machine + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd") + ")",
                                                       //beforeTitle = paramText + " : " + machine + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd") + " )",
                                        data = new List<DrildownData>(),
                                    });
                                    date = strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd");
                                    // dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "SHIFT", param, componentId, employeeId, SortColumn, "");
                                    DataRow[] results1 = null;
                                    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                        results1 = dtShift.Select("PDate = '" + date + "' AND MachineID ='" + machine + "'");
                                    else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                        results1 = dtShift.Select("PDate = '" + date + "' AND PlantID ='" + machine + "'");
                                    else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                        results1 = dtShift.Select("PDate = '" + date + "' AND GroupID ='" + machine + "'");
                                    else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                        results1 = dtShift.Select("PDate = '" + date + "' AND ComponentID ='" + machine + "'");
                                    else
                                        results1 = dtShift.Select("PDate = '" + date + "' AND OperatorID ='" + machine + "'");

                                    foreach (DataRow drshift in results1)
                                    {
                                        if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                        {
                                            chart.drilldown[i].data.Add(new DrildownData
                                            {
                                                name = drshift["Shift"].ToString(),
                                                y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                                                drilldown = machine + "/" + month + "/" + Convert.ToDateTime(drshift["PDate"]).ToString("dd") + "/" + drshift["Shift"].ToString(),
                                                afterTitel = machine + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd") + "-" + drshift["Shift"].ToString() + ")", //+ "/" + drshift["Shift"].ToString(),
                                                                                                                                                                                                       //beforeTitle = paramText + " : " + machine + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd") + " )",
                                            });
                                        }
                                        else
                                        {
                                            chart.drilldown[i].data.Add(new DrildownData
                                            {
                                                name = drshift["Shift"].ToString(),
                                                y = Convert.ToDecimal(drshift["Parameter"].ToString())
                                            });
                                        }
                                    }
                                    i++;
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
                //throw ex;
            }
            stopwatch.Stop();
            Logger.WriteDebugLog("BindDashboardChartData : " + stopwatch.Elapsed.TotalSeconds);
            return chart;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Chart<Series_VDG> GetLineOEEChartData(string plantId, string strYear, string strMonth, string strDay, string param, string strShift, string componentId, string employeeId, string cellId, string SortColumn, string chartOrder, string paramText, string viewType)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool isCombined = BindCockpitView.IsChartCombined_OEEDAshboard();
            string CombinedCharts = isCombined ? BindCockpitView.GetCombinedChartNames_OEEDAshboard() : param;
            var distCharts = CombinedCharts.Split(',');
            if (!distCharts.Contains(param) || viewType == "ComponentwiseView" || viewType == "OperatorwiseView")
            {
                distCharts = new string[0];
                distCharts = new string[] { param };
            }
            Chart<Series_VDG> chartData = null;
            chartData = new Chart<Series_VDG>
            {
                Title = (distCharts.Length > 1) ? "Efficiency (%)" : paramText,
                Subtitle = "SubTitle",
                XAxisTitle = paramText,
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };
            string date = string.Empty;
            DataTable dtYear = new DataTable();
            DataTable dtMonth = new DataTable();
            DataTable dtDay = new DataTable();
            DataTable dtShift = new DataTable();
            DataTable dtHours = new DataTable();
            List<string> shiftData = new List<string>();
            List<string> machineData = new List<string>();
            List<string> monthData = new List<string>();
            string[] monthName = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            if (strShift.Equals("All", StringComparison.OrdinalIgnoreCase))
                strShift = "";
            if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
                plantId = "";
            if (componentId.Equals("All", StringComparison.OrdinalIgnoreCase))
                componentId = "";
            if (employeeId.Equals("All", StringComparison.OrdinalIgnoreCase))
                employeeId = "";
            if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase))
                cellId = "";

            #region "Month Condition------------"
            if (strMonth.Equals("Jan", StringComparison.OrdinalIgnoreCase))
                strMonth = "01";
            else if (strMonth.Equals("Feb", StringComparison.OrdinalIgnoreCase))
                strMonth = "02";
            else if (strMonth.Equals("Mar", StringComparison.OrdinalIgnoreCase))
                strMonth = "03";
            else if (strMonth.Equals("Apr", StringComparison.OrdinalIgnoreCase))
                strMonth = "04";
            else if (strMonth.Equals("May", StringComparison.OrdinalIgnoreCase))
                strMonth = "05";
            else if (strMonth.Equals("Jun", StringComparison.OrdinalIgnoreCase))
                strMonth = "06";
            else if (strMonth.Equals("Jul", StringComparison.OrdinalIgnoreCase))
                strMonth = "07";
            else if (strMonth.Equals("Aug", StringComparison.OrdinalIgnoreCase))
                strMonth = "08";
            else if (strMonth.Equals("Sep", StringComparison.OrdinalIgnoreCase))
                strMonth = "09";
            else if (strMonth.Equals("Oct", StringComparison.OrdinalIgnoreCase))
                strMonth = "10";
            else if (strMonth.Equals("Nov", StringComparison.OrdinalIgnoreCase))
                strMonth = "11";
            else if (strMonth.Equals("Dec", StringComparison.OrdinalIgnoreCase))
                strMonth = "12";
            #endregion
            try
            {
                if (strYear != "" && strMonth != "" && strDay != "")
                {
                    #region "Day Wise Information-------------"
                    date = strYear + "-" + strMonth + "-" + strDay;
                    chartData.series = new List<Series_VDG>();
                    Series_VDG charttdataaa = null;
                    foreach (var chartt in distCharts)
                    {
                        param = chartt;
                        dtMonth = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "currentDAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);
                        charttdataaa = new Series_VDG();
                        charttdataaa.name = HttpContext.GetLocalResourceObject("~/Dashboard.aspx", param).ToString();
                        charttdataaa.type = "line";
                        //charttdataaa.colorByPoint = true;
                        charttdataaa.currentParam = "day";
                        charttdataaa.nextParam = "shift";
                        charttdataaa.machine = "";
                        charttdataaa.day = "";
                        charttdataaa.month = strMonth;
                        charttdataaa.btnVisible = "hidden";
                        charttdataaa.previousParam = "";
                        //charttdataaa.data = new List<Data>();
                        charttdataaa.data = new List<double>();
                        charttdataaa.Category = new List<string>();
                        //Data dataa = null;
                        foreach (DataRow item in dtMonth.Rows)
                        {
                            machineData.Add(viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                            //dataa = new Data();
                            //dataa.name = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                            //if (!Convert.IsDBNull(item["Parameter"]))
                            //{
                            //    dataa.y = Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0));
                            //}
                            charttdataaa.data.Add(Convert.IsDBNull(item["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0)));
                            charttdataaa.Category.Add((viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()));
                        }
                        chartData.series.Add(charttdataaa);
                    }
                    #endregion
                    #region-------------------------------Shift Wise Info-----------------------------------
                    //if (SortColumn == "MachineId")
                    //    SortColumn = "OEffy";
                    //chartData.drilldown = new List<DrildownSeries>();
                    //int i = 0;
                    //dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    //foreach (var machine in machineData)
                    //{
                    //    chart.drilldown.Add(new DrildownSeries
                    //    {
                    //        name = machine,
                    //        id = machine + "-" + strMonth + "-" + strDay,
                    //        data = new List<DrildownData>(),
                    //    });
                    //    DataRow[] results = null;
                    //    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtShift.Select("Month = " + strMonth + " AND MachineID ='" + machine + "' AND Day ='" + strDay + "'");
                    //    else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtShift.Select("Month = " + strMonth + " AND PlantID ='" + machine + "' AND Day ='" + strDay + "'");
                    //    else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtShift.Select("Month = " + strMonth + " AND GroupID ='" + machine + "' AND Day ='" + strDay + "'");
                    //    else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtShift.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "' AND Day ='" + strDay + "'");
                    //    else
                    //        results = dtShift.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "' AND Day ='" + strDay + "'");

                    //    foreach (DataRow drshift in results)
                    //    {
                    //        //if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase) || viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //        //{
                    //        if (!DataBaseAccess.HourlyLevelDrillDown)
                    //        {
                    //            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //            {
                    //                chart.drilldown[i].data.Add(new DrildownData
                    //                {
                    //                    name = drshift["Shift"].ToString(),
                    //                    y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                    //                    drilldown = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString(),
                    //                    afterTitel = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["Shift"].ToString() + ")",
                    //                    beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")"
                    //                });
                    //            }
                    //            else
                    //            {
                    //                chart.drilldown[i].data.Add(new DrildownData
                    //                {
                    //                    name = drshift["Shift"].ToString(),
                    //                    y = Convert.ToDecimal(drshift["Parameter"].ToString())
                    //                });
                    //            }
                    //        }
                    //        else
                    //        {
                    //            chart.drilldown[i].data.Add(new DrildownData
                    //            {
                    //                name = drshift["Shift"].ToString(),
                    //                y = Convert.ToDecimal(drshift["Parameter"].ToString())
                    //            });
                    //        }
                    //        shiftData.Add(drshift["Shift"].ToString());
                    //    }
                    //    i++;
                    //}
                    #endregion
                    if (DataBaseAccess.HourlyLevelDrillDown)
                    {
                        #region-------------------------------Hours Wise Info-----------------------------------
                        //shiftData = (from w in shiftData select w).Distinct().ToList();
                        //dtHours = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "CurrentDAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                        //foreach (var machine in machineData)
                        //{
                        //    foreach (var shift in shiftData)
                        //    {
                        //        chart.drilldown.Add(new DrildownSeries
                        //        {
                        //            name = machine,
                        //            id = machine + "/" + strMonth + "/" + strDay + "/" + shift,
                        //            data = new List<DrildownData>(),
                        //        });
                        //        DataRow[] results = null;
                        //        if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                        //            results = dtHours.Select("Month = " + strMonth + " AND MachineID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                        //        else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                        //            results = dtHours.Select("Month = " + strMonth + " AND PlantID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                        //        else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                        //            results = dtHours.Select("Month = " + strMonth + " AND GroupID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                        //        else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                        //            results = dtHours.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                        //        else
                        //            results = dtHours.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");

                        //        foreach (DataRow drshift in results)
                        //        {
                        //            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                        //            {
                        //                chart.drilldown[i].data.Add(new DrildownData
                        //                {
                        //                    name = drshift["HourID"].ToString(),
                        //                    y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                        //                    drilldown = drshift["MachineID"].ToString() + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["ShiftName"].ToString(),
                        //                    afterTitel = drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["ShiftName"].ToString() + ")",
                        //                    beforeTitle = paramText + " : " + drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")"
                        //                });
                        //            }
                        //            else
                        //            {
                        //                chart.drilldown[i].data.Add(new DrildownData
                        //                {
                        //                    name = drshift["HourID"].ToString(),
                        //                    y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                        //                });
                        //            }
                        //        }
                        //        i++;
                        //    }
                        //}
                        #endregion
                    }

                }
                else if (strYear != "" && strMonth != "")
                {
                    #region "month Wise Information ------------"
                    date = strYear + "-" + strMonth + "-" + "01";
                    chartData.series = new List<Series_VDG>();
                    Series_VDG charttdataaa = null;
                    foreach (var chartt in distCharts)
                    {
                        param = chartt;
                        dtMonth = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "currentMONTH", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);
                        //chartData.series = new List<Series>();
                        charttdataaa = new Series_VDG();
                        charttdataaa.name = param;
                        charttdataaa.type = "line";
                        //charttdataaa.colorByPoint = true;
                        charttdataaa.currentParam = "month";
                        charttdataaa.nextParam = "day";
                        charttdataaa.machine = "";
                        charttdataaa.day = "";
                        charttdataaa.month = strMonth;
                        charttdataaa.btnVisible = "hidden";
                        charttdataaa.previousParam = "";
                        //charttdataaa.data = new List<Data>();
                        charttdataaa.data = new List<double>();
                        charttdataaa.Category = new List<string>();
                        //Data dataa = null;
                        foreach (DataRow item in dtMonth.Rows)
                        {
                            machineData.Add(viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                            //dataa = new Data();
                            //dataa.name = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                            //if (!Convert.IsDBNull(item["Parameter"]))
                            //{
                            //    dataa.y = Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0));
                            //}
                            //charttdataaa.data.Add(dataa);
                            charttdataaa.data.Add(Convert.IsDBNull(item["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0)));
                            charttdataaa.Category.Add((viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()));
                        }
                        chartData.series.Add(charttdataaa);
                    }
                    #region---------------------------Day Wise Start---------------------------------
                    //if (SortColumn == "MachineId")
                    //    SortColumn = "OEffy";
                    //chartData.drilldown = new List<DrildownSeries>();
                    //int i = 0;

                    //dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "DAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    //foreach (var machine in machineData)
                    //{
                    //    chart.drilldown.Add(new DrildownSeries
                    //    {
                    //        name = machine,
                    //        id = machine,
                    //        data = new List<DrildownData>(),
                    //    });
                    //    DataRow[] results = null;
                    //    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtDay.Select("Month = " + strMonth + " AND MachineID ='" + machine + "'");
                    //    else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtDay.Select("Month = " + strMonth + " AND PlantID ='" + machine + "'");
                    //    else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtDay.Select("Month = " + strMonth + " AND GroupID ='" + machine + "'");
                    //    else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtDay.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "'");
                    //    else
                    //        results = dtDay.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "'");

                    //    foreach (DataRow dr in results)
                    //    {
                    //        chart.drilldown[i].data.Add(new DrildownData
                    //        {
                    //            name = "Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                    //            y = Convert.ToDecimal(dr["Parameter"].ToString()),
                    //            drilldown = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + "/" + strMonth + "/" + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                    //            afterTitel = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + strMonth + "-" + Convert.ToDateTime(dr["PDate"]).ToString("dd") + ")",
                    //            beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + strMonth + ")"
                    //        });
                    //        monthData.Add(Convert.ToDateTime(dr["PDate"]).ToString("dd"));
                    //    }
                    //    i++;
                    //}
                    #endregion
                    #region--------------------------Shift Wise Information-----------------------------
                    //monthData = (from w in monthData select w).Distinct().ToList();
                    //dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    //foreach (var machine in machineData)
                    //{
                    //    foreach (var day in monthData)
                    //    {
                    //        chart.drilldown.Add(new DrildownSeries
                    //        {
                    //            name = machine,
                    //            id = machine + "/" + strMonth + "/" + day,
                    //            data = new List<DrildownData>(),
                    //        });
                    //        DataRow[] results = null;
                    //        if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //            results = dtShift.Select("Month = " + strMonth + " AND MachineID ='" + machine + "' AND Day ='" + day + "'");
                    //        else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //            results = dtShift.Select("Month = " + strMonth + " AND PlantID ='" + machine + "' AND Day ='" + day + "'");
                    //        else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                    //            results = dtShift.Select("Month = " + strMonth + " AND GroupID ='" + machine + "' AND Day ='" + day + "'");
                    //        else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    //            results = dtShift.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "' AND Day ='" + day + "'");
                    //        else
                    //            results = dtShift.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "' AND Day ='" + day + "'");

                    //        foreach (DataRow drshift in results)
                    //        {
                    //            //if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase) || viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //            //{
                    //            if (!DataBaseAccess.HourlyLevelDrillDown)
                    //            {
                    //                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //                {
                    //                    chart.drilldown[i].data.Add(new DrildownData
                    //                    {
                    //                        name = drshift["Shift"].ToString(),
                    //                        y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                    //                        drilldown = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString(),
                    //                        afterTitel = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["Shift"].ToString() + ")",
                    //                        beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")",
                    //                    });
                    //                }
                    //                else
                    //                {
                    //                    chart.drilldown[i].data.Add(new DrildownData
                    //                    {
                    //                        name = drshift["Shift"].ToString(),
                    //                        y = Convert.ToDecimal(drshift["Parameter"].ToString())
                    //                    });
                    //                }
                    //            }
                    //            else
                    //            {
                    //                chart.drilldown[i].data.Add(new DrildownData
                    //                {
                    //                    name = drshift["Shift"].ToString(),
                    //                    y = Convert.ToDecimal(drshift["Parameter"].ToString())
                    //                });
                    //            }
                    //            shiftData.Add(drshift["Shift"].ToString());
                    //        }
                    //        i++;
                    //    }
                    //}
                    #endregion
                    if (DataBaseAccess.HourlyLevelDrillDown)
                    {
                        #region-------------------------------Hours Wise Info-----------------------------------
                        //shiftData = (from w in shiftData select w).Distinct().ToList();
                        //dtHours = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "Currentmonth", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                        //foreach (var machine in machineData)
                        //{
                        //    foreach (var day in monthData)
                        //    {
                        //        foreach (var shift in shiftData)
                        //        {
                        //            chart.drilldown.Add(new DrildownSeries
                        //            {
                        //                name = machine,
                        //                id = machine + "/" + strMonth + "/" + day + "/" + shift,
                        //                data = new List<DrildownData>(),
                        //            });
                        //            DataRow[] results = null;
                        //            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                        //                results = dtHours.Select("Month = " + strMonth + " AND MachineID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                        //            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                        //                results = dtHours.Select("Month = " + strMonth + " AND PlantID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                        //            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                        //                results = dtHours.Select("Month = " + strMonth + " AND GroupID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                        //            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                        //                results = dtHours.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                        //            else
                        //                results = dtHours.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                        //            foreach (DataRow drshift in results)
                        //            {
                        //                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                        //                {
                        //                    chart.drilldown[i].data.Add(new DrildownData
                        //                    {
                        //                        name = drshift["HourID"].ToString(),
                        //                        y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                        //                        drilldown = drshift["MachineID"].ToString() + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["ShiftName"].ToString(),
                        //                        afterTitel = drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["ShiftName"].ToString() + ")",
                        //                        beforeTitle = paramText + " : " + drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")"
                        //                    });
                        //                }
                        //                else
                        //                {
                        //                    chart.drilldown[i].data.Add(new DrildownData
                        //                    {
                        //                        name = drshift["HourID"].ToString(),
                        //                        y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                        //                    });
                        //                }
                        //            }
                        //            i++;
                        //        }
                        //    }
                        //}
                        #endregion
                    }
                    #endregion
                }
                else if (strYear != "")
                {
                    #region "Year Wise Information -----------"
                    date = strYear + "-" + "01" + "-" + "01";
                    chartData.series = new List<Series_VDG>();
                    Series_VDG charttdataaa = null;
                    foreach (var chartt in distCharts)
                    {
                        param = chartt;
                        dtYear = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "YEAR", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                        //------------------------Year Wise Start--------------------------
                        charttdataaa = new Series_VDG();
                        charttdataaa.name = HttpContext.GetLocalResourceObject("~/Dashboard.aspx", param).ToString();
                        charttdataaa.type = "line";
                        //charttdataaa.colorByPoint = true;
                        charttdataaa.currentParam = "year";
                        charttdataaa.nextParam = "month";
                        charttdataaa.machine = "";
                        charttdataaa.day = "";
                        charttdataaa.month = "";
                        charttdataaa.btnVisible = "hidden";
                        charttdataaa.previousParam = "";
                        //charttdataaa.data = new List<Data>();
                        charttdataaa.data = new List<double>();
                        charttdataaa.Category = new List<string>();
                        //Data dataa = null;
                        foreach (DataRow item in dtYear.Rows)
                        {
                            machineData.Add(viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                            //dataa = new Data();
                            //dataa.name = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                            //dataa.y = Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0));
                            //charttdataaa.data.Add(dataa);
                            charttdataaa.data.Add(Convert.IsDBNull(item["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(item["Parameter"].ToString()), 0)));
                            charttdataaa.Category.Add((viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()));

                        }
                        chartData.series.Add(charttdataaa);
                    }
                    #endregion
                    #region---------------------------Month Wise Start----------------------------
                    //if (SortColumn == "MachineId")
                    //    SortColumn = "OEffy";
                    //dtMonth = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "MONTH", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    //chartData.drilldown = new List<DrildownSeries>();
                    //int i = 0;
                    //foreach (var machine in machineData)
                    //{
                    //    chart.drilldown.Add(new DrildownSeries
                    //    {
                    //        name = machine,
                    //        id = machine,
                    //        data = new List<DrildownData>(),
                    //    });
                    //    DataRow[] results = null;
                    //    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtMonth.Select("MachineID = '" + machine + "'");
                    //    else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtMonth.Select("PlantID = '" + machine + "'");
                    //    else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtMonth.Select("GroupID = '" + machine + "'");
                    //    else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    //        results = dtMonth.Select("ComponentID = '" + machine + "'");
                    //    else
                    //        results = dtMonth.Select("OperatorID = '" + machine + "'");
                    //    if (results != null)
                    //    {
                    //        foreach (DataRow drmonth in results)
                    //        {
                    //            chart.drilldown[i].data.Add(new DrildownData
                    //            {
                    //                name = drmonth["NameOftheMonth"].ToString(),
                    //                y = Convert.ToDecimal(drmonth["Parameter"].ToString()),
                    //                drilldown = (viewType == "MachinewiseView" ? drmonth["MachineID"].ToString() : viewType == "CellWiseView" ? drmonth["GroupID"].ToString() : viewType == "PlantwiseView" ? drmonth["PlantID"].ToString() : viewType == "ComponentwiseView" ? drmonth["ComponentID"].ToString() : drmonth["OperatorID"].ToString()) + "-" + Convert.ToDateTime(drmonth["Startdate"]).ToString("MM"),
                    //                afterTitel = (viewType == "MachinewiseView" ? drmonth["MachineID"].ToString() : viewType == "CellWiseView" ? drmonth["GroupID"].ToString() : viewType == "PlantwiseView" ? drmonth["PlantID"].ToString() : viewType == "ComponentwiseView" ? drmonth["ComponentID"].ToString() : drmonth["OperatorID"].ToString()) + " (" + strYear + "-" + Convert.ToDateTime(drmonth["Startdate"]).ToString("MM") + ")",
                    //                beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? drmonth["MachineID"].ToString() : viewType == "CellWiseView" ? drmonth["GroupID"].ToString() : viewType == "PlantwiseView" ? drmonth["PlantID"].ToString() : viewType == "ComponentwiseView" ? drmonth["ComponentID"].ToString() : drmonth["OperatorID"].ToString()) + " (" + strYear + ")"
                    //            });
                    //        }
                    //        i++;
                    //    }
                    //}
                    #endregion
                    #region---------------------------Day Wise Start---------------------------------

                    //dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "DAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    //foreach (var machine in machineData)
                    //{
                    //    foreach (var month in monthName)
                    //    {
                    //        chart.drilldown.Add(new DrildownSeries
                    //        {
                    //            name = machine,
                    //            id = machine + "-" + month,
                    //            data = new List<DrildownData>(),
                    //        });
                    //        DataRow[] results = null;
                    //        if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //            results = dtDay.Select("Month = " + month + " AND MachineID ='" + machine + "'");
                    //        else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //            results = dtDay.Select("Month = " + month + " AND PlantID ='" + machine + "'");
                    //        else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                    //            results = dtDay.Select("Month = " + month + " AND GroupID ='" + machine + "'");
                    //        else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    //            results = dtDay.Select("Month = " + month + " AND ComponentID ='" + machine + "'");
                    //        else
                    //            results = dtDay.Select("Month = " + month + " AND OperatorID ='" + machine + "'");

                    //        if (results != null)
                    //        {
                    //            foreach (DataRow dr in results)
                    //            {
                    //                chart.drilldown[i].data.Add(new DrildownData
                    //                {
                    //                    name = "Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                    //                    y = Convert.ToDecimal(dr["Parameter"].ToString()),
                    //                    drilldown = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + "/" + month + "/" + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                    //                    afterTitel = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + month + ")",// "-" + Convert.ToDateTime(dr["PDate"]).ToString("dd") +
                    //                    beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + month + ")"
                    //                });
                    //            }
                    //            i++;
                    //        }
                    //    }
                    //}
                    #endregion
                    #region-------------------------------------Shift Wise Start---------------------------------------------------------
                    //if (DataBaseAccess.ThirdLevelDrillDown)
                    //{
                    //    //date = string.Empty;

                    //    dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    //    foreach (var machine in machineData)
                    //    {
                    //        foreach (var month in monthName)
                    //        {
                    //            date = strYear + "-" + month + "-" + "01";
                    //            // dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "DAY", param, componentId, employeeId, SortColumn, "");
                    //            DataRow[] results = null;
                    //            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //                results = dtDay.Select("Month = '" + month + "' AND MachineID ='" + machine + "'");//"MachineID = '" + machine + "'" ///"PDate = '" + date + "' AND MachineID ='" + machine + "'"
                    //            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //                results = dtDay.Select("Month = '" + month + "' AND PlantID ='" + machine + "'");
                    //            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                    //                results = dtDay.Select("Month = '" + month + "' AND GroupID ='" + machine + "'");
                    //            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    //                results = dtDay.Select("Month = '" + month + "' AND ComponentID ='" + machine + "'");
                    //            else
                    //                results = dtDay.Select("Month = '" + month + "' AND OperatorID ='" + machine + "'");

                    //            foreach (DataRow day in results)
                    //            {
                    //                chart.drilldown.Add(new DrildownSeries
                    //                {
                    //                    id = machine + "/" + month + "/" + Convert.ToDateTime(day["PDate"]).ToString("dd"),
                    //                    name = machine,//+ "/" + strYear + "/" + month + "/" + Convert.ToDateTime(day["PDate"]).ToString("dd"),
                    //                                   //drilldown = machine + "/" + strYear + "/" + month + "/" + Convert.ToDateTime(day["PDate"]).ToString("dd"),
                    //                                   //afterTitel = machine + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd") + ")",
                    //                                   //beforeTitle = paramText + " : " + machine + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd") + " )",
                    //                    data = new List<DrildownData>(),
                    //                });
                    //                date = strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd");
                    //                // dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "SHIFT", param, componentId, employeeId, SortColumn, "");
                    //                DataRow[] results1 = null;
                    //                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //                    results1 = dtShift.Select("PDate = '" + date + "' AND MachineID ='" + machine + "'");
                    //                else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                    //                    results1 = dtShift.Select("PDate = '" + date + "' AND PlantID ='" + machine + "'");
                    //                else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                    //                    results1 = dtShift.Select("PDate = '" + date + "' AND GroupID ='" + machine + "'");
                    //                else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    //                    results1 = dtShift.Select("PDate = '" + date + "' AND ComponentID ='" + machine + "'");
                    //                else
                    //                    results1 = dtShift.Select("PDate = '" + date + "' AND OperatorID ='" + machine + "'");

                    //                foreach (DataRow drshift in results1)
                    //                {
                    //                    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                    //                    {
                    //                        chart.drilldown[i].data.Add(new DrildownData
                    //                        {
                    //                            name = drshift["Shift"].ToString(),
                    //                            y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                    //                            drilldown = machine + "/" + month + "/" + Convert.ToDateTime(drshift["PDate"]).ToString("dd") + "/" + drshift["Shift"].ToString(),
                    //                            afterTitel = machine + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd") + "-" + drshift["Shift"].ToString() + ")", //+ "/" + drshift["Shift"].ToString(),
                    //                                                                                                                                                                                   //beforeTitle = paramText + " : " + machine + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd") + " )",
                    //                        });//, id = machine + "/" + strYear + "/" + month + "/" + Convert.ToDateTime(drshift["PDate"]).ToString("dd") 
                    //                    }
                    //                    else
                    //                    {
                    //                        chart.drilldown[i].data.Add(new DrildownData
                    //                        {
                    //                            name = drshift["Shift"].ToString(),
                    //                            y = Convert.ToDecimal(drshift["Parameter"].ToString())
                    //                        });
                    //                    }
                    //                }
                    //                i++;
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion

                }
                ///--------------------End Chrome------------------
                ///
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            stopwatch.Stop();
            Logger.WriteDebugLog("BindDashboardChartData : " + stopwatch.Elapsed.TotalSeconds);
            HttpContext.Current.Session["ChartDataForBackFun"] = null;
            List<Chart<Series_VDG>> chartList = new List<Chart<Series_VDG>>();
            chartList.Add(chartData);
            HttpContext.Current.Session["ChartDataForBackFun"] = chartList;
            return chartData;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Chart<Series> GetLineOEEChartData_old(string plantId, string strYear, string strMonth, string strDay, string param, string strShift, string componentId, string employeeId, string cellId, string SortColumn, string chartOrder, string paramText, string viewType)
        {
            var chart = new Chart<Series>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = paramText,
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };
            string date = string.Empty;
            DataTable dtYear = new DataTable();
            DataTable dtMonth = new DataTable();
            DataTable dtDay = new DataTable();
            DataTable dtShift = new DataTable();
            DataTable dtHours = new DataTable();
            List<string> shiftData = new List<string>();
            List<string> machineData = new List<string>();
            List<string> monthData = new List<string>();
            string[] monthName = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            if (strShift.Equals("All", StringComparison.OrdinalIgnoreCase))
                strShift = "";
            if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
                plantId = "";
            if (componentId.Equals("All", StringComparison.OrdinalIgnoreCase))
                componentId = "";
            if (employeeId.Equals("All", StringComparison.OrdinalIgnoreCase))
                employeeId = "";
            if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase))
                cellId = "";

            #region "Month Condition------------"
            if (strMonth.Equals("Jan", StringComparison.OrdinalIgnoreCase))
                strMonth = "01";
            else if (strMonth.Equals("Feb", StringComparison.OrdinalIgnoreCase))
                strMonth = "02";
            else if (strMonth.Equals("Mar", StringComparison.OrdinalIgnoreCase))
                strMonth = "03";
            else if (strMonth.Equals("Apr", StringComparison.OrdinalIgnoreCase))
                strMonth = "04";
            else if (strMonth.Equals("May", StringComparison.OrdinalIgnoreCase))
                strMonth = "05";
            else if (strMonth.Equals("Jun", StringComparison.OrdinalIgnoreCase))
                strMonth = "06";
            else if (strMonth.Equals("Jul", StringComparison.OrdinalIgnoreCase))
                strMonth = "07";
            else if (strMonth.Equals("Aug", StringComparison.OrdinalIgnoreCase))
                strMonth = "08";
            else if (strMonth.Equals("Sep", StringComparison.OrdinalIgnoreCase))
                strMonth = "09";
            else if (strMonth.Equals("Oct", StringComparison.OrdinalIgnoreCase))
                strMonth = "10";
            else if (strMonth.Equals("Nov", StringComparison.OrdinalIgnoreCase))
                strMonth = "11";
            else if (strMonth.Equals("Dec", StringComparison.OrdinalIgnoreCase))
                strMonth = "12";
            #endregion

            if (strYear != "" && strMonth != "" && strDay != "")
            {
                #region "Day Wise Information-------------"
                date = strYear + "-" + strMonth + "-" + strDay;

                dtMonth = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "currentDAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                chart.series = new List<Series>();
                chart.series.Add(new Series
                {
                    name = HttpContext.GetLocalResourceObject("~/Dashboard.aspx", param).ToString(),
                    type = "line",
                    data = new List<Data>()
                });
                foreach (DataRow item in dtMonth.Rows)
                {
                    machineData.Add(viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                    chart.series[0].data.Add(new Data
                    {
                        name = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()),
                        y = Convert.ToInt32(Convert.ToDecimal(item["Parameter"].ToString())),
                        drilldown = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + "-" + Convert.ToDateTime(item["PDate"]).ToString("MM") + "-" + strDay,
                        afterTitel = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + " (" + strYear + "-" + strMonth + "-" + strDay + ")",
                        beforeTitle = ""
                    });
                }


                //-------------------------------Shift Wise Info-----------------------------------
                if (SortColumn == "MachineId")
                    SortColumn = "OEffy";
                chart.drilldown = new List<DrildownSeries>();
                int i = 0;
                dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                foreach (var machine in machineData)
                {
                    chart.drilldown.Add(new DrildownSeries
                    {
                        name = machine,
                        id = machine + "-" + strMonth + "-" + strDay,
                        data = new List<DrildownData>(),
                    });
                    DataRow[] results = null;
                    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                        results = dtShift.Select("Month = " + strMonth + " AND MachineID ='" + machine + "' AND Day ='" + strDay + "'");
                    else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                        results = dtShift.Select("Month = " + strMonth + " AND PlantID ='" + machine + "' AND Day ='" + strDay + "'");
                    else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                        results = dtShift.Select("Month = " + strMonth + " AND GroupID ='" + machine + "' AND Day ='" + strDay + "'");
                    else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                        results = dtShift.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "' AND Day ='" + strDay + "'");
                    else
                        results = dtShift.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "' AND Day ='" + strDay + "'");

                    foreach (DataRow drshift in results)
                    {
                        //if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase) || viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                        //{
                        if (!DataBaseAccess.HourlyLevelDrillDown)
                        {
                            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                            {
                                chart.drilldown[i].data.Add(new DrildownData
                                {
                                    name = drshift["Shift"].ToString(),
                                    y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                                    drilldown = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString(),
                                    afterTitel = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["Shift"].ToString() + ")",
                                    beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")"
                                });
                            }
                            else
                            {
                                chart.drilldown[i].data.Add(new DrildownData
                                {
                                    name = drshift["Shift"].ToString(),
                                    y = Convert.ToDecimal(drshift["Parameter"].ToString())
                                });
                            }
                        }
                        else
                        {
                            chart.drilldown[i].data.Add(new DrildownData
                            {
                                name = drshift["Shift"].ToString(),
                                y = Convert.ToDecimal(drshift["Parameter"].ToString())
                            });
                        }
                        shiftData.Add(drshift["Shift"].ToString());
                    }
                    i++;
                }

                if (DataBaseAccess.HourlyLevelDrillDown)
                {
                    //-------------------------------Hours Wise Info-----------------------------------
                    shiftData = (from w in shiftData select w).Distinct().ToList();
                    dtHours = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "CurrentDAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    foreach (var machine in machineData)
                    {
                        foreach (var shift in shiftData)
                        {
                            chart.drilldown.Add(new DrildownSeries
                            {
                                name = machine,
                                id = machine + "/" + strMonth + "/" + strDay + "/" + shift,
                                data = new List<DrildownData>(),
                            });
                            DataRow[] results = null;
                            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtHours.Select("Month = " + strMonth + " AND MachineID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtHours.Select("Month = " + strMonth + " AND PlantID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtHours.Select("Month = " + strMonth + " AND GroupID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtHours.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");
                            else
                                results = dtHours.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "' AND Day =" + strDay + " AND ShiftName ='" + shift + "'");

                            foreach (DataRow drshift in results)
                            {
                                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                {
                                    chart.drilldown[i].data.Add(new DrildownData
                                    {
                                        name = drshift["HourID"].ToString(),
                                        y = Convert.ToInt32(Convert.ToDecimal(drshift["Parameter"].ToString())),
                                        drilldown = drshift["MachineID"].ToString() + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["ShiftName"].ToString(),
                                        afterTitel = drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["ShiftName"].ToString() + ")",
                                        beforeTitle = paramText + " : " + drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")"
                                    });
                                }
                                else
                                {
                                    chart.drilldown[i].data.Add(new DrildownData
                                    {
                                        name = drshift["HourID"].ToString(),
                                        y = Convert.ToInt32(Convert.ToDecimal(drshift["Parameter"].ToString())),
                                    });
                                }
                            }
                            i++;
                        }
                    }
                }
                #endregion
            }
            else if (strYear != "" && strMonth != "")
            {
                #region "month Wise Information ------------"
                date = strYear + "-" + strMonth + "-" + "01";
                dtMonth = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "currentMONTH", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                chart.series = new List<Series>();
                chart.series.Add(new Series
                {
                    name = HttpContext.GetLocalResourceObject("~/Dashboard.aspx", param).ToString(),
                    type = "line",
                    data = new List<Data>()
                });
                foreach (DataRow item in dtMonth.Rows)
                {
                    machineData.Add(viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                    chart.series[0].data.Add(new Data
                    {
                        name = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()),
                        y = Convert.ToInt32(Convert.ToDecimal(item["Parameter"].ToString())),
                        drilldown = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()),
                        afterTitel = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + " (" + strYear + "-" + strMonth + ")",
                        beforeTitle = ""
                    });
                }

                //---------------------------Day Wise Start---------------------------------
                if (SortColumn == "MachineId")
                    SortColumn = "OEffy";
                chart.drilldown = new List<DrildownSeries>();
                int i = 0;

                dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "DAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                foreach (var machine in machineData)
                {
                    chart.drilldown.Add(new DrildownSeries
                    {
                        name = machine,
                        id = machine,
                        data = new List<DrildownData>(),
                    });
                    DataRow[] results = null;
                    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                        results = dtDay.Select("Month = " + strMonth + " AND MachineID ='" + machine + "'");
                    else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                        results = dtDay.Select("Month = " + strMonth + " AND PlantID ='" + machine + "'");
                    else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                        results = dtDay.Select("Month = " + strMonth + " AND GroupID ='" + machine + "'");
                    else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                        results = dtDay.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "'");
                    else
                        results = dtDay.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "'");

                    foreach (DataRow dr in results)
                    {
                        chart.drilldown[i].data.Add(new DrildownData
                        {
                            name = "Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                            y = Convert.ToDecimal(dr["Parameter"].ToString()),
                            drilldown = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + "/" + strMonth + "/" + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                            afterTitel = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + strMonth + "-" + Convert.ToDateTime(dr["PDate"]).ToString("dd") + ")",
                            beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + strMonth + ")"
                        });
                        monthData.Add(Convert.ToDateTime(dr["PDate"]).ToString("dd"));
                    }
                    i++;
                }
                //--------------------------Shift Wise Information-----------------------------
                monthData = (from w in monthData select w).Distinct().ToList();
                dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                foreach (var machine in machineData)
                {
                    foreach (var day in monthData)
                    {
                        chart.drilldown.Add(new DrildownSeries
                        {
                            name = machine,
                            id = machine + "/" + strMonth + "/" + day,
                            data = new List<DrildownData>(),
                        });
                        DataRow[] results = null;
                        if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtShift.Select("Month = " + strMonth + " AND MachineID ='" + machine + "' AND Day ='" + day + "'");
                        else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtShift.Select("Month = " + strMonth + " AND PlantID ='" + machine + "' AND Day ='" + day + "'");
                        else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtShift.Select("Month = " + strMonth + " AND GroupID ='" + machine + "' AND Day ='" + day + "'");
                        else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtShift.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "' AND Day ='" + day + "'");
                        else
                            results = dtShift.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "' AND Day ='" + day + "'");

                        foreach (DataRow drshift in results)
                        {
                            //if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase) || viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                            //{
                            if (!DataBaseAccess.HourlyLevelDrillDown)
                            {
                                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                {
                                    chart.drilldown[i].data.Add(new DrildownData
                                    {
                                        name = drshift["Shift"].ToString(),
                                        y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                                        drilldown = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString(),
                                        afterTitel = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["Shift"].ToString() + ")",
                                        beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "CellWiseView" ? drshift["GroupID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")",
                                    });
                                }
                                else
                                {
                                    chart.drilldown[i].data.Add(new DrildownData
                                    {
                                        name = drshift["Shift"].ToString(),
                                        y = Convert.ToDecimal(drshift["Parameter"].ToString())
                                    });
                                }
                            }
                            else
                            {
                                chart.drilldown[i].data.Add(new DrildownData
                                {
                                    name = drshift["Shift"].ToString(),
                                    y = Convert.ToDecimal(drshift["Parameter"].ToString())
                                });
                            }
                            shiftData.Add(drshift["Shift"].ToString());
                        }
                        i++;
                    }
                }

                if (DataBaseAccess.HourlyLevelDrillDown)
                {
                    //-------------------------------Hours Wise Info-----------------------------------
                    shiftData = (from w in shiftData select w).Distinct().ToList();
                    dtHours = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "Currentmonth", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    foreach (var machine in machineData)
                    {
                        foreach (var day in monthData)
                        {
                            foreach (var shift in shiftData)
                            {
                                chart.drilldown.Add(new DrildownSeries
                                {
                                    name = machine,
                                    id = machine + "/" + strMonth + "/" + day + "/" + shift,
                                    data = new List<DrildownData>(),
                                });
                                DataRow[] results = null;
                                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                    results = dtHours.Select("Month = " + strMonth + " AND MachineID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                                else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                    results = dtHours.Select("Month = " + strMonth + " AND PlantID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                                else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                    results = dtHours.Select("Month = " + strMonth + " AND GroupID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                                else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                    results = dtHours.Select("Month = " + strMonth + " AND ComponentID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                                else
                                    results = dtHours.Select("Month = " + strMonth + " AND OperatorID ='" + machine + "' AND Day =" + day + " AND ShiftName ='" + shift + "'");
                                foreach (DataRow drshift in results)
                                {
                                    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                    {
                                        chart.drilldown[i].data.Add(new DrildownData
                                        {
                                            name = drshift["HourID"].ToString(),
                                            y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                                            drilldown = drshift["MachineID"].ToString() + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["ShiftName"].ToString(),
                                            afterTitel = drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["ShiftName"].ToString() + ")",
                                            beforeTitle = paramText + " : " + drshift["MachineID"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")"
                                        });
                                    }
                                    else
                                    {
                                        chart.drilldown[i].data.Add(new DrildownData
                                        {
                                            name = drshift["HourID"].ToString(),
                                            y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                                        });
                                    }
                                }
                                i++;
                            }
                        }
                    }
                }
                #endregion
            }
            else if (strYear != "")
            {
                #region "Year Wise Information -----------"
                date = strYear + "-" + "01" + "-" + "01";
                dtYear = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "YEAR", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                //------------------------Year Wise Start--------------------------
                chart.series = new List<Series>();
                chart.series.Add(new Series
                {
                    name = HttpContext.GetLocalResourceObject("~/Dashboard.aspx", param).ToString(),
                    type = "line",
                    data = new List<Data>()
                });
                foreach (DataRow item in dtYear.Rows)
                {
                    machineData.Add(viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString());
                    chart.series[0].data.Add(new Data
                    {
                        name = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()),
                        y = Convert.ToInt32(Convert.ToDecimal(item["Parameter"].ToString())),
                        drilldown = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()),
                        afterTitel = (viewType == "MachinewiseView" ? item["MachineID"].ToString() : viewType == "CellWiseView" ? item["GroupID"].ToString() : viewType == "PlantwiseView" ? item["PlantID"].ToString() : viewType == "ComponentwiseView" ? item["ComponentID"].ToString() : item["OperatorID"].ToString()) + "(" + strYear + ")",
                        beforeTitle = ""
                    });
                }
                //---------------------------Month Wise Start----------------------------
                if (SortColumn == "MachineId")
                    SortColumn = "OEffy";
                dtMonth = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "MONTH", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                chart.drilldown = new List<DrildownSeries>();
                int i = 0;
                foreach (var machine in machineData)
                {
                    chart.drilldown.Add(new DrildownSeries
                    {
                        name = machine,
                        id = machine,
                        data = new List<DrildownData>(),
                    });
                    DataRow[] results = null;
                    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                        results = dtMonth.Select("MachineID = '" + machine + "'");
                    else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                        results = dtMonth.Select("PlantID = '" + machine + "'");
                    else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                        results = dtMonth.Select("GroupID = '" + machine + "'");
                    else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                        results = dtMonth.Select("ComponentID = '" + machine + "'");
                    else
                        results = dtMonth.Select("OperatorID = '" + machine + "'");
                    if (results != null)
                    {
                        foreach (DataRow drmonth in results)
                        {
                            chart.drilldown[i].data.Add(new DrildownData
                            {
                                name = drmonth["NameOftheMonth"].ToString(),
                                y = Convert.ToDecimal(drmonth["Parameter"].ToString()),
                                drilldown = (viewType == "MachinewiseView" ? drmonth["MachineID"].ToString() : viewType == "CellWiseView" ? drmonth["GroupID"].ToString() : viewType == "PlantwiseView" ? drmonth["PlantID"].ToString() : viewType == "ComponentwiseView" ? drmonth["ComponentID"].ToString() : drmonth["OperatorID"].ToString()) + "-" + Convert.ToDateTime(drmonth["Startdate"]).ToString("MM"),
                                afterTitel = (viewType == "MachinewiseView" ? drmonth["MachineID"].ToString() : viewType == "CellWiseView" ? drmonth["GroupID"].ToString() : viewType == "PlantwiseView" ? drmonth["PlantID"].ToString() : viewType == "ComponentwiseView" ? drmonth["ComponentID"].ToString() : drmonth["OperatorID"].ToString()) + " (" + strYear + "-" + Convert.ToDateTime(drmonth["Startdate"]).ToString("MM") + ")",
                                beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? drmonth["MachineID"].ToString() : viewType == "CellWiseView" ? drmonth["GroupID"].ToString() : viewType == "PlantwiseView" ? drmonth["PlantID"].ToString() : viewType == "ComponentwiseView" ? drmonth["ComponentID"].ToString() : drmonth["OperatorID"].ToString()) + " (" + strYear + ")"
                            });
                        }
                        i++;
                    }
                }
                //---------------------------Day Wise Start---------------------------------

                dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "DAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                foreach (var machine in machineData)
                {
                    foreach (var month in monthName)
                    {
                        chart.drilldown.Add(new DrildownSeries
                        {
                            name = machine,
                            id = machine + "-" + month,
                            data = new List<DrildownData>(),
                        });
                        DataRow[] results = null;
                        if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtDay.Select("Month = " + month + " AND MachineID ='" + machine + "'");
                        else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtDay.Select("Month = " + month + " AND PlantID ='" + machine + "'");
                        else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtDay.Select("Month = " + month + " AND GroupID ='" + machine + "'");
                        else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                            results = dtDay.Select("Month = " + month + " AND ComponentID ='" + machine + "'");
                        else
                            results = dtDay.Select("Month = " + month + " AND OperatorID ='" + machine + "'");

                        if (results != null)
                        {
                            foreach (DataRow dr in results)
                            {
                                chart.drilldown[i].data.Add(new DrildownData
                                {
                                    name = "Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                                    y = Convert.ToDecimal(dr["Parameter"].ToString()),
                                    drilldown = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + "/" + month + "/" + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                                    afterTitel = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + month + ")",// "-" + Convert.ToDateTime(dr["PDate"]).ToString("dd") +
                                    beforeTitle = paramText + " : " + (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "CellWiseView" ? dr["GroupID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + month + ")"
                                });
                            }
                            i++;
                        }
                    }
                }

                //-------------------------------------Shift Wise Start---------------------------------------------------------
                if (DataBaseAccess.ThirdLevelDrillDown)
                {
                    //date = string.Empty;

                    dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                    foreach (var machine in machineData)
                    {
                        foreach (var month in monthName)
                        {
                            date = strYear + "-" + month + "-" + "01";
                            // dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "DAY", param, componentId, employeeId, SortColumn, "");
                            DataRow[] results = null;
                            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = '" + month + "' AND MachineID ='" + machine + "'");//"MachineID = '" + machine + "'" ///"PDate = '" + date + "' AND MachineID ='" + machine + "'"
                            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = '" + month + "' AND PlantID ='" + machine + "'");
                            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = '" + month + "' AND GroupID ='" + machine + "'");
                            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = '" + month + "' AND ComponentID ='" + machine + "'");
                            else
                                results = dtDay.Select("Month = '" + month + "' AND OperatorID ='" + machine + "'");

                            foreach (DataRow day in results)
                            {
                                chart.drilldown.Add(new DrildownSeries
                                {
                                    id = machine + "/" + month + "/" + Convert.ToDateTime(day["PDate"]).ToString("dd"),
                                    name = machine,//+ "/" + strYear + "/" + month + "/" + Convert.ToDateTime(day["PDate"]).ToString("dd"),
                                                   //drilldown = machine + "/" + strYear + "/" + month + "/" + Convert.ToDateTime(day["PDate"]).ToString("dd"),
                                                   //afterTitel = machine + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd") + ")",
                                                   //beforeTitle = paramText + " : " + machine + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd") + " )",
                                    data = new List<DrildownData>(),
                                });
                                date = strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd");
                                // dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "SHIFT", param, componentId, employeeId, SortColumn, "");
                                DataRow[] results1 = null;
                                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                    results1 = dtShift.Select("PDate = '" + date + "' AND MachineID ='" + machine + "'");
                                else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                    results1 = dtShift.Select("PDate = '" + date + "' AND PlantID ='" + machine + "'");
                                else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                    results1 = dtShift.Select("PDate = '" + date + "' AND GroupID ='" + machine + "'");
                                else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                    results1 = dtShift.Select("PDate = '" + date + "' AND ComponentID ='" + machine + "'");
                                else
                                    results1 = dtShift.Select("PDate = '" + date + "' AND OperatorID ='" + machine + "'");

                                foreach (DataRow drshift in results1)
                                {
                                    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                    {
                                        chart.drilldown[i].data.Add(new DrildownData
                                        {
                                            name = drshift["Shift"].ToString(),
                                            y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                                            drilldown = machine + "/" + month + "/" + Convert.ToDateTime(drshift["PDate"]).ToString("dd") + "/" + drshift["Shift"].ToString(),
                                            afterTitel = machine + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd") + "-" + drshift["Shift"].ToString() + ")", //+ "/" + drshift["Shift"].ToString(),
                                                                                                                                                                                                   //beforeTitle = paramText + " : " + machine + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd") + " )",
                                        });//, id = machine + "/" + strYear + "/" + month + "/" + Convert.ToDateTime(drshift["PDate"]).ToString("dd") 
                                    }
                                    else
                                    {
                                        chart.drilldown[i].data.Add(new DrildownData
                                        {
                                            name = drshift["Shift"].ToString(),
                                            y = Convert.ToDecimal(drshift["Parameter"].ToString())
                                        });
                                    }
                                }
                                i++;
                            }
                        }
                    }
                }
                #endregion
            }
            ///--------------------End Chrome------------------
            return chart;
        }
        #endregion

        //protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindCellId(ddlPlantId.SelectedItem == null ? "" : ddlPlantId.SelectedItem.Text);
        //}
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Chart<Series_VDG> GetColumnChartDataFromSession()
        {
            Chart<Series_VDG> chart = new Chart<Series_VDG>();
            try
            {
                if (HttpContext.Current.Session["ChartDataForBackFun"] != null)
                {
                    List<Chart<Series_VDG>> chartList = HttpContext.Current.Session["ChartDataForBackFun"] as List<Chart<Series_VDG>>;
                    chartList.RemoveAt(chartList.Count - 1);
                    chart = chartList[chartList.Count - 1];
                    HttpContext.Current.Session["ChartDataForBackFun"] = chartList;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetColumnChartDataFromSession" + ex.ToString());
            }
            return chart;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetCellIdData(string PlantId)
        {
            List<string> lstCellId = new List<string>();
            try
            {
                lstCellId = BindCockpitView.ViewCellsToDisplay(PlantId == "Plant All" ? "" : PlantId);
                lstCellId.Insert(0, "All");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return lstCellId;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string getLicenseExpireDayFromSession()
        {
            string res = "";
            if (HttpContext.Current.Session["RemainingDaysForShanti"] != null)
            {
                res = HttpContext.Current.Session["RemainingDaysForShanti"].ToString();
            }
            return res;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static bool getQEVisibility()
        {
            bool isVisible = true;
            if (HttpContext.Current.Session["QEVisibility"] != null)
            {
                isVisible = HttpContext.Current.Session["QEVisibility"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false;
            }
            return isVisible;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Chart<Series_VDG> GetColumnDayChartData(string plantId, string strYear, string strMonth, string strDay, string param, string strShift, string componentId, string employeeId, string cellId, string SortColumn, string chartOrder, string paramText, string viewType, string machine, string currentParam, string nextParam, string selectedMonth, string selectedDay, string prevParam, string backBtnStatus, string columnColor)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool isCombined = BindCockpitView.IsChartCombined_OEEDAshboard();
            string CombinedCharts = isCombined ? BindCockpitView.GetCombinedChartNames_OEEDAshboard() : param;
            var distCharts = CombinedCharts.Split(',');
            if (!distCharts.Contains(param)) /*|| viewType == "ComponentwiseView" || viewType == "OperatorwiseView")*/
            {
                distCharts = new string[0];
                distCharts = new string[] { param };
            }
            if (distCharts.Length > 1)
                paramText = "Efficiency (%)";
            string date = string.Empty;
            string machinePlantEmp = "";
            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                machinePlantEmp = machine;
            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                machinePlantEmp = plantId;
            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                machinePlantEmp = componentId;
            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
            {
                machinePlantEmp = cellId;
            }
            else
                machinePlantEmp = employeeId;
            Chart<Series_VDG> chartData = null;
            chartData = new Chart<Series_VDG>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = paramText,
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };
            DataTable dtYear = new DataTable();
            DataTable dtMonth = new DataTable();
            DataTable dtDay = new DataTable();
            DataTable dtShift = new DataTable();
            DataTable dtHours = new DataTable();
            List<string> shiftData = new List<string>();
            List<string> machineData = new List<string>();
            List<string> monthData = new List<string>();

            string[] monthName = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };

            if (strShift.Equals("All", StringComparison.OrdinalIgnoreCase))
                strShift = "";
            if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
                plantId = "";
            if (componentId.Equals("All", StringComparison.OrdinalIgnoreCase))
                componentId = "";
            if (employeeId.Equals("All", StringComparison.OrdinalIgnoreCase))
                employeeId = "";
            if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase))
                cellId = "";

            #region "Month Condition------------"
            if (strMonth.Equals("Jan", StringComparison.OrdinalIgnoreCase))
                strMonth = "01";
            else if (strMonth.Equals("Feb", StringComparison.OrdinalIgnoreCase))
                strMonth = "02";
            else if (strMonth.Equals("Mar", StringComparison.OrdinalIgnoreCase))
                strMonth = "03";
            else if (strMonth.Equals("Apr", StringComparison.OrdinalIgnoreCase))
                strMonth = "04";
            else if (strMonth.Equals("May", StringComparison.OrdinalIgnoreCase))
                strMonth = "05";
            else if (strMonth.Equals("Jun", StringComparison.OrdinalIgnoreCase))
                strMonth = "06";
            else if (strMonth.Equals("Jul", StringComparison.OrdinalIgnoreCase))
                strMonth = "07";
            else if (strMonth.Equals("Aug", StringComparison.OrdinalIgnoreCase))
                strMonth = "08";
            else if (strMonth.Equals("Sep", StringComparison.OrdinalIgnoreCase))
                strMonth = "09";
            else if (strMonth.Equals("Oct", StringComparison.OrdinalIgnoreCase))
                strMonth = "10";
            else if (strMonth.Equals("Nov", StringComparison.OrdinalIgnoreCase))
                strMonth = "11";
            else if (strMonth.Equals("Dec", StringComparison.OrdinalIgnoreCase))
                strMonth = "12";
            if (selectedMonth.Equals("Jan", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "01";
            else if (selectedMonth.Equals("Feb", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "02";
            else if (selectedMonth.Equals("Mar", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "03";
            else if (selectedMonth.Equals("Apr", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "04";
            else if (selectedMonth.Equals("May", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "05";
            else if (selectedMonth.Equals("Jun", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "06";
            else if (selectedMonth.Equals("Jul", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "07";
            else if (selectedMonth.Equals("Aug", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "08";
            else if (selectedMonth.Equals("Sep", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "09";
            else if (selectedMonth.Equals("Oct", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "10";
            else if (selectedMonth.Equals("Nov", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "11";
            else if (selectedMonth.Equals("Dec", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "12";
            #endregion

            try
            {
                if (strYear != "" && strMonth != "" && strDay != "")
                {
                    #region "Day Wise Information-------------"
                    date = strYear + "-" + strMonth + "-" + strDay;
                    //-------------------------------Shift Wise Info-----------------------------------
                    chartData.series = new List<Series_VDG>();
                    Series_VDG charttdataaa = null;
                    foreach (var chartt in distCharts)
                    {
                        param = chartt;
                        if (SortColumn == "MachineId")
                            SortColumn = "OEffy";
                        if (nextParam == "shift")
                        {
                            charttdataaa = new Series_VDG();
                            charttdataaa.name = param;
                            charttdataaa.type = "column";
                            if (distCharts.Length == 1)
                            {
                                charttdataaa.colorByPoint = true;
                            }
                            charttdataaa.currentParam = "shift";
                            charttdataaa.nextParam = "";
                            charttdataaa.machine = machinePlantEmp;
                            charttdataaa.day = "";
                            charttdataaa.month = strMonth;
                            charttdataaa.btnVisible = "visible";
                            charttdataaa.previousParam = "BindFilterData";
                            //charttdataaa.color = columnColor;
                            charttdataaa.btnText = "Back to " + machinePlantEmp + "(" + selectedMonth + ")";
                            //charttdataaa.data = new List<Data>();
                            charttdataaa.Category = new List<string>();
                            charttdataaa.data = new List<double>();
                            charttdataaa.drilldown = new List<string>();
                            //Data dataa = null;
                            dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                            DataRow[] results = null;
                            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND MachineID ='" + machinePlantEmp + "' AND Day ='" + strDay + "'");
                            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND PlantID ='" + machinePlantEmp + "' AND Day ='" + strDay + "'");
                            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND ComponentID ='" + machinePlantEmp + "' AND Day ='" + strDay + "'");
                            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND GroupID ='" + machinePlantEmp + "' AND Day='" + strDay + "'");
                            else
                                results = dtShift.Select("Month = " + strMonth + " AND OperatorID ='" + machinePlantEmp + "' AND Day ='" + strDay + "'");

                            string title = "";
                            foreach (DataRow drshift in results)
                            {
                                if (!DataBaseAccess.HourlyLevelDrillDown)
                                {
                                    title = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : viewType == "CellWiseView" ? drshift["Groupid"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")";
                                    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                    {
                                        //dataa = new Data();
                                        //dataa.name = drshift["Shift"].ToString();
                                        //if (!Convert.IsDBNull(drshift["Parameter"]))
                                        //{
                                        //    dataa.y = Convert.ToInt32(Math.Round(Convert.ToDecimal(drshift["Parameter"].ToString()), 0));
                                        //}
                                        //dataa.drilldown = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : viewType == "CellWiseView" ? drshift["Groupid"].ToString() : drshift["OperatorID"].ToString()) + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString();
                                        //dataa.afterTitel = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : viewType == "CellWiseView" ? drshift["Groupid"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["Shift"].ToString() + ")";
                                        //charttdataaa.data.Add(dataa);
                                        charttdataaa.data.Add(Convert.IsDBNull(drshift["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(drshift["Parameter"].ToString()), 0)));
                                        charttdataaa.Category.Add(drshift["Shift"].ToString());
                                        charttdataaa.drilldown.Add((viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : viewType == "CellWiseView" ? drshift["Groupid"].ToString() : drshift["OperatorID"].ToString()) + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString());
                                    }
                                    else
                                    {
                                        //dataa = new Data();
                                        //dataa.name = drshift["Shift"].ToString();
                                        //if (!Convert.IsDBNull(drshift["Parameter"]))
                                        //{
                                        //    dataa.y = Convert.ToInt32(Math.Round(Convert.ToDecimal(drshift["Parameter"].ToString()), 0));
                                        //}
                                        //charttdataaa.data.Add(dataa);
                                        charttdataaa.data.Add(Convert.IsDBNull(drshift["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(drshift["Parameter"].ToString()), 0)));
                                        charttdataaa.Category.Add(drshift["Shift"].ToString());
                                    }
                                }
                                else
                                {
                                    //chart.drilldown[i].data.Add(new DrildownData
                                    //{
                                    //    name = drshift["Shift"].ToString(),
                                    //    y = Convert.ToDecimal(drshift["Parameter"].ToString()),
                                    //});
                                }

                            }
                            chartData.series.Add(charttdataaa);
                            if (title != "")
                            {
                                chartData.Title = paramText + " : " + title;
                            }
                            else
                            {
                                chartData.Title = paramText;
                            }
                        }
                    }
                    #endregion
                }
                else if (strYear != "" && strMonth != "")
                {
                    chartData.series = new List<Series_VDG>();
                    Series_VDG charttdataaa = null;
                    foreach (var chartt in distCharts)
                    {
                        param = chartt;
                        if (SortColumn == "MachineId")
                            SortColumn = "OEffy";
                        date = strYear + "-" + strMonth + "-" + "01";
                        if (nextParam == "day" || (prevParam == "day" && backBtnStatus == "comingFromBack"))
                        {
                            #region "day Wise Information ------------"
                            dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "DAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);
                            //dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "DAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);
                            DataRow[] results = null;
                            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + strMonth + " AND MachineID ='" + machinePlantEmp + "'");
                            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + strMonth + " AND PlantID ='" + machinePlantEmp + "'");
                            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + strMonth + " AND ComponentID ='" + machinePlantEmp + "'");
                            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + strMonth + " AND GroupID ='" + machinePlantEmp + "'");
                            else
                                results = dtDay.Select("Month = " + strMonth + " AND OperatorID ='" + machinePlantEmp + "'");
                            //chartData.series = new List<Series>();
                            charttdataaa = new Series_VDG();
                            charttdataaa.name = param;
                            charttdataaa.type = "column";
                            if (distCharts.Length == 1)
                            {
                                charttdataaa.colorByPoint = true;
                            }
                            charttdataaa.currentParam = "day";
                            charttdataaa.nextParam = "shift";
                            charttdataaa.machine = machinePlantEmp;
                            charttdataaa.day = "";
                            charttdataaa.month = strMonth;
                            charttdataaa.btnVisible = "visible";
                            charttdataaa.previousParam = "BindFilterData";
                            charttdataaa.btnText = "Back to " + paramText;
                            //charttdataaa.color = columnColor;
                            //charttdataaa.data = new List<Data>();
                            charttdataaa.data = new List<double>();
                            charttdataaa.Category = new List<string>();
                            //Data dataa = null;
                            string title = "";
                            foreach (DataRow dr in results)
                            {
                                //dataa = new Data();
                                //dataa.name = "Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd");
                                //if (!Convert.IsDBNull(dr["Parameter"]))
                                //{
                                //    dataa.y = Convert.ToInt32(Math.Round(Convert.ToDecimal(dr["Parameter"].ToString()), 0));
                                //}
                                title = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : viewType == "CellWiseView" ? dr["Groupid"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + strMonth + ")";
                                //charttdataaa.data.Add(dataa);
                                charttdataaa.data.Add(Convert.IsDBNull(dr["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(dr["Parameter"].ToString()), 0)));
                                charttdataaa.Category.Add("Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd"));
                            }
                            chartData.series.Add(charttdataaa);
                            if (title != "")
                            {
                                chartData.Title = paramText + " : " + title;
                            }
                            else
                            {
                                chartData.Title = paramText;
                            }

                            #endregion
                        }
                        else if (nextParam == "shift")
                        {
                            //--------------------------Shift Wise Information-----------------------------
                            monthData = (from w in monthData select w).Distinct().ToList();
                            dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);
                            //chartData.series = new List<Series>();
                            charttdataaa = new Series_VDG();
                            charttdataaa.name = param;
                            charttdataaa.type = "column";
                            if (distCharts.Length == 1)
                            {
                                charttdataaa.colorByPoint = true;
                            }
                            charttdataaa.currentParam = "shift";
                            charttdataaa.nextParam = "";
                            charttdataaa.machine = machinePlantEmp;
                            charttdataaa.day = selectedDay;
                            charttdataaa.month = strMonth;
                            charttdataaa.btnVisible = "visible";
                            charttdataaa.btnText = "Back to " + machinePlantEmp + "(" + selectedMonth + ")";
                            charttdataaa.previousParam = "day";
                            //charttdataaa.color = columnColor;
                            //charttdataaa.data = new List<Data>();
                            charttdataaa.data = new List<double>();
                            charttdataaa.Category = new List<string>();
                            charttdataaa.drilldown = new List<string>();
                            //Data dataa = null;
                            DataRow[] results = null;
                            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND MachineID ='" + machinePlantEmp + "' AND Day ='" + selectedDay + "'");
                            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND PlantID ='" + machinePlantEmp + "' AND Day ='" + selectedDay + "'");
                            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND ComponentID ='" + machinePlantEmp + "' AND Day ='" + selectedDay + "'");
                            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND GroupID ='" + machinePlantEmp + "' AND Day ='" + selectedDay + "'");
                            else
                                results = dtShift.Select("Month = " + strMonth + " AND OperatorID ='" + machinePlantEmp + "' AND Day ='" + selectedDay + "'");
                            string title = "";
                            foreach (DataRow drshift in results)
                            {

                                if (!DataBaseAccess.HourlyLevelDrillDown)
                                {
                                    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                    {
                                        //dataa = new Data();
                                        //dataa.name = drshift["Shift"].ToString();
                                        //if (!Convert.IsDBNull(drshift["Parameter"]))
                                        //{
                                        //    dataa.y = Convert.ToInt32(Math.Round(Convert.ToDecimal(drshift["Parameter"].ToString()), 0));
                                        //}
                                        //dataa.drilldown = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : viewType == "CellWiseView" ? drshift["Groupid"].ToString() : drshift["OperatorID"].ToString()) + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString();
                                        //dataa.afterTitel = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : viewType == "CellWiseView" ? drshift["Groupid"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["Shift"].ToString() + ")";
                                        //charttdataaa.data.Add(dataa);
                                        charttdataaa.data.Add(Convert.IsDBNull(drshift["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(drshift["Parameter"].ToString()), 0)));
                                        charttdataaa.Category.Add(drshift["Shift"].ToString());
                                        charttdataaa.drilldown .Add( (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : viewType == "CellWiseView" ? drshift["Groupid"].ToString() : drshift["OperatorID"].ToString()) + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString());
                                    }
                                    else
                                    {
                                        //dataa = new Data();
                                        //dataa.name = drshift["Shift"].ToString();
                                        //if (!Convert.IsDBNull(drshift["Parameter"]))
                                        //{
                                        //    dataa.y = Convert.ToInt32(Convert.ToDecimal(drshift["Parameter"].ToString()));
                                        //}
                                        //charttdataaa.data.Add(dataa);
                                        charttdataaa.data.Add(Convert.IsDBNull(drshift["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(drshift["Parameter"].ToString()), 0)));
                                        charttdataaa.Category.Add(drshift["Shift"].ToString());
                                    }
                                    title = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : viewType == "CellWiseView" ? drshift["Groupid"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")";
                                }
                                else
                                {
                                    //chart.drilldown[i].data.Add(new DrildownData
                                    //{
                                    //    name = drshift["Shift"].ToString(),
                                    //    y = Convert.ToDecimal(drshift["Parameter"].ToString())
                                    //});
                                }
                            }
                            chartData.series.Add(charttdataaa);
                            if (title != "")
                            {
                                chartData.Title = paramText + " : " + title;
                            }
                            else
                            {
                                chartData.Title = paramText;
                            }

                        }
                    }
                }
                else if (strYear != "")
                {
                    #region "Year Wise Information -----------"
                    date = strYear + "-" + "01" + "-" + "01";

                    //------------------------Year Wise Start--------------------------
                    if (SortColumn == "MachineId")
                        SortColumn = "OEffy";
                    string title = "";
                    if (nextParam == "month" || (prevParam == "month" && backBtnStatus == "comingFromBack"))
                    {
                        chartData.series = new List<Series_VDG>();
                        Series_VDG charttdataaa = null;
                        foreach (var chartt in distCharts)
                        {
                            param = chartt;
                            dtMonth = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "MONTH", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);
                            //---------------------------Month Wise Start----------------------------
                            //chartData.series = new List<Series>();
                            charttdataaa = new Series_VDG();
                            charttdataaa.name = param;
                            charttdataaa.type = "column";
                            if (distCharts.Length == 1)
                            {
                                charttdataaa.colorByPoint = true;
                            }
                            charttdataaa.currentParam = "month";
                            charttdataaa.nextParam = "day";
                            charttdataaa.machine = machinePlantEmp;
                            charttdataaa.day = "";
                            charttdataaa.month = "";
                            charttdataaa.btnVisible = "visible";
                            charttdataaa.btnText = "Back to " + paramText;
                            charttdataaa.previousParam = "BindFilterData";
                            //charttdataaa.color = columnColor;
                            //charttdataaa.data = new List<Data>();
                            charttdataaa.data = new List<double>();
                            charttdataaa.Category = new List<string>();
                            //Data dataa = null;
                            chartData.drilldown = new List<DrildownSeries>();

                            DataRow[] results = null;
                            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtMonth.Select("MachineID = '" + machinePlantEmp + "'");
                            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtMonth.Select("PlantID = '" + machinePlantEmp + "'");
                            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtMonth.Select("ComponentID = '" + machinePlantEmp + "'");
                            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtMonth.Select("Groupid = '" + machinePlantEmp + "'");
                            else
                                results = dtMonth.Select("OperatorID = '" + machinePlantEmp + "'");

                            if (results != null)
                            {
                                foreach (DataRow drmonth in results)
                                {
                                    //dataa = new Data();
                                    //dataa.name = drmonth["NameOftheMonth"].ToString(); ;
                                    //if (!Convert.IsDBNull(drmonth["Parameter"]))
                                    //{
                                    //    dataa.y = Convert.ToInt32(Math.Round(Convert.ToDecimal(drmonth["Parameter"].ToString()), 0));
                                    //}
                                    title = (viewType == "MachinewiseView" ? drmonth["MachineID"].ToString() : viewType == "PlantwiseView" ? drmonth["PlantID"].ToString() : viewType == "ComponentwiseView" ? drmonth["ComponentID"].ToString() : viewType == "CellWiseView" ? drmonth["Groupid"].ToString() : drmonth["OperatorID"].ToString()) + " (" + strYear + ")";
                                    //charttdataaa.data.Add(dataa);
                                    charttdataaa.data.Add(Convert.IsDBNull(drmonth["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(drmonth["Parameter"].ToString()), 0)));
                                    charttdataaa.Category.Add(drmonth["NameOftheMonth"].ToString());
                                }
                            }
                            chartData.series.Add(charttdataaa);
                            if (title != "")
                            {
                                chartData.Title = paramText + " : " + title;
                            }
                            else
                            {
                                chartData.Title = paramText;
                            }
                        }
                    }
                    else
                    if (nextParam == "day" || (prevParam == "day" && backBtnStatus == "comingFromBack"))
                    {
                        //---------------------------Day Wise Start---------------------------------
                        chartData.series = new List<Series_VDG>();
                        Series_VDG charttdataaa = null;
                        foreach (var chartt in distCharts)
                        {
                            param = chartt;
                            dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "DAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);
                            string nextParam1 = "";
                            if (DataBaseAccess.ThirdLevelDrillDown)
                            {
                                nextParam1 = "shift";
                            }
                            charttdataaa = new Series_VDG();
                            //charttdataaa.name = machinePlantEmp + "(" + selectedMonth + ")";
                            charttdataaa.name = param;
                            charttdataaa.type = "column";
                            //charttdataaa.colorByPoint = false;
                            charttdataaa.currentParam = "day";
                            charttdataaa.nextParam = nextParam1;
                            charttdataaa.machine = machinePlantEmp;
                            charttdataaa.day = "";
                            charttdataaa.month = selectedMonth;
                            charttdataaa.btnVisible = "visible";
                            charttdataaa.btnText = "Back to " + machinePlantEmp;
                            charttdataaa.previousParam = "month";
                            //charttdataaa.color = columnColor;
                            //charttdataaa.data = new List<Data>();
                            charttdataaa.data = new List<double>();
                            charttdataaa.Category = new List<string>();
                            charttdataaa.drilldown = new List<string>();
                            //Data dataa = null;
                            DataRow[] results = null;
                            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + selectedMonth + " AND MachineID ='" + machinePlantEmp + "'");
                            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + selectedMonth + " AND PlantID ='" + machinePlantEmp + "'");
                            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + selectedMonth + " AND ComponentID ='" + machinePlantEmp + "'");
                            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + selectedMonth + " AND GroupID ='" + machinePlantEmp + "'");
                            else
                                results = dtDay.Select("Month = " + selectedMonth + " AND OperatorID ='" + machinePlantEmp + "'");

                            if (results != null)
                            {
                                foreach (DataRow dr in results)
                                {
                                    string dillValue = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : viewType == "CellWiseView" ? dr["Groupid"].ToString() : dr["OperatorID"].ToString()) + "/" + selectedMonth + "/" + Convert.ToDateTime(dr["PDate"]).ToString("dd");
                                    if (DataBaseAccess.ThirdLevelDrillDown) //kkk
                                    {
                                        dillValue = null;
                                    }
                                    //dataa = new Data();
                                    //dataa.name = "Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd");
                                    //dataa.y = Convert.ToInt32(Math.Round(Convert.ToDecimal(dr["Parameter"].ToString()), 0));
                                    //dataa.drilldown = dillValue;
                                    //dataa.afterTitel = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : viewType == "CellWiseView" ? dr["Groupid"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + selectedMonth + ")";

                                    title = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : viewType == "CellWiseView" ? dr["Groupid"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + selectedMonth + ")";
                                    //charttdataaa.data.Add(dataa);
                                    charttdataaa.data.Add(Convert.IsDBNull(dr["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(dr["Parameter"].ToString()), 0)));
                                    charttdataaa.Category.Add("Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd"));
                                    charttdataaa.drilldown.Add(dillValue);
                                }
                                chartData.series.Add(charttdataaa);
                            }
                            if (title != "")
                            {
                                chartData.Title = paramText + " : " + title;
                            }
                            else
                            {
                                chartData.Title = paramText;
                            }
                        }
                    }
                    else
                    //--------------------------------------------------Shift Wise Start---------------------------------------------
                    if (nextParam == "shift")
                    {
                        if (DataBaseAccess.ThirdLevelDrillDown)
                        {
                            //---------------------------Day Wise Start---------------------------------
                            // dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "DAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);
                            chartData.series = new List<Series_VDG>();
                            Series_VDG charttdataaa = null;
                            foreach (var chartt in distCharts)
                            {
                                param = chartt;
                                dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);
                                //chartData.series = new List<Series>();
                                charttdataaa = new Series_VDG();
                                charttdataaa.name = param;
                                charttdataaa.type = "column";
                                //charttdataaa.colorByPoint = false;
                                charttdataaa.currentParam = "shift";
                                charttdataaa.nextParam = "";
                                charttdataaa.machine = machinePlantEmp;
                                charttdataaa.day = "";
                                charttdataaa.month = selectedMonth;
                                charttdataaa.btnVisible = "visible";
                                charttdataaa.btnText = "Back to " + machinePlantEmp + "(" + selectedMonth + ")";
                                charttdataaa.previousParam = "day";
                                //charttdataaa.color = columnColor;
                                //charttdataaa.data = new List<Data>();
                                charttdataaa.data = new List<double>();
                                charttdataaa.Category = new List<string>();
                                //Data dataa = null;
                                date = strYear + "-" + selectedMonth + "-" + "01";
                                date = strYear + "-" + selectedMonth + "-" + selectedDay;
                                DataRow[] results1 = null;
                                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                    results1 = dtShift.Select("PDate = '" + date + "' AND MachineID ='" + machinePlantEmp + "'");
                                else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                    results1 = dtShift.Select("PDate = '" + date + "' AND PlantID ='" + machinePlantEmp + "'");
                                else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                    results1 = dtShift.Select("PDate = '" + date + "' AND ComponentID ='" + machinePlantEmp + "'");
                                else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                    results1 = dtShift.Select("PDate = '" + date + "' AND Groupid ='" + machinePlantEmp + "'");
                                else
                                    results1 = dtShift.Select("PDate = '" + date + "' AND OperatorID ='" + machinePlantEmp + "'");
                                string stitle = "";
                                foreach (DataRow drshift in results1)
                                {
                                    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                    {
                                        //dataa = new Data();
                                        //dataa.name = drshift["Shift"].ToString();
                                        //dataa.y = Convert.ToInt32(Math.Round(Convert.ToDecimal(drshift["Parameter"].ToString()), 0));
                                        //dataa.drilldown = machinePlantEmp + "/" + selectedMonth + "/" + Convert.ToDateTime(drshift["PDate"]).ToString("dd") + "/" + drshift["Shift"].ToString();
                                        //dataa.afterTitel = machinePlantEmp + " (" + strYear + "-" + selectedMonth + "-" + selectedDay + "-" + drshift["Shift"].ToString() + ")";
                                        //charttdataaa.data.Add(dataa);
                                        charttdataaa.data.Add(Convert.IsDBNull(drshift["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(drshift["Parameter"].ToString()), 0)));
                                        charttdataaa.Category.Add(drshift["Shift"].ToString());
                                        charttdataaa.drilldown.Add(machinePlantEmp + "/" + selectedMonth + "/" + Convert.ToDateTime(drshift["PDate"]).ToString("dd") + "/" + drshift["Shift"].ToString());
                                    }
                                    else
                                    {
                                        //dataa = new Data();
                                        //dataa.name = drshift["Shift"].ToString();
                                        //dataa.y = Convert.ToInt32(Convert.ToDecimal(drshift["Parameter"].ToString()));
                                        //charttdataaa.data.Add(dataa);
                                        charttdataaa.data.Add(Convert.IsDBNull(drshift["Parameter"]) ? 0 : Convert.ToInt32(Math.Round(Convert.ToDecimal(drshift["Parameter"].ToString()), 0)));
                                        charttdataaa.Category.Add(drshift["Shift"].ToString());
                                    }
                                    stitle = machinePlantEmp + " (" + strYear + "-" + selectedMonth + ")";
                                }
                                chartData.series.Add(charttdataaa);
                                if (stitle != "")
                                {
                                    chartData.Title = paramText + " : " + stitle;
                                }
                                else
                                {
                                    chartData.Title = paramText;
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
                //throw ex;
            }
            stopwatch.Stop();
            Logger.WriteDebugLog("BindDashboardChartData : " + stopwatch.Elapsed.TotalSeconds);
            //kkkk
            if (HttpContext.Current.Session["ChartDataForBackFun"] != null)
            {
                List<Chart<Series_VDG>> chartList = HttpContext.Current.Session["ChartDataForBackFun"] as List<Chart<Series_VDG>>;
                chartList.Add(chartData);
                HttpContext.Current.Session["ChartDataForBackFun"] = chartList;
            }
            return chartData;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Chart<Series_VDG> GetLineDrillDownChartData(string plantId, string strYear, string strMonth, string strDay, string param, string strShift, string componentId, string employeeId, string cellId, string SortColumn, string chartOrder, string paramText, string viewType, string machine, string currentParam, string nextParam, string selectedMonth, string selectedDay, string prevParam, string backBtnStatus, string columnColor)
        {
            bool isCombined = BindCockpitView.IsChartCombined_OEEDAshboard();
            string CombinedCharts = isCombined ? BindCockpitView.GetCombinedChartNames_OEEDAshboard() : param;
            var distCharts = CombinedCharts.Split(',');
            if (!distCharts.Contains(param))// || viewType == "ComponentwiseView" || viewType == "OperatorwiseView")
            {
                distCharts = new string[0];
                distCharts = new string[] { param };
            }
            if (distCharts.Length > 1)
                paramText = "Efficiency (%)";
            string date = string.Empty;
            string machinePlantEmp = "";
            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                machinePlantEmp = machine;
            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                machinePlantEmp = plantId;
            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                machinePlantEmp = componentId;
            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
            {
                machinePlantEmp = cellId;
            }
            else
                machinePlantEmp = employeeId;
            Chart<Series_VDG> chartData = null;
            chartData = new Chart<Series_VDG>
            {
                Title = isCombined ? "Efficiency (%)" : paramText,
                Subtitle = "SubTitle",
                XAxisTitle = paramText,
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };
            DataTable dtYear = new DataTable();
            DataTable dtMonth = new DataTable();
            DataTable dtDay = new DataTable();
            DataTable dtShift = new DataTable();
            DataTable dtHours = new DataTable();
            List<string> shiftData = new List<string>();
            List<string> machineData = new List<string>();
            List<string> monthData = new List<string>();
            string[] monthName = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            if (strShift.Equals("All", StringComparison.OrdinalIgnoreCase))
                strShift = "";
            if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
                plantId = "";
            if (componentId.Equals("All", StringComparison.OrdinalIgnoreCase))
                componentId = "";
            if (employeeId.Equals("All", StringComparison.OrdinalIgnoreCase))
                employeeId = "";
            if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase))
                cellId = "";

            #region "Month Condition------------"
            if (strMonth.Equals("Jan", StringComparison.OrdinalIgnoreCase))
                strMonth = "01";
            else if (strMonth.Equals("Feb", StringComparison.OrdinalIgnoreCase))
                strMonth = "02";
            else if (strMonth.Equals("Mar", StringComparison.OrdinalIgnoreCase))
                strMonth = "03";
            else if (strMonth.Equals("Apr", StringComparison.OrdinalIgnoreCase))
                strMonth = "04";
            else if (strMonth.Equals("May", StringComparison.OrdinalIgnoreCase))
                strMonth = "05";
            else if (strMonth.Equals("Jun", StringComparison.OrdinalIgnoreCase))
                strMonth = "06";
            else if (strMonth.Equals("Jul", StringComparison.OrdinalIgnoreCase))
                strMonth = "07";
            else if (strMonth.Equals("Aug", StringComparison.OrdinalIgnoreCase))
                strMonth = "08";
            else if (strMonth.Equals("Sep", StringComparison.OrdinalIgnoreCase))
                strMonth = "09";
            else if (strMonth.Equals("Oct", StringComparison.OrdinalIgnoreCase))
                strMonth = "10";
            else if (strMonth.Equals("Nov", StringComparison.OrdinalIgnoreCase))
                strMonth = "11";
            else if (strMonth.Equals("Dec", StringComparison.OrdinalIgnoreCase))
                strMonth = "12";

            if (selectedMonth.Equals("Jan", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "01";
            else if (selectedMonth.Equals("Feb", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "02";
            else if (selectedMonth.Equals("Mar", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "03";
            else if (selectedMonth.Equals("Apr", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "04";
            else if (selectedMonth.Equals("May", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "05";
            else if (selectedMonth.Equals("Jun", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "06";
            else if (selectedMonth.Equals("Jul", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "07";
            else if (selectedMonth.Equals("Aug", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "08";
            else if (selectedMonth.Equals("Sep", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "09";
            else if (selectedMonth.Equals("Oct", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "10";
            else if (selectedMonth.Equals("Nov", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "11";
            else if (selectedMonth.Equals("Dec", StringComparison.OrdinalIgnoreCase))
                selectedMonth = "12";
            #endregion
            try
            {
                if (strYear != "" && strMonth != "" && strDay != "")
                {
                    #region "Day Wise Information-------------"
                    date = strYear + "-" + strMonth + "-" + strDay;

                    //-------------------------------Shift Wise Info-----------------------------------
                    if (SortColumn == "MachineId")
                        SortColumn = "OEffy";
                    if (nextParam == "shift")
                    {
                        //chart.series = new List<Series>();
                        chartData.series = new List<Series_VDG>();
                        Series_VDG charttdataaa = null;
                        foreach (var chartt in distCharts)
                        {
                            param = chartt;
                            charttdataaa = new Series_VDG();
                            charttdataaa.name = machine;
                            charttdataaa.type = "line";
                            charttdataaa.currentParam = "shift";
                            charttdataaa.nextParam = "";
                            charttdataaa.machine = machinePlantEmp;
                            charttdataaa.day = "";
                            charttdataaa.month = strMonth;
                            charttdataaa.btnVisible = "visible";
                            charttdataaa.btnText = "Back to " + paramText;
                            charttdataaa.previousParam = "";
                            //charttdataaa.data = new List<Data>();
                            charttdataaa.data = new List<double>();
                            charttdataaa.Category = new List<string>();
                            charttdataaa.drilldown = new List<string>();
                            //Data dataa = new Data();
                            dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                            DataRow[] results = null;
                            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND MachineID ='" + machinePlantEmp + "' AND Day ='" + strDay + "'");
                            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND PlantID ='" + machinePlantEmp + "' AND Day ='" + strDay + "'");
                            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND ComponentID ='" + machinePlantEmp + "' AND Day ='" + strDay + "'");
                            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND GroupID ='" + machinePlantEmp + "' AND DAY='" + strDay + "'");
                            else
                                results = dtShift.Select("Month = " + strMonth + " AND OperatorID ='" + machinePlantEmp + "' AND Day ='" + strDay + "'");

                            string title = "";
                            foreach (DataRow drshift in results)
                            {
                                if (!DataBaseAccess.HourlyLevelDrillDown)
                                {
                                    title = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : viewType == "CellWiseView" ? drshift["Groupid"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")";
                                    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                    {
                                        //dataa = new Data();
                                        //dataa.name = drshift["Shift"].ToString();
                                        //dataa.y = Convert.ToInt32(Convert.ToDecimal(drshift["Parameter"].ToString()));
                                        //dataa.drilldown = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : viewType == "CellWiseView" ? drshift["Groupid"].ToString() : drshift["OperatorID"].ToString()) + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString();
                                        //dataa.afterTitel = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : viewType == "CellWiseView" ? drshift["Groupid"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["Shift"].ToString() + ")";
                                        //charttdataaa.data.Add(dataa);
                                        charttdataaa.data.Add(Convert.IsDBNull(drshift["Parameter"]) ? 0 : Convert.ToInt32(Convert.ToDecimal(drshift["Parameter"].ToString())));
                                        charttdataaa.Category.Add(drshift["Shift"].ToString());
                                        charttdataaa.drilldown.Add((viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : viewType == "CellWiseView" ? drshift["Groupid"].ToString() : drshift["OperatorID"].ToString()) + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString());
                                    }
                                    else
                                    {
                                        //dataa = new Data();
                                        //dataa.name = drshift["Shift"].ToString();
                                        //dataa.y = Convert.ToInt32(Convert.ToDecimal(drshift["Parameter"].ToString()));
                                        //charttdataaa.data.Add(dataa);
                                        charttdataaa.data.Add(Convert.IsDBNull(drshift["Parameter"]) ? 0 : Convert.ToInt32(Convert.ToDecimal(drshift["Parameter"].ToString())));
                                        charttdataaa.Category.Add(drshift["Shift"].ToString());
                                    }
                                    title = machinePlantEmp + " (" + strYear + "-" + selectedMonth + ")";
                                }
                                else
                                {
                                    //chart.drilldown[i].data.Add(new DrildownData
                                    //{
                                    //    name = drshift["Shift"].ToString(),
                                    //    y = Convert.ToDecimal(drshift["Parameter"].ToString())
                                    //});
                                }
                            }
                            chartData.series.Add(charttdataaa);
                            if (title != "")
                            {
                                chartData.Title = paramText + " : " + title;
                            }
                            else
                            {
                                chartData.Title = paramText;
                            }
                        }
                    }
                    #endregion
                }
                else if (strYear != "" && strMonth != "")
                {
                    #region "month Wise Information ------------"
                    date = strYear + "-" + strMonth + "-" + "01";
                    chartData.series = new List<Series_VDG>();
                    Series_VDG charttdataaa = null;
                    foreach (var chartt in distCharts)
                    {
                        param = chartt;
                        //---------------------------Day Wise Start---------------------------------
                        if (SortColumn == "MachineId")
                            SortColumn = "OEffy";
                        if (nextParam == "day" || (prevParam == "day" && backBtnStatus == "comingFromBack"))
                        {
                            dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, "", "DAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);
                            //dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "DAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);
                            //chartData.series = new List<Series>();
                            charttdataaa = new Series_VDG();
                            charttdataaa.name = param;
                            charttdataaa.type = "line";
                            charttdataaa.currentParam = "day";
                            charttdataaa.nextParam = "shift";
                            charttdataaa.machine = machinePlantEmp;
                            charttdataaa.day = "";
                            charttdataaa.month = strMonth;
                            charttdataaa.btnVisible = "visible";
                            charttdataaa.btnText = "Back to " + paramText;
                            charttdataaa.previousParam = "BindFilterData";
                            //charttdataaa.data = new List<Data>();
                            charttdataaa.data = new List<double>();
                            charttdataaa.Category = new List<string>();
                            //charttdataaa.drilldown = new List<string>();
                            //Data dataa = new Data();
                            DataRow[] results = null;
                            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + strMonth + " AND MachineID ='" + machinePlantEmp + "'");
                            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + strMonth + " AND PlantID ='" + machinePlantEmp + "'");
                            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + strMonth + " AND ComponentID ='" + machinePlantEmp + "'");
                            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + strMonth + " AND GroupID ='" + machinePlantEmp + "'");
                            else
                                results = dtDay.Select("Month = " + strMonth + " AND OperatorID ='" + machinePlantEmp + "'");
                            string title = "";
                            foreach (DataRow dr in results)
                            {
                                //dataa = new Data();
                                //dataa.name = "Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd");
                                //dataa.y = Convert.ToInt32(Convert.ToDecimal(dr["Parameter"].ToString()));
                                title = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : viewType == "CellWiseView" ? dr["Groupid"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + strMonth + ")";
                                //charttdataaa.data.Add(dataa);
                                charttdataaa.data.Add(Convert.IsDBNull(dr["Parameter"]) ? 0 : Convert.ToInt32(Convert.ToDecimal(dr["Parameter"].ToString())));
                                charttdataaa.Category.Add("Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd"));
                            }
                            chartData.series.Add(charttdataaa);
                            if (title != "")
                            {
                                chartData.Title = paramText + " : " + title;
                            }
                            else
                            {
                                chartData.Title = paramText;
                            }
                        }
                        else if (nextParam == "shift")
                        {

                            //--------------------------Shift Wise Information-----------------------------
                            dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);
                            //chart.series = new List<Series>();
                            charttdataaa = new Series_VDG();
                            charttdataaa.name = param;
                            charttdataaa.type = "line";
                            charttdataaa.currentParam = "shift";
                            charttdataaa.nextParam = "";
                            charttdataaa.machine = machine;
                            charttdataaa.day = selectedDay;
                            charttdataaa.month = strMonth;
                            charttdataaa.btnVisible = "visible";
                            charttdataaa.btnText = "Back to " + machine;
                            charttdataaa.previousParam = "day";
                            //charttdataaa.data = new List<Data>();
                            charttdataaa.data = new List<double>();
                            charttdataaa.Category = new List<string>();
                            charttdataaa.drilldown = new List<string>();
                            //Data dataa = new Data();
                            DataRow[] results = null;
                            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND MachineID ='" + machinePlantEmp + "' AND Day ='" + selectedDay + "'");
                            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND PlantID ='" + machinePlantEmp + "' AND Day ='" + selectedDay + "'");
                            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND ComponentID ='" + machinePlantEmp + "' AND Day ='" + selectedDay + "'");
                            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtShift.Select("Month = " + strMonth + " AND GroupID ='" + machinePlantEmp + "' AND DAY='" + selectedDay + "'");
                            else
                                results = dtShift.Select("Month = " + strMonth + " AND OperatorID ='" + machinePlantEmp + "' AND Day ='" + selectedDay + "'");
                            string title = "";
                            foreach (DataRow drshift in results)
                            {
                                if (!DataBaseAccess.HourlyLevelDrillDown)
                                {
                                    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                    {
                                        //dataa = new Data();
                                        //dataa.name = drshift["Shift"].ToString();
                                        //dataa.y = Convert.ToInt32(Convert.ToDecimal(drshift["Parameter"].ToString()));
                                        //dataa.drilldown = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : viewType == "CellWiseView" ? drshift["Groupid"].ToString() : drshift["OperatorID"].ToString()) + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString();
                                        //dataa.afterTitel = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : viewType == "CellWiseView" ? drshift["Groupid"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["Shift"].ToString() + ")";
                                        //charttdataaa.data.Add(dataa);
                                        charttdataaa.data.Add(Convert.IsDBNull(drshift["Parameter"]) ? 0 : Convert.ToInt32(Convert.ToDecimal(drshift["Parameter"].ToString())));
                                        charttdataaa.Category.Add(drshift["Shift"].ToString());
                                        charttdataaa.drilldown.Add((viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : viewType == "CellWiseView" ? drshift["Groupid"].ToString() : drshift["OperatorID"].ToString()) + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString());
                                    }
                                    else
                                    {
                                        //dataa = new Data();
                                        //dataa.name = drshift["Shift"].ToString();
                                        //dataa.y = Convert.ToInt32(Convert.ToDecimal(drshift["Parameter"].ToString()));
                                        //charttdataaa.data.Add(dataa);
                                        charttdataaa.data.Add(Convert.IsDBNull(drshift["Parameter"]) ? 0 : Convert.ToInt32(Convert.ToDecimal(drshift["Parameter"].ToString())));
                                        charttdataaa.Category.Add(drshift["Shift"].ToString());
                                    }
                                    title = (viewType == "MachinewiseView" ? drshift["MachineID"].ToString() : viewType == "PlantwiseView" ? drshift["PlantID"].ToString() : viewType == "ComponentwiseView" ? drshift["ComponentID"].ToString() : viewType == "CellWiseView" ? drshift["Groupid"].ToString() : drshift["OperatorID"].ToString()) + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")";
                                }
                                else
                                {
                                    //chart.drilldown[i].data.Add(new DrildownData
                                    //{
                                    //    name = drshift["Shift"].ToString(),
                                    //    y = Convert.ToDecimal(drshift["Parameter"].ToString())
                                    //});
                                }
                            }
                            chartData.series.Add(charttdataaa);
                            if (title != "")
                            {
                                chartData.Title = paramText + " : " + title;
                            }
                            else
                            {
                                chartData.Title = paramText;
                            }
                        }
                    }
                    #endregion
                }
                else if (strYear != "")
                {
                    #region "Year Wise Information -----------"
                    date = strYear + "-" + "01" + "-" + "01";
                    #endregion
                    #region---------------------------Month Wise Start----------------------------
                    if (SortColumn == "MachineId")
                        SortColumn = "OEffy";
                    string title = "";
                    chartData.series = new List<Series_VDG>();
                    Series_VDG charttdataaa = null;
                    foreach (var chartt in distCharts)
                    {
                        param = chartt;
                        if (nextParam == "month" || (prevParam == "month" && backBtnStatus == "comingFromBack"))
                        {
                            dtMonth = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "MONTH", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);

                            charttdataaa = new Series_VDG();
                            charttdataaa.name = param;
                            charttdataaa.type = "line";
                            charttdataaa.currentParam = "month";
                            charttdataaa.nextParam = "day";
                            charttdataaa.machine = machinePlantEmp;
                            charttdataaa.day = "";
                            charttdataaa.month = "";
                            charttdataaa.btnVisible = "visible";
                            charttdataaa.btnText = "Back to " + paramText;
                            charttdataaa.previousParam = "BindFilterData";
                            //charttdataaa.data = new List<Data>();
                            charttdataaa.data = new List<double>();
                            charttdataaa.Category = new List<string>();
                            //charttdataaa.drilldown = new List<string>();
                            //Data dataa = new Data();
                            DataRow[] results = null;
                            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtMonth.Select("MachineID = '" + machinePlantEmp + "'");
                            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtMonth.Select("PlantID = '" + machinePlantEmp + "'");
                            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtMonth.Select("ComponentID = '" + machinePlantEmp + "'");
                            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtMonth.Select("GroupID ='" + machinePlantEmp + "'");
                            else
                                results = dtMonth.Select("OperatorID = '" + machinePlantEmp + "'");
                            if (results != null)
                            {
                                foreach (DataRow drmonth in results)
                                {
                                    //dataa = new Data();
                                    //dataa.name = drmonth["NameOftheMonth"].ToString();
                                    //dataa.y = Convert.ToInt32(Convert.ToDecimal(drmonth["Parameter"].ToString()));
                                    //charttdataaa.data.Add(dataa);
                                    charttdataaa.data.Add(Convert.IsDBNull(drmonth["Parameter"]) ? 0 : Convert.ToInt32(Convert.ToDecimal(drmonth["Parameter"].ToString())));
                                    charttdataaa.Category.Add(drmonth["NameOftheMonth"].ToString());
                                    title = (viewType == "MachinewiseView" ? drmonth["MachineID"].ToString() : viewType == "PlantwiseView" ? drmonth["PlantID"].ToString() : viewType == "ComponentwiseView" ? drmonth["ComponentID"].ToString() : viewType == "CellWiseView" ? drmonth["Groupid"].ToString() : drmonth["OperatorID"].ToString()) + " (" + strYear + ")";
                                }
                                chartData.series.Add(charttdataaa);
                            }
                            if (title != "")
                            {
                                chartData.Title = paramText + " : " + title;
                            }
                            else
                            {
                                chartData.Title = paramText;
                            }
                        }
                        else if (nextParam == "day" || (prevParam == "day" && backBtnStatus == "comingFromBack"))
                        {
                            //---------------------------Day Wise Start---------------------------------

                            dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "DAY", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);
                            string nextParam1 = "";
                            if (DataBaseAccess.ThirdLevelDrillDown)
                            {
                                nextParam1 = "shift";
                            }
                            charttdataaa = new Series_VDG();
                            //charttdataaa.name = machine + "(" + selectedMonth + ")";
                            charttdataaa.name = param;
                            charttdataaa.type = "line";
                            charttdataaa.currentParam = "day";
                            charttdataaa.nextParam = nextParam1;
                            charttdataaa.machine = machinePlantEmp;
                            charttdataaa.day = "";
                            charttdataaa.month = selectedMonth;
                            charttdataaa.btnVisible = "visible";
                            charttdataaa.btnText = "Back to " + machine;
                            charttdataaa.previousParam = "month";
                            //charttdataaa.data = new List<Data>();
                            charttdataaa.data = new List<double>();
                            charttdataaa.Category = new List<string>();
                            charttdataaa.drilldown = new List<string>();
                            //Data dataa = new Data();

                            DataRow[] results = null;
                            if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + selectedMonth + " AND MachineID ='" + machinePlantEmp + "'");
                            else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + selectedMonth + " AND PlantID ='" + machinePlantEmp + "'");
                            else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + selectedMonth + " AND ComponentID ='" + machinePlantEmp + "'");
                            else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                results = dtDay.Select("Month = " + selectedMonth + " AND GroupID ='" + machinePlantEmp + "'");
                            else
                                results = dtDay.Select("Month = " + selectedMonth + " AND OperatorID ='" + machinePlantEmp + "'");

                            if (results != null)
                            {
                                foreach (DataRow dr in results)
                                {
                                    //dataa = new Data();

                                    string dillValue = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : viewType == "CellWiseView" ? dr["Groupid"].ToString() : dr["OperatorID"].ToString()) + "/" + selectedMonth + "/" + Convert.ToDateTime(dr["PDate"]).ToString("dd");
                                    if (DataBaseAccess.ThirdLevelDrillDown) //kkk
                                    {
                                        dillValue = null;
                                    }
                                    //dataa.name = "Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd");
                                    //dataa.y = Convert.ToInt32(Convert.ToDecimal(dr["Parameter"].ToString()));
                                    //dataa.drilldown = dillValue;
                                    //dataa.afterTitel = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : viewType == "CellWiseView" ? dr["Groupid"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + selectedMonth + ")";
                                    title = (viewType == "MachinewiseView" ? dr["MachineID"].ToString() : viewType == "PlantwiseView" ? dr["PlantID"].ToString() : viewType == "ComponentwiseView" ? dr["ComponentID"].ToString() : viewType == "CellWiseView" ? dr["Groupid"].ToString() : dr["OperatorID"].ToString()) + " (" + strYear + "-" + selectedMonth + ")";
                                    //charttdataaa.data.Add(dataa);
                                    charttdataaa.data.Add(Convert.IsDBNull(dr["Parameter"]) ? 0 : Convert.ToInt32(Convert.ToDecimal(dr["Parameter"].ToString())));
                                    charttdataaa.Category.Add("Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd"));
                                    charttdataaa.drilldown.Add(dillValue);
                                }
                                chartData.series.Add(charttdataaa);
                            }
                            if (title != "")
                            {
                                chartData.Title = paramText + " : " + title;
                            }
                            else
                            {
                                chartData.Title = paramText;
                            }
                        }
                        else if (nextParam == "shift")
                        {

                            #region-------------------------------------Shift Wise Start---------------------------------------------------------
                            if (DataBaseAccess.ThirdLevelDrillDown)
                            {
                                //date = string.Empty;

                                dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "SHIFT", param, componentId, employeeId, cellId, SortColumn, chartOrder, viewType);
                                charttdataaa = new Series_VDG();
                                charttdataaa.name = machine;
                                charttdataaa.type = "line";
                                charttdataaa.currentParam = "shift";
                                charttdataaa.nextParam = "";
                                charttdataaa.machine = machinePlantEmp;
                                charttdataaa.day = "";
                                charttdataaa.month = selectedMonth;
                                charttdataaa.btnVisible = "visible";
                                charttdataaa.btnText = "Back to " + machinePlantEmp + "(" + selectedMonth + ")";
                                charttdataaa.previousParam = "day";
                                //charttdataaa.data = new List<Data>();
                                charttdataaa.data = new List<double>();
                                charttdataaa.Category = new List<string>();
                                charttdataaa.drilldown = new List<string>();
                                //Data dataa = new Data();

                                date = strYear + "-" + selectedMonth + "-" + selectedDay;
                                // dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "SHIFT", param, componentId, employeeId, SortColumn, "");
                                DataRow[] results1 = null;
                                if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                    results1 = dtShift.Select("PDate = '" + date + "' AND MachineID ='" + machinePlantEmp + "'");
                                else if (viewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                                    results1 = dtShift.Select("PDate = '" + date + "' AND PlantID ='" + machinePlantEmp + "'");
                                else if (viewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                                    results1 = dtShift.Select("PDate = '" + date + "' AND ComponentID ='" + machinePlantEmp + "'");
                                else if (viewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                                    results1 = dtShift.Select("PDate = " + date + " AND GroupID ='" + machinePlantEmp + "'");
                                else
                                    results1 = dtShift.Select("PDate = '" + date + "' AND OperatorID ='" + machinePlantEmp + "'");

                                foreach (DataRow drshift in results1)
                                {
                                    if (viewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                                    {
                                        //dataa = new Data();
                                        //dataa.name = drshift["Shift"].ToString();
                                        //dataa.y = Convert.ToInt32(Convert.ToDecimal(drshift["Parameter"].ToString()));
                                        //dataa.drilldown = machinePlantEmp + "/" + selectedMonth + "/" + Convert.ToDateTime(drshift["PDate"]).ToString("dd") + "/" + drshift["Shift"].ToString();
                                        //dataa.afterTitel = machinePlantEmp + " (" + strYear + "-" + selectedMonth + "-" + selectedDay + "-" + drshift["Shift"].ToString() + ")";
                                        //charttdataaa.data.Add(dataa);
                                        charttdataaa.data.Add(Convert.IsDBNull(drshift["Parameter"]) ? 0 : Convert.ToInt32(Convert.ToDecimal(drshift["Parameter"].ToString())));
                                        charttdataaa.Category.Add(drshift["Shift"].ToString());
                                        charttdataaa.drilldown.Add(machinePlantEmp + "/" + selectedMonth + "/" + Convert.ToDateTime(drshift["PDate"]).ToString("dd") + "/" + drshift["Shift"].ToString());
                                    }
                                    else
                                    {
                                        //dataa = new Data();
                                        //dataa.name = drshift["Shift"].ToString();
                                        //dataa.y = Convert.ToInt32(Convert.ToDecimal(drshift["Parameter"].ToString()));
                                        //charttdataaa.data.Add(dataa);
                                        charttdataaa.data.Add(Convert.IsDBNull(drshift["Parameter"]) ? 0 : Convert.ToInt32(Convert.ToDecimal(drshift["Parameter"].ToString())));
                                        charttdataaa.Category.Add(drshift["Shift"].ToString());
                                    }
                                    title = machinePlantEmp + " (" + strYear + "-" + selectedMonth + "-" + selectedDay + " )";
                                }
                                chartData.series.Add(charttdataaa);
                                if (title != "")
                                {
                                    chartData.Title = paramText + " : " + title;
                                }
                                else
                                {
                                    chartData.Title = paramText;
                                }

                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                ///--------------------End Chrome------------------
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            if (HttpContext.Current.Session["ChartDataForBackFun"] != null)
            {
                List<Chart<Series_VDG>> chartList = HttpContext.Current.Session["ChartDataForBackFun"] as List<Chart<Series_VDG>>;
                chartList.Add(chartData);
                HttpContext.Current.Session["ChartDataForBackFun"] = chartList;
            }
            return chartData;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetChartType(string viewtype)
        {
            string ChartType = "";
            try
            {
                bool isCombined = BindCockpitView.IsChartCombined_OEEDAshboard();
                if (isCombined)
                    ChartType = "Combined";
                else
                    ChartType = "Separate";

                //if (viewtype == "ComponentwiseView"||viewtype== "OperatorwiseView")
                //{
                //    ChartType = "Separate";
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return ChartType;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetCombinedChartType(string viewtype)
        {
            string CombinedCharts = "";
            try
            {
                CombinedCharts = BindCockpitView.GetCombinedChartNames_OEEDAshboard();
                //var distCharts = CombinedCharts.Split(',');
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return CombinedCharts;
        }
    }
}