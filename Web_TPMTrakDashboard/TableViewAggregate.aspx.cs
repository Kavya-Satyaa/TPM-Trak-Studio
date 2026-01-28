using BusinessClassLibrary;
using Elmah;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class TableViewAggregate : System.Web.UI.Page
    {
        public static ICockpitStyle values = CockpitDataBaseAccess.GetCockpitBackColorValues();
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
                            if (Request.QueryString["setFilter"].ToString() == "1" && Session["HistoryIconicTableViewFilter"] != null)
                            {
                                setFilterFromSession = 1;
                                filterData = Session["HistoryIconicTableViewFilter"] as IconicTableFilterEntity;
                                backButtonText = Request.QueryString["backButtonText"].ToString();
                            }
                        }
                    }
                    BindDayShift();
                    //if (Session["FromDateAgg"] != null && Session["ToDateAgg"] != null)
                    //{
                    //	txtFromDate.Text = Convert.ToDateTime(Session["FromDateAgg"].ToString()).ToString("dd-MM-yyyy");
                    //	txtToDate.Text = Convert.ToDateTime(Session["ToDateAgg"].ToString()).ToString("dd-MM-yyyy");
                    //}
                    //else
                    //{
                    txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    //gettimings();
                    //}
                    //timerDataChange.Enabled = false;
                    BindPlantId();
                    if (setFilterFromSession == 1)
                    {
                        txtFromDate.Text = filterData.FromDate;
                        txtToDate.Text = filterData.ToDate;
                        HelperClassGeneric.setDropdownValue(ddlPlantId, filterData.PlantId);
                    }
                    BindCellId(ddlPlantId.SelectedItem == null ? "" : ddlPlantId.SelectedItem.Text);
                    //ddlDayShift_SelectedIndexChanged("frist", null);
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
                        List<IonicViewStack> stackList = (List<IonicViewStack>)Session["IonicViewsStack"];
                        Session["HistoryTableViewsStack"] = stackList;
                        lbBackButton.Text = backButtonText;
                        tdBackBtn.Visible = true;
                        lbBackButton.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
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
                }
            }
            else if (defaultShift.Equals("CurrentShift"))
            {
                var currShiftVals = DataBaseAccess.GetCurrentOrPreviousShiftVals("[s_GetCurrentShift]");
                if (currShiftVals != null)
                {
                    SetDateTimeToControl(currShiftVals);
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

        private void setDefaultView()        {
            string view = CockpitDataBaseAccess.getHistoricalAnalyticsDefaultView("HistoricalDefaultView", "Table");            if (view != "")            {                ddlView.SelectedValue = view;            }        }
        private void SetDateTimeToControl(List<string> shiftVals)
        {
            txtFromDate.Text = Convert.ToDateTime(shiftVals[0]).ToString("dd-MM-yyyy HH:mm");
            txtToDate.Text = Convert.ToDateTime(shiftVals[1]).ToString("dd-MM-yyyy HH:mm");
        }

        //private bool CheckDateRange()
        //{
        //	bool isDateInRange = false;
        //	try
        //	{
        //		DateTime dt1 = Util.GetDateTime(txtFromDate.Text);
        //		DateTime dt2 = Util.GetDateTime(txtToDate.Text);
        //		var hours = (dt2 - dt1).TotalHours;

        //		if (Math.Abs(hours) <= 168)
        //		{
        //			isDateInRange = true;
        //		}
        //	}
        //	catch (Exception ex)
        //	{
        //		ErrorSignal.FromCurrentContext().Raise(ex);
        //		lblMessages.Text = ex.ToString();
        //	}
        //	return isDateInRange;
        //}

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
                lbCellID.DataSource = lstCellId;
                lbCellID.DataBind();
                foreach (ListItem item in lbCellID.Items)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Day Shift Data"
        private void BindDayShift()
        {
            try
            {
                //List<string> lstPlantData = CockpitDataBaseAccess.GetAllPredefinedShifts();
                //ddlDayShift.DataSource = lstPlantData;
                //ddlDayShift.DataBind();
                //ddlDayShift.SelectedIndex = 3;
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Day Shift Wise change Data"
        protected void ddlDayShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //	string selectedShift = ddlDayShift.SelectedValue.ToString();
            //	string logicalDayStart = string.Empty, logicalDayEnd = string.Empty;
            //	if (selectedShift.Contains("Today"))
            //	{
            //		if (ddlDayShift.SelectedValue.Equals("Today - All"))
            //		{
            //			logicalDayStart = CockpitDataBaseAccess.GetLogicalDay(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //			txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("dd-MM-yyyy HH:mm");
            //			//dtpTimeFrom.Value = Convert.ToDateTime(logicalDayStart);

            //			logicalDayEnd = CockpitDataBaseAccess.GetLogicalDayEnd(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //			txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("dd-MM-yyyy HH:mm");
            //			//dtpTimeTo.Value = Convert.ToDateTime(logicalDayEnd);
            //		}

            //		else
            //		{
            //			int index = selectedShift.IndexOf('-');
            //			var shift = CockpitDataBaseAccess.GetCurrentShiftTime(selectedShift.Substring(index + 1).Trim());
            //			if (shift != null)
            //			{
            //				logicalDayStart = shift[0];
            //				logicalDayEnd = shift[1];

            //				txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("dd-MM-yyyy HH:mm");
            //				//dtpTimeFrom.Value = Convert.ToDateTime(logicalDayStart);

            //				txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("dd-MM-yyyy HH:mm");
            //				//dtpTimeTo.Value = Convert.ToDateTime(logicalDayEnd);
            //			}
            //		}
            //	}
            //	else
            //	{

            //		if (ddlDayShift.SelectedValue.Equals("Yesterday - All"))
            //		{
            //			logicalDayStart = CockpitDataBaseAccess.GetLogicalDay(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
            //			txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("dd-MM-yyyy HH:mm");
            //			//dtpTimeFrom.Value = Convert.ToDateTime(logicalDayStart);

            //			logicalDayEnd = CockpitDataBaseAccess.GetLogicalDayEnd(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
            //			txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("dd-MM-yyyy HH:mm");
            //			//dtpTimeTo.Value = Convert.ToDateTime(logicalDayEnd);
            //		}
            //		else
            //		{
            //			int index = selectedShift.IndexOf('-');
            //			var shift = CockpitDataBaseAccess.GetCurrentShiftTime(selectedShift.Substring(index + 1).Trim());
            //			if (shift != null)
            //			{
            //				logicalDayStart = shift[0];
            //				logicalDayEnd = shift[1];

            //				txtFromDate.Text = Convert.ToDateTime(logicalDayStart).AddDays(-1).ToString("dd-MM-yyyy HH:mm");
            //				//dtpTimeFrom.Value = Convert.ToDateTime(logicalDayStart);

            //				txtToDate.Text = Convert.ToDateTime(logicalDayEnd).AddDays(-1).ToString("dd-MM-yyyy HH:mm");
            //				//dtpTimeTo.Value = Convert.ToDateTime(logicalDayEnd);
            //			}
            //		}
            //	}
            //	if (!sender.ToString().Equals("frist", StringComparison.OrdinalIgnoreCase))
            //		BindListItem();
            //}
            //catch (Exception ex)
            //{
            //	ErrorSignal.FromCurrentContext().Raise(ex);
            //	lblMessages.Text = ex.ToString();
            //	Logger.WriteErrorLog(ex.ToString());
            //}
        }
        #endregion

        private void BindTableData(string type, DataTable dt)
        {
            try
            {
                List<string> dtColumnList = dt.Columns.Cast<DataColumn>().Select(k => k.ColumnName).ToList();
                if (type == "Machine")
                {
                    List<string> listOfColNames = new List<string>();
                    List<string> val = CockpitDataBaseAccess.GetOrderedLabels(out listOfColNames, "WebCockpitGridColumnAggregate", Session["Language"] == null ? "en" : Session["Language"].ToString());
                    for (int i = 0; i < listOfColNames.Count; i++)
                    {
                        if (dtColumnList.Contains(listOfColNames[i], StringComparer.OrdinalIgnoreCase))
                        {
                            BoundField boundfield = new BoundField();

                            boundfield.DataField = listOfColNames[i].ToString();
                            boundfield.HeaderText = val[i].ToString();
                            if (listOfColNames[i].Equals("OEffy", StringComparison.OrdinalIgnoreCase) || listOfColNames[i].Equals("PEffy", StringComparison.OrdinalIgnoreCase) ||
                                listOfColNames[i].Equals("AEffy", StringComparison.OrdinalIgnoreCase) || listOfColNames[i].Equals("QEffy", StringComparison.OrdinalIgnoreCase))
                            {
                                boundfield.DataFormatString = "{0:F0}";
                            }
                            //boundfield.HeaderStyle.Width = new Unit("100px");
                            gridviewTableData.Columns.Add(boundfield);
                        }
                    }
                    // BindListItem(ddlPlantId.SelectedValue.ToString(),ddlCellID.SelectedValue.ToString());
                }
                if (type == "Plant")
                {
                    List<string> listOfColNames = new List<string>();
                    List<string> val = CockpitDataBaseAccess.GetOrderedLabels(out listOfColNames, "WebCockpitGridColumnAggregate", Session["Language"] == null ? "en" : Session["Language"].ToString());
                    //var lisoColName=listOfColNames.Select(x => x.Replace("MachineDescription", "PlantDescription")).ToList();
                    //listOfColNames.Find("MachineDescription").Replace("MachineDescription", "PlantDescription");
                    //for (int i = 0; i < listOfColNames.Count; i++)
                    //{
                    //    if (listOfColNames[i] == "MachineDescription")
                    //    {
                    //        listOfColNames[i] = "PlantDescription";
                    //    }
                    //}
                    //for (int i = 0; i < val.Count; i++)
                    //{
                    //    if(val[i]== "Machine Description")
                    //    {
                    //        val[i] = "Plant Description";
                    //    }
                    //}
                    for (int i = 0; i < listOfColNames.Count; i++)
                    {
                        if (dtColumnList.Contains(listOfColNames[i], StringComparer.OrdinalIgnoreCase))
                        {
                            BoundField boundfield = new BoundField();

                            boundfield.DataField = listOfColNames[i].ToString();
                            boundfield.HeaderText = val[i].ToString();
                            if (listOfColNames[i].Equals("OEffy", StringComparison.OrdinalIgnoreCase) || listOfColNames[i].Equals("PEffy", StringComparison.OrdinalIgnoreCase) ||
                                listOfColNames[i].Equals("AEffy", StringComparison.OrdinalIgnoreCase) || listOfColNames[i].Equals("QEffy", StringComparison.OrdinalIgnoreCase))
                            {
                                boundfield.DataFormatString = "{0:F0}";
                            }
                            //boundfield.HeaderStyle.Width = new Unit("100px");
                            gvPlantTableData.Columns.Add(boundfield);
                        }
                    }
                    // BindPlantDetails();
                }
                if (type == "Cell")
                {
                    List<string> listOfColNames = new List<string>();
                    List<string> val = CockpitDataBaseAccess.GetOrderedLabels(out listOfColNames, "WebCockpitGridColumnAggregate", Session["Language"] == null ? "en" : Session["Language"].ToString());
                    //for (int i = 0; i < listOfColNames.Count; i++)
                    //{
                    //    if (listOfColNames[i] == "MachineDescription")
                    //    {
                    //        listOfColNames[i] = "GroupDescription";
                    //    }
                    //    //listOfColNames[i] = listOfColNames[i].Replace("MachineDescription", "GroupDescription");
                    //}
                    //for (int i = 0; i < val.Count; i++)
                    //{
                    //    if (val[i] == "Machine Description")
                    //    {
                    //        val[i] = "Group Description";
                    //    }
                    //    // val[i] = val[i].Replace("Machine Description", "Group Description");
                    //}
                    for (int i = 0; i < listOfColNames.Count; i++)
                    {
                        if (dtColumnList.Contains(listOfColNames[i], StringComparer.OrdinalIgnoreCase))
                        {
                            BoundField boundfield = new BoundField();

                            boundfield.DataField = listOfColNames[i].ToString();
                            boundfield.HeaderText = val[i].ToString();
                            if (listOfColNames[i].Equals("OEffy", StringComparison.OrdinalIgnoreCase) || listOfColNames[i].Equals("PEffy", StringComparison.OrdinalIgnoreCase) ||
                                listOfColNames[i].Equals("AEffy", StringComparison.OrdinalIgnoreCase) || listOfColNames[i].Equals("QEffy", StringComparison.OrdinalIgnoreCase))
                            {
                                boundfield.DataFormatString = "{0:F0}";
                            }
                            //boundfield.HeaderStyle.Width = new Unit("100px");
                            gvCellTableData.Columns.Add(boundfield);
                        }
                    }
                    // BindListItem(ddlPlantId.SelectedValue.ToString(), ddlCellID.SelectedValue.ToString());
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void BindListItem(string plantid, string cellid)
        {
            try
            {
                setFilterValueToSession();
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

                //if ((ToDate - FromDate).TotalDays > 30)
                //{
                //	lblMessages.Text = "Difference between to date and from date cannot be more than 31 days.";
                //	return;
                //}
                else
                {


                    strDate = FromDate.ToString("yyyy-MM-dd");
                    endDate = ToDate.ToString("yyyy-MM-dd");
                    if (plantid == "All")
                        plantid = "";
                    if (cellid.Equals("All", StringComparison.OrdinalIgnoreCase))
                        cellid = "";
                    DataTable dt = new DataTable();
                    dt = CachedData.getChachedData(strDate, endDate);
                    if (dt == null)
                    {
                        dt = CockpitDataBaseAccess.GetTabelCockpitDetailsAggregated("[s_GetShiftAgg_ProductionReport]", strDate, endDate, plantid, cellid, "Machinewise", ddlSortOrder.SelectedValue, DataBaseAccess.getMachineIDWithSeparatorForScreen(cellid, "TableCockpitAgg"));
                        if (gridviewTableData.Columns.Count == 1)
                        {
                            //for (int i = gridviewTableData.Columns.Count - 1; i >= 1; i--)
                            //{
                            //    gridviewTableData.Columns.RemoveAt(i);
                            //}
                            BindTableData("Machine", dt);
                        }
                    }
                    gridviewTableData.DataSource = dt;
                    gridviewTableData.DataBind();
                    gvPlantTableData.Visible = false;
                    gvCellTableData.Visible = false;
                    gridviewTableData.Visible = true;
                    //lbBackButton.Visible = true;
                    //lbBackButton.Text = "BackToCell";
                    //tdBackBtn.Visible = true;
                    //gridviewTableData.UseAccessibleHeader = true;
                    //if (dt != null && dt.Rows.Count > 0)
                    //    gridviewTableData.HeaderRow.TableSection = TableRowSection.TableHeader;

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void BindPlantDetails(string plantid)
        {
            try
            {
                setFilterValueToSession();
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
                else
                {


                    strDate = FromDate.ToString("yyyy-MM-dd");
                    endDate = ToDate.ToString("yyyy-MM-dd");
                    string cellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    if (plantid == "All")
                        plantid = "";
                    if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase))
                        cellId = "";
                    DataTable dt = new DataTable();
                    dt = CachedData.getChachedData(strDate, endDate);
                    if (dt == null)
                    {
                        dt = CockpitDataBaseAccess.GetTabelCockpitDetailsAggregated("[s_GetShiftAgg_ProductionReport]", strDate, endDate, plantid, cellId, "Plantwise", ddlSortOrder.SelectedValue, DataBaseAccess.getMachineIDWithSeparatorForScreen(cellId, "TableCockpitAgg"));
                        if (gvPlantTableData.Columns.Count == 1)
                        {
                            //for (int i = gvPlantTableData.Columns.Count - 1; i >= 1; i--)
                            //{
                            //    gvPlantTableData.Columns.RemoveAt(i);
                            //}
                            BindTableData("Plant", dt);
                        }
                    }
                    gvPlantTableData.DataSource = dt;
                    gvPlantTableData.DataBind();
                    gvPlantTableData.Visible = true;
                    gvCellTableData.Visible = false;
                    gridviewTableData.Visible = false;
                    lbBackButton.Visible = false;
                    tdBackBtn.Visible = false;

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void BindCellDetials(string plantid, string cellid)
        {
            try
            {
                setFilterValueToSession();
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
                else
                {

                    strDate = FromDate.ToString("yyyy-MM-dd");
                    endDate = ToDate.ToString("yyyy-MM-dd");
                    if (plantid == "All")
                        plantid = "";
                    if (cellid.Equals("All", StringComparison.OrdinalIgnoreCase))
                        cellid = "";
                    DataTable dt = new DataTable();
                    dt = CachedData.getChachedData(strDate, endDate);
                    if (dt == null)
                    {
                        dt = CockpitDataBaseAccess.GetTabelCockpitDetailsAggregated("[s_GetShiftAgg_ProductionReport]", strDate, endDate, plantid, cellid, "cellwise", ddlSortOrder.SelectedValue, DataBaseAccess.getMachineIDWithSeparatorForScreen(cellid, "TableCockpitAgg"));
                        if (gvCellTableData.Columns.Count == 1)
                        {
                            //for (int i = gvCellTableData.Columns.Count - 1; i >= 1; i--)
                            //{
                            //    gvCellTableData.Columns.RemoveAt(i);
                            //}
                            BindTableData("Cell", dt);
                        }
                    }
                    gvCellTableData.DataSource = dt;
                    gvCellTableData.DataBind();
                    gvPlantTableData.Visible = false;
                    gvCellTableData.Visible = true;
                    gridviewTableData.Visible = false;
                    //lbBackButton.Visible = true;
                    //lbBackButton.Text = "BackToPlant";
                    //tdBackBtn.Visible = true;
                    //gridviewTableData.UseAccessibleHeader = true;
                    //if (dt != null && dt.Rows.Count > 0)
                    //    gridviewTableData.HeaderRow.TableSection = TableRowSection.TableHeader;

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
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
                    int colIndex = GetColumnIndexByDBName(this.gridviewTableData, "PEffy");
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
                                if (Convert.ToDouble(PEval) > 100 && !string.IsNullOrEmpty(values.PEGreaterThanHundredBackColor))
                                {
                                    e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.PEGreaterThanHundredBackColor.Substring(1));
                                    e.Row.Cells[colIndex].ToolTip = "Check Cycle Time";
                                }
                                else
                                    e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));////"Green"; 
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
                    int colIndex1 = GetColumnIndexByDBName(this.gridviewTableData, "OEffy");
                    if (colIndex1 != -1)
                    {
                        //e.Row.Cells[colIndex1].CssClass = "hypercol";
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
                    int colindex2 = GetColumnIndexByDBName(this.gridviewTableData, "AEffy");
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
                    int colindex3 = GetColumnIndexByDBName(this.gridviewTableData, "QEffy");
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
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
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
                ErrorSignal.FromCurrentContext().Raise(ex);
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
                ErrorSignal.FromCurrentContext().Raise(ex);
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
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
        }

        protected void btnProcess_Click1(object sender, EventArgs e)
        {
            tdBackBtn.Visible = false;            Session["HistoryTableViewsStack"] = null;
            if (ddlView.SelectedValue == "Plantwise")
            {
                BindPlantDetails(ddlPlantId.SelectedValue == null ? "" : ddlPlantId.SelectedValue.ToString());
            }
            else if (ddlView.SelectedValue == "cellwise")
            {
                BindCellDetials(ddlPlantId.SelectedValue == null ? "" : ddlPlantId.SelectedValue.ToString(), DataBaseAccess.getCellIDWithSeparator(lbCellID));
                //hfPlantIdForBack.Value = ddlPlantId.SelectedValue;                //hdnCellPlantIDForBack.Value = ddlPlantId.SelectedValue;                //hdnCellIDForBack.Value = ddlCellID.SelectedValue;

            }
            else if (ddlView.SelectedValue == "Machinewise")
            {
                BindListItem(ddlPlantId.SelectedValue == null ? "" : ddlPlantId.SelectedValue.ToString(), DataBaseAccess.getCellIDWithSeparator(lbCellID));
                //hfPlantIdForBack.Value = ddlPlantId.SelectedValue;                //hdnCellPlantIDForBack.Value = ddlPlantId.SelectedValue;                //hdnCellIDForBack.Value = ddlCellID.SelectedValue;
            }

        }
        private void setFilterValueToSession()
        {
            IconicTableFilterEntity filterData = new IconicTableFilterEntity();
            filterData.FromDate = txtFromDate.Text;
            filterData.ToDate = txtToDate.Text;
            filterData.PlantId = ddlPlantId.SelectedValue;
            filterData.CellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
            filterData.View = ddlView.SelectedValue;
            filterData.SortOrder = ddlSortOrder.SelectedValue;
            Session["HistoryIconicTableViewFilter"] = filterData;
        }
        protected void timerDataChange_Tick(object sender, EventArgs e)
        {
            try
            {
                //	BindListItem();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnTrigger_Click(object sender, EventArgs e)
        {
            //timerDataChange.Interval = Convert.ToInt32(DataBaseAccess.AutoRefreshData);
            //if (chkAutoBox.Checked)
            //	timerDataChange.Enabled = true;
            //else
            //	timerDataChange.Enabled = false;

        }

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellId(ddlPlantId.SelectedItem == null ? "" : ddlPlantId.SelectedItem.Text);
        }

        protected void gvPlantTableData_RowDataBound(object sender, GridViewRowEventArgs e)
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
                                if (Convert.ToDouble(PEval) > 100 && !string.IsNullOrEmpty(values.PEGreaterThanHundredBackColor))
                                {
                                    e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.PEGreaterThanHundredBackColor.Substring(1));
                                    e.Row.Cells[colIndex].ToolTip = "Check Cycle Time";
                                }
                                else
                                    e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));////"Green"; 
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
                        //e.Row.Cells[colIndex1].CssClass = "hypercol";
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
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        protected void gvCellTableData_RowDataBound(object sender, GridViewRowEventArgs e)
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
                                if (Convert.ToDouble(PEval) > 100 && !string.IsNullOrEmpty(values.PEGreaterThanHundredBackColor))
                                {
                                    e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.PEGreaterThanHundredBackColor.Substring(1));
                                    e.Row.Cells[colIndex].ToolTip = "Check Cycle Time";
                                }
                                else
                                    e.Row.Cells[colIndex].BackColor = ColorTranslator.FromHtml("#" + values.GoodRunning.Substring(3));////"Green"; 
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
                        //e.Row.Cells[colIndex1].CssClass = "hypercol";
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
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        protected void lbBackButton_Click(object sender, EventArgs e)
        {
            //string text = lbBackButton.Text;
            //if (text == "BackToCell")
            //{
            //    BindCellDetials(hdnCellPlantIDForBack.Value, hdnCellIDForBack.Value);

            //}
            //else if (text == "BackToPlant")
            //{
            //    BindPlantDetails(hfPlantIdForBack.Value);
            //}
            string text = lbBackButton.Text;
            if (text == "BackToCell")
            {
                if (Session["HistoryTableViewsStack"] != null)
                {
                    List<IonicViewStack> list = (List<IonicViewStack>)Session["HistoryTableViewsStack"];
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
                        BindCellDetials(ddlPlantId.SelectedValue, DataBaseAccess.getCellIDWithSeparator(lbCellID));
                        list.RemoveAll(x => x.Order == i + 1);
                        if (list.Count > 0)
                        {
                            Session["HistoryTableViewsStack"] = list;
                            lbBackButton.Text = "BackToPlant";
                        }
                        else
                        {
                            Session["HistoryTableViewsStack"] = null;
                            lbBackButton.Visible = false;
                            tdBackBtn.Visible = false;
                        }
                    }
                }

            }
            else if (text == "BackToPlant")
            {
                if (Session["HistoryTableViewsStack"] != null)
                {
                    List<IonicViewStack> list = (List<IonicViewStack>)Session["HistoryTableViewsStack"];
                    if (list.Count > 0)
                    {
                        ddlPlantId.SelectedValue = list[0].PlantId;
                        DataBaseAccess.setCellIDListBox(lbCellID, list[0].CellId);
                        ddlView.SelectedValue = list[0].ViewBeforeClick;
                        ddlView_SelectedIndexChanged(null, null);
                        ddlSortOrder.SelectedValue = list[0].SortOrder;
                        BindPlantDetails(ddlPlantId.SelectedValue);
                    }

                    Session["HistoryTableViewsStack"] = null;
                }
            }
        }

        protected void gvPlantTableData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Session["HistoryTableViewsStack"] = null;
            List<IonicViewStack> list = new List<IonicViewStack>();
            IonicViewStack data = new IonicViewStack();
            data.PlantId = ddlPlantId.SelectedValue;
            data.CellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
            data.Order = 1;
            data.ViewBeforeClick = "Plantwise";
            data.SortOrder = ddlSortOrder.SelectedValue;
            ddlView.SelectedValue = "cellwise";
            ddlView_SelectedIndexChanged(null, null);
            list.Add(data);
            Session["HistoryTableViewsStack"] = list;
            string plantid = (gvPlantTableData.Rows[e.RowIndex].FindControl("hfPlantId") as HiddenField).Value;
            BindCellDetials(plantid, DataBaseAccess.getCellIDWithSeparator(lbCellID));
            ddlPlantId.SelectedValue = plantid;
            setFilterValueToSession();
            lbBackButton.Visible = true;
            lbBackButton.Text = "BackToPlant";
            tdBackBtn.Visible = true;
        }
        protected void gvCellTableData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            if (Session["HistoryTableViewsStack"] != null)
            {
                List<IonicViewStack> list = (List<IonicViewStack>)Session["HistoryTableViewsStack"];
                IonicViewStack data = new IonicViewStack();
                data.PlantId = ddlPlantId.SelectedValue;
                data.CellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                data.Order = 2;
                data.ViewBeforeClick = "cellwise";
                data.SortOrder = ddlSortOrder.SelectedValue;
                ddlView.SelectedValue = "Machinewise";
                ddlView_SelectedIndexChanged(null, null);
                list.Add(data);
                Session["HistoryTableViewsStack"] = list;
            }
            else
            {
                List<IonicViewStack> list = new List<IonicViewStack>();
                IonicViewStack data = new IonicViewStack();
                data.PlantId = ddlPlantId.SelectedValue;
                data.CellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                data.Order = 1;
                data.ViewBeforeClick = "cellwise";
                data.SortOrder = ddlSortOrder.SelectedValue;
                ddlView.SelectedValue = "Machinewise";
                ddlView_SelectedIndexChanged(null, null);
                list.Add(data);
                Session["HistoryTableViewsStack"] = list;
            }
            string plantid = (gvCellTableData.Rows[e.RowIndex].FindControl("hdnCellPlantID") as HiddenField).Value;
            string cellid = (gvCellTableData.Rows[e.RowIndex].FindControl("hfCellId") as HiddenField).Value;
            BindListItem(plantid, cellid);

            ddlPlantId.SelectedValue = plantid;
            DataBaseAccess.setCellIDListBox(lbCellID, cellid);
            setFilterValueToSession();
            lbBackButton.Visible = true;
            lbBackButton.Text = "BackToCell";
            tdBackBtn.Visible = true;

        }
        protected void ddlView_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<ListItem> list = new List<ListItem>();
            if (ddlView.SelectedValue == "Plantwise")
            {
                list.Add(new ListItem("Plant ID - Asc", "Plantid asc"));
                list.Add(new ListItem("Plant ID - Desc", "Plantid desc"));
            }
            else if (ddlView.SelectedValue == "cellwise")
            {
                list.Add(new ListItem("Group ID - Asc", "Groupid asc"));
                list.Add(new ListItem("Group ID - Desc", "Groupid desc"));
            }
            else if (ddlView.SelectedValue == "Machinewise")
            {
                list.Add(new ListItem("Machine ID - Asc", "MachineID asc"));
                list.Add(new ListItem("Machine ID - Desc", "MachineID desc"));

            }
            list.Add(new ListItem("Production Count - Asc", "ProdCount asc"));
            list.Add(new ListItem("Production Count - Desc", "ProdCount desc"));
            list.Add(new ListItem("Rejectoin Count - Asc", "RejCount asc"));
            list.Add(new ListItem("Rejectoin Count - Desc", "RejCount desc"));
            list.Add(new ListItem("AE - Asc", "AEffy asc"));
            list.Add(new ListItem("AE - Desc", "AEffy desc"));
            list.Add(new ListItem("PE - Asc", "PEffy asc"));
            list.Add(new ListItem("PE- Desc", "PEffy desc"));
            list.Add(new ListItem("QE - Asc", "QEffy asc"));
            list.Add(new ListItem("QE - Desc", "QEffy desc"));
            list.Add(new ListItem("OE - Asc", "OEffy asc"));
            list.Add(new ListItem("OE - Desc", "OEffy desc"));

            if (ddlView.SelectedValue == "Machinewise")
            {
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
            Response.Redirect(string.Format("~/IonicViewAggregated.aspx?setFilter={0}&backButtonText={1}", "1", backButtonText), false);
        }
    }
}