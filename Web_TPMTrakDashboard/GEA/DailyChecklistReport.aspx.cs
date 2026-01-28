using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class DailyChecklistReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                if (!IsPostBack)
                {
                    SessionClear.ClearSession();
                    BindLineIDs();
                    BindMachineIDs();
                    BindMonths();
                    BindYears();
                    BindChecklistReportData();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void BindLineIDs()
        {
            try
            {
                List<string> lstLineData = GEADatabaseAccess.GetLineIDsForPlant("");
                if (lstLineData != null && lstLineData.Count > 0)
                {
                    ddlLineID.DataSource = lstLineData;
                    ddlLineID.DataBind();
                    ddlLineID.Items.Insert(0, new ListItem
                    {
                        Text = "Line All",
                        Value = "All"
                    });
                    ddlLineID.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private void BindMachineIDs()
        {
            try
            {
                string LineId = ddlLineID.SelectedItem != null ? ddlLineID.SelectedValue : "";
                if (!string.IsNullOrEmpty(LineId))
                {
                    if (LineId.Equals("All", StringComparison.OrdinalIgnoreCase)) LineId = "";
                    List<string> lstMachineIDs = GEADatabaseAccess.GetAllMachineByPlantandGroup("", LineId);
                    if (lstMachineIDs != null && lstMachineIDs.Count > 0)
                    {
                        ddlMachineId.DataSource = lstMachineIDs;
                        ddlMachineId.DataBind();
                        ddlMachineId.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private void BindYears()
        {
            try
            {
                List<int> lstYears = Enumerable.Range(DateTime.Now.Year - 9, 10).Reverse().ToList();
                if (lstYears != null && lstYears.Count > 0)
                {
                    ddlYear.DataSource = lstYears;
                    ddlYear.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private void BindMonths()
        {
            try
            {
                var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                string currentMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                for (int i = 0; i < months.Length; i++)
                {
                    ddlMonth.Items.Add(new ListItem(months[i], (i + 1).ToString()));
                }
                if (!string.IsNullOrEmpty(currentMonthName))
                    ddlMonth.SelectedIndex = Array.IndexOf(months, currentMonthName);
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private void BindChecklistReportData()
        {
            DataTable dtDailylyChklistReportData = new DataTable();
            try
            {
                string LineId = ddlLineID.SelectedItem != null ? ddlLineID.SelectedValue : "";
                string MachineID = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                int Month = ddlMonth.SelectedItem != null ? Convert.ToInt32(ddlMonth.SelectedItem.Value) : DateTime.Now.Month;
                int Year = ddlYear.SelectedItem != null ? Convert.ToInt32(ddlYear.SelectedItem.Text) : DateTime.Now.Year;
                string startDate = Year + "-" + Month + "-01";
                if (!string.IsNullOrEmpty(LineId) && !string.IsNullOrEmpty(MachineID))
                {
                    if (LineId.Equals("All", StringComparison.OrdinalIgnoreCase)) LineId = "";
                    dtDailylyChklistReportData = GEADatabaseAccess.GetDailyChecklistReportData(LineId, MachineID, startDate, out DataTable SecondGrid);
                    if (dtDailylyChklistReportData != null && dtDailylyChklistReportData.Rows.Count > 0)
                    {
                        DataTable dtDailylyChklistReportData_Copy = dtDailylyChklistReportData.Copy();
                        Session["DailylyChklistReportData"] = dtDailylyChklistReportData;
                        dtDailylyChklistReportData_Copy.Columns.Remove("Frequency");
                        dtDailylyChklistReportData_Copy.Columns.Remove("FreqID");
                        dtDailylyChklistReportData_Copy.Columns.Remove("TemplateType");
                        dtDailylyChklistReportData_Copy.Columns.Remove("Machineid");
                        GridDailyChecklistReport.DataSource = dtDailylyChklistReportData_Copy;
                        GridDailyChecklistReport.DataBind();
                    }
                    else
                    {
                        GridDailyChecklistReport.DataSource = new DataTable();
                        GridDailyChecklistReport.DataBind();
                        Session["DailylyChklistReportData"] = dtDailylyChklistReportData;
                        Session["DailylyChklistSupManinData"] = SecondGrid;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindChecklistReportData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            bool successfull = false;
            DataTable dtDailylyChklistReportData = new DataTable();
            DataTable SupData = new DataTable();
            DataTable dtRemarks = new DataTable();
            try
            {
                string LineId = ddlLineID.SelectedItem != null ? ddlLineID.SelectedValue : "";
                string MachineID = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                string Month = ddlMonth.SelectedItem != null ? ddlMonth.SelectedItem.Text : DateTime.Now.ToString("MMMM");
                int Year = ddlYear.SelectedItem != null ? Convert.ToInt32(ddlYear.SelectedItem.Text) : DateTime.Now.Year;
                string startDate = Year + "-" + Month + "-01";
                if (Session["DailylyChklistReportData"] != null && Session["DailylyChklistSupManinData"] != null)
                {
                    dtDailylyChklistReportData = Session["DailylyChklistReportData"] as DataTable;
                    SupData = Session["DailylyChklistSupManinData"] as DataTable; 
                }
                else
                {
                    if (!string.IsNullOrEmpty(MachineID))
                    {
                        dtDailylyChklistReportData = GEADatabaseAccess.GetDailyChecklistReportData(LineId, MachineID, startDate,out SupData);
                    }
                }
                dtRemarks = GEADatabaseAccess.GetDailyChecklistReportRemarksData(LineId, MachineID, startDate);
                if (dtDailylyChklistReportData != null && dtDailylyChklistReportData.Rows.Count > 0)
                {
                    Session["DailylyChklistReportData"] = dtDailylyChklistReportData;
                    successfull = GEAGenerateReport.GenerateDailyChklistReport(LineId, MachineID, Month, Year, dtDailylyChklistReportData,SupData, dtRemarks);
                    if (successfull)
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('Export Successful.')", true);
                    else
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('Error. Export Unsuccessful.')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('No data to export.')", true);
                }

            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.DarkRed;
                lblMessages.Text = ex.Message;
            }
        }

        protected void ddlLineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineIDs();
        }

        protected void GridDailyChecklistReport_DataBound(object sender, EventArgs e)
        {
            if (GridDailyChecklistReport.Rows.Count > 0)
            {
                for (int i = 3; i < GridDailyChecklistReport.Rows[0].Cells.Count; i++)
                {
                    string date = GridDailyChecklistReport.HeaderRow.Cells[i].Text;
                    GridDailyChecklistReport.HeaderRow.Cells[i].Text = date.Split('-')[2];
                }
            }

        }
    }
}