using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
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
    public partial class WeekDefinition : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtYear.Text = DateTime.Now.ToString("yyyy");
                btnWeekView_Click(null, EventArgs.Empty);
                //BindDataGrid();
            }
        }

        protected void btnWeekGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                int year = Convert.ToInt32(txtYear.Text);
                int weekNo = 1;
                int MonthStart = DataBaseAccess.getFinancialsMonth();

                int lastYear = MonthStart > 1 ? year + 1 : year;
                int lastMonth = MonthStart > 1 ? MonthStart - 1 : 12;
                int lastDay = DateTime.DaysInMonth(lastYear, lastMonth);

                //DateTime nextDay = new DateTime(year, 1, 1);
                //DateTime lastDayOfYear = new DateTime(year, 12, 31);

                DateTime nextDay = new DateTime(year, MonthStart, 1);
                DateTime lastDayOfYear = new DateTime(lastYear, lastMonth, lastDay);

                string startingDayOfweek = ddlStartingDayOfWeek.SelectedItem.Text;
                DataBaseAccess.DeleteFromCalender(year, MonthStart);

                DataTable dtInsertInToCalender = new DataTable();
                dtInsertInToCalender.Columns.AddRange(new DataColumn[] { new DataColumn("WeekDate", typeof(DateTime)), new DataColumn("WeekNumber", typeof(int)), new DataColumn("MonthVal", typeof(int)), new DataColumn("YearNo", typeof(int)) });

                while (nextDay <= lastDayOfYear)
                {
                    DataRow dataRow = dtInsertInToCalender.NewRow();
                    dataRow["WeekDate"] = nextDay;
                    dataRow["WeekNumber"] = weekNo;
                    dataRow["MonthVal"] = nextDay.Month;
                    dataRow["YearNo"] = nextDay.Year;
                    dtInsertInToCalender.Rows.Add(dataRow);
                    //DataBaseAccess.InsertInToCalender(nextDay, weekNo, nextDay.Month, nextDay.Year);
                    nextDay = nextDay.AddDays(1);
                    if (nextDay.DayOfWeek.ToString().Equals(startingDayOfweek, StringComparison.OrdinalIgnoreCase))
                        weekNo++;
                }

                DataBaseAccess.BulkInsertIntoCalender(dtInsertInToCalender);

                btnWeekView_Click(null, EventArgs.Empty);
                lblMessages.ForeColor = System.Drawing.Color.Green;
                lblMessages.Text = "Week information for the year " + year + " has been generated.";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                //ScriptManager.RegisterStartupScript(this, GetType(), "Successful", "alert('Week information for the year " + year + " has been generated.')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error while Generating Week Definition :" + ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }

        protected void btnWeekView_Click(object sender, EventArgs e)
        {
            try
            {
                int year = Convert.ToInt32(txtYear.Text);
                DataTable dtWeekInformation = DataBaseAccess.GetWeekInformationFromDB(year);
                if (dtWeekInformation != null && dtWeekInformation.Rows.Count > 0)
                {
                    gvWeekDefinition.DataSource = dtWeekInformation;
                    gvWeekDefinition.DataBind();
                }
                else if (dtWeekInformation != null && dtWeekInformation.Rows.Count <= 0)
                {
                    gvWeekDefinition.DataSource = dtWeekInformation;
                    gvWeekDefinition.DataBind();
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Weekly Information is not yet populated for the year " + year;
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "No Data", "alert('Weekly Information is not yet populated for the year " + year + "')", true);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }

        }


        //public static DateTime FirstDateOfWeekISO8601(int year, string DayOfweek)
        //{
        //    int weekNumber = 1;
        //    DateTime jan1 = new DateTime(year, 1, 1);
        //    int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

        //    DateTime firstThursday = jan1.AddDays(daysOffset);
        //    var cal = CultureInfo.CurrentCulture.Calendar;
        //    int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        //    var weekNum = weekNumber;
        //    if (firstWeek == 1)
        //    {
        //        weekNum -= 1;
        //    }

        //    var result = firstThursday.AddDays(weekNum * 7);

        //    return result.AddDays(-3);
        //}
    }
}