using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.EnergyModule.Models;

namespace Web_TPMTrakDashboard.EnergyModule
{
    public partial class PowerProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDateTime.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy HH:mm:ss");
                txtToDateTime.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                BindPlantIDs();
                BindCellIDs();
                List<string> allCellIDs = DataBaseAccess.GetCellIDs("");
                if (allCellIDs.Count > 0)
                {
                    tdCell.Visible = true;
                    ddlCell.Visible = true;
                    //ddlMachine.DataSource = DataBaseAccess.GetAllEM_Machines_Kiswok(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString(), ddlCell.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedValue.ToString(), "", "");
                    ddlMachine.DataSource = DataBaseAccess.GetAllEM_Machines_Kiswok(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString(), ddlCell.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedValue.ToString(), "Report", ddlMachineType.SelectedValue.ToString());
                    ddlMachine.DataBind();
                }
                else
                {
                    tdCell.Visible = false;
                    ddlCell.Visible = false;
                    //ddlMachine.DataSource = DataBaseAccess.GetAllEM_Machines();
                    //ddlMachine.DataSource = DataBaseAccess.GetAllEM_Machines(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString());
                    ddlMachine.DataSource = DataBaseAccess.GetAllEM_Machines_Kiswok(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString(), ddlCell.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedValue.ToString(), "Report", ddlMachineType.SelectedValue.ToString());
                }
                ddlMachine.DataBind();
                //lblForProfile.Text = "POWER PROFILE - " + ddlMachine.SelectedValue;
                ddlDurationFormat_SelectedIndexChanged(null, null);
            }
        }

        protected void ddlDurationFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDurationFormat.SelectedValue.Equals("Hour", StringComparison.OrdinalIgnoreCase))
            {
                for (int i = 1; i < 24; i++)
                {
                    ddlDurationValue.Items.Add(i.ToString());
                }
                ddlDurationValue.SelectedValue = "8";
            }
            else
            {
                for (int i = 1; i < 59; i++)
                {
                    ddlDurationValue.Items.Add(i.ToString());
                }
                ddlDurationValue.SelectedValue = "1";
            }
        }

        //protected void btnProcess_Click(object sender, EventArgs e)
        //{

        //}
        #region "Get Line chart for Watt measurement"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static LineChartSeries GetWattData(string fromDate, string toDate, string selectedMachineId, string durationType, string durationVal)
        {
            double[] Data = null;
            List<double[]> DataList = new List<double[]>();
            List<string> datetimes = new List<string>();
            List<double> data = new List<double>();
            DateTime fromdateTime = DateTime.Now;
            DateTime todateTime = DateTime.Now;
            //fromdateTime = DateTime.ParseExact(fromDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //todateTime = DateTime.ParseExact(toDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            fromdateTime = Util.GetDateTime(fromDate);
            todateTime = Util.GetDateTime(toDate);
            fromDate = fromdateTime.ToString("yyyy-MM-dd HH:mm:ss");
            toDate = todateTime.ToString("yyyy-MM-dd HH:mm:ss");
            var WattLineChart = new LineChartSeries();
            var WattSeries = new DataSeries();
            DataTable dt = DataBaseAccess.GetEnergyData(fromDate, toDate, selectedMachineId);
            try
            {

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        datetimes.Add(dt.Rows[i]["gtime"].ToString());
                        data.Add(Convert.ToDouble(dt.Rows[i]["watt"].ToString()));
                    }
                    for (int i = 0; i < data.Count; i++)
                    {
                        Data = new double[2];
                        DateTime dtime = new DateTime();
                        //DateTime.TryParseExact(datetimes[i], "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtime);
                        dtime = Util.GetDateTime(datetimes[i]);
                       // DateTime.TryParseExact(datetimes[i], "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtime);
                        Data[1] = data[i];
                        Data[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        DataList.Add(Data);
                    }
                    List<long> cat = new List<long>();
                    WattSeries.data = DataList;
                    WattSeries.name = "Watt";
                    WattSeries.color = "#017811";
                    WattLineChart.series = new List<DataSeries>();
                    WattLineChart.series.Add(WattSeries);
                    WattLineChart.Category = cat;
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

            return WattLineChart;
        }
        #endregion

        #region "Get Line chart for Ampere measure"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static LineChartSeries GetAmpereData(string fromDate, string toDate, string selectedMachineId, string durationType, string durationVal)
        {
            double[] rData = null;
            double[] yData = null;
            double[] bData = null;
            List<double[]> rDataList = new List<double[]>();
            List<double[]> yDataList = new List<double[]>();
            List<double[]> bDataList = new List<double[]>();
            //List<string> datetimes = new List<string>();
            //List<double> Rdata = new List<double>();
            //List<double> Ydata = new List<double>();
            //List<double> Bdata = new List<double>();
            DateTime fromdateTime = DateTime.Now;
            DateTime todateTime = DateTime.Now;
           // fromdateTime = DateTime.ParseExact(fromDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
           // todateTime = DateTime.ParseExact(toDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            fromdateTime = Util.GetDateTime(fromDate);
            todateTime = Util.GetDateTime(toDate);
            fromDate = fromdateTime.ToString("yyyy-MM-dd HH:mm:ss");
            toDate = todateTime.ToString("yyyy-MM-dd HH:mm:ss");
            var AmpereLineChart = new LineChartSeries();
            var RSeries = new DataSeries();
            var YSeries = new DataSeries();
            var BSeries = new DataSeries();
            DataTable dt = DataBaseAccess.GetEnergyData(fromDate, toDate, selectedMachineId);
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    datetimes.Add(dt.Rows[i]["gtime"].ToString());
                    //    Rdata.Add(Convert.ToDouble(dt.Rows[i]["AmpereR"].ToString()));
                    //    Ydata.Add(Convert.ToDouble(dt.Rows[i]["AmpereY"].ToString()));
                    //    Bdata.Add(Convert.ToDouble(dt.Rows[i]["AmpereB"].ToString()));
                    //}
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        rData = new double[2];
                        yData = new double[2];
                        bData = new double[2];
                        DateTime dtime = new DateTime();
                        //DateTime.TryParseExact(dt.Rows[i]["gtime"].ToString(), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtime);
                        dtime = Util.GetDateTime(dt.Rows[i]["gtime"].ToString());
                        rData[1] = Convert.ToDouble(dt.Rows[i]["AmpereR"].ToString());
                        rData[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        rDataList.Add(rData);

                        yData[1] = Convert.ToDouble(dt.Rows[i]["AmpereY"].ToString());
                        yData[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        yDataList.Add(yData);

                        bData[1] = Convert.ToDouble(dt.Rows[i]["AmpereB"].ToString());
                        bData[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        bDataList.Add(bData);
                    }
                    List<long> cat = new List<long>();
                    RSeries.data = rDataList;
                    RSeries.name = "AmpereR";
                    RSeries.color = "red";

                    YSeries.data = yDataList;
                    YSeries.name = "AmpereY";
                    YSeries.color = "yellow";

                    BSeries.data = bDataList;
                    BSeries.name = "AmpereB";
                    BSeries.color = "blue";

                    AmpereLineChart.series = new List<DataSeries>();
                    AmpereLineChart.series.Add(RSeries);
                    AmpereLineChart.series.Add(YSeries);
                    AmpereLineChart.series.Add(BSeries);
                    AmpereLineChart.Category = cat;
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

            return AmpereLineChart;
        }
        #endregion

        #region "Get Line chart for Volt measure"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static LineChartSeries GetVoltData(string fromDate, string toDate, string selectedMachineId, string durationType, string durationVal)
        {
            double[] DataV1 = null;
            double[] DataV2 = null;
            double[] DataV3 = null;
            double[] DataV4 = null;
            double[] DataV5 = null;
            double[] DataV6 = null;
            List<double[]> DataListV1 = new List<double[]>();
            List<double[]> DataListV2 = new List<double[]>();
            List<double[]> DataListV3 = new List<double[]>();
            List<double[]> DataListV4 = new List<double[]>();
            List<double[]> DataListV5 = new List<double[]>();
            List<double[]> DataListV6 = new List<double[]>();
            //List<string> datetimes = new List<string>();
            //List<double> Rdata = new List<double>();
            //List<double> Ydata = new List<double>();
            //List<double> Bdata = new List<double>();
            DateTime fromdateTime = DateTime.Now;
            DateTime todateTime = DateTime.Now;
            // fromdateTime = DateTime.ParseExact(fromDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            // todateTime = DateTime.ParseExact(toDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            fromdateTime = Util.GetDateTime(fromDate);
            todateTime = Util.GetDateTime(toDate);
            fromDate = fromdateTime.ToString("yyyy-MM-dd HH:mm:ss");
            toDate = todateTime.ToString("yyyy-MM-dd HH:mm:ss");
            var VoltLineChart = new LineChartSeries();
            var SeriesV1 = new DataSeries();
            var SeriesV2 = new DataSeries();
            var SeriesV3 = new DataSeries();
            var SeriesV4 = new DataSeries();
            var SeriesV5 = new DataSeries();
            var SeriesV6 = new DataSeries();
            DataTable dt = DataBaseAccess.GetEnergyData(fromDate, toDate, selectedMachineId);
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    datetimes.Add(dt.Rows[i]["gtime"].ToString());
                    //    Rdata.Add(Convert.ToDouble(dt.Rows[i]["AmpereR"].ToString()));
                    //    Ydata.Add(Convert.ToDouble(dt.Rows[i]["AmpereY"].ToString()));
                    //    Bdata.Add(Convert.ToDouble(dt.Rows[i]["AmpereB"].ToString()));
                    //}
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataV1 = new double[2];
                        DataV2 = new double[2];
                        DataV3 = new double[2];
                        DataV4 = new double[2];
                        DataV5 = new double[2];
                        DataV6 = new double[2];
                        DateTime dtime = new DateTime();
                        // DateTime.TryParseExact(dt.Rows[i]["gtime"].ToString(), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtime);
                        dtime = Util.GetDateTime(dt.Rows[i]["gtime"].ToString());
                        DataV1[1] = Convert.ToDouble(dt.Rows[i]["Volt1"].ToString());
                        DataV1[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        DataListV1.Add(DataV1);

                        DataV2[1] = Convert.ToDouble(dt.Rows[i]["Volt2"].ToString());
                        DataV2[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        DataListV2.Add(DataV2);

                        DataV3[1] = Convert.ToDouble(dt.Rows[i]["Volt3"].ToString());
                        DataV3[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        DataListV3.Add(DataV3);

                        DataV4[1] = Convert.ToDouble(dt.Rows[i]["Volt4"].ToString());
                        DataV4[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        DataListV4.Add(DataV4);

                        DataV5[1] = Convert.ToDouble(dt.Rows[i]["Volt5"].ToString());
                        DataV5[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        DataListV5.Add(DataV5);

                        DataV6[1] = Convert.ToDouble(dt.Rows[i]["Volt6"].ToString());
                        DataV6[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        DataListV6.Add(DataV6);
                    }
                    List<long> cat = new List<long>();
                    SeriesV1.data = DataListV1;
                    //SeriesV1.name = "Volt1";
                    SeriesV1.name = "VLN-R";
                    SeriesV1.color = "red";

                    SeriesV2.data = DataListV2;
                    //SeriesV2.name = "Volt2";
                    SeriesV2.name = "VLN-Y";
                    SeriesV2.color = "yellow";

                    SeriesV3.data = DataListV3;
                    //SeriesV3.name = "Volt3";
                    SeriesV3.name = "VLN-B";
                    SeriesV3.color = "blue";

                    SeriesV4.data = DataListV4;
                    //SeriesV4.name = "Volt4";
                    SeriesV4.name = "VLN-R-Y";
                    SeriesV4.color = "#017811";

                    SeriesV5.data = DataListV5;
                    //SeriesV5.name = "Volt5";
                    SeriesV5.name = "VLN-Y-B";
                    SeriesV5.color = "#09c7e0";

                    SeriesV6.data = DataListV6;
                    //SeriesV6.name = "Volt6";
                    SeriesV6.name = "VLN-B-R";
                    SeriesV6.color = "#cf0692";

                    VoltLineChart.series = new List<DataSeries>();
                    VoltLineChart.series.Add(SeriesV1);
                    VoltLineChart.series.Add(SeriesV2);
                    VoltLineChart.series.Add(SeriesV3);
                    VoltLineChart.series.Add(SeriesV4);
                    VoltLineChart.series.Add(SeriesV5);
                    VoltLineChart.series.Add(SeriesV6);
                    VoltLineChart.Category = cat;
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

            return VoltLineChart;
        }
        #endregion

        #region "Get Line chart for PF measure"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static LineChartSeries GetPFData(string fromDate, string toDate, string selectedMachineId, string durationType, string durationVal)
        {
            double[] Data = null;
            List<double[]> DataList = new List<double[]>();
            List<string> datetimes = new List<string>();
            List<double> data = new List<double>();
            DateTime fromdateTime = DateTime.Now;
            DateTime todateTime = DateTime.Now;
            // fromdateTime = DateTime.ParseExact(fromDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            // todateTime = DateTime.ParseExact(toDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            fromdateTime = Util.GetDateTime(fromDate);
            todateTime = Util.GetDateTime(toDate);
            fromDate = fromdateTime.ToString("yyyy-MM-dd HH:mm:ss");
            toDate = todateTime.ToString("yyyy-MM-dd HH:mm:ss");
            var PFLineChart = new LineChartSeries();
            var PFSeries = new DataSeries();
            DataTable dt = DataBaseAccess.GetEnergyData(fromDate, toDate, selectedMachineId);
            try
            {

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        datetimes.Add(dt.Rows[i]["gtime"].ToString());
                        data.Add(Convert.ToDouble(dt.Rows[i]["pf"].ToString()));
                    }
                    for (int i = 0; i < data.Count; i++)
                    {
                        Data = new double[2];
                        DateTime dtime = new DateTime();
                        // DateTime.TryParseExact(datetimes[i], "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtime);
                        dtime = Util.GetDateTime(datetimes[i]);
                        Data[1] = data[i];
                        Data[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        DataList.Add(Data);
                    }
                    List<long> cat = new List<long>();
                    PFSeries.data = DataList;
                    PFSeries.name = "PF";
                    PFSeries.color = "#017811";
                    PFLineChart.series = new List<DataSeries>();
                    PFLineChart.series.Add(PFSeries);
                    PFLineChart.Category = cat;
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

            return PFLineChart;
        }
        #endregion

        protected void rbl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbl.SelectedValue.Equals("History", StringComparison.OrdinalIgnoreCase))
            {
                txtFromDateTime.Enabled = true;
                txtToDateTime.Enabled = true;
                ddlDurationFormat.Enabled = true;
                ddlDurationValue.Enabled = true;
                btnProcess.Enabled = true;
                timerToAutoRefresh.Enabled = false;
            }
            else
            {
                txtFromDateTime.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy HH:mm:ss");
                txtToDateTime.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                txtFromDateTime.Enabled = false;
                txtToDateTime.Enabled = false;
                ddlDurationFormat.Enabled = false;
                ddlDurationValue.Enabled = false;
                btnProcess.Enabled = false;
                timerToAutoRefresh.Enabled = true;
                timerToAutoRefresh.Interval = 1000 * Convert.ToInt32(WebConfigurationManager.AppSettings["EnergyAutoRefreshInterval"].ToString()); 
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "CallbtnProcess", "btnProcessClicked();", true);
        }

        protected void timerToAutoRefresh_Tick(object sender, EventArgs e)
        {
            if (!rbl.SelectedValue.Equals("History", StringComparison.OrdinalIgnoreCase))
            {
                txtFromDateTime.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy HH:mm:ss");
                txtToDateTime.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "CallbtnProcess", "btnProcessClicked();", true);
        }
        private void BindPlantIDs()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlants();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindCellIDs()
        {
            try
            {
                List<string> list = DataBaseAccess.GetCellIDs(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString());
                ddlCell.DataSource = list;
                ddlCell.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellIDs(); 
            ddlMachine.DataSource = DataBaseAccess.GetAllEM_Machines(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString());
            ddlMachine.DataBind();
        }

        protected void ddlCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMachine.DataSource = DataBaseAccess.GetAllEM_Machines_Kiswok(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString(), ddlCell.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedValue.ToString(), "", "");
            ddlMachine.DataBind();
        }

        protected void ddlMachineType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlMachine.DataSource = DataBaseAccess.GetAllEM_Machines_Kiswok(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString(), ddlCell.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedValue.ToString(), "Report", ddlMachineType.SelectedValue.ToString());
                ddlMachine.DataBind();
                if (ddlMachineType.SelectedValue.ToString().Equals("Non-Machine EM", StringComparison.OrdinalIgnoreCase))
                {
                    tdCell.Visible = false;
                    tdPlant.Visible = false;
                    ddlPlant.Visible = false;
                    ddlCell.Visible = false;
                }
                else
                {
                    tdCell.Visible = true;
                    tdPlant.Visible = true;
                    ddlPlant.Visible = true;
                    ddlCell.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
    public class DataSeries
    {
        public List<double[]> data { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string color { get; set; }
        public int yAxis { get; set; } = 0;
    }
    public class LineChartSeries
    {
        public string MachineID { get; set; }
        public List<long> Category { get; set; }
        public List<DataSeries> series { get; set; }
        public List<DataSeries> seriesMonthly { get; set; } = new List<DataSeries>();
        public List<LineChartSeries> lineChartSeries { get; set; } = new List<LineChartSeries>();
    }
    public class Series
    {
        public string name { get; set; }
        public List<double> data { get; set; }
    }
    public class DataForWatt
    {
        public string TitleOfWatt { get; set; }
        public string xAxisTitleWatt { get; set; }
        public string yAxisTitleWatt { get; set; }
        public int minValueWatt { get; set; }
        public int maxValueWatt { get; set; }

        public List<string> wattCategories { get; set; }
        public List<Series> seriesWatt { get; set; }
    }
    public class DataForAmpere
    {
        public string TitleAmpere { get; set; }
        public string xAxisTitleAmpere { get; set; }
        public string yAxisTitleAmpere { get; set; }
        public int minValueAmpere { get; set; }
        public int maxValueAmpere { get; set; }

        public List<string> ampereCategories { get; set; }
        public List<Series> seriesAmpere { get; set; }
    }
    public class DataForVolt
    {
        public string TitleVolt { get; set; }
        public string xAxisTitleVolt { get; set; }
        public string yAxisTitleVolt { get; set; }
        public int minValueVolt { get; set; }
        public int maxValueVolt { get; set; }

        public List<string> voltCategories { get; set; }
        public List<Series> seriesVolt { get; set; }
    }
    public class DataForPF
    {
        public string TitlePF { get; set; }
        public string xAxisTitlePF { get; set; }
        public string yAxisTitlePF { get; set; }
        public int minValuePF { get; set; }
        public int maxValuePF { get; set; }

        public List<string> PFCategories { get; set; }
        public List<Series> seriesPF { get; set; }
    }
}