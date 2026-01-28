using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class WeeklyChecklistReport : System.Web.UI.Page
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
                    BindYears();
                    BindChecklistReportData();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
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
                    ddlMachineId.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private void BindChecklistReportData()
        {
            DataTable dtWeeklyChklistReportData = new DataTable();
            try
            {
                string LineId = ddlLineID.SelectedItem != null ? ddlLineID.SelectedValue : "";
                string MachineID = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                int Year = ddlYear.SelectedItem != null ? Convert.ToInt32(ddlYear.SelectedItem.Text) : DateTime.Now.Year;
                if (!string.IsNullOrEmpty(LineId) && !string.IsNullOrEmpty(MachineID))
                {
                    if (LineId.Equals("All", StringComparison.OrdinalIgnoreCase)) LineId = "";
                    Session["LineID"] = LineId;
                    Session["MachineID"] = MachineID;
                    dtWeeklyChklistReportData = GEADatabaseAccess.GetWeeklyChklistReportData(LineId, MachineID, Year);
                    DataTable SecondGrid = GEADatabaseAccess.GetWeeklyChklistReportData(LineId, MachineID, Year, "Second");
                    if (dtWeeklyChklistReportData != null && dtWeeklyChklistReportData.Rows.Count > 0)
                    {
                        List<string> spanHeaders = dtWeeklyChklistReportData.Columns.Cast<DataColumn>().Select(x => x.ColumnName.Contains("-") ? x.ColumnName.Split('-')[0] : x.ColumnName).ToList();
                        List<string> headers = dtWeeklyChklistReportData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                        Session["SpanHeaders"] = spanHeaders;
                        Session["WeeklyChklistReportData"] = dtWeeklyChklistReportData;
                        Session["WeeklyChklistSecondReportData"] = SecondGrid;
                        GridMaintTransactionReport.Columns.Clear();
                        foreach (string header in headers)
                        {
                            if (headers.IndexOf(header) < 7)
                            {
                                if (!header.Equals("Machineid") && !header.Equals("Frequency") && !header.Equals("FreqID"))
                                {
                                    TemplateField templateField = new TemplateField();
                                    templateField.HeaderText = header.Equals("Chekpoints") ? "Check Points" : header;
                                    templateField.ControlStyle.Width = header.Equals("Chekpoints") ? Unit.Pixel(240) : Unit.Pixel(150);
                                    templateField.ItemTemplate = new LabelTemplateGenerator(ListItemType.Item, header);
                                    GridMaintTransactionReport.Columns.Add(templateField);
                                }
                            }
                            else
                            {
                                TemplateField templateField = new TemplateField
                                {
                                    HeaderText = header.Split('-')[1],
                                    ItemTemplate = new ImageTemplateGenerator(ListItemType.Item, header)
                                };
                                GridMaintTransactionReport.Columns.Add(templateField);
                            }
                        }
                        GridMaintTransactionReport.DataSource = dtWeeklyChklistReportData;
                        GridMaintTransactionReport.DataBind();
                    }
                    else
                    {
                        GridMaintTransactionReport.DataSource = null;
                        GridMaintTransactionReport.DataBind();
                    }
                }
                else
                {
                    GridMaintTransactionReport.DataSource = null;
                    GridMaintTransactionReport.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        protected void GridMaintTransactionReport_DataBound(object sender, EventArgs e)
        {
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            List<string> headers = Session["SpanHeaders"] as List<string>;
            if (headers != null && headers.Count > 0)
            {
                Dictionary<string, int> headerCounts = headers.GroupBy(x => x).Select(x => new { Month = x.Key, Count = x.Count() }).ToDictionary(x => x.Month, x => x.Count);
                foreach (KeyValuePair<string, int> keyValuePair in headerCounts)
                {
                    if (!keyValuePair.Key.Equals("Machineid") && !keyValuePair.Key.Equals("Frequency") && !keyValuePair.Key.Equals("FreqID"))
                    {
                        if (keyValuePair.Key.Equals("ActivityID") || keyValuePair.Key.Equals("Chekpoints") || keyValuePair.Key.Equals("Method") || keyValuePair.Key.Equals("Criteria"))
                        {
                            TableHeaderCell cellHeaderGroup = new TableHeaderCell
                            {
                                Text = "",
                                ColumnSpan = keyValuePair.Value,
                            };
                            row.Controls.Add(cellHeaderGroup);
                        }
                        else
                        {
                            TableHeaderCell cellHeaderGroup = new TableHeaderCell
                            {
                                Text = keyValuePair.Key,
                                ColumnSpan = keyValuePair.Value
                            };
                            row.Controls.Add(cellHeaderGroup);
                        }
                    }
                }
                row.BackColor = ColorTranslator.FromHtml("#5391CA");
                if (GridMaintTransactionReport.HeaderRow != null)
                    GridMaintTransactionReport.HeaderRow.Parent.Controls.AddAt(0, row);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindChecklistReportData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            bool successfull = false;
            DataTable dtWeeklyChklistReportData = new DataTable();
            DataTable SecondGrid = new DataTable();
            DataTable remarksGrid = new DataTable();
            try
            {
                string LineId = ddlLineID.SelectedItem != null ? ddlLineID.SelectedValue : "";
                string MachineID = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                int Year = ddlYear.SelectedItem != null ? Convert.ToInt32(ddlYear.SelectedItem.Text) : DateTime.Now.Year;
                if (Session["WeeklyChklistReportData"] != null && Session["WeeklyChklistSecondReportData"] == null)
                {
                    dtWeeklyChklistReportData = Session["WeeklyChklistReportData"] as DataTable;
                }
                else
                {
                    if (!string.IsNullOrEmpty(LineId) && !string.IsNullOrEmpty(MachineID))
                    {
                        if (LineId.Equals("All", StringComparison.OrdinalIgnoreCase)) LineId = "";
                        Session["LineID"] = LineId;
                        Session["MachineID"] = MachineID;
                        dtWeeklyChklistReportData = GEADatabaseAccess.GetWeeklyChklistReportData(LineId, MachineID, Year );
                        SecondGrid = GEADatabaseAccess.GetWeeklyChklistReportData(LineId, MachineID, Year,"Second");
                       
                    }
                }
                remarksGrid = GEADatabaseAccess.GetWeeklyChklistReportRemarksData(LineId, MachineID, Year);
                if (dtWeeklyChklistReportData != null && dtWeeklyChklistReportData.Rows.Count > 0)
                {
                    Session["WeeklyChklistReportData"] = dtWeeklyChklistReportData;
                    Session["WeeklyChklistSecondReportData"] = SecondGrid;
                    successfull = GEAGenerateReport.GenerateWeeklyChklistReport(LineId, MachineID, dtWeeklyChklistReportData,SecondGrid, Year, remarksGrid);
                    if (successfull)
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ExportSuccess", "alert('Export Successful.')", true);
                    else
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ExportError", "alert('Error. Export Unsuccessful.')", true);
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
    }
}