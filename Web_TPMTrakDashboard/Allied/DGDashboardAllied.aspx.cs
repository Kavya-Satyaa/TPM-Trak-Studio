using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Allied
{
    public partial class DGDashboardAllied : System.Web.UI.Page
    {
        public int fontSize = 20;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindMachines();
                BindGrid();
            }
        }
        private void BindMachines()
        {
            var machines = AlliedDBAccess.GetMachines();
            ddlMachineID.DataSource = machines;
            ddlMachineID.DataBind();
        }
        private void BindGrid()

        {
            try
            {
                DataTable dt = new DataTable();
                //var details = AlliedDBAccess.GetDashBoardDetails(ddlMachineID.SelectedValue.ToString(), txtFromdate.Text, txtToDate.Text/*,out dashboard*/);
                //DateTime Fromdate = Util.GetDateTime(txtFromdate.Text);
                //DateTime Todate = Util.GetDateTime(txtToDate.Text);
                dt = AlliedDBAccess.GetDashboarddetails(ddlMachineID.SelectedValue.ToString(), txtFromdate.Text, txtToDate.Text);
                lvDashboardDetails.DataSource = dt;
                lvDashboardDetails.DataBind();
                Session["DashboardDetails"] = dt;
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }


        protected void btnView_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<DashboardChartData> GetDashboardData(string machineid, string fromdate, string todate)
        {
            List<DashboardChartData> chartDataList = new List<DashboardChartData>();
            try
            {
                DashboardChartData chartData = new DashboardChartData();
                //List<DashBoardData> Data = new List<DashBoardData>();
                //Data = DataBaseAccess.GetDashBoardDetails(machineid, fromdate, todate);
                DataTable dtData = new DataTable();
                dtData = HttpContext.Current.Session["DashboardDetails"] as DataTable;
                List<DashboardChart> listChartData = new List<DashboardChart>();
                int yAxisValue = 0;
                listChartData.AddRange(getChartSeriesData(machineid, yAxisValue, dtData, "red"));
                yAxisValue++;
                chartData.Category = new List<string>() { machineid };
                chartData.data = listChartData;
                chartDataList.Add(chartData);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return chartDataList;
        }
        private static List<DashboardChart> getChartSeriesData(string machineid, int yvalue, DataTable dashboardList, string color)
        {
            List<DashboardChart> listChartData = new List<DashboardChart>();
            try
            {
                DataTable filterDt = new DataTable();
                //ignore event end time ==null and take last recond
                var listdata = dashboardList.AsEnumerable().Where(k => k.Field<string>("MachineID").Equals(machineid, StringComparison.OrdinalIgnoreCase) && k.Field<DateTime?>("EventEndtime") != null);
                if (listdata.Any())
                {
                    filterDt = listdata.CopyToDataTable();
                    int lastRowIndex = dashboardList.Rows.Count - 1;
                    if (dashboardList.Rows[lastRowIndex]["EventEndtime"].ToString() == "")
                    {
                        filterDt.ImportRow(dashboardList.Rows[lastRowIndex]);
                    }
                }
                //dashboardList = listdata.ToList();
                DateTime? lastDateTime = null;
                if (filterDt.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (DataRow item in filterDt.Rows)
                    {
                        DashboardChart data = new DashboardChart();
                        string startdate = item["EventStarttime"].ToString();
                        string enddate = item["EventEndtime"].ToString();
                        data.y = yvalue;
                        data.startTime = startdate;
                        data.EndTime = enddate;
                        DateTime startdatetime = Util.GetDateTime(startdate);
                        DateTime enddatetime = Util.GetDateTime(enddate);
                        data.x = (double)(startdatetime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        data.x2 = (double)(enddatetime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        data.color = "green";
                        data.borderColor = "green";
                        listChartData.Add(data);

                        //start-end --> green
                        //end-start  --> red (this record we need to insert manually) 
                        if (i < (filterDt.Rows.Count - 1))
                        {
                            data = new DashboardChart();
                            startdatetime = Util.GetDateTime(filterDt.Rows[i + 1]["EventStarttime"].ToString());
                            data.y = yvalue;
                            data.startTime = enddate;
                            data.EndTime = filterDt.Rows[i + 1]["EventStarttime"].ToString();
                            data.color = "red";
                            data.borderColor = "red";
                            data.x = (double)(enddatetime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                            data.x2 = (double)(startdatetime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                            listChartData.Add(data);
                        }
                       
                        i++;
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