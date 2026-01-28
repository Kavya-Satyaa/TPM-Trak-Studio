using Elmah;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class Statistics : System.Web.UI.Page
    {
        private string machineid, fromDate, toDate, component, componentOperation, operation;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                if (Request.QueryString["machineId"] != null && Request.QueryString["component"] != null)
                {
                    btnRefreshHistogramChart.Visible = false;
                    Session["CuttingDatadatasource"] = null;
                    Session["StatisticsChartData"] = null;
                    machineid = Request.QueryString["machineId"].ToString();
                    fromDate = Request.QueryString["fromdate"].ToString();
                    toDate = Request.QueryString["Todate"].ToString();
                    componentOperation = Request.QueryString["component"].ToString();
                    string[] componentVal = componentOperation.Split('|');
                    component = componentVal[0].Trim();
                    operation = componentVal[1].Trim();
                    Statistics_Load();
                }
            }

        }
        private void Statistics_Load()
        {
            try
            {
                DateTime FromDate = DateTime.Now;
                DateTime ToDate = DateTime.Now;
                FromDate = Util.GetDateTime(fromDate);
                ToDate = Util.GetDateTime(toDate);

                string fromDate1 = FromDate.ToSQLDateTimeFormat();
                string toDate1 = ToDate.ToSQLDateTimeFormat();
                VDGComponentStatisticValues vals = VDGDataBaseAccess.GetComponentStats(fromDate1, toDate1, machineid, component, operation);
                if (vals != null)
                {
                    lblStd.Text = vals.CuttingStTime;
                    lblMax.Text = vals.CuttingMax;
                    lblMin.Text = vals.CuttingMin;
                    lblAvg.Text = vals.CuttingAvgTime;
                    lblRang.Text = vals.CuttingRange;
                    lblLoadstd.Text = vals.LoadUnLoadStTime;
                    lblLoadMax.Text = vals.LoadUnLoadMax;
                    lblLoadMin.Text = vals.LoadUnLoadMin;
                    lblLoadAvg.Text = vals.LoadUnLoadAvgTime;
                    lblLoadRang.Text = vals.LoadUnLoadRange;
                }

                lblMachineName.Text = machineid + " - " + component + " < " + operation + " > ";
                // BindgridData();

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }

        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            Session["CuttingDatadatasource"] = null;
            Session["StatisticsChartData"] = null;
            BindgridData();
        }

        private void BindgridData()
        {
            try
            {
                if (Variance.SelectedValue != "Most Frequent Occ")
                {
                    int value;
                    int.TryParse(txtboxVal.Text, out value);

                    if (!int.TryParse(txtboxVal.Text, out value))
                    {
                        lblMessages.Text = GetGlobalResourceObject("CommanResource", "PleaseEnterOnlyNumbers").ToString();
                        return;
                    }
                }
                if (Request.QueryString["machineId"] != null && Request.QueryString["component"] != null)
                {

                    machineid = Request.QueryString["machineId"].ToString();
                    fromDate = Request.QueryString["fromdate"].ToString();
                    toDate = Request.QueryString["Todate"].ToString();
                    componentOperation = Request.QueryString["component"].ToString();
                    string[] componentVal = componentOperation.Split('|');
                    component = componentVal[0].Trim();
                    operation = componentVal[1].Trim();
                }

                DateTime FromDate = DateTime.Now;
                DateTime ToDate = DateTime.Now;
                FromDate = Util.GetDateTime(fromDate);
                ToDate = Util.GetDateTime(toDate);

                string fromDate1 = FromDate.ToSQLDateTimeFormat();
                string toDate1 = ToDate.ToSQLDateTimeFormat();
                DataTable dt = null;
                if (Session["CuttingDatadatasource"] == null)
                {
                    dt = VDGDataBaseAccess.GetComponentsVariance(fromDate1, toDate1, machineid, component, operation, VarianceType.SelectedValue.ToString(), Variance.SelectedValue.ToString(), txtboxVal.Text.ToString());
                    Session["CuttingDatadatasource"] = dt;
                    if (!Variance.SelectedValue.ToString().Equals("Most Frequent Occ", StringComparison.OrdinalIgnoreCase))
                    {
                        string chartDataColName = "";
                        if (dt.Columns.Contains("CycleTime"))
                        {
                            chartDataColName = "CycleTime";
                        }
                        else
                        {
                            chartDataColName = "Loadunload";
                        }
                        Session["StatisticsChartData"] = dt.AsEnumerable().Select(k => Convert.ToDouble(k.Field<int>(chartDataColName))).ToList();
                    }
                }
                else
                {
                    dt = Session["CuttingDatadatasource"] as DataTable;

                }
                if (Variance.SelectedValue.ToString().Equals("Most Frequent Occ", StringComparison.OrdinalIgnoreCase))
                {
                    lblNoOfComp.Text = string.Empty;
                    //  pnlMostFreqOcc.BringToFront();

                    lblMostFreOcc.Text =
                        "----  " + GetLocalResourceObject("MostFrequentlyOccured").ToString() + "  ----" + "<br/><br/>" + VarianceType.SelectedValue.ToString() + " is : " +
                        dt.Rows[0]["val"] + " secs.\n" + "<br/>" + GetLocalResourceObject("NoOfOccurencesis").ToString() + " : " +
                        dt.Rows[0]["FreqOcc"];
                    gridCuttingData.Visible = false;
                    divMostFreOccScroll.Visible = true;
                    gridLoadUnload.Visible = false;
                    btnRefreshHistogramChart.Visible = false;
                }
                else
                {
                    if (VarianceType.SelectedValue.ToString().Equals("Cutting Time", StringComparison.OrdinalIgnoreCase))
                    {
                        lblNoOfComp.Text = " :: " + dt.Rows.Count + GetLocalResourceObject("Componentsarefound").ToString() + " :: ";
                        gridCuttingData.DataSource = dt;
                        gridCuttingData.DataBind();
                        gridCuttingData.Visible = true;
                        divMostFreOccScroll.Visible = false;
                        gridLoadUnload.Visible = false;
                    }
                    else
                    {
                        lblNoOfComp.Text = " :: " + dt.Rows.Count + GetLocalResourceObject("Componentsarefound").ToString() + " :: ";
                        gridLoadUnload.DataSource = dt;
                        gridLoadUnload.DataBind();
                        gridCuttingData.Visible = false;
                        divMostFreOccScroll.Visible = false;
                        gridLoadUnload.Visible = true;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        btnRefreshHistogramChart.Visible = true;
                    }
                    else
                    {
                        btnRefreshHistogramChart.Visible = false;
                    }
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "bindchart", "BindChart();", true);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        protected void OnPageIndexChangingCutting(object sender, GridViewPageEventArgs e)
        {
            gridCuttingData.PageIndex = e.NewPageIndex;
            BindgridData();
        }
        protected void OnPageIndexChangingLU(object sender, GridViewPageEventArgs e)
        {
            gridLoadUnload.PageIndex = e.NewPageIndex;
            BindgridData();
        }

        public SortDirection dir
        {
            get
            {
                if (Session["dirState"] == null)
                {
                    Session["dirState"] = SortDirection.Ascending;
                }
                return (SortDirection)Session["dirState"];
            }
            set
            {
                Session["dirState"] = value;
            }

        }
        protected void gridCuttingData_Sorting(object sender, GridViewSortEventArgs e)
        {

            DataTable dt = null;
            dt = Session["CuttingDatadatasource"] as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                string SortDir = string.Empty;
                if (dir == SortDirection.Ascending)
                {
                    dir = SortDirection.Descending;
                    SortDir = "Desc";
                }
                else
                {
                    dir = SortDirection.Ascending;
                    SortDir = "Asc";
                }
                DataView sortedView = new DataView(dt);
                sortedView.Sort = e.SortExpression + " " + SortDir;
                gridCuttingData.DataSource = sortedView;
                gridCuttingData.DataBind();
                btnRefreshHistogramChart_Click(null, null);
            }
            else
            {
                BindgridData();
            }
        }

        protected void btnRefreshHistogramChart_Click(object sender, EventArgs e)
        {
            try
            {
                GridView grid = null;
                if (VarianceType.SelectedValue == "Cutting Time")
                {
                    grid = gridCuttingData;
                }
                else if (VarianceType.SelectedValue == "Load Unload")
                {
                    grid = gridLoadUnload;
                }
                if (grid != null)
                {
                    List<double> chartData = new List<double>();
                    for (int i = 0; i < grid.Rows.Count; i++)
                    {
                        if ((grid.Rows[i].FindControl("chkSelect") as CheckBox).Checked)
                        {
                            chartData.Add(Convert.ToDouble(grid.Rows[i].Cells[2].Text.ToString()));
                        }
                    }
                    HttpContext.Current.Session["StatisticsChartData"] = chartData;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "bindchart", "BindChart();", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void gridLoadUnload_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = null;
            dt = Session["CuttingDatadatasource"] as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                string SortDir = string.Empty;
                if (dir == SortDirection.Ascending)
                {
                    dir = SortDirection.Descending;
                    SortDir = "Desc";
                }
                else
                {
                    dir = SortDirection.Ascending;
                    SortDir = "Asc";
                }
                DataView sortedView = new DataView(dt);
                sortedView.Sort = e.SortExpression + " " + SortDir;
                gridLoadUnload.DataSource = sortedView;
                gridLoadUnload.DataBind();
                btnRefreshHistogramChart_Click(null, null);
            }
            else
            {
                BindgridData();
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static ObservableCollection<HistogramData> getHistogramValueFrequency()
        {
            ObservableCollection<HistogramData> HistogramGraphData = new ObservableCollection<HistogramData>();
           // DataTable chartDT = new DataTable();
            double[] data = null;

            try
            {
                if (HttpContext.Current.Session["StatisticsChartData"] != null)
                {
                    List<double> Xseries = (HttpContext.Current.Session["StatisticsChartData"] as List<double>);
                    if (Xseries.Count > 0)
                    {
                        //data = new double[chartDT.Rows.Count];
                        //for (int i = 0; i < chartDT.Rows.Count; i++)
                        //{
                        //    if (chartDT.Columns.Contains("CycleTime"))
                        //    {
                        //        data[i] = Convert.ToDouble(chartDT.Rows[i]["CycleTime"].ToString());
                        //    }
                        //    else
                        //    {
                        //        data[i] = Convert.ToDouble(chartDT.Rows[i]["Loadunload"].ToString());
                        //    }
                        //}

                       // List<double> Xseries = chartDT.ToList();
                        double Mean = Math.Round(Xseries.Mean(), 2);
                        HttpContext.Current.Session["HistoMean"] = Mean;
                        double Variance = Math.Round(Xseries.Variance(), 2);
                        double StdDeviation = Math.Round(Xseries.StandardDeviation(), 2);
                        HttpContext.Current.Session["HistoStdDeviation"] = StdDeviation;
                        double Min, Max, Diff, Interval = 0.0;
                        Min = Math.Round(Mean - (4 * StdDeviation),0);
                        Max = Math.Round(Mean + (4 * StdDeviation), 0);
                        Diff = Max - Min;
                        Interval = Math.Round((Diff / 10), 0);
                        //Min = Xseries.Min();
                        //Min -= Min % 10;
                        //Max = Xseries.Max();
                        //Max = Max + (10 - (Max % 10));
                        HistogramGraphData.Add(new HistogramData() { Value = Math.Round(Min, 0) });
                        //for (int i = 0; i < 100; i++)
                        //{
                        //    if (HistogramGraphData[i].Value >= Max)
                        //        break;
                        //    HistogramGraphData.Add(new HistogramData() { Value = Math.Round(HistogramGraphData[i].Value + Interval, 2) });
                        //}
                        int histoCount = 0;
                        //Interval = 20;
                        while ((HistogramGraphData[histoCount].Value < Max))
                        {
                            if (HistogramGraphData[histoCount].Value >= Max)
                                break;
                            HistogramGraphData.Add(new HistogramData() { Value = Math.Round(HistogramGraphData[histoCount].Value + Interval, 0) });
                            histoCount++;
                        }
                        foreach (double value in Xseries)
                        {
                            for (int i = 0; i < HistogramGraphData.Count - 1; i++)
                            {
                                if (value >= HistogramGraphData[i].Value && value < HistogramGraphData[i + 1].Value)
                                {
                                    HistogramGraphData[i].Frequency++;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            HttpContext.Current.Session["HistoValueFrequency"] = HistogramGraphData;
            return HistogramGraphData;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static ObservableCollection<HistogramData> getHistogramLineSeriesData()
        {
            List<int> FreqList = new List<int>();
            List<int> AbsCeilNormDistList = new List<int>();
            ObservableCollection<HistogramData> normalDistDataFirst = new ObservableCollection<HistogramData>();
            ObservableCollection<HistogramData> HistogramGraphData = new ObservableCollection<HistogramData>();
            if (HttpContext.Current.Session["HistoValueFrequency"] != null)
            {
                HistogramGraphData = (ObservableCollection<HistogramData>)HttpContext.Current.Session["HistoValueFrequency"];


                if (HistogramGraphData != null)
                {
                    FreqList.Clear(); AbsCeilNormDistList.Clear();
                    foreach (HistogramData item in HistogramGraphData)
                    {
                        double nd = StatisticalFormulas.GetNormalDistribution(item.Value, HttpContext.Current.Session["HistoMean"] != null ? Convert.ToDouble(HttpContext.Current.Session["HistoMean"].ToString()) : 0.0, HttpContext.Current.Session["HistoStdDeviation"] != null ? Convert.ToDouble(HttpContext.Current.Session["HistoStdDeviation"].ToString()) : 0.0, false);
                        if (double.IsNaN(nd))
                        {
                            nd = 0;
                        }
                        nd = Math.Round(nd * 10000, 2);

                        normalDistDataFirst.Add(new HistogramData() { Value = item.Value, Frequency = nd });
                    }
                }
            }
            return normalDistDataFirst;
        }

    }

    public class HistogramData
    {
        public HistogramData()
        {
            Frequency = 0;
        }

        public double Frequency { get; set; }
        public double Value { get; set; }
    }
}