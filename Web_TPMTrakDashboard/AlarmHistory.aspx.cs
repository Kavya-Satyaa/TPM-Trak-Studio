using Elmah;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using System.Drawing;
using System.Web.Services;

namespace Web_TPMTrakDashboard
{
    public partial class Alarm_History : System.Web.UI.Page
    {
        string APP_PATH = HttpContext.Current.Server.MapPath("~/Reports");

        List<string> ddllist = new List<string>();
        List<string> ddldata = new List<string>();
        List<AlarmHistory> AlarmReportData = new List<AlarmHistory>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                else
                    SessionClear.ClearSession();
                timerDataChange.Enabled = false;
                txtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                ddlplant.DataSource = Binddata("Plant");
                ddlplant.DataBind();
                ddlplant.Items.Insert(0, "ALL");
                ddlMachine.DataSource = BindMachine();
                ddlMachine.DataBind();
                ddlMachine.Items.Insert(0, "ALL");
                ddlshift.DataSource = Bindshift();
                ddlshift.DataBind();
                ddlshift.Items.Insert(0, "Day");
                Bindalarm();
                BindGrid();
            }
        }

        private List<string> Bindshift()
        {
            ddldata = DataBaseAccess.shiftdetail();
            return ddldata;
        }

        private object BindMachine()
        {
            ddldata = DataBaseAccess.GetMachineInfo(ddlplant.SelectedValue);
            return ddldata;
        }

        private void Bindalarm()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<Lookup> alarmCategary = new List<Lookup>();
            Lookup lookup1 = new Lookup();
            lookup1.Name = "ALL";
            lookup1.Value = "ALL";
            alarmCategary.Add(lookup1);
            try
            {
                cmd = new SqlCommand("select * from Focas_AlarmCategory", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        Lookup lookup = new Lookup();
                        lookup.Name = rdr[1].ToString();
                        lookup.Value = rdr[0].ToString();
                        alarmCategary.Add(lookup);
                    }
                }
            }
            catch (Exception es)
            {
                Logger.WriteErrorLog(es.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            ddlalarmgrp.DataTextField = "Name";
            ddlalarmgrp.DataValueField = "Value";
            ddlalarmgrp.DataSource = alarmCategary;
            ddlalarmgrp.DataBind();

        }

        private List<string> Binddata(string name)
        {
            ddllist = DataBaseAccess.GetAlldetails(name);
            return ddllist;
        }

        private void BindGrid()
        {
            string S_fromdate, s_todate;
            string fromd = string.Empty, tod = string.Empty;
            DateTime from, to;
            string machineId = ddlMachine.Text.ToString() == "ALL" ? string.Empty : ddlMachine.Text.ToString();
            string alarmgroup = ddlalarmgrp.SelectedValue.ToString();
            string filterBy = ddlFilterBy.SelectedValue.ToString();
            string Shift = ddlshift.Text.ToString();
            string parameter;
            from = Util.GetDateTime(txtFromDate.Text);
            to = Util.GetDateTime(txtToDate.Text);
            S_fromdate = from.ToSQLDateTimeFormat();
            s_todate = to.ToSQLDateTimeFormat();
            parameter = radiobtn.SelectedValue.ToString();
            if (parameter == "Summary")
            {
                AlarmReportData = DataBaseAccess.AlarmReportSummaryData(S_fromdate, s_todate, machineId, alarmgroup, parameter, Shift, filterBy);
                GridViewAlaramHistory.Columns[7].Visible = false;
                GridViewAlaramHistory.Columns[8].Visible = false;
                GridViewAlaramHistory.Columns[9].Visible = false;
                GridViewAlaramHistory.Columns[4].Visible = true;
                GridViewAlaramHistory.Columns[5].Visible = true;
                GridViewAlaramHistory.Columns[6].Visible = true;
            }
            else
            {
                GridViewAlaramHistory.Columns[4].Visible = false;
                GridViewAlaramHistory.Columns[5].Visible = false;
                GridViewAlaramHistory.Columns[6].Visible = false;
                GridViewAlaramHistory.Columns[7].Visible = true;
                GridViewAlaramHistory.Columns[8].Visible = false;
                GridViewAlaramHistory.Columns[9].Visible = false;
                AlarmReportData = DataBaseAccess.AlarmReportDetailData(S_fromdate, s_todate, machineId, alarmgroup, parameter, Shift, filterBy);
            }
            if (ddlMachine.Text == "ALL")
            {
                GridViewAlaramHistory.Columns[1].Visible = true;
            }
            else
            {
                GridViewAlaramHistory.Columns[1].Visible = false;
            }
            GridViewAlaramHistory.DataSource = AlarmReportData;
            GridViewAlaramHistory.DataBind();

        }

        protected void view_Click(object sender, EventArgs e)
        {
            DateTime fromdate, todate;
            fromdate = Util.GetDateTime(txtFromDate.Text);
            todate = Util.GetDateTime(txtToDate.Text);
            if (fromdate > todate)
            {
                Response.Write("<script>alert('Fromdate is greater than to date.');</script>");
            }
            else
            {
                BindGrid();
            }
        }

        protected void Export_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> columnNames = new List<string>();
                string S_fromdate, s_todate;
                DateTime from, to;
                string reportName = string.Empty;
                string machineId = ddlMachine.Text.ToString() == "ALL" ? string.Empty : ddlMachine.Text.ToString();
                string alarmgroup = ddlalarmgrp.SelectedValue.ToString();
                string filterBy = ddlFilterBy.SelectedValue.ToString();
                string Shift = ddlshift.Text.ToString();
                string parameter;
                var dt = new DataTable();
                from = Util.GetDateTime(txtFromDate.Text);
                to = Util.GetDateTime(txtToDate.Text);
                S_fromdate = from.ToSQLDateTimeFormat();
                s_todate = to.ToSQLDateTimeFormat();
                parameter = radiobtn.SelectedValue.ToString();
                var returnValue = new DataTable();
                if (radiobtn.SelectedValue == "Summary")
                {
                    AlarmReportData = DataBaseAccess.AlarmReportSummaryData(S_fromdate, s_todate, machineId, alarmgroup, parameter, Shift, filterBy);
                    string templateFile = string.Empty;
                    string excelFilePath = string.Empty;
                    templateFile = Path.Combine(APP_PATH, "Alarm History\\" + "AlarmHistory_Summary.xlsx");
                    string GeneratedReportPath = Path.Combine(APP_PATH, "GeneratedReports");
                    if (!File.Exists(templateFile))
                    {
                        Logger.WriteErrorLog("Template doesnt exists");
                        return;
                    }
                    #region
                    if (!Directory.Exists(GeneratedReportPath))
                    {
                        try
                        {
                            Directory.CreateDirectory(GeneratedReportPath);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog(ex.Message.ToString());
                        }
                    }
                    string exportedReportFile = null;
                    exportedReportFile = Path.Combine(GeneratedReportPath, "AlarmHistory_Summary" + string.Format("{0:ddMMMyyyy_HHmmss}", DateTime.Now) + ".xlsx");
                    if (File.Exists(exportedReportFile))
                    {
                        var dirInfo = new DirectoryInfo(exportedReportFile);
                        dirInfo.Attributes &= ~FileAttributes.ReadOnly;
                        File.Delete(exportedReportFile);
                    }
                    File.Copy(templateFile, exportedReportFile, true);
                    #endregion
                    FileInfo newFile = new FileInfo(exportedReportFile);
                    ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                    ExcelPackage pck = new ExcelPackage(newFile, true);
                    var wsDt = pck.Workbook.Worksheets[1];

                    if (machineId == "")
                    {
                        wsDt.Cells["D3"].Value = "PLANT:" + ddlplant.Text;
                        wsDt.Cells["D3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        wsDt.Cells["B4"].Value = from.ToString("dd-MM-yyyy");
                        wsDt.Cells["E4"].Value = to.ToString("dd-MM-yyyy");
                        wsDt.Cells["G4"].Value = "ALL";
                    }
                    else
                    {
                        wsDt.Column(2).Hidden = true;
                        wsDt.Cells["D3"].Value = "PLANT:" + ddlplant.Text;
                        wsDt.Cells["D3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        wsDt.Cells["C4"].Value = from.ToString("dd-MM-yyyy");
                        wsDt.Cells["E4"].Value = to.ToString("dd-MM-yyyy");
                        wsDt.Cells["G4"].Value = ddlMachine.Text;
                    }
                    int i = 9, j = 1, k = 9, l = 1;
                    foreach (AlarmHistory entity in AlarmReportData)
                    {
                        j = 1;
                        wsDt.Cells[i, j].Value = entity.SLNO;
                        j++;
                        wsDt.Cells[i, j].Value = entity.MachineID;
                        j++;
                        wsDt.Cells[i, j].Value = entity.AlarmNo;
                        j++;
                        wsDt.Cells[i, j].Value = entity.Message;
                        j++;
                        wsDt.Cells[i, j].Value = entity.FirstOccurence;
                        j++;
                        wsDt.Cells[i, j].Value = entity.LastOccurence;
                        j++;
                        wsDt.Cells[i, j].Value = entity.NoOfOccur;
                        i++;
                    }
                    i--;
                    wsDt.Cells[k, l, i, j].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDt.Cells[k, l, i, j].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDt.Cells[k, l, i, j].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDt.Cells[k, l, i, j].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDt.Cells[k, l, i, j].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    wsDt.Cells[k, l, i, j].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    wsDt.Cells[k, l, i, j].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    wsDt.Cells[k, l, i, j].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    //pck.SaveAs(newFile);
                    DownloadMultipleFile(exportedReportFile, pck.GetAsByteArray());
                    Logger.WriteDebugLog("Alarm History Detail report saved.");
                    Response.Write("<script>alert('Alarm History Summary report saved.');</script>");

                }
                if (radiobtn.SelectedValue == "Details")
                {
                    AlarmReportData = DataBaseAccess.AlarmReportDetailData(S_fromdate, s_todate, machineId, alarmgroup, parameter, Shift, filterBy);
                    string templateFile = string.Empty;
                    string excelFilePath = string.Empty;
                    templateFile = Path.Combine(APP_PATH, "Alarm History\\" + "AlarmHistory_Detail.xlsx");
                    string GeneratedReportPath = Path.Combine(APP_PATH, "GeneratedReports");
                    if (!File.Exists(templateFile))
                    {
                        Logger.WriteErrorLog("Template doesnt exists");
                        return;
                    }
                    #region
                    if (!Directory.Exists(GeneratedReportPath))
                    {
                        try
                        {
                            Directory.CreateDirectory(GeneratedReportPath);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog(ex.Message.ToString());
                        }
                    }
                    string exportedReportFile = null;
                    exportedReportFile = Path.Combine(GeneratedReportPath, "AlarmHistory_Detail" + string.Format("{0:ddMMMyyyy_HHmmss}", DateTime.Now) + ".xlsx");
                    if (File.Exists(exportedReportFile))
                    {
                        var dirInfo = new DirectoryInfo(exportedReportFile);
                        dirInfo.Attributes &= ~FileAttributes.ReadOnly;
                        File.Delete(exportedReportFile);
                    }
                    File.Copy(templateFile, exportedReportFile, true);
                    #endregion
                    FileInfo newFile = new FileInfo(exportedReportFile);
                    ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                    ExcelPackage pck = new ExcelPackage(newFile, true);
                    var wsDt = pck.Workbook.Worksheets[1];
                    wsDt.Cells["D3"].Value = "PLANT:" + ddlplant.Text;
                    if (machineId == "")
                    {
                        wsDt.Cells["D3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        wsDt.Cells["B4"].Value = from.ToString("dd-MM-yyyy"); 
                        wsDt.Cells["E4"].Value = to.ToString("dd-MM-yyyy"); 
                        wsDt.Cells["G4"].Value = "ALL";
                    }
                    else
                    {
                        wsDt.Cells["D3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        wsDt.Column(2).Hidden = true;
                        wsDt.Cells["C4"].Value = from.ToString("dd-MM-yyyy");
                        wsDt.Cells["E4"].Value = to.ToString("dd-MM-yyyy");
                        wsDt.Cells["G4"].Value = ddlMachine.Text;
                    }
                    int i = 9, j = 1, k = 9, l = 1;
                    foreach (AlarmHistory entity in AlarmReportData)
                    {
                        j = 1;
                        wsDt.Cells[i, j].Value = entity.SLNO;
                        j++;
                        wsDt.Cells[i, j].Value = entity.MachineID;
                        j++;
                        wsDt.Cells[i, j].Value = entity.AlarmNo;
                        j++;
                        wsDt.Cells[i, j].Value = entity.Message;
                        j++;
                        wsDt.Cells[i, j].Value = entity.StartTime;
                        //j++;
                        //wsDt.Cells[i, j].Value = entity.Endtime;
                        //j++;
                        //wsDt.Cells[i, j].Value = entity.duration;
                        i++;
                    }
                    i--;
                    wsDt.Cells[k, l, i, j].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDt.Cells[k, l, i, j].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDt.Cells[k, l, i, j].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDt.Cells[k, l, i, j].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDt.Cells[k, l, i, j].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    wsDt.Cells[k, l, i, j].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    wsDt.Cells[k, l, i, j].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    wsDt.Cells[k, l, i, j].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    DownloadMultipleFile(exportedReportFile, pck.GetAsByteArray());
                    Logger.WriteDebugLog("Alarm History Detail report saved.");
                    Response.Write("<script>alert('Alarm History Detail report saved.');</script>");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
        }

        internal static void DownloadMultipleFile(string fileName, byte[] byteArray)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Path.GetFileName(fileName) + "\"");
            HttpContext.Current.Response.OutputStream.Write(byteArray, 0, byteArray.Length);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }
        protected void timerDataChange_Tick(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void btnTrigger_Click(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(DataBaseAccess.AutoRefreshData);
                if (value > 10000)
                    timerDataChange.Interval = value;
                else
                    timerDataChange.Interval = 10000;

                if (chkAutoBox.Checked)
                    timerDataChange.Enabled = true;
                else
                    timerDataChange.Enabled = false;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static AlarmHistory GetAlarmNoPDFDetails(string machine, string alarmNo)
        {
            AlarmHistory pageDetails = new AlarmHistory();
            try
            {
                pageDetails = DataBaseAccess.GetAlarmNoPageDetails(machine, alarmNo);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetAlarmNoPDFDetails: " + ex.ToString());
            }
            return pageDetails;
        }

        protected void GridViewAlaramHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["BajajPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {

                        e.Row.Cells[2].ForeColor = ColorTranslator.FromHtml("#4444c7");
                        e.Row.Cells[2].Attributes["onclick"] = "return AlarmNoClick(this);";
                        e.Row.Cells[2].Font.Underline = true;
                        e.Row.ToolTip = "Click to select this row.";
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GridViewAlaramHistory_RowDataBound: " + ex.Message);
            }
        }

        protected void ddlplant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlMachine.DataSource = BindMachine();
                ddlMachine.DataBind();
                ddlMachine.Items.Insert(0, "ALL");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}