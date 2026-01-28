using Elmah;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Web_TPMTrakDashboard.AceDesigners.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.AceDesigners
{
    public class AceGenerateReport
    {
        static readonly string appPath = HttpContext.Current.Server.MapPath("~/AceDesigners");
        #region "Get Report Template File Path"
        public static string GetReportPath(string reportName)
        {
            string src = string.Empty;
            if (HttpContext.Current.Session["Language"] == null)
                src = Path.Combine(appPath, "ReportTemplates", reportName);
            else
            {
                if (HttpContext.Current.Session["Language"].ToString() != "en")
                    src = Path.Combine(appPath, "ReportTemplates-" + HttpContext.Current.Session["Language"].ToString() + "", reportName);
                else
                    src = Path.Combine(appPath, "ReportTemplate", reportName);
            }
            return src;
        }
        #endregion
        public static string SafeFileName(string name)
        {
            StringBuilder str = new StringBuilder(name);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.ToString();
        }
        private static void DownloadMultipleFile(string fileName, byte[] byteArray)
        {
            try
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
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        private static void setBorderThin(ExcelWorksheet workSheet, int fromRow, int fromCol, int toRow, int toCol)
        {
            try
            {
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        #region ---- Scrap Entry Report ----
        internal static bool generateScheduleErrorReport(string fromDate, string toDate, string poNumber, string viewType, List<ScheduleImportErrorEntity> listData)
        {
            bool successfull = false;
            try
            {
                string Filename = "ScheduleErrorReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "ScheduleErrorReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Schedule Error Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    int rowStart = 6;
                    int colStart = 1;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    //Excel.Workbook.Worksheets.Delete("Sheet1");
                    if (listData != null && listData.Count > 0)
                    {
                        var workSheet = Excel.Workbook.Worksheets["Sheet1"];
                        if (viewType.Equals("Date", StringComparison.OrdinalIgnoreCase))
                        {
                            workSheet.Cells["A3"].Value = "From Date";
                            workSheet.Cells["B3"].Value = fromDate;
                            workSheet.Cells["C3"].Value = "To Date";
                            workSheet.Cells["D3"].Value = toDate;
                        }
                        else
                        {
                            workSheet.Cells["A3"].Value = "ProductionOrder";
                            workSheet.Cells["B3"].Value = poNumber;
                        }

                        for (int i = 0; i < listData.Count; i++)
                        {

                            colStart = 1;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].MachineDesc;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].ProductionOrder;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].CompID;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].OpnNo;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].ErroMsg;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].UpdatedTS;
                            rowStart++;
                        }
                        setBorderThin(workSheet, 6, 1, rowStart - 1, colStart);
                        workSheet.Cells[3, 1, rowStart, colStart].AutoFitColumns();
                    }
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    successfull = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return successfull;
        }
        #endregion

        #region ----- Event Log Report -----
        internal static string GenerateEventLogReport(string FromDate, string ToDate, string MachineID, string EventID)
        {
            DataTable dt_ICD = new DataTable();
            DataTable dt_BCD = new DataTable();
            DataTable dt_SBP = new DataTable();
            string Generated = string.Empty;
            try
            {
                bool MachineColumnVisibility = MachineID == "" ? false : (MachineID.Split(',').Length > 1 ? false : true);
                dt_ICD = AceDatabaseAccess.GetEventLogsDataReport(FromDate, ToDate, MachineID, EventID, out dt_BCD, out dt_SBP);
                if (dt_ICD.Rows.Count > 0 || dt_BCD.Rows.Count > 0 || dt_SBP.Rows.Count > 0)
                {
                    string Filename = "ACE_EventLogsReport.xlsx";
                    string Source = GetReportPath(Filename);
                    string Template = "ACE_EventLogsReport" + DateTime.Now + ".xlsx";
                    string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                    if (!File.Exists(Source))
                    {
                        Logger.WriteDebugLog("Schedule Error Report template does not exists at - " + Source);
                        Generated = "Template does not exist";
                    }
                    else
                    {
                        FileInfo file = new FileInfo(Source);
                        ExcelPackage excel = new ExcelPackage(file);
                        var sheet = excel.Workbook.Worksheets[1];

                        int RowStart = 3, colStart = 1, rowStartCopy = RowStart, colStartCopy = colStart, colEnd = 2 ;

                        if (dt_ICD.Rows.Count > 0)
                        {
                            int col = 2;
                            sheet.Cells[1, col].Value = "From Date";
                            sheet.Cells[1, ++col].Value = Util.GetDateTime(FromDate).ToString("dd-MM-yyyy HH:mm:ss");
                            sheet.Cells[1, ++col].Value = "To Date";
                            sheet.Cells[1, ++col].Value = Util.GetDateTime(ToDate).ToString("dd-MM-yyyy HH:mm:ss");
                            if (MachineColumnVisibility)
                            {
                                sheet.Cells[1, ++col].Value = "Machine";
                                sheet.Cells[1, ++col].Value = MachineID;
                            }
                            sheet.Cells[1, ++col].Value = "Event";
                            sheet.Cells[1, ++col].Value = EventID == "" ? "All" : EventID;
                            sheet.Cells[1, 2, 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells[1, 2, 1, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 176, 240));
                            sheet.Cells[1, 2, 1, col].Style.Font.Bold = true;
                            setBorderThin(sheet, 1, 2, 1, col);

                            sheet.Cells[RowStart, colStart, RowStart, colStart + colEnd].Merge = true;
                            sheet.Cells[RowStart, colStart].Style.Font.Bold = true;
                            sheet.Cells[RowStart, colStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet.Cells[RowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells[RowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(52, 84, 107));
                            sheet.Cells[RowStart, colStart].Value = dt_ICD.Rows[0]["EventID"].ToString();

                            RowStart++;
                            sheet.Cells[RowStart, colStart].Value = "Sl No";
                            colStart++;
                            if (!MachineColumnVisibility)
                            {
                                sheet.Cells[RowStart, colStart].Value = "Machine";
                                sheet.Column(colStart).Width = 25;
                                colStart++;
                                colEnd = 3;
                            }
                            sheet.Cells[RowStart, colStart].Value = "Time";
                            sheet.Column(colStart).Width = 25;
                            colStart++;
                            sheet.Cells[RowStart, colStart].Value = "Enabled/Disabled";
                            sheet.Column(colStart).Width = 20;

                            sheet.Cells[RowStart, colStart - colEnd, RowStart, colStart].Style.Font.Bold = true;
                            sheet.Cells[RowStart, colStart - colEnd, RowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells[RowStart, colStart - colEnd, RowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(46, 104, 134));
                            RowStart++;
                            var MachinesList = dt_ICD.AsEnumerable().Select(x => x["MachineID"].ToString()).Distinct().ToList();
                            foreach (var Machine in MachinesList)
                            {
                                //colStart = 1;
                                //sheet.Cells[RowStart, colStart, RowStart, colStart + 2].Merge = true;
                                //sheet.Cells[RowStart, colStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                //sheet.Cells[RowStart, colStart].Style.Font.Bold = true;
                                //sheet.Cells[RowStart, colStart].Value = Machine;
                                //RowStart++;
                                DataTable temp_dt = dt_ICD.AsEnumerable().Where(x => x["MachineID"].ToString().Equals(Machine, StringComparison.OrdinalIgnoreCase)).OrderBy(x => Util.GetDateTime(x["AlarmTime"].ToString()).ToString("dd-MM-yyyy hh:mm:ss tt")).CopyToDataTable();
                                foreach (DataRow row in temp_dt.Rows)
                                {
                                    colStart = 1;
                                    sheet.Cells[RowStart, colStart].Value = row["SlNo"].ToString();
                                    colStart++;
                                    if (!MachineColumnVisibility)
                                    {
                                        sheet.Cells[RowStart, colStart].Value = Machine;
                                        colStart++;
                                    }
                                    sheet.Cells[RowStart, colStart].Value = Util.GetDateTime(row["AlarmTime"].ToString()).ToString("dd-MM-yyyy hh:mm:ss tt");
                                    colStart++;
                                    sheet.Cells[RowStart, colStart].Value = row["EnableDisable"].ToString();

                                    RowStart++;
                                }
                            }

                            setBorderThin(sheet, rowStartCopy, colStartCopy, RowStart, colStart);
                        }

                        if (dt_BCD.Rows.Count > 0)
                        {
                            RowStart = rowStartCopy;
                            colStart += 2;
                            int temp_colStart = colStart;
                            sheet.Cells[RowStart, colStart, RowStart, colStart + colEnd].Merge = true;
                            sheet.Cells[RowStart, colStart].Style.Font.Bold = true;
                            sheet.Cells[RowStart, colStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet.Cells[RowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells[RowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(52, 84, 107));
                            sheet.Cells[RowStart, colStart].Value = dt_BCD.Rows[0]["EventID"].ToString();
                            RowStart++;
                            sheet.Cells[RowStart, colStart].Value = "Sl No";
                            colStart++;
                            if (!MachineColumnVisibility)
                            {
                                sheet.Cells[RowStart, colStart].Value = "Machine";
                                sheet.Column(colStart).Width = 25;
                                colStart++;
                            }
                            sheet.Cells[RowStart, colStart].Value = "Time";
                            sheet.Column(colStart).Width = 25;
                            colStart++;
                            sheet.Cells[RowStart, colStart].Value = "Enabled/Disabled";
                            sheet.Column(colStart).Width = 20;
                            sheet.Cells[RowStart, colStart - colEnd, RowStart, colStart].Style.Font.Bold = true;
                            sheet.Cells[RowStart, colStart - colEnd, RowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells[RowStart, colStart - colEnd, RowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(46, 104, 134));
                            RowStart++;
                            var MachinesList = dt_BCD.AsEnumerable().Select(x => x["MachineID"].ToString()).Distinct().ToList();
                            foreach (var Machine in MachinesList)
                            {
                                //colStart = temp_colStart;

                                //sheet.Cells[RowStart, colStart, RowStart, colStart + 2].Merge = true;
                                //sheet.Cells[RowStart, colStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                //sheet.Cells[RowStart, colStart].Style.Font.Bold = true;
                                //sheet.Cells[RowStart, colStart].Value = Machine;
                                //RowStart++;
                                DataTable temp_dt = dt_BCD.AsEnumerable().Where(x => x["MachineID"].ToString().Equals(Machine, StringComparison.OrdinalIgnoreCase)).OrderBy(x => Util.GetDateTime(x["AlarmTime"].ToString()).ToString("dd-MM-yyyy hh:mm:ss tt")).CopyToDataTable();
                                foreach (DataRow row in temp_dt.Rows)
                                {
                                    colStart = temp_colStart;
                                    sheet.Cells[RowStart, colStart].Value = row["SlNo"].ToString();
                                    colStart++;
                                    if (!MachineColumnVisibility)
                                    {
                                        sheet.Cells[RowStart, colStart].Value = Machine;
                                        colStart++;
                                    }
                                    sheet.Cells[RowStart, colStart].Value = Util.GetDateTime(row["AlarmTime"].ToString()).ToString("dd-MM-yyyy hh:mm:ss tt");
                                    colStart++;
                                    sheet.Cells[RowStart, colStart].Value = row["EnableDisable"].ToString();

                                    RowStart++;
                                }
                            }
                            setBorderThin(sheet, rowStartCopy, temp_colStart, RowStart, colStart);
                        }

                        if (dt_SBP.Rows.Count > 0)
                        {
                            RowStart = rowStartCopy;
                            colStart += 2;
                            int temp_colStart = colStart;
                            sheet.Cells[RowStart, colStart, RowStart, colStart + colEnd].Merge = true;
                            sheet.Cells[RowStart, colStart].Style.Font.Bold = true;
                            sheet.Cells[RowStart, colStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet.Cells[RowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells[RowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(52, 84, 107));
                            sheet.Cells[RowStart, colStart].Value = dt_SBP.Rows[0]["EventID"].ToString();
                            RowStart++;
                            sheet.Cells[RowStart, colStart].Value = "Sl No";
                            colStart++;
                            if (!MachineColumnVisibility)
                            {
                                sheet.Cells[RowStart, colStart].Value = "Machine";
                                sheet.Column(colStart).Width = 25;
                                colStart++;
                            }
                            sheet.Cells[RowStart, colStart].Value = "Time";
                            sheet.Column(colStart).Width = 25;
                            colStart++;
                            sheet.Cells[RowStart, colStart].Value = "Enabled/Disabled";
                            sheet.Column(colStart).Width = 20;
                            sheet.Cells[RowStart, colStart - colEnd, RowStart, colStart].Style.Font.Bold = true;
                            sheet.Cells[RowStart, colStart - colEnd, RowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells[RowStart, colStart - colEnd, RowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(46, 104, 134));
                            RowStart++;
                            var MachinesList = dt_SBP.AsEnumerable().Select(x => x["MachineID"].ToString()).Distinct().ToList();
                            foreach (var Machine in MachinesList)
                            {
                                //colStart = temp_colStart;

                                //sheet.Cells[RowStart, colStart, RowStart, colStart + 2].Merge = true;

                                //sheet.Cells[RowStart, colStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                //sheet.Cells[RowStart, colStart].Style.Font.Bold = true;
                                //sheet.Cells[RowStart, colStart].Value = Machine;
                                //RowStart++;
                                DataTable temp_dt = dt_SBP.AsEnumerable().Where(x => x["MachineID"].ToString().Equals(Machine, StringComparison.OrdinalIgnoreCase)).OrderBy(x => Util.GetDateTime(x["AlarmTime"].ToString()).ToString("dd-MM-yyyy hh:mm:ss tt")).CopyToDataTable();
                                foreach (DataRow row in temp_dt.Rows)
                                {
                                    colStart = temp_colStart;
                                    sheet.Cells[RowStart, colStart].Value = row["SlNo"].ToString();
                                    colStart++;
                                    if (!MachineColumnVisibility)
                                    {
                                        sheet.Cells[RowStart, colStart].Value = Machine;
                                        colStart++;
                                    }
                                    sheet.Cells[RowStart, colStart].Value = Util.GetDateTime(row["AlarmTime"].ToString()).ToString("dd-MM-yyyy hh:mm:ss tt");
                                    colStart++;
                                    sheet.Cells[RowStart, colStart].Value = row["EnableDisable"].ToString();

                                    RowStart++;
                                }
                            }
                            setBorderThin(sheet, rowStartCopy, temp_colStart, RowStart, colStart);
                        }

                        DownloadMultipleFile(destination, excel.GetAsByteArray());
                        Generated = "Generated";
                    }
                }
                else
                {
                    Generated = "Data not Found";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return Generated;
        }
        #endregion

    }
}