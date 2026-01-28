using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.PTA;

namespace Web_TPMTrakDashboard
{
    public partial class RunTimeChart : System.Web.UI.Page
    {
        string plantID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["FromDate"] != null && Session["ToDate"] != null)
                {
                    txtFromDate.Text = Convert.ToDateTime(Session["FromDate"].ToString()).ToString("dd-MM-yyyy");
                }
                else
                    txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");

                if (Page.ClientQueryString.Length > 0)
                {
                    if (Request.QueryString["source"] != null)
                    {
                        if (Request.QueryString["source"].ToString() == "PTA")
                        {
                            txtFromDate.Text = Request.QueryString["date"].ToString();
                            plantID = Request.QueryString["plantId"].ToString();
                        }
                    }
                }

                BindPlantId();
                BindShiftData();
            }
        }
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

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
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
                if (plantID != "")
                {
                    HelperClassGeneric.setDropdownValue(ddlPlantId, plantID);
                }
                ddlPlantId_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        #endregion

        #region "Bind Cell Id"
        private void BindCellId(string plantId)
        {
            try
            {
                List<string> lstCellId = BindCockpitView.ViewCellsToDisplay(plantId == "Plant All" ? "" : plantId);
                ddlCellID.DataSource = lstCellId;
                ddlCellID.DataBind();
                ddlCellID.Items.Insert(0, new ListItem
                {
                    Text = GetGlobalResourceObject("CommanResource", "CellAll").ToString(),
                    Value = "All"
                });

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            ddlCellID.SelectedIndex = 0;
            ddlCellID_SelectedIndexChanged(null, null);
        }
        #endregion

        #region "Bind Machine Id"
        private void BindMachines(string cellId)
        {
            List<string> allMachineName = new List<string>();
            try
            {
                cellId = cellId == "Cell All" ? "" : cellId;
                ddlMachineId.Items.Clear();
                if (string.IsNullOrEmpty(cellId))
                {
                    allMachineName = VDGDataBaseAccess.GetAllMachines(ddlPlantId.SelectedValue.ToString());
                }
                else
                {
                    allMachineName = CockpitDataBaseAccess.GetMachinesForCell(cellId, ddlPlantId.SelectedValue.ToString());
                }

                if (allMachineName != null && allMachineName.Count > 0)
                {
                    ddlMachineId.DataSource = allMachineName;
                    ddlMachineId.DataBind();
                }
                foreach (ListItem item in ddlMachineId.Items)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        #endregion

        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachines(ddlCellID.SelectedItem == null ? "" : ddlCellID.SelectedItem.Text);
        }
        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellId(ddlPlantId.SelectedItem == null ? "" : ddlPlantId.SelectedItem.Text);
            //btnProcess_Click(null, null);
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<RunTimeChartData> getRunTimeChartData(string plant, string machine, string shift, string date, bool appendData,string totalPageNo)
        {
            List<RunTimeChartData> chartDataList = new List<RunTimeChartData>();
            List<RunChartMachineIndexData> machineIndexList = new List<RunChartMachineIndexData>();
            try
            {
                DataTable dtAllData = new DataTable();
                DataTable dtempty = new DataTable();
                DataTable dtICD = new DataTable();
                DataTable dtML = new DataTable();
                DataTable dtPDT = new DataTable();
                List<string> distMachineList = machine.Split(',').ToList();
                string shiftdetailsStartTime, ShiftStartDateTime;
                shiftdetailsStartTime = string.Empty;
                ShiftStartDateTime = string.Empty;
                if (appendData == false)
                {
                    HttpContext.Current.Session["RunTimeChartMachineLastDate"] = null;
                }

                string frmDate = "", todate = "", shiiftName = "";
                if (shift.Equals("All", StringComparison.OrdinalIgnoreCase))
                {
                    frmDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(date)).ToString("yyyy-MM-dd HH:mm:ss");
                    todate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(date)).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    frmDate = getShiftDetailsOfDate(date, shift, out todate);
                }

                dtAllData = DataBaseAccessPTA.GetMachineUpDownTimes(frmDate, todate, shift, plant, machine, "");
                dtICD = DataBaseAccessPTA.GetMachineUpDownTimes(frmDate, todate, shift, plant, machine, "ICD");
                dtML = DataBaseAccessPTA.GetMachineUpDownTimes(frmDate, todate, shift, plant, machine, "ML");
                object OPDT = null;
                try
                {
                    OPDT = DataBaseAccessPTA.IgnorePDT();
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex);
                }
                if (OPDT != null)
                {
                    if (OPDT.ToString().ToUpper() == "Y")
                    {
                        dtPDT = DataBaseAccessPTA.GetMachineUpDownTimes(frmDate, todate, shift, plant, machine, "PlannedDT");
                    }
                }



                RunTimeChartData chartData = new RunTimeChartData();
                List<string> listCategory = new List<string>();
                List<RunChartData> listChartData = new List<RunChartData>();
                List<RunTimeChartPlantLine> listPlotLine = new List<RunTimeChartPlantLine>();
                int yAxisValue = 0;
                //int paginationvalue = Convert.ToInt32(WebConfigurationManager.AppSettings["RunTimeChartPagination"].ToString());
                int paginationvalue = Convert.ToInt32(totalPageNo);
                List<RunTimeChartMachineTSData> machineTSList = new List<RunTimeChartMachineTSData>();
                for (int machineCount = 0; machineCount < distMachineList.Count; machineCount++)
                {
                    string machineid = "";
                    machineid = distMachineList[machineCount];

                    listCategory.Add(machineid);

                    listChartData.AddRange(getChartSeriesData(machineid, yAxisValue, dtAllData, "#00ff00", "All", appendData));
                    listChartData.AddRange(getChartSeriesData(machineid, yAxisValue, dtICD, "#00ff00", "ICD", appendData));
                    listChartData.AddRange(getChartSeriesData(machineid, yAxisValue, dtML, "#00ff00", "Management Loss", appendData));
                    listChartData.AddRange(getChartSeriesData(machineid, yAxisValue, dtPDT, "#ff0000", "PDT", appendData));

                    yAxisValue++;

                    if ((machineCount + 1) % paginationvalue == 0)
                    {
                        chartData.Category = listCategory;
                        chartData.data = listChartData;
                        chartDataList.Add(chartData);

                        yAxisValue = 0;
                        chartData = new RunTimeChartData();
                        listCategory = new List<string>();
                        listChartData = new List<RunChartData>();
                    }
                    else if (machineCount == distMachineList.Count - 1)
                    {
                        chartData.Category = listCategory;
                        chartData.data = listChartData;
                        chartDataList.Add(chartData);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return chartDataList;
        }
        internal static string getShiftDetailsOfDate(string date, string shiftName, out string shiftEndTime)
        {
            string shiftStartTime = "";
            shiftEndTime = "";
            try
            {
                DateTime date1 = Util.GetDateTime(date).AddHours(13);
                List<PDTData> allShiftList = DataBaseAccess.getShiftTimeDetails(date1);
                if (allShiftList.Count > 0)
                {
                    PDTData data = allShiftList.Where(k => k.ShiftName.Equals(shiftName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (data != null)
                    {
                        shiftStartTime = data.FromDateTime;
                        shiftEndTime = data.ToDateTime;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return shiftStartTime;
        }
        private static List<RunChartData> getChartSeriesData(string machineid, int yvalue, DataTable dtTimeDetails, string color, string status, bool appendData)
        {
            List<RunChartData> listChartData = new List<RunChartData>();
            try
            {
                var datarows = dtTimeDetails.AsEnumerable().Where(k => k.Field<string>("MachineID").Equals(machineid, StringComparison.OrdinalIgnoreCase));
                if (datarows.Any())
                {
                    dtTimeDetails = datarows.CopyToDataTable();
                }
                else
                {
                    dtTimeDetails = dtTimeDetails.Clone();
                }

                string tempStatus = status;
                DateTime? lastDateTime = null;
                if (appendData == true)
                {
                    if (HttpContext.Current.Session["RunTimeChartMachineLastDate"] != null)
                    {
                        List<RunTimeChartMachineTSData> machineLastTSList = new List<RunTimeChartMachineTSData>();
                        machineLastTSList = HttpContext.Current.Session["RunTimeChartMachineLastDate"] as List<RunTimeChartMachineTSData>;
                        RunTimeChartMachineTSData machineTSData = machineLastTSList.Where(k => k.MachineID.Equals(machineid, StringComparison.OrdinalIgnoreCase) && k.Reason == tempStatus).FirstOrDefault();
                        if (machineTSData != null)
                        {
                            if (machineTSData.LastTimeStamp != null)
                            {
                                lastDateTime = machineLastTSList.Where(k => k.MachineID.Equals(machineid, StringComparison.OrdinalIgnoreCase) && k.Reason == tempStatus).Select(k => k.LastTimeStamp).FirstOrDefault();
                            }
                        }
                    }
                    if (lastDateTime != null)
                    {
                        datarows = dtTimeDetails.AsEnumerable().Where(k => k.Field<DateTime>("Starttime") >= lastDateTime);
                        if (datarows.Any())
                        {
                            dtTimeDetails = datarows.CopyToDataTable();
                        }
                        else
                        {
                            dtTimeDetails = dtTimeDetails.Clone();
                        }
                    }
                }
                //DataTable filterTbl = null;
                //var dataRows = dtTimeDetails.AsEnumerable().Where(k => k.Field<string>("MachineID").Equals(machineid, StringComparison.OrdinalIgnoreCase));
                if (dtTimeDetails.Rows.Count > 0)
                {
                    //filterTbl = dtTimeDetails = dataRows.CopyToDataTable();
                    if (dtTimeDetails.Rows.Count > 0)
                    {
                        foreach (DataRow item in dtTimeDetails.Rows)
                        {
                            RunChartData data = new RunChartData();
                            data.y = yvalue;
                            string startdate = item["Starttime"].ToString();
                            string enddate = item["Endtime"].ToString();
                            data.StartDate = startdate;
                            data.EndDate = enddate;
                            DateTime startdatetime = Util.GetDateTime(startdate);
                            DateTime enddatetime = Util.GetDateTime(enddate);
                            data.x = (double)(startdatetime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                            data.x2 = (double)(enddatetime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                            data.color = item["Color"].ToString();
                            data.borderColor = item["Color"].ToString();
                            if (tempStatus == "All")
                            {
                                if (data.color.Equals("Green", StringComparison.OrdinalIgnoreCase))
                                {
                                    status = "Running";
                                }
                                else if (data.color.Equals("Red", StringComparison.OrdinalIgnoreCase))
                                {
                                    status = "Down Reason";
                                }
                            }
                            data.status = status;
                            data.DownReason = item["DownReason"].ToString();
                            listChartData.Add(data);
                        }
                    }
                }
                else
                {
                    if (appendData == false)
                    {
                        RunChartData data = new RunChartData();
                        data.y = yvalue;
                        listChartData.Add(data);
                    }
                }
                if (appendData == false)
                {
                    //insert all reason last timestamp
                    List<RunTimeChartMachineTSData> machineLastTSList = new List<RunTimeChartMachineTSData>();
                    if (HttpContext.Current.Session["RunTimeChartMachineLastDate"] != null)
                    {
                        machineLastTSList = HttpContext.Current.Session["RunTimeChartMachineLastDate"] as List<RunTimeChartMachineTSData>;
                    }
                    RunTimeChartMachineTSData machineTSData = new RunTimeChartMachineTSData();
                    machineTSData.MachineID = machineid;
                    machineTSData.Reason = tempStatus;
                    if (dtTimeDetails.Rows.Count > 0)
                    {
                        machineTSData.LastTimeStamp = dtTimeDetails.AsEnumerable().Select(k => k.Field<DateTime>("Endtime")).Max();
                    }
                    machineLastTSList.Add(machineTSData);
                    HttpContext.Current.Session["RunTimeChartMachineLastDate"] = machineLastTSList;
                }
                else
                {
                    //update all reason last timestamp
                    if (dtTimeDetails.Rows.Count > 0)
                    {
                        List<RunTimeChartMachineTSData> machineLastTSList = new List<RunTimeChartMachineTSData>();
                        if (HttpContext.Current.Session["RunTimeChartMachineLastDate"] != null)
                        {
                            machineLastTSList = HttpContext.Current.Session["RunTimeChartMachineLastDate"] as List<RunTimeChartMachineTSData>;
                            DateTime lastTS = dtTimeDetails.AsEnumerable().Select(k => k.Field<DateTime>("Endtime")).Max();
                            machineLastTSList.Where(k => k.MachineID.Equals(machineid, StringComparison.OrdinalIgnoreCase) && k.Reason == tempStatus).Select(k => k.LastTimeStamp = lastTS).FirstOrDefault();
                            HttpContext.Current.Session["RunTimeChartMachineLastDate"] = machineLastTSList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return listChartData;
        }
    }
}