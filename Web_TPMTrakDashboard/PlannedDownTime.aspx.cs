using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.KTASpindle;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.PTA;

namespace Web_TPMTrakDashboard
{
    public partial class PlannedDownTime : System.Web.UI.Page
    {
        int allChecked = 0;
        //SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EmpDB"].ConnectionString);
        //SqlCommand sqlCommand = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["FltrDate"] = null;
                Session["FltrReason"] = null;
                BindPlantID();
                BindShift();
                foreach (ListItem item in ddlMachineIDs.Items)
                {
                    item.Selected = true;
                }
                BindHolidaysDropDowns();
                setActiveTab("holiday");
                btnHodiday_Click(null, null);
            }
            // loadGvMachineShift();
        }

        private void BindShift()
        {
            try
            {
                var allShift = BindCockpitView.GetAllShift();
                if (allShift != null && allShift.Count > 0)
                {
                    ddlshiftsDowns.DataSource = allShift;
                    ddlshiftsDowns.DataBind();
                    lbShiftNameShiftDownFilter.DataSource = allShift;
                    lbShiftNameShiftDownFilter.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindPlantID()
        {
            try
            {
                ddlPlantID.DataSource = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllPlants();
                ddlPlantID.DataBind();
                BindMachineIDs();
                //ddlPlantId.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommanResource", "PlantAll").ToString(), "All"));
                //ddlPlantId_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                // lblMessages.ForeColor = System.Drawing.Color.Red;
                //lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }
        private void BindMachineIDs()
        {
            try
            {
                string plant = ddlPlantID.SelectedValue == null ? "" : ddlPlantID.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantID.SelectedItem.ToString();
                //string Cell = ddlCell.SelectedValue == null ? "" : ddlCell.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedItem.ToString();
                List<string> list = DataBaseAccess.getmachineidfromplant(plant);
                list.Remove("All");
                ddlMachineIDs.DataSource = list;
                // ddlMachineIDs.DataSource = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllMacForPlant(plant);
                ddlMachineIDs.DataBind();

                ddlMachineAllDown.DataSource = list;
                ddlMachineAllDown.DataBind();
                //ddlFilterWeekByMachine.DataSource = list;
                //ddlFilterWeekByMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                // lblMessages.ForeColor = System.Drawing.Color.Red;
                //lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }
     

        private void BindHolidaysDropDowns()
        {
            try
            {
                txtFromYear.Text = DateTime.Now.Year.ToString();
                txtToYear.Text = DateTime.Now.Year.ToString();
                var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                for (int i = 0; i < months.Length - 1; i++)
                {
                    ddlMonth.Items.Add(new ListItem(months[i], i.ToString()));
                }
                for (int i = 1; i <= 31; i++)
                {
                    ddlDay.Items.Add(i.ToString());
                }
              
            }
            catch (Exception ex)
            {

            }
        }
        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedMonth = ddlMonth.SelectedItem.Text;
            switch (selectedMonth)
            {
                case "January":
                case "March":
                case "May":
                case "July":
                case "August":
                case "October":
                case "December":
                    ddlDay.Items.Clear();
                    for (int i = 1; i <= 31; i++)
                    {
                        ddlDay.Items.Add(i.ToString());
                    }
                    break;
                case "February":
                    ddlDay.Items.Clear();
                    for (int i = 1; i <= 29; i++)
                    {
                        ddlDay.Items.Add(i.ToString());
                    }
                    break;
                case "April":
                case "June":
                case "September":
                case "November":
                    ddlDay.Items.Clear();
                    for (int i = 1; i < 31; i++)
                    {
                        ddlDay.Items.Add(i.ToString());
                    }
                    break;
            }
        }

        #region ---------- New ---------------------
        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineIDs();
        }
        private void setActiveTab(string tab)
        {
            tabHoliday.Attributes["class"] = "";
            tabWeeklyOff.Attributes["class"] = "";
            tabDailyDown.Attributes["class"] = "";
            tabShiftDown.Attributes["class"] = "";
            tabAllDown.Attributes["class"] = "";
            tabHolidayContent.Attributes["class"] = "";
            tabWeeklyOffContent.Attributes["class"] = "";
            tabDailyDownContent.Attributes["class"] = "";
            tabShiftDownContent.Attributes["class"] = "";
            tabAllDownContent.Attributes["class"] = "";
            gvHolidays.Visible = false;
            gvWeekOffs.Visible = false;
            gvDailyDowns.Visible = false;
            gvMachineShift.Visible = false;
            gvAllDowns.Visible = false;
            hdnActiveTab.Value = tab;
            if (tab == "holiday")
            {
                tabHoliday.Attributes.Add("class", "active");
                tabHolidayContent.Attributes.Add("class", "active");
                gvHolidays.Visible = true;
            }
            else if (tab == "weeklyoff")
            {
                tabWeeklyOff.Attributes.Add("class", "active");
                tabWeeklyOffContent.Attributes.Add("class", "active");
                gvWeekOffs.Visible = true;
            }
            else if (tab == "dailydown")
            {
                tabDailyDown.Attributes.Add("class", "active");
                tabDailyDownContent.Attributes.Add("class", "active");
                gvDailyDowns.Visible = true;
            }
            else if (tab == "shiftdown")
            {
                tabShiftDown.Attributes.Add("class", "active");
                tabShiftDownContent.Attributes.Add("class", "active");
                gvMachineShift.Visible = true;
            }
            else if (tab == "alldown")
            {
                tabAllDown.Attributes.Add("class", "active");
                tabAllDownContent.Attributes.Add("class", "active");
                gvAllDowns.Visible = true;
            }

        }
        #endregion

        #region  -- Holiday----
        protected void btnHodiday_Click(object sender, EventArgs e)
        {
            //plantMachineFilterContainer.Style.Add("visibility", "visible");
            setActiveTab("holiday");
            divHolidayFilter.Visible = false;
            chkSelectAllHoliday.Visible = false;
            if (divHolidayFilter.Visible)
            {
                btnApplyHoliday_Click(null, null);
            }
            BindHolidayFilterReason("");
            List<HolidayListDetails> holidayListDetailsList = new List<HolidayListDetails>();
            holidayListDetailsList.Add(new HolidayListDetails { Holiday="", Reason="", MachineID = "" });
            gvHolidays.DataSource = holidayListDetailsList;
            gvHolidays.DataBind();
            ddlReason.Value = "";
        }
        private void BindHolidayData(string value, string value2, string reason, string machine, string param)
        {
            try
            {
                List<HolidayListDetails> holidayListDetailsList = new List<HolidayListDetails>();
                holidayListDetailsList = DataBaseAccess.getHolidayList(value, value2, reason, machine, param);
                gvHolidays.DataSource = holidayListDetailsList;
                gvHolidays.DataBind();
                chkSelectAllHoliday.Checked = false;
                chkSelectAllHoliday.Visible = false;
                Session["HolidayList"] = holidayListDetailsList;
                if(holidayListDetailsList.Count>1)
                {
                    chkSelectAllHoliday.Visible = true;
                }
                ViewState["SelectAll"] = false;
            }
            catch (Exception ex)
            {

            }
        }
        private void BindHolidayFilterReason(string param)
        {
            try
            {
                List<string> list = DataBaseAccess.getAllReasons();
                if (param == "")
                {
                    ddlFilterByReason.DataSource = list;
                    ddlFilterByReason.DataBind();
                    ddlFilterByReason.Items.Insert(0, "All");
                }
                else if (param == "deleteTheData")
                {
                    string reason = ddlFilterByReason.Text;
                    int flag = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (reason == list[i])
                        {
                            flag = 1;
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        ddlFilterByReason.DataSource = list;
                        ddlFilterByReason.DataBind();
                        ddlFilterByReason.Items.Insert(0, "All");
                    }
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (ddlFilterByReason.Items.FindByValue(list[i]) == null)
                        {
                            ddlFilterByReason.Items.Add(list[i]);
                        }
                    }
                }

            }
            catch (Exception ex)
            { }
        }
        protected void lbtnReload_Click(object sender, EventArgs e)
        {
            btnHodiday_Click(null, null);
        }
        protected void lnkFilter_Click(object sender, EventArgs e)
        {
            try
            {
                divHolidayFilter.Visible = true;
                ddlFilter.SelectedValue = "ByDate";
                ddlFilter_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            { }
        }
        protected void btnSaveHoliday_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("StartTime", typeof(DateTime));
                dt.Columns.Add("EndTime", typeof(DateTime));
                dt.Columns.Add("Machine", typeof(string));
                dt.Columns.Add("DownReason", typeof(string));
                dt.Columns.Add("DownType", typeof(string));
                dt.Columns.Add("DayName", typeof(string));
                dt.Columns.Add("ShiftName", typeof(string));
                dt.Columns.Add("ShiftStart", typeof(string));
                dt.Columns.Add("ShiftEnd", typeof(string));
                if (txtFromYear.Text.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select From Year.');", true);
                    return;
                }
                if (txtToYear.Text.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select To Year.');", true);
                    return;
                }
                int fromYear = Convert.ToInt16(txtFromYear.Text.Trim());
                int toYear = Convert.ToInt16(txtToYear.Text.Trim());
                string month = (Convert.ToInt16((ddlMonth.SelectedValue == null ? "" : ddlMonth.SelectedValue.ToString())) + 1).ToString("00");
                string day = (Convert.ToInt16((ddlDay.SelectedItem == null ? "" : ddlDay.SelectedItem.ToString()))).ToString("00");
                string mc = "";
                foreach (ListItem item in ddlMachineIDs.Items)
                {
                    if (item.Selected)
                    {
                        mc += item.Value + ",";
                    }
                }
                if (mc.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select Machine.');", true);
                    return;
                }
                string reason = ddlReason.Value.Trim();
                if (reason == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "openWarningModal('Please enter Reason.')", true);
                    return;
                }
                string sqlQuery = "";
                for (int i = fromYear; i <= toYear; i++)
                {
                    string holiday = i + "-" + month + "-" + day + " 00:00:00";
                    DateTime incrementDate = Util.GetDateTime(holiday);
                   
                    foreach (ListItem item in ddlMachineIDs.Items)
                    {
                        if (item.Selected)
                        {
                            var tblRow = dt.NewRow();
                            tblRow[0] = Convert.ToDateTime(VDGDataBaseAccess.GetLogicalDayStart(incrementDate.ToString("yyyy-MM-dd HH:mm:ss")));
                            tblRow[1] = Convert.ToDateTime(VDGDataBaseAccess.GetLogicalDayEnd(incrementDate.ToString("yyyy-MM-dd HH:mm:ss")));
                            tblRow[2] = item.Value;
                            tblRow[3] = reason;
                            tblRow[4] = "Holiday";
                            tblRow[5] = "";
                            tblRow[6] = "";
                            dt.Rows.Add(tblRow);
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    DataBaseAccess.deletePORawDataRecords();
                    List<PDTData> rejectedShiftDownItems = new List<PDTData>();
                    rejectedShiftDownItems = DataBaseAccess.saveShiftDownDetails(dt, "HolidaySave");
                    if (rejectedShiftDownItems.Count > 0)
                    {
                        if (rejectedShiftDownItems.Count != dt.Rows.Count)
                        {
                            ddlReason.Value = "";
                        }
                        Session["PDTRejectedItems"] = rejectedShiftDownItems;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "displayRejectedItems();", true);
                    }
                    else
                    {
                        ddlReason.Value = "";
                        Session["PDTRejectedItems"] = null;
                    }
                }
                BindHolidayFilterReason("appendTheData");
                if (divHolidayFilter.Visible)
                {
                    btnApplyHoliday_Click(null, null);
                }
                else
                {
                    BindHolidayData("", "", "", "", "");
                }

                //divHolidayFilter.Visible = false;
                //ddlReason.Value = "";
            }
            catch (Exception ex)
            {

            }


        }
        protected void lnkHolidayFilterClose_Click(object sender, EventArgs e)
        {
            divHolidayFilter.Visible = false;
            chkSelectAllHoliday.Visible = false;
            btnHodiday_Click(null, null);
        }
        protected void btnApplyHoliday_Click(object sender, EventArgs e)
        {
            try
            {
                string filterValue = ddlFilter.SelectedValue == null ? "" : ddlFilter.SelectedValue.ToString();
                switch (filterValue)
                {
                    case "ByDate":
                        DateTime fromdate = DateTime.Now.Date;
                        DateTime todate = DateTime.Now.Date;
                        fromdate = Util.GetDateTime(txtFilterByDateFrom.Text + " 00:00:00");
                        todate = Util.GetDateTime(txtFilterByDateTo.Text + " 00:00:00");
                        string reason = ddlFilterByReason.SelectedValue == "All" ? "" : ddlFilterByReason.SelectedValue.ToString();
                        string machine1 = "";
                        foreach (ListItem item in ddlFilterByMachines.Items)
                        {
                            if (item.Selected)
                            {
                                if (machine1 == "")
                                {
                                    machine1 = "'" + item.Text + "'";
                                }
                                else
                                {
                                    machine1 += ",'" + item.Text + "'";
                                }

                            }

                        }
                        if (string.IsNullOrEmpty(machine1))
                        {
                            List<HolidayListDetails> holidayListDetailsList = new List<HolidayListDetails>();
                            holidayListDetailsList.Add(new HolidayListDetails { Holiday = "", Reason = "", MachineID = "" });
                            gvHolidays.DataSource = holidayListDetailsList;
                            gvHolidays.DataBind();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select Machine');", true);
                            return;
                        }
                        BindHolidayData(fromdate.ToString("yyyy-MM-dd HH:mm:ss"), todate.ToString("yyyy-MM-dd HH:mm:ss"), reason, machine1, "ByDate");
                        break;
                    case "ByReason":
                        BindHolidayData(ddlFilterByReason.SelectedValue == "All" ? "" : ddlFilterByReason.SelectedValue.ToString(), "", "", "", "ByReason");
                        break;
                    case "ByMachine":
                        string machine = "";
                        foreach (ListItem item in ddlFilterByMachines.Items)
                        {
                            if (item.Selected)
                            {
                                if (machine == "")
                                {
                                    machine = "'" + item.Text + "'";
                                }
                                else
                                {
                                    machine += ",'" + item.Text + "'";
                                }

                            }
                        }
                        if (string.IsNullOrEmpty(machine))
                        {
                            List<HolidayListDetails> holidayListDetailsList = new List<HolidayListDetails>();
                            holidayListDetailsList.Add(new HolidayListDetails { Holiday = "", Reason = "", MachineID = "" });
                            gvHolidays.DataSource = holidayListDetailsList;
                            gvHolidays.DataBind();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select Machine');", true);
                            return;
                        }
                        BindHolidayData(machine, "", "", "", "ByMachine");
                        break;

                }

            }
            catch (Exception ex)
            {

            }
        }
        protected void btnDeleteRecordsYes_Click(object sender, EventArgs e)
        {
            try
            {
                string fltrDate = "", fltrReason = "";
                List<string> fltrMachine = new List<string>();
                Session["FltrDate"] = fltrDate;
                Session["FltrReason"] = fltrReason;
                List<HolidayListDetails> list = Session["HolidayList"] as List<HolidayListDetails>;
                //Session["FltrMachine"] = fltrMachine;
                string sqlQuery = "";
                if (Convert.ToBoolean(ViewState["SelectAll"]))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].CkdValue)
                        {
                            //string machine = (gvHolidays.Rows[i].FindControl("lblMachineId") as Label).Text;
                            string holiday = list[i].Date;
                            string machine = list[i].MachineID;
                            if (holiday != "")
                            {
                                DateTime date = DateTime.Now.Date;
                                date = Util.GetDateTime(holiday);
                                holiday = date.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            string ss = @"delete from HolidayList where  Holiday='" + holiday + "' and MachineID='" + machine + "'; delete from PlannedDownTimes where  StartTime='" + holiday + "' and Machine='" + machine + "' and DownType='Holiday';";
                            sqlQuery += ss;
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < gvHolidays.Rows.Count; i++)
                    {
                        if ((gvHolidays.Rows[i].FindControl("chkSelectHoliday") as CheckBox).Checked)
                        {
                            string holiday = (gvHolidays.Rows[i].FindControl("hdnDate") as HiddenField).Value;
                            string machine = (gvHolidays.Rows[i].FindControl("lblMachineId") as Label).Text;
                            if (holiday != "")
                            {
                                DateTime date = DateTime.Now.Date;
                                date = Util.GetDateTime(holiday);
                                holiday = date.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            string ss = @"delete from HolidayList where  Holiday='" + holiday + "' and MachineID='" + machine + "'; delete from PlannedDownTimes where  StartTime='" + holiday + "' and Machine='" + machine + "' and DownType='Holiday';";
                            sqlQuery += ss;
                        }

                    }
                }
                int success = DataBaseAccess.deleteHolidayDetails(sqlQuery);
                if (success <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "closeDeleteConfirmModal('holidayListConfirmModal');alert('Failed to delete Holiday List');", true);
                    return;
                }
                chkSelectAllHoliday.Checked = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "closeDeleteConfirmModal('holidayListConfirmModal');", true);
                BindHolidayFilterReason("deleteTheData");
                if (divHolidayFilter.Visible)
                {
                    btnApplyHoliday_Click(null, null);
                }
                else
                {
                    BindHolidayData("", "", "", "", "");
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string filterValue = ddlFilter.SelectedValue == null ? "" : ddlFilter.SelectedValue.ToString();
                holidayDateContainer.Visible = false;
                holidayReasonContainer.Visible = false;
                holidayMachineContainer.Visible = false;
                switch (filterValue)
                {
                    case "ByDate":
                        txtFilterByDateFrom.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
                        txtFilterByDateTo.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
                        holidayDateContainer.Visible = true;
                        holidayReasonContainer.Visible = true;
                        holidayMachineContainer.Visible = true; 
                        List<string> list= DataBaseAccess.getmachineidfromplant("");
                        list.Remove("All");
                        ddlFilterByMachines.DataSource = list;
                        ddlFilterByMachines.DataBind();
                        //lblDate.Visible = true;
                        //txtFilterByDate.Visible = true;
                        //lblReason.Visible = false;
                        //ddlFilterByReason.Visible = false;
                        break;
                    case "ByReason":
                        holidayReasonContainer.Visible = true;
                        //lblDate.Visible = false;
                        //txtFilterByDate.Visible = false;
                        //List<string> reasons = DataBaseAccess.getAllReasons();
                        //ddlFilterByReason.DataSource = reasons;
                        //ddlFilterByReason.DataBind();
                        //lblReason.Visible = true;
                        //ddlFilterByReason.Visible = true;
                        ////lblMachine.Visible = false;
                        ////ddlFilterByMachines.Visible = false;
                        break;
                    case "ByMachine":
                        holidayMachineContainer.Visible = true;
                        //lblDate.Visible = false;
                        //txtFilterByDate.Visible = false;
                        //lblReason.Visible = false;
                        //ddlFilterByReason.Visible = false;
                        List<string> list1 = DataBaseAccess.getmachineidfromplant("");
                        list1.Remove("All");
                        ddlFilterByMachines.DataSource = list1;
                        ddlFilterByMachines.DataBind();
                        //lblMachine.Visible = true;
                        //ddlFilterByMachines.Visible = true;
                        break;

                }

            }
            catch (Exception ex)
            {

            }
        }
        protected void gvHolidays_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvHolidays.PageIndex = e.NewPageIndex;
                if (Session["HolidayList"] != null)
                {
                    gvHolidays.DataSource = Session["HolidayList"] as List<HolidayListDetails>;
                    gvHolidays.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void gvHolidays_PreRender(object sender, EventArgs e)
        {
            try
            {
                GridView grid = (GridView)sender;
                if (grid != null)
                {
                    GridViewRow pagerRow = (GridViewRow)grid.BottomPagerRow;
                    if (pagerRow != null)
                    {
                        pagerRow.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        #endregion

        #region ------- Weeklyoff ---------------
        protected void btnWeeklyOff_Click(object sender, EventArgs e)
        {
            plantMachineFilterContainer.Style.Add("visibility", "visible");
            setActiveTab("weeklyoff");
            setWeeklyOffFilterData();
            txtFromWeeklyOffs.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
            txtToWeeklyOffs.Text = DateTime.Now.Date.AddDays(7).ToString("dd-MM-yyyy");
            divWeeklyFilter.Visible = false;
            chkSelectAllWeekOffs.Visible = false;
            List<PDTData> list = new List<PDTData>();
            list.Add(new PDTData { FromDateTime = "", Day = "", Reason = "", MachineID = "" });
            gvWeekOffs.DataSource = list;
            gvWeekOffs.DataBind();
            BindWeeklyReason("");
            txtWeeklyOffReason.Text = "";
        }
        private void BindWeeklyReason(string param)
        {
            try
            {
                List<string> list = DataBaseAccess.getDailyDownReason("WeeklyOff");


                if (param == "")
                {
                    ddlWeeklyOffFilterReason.DataSource = list;
                    ddlWeeklyOffFilterReason.DataBind();
                    ddlWeeklyOffFilterReason.Items.Insert(0, "All");
                }
                else if (param == "deleteTheData")
                {
                    string reason = ddlWeeklyOffFilterReason.Text;
                    int flag = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (reason == list[i])
                        {
                            flag = 1;
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        ddlWeeklyOffFilterReason.DataSource = list;
                        ddlWeeklyOffFilterReason.DataBind();
                        ddlWeeklyOffFilterReason.Items.Insert(0, "All");
                    }
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (ddlWeeklyOffFilterReason.Items.FindByValue(list[i]) == null)
                        {
                            ddlWeeklyOffFilterReason.Items.Add(list[i]);
                        }
                    }
                }

            }
            catch (Exception ex)
            { }
        }
        private void setWeeklyOffFilterData()
        {
            try
            {
                string plant = ddlPlantID.SelectedValue == null ? "" : ddlPlantID.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantID.SelectedItem.ToString();
                //string Cell = ddlCell.SelectedValue == null ? "" : ddlCell.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedItem.ToString();
                List<string> list = DataBaseAccess.getmachineidfromplant(plant);
                list.Remove("All");
                //ddlFilterWeekByMachine.DataSource = list;
                //ddlFilterWeekByMachine.DataBind();
                lbFilterWeekByMachine.DataSource = list;
                lbFilterWeekByMachine.DataBind();
                txtFromWeekOffs.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
                txtToWeekOffs.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
            }
            catch (Exception ex)
            { }
        }
        private void BindWeeklyOffData()
        {
            string fromDate = "", toDate = "", machineid = "", reason = "";
            string viewType = "";
            List<PDTData> list = new List<PDTData>();
            if (divWeeklyFilter.Visible)
            {
                viewType = ddlWeeklyOffFilterType.SelectedValue;
                if (ddlWeeklyOffFilterType.SelectedValue == "ByDate")
                {
                    DateTime fromdate = Util.GetDateTime(txtFromWeekOffs.Text.Trim());
                    fromDate = fromdate.ToString("yyyy-MM-dd");
                    DateTime todate = Util.GetDateTime(txtToWeekOffs.Text);
                    toDate = todate.ToString("yyyy-MM-dd");
                    foreach (ListItem item in lbFilterWeekByMachine.Items)
                    {
                        if (item.Selected)
                        {
                            machineid += "'" + item.Value + "',";
                        }
                    }
                    if (machineid.Length > 0)
                    {
                        machineid = machineid.Remove(machineid.Length - 1);
                    }
                    reason = ddlWeeklyOffFilterReason.SelectedValue == "All" ? "" : ddlWeeklyOffFilterReason.SelectedValue;
                    if (string.IsNullOrEmpty(machineid))
                    {
                        list.Add(new PDTData { FromDateTime = "", Day = "", Reason = "", MachineID = "" });
                        gvWeekOffs.DataSource = list;
                        gvWeekOffs.DataBind();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select Machine');", true);
                        return;
                    }
                }
                else if (ddlWeeklyOffFilterType.SelectedValue == "ByReason")
                {
                    reason = ddlWeeklyOffFilterReason.SelectedValue == "All" ? "" : ddlWeeklyOffFilterReason.SelectedValue;
                }
                else if (ddlWeeklyOffFilterType.SelectedValue == "ByMachine")
                {
                    foreach (ListItem item in lbFilterWeekByMachine.Items)
                    {
                        if (item.Selected)
                        {
                            machineid += "'" + item.Value + "',";
                        }
                    }
                    if (machineid.Length > 0)
                    {
                        machineid = machineid.Remove(machineid.Length - 1);
                    }

                    if (string.IsNullOrEmpty(machineid))
                    {
                        list.Add(new PDTData { FromDateTime = "", Day = "", Reason = "", MachineID = "" });
                        gvWeekOffs.DataSource = list;
                        gvWeekOffs.DataBind();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select Machine');", true);
                        return;
                    }

                }
            }
            list = DataBaseAccess.getWeeklyOffDetails(fromDate, toDate, machineid, reason, viewType);
            gvWeekOffs.DataSource = list;
            gvWeekOffs.DataBind();
            Session["PDTList"] = list;
            chkSelectAllWeekOffs.Checked = false;
            chkSelectAllWeekOffs.Visible = false;
            if (list.Count > 1)
                chkSelectAllWeekOffs.Visible = true;
            ViewState["SelectAll"] = false;

        }
        protected void ddlWeeklyOffFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                weeklyOffFilterDateContainer.Visible = false;
                weeklyOffFilterMachineContainer.Visible = false;
                weeklyOffFilterReasonContainer.Visible = false;
                if (ddlWeeklyOffFilterType.SelectedValue == "ByDate")
                {
                    weeklyOffFilterDateContainer.Visible = true;
                    weeklyOffFilterMachineContainer.Visible = true;
                    weeklyOffFilterReasonContainer.Visible = true;
                }
                else if (ddlWeeklyOffFilterType.SelectedValue == "ByReason")
                {
                    weeklyOffFilterReasonContainer.Visible = true;
                }
                else if (ddlWeeklyOffFilterType.SelectedValue == "ByMachine")
                {
                    weeklyOffFilterMachineContainer.Visible = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnSaveWeekOffs_Click(object sender, EventArgs e)
        {
            try
            {
                string reason = txtWeeklyOffReason.Text.Trim();
                if (reason == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "openWarningModal('Please enter Reason.')", true);
                    return;
                }
                DataTable dt = new DataTable();
                dt.Columns.Add("StartTime", typeof(DateTime));
                dt.Columns.Add("EndTime", typeof(DateTime));
                dt.Columns.Add("Machine", typeof(string));
                dt.Columns.Add("DownReason", typeof(string));
                dt.Columns.Add("DownType", typeof(string));
                dt.Columns.Add("DayName", typeof(string));
                dt.Columns.Add("ShiftName", typeof(string));
                dt.Columns.Add("ShiftStart", typeof(string));
                dt.Columns.Add("ShiftEnd", typeof(string));

                DateTime fromdate = Util.GetDateTime(txtFromWeeklyOffs.Text.Trim());
                DateTime todate = Util.GetDateTime(txtToWeeklyOffs.Text);
                DateTime incrementDate = fromdate;
                string selectedDays = "", selectedWeek = "";
                string mc = "";
                foreach (ListItem item in ddlMachineIDs.Items)
                {
                    if (item.Selected)
                    {
                        mc += item.Value + ",";
                    }
                }
                if (mc.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select Machine.');", true);
                    return;
                }
                foreach (ListItem item in ddlDays.Items)
                {
                    if (item.Selected)
                    {
                        selectedDays += item.Value.ToLower() + ",";
                    }
                }
                if (selectedDays.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select Days.');", true);
                    return;
                }
                foreach (ListItem item in ddlWeek.Items)
                {
                    if (item.Selected)
                    {
                        selectedWeek += item.Value + ",";
                    }
                }
                if (selectedWeek.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select Week.');", true);
                    return;
                }

                while (incrementDate <= todate)
                {
                    string incrementDateDay = incrementDate.ToString("ddd").ToLower();
                    if (selectedDays.IndexOf(incrementDateDay, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        //string incrementDateWeek = ((int)incrementDate.DayOfWeek).ToString();
                        string incrementDateWeek = GetWeekOfMonth(incrementDate).ToString();
                        if (selectedWeek.IndexOf(incrementDateWeek, StringComparison.OrdinalIgnoreCase) >= 0)
                        {

                            foreach (ListItem item in ddlMachineIDs.Items)
                            {
                                if (item.Selected)
                                {

                                    //PDTData data = new PDTData();
                                    //data.FromDateTime= VDGDataBaseAccess.GetLogicalDayStart(incrementDate.ToString("yyyy-MM-dd HH:mm:ss"));
                                    //data.ToDateTime = VDGDataBaseAccess.GetLogicalDayEnd(incrementDate.ToString("yyyy-MM-dd HH:mm:ss"));
                                    //data.Reason = "WeeklyOff";
                                    //data.MachineID = item.Value;
                                    var tblRow = dt.NewRow();
                                    tblRow[0] = Convert.ToDateTime(VDGDataBaseAccess.GetLogicalDayStart(incrementDate.ToString("yyyy-MM-dd HH:mm:ss")));
                                    tblRow[1] = Convert.ToDateTime(VDGDataBaseAccess.GetLogicalDayEnd(incrementDate.ToString("yyyy-MM-dd HH:mm:ss")));
                                    tblRow[2] = item.Value;
                                    tblRow[3] = reason.Trim();
                                    tblRow[4] = "WeeklyOff";
                                    tblRow[5] = incrementDate.ToString("dddd").ToLower();
                                    tblRow[6] = "";
                                    dt.Rows.Add(tblRow);
                                }
                            }
                        }
                    }
                    incrementDate = incrementDate.AddDays(1);
                }
                if (dt.Rows.Count > 0)
                {
                    DataBaseAccess.deletePORawDataRecords();
                    List<PDTData> rejectedWeeklyOffItems = new List<PDTData>();
                    rejectedWeeklyOffItems = DataBaseAccess.saveWeeklyOffDetails(dt, "PDTsave");

                    if (rejectedWeeklyOffItems.Count > 0)
                    {
                        if (dt.Rows.Count != rejectedWeeklyOffItems.Count)
                        {
                            txtWeeklyOffReason.Text = "";
                        }
                        Session["PDTRejectedItems"] = rejectedWeeklyOffItems;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "displayRejectedItems();", true);
                    }
                    else
                    {
                        txtWeeklyOffReason.Text = "";
                        Session["PDTRejectedItems"] = null;
                    }
                }
                BindWeeklyReason("appendTheData");
                BindWeeklyOffData();
            }
            catch (Exception ex) { }
        }
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        public static int GetWeekNumberOfMonth(DateTime date)
        {
            date = date.Date;
            DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
            DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            if (firstMonthMonday > date)
            {
                firstMonthDay = firstMonthDay.AddMonths(-1);
                firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            }
            return (date - firstMonthMonday).Days / 7 + 1;
        }
        public static int GetWeekOfMonth(DateTime date)
        {
            DateTime selectedDate = date;
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);
            DateTime beginningDate = beginningOfMonth;
            //while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
            while (date.Date.AddDays(1).DayOfWeek != DayOfWeek.Sunday)
            {

                date = date.AddDays(1);
            }
            int result, flag = 0;
            if ((beginningDate.Date.DayOfWeek != DayOfWeek.Sunday))
            {
                while ((beginningDate.Date.DayOfWeek != DayOfWeek.Sunday))
                {
                    if ((beginningDate.Date.DayOfWeek == selectedDate.DayOfWeek))
                    {
                        flag = 1;
                        break;
                    }
                    beginningDate = beginningDate.Date.AddDays(1);
                }
            }
            else
            {
                flag = 1;
            }
            if (flag == 0)
            {
                result = (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f);
            }
            else
            {
                result = (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
            }

            return result;
        }
        protected void btnApplyWeekOffsFilter_Click(object sender, EventArgs e)
        {
            BindWeeklyOffData();
        }
        protected void lnkWeeklyFilter_Click(object sender, EventArgs e)
        {
            try
            {
                divWeeklyFilter.Visible = true;
                ddlWeeklyOffFilterType.SelectedValue = "ByDate";
                ddlWeeklyOffFilterType_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            { }
        }
        protected void lnkWeeklyFilterClose_Click(object sender, EventArgs e)
        {
            //divWeeklyFilter.Visible = false;
            //BindWeeklyOffData();
            btnWeeklyOff_Click(null, null);
            chkSelectAllWeekOffs.Visible = false;
        }
        protected void lbtnReloadWeekOffs_Click(object sender, EventArgs e)
        {
            //chkSelectAllWeekOffs.Checked = false;
            //BindWeeklyOffData();
            btnWeeklyOff_Click(null, null);
        }

        protected void lbtnDeleteWeekOffs_Click(object sender, EventArgs e)
        {
            BindWeeklyOffData();
        }
        protected void btnDeleteWeeklyOffData_Click(object sender, EventArgs e)
        {
            try
            {
                string machine = "";
                string deletedid = "";
                List<PDTData> list = Session["PDTList"] as List<PDTData>;
                if (Convert.ToBoolean(ViewState["SelectAll"]))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].CkdValue)
                        {
                            string id = "";
                            id = list[i].ID;
                            deletedid += id + ",";
                            // int success = AdvikDatabaseAccess.deleteWeeklyOffDetails(id);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < gvWeekOffs.Rows.Count; i++)
                    {
                        if ((gvWeekOffs.Rows[i].FindControl("chkSelectWeekOffs") as CheckBox).Checked)
                        {
                            string id = "";
                            id = (gvWeekOffs.Rows[i].FindControl("hdnID") as HiddenField).Value;
                            deletedid += id + ",";
                            // int success = AdvikDatabaseAccess.deleteWeeklyOffDetails(id);
                        }
                    }
                }
                if (deletedid.Length > 0)
                {
                    deletedid = deletedid.Remove(deletedid.Length - 1);
                }
                int success = DataBaseAccess.deleteShiftDownDetails(deletedid,"", false);
                if (success <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "closeDeleteConfirmModal('weeklyOffDeleteConfirmModal');alert('Failed to delete Weekly Off List');", true);
                }
                //chkSelectAllWeekOffs.Checked = false;
                BindWeeklyReason("deleteTheData");
                BindWeeklyOffData();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "closeDeleteConfirmModal('weeklyOffDeleteConfirmModal');", true);
            }
            catch (Exception ex)
            {

            }
        }
        protected void gvWeekOffs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvWeekOffs.PageIndex = e.NewPageIndex;
                if (Session["PDTList"] != null)
                {
                    gvWeekOffs.DataSource = Session["PDTList"] as List<PDTData>;
                    gvWeekOffs.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void gvWeekOffs_PreRender(object sender, EventArgs e)
        {
            try
            {
                GridView grid = (GridView)sender;
                if (grid != null)
                {
                    GridViewRow pagerRow = (GridViewRow)grid.BottomPagerRow;
                    if (pagerRow != null)
                    {
                        pagerRow.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region ----- ShiftDown ----------
        protected void btnShiftDown_Click(object sender, EventArgs e)
        {
            plantMachineFilterContainer.Style.Add("visibility", "visible");
            setActiveTab("shiftdown");
            txtFromShiftDowns.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
            txtToShiftDowns.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
            txtShiftDownReason.Text = "";
            divShiftDownFilterContainer.Visible = false;
            chkAllShiftDown.Visible = false;
            txtFromShiftDownFilter.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
            txtToShiftDownFilter.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
            List<string> list= DataBaseAccess.getmachineidfromplant(ddlPlantID.SelectedValue);
            ddlShiftDownFilterMachines.DataSource = list;
            ddlShiftDownFilterMachines.DataBind();
            List<PDTData> shiftlist = new List<PDTData>();
            shiftlist.Add(new PDTData { CkdValue = false,MachineID = "",ShiftName="", Reason = "", FromDateTime = "",ID="" });
            gvMachineShift.DataSource = shiftlist;
            gvMachineShift.DataBind();
            BindShiftDownReason("");
        }
        private void BindShiftDownDetails()
        {
            try
            {
                string fromDate = "", toDate = "", machineid = "";
                string selectedFilterDay = "", selectedFilterShift = "";
                string reason = "", viewType = "";
                List<PDTData> list = new List<PDTData>();
                if (divShiftDownFilterContainer.Visible)
                {
                    viewType = ddlShiftFilterType.SelectedValue;
                    if (ddlShiftFilterType.SelectedValue == "ByDate")
                    {
                        DateTime fromdate = Util.GetDateTime(txtFromShiftDownFilter.Text.Trim());
                        fromDate = fromdate.ToString("yyyy-MM-dd");
                        DateTime todate = Util.GetDateTime(txtToShiftDownFilter.Text);
                        toDate = todate.ToString("yyyy-MM-dd");
                        foreach (ListItem item in lbShiftNameShiftDownFilter.Items)
                        {
                            if (item.Selected)
                            {
                                selectedFilterShift += "'" + item.Value + "',";
                            }
                        }
                        if (selectedFilterShift.Length > 0)
                        {
                            selectedFilterShift = selectedFilterShift.Remove(selectedFilterShift.Length - 1);
                        }
                        foreach (ListItem item in lbDayShiftDownFilter.Items)
                        {
                            if (item.Selected)
                            {
                                selectedFilterDay += "'" + item.Value.ToLower() + "',";
                            }
                        }
                        if (selectedFilterDay.Length > 0)
                        {
                            selectedFilterDay = selectedFilterDay.Remove(selectedFilterDay.Length - 1);
                        }
                        foreach (ListItem item in ddlShiftDownFilterMachines.Items)
                        {
                            if (item.Selected)
                            {
                                machineid += "'" + item.Value + "',";
                            }
                        }
                        if (machineid.Length > 0)
                        {
                            machineid = machineid.Remove(machineid.Length - 1);
                        }
                        reason = ddlShiftFilterReason.SelectedValue == "All" ? "" : ddlShiftFilterReason.SelectedValue;
                        if (string.IsNullOrEmpty(machineid))
                        {
                            list.Add(new PDTData { CkdValue = false, MachineID = "", ShiftName = "", Reason = "", FromDateTime = "", ID = "" });
                            gvMachineShift.DataSource = list;
                            gvMachineShift.DataBind();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select Machine');", true);
                            return;
                        }
                    }
                    else if (ddlShiftFilterType.SelectedValue == "ByReason")
                    {
                        reason = ddlShiftFilterReason.SelectedValue == "All" ? "" : ddlShiftFilterReason.SelectedValue;
                    }
                    else if (ddlShiftFilterType.SelectedValue == "ByMachine")
                    {
                        foreach (ListItem item in ddlShiftDownFilterMachines.Items)
                        {
                            if (item.Selected)
                            {
                                machineid += "'" + item.Value + "',";
                            }
                        }
                        if (machineid.Length > 0)
                        {
                            machineid = machineid.Remove(machineid.Length - 1);
                        }

                        if (string.IsNullOrEmpty(machineid))
                        {
                            list.Add(new PDTData { CkdValue = false, MachineID = "", ShiftName = "", Reason = "", FromDateTime = "", ID = "" });
                            gvMachineShift.DataSource = list;
                            gvMachineShift.DataBind();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select Machine');", true);
                            return;
                        }
                    }

                }

                Logger.WriteDebugLog("Before proc call" + DateTime.Now);
                list = DataBaseAccess.getShiftDownDetails(fromDate, toDate, machineid, selectedFilterDay, selectedFilterShift, reason, viewType);
                Logger.WriteDebugLog("After proc call" + DateTime.Now);
                gvMachineShift.DataSource = list;
                gvMachineShift.DataBind();
                Session["PDTList"] = list;
                Logger.WriteDebugLog("After bind" + DateTime.Now);
                chkAllShiftDown.Checked = false;
                chkAllShiftDown.Visible = false;
                if (list.Count > 1)
                    chkAllShiftDown.Visible = true;
                ViewState["SelectAll"] = false;
            }
            catch (Exception ex)
            { }
        }
        private void BindShiftDownReason(string param)
        {
            try
            {
                List<string> list = DataBaseAccess.getDailyDownReason("Shift");
                if (param == "")
                {
                    ddlShiftFilterReason.DataSource = list;
                    ddlShiftFilterReason.DataBind();
                    ddlShiftFilterReason.Items.Insert(0, "All");
                }
                else if (param == "deleteTheData")
                {
                    string reason = ddlShiftFilterReason.Text;
                    int flag = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (reason == list[i])
                        {
                            flag = 1;
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        ddlShiftFilterReason.DataSource = list;
                        ddlShiftFilterReason.DataBind();
                        ddlShiftFilterReason.Items.Insert(0, "All");
                    }
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (ddlShiftFilterReason.Items.FindByValue(list[i]) == null)
                        {
                            ddlShiftFilterReason.Items.Add(list[i]);
                        }
                    }
                }

            }
            catch (Exception ex)
            { }
        }
        protected void ddlShiftFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                shiftFilterDateTbl.Visible = false;
                shiftFilterDayTbl.Visible = false;
                shiftFilterMachineTbl.Visible = false;
                shiftFilterReasonTbl.Visible = false;
                if (ddlShiftFilterType.SelectedValue == "ByDate")
                {
                    shiftFilterDateTbl.Visible = true;
                    shiftFilterDayTbl.Visible = true;
                    shiftFilterMachineTbl.Visible = true;
                    shiftFilterReasonTbl.Visible = true;
                }
                else if (ddlShiftFilterType.SelectedValue == "ByReason")
                {
                    shiftFilterReasonTbl.Visible = true;
                }
                else if (ddlShiftFilterType.SelectedValue == "ByMachine")
                {
                    shiftFilterMachineTbl.Visible = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void lnkReloadShiftDown_Click(object sender, EventArgs e)
        {
            btnShiftDown_Click(null, null);
        }
        protected void lnkShiftDownFilter_Click(object sender, EventArgs e)
        {
            try
            {
                divShiftDownFilterContainer.Visible = true;
                ddlShiftFilterType.SelectedValue = "ByDate";
                ddlShiftFilterType_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {

            }
        }
        protected void lnkShiftDownFilterClose_Click(object sender, EventArgs e)
        {
            btnShiftDown_Click(null, null);
        }
        protected void btnApplyShiftDownFilter_Click(object sender, EventArgs e)
        {
            BindShiftDownDetails();
        }
        protected void btnSaveShiftDown_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("StartTime", typeof(DateTime));
                dt.Columns.Add("EndTime", typeof(DateTime));
                dt.Columns.Add("Machine", typeof(string));
                dt.Columns.Add("DownReason", typeof(string));
                dt.Columns.Add("DownType", typeof(string));
                dt.Columns.Add("DayName", typeof(string));
                dt.Columns.Add("ShiftName", typeof(string));
                dt.Columns.Add("ShiftStart", typeof(string));
                dt.Columns.Add("ShiftEnd", typeof(string));

                DateTime fromdate = Util.GetDateTime(txtFromShiftDowns.Text.Trim());
                DateTime todate = Util.GetDateTime(txtToShiftDowns.Text);
                DateTime incrementDate = fromdate;
                string selectedDays = "", selectedShift = "";
                string mc = "";
                foreach (ListItem item in ddlMachineIDs.Items)
                {
                    if (item.Selected)
                    {
                        mc += item.Value.ToLower() + ",";
                    }
                }
                if (mc.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select Machine.');", true);
                    return;
                }
                foreach (ListItem item in ddlOffDays.Items)
                {
                    if (item.Selected)
                    {
                        selectedDays += item.Value.ToLower() + ",";
                    }
                }
                if (selectedDays.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select Days.');", true);
                    return;
                }
                foreach (ListItem item in ddlshiftsDowns.Items)
                {
                    if (item.Selected)
                    {
                        selectedShift += item.Value + ",";
                    }
                }
                if (selectedShift.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select Shift.');", true);
                    return;
                }
                string shiftDownReason = txtShiftDownReason.Text.Trim();
                if (shiftDownReason == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please enter Down Reason.');", true);
                    return;
                }
                while (incrementDate <= todate)
                {
                    string incrementDateDay = incrementDate.ToString("ddd").ToLower();
                    if (selectedDays.IndexOf(incrementDateDay, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        //string incrementDateWeek = ((int)incrementDate.DayOfWeek).ToString();
                        foreach (ListItem shiftItem in ddlshiftsDowns.Items)
                        {
                            if (shiftItem.Selected)
                            {
                                List<PDTData> shiftDetails = new List<PDTData>();
                                shiftDetails = DataBaseAccess.getShiftTimeDetails(incrementDate.AddDays(1));
                                var shiftTimeDetails = shiftDetails.Where(s => s.ShiftName == shiftItem.Value).ToList();
                                if (shiftTimeDetails.Count > 0)
                                {
                                    string shiftStartTime = "", shiftEndTime = "";
                                    foreach (var sDetails in shiftTimeDetails)
                                    {
                                        shiftStartTime = sDetails.FromDateTime;
                                        shiftEndTime = sDetails.ToDateTime;
                                    }
                                    foreach (ListItem item in ddlMachineIDs.Items)
                                    {
                                        if (item.Selected)
                                        {
                                            var tblRow = dt.NewRow();
                                            tblRow[0] = Convert.ToDateTime(shiftStartTime).ToString("yyyy-MM-dd HH:mm:ss");
                                            tblRow[1] = Convert.ToDateTime(shiftEndTime).ToString("yyyy-MM-dd HH:mm:ss");
                                            tblRow[2] = item.Value;
                                            tblRow[3] = shiftDownReason;
                                            tblRow[4] = "Shift";
                                            tblRow[5] = incrementDate.ToString("dddd").ToLower();
                                            tblRow[6] = shiftItem.Value;
                                            dt.Rows.Add(tblRow);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    incrementDate = incrementDate.AddDays(1);
                }
                if (dt.Rows.Count > 0)
                {
                    DataBaseAccess.deletePORawDataRecords();
                    List<PDTData> rejectedShiftDownItems = new List<PDTData>();
                    rejectedShiftDownItems = DataBaseAccess.saveShiftDownDetails(dt, "PDTsave");

                    if (rejectedShiftDownItems.Count > 0)
                    {
                        if (rejectedShiftDownItems.Count != dt.Rows.Count)
                        {
                            txtShiftDownReason.Text = "";
                        }
                        Session["PDTRejectedItems"] = rejectedShiftDownItems;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "displayRejectedItems();", true);
                    }
                    else
                    {
                        txtShiftDownReason.Text = "";
                        Session["PDTRejectedItems"] = null;
                    }
                }
                BindShiftDownReason("appendTheData");
                BindShiftDownDetails();

            }
            catch (Exception ex)
            { }
        }
        protected void btnDeleteShiftDownData_Click(object sender, EventArgs e)
        {
            try
            {
                string machine = "";
                string deletedid = "";
                List<PDTData> list = Session["PDTList"] as List<PDTData>;
                if (Convert.ToBoolean(ViewState["SelectAll"]))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].CkdValue)
                        {
                            string id = "";
                            id = list[i].ID;
                            deletedid += id + ",";
                            // int success = AdvikDatabaseAccess.deleteWeeklyOffDetails(id);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < gvMachineShift.Rows.Count; i++)
                    {
                        if ((gvMachineShift.Rows[i].FindControl("chkSelectShiftDown") as CheckBox).Checked)
                        {
                            string id = (gvMachineShift.Rows[i].FindControl("hdnID") as HiddenField).Value;
                            deletedid += id + ",";
                        }
                    }
                }
                if (deletedid.Length > 0)
                {
                    deletedid = deletedid.Remove(deletedid.Length - 1);
                }
                int success = DataBaseAccess.deleteShiftDownDetails(deletedid,"", false);
                if (success <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "closeDeleteConfirmModal('shiftDownDeleteConfirmModal');alert('Failed to delete Shift Down List');", true);

                }
                //chkSelectAllWeekOffs.Checked = false;
                BindShiftDownReason("deleteTheData");
                BindShiftDownDetails();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage1", "closeDeleteConfirmModal('shiftDownDeleteConfirmModal');", true);
            }
            catch (Exception ex)
            {

            }
        }
        protected void gvMachineShift_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvMachineShift.PageIndex = e.NewPageIndex;
                if (Session["PDTList"] != null)
                {
                    gvMachineShift.DataSource = Session["PDTList"] as List<PDTData>;
                    gvMachineShift.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void gvMachineShift_PreRender(object sender, EventArgs e)
        {
            try
            {
                GridView grid = (GridView)sender;
                if (grid != null)
                {
                    GridViewRow pagerRow = (GridViewRow)grid.BottomPagerRow;
                    if (pagerRow != null)
                    {
                        pagerRow.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region ----All Down -------
        protected void btnAllDown_Click(object sender, EventArgs e)
        {
            string plant = "";
            try
            {
                setActiveTab("alldown");
                txtFromAllDowns.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
                txtToAllDowns.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
                //ddlPlantAllDown.DataSource = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllPlants();
                //ddlPlantAllDown.DataBind();
                //string plant = ddlPlantAllDown.SelectedValue == null ? "" : ddlPlantAllDown.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantAllDown.SelectedItem.ToString();
                //List<string> list = DataBaseAccess.GetAllMachines("");
                //if (list.Count > 0)
                //    list.Add("All");
                //ddlMachineAllDown.DataSource = list;
                //ddlMachineAllDown.DataBind();
                BindMachineIDs();
                BindAllDownData();
            }
            catch (Exception ex)
            { }
        }
        private void BindAllDownData()
        {
            try
            {
                plantMachineFilterContainer.Style.Add("visibility", "visible");
                DateTime fromdate = Util.GetDateTime(txtFromAllDowns.Text.Trim());
                DateTime todate = Util.GetDateTime(txtToAllDowns.Text.Trim());
                string machineid = ddlMachineAllDown.SelectedValue;
                string downtype = ddlAllDowns.SelectedValue;
                List<PDTData> list = DataBaseAccess.getAllDownDetails(fromdate, todate, machineid, downtype);
                gvAllDowns.DataSource = list;
                gvAllDowns.DataBind();
                chkSelectAllDowns.Checked = false;
                Session["PDTList"] = list;
                ViewState["SelectAll"] = false;
            }
            catch (Exception ex) { }
        }
        protected void ddlPlantAllDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string plant = ddlPlantAllDown.SelectedValue == null ? "" : ddlPlantAllDown.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantAllDown.SelectedItem.ToString();
                List<string> list = DataBaseAccess.getmachineidfromplant(plant);
                ddlMachineAllDown.DataSource = list;
                ddlMachineAllDown.DataBind();
            }
            catch (Exception ex) { }
        }
        protected void lnkAllDownReload_Click(object sender, EventArgs e)
        {
            btnAllDown_Click(null, null);
        }
        protected void btnDeleteAllDown_Click(object sender, EventArgs e)
        {
            try
            {
                string machine = "";
                string deletedid = "";
                bool isHolidayData = false;
                List<PDTData> list = Session["PDTList"] as List<PDTData>;
                if (Convert.ToBoolean(ViewState["SelectAll"]))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].CkdValue)
                        {
                            string id = "";
                            id = list[i].ID;
                            if (list[i].DownType == "Holidays")
                            {
                                deletedid += "'" + id + "',";
                                isHolidayData = true;
                                machine = list[i].MachineID;
                            }
                            else
                            {
                                deletedid += id + ",";
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < gvAllDowns.Rows.Count; i++)
                    {
                        if ((gvAllDowns.Rows[i].FindControl("chkSelectDowns") as CheckBox).Checked)
                        {
                            string id = (gvAllDowns.Rows[i].FindControl("hdnID") as HiddenField).Value;
                            if ((gvAllDowns.Rows[i].FindControl("lblAllDowns") as Label).Text == "Holidays")
                            {
                                deletedid += "'" + id + "',";
                                isHolidayData = true;
                                machine = (gvAllDowns.Rows[i].FindControl("lblAllMachineId") as Label).Text;
                            }
                            else
                            {
                                deletedid += id + ",";
                            }
                        }
                    }
                }
                if (deletedid.Length > 0)
                {
                    deletedid = deletedid.Remove(deletedid.Length - 1);
                }
                int success = DataBaseAccess.deleteShiftDownDetails(deletedid, machine, isHolidayData);
                if (success <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "closeDeleteConfirmModal('allDownDeleteConfirmModal');alert('Failed to delete Shift Down List');", true);

                }
                //chkSelectAllWeekOffs.Checked = false;
                BindAllDownData();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage1", "closeDeleteConfirmModal('allDownDeleteConfirmModal');", true);
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnViewAllDowns_Click(object sender, EventArgs e)
        {
            BindAllDownData();
        }
        #endregion

        #region ---DailyDown----
        private void BindDailyDownReason(string param)
        {
            try
            {

                List<string> list = DataBaseAccess.getDailyDownReason("DailyDown");
                if (param == "")
                {
                    ddlDailyDownsByReason.DataSource = list;
                    ddlDailyDownsByReason.DataBind();
                    ddlDailyDownsByReason.Items.Insert(0, "All");
                }
                else if (param == "deleteTheData")
                {
                    string reason = ddlDailyDownsByReason.Text;
                    int flag = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (reason == list[i])
                        {
                            flag = 1;
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        ddlDailyDownsByReason.DataSource = list;
                        ddlDailyDownsByReason.DataBind();
                        ddlDailyDownsByReason.Items.Insert(0, "All");
                    }
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (ddlDailyDownsByReason.Items.FindByValue(list[i]) == null)
                        {
                            ddlDailyDownsByReason.Items.Add(list[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
       
        protected void btnDailyDown_Click(object sender, EventArgs e)
        {
            try
            {
               // plantMachineFilterContainer.Style.Add("visibility", "visible");
                setActiveTab("dailydown");
                getShiftDetails();
                txtFilterFromDailyDowns.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
                txtFilterToDailyDowns.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
                txtFromDailyDowns.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDailyDowns.Text = DateTime.Now.ToString("dd-MM-yyyy");
                string shiftStartTime, shiftEndTime, shiftname;
                shiftStartTime = DataBaseAccessPTA.CurrentStartEndTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), out shiftEndTime, out shiftname);
                txtFromTimeDailydown.Text = Util.GetDateTime(shiftStartTime).ToString("HH:mm:ss");
                txtToTimeDailydown.Text = Util.GetDateTime(shiftEndTime).ToString("HH:mm:ss");
                List<string> list = DataBaseAccess.getmachineidfromplant("");
                list.Remove("All");
                ddlDailyDownFilterMachines.DataSource = list;
                ddlDailyDownFilterMachines.DataBind();
                chkSelectAllDailyDowns.Visible = false;
                divDailyDownFilter.Visible = false;
                BindDailyDownReason("");
                txtDailyDownReason.Text = "";
                List<PDTData> Ddlist = new List<PDTData>();
                Ddlist.Add(new PDTData { FromDateTime = "",ToDateTime="",Reason = "", MachineID = "",ID="",CkdValue=false});
                gvDailyDowns.DataSource = Ddlist;
                gvDailyDowns.DataBind();
            }
            catch (Exception ex)
            { }
        }
        private void getShiftDetails()
        {
            try
            {
                List<PDTData> shiftDetails = new List<PDTData>();
                shiftDetails = DataBaseAccess.getShiftTimeDetails(DateTime.Now);
                Session["ShiftDetails"] = shiftDetails;
            }
            catch (Exception ex)
            {

            }
        }
        protected void ddlDailyDownFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dailyDownFilterDateTbl.Visible = false;
                dailyDownFilterMachineTbl.Visible = false;
                dailyDownFilterReasonTbl.Visible = false;
                if (ddlDailyDownFilterType.SelectedValue == "ByDate")
                {
                    dailyDownFilterDateTbl.Visible = true;
                    dailyDownFilterMachineTbl.Visible = true;
                    dailyDownFilterReasonTbl.Visible = true;
                }
                else if (ddlDailyDownFilterType.SelectedValue == "ByReason")
                {
                    dailyDownFilterReasonTbl.Visible = true;
                }
                else if (ddlDailyDownFilterType.SelectedValue == "ByMachine")
                {
                    dailyDownFilterMachineTbl.Visible = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void BindDailyDownData()
        {
            string fromDate = "", toDate = "", machineid = "";
            string downReason = "";
            string viewType = "";
            List<PDTData> list = new List<PDTData>();
            if (divDailyDownFilter.Visible)
            {
                viewType = ddlDailyDownFilterType.SelectedValue;
                if (ddlDailyDownFilterType.SelectedValue == "ByDate")
                {
                    fromDate = txtFilterFromDailyDowns.Text;
                    toDate = txtFilterToDailyDowns.Text;
                    downReason = ddlDailyDownsByReason.SelectedValue == "All" ? "" : ddlDailyDownsByReason.SelectedValue;
                    foreach (ListItem item in ddlDailyDownFilterMachines.Items)
                    {
                        if (item.Selected)
                        {
                            machineid += "'" + item.Value + "',";
                        }
                    }
                    if (machineid.Length > 0)
                    {
                        machineid = machineid.Remove(machineid.Length - 1);
                    }
                    if (string.IsNullOrEmpty(machineid))
                    {
                        list.Add(new PDTData { FromDateTime = "", ToDateTime = "", Reason = "", MachineID = "", ID = "", CkdValue = false });
                        gvDailyDowns.DataSource = list;
                        gvDailyDowns.DataBind();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select Machine');", true);
                        return;
                    }
                }
                else if (ddlDailyDownFilterType.SelectedValue == "ByReason")
                {
                    downReason = ddlDailyDownsByReason.SelectedValue == "All" ? "" : ddlDailyDownsByReason.SelectedValue;
                }
                else if (ddlDailyDownFilterType.SelectedValue == "ByMachine")
                {
                    foreach (ListItem item in ddlDailyDownFilterMachines.Items)
                    {
                        if (item.Selected)
                        {
                            machineid += "'" + item.Value + "',";
                        }
                    }
                    if (machineid.Length > 0)
                    {
                        machineid = machineid.Remove(machineid.Length - 1);
                    }

                    if (string.IsNullOrEmpty(machineid))
                    {
                        list.Add(new PDTData { FromDateTime = "", ToDateTime = "", Reason = "", MachineID = "", ID = "", CkdValue = false });
                        gvDailyDowns.DataSource = list;
                        gvDailyDowns.DataBind();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select Machine');", true);
                        return;
                    }
                }
            }           
            list = DataBaseAccess.getDailyDownDetails(fromDate, toDate, downReason, machineid, viewType);
            gvDailyDowns.DataSource = list;
            gvDailyDowns.DataBind();
            chkSelectAllDailyDowns.Checked = false;
            chkSelectAllDailyDowns.Visible = false;
            Session["PDTList"] = list;
            if (list.Count > 0)
                chkSelectAllDailyDowns.Visible = true;
            ViewState["SelectAll"] = false;
        }
        //private void BindDailyDownFilterMachineid()
        //{
        //    try
        //    {
        //        List<string> list = DataBaseAccess.getmachineidfromplant("");
        //        list.Remove("All");
        //        ddlDailyDownFilterMachines.DataSource = list;
        //        ddlDailyDownFilterMachines.DataBind();
        //    }
        //    catch (Exception ex) { }
        //}
        protected void lnkDailyDownFilter_Click(object sender, EventArgs e)
        {
            try
            {
                divDailyDownFilter.Visible = true;
                ddlDailyDownFilterType.SelectedValue = "ByDate";
                ddlDailyDownFilterType_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            { }
        }

        protected void lnkDailyDownsReload_Click(object sender, EventArgs e)
        {
            try
            {
                //chkSelectAllDailyDowns.Checked = false;
                //BindDailyDownData();
                btnDailyDown_Click(null, null);
            }
            catch (Exception ex)
            { }
        }

        protected void lnkDailyDownFilterClose_Click(object sender, EventArgs e)
        {
            // divDailyDownFilter.Visible = false;
            btnDailyDown_Click(null, null);
        }

        protected void btnDailyDownFilterAppy_Click(object sender, EventArgs e)
        {
            BindDailyDownData();
        }
        protected void btnDeleteDailyDownData_Click(object sender, EventArgs e)
        {
            try
            {
                string machine = "";
                string deletedid = "";
                List<PDTData> list = Session["PDTList"] as List<PDTData>;
                if (Convert.ToBoolean(ViewState["SelectAll"]))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].CkdValue)
                        {
                            string id = "";
                            id = list[i].ID;
                            deletedid += id + ",";
                            // int success = AdvikDatabaseAccess.deleteWeeklyOffDetails(id);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < gvDailyDowns.Rows.Count; i++)
                    {
                        if ((gvDailyDowns.Rows[i].FindControl("chkSelectDailyDowns") as CheckBox).Checked)
                        {
                            string id = (gvDailyDowns.Rows[i].FindControl("hdnID") as HiddenField).Value;
                            deletedid += id + ",";
                        }
                    }
                }
                if (deletedid.Length > 0)
                {
                    deletedid = deletedid.Remove(deletedid.Length - 1);
                }
                int success = DataBaseAccess.deleteShiftDownDetails(deletedid, "", false);
                if (success <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "closeDeleteConfirmModal('dailyDownDeleteConfirmModal');alert('Failed to delete Daily Down List');", true);

                }
                //chkSelectAllWeekOffs.Checked = false;
                BindDailyDownReason("deleteTheData");
                BindDailyDownData();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage1", "closeDeleteConfirmModal('dailyDownDeleteConfirmModal');", true);

            }
            catch (Exception ex)
            { }
        }
        protected void btnDailyDownSave_Click(object sender, EventArgs e)
        {
            try
            {
                string machineid = "";
                if (txtFromDailyDowns.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select From Date.');", true);
                    return;
                }
                if (txtToDailyDowns.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select To Date.');", true);
                    return;
                }
                if (txtFromTimeDailydown.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select From Time.');", true);
                    return;
                }
                if (txtToTimeDailydown.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select To Time.');", true);
                    return;
                }
                if (txtDailyDownReason.Text.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please enter Reason.');", true);
                    return;
                }
                if (Session["ShiftDetails"] == null)
                {
                    getShiftDetails();
                }
                foreach (ListItem item in ddlMachineIDs.Items)
                {
                    if (item.Selected)
                    {
                        machineid += "'" + item.Value + "',";
                    }
                }
                if (machineid.Length > 0)
                {
                    machineid = machineid.Remove(machineid.Length - 1);
                }
                if (machineid == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Please select Machine.');", true);
                    return;
                }
                DateTime startTime = Util.GetDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + txtFromTimeDailydown.Text);
                DateTime endTime = Util.GetDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + txtToTimeDailydown.Text);
                List<PDTData> shiftDetails = (List<PDTData>)Session["ShiftDetails"];
                bool isShiftInNextDay = true;
                DateTime nextDayshiftStartTime = DateTime.Now;
                if (shiftDetails.Count > 0)
                {
                    for (int i = 0; i < shiftDetails.Count; i++)
                    {
                        DateTime shiftStartTime = Util.GetDateTime(shiftDetails[i].FromDateTime);
                        DateTime shiftEndTime = Util.GetDateTime(shiftDetails[i].ToDateTime);
                        if (startTime.TimeOfDay >= shiftStartTime.TimeOfDay && startTime.TimeOfDay < shiftEndTime.TimeOfDay)
                        {
                            isShiftInNextDay = false;
                            if (endTime.TimeOfDay > shiftEndTime.TimeOfDay || endTime.TimeOfDay < shiftStartTime.TimeOfDay)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Time should not cross the shift time.');", true);
                                return;
                            }
                            break;
                        }
                    }
                    if (isShiftInNextDay)
                    {
                        for (int i = 0; i < shiftDetails.Count; i++)
                        {
                            DateTime shiftStartTime = Util.GetDateTime(shiftDetails[i].FromDateTime);
                            DateTime shiftEndTime = Util.GetDateTime(shiftDetails[i].ToDateTime);
                            nextDayshiftStartTime = shiftStartTime;
                            //if ((!(startTime.TimeOfDay >= shiftStartTime.TimeOfDay && startTime.TimeOfDay < shiftEndTime.TimeOfDay)) && ((startTime.TimeOfDay > shiftEndTime.TimeOfDay && startTime.TimeOfDay >= shiftStartTime.TimeOfDay) || (startTime.TimeOfDay < shiftEndTime.TimeOfDay && startTime.TimeOfDay <= shiftStartTime.TimeOfDay)))
                            if (shiftStartTime.TimeOfDay >= shiftEndTime.TimeOfDay)
                            {
                                if (!(endTime.TimeOfDay <= shiftEndTime.TimeOfDay || endTime.TimeOfDay >= shiftStartTime.TimeOfDay))
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "warningMessage", "openWarningModal('Time should not cross the shift time.');", true);
                                    return;
                                }
                                break;
                            }
                        }
                    }

                }
                DataTable dt = new DataTable();
                dt.Columns.Add("StartTime", typeof(DateTime));
                dt.Columns.Add("EndTime", typeof(DateTime));
                dt.Columns.Add("Machine", typeof(string));
                dt.Columns.Add("DownReason", typeof(string));
                dt.Columns.Add("DownType", typeof(string));
                dt.Columns.Add("DayName", typeof(string));
                dt.Columns.Add("ShiftName", typeof(string));
                dt.Columns.Add("ShiftStart", typeof(string));
                dt.Columns.Add("ShiftEnd", typeof(string));

                string selectedshidtStart, selectedshidtEnd, shiftname;
                selectedshidtStart = DataBaseAccessPTA.CurrentStartEndTime(startTime.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss"), out selectedshidtEnd, out shiftname);

                DateTime fromdate = Util.GetDateTime(txtFromDailyDowns.Text);
                DateTime todate = Util.GetDateTime(txtToDailyDowns.Text);
                DateTime incrementDate = fromdate;
                while (incrementDate <= todate)
                {
                    foreach (ListItem item in ddlMachineIDs.Items)
                    {
                        if (item.Selected)
                        {
                            var tblRow = dt.NewRow();

                            if (isShiftInNextDay)
                            {
                                //if (endTime.TimeOfDay > Util.GetDateTime("01-01-2020 23:59:59").TimeOfDay)
                                if (endTime.TimeOfDay <= Util.GetDateTime(nextDayshiftStartTime.ToString("yyyy-MM-dd HH:mm:ss")).TimeOfDay)
                                {
                                    DateTime incrementDateToNextShift = incrementDate.AddDays(1);
                                    tblRow[1] = incrementDateToNextShift.ToString("yyyy-MM-dd") + " " + endTime.ToString("HH:mm:ss");
                                    tblRow[8] = incrementDateToNextShift.ToString("yyyy-MM-dd") + " " + Util.GetDateTime(selectedshidtEnd).ToString("HH:mm:ss");
                                }
                                else
                                {
                                    tblRow[1] = incrementDate.ToString("yyyy-MM-dd") + " " + endTime.ToString("HH:mm:ss");
                                    tblRow[8] = incrementDate.ToString("yyyy-MM-dd") + " " + Util.GetDateTime(selectedshidtEnd).ToString("HH:mm:ss");
                                }
                                //if (startTime.TimeOfDay > Util.GetDateTime("01-01-2020 23:59:59").TimeOfDay)
                                if (startTime.TimeOfDay < Util.GetDateTime(nextDayshiftStartTime.ToString("yyyy-MM-dd HH:mm:ss")).TimeOfDay)
                                {
                                    DateTime incrementDateToNextShift = incrementDate.AddDays(1);
                                    tblRow[0] = incrementDateToNextShift.ToString("yyyy-MM-dd") + " " + startTime.ToString("HH:mm:ss");
                                    tblRow[7] = incrementDate.ToString("yyyy-MM-dd") + " " + Util.GetDateTime(selectedshidtStart).ToString("HH:mm:ss");
                                }
                                else
                                {
                                    tblRow[0] = incrementDate.ToString("yyyy-MM-dd") + " " + startTime.ToString("HH:mm:ss");
                                    tblRow[7] = incrementDate.ToString("yyyy-MM-dd") + " " + Util.GetDateTime(selectedshidtStart).ToString("HH:mm:ss");
                                }
                            }
                            else
                            {
                                tblRow[1] = incrementDate.ToString("yyyy-MM-dd") + " " + endTime.ToString("HH:mm:ss");
                                tblRow[0] = incrementDate.ToString("yyyy-MM-dd") + " " + startTime.ToString("HH:mm:ss");
                                tblRow[7] = incrementDate.ToString("yyyy-MM-dd") + " " + Util.GetDateTime(selectedshidtStart).ToString("HH:mm:ss");
                                tblRow[8] = incrementDate.ToString("yyyy-MM-dd") + " " + Util.GetDateTime(selectedshidtEnd).ToString("HH:mm:ss");
                            }
                            tblRow[2] = item.Value;
                            tblRow[3] = txtDailyDownReason.Text.Trim();
                            tblRow[4] = "DailyDown";
                            tblRow[5] = incrementDate.ToString("dddd");
                            tblRow[6] = shiftname;
                            dt.Rows.Add(tblRow);
                        }
                    }
                    incrementDate = incrementDate.AddDays(1);
                }
                if (dt.Rows.Count > 0)
                {
                    DataBaseAccess.deletePORawDataRecords();
                    List<PDTData> rejectedShiftDownItems = new List<PDTData>();
                    rejectedShiftDownItems = DataBaseAccess.saveShiftDownDetails(dt, "PDTsave");

                    if (rejectedShiftDownItems.Count > 0)
                    {
                        if (rejectedShiftDownItems.Count != dt.Rows.Count)
                        {
                            txtDailyDownReason.Text = "";
                        }
                        Session["PDTRejectedItems"] = rejectedShiftDownItems;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "displayRejectedItems();", true);
                    }
                    else
                    {
                        txtDailyDownReason.Text = "";
                        Session["PDTRejectedItems"] = null;
                    }
                }
                BindDailyDownReason("appendTheData");
                BindDailyDownData();
            }
            catch (Exception ex) { }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            //if (hdnActiveTab.Value == "weeklyoff")
            //{
            //    btnWeeklyOff_Click(null, null);
            //}
            //else if (hdnActiveTab.Value == "dailydown")
            //{

            //}
            //else if (hdnActiveTab.Value == "shiftdown")
            //{
            //    btnShiftDown_Click(null, null);
            //}
            //else if (hdnActiveTab.Value == "alldown")
            //{

            //}
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<PDTData> getPDTRejectedData()
        {
            List<PDTData> finallist = new List<PDTData>();
            if (HttpContext.Current.Session["PDTRejectedItems"] != null)
            {
                finallist = (List<PDTData>)HttpContext.Current.Session["PDTRejectedItems"];
            }
            return finallist;
        }

        protected void gvDailyDowns_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDailyDowns.PageIndex = e.NewPageIndex;
                if (Session["PDTList"] != null)
                {
                    gvDailyDowns.DataSource = Session["PDTList"] as List<PDTData>;
                    gvDailyDowns.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void gvDailyDowns_PreRender(object sender, EventArgs e)
        {
            try
            {
                GridView grid = (GridView)sender;
                if (grid != null)
                {
                    GridViewRow pagerRow = (GridViewRow)grid.BottomPagerRow;
                    if (pagerRow != null)
                    {
                        pagerRow.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        #endregion
        protected void gvAllDowns_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAllDowns.PageIndex = e.NewPageIndex;
                if (Session["PDTList"] != null)
                {
                    gvAllDowns.DataSource = Session["PDTList"] as List<PDTData>;
                    gvAllDowns.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void gvAllDowns_PreRender(object sender, EventArgs e)
        {
            try
            {
                GridView grid = (GridView)sender;
                if (grid != null)
                {
                    GridViewRow pagerRow = (GridViewRow)grid.BottomPagerRow;
                    if (pagerRow != null)
                    {
                        pagerRow.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        protected void ddlShiftType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindMachineIDs();
                BindShift();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ViewState["SelectAll"] = false;
                CheckBox chk = (CheckBox)sender;
                string name = chk.ID;
                switch(name)
                {
                    case "chkSelectAllHoliday":
                        List<HolidayListDetails> list = Session["HolidayList"] as List<HolidayListDetails>;
                        if (chk.Checked)
                        {
                            foreach (HolidayListDetails item in list)
                            {
                                item.CkdValue = true;
                            }
                            ViewState["SelectAll"] = true;
                        }
                        else
                        {
                            foreach (HolidayListDetails item in list)
                            {
                                item.CkdValue = false;
                            }
                        }
                        Session["HolidayList"] = list;
                        if (Session["HolidayList"] != null)
                        {
                            gvHolidays.DataSource = Session["HolidayList"] as List<HolidayListDetails>;
                            gvHolidays.DataBind();
                        }
                        break;
                    case "chkSelectAllWeekOffs":
                        BindCheckedData(chk.Checked);
                        if (Session["PDTList"] != null)
                        {
                            gvWeekOffs.DataSource = Session["PDTList"] as List<PDTData>;
                            gvWeekOffs.DataBind();
                        }
                        break;
                    case "chkSelectAllDailyDowns":
                        BindCheckedData(chk.Checked);
                        if (Session["PDTList"] != null)
                        {
                            gvDailyDowns.DataSource = Session["PDTList"] as List<PDTData>;
                            gvDailyDowns.DataBind();
                        }
                        break;
                    case "chkAllShiftDown":
                        BindCheckedData(chk.Checked);
                        if (Session["PDTList"] != null)
                        {
                            gvMachineShift.DataSource = Session["PDTList"] as List<PDTData>;
                            gvMachineShift.DataBind();
                        }
                        break;
                    case "chkSelectAllDowns":
                        BindCheckedData(chk.Checked);
                        if (Session["PDTList"] != null)
                        {
                            gvAllDowns.DataSource = Session["PDTList"] as List<PDTData>;
                            gvAllDowns.DataBind();
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindCheckedData(bool Val)
        {
            try
            {
                List<PDTData> list = Session["PDTList"] as List<PDTData>;
                if (Val)
                {
                    ViewState["SelectAll"] = true;
                    foreach (PDTData item in list)
                    {
                        item.CkdValue = true;
                    }
                }
                else
                {
                    foreach (PDTData item in list)
                    {
                        item.CkdValue = false;
                    }
                }
                Session["PDTList"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
    }
}