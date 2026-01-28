using Elmah;
using GEA_NonMachining.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;
using Web_TPMTrakDashboard.GEA.Models;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.WebAndon;
using Xceed.Words.NET;

namespace Web_TPMTrakDashboard.GEA
{
    public class GEAGenerateReport
    {
        static readonly string appPath = HttpContext.Current.Server.MapPath("~/GEA/Reports");
        static string appPathForReportOutput = HttpContext.Current.Server.MapPath("~/GEA/Reports/ReportsOutput/");
        static string BalancingProductionOrder = "";
        static string reportName = "";
        public static string reportNameForHeaderFooter = "";
        public static bool isHeaderFooterRequired = false;
        public static bool isPagePotrait = false;
        public static string SafeFileName(string name)
        {
            StringBuilder str = new StringBuilder(name);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.ToString();
        }

        private static void createReportOutput()
        {
            if (!Directory.Exists(appPathForReportOutput))
            {
                Directory.CreateDirectory(appPathForReportOutput);
            }
        }

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
                    src = Path.Combine(appPath, "ReportTemplates", reportName);
            }
            return src;
        }
        #endregion

        #region "Download File"
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
        #endregion

        #region Maintenance Checklist Report - GEA
        internal static bool GenerateWeeklyChklistReport(string lineId, string machineID, DataTable dtWeeklyChklistReportData, DataTable dtsecond, int Year, DataTable remarksDT)
        {
            bool successfull = false;
            try
            {
                string Filename = "YearlyMaintenanceChecklistReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "YearlyMaintenanceChecklistReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Yearly Maintenance Checklist Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    int rowStart = 8;
                    int colStart = 5;
                    int data_row_num = 1;
                    int totalWeekColCount = 65;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var wrkshtYearlyMaintenanceChklist = Excel.Workbook.Worksheets[1];
                    var wsRemarks = Excel.Workbook.Worksheets[2];
                    wrkshtYearlyMaintenanceChklist.Cells["B4"].Value = lineId;
                    wrkshtYearlyMaintenanceChklist.Cells["D4"].Value = machineID;
                    wsRemarks.Cells["B4"].Value = lineId;
                    wsRemarks.Cells["A5"].Value = "Machine Name :" + machineID;

                    System.Drawing.Image img = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(getGEALogoPath()));
                    ExcelPicture pic = wrkshtYearlyMaintenanceChklist.Drawings.AddPicture("geaLogo", img);
                    pic.SetPosition(0, 0, 0, 0);
                    pic.SetSize(250, 90);
                    pic = wsRemarks.Drawings.AddPicture("geaLogo", img);
                    pic.SetPosition(0, 0, 0, 0);
                    pic.SetSize(250, 90);

                    Dictionary<string, int> headerCounts = new Dictionary<string, int>();
                    List<string> weeksList = new List<string>();
                    wrkshtYearlyMaintenanceChklist.Cells["BB3"].Value = "Date : " + DateTime.Now.ToString("dd-MMM-yyyy");
                    if (dtWeeklyChklistReportData != null && dtWeeklyChklistReportData.Rows.Count > 0 && dtWeeklyChklistReportData.Columns.Count > 7)
                    {
                        List<string> spanHeaders = dtWeeklyChklistReportData.Columns.Cast<DataColumn>().Where(x => x.Ordinal > 6).Select(x => x.ColumnName.Contains("-") ? x.ColumnName.Split('-')[0] : x.ColumnName).ToList();
                        weeksList = dtWeeklyChklistReportData.Columns.Cast<DataColumn>().Where(x => x.Ordinal > 6).Select(x => x.ColumnName.Contains("-") ? x.ColumnName.Split('-')[1] : x.ColumnName).ToList();
                        headerCounts = spanHeaders.GroupBy(x => x).Select(x => new { Month = x.Key, Count = x.Count() }).ToDictionary(x => x.Month, x => x.Count);
                        try
                        {
                            foreach (KeyValuePair<string, int> keyValuePair in headerCounts)
                            {
                                string yearInTwoDigit = Year.ToString().Substring(2, 2);
                                wrkshtYearlyMaintenanceChklist.Cells[6, colStart].Value = keyValuePair.Key + "-" + yearInTwoDigit;
                                wrkshtYearlyMaintenanceChklist.Cells[6, colStart, 6, colStart + (keyValuePair.Value - 1)].Merge = true;
                                wrkshtYearlyMaintenanceChklist.Cells[6, colStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                colStart += keyValuePair.Value;
                            }
                            colStart = 5;
                            foreach (string weekName in weeksList)
                            {
                                string weekNameWithSpace = weekName.Insert(4, " ");
                                wrkshtYearlyMaintenanceChklist.Cells[7, colStart].Value = weekNameWithSpace;
                                wrkshtYearlyMaintenanceChklist.Cells[7, colStart].Style.TextRotation = 90;
                                colStart++;
                            }
                            wrkshtYearlyMaintenanceChklist.Cells[7, 5, 7, colStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            totalWeekColCount = colStart - 1;
                            wrkshtYearlyMaintenanceChklist.Cells[6, 5, 7, totalWeekColCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            wrkshtYearlyMaintenanceChklist.Cells[6, 5, 7, totalWeekColCount].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 0, 255));
                            wrkshtYearlyMaintenanceChklist.Cells[6, 5, 7, totalWeekColCount].Style.Font.Color.SetColor(Color.White);

                            for (int i = 0; i < dtWeeklyChklistReportData.Rows.Count; i++)
                            {
                                colStart = 5;
                                wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1].Value = data_row_num;
                                wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                wrkshtYearlyMaintenanceChklist.Cells[rowStart, 2].Value = dtWeeklyChklistReportData.Rows[i]["Chekpoints"];
                                wrkshtYearlyMaintenanceChklist.Cells[rowStart, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                wrkshtYearlyMaintenanceChklist.Cells[rowStart, 3].Value = dtWeeklyChklistReportData.Rows[i]["Method"];
                                wrkshtYearlyMaintenanceChklist.Cells[rowStart, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                wrkshtYearlyMaintenanceChklist.Cells[rowStart, 4].Value = dtWeeklyChklistReportData.Rows[i]["Criteria"];
                                wrkshtYearlyMaintenanceChklist.Cells[rowStart, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                for (int j = 7; j < dtWeeklyChklistReportData.Columns.Count; j++)
                                {
                                    if (dtWeeklyChklistReportData.Rows[i][j].ToString().Equals("1"))
                                    {
                                        var circle_done = wrkshtYearlyMaintenanceChklist.Drawings.AddShape("Circle_ActDone" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + i + j, eShapeStyle.FlowChartConnector);
                                        circle_done.Fill.Style = eFillStyle.SolidFill;
                                        circle_done.Fill.Transparancy = 20;
                                        circle_done.Border.Fill.Style = eFillStyle.SolidFill;
                                        circle_done.Border.LineStyle = eLineStyle.Solid;
                                        circle_done.Border.Width = 1;
                                        circle_done.Border.Fill.Color = Color.Black;
                                        circle_done.Border.LineCap = eLineCap.Round;
                                        circle_done.Fill.Color = ColorTranslator.FromHtml("#048204");
                                        circle_done.SetSize(13, 13);
                                        circle_done.SetPosition(rowStart - 1, 3, colStart - 1, 6);
                                    }
                                    if (dtWeeklyChklistReportData.Rows[i][j].ToString().Equals("2"))
                                    {
                                        var circle_notdone = wrkshtYearlyMaintenanceChklist.Drawings.AddShape("Circle_ActNotDone" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + i + j, eShapeStyle.FlowChartConnector);
                                        circle_notdone.Fill.Style = eFillStyle.SolidFill;
                                        circle_notdone.Fill.Transparancy = 20;
                                        circle_notdone.Border.Fill.Style = eFillStyle.SolidFill;
                                        circle_notdone.Border.LineStyle = eLineStyle.Solid;
                                        circle_notdone.Border.Width = 1;
                                        circle_notdone.Border.Fill.Color = Color.Black;
                                        circle_notdone.Border.LineCap = eLineCap.Round;
                                        circle_notdone.Fill.Color = ColorTranslator.FromHtml("#DF4006");
                                        circle_notdone.SetSize(13, 13);
                                        circle_notdone.SetPosition(rowStart - 1, 3, colStart - 1, 6);
                                    }
                                    if (dtWeeklyChklistReportData.Rows[i][j].ToString().Equals("3"))
                                    {
                                        var circle_chkdone_replaced = wrkshtYearlyMaintenanceChklist.Drawings.AddShape("Circle_ActChkDoneReplaced" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + i + j, eShapeStyle.FlowChartConnector);
                                        circle_chkdone_replaced.Fill.Style = eFillStyle.SolidFill;
                                        circle_chkdone_replaced.Fill.Transparancy = 20;
                                        circle_chkdone_replaced.Border.Fill.Style = eFillStyle.SolidFill;
                                        circle_chkdone_replaced.Border.LineStyle = eLineStyle.Solid;
                                        circle_chkdone_replaced.Border.Width = 1;
                                        circle_chkdone_replaced.Border.Fill.Color = Color.Black;
                                        circle_chkdone_replaced.Border.LineCap = eLineCap.Round;
                                        circle_chkdone_replaced.Fill.Color = ColorTranslator.FromHtml("#184abc");
                                        circle_chkdone_replaced.SetSize(13, 13);
                                        circle_chkdone_replaced.SetPosition(rowStart - 1, 3, colStart - 1, 6);
                                    }
                                    colStart++;
                                }
                                data_row_num++;
                                rowStart++;

                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog(ex.ToString());
                        }

                    }
                    wrkshtYearlyMaintenanceChklist.Cells[8, 1, rowStart, totalWeekColCount].Style.Font.Size = 9;
                    rowStart = rowStart + 3;

                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1, rowStart, 4].Merge = true;
                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1].Style.Font.Bold = true;
                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1].Value = "Sign of Operator";
                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rowStart++;
                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1, rowStart, 4].Merge = true;
                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1].Style.Font.Bold = true;
                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1].Value = "Sign of Superviser";
                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rowStart++;
                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1, rowStart, 4].Merge = true;
                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1].Value = "Sign of Maintenance Incharge";
                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1].Style.Font.Bold = true;
                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    int k = 5;
                    Dictionary<string, int> columns = new Dictionary<string, int>();
                    foreach (string week in weeksList)
                    {
                        if (columns.ContainsKey(week))
                            k++;
                        else
                            columns.Add(week, k++);
                    }
                    if (dtsecond != null && dtsecond.Rows.Count > 0)
                    {
                        wrkshtYearlyMaintenanceChklist.Row(rowStart).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        foreach (DataRow row in dtsecond.Rows)
                        {
                            wrkshtYearlyMaintenanceChklist.Cells[rowStart, columns["Week" + row["WeekNo"].ToString()]].Value = row["Name"].ToString();
                            wrkshtYearlyMaintenanceChklist.Cells[rowStart, columns["Week" + row["WeekNo"].ToString()]].Style.TextRotation = 90;
                            wrkshtYearlyMaintenanceChklist.Cells[rowStart, columns["Week" + row["WeekNo"].ToString()]].Style.Font.Bold = true;
                            int week = columns["Week" + row["WeekNo"].ToString()] + 1;
                            if (!(columns.ContainsValue(week)))
                            {
                                wrkshtYearlyMaintenanceChklist.Cells[rowStart, columns["Week" + row["WeekNo"].ToString()], rowStart, (columns["Week" + row["WeekNo"].ToString()] + 1)].Merge = true;

                            }
                        }
                    }
                    int rowBorderStart = 6;

                    //wrkshtYearlyMaintenanceChklist.Cells[5, 1, rowStart, 65].AutoFitColumns();

                    setThinBorder(wrkshtYearlyMaintenanceChklist, rowBorderStart, 1, rowStart, totalWeekColCount);


                    wrkshtYearlyMaintenanceChklist.Cells[1, 54, 1, totalWeekColCount].Merge = true;
                    wrkshtYearlyMaintenanceChklist.Cells[2, 54, 2, totalWeekColCount].Merge = true;
                    wrkshtYearlyMaintenanceChklist.Cells[3, 54, 3, totalWeekColCount].Merge = true;
                    wrkshtYearlyMaintenanceChklist.Cells[4, 54, 4, totalWeekColCount].Merge = true;

                    if (totalWeekColCount > 65)
                    {
                        setThinBorder(wrkshtYearlyMaintenanceChklist, 1, 66, 4, totalWeekColCount);
                    }


                    wrkshtYearlyMaintenanceChklist.Cells[1, totalWeekColCount, rowStart, totalWeekColCount].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    wrkshtYearlyMaintenanceChklist.Cells[1, totalWeekColCount, rowStart, totalWeekColCount].Style.Border.Right.Color.SetColor(Color.Black);

                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1, rowStart, totalWeekColCount].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                    wrkshtYearlyMaintenanceChklist.Cells[rowStart, 1, rowStart, totalWeekColCount].Style.Border.Bottom.Color.SetColor(Color.Black);


                    #region --- Remarks Sheet ------------
                    rowStart = 8;
                    colStart = 5;
                    data_row_num = 1;
                    totalWeekColCount = 65;
                    headerCounts = new Dictionary<string, int>();
                    weeksList = new List<string>();
                    if (remarksDT != null && remarksDT.Rows.Count > 0 && remarksDT.Columns.Count > 7)
                    {
                        List<string> spanHeaders = remarksDT.Columns.Cast<DataColumn>().Where(x => x.Ordinal > 6).Select(x => x.ColumnName.Contains("-") ? x.ColumnName.Split('-')[0] : x.ColumnName).ToList();
                        weeksList = remarksDT.Columns.Cast<DataColumn>().Where(x => x.Ordinal > 6).Select(x => x.ColumnName.Contains("-") ? x.ColumnName.Split('-')[1] : x.ColumnName).ToList();
                        headerCounts = spanHeaders.GroupBy(x => x).Select(x => new { Month = x.Key, Count = x.Count() }).ToDictionary(x => x.Month, x => x.Count);
                        try
                        {
                            foreach (KeyValuePair<string, int> keyValuePair in headerCounts)
                            {
                                string yearInTwoDigit = Year.ToString().Substring(2, 2);
                                wsRemarks.Cells[6, colStart].Value = keyValuePair.Key + "-" + yearInTwoDigit;
                                wsRemarks.Cells[6, colStart, 6, colStart + (keyValuePair.Value - 1)].Merge = true;
                                wsRemarks.Cells[6, colStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                colStart += keyValuePair.Value;
                            }
                            colStart = 5;
                            foreach (string weekName in weeksList)
                            {
                                string weekNameWithSpace = weekName.Insert(4, " ");
                                wsRemarks.Cells[7, colStart].Value = weekNameWithSpace;
                                wsRemarks.Cells[7, colStart].Style.TextRotation = 90;
                                colStart++;
                            }
                            wsRemarks.Cells[7, 5, 7, colStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            totalWeekColCount = colStart - 1;
                            wsRemarks.Cells[6, 5, 7, totalWeekColCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            wsRemarks.Cells[6, 5, 7, totalWeekColCount].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 0, 255));
                            wsRemarks.Cells[6, 5, 7, totalWeekColCount].Style.Font.Color.SetColor(Color.White);

                            for (int i = 0; i < remarksDT.Rows.Count; i++)
                            {
                                colStart = 5;
                                wsRemarks.Cells[rowStart, 1].Value = data_row_num;
                                wsRemarks.Cells[rowStart, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                wsRemarks.Cells[rowStart, 2].Value = remarksDT.Rows[i]["Chekpoints"];
                                wsRemarks.Cells[rowStart, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                wsRemarks.Cells[rowStart, 3].Value = remarksDT.Rows[i]["Method"];
                                wsRemarks.Cells[rowStart, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                wsRemarks.Cells[rowStart, 4].Value = remarksDT.Rows[i]["Criteria"];
                                wsRemarks.Cells[rowStart, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                for (int j = 7; j < remarksDT.Columns.Count; j++)
                                {
                                    wsRemarks.Cells[rowStart, colStart].Value = remarksDT.Rows[i][j].ToString();
                                    colStart++;
                                }
                                data_row_num++;
                                rowStart++;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog(ex.ToString());
                        }

                    }
                    wsRemarks.Cells[8, 1, rowStart, totalWeekColCount].Style.Font.Size = 9;
                    rowBorderStart = 6;
                    setThinBorder(wsRemarks, rowBorderStart, 1, rowStart, totalWeekColCount);
                    if (totalWeekColCount > 65)
                    {
                        wsRemarks.Cells[1, 66, 3, totalWeekColCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        wsRemarks.Cells[1, 66, 3, totalWeekColCount].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 0, 255));
                    }
                    wsRemarks.Cells[1, totalWeekColCount, rowStart, totalWeekColCount].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    wsRemarks.Cells[1, totalWeekColCount, rowStart, totalWeekColCount].Style.Border.Right.Color.SetColor(Color.Black);
                    wsRemarks.Cells[rowStart, 1, rowStart, totalWeekColCount].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                    wsRemarks.Cells[rowStart, 1, rowStart, totalWeekColCount].Style.Border.Bottom.Color.SetColor(Color.Black);
                    #endregion

                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    successfull = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return successfull;
        }
        private static void setThinBorder(ExcelWorksheet excelWorksheet, int fromRow, int fromCol, int toRow, int toCol)
        {
            excelWorksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Color.SetColor(Color.Black);
            excelWorksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Color.SetColor(Color.Black);
            excelWorksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Color.SetColor(Color.Black);
            excelWorksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Color.SetColor(Color.Black);
        }
        #endregion

        #region Maintenance Daily Checklist Report - GEA
        internal static bool GenerateDailyChklistReport(string lineId, string machineID, string month, int year, DataTable dtDailylyChklistReportData, DataTable dtSuperisorMainTainanceData, DataTable dtRemarks)
        {
            bool successfull = false;
            try
            {
                string Filename = "DailyMaintenanceChecklistReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "DailyMaintenanceChecklistReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Daily Maintenance Checklist Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    int rowStart;
                    int colStart = 5;
                    int data_row_num;
                    int Colmonth = 0;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var wrkshtDailyMaintenanceChklist1 = Excel.Workbook.Worksheets[1];
                    var wsRemarksCopy = Excel.Workbook.Worksheets[2];
                    if (dtDailylyChklistReportData != null && dtDailylyChklistReportData.Rows.Count > 0)
                    {
                        var distinctMachineID = dtDailylyChklistReportData.AsEnumerable().Select(s => s.Field<string>("Machineid")).Distinct();
                        List<string> spanHeaders = dtDailylyChklistReportData.Columns.Cast<DataColumn>().Where(x => x.Ordinal > 6).Select(x => x.ColumnName.Contains("-") ? x.ColumnName.Split('-')[2] : x.ColumnName).ToList();

                        foreach (string header in spanHeaders)
                        {
                            wrkshtDailyMaintenanceChklist1.Cells[6, colStart].Value = Convert.ToInt32(header);
                            colStart++;
                        }
                        wrkshtDailyMaintenanceChklist1.Cells[6, colStart].Value = "Remarks";
                        wrkshtDailyMaintenanceChklist1.Cells[6, colStart].AutoFitColumns();

                        string workSheetName = Excel.Workbook.Worksheets[1].Name;
                        ExcelWorksheet pck = Excel.Workbook.Worksheets[1];

                        foreach (string machineid in distinctMachineID)
                        {
                            rowStart = 7;
                            data_row_num = 1;
                            var wrkshtDailyMaintenanceChklist = Excel.Workbook.Worksheets.Add(machineid, pck);

                            System.Drawing.Image img = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(getGEALogoPath()));
                            ExcelPicture pic = wrkshtDailyMaintenanceChklist.Drawings.AddPicture("geaLogo", img);
                            pic.SetPosition(0, 0, 0, 0);
                            pic.SetSize(380, 80);

                            wrkshtDailyMaintenanceChklist.Cells["AG3"].Value = "Date: " + DateTime.Now.ToString("dd-MMMM-yyyy");
                            wrkshtDailyMaintenanceChklist.Cells["A4"].Value = wrkshtDailyMaintenanceChklist.Cells["A4"].Value.ToString() + " " + lineId;
                            wrkshtDailyMaintenanceChklist.Cells["C4"].Value = wrkshtDailyMaintenanceChklist.Cells["C4"].Value.ToString() + " " + machineid;
                            wrkshtDailyMaintenanceChklist.Cells["K4"].Value = wrkshtDailyMaintenanceChklist.Cells["K4"].Value.ToString() + " " + month;
                            wrkshtDailyMaintenanceChklist.Cells["S4"].Value = wrkshtDailyMaintenanceChklist.Cells["S4"].Value.ToString() + " " + year;
                            for (int i = 0; i < dtDailylyChklistReportData.Rows.Count; i++)
                            {
                                if (dtDailylyChklistReportData.Rows[i]["Machineid"].ToString() == machineid)
                                {
                                    colStart = 5;
                                    wrkshtDailyMaintenanceChklist.Cells[rowStart, 1].Value = data_row_num;
                                    wrkshtDailyMaintenanceChklist.Cells[rowStart, 2].Value = dtDailylyChklistReportData.Rows[i]["Activity"];
                                    wrkshtDailyMaintenanceChklist.Cells[rowStart, 3].Value = dtDailylyChklistReportData.Rows[i]["Method"];
                                    wrkshtDailyMaintenanceChklist.Cells[rowStart, 4].Value = dtDailylyChklistReportData.Rows[i]["Criteria"];
                                    for (int j = 7; j < dtDailylyChklistReportData.Columns.Count; j++)
                                    {
                                        if (!string.IsNullOrEmpty(dtDailylyChklistReportData.Rows[i][j].ToString()))
                                            wrkshtDailyMaintenanceChklist.Cells[rowStart, colStart].Value = dtDailylyChklistReportData.Rows[i][j].ToString();
                                        colStart++;
                                    }
                                    rowStart++;
                                    data_row_num++;
                                }
                            }
                            wrkshtDailyMaintenanceChklist.Cells[rowStart, 1, rowStart, 4].Merge = true;
                            wrkshtDailyMaintenanceChklist.Cells[rowStart, 1, rowStart, 4].Value = "Sign of Operator";
                            wrkshtDailyMaintenanceChklist.Cells[rowStart, 1, rowStart, 4].Style.Font.Bold = true;
                            rowStart++;
                            wrkshtDailyMaintenanceChklist.Cells[rowStart, 1, rowStart, 4].Merge = true;
                            wrkshtDailyMaintenanceChklist.Cells[rowStart, 1, rowStart, 4].Value = "Sign of Supervisor";
                            wrkshtDailyMaintenanceChklist.Cells[rowStart, 1, rowStart, 4].Style.Font.Bold = true;
                            rowStart++;
                            wrkshtDailyMaintenanceChklist.Cells[rowStart, 1, rowStart, 4].Merge = true;
                            wrkshtDailyMaintenanceChklist.Cells[rowStart, 1, rowStart, 4].Value = "Sign of Maintenance Incharge";
                            wrkshtDailyMaintenanceChklist.Cells[rowStart, 1, rowStart, 4].Style.Font.Bold = true;

                            wrkshtDailyMaintenanceChklist.Cells[7, 1, rowStart, 36].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            wrkshtDailyMaintenanceChklist.Cells[7, 1, rowStart, 36].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            wrkshtDailyMaintenanceChklist.Cells[7, 1, rowStart, 36].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            wrkshtDailyMaintenanceChklist.Cells[7, 1, rowStart, 36].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            wrkshtDailyMaintenanceChklist.Cells[7, 1, rowStart, 36].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                            wrkshtDailyMaintenanceChklist.Cells[7, 1, rowStart, 36].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                            wrkshtDailyMaintenanceChklist.Cells[7, 1, rowStart, 36].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                            wrkshtDailyMaintenanceChklist.Cells[7, 1, rowStart, 36].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);

                            rowStart++;
                            wrkshtDailyMaintenanceChklist.Cells[rowStart, 1].Value = "Pictures :";
                            wrkshtDailyMaintenanceChklist.Cells[rowStart, 1].Style.Font.Bold = true;

                            wrkshtDailyMaintenanceChklist.Cells[rowStart + 4, 1, rowStart + 4, 36].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            wrkshtDailyMaintenanceChklist.Cells[rowStart + 4, 1, rowStart + 4, 36].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                            wrkshtDailyMaintenanceChklist.Cells[1, 36, rowStart + 4, 36].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            wrkshtDailyMaintenanceChklist.Cells[1, 36, rowStart + 4, 36].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                            if (dtSuperisorMainTainanceData != null && dtSuperisorMainTainanceData.Rows.Count > 0)
                            {
                                DataTable dtMachineData = dtSuperisorMainTainanceData.AsEnumerable().Where(x => x.Field<string>("machineid").Equals(machineid)).CopyToDataTable();
                                if (dtMachineData != null && dtMachineData.Rows.Count > 0)
                                {
                                    Dictionary<int, int> ColValuePair = new Dictionary<int, int>();
                                    DateTime Date = Util.GetDateTime("01-" + DateTime.Now.Month.ToString("00") + "-" + DateTime.Now.Year.ToString());
                                    Colmonth = DateTime.Now.Month;
                                    int col = 5;
                                    while ((Date.Month == Colmonth))
                                    {
                                        ColValuePair.Add(Date.Day, col++);
                                        Date = Date.AddDays(1);
                                    }
                                    rowStart = rowStart - 3;
                                    //rowStart++;
                                    if (dtMachineData.AsEnumerable().Where(x => x.Field<string>("Role").Equals("Operator")).Count() > 0)
                                    {
                                        DataTable dtOperator = dtMachineData.AsEnumerable().Where(x => x.Field<string>("Role").Equals("Operator")).CopyToDataTable();
                                        foreach (DataRow Row in dtOperator.Rows)
                                        {
                                            DateTime SupCheckedDate = Convert.ToDateTime(Row["Date"].ToString());
                                            wrkshtDailyMaintenanceChklist.Cells[rowStart, ColValuePair[SupCheckedDate.Day]].Value = Row["Name"].ToString();
                                        }
                                    }
                                    rowStart++;
                                    if (dtMachineData.AsEnumerable().Where(x => x.Field<string>("Role").Equals("Supervisor")).Count() > 0)
                                    {
                                        DataTable dtsupervisor = dtMachineData.AsEnumerable().Where(x => x.Field<string>("Role").Equals("Supervisor")).CopyToDataTable();
                                        foreach (DataRow Row in dtsupervisor.Rows)
                                        {
                                            DateTime SupCheckedDate = Convert.ToDateTime(Row["Date"].ToString());
                                            wrkshtDailyMaintenanceChklist.Cells[rowStart, ColValuePair[SupCheckedDate.Day]].Value = Row["Name"].ToString();
                                        }
                                    }
                                    rowStart++;
                                    if (dtMachineData.AsEnumerable().Where(x => x.Field<string>("Role").Equals("MaintainanceEngineer")).Count() > 0)
                                    {
                                        DataTable dtMaintainance = dtMachineData.AsEnumerable().Where(x => x.Field<string>("Role").Equals("MaintainanceEngineer")).CopyToDataTable();
                                        foreach (DataRow Row in dtMaintainance.Rows)
                                        {
                                            DateTime SupCheckedDate = Convert.ToDateTime(Row["Date"].ToString());
                                            wrkshtDailyMaintenanceChklist.Cells[rowStart, ColValuePair[SupCheckedDate.Day]].Value = Row["Name"].ToString();
                                        }
                                    }
                                    wrkshtDailyMaintenanceChklist.Cells[7, 1, rowStart, 40].AutoFitColumns();
                                }
                            }
                            else
                                wrkshtDailyMaintenanceChklist.Cells[7, 1, rowStart, 40].AutoFitColumns();
                        }
                        Excel.Workbook.Worksheets.Delete("Daily");
                    }

                    if (dtRemarks != null && dtRemarks.Rows.Count > 0)
                    {

                        colStart = 5;
                        Colmonth = 0;

                        var distinctMachineID = dtRemarks.AsEnumerable().Select(s => s.Field<string>("Machineid")).Distinct();
                        List<string> spanHeaders = dtRemarks.Columns.Cast<DataColumn>().Where(x => x.Ordinal > 6).Select(x => x.ColumnName.Contains("-") ? x.ColumnName.Split('-')[2] : x.ColumnName).ToList();

                        foreach (string header in spanHeaders)
                        {
                            wsRemarksCopy.Cells[6, colStart].Value = Convert.ToInt32(header);
                            colStart++;
                        }
                        wsRemarksCopy.Cells[6, colStart].Value = "Remarks";
                        wsRemarksCopy.Cells[6, colStart].AutoFitColumns();

                        string workSheetName = Excel.Workbook.Worksheets[1].Name;
                        ExcelWorksheet pck = Excel.Workbook.Worksheets[1];

                        foreach (string machineid in distinctMachineID)
                        {
                            rowStart = 7;
                            data_row_num = 1;
                            var wsRemarks = Excel.Workbook.Worksheets.Add(machineid + "-Remarks", pck);

                            System.Drawing.Image img = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(getGEALogoPath()));
                            ExcelPicture pic = wsRemarks.Drawings.AddPicture("geaLogo" + machineid, img);
                            pic.SetPosition(0, 0, 0, 0);
                            pic.SetSize(380, 80);

                            wsRemarks.Cells["AG3"].Value = "Date: " + DateTime.Now.ToString("dd-MMMM-yyyy");
                            wsRemarks.Cells["A4"].Value = wsRemarks.Cells["A4"].Value.ToString() + " " + lineId;
                            wsRemarks.Cells["C4"].Value = wsRemarks.Cells["C4"].Value.ToString() + " " + machineid;
                            wsRemarks.Cells["K4"].Value = wsRemarks.Cells["K4"].Value.ToString() + " " + month;
                            wsRemarks.Cells["S4"].Value = wsRemarks.Cells["S4"].Value.ToString() + " " + year;
                            for (int i = 0; i < dtRemarks.Rows.Count; i++)
                            {
                                if (dtRemarks.Rows[i]["Machineid"].ToString() == machineid)
                                {
                                    colStart = 5;
                                    wsRemarks.Cells[rowStart, 1].Value = data_row_num;
                                    wsRemarks.Cells[rowStart, 2].Value = dtRemarks.Rows[i]["Activity"];
                                    wsRemarks.Cells[rowStart, 3].Value = dtRemarks.Rows[i]["Method"];
                                    wsRemarks.Cells[rowStart, 4].Value = dtRemarks.Rows[i]["Criteria"];
                                    for (int j = 7; j < dtRemarks.Columns.Count; j++)
                                    {
                                        if (!string.IsNullOrEmpty(dtRemarks.Rows[i][j].ToString()))
                                            wsRemarks.Cells[rowStart, colStart].Value = dtRemarks.Rows[i][j].ToString();
                                        colStart++;
                                    }
                                    rowStart++;
                                    data_row_num++;
                                }
                            }


                            wsRemarks.Cells[7, 1, rowStart, 36].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            wsRemarks.Cells[7, 1, rowStart, 36].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            wsRemarks.Cells[7, 1, rowStart, 36].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            wsRemarks.Cells[7, 1, rowStart, 36].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            wsRemarks.Cells[7, 1, rowStart, 36].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                            wsRemarks.Cells[7, 1, rowStart, 36].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                            wsRemarks.Cells[7, 1, rowStart, 36].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                            wsRemarks.Cells[7, 1, rowStart, 36].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                            rowStart++;

                        }
                        Excel.Workbook.Worksheets.Delete("Remarks");
                    }

                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    successfull = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return successfull;
        }

        internal static string GenerateProductionStatusReport(DateTime FromDate, DateTime ToDate, string fabNumber, string proType)
        {
            string successfull = "";
            try
            {
                string Filename = "ProductionOrderStatus.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "ProductionOrderStatus" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("ProductionOrderStatus Report template does not exists at - " + Source);
                    successfull = "TemplateNotFound";
                }
                else
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[1];
                    DataTable dt = GEADatabaseAccess.GetProdOrderStatus(proType, FromDate, ToDate, fabNumber);
                    if (dt.Rows.Count > 0)
                    {
                        worksheet.Cells["B4"].Value = FromDate.ToString("dd-MMM-yyyy");
                        worksheet.Cells["E4"].Value = ToDate.ToString("dd-MMM-yyyy");
                        List<string> ProdOrderList = dt.AsEnumerable().Select(x => x.Field<string>("ProductionOrder")).Distinct().ToList();
                        int row = 7;
                        foreach (string ProdOrder in ProdOrderList)
                        {
                            DataTable dtProdOrder = dt.AsEnumerable().Where(z => z.Field<string>("ProductionOrder").Equals(ProdOrder)).CopyToDataTable();
                            string FabNumber = dtProdOrder.Rows[0]["FabricationNo"].ToString();
                            foreach (DataRow dtrow in dtProdOrder.Rows)
                            {
                                switch (dtrow["Department"].ToString())
                                {
                                    case "Operator":
                                        {
                                            worksheet.Cells[row, 1].Value = ProdOrder;
                                            worksheet.Cells[row, 2].Value = FabNumber;
                                            worksheet.Cells[row, 3].Value = dtrow["Department"];
                                            worksheet.Cells[row, 4].Value = dtrow["Assembly-1"].ToString().Equals("1") ? "Yes" : dtrow["Assembly-1"].ToString().Equals("0") ? "No" : "-";
                                            worksheet.Cells[row, 5].Value = dtrow["Assembly-2"].ToString().Equals("1") ? "Yes" : dtrow["Assembly-2"].ToString().Equals("0") ? "No" : "-";
                                            worksheet.Cells[row, 6].Value = dtrow["Testing"].ToString().Equals("1") ? "Yes" : "No";
                                            worksheet.Cells[row, 7].Value = dtrow["Packing"].ToString().Equals("1") ? "Yes" : "No";
                                            worksheet.Cells[row, 8].Value = dtrow["Balancing"].ToString().Equals("1") ? "Yes" : "No";
                                            break;
                                        }
                                    case "Logistics":
                                        {
                                            worksheet.Cells[row + 3, 1].Value = ProdOrder;
                                            worksheet.Cells[row + 3, 2].Value = FabNumber;
                                            worksheet.Cells[row + 3, 3].Value = dtrow["Department"];
                                            worksheet.Cells[row + 3, 4].Value = "-";
                                            worksheet.Cells[row + 3, 5].Value = "-";
                                            worksheet.Cells[row + 3, 6].Value = "-";
                                            worksheet.Cells[row + 3, 7].Value = dtrow["Packing"].ToString().Equals("1") ? "Yes" : "No";
                                            worksheet.Cells[row + 3, 8].Value = "-";
                                            break;
                                        }
                                    case "Quality":
                                        {
                                            worksheet.Cells[row + 2, 1].Value = ProdOrder;
                                            worksheet.Cells[row + 2, 2].Value = FabNumber;
                                            worksheet.Cells[row + 2, 3].Value = dtrow["Department"];
                                            worksheet.Cells[row + 2, 4].Value = dtrow["Assembly-1"].ToString().Equals("1") ? "Yes" : dtrow["Assembly-1"].ToString().Equals("0") ? "No" : "-";
                                            worksheet.Cells[row + 2, 5].Value = dtrow["Assembly-2"].ToString().Equals("1") ? "Yes" : dtrow["Assembly-2"].ToString().Equals("0") ? "No" : "-";
                                            worksheet.Cells[row + 2, 6].Value = dtrow["Testing"].ToString().Equals("1") ? "Yes" : "No";
                                            worksheet.Cells[row + 2, 7].Value = dtrow["Packing"].ToString().Equals("1") ? "Yes" : "No";
                                            worksheet.Cells[row + 2, 8].Value = "-";
                                            break;
                                        }
                                }
                            }

                            worksheet.Cells[row + 1, 1].Value = ProdOrder;
                            worksheet.Cells[row + 1, 2].Value = FabNumber;
                            worksheet.Cells[row + 1, 3].Value = "Supervisor";
                            //worksheet.Cells[row + 1, 4].Value = GEADatabaseAccess.GetAssemblyReprotData("Assembly-1", FabNumber, ProdOrder);
                            //worksheet.Cells[row + 1, 5].Value = GEADatabaseAccess.GetAssemblyReprotData("Assembly-2", FabNumber, ProdOrder);
                            worksheet.Cells[row + 1, 4].Value = "-";
                            worksheet.Cells[row + 1, 5].Value = "-";
                            worksheet.Cells[row + 1, 6].Value = GEADatabaseAccess.GetAssemblyReprotData("Testing", FabNumber, ProdOrder);
                            worksheet.Cells[row + 1, 7].Value = "-";
                            worksheet.Cells[row + 1, 8].Value = "-";
                            worksheet.Cells[row, 1, row + 3, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, 1, row + 3, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, 1, row + 3, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, 1, row + 3, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, 1, row + 3, 8].Style.Border.Top.Color.SetColor(Color.Black);
                            worksheet.Cells[row, 1, row + 3, 8].Style.Border.Bottom.Color.SetColor(Color.Black);
                            worksheet.Cells[row, 1, row + 3, 8].Style.Border.Left.Color.SetColor(Color.Black);
                            worksheet.Cells[row, 1, row + 3, 8].Style.Border.Right.Color.SetColor(Color.Black);
                            worksheet.Cells[row, 1, row + 3, 8].Style.Border.BorderAround(ExcelBorderStyle.Thick);

                            worksheet.Cells[row, 1, row + 3, 1].Merge = true;
                            worksheet.Cells[row, 2, row + 3, 2].Merge = true;
                            row = row + 4;
                        }
                        //worksheet.Cells[6, 1, row+4, 7].AutoFitColumns();
                        DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return successfull;
        }


        internal static string DPHUReport(DateTime fromDate, DateTime ToDate, string MachineID)
        {
            string successfull = "";
            try
            {
                string Filename = "";
                string templateName = "";
                if (MachineID.Equals("Quality Incoming"))
                {
                    Filename = "DPHU_IncomingReport.xlsx";
                    templateName = "DPHU Quality Incoming";
                }
                else if (MachineID.Equals("Quality In house"))
                {
                    Filename = "DPHU_InHouseReport.xlsx";
                    templateName = "DPHU Quality In-house";
                }

                string Source = GetReportPath(Filename);
                string Template = templateName + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("DPHU Report template does not exists at - " + Source);
                    successfull = "TemplateNotFound";
                }
                else
                {

                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[1];
                    DataTable dt = GEADatabaseAccess.GetQuaityDPHUReport(fromDate, ToDate, MachineID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (MachineID.Equals("Quality In house"))
                        {
                            worksheet.Cells["B4"].Value = fromDate.ToString("dd-MM-yyyy");
                            worksheet.Cells["B5"].Value = ToDate.ToString("dd-MM-yyyy");
                            int row = 7, slno = 0;
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                worksheet.Cells[row, 1].Value = ++slno;
                                worksheet.Cells[row, 2].Value = dtrow["ScheduleDate"];
                                DateTime ScheduleDateTime = Convert.ToDateTime(dtrow["ScheduleDate"]);
                                worksheet.Cells[row, 3].Value = dtrow["ProductionOrderNo"];
                                worksheet.Cells[row, 4].Value = dtrow["ScheduleMonth"] + "-" + ScheduleDateTime.Year.ToString().Substring(2, 2);
                                worksheet.Cells[row, 5].Value = dtrow["PartNo"];
                                worksheet.Cells[row, 6].Value = dtrow["CompDescription"];
                                worksheet.Cells[row, 7].Value = dtrow["ScheduleQty"];
                                worksheet.Cells[row, 8].Value = dtrow["Identity_SerialNo"];
                                worksheet.Cells[row, 9].Value = "";
                                worksheet.Cells[row, 10].Value = "";
                                worksheet.Cells[row, 11].Value = dtrow["Phenomenon"];
                                worksheet.Cells[row, 12].Value = dtrow["InspectionDate"];
                                worksheet.Cells[row, 13].Value = dtrow["EmployeeName"];
                                worksheet.Cells[row, 14].Value = dtrow["IndicationOfStatus"];
                                worksheet.Cells[row, 15].Value = dtrow["AcceptedQty"];
                                worksheet.Cells[row, 16].Value = dtrow["ReworkQty"];
                                worksheet.Cells[row, 17].Value = dtrow["RejectedQty"];
                                worksheet.Cells[row, 18].Value = dtrow["DeviationQty"];
                                List<string> ProblemDesc = new List<string>();
                                int reasonNum = 0;
                                if (dtrow["Reason1"] != null && dtrow["Reason1"].ToString() != "")
                                {
                                    reasonNum++;
                                    ProblemDesc.Add(reasonNum + ". " + dtrow["Reason1"].ToString());
                                }
                                if (dtrow["Reason2"] != null && dtrow["Reason2"].ToString() != "")
                                {
                                    reasonNum++;
                                    ProblemDesc.Add(reasonNum + ". " + dtrow["Reason2"].ToString());
                                }
                                if (dtrow["Reason3"] != null && dtrow["Reason3"].ToString() != "")
                                {
                                    reasonNum++;
                                    ProblemDesc.Add(reasonNum + ". " + dtrow["Reason3"].ToString());
                                }
                                if (dtrow["Reason4"] != null && dtrow["Reason4"].ToString() != "")
                                {
                                    reasonNum++;
                                    ProblemDesc.Add(reasonNum + ". " + dtrow["Reason4"].ToString());
                                }
                                if (dtrow["Reason5"] != null && dtrow["Reason5"].ToString() != "")
                                {
                                    reasonNum++;
                                    ProblemDesc.Add(reasonNum + ". " + dtrow["Reason5"].ToString());
                                }
                                worksheet.Cells[row, 19].Value = String.Join("\n", ProblemDesc);
                                worksheet.Cells[row, 19].Style.WrapText = true;
                                worksheet.Cells[row, 20].Value = dtrow["RootCause"];
                                worksheet.Cells[row, 21].Value = dtrow["ActionNCR"];
                                worksheet.Cells[row, 22].Value = dtrow["Responsible"];
                                worksheet.Cells[row, 23].Value = dtrow["Status"];
                                worksheet.Cells[row++, 24].Value = dtrow["TargetDate"];
                            }
                            row--;
                            worksheet.Cells[7, 1, row, 24].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[7, 1, row, 24].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[7, 1, row, 24].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[7, 1, row, 24].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[7, 1, row, 24].Style.Border.Top.Color.SetColor(Color.Black);
                            worksheet.Cells[7, 1, row, 24].Style.Border.Bottom.Color.SetColor(Color.Black);
                            worksheet.Cells[7, 1, row, 24].Style.Border.Left.Color.SetColor(Color.Black);
                            worksheet.Cells[7, 1, row, 24].Style.Border.Right.Color.SetColor(Color.Black);
                            worksheet.Cells[6, 1, row, 24].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                            worksheet.Cells[6, 1, row, 18].AutoFitColumns();

                        }
                        else if (MachineID.Equals("Quality Incoming"))
                        {
                            worksheet.Cells["B3"].Value = fromDate.ToString("dd-MM-yyyy");
                            worksheet.Cells["B4"].Value = ToDate.ToString("dd-MM-yyyy");
                            int row = 6, slno = 0;
                            foreach (DataRow dtrow in dt.Rows)
                            {
                                worksheet.Cells[row, 1].Value = ++slno;
                                worksheet.Cells[row, 2].Value = dtrow["GRNDate"];
                                worksheet.Cells[row, 3].Value = dtrow["GRNNo"];
                                worksheet.Cells[row, 4].Value = dtrow["ProductionOrderNo"];
                                DateTime ScheduleDateTime = Convert.ToDateTime(dtrow["GRNDate"]);
                                worksheet.Cells[row, 5].Value = dtrow["GRNMonth"] + "-" + ScheduleDateTime.Year.ToString().Substring(2, 2);
                                worksheet.Cells[row, 6].Value = dtrow["PartNo"];
                                worksheet.Cells[row, 7].Value = dtrow["Loc"];
                                worksheet.Cells[row, 8].Value = dtrow["CompDescription"];
                                worksheet.Cells[row, 9].Value = dtrow["Qty"];
                                worksheet.Cells[row, 10].Value = dtrow["Unit"];
                                worksheet.Cells[row, 11].Value = dtrow["SupplierName"];
                                worksheet.Cells[row, 12].Value = dtrow["Origin"];
                                worksheet.Cells[row, 13].Value = dtrow["EmployeeName"];
                                worksheet.Cells[row, 14].Value = dtrow["InspectionMonth"];
                                worksheet.Cells[row, 15].Value = dtrow["InspectionDate"];
                                worksheet.Cells[row, 16].Value = dtrow["PriorityChanged"];
                                worksheet.Cells[row, 17].Value = dtrow["RepeatedIssues"];
                                worksheet.Cells[row, 18].Value = dtrow["Phenomenon"];
                                List<string> ProblemDesc = new List<string>();
                                int reasonNum = 0;
                                if (dtrow["Reason1"] != null && dtrow["Reason1"].ToString() != "")
                                {
                                    reasonNum++;
                                    ProblemDesc.Add(reasonNum + ". " + dtrow["Reason1"].ToString());
                                }
                                if (dtrow["Reason2"] != null && dtrow["Reason2"].ToString() != "")
                                {
                                    reasonNum++;
                                    ProblemDesc.Add(reasonNum + ". " + dtrow["Reason2"].ToString());
                                }
                                if (dtrow["Reason3"] != null && dtrow["Reason3"].ToString() != "")
                                {
                                    reasonNum++;
                                    ProblemDesc.Add(reasonNum + ". " + dtrow["Reason3"].ToString());
                                }
                                if (dtrow["Reason4"] != null && dtrow["Reason4"].ToString() != "")
                                {
                                    reasonNum++;
                                    ProblemDesc.Add(reasonNum + ". " + dtrow["Reason4"].ToString());
                                }
                                if (dtrow["Reason5"] != null && dtrow["Reason5"].ToString() != "")
                                {
                                    reasonNum++;
                                    ProblemDesc.Add(reasonNum + ". " + dtrow["Reason5"].ToString());
                                }
                                worksheet.Cells[row, 19].Value = String.Join("\n", ProblemDesc);
                                worksheet.Cells[row, 19].Style.WrapText = true;
                                worksheet.Cells[row, 20].Value = dtrow["RootCause"];
                                worksheet.Cells[row, 21].Value = dtrow["Responsible"];
                                worksheet.Cells[row, 22].Value = dtrow["InspectedQty"];
                                worksheet.Cells[row, 23].Value = dtrow["ReleasedQty"];
                                worksheet.Cells[row, 24].Value = dtrow["ReworkQty"];
                                worksheet.Cells[row, 25].Value = dtrow["BlockedQty"];
                                worksheet.Cells[row, 26].Value = dtrow["NCRorQualityReportNo"];
                                worksheet.Cells[row, 27].Value = dtrow["TargetDate"];
                                worksheet.Cells[row++, 28].Value = dtrow["Status"];
                            }
                            row--;
                            worksheet.Cells[6, 1, row, 28].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[6, 1, row, 28].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[6, 1, row, 28].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[6, 1, row, 28].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[6, 1, row, 28].Style.Border.Top.Color.SetColor(Color.Black);
                            worksheet.Cells[6, 1, row, 28].Style.Border.Bottom.Color.SetColor(Color.Black);
                            worksheet.Cells[6, 1, row, 28].Style.Border.Left.Color.SetColor(Color.Black);
                            worksheet.Cells[6, 1, row, 28].Style.Border.Right.Color.SetColor(Color.Black);
                            worksheet.Cells[5, 1, row, 28].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                            worksheet.Cells[5, 1, row, 17].AutoFitColumns();
                            worksheet.Column(10).Hidden = true;
                        }
                    }

                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    successfull = "Generated";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return successfull;
        }
        #endregion

        #region "Production Schedule Report"
        internal static bool GenerateProductionScheduleReport(string machineID, string from_date, string to_date, List<ScheduleDetailsEntity> prodScheduleDetails)
        {
            bool successfull = false;
            try
            {
                string Filename = "ProductionScheduleReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "ProductionScheduleReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Production Schedule Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    int rowStart = 6;
                    int val = 0;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                    var wrkshtProductionSchedule = excelPackage.Workbook.Worksheets[1];
                    wrkshtProductionSchedule.Cells["B3"].Value = machineID;
                    wrkshtProductionSchedule.Cells["F3"].Value = from_date;
                    wrkshtProductionSchedule.Cells["I3"].Value = to_date;
                    if (prodScheduleDetails != null && prodScheduleDetails.Count > 0)
                    {
                        foreach (ScheduleDetailsEntity scheduleDetails in prodScheduleDetails)
                        {
                            wrkshtProductionSchedule.Cells[rowStart, 1].Value = scheduleDetails.Priority;
                            wrkshtProductionSchedule.Cells[rowStart, 2].Value = scheduleDetails.ProductionOrderNumber;
                            wrkshtProductionSchedule.Cells[rowStart, 3].Value = scheduleDetails.MaterialID;
                            wrkshtProductionSchedule.Cells[rowStart, 4].Value = scheduleDetails.Model;
                            wrkshtProductionSchedule.Cells[rowStart, 5].Value = scheduleDetails.ModelDescription;
                            wrkshtProductionSchedule.Cells[rowStart, 6].Value = scheduleDetails.OperationNumber;
                            if (int.TryParse(scheduleDetails.OperationNumber, out val))
                                wrkshtProductionSchedule.Cells[rowStart, 6].Style.Numberformat.Format = "Number";
                            wrkshtProductionSchedule.Cells[rowStart, 7].Value = scheduleDetails.Quantity;
                            wrkshtProductionSchedule.Cells[rowStart, 8].Value = scheduleDetails.StdCycleTime;
                            if (int.TryParse(scheduleDetails.StdCycleTime, out val))
                                wrkshtProductionSchedule.Cells[rowStart, 8].Style.Numberformat.Format = "Number";
                            wrkshtProductionSchedule.Cells[rowStart, 9].Value = scheduleDetails.StdSetupTime;
                            if (int.TryParse(scheduleDetails.StdSetupTime, out val))
                                wrkshtProductionSchedule.Cells[rowStart, 9].Style.Numberformat.Format = "Number";
                            wrkshtProductionSchedule.Cells[rowStart, 10].Value = scheduleDetails.OldScheduledStartTime;
                            wrkshtProductionSchedule.Cells[rowStart, 11].Value = scheduleDetails.OldScheduledEndTime;
                            wrkshtProductionSchedule.Cells[rowStart, 12].Value = scheduleDetails.ScheduledStartTime;
                            wrkshtProductionSchedule.Cells[rowStart, 13].Value = scheduleDetails.ScheduledEndTime;
                            wrkshtProductionSchedule.Cells[rowStart, 14].Value = scheduleDetails.ActualStartTime;
                            wrkshtProductionSchedule.Cells[rowStart, 15].Value = scheduleDetails.PredictedCompletionTime;
                            wrkshtProductionSchedule.Cells[rowStart, 16].Value = scheduleDetails.ActualEndTime;
                            wrkshtProductionSchedule.Cells[rowStart, 17].Value = scheduleDetails.Status;
                            rowStart++;
                        }
                    }
                    DownloadMultipleFile(destination, excelPackage.GetAsByteArray());
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

        #region--------------------- Production Schedule Reports--------------------------
        internal static string GenerateProductionScheduleReports(string FromDate, string ToDate, string machineId, string status, string prodOrder, string materialID)
        {
            string successful = "";
            try
            {
                string Filename = "ProductionScheduleHistoryReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "ProductionScheduleHistoryReport" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Production Schedule History Report template does not exists at - " + Source);
                    successful = "TemplateNotFound";
                }
                else
                {
                    int rowStart = 6;

                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                    var worksheet = excelPackage.Workbook.Worksheets[1];
                    DataTable dt = GEADatabaseAccess.GetProductionScheduleData(FromDate, ToDate, machineId, status, prodOrder, materialID);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        worksheet.Cells["B3"].Value = machineId;
                        worksheet.Cells["F3"].Value = FromDate;
                        worksheet.Cells["I3"].Value = ToDate;
                        foreach (DataRow dtrow in dt.Rows)
                        {

                            worksheet.Cells[rowStart, 1].Value = Util.GetDateTime(dtrow["SCHLoaded_DateAndTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                            worksheet.Cells[rowStart, 2].Value = dtrow["SCHPriority"];
                            worksheet.Cells[rowStart, 3].Value = dtrow["PONumber"];
                            worksheet.Cells[rowStart, 4].Value = dtrow["MaterialID"];
                            worksheet.Cells[rowStart, 5].Value = dtrow["Model"];
                            worksheet.Cells[rowStart, 6].Value = dtrow["ModelDescription"];
                            worksheet.Cells[rowStart, 7].Value = dtrow["Opnno"];

                            worksheet.Cells[rowStart, 8].Value = dtrow["Qty"];
                            worksheet.Cells[rowStart, 9].Value = dtrow["stdcycletime"];

                            worksheet.Cells[rowStart, 10].Value = dtrow["StdSetupTime"];

                            worksheet.Cells[rowStart, 11].Value = dtrow["ScheduleStartTime"];
                            worksheet.Cells[rowStart, 12].Value = dtrow["schedulecompletedtime"];
                            worksheet.Cells[rowStart, 13].Value = dtrow["RevisedStartTime"];
                            worksheet.Cells[rowStart, 14].Value = dtrow["RevisedCompletedTime"];
                            worksheet.Cells[rowStart, 15].Value = dtrow["ActualStartTime"];
                            worksheet.Cells[rowStart, 16].Value = dtrow["PredictedCompletion"];
                            worksheet.Cells[rowStart, 17].Value = dtrow["ActualEndtime"];
                            worksheet.Cells[rowStart, 18].Value = dtrow["SCHStatus"];
                            rowStart++;
                        }
                    }
                    rowStart--;
                    worksheet.Cells[5, 1, rowStart, 18].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[5, 1, rowStart, 18].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[5, 1, rowStart, 18].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[5, 1, rowStart, 18].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[5, 1, rowStart, 18].Style.Border.Top.Color.SetColor(Color.Black);
                    worksheet.Cells[5, 1, rowStart, 18].Style.Border.Right.Color.SetColor(Color.Black);
                    worksheet.Cells[5, 1, rowStart, 18].Style.Border.Left.Color.SetColor(Color.Black);
                    worksheet.Cells[5, 1, rowStart, 18].Style.Border.Bottom.Color.SetColor(Color.Black);
                    DownloadMultipleFile(destination, excelPackage.GetAsByteArray());
                    successful = "Download Successful";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return successful;
        }
        #endregion

        #region "Assembly Schedule Report"
        internal static bool GenerateAssemblyScheduleReport(string machineID, string from_date, string to_date, List<AssemblyScheduleDetailsEntity> prodScheduleDetails)
        {
            bool successfull = false;
            try
            {
                string Filename = "AssemblyScheduleReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "AssemblyScheduleReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Assembly Schedule Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    int rowStart = 6;
                    int val = 0;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                    var wrkshtProductionSchedule = excelPackage.Workbook.Worksheets[1];
                    wrkshtProductionSchedule.Cells["B3"].Value = machineID;
                    wrkshtProductionSchedule.Cells["E3"].Value = from_date;
                    wrkshtProductionSchedule.Cells["H3"].Value = to_date;
                    if (prodScheduleDetails != null && prodScheduleDetails.Count > 0)
                    {
                        foreach (AssemblyScheduleDetailsEntity scheduleDetails in prodScheduleDetails)
                        {
                            wrkshtProductionSchedule.Cells[rowStart, 1].Value = scheduleDetails.Priority;
                            wrkshtProductionSchedule.Cells[rowStart, 2].Value = scheduleDetails.ProductionOrderNumber;
                            wrkshtProductionSchedule.Cells[rowStart, 3].Value = scheduleDetails.LocalExport;
                            wrkshtProductionSchedule.Cells[rowStart, 4].Value = scheduleDetails.SaleOrder;
                            wrkshtProductionSchedule.Cells[rowStart, 5].Value = scheduleDetails.OperationNumber;
                            if (int.TryParse(scheduleDetails.OperationNumber, out val))
                                wrkshtProductionSchedule.Cells[rowStart, 5].Style.Numberformat.Format = "Number";
                            wrkshtProductionSchedule.Cells[rowStart, 6].Value = scheduleDetails.Model;
                            wrkshtProductionSchedule.Cells[rowStart, 7].Value = scheduleDetails.ScrollWelded;
                            wrkshtProductionSchedule.Cells[rowStart, 8].Value = scheduleDetails.RDDMachines;
                            wrkshtProductionSchedule.Cells[rowStart, 9].Value = scheduleDetails.FabricationNumber;
                            wrkshtProductionSchedule.Cells[rowStart, 10].Value = scheduleDetails.Quantity;
                            wrkshtProductionSchedule.Cells[rowStart, 11].Value = scheduleDetails.StdCycleTime;
                            if (int.TryParse(scheduleDetails.StdCycleTime, out val))
                                wrkshtProductionSchedule.Cells[rowStart, 11].Style.Numberformat.Format = "Number";
                            wrkshtProductionSchedule.Cells[rowStart, 12].Value = scheduleDetails.StdSetupTime;
                            if (int.TryParse(scheduleDetails.StdSetupTime, out val))
                                wrkshtProductionSchedule.Cells[rowStart, 12].Style.Numberformat.Format = "Number";
                            wrkshtProductionSchedule.Cells[rowStart, 13].Value = scheduleDetails.OldScheduledStartTime;
                            wrkshtProductionSchedule.Cells[rowStart, 14].Value = scheduleDetails.OldScheduledEndTime;
                            wrkshtProductionSchedule.Cells[rowStart, 15].Value = scheduleDetails.ScheduledStartTime;
                            wrkshtProductionSchedule.Cells[rowStart, 16].Value = scheduleDetails.ScheduledEndTime;
                            wrkshtProductionSchedule.Cells[rowStart, 17].Value = scheduleDetails.ActualStartTime;
                            wrkshtProductionSchedule.Cells[rowStart, 18].Value = scheduleDetails.PredictedCompletionTime;
                            wrkshtProductionSchedule.Cells[rowStart, 19].Value = scheduleDetails.ActualEndTime;
                            wrkshtProductionSchedule.Cells[rowStart, 20].Value = scheduleDetails.Customer;
                            wrkshtProductionSchedule.Cells[rowStart, 21].Value = scheduleDetails.Location;
                            wrkshtProductionSchedule.Cells[rowStart, 22].Value = scheduleDetails.Activities;
                            wrkshtProductionSchedule.Cells[rowStart, 23].Value = scheduleDetails.Status;
                            rowStart++;
                        }
                    }
                    DownloadMultipleFile(destination, excelPackage.GetAsByteArray());
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

        #region Maintenance Self Inspection Report - GEA
        internal static bool GenerateSelfInspectionReport(List<SelfInspectionDetails> descriptionList, List<SelfInspectionDetails> gridList)
        {
            bool successfull = false;
            try
            {
                string Filename = "SelfInspectionReport.docx";
                string Source = GetReportPath(Filename);
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Self Inspection Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    if (descriptionList.Count > 0 && gridList.Count > 0)
                    {
                        using (var document = DocX.Load(Source))
                        {


                            var table1 = document.Tables[1];
                            if (table1 == null)
                            {
                                Console.WriteLine("\tError, couldn't find Self Inspection Report first table in current document.");
                                successfull = false;
                            }
                            else
                            {
                                setcellvalueToTable1(table1, 0, 1, descriptionList.Select(x => x.Description).First());
                                setcellvalueToTable1(table1, 0, 3, descriptionList.Select(x => x.PlanNumber).First());
                                setcellvalueToTable1(table1, 2, 1, descriptionList.Select(x => x.PartNumber).First());
                                setcellvalueToTable1(table1, 2, 3, descriptionList.Select(x => x.DrawingNumber).First());
                                setcellvalueToTable1(table1, 3, 1, descriptionList.Select(x => x.ProductionOrder).First());
                                setcellvalueToTable1(table1, 3, 3, descriptionList.Select(x => x.SerailNumber).First());
                                setcellvalueToTable1(table1, 4, 1, descriptionList.Select(x => x.MachineID).First());
                                setcellvalueToTable1(table1, 4, 3, descriptionList.Select(x => x.OperationNumber).First());
                            }
                            var table2 = document.Tables[2];
                            if (table2 == null)
                            {
                                Console.WriteLine("\tError, couldn't find Self Inspection Report second table in current document.");
                                successfull = false;
                            }
                            else
                            {
                                for (var i = 0; i < gridList.Count; i++)
                                {
                                    int j = 0;
                                    var newrow = table2.InsertRow();
                                    newrow.Cells[j].Paragraphs[0].Append(gridList[i].RowNumber).FontSize(10);
                                    j++;
                                    newrow.Cells[j].Paragraphs[0].Append(gridList[i].Parameter).FontSize(10);
                                    j++;
                                    newrow.Cells[j].Paragraphs[0].Append(gridList[i].OperatorMeasurement).FontSize(10);
                                    j++;
                                    newrow.Cells[j].Paragraphs[0].Append(gridList[i].QualityMeasurement).FontSize(10);
                                    j++;
                                    newrow.Cells[j].Paragraphs[0].Append(gridList[i].OperatorName).FontSize(10);
                                    j++;
                                    newrow.Cells[j].Paragraphs[0].Append(gridList[i].DateorShift).FontSize(10);
                                    j++;
                                    newrow.Cells[j].Paragraphs[0].Append(gridList[i].Remarks).FontSize(10);
                                }
                            }
                            createReportOutput();
                            document.SaveAs(appPathForReportOutput + "SelfInspectionReport.docx");
                            string destination = Path.Combine(appPathForReportOutput, "SelfInspectionReport.docx");
                            HttpContext.Current.Response.ClearHeaders();
                            HttpContext.Current.Response.ContentType = "application/docx";
                            HttpContext.Current.Response.Clear();
                            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=\"SelfInspectionReport.docx\"");
                            HttpContext.Current.Response.TransmitFile(destination);
                            HttpContext.Current.Response.Flush();
                            HttpContext.Current.Response.SuppressContent = true;
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            successfull = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return successfull;
        }

        private static void setcellvalueToTable1(Xceed.Document.NET.Table table, int row, int col, string value)
        {
            table.Rows[row].Cells[col].Paragraphs[0].Append(value == "&nbsp;" ? "" : value).FontSize(10);
        }
        #endregion


        internal static string quality8dReport(string MachineID, string ProductionOrder, string grnNumber)
        {
            string ReportStatus = string.Empty;
            try
            {
                string productionorder = ""; string reportName = ""; string Issuer = "", ReportNo = "", Field = "", ProblemOriginatedat = ""; DateTime IssueDate = DateTime.Now;
                List<_8DHeader> qltyIncomingReportData = GEADatabaseAccess.Get8DReport(MachineID, ProductionOrder, grnNumber, out Issuer, out ReportNo, out IssueDate, out Field, out ProblemOriginatedat);
                ReportNo = GEADatabaseAccess.GET8DReportNo(MachineID, ProductionOrder);
                if (qltyIncomingReportData != null && qltyIncomingReportData.Count > 0)
                {
                    string Filename = "Qualityincoming.docx";
                    string Source = GetReportPath(Filename);
                    string Template = "Qualityincoming" + DateTime.Now + ".docx";
                    string destination_dir = Path.Combine(appPath, "GeneratedReports");
                    string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                    if (!Directory.Exists(destination_dir)) Directory.CreateDirectory(destination_dir);
                    if (!File.Exists(Source))
                    {
                        Logger.WriteDebugLog("Daily Maintenance Checklist Report template does not exists at - " + Source);
                        ReportStatus = "TemplateNotFound";
                    }
                    else
                    {
                        using (var document = DocX.Load(Source))
                        {
                            var companyTable = document.Tables[1];
                            var data1 = document.Tables[2];
                            var data2 = document.Tables[3];
                            var data3 = document.Tables[4];
                            var data4 = document.Tables[5];
                            var data5 = document.Tables[6];
                            var data6 = document.Tables[7];
                            var data7 = document.Tables[8];
                            var data8 = document.Tables[9];
                            var data9 = document.Tables[11];

                            if (data1 == null || data2 == null || data3 == null || data4 == null || data5 == null || data6 == null || data7 == null || data8 == null)
                            {
                                Console.WriteLine("\tError, couldn't find quality incoming Report tables in current document.");
                                ReportStatus = "Failed";
                            }
                            else
                            {
                                SetcellvalueToTable(companyTable, 1, 1, Issuer);
                                SetcellvalueToTable(companyTable, 1, 3, ReportNo);
                                SetcellvalueToTable(companyTable, 2, 1, Field);
                                SetcellvalueToTable(companyTable, 2, 3, IssueDate.ToString("dd-MM-yyyy"));
                                SetcellvalueToTable(companyTable, 3, 1, ProblemOriginatedat);
                                //setcellvalueToTable1(data1, 3, 1, qltyIncomingReportData[0].GridOrRichText);
                                //setcellvalueToTable1(data2, 3, 1, qltyIncomingReportData[1].GridOrRichText);
                                //setcellvalueToTable1(data4, 3, 1, qltyIncomingReportData[2].GridOrRichText);
                                //setcellvalueToTable1(data2, 3, 1, qltyIncomingReportData[1].GridOrRichText);
                                //setcellvalueToTable1(data8, 3, 1, qltyIncomingReportData[7].GridOrRichText);
                                int row = 4;

                                foreach (_8DHeader data in qltyIncomingReportData)
                                {
                                    switch (data.headerID)
                                    {
                                        case 1:
                                            setcellvalueToTable1(data1, 2, 0, data.GridOrRichText);
                                            break;
                                        case 2:
                                            setcellvalueToTable1(data2, 2, 0, data.GridOrRichText);
                                            break;
                                        case 3:
                                            row = 3;
                                            foreach (_8DreportGridCol colval in data.GridData)
                                            {

                                                setcellvalueToTable1(data3, row, 0, colval.Measure);
                                                setcellvalueToTable1(data3, row, 1, colval.Responsible);
                                                setcellvalueToTable1(data3, row++, 2, colval.Deadline);
                                            }
                                            break;
                                        case 4:
                                            setcellvalueToTable1(data4, 2, 0, data.GridOrRichText);
                                            break;
                                        case 5:
                                            row = 3;
                                            foreach (_8DreportGridCol colval in data.GridData)
                                            {
                                                data3.Rows.Add(data3.Rows[1]);
                                                setcellvalueToTable1(data5, row, 0, colval.Measure);
                                                setcellvalueToTable1(data5, row, 1, colval.Responsible);
                                                setcellvalueToTable1(data5, row++, 2, colval.Deadline);
                                            }
                                            break;
                                        case 6:
                                            row = 3;
                                            foreach (_8DreportGridCol colval in data.GridData)
                                            {
                                                data3.Rows.Add(data3.Rows[1]);
                                                setcellvalueToTable1(data6, row, 0, colval.Measure);
                                                setcellvalueToTable1(data6, row, 1, colval.Responsible);
                                                setcellvalueToTable1(data6, row++, 2, colval.Deadline);
                                            }
                                            break;
                                        case 7:
                                            row = 3;
                                            foreach (_8DreportGridCol colval in data.GridData)
                                            {
                                                data3.Rows.Add(data3.Rows[row]);
                                                setcellvalueToTable1(data7, row, 0, colval.Measure);
                                                setcellvalueToTable1(data7, row, 1, colval.Responsible);
                                                setcellvalueToTable1(data7, row++, 2, colval.Deadline);

                                            }
                                            break;
                                        case 8:
                                            setcellvalueToTable1(data8, 2, 0, data.GridOrRichText);
                                            break;
                                    }
                                }
                                setcellvalueToTable1(data9, 0, 1, IssueDate.ToString("dd-MM-yyyy"));
                            }
                            document.SaveAs(destination);
                            HttpContext.Current.Response.ClearHeaders();
                            HttpContext.Current.Response.ContentType = "application/docx";
                            HttpContext.Current.Response.Clear();
                            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=\"8D" + "-" + ReportNo + ".docx\"");
                            HttpContext.Current.Response.TransmitFile(destination);
                            HttpContext.Current.Response.Flush();
                            HttpContext.Current.Response.SuppressContent = true;
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            ReportStatus = "Generated";
                        }
                    }
                }
                else
                {
                    ReportStatus = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }

        #region "Quality Incoming Inspection Report - GEA"
        internal static string GenerateQualityIncomingReport(string machine_id, string prod_order, string comp_id, string opn_num, string ins_plan)
        {
            string ReportStatus = string.Empty;
            try
            {
                List<QltyIncomingTransactionEntity> qltyIncomingReportData = GEADatabaseAccess.GetQltyIncomingReportData(machine_id, prod_order, comp_id, opn_num, ins_plan);
                if (qltyIncomingReportData != null && qltyIncomingReportData.Count > 0)
                {
                    string Filename = "QualityIncomingInspectionReport.docx";
                    string Source = GetReportPath(Filename);
                    string Template = "QualityIncomingInspectionReport" + DateTime.Now + ".docx";
                    string destination_dir = Path.Combine(appPath, "GeneratedReports");
                    string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                    if (!Directory.Exists(destination_dir)) Directory.CreateDirectory(destination_dir);
                    if (!File.Exists(Source))
                    {
                        Logger.WriteDebugLog("Daily Maintenance Checklist Report template does not exists at - " + Source);
                        ReportStatus = "TemplateNotFound";
                    }
                    else
                    {
                        using (var document = DocX.Load(Source))
                        {
                            var companyTable = document.Tables[0];
                            var descriptionTable = document.Tables[1];
                            var qualityInsDataTable = document.Tables[2];
                            var insParamsTable = document.Tables[3];
                            if (descriptionTable == null || qualityInsDataTable == null || insParamsTable == null)
                            {
                                Console.WriteLine("\tError, couldn't find quality incoming Report tables in current document.");
                                ReportStatus = "Failed";
                            }
                            else
                            {
                                SetcellvalueToTable(companyTable, 0, 1, "Description");
                                SetcellvalueToTable(descriptionTable, 0, 1, "Description");
                                SetcellvalueToTable(descriptionTable, 0, 3, ins_plan);
                                SetcellvalueToTable(descriptionTable, 1, 1, prod_order);
                                SetcellvalueToTable(descriptionTable, 1, 3, comp_id);
                                if (qltyIncomingReportData.Count > 10)
                                {
                                    int extra_rows = qltyIncomingReportData.Count - 10;
                                    for (int i = 0; i < extra_rows; i++)
                                        qualityInsDataTable.InsertRow();
                                }
                                int row_num = 2;
                                foreach (QltyIncomingTransactionEntity qltyIncomingData in qltyIncomingReportData)
                                {
                                    SetcellvalueToTable(qualityInsDataTable, row_num, 0, qltyIncomingData.ID.ToString());
                                    SetcellvalueToTable(qualityInsDataTable, row_num, 1, qltyIncomingData.CharacteristicCode);
                                    SetcellvalueToTable(qualityInsDataTable, row_num, 2, qltyIncomingData.SetValue);
                                    SetcellvalueToTable(qualityInsDataTable, row_num, 3, $"{qltyIncomingData.InspectionValueOneA}/{qltyIncomingData.InspectionValueOneB}");
                                    SetcellvalueToTable(qualityInsDataTable, row_num, 4, $"{qltyIncomingData.InspectionValueTwoA}/{qltyIncomingData.InspectionValueTwoB}");
                                    SetcellvalueToTable(qualityInsDataTable, row_num, 5, $"{qltyIncomingData.InspectionValueThreeA}/{qltyIncomingData.InspectionValueThreeB}");
                                    SetcellvalueToTable(qualityInsDataTable, row_num, 6, $"{qltyIncomingData.InspectionValueFourA}/{qltyIncomingData.InspectionValueFourB}");
                                    SetcellvalueToTable(qualityInsDataTable, row_num, 7, $"{qltyIncomingData.InspectionValueFiveA}/{qltyIncomingData.InspectionValueFiveB}");
                                    SetcellvalueToTable(qualityInsDataTable, row_num, 8, qltyIncomingData.Comments);
                                    row_num++;
                                }
                            }
                            document.SaveAs(destination);
                            HttpContext.Current.Response.ClearHeaders();
                            HttpContext.Current.Response.ContentType = "application/docx";
                            HttpContext.Current.Response.Clear();
                            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=\"QualityIncomingInspectionReport.docx\"");
                            HttpContext.Current.Response.TransmitFile(destination);
                            HttpContext.Current.Response.Flush();
                            HttpContext.Current.Response.SuppressContent = true;
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            ReportStatus = "Generated";
                        }
                    }
                }
                else
                {
                    ReportStatus = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }

        private static void SetcellvalueToTable(Xceed.Document.NET.Table table, int row, int col, string value)
        {
            table.Rows[row].Cells[col].Paragraphs[0].Append(value == "&nbsp;" ? "" : value).FontSize(10);
        }
        #endregion


        #region -----Machine Data Assemebly Report -----
        private static PdfPCell getPdfCellForVibration(string value)
        {
            iTextSharp.text.Font font = FontFactory.GetFont("Calibri (Body)", 10);
            Chunk chunk = new Chunk("Measuring procedure: WSN 76 - ", font);
            chunk.Font.Color = new iTextSharp.text.BaseColor(10, 10, 10);
            chunk.Font.Size = 7;
            Phrase phrase = new Phrase();
            phrase.Add(chunk);
            font = FontFactory.GetFont("Calibri (Body)", 10, iTextSharp.text.Font.UNDERLINE);
            chunk = new Chunk("    102    ", font);
            chunk.Font.Color = new iTextSharp.text.BaseColor(10, 10, 10);
            chunk.Font.Size = 7;
            phrase.Add(chunk);
            font = FontFactory.GetFont("Calibri (Body)", 10);
            chunk = new Chunk("  -  ", font);
            chunk.Font.Color = new iTextSharp.text.BaseColor(10, 10, 10);
            chunk.Font.Size = 7;
            phrase.Add(chunk);
            font = FontFactory.GetFont("Calibri (Body)", 10, iTextSharp.text.Font.UNDERLINE);
            chunk = new Chunk(" 30    ", font);
            chunk.Font.Color = new iTextSharp.text.BaseColor(10, 10, 10);
            chunk.Font.Size = 7;
            phrase.Add(chunk);
            font = FontFactory.GetFont("Calibri (Body)", 10);
            chunk = new Chunk(" ", font);
            chunk.Font.Color = new iTextSharp.text.BaseColor(10, 10, 10);
            chunk.Font.Size = 7;
            phrase.Add(chunk);
            PdfPCell cell = new PdfPCell(phrase);
            cell.ExtraParagraphSpace = 3;
            cell.BorderColor = getPdfCellBorderColor();
            return cell;
        }
        private static PdfPCell getPdfCellWithBoldHeader(string value, int size)
        {
            iTextSharp.text.Font font = FontFactory.GetFont("Calibri (Body)", 13, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font boldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD);
            Chunk chunk = new Chunk(value, font);
            chunk.Font.Color = new iTextSharp.text.BaseColor(10, 10, 10);
            chunk.Font.Size = size;
            Phrase phrase = new Phrase();
            phrase.Add(chunk);
            PdfPCell cell = new PdfPCell(phrase);
            cell.ExtraParagraphSpace = 3;
            cell.BorderColor = getPdfCellBorderColor();
            return cell;
        }
        private static iTextSharp.text.BaseColor getPdfCellBorderColor()
        {
            iTextSharp.text.BaseColor baseColor = new iTextSharp.text.BaseColor(122, 121, 121);
            try
            {
                if (reportName.Equals("FirstSample", StringComparison.OrdinalIgnoreCase))
                {
                    baseColor = new iTextSharp.text.BaseColor(199, 197, 197);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return baseColor;
        }
        private static PdfPCell getPdfCellWithBoldHeaderUnderLine(string value, int fontSize)
        {
            iTextSharp.text.Font font = FontFactory.GetFont("Calibri (Body)", 13, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            iTextSharp.text.Font boldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD);
            Chunk chunk = new Chunk(value, font);
            chunk.Font.Color = new iTextSharp.text.BaseColor(10, 10, 10);
            chunk.Font.Size = fontSize;
            Phrase phrase = new Phrase();
            phrase.Add(chunk);
            PdfPCell cell = new PdfPCell(phrase);
            cell.ExtraParagraphSpace = 3;
            cell.BorderColor = getPdfCellBorderColor();
            return cell;
        }
        private static PdfPCell getPdfCellWithoutBoldText(string value)
        {
            iTextSharp.text.Font font = FontFactory.GetFont("Calibri (Body)", 10);
            Chunk chunk = new Chunk(value, font);
            chunk.Font.Color = new iTextSharp.text.BaseColor(10, 10, 10);
            chunk.Font.Size = 7;
            Phrase phrase = new Phrase();
            phrase.Add(chunk);
            PdfPCell cell = new PdfPCell(phrase);
            cell.ExtraParagraphSpace = 3;
            cell.BorderColor = getPdfCellBorderColor();
            return cell;
        }
        private static PdfPCell getPdfCellWithText(string value)
        {
            iTextSharp.text.Font font = FontFactory.GetFont("Calibri (Body)", 10, iTextSharp.text.Font.BOLD);
            Chunk chunk = new Chunk(value, font);
            chunk.Font.Color = new iTextSharp.text.BaseColor(10, 10, 10);
            chunk.Font.Size = 7;
            Phrase phrase = new Phrase();
            phrase.Add(chunk);
            PdfPCell cell = new PdfPCell(phrase);
            cell.ExtraParagraphSpace = 3;
            cell.BorderColor = getPdfCellBorderColor();
            return cell;
        }
        private static PdfPCell getPdfCellWithText(string value, int size)
        {
            iTextSharp.text.Font font = FontFactory.GetFont("Calibri (Body)", 10, iTextSharp.text.Font.BOLD);
            Chunk chunk = new Chunk(value, font);
            chunk.Font.Color = new iTextSharp.text.BaseColor(10, 10, 10);
            chunk.Font.Size = size;
            Phrase phrase = new Phrase();
            phrase.Add(chunk);
            PdfPCell cell = new PdfPCell(phrase);
            cell.ExtraParagraphSpace = 3;
            cell.BorderColor = getPdfCellBorderColor();
            return cell;
        }
        private static PdfPCell getPdfCellWithText(string value, int size, bool bold)
        {
            iTextSharp.text.Font font = null;
            if (bold)
            {
                font = FontFactory.GetFont("Calibri (Body)", 10, iTextSharp.text.Font.BOLD);
            }
            else
            {
                font = FontFactory.GetFont("Calibri (Body)", 10);
            }
            Chunk chunk = new Chunk(value, font);
            chunk.Font.Color = new iTextSharp.text.BaseColor(10, 10, 10);
            chunk.Font.Size = size;
            Phrase phrase = new Phrase();
            phrase.Add(chunk);
            PdfPCell cell = new PdfPCell(phrase);
            cell.ExtraParagraphSpace = 3;
            cell.BorderColor = getPdfCellBorderColor();
            return cell;
        }
        private static PdfPCell getPdfCellWithSupSubScript(string txt1, string txt2, bool isSubscript)
        {
            iTextSharp.text.Font font = FontFactory.GetFont("Calibri (Body)", 10);
            Chunk chunk = new Chunk(txt1, font);
            chunk.Font.Color = new iTextSharp.text.BaseColor(10, 10, 10);
            chunk.Font.Size = 10;
            Phrase phrase = new Phrase();
            phrase.Add(chunk);
            chunk = new Chunk(txt2, font);
            chunk.Font.Color = new iTextSharp.text.BaseColor(10, 10, 10);
            chunk.Font.Size = 7;
            if (isSubscript)
            {
                chunk.SetTextRise(-2);
            }
            else
            {
                chunk.SetTextRise(2);
            }
            phrase.Add(chunk);
            PdfPCell cell = new PdfPCell(phrase);
            cell.ExtraParagraphSpace = 3;
            cell.BorderColor = getPdfCellBorderColor();
            return cell;
        }
        private static PdfPCell getPdfCellGridHeader(string value)
        {
            iTextSharp.text.Font font = FontFactory.GetFont("Calibri (Body)", 10);
            Chunk chunk = new Chunk(value, font);
            chunk.Font.Color = new iTextSharp.text.BaseColor(252, 250, 250);
            chunk.Font.Size = 7;
            Phrase phrase = new Phrase();
            phrase.Add(chunk);
            PdfPCell cell = new PdfPCell(phrase);
            cell.ExtraParagraphSpace = 3;
            cell.BorderColor = getPdfCellBorderColor();
            cell.BackgroundColor = new BaseColor(45, 53, 73);
            return cell;
        }
        internal static string machineDataAssmeblyReport(string ProductionOrder, string FabricationNo, string machineId)
        {
            string ReportStatus = string.Empty;
            try
            {
                PdfPTable machineDataTbl = machineDataAssmeblyReportTbl(ProductionOrder, FabricationNo, machineId, 20, false, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    Document pdfDoc = new Document(PageSize.A4, 15, 15, 25, 25);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();
                    pdfDoc.Add(machineDataTbl);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=MachineDataAssembly.pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }
        private static string getGEALogoPath()
        {
            string imagesPath = "~/GEA/Icons/GEALogo.jpg";
            try
            {
                imagesPath = "~/CompanyLogo/";
                var dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(imagesPath));
                List<string> fileNames = (from flInfo in dir.GetFiles() select flInfo.Name).ToList();
                if (fileNames.Count > 0)
                {
                    imagesPath = imagesPath + fileNames[0];
                }
                else
                {
                    imagesPath = "~/GEA/Icons/GEALogo.jpg";
                }
                //byte[] file1;
                //file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(imagesPath));//ImagePath  
                //geaLogo = iTextSharp.text.Image.GetInstance(file1);
                //geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                //geaLogo.ScaleToFit(80f, 60f);
            }
            catch (Exception ex)
            {
                imagesPath = "~/GEA/Icons/GEALogo.jpg";
                Logger.WriteErrorLog(ex.Message);
            }
            return imagesPath;
        }
        internal static PdfPTable machineDataAssmeblyReportTbl(string ProductionOrder, string FabricationNo, string machineID, int mainTblSpaceBefore, bool isFromFinalReport, out string ReportStatus)
        {
            ReportStatus = string.Empty;
            PdfPTable machineDataTbl = new PdfPTable(6);
            try
            {
                string operatorname = ""; string TechnicianName = "", UpdatedDate1 = "", machineType = "", formatNo = "", RevNo = "";
                DataTable secondGridData, ChecklistData, headerData;
                // Dictionary<string, string> headerData = new Dictionary<string, string>();
                DataTable firstGridTbl = GEADatabaseAccess.GetMachineDataAssemblyReportData(ProductionOrder, FabricationNo, machineID, out secondGridData, out headerData, out ChecklistData);
                if (firstGridTbl.Rows.Count >= 0)
                {
                    if (firstGridTbl.Rows.Count > 0)
                    {
                        operatorname = firstGridTbl.Rows[0]["UpdatedBy"].ToString();
                        TechnicianName = firstGridTbl.Rows[0]["TechnicianName"].ToString();
                        if (firstGridTbl.Rows[0]["UpdatedTS"] != null && firstGridTbl.Rows[0]["UpdatedTS"].ToString() != "")
                        {
                            UpdatedDate1 = Convert.ToDateTime(firstGridTbl.Rows[0]["UpdatedTS"]).ToString("dd-MMM-yy");
                        }
                        machineType = firstGridTbl.Rows[0]["MachineType"] == null ? "" : firstGridTbl.Rows[0]["MachineType"].ToString();
                        formatNo = firstGridTbl.Rows[0]["FormatNo"] == null ? "" : firstGridTbl.Rows[0]["FormatNo"].ToString();
                        RevNo = firstGridTbl.Rows[0]["RevNo"] == null ? "" : firstGridTbl.Rows[0]["RevNo"].ToString();
                    }

                    machineDataTbl.SplitLate = false;
                    machineDataTbl.WidthPercentage = 100;
                    machineDataTbl.SpacingBefore = mainTblSpaceBefore;
                    machineDataTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    machineDataTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    int paddingValue = 0;
                    //header
                    byte[] file1;
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(getGEALogoPath()));//ImagePath  
                    iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(80f, 60f);
                    PdfPCell logoCell = new PdfPCell(geaLogo, false);
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    machineDataTbl.AddCell(logoCell);

                    //machineDataTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("GEA",10)) { HorizontalAlignment = Element.ALIGN_CENTER, Colspan = 1});
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Machine Data - Assembly", 13)) { HorizontalAlignment = Element.ALIGN_CENTER, Colspan = 3, VerticalAlignment = Element.ALIGN_MIDDLE });
                    PdfPTable formateDataTbl = new PdfPTable(1);
                    formateDataTbl.WidthPercentage = 100;
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Format No: " + formatNo)) { Border = 0 });
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Rev No: " + RevNo)) { Border = 0 });
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Date: " + "04-Mar-16")) { Border = 0 });
                    machineDataTbl.AddCell(new PdfPCell(formateDataTbl) { HorizontalAlignment = Element.ALIGN_LEFT, Colspan = 2 });

                    //machine details
                    PdfPTable machineScheduleTbl = new PdfPTable(4);
                    machineScheduleTbl.TotalWidth = 600;
                    if (isFromFinalReport)
                    {
                        int[] tblCellWidth6 = { 0, 100, 400, 200 };
                        machineScheduleTbl.SetWidths(tblCellWidth6);
                    }
                    else
                    {
                        PdfPTable machieModelTbl = new PdfPTable(1);
                        machieModelTbl.WidthPercentage = 100;
                        machieModelTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Machine No", 9)));
                        machieModelTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Model", 9)));
                        machieModelTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("", 9)));
                        machineDataTbl.AddCell(new PdfPCell(machieModelTbl) { Colspan = 1 });

                        int[] tblCellWidth6 = { 200, 100, 100, 200 };
                        machineScheduleTbl.SetWidths(tblCellWidth6);
                    }
                    machineScheduleTbl.WidthPercentage = 40;
                    if (headerData.Rows.Count > 0)
                    {
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Type")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineType)) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Machine No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Scroll No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Bowl No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Order No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["SaleOrder"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Customer")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["Customer"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Country")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["Location"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Type")) { Border = 0,Width=60 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineType)) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0,Colspan=4 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Machine No")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Scroll No")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Bowl No")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Order No")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["ProductionOrder"].ToString())) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Customer")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["Customer"].ToString())) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Country")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["Location"].ToString())) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        //machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                    }
                    if (isFromFinalReport)
                    {
                        machineDataTbl.AddCell(new PdfPCell(machineScheduleTbl) { Colspan = 6 });
                    }
                    else
                    {
                        machineDataTbl.AddCell(new PdfPCell(machineScheduleTbl) { Colspan = 5 });
                    }

                    paddingValue = 7;
                    //Bind first grid
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellGridHeader("Description")) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellGridHeader("Part No")) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellGridHeader("Series/Serial No")) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellGridHeader("Speed (RPM)")) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellGridHeader("Density(p)")) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellGridHeader("Stamps")) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    if (firstGridTbl.Rows.Count > 0)
                    {
                        for (int i = 0; i < firstGridTbl.Rows.Count; i++)
                        {
                            bool isParameterIDBearingHousing = false;
                            if (firstGridTbl.Rows[i]["ParameterID"].ToString() == "Bearing Housing")
                            {
                                isParameterIDBearingHousing = true;
                            }
                            machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(firstGridTbl.Rows[i]["ParameterID"] == null ? "" : firstGridTbl.Rows[i]["ParameterID"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                            machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(firstGridTbl.Rows[i]["PartNo"] == null ? "" : firstGridTbl.Rows[i]["PartNo"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                            if (isParameterIDBearingHousing)
                            {
                                machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Solid Side")) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                            }
                            else
                            {
                                machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(firstGridTbl.Rows[i]["SerialNo"] == null ? "" : firstGridTbl.Rows[i]["SerialNo"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                            }

                            if (firstGridTbl.Rows[i]["ParameterID"].ToString().Equals("Secondary Gear", StringComparison.OrdinalIgnoreCase))
                            {
                                machineDataTbl.AddCell(new PdfPCell(getcheckBoxWithText("Ensure Loctite242/245 for the Lock nut", ChecklistData.Rows[0]["EnsureLocitte"].ToString(), 9)) { HorizontalAlignment = Element.ALIGN_LEFT, Colspan = 3, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                            }
                            else
                            {
                                machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(firstGridTbl.Rows[i]["Speed"] == null ? "" : firstGridTbl.Rows[i]["Speed"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                                if (isParameterIDBearingHousing)
                                {
                                    machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Liquid Side")) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                                }
                                else
                                {
                                    machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(firstGridTbl.Rows[i]["Density"] == null ? "" : firstGridTbl.Rows[i]["Density"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                                }

                                machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(firstGridTbl.Rows[i]["Stamps"] == null ? "" : firstGridTbl.Rows[i]["Stamps"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT });
                            }
                        }
                    }

                    //Accessories data
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellGridHeader("Accessories")) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellGridHeader("Vibration Pick Up")) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellGridHeader("Temp Filler")) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellGridHeader("Auto Lube")) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellGridHeader("Flushing")) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellGridHeader("")) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    for (int i = 0; i < ChecklistData.Rows.Count; i++)
                    {
                        machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(ChecklistData.Rows[i]["ParameterID"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Sr No: " + ChecklistData.Rows[i]["VibrationPickUp"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        if (string.Equals(ChecklistData.Rows[i]["TempFiller"].ToString(), "Ok", StringComparison.OrdinalIgnoreCase))
                        {
                            byte[] checkfile;
                            checkfile = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/Check.png"));//ImagePath  
                            iTextSharp.text.Image checkjpg = iTextSharp.text.Image.GetInstance(checkfile);
                            checkjpg.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                            checkjpg.ScaleToFit(10f, 10f);
                            PdfPCell cell = new PdfPCell(checkjpg, false);
                            cell.BorderWidth = 1;
                            cell.PaddingTop = paddingValue;
                            cell.PaddingBottom = paddingValue;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            machineDataTbl.AddCell(cell).BorderColor = new BaseColor(122, 121, 121);
                        }
                        else
                        {
                            byte[] uncheckfile;
                            uncheckfile = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/Uncheck.png"));//ImagePath  
                            iTextSharp.text.Image uncheckjpg = iTextSharp.text.Image.GetInstance(uncheckfile);
                            uncheckjpg.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                            uncheckjpg.ScaleToFit(10f, 10f);
                            PdfPCell cell = new PdfPCell(uncheckjpg, false);
                            cell.BorderWidth = 1;
                            cell.PaddingTop = paddingValue;
                            cell.PaddingBottom = paddingValue;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            machineDataTbl.AddCell(cell).BorderColor = new BaseColor(122, 121, 121);
                        }
                        if (string.Equals(ChecklistData.Rows[i]["AutoLube"].ToString(), "Ok", StringComparison.OrdinalIgnoreCase))
                        {
                            byte[] checkfile;
                            checkfile = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/Check.png"));//ImagePath  
                            iTextSharp.text.Image checkjpg = iTextSharp.text.Image.GetInstance(checkfile);
                            checkjpg.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                            checkjpg.ScaleToFit(10f, 10f);
                            PdfPCell cell = new PdfPCell(checkjpg, false);
                            cell.BorderWidth = 1;
                            cell.PaddingTop = paddingValue;
                            cell.PaddingBottom = paddingValue;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            machineDataTbl.AddCell(cell).BorderColor = new BaseColor(122, 121, 121);
                        }
                        else
                        {
                            byte[] uncheckfile;
                            uncheckfile = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/Uncheck.png"));//ImagePath  
                            iTextSharp.text.Image uncheckjpg = iTextSharp.text.Image.GetInstance(uncheckfile);
                            uncheckjpg.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                            uncheckjpg.ScaleToFit(10f, 10f);
                            PdfPCell cell = new PdfPCell(uncheckjpg, false);
                            cell.BorderWidth = 1;
                            cell.PaddingTop = paddingValue;
                            cell.PaddingBottom = paddingValue;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            machineDataTbl.AddCell(cell).BorderColor = new BaseColor(122, 121, 121);
                        }
                        if (string.Equals(ChecklistData.Rows[i]["Flushing"].ToString(), "Ok", StringComparison.OrdinalIgnoreCase))
                        {
                            byte[] checkfile;
                            checkfile = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/Check.png"));//ImagePath  
                            iTextSharp.text.Image checkjpg = iTextSharp.text.Image.GetInstance(checkfile);
                            checkjpg.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                            checkjpg.ScaleToFit(10f, 10f);
                            PdfPCell cell = new PdfPCell(checkjpg, false);
                            cell.BorderWidth = 1;
                            cell.PaddingTop = paddingValue;
                            cell.PaddingBottom = paddingValue;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            machineDataTbl.AddCell(cell).BorderColor = new BaseColor(122, 121, 121);
                        }
                        else
                        {
                            byte[] uncheckfile;
                            uncheckfile = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/Uncheck.png"));//ImagePath  
                            iTextSharp.text.Image uncheckjpg = iTextSharp.text.Image.GetInstance(uncheckfile);
                            uncheckjpg.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                            uncheckjpg.ScaleToFit(10f, 10f);
                            PdfPCell cell = new PdfPCell(uncheckjpg, false);
                            cell.BorderWidth = 1;
                            cell.PaddingTop = paddingValue;
                            cell.PaddingBottom = paddingValue;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            machineDataTbl.AddCell(cell).BorderColor = new BaseColor(122, 121, 121);
                        }
                        machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    }

                    //Specification details
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellGridHeader("Specification")) { HorizontalAlignment = Element.ALIGN_CENTER, Colspan = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellGridHeader("Main Motor")) { HorizontalAlignment = Element.ALIGN_CENTER, Colspan = 2, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellGridHeader("Secondary Motor")) { HorizontalAlignment = Element.ALIGN_CENTER, Colspan = 3, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    for (int i = 0; i < secondGridData.Rows.Count; i++)
                    {
                        machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(secondGridData.Rows[i]["ParameterID"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, Colspan = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(secondGridData.Rows[i]["MainMotor"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, Colspan = 2, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(secondGridData.Rows[i]["SecondaryMotor"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, Colspan = 3, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    }


                    machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Technician Name")) { HorizontalAlignment = Element.ALIGN_CENTER, Colspan = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(TechnicianName)) { HorizontalAlignment = Element.ALIGN_CENTER, Colspan = 1 });
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Signature with date")) { HorizontalAlignment = Element.ALIGN_CENTER, Colspan = 2, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(operatorname + ", " + UpdatedDate1)) { HorizontalAlignment = Element.ALIGN_CENTER, Colspan = 2, PaddingTop = paddingValue, PaddingBottom = paddingValue });

                }
                else
                {
                    ReportStatus = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
            }
            return machineDataTbl;
        }

        #endregion

        #region -------- Electo-Tech Equipment Report -----
        internal static string electroTechEquipmentReport(string ProductionOrder, string FabricationNo, string machineID)
        {
            string ReportStatus = string.Empty;
            try
            {
                PdfPTable electoTechTbl = electroTechEquipmentReport(ProductionOrder, FabricationNo, machineID, 20, false, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    Document pdfDoc = new Document(PageSize.A4, 15, 15, 25, 25);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();
                    pdfDoc.Add(electoTechTbl);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ElectroTechEquipment.pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }
        internal static PdfPTable electroTechEquipmentReport(string ProductionOrder, string FabricationNo, string machineID, int mainTblSpaceBefore, bool isFromFinalReport, out string ReportStatus)
        {
            ReportStatus = string.Empty;
            PdfPTable electoTechTbl = new PdfPTable(1);
            try
            {
                DataTable headerNameTbl;
                DataTable machineDataTbl = GEADatabaseAccess.GetElectroTechEquipmentReportData(ProductionOrder, FabricationNo, machineID, out headerNameTbl);
                if (machineDataTbl.Rows.Count > 0 || headerNameTbl.Rows.Count > 0)
                {
                    string machineType = "", checkedBy = "", checkedDate = "", formatNo = "", revNo = "", customer = "", orderno = "";
                    if (headerNameTbl.Rows.Count > 0)
                    {
                        machineType = headerNameTbl.Rows[0]["MachineType"] == null ? "" : headerNameTbl.Rows[0]["MachineType"].ToString();
                        checkedBy = headerNameTbl.Rows[0]["UpdatedBy"] == null ? "" : headerNameTbl.Rows[0]["UpdatedBy"].ToString();
                        if (headerNameTbl.Rows[0]["UpdatedTS"] != null && headerNameTbl.Rows[0]["UpdatedTS"].ToString() != "")
                        {
                            checkedDate = Convert.ToDateTime(headerNameTbl.Rows[0]["UpdatedTS"]).ToString("dd-MMM-yy");
                        }
                        formatNo = headerNameTbl.Rows[0]["FormatNo"] == null ? "" : headerNameTbl.Rows[0]["FormatNo"].ToString();
                        revNo = headerNameTbl.Rows[0]["RevNo"] == null ? "" : headerNameTbl.Rows[0]["RevNo"].ToString();
                    }

                    electoTechTbl.SplitLate = false;
                    electoTechTbl.WidthPercentage = 100;
                    electoTechTbl.SpacingBefore = mainTblSpaceBefore;
                    electoTechTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    electoTechTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    //header
                    PdfPTable headerTbl = new PdfPTable(7);
                    headerTbl.WidthPercentage = 100;
                    byte[] file1;
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(getGEALogoPath()));//ImagePath  
                    iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(70f, 60f);
                    PdfPCell logoCell = new PdfPCell(geaLogo, false);
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerTbl.AddCell(new PdfPCell(logoCell) { Colspan = 1 });
                    headerTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Test Report of the Electrotechnical Equipment(Internal) Abnahmeprotokoll der elektrotechnischen Ausrustung (intern)", 9)) { HorizontalAlignment = Element.ALIGN_CENTER, Colspan = 4, VerticalAlignment = Element.ALIGN_MIDDLE });
                    PdfPTable formateDataTbl = new PdfPTable(1);
                    formateDataTbl.WidthPercentage = 100;
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Format No: " + formatNo)) { Border = 0 });
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Rev No: " + revNo)) { Border = 0 });
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Date: " + "04-Mar-16")) { Border = 0 });
                    headerTbl.AddCell(new PdfPCell(formateDataTbl) { HorizontalAlignment = Element.ALIGN_LEFT, Colspan = 2 });
                    electoTechTbl.AddCell(new PdfPCell(headerTbl));

                    if (machineDataTbl.Rows.Count > 0)
                    {
                        PdfPTable machineScheduleTbl = new PdfPTable(4);
                        machineScheduleTbl.TotalWidth = 600;
                        if (isFromFinalReport)
                        {
                            int[] tblCellWidth6 = { 0, 100, 400, 200 };
                            machineScheduleTbl.SetWidths(tblCellWidth6);
                        }
                        else
                        {
                            int[] tblCellWidth6 = { 200, 100, 100, 200 };
                            machineScheduleTbl.SetWidths(tblCellWidth6);
                        }
                        machineScheduleTbl.WidthPercentage = 40;
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Type")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineType)) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Machine No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Scroll No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Bowl No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Order No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["SaleOrder"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Customer")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["Customer"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Country")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["Location"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        electoTechTbl.AddCell(new PdfPCell(machineScheduleTbl));
                        customer = machineDataTbl.Rows[0]["Customer"].ToString();
                        orderno = machineDataTbl.Rows[0]["ProductionOrder"].ToString();
                    }
                    PdfPTable pdfTable = new PdfPTable(2);
                    pdfTable.WidthPercentage = 100;
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Decanter-Type: " + machineType, 7)) { Border = 0 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Order.-No.: " + orderno, 7)) { Border = 0 });
                    electoTechTbl.AddCell(pdfTable);
                    pdfTable = new PdfPTable(2);
                    pdfTable.WidthPercentage = 100;
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Masch.-No.: " + orderno, 7)) { Border = 0 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Customer: " + customer, 7)) { Border = 0 });
                    electoTechTbl.AddCell(pdfTable);

                    string headerName = "", value = "";
                    bool isChecked;
                    byte[] checkfile;
                    checkfile = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/GEA/Icons/Right.png"));//ImagePath  
                    iTextSharp.text.Image checkedPng = iTextSharp.text.Image.GetInstance(checkfile);
                    checkedPng.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    checkedPng.ScaleToFit(10f, 10f);
                    int paddingleftValue = 2;
                    pdfTable = new PdfPTable(3);
                    int[] tblCellWidth = { 8, 5, 250 };
                    pdfTable.SetWidths(tblCellWidth);
                    pdfTable.WidthPercentage = 100;
                    headerName = getElectroTechHeaderName(headerNameTbl, 1, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Padding = 0, PaddingLeft = paddingleftValue });

                    headerName = getElectroTechHeaderName(headerNameTbl, 2, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Padding = 0, PaddingLeft = paddingleftValue });

                    headerName = getElectroTechHeaderName(headerNameTbl, 3, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Padding = 0, PaddingLeft = paddingleftValue });
                    electoTechTbl.AddCell(pdfTable);



                    pdfTable = new PdfPTable(3);
                    int[] tblCellWidth1 = { 8, 5, 250 };
                    pdfTable.SetWidths(tblCellWidth1);
                    pdfTable.WidthPercentage = 100;
                    headerName = getElectroTechHeaderName(headerNameTbl, 4, 1, out isChecked, out value);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeaderUnderLine(headerName, 7)) { Border = 0, Colspan = 3 });

                    headerName = getElectroTechHeaderName(headerNameTbl, 5, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Padding = 0, PaddingLeft = paddingleftValue });

                    headerName = getElectroTechHeaderName(headerNameTbl, 6, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    PdfPTable pdfInnerTable = new PdfPTable(7);
                    int[] tblCellWidth2 = { 100, 12, 100, 12, 100, 12, 100 };
                    pdfInnerTable.SetWidths(tblCellWidth2);
                    pdfInnerTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)));
                    headerName = getElectroTechHeaderName(headerNameTbl, 6, 2, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfInnerTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, PaddingTop = 1, PaddingBottom = 1, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfInnerTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { });
                    }
                    pdfInnerTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)));
                    headerName = getElectroTechHeaderName(headerNameTbl, 6, 3, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfInnerTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, PaddingTop = 1, PaddingBottom = 1, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfInnerTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { });
                    }
                    pdfInnerTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)));
                    headerName = getElectroTechHeaderName(headerNameTbl, 6, 4, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfInnerTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, PaddingTop = 1, PaddingBottom = 1, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfInnerTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { });
                    }
                    pdfInnerTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)));
                    pdfTable.AddCell(pdfInnerTable);

                    headerName = getElectroTechHeaderName(headerNameTbl, 7, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, PaddingLeft = paddingleftValue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Padding = 0, PaddingLeft = paddingleftValue });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Colspan = 3, MinimumHeight = 7 });

                    PdfPTable remarksTbl = new PdfPTable(2);
                    int[] tblCellWidth3 = { 100, 300 };
                    remarksTbl.SetWidths(tblCellWidth3);
                    headerName = getElectroTechHeaderName(headerNameTbl, 8, 1, out isChecked, out value);
                    remarksTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeaderUnderLine(headerName, 7)) { Border = 0 });
                    remarksTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(value)) { Border = 0, MinimumHeight = 30 });
                    pdfTable.AddCell(new PdfPCell(remarksTbl) { Colspan = 3, Border = 0 });
                    electoTechTbl.AddCell(pdfTable);

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Colspan = 3, MinimumHeight = 7 });

                    pdfTable = new PdfPTable(3);
                    int[] tblCellWidth4 = { 8, 5, 250 };
                    pdfTable.SetWidths(tblCellWidth4);
                    pdfTable.WidthPercentage = 100;
                    headerName = getElectroTechHeaderName(headerNameTbl, 9, 1, out isChecked, out value);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeaderUnderLine(headerName, 7)) { Border = 0, Colspan = 3 });
                    headerName = getElectroTechHeaderName(headerNameTbl, 10, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)));
                    headerName = getElectroTechHeaderName(headerNameTbl, 11, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Padding = 0, PaddingLeft = paddingleftValue });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Colspan = 3, MinimumHeight = 7 });

                    headerName = getElectroTechHeaderName(headerNameTbl, 12, 1, out isChecked, out value);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeaderUnderLine(headerName, 7)) { Border = 0, Colspan = 3 });
                    headerName = getElectroTechHeaderName(headerNameTbl, 13, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Padding = 0, PaddingLeft = paddingleftValue });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Colspan = 3, MinimumHeight = 7 });

                    headerName = getElectroTechHeaderName(headerNameTbl, 14, 1, out isChecked, out value);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeaderUnderLine(headerName, 7)) { Border = 0, Colspan = 3 });
                    headerName = getElectroTechHeaderName(headerNameTbl, 15, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Padding = 0, PaddingLeft = paddingleftValue });
                    headerName = getElectroTechHeaderName(headerNameTbl, 16, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Padding = 0, PaddingLeft = paddingleftValue });
                    headerName = getElectroTechHeaderName(headerNameTbl, 17, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Padding = 0, PaddingLeft = paddingleftValue });
                    headerName = getElectroTechHeaderName(headerNameTbl, 18, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Padding = 0, PaddingLeft = paddingleftValue });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Colspan = 3, MinimumHeight = 7 });

                    headerName = getElectroTechHeaderName(headerNameTbl, 19, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Padding = 0, PaddingLeft = paddingleftValue });
                    headerName = getElectroTechHeaderName(headerNameTbl, 20, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Padding = 0, PaddingLeft = paddingleftValue });
                    headerName = getElectroTechHeaderName(headerNameTbl, 21, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfInnerTable = new PdfPTable(3);
                    // int[] tblCellWidth4 = { 8, 5, 250 };
                    //  pdfTable.SetWidths(tblCellWidth4);
                    pdfInnerTable.WidthPercentage = 100;
                    pdfInnerTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Border = 0, Padding = 0 });
                    PdfPTable pdfInnerTbl2 = new PdfPTable(1);
                    pdfInnerTbl2.WidthPercentage = 100;
                    headerName = getElectroTechHeaderName(headerNameTbl, 21, 2, out isChecked, out value);
                    pdfInnerTbl2.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName + value)) { Border = 0 });
                    headerName = getElectroTechHeaderName(headerNameTbl, 21, 5, out isChecked, out value);
                    pdfInnerTbl2.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName + value)) { Border = 0 });
                    pdfInnerTable.AddCell(new PdfPCell(pdfInnerTbl2) { Border = 0 });

                    pdfInnerTbl2 = new PdfPTable(2);
                    pdfInnerTbl2.WidthPercentage = 100;
                    headerName = getElectroTechHeaderName(headerNameTbl, 21, 3, out isChecked, out value);
                    pdfInnerTbl2.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName + value)) { Border = 0 });
                    headerName = getElectroTechHeaderName(headerNameTbl, 21, 4, out isChecked, out value);
                    pdfInnerTbl2.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName + "   > " + value + "   %")) { Border = 0 });
                    headerName = getElectroTechHeaderName(headerNameTbl, 21, 6, out isChecked, out value);
                    pdfInnerTbl2.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName + value)) { Border = 0 });
                    headerName = getElectroTechHeaderName(headerNameTbl, 21, 7, out isChecked, out value);
                    pdfInnerTbl2.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName + "   > " + value + "   %")) { Border = 0 });
                    pdfInnerTable.AddCell(new PdfPCell(pdfInnerTbl2) { Border = 0 });
                    pdfTable.AddCell(pdfInnerTable);

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Colspan = 3, MinimumHeight = 7 });

                    headerName = getElectroTechHeaderName(headerNameTbl, 22, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Padding = 0, PaddingLeft = paddingleftValue });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Colspan = 3, MinimumHeight = 7 });

                    headerName = getElectroTechHeaderName(headerNameTbl, 23, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Border = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    headerName = getElectroTechHeaderName(headerNameTbl, 24, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Border = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    headerName = getElectroTechHeaderName(headerNameTbl, 25, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Border = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    headerName = getElectroTechHeaderName(headerNameTbl, 26, 1, out isChecked, out value);
                    if (isChecked)
                    {
                        pdfTable.AddCell(new PdfPCell(checkedPng, false) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 0, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Padding = 0, PaddingLeft = paddingleftValue });
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Padding = 0, PaddingLeft = paddingleftValue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(headerName)) { Border = 0, Padding = 0, PaddingLeft = paddingleftValue });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Colspan = 3, MinimumHeight = 7 });

                    pdfInnerTable = new PdfPTable(2);
                    pdfInnerTable.WidthPercentage = 100;
                    pdfInnerTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Checked by: " + checkedBy, 7)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    pdfInnerTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Datum: " + checkedDate, 7)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT, Padding = 0 });
                    pdfTable.AddCell(new PdfPCell(pdfInnerTable) { Border = 0, Colspan = 3 });

                    electoTechTbl.AddCell(pdfTable);
                }
                else
                {
                    ReportStatus = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return electoTechTbl;
        }
        private static string getElectroTechHeaderName(DataTable dt, int headerid, int subheaderid, out bool isChecked, out string value)
        {
            value = "";
            isChecked = false;
            string headerName = "";
            try
            {
                headerName = dt.AsEnumerable().Where(k => k.Field<int>("HeaderID") == headerid && k.Field<int>("SubHeaderID") == subheaderid).Select(k => k.Field<string>("ValueInText")).First();
                if (headerName != null)
                {
                    headerName = headerName.Replace("?", "\n");
                }

                value = dt.AsEnumerable().Where(k => k.Field<int>("HeaderID") == headerid && k.Field<int>("SubHeaderID") == subheaderid).Select(k => k.Field<string>("Checked")).First();
                if (value != null)
                {
                    if (string.Equals(value, "Ok", StringComparison.OrdinalIgnoreCase))
                    {
                        isChecked = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return headerName;
        }
        #endregion

        #region  ------- Testing Report ---------
        internal static string testingReport(string ProductionOrder, string FabricationNo, string componentid)
        {
            string ReportStatus = string.Empty;
            try
            {
                componentid = GEADatabaseAccess.GetComponentIDNumber(ProductionOrder, FabricationNo);
                PdfPTable mainTbl = testingReportTbl(ProductionOrder, FabricationNo, componentid, 20, false, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    Document pdfDoc = new Document(PageSize.A4, 15, 15, 25, 25);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();
                    pdfDoc.Add(mainTbl);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=TestingReport.pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }

        internal static PdfPTable testingReportTbl(string ProductionOrder, string FabricationNo, string componentid, int mainTblSpaceBefore, bool isFromFinalReport, out string ReportStatus)
        {
            ReportStatus = string.Empty;
            PdfPTable mainTbl = new PdfPTable(1);
            try
            {
                BalancingProductionOrder = "";
                DataTable headerNameTbl;
                componentid = GEADatabaseAccess.GetComponentIDNumber(ProductionOrder, FabricationNo);
                string machineid = GEADatabaseAccess.getMachineIDForTestingPackingReport("Testing");
                DataTable machineDataTbl = GEADatabaseAccess.GetTestingReportData(ProductionOrder, FabricationNo, componentid, machineid, out headerNameTbl);
                if (machineDataTbl.Rows.Count > 0 || headerNameTbl.Rows.Count > 0)
                {
                    string machineType = "", checkedBy = "", checkedDate = "", formatNo = "", revNo = "";
                    if (headerNameTbl.Rows.Count > 0)
                    {
                        machineType = headerNameTbl.Rows[0]["MachineType"] == null ? "" : headerNameTbl.Rows[0]["MachineType"].ToString();
                        checkedBy = headerNameTbl.Rows[0]["UpdatedBy"] == null ? "" : headerNameTbl.Rows[0]["UpdatedBy"].ToString();
                        if (headerNameTbl.Rows[0]["UpdatedTS"] != null && headerNameTbl.Rows[0]["UpdatedTS"].ToString() != "")
                        {
                            checkedDate = Convert.ToDateTime(headerNameTbl.Rows[0]["UpdatedTS"]).ToString("dd-MMM-yy");
                        }
                        formatNo = headerNameTbl.Rows[0]["FormatNo"] == null ? "" : headerNameTbl.Rows[0]["FormatNo"].ToString();
                        revNo = headerNameTbl.Rows[0]["RevNo"] == null ? "" : headerNameTbl.Rows[0]["RevNo"].ToString();
                        BalancingProductionOrder = headerNameTbl.Rows[0]["BalancingProductionOrder"] == null ? "" : headerNameTbl.Rows[0]["BalancingProductionOrder"].ToString();
                    }

                    mainTbl.SplitLate = false;
                    mainTbl.WidthPercentage = 100;
                    mainTbl.SpacingBefore = mainTblSpaceBefore;
                    mainTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    mainTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    //header
                    PdfPTable headerTbl = new PdfPTable(7);
                    headerTbl.WidthPercentage = 100;
                    byte[] file1;
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(getGEALogoPath()));//ImagePath  
                    iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(70f, 60f);
                    PdfPCell logoCell = new PdfPCell(geaLogo, false);
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerTbl.AddCell(new PdfPCell(logoCell) { Colspan = 1 });

                    PdfPTable pdfTable = new PdfPTable(1);
                    pdfTable.WidthPercentage = 100;
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Decanter checklist - FAT", 9)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderWidthBottom = 0 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("complete workmanship, protocols", 6)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0, FixedHeight = 10 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("and dokumentation to deliver", 6)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderWidthTop = 0, FixedHeight = 7 });
                    headerTbl.AddCell(new PdfPCell(pdfTable) { Colspan = 4, Border = 0 });

                    PdfPTable formateDataTbl = new PdfPTable(1);
                    formateDataTbl.WidthPercentage = 100;
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Format No: " + formatNo)) { Border = 0 });
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Rev No: " + revNo)) { Border = 0 });
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Date: " + "14-Feb-17")) { Border = 0 });
                    headerTbl.AddCell(new PdfPCell(formateDataTbl) { HorizontalAlignment = Element.ALIGN_LEFT, Colspan = 2 });
                    mainTbl.AddCell(new PdfPCell(headerTbl));

                    if (machineDataTbl.Rows.Count > 0)
                    {
                        PdfPTable machineScheduleTbl = new PdfPTable(4);
                        machineScheduleTbl.TotalWidth = 600;
                        if (isFromFinalReport)
                        {
                            int[] tblCellWidth6 = { 0, 100, 400, 200 };
                            machineScheduleTbl.SetWidths(tblCellWidth6);
                        }
                        else
                        {
                            int[] tblCellWidth6 = { 200, 100, 100, 200 };
                            machineScheduleTbl.SetWidths(tblCellWidth6);
                        }
                        machineScheduleTbl.WidthPercentage = 40;
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Type")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineType)) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Machine No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Scroll No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Bowl No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Order No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["SaleOrder"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Customer")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["Customer"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Country")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["Location"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(machineScheduleTbl));
                    }
                    else
                    {
                        PdfPTable machineScheduleTbl = new PdfPTable(4);
                        machineScheduleTbl.TotalWidth = 600;
                        if (isFromFinalReport)
                        {
                            int[] tblCellWidth6 = { 0, 100, 400, 200 };
                            machineScheduleTbl.SetWidths(tblCellWidth6);
                        }
                        else
                        {
                            int[] tblCellWidth6 = { 200, 100, 100, 200 };
                            machineScheduleTbl.SetWidths(tblCellWidth6);
                        }
                        machineScheduleTbl.WidthPercentage = 40;
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Type")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Machine No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Scroll No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Bowl No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Order No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Customer")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Country")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(machineScheduleTbl));
                    }

                    pdfTable = new PdfPTable(3);
                    int[] tblCellWidth2 = { 300, 30, 80 };
                    pdfTable.SetWidths(tblCellWidth2);
                    pdfTable.WidthPercentage = 100;
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Before dismantling the machine on the testified")));
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Done")) { Rowspan = 2, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Date, Name")) { Rowspan = 2, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Vor dem Demontieren auf dem Prufstand")) { });
                    // pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { });
                    // pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { });
                    for (int i = 0; i < headerNameTbl.Rows.Count; i++)
                    {
                        string parameter = headerNameTbl.Rows[i]["Parameter"].ToString();
                        if (parameter.Equals("Stromaufnahme Hauptmotor bei Wasserdurchsatz,notiert?Current (Amp) of mainmotor with water througput into decanter, noted         ", StringComparison.OrdinalIgnoreCase))
                        {
                            string ampValue = headerNameTbl.Rows[i]["Amps"].ToString();
                            parameter = parameter.Replace("(Amp)", "(" + ampValue + " Amp)");
                        }
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getDecadeChecklistPackingHeaderName(parameter))) { PaddingTop = 3, PaddingBottom = 3 });
                        if (string.Equals(headerNameTbl.Rows[i]["Checked"].ToString(), "Ok", StringComparison.OrdinalIgnoreCase))
                        {
                            byte[] checkfile1;
                            checkfile1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/Check.png"));//ImagePath  
                            iTextSharp.text.Image checkjpg = iTextSharp.text.Image.GetInstance(checkfile1);
                            checkjpg.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                            checkjpg.ScaleToFit(10f, 10f);
                            PdfPCell cell = new PdfPCell(checkjpg, false);
                            cell.BorderWidth = 1;
                            cell.Padding = 2;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            pdfTable.AddCell(cell).BorderColor = new BaseColor(122, 121, 121);
                        }
                        else
                        {
                            byte[] uncheckfile;
                            uncheckfile = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/Uncheck.png"));//ImagePath  
                            iTextSharp.text.Image uncheckjpg = iTextSharp.text.Image.GetInstance(uncheckfile);
                            uncheckjpg.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                            uncheckjpg.ScaleToFit(10f, 10f);
                            PdfPCell cell = new PdfPCell(uncheckjpg, false);
                            cell.BorderWidth = 1;
                            cell.Padding = 2;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            pdfTable.AddCell(cell).BorderColor = new BaseColor(122, 121, 121);
                        }
                        string updatedTS = string.IsNullOrEmpty(headerNameTbl.Rows[i]["UpdatedTS"].ToString()) ? "" : (Convert.ToDateTime(headerNameTbl.Rows[i]["UpdatedTS"].ToString()).ToString("dd-MMM-yy"));
                        string updatedBy = string.IsNullOrEmpty(headerNameTbl.Rows[i]["UpdatedBy"].ToString()) ? "" : headerNameTbl.Rows[i]["UpdatedBy"].ToString();
                        if (!string.IsNullOrEmpty(updatedBy))
                        {
                            updatedBy = updatedTS + ", " + updatedBy;
                        }
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(updatedBy)) { VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    mainTbl.AddCell(pdfTable);
                }
                else
                {
                    ReportStatus = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
            }
            return mainTbl;
        }
        #endregion

        #region  ------- Vibration Test Protocol Report ---------
        internal static string vibrationTestProtocolReport(string ProductionOrder, string FabricationNo, string componentid)
        {
            string ReportStatus = string.Empty;
            try
            {
                componentid = GEADatabaseAccess.GetComponentIDNumber(ProductionOrder, FabricationNo);
                PdfPTable mainTbl = vibrationTestProtocolReport(ProductionOrder, FabricationNo, componentid, 20, false, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    Document pdfDoc = new Document(PageSize.A4, 15, 15, 25, 25);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();
                    pdfDoc.Add(mainTbl);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=VibrationTestProtocol.pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }

        internal static PdfPTable vibrationTestProtocolReport(string ProductionOrder, string FabricationNo, string componentid, int mainTblSpaceBefore, bool isFromFinalReport, out string ReportStatus)
        {
            ReportStatus = string.Empty;
            PdfPTable mainTbl = new PdfPTable(1);
            try
            {
                DataTable remarksMeasureCondTbl, measuredValueTbl;
                componentid = GEADatabaseAccess.GetComponentIDNumber(ProductionOrder, FabricationNo);
                string machineid = GEADatabaseAccess.getMachineIDForTestingPackingReport("Testing");
                DataTable machineDataTbl = GEADatabaseAccess.GetVibrationTestProtocolReportData(ProductionOrder, FabricationNo, componentid, machineid, out remarksMeasureCondTbl, out measuredValueTbl);
                if (machineDataTbl.Rows.Count > 0 || remarksMeasureCondTbl.Rows.Count > 0 || measuredValueTbl.Rows.Count > 0)
                {
                    string machineType = "", checkedBy = "", checkedDate = "", signatureBy = "";
                    if (remarksMeasureCondTbl.Rows.Count > 0)
                    {
                        machineType = remarksMeasureCondTbl.Rows[0]["MachineType"] == null ? "" : remarksMeasureCondTbl.Rows[0]["MachineType"].ToString();
                        checkedBy = remarksMeasureCondTbl.Rows[0]["UpdatedBy"] == null ? "" : remarksMeasureCondTbl.Rows[0]["UpdatedBy"].ToString();
                        if (remarksMeasureCondTbl.Rows[0]["UpdatedTS"] != null && remarksMeasureCondTbl.Rows[0]["UpdatedTS"].ToString() != "")
                        {
                            checkedDate = Convert.ToDateTime(remarksMeasureCondTbl.Rows[0]["UpdatedTS"]).ToString("dd-MMM-yy");
                        }
                        signatureBy = remarksMeasureCondTbl.Rows[0]["SignatureBy"] == null ? "" : remarksMeasureCondTbl.Rows[0]["SignatureBy"].ToString();
                    }

                    mainTbl.SplitLate = false;
                    mainTbl.WidthPercentage = 100;
                    mainTbl.SpacingBefore = mainTblSpaceBefore;
                    mainTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    mainTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    //header
                    PdfPTable headerTbl = new PdfPTable(7);
                    headerTbl.WidthPercentage = 100;
                    PdfPTable pdfTable = new PdfPTable(1);
                    pdfTable.WidthPercentage = 100;
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Decanter Acceptance", 9)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderWidthBottom = 0 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Vibration Test Protocol", 7)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderWidthTop = 0 });
                    headerTbl.AddCell(new PdfPCell(pdfTable) { Colspan = 6 });
                    byte[] file1;
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(getGEALogoPath()));//ImagePath  
                    iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(70f, 60f);
                    PdfPCell logoCell = new PdfPCell(geaLogo, false);
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerTbl.AddCell(new PdfPCell(logoCell) { Colspan = 1 });
                    mainTbl.AddCell(new PdfPCell(headerTbl));

                    if (machineDataTbl.Rows.Count > 0)
                    {
                        PdfPTable machineScheduleTbl = new PdfPTable(4);
                        machineScheduleTbl.TotalWidth = 600;
                        if (isFromFinalReport)
                        {
                            int[] tblCellWidth1 = { 0, 100, 400, 200 };
                            machineScheduleTbl.SetWidths(tblCellWidth1);
                        }
                        else
                        {
                            int[] tblCellWidth1 = { 200, 100, 100, 200 };
                            machineScheduleTbl.SetWidths(tblCellWidth1);
                        }
                        machineScheduleTbl.WidthPercentage = 40;
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Type")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineType)) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Machine No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Scroll No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Bowl No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Order No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["SaleOrder"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Customer")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["Customer"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Country")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["Location"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(machineScheduleTbl));
                    }
                    else
                    {
                        PdfPTable machineScheduleTbl = new PdfPTable(4);
                        machineScheduleTbl.TotalWidth = 600;
                        if (isFromFinalReport)
                        {
                            int[] tblCellWidth1 = { 0, 100, 400, 200 };
                            machineScheduleTbl.SetWidths(tblCellWidth1);
                        }
                        else
                        {
                            int[] tblCellWidth1 = { 200, 100, 100, 200 };
                            machineScheduleTbl.SetWidths(tblCellWidth1);
                        }
                        machineScheduleTbl.WidthPercentage = 40;
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Type")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Machine No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Scroll No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Bowl No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Order No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Customer")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Country")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(machineScheduleTbl));
                    }

                    //pdfTable = new PdfPTable(1);
                    //pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Admissible vibration severity (sum vibration)")) { Border = 0 });
                    //pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0 });
                    //pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Measured variable:  Veff (mm/s)")) { Border = 0 });
                    //pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0 });
                    //mainTbl.AddCell(new PdfPCell(pdfTable));

                    //headerTbl.AddCell(new PdfPCell(pdfTable) { Colspan = 6 });
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/GEA/Icons/VibrationTest1.PNG"));//ImagePath  
                    geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    //geaLogo.ScaleToFit(50f, 50f);
                    logoCell = new PdfPCell(geaLogo, true);
                    //logoCell.MinimumHeight = 200;
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    mainTbl.AddCell(new PdfPCell(logoCell) { BorderWidth = 1 });

                    //pdfTable = new PdfPTable(1);
                    //pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Measuing point C only on the machine version with clutch housing on the main drive, ")) { Border = 0 });
                    //pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Measuring point E only on the machine version with clutch housing at secondary drive and")) { Border = 0 });
                    //pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Measuring point F only on machine version with secondary motor.")) { Border = 0 });
                    //pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0 });
                    //mainTbl.AddCell(new PdfPCell(pdfTable));

                    pdfTable = new PdfPTable(4);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 4, BorderWidthTop = 0, MinimumHeight = 1 });
                    PdfPTable innerTbl = new PdfPTable(1);
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, MinimumHeight = 10 });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeaderUnderLine("Measuring conditions:", 7)) { Border = 0 });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, MinimumHeight = 10 });
                    innerTbl.AddCell(new PdfPCell(getPdfCellForVibration("")) { Border = 0 });

                    PdfPTable innerInnerTbl = new PdfPTable(7);
                    int[] tblCellWidth0 = { 15, 5, 15, 5, 40, 20, 40 };
                    innerInnerTbl.SetWidths(tblCellWidth0);
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Bowl: ")) { Border = 0 });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("   ")) { Border = 0 });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("   ")) { });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("   ")) { Border = 0 });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("filled with water")) { Border = 0 });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("   ")) { Border = 0 });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("   ")) { Border = 0 });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("   ")) { Border = 0 });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("   ")) { Border = 0 });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("X")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("   ")) { Border = 0 });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("with water throughput")) { Border = 0 });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getVibrationTestReportParameterValue(remarksMeasureCondTbl, "Filled with water with water throughput"))) { Border = 0, BorderWidthBottom = 1, HorizontalAlignment = Element.ALIGN_CENTER });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("l/h")) { Border = 0 });
                    innerTbl.AddCell(new PdfPCell(innerInnerTbl) { Border = 0 });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Colspan = 4, MinimumHeight = 10 });

                    innerInnerTbl = new PdfPTable(4);
                    int[] tblCellWidth6 = { 50, 100, 50, 200 };
                    innerInnerTbl.SetWidths(tblCellWidth6);
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Speeds: ")) { Border = 0 });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Bowl: ")) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getVibrationTestReportParameterValue(remarksMeasureCondTbl, "Bowl Speed"))) { Border = 0 });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("1/min")) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0 });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Motor: ")) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getVibrationTestReportParameterValue(remarksMeasureCondTbl, "Motor Speed"))) { Border = 0 });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("1/min")) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0 });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Differential speed: ")) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getVibrationTestReportParameterValue(remarksMeasureCondTbl, "Differential Speed"))) { Border = 0 });
                    innerInnerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("1/min")) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    innerTbl.AddCell(new PdfPCell(innerInnerTbl) { Border = 0 });


                    pdfTable.AddCell(new PdfPCell(innerTbl) { Colspan = 2, Border = 0 });
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/GEA/Icons/VibrationTest2.PNG"));//ImagePath  
                    geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    //geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(50f, 50f);
                    logoCell = new PdfPCell(geaLogo, true);
                    logoCell.VerticalAlignment = Element.ALIGN_CENTER;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    logoCell.Border = 0;
                    pdfTable.AddCell(new PdfPCell(logoCell) { Colspan = 2, Border = 0, BorderWidthBottom = 1, BorderWidthRight = 1 });
                    // pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("*) Measuring point for \n deceleration curve")) { Border = 0,VerticalAlignment=Element.ALIGN_CENTER
                    // }) ;
                    mainTbl.AddCell(new PdfPCell(pdfTable));

                    //mainTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Measured values: (mm/s)")) { Border = 0 });
                    pdfTable = new PdfPTable(1);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, MinimumHeight = 10 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Measured values: (mm/s)")) { Border = 0 });
                    mainTbl.AddCell(new PdfPCell(pdfTable));

                    pdfTable = new PdfPTable(8);
                    //pdfTable.SpacingAfter = 50;
                    //pdfTable.SpacingAfter = 50;
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Measuring direction \n Measuring point")) { Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                    innerTbl = new PdfPTable(2);
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("horizontal")) { Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("min.")) { Colspan = 1, HorizontalAlignment = Element.ALIGN_CENTER });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("max.")) { Colspan = 1, HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(innerTbl) { Colspan = 2 });
                    innerTbl = new PdfPTable(2);
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("vertical")) { Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("min.")) { Colspan = 1, HorizontalAlignment = Element.ALIGN_CENTER });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("max.")) { Colspan = 1, HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(innerTbl) { Colspan = 2 });
                    innerTbl = new PdfPTable(2);
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("axial")) { Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("min.")) { Colspan = 1, HorizontalAlignment = Element.ALIGN_CENTER });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("max.")) { Colspan = 1, HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(innerTbl) { Colspan = 2 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("A")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Solids side")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[0]["HorizontalMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[0]["HorizontalMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[0]["VerticalMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[0]["VerticalMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[0]["AxialMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[0]["AxialMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("B")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Liquid side")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[1]["HorizontalMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[1]["HorizontalMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[1]["VerticalMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[1]["VerticalMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[1]["AxialMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[1]["AxialMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });


                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("C")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Clutch housing \n Main drive")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[2]["HorizontalMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[2]["HorizontalMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[2]["VerticalMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[2]["VerticalMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[2]["AxialMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[2]["AxialMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("D")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Main motor")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[3]["HorizontalMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[3]["HorizontalMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[3]["VerticalMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[3]["VerticalMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[3]["AxialMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[3]["AxialMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("E")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Clutch housing \n Secondary drive")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[4]["HorizontalMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[4]["HorizontalMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[4]["VerticalMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[4]["VerticalMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[4]["AxialMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[4]["AxialMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("F")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Secondary motor")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[5]["HorizontalMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[5]["HorizontalMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[5]["VerticalMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[5]["VerticalMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[5]["AxialMin"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuredValueTbl.Rows[5]["AxialMax"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 8, MinimumHeight = 10 });
                    mainTbl.AddCell(new PdfPCell(pdfTable));

                    mainTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Remarks:")) { BorderWidthBottom = 0 });

                    pdfTable = new PdfPTable(4);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Balancing mass")) { Rowspan = 2, Colspan = 1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Solid side screws secured with loctite")) { Colspan = 2 });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getVibrationTestReportParameterValue(remarksMeasureCondTbl, "Solid side screws secured with loctite"))) { Colspan = 1 });
                    // pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Lequid side leads secured by punching")) { Colspan = 2 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getVibrationTestReportParameterValue(remarksMeasureCondTbl, "Lequid side leads secured by punching"))) { Colspan = 1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Bearing tempreture")) { Rowspan = 3, Colspan = 1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("stabilized tempreture ")) { Colspan = 1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Greased ____gm \n grease each bearing max tempreture")) { Colspan = 1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("After greasing stabilized \n tempreture ")) { Colspan = 1 });
                    //pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getVibrationTestReportParameterValue(remarksMeasureCondTbl, "Stabilized tempreture min"))) { Colspan = 1 });
                    //pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("ka")) { Colspan = 1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getVibrationTestReportParameterValue(remarksMeasureCondTbl, "Greased_gm grease each bearing min"))) { Colspan = 1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getVibrationTestReportParameterValue(remarksMeasureCondTbl, "After greasing stabilized tempreture min"))) { Colspan = 1 });
                    //pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getVibrationTestReportParameterValue(remarksMeasureCondTbl, "Stabilized tempreture max"))) { Colspan = 1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getVibrationTestReportParameterValue(remarksMeasureCondTbl, "Greased_gm grease each bearing max"))) { Colspan = 1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getVibrationTestReportParameterValue(remarksMeasureCondTbl, "After greasing stabilized tempreture max"))) { Colspan = 1 });
                    mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });

                    pdfTable = new PdfPTable(3);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Bengaluru: " + checkedDate)) { Border = 0 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Name: " + checkedBy)) { Border = 0 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Signature: " + signatureBy)) { Border = 0 });
                    mainTbl.AddCell(pdfTable);
                }
                else
                {
                    ReportStatus = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
            }
            return mainTbl;
        }
        private static string getVibrationTestReportParameterValue(DataTable dt, string parameterid)
        {
            string result = "-";
            try
            {
                var remarksVal = dt.AsEnumerable().Where(k => k.Field<string>("ParameterID") == parameterid).FirstOrDefault();
                if (remarksVal != null)
                {
                    result = remarksVal["ParameterValue"].ToString();
                    if (result == "")
                    {
                        result = "-";
                    }
                }
            }
            catch (Exception ex)
            { }
            return result;
        }
        #endregion

        #region ----- Noise Measurement Report -----
        internal static string NoiseMeasurementReport(string ProductionOrder, string FabricationNo, string componentid)
        {
            string ReportStatus = string.Empty;
            try
            {
                componentid = GEADatabaseAccess.GetComponentIDNumber(ProductionOrder, FabricationNo);
                PdfPTable mainTbl = NoiseMeasurementReport(ProductionOrder, FabricationNo, componentid, 20, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 15, 15, 25, 25);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();
                    pdfDoc.Add(mainTbl);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=NoiseMeasurementReport.pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }

        internal static PdfPTable NoiseMeasurementReport(string ProductionOrder, string FabricationNo, string componentid, int mainTblSpaceBefore, out string ReportStatus)
        {
            ReportStatus = string.Empty;
            PdfPTable mainTbl = new PdfPTable(3);
            try
            {
                if (GEADatabaseAccess.isProductionOrderProMaterial(ProductionOrder, FabricationNo, "NoiseMeasurement"))
                {
                    componentid = GEADatabaseAccess.GetComponentIDNumber(ProductionOrder, FabricationNo);
                    DataTable machineDimensionTbl, speedCapacityTbl, measuringPointTbl;
                    string machineid = GEADatabaseAccess.getMachineIDForTestingPackingReport("Testing");
                    DataTable machineDataTbl = GEADatabaseAccess.GetProNoiseMeasurementProtocolReportData(ProductionOrder, FabricationNo, componentid, machineid, out machineDimensionTbl, out speedCapacityTbl, out measuringPointTbl);
                    if (machineDataTbl.Rows.Count > 0 || machineDimensionTbl.Rows.Count > 0 || speedCapacityTbl.Rows.Count > 0 || measuringPointTbl.Rows.Count > 0)
                    {
                        string machineType = "", checkedBy = "", checkedDate = "", customer = "", orderno = "", location = "", inspector = "";
                        if (speedCapacityTbl.Rows.Count > 0)
                        {
                            machineType = speedCapacityTbl.Rows[0]["MachineType"] == null ? "" : speedCapacityTbl.Rows[0]["MachineType"].ToString();
                            checkedBy = speedCapacityTbl.Rows[0]["UpdatedBy"] == null ? "" : speedCapacityTbl.Rows[0]["UpdatedBy"].ToString();
                            if (speedCapacityTbl.Rows[0]["UpdatedTS"] != null && speedCapacityTbl.Rows[0]["UpdatedTS"].ToString() != "")
                            {
                                checkedDate = Convert.ToDateTime(speedCapacityTbl.Rows[0]["UpdatedTS"]).ToString("dd-MMM-yy");
                            }
                            inspector = checkedBy;
                        }
                        string machineDimension = "";
                        double machineDimValue = 0;
                        if (machineDimensionTbl.Rows.Count > 0)
                        {
                            var row = machineDimensionTbl.AsEnumerable().Where(k => k.Field<string>("MaterialID") == componentid).FirstOrDefault();
                            if (row != null)
                            {
                                machineDimension = row["Dimension"].ToString();
                                if (row["Value"].ToString() != "" || row["Value"] != null)
                                {
                                    machineDimValue = Convert.ToDouble(row["Value"].ToString());
                                }
                            }

                        }
                        if (machineDataTbl.Rows.Count > 0)
                        {
                            orderno = machineDataTbl.Rows[0]["SaleOrder"].ToString();
                            customer = machineDataTbl.Rows[0]["Customer"].ToString();
                            location = machineDataTbl.Rows[0]["Location"].ToString();
                        }

                        mainTbl.SplitLate = false;
                        mainTbl.WidthPercentage = 100;
                        mainTbl.SpacingBefore = mainTblSpaceBefore;
                        mainTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        mainTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                        //header
                        PdfPTable pdfPTable = new PdfPTable(4);
                        pdfPTable.WidthPercentage = 100;
                        //pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Reprot", 12)) { Colspan = 4 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Geräuschmessbericht / Noise Measurement Report", 10)) { Colspan = 4 });

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Messvorschrift / Measurement Method EN ISO 3746")) { Colspan = 4 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Maschinendaten / Machine data")) { Colspan = 4 });

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Maschinentyp / Machine type:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(machineType)));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Datum / Date:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(checkedDate)));

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Maschinen-Fabr.-Nr. / Machine fabrication number: ")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(FabricationNo)));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Ort der Messung / Place of measurement: ")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("BANGALORE")));

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Auftragsnummer / Order number:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(orderno)));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Prüfer / Inspector:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(inspector)));

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Kunde / Customer:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(customer)));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Gerüst / Scaffold:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("nein")));

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Abmessungen der Maschine / Dimensions of the machine:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(machineDimension)));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Austragsschächte / Discharge chutes: ")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("offen")));

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Höhe Sockel / Height of base:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getNoiseMeasurementSpeedCapacityValue(speedCapacityTbl, "Höhe Sockel / Height of base:", "m"))));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Be- / Entlüftung / Ventilation:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("offen")));

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Trommeldrehzahl / Bowl speed:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getNoiseMeasurementSpeedCapacityValue(speedCapacityTbl, "Trommeldrehzahl / Bowl speed:", "/min"))));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Zulaufleistung / Capacity:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getNoiseMeasurementSpeedCapacityValue(speedCapacityTbl, "Zulaufleistung / Capacity:", "m³/h"))));


                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Differenzdrehzahl / Differential speed:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getNoiseMeasurementSpeedCapacityValue(speedCapacityTbl, "Differenzdrehzahl / Differential speed:", "/min"))));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Wasser / Water")));


                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Motortyp / Motor type:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getNoiseMeasurementSpeedCapacityValue(speedCapacityTbl, "Motortyp / Motor type:", ""))));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Ablaufdruck / Discharge pressure:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("0 bar")));


                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Drehzahl / Speed:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getNoiseMeasurementSpeedCapacityValue(speedCapacityTbl, "Drehzahl / Speed:", "/min"))));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 2 });

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 4, MinimumHeight = 20 });

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Messdaten / Measurement data", 6)) { Colspan = 2 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Schalldämmpaket / Noise absorbing package ", 6)) { Colspan = 2 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Messgerät / Measuring instrument:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Smart Sensor AR814")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 2 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Messabstand / Measurement distance:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getNoiseMeasurementSpeedCapacityValue(speedCapacityTbl, "Messabstand/Measurement distance:", "m"))));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Umgebungskorrektur K2 /\nAmbient noise correction K2:")) { Rowspan = 2 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getNoiseMeasurementSpeedCapacityValue(speedCapacityTbl, "Umgebungskorrektur K2/Ambient noise correction k2 :", "dB"))) { Rowspan = 2, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Messfläche / Measurement surface:")));
                        // pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(machineDimValue + " m²")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getNoiseMeasurementSpeedCapacityValue(speedCapacityTbl, "Messfläche / Measurement surface:", " m²"))));
                        //pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 2 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Messgröße / Measured variable:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Schalldruckpegel / Sound pressure level LA")) { Colspan = 3 });
                        mainTbl.AddCell(new PdfPCell(pdfPTable) { Colspan = 2 });

                        pdfPTable = new PdfPTable(2);
                        pdfPTable.WidthPercentage = 100;
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Messpunkt / Measuring point", 6)) { Rowspan = 4, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = 40 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Schalldruckpegel / Sound pressure level dB(A)", 6)) { Rowspan = 4, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = 40 });

                        double sumOfSoundPressure = 0, sourcePressureAvg;
                        double avgSoundPressure1, avgSoundPressure2, surfaceRatio1, surfaceRatio2, soundPower1A, soundPower2A, soundPower1mW, soundPower2mW;
                        for (int i = 0; i < measuringPointTbl.Rows.Count; i++)
                        {
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuringPointTbl.Rows[i]["ValueInText"].ToString())) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 8, PaddingBottom = 8 });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuringPointTbl.Rows[i]["Checked"].ToString())) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 8, PaddingBottom = 8 });
                            if (measuringPointTbl.Rows[i]["Checked"].ToString() != "")
                            {
                                double PowerValue = Math.Pow(10, (Convert.ToDouble(measuringPointTbl.Rows[i]["Checked"].ToString()) / 10));
                                sumOfSoundPressure += PowerValue;
                            }
                        }
                        if (measuringPointTbl.Rows.Count > 0)
                        {
                            sourcePressureAvg = sumOfSoundPressure / measuringPointTbl.Rows.Count;
                        }
                        sourcePressureAvg = sumOfSoundPressure / measuringPointTbl.Rows.Count;
                        mainTbl.AddCell(new PdfPCell(pdfPTable));

                        try
                        {
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/GEA/Images/NoisePro.png")))
                            {
                                byte[] file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/GEA/Images/NoisePro.png"));//ImagePath  
                                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(file1);
                                image.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                                //image.ScaleAbsolute(50f, 50f);
                                PdfPCell imageCell = new PdfPCell(image, true);
                                imageCell.VerticalAlignment = Element.ALIGN_TOP;
                                imageCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                imageCell.Border = 0;
                                pdfPTable = new PdfPTable(1);
                                pdfPTable.SplitLate = false;
                                pdfPTable.AddCell(new PdfPCell(imageCell));
                                mainTbl.AddCell(new PdfPCell(pdfPTable) { Colspan = 2, Padding = 2 });
                            }
                            else
                            {
                                mainTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 2 });
                            }
                        }
                        catch (Exception ex)
                        {
                            mainTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 2 });
                        }

                        double ambientNoiseCorrection = 0, heightOfBase = 1, measurementSurface = 0, measurementDistance = 0, d1 = 0, d2 = 0, d3 = 0;
                        var remarksVal = speedCapacityTbl.AsEnumerable().Where(k => k.Field<string>("ValueInText") == "Umgebungskorrektur K2/Ambient noise correction k2 :").FirstOrDefault();
                        if (remarksVal != null)
                        {
                            ambientNoiseCorrection = string.IsNullOrEmpty(remarksVal["Checked"].ToString()) ? 0 : Convert.ToDouble(remarksVal["Checked"].ToString());
                        }
                        remarksVal = speedCapacityTbl.AsEnumerable().Where(k => k.Field<string>("ValueInText") == "Messabstand/Measurement distance:").FirstOrDefault();
                        if (remarksVal != null)
                        {
                            measurementDistance = string.IsNullOrEmpty(remarksVal["Checked"].ToString()) ? 0 : Convert.ToDouble(remarksVal["Checked"].ToString());
                        }
                        remarksVal = speedCapacityTbl.AsEnumerable().Where(k => k.Field<string>("ValueInText") == "Höhe Sockel / Height of base:").FirstOrDefault();
                        if (remarksVal != null)
                        {
                            heightOfBase = string.IsNullOrEmpty(remarksVal["Checked"].ToString()) ? 0 : Convert.ToDouble(remarksVal["Checked"].ToString());
                        }
                        if (!string.IsNullOrEmpty(machineDimension))
                        {
                            var _dimensionArr = machineDimension.Split(new char[] { 'X', '*', 'm' }, StringSplitOptions.RemoveEmptyEntries);
                            double.TryParse(_dimensionArr[0].Trim(), out d1);
                            double.TryParse(_dimensionArr[1].Trim(), out d2);
                            double.TryParse(_dimensionArr[2].Trim(), out d3);
                        }
                        measurementSurface = Math.Round(((d1 + (2 * measurementDistance)) * (heightOfBase + d3 + measurementDistance)) * 2 + ((d2 + (2 * measurementDistance)) *
                            (heightOfBase + d3 + measurementDistance)) * 2 + ((d1 + (2 * measurementDistance)) * (d2 + (2 * measurementDistance))), 1);

                        avgSoundPressure1 = Math.Round((10 * (Math.Log10(sourcePressureAvg))), 1) - ambientNoiseCorrection;
                        avgSoundPressure2 = Math.Ceiling(avgSoundPressure1);
                        surfaceRatio1 = Math.Round((10 * Math.Log10(measurementSurface)), 1);
                        surfaceRatio2 = (((Math.Ceiling(surfaceRatio1 * 2.0))) / 2.0);
                        soundPower1A = Math.Round((avgSoundPressure1 + surfaceRatio1), 1);
                        soundPower2A = (avgSoundPressure2 + surfaceRatio2);
                        soundPower1mW = Math.Round((Math.Pow(10, (soundPower1A / 10)) * ((Math.Pow(10, -12)) * 1000)), 1);
                        soundPower2mW = (Math.Ceiling((Math.Round((Math.Pow(10, (soundPower2A / 10)) * ((Math.Pow(10, -12)) * 1000)), 1)) * 2.0) / 2.0);

                        pdfPTable = new PdfPTable(2);
                        pdfPTable.WidthPercentage = 100;
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Ergebnisse berechnet / Results calculated: ")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Ergebnisse eingetragen / Results noted: ")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Gemittelter Schalldruckpegel / Averaged sound pressure level")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Gemittelter Schalldruckpegel / Averaged sound pressure level")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(avgSoundPressure1 + "    dB(A)")) { HorizontalAlignment = Element.ALIGN_CENTER });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(avgSoundPressure2 + "    dB(A)")) { HorizontalAlignment = Element.ALIGN_CENTER });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Messflächenmaß / Measurement surface ratio")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Messflächenmaß / Measurement surface ratio")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(surfaceRatio1 + "    dB(A)")) { HorizontalAlignment = Element.ALIGN_CENTER });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(surfaceRatio2 + "    dB(A)")) { HorizontalAlignment = Element.ALIGN_CENTER });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Schallleistungspegel / Sound power level")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Schallleistungspegel / Sound power level")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(soundPower1A + "    dB(A)")) { HorizontalAlignment = Element.ALIGN_CENTER });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(soundPower2A + "    dB(A)")) { HorizontalAlignment = Element.ALIGN_CENTER });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Schallleistung / Sound power")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Schallleistung / Sound power")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(soundPower1mW + "    mW")) { HorizontalAlignment = Element.ALIGN_CENTER });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(soundPower2mW + "    mW")) { HorizontalAlignment = Element.ALIGN_CENTER });

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Bemerkungen / Notes:")) { Colspan = 2, BorderWidthBottom = 0 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { MinimumHeight = 30, Colspan = 2, Border = 0 });

                        mainTbl.AddCell(new PdfPCell(pdfPTable));
                    }
                    else
                    {
                        ReportStatus = "NoDataFound";
                    }
                }
                else
                {

                    componentid = GEADatabaseAccess.GetComponentIDNumber(ProductionOrder, FabricationNo);
                    DataTable machineDimensionTbl, speedCapacityTbl, measuringPointTbl;
                    string machineid = GEADatabaseAccess.getMachineIDForTestingPackingReport("Testing");
                    DataTable machineDataTbl = GEADatabaseAccess.GetNoiseMeasurementProtocolReportData(ProductionOrder, FabricationNo, componentid, machineid, out machineDimensionTbl, out speedCapacityTbl, out measuringPointTbl);
                    if (machineDataTbl.Rows.Count > 0 || machineDimensionTbl.Rows.Count > 0 || speedCapacityTbl.Rows.Count > 0 || measuringPointTbl.Rows.Count > 0)
                    {
                        string machineType = "", checkedBy = "", checkedDate = "", customer = "", orderno = "", location = "", inspector = "";
                        if (speedCapacityTbl.Rows.Count > 0)
                        {
                            machineType = speedCapacityTbl.Rows[0]["MachineType"] == null ? "" : speedCapacityTbl.Rows[0]["MachineType"].ToString();
                            checkedBy = speedCapacityTbl.Rows[0]["UpdatedBy"] == null ? "" : speedCapacityTbl.Rows[0]["UpdatedBy"].ToString();
                            if (speedCapacityTbl.Rows[0]["UpdatedTS"] != null && speedCapacityTbl.Rows[0]["UpdatedTS"].ToString() != "")
                            {
                                checkedDate = Convert.ToDateTime(speedCapacityTbl.Rows[0]["UpdatedTS"]).ToString("dd-MMM-yy");
                            }
                            inspector = checkedBy;
                        }
                        string machineDimension = "";
                        double machineDimValue = 0;
                        if (machineDimensionTbl.Rows.Count > 0)
                        {
                            var row = machineDimensionTbl.AsEnumerable().Where(k => k.Field<string>("MaterialID") == componentid).FirstOrDefault();
                            if (row != null)
                            {
                                machineDimension = row["Dimension"].ToString();
                                if (row["Value"].ToString() != "" || row["Value"] != null)
                                {
                                    machineDimValue = Convert.ToDouble(row["Value"].ToString());
                                }
                            }

                        }
                        if (machineDataTbl.Rows.Count > 0)
                        {
                            orderno = machineDataTbl.Rows[0]["SaleOrder"].ToString();
                            customer = machineDataTbl.Rows[0]["Customer"].ToString();
                            location = machineDataTbl.Rows[0]["Location"].ToString();
                        }

                        mainTbl.SplitLate = false;
                        mainTbl.WidthPercentage = 100;
                        mainTbl.SpacingBefore = mainTblSpaceBefore;
                        mainTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        mainTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                        //header
                        PdfPTable pdfPTable = new PdfPTable(4);
                        pdfPTable.WidthPercentage = 100;
                        //pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Reprot", 12)) { Colspan = 4 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Geräuschmessbericht / Noise Measurement Report", 10)) { Colspan = 4 });

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Messvorschrift / Measurement Method EN ISO 3746")) { Colspan = 4 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Maschinendaten / Machine data")) { Colspan = 4 });

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Maschinentyp / Machine type:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(machineType)));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Datum / Date:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(checkedDate)));

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Maschinen-Fabr.-Nr. / Machine fabrication number: ")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(FabricationNo)));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Ort der Messung / Place of measurement: ")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("BANGALORE")));

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Auftragsnummer / Order number:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(orderno)));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Prüfer / Inspector:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(inspector)));

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Kunde / Customer:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(customer)));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Gerüst / Scaffold:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("NO")));

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Abmessungen der Maschine / Dimensions of the machine:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(machineDimension)));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Austragsschächte / Discharge chutes: ")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("OPEN")));

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Trommeldrehzahl / Bowl speed:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getNoiseMeasurementSpeedCapacityValue(speedCapacityTbl, "Trommeldrehzahl / Bowl speed:", "/min"))));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Be- / Entlüftung / Ventilation:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("NO")));

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Differenzdrehzahl / Differential speed:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getNoiseMeasurementSpeedCapacityValue(speedCapacityTbl, "Differenzdrehzahl / Differential speed:", "/min"))));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Zulaufleistung / Capacity:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getNoiseMeasurementSpeedCapacityValue(speedCapacityTbl, "Zulaufleistung / Capacity:", "m³/h"))));

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Motortyp / Motor type:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getNoiseMeasurementSpeedCapacityValue(speedCapacityTbl, "Motortyp / Motor type:", ""))));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Wasser / Water")));

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Drehzahl / Speed:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getNoiseMeasurementSpeedCapacityValue(speedCapacityTbl, "Drehzahl / Speed:", "/min"))));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Ablaufdruck / Discharge pressure:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("0 bar")));

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 4, MinimumHeight = 20 });

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Messdaten / Measurement data", 6)) { Colspan = 2 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Schalldämmpaket / Noise absorbing package ", 6)) { Colspan = 2 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Messgerät / Measuring instrument:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("2238 Mediator")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 2 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Messabstand / Measurement distance:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("1 m")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 2 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Messfläche / Measurement surface:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(machineDimValue + " m²")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 2 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Messgröße / Measured variable:")));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Schalldruckpegel / Sound pressure level LA")) { Colspan = 3 });
                        mainTbl.AddCell(new PdfPCell(pdfPTable) { Colspan = 2 });

                        pdfPTable = new PdfPTable(2);
                        pdfPTable.WidthPercentage = 100;
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Messpunkt / Measuring point", 6)) { Rowspan = 4, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = 40 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Schalldruckpegel / Sound pressure level dB(A)", 6)) { Rowspan = 4, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = 40 });

                        double sumOfSoundPressure = 0, sourcePressureAvg;
                        double avgSoundPressure1, avgSoundPressure2, surfaceRatio1, surfaceRatio2, soundPower1A, soundPower2A, soundPower1mW, soundPower2mW;
                        for (int i = 0; i < measuringPointTbl.Rows.Count; i++)
                        {
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuringPointTbl.Rows[i]["ValueInText"].ToString())) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 8, PaddingBottom = 8 });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(measuringPointTbl.Rows[i]["Checked"].ToString())) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 8, PaddingBottom = 8 });
                            if (measuringPointTbl.Rows[i]["Checked"].ToString() != "")
                            {
                                double PowerValue = Math.Pow(10, (Convert.ToDouble(measuringPointTbl.Rows[i]["Checked"].ToString()) / 10));
                                sumOfSoundPressure += PowerValue;
                            }
                        }
                        if (measuringPointTbl.Rows.Count > 0)
                        {
                            sourcePressureAvg = sumOfSoundPressure / measuringPointTbl.Rows.Count;
                        }
                        sourcePressureAvg = sumOfSoundPressure / measuringPointTbl.Rows.Count;
                        mainTbl.AddCell(new PdfPCell(pdfPTable));

                        try
                        {
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/GEA/Images/" + componentid + ".PNG")))
                            {
                                byte[] file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/GEA/Images/" + componentid + ".PNG"));//ImagePath  
                                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(file1);
                                image.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                                //image.ScaleAbsolute(50f, 50f);
                                PdfPCell imageCell = new PdfPCell(image, true);
                                imageCell.VerticalAlignment = Element.ALIGN_TOP;
                                imageCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                imageCell.Border = 0;
                                pdfPTable = new PdfPTable(1);
                                pdfPTable.SplitLate = false;
                                pdfPTable.AddCell(new PdfPCell(imageCell));
                                mainTbl.AddCell(new PdfPCell(pdfPTable) { Colspan = 2, PaddingBottom = 5 });
                            }
                            else
                            {
                                mainTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 2 });
                            }
                        }
                        catch (Exception ex)
                        {
                            mainTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 2 });
                        }


                        avgSoundPressure1 = Math.Round((10 * (Math.Log10(sourcePressureAvg))), 1);
                        avgSoundPressure2 = Math.Ceiling(avgSoundPressure1);
                        surfaceRatio1 = Math.Round((10 * Math.Log10(machineDimValue)), 1);
                        surfaceRatio2 = (((Math.Ceiling(surfaceRatio1 * 2.0))) / 2.0);
                        soundPower1A = Math.Round((avgSoundPressure1 + surfaceRatio1), 1);
                        soundPower2A = (avgSoundPressure2 + surfaceRatio2);
                        soundPower1mW = Math.Round((Math.Pow(10, (soundPower1A / 10)) * ((Math.Pow(10, -12)) * 1000)), 1);
                        soundPower2mW = (Math.Ceiling((Math.Round((Math.Pow(10, (soundPower1A / 10)) * ((Math.Pow(10, -12)) * 1000)), 1)) * 2.0) / 2.0);

                        pdfPTable = new PdfPTable(2);
                        pdfPTable.WidthPercentage = 100;
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Ergebnisse berechnet / Results calculated: ")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Ergebnisse eingetragen / Results noted: ")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Gemittelter Schalldruckpegel / Averaged sound pressure level")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Gemittelter Schalldruckpegel / Averaged sound pressure level")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(avgSoundPressure1 + "    dB(A)")) { HorizontalAlignment = Element.ALIGN_CENTER });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(avgSoundPressure2 + "    dB(A)")) { HorizontalAlignment = Element.ALIGN_CENTER });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Messflächenmaß / Measurement surface ratio")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Messflächenmaß / Measurement surface ratio")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(surfaceRatio1 + "    dB(A)")) { HorizontalAlignment = Element.ALIGN_CENTER });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(surfaceRatio2 + "    dB(A)")) { HorizontalAlignment = Element.ALIGN_CENTER });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Schallleistungspegel / Sound power level")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Schallleistungspegel / Sound power level")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(soundPower1A + "    dB(A)")) { HorizontalAlignment = Element.ALIGN_CENTER });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(soundPower2A + "    dB(A)")) { HorizontalAlignment = Element.ALIGN_CENTER });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Schallleistung / Sound power")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Schallleistung / Sound power")) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(soundPower1mW + "    mW")) { HorizontalAlignment = Element.ALIGN_CENTER });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(soundPower2mW + "    mW")) { HorizontalAlignment = Element.ALIGN_CENTER });

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Bemerkungen / Notes:")) { Colspan = 2, BorderWidthBottom = 0 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { MinimumHeight = 30, Colspan = 2, Border = 0 });

                        mainTbl.AddCell(new PdfPCell(pdfPTable));
                    }
                    else
                    {
                        ReportStatus = "NoDataFound";
                    }
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
            }
            return mainTbl;
        }
        private static string getNoiseMeasurementSpeedCapacityValue(DataTable dt, string parameterid, string unit)
        {
            string result = "";
            try
            {
                var remarksVal = dt.AsEnumerable().Where(k => k.Field<string>("ValueInText") == parameterid).FirstOrDefault();
                if (remarksVal != null)
                {
                    result = remarksVal["Checked"].ToString();
                }
            }
            catch (Exception ex)
            { }
            result = result + " " + unit;
            return result;
        }
        #endregion

        #region -------- Decanter checklist packing -------
        internal static string decanterChecklistPackingReport(string ProductionOrder, string FabricationNo)
        {
            string ReportStatus = string.Empty;
            try
            {
                PdfPTable mainTbl = decanterChecklistPackingReport(ProductionOrder, FabricationNo, 20, false, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    Document pdfDoc = new Document(PageSize.A4, 15, 15, 25, 25);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();
                    pdfDoc.Add(mainTbl);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=DecanterChecklistPacking.pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }

        internal static PdfPTable decanterChecklistPackingReport(string ProductionOrder, string FabricationNo, int mainTblSpaceBefore, bool isFromFinalReport, out string ReportStatus)
        {
            ReportStatus = string.Empty;
            PdfPTable mainTbl = new PdfPTable(1);
            try
            {
                DataTable checklistData;
                string machineid = GEADatabaseAccess.getMachineIDForTestingPackingReport("Packing");
                string param = "NonPro";
                if (GEADatabaseAccess.isProductionOrderProMaterial(ProductionOrder, FabricationNo, "decanterChecklistPacking"))
                {
                    param = "Pro";
                }
                DataTable machineDataTbl = GEADatabaseAccess.GetDecanterChecklistPackingReportData(ProductionOrder, FabricationNo, machineid, out checklistData, param);
                if (machineDataTbl.Rows.Count > 0 || checklistData.Rows.Count > 0)
                {
                    string machineType = "", checkedBy = "", checkedDate = "", formateNo = "", revNo = "";
                    if (checklistData.Rows.Count > 0)
                    {
                        machineType = checklistData.Rows[0]["MachineType"] == null ? "" : checklistData.Rows[0]["MachineType"].ToString();
                        checkedBy = checklistData.Rows[0]["UpdatedBy"] == null ? "" : checklistData.Rows[0]["UpdatedBy"].ToString();
                        if (checklistData.Rows[0]["UpdatedTS"] != null && checklistData.Rows[0]["UpdatedTS"].ToString() != "")
                        {
                            checkedDate = Convert.ToDateTime(checklistData.Rows[0]["UpdatedTS"]).ToString("dd-MMM-yy");
                        }
                        formateNo = checklistData.Rows[0]["FormatNo"] == null ? "" : checklistData.Rows[0]["FormatNo"].ToString();
                        revNo = checklistData.Rows[0]["RevNo"] == null ? "" : checklistData.Rows[0]["RevNo"].ToString();
                    }

                    mainTbl.SplitLate = false;
                    mainTbl.WidthPercentage = 100;
                    mainTbl.SpacingBefore = mainTblSpaceBefore;
                    mainTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    mainTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    //header
                    PdfPTable pdfTable = new PdfPTable(7);
                    pdfTable.WidthPercentage = 100;
                    byte[] file1;
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(getGEALogoPath()));//ImagePath  
                    iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(70f, 60f);
                    PdfPCell logoCell = new PdfPCell(geaLogo, false);
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfTable.AddCell(new PdfPCell(logoCell) { Colspan = 1 });
                    PdfPTable innerTbl = new PdfPTable(1);
                    innerTbl.WidthPercentage = 100;
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Decanter checklist - Packing", 10)) { HorizontalAlignment = Element.ALIGN_CENTER, BorderWidthBottom = 0 });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("complete workmanship, protocols", 7)) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("and dokumentation to deliver", 7)) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 });
                    pdfTable.AddCell(new PdfPCell(innerTbl) { Colspan = 4 });
                    innerTbl = new PdfPTable(1);
                    innerTbl.WidthPercentage = 100;
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Format No: " + formateNo)) { Border = 0 });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Rev No: " + revNo)) { Border = 0 });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Date: " + "21-Dec-15")) { Border = 0 });
                    pdfTable.AddCell(new PdfPCell(innerTbl) { Colspan = 2 });
                    mainTbl.AddCell(new PdfPCell(pdfTable));

                    if (machineDataTbl.Rows.Count > 0)
                    {
                        PdfPTable machineScheduleTbl = new PdfPTable(4);
                        machineScheduleTbl.TotalWidth = 600;
                        if (isFromFinalReport)
                        {
                            int[] tblCellWidth6 = { 0, 100, 400, 200 };
                            machineScheduleTbl.SetWidths(tblCellWidth6);
                        }
                        else
                        {

                            int[] tblCellWidth6 = { 200, 100, 100, 200 };
                            machineScheduleTbl.SetWidths(tblCellWidth6);
                        }
                        machineScheduleTbl.WidthPercentage = 40;
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Type")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineType)) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Machine No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Scroll No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Bowl No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Order No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["SaleOrder"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Customer")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["Customer"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Country")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["Location"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(machineScheduleTbl));
                    }
                    else
                    {
                        PdfPTable machineScheduleTbl = new PdfPTable(4);
                        machineScheduleTbl.TotalWidth = 600;
                        if (isFromFinalReport)
                        {
                            int[] tblCellWidth6 = { 0, 100, 400, 200 };
                            machineScheduleTbl.SetWidths(tblCellWidth6);
                        }
                        else
                        {
                            int[] tblCellWidth6 = { 200, 100, 100, 200 };
                            machineScheduleTbl.SetWidths(tblCellWidth6);
                        }
                        machineScheduleTbl.WidthPercentage = 40;
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Type")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Machine No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Scroll No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Bowl No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Order No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Customer")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Country")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(machineScheduleTbl));
                    }

                    pdfTable = new PdfPTable(3);
                    int[] tblCellWidth = { 300, 30, 80 };
                    pdfTable.SetWidths(tblCellWidth);
                    pdfTable.WidthPercentage = 100;
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Check before packing \nVor dem Einpacken prüfen", 7)));
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Done", 7)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Date,Name", 7)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    for (int i = 0; i < checklistData.Rows.Count; i++)
                    {
                        if (string.Equals(checklistData.Rows[i]["Enabled"].ToString(), "true", StringComparison.OrdinalIgnoreCase))
                        {
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getDecadeChecklistPackingHeaderName(checklistData.Rows[i]["ParameterID"].ToString()))));
                            if (string.Equals(checklistData.Rows[i]["Checked"].ToString(), "Ok", StringComparison.OrdinalIgnoreCase))
                            {
                                byte[] checkfile1;
                                checkfile1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/Check.png"));//ImagePath  
                                iTextSharp.text.Image checkjpg = iTextSharp.text.Image.GetInstance(checkfile1);
                                checkjpg.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                                checkjpg.ScaleToFit(10f, 10f);
                                PdfPCell cell = new PdfPCell(checkjpg, false);
                                cell.BorderWidth = 1;
                                cell.Padding = 2;
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                pdfTable.AddCell(cell).BorderColor = new BaseColor(122, 121, 121);
                            }
                            else
                            {
                                byte[] uncheckfile;
                                uncheckfile = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/Uncheck.png"));//ImagePath  
                                iTextSharp.text.Image uncheckjpg = iTextSharp.text.Image.GetInstance(uncheckfile);
                                uncheckjpg.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                                uncheckjpg.ScaleToFit(10f, 10f);
                                PdfPCell cell = new PdfPCell(uncheckjpg, false);
                                cell.BorderWidth = 1;
                                cell.Padding = 2;
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                pdfTable.AddCell(cell).BorderColor = new BaseColor(122, 121, 121);
                            }
                            string updatedBy = checklistData.Rows[i]["UpdatedBy"].ToString();
                            if (!string.IsNullOrEmpty(updatedBy))
                            {
                                updatedBy = (string.IsNullOrEmpty(checklistData.Rows[i]["UpdatedTS"].ToString()) ? "" : (Convert.ToDateTime(checklistData.Rows[i]["UpdatedTS"].ToString())).ToString("dd-MMM-yy")) + ", " + updatedBy;
                            }
                            else
                            {
                                updatedBy = "";
                            }
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(updatedBy)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        }
                        else
                        {
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader(getDecadeChecklistPackingHeaderName(checklistData.Rows[i]["ParameterID"].ToString()), 7)) { Colspan = 3, MinimumHeight = 20, VerticalAlignment = Element.ALIGN_BOTTOM });
                        }
                    }
                    mainTbl.AddCell(new PdfPCell(pdfTable));
                }
                else
                {
                    ReportStatus = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
            }
            return mainTbl;
        }
        private static string getDecadeChecklistPackingHeaderName(string value)
        {
            string headerName = "";
            try
            {
                headerName = value.Replace("?", "\n");

            }
            catch (Exception ex)
            {

            }
            return headerName;
        }
        #endregion

        #region -------- Decanter final checklist packing -------
        internal static string decanterFinalChecklistPackingReport(string ProductionOrder, string FabricationNo)
        {
            string ReportStatus = string.Empty;
            try
            {
                PdfPTable mainTbl = decanterFinalChecklistPackingReportTbl(ProductionOrder, FabricationNo, 20, false, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    Document pdfDoc = new Document(PageSize.A4, 15, 15, 25, 25);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();

                    pdfDoc.Add(mainTbl);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=DecanterFinalChecklistPacking.pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }

        internal static PdfPTable decanterFinalChecklistPackingReportTbl(string ProductionOrder, string FabricationNo, int mainTblSpaceBefore, bool isFromFinalReport, out string ReportStatus)
        {
            ReportStatus = string.Empty;
            PdfPTable mainTbl = new PdfPTable(1);
            try
            {
                DataTable checklistData, remarksTbl;
                string machineid = GEADatabaseAccess.getMachineIDForTestingPackingReport("Packing");
                string param = "NonPro";
                if (GEADatabaseAccess.isProductionOrderProMaterial(ProductionOrder, FabricationNo, "decanterFinalChecklistPacking"))
                {
                    param = "Pro";
                }
                DataTable machineDataTbl = GEADatabaseAccess.GetDecanterFinalChecklistPackingReportData(ProductionOrder, FabricationNo, machineid, out checklistData, param, out remarksTbl);
                if (machineDataTbl.Rows.Count > 0 || checklistData.Rows.Count > 0 || remarksTbl.Rows.Count > 0)
                {
                    string machineType = "", checkedBy = "", checkedDate = "", formateNo = "", revNo = "", compid = "", technicianName = "";
                    if (checklistData.Rows.Count > 0)
                    {
                        machineType = checklistData.Rows[0]["MachineType"] == null ? "" : checklistData.Rows[0]["MachineType"].ToString();
                        checkedBy = checklistData.Rows[0]["UpdatedBy"] == null ? "" : checklistData.Rows[0]["UpdatedBy"].ToString();
                        if (checklistData.Rows[0]["UpdatedTS"] != null && checklistData.Rows[0]["UpdatedTS"].ToString() != "")
                        {
                            checkedDate = Convert.ToDateTime(checklistData.Rows[0]["UpdatedTS"]).ToString("dd-MMM-yy");
                        }
                        formateNo = checklistData.Rows[0]["FormatNo"] == null ? "" : checklistData.Rows[0]["FormatNo"].ToString();
                        revNo = checklistData.Rows[0]["RevNo"] == null ? "" : checklistData.Rows[0]["RevNo"].ToString();
                        technicianName = checklistData.Rows[0]["TechnicianName"] == null ? "" : checklistData.Rows[0]["TechnicianName"].ToString();
                    }

                    mainTbl.SplitLate = false;
                    mainTbl.WidthPercentage = 100;
                    mainTbl.SpacingBefore = mainTblSpaceBefore;
                    mainTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    mainTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    //header
                    PdfPTable pdfTable = new PdfPTable(7);
                    pdfTable.WidthPercentage = 100;
                    byte[] file1;
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(getGEALogoPath()));//ImagePath  
                    iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(70f, 60f);
                    PdfPCell logoCell = new PdfPCell(geaLogo, false);
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfTable.AddCell(new PdfPCell(logoCell) { Colspan = 1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Decanter Final Checklist - Packing", 10)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    PdfPTable innerTbl = new PdfPTable(1);
                    innerTbl.WidthPercentage = 100;
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Format No: " + formateNo)) { Border = 0 });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Rev No: " + revNo)) { Border = 0 });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Date: " + "28-Feb-20")) { Border = 0 });
                    pdfTable.AddCell(new PdfPCell(innerTbl) { Colspan = 2 });
                    mainTbl.AddCell(new PdfPCell(pdfTable));

                    if (machineDataTbl.Rows.Count > 0)
                    {
                        compid = machineDataTbl.Rows[0]["ScrollWelded"].ToString();
                    }

                    pdfTable = new PdfPTable(4);
                    int[] tblCellWidth = { 30, 300, 30, 30 };
                    pdfTable.SetWidths(tblCellWidth);
                    pdfTable.WidthPercentage = 100;
                    pdfTable.AddCell(new PdfPCell(getPdfCellGridHeader("S. No")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 8, PaddingBottom = 8 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellGridHeader("Item List")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellGridHeader("Yes")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellGridHeader("No")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    for (int i = 0; i < checklistData.Rows.Count; i++)
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText((i + 1).ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getDecadeChecklistPackingHeaderName(checklistData.Rows[i]["ParameterID"].ToString()))) { PaddingBottom = 8, PaddingTop = 8 });
                        byte[] checkfile1;
                        checkfile1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/GEA/Icons/Right.png"));//ImagePath  
                        iTextSharp.text.Image checkjpg = iTextSharp.text.Image.GetInstance(checkfile1);
                        checkjpg.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        checkjpg.ScaleToFit(10f, 10f);
                        PdfPCell cell = new PdfPCell(checkjpg, false);
                        //cell.BorderWidth = 1;
                        cell.Border = 1;
                        cell.Padding = 2;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //pdfTable.AddCell(cell).BorderColor = new BaseColor(122, 121, 121);
                        //pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")));
                        if (string.Equals(checklistData.Rows[i]["Checked"].ToString(), "", StringComparison.OrdinalIgnoreCase))
                        {
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")));
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")));
                        }
                        else if (string.Equals(checklistData.Rows[i]["Checked"].ToString(), "Ok", StringComparison.OrdinalIgnoreCase))
                        {
                            pdfTable.AddCell(new PdfPCell(cell) { Border = 0 });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")));
                        }
                        else
                        {
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")));
                            pdfTable.AddCell(new PdfPCell(cell));

                        }
                    }

                    //pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Missing Parts, if any:")) { MinimumHeight = 25, VerticalAlignment = Element.ALIGN_BOTTOM, Colspan = 4 });
                    //for (int i = 0; i < remarksTbl.Rows.Count; i++)
                    //{
                    //    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText((i + 1).ToString())));
                    //    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getDecadeChecklistPackingHeaderName(remarksTbl.Rows[i]["Checked"].ToString()))) { Colspan = 3 });
                    //}

                    PdfPTable pdfInnerTable = new PdfPTable(3);
                    pdfInnerTable.WidthPercentage = 100;
                    PdfPTable pdfInnerInnerTable = new PdfPTable(2);
                    int[] tblCellWidth5 = { 30, 300 };
                    pdfInnerInnerTable.SetWidths(tblCellWidth5);
                    pdfInnerInnerTable.WidthPercentage = 100;
                    pdfInnerInnerTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Missing Parts, if any:")) { MinimumHeight = 25, VerticalAlignment = Element.ALIGN_BOTTOM, Colspan = 4 });
                    for (int i = 0; i < remarksTbl.Rows.Count; i++)
                    {
                        pdfInnerInnerTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText((i + 1).ToString())) { PaddingBottom = 12, PaddingTop = 12, HorizontalAlignment = Element.ALIGN_CENTER });
                        pdfInnerInnerTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getDecadeChecklistPackingHeaderName(remarksTbl.Rows[i]["Checked"].ToString()))) { });
                    }
                    pdfInnerTable.AddCell(new PdfPCell(pdfInnerInnerTable) { Colspan = 2 });

                    PdfPTable machineScheduleTbl = new PdfPTable(4);
                    machineScheduleTbl.TotalWidth = 600;
                    int[] tblCellWidth6 = { 5, 100, 100, 5 };
                    machineScheduleTbl.SetWidths(tblCellWidth6);
                    if (machineDataTbl.Rows.Count > 0)
                    {
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Type")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineType)) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Machine No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Scroll No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Bowl No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Order No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["SaleOrder"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Customer")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["Customer"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Country")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineDataTbl.Rows[0]["Location"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                    }
                    else
                    {
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Type")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Machine No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Scroll No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Bowl No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Order No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Customer")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Country")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                    }
                    pdfInnerTable.AddCell(new PdfPCell(machineScheduleTbl) { VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(pdfInnerTable) { Colspan = 4 });
                    mainTbl.AddCell(new PdfPCell(pdfTable));
                    pdfTable = new PdfPTable(2);
                    pdfTable.WidthPercentage = 100;
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Technician Name: " + technicianName)) { PaddingBottom = 8, PaddingTop = 8 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Signature with Date: " + checkedBy + ", " + checkedDate)) { VerticalAlignment = Element.ALIGN_MIDDLE });
                    mainTbl.AddCell(new PdfPCell(pdfTable));
                }
                else
                {
                    ReportStatus = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
            }
            return mainTbl;
        }
        #endregion

        #region ------ Blue card report -----
        internal static string decanterAcceptanceTestCardReport(string ProductionOrder, string FabricationNo)
        {
            string ReportStatus = string.Empty;
            try
            {
                PdfPTable mainTbl = decanterAcceptanceTestCardReportTbl(ProductionOrder, FabricationNo, 20, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    Document pdfDoc = new Document(PageSize.A4, 15, 15, 25, 25);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();
                    pdfDoc.Add(mainTbl);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=DecanterAcceptanceTestCard.pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }

        internal static PdfPTable decanterAcceptanceTestCardReportTbl(string ProductionOrder, string FabricationNo, int mainTblSpaceBefore, out string ReportStatus)
        {
            ReportStatus = string.Empty;
            PdfPTable mainTbl = new PdfPTable(1);
            try
            {
                DataTable speedChecklistTbl, materialIdentificationTbl, supervisorAssemblyTbl, logisticTbl, qualityTbl, remarkTbl;
                // string machineid = GEADatabaseAccess.getMachineIDForTestingPackingReport("Packing");
                string procName = "[dbo].[S_GetBlueCardReport_GEA]";
                if (GEADatabaseAccess.isProductionOrderProMaterial(ProductionOrder, FabricationNo, "BlueTestCard"))
                {
                    procName = "[dbo].[S_GetBlueCardProReport_GEA]";
                }
                DataTable machineDataTbl = GEADatabaseAccess.GetDecanterAcceptanceTestCardReportData(procName, ProductionOrder, FabricationNo, out speedChecklistTbl, out materialIdentificationTbl, out supervisorAssemblyTbl, out logisticTbl, out qualityTbl, out remarkTbl);
                if (machineDataTbl.Rows.Count > 0 || speedChecklistTbl.Rows.Count > 0 || materialIdentificationTbl.Rows.Count > 0 || supervisorAssemblyTbl.Rows.Count > 0 || logisticTbl.Rows.Count > 0 || qualityTbl.Rows.Count > 0 || remarkTbl.Rows.Count > 0)
                {
                    string machineType = "", checkedBy = "", checkedDate = "", formateNo = "", revNo = "", appovedBy = "";
                    if (speedChecklistTbl.Rows.Count > 0)
                    {
                        machineType = speedChecklistTbl.Rows[0]["MachineType"] == null ? "" : speedChecklistTbl.Rows[0]["MachineType"].ToString();
                        checkedBy = speedChecklistTbl.Rows[0]["UpdatedBy"] == null ? "" : speedChecklistTbl.Rows[0]["UpdatedBy"].ToString();
                        if (speedChecklistTbl.Rows[0]["UpdatedTS"] != null && speedChecklistTbl.Rows[0]["UpdatedTS"].ToString() != "")
                        {
                            checkedDate = Convert.ToDateTime(speedChecklistTbl.Rows[0]["UpdatedTS"]).ToString("dd-MMM-yy");
                        }
                        formateNo = speedChecklistTbl.Rows[0]["FormatNo"] == null ? "" : speedChecklistTbl.Rows[0]["FormatNo"].ToString();
                        revNo = speedChecklistTbl.Rows[0]["RevNo"] == null ? "" : speedChecklistTbl.Rows[0]["RevNo"].ToString();
                        appovedBy = speedChecklistTbl.Rows[0]["ApprovedBy"] == null ? "" : speedChecklistTbl.Rows[0]["ApprovedBy"].ToString();
                    }

                    mainTbl.SplitLate = false;
                    mainTbl.WidthPercentage = 100;
                    mainTbl.SpacingBefore = mainTblSpaceBefore;
                    mainTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    mainTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    //header
                    PdfPTable pdfTable = new PdfPTable(7);
                    pdfTable.WidthPercentage = 100;
                    byte[] file1;
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(getGEALogoPath()));//ImagePath  
                    iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(70f, 60f);
                    PdfPCell logoCell = new PdfPCell(geaLogo, false);
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfTable.AddCell(new PdfPCell(logoCell) { Colspan = 1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("DECANTER ACCEPTANCE \n Test Card", 10)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_CENTER });
                    PdfPTable innerTbl = new PdfPTable(1);
                    innerTbl.WidthPercentage = 100;
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Format No: " + formateNo)) { Border = 0 });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Rev No: " + revNo)) { Border = 0 });
                    // innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Date: " + DateTime.Now.ToString("dd-MMM-yyyy"))) { Border = 0 });
                    pdfTable.AddCell(new PdfPCell(innerTbl) { Colspan = 2 });
                    mainTbl.AddCell(new PdfPCell(pdfTable) { BackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(180, 244, 253)) });

                    int paddingValue = 6;
                    pdfTable = new PdfPTable(2);
                    pdfTable.WidthPercentage = 100;
                    if (machineDataTbl.Rows.Count > 0)
                    {
                        PdfPTable machineScheduleTbl = new PdfPTable(2);
                        // machineScheduleTbl.TotalWidth = 600;
                        int[] tblCellWidth0 = { 100, 200 };
                        machineScheduleTbl.SetWidths(tblCellWidth0);
                        machineScheduleTbl.WidthPercentage = 40;
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Type:")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue }); ;
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(machineType)) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Machine No")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Scroll No")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Bowl No")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(machineDataTbl.Rows[0]["ScrollWelded"].ToString())) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Order No")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(machineDataTbl.Rows[0]["SaleOrder"].ToString())) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Customer")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(machineDataTbl.Rows[0]["Customer"].ToString())) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Country")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(machineDataTbl.Rows[0]["Location"].ToString())) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        pdfTable.AddCell(new PdfPCell(machineScheduleTbl) { Border = 0 });
                    }
                    else
                    {
                        PdfPTable machineScheduleTbl = new PdfPTable(2);
                        int[] tblCellWidth0 = { 100, 200 };
                        machineScheduleTbl.SetWidths(tblCellWidth0);
                        machineScheduleTbl.WidthPercentage = 40;
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Type:")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Machine No")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Scroll No")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Bowl No")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Order No")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Customer")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Country")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        pdfTable.AddCell(new PdfPCell(machineScheduleTbl) { Border = 0 });
                    }
                    innerTbl = new PdfPTable(3);
                    int[] tblCellWidth6 = { 100, 80, 50 };
                    innerTbl.SetWidths(tblCellWidth6);
                    innerTbl.WidthPercentage = 100;
                    if (speedChecklistTbl.Rows.Count > 0)
                    {
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("       Max Permissible Bowl speed:")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getBlueCardReportValue(speedChecklistTbl, "Max Permissible Bowl speed"))) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Min⁻¹")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("       Permissible Operation Speed:")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getBlueCardReportValue(speedChecklistTbl, "Permissible Operation speed"))) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Min⁻¹")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("       Density of solids:")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getBlueCardReportValue(speedChecklistTbl, "Density id solids"))) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Kg/dm³")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Colspan = 3, Rowspan = 3, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    }
                    else
                    {
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("       Max Permissible Bowl speed:")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("       ")) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Min⁻¹")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("       Permissible Operation Speed:")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("       ")) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Min⁻¹")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("       Density of solids:")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("       ")) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Kg/dm³")) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Colspan = 3, Rowspan = 3, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    }
                    pdfTable.AddCell(new PdfPCell(innerTbl) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 2, MinimumHeight = 10, BorderWidthBottom = 0, BorderWidthTop = 0 });
                    //Check uncheck image
                    byte[] checkfile1;
                    checkfile1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/Check.png"));//ImagePath  
                    iTextSharp.text.Image checkImage = iTextSharp.text.Image.GetInstance(checkfile1);
                    checkImage.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    checkImage.ScaleToFit(10f, 10f);
                    PdfPCell checkCell = new PdfPCell(checkImage, false);
                    checkCell.BorderWidth = 0;
                    checkCell.Padding = 2;
                    checkCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    checkfile1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/Uncheck.png"));//ImagePath  
                    iTextSharp.text.Image uncheckImage = iTextSharp.text.Image.GetInstance(checkfile1);
                    uncheckImage.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    uncheckImage.ScaleToFit(10f, 10f);
                    PdfPCell unCheckCell = new PdfPCell(uncheckImage, false);
                    unCheckCell.BorderWidth = 0;
                    unCheckCell.Padding = 2;
                    unCheckCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    checkfile1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/GEA/Icons/Right.png"));//ImagePath  
                    iTextSharp.text.Image rightImage = iTextSharp.text.Image.GetInstance(checkfile1);
                    rightImage.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    rightImage.ScaleToFit(10f, 10f);
                    PdfPCell rightImageCell = new PdfPCell(rightImage, false);
                    rightImageCell.BorderWidth = 0;
                    rightImageCell.Padding = 2;
                    rightImageCell.HorizontalAlignment = Element.ALIGN_CENTER;


                    innerTbl = new PdfPTable(5);
                    int[] tblCellWidth = { 150, 10, 50, 10, 100 };
                    innerTbl.SetWidths(tblCellWidth);
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Inspection sheet for Decanter scroll is found attached:")) { Border = 0 });
                    if (speedChecklistTbl.Rows.Count > 0)
                    {
                        if (string.Equals(getBlueCardReportValue(speedChecklistTbl, "Inspection sheet for Decanter scroll is found attached"), "", StringComparison.OrdinalIgnoreCase))
                        {
                            innerTbl.AddCell(new PdfPCell(unCheckCell) { Border = 0 });
                            innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Yes")) { Border = 0 });
                            innerTbl.AddCell(new PdfPCell(unCheckCell) { Border = 0 });
                            innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("No")) { Border = 0 });
                        }
                        else if (string.Equals(getBlueCardReportValue(speedChecklistTbl, "Inspection sheet for Decanter scroll is found attached"), "Ok", StringComparison.OrdinalIgnoreCase))
                        {
                            innerTbl.AddCell(new PdfPCell(checkCell) { Border = 0 });
                            innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Yes")) { Border = 0 });
                            innerTbl.AddCell(new PdfPCell(unCheckCell) { Border = 0 });
                            innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("No")) { Border = 0 });
                        }
                        else
                        {
                            innerTbl.AddCell(new PdfPCell(unCheckCell) { Border = 0 });
                            innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Yes")) { Border = 0 });
                            innerTbl.AddCell(new PdfPCell(checkCell) { Border = 0 });
                            innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("No")) { Border = 0 });
                        }
                    }
                    else
                    {
                        innerTbl.AddCell(new PdfPCell(unCheckCell) { Border = 0 });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Yes")) { Border = 0 });
                        innerTbl.AddCell(new PdfPCell(unCheckCell) { Border = 0 });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("No")) { Border = 0 });
                    }
                    pdfTable.AddCell(new PdfPCell(innerTbl) { Border = 0, Colspan = 2 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Colspan = 2, MinimumHeight = 10 });
                    mainTbl.AddCell(new PdfPCell(pdfTable) { BorderWidthBottom = 0, BackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(180, 244, 253)) });
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, MinimumHeight = 10, BackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(180, 244, 253)) });

                    pdfTable = new PdfPTable(8);
                    int[] tblCellWidth11 = { 20, 50, 50, 50, 50, 50, 50, 50 };
                    pdfTable.SetWidths(tblCellWidth11);
                    pdfTable.WidthPercentage = 100;
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Material Identification Data")) { Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Part No")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Check No (Series+Serial No)")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("OverSpeed/UDP")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Dye-Pen Test")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    innerTbl = new PdfPTable(2);
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Stamped:")) { HorizontalAlignment = Element.ALIGN_CENTER, Colspan = 2 });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithSupSubScript("n", "max", true)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithSupSubScript("O", "perm", true)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(innerTbl) { Colspan = 2 });
                    for (int i = 0; i < materialIdentificationTbl.Rows.Count; i++)
                    {
                        int paddingvalue = 6;
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(materialIdentificationTbl.Rows[i]["ParameterID"].ToString())) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(materialIdentificationTbl.Rows[i]["PartNo"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(materialIdentificationTbl.Rows[i]["CheckNo(Series+SerialNo)"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(materialIdentificationTbl.Rows[i]["OverSpeed/UDP"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(materialIdentificationTbl.Rows[i]["Dye-Pen Test"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(materialIdentificationTbl.Rows[i]["MaxStamped"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(materialIdentificationTbl.Rows[i]["PermStamped"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    }

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 3 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Yes")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("No")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Date & Name")) { Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER });

                    //Supervisor data

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Supervisor Assembly/FAT")) { Colspan = 1, Rowspan = supervisorAssemblyTbl.Rows.Count, HorizontalAlignment = Element.ALIGN_CENTER, Rotation = 90, VerticalAlignment = Element.ALIGN_CENTER });
                    paddingValue = 6;
                    for (int i = 0; i < supervisorAssemblyTbl.Rows.Count; i++)
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(supervisorAssemblyTbl.Rows[i]["ParameterID"].ToString())) { Border = 0, Colspan = 2, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        if (string.Equals(supervisorAssemblyTbl.Rows[i]["Checked"].ToString(), "", StringComparison.OrdinalIgnoreCase))
                        {
                            pdfTable.AddCell(new PdfPCell(unCheckCell) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                            pdfTable.AddCell(new PdfPCell(unCheckCell) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        }
                        else if (string.Equals(supervisorAssemblyTbl.Rows[i]["Checked"].ToString(), "Ok", StringComparison.OrdinalIgnoreCase))
                        {
                            pdfTable.AddCell(new PdfPCell(checkCell) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                            pdfTable.AddCell(new PdfPCell(unCheckCell) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        }
                        else
                        {
                            pdfTable.AddCell(new PdfPCell(unCheckCell) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                            pdfTable.AddCell(new PdfPCell(checkCell) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        }
                        string supDate = "";
                        if (!string.IsNullOrEmpty(supervisorAssemblyTbl.Rows[i]["UpdatedTS"].ToString()))
                        {
                            supDate = Convert.ToDateTime(supervisorAssemblyTbl.Rows[i]["UpdatedTS"].ToString()).ToString("dd-MMM-yy");
                        }
                        string supBy = supervisorAssemblyTbl.Rows[i]["UpdatedBy"].ToString();
                        if (supBy == "")
                        {
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(supDate)) { Border = 0, BorderWidthBottom = 1, Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        }
                        else
                        {
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(supDate + " & " + supBy)) { Border = 0, BorderWidthBottom = 1, Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        }

                    }
                    mainTbl.AddCell(new PdfPCell(pdfTable) { BackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(180, 244, 253)) });

                    pdfTable = new PdfPTable(2);
                    int[] tblCellWidth3 = { 17, 300 };
                    pdfTable.SetWidths(tblCellWidth3);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Quality \n Management")) { Rowspan = qualityTbl.Rows.Count, HorizontalAlignment = Element.ALIGN_CENTER, Rotation = 90, VerticalAlignment = Element.ALIGN_CENTER });
                    innerTbl = new PdfPTable(2);
                    // innerTbl.SpacingAfter = 20;
                    // innerTbl.SpacingBefore = 20;
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Colspan = 2, MinimumHeight = 20 });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Decanter Checked and Released")) { Border = 0 });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("i.A.   " + qualityTbl.Rows[0]["UpdatedBy"].ToString() + "    " + qualityTbl.Rows[0]["UpdatedTS"].ToString())) { Border = 0, BorderWidthBottom = 1 });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Colspan = 2, MinimumHeight = 20 });
                    pdfTable.AddCell(new PdfPCell(innerTbl) { Border = 0 });
                    mainTbl.AddCell(new PdfPCell(pdfTable) { BackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(180, 244, 253)) });

                    pdfTable = new PdfPTable(2);
                    int[] tblCellWidth4 = { 17, 300 };
                    pdfTable.SetWidths(tblCellWidth4);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Logistics")) { Rowspan = logisticTbl.Rows.Count, HorizontalAlignment = Element.ALIGN_CENTER, Rotation = 90, VerticalAlignment = Element.ALIGN_CENTER });
                    innerTbl = new PdfPTable(2);
                    //innerTbl.SpacingAfter = 20;
                    // innerTbl.SpacingBefore = 20;
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Colspan = 2, MinimumHeight = 10 });
                    paddingValue = 6;
                    for (int i = 0; i < logisticTbl.Rows.Count; i++)
                    {
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(logisticTbl.Rows[i]["ParameterID"].ToString())) { Border = 0, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(logisticTbl.Rows[i]["UpdatedBy"].ToString() + "    " + logisticTbl.Rows[i]["UpdatedTS"].ToString())) { Border = 0, BorderWidthBottom = 1, PaddingTop = paddingValue, PaddingBottom = paddingValue });
                    }
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, Colspan = 2, MinimumHeight = 10 });
                    pdfTable.AddCell(new PdfPCell(innerTbl) { Border = 0 });
                    mainTbl.AddCell(new PdfPCell(pdfTable) { BackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(180, 244, 253)) });
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { MinimumHeight = 10, BackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(180, 244, 253)) });

                    pdfTable = new PdfPTable(2);
                    int[] tblCellWidth5 = { 200, 100 };
                    pdfTable.SetWidths(tblCellWidth5);
                    appovedBy = remarkTbl.Rows[0]["ApprovedBy"].ToString();
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Discrepancies if any:")));
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Approved By")));
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(getBlueCardReportValue(remarkTbl, "Discrepancies if any"))) { MinimumHeight = 60 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(appovedBy)) { MinimumHeight = 60 });

                    mainTbl.AddCell(new PdfPCell(pdfTable) { BackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(180, 244, 253)) });
                }
                else
                {
                    ReportStatus = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
            }
            return mainTbl;
        }
        private static string getBlueCardReportValue(DataTable dt, string parameterid)
        {
            string result = "";
            try
            {
                var remarksVal = dt.AsEnumerable().Where(k => k.Field<string>("ParameterID") == parameterid).FirstOrDefault();
                if (remarksVal != null)
                {
                    result = remarksVal["Checked"].ToString();
                }
            }
            catch (Exception ex)
            { }
            return result;
        }
        #endregion

        #region  ------- Final Report ---------
        private static void BindExistingPDFToFinalReport(PdfWriter pdfWriter, Document pdfDoc, string imgPdfPath, string filename)
        {
            try
            {
                string[] pdfFileArray = null;
                List<PdfReader> readerList = new List<PdfReader>();
                PdfContentByte cb = pdfWriter.DirectContent;
                pdfFileArray = Directory.GetFiles(imgPdfPath);
                foreach (string pdffile in pdfFileArray)
                {
                    string fileInLC = pdffile.ToLower();
                    filename = filename.ToLower();
                    if (fileInLC.Contains(filename + ".pdf"))
                    {
                        PdfReader pdfReader = new PdfReader(pdffile);
                        readerList.Add(pdfReader);
                    }
                }
                foreach (PdfReader reader in readerList)
                {
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = pdfWriter.GetImportedPage(reader, i);

                        iTextSharp.text.Rectangle currentPageRectangle = reader.GetPageSizeWithRotation(i);
                        if (currentPageRectangle.Width > currentPageRectangle.Height)
                        {
                            //page is landscape
                            if (page.Height < page.Width)
                            {
                                cb.PdfDocument.NewPage();
                                PdfImportedPage page1 = pdfWriter.GetImportedPage(reader, i);
                                iTextSharp.text.Rectangle psize = reader.GetPageSizeWithRotation(i);
                                int index = reader.GetPageRotation(i);
                                switch (index)
                                {
                                    case 0:
                                        cb.AddTemplate(page1, 0, 1.0F, -1.0F, 0, psize.Height, 0);
                                        break;
                                    case 90:
                                        cb.AddTemplate(page1, 0, 1.0F, -1.0F, 0, psize.Width, 0);
                                        break;
                                    case 180:
                                        cb.AddTemplate(page1, -1f, 0, 0, -1f, 0, 0);
                                        break;
                                    case 270:
                                        cb.AddTemplate(page1, 0, 1.0F, -1.0F, 0, psize.Width, 0);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(0, 0, PageSize.A4.Width, PageSize.A4.Height, 0));
                                pdfDoc.Add(iTextSharp.text.Image.GetInstance(page));
                            }
                            pdfDoc.NewPage();
                        }
                        else
                        {
                            //page is portrait

                            cb.PdfDocument.NewPage();
                            PdfImportedPage page1 = pdfWriter.GetImportedPage(reader, i);


                            iTextSharp.text.Rectangle psize = reader.GetPageSizeWithRotation(i);
                            switch (psize.Rotation)
                            {
                                case 0:
                                    cb.AddTemplate(page1, 1f, 0, 0, 1f, 0, 0);
                                    break;
                                case 90:
                                    cb.AddTemplate(page1, 0, -1f, 1f, 0, 0, psize.Height);
                                    break;
                                case 180:
                                    cb.AddTemplate(page1, -1f, 0, 0, -1f, 0, 0);
                                    break;
                                case 270:
                                    cb.AddTemplate(page1, 0, 1.0F, -1.0F, 0, psize.Width, 0);
                                    break;
                                default:
                                    break;
                            }
                            pdfDoc.NewPage();
                        }

                        // if (page.Width > page.Height)
                        //  {
                        // pdfDoc.SetPageSize(PageSize.A4.Rotate());

                        // }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindExistingPDFToFinalReport = " + ex.Message);
            }
        }
        internal static string assemblyTestingPackingReport(string ProductionOrder, string FabricationNo, string componentid)
        {
            string ReportStatus = string.Empty;
            try
            {
                //Document pdfDoc = new Document(PageSize.A4, 15, 15, 25, 25);
                Document pdfDoc = new Document(PageSize.A4, 0, 0, 0, 0);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                pdfDoc.Open();
                Paragraph p2 = new Paragraph();

                string imgPdfPath = ConfigurationManager.AppSettings["GEAImagePath"].ToString() + "\\" + ProductionOrder + "_" + FabricationNo;
                try
                {

                    BindExistingPDFToFinalReport(pdfWriter, pdfDoc, imgPdfPath, "1");

                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.Message);
                }
                //pdfDoc.Add(p2);
                //pdfDoc.NewPage();
                PdfPTable finalTbl = new PdfPTable(1);
                finalTbl.SplitLate = false;
                finalTbl.WidthPercentage = 100;
                finalTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                finalTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPTable mainTbl = decanterAcceptanceTestCardReportTbl(ProductionOrder, FabricationNo, 0, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    finalTbl.AddCell(new PdfPCell(mainTbl));
                    pdfDoc.Add(getFinalReportParagraph(finalTbl));
                }



                string machineID = GEADatabaseAccess.getPOMachineIDForAssemblyProcess(ProductionOrder, FabricationNo, "machineAssembly");
                mainTbl = machineDataAssmeblyReportTbl(ProductionOrder, FabricationNo, machineID, 0, true, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    p2 = new Paragraph();
                    pdfDoc.Add(p2);
                    pdfDoc.NewPage();
                    finalTbl = new PdfPTable(1);
                    finalTbl.SplitLate = false;
                    finalTbl.WidthPercentage = 100;
                    finalTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    finalTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    finalTbl.AddCell(new PdfPCell(mainTbl));
                    pdfDoc.Add(getFinalReportParagraph(finalTbl));
                }


                machineID = GEADatabaseAccess.getPOMachineIDForAssemblyProcess(ProductionOrder, FabricationNo, "electo");
                mainTbl = electroTechEquipmentReport(ProductionOrder, FabricationNo, machineID, 0, true, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    p2 = new Paragraph();
                    pdfDoc.Add(p2);
                    pdfDoc.NewPage();
                    finalTbl = new PdfPTable(1);
                    finalTbl.SplitLate = false;
                    finalTbl.WidthPercentage = 100;
                    finalTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    finalTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    finalTbl.AddCell(new PdfPCell(mainTbl));
                    pdfDoc.Add(getFinalReportParagraph(finalTbl));
                }


                mainTbl = testingReportTbl(ProductionOrder, FabricationNo, componentid, 0, true, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    //ReportStatus = "Failed";
                    //return ReportStatus;
                    p2 = new Paragraph();
                    pdfDoc.Add(p2);
                    pdfDoc.NewPage();
                    finalTbl = new PdfPTable(1);
                    finalTbl.SplitLate = false;
                    finalTbl.WidthPercentage = 100;
                    finalTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    finalTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    finalTbl.AddCell(new PdfPCell(mainTbl));
                    pdfDoc.Add(getFinalReportParagraph(finalTbl));
                }


                mainTbl = vibrationTestProtocolReport(ProductionOrder, FabricationNo, componentid, 0, true, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    p2 = new Paragraph();
                    pdfDoc.Add(p2);
                    pdfDoc.NewPage();
                    finalTbl = new PdfPTable(1);
                    finalTbl.SplitLate = false;
                    finalTbl.WidthPercentage = 100;
                    finalTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    finalTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    finalTbl.AddCell(new PdfPCell(mainTbl));
                    pdfDoc.Add(getFinalReportParagraph(finalTbl));
                }


                mainTbl = NoiseMeasurementReport(ProductionOrder, FabricationNo, componentid, 0, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    p2 = new Paragraph();
                    pdfDoc.Add(p2);
                    pdfDoc.NewPage();
                    finalTbl = new PdfPTable(1);
                    finalTbl.SplitLate = false;
                    finalTbl.WidthPercentage = 100;
                    finalTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    finalTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    finalTbl.AddCell(new PdfPCell(mainTbl));
                    pdfDoc.Add(getFinalReportParagraph(finalTbl));
                }


                //p2 = new Paragraph();
                //pdfDoc.Add(p2);
                //pdfDoc.NewPage();
                //CEChecklistReportTbl(pdfDoc, ProductionOrder, FabricationNo, 0,true, out ReportStatus);
                //p2 = new Paragraph();
                //pdfDoc.Add(p2);
                //pdfDoc.NewPage();

                BalancingProductionOrder = GEADatabaseAccess.getBalancingProductionOrderForFinalReport(ProductionOrder);
                if (BalancingProductionOrder != "")
                {

                    mainTbl = balancingCertificateReportTbl(BalancingProductionOrder, 0, out ReportStatus);
                    if (ReportStatus == string.Empty)
                    {
                        // ReportStatus = "Failed";
                        //return ReportStatus;
                        p2 = new Paragraph();
                        pdfDoc.Add(p2);
                        pdfDoc.NewPage();
                        finalTbl = new PdfPTable(1);
                        finalTbl.SplitLate = false;
                        finalTbl.WidthPercentage = 100;
                        finalTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        finalTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        finalTbl.AddCell(new PdfPCell(mainTbl));
                        pdfDoc.Add(getFinalReportParagraph(finalTbl));
                    }
                }


                //DataTable dt = GEADatabaseAccess.getTestProtocolPODetailsForFinalReport(ProductionOrder);
                if (BalancingProductionOrder != "")
                {
                    DataTable dt = GEADatabaseAccess.getTestProtocolPODetailsForFinalReport(BalancingProductionOrder);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            var firstRow = dt.Rows[0];
                            p2 = new Paragraph();
                            pdfDoc.Add(p2);
                            pdfDoc.NewPage();
                            mainTbl = QualityTestProtocolTbl(firstRow["MachineID"].ToString(), firstRow["ProductionOrderNo"].ToString(), firstRow["MaterialID"].ToString(), firstRow["OperationNo"].ToString(), firstRow["PlanAndRevNo"].ToString(), 0, pdfDoc, firstRow["GrnNo"].ToString(), out ReportStatus);
                            if (ReportStatus == string.Empty)
                            {
                                // ReportStatus = "Failed";
                                //return ReportStatus;
                                //p2 = new Paragraph();
                                //pdfDoc.Add(p2);
                                //pdfDoc.NewPage();
                                //finalTbl = new PdfPTable(1);
                                //finalTbl.SplitLate = false;
                                //finalTbl.WidthPercentage = 100;
                                //finalTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //finalTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                //finalTbl.AddCell(new PdfPCell(mainTbl));
                                //pdfDoc.Add(getFinalReportParagraph(finalTbl));
                            }
                        }
                    }
                }


                mainTbl = decanterChecklistPackingReport(ProductionOrder, FabricationNo, 0, true, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    p2 = new Paragraph();
                    pdfDoc.Add(p2);
                    pdfDoc.NewPage();
                    finalTbl = new PdfPTable(1);
                    finalTbl.SplitLate = false;
                    finalTbl.WidthPercentage = 100;
                    finalTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    finalTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    finalTbl.AddCell(new PdfPCell(mainTbl));
                    pdfDoc.Add(getFinalReportParagraph(finalTbl));
                }


                mainTbl = decanterFinalChecklistPackingReportTbl(ProductionOrder, FabricationNo, 0, true, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    p2 = new Paragraph();
                    pdfDoc.Add(p2);
                    pdfDoc.NewPage();
                    finalTbl = new PdfPTable(1);
                    finalTbl.SplitLate = false;
                    finalTbl.WidthPercentage = 100;
                    finalTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    finalTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    finalTbl.AddCell(new PdfPCell(mainTbl));
                    pdfDoc.Add(getFinalReportParagraph(finalTbl));
                }





                try
                {

                    string[] FileArray = Directory.GetFiles(imgPdfPath);
                    List<int> imageNumberList = new List<int>();
                    foreach (string file in FileArray)
                    {
                        if (file.Length > 0)
                        {
                            string imageName = file.Split(new[] { "\\" }, StringSplitOptions.None).Last();
                            imageName = imageName.ToLower();
                            if (!imageName.Contains(".pdf"))
                            {
                                imageName = imageName.Split('.').First();
                                imageNumberList.Add(Convert.ToInt32(imageName));
                            }
                        }
                    }
                    imageNumberList.Sort();
                    for (int i = 0; i < imageNumberList.Count; i++)
                    {
                        foreach (string file in FileArray)
                        {
                            string fileInLC = file.ToLower();
                            if (fileInLC.Contains("\\" + imageNumberList[i].ToString() + ".") && (!fileInLC.Contains(imageNumberList[i].ToString() + ".pdf")))
                            {
                                if (File.Exists(file))
                                {
                                    p2 = new Paragraph();
                                    pdfDoc.Add(p2);
                                    pdfDoc.NewPage();
                                    finalTbl = new PdfPTable(1);
                                    finalTbl.SplitLate = false;
                                    finalTbl.WidthPercentage = 100;
                                    finalTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    finalTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    byte[] file1 = System.IO.File.ReadAllBytes(file);//ImagePath  
                                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(file1);
                                    image.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                                    PdfPCell imageCell = new PdfPCell(image, true);
                                    imageCell.VerticalAlignment = Element.ALIGN_TOP;
                                    imageCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    imageCell.Border = 0;
                                    finalTbl.AddCell(new PdfPCell(imageCell));
                                    pdfDoc.Add(getFinalReportParagraph(finalTbl));
                                }
                                break;
                            }
                        }
                    }


                }
                catch (Exception ex)
                { }

                try
                {
                    pdfDoc.Add(p2);
                    pdfDoc.NewPage();

                    BindExistingPDFToFinalReport(pdfWriter, pdfDoc, imgPdfPath, "2");

                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.Message);
                }

                //foreach (string file in FileArray)
                //{
                //    if (file.Contains(ProductionOrder + "_" + FabricationNo))
                //    {
                //        Path = file;
                //    }
                //}
                //if (File.Exists(Path))
                //{
                //    byte[] file1 = System.IO.File.ReadAllBytes(Path);//ImagePath  
                //    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(file1);
                //    image.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                //    PdfPCell imageCell = new PdfPCell(image, true);
                //    imageCell.VerticalAlignment = Element.ALIGN_TOP;
                //    imageCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //    imageCell.Border = 0;
                //    finalTbl.AddCell(new PdfPCell(imageCell));
                //}
                //else
                //{
                //    finalTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")));
                //}



                //pdfDoc.Add(finalTbl);
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + FabricationNo + ".pdf");
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Write(pdfDoc);
                //Response.End();
                HttpContext.Current.Response.Flush();
                ReportStatus = "Generated";
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
            }
            return ReportStatus;
        }
        internal static Paragraph getFinalReportParagraph(PdfPTable finalTbl)
        {
            Paragraph p2 = new Paragraph();
            try
            {

                p2.IndentationLeft = 15;
                p2.IndentationRight = 15;
                p2.Add(new Paragraph(25, "\u00a0"));
                p2.Add(finalTbl);
                p2.Add(new Paragraph(25, "\u00a0"));
            }
            catch (Exception ex)
            { }
            return p2;
        }
        #endregion
        #region ------ Balancing Certificate Report ------
        internal static string balancingCertificateReport(string ProductionOrder)
        {
            string ReportStatus = string.Empty;
            try
            {
                PdfPTable mainTbl = balancingCertificateReportTbl(ProductionOrder, 20, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    Document pdfDoc = new Document(PageSize.A4, 15, 15, 25, 25);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();
                    pdfDoc.Add(mainTbl);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=BalancingCertificate.pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }

        internal static PdfPTable balancingCertificateReportTbl(string ProductionOrder, int mainTblSpaceBefore, out string ReportStatus)
        {
            ReportStatus = string.Empty;
            PdfPTable mainTbl = new PdfPTable(1);
            try
            {

                DataTable balancingDataTbl = new DataTable();
                balancingDataTbl = GEADatabaseAccess.GetBalancingCertificateReportData(ProductionOrder);
                if (balancingDataTbl.Rows.Count > 0)
                {

                    mainTbl.SplitLate = false;
                    mainTbl.WidthPercentage = 100;
                    mainTbl.SpacingBefore = mainTblSpaceBefore;
                    mainTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    mainTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    int paddingvalue = 8;
                    //header
                    PdfPTable pdfTable = new PdfPTable(4);
                    pdfTable.WidthPercentage = 100;
                    byte[] file1;
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(getGEALogoPath()));//ImagePath  
                    iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(80f, 60f);
                    PdfPCell logoCell = new PdfPCell(geaLogo, false);
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfTable.AddCell(new PdfPCell(logoCell) { Colspan = 1, PaddingTop = 10, PaddingBottom = 10, });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Balancing Certificate", 10)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(237, 237, 237)) });

                    string model = balancingDataTbl.Rows[0]["Model"].ToString();
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Date & Time")) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    string datetimes = "";
                    if (!string.IsNullOrEmpty(balancingDataTbl.Rows[0]["SelectedDateAndTime"].ToString()))
                    {
                        datetimes = Util.GetDateTime(balancingDataTbl.Rows[0]["SelectedDateAndTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(datetimes)) { Colspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Scroll Number ")) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(model + " - " + ProductionOrder + " / " + balancingDataTbl.Rows[0]["ScrollNumber"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Model")) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(model)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Measuring Speed")) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(balancingDataTbl.Rows[0]["MeasuringSpeed"].ToString())) { Colspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Mode of Balancing")) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });

                    //Check uncheck image
                    byte[] checkfile1;
                    checkfile1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/Uncheck.png"));//ImagePath  
                    iTextSharp.text.Image uncheckImage = iTextSharp.text.Image.GetInstance(checkfile1);
                    uncheckImage.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    uncheckImage.ScaleToFit(10f, 10f);
                    PdfPCell unCheckCell = new PdfPCell(uncheckImage, false);
                    unCheckCell.BorderWidth = 0;
                    unCheckCell.Padding = 2;
                    unCheckCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    checkfile1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/Check.png"));//ImagePath  
                    iTextSharp.text.Image rightImage = iTextSharp.text.Image.GetInstance(checkfile1);
                    rightImage.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    rightImage.ScaleToFit(10f, 10f);
                    PdfPCell rightImageCell = new PdfPCell(rightImage, false);
                    rightImageCell.BorderWidth = 0;
                    rightImageCell.Padding = 2;
                    rightImageCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPTable innerTbl = new PdfPTable(4);
                    innerTbl.WidthPercentage = 100;
                    if (balancingDataTbl.Rows[0]["AddingChecked"].ToString().Equals("True", StringComparison.OrdinalIgnoreCase) || balancingDataTbl.Rows[0]["AddingChecked"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        innerTbl.AddCell(new PdfPCell(rightImageCell) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                    }
                    else
                    {
                        innerTbl.AddCell(new PdfPCell(unCheckCell) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                    }
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Adding")) { Border = 0, BorderWidthRight = 0, HorizontalAlignment = Element.ALIGN_LEFT });

                    if (balancingDataTbl.Rows[0]["RemovingChecked"].ToString().Equals("True", StringComparison.OrdinalIgnoreCase) || balancingDataTbl.Rows[0]["RemovingChecked"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        innerTbl.AddCell(new PdfPCell(rightImageCell) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                    }
                    else
                    {
                        innerTbl.AddCell(new PdfPCell(unCheckCell) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                    }
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Removing")) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    pdfTable.AddCell(new PdfPCell(innerTbl) { Colspan = 3, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 0, PaddingBottom = 0, PaddingTop = 0 });

                    mainTbl.AddCell(pdfTable);
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, MinimumHeight = 10 });
                    pdfTable = new PdfPTable(7);
                    pdfTable.WidthPercentage = 100;
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("")) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Tol (g)")) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("r (mm)")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Dia (mm)")) { Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("iso")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Unit (KG)")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    DataTable dt = GEADatabaseAccess.BalancingReportData(model);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string dimValue = "a";
                            if (i == 0)
                            {
                                dimValue = "a";
                            }
                            else if (i == 1)
                            {
                                dimValue = "b";
                            }
                            else if (i == 2)
                            {
                                dimValue = "c";
                            }
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(dt.Rows[i]["Param1"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(dt.Rows[i]["Tol_g"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(dt.Rows[i]["R_mm"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(dimValue)) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(dt.Rows[i]["Dim_mm"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(dt.Rows[i]["ISO"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(dt.Rows[i]["uNIT"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                        }
                    }
                    mainTbl.AddCell(pdfTable);


                    try
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/GEA/Images/BalancingCertificate.png")))
                        {
                            file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/GEA/Images/BalancingCertificate.png"));//ImagePath  
                            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(file1);
                            image.ScaleToFit(50, 50);
                            image.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                            //image.ScaleAbsolute(50f, 50f);
                            PdfPCell imageCell = new PdfPCell(image, true);
                            imageCell.VerticalAlignment = Element.ALIGN_TOP;
                            imageCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            imageCell.Border = 0;
                            imageCell.FixedHeight = 100;
                            pdfTable = new PdfPTable(3);
                            int[] tblCellWidth1 = { 100, 400, 100 };
                            pdfTable.SetWidths(tblCellWidth1);
                            pdfTable.SplitLate = false;
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0 });
                            pdfTable.AddCell(new PdfPCell(imageCell));
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0 });
                            mainTbl.AddCell(new PdfPCell(pdfTable) { PaddingBottom = 10, PaddingTop = 10 });
                        }
                        else
                        {
                            mainTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { });
                        }
                    }
                    catch (Exception ex)
                    {
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Colspan = 2 });
                    }



                    pdfTable = new PdfPTable(2);
                    pdfTable.WidthPercentage = 100;
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Residual Unbalance", 10)) { Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(237, 237, 237)), Padding = 5, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("P1(g)")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("P2(g)")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(balancingDataTbl.Rows[0]["P1_a"].ToString())) { VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(balancingDataTbl.Rows[0]["P2_a"].ToString())) { VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    //pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(balancingDataTbl.Rows[0]["P1_b"].ToString())) { VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    //pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(balancingDataTbl.Rows[0]["P1_b"].ToString())) { VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    //mainTbl.AddCell(pdfTable);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithSupSubScript(balancingDataTbl.Rows[0]["P1_b"].ToString(), "o", false)) { VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithSupSubScript(balancingDataTbl.Rows[0]["P1_b"].ToString(), "o", false)) { VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    mainTbl.AddCell(pdfTable);

                    mainTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { BorderWidthTop = 0, BorderWidthBottom = 0, MinimumHeight = 10 });

                    pdfTable = new PdfPTable(2);
                    pdfTable.WidthPercentage = 100;
                    int[] tblCellWidth0 = { 100, 400 };
                    pdfTable.SetWidths(tblCellWidth0);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Operator Name")) { PaddingBottom = paddingvalue, PaddingTop = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(balancingDataTbl.Rows[0]["Name"].ToString())) { PaddingBottom = paddingvalue, PaddingTop = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Date ")) { PaddingBottom = paddingvalue, PaddingTop = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    string confirmdate = "";
                    if (!string.IsNullOrEmpty(balancingDataTbl.Rows[0]["ConfirmedDateAndTime"].ToString()))
                    {
                        confirmdate = Util.GetDateTime(balancingDataTbl.Rows[0]["ConfirmedDateAndTime"].ToString()).ToString("dd-MMM-yy");
                    }
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(confirmdate)) { PaddingBottom = paddingvalue, PaddingTop = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    mainTbl.AddCell(pdfTable);

                }
                else
                {
                    ReportStatus = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
            }
            return mainTbl;
        }
        #endregion

        #region ----- Hardness Report ----
        private static string SlnoForReportGeneration = "";
        internal static string hardnessReport(string machine, string ProductionOrder, string partNumber, string grnNumber)
        {
            string ReportStatus = string.Empty;
            try
            {
                PdfPTable machineDataTbl = hardnessReportTbl(machine, ProductionOrder, partNumber, 20, false, grnNumber, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 15, 15, 25, 25);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();
                    pdfDoc.Add(machineDataTbl);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=HT-" + partNumber + "-" + SlnoForReportGeneration + ".pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }
        internal static PdfPTable hardnessReportTbl(string machine, string ProductionOrder, string partNumber, int mainTblSpaceBefore, bool isFromFinalReport, string grnNumber, out string ReportStatus)
        {
            ReportStatus = string.Empty;
            PdfPTable machineDataTbl = new PdfPTable(1);
            try
            {
                DataTable secondGridData;
                DataTable firstGridTbl = GEADatabaseAccess.GetHardnessReportData(machine, ProductionOrder, partNumber, grnNumber, out secondGridData);
                if (firstGridTbl.Rows.Count >= 0)
                {
                    iTextSharp.text.BaseColor backColor = new iTextSharp.text.BaseColor(241, 241, 241);
                    machineDataTbl.SplitLate = false;
                    machineDataTbl.WidthPercentage = 100;
                    machineDataTbl.SpacingBefore = mainTblSpaceBefore;
                    machineDataTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    machineDataTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    int paddingValue = 0;
                    //header
                    byte[] file1;
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(getGEALogoPath()));//ImagePath  
                    iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(100f, 90f);
                    PdfPCell logoCell = new PdfPCell(geaLogo, false);
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPTable pdfPTable = new PdfPTable(1);
                    //int[] tblCellWidth1 = { 500, 100 };
                    //pdfPTable.SetWidths(tblCellWidth1);
                    pdfPTable.AddCell(new PdfPCell(logoCell) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 0, PaddingTop = 5 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Härte - Prüfprotokoll / Hardness test report / Procès-verbal d’essai de dureté", 13)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 });
                    machineDataTbl.AddCell(new PdfPCell(pdfPTable) { Border = 0 });

                    pdfPTable = new PdfPTable(6);
                    pdfPTable.SplitLate = false;
                    pdfPTable.WidthPercentage = 100;
                    int fs1 = 9, fs3 = 8, fs2 = 6;
                    //pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Rohteil-Nr./Unmachined part No. / No.de l’èbauche", fs1)));
                    bool textBold = false;
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Rohteil-Nr./Unmachined part No. / No.de l’èbauche", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Erzeugnisform / Product / Produid", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Reihen-Nr./Serial No./ No.de Série", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Auftrags Nr./Order No./ No.de commande", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("QB-Nr./QB-No./QB - No", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Blatt-Nr./Sheet No./ Feullie No.", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    foreach (DataRow row in secondGridData.Rows)
                    {
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["UnmachinedPartNo"].ToString())) { Border = 0, BackgroundColor = backColor });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["ProductID"].ToString())) { Border = 0, BackgroundColor = backColor });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["SerialNo"].ToString())) { Border = 0, BackgroundColor = backColor });
                        SlnoForReportGeneration = row["SerialNo"].ToString();
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["ProductionOrderNo"].ToString())) { Border = 0, BackgroundColor = backColor });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["QBNo"].ToString())) { Border = 0, BackgroundColor = backColor });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["SheetNo"].ToString())) { Border = 0, BackgroundColor = backColor });
                        break;
                    }
                    machineDataTbl.AddCell(new PdfPCell(pdfPTable) { Border = 0 });
                    //machineDataTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 10 });
                    pdfPTable = new PdfPTable(6);
                    pdfPTable.SplitLate = false;
                    pdfPTable.WidthPercentage = 100;
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Teil-Nr./ Part-No. /No. de la pièce", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Werkstoff-Nr./Material No./No.du matériau", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Prüfkraft F / Test load /Charge de éprouve", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Kugeldurchmesser / Dia. of ball Diamètre de la bille", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Umrechnungsfaktor 2) / Conversion factor 2) Coefficient de conversion 2)", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Vorschrift / Tensile strengt Résistance à la traction(Min / Max)", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    foreach (DataRow row in secondGridData.Rows)
                    {
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["PartNo"].ToString())) { Border = 0, BackgroundColor = backColor });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["MaterialNo"].ToString())) { Border = 0, BackgroundColor = backColor });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["TestLoad"].ToString())) { Border = 0, BackgroundColor = backColor });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["DiaOfBall"].ToString())) { Border = 0, BackgroundColor = backColor });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["ConversionFactor"].ToString())) { Border = 0, BackgroundColor = backColor });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["TensileStrengt"].ToString())) { Border = 0, BackgroundColor = backColor });
                        break;
                    }
                    machineDataTbl.AddCell(new PdfPCell(pdfPTable) { Border = 0 });
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 20 });
                    pdfPTable = new PdfPTable(8);
                    pdfPTable.SplitLate = false;
                    pdfPTable.WidthPercentage = 100;
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Charge", fs1)));
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Lfd-Nr. / Consecutive No. / No.d´ordre", fs1)));
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("WB- Los.- Nr.HT - Lot - No.TT - No d´Lot", fs1)));
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Härtewert1) / Hardness No1) / Indice de dreté1)", fs1)) { Colspan = 4 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Bemerkung Remarks Remarques", fs1)));
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("", fs1)));
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("", fs1)));
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("", fs1)));
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("0°", fs3)) { });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("90°", fs3)) { });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("180°", fs3)) { });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("270°", fs3)) { });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("", fs1)));
                    foreach (DataRow row in secondGridData.Rows)
                    {
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["Charge"].ToString())));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["ConsecutiveNo"].ToString())));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["LotNo"].ToString())));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["HardnessNo_0D"].ToString())));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["HardnessNo_90D"].ToString())));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["HardnessNo_180D"].ToString())));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["HardnessNo_270D"].ToString())));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["Remarks"].ToString())));
                    }
                    machineDataTbl.AddCell(new PdfPCell(pdfPTable));
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 20 });

                    pdfPTable = new PdfPTable(4);
                    pdfPTable.SplitLate = false;
                    pdfPTable.WidthPercentage = 100;
                    PdfPTable innerTbl = new PdfPTable(1);
                    textBold = false;
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithText("Bemerkungen/Remarks/Nota", fs2, textBold)) { Border = 0 });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithText("1) Härteprüfung nach DIN EN ISO 6506-1 / 6506-2 / EN ISO 6507-1 Hardness test according to DIN EN ISO 6506 - 1 / 6506 - 2 / EN ISO 6507 - 1 Essai de dureté selon DIN EN ISO 6506 - 1 / 6506 - 2 / EN ISO 6507 - 1", fs2, textBold)) { Border = 0 });
                    innerTbl.AddCell(new PdfPCell(getPdfCellWithText("2) Umrechnungsfaktor siehe WSN 60-/ Conversion factor see WSN 60- / Coefficient de conversion, voir normes WSN 60 - ", fs2, textBold)) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(innerTbl) { Rowspan = 2, Border = 0 });

                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Ort: / Place: / Lieu:", fs1, textBold)) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Datum: / Date: / Date d’essai:", fs1, textBold)) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Name des Prüfers: / Name of examiner: / Nom de l’essayeur: ", fs1, textBold)) { Border = 0 });

                    foreach (DataRow row in secondGridData.Rows)
                    {
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["Place"].ToString())) { Border = 0 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["Date"].ToString())) { Border = 0 });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(row["NameOfExaminer"].ToString())) { Border = 0 });
                        break;
                    }
                    machineDataTbl.AddCell(new PdfPCell(pdfPTable) { Border = 0 });
                }
                else
                {
                    ReportStatus = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
            }
            return machineDataTbl;
        }
        #endregion

        #region ----- Dye Penetration Report ------
        internal static string DyePenetrationReport(string machine, string ProductionOrder, string partNumber, string grnNumber)
        {
            string ReportStatus = string.Empty;
            try
            {
                PdfPTable machineDataTbl = DyePenetrationReportTbl(machine, ProductionOrder, partNumber, 20, false, grnNumber, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 15, 15, 25, 25);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();
                    pdfDoc.Add(machineDataTbl);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=DPT-" + partNumber + "-" + SlnoForReportGeneration + ".pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }
        internal static PdfPTable DyePenetrationReportTbl(string machine, string ProductionOrder, string partNumber, int mainTblSpaceBefore, bool isFromFinalReport, string grnNumber, out string ReportStatus)
        {
            ReportStatus = string.Empty;
            PdfPTable machineDataTbl = new PdfPTable(1);
            try
            {
                DataTable secondGridData;
                DataTable firstGridTbl = GEADatabaseAccess.GetDyePenetrationReportData(machine, ProductionOrder, partNumber, grnNumber, out secondGridData);
                if (firstGridTbl.Rows.Count >= 0)
                {
                    iTextSharp.text.BaseColor backColor = new iTextSharp.text.BaseColor(241, 241, 241);
                    machineDataTbl.SplitLate = false;
                    machineDataTbl.WidthPercentage = 100;
                    machineDataTbl.SpacingBefore = mainTblSpaceBefore;
                    machineDataTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    machineDataTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    int paddingValue = 0;
                    //header
                    byte[] file1;
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(getGEALogoPath()));//ImagePath  
                    iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(100f, 90f);
                    PdfPCell logoCell = new PdfPCell(geaLogo, false);
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPTable pdfPTable = new PdfPTable(1);
                    //int[] tblCellWidth1 = { 500, 100 };
                    //pdfPTable.SetWidths(tblCellWidth1);
                    pdfPTable.AddCell(new PdfPCell(logoCell) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 0, PaddingTop = 5 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Farbeindring-Prüfprotokoll / Dye penetration test report / Procès-verbal d‘essai de ressuage \nWSN76 - 0019 - 00(EN ISO 3452 - 1)", 13)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 });
                    machineDataTbl.AddCell(new PdfPCell(pdfPTable) { Border = 0 });
                    machineDataTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 15 });
                    pdfPTable = new PdfPTable(6);
                    pdfPTable.SplitLate = false;
                    pdfPTable.WidthPercentage = 100;
                    int fs1 = 9, fs3 = 8, fs2 = 6;
                    bool textBold = false;
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Rohteil-Nr./Unmachined part No. / No.de l’èbauche", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Erzeugnisform / Product / Produid", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Reihen-Nr./Serial No./ No.de Série", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Auftrags Nr./Order No./ No.de commande", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("QB-Nr./QB-No./QB - No.", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Blatt-Nr./Sheet No./ Feullie No.", fs1, textBold)) { Border = 0, BackgroundColor = backColor });
                    foreach (DataRow row in secondGridData.Rows)
                    {
                        textBold = true;
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(row["UnmachinedPartNo"].ToString(), fs3, textBold)) { Border = 0, BackgroundColor = backColor });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(row["ProductID"].ToString(), fs3, textBold)) { Border = 0, BackgroundColor = backColor });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(row["SerialNo"].ToString(), fs3, textBold)) { Border = 0, BackgroundColor = backColor });
                        SlnoForReportGeneration = row["SerialNo"].ToString();
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(row["ProductionOrderNo"].ToString(), fs3, textBold)) { Border = 0, BackgroundColor = backColor });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(row["QBNo"].ToString(), fs3, textBold)) { Border = 0, BackgroundColor = backColor });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(row["SheetNo"].ToString(), fs3, textBold)) { Border = 0, BackgroundColor = backColor });
                        break;
                    }
                    textBold = false;
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Teil-Nr./ Part-No./No. de la pièce", fs1, textBold)) { Colspan = 2, Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Werkstoff-Nr./Material No./No.du matériau", fs1, textBold)) { Colspan = 2, Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, BackgroundColor = backColor });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, BackgroundColor = backColor });
                    foreach (DataRow row in secondGridData.Rows)
                    {
                        textBold = true;
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(row["PartNo"].ToString(), fs3, textBold)) { Colspan = 2, Border = 0, BackgroundColor = backColor });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(row["MaterialNo"].ToString(), fs3, textBold)) { Colspan = 2, Border = 0, BackgroundColor = backColor });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, BackgroundColor = backColor });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, BackgroundColor = backColor });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, BackgroundColor = backColor });
                        break;
                    }
                    textBold = false;
                    machineDataTbl.AddCell(new PdfPCell(pdfPTable) { Border = 0 });
                    // machineDataTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 20 });

                    DataRow tblRow = secondGridData.Rows[0];
                    pdfPTable = new PdfPTable(5);
                    pdfPTable.SplitLate = false;
                    pdfPTable.WidthPercentage = 100;
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Bezeichnung des Prüfmittelsystems/Testing agent System / Système de ressuage", fs3, textBold)) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Vorreinigung  / Pre-Cleaning / Pré nettoyage \nSKC-1\n\n", fs3, textBold)) { Border = 0 });
                    PdfPTable innerTable = new PdfPTable(1);
                    innerTable.SplitLate = false;
                    innerTable.WidthPercentage = 100;
                    innerTable.AddCell(new PdfPCell(getPdfCellWithText("Charge", fs3, textBold)) { Border = 0 });
                    innerTable.AddCell(new PdfPCell(getPdfCellWithText(tblRow["Charge1"].ToString(), fs3, textBold)) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(innerTable) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Prüftemperatur / Test Temperature / Temperature d’essai\nRoom Temperature", fs3, textBold)) { Border = 0 });

                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("//AD", fs3, true)) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Eindringmittel (Rot) / Dye penetrant (red) / Pénétrant (rouge)\nSKL-WP\n\n", fs3, textBold)) { Border = 0 });
                    innerTable = new PdfPTable(1);
                    innerTable.SplitLate = false;
                    innerTable.WidthPercentage = 100;
                    innerTable.AddCell(new PdfPCell(getPdfCellWithText("Charge", fs3, textBold)) { Border = 0 });
                    innerTable.AddCell(new PdfPCell(getPdfCellWithText(tblRow["Charge2"].ToString(), fs3, textBold)) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(innerTable) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Zeit / Time / Temps\n 15 Minutes", fs3, textBold)) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Prüfumfang / Test scope / Etendue de Contrôle \n\n 100%", fs3, textBold)) { Border = 0 });

                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Zwischenreiniger / Intermediate cleaning agent / Nettoyage intermédiaire\n\n", fs3, textBold)) { Border = 0 });
                    innerTable = new PdfPTable(1);
                    innerTable.SplitLate = false;
                    innerTable.WidthPercentage = 100;
                    innerTable.AddCell(new PdfPCell(getPdfCellWithText("Charge", fs3, textBold)) { Border = 0 });
                    innerTable.AddCell(new PdfPCell(getPdfCellWithText("WATER", fs3, textBold)) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(innerTable) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });

                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Entwickler / Developer / Révélateur\nSKD-S2\n\n", fs3, textBold)) { Border = 0 });
                    innerTable = new PdfPTable(1);
                    innerTable.SplitLate = false;
                    innerTable.WidthPercentage = 100;
                    innerTable.AddCell(new PdfPCell(getPdfCellWithText("Charge", fs3, textBold)) { Border = 0 });
                    innerTable.AddCell(new PdfPCell(getPdfCellWithText(tblRow["Charge3"].ToString(), fs3, textBold)) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(innerTable) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Zeit / Time / Temps\n 15 Minutes", fs3, textBold)) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                    machineDataTbl.AddCell(new PdfPCell(pdfPTable) { Border = 0 });

                    machineDataTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 20 });

                    pdfPTable = new PdfPTable(5);
                    pdfPTable.SplitLate = false;
                    pdfPTable.WidthPercentage = 100;
                    textBold = true;
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Lfd. Nr in der Reihe Consecutive number No.d’ordre", fs3, textBold)) { });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Arbeitsfolge Seq.of operation Séq.de travaux", fs3, textBold)) { });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Fehlerfrei bzw belassbar Faultless or acceptable Sans défaut resp.accepable", fs3, textBold)) { });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Fehlerfrei nach Nacharbeit Faultless after reworking Sans défauts après retouche", fs3, textBold)) { });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Bemerkung Remarks Remarques", fs3, textBold)) { });
                    textBold = false;
                    foreach (DataRow row in secondGridData.Rows)
                    {
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(row["ConsecutiveNo"].ToString(), fs3, textBold)) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(row["SeqOfOperation"].ToString(), fs3, textBold)) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(row["Faultless"].ToString(), fs3, textBold)) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(row["FaultlessAfterRework"].ToString(), fs3, textBold)) { });
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(row["Remarks"].ToString(), fs3, textBold)) { });
                    }
                    machineDataTbl.AddCell(new PdfPCell(pdfPTable));

                    machineDataTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 20 });

                    pdfPTable = new PdfPTable(4);
                    pdfPTable.SplitLate = false;
                    pdfPTable.WidthPercentage = 100;
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Ort: / Place: / Lieu:", fs3, textBold)) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Datum: / Date: / Date d’essai:", fs3, textBold)) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Name des Prüfers: / Name of examiner: / Nom de L’essayeur: ", fs3, textBold)) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Nr. des Prüfers: / No. of examiner: / No.de L’essayeur", fs3, textBold)) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(tblRow["Place"].ToString(), fs3, textBold)) { Border = 0 });
                    string date = string.IsNullOrEmpty(tblRow["Date"].ToString()) ? "" : Util.GetDateTime(tblRow["Date"].ToString()).ToString("dd-MM-yyyy");
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(date, fs3, textBold)) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(tblRow["NameOfExaminer"].ToString(), fs3, textBold)) { Border = 0 });
                    pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(tblRow["NoOfExaminer"].ToString(), fs3, textBold)) { Border = 0 });
                    machineDataTbl.AddCell(new PdfPCell(pdfPTable) { Border = 0 });
                }
                else
                {
                    ReportStatus = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
            }
            return machineDataTbl;
        }
        #endregion


        #region ----- First Sample Report ------

        internal static string FirstSampleReport(string machine, string ProductionOrder, string partNumber, string grnNumber)
        {
            string ReportStatus = string.Empty;
            try
            {
                reportNameForHeaderFooter = "FirstSample";
                PdfPTable machineDataTbl1 = FirstSampleReportTbl(machine, ProductionOrder, partNumber, 0, false, "first", grnNumber, out ReportStatus);
                PdfPTable machineDataTbl2 = FirstSampleReportTbl(machine, ProductionOrder, partNumber, 0, false, "second", grnNumber, out ReportStatus);
                PdfPTable machineDataTbl3 = FirstSampleReportTbl(machine, ProductionOrder, partNumber, 0, false, "third", grnNumber, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    Document pdfDoc = new Document(PageSize.A4, 15, 15, 10, 10);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();
                    pdfWriter.PageEvent = new HeaderFooter();
                    pdfDoc.Add(getFinalReportParagraph(machineDataTbl1));
                    Paragraph p2 = new Paragraph();
                    pdfDoc.Add(p2);
                    pdfDoc.NewPage();
                    pdfDoc.Add(getFinalReportParagraph(machineDataTbl2));
                    p2 = new Paragraph();
                    pdfDoc.Add(p2);
                    pdfDoc.NewPage();
                    pdfDoc.Add(getFinalReportParagraph(machineDataTbl3));
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=FSR-" + partNumber + "-" + ProductionOrder + ".pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }
        internal static PdfPTable FirstSampleReportTbl(string machine, string ProductionOrder, string partNumber, int mainTblSpaceBefore, bool isFromFinalReport, string param, string grnNumber, out string ReportStatus)
        {
            ReportStatus = string.Empty;
            PdfPTable machineDataTbl = new PdfPTable(1);

            try
            {

                DataSet gridTbls = new DataSet();
                if (param.Equals("first", StringComparison.OrdinalIgnoreCase))
                {
                    gridTbls = GEADatabaseAccess.GetFirstSampleReportData(machine, ProductionOrder, partNumber, grnNumber);
                    HttpContext.Current.Session["FirstSampleData"] = gridTbls;

                }
                else
                {
                    if (HttpContext.Current.Session["FirstSampleData"] != null)
                    {
                        gridTbls = HttpContext.Current.Session["FirstSampleData"] as DataSet;
                    }

                }
                DataTable firstTbl, secondTbl, thirdTbl, fourthTbl;
                firstTbl = gridTbls.Tables[0];
                secondTbl = gridTbls.Tables[1];
                thirdTbl = gridTbls.Tables[2];
                fourthTbl = gridTbls.Tables[3];
                if (firstTbl.Rows.Count >= 0)
                {
                    reportName = "FirstSample";
                    iTextSharp.text.BaseColor backColor = new iTextSharp.text.BaseColor(216, 216, 216);
                    iTextSharp.text.BaseColor borderColor = getPdfCellBorderColor();
                    machineDataTbl.SplitLate = false;
                    machineDataTbl.WidthPercentage = 100;
                    machineDataTbl.SpacingBefore = mainTblSpaceBefore;
                    machineDataTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    machineDataTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    byte[] file1;
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(getGEALogoPath()));//ImagePath  
                    iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(100f, 70f);
                    PdfPCell logoCell = new PdfPCell(geaLogo, false);
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    logoCell.Border = 0;
                    int fs1 = 9, fs3 = 7, fs2 = 8;
                    bool textBold = false;
                    int colspan = 1;
                    if (param.Equals("first", StringComparison.OrdinalIgnoreCase))
                    {
                        int paddingValue = 0;
                        //header
                        PdfPTable pdfPTable = new PdfPTable(2);
                        int[] tblCellWidth1 = { 100, 500 };
                        pdfPTable.SetWidths(tblCellWidth1);
                        pdfPTable.AddCell(new PdfPCell(logoCell));
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("First sample report", 13)) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = backColor });
                        machineDataTbl.AddCell(new PdfPCell(pdfPTable) { Border = 0 });
                        machineDataTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 15 });

                        pdfPTable = new PdfPTable(2);
                        pdfPTable.SplitLate = false;
                        pdfPTable.WidthPercentage = 100;
                        int[] tblCellWidth2 = { 30, 700 };
                        pdfPTable.SetWidths(tblCellWidth2);
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("GEA-Purchasing", fs1, textBold)) { BackgroundColor = backColor, Rotation = 90, VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER });

                        DataRow tblRow = secondTbl.Rows[0];
                        PdfPTable innerTable = new PdfPTable(3);
                        innerTable.SplitLate = false;
                        innerTable.WidthPercentage = 100;
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("1st Inspection", tblRow["Inspection1"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("2nd Inspection", tblRow["Inspection2"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("3rd Inspection", tblRow["Inspection3"].ToString(), 9)) { BorderColor = borderColor });

                        PdfPTable innerTable1 = new PdfPTable(2);
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Date", fs2, textBold)) { BackgroundColor = backColor });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText(Util.GetDateTime(tblRow["InspectionDate"].ToString()).ToString("dd-MM-yyyy"), fs2, textBold)));
                        innerTable.AddCell(new PdfPCell(innerTable1));
                        innerTable.AddCell(new PdfPCell(getPdfCellWithText("GEA Company", fs2, textBold)) { BackgroundColor = backColor });
                        innerTable.AddCell(new PdfPCell(getPdfCellWithText("")));

                        innerTable.AddCell(new PdfPCell(getPdfCellWithText("Reason for first sample inspection", fs2, true)) { Colspan = 3, BackgroundColor = backColor });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("New supplier", tblRow["Reason_NewSupplier"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("New part", tblRow["Reason_NewPart"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("Amended specification", tblRow["Reason_AmendedSpecific"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("Amended", tblRow["Reason_Amended"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("Other", tblRow["Reason_Other"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("Repetitive inspection (for application in a different GEA Comp.", tblRow["Reason_RepetativeInspection"].ToString(), 9)) { BorderColor = borderColor });
                        pdfPTable.AddCell(new PdfPCell(innerTable) { BorderColor = borderColor });

                        int minHeight = 15;
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("")) { Colspan = 2, Border = 0, MinimumHeight = minHeight });

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Supplier", fs1, textBold)) { BackgroundColor = backColor, Rotation = 90, VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 });
                        innerTable = new PdfPTable(3);
                        innerTable.SplitLate = false;
                        innerTable.AddCell(new PdfPCell(getFirstSamplePdfPTable("Supplier", tblRow["Supplier"].ToString(), fs2, backColor)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getFirstSamplePdfPTable("Order No.", tblRow["ProductionOrderNo"].ToString(), fs2, backColor)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getFirstSamplePdfPTable("Designation", "", fs2, backColor)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getFirstSamplePdfPTable("Part No.", tblRow["PartNo"].ToString(), fs2, backColor)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getFirstSamplePdfPTable("Material", tblRow["Material"].ToString(), fs2, backColor)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getFirstSamplePdfPTable("Number of samples", tblRow["NoOfSamples"].ToString(), fs2, backColor)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getPdfCellWithText("Documents submitted by suppliers", fs2, true)) { Colspan = 3, BackgroundColor = backColor });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("Measuring report", tblRow["DocumentType1"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("3.1 certificate according to DIN EN 10204", tblRow["DocumentType2"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("other inspection results (see encl.)", tblRow["DocumentType3"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getPdfCellWithText("Input stock used (for forged / hot-formed parts only)", fs2, true)) { Colspan = 3, BackgroundColor = backColor });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("Ingot casting", tblRow["InputStockUsed1"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("Continuous casting", tblRow["InputStockUsed2"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("Strain (at least 4-fold)", tblRow["InputStockUsed3"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getPdfCellWithText("Inspections carried out on standard parts", fs2, true)) { Colspan = 3, BackgroundColor = backColor });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("In accordance to specification of relevant DN-/ EN-Standard and considered as good", tblRow["InspectionCarriedBit"].ToString(), 9)) { Colspan = 3, BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(getPdfCellWithText("Confirmation of supplier", fs2, true)) { Colspan = 3, BackgroundColor = backColor });

                        innerTable1 = new PdfPTable(1);
                        innerTable1.SplitLate = false;
                        innerTable1.WidthPercentage = 100;
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("We hereby confirm that", fs2, textBold)));
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("    1. The first samples submitted were manufactured exclusively with series equipment under series conditions.", fs2, textBold)));
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("    2. The correct execution of first sample inspection and correctness of the first sample report \n  (any deviations must be specified separately in this report).", fs2, textBold)));
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("    3. A release does not discharge the supplier from his obligation to deliver in accordance with the respective valid drawing and specified functional features.", fs2, textBold)));
                        innerTable.AddCell(new PdfPCell(innerTable1) { Colspan = 3 });
                        innerTable.AddCell(new PdfPCell(getPdfCellWithText("Comments", fs2, true)) { Colspan = 3, BackgroundColor = backColor });
                        innerTable.AddCell(new PdfPCell(getPdfCellWithText(tblRow["Comment"].ToString(), fs2, textBold)) { Colspan = 2 });
                        innerTable1 = new PdfPTable(2);
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText(tblRow["Date"].ToString() + " , " + tblRow["Signature"].ToString(), fs2, textBold)) { Colspan = 2, Border = 0 });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Date, Signature", fs2, textBold)) { Border = 0 });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Company stamp", fs2, textBold)) { Border = 0 });
                        innerTable.AddCell(new PdfPCell(innerTable1) { BorderColor = borderColor });
                        pdfPTable.AddCell(new PdfPCell(innerTable) { Border = 0 });

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("")) { Colspan = 2, Border = 0, MinimumHeight = minHeight });

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("GEA", fs1, textBold)) { BackgroundColor = backColor, Rotation = 90, VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER });
                        colspan = 1;
                        innerTable = new PdfPTable(colspan);
                        innerTable.SplitLate = false;
                        innerTable.AddCell(new PdfPCell(getPdfCellWithText("Inspection through GEA", fs2, true)) { Colspan = colspan, BackgroundColor = backColor, Border = 0 });
                        innerTable.AddCell(new PdfPCell(getPdfCellWithText("Material", fs2, true)) { Colspan = colspan, BackgroundColor = backColor, BorderWidthTop = 0 });
                        innerTable1 = new PdfPTable(4);
                        innerTable1.AddCell(new PdfPCell(getcheckBoxWithText("Mechanical test", tblRow["Material_MechanicalTest"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable1.AddCell(new PdfPCell(getcheckBoxWithText("Analysis check", tblRow["Material_AnalysisCheck"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable1.AddCell(new PdfPCell(getcheckBoxWithText("Structure", tblRow["Material_Structure"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable1.AddCell(new PdfPCell(getcheckBoxWithText("Other", tblRow["Material_Other"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(innerTable1) { Colspan = colspan });
                        innerTable.AddCell(new PdfPCell(getPdfCellWithText("Comments", fs2, true)) { Colspan = colspan, BackgroundColor = backColor });
                        innerTable.AddCell(new PdfPCell(getPdfCellWithText(tblRow["GEAComments"].ToString(), fs2, textBold)) { Colspan = colspan, MinimumHeight = 30 });
                        innerTable.AddCell(new PdfPCell(getPdfCellWithText("Dimensions", fs2, true)) { Colspan = colspan, BackgroundColor = backColor });
                        innerTable1 = new PdfPTable(5);
                        innerTable1.AddCell(new PdfPCell(getcheckBoxWithText("Dimensional check", tblRow["Dimension_Check"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Condition upon delivery:", fs2, textBold)) { VerticalAlignment = Element.ALIGN_MIDDLE });
                        innerTable1.AddCell(new PdfPCell(getcheckBoxWithText("raw", tblRow["Dimension_Check1"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable1.AddCell(new PdfPCell(getcheckBoxWithText("pre-machined", tblRow["Dimension_Check2"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable1.AddCell(new PdfPCell(getcheckBoxWithText("finished / ready for use", tblRow["Dimension_Check3"].ToString(), 9)) { BorderColor = borderColor });
                        innerTable.AddCell(new PdfPCell(innerTable1) { Colspan = colspan });
                        innerTable.AddCell(new PdfPCell(getPdfCellWithText("(when having performed a dimensional check, please enclose measuring report)", fs2, textBold)) { Colspan = colspan });
                        pdfPTable.AddCell(new PdfPCell(innerTable) { Border = 0 });

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("")) { Colspan = 2, Border = 0, MinimumHeight = minHeight });

                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("GEA", fs1, textBold)) { BackgroundColor = backColor, Rotation = 90, VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER });
                        innerTable = new PdfPTable(2);
                        innerTable.SplitLate = false;
                        colspan = 4;
                        innerTable1 = new PdfPTable(colspan);
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Assessment by GEA", fs2, true)) { Colspan = colspan, BackgroundColor = backColor });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Decision", fs2, textBold)) { });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Released", fs2, textBold)) { BackgroundColor = backColor });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Released subject to conditions", fs2, textBold)) { BackgroundColor = backColor });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("First sample rejected", fs2, textBold)) { BackgroundColor = backColor });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Measuring results", fs2, textBold)) { Rowspan = 2, BackgroundColor = backColor });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("", fs2, textBold)) { MinimumHeight = 10 });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("", fs2, textBold)) { MinimumHeight = 10 });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("", fs2, textBold)) { MinimumHeight = 10 });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Name/ signature", fs2, textBold)) { });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Name/ signature", fs2, textBold)) { });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Name/ signature", fs2, textBold)) { });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Material", fs2, textBold)) { Rowspan = 2, BackgroundColor = backColor });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("", fs2, textBold)) { MinimumHeight = 10 });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("", fs2, textBold)) { MinimumHeight = 10 });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("", fs2, textBold)) { MinimumHeight = 10 });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Name/ signature", fs2, textBold)) { });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Name/ signature", fs2, textBold)) { });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Name/ signature", fs2, textBold)) { });
                        //innerTable1.AddCell(new PdfPCell(getPdfCellWithText("", fs2, textBold)) { });
                        // innerTable1.AddCell(new PdfPCell(getPdfCellWithText("", fs2, textBold)) { });
                        innerTable.AddCell(new PdfPCell(innerTable1));


                        colspan = 1;
                        innerTable1 = new PdfPTable(colspan);
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Conditions or reasons for rejection", fs2, true)) { Colspan = colspan, BackgroundColor = backColor });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText(tblRow["GEAReasonForRejection"].ToString(), fs2, textBold)) { });
                        innerTable.AddCell(new PdfPCell(innerTable1) { Border = 0 });

                        innerTable.AddCell(new PdfPCell(getPdfCellWithText("")) { BorderColor = borderColor });
                        colspan = 2;
                        innerTable1 = new PdfPTable(colspan);
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText(string.IsNullOrEmpty(tblRow["RejDate"].ToString()) ? "" : Util.GetDateTime(tblRow["RejDate"].ToString()).ToString("dd-MM-yyyy") + ", " + tblRow["RejSignature"].ToString(), fs2, true)) { Colspan = colspan });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Datum, signature", fs2, textBold)) { });
                        innerTable1.AddCell(new PdfPCell(getPdfCellWithText("Company stamp", fs2, textBold)) { });
                        innerTable.AddCell(new PdfPCell(innerTable1) { Border = 0 });
                        pdfPTable.AddCell(new PdfPCell(innerTable) { Border = 0 });
                        machineDataTbl.AddCell(new PdfPCell(pdfPTable) { Border = 0 });
                    }
                    else if (param.Equals("second", StringComparison.OrdinalIgnoreCase))
                    {
                        if (thirdTbl.Rows.Count > 0)
                        {
                            DataRow tblRow = thirdTbl.Rows[0];

                            PdfPTable pdfPTable = new PdfPTable(2);
                            int[] tblCellWidth1 = { 100, 500 };
                            pdfPTable.SetWidths(tblCellWidth1);
                            pdfPTable.AddCell(new PdfPCell(logoCell) { Border = 0 });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("First sample measuring report", 13)) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 });
                            machineDataTbl.AddCell(new PdfPCell(pdfPTable) { Border = 0, BackgroundColor = backColor });

                            machineDataTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 7 });

                            pdfPTable = new PdfPTable(4);
                            pdfPTable.SplitLate = false;
                            pdfPTable.WidthPercentage = 100;
                            int[] tblCellWidth2 = { 100, 500, 100, 300 };
                            pdfPTable.SetWidths(tblCellWidth2);
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Supplier", fs2, textBold)) { VerticalAlignment = Element.ALIGN_RIGHT, BackgroundColor = backColor, BorderColor = borderColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(tblRow["Supplier"].ToString(), fs2, textBold)) { BorderColor = borderColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Part No.", fs2, textBold)) { VerticalAlignment = Element.ALIGN_RIGHT, BackgroundColor = backColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(tblRow["PartNo"].ToString(), fs2, textBold)) { });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Order No.", fs2, textBold)) { VerticalAlignment = Element.ALIGN_RIGHT, BackgroundColor = backColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(tblRow["ProductionOrderNo"].ToString(), fs2, textBold)) { });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("IAW Standard", fs2, textBold)) { VerticalAlignment = Element.ALIGN_RIGHT, BackgroundColor = backColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(tblRow["IAWStandard"].ToString(), fs2, textBold)) { });
                            machineDataTbl.AddCell(new PdfPCell(pdfPTable) { Border = 0 });

                            machineDataTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 7 });

                            pdfPTable = new PdfPTable(7);
                            pdfPTable.SplitLate = false;
                            pdfPTable.WidthPercentage = 100;
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Pos.", fs2, true)) { BackgroundColor = backColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Set value", fs2, true)) { BackgroundColor = backColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("As is value supplier", fs2, true)) { BackgroundColor = backColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("ok", fs2, true)) { BackgroundColor = backColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("n. ok", fs2, true)) { BackgroundColor = backColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("As is value GEA", fs2, true)) { BackgroundColor = backColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Assessment of deviation (see below)", fs2, true)) { BackgroundColor = backColor });

                            for (int i = 0; i < thirdTbl.Rows.Count; i++)
                            {
                                pdfPTable.AddCell(new PdfPCell(getPdfCellWithText((i + 1).ToString(), fs2, textBold)) { });
                                pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(thirdTbl.Rows[i]["SetValue"].ToString(), fs2, textBold)) { });
                                pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(thirdTbl.Rows[i]["AsIsValueSupplier"].ToString(), fs2, textBold)) { });
                                pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(thirdTbl.Rows[i]["Ok"].ToString(), fs2, textBold)) { });
                                pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(thirdTbl.Rows[i]["NOk"].ToString(), fs2, textBold)) { });
                                pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(thirdTbl.Rows[i]["AsIsValueGEA"].ToString(), fs2, textBold)) { });
                                pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(thirdTbl.Rows[i]["AssessmentOfDeviation"].ToString(), fs2, textBold)) { });
                            }
                            machineDataTbl.AddCell(new PdfPCell(pdfPTable));

                            machineDataTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 7 });

                            colspan = 2;
                            pdfPTable = new PdfPTable(colspan);
                            pdfPTable.SplitLate = false;
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Assessment of deviations from GEA Mechanical Equipment", fs2, true)) { Colspan = colspan, BackgroundColor = backColor });
                            PdfPTable innerTbl = new PdfPTable(1);
                            innerTbl.AddCell(new PdfPCell(getPdfCellWithText("1 - Deviation accepted", fs2, textBold)) { PaddingTop = 5, Border = 0 });
                            innerTbl.AddCell(new PdfPCell(getPdfCellWithText("2 - Deviation accepted after check back with: ........................................", fs2, textBold)) { Border = 0 });
                            innerTbl.AddCell(new PdfPCell(getPdfCellWithText("   (see comments)", fs2, textBold)) { PaddingBottom = 5, Border = 0 });
                            pdfPTable.AddCell(new PdfPCell(innerTbl) { BorderColor = backColor });
                            innerTbl = new PdfPTable(1);
                            innerTbl.AddCell(new PdfPCell(getPdfCellWithText("3 - Deviation not accepted", fs2, textBold)) { PaddingTop = 5, Border = 0 });
                            innerTbl.AddCell(new PdfPCell(getPdfCellWithText("4 - Miscellaneous (see comments)", fs2, textBold)) { PaddingBottom = 5, Border = 0 });
                            pdfPTable.AddCell(new PdfPCell(innerTbl) { BorderColor = backColor });

                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Comments", fs2, true)) { Colspan = colspan, BackgroundColor = backColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(tblRow["Comment"].ToString(), fs2, textBold)) { Colspan = colspan, MinimumHeight = 10 });

                            pdfPTable.AddCell(new PdfPCell(getcheckBoxWithText("Comments continued on a separate sheet", "0", 9)) { PaddingTop = 5, PaddingBottom = 5, BorderColor = borderColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("")) { BorderWidthBottom = 0 });
                            string date = string.IsNullOrEmpty(tblRow["Date"].ToString()) ? "" : Util.GetDateTime(tblRow["Date"].ToString()).ToString("dd-MM-yyyy");
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Date: " + date, fs2, textBold)) { });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Signature: " + tblRow["Signature"].ToString(), fs2, textBold)) { BorderWidthTop = 0 });
                            machineDataTbl.AddCell(new PdfPCell(pdfPTable));

                        }
                    }
                    else if (param.Equals("third", StringComparison.OrdinalIgnoreCase))
                    {
                        if (fourthTbl.Rows.Count > 0)
                        {
                            DataRow tblRow = fourthTbl.Rows[0];

                            PdfPTable pdfPTable = new PdfPTable(7);
                            pdfPTable.SplitLate = false;
                            pdfPTable.WidthPercentage = 100;
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Pos.", fs2, true)) { BackgroundColor = backColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Set value", fs2, true)) { BackgroundColor = backColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("As is value supplier", fs2, true)) { BackgroundColor = backColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("ok", fs2, true)) { BackgroundColor = backColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("n. ok", fs2, true)) { BackgroundColor = backColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("As is value GEA", fs2, true)) { BackgroundColor = backColor });
                            pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("Assessment of deviation (see below)", fs2, true)) { BackgroundColor = backColor });
                            for (int i = 0; i < fourthTbl.Rows.Count; i++)
                            {
                                pdfPTable.AddCell(new PdfPCell(getPdfCellWithText((i + 1).ToString(), fs2, textBold)) { });
                                pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(thirdTbl.Rows[i]["SetValue"].ToString(), fs2, textBold)) { });
                                pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(thirdTbl.Rows[i]["AsIsValueSupplier"].ToString(), fs2, textBold)) { });
                                pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(thirdTbl.Rows[i]["Ok"].ToString(), fs2, textBold)) { });
                                pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(thirdTbl.Rows[i]["NOk"].ToString(), fs2, textBold)) { });
                                pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(thirdTbl.Rows[i]["AsIsValueGEA"].ToString(), fs2, textBold)) { });
                                pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(thirdTbl.Rows[i]["AssessmentOfDeviation"].ToString(), fs2, textBold)) { });
                            }
                            machineDataTbl.AddCell(new PdfPCell(pdfPTable));
                        }
                    }
                    else
                    {
                        ReportStatus = "NoDataFound";
                    }
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
            }
            finally
            {
                reportName = "";
            }
            return machineDataTbl;
        }

        public partial class HeaderFooter : PdfPageEventHelper
        {
            public override void OnEndPage(PdfWriter writer, Document doc)
            {
                GEAGenerateReport thiPage = new GEAGenerateReport();
                bool isHeaderFooterRequired = GEAGenerateReport.isHeaderFooterRequired;
                bool isPagePotrait = GEAGenerateReport.isPagePotrait;
                string reportName = GEAGenerateReport.reportNameForHeaderFooter;
                iTextSharp.text.BaseColor backColor = new iTextSharp.text.BaseColor(242, 242, 242);
                iTextSharp.text.BaseColor backColor1 = new iTextSharp.text.BaseColor(227, 227, 227);
                iTextSharp.text.BaseColor borderColor = backColor;
                int fs2 = 6;
                //footer
                PdfPTable finalTable = new PdfPTable(7);
                if (reportName.Equals("FirstSample", StringComparison.OrdinalIgnoreCase))
                {
                    finalTable = new PdfPTable(7);
                    finalTable.TotalWidth = 500;
                    finalTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("Number", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor1 });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("FB 7.4.3-3", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("Date", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor1 });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("21.03.2014", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("Revision", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor1 });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("0", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("E-SCP 4.1.3 Quality & HSE 59302 Oelde, Germany", fs2, false)) { Rowspan = 2, BorderColor = borderColor, BackgroundColor = backColor1 });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("Edited", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor1 });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("J. Getz", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor1 });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("U. Förster", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("Released", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor1 });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("D. Schlingmeyer", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor });

                    finalTable.WriteSelectedRows(0, -1, 45, 45, writer.DirectContent);
                }
                if (reportName.Equals("QualityTestProtocol", StringComparison.OrdinalIgnoreCase))
                {
                    if (isHeaderFooterRequired == false)
                    {
                        return;
                    }
                    finalTable = new PdfPTable(7);
                    if (isPagePotrait)
                    {
                        finalTable.TotalWidth = 500;
                    }
                    else
                    {
                        finalTable.TotalWidth = 750;
                    }
                    finalTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("Number", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor1 });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("FB 7.4.3-2", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("Date", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor1 });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("12.06.2014", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("Revision", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor1 });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("0", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("E-SCP 4.1.3 Quality & HSE 59302 Oelde, Germany", fs2, false)) { Rowspan = 2, BorderColor = borderColor, BackgroundColor = backColor1 });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("Edited", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor1 });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("J. Getz", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor1 });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("B. Guthues", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("Released", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor1 });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("D. Schlingmeyer", fs2, false)) { BorderColor = borderColor, BackgroundColor = backColor });

                    finalTable.WriteSelectedRows(0, -1, 45, 45, writer.DirectContent);
                }
                if (reportName.Equals("ProDecanter", StringComparison.OrdinalIgnoreCase))
                {
                    if (isHeaderFooterRequired == false)
                    {
                        return;
                    }
                    fs2 = 7;
                    finalTable = new PdfPTable(4);
                    finalTable.TotalWidth = 500;
                    finalTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("Worked: \nPK     2021.10.01", fs2, false)) { BorderColor = borderColor });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("Checked:\nAS     2021.10.01", fs2, false)) { BorderColor = borderColor });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("Approved", fs2, false)) { BorderColor = borderColor });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("GEA Westfalia Separator India Pvt Ltd.", fs2, false)) { BorderColor = borderColor });

                    finalTable.WriteSelectedRows(0, -1, 35, 35, writer.DirectContent);
                }


            }

        }

        public partial class Header : PdfPageEventHelper
        {
            public override void OnEndPage(PdfWriter writer, Document doc)
            {
                GEAGenerateReport thiPage = new GEAGenerateReport();
                string reportName = GEAGenerateReport.reportNameForHeaderFooter;
                bool isHeaderFooterRequired = GEAGenerateReport.isHeaderFooterRequired;
                iTextSharp.text.BaseColor backColor = new iTextSharp.text.BaseColor(242, 242, 242);
                iTextSharp.text.BaseColor backColor1 = new iTextSharp.text.BaseColor(227, 227, 227);
                iTextSharp.text.BaseColor borderColor = backColor;
                int fs2 = 6;
                //footer
                PdfPTable finalTable = new PdfPTable(7);
                if (reportName.Equals("ProDecanter", StringComparison.OrdinalIgnoreCase))
                {
                    if (isHeaderFooterRequired == false)
                    {
                        return;
                    }
                    finalTable = new PdfPTable(3);
                    finalTable.TotalWidth = 550;
                    finalTable.SpacingBefore = 10;
                    int[] cellWidth = new int[3] { 150, 400, 100 };
                    finalTable.SetWidths(cellWidth);
                    byte[] file1;
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(getGEALogoPath()));//ImagePath  
                    iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(100f, 70f);
                    PdfPCell logoCell = new PdfPCell(geaLogo, false);
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    finalTable.AddCell(new PdfPCell(logoCell) { Rowspan = 2 });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("Assembly and Test process control for\nPRO Decanter\nPRO2200 5000 5500", 12, true)) { HorizontalAlignment = Element.ALIGN_CENTER, Rowspan = 2 });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("WSN", 12, true)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    finalTable.AddCell(new PdfPCell(getPdfCellWithText("95-5101-00", 12, false)) { HorizontalAlignment = Element.ALIGN_CENTER });

                    finalTable.WriteSelectedRows(0, -1, 20, doc.PageSize.Height - 15, writer.DirectContent);
                }

            }


        }
        private static PdfPTable getcheckBoxWithText(string text, string value, int fontSize)
        {
            PdfPTable pdfPTable = new PdfPTable(2);
            try
            {
                int[] tblCellWidth2 = { 50, 700 };
                pdfPTable.SetWidths(tblCellWidth2);
                pdfPTable.AddCell(new PdfPCell(getCheckUncheckImgCell(value)) { PaddingLeft = 3, Border = 0 });
                pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(text, fontSize, false)) { PaddingLeft = 5, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return pdfPTable;
        }
        private static PdfPTable getFirstSamplePdfPTable(string text1, string text2, int size, iTextSharp.text.BaseColor backColor)
        {
            PdfPTable pdfPTable = new PdfPTable(2);
            try
            {
                int[] tblCellWidth2 = { 300, 500 };
                pdfPTable.SetWidths(tblCellWidth2);
                pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(text1, size, false)) { BackgroundColor = backColor, Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                pdfPTable.AddCell(new PdfPCell(getPdfCellWithText(text2, size, false)) { Border = 0 });
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return pdfPTable;
        }
        private static PdfPCell getCheckUncheckImgCell(string value)
        {
            PdfPCell cell = null;
            if (value.Equals("1", StringComparison.OrdinalIgnoreCase) || value.Equals("OK", StringComparison.OrdinalIgnoreCase) || value.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                byte[] checkfile;
                checkfile = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/Check.png"));//ImagePath  
                iTextSharp.text.Image checkjpg = iTextSharp.text.Image.GetInstance(checkfile);
                checkjpg.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                checkjpg.ScaleToFit(10f, 10f);
                cell = new PdfPCell(checkjpg, false);
                cell.BorderWidth = 1;
                //cell.PaddingTop = paddingValue;
                //cell.PaddingBottom = paddingValue;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.Border = 0;
                // cell.BorderColor = getPdfCellBorderColor();
            }
            else
            {
                byte[] uncheckfile;
                uncheckfile = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/Uncheck.png"));//ImagePath  
                iTextSharp.text.Image uncheckjpg = iTextSharp.text.Image.GetInstance(uncheckfile);
                uncheckjpg.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                uncheckjpg.ScaleToFit(10f, 10f);
                cell = new PdfPCell(uncheckjpg, false);
                cell.BorderWidth = 1;
                //cell.PaddingTop = paddingValue;
                //cell.PaddingBottom = paddingValue;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.Border = 0;
            }
            return cell;
        }
        #endregion

        #region  ------ DC Report ------
        internal static string DCReport(string machineID, string productionorder, string partnumber, string grnNumber)
        {
            string ReportStatus = string.Empty;
            try
            {
                PdfPTable mainTbl = DCReportTbl(machineID, productionorder, partnumber, 20, grnNumber, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    Document pdfDoc = new Document(PageSize.A4, 15, 15, 25, 25);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();
                    pdfDoc.Add(mainTbl);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=DF-" + partnumber + "-" + DateTime.Now.ToString("ddMMyyyy") + ".pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }

        private static PdfPTable DCReportTbl(string machineID, string productionorder, string partnumber, int mainTblSpaceBefore, string grnNumber, out string reportStatus)
        {
            reportStatus = string.Empty;
            PdfPTable maintable = new PdfPTable(1);
            try
            {
                DataTable DCDataTbl = new DataTable();
                DCDataTbl = GEADatabaseAccess.GetDCReportData(machineID, productionorder, partnumber, grnNumber);
                string partname = GEADatabaseAccess.getPartName(partnumber);
                if (DCDataTbl.Rows.Count > 0)
                {
                    maintable.SplitLate = false;
                    maintable.WidthPercentage = 100;
                    maintable.SpacingBefore = mainTblSpaceBefore;
                    maintable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    maintable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    //header
                    int paddinghdrval = 8;
                    PdfPTable innertable = new PdfPTable(3);
                    int[] gridCellWidth = { 200, 500, 200 };
                    innertable.SetWidths(gridCellWidth);
                    innertable.SplitLate = false;

                    byte[] file1;
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(getGEALogoPath()));//ImagePath  
                    iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(50f, 50f);
                    PdfPCell logoCell = new PdfPCell(geaLogo, true);
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    innertable.AddCell(new PdfPCell(logoCell) { Border = PdfPCell.RIGHT_BORDER, PaddingTop = 10, PaddingBottom = 10, PaddingRight = 3 });
                    innertable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("REQUEST FOR DEVIATION", 12)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = PdfPCell.RIGHT_BORDER, Padding = paddinghdrval });
                    innertable.AddCell(new PdfPCell(getPdfCellWithText("Date: " + "  " + DCDataTbl.Rows[0]["Date"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddinghdrval, PaddingBottom = paddinghdrval, Border = 0 });
                    maintable.AddCell(innertable);

                    maintable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 10 });

                    //griddata
                    int paddingvalue = 6;
                    PdfPTable innertable1 = new PdfPTable(4);
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("Part Name:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(partname)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("Deviation Requested by:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(DCDataTbl.Rows[0]["DevRequestedBy"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("Part No.:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(DCDataTbl.Rows[0]["PartNo"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("Signature:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(DCDataTbl.Rows[0]["Signature"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("GRN No.:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(DCDataTbl.Rows[0]["GrnNo"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("Supplier:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(DCDataTbl.Rows[0]["Supplier"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("Job card No.:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(DCDataTbl.Rows[0]["JobCardNo"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("In-house:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(DCDataTbl.Rows[0]["Inhouse"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("Received Qty.:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(DCDataTbl.Rows[0]["ReceivedQty"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("Deviation Qty:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(DCDataTbl.Rows[0]["DeviationQty"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    maintable.AddCell(new PdfPCell(innertable1) { Border = 0 });

                    maintable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 10 });
                    //Non-conformance Details
                    maintable.AddCell(new PdfPCell(getPdfCellWithText("Non-conformance Details:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    PdfPTable innertable2 = new PdfPTable(2);
                    int[] gridCellWidth1 = { 100, 600 };
                    innertable2.SetWidths(gridCellWidth1);
                    innertable2.SplitLate = false;
                    innertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Deviation Type")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Rowspan = 2 });

                    PdfPTable chkboxinnertable = new PdfPTable(10);
                    int[] gridCellWidth6 = { 80, 30, 80, 30, 50, 30, 120, 30, 50, 30 };
                    chkboxinnertable.SetWidths(gridCellWidth6);
                    chkboxinnertable.SplitLate = false;
                    chkboxinnertable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Visual / Cosmetics")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                    chkboxinnertable.AddCell(new PdfPCell(getCheckUncheckImgCell(DCDataTbl.Rows[0]["Visual"].ToString())) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    chkboxinnertable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Dimensional")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                    chkboxinnertable.AddCell(new PdfPCell(getCheckUncheckImgCell(DCDataTbl.Rows[0]["Dimensional"].ToString())) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    chkboxinnertable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Material")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                    chkboxinnertable.AddCell(new PdfPCell(getCheckUncheckImgCell(DCDataTbl.Rows[0]["Material"].ToString())) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    chkboxinnertable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Functional / Performance")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                    chkboxinnertable.AddCell(new PdfPCell(getCheckUncheckImgCell(DCDataTbl.Rows[0]["Functional"].ToString())) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    chkboxinnertable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Process")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                    chkboxinnertable.AddCell(new PdfPCell(getCheckUncheckImgCell(DCDataTbl.Rows[0]["Process"].ToString())) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    innertable2.AddCell(new PdfPCell(chkboxinnertable) { Border = PdfPCell.RIGHT_BORDER });

                    PdfPTable chkboxinnertable2 = new PdfPTable(5);
                    int[] gridCellWidth7 = { 100, 70, 100, 70, 800 };
                    chkboxinnertable2.SetWidths(gridCellWidth7);
                    chkboxinnertable2.SplitLate = false;
                    chkboxinnertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Other")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                    chkboxinnertable2.AddCell(new PdfPCell(getCheckUncheckImgCell(DCDataTbl.Rows[0]["Other"].ToString())) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    chkboxinnertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Permanent")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                    chkboxinnertable2.AddCell(new PdfPCell(getCheckUncheckImgCell(DCDataTbl.Rows[0]["Permanent"].ToString())) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER });
                    chkboxinnertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                    innertable2.AddCell(new PdfPCell(chkboxinnertable2) { Border = PdfPCell.RIGHT_BORDER });

                    innertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Deviation Description")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText(DCDataTbl.Rows[0]["DeviationDescription"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Effect on Customer")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                    PdfPTable chkboxinnertable1 = new PdfPTable(10);
                    int[] gridCellWidth5 = { 80, 50, 80, 50, 80, 50, 80, 50, 80, 500 };
                    chkboxinnertable1.SetWidths(gridCellWidth5);
                    chkboxinnertable1.SplitLate = false;
                    chkboxinnertable1.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Negligible")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                    chkboxinnertable1.AddCell(new PdfPCell(getCheckUncheckImgCell(DCDataTbl.Rows[0]["Negligible"].ToString())) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    chkboxinnertable1.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Minor")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                    chkboxinnertable1.AddCell(new PdfPCell(getCheckUncheckImgCell(DCDataTbl.Rows[0]["Minor"].ToString())) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    chkboxinnertable1.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Moderate")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                    chkboxinnertable1.AddCell(new PdfPCell(getCheckUncheckImgCell(DCDataTbl.Rows[0]["Moderate"].ToString())) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    chkboxinnertable1.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Major")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                    chkboxinnertable1.AddCell(new PdfPCell(getCheckUncheckImgCell(DCDataTbl.Rows[0]["Major"].ToString())) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    chkboxinnertable1.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Severe")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                    chkboxinnertable1.AddCell(new PdfPCell(getCheckUncheckImgCell(DCDataTbl.Rows[0]["Severe"].ToString())) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    innertable2.AddCell(new PdfPCell(chkboxinnertable1) { Border = PdfPCell.RIGHT_BORDER });

                    innertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Effect Description")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText(DCDataTbl.Rows[0]["EffectDescription"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    maintable.AddCell(new PdfPCell(innertable2) { Border = 0 });

                    maintable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 10 });
                    //analysis
                    maintable.AddCell(new PdfPCell(getPdfCellWithText("Analysis / Action:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PdfPTable innertable3 = new PdfPTable(2);
                    int[] gridCellWidth2 = { 100, 600 };
                    innertable3.SetWidths(gridCellWidth2);
                    innertable3.SplitLate = false;
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Root Cause")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText(DCDataTbl.Rows[0]["RootCause"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Corrective Action")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText(DCDataTbl.Rows[0]["CorrectiveAction"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Preventive Action")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText(DCDataTbl.Rows[0]["PreventiveAction"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    maintable.AddCell(new PdfPCell(innertable3) { Border = 0 });

                    maintable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 10 });
                    //Approving Authority
                    maintable.AddCell(new PdfPCell(getPdfCellWithText("Approving Authority: QA Head / Technical Head:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PdfPTable innertable4 = new PdfPTable(2);
                    int[] gridcellwidth3 = { 200, 600 };
                    innertable4.SetWidths(gridcellwidth3);
                    innertable4.SplitLate = false;
                    innertable4.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Deviation is Approved")) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, Border = 0, PaddingBottom = paddingvalue });
                    innertable4.AddCell(new PdfPCell(getCheckUncheckImgCell(DCDataTbl.Rows[0]["DeviationApproved"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, Border = 0, PaddingBottom = paddingvalue });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Deviation is Not Approved")) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, Border = 0, PaddingBottom = paddingvalue });
                    innertable4.AddCell(new PdfPCell(getCheckUncheckImgCell(DCDataTbl.Rows[0]["DeviationNotApproved"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, Border = 0, PaddingBottom = paddingvalue });
                    maintable.AddCell(innertable4);

                    //tabledata
                    PdfPTable innertable5 = new PdfPTable(2);
                    innertable5.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Approved by QA Head")) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    innertable5.AddCell(new PdfPCell(getPdfCellWithoutBoldText(DCDataTbl.Rows[0]["QAHead"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    innertable5.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Approved by Technical Head")) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    innertable5.AddCell(new PdfPCell(getPdfCellWithoutBoldText(DCDataTbl.Rows[0]["TechnicalHead"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                    maintable.AddCell(new PdfPCell(innertable5) { Border = 0 });

                    //footer
                    PdfPTable innertable6 = new PdfPTable(1);
                    string date = string.IsNullOrEmpty(DCDataTbl.Rows[0]["SignDate"].ToString()) ? "" : Util.GetDateTime(DCDataTbl.Rows[0]["SignDate"].ToString()).ToString("dd-MM-yyyy");
                    innertable6.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { Border = 0, MinimumHeight = 10 });
                    innertable6.AddCell(new PdfPCell(getPdfCellWithoutBoldText(DCDataTbl.Rows[0]["QASign"].ToString() + " / " + date)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                    innertable6.AddCell(new PdfPCell(getPdfCellWithoutBoldText("QA Sign. / Date")) { PaddingRight = 30, Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT, PaddingBottom = paddingvalue });
                    maintable.AddCell(innertable6);
                    maintable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Format Control No.: F/QAD/12/R1  ")) { PaddingRight = 30, HorizontalAlignment = Element.ALIGN_LEFT });

                }
                else
                {
                    reportStatus = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                reportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
            }
            return maintable;
        }

        #endregion

        #region  ------ NCR Report ------
        internal static string NCReport(string machineID, string ProductionOrder, string Partnumber, string grnNumber)
        {
            string ReportStatus = string.Empty;
            try
            {
                PdfPTable mainTbl = NCRReportTbl(machineID, ProductionOrder, Partnumber, 20, grnNumber, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 15, 15, 25, 25);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();
                    pdfDoc.Add(mainTbl);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=NC-" + SlnoForReportGeneration + ".pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }
        internal static PdfPTable NCRReportTbl(string machineID, string ProductionOrder, string PartNumber, int mainTblSpaceBefore, string grnNumber, out string ReportStatus)
        {
            ReportStatus = string.Empty;
            PdfPTable mainTbl = new PdfPTable(1);
            try
            {
                DataTable NCRDataTbl = new DataTable();
                NCRDataTbl = GEADatabaseAccess.GetNCRReportData(machineID, ProductionOrder, PartNumber, grnNumber);
                if (NCRDataTbl.Rows.Count > 0)
                {
                    mainTbl.SplitLate = false;
                    mainTbl.WidthPercentage = 100;
                    mainTbl.SpacingBefore = mainTblSpaceBefore;
                    mainTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    mainTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    //header
                    int paddinghdrval = 8;
                    PdfPTable innertbl = new PdfPTable(3);
                    int[] gridCellWidth1 = { 150, 600, 150 };
                    innertbl.SetWidths(gridCellWidth1);

                    byte[] file1;
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(getGEALogoPath()));//ImagePath  
                    iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(50f, 50f);
                    PdfPCell logoCell = new PdfPCell(geaLogo, true);
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    innertbl.AddCell(new PdfPCell(logoCell) { Border = PdfPCell.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 10, PaddingBottom = 10, PaddingRight = 3 });
                    innertbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("NON-CONFORMANCE REPORT", 12)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = PdfPCell.RIGHT_BORDER, Padding = paddinghdrval });
                    innertbl.AddCell(new PdfPCell(getPdfCellWithText("NC.No. : " + NCRDataTbl.Rows[0]["NCNo"])) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0, PaddingTop = paddinghdrval, PaddingBottom = paddinghdrval });
                    SlnoForReportGeneration = NCRDataTbl.Rows[0]["NCNo"].ToString();
                    mainTbl.AddCell(innertbl);
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 10 });

                    int paddingvalue = 6;

                    //tabledata
                    PdfPTable innertable1 = new PdfPTable(4);
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("Part Name:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(NCRDataTbl.Rows[0]["PartName"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("Date:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(NCRDataTbl.Rows[0]["Date"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("Part No:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(NCRDataTbl.Rows[0]["PartNo"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("Supplier:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(NCRDataTbl.Rows[0]["Supplier"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("Batch No./ H.T No.:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(NCRDataTbl.Rows[0]["BatchNo"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("Received Qty.:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(NCRDataTbl.Rows[0]["ReceivedQty"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("GRN No.:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(NCRDataTbl.Rows[0]["GrnNo"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText("NC Qty:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithText(NCRDataTbl.Rows[0]["NCQty"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    mainTbl.AddCell(new PdfPCell(innertable1) { Border = 0 });

                    //Reasons
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Reason for Non-conformance:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PdfPTable innertable2 = new PdfPTable(2);
                    int[] gridCellWidth = { 50, 650 };
                    innertable2.SetWidths(gridCellWidth);
                    innertable2.SplitLate = false;
                    for (int i = 1; i <= 5; i++)
                    {
                        innertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText(i.ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        innertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText(NCRDataTbl.Rows[0]["Reason" + i].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });

                    }
                    mainTbl.AddCell(new PdfPCell(innertable2) { Border = 0 });
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 10 });

                    //images
                    innertable2 = new PdfPTable(1);
                    innertable2.SplitLate = false;
                    innertable2.AddCell(new PdfPCell(getPdfCellWithText("Details / Photos:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    innertable2.AddCell(new PdfPCell(getPdfCellWithText(NCRDataTbl.Rows[0]["Details"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 5, PaddingBottom = 5 });
                    innertable2.AddCell(new PdfPCell(getNCIqReportImages(machineID, "NC", ProductionOrder, PartNumber, grnNumber)));
                    mainTbl.AddCell(new PdfPCell(innertable2) { Border = 0 });

                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 10 });
                    //footercolumns
                    PdfPTable innertable3 = new PdfPTable(4);
                    innertable3.AddCell(new PdfPCell(getPdfCellWithText("Disposal")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Rowspan = 2 });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithText("Rework")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithText("Concessional Acceptance")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithText("Rejected")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithText(NCRDataTbl.Rows[0]["Rework"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithText(NCRDataTbl.Rows[0]["Acceptance"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithText(NCRDataTbl.Rows[0]["Rejected"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    mainTbl.AddCell(new PdfPCell(innertable3) { Border = 0 });

                    PdfPTable innertable4 = new PdfPTable(1);
                    string Qasign = NCRDataTbl.Rows[0]["QASign"].ToString();
                    string date = Util.GetDateTime(NCRDataTbl.Rows[0]["SignDate"].ToString()).ToString("dd-MM-yyyy");
                    innertable4.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 10, });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithText(Qasign + "  " + "/" + " " + date)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 0 });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithText("QA Sign." + "  " + "/" + " " + " Date")) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 0, PaddingRight = 30 });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithText("")) { MinimumHeight = 7 });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Format Control No.: F/QAD/07/R1  ")) { MinimumHeight = 10, HorizontalAlignment = Element.ALIGN_LEFT, PaddingLeft = 15 });
                    mainTbl.AddCell(new PdfPCell(innertable4));

                }
                else
                {
                    ReportStatus = "NoDataFound";
                }
            }

            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
            }
            return mainTbl;
        }
        private static PdfPTable getNCIqReportImages(string machineid, string reportName, string productionOrder, string partNumber, string grnNumber)
        {
            PdfPTable pdfPTable = new PdfPTable(2);
            pdfPTable.SplitLate = false;
            try
            {
                string reportPath = ConfigurationManager.AppSettings["GEAReportImagePath"].ToString();
                string folderName = partNumber + "_" + grnNumber;
                reportPath = Path.Combine(reportPath, machineid + "\\" + reportName + "\\" + productionOrder + "\\" + folderName);
                string[] FileArray = Directory.GetFiles(reportPath);
                List<int> imageNumberList = new List<int>();
                foreach (string file in FileArray)
                {
                    byte[] file1 = System.IO.File.ReadAllBytes(file);//ImagePath  
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(file1);
                    image.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    PdfPCell imageCell = new PdfPCell(image, true);
                    imageCell.VerticalAlignment = Element.ALIGN_TOP;
                    imageCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfPTable.AddCell(new PdfPCell(imageCell) { Padding = 2 });
                }
                if (FileArray.Length > 0)
                {
                    if ((FileArray.Length % 2) != 0)
                    {
                        pdfPTable.AddCell(new PdfPCell(getPdfCellWithText("")) { Padding = 2 });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return pdfPTable;
        }
        #endregion


        #region ------ IQ Report ------
        internal static string IQReport(string machineID, string ProductionOrder, string partnumber, string grnNumber)
        {
            string ReportStatus = string.Empty;
            try
            {
                PdfPTable mainTbl = IQReportTbl(machineID, ProductionOrder, partnumber, 20, grnNumber, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    Document pdfDoc = new Document(PageSize.A4, 15, 15, 25, 25);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();
                    pdfDoc.Add(mainTbl);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=IQR-" + SlnoForReportGeneration + ".pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }

        private static PdfPTable IQReportTbl(string machineID, string productionOrder, string partnumber, int mainTblSpaceBefore, string grnNumber, out string reportStatus)
        {
            reportStatus = string.Empty;
            PdfPTable maintable = new PdfPTable(1);
            try
            {
                iTextSharp.text.BaseColor backColor = new iTextSharp.text.BaseColor(255, 255, 153);
                DataTable IQDataTbl = new DataTable();
                IQDataTbl = GEADatabaseAccess.GetIQReportData(machineID, productionOrder, partnumber, grnNumber);
                if (IQDataTbl.Rows.Count > 0)
                {
                    maintable.SplitLate = false;
                    maintable.WidthPercentage = 100;
                    maintable.SpacingBefore = mainTblSpaceBefore;
                    maintable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    maintable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    //header
                    int paddinghdrval = 8;
                    PdfPTable innertable = new PdfPTable(3);
                    int[] gridCellWidth = { 200, 500, 200 };
                    innertable.SetWidths(gridCellWidth);
                    innertable.SplitLate = false;

                    byte[] file1;
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(getGEALogoPath()));//ImagePath  
                    iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(50f, 50f);
                    PdfPCell logoCell = new PdfPCell(geaLogo, false);
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    innertable.AddCell(new PdfPCell(logoCell) { Border = PdfPCell.RIGHT_BORDER, PaddingTop = 10, PaddingBottom = 10 });
                    innertable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Qualitätsbericht intern / Internal Quality Report", 12)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = PdfPCell.RIGHT_BORDER, Padding = paddinghdrval });
                    innertable.AddCell(new PdfPCell(getPdfCellWithText("Nr.No. :  " + IQDataTbl.Rows[0]["NrNo"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddinghdrval, PaddingBottom = paddinghdrval, Border = 0 });
                    SlnoForReportGeneration = IQDataTbl.Rows[0]["NrNo"].ToString();
                    maintable.AddCell(innertable);
                    maintable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 5 });

                    //masterdata
                    int paddingval = 6;
                    PdfPTable innertable1 = new PdfPTable(4);
                    innertable1.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Stammdaten / Master Data", 10)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Rowspan = 2 });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Benennung / Teil - description / part")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithoutBoldText(" Betriebsauftrag / production order")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Part Number")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithoutBoldText(IQDataTbl.Rows[0]["PartDescription"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithoutBoldText(IQDataTbl.Rows[0]["ProductionOrderNo"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable1.AddCell(new PdfPCell(getPdfCellWithoutBoldText(IQDataTbl.Rows[0]["PartNo"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    maintable.AddCell(new PdfPCell(innertable1) { Border = 0 });

                    //masterdata1
                    PdfPTable innertable2 = new PdfPTable(4);
                    int[] gridcellwidth = { 200, 200, 600, 400 };
                    innertable2.SetWidths(gridcellwidth);
                    innertable2.SplitLate = false;
                    innertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText("bel. Kostenst./charg.cost centre")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Mehrk.Urs. / extra costs")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Description of the issue")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Rohteil-Nr. / Mat.-Nr. / raw material/part no.")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText(IQDataTbl.Rows[0]["IssueDescription"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText(IQDataTbl.Rows[0]["RawMaterialPartNo"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    maintable.AddCell(new PdfPCell(innertable2) { Border = 0 });

                    PdfPTable innertable3 = new PdfPTable(10);
                    int[] gridcellwidth1 = { 40, 100, 100, 100, 100, 100, 30, 30, 30, 150 };
                    innertable3.SetWidths(gridcellwidth1);
                    innertable3.SplitLate = false;
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("ME/UN")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Losgröße / lot size")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("beanstandet / under claim")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("wieder verwendbar /reusable")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("bedingt verwendbar/ condition. reusable")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Ausschuß / scrap")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("MA /")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("BA /")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("AA /")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText(" Zeichnung  Nr. / drawing no.")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable3.AddCell(new PdfPCell(getCheckUncheckImgCell("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Border = PdfPCell.RIGHT_BORDER, BackgroundColor = backColor });
                    innertable3.AddCell(new PdfPCell(getCheckUncheckImgCell("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Border = PdfPCell.RIGHT_BORDER, BackgroundColor = backColor });
                    innertable3.AddCell(new PdfPCell(getCheckUncheckImgCell("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Border = 0, BackgroundColor = backColor });
                    innertable3.AddCell(new PdfPCell(getPdfCellWithoutBoldText(IQDataTbl.Rows[0]["DrawingNo"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    maintable.AddCell(new PdfPCell(innertable3) { Border = 0 });

                    PdfPTable innertable4 = new PdfPTable(7);
                    int[] gridcellwidth2 = { 150, 100, 150, 100, 150, 100, 100 };
                    innertable4.SetWidths(gridcellwidth2);
                    innertable4.SplitLate = false;
                    innertable4.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Abweichung festgestellt bei Arbg.Nr./fault detection at working group no.")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Kostenstelle / cost centre")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Abweichung entstand bei Arbeitsg.Nr. / fault caused at working group no.")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Kostenstelle / cost centre")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Masch.-Gruppen-Sch.-Nr. / machine group no.")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Personalnr. / Personnel - No.")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Werkstoff / material")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable4.AddCell(new PdfPCell(getPdfCellWithoutBoldText(IQDataTbl.Rows[0]["Material"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    maintable.AddCell(new PdfPCell(innertable4) { Border = 0 });

                    //masterdata2
                    PdfPTable innertable5 = new PdfPTable(5);
                    int[] gridcellwidth3 = { 250, 250, 30, 120, 150 };
                    innertable5.SetWidths(gridcellwidth3);
                    innertable5.SplitLate = false;
                    innertable5.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Eingeleitete Aktionen: / action initiated:")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable5.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Durchzuführen von: / To be carried out by:")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable5.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Einbaumeldung / ")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Colspan = 2 });
                    innertable5.AddCell(new PdfPCell(getPdfCellWithoutBoldText(" Charge-Nr. / batch no.")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });

                    innertable5.AddCell(new PdfPCell(getPdfCellWithoutBoldText(IQDataTbl.Rows[0]["LoginName"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Rowspan = 3 });
                    innertable5.AddCell(new PdfPCell(getPdfCellWithoutBoldText(IQDataTbl.Rows[0]["CarriedOutBy"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Rowspan = 3 });
                    innertable5.AddCell(new PdfPCell(getCheckUncheckImgCell("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Border = PdfPCell.RIGHT_BORDER, BackgroundColor = backColor });
                    innertable5.AddCell(new PdfPCell(getPdfCellWithoutBoldText("ja / yes")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable5.AddCell(new PdfPCell(getPdfCellWithoutBoldText(IQDataTbl.Rows[0]["BatchNo"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable5.AddCell(new PdfPCell(getPdfCellWithoutBoldText("an Konstr./to construct.")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Colspan = 2 });
                    innertable5.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Trommel Reihe/lfd Nr. / bowl series/serial no.")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Colspan = 2 });
                    innertable5.AddCell(new PdfPCell(getCheckUncheckImgCell("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Border = PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER, BackgroundColor = backColor });
                    innertable5.AddCell(new PdfPCell(getPdfCellWithoutBoldText("ja / yes")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable5.AddCell(new PdfPCell(getPdfCellWithoutBoldText(IQDataTbl.Rows[0]["BowlSeries"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    maintable.AddCell(new PdfPCell(innertable5) { Border = 0 });

                    //images
                    innertable2 = new PdfPTable(1);
                    innertable2.SplitLate = false;
                    innertable2.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Abweichungsbeschreibung / Stellungnahmen / Skizzen / Rückgabebegründung - description of fault / statements / sketches / reason of return ", 9)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
                    innertable2.AddCell(new PdfPCell(getPdfCellWithoutBoldText(IQDataTbl.Rows[0]["Reason"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 5, PaddingBottom = 10 });
                    innertable2.AddCell(new PdfPCell(getNCIqReportImages(machineID, "IQ", productionOrder, partnumber, grnNumber)));
                    maintable.AddCell(new PdfPCell(innertable2) { Border = 0 });

                    maintable.AddCell(new PdfPCell(getPdfCellWithBoldHeader(" Nacharbeit / rework", 9)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_CENTER });

                    //masterdata3
                    int paddingtblval = 1;
                    PdfPTable innertable6 = new PdfPTable(6);
                    int[] gridcellwidth4 = { 70, 200, 400, 100, 100, 100 };
                    innertable6.SetWidths(gridcellwidth4);
                    innertable6.SplitLate = false;
                    innertable6.AddCell(new PdfPCell(getPdfCellWithoutBoldText("lfd. Nr. / item")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable6.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Arb.-Platz / workplace")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable6.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Nacharbeitsbeschreibung / description of rework")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable6.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Zeit in Std / time in h ")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable6.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Geprüft Stamm-Nr. / Controlled staff no.")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable6.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Kosten / costs")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });

                    for (int i = 10; i <= 70; i += 10)
                    {
                        innertable6.AddCell(new PdfPCell(getPdfCellWithoutBoldText(i.ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingtblval, PaddingBottom = paddingtblval, BackgroundColor = backColor });
                        innertable6.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingtblval, PaddingBottom = paddingtblval, BackgroundColor = backColor });
                        innertable6.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingtblval, PaddingBottom = paddingtblval, BackgroundColor = backColor });
                        innertable6.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingtblval, PaddingBottom = paddingtblval, BackgroundColor = backColor });
                        innertable6.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingtblval, PaddingBottom = paddingtblval, BackgroundColor = backColor });
                        innertable6.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingtblval, PaddingBottom = paddingtblval, BackgroundColor = backColor });
                    }
                    maintable.AddCell(new PdfPCell(innertable6) { Border = 0 });

                    //masterdata4
                    PdfPTable innertable7 = new PdfPTable(6);
                    innertable7.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Aussteller: / issued by:")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable7.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Datum / date")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable7.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Abteilung / Kostenstelle department / cost centre")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable7.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Unterschrift / QA.signature")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable7.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Meister / Unterschrift senior / Prod.signature ")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable7.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Fertigungskontrolle / production control ")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable7.AddCell(new PdfPCell(getPdfCellWithoutBoldText(IQDataTbl.Rows[0]["IssuedBy"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    string datum = "";
                    if (IQDataTbl.Rows[0]["Date"].ToString() != "")
                    {
                        datum = Util.GetDateTime(IQDataTbl.Rows[0]["Date"].ToString()).ToString("dd-MM-yyyy");
                    }
                    innertable7.AddCell(new PdfPCell(getPdfCellWithoutBoldText(datum)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable7.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Production India")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable7.AddCell(new PdfPCell(getPdfCellWithoutBoldText(IQDataTbl.Rows[0]["QASignature"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable7.AddCell(new PdfPCell(getPdfCellWithoutBoldText(IQDataTbl.Rows[0]["HeadOfProdSignature"].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable7.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    maintable.AddCell(new PdfPCell(innertable7) { Border = 0 });

                    PdfPTable innertable8 = new PdfPTable(3);
                    int[] gridcellwidth5 = { 400, 200, 200 };
                    innertable8.SetWidths(gridcellwidth5);
                    innertable8.SplitLate = false;
                    innertable8.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Warenrückgabedaten: / Data of returned goods:", 10)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Rowspan = 2 });
                    innertable8.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Wareneingang Nr. / no. of goods receipt")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable8.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Bestell-Nr. / purchase order no.")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable8.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, MinimumHeight = 15, BackgroundColor = backColor });
                    innertable8.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, MinimumHeight = 15, BackgroundColor = backColor });
                    maintable.AddCell(new PdfPCell(innertable8) { Border = 0 });

                    //masterdata5
                    PdfPTable innertable9 = new PdfPTable(10);
                    int[] gridcellwidth6 = { 200, 100, 100, 100, 80, 20, 80, 20, 100, 100 };
                    innertable9.SetWidths(gridcellwidth6);
                    innertable9.SplitLate = false;
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Firma: / company:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Colspan = 7, BackgroundColor = backColor });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Lieferanten-Nr. / supplier no.")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });

                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Versandanschrift: / shipping address:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Colspan = 7, BackgroundColor = backColor });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Lieferanten-Kom.-Nr. / supplier's consignment no.")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });

                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Versandort: / place of despatch:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Bestimmungsort: / destination:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Colspan = 2 });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Colspan = 4, BackgroundColor = backColor });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Station: / station:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });

                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Versand-Datum: / despatch date:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("B-Anzeige: / debit note:")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("frei / prepaid")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER });
                    innertable9.AddCell(new PdfPCell(getCheckUncheckImgCell("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Border = 0, BackgroundColor = backColor });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("unfrei/ex works")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER });
                    innertable9.AddCell(new PdfPCell(getCheckUncheckImgCell("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Border = 0, BackgroundColor = backColor });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText(" Vers.-Kom. / shipment order no.")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, BackgroundColor = backColor });

                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Wareneingangskontrolle / incoming goods inspection")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Rowspan = 2 });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, MinimumHeight = 15, BackgroundColor = backColor });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, MinimumHeight = 15, BackgroundColor = backColor });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, MinimumHeight = 15, BackgroundColor = backColor });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, MinimumHeight = 15, Colspan = 2, BackgroundColor = backColor });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, MinimumHeight = 15, Colspan = 2, BackgroundColor = backColor });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, MinimumHeight = 15, BackgroundColor = backColor });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, MinimumHeight = 15, BackgroundColor = backColor });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Benennen und Speichern")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("WSE")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("WSN")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Colspan = 2 });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("QB")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval, Colspan = 2 });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Dispositon / material planning")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    innertable9.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Einkauf / purchasing")) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = paddingval, PaddingBottom = paddingval });
                    maintable.AddCell(new PdfPCell(innertable9) { Border = 0 });
                }
                else
                {
                    reportStatus = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                reportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
            }
            return maintable;
        }

        #endregion

        #region ---- Quality Test Protocol ------
        internal static string QualityTestProtocol(string machine, string productionOrder, string materialId, string operationNo, string planNo, string grnNumber)
        {
            string ReportStatus = string.Empty;
            try
            {
                string pdfFileName = productionOrder + "_" + materialId + "_" + grnNumber;
                reportNameForHeaderFooter = "QualityTestProtocol";
                isPagePotrait = false;
                string reportPath = ConfigurationManager.AppSettings["GEAReportImagePath"].ToString();
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 15, 15, 10, 50);
                try
                {
                    if (machine.Equals("Quality Incoming", StringComparison.OrdinalIgnoreCase))
                    {
                        reportPath = Path.Combine(reportPath, "QualityTestProtocol");
                        string[] pdfFileArray = Directory.GetFiles(reportPath);
                        int flag = 0;
                        string filename = pdfFileName.ToLower();
                        foreach (string pdffile in pdfFileArray)
                        {
                            string fileInLC = pdffile.ToLower();
                            if (fileInLC.Contains(filename + ".pdf"))
                            {
                                flag = 1;
                                break;
                            }
                        }
                        if (flag == 1)
                        {
                            isPagePotrait = true;
                            pdfDoc = new Document(PageSize.A4, 15, 15, 10, 50);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                pdfDoc.Open();
                isHeaderFooterRequired = true;
                pdfWriter.PageEvent = new HeaderFooter();

                PdfPTable mainTbl = QualityTestProtocolTbl(machine, productionOrder, materialId, operationNo, planNo, 0, pdfDoc, grnNumber, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    if (machine.Equals("Quality Incoming", StringComparison.OrdinalIgnoreCase))
                    {
                        isHeaderFooterRequired = false;
                        Paragraph p2 = new Paragraph();
                        pdfDoc.Add(p2);
                        pdfDoc.NewPage();
                        BindExistingPDFToFinalReport(pdfWriter, pdfDoc, reportPath, pdfFileName);
                    }
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=TP-" + materialId + "-" + productionOrder + ".pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
                else
                {
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }
        private static void BindExistingPDFToTestProtoColReport(PdfWriter pdfWriter, Document pdfDoc, string imgPdfPath, string filename)
        {
            try
            {
                string[] pdfFileArray = null;
                List<PdfReader> readerList = new List<PdfReader>();
                PdfContentByte cb = pdfWriter.DirectContent;
                pdfFileArray = Directory.GetFiles(imgPdfPath);
                foreach (string pdffile in pdfFileArray)
                {
                    string fileInLC = pdffile.ToLower();
                    filename = filename.ToLower();
                    if (fileInLC.Contains(filename + ".pdf"))
                    {
                        PdfReader pdfReader = new PdfReader(pdffile);
                        readerList.Add(pdfReader);
                    }
                }
                foreach (PdfReader reader in readerList)
                {
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = pdfWriter.GetImportedPage(reader, i);

                        iTextSharp.text.Rectangle currentPageRectangle = reader.GetPageSizeWithRotation(i);
                        if (currentPageRectangle.Width > currentPageRectangle.Height)
                        {
                            //page is landscape
                            //Paragraph p1 = new Paragraph();
                            //pdfDoc.Add(p1);
                            //pdfDoc.NewPage();
                            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(0, 0, PageSize.A4.Width, PageSize.A4.Height, 0));
                            pdfDoc.Add(iTextSharp.text.Image.GetInstance(page));
                        }
                        else
                        {
                            //page is portrait

                            cb.PdfDocument.NewPage();
                            PdfImportedPage page1 = pdfWriter.GetImportedPage(reader, i);


                            iTextSharp.text.Rectangle psize = reader.GetPageSizeWithRotation(i);
                            switch (psize.Rotation)
                            {
                                case 0:
                                    cb.AddTemplate(page1, 1f, 0, 0, 1f, 0, 0);
                                    break;
                                case 90:
                                    cb.AddTemplate(page1, 0, -1f, 1f, 0, 0, psize.Height);
                                    break;
                                case 180:
                                    cb.AddTemplate(page1, -1f, 0, 0, -1f, 0, 0);
                                    break;
                                case 270:
                                    cb.AddTemplate(page1, 0, 1.0F, -1.0F, 0, psize.Width, 0);
                                    break;
                                default:
                                    break;
                            }
                            pdfDoc.NewPage();
                        }

                        // if (page.Width > page.Height)
                        //  {
                        // pdfDoc.SetPageSize(PageSize.A4.Rotate());

                        // }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindExistingPDFToFinalReport = " + ex.Message);
            }
        }
        private static PdfPTable QualityTestProtocolTbl(string machine, string productionOrder, string materialId, string operationNo, string planNo, int mainTblSpaceBefore, Document pdfDoc, string grnNumber, out string reportStatus)
        {
            reportStatus = string.Empty;
            PdfPTable maintable = new PdfPTable(1);
            try
            {

                DataTable dataTbl1 = new DataTable();
                DataTable dataTbl2 = new DataTable();
                dataTbl1 = GEADatabaseAccess.GetQualityTestProtocol(machine, productionOrder, materialId, operationNo, planNo, grnNumber, out dataTbl2);
                string compDesc = GEADatabaseAccess.getPartName(materialId);
                string sampleQty = GEADatabaseAccess.getSampleQty(productionOrder, machine);
                if (dataTbl1.Rows.Count > 0)
                {
                    var firstDataRow = dataTbl1.Rows[0];

                    iTextSharp.text.BaseColor backColor = new iTextSharp.text.BaseColor(219, 219, 219);
                    iTextSharp.text.BaseColor borderColor = new iTextSharp.text.BaseColor(214, 214, 214);
                    iTextSharp.text.BaseColor borderColor2 = new iTextSharp.text.BaseColor(250, 250, 250);
                    int fs2 = 8;


                    int paddinghdrval = 0;
                    PdfPTable innertable = new PdfPTable(2);

                    //masterdata
                    List<string> sampleCountList = new List<string>();
                    List<string> dataTbl1ColumnNames = dataTbl1.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                    for (int i = 12; i < dataTbl1.Columns.Count; i++)
                    {
                        //string col = dataTbl1ColumnNames.Where(k => k == i.ToString()).FirstOrDefault();
                        //if (!string.IsNullOrEmpty(col))
                        //{
                        sampleCountList.Add(dataTbl1.Columns[i].ColumnName);
                        //}
                    }
                    bool textBold = false;
                    int noOfSampleToShowInEachPage = 5;
                    int tempsampleC = 0, sampleHCount = 0;
                    for (int sampleC = 0; sampleC < sampleCountList.Count; sampleC++)
                    {
                        if (sampleC == 0 || ((sampleC % noOfSampleToShowInEachPage) == 0))
                        {

                            maintable = new PdfPTable(1);
                            maintable.SplitLate = false;
                            maintable.WidthPercentage = 100;
                            maintable.SpacingBefore = mainTblSpaceBefore;
                            maintable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            maintable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                            //header
                            innertable = new PdfPTable(2);
                            int[] gridCellWidth = { 200, 600 };
                            innertable.SetWidths(gridCellWidth);
                            innertable.SplitLate = false;

                            byte[] file1;
                            file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(getGEALogoPath()));//ImagePath  
                            iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                            geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                            geaLogo.ScaleToFit(100f, 70f);
                            PdfPCell logoCell = new PdfPCell(geaLogo, false);
                            logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            innertable.AddCell(new PdfPCell(logoCell) { Border = 0, PaddingTop = 5, PaddingBottom = 5 });
                            innertable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Test Protocol", 12)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0, Padding = paddinghdrval, BackgroundColor = backColor });
                            innertable.AddCell(new PdfPCell(getPdfCellWithText("", fs2, textBold)) { Border = 0, MinimumHeight = 5, Colspan = 2 });
                            innertable.AddCell(new PdfPCell(getPdfCellWithText("GEA Company", fs2, textBold)) { BackgroundColor = backColor, BorderColor = borderColor2, PaddingTop = 2, PaddingBottom = 2 });
                            innertable.AddCell(new PdfPCell(getPdfCellWithText("", fs2, textBold)) { BorderColor = borderColor2 });
                            maintable.AddCell(new PdfPCell(innertable) { Border = 0 });
                            maintable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 5 });

                            //description details
                            innertable = new PdfPTable(4);
                            int[] gridCellWidth3 = { 100, 400, 100, 200 };
                            innertable.SetWidths(gridCellWidth3);
                            innertable.SplitLate = false;
                            innertable.AddCell(new PdfPCell(getPdfCellWithText("Description", fs2, textBold)) { BackgroundColor = backColor, BorderColor = borderColor2 });
                            innertable.AddCell(new PdfPCell(getPdfCellWithText(compDesc, fs2, textBold)) { BorderColor = borderColor });
                            innertable.AddCell(new PdfPCell(getPdfCellWithText("Test plan No.", fs2, textBold)) { BackgroundColor = backColor, BorderColor = borderColor2 });
                            innertable.AddCell(new PdfPCell(getPdfCellWithText(planNo, fs2, textBold)) { BorderColor = borderColor });

                            innertable.AddCell(new PdfPCell(getPdfCellWithText("Order-No.", fs2, textBold)) { BackgroundColor = backColor, BorderColor = borderColor2 });
                            PdfPTable innertable3 = new PdfPTable(3);
                            int[] gridCellWidth4 = { 200, 100, 200 };
                            innertable3.SetWidths(gridCellWidth4);
                            innertable3.AddCell(new PdfPCell(getPdfCellWithText(productionOrder, fs2, textBold)) { BorderColor = borderColor });
                            innertable3.AddCell(new PdfPCell(getPdfCellWithText("Part-No.", fs2, textBold)) { BackgroundColor = backColor, BorderColor = borderColor });
                            innertable3.AddCell(new PdfPCell(getPdfCellWithText(materialId, fs2, textBold)) { BorderColor = borderColor });
                            innertable.AddCell(new PdfPCell(innertable3) { Border = 0 });
                            innertable.AddCell(new PdfPCell(getPdfCellWithText("Drawing-No.", fs2, textBold)) { BackgroundColor = backColor, BorderColor = borderColor2 });
                            innertable.AddCell(new PdfPCell(getPdfCellWithText(materialId, fs2, textBold)) { BorderColor = borderColor });

                            innertable.AddCell(new PdfPCell(getPdfCellWithText("Text Area", fs2, textBold)) { BackgroundColor = backColor, BorderColor = borderColor2 });
                            innertable3 = new PdfPTable(2);
                            innertable3.AddCell(new PdfPCell(getcheckBoxWithText("Incoming goods", "1", 9)) { BorderColor = borderColor, Border = 0, BorderWidthBottom = 1 });
                            innertable3.AddCell(new PdfPCell(getcheckBoxWithText("Production", "0", 9)) { BorderColor = borderColor, Border = 0, BorderWidthBottom = 1 });
                            innertable.AddCell(new PdfPCell(innertable3) { Border = 0 });
                            innertable.AddCell(new PdfPCell(getPdfCellWithText("Sample Qty.", fs2, textBold)) { BackgroundColor = backColor, BorderColor = borderColor2 });
                            innertable.AddCell(new PdfPCell(getPdfCellWithText(sampleQty, fs2, textBold)) { BorderColor = borderColor });
                            maintable.AddCell(new PdfPCell(innertable) { Border = 0 });
                            maintable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 5 });

                            sampleHCount = noOfSampleToShowInEachPage;
                            tempsampleC = sampleC;
                            if ((sampleC + noOfSampleToShowInEachPage) > sampleCountList.Count)
                            {
                                sampleHCount = sampleCountList.Count - sampleC;
                            }

                            innertable = new PdfPTable(4 + sampleHCount);
                            int[] gridCellWidth6 = new int[4 + sampleHCount];
                            for (int i = 0; i < 4 + sampleHCount; i++)
                            {
                                if (i == 0)
                                {
                                    gridCellWidth6[i] = 50;
                                }
                                else if (i == 1)
                                {
                                    gridCellWidth6[i] = 300;
                                }
                                else if (i == 2)
                                {
                                    gridCellWidth6[i] = 100;
                                }
                                else if (i == (4 + sampleHCount - 1))
                                {
                                    gridCellWidth6[i] = 150;
                                }
                                else
                                {
                                    gridCellWidth6[i] = 70;
                                }
                            }
                            innertable.SetWidths(gridCellWidth6);
                            innertable.SplitLate = false;
                            innertable.AddCell(new PdfPCell(getPdfCellWithText("No.", fs2, textBold)) { VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = backColor, BorderColor = borderColor2, Rowspan = 2 });
                            innertable.AddCell(new PdfPCell(getPdfCellWithText("Inspection Characteristics", fs2, textBold)) { VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = backColor, BorderColor = borderColor2, Rowspan = 2 });
                            innertable.AddCell(new PdfPCell(getPdfCellWithText("Set value (with tolerance field)", fs2, textBold)) { VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = backColor, BorderColor = borderColor2, Rowspan = 2 });
                            innertable.AddCell(new PdfPCell(getPdfCellWithText("Is Value", fs2, textBold)) { VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = backColor, BorderColor = borderColor2, Colspan = sampleHCount, HorizontalAlignment = Element.ALIGN_CENTER });
                            innertable.AddCell(new PdfPCell(getPdfCellWithText("Comments", fs2, textBold)) { VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = backColor, BorderColor = borderColor2, Rowspan = 2 });
                            for (int k = sampleC; k < sampleC + sampleHCount; k++)
                            {
                                innertable.AddCell(new PdfPCell(getPdfCellWithText("Sample " + sampleCountList[k], fs2, textBold)) { VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = backColor, BorderColor = borderColor2 });
                            }

                            for (int i = 0; i < dataTbl1.Rows.Count; i++)
                            {
                                innertable.AddCell(new PdfPCell(getPdfCellWithText(dataTbl1.Rows[i]["Slno"].ToString(), fs2, textBold)) { BorderColor = borderColor });
                                innertable.AddCell(new PdfPCell(getPdfCellWithText(dataTbl1.Rows[i]["CharacteristicCode"].ToString(), fs2, textBold)) { BorderColor = borderColor });
                                innertable.AddCell(new PdfPCell(getPdfCellWithText(dataTbl1.Rows[i]["SetValue"].ToString(), fs2, textBold)) { BorderColor = borderColor });
                                for (int k = sampleC; k < sampleC + sampleHCount; k++)
                                {
                                    innertable.AddCell(new PdfPCell(getPdfCellWithText(dataTbl1.Rows[i][sampleCountList[k]].ToString(), fs2, textBold)) { BorderColor = borderColor });
                                }
                                innertable.AddCell(new PdfPCell(getPdfCellWithText(dataTbl1.Rows[i]["Remarks"].ToString(), fs2, textBold)) { BorderColor = borderColor });
                            }
                        }
                        if ((sampleC != 0 && (((sampleC + 1) % noOfSampleToShowInEachPage) == 0)) || (sampleC == sampleCountList.Count - 1))
                        {
                            maintable.AddCell(new PdfPCell(innertable) { Border = 0 }); //add sample details table
                            maintable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 5 });
                            if (dataTbl2.Rows.Count > 0)
                            {
                                innertable = new PdfPTable(3);
                                int[] gridCellWidth = { 250, 250, 400 };
                                innertable.SetWidths(gridCellWidth);
                                innertable.SplitLate = false;
                                innertable.AddCell(new PdfPCell(getcheckBoxWithText("Release", dataTbl2.Rows[0]["Release"].ToString(), 9)) { BorderColor = borderColor, Border = 0 });
                                innertable.AddCell(new PdfPCell(getcheckBoxWithText("Deviation permit with edition", dataTbl2.Rows[0]["DeviationWithRemarks"].ToString(), 9)) { BorderColor = borderColor, Border = 0 });
                                PdfPTable innertable1 = new PdfPTable(2);
                                int[] gridCellWidth1 = { 200, 600 };
                                innertable1.SetWidths(gridCellWidth1);
                                innertable1.SplitLate = false;
                                innertable1.AddCell(new PdfPCell(getPdfCellWithText("Editions:", fs2, textBold)) { BackgroundColor = backColor, BorderColor = borderColor });
                                innertable1.AddCell(new PdfPCell(getPdfCellWithText(dataTbl2.Rows[0]["Remarks"].ToString(), fs2, textBold)) { BorderColor = borderColor });
                                innertable.AddCell(new PdfPCell(innertable1) { Border = 0 });
                                innertable.AddCell(new PdfPCell(getcheckBoxWithText("Deviation permit without edition", dataTbl2.Rows[0]["DeviationWithOutRemarks"].ToString(), 9)) { Border = 0 });
                                innertable.AddCell(new PdfPCell(getcheckBoxWithText("Blocked", dataTbl2.Rows[0]["Blocked"].ToString(), 9)) { Border = 0 });
                                innertable1 = new PdfPTable(2);
                                int[] gridCellWidth2 = { 200, 600 };
                                innertable1.SetWidths(gridCellWidth2);
                                innertable1.SplitLate = false;
                                innertable1.AddCell(new PdfPCell(getPdfCellWithText("Reason for blocking:", fs2, textBold)) { BackgroundColor = backColor, BorderColor = borderColor });
                                innertable1.AddCell(new PdfPCell(getPdfCellWithText(dataTbl2.Rows[0]["Reason"].ToString(), fs2, textBold)) { BorderColor = borderColor });
                                innertable.AddCell(new PdfPCell(innertable1) { Border = 0 });
                                maintable.AddCell(new PdfPCell(innertable) { Border = 0 });
                                maintable.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 10 });

                                innertable = new PdfPTable(7);
                                int[] gridCellWidth5 = { 50, 100, 100, 100, 100, 100, 100 };
                                innertable.SetWidths(gridCellWidth5);
                                innertable.SplitLate = false;
                                innertable.AddCell(new PdfPCell(getPdfCellWithText("Examiner", fs2, textBold)) { Border = 0 });
                                innertable.AddCell(new PdfPCell(getPdfCellWithText(firstDataRow["InspectedBy"].ToString(), fs2, textBold)) { Border = 0, BorderWidthBottom = 1, BorderColorBottom = borderColor });
                                innertable.AddCell(new PdfPCell(getPdfCellWithText("Date", fs2, textBold)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                                innertable.AddCell(new PdfPCell(getPdfCellWithText(string.IsNullOrEmpty(firstDataRow["InspectedTS"].ToString()) ? "" : Util.GetDateTime(firstDataRow["InspectedTS"].ToString()).ToString("dd-MM-yyyy"), fs2, textBold)) { Border = 0, BorderWidthBottom = 1, BorderColorBottom = borderColor });
                                innertable.AddCell(new PdfPCell(getPdfCellWithText("Signature", fs2, textBold)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                                innertable.AddCell(new PdfPCell(getPdfCellWithText(firstDataRow["InspectedBy"].ToString(), fs2, textBold)) { Border = 0, BorderWidthBottom = 1, BorderColorBottom = borderColor });
                                innertable.AddCell(new PdfPCell(getPdfCellWithText("", fs2, textBold)) { Border = 0 });
                                maintable.AddCell(new PdfPCell(innertable) { Border = 0 });
                            }
                            pdfDoc.Add(getFinalReportParagraph(maintable));
                            Paragraph p2 = new Paragraph();
                            pdfDoc.Add(p2);
                            pdfDoc.NewPage();
                        }
                    }


                }
                else
                {
                    reportStatus = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                reportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
            }
            return maintable;
        }
        #endregion
        #region ------ CE Checklist Report ------
        internal static string CEChecklistReport(string ProductionOrder, string fabricationNumber)
        {
            string ReportStatus = string.Empty;
            try
            {
                Document pdfDoc = new Document(PageSize.A4, 15, 15, 25, 25);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                pdfDoc.Open();
                PdfPTable mainTbl = CEChecklistReportTbl(pdfDoc, ProductionOrder, fabricationNumber, 20, false, out ReportStatus);
                if (ReportStatus == string.Empty)
                {
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=CEChecklist_" + fabricationNumber + ".pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
            }
            return ReportStatus;
        }
        internal static PdfPTable CEChecklistReportTbl(Document pdfDoc, string ProductionOrder, string fabricationNumber, int mainTblSpaceBefore, bool isFromFinalReport, out string ReportStatus)
        {
            ReportStatus = string.Empty;
            PdfPTable mainTbl = new PdfPTable(6);
            try
            {

                DataTable dtSchedule = new DataTable();
                DataTable dtData = new DataTable();
                dtData = GEADatabaseAccess.getCEChecklistReportTblData(ProductionOrder, fabricationNumber, out dtSchedule);
                if (dtSchedule.Rows.Count > 0 && dtData.Rows.Count > 0)
                {
                    string machineType = GEADatabaseAccess.getMachineTypeForPO(ProductionOrder, fabricationNumber);
                    mainTbl.SplitLate = false;
                    mainTbl.WidthPercentage = 100;
                    mainTbl.SpacingBefore = mainTblSpaceBefore;
                    mainTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    mainTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    int[] cellWidth1 = new int[6] { 25, 400, 30, 30, 30, 120 };
                    mainTbl.SetWidths(cellWidth1);
                    int paddingvalue = 2;
                    int fs1 = 10, fs2 = 8, fs3 = 8;
                    iTextSharp.text.BaseColor borderColor1 = new iTextSharp.text.BaseColor(219, 219, 219);
                    iTextSharp.text.BaseColor borderColor2 = new iTextSharp.text.BaseColor(0, 0, 0);
                    iTextSharp.text.BaseColor backColorSilver = new iTextSharp.text.BaseColor(230, 230, 230);

                    DataRow scheduleRow = dtSchedule.Rows[0];
                    //header
                    PdfPTable pdfTable = new PdfPTable(1);
                    pdfTable.WidthPercentage = 100;
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("CE - Checkliste für Einzelmaschinen", fs1, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("EC-checklist for single machines", fs1, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("(z.B. Separatoren, Dekanter, Pumpen, Mischer, Drehbürstensiebe)", 8, false)) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0, Padding = 0 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("(for example separators, decanter, pumps, mixer, rotary brushes sieve)", 8, false)) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0, Padding = 0 });
                    mainTbl.AddCell(new PdfPCell(pdfTable) { Colspan = 2 });

                    pdfTable = new PdfPTable(1);
                    byte[] file1;
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(getGEALogoPath()));//ImagePath  
                    iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(70f, 50f);
                    PdfPCell logoCell = new PdfPCell(geaLogo, false);
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfTable.AddCell(new PdfPCell(logoCell) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_RIGHT, PaddingRight = 5 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("GEA Mechanical Equipment", fs2, false)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 0 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("GEA Westfalia Separator GmbH", fs2, false)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 0 });
                    mainTbl.AddCell(new PdfPCell(pdfTable) { Colspan = 4 });

                    pdfTable = new PdfPTable(2);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Auftrags-Nr. / order number:\n\n" + scheduleRow["SaleOrder"].ToString(), fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthBottom = 1, BorderColorBottom = borderColor1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Masch.- Typ / machine type:\n\n" + machineType, fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthBottom = 1, BorderColorBottom = borderColor1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Maschinen-Nr. / serial number:\n\n" + scheduleRow["FabricationNo"].ToString(), fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthBottom = 1, BorderColorBottom = borderColor1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Kunde / customer: \n\n" + scheduleRow["Customer"].ToString(), fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthBottom = 1, BorderColorBottom = borderColor1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Inverkehrbringer / distributor:(First ordering company) \n\nGEA WS Group Germany", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthBottom = 1, BorderColorBottom = borderColor1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Best.-land / ordering country,final destination:  \n\n" + scheduleRow["Location"].ToString(), fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthBottom = 1, BorderColorBottom = borderColor1 });
                    mainTbl.AddCell(new PdfPCell(pdfTable) { Colspan = 2 });


                    string coordinator = "";
                    DataRow parameterData = dtData.AsEnumerable().Where(k => k.Field<string>("Parameter") == "0").FirstOrDefault();
                    if (parameterData != null)
                    {

                        coordinator = parameterData["Coordinator"].ToString();
                    }
                    string date = "", name = "";
                    var latestRow = dtData.AsEnumerable().OrderByDescending(k => k.Field<DateTime>("UpdatedTS")).FirstOrDefault();
                    if (latestRow != null)
                    {
                        date = string.IsNullOrEmpty(latestRow["UpdatedTS"].ToString()) ? "" : Util.GetDateTime(latestRow["UpdatedTS"].ToString()).ToString("dd-MM-yyyy");
                        name = latestRow["UpdatedBy"].ToString();
                    }
                    pdfTable = new PdfPTable(4);
                    cellWidth1 = new int[4] { 30, 30, 30, 120 };
                    pdfTable.SetWidths(cellWidth1);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("SB v. BL : " + coordinator + "\n\nCoordinator of  BL", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Status", fs2, false)) { Colspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Erledigung / execution", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, HorizontalAlignment = Element.ALIGN_CENTER });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("NR", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("J", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("N", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, BackgroundColor = backColorSilver });
                    //pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Datum\n" + date + "\n\nName\n" + name, fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Datum         Name", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, HorizontalAlignment = Element.ALIGN_CENTER });
                    mainTbl.AddCell(new PdfPCell(pdfTable) { Colspan = 4 });

                    int rowSpan = 1;
                    float borderwidth = 0.5f;
                    parameterData = dtData.AsEnumerable().Where(k => k.Field<string>("Parameter") == "1.0").FirstOrDefault();
                    //1.0
                    if (parameterData != null)
                    {
                        rowSpan = 3;
                        var rowValue = JObject.Parse(parameterData["CE_value"].ToString());
                        string data1 = rowValue["PED"].ToString();
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("1.0", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeaderUnderLine("anzuwendende Richtlinien / specification to be applied", fs2)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthLeft = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthLeft = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthLeft = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BackgroundColor = backColorSilver, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Abnahme / acceptance", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BackgroundColor = backColorSilver });

                        //1.1
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("1.1", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        pdfTable = new PdfPTable(2);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Zusätzlich zur EG-MRL gilt / additional to the EC machinery directive is valid", fs3, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText("DGRL/pressure equipment directive (PED)", rowValue["PED"].ToString(), fs2)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText("ATEX/ ATEX-directive", rowValue["ATEX"].ToString(), fs2)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText("NEC500-directive", rowValue["NEC500"].ToString(), fs2)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthLeft = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });

                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText(rowValue["UpdatedBy"].ToString() + "\n" + getCECheckListDate(rowValue["UpdatedTS_String"].ToString()), fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderColorBottom = borderColor1, Rowspan = rowSpan, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER });

                        //1.2
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("1.2", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        pdfTable = new PdfPTable(2);
                        cellWidth1 = new int[2] { 300, 200 };
                        pdfTable.SetWidths(cellWidth1);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Techn. Dokumentation zur Konformität liegt vor / \ntechn. documentation on the declaration of conformity exists", fs3, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText("Techn.Dokumentation zur Konformität angefordert bei: ", rowValue["Document_Of_Decleration"].ToString(), fs2)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Document_Of_Decleration_1_2_1"].ToString(), fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthBottom = 1 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("techn. documentation on the declaration of conformity requested with", fs3, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Document_Of_Decleration_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Document_Of_Decleration_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });

                        //1.3
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("1.3", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("CE-Zeichen auf Typenschild / CE-mark on the name plate is necessary", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["CE_mark_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["CE_mark_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });
                    }


                    //2.0
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("2.0", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeaderUnderLine("Maschinenausführung / machine design", fs2)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthLeft = 0, BackgroundColor = backColorSilver });
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthLeft = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BackgroundColor = backColorSilver });
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Montage / assembly", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BackgroundColor = backColorSilver });

                    //2.1
                    parameterData = dtData.AsEnumerable().Where(k => k.Field<string>("Parameter") == "2.1").FirstOrDefault();
                    if (parameterData != null)
                    {
                        rowSpan = 5;
                        var rowValue = JObject.Parse(parameterData["CE_value"].ToString());
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("2.1", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        pdfTable = new PdfPTable(2);
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("allgemeine Sicherheitsanforderungen werden eingehalten general safety requirements", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthLeft = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BackgroundColor = backColorSilver, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText(rowValue["GeneralSafetyRequirement_Assembly"].ToString() + "\n" + getCECheckListDate(rowValue["UpdatedTS_String"].ToString()), fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderColorBottom = borderColor1, Rowspan = rowSpan, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER });

                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("       - Maschinenteile ohne scharfe Kanten-Grat / machine parts without sharp edges - burs", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["GeneralSafetyRequirement_A_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["GeneralSafetyRequirement_A_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });

                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("       - Berührungsschutz vor drehenden Teilen nach DIN EN ISO 13857 / protection against contact with rotating parts acc.to DIN EN ISO 13857", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["GeneralSafetyRequirement_B_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["GeneralSafetyRequirement_B_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });

                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("       - Schutzeinrichtungen / protective devices\n\n              - nicht einfach zu umgehen / not easy to bypass", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["GeneralSafetyRequirement_C_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["GeneralSafetyRequirement_C_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });

                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("               - nur mit Werkzeug zu lösen / can only be dismantled with a tool", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["GeneralSafetyRequirement_D_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["GeneralSafetyRequirement_D_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });

                    }
                    //2.2
                    parameterData = dtData.AsEnumerable().Where(k => k.Field<string>("Parameter") == "2.2").FirstOrDefault();
                    if (parameterData != null)
                    {
                        var rowValue = JObject.Parse(parameterData["CE_value"].ToString());

                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("      - Daten Geräuschemission überprüft / data of the noise emission checked:", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Prüfstand / test bench", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderColorBottom = borderColor1, BackgroundColor = backColorSilver });

                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        pdfTable = new PdfPTable(7);
                        cellWidth1 = new int[7] { 30, 10, 100, 30, 10, 100, 30 };
                        pdfTable.SetWidths(cellWidth1);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithSupSubScript("L", "PA", true)) { HorizontalAlignment = Element.ALIGN_RIGHT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("=", fs2, false)) { HorizontalAlignment = Element.ALIGN_RIGHT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["NoiseEmissionChecked_LPA"].ToString(), fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthBottom = 1 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(" dBA", fs2, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, Colspan = 3 });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithSupSubScript("L", "WA", true)) { HorizontalAlignment = Element.ALIGN_RIGHT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("=", fs2, false)) { HorizontalAlignment = Element.ALIGN_RIGHT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["NoiseEmissionChecked_LWA_A"].ToString(), fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthBottom = 1 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(" dBA", fs2, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("=", fs2, false)) { HorizontalAlignment = Element.ALIGN_RIGHT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["NoiseEmissionChecked_LWA_B"].ToString(), fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthBottom = 1 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(" mW", fs2, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["NoiseEmissionChecked_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["NoiseEmissionChecked_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText(rowValue["NoiseEmissionChecked"].ToString() + "\n" + getCECheckListDate(rowValue["UpdatedTS_String"].ToString()), fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderColorBottom = borderColor1, Rowspan = 4, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER });


                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("2.2", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Notwendige Zusatzeinrichtungen / necessary auxiliary equipment", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });


                        //2.2.1
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("2.2.1", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Bei Maschinen in Ex-Ausführung (ATEX/NEC500) / ATEX-/NEC500-machines\n => siehe ATEX/NEC500-Checkliste / look at the ATEX/NEC500-checklist", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });

                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("- Sind die Anforderungen nach ATEX/NEC500 erfüllt? /\n Are ATEX/NEC500-requests fulfilled?", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["MachineDesign_2_2_1_NR"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["MachineDesign_2_2_1_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["MachineDesign_2_2_1_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });

                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Prüfer Elektro / electrical tester", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderColorBottom = borderColor1, BackgroundColor = backColorSilver });
                    }

                    //2.2.2
                    parameterData = dtData.AsEnumerable().Where(k => k.Field<string>("Parameter") == "2.3").FirstOrDefault();
                    if (parameterData != null)
                    {
                        var rowValue = JObject.Parse(parameterData["CE_value"].ToString());
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("2.2.2", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Potentialausgleichschraube mit symbolischer Kennzeichnung equipotential screw with symbolic mark", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["SymbolicMark_2_2_2_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["SymbolicMark_2_2_2_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText(rowValue["SymbolicMarkTesterChecked"].ToString() + "\n" + getCECheckListDate(rowValue["UpdatedTS_String"].ToString()), fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderColorBottom = borderColor1, Rowspan = 4, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER });

                        //2.2.3
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("2.2.3", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Elektrotechn. Maschinenausrüstung gemäß E-Abnahmeprotokoll i.O. Electronical equipment on the machine in acc.with the acceptance report o.k", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["ElectronicalEquipment_2_2_3_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["ElectronicalEquipment_2_2_3_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });

                        //2.2.4
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("2.2.4", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Steuerungselemente (Separatoren u. Dekanter) bestellt/vorhanden und  CE - konform\n Parts of control (separators and decanter) ordered / present and EC-compliant", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["EC_Compliant_2_2_4_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["EC_Compliant_2_2_4_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });

                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        pdfTable = new PdfPTable(2);
                        cellWidth1 = new int[2] { 400, 20 };
                        pdfTable.SetWidths(cellWidth1);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("- Anlaufsteuerung (MCC) vorhanden  /  start up control is present", fs2, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["StartupControl_2_2_4_1"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, PaddingRight = 7, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("- Steuerschrank (PLC) vorhanden  /  control cabinet is present", fs2, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["ControlCabinet_2_2_4_2"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, PaddingRight = 7, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("- Kompaktst. (MCC+PLC integriert) vorhanden / compact control cabinet  present", fs2, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["CompactControlCabinet_2_2_4_3"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, PaddingRight = 7, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("- Nur Einzelteile (E-Pläne, FU, auf Montageplatte, …) oder nichts bestellt Only components parts were ordered (electr. layout, frequency converter, on assembly plate, ….)	or nothing	", fs2, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["ComponentParts_2_2_4_4"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, PaddingRight = 7, Border = 0, });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });
                    }

                    //footer
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, MinimumHeight = 15, Border = 0, Colspan = 6 });
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("NR =nicht relevant / not relevant                              J = ja / yes                            N = nein / no                               132202-OR-DE/EN-01.2017-000", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, MinimumHeight = 15, Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                    if (isFromFinalReport)
                    {
                        pdfDoc.Add(getFinalReportParagraph(mainTbl));
                    }
                    else
                    {
                        pdfDoc.Add(mainTbl);
                    }
                    Paragraph p2 = new Paragraph();
                    pdfDoc.Add(p2);
                    pdfDoc.NewPage();

                    mainTbl = new PdfPTable(6);
                    mainTbl.SplitLate = false;
                    mainTbl.WidthPercentage = 100;
                    mainTbl.SpacingBefore = mainTblSpaceBefore;
                    mainTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    mainTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cellWidth1 = new int[6] { 25, 400, 30, 30, 30, 120 };
                    mainTbl.SetWidths(cellWidth1);
                    //header
                    pdfTable = new PdfPTable(1);
                    pdfTable.WidthPercentage = 100;
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("CE - Checkliste für Einzelmaschinen", fs1, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("EC-checklist for single machines", fs1, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("(z.B. Separatoren, Dekanter, Pumpen, Mischer, Drehbürstensiebe)", 8, false)) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0, Padding = 0 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("(for example separators, decanter, pumps, mixer, rotary brushes sieve)", 8, false)) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0, Padding = 0 });
                    mainTbl.AddCell(new PdfPCell(pdfTable) { Colspan = 2 });

                    pdfTable = new PdfPTable(1);
                    pdfTable.AddCell(new PdfPCell(logoCell) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, PaddingRight = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("GEA Mechanical Equipment", fs2, false)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 0 });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("GEA Westfalia Separator GmbH", fs2, false)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 0 });
                    mainTbl.AddCell(new PdfPCell(pdfTable) { Colspan = 4 });

                    //3.0
                    parameterData = dtData.AsEnumerable().Where(k => k.Field<string>("Parameter") == "3.0").FirstOrDefault();
                    if (parameterData != null)
                    {
                        var rowValue = JObject.Parse(parameterData["CE_value"].ToString());
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("3.0", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeaderUnderLine("Kennzeichnung / marking", fs2)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthLeft = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthLeft = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Prüfstand / test bench", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BackgroundColor = backColorSilver });

                        //3.1
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("3.1", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Maschine vollst. mit Typenschild gem. 	 MRL, Zentrifugennorm\nmachine compl. with name plate acc. EC machinery directive, centrifuge standard", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Marking_3_1_Centrifuge_Standard_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Marking_3_1_Centrifuge_Standard_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText(rowValue["UpdatedBy"].ToString() + "\n" + getCECheckListDate(rowValue["UpdatedTS_String"].ToString()), fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderColorBottom = borderColor1, Rowspan = 5, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER });


                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("           Zusatzkennzeichnung gem	.	- DGRL 	(Kesselschild)\n           Add. marking acc. pressure equipment directive (PED)", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Marking_3_1_Centrifuge_PED_NR"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Marking_3_1_Centrifuge_PED_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Marking_3_1_Centrifuge_PED_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });


                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        pdfTable = new PdfPTable(2);
                        cellWidth1 = new int[2] { 350, 100 };
                        pdfTable.SetWidths(cellWidth1);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("              Zusatzkennzeichnung gem	.	- ATEX", fs3, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, Colspan = 2 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("             Add. marking acc. 				                                            Atex directive", fs3, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Marking_3_1_ATEX_Directive"].ToString(), fs3, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthBottom = 1 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(" (vom Typenschild übernehmen /has to be taken from the name plate)", fs3, false)) { HorizontalAlignment = Element.ALIGN_RIGHT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, Colspan = 2 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Marking_3_1_Centrifuge_ATEX_NR"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Marking_3_1_Centrifuge_ATEX_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Marking_3_1_Centrifuge_ATEX_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });

                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("-Satz Schilder mit Sicherheitsaufkleber (Sep., Dek.) / \nset of plates with safety stickers (sep., dec.)", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Marking_3_1_Centrifuge_Safety_Sticker_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Marking_3_1_Centrifuge_Safety_Sticker_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });

                        //3.2
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("3.2", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        pdfTable = new PdfPTable(4);
                        cellWidth1 = new int[4] { 120, 50, 50, 130 };
                        pdfTable.SetWidths(cellWidth1);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Sprache Sicherheitsschilder / language of safety stickers", fs3, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, Colspan = 3 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Marketing_3_2_Safety_Stickers"].ToString(), fs3, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthBottom = 1 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Sprache des Typenschildes in", fs3, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Marketing_3_2_Language_1"].ToString(), fs3, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthBottom = 1, Colspan = 2 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("und CE-Zeichen auf Typenschild /", fs3, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Language of the name plate is", fs3, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Marketing_3_2_Language_2"].ToString(), fs3, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthBottom = 1, Colspan = 2 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("and the CE-symbol is on the name plate", fs3, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs3, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, Colspan = 1 });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText(" ja/yes", rowValue["Marketing_3_2_Language_Yes"].ToString(), fs3)) { HorizontalAlignment = Element.ALIGN_RIGHT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, PaddingRight = 10, Colspan = 2 });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText(" nein/no", rowValue["Marketing_3_2_Language_No"].ToString(), fs3)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });
                    }
                    //4.0
                    parameterData = dtData.AsEnumerable().Where(k => k.Field<string>("Parameter") == "4.0").FirstOrDefault();
                    if (parameterData != null)
                    {
                        var rowValue = JObject.Parse(parameterData["CE_value"].ToString());
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("4.0", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeaderUnderLine("Maschinenausführung / machine construction", fs2)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthLeft = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthLeft = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Packerei / packing area", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BackgroundColor = backColorSilver });

                        //4.1
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("4.1", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Maschine komplett und verwendungsfertig / machine complete and ready for use", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Machine_Construction_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Machine_Construction_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Machine_Construction_Packing_Area"].ToString() + "\n" + getCECheckListDate(rowValue["UpdatedTS_String"].ToString()), fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderColorBottom = borderColor1, Rowspan = 3, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER });

                        //4.2
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("4.2", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        pdfTable = new PdfPTable(4);
                        cellWidth1 = new int[4] { 150, 250, 150, 200 };
                        pdfTable.SetWidths(cellWidth1);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Werkzeuge / tools", fs3, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, Colspan = 4 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("gehören zum Gesamtauftrag auch Werkzeuge für jeden Maschinentyp?", fs3, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, Colspan = 4 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Are there tools for each machine type included in the whole order?", fs3, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, Colspan = 4 });

                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText("  ja/yes             (", rowValue["Tools_4_2_YES"].ToString(), fs3)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("sind vollst./are they complete :", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText("  ja/yes", rowValue["Tools_4_2_YES_Are_They_Complete_Yes"].ToString(), fs3)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText("  nein/no)", rowValue["Tools_4_2_YES_Are_They_Complete_NO"].ToString(), fs3)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });

                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText("  nein/no           (", rowValue["Tools_4_2_NO"].ToString(), fs3)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText("nicht bei dieser HP geprüft, kein Werkzeug aufgeführt/\nnot checked at this item, there no tool is listed)", rowValue["Tools_4_2_NO_NO_TOOL"].ToString(), fs3)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, Colspan = 3 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });

                        //4.3
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("4.3", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Lastaufnahmemittel mit CE-Kennzeichnung  /  lifting device with CE-mark", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Machine_Construction_4_3_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Machine_Construction_4_3_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });

                    }
                    //5.0
                    parameterData = dtData.AsEnumerable().Where(k => k.Field<string>("Parameter") == "5.0").FirstOrDefault();
                    if (parameterData != null)
                    {
                        var rowValue = JObject.Parse(parameterData["CE_value"].ToString());
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("5.0", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeaderUnderLine("Betriebsanleitung (BA) Maschine / Instruction manual", fs2)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthLeft = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthLeft = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Packerei / packing area", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BackgroundColor = backColorSilver });

                        //5.1
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("5.1", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        pdfTable = new PdfPTable(4);
                        cellWidth1 = new int[4] { 200, 60, 20, 20 };
                        pdfTable.SetWidths(cellWidth1);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Für Maschine als Original vorhanden/original edition for the machine available", fs3, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, Colspan = 4 });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Sprache/language:", fs3, false)) { HorizontalAlignment = Element.ALIGN_RIGHT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Instruction_Manual_Language"].ToString(), fs3, false)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthBottom = 1 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("N", fs3, false)) { HorizontalAlignment = Element.ALIGN_RIGHT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Instruction_Manual_Language_N_N"].ToString())) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Instruction_Manual_Language_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Instruction_Manual_Packing_Area"].ToString() + "\n" + getCECheckListDate(rowValue["UpdatedTS_String"].ToString()), fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderColorBottom = borderColor1, Rowspan = 3, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER });


                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Für vorhandene BA	- Typenbezeichnung entspricht dem Typenschild\nFor existing manual 	- type designation corresponds to the nameplate", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Type_Designation_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Type_Designation_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });

                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Für Maschine als Ersatzbestätigung vorhanden und der CE-Checkliste beigefügt\nConfirmation of delivery issued for manuel and enclosed with the checklist\n     - Datenblatt mit Geräuschangaben an SB in BU gegeben / datasheet with\n     noise specification forwarded to the commercial coordinator of the BU", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Instruction_Manual_NR"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Instruction_Manual_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });
                    }

                    //6.0
                    parameterData = dtData.AsEnumerable().Where(k => k.Field<string>("Parameter") == "6.0").FirstOrDefault();
                    if (parameterData != null)
                    {
                        var rowValue = JObject.Parse(parameterData["CE_value"].ToString());
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("6.0", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeaderUnderLine("Sicherheitsanforderungen und Maßnahmen (nach neuer Zentrifugennorm)", fs2)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthLeft = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthLeft = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Packerei / packing area", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BackgroundColor = backColorSilver });

                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Safety requirements and actions (acc. to the new centrifuge standard)\n- Äußere Angabe von Gewicht, Schwerpunktlage, Hubposition am Verschlag\ndetails outside (weight, centre of gravity, lifting position at the crate", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["New_Centrifuge_Standard_NR"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["New_Centrifuge_Standard_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["New_Centrifuge_Standard_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText(rowValue["New_Centrifuge_Standard_Packing_Area"].ToString() + "\n" + getCECheckListDate(rowValue["UpdatedTS_String"].ToString()), fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderColorBottom = borderColor1, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER });
                    }

                    //7.0
                    parameterData = dtData.AsEnumerable().Where(k => k.Field<string>("Parameter") == "7.0").FirstOrDefault();
                    if (parameterData != null)
                    {
                        var rowValue = JObject.Parse(parameterData["CE_value"].ToString());
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("7.0", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeaderUnderLine("Auslieferung / delivery", fs2)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthLeft = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthLeft = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Packerei / packing area", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BackgroundColor = backColorSilver });


                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        pdfTable = new PdfPTable(2);
                        cellWidth1 = new int[2] { 200, 400 };
                        pdfTable.SetWidths(cellWidth1);
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText(" CE-Kennzeichnung,\n (CE-mark ", rowValue["Delivery_CE_Mark"].ToString(), fs2)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText("CE0044-Kennzeichnung (DGRL)		 vorgenommen\n(pressure equipment(PED)-mark)		 on nameplate", rowValue["Delivery_PED_Mark"].ToString(), fs2)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Delivery_Mark_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Delivery_Mark_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Delivery_Packing_Area"].ToString() + "\n" + getCECheckListDate(rowValue["UpdatedTS_String"].ToString()), fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderColorBottom = borderColor1, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, Rowspan = 2 });


                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("- in MK eingetragen (entered in the machine card)", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Machine_Card_J"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Machine_Card_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });
                    }

                    //8.0
                    parameterData = dtData.AsEnumerable().Where(k => k.Field<string>("Parameter") == "8.0").FirstOrDefault();
                    if (parameterData != null)
                    {
                        var rowValue = JObject.Parse(parameterData["CE_value"].ToString());
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("8.0", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeaderUnderLine("Dokumentationsabschluß / documentation completion", fs2)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthLeft = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthLeft = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthRight = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BackgroundColor = backColorSilver });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Abnahme / acceptance", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BackgroundColor = backColorSilver });

                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Konformitätserklärung  -  Einbauerklärung       ausgestellt   /", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Declaration_Of_InCoporation_R"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Declaration_Of_InCoporation_N"].ToString())) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText(rowValue["UpdatedBy"].ToString() + "\n" + getCECheckListDate(rowValue["UpdatedTS_String"].ToString()), fs2, true)) { Rowspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderColorBottom = borderColor1, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER });

                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        pdfTable = new PdfPTable(2);
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText("declaration of conformity", rowValue["DeclarationOfConformity"].ToString(), fs2)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText("- declaration of incoporation issued", rowValue["DeclarationOfIncorporationIssued"].ToString(), fs2)) { HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });

                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0, BorderWidthRight = 0, BorderColorBottom = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("(  nicht zutreffendes streichen / delete as applicable   ) ", fs3, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderColorBottom = borderColor1, BorderColorTop = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderColor = borderColor1, Border = 1 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = borderwidth, BorderColorLeft = borderColor2, BorderWidthRight = borderwidth, BorderColorRight = borderColor2, BackgroundColor = backColorSilver, BorderWidthBottom = borderwidth, BorderColorBottom = borderColor1 });
                    }

                    //footer
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, MinimumHeight = 15, Border = 0, Colspan = 6 });
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("NR =nicht relevant / not relevant                              J = ja / yes                            N = nein / no                               132202-OR-DE/EN-01.2017-000", fs2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, MinimumHeight = 15, Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                    if (isFromFinalReport)
                    {
                        pdfDoc.Add(getFinalReportParagraph(mainTbl));
                    }
                    else
                    {
                        pdfDoc.Add(mainTbl);
                    }
                }
                else
                {
                    ReportStatus = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
            }
            return mainTbl;
        }
        private static string getCECheckListDate(string dateTime)
        {
            string date = "";
            try
            {
                if (!string.IsNullOrEmpty(dateTime))
                {
                    date = Util.GetDateTime(dateTime).ToString("dd-MM-yyyy");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return date;
        }
        #endregion

        #region ----- Pro Decanter Report ----
        internal static string ProDecanterReport(string ProductionOrder, string fabricationNumber, bool generateOnlyProReport)
        {
            string ReportStatus = string.Empty;
            try
            {
                isHeaderFooterRequired = true;
                float marginTop = 15, marginBottom = 10;
                if (generateOnlyProReport)
                {
                    marginTop = 70;
                    marginBottom = 45;
                }
                Document pdfDoc = new Document(PageSize.A4, 15, 15, marginTop, marginBottom);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                pdfDoc.Open();
                reportNameForHeaderFooter = "ProDecanter";
                pdfWriter.PageEvent = new HeaderFooter();
                pdfWriter.PageEvent = new Header();
                ReportStatus = ProDecanterReportTbl(pdfDoc, pdfWriter, ProductionOrder, fabricationNumber, 20, false, generateOnlyProReport);
                if (ReportStatus == string.Empty)
                {
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fabricationNumber + ".pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
            }
            return ReportStatus;
        }
        private static void setProDecanterDocMargin(Document pdfDoc, bool isHeaderThereInPage)
        {
            if (isHeaderThereInPage)
            {
                pdfDoc.SetMargins(15, 15, 70, 45);
            }
            else
            {
                pdfDoc.SetMargins(15, 15, 15, 10);
            }
        }
        private static iTextSharp.text.BaseColor getProDecanterBackColor(string POMaterialID, string model)
        {
            if (model.Contains(POMaterialID))
            {
                return iTextSharp.text.BaseColor.YELLOW;
            }
            else
            {
                return iTextSharp.text.BaseColor.WHITE;
            }
        }
        internal static string ProDecanterReportTbl(Document pdfDoc, PdfWriter pdfWriter, string ProductionOrder, string fabricationNumber, int mainTblSpaceBefore, bool isFromFinalReport, bool generateOnlyProReport)
        {
            string ReportStatus = string.Empty;
            PdfPTable finalTbl = new PdfPTable(1);
            finalTbl = new PdfPTable(1);
            finalTbl.SplitLate = false;
            finalTbl.WidthPercentage = 100;
            finalTbl.SpacingBefore = mainTblSpaceBefore;
            finalTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            finalTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPTable mainTbl = new PdfPTable(1);
            try
            {
                DataTable dtSchedule = new DataTable();
                DataTable dtData = new DataTable();
                dtData = GEADatabaseAccess.getProDecanterReportData(ProductionOrder, fabricationNumber, out dtSchedule);

                if (dtSchedule.Rows.Count > 0 && dtData.Rows.Count > 0)
                {
                    Paragraph p2 = new Paragraph();
                    string imgPdfPath = "";
                    if (generateOnlyProReport == false)
                    {
                        isHeaderFooterRequired = false;

                        //Add upload pdf
                        imgPdfPath = ConfigurationManager.AppSettings["GEAImagePath"].ToString() + "\\" + ProductionOrder + "_" + fabricationNumber;
                        try
                        {

                            BindExistingPDFToFinalReport(pdfWriter, pdfDoc, imgPdfPath, "1");

                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog(ex.Message);
                        }


                        //Blue test card report
                        mainTbl = new PdfPTable(1);
                        mainTbl.SplitLate = false;
                        mainTbl.WidthPercentage = 100;
                        mainTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        mainTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        mainTbl = decanterAcceptanceTestCardReportTbl(ProductionOrder, fabricationNumber, 0, out ReportStatus);

                        p2 = new Paragraph();
                        if (ReportStatus == string.Empty)
                        {
                            finalTbl.AddCell(new PdfPCell(mainTbl) { });
                            pdfDoc.Add(finalTbl);
                            setProDecanterDocMargin(pdfDoc, true);
                            pdfDoc.Add(p2);
                            pdfDoc.NewPage();
                        }
                    }
                    isHeaderFooterRequired = true;
                    setProDecanterDocMargin(pdfDoc, isHeaderFooterRequired);

                    #region -------- Pro decanter ---------
                    string materialId = "";
                    //Common Process
                    mainTbl = new PdfPTable(1);
                    mainTbl.SplitLate = false;
                    mainTbl.WidthPercentage = 100;
                    mainTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    mainTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    int paddingvalue = 3;
                    int fs1 = 8, fs2 = 11, fs3 = 8;
                    iTextSharp.text.BaseColor borderColor1 = new iTextSharp.text.BaseColor(219, 219, 219);
                    iTextSharp.text.BaseColor borderColor2 = new iTextSharp.text.BaseColor(0, 0, 0);
                    iTextSharp.text.BaseColor backColorSilver = new iTextSharp.text.BaseColor(230, 230, 230);
                    int minHeight = 10;
                    PdfPTable pdfTable = new PdfPTable(2);
                    int[] cellWidth = new int[2] { 100, 300 };

                    List<DataRow> processDataList = dtData.AsEnumerable().Where(k => k.Field<string>("ReportType").Equals("ProReport", StringComparison.OrdinalIgnoreCase) && k.Field<string>("Parameter").Equals("0", StringComparison.OrdinalIgnoreCase)).ToList();
                    if (processDataList != null)
                    {

                        var rowValue = JObject.Parse(processDataList[0]["Pro_value"].ToString());

                        //header
                        DataRow scheduleRow = dtSchedule.Rows[0];
                        pdfTable = new PdfPTable(2);
                        pdfTable.WidthPercentage = 100;
                        cellWidth = new int[2] { 100, 300 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Sales Order / Item number:", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(scheduleRow["SaleOrder"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Decanter part number:", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(scheduleRow["MaterialID"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        materialId = scheduleRow["MaterialID"].ToString();
                        materialId = materialId.Replace("PRO", "").Trim();
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Type:", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Type"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Fabrication number:", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(scheduleRow["FabricationNo"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = minHeight });

                        pdfTable = new PdfPTable(5);
                        pdfTable.SplitLate = false;
                        pdfTable.WidthPercentage = 100;
                        cellWidth = new int[5] { 70, 200, 100, 150, 70 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("In Process Nonconforming Record ", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, Colspan = 5 });

                        paddingvalue = 10;
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Process", fs1, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Nonconformity Note", fs1, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Issued by", fs1, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Action & Status", fs1, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked by", fs1, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        foreach (DataRow row in processDataList)
                        {
                            rowValue = JObject.Parse(row["Pro_value"].ToString());
                            string jsonV = rowValue["Process_Non_Conforming_Record"].ToString();
                            var list = new JavaScriptSerializer().Deserialize<dynamic>(jsonV);
                            foreach (var item in list)
                            {
                                pdfTable.AddCell(new PdfPCell(getPdfCellWithText(item["Process"], fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                                pdfTable.AddCell(new PdfPCell(getPdfCellWithText(item["Non_Conformity_Note"], fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                                pdfTable.AddCell(new PdfPCell(getPdfCellWithText(item["IssueBy"], fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                                pdfTable.AddCell(new PdfPCell(getPdfCellWithText(item["Action_Status"], fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                                pdfTable.AddCell(new PdfPCell(getPdfCellWithText(item["CheckedBy"], fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                            }
                        }
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });
                    }
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = minHeight });
                    paddingvalue = 3;
                    finalTbl = new PdfPTable(1);
                    finalTbl = new PdfPTable(1);
                    finalTbl.SplitLate = false;
                    finalTbl.WidthPercentage = 100;
                    finalTbl.SpacingBefore = mainTblSpaceBefore;
                    finalTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    finalTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    finalTbl.AddCell(new PdfPCell(mainTbl) { Padding = 10 });
                    pdfDoc.Add(finalTbl);
                    p2 = new Paragraph();
                    if (pdfDoc.IsOpen())
                    {
                        pdfDoc.Add(p2);
                        pdfDoc.NewPage();
                    }

                    paddingvalue = 4;
                    //Painting 1.1.1
                    mainTbl = new PdfPTable(1);
                    mainTbl.SplitLate = false;
                    mainTbl.WidthPercentage = 100;
                    mainTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    mainTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("1. Painting", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("1.1 Confirm the information ", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });

                    //Painting 1.1.1
                    pdfTable = new PdfPTable(2);
                    cellWidth = new int[2] { 600, 100 };
                    pdfTable.SetWidths(cellWidth);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("1.1.1 Check painting color according to machine card.", fs1, false)) { Border = 0 });
                    pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell("fasle")) { Border = 0 });
                    mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });

                    //Painting 1.1.2
                    pdfTable = new PdfPTable(2);
                    cellWidth = new int[2] { 600, 100 };
                    pdfTable.SetWidths(cellWidth);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("1.1.2 Check painting standards according to machine card.	", fs1, false)) { Border = 0 });
                    pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell("fasle")) { Border = 0 });
                    mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });

                    pdfTable = new PdfPTable(4);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("WSN", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Coating system", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Corrosivity category", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    pdfTable.AddCell(new PdfPCell(getcheckBoxWithText("WSN 66-1000-35", "false", fs1)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Textured paint", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("C3", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    pdfTable.AddCell(new PdfPCell(getcheckBoxWithText("WSN 66-1000-36", "false", fs1)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Smooth paint", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("C5", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingBottom = paddingvalue, PaddingTop = paddingvalue });

                    //Painting 1.2
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("1.2 Degreasing 	WSN66-1000-03", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });

                    //Painting 1.3
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("1.3 Painting condition", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });

                    pdfTable = new PdfPTable(5);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("R.H.%", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Temperature °C", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Act.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Requirement. ", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Act.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Requirement. ", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("10~75%", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("15~35°C", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingBottom = paddingvalue, PaddingTop = paddingvalue });

                    //Painting 1.4
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("1.4 Painting Inspection Record	", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });

                    //Painting 1.4.1
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("1.4.1 Paint information and usage ", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });

                    pdfTable = new PdfPTable(7);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Type/", fs1, false)) { Colspan = 2, Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Lot number", fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Due date", fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Act.", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("2 layer", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("3 layer", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Intergard 345", fs1, false)) { Rowspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("AAB601", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { Rowspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("AAA046", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("GTA220", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingBottom = paddingvalue, PaddingTop = paddingvalue });

                    //Painting 1.4.2
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("1.4.2 Use standard of paint ", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });

                    pdfTable = new PdfPTable(6);
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Type/", fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Description", fs1, false)) { Colspan = 4, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("painting material: Hardener (weight ratio)", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("painting material: Hardener (volume ratio)", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Intergard 345", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("6.4:1", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("15% Thinner", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("4:1", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("15% Thinner", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingBottom = paddingvalue, PaddingTop = paddingvalue });


                    //Painting 1.4.3
                    var processData = dtData.AsEnumerable().Where(k => k.Field<string>("ReportType").Equals("Painting", StringComparison.OrdinalIgnoreCase) && k.Field<string>("Parameter").Equals("1.1.1", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (processData != null)
                    {
                        var rowValue = JObject.Parse(processData["Pro_value"].ToString());
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("1.4.3 Frame", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable = new PdfPTable(7);
                        pdfTable.SplitLate = true;
                        pdfTable.WidthPercentage = 100;
                        cellWidth = new int[7] { 100, 90, 100, 100, 120, 120, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Measurement", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Average", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Measurement", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Average", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Textured\n paint", fs1, false)) { Rowspan = 5, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Point", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Top painting WFT / [114~144μm]", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Textured dry layer thickness(final) \n[160~190μm] ", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["PaintingFrame1Checked"].ToString(), fs1, false)) { Rowspan = 10, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("A", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Texture_A_WFT"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Texture_Avg_WFT"].ToString(), fs1, false)) { Rowspan = 4, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Texture_A_Dry"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Texture_Avg_Dry"].ToString(), fs1, false)) { Rowspan = 4, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("B", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Texture_B_WFT"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Texture_B_Dry"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("C", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Texture_C_WFT"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Texture_C_Dry"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("D", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Texture_D_WFT"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Texture_D_Dry"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        paddingvalue = 3;
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Smooth paint", fs1, false)) { Rowspan = 5, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Point", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Top Painting 1 DFT /[200~230μm]", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Top painting 2 DFT \n[320~350μm]", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("A", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Smooth_A_WFT"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Smooth_Avg_WFT"].ToString(), fs1, false)) { Rowspan = 4, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Smooth_A_Dry"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Smooth_Avg_Dry"].ToString(), fs1, false)) { Rowspan = 4, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });


                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("B", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Smooth_B_WFT"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Smooth_B_Dry"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("C", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Smooth_C_WFT"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Smooth_C_Dry"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("D", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Smooth_D_WFT"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Frame_Smooth_D_Dry"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("Packing_1.1.1.png", 250f, 250f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Colspan = 4 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Note：\n1.The thickness of the primer guaranteed by the supplier, machining surface 30μm~50μm; Other surface 80μm~120μm.\n2.The surface of frame where hood is placed, will be painted without particles.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Colspan = 2, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["PaintingFrame2Checked"].ToString(), fs1, false)) { Rowspan = 9, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Oven drying time", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Start", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Ending", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Temperature", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Textured\n paint", fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Top Painting", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("normal temperature /\n 0.5H", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Textured", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("60°C / 1H", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Smooth\n paint", fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Top Painting 1", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("60°C / 1H", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Top Painting 2", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("60°C / 1H", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Visual inspection", fs1, false)) { Rowspan = 3, Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        PdfPTable innerTable = new PdfPTable(1);
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("have air bubbles", rowValue["FrameHaveAirBubble"].ToString(), fs1)) { Border = 0 });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("no air bubbles", rowValue["FrameHaveNoAirBubble"].ToString(), fs1)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(innerTable) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Adhesion check", fs1, false)) { Rowspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        innerTable = new PdfPTable(1);
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("   level0", rowValue["FrameLevel0"].ToString(), fs1)) { Border = 0 });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("   level1", rowValue["FrameLevel1"].ToString(), fs1)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(innerTable) { Rowspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        innerTable = new PdfPTable(1);
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("Painting missing", rowValue["FramePaintingMissing"].ToString(), fs1)) { Border = 0 });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("Painting ok", rowValue["FramePaintingOk"].ToString(), fs1)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(innerTable) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        innerTable = new PdfPTable(1);
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("Painting sag", rowValue["FramePaintingSAG"].ToString(), fs1)) { Border = 0 });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("Painted no sag", rowValue["FramePaintingNoSAG"].ToString(), fs1)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(innerTable) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });

                    }


                    //Painting 1.4.4
                    processData = dtData.AsEnumerable().Where(k => k.Field<string>("ReportType").Equals("Painting", StringComparison.OrdinalIgnoreCase) && k.Field<string>("Parameter").Equals("1.1.2", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (processData != null)
                    {
                        var rowValue = JObject.Parse(processData["Pro_value"].ToString());
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("1.4.4  Bearing housing", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable = new PdfPTable(7);
                        pdfTable.SplitLate = false;
                        pdfTable.WidthPercentage = 100;
                        cellWidth = new int[7] { 70, 90, 100, 100, 120, 120, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Measurement", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Average", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Measurement", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Average", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Textured\n paint", fs1, false)) { Rowspan = 5, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Point", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Top painting WFT \n[114~144μm]", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Textured dry layer thickness(final) \n[160~190μm] ", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["PaintingBearinghouse1Checked"].ToString(), fs1, false)) { Rowspan = 10, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("A", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Texture_A_WFT"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Texture_Avg_WFT"].ToString(), fs1, false)) { Rowspan = 4, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Texture_A_Dry"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Texture_Avg_Dry"].ToString(), fs1, false)) { Rowspan = 4, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("B", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Texture_B_WFT"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Texture_B_Dry"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("C", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Texture_C_WFT"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Texture_C_Dry"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("D", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Texture_D_WFT"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Texture_D_Dry"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Smooth\n paint", fs1, false)) { Rowspan = 5, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Point", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Top Painting 1 DFT /[200~230μm]", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Top painting 2 DFT \n[320~350μm]", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("A", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Smooth_A_WFT"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Smooth_Avg_WFT"].ToString(), fs1, false)) { Rowspan = 4, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Smooth_A_Dry"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Smooth_Avg_Dry"].ToString(), fs1, false)) { Rowspan = 4, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });


                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("B", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Smooth_B_WFT"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Smooth_B_Dry"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("C", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Smooth_C_WFT"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Smooth_C_Dry"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("D", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Smooth_D_WFT"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["BearingHousing_Smooth_D_Dry"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("Packing_1.1.2.png", 150f, 150f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Colspan = 4 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Note：\nThe thickness of the primer guaranteed by the supplier，machining surface 30μm -50μm, other surface 80μm~120μm", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Colspan = 2, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["PaintingBearinghouse2Checked"].ToString(), fs1, false)) { Rowspan = 9, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Oven drying time", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Start", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Ending", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Temperature", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Textured\n paint", fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Top Painting", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("normal temperature \n/ 0.5H", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Textured", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("60°C / 1H", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Smooth\n paint", fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Top Painting 1", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("60°C / 1H", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Top Painting 2", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("60°C / 1H", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Visual inspection", fs1, false)) { Rowspan = 3, Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        PdfPTable innerTable = new PdfPTable(1);
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("have air bubbles", rowValue["BearingHousingHaveAirBubble"].ToString(), fs1)) { Border = 0 });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("no air bubbles", rowValue["BearingHousingHaveNoAirBubble"].ToString(), fs1)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(innerTable) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Adhesion check", fs1, false)) { Rowspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        innerTable = new PdfPTable(1);
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("   level0", rowValue["BearingHousingLevel0"].ToString(), fs1)) { Border = 0 });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("   level1", rowValue["BearingHousingLevel1"].ToString(), fs1)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(innerTable) { Rowspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        innerTable = new PdfPTable(1);
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("Painting missing", rowValue["BearingHousingPaintingMissing"].ToString(), fs1)) { Border = 0 });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("Painting ok", rowValue["BearingHousingPaintingOk"].ToString(), fs1)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(innerTable) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        innerTable = new PdfPTable(1);
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("Painting sag", rowValue["BearingHousingPaintingSAG"].ToString(), fs1)) { Border = 0 });
                        innerTable.AddCell(new PdfPCell(getcheckBoxWithText("Painted no sag", rowValue["BearingHousingPaintingNoSAG"].ToString(), fs1)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(innerTable) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });
                    }

                    processData = dtData.AsEnumerable().Where(k => k.Field<string>("ReportType").Equals("Painting", StringComparison.OrdinalIgnoreCase) && k.Field<string>("Parameter").Equals("1.1.3", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (processData != null)
                    {
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = minHeight });
                        var rowValue = JObject.Parse(processData["Pro_value"].ToString());
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("1.4.5 Only use black paint\n   Please use black paint to spraying surface A. ", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });

                        pdfTable = new PdfPTable(4);
                        pdfTable.SplitLate = false;
                        pdfTable.WidthPercentage = 100;
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 4000", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 4000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 5000", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 5000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 7000", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 7000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Bushing", fs2, true)) { Colspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["PaintingBlackpaintChecked"].ToString(), fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        PdfPTable innerTable = new PdfPTable(2);
                        cellWidth = new int[2] { 30, 100 };
                        innerTable.SetWidths(cellWidth);
                        innerTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Pro4000Model"].ToString())) { Border = 0 });
                        innerTable.AddCell(new PdfPCell(getProDecanterImages("Packing_1.1.3.png", 80f, 80f)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(innerTable) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        innerTable = new PdfPTable(2);
                        cellWidth = new int[2] { 30, 100 };
                        innerTable.SetWidths(cellWidth);
                        innerTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Pro5000Model"].ToString())) { Border = 0 });
                        innerTable.AddCell(new PdfPCell(getProDecanterImages("Packing_1.1.3.png", 80f, 80f)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(innerTable) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        innerTable = new PdfPTable(2);
                        cellWidth = new int[2] { 30, 100 };
                        innerTable.SetWidths(cellWidth);
                        innerTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Pro7000Model"].ToString())) { Border = 0 });
                        innerTable.AddCell(new PdfPCell(getProDecanterImages("Packing_1.1.3.png", 80f, 80f)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(innerTable) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });
                    }
                    paddingvalue = 3;
                    //Assembly 2
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = 60 });
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("2. Assembly", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });


                    processData = dtData.AsEnumerable().Where(k => k.Field<string>("ReportType").Equals("Assembly", StringComparison.OrdinalIgnoreCase) && k.Field<string>("Parameter").Equals("2", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    PdfPTable innerTbl = new PdfPTable(1);
                    if (processData != null)
                    {
                        var rowValue = JObject.Parse(processData["Pro_value"].ToString());

                        //Assembly 2.1
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("2.1 Check and write down fabrication number", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable = new PdfPTable(3);
                        cellWidth = new int[3] { 400, 150, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Item", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Description", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Fabrication number of machine according to the machine card", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(fabricationNumber, fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Assembly_2_1_Checked"].ToString(), fs1, false)) { Rowspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Fabrication number of the bowl shell according to the shell surface mark", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(fabricationNumber, fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Fabrication number of scroll according to the scroll surface mark", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(fabricationNumber, fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });



                        //Assembly 2.2
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("2.2 Write down the Material identification data in the below chart", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });

                        string machineid = GEADatabaseAccess.getMachineIDProDecanterAssemblyReportData(ProductionOrder, fabricationNumber);
                        DataTable assemblyDT = GEADatabaseAccess.getProDecanterAssemblyReportData(ProductionOrder, fabricationNumber, machineid);
                        if (assemblyDT.Rows.Count > 0)
                        {
                            pdfTable = new PdfPTable(6);
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Item", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Part Number", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Check No\n(series+serial no)", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Production Number", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Dye-pen test", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                            int i = 0;
                            foreach (DataRow row in assemblyDT.Rows)
                            {
                                pdfTable.AddCell(new PdfPCell(getPdfCellWithText(row["ParameterID"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                                pdfTable.AddCell(new PdfPCell(getPdfCellWithText(row["PartNo"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                                pdfTable.AddCell(new PdfPCell(getPdfCellWithText(row["SerialNo"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                                pdfTable.AddCell(new PdfPCell(getPdfCellWithText(row["ProductionNumber"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                                pdfTable.AddCell(new PdfPCell(getPdfCellWithText(row["Dyepentest"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                                if (i == 0)
                                {
                                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Assembly_2_2_Checked"].ToString(), fs1, false)) { Rowspan = assemblyDT.Rows.Count, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                                    i++;
                                }
                            }
                            mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });
                        }

                        //Assembly 2.3
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("2.3 Fix the bushing on the shell glue with Loctite 275 and activator according to the drawing.", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Assembly_2_3"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });

                        //Assembly 2.4
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("2.4  Check below size with a depth gauge", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable = new PdfPTable(5);
                        pdfTable.SplitLate = false;
                        cellWidth = new int[5] { 400, 150, 150, 150, 150 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Description/", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Type/", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Standard/标准", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Actual/实际", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("Assembly_2.4.1.png", 100f, 100f)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 4000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = 50, BackgroundColor = getProDecanterBackColor(materialId, "PRO 4000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("24.8 + 0.8", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = 50, BackgroundColor = getProDecanterBackColor(materialId, "PRO 4000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Assembly_Pro4000"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = 50, BackgroundColor = getProDecanterBackColor(materialId, "PRO 4000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Assembly_2_4_1_Checked"].ToString(), fs1, false)) { Rowspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 5000/5500", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 5000/5500") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("40.3 + 0.8", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 5000/5500") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Assembly_Pro5000"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 5000/5500") });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Drawing no.:\n8441-6600-000 / 8442-6600-000/ 8442-6600-010 / 8444-6600-000\nDistance between surface of bushing and the plane surface of the\ngearbox on the solid side.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 7000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 7000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("35.3 + 1.0", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 7000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Assembly_Pro7000"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 7000") });

                        innerTbl = new PdfPTable(1);
                        innerTbl.AddCell(new PdfPCell(getProDecanterImages("Assembly_2.4.2.png", 100f, 100f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("Drawing no.:\n8443-6600-000\nThe dimension has to be measured from the plan surface of the \nprimary gear box, after the assembly of the bearing housing \nincluding the bearing", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(innerTbl) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 6000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 6000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("50.4 + 0.5", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 6000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Assembly_Pro6000"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 6000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Assembly_2_4_2_Checked"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });

                        //Assembly 2.5
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("2.5 Please make the “0” mark on the same line.", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Assembly_2_5"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });

                        //Assembly 2.6
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("2.6 Lubrication and grease position should follow the working instruction.", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Assembly_2_6"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });

                        //Assembly 2.7
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("2.7 Cleaning the rotor parts from corrosion protection wax.", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Assembly_2_7"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });

                        //Assembly 2.8
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("2.8 Assembly rotor on the frame.\n    Turn the bowl by hands to avoid the bolts fixed. ", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Assembly_2_8"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });

                        //Assembly 2.9
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("2.9 Fitting the main motor and secondary motor.\n     Main and secondary motor assembly according to the working instruction.", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Assembly_2_9"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });

                        pdfTable = new PdfPTable(3);
                        cellWidth = new int[3] { 500, 200, 150 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Item", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Description", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Check the belt pulleys for correct alignment after tension the drive belt\nFirst groove belt installed near the motor \n- Avoid the errors illustrated. \n- The deviation of the pulley alignment should 0 mm. ", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("Assembly_2.9.png", 70f, 70f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Assembly_2_10_Checked"].ToString(), fs1, false)) { Rowspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("The main motor support plate should be parallel with frame.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Assembly_MainMotorSupport"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });


                        innerTbl = new PdfPTable(3);
                        cellWidth = new int[3] { 100, 70, 200 };
                        innerTbl.SetWidths(cellWidth);
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("Tension the drive belt\nUse measuring device 0003-0534-000 for this purpose. 0003-0534-000\nThe specified value for ideally tensioned belts is for new belts: 1000 +100 N ", fs1, false)) { Colspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("Tension Force（ F ）= ", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthBottom = 1 });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(innerTbl) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Assembly_Tension_drive_belt"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });

                        //Assembly 2.10.1
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("2.10.1 Fill the oil      ", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Assembly_2_10_1"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("     Fill the gearbox completely with oil. Insert the oil drain screws and tighten them with a torque wrench. Torque value according description on the head of the screw. Fill the oil surge vessel halfway with oil.\n\n     Lubrication and maintenance schedule according to customer order. ", fs1, false)) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Assembly 2.10.2
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("2.10.2 Regrease of gear boxes (Pro 2200) ", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Assembly_2_10_2"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("     Regrease the gear boxes after the initial run. Disassemble the secondary gear box plug and regrease until grease is coming out. Close the secondary gear box. Disassemble the primary gear box plug. Regerease the primary gear box until grease is coming out. Close the primary gear box.", fs1, false)) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Assembly 2.11
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("2.11 Before gluing moulded gasket, clean the surface of the protective casing, especially where the moulded gasket is glued.", fs1, false)) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable = new PdfPTable(4);
                        pdfTable.SplitLate = false;
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Type/", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Description/", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        innerTbl = new PdfPTable(6);
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("PRO", fs1 - 1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText(" 4000", fs1 - 2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "4000") });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("/ 5000", fs1 - 2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "5000") });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("/ 5500", fs1 - 2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "5500") });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("/ 6000", fs1 - 2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "6000") });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("/ 7000", fs1 - 2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "7000") });
                        pdfTable.AddCell(new PdfPCell(innerTbl) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        // pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 4000/5000/5500/6000/7000", fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("Assembly_2.11.1.png", 80f, 80f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthRight = 0, BorderWidthBottom = 0 });
                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("Assembly_2.11.2.png", 80f, 80f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthLeft = 0, BorderWidthBottom = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Assembly_2_11_Checked"].ToString(), fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Where the moulded gasket is glued.", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthTop = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });

                        //Assembly 2.12
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("2.12 Check the distance between the speed switch with slide disc.", fs1, false)) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable = new PdfPTable(4);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Type", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Distance", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Description", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        innerTbl = new PdfPTable(6);
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("PRO", fs1 - 1, false)) { Colspan = 6, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, HorizontalAlignment = Element.ALIGN_CENTER });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText(" 2200", fs1 - 2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "2200") });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("/ 4000", fs1 - 2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "4000") });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("/ 5000", fs1 - 2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "5000") });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("/ 5500", fs1 - 2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "5500") });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("/ 6000", fs1 - 2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "6000") });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("/ 7000", fs1 - 2, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "7000") });
                        pdfTable.AddCell(new PdfPCell(innerTbl) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        //pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 2200/4000/5000/5500/6000/7000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("2.5mm", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("Assembly_2.12.png", 80f, 80f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Assembly_2_12_Checked"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });

                        //Assembly 2.13
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("2.13 Electrical connection according to diagram.", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["ElectricalConnection_2_13"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0 });

                    }

                    finalTbl = new PdfPTable(1);
                    finalTbl.SplitLate = false;
                    finalTbl.WidthPercentage = 100;
                    finalTbl.SpacingBefore = mainTblSpaceBefore;
                    finalTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    finalTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    finalTbl.AddCell(new PdfPCell(mainTbl) { Padding = 10 });
                    pdfDoc.Add(finalTbl);
                    p2 = new Paragraph();
                    if (pdfDoc.IsOpen())
                    {
                        pdfDoc.Add(p2);
                        pdfDoc.NewPage();
                    }

                    //Testing 3
                    mainTbl = new PdfPTable(1);
                    mainTbl.SplitLate = true;
                    mainTbl.WidthPercentage = 100;
                    mainTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    mainTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = minHeight });
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("3. Testing", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });

                    string balancingPO = "";
                    processData = dtData.AsEnumerable().Where(k => k.Field<string>("ReportType").Equals("Testing", StringComparison.OrdinalIgnoreCase) && k.Field<string>("Parameter").Equals("3", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    innerTbl = new PdfPTable(1);
                    if (processData != null)
                    {
                        var rowValue = JObject.Parse(processData["Pro_value"].ToString());

                        try
                        {
                            balancingPO = rowValue["BalancingProOrder"].ToString();
                        }
                        catch (Exception ex)
                        {

                        }

                        //Testing 3.1
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("3.1 Standard acceptant test and special customer acceptant tests according to machine card.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });

                        float borderWidth = 0.5f;
                        pdfTable = new PdfPTable(5);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Testing number", fs1, false)) { Colspan = 4, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { Colspan = 4, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText(" 9010-9085-100", rowValue["TestingNumber_9010_9085_100"].ToString(), fs1)) { Border = 0, BorderWidthLeft = borderWidth });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText(" 9010-9085-110", rowValue["TestingNumber_9010_9085_110"].ToString(), fs1)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText(" 9010-9085-120", rowValue["TestingNumber_9010_9085_120"].ToString(), fs1)) { Border = 0 });
                        innerTbl = new PdfPTable(2);
                        innerTbl.AddCell(new PdfPCell(getcheckBoxWithText("   Other: ", rowValue["TestingNumber_other"].ToString(), fs1)) { Border = 0 });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Number_Other1"].ToString(), fs1, false)) { Border = 0, BorderWidthBottom = borderWidth });
                        pdfTable.AddCell(new PdfPCell(innerTbl) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_3_1_Checked"].ToString(), fs1, false)) { Rowspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText(" 9010-9074-120", rowValue["TestingNumber_9010_9074_120"].ToString(), fs1)) { Border = 0, BorderWidthLeft = borderWidth });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText(" 9010-9083-100", rowValue["TestingNumber_9010_9083_100"].ToString(), fs1)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText(" 9010-9075-100", rowValue["TestingNumber_9010_9075_100"].ToString(), fs1)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Number_Other2"].ToString(), fs1, false)) { Border = 0, BorderWidthBottom = borderWidth });

                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText(" 9010-9074-100", rowValue["TestingNumber_9010_9074_100"].ToString(), fs1)) { Border = 0, BorderWidthBottom = borderWidth, BorderWidthLeft = borderWidth });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText(" 9010-9089-210", rowValue["TestingNumber_9010_9089_210"].ToString(), fs1)) { Border = 0, BorderWidthBottom = borderWidth });
                        pdfTable.AddCell(new PdfPCell(getcheckBoxWithText(" 9011-9080-010", rowValue["TestingNumber_9010_9080_010"].ToString(), fs1)) { Border = 0, BorderWidthBottom = borderWidth });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Number_Other3"].ToString(), fs1, false)) { Border = 0, BorderWidthBottom = borderWidth });

                        //mainTbl.AddCell(new PdfPCell(pdfTable) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BorderWidthLeft = 1, BorderWidthRight = 1 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });

                        //Testing 3.2
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("3.2 Check of decanter version according machine card /", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Testing_3_2"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Testing 3.3
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("3.3 Mechanical installation on the test bench / ", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Testing_3_3"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Testing 3.4
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("3.4 Make sure lock screw loosed and fasten the nut /", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Testing_3_4"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Testing 3.5
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("3.5 Make sure band of coupling was cut off /	", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Testing_3_5"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Testing 3.6
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("3.6 Electrical connection according to diagram\nMotor wiring according to technical characteristics of the machine card, keep this setting for delivery", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Testing_3_6"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Testing 3.7
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("3.7 Checking the direction of rotation of the motors\n    Start machine then check the direction of the rotation of the motors.", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Testing_3_7"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });


                        pdfTable = new PdfPTable(4);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Type", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Description", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        innerTbl = new PdfPTable(6);
                        cellWidth = new int[6] { 10, 25, 25, 28, 28, 28 };
                        innerTbl.SetWidths(cellWidth);
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs1 - 1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("PRO", fs1 - 1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("2200", fs1 - 1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "2200") });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("/ 4000", fs1 - 1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "4000") });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("/ 6000", fs1 - 1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "6000") });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("/ 7000", fs1 - 1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "7000") });
                        pdfTable.AddCell(new PdfPCell(innerTbl) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        //pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 2200/4000/6000/7000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("Testing_3.7.1.png", 80f, 80f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("The main motor and the secondary motor rotate counterclockwise.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Motor_Direction_Checked"].ToString(), fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        innerTbl = new PdfPTable(5);
                        cellWidth = new int[5] { 25, 25, 25, 25, 30 };
                        innerTbl.SetWidths(cellWidth);
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs1 - 1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("PRO", fs1 - 1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("5000", fs1 - 1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "5000") });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("/ 5500", fs1 - 1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "5500") });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs1 - 1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(innerTbl) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        //pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 5000/5500", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("Testing_3.7.2.png", 80f, 80f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("The main motor rotates counterclockwise, the secondary motor rotates clockwise.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });


                        //Testing 3.8
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("3.8 Three times De-aeration of gear box", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 400, 200 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("      First time:  After approx. 45 minutes of running and measurement of the bowl unbalance, stop the decanter and de-aerate the gear box.\n     Second time: After balancing step 2 de-aerate the gear box again.\n     Third time: After balancing step 3 de-aerate the gear box again.\n      PRO 4000 and PRO 7000 with the new gear box types, we need to de-aerate the gear boxes at below two different places.\n       PRO 4000  PRO 7000。", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("Testing_3.8.png", 100f, 100f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Testing 3.9
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("3.9 Checking the oil in the tank for discoloration ", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Testing_3_9"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Testing 3.10
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("3.10 Checking the screw plug of the gearbox for damage ", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Testing_3_10"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Testing 3.11
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("3.11 Balancing procedure for Decanter complete of UCT series according to WSN76-0100-62.\nWSN76-0100-62。", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Testing_3_11"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Testing 3.12
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("3.12 For calculation of the balancing weight and position use the Excel balancing tool. Write down the balancing result. ", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable = new PdfPTable(4);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Unbalancing of the solid side (end of the balancing test)", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Unbalancing of the liquid side(end of the balancing test)", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("mm/s", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("meas. plane", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("mm/s", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("meas. plane", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Solid_MMS"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Solid_MeasPlane"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Liquid_MMS"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Liquid_MeasPlane"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                    }


                    //Testing Report 4
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = minHeight });
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("4. Test Report", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });

                    processData = dtData.AsEnumerable().Where(k => k.Field<string>("ReportType").Equals("TestReport", StringComparison.OrdinalIgnoreCase) && k.Field<string>("Parameter").Equals("4", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    innerTbl = new PdfPTable(1);
                    if (processData != null)
                    {
                        var rowValue = JObject.Parse(processData["Pro_value"].ToString());

                        //Testing Report 4.1
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("4.1 Recording the data/ (Requirements according to the machine card)", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable = new PdfPTable(5);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Order number", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_OrderNumber"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Machine type:", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_MachineType"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Separator part number", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_SeperatePartNumber"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Bowl number", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Bowl_Number"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["TestReport_4_1_Checked"].ToString(), fs1, false)) { Rowspan = 4, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Measured Bowl speed", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Bowl_Speed"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Requirement Flow Rate", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Req_Flow_Rate"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Measured Differential speed", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Differential_Speed"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Measured Flow Rate\n（±2m3 / h）", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Measured_Flow_Rate"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Temperature for\nSolid side （< 100ºC）", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Solid_Temp"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Temperature for \nliquid side （< 100ºC）", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Liquid_Temp"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });


                        var processData2 = dtData.AsEnumerable().Where(k => k.Field<string>("ReportType").Equals("TestReport", StringComparison.OrdinalIgnoreCase) && k.Field<string>("Parameter").Equals("4.2", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        innerTbl = new PdfPTable(1);
                        if (processData2 != null)
                        {
                            var rowValue2 = JObject.Parse(processData2["Pro_value"].ToString());
                            //Test report 4.2
                            mainTbl.AddCell(new PdfPCell(getPdfCellWithText("4.2 Check of the electromotor", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                            pdfTable = new PdfPTable(5);
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Motor", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Series No", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Type", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Manufacture", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });


                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Motor for main drive", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue2["Testing_Main_Drive_SerialNumber"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue2["Testing_Main_Drive_Type"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                            innerTbl = new PdfPTable(1);
                            innerTbl.AddCell(new PdfPCell(getcheckBoxWithText(" WEG", rowValue2["Testing_Main_Drive_WEG"].ToString(), fs1)) { Border = 0 });
                            innerTbl.AddCell(new PdfPCell(getcheckBoxWithText(" Other", rowValue2["Testing_Main_Drive_Other"].ToString(), fs1)) { Border = 0 });
                            pdfTable.AddCell(new PdfPCell(innerTbl) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                            string checkedName = rowValue2["TestReport_4_2_Checked"].ToString();
                            if (string.IsNullOrEmpty(checkedName))
                            {
                                checkedName = rowValue2["AssemblyReport_4_2_Checked"].ToString();
                            }
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText(checkedName, fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Motor for secondary drive", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue2["Testing_Secondary_Drive_SerialNumber"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                            pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue2["Testing_Secondary_Drive_Type"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                            innerTbl = new PdfPTable(1);
                            innerTbl.AddCell(new PdfPCell(getcheckBoxWithText(" WEG", rowValue2["Testing_Secondary_Drive_WEG"].ToString(), fs1)) { Border = 0 });
                            innerTbl.AddCell(new PdfPCell(getcheckBoxWithText(" Other", rowValue2["Testing_Secondary_Drive_Other"].ToString(), fs1)) { Border = 0 });
                            pdfTable.AddCell(new PdfPCell(innerTbl) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                            mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        }

                        //Test report 4.3
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("4.3 Measurement vibration data (mm/s) according to WSN76-0102-00", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable = new PdfPTable(5);
                        cellWidth = new int[5] { 50, 70, 200, 100, 150 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Measurement position ", fs1, false)) { Colspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Vibration （mm/s）", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked / ", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("B", fs1, false)) { Rowspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Point 2", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Liquid end Vertical", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Point2"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["TestReport_4_3_Checked"].ToString(), fs1, false)) { Rowspan = 9, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Point 3", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Liquid end bearing Horizontal", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Point3"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Point 4", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Liquid end Axial", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Point4"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("A", fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Point 6", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Solid end Vertical", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Point6"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Point 7", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Solid end bearing Horizontal", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Point7"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("D", fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Point 8", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Main motor Vertical", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Point8"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Point 9", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Main motor Horizontal", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Point9"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("F", fs1, false)) { Rowspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Point 10", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Secondary motor Vertical", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Point10"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Point 11", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Secondary motor Horizontal", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Testing_Point11"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("TestReport.png", 300f, 100f)) { Colspan = 5, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Vibration admissible /mm/s）", fs1, false)) { Colspan = 5, Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("The vibration admissible  in accordance to WSN 76-0102-50 / WSN 76-0102-50", fs1, false)) { Colspan = 5, Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Test Report 4.4
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("4. 4 Frequency spectrums from scroll and bowl according to WSN76-0103-00. WSN76-0103-00", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Testing_4_4"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Test Report 4.5
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("4. 5 Read and write bearing temperature from control cabinet screen.", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Testing_4_5"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Test Report 4.6
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("4. 6 Check the differential speed range according machine card. ", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Testing_4_6"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Test Report 4.7
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("4. 7 Leakage check of liquid side catcher and decanter hood sealing.", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Testing_4_7"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("Therefore, install the positive regulation plates on test field.\n\nFeed capacity during testing：", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });

                        pdfTable = new PdfPTable(4);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Type", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Feed capacity\n [m3/h]", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Regulation plates diameter\n [mm]", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 2200", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 2200") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("13", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 2200") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("180", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 2200") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["TestReport_Feed_Capacity_Checked"].ToString(), fs1, false)) { Rowspan = 6, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 4000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 4000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("15", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 4000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("210", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 4000") });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 5000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 5000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("20", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 5000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("240", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 5000") });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 5500", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 5500") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("30", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 5500") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("240", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 5500") });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 6000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 6000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("35", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 6000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("280", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 6000") });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 7000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 7000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("50", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 7000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("335", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = getProDecanterBackColor(materialId, "PRO 7000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checking time 15min ", fs1, false)) { Colspan = 4, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Test Report 4.8
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("4.8 Make sure no any water comes out after the threaded plug removed from liquid and solid side. ", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Testing_4_8"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                    }

                    //Uninstall the machine 
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = minHeight });
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("5. Uninstall the machine", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });

                    processData = dtData.AsEnumerable().Where(k => k.Field<string>("ReportType").Equals("UnInstalltheMachine", StringComparison.OrdinalIgnoreCase) && k.Field<string>("Parameter").Equals("5", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    innerTbl = new PdfPTable(1);
                    if (processData != null)
                    {
                        var rowValue = JObject.Parse(processData["Pro_value"].ToString());

                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 400, 150 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Description/", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("5.1 Drain rest water out of the bowl.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthTop = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Uninstall_the_Machine_Checked"].ToString(), fs1, false)) { Rowspan = 11, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("5.1.1 Shut down the machine the first time.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthTop = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("5.1.1.1 Stop the machine, when machine runs to 500prm, then shut off the feed pump and close the valve.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthTop = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("5.1.1.2 Machine runs to 0 rpm.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthTop = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("5.1.1.3 Restart of the machine to 300 rpm, then shutdown.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthTop = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("5.2 Uninstall the machine.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthTop = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("5.2.1 After stop of the machine, turn the main switch to “OFF”.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthTop = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("5.2.2 Eject the machine.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthTop = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("5.2.3 Disconnect the cable between the machine, motor and control cabinet.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthTop = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("5.2.4 Disconnect the feed pipe.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 0, BorderWidthTop = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("5.2.5. Change the positive regulation plate from the test field version to the customer order version before sending  the decanter to final control. ", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, BorderWidthBottom = 1, BorderWidthTop = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    }

                    // Final control 
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0, MinimumHeight = minHeight });
                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("6. Final control", fs2, true)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });

                    processData = dtData.AsEnumerable().Where(k => k.Field<string>("ReportType").Equals("FinalControl", StringComparison.OrdinalIgnoreCase) && k.Field<string>("Parameter").Equals("6.1", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    innerTbl = new PdfPTable(1);
                    if (processData != null)
                    {
                        var rowValue = JObject.Parse(processData["Pro_value"].ToString());

                        //Final Control 6.1
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("6.1 Painting", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable = new PdfPTable(3);
                        pdfTable.SplitLate = false;
                        cellWidth = new int[3] { 150, 400, 150 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Item", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Description", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Color ", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        innerTbl = new PdfPTable(2);
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("Machine card；\nCheck the complete machine, also the inside of the frame and the surface of the gear box, regarding painting surface damages.\nPlease pay attention to keep primer for below positions without final coating: motor feet, bear housing seat both liquid and solid side.", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        innerTbl.AddCell(new PdfPCell(getProDecanterImages("FinalControl_6.1.1.png", 130f, 100f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        innerTbl.AddCell(new PdfPCell(getProDecanterImages("FinalControl_6.1.2.png", 130f, 100f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        innerTbl.AddCell(new PdfPCell(getProDecanterImages("FinalControl_6.1.3.png", 130f, 100f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0, });
                        pdfTable.AddCell(new PdfPCell(innerTbl) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Final_Control_Checked"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                    }

                    processData = dtData.AsEnumerable().Where(k => k.Field<string>("ReportType").Equals("FinalControl", StringComparison.OrdinalIgnoreCase) && k.Field<string>("Parameter").Equals("6", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (processData != null)
                    {
                        var rowValue = JObject.Parse(processData["Pro_value"].ToString());

                        //Final Control 6.2
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("6.2 Adhesive Plate", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable = new PdfPTable(5);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Item", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Description", fs1, false)) { Colspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Plate position", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Adhesive plate assembly drawing", fs1, false)) { Colspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Final_Control_6_2_Checked"].ToString(), fs1, false)) { Rowspan = 5, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Nameplate content", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Nameplate drawing and machine card", fs1, false)) { Colspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Motor plate", fs1, false)) { Rowspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Check whether the motor plate is correct.", fs1, false)) { Colspan = 3, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE, BorderWidthBottom = 0, });

                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("FinalControl_6.2.1.png", 100f, 170f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "Pro 4000/7000") });
                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("FinalControl_6.2.2.png", 100f, 170f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "Pro 5000/5500") });
                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("FinalControl_6.2.3.png", 100f, 170f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0, BackgroundColor = getProDecanterBackColor(materialId, "Pro 2200/6000") });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Pro 4000/7000\n0024-3601-000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, Border = 0, BorderWidthBottom = 1, BackgroundColor = getProDecanterBackColor(materialId, "Pro 4000/7000") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Pro 5000/5500\n0024-3596-000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, Border = 0, BorderWidthBottom = 1, BackgroundColor = getProDecanterBackColor(materialId, "Pro 5000/5500") });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Pro 2200/6000\n0024-3599-000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, Border = 0, BorderWidthBottom = 1, BackgroundColor = getProDecanterBackColor(materialId, "Pro 2200/6000") });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Final Control 6.3
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("6.3 Checking oil level and leakage", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable = new PdfPTable(3);
                        cellWidth = new int[3] { 400, 150, 150 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Description/", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("6.3.1 The oil level on oil surge vessel should be between the high and low mark line.\n6.3.2 Check the oil tank for cracks.\n     6.3.3 Open the protective casing and check for oil leakage. \n      6.3.4 Clean the complete machine from dust and fingerprints.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, });
                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("FinalControl_6.3.png", 100f, 100f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Final_Control_6_3_Checked"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Final Control 6.4
                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("6.4 Fixing of the transport securing devices (solid side and liquid side).  \n    Fixing of bowl, torque the screws with 30 Nm and secure them with the nuts.", fs1, false)) { Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Final_Control_6_4"].ToString())) { Border = 0 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        pdfTable = new PdfPTable(2);
                        cellWidth = new int[2] { 600, 150 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 2200 / 4000 / 5000 / 5500 / 6000 / 7000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("FinalControl_6.4.png", 100f, 100f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Final_Control_Fixing_Bowl_Checked"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });


                        //Final Control 6.5
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("6.5 Spray protective wax to the flange of the gearbox", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable = new PdfPTable(4);
                        cellWidth = new int[4] { 150, 250, 250, 150 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Type/", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Description/", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 4000 / 7000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Where the protective wax is sprayed.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("FinalControl_6.5.png", 100f, 100f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Final_Control_6_5_Checked"].ToString(), fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Final Control 6.6
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("6.6 Spray protective wax to the shaft of motor and gearbox.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable = new PdfPTable(4);
                        cellWidth = new int[4] { 330, 200, 200, 100 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Type/", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Description/", fs1, false)) { Colspan = 2, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Checked", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        innerTbl = new PdfPTable(6);
                        cellWidth = new int[6] { 120, 90, 90, 90, 90, 90 };
                        innerTbl.SetWidths(cellWidth);
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("PRO 2200", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("4000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("5000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("5500", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("6000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });
                        innerTbl.AddCell(new PdfPCell(getPdfCellWithText("7000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });
                        innerTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Shaft_Pro2200"].ToString())) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        innerTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Shaft_Pro4000"].ToString())) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        innerTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Shaft_Pro5000"].ToString())) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        innerTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Shaft_Pro5500"].ToString())) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        innerTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Shaft_Pro6000"].ToString())) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        innerTbl.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Shaft_Pro7000"].ToString())) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(innerTbl) { Rowspan = 6, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("FinalControl_6.6.1.png", 100f, 150f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderWidthBottom = 0, BorderWidthRight = 0, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("FinalControl_6.6.2.png", 100f, 150f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderWidthBottom = 0, BorderWidthLeft = 0, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(rowValue["Final_Control_6_6_Checked"].ToString(), fs1, false)) { Rowspan = 6, PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 4000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 5000 / 5500", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });

                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("FinalControl_6.6.3.png", 100f, 150f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderWidthBottom = 0, BorderWidthRight = 0, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("FinalControl_6.6.4.png", 100f, 150f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderWidthBottom = 0, BorderWidthLeft = 0, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 6000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 7000", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });

                        pdfTable.AddCell(new PdfPCell(getProDecanterImages("FinalControl_6.6.5.png", 100f, 150f)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderWidthBottom = 0, BorderWidthRight = 0, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("PRO 2200\nWhere the protective wax is sprayed.", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0, BorderWidthBottom = 1 });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0, BorderWidthBottom = 1 });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                        //Final Control 6.7
                        mainTbl.AddCell(new PdfPCell(getPdfCellWithText("6.7 Complete documentation available", fs1, false)) { PaddingTop = paddingvalue, PaddingBottom = paddingvalue, Border = 0 });
                        pdfTable = new PdfPTable(2);
                        pdfTable.SplitLate = false;
                        cellWidth = new int[2] { 400, 200 };
                        pdfTable.SetWidths(cellWidth);
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Machine card", fs1, false)) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Final_Control_Machine_Card"].ToString())) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Instruction Manual", fs1, false)) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Final_Control_Instruction_Manual"].ToString())) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Maintenance and Lubrication Schedule", fs1, false)) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        pdfTable.AddCell(new PdfPCell(getCheckUncheckImgCell(rowValue["Final_Control_Maintainance_Lubrication"].ToString())) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });
                        mainTbl.AddCell(new PdfPCell(pdfTable) { Border = 0, PaddingTop = paddingvalue, PaddingBottom = paddingvalue });

                    }
                    finalTbl = new PdfPTable(1);
                    finalTbl.SplitLate = false;
                    finalTbl.WidthPercentage = 100;
                    finalTbl.SpacingBefore = mainTblSpaceBefore;
                    finalTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    finalTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    finalTbl.AddCell(new PdfPCell(mainTbl) { Padding = 10 });
                    pdfDoc.Add(finalTbl);

                    #endregion

                    if (generateOnlyProReport == false)
                    {
                        setProDecanterDocMargin(pdfDoc, false);
                        p2 = new Paragraph();
                        pdfDoc.Add(p2);
                        pdfDoc.NewPage();
                        isHeaderFooterRequired = false;

                        //Noise measurement report
                        mainTbl = NoiseMeasurementReport(ProductionOrder, fabricationNumber, "", 0, out ReportStatus);
                        if (ReportStatus == string.Empty)
                        {

                            pdfDoc.Add(mainTbl);
                            p2 = new Paragraph();
                            pdfDoc.Add(p2);
                            pdfDoc.NewPage();
                        }

                        //Balancing report
                        if (!string.IsNullOrEmpty(balancingPO))
                        {

                            mainTbl = balancingCertificateReportTbl(balancingPO, 0, out ReportStatus);
                            pdfDoc.Add(mainTbl);
                            p2 = new Paragraph();
                            pdfDoc.Add(p2);
                            pdfDoc.NewPage();
                        }

                        //Checklist packing report
                        mainTbl = decanterChecklistPackingReport(ProductionOrder, fabricationNumber, 0, true, out ReportStatus);
                        if (ReportStatus == string.Empty)
                        {

                            pdfDoc.Add(mainTbl);
                        }
                        p2 = new Paragraph();
                        if (pdfDoc.IsOpen())
                        {
                            pdfDoc.Add(p2);
                            pdfDoc.NewPage();
                        }

                        //Checklist Final packing report
                        mainTbl = decanterFinalChecklistPackingReportTbl(ProductionOrder, fabricationNumber, 0, true, out ReportStatus);
                        if (ReportStatus == string.Empty)
                        {

                            pdfDoc.Add(mainTbl);
                        }
                        p2 = new Paragraph();
                        if (pdfDoc.IsOpen())
                        {
                            pdfDoc.Add(p2);
                            pdfDoc.NewPage();
                        }



                        //Add upload pdf
                        try
                        {

                            BindExistingPDFToFinalReport(pdfWriter, pdfDoc, imgPdfPath, "2");

                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog(ex.Message);
                        }
                        addImagesFromFloder(pdfDoc, imgPdfPath);
                    }
                }
                else
                {
                    ReportStatus = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
            }
            return ReportStatus;
        }
        private static void addImagesFromFloder(Document pdfDoc, string imgPdfPath)
        {
            try
            {

                string[] FileArray = Directory.GetFiles(imgPdfPath);
                List<int> imageNumberList = new List<int>();
                foreach (string file in FileArray)
                {
                    if (file.Length > 0)
                    {
                        string imageName = file.Split(new[] { "\\" }, StringSplitOptions.None).Last();
                        imageName = imageName.ToLower();
                        if (!imageName.Contains(".pdf"))
                        {
                            imageName = imageName.Split('.').First();
                            imageNumberList.Add(Convert.ToInt32(imageName));
                        }
                    }
                }
                imageNumberList.Sort();
                for (int i = 0; i < imageNumberList.Count; i++)
                {
                    foreach (string file in FileArray)
                    {
                        string fileInLC = file.ToLower();
                        if (fileInLC.Contains("\\" + imageNumberList[i].ToString() + ".") && (!fileInLC.Contains(imageNumberList[i].ToString() + ".pdf")))
                        {
                            if (File.Exists(file))
                            {
                                Paragraph p2 = new Paragraph();
                                pdfDoc.Add(p2);
                                pdfDoc.NewPage();
                                PdfPTable finalTbl = new PdfPTable(1);
                                finalTbl.SplitLate = false;
                                finalTbl.WidthPercentage = 100;
                                finalTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                finalTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                byte[] file1 = System.IO.File.ReadAllBytes(file);//ImagePath  
                                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(file1);
                                image.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                                PdfPCell imageCell = new PdfPCell(image, true);
                                imageCell.VerticalAlignment = Element.ALIGN_TOP;
                                imageCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                imageCell.Border = 0;
                                finalTbl.AddCell(new PdfPCell(imageCell));
                                pdfDoc.Add(getFinalReportParagraph(finalTbl));
                            }
                            break;
                        }
                    }
                }


            }
            catch (Exception ex)
            { }

        }
        private static PdfPCell getProDecanterImages(string fileName, float width, float height)
        {
            PdfPCell logoCell = new PdfPCell();
            try
            {
                byte[] file1;
                file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/GEA/Images/ProDecanter/" + fileName));//ImagePath  
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(file1);
                image.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                image.ScaleToFit(width, height);
                logoCell = new PdfPCell(image, false);
                logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getProDecanterImages = " + ex.Message);
            }
            return logoCell;
        }
        #endregion


        #region  Quality PC Report
        internal static string QualityPCTimeConsolidatedReport(string MachineID, string fromDate, string toDate)
        {
            string successfull = "";
            try
            {
                string Filename = "QualityPCTimeConsolidatedReport.xlsx";

                string Source = GetReportPath(Filename);
                string Template = "QualityPC_TimeConsolidated_Report" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("QualityPC TimeConsolidated Report template does not exists at - " + Source);
                    successfull = "TemplateNotFound";
                }
                else
                {

                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[1];
                    DataTable dt = GEADatabaseAccess.GetQualityPCTimeConsolidatedReport(MachineID, fromDate, toDate);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        worksheet.Cells["B2"].Value = fromDate;
                        worksheet.Cells["B3"].Value = toDate;
                        worksheet.Cells["B4"].Value = MachineID;
                        int rowCount = 8, colCount = 1;
                        var distDate = dt.AsEnumerable().Select(k => k.Field<DateTime>("Date")).Distinct().ToList();
                        foreach (DateTime date in distDate)
                        {
                            colCount = 1;
                            DataTable datewiseTbl = dt.AsEnumerable().Where(k => k.Field<DateTime>("Date") == date).CopyToDataTable();
                            worksheet.Cells[rowCount, colCount].Value = date.ToString("dd-MM-yyyy");
                            colCount++;
                            setQualityPCReportValue(worksheet, rowCount, colCount, datewiseTbl, "Test Protocol Report");
                            colCount += 2;
                            setQualityPCReportValue(worksheet, rowCount, colCount, datewiseTbl, "Hardness Test Report");
                            colCount += 2;
                            setQualityPCReportValue(worksheet, rowCount, colCount, datewiseTbl, "Dye Penetration Report");
                            colCount += 2;
                            setQualityPCReportValue(worksheet, rowCount, colCount, datewiseTbl, "Non-conformence Report");
                            colCount += 2;
                            setQualityPCReportValue(worksheet, rowCount, colCount, datewiseTbl, "Internal Quality Report");
                            colCount += 2;
                            setQualityPCReportValue(worksheet, rowCount, colCount, datewiseTbl, "Deviation Permit Report");
                            colCount += 2;
                            setQualityPCReportValue(worksheet, rowCount, colCount, datewiseTbl, "First Sample Report");
                            rowCount++;
                        }
                        setThinBorder(worksheet, 8, 1, rowCount - 1, 15);
                        DownloadMultipleFile(destination, Excel.GetAsByteArray());
                        successfull = "Generated";
                    }
                    else
                    {
                        successfull = "NoDataFound";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return successfull;
        }

        internal static string QualityPCYearlyReport(string MachineID, string year)
        {
            string successfull = "";
            try
            {
                string Filename = "QualityPCYearlyReport.xlsx";

                string Source = GetReportPath(Filename);
                string Template = "QualityPC_Yearly_Report" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("QualityPC Yearly Report template does not exists at - " + Source);
                    successfull = "TemplateNotFound";
                }
                else
                {

                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[1];
                    DataTable dt = GEADatabaseAccess.GetQualityPCTYearlyReport(MachineID, year);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        worksheet.Cells["B2"].Value = year;
                        worksheet.Cells["B4"].Value = MachineID;
                        int rowCount = 8, colCount = 1;
                        List<string> months = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().Where(k => !string.IsNullOrEmpty(k)).ToList();
                        foreach (string month in months)
                        {
                            colCount = 1;
                            var rows = dt.AsEnumerable().Where(k => k.Field<string>("month") == month);
                            DataTable yearwiseTbl = new DataTable();
                            if (rows.Any())
                            {
                                yearwiseTbl = rows.CopyToDataTable();
                            }
                            worksheet.Cells[rowCount, colCount].Value = month;
                            if (yearwiseTbl.Rows.Count > 0)
                            {
                                colCount++;
                                setQualityPCReportValue(worksheet, rowCount, colCount, yearwiseTbl, "Test Protocol Report");
                                colCount += 2;
                                setQualityPCReportValue(worksheet, rowCount, colCount, yearwiseTbl, "Hardness Test Report");
                                colCount += 2;
                                setQualityPCReportValue(worksheet, rowCount, colCount, yearwiseTbl, "Dye Penetration Report");
                                colCount += 2;
                                setQualityPCReportValue(worksheet, rowCount, colCount, yearwiseTbl, "Non-conformence Report");
                                colCount += 2;
                                setQualityPCReportValue(worksheet, rowCount, colCount, yearwiseTbl, "Internal Quality Report");
                                colCount += 2;
                                setQualityPCReportValue(worksheet, rowCount, colCount, yearwiseTbl, "Deviation Permit Report");
                                colCount += 2;
                                setQualityPCReportValue(worksheet, rowCount, colCount, yearwiseTbl, "First Sample Report");
                            }
                            rowCount++;
                        }
                        setThinBorder(worksheet, 8, 1, rowCount - 1, 15);
                        DownloadMultipleFile(destination, Excel.GetAsByteArray());
                        successfull = "Generated";
                    }
                    else
                    {
                        successfull = "NoDataFound";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return successfull;
        }
        private static void setQualityPCReportValue(ExcelWorksheet excelWorksheet, int rowCount, int colCount, DataTable dt, string reportName)
        {
            try
            {
                var row = dt.AsEnumerable().Where(k => k.Field<string>("ReportName").Equals(reportName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (row != null)
                {
                    excelWorksheet.Cells[rowCount, colCount].Value = row["NoofReportsPrepared"];
                    excelWorksheet.Cells[rowCount, colCount + 1].Value = row["NoOfReportsCompleted"];
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        #endregion

        #region ---Machine Mix Report----
        internal static string GenerateMachineMixReport(DateTime fromDate, DateTime toDate)
        {
            string successfull = "";
            try
            {
                string Filename = "MachineMixReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "MachineMixReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Machine Mix Report template does not exists at - " + Source);
                    successfull = "TemplateNotFound";
                }
                else
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[1];
                    DataTable dt = GEADatabaseAccess.GetMachineMixDetails(fromDate, toDate, out DataTable dtTotal);
                    if (dt.Rows.Count > 0)
                    {
                        worksheet.Cells["B3"].Value = fromDate.ToString("dd-MMM-yyyy");
                        worksheet.Cells["B4"].Value = toDate.ToString("dd-MMM-yyyy");

                        int row = 6, trow = 6;

                        foreach (DataRow item in dt.Rows)
                        {
                            worksheet.Cells[row, 1].Value = item["FabNo"].ToString();
                            worksheet.Cells[row, 2].Value = item["ModelNo"].ToString();
                            row++;
                        }
                        row--;
                        foreach (DataRow ditem in dtTotal.Rows)
                        {
                            worksheet.Cells[trow, 4].Value = ditem["ModelNo"].ToString();
                            worksheet.Cells[trow, 5].Value = Convert.ToInt32(ditem["FabCount"].ToString());
                            trow++;
                        }
                        trow--;

                        worksheet.Cells["E4"].Value = dtTotal.AsEnumerable().Select(x => x.Field<int>("FabCount")).Sum();
                        worksheet.Cells[6, 1, row, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[6, 1, row, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[6, 1, row, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[6, 1, row, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[6, 1, row, 2].Style.Border.Top.Color.SetColor(Color.Black);
                        worksheet.Cells[6, 1, row, 2].Style.Border.Bottom.Color.SetColor(Color.Black);
                        worksheet.Cells[6, 1, row, 2].Style.Border.Left.Color.SetColor(Color.Black);
                        worksheet.Cells[6, 1, row, 2].Style.Border.Right.Color.SetColor(Color.Black);
                        worksheet.Cells[5, 1, row, 2].AutoFitColumns();

                        worksheet.Cells[6, 4, trow, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[6, 4, trow, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[6, 4, trow, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[6, 4, trow, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[6, 4, trow, 5].Style.Border.Top.Color.SetColor(Color.Black);
                        worksheet.Cells[6, 4, trow, 5].Style.Border.Bottom.Color.SetColor(Color.Black);
                        worksheet.Cells[6, 4, trow, 5].Style.Border.Left.Color.SetColor(Color.Black);
                        worksheet.Cells[6, 4, trow, 5].Style.Border.Right.Color.SetColor(Color.Black);

                    }
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    successfull = "Generated";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return successfull;
        }

        #endregion

        #region -- Parked Order report---
        internal static string ParkedOrderReasonsReport(string CellID, string machineID, DateTime FromDate, DateTime ToDate)
        {
            string successfull = "";
            Logger.WriteDebugLog("Parked Order Reasons Report");
            string time = string.Empty;
            string Filename = "ParkedOrderReport.xlsx";
            string Source = GetReportPath(Filename);
            string Template = "ParkedOrderReport" + DateTime.Now + ".xlsx";
            string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
            if (!File.Exists(Source))
            {
                Logger.WriteDebugLog("ParkedOrderReport template does not found at - \n " + Source);
                successfull = "TemplateNotFound";
            }
            try
            {
                FileInfo newFile = new FileInfo(Source);
                ExcelPackage Excel = new ExcelPackage(newFile, true);
                var WorkSheet = Excel.Workbook.Worksheets[1];
                DataTable dtList = GEADatabaseAccess.GetParkedOrderReasons(CellID, machineID, FromDate, ToDate);
                if (dtList != null && dtList.Rows.Count > 0)
                {
                    WorkSheet.Cells["B3"].Value = Util.GetDateTime(FromDate.ToString("dd-MM-yyyy"));
                    WorkSheet.Cells["B4"].Value = Util.GetDateTime(ToDate.ToString("dd-MM-yyyy"));
                    WorkSheet.Cells["F3"].Value = string.IsNullOrEmpty(CellID) ? "All" : CellID;
                    WorkSheet.Cells["F4"].Value = string.IsNullOrEmpty(machineID) ? "All" : machineID.Replace("'", "");

                    int row = 6, i = 1;
                    foreach (DataRow dtrow in dtList.Rows)
                    {
                        WorkSheet.Cells[row, 1].Value = i++;
                        WorkSheet.Cells[row, 2].Value = dtrow["SNo"];
                        WorkSheet.Cells[row, 3].Value = Util.GetDateTime(dtrow["ParkedTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        WorkSheet.Cells[row, 4].Value = dtrow["MachineID"];
                        WorkSheet.Cells[row, 5].Value = dtrow["ComponentID"];
                        WorkSheet.Cells[row, 6].Value = dtrow["FabricationID"];
                        WorkSheet.Cells[row, 7].Value = dtrow["ParkingPercent"];
                        WorkSheet.Cells[row, 8].Value = dtrow["ReasonForParking"];
                        row++;
                    }
                    row--;
                    WorkSheet.Cells[6, 1, row, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    WorkSheet.Cells[6, 1, row, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    WorkSheet.Cells[6, 1, row, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    WorkSheet.Cells[6, 1, row, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    WorkSheet.Cells[6, 1, row, 8].Style.Border.Top.Color.SetColor(Color.Black);
                    WorkSheet.Cells[6, 1, row, 8].Style.Border.Right.Color.SetColor(Color.Black);
                    WorkSheet.Cells[6, 1, row, 8].Style.Border.Left.Color.SetColor(Color.Black);
                    WorkSheet.Cells[6, 1, row, 8].Style.Border.Bottom.Color.SetColor(Color.Black);
                    WorkSheet.Cells[5, 1, row, 8].AutoFitColumns();
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    Logger.WriteDebugLog("Parked Order Reasons Report Generated.");
                    successfull = "Generated";

                }
                else successfull = "NoDataFound";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return successfull;
        }


        #endregion

        #region-----Monthly Operator efficiency report------------
        internal static string MonthlyOperatorEfficiencyReport(string startTime, string endTime, string shift, string plantID, string operatorID)
        {
            string successful = "";
            try
            {
                string Filename = "MonthlyOperatorEfficiencyReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "MonthlyOperatorEfficiencyReport" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Monthly Operator Efficiency Report template does not exists at - " + Source);
                    successful = "TemplateNotFound";
                }
                else
                {
                    int rowStart = 3;
                    int col = 1;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                    var worksheet = excelPackage.Workbook.Worksheets[1];
                    DataTable dt = GEADatabaseAccess.GetMonthlyOperatorEfficiencyData(startTime, endTime, shift, plantID, operatorID);
                    List<string> dynCols = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                    int headercol = 5;
                    for (int i = 5; i < dynCols.Count - 1; i++)
                    {
                        //worksheet.InsertColumn(headercol, 1, headercol);
                        string headers = dynCols[i];
                        worksheet.Cells[rowStart, headercol].Value = headers;
                        worksheet.Column(headercol).Width = 16;

                        headercol++;
                    }

                    worksheet.DeleteColumn(headercol);

                    rowStart++;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        worksheet.Cells["E2"].Value = startTime;
                        worksheet.Cells["H2"].Value = endTime;
                        worksheet.Cells["N2"].Value = shift;


                        //worksheet.Cells["I3"].Value = ToDate;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            col = 1;
                            worksheet.Cells[rowStart, col].Value = dt.Rows[i]["PlantID"];
                            col++;
                            worksheet.Cells[rowStart, col].Value = dt.Rows[i]["OperatorName"];
                            col++;
                            worksheet.Cells[rowStart, col].Value = dt.Rows[i]["Dept"];
                            col++;
                            worksheet.Cells[rowStart, col].Value = dt.Rows[i]["PrevYearOEE"];
                            col++;
                            for (int j = 5; j < dynCols.Count - 1; j++)
                            {
                                worksheet.Cells[rowStart, col].Value = dt.Rows[i][j];
                                col++;
                            }
                            worksheet.Cells[rowStart, col].Value = dt.Rows[i]["TargetValue"];
                            worksheet.Cells[3, 4, rowStart, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[3, 4, rowStart, 4].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(31, 185, 35));
                            worksheet.Cells[3, 17, rowStart, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[3, 17, rowStart, 17].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(31, 185, 35));
                            rowStart++;

                        }
                        rowStart--;
                        worksheet.Cells[3, 1, rowStart, 17].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[3, 1, rowStart, 17].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[3, 1, rowStart, 17].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[3, 1, rowStart, 17].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[3, 1, rowStart, 17].Style.Border.Top.Color.SetColor(Color.Black);
                        worksheet.Cells[3, 1, rowStart, 17].Style.Border.Right.Color.SetColor(Color.Black);
                        worksheet.Cells[3, 1, rowStart, 17].Style.Border.Left.Color.SetColor(Color.Black);
                        worksheet.Cells[3, 1, rowStart, 17].Style.Border.Bottom.Color.SetColor(Color.Black);
                        //worksheet.Cells[3, 1, rowStart, 17].AutoFitColumns();
                        DownloadMultipleFile(destination, excelPackage.GetAsByteArray());
                        successful = "Download Successful";
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return successful;
        }
        #endregion

        #region-----------Model std.Time vs Actual--------------
        internal static string ModelstdTimevsActualReport(string fromDate, string ToDate, string prodNum, string FabNum, string processType)
        {
            string successful = "";
            try
            {
                string Filename = "ModelStdvsActualReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "ModelStdvsActualReport" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Model std.Time vs Actual Report template does not exists at - " + Source);
                    successful = "TemplateNotFound";
                }
                else
                {

                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                    var worksheet = excelPackage.Workbook.Worksheets[1];
                    DataTable dt = GEADatabaseAccess.GetModelStdVsActualData(fromDate, ToDate, prodNum, FabNum, processType);
                    var values = dt.AsEnumerable().Select(x => x.Field<string>("MachineID")).Distinct().ToList();
                    int row = 6;
                    string timeformat = GEADatabaseAccess.getTimeFormatFromCockpit();
                    DataTable dtvalues = new DataTable();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        worksheet.Cells["B3"].Value = fromDate;
                        worksheet.Cells["D3"].Value = ToDate;
                        worksheet.Cells["F3"].Value = processType;
                        foreach (var item in values)
                        {
                            dtvalues = dt.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(item)).CopyToDataTable();
                            int dtcount = dtvalues.Rows.Count;
                            worksheet.Cells[row, 1, row + dtcount - 1, 1].Merge = true;
                            worksheet.Cells[row, 1, row + dtcount - 1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells[row, 1].Value = item;
                            foreach (DataRow dtrow in dtvalues.Rows)
                            {
                                worksheet.Cells[row, 2].Value = dtrow["ComponentID"];
                                worksheet.Cells[row, 3].Value = dtrow["OperationDescription"];
                                setTimeSpanFormat(timeformat, worksheet, row, 4, dtrow["StdTime"].ToString());
                                setTimeSpanFormat(timeformat, worksheet, row, 5, dtrow["ActualTime"].ToString());
                                worksheet.Cells[row, 6].Value = Math.Round(Convert.ToDouble(dtrow["ProdQty"]), 2);
                                row++;
                            }
                        }
                        row--;
                        worksheet.Cells[5, 1, row, 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[5, 1, row, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[5, 1, row, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[5, 1, row, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[5, 1, row, 6].Style.Border.Top.Color.SetColor(Color.Black);
                        worksheet.Cells[5, 1, row, 6].Style.Border.Right.Color.SetColor(Color.Black);
                        worksheet.Cells[5, 1, row, 6].Style.Border.Left.Color.SetColor(Color.Black);
                        worksheet.Cells[5, 1, row, 6].Style.Border.Bottom.Color.SetColor(Color.Black);
                        //worksheet.Cells[4, 1, row, 5].AutoFitColumns();
                        DownloadMultipleFile(destination, excelPackage.GetAsByteArray());
                        Logger.WriteDebugLog("Model std.Time vs Actual Report Generated.");
                        successful = "Generated";
                    }
                    else successful = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return successful;
        }
        public static void setTimeSpanFormat(string timeFormat, ExcelWorksheet worksheet, int rowPos, int colPos, string value)
        {
            try
            {
                if (timeFormat.Equals("hh:mm:ss"))
                {
                    //TimeSpan timeSpan = TimeSpan.Parse(dt.Rows[i][item].ToString());
                    var valueSplit = value.Split(':');
                    TimeSpan timeSpan = new TimeSpan(int.Parse(valueSplit[0]),    // hours
                                                     int.Parse(valueSplit[1]),    // minutes
                                                     int.Parse(valueSplit[2]));
                    worksheet.Cells[rowPos, colPos].Value = timeSpan;
                    worksheet.Cells[rowPos, colPos].Style.Numberformat.Format = "[h]:mm:ss";
                }
                else
                {
                    worksheet.Cells[rowPos, colPos].Value = string.IsNullOrEmpty(value) ? 0 : Convert.ToDouble(value);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("setTimeSpanFormat = " + ex.Message);
            }
        }
        #endregion
        #region------MachineWise Assembly Report----------
        internal static string MachineWiseAssemblyReport(string fabNum)
        {
            string successful = "";
            try
            {
                string Filename = "MachineWiseAssemblyReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "MachineWiseAssemblyReport" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("MachineWise Assembly Report template does not exists at - " + Source);
                    successful = "TemplateNotFound";
                }
                else
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                    var worksheet = excelPackage.Workbook.Worksheets[1];
                    DataTable dt = GEADatabaseAccess.GetMachineWiseAssemblyData(fabNum, out DataTable data);
                    var values = dt.AsEnumerable().Select(x => x.Field<string>("MachineID")).Distinct().ToList();
                    //var Assem = dt.AsEnumerable().Select(x => x.Field<string>("MachineID").Contains("Assembly")).Distinct().ToList();
                    int rowstart = 7;
                    DataTable dtvalues = new DataTable();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        worksheet.Cells["B4"].Value = fabNum;
                        foreach (var item in values)
                        {

                            //if (item.Contains("Assembly"))
                            //{
                            //    dtvalues = dt.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(item)).CopyToDataTable();

                            //}

                            //else if (item.Equals("Testing") || item.Equals("Packing"))
                            //{
                            //    dtvalues = dt.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(item)).CopyToDataTable();
                            //    worksheet.Cells[rowstart, 1, rowstart, 4].Merge = true;
                            //    worksheet.Cells[rowstart, 1].Style.Font.Bold = true;
                            //    worksheet.Cells[rowstart, 1].Style.Font.Color.SetColor(Color.Black);
                            //    worksheet.Cells[rowstart, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            //    worksheet.Cells[rowstart, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            //    worksheet.Cells[rowstart, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(208, 206, 206));
                            //    worksheet.Cells[rowstart, 1].Value = item;
                            //    rowstart++;

                            dtvalues = dt.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(item)).CopyToDataTable();
                            worksheet.Cells[rowstart, 1, rowstart, 4].Merge = true;
                            worksheet.Cells[rowstart, 1].Style.Font.Bold = true;
                            worksheet.Cells[rowstart, 1].Style.Font.Color.SetColor(Color.Black);
                            worksheet.Cells[rowstart, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[rowstart, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowstart, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(208, 206, 206));
                            string[] val = item.Split('-');
                            worksheet.Cells[rowstart, 1].Value = val[0] + " " + "Process";
                            rowstart++;

                            foreach (DataRow dtrow in dtvalues.Rows)
                            {
                                worksheet.Cells[rowstart, 1].Value = dtrow["Activity"];
                                worksheet.Cells[rowstart, 2].Value = dtrow["StdCycleTime"];
                                worksheet.Cells[rowstart, 3].Value = dtrow["ActualTime"];
                                worksheet.Cells[rowstart, 4].Value = dtrow["ICDReason"];
                                rowstart++;
                            }
                            //rowstart++;

                        }
                        rowstart++;
                        worksheet.Cells[rowstart, 1, rowstart, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowstart, 1, rowstart, 4].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 0, 255));
                        worksheet.Cells[rowstart, 1, rowstart, 4].Style.Font.Bold = true;
                        foreach (DataRow dtRows in data.Rows)
                        {
                            worksheet.Cells[rowstart, 1, rowstart, 4].Style.Font.Color.SetColor(Color.White);
                            worksheet.Cells[rowstart, 1].Value = " Assembly Process Cycle Start";
                            worksheet.Cells[rowstart + 1, 1].Value = Util.GetDateTime(dtRows["AssemblyST"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                            worksheet.Cells[rowstart, 2].Value = " Packing process Cycle End";
                            worksheet.Cells[rowstart + 1, 2].Value = Util.GetDateTime(dtRows["PackingET"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                            worksheet.Cells[rowstart, 3].Value = "Average Days To Complete The Machine";
                            worksheet.Cells[rowstart + 1, 3].Value = dtRows["AvgDays"];
                            worksheet.Cells[rowstart, 3, rowstart, 4].Merge = true;
                            worksheet.Cells[rowstart + 1, 3, rowstart + 1, 4].Merge = true;

                        }
                    }
                    rowstart--;
                    worksheet.Cells[7, 1, rowstart + 2, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[7, 1, rowstart + 2, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[7, 1, rowstart + 2, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[7, 1, rowstart + 2, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[7, 1, rowstart + 2, 4].Style.Border.Top.Color.SetColor(Color.Black);
                    worksheet.Cells[7, 1, rowstart + 2, 4].Style.Border.Right.Color.SetColor(Color.Black);
                    worksheet.Cells[7, 1, rowstart + 2, 4].Style.Border.Left.Color.SetColor(Color.Black);
                    worksheet.Cells[7, 1, rowstart + 2, 4].Style.Border.Bottom.Color.SetColor(Color.Black);
                    //worksheet.Cells[rowstart, 3].AutoFitColumns();
                    DownloadMultipleFile(destination, excelPackage.GetAsByteArray());
                    Logger.WriteDebugLog("Model std.Time vs Actual Report Generated.");
                    successful = "Generated";

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return successful;
        }
        #endregion


        #region Maintenance Checklist Report - GEA
        internal static bool GenerateMaterialTrackingData(string fromDate, string toDate, List<MaterialTracking_GEA> list)
        {
            bool successfull = false;
            try
            {
                string Filename = "StoresTracebilityReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "StoresTracebilityReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Material Tracking Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    int rowCount = 6;
                    int rowCountStart = rowCount;
                    int colCount = 1;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[1];
                    worksheet.Cells["B3"].Value = fromDate;
                    worksheet.Cells["D3"].Value = toDate;

                    System.Drawing.Image img = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(getGEALogoPath()));
                    ExcelPicture pic = worksheet.Drawings.AddPicture("geaLogo", img);
                    pic.SetPosition(0, 0, 0, 0);
                    pic.SetSize(280, 52);

                    for (int i = 0; i < list.Count; i++)
                    {
                        colCount = 1;
                        worksheet.Cells[rowCount, colCount].Value = list[i].SNo;
                        colCount++;
                        worksheet.Cells[rowCount, colCount].Value = list[i].MachineID;
                        colCount++;
                        worksheet.Cells[rowCount, colCount].Value = list[i].DateOfSchedule;
                        colCount++;
                        worksheet.Cells[rowCount, colCount].Value = list[i].ProductionOrderNo;
                        colCount++;
                        worksheet.Cells[rowCount, colCount].Value = list[i].CompID;
                        colCount++;
                        worksheet.Cells[rowCount, colCount].Value = list[i].seriesNo;
                        colCount++;
                        worksheet.Cells[rowCount, colCount].Value = list[i].CycleTime;
                        colCount++;
                        worksheet.Cells[rowCount, colCount].Value = list[i].TimeWaitingAtStores;
                        colCount++;
                        worksheet.Cells[rowCount, colCount].Value = list[i].ReceiverName;
                        colCount++;
                        worksheet.Cells[rowCount, colCount].Value = list[i].Status;
                        colCount++;
                        worksheet.Cells[rowCount, colCount].Value = list[i].DateTimeCompletion;
                        rowCount++;
                    }
                    setThinBorder(worksheet, rowCountStart, 1, rowCount - 1, colCount);


                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    successfull = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return successfull;
        }
        #endregion

        #region --- Picking list report / Missing item
        internal static string GeneratePickingListReport(string productionOrder, string fabricationNo)
        {
            string ReportStatus = string.Empty;
            try
            {
                DataTable headerData = GEADatabaseAccess.getPickingReportHeaderData(productionOrder);
                string componentID = GEADatabaseAccess.getPickinglistComponentID(productionOrder, fabricationNo);
                List<MissingItemEntity> list = GEADatabaseAccess.getPickingListReport(productionOrder, fabricationNo, componentID);
                string formatNo = "", RevNo = "";
                if (list.Count > 0)
                {
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();


                    PdfPTable mainTbl = new PdfPTable(1);
                    mainTbl.SplitLate = false;
                    mainTbl.WidthPercentage = 100;

                    PdfPTable pdfTable = new PdfPTable(3);
                    pdfTable.WidthPercentage = 100;
                    int[] tblCellWidth = { 200, 600, 200 };
                    pdfTable.SetWidths(tblCellWidth);

                    byte[] file1;
                    file1 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/GEA/Icons/GEALogo.jpg"));//ImagePath  
                    iTextSharp.text.Image geaLogo = iTextSharp.text.Image.GetInstance(file1);
                    geaLogo.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    geaLogo.ScaleToFit(110f, 90f);
                    PdfPCell logoCell = new PdfPCell(geaLogo, false);
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfTable.AddCell(new PdfPCell(logoCell) { });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Picking Item", 15)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    PdfPTable formateDataTbl = new PdfPTable(1);
                    formateDataTbl.WidthPercentage = 100;
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Format No: " + formatNo)) { Border = 0 });
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Rev No: " + RevNo)) { Border = 0 });
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText("Date: " + "04-Mar-2022")) { Border = 0 });
                    pdfTable.AddCell(new PdfPCell(formateDataTbl) { HorizontalAlignment = Element.ALIGN_LEFT, Colspan = 2 });


                    //machine details

                    PdfPTable machieModelTbl = new PdfPTable(1);
                    machieModelTbl.WidthPercentage = 100;
                    machieModelTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Machine No", 9)));
                    machieModelTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Model", 9)));
                    machieModelTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("", 9)));
                    pdfTable.AddCell(new PdfPCell(machieModelTbl) { });

                    PdfPTable machineScheduleTbl = new PdfPTable(4);
                    machineScheduleTbl.TotalWidth = 600;
                    int[] tblCellWidth6 = { 200, 100, 100, 200 };
                    machineScheduleTbl.SetWidths(tblCellWidth6);
                    machineScheduleTbl.WidthPercentage = 40;
                    string machineType = componentID.ToLower().Contains("pro") ? "PRO Model" : "NonPRO Model";
                    if (headerData.Rows.Count > 0)
                    {
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Type")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineType)) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Production Order")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(productionOrder)) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Machine No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Scroll No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Bowl No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["ScrollWelded"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Order No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["SaleOrder"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Customer")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["Customer"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Country")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(headerData.Rows[0]["Location"].ToString())) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                    }
                    else
                    {
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Type")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(machineType)) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Production Order")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText(productionOrder)) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Machine No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Scroll No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Bowl No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Order No")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Customer")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("Country")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                        machineScheduleTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { Border = 0 });
                    }
                    pdfTable.AddCell(new PdfPCell(machineScheduleTbl) { Colspan = 2 });

                    mainTbl.AddCell(new PdfPCell(pdfTable));





                    mainTbl.AddCell(new PdfPCell(getPdfCellWithText("")) { MinimumHeight = 6, Border = 0 });
                    iTextSharp.text.BaseColor backColor = new iTextSharp.text.BaseColor(45, 53, 73);
                    int fs1 = 7;
                    int padding = 1;
                    pdfTable = new PdfPTable(9);
                    pdfTable.WidthPercentage = 100;
                    pdfTable.SplitLate = false;
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Item", fs1, true, BaseColor.WHITE)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = padding, BackgroundColor = backColor });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Material ID", fs1, true, BaseColor.WHITE)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = padding, BackgroundColor = backColor });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Material Description", fs1, true, BaseColor.WHITE)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = padding, BackgroundColor = backColor });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Qty", fs1, true, BaseColor.WHITE)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = padding, BackgroundColor = backColor });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Date of Missing", fs1, true, BaseColor.WHITE)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = padding, BackgroundColor = backColor });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Is Issued", fs1, true, BaseColor.WHITE)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = padding, BackgroundColor = backColor });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Status", fs1, true, BaseColor.WHITE)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = padding, BackgroundColor = backColor });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Date of ReIssue", fs1, true, BaseColor.WHITE)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = padding, BackgroundColor = backColor });
                    pdfTable.AddCell(new PdfPCell(getPdfCellWithText("Operator", fs1, true, BaseColor.WHITE)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = padding, BackgroundColor = backColor });
                    foreach (MissingItemEntity data in list)
                    {
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(data.Item, fs1, false, BaseColor.BLACK)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = padding });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(data.MaterialID, fs1, false, BaseColor.BLACK)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = padding });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(data.MaterialDesc, fs1, false, BaseColor.BLACK)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = padding });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(data.Qty, fs1, false, BaseColor.BLACK)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = padding });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(data.DateOfMissing, fs1, false, BaseColor.BLACK)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = padding });
                        if (data.IsIssued == true)
                        {
                            PdfPTable innerTbl = new PdfPTable(1);
                            innerTbl.WidthPercentage = 100;
                            innerTbl.SplitLate = false;
                            innerTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false, BaseColor.BLACK)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new iTextSharp.text.BaseColor(29, 196, 73), });
                            pdfTable.AddCell(new PdfPCell(innerTbl) { Padding = 2 });
                        }
                        else
                        {
                            PdfPTable innerTbl = new PdfPTable(1);
                            innerTbl.WidthPercentage = 100;
                            innerTbl.SplitLate = false;
                            innerTbl.AddCell(new PdfPCell(getPdfCellWithText("", fs1, false, BaseColor.BLACK)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255), });
                            pdfTable.AddCell(new PdfPCell(innerTbl) { Padding = 2 });
                        }
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(data.Status, fs1, false, BaseColor.BLACK)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = padding });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(data.DateOfReIssue, fs1, false, BaseColor.BLACK)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = padding });
                        pdfTable.AddCell(new PdfPCell(getPdfCellWithText(data.Operator, fs1, false, BaseColor.BLACK)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = padding });
                    }
                    mainTbl.AddCell(new PdfPCell(pdfTable));

                    pdfDoc.Add(mainTbl);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=PickingList-" + componentID + "-" + fabricationNo + ".pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    //Response.End();
                    HttpContext.Current.Response.Flush();
                    ReportStatus = "Generated";
                }
            }
            catch (Exception ex)
            {
                ReportStatus = "Failed";
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return ReportStatus;
        }
        private static PdfPCell getPdfCellWithText(string value, int size, bool bold, BaseColor fontColor)
        {
            iTextSharp.text.Font font = null;
            if (bold)
            {
                font = FontFactory.GetFont("Calibri (Body)", 10, iTextSharp.text.Font.BOLD);
            }
            else
            {
                font = FontFactory.GetFont("Calibri (Body)", 10);
            }
            Chunk chunk = new Chunk(value, font);
            chunk.Font.Color = fontColor;
            chunk.Font.Size = size;
            Phrase phrase = new Phrase();
            phrase.Add(chunk);
            PdfPCell cell = new PdfPCell(phrase);
            cell.ExtraParagraphSpace = 3;
            //cell.BorderColor = getPdfCellBorderColor();
            return cell;
        }
        #endregion
        #region --- Picking list report / Completed
        internal static string GeneratePickingListCompleteReport(string productionOrder, string fabricationNo)
        {
            string successful = "";
            try
            {
                string Filename = "PickingListCompleteTemplate.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "PickingListCompleteTemplate" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("PickingListCompleteTemplate does not exists at - " + Source);
                    successful = "TemplateNotFound";
                }
                else
                {

                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                    var worksheet = excelPackage.Workbook.Worksheets[1];
                    DataTable headerData = GEADatabaseAccess.getPickingReportHeaderData(productionOrder);
                    string componentID = GEADatabaseAccess.getPickinglistComponentID(productionOrder, fabricationNo);
                    List<MissingItemEntity> list = GEADatabaseAccess.getPickingListReport(productionOrder, fabricationNo, componentID);
                    string machineType = componentID.ToLower().Contains("pro") ? "PRO Model" : "NonPRO Model";

                    int rowCount = 11;
                    int startRowCount = rowCount;
                    int colCount = 1;
                    string timeformat = GEADatabaseAccess.getTimeFormatFromCockpit();
                    DataTable dtvalues = new DataTable();
                    if (list.Count > 0)
                    {
                        worksheet.Cells["D2"].Value = machineType;
                        worksheet.Cells["D3"].Value = productionOrder;
                        if (headerData.Rows.Count > 0)
                        {
                            worksheet.Cells["D4"].Value = headerData.Rows[0]["ScrollWelded"].ToString();
                            worksheet.Cells["D5"].Value = headerData.Rows[0]["ScrollWelded"].ToString();
                            worksheet.Cells["D6"].Value = headerData.Rows[0]["ScrollWelded"].ToString();
                            worksheet.Cells["D7"].Value = headerData.Rows[0]["SaleOrder"].ToString();
                            worksheet.Cells["D8"].Value = headerData.Rows[0]["Customer"].ToString();
                            worksheet.Cells["D9"].Value = headerData.Rows[0]["Location"].ToString();
                        }
                        foreach (MissingItemEntity data in list)
                        {
                            colCount = 1;
                            worksheet.Cells[rowCount, colCount].Value = data.Item;
                            colCount++;
                            worksheet.Cells[rowCount, colCount].Value = data.MaterialID;
                            colCount++;
                            worksheet.Cells[rowCount, colCount].Value = data.MaterialDesc;
                            colCount++;
                            worksheet.Cells[rowCount, colCount].Value = data.Qty;
                            colCount++;
                            worksheet.Cells[rowCount, colCount].Value = data.DateOfMissing;
                            colCount++;
                            if (data.IsIssued == true)
                            {
                                worksheet.Cells[rowCount, colCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[rowCount, colCount].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(29, 196, 73));
                            }
                            else
                            {
                                worksheet.Cells[rowCount, colCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[rowCount, colCount].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 0, 0));
                            }
                            colCount++;
                            worksheet.Cells[rowCount, colCount].Value = data.Status;
                            colCount++;
                            worksheet.Cells[rowCount, colCount].Value = data.DateOfReIssue;
                            colCount++;
                            worksheet.Cells[rowCount, colCount].Value = data.OperatorName;
                            rowCount++;
                        }
                        setThinBorder(worksheet, startRowCount, 1, rowCount - 1, colCount);
                        successful = "Generated";
                        DownloadMultipleFile(destination, excelPackage.GetAsByteArray());
                    }
                    else successful = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return successful;
        }
        #endregion

        #region --- Picking list report / Missing
        internal static string GeneratePickingListMissingReport(string productionOrder, string fabricationNo)
        {
            string successful = "";
            try
            {
                string Filename = "PickingListMissingTemplate.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "PickingListMissingReport" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("PickingListMissingTemplate does not exists at - " + Source);
                    successful = "TemplateNotFound";
                }
                else
                {

                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                    var worksheet = excelPackage.Workbook.Worksheets[1];
                    DataTable headerData = GEADatabaseAccess.getPickingReportHeaderData(productionOrder);
                    string componentID = GEADatabaseAccess.getPickinglistComponentID(productionOrder, fabricationNo);
                    List<MissingItemEntity> list = GEADatabaseAccess.getPickingListMissingReport(productionOrder, fabricationNo, componentID);
                    string machineType = componentID.ToLower().Contains("pro") ? "PRO Model" : "NonPRO Model";

                    int rowCount = 11;
                    int startRowCount = rowCount;
                    int colCount = 1;
                    string timeformat = GEADatabaseAccess.getTimeFormatFromCockpit();
                    DataTable dtvalues = new DataTable();
                    if (list.Count > 0)
                    {
                        worksheet.Cells["D2"].Value = machineType;
                        worksheet.Cells["D3"].Value = productionOrder;
                        if (headerData.Rows.Count > 0)
                        {
                            worksheet.Cells["D4"].Value = headerData.Rows[0]["ScrollWelded"].ToString();
                            worksheet.Cells["D5"].Value = headerData.Rows[0]["ScrollWelded"].ToString();
                            worksheet.Cells["D6"].Value = headerData.Rows[0]["ScrollWelded"].ToString();
                            worksheet.Cells["D7"].Value = headerData.Rows[0]["SaleOrder"].ToString();
                            worksheet.Cells["D8"].Value = headerData.Rows[0]["Customer"].ToString();
                            worksheet.Cells["D9"].Value = headerData.Rows[0]["Location"].ToString();
                        }
                        foreach (MissingItemEntity data in list)
                        {
                            colCount = 1;
                            worksheet.Cells[rowCount, colCount].Value = data.Item;
                            colCount++;
                            worksheet.Cells[rowCount, colCount].Value = data.MaterialID;
                            colCount++;
                            worksheet.Cells[rowCount, colCount].Value = data.MaterialDesc;
                            colCount++;
                            worksheet.Cells[rowCount, colCount].Value = data.Qty;
                            colCount++;
                            worksheet.Cells[rowCount, colCount].Value = data.ShortageQty;
                            colCount++;
                            worksheet.Cells[rowCount, colCount].Value = data.DateOfMissing;
                            colCount++;
                            if (data.IsIssued == true)
                            {
                                worksheet.Cells[rowCount, colCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[rowCount, colCount].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(29, 196, 73));
                            }
                            else
                            {
                                worksheet.Cells[rowCount, colCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[rowCount, colCount].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 0, 0));
                            }
                            colCount++;
                            worksheet.Cells[rowCount, colCount].Value = data.Status;
                            colCount++;
                            worksheet.Cells[rowCount, colCount].Value = data.DateOfReIssue;
                            colCount++;
                            worksheet.Cells[rowCount, colCount].Value = data.OperatorName;
                            rowCount++;
                        }
                        setThinBorder(worksheet, startRowCount, 1, rowCount - 1, colCount);
                        successful = "Generated";
                        DownloadMultipleFile(destination, excelPackage.GetAsByteArray());
                    }
                    else successful = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return successful;
        }
        #endregion

        #region --- Pickinglist import template
        internal static string GeneratePickingListMasterImportReport()
        {
            string successful = "";
            try
            {
                string Filename = "PickingListImportTemplate.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "PickingListImportTemplate" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("PickingListImportTemplate does not exists at - " + Source);
                    successful = "TemplateNotFound";
                }
                else
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                    DownloadMultipleFile(destination, excelPackage.GetAsByteArray());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return successful;
        }
        #endregion
    }
}