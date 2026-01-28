using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class PMActivityDateGeneration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtYear.Text = DateTime.Now.ToString("yyyy");
                DateTime now = DateTime.Now;
                now = new DateTime(now.Year, now.Month, 1);
                txtFromDate.Text = now.ToString("dd-MM-yyyy HH:mm");
                txtToDate.Text = now.AddMonths(1).AddSeconds(-1).ToString("dd-MM-yyyy HH:mm");
                txtStartDate.Text = DateTime.Now.ToString("dd-MM-yyyy") + " 00:00";
                BindFrequency(ddlFrequency, false);
                BindMachineIds();
                BindActivityTimingData();
            }
        }
        private void BindMachineIds()
        {
            try
            {
                List<string> machines = DataBaseAccess.GetMachineInfoForPM();
                ddlMachineID.DataSource = machines;
                ddlMachineID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindFrequency(DropDownList ddl, bool IsAllRequired)
        {
            try
            {
                List<ListItem> list = DataBaseAccess.getFrequencyForActivityMasters();
                if (IsAllRequired)
                {
                    list.Insert(0, new ListItem { Text = "All", Value = "" });
                }
                ddl.DataSource = list;
                ddl.DataTextField = "Text";
                ddl.DataValueField = "Value";
                ddl.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindActivityTimingData()
        {
            try
            {
                //string machinidlist = "";
                //foreach (ListItem item in lbMachineActivityTiming.Items)
                //{
                //    if (item.Selected)
                //    {
                //        if (machinidlist == "")
                //        {
                //            machinidlist += item.Value;
                //        }
                //        else
                //        {
                //            machinidlist += "," + item.Value;
                //        }
                //    }
                //}

                DataTable dt = DataBaseAccess.GetActivityData(ddlFrequency.SelectedItem.Text, txtYear.Text, txtFromDate.Text, txtToDate.Text, ddlMachineID.SelectedValue);
                DataTable newDt = new DataTable();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow newRow = newDt.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string columnName = dt.Columns[j].ColumnName;
                        string rowValue = "";
                        if (i == 0)
                        {
                            newDt.Columns.Add(columnName, typeof(string));
                        }

                        if (j >= 9)
                        {
                            string date = dt.Rows[i][j].ToString();
                            if (date != "" && date != null)
                            {
                                rowValue = Util.GetDateTime(date).ToString("dd-MM-yyyy HH:mm");
                            }
                        }
                        else
                        {
                            rowValue = dt.Rows[i][j].ToString();
                        }
                        newRow[columnName] = rowValue;
                    }
                    newDt.Rows.Add(newRow);
                }
                gvActvityTiming.DataSource = newDt;
                gvActvityTiming.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindActivityTimingData();
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                bool res;
                res = DataBaseAccess.GenerateActivityData(ddlFrequency.SelectedItem.ToString(), txtYear.Text, txtStartDate.Text, ddlMachineID.SelectedValue);
                if (res == true)
                {
                    BindActivityTimingData();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "No Parts", "alert('Activity information generated successfully')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "No Parts", "alert('Activity information could not be generated')", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string activityTimingsUpdationValidation(string year, string activity, string frequency, string frequencyName, string oldDateTime, string newDateTime, string machineid, string uniqueCode, string main_MachineID, string shiftID)
        {
            string message = "";
            try
            {
                bool proceed = true, status = false;
                DateTime olddDate = Util.GetDateTime(oldDateTime);
                DateTime newDate = Util.GetDateTime(newDateTime);
                switch (frequencyName.ToLower())
                {
                    case "shift":
                        if (olddDate.Day != newDate.Day)
                        {
                            proceed = false;
                            message = "The new date cannot be on a different day";
                        }
                        break;
                    case "daily":
                        if (olddDate.Day != newDate.Day)
                        {
                            proceed = false;
                            message = "The new date cannot be on a different day";
                        }
                        break;
                    case "weekly":
                        if (CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(olddDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday) != CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(newDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                        {
                            proceed = false;
                            message = "The new date cannot be on a different week";
                        }
                        break;
                    case "monthly":
                        if (olddDate.Month != newDate.Month)
                        {
                            proceed = false;
                            message = "The new date cannot be on a different month";
                        }
                        break;
                    case "yearly":
                        if (olddDate.Year != newDate.Year)
                        {
                            proceed = false;
                            message = "The new date cannot be on a different year";
                        }
                        break;
                    case string x when x.Contains("days"):
                        if (x.ToLower().Equals("15 days", StringComparison.OrdinalIgnoreCase) && (Math.Abs(olddDate.Day - newDate.Day) > 14))
                        {
                            proceed = false;
                            message = "The new date should be within 15 days";
                        }
                        break;
                    case string x when x.Contains("month"):
                        if (x.ToLower().Equals("1 month", StringComparison.OrdinalIgnoreCase) && (Math.Abs(olddDate.Month - newDate.Month) > 0))
                        {
                            status = true;
                        }
                        else if (x.ToLower().Equals("3 month", StringComparison.OrdinalIgnoreCase) && (Math.Abs(olddDate.Month - newDate.Month) > 2))
                        {
                            status = true;
                        }
                        else if (x.ToLower().Equals("6 month", StringComparison.OrdinalIgnoreCase) && (Math.Abs(olddDate.Month - newDate.Month) > 5))
                        {
                            status = true;
                        }
                        if (status)
                        {
                            proceed = false;
                            message = "The new date should be within " + x;
                        }
                        break;
                    case string x when x.Contains("year"):
                        if (x.ToLower().Equals("1 year", StringComparison.OrdinalIgnoreCase) && (Math.Abs(olddDate.Year - newDate.Year) > 0))
                        {
                            status = true;
                        }
                        if (x.ToLower().Equals("2 year", StringComparison.OrdinalIgnoreCase) && (Math.Abs(olddDate.Year - newDate.Year) > 1))
                        {
                            status = true;
                        }
                        if (status)
                        {
                            proceed = false;
                            message = "The new date should be within " + x;
                        }
                        break;
                        // include 15 days/ 3, 6 month / 2 years if they mean fortnight, quarter etc.
                }
                if (proceed)
                {
                    year = olddDate.Year.ToString();
                    if (DataBaseAccess.UpdateActivityData(frequencyName, year, olddDate, newDate, activity, main_MachineID,shiftID))
                    {
                        message = "Updated";
                    }
                    else
                    {
                        message = "Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return message;
        }

        protected void btnActivityTimingDateUpdate_Click(object sender, EventArgs e)
        {
            BindActivityTimingData();
        }
    }
}