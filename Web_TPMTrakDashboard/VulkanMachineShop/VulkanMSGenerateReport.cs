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
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.VulkanMachineShop.Model;

namespace Web_TPMTrakDashboard.VulkanMachineShop
{
    public class VulkanMSGenerateReport
    {
        static readonly string appPath = HttpContext.Current.Server.MapPath("~/VulkanMachineShop");
        #region "Get Report Template File Path"
        public static string GetReportPath(string reportName)
        {
            string src = string.Empty;
            if (HttpContext.Current.Session["Language"] == null)
                src = Path.Combine(appPath, "ReportTemplate", reportName);
            else
            {
                if (HttpContext.Current.Session["Language"].ToString() != "en")
                    src = Path.Combine(appPath, "ReportTemplate-" + HttpContext.Current.Session["Language"].ToString() + "", reportName);
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

        #region ----Schedule master ----
        internal static bool generateScheduleMasterReport(string plant, string cell, string machine, string status, string compID, string compDesc, string fromDate, string toDate, List<ScheduleMasterEntity> list)
        {
            bool successfull = false;
            try
            {
                string Filename = "ScheduleMasterReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "ScheduleMasterReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("ScheduleMasterReport template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    int rowStart = 7;
                    int colStart = 1;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    //Excel.Workbook.Worksheets.Delete("Sheet1");
                    if (list != null && list.Count > 0)
                    {

                        var workSheet = Excel.Workbook.Worksheets["Sheet1"];
                        workSheet.Cells["B3"].Value = plant;
                        workSheet.Cells["E3"].Value = cell;
                        workSheet.Cells["H3"].Value = machine;
                        workSheet.Cells["K3"].Value = status;
                        workSheet.Cells["B4"].Value = fromDate;
                        workSheet.Cells["E4"].Value = toDate;
                        workSheet.Cells["H4"].Value = compID;
                        workSheet.Cells["K4"].Value = compDesc;
                        for (int i = 0; i < list.Count; i++)
                        {
                            colStart = 1;
                            workSheet.Cells[rowStart, colStart].Value = list[i].ScheduleDate;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].Plant;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].Cell;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].Machine;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].Component;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].Operation;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].CompDesc;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].Status;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].Priorityno;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].WorkOrder;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = list[i].WorkOrderQty;
                            rowStart++;
                        }
                        workSheet.Cells[6, 1, rowStart, colStart].AutoFitColumns();
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

        #region --- PM Report ---
        #endregion

        internal static string DownloadPMMasterEntryImportFile()
        {
            string isSuccessful = "";
            string Filename = "ImportTemplate_PMMaster.xlsx";
            string Source = GetReportPath(Filename);
            string Template = "ImportTemplate_PMMaster" + DateTime.Now + ".xlsx";
            string destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
            if (!File.Exists(Source))
            {
                Logger.WriteErrorLog("ImportTemplate_PMMaster doesn't exist");
                isSuccessful = "Template Not Found";
            }
            else
            {
                FileInfo file = new FileInfo(Source);
                ExcelPackage excel = new ExcelPackage(file);

                DownloadMultipleFile(destination, excel.GetAsByteArray());
                isSuccessful = "Downloaded";
            }
            return isSuccessful;
        }
    }
}