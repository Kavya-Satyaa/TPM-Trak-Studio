using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik.DataBaseAccess;
using Web_TPMTrakDashboard.Advik.Models;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Advik
{
    public partial class HolidayList : System.Web.UI.Page
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
                // BindPlantID();
                //BindMachineIDs();
                // ddlPlantID_SelectedIndexChanged(null, null);
                //List<string> shifts = new List<string>() { "Shift-A", "Shift-B", "Shift-C" };
                //ddlshiftsDowns.DataSource = shifts;
                //ddlshiftsDowns.DataBind();
                //loadGvMachineShift();
                BindHolidaysDropDowns();

                btnHodiday_Click(null, null);



            }
            // loadGvMachineShift();
        }
        private void BindPlantID()
        {
            try
            {
                //ddlPlantID.DataSource = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllPlants();
                //ddlPlantID.DataBind();
                ////ddlPlantId.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommanResource", "PlantAll").ToString(), "All"));
                ////ddlPlantId_SelectedIndexChanged(null, null);
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
                //string plant = ddlPlantID.SelectedValue == null ? "" : ddlPlantID.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantID.SelectedItem.ToString();
                ////string Cell = ddlCell.SelectedValue == null ? "" : ddlCell.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedItem.ToString();
                //ddlMachineIDs.DataSource = AdvikDatabaseAccess.getMachineForPlant(plant);
                //// ddlMachineIDs.DataSource = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllMacForPlant(plant);
                //ddlMachineIDs.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                // lblMessages.ForeColor = System.Drawing.Color.Red;
                //lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }

        }
        private void loadGvMachineShift()
        {
            //try
            //{
            //    ClearDataGridColumns();
            //    DataTable dtMachinesShift = GetDtMachineShift();
            //    if (dtMachinesShift != null)
            //    {
            //        foreach (DataColumn dataColumn in dtMachinesShift.Columns)
            //        {
            //            if (dataColumn.ColumnName.Equals("MachineID", StringComparison.OrdinalIgnoreCase))
            //            {
            //                BoundField boundField = new BoundField();
            //                boundField.HeaderText = dataColumn.ColumnName;
            //                boundField.DataField = dataColumn.ColumnName;
            //                gvMachineShift.Columns.Add(boundField);
            //            }
            //            else
            //            {
            //                string col = dataColumn.ColumnName;
            //                col = col.Substring(col.Length - 1);
            //                CheckBoxField checkBoxField = new CheckBoxField();
            //                checkBoxField.HeaderText = col;
            //                checkBoxField.DataField = dataColumn.ColumnName;
            //                gvMachineShift.Columns.Add(checkBoxField);
            //            }
            //        }
            //        gvMachineShift.DataSource = dtMachinesShift;
            //        gvMachineShift.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Exps", "alert('Alert:" + ex.Message + "')", true);
            //}

        }

        private DataTable GetDtMachineShift()
        {
            DataTable dataTable = new DataTable();
            //string query = "select * from MachineShiftDowns";
            //try
            //{
            //    sqlCommand = new SqlCommand(query, sqlConnection);
            //    sqlCommand.CommandType = CommandType.Text;
            //    sqlConnection.Open();
            //    dataTable.Load(sqlCommand.ExecuteReader());
            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Exp", "alert('Alert:" + ex.Message + "')", true);
            //}
            //finally
            //{

            //    sqlCommand.Dispose();
            //    if (sqlConnection != null) sqlConnection.Close();

            //}
            return dataTable;
        }

        private void ClearDataGridColumns()
        {
            //    if (gvMachineShift.Columns.Count > 0)
            //    {
            //        for (int i = gvMachineShift.Columns.Count - 1; i >= 0; i--)
            //        {
            //            gvMachineShift.Columns.RemoveAt(i);
            //        }
            //    }
        }

        private void BindHolidaysDropDowns()
        {
            try
            {


                for (int yr = DateTime.Now.Year; yr <= DateTime.Now.Year + 100; yr++)
                {
                    ddlFromYear.Items.Add(yr.ToString());
                    ddlToYear.Items.Add(yr.ToString());
                }
                var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                for (int i = 0; i < months.Length - 1; i++)
                {
                    ddlMonth.Items.Add(new ListItem(months[i], i.ToString()));
                }
                for (int i = 1; i <= 31; i++)
                {
                    ddlDay.Items.Add(i.ToString());
                }
                //List<string> plants = new List<string> { "All", "plant01", "plant02", "plant03", "plant04", "plant05", "plant06" };
                //List<string> machines = new List<string> { "All", "machine01", "machine02", "machine03", "machine04", "machine05", "machine06" };
                //ddlPlantAllDown.DataSource = plants;
                //ddlPlantAllDown.DataBind();

                //ddlMachineAllDown.DataSource = machines;
                //ddlMachineAllDown.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        private void BindAllDownDropDowns()
        {
            //List<string> plants = new List<string> { "All", "plant01", "plant02", "plant03", "plant04", "plant05", "plant06" };
            //List<string> machines = new List<string> { "All", "machine01", "machine02", "machine03", "machine04", "machine05", "machine06" };
            //ddlPlantAllDown.DataSource = plants;
            //ddlPlantAllDown.DataBind();
            //ddlMachineAllDown.DataSource = machines;
            //ddlMachineAllDown.DataBind();
        }
        protected void chkSelectAllHoliday_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAllHoliday.Checked)
            {
                foreach (GridViewRow row in gvHolidays.Rows)
                {
                    (row.FindControl("chkSelectHoliday") as CheckBox).Checked = true;
                    row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                }
            }
            else
            {
                foreach (GridViewRow row in gvHolidays.Rows)
                {
                    (row.FindControl("chkSelectHoliday") as CheckBox).Checked = false;
                }
            }
        }

        protected void gvHolidays_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
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

        protected void gvMachineShift_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //GridViewRow gvRow = e.Row;
            //if (gvRow.RowType == DataControlRowType.Header)
            //{
            //    if (gvRow.Cells[0].Text == "MachineID")
            //    {
            //        gvRow.Cells.Remove(gvRow.Cells[0]);
            //        GridViewRow gvHeader = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            //        TableHeaderCell headerCell0 = new TableHeaderCell()
            //        {
            //            Text = "MachineID",
            //            HorizontalAlign = HorizontalAlign.Center,
            //            RowSpan = 2
            //        };
            //        gvHeader.Cells.Add(headerCell0);
            //        DataTable dtMachinesShift = GetDtMachineShift();
            //        string prevDate = dtMachinesShift.Columns[1].ColumnName;
            //        prevDate = prevDate.Substring(0, prevDate.Length - 2);
            //        int colspan = 0;
            //        foreach (DataColumn dataColumn in dtMachinesShift.Columns)
            //        {
            //            if (!dataColumn.ColumnName.Equals("MachineID", StringComparison.OrdinalIgnoreCase))
            //            {

            //                string date = dataColumn.ColumnName;
            //                date = date.Substring(0, date.Length - 2);
            //                if (!date.Equals(prevDate, StringComparison.OrdinalIgnoreCase))
            //                {
            //                    TableHeaderCell headerCell = new TableHeaderCell()
            //                    {
            //                        Text = prevDate,
            //                        HorizontalAlign = HorizontalAlign.Center,
            //                        ColumnSpan = colspan,
            //                        Width = new Unit("15%")
            //                    };
            //                    headerCell.Style.Add("cell-spacing", "2%");
            //                    gvHeader.Cells.Add(headerCell);
            //                    prevDate = date;
            //                    colspan = 1;
            //                }
            //                else
            //                {
            //                    colspan++;
            //                }
            //            }

            //        }
            //        TableHeaderCell headerCellLast = new TableHeaderCell()
            //        {
            //            Text = prevDate,
            //            HorizontalAlign = HorizontalAlign.Center,
            //            ColumnSpan = colspan,
            //            Width = new Unit("15%")
            //        };
            //        gvHeader.Cells.Add(headerCellLast);
            //        gvMachineShift.Controls[0].Controls.AddAt(0, gvHeader);
            //    }
            //}
            //if (gvRow.RowType == DataControlRowType.DataRow)
            //{
            //    for (int i = 0; i < gvRow.Cells.Count; i++)
            //    {
            //        if (gvRow.Cells[i].HasControls())
            //        {
            //            if (gvRow.Cells[i].Controls[0].GetType().Equals(typeof(CheckBox)))
            //            {
            //                CheckBox checkBox = (CheckBox)gvRow.Cells[i].Controls[0];
            //                checkBox.Enabled = false;
            //                if (checkBox.Checked)
            //                {
            //                    checkBox.ToolTip = "Down Reason";
            //                }

            //            }
            //        }
            //    }
            //}
        }


        #region ---------- New ---------------------
        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineIDs();
            //string plantid = ddlPlantID.SelectedValue;
            //List<string> machines = new List<string>() { "machine 01", "machine 02", "machine 03", "machine 04" };
            //ddlMachineIDs.DataSource = machines;
            //ddlMachineIDs.DataBind();
        }
        private void setActiveTab(string tab)
        {
            tabHoliday.Attributes["class"] = "";
            //tabWeeklyOff.Attributes["class"] = "";
            //tabDailyDown.Attributes["class"] = "";
            //tabShiftDown.Attributes["class"] = "";
            //tabAllDown.Attributes["class"] = "";
            //tabHolidayContent.Attributes["class"] = "";
            //tabWeeklyOffContent.Attributes["class"] = "";
            //tabDailyDownContent.Attributes["class"] = "";
            //tabShiftDownContent.Attributes["class"] = "";
            //tabAllDownContent.Attributes["class"] = "";
            gvHolidays.Visible = false;
            //gvWeekOffs.Visible = false;
            //gvDailyDowns.Visible = false;
            //gvMachineShift.Visible = false;
            //gvAllDowns.Visible = false;
            if (tab == "holiday")
            {
                tabHoliday.Attributes.Add("class", "active");
                tabHolidayContent.Attributes.Add("class", "active");
                gvHolidays.Visible = true;
            }
            //else if (tab == "weeklyoff")
            //{
            //    tabWeeklyOff.Attributes.Add("class", "active");
            //    tabWeeklyOffContent.Attributes.Add("class", "active");
            //    gvWeekOffs.Visible = true;
            //}
            //else if (tab == "dailydown")
            //{
            //    tabDailyDown.Attributes.Add("class", "active");
            //    tabDailyDownContent.Attributes.Add("class", "active");
            //    gvDailyDowns.Visible = true;
            //}
            //else if (tab == "shiftdown")
            //{
            //    tabShiftDown.Attributes.Add("class", "active");
            //    tabShiftDownContent.Attributes.Add("class", "active");
            //    gvMachineShift.Visible = true;
            //}
            //else if (tab == "alldown")
            //{
            //    tabAllDown.Attributes.Add("class", "active");
            //    tabAllDownContent.Attributes.Add("class", "active");
            //    gvAllDowns.Visible = true;
            //}

        }
        protected void btnHodiday_Click(object sender, EventArgs e)
        {
            setActiveTab("holiday");
            //BindHolidaysDropDowns();

            //BindHolidayData("","");

            if (txtFilterByDate.Visible || ddlFilterByReason.Visible )
            {
                if (Session["FltrDate"] == null && Session["FltrReason"] == null && Session["FltrMachine"] == null)
                {
                    BindHolidayData("", "");
                    divHolidayFilter.Visible = false;
                    return;
                }
                divHolidayFilter.Visible = true;
                if (txtFilterByDate.Visible)
                {
                    txtFilterByDate.Text = Session["FltrDate"].ToString();
                }
                if (ddlFilterByReason.Visible)
                {
                    ddlFilterByReason.Text = Session["FltrReason"].ToString();
                }
                //List<string> fltrMachine = (List<string>)Session["FltrMachine"];
                //if (ddlFilterByMachines.Visible)
                //{
                //    foreach (ListItem item in ddlFilterByMachines.Items)
                //    {
                //        if (fltrMachine.Contains(item.Text))
                //        {
                //            item.Selected = true;
                //        }
                //        else
                //        {
                //            item.Selected = false;
                //        }

                //    }
                //}
                btnApplyHoliday_Click(null, null);
                ddlFilter_SelectedIndexChanged(null, null);
            }
            else
            {
                BindHolidayData("", "");
                divHolidayFilter.Visible = false;
            }
        }
        private void BindHolidayData(string value, string param)
        {
            //List<Holidays> holidays = new List<Holidays>();
            //holidays.Add(new Holidays() { Holiday = "01-jan-2019", MachineID = "Machine01", Reason = "First Day of the year" });
            //holidays.Add(new Holidays() { Holiday = "26-jan-2019", MachineID = "Machine01", Reason = "Republic day" });
            //holidays.Add(new Holidays() { Holiday = "15-Aug-2019", MachineID = "Machine01", Reason = "Independence day" });
            //gvHolidays.DataSource = holidays;
            //gvHolidays.DataBind();

            try
            {
                List<HolidayListDetails> holidayListDetailsList = new List<HolidayListDetails>();
                holidayListDetailsList = AdvikDatabaseAccess.getHolidayList(value, param);
                gvHolidays.DataSource = holidayListDetailsList;
                gvHolidays.DataBind();

            }
            catch (Exception ex)
            {

            }
        }
        protected void btnWeeklyOff_Click(object sender, EventArgs e)
        {
            //setActiveTab("weeklyoff");
            //BindWeeklyFilterMachine();
            //txtFromWeekOffs.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
            //txtToWeekOffs.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
            //txtFromWeeklyOffs.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
            //txtToWeeklyOffs.Text = DateTime.Now.Date.AddDays(7).ToString("dd-MM-yyyy");
            //divWeeklyFilter.Visible = false;
            //BindWeeklyOffData();
        }
        private void BindWeeklyFilterMachine()
        {
            //ddlFilterWeekByMachine
        }
        private void BindWeeklyOffData()
        {
            //List<Holidays> weekHolidays = new List<Holidays>();
            //weekHolidays.Add(new Holidays() { Holiday = "03-Nov-2019", MachineID = "Machine01", Reason = "Sunday" });
            //weekHolidays.Add(new Holidays() { Holiday = "09-Nov-2019", MachineID = "Machine01", Reason = "Saturday" });
            //weekHolidays.Add(new Holidays() { Holiday = "10-Nov-2019", MachineID = "Machine01", Reason = "Sunday" });
            //weekHolidays.Add(new Holidays() { Holiday = "17-Nov-2019", MachineID = "Machine01", Reason = "Sunday" });
            //weekHolidays.Add(new Holidays() { Holiday = "23-Nov-2019", MachineID = "Machine01", Reason = "Saturday" });
            //weekHolidays.Add(new Holidays() { Holiday = "24-Nov-2019", MachineID = "Machine01", Reason = "Sunday" });
            //gvWeekOffs.DataSource = weekHolidays;
            //gvWeekOffs.DataBind();
        }

        protected void btnDailyDown_Click(object sender, EventArgs e)
        {
            //setActiveTab("dailydown");

            //txtFilterFromDailyDowns.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
            //txtFilterToDailyDowns.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
            //txtFromDailyDowns.Text = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");
            //txtToDailyDowns.Text = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");
            //BindDailyDownFilterMachineid();
            //divDailyDownFilter.Visible = false;
            //BindDailyDownData();
        }
        private void BindDailyDownData()
        {
            //List<Holidays> dailydowns = new List<Holidays>()
            //    {
            //        new Holidays{Holiday="20-Nov-2019 13:00-13:30", MachineID="Machine01", Reason="Lunch Break"},
            //        new Holidays{Holiday="20-Nov-2019 13:00-13:30", MachineID="Machine02", Reason="Lunch Break"},
            //        new Holidays{Holiday="20-Nov-2019 13:00-13:30", MachineID="Machine03", Reason="Lunch Break"},
            //        new Holidays{Holiday="20-Nov-2019 13:00-13:30", MachineID="Machine04", Reason="Lunch Break"}
            //    };
            //gvDailyDowns.DataSource = dailydowns;
            //gvDailyDowns.DataBind();
        }
        private void BindDailyDownFilterMachineid()
        {
            //ddlDailyDownFilterMachines.DataSource
        }



        protected void btnShiftDown_Click(object sender, EventArgs e)
        {
            //setActiveTab("shiftdown");
            //txtFromShiftDowns.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
            //txtToShiftDowns.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
        }

        protected void btnAllDown_Click(object sender, EventArgs e)
        {
            //setActiveTab("alldown");
            //BindAllDownDropDowns();
            //txtFromAllDowns.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
            //txtToAllDowns.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
            //BindAllDownData();
        }
        private void BindAllDownData()
        {
            //List<Down> allDowns = new List<Down>
            //    {
            //        new Down() { Holiday = "01-jan-2019", MachineID = "Machine01", Reason = "First Day of the year", Downs="Holidays" },
            //        new Down() { Holiday = "26-jan-2019", MachineID = "Machine01", Reason = "Republic day", Downs="Holidays"  },
            //        new Down() { Holiday = "15-Aug-2019", MachineID = "Machine01", Reason = "Independence day", Downs="Holidays"  },
            //        new Down() { Holiday = "03-Nov-2019", MachineID = "Machine01", Reason = "Sunday", Downs="Weekly Off"  },
            //        new Down() { Holiday = "09-Nov-2019", MachineID = "Machine01", Reason = "Saturday", Downs="Weekly Off"  },
            //        new Down() { Holiday = "10-Nov-2019", MachineID = "Machine01", Reason = "Sunday", Downs="Weekly Off"  },
            //        new Down() { Holiday = "17-Nov-2019", MachineID = "Machine01", Reason = "Sunday", Downs="Weekly Off"  },
            //        new Down() { Holiday = "23-Nov-2019", MachineID = "Machine01", Reason = "Saturday", Downs="Weekly Off"  },
            //        new Down() { Holiday = "24-Nov-2019", MachineID = "Machine01", Reason = "Sunday", Downs="Weekly Off"  },
            //        new Down{Holiday="20-Nov-2019 13:00-13:30", MachineID="Machine01", Reason="Lunch Break", Downs="DailyDowns" },
            //        new Down{Holiday="20-Nov-2019 13:00-13:30", MachineID="Machine02", Reason="Lunch Break", Downs="DailyDowns" },
            //        new Down{Holiday="20-Nov-2019 13:00-13:30", MachineID="Machine03", Reason="Lunch Break", Downs="DailyDowns" },
            //        new Down{Holiday="20-Nov-2019 13:00-13:30", MachineID="Machine04", Reason="Lunch Break", Downs="DailyDowns" }
            //    };
            //gvAllDowns.DataSource = allDowns;
            //gvAllDowns.DataBind();
        }
        protected void lbtnReload_Click(object sender, EventArgs e)
        {
            chkSelectAllHoliday.Checked = false;
            Session["FltrDate"] = null;
            Session["FltrReason"] = null;
            Session["FltrMachine"] = null;
            btnHodiday_Click(null, null);
        }

        protected void lnkFilter_Click(object sender, EventArgs e)
        {
            divHolidayFilter.Visible = true;
            ddlFilter_SelectedIndexChanged(null, null);
        }
        protected void btnSaveHoliday_Click(object sender, EventArgs e)
        {
            try
            {
                int fromYear = Convert.ToInt16(ddlFromYear.SelectedItem == null ? "" : ddlFromYear.SelectedItem.ToString());
                int toYear = Convert.ToInt16(ddlToYear.SelectedItem == null ? "" : ddlToYear.SelectedItem.ToString());
                string month = (Convert.ToInt16((ddlMonth.SelectedValue == null ? "" : ddlMonth.SelectedValue.ToString())) + 1).ToString("00");
                string day = (Convert.ToInt16((ddlDay.SelectedItem == null ? "" : ddlDay.SelectedItem.ToString()))).ToString("00");
                string reason = ddlReason.Value;

                if (reason == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Reason field is required.')", true);
                    return;
                }
                string sqlQuery = "";
                //foreach (ListItem item in ddlMachineIDs.Items)
                //{
                //    if (item.Selected)
                //    {
                //        for (int i = fromYear; i <= toYear; i++)
                //        {
                //            string holiday = i + "-" + month + "-" + day + " 00:00:00";
                //            string ss = @"if exists(select * from HolidayList where Holiday='" + holiday + "' and MachineID='" + item.Text + "') begin update HolidayList set Reason = '" + reason + "' where Holiday = '" + holiday + "' and MachineID = '" + item.Text + "' end else begin insert into HolidayList(Holiday, Reason, MachineID) values('" + holiday + "', '" + reason + "', '" + item.Text + "')end; ";
                //            sqlQuery += ss;
                //        }
                //    }

                //}
                for (int i = fromYear; i <= toYear; i++)
                {
                    string holiday = i + "-" + month + "-" + day + " 00:00:00";
                    string ss = @"if exists(select * from HolidayList where Holiday='" + holiday + "') begin update HolidayList set Reason = '" + reason + "' where Holiday = '" + holiday + "' end else begin insert into HolidayList(Holiday, Reason) values('" + holiday + "', '" + reason + "')end; ";
                    sqlQuery += ss;
                }
                if (sqlQuery == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Machine Id required')", true);
                    return;
                }
                int success = AdvikDatabaseAccess.saveupdateHolidayDetails(sqlQuery);
                if (success <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Failed to Save Holiday List')", true);
                    return;
                }
                BindHolidayData("","");
                divHolidayFilter.Visible = false;
                //string machine = "";
                //foreach (ListItem item in ddlMachineIDs.Items)
                //{
                //    if (item.Selected)
                //    {
                //        if (machine == "")
                //        {
                //            machine = "'" + item.Text + "'";
                //        }
                //        else
                //        {
                //            machine += machine + ",'" + item.Text + "'";
                //        }

                //    }

                //}
                // BindHolidayData(machine, "ByMachine");
                //btnHodiday_Click(null, null);
            }
            catch (Exception ex)
            {

            }


        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            BindHolidayData("", "");
        }

        protected void lnkHolidayFilterClose_Click(object sender, EventArgs e)
        {
            divHolidayFilter.Visible = false;
        }

        #endregion

        protected void lbtnReloadWeekOffs_Click(object sender, EventArgs e)
        {
            //chkSelectAllWeekOffs.Checked = false;
            //BindWeeklyOffData();
        }

        protected void lbtnDeleteWeekOffs_Click(object sender, EventArgs e)
        {
            BindWeeklyOffData();
        }

        protected void lnkWeeklyFilter_Click(object sender, EventArgs e)
        {
            // divWeeklyFilter.Visible = true;
        }

        protected void btnApplyWeekOffsFilter_Click(object sender, EventArgs e)
        {
            //string s1 = txtFromWeekOffs.Text;
            //string s2 = txtToWeekOffs.Text;
            //string s3 = ddlFilterWeekByMachine.SelectedValue;
            //BindWeeklyOffData();
        }

        protected void lnkWeeklyFilterClose_Click(object sender, EventArgs e)
        {
            // divWeeklyFilter.Visible = false;
        }

        protected void lnkDailyDownFilter_Click(object sender, EventArgs e)
        {
            // divDailyDownFilter.Visible = true;
        }

        protected void lnkDailyDownsReload_Click(object sender, EventArgs e)
        {
            //chkSelectAllDailyDowns.Checked = false;
            //BindDailyDownData();
        }

        protected void lnkDailyDownDelete_Click(object sender, EventArgs e)
        {
            BindDailyDownData();
        }

        protected void lnkDailyDownFilterClose_Click(object sender, EventArgs e)
        {
            // divDailyDownFilter.Visible = false;
        }

        protected void btnDailyDownFilterAppy_Click(object sender, EventArgs e)
        {
            BindDailyDownData();
        }

        protected void btnDailyDownSave_Click(object sender, EventArgs e)
        {
            BindDailyDownData();
        }

        protected void lnkAllDownRepeat_Click(object sender, EventArgs e)
        {
            BindAllDownData();
        }

        protected void lnkAllDownDelete_Click(object sender, EventArgs e)
        {
            BindAllDownData();
        }

        protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string filterValue = ddlFilter.SelectedValue == null ? "" : ddlFilter.SelectedValue.ToString();
                switch (filterValue)
                {
                    case "ByDate":
                        lblDate.Visible = true;
                        txtFilterByDate.Visible = true;
                        lblReason.Visible = false;
                        ddlFilterByReason.Visible = false;
                        //lblMachine.Visible = false;
                        //ddlFilterByMachines.Visible = false;
                        break;
                    case "ByReason":
                        lblDate.Visible = false;
                        txtFilterByDate.Visible = false;
                        List<string> reasons = AdvikDatabaseAccess.getAllReasons();
                        ddlFilterByReason.DataSource = reasons;
                        ddlFilterByReason.DataBind();
                        lblReason.Visible = true;
                        ddlFilterByReason.Visible = true;
                        //lblMachine.Visible = false;
                        //ddlFilterByMachines.Visible = false;
                        break;
                    //case "ByMachine":
                    //    lblDate.Visible = false;
                    //    txtFilterByDate.Visible = false;
                    //    lblReason.Visible = false;
                    //    ddlFilterByReason.Visible = false;
                    //    ddlFilterByMachines.DataSource = AdvikDatabaseAccess.getMachineForPlant("");
                    //    ddlFilterByMachines.DataBind();
                    //    lblMachine.Visible = true;
                    //    ddlFilterByMachines.Visible = true;
                    //    break;

                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void btnApplyHoliday_Click(object sender, EventArgs e)
        {
            try
            {
                string filterValue = ddlFilter.SelectedValue == null ? "" : ddlFilter.SelectedValue.ToString();
                switch (filterValue)
                {
                    case "ByDate":
                        DateTime date = DateTime.Now.Date;
                        date = Util.GetDateTime(txtFilterByDate.Text + " 00:00:00");
                        BindHolidayData(date.ToString("yyyy-MM-dd HH:mm:ss"), "ByDate");
                        break;
                    case "ByReason":
                        BindHolidayData(ddlFilterByReason.SelectedItem == null ? "" : ddlFilterByReason.SelectedItem.ToString(), "ByReason");
                        break;
                    //case "ByMachine":
                    //    string machine = "";
                    //    foreach (ListItem item in ddlFilterByMachines.Items)
                    //    {
                    //        if (item.Selected)
                    //        {
                    //            if (machine == "")
                    //            {
                    //                machine = "'" + item.Text + "'";
                    //            }
                    //            else
                    //            {
                    //                machine += machine + ",'" + item.Text + "'";
                    //            }

                    //        }

                    //    }
                    //    if (machine == "")
                    //    {
                    //        return;
                    //    }
                    //    BindHolidayData(machine, "ByMachine");
                    //    break;

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
                if (txtFilterByDate.Visible)
                {
                    fltrDate = txtFilterByDate.Text;
                }
                if (ddlFilterByReason.Visible)
                {
                    fltrReason = ddlFilterByReason.SelectedValue == null ? "" : ddlFilterByReason.SelectedValue.ToString();
                }
                //if (ddlFilterByMachines.Visible)
                //{
                //    foreach (ListItem item in ddlFilterByMachines.Items)
                //    {
                //        if (item.Selected)
                //        {
                //            fltrMachine.Add(item.Text);
                //        }

                //    }
                //}
                Session["FltrDate"] = fltrDate;
                Session["FltrReason"] = fltrReason;
                //Session["FltrMachine"] = fltrMachine;
                string sqlQuery = "";
                for (int i = 0; i < gvHolidays.Rows.Count; i++)
                {
                    if ((gvHolidays.Rows[i].FindControl("chkSelectHoliday") as CheckBox).Checked)
                    {
                        //string machine = (gvHolidays.Rows[i].FindControl("lblMachineId") as Label).Text;
                        string holiday = (gvHolidays.Rows[i].FindControl("lblHoliday") as Label).Text;
                        if (holiday != "")
                        {
                            DateTime date = DateTime.Now.Date;
                            date = Util.GetDateTime(holiday);
                            holiday = date.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        string ss = @"delete from HolidayList where  Holiday='" + holiday + "'; ";
                        sqlQuery += ss;
                    }

                }
                int success = AdvikDatabaseAccess.deleteHolidayDetails(sqlQuery);
                if (success <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Failed to delete Holiday List')", true);
                    return;
                }
                chkSelectAllHoliday.Checked = false;
                //BindHolidayData("", "");
                //if(txtFilterByDate.Visible || ddlFilterByReason.Visible || ddlFilterByMachines.Visible)
                //{
                //    ddlFilter_SelectedIndexChanged(null, null);
                //    if (txtFilterByDate.Visible)
                //    {
                //        txtFilterByDate.Text = fltrDate;
                //    }
                //    if (ddlFilterByReason.Visible)
                //    {
                //        ddlFilterByReason.Text = fltrReason;
                //    }
                //    if (ddlFilterByMachines.Visible)
                //    {
                //        foreach (ListItem item in ddlFilterByMachines.Items)
                //        {
                //            if (fltrMachine.Contains(item.Text))
                //            {
                //                item.Selected = true;
                //            }
                //            else
                //            {
                //                item.Selected = false ;
                //            }

                //        }
                //    }
                //    btnApplyHoliday_Click(null, null);
                //}
                //else
                //{
                //    BindHolidayData("", "");
                //}

            }
            catch (Exception ex)
            {

            }
        }
    }

}