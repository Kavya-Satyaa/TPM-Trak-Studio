using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessClassLibrary;
using Elmah;
using Web_TPMTrakDashboard.Models;
using CockpitDataBaseAccess = Web_TPMTrakDashboard.Models.CockpitDataBaseAccess;
using System.Threading;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Services;

namespace Web_TPMTrakDashboard
{
    public partial class IonicView : System.Web.UI.Page
    {
        public static AppUISettings settings = null;
        List<string> listOfColNames = new List<string>();
        string defaultShift = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
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
                    //	txtFromDate.Text = Convert.ToDateTime(Session["FromDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    //	txtToDate.Text = Convert.ToDateTime(Session["ToDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    //}
                    //else
                    //{
                    gettimings();
                    //}

                    BindPlantId();
                    if (setFilterFromSession == 1)
                    {
                        txtFromDate.Text = filterData.FromDate;
                        txtToDate.Text = filterData.ToDate;
                        HelperClassGeneric.setDropdownValue(ddlDayShift, filterData.PredefinedTime);
                        HelperClassGeneric.setDropdownValue(ddlPlantId, filterData.PlantId);
                    }
                    BindCellId(ddlPlantId.SelectedItem == null ? "" : ddlPlantId.SelectedItem.Text);
                    //txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy HH:mm tt");
                    //txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm tt");               

                    //ddlDayShift_SelectedIndexChanged("frist", null);
                    // BindData();
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
                        Session["LAIonicViewsStack"] = Session["LATableViewsStack"] as List<LVIonicViewStack>;
                        lbBackButton.Text = backButtonText;
                        tdlbBackButton.Visible = true;
                        lbBackButton.Visible = true;
                    }
                    timerDataChange.Enabled = false;
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void setDefaultView()
        {
            try
            {
                string view = CockpitDataBaseAccess.getHistoricalAnalyticsDefaultView("LiveDefaultView", "Ionic");
                if (view != "")
                {
                    ddlView.SelectedValue = view;
                    // Session["CurrentView"] = ddlView.SelectedValue;
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void gettimings()
        {
            try
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
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }


        private void SetDateTimeToControl(List<string> shiftVals)
        {
            txtFromDate.Text = Convert.ToDateTime(shiftVals[0]).ToString("dd-MM-yyyy HH:mm:ss");
            txtToDate.Text = Convert.ToDateTime(shiftVals[1]).ToString("dd-MM-yyyy HH:mm:ss");
        }

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
                Logger.WriteErrorLog("BindDayShift: " + ex.Message);
                //lblMessages.Text = ex.Message;
            }
        }
        #endregion

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
                Logger.WriteErrorLog("BindDayShift: " + ex.Message);
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
                //ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog("BindDayShift: " + ex.Message);
            }
        }
        #endregion

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                // BindData();
                tdlbBackButton.Visible = false;
                Session["LAIonicViewsStack"] = null;
                //  Session["CurrentView"] = ddlView.SelectedValue;
                if (ddlView.SelectedValue == "Plantwise")
                {
                    BindPlantData(ddlPlantId.SelectedValue, ddlSortOrder.SelectedValue);

                }
                else if (ddlView.SelectedValue == "cellwise")
                {
                    BindCellData(ddlPlantId.SelectedValue, DataBaseAccess.getCellIDWithSeparator(lbCellID), ddlSortOrder.SelectedValue);
                }
                else if (ddlView.SelectedValue == "Machinewise")
                {
                    BindMachineData(ddlPlantId.SelectedValue, DataBaseAccess.getCellIDWithSeparator(lbCellID), ddlSortOrder.SelectedValue);
                }
                hdnView.Value = ddlView.SelectedValue;
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void setFilterValueToSession()
        {
            try
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
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private List<ListItem> getStorkeColumn()
        {
            List<ListItem> list = new List<ListItem>();
            try
            {
                List<string> listOfColNames = new List<string>();
                List<string> val = CockpitDataBaseAccess.GetOrderedLabels(out listOfColNames, "CockpitGridColumn", Session["Language"] == null ? "en" : Session["Language"].ToString());
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
        private void BindPlantData(string plantid, string sortOrder)
        {
            try
            {
                setFilterValueToSession();
                hdnView.Value = ddlView.SelectedValue;
                string strDate = string.Empty, endDate = string.Empty;
                DateTime FromDate = DateTime.Now.Date;
                DateTime ToDate = DateTime.Now.Date;

                FromDate = Util.GetDateTime(txtFromDate.Text);
                ToDate = Util.GetDateTime(txtToDate.Text);
                Session["FromDate"] = FromDate.ToString("yyyy-MM-dd HH:mm:ss");
                Session["ToDate"] = ToDate.ToString("yyyy-MM-dd HH:mm:ss");

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
                    lblMessages.Text = "Difference between to date and from date cannot be more than " + interval + " days.<br> Please visit Historical Analytics -> Iconic View to see more than " + interval + " days of data.";
                    return;
                }
                else
                {
                    strDate = FromDate.ToSQLDateTimeFormat();
                    endDate = ToDate.ToSQLDateTimeFormat();
                    string orderId = MachineOrder(ddlMachineOrder.SelectedValue.ToString());
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
                    ICockpitStyle colorSetting = CockpitDataBaseAccess.GetCockpitBackColorValues();
                    List<CockpitData> COCKPIT_DATA = new List<CockpitData>();
                    // string selectedPlant = ddlPlantId.SelectedValue.ToString();
                    string cellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    if (plantid == "All")
                        plantid = "";
                    if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase) || cellId.Equals("LineAll", StringComparison.OrdinalIgnoreCase))
                        cellId = "";
                    //if (CheckDateRange())
                    //{
                    COCKPIT_DATA = CockpitDataBaseAccess.GetMachineCockpitDataForPlantCell("s_GetCockpitData_WithTempTable_eshopx", strDate, endDate, plantid, DataBaseAccess.getMachineIDWithSeparatorForScreen(cellId, "IconicCockpitLive"), listOfColNames, val, orderId, colorSetting, cellId, "Plantwise", sortOrder);
                    //}
                    //else
                    //{
                    //    COCKPIT_DATA = CockpitDataBaseAccess.GetMachineCockpitDataForPlantCell("s_GetCockpitData_eshopx", strDate, endDate, plantid, "", listOfColNames, val, orderId, colorSetting, cellId, "Plantwise", sortOrder);
                    //}
                    lvPlantDetails.DataSource = COCKPIT_DATA;
                    lvPlantDetails.DataBind();
                    lvPlantDetails.Visible = true;
                    lvCellDetails.Visible = false;
                    LstCustomers.Visible = false;
                    lbBackButton.Visible = false;
                    tdlbBackButton.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "graphs", "SetIconicBoxWidth();", true);
                }
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void BindCellData(string plantid, string cellId, string sortOrder)
        {
            try
            {
                setFilterValueToSession();
                hdnView.Value = ddlView.SelectedValue;
                string strDate = string.Empty, endDate = string.Empty;
                DateTime FromDate = DateTime.Now.Date;
                DateTime ToDate = DateTime.Now.Date;

                FromDate = Util.GetDateTime(txtFromDate.Text);
                ToDate = Util.GetDateTime(txtToDate.Text);
                Session["FromDate"] = FromDate.ToString("yyyy-MM-dd HH:mm:ss");
                Session["ToDate"] = ToDate.ToString("yyyy-MM-dd HH:mm:ss");

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
                    lblMessages.Text = "Difference between to date and from date cannot be more than " + interval + " days.<br> Please visit Historical Analytics -> Iconic View to see more than " + interval + " days of data.";
                    return;
                }
                else
                {
                    strDate = FromDate.ToSQLDateTimeFormat();
                    endDate = ToDate.ToSQLDateTimeFormat();
                    string orderId = MachineOrder(ddlMachineOrder.SelectedValue.ToString());
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
                    ICockpitStyle colorSetting = CockpitDataBaseAccess.GetCockpitBackColorValues();
                    List<CockpitData> COCKPIT_DATA = new List<CockpitData>();
                    //  string selectedPlant = ddlPlantId.SelectedValue.ToString();
                    // string cellId = ddlCellID.SelectedValue.ToString();
                    if (plantid == "All")
                        plantid = "";
                    if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase) || cellId.Equals("LineAll", StringComparison.OrdinalIgnoreCase))
                        cellId = "";
                    //if (CheckDateRange())
                    //{
                    COCKPIT_DATA = CockpitDataBaseAccess.GetMachineCockpitDataForPlantCell("s_GetCockpitData_WithTempTable_eshopx", strDate, endDate, plantid, DataBaseAccess.getMachineIDWithSeparatorForScreen(cellId, "IconicCockpitLive"), listOfColNames, val, orderId, colorSetting, cellId, "cellwise", sortOrder);
                    //}
                    //else
                    //{
                    //    COCKPIT_DATA = CockpitDataBaseAccess.GetMachineCockpitDataForPlantCell("s_GetCockpitData_eshopx", strDate, endDate, plantid, "", listOfColNames, val, orderId, colorSetting, cellId, "cellwise", sortOrder);
                    //}
                    lvCellDetails.DataSource = COCKPIT_DATA;
                    lvCellDetails.DataBind();
                    lvCellDetails.Visible = true;
                    lvPlantDetails.Visible = false;
                    LstCustomers.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "graphs", "SetIconicBoxWidth();", true);
                }
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void BindMachineData(string plantid, string cellId, string sortOrder)
        {
            try
            {
                setFilterValueToSession();
                hdnView.Value = ddlView.SelectedValue;
                string strDate = string.Empty, endDate = string.Empty;
                DateTime FromDate = DateTime.Now.Date;
                DateTime ToDate = DateTime.Now.Date;

                FromDate = Util.GetDateTime(txtFromDate.Text);
                ToDate = Util.GetDateTime(txtToDate.Text);
                Session["FromDate"] = FromDate.ToString("yyyy-MM-dd HH:mm:ss");
                Session["ToDate"] = ToDate.ToString("yyyy-MM-dd HH:mm:ss");

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
                    lblMessages.Text = "Difference between to date and from date cannot be more than " + interval + " days.<br> Please visit Historical Analytics -> Iconic View to see more than " + interval + " days of data.";
                    return;
                }
                else
                {
                    strDate = FromDate.ToSQLDateTimeFormat();
                    endDate = ToDate.ToSQLDateTimeFormat();
                    string orderId = MachineOrder(ddlMachineOrder.SelectedValue.ToString());
                    List<string> val = CockpitDataBaseAccess.GetOrderedLabels(out listOfColNames, "CockpitGridColumn", Session["Language"] == null ? "en" : Session["Language"].ToString());
                    ICockpitStyle colorSetting = CockpitDataBaseAccess.GetCockpitBackColorValues();
                    List<CockpitData> COCKPIT_DATA = new List<CockpitData>();
                    //   string selectedPlant = ddlPlantId.SelectedValue.ToString();
                    //   string cellId = ddlCellID.SelectedValue.ToString();
                    if (plantid == "All")
                        plantid = "";
                    if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase) || cellId.Equals("LineAll", StringComparison.OrdinalIgnoreCase))
                        cellId = "";
                    //if (CheckDateRange())
                    //{
                    COCKPIT_DATA = CockpitDataBaseAccess.GetMachineCockpitData("s_GetCockpitData_WithTempTable_eshopx", strDate, endDate, plantid, DataBaseAccess.getMachineIDWithSeparatorForScreen(cellId, "IconicCockpitLive"), listOfColNames, val, orderId, colorSetting, cellId, "Machinewise", sortOrder);
                    //}
                    //else
                    //{
                    //    COCKPIT_DATA = CockpitDataBaseAccess.GetMachineCockpitData("s_GetCockpitData_eshopx", strDate, endDate, plantid, "", listOfColNames, val, orderId, colorSetting, cellId, "Machinewise", sortOrder);
                    //}
                    LstCustomers.DataSource = COCKPIT_DATA;
                    LstCustomers.DataBind();
                    LstCustomers.Visible = true;
                    lvPlantDetails.Visible = false;
                    lvCellDetails.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "graphs", "SetIconicBoxWidth();", true);
                }
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private bool CheckDateRange()
        {
            bool isDateInRange = false;
            try
            {

                DateTime dt1 = DateTime.Now.Date;
                dt1 = Util.GetDateTime(txtFromDate.Text);
                //Convert.ToDateTime(txtFromDate.Text, Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                DateTime dt2 = DateTime.Now.Date;
                //  Convert.ToDateTime(txtToDate.Text, Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                dt2 = Util.GetDateTime(txtToDate.Text);
                var hours = (dt2 - dt1).TotalHours;

                if (Math.Abs(hours) <= 168)
                {
                    isDateInRange = true;
                }
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
            return isDateInRange;
        }

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
                Session["FromDate"] = FromDate.ToString("yyyy-MM-dd HH:mm:ss");
                Session["ToDate"] = ToDate.ToString("yyyy-MM-dd HH:mm:ss");

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
                    string orderId = MachineOrder(ddlMachineOrder.SelectedValue.ToString());
                    List<string> val = CockpitDataBaseAccess.GetOrderedLabels(out listOfColNames, "CockpitGridColumn", Session["Language"] == null ? "en" : Session["Language"].ToString());
                    ICockpitStyle colorSetting = CockpitDataBaseAccess.GetCockpitBackColorValues();
                    List<CockpitData> COCKPIT_DATA = new List<CockpitData>();
                    string selectedPlant = ddlPlantId.SelectedValue.ToString();
                    string cellId = DataBaseAccess.getCellIDWithSeparator(lbCellID);
                    if (selectedPlant == "All")
                        selectedPlant = "";
                    if (cellId.Equals("All", StringComparison.OrdinalIgnoreCase) || cellId.Equals("LineAll", StringComparison.OrdinalIgnoreCase))
                        cellId = "";
                    if (CheckDateRange())
                    {
                        // COCKPIT_DATA = CockpitDataBaseAccess.GetMachineCockpitData("s_GetCockpitData_WithTempTable_eshopx", strDate, endDate, selectedPlant, "", listOfColNames, val, orderId, colorSetting, cellId);
                    }
                    else
                    {
                        // COCKPIT_DATA = CockpitDataBaseAccess.GetMachineCockpitData("s_GetCockpitData_eshopx", strDate, endDate, selectedPlant, "", listOfColNames, val, orderId, colorSetting, cellId);
                    }
                    LstCustomers.DataSource = COCKPIT_DATA;
                    LstCustomers.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "graphs", "SetIconicBoxWidth();", true);
                }
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
        }

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
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        #endregion

        protected void timerDataChange_Tick(object sender, EventArgs e)
        {
            try
            {
                bindCurrentData();
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog("BindDayShift: " + ex.Message);
            }
        }

        protected void btnTrigger_Click(object sender, EventArgs e)
        {
            try
            {
                timerDataChange.Interval = Convert.ToInt32(DataBaseAccess.AutoRefreshData);
                if (chkAutoBox.Checked)
                    timerDataChange.Enabled = true;
                else
                    timerDataChange.Enabled = false;
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog("BindDayShift: " + ex.Message);
            }
        }

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellId(ddlPlantId.SelectedItem == null ? "" : ddlPlantId.SelectedItem.Text);
        }
        private void bindCurrentData()
        {
            string view = "";
            string plantid = ddlPlantId.SelectedValue;
            string cellid = DataBaseAccess.getCellIDWithSeparator(lbCellID);
            view = ddlView.SelectedValue.ToString();
            // ddlView.SelectedValue = view;
            //ddlView_SelectedIndexChanged(null, null);
            if (view == "Plantwise")
            {
                BindPlantData(plantid, ddlSortOrder.SelectedValue);
            }
            else if (view == "cellwise")
            {
                BindCellData(plantid, cellid, ddlSortOrder.SelectedValue);
            }
            else
            {
                BindMachineData(plantid, cellid, ddlSortOrder.SelectedValue);
            }
            //if (Session["LAIonicViewsStack"] != null)
            //{
            //    List<LVIonicViewStack> list = (List<LVIonicViewStack>)Session["LAIonicViewsStack"];
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
            //            BindPlantData(plantid,ddlSortOrder.SelectedValue);
            //        }
            //        else if (view == "cellwise")
            //        {
            //            BindCellData(plantid, cellid,ddlSortOrder.SelectedValue);
            //        }
            //        else
            //        {
            //            BindMachineData(plantid, cellid, ddlSortOrder.SelectedValue);
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
            //            BindPlantData(plantid, ddlSortOrder.SelectedValue);
            //        }
            //        else if (view == "cellwise")
            //        {
            //            BindCellData(plantid, cellid, ddlSortOrder.SelectedValue);
            //        }
            //        else
            //        {
            //            BindMachineData(plantid, cellid, ddlSortOrder.SelectedValue);
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
            //        BindPlantData(plantid, ddlSortOrder.SelectedValue);
            //    }
            //    else if (view == "cellwise")
            //    {
            //        BindCellData(plantid, cellid, ddlSortOrder.SelectedValue);
            //    }
            //    else
            //    {
            //        BindMachineData(plantid, cellid, ddlSortOrder.SelectedValue);
            //    }
            //}
        }
        protected void lvCellDetails_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            if (Session["LAIonicViewsStack"] != null)
            {
                List<LVIonicViewStack> list = (List<LVIonicViewStack>)Session["LAIonicViewsStack"];
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
                Session["LAIonicViewsStack"] = list;
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
                Session["LAIonicViewsStack"] = list;
            }
            string plantid = (lvCellDetails.Items[e.ItemIndex].FindControl("hdnCellPlantID") as HiddenField).Value;
            string cellid = (lvCellDetails.Items[e.ItemIndex].FindControl("hfCellId") as HiddenField).Value;
            BindMachineData(plantid, cellid, "");

            ddlPlantId.SelectedValue = plantid;
            DataBaseAccess.setCellIDListBox(lbCellID, cellid);
            setFilterValueToSession();
            lbBackButton.Visible = true;
            lbBackButton.Text = "BackToCell";
            tdlbBackButton.Visible = true;
        }
        protected void lvPlantDetails_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            Session["LAIonicViewsStack"] = null;
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
            Session["LAIonicViewsStack"] = list;
            string plantid = (lvPlantDetails.Items[e.ItemIndex].FindControl("hfPlantId") as HiddenField).Value;
            BindCellData(plantid, DataBaseAccess.getCellIDWithSeparator(lbCellID), ddlSortOrder.SelectedValue);
            ddlPlantId.SelectedValue = plantid;
            setFilterValueToSession();
            lbBackButton.Visible = true;
            lbBackButton.Text = "BackToPlant";
            tdlbBackButton.Visible = true;
        }
        protected void lbBackButton_Click1(object sender, EventArgs e)
        {
            string text = lbBackButton.Text;
            if (text == "BackToCell")
            {
                if (Session["LAIonicViewsStack"] != null)
                {
                    List<LVIonicViewStack> list = (List<LVIonicViewStack>)Session["LAIonicViewsStack"];
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
                        BindCellData(ddlPlantId.SelectedValue, DataBaseAccess.getCellIDWithSeparator(lbCellID), ddlSortOrder.SelectedValue);
                        list.RemoveAll(x => x.Order == i + 1);
                        if (list.Count > 0)
                        {
                            Session["LAIonicViewsStack"] = list;
                            lbBackButton.Text = "BackToPlant";
                        }
                        else
                        {
                            Session["LAIonicViewsStack"] = null;
                            lbBackButton.Visible = false;
                            tdlbBackButton.Visible = false;
                        }
                    }
                }
                else
                {
                    Session["LAIonicViewsStack"] = null;
                    lbBackButton.Visible = false;
                    tdlbBackButton.Visible = false;
                }
            }
            else if (text == "BackToPlant")
            {
                if (Session["LAIonicViewsStack"] != null)
                {
                    List<LVIonicViewStack> list = (List<LVIonicViewStack>)Session["LAIonicViewsStack"];
                    if (list.Count > 0)
                    {
                        ddlPlantId.SelectedValue = list[0].PlantId;
                        DataBaseAccess.setCellIDListBox(lbCellID, list[0].CellId);
                        ddlView.SelectedValue = list[0].ViewBeforeClick;
                        ddlView_SelectedIndexChanged(null, null);
                        ddlSortOrder.SelectedValue = list[0].SortOrder;
                        BindPlantData(ddlPlantId.SelectedValue, ddlSortOrder.SelectedValue);
                    }

                    Session["LAIonicViewsStack"] = null;
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
            Response.Redirect(string.Format("~/tableView.aspx?setFilter={0}&backButtonText={1}", "1", backButtonText), false);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool isPageEnabled(string pageName)
        {
            bool pageEnabled = false;
            try
            {
                if (ConfigurationManager.AppSettings[pageName].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    pageEnabled = true;

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("isPageEnabled =" + ex.Message);
            }
            return pageEnabled;
        }
    }
    public class LVIonicViewStack
    {
        public string PlantId { get; set; }
        public string CellId { get; set; }
        public int Order { get; set; }
        public string View { get; set; }
        public string ViewBeforeClick { get; set; }
        public string SortOrder { get; set; }
    }
}