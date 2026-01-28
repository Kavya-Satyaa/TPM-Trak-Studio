using OfficeOpenXml2;
using OfficeOpenXml2.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Cumi.Model
{
    public class CumiGenerateReport
    {
        static readonly string appPath = HttpContext.Current.Server.MapPath("~/Cumi");
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
                    src = Path.Combine(appPath, "ImportTemplate", reportName);
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
                Logger.WriteErrorLog(ex);
            }
        }
        private static void setBorderThin(ExcelWorksheet workSheet, int fromRow, int fromCol, int toRow, int toCol)
        {
            try
            {
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
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
        #region ---- Loss Report ----
        internal static bool generateLossReport(string fromDate, string toDate, string plant, string category, List<LossReportEntity> listData, string mttr, string mtbf)
        {
            bool successfull = false;
            try
            {
                string Filename = "LossReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "LossReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Loss Report Report template does not exists at - " + Source);
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
                        workSheet.Cells["B2"].Value = fromDate;
                        workSheet.Cells["D2"].Value = toDate;
                        workSheet.Cells["F2"].Value = plant;
                        workSheet.Cells["H2"].Value = category;
                        workSheet.Cells["C4"].Value = mttr;
                        workSheet.Cells["G4"].Value = mtbf;

                        for (int i = 0; i < listData.Count; i++)
                        {

                            colStart = 1;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].Machine;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].StartDate;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].EndDate;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].StartTime;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].EndTime;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = HelperClassGeneric.getDoubleValueFromString(listData[i].TotalTime);
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = HelperClassGeneric.getDoubleValueFromString(listData[i].PDT);
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].LossType;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].SubLoss;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].LossID;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].OperatorID;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].MntcEmpID;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].WhyWhyDocFileName;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].OtherDocFileName;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].Remarks;
                            rowStart++;
                        }
                        setBorderThin(workSheet, 5, 1, rowStart - 1, colStart);
                        workSheet.Cells[5, 1, rowStart, colStart].AutoFitColumns();
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

        #region --Process Parameter Report---
        internal static bool generateProcessParameterReport(DateTime fromDate, DateTime toDate, string plantID, string machineID, string Shift,List<ProcessParameterEntity> listData)
        {
            bool successfull = false;
            try
            {
                string Filename = "ProcessParameterReportCumi.xlsx";
                string Source = Util.GetReportPath(Filename);
                string Template = "ProcessParameterReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Process Parameter Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {

                    int rowStart = 7;
                    int colStart = 1;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    
                    if (listData != null && listData.Count > 0)
                    {
                        var workSheet = Excel.Workbook.Worksheets["Sheet1"];
                        workSheet.Cells["B3"].Value = fromDate;
                        workSheet.Cells["E3"].Value = toDate;
                        workSheet.Cells["H3"].Value = Shift;
                        workSheet.Cells["J3"].Value = machineID;

                        for (int i = 0; i < listData.Count; i++)
                        {
                            colStart = 1;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].Slno;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].Product;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].PONumber;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].EmployeeID;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].StartDate;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].EndDate;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].MachiningTime;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].LoadUnload;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].HydraulicPressure_Top;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].HydraulicPressure_Bottom;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].HydraulicTemp;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].TopRamStroke;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].BottomRamStroke;
                            rowStart++;
                        }
                        setBorderThin(workSheet, 5, 1, rowStart - 1, colStart);
                        workSheet.Cells[5, 1, rowStart, 8].AutoFitColumns();
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

        #region --PO Status---
        internal static bool GenerateCumiPOStatusReport(List<POStatusEntity> list, string fromDate, string toDate, string PONumber,string value)
        {
            bool successfull = false;
            try
            {
                string src, dst = string.Empty;
                string reportName = "POStatusReport.xlsx";
                src = Util.GetReportPath(reportName);

                string tempfileName = "POStatusReport" + DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss") + ".xlsx";
                dst = Path.Combine(appPath, "Temp", SafeFileName(tempfileName));

                if (!File.Exists(src))
                {
                    Logger.WriteDebugLog("POStatusReport not found - " + src);
                    successfull = false;
                }
                if (true)
                {
                    FileInfo newFile = new FileInfo(src);
                    ExcelPackage pck = new ExcelPackage(newFile, true);
                    var ws = pck.Workbook.Worksheets["Sheet1"];

                    if (value.Equals("DateView", StringComparison.OrdinalIgnoreCase))
                    {
                        ws.Cells["B3"].Value = Util.GetDateTime(fromDate);
                        ws.Cells["E3"].Value = Util.GetDateTime(toDate);
                    }
                    else
                    {
                        ws.Cells["B3"].Value = fromDate;
                        ws.Cells["E3"].Value = toDate;
                    }
                    ws.Cells["B4"].Value = PONumber;

                    int rowStart = 7, colStart = 1;
                    int rowStartCopy = rowStart;

                    if (list.Count > 0)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            colStart = 1;
                            //ws.Cells[rowStart, colStart].Value =list[j].Date;
                            //colStart++;
                            //ws.Cells[rowStart, colStart].Value = list[j].Shift;
                            //colStart++;
                            //ws.Cells[rowStart, colStart].Value = list[j].MachineID;
                            //colStart++;
                            ws.Cells[rowStart, colStart].Value = list[j].PONumber;
                            colStart++;
                            ws.Cells[rowStart, colStart].Value = list[j].ItemCode;
                            colStart++;
                            ws.Cells[rowStart, colStart].Value = list[j].POQty;
                            colStart++;
                            ws.Cells[rowStart, colStart].Value = list[j].ProducedQty;
                            colStart++;
                            ws.Cells[rowStart, colStart].Value = list[j].BalanceQty;
                            rowStart++;
                        }
                        rowStart--;
                        ws.Cells[6, 1, rowStart, colStart].AutoFitColumns();
                        setBorderThin(ws, 6, 1, rowStart, colStart);
                    }

                    DownloadMultipleFile(dst, pck.GetAsByteArray());
                    successfull = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog("POStatusReport :  " + ex.Message);
            }
            return successfull;
        }

        #endregion

        #region ---QualityRejection Report---
        internal static bool GenerateCumiQualityReport(List<QualityRejectionEntity> listData, DateTime fromDate, DateTime toDate, string plant, string machine, string employee, string Category, string rejReason)
        {
            bool successfull = false;
            try
            {
                string Filename = "QualityReport.xlsx";
                string Source = Util.GetReportPath(Filename);
                string Template = "QualityReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("QualityReport template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {

                    int rowStart = 7;
                    int colStart = 1;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    
                    if (listData != null && listData.Count > 0)
                    {
                        var workSheet = Excel.Workbook.Worksheets["Sheet1"];
                        workSheet.Cells["B3"].Value = fromDate;
                        workSheet.Cells["B4"].Value = toDate;
                        workSheet.Cells["D3"].Value = plant;
                        workSheet.Cells["D4"].Value = employee;
                        workSheet.Cells["G3"].Value = machine;
                        workSheet.Cells["G4"].Value = rejReason;

                        var datalist = listData.AsEnumerable().Select(x=>x.MachineId).Distinct().ToList();

                        for (int i = 0; i < datalist.Count; i++)
                        {
                            var list = listData.AsEnumerable().Where(x => x.MachineId.Equals(datalist[i])).Distinct().ToList();

                            var Quantity = list.Sum(x => Convert.ToInt32(x.RejQty)).ToString();
                            var TotalWeight = list.Sum(x => Convert.ToInt32(x.TotalWeight)).ToString();
                            for (int k=0;k<list.Count;k++)
                            {
                                colStart = 1;
                                workSheet.Cells[rowStart, colStart].Value = list[k].MachineId;
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = list[k].Date;
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = list[k].PONumber;
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = list[k].ItemCode;
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = list[k].ItemDesc;
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = list[k].Operator;
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = list[k].RejReason;
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = list[k].RejQty;
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = list[k].TotalWeight;
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = list[k].Document;
                                rowStart++;
                            }

                            workSheet.Cells[rowStart, 1].Value = "Total";
                            workSheet.Cells[rowStart, 1, rowStart, 7].Merge = true;
                            workSheet.Cells[rowStart, 1, rowStart, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[rowStart, 1, rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[rowStart, 1, rowStart, colStart].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                            workSheet.Cells[rowStart, 1, rowStart, colStart].Style.Font.Bold = true;
                            workSheet.Cells[rowStart, 8].Value = Quantity;
                            workSheet.Cells[rowStart++, 9].Value = TotalWeight;
                        }
                        setBorderThin(workSheet, 5, 1, rowStart-1, colStart);
                        workSheet.Cells[5, 1, rowStart, colStart].AutoFitColumns();
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