using BusinessClassLibrary;
using Elmah;
using MachineStatusPage.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class tableView : System.Web.UI.Page
    {
        public static ICockpitStyle values = CockpitDataBaseAccess.GetCockpitBackColorValues();
        public static MachineStatusColorStyle machineStatusColors = CockpitDataBaseAccess.GetMachineStatusColorValues();
        string defaultShift = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                if (!IsPostBack)
                {
                    //txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy HH:mm tt");
                    //txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm tt");
                    values = CockpitDataBaseAccess.GetCockpitBackColorValues();
                    SessionClear.ClearSession();
                    int setFilterFromSession = 0;
                    IconicTableFilterEntity filterData = new IconicTableFilterEntity();
                    string backButtonText = "";
                    if (Page.ClientQueryString.Length > 0)
                    {
                        if (Request.QueryString["setFilter"] != null)
                        {
                            if (Request.QueryString["setFilter"].ToString() == "1" && Session["LiveIconicTableViewFilter"] != null)
                            {
                                setFilterFromSession = 1;
                                filterData = Session["LiveIconicTableViewFilter"] as IconicTableFilterEntity;
                                backButtonText = Request.QueryString["backButtonText"].ToString();
                            }
                        }
                    }
                    BindDayShift();
                    //if (Session["FromDate"] != null && Session["ToDate"] != null)
                    //{
                    //    txtFromDate.Text = Convert.ToDateTime(Session["FromDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    //    txtToDate.Text = Convert.ToDateTime(Session["ToDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    //}
                    //else
                    //{
                    gettimings();
                    //}
                    timerDataChange.Enabled = false;
                    BindPlantId();
                    if (setFilterFromSession == 1)
                    {
                        txtFromDate.Text = filterData.FromDate;
                        txtToDate.Text = filterData.ToDate;
                        HelperClassGeneric.setDropdownValue(ddlDayShift, filterData.PredefinedTime);
                        HelperClassGeneric.setDropdownValue(ddlPlantId, filterData.PlantId);
                    }
                    BindCellId(ddlPlantId.SelectedItem == null ? "" : ddlPlantId.SelectedItem.Text);
                    //ddlDayShift_SelectedIndexChanged("frist", null);
                    //BindTableData();
                    if (setFilterFromSession == 1)
                    {
                        HelperClassGeneric.clearListBoxValue(lbCellID);
                        HelperClassGeneric.setListBoxValue(lbCellID, filterData.CellId);
                        HelperClassGeneric.setDropdownValue(ddlView, filterData.View);
                    }
                    else
                    {
                        setDefaultView();
                    }
                    ddlView_SelectedIndexChanged(null, null);
                    if (setFilterFromSession == 1)
                    {
                        HelperClassGeneric.setDropdownValue(ddlSortOrder, filterData.SortOrder);
                    }
                    btnProcess_Click1(null, null);
                    if (backButtonText != "")
                    {
                        Session["LATableViewsStack"] = Session["LAIonicViewsStack"] as List<LVIonicViewStack>;
                        lbBackButton.Text = backButtonText;
                        tdBackBtn.Visible = true;
                        lbBackButton.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //rrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void setDefaultView()
        {
            string view = CockpitDataBaseAccess.getHistoricalAnalyticsDefaultView("LiveDefaultView", "Table");
            if (view != "")
            {
                ddlView.SelectedValue = view;
                // Session["CurrentView"] = ddlView.SelectedValue;
            }
        }
        private void gettimings()
        {
            defaultShift = DataBaseAccess.GetDefaultCockpitDefaultShift();

            if (defaultShift.Equals("PreviousShift"))
            {
                var prevShiftVals = DataBaseAccess.GetCurrentOrPreviousShiftVals("[s_GetPreviousShift]");
                if (prevShiftVals != null)
                {
                    SetDateTimeToControl(prevShiftVals);
                    DateTime logicalDayStart = Convert.ToDateTime(prevShiftVals[2]);
                    string ddlValue = "Today";
                    if (logicalDayStart.Date != DateTime.Now.Date)
                    {
                        ddlValue = "Yesterday";
                    }
                    ddlValue = ddlValue + " - " + prevShiftVals[3];
                    HelperClassGeneric.setDropdownValue(ddlDayShift, ddlValue);
                }
            }
            else if (defaultShift.Equals("CurrentShift"))
            {
                var currShiftVals = DataBaseAccess.GetCurrentOrPreviousShiftVals("[s_GetCurrentShift]");
                if (currShiftVals != null)
                {
                    SetDateTimeToControl(currShiftVals);
                    string ddlValue = "Today" + " - " + currShiftVals[3];
                    HelperClassGeneric.setDropdownValue(ddlDayShift, ddlValue);
                }
            }
            else if (defaultShift.Equals("Last 24Hrs"))
            {
                string fromdate = DateTime.Today.AddDays(-1).ToString("dd-MM-yyyy");
                txtFromDate.Text = fromdate + DateTime.Now.ToString("HH:mm:ss");
                string todate = DateTime.Today.ToString("dd-MM-yyyy");
                txtToDate.Text = todate + DateTime.Now.ToString("HH:mm:ss");
            }
        }

        private void SetDateTimeToControl(List<string> shiftVals)
        {
            txtFromDate.Text = Convert.ToDateTime(shiftVals[0]).ToString("dd-MM-yyyy HH:mm");
            txtToDate.Text = Convert.ToDateTime(shiftVals[1]).ToString("dd-MM-yyyy HH:mm");
        }

        private bool CheckDateRange()
        {
            bool isDateInRange = false;
            try
            {
                DateTime dt1 = Util.GetDateTime(txtFromDate.Text);
                DateTime dt2 = Util.GetDateTime(txtToDate.Text);
                var hours = (dt2 - dt1).TotalHours;

                if (Math.Abs(hours) <= 168)
                {
                    isDateInRange = true;
                }
            }
            catch (Exception ex)
            {
                //rrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
            return isDateInRange;
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
                // //rrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        #endregion

        #region "Bind Cell Id"
        private void BindCellId(string plantId)
        {
            try
            {
                List<string> lstCellId = BindCockpitView.ViewCellsToDisplay(plantId == "Plant All" ? "" : plantId);
                lbCellID.DataSource = lstCellId;
                lbCellID.DataBind();
                foreach (ListItem item in lbCellID.Items)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                //rrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        #endregion

        #region "Day Shift Data"
        private void BindDayShift()
        {
            try
            {
                List<string> lstPlantData = CockpitDataBaseAccess.GetAllPredefinedShifts();
                ddlDayShift.DataSource = lstPlantData;
                ddlDayShift.DataBind();
                //ddlDayShift.SelectedIndex = 3;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        #endregion

        #region "Day Shift Wise change Data"
        protected void ddlDayShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedShift = ddlDayShift.SelectedValue.ToString();
                string logicalDayStart = string.Empty, logicalDayEnd = string.Empty;
                if (selectedShift.Contains("Today"))
                {
                    if (ddlDayShift.SelectedValue.Equals("Today - All"))
                    {
                        logicalDayStart = CockpitDataBaseAccess.GetLogicalDay(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("dd-MM-yyyy HH:mm");
                        //dtpTimeFrom.Value = Convert.ToDateTime(logicalDayStart);

                        logicalDayEnd = CockpitDataBaseAccess.GetLogicalDayEnd(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("dd-MM-yyyy HH:mm");
                        //dtpTimeTo.Value = Convert.ToDateTime(logicalDayEnd);
                    }

                    else
                    {
                        int index = selectedShift.IndexOf('-');
                        var shift = CockpitDataBaseAccess.GetCurrentShiftTime(selectedShift.Substring(index + 1).Trim());
                        if (shift != null)
                        {
                            logicalDayStart = shift[0];
                            logicalDayEnd = shift[1];

                            txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("dd-MM-yyyy HH:mm");
                            //dtpTimeFrom.Value = Convert.ToDateTime(logicalDayStart);

                            txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("dd-MM-yyyy HH:mm");
                            //dtpTimeTo.Value = Convert.ToDateTime(logicalDayEnd);
                        }
                    }
                }
                else
                {

                    if (ddlDayShift.SelectedValue.Equals("Yesterday - All"))
                    {
                        logicalDayStart = CockpitDataBaseAccess.GetLogicalDay(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
                        txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("dd-MM-yyyy HH:mm");
                        //dtpTimeFrom.Value = Convert.ToDateTime(logicalDayStart);

                        logicalDayEnd = CockpitDataBaseAccess.GetLogicalDayEnd(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
                        txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("dd-MM-yyyy HH:mm");
                        //dtpTimeTo.Value = Convert.ToDateTime(logicalDayEnd);
                    }
                    else
                    {
                        int index = selectedShift.IndexOf('-');
                        var shift = CockpitDataBaseAccess.GetCurrentShiftTime(selectedShift.Substring(index + 1).Trim());
                        if (shift != null)
                        {
                            logicalDayStart = shift[0];
                            logicalDayEnd = shift[1];

                            txtFromDate.Text = Convert.ToDateTime(logicalDayStart).AddDays(-1).ToString("dd-MM-yyyy HH:mm");
                            //dtpTimeFrom.Value = Convert.ToDateTime(logicalDayStart);

                            txtToDate.Text = Convert.ToDateTime(logicalDayEnd).AddDays(-1).ToString("dd-MM-yyyy HH:mm");
                            //dtpTimeTo.Value = Convert.ToDateTime(logicalDayEnd);
                        }
                    }
                }
                if (!sender.ToString().Equals("frist", StringComparison.OrdinalIgnoreCase))
                    bindCurrentData();
            }
            catch (Exception ex)
            {
                //rrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        #endregion
        private List<ListItem> getStorkeColumn()
        {
            List<ListItem> list = new List<ListItem>();
            try
            {
                List<string> listOfColNames = new List<string>();
                List<string> val = CockpitDataBaseAccess.GetOrderedLabels(out listOfColNames, "WebTPMTrakTableView", Session["Language"] == null ? "en" : Session["Language"].ToString());
                if (listOfColNames.Contains("NoOfStrokes"))
                {
                    list.Add(new ListItem() { Text = "Stroke Count", Value = "NoOfStrokes" });
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return list;
        }
        private void BindTableData(string view, DataTable dt)
        {
            try
            {
                List<string> dtColumnList = dt.Columns.Cast<DataColumn>().Select(k => k.ColumnName).ToList();
                if (view == "Machine")
                {

                    List<string> listOfColNames = new List<string>();
                    List<string> val = CockpitDataBaseAccess.GetOrderedLabels(out listOfColNames, "WebTPMTrakTableView", Session["Language"] == null ? "en" : Session["Language"].ToString());
                    for (int i = 0; i < listOfColNames.Count; i++)
                    {
                        if (listOfColNames[i].Equals("OPEffy", StringComparison.OrdinalIgnoreCase))
                        {
                            listOfColNames[i] = "OperatorEfficiency";
                        }
                        else if (listOfColNames[i].Equals("OverallOPEffy", StringComparison.OrdinalIgnoreCase))
                        {
                            listOfColNames[i] = "OverallOperatorEfficiency";
                        }
                        if (dtColumnList.Contains(listOfColNames[i], StringComparer.OrdinalIgnoreCase))
                        {
                            BoundField boundfield = new BoundField();

                            boundfield.DataField = listOfColNames[i].ToString();
                            boundfield.HeaderText = val[i].ToString();
                            if (listOfColNames[i].Equals("OverAllEfficiency", StringComparison.OrdinalIgnoreCase) || listOfColNames[i].Equals("ProductionEfficiency", StringComparison.OrdinalIgnoreCase) ||
                                listOfColNames[i].Equals("AvailabilityEfficiency", StringComparison.OrdinalIgnoreCase) || listOfColNames[i].Equals("PrevOEE", StringComparison.OrdinalIgnoreCase))
                            {
                                boundfield.HeaderStyle.CssClass = "SortClick";
                                if (listOfColNames[i].Equals("OverAllEfficiency", StringComparison.OrdinalIgnoreCase))
                                {
                                    boundfield.SortExpression = "Overallefficiency";
                                }
                                else if (listOfColNames[i].Equals("ProductionEfficiency", StringComparison.OrdinalIgnoreCase))
                                {
                                    boundfield.SortExpression = "ProductionEfficiency";
                                }
                                else if (listOfColNames[i].Equals("AvailabilityEfficiency", StringComparison.OrdinalIgnoreCase))
                                {
                                    boundfield.SortExpression = "AvailabilityEfficiency";
                                }
                                boundfield.DataFormatString = "{0:F0}";
                            }
                            //boundfield.HeaderStyle.Width = new Unit("100px");
                            gridviewTableData.Columns.Add(boundfield);
                        }
                    }
                    // BindListItem();
                }
                else
                {
                    List<string> listOfColNames = new List<string>();
                    List<string> val = CockpitDataBaseAccess.GetOrderedLabels(out listOfColNames, "WebCockpitGridColumnAggregate", Session["Language"] == null ? "en" : Session["Language"].ToString());
                    //if (ConfigurationManager.AppSettings["AmararagaMangalPages"].ToString() == "1")
                    //{
                    //    List<ListItem> list = getStorkeColumn();
                    //    if (list.Count > 0)
                    //    {
                    //        val.Add(list[0].Text);
                    //        listOfColNames.Add(list[0].Value);
                    //    }
                    //    //if (val != null)
                    //    //{
                    //    //    val.Add("Stroke Count");
                    //    //    listOfColNames.Add("NoOfStrokes");
                    //    //}
                    //}
                    for (int i = 0; i < listOfColNames.Count; i++)
                    {
                        if (listOfColNames[i].Equals("OPEffy", StringComparison.OrdinalIgnoreCase))
                        {
                            if (view == "Cell")
                            {
                                listOfColNames[i] = "Offy";
                            }
                            else
                            {
                                listOfColNames[i] = "OPREffy";
                            }
                        }
                        else if (listOfColNames[i].Equals("OverallOPEffy", StringComparison.OrdinalIgnoreCase))
                        {
                            if (view == "Cell")
                            {
                                listOfColNames[i] = "OOffy";
                            }
                            else
                            {
                                listOfColNames[i] = "OverallOPREffy";
                            }
                        }
                        if (dtColumnList.Contains(listOfColNames[i], StringComparer.OrdinalIgnoreCase))
                        {
                            BoundField boundfield = new BoundField();

                            boundfield.DataField = listOfColNames[i].ToString();
                            boundfield.HeaderText = val[i].ToString();
                            if (listOfColNames[i].Equals("OEffy", StringComparison.OrdinalIgnoreCase) || listOfColNames[i].Equals("PEffy", StringComparison.OrdinalIgnoreCase) ||
                                listOfColNames[i].Equals("AEffy", StringComparison.OrdinalIgnoreCase) || listOfColNames[i].Equals("PrevOEE", StringComparison.OrdinalIgnoreCase))
                            {
                                if (listOfColNames[i].Equals("OEffy", StringComparison.OrdinalIgnoreCase))
                                {
                                    boundfield.SortExpression = "OEffy";
                                }
                                else if (listOfColNames[i].Equals("PEffy", StringComparison.OrdinalIgnoreCase))
                                {
                                    boundfield.SortExpression = "PEffy";
                                }
                                else if (listOfColNames[i].Equals("AEffy", StringComparison.OrdinalIgnoreCase))
                                {
                                    boundfield.SortExpression = "AEffy";
                                }
                                boundfield.DataFormatString = "{0:F0}";
                            }
                            //boundfield.HeaderStyle.Width = new Unit("100px");
                            if (view == "Plant")
                            {
                                gvPlantTableData.Columns.Add(boundfield);
                            }
                            else
                            {
                                gvCellTableData.Columns.Add(boundfield);
                            }

                        }
                    }
                    //BindListItem();
                }
            }
            catch (Exception ex)
            {
                //rrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindListItem()
        {
            try
            {
                string strDate = string.Empty, shift = string.Empty, endDate = string.Empty;
                DateTime FromDate = DateTime.Now.Date;
                DateTime ToDate = DateTime.Now.Date;
                FromDate = Util.GetDateTime(txtFromDate.Text.Trim());
                ToDate = Util.GetDateTime(txtToDate.Text.Trim());
                Session["FromDateAgg"] = FromDate.ToString("yyyy-MM-dd HH:mm:ss");
                Session["ToDateAgg"] = ToDate.ToString("yyyy-MM-dd HH:mm:ss");
                if (DateTime.Compare(FromDate, ToDate) > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('From date cannot be greater than To date.')", true);
                    FromDate = DateTime.Now;
                    ToDate = DateTime.Now.AddDays(1);
                    txtFromDate.Text = FromDate.ToString("dd-MM-yyyy hh:mm");
                    txtToDate.Text = ToDate.ToString("dd-MM-yyyy hh:mm");
                }

                if ((ToDate - FromDate).TotalDays > 15)
                {
                    lblMessages.Text = "Difference between to date and from date cannot be more than 15 days.";
                    return;
                }
                else
                {
                    strDate = FromDate.ToSQLDateTimeFormat();
                    endDate = ToDate.ToSQLDateTimeFormat();
                    string selectedPlant = ddlPlantId.SelectedValue.ToString();
                    string cellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    if (selectedPlant == "All")
                        selectedPlant = "";
                    if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase))
                        cellId = "";
                    DataTable dt = new DataTable();
                    dt = CachedData.getChachedData(strDate, endDate);
                    if (dt == null)
                    {
                        if (CheckDateRange())
                        {
                            //dt = CockpitDataBaseAccess.GetTabelCockpitDetails("[s_GetCockpitData_WithTempTable_eshopx]", strDate, endDate, selectedPlant, cellId);
                        }
                        else
                        {
                            // dt = CockpitDataBaseAccess.GetTabelCockpitDetails("[s_GetCockpitData_eshopx]", strDate, endDate, selectedPlant, cellId);
                        }
                    }
                    DataTable dtIncremented = new DataTable();

                    DataColumn SlNo = new DataColumn();
                    SlNo.ColumnName = "SlNo";
                    SlNo.AutoIncrement = true;
                    SlNo.AutoIncrementSeed = 1;
                    SlNo.AutoIncrementStep = 1;
                    SlNo.DataType = typeof(Int32);
                    dtIncremented.Columns.Add(SlNo);
                    DataTableReader dtReader = new DataTableReader(dt);
                    dtIncremented.BeginLoadData();
                    dtIncremented.Load(dtReader);
                    dtIncremented.EndLoadData();

                    Session["TableViewData"] = dt;
                    Session["SortnumberMachineId"] = 0;
                    Session["SortnumberbyOverallefficiency"] = 0;
                    Session["SortnumberbyProductionEfficiency"] = 0;
                    Session["SortnumberbyAvailabilityEfficiency"] = 0;
                    Session["SortnumberbyPrevOEE"] = 0;
                    gridviewTableData.DataSource = dtIncremented;
                    gridviewTableData.DataBind();
                    //gridviewTableData.UseAccessibleHeader = true;
                    //if (dt != null && dt.Rows.Count > 0)
                    //    gridviewTableData.HeaderRow.TableSection = TableRowSection.TableHeader;

                }
            }
            catch (Exception ex)
            {
                //rrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void gridviewTableData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //int remark = GetColumnIndexByDBName(this.gridviewTableData, "Remarks1");
                    //if (remark != -1)
                    //{
                    //    DataRow row = ((DataRowView)e.Row.DataItem).Row;
                    //    String name = row.Field<String>("Remarks1");
                    //    e.Row.Cells[remark].Text = "";
                    //    e.Row.Cells[remark].Attributes.Add("class", "loaders-container la-cog la-2x " + name + "");
                    //}
                    int colIndex = GetColumnIndexByDBName(this.gridviewTableData, "ProductionEfficiency");
                    if (colIndex != -1)
                    {
                        double PEGreenVal = 0, PEval = 0, PERedVal = 0;
                        if (drv["ProductionEfficiency"] != DBNull.Value)
                        {
                            PEval = Convert.ToDouble(drv["ProductionEfficiency"]);
                        }
                        if (drv["PEGreen"] != DBNull.Value)
                        {
                            PEGreenVal = Convert.ToDouble(drv["PEGreen"]);
                        }
                        if (drv["PERed"] != DBNull.Value)
                        {
                            PERedVal = Convert.ToDouble(drv["PERed"]);
                        }

                        if (PEval <= PERedVal && PEval > 0)
                        {
                            e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));
                        }
                        else if (PEval >= PEGreenVal)
                        {
                            if (ConfigurationManager.AppSettings["DantalHydraulicsPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                            {
                                if (PEval > 100 && !string.IsNullOrEmpty(values.PEGreaterThanHundredBackColor))
                                {
                                    e.Row.Cells[colIndex].ToolTip = "Check Cycle Time";
                                    e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.PEGreaterThanHundredBackColor.Substring(1));
                                }
                                else
                                    e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));// ColorTranslator.FromHtml(values.GoodRunning);//Color.Green(GOOD);
                            }
                            else
                                e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));// ColorTranslator.FromHtml(values.GoodRunning);//Color.Green(GOOD);
                        }
                        else if (PEval > PERedVal && PEval < PEGreenVal)
                        {
                            e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//ColorTranslator.FromHtml(values.ModeratelyRunning);//Color.Yellow(MODERATE);;
                        }
                        //e.Row.Cells[colIndex].BackColor = GetColorFrom(name); //System.Drawing.Color.FromName(name);
                    }
                    int colIndex1 = GetColumnIndexByDBName(this.gridviewTableData, "OverAllEfficiency");
                    if (colIndex1 != -1)
                    {
                        e.Row.Cells[colIndex1].CssClass = "hypercol HL_OEE";
                        double OEval = 0, OEGreenVal = 0, OERedVal = 0;
                        if (drv["OverAllEfficiency"] != DBNull.Value)
                        {
                            OEval = Convert.ToDouble(drv["OverAllEfficiency"]);
                        }
                        if (drv["OEGreen"] != DBNull.Value)
                        {
                            OEGreenVal = Convert.ToDouble(drv["OEGreen"]);
                        }
                        if (drv["OERed"] != DBNull.Value)
                        {
                            OERedVal = Convert.ToDouble(drv["OERed"]);
                        }
                        if (OEval <= OERedVal && OEval > 0)
                        {
                            e.Row.Cells[colIndex1].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));//Color.Red(BAD);
                        }
                        else if (OEval >= OEGreenVal)
                        {
                            e.Row.Cells[colIndex1].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));//Color.Green(GOOD);
                        }
                        else if (OEval > OERedVal && OEval < OEGreenVal)
                        {
                            e.Row.Cells[colIndex1].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//Color.Yellow(MODERATE);
                        }
                    }
                    int colindex2 = GetColumnIndexByDBName(this.gridviewTableData, "AvailabilityEfficiency");
                    if (colindex2 != -1)
                    {
                        double AEval = 0, AEGreenVal = 0, AERedVal = 0;
                        if (drv["AvailabilityEfficiency"] != DBNull.Value)
                        {
                            AEval = Convert.ToDouble(drv["AvailabilityEfficiency"]);
                        }
                        if (drv["AEGreen"] != DBNull.Value)
                        {
                            AEGreenVal = Convert.ToDouble(drv["AEGreen"]);
                        }
                        if (drv["AERed"] != DBNull.Value)
                        {
                            AERedVal = Convert.ToDouble(drv["AERed"]);
                        }
                        if (AEval <= AERedVal && AEval > 0)
                        {
                            e.Row.Cells[colindex2].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));//Color.Red (BAD);
                        }
                        else if (AEval >= AEGreenVal)
                        {
                            e.Row.Cells[colindex2].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));//Color.Green(GOOD);
                        }
                        else if (AEval > AERedVal && AEval < AEGreenVal)
                        {
                            e.Row.Cells[colindex2].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//Color.Yellow(MODERATE);
                        }
                    }
                    int colindex3 = GetColumnIndexByDBName(this.gridviewTableData, "QualityEfficiency");
                    if (colindex3 != -1)
                    {
                        double QEval = 0, QEGreenVal = 0, QERedVal = 0;
                        if (drv["QualityEfficiency"] != DBNull.Value)
                        {
                            QEval = Convert.ToDouble(drv["QualityEfficiency"]);
                        }
                        if (drv["QEGreen"] != DBNull.Value)
                        {
                            QEGreenVal = Convert.ToDouble(drv["QEGreen"]);
                        }
                        if (drv["QERed"] != DBNull.Value)
                        {
                            QERedVal = Convert.ToDouble(drv["QERed"]);
                        }
                        if (QEval <= QERedVal && QEval > 0)
                        {
                            e.Row.Cells[colindex3].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));//Color.Red (BAD);
                        }
                        else if (QEval >= QEGreenVal)
                        {
                            e.Row.Cells[colindex3].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));//Color.Green(GOOD);
                        }
                        else if (QEval > QERedVal && QEval < QEGreenVal)
                        {
                            e.Row.Cells[colindex3].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//Color.Yellow(MODERATE);
                        }
                    }
                    int colIndex4 = GetColumnIndexByDBName(this.gridviewTableData, "AirPressureCnt");
                    if (colIndex4 != -1)
                    {
                        e.Row.Cells[colIndex4].CssClass = "hypercol HL_AirPressure";
                    }
                    colIndex4 = GetColumnIndexByDBName(this.gridviewTableData, "SpindleRunTime");
                    if (colIndex4 != -1)
                    {
                        e.Row.Cells[colIndex4].CssClass = "hypercol HL_SpindleRuntime";
                    }
                    int colindex5 = GetColumnIndexByDBName(this.gridviewTableData, "OperatorEfficiency");
                    if (colindex5 != -1)
                    {
                        double OPRval = 0, OPRGreenVal = 0, OPRRedVal = 0;
                        if (drv["OperatorEfficiency"] != DBNull.Value)
                        {
                            OPRval = Convert.ToDouble(drv["OperatorEfficiency"]);
                        }
                        if (drv["OPRGreen"] != DBNull.Value)
                        {
                            OPRGreenVal = Convert.ToDouble(drv["OPRGreen"]);
                        }
                        if (drv["OPRRed"] != DBNull.Value)
                        {
                            OPRRedVal = Convert.ToDouble(drv["OPRRed"]);
                        }
                        if (OPRval <= OPRRedVal && OPRval > 0)
                        {
                            e.Row.Cells[colindex5].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));//Color.Red (BAD);
                        }
                        else if (OPRval >= OPRGreenVal)
                        {
                            e.Row.Cells[colindex5].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));//Color.Green(GOOD);
                        }
                        else if (OPRval > OPRRedVal && OPRval < OPRGreenVal)
                        {
                            e.Row.Cells[colindex5].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//Color.Yellow(MODERATE);
                        }
                    }
                    int colindex6 = GetColumnIndexByDBName(this.gridviewTableData, "OverallOperatorEfficiency");
                    if (colindex6 != -1)
                    {
                        double OEval = 0, OEGreenVal = 0, OERedVal = 0;
                        if (drv["OverallOperatorEfficiency"] != DBNull.Value)
                        {
                            OEval = Convert.ToDouble(drv["OverallOperatorEfficiency"]);
                        }
                        if (drv["OEGreen"] != DBNull.Value)
                        {
                            OEGreenVal = Convert.ToDouble(drv["OEGreen"]);
                        }
                        if (drv["OERed"] != DBNull.Value)
                        {
                            OERedVal = Convert.ToDouble(drv["OERed"]);
                        }
                        if (OEval <= OERedVal && OEval > 0)
                        {
                            e.Row.Cells[colindex6].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));//Color.Red(BAD);
                        }
                        else if (OEval >= OEGreenVal)
                        {
                            e.Row.Cells[colindex6].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));//Color.Green(GOOD);
                        }
                        else if (OEval > OERedVal && OEval < OEGreenVal)
                        {
                            e.Row.Cells[colindex6].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//Color.Yellow(MODERATE);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //rrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        public Color GetColorFrom(string colorName)
        {
            try
            {
                if (colorName.Equals("Green", StringComparison.OrdinalIgnoreCase))
                {
                    return System.Drawing.ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));
                }
                else if (colorName.Equals("Yellow", StringComparison.OrdinalIgnoreCase))
                {
                    return System.Drawing.ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));
                }
                else if (colorName.Equals("Red", StringComparison.OrdinalIgnoreCase))
                {
                    return System.Drawing.ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));
                }
                return Color.White;
            }
            catch (Exception ex)
            {
                //rrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
        }

        static public int GetColumnIndexByDBName(GridView aGridView, String ColumnText)
        {
            try
            {
                BoundField DataColumn;
                for (int Index = 0; Index < aGridView.Columns.Count; Index++)
                {
                    DataColumn = aGridView.Columns[Index] as BoundField;

                    if (DataColumn != null)
                    {
                        if (DataColumn.DataField == ColumnText)
                            return Index;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                //rrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
        }

        static public DataControlFieldCell GetCellByName(GridViewRow Row, String CellName)
        {
            try
            {
                foreach (DataControlFieldCell Cell in Row.Cells)
                {
                    if (Cell.ContainingField.ToString() == CellName)
                        return Cell;
                }
                return null;
            }
            catch (Exception ex)
            {
                //rrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
        }

        protected void btnProcess_Click1(object sender, EventArgs e)
        {
            // BindListItem();
            tdBackBtn.Visible = false;
            Session["LATableViewsStack"] = null;
            //  Session["CurrentView"] = ddlView.SelectedValue;
            if (ddlView.SelectedValue == "Plantwise")
            {
                BindPlantItem(ddlPlantId.SelectedValue);

            }
            else if (ddlView.SelectedValue == "cellwise")
            {
                BindCellItem(ddlPlantId.SelectedValue, DataBaseAccess.getCellIDWithSeparator(lbCellID));
            }
            else if (ddlView.SelectedValue == "Machinewise")
            {
                BindMachineItem(ddlPlantId.SelectedValue, DataBaseAccess.getCellIDWithSeparator(lbCellID), ddlSortOrder.SelectedValue);
            }
            hdnView.Value = ddlView.SelectedValue;
        }
        private void setFilterValueToSession()
        {
            IconicTableFilterEntity filterData = new IconicTableFilterEntity();
            filterData.FromDate = txtFromDate.Text;
            filterData.ToDate = txtToDate.Text;
            filterData.PlantId = ddlPlantId.SelectedValue;
            filterData.CellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
            filterData.View = ddlView.SelectedValue;
            filterData.PredefinedTime = ddlDayShift.SelectedValue;
            filterData.SortOrder = ddlSortOrder.SelectedValue;
            Session["LiveIconicTableViewFilter"] = filterData;
        }
        private void BindPlantItem(string plantid)
        {
            try
            {
                setFilterValueToSession();
                hdnView.Value = ddlView.SelectedValue;
                string strDate = string.Empty, shift = string.Empty, endDate = string.Empty;
                DateTime FromDate = DateTime.Now.Date;
                DateTime ToDate = DateTime.Now.Date;
                FromDate = Util.GetDateTime(txtFromDate.Text.Trim());
                ToDate = Util.GetDateTime(txtToDate.Text.Trim());
                Session["FromDateAgg"] = FromDate.ToString("yyyy-MM-dd HH:mm:ss");
                Session["ToDateAgg"] = ToDate.ToString("yyyy-MM-dd HH:mm:ss");
                if (DateTime.Compare(FromDate, ToDate) > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('From date cannot be greater than To date.')", true);
                    //FromDate = DateTime.Now;
                    //ToDate = DateTime.Now.AddDays(1);
                    //txtFromDate.Text = FromDate.ToString("dd-MM-yyyy hh:mm");
                    //txtToDate.Text = ToDate.ToString("dd-MM-yyyy hh:mm");
                }
                int interval = CockpitDataBaseAccess.getLiveCockpitDateInterval();
                if ((ToDate - FromDate).TotalDays > interval)
                {
                    lblMessages.Text = "Difference between to date and from date cannot be more than " + interval + " days.<br> Please visit Historical Analytics -> Table View to see more than " + interval + " days of data.";
                    return;
                }
                else
                {

                    strDate = FromDate.ToSQLDateTimeFormat();
                    endDate = ToDate.ToSQLDateTimeFormat();

                    //  string selectedPlant = ddlPlantId.SelectedValue.ToString();
                    string cellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    if (plantid == "All")
                        plantid = "";
                    if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase))
                        cellId = "";
                    DataTable dt = null;
                    // dt = CachedData.getChachedData(strDate, endDate);
                    if (dt == null)
                    {
                        //if (CheckDateRange())
                        //{
                        dt = CockpitDataBaseAccess.GetTabelCockpitDetails("[s_GetCockpitData_WithTempTable_eshopx]", strDate, endDate, plantid, cellId, "Plantwise", ddlSortOrder.SelectedValue, DataBaseAccess.getMachineIDWithSeparatorForScreen(cellId, "IconicCockpitLive"));
                        if (gvPlantTableData.Columns.Count == 1)
                        {
                            BindTableData("Plant", dt);
                        }
                        //}
                        //else
                        //{
                        //    dt = CockpitDataBaseAccess.GetTabelCockpitDetails("[s_GetCockpitData_eshopx]", strDate, endDate, plantid, cellId, "Plantwise", ddlSortOrder.SelectedValue);
                        //}
                    }
                    Session["TableViewData"] = dt;
                    Session["SortnumberMachineId"] = 0;
                    Session["SortnumberbyOverallefficiency"] = 0;
                    Session["SortnumberbyProductionEfficiency"] = 0;
                    Session["SortnumberbyAvailabilityEfficiency"] = 0;
                    Session["SortnumberbyPrevOEE"] = 0;
                    gvPlantTableData.DataSource = dt;
                    gvPlantTableData.DataBind();
                    gvPlantTableData.Visible = true;
                    gvCellTableData.Visible = false;
                    gridviewTableData.Visible = false;
                    lbBackButton.Visible = false;
                    tdBackBtn.Visible = false;
                    clearGridData(gvCellTableData, 1);
                    clearGridData(gridviewTableData, 2);
                    //gridviewTableData.UseAccessibleHeader = true;
                    //if (dt != null && dt.Rows.Count > 0)
                    //    gridviewTableData.HeaderRow.TableSection = TableRowSection.TableHeader;

                }
            }
            catch (Exception ex)
            {
                //rrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void BindCellItem(string plantid, string cellId)
        {
            try
            {
                setFilterValueToSession();
                hdnView.Value = ddlView.SelectedValue;
                string strDate = string.Empty, shift = string.Empty, endDate = string.Empty;
                DateTime FromDate = DateTime.Now.Date;
                DateTime ToDate = DateTime.Now.Date;
                FromDate = Util.GetDateTime(txtFromDate.Text.Trim());
                ToDate = Util.GetDateTime(txtToDate.Text.Trim());
                Session["FromDateAgg"] = FromDate.ToString("yyyy-MM-dd HH:mm:ss");
                Session["ToDateAgg"] = ToDate.ToString("yyyy-MM-dd HH:mm:ss");
                if (DateTime.Compare(FromDate, ToDate) > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('From date cannot be greater than To date.')", true);
                    //FromDate = DateTime.Now;
                    //ToDate = DateTime.Now.AddDays(1);
                    //txtFromDate.Text = FromDate.ToString("dd-MM-yyyy hh:mm");
                    //txtToDate.Text = ToDate.ToString("dd-MM-yyyy hh:mm");
                }
                int interval = CockpitDataBaseAccess.getLiveCockpitDateInterval();
                if ((ToDate - FromDate).TotalDays > interval)
                {
                    lblMessages.Text = "Difference between to date and from date cannot be more than " + interval + " days. <br> Please visit Historical Analytics -> Table View to see more than " + interval + " days of data.";
                    return;
                }
                else
                {

                    strDate = FromDate.ToSQLDateTimeFormat();
                    endDate = ToDate.ToSQLDateTimeFormat();
                    //  string selectedPlant = ddlPlantId.SelectedValue.ToString();
                    // string cellId = ddlCellID.SelectedValue.ToString();
                    if (plantid == "All")
                        plantid = "";
                    if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase))
                        cellId = "";
                    DataTable dt = null;
                    //dt = CachedData.getChachedData(strDate, endDate);
                    if (dt == null)
                    {
                        //if (CheckDateRange())
                        {
                            dt = CockpitDataBaseAccess.GetTabelCockpitDetails("[s_GetCockpitData_WithTempTable_eshopx]", strDate, endDate, plantid, cellId, "cellwise", ddlSortOrder.SelectedValue, DataBaseAccess.getMachineIDWithSeparatorForScreen(cellId, "IconicCockpitLive"));
                            if (gvCellTableData.Columns.Count == 1)
                            {
                                BindTableData("Cell", dt);
                            }
                        }
                        //else
                        //{
                        //    dt = CockpitDataBaseAccess.GetTabelCockpitDetails("[s_GetCockpitData_eshopx]", strDate, endDate, plantid, cellId, "cellwise", ddlSortOrder.SelectedValue);
                        //}
                    }
                    Session["TableViewData"] = dt;
                    Session["SortnumberMachineId"] = 0;
                    Session["SortnumberbyOverallefficiency"] = 0;
                    Session["SortnumberbyProductionEfficiency"] = 0;
                    Session["SortnumberbyAvailabilityEfficiency"] = 0;
                    Session["SortnumberbyPrevOEE"] = 0;
                    gvCellTableData.DataSource = dt;
                    gvCellTableData.DataBind();
                    gvPlantTableData.Visible = false;
                    gvCellTableData.Visible = true;
                    gridviewTableData.Visible = false;
                    clearGridData(gvPlantTableData, 1);
                    clearGridData(gridviewTableData, 2);
                }
            }
            catch (Exception ex)
            {
                //rrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void BindMachineItem(string plantid, string cellId, string sortOrder)
        {
            try
            {
                setFilterValueToSession();
                hdnView.Value = ddlView.SelectedValue;
                string strDate = string.Empty, shift = string.Empty, endDate = string.Empty;
                DateTime FromDate = DateTime.Now.Date;
                DateTime ToDate = DateTime.Now.Date;
                FromDate = Util.GetDateTime(txtFromDate.Text.Trim());
                ToDate = Util.GetDateTime(txtToDate.Text.Trim());
                Session["FromDateAgg"] = FromDate.ToString("yyyy-MM-dd HH:mm:ss");
                Session["ToDateAgg"] = ToDate.ToString("yyyy-MM-dd HH:mm:ss");
                if (DateTime.Compare(FromDate, ToDate) > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('From date cannot be greater than To date.')", true);
                    //FromDate = DateTime.Now;
                    //ToDate = DateTime.Now.AddDays(1);
                    //txtFromDate.Text = FromDate.ToString("dd-MM-yyyy hh:mm");
                    //txtToDate.Text = ToDate.ToString("dd-MM-yyyy hh:mm");
                }
                int interval = CockpitDataBaseAccess.getLiveCockpitDateInterval();
                if ((ToDate - FromDate).TotalDays > interval)
                {
                    lblMessages.Text = "Difference between to date and from date cannot be more than " + interval + " days.<br> Please visit Historical Analytics -> Table View to see more than " + interval + " days of data.";
                    return;
                }
                else
                {

                    strDate = FromDate.ToSQLDateTimeFormat();
                    endDate = ToDate.ToSQLDateTimeFormat();
                    //  string selectedPlant = ddlPlantId.SelectedValue.ToString();
                    // string cellId = ddlCellID.SelectedValue.ToString();
                    if (plantid == "All")
                        plantid = "";
                    if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase))
                        cellId = "";
                    DataTable dt = null;
                    //dt = CachedData.getChachedData(strDate, endDate);
                    if (dt == null)
                    {
                        //if (CheckDateRange())
                        //{
                        dt = CockpitDataBaseAccess.GetTabelCockpitDetails("[s_GetCockpitData_WithTempTable_eshopx]", strDate, endDate, plantid, cellId, "Machinewise", sortOrder, DataBaseAccess.getMachineIDWithSeparatorForScreen(cellId, "IconicCockpitLive"));
                        if (gridviewTableData.Columns.Count == 3)
                        {
                            BindTableData("Machine", dt);
                        }
                        //}
                        //else
                        //{
                        //    dt = CockpitDataBaseAccess.GetTabelCockpitDetails("[s_GetCockpitData_eshopx]", strDate, endDate, plantid, cellId, "Machinewise", sortOrder);
                        //}
                    }
                    DataTable dtIncremented = new DataTable();

                    DataColumn SlNo = new DataColumn();
                    SlNo.ColumnName = "SlNo";
                    SlNo.AutoIncrement = true;
                    SlNo.AutoIncrementSeed = 1;
                    SlNo.AutoIncrementStep = 1;
                    SlNo.DataType = typeof(Int32);

                    dtIncremented.Columns.Add(SlNo);
                    DataTableReader dtReader = new DataTableReader(dt);
                    dtIncremented.BeginLoadData();
                    dtIncremented.Load(dtReader);
                    dtIncremented.EndLoadData();

                    Session["TableViewData"] = dt;
                    Session["SortnumberMachineId"] = 0;
                    Session["SortnumberbyOverallefficiency"] = 0;
                    Session["SortnumberbyProductionEfficiency"] = 0;
                    Session["SortnumberbyAvailabilityEfficiency"] = 0;
                    Session["SortnumberbyPrevOEE"] = 0;
                    gridviewTableData.DataSource = dtIncremented;
                    gridviewTableData.DataBind();
                    gvPlantTableData.Visible = false;
                    gvCellTableData.Visible = false;
                    gridviewTableData.Visible = true;
                    clearGridData(gvPlantTableData, 1);
                    clearGridData(gvCellTableData, 1);
                }
            }
            catch (Exception ex)
            {
                //rrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void clearGridData(GridView gv, int col)
        {
            //for (int i = gv.Columns.Count - 1; i >= col; i--)
            //{
            //    gv.Columns.RemoveAt(i);
            //}
            //int j = gv.Columns.Count;
        }
        protected void timerDataChange_Tick(object sender, EventArgs e)
        {
            try
            {
                //BindListItem();
                bindCurrentData();
            }
            catch (Exception ex)
            {
                //rrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnTrigger_Click(object sender, EventArgs e)
        {
            timerDataChange.Interval = Convert.ToInt32(DataBaseAccess.AutoRefreshData);
            if (chkAutoBox.Checked)
                timerDataChange.Enabled = true;
            else
                timerDataChange.Enabled = false;

        }

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellId(ddlPlantId.SelectedItem == null ? "" : ddlPlantId.SelectedItem.Text);
        }

        protected void gridviewTableData_Sorting(object sender, GridViewSortEventArgs e)
        {
            string name = e.SortExpression;
            DataTable dt = Session["TableViewData"] as DataTable;
            int count = 0;
            DataTable sortedDT = new DataTable();
            switch (name)
            {
                case "MachineID":
                    {
                        int.TryParse(Session["SortnumberMachineId"].ToString(), out count);
                        if (count == 0)
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "MachineId desc";
                            sortedDT = dv.ToTable();
                            count++;
                            Session["SortnumberMachineId"] = count;
                        }
                        else
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "MachineId asc";
                            sortedDT = dv.ToTable();
                            count--;
                            Session["SortnumberMachineId"] = count;
                        }
                        Session["SortnumberbyOverallefficiency"] = 0;
                        Session["SortnumberbyProductionEfficiency"] = 0;
                        Session["SortnumberbyAvailabilityEfficiency"] = 0;
                        break;
                    }
                case "Overallefficiency":
                    {
                        int.TryParse(Session["SortnumberbyOverallefficiency"].ToString(), out count);
                        if (count == 0)
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "Overallefficiency desc";
                            sortedDT = dv.ToTable();
                            count++;
                            Session["SortnumberbyOverallefficiency"] = count;
                        }
                        else
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "Overallefficiency asc";
                            sortedDT = dv.ToTable();
                            count--;
                            Session["SortnumberbyOverallefficiency"] = count;
                        }
                        Session["SortnumberMachineId"] = 0;
                        Session["SortnumberbyProductionEfficiency"] = 0;
                        Session["SortnumberbyAvailabilityEfficiency"] = 0;
                        break;
                    }
                case "ProductionEfficiency":
                    {
                        int.TryParse(Session["SortnumberbyProductionEfficiency"].ToString(), out count);
                        if (count == 0)
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "ProductionEfficiency desc";
                            sortedDT = dv.ToTable();
                            count++;
                            Session["SortnumberbyProductionEfficiency"] = count;
                        }
                        else
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "ProductionEfficiency asc";
                            sortedDT = dv.ToTable();
                            count--;
                            Session["SortnumberbyProductionEfficiency"] = count;
                        }
                        Session["SortnumberMachineId"] = 0;
                        Session["SortnumberbyOverallefficiency"] = 0;
                        Session["SortnumberbyAvailabilityEfficiency"] = 0;
                        break;
                    }
                case "AvailabilityEfficiency":
                    {
                        int.TryParse(Session["SortnumberbyAvailabilityEfficiency"].ToString(), out count);
                        if (count == 0)
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "AvailabilityEfficiency desc";
                            sortedDT = dv.ToTable();
                            count++;
                            Session["SortnumberbyAvailabilityEfficiency"] = count;
                        }
                        else
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "AvailabilityEfficiency asc";
                            sortedDT = dv.ToTable();
                            count--;
                            Session["SortnumberbyAvailabilityEfficiency"] = count;
                        }
                        Session["SortnumberMachineId"] = 0;
                        Session["SortnumberbyProductionEfficiency"] = 0;
                        Session["SortnumberbyOverallefficiency"] = 0;
                        break;
                    }
            }

            DataTable dtIncremented = new DataTable();

            DataColumn SlNo = new DataColumn();
            SlNo.ColumnName = "SlNo";
            SlNo.AutoIncrement = true;
            SlNo.AutoIncrementSeed = 1;
            SlNo.AutoIncrementStep = 1;
            SlNo.DataType = typeof(Int32);
            dtIncremented.Columns.Add(SlNo);
            DataTableReader dtReader = new DataTableReader(sortedDT);
            dtIncremented.BeginLoadData();
            dtIncremented.Load(dtReader);
            dtIncremented.EndLoadData();
            //dt.Columns.Add("SlNo", typeof(Int32));

            gridviewTableData.DataSource = dtIncremented;
            gridviewTableData.DataBind();

        }

        #region "Refresh Machine Status"
        [WebMethod]
        public static List<MachineStatusData> GetMachineStatusData(string plantId)
        {
            if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(plantId))
                plantId = "";
            return DataBaseAccess.GetMachineStatusData(plantId);
        }
        #endregion
        private void bindCurrentData()
        {
            string view = "";
            string plantid = ddlPlantId.SelectedValue;
            string cellid = DataBaseAccess.getCellIDWithSeparator(lbCellID);
            view = ddlView.SelectedValue.ToString();
            //ddlView.SelectedValue = view;
            //ddlView_SelectedIndexChanged(null, null);
            if (view == "Plantwise")
            {
                BindPlantItem(plantid);
            }
            else if (view == "cellwise")
            {
                BindCellItem(plantid, cellid);
            }
            else
            {
                BindMachineItem(plantid, cellid, ddlSortOrder.SelectedValue);
            }
            //if (Session["LATableViewsStack"] != null)
            //{
            //    List<LVIonicViewStack> list = (List<LVIonicViewStack>)Session["LATableViewsStack"];
            //    if (list.Count > 0)
            //    {
            //        int i = 0;
            //        if (list.Count == 1)
            //        {
            //            i = 0;
            //        }
            //        else
            //        {
            //            i = 1;
            //        }
            //        string plantid = list[i].PlantId;
            //        string cellid = list[i].CellId;
            //        view = list[i].View;
            //        ddlView.SelectedValue = view;
            //        ddlView_SelectedIndexChanged(null, null);
            //        ddlSortOrder.SelectedValue = list[i].SortOrder;
            //        if (view == "Plantwise")
            //        {
            //            BindPlantItem(plantid);
            //        }
            //        else if (view == "cellwise")
            //        {
            //            BindCellItem(plantid, cellid);
            //        }
            //        else
            //        {
            //            BindMachineItem(plantid, cellid, ddlSortOrder.SelectedValue);
            //        }
            //    }
            //    else
            //    {
            //        string plantid = ddlPlantId.SelectedValue;
            //        string cellid = ddlCellID.SelectedValue;
            //        view = ddlView.SelectedValue.ToString();
            //        ddlView.SelectedValue = view;
            //        ddlView_SelectedIndexChanged(null, null);
            //        if (view == "Plantwise")
            //        {
            //            BindPlantItem(plantid);
            //        }
            //        else if (view == "cellwise")
            //        {
            //            BindCellItem(plantid, cellid);
            //        }
            //        else
            //        {
            //            BindMachineItem(plantid, cellid, ddlSortOrder.SelectedValue);
            //        }
            //    }
            //}
            //else
            //{
            //    string plantid = ddlPlantId.SelectedValue;
            //    string cellid = ddlCellID.SelectedValue;
            //    view = ddlView.SelectedValue.ToString();
            //    ddlView.SelectedValue = view;
            //    ddlView_SelectedIndexChanged(null, null);
            //    if (view == "Plantwise")
            //    {
            //        BindPlantItem(plantid);
            //    }
            //    else if (view == "cellwise")
            //    {
            //        BindCellItem(plantid, cellid);
            //    }
            //    else
            //    {
            //        BindMachineItem(plantid, cellid, ddlSortOrder.SelectedValue);
            //    }
            //}
        }
        protected void gvPlantTableData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int col = e.Row.Controls.Count;
                    int colIndex = GetColumnIndexByDBName(this.gvPlantTableData, "PEffy");
                    if (colIndex != -1)
                    {
                        double PEGreenVal = 0, PEval = 0, PERedVal = 0;
                        if (drv["PEffy"] != DBNull.Value)
                        {
                            PEval = Convert.ToDouble(drv["PEffy"]);
                        }
                        if (drv["PEGreen"] != DBNull.Value)
                        {
                            PEGreenVal = Convert.ToDouble(drv["PEGreen"]);
                        }
                        if (drv["PERed"] != DBNull.Value)
                        {
                            PERedVal = Convert.ToDouble(drv["PERed"]);
                        }

                        if (PEval <= PERedVal && PEval > 0)
                        {
                            e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));
                        }
                        else if (PEval >= PEGreenVal)
                        {
                            if (ConfigurationManager.AppSettings["DantalHydraulicsPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                            {
                                if (PEval > 100 && !string.IsNullOrEmpty(values.PEGreaterThanHundredBackColor))
                                {
                                    e.Row.Cells[colIndex].ToolTip = "Check Cycle Time";
                                    e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.PEGreaterThanHundredBackColor.Substring(1));
                                }
                                else
                                    e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));// ColorTranslator.FromHtml(values.GoodRunning);//Color.Green(GOOD);
                            }
                            else
                                e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));// ColorTranslator.FromHtml(values.GoodRunning);//Color.Green(GOOD);
                        }
                        else if (PEval > PERedVal && PEval < PEGreenVal)
                        {
                            e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//ColorTranslator.FromHtml(values.ModeratelyRunning);//Color.Yellow(MODERATE);;
                        }
                        //e.Row.Cells[colIndex].BackColor = GetColorFrom(name); //System.Drawing.Color.FromName(name);
                    }
                    int colIndex1 = GetColumnIndexByDBName(this.gvPlantTableData, "OEffy");
                    if (colIndex1 != -1)
                    {
                        e.Row.Cells[colIndex1].CssClass = "hypercol";
                        double OEval = 0, OEGreenVal = 0, OERedVal = 0;
                        if (drv["OEffy"] != DBNull.Value)
                        {
                            OEval = Convert.ToDouble(drv["OEffy"]);
                        }
                        if (drv["OEGreen"] != DBNull.Value)
                        {
                            OEGreenVal = Convert.ToDouble(drv["OEGreen"]);
                        }
                        if (drv["OERed"] != DBNull.Value)
                        {
                            OERedVal = Convert.ToDouble(drv["OERed"]);
                        }
                        if (OEval <= OERedVal && OEval > 0)
                        {
                            e.Row.Cells[colIndex1].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));//Color.Red(BAD);
                        }
                        else if (OEval >= OEGreenVal)
                        {
                            e.Row.Cells[colIndex1].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));//Color.Green(GOOD);
                        }
                        else if (OEval > OERedVal && OEval < OEGreenVal)
                        {
                            e.Row.Cells[colIndex1].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//Color.Yellow(MODERATE);
                        }
                    }
                    int colindex2 = GetColumnIndexByDBName(this.gvPlantTableData, "AEffy");
                    if (colindex2 != -1)
                    {
                        double AEval = 0, AEGreenVal = 0, AERedVal = 0;
                        if (drv["AEffy"] != DBNull.Value)
                        {
                            AEval = Convert.ToDouble(drv["AEffy"]);
                        }
                        if (drv["AEGreen"] != DBNull.Value)
                        {
                            AEGreenVal = Convert.ToDouble(drv["AEGreen"]);
                        }
                        if (drv["AERed"] != DBNull.Value)
                        {
                            AERedVal = Convert.ToDouble(drv["AERed"]);
                        }
                        if (AEval <= AERedVal && AEval > 0)
                        {
                            e.Row.Cells[colindex2].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));//Color.Red (BAD);
                        }
                        else if (AEval >= AEGreenVal)
                        {
                            e.Row.Cells[colindex2].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));//Color.Green(GOOD);
                        }
                        else if (AEval > AERedVal && AEval < AEGreenVal)
                        {
                            e.Row.Cells[colindex2].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//Color.Yellow(MODERATE);
                        }
                    }
                    int colindex3 = GetColumnIndexByDBName(this.gvPlantTableData, "QEffy");
                    if (colindex3 != -1)
                    {
                        double QEval = 0, QEGreenVal = 0, QERedVal = 0;
                        if (drv["QEffy"] != DBNull.Value)
                        {
                            QEval = Convert.ToDouble(drv["QEffy"]);
                        }
                        if (drv["QEGreen"] != DBNull.Value)
                        {
                            QEGreenVal = Convert.ToDouble(drv["QEGreen"]);
                        }
                        if (drv["QERed"] != DBNull.Value)
                        {
                            QERedVal = Convert.ToDouble(drv["QERed"]);
                        }
                        if (QEval <= QERedVal && QEval > 0)
                        {
                            e.Row.Cells[colindex3].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));//Color.Red (BAD);
                        }
                        else if (QEval >= QEGreenVal)
                        {
                            e.Row.Cells[colindex3].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));//Color.Green(GOOD);
                        }
                        else if (QEval > QERedVal && QEval < QEGreenVal)
                        {
                            e.Row.Cells[colindex3].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//Color.Yellow(MODERATE);
                        }
                    }
                    int colindex4 = GetColumnIndexByDBName(this.gvPlantTableData, "OPREffy");
                    if (colindex4 != -1)
                    {
                        double OPRval = 0, OPRGreenVal = 0, OPRRedVal = 0;
                        if (drv["OPREffy"] != DBNull.Value)
                        {
                            OPRval = Convert.ToDouble(drv["OPREffy"]);
                        }
                        if (drv["OPRGreen"] != DBNull.Value)
                        {
                            OPRGreenVal = Convert.ToDouble(drv["OPRGreen"]);
                        }
                        if (drv["OPRRed"] != DBNull.Value)
                        {
                            OPRRedVal = Convert.ToDouble(drv["OPRRed"]);
                        }
                        if (OPRval <= OPRRedVal && OPRval > 0)
                        {
                            e.Row.Cells[colindex4].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));//Color.Red (BAD);
                        }
                        else if (OPRval >= OPRGreenVal)
                        {
                            e.Row.Cells[colindex4].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));//Color.Green(GOOD);
                        }
                        else if (OPRval > OPRRedVal && OPRval < OPRGreenVal)
                        {
                            e.Row.Cells[colindex4].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//Color.Yellow(MODERATE);
                        }
                    }
                    int colindex5 = GetColumnIndexByDBName(this.gvPlantTableData, "OverallOPREffy");
                    if (colindex5 != -1)
                    {
                        double OEval = 0, OEGreenVal = 0, OERedVal = 0;
                        if (drv["OverallOPREffy"] != DBNull.Value)
                        {
                            OEval = Convert.ToDouble(drv["OverallOPREffy"]);
                        }
                        if (drv["OEGreen"] != DBNull.Value)
                        {
                            OEGreenVal = Convert.ToDouble(drv["OEGreen"]);
                        }
                        if (drv["OERed"] != DBNull.Value)
                        {
                            OERedVal = Convert.ToDouble(drv["OERed"]);
                        }
                        if (OEval <= OERedVal && OEval > 0)
                        {
                            e.Row.Cells[colindex5].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));//Color.Red(BAD);
                        }
                        else if (OEval >= OEGreenVal)
                        {
                            e.Row.Cells[colindex5].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));//Color.Green(GOOD);
                        }
                        else if (OEval > OERedVal && OEval < OEGreenVal)
                        {
                            e.Row.Cells[colindex5].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//Color.Yellow(MODERATE);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //rrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void gvPlantTableData_Sorting(object sender, GridViewSortEventArgs e)
        {
            string name = e.SortExpression;
            DataTable dt = Session["TableViewData"] as DataTable;
            int count = 0;

            switch (name)
            {
                case "PlantID":
                    {
                        int.TryParse(Session["SortnumberMachineId"].ToString(), out count);
                        if (count == 0)
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "PlantID desc";
                            DataTable sortedDT = dv.ToTable();
                            count++;
                            Session["SortnumberMachineId"] = count;
                        }
                        else
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "PlantID asc";
                            DataTable sortedDT = dv.ToTable();
                            count--;
                            Session["SortnumberMachineId"] = count;
                        }
                        Session["SortnumberbyOverallefficiency"] = 0;
                        Session["SortnumberbyProductionEfficiency"] = 0;
                        Session["SortnumberbyAvailabilityEfficiency"] = 0;
                        break;
                    }
                case "OEffy":
                    {
                        int.TryParse(Session["SortnumberbyOverallefficiency"].ToString(), out count);
                        if (count == 0)
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "OEffy desc";
                            DataTable sortedDT = dv.ToTable();
                            count++;
                            Session["SortnumberbyOverallefficiency"] = count;
                        }
                        else
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "OEffy asc";
                            DataTable sortedDT = dv.ToTable();
                            count--;
                            Session["SortnumberbyOverallefficiency"] = count;
                        }
                        Session["SortnumberMachineId"] = 0;
                        Session["SortnumberbyProductionEfficiency"] = 0;
                        Session["SortnumberbyAvailabilityEfficiency"] = 0;
                        break;
                    }
                case "PEffy":
                    {
                        int.TryParse(Session["SortnumberbyProductionEfficiency"].ToString(), out count);
                        if (count == 0)
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "PEffy desc";
                            DataTable sortedDT = dv.ToTable();
                            count++;
                            Session["SortnumberbyProductionEfficiency"] = count;
                        }
                        else
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "PEffy asc";
                            DataTable sortedDT = dv.ToTable();
                            count--;
                            Session["SortnumberbyProductionEfficiency"] = count;
                        }
                        Session["SortnumberMachineId"] = 0;
                        Session["SortnumberbyOverallefficiency"] = 0;
                        Session["SortnumberbyAvailabilityEfficiency"] = 0;
                        break;
                    }
                case "AEffy":
                    {
                        int.TryParse(Session["SortnumberbyAvailabilityEfficiency"].ToString(), out count);
                        if (count == 0)
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "AEffy desc";
                            DataTable sortedDT = dv.ToTable();
                            count++;
                            Session["SortnumberbyAvailabilityEfficiency"] = count;
                        }
                        else
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "AEffy asc";
                            DataTable sortedDT = dv.ToTable();
                            count--;
                            Session["SortnumberbyAvailabilityEfficiency"] = count;
                        }
                        Session["SortnumberMachineId"] = 0;
                        Session["SortnumberbyProductionEfficiency"] = 0;
                        Session["SortnumberbyOverallefficiency"] = 0;
                        break;
                    }
            }
            gvPlantTableData.DataSource = dt;
            gvPlantTableData.DataBind();
        }

        protected void gvPlantTableData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Session["LATableViewsStack"] = null;
            List<LVIonicViewStack> list = new List<LVIonicViewStack>();
            LVIonicViewStack data = new LVIonicViewStack();
            data.PlantId = ddlPlantId.SelectedValue;
            data.CellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
            data.Order = 1;
            data.View = "cellwise";
            data.ViewBeforeClick = "Plantwise";
            data.SortOrder = ddlSortOrder.SelectedValue;
            ddlView.SelectedValue = data.View;
            ddlView_SelectedIndexChanged(null, null);
            list.Add(data);
            Session["LATableViewsStack"] = list;
            string plantid = (gvPlantTableData.Rows[e.RowIndex].FindControl("hfPlantId") as HiddenField).Value;
            BindCellItem(plantid, DataBaseAccess.getCellIDWithSeparator(lbCellID));
            ddlPlantId.SelectedValue = plantid;
            setFilterValueToSession();
            lbBackButton.Visible = true;
            lbBackButton.Text = "BackToPlant";
            tdBackBtn.Visible = true;
        }

        protected void gvCellTableData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int colIndex = GetColumnIndexByDBName(this.gvCellTableData, "PEffy");
                    if (colIndex != -1)
                    {
                        double PEGreenVal = 0, PEval = 0, PERedVal = 0;
                        if (drv["PEffy"] != DBNull.Value)
                        {
                            PEval = Convert.ToDouble(drv["PEffy"]);
                        }
                        if (drv["PEGreen"] != DBNull.Value)
                        {
                            PEGreenVal = Convert.ToDouble(drv["PEGreen"]);
                        }
                        if (drv["PERed"] != DBNull.Value)
                        {
                            PERedVal = Convert.ToDouble(drv["PERed"]);
                        }

                        if (PEval <= PERedVal && PEval > 0)
                        {
                            e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));
                        }
                        else if (PEval >= PEGreenVal)
                        {
                            if (ConfigurationManager.AppSettings["DantalHydraulicsPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                            {
                                if (PEval > 100 && !string.IsNullOrEmpty(values.PEGreaterThanHundredBackColor))
                                {
                                    e.Row.Cells[colIndex].ToolTip = "Check Cycle Time";
                                    e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.PEGreaterThanHundredBackColor.Substring(1));
                                }
                                else
                                    e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));// ColorTranslator.FromHtml(values.GoodRunning);//Color.Green(GOOD);
                            }
                            else
                                e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));// ColorTranslator.FromHtml(values.GoodRunning);//Color.Green(GOOD);
                        }
                        else if (PEval > PERedVal && PEval < PEGreenVal)
                        {
                            e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//ColorTranslator.FromHtml(values.ModeratelyRunning);//Color.Yellow(MODERATE);;
                        }
                        //e.Row.Cells[colIndex].BackColor = GetColorFrom(name); //System.Drawing.Color.FromName(name);
                    }
                    int colIndex1 = GetColumnIndexByDBName(this.gvCellTableData, "OEffy");
                    if (colIndex1 != -1)
                    {
                        e.Row.Cells[colIndex1].CssClass = "hypercol";
                        double OEval = 0, OEGreenVal = 0, OERedVal = 0;
                        if (drv["OEffy"] != DBNull.Value)
                        {
                            OEval = Convert.ToDouble(drv["OEffy"]);
                        }
                        if (drv["OEGreen"] != DBNull.Value)
                        {
                            OEGreenVal = Convert.ToDouble(drv["OEGreen"]);
                        }
                        if (drv["OERed"] != DBNull.Value)
                        {
                            OERedVal = Convert.ToDouble(drv["OERed"]);
                        }
                        if (OEval <= OERedVal && OEval > 0)
                        {
                            e.Row.Cells[colIndex1].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));//Color.Red(BAD);
                        }
                        else if (OEval >= OEGreenVal)
                        {
                            e.Row.Cells[colIndex1].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));//Color.Green(GOOD);
                        }
                        else if (OEval > OERedVal && OEval < OEGreenVal)
                        {
                            e.Row.Cells[colIndex1].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//Color.Yellow(MODERATE);
                        }
                    }
                    int colindex2 = GetColumnIndexByDBName(this.gvCellTableData, "AEffy");
                    if (colindex2 != -1)
                    {
                        double AEval = 0, AEGreenVal = 0, AERedVal = 0;
                        if (drv["AEffy"] != DBNull.Value)
                        {
                            AEval = Convert.ToDouble(drv["AEffy"]);
                        }
                        if (drv["AEGreen"] != DBNull.Value)
                        {
                            AEGreenVal = Convert.ToDouble(drv["AEGreen"]);
                        }
                        if (drv["AERed"] != DBNull.Value)
                        {
                            AERedVal = Convert.ToDouble(drv["AERed"]);
                        }
                        if (AEval <= AERedVal && AEval > 0)
                        {
                            e.Row.Cells[colindex2].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));//Color.Red (BAD);
                        }
                        else if (AEval >= AEGreenVal)
                        {
                            e.Row.Cells[colindex2].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));//Color.Green(GOOD);
                        }
                        else if (AEval > AERedVal && AEval < AEGreenVal)
                        {
                            e.Row.Cells[colindex2].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//Color.Yellow(MODERATE);
                        }
                    }
                    int colindex3 = GetColumnIndexByDBName(this.gvCellTableData, "QEffy");
                    if (colindex3 != -1)
                    {
                        double QEval = 0, QEGreenVal = 0, QERedVal = 0;
                        if (drv["QEffy"] != DBNull.Value)
                        {
                            QEval = Convert.ToDouble(drv["QEffy"]);
                        }
                        if (drv["QEGreen"] != DBNull.Value)
                        {
                            QEGreenVal = Convert.ToDouble(drv["QEGreen"]);
                        }
                        if (drv["QERed"] != DBNull.Value)
                        {
                            QERedVal = Convert.ToDouble(drv["QERed"]);
                        }
                        if (QEval <= QERedVal && QEval > 0)
                        {
                            e.Row.Cells[colindex3].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));//Color.Red (BAD);
                        }
                        else if (QEval >= QEGreenVal)
                        {
                            e.Row.Cells[colindex3].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));//Color.Green(GOOD);
                        }
                        else if (QEval > QERedVal && QEval < QEGreenVal)
                        {
                            e.Row.Cells[colindex3].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//Color.Yellow(MODERATE);
                        }
                    }
                    int colindex4 = GetColumnIndexByDBName(this.gvCellTableData, "Offy");
                    if (colindex4 != -1)
                    {
                        double OPRval = 0, OPRGreenVal = 0, OPRRedVal = 0;
                        if (drv["Offy"] != DBNull.Value)
                        {
                            OPRval = Convert.ToDouble(drv["Offy"]);
                        }
                        if (drv["OPRGreen"] != DBNull.Value)
                        {
                            OPRGreenVal = Convert.ToDouble(drv["OPRGreen"]);
                        }
                        if (drv["OPRRed"] != DBNull.Value)
                        {
                            OPRRedVal = Convert.ToDouble(drv["OPRRed"]);
                        }
                        if (OPRval <= OPRRedVal && OPRval > 0)
                        {
                            e.Row.Cells[colindex4].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));//Color.Red (BAD);
                        }
                        else if (OPRval >= OPRGreenVal)
                        {
                            e.Row.Cells[colindex4].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));//Color.Green(GOOD);
                        }
                        else if (OPRval > OPRRedVal && OPRval < OPRGreenVal)
                        {
                            e.Row.Cells[colindex4].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//Color.Yellow(MODERATE);
                        }
                    }
                    int colindex5 = GetColumnIndexByDBName(this.gvCellTableData, "OOffy");
                    if (colindex5 != -1)
                    {
                        double OEval = 0, OEGreenVal = 0, OERedVal = 0;
                        if (drv["OOffy"] != DBNull.Value)
                        {
                            OEval = Convert.ToDouble(drv["OOffy"]);
                        }
                        if (drv["OEGreen"] != DBNull.Value)
                        {
                            OEGreenVal = Convert.ToDouble(drv["OEGreen"]);
                        }
                        if (drv["OERed"] != DBNull.Value)
                        {
                            OERedVal = Convert.ToDouble(drv["OERed"]);
                        }
                        if (OEval <= OERedVal && OEval > 0)
                        {
                            e.Row.Cells[colindex5].BackColor = ColorTranslator.FromHtml("#" + values.BadlyRunning.Substring(3));//Color.Red(BAD);
                        }
                        else if (OEval >= OEGreenVal)
                        {
                            e.Row.Cells[colindex5].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));//Color.Green(GOOD);
                        }
                        else if (OEval > OERedVal && OEval < OEGreenVal)
                        {
                            e.Row.Cells[colindex5].BackColor = ColorTranslator.FromHtml("#" + values.ModeratelyRunning.Substring(3));//Color.Yellow(MODERATE);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //rrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void gvCellTableData_Sorting(object sender, GridViewSortEventArgs e)
        {
            string name = e.SortExpression;
            DataTable dt = Session["TableViewData"] as DataTable;
            int count = 0;

            switch (name)
            {
                case "GroupID":
                    {
                        int.TryParse(Session["SortnumberMachineId"].ToString(), out count);
                        if (count == 0)
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "GroupID desc";
                            DataTable sortedDT = dv.ToTable();
                            count++;
                            Session["SortnumberMachineId"] = count;
                        }
                        else
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "GroupID asc";
                            DataTable sortedDT = dv.ToTable();
                            count--;
                            Session["SortnumberMachineId"] = count;
                        }
                        Session["SortnumberbyOverallefficiency"] = 0;
                        Session["SortnumberbyProductionEfficiency"] = 0;
                        Session["SortnumberbyAvailabilityEfficiency"] = 0;
                        break;
                    }
                case "OEffy":
                    {
                        int.TryParse(Session["SortnumberbyOverallefficiency"].ToString(), out count);
                        if (count == 0)
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "OEffy desc";
                            DataTable sortedDT = dv.ToTable();
                            count++;
                            Session["SortnumberbyOverallefficiency"] = count;
                        }
                        else
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "OEffy asc";
                            DataTable sortedDT = dv.ToTable();
                            count--;
                            Session["SortnumberbyOverallefficiency"] = count;
                        }
                        Session["SortnumberMachineId"] = 0;
                        Session["SortnumberbyProductionEfficiency"] = 0;
                        Session["SortnumberbyAvailabilityEfficiency"] = 0;
                        break;
                    }
                case "PEffy":
                    {
                        int.TryParse(Session["SortnumberbyProductionEfficiency"].ToString(), out count);
                        if (count == 0)
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "PEffy desc";
                            DataTable sortedDT = dv.ToTable();
                            count++;
                            Session["SortnumberbyProductionEfficiency"] = count;
                        }
                        else
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "PEffy asc";
                            DataTable sortedDT = dv.ToTable();
                            count--;
                            Session["SortnumberbyProductionEfficiency"] = count;
                        }
                        Session["SortnumberMachineId"] = 0;
                        Session["SortnumberbyOverallefficiency"] = 0;
                        Session["SortnumberbyAvailabilityEfficiency"] = 0;
                        break;
                    }
                case "AEffy":
                    {
                        int.TryParse(Session["SortnumberbyAvailabilityEfficiency"].ToString(), out count);
                        if (count == 0)
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "AEffy desc";
                            DataTable sortedDT = dv.ToTable();
                            count++;
                            Session["SortnumberbyAvailabilityEfficiency"] = count;
                        }
                        else
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "AEffy asc";
                            DataTable sortedDT = dv.ToTable();
                            count--;
                            Session["SortnumberbyAvailabilityEfficiency"] = count;
                        }
                        Session["SortnumberMachineId"] = 0;
                        Session["SortnumberbyProductionEfficiency"] = 0;
                        Session["SortnumberbyOverallefficiency"] = 0;
                        break;
                    }
            }
            gvCellTableData.DataSource = dt;
            gvCellTableData.DataBind();
        }

        protected void gvCellTableData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Session["LATableViewsStack"] != null)
            {
                List<LVIonicViewStack> list = (List<LVIonicViewStack>)Session["LATableViewsStack"];
                LVIonicViewStack data = new LVIonicViewStack();
                data.PlantId = ddlPlantId.SelectedValue;
                data.CellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                data.Order = 2;
                data.View = "Machinewise";
                data.ViewBeforeClick = "cellwise";
                data.SortOrder = ddlSortOrder.SelectedValue;
                ddlView.SelectedValue = data.View;
                ddlView_SelectedIndexChanged(null, null);
                list.Add(data);
                Session["LATableViewsStack"] = list;
            }
            else
            {
                List<LVIonicViewStack> list = new List<LVIonicViewStack>();
                LVIonicViewStack data = new LVIonicViewStack();
                data.PlantId = ddlPlantId.SelectedValue;
                data.CellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                data.Order = 1;
                data.View = "Machinewise";
                data.ViewBeforeClick = "cellwise";
                data.SortOrder = ddlSortOrder.SelectedValue;
                ddlView.SelectedValue = data.View;
                ddlView_SelectedIndexChanged(null, null);
                list.Add(data);
                Session["LATableViewsStack"] = list;
            }
            string plantid = (gvCellTableData.Rows[e.RowIndex].FindControl("hdnCellPlantID") as HiddenField).Value;
            string cellid = (gvCellTableData.Rows[e.RowIndex].FindControl("hfCellId") as HiddenField).Value;
            BindMachineItem(plantid, cellid, "");

            ddlPlantId.SelectedValue = plantid;
            DataBaseAccess.setCellIDListBox(lbCellID, cellid);
            setFilterValueToSession();
            lbBackButton.Visible = true;
            lbBackButton.Text = "BackToCell";
            tdBackBtn.Visible = true;
        }

        protected void lbBackButton_Click(object sender, EventArgs e)
        {
            string text = lbBackButton.Text;
            if (text == "BackToCell")
            {
                if (Session["LATableViewsStack"] != null)
                {
                    List<LVIonicViewStack> list = (List<LVIonicViewStack>)Session["LATableViewsStack"];
                    if (list.Count > 0)
                    {
                        int i = 0;
                        if (list.Count == 1)
                        {
                            i = 0;
                        }
                        else
                        {
                            i = 1;
                        }
                        ddlPlantId.SelectedValue = list[i].PlantId;
                        DataBaseAccess.setCellIDListBox(lbCellID, list[i].CellId);
                        ddlView.SelectedValue = list[i].ViewBeforeClick;
                        ddlView_SelectedIndexChanged(null, null);
                        ddlSortOrder.SelectedValue = list[i].SortOrder;
                        BindCellItem(ddlPlantId.SelectedValue, DataBaseAccess.getCellIDWithSeparator(lbCellID));
                        list.RemoveAll(x => x.Order == i + 1);
                        if (list.Count > 0)
                        {
                            Session["LATableViewsStack"] = list;
                            lbBackButton.Text = "BackToPlant";
                        }
                        else
                        {
                            Session["LATableViewsStack"] = null;
                            lbBackButton.Visible = false;
                            tdBackBtn.Visible = false;
                        }
                    }
                }
                else
                {
                    Session["LATableViewsStack"] = null;
                    lbBackButton.Visible = false;
                    tdBackBtn.Visible = false;
                }
            }
            else if (text == "BackToPlant")
            {
                if (Session["LATableViewsStack"] != null)
                {
                    List<LVIonicViewStack> list = (List<LVIonicViewStack>)Session["LATableViewsStack"];
                    if (list.Count > 0)
                    {
                        ddlPlantId.SelectedValue = list[0].PlantId;
                        DataBaseAccess.setCellIDListBox(lbCellID, list[0].CellId);
                        ddlView.SelectedValue = list[0].ViewBeforeClick;
                        ddlView_SelectedIndexChanged(null, null);
                        ddlSortOrder.SelectedValue = list[0].SortOrder;
                        BindPlantItem(ddlPlantId.SelectedValue);
                    }

                    Session["LATableViewsStack"] = null;
                }
            }

        }
        protected void ddlView_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<ListItem> list = new List<ListItem>();
            if (ddlView.SelectedValue == "Plantwise")
            {
                list.Add(new ListItem("Plant ID - Asc", "Plantid asc"));
                list.Add(new ListItem("Plant ID - Desc", "Plantid desc"));
                list.Add(new ListItem("Production Count - Asc", "Components asc"));
                list.Add(new ListItem("Production Count - Desc", "Components desc"));
                list.Add(new ListItem("Rejectoin Count - Asc", "RejCount asc"));
                list.Add(new ListItem("Rejectoin Count - Desc", "RejCount desc"));
                list.Add(new ListItem("AE - Asc", "AvailabilityEfficiency asc"));
                list.Add(new ListItem("AE - Desc", "AvailabilityEfficiency desc"));
                list.Add(new ListItem("PE - Asc", "ProductionEfficiency asc"));
                list.Add(new ListItem("PE - Desc", "ProductionEfficiency desc"));
                list.Add(new ListItem("QE - Asc", "QualityEfficiency asc"));
                list.Add(new ListItem("QE - Desc", "QualityEfficiency desc"));
                list.Add(new ListItem("OE - Asc", "OverAllEfficiency asc"));
                list.Add(new ListItem("OE - Desc", "OverAllEfficiency desc"));
            }
            else if (ddlView.SelectedValue == "cellwise")
            {
                list.Add(new ListItem("Group ID - Asc", "Groupid asc"));
                list.Add(new ListItem("Group ID - Desc", "Groupid desc"));
                list.Add(new ListItem("Production Count - Asc", "Components asc"));
                list.Add(new ListItem("Production Count - Desc", "Components desc"));
                list.Add(new ListItem("Rejectoin Count - Asc", "RejCount asc"));
                list.Add(new ListItem("Rejectoin Count - Desc", "RejCount desc"));
                list.Add(new ListItem("AE - Asc", "AvailabilityEfficiency asc"));
                list.Add(new ListItem("AE - Desc", "AvailabilityEfficiency desc"));
                list.Add(new ListItem("PE - Asc", "ProductionEfficiency asc"));
                list.Add(new ListItem("PE - Desc", "ProductionEfficiency desc"));
                list.Add(new ListItem("QE - Asc", "QualityEfficiency asc"));
                list.Add(new ListItem("QE - Desc", "QualityEfficiency desc"));
                list.Add(new ListItem("OE - Asc", "OverAllEfficiency asc"));
                list.Add(new ListItem("OE - Desc", "OverAllEfficiency desc"));
            }
            else if (ddlView.SelectedValue == "Machinewise")
            {
                list.Add(new ListItem("Machine ID - Asc", "MachineId asc"));
                list.Add(new ListItem("Machine ID - Desc", "MachineId desc"));
                list.Add(new ListItem("Components - Asc", "Components asc"));
                list.Add(new ListItem("Components - Desc", "Components desc"));
                list.Add(new ListItem("Rejectoin Count - Asc", "RejCount asc"));
                list.Add(new ListItem("Rejectoin Count - Desc", "RejCount desc"));
                list.Add(new ListItem("AE - Asc", "AvailabilityEfficiency asc"));
                list.Add(new ListItem("AE - Desc", "AvailabilityEfficiency desc"));
                list.Add(new ListItem("PE - Asc", "ProductionEfficiency asc"));
                list.Add(new ListItem("PE - Desc", "ProductionEfficiency desc"));
                list.Add(new ListItem("QE - Asc", "QualityEfficiency asc"));
                list.Add(new ListItem("QE - Desc", "QualityEfficiency desc"));
                list.Add(new ListItem("OEE - Asc", "OverAllEfficiency asc"));
                list.Add(new ListItem("OEE - Desc", "OverAllEfficiency desc"));
                list.Add(new ListItem("Utilised Time - Asc", "UtilisedTime asc"));
                list.Add(new ListItem("Utilised Time - Desc", "UtilisedTime desc"));
                list.Add(new ListItem("Custom Sort Order", "CustomSortorder"));

            }
            ddlSortOrder.DataSource = list;
            ddlSortOrder.DataTextField = "Text";
            ddlSortOrder.DataValueField = "Value";
            ddlSortOrder.DataBind();
        }
        protected void lnkSwitch_Click(object sender, EventArgs e)
        {
            string backButtonText = "";
            if (lbBackButton.Visible)
            {
                backButtonText = lbBackButton.Text;
            }
            Response.Redirect(string.Format("~/IonicView.aspx?setFilter={0}&backButtonText={1}", "1", backButtonText), false);
        }
    }
}