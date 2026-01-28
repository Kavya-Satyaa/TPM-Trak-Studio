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
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Denso.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Denso
{
    public class ReportsDenso
    {
        static string appPath = HttpContext.Current.Server.MapPath("~/Denso/ReportTemplate");
        //string appPath= D:\Renu\DENSO\TPM-TrakAnalytics\Web_TPMTrakDashboard\Denso\Reports;
        public static string SafeFileName(string name)
        {
            StringBuilder str = new StringBuilder(name);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.ToString();
        }
        public static string GetReportPath(string reportName)
        {
            string src;
            if (HttpContext.Current.Session["Language"] == null)
                src = Path.Combine(appPath, reportName);
            else
            {
                if (HttpContext.Current.Session["Language"].ToString() != "en")
                    src = Path.Combine(appPath, reportName);
                else
                    src = Path.Combine(appPath, reportName);
            }
            return src;
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
                Logger.WriteErrorLog(ex);
            }
        }
        private static void setThinBorder(ExcelWorksheet worksheet, int fromRow, int fromColumn, int toRow, int toColumn)
        {
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
        }
        #region---------Static Accuracy Report Denso----------------
        internal static string GetStaticAccuracyReport(string Machine, string Year, string Month)
        {
            string generated = "Generated";
            try
            {
                string time = string.Empty;
                string Filename = "StaticAccuracyReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "StaticAccuracyReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("StaticAccuracyReport template does not found at - \n " + Source);
                }
                else
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[1];
                    worksheet.PrinterSettings.PaperSize = ePaperSize.A4;
                    worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                    worksheet.PrinterSettings.HorizontalCentered = true;
                    worksheet.PrinterSettings.FitToWidth = 1;
                    worksheet.PrinterSettings.FitToHeight = 0;
                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.TopMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.BottomMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.LeftMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.RightMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.HeaderMargin = 0;
                    worksheet.PrinterSettings.FooterMargin = 0;
                    worksheet.PrinterSettings.ShowGridLines = true;
                    worksheet.PrinterSettings.RepeatRows = new ExcelAddress("1:3");
                    DataTable dt = DensoDBAccess.getStaticAccuracyTransactionDetails(Machine, Year, Month, "Report");
                    var distMachine = dt.AsEnumerable().Select(k => k["MachineID"].ToString()).Distinct().ToList();
                    var distYear = dt.AsEnumerable().Select(k => k["Year"].ToString()).Distinct().ToList();
                    var distMonth = dt.AsEnumerable().Select(k => k["Month"].ToString()).Distinct().ToList();
                    var distWeeks = dt.AsEnumerable().Select(k => k["WeekNo"].ToString()).Distinct().ToList();
                    if (dt.Rows.Count > 0)
                    {
                        int rowpos = 4; int colpos = 1;
                        worksheet.Cells["A1"].Value = "STATIC ACCURACY SHEET";

                        //worksheet.Cells["B2"].Value = Year + "-" + Month;
                        worksheet.Cells["B3"].Value = DateTime.Now.ToString("dd-MMM-yyyy");
                        for (int machineC = 0; machineC < distMachine.Count; machineC++)
                        {
                            DataTable dtMachines = dt.AsEnumerable().Where(k => k["MachineID"].ToString().Equals(distMachine[machineC], StringComparison.OrdinalIgnoreCase)).CopyToDataTable();
                            var distChekpoint = dtMachines.AsEnumerable().Select(k => k["Checkpoint"].ToString()).Distinct().ToList();
                            int i = 0;
                            foreach (string checkpoint in distChekpoint)
                            {
                                colpos = 1;
                                var firstRow = dtMachines.AsEnumerable().Where(k => k["Checkpoint"].ToString().Equals(checkpoint, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                worksheet.Cells[rowpos, colpos].Value = firstRow["MachineID"];
                                if (i == 0)
                                {
                                    worksheet.Cells[rowpos, colpos, rowpos + distChekpoint.Count - 1, colpos].Merge = true;
                                }
                                colpos++;
                                worksheet.Cells[rowpos, colpos].Value = firstRow["Checkpoint"];
                                colpos++;
                                foreach (string month in distMonth)
                                {
                                    var distWeek = dtMachines.AsEnumerable().Where(k => k["Checkpoint"].ToString().Equals(checkpoint, StringComparison.OrdinalIgnoreCase) && k["Month"].ToString() == month).Select(k => k["WeekNo"].ToString()).Distinct().ToList();
                                    if (machineC == 0 && i == 0)
                                    {
                                        worksheet.Cells[2, colpos].Value = HelperClassGeneric.getAbbreviatedMonthName(month);
                                        worksheet.Cells[2, colpos, 2, colpos + distWeek.Count - 1].Merge = true;
                                        worksheet.Cells[2, colpos, 2, colpos + distWeek.Count - 1].Style.Font.Bold.ToString();
                                        worksheet.Cells[2, colpos, 2, colpos + distWeek.Count - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    }
                                    var dataValueRows = dtMachines.AsEnumerable().Where(k => k["Checkpoint"].ToString().Equals(checkpoint, StringComparison.OrdinalIgnoreCase) && k["Month"].ToString() == month).ToList();
                                    foreach (DataRow valueRow in dataValueRows)
                                    {
                                        if (machineC == 0 && i == 0)
                                        {
                                            worksheet.Cells[3, colpos].Value = "W" + valueRow["WeekNo"];
                                        }
                                        worksheet.Cells[rowpos, colpos].Value = valueRow["Value"];
                                        colpos++;
                                    }
                                }
                                rowpos++;
                                i++;
                            }
                        }
                        colpos--;
                        worksheet.Cells[rowpos, 1].Value = "TM SIGN";
                        worksheet.Cells[rowpos, 1, rowpos, 2].Merge = true;
                        rowpos++;
                        worksheet.Cells[rowpos, 1].Value = "TL SIGN";
                        worksheet.Cells[rowpos, 1, rowpos, 2].Merge = true;
                        rowpos++;
                        worksheet.Cells[rowpos, 1].Value = "HOSS SIGN";
                        worksheet.Cells[rowpos, 1, rowpos, 2].Merge = true;

                        worksheet.Cells[1, 1, 1, colpos].Merge = true;
                        worksheet.Cells[1, 1, 1, colpos].Style.Font.Bold.ToString();
                        worksheet.Cells[1, 1, 1, colpos].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[1, 1, 1, colpos].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        setThinBorder(worksheet, 1, 1, rowpos, colpos);
                        worksheet.Cells[1, 1, rowpos, colpos].AutoFitColumns();
                    }
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return generated;
        }
        #endregion

        #region----------------PokayOke Transaction Report Denso---
        internal static string GetPokayOkeReport(string Machine, string Year, string Month)
        {
            string generated = "";
            try
            {
                string Filename = "PokayOkeTransactionReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "PokayOkeTransactionReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("PokayOkeTransactionReport template does not found at - \n " + Source);
                }
                else
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[1];
                    worksheet.PrinterSettings.PaperSize = ePaperSize.A4;
                    worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                    worksheet.PrinterSettings.HorizontalCentered = true;
                    worksheet.PrinterSettings.FitToWidth = 1;
                    worksheet.PrinterSettings.FitToHeight = 0;
                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.TopMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.BottomMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.LeftMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.RightMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.HeaderMargin = 0;
                    worksheet.PrinterSettings.FooterMargin = 0;
                    worksheet.PrinterSettings.ShowGridLines = true;
                    worksheet.PrinterSettings.RepeatRows = new ExcelAddress("1:4");
                    DataTable dt = DensoDBAccess.getPokayOkeTransactionDetails(Machine, Year, Month, "Report_CheckSheetDetails");
                    var distMachine = dt.AsEnumerable().Select(k => k["MachineID"].ToString()).Distinct().ToList();
                    var distItem = dt.AsEnumerable().Select(k => k["Item"].ToString()).Distinct().ToList();
                    var disWeek = dt.AsEnumerable().Select(k => k["WeekNo"].ToString()).Distinct().ToList();
                    worksheet.Cells["B2"].Value = Year + "-" + Month;
                    worksheet.Cells["B3"].Value = DateTime.Now.ToString("dd-MMM-yyyy");
                    int col = 7;
                    foreach (var WeekNo in disWeek)
                    {
                        worksheet.Cells[4, col].Value = "Week" + "-" + WeekNo;
                        col++;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        int rowpos = 5; int colpos = 7; int i = 1;

                        for (int machineC = 0; machineC < distMachine.Count; machineC++)
                        {
                            DataTable dtMachines = dt.AsEnumerable().Where(k => k["MachineID"].ToString().Equals(distMachine[machineC], StringComparison.OrdinalIgnoreCase)).CopyToDataTable();
                            var distChekpoint = dtMachines.AsEnumerable().Select(k => k["Checkpoint"].ToString()).Distinct().ToList();

                            worksheet.Cells[rowpos, 1].Value = i++;
                            worksheet.Cells[rowpos, 1, rowpos + (distChekpoint.Count - 1), 1].Merge = true;
                            worksheet.Cells[rowpos, 1, rowpos + (distChekpoint.Count - 1), 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells[rowpos, 1, rowpos + (distChekpoint.Count - 1), 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[rowpos, 2].Value = distMachine[machineC];
                            worksheet.Cells[rowpos, 2, rowpos + (distChekpoint.Count - 1), 2].Merge = true;
                            worksheet.Cells[rowpos, 2, rowpos + (distChekpoint.Count - 1), 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells[rowpos, 1, rowpos + (distChekpoint.Count - 1), 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            foreach (string checkpoint in distChekpoint)
                            {
                                var firstRow = dtMachines.AsEnumerable().Where(k => k["Checkpoint"].ToString().Equals(checkpoint, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                                worksheet.Cells[rowpos, 3].Value = firstRow["Item"];
                                worksheet.Cells[rowpos, 4].Value = firstRow["Function"];
                                worksheet.Cells[rowpos, 5].Value = firstRow["CheckPoint"];
                                worksheet.Cells[rowpos, 6].Value = firstRow["CheckInterval"];
                                var distWeek = dt.AsEnumerable().Where(k => k["Checkpoint"].ToString().Equals(checkpoint, StringComparison.OrdinalIgnoreCase)).Select(k => k["WeekNo"].ToString()).Distinct().ToList();
                                foreach (var week in distWeek)
                                {
                                    var dataValueRows = dtMachines.AsEnumerable().Where(k => k["CheckPoint"].ToString().Equals(checkpoint, StringComparison.OrdinalIgnoreCase) && k["WeekNo"].ToString() == week).FirstOrDefault();
                                    worksheet.Cells[rowpos, colpos].Value = dataValueRows["Value"];
                                    colpos++;
                                }
                                colpos = colpos - distWeek.Count;
                                rowpos++;
                            }
                        }
                        rowpos--;
                        colpos += 4;
                        setThinBorder(worksheet, 4, 1, rowpos, colpos);
                        worksheet.Cells[4, 7, rowpos, colpos].AutoFitColumns();
                    }
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    generated = "Generated";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return generated;
        }
        #endregion
        #region------First Information Report Denso----------------
        internal static string GetFirstInformationReport(string machineID, string downCategory, string downID, string startTime, string endTime, string downDesc, string startData, string endData, List<ListItem> listHours)
        {
            string Generated = "";
            try
            {
                string Filename = "FirstInformationReport_Denso.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "FirstInformationReport_Denso" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("FirstInformationReport_Denso template does not found at - \n " + Source);
                }
                else
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[1];
                    worksheet.PrinterSettings.PaperSize = ePaperSize.A4;
                    worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                    worksheet.PrinterSettings.HorizontalCentered = true;
                    worksheet.PrinterSettings.FitToWidth = 1;
                    worksheet.PrinterSettings.FitToHeight = 0;
                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.TopMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.BottomMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.LeftMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.RightMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.HeaderMargin = 0;
                    worksheet.PrinterSettings.FooterMargin = 0;
                    worksheet.PrinterSettings.ShowGridLines = true;
                    //worksheet.PrinterSettings.RepeatRows = new ExcelAddress("1:5");
                    DataTable dt = DensoDBAccess.getFIRTransactionDetails(machineID, downCategory, downID, startTime, endTime, out List<FIRTransDataEntity> listActionTaken, out DataTable dt3);
                    int row = 2; int col = 7;
                    for (int i = 0; i < listHours.Count; i++)
                    {
                        if (i == 4)
                        {
                            row = 3;
                            col = 7;
                        }
                        if (listHours[i].Text.Equals("red", StringComparison.OrdinalIgnoreCase))
                        {
                            worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 87, 87));
                        }
                        col++;
                    }
                    row = 11; col = 2;
                    for (int i = 0; i < listActionTaken.Count; i++)
                    {
                        col = 2;
                        worksheet.Cells[row, col].Value = listActionTaken[i].ActionTaken;
                        worksheet.Cells[row, col, row, col + 1].Merge = true;
                        col += 2;
                        worksheet.Cells[row, col].Value = listActionTaken[i].ActionTakenByWhom;
                        col++;
                        worksheet.Cells[row, col].Value = listActionTaken[i].ActionTakenResult;
                        worksheet.Cells[row, col, row, col + 1].Merge = true;
                        row++;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtRow in dt.Rows)
                        {
                            worksheet.Cells["C2"].Value = downDesc;
                            worksheet.Cells["C4"].Value = dtRow["PlantID"] + "/" + dtRow["MachineID"];
                            worksheet.Cells["E4"].Value = startData;
                            worksheet.Cells["E5"].Value = endData;
                            worksheet.Cells["C6"].Value = dtRow["RootCause"];
                            var distDepartment = dt3.AsEnumerable().Select(k => k.Field<string>("Department")).Distinct().ToList();
                            foreach (var Department in distDepartment)
                            {
                                var employee = dt3.AsEnumerable().Where(k => k["Department"].ToString().Equals(Department, StringComparison.OrdinalIgnoreCase)).Select(k => k.Field<string>("EmployeeID")).ToList();
                                foreach (var emplyee in employee)
                                {
                                    if (Department == "PRD")
                                    {
                                        worksheet.Cells["D7"].Value = emplyee;
                                    }
                                    if (Department == "MTD")
                                    {
                                        worksheet.Cells["D8"].Value = emplyee;
                                    }
                                    if (Department == "PED")
                                    {
                                        worksheet.Cells["D9"].Value = emplyee;
                                    }
                                    if (Department == "QAD")
                                    {
                                        worksheet.Cells["F7"].Value = emplyee;
                                    }
                                    if (Department == "SQD")
                                    {
                                        worksheet.Cells["F8"].Value = emplyee;
                                    }
                                    if (Department == "PCD")
                                    {
                                        worksheet.Cells["F9"].Value = emplyee;
                                    }
                                }
                            }
                            //foreach (DataRow dtR in dt3.Rows)
                            //{
                            //    worksheet.Cells["D5"].Value = dtR["EmployeeID"];
                            //    worksheet.Cells["D6"].Value = dtR["EmployeeID"];
                            //    worksheet.Cells["D7"].Value = dtR["EmployeeID"];
                            //    worksheet.Cells["F5"].Value = dtR["EmployeeID"];
                            //    worksheet.Cells["F6"].Value = dtR["EmployeeID"];
                            //    worksheet.Cells["F7"].Value = dtR["EmployeeID"];
                            //}
                            worksheet.Cells[row, 1, row, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, 1, row, 4].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(208, 206, 206));
                            worksheet.Cells[row, 1, row, 4].Style.Font.Color.SetColor(Color.FromArgb(22, 22, 242));
                            worksheet.Cells[row, 1, row, 4].Style.Font.Bold = true;
                            worksheet.Cells[row, 1].Value = 6;
                            worksheet.Cells[row, 2].Value = "Nex Action Decided";
                            worksheet.Cells[row, 2, row, 3].Merge = true;
                            worksheet.Cells[row, 4].Value = "By Whom";
                            worksheet.Cells[row, 5].Value = "Impact/Result";
                            worksheet.Cells[row, 5, row, 6].Merge = true;
                            col = 2; row++;
                            worksheet.Cells[row, 2].Value = dtRow["NextActionDecided"];
                            worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells[row, 2, row, 3].Merge = true;
                            worksheet.Cells[row, 4].Value = dtRow["NextActionByWhom"];
                            worksheet.Cells[row, 5].Value = dtRow["NextActionResult"];
                            worksheet.Cells[row, 5, row, 6].Merge = true;
                            row++;
                            var exelworksheet2 = Excel.Workbook.Worksheets[2];
                            exelworksheet2.Cells["C1"].Value = dtRow["StockStatus"];
                            exelworksheet2.Cells["C2"].Value = dtRow["StockImpact"];
                            exelworksheet2.Cells["C3"].Value = dtRow["PresentStatus"];
                            exelworksheet2.Cells["H1"].Value = dtRow["Details"];
                            exelworksheet2.Cells["H3"].Value = dtRow["PartNo"];
                            exelworksheet2.Cells["H4"].Value = dtRow["PartName"];
                            exelworksheet2.Cells["H5"].Value = dtRow["QtyHold"];
                            exelworksheet2.Cells[1, 1, 5, 10].Copy(worksheet.Cells[row, 1]);
                            exelworksheet2.Cells[1, 1, 5, 10].Clear();
                        }
                    }
                    row += 4;
                    worksheet.Cells[4, 7, 11 + listActionTaken.Count + dt.Rows.Count, 10].Merge = true;
                    worksheet.Cells[4, 7, row, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[4, 7, row, 10].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 228, 214));
                    worksheet.Cells[2, 1, row, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[2, 1, row, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(208, 206, 206));

                    setThinBorder(worksheet, 2, 1, row, 10);
                    //worksheet.Cells[4, 7, row, 10].AutoFitColumns();
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    Generated = "Generated";
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return Generated;
        }
        #endregion
        #region----------------Daily Checkpoint Report-------------
        internal static string GetDailyCheckPointReport(string Machine, string Year, string Month, string Week)
        {
            string generated = "";
            try
            {
                string Filename = "DailyInspectionCheckSheet.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "DailyInspectionCheckSheet" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("DailyInspectionCheckSheet template does not found at - \n " + Source);
                }
                else
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[1];
                    worksheet.PrinterSettings.PaperSize = ePaperSize.A4;
                    worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                    worksheet.PrinterSettings.HorizontalCentered = true;
                    worksheet.PrinterSettings.FitToWidth = 1;
                    worksheet.PrinterSettings.FitToHeight = 0;
                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.TopMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.BottomMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.LeftMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.RightMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.HeaderMargin = 0;
                    worksheet.PrinterSettings.FooterMargin = 0;
                    worksheet.PrinterSettings.ShowGridLines = true;
                    worksheet.PrinterSettings.RepeatRows = new ExcelAddress("1:5");
                    DataTable dt = DensoDBAccess.getDailyCheckpointTransactionDetails(Machine, Year, Month, Week, "Report_CheckSheetDetails");
                    var distDate = dt.AsEnumerable().Select(k => k.Field<DateTime>("Date")).Distinct().ToList();
                    var distCheckpoints = dt.AsEnumerable().Select(k => k["InspectionItem"].ToString()).Distinct().ToList();
                    var distShift = dt.AsEnumerable().Where(k => !string.IsNullOrEmpty(k["Shift"].ToString())).Select(k => k["Shift"].ToString()).Distinct().ToList();
                    int i = 1; int rowpos = 6; int colpos = 1;
                    //if (dt3.Rows.Count > 0)
                    //{
                    //    foreach (DataRow dtRoww in dt3.Rows)
                    //    {
                    //        worksheet.Cells["G3"].Value = "";
                    //        worksheet.Cells["F4"].Value = dtRoww["ApprovedBy"];
                    //        worksheet.Cells["H4"].Value = dtRoww["PreparedBy"];
                    //        worksheet.Cells["AK1"].Value = dtRoww["Year"] + "/" + dtRoww["Month"];
                    //        worksheet.Cells["AI4"].Value = dtRoww["Maintenance_ManagerSign"];
                    //        worksheet.Cells["AJ4"].Value = dtRoww["Maintenance_TeamLeaderSign"];
                    //        worksheet.Cells["AK4"].Value = dtRoww["Production_ManagerSign"];
                    //        worksheet.Cells["AL4"].Value = dtRoww["Production_TeamLeaderSign"];
                    //    }
                    //}
                    if (dt.Rows.Count > 0)
                    {
                        foreach (string checkPoint in distCheckpoints)
                        {
                            colpos = 1;
                            DataRow firstRow = dt.AsEnumerable().Where(k => k["InspectionItem"].ToString().Equals(checkPoint, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                            worksheet.Cells["D4"].Value = firstRow["MachineID"];
                            worksheet.Cells[rowpos, colpos].Value = i++;
                            colpos++;
                            worksheet.Cells[rowpos, colpos].Value = firstRow["Category"];
                            colpos++;
                            worksheet.Cells[rowpos, colpos].Value = firstRow["InspectionItem"];
                            colpos++;
                            worksheet.Cells[rowpos, colpos].Value = firstRow["JudgementCriteria"];
                            colpos++;
                            worksheet.Cells[rowpos, colpos].Value = firstRow["Method"];
                            colpos++;
                            worksheet.Cells[rowpos, colpos].Value = firstRow["Cycle"];
                            colpos++;
                            worksheet.Cells[rowpos, colpos].Value = firstRow["PersonInCharge"];
                            colpos++;
                            List<string> distDayShift = new List<string>();
                            if (firstRow["Frequency"].ToString().Equals("Shift", StringComparison.OrdinalIgnoreCase))
                            {
                                distDayShift = distShift;
                            }
                            else if (firstRow["Frequency"].ToString().Equals("Day", StringComparison.OrdinalIgnoreCase))
                            {
                                distDayShift.Add("");
                            }
                           // distDate.RemoveRange(7, (distDate.Count - 7));
                            foreach (string shift in distDayShift)
                            {
                                colpos = 8;
                                DataRow shiftrow = dt.AsEnumerable().Where(k => k["Shift"].ToString() == shift && k["InspectionItem"].ToString() == checkPoint).FirstOrDefault();
                                worksheet.Cells[rowpos, colpos].Value = shiftrow["Shift"];

                                colpos++;
                                foreach (DateTime date in distDate)
                                {
                                    DataRow valueRow = null;
                                    if (firstRow["Frequency"].ToString().Equals("Shift", StringComparison.OrdinalIgnoreCase))
                                    {
                                        valueRow = dt.AsEnumerable().Where(k => k.Field<DateTime>("Date") == date && k["Shift"].ToString() == shift && k["InspectionItem"].ToString() == checkPoint).FirstOrDefault();
                                    }
                                    else if (firstRow["Frequency"].ToString().Equals("Day", StringComparison.OrdinalIgnoreCase))
                                    {
                                        valueRow = dt.AsEnumerable().Where(k => k.Field<DateTime>("Date") == date && k["InspectionItem"].ToString() == checkPoint).FirstOrDefault();
                                    }
                                    worksheet.Cells[5, colpos].Value = date.Day.ToString();
                                    worksheet.Cells[rowpos, colpos].Value = valueRow["Value"];
                                    colpos++;
                                }
                                rowpos++;
                            }
                            if (distDayShift.Count > 1)
                            {
                                worksheet.Cells[rowpos - 3, 1, rowpos + distDayShift.Count - 4, 1].Merge = true;
                                worksheet.Cells[rowpos - 3, 2, rowpos + distDayShift.Count - 4, 2].Merge = true;
                                worksheet.Cells[rowpos - 3, 3, rowpos + distDayShift.Count - 4, 3].Merge = true;
                                worksheet.Cells[rowpos - 3, 4, rowpos + distDayShift.Count - 4, 4].Merge = true;
                                worksheet.Cells[rowpos - 3, 5, rowpos + distDayShift.Count - 4, 5].Merge = true;
                                worksheet.Cells[rowpos - 3, 6, rowpos + distDayShift.Count - 4, 6].Merge = true;
                                worksheet.Cells[rowpos - 3, 7, rowpos + distDayShift.Count - 4, 7].Merge = true;
                            }
                        }
                        rowpos--;
                        colpos--;
                        setThinBorder(worksheet, 1, 1, rowpos, colpos);
                        //worksheet.Cells[4, 7, rowpos, colpos].AutoFitColumns();

                    }
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    generated = "Generated";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return generated;
        }
        #endregion
        #region---------------5S Checksheet Report----------------
        internal static string Get5SChecksheetReport(string MachineID, string year, string Month)
        {
            string generated = "";
            try
            {
                string Filenane = "5SChecksheetReport.xlsx";
                string Source = GetReportPath(Filenane);
                string Template = "5SChecksheetReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("5SChecksheetReport template does not found at - \n " + Source);
                }
                else
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[1];
                    worksheet.PrinterSettings.PaperSize = ePaperSize.A4;
                    worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                    worksheet.PrinterSettings.HorizontalCentered = true;
                    worksheet.PrinterSettings.FitToWidth = 1;
                    worksheet.PrinterSettings.FitToHeight = 0;
                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.TopMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.BottomMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.LeftMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.RightMargin = Convert.ToDecimal("0.25");
                    worksheet.PrinterSettings.HeaderMargin = 0;
                    worksheet.PrinterSettings.FooterMargin = 0;
                    worksheet.PrinterSettings.ShowGridLines = true;
                    worksheet.PrinterSettings.RepeatRows = new ExcelAddress("1:5");
                    DataTable dt = DensoDBAccess.getFiveSCheckpointTransactionDetails(MachineID, year, Month, "Report_CheckSheetDetails");
                    var distDate = dt.AsEnumerable().Select(k => k.Field<DateTime>("Date")).Distinct().ToList();
                    var distCheckpoints = dt.AsEnumerable().Select(k => k["CheckItem"].ToString()).Distinct().ToList();
                    //var distShift = dt.AsEnumerable().Where(k => !string.IsNullOrEmpty(k["ShiftID"].ToString())).Select(k => k["ShiftID"].ToString()).Distinct().ToList();
                    if (dt.Rows.Count > 0)
                    {
                        int i = 1; int rowpos = 6; int colpos = 1; int col = 5;
                        worksheet.Cells[1, 2].Value = Month + "/" + year;
                        worksheet.Cells[3, 1, 3, distDate.Count + 4].Merge = true;
                        worksheet.Cells[3, 1, 3, distDate.Count + 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        foreach (DateTime date in distDate)
                        {
                            worksheet.Cells[5, col].Value = date.Day.ToString();
                            col++;
                        }
                        worksheet.Cells[4, 4, 4, 4 + distDate.Count - 1].Merge = true;
                        worksheet.Cells[4, 4, 4, 4 + distDate.Count - 1].Value = "Date";
                        worksheet.Cells[4, 4, 4, 4 + distDate.Count - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        foreach (string checkPoint in distCheckpoints)
                        {
                            var distShift = dt.AsEnumerable().Where(k => k["CheckItem"].ToString().Equals(checkPoint, StringComparison.OrdinalIgnoreCase)).Select(k => k["ShiftID"].ToString()).Distinct().ToList();
                            DataTable dtCheckPoints = dt.AsEnumerable().Where(k => k["CheckItem"].ToString().Equals(checkPoint, StringComparison.OrdinalIgnoreCase)).CopyToDataTable();
                            DataRow CheckpointRow = dt.AsEnumerable().Where(k => k["CheckItem"].ToString().Equals(checkPoint, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                            worksheet.Cells[rowpos, 1].Value = i++;
                            worksheet.Cells[rowpos, 1, rowpos + (distShift.Count - 1), 1].Merge = true;
                            colpos++;
                            worksheet.Cells[rowpos, 2].Value = checkPoint;
                            worksheet.Cells[rowpos, 2, rowpos + (distShift.Count - 1), 2].Merge = true;
                            colpos++;
                            worksheet.Cells[rowpos, 3].Value = CheckpointRow["Cycle"];
                            worksheet.Cells[rowpos, 3, rowpos + (distShift.Count - 1), 3].Merge = true;
                            colpos++;
                            foreach (string shift in distShift)
                            {
                                colpos = 4;
                                DataRow firstRow = dt.AsEnumerable().Where(k => k["CheckItem"].ToString().Equals(checkPoint, StringComparison.OrdinalIgnoreCase) && k["ShiftID"].ToString() == shift).FirstOrDefault();
                                worksheet.Cells[3, 1, 3, distDate.Count + 4].Value = "Items Used:" + firstRow["ItemsUsed"];
                                worksheet.Cells[rowpos, colpos].Value = firstRow["ShiftName"];
                                colpos++;
                                foreach (DateTime date in distDate)
                                {
                                    DataRow valueRow = dt.AsEnumerable().Where(k => k.Field<DateTime>("Date") == date && k["ShiftID"].ToString() == shift && k["CheckItem"].ToString() == checkPoint).FirstOrDefault();
                                    worksheet.Cells[rowpos, colpos].Value = valueRow["Value"];
                                    colpos++;
                                }
                                rowpos++;
                            }
                        }
                        rowpos--; colpos--;
                        setThinBorder(worksheet, 1, 1, rowpos, colpos);
                        worksheet.Cells[1, 7, rowpos, colpos].AutoFitColumns();
                    }
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    generated = "Generated";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return generated;
        }
        #endregion
    }
}