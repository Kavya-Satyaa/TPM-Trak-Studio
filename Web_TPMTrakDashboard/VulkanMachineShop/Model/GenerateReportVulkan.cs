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

namespace Web_TPMTrakDashboard.VulkanMachineShop.Model
{
    public class GenerateReportVulkan
    {
        static readonly string appPath = HttpContext.Current.Server.MapPath("~/VulkanMachineShop/ReportTemplate");
        static string appPathForReportOutput = HttpContext.Current.Server.MapPath("~/VulkanMachineShop/ReportTemplate/ReportsOutput/");
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
            string src = string.Empty;
            if (HttpContext.Current.Session["Language"] == null)
                src = Path.Combine(appPath, reportName);
            else
            {
                if (HttpContext.Current.Session["Language"].ToString() != "en")
                    src = Path.Combine(appPath, HttpContext.Current.Session["Language"].ToString() + "", reportName);
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
        private static void setBorderThin(ExcelWorksheet sheet, int fromRow, int fromcol, int toRow, int toCol)
        {
            var modelTable3 = sheet.Cells[fromRow, fromcol, toRow, toCol];
            modelTable3.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        internal static void GenerateInspectionReport(string StartDate, string EndDate, string MachineID)
        {
            string isSuccessful = "";
            try
            {
                string Filename = "InspectionReportVulkan.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "InspectionReportVulkan" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Inspection Report template does not exists at - " + Source);
                    isSuccessful = "TemplateNotFound";
                }
                else
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    Excel.Workbook.Worksheets.Delete(1);
                    DataTable dt_Operator = new DataTable();
                    DataTable dt_InspectionData = new DataTable();
                    DataTable dt_ApprovedData = new DataTable();
                    DataTable dt_HeatCodes = new DataTable();
                    dt_InspectionData = VulkanMSDBAccess.GetInspectionReportData(StartDate, EndDate, MachineID, out dt_Operator, out dt_ApprovedData,out dt_HeatCodes);

                    var MachineList = dt_InspectionData.AsEnumerable().Select(x => x["MachineID"].ToString()).Distinct().ToList();
                    foreach (var Machine in MachineList)
                    {
                        var DateList = dt_InspectionData.AsEnumerable().Where(x => x["MachineID"].ToString() == Machine).Select(x => x["Date"].ToString()).Distinct().ToList();
                        foreach (var Date in DateList)
                        {
                            var shiftList = dt_InspectionData.AsEnumerable().Where(x => x["Date"].ToString() == Date).Select(x => x["Shift"].ToString()).Distinct().ToList();
                            foreach (var shift in shiftList)
                            {
                                DataTable dt = dt_InspectionData.AsEnumerable().Where(x => x["MachineID"].ToString() == Machine || x["Date"].ToString() == Date).CopyToDataTable();

                                var distComponents = dt.AsEnumerable().Select(x => x["ComponentID"].ToString()).Distinct().ToList();

                                Excel.Workbook.Worksheets.Add($"sheet_{Machine}_{Util.GetDateTime(Date).ToString("ddMmyyyy")}_{shift}");
                                var sheet = Excel.Workbook.Worksheets[$"sheet_{Machine}_{Util.GetDateTime(Date).ToString("ddMmyyyy")}_{shift}"];

                                sheet.Cells[2, 1].Value = "Machine ID";
                                sheet.Cells[2, 2].Value = Machine;
                                sheet.Cells[2, 3].Value = "Date";
                                sheet.Cells[2, 4].Value = Util.GetDateTime(Date).ToString("dd-MM-yyyy");
                                sheet.Cells[2, 6].Value = "Shift";
                                sheet.Cells[2, 7].Value = shift;
                                sheet.Cells[2, 1, 2, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[2, 1, 2, 7].Style.Font.Bold = true;
                                int rowStart = 5, colStart = 1, j = 0, tempRowStart = 5;

                                while (j < distComponents.Count)
                                {
                                    colStart = 1;
                                    sheet.Cells[rowStart, colStart].Value = "Component ID";
                                    colStart++;
                                    sheet.Cells[rowStart, colStart].Value = "Operation No";
                                    colStart++;
                                    sheet.Cells[rowStart, colStart].Value = "Characteristic ID";

                                    for (int i = 12; i < dt.Columns.Count; i++)
                                    {
                                        colStart++;
                                        sheet.Cells[rowStart, colStart].Value = dt.Columns[i].ColumnName.ToString();
                                    }
                                    sheet.Cells[rowStart, 1, rowStart, colStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    rowStart++;

                                    var ComponentWiseList = dt.AsEnumerable().Where(x => (x["MachineID"].ToString() == Machine && x["Date"].ToString() == Date && x["Shift"].ToString() == shift && x["ComponentID"].ToString() == distComponents[j])).CopyToDataTable();
                                    foreach (DataRow Component in ComponentWiseList.Rows)
                                    {
                                        colStart = 1;
                                        sheet.Cells[rowStart, colStart].Value = Component["ComponentID"].ToString();
                                        colStart++;
                                        sheet.Cells[rowStart, colStart].Value = Component["OperationNo"].ToString();
                                        colStart++;
                                        sheet.Cells[rowStart, colStart].Value = Component["CharacteristicID"].ToString();

                                        for (int i = 12; i < dt.Columns.Count; i++)
                                        {
                                            colStart++;
                                            sheet.Cells[rowStart, colStart].Value = Component[i].ToString();
                                        }
                                        rowStart++;
                                    }

                                    sheet.Cells[rowStart, 1, rowStart, 3].Merge = true;
                                    sheet.Cells[rowStart, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    sheet.Cells[rowStart, 1].Value = "Opertaor Sign";
                                    var OperatorRow = dt_Operator.AsEnumerable().Where(x => (x["MachineID"].ToString() == Machine && x["Date"].ToString() == Date && x["Shift"].ToString() == shift)).CopyToDataTable();
                                    if (OperatorRow.Rows.Count > 0)
                                    {
                                        DataRow row = OperatorRow.Rows[0];
                                        colStart = 4;
                                        for (int i = 3; i < OperatorRow.Columns.Count; i++)
                                        {
                                            sheet.Cells[rowStart, colStart].Value = row[i].ToString();
                                            colStart++;
                                        }
                                    }
                                    j++;
                                    for(int i = 1; i <= colStart; i++)
                                    {
                                        sheet.Column(i).Width = 15;
                                    }
                                    sheet.Cells[1, 1, 1, colStart - 1].Merge = true;
                                    sheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    sheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    sheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                                    sheet.Cells[1, 1].Value = "Inspection Report";
                                    setBorderThin(sheet, tempRowStart, 1, rowStart, colStart-1);
                                    setBorderThin(sheet, 1, 1, 2, colStart-1);
                                    rowStart += 2;
                                    tempRowStart = rowStart;
                                }

                                sheet.Cells[rowStart + 3, colStart - 2].Value = "Verified & Approved By:";
                                if (dt_ApprovedData.Rows.Count > 0)
                                {
                                    string ApproverSign = dt_ApprovedData.AsEnumerable().Where(x => (x["MachineID"].ToString() == Machine && x["Date"].ToString() == Date && x["Shift"].ToString() == shift)).Select(x => x["ApprovedBy"].ToString()).FirstOrDefault();
                                    string ApproverTS = dt_ApprovedData.AsEnumerable().Where(x => (x["MachineID"].ToString() == Machine && x["Date"].ToString() == Date && x["Shift"].ToString() == shift)).Select(x => x["ApprovedTS"].ToString()).FirstOrDefault();
                                    sheet.Cells[rowStart + 2, colStart - 2].Value = ApproverSign + "_" + ApproverTS;
                                }
                            }
                        }
                    }

                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GenerateInspectionReport: {ex.Message}");
            }
        }
    }
}