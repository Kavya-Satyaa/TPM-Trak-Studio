using ChartDirector;
using Elmah;
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
using Web_TPMTrakDashboard.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using ModelClassLibrary;
using BusinessClassLibrary;
using System.Drawing;
using System.Configuration;
using Web_TPMTrakDashboard.Cumi.Model;
using Web_TPMTrakDashboard.EnergyModule;
using Util = Web_TPMTrakDashboard.EnergyModule.Util;

namespace Web_TPMTrakDashboard.Cumi
{
    public partial class ProcessParameterSPC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // to do get logical day start and end
                var now = DateTime.Now;
                var first = new DateTime(now.Year, now.Month, 1);
                txtFromDateMonth.Text = first.Year.ToString();
                txtToDateMonth.Text = first.Month.ToString("MMM");
                BindAllMachines();
            }
        }
        private void BindAllMachines()
        {
            try
            {
                List<string> list = CumiDBAccess.BindAllMachinesSPC();
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPlant: " + ex.Message);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Web_TPMTrakDashboard.EnergyModule.LineChartSeries GetDailySPCDataGraph(string machineid, string fromDate, string toDate)
        {

            var MonthStart = new DateTime(Convert.ToInt32(fromDate), Convert.ToInt32(toDate), 1);
            var MonthEnd = MonthStart.AddMonths(1).AddDays(-1);

            double[] DataV1 = null;
            double[] DataV2 = null;
            double[] DataV3 = null;
            double[] DataV4 = null;

            double[] DataV5 = null;
            double[] DataV6 = null;
            double[] DataV7 = null;

            List<double[]> DataListV1 = new List<double[]>();
            List<double[]> DataListV2 = new List<double[]>();
            List<double[]> DataListV3 = new List<double[]>();
            List<double[]> DataListV4 = new List<double[]>();

            List<double[]> DataListV5 = new List<double[]>();
            List<double[]> DataListV6 = new List<double[]>();
            List<double[]> DataListV7 = new List<double[]>();

            //DateTime fromDateTime = DateTime.Now;
            //DateTime toDateTime = DateTime.Now;
            //fromDateTime = EnergyModule.Util.GetDateTime(fromDate);
            //toDateTime = EnergyModule.Util.GetDateTime(toDate);
            //fromDate = fromDateTime.ToString("yyyy-MM-dd");
            //toDate = toDateTime.ToString("yyyy-MM-dd");

            fromDate = MonthStart.ToString("yyyy-MM-dd");
            toDate = MonthEnd.ToString("yyyy-MM-dd");

            DataTable dt1 = new DataTable();

            var SPCBarChart = new LineChartSeries();

            var SeriesV1 = new DataSeries();
            var SeriesV2 = new DataSeries();
            var SeriesV3 = new DataSeries();
            var SeriesV4 = new DataSeries();

            var SeriesV5 = new DataSeries();
            var SeriesV6 = new DataSeries();
            var SeriesV7 = new DataSeries();

            //List<ProcessParameterSPCDataInfo> spcdata = CumiDBAccess.GetProcessParameterSPCData(fromDate, toDate, machineid, out dt);
            DataTable dt = CumiDBAccess.GetProcessParameterSPCData(fromDate, toDate, machineid, out dt1);

            try
            {
                //ToConvertListToDataTable toConvert = new ToConvertListToDataTable();
                //dt = toConvert.ToDataTable(spcdata);
                DateTime dtime1 = new DateTime();
                if (dt != null && dt.Rows.Count > 0)
                {
                    var TargetValue = 0.0;
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            DataV5 = new double[2];
                            DataV6 = new double[2];
                            DataV7 = new double[2];

                            DataV5[1] = Convert.ToDouble(dt1.Rows[i]["AvgKwh"]);
                            DataV5[0] = (double)(dtime1 - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            DataListV5.Add(DataV5);

                            DataV6[1] = Convert.ToDouble(dt1.Rows[i]["AvgDailyTonnage"]);
                            DataV6[0] = (double)(dtime1 - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            DataListV6.Add(DataV6);

                            DataV7[1] = Convert.ToDouble(dt1.Rows[i]["AvgSPC"]);
                            DataV7[0] = (double)(dtime1 - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            DataListV7.Add(DataV7);

                            //DataV7[1] = Convert.ToDouble(dt1.Rows[i]["SPCTarget"]);
                            //DataV7[0] = (double)(dtime1 - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            //DataListV7.Add(DataV7);
                            var target = dt1.Rows[i]["SPCTarget"];
                            TargetValue = target != DBNull.Value ? Convert.ToDouble(dt1.Rows[i]["SPCTarget"]) : TargetValue;
                        }
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataV1 = new double[2];
                        DataV2 = new double[2];
                        DataV3 = new double[2];
                        DataV4 = new double[2];

                        DateTime dtime = new DateTime();
                        dtime = Util.GetDateTime(dt.Rows[i]["StartDate"].ToString());


                        dtime1 = i == 0 ? Util.GetDateTime(dt.Rows[i]["StartDate"].ToString()) : DateTime.Now;

                        DataV1[1] = Convert.ToDouble(dt.Rows[i]["Kwh"]);
                        DataV1[0] = (dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        DataListV1.Add(DataV1);

                        DataV2[1] = Convert.ToDouble(dt.Rows[i]["Dailytonnage"]);
                        DataV2[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        DataListV2.Add(DataV2);

                        DataV3[1] = Convert.ToDouble(dt.Rows[i]["SPC"]);
                        DataV3[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        DataListV3.Add(DataV3);

                        if (TargetValue != 0.0 && (i == 0 || i == dt.Rows.Count - 1))
                        {
                            DataV4[1] = Convert.ToDouble(TargetValue);
                            DataV4[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            DataListV4.Add(DataV4);
                        }
                    }
                   
                   
                   

                    List<long> cat = new List<long>();
                    SeriesV1.data = DataListV1;
                    SeriesV1.name = "KWH";
                    SeriesV1.type = "column";
                  

                    SeriesV2.data = DataListV2;
                    SeriesV2.name = "Daily Tonnage";
                    SeriesV2.type = "column";

                    SeriesV3.data = DataListV3;
                    SeriesV3.name = "SPC";
                    SeriesV3.type = "line";
                    SeriesV3.yAxis = 1;

                    SeriesV4.data = DataListV4;
                    SeriesV4.name = "Target";
                    SeriesV4.type = "line";
                    SeriesV4.color = "blue";
                    SeriesV4.yAxis = 1;

                    SeriesV5.data = DataListV5;
                    SeriesV5.name = "Average Kwh";

                    SeriesV6.data = DataListV6;
                    SeriesV6.name = "Average Daily Tonnage";

                    SeriesV7.data = DataListV7;
                    SeriesV7.name = "Average SPC";

                    SPCBarChart.series = new List<DataSeries>();
                    SPCBarChart.series.Add(SeriesV1);
                    SPCBarChart.series.Add(SeriesV2);
                    SPCBarChart.series.Add(SeriesV3);
                    SPCBarChart.series.Add(SeriesV4);
                    SPCBarChart.Category = cat;
                    SPCBarChart.seriesMonthly.Add(SeriesV5);
                    SPCBarChart.seriesMonthly.Add(SeriesV6);
                    SPCBarChart.seriesMonthly.Add(SeriesV7);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

            return SPCBarChart;
        }
    }
}