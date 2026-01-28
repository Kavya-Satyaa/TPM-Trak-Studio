using Elmah;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Web_TPMTrakDashboard.Models;
using static Web_TPMTrakDashboard.AmararajaMangal.DTO;

namespace Web_TPMTrakDashboard.KTASpindle
{
    public class KTAGenerateReport
    {
        static readonly string appPath = HttpContext.Current.Server.MapPath("~/KTASpindle");
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
        internal static bool generateScheduleMasterReport(string plant,string cell,string machine,string status,string compID,string compDesc,string fromDate,string toDate, List<ScheduleMasterEntity> list)
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

        #region ------- Production Report -----------
        public static string GenerateProductionReport(DateTime FromDate, DateTime ToDate, string Shift, string MachineID)
        {
            string isGenerated = "";
            try
            {
                string Filename = "ProductionReportKTA.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "ProductionReportKTA" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Scrap Entry Report template does not exists at - " + Source);
                }
                else
                {
                    string Format1 = "hh:mm:ss", Format2 = "hh:mm:ss tt";
                    int rowStart = 1, ColumnStart = 1, rows=7, col=1;
                    FileInfo fileInfo = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(fileInfo, true);
                    var ws = Excel.Workbook.Worksheets[1];
                    DataTable dt = DBAccess.GetProductionreportData(FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd"), Shift, MachineID);
                    if (dt.Rows.Count > 0)
                    {
                        //ws.Cells[row, col].Value = "From Date:";
                        //ws.Cells[row, col + 1, row, col + 3].Merge = true;
                        ws.Cells[4, col+2].Value = FromDate.ToString("dd-MM-yyyy");
                        //ws.Cells[row, col + 4].Value = "To Date:";
                        //ws.Cells[row, col + 6, row, col + 8].Merge = true;            
                        ws.Cells[4, col + 6].Value = ToDate.ToString("dd-MM-yyyy");
                        //row++;

                        foreach (DataRow row in dt.Rows)
                        {
                            col = 1;
                            ws.Cells[rows, col].Value = Convert.ToDateTime(row["Date"].ToString()).ToString("dd-MM-yyyy");
                            col++;
                            ws.Cells[rows, col].Value =row["ShiftName"].ToString();
                            col++;
                            ws.Cells[rows, col].Value =row["machineid"].ToString();
                            col++;
                            ws.Cells[rows, col].Value =row["operatorname"].ToString();
                            col++;
                            ws.Cells[rows, col].Value = string.IsNullOrEmpty(row["OperatorLogin"].ToString()) ? "" : Convert.ToDateTime(row["OperatorLogin"].ToString()).ToString("hh:mm:ss tt"); 
                            col++;
                            ws.Cells[rows, col].Value = string.IsNullOrEmpty(row["OperatorLogout"].ToString()) ? "" : Convert.ToDateTime(row["OperatorLogout"].ToString()).ToString("hh:mm:ss tt");
                            col++;
                            ws.Cells[rows, col].Value = string.IsNullOrEmpty(row["FirstCycleStartTime"].ToString()) ? "" : Convert.ToDateTime(row["FirstCycleStartTime"].ToString()).ToString("hh:mm:ss tt");
                            col++;
                            ws.Cells[rows, col].Value = string.IsNullOrEmpty(row["LastCycleEndTime"].ToString()) ? "" : Convert.ToDateTime(row["LastCycleEndTime"].ToString()).ToString("hh:mm:ss tt");
                            col++;

                            setTimeSpanFormat(Format1, ws, rows, col, row["totaltime"].ToString());
                            //ws.Cells[rows, col].Value = row["totaltime"].ToString() != "" ? (row["totaltime"]) : "";
                            col++;
                            setTimeSpanFormat(Format1, ws, rows, col, row["UtilizedTime"].ToString());
                           // ws.Cells[rows, col].Value = row["UtilizedTime"].ToString() != "" ? (row["UtilizedTime"]) : "";
                            col++;
                            setTimeSpanFormat(Format1, ws, rows, col, row["DownTime_ML"].ToString());
                            //ws.Cells[rows, col].Value = row["DownTime_ML"].ToString() != "" ? (row["DownTime_ML"]) : "";
                            col++;
                            ws.Cells[rows, col].Value = string.IsNullOrEmpty(row["LunchDinner_Start"].ToString()) ? "" : Convert.ToDateTime(row["LunchDinner_Start"].ToString()).ToString("hh:mm:ss tt");
                            col++;

                            ws.Cells[rows, col].Value = string.IsNullOrEmpty(row["LunchDinner_End"].ToString()) ? "" : Convert.ToDateTime(row["LunchDinner_End"].ToString()).ToString("hh:mm:ss tt");
                            col++;
                            setTimeSpanFormat(Format1, ws, rows, col, row["TotalLunchDinner"].ToString());
                            //ws.Cells[rows, col].Value = row["TotalLunchDinner"].ToString() != "" ? (row["TotalLunchDinner"]) : "";
                            col++;
                            ws.Cells[rows, col].Value = HelperClassGeneric.getDoubleValueFromString(row["PartCount"].ToString());
                            col++;
                            ws.Cells[rows, col].Value = HelperClassGeneric.getDoubleValueFromString(row["AEpercentage"].ToString());
                            col++;
                            ws.Cells[rows, col].Value = HelperClassGeneric.getDoubleValueFromString(row["PEpercentage"].ToString());
                            col++;
                            ws.Cells[rows, col].Value = HelperClassGeneric.getDoubleValueFromString(row["QEpercentage"].ToString());
                            col++;
                            ws.Cells[rows, col].Value = HelperClassGeneric.getDoubleValueFromString(row["OEEpercentage"].ToString());
                            ws.Row(rows).Height = Convert.ToDouble(25);
                            rows++;
                        }


                        setBorderThin(ws, 7, 1, rows-1, col);
                        DownloadMultipleFile(destination, Excel.GetAsByteArray());
                        isGenerated = "Generated";
                    }
                    else
                    {
                        isGenerated = "NodataFound";
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("GenerateProductionReport:" + ex.Message);
            }
            return isGenerated;
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
                else if (timeFormat.Equals("hh:mm", StringComparison.OrdinalIgnoreCase))
                {
                    //TimeSpan timeSpan = TimeSpan.Parse(dt.Rows[i][item].ToString());
                    var valueSplit = value.Split(':');
                    TimeSpan timeSpan = new TimeSpan(int.Parse(valueSplit[0]),    // hours
                                                     int.Parse(valueSplit[1]),    // minutes
                                                     0);
                    worksheet.Cells[rowPos, colPos].Value = timeSpan;
                    worksheet.Cells[rowPos, colPos].Style.Numberformat.Format = "[h]:mm";
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
    }
}