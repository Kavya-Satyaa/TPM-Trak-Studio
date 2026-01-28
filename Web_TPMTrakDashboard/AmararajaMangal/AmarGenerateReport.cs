using Elmah;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using static Web_TPMTrakDashboard.AmararajaMangal.DTO;

namespace Web_TPMTrakDashboard.AmararajaMangal
{
    public class AmarGenerateReport
    {
        static readonly string appPath = HttpContext.Current.Server.MapPath("~/AmararajaMangal");
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
        internal static bool generateScrapEntryReport(string machineID, string shift, string date, List<ScrapEntryData> listData)
        {
            bool successfull = false;
            try
            {
                string Filename = "ScrapEntryReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "ScrapEntryReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Scrap Entry Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    int rowStart = 7;
                    int colStart = 1;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    //Excel.Workbook.Worksheets.Delete("Sheet1");
                    if (listData != null && listData.Count > 0)
                    {

                        var workSheet = Excel.Workbook.Worksheets["Sheet1"];
                        workSheet.Cells["B3"].Value = date;
                        workSheet.Cells["D3"].Value = shift;
                        workSheet.Cells["F3"].Value = machineID;
                        for (int i = 0; i < listData.Count; i++)
                        {
                            int rowSpan = Convert.ToInt32(listData[i].RowSpan);
                            colStart = 1;
                            if (rowSpan > 0)
                            {
                                workSheet.Cells[rowStart, colStart].Value = listData[i].MachineID;
                                workSheet.Cells[rowStart, colStart, rowStart + rowSpan - 1, colStart].Merge = true;
                            }
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].PartID;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].TotalPartProduced;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].AcceptedPart;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].UnitWeight;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].TotalAcceptedPart;
                            colStart++;
                           
                            if (rowSpan > 0)
                            {
                                workSheet.Cells[rowStart, colStart].Value = listData[i].TotalWeight;
                                workSheet.Cells[rowStart, colStart, rowStart + rowSpan - 1, colStart].Merge = true;
                            }
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listData[i].Rejection;
                            colStart++;
                            if (rowSpan > 0)
                            {
                                workSheet.Cells[rowStart, colStart].Value = listData[i].RejectedParts;
                                workSheet.Cells[rowStart, colStart, rowStart + rowSpan - 1, colStart].Merge = true;
                            }
                            colStart++;
                            if (rowSpan > 0)
                            {
                                workSheet.Cells[rowStart, colStart].Value = listData[i].DesignScrap;
                                workSheet.Cells[rowStart, colStart, rowStart + rowSpan - 1, colStart].Merge = true;
                            }
                            colStart++;
                            if (rowSpan > 0)
                            {
                                workSheet.Cells[rowStart, colStart].Value = listData[i].LayoutScrap;
                                workSheet.Cells[rowStart, colStart, rowStart + rowSpan - 1, colStart].Merge = true;
                            }
                            colStart++;
                            if (rowSpan > 0)
                            {
                                workSheet.Cells[rowStart, colStart].Value = listData[i].TotalScrap;
                                workSheet.Cells[rowStart, colStart, rowStart + rowSpan - 1, colStart].Merge = true;
                            }
                            rowStart++;
                        }
                        setBorderThin(workSheet, 7, 1, rowStart - 1, colStart);
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