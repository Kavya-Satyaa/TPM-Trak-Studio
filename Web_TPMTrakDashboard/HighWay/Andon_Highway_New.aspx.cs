using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.HighWay
{
    public partial class Andon_Highway_New : System.Web.UI.Page
    {
        public static AndonParameters_Highway settings = new AndonParameters_Highway();
        public static string MainHeaderFontSize;
        public static string SubHeaderFontSize;
        public static string EfficiencyFontSize;
        public static string EfficiencyHeaderFontSize;
        public static string SubHeaderText;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["ComputerName"] == null)
                {
                    ComputerDiv.Visible = true;
                    return;
                }
                else
                {
                    ComputerDiv.Visible = false;
                }
                lblDateTime.InnerText = "DATE : " + DateTime.Now.ToString("dd-MM-yyyy") + "  " + "TIME : " + DateTime.Now.ToString("HH:mm:ss");
                lblShift.Text = "SHIFT - " + DBAccess.GetCurrentShift();
                SetSettings();
            }
        }
        private void SetSettings()
        {
            try
            {
                DataTable dt1 = DBAccess.GetAndonSettingData(HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString());
                if (dt1.Rows.Count > 0)
                {
                    MainHeaderFontSize = settings.MainHeaderFontSize = dt1.AsEnumerable().Where(x => x["ValueInText"].ToString().Equals("MainHeaderFontSize", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault() + "px";
                    settings.SubHeaderFontSize = dt1.AsEnumerable().Where(x => x.Field<string>("ValueInText").ToString().Equals("SubHeaderFontSize", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault() + "px";
                    settings.EfficiencyFontSize = dt1.AsEnumerable().Where(x => x.Field<string>("ValueInText").ToString().Equals("EfficiencyFontSize", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault() + "px";
                    settings.EfficiencyHeaderFontSize = dt1.AsEnumerable().Where(x => x.Field<string>("ValueInText").ToString().Equals("EfficiencyHeaderFontSize", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault() + "px";
                    settings.DatarefreshInterval = string.IsNullOrEmpty(dt1.AsEnumerable().Where(x => x.Field<string>("ValueInText").ToString().Equals("DataRefreshInterval", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault()) ? 10 * 1000 : Convert.ToInt32(dt1.AsEnumerable().Where(x => x.Field<string>("ValueInText").ToString().Equals("DataRefreshInterval", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault()) * 1000;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private static void insertLatestDatatoCache()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void insertLatestDataToMainCacheMemory()
        {
            try
            {
                insertLatestDatatoCache();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("insertLatestDataToMainCacheMemory = " + ex);
            }
        }
        public static Double getDoubleValueFromString(string value)
        {
            Double valueInDouble = 0;
            try
            {
                if (value != "")
                {
                    //valueInDouble = Convert.ToDouble(value);
                    if (double.TryParse(value, out valueInDouble))
                    {
                        return Math.Round(valueInDouble, 3); // Round to 3 decimal places
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return valueInDouble;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<AndonData_Highway> GetHourwiseEnergy()
        {
            //DataTable dt = DBAccess.GetAndonData_Highway(DateTime.Now.ToString(),DBAccess.GetCurrentShift(),"","",DBAccess.GetCellID());
            //DataTable dtHeader = new DataTable();
            //DataTable dtDown = new DataTable();
            //DataTable dt = DBAccess.GetAndonData_Highway("2024-01-13", "", "", "", DBAccess.GetCellID(), out dtHeader, out dtDown);
            //DataTable dt = DBAccess.GetAndonData_Highway(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DBAccess.GetCurrentShift(), "", "", DBAccess.GetCellID(), out dtHeader, out dtDown);
            //HttpContext.Current.Session["DownData"] = dtDown;
            //HttpContext.Current.Session["HeaderData"] = dtHeader;
            DataTable dt = HttpContext.Current.Session["PlantWiseChartData"] as DataTable;
            DataTable dtOperator = new DataTable();
            DataTable dtPlant = new DataTable();
            DataTable dtMachine = new DataTable();
            #region-- Dummy data
            dtOperator.Columns.Add("Value", typeof(double));
            dtOperator.Columns.Add("Time", typeof(string));
            DataRow dtRow = dtOperator.NewRow();
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 32;
            dtRow["Time"] = "01:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 31;
            dtRow["Time"] = "02:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 32;
            dtRow["Time"] = "03:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 32;
            dtRow["Time"] = "04:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 31;
            dtRow["Time"] = "05:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 32;
            dtRow["Time"] = "06:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 32;
            dtRow["Time"] = "07:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 31;
            dtRow["Time"] = "08:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 32;
            dtRow["Time"] = "09:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 31;
            dtRow["Time"] = "10:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 31;
            dtRow["Time"] = "11:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 32;
            dtRow["Time"] = "12:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 31;
            dtRow["Time"] = "13:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 32;
            dtRow["Time"] = "14:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 31;
            dtRow["Time"] = "15:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "16:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "17:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "18:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "19:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "20:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "21:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "22:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "23:00";
            dtOperator.Rows.Add(dtRow);
            dtRow = dtOperator.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "24:00";
            dtOperator.Rows.Add(dtRow);


            dtPlant.Columns.Add("Value", typeof(double));
            dtPlant.Columns.Add("Time", typeof(string));
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 20;
            dtRow["Time"] = "01:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 21;
            dtRow["Time"] = "02:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 22;
            dtRow["Time"] = "03:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 21;
            dtRow["Time"] = "04:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 20;
            dtRow["Time"] = "05:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 21;
            dtRow["Time"] = "06:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 22;
            dtRow["Time"] = "07:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 21;
            dtRow["Time"] = "08:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 20;
            dtRow["Time"] = "09:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 21;
            dtRow["Time"] = "10:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 22;
            dtRow["Time"] = "11:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 21;
            dtRow["Time"] = "12:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 20;
            dtRow["Time"] = "13:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 21;
            dtRow["Time"] = "14:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 21;
            dtRow["Time"] = "15:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "16:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "17:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "18:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "19:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "20:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "21:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "22:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "23:00";
            dtPlant.Rows.Add(dtRow);
            dtRow = dtPlant.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "24:00";
            dtPlant.Rows.Add(dtRow);

            dtMachine.Columns.Add("Value", typeof(double));
            dtMachine.Columns.Add("Time", typeof(string));
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 40;
            dtRow["Time"] = "01:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 39;
            dtRow["Time"] = "02:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 38;
            dtRow["Time"] = "03:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 39;
            dtRow["Time"] = "04:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 40;
            dtRow["Time"] = "05:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 39;
            dtRow["Time"] = "06:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 38;
            dtRow["Time"] = "07:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 39;
            dtRow["Time"] = "08:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 40;
            dtRow["Time"] = "09:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 39;
            dtRow["Time"] = "10:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 38;
            dtRow["Time"] = "11:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 40;
            dtRow["Time"] = "12:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 39;
            dtRow["Time"] = "13:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 40;
            dtRow["Time"] = "14:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 39;
            dtRow["Time"] = "15:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "16:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "17:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "18:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "19:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "20:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "21:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "22:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "23:00";
            dtMachine.Rows.Add(dtRow);
            dtRow = dtMachine.NewRow();
            dtRow["Value"] = 0;
            dtRow["Time"] = "24:00";
            dtMachine.Rows.Add(dtRow);
            #endregion
            List<AndonData_Highway> andonDataList = new List<AndonData_Highway>();
            AndonData_Highway andonData = null;
            List<double> Values1 = new List<double>();
            List<string> Times = new List<string>();
            ChartSeries data = null;
            try
            {
                //StartTime	Endtime	LineOEE	AvgAE	AvgPE	TotalDowntime
                data = new ChartSeries();
                andonData = new AndonData_Highway();
                Values1 = dt.AsEnumerable().Select(x => getDoubleValueFromString(x["AvgAE"].ToString())).ToList();
                //Times = dt.AsEnumerable().Select(x => Convert.ToDateTime(x["StartTime"].ToString()).ToString("HH:mm")).ToList();
                Times = dt.AsEnumerable().Select(x => Convert.ToDateTime(x["Pdate"].ToString()).ToString("dd-MM-yyyy")).ToList();
                data.Value = Values1;
                data.Time = Times;
                andonData.OperatorData = data;

                data = new ChartSeries();
                Values1 = dt.AsEnumerable().Select(x => getDoubleValueFromString(x["AvgPE"].ToString())).ToList();
                //Times = dt.AsEnumerable().Select(x => Convert.ToDateTime(x["StartTime"].ToString()).ToString("HH:mm")).ToList();
                Times = dt.AsEnumerable().Select(x => Convert.ToDateTime(x["Pdate"].ToString()).ToString("dd-MM-yyyy")).ToList();
                data.Value = Values1;
                data.Time = Times;
                andonData.PlantData = data;

                data = new ChartSeries();
                Values1 = dt.AsEnumerable().Select(x => getDoubleValueFromString(x["LineOEE"].ToString())).ToList();
                //Times = dt.AsEnumerable().Select(x => Convert.ToDateTime(x["StartTime"].ToString()).ToString("HH:mm")).ToList();
                Times = dt.AsEnumerable().Select(x => Convert.ToDateTime(x["Pdate"].ToString()).ToString("dd-MM-yyyy")).ToList();
                data.Value = Values1;
                data.Time = Times;
                andonData.MachineData = data;
                andonDataList.Add(andonData);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return andonDataList;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static DataForSummery GetSummeryPieData()
        {
            DataTable dtHeader = new DataTable();
            DataTable dtDown = new DataTable();
            //DataTable dt = DBAccess.GetAndonData_Highway("2024-01-17 15:22:43", DBAccess.GetCurrentShift(), "", "", DBAccess.GetCellID(), out dtHeader, out dtDown);
            DataTable dt = DBAccess.GetAndonData_Highway(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DBAccess.GetCurrentShift(), "", "", DBAccess.GetCellID(), out dtHeader, out dtDown);
            HttpContext.Current.Session["DownData"] = dtDown;
            HttpContext.Current.Session["HeaderData"] = dtHeader;
            HttpContext.Current.Session["PlantWiseChartData"] = dt;

            DataForSummery chartForSummery = new DataForSummery();
            chartForSummery.data = new List<pieData>();
            try
            {
                if (HttpContext.Current.Session["DownData"] != null)
                {
                    dtDown = HttpContext.Current.Session["DownData"] as DataTable;
                }
                #region dummy data
                //DataRow dtRow = dt.NewRow();
                //dt.Columns.Add("Downcategory", typeof(string));
                //dt.Columns.Add("DowmTime", typeof(double));
                //dtRow = dt.NewRow();
                //dtRow["Downcategory"] = "Tool Change";
                //dtRow["DowmTime"] = 32;
                //dt.Rows.Add(dtRow);
                //dtRow = dt.NewRow();
                //dtRow["Downcategory"] = "No Material";
                //dtRow["DowmTime"] = 19;
                //dt.Rows.Add(dtRow);
                //dtRow = dt.NewRow();
                //dtRow["Downcategory"] = "Setting Time";
                //dtRow["DowmTime"] = 15;
                //dt.Rows.Add(dtRow);
                //dtRow = dt.NewRow();
                //dtRow["Downcategory"] = "Machine Breakdown";
                //dtRow["DowmTime"] = 13;
                //dt.Rows.Add(dtRow);
                //dtRow = dt.NewRow();
                //dtRow["Downcategory"] = "Quality Hold";
                //dtRow["DowmTime"] = 6;
                //dt.Rows.Add(dtRow);
                //dtRow = dt.NewRow();
                //dtRow["Downcategory"] = "Others";
                //dtRow["DowmTime"] = 6;
                //dt.Rows.Add(dtRow);
                //dtRow = dt.NewRow();
                //dtRow["Downcategory"] = "No Tool";
                //dtRow["DowmTime"] = 5;
                //dt.Rows.Add(dtRow);
                #endregion

                if (HttpContext.Current.Session["HeaderData"] != null)
                {
                    dtHeader = HttpContext.Current.Session["HeaderData"] as DataTable;
                }
                HeaderData headerdataa = new HeaderData();
                if (dtHeader.Rows.Count > 0)
                {
                    //LineOEE	AvgAE	AvgPE	TotalDowntime
                    headerdataa.PlantUtilization = dtHeader.Rows[0]["LineOEE"].ToString() + " " + "%";
                    headerdataa.MachineUtilization = dtHeader.Rows[0]["AvgAE"].ToString() + " " + "%";
                    headerdataa.OperatorEfficiency = dtHeader.Rows[0]["AvgPE"].ToString() + " " + "%";
                    if (!Convert.IsDBNull(dtHeader.Rows[0]["TotalDowntime"]))
                    {
                        var downtime = dtHeader.Rows[0]["TotalDowntime"].ToString().Split(':');
                        headerdataa.IdleTime = downtime[0] + "H" + " " + downtime[1] + "m";
                    }
                }
                else
                {
                    headerdataa.PlantUtilization = "0" + "%";
                    headerdataa.MachineUtilization = "0" + "%";
                    headerdataa.OperatorEfficiency = "0" + "%";
                    headerdataa.IdleTime = "0" + "H" + " " + "0" + "m";
                }
                headerdataa.Date = "Date : " + DateTime.Now.ToString("dd-MM-yyyy") + "  " + "Time : " + DateTime.Now.ToString("HH:mm:ss");
                headerdataa.Shift = "Shift - " + DBAccess.GetCurrentShift();
                if (dtDown != null)
                {
                    //DownID	DownTime	DownTimeInPrecentage	TotalDownTime
                    chartForSummery.name = dtDown.Columns["DownTimeInPrecentage"].ColumnName;
                    foreach (DataRow dataRow in dtDown.Rows)
                    {
                        pieData pData = new pieData();
                        pData.name = dataRow["DownID"].ToString();
                        pData.y = Convert.IsDBNull(dataRow["DownTimeInPrecentage"]) ? 0 : Convert.ToDouble(dataRow["DownTimeInPrecentage"].ToString());
                        chartForSummery.data.Add(pData);
                    }
                    chartForSummery.headerdata = headerdataa;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return chartForSummery;
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            Response.Cookies["ComputerName"].Value = txtComputerName.Text;
            Response.Cookies["ComputerName"].Expires = DateTime.MaxValue;
            Response.Redirect("Andon_Highway_New.aspx", false);
        }
    }
}