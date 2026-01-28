using BusinessClassLibrary;
using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class IonicViewAggregated : System.Web.UI.Page
    {
        public static AppUISettings settings = null;
        List<string> listOfColNames = new List<string>();
        string defaultShift = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Language"] == null || Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            settings = DataBaseAccess.ViewAppUISettings();
            if (!IsPostBack)
            {
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
                //if (Session["FromDateAgg"] != null && Session["ToDateAgg"] != null)
                //{
                //    txtFromDate.Text = Convert.ToDateTime(Session["FromDateAgg"].ToString()).ToString("dd-MM-yyyy");
                //    txtToDate.Text = Convert.ToDateTime(Session["ToDateAgg"].ToString()).ToString("dd-MM-yyyy");
                //}
                //else
                //{
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                //gettimings();
                //}
                //BindDayShift();
                BindPlantId();
                if (setFilterFromSession == 1)
                {
                    txtFromDate.Text = filterData.FromDate;
                    txtToDate.Text = filterData.ToDate;
                    HelperClassGeneric.setDropdownValue(ddlPlantId, filterData.PlantId);
                }
                BindCellId(ddlPlantId.SelectedItem == null ? "" : ddlPlantId.SelectedItem.Text);
                //txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy HH:mm tt");
                //txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm tt");               

                //ddlDayShift_SelectedIndexChanged("frist", null);
                //BindData();
                //BindPlantData();
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
                btnProcess_Click(null, null);
                if (backButtonText != "")
                {
                    Session["IonicViewsStack"] = Session["HistoryTableViewsStack"] as List<IonicViewStack>;
                    lbBackButton.Text = backButtonText;
                    tdlbBackButton.Visible = true;
                    lbBackButton.Visible = true;
                }
                //timerDataChange.Enabled = false;
            }
        }
        private void setDefaultView()
        {
           string view= CockpitDataBaseAccess.getHistoricalAnalyticsDefaultView("HistoricalDefaultView", "Ionic");
            if (view != "")
            {
                ddlView.SelectedValue = view;
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

        private void SetDateTimeToControl(List<string> shiftVals)
        {
            txtFromDate.Text = Convert.ToDateTime(shiftVals[0]).ToString("dd-MM-yyyy HH:mm");
            txtToDate.Text = Convert.ToDateTime(shiftVals[1]).ToString("dd-MM-yyyy HH:mm");
        }

        //#region "Day Shift Data"
        //private void BindDayShift()
        //{
        //	try
        //	{
        //		List<string> lstPlantData = CockpitDataBaseAccess.GetAllPredefinedShifts();
        //		ddlDayShift.DataSource = lstPlantData;
        //		ddlDayShift.DataBind();
        //		//ddlDayShift.SelectedIndex = 3;
        //	}
        //	catch (Exception ex)
        //	{
        //		lblMessages.Text = ex.Message;
        //	}
        //}
        //#endregion

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
                //ddlCellID.Items.Insert(0, new ListItem
                //{
                //    Text = GetGlobalResourceObject("CommanResource", "CellAll").ToString(),
                //    Value = "All"
                //});
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            tdlbBackButton.Visible = false;
            Session["IonicViewsStack"] = null;
            if (ddlView.SelectedValue == "Plantwise")
            {
                BindPlantData(ddlPlantId.SelectedValue);

            }
            else if (ddlView.SelectedValue == "cellwise")
            {
                BindCellData(ddlPlantId.SelectedValue, DataBaseAccess.getCellIDWithSeparator(lbCellID));
            }
            else if (ddlView.SelectedValue == "Machinewise")
            {
                BindMachineData(ddlPlantId.SelectedValue, DataBaseAccess.getCellIDWithSeparator(lbCellID));
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
        //private bool CheckDateRange()
        //{
        //	bool isDateInRange = false;
        //	try
        //	{

        //		DateTime dt1 = DateTime.Now.Date;
        //		dt1 = Util.GetDateTime(txtFromDate.Text);
        //		//Convert.ToDateTime(txtFromDate.Text, Thread.CurrentThread.CurrentCulture.DateTimeFormat);
        //		DateTime dt2 = DateTime.Now.Date;
        //		//  Convert.ToDateTime(txtToDate.Text, Thread.CurrentThread.CurrentCulture.DateTimeFormat);
        //		dt2 = Util.GetDateTime(txtToDate.Text);
        //		var hours = (dt2 - dt1).TotalHours;

        //		if (Math.Abs(hours) <= 48)
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

        #region "Machine Order By"
        private string MachineOrder(string selectedOrder)
        {
            string orderBy = "";
            switch (selectedOrder)
            {
                case "AE - ASC":
                    orderBy = "AvailabilityEfficiency ASC";
                    break;
                case "PE - ASC":
                    orderBy = "ProductionEfficiency ASC";
                    break;
                case "OE - ASC":
                    orderBy = "OverallEfficiency ASC";
                    break;
                case "AE - DESC":
                    orderBy = "AvailabilityEfficiency DESC";
                    break;
                case "PE - DESC":
                    orderBy = "ProductionEfficiency DESC";
                    break;
                case "OE - DESC":
                    orderBy = "OverallEfficiency DESC";
                    break;
                case "COUNT - ASC":
                    orderBy = "Components ASC";
                    break;
                case "COUNT - DESC":
                    orderBy = "Components DESC";
                    break;
                default:
                    orderBy = "";
                    break;
            }
            return orderBy;
        }
        #endregion

        private void BindData()
        {
            try
            {
                string strDate = string.Empty, endDate = string.Empty;
                DateTime FromDate = DateTime.Now.Date;
                DateTime ToDate = DateTime.Now.Date;

                FromDate = Util.GetDateTime(txtFromDate.Text);
                ToDate = Util.GetDateTime(txtToDate.Text);
                Session["FromDateAgg"] = FromDate.ToString("yyyy-MM-dd HH:mm:ss");
                Session["ToDateAgg"] = ToDate.ToString("yyyy-MM-dd HH:mm:ss");

                if (DateTime.Compare(FromDate, ToDate) > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('From date cannot be greater than To date.')", true);
                    FromDate = DateTime.Now;
                    ToDate = DateTime.Now.AddDays(1);
                    txtFromDate.Text = FromDate.ToString("dd-MM-yyyy hh:mm");
                    txtToDate.Text = ToDate.ToString("dd-MM-yyyy hh:mm");
                    BindData();
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
                    //string orderId = MachineOrder(ddlMachineOrder.SelectedValue.ToString());
                    List<string> val = CockpitDataBaseAccess.GetOrderedLabels(out listOfColNames, "WebCockpitGridColumnAggregate", Session["Language"] == null ? "en" : Session["Language"].ToString());
                    ICockpitStyle colorSetting = CockpitDataBaseAccess.GetCockpitBackColorValues();
                    List<CockpitData> COCKPIT_DATA = new List<CockpitData>();
                    string selectedPlant = ddlPlantId.SelectedValue.ToString();
                    string cellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    if (selectedPlant == "All")
                        selectedPlant = "";
                    if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase))
                        cellId = "";
                    //if (CheckDateRange())
                    //{
                   // COCKPIT_DATA = CockpitDataBaseAccess.GetMachineCockpitDataaggregate("s_GetShiftAgg_ProductionReport", strDate, endDate, selectedPlant, "", listOfColNames, val, "", colorSetting, cellId, ddlView.SelectedValue);
                    //}
                    //else
                    //{
                    //COCKPIT_DATA = CockpitDataBaseAccess.GetMachineCockpitDataaggregate("s_GetShiftAgg_ProductionReport", strDate, endDate, selectedPlant, "", listOfColNames, val, orderId, colorSetting, cellId);
                    //}
                    LstCustomers.DataSource = COCKPIT_DATA;
                    LstCustomers.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        #region "Day Shift Wise change Data"
        protected void ddlDayShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //	//string selectedShift = ddlDayShift.SelectedValue.ToString();
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
            //		BindData();
            //}
            //catch (Exception ex)
            //{
            //	ErrorSignal.FromCurrentContext().Raise(ex);
            //	lblMessages.Text = ex.ToString();
            //	Logger.WriteErrorLog(ex.ToString());
            //}
        }
        #endregion

        protected void timerDataChange_Tick(object sender, EventArgs e)
        {
            try
            {
                BindData();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        //protected void btnTrigger_Click(object sender, EventArgs e)
        //{
        //	try
        //	{
        //		timerDataChange.Interval = Convert.ToInt32(DataBaseAccess.AutoRefreshData);
        //		if (chkAutoBox.Checked)
        //			timerDataChange.Enabled = true;
        //		else
        //			timerDataChange.Enabled = false;
        //	}
        //	catch (Exception ex)
        //	{
        //		ErrorSignal.FromCurrentContext().Raise(ex);
        //		lblMessages.Text = ex.Message;
        //	}
        //}

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellId(ddlPlantId.SelectedItem == null ? "" : ddlPlantId.SelectedItem.Text);
        }

        protected void LstCustomers_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            string machineid = (LstCustomers.Items[e.ItemIndex].FindControl("hfMachineId") as HiddenField).Value;
            string viewtype = (LstCustomers.Items[e.ItemIndex].FindControl("hfViewType") as HiddenField).Value;
        }
        private void BindCellData(string plantid, string cellid)
        {
            try
            {
                setFilterValueToSession();
                string strDate = string.Empty, endDate = string.Empty;
                DateTime FromDate = DateTime.Now.Date;
                DateTime ToDate = DateTime.Now.Date;

                FromDate = Util.GetDateTime(txtFromDate.Text);
                ToDate = Util.GetDateTime(txtToDate.Text);
                Session["FromDateAgg"] = FromDate.ToString("yyyy-MM-dd HH:mm:ss");
                Session["ToDateAgg"] = ToDate.ToString("yyyy-MM-dd HH:mm:ss");

                if (DateTime.Compare(FromDate, ToDate) > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('From date cannot be greater than To date.')", true);
                    //FromDate = DateTime.Now;
                    //ToDate = DateTime.Now.AddDays(1);
                    //txtFromDate.Text = FromDate.ToString("dd-MM-yyyy hh:mm");
                    //txtToDate.Text = ToDate.ToString("dd-MM-yyyy hh:mm");
                    //  BindData();
                }
                else
                {
                    strDate = FromDate.ToString("yyyy-MM-dd");
                    endDate = ToDate.ToString("yyyy-MM-dd");
                    List<string> val = CockpitDataBaseAccess.GetOrderedLabels(out listOfColNames, "WebCockpitGridColumnAggregate", Session["Language"] == null ? "en" : Session["Language"].ToString());
                    ICockpitStyle colorSetting = CockpitDataBaseAccess.GetCockpitBackColorValues();
                    List<CockpitData> COCKPIT_DATA = new List<CockpitData>();
                    if (plantid == "All")
                        plantid = "";
                    if (cellid.Equals("All", StringComparison.OrdinalIgnoreCase))
                        cellid = "";
                    COCKPIT_DATA = CockpitDataBaseAccess.GetMachineCockpitDataaggregate("s_GetShiftAgg_ProductionReport", strDate, endDate, plantid, DataBaseAccess.getMachineIDWithSeparatorForScreen(cellid, "IconicCockpitAgg"), listOfColNames, val, "", colorSetting, cellid, "cellwise", ddlSortOrder.SelectedValue);
                    lvCellDetails.DataSource = COCKPIT_DATA;
                    lvCellDetails.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "graphs", "SetIconicBoxWidth();", true);
                    lvCellDetails.Visible = true;
                    lvPlantDetails.Visible = false;
                    LstCustomers.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindMachineData(string plantid, string cellId)
        {
            try
            {
                setFilterValueToSession();
                string strDate = string.Empty, endDate = string.Empty;
                DateTime FromDate = DateTime.Now.Date;
                DateTime ToDate = DateTime.Now.Date;

                FromDate = Util.GetDateTime(txtFromDate.Text);
                ToDate = Util.GetDateTime(txtToDate.Text);
                Session["FromDateAgg"] = FromDate.ToString("yyyy-MM-dd HH:mm:ss");
                Session["ToDateAgg"] = ToDate.ToString("yyyy-MM-dd HH:mm:ss");

                if (DateTime.Compare(FromDate, ToDate) > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('From date cannot be greater than To date.')", true);
                    //FromDate = DateTime.Now;
                    //ToDate = DateTime.Now.AddDays(1);
                    //txtFromDate.Text = FromDate.ToString("dd-MM-yyyy hh:mm");
                    //txtToDate.Text = ToDate.ToString("dd-MM-yyyy hh:mm");
                    //BindData();
                }
                else
                {
                    strDate = FromDate.ToString("yyyy-MM-dd");
                    endDate = ToDate.ToString("yyyy-MM-dd");
                    List<string> val = CockpitDataBaseAccess.GetOrderedLabels(out listOfColNames, "WebCockpitGridColumnAggregate", Session["Language"] == null ? "en" : Session["Language"].ToString());
                    ICockpitStyle colorSetting = CockpitDataBaseAccess.GetCockpitBackColorValues();
                    List<CockpitData> COCKPIT_DATA = new List<CockpitData>();

                    if (plantid == "All")
                        plantid = "";
                    if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase))
                        cellId = "";
                    COCKPIT_DATA = CockpitDataBaseAccess.GetMachineCockpitDataaggregate("s_GetShiftAgg_ProductionReport", strDate, endDate, plantid, DataBaseAccess.getMachineIDWithSeparatorForScreen(cellId, "IconicCockpitAgg"), listOfColNames, val, "", colorSetting, cellId, "Machinewise", ddlSortOrder.SelectedValue);
                    LstCustomers.DataSource = COCKPIT_DATA;
                    LstCustomers.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "graphs", "SetIconicBoxWidth();", true);
                    LstCustomers.Visible = true;
                    lvPlantDetails.Visible = false;
                    lvCellDetails.Visible = false;

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void BindPlantData(string plantid)
        {
            try
            {
                setFilterValueToSession();
                string strDate = string.Empty, endDate = string.Empty;
                DateTime FromDate = DateTime.Now.Date;
                DateTime ToDate = DateTime.Now.Date;

                FromDate = Util.GetDateTime(txtFromDate.Text);
                ToDate = Util.GetDateTime(txtToDate.Text);
                Session["FromDateAgg"] = FromDate.ToString("yyyy-MM-dd HH:mm:ss");
                Session["ToDateAgg"] = ToDate.ToString("yyyy-MM-dd HH:mm:ss");

                if (DateTime.Compare(FromDate, ToDate) > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('From date cannot be greater than To date.')", true);
                    //FromDate = DateTime.Now;
                    //ToDate = DateTime.Now.AddDays(1);
                    //txtFromDate.Text = FromDate.ToString("dd-MM-yyyy hh:mm");
                    //txtToDate.Text = ToDate.ToString("dd-MM-yyyy hh:mm");
                    //BindData();
                }
                else
                {
                    strDate = FromDate.ToString("yyyy-MM-dd");
                    endDate = ToDate.ToString("yyyy-MM-dd");
                    List<string> val = CockpitDataBaseAccess.GetOrderedLabels(out listOfColNames, "WebCockpitGridColumnAggregate", Session["Language"] == null ? "en" : Session["Language"].ToString());
                    ICockpitStyle colorSetting = CockpitDataBaseAccess.GetCockpitBackColorValues();
                    List<CockpitData> COCKPIT_DATA = new List<CockpitData>();
                    //string selectedPlant = ddlPlantId.SelectedValue.ToString();
                    string cellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    if (plantid == "All")
                        plantid = "";
                    if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase))
                        cellId = "";
                    COCKPIT_DATA = CockpitDataBaseAccess.GetMachineCockpitDataaggregate("s_GetShiftAgg_ProductionReport", strDate, endDate, plantid, DataBaseAccess.getMachineIDWithSeparatorForScreen(cellId, "IconicCockpitAgg"), listOfColNames, val, "", colorSetting, cellId, "Plantwise", ddlSortOrder.SelectedValue);
                    lvPlantDetails.DataSource = COCKPIT_DATA;
                    lvPlantDetails.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "graphs", "SetIconicBoxWidth();", true);
                    lvPlantDetails.Visible = true;
                    lvCellDetails.Visible = false;
                    LstCustomers.Visible = false;
                    lbBackButton.Visible = false;
                    tdlbBackButton.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        protected void lvPlantDetails_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

            Session["IonicViewsStack"] = null;
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
            Session["IonicViewsStack"] = list;
            string plantid = (lvPlantDetails.Items[e.ItemIndex].FindControl("hfPlantId") as HiddenField).Value;
            BindCellData(plantid, DataBaseAccess.getCellIDWithSeparator(lbCellID));
            ddlPlantId.SelectedValue = plantid;
            setFilterValueToSession();
            lbBackButton.Visible = true;
            lbBackButton.Text = "BackToPlant";
            tdlbBackButton.Visible = true;
        }

        protected void lvCellDetails_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
           
            if(Session["IonicViewsStack"]!=null)
            {
                List<IonicViewStack> list = (List<IonicViewStack>)Session["IonicViewsStack"];
                IonicViewStack data = new IonicViewStack();
                data.PlantId = ddlPlantId.SelectedValue;
                data.CellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                data.Order = 2;
                data.ViewBeforeClick = "cellwise";
                data.SortOrder = ddlSortOrder.SelectedValue;
                ddlView.SelectedValue = "Machinewise";
                ddlView_SelectedIndexChanged(null, null);
                list.Add(data);
                Session["IonicViewsStack"] = list;
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
                Session["IonicViewsStack"] = list;
            }
            string plantid = (lvCellDetails.Items[e.ItemIndex].FindControl("hdnCellPlantID") as HiddenField).Value;
            string cellid = (lvCellDetails.Items[e.ItemIndex].FindControl("hfCellId") as HiddenField).Value;
            BindMachineData(plantid,cellid );

            ddlPlantId.SelectedValue = plantid;
            DataBaseAccess.setCellIDListBox(lbCellID, cellid);
            setFilterValueToSession();
            lbBackButton.Visible = true;
            lbBackButton.Text = "BackToCell";
            tdlbBackButton.Visible = true;
        }

        protected void lbBackButton_Click(object sender, EventArgs e)
        {
            string text = lbBackButton.Text;
            if (text == "BackToCell")
            {
                if (Session["IonicViewsStack"] != null)
                {
                    List<IonicViewStack> list = (List<IonicViewStack>)Session["IonicViewsStack"];
                    if (list.Count > 0)
                    {
                        int i = 0;
                        if(list.Count==1)
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
                        BindCellData(ddlPlantId.SelectedValue, DataBaseAccess.getCellIDWithSeparator(lbCellID));
                        list.RemoveAll(x => x.Order == i+1);
                        if(list.Count>0)
                        {
                            Session["IonicViewsStack"] = list;
                            lbBackButton.Text = "BackToPlant";
                        }
                        else
                        {
                            Session["IonicViewsStack"] = null;
                            lbBackButton.Visible = false;
                            tdlbBackButton.Visible = false;
                        }
                    }
                }

            }
            else if (text == "BackToPlant")
            {
                if (Session["IonicViewsStack"] != null)
                {
                    List<IonicViewStack> list = (List<IonicViewStack>)Session["IonicViewsStack"];
                    if(list.Count>0)
                    {
                        ddlPlantId.SelectedValue = list[0].PlantId;
                        DataBaseAccess.setCellIDListBox(lbCellID, list[0].CellId);
                        ddlView.SelectedValue = list[0].ViewBeforeClick;
                        ddlView_SelectedIndexChanged(null, null);
                        ddlSortOrder.SelectedValue = list[0].SortOrder;
                        BindPlantData(ddlPlantId.SelectedValue);
                    }
                   
                    Session["IonicViewsStack"] = null;
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
            list.Add(new ListItem("PE - Desc", "PEffy desc"));
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
            Response.Redirect(string.Format("~/TableViewAggregate.aspx?setFilter={0}&backButtonText={1}", "1", backButtonText), false);
        }
    }
    public class IonicViewStack
    {
        public string PlantId { get; set; }
        public string CellId { get; set; }
        public int Order { get; set; }
        public string ViewBeforeClick { get; set; }
        public string SortOrder { get; set; }
    }
}