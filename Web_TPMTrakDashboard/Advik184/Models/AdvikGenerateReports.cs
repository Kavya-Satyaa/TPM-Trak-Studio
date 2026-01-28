using Elmah;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Web_TPMTrakDashboard.Advik184.Models
{
    public class AdvikGenerateReports
    {
        internal static void setWorkSheetSetting(ExcelWorksheet wksheet)
        {
            wksheet.PrinterSettings.Orientation = eOrientation.Landscape;
            wksheet.PrinterSettings.FitToPage = true;
            wksheet.PrinterSettings.FitToWidth = 1;
            wksheet.PrinterSettings.FitToHeight = 0;

        }
        static readonly string appPath = HttpContext.Current.Server.MapPath("~/Advik184");
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
        public static string SafeFileName(string name)
        {
            StringBuilder str = new StringBuilder(name);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.ToString();
        }
        #region -----FI Slno report----
        internal static bool GenerateFinalInspectionSlnoReport(string plant, string status, string fromDate, string todate, string slno, string viewType, List<FinalInspectionEnity> list)
        {
            bool successfull = false;
            try
            {
                string Filename = "FISlnoReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "FISlnoReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("FI Slno Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    int rowStart;
                    int colStart = 1;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    if (list != null && list.Count > 0)
                    {

                        var workSheet = Excel.Workbook.Worksheets[1];
                        setWorkSheetSetting(workSheet);
                        rowStart = 7;
                        workSheet.Cells["B3"].Value = plant;
                        workSheet.Cells["F3"].Value = status;
                        if (viewType.Equals("date", StringComparison.OrdinalIgnoreCase))
                        {
                            workSheet.Cells["A4"].Value = "From";
                            workSheet.Cells["B4"].Value = fromDate;
                            workSheet.Cells["E4"].Value = "To";
                            workSheet.Cells["F4"].Value = todate;
                        }
                        else
                        {
                            workSheet.Cells["B4"].Value = slno;
                        }
                        for (int i = 0; i < list.Count; i++)
                        {

                            colStart = 1;
                            workSheet.Cells[rowStart, colStart].Value = list[i].SerialNumber;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].Model;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].PartNumber;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].PlantID;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].Date;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].Status;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].Remarks;
                            rowStart++;
                        }
                        setBorderThin(workSheet, 7, 1, rowStart - 1, colStart);
                        workSheet.Cells[7, 1, rowStart, colStart].AutoFitColumns();
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
        internal static bool GenerateFinalInspectionStatusAndParamReport(string plant, string status, string fromDate, string todate, string slno, string viewType, List<FinalInspectionEnity> listStatus, List<ParameterMasterEntity> listPatameter)
        {
            bool successfull = false;
            try
            {
                string Filename = "FIStatusAndParamReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "FIStatusAndParamReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("FI Status and Param Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    int rowStart;
                    int colStart = 1;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var workSheet = Excel.Workbook.Worksheets[1];
                    setWorkSheetSetting(workSheet);
                    rowStart = 6;
                    workSheet.Cells["B3"].Value = plant;
                    workSheet.Cells["F3"].Value = status;
                    if (viewType.Equals("date", StringComparison.OrdinalIgnoreCase))
                    {
                        workSheet.Cells["A4"].Value = "From";
                        workSheet.Cells["B4"].Value = fromDate;
                        workSheet.Cells["E4"].Value = "To";
                        workSheet.Cells["F4"].Value = todate;
                    }
                    else
                    {
                        workSheet.Cells["B4"].Value = slno;
                    }
                    int startRow = rowStart;
                    if (listStatus.Count > 0)
                    {
                        startRow = rowStart;
                        workSheet.Cells[rowStart, 1].Value = "Station Status";
                        workSheet.Cells[rowStart, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[rowStart, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(248, 203, 173));
                        workSheet.Cells[rowStart, 1, rowStart, 2].Merge = true;
                        rowStart++;
                        for (int i = 0; i < listStatus.Count; i++)
                        {
                            if (i > 0)
                            {
                                startRow = rowStart;
                            }
                            FinalInspectionEnity data = listStatus[i];
                            workSheet.Cells[rowStart, 1].Value = data.MachineID;
                            workSheet.Cells[rowStart, 1].Style.Font.Bold = true;
                            workSheet.Cells[rowStart, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[rowStart, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[rowStart, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(252, 229, 213));
                            workSheet.Cells[rowStart, 1, rowStart, 2].Merge = true;
                            rowStart++;
                            List<FinalInspectionEnity> statusList = data.StatusList;
                            foreach (FinalInspectionEnity statusData in statusList)
                            {
                                workSheet.Cells[rowStart, 1].Value = statusData.Label;
                                workSheet.Cells[rowStart, 1].Style.Font.Bold = true;
                                workSheet.Cells[rowStart, 2].Value = statusData.Value;
                                rowStart++;
                            }
                            setBorderThin(workSheet, startRow, 1, rowStart - 1, 2);
                            rowStart++;
                        }
                    }
                    if (listPatameter.Count > 0)
                    {
                        startRow = rowStart;
                        workSheet.Cells[rowStart, 1].Value = "Parameter List";
                        workSheet.Cells[rowStart, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[rowStart, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(248, 203, 173));
                        workSheet.Cells[rowStart, 1, rowStart, 2].Merge = true;
                        rowStart++;
                        for (int i = 0; i < listPatameter.Count; i++)
                        {

                            colStart = 1;
                            workSheet.Cells[rowStart, colStart].Value = listPatameter[i].Label;
                            workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listPatameter[i].Value;
                            rowStart++;
                        }
                        setBorderThin(workSheet, startRow, 1, rowStart - 1, colStart);
                    }
                    workSheet.Cells[6, 1, rowStart, 2].AutoFitColumns();
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

        #endregion
        #region -----Traceability report----
        internal static bool GenerateTraceabilityReport(string plant, string machine, string fromDate, string todate, string slno, string viewType, List<TraceabilityEntity> list)
        {
            bool successfull = false;
            try
            {
                string Filename = "TraceabilityReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "TraceabilityReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Traceability Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    int rowStart;
                    int colStart = 1;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    if (list != null && list.Count > 0)
                    {

                        var workSheet = Excel.Workbook.Worksheets[1];
                        setWorkSheetSetting(workSheet);
                        rowStart = 7;
                        workSheet.Cells["B3"].Value = plant;
                        if (viewType.Equals("date", StringComparison.OrdinalIgnoreCase))
                        {
                            workSheet.Cells["D3"].Value = "From";
                            workSheet.Cells["E3"].Value = fromDate;
                            workSheet.Cells["G3"].Value = "To";
                            workSheet.Cells["H3"].Value = todate;
                        }
                        else
                        {
                            workSheet.Cells["E3"].Value = slno;
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            colStart = 1;
                            workSheet.Cells[rowStart, colStart].Value = list[i].QRCode;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].Machine;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].ModelName;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].StartTime;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].EndTime;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].ElapsedTime;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].Value;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].Result;
                            rowStart++;
                        }
                        setBorderThin(workSheet, 6, 1, rowStart - 1, colStart);
                        workSheet.Cells[7, 1, rowStart, colStart].AutoFitColumns();
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
        #region --------- PokayOke Report -----
        internal static bool GeneratePokayOkeReport(string fromDate, string todate, List<PokayokeMasterEntity> list)
        {
            bool successfull = false;
            try
            {
                string Filename = "PokayOkeReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "PokayOkeReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("PokayOkeReport template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    int rowStart;
                    int colStart = 1;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    if (list != null && list.Count > 0)
                    {

                        var workSheet = Excel.Workbook.Worksheets[1];
                        setWorkSheetSetting(workSheet);
                        rowStart = 6;
                        workSheet.Cells["B3"].Value = fromDate;
                        workSheet.Cells["F3"].Value = todate;
                        for (int i = 0; i < list.Count; i++)
                        {

                            colStart = 1;
                            workSheet.Cells[rowStart, colStart].Value = list[i].MachineID;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].PokayokeID;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].PokayokeName;
                            colStart++;
                            //workSheet.Cells[rowStart, colStart].Value = list[i].RegisterID;
                            //colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].Result;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].UpdatedTS;
                            rowStart++;
                        }
                        setBorderThin(workSheet, 6, 1, rowStart - 1, colStart);
                        workSheet.Cells[6, 1, rowStart, colStart].AutoFitColumns();
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
    }
}