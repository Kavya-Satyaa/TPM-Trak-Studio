using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.MachineConnect.Model
{
    public class MachineConnectGenerateReport
    {
        static string appPath = HttpContext.Current.Server.MapPath("~/MachineConnect/Model/ReportTemplate");
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
        private static void setCellColor(ExcelRange modelTable1)
        {
            modelTable1.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            modelTable1.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            modelTable1.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            modelTable1.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            Color colFromHex1 = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            modelTable1.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            modelTable1.Style.Fill.BackgroundColor.SetColor(colFromHex1);
        }

        internal static string GenerateOffsetHistoryReport(string fromDate, string toDate, string machine, List<WearOffsetEntity> list)
        {
            string generated = "";
            try
            {
                string Filename = "OffsetChangeHistoryReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "OffsetChangeHistoryReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("OffsetChangeHistoryReport template does not found at - \n " + Source);
                }
                else
                {
                    if (list.Count == 0)
                    {
                        generated = "NoData";
                        return generated;
                    }
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets["Sheet1"];
                    int rowpos = 7;
                    foreach (WearOffsetEntity data in list)
                    {
                        worksheet.Cells[rowpos, 1].Value = data.Timestamp;
                        worksheet.Cells[rowpos, 1].Style.Numberformat.Format = "yyyy-MM-dd hh:mm:ss";
                        worksheet.Cells[rowpos, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowpos, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[rowpos, 2].Value = data.OffsetNo;
                        worksheet.Cells[rowpos, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowpos, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[rowpos, 3].Value = data.MachineMode;
                        worksheet.Cells[rowpos, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowpos, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[rowpos, 4].Value = data.ProgramNo;
                        worksheet.Cells[rowpos, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowpos, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[rowpos, 5].Value = data.OffsetX;
                        worksheet.Cells[rowpos, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowpos, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[rowpos, 6].Value = data.OffsetZ;
                        worksheet.Cells[rowpos, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowpos, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Row(rowpos).Height = 25;
                        rowpos++;
                    }
                    worksheet.Cells["B2"].Value = fromDate;
                    worksheet.Cells["D2"].Value = toDate;
                    worksheet.Cells["B4"].Value = machine;
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
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
        private static string GetTimeInHourMinutesSeconds(TimeSpan timeSpan)
        {
            return (((int)timeSpan.TotalHours).ToString("0#") + ":" + timeSpan.Minutes.ToString("0#") + ":" + timeSpan.Seconds.ToString("0#"));
        }
        public static string ExportStoppageReport(List<StoppageDataEntity> StoppageDuration, string fromDate, string toDate, string plant, string param)
        {
            if (StoppageDuration == null || StoppageDuration.Count == 0)
            {
                return "NoData";
            }
            string generated = "";
            object misValue = System.Reflection.Missing.Value;
            try
            {
                string Filename = "";
                string Source = "";
                string Template = "";
                if (param == "Hour")
                {
                    //if (ConfigurationManager.AppSettings["language"].ToString().Equals("EN", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    src = Path.Combine(APath, "Reports\\EN\\StoppageReport.xlsx");
                    //}
                    //else if (ConfigurationManager.AppSettings["language"].ToString().Equals("ZH", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    src = Path.Combine(APath, "Reports\\ZH\\StoppageReport.xlsx");
                    //}
                    Filename = "StoppageReport.xlsx";
                    Source = GetReportPath(Filename);
                    if (!File.Exists(Source))
                    {
                        Logger.WriteDebugLog("StoppageReport template does not found at - \n " + Source);
                        return "NoTemplate";
                    }
                    Template = "StoppageReportHourwise" + DateTime.Now + ".xlsx";
                }
                else
                {
                    //if (ConfigurationManager.AppSettings["language"].ToString().Equals("EN", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    src = Path.Combine(APath, "Reports\\EN\\DayOrShiftwise_StoppageReport.xlsx");
                    //}
                    //else if (ConfigurationManager.AppSettings["language"].ToString().Equals("ZH", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    src = Path.Combine(APath, "Reports\\ZH\\DayOrShiftwise_StoppageReport.xlsx");
                    //}
                    Filename = "DayOrShiftwise_StoppageReport.xlsx";
                    Source = GetReportPath(Filename);
                    if (!File.Exists(Source))
                    {
                        Logger.WriteDebugLog("DayOrShiftwise_StoppageReport template does not found at - \n " + Source);
                        return "NoTemplate";
                    }

                    if (param == "Day")
                    {
                        Template = "StoppageReport_DayWise" + DateTime.Now + ".xlsx";
                    }
                    else
                    {
                        Template = "StoppageReport_ShiftWise" + DateTime.Now + ".xlsx";
                    }
                }
                string destination = Path.Combine(appPath, "Temp", SafeFileName(Template));


                FileInfo newFile = new FileInfo(Source);
                ExcelPackage pck = new ExcelPackage(newFile, true);
                var wsDt = pck.Workbook.Worksheets["StoppageData"];

                string workSheetName = wsDt.Name;
                //wsDt.Cells[2, 2].Value = plant;
                //wsDt.Cells[2, 5].Value = param;
                //wsDt.Cells[4, 2].Value = (Convert.ToDateTime(fromDate)).ToString("dd-MMM-yyyy");
                //wsDt.Cells[4, 5].Value = (Convert.ToDateTime(toDate)).ToString("dd-MMM-yyyy");

                wsDt.Cells[3, 1].Value = wsDt.Cells[3, 1].Value + plant;
                // wsDt.Cells[2, 5].Value = param;

                wsDt.Cells[4, 2].Value = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(fromDate)).ToString("yyyy-MM-dd HH:mm:ss");
                wsDt.Cells[4, 5].Value = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(toDate)).ToString("yyyy-MM-dd HH:mm:ss");

                string machineId = string.Empty;
                string date = string.Empty;
                string shift = string.Empty;
                string Shift1 = string.Empty;
                string Shift12 = string.Empty;
                string fromTime = string.Empty;
                string strdate1 = string.Empty;
                string strmachineid = string.Empty;
                int row1 = 7, row = 7, row2 = 7, rowToMergHour = 7, column = 0, Col = 1, row3 = 7;
                object[,] arr1 = new object[StoppageDuration.Count, 10];
                string strDate = string.Empty;
                string strFromTime = string.Empty;
                string strshift = string.Empty;
                int rowShift = row2, rowHour = row2;


                for (int r = 0; r < StoppageDuration.Count; r++)
                {
                    column = 1;
                    var cc = StoppageDuration.Count;


                    if (!string.IsNullOrEmpty(strmachineid) && strmachineid != StoppageDuration[r].MachineID.ToString())
                    {
                        string meargeCellAdd = string.Format("A{0}:A{1}", row1, row2 - 1);
                        string val = string.Format("F{0}:F{1}", row1, row2 - 1);
                        wsDt.Cells[meargeCellAdd].Merge = true;
                        wsDt.Cells[meargeCellAdd].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                        row1 = row2;
                    }

                    #region DAYSHIFT
                    if (param.Equals("Day") || param.Equals("Shift"))
                    {
                        wsDt.Cells[row2, 1].Value = StoppageDuration[r].MachineID;
                        wsDt.Cells[row2, 1].Style.Font.Bold = true;
                        wsDt.Cells[row2, 3].Value = StoppageDuration[r].Shift;

                        #region Shift
                        if (param == "Shift")
                        {

                            if (!string.IsNullOrEmpty(StoppageDuration[r].FromTime))
                            {
                                wsDt.Cells[row2, 2].Value = StoppageDuration[r].FromTime.ToString();
                                if (Shift1 != "Total")
                                {
                                    if (!string.IsNullOrEmpty(strDate) && (strDate != StoppageDuration[r].FromTime || strshift != StoppageDuration[r].Shift.ToString()))
                                    {
                                        string meargeCellAdd = string.Format("B{0}:B{1}", rowShift, row2 - 1);
                                        wsDt.Cells[meargeCellAdd].Merge = true;
                                        wsDt.Cells[meargeCellAdd].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                        string meargeShift = string.Format("C{0}:C{1}", rowShift, row2 - 1);
                                        wsDt.Cells[meargeShift].Merge = true;
                                        wsDt.Cells[meargeShift].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                        rowShift = row2;
                                    }
                                }
                                else if (Shift1 == "Total")
                                {
                                    rowShift = rowShift + 2;
                                }
                                Shift1 = "";
                                strDate = StoppageDuration[r].FromTime;
                                fromTime = StoppageDuration[r].FromTime;
                            }
                            else
                            {
                                if (StoppageDuration[r].FromTime == null)
                                {
                                    wsDt.Cells[row2, 2].Value = "Total";

                                    Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                                    var modelTable1234 = wsDt.Cells[row2, 1, row2, 8];
                                    modelTable1234.Style.Font.Bold = true;
                                    modelTable1234.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    modelTable1234.Style.Fill.BackgroundColor.SetColor(colFromHex);
                                    Shift1 = "Total";
                                }

                                if (StoppageDuration[r].FromTime == null && (strDate != StoppageDuration[r].FromTime || (strshift != StoppageDuration[r].Shift.ToString())))
                                {
                                    string meargeCellAdd = string.Format("B{0}:B{1}", rowShift, row2 - 1);
                                    wsDt.Cells[meargeCellAdd].Merge = true;
                                    wsDt.Cells[meargeCellAdd].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                                    string meargeShift = string.Format("C{0}:C{1}", rowShift, row2 - 1);
                                    wsDt.Cells[meargeShift].Merge = true;
                                    wsDt.Cells[meargeShift].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                    rowShift = row2;
                                    Shift1 = "";
                                }
                            }

                            fromTime = StoppageDuration[r].FromTime;
                        }
                        #endregion
                        #region Day
                        if (param == "Day")
                        {

                            if (!string.IsNullOrEmpty(StoppageDuration[r].FromTime))
                            {
                                wsDt.Cells[row2, 2].Value = StoppageDuration[r].FromTime.ToString();


                                if (!string.IsNullOrEmpty(strDate) && ((strDate != StoppageDuration[r].FromTime || (strmachineid != StoppageDuration[r].MachineID.ToString()))))
                                {
                                    if (Shift1 != "Total")
                                    {
                                        string meargeCellAdd = string.Format("B{0}:B{1}", row3, row2 - 1);

                                        wsDt.Cells[meargeCellAdd].Merge = true;
                                        wsDt.Cells[meargeCellAdd].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                        row3 = row2;
                                    }
                                    else if (Shift1 == "Total")
                                    {
                                        row3 = row3 + 2;
                                    }
                                    Shift1 = "";
                                }

                                strDate = StoppageDuration[r].FromTime;
                                fromTime = StoppageDuration[r].FromTime;
                            }
                            else
                            {
                                if (StoppageDuration[r].FromTime == null)
                                {
                                    wsDt.Cells[row2, 2].Value = "Total";

                                    Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                                    var modelTable1234 = wsDt.Cells[row2, 1, row2, 7];
                                    modelTable1234.Style.Font.Bold = true;
                                    modelTable1234.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    modelTable1234.Style.Fill.BackgroundColor.SetColor(colFromHex);
                                    Shift1 = "Total";
                                }
                                if (StoppageDuration[r].FromTime == null && (strDate != StoppageDuration[r].FromTime || (strmachineid != StoppageDuration[r].MachineID.ToString())))
                                {
                                    string meargeCellAdd = string.Format("B{0}:B{1}", row3, row2 - 1);
                                    wsDt.Cells[meargeCellAdd].Merge = true;
                                    wsDt.Cells[meargeCellAdd].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                    row3 = row2;
                                    Shift1 = "";
                                }

                            }
                            if ((wsDt.Cells[row2, 2].Value.ToString()).Equals("Total"))
                            {
                                Color colFromHex1 = System.Drawing.ColorTranslator.FromHtml("#FFFF00");

                                var modelTable12341 = wsDt.Cells[row2, 1, row2, 7];
                                modelTable12341.Style.Font.Bold = true;
                                modelTable12341.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                                modelTable12341.Style.Fill.BackgroundColor.SetColor(colFromHex1);
                                setThinBorder(wsDt, row2, 1, row2, 7);
                            }
                            else
                            {
                                setThinBorder(wsDt, row2, 1, row2, 7);
                                setCellColor(wsDt.Cells[row2, 1, row2, 7]);
                            }
                            fromTime = StoppageDuration[r].FromTime;

                        }
                        #endregion
                        if ((wsDt.Cells[row2, 2].Value.ToString()).Equals("Total"))
                        {
                            Color colFromHex1 = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                            var modelTable12341 = wsDt.Cells[row2, 1, row2, 8];
                            modelTable12341.Style.Font.Bold = true;
                            modelTable12341.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                            modelTable12341.Style.Fill.BackgroundColor.SetColor(colFromHex1);
                            setThinBorder(wsDt, row2, 1, row2, 8);
                        }
                        else
                        {
                            setThinBorder(wsDt, row2, 1, row2, 8);
                            setCellColor(wsDt.Cells[row2, 1, row2, 8]);
                        }
                        wsDt.Cells[row2, 4].Value = StoppageDuration[r].BatchStart;
                        wsDt.Cells[row2, 5].Value = StoppageDuration[r].BatchEnd;

                        TimeSpan ts = TimeSpan.FromSeconds(StoppageDuration[r].StoppagetimeInInt);

                        wsDt.Cells[row2, 6].Value = GetTimeInHourMinutesSeconds(ts);
                        wsDt.Cells[row2, 7].Value = Math.Round(StoppageDuration[r].StoppagetimeInInt / 60, 2);
                        wsDt.Cells[row2, 8].Value = StoppageDuration[r].Reason;
                        row2 = row2 + 1;

                        strmachineid = StoppageDuration[r].MachineID;
                        strshift = StoppageDuration[r].Shift;
                    }
                    #endregion

                    #region Hour
                    if (param.Equals("Hour"))
                    {
                        wsDt.Cells[row2, 1].Value = StoppageDuration[r].MachineID;

                        if (!string.IsNullOrEmpty(StoppageDuration[r].Date))
                        {
                            wsDt.Cells[row2, 2].Value = StoppageDuration[r].Date.ToString();
                            if (Shift1 != "Total")
                            {
                                if (!string.IsNullOrEmpty(strDate) && (strDate != StoppageDuration[r].Date || strshift != StoppageDuration[r].Shift.ToString()))
                                {
                                    string meargeShift = string.Format("C{0}:C{1}", rowShift, row2 - 1);
                                    wsDt.Cells[meargeShift].Merge = true;
                                    wsDt.Cells[meargeShift].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                    rowShift = row2;
                                }

                                if (!string.IsNullOrEmpty(strDate) && (fromTime != StoppageDuration[r].FromTime || strshift != StoppageDuration[r].Shift.ToString()))
                                {
                                    string mergeFromTime = string.Format("D{0}:D{1}", rowHour, row2 - 1);
                                    wsDt.Cells[mergeFromTime].Merge = true;
                                    wsDt.Cells[mergeFromTime].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                    string mergeToTime = string.Format("E{0}:E{1}", rowHour, row2 - 1);
                                    wsDt.Cells[mergeToTime].Merge = true;
                                    wsDt.Cells[mergeToTime].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                    rowHour = row2;
                                }

                            }
                            else if (Shift1 == "Total")
                            {
                                rowShift = rowShift + 2;
                                rowHour = rowHour + 2;
                            }


                            if (!string.IsNullOrEmpty(strDate) && ((strDate != StoppageDuration[r].Date || (strmachineid != StoppageDuration[r].MachineID.ToString()))))
                            {
                                if (Shift1 != "Total")
                                {
                                    string meargeCellAdd = string.Format("B{0}:B{1}", row3, row2 - 1);
                                    wsDt.Cells[meargeCellAdd].Merge = true;
                                    wsDt.Cells[meargeCellAdd].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                    row3 = row2;
                                }
                                else if (Shift1 == "Total")
                                {
                                    row3 = row3 + 2;
                                }
                                Shift1 = "";
                            }

                            strDate = StoppageDuration[r].Date;
                            fromTime = StoppageDuration[r].FromTime;
                        }
                        else
                        {
                            if (StoppageDuration[r].Date == null)
                            {
                                wsDt.Cells[row2, 2].Value = "Total";

                                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                                var modelTable1234 = wsDt.Cells[row2, 1, row2, 10];
                                modelTable1234.Style.Font.Bold = true;
                                modelTable1234.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                modelTable1234.Style.Fill.BackgroundColor.SetColor(colFromHex);
                                Shift1 = "Total";
                            }
                            if (StoppageDuration[r].Date == null && (strDate != StoppageDuration[r].Date || (strmachineid != StoppageDuration[r].MachineID.ToString())))
                            {
                                string meargeCellAdd = string.Format("B{0}:B{1}", row3, row2 - 1);
                                wsDt.Cells[meargeCellAdd].Merge = true;
                                wsDt.Cells[meargeCellAdd].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                                row3 = row2;
                                Shift1 = "";
                            }
                            if (StoppageDuration[r].Date == null && (strDate != StoppageDuration[r].Date || (strshift != StoppageDuration[r].Shift.ToString())))
                            {
                                string meargeShift = string.Format("C{0}:C{1}", rowShift, row2 - 1);
                                wsDt.Cells[meargeShift].Merge = true;
                                wsDt.Cells[meargeShift].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                rowShift = row2;
                                Shift1 = "";
                            }

                            if (StoppageDuration[r].Date == null && (fromTime != StoppageDuration[r].FromTime || strshift != StoppageDuration[r].Shift.ToString()))
                            {
                                string mergeFromTime = string.Format("D{0}:D{1}", rowHour, row2 - 1);
                                wsDt.Cells[mergeFromTime].Merge = true;
                                wsDt.Cells[mergeFromTime].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                string mergeToTime = string.Format("E{0}:E{1}", rowHour, row2 - 1);
                                wsDt.Cells[mergeToTime].Merge = true;
                                wsDt.Cells[mergeToTime].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                rowHour = row2;
                                Shift1 = "";
                            }
                        }
                        if ((wsDt.Cells[row2, 2].Value.ToString()).Equals("Total"))
                        {
                            Color colFromHex1 = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                            var modelTable12341 = wsDt.Cells[row2, 1, row2, 10];
                            modelTable12341.Style.Font.Bold = true;
                            modelTable12341.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            modelTable12341.Style.Fill.BackgroundColor.SetColor(colFromHex1);
                            setThinBorder(wsDt, row2, 1, row2, 10);
                        }
                        else
                        {
                            setThinBorder(wsDt, row2, 1, row2, 10);
                            setCellColor(wsDt.Cells[row2, 1, row2, 10]);
                        }
                        fromTime = StoppageDuration[r].FromTime;

                        wsDt.Cells[row2, 3].Value = StoppageDuration[r].Shift;
                        wsDt.Cells[row2, 4].Value = StoppageDuration[r].FromTime;
                        wsDt.Cells[row2, 5].Value = StoppageDuration[r].ToTime;
                        wsDt.Cells[row2, 6].Value = StoppageDuration[r].BatchStart;
                        wsDt.Cells[row2, 7].Value = StoppageDuration[r].BatchEnd;
                        TimeSpan ts = TimeSpan.FromSeconds(StoppageDuration[r].StoppagetimeInInt);
                        wsDt.Cells[row2, 8].Value = GetTimeInHourMinutesSeconds(ts);
                        wsDt.Cells[row2, 9].Value = Math.Round(StoppageDuration[r].StoppagetimeInInt / 60, 2);
                        wsDt.Cells[row2, 10].Value = StoppageDuration[r].Reason;
                        row2 = row2 + 1;
                        strmachineid = StoppageDuration[r].MachineID;
                        strshift = StoppageDuration[r].Shift;
                    }
                    #endregion
                }

                if (!string.IsNullOrEmpty(strmachineid))
                {
                    string meargeCellAdd = string.Format("A{0}:A{1}", row1, row2 - 1);
                    if (param.Equals("Hour"))
                    {
                        setCellColor(wsDt.Cells[row1, 1, row2 - 2, 1]);
                    }
                    else
                    {
                        setCellColor(wsDt.Cells[row1, 1, row2 - 2, 1]);
                    }
                    wsDt.Cells[meargeCellAdd].Merge = true;
                    wsDt.Cells[meargeCellAdd].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                }

                if (param.Equals("Day"))
                {
                    wsDt.Column(3).Width = 0;
                }
                DownloadMultipleFile(destination, pck.GetAsByteArray());
                generated = "Generated";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {


            }
            return generated;
        }
        public static string ExportProductionReportEpplus(List<ProductionData> details, ProductionAnalysisForExcelSummary summaryData, List<string> cumProgramNumber,
    List<string> cumPartsCount, string fromDate, string toDate, string plant, string param, List<string> cumOEE)
        {
            string generated = "";
            #region
            if (details == null || details.Count == 0)
            {
                return "NoData";
            }
            object misValue = System.Reflection.Missing.Value;
            try
            {
                string Filename = "";
                string Source = "";
                string Template = "";
                if (param == "Hour")
                {
                    //if (ConfigurationManager.AppSettings["language"].ToString().Equals("EN", StringComparison.OrdinalIgnoreCase))
                    //{
                    //src = Path.Combine(APath, "Reports\\EN\\ShiftAnalysisReport.xlsx");
                    //}
                    //else if (ConfigurationManager.AppSettings["language"].ToString().Equals("ZH", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    src = Path.Combine(APath, "Reports\\ZH\\ShiftAnalysisReport.xlsx");
                    //}

                    Filename = "ShiftAnalysisReport.xlsx";
                    Source = GetReportPath(Filename);
                    if (!File.Exists(Source))
                    {
                        Logger.WriteDebugLog("ProductionAnalysisReport template does not found at - \n " + Source);
                        return "NoTemplate";
                    }
                    if (param == "Day")
                    {
                        Template = "ProductionAnalysisReportDayWise" + DateTime.Now + ".xlsx";

                    }
                    else
                    {
                        Template = "ProductionAnalysisReportShiftWise" + DateTime.Now + ".xlsx";
                    }
                }
                else
                {
                    //if (ConfigurationManager.AppSettings["language"].ToString().Equals("EN", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    src = Path.Combine(APath, "Reports\\EN\\ProductionAnalysisReport.xlsx");
                    //}
                    //else if (ConfigurationManager.AppSettings["language"].ToString().Equals("ZH", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    src = Path.Combine(APath, "Reports\\ZH\\ProductionAnalysisReport.xlsx");
                    //}
                    Filename = "ProductionAnalysisReport.xlsx";
                    Source = GetReportPath(Filename);
                    if (!File.Exists(Source))
                    {
                        Logger.WriteDebugLog("ProductionAnalysisReport template does not found at - \n " + Source);
                        return "NoTemplate";
                    }
                    if (param == "Day")
                    {
                        Template = "ProductionAnalysisReportDayWise" + DateTime.Now + ".xlsx";

                    }
                    else
                    {
                        Template = "ProductionAnalysisReportShiftWise" + DateTime.Now + ".xlsx";
                    }

                }
                string destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                FileInfo newFile = new FileInfo(Source);
                ExcelPackage pck = new ExcelPackage(newFile, true);
                var wsDt = pck.Workbook.Worksheets["ProductionAnalysisReport"];
                var wrkSht2 = pck.Workbook.Worksheets["ProductionAnalysisSummary"];

                #endregion

                string workSheetName = wsDt.Name;
                wsDt.Cells[3, 1].Value = wsDt.Cells[3, 1].Value + plant;
                // wsDt.Cells[2, 5].Value = param;
                wsDt.Cells[4, 2].Value = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(fromDate)).ToString("yyyy-MM-dd HH:mm:ss");
                wsDt.Cells[4, 5].Value = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(toDate)).ToString("yyyy-MM-dd HH:mm:ss");

                string machineId = string.Empty;
                string date = string.Empty;
                string shift = string.Empty;
                string Shift1 = string.Empty;
                string Shift12 = string.Empty;
                string fromTime = string.Empty;
                string strdate1 = string.Empty;
                int row1 = 7, row = 7, row2 = 7, rowToMergHour = 7, column = 0, Col = 1, row3 = 7;
                int rowShift = row2;
                if (param.Equals("Day"))
                {
                    wsDt.DeleteColumn(3);
                }
                object[,] arr1 = new object[details.Count, 10];
                string strmachineid = string.Empty;
                string strDate = string.Empty;
                string strFromTime = string.Empty;
                string strshift = string.Empty;
                for (int r = 0; r < details.Count; r++)
                {
                    column = 1;
                    var cc = details.Count;


                    if (!string.IsNullOrEmpty(strmachineid) && strmachineid != details[r].MachineID.ToString())
                    {
                        string meargeCellAdd = string.Format("A{0}:A{1}", row1, row2 - 1);
                        wsDt.Cells[meargeCellAdd].Merge = true;
                        row1 = row2;
                    }
                    wsDt.Cells[row2, column].Value = details[r].MachineID;
                    wsDt.Cells[row2, column].Style.Font.Bold = true;
                    column = column + 1;

                    #region Shift
                    if (param == "Shift")
                    {

                        if (!string.IsNullOrEmpty(details[r].Shift))
                        {
                            if (Convert.ToDateTime(details[r].Date) != DateTime.MinValue)
                            {
                                wsDt.Cells[row2, column].Value = Convert.ToDateTime(details[r].Date).ToString("dd-MMM-yyyy");
                            }
                            column = column + 1;
                            wsDt.Cells[row2, column].Value = details[r].Shift;
                            column = column + 1;

                            if (!string.IsNullOrEmpty(strdate1) && ((strdate1 != details[r].Date)))
                            {
                                if (Convert.ToDateTime(details[r].Date) != DateTime.MinValue)
                                {
                                    string mearg112 = string.Format("B{0}:B{1}", row, row2 - 1);
                                    wsDt.Cells[mearg112].Merge = true;
                                    row = row2;
                                }
                                else
                                {
                                    string mearg112 = string.Format("B{0}:B{1}", row, row2 - 1);
                                    wsDt.Cells[mearg112].Merge = true;
                                    row = row2 + 1;
                                }
                            }
                            strdate1 = details[r].Date;
                            if (!string.IsNullOrEmpty(strshift) && ((strDate != details[r].Date || strshift != details[r].Shift.ToString() || (strmachineid != details[r].MachineID.ToString()))))
                            {
                                if (Shift1 != "Total")
                                {
                                    string mearge1 = string.Format("D{0}:D{1}", row3, row2 - 1);
                                    string merg112 = string.Format("E{0}:E{1}", row3, row2 - 1);
                                    string meargeCellAdd = string.Format("F{0}:F{1}", row3, row2 - 1);
                                    string mergeOEE = string.Format("J{0}:J{1}", row3, row2 - 1);

                                    wsDt.Cells[meargeCellAdd].Merge = true;
                                    wsDt.Cells[mearge1].Merge = true;
                                    wsDt.Cells[merg112].Merge = true;
                                    wsDt.Cells[mergeOEE].Merge = true;
                                    row3 = row2;
                                }
                                else if (Shift1 == "Total")
                                {
                                    row3 = row3 + 2;
                                }
                                Shift1 = "";
                            }
                            strshift = details[r].Shift;
                            strDate = details[r].Date;
                            fromTime = details[r].FromTime;
                        }
                        else
                        {
                            if (details[r].Shift == null)
                            {
                                wsDt.Cells[row2, 3].Value = "Total";
                                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                                var modelTable1234 = wsDt.Cells[row2, Col, row2, 10];
                                modelTable1234.Style.Font.Bold = true;
                                modelTable1234.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                modelTable1234.Style.Fill.BackgroundColor.SetColor(colFromHex);
                                Shift1 = "Total";
                                Shift12 = "Total";
                                column = column + 1;

                            }
                            if (details[r].Shift == null && (strDate != details[r].Date || strshift != details[r].Shift.ToString() || (strmachineid != details[r].MachineID.ToString())))
                            {
                                string meargeCellAdd = string.Format("F{0}:F{1}", row3, row2 - 1);
                                string mearge1 = string.Format("D{0}:D{1}", row3, row2 - 1);
                                string merg112 = string.Format("E{0}:E{1}", row3, row2 - 1);
                                string mergeOEE = string.Format("J{0}:J{1}", row3, row2 - 1);
                                // setCellColor(wsDt.Cells[row3, 1, row2 - 1, 7]);
                                wsDt.Cells[meargeCellAdd].Merge = true;
                                wsDt.Cells[mearge1].Merge = true;
                                wsDt.Cells[merg112].Merge = true;
                                wsDt.Cells[mergeOEE].Merge = true;
                                row3 = row2;
                                Shift1 = "";
                                Shift12 = "";
                            }
                        }

                        if ((wsDt.Cells[row2, 3].Value.ToString()).Equals("Total"))
                        {
                            Color colFromHex1 = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                            var modelTable12341 = wsDt.Cells[row2, Col, row2, 10];
                            modelTable12341.Style.Font.Bold = true;
                            modelTable12341.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            modelTable12341.Style.Fill.BackgroundColor.SetColor(colFromHex1);
                            setThinBorder(wsDt, row2, 1, row2, 10);
                        }
                        else
                        {
                            setThinBorder(wsDt, row2, 1, row2, 10);
                            setCellColor(wsDt.Cells[row2, 1, row2, 10]);
                        }
                        fromTime = details[r].Date;
                        strshift = details[r].Shift;
                    }
                    #endregion

                    #region Day
                    if (param == "Day")
                    {

                        if (!string.IsNullOrEmpty(details[r].FromTime))
                        {
                            wsDt.Cells[row2, column].Value = Convert.ToDateTime(details[r].FromTime).ToString("dd-MMM-yyyy");
                            column = column + 1;

                            if (!string.IsNullOrEmpty(strDate) && ((strDate != details[r].FromTime || (strmachineid != details[r].MachineID.ToString()))))
                            {
                                if (Shift1 != "Total")
                                {
                                    string meargeCellAdd = string.Format("C{0}:C{1}", row3, row2 - 1);
                                    string mearge1 = string.Format("D{0}:D{1}", row3, row2 - 1);
                                    string merg112 = string.Format("E{0}:E{1}", row3, row2 - 1);
                                    string mergeOEE = string.Format("I{0}:I{1}", row3, row2 - 1);
                                    wsDt.Cells[meargeCellAdd].Merge = true;
                                    wsDt.Cells[mearge1].Merge = true;
                                    wsDt.Cells[merg112].Merge = true;
                                    wsDt.Cells[mergeOEE].Merge = true;
                                    row3 = row2;
                                }
                                else if (Shift1 == "Total")
                                {
                                    row3 = row3 + 2;
                                }
                                Shift1 = "";
                            }
                            strDate = details[r].FromTime;
                            fromTime = details[r].FromTime;
                        }
                        else
                        {
                            if (details[r].FromTime == null)
                            {
                                wsDt.Cells[row2, 2].Value = "Total";

                                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                                var modelTable1234 = wsDt.Cells[row2, Col, row2, 8];//sa
                                modelTable1234.Style.Font.Bold = true;
                                modelTable1234.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                modelTable1234.Style.Fill.BackgroundColor.SetColor(colFromHex);
                                Shift1 = "Total";

                                column = column + 1;

                            }
                            if (details[r].FromTime == null && (strDate != details[r].FromTime || (strmachineid != details[r].MachineID.ToString())))
                            {


                                string meargeCellAdd = string.Format("C{0}:C{1}", row3, row2 - 1);
                                string mearge1 = string.Format("D{0}:D{1}", row3, row2 - 1);
                                string merg112 = string.Format("E{0}:E{1}", row3, row2 - 1);
                                string mergeOEE = string.Format("I{0}:I{1}", row3, row2 - 1);
                                wsDt.Cells[meargeCellAdd].Merge = true;
                                wsDt.Cells[mearge1].Merge = true;
                                wsDt.Cells[merg112].Merge = true;
                                wsDt.Cells[mergeOEE].Merge = true;
                                row3 = row2;
                                Shift1 = "";
                            }

                        }
                        if ((wsDt.Cells[row2, 2].Value.ToString()).Equals("Total"))
                        {
                            Color colFromHex1 = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                            var modelTable12341 = wsDt.Cells[row2, Col, row2, 9];//sa
                            modelTable12341.Style.Font.Bold = true;
                            modelTable12341.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                            modelTable12341.Style.Fill.BackgroundColor.SetColor(colFromHex1);
                            setThinBorder(wsDt, row2, 1, row2, 9);//sa
                        }
                        else
                        {
                            setThinBorder(wsDt, row2, 1, row2, 9);
                            setCellColor(wsDt.Cells[row2, 1, row2, 9]);
                        }
                        fromTime = details[r].FromTime;

                    }
                    #endregion

                    #region hour commented

                    //if (param == "Hour")
                    //{
                    //    if (!string.IsNullOrEmpty(details[r].Date))
                    //    {

                    //        wsDt.Cells[row2, column].Value = Convert.ToDateTime(details[r].Date).ToString("dd-MMM-yyyy");
                    //        column = column + 1;
                    //        wsDt.Cells[row2, column].Value = details[r].Shift;
                    //        column = column + 1;
                    //        wsDt.Cells[row2, column].Value = details[r]._FromTime;
                    //        column = column + 1;
                    //        wsDt.Cells[row2, column].Value = details[r]._ToTime;
                    //        column = column + 1;


                    //        if (!string.IsNullOrEmpty(fromTime) && fromTime == details[r]._FromTime && (strmachineid == details[r].MachineID.ToString()))
                    //        {

                    //            string meargeCellAdd = string.Format("F{0}:F{1}", row3, row2);
                    //            string mearge1 = string.Format("G{0}:G{1}", row3, row2);
                    //            string merg112 = string.Format("H{0}:H{1}", row3, row2);
                    //            wsDt.Cells[meargeCellAdd].Merge = true;
                    //            wsDt.Cells[mearge1].Merge = true;
                    //            wsDt.Cells[merg112].Merge = true;

                    //        }

                    //        fromTime = details[r]._FromTime;
                    //        row3 = row2;

                    //        if (!string.IsNullOrEmpty(strDate) && ((strDate != details[r].Date.ToString()) || (strmachineid != details[r].MachineID.ToString())))
                    //        {
                    //            if (Shift1 != "Total")
                    //            {
                    //                Shift1 = "";
                    //                string meargeCellAdd = string.Format("B{0}:B{1}", row, row2 - 1);
                    //                setCellColor(wsDt.Cells[row, 1, row2 - 1, 11]);
                    //                wsDt.Cells[meargeCellAdd].Merge = true;
                    //                row = row2;
                    //            }
                    //            else
                    //            {
                    //                string meargeCellAdd = string.Format("B{0}:B{1}", row, row2 - 2);
                    //                setCellColor(wsDt.Cells[row, 1, row2 - 2, 11]);
                    //                wsDt.Cells[meargeCellAdd].Merge = true;
                    //                row = row2;
                    //                Shift1 = "";
                    //            }
                    //        }
                    //        #region MyRegion

                    //        if (!string.IsNullOrEmpty(strshift) && ((strshift != details[r].Shift.ToString()) || (strDate != details[r].Date.ToString())))
                    //        {

                    //            if (Shift12 != "Total")
                    //            {
                    //                string meargeCellAdd = string.Format("C{0}:C{1}", rowToMergHour, row2 - 1);
                    //                setCellColor(wsDt.Cells[rowToMergHour, 1, row2 - 1, 11]);
                    //                wsDt.Cells[meargeCellAdd].Merge = true;
                    //                rowToMergHour = row2;
                    //                Shift12 = "";
                    //            }
                    //            else
                    //            {

                    //                string meargeCellAdd = string.Format("C{0}:C{1}", rowToMergHour, row2 - 2);
                    //                setCellColor(wsDt.Cells[rowToMergHour, 1, row2 - 2, 11]);
                    //                wsDt.Cells[meargeCellAdd].Merge = true;
                    //                rowToMergHour = row2;
                    //                Shift12 = "";
                    //            }
                    //        }

                    //        #endregion

                    //        strDate = details[r].Date;
                    //        strshift = details[r].Shift;
                    //    }
                    //    else
                    //    {
                    //        if (details[r].Date == null)
                    //        {
                    //            var modelTable12 = wsDt.Cells[row2, Col, row2, 11];
                    //            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                    //            modelTable12.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //            modelTable12.Style.Fill.BackgroundColor.SetColor(colFromHex);
                    //            wsDt.Cells[row2, 2].Value = details[r].Shift;

                    //            Shift1 = details[r].Shift;
                    //            Shift12 = details[r].Shift;
                    //            column = column + 4;
                    //        }
                    //    }
                    //}
                    #endregion

                    #region hour
                    if (param == "Hour")
                    {
                        if (!string.IsNullOrEmpty(details[r].FromTime))
                        {
                            wsDt.Cells[row2, column].Value = Convert.ToDateTime(details[r].Date).ToString("dd-MMM-yyyy");
                            column = column + 1;
                            wsDt.Cells[row2, column].Value = details[r].Shift;
                            column = column + 1;
                            wsDt.Cells[row2, column].Value = details[r].FromTime;
                            column = column + 1;
                            wsDt.Cells[row2, column].Value = details[r].ToTime;
                            column = column + 1;
                            //  strshift = details[r].Shift;
                            if (!string.IsNullOrEmpty(strDate) && ((strDate != details[r].FromTime || (strmachineid != details[r].MachineID.ToString()))))
                            {
                                if (Shift1 != "Total")
                                {
                                    if (!string.IsNullOrEmpty(strDate) && (strshift != details[r].Shift.ToString()))
                                    {
                                        string meargeShift = string.Format("C{0}:C{1}", rowToMergHour, row2 - 1);
                                        wsDt.Cells[meargeShift].Merge = true;
                                        wsDt.Cells[meargeShift].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                        rowToMergHour = row2;
                                    }
                                    if (!string.IsNullOrEmpty(strDate) && (strdate1 != details[r].Date || strshift != details[r].Shift.ToString()))
                                    {
                                        string meargeShift = string.Format("B{0}:B{1}", rowShift, row2 - 1);
                                        wsDt.Cells[meargeShift].Merge = true;
                                        wsDt.Cells[meargeShift].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                        rowShift = row2;
                                    }
                                    string meargeCellAdd = string.Format("F{0}:F{1}", row3, row2 - 1);
                                    string mearge1 = string.Format("G{0}:G{1}", row3, row2 - 1);
                                    string merg112 = string.Format("H{0}:H{1}", row3, row2 - 1);
                                    string mergeOEE = string.Format("L{0}:L{1}", row3, row2 - 1);

                                    wsDt.Cells[meargeCellAdd].Merge = true;
                                    wsDt.Cells[mearge1].Merge = true;
                                    wsDt.Cells[merg112].Merge = true;
                                    wsDt.Cells[mergeOEE].Merge = true;
                                    row3 = row2;
                                }
                                else if (Shift1 == "Total")
                                {
                                    rowToMergHour = rowToMergHour + 2;
                                    rowShift = rowShift + 2;
                                    row3 = row3 + 2;
                                }
                                Shift1 = "";
                            }
                            strdate1 = details[r].Date;
                            strDate = details[r].FromTime;
                            fromTime = details[r].FromTime;
                            strshift = details[r].Shift;
                        }
                        else
                        {
                            if (details[r].FromTime == null)
                            {
                                wsDt.Cells[row2, 3].Value = "Total";

                                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                                var modelTable1234 = wsDt.Cells[row2, Col, row2, 11];
                                modelTable1234.Style.Font.Bold = true;
                                modelTable1234.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                modelTable1234.Style.Fill.BackgroundColor.SetColor(colFromHex);
                                Shift1 = "Total";

                                column = column + 4;

                            }
                            if (details[r].Date == null && (strDate != details[r].Date || (strshift != details[r].Shift.ToString())))
                            {
                                string meargeShift = string.Format("B{0}:B{1}", rowShift, row2 - 1);
                                wsDt.Cells[meargeShift].Merge = true;
                                wsDt.Cells[meargeShift].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                rowShift = row2;
                                Shift1 = "";
                            }

                            if (details[r].Date == null && ((strshift != details[r].Shift.ToString())))
                            {
                                string meargeShift = string.Format("C{0}:C{1}", rowToMergHour, row2 - 1);
                                wsDt.Cells[meargeShift].Merge = true;
                                wsDt.Cells[meargeShift].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                                rowToMergHour = row2;
                                Shift1 = "";
                            }

                            if (details[r].FromTime == null && (strDate != details[r].FromTime || (strmachineid != details[r].MachineID.ToString())))
                            {
                                string meargeCellAdd = string.Format("F{0}:F{1}", row3, row2 - 1);
                                string mearge1 = string.Format("G{0}:G{1}", row3, row2 - 1);
                                string merg112 = string.Format("H{0}:H{1}", row3, row2 - 1);
                                string mergeOEE = string.Format("L{0}:L{1}", row3, row2 - 1);

                                wsDt.Cells[meargeCellAdd].Merge = true;
                                wsDt.Cells[mearge1].Merge = true;
                                wsDt.Cells[merg112].Merge = true;
                                wsDt.Cells[mergeOEE].Merge = true;
                                row3 = row2;
                                Shift1 = "";
                            }

                        }


                        if ((wsDt.Cells[row2, 3].Value.ToString()).Equals("Total"))
                        {
                            Color colFromHex1 = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                            var modelTable12341 = wsDt.Cells[row2, Col, row2, 12];
                            modelTable12341.Style.Font.Bold = true;
                            modelTable12341.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                            modelTable12341.Style.Fill.BackgroundColor.SetColor(colFromHex1);
                            setThinBorder(wsDt, row2, 1, row2, 12);
                        }
                        else
                        {
                            setThinBorder(wsDt, row2, 1, row2, 12);
                            setCellColor(wsDt.Cells[row2, 1, row2, 12]);
                        }
                        fromTime = details[r].FromTime;
                        strdate1 = details[r].Date;



                    }
                    #endregion
                    wsDt.Cells[row2, column].Value = details[r].PowerOnTime;
                    column = column + 1;
                    wsDt.Cells[row2, column].Value = details[r].OperationTime;
                    column = column + 1;
                    wsDt.Cells[row2, column].Value = details[r].CuttingTime;
                    column = column + 1;
                    wsDt.Cells[row2, column].Value = details[r].ProgramNoVal;
                    column = column + 1;
                    wsDt.Cells[row2, column].Value = details[r].ProgramComment;
                    column = column + 1;

                    wsDt.Cells[row2, column].Value = details[r].PartsCount;
                    column = column + 1;
                    wsDt.Cells[row2, column].Value = details[r].OEE;
                    column = column + 1;
                    row2 = row2 + 1;
                    strmachineid = details[r].MachineID;
                }

                if (!string.IsNullOrEmpty(strmachineid))
                {
                    string meargeCellAdd = string.Format("A{0}:A{1}", row1, row2 - 1);
                    setCellColor(wsDt.Cells[row1, 1, row2 - 2, 8]);
                    wsDt.Cells[meargeCellAdd].Merge = true;
                }
                if (param == "Shift")
                {
                    if (!string.IsNullOrEmpty(strDate))
                    {
                        string meargeCellAdd = string.Format("B{0}:B{1}", row, row2 - 2);
                        setCellColor(wsDt.Cells[row, 1, row2 - 1, 9]);
                        wsDt.Cells[meargeCellAdd].Merge = true;

                        Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                        var modelTable12345 = wsDt.Cells[row2 - 1, Col, row2 - 1, 9];
                        modelTable12345.Style.Font.Bold = true;
                        modelTable12345.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        modelTable12345.Style.Fill.BackgroundColor.SetColor(colFromHex);
                    }
                }
                if (param == "Day")
                {
                    setThinBorder(wsDt, row2 - 1, 1, row2 - 1, 9);
                    Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                    var modelTable12345 = wsDt.Cells[row2 - 1, Col, row2 - 1, 9];
                    modelTable12345.Style.Font.Bold = true;
                    modelTable12345.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    modelTable12345.Style.Fill.BackgroundColor.SetColor(colFromHex);
                }
                //if (param == "Hour")
                //{
                //    string meargeCellAdd = string.Format("B{0}:B{1}", row, row2 - 2);
                //    setCellColor(wsDt.Cells[row, 1, row2 - 1, 11]);
                //    wsDt.Cells[meargeCellAdd].Merge = true;
                //}

                if (param == "Hour")
                {
                    //if (!string.IsNullOrEmpty(strshift))
                    //{
                    //    string meargeCellAdd = string.Format("C{0}:C{1}", rowToMergHour, row2 - 2);
                    //    setCellColor(wsDt.Cells[rowToMergHour, 1, row2 - 2, 11]);
                    //    wsDt.Cells[meargeCellAdd].Merge = true;
                    //}

                    Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                    var modelTable12345 = wsDt.Cells[row2 - 1, Col, row2 - 1, 12];
                    modelTable12345.Style.Font.Bold = true;
                    modelTable12345.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    modelTable12345.Style.Fill.BackgroundColor.SetColor(colFromHex);
                }

                #region populate Summary

                string workSheetName1 = wrkSht2.Name;
                //wrkSht2.Cells[2, 2].Value = plant;
                //wrkSht2.Cells[2, 5].Value = param;

                //wrkSht2.Cells[4, 2].Value = (Convert.ToDateTime(fromDate)).ToString("dd-MMM-yyyy"); ;
                //wrkSht2.Cells[4, 5].Value = (Convert.ToDateTime(toDate)).ToString("dd-MMM-yyyy");


                wrkSht2.Cells[2, 1].Value = wrkSht2.Cells[2, 1].Value + plant;
                // wsDt.Cells[2, 5].Value = param;

                wrkSht2.Cells[3, 2].Value = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(fromDate)).ToString("yyyy-MM-dd HH:mm:ss");
                wrkSht2.Cells[3, 5].Value = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(toDate)).ToString("yyyy-MM-dd HH:mm:ss");
                var summary = summaryData;

                //wrkSht2.Cells[8, 4].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.TotalTime)) + " Hrs.";
                //wrkSht2.Cells[9, 4].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.TotalTime)) + " Hrs.";

                //wrkSht2.Cells[10, 4].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.PowerOnTime)) + " Hrs.";
                //wrkSht2.Cells[11, 4].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.PowerOnTime)) + " Hrs.";

                //wrkSht2.Cells[12, 4].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.OperatingTime)) + " Hrs.";
                //wrkSht2.Cells[13, 4].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.OperatingTime)) + " Hrs.";

                //wrkSht2.Cells[14, 4].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.CuttingTime)) + " Hrs.";
                //wrkSht2.Cells[15, 4].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.CuttingTime)) + " Hrs.";

                //wrkSht2.Cells[16, 4].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.OperatingWithoutCutting)) + " Hrs.";
                //wrkSht2.Cells[17, 4].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.OperatingWithoutCutting)) + " Hrs.";

                //wrkSht2.Cells[18, 4].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.NonOperatingTime)) + " Hrs.";
                //wrkSht2.Cells[19, 4].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.NonOperatingTime)) + " Hrs.";

                //wrkSht2.Cells[20, 4].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.PowerOffTime)) + " Hrs.";
                //wrkSht2.Cells[21, 4].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.PowerOffTime)) + " Hrs.";

                wrkSht2.Cells[8, 4].Value = summary.TotalTime.ToString() + " mins";
                wrkSht2.Cells[9, 4].Value = summary.TotalTime.ToString() + " mins";

                wrkSht2.Cells[10, 4].Value = summary.PowerOnTime.ToString() + " mins";
                wrkSht2.Cells[11, 4].Value = summary.PowerOnTime.ToString() + " mins";

                wrkSht2.Cells[12, 4].Value = summary.OperatingTime.ToString() + " mins";
                wrkSht2.Cells[13, 4].Value = summary.OperatingTime.ToString() + " mins";

                wrkSht2.Cells[14, 4].Value = summary.CuttingTime.ToString() + " mins";
                wrkSht2.Cells[15, 4].Value = summary.CuttingTime.ToString() + " mins";

                wrkSht2.Cells[16, 4].Value = summary.OperatingWithoutCutting.ToString() + " mins";
                wrkSht2.Cells[17, 4].Value = summary.OperatingWithoutCutting.ToString() + " mins";

                wrkSht2.Cells[18, 4].Value = summary.NonOperatingTime.ToString() + " mins";
                wrkSht2.Cells[19, 4].Value = summary.NonOperatingTime.ToString() + " mins";

                wrkSht2.Cells[20, 4].Value = summary.PowerOffTime.ToString() + " mins";
                wrkSht2.Cells[21, 4].Value = summary.PowerOffTime.ToString() + " mins";

                double totalTime = Math.Round(((summary.TotalTime) / (summary.TotalTime) * 100), 2);
                var operatingTime = (summary.CuttingTime + summary.OperatingWithoutCutting);
                double powerOntimePercent = Math.Round(((summary.PowerOnTime) / (summary.TotalTime) * 100), 2);
                double operatingButNotCuttingTime = Math.Round(((summary.OperatingWithoutCutting) / (summary.TotalTime) * 100), 2);
                double nonoperatingTime = Math.Round(((summary.NonOperatingTime) / (summary.TotalTime) * 100), 2);
                double cuttingTime = Math.Round(((summary.CuttingTime) / (summary.TotalTime) * 100), 2);
                double powerOffTime = Math.Round(((summary.PowerOffTime) / (summary.TotalTime) * 100), 2);
                double powerOnTime = Math.Round(((summary.PowerOnTime) / (summary.TotalTime) * 100), 2);
                double calculatedOpeartingTime = Math.Round(((operatingTime) / (summary.TotalTime) * 100), 2);

                wrkSht2.Cells[8, 5].Value = totalTime + " %";
                wrkSht2.Cells[9, 5].Value = totalTime + " %";

                wrkSht2.Cells[10, 5].Value = powerOnTime + " %";
                wrkSht2.Cells[11, 5].Value = powerOnTime + " %";

                wrkSht2.Cells[12, 5].Value = calculatedOpeartingTime + " %";
                wrkSht2.Cells[13, 5].Value = calculatedOpeartingTime + " %";

                wrkSht2.Cells[14, 5].Value = cuttingTime + " %";
                wrkSht2.Cells[15, 5].Value = cuttingTime + " %";

                wrkSht2.Cells[16, 5].Value = operatingButNotCuttingTime + " %";
                wrkSht2.Cells[17, 5].Value = operatingButNotCuttingTime + " %";

                wrkSht2.Cells[18, 5].Value = nonoperatingTime + " %";
                wrkSht2.Cells[19, 5].Value = nonoperatingTime + " %";

                wrkSht2.Cells[20, 5].Value = powerOffTime + " %";
                wrkSht2.Cells[21, 5].Value = powerOffTime + " %";


                int r1, r2 = 0;
                r1 = 28; r2 = 28;

                int strow = r1;
                if (cumProgramNumber.Count > 0)
                {
                    foreach (var item in cumProgramNumber)
                    {
                        wrkSht2.Cells[r1, 1].Value = item;
                        r1 = r1 + 1;
                    }
                    wrkSht2.Cells[strow, 1, r1 - 1, 1].Style.Font.Bold = true;
                    setThinBorder(wrkSht2, strow, 1, r1 - 1, 1);
                }
                strow = r2;
                if (cumPartsCount.Count > 0)
                {
                    foreach (var item in cumPartsCount)
                    {
                        wrkSht2.Cells[r2, 2].Value = item;
                        r2 = r2 + 1;
                    }
                    wrkSht2.Cells[strow, 2, r2 - 1, 2].Style.Font.Bold = true;
                    setThinBorder(wrkSht2, strow, 2, r2 - 1, 2);
                }
                #endregion

                #region                            

                //To plot Time In Mins.
                wrkSht2.Cells[8, 10].Value = summary.PowerOnTime;
                wrkSht2.Cells[9, 10].Value = summary.PowerOffTime;

                wrkSht2.Cells[10, 9].Value = summary.OperatingTime;
                wrkSht2.Cells[11, 9].Value = summary.NonOperatingTime;

                wrkSht2.Cells[12, 8].Value = summary.CuttingTime;
                wrkSht2.Cells[13, 8].Value = summary.OperatingWithoutCutting;
                //wrkSht2.Cells[26, 11].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.PowerOnTime));
                //wrkSht2.Cells[27, 11].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.PowerOffTime));
                //wrkSht2.Cells[29, 11].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.OperatingTime));
                //wrkSht2.Cells[30, 11].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.NonOperatingTime));
                //wrkSht2.Cells[32, 11].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.CuttingTime));
                //wrkSht2.Cells[33, 11].Value = GetTimeInHourMinutes(TimeSpan.FromMinutes(summary.OperatingWithoutCutting));
                #endregion

                DownloadMultipleFile(destination, pck.GetAsByteArray());
                generated = "Generated";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);

            }
            return generated;
        }
    }
}