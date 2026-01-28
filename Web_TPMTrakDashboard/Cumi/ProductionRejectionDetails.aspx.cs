using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.Cumi.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Cumi
{
    public partial class ProductionRejectionDetails : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // timer.Enabled = false;
                //   cbAutorefresh.Checked = true;
                ViewState["AutorefreshValue"] = true;
                txtYear.Text = DateTime.Now.ToString("yyyy");
                txtMonth.Text = DateTime.Now.ToString("MM");
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindPlant();
                BindShift();
                hfSelectedMenu.Value = "HourlyDataMenu";
                if (Request.QueryString["ScreenType"] == null)
                {
                    ViewState["ScreenType"] = "AcceptedScreen";
                }
                else
                {
                    ViewState["ScreenType"] = Request.QueryString["ScreenType"].ToString();
                }
                if (ViewState["ScreenType"].ToString().Equals("RejectionScreen", StringComparison.OrdinalIgnoreCase))
                {
                    Page.Title = "Quality Details";
                }
                else
                {
                    Page.Title = "Production Details";
                }
                btnView_Click(null, null);
                HelperClass.openFunction(this, "setActiveSubmenuValue");
            }
        }
        private void BindShift()
        {
            try
            {
                List<string> shiftDetails = DataBaseAccess.GetAllShifts("");
                //List<PDTData> shiftDetails = DataBaseAccess.getShiftTimeDetails(DateTime.Now);
                //List<ListItem> list = new List<ListItem>();
                //foreach (PDTData data in shiftDetails)
                //{
                //    list.Add(new ListItem() { Text = data.ShiftName, Value = data.ShiftID + ";;" + data.FromDateTime + ";;" + data.ToDateTime });
                //}
                ddlShift.DataSource = shiftDetails;
                //ddlShift.DataTextField = "Text";
                //ddlShift.DataValueField = "Value";
                ddlShift.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindShift: " + ex.Message);
            }
        }
        private void BindPlant()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlantsForPlantInfo();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();

                BindMachine();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPlant: " + ex.Message);
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = CumiDBAccess.GetAllMachinedByPlant(ddlPlant.SelectedValue);
                lbMachine.DataSource = list;
                lbMachine.DataBind();
                if (lbMachine.Items.Count > 0)
                {
                    //lbMachine.Items[0].Selected = true;
                    foreach (ListItem item in lbMachine.Items)
                    {
                        item.Selected = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachine: " + ex.Message);
            }
        }
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindMachine();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlPlant_SelectedIndexChanged: " + ex.Message);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                BindDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnView_Click: " + ex.Message);
            }
        }
        protected void btnMenu_Click(object sender, EventArgs e)
        {
            try
            {
                BindDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnMenu_Click: " + ex.Message);
            }

        }

        protected void rblViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                BindDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("rblViewType_SelectedIndexChanged: " + ex.Message);
            }
        }

        private void BindDetails()
        {
            try
            {
                string viewType = rblViewType.SelectedValue;
                string selectedMenu = hfSelectedMenu.Value;
                string procName = "";
                string screenType = ViewState["ScreenType"].ToString();
                // tdFromDate.Visible = false;
                tdFromDateControl.Visible = false;
                // tdToDate.Visible = false;
                tdToDateControl.Visible = false;
                // tdYear.Visible = false;
                tdYearControl.Visible = false;
                //  tdShift.Visible = false;
                tdShiftControl.Visible = false;
                //  tdMonth.Visible = false;
                tdMonthControl.Visible = false;
                tdAggLiveControl.Visible = false;
                spanDate.InnerText = "From Date";
                ddlShift.Enabled = true;
                txtFromDate.Enabled = true;
                tdAutorefresh.Visible = false;
                chartDiv.Visible = true;


                if ((selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase) || selectedMenu.Equals("ShiftDataMenu", StringComparison.OrdinalIgnoreCase)) && !viewType.Equals("Report", StringComparison.OrdinalIgnoreCase))
                {
                    cbAutorefresh.Checked = (bool)ViewState["AutorefreshValue"];
                }
                else
                {
                    cbAutorefresh.Checked = false;
                }

                if (cbAutorefresh.Checked)
                {
                    int inetrval = 1000 * Convert.ToInt32(WebConfigurationManager.AppSettings["CumiProdAndRejInterval"].ToString());
                    timer.Enabled = true;
                    timer.Interval = inetrval;
                }
                else
                {
                    timer.Enabled = false;
                }

                if (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase) || selectedMenu.Equals("ShiftDataMenu", StringComparison.OrdinalIgnoreCase))
                {


                    //  tdFromDate.Visible = true;
                    tdFromDateControl.Visible = true;
                    chartDiv.Visible = false;

                    if (viewType.Equals("Report", StringComparison.OrdinalIgnoreCase))
                    {
                        //  tdToDate.Visible = true;
                        tdToDateControl.Visible = true;


                        //if (Math.Abs((Util.GetDateTime(DateTime.Now.ToString("dd-MM-yyyy")) - Util.GetDateTime(txtFromDate.Text)).TotalDays) > 7) // in dashboard if selected 1 month back date and in report 7 days restriction is there 
                        //{
                        //    txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                        //    txtToDate.Text = txtFromDate.Text;
                        //}
                    }
                    else
                    {
                        ViewState["AutorefreshValue"] = cbAutorefresh.Checked;
                        tdAutorefresh.Visible = true;
                        if (cbAutorefresh.Checked)
                        {
                            ddlShift.Enabled = false;
                            txtFromDate.Enabled = false;
                            txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                            txtToDate.Text = txtFromDate.Text;
                            List<ListItem> shiftDetails = CumiDBAccess.GetCurrentShiftDetails();
                            string currentShift = "";
                            if (shiftDetails.Count > 0)
                            {
                                currentShift = shiftDetails.Where(x => x.Text == "ShiftName").Select(x => x.Value).FirstOrDefault();
                            }
                            if (ddlShift.Items.FindByValue(currentShift) != null)
                            {
                                ddlShift.SelectedValue = currentShift;
                            }
                        }
                        else
                        {
                            txtToDate.Text = txtFromDate.Text;
                            if (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase) || (selectedMenu.Equals("ShiftDataMenu", StringComparison.OrdinalIgnoreCase) && rblAggLiveSSelection.SelectedValue == "Live"))
                            {
                                if (Math.Abs((Util.GetDateTime(DateTime.Now.ToString("dd-MM-yyyy")) - Util.GetDateTime(txtFromDate.Text)).TotalDays) > 7) // in dashboard if selected 1 month back date and in report 7 days restriction is there 
                                {
                                    txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                                    txtToDate.Text = txtFromDate.Text;
                                }
                            }


                        }

                        //  tdShift.Visible = true;
                        tdShiftControl.Visible = true;
                        spanDate.InnerText = "Date";

                        if (selectedMenu.Equals("ShiftDataMenu", StringComparison.OrdinalIgnoreCase))
                        {
                            tdAggLiveControl.Visible = true;
                        }



                    }
                    if (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase))
                    {
                        procName = "[SP_HourlyProductionAndRejectionData_CUMI]";
                    }
                    else
                    {
                        if (viewType.Equals("Report", StringComparison.OrdinalIgnoreCase))
                        {
                            procName = "[SP_AggregatedShiftProductionAndRejectionData_CUMI]";
                        }
                        else
                        {
                            if (tdAggLiveControl.Visible)
                            {
                                if (rblAggLiveSSelection.SelectedValue == "Aggregate")
                                {
                                    procName = "[SP_AggregatedShiftProductionAndRejectionData_CUMI]";
                                }
                                else if (rblAggLiveSSelection.SelectedValue == "Live")
                                {
                                    //txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                                    procName = "[SP_ShiftProductionAndRejectionData_CUMI]";
                                }
                                else
                                {
                                    if (txtFromDate.Text == DateTime.Now.ToString("dd-MM-yyyy"))
                                    {
                                        procName = "[SP_ShiftProductionAndRejectionData_CUMI]";
                                    }
                                    else
                                    {
                                        procName = "[SP_AggregatedShiftProductionAndRejectionData_CUMI]";
                                    }
                                }
                            }
                            else
                            {
                                procName = "[SP_ShiftProductionAndRejectionData_CUMI]";
                            }
                        }
                    }
                }
                else if (selectedMenu.Equals("DayDataMenu", StringComparison.OrdinalIgnoreCase) || selectedMenu.Equals("WeekDataMenu", StringComparison.OrdinalIgnoreCase))
                {

                    // tdYear.Visible = true;
                    tdYearControl.Visible = true;
                    // tdMonth.Visible = true;
                    tdMonthControl.Visible = true;
                    if (selectedMenu.Equals("DayDataMenu", StringComparison.OrdinalIgnoreCase))
                    {
                        procName = "[SP_DaywiseProductionAndRejectionData_CUMI]";
                    }
                    else
                    {
                        procName = "[SP_WeeklyProductionAndRejectionData_CUMI]";

                    }
                }
                else if (selectedMenu.Equals("MonthDataMenu", StringComparison.OrdinalIgnoreCase))
                {

                    // tdYear.Visible = true;
                    tdYearControl.Visible = true;

                    procName = "[SP_MonthwiseProductionAndRejectionData_CUMI]";
                }

                ViewState["Plant"] = ddlPlant.SelectedValue;
                ViewState["FromDate"] = txtFromDate.Text;
                ViewState["ToDate"] = txtToDate.Text;
                ViewState["Shift"] = ddlShift.SelectedValue;
                ViewState["Year"] = txtYear.Text;
                ViewState["Month"] = txtMonth.Text;
                string machines = "";
                foreach (ListItem item in lbMachine.Items)
                {
                    if (item.Selected)
                    {
                        if (machines == "")
                        {
                            machines = "'" + item.Value + "'";
                        }
                        else
                        {
                            machines += ",'" + item.Value + "'";
                        }
                    }

                }
                ViewState["Machine"] = machines;

                DataTable dtTotal = new DataTable();
                DataTable dtRightTotal = new DataTable();
                DataTable dtRightTotal_1 = new DataTable();
                DataTable dt = CumiDBAccess.GetProductionRejectionDetails(procName, screenType, viewType, ViewState["Plant"].ToString(), ViewState["Machine"].ToString(), ViewState["FromDate"].ToString(), ViewState["ToDate"].ToString(), ViewState["Shift"].ToString(), ViewState["Year"].ToString(), ViewState["Month"].ToString(), false, out dtTotal, out dtRightTotal, out dtRightTotal_1);


                Session["P_R_Details"] = dt;
                Session["TotalDetails"] = dtTotal;


                bool dateColumnVisibility = false, shiftColumnVisibility = false;
                if ((selectedMenu.Equals("ShiftDataMenu", StringComparison.OrdinalIgnoreCase) || selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase)) && (viewType.Equals("Report", StringComparison.OrdinalIgnoreCase) || viewType.Equals("Dashboard", StringComparison.OrdinalIgnoreCase)))
                {
                    if ((selectedMenu.Equals("ShiftDataMenu", StringComparison.OrdinalIgnoreCase) || selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase)) && (viewType.Equals("Report", StringComparison.OrdinalIgnoreCase)))
                    {
                        dateColumnVisibility = true;
                    }

                    if (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase) && (viewType.Equals("Report", StringComparison.OrdinalIgnoreCase) || viewType.Equals("Dashboard", StringComparison.OrdinalIgnoreCase)))
                    {
                        shiftColumnVisibility = true;
                    }

                    if (selectedMenu.Equals("ShiftDataMenu", StringComparison.OrdinalIgnoreCase))
                    {
                        dt.Columns.Add("ShiftName", typeof(string));
                        dtTotal.Columns.Add("ShiftName", typeof(string));
                        dtRightTotal.Columns.Add("ShiftName", typeof(string));
                    }
                }
                else
                {
                    dt.Columns.Add("date", typeof(string));
                    dtTotal.Columns.Add("date", typeof(string));
                    dtRightTotal.Columns.Add("date", typeof(string));

                    dt.Columns.Add("ShiftName", typeof(string));
                    dtTotal.Columns.Add("ShiftName", typeof(string));
                    dtRightTotal.Columns.Add("ShiftName", typeof(string));
                }

                var distinctRowDetails = dt.AsEnumerable().Select(x => new { date = x.Field<string>("date"), shift = x.Field<string>("ShiftName"), machine = x.Field<string>("MachineID") }).Distinct();
                var distinctColumnDetails = dt.AsEnumerable().Select(x => new { name = x.Field<string>("Name") }).Distinct().ToList();

                List<MachineTypeDetails> list = new List<MachineTypeDetails>();
                MachineTypeDetails dataContentDetails = null;
                MachineTypeDetails dataHeaderDetails = new MachineTypeDetails();
                MachineTypeDetails dataTotalDetails = new MachineTypeDetails();

                int i = 0;
                string prviousDate = "", previousShift = "";
                foreach (var row in distinctRowDetails)
                {
                    bool isLastMachine = false;
                    if (i == 0)
                    {
                        dataHeaderDetails.HeaderVisibility = "table-cell";
                        if (!selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase))
                        {
                            dataHeaderDetails.PlanActualHeaderVisibility = "table-cell";
                        }
                        if (dateColumnVisibility)
                        {
                            dataHeaderDetails.DateHeaderVisibility = "table-cell";
                        }
                        if (shiftColumnVisibility)
                        {
                            dataHeaderDetails.ShiftHeaderVisibility = "table-cell";
                        }
                    }

                    dataContentDetails = new MachineTypeDetails();
                    dataContentDetails.MachineID = row.machine;
                    dataContentDetails.ContentVisibility = "table-cell";
                    if (!selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase))
                    {
                        dataContentDetails.PlanActualConentVisibility = "table-cell";
                        if (screenType.Equals("RejectionScreen", StringComparison.OrdinalIgnoreCase))
                        {
                            dataContentDetails.PlanName = "Production Count";
                            dataContentDetails.ActualName = "No. Of Rejection";
                        }
                        else
                        {
                            dataContentDetails.PlanName = "Plan";
                            dataContentDetails.ActualName = "Actual";
                        }
                        dataContentDetails.MachineIDRowSpanForExport = 2 - 1; //for export
                    }

                    if (dateColumnVisibility)
                    {
                        dataContentDetails.Date = string.IsNullOrEmpty(row.date) ? "" : Util.GetDateTime(row.date).ToString("dd-MM-yyyy");

                        if (prviousDate != dataContentDetails.Date)
                        {
                            int dsmcount = dt.AsEnumerable().Where(x => x.Field<string>("date") == row.date).Select(x => new { m = x.Field<string>("MachineID"), s = x.Field<string>("ShiftName") }).Distinct().Count();
                            dataContentDetails.DateRowSpan = dsmcount + 1 + (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase) && screenType.Equals("RejectionScreen", StringComparison.OrdinalIgnoreCase) ? 1 : 0); // 1 for total // one more condition for hourly weight

                            if (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase))
                            {
                                //dataContentDetails.DateRowSpanForExport = dsmcount + 1 + (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase) && screenType.Equals("RejectionScreen", StringComparison.OrdinalIgnoreCase) ? 1 : 0);
                                dataContentDetails.DateRowSpanForExport = dataContentDetails.DateRowSpan;

                            }
                            else
                            {
                                dataContentDetails.DateRowSpanForExport = dsmcount * 2; // for 1 machine plan and actual is there so *2

                                dataContentDetails.DateRowSpanForExport += 3; // for total 3 rows
                            }


                            dataContentDetails.DateConentVisibility = "table-cell";

                            previousShift = "";
                        }
                        else
                        {
                            dataContentDetails.DateConentVisibility = "none";
                        }
                    }

                    if (shiftColumnVisibility)
                    {
                        dataContentDetails.Shift = row.shift;

                        if (previousShift != dataContentDetails.Shift)
                        {
                            dataContentDetails.ShiftRowSpan = dt.AsEnumerable().Where(x => x.Field<string>("date") == row.date && x.Field<string>("ShiftName") == row.shift).Select(x => x.Field<string>("MachineID")).Distinct().Count();

                            dataContentDetails.ShiftConentVisibility = "table-cell";
                        }
                        else
                        {
                            dataContentDetails.ShiftConentVisibility = "none";
                        }
                    }


                    string lastShiftForDate = dt.AsEnumerable().Where(x => x.Field<string>("date") == row.date).Select(x => x.Field<string>("ShiftName")).LastOrDefault();
                    if (row.machine == dt.AsEnumerable().Where(x => x.Field<string>("date") == row.date && row.shift == lastShiftForDate).Select(x => x.Field<string>("MachineID")).LastOrDefault())
                    {
                        isLastMachine = true;
                    }

                    List<DynamicColumnDetails> dynamicHeaderList = new List<DynamicColumnDetails>();
                    List<DynamicColumnDetails> dynamicContentList = new List<DynamicColumnDetails>();
                    List<DynamicColumnDetails> dynamicTotalList = new List<DynamicColumnDetails>();
                    List<DynamicColumnDetails> dynamicTotalWeightList = new List<DynamicColumnDetails>();
                    DynamicColumnDetails dynamicHeaderDetails = null;
                    DynamicColumnDetails dynamicContentDetails = null;

                    int columnCount = 1;
                    foreach (var column in distinctColumnDetails)
                    {
                        if (i == 0)
                        {
                            dynamicHeaderDetails = new DynamicColumnDetails();
                            dynamicHeaderDetails.DynamicHeaderVisibility = "table";
                            if (selectedMenu.Equals("DayDataMenu", StringComparison.OrdinalIgnoreCase))
                            {
                                dynamicHeaderDetails.HeaderName = "Day " + column.name.Split(' ')[0].Split('-')[2];
                            }
                            else if (selectedMenu.Equals("WeekDataMenu", StringComparison.OrdinalIgnoreCase))
                            {
                                dynamicHeaderDetails.HeaderName = "W " + columnCount;
                            }
                            else
                            {
                                dynamicHeaderDetails.HeaderName = column.name;
                            }
                            dynamicHeaderDetails.HeaderID = column.name;
                            if (!selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase))
                            {
                                dynamicHeaderDetails.HeaderChartVisibility = "block";
                            }
                            dynamicHeaderList.Add(dynamicHeaderDetails);
                        }

                        dynamicContentDetails = new DynamicColumnDetails();
                        if (screenType.Equals("AcceptedScreen", StringComparison.OrdinalIgnoreCase))
                        {
                            if (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase))
                            {
                                dynamicContentDetails.DynamicOneRowVisibility = "table";
                                var valueDetails = dt.AsEnumerable().Where(x => x.Field<string>("MachineID") == row.machine && x.Field<string>("date") == row.date && x.Field<string>("ShiftName") == row.shift && x.Field<string>("Name") == column.name).Select(x => new { value = x.Field<dynamic>("Qty") }).FirstOrDefault();
                                if (valueDetails != null)
                                {
                                    if (valueDetails.value != null)
                                    {
                                        dynamicContentDetails.Value = Convert.ToString(valueDetails.value);
                                        dynamicContentDetails.ValueContentColor = "unset";

                                    }
                                }
                            }
                            else
                            {
                                dynamicContentDetails.DynamicTwoRowsVisibility = "table";
                                var valueDetails = dt.AsEnumerable().Where(x => x.Field<string>("MachineID") == row.machine && x.Field<string>("date") == row.date && x.Field<string>("ShiftName") == row.shift && x.Field<string>("Name") == column.name).Select(x => new { plan = x.Field<dynamic>("PlnQty"), actual = x.Field<dynamic>("Qty") }).FirstOrDefault();
                                if (valueDetails != null)
                                {
                                    if (valueDetails.actual != null)
                                    {
                                        dynamicContentDetails.Actual = Convert.ToString(valueDetails.actual);
                                        dynamicContentDetails.ActualContentColor = "unset";

                                    }
                                    if (valueDetails.plan != null)
                                    {
                                        dynamicContentDetails.Plan = Convert.ToString(valueDetails.plan);
                                        dynamicContentDetails.PlanContentColor = "unset";
                                    }
                                }
                            }
                        }
                        else if (screenType.Equals("RejectionScreen", StringComparison.OrdinalIgnoreCase))
                        {
                            if (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase))
                            {
                                dynamicContentDetails.DynamicOneRowVisibility = "table";
                                var valueDetails = dt.AsEnumerable().Where(x => x.Field<string>("MachineID") == row.machine && x.Field<string>("date") == row.date && x.Field<string>("ShiftName") == row.shift && x.Field<string>("Name") == column.name).Select(x => new { value = x.Field<dynamic>("Qty") }).FirstOrDefault();
                                if (valueDetails != null)
                                {
                                    if (valueDetails.value != null)
                                    {
                                        dynamicContentDetails.Value = Convert.ToString(valueDetails.value);
                                        dynamicContentDetails.ValueContentColor = "unset";

                                    }
                                }
                            }
                            else
                            {
                                dynamicContentDetails.DynamicTwoRowsVisibility = "table";
                                var valueDetails = dt.AsEnumerable().Where(x => x.Field<string>("MachineID") == row.machine && x.Field<string>("date") == row.date && x.Field<string>("ShiftName") == row.shift && x.Field<string>("Name") == column.name).Select(x => new { plan = x.Field<dynamic>("ProdQty"), actual = x.Field<dynamic>("Qty") }).FirstOrDefault();
                                if (valueDetails != null)
                                {
                                    if (valueDetails.actual != null)
                                    {
                                        dynamicContentDetails.Actual = Convert.ToString(valueDetails.actual);
                                        dynamicContentDetails.ActualContentColor = "unset";

                                    }
                                    if (valueDetails.plan != null)
                                    {
                                        dynamicContentDetails.Plan = Convert.ToString(valueDetails.plan);
                                        dynamicContentDetails.PlanContentColor = "unset";
                                    }
                                }
                            }
                        }

                        dynamicContentList.Add(dynamicContentDetails);

                        if (isLastMachine)
                        {
                            dynamicContentDetails = new DynamicColumnDetails();

                            if (screenType.Equals("AcceptedScreen", StringComparison.OrdinalIgnoreCase))
                            {
                                if (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase))
                                {
                                    dynamicContentDetails.DynamicOneRowVisibility = "table";
                                    var totalDetails = dtTotal.AsEnumerable().Where(x => x.Field<string>("date") == row.date && x.Field<string>("Name") == column.name).Select(x => new { value = x.Field<dynamic>("TotalActual") }).FirstOrDefault();

                                    if (totalDetails != null)
                                    {
                                        if (totalDetails.value != null)
                                        {
                                            dynamicContentDetails.Value = Convert.ToString(totalDetails.value);
                                            dynamicContentDetails.ValueContentColor = "unset";

                                        }
                                    }
                                }
                                else
                                {
                                    dynamicContentDetails.DynamicTwoRowsVisibility = "table";
                                    dynamicContentDetails.RowCompletionVisibility = "table-row";
                                    var totalDetails = dtTotal.AsEnumerable().Where(x => x.Field<string>("date") == row.date && x.Field<string>("Name") == column.name).Select(x => new { plan = x.Field<dynamic>("TotalPlan"), actual = x.Field<dynamic>("TotalActual"), rowcompletion = x.Field<dynamic>("TotalCompletion") }).FirstOrDefault();

                                    if (totalDetails != null)
                                    {
                                        if (totalDetails.actual != null)
                                        {
                                            dynamicContentDetails.Actual = Convert.ToString(totalDetails.actual);
                                            dynamicContentDetails.ActualContentColor = "unset";

                                        }
                                        if (totalDetails.plan != null)
                                        {
                                            dynamicContentDetails.Plan = Convert.ToString(totalDetails.plan);
                                            dynamicContentDetails.PlanContentColor = "unset";
                                        }
                                        if (totalDetails.rowcompletion != null)
                                        {
                                            dynamicContentDetails.RowCompletion = Convert.ToString(totalDetails.rowcompletion);
                                            dynamicContentDetails.RowCompletionContentColor = "unset";
                                        }
                                    }
                                }
                            }
                            else if (screenType.Equals("RejectionScreen", StringComparison.OrdinalIgnoreCase))
                            {
                                if (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase))
                                {

                                    dynamicContentDetails.DynamicOneRowVisibility = "table";
                                    var totalDetails = dtTotal.AsEnumerable().Where(x => x.Field<string>("date") == row.date && x.Field<string>("Name") == column.name).Select(x => new { value = x.Field<dynamic>("TotalActual") }).FirstOrDefault();

                                    if (totalDetails != null)
                                    {
                                        if (totalDetails.value != null)
                                        {
                                            dynamicContentDetails.Value = Convert.ToString(totalDetails.value);
                                            dynamicContentDetails.ValueContentColor = "unset";

                                        }
                                    }

                                    DynamicColumnDetails dynamicContentTotalWeightDetails = new DynamicColumnDetails();
                                    dynamicContentTotalWeightDetails.DynamicOneRowVisibility = "table";
                                    totalDetails = dtTotal.AsEnumerable().Where(x => x.Field<string>("date") == row.date && x.Field<string>("Name") == column.name).Select(x => new { value = x.Field<dynamic>("TotalWeight") }).FirstOrDefault();

                                    if (totalDetails != null)
                                    {
                                        if (totalDetails.value != null)
                                        {
                                            dynamicContentTotalWeightDetails.Value = Convert.ToString(totalDetails.value);
                                            dynamicContentTotalWeightDetails.ValueContentColor = "unset";

                                        }
                                    }
                                    dynamicTotalWeightList.Add(dynamicContentTotalWeightDetails);
                                }
                                else
                                {
                                    dynamicContentDetails.DynamicTwoRowsVisibility = "table";
                                    dynamicContentDetails.RowCompletionVisibility = "table-row";
                                    var totalDetails = dtTotal.AsEnumerable().Where(x => x.Field<string>("date") == row.date && x.Field<string>("Name") == column.name).Select(x => new { plan = x.Field<dynamic>("TotalProdActual"), actual = x.Field<dynamic>("TotalRejActual"), weight = x.Field<dynamic>("TotalWeight") }).FirstOrDefault();

                                    if (totalDetails != null)
                                    {
                                        if (totalDetails.actual != null)
                                        {
                                            dynamicContentDetails.Actual = Convert.ToString(totalDetails.actual);
                                            dynamicContentDetails.ActualContentColor = "unset";

                                        }
                                        if (totalDetails.plan != null)
                                        {
                                            dynamicContentDetails.Plan = Convert.ToString(totalDetails.plan);
                                            dynamicContentDetails.PlanContentColor = "unset";
                                        }
                                        if (totalDetails.weight != null)
                                        {
                                            dynamicContentDetails.RowCompletion = Convert.ToString(totalDetails.weight);
                                            dynamicContentDetails.RowCompletionContentColor = "unset";
                                        }
                                    }
                                }
                            }
                            dynamicTotalList.Add(dynamicContentDetails);
                        }

                        columnCount++;
                    }

                    #region ---- Right side total Columns ----
                    dynamicContentDetails = new DynamicColumnDetails();
                    if (screenType.Equals("AcceptedScreen", StringComparison.OrdinalIgnoreCase))
                    {
                        if (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase))
                        {
                            if (i == 0)
                            {
                                dynamicHeaderDetails = new DynamicColumnDetails();
                                dynamicHeaderDetails.DynamicHeaderVisibility = "table";
                                dynamicHeaderDetails.HeaderName = "Total";
                                dynamicHeaderList.Add(dynamicHeaderDetails);

                                dynamicHeaderDetails = new DynamicColumnDetails();
                                dynamicHeaderDetails.DynamicHeaderVisibility = "table";
                                dynamicHeaderDetails.HeaderName = "Plan";
                                dynamicHeaderList.Add(dynamicHeaderDetails);

                                dynamicHeaderDetails = new DynamicColumnDetails();
                                dynamicHeaderDetails.DynamicHeaderVisibility = "table";
                                dynamicHeaderDetails.HeaderName = "% Completion";
                                dynamicHeaderList.Add(dynamicHeaderDetails);

                                dynamicHeaderDetails = new DynamicColumnDetails();
                                dynamicHeaderDetails.DynamicHeaderVisibility = "table";
                                dynamicHeaderDetails.HeaderName = "Chart";
                                dynamicHeaderList.Add(dynamicHeaderDetails);
                            }
                            var machineLevelValueDetails = dt.AsEnumerable().Where(x => x.Field<string>("MachineID") == row.machine && x.Field<string>("date") == row.date && x.Field<string>("ShiftName") == row.shift).Select(x => new { plantotal = x.Field<dynamic>("Plan"), actualtotal = x.Field<dynamic>("Total"), completiontotal = x.Field<dynamic>("Completion") }).FirstOrDefault();

                            dynamicContentDetails.DynamicOneRowVisibility = "table";
                            if (machineLevelValueDetails.actualtotal != null)
                            {
                                dynamicContentDetails.Value = Convert.ToString(machineLevelValueDetails.actualtotal);
                                dynamicContentDetails.ValueContentColor = "unset";
                            }
                            dynamicContentList.Add(dynamicContentDetails);

                            dynamicContentDetails = new DynamicColumnDetails();
                            dynamicContentDetails.DynamicOneRowVisibility = "table";
                            if (machineLevelValueDetails.plantotal != null)
                            {
                                dynamicContentDetails.Value = Convert.ToString(machineLevelValueDetails.plantotal);
                                dynamicContentDetails.ValueContentColor = "unset";
                            }
                            dynamicContentList.Add(dynamicContentDetails);

                            dynamicContentDetails = new DynamicColumnDetails();
                            dynamicContentDetails.DynamicOneRowVisibility = "block";
                            if (machineLevelValueDetails.completiontotal != null)
                            {
                                dynamicContentDetails.Value = Convert.ToString(machineLevelValueDetails.completiontotal);
                                dynamicContentDetails.ValueContentColor = "unset";

                            }
                            dynamicContentList.Add(dynamicContentDetails);

                            dynamicContentDetails = new DynamicColumnDetails();
                            dynamicContentDetails.DynamicChartVisibility = "block";
                            dynamicContentDetails.DynamicChartIconVisibility = "block";
                            dynamicContentList.Add(dynamicContentDetails);
                        }
                        else
                        {
                            dynamicContentDetails.DynamicTwoRowsVisibility = "table";
                            if (i == 0)
                            {
                                dynamicHeaderDetails = new DynamicColumnDetails();
                                dynamicHeaderDetails.DynamicHeaderVisibility = "table";
                                dynamicHeaderDetails.HeaderName = (selectedMenu.Equals("DayDataMenu", StringComparison.OrdinalIgnoreCase) ? "Day Total" : selectedMenu.Equals("MonthDataMenu", StringComparison.OrdinalIgnoreCase) ? "YTD" : "Total");

                                dynamicHeaderList.Add(dynamicHeaderDetails);

                                dynamicHeaderDetails = new DynamicColumnDetails();
                                dynamicHeaderDetails.DynamicHeaderVisibility = "table";
                                dynamicHeaderDetails.HeaderName = "% Completion";
                                dynamicHeaderList.Add(dynamicHeaderDetails);

                                dynamicHeaderDetails = new DynamicColumnDetails();
                                dynamicHeaderDetails.DynamicHeaderVisibility = "table";
                                dynamicHeaderDetails.HeaderName = "Chart";
                                dynamicHeaderList.Add(dynamicHeaderDetails);
                            }
                            var machineLevelValueDetails = dt.AsEnumerable().Where(x => x.Field<string>("MachineID") == row.machine && x.Field<string>("date") == row.date && x.Field<string>("ShiftName") == row.shift).Select(x => new { plantotal = x.Field<dynamic>("Plan"), actualtotal = x.Field<dynamic>("Total"), completiontotal = x.Field<dynamic>("Completion") }).FirstOrDefault();
                            if (machineLevelValueDetails.actualtotal != null)
                            {
                                dynamicContentDetails.Actual = Convert.ToString(machineLevelValueDetails.actualtotal);
                                dynamicContentDetails.ActualContentColor = "unset";
                            }
                            if (machineLevelValueDetails.plantotal != null)
                            {
                                dynamicContentDetails.Plan = Convert.ToString(machineLevelValueDetails.plantotal);
                                dynamicContentDetails.PlanContentColor = "unset";
                            }
                            dynamicContentList.Add(dynamicContentDetails);

                            dynamicContentDetails = new DynamicColumnDetails();
                            dynamicContentDetails.DynamicMergedRowVisibility = "block";
                            if (machineLevelValueDetails.completiontotal != null)
                            {
                                dynamicContentDetails.Completion = Convert.ToString(machineLevelValueDetails.completiontotal);
                                dynamicContentDetails.CompletionContentColor = "unset";

                            }
                            dynamicContentList.Add(dynamicContentDetails);

                            dynamicContentDetails = new DynamicColumnDetails();
                            dynamicContentDetails.DynamicChartVisibility = "block";
                            dynamicContentDetails.DynamicChartIconVisibility = "block";
                            dynamicContentList.Add(dynamicContentDetails);
                        }
                    }
                    else if (screenType.Equals("RejectionScreen", StringComparison.OrdinalIgnoreCase))
                    {
                        if (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase))
                        {
                            if (i == 0)
                            {
                                dynamicHeaderDetails = new DynamicColumnDetails();
                                dynamicHeaderDetails.DynamicHeaderVisibility = "table";
                                dynamicHeaderDetails.HeaderName = "Total Rejection";
                                dynamicHeaderList.Add(dynamicHeaderDetails);

                                dynamicHeaderDetails = new DynamicColumnDetails();
                                dynamicHeaderDetails.DynamicHeaderVisibility = "table";
                                dynamicHeaderDetails.HeaderName = "Total Production";
                                dynamicHeaderList.Add(dynamicHeaderDetails);

                                dynamicHeaderDetails = new DynamicColumnDetails();
                                dynamicHeaderDetails.DynamicHeaderVisibility = "table";
                                dynamicHeaderDetails.HeaderName = "% Rejection";
                                dynamicHeaderList.Add(dynamicHeaderDetails);

                                dynamicHeaderDetails = new DynamicColumnDetails();
                                dynamicHeaderDetails.DynamicHeaderVisibility = "table";
                                dynamicHeaderDetails.HeaderName = "Chart";
                                dynamicHeaderList.Add(dynamicHeaderDetails);
                            }
                            var machineLevelValueDetails = dt.AsEnumerable().Where(x => x.Field<string>("MachineID") == row.machine && x.Field<string>("date") == row.date && x.Field<string>("ShiftName") == row.shift).Select(x => new { plantotal = x.Field<dynamic>("Plan"), actualtotal = x.Field<dynamic>("Total"), completiontotal = x.Field<dynamic>("Completion") }).FirstOrDefault();

                            dynamicContentDetails.DynamicOneRowVisibility = "table";
                            if (machineLevelValueDetails.actualtotal != null)
                            {
                                dynamicContentDetails.Value = Convert.ToString(machineLevelValueDetails.actualtotal);
                                dynamicContentDetails.ValueContentColor = "unset";
                            }
                            dynamicContentList.Add(dynamicContentDetails);

                            dynamicContentDetails = new DynamicColumnDetails();
                            dynamicContentDetails.DynamicOneRowVisibility = "table";
                            if (machineLevelValueDetails.plantotal != null)
                            {
                                dynamicContentDetails.Value = Convert.ToString(machineLevelValueDetails.plantotal);
                                dynamicContentDetails.ValueContentColor = "unset";
                            }
                            dynamicContentList.Add(dynamicContentDetails);

                            dynamicContentDetails = new DynamicColumnDetails();
                            dynamicContentDetails.DynamicOneRowVisibility = "block";
                            if (machineLevelValueDetails.completiontotal != null)
                            {
                                dynamicContentDetails.Value = Convert.ToString(machineLevelValueDetails.completiontotal);
                                dynamicContentDetails.ValueContentColor = "unset";

                            }
                            dynamicContentList.Add(dynamicContentDetails);

                            dynamicContentDetails = new DynamicColumnDetails();
                            dynamicContentDetails.DynamicChartVisibility = "block";
                            dynamicContentDetails.DynamicChartIconVisibility = "block";
                            dynamicContentList.Add(dynamicContentDetails);
                        }
                        else
                        {
                            dynamicContentDetails.DynamicTwoRowsVisibility = "table";
                            if (i == 0)
                            {
                                dynamicHeaderDetails = new DynamicColumnDetails();
                                dynamicHeaderDetails.DynamicHeaderVisibility = "table";
                                dynamicHeaderDetails.HeaderName = "Total";
                                dynamicHeaderList.Add(dynamicHeaderDetails);

                                //dynamicHeaderDetails = new DynamicColumnDetails();
                                //dynamicHeaderDetails.DynamicHeaderVisibility = "table";
                                //dynamicHeaderDetails.HeaderName = "Total Production";
                                //dynamicHeaderList.Add(dynamicHeaderDetails);

                                dynamicHeaderDetails = new DynamicColumnDetails();
                                dynamicHeaderDetails.DynamicHeaderVisibility = "table";
                                dynamicHeaderDetails.HeaderName = "Chart";
                                dynamicHeaderList.Add(dynamicHeaderDetails);
                            }
                            var machineLevelValueDetails = dt.AsEnumerable().Where(x => x.Field<string>("MachineID") == row.machine && x.Field<string>("date") == row.date && x.Field<string>("ShiftName") == row.shift).Select(x => new { actualtotal_rej = x.Field<dynamic>("Total"), actualtotal_prod = x.Field<dynamic>("TotalProdQty") }).FirstOrDefault();
                            //plantotal_rej = x.Field<dynamic>("Plan"), plantotal_prod = x.Field<dynamic>("TotalPlan")
                            if (machineLevelValueDetails.actualtotal_rej != null)
                            {
                                dynamicContentDetails.Actual = Convert.ToString(machineLevelValueDetails.actualtotal_rej);
                                dynamicContentDetails.ActualContentColor = "unset";
                            }
                            //if (machineLevelValueDetails.plantotal_rej != null)
                            //{
                            //    dynamicContentDetails.Plan = Convert.ToString(machineLevelValueDetails.plantotal_rej);
                            //    dynamicContentDetails.PlanContentColor = "unset";
                            //}
                            //dynamicContentList.Add(dynamicContentDetails);

                            // dynamicContentDetails = new DynamicColumnDetails();
                            // dynamicContentDetails.DynamicTwoRowsVisibility = "table";
                            if (machineLevelValueDetails.actualtotal_prod != null)
                            {
                                dynamicContentDetails.Plan = Convert.ToString(machineLevelValueDetails.actualtotal_prod);
                                dynamicContentDetails.PlanContentColor = "unset";
                            }
                            //if (machineLevelValueDetails.plantotal_prod != null)
                            //{
                            //    dynamicContentDetails.Plan = Convert.ToString(machineLevelValueDetails.plantotal_prod);
                            //    dynamicContentDetails.PlanContentColor = "unset";
                            //}
                            dynamicContentList.Add(dynamicContentDetails);

                            dynamicContentDetails = new DynamicColumnDetails();
                            dynamicContentDetails.DynamicChartVisibility = "block";
                            dynamicContentDetails.DynamicChartIconVisibility = "block";
                            dynamicContentList.Add(dynamicContentDetails);
                        }
                    }



                    if (isLastMachine)
                    {
                        dynamicContentDetails = new DynamicColumnDetails();
                        if (screenType.Equals("AcceptedScreen", StringComparison.OrdinalIgnoreCase))
                        {
                            if (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase))
                            {
                                //var rightTotalDetails = dtRightTotal.AsEnumerable().Where(x => x.Field<string>("date") == row.date).Select(x => new { plantotal = x.Field<dynamic>("TotalPlanQty"), actualtotal = x.Field<dynamic>("TotalProdQty"), completiontotal = x.Field<dynamic>("TotalCompletion") }).FirstOrDefault();
                                var rightTotalDetails = dtRightTotal.AsEnumerable().Where(x => x.Field<string>("date") == row.date).Select(x => new { actualtotal = x.Field<dynamic>("TotalProdQty"), completiontotal = x.Field<dynamic>("TotalCompletion") }).FirstOrDefault();

                                dynamicContentDetails.DynamicOneRowVisibility = "table";
                                if (rightTotalDetails.actualtotal != null)
                                {
                                    dynamicContentDetails.Value = Convert.ToString(rightTotalDetails.actualtotal);
                                    dynamicContentDetails.ValueContentColor = "unset";
                                }
                                dynamicTotalList.Add(dynamicContentDetails);

                                var rightTotalDetails_1 = dtRightTotal_1.AsEnumerable().Where(x => x.Field<string>("date") == row.date).Select(x => new { plantotal = x.Field<dynamic>("TotalPlanQty"), completiontotal = x.Field<dynamic>("TotalCompletion") }).FirstOrDefault();
                                dynamicContentDetails = new DynamicColumnDetails();
                                dynamicContentDetails.DynamicOneRowVisibility = "table";
                                if (rightTotalDetails_1.plantotal != null)
                                {
                                    dynamicContentDetails.Value = Convert.ToString(rightTotalDetails_1.plantotal);
                                    dynamicContentDetails.ValueContentColor = "unset";
                                }
                                dynamicTotalList.Add(dynamicContentDetails);

                                dynamicContentDetails = new DynamicColumnDetails();
                                dynamicContentDetails.DynamicOneRowVisibility = "block";
                                if (rightTotalDetails.completiontotal != null)
                                {
                                    dynamicContentDetails.Value = Convert.ToString(rightTotalDetails.completiontotal);
                                    dynamicContentDetails.ValueContentColor = "unset";

                                }
                                dynamicTotalList.Add(dynamicContentDetails);

                                dynamicContentDetails = new DynamicColumnDetails();
                                dynamicContentDetails.DynamicChartVisibility = "block";
                                dynamicTotalList.Add(dynamicContentDetails);
                            }
                            else
                            {
                                var rightTotalDetails = dtRightTotal.AsEnumerable().Where(x => x.Field<string>("date") == row.date).Select(x => new { plantotal = x.Field<dynamic>("TotalPlanQty"), actualtotal = x.Field<dynamic>("TotalProdQty"), rowcompletiontotal = x.Field<dynamic>("TotalCompletion") }).FirstOrDefault();

                                dynamicContentDetails.DynamicTwoRowsVisibility = "table";
                                dynamicContentDetails.RowCompletionVisibility = "table-row";
                                if (rightTotalDetails.actualtotal != null)
                                {
                                    dynamicContentDetails.Actual = Convert.ToString(rightTotalDetails.actualtotal);
                                    dynamicContentDetails.ActualContentColor = "unset";
                                }
                                if (rightTotalDetails.plantotal != null)
                                {
                                    dynamicContentDetails.Plan = Convert.ToString(rightTotalDetails.plantotal);
                                    dynamicContentDetails.PlanContentColor = "unset";
                                }
                                if (rightTotalDetails.rowcompletiontotal != null)
                                {
                                    dynamicContentDetails.RowCompletion = Convert.ToString(rightTotalDetails.rowcompletiontotal);
                                    dynamicContentDetails.RowCompletionContentColor = "unset";
                                }
                                dynamicTotalList.Add(dynamicContentDetails);

                                dynamicContentDetails = new DynamicColumnDetails();
                                dynamicContentDetails.DynamicMergedRowVisibility = "block";
                                //if (rightTotalDetails.completiontotal != null)
                                //{
                                //    dynamicContentDetails.Completion = Convert.ToString(rightTotalDetails.completiontotal);
                                //    dynamicContentDetails.CompletionContentColor = "unset";

                                //}
                                dynamicTotalList.Add(dynamicContentDetails);

                                dynamicContentDetails = new DynamicColumnDetails();
                                dynamicContentDetails.DynamicChartVisibility = "block";
                                dynamicTotalList.Add(dynamicContentDetails);
                            }
                        }
                        else if (screenType.Equals("RejectionScreen", StringComparison.OrdinalIgnoreCase))
                        {
                            if (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase))
                            {
                                DynamicColumnDetails dynamicToatlWightContentDetails = new DynamicColumnDetails();

                                var rightTotalDetails = dtRightTotal.AsEnumerable().Where(x => x.Field<string>("date") == row.date).Select(x => new { rejectiontotal = x.Field<dynamic>("TotalRejQty"), productiontotal = x.Field<dynamic>("TotalProdQty"), completiontotal = x.Field<dynamic>("TotalCompletion") }).FirstOrDefault();

                                dynamicContentDetails.DynamicOneRowVisibility = "table";
                                if (rightTotalDetails.rejectiontotal != null)
                                {
                                    dynamicContentDetails.Value = Convert.ToString(rightTotalDetails.rejectiontotal);
                                    dynamicContentDetails.ValueContentColor = "unset";
                                }
                                dynamicTotalList.Add(dynamicContentDetails);

                                dynamicToatlWightContentDetails.DynamicOneRowVisibility = "table";
                                dynamicTotalWeightList.Add(dynamicToatlWightContentDetails);


                                dynamicContentDetails = new DynamicColumnDetails();
                                dynamicContentDetails.DynamicOneRowVisibility = "table";
                                if (rightTotalDetails.productiontotal != null)
                                {
                                    dynamicContentDetails.Value = Convert.ToString(rightTotalDetails.productiontotal);
                                    dynamicContentDetails.ValueContentColor = "unset";
                                }
                                dynamicTotalList.Add(dynamicContentDetails);

                                dynamicToatlWightContentDetails = new DynamicColumnDetails();
                                dynamicToatlWightContentDetails.DynamicOneRowVisibility = "table";
                                dynamicTotalWeightList.Add(dynamicToatlWightContentDetails);


                                dynamicContentDetails = new DynamicColumnDetails();
                                dynamicContentDetails.DynamicOneRowVisibility = "block";
                                if (rightTotalDetails.completiontotal != null)
                                {
                                    dynamicContentDetails.Value = Convert.ToString(rightTotalDetails.completiontotal);
                                    dynamicContentDetails.ValueContentColor = "unset";

                                }
                                dynamicTotalList.Add(dynamicContentDetails);

                                dynamicToatlWightContentDetails = new DynamicColumnDetails();
                                dynamicToatlWightContentDetails.DynamicOneRowVisibility = "table";
                                dynamicTotalWeightList.Add(dynamicToatlWightContentDetails);

                                dynamicContentDetails = new DynamicColumnDetails();
                                dynamicContentDetails.DynamicChartVisibility = "block";
                                dynamicTotalList.Add(dynamicContentDetails);

                                dynamicToatlWightContentDetails = new DynamicColumnDetails();
                                dynamicToatlWightContentDetails.DynamicChartVisibility = "block";
                                dynamicTotalWeightList.Add(dynamicToatlWightContentDetails);
                            }
                            else
                            {
                                var rightTotalDetails = dtRightTotal.AsEnumerable().Where(x => x.Field<string>("date") == row.date).Select(x => new { actualtotal_rej = x.Field<dynamic>("RejectionTotal"), actualtotal_prod = x.Field<dynamic>("ProductionTotal") }).FirstOrDefault();
                                // , plantotal_prod = x.Field<dynamic>("TotalPlanQty"), plantotal_rej = x.Field<dynamic>("PlannedRejectionTotal"), 

                                dynamicContentDetails.DynamicTwoRowsVisibility = "table";
                                dynamicContentDetails.RowCompletionVisibility = "table-row";
                                if (rightTotalDetails.actualtotal_rej != null)
                                {
                                    dynamicContentDetails.Actual = Convert.ToString(rightTotalDetails.actualtotal_rej);
                                    dynamicContentDetails.ActualContentColor = "unset";
                                }
                                //if (rightTotalDetails.plantotal_rej != null)
                                //{
                                //    dynamicContentDetails.Plan = Convert.ToString(rightTotalDetails.plantotal_rej);
                                //    dynamicContentDetails.PlanContentColor = "unset";
                                //}
                                //dynamicTotalList.Add(dynamicContentDetails);

                                //dynamicContentDetails = new DynamicColumnDetails();
                                //dynamicContentDetails.DynamicTwoRowsVisibility = "table";
                                if (rightTotalDetails.actualtotal_prod != null)
                                {
                                    dynamicContentDetails.Plan = Convert.ToString(rightTotalDetails.actualtotal_prod);
                                    dynamicContentDetails.PlanContentColor = "unset";
                                }
                                //if (rightTotalDetails.plantotal_prod != null)
                                //{
                                //    dynamicContentDetails.Plan = Convert.ToString(rightTotalDetails.plantotal_prod);
                                //    dynamicContentDetails.PlanContentColor = "unset";
                                //}

                                //if (rightTotalDetails.weight != null)
                                //{
                                //    dynamicContentDetails.Plan = Convert.ToString(rightTotalDetails.actualtotal_prod);
                                //    dynamicContentDetails.PlanContentColor = "unset";
                                //}

                                dynamicTotalList.Add(dynamicContentDetails);

                                dynamicContentDetails = new DynamicColumnDetails();
                                dynamicContentDetails.DynamicChartVisibility = "block";
                                dynamicTotalList.Add(dynamicContentDetails);
                            }
                        }
                    }
                    #endregion

                    if (i == 0)
                    {
                        dataHeaderDetails.DynamicColumnCountForExport = distinctColumnDetails.Count;
                        dataHeaderDetails.DynamicColumnDetails = dynamicHeaderList;
                        list.Add(dataHeaderDetails);
                        i++;
                    }

                    dataContentDetails.DynamicColumnCountForExport = distinctColumnDetails.Count;
                    dataContentDetails.DynamicColumnDetails = dynamicContentList;
                    list.Add(dataContentDetails);

                    if (isLastMachine)
                    {
                        dataTotalDetails = new MachineTypeDetails();
                        if (shiftColumnVisibility)
                        {
                            dataTotalDetails.ShiftConentVisibility = "table-cell";
                            dataTotalDetails.ShiftRowSpan = 1;
                        }
                        dataTotalDetails.MachineID = "Total";
                        dataTotalDetails.ContentVisibility = "table-cell";
                        dataTotalDetails.RowBackColor = "row-bakc-color";
                        if (!selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase))
                        {
                            dataTotalDetails.PlanActualConentVisibility = "table-cell";
                            if (screenType.Equals("RejectionScreen", StringComparison.OrdinalIgnoreCase))
                            {
                                dataTotalDetails.PlanName = "Production Count";
                                dataTotalDetails.ActualName = "No. Of Rejection";
                                dataTotalDetails.RowCompletionNameVisibility = "table-row";
                                dataTotalDetails.RowCompletionName = "Total (Weight)";
                            }
                            else
                            {
                                dataTotalDetails.PlanName = "Plan";
                                dataTotalDetails.ActualName = "Actual";
                                dataTotalDetails.RowCompletionNameVisibility = "table-row";
                                dataTotalDetails.RowCompletionName = "% Completion";
                            }
                            dataTotalDetails.MachineIDRowSpanForExport = 3 - 1; //for export
                        }
                        dataTotalDetails.DynamicColumnDetails = dynamicTotalList;
                        list.Add(dataTotalDetails);

                        if (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase) && screenType.Equals("RejectionScreen", StringComparison.OrdinalIgnoreCase))
                        {
                            dataTotalDetails = new MachineTypeDetails();
                            if (shiftColumnVisibility)
                            {
                                dataTotalDetails.ShiftConentVisibility = "table-cell";
                                dataTotalDetails.ShiftRowSpan = 1;
                            }
                            dataTotalDetails.MachineID = "Total (Weight)";
                            dataTotalDetails.ContentVisibility = "table-cell";
                            dataTotalDetails.RowBackColor = "row-bakc-color";
                            dataTotalDetails.DynamicColumnDetails = dynamicTotalWeightList;
                            list.Add(dataTotalDetails);

                        }


                    }

                    prviousDate = dataContentDetails.Date;
                    previousShift = dataContentDetails.Shift;
                }

                lvDetails.DataSource = list;
                lvDetails.DataBind();

                Session["CumiProductionRejectionDetails"] = list;

                if (viewType.Equals("Report", StringComparison.OrdinalIgnoreCase))
                {
                    btnExport.Visible = true;
                }
                else
                {
                    btnExport.Visible = false;
                }

                if (selectedMenu.Equals("DayDataMenu", StringComparison.OrdinalIgnoreCase) || selectedMenu.Equals("WeekDataMenu", StringComparison.OrdinalIgnoreCase) || selectedMenu.Equals("MonthDataMenu", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClass.openFunction(this, "BindDownChart");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindDetails: " + ex.Message);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ChartDetails GetChartDetails(string param, string headerID, string machineID, string date, string shift, string selectedMenu, string screenType, string fromDate, string toDate, string multiselectMachines, string plant)
        {
            ChartDetails data = new ChartDetails();
            try
            {
                shift = (shift == "All" ? "" : shift);
                DataTable dt = new DataTable();
                DataTable dtTotal = new DataTable();
                if (HttpContext.Current.Session["TotalDetails"] != null)
                {
                    dtTotal = (DataTable)(HttpContext.Current.Session["TotalDetails"]);
                }
                if (HttpContext.Current.Session["P_R_Details"] != null)
                {
                    dt = (DataTable)(HttpContext.Current.Session["P_R_Details"]);
                }

                if (screenType == "AcceptedScreen")
                {
                    if (param.Equals("HeaderChart", StringComparison.OrdinalIgnoreCase))
                    {
                        double plan = Convert.ToDouble(dtTotal.AsEnumerable().Where(x => x.Field<string>("Name").Equals(headerID, StringComparison.OrdinalIgnoreCase)).Sum(x => Convert.ToDouble(x.Field<double>("TotalPlan"))));
                        double actual = Convert.ToDouble(dtTotal.AsEnumerable().Where(x => x.Field<string>("Name").Equals(headerID, StringComparison.OrdinalIgnoreCase)).Sum(x => Convert.ToDouble(x.Field<double>("TotalActual"))));
                        double remaining = plan - actual;
                        //data.PieChartDetails.Add(new PieChartDetails() { name = "Actual", y = Convert.ToDouble(dtTotal.AsEnumerable().Where(x => x.Field<string>("Name").Equals(headerID, StringComparison.OrdinalIgnoreCase)).Sum(x => Convert.ToDouble(x.Field<double>("TotalActual")))) });
                        //data.PieChartDetails.Add(new PieChartDetails() { name = "Plan", y = Convert.ToDouble(dtTotal.AsEnumerable().Where(x => x.Field<string>("Name").Equals(headerID, StringComparison.OrdinalIgnoreCase)).Sum(x => Convert.ToDouble(x.Field<double>("TotalPlan")))) });
                        data.PieChartDetails.Add(new PieChartDetails() { name = "Actual", y = actual });
                        data.PieChartDetails.Add(new PieChartDetails() { name = "Remaining", y = Math.Abs(remaining), positive = (remaining >= 0 ? true : false) });
                    }
                    else
                    {

                        var TotalDetails = (dynamic)null;
                        if (selectedMenu.Equals("DayDataMenu", StringComparison.OrdinalIgnoreCase) || selectedMenu.Equals("WeekDataMenu", StringComparison.OrdinalIgnoreCase) || selectedMenu.Equals("MonthDataMenu", StringComparison.OrdinalIgnoreCase))
                        {
                            TotalDetails = dt.AsEnumerable().Where(x => x.Field<string>("Machineid").Equals(machineID, StringComparison.OrdinalIgnoreCase)).Select(x => new { actual = x.Field<double>("Total"), plan = x.Field<double>("Plan") }).FirstOrDefault();
                        }
                        else if (selectedMenu.Equals("ShiftDataMenu", StringComparison.OrdinalIgnoreCase))
                        {
                            TotalDetails = dt.AsEnumerable().Where(x => x.Field<string>("Machineid").Equals(machineID, StringComparison.OrdinalIgnoreCase) && Util.GetDateTime(x.Field<string>("date")).ToString("dd-MM-yyyy").Equals(date, StringComparison.OrdinalIgnoreCase)).Select(x => new { actual = x.Field<double>("Total"), plan = x.Field<double>("Plan") }).FirstOrDefault();

                        }
                        else if (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase))
                        {
                            TotalDetails = dt.AsEnumerable().Where(x => x.Field<string>("Machineid").Equals(machineID, StringComparison.OrdinalIgnoreCase) && Util.GetDateTime(x.Field<string>("date")).ToString("dd-MM-yyyy").Equals(date, StringComparison.OrdinalIgnoreCase) && x.Field<string>("ShiftName").Equals(shift, StringComparison.OrdinalIgnoreCase)).Select(x => new { actual = x.Field<double>("Total"), plan = x.Field<double>("Plan") }).FirstOrDefault();
                        }
                        //data.PieChartDetails.Add(new PieChartDetails() { name = "Actual", y = Convert.ToDouble(TotalDetails.actual) });
                        //data.PieChartDetails.Add(new PieChartDetails() { name = "Plan", y = Convert.ToDouble(TotalDetails.plan) });
                        double remaining = Convert.ToDouble(TotalDetails.plan) - Convert.ToDouble(TotalDetails.actual);
                        data.PieChartDetails.Add(new PieChartDetails() { name = "Actual", y = Convert.ToDouble(TotalDetails.actual) });
                        data.PieChartDetails.Add(new PieChartDetails() { name = "Remaining", y = Math.Abs(remaining), positive = (remaining >= 0 ? true : false) });
                    }
                }
                else if (screenType == "RejectionScreen")
                {
                    if (param.Equals("HeaderChart", StringComparison.OrdinalIgnoreCase))
                    {

                        data.PieChartDetails.Add(new PieChartDetails() { name = "Actual", y = Convert.ToDouble(dtTotal.AsEnumerable().Where(x => x.Field<string>("Name").Equals(headerID, StringComparison.OrdinalIgnoreCase)).Sum(x => Convert.ToDouble(x.Field<double>("TotalRejActual")))) });
                        data.PieChartDetails.Add(new PieChartDetails() { name = "Production", y = Convert.ToDouble(dtTotal.AsEnumerable().Where(x => x.Field<string>("Name").Equals(headerID, StringComparison.OrdinalIgnoreCase)).Sum(x => Convert.ToDouble(x.Field<double>("TotalProdActual")))) });
                    }
                    else
                    {

                        var TotalDetails = (dynamic)null;
                        //data.PieChartDetails.Add(new PieCh
                        if (selectedMenu.Equals("DayDataMenu", StringComparison.OrdinalIgnoreCase) || selectedMenu.Equals("WeekDataMenu", StringComparison.OrdinalIgnoreCase) || selectedMenu.Equals("MonthDataMenu", StringComparison.OrdinalIgnoreCase))
                        {
                            TotalDetails = dt.AsEnumerable().Where(x => x.Field<string>("Machineid").Equals(machineID, StringComparison.OrdinalIgnoreCase)).Select(x => new { actual = x.Field<double>("Total"), plan = x.Field<double>("TotalProdQty") }).FirstOrDefault();
                        }
                        else if (selectedMenu.Equals("ShiftDataMenu", StringComparison.OrdinalIgnoreCase))
                        {
                            TotalDetails = dt.AsEnumerable().Where(x => x.Field<string>("Machineid").Equals(machineID, StringComparison.OrdinalIgnoreCase) && Util.GetDateTime(x.Field<string>("date")).ToString("dd-MM-yyyy").Equals(date, StringComparison.OrdinalIgnoreCase)).Select(x => new { actual = x.Field<double>("Total"), plan = x.Field<double>("TotalProdQty") }).FirstOrDefault();

                        }
                        else if (selectedMenu.Equals("HourlyDataMenu", StringComparison.OrdinalIgnoreCase))
                        {
                            TotalDetails = dt.AsEnumerable().Where(x => x.Field<string>("Machineid").Equals(machineID, StringComparison.OrdinalIgnoreCase) && Util.GetDateTime(x.Field<string>("date")).ToString("dd-MM-yyyy").Equals(date, StringComparison.OrdinalIgnoreCase) && x.Field<string>("ShiftName").Equals(shift, StringComparison.OrdinalIgnoreCase)).Select(x => new { actual = x.Field<double>("Total"), plan = x.Field<double>("Plan") }).FirstOrDefault();
                        }
                        data.PieChartDetails.Add(new PieChartDetails() { name = "Actual", y = Convert.ToDouble(TotalDetails.actual) });
                        data.PieChartDetails.Add(new PieChartDetails() { name = "Production", y = Convert.ToDouble(TotalDetails.plan) });
                    }

                    string fromDateTime = "", toDateTime = "";
                    List<PDTData> shiftTimeDetails = DataBaseAccess.getShiftTimeDetails(Util.GetDateTime(fromDate));
                    if (string.IsNullOrEmpty(shift))
                    {
                        fromDateTime = shiftTimeDetails.Select(x => x.FromDateTime).FirstOrDefault();

                    }
                    else
                    {
                        fromDateTime = shiftTimeDetails.Where(x => x.ShiftName == shift).Select(x => x.FromDateTime).FirstOrDefault();
                    }
                    shiftTimeDetails = DataBaseAccess.getShiftTimeDetails(Util.GetDateTime(toDate));
                    if (string.IsNullOrEmpty(shift))
                    {
                        toDateTime = shiftTimeDetails.Select(x => x.ToDateTime).LastOrDefault();

                    }
                    else
                    {
                        toDateTime = shiftTimeDetails.Where(x => x.ShiftName == shift).Select(x => x.ToDateTime).FirstOrDefault();
                    }
                    //foreach (PDTData data in shiftDetails)
                    //{
                    //    list.Add(new ListItem() { Text = data.ShiftName, Value = data.ShiftID + ";;" + data.FromDateTime + ";;" + data.ToDateTime });
                    //}


                    DataTable dtParetoChartDetails = CumiDBAccess.GetProductionRejectionParetoChartDetails(plant, multiselectMachines, fromDateTime, toDateTime);
                    foreach (DataRow row in dtParetoChartDetails.Rows)
                    {
                        data.ParetoChartDetails.Categories.Add(row["rejectionid"].ToString());
                        data.ParetoChartDetails.Values.Add(Convert.ToDouble(row["RejQty"].ToString()));
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetChartDetails: " + ex.Message);
            }
            return data;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string Generated = TMPTrakGenerateReport.GenerateCumiProductionRejectionReport(ViewState["ScreenType"].ToString(), hfSelectedMenu.Value, ViewState["Plant"].ToString(), ViewState["FromDate"].ToString(), ViewState["ToDate"].ToString(), ViewState["Year"].ToString(), ViewState["Month"].ToString());
                if (Generated.Equals("", StringComparison.OrdinalIgnoreCase))
                {
                    // HelperClass.opene(this, "No Data Found");
                }
                else if (Generated.Equals("NodataFound", StringComparison.OrdinalIgnoreCase))
                {
                    //
                }
                else if (Generated.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                {
                    //HelperClass.ope(this, "No Data Found");
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnExport_Click: " + ex.Message);
            }
        }

        protected void cbAutorefresh_CheckedChanged(object sender, EventArgs e)
        {
            ViewState["AutorefreshValue"] = cbAutorefresh.Checked;
            BindDetails();
        }

        protected void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                //int inetrval = 1000 * Convert.ToInt32(WebConfigurationManager.AppSettings["MachineStatusRefreshInetrval"].ToString());
                //if (cbAutorefresh.Checked)
                //{
                //    int inetrval = 1000 * 10;
                //    timer.Enabled = true;
                //    timer.Interval = inetrval; 
                //}
                //else
                //{
                //    timer.Enabled = false;
                //}

                BindDetails();
                if (hdnIsChartModalOpen.Value.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "openModal", "openChartModal();", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("timer_Tick: " + ex.Message);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static LineChartDetails GetDownChartDetails(string screenType, string selectedMenu)
        {
            LineChartDetails finalData = new LineChartDetails();
            try
            {

                if (selectedMenu.Equals("DayDataMenu", StringComparison.OrdinalIgnoreCase) || selectedMenu.Equals("WeekDataMenu", StringComparison.OrdinalIgnoreCase) || selectedMenu.Equals("MonthDataMenu", StringComparison.OrdinalIgnoreCase))
                {
                    DataTable dt = new DataTable();

                    if (HttpContext.Current.Session["P_R_Details"] != null)
                    {
                        dt = (DataTable)(HttpContext.Current.Session["P_R_Details"]);
                    }


                    //string[] colors = { "#460887", "#8f0b2c", "#0f3c85", "#0b7840", "#964712", "#820868", "#1b1063" };
                    //string[] transaperentColors = { "#6d0bd4", "#c9264f", "#306ccf", "#1cd676", "#db7a39", "#db2eb6", "#3721c4" };

                    if (screenType == "AcceptedScreen" || screenType == "RejectionScreen")
                    {
                        var distinctYAxisDetails = dt.AsEnumerable().Select(x => x.Field<string>("Name")).Distinct();

                        var distinctMachineDetails = dt.AsEnumerable().Select(x => x.Field<string>("MachineID")).Distinct();

                        List<string> categoryList = new List<string>();
                        List<LineChartSeriesDetails> lineSeries = new List<LineChartSeriesDetails>();
                        LineChartSeriesDetails seriesData = null;

                        int machineCount = 0;
                        int weekCount = 1;
                        foreach (var machine in distinctMachineDetails)
                        {
                            List<Double> actualData = new List<Double>();
                            List<Double> planData = new List<Double>();
                            string planName = "Plan", actulaName = "Actual";
                            foreach (var yaxis in distinctYAxisDetails)
                            {

                                if (screenType == "AcceptedScreen")
                                {
                                    var details = dt.AsEnumerable().Where(x => x.Field<string>("Machineid") == machine && x.Field<string>("Name") == yaxis).Select(x => new { actual = x.Field<double>("Qty"), plan = x.Field<double>("PlnQty") }).FirstOrDefault();
                                    actualData.Add(Convert.ToDouble(details.actual));
                                    planData.Add(Convert.ToDouble(details.plan));

                                    planName = "Plan"; actulaName = "Actual";
                                }
                                else if (screenType == "RejectionScreen")
                                {
                                    var details = dt.AsEnumerable().Where(x => x.Field<string>("Machineid") == machine && x.Field<string>("Name") == yaxis).Select(x => new { actual = x.Field<double>("Qty"), plan = x.Field<double>("ProdQty") }).FirstOrDefault();
                                    actualData.Add(Convert.ToDouble(details.actual));
                                    planData.Add(Convert.ToDouble(details.plan));

                                    planName = "Production"; actulaName = "Actual";
                                }


                                if (machineCount == 0)
                                {
                                    if (selectedMenu.Equals("DayDataMenu", StringComparison.OrdinalIgnoreCase))
                                    {
                                        categoryList.Add("Day " + yaxis.Split(' ')[0].Split('-')[2]);
                                    }
                                    else if (selectedMenu.Equals("WeekDataMenu", StringComparison.OrdinalIgnoreCase))
                                    {
                                        categoryList.Add("W " + weekCount);
                                        weekCount++;
                                    }
                                    else
                                    {
                                        categoryList.Add(yaxis);
                                    }

                                }
                            }

                            // string color = colors[machineCount];
                            seriesData = new LineChartSeriesDetails();
                            seriesData.name = machine + " - " + actulaName;
                            //  seriesData.color = color;
                            seriesData.data = actualData;
                            lineSeries.Add(seriesData);

                            seriesData = new LineChartSeriesDetails();
                            seriesData.name = machine + " - " + planName;
                            //  seriesData.color = transaperentColors[machineCount];
                            seriesData.data = planData;
                            lineSeries.Add(seriesData);
                            machineCount++;

                        }


                        finalData.title = selectedMenu.Equals("DayDataMenu", StringComparison.OrdinalIgnoreCase) ? "Day Wise Chart" : selectedMenu.Equals("WeekDataMenu", StringComparison.OrdinalIgnoreCase) ? "Week Wise Chart" : selectedMenu.Equals("MonthDataMenu", StringComparison.OrdinalIgnoreCase) ? "Month Wise Chart" : "";
                        finalData.Categories = categoryList;
                        finalData.LineChartSeriesDetails = lineSeries;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetDownChartDetails: " + ex.Message);
            }
            return finalData;
        }
        public static string GenerateRgba(string backgroundColor, double backgroundOpacity)
        {
            Color color = ColorTranslator.FromHtml(backgroundColor);
            int r = Convert.ToInt16(color.R);
            int g = Convert.ToInt16(color.G);
            int b = Convert.ToInt16(color.B);
            var hexString = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            // var hexString = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, backgroundOpacity);
            // string htmlColor = ColorTranslator.ToHtml(String.Format("rgba({0}, {1}, {2}, {3});", r, g, b, backgroundOpacity));
            var hex = String.Format("#{0}{1}{2}{3}"
            , color.A.ToString("X").Length == 1 ? String.Format("0{0}", backgroundOpacity.ToString("X")) : backgroundOpacity.ToString("X")
            , color.R.ToString("X").Length == 1 ? String.Format("0{0}", color.R.ToString("X")) : color.R.ToString("X")
            , color.G.ToString("X").Length == 1 ? String.Format("0{0}", color.G.ToString("X")) : color.G.ToString("X")
            , color.B.ToString("X").Length == 1 ? String.Format("0{0}", color.B.ToString("X")) : color.B.ToString("X"));
            return hexString;
        }
    }
}