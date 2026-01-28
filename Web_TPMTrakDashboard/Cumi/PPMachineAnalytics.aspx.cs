using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Cumi.Model;
using Web_TPMTrakDashboard.EnergyModule;
using Web_TPMTrakDashboard.EnergyModule.Models;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Cumi
{
    public partial class PPMachineAnalytics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                // to do get logical day start and end
                var now = DateTime.Now;
                var first = new DateTime(now.Year, now.Month, now.Day, 6, 0, 0);
                //var first = new DateTime(2022, 11, 13, 6, 0, 0);
                txtFromDateMonth.Text = DateTime.ParseExact(VDGDataBaseAccess.GetLogicalDayStart(now.AddDays(-1).ToString("yyyy-MM-dd")), "yyyy-MM-dd HH:mm:ss", provider).ToString("dd-MM-yyyy HH:mm:ss");
                txtToDateMonth.Text = DateTime.ParseExact(VDGDataBaseAccess.GetLogicalDayEnd(now.AddDays(-1).ToString("yyyy-MM-dd")), "yyyy-MM-dd HH:mm:ss", provider).ToString("dd-MM-yyyy HH:mm:ss");
                BindPlant();
                //BindShift();
            }
        }
        private void BindPlant()
        {
            try
            {
                List<string> list = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllPlantsForPlantInfoPP();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPlant: " + ex.Message);
            }
        }
        //private void BindShift()
        //{
        //    try
        //    {
        //        List<PDTData> shiftDetails = Models.DataBaseAccess.getShiftTimeDetails(DateTime.Now);
        //        List<ListItem> list = new List<ListItem>();
        //        foreach (PDTData data in shiftDetails)
        //        {
        //            list.Add(new ListItem() { Text = data.ShiftName, Value = data.FromDateTime + ";;" + data.ToDateTime });
        //        }
        //        ddlShift.DataSource = list;
        //        ddlShift.DataTextField = "Text";
        //        ddlShift.DataValueField = "Value";
        //        ddlShift.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog("BindShift: " + ex.Message);
        //    }
        //}
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<string> BindMachine(string plant)
        {
            List<string> list = new List<string>();
            try
            {
                list = CumiDBAccess.GetAllMachinedByPlant(plant);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachine: " + ex.Message);
            }
            return list;
        }
        #region "Get Line chart for Volt measure"
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static LineChartSeries GetPressureData(string plantid, string machineid, string fromDate, string toDate, string ddlShift)
        {
            double[] DataV1 = null;
            double[] DataV2 = null;
            List<double[]> DataListV1 = new List<double[]>();
            List<double[]> DataListV2 = new List<double[]>();
            DateTime fromdateTime = DateTime.Now;
            DateTime todateTime = DateTime.Now;
            fromdateTime = EnergyModule.Util.GetDateTime(fromDate);
            todateTime = EnergyModule.Util.GetDateTime(toDate);
            fromDate = fromdateTime.ToString("yyyy-MM-dd HH:mm:ss");
            toDate = todateTime.ToString("yyyy-MM-dd HH:mm:ss");
            var VoltLineChart = new LineChartSeries();
            var VoltLineChartORG = new LineChartSeries();
            var SeriesV1 = new DataSeries();
            var SeriesV2 = new DataSeries();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();

            List<ProcessParametersInfo> paramlistt = CumiDBAccess.GetProcessParameterDetails(plantid, machineid, "", fromDate, toDate, "", "DataForTrendChart", out dt, out dt2);

            HttpContext.Current.Session["ParameterList"] = paramlistt;
            try
            {
                if (paramlistt != null && paramlistt.Count > 0)
                {
                    var GetMachineNames = paramlistt.Select(x => x.MachineId).Distinct().ToList();
                    foreach (var machine in GetMachineNames)
                    {


                        VoltLineChart = new LineChartSeries();
                        DataListV1 = new List<double[]>();
                        DataListV2 = new List<double[]>();
                        SeriesV1 = new DataSeries();
                        SeriesV2 = new DataSeries();
                        List<ProcessParametersInfo> paramlisttBottom = paramlistt.Where(x => x.ParameterId == "2" && x.MachineId == machine).ToList();
                        List<ProcessParametersInfo> paramlisttTop = paramlistt.Where(x => x.ParameterId == "1" && x.MachineId == machine).ToList();
                        if (paramlisttBottom.Count == 0 && paramlisttTop.Count == 0)
                        {
                            continue;
                        }
                        for (int i = 0; i < paramlisttBottom.Count; i++)
                        {
                            DataV1 = new double[2];
                            DateTime dtime = new DateTime();
                            dtime = (DateTime)paramlisttBottom[i].updatedtimestamp;
                            DataV1[1] = Convert.ToDouble(paramlisttBottom[i].MaxValue);
                            DataV1[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            DataListV1.Add(DataV1);
                        }
                        for (int j = 0; j < paramlisttTop.Count; j++)
                        {
                            DataV2 = new double[2];
                            DateTime dtime = new DateTime();
                            dtime = (DateTime)paramlisttTop[j].updatedtimestamp;

                            DataV2[1] = Convert.ToDouble(paramlisttTop[j].MaxValue);
                            DataV2[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            DataListV2.Add(DataV2);
                        }
                        List<long> cat = new List<long>();
                        SeriesV1.data = DataListV1;
                        SeriesV1.name = "Bottom Hydraulic Pressure";

                        SeriesV2.data = DataListV2;
                        SeriesV2.name = "Top Hydraulic Pressure";

                        VoltLineChart.series = new List<DataSeries>();
                        VoltLineChart.series.Add(SeriesV2);
                        VoltLineChart.series.Add(SeriesV1);
                        VoltLineChart.Category = cat;
                        VoltLineChart.MachineID = machine;
                        VoltLineChartORG.lineChartSeries.Add(VoltLineChart);


                        //if (dt != null && dt.Rows.Count > 0)
                        //{
                        //    for (int i = 0; i < dt.Rows.Count; i++)
                        //    {
                        //        DataV1 = new double[2];
                        //        DataV2 = new double[2];
                        //        DateTime dtime = new DateTime();
                        //        dtime = EnergyModule.Util.GetDateTime(dt.Rows[i]["updatedtimestamp"].ToString());
                        //        DataV1[1] = Convert.ToDouble(dt.Rows[i]["MinValue"].ToString());
                        //        DataV1[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        //        DataListV1.Add(DataV1);

                        //        DataV2[1] = Convert.ToDouble(dt.Rows[i]["MaxValue"].ToString());
                        //        DataV2[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        //        DataListV2.Add(DataV2);

                        //    }
                        //    List<long> cat = new List<long>();
                        //    SeriesV1.data = DataListV1;
                        //    SeriesV1.name = "Bottom Hydraulic Pressure";

                        //    SeriesV2.data = DataListV2;
                        //    SeriesV2.name = "Top Hydraulic Pressure";

                        //    VoltLineChart.series = new List<DataSeries>();
                        //    VoltLineChart.series.Add(SeriesV2);
                        //    VoltLineChart.series.Add(SeriesV1);
                        //    VoltLineChart.Category = cat;
                        //    VoltLineChart.MachineID = machine;
                        //    VoltLineChartORG.lineChartSeries.Add(VoltLineChart);
                        //}
                        //else
                        //{

                        //}
                    }
                }


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

            return VoltLineChartORG;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static LineChartSeries GetRamData(string plantid, string machineid, string fromDate, string toDate, string ddlShift)
        {
            double[] DataV1 = null;
            double[] DataV2 = null;
            List<double[]> DataListV1 = new List<double[]>();
            List<double[]> DataListV2 = new List<double[]>();
            DateTime fromdateTime = DateTime.Now;
            DateTime todateTime = DateTime.Now;
            fromdateTime = EnergyModule.Util.GetDateTime(fromDate);
            todateTime = EnergyModule.Util.GetDateTime(toDate);
            fromDate = fromdateTime.ToString("yyyy-MM-dd HH:mm:ss");
            toDate = todateTime.ToString("yyyy-MM-dd HH:mm:ss");
            var VoltLineChart = new LineChartSeries();
            var VoltLineChartORG = new LineChartSeries();
            var SeriesV1 = new DataSeries();
            var SeriesV2 = new DataSeries();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            //List<ProcessParametersInfo> ParameterList = HttpContext.Current.Session["ParameterList"] as List<ProcessParametersInfo>;

            List<ProcessParametersInfo> paramlistt = HttpContext.Current.Session["ParameterList"] as List<ProcessParametersInfo>;
            paramlistt = paramlistt == null || paramlistt.Count == 0 ? CumiDBAccess.GetProcessParameterDetails(plantid, machineid, "", fromDate, toDate, "", "DataForTrendChart", out dt, out dt2) : paramlistt;
            try
            {
                if (paramlistt != null && paramlistt.Count > 0)
                {
                    var GetMachineNames = paramlistt.Select(x => x.MachineId).Distinct().ToList();
                    foreach (var machine in GetMachineNames)
                    {


                        VoltLineChart = new LineChartSeries();
                        DataListV1 = new List<double[]>();
                        DataListV2 = new List<double[]>();
                        SeriesV1 = new DataSeries();
                        SeriesV2 = new DataSeries();
                        List<ProcessParametersInfo> paramlisttBottom = paramlistt.Where(x => x.ParameterId == "4" && x.MachineId == machine).ToList();
                        List<ProcessParametersInfo> paramlisttTop = paramlistt.Where(x => x.ParameterId == "3" && x.MachineId == machine).ToList();
                        if (paramlisttBottom.Count == 0 && paramlisttTop.Count == 0)
                        {
                            continue;
                        }
                        for (int i = 0; i < paramlisttBottom.Count; i++)
                        {
                            DataV1 = new double[2];
                            DateTime dtime = new DateTime();
                            dtime = (DateTime)paramlisttBottom[i].updatedtimestamp;
                            DataV1[1] = Convert.ToDouble(paramlisttBottom[i].MaxValue);
                            DataV1[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            DataListV1.Add(DataV1);
                        }
                        for (int j = 0; j < paramlisttTop.Count; j++)
                        {
                            DataV2 = new double[2];
                            DateTime dtime = new DateTime();
                            dtime = (DateTime)paramlisttTop[j].updatedtimestamp;

                            DataV2[1] = Convert.ToDouble(paramlisttTop[j].MaxValue);
                            DataV2[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            DataListV2.Add(DataV2);
                        }
                        List<long> cat = new List<long>();
                        SeriesV1.data = DataListV1;
                        SeriesV1.name = "Bottom Ram Stroke";

                        SeriesV2.data = DataListV2;
                        SeriesV2.name = "Top Ram Stroke";

                        VoltLineChart.series = new List<DataSeries>();
                        VoltLineChart.series.Add(SeriesV2);
                        VoltLineChart.series.Add(SeriesV1);
                        VoltLineChart.Category = cat;
                        VoltLineChart.MachineID = machine;
                        VoltLineChartORG.lineChartSeries.Add(VoltLineChart);


                        //if (dt != null && dt.Rows.Count > 0)
                        //{
                        //    for (int i = 0; i < dt.Rows.Count; i++)
                        //    {
                        //        DataV1 = new double[2];
                        //        DataV2 = new double[2];
                        //        DateTime dtime = new DateTime();
                        //        dtime = EnergyModule.Util.GetDateTime(dt.Rows[i]["updatedtimestamp"].ToString());
                        //        DataV1[1] = Convert.ToDouble(dt.Rows[i]["MinValue"].ToString());
                        //        DataV1[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        //        DataListV1.Add(DataV1);

                        //        DataV2[1] = Convert.ToDouble(dt.Rows[i]["MaxValue"].ToString());
                        //        DataV2[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        //        DataListV2.Add(DataV2);

                        //    }
                        //    List<long> cat = new List<long>();
                        //    SeriesV1.data = DataListV1;
                        //    SeriesV1.name = "Bottom Ram Stroke";

                        //    SeriesV2.data = DataListV2;
                        //    SeriesV2.name = "Top Ram Stroke";

                        //    VoltLineChart.series = new List<DataSeries>();
                        //    VoltLineChart.series.Add(SeriesV2);
                        //    VoltLineChart.series.Add(SeriesV1);
                        //    VoltLineChart.Category = cat;
                        //    VoltLineChart.MachineID = machine;
                        //    VoltLineChartORG.lineChartSeries.Add(VoltLineChart);
                        //}
                        //else
                        //{

                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

            return VoltLineChartORG;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static LineChartSeries GetOilData(string plantid, string machineid, string fromDate, string toDate, string ddlShift)
        {
            double[] DataV1 = null;
            List<double[]> DataListV1 = new List<double[]>();
            DateTime fromdateTime = DateTime.Now;
            DateTime todateTime = DateTime.Now;
            fromdateTime = EnergyModule.Util.GetDateTime(fromDate);
            todateTime = EnergyModule.Util.GetDateTime(toDate);
            fromDate = fromdateTime.ToString("yyyy-MM-dd HH:mm:ss");
            toDate = todateTime.ToString("yyyy-MM-dd HH:mm:ss");
            var VoltLineChart = new LineChartSeries();
            var VoltLineChartORG = new LineChartSeries();
            var SeriesV1 = new DataSeries();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();

            List<ProcessParametersInfo> paramlistt = HttpContext.Current.Session["ParameterList"] as List<ProcessParametersInfo>;
            paramlistt = paramlistt == null || paramlistt.Count == 0 ? CumiDBAccess.GetProcessParameterDetails(plantid, machineid, "", fromDate, toDate, "", "DataForTrendChart", out dt, out dt2) : paramlistt;
            //List<ProcessParametersInfo> paramlistt = CumiDBAccess.GetProcessParameterDetails(plantid, machineid, "", fromDate, toDate, "", "DataForTrendChart", out dt);
            try
            {
                var GetMachineNames = paramlistt.Select(x => x.MachineId).Distinct().ToList();
                foreach (var machine in GetMachineNames)
                {
                    VoltLineChart = new LineChartSeries();
                    DataListV1 = new List<double[]>();
                    SeriesV1 = new DataSeries();
                    List<ProcessParametersInfo> paramlisttOil = paramlistt.Where(x => x.ParameterId == "5" && x.MachineId == machine).ToList();
                    if (paramlisttOil.Count == 0)
                    {
                        continue;
                    }
                    for (int i = 0; i < paramlisttOil.Count; i++)
                    {
                        DataV1 = new double[2];
                        DateTime dtime = new DateTime();
                        dtime = (DateTime)paramlisttOil[i].updatedtimestamp;
                        DataV1[1] = Convert.ToDouble(paramlisttOil[i].MaxValue);
                        DataV1[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        DataListV1.Add(DataV1);
                    }
                    List<long> cat = new List<long>();
                    SeriesV1.data = DataListV1;
                    SeriesV1.name = "Oil Temperature";

                    VoltLineChart.series = new List<DataSeries>();
                    VoltLineChart.series.Add(SeriesV1);
                    VoltLineChart.Category = cat;
                    VoltLineChart.MachineID = machine;
                    VoltLineChartORG.lineChartSeries.Add(VoltLineChart);
                    //dt = new DataTable();
                    //ToConvertListToDataTable toConvert = new ToConvertListToDataTable();  /*.Where(x => x.ParameterId == "1" || x.ParameterId == "2")*/
                    //dt = toConvert.ToDataTable(paramlistt.Where(x => (x.ParameterId == "3" || x.ParameterId == "4") && x.MachineId == machine).ToList());
                    //if (dt != null && dt.Rows.Count > 0)
                    //{
                    //    List<long> cat = new List<long>();
                    //    for (int i = 0; i < dt.Rows.Count; i++)
                    //    {
                    //        DataV1 = new double[2];
                    //        DateTime dtime = new DateTime();
                    //        dtime = EnergyModule.Util.GetDateTime(dt.Rows[i]["updatedtimestamp"].ToString());
                    //        DataV1[1] = Convert.ToDouble(dt.Rows[i]["Value"].ToString());
                    //        DataV1[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                    //        DataListV1.Add(DataV1);
                    //        //cat.Add(EnergyModule.Util.GetDateTime(dt.Rows[i]["updatedtimestamp"].ToString()).Month);
                    //    }

                    //    SeriesV1.data = DataListV1;
                    //    SeriesV1.name = "Oil Temperature";

                    //    VoltLineChart.series = new List<DataSeries>();
                    //    VoltLineChart.series.Add(SeriesV1);
                    //    VoltLineChart.Category = cat;
                    //    VoltLineChart.MachineID = machine;
                    //    VoltLineChartORG.lineChartSeries.Add(VoltLineChart);
                    //}
                    //else
                    //{

                    //}
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

            return VoltLineChartORG;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static LineChartSeries GetCoolantData(string plantid, string machineid, string fromDate, string toDate, string ddlShift)
        {
            double[] DataV1 = null;
            List<double[]> DataListV1 = new List<double[]>();
            DateTime fromdateTime = DateTime.Now;
            DateTime todateTime = DateTime.Now;
            fromdateTime = EnergyModule.Util.GetDateTime(fromDate);
            todateTime = EnergyModule.Util.GetDateTime(toDate);
            fromDate = fromdateTime.ToString("yyyy-MM-dd HH:mm:ss");
            toDate = todateTime.ToString("yyyy-MM-dd HH:mm:ss");
            var VoltLineChart = new LineChartSeries();
            var VoltLineChartORG = new LineChartSeries();
            var SeriesV1 = new DataSeries();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();

            List<ProcessParametersInfo> paramlistt = HttpContext.Current.Session["ParameterList"] as List<ProcessParametersInfo>;
            paramlistt = paramlistt == null || paramlistt.Count == 0 ? CumiDBAccess.GetProcessParameterDetails(plantid, machineid, "", fromDate, toDate, "", "DataForTrendChart", out dt, out dt2) : paramlistt;
            //List<ProcessParametersInfo> paramlistt = CumiDBAccess.GetProcessParameterDetails(plantid, machineid, "", fromDate, toDate, "", "DataForTrendChart", out dt);
            try
            {
                var GetMachineNames = paramlistt.Select(x => x.MachineId).Distinct().ToList();
                //foreach (var machine in GetMachineNames)
                //{
                VoltLineChart = new LineChartSeries();
                DataListV1 = new List<double[]>();
                SeriesV1 = new DataSeries();
                List<ProcessParametersInfo> paramlisttOil = paramlistt.Where(x => x.ParameterId == "6").ToList();
                if (paramlisttOil != null && paramlisttOil.Count > 0)
                {
                    for (int i = 0; i < paramlisttOil.Count; i++)
                    {
                        DataV1 = new double[2];
                        DateTime dtime = new DateTime();
                        dtime = (DateTime)paramlisttOil[i].updatedtimestamp;
                        DataV1[1] = Convert.ToDouble(paramlisttOil[i].MaxValue);
                        DataV1[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        DataListV1.Add(DataV1);
                    }
                    List<long> cat = new List<long>();
                    SeriesV1.data = DataListV1;
                    SeriesV1.name = "Cooling Tower Water Temperature";
                    //SeriesV1.color = "red";

                    VoltLineChart.series = new List<DataSeries>();
                    VoltLineChart.series.Add(SeriesV1);
                    VoltLineChart.Category = cat;
                    VoltLineChart.MachineID = "Coolant Temperature";
                    VoltLineChartORG.lineChartSeries.Add(VoltLineChart);
                }
                //dt = new DataTable();
                //ToConvertListToDataTable toConvert = new ToConvertListToDataTable();  /*.Where(x => x.ParameterId == "1" || x.ParameterId == "2")*/
                //dt = toConvert.ToDataTable(paramlistt.Where(x => (x.ParameterId == "3" || x.ParameterId == "4") && x.MachineId == machine).ToList());
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    List<long> cat = new List<long>();
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        DataV1 = new double[2];
                //        DateTime dtime = new DateTime();
                //        dtime = EnergyModule.Util.GetDateTime(dt.Rows[i]["updatedtimestamp"].ToString());
                //        DataV1[1] = Convert.ToDouble(dt.Rows[i]["Value"].ToString());
                //        DataV1[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                //        DataListV1.Add(DataV1);
                //        //cat.Add(EnergyModule.Util.GetDateTime(dt.Rows[i]["updatedtimestamp"].ToString()).Month);
                //    }

                //    SeriesV1.data = DataListV1;
                //    SeriesV1.name = "Oil Temperature";

                //    VoltLineChart.series = new List<DataSeries>();
                //    VoltLineChart.series.Add(SeriesV1);
                //    VoltLineChart.Category = cat;
                //    VoltLineChart.MachineID = machine;
                //    VoltLineChartORG.lineChartSeries.Add(VoltLineChart);
                //}
                //else
                //{

                //}
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

            return VoltLineChartORG;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string ResetSession()
        {
            HttpContext.Current.Session.Remove("ParameterList");
            return "success";
        }
        #endregion

        //protected void timerToAutoRefreshPP_Tick(object sender, EventArgs e)
        //{
        //    txtFromDateMonth.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy HH:mm:ss");
        //    txtToDateMonth.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

        //    ScriptManager.RegisterStartupScript(this, GetType(), "CallbtnProcess", "btnProcessClicked();", true);
        //}
    }
}