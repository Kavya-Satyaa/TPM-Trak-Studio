using Elmah;
using OfficeOpenXml2;
using OfficeOpenXml2.Drawing;
using OfficeOpenXml2.Drawing.Chart;
using OfficeOpenXml2.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public static class TPMTrakGenerateReportNewDll
    {
        static string appPath = HttpContext.Current.Server.MapPath("~/Reports");

        public static string SafeFileName(string name)
        {
            StringBuilder str = new StringBuilder(name);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.ToString();
        }

        private static Double getdouble(string data)
        {
            double Value = 0.00;
            double.TryParse(data, out Value);
            Math.Round(Value, 2);
            return Value;

        }

        #region "Down Load File"
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
                //HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //Logger.WriteErrorLog("GENERATED ERROR : \n" + "Report generation Failed Error: " + ex.ToString());
            }
        }
        #endregion
        #region "Down time report for Machine down time matrix "
        public static string MachineDownTimeMatrix(DateTime StartDate, string PlantId, string MachineId, DateTime EndDate, string DownID, int Exclude, string width, string height, string LimitData, string cellID, string type, string format)
        {
            string Generated = "";
            try
            {
                string proc = "", reportName = "", strtTime = "", endTime = "";
                string totalAvailableTime = "", totalUtilizedTime = "";
                if (type.Equals("aggregate", StringComparison.OrdinalIgnoreCase))
                {
                    proc = "s_GetSONA_ShiftAgg_DowntimeMatrix";
                    reportName = "MachineDownTimeReport.xlsx";
                    strtTime = StartDate.ToString("dd-MMM-yyyy");
                    endTime = EndDate.ToString("dd-MMM-yyyy");
                    DataTable totalTimeData = TMPTrakDataBase.GetProdReportData(StartDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"), "", PlantId == "ALL" ? "" : PlantId, MachineId, "ConsolidatedForMatrixReport", cellID);
                    if (totalTimeData != null)
                    {
                        foreach (DataRow row in totalTimeData.Rows)
                        {
                            totalAvailableTime = row["TotalTime"].ToString();
                            totalUtilizedTime = row["UtilisedTime"].ToString();
                        }
                    }
                }
                else if (type.Equals("standard", StringComparison.OrdinalIgnoreCase))
                {
                    proc = "s_GetDownTimeMatrixfromAutoData";
                    reportName = "MachineDownTimeReport.xlsx";
                    strtTime = StartDate.ToString("dd-MMM-yyyy hh:mm:ss tt");
                    endTime = EndDate.ToString("dd-MMM-yyyy hh:mm:ss tt");
                    DataTable totalTimeData = CockpitDataBaseAccess.GetMachineCockpitTotalData("s_GetCockpitData_WithTempTable_Shanti", StartDate.ToString("yyyy-MM-dd HH:mm:ss"), EndDate.ToString("yyyy-MM-dd HH:mm:ss"), PlantId == "ALL" ? "" : PlantId, MachineId, null, null, "", null, cellID, "Consolidated", "");
                    if (totalTimeData != null)
                    {
                        foreach (DataRow row in totalTimeData.Rows)
                        {
                            totalAvailableTime = row["Totaltime"].ToString();
                            totalUtilizedTime = row["UtilisedTime"].ToString();
                        }
                    }
                }
                if (format.Equals("TimeWise"))
                {
                    reportName = "MachineDownTimeReport_Advik.xlsx";

                }
                string src, dst = string.Empty;

                src = Util.GetReportPath(reportName);
                string tempfileName = "MachineDownTimeReport" + "_" + Guid.NewGuid() + ".xlsx";
                dst = Path.Combine(appPath, "Temp", SafeFileName(tempfileName));
                if (PlantId.Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    PlantId = "";
                if (MachineId.Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    MachineId = "";
                int screenWidth = Convert.ToInt32(width) - 40;
                int screenHeight = Convert.ToInt32(height) - 200;
                if (!File.Exists(src))
                {
                    Logger.WriteDebugLog("Machine Down Time Report Template Not Found at the Path - \n " + src);
                }

                //string str1 = VDGDataBaseAccess.GetLogicalDayStart(StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
                //DateTime strfromdate = DateTime.Now;
                //DateTime.TryParse(str1, out strfromdate);

                //string str2 = VDGDataBaseAccess.GetLogicalDayEnd(EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                //DateTime strtodate = DateTime.Now;
                //DateTime.TryParse(str2, out strtodate);


                if (format.Equals("TimeAndFreqWise"))
                {
                    FileInfo newFile = new FileInfo(src);
                    ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                    ExcelWorksheet downTimeSheet = excelPackage.Workbook.Worksheets[0];
                    ExcelWorksheet ws = excelPackage.Workbook.Worksheets[1];
                    ExcelWorksheet ws2 = excelPackage.Workbook.Worksheets[2];
                    ExcelWorksheet ws3 = excelPackage.Workbook.Worksheets[3];
                    ExcelWorksheet ws4 = excelPackage.Workbook.Worksheets[4];
                    ExcelWorksheet ws5 = excelPackage.Workbook.Worksheets[5];
                    ExcelWorksheet ws6 = excelPackage.Workbook.Worksheets[6];
                    ExcelWorksheet ws7 = excelPackage.Workbook.Worksheets[7];
                    //foreach (ExcelWorksheet sheet in excelPackage.Workbook.Worksheets)
                    //{
                    //    sheet.Name = sheet.Name.Replace("5", LimitData);
                    //}
                    ws.Cells["C2"].Value = "Start Time";
                    ws.Cells["F2"].Value = "End Time";

                    if (type.Equals("aggregate", StringComparison.OrdinalIgnoreCase))
                    {
                        ws.Cells["C2"].Value = "Start Date";
                        ws.Cells["F2"].Value = "End Date";
                        ws.Cells["D2"].Value = StartDate.ToString("dd-MM-yyyy");
                        ws.Cells["G2"].Value = EndDate.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        ws.Cells["C2"].Value = "Start Time";
                        ws.Cells["F2"].Value = "End Time";
                        ws.Cells["D2"].Value = strtTime;
                        ws.Cells["G2"].Value = endTime;
                    }
                    ws.Cells["I2"].Value = "Plants";
                    ws.Cells["J2"].Value = PlantId == "" ? "All" : PlantId;
                    ws.Cells[4, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[4, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    int r = 7, count = 0, c = 3;
                    List<string> lstMachineNames = new List<string>();
                    List<string> lstMacTotal = new List<string>();
                    List<string> lstMacFreqTotal = new List<string>();
                    //List<string> lstRevenueTotal = new List<string>();
                    string mxkSectohhmmss = string.Empty;

                    string PrevDownFreq = string.Empty;
                    string DownIDTotal = string.Empty;
                    string downtime = string.Empty;
                    string PrevMachine = string.Empty;
                    int range = Convert.ToInt32(LimitData) + 6;
                    string Prevdown = string.Empty; string Machinename = "";

                    if (ConfigurationManager.AppSettings["sonapages"].Equals("1"))
                    {
                        Machinename = "machineDescription";
                    }
                    else
                        Machinename = "MachineID";

                    DataTable dt = TMPTrakDataBase.MachineDownTimeMatrix(StartDate, EndDate, MachineId, PlantId, DownID, Exclude, "DTime", cellID, proc, "");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        #region "Frist Sheet Data Plat"
                        #region "Header Part Machine "
                        foreach (DataRow rdr in dt.Rows)
                        {
                            if (count == 0)
                            {
                                Prevdown = rdr["DownCode"].ToString();
                                PrevDownFreq = rdr["DownCode"].ToString();
                                PrevMachine = rdr[Machinename].ToString();
                                lstMachineNames.Add(rdr["MachineID"].ToString());
                                if (ConfigurationManager.AppSettings["sonapages"].Equals("1", StringComparison.OrdinalIgnoreCase))
                                {
                                    ws.Cells[5, c].Value = rdr["MachineID"].ToString();
                                    ws2.Cells[5, c].Value = rdr["MachineID"].ToString();
                                    ws3.Cells[5, c].Value = rdr["MachineID"].ToString();
                                }
                                ws.Cells[5, c].Value = rdr["machineDescription"].ToString();
                                ws2.Cells[5, c].Value = rdr["machineDescription"].ToString();
                                ws3.Cells[5, c].Value = rdr["machineDescription"].ToString();
                                ws.Cells[6, c].Value = PrevMachine;
                                ws2.Cells[6, c].Value = PrevMachine;
                                ws3.Cells[6, c].Value = PrevMachine;
                                lstMacTotal.Add(rdr["TotalMachine"].ToString());
                                lstMacFreqTotal.Add(rdr["TotalMachineFreq"].ToString());
                                //lstRevenueTotal.Add(rdr["TotalRevenueLossPerHour"].ToString());
                            }
                            if (PrevMachine != rdr[Machinename].ToString())
                            {
                                c = c + 1;
                                if (ConfigurationManager.AppSettings["sonapages"].Equals("1", StringComparison.OrdinalIgnoreCase))
                                {
                                    ws.Cells[5, c].Value = rdr["MachineID"].ToString();
                                    ws2.Cells[5, c].Value = rdr["MachineID"].ToString();
                                    ws3.Cells[5, c].Value = rdr["MachineID"].ToString();
                                }
                                ws.Cells[5, c].Value = rdr["machineDescription"].ToString();
                                ws2.Cells[5, c].Value = rdr["machineDescription"].ToString();
                                ws3.Cells[5, c].Value = rdr["machineDescription"].ToString();
                                ws.Cells[6, c].Value = rdr[Machinename].ToString();
                                ws2.Cells[6, c].Value = rdr[Machinename].ToString();
                                ws3.Cells[6, c].Value = rdr[Machinename].ToString();
                                lstMachineNames.Add(rdr["MachineID"].ToString());
                                lstMacTotal.Add(rdr["TotalMachine"].ToString());
                                lstMacFreqTotal.Add(rdr["TotalMachineFreq"].ToString());
                                //lstRevenueTotal.Add(rdr["TotalRevenueLossPerHour"].ToString());
                            }
                            else if (PrevMachine == rdr[Machinename].ToString() && count != 0)
                            {
                                break;
                            }
                            count++;
                        }
                        #endregion

                        #region "Machine Value Define First Sheet"
                        r = 7;
                        c = 3;
                        Prevdown = "";
                        TimeSpan timeSpan = TimeSpan.MinValue;
                        foreach (DataRow rdr in dt.Rows)
                        {
                            if (Prevdown == "" || Prevdown == rdr["DownCode"].ToString())
                            {
                                //ws.Cells[r, 1].Value = rdr["DownCode"].ToString();
                                ws.Cells[r, c].Value = Convert.ToDecimal(rdr["DownTime"].ToString()) / 86400;
                            }
                            else if (Prevdown != rdr["DownCode"].ToString())
                            {
                                r = r + 1;
                                c = 3;
                                ws.Cells[r, c].Value = Convert.ToDecimal(rdr["DownTime"].ToString()) / 86400;
                            }
                            c = c + 1;
                            ws.Cells[r, 1].Value = rdr["DownCode"].ToString();
                            ws.Cells[r, 2].Value = rdr["Owner"].ToString();
                            Prevdown = rdr["DownCode"].ToString();
                        }

                        if (proc == "s_GetDownTimeMatrixfromAutoData")
                        {

                            var DownCodeWiseTotalTime = dt.AsEnumerable().Select(x => new { DownID = x["DownCode"].ToString(), TotalTime = x["TotalDown"].ToString(), RevenueLoss = x["TotalDownRevenueLossInRupees"].ToString() }).Distinct().ToList();
                            var MachineWiseTotalTimeAndRevenue = dt.AsEnumerable().Select(x => new { MachineID = x["MachineID"].ToString(), TotalTime = x["TotalMachine"].ToString(), RevenueLoss = x["TotalMachineRevenueLossInRupees"].ToString() }).Distinct().ToList();

                            int totalr = 7, totalc = c;
                            ws.Cells[6, totalc].Value = "Total Down Time";
                            ws.Cells[6, totalc + 1].Value = "Total Revenue Loss in Rupees";
                            foreach (var value in DownCodeWiseTotalTime)
                            {
                                totalc = c;
                                if (Convert.ToInt32(value.TotalTime) > 0)
                                {
                                    timeSpan = TimeSpan.FromSeconds(Convert.ToDouble(value.TotalTime));
                                    string answer = string.Format("{0:00}:{1:00}:{2:00}",
                                             (int)timeSpan.TotalHours,
                                             timeSpan.Minutes,
                                             timeSpan.Seconds);
                                    //ws.Cells[r + 2, c].Value = Convert.ToDecimal(value) / 86400;
                                    ws.Cells[totalr, totalc].Style.Numberformat.Format = "General";
                                    ws.Cells[totalr, totalc].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    ws.Cells[totalr, totalc].Value = answer;

                                    ws.Cells[totalr, totalc + 1].Style.Numberformat.Format = "General";
                                    ws.Cells[totalr, totalc + 1].Value = Math.Round(Convert.ToDouble(value.RevenueLoss), 2);
                                    totalr++;
                                }
                            }

                            c = 3;
                            ws.Cells[r + 2, 1].Value = "Total Down Time";
                            ws.Cells[r + 3, 1].Value = "Total Revenue Loss in Rupees";
                            foreach (var value in MachineWiseTotalTimeAndRevenue)
                            {
                                if (Convert.ToInt32(value.TotalTime) > 0)
                                {
                                    timeSpan = TimeSpan.FromSeconds(Convert.ToDouble(value.TotalTime));
                                    string answer = string.Format("{0:00}:{1:00}:{2:00}",
                                             (int)timeSpan.TotalHours,
                                             timeSpan.Minutes,
                                             timeSpan.Seconds);
                                    //ws.Cells[r + 2, c].Value = Convert.ToDecimal(value) / 86400;
                                    ws.Cells[r + 2, c].Style.Numberformat.Format = "General";
                                    ws.Cells[r + 2, c].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    ws.Cells[r + 2, c].Value = answer;
                                    ws.Cells[r + 3, c].Style.Numberformat.Format = "General";
                                    ws.Cells[r + 3, c].Value = Math.Round(Convert.ToDouble(value.RevenueLoss), 2);
                                }
                                c = c + 1;

                            }
                        }
                        else
                        {
                            c = 3;
                            ws.Cells[r + 2, 1].Value = "Total Down Time";
                            foreach (var value in lstMacTotal)
                            {
                                if (Convert.ToInt32(value) > 0)
                                {
                                    timeSpan = TimeSpan.FromSeconds(Convert.ToDouble(value));
                                    string answer = string.Format("{0:00}:{1:00}:{2:00}",
                                             (int)timeSpan.TotalHours,
                                             timeSpan.Minutes,
                                             timeSpan.Seconds);
                                    //ws.Cells[r + 2, c].Value = Convert.ToDecimal(value) / 86400;
                                    ws.Cells[r + 2, c].Style.Numberformat.Format = "General";
                                    ws.Cells[r + 2, c].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    ws.Cells[r + 2, c].Value = answer;
                                }
                                c = c + 1;

                            }
                        }


                        ws.Cells[4, 1, 7, 26].AutoFitColumns();
                        #endregion
                        #endregion

                        #region "Second Sheet Start"
                        r = 7;
                        c = 3;
                        PrevDownFreq = "";
                        ws2.Cells["C2"].Value = "Start Time";
                        ws2.Cells["D2"].Value = strtTime;
                        ws2.Cells["F2"].Value = "End Time";
                        ws2.Cells["G2"].Value = endTime;
                        ws2.Cells["I2"].Value = "Plants";
                        ws2.Cells["J2"].Value = PlantId == "" ? "All" : PlantId;
                        ws2.Cells[4, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws2.Cells[4, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        foreach (DataRow rdr in dt.Rows)
                        {
                            if (PrevDownFreq == "" || PrevDownFreq == rdr["DownCode"].ToString())
                            {
                                ws2.Cells[r, c].Value = Convert.ToInt32(rdr["DownFreq"].ToString());
                            }
                            else
                            {
                                r = r + 1;
                                c = 3;
                                ws2.Cells[r, c].Value = Convert.ToInt32(rdr["DownFreq"].ToString());
                            }
                            c = c + 1;
                            ws2.Cells[r, 1].Value = rdr["DownCode"].ToString();
                            ws2.Cells[r, 2].Value = rdr["Owner"].ToString();
                            PrevDownFreq = rdr["DownCode"].ToString();
                        }
                        c = 3;
                        ws2.Cells[r + 2, 1].Value = "Total";

                        foreach (string value in lstMacFreqTotal)
                        {
                            //-------Start --------------
                            int total = 0;
                            if (int.TryParse(value, out total))
                                ws2.Cells[r + 2, c].Value = total;
                            //------END---------------------
                            c = c + 1;

                        }
                        ws2.Cells[4, 1, 7, 26].AutoFitColumns();
                        #endregion

                        #region "Third Sheet Plat Data"
                        #region "MCs by Top-5 Downs"
                        var chart11 = ws3.Drawings["Chart 1"] as ExcelBarChart;
                        chart11.Border.LineStyle = eLineStyle.Solid;
                        chart11.YAxis.Format = "[h]:mm:ss;@";
                        chart11.SetSize(screenWidth, screenHeight);
                        chart11.SetPosition(10, 22);
                        chart11.Title.Text = "Down Time Comparison Graph";
                        chart11.YAxis.Title.Text = "Down Time";
                        string sheetName = ws3.Name.Replace("5", LimitData);
                        for (int i = 2; i < c; i++)
                        {
                            ExcelChartSerie aa = chart11.Series.Add(ws.Cells[7, i, range, i], ws.Cells[7, 1, range, 1]);
                            aa.HeaderAddress = new ExcelAddress("'" + sheetName + "'!" + GetExcelColumnName(i) + "6");
                            chartDataLabelVerticle270(aa as ExcelBarChartSerie, 0);
                        }

                        #endregion

                        #region "Top 5 Down by MC"
                        range = Convert.ToInt32(LimitData) + 6;
                        var barchart = ws4.Drawings["Chart 2"] as ExcelBarChart;
                        barchart.Border.LineStyle = eLineStyle.Solid;
                        // var series = barchart.Series[0];
                        barchart.Title.Text = "Down Time Comparison Graph";
                        barchart.YAxis.Title.Text = "Down Time";
                        barchart.YAxis.Format = "[h]:mm:ss;@";
                        barchart.SetSize(screenWidth, screenHeight + 300);
                        barchart.SetPosition(10, 22);

                        var seriePE = barchart.Series.Add(ws.Cells[7, 2, 7, c], ws.Cells[6, 2, 6, c]);
                        seriePE.Header = ws.Cells["A7"].Value.ToString();
                        chartDataLabelVerticle270(seriePE as ExcelBarChartSerie, 0);
                        for (int i = 8; i <= range; i++)
                        {
                            ExcelChartSerie seri = barchart.Series.Add(ws.Cells[i, 2, i, c], ws.Cells[6, 2, 6, c]);
                            seri.Header = ws.Cells["A" + i].Value == null ? "" : ws.Cells["A" + i].Value.ToString();//new ExcelAddressBase("'Time-wise'!$A$7");
                            chartDataLabelVerticle270(seri as ExcelBarChartSerie, 0);
                        }
                        #endregion
                        #endregion
                    }
                    else
                        Generated = "nodatafound";
                    //ws2.Cells["F2"].Value = strtTime; ws2.Cells["J2"].Value = endTime;
                    #region "Plot Five Sheet Data"
                    DataTable dt2 = TMPTrakDataBase.MachineDownTimeMatrix(StartDate, EndDate, MachineId, PlantId, DownID, Exclude, "DFreq", cellID, proc, "");
                    ws5.Cells["C2"].Value = "Start Time";
                    ws5.Cells["D2"].Value = VDGDataBaseAccess.GetLogicalDayStart(strtTime);
                    ws5.Cells["F2"].Value = "End Time";
                    ws5.Cells["G2"].Value = VDGDataBaseAccess.GetLogicalDayEnd(endTime);
                    ws5.Cells["I2"].Value = "Plants";
                    ws5.Cells["J2"].Value = PlantId == "" ? "All" : PlantId;
                    ws5.Cells[4, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws5.Cells[4, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    r = 7;
                    c = 3;
                    count = 0;
                    if (dt2 != null && dt2.Rows.Count > 0)
                    {
                        #region "Header Part Bind"
                        lstMacFreqTotal.Clear();
                        lstMachineNames.Clear();
                        lstMacTotal.Clear();
                        foreach (DataRow rdr in dt2.Rows)
                        {
                            if (count == 0)
                            {
                                PrevDownFreq = rdr["DownCode"].ToString();
                                PrevMachine = rdr[Machinename].ToString();
                                lstMachineNames.Add(rdr["MachineID"].ToString());
                                if (ConfigurationManager.AppSettings["sonapages"].Equals("1", StringComparison.OrdinalIgnoreCase))
                                {
                                    ws5.Cells[5, c].Value = rdr["MachineID"].ToString();
                                }
                                ws5.Cells[5, c].Value = rdr["machineDescription"].ToString();
                                ws5.Cells[6, c].Value = PrevMachine;
                                lstMacFreqTotal.Add(rdr["TotalMachineFreq"].ToString());
                            }
                            if (PrevMachine != rdr[Machinename].ToString())
                            {
                                c = c + 1;
                                if (ConfigurationManager.AppSettings["sonapages"].Equals("1", StringComparison.OrdinalIgnoreCase))
                                {
                                    ws5.Cells[5, c].Value = rdr["MachineID"].ToString();
                                }
                                ws5.Cells[5, c].Value = rdr["machineDescription"].ToString();
                                ws5.Cells[6, c].Value = rdr[Machinename].ToString();
                                lstMachineNames.Add(rdr["MachineID"].ToString());
                                lstMacFreqTotal.Add(rdr["TotalMachineFreq"].ToString());
                            }
                            else if (PrevMachine == rdr[Machinename].ToString() && count != 0)
                            {
                                break;
                            }
                            count++;
                        }
                        #endregion

                        #region "Value Part Bind Data"
                        r = 7;
                        c = 3;
                        foreach (DataRow rdr in dt2.Rows)
                        {
                            if (PrevDownFreq == "" || PrevDownFreq == rdr["DownCode"].ToString())
                            {
                                ws5.Cells[r, c].Value = Convert.ToInt32(rdr["DownFreq"].ToString());
                            }
                            else
                            {
                                r = r + 1;
                                c = 3;
                                ws5.Cells[r, c].Value = Convert.ToInt32(rdr["DownFreq"].ToString());
                            }
                            c = c + 1;
                            ws5.Cells[r, 1].Value = rdr["DownCode"].ToString();
                            ws5.Cells[r, 2].Value = rdr["Owner"].ToString();
                            PrevDownFreq = rdr["DownCode"].ToString();
                        }
                        c = 3;
                        ws5.Cells[r + 2, 1].Value = "Total";

                        foreach (string value in lstMacFreqTotal)
                        {
                            int tolal = 0;
                            if (int.TryParse(value, out tolal))
                                ws5.Cells[r + 2, c].Value = tolal;
                            c = c + 1;

                        }
                        ws5.Cells[4, 1, 7, 26].AutoFitColumns();
                        #endregion

                        #region "Graph Plat Last Sheet"
                        #region "MCs by Top-5 Freq"
                        range = Convert.ToInt32(LimitData) + 6;
                        var chartfreqWise = (ExcelBarChart)ws6.Drawings.AddChart("Down Frequency Comparison Graph", eChartType.ColumnClustered);
                        chartfreqWise.Border.LineStyle = eLineStyle.Solid;
                        chartfreqWise.Title.Text = "Down Frequency Comparison Graph";
                        chartfreqWise.YAxis.Title.Text = "Down Frequency";
                        //chartfreqWise.XAxis.Title.Text = "Down Frequency Comparison Graph";
                        chartfreqWise.SetSize(screenWidth, screenHeight);
                        chartfreqWise.SetPosition(10, 22);
                        for (int i = 2; i < c; i++)
                        {
                            ExcelChartSerie aa = chartfreqWise.Series.Add(ws5.Cells[7, i, range, i], ws5.Cells[7, 1, range, 1]);
                            aa.HeaderAddress = new ExcelAddress("'Freq-wise'!" + GetExcelColumnName(i) + "6");
                            chartDataLabelVerticle270(aa as ExcelBarChartSerie, 45);
                        }
                        #endregion

                        #region "Top 5 Freq by MC"
                        range = Convert.ToInt32(LimitData) + 6;
                        var BarChartForFreq = ws7.Drawings["Chart 1"] as ExcelBarChart;
                        //var series1 = BarChartForFreq.Series[0];
                        BarChartForFreq.Title.Text = "Down Frequency Comparison Graph";
                        BarChartForFreq.YAxis.Title.Text = "Down Frequency";
                        BarChartForFreq.SetSize(screenWidth, screenHeight + 300);
                        var seriss = BarChartForFreq.Series.Add(ws5.Cells[7, 2, 7, c], ws5.Cells[6, 2, 6, c]);
                        seriss.Header = ws5.Cells["A" + 7].Value.ToString();
                        chartDataLabelVerticle270(seriss as ExcelBarChartSerie, 45);
                        BarChartForFreq.SetPosition(10, 22);
                        for (int i = 8; i <= range; i++)
                        {
                            ExcelChartSerie seri = BarChartForFreq.Series.Add(ws5.Cells[i, 2, i, c], ws5.Cells[6, 2, 6, c]);
                            seri.Header = ws5.Cells["A" + i].Value == null ? "" : ws5.Cells["A" + i].Value.ToString();//new ExcelAddressBase("'Time-wise'!$A$7");
                            chartDataLabelVerticle270(seri as ExcelBarChartSerie, 45);
                        }
                        //series1.XSeries = ws5.Cells[6, 2, 6, c].FullAddress;
                        //series1.Series = ws5.Cells[7, 2, 7, c].FullAddress;                    
                        #endregion
                        #endregion
                    }
                    else
                        Generated = "nodatafound";
                    #endregion

                    #region ------ Down Time Pareto ------
                    DataTable dtDownTimePareto = TMPTrakDataBase.MachineDownTimeMatrix(StartDate, EndDate, MachineId, PlantId, DownID, Exclude, "DLoss_By_Catagory", cellID, proc, "");
                    DataView dvDownTimePareto = dtDownTimePareto.DefaultView;
                    dvDownTimePareto.Sort = "DownTime desc";
                    dtDownTimePareto = dvDownTimePareto.ToTable();
                    r = 6; c = 26;
                    int startRow = r, startCol = c;
                    int catRow = r;
                    decimal totalTime = 0;
                    downTimeSheet.Cells[r - 1, c].Value = "Category";
                    downTimeSheet.Cells[r - 1, c + 1].Value = "DownID";
                    downTimeSheet.Cells[r - 1, c + 2].Value = "DownTime";
                    downTimeSheet.Cells[r - 1, c + 3].Value = "DownTimeInSeconds";
                    downTimeSheet.Cells[r - 1, c + 4].Value = "DownTimeFormat";
                    downTimeSheet.Cells[r - 1, c + 5].Value = "Percentage";
                    downTimeSheet.Cells[r - 1, c + 7].Value = "Category";
                    downTimeSheet.Cells[r - 1, c + 8].Value = "DownTimeFormat";
                    int topDownReason = Convert.ToInt32(LimitData);
                    int loopCount = 0;
                    foreach (DataRow row in dtDownTimePareto.Rows)
                    {
                        if (loopCount >= topDownReason)
                        {
                            decimal downTimeMinTotal = 0, downTimeSecTotal = 0;
                            var declistVal = dtDownTimePareto.AsEnumerable().Select(k => new { downTimeMin = k.Field<double>("DownTime"), downTimeSec = k.Field<double>("DownTimeInSeconds") }).Skip(topDownReason).ToList();
                            if (declistVal.Count > 0)
                            {
                                downTimeMinTotal = Convert.ToDecimal(declistVal.Sum(k => k.downTimeMin));
                                downTimeSecTotal = Convert.ToDecimal(declistVal.Sum(k => k.downTimeSec));
                                downTimeSheet.Cells[r, c].Value = "";
                                downTimeSheet.Cells[r, c + 1].Value = "Other";
                                downTimeSheet.Cells[r, c + 2].Value = downTimeMinTotal;
                                downTimeSheet.Cells[r, c + 3].Value = downTimeSecTotal;
                                downTimeSheet.Cells[r, c + 4].Value = downTimeSecTotal / 86400;
                                downTimeSheet.Cells[r, c + 4].Style.Numberformat.Format = "[h]:mm";
                                totalTime += downTimeSecTotal;
                            }
                            loopCount++;
                            break;
                        }
                        else
                        {
                            downTimeSheet.Cells[r, c].Value = row["Catagory"];
                            downTimeSheet.Cells[r, c + 1].Value = row["DownID"];
                            downTimeSheet.Cells[r, c + 2].Value = row["DownTime"];
                            downTimeSheet.Cells[r, c + 3].Value = row["DownTimeInSeconds"];
                            downTimeSheet.Cells[r, c + 4].Value = Convert.ToDecimal(row["DownTimeInSeconds"].ToString()) / 86400;
                            downTimeSheet.Cells[r, c + 4].Style.Numberformat.Format = "[h]:mm";
                            totalTime += Convert.ToDecimal(row["DownTimeInSeconds"].ToString());
                            r++;
                        }
                        loopCount++;
                    }
                    r = startRow; c = startCol;
                    var categoryDownTimeGroup = dtDownTimePareto.AsEnumerable().GroupBy(k => k.Field<string>("Catagory")).Select(k => new
                    {
                        Category = k.First().Field<string>("Catagory"),
                        DownTimeInSec = k.Sum(d => d.Field<double>("DownTimeInSeconds")),
                    }).ToList();
                    foreach (var data in categoryDownTimeGroup)
                    {
                        downTimeSheet.Cells[r, c + 7].Value = data.Category;
                        downTimeSheet.Cells[r, c + 8].Value = Convert.ToDecimal(data.DownTimeInSec) / 86400;
                        downTimeSheet.Cells[r, c + 8].Style.Numberformat.Format = "[h]:mm";
                        r++;
                    }
                    catRow = r - 1;
                    r = startRow;
                    decimal previosValueSum = 0;
                    for (int i = 0; i < loopCount; i++)
                    {

                        decimal value = ((Convert.ToDecimal(downTimeSheet.Cells[r, c + 3].Text) + previosValueSum) / totalTime);
                        downTimeSheet.Cells[r, c + 5].Value = value;
                        downTimeSheet.Cells[r, c + 5].Style.Numberformat.Format = "#0.00%";
                        previosValueSum += Convert.ToDecimal(downTimeSheet.Cells[r, c + 3].Text);
                        r++;
                    }
                    downTimeSheet.Cells[startRow - 1, startCol, r, startCol + 8].Style.Font.Color.SetColor(Color.White);
                    r--;
                    var downTimePieChart = downTimeSheet.Drawings.AddChart("PieChart", eChartType.Pie);
                    downTimePieChart.SetPosition(2, 10, 0, 10);
                    downTimePieChart.SetSize(1800, 600);
                    downTimePieChart.Title.Text = "";
                    var series = downTimePieChart.Series.Add(downTimeSheet.Cells[startRow, startCol + 2, r, startCol + 2], downTimeSheet.Cells[startRow, startCol + 1, r, startCol + 1]);
                    var pieSeries = (ExcelPieChartSerie)series;
                    pieSeries.Explosion = 0;
                    pieSeries.DataLabel.ShowValue = true;
                    pieSeries.DataLabel.ShowCategory = true;
                    pieSeries.DataLabel.ShowPercent = true;
                    pieSeries.DataLabel.ShowLeaderLines = true;
                    pieSeries.DataLabel.Separator = ";  ";
                    pieSeries.DataLabel.Position = eLabelPosition.BestFit;

                    var xdoc = downTimePieChart.ChartXml;
                    var nsuri = xdoc.DocumentElement.NamespaceURI;
                    var nsm = new System.Xml.XmlNamespaceManager(xdoc.NameTable);
                    nsm.AddNamespace("c", nsuri);
                    var numFmtNode = xdoc.CreateElement("c:numFmt", nsuri);

                    var formatCodeAtt = xdoc.CreateAttribute("formatCode", nsuri);
                    formatCodeAtt.Value = "0.0%";
                    numFmtNode.Attributes.Append(formatCodeAtt);

                    var sourceLinkedAtt = xdoc.CreateAttribute("sourceLinked", nsuri);
                    sourceLinkedAtt.Value = "0";
                    numFmtNode.Attributes.Append(sourceLinkedAtt);

                    var dLblsNode = xdoc.SelectSingleNode("c:chartSpace/c:chart/c:plotArea/c:pieChart/c:ser/c:dLbls", nsm);
                    dLblsNode.AppendChild(numFmtNode);


                    var downTimeParetoBarChart = (ExcelBarChart)downTimeSheet.Drawings.AddChart("paretoChart", eChartType.ColumnClustered);
                    downTimeParetoBarChart.SetSize(1800, 600);
                    downTimeParetoBarChart.SetPosition(35, 10, 0, 10);
                    ExcelBarChartSerie downTimeParetoBarSeries = downTimeParetoBarChart.Series.Add(ExcelRange.GetAddress(startRow, startCol + 4, r, startCol + 4), ExcelRange.GetAddress(startRow, startCol + 1, r, startCol + 1));
                    ExcelLineChart downTimeParetoLineChart = (ExcelLineChart)downTimeParetoBarChart.PlotArea.ChartTypes.Add(eChartType.Line);
                    ExcelLineChartSerie downTimeParetoLineSeries = downTimeParetoLineChart.Series.Add(ExcelRange.GetAddress(startRow, startCol + 5, r, startCol + 5), ExcelRange.GetAddress(startRow, startCol + 1, r, startCol + 1)) as ExcelLineChartSerie;
                    downTimeParetoBarSeries.Header = "Down ID";
                    downTimeParetoBarSeries.Fill.Color = Color.FromArgb(140, 185, 226);
                    downTimeParetoLineChart.UseSecondaryAxis = true;
                    downTimeParetoLineSeries.Header = "Pareto";
                    downTimeParetoBarChart.YAxis.Format = "[h]:mm;@";
                    downTimeParetoBarChart.VaryColors = true;
                    downTimeParetoBarChart.YAxis.Title.Text = "Time (hh:mm)";
                    downTimeParetoBarChart.Title.Text = "";
                    downTimeParetoBarChart.Title.Font.Size = 20;
                    chartDataLabelVerticle270(downTimeParetoBarSeries as ExcelBarChartSerie, 0);
                    downTimeParetoLineSeries.DataLabel.ShowValue = true;
                    downTimeParetoLineSeries.DataLabel.Position = eLabelPosition.Top;
                    downTimeParetoLineSeries.Marker.Size = 5;
                    downTimeParetoLineSeries.Marker.Style = eMarkerStyle.Circle;
                    downTimeParetoLineSeries.DataLabel.Font.Size = 8;
                    downTimeParetoBarSeries.DataLabel.Font.Size = 8;
                    downTimeParetoBarChart.Legend.Position = eLegendPosition.Bottom;


                    var downTimeBarChart = (ExcelBarChart)downTimeSheet.Drawings.AddChart("barChart", eChartType.ColumnClustered);
                    downTimeBarChart.SetSize(1800, 600);
                    downTimeBarChart.SetPosition(66, 10, 0, 10);
                    ExcelBarChartSerie downTimeBarSeries = downTimeBarChart.Series.Add(ExcelRange.GetAddress(startRow, startCol + 8, catRow, startCol + 8), ExcelRange.GetAddress(startRow, startCol + 7, catRow, startCol + 7));

                    downTimeBarSeries.Header = "";
                    downTimeBarSeries.Fill.Color = Color.FromArgb(140, 185, 226);
                    downTimeBarChart.YAxis.Format = "[h]:mm;@";
                    downTimeBarChart.VaryColors = true;
                    downTimeBarChart.YAxis.Title.Text = "Time (hh:mm)";
                    downTimeBarChart.XAxis.Title.Text = "Category";
                    downTimeBarChart.Title.Text = "";
                    downTimeBarChart.Title.Font.Size = 20;
                    chartDataLabelVerticle270(downTimeBarSeries as ExcelBarChartSerie, 0);
                    downTimeBarSeries.DataLabel.Font.Size = 8;

                    #endregion

                    if (ConfigurationManager.AppSettings["ShowOwner"].ToString().Equals("0", StringComparison.OrdinalIgnoreCase))
                    {
                        ws.Column(2).Hidden = true;
                        ws2.Column(2).Hidden = true;
                        ws5.Column(2).Hidden = true;
                    }

                    foreach (ExcelWorksheet sheet in excelPackage.Workbook.Worksheets)
                    {
                        sheet.Name = sheet.Name.Replace("5", LimitData);
                    }
                    DownloadMultipleFile(dst, excelPackage.GetAsByteArray());
                    Logger.WriteDebugLog("Down time report to Machine wise down time details report generated sucessfully.");
                    Generated = "Generated";
                }
                else
                {
                    FileInfo newFile = new FileInfo(src);
                    ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                    ExcelWorksheet ws = excelPackage.Workbook.Worksheets[0];
                    ExcelWorksheet ws2 = excelPackage.Workbook.Worksheets[1];
                    ExcelWorksheet ws3 = excelPackage.Workbook.Worksheets[2];

                    //foreach (ExcelWorksheet sheet in excelPackage.Workbook.Worksheets)
                    //{
                    //    sheet.Name = sheet.Name.Replace("5", LimitData);
                    //}
                    ws.Cells["C2"].Value = "Start Time";
                    ws.Cells["D2"].Value = strtTime;
                    ws.Cells["F2"].Value = "End Time";
                    ws.Cells["G2"].Value = endTime;
                    ws.Cells["I2"].Value = "Plants";
                    ws.Cells["J2"].Value = PlantId == "" ? "All" : PlantId;
                    ws.Cells[4, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[4, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    int r = 7, count = 0, c = 3;
                    List<string> lstMachineNames = new List<string>();
                    List<string> lstMacTotal = new List<string>();
                    List<string> lstMacFreqTotal = new List<string>();
                    string mxkSectohhmmss = string.Empty;

                    string PrevDownFreq = string.Empty;
                    string DownIDTotal = string.Empty;
                    string downtime = string.Empty;
                    string PrevMachine = string.Empty;
                    int range = Convert.ToInt32(LimitData) + 6;
                    string Prevdown = string.Empty; string Machinename = "";

                    if (ConfigurationManager.AppSettings["sonapages"].Equals("1"))
                    {
                        Machinename = "machineDescription";
                    }
                    else
                        Machinename = "MachineID";

                    DataTable dt = TMPTrakDataBase.MachineDownTimeMatrix(StartDate, EndDate, MachineId, PlantId, DownID, Exclude, "DTime", cellID, proc, "");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        #region "Frist Sheet Data Plat"
                        #region "Header Part Machine "
                        foreach (DataRow rdr in dt.Rows)
                        {
                            if (count == 0)
                            {
                                Prevdown = rdr["DownCode"].ToString();
                                PrevDownFreq = rdr["DownCode"].ToString();
                                PrevMachine = rdr[Machinename].ToString();
                                lstMachineNames.Add(rdr["MachineID"].ToString());
                                if (ConfigurationManager.AppSettings["sonapages"].Equals("1", StringComparison.OrdinalIgnoreCase))
                                {
                                    ws.Cells[5, c].Value = rdr["MachineID"].ToString();
                                    ws.Cells[5, c, 5, c + 1].Merge = true;
                                }
                                //ws.Cells[6, c].Value = "Total Available Time";
                                //ws.Cells[7, c].Value = "Hrs";
                                //c++;
                                //ws.Cells[6, c].Value = "Utilized Time";
                                //ws.Cells[7, c].Value = "Hrs";
                                //c++;
                                ws.Cells[5, c].Value = rdr["machineDescription"].ToString();
                                ws.Cells[5, c, 5, c + 1].Merge = true;
                                ws.Cells[6, c].Value = PrevMachine;
                                ws.Cells[6, c, 6, c + 1].Merge = true;
                                ws.Cells[7, c].Value = "Down Time";
                                ws.Cells[7, c + 1].Value = "Frequency";
                                lstMacTotal.Add(rdr["TotalMachine"].ToString());
                                lstMacFreqTotal.Add(rdr["TotalMachineFreq"].ToString());
                            }
                            if (PrevMachine != rdr[Machinename].ToString())
                            {
                                c = c + 2;
                                if (ConfigurationManager.AppSettings["sonapages"].Equals("1", StringComparison.OrdinalIgnoreCase))
                                {
                                    ws.Cells[5, c].Value = rdr["MachineID"].ToString();
                                    ws.Cells[5, c, 5, c + 1].Merge = true;
                                }
                                ws.Cells[5, c].Value = rdr["machineDescription"].ToString();
                                ws.Cells[5, c, 5, c + 1].Merge = true;
                                ws.Cells[6, c].Value = rdr[Machinename].ToString();
                                ws.Cells[6, c, 6, c + 1].Merge = true;
                                ws.Cells[7, c].Value = "Down Time";
                                ws.Cells[7, c + 1].Value = "Frequency";
                                lstMachineNames.Add(rdr["MachineID"].ToString());
                                lstMacTotal.Add(rdr["TotalMachine"].ToString());
                                lstMacFreqTotal.Add(rdr["TotalMachineFreq"].ToString());
                            }
                            else if (PrevMachine == rdr[Machinename].ToString() && count != 0)
                            {
                                break;
                            }
                            count++;
                        }
                        #endregion
                        #region "Machine Value Define Frist Sheet"
                        r = 8;
                        c = 3;
                        Prevdown = "";
                        TimeSpan timeSpan = TimeSpan.MinValue;
                        foreach (DataRow rdr in dt.Rows)
                        {
                            if (Prevdown == "" || Prevdown == rdr["DownCode"].ToString())
                            {
                                //ws.Cells[r, 1].Value = rdr["DownCode"].ToString();
                                ws.Cells[r, c].Value = Convert.ToDecimal(rdr["DownTime"].ToString()) / 86400;
                                ws.Column(c + 1).Style.Numberformat.Format = "0";
                                ws.Cells[r, c + 1].Value = Convert.ToDecimal(rdr["DownFreq"].ToString());
                                //ws.Cells[r, c + 1].Style.Numberformat.Format = "Number";
                            }
                            else if (Prevdown != rdr["DownCode"].ToString())
                            {
                                r = r + 1;
                                c = 3;
                                ws.Cells[r, c].Value = Convert.ToDecimal(rdr["DownTime"].ToString()) / 86400;
                                ws.Column(c + 1).Style.Numberformat.Format = "0";
                                ws.Cells[r, c + 1].Value = Convert.ToDecimal(rdr["DownFreq"].ToString());
                            }
                            c = c + 2;
                            ws.Cells[r, 1].Value = rdr["DownCode"].ToString();
                            ws.Cells[r, 2].Value = rdr["Owner"].ToString();
                            Prevdown = rdr["DownCode"].ToString();
                        }
                        c = 3;
                        ws.Cells[r + 2, 1].Value = "Total";
                        foreach (string value in lstMacTotal)
                        {
                            if (Convert.ToInt32(value) > 0)
                            {
                                timeSpan = TimeSpan.FromSeconds(Convert.ToDouble(value));
                                string answer = string.Format("{0:00}:{1:00}:{2:00}",
                                         (int)timeSpan.TotalHours,
                                         timeSpan.Minutes,
                                         timeSpan.Seconds);
                                //ws.Cells[r + 2, c].Value = Convert.ToDecimal(value) / 86400;
                                ws.Cells[r + 2, c].Style.Numberformat.Format = "[h]:mm:ss";
                                ws.Cells[r + 2, c].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                ws.Cells[r + 2, c].Value = timeSpan;
                            }
                            c = c + 2;

                        }
                        c = 4;
                        foreach (string values in lstMacFreqTotal)
                        {
                            //-------Start --------------
                            int total = 0;
                            if (int.TryParse(values, out total))
                                ws.Cells[r + 2, c].Value = total;
                            //------END---------------------
                            c = c + 2;
                        }
                        #endregion
                        #region Charts 

                        range = Convert.ToInt32(LimitData) + 7;
                        var chart11 = ws2.Drawings["Chart 1"] as ExcelBarChart;
                        chart11.Border.LineStyle = eLineStyle.Solid;
                        chart11.YAxis.Format = "[h]:mm:ss;@";
                        chart11.SetSize(screenWidth, screenHeight);
                        chart11.SetPosition(10, 22);
                        // chart11.DataLabel.ShowValue = true;
                        chart11.Title.Text = "Down Time Comparison Graph";
                        chart11.YAxis.Title.Text = "Down Time";
                        for (int i = 4; i < c + 3; i = i + 2)
                        {

                            ExcelChartSerie aa = chart11.Series.Add(ws.Cells[8, i, range, i], ws.Cells[8, 1, range, 1]);
                            aa.HeaderAddress = new ExcelAddress("'Time-wise'!" + GetExcelColumnName(i) + "6");
                            chartDataLabelVerticle270(aa as ExcelBarChartSerie, 0);
                        }
                        var barchart = ws2.Drawings["Chart 2"] as ExcelBarChart;
                        barchart.Border.LineStyle = eLineStyle.Solid;
                        // var series = barchart.Series[0];
                        barchart.Title.Text = "Frequency Comparison Graph";
                        barchart.YAxis.Title.Text = "Frequency";
                        barchart.YAxis.Format = "00";
                        barchart.SetSize(screenWidth, screenHeight + 300);
                        barchart.SetPosition(800, 22);
                        //barchart.DataLabel.ShowValue = true;
                        //barchart.DataLabel.TextBody.VerticalText = eTextVerticalType.Vertical270;
                        for (int i = 5; i < c + 3; i = i + 2)
                        {

                            ExcelChartSerie aa = barchart.Series.Add(ws.Cells[8, i, range, i], ws.Cells[8, 1, range, 1]);
                            aa.HeaderAddress = new ExcelAddress("'Time-wise'!" + GetExcelColumnName(i - 1) + "6");
                            chartDataLabelVerticle270(aa as ExcelBarChartSerie, 45);
                        }
                        //chart11.UseSecondaryAxis = true;
                        #endregion
                        #endregion
                        ws.InsertColumn(3, 2);


                        var dtnew = dt.AsEnumerable().Select(x => new { DowCode = x.Field<string>("DownCode"), DownTime = x.Field<double>("TotalDown"), DownFreq = x.Field<double>("TotalDownFreq") }).Distinct().ToList();
                        c = 3;
                        ws.Cells[4, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[4, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[4, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[6, (c)].Value = "Total";
                        ws.Cells[6, c, 6, c + 1].Merge = true;
                        ws.Cells[7, c].Value = "DownTime";
                        ws.Cells[7, c + 1].Value = "Frequency";
                        r = 8;
                        ws.Cells[6, 3, 7, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[6, 3, 7, 4].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#CFB296"));
                        ws.Cells[6, 3, 7, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[6, 3, 7, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[6, 3, 7, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[6, 3, 7, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[6, 3, 7, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[6, 3, 7, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[6, 3, 7, 4].Style.Border.Bottom.Color.SetColor(Color.Black);
                        ws.Cells[6, 3, 7, 4].Style.Border.Top.Color.SetColor(Color.Black);
                        ws.Cells[6, 3, 7, 4].Style.Border.Left.Color.SetColor(Color.Black);
                        ws.Cells[6, 3, 7, 4].Style.Border.Right.Color.SetColor(Color.Black);
                        ws.Cells[4, 1, 7, 26].AutoFitColumns();
                        ws.Column(3).Width = 20; ws.Column(4).Width = 20;
                        ws.Column(c + 1).Style.Numberformat.Format = "0";
                        ws.Column(c).Style.Numberformat.Format = "General";

                        for (int i = 0; i < dtnew.Count; i++)
                        {
                            timeSpan = TimeSpan.FromSeconds(Convert.ToDouble(dtnew[i].DownTime.ToString()));
                            string answer = string.Format("{0:00}:{1:00}:{2:00}",
                                     (int)timeSpan.TotalHours,
                                     timeSpan.Minutes,
                                     timeSpan.Seconds);
                            ws.Cells[r, c].Value = Convert.ToDouble(dtnew[i].DownTime.ToString()) / 86400;
                            ws.Cells[r, c].Style.Numberformat.Format = "[h]:mm:ss";
                            ws.Cells[r++, (c + 1)].Value = dtnew[i].DownFreq;
                        }

                        ws.Cells[7, 2, 7, 3].AutoFitColumns();

                        //Add Total Availability and Utilized Time
                        c = 3;
                        ws.InsertColumn(3, 2);
                        ws.Cells[6, c].Value = "Total Available Time";
                        ws.Cells[6, c + 1].Value = "Utilized Time";
                        ws.Cells[6, c, 6, c + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[7, c].Value = "Hrs";
                        ws.Cells[7, c + 1].Value = "Hrs";
                        ws.Cells[7, c, 7, c + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[8, c].Value = totalAvailableTime;
                        ws.Cells[8, c].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[8, c].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[8, c, dtnew.Count + 7, c].Merge = true;
                        ws.Cells[8, c + 1].Value = totalUtilizedTime;
                        ws.Cells[8, c + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[8, c + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[8, c + 1, dtnew.Count + 7, c + 1].Merge = true;
                        ws.Cells[6, c, 7, c + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[6, c, 7, c + 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#CFB296"));
                        ws.Cells[6, c, 7, c + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[6, c, 7, c + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[6, c, 7, c + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[6, c, 7, c + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[6, c, 7, c + 1].Style.Border.Bottom.Color.SetColor(Color.Black);
                        ws.Cells[6, c, 7, c + 1].Style.Border.Top.Color.SetColor(Color.Black);
                        ws.Cells[6, c, 7, c + 1].Style.Border.Left.Color.SetColor(Color.Black);
                        ws.Cells[6, c, 7, c + 1].Style.Border.Right.Color.SetColor(Color.Black);
                        ws.Cells[7, c, 7, c + 1].AutoFitColumns();
                        if (ConfigurationManager.AppSettings["ShantiIronPages"].ToString() != "1")
                        {
                            ws.Column(3).Hidden = true;
                            ws.Column(4).Hidden = true;
                        }
                        ws.Cells["E5"].Value = "Mc Description";
                        ws.Cells["E5"].Style.Font.Bold = true;
                        ws.Cells["E5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["E5"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells["E5"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffcc99"));
                        ws.Cells[5, 5, 5, 6].Merge = true;

                        #region ----- Third Sheet (Pareto Chart) ------
                        var distMachineid = dt.AsEnumerable().Select(k => k.Field<string>("MachineID")).Distinct().ToList();


                        DataTable dtTotal = new DataTable();
                        dtTotal.Columns.Add("DownCode", typeof(string));
                        dtTotal.Columns.Add("DownTime", typeof(double));
                        for (int i = 0; i < dtnew.Count; i++)
                        {
                            var dTrow = dtTotal.NewRow();
                            dTrow["DownCode"] = dtnew[i].DowCode;
                            dTrow["DownTime"] = Convert.ToDouble(dtnew[i].DownTime);
                            dtTotal.Rows.Add(dTrow);
                        }

                        //var totalDt = dt.AsEnumerable()
                        //              .GroupBy(k => k.Field<string>("DownCode").ToUpper())
                        //              .Select(g =>
                        //              {
                        //                  var dTrow = dt.NewRow();
                        //                  dTrow["DownCode"] = g.Key.ToUpper();
                        //                  dTrow["DownTime"] = g.Sum(k => k.Field<double>("TotalDown"));
                        //                  return dTrow;
                        //              }).CopyToDataTable();

                        r = 2; c = 29;
                        int startRow = r;
                        int chartPositionRow = r;
                        distMachineid.Insert(0, "Total");
                        foreach (string machine in distMachineid)
                        {
                            DataTable machineDt = new DataTable();
                            if (machine.Equals("Total", StringComparison.OrdinalIgnoreCase))
                            {
                                machineDt = dtTotal.AsEnumerable().OrderByDescending(k => k.Field<double>("DownTime")).CopyToDataTable();
                            }
                            else
                            {
                                machineDt = dt.AsEnumerable().Where(k => k.Field<string>("MachineID") == machine).OrderByDescending(k => k.Field<double>("DownTime")).CopyToDataTable();
                            }
                            decimal totalTime = 0;
                            ws3.Cells[startRow - 1, c].Value = "Down Code";
                            ws3.Cells[startRow - 1, c + 1].Value = "Down Time (hh:mm:ss)";
                            ws3.Cells[startRow - 1, c + 2].Value = "Down Time (Sec)";
                            ws3.Cells[startRow - 1, c + 3].Value = "Percentage";
                            r = startRow;
                            foreach (DataRow row in machineDt.Rows)
                            {
                                ws3.Cells[r, c].Value = row["DownCode"].ToString();
                                ws3.Cells[r, c + 1].Value = Convert.ToDecimal(row["DownTime"].ToString()) / 86400;
                                ws3.Cells[r, c + 1].Style.Numberformat.Format = "[h]:mm:ss";
                                ws3.Cells[r, c + 2].Value = Convert.ToDecimal(row["DownTime"].ToString());
                                totalTime += Convert.ToDecimal(row["DownTime"].ToString());
                                r++;
                            }
                            r = startRow;
                            decimal previosValueSum = 0;
                            foreach (DataRow row in machineDt.Rows)
                            {

                                decimal value = ((Convert.ToDecimal(ws3.Cells[r, c + 2].Text) + previosValueSum) / totalTime);
                                ws3.Cells[r, c + 3].Value = value;
                                ws3.Cells[r, c + 3].Style.Numberformat.Format = "#0.00%";
                                previosValueSum += Convert.ToDecimal(ws3.Cells[r, c + 2].Text);
                                r++;
                            }
                            ws3.Cells[startRow - 1, c, r, c + 3].Style.Font.Color.SetColor(Color.White);

                            r--;
                            var chart = (ExcelBarChart)ws3.Drawings.AddChart("paretoChart" + machine, eChartType.ColumnClustered);
                            chart.SetSize(1800, 600);
                            chart.SetPosition(chartPositionRow - 1, 10, 0, 10);
                            ExcelBarChartSerie ch1 = chart.Series.Add(ExcelRange.GetAddress(startRow, c + 1, r, c + 1), ExcelRange.GetAddress(startRow, c, r, c));
                            ExcelLineChart chart2 = (ExcelLineChart)chart.PlotArea.ChartTypes.Add(eChartType.Line);
                            ExcelLineChartSerie ch2 = chart2.Series.Add(ExcelRange.GetAddress(startRow, c + 3, r, c + 3), ExcelRange.GetAddress(startRow, c, r, c)) as ExcelLineChartSerie;
                            ch1.Header = "Down Time";
                            ch1.Fill.Color = Color.FromArgb(140, 185, 226);
                            chart2.UseSecondaryAxis = true;
                            ch2.Header = "Pareto";
                            chart.YAxis.Format = "[h]:mm:ss;@";
                            chart.VaryColors = true;
                            chart.YAxis.Title.Text = "Down Time";
                            chart.Title.Text = machine;
                            chart.Title.Font.Size = 20;
                            chartDataLabelVerticle270(ch1 as ExcelBarChartSerie, 0);
                            ch2.DataLabel.ShowValue = true;
                            ch2.DataLabel.Position = eLabelPosition.Top;
                            ch2.Marker.Size = 5;
                            ch2.Marker.Style = eMarkerStyle.Circle;
                            ch2.DataLabel.Font.Size = 8;
                            ch1.DataLabel.Font.Size = 8;
                            //startRow += 30;
                            //r = startRow;
                            chartPositionRow += 32;
                            c = c + 5;
                        }
                        #endregion

                        if (ConfigurationManager.AppSettings["ShowOwner"].ToString().Equals("0", StringComparison.OrdinalIgnoreCase))
                        {
                            ws.Column(2).Hidden = true;
                        }

                        DownloadMultipleFile(dst, excelPackage.GetAsByteArray());
                        Logger.WriteDebugLog("Down time report to Machine wise down time details report generated sucessfully.");
                        Generated = "Generated";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteDebugLog(ex.Message);
            }
            return Generated;
        }
        public static void chartDataLabelVerticle270(ExcelBarChartSerie chartSerie, int rotation)
        {
            try
            {
                chartSerie.DataLabel.ShowValue = true;
                if (ConfigurationManager.AppSettings["ExcelChartDataLabelOrientation"].ToString() == "1")
                {
                    chartSerie.DataLabel.TextBody.VerticalText = eTextVerticalType.Vertical270;
                    //chartSerie.DataLabel.TextBody.Rotation = rotation;
                }
                chartSerie.DataLabel.Position = eLabelPosition.OutEnd;
                chartSerie.DataLabel.Font.Size = 11;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }


        internal static string ProductionandDownTimeReport_MachinewiseGraph(DateTime fromDate, string PlantID, string machineId, string shiftID, DateTime toDate, string cellID)
        {
            string Generated = "";
            try
            {
                string src, dst = string.Empty;
                string reportName = "MachinewiseProdReport_Graphs.xlsx";
                src = Util.GetReportPath(reportName);
                string tempfileName = "MachineDownTimeReport" + "_" + Guid.NewGuid() + ".xlsx";
                dst = Path.Combine(appPath, "Temp", SafeFileName(tempfileName));
                if (PlantID.Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    PlantID = "";
                if (machineId.Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    machineId = "";
                if (shiftID.Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    shiftID = "";
                if (cellID.Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    cellID = "";
                if (!File.Exists(src))
                {
                    Logger.WriteDebugLog("Machine Production and DownTimeReport Report Template Not Found at the Path - \n " + src);
                }
                FileInfo newFile = new FileInfo(src);
                ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                ExcelWorksheet ws = excelPackage.Workbook.Worksheets[0];
                ExcelWorksheet ws2 = excelPackage.Workbook.Worksheets[1];
                ExcelWorksheet ws3 = excelPackage.Workbook.Worksheets[2];

                ws.Cells["B2"].Value = fromDate.ToString("dd-MMM-yyyy HH:mm:ss");
                ws.Cells["F2"].Value = toDate.ToString("dd-MMM-yyyy HH:mm:ss");

                ws2.Cells["B2"].Value = fromDate.ToString("dd-MMM-yyyy HH:mm:ss");
                ws2.Cells["F2"].Value = toDate.ToString("dd-MMM-yyyy HH:mm:ss");

                ws3.Cells["B2"].Value = fromDate.ToString("dd-MMM-yyyy HH:mm:ss");
                ws3.Cells["F2"].Value = toDate.ToString("dd-MMM-yyyy HH:mm:ss");

                string timeFormat = DataBaseAccess.getTimeFormatFromCockpit();
                DataTable dt = TMPTrakDataBase.SM_MachinewiseProdReportFromAutoData(fromDate, toDate, machineId, PlantID, cellID);
                int colPos = 0, columnNo = 1, rowPos = 3;
                bool isFirstChart = true;

                #region     -----First Sheet-------
                if (dt.Rows.Count > 0 && dt != null)
                {
                    var distCellID = dt.AsEnumerable().Where(k => k.Field<string>("Groupid") != null).Select(k => k.Field<string>("Groupid")).Distinct().ToList();
                    DataTable Groupdt = new DataTable();

                    int j = 5, l = 30;
                    int mainrow = j;
                    int rowval = j;

                    int Cellcnt = distCellID.Count();

                    foreach (string Cell in distCellID)
                    {
                        Groupdt = dt.AsEnumerable().Where(x => x.Field<string>("Groupid") != null && x.Field<string>("Groupid").Equals(Cell)).CopyToDataTable();
                        foreach (DataRow items in Groupdt.Rows)
                        {
                            ws.Cells[j, l].Value = items["MachineID"].ToString();
                            ws.Cells[j, l + 1].Value = Convert.ToDecimal(items["AvailabilityEfficiency"].ToString());
                            ws.Cells[j, l + 1].Style.Numberformat.Format = "0.00";
                            ws.Cells[j, l + 2].Value = Convert.ToDecimal(items["ProductionEfficiency"].ToString());
                            ws.Cells[j, l + 2].Style.Numberformat.Format = "0.00";
                            ws.Cells[j, l + 3].Value = Convert.ToDecimal(items["QualityEfficiency"].ToString());
                            ws.Cells[j, l + 3].Style.Numberformat.Format = "0.00";
                            ws.Cells[j, l + 4].Value = Convert.ToDecimal(items["OverAllEfficiency"].ToString());
                            ws.Cells[j, l + 4].Style.Numberformat.Format = "0.00";
                            j++;
                        }
                        ws.Cells[rowval, l, j - 1, l + 4].Style.Font.Color.SetColor(Color.White);

                        j--;

                        int chartWidth = 1950, chartHeight = 550;
                        if (columnNo != Cellcnt + 1)
                        {
                            if (isFirstChart)
                            {
                                if (Cellcnt == 1)
                                {
                                    chartWidth = 1950; chartHeight = 550;
                                }
                                else
                                {
                                    chartWidth = 1950; chartHeight = 550;
                                    isFirstChart = false;
                                    columnNo++;
                                }
                            }
                            else
                            {
                                if (columnNo % 2 == 0)
                                {
                                    rowPos = rowPos + 28;
                                    colPos = 0;
                                    columnNo++;
                                    chartWidth = 1950; chartHeight = 550;
                                    //colPos = colPos + 14;
                                    //columnNo++;
                                }
                                else if (columnNo % 2 != 0 && columnNo != Cellcnt)
                                {
                                    rowPos = rowPos + 28;
                                    colPos = 0;
                                    columnNo++;
                                }
                                else
                                {
                                    rowPos = rowPos + 28;
                                    colPos = 0;
                                    columnNo++;
                                    chartWidth = 1950; chartHeight = 550;
                                }
                            }
                        }

                        var chart = (ExcelBarChart)ws.Drawings.AddChart("BarChart" + Cell, eChartType.ColumnClustered);
                        chart.SetSize(chartWidth, chartHeight);
                        chart.SetPosition(rowPos, 10, colPos, 10);
                        ExcelChartSerie ch1 = chart.Series.Add(ExcelRange.GetAddress(rowval, l + 1, j, l + 1), ExcelRange.GetAddress(rowval, l, j, l));
                        ExcelChartSerie ch2 = chart.Series.Add(ExcelRange.GetAddress(rowval, l + 2, j, l + 2), ExcelRange.GetAddress(rowval, l, j, l));
                        ExcelChartSerie ch3 = chart.Series.Add(ExcelRange.GetAddress(rowval, l + 3, j, l + 3), ExcelRange.GetAddress(rowval, l, j, l));
                        ExcelChartSerie ch4 = chart.Series.Add(ExcelRange.GetAddress(rowval, l + 4, j, l + 4), ExcelRange.GetAddress(rowval, l, j, l));
                        ch1.Header = "%AE";
                        ch2.Header = "%PE";
                        ch3.Header = "%QE";
                        ch4.Header = "%OEE";

                        //Format the legend
                        chart.Legend.Position = eLegendPosition.Top;
                        chart.Legend.Font.Bold = true;
                        chart.Legend.Font.Size = 14F;

                        chart.XAxis.Font.Size = 12F;
                        chart.XAxis.Font.Bold = true;
                        chart.YAxis.Font.Size = 12F;
                        chart.YAxis.Font.Bold = true;

                        chart.YAxis.Format = "";
                        chart.VaryColors = true;
                        chart.Title.Text = Cell + " CELL";
                        chart.Title.Font.Size = 18F;
                        chart.DataLabel.ShowValue = true;
                        chartDataLabelVerticle270(ch1 as ExcelBarChartSerie, -40);
                        chartDataLabelVerticle270(ch2 as ExcelBarChartSerie, -40);
                        chartDataLabelVerticle270(ch3 as ExcelBarChartSerie, -40);
                        chartDataLabelVerticle270(ch4 as ExcelBarChartSerie, -40);

                        mainrow += 30;
                        rowval = j + 1;
                        j = j + 1;

                    }

                }
                #endregion



                #region ----- Second Sheet (Pareto Chart) ------
                DataTable Machdowndt = TMPTrakDataBase.MachineDownTimeMatrix(fromDate, toDate, machineId, PlantID, "", 0, "DTime", cellID, "s_GetDownTimeMatrixfromAutoData", "");

                if (Machdowndt.Rows.Count > 0 && Machdowndt != null)
                {

                    var distMachineid = Machdowndt.AsEnumerable().Select(k => k.Field<string>("MachineID")).Distinct().ToList();
                    var dtnew = Machdowndt.AsEnumerable().Select(x => new { DowCode = x.Field<string>("DownCode"), DownTime = x.Field<double>("TotalDown"), DownFreq = x.Field<double>("TotalDownFreq") }).Distinct().ToList();
                    DataTable dtTotal = new DataTable();
                    dtTotal.Columns.Add("DownCode", typeof(string));
                    dtTotal.Columns.Add("DownTime", typeof(double));
                    for (int i = 0; i < dtnew.Count; i++)
                    {
                        var dTrow = dtTotal.NewRow();
                        dTrow["DownCode"] = dtnew[i].DowCode;
                        dTrow["DownTime"] = Convert.ToDouble(dtnew[i].DownTime);
                        dtTotal.Rows.Add(dTrow);
                    }

                    colPos = 0; columnNo = 1; rowPos = 3;
                    isFirstChart = true;
                    int Machcnt = distMachineid.Count() + 1;

                    int r = 5, c = 29;
                    int startRow = r;
                    distMachineid.Insert(0, "Total");
                    foreach (string machine in distMachineid)
                    {
                        DataTable machineDt = new DataTable();
                        if (machine.Equals("Total", StringComparison.OrdinalIgnoreCase))
                        {
                            if (dtTotal.Rows.Count > 0)
                            {
                                machineDt = dtTotal.AsEnumerable().OrderByDescending(k => k.Field<double>("DownTime")).CopyToDataTable();
                            }
                        }
                        else
                        {
                            if (Machdowndt.Rows.Count > 0)
                            {
                                machineDt = Machdowndt.AsEnumerable().Where(k => k.Field<string>("MachineID") == machine).OrderByDescending(k => k.Field<double>("DownTime")).CopyToDataTable();
                            }
                        }
                        decimal totalTime = 0;
                        ws2.Cells[r - 1, c].Value = "Down Code";
                        ws2.Cells[r - 1, c + 1].Value = "Down Time (hh:mm:ss)";
                        ws2.Cells[r - 1, c + 2].Value = "Down Time (Sec)";
                        ws2.Cells[r - 1, c + 3].Value = "Percentage";
                        foreach (DataRow item in machineDt.Rows)
                        {
                            ws2.Cells[r, c].Value = item["DownCode"].ToString();
                            ws2.Cells[r, c + 1].Value = Convert.ToDecimal(item["DownTime"].ToString()) / 86400;
                            ws2.Cells[r, c + 1].Style.Numberformat.Format = "[h]:mm:ss";
                            ws2.Cells[r, c + 2].Value = Convert.ToDecimal(item["DownTime"].ToString());
                            totalTime += Convert.ToDecimal(item["DownTime"].ToString());
                            r++;
                        }
                        r = startRow;
                        decimal previosValueSum = 0;
                        foreach (DataRow item in machineDt.Rows)
                        {

                            decimal value = ((Convert.ToDecimal(ws2.Cells[r, c + 2].Text) + previosValueSum) / totalTime);
                            ws2.Cells[r, c + 3].Value = value;
                            ws2.Cells[r, c + 3].Style.Numberformat.Format = "#0%";
                            previosValueSum += Convert.ToDecimal(ws2.Cells[r, c + 2].Text);
                            r++;
                        }
                        ws2.Cells[startRow - 1, c, r, c + 3].Style.Font.Color.SetColor(Color.White);

                        r--;

                        int chartWidth = 940, chartHeight = 550;

                        if (columnNo != Machcnt + 1)
                        {
                            if (isFirstChart)
                            {
                                if (Machcnt == 1)
                                {
                                    chartWidth = 940; chartHeight = 550;
                                }
                                else
                                {
                                    chartWidth = 940; chartHeight = 550;
                                    isFirstChart = false;
                                    columnNo++;
                                }
                            }
                            else
                            {
                                if (columnNo % 2 == 0)
                                {
                                    colPos = colPos + 14;
                                    columnNo++;
                                }
                                else if (columnNo % 2 != 0 && columnNo != Machcnt)
                                {
                                    rowPos = rowPos + 28;
                                    colPos = 0;
                                    columnNo++;
                                }
                                else
                                {
                                    rowPos = rowPos + 28;
                                    colPos = 0;
                                    chartWidth = 940; chartHeight = 550;
                                    columnNo++;
                                }
                            }
                        }


                        var chart = (ExcelBarChart)ws2.Drawings.AddChart("paretoChart" + machine, eChartType.ColumnClustered);
                        chart.SetSize(chartWidth, chartHeight);
                        chart.SetPosition(rowPos, 10, colPos, 10);
                        ExcelChartSerie ch1 = chart.Series.Add(ExcelRange.GetAddress(startRow, c + 1, r, c + 1), ExcelRange.GetAddress(startRow, c, r, c));
                        ExcelLineChart chart2 = (ExcelLineChart)chart.PlotArea.ChartTypes.Add(eChartType.Line);
                        ExcelLineChartSerie ch2 = chart2.Series.Add(ExcelRange.GetAddress(startRow, c + 3, r, c + 3), ExcelRange.GetAddress(startRow, c, r, c)) as ExcelLineChartSerie;
                        ch1.Header = "Down Time";
                        chart2.UseSecondaryAxis = true;
                        ch2.Header = "Pareto";
                        ch2.DataLabel.Position = eLabelPosition.Top;
                        ch2.DataLabel.ShowValue = true;
                        ch2.Marker.Style = eMarkerStyle.Circle;
                        ch2.Marker.Fill.Color = Color.FromArgb(237, 125, 49);
                        ch2.Marker.Size = 5;
                        ch2.DataLabel.Fill.Color = Color.FromArgb(255, 192, 0);
                        chart.YAxis.Format = "[h]:mm:ss;@";
                        chart.XAxis.Font.Size = 12F;
                        chart.XAxis.Font.Bold = true;
                        chart.XAxis.TextBody.Rotation = -40;
                        chart.YAxis.Font.Size = 12F;
                        chart.YAxis.Font.Bold = true;
                        chart.VaryColors = true;
                        chart.YAxis.Title.Text = "Down Time (hh:mm:ss)";
                        chart.YAxis.Title.Font.Size = 14F;
                        chart.Title.Text = machine;
                        chart.Title.Font.Size = 18F;
                        chart.DataLabel.ShowValue = true;
                        chart2.DataLabel.ShowValue = true;

                        chart.Legend.Font.Size = 14F;
                        chart.Legend.Font.Bold = true;

                        chartDataLabelVerticle270(ch1 as ExcelBarChartSerie, -40);
                        chart.YAxis.Title.TextVertical = eTextVerticalType.Vertical270;
                        startRow += 30;
                        r = startRow;
                    }
                }
                #endregion

                #region "Third Sheet"
                int row = 6;

                if (dt.Rows.Count > 0 && dt != null)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        ws3.Cells[row, 1].Value = item["MachineID"];
                        ws3.Cells[row, 2].Value = item["MachineDescription"];
                        ws3.Cells[row, 3].Value = item["Efficiency"];
                        setTimeSpanFormat(timeFormat, ws3, row, 4, item["NetUtilisedTime"].ToString());
                        setTimeSpanFormat(timeFormat, ws3, row, 5, item["DownTime"].ToString());
                        setTimeSpanFormat(timeFormat, ws3, row, 6, item["NetManagementLoss"].ToString());
                        setTimeSpanFormat(timeFormat, ws3, row, 7, item["TotalPDT"].ToString());
                        if (!(item["AvailabilityEfficiency"] is DBNull))
                            ws3.Cells[row, 8].Value = Math.Round(Convert.ToDecimal(item["AvailabilityEfficiency"]), 2);
                        if (!(item["ProductionEfficiency"] is DBNull))
                            ws3.Cells[row, 9].Value = Math.Round(Convert.ToDecimal(item["ProductionEfficiency"]), 2);
                        if (!(item["QualityEfficiency"] is DBNull))
                            ws3.Cells[row, 10].Value = Math.Round(Convert.ToDecimal(item["QualityEfficiency"]), 2);
                        if (!(item["OverAllEfficiency"] is DBNull))
                            ws3.Cells[row, 11].Value = Math.Round(Convert.ToDecimal(item["OverAllEfficiency"]), 2);
                        row++;
                    }

                    string modelRange = "A6:K" + (row - 1).ToString();
                    var modelTable = ws3.Cells[modelRange];
                    // Assign borders
                    modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    //if (DataBaseAccess.isQERequired() == false)
                    //{
                    //    ws3.Column(10).Hidden = true;
                    //}

                }
                #endregion

                DownloadMultipleFile(dst, excelPackage.GetAsByteArray());
                Logger.WriteDebugLog("Down time report to Machine wise down time details report generated sucessfully.");
                Generated = "Generated";
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return Generated;
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
                else if (timeFormat.Equals("hh:mm"))
                {
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
        public static string MachineDownTimeMatrixFormat3(DateTime StartDate, string PlantId, string MachineId, DateTime EndDate, string DownID, int Exclude, string width, string height, string LimitData, string cellID, string type, string format, string operatorID)
        {
            string Generated = "";
            try
            {
                string proc = "", reportName = "", strtTime = "", endTime = "";
                string totalAvailableTime = "", totalUtilizedTime = "";
                reportName = "DownTimeReportFormat3.xlsx";
                strtTime = StartDate.ToString("dd-MMM-yyyy");
                endTime = EndDate.ToString("dd-MMM-yyyy");
                DataTable dt = TMPTrakDataBase.MachineDownTimeMatrixForConfidental(StartDate, EndDate, MachineId, PlantId, DownID, Exclude, "DTime", cellID, proc, "", operatorID.Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : operatorID, "DownReport");
                DataTable dtSummary = TMPTrakDataBase.MachineDownTimeMatrixForConfidental(StartDate, EndDate, MachineId, PlantId, DownID, Exclude, "DTime", cellID, proc, "", operatorID.Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : operatorID, "Sum");
                string src, dst = string.Empty;
                src = Util.GetReportPath(reportName);
                string tempfileName = "MachineDownTimeReport" + "_" + Guid.NewGuid() + ".xlsx";
                dst = Path.Combine(appPath, "Temp", SafeFileName(tempfileName));
                if (PlantId.Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    PlantId = "";
                if (MachineId.Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    MachineId = "";
                int screenWidth = Convert.ToInt32(width) - 40;
                int screenHeight = Convert.ToInt32(height) - 200;
                if (!File.Exists(src))
                {
                    Logger.WriteDebugLog("Machine Down Time Report Template Not Found at the Path - \n " + src);
                }

                FileInfo newFile = new FileInfo(src);
                ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                ExcelWorksheet ws = excelPackage.Workbook.Worksheets[0];
                ws.Cells["B3"].Value = strtTime;
                ws.Cells["D3"].Value = endTime;
                ws.Cells["F3"].Value = PlantId == "" ? "All" : PlantId;
                ws.Cells["H3"].Value = MachineId == "" ? "All" : MachineId;
                if (dt != null && dt.Rows.Count > 0)
                {
                    int rowCount = 6;
                    int cellCount = 1;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cellCount = 1;
                        ws.Cells[rowCount, cellCount].Value = dt.Rows[i]["MachineID"].ToString();
                        cellCount++;
                        ws.Cells[rowCount, cellCount].Value = dt.Rows[i]["MachineDescription"].ToString();
                        //cellCount++;
                        //ws.Cells[rowCount, cellCount].Value = dt.Rows[i]["Date"].ToString();
                        cellCount++;
                        ws.Cells[rowCount, cellCount].Value = dt.Rows[i]["ComponentID"].ToString();
                        cellCount++;
                        ws.Cells[rowCount, cellCount].Value = dt.Rows[i]["OperationNo"].ToString();
                        cellCount++;
                        ws.Cells[rowCount, cellCount].Value = dt.Rows[i]["OperatorName"].ToString();
                        cellCount++;
                        ws.Cells[rowCount, cellCount].Value = dt.Rows[i]["StartTime"].ToString();
                        cellCount++;
                        ws.Cells[rowCount, cellCount].Value = dt.Rows[i]["EndTime"].ToString();
                        cellCount++;
                        ws.Cells[rowCount, cellCount].Value = dt.Rows[i]["LapsedTime"].ToString();
                        cellCount++;
                        ws.Cells[rowCount, cellCount].Value = dt.Rows[i]["DownID"].ToString();
                        cellCount++;
                        ws.Cells[rowCount, cellCount].Value = dt.Rows[i]["DowThreshold"].ToString();
                        cellCount++;
                        ws.Cells[rowCount, cellCount].Value = dt.Rows[i]["PDT"].ToString();
                        cellCount++;
                        ws.Cells[rowCount, cellCount].Value = dt.Rows[i]["ActualDown"].ToString();
                        cellCount++;
                        ws.Cells[rowCount, cellCount].Value = dt.Rows[i]["StdSetup"].ToString();
                        cellCount++;
                        ws.Cells[rowCount, cellCount].Value = dt.Rows[i]["SetupEff"].ToString();
                        rowCount++;
                    }
                    for (int i = 1; i <= cellCount; i++)
                    {
                        ws.Column(i).AutoFit();
                    }
                    if (dtSummary.Rows.Count > 0)
                    {
                        ws.Cells[rowCount, 1].Value = "Total";
                        ws.Cells[rowCount, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowCount, 1, rowCount, cellCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[rowCount, 1, rowCount, cellCount].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 228, 214));
                        ws.Row(rowCount).Style.Font.Bold = true;
                        ws.Cells[rowCount, 1, rowCount, 7].Merge = true;
                        ws.Cells[rowCount, 8].Value = dtSummary.Rows[0]["LapsedSum"].ToString();
                        ws.Cells[rowCount, 12].Value = dtSummary.Rows[0]["DownSum"].ToString();
                        rowCount++;
                    }
                    setThinBorder(ws, 5, 1, rowCount - 1, cellCount);
                    DownloadMultipleFile(dst, excelPackage.GetAsByteArray());
                    Generated = "Generated";
                }
                else
                    Generated = "nodatafound";
                Logger.WriteDebugLog("Down time report to Machine wise down time details report generated sucessfully.");


            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return Generated;
        }
        private static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
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
        #endregion

        #region "OEE Trend Report "
        public static string OEETrendReportData(DateTime startDate, DateTime endDate, string shift, string plant, string machine)
        {
            string Generated = "";
            try
            {
                DataTable dt = new DataTable();
                string Source = string.Empty, destination = string.Empty, template = string.Empty;
                string ReportName = "OEE Trend.xlsx";
                Source = Util.GetReportPath(ReportName);
                template = "OEE Trend_" + DateTime.Now + ".xlsx";
                destination = Path.Combine(appPath, "Temp", SafeFileName(template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("OEE Trend.xlsx - \n " + Source);
                }
                FileInfo newFile = new FileInfo(Source);
                ExcelPackage Excel = new ExcelPackage(newFile, true);
                var ws = Excel.Workbook.Worksheets[0];
                string currmonth = "", currmonth1 = "";
                int rowposit = 0;

                System.Drawing.Image img = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(Util.getCompanyLogoPath()));
                ExcelPicture pic = ws.Drawings.AddPicture("geaLogo", img);
                pic.SetPosition(0, 0, 0, 0);
                pic.SetSize(270, 85);

                dt = TMPTrakDataBase.GetOEETrendReportData(startDate, endDate, shift, plant, machine, "All Machines", "Format1");
                if (dt != null && dt.Rows.Count > 0)
                {
                    ws.Cells[1, 2].Value = "ONLINE TRACKING OF OEE TREND ";
                    ws.Cells[2, 4].Value = startDate.ToString("dd-MMM-yyyy");
                    ws.Cells[2, 7].Value = endDate.ToString("dd-MMM-yyyy");
                    if (shift != string.Empty)
                        ws.Cells[2, 13].Value = shift;
                    else
                        ws.Cells[2, 13].Value = "ALL";

                    ws.Cells[3, 3].Value = "" + startDate.AddYears(-1).ToString("yyyy") + " - " + startDate.ToString("yyyy") + " (Actual)";
                    ws.Cells[3, 16].Value = "" + startDate.ToString("yyyy") + " - " + startDate.AddYears(1).ToString("yyyy") + " (Target)";
                    int Row = 4;
                    int col = 1;
                    int i = 0;

                    var distPlantMachineID = dt.AsEnumerable().Select(k => new { MachineID = k.Field<string>("MachineID"), PlantID = k.Field<string>("Plantid") }).Distinct().ToList();
                    for (i = 0; i < distPlantMachineID.Count; i++)
                    {
                        DataRow row = dt.AsEnumerable().Where(k => k.Field<string>("Plantid") == distPlantMachineID[i].PlantID && k.Field<string>("MachineID") == distPlantMachineID[i].MachineID).FirstOrDefault();
                        col = 1;
                        ws.Cells[Row, col].Value = row["Plantid"].ToString();

                        col = col + 1;
                        ws.Cells[Row, col].Value = row["MachineID"].ToString();

                        //col = col + 1;
                        //ws.Cells[Row, col].Value = row["ownername"].ToString();

                        col = col + 1;
                        ws.Cells[Row, col].Value = row["prevyearoee"];

                        col = 16;
                        ws.Cells[Row, col].Value = row["machinewisetarget"];
                        Row++;
                    }

                    /* ------------------------------------------------------------------ */
                    /* For displaying the data for Month wise  */
                    /* Starts here------------------------------------------------------- */
                    // dt = TMPTrakDataBase.GetWiproReportFormat1(string.Format("{0:yyyy-MM-dd}", DateTime.Parse(sttime)), string.Format("{0:yyyy-MM-dd}", DateTime.Parse(ndtime)), shiftname, plantname, machinename, "All Machines", "Format1");
                    Row = 4;
                    col = 4;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        currmonth = dt.Rows[i]["Pdate"].ToString();
                        if (currmonth1 != currmonth)
                        {
                            currmonth1 = dt.Rows[i]["Pdate"].ToString();
                            Row = 4;
                            if (i != 0)
                            {
                                col++;
                            }
                        }
                        ws.Cells[3, col].Value = dt.Rows[i]["Pdate"].ToString();
                        ws.Cells[Row, col].Value = dt.Rows[i]["oeffy"];
                        Row++;
                    }

                    string modelRange = "C3:C" + (Row - 1).ToString();
                    var modelTable = ws.Cells[modelRange];
                    modelTable.Style.Fill.PatternType = OfficeOpenXml2.Style.ExcelFillStyle.Solid;
                    modelTable.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 153, 204, 0));

                    string modelRange1 = "P3:P" + (Row - 1).ToString();
                    var modelTable1 = ws.Cells[modelRange1];
                    modelTable1.Style.Fill.PatternType = OfficeOpenXml2.Style.ExcelFillStyle.Solid;
                    modelTable1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 153, 204, 0));

                    string modelRange2 = "A3:P" + (Row - 1).ToString();
                    var modelTable2 = ws.Cells[modelRange2];
                    modelTable2.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    modelTable2.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    modelTable2.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    modelTable2.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    modelTable2.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    modelTable2.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    modelTable2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    modelTable2.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

                    ws.Cells["C3:P" + (Row - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["C3:P" + (Row - 1)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    /* Ends here----------------------------------------------------------- */

                    #region /* ----------------------Format 2 ------------------------------------- */
                    rowposit = Row;
                    Row = Row + 3;
                    col = 2;
                    dt = TMPTrakDataBase.GetOEETrendReportData(startDate, endDate, shift, plant, machine, "Critical Machines", "Format2");
                    string modelRange5 = "";
                    if (dt.Rows.Count > 0)
                    {
                        ws.Cells[Row, col].Value = "OEE FOR FOCUSED MACHINE";
                        string modelrange = "B" + Row + ":B" + Row;
                        var maintable = ws.Cells[modelrange];
                        maintable.Style.Fill.PatternType = OfficeOpenXml2.Style.ExcelFillStyle.Solid;
                        maintable.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 255, 204, 153));
                        maintable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        maintable.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[Row, col, Row, 6].Merge = true;


                        ws.Cells["B" + Row + ":F" + Row].Style.Font.Bold = true;
                        ws.Cells["B" + Row + ":F" + Row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["B" + Row + ":F" + Row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        ws.Cells[Row + 1, 2].Value = "Month";
                        ws.Cells[Row + 1, 3].Value = "OEE";
                        ws.Cells[Row + 1, 4].Value = "A";
                        ws.Cells[Row + 1, 5].Value = "P";
                        ws.Cells[Row + 1, 6].Value = "QR";

                        ws.Cells["B" + (Row + 1) + ":F" + (Row + 1)].Style.Fill.PatternType = OfficeOpenXml2.Style.ExcelFillStyle.Solid;
                        ws.Cells["B" + (Row + 1) + ":F" + (Row + 1)].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 153, 204, 255));
                        ws.Cells["B" + (Row + 1) + ":F" + (Row + 1)].Style.Font.Bold = true;
                        int rwcol = Row + 2;
                        ws.Cells[Row + 2, 2].Value = startDate.AddYears(-1).ToString("yyyy") + " - " + startDate.ToString("yyyy");
                        ws.Cells[Row + 2, 3].Value = dt.Rows[0]["prevyearoee"];
                        ws.Cells[Row + 15, 2].Value = "Target";
                        ws.Cells[Row + 15, 3].Value = dt.Rows[0]["machinewisetarget"];
                        Row = Row + 3;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            col = 2;
                            ws.Cells[Row, col].Value = dt.Rows[i]["Pdate"].ToString();

                            col = col + 1;
                            ws.Cells[Row, col].Value = dt.Rows[i]["oeffy"];

                            col = col + 1;
                            ws.Cells[Row, col].Value = dt.Rows[i]["aeffy"];

                            col = col + 1;
                            ws.Cells[Row, col].Value = dt.Rows[i]["peffy"];

                            col = col + 1;
                            ws.Cells[Row, col].Value = dt.Rows[i]["qeffy"];

                            Row = Row + 1;
                        }
                        ws.Cells["B" + rwcol + ":B" + Row].Style.Font.Color.SetColor(Color.Brown);
                        ws.Cells["B" + rwcol + ":B" + Row].Style.Font.Bold = true;

                        modelRange5 = "B" + (rowposit + 3) + ":F" + (Row).ToString();
                        var modelTable5 = ws.Cells[modelRange5];
                        modelTable5.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        modelTable5.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        modelTable5.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        modelTable5.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        modelTable5.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        modelTable5.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        modelTable5.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        modelTable5.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

                        ws.Cells["C" + (Row) + ":C" + (Row)].Style.Fill.PatternType = OfficeOpenXml2.Style.ExcelFillStyle.Solid;
                        ws.Cells["C" + (Row) + ":C" + (Row)].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 153, 204, 0));
                        ws.Cells["C" + (rwcol) + ":C" + (rwcol)].Style.Fill.PatternType = OfficeOpenXml2.Style.ExcelFillStyle.Solid;
                        ws.Cells["C" + (rwcol) + ":C" + (rwcol)].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 255, 137, 79));
                        ws.Cells["C" + (rowposit + 3) + ":F" + Row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["C" + (rowposit + 3) + ":F" + Row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["B" + (rwcol - 2) + ":F" + Row].AutoFitColumns();
                    }
                    #endregion /* Ends here--------------------------------------------------------- */

                    #region /* ----------------------Format 3 ------------------------------------- */
                    /* Starts here--------------------------------------------------------- */
                    Row = rowposit;
                    Row = Row + 3;
                    col = 8;

                    dt = TMPTrakDataBase.GetOEETrendReportData(startDate, endDate, shift, plant, machine, "ALL Machines", "Format3");
                    if (dt.Rows.Count > 0)
                    {
                        ws.Cells[Row, col].Value = "OVERALL PLANT OEE";
                        int rowval = Row;
                        ws.Cells["H" + Row + ":H" + (Row)].Style.Fill.PatternType = OfficeOpenXml2.Style.ExcelFillStyle.Solid;
                        ws.Cells["H" + (Row) + ":H" + (Row)].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 255, 204, 153));
                        string modelRange6 = "H" + Row + ":L" + (Row).ToString();
                        var modelTable6 = ws.Cells[modelRange5];
                        modelTable6.Style.Font.Bold = true;
                        ws.Cells["H" + Row + ":L" + Row].Merge = true;
                        ws.Cells[Row + 1, 8].Value = "Month";
                        ws.Cells[Row + 1, 9].Value = "OEE";
                        ws.Cells[Row + 1, 10].Value = "A";
                        ws.Cells[Row + 1, 11].Value = "P";
                        ws.Cells[Row + 1, 12].Value = "QR";
                        ws.Cells["H" + (Row + 1) + ":L" + (Row + 1)].Style.Fill.PatternType = OfficeOpenXml2.Style.ExcelFillStyle.Solid;
                        ws.Cells["H" + (Row + 1) + ":L" + (Row + 1)].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                        ws.Cells["H" + (Row + 1) + ":L" + (Row + 1)].Style.Font.Bold = true;
                        ws.Cells[Row + 14, 8].Value = "Target";
                        ws.Cells[Row + 14, 9].Value = dt.Rows[0]["machinewisetarget"];
                        Row = Row + 2;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            col = 8;
                            ws.Cells[Row, col].Value = dt.Rows[i]["Pdate"].ToString();

                            col = col + 1;
                            ws.Cells[Row, col].Value = dt.Rows[i]["oeffy"];

                            col = col + 1;
                            ws.Cells[Row, col].Value = dt.Rows[i]["aeffy"];

                            col = col + 1;
                            ws.Cells[Row, col].Value = dt.Rows[i]["peffy"];

                            col = col + 1;
                            ws.Cells[Row, col].Value = dt.Rows[i]["qeffy"];

                            Row = Row + 1;
                        }

                        ws.Cells["H" + rowval + ":H" + Row].Style.Font.Color.SetColor(Color.Brown);
                        ws.Cells["H" + rowval + ":H" + Row].Style.Font.Bold = true;
                        string modelRange7 = "H" + rowval + ":L" + Row.ToString();
                        var modelTable7 = ws.Cells[modelRange7];
                        modelTable7.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        modelTable7.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        modelTable7.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        modelTable7.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        modelTable7.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        modelTable7.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        modelTable7.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        modelTable7.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                        ws.Cells["I" + Row + ":I" + Row].Style.Fill.PatternType = OfficeOpenXml2.Style.ExcelFillStyle.Solid;
                        ws.Cells["I" + Row + ":I" + Row].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 153, 204, 0));
                    }
                    ws.Cells[3, 1, Row, 16].AutoFitColumns();
                    #endregion /* Ends here--------------------------------------------------------- */


                    #region  /* ----------------------Format 4 ------------------------------------- */
                    int sheetno;
                    int row1;
                    string Cur_machine = string.Empty;
                    sheetno = 1;

                    ws = Excel.Workbook.Worksheets[sheetno];

                    dt = TMPTrakDataBase.GetOEETrendReportData(startDate, endDate, shift, plant, machine, "ALL Machines", "Format4");
                    ws = Excel.Workbook.Worksheets[sheetno];
                    Row = 1;
                    col = 1;
                    if (dt.Rows.Count > 0)
                    {
                        ws.Cells[4, 1].Value = startDate.AddYears(-1).ToString("yyyy") + " - " + startDate.ToString("yyyy");
                        // DataRow firstRow = dt.Rows[0];
                        //if (firstRow["prevyearoee"].ToString() == string.Empty)
                        //{
                        //    ws.Cells[4, 2].Value = string.Empty;
                        //}
                        //else
                        //{
                        //    ws.Cells[4, 2].Value = firstRow["prevyearoee"].ToString();
                        //}
                        //wrksht.Cells[17, 1] = "Target";
                        //if (firstRow["machinewisetarget"].ToString() == string.Empty)
                        //{
                        //    ws.Cells[17, 2].Value = string.Empty;
                        //}
                        //else
                        //{
                        //    ws.Cells[17, 2].Value = firstRow["machinewisetarget"].ToString();
                        //}
                        ws.Cells[2, 2].Value = "" + startDate.ToString("dd-MMM-yyyy");
                        ws.Cells[2, 5].Value = "" + endDate.ToString("dd-MMM-yyyy");

                        if (shift != string.Empty)
                            ws.Cells[2, 6].Value = "Shift: " + shift + " ";
                        else
                            ws.Cells[2, 6].Value = " Shift: ALL";

                        Cur_machine = string.Empty;
                        Row = 3;
                        ExcelWorksheet copyworksheet = ws;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Cur_machine == dt.Rows[i]["machineid"].ToString())
                            {
                                col = 1;
                                ws.Cells[Row, col].Value = dt.Rows[i]["Pdate"].ToString();

                                col = col + 2;
                                ws.Cells[Row, col].Value = dt.Rows[i]["oeffy"];

                                col = col + 1;
                                ws.Cells[Row, col].Value = dt.Rows[i]["aeffy"];

                                col = col + 1;
                                ws.Cells[Row, col].Value = dt.Rows[i]["peffy"];

                                col = col + 1;
                                ws.Cells[Row, col].Value = dt.Rows[i]["QEffy"];
                                Row = Row + 1;
                            }
                            else
                            {
                                ws = Excel.Workbook.Worksheets.Add(dt.Rows[i]["machineid"].ToString(), copyworksheet);
                                Row = 5;
                                col = 1;
                                Cur_machine = dt.Rows[i]["machineid"].ToString();
                                ws.Cells[1, 1].Value = Cur_machine;
                                if (dt.Rows[i]["prevyearoee"].ToString() == string.Empty)
                                {
                                    ws.Cells[4, 2].Value = string.Empty;
                                }
                                else
                                {
                                    ws.Cells[4, 2].Value = dt.Rows[i]["prevyearoee"];
                                }
                                if (dt.Rows[i]["machinewisetarget"].ToString() == string.Empty)
                                {
                                    ws.Cells[17, 2].Value = string.Empty;
                                }
                                else
                                {
                                    ws.Cells[17, 2].Value = dt.Rows[i]["machinewisetarget"];
                                }
                                var chart11 = ws.Drawings["Chart 2"] as ExcelChart;
                                chart11.Title.Text = Cur_machine;

                                var chart12 = ws.Drawings["Chart 3"] as ExcelChart;
                                chart12.Title.Text = Cur_machine + " PARETO LOSS ";
                                i--;
                            }
                        }
                    }

                    #endregion


                    #region  /* ----------------------Format 5 ------------------------------------- */
                    string check = string.Empty;

                    sheetno = 2;
                    dt = TMPTrakDataBase.GetOEETrendReportData(startDate, endDate, shift, plant, machine, "ALL Machines", "Format5");

                    if (dt.Rows.Count > 0)
                    {
                        var distMachineID = dt.AsEnumerable().Select(k => k.Field<string>("Machineid")).Distinct().ToList();
                        foreach (string machineID in distMachineID)
                        {
                            row1 = 23;
                            ws = Excel.Workbook.Worksheets[machineID];
                            var rows = dt.AsEnumerable().Where(k => k.Field<string>("Machineid") == machineID).ToList();
                            decimal totalTime = 0;
                            int startRow = row1;
                            ws.Cells["B21"].Formula = "=SUM(B" + startRow + ":B" + (startRow + rows.Count - 1) + ")";
                            foreach (DataRow row in rows)
                            {
                                ws.Cells[row1, 1].Value = row["Downid"].ToString();
                                ws.Cells[row1, 2].Value = row["downtime"];
                                totalTime += Convert.ToDecimal(row["downtime"].ToString());
                                ws.Cells[row1, 3].Value = totalTime;
                                ws.Cells[row1, 4].Formula = "=(C" + row1 + "/B21)*100";
                                row1++;
                            }
                            int chartCount = 0;
                            foreach (var excelDrawing in ws.Drawings)
                            {
                                chartCount++;
                                if (!(excelDrawing is ExcelChart chart))
                                    continue;
                                foreach (var chartType in chart.PlotArea.ChartTypes)
                                {
                                    foreach (ExcelChartSerie serie in chartType.Series)
                                    {
                                        serie.Series = "'" + machineID + "'!" + serie.Series.Split('!')[1].Replace("26", (row1 - 1).ToString());
                                        serie.XSeries = "'" + machineID + "'!" + serie.XSeries.Split('!')[1].Replace("26", (row1 - 1).ToString());
                                        //serie.Series = "'" + machineID + "'!" + serie.Series.Split('!')[1];
                                        //serie.XSeries = "'" + machineID + "'!" + serie.XSeries.Split('!')[1];
                                        //serie.Series = serie.Series.Replace("Sheet2", "'" + machineID + "'");
                                        //serie.XSeries = serie.XSeries.Replace("Sheet2", "'" + machineID + "'");
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                    Excel.Workbook.Worksheets.Delete("Sheet2");
                    //setThinBorder(worksheet, 6, 1, rowCount - 1, cellCount);
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    Generated = "Generated";
                }
                else
                    Generated = "Nodatafound";
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog(ex.Message);
            }
            return Generated;
        }
        #endregion

        #region -----Component Setup Report----
        internal static string ComponentSetupReport(DateTime fromDate, DateTime toDate, string PlantID, string CellID, string machineID, string Operator, string ComponentID)
        {
            string Generated = "";
            try
            {
                string src, dst = string.Empty;
                string reportName = "ComponentSetupReport.xlsx";
                src = Util.GetReportPath(reportName);

                string tempfileName = "ComponentSetupReport" + "_" + Guid.NewGuid() + ".xlsx";
                dst = Path.Combine(appPath, "Temp", SafeFileName(tempfileName));

                if (!File.Exists(src))
                {
                    Logger.WriteDebugLog("Component Setup report Log by day-Excel - \n " + src);
                }

                string Comp = ComponentID.Equals("") ? "All" : ComponentID;
                string Opr = Operator.Equals("") ? "All" : Operator;

                FileInfo newFile = new FileInfo(src);
                ExcelPackage pck = new ExcelPackage(newFile, true);
                var wsDts = pck.Workbook.Worksheets["Sheet1"];
                wsDts.Cells["B2"].Value = fromDate.ToString("dd-MMM-yyyy hh:mm:ss");
                wsDts.Cells["F2"].Value = toDate.ToString("dd-MMM-yyyy hh:mm:ss");
                wsDts.Cells["K2"].Value = CellID;
                wsDts.Cells["B4"].Value = machineID;
                wsDts.Cells["G4"].Value = Comp.Replace("'", "");
                wsDts.Cells["K4"].Value = Opr.Replace("'", "");

                wsDts.PrinterSettings.PaperSize = ePaperSize.A3;
                wsDts.PrinterSettings.Orientation = eOrientation.Landscape;

                string modelRange = "";
                DataTable Setupdowndt = new DataTable();
                DataTable dt = TMPTrakDataBase.GetComponentSetupDetails(fromDate, toDate, PlantID, CellID, machineID, ComponentID, Operator, out Setupdowndt);
                DataTable Downcodelst = TMPTrakDataBase.GetDownCodeList();
                int DownCode_count = Downcodelst.AsEnumerable().Select(x => x.Field<string>("downid")).Distinct().Count();

                DataRow rows = Downcodelst.NewRow();
                rows["downid"] = "ComponentID";
                Downcodelst.Rows.InsertAt(rows, 0);

                int row = 8, col = 1;

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        wsDts.Cells[row, 1].Value = dt.Rows[i]["Component"];
                        wsDts.Cells[row, 2].Value = dt.Rows[i]["Operation"];
                        wsDts.Cells[row, 3].Value = dt.Rows[i]["Machineid"];
                        wsDts.Cells[row, 3, row, 4].Merge = true;
                        wsDts.Cells[row, 5].Value = Util.GetDateTime(dt.Rows[i]["BatchStart"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        wsDts.Cells[row, 5, row, 6].Merge = true;
                        wsDts.Cells[row, 7].Value = Util.GetDateTime(dt.Rows[i]["BatchEnd"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        wsDts.Cells[row, 7, row, 8].Merge = true;
                        wsDts.Cells[row, 9].Value = dt.Rows[i]["Operator"];
                        wsDts.Cells[row, 9, row, 10].Merge = true;
                        //wsDts.Cells[row, 11].Value = dt.Rows[i]["StdSetupTime"];
                        //wsDts.Cells[row, 12].Value = dt.Rows[i]["ActSetupTime"];
                        setTimeSpanFormat("hh:mm:ss", wsDts, row, 11, dt.Rows[i]["StdSetupTime"].ToString());
                        setTimeSpanFormat("hh:mm:ss", wsDts, row, 12, dt.Rows[i]["ActSetupTime"].ToString());
                        row++;
                    }
                    int lastrow = row - 1;
                    int newrow = row + 1;

                    modelRange = wsDts.Cells[8, 1, lastrow, 12].ToString();
                    var modelTable = wsDts.Cells[modelRange];
                    // Assign borders
                    modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    wsDts.Cells[modelRange].AutoFitColumns();

                    //Second DownReason Grid
                    foreach (DataRow downitem in Downcodelst.Rows)
                    {

                        wsDts.Cells[newrow, col].Value = downitem["downid"].ToString();
                        wsDts.Cells[newrow, col].Style.Font.Bold = true;
                        wsDts.Cells[newrow, col].Style.Font.Size = 11;
                        wsDts.Cells[newrow, col].Style.TextRotation = 90;
                        wsDts.Cells[newrow, col].Style.WrapText = true;
                        wsDts.Row(newrow).Height = 76;
                        wsDts.Cells[newrow, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        wsDts.Cells[newrow, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsDts.Cells[newrow, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        wsDts.Cells[newrow, col].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FCD5B4"));

                        wsDts.Cells[newrow, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        wsDts.Cells[newrow, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        wsDts.Cells[newrow, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        wsDts.Cells[newrow, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        wsDts.Cells[newrow, col].Style.Border.Top.Color.SetColor(Color.Black);
                        wsDts.Cells[newrow, col].Style.Border.Right.Color.SetColor(Color.Black);
                        wsDts.Cells[newrow, col].Style.Border.Left.Color.SetColor(Color.Black);
                        wsDts.Cells[newrow, col].Style.Border.Bottom.Color.SetColor(Color.Black);

                        col++;
                    }
                    newrow++;
                    int lastcol = DownCode_count + 1;
                    if (Setupdowndt != null && Setupdowndt.Rows.Count > 0)
                    {
                        for (int j = 0; j < Setupdowndt.Rows.Count; j++)
                        {
                            int c1 = 2;
                            wsDts.Cells[newrow, 1].Value = Setupdowndt.Rows[j]["Component"];
                            for (int i = 2; i <= lastcol; i++)
                            {
                                //wsDts.Cells[newrow, i].Value = Setupdowndt.Rows[j][c1];
                                setTimeSpanFormat("hh:mm:ss", wsDts, newrow, i, Setupdowndt.Rows[j][c1].ToString());
                                c1++;
                            }

                            newrow++;
                        }
                    }

                    modelRange = wsDts.Cells[lastrow + 2, 1, newrow - 1, lastcol].ToString();
                    var modelTable1 = wsDts.Cells[modelRange];
                    // Assign borders
                    modelTable1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    modelTable1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    modelTable1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    modelTable1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    //wsDts.Cells[modelRange].AutoFitColumns();

                    DownloadMultipleFile(dst, pck.GetAsByteArray());
                    Logger.WriteDebugLog("Component Setup report generated sucessfully.");
                    Generated = "Generated";
                }
                else
                    Generated = "Nodatafound";

                #region --oldcode--
                //int lastcol = col + DownCode_count-1;

                //wsDts.Cells[4, col, 4, lastcol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //wsDts.Cells[4, col, 4, lastcol].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FCD5B4"));
                //wsDts.Cells[4, col, 4, lastcol].Merge = true;
                //wsDts.Cells[4, col, 4, lastcol].Value = "Setup Delay Reasons";
                //wsDts.Cells[4, col, 4, lastcol].Style.Font.Bold = true;
                //wsDts.Cells[4, col, 4, lastcol].Style.Font.Size = 18;
                //wsDts.Cells[4, col, 4, lastcol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //wsDts.Cells[4, col, 4, lastcol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //wsDts.Cells[4, col, 4, lastcol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //wsDts.Cells[4, col, 4, lastcol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //wsDts.Cells[4, col, 4, lastcol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //wsDts.Cells[4, col, 4, lastcol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //wsDts.Cells[4, col, 4, lastcol].Style.Border.Top.Color.SetColor(Color.Black);
                //wsDts.Cells[4, col, 4, lastcol].Style.Border.Right.Color.SetColor(Color.Black);
                //wsDts.Cells[4, col, 4, lastcol].Style.Border.Left.Color.SetColor(Color.Black);
                //wsDts.Cells[4, col, 4, lastcol].Style.Border.Bottom.Color.SetColor(Color.Black);

                //foreach (DataRow downitem in Downcodelst.Rows)
                //{
                //    wsDts.Cells[5, col, 6, col].Value = downitem["downid"].ToString();
                //    wsDts.Cells[5, col, 6, col].Merge = true;
                //    wsDts.Cells[5, col, 6, col].Style.Font.Bold = true;
                //    wsDts.Cells[5, col, 6, col].Style.Font.Size = 11;
                //    wsDts.Cells[5, col, 6, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    wsDts.Cells[5, col, 6, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //    wsDts.Cells[5, col, 6, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //    wsDts.Cells[5, col, 6, col].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FCD5B4"));

                //    wsDts.Cells[5, col, 6, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //    wsDts.Cells[5, col, 6, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //    wsDts.Cells[5, col, 6, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //    wsDts.Cells[5, col, 6, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //    wsDts.Cells[5, col, 6, col].Style.Border.Top.Color.SetColor(Color.Black);
                //    wsDts.Cells[5, col, 6, col].Style.Border.Right.Color.SetColor(Color.Black);
                //    wsDts.Cells[5, col, 6, col].Style.Border.Left.Color.SetColor(Color.Black);
                //    wsDts.Cells[5, col, 6, col].Style.Border.Bottom.Color.SetColor(Color.Black);

                //    col++;
                //}

                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        int c1 = 12;
                //        wsDts.Cells[row, 1].Value = dt.Rows[i]["Component"];
                //        wsDts.Cells[row, 2].Value = dt.Rows[i]["Operation"];
                //        wsDts.Cells[row, 3].Value = dt.Rows[i]["Machineid"];
                //        wsDts.Cells[row, 4].Value = Util.GetDateTime(dt.Rows[i]["BatchStart"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                //        wsDts.Cells[row, 5].Value = Util.GetDateTime(dt.Rows[i]["BatchEnd"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                //        wsDts.Cells[row, 6].Value = dt.Rows[i]["Operator"];
                //        wsDts.Cells[row, 7].Value = dt.Rows[i]["StdSetupTime"];
                //        wsDts.Cells[row, 8].Value = dt.Rows[i]["ActSetupTime"];
                //        for (int j = 9; j <= lastcol; j++)
                //        {
                //            wsDts.Cells[row, j].Value = dt.Rows[i][c1];
                //            c1++;
                //        }

                //        row++;
                //    }
                //    int lastrow = row - 1;

                //    modelRange = wsDts.Cells[7, 1, lastrow, lastcol].ToString();
                //    var modelTable = wsDts.Cells[modelRange];
                //    // Assign borders
                //    modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //    modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //    modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //    modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //    //wsDts.Cells[modelRange].AutoFitColumns();

                //    DownloadMultipleFile(dst, pck.GetAsByteArray());
                //    Logger.WriteDebugLog("Component Setup report generated sucessfully.");
                //    Generated = "Generated";
                //}
                //else
                //    Generated = "Nodatafound";
                #endregion

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return Generated;
        }
        #endregion

        #region ---- Leonine Report ---
        public static string GenerateLeonineReport(string shift, DateTime fromDate, DateTime toDate, string grpId, string machineId)
        {
            string Generated = "";
            try
            {

                // string startDate = "", endDate = "";
                DataTable dt = TMPTrakDataBase.GetLeonineReportData(shift, fromDate, toDate, grpId, machineId);
                string src, dst = string.Empty;
                src = Util.GetReportPath("LeonineHourlyProductionReport.xlsx");
                string tempfileName = "LeonineHourlyProductionReport" + "_" + Guid.NewGuid() + ".xlsx";
                dst = Path.Combine(appPath, "Temp", SafeFileName(tempfileName));
                if (!File.Exists(src))
                {
                    Logger.WriteDebugLog("Leonine Hourly Production Report Template Not Found at the Path - \n " + src);
                }

                FileInfo newFile = new FileInfo(src);
                ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                excelPackage.Workbook.Worksheets.Delete("Sheet1");


                if (dt != null && dt.Rows.Count > 0)
                {

                    var distinctHour = dt.AsEnumerable().Select(x => x.Field<dynamic>("HourID")).Distinct();
                    var distinctDate = dt.AsEnumerable().Select(x => x.Field<dynamic>("ShiftDate")).Distinct();
                    var distinctHourStartEnd = dt.AsEnumerable().Select(x => new { HourStart = x.Field<DateTime>("HourStart"), HourEnd = x.Field<DateTime>("HourEnd") }).Distinct().ToList();

                    int wsCount = 0, componentColNo = 3, opnColNo = 4, oprColNo = 5, fromHourlyTargetColNo = 18, toCommentColNo = 24;
                    foreach (var date in distinctDate)
                    {
                        // ExcelWorksheet ws = excelPackage.Workbook.Worksheets[wsCount];
                        string dateToDisplay = Util.GetDateTime(Convert.ToString(date)).ToString("dd-MM-yyyy");
                        ExcelWorksheet ws = excelPackage.Workbook.Worksheets.Add(dateToDisplay);
                        //ws.Name = dateToDisplay;
                        int colStart = 1, rowStart = 2;
                        LeonnineReportHeader(ws, rowStart, colStart, "Shift", true);
                        colStart++;
                        LeonnineReportHeader(ws, rowStart, colStart, "Machine", true);
                        colStart++;
                        LeonnineReportHeader(ws, rowStart, colStart, "Component", true);
                        componentColNo = colStart;
                        colStart++;
                        LeonnineReportHeader(ws, rowStart, colStart, "Opn", true);
                        opnColNo = colStart;
                        colStart++;
                        LeonnineReportHeader(ws, rowStart, colStart, "Operator", true);
                        oprColNo = colStart;
                        colStart++;
                        int hourC = 0;
                        foreach (var hour in distinctHour)
                        {

                            string hourStartEnd = Convert.ToDateTime(distinctHourStartEnd[hourC].HourStart).ToString("HH:mm") + " -\n" + Convert.ToDateTime(distinctHourStartEnd[hourC].HourEnd).ToString("HH:mm");
                            LeonnineReportHeader(ws, rowStart, colStart, hourStartEnd, false);
                            ws.Cells[rowStart, colStart].Style.Font.Size = 12;
                            ws.Cells[rowStart, colStart].Style.WrapText = true;
                            LeonnineReportHeader(ws, rowStart + 1, colStart, Convert.ToString(hour), false);
                            colStart++;
                            hourC++;
                        }
                        LeonnineReportHeader(ws, rowStart, colStart, "Hourly\nTarget", true);
                        fromHourlyTargetColNo = colStart;
                        colStart++;
                        LeonnineReportHeader(ws, rowStart, colStart, "Shift\nTarget", true);
                        colStart++;
                        LeonnineReportHeader(ws, rowStart, colStart, "Total\nOutput", true);
                        colStart++;
                        LeonnineReportHeader(ws, rowStart, colStart, "Setup Time\n(Min)", true);
                        colStart++;
                        LeonnineReportHeader(ws, rowStart, colStart, "Production\nHours", true);
                        colStart++;
                        LeonnineReportHeader(ws, rowStart, colStart, "Operator\nSign", true);
                        colStart++;
                        LeonnineReportHeader(ws, rowStart, colStart, "TPM/Supervisor\nComments", true);
                        toCommentColNo = colStart;
                        ws.Row(rowStart).Height = 38;


                        ws.Cells[1, 1, 1, colStart].Merge = true;
                        ws.Cells["A1"].Value = "Hourly Production Reports   -   Date: " + dateToDisplay;
                        ws.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A1"].Style.Font.Size = 15;
                        ws.Cells["A1"].Style.Font.Bold = true;
                        ws.Cells["A1"].Style.Font.UnderLineType = ExcelUnderLineType.Single;
                        ws.Row(1).Height = 35;

                        colStart = 1;
                        rowStart += 2;

                        var distinctShiftMachine = dt.AsEnumerable().Where(x => x.Field<dynamic>("ShiftDate") == date).Select(x => new { shift = x.Field<dynamic>("ShiftName"), machine = x.Field<dynamic>("MachineID") }).Distinct();

                        foreach (var sm in distinctShiftMachine)
                        {
                            DataTable dtShiftMachineDeails = dt.AsEnumerable().Where(x => x.Field<dynamic>("ShiftDate") == date && x.Field<dynamic>("ShiftName") == sm.shift && x.Field<dynamic>("MachineID") == sm.machine).CopyToDataTable();

                            var distinctCOO = dtShiftMachineDeails.AsEnumerable().Select(x => new { component = x.Field<dynamic>("ComponentID"), operation = x.Field<dynamic>("OperationNo"), opr = x.Field<dynamic>("OperatorID") }).Distinct();

                            Double totalProductionHour = 0;
                            int styleColStart = 1, styleRowStart = rowStart;
                            foreach (var coo in distinctCOO)
                            {
                                colStart = 1;
                                DataTable dtCOODeails = dt.AsEnumerable().Where(x => x.Field<dynamic>("ShiftDate") == date && x.Field<dynamic>("ShiftName") == sm.shift && x.Field<dynamic>("MachineID") == sm.machine && x.Field<dynamic>("ComponentID") == coo.component && x.Field<dynamic>("OperationNo") == coo.operation && x.Field<dynamic>("OperatorID") == coo.opr).CopyToDataTable();

                                ws.Cells[rowStart, colStart].Value = sm.shift;
                                colStart++;
                                ws.Cells[rowStart, colStart].Value = sm.machine;
                                colStart++;
                                ws.Cells[rowStart, colStart].Value = coo.component;
                                colStart++;
                                ws.Cells[rowStart, colStart].Value = coo.operation;
                                colStart++;
                                ws.Cells[rowStart, colStart].Value = coo.opr;
                                colStart++;
                                foreach (var hour in distinctHour)
                                {
                                    ws.Cells[rowStart, colStart].Value = dtCOODeails.AsEnumerable().Where(x => x.Field<dynamic>("HourID") == hour).Select(x => x.Field<dynamic>("HourlyPartCount")).FirstOrDefault();
                                    colStart++;
                                }
                                var cooDetails = dtCOODeails.AsEnumerable().Select(x => new { hourlytarget = x.Field<dynamic>("HourlyTarget"), shifttarget = x.Field<dynamic>("ShiftTarget"), totaloutput = x.Field<dynamic>("TotalOutput"), setuptime = x.Field<dynamic>("SetupTimeInMin"), productionhours = x.Field<dynamic>("ProductionHours") }).FirstOrDefault();
                                styleColStart = colStart;
                                ws.Cells[rowStart, colStart].Value = cooDetails.hourlytarget;
                                colStart++;
                                ws.Cells[rowStart, colStart].Value = cooDetails.shifttarget;
                                colStart++;
                                ws.Cells[rowStart, colStart].Value = cooDetails.totaloutput;
                                colStart++;
                                ws.Cells[rowStart, colStart].Value = cooDetails.setuptime;
                                colStart++;
                                ws.Cells[rowStart, colStart].Value = cooDetails.productionhours;
                                totalProductionHour += Convert.ToDouble(cooDetails.productionhours);
                                //colStart++;

                                ws.Row(rowStart).Height = 25;
                                rowStart++;
                            }

                            ws.Cells[styleRowStart, styleColStart, rowStart, colStart + 4].Style.Font.Size = 14;
                            ws.Cells[styleRowStart, styleColStart, rowStart, colStart + 4].Style.Font.Bold = true;

                            var distinctComponentID = dtShiftMachineDeails.AsEnumerable().Select(x => x.Field<dynamic>("ComponentID")).Distinct();
                            var distinctOperation = dtShiftMachineDeails.AsEnumerable().Select(x => x.Field<dynamic>("OperationNo")).Distinct();
                            var distinctOperator = dtShiftMachineDeails.AsEnumerable().Select(x => x.Field<dynamic>("OperatorID")).Distinct();
                            if (distinctComponentID.Count() > 1 || distinctOperation.Count() > 1 || distinctOperator.Count() > 1)
                            {
                                colStart = 1;
                                ws.Cells[rowStart, colStart].Value = sm.shift;
                                colStart++;
                                ws.Cells[rowStart, colStart].Value = sm.machine;
                                colStart++;
                                ws.Cells[rowStart, colStart].Value = String.Join(",\n", distinctComponentID.ToArray());
                                colStart++;
                                ws.Cells[rowStart, colStart].Value = String.Join(",\n", distinctOperation.ToArray());
                                colStart++;
                                ws.Cells[rowStart, colStart].Value = String.Join(",\n", distinctOperator.ToArray());
                                colStart = colStart + distinctHour.Count(); //hour
                                colStart = colStart + 5;
                                ws.Cells[rowStart, colStart].Value = totalProductionHour;

                                int maxRows = new[] { distinctComponentID.Count(), distinctOperation.Count(), distinctOperator.Count() }.Max();
                                ws.Row(rowStart).Height = 18 * maxRows;
                                ws.Cells[rowStart, 1, rowStart, colStart + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[rowStart, 1, rowStart, colStart + 2].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#92d050"));

                                //colStart++;
                                rowStart++;
                            }
                        }

                        colStart = colStart + 2; // for 2 dummy column
                        rowStart--;
                        ws.Cells[2, 1, rowStart, colStart].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[2, 1, rowStart, colStart].AutoFitColumns();
                        ws.Column(componentColNo).Style.WrapText = true;
                        ws.Column(opnColNo).Style.WrapText = true;
                        ws.Column(oprColNo).Style.WrapText = true;
                        for (int i = fromHourlyTargetColNo; i <= toCommentColNo; i++)
                        {
                            ws.Column(i).Style.WrapText = true;
                        }
                        //ws.Cells[2, 1, rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //ws.Cells[2, 1, rowStart, colStart].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#CFB296"));
                        //ws.Cells[2, 1, rowStart, colStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //ws.Cells[2, 1, rowStart, colStart].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[2, 1, rowStart, colStart].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[2, 1, rowStart, colStart].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[2, 1, rowStart, colStart].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[2, 1, rowStart, colStart].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[2, 1, rowStart, colStart].Style.Border.Bottom.Color.SetColor(Color.Black);
                        ws.Cells[2, 1, rowStart, colStart].Style.Border.Top.Color.SetColor(Color.Black);
                        ws.Cells[2, 1, rowStart, colStart].Style.Border.Left.Color.SetColor(Color.Black);
                        ws.Cells[2, 1, rowStart, colStart].Style.Border.Right.Color.SetColor(Color.Black);

                        ws.PrinterSettings.FitToPage = true;
                        ws.PrinterSettings.PaperSize = ePaperSize.A4;
                        ws.PrinterSettings.Orientation = eOrientation.Portrait;
                        ws.PrinterSettings.BottomMargin = (decimal)1.91 / 2.54M; ;
                        ws.PrinterSettings.TopMargin = (decimal)1.91 / 2.54M; ;
                        ws.PrinterSettings.LeftMargin = (decimal).64 / 2.54M; ;
                        ws.PrinterSettings.RightMargin = (decimal).64 / 2.54M; ;
                        //ws.PrinterSettings.HorizontalCentered = true;
                        //ws.PrinterSettings.FitToWidth = 1;
                        //ws.PrinterSettings.FitToHeight = 0;
                        wsCount++;
                    }

                    DownloadMultipleFile(dst, excelPackage.GetAsByteArray());
                    Logger.WriteDebugLog("Down time report to Machine wise down time details report generated sucessfully.");
                    Generated = "Generated";
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return Generated;
        }
        private static void LeonnineReportHeader(ExcelWorksheet excelWorksheet, int rowStart, int colStart, string value, bool isMerge)
        {
            try
            {
                excelWorksheet.Cells[rowStart, colStart].Value = value;
                excelWorksheet.Cells[rowStart, colStart].Style.Font.Size = 13;
                excelWorksheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                excelWorksheet.Cells[rowStart, colStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                excelWorksheet.Cells[rowStart, colStart].Style.Font.Color.SetColor(ColorTranslator.FromHtml("#060a7e"));
                if (isMerge)
                {
                    excelWorksheet.Cells[rowStart, colStart, rowStart + 1, colStart].Merge = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("LeonnineReportHeader: " + ex.Message);
            }
        }


        #endregion

        #region ---KKPillar Report----
        internal static string KKPillarReport(DateTime fromDate, DateTime toDate, string PlantID, string ShiftID, string machineId)
        {
            string Generated = "";
            try
            {
                string src, dst = string.Empty;
                string reportName = "KKPillarReport.xlsx";
                src = Util.GetReportPath(reportName);

                string tempfileName = "KKPillarReport" + "_" + Guid.NewGuid() + ".xlsx";
                dst = Path.Combine(appPath, "Temp", SafeFileName(tempfileName));

                if (!File.Exists(src))
                {
                    Logger.WriteDebugLog("KKPillar report Log by day-Excel - \n " + src);
                }


                FileInfo newFile = new FileInfo(src);
                ExcelPackage pck = new ExcelPackage(newFile, true);
                var wsDts = pck.Workbook.Worksheets["Sheet1"];

                string modelRange = "";
                int row = 5, columns = 1;
                DataTable Setupdowndt = new DataTable();
                DataTable dt = TMPTrakDataBase.GetKKPillar_ProductionDown_Details(fromDate, toDate, PlantID, machineId, ShiftID);
                DataTable Downcodelst = TMPTrakDataBase.GetKKPillar_Production_DownCodeList();
                int DownCode_count = Downcodelst.AsEnumerable().Select(x => x.Field<string>("downid")).Distinct().Count();
                if (Downcodelst.Rows.Count > 0 && Downcodelst != null)
                {
                    int col = 13;
                    foreach (DataRow item in Downcodelst.Rows)
                    {
                        wsDts.Cells[4, col].Value = item["downid"].ToString();
                        col++;
                        if (col.Equals(20) || col.Equals(48) || col.Equals(54) || col.Equals(59) || col.Equals(71) || col.Equals(74) || col.Equals(79))
                        {
                            col++;
                        }
                        else if (col.Equals(25))
                        {
                            col = col + 6;
                        }
                        else if (col.Equals(41))
                        {
                            col = col + 2;
                        }
                    }
                }
                if (dt.Rows.Count > 0 && dt != null)
                {

                    foreach (DataRow item in dt.Rows)
                    {
                        wsDts.Cells[row, 1].Value = item["Division"];
                        wsDts.Cells[row, 2].Value = Util.GetDateTime(item["Ddate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        wsDts.Cells[row, 3].Value = item["Shift"];
                        wsDts.Cells[row, 4].Value = item["MachineID"];
                        wsDts.Cells[row, 5].Value = item["CircleNo"];
                        wsDts.Cells[row, 6].Value = item["BatchNo"];
                        wsDts.Cells[row, 7].Value = item["Operator"];
                        wsDts.Cells[row, 8].Value = item["FCode"];
                        wsDts.Cells[row, 9].Value = item["Operation"];
                        wsDts.Cells[row, 10].Value = Convert.ToDouble(item["AvgCycleTime"]);
                        wsDts.Cells[row, 11].Value = Convert.ToDouble(item["UtilisedTime"]);
                        wsDts.Cells[row, 12].Value = Convert.ToDouble(item["ActualCount"]);

                        wsDts.Cells[row, 13].Value = Convert.ToDouble(item["A"]);
                        wsDts.Cells[row, 14].Value = Convert.ToDouble(item["B"]);
                        wsDts.Cells[row, 15].Value = Convert.ToDouble(item["C"]);
                        wsDts.Cells[row, 16].Value = Convert.ToDouble(item["D"]);
                        wsDts.Cells[row, 17].Value = Convert.ToDouble(item["E"]);
                        wsDts.Cells[row, 18].Value = Convert.ToDouble(item["F"]);
                        wsDts.Cells[row, 19].Value = Convert.ToDouble(item["G"]);
                        wsDts.Cells[row, 20].Formula = "=SUM(N" + row + ":S" + row + ")";
                        wsDts.Cells[row, 20].Style.Numberformat.Format = "00.00";

                        wsDts.Cells[row, 21].Value = Convert.ToDouble(item["H"]);
                        wsDts.Cells[row, 22].Value = Convert.ToDouble(item["I"]);
                        wsDts.Cells[row, 23].Value = Convert.ToDouble(item["J"]);
                        wsDts.Cells[row, 24].Value = Convert.ToDouble(item["K"]);
                        wsDts.Cells[row, 30].Formula = "=SUM(U" + row + ":AC" + row + ")";
                        wsDts.Cells[row, 30].Style.Numberformat.Format = "00.00";

                        wsDts.Cells[row, 31].Value = Convert.ToDouble(item["L"]);
                        wsDts.Cells[row, 32].Value = Convert.ToDouble(item["M"]);
                        wsDts.Cells[row, 33].Value = Convert.ToDouble(item["N"]);
                        wsDts.Cells[row, 34].Value = Convert.ToDouble(item["O"]);
                        wsDts.Cells[row, 35].Value = Convert.ToDouble(item["P"]);
                        wsDts.Cells[row, 36].Value = Convert.ToDouble(item["Q"]);
                        wsDts.Cells[row, 37].Value = Convert.ToDouble(item["R"]);
                        wsDts.Cells[row, 38].Value = Convert.ToDouble(item["S"]);
                        wsDts.Cells[row, 39].Value = Convert.ToDouble(item["T"]);
                        wsDts.Cells[row, 40].Value = Convert.ToDouble(item["U"]);
                        wsDts.Cells[row, 41].Formula = "=SUM(AF" + row + ":AN" + row + ")";
                        wsDts.Cells[row, 41].Style.Numberformat.Format = "00.00";


                        wsDts.Cells[row, 43].Value = Convert.ToDouble(item["V"]);
                        wsDts.Cells[row, 44].Value = Convert.ToDouble(item["W"]);
                        wsDts.Cells[row, 45].Value = Convert.ToDouble(item["X"]);
                        wsDts.Cells[row, 46].Value = Convert.ToDouble(item["Y"]);
                        wsDts.Cells[row, 47].Value = Convert.ToDouble(item["Z"]);
                        wsDts.Cells[row, 48].Formula = "=SUM(AQ" + row + ":AU" + row + ")";
                        wsDts.Cells[row, 48].Style.Numberformat.Format = "00.00";

                        wsDts.Cells[row, 49].Value = Convert.ToDouble(item["AA"]);
                        wsDts.Cells[row, 50].Value = Convert.ToDouble(item["AB"]);
                        wsDts.Cells[row, 51].Value = Convert.ToDouble(item["AC"]);
                        wsDts.Cells[row, 52].Value = Convert.ToDouble(item["AD"]);
                        wsDts.Cells[row, 53].Value = Convert.ToDouble(item["AE"]);
                        wsDts.Cells[row, 54].Formula = "=SUM(AW" + row + ":BA" + row + ")";
                        wsDts.Cells[row, 54].Style.Numberformat.Format = "00.00";

                        wsDts.Cells[row, 55].Value = Convert.ToDouble(item["AF"]);
                        wsDts.Cells[row, 56].Value = Convert.ToDouble(item["AG"]);
                        wsDts.Cells[row, 57].Value = Convert.ToDouble(item["AH"]);
                        wsDts.Cells[row, 58].Value = Convert.ToDouble(item["AI"]);


                        wsDts.Cells[row, 60].Value = Convert.ToDouble(item["AJ"]);
                        wsDts.Cells[row, 61].Value = Convert.ToDouble(item["AK"]);
                        wsDts.Cells[row, 62].Value = Convert.ToDouble(item["AL"]);
                        wsDts.Cells[row, 63].Value = Convert.ToDouble(item["AM"]);
                        wsDts.Cells[row, 64].Value = Convert.ToDouble(item["AN"]);
                        wsDts.Cells[row, 65].Value = Convert.ToDouble(item["AO"]);
                        wsDts.Cells[row, 66].Value = Convert.ToDouble(item["AP"]);
                        wsDts.Cells[row, 67].Value = Convert.ToDouble(item["AQ"]);
                        wsDts.Cells[row, 68].Value = Convert.ToDouble(item["AR"]);
                        wsDts.Cells[row, 69].Value = Convert.ToDouble(item["AS"]);
                        wsDts.Cells[row, 70].Value = Convert.ToDouble(item["AT"]);

                        wsDts.Cells[row, 72].Value = Convert.ToDouble(item["AU"]);
                        wsDts.Cells[row, 73].Value = Convert.ToDouble(item["AV"]);
                        wsDts.Cells[row, 74].Formula = "=SUM(BC" + row + ":BU" + row + ")";
                        wsDts.Cells[row, 74].Style.Numberformat.Format = "00.00";

                        wsDts.Cells[row, 75].Value = Convert.ToDouble(item["AW"]);
                        wsDts.Cells[row, 76].Value = Convert.ToDouble(item["AX"]);
                        wsDts.Cells[row, 77].Value = Convert.ToDouble(item["AY"]);
                        wsDts.Cells[row, 78].Value = Convert.ToDouble(item["AZ"]);
                        wsDts.Cells[row, 79].Formula = "=SUM(BY" + row + ":BZ" + row + ")";
                        wsDts.Cells[row, 79].Style.Numberformat.Format = "00.00";
                        wsDts.Cells[row, 80].Value = Convert.ToDouble(item["B1"]);
                        wsDts.Cells[row, 81].Value = Convert.ToDouble(item["B2"]);
                        //wsDts.Cells[row, columns].Formula = "=IF(H"+row+"= ' ', '', IF(C"+row+ "='FIRST', 510, IF(C"+row+ " = 'SECOND', 495, IF(C"+row+" = 'C', 435,))))";
                        //if(item["Shift"].ToString() == "FIRST")
                        //{
                        //    wsDts.Cells[row, columns].Value = Convert.ToDouble("510");
                        //}
                        //else if(item["Shift"].ToString() == "SECOND")
                        //{
                        //    wsDts.Cells[row, columns].Value = Convert.ToDouble("495");
                        //}
                        //else
                        //{
                        //    wsDts.Cells[row, columns].Value = Convert.ToDouble("435");
                        //}
                        //wsDts.Cells[row, columns].Formula = "=(AX" + row + "-AC" + row + ")";
                        //columns++;
                        //wsDts.Cells[row, columns].Formula = "=SUM(AW" + row + "+AV" + row + "+AU" + row + "+AQ" + row + "+AP" + row + "+U" + row + "+R" + row + "+P" + row + "+Q" + row + "+M" + row + ")";
                        //columns++;
                        //wsDts.Cells[row, columns].Formula = "=(AY" + row + "-AZ" + row + ")";
                        //columns++;
                        //wsDts.Cells[row, columns].Formula = "=IFERROR(S" + row + "+T" + row + ","+""+")";
                        //columns++;
                        //wsDts.Cells[row, columns].Formula = "=IFERROR(BA" + row + "/J" + row + ","+""+")";
                        //columns++;
                        //wsDts.Cells[row, columns].Formula = "=IF(AY"+row+"=0,"+""+",(BA" + row + "/AY" + row + "))";
                        //wsDts.Cells[row, columns].Style.Numberformat.Format = "0.0%";
                        //columns++;
                        wsDts.Cells[row, 89].Formula = "=IFERROR((L" + row + "/(CG" + row + "/J" + row + ")),0)";
                        wsDts.Cells[row, 89].Style.Numberformat.Format = "0.00";
                        wsDts.Cells[row, 90].Formula = "=IF(L" + row + "=0,1,((L" + row + "-CN" + row + ")/L" + row + "))";
                        wsDts.Cells[row, 90].Style.Numberformat.Format = "0.00";
                        wsDts.Cells[row, 91].Formula = "=(CJ" + row + "*CK" + row + "*CL" + row + ")";
                        wsDts.Cells[row, 91].Style.Numberformat.Format = "0.00";
                        wsDts.Cells[row, 92].Value = getdouble(item["RejCount"].ToString());
                        wsDts.Cells[row, 93].Value = getdouble(item["PartCount"].ToString());
                        row++;
                    }
                    // wsDts.Cells["AX3"].Formula = "=SUBTOTAL(9,L5:L" + (row - 1) + ")";
                    wsDts.Cells["CD3"].Formula = "=SUBTOTAL(9,CD5:CD" + (row - 1) + ")";
                    wsDts.Cells["CE3"].Formula = "=SUBTOTAL(9,CE5:CE" + (row - 1) + ")";
                    wsDts.Cells["CF3"].Formula = "=SUBTOTAL(9,CF5:CF" + (row - 1) + ")";
                    wsDts.Cells["CG3"].Formula = "=SUBTOTAL(9,CG5:CG" + (row - 1) + ")";
                    wsDts.Cells["CH3"].Formula = "=SUBTOTAL(9,CH5:CH" + (row - 1) + ")";
                    wsDts.Cells["CI3"].Formula = "=SUBTOTAL(9,CI5:CI" + (row - 1) + ")";
                    wsDts.Cells["L3"].Formula = "=SUBTOTAL(9,L5:L" + (row - 1) + ")";
                    wsDts.Cells["CN3"].Formula = "=SUBTOTAL(9,CN5:CN" + (row - 1) + ")";

                    int lastrow = row - 1;
                    // Apply a striping effect to all rows by alternating white & gray background color
                    for (var r = 5; r <= lastrow; r++)
                    {
                        if (r % 2 != 0)
                        {
                            wsDts.Cells[r, 1, r, 93].Style.Fill.PatternType = OfficeOpenXml2.Style.ExcelFillStyle.Solid;
                            wsDts.Cells[r, 1, r, 93].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 220, 230, 241));
                        }
                        else
                        {
                            wsDts.Cells[r, 1, r, 93].Style.Fill.PatternType = OfficeOpenXml2.Style.ExcelFillStyle.Solid;
                            wsDts.Cells[r, 1, r, 93].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 255, 255, 255));
                        }

                    }
                    wsDts.Cells[5, 1, lastrow, 93].Style.Font.Color.SetColor(Color.Black);

                    modelRange = wsDts.Cells[5, 1, lastrow, 93].ToString();
                    var modelTable1 = wsDts.Cells[modelRange];
                    // Assign borders
                    modelTable1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    modelTable1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    modelTable1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    modelTable1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    DownloadMultipleFile(dst, pck.GetAsByteArray());
                    Logger.WriteDebugLog("KKPillar report generated sucessfully.");
                    Generated = "Generated";
                }
                else
                    Generated = "Nodatafound";



            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return Generated;
        }
        #endregion

        #region ---- Utilised and Downtime Report ----------
        public static string UtilisedAndDowntimeReportAgg(string plant, string cell, string machine, string fromDate, string toDate, string shift, string type, string year, string month, DataTable dt)
        {
            string Generated = "";
            try
            {
                string proc = "", reportName = "", strtTime = "", endTime = "";
                reportName = "UtilisedAndDowntimeReport.xlsx";

                string src, dst = string.Empty;
                src = Util.GetReportPath(reportName);
                string tempfileName = "UtilisedAndDowntimeReport" + "_" + Guid.NewGuid() + ".xlsx";
                dst = Path.Combine(appPath, "Temp", SafeFileName(tempfileName));
                if (!File.Exists(src))
                {
                    Logger.WriteDebugLog("Utilised And Downtime Report Template Not Found at the Path - \n " + src);
                }

                FileInfo newFile = new FileInfo(src);
                ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                ExcelWorksheet ws = excelPackage.Workbook.Worksheets[0];
                ws.Cells["B3"].Value = plant;
                ws.Cells["D3"].Value = cell;
                if (type.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                {
                    ws.Cells["A4"].Value = "Date";
                    ws.Cells["B4"].Value = fromDate;
                    ws.Cells["C4"].Value = "Shift";
                    ws.Cells["D4"].Value = shift;
                }
                else if (type.Equals("Month", StringComparison.OrdinalIgnoreCase))
                {
                    ws.Cells["A4"].Value = "Year";
                    ws.Cells["B4"].Value = year;
                    ws.Cells["C4"].Value = "Month";
                    ws.Cells["D4"].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(month));
                }
                else
                {
                    ws.Cells["A4"].Value = "From";
                    ws.Cells["B4"].Value = fromDate;
                    ws.Cells["C4"].Value = "To";
                    ws.Cells["D4"].Value = toDate;
                }

                int rowCount = 7, colCount = 1;
                ws.Cells[rowCount, 1].Value = "Machine ID";
                ws.Cells[rowCount, 2].Value = "Utilised Time";
                ws.Cells[rowCount, 3].Value = "Down Time";
                ws.Cells[rowCount, 4].Value = "Management Loss";
                ws.Cells[rowCount, 5].Value = "Total";
                rowCount = 8;
                int startRowCount = rowCount;
                foreach (DataRow row in dt.Rows)
                {
                    colCount = 1;
                    ws.Cells[rowCount, 1].Value = row["MachineID"].ToString();
                    setTimeSpanFormat("hh:mm:ss", ws, rowCount, 2, row["UtilisedTime"].ToString());
                    setTimeSpanFormat("hh:mm:ss", ws, rowCount, 3, row["DownTime"].ToString());
                    setTimeSpanFormat("hh:mm:ss", ws, rowCount, 4, row["MgmtLoss"].ToString());
                    setTimeSpanFormat("hh:mm:ss", ws, rowCount, 5, row["TotalTime"].ToString());
                    rowCount++;
                }
                ws.Cells[startRowCount - 1, 1, rowCount, 5].Style.Font.Color.SetColor(Color.White);
                ws.Cells[2, 1, rowCount, 6].AutoFitColumns();

                rowCount--;
                var chart = ws.Drawings.AddChart("TimeChart", eChartType.ColumnStacked);
                chart.SetSize(1800, 700);
                chart.SetPosition(5, 5, 0, 5);
                var chartSeries = chart.Series.Add(ws.Cells[startRowCount, 2, rowCount, 2], ws.Cells[startRowCount, 1, rowCount, 1]);
                setUtilisedAndDowntimeReportChartSeries(chartSeries, "Utilised Time", Color.FromArgb(77, 255, 77));

                chartSeries = chart.Series.Add(ws.Cells[startRowCount, 3, rowCount, 3], ws.Cells[startRowCount, 1, rowCount, 1]);
                setUtilisedAndDowntimeReportChartSeries(chartSeries, "Down Time", Color.FromArgb(255, 82, 82));

                chartSeries = chart.Series.Add(ws.Cells[startRowCount, 4, rowCount, 4], ws.Cells[startRowCount, 1, rowCount, 1]);
                setUtilisedAndDowntimeReportChartSeries(chartSeries, "Management Loss", Color.FromArgb(255, 255, 0));

                ExcelLineChart lineChart = (ExcelLineChart)chart.PlotArea.ChartTypes.Add(eChartType.Line);
                ExcelLineChartSerie lineSeries = lineChart.Series.Add(ExcelRange.GetAddress(startRowCount, 5, rowCount, 5), ExcelRange.GetAddress(startRowCount, 1, rowCount, 1)) as ExcelLineChartSerie;
                lineSeries.Header = "Total";
                lineChart.UseSecondaryAxis = false;
                lineSeries.LineWidth = 0;
                lineSeries.DataLabel.ShowValue = true;
                lineSeries.DataLabel.Position = eLabelPosition.Top;

                chart.YAxis.Format = "[h]:mm:ss;@";
                chart.Title.Text = "Utilised and Down Time Chart";
                chart.YAxis.Title.Text = "Time (hh:mm:ss)";
                chart.XAxis.Title.Text = "Machine ID";
                DownloadMultipleFile(dst, excelPackage.GetAsByteArray());
                Generated = "Generated";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return Generated;
        }
        private static void setUtilisedAndDowntimeReportChartSeries(ExcelChartSerie chartSerie, string seriesName, Color color)
        {
            try
            {
                var chartBarSerie = chartSerie as ExcelBarChartSerie;
                chartBarSerie.Fill.Color = color;
                chartBarSerie.DataLabel.ShowValue = true;

                //chartBarSerie.DataLabel.Position = eLabelPosition.OutEnd;
                chartBarSerie.DataLabel.Font.Size = 11;

                chartBarSerie.Header = seriesName;
                //chartDataLabelVerticle270(chartSerie as ExcelBarChartSerie, 0);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }

        }
        #endregion

        #region ---- ShiftwiseOperatorPerformance Report----
        internal static string ShiftwiseOperatorPerformanceReport(DateTime fromDate, DateTime toDate, string PlantID, string ShiftID, string machineId, string Operator)
        {
            string Generated = "";
            try
            {
                DataTable dt = new DataTable();
                string Source = string.Empty, destination = string.Empty, template = string.Empty;
                string reportTemp = TMPTrakDataBase.ReportParameter_OPE();
                string ReportName = "";
                if (reportTemp.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ReportName = "SM_ShiftwiseOperatorPerformanceReport_OPE.xlsx";
                }
                else
                {
                    ReportName = "SM_ShiftwiseOperatorPerformanceReport.xlsx";
                }
                Source = Util.GetReportPath(ReportName);
                template = "SM_ShiftwiseOperatorPerformanceReport_" + DateTime.Now + ".xlsx";
                destination = Path.Combine(appPath, "Temp", SafeFileName(template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("SM_ShiftwiseOperatorPerformanceReport.xlsx - \n " + Source);
                }
                FileInfo newFile = new FileInfo(Source);
                ExcelPackage Excel = new ExcelPackage(newFile, true);
                var ws = Excel.Workbook.Worksheets[0];

                dt = TMPTrakDataBase.GetShiftwiseOperatorPerformanceData(fromDate, toDate, ShiftID, PlantID, machineId, Operator);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (reportTemp.Equals("Y", StringComparison.OrdinalIgnoreCase))
                    {
                        ws.Cells["B2"].Value = fromDate.ToString("dd-MM-yyyy HH:mm:ss");
                        ws.Cells["E2"].Value = toDate.ToString("dd-MM-yyyy HH:mm:ss");
                        ws.Cells["G2"].Value = string.IsNullOrEmpty(PlantID) ? "ALL" : PlantID;
                        //ws.Cells["I2"].Value = machineId;
                        //ws.Cells["L2"].Value = string.IsNullOrEmpty(ShiftID) ? "ALL" : ShiftID;
                        //ws.Cells["N2"].Value = string.IsNullOrEmpty(Operator) ? "ALL" : Operator;

                        string modelRange = string.Empty;
                        int row = 5, col = 1;

                        foreach (DataRow item in dt.Rows)
                        {
                            col = 1;
                            ws.Cells[row, col].Value = Util.GetDateTime(item["Date"].ToString()).ToString("dd-MM-yyyy");
                            col++;
                            ws.Cells[row, col].Value = item["Shift"];
                            col++;
                            ws.Cells[row, col].Value = item["MachineName"];
                            col++;
                            ws.Cells[row, col].Value = item["ProductNo"];
                            col++;
                            ws.Cells[row, col].Value = item["OprnCode"];
                            col++;
                            ws.Cells[row, col].Value = item["OperatorName"];
                            col++;
                            ws.Cells[row, col].Value = item["Target"];
                            col++;
                            ws.Cells[row, col].Value = item["Actual"];
                            col++;
                            ws.Cells[row, col].Value = item["Difference"];
                            col++;
                            ws.Cells[row, col].Value = item["StdCycleTime"];
                            col++;
                            ws.Cells[row, col].Value = item["ActualCycleTime"];
                            col++;
                            ws.Cells[row, col].Value = item["stdloadunload"];
                            col++;
                            ws.Cells[row, col].Value = item["actualloadunload"];
                            col++;
                            ws.Cells[row, col].Value = item["Downtimecode"];
                            col++;
                            setTimeSpanFormat("hh:mm:ss", ws, row, col, item["Utilisedtime"].ToString());
                            col++;
                            setTimeSpanFormat("hh:mm:ss", ws, row, col, item["TotalDown"].ToString());
                            col++;
                            ws.Cells[row, col].Value = item["AE"];
                            col++;
                            ws.Cells[row, col].Value = item["PE"];
                            col++;
                            ws.Cells[row, col].Value = item["OE"];
                            col++;
                            ws.Cells[row, col].Value = item["OPE"];
                            row++;
                        }
                        int lastrow = row - 1;

                        ws.Cells[row, 1].Value = "Total";
                        ws.Cells[row, 1, row, 6].Merge = true;
                        ws.Cells[row, 7].Value = dt.AsEnumerable().Select(x => x.Field<double>("Target")).Sum();
                        ws.Cells[row, 8].Value = dt.AsEnumerable().Select(x => x.Field<double>("Actual")).Sum();
                        ws.Cells[row, 9].Value = dt.AsEnumerable().Select(x => x.Field<double>("Difference")).Sum();
                        ws.Cells[row, 15].Formula = "=Sum(O" + 5 + ":O" + lastrow.ToString() + ")";
                        ws.Cells[row, 15].Style.Numberformat.Format = "[h]:mm:ss";
                        ws.Cells[row, 16].Formula = "=Sum(P" + 5 + ":P" + lastrow.ToString() + ")";
                        ws.Cells[row, 16].Style.Numberformat.Format = "[h]:mm:ss";
                        ws.Cells[row, 17].Value = Math.Round(dt.AsEnumerable().Where(x => x.Field<double>("AE") != 0).Average(x => x.Field<double>("AE")), 2);
                        ws.Cells[row, 18].Value = Math.Round(dt.AsEnumerable().Where(x => x.Field<double>("PE") != 0).Average(x => x.Field<double>("PE")), 2);
                        ws.Cells[row, 19].Value = Math.Round(dt.AsEnumerable().Where(x => x.Field<double>("OE") != 0).Average(x => x.Field<double>("OE")), 2);
                        ws.Cells[row, 20].Value = Math.Round(dt.AsEnumerable().Where(x => x.Field<double>("OPE") != 0).Average(x => x.Field<double>("OPE")), 2);

                        ws.Cells[row, 1, row, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, 1, row, 20].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#F4B084"));
                        ws.Cells[row, 1, row, 20].Style.Font.Bold = true;
                        ws.Cells[row, 1, row, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        modelRange = "A5:T" + (lastrow).ToString();
                        var modelTable = ws.Cells[modelRange];
                        // Assign borders
                        modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //ws.Cells["A5:K" + (lastrow).ToString()].AutoFitColumns();
                        //ws.Cells["P5:R" + (lastrow).ToString()].AutoFitColumns();
                    }
                    else
                    {
                        ws.Cells["B2"].Value = fromDate.ToString("dd-MM-yyyy HH:mm:ss");
                        ws.Cells["E2"].Value = toDate.ToString("dd-MM-yyyy HH:mm:ss");
                        ws.Cells["H2"].Value = string.IsNullOrEmpty(PlantID) ? "ALL" : PlantID;
                        ws.Cells["J2"].Value = machineId;
                        ws.Cells["M2"].Value = string.IsNullOrEmpty(ShiftID) ? "ALL" : ShiftID;
                        ws.Cells["O2"].Value = string.IsNullOrEmpty(Operator) ? "ALL" : Operator;

                        string modelRange = string.Empty;
                        int row = 5, col = 1;

                        foreach (DataRow item in dt.Rows)
                        {
                            col = 1;
                            ws.Cells[row, col].Value = Util.GetDateTime(item["Date"].ToString()).ToString("dd-MM-yyyy");
                            col++;
                            ws.Cells[row, col].Value = item["Shift"];
                            col++;
                            ws.Cells[row, col].Value = item["MachineName"];
                            col++;
                            ws.Cells[row, col].Value = item["ProductNo"];
                            col++;
                            ws.Cells[row, col].Value = item["ProductName"];
                            col++;
                            ws.Cells[row, col].Value = item["OprnCode"];
                            col++;
                            ws.Cells[row, col].Value = item["OpnDescription"];
                            col++;
                            ws.Cells[row, col].Value = item["OperatorName"];
                            col++;
                            ws.Cells[row, col].Value = item["Target"];
                            col++;
                            ws.Cells[row, col].Value = item["Actual"];
                            col++;
                            ws.Cells[row, col].Value = item["Difference"];
                            col++;
                            ws.Cells[row, col].Value = item["StdCycleTime"];
                            col++;
                            ws.Cells[row, col].Value = item["ActualCycleTime"];
                            col++;
                            ws.Cells[row, col].Value = item["Downtimecode"];
                            col++;
                            ws.Cells[row, col].Value = item["DowntimeDescription"];
                            col++;
                            setTimeSpanFormat("hh:mm:ss", ws, row, col, item["Utilisedtime"].ToString());
                            col++;
                            setTimeSpanFormat("hh:mm:ss", ws, row, col, item["TotalDown"].ToString());
                            col++;
                            ws.Cells[row, col].Value = item["AE"];
                            col++;
                            ws.Cells[row, col].Value = item["PE"];
                            col++;
                            ws.Cells[row, col].Value = item["OE"];

                            row++;
                        }
                        int lastrow = row - 1;

                        ws.Cells[row, 1].Value = "Total";
                        ws.Cells[row, 1, row, 8].Merge = true;
                        ws.Cells[row, 9].Value = dt.AsEnumerable().Select(x => x.Field<double>("Target")).Sum();
                        ws.Cells[row, 10].Value = dt.AsEnumerable().Select(x => x.Field<double>("Actual")).Sum();
                        ws.Cells[row, 11].Value = dt.AsEnumerable().Select(x => x.Field<double>("Difference")).Sum();
                        ws.Cells[row, 16].Formula = "=Sum(P" + 5 + ":P" + lastrow.ToString() + ")";
                        ws.Cells[row, 16].Style.Numberformat.Format = "[h]:mm:ss";
                        ws.Cells[row, 17].Formula = "=Sum(Q" + 5 + ":Q" + lastrow.ToString() + ")";
                        ws.Cells[row, 17].Style.Numberformat.Format = "[h]:mm:ss";
                        ws.Cells[row, 20].Value = Math.Round(dt.AsEnumerable().Where(x => x.Field<double>("OE") != 0).Average(x => x.Field<double>("OE")), 2);
                        ws.Cells[row, 18].Value = Math.Round(dt.AsEnumerable().Where(x => x.Field<double>("AE") != 0).Average(x => x.Field<double>("AE")), 2);
                        ws.Cells[row, 19].Value = Math.Round(dt.AsEnumerable().Where(x => x.Field<double>("PE") != 0).Average(x => x.Field<double>("PE")), 2);

                        ws.Cells[row, 1, row, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, 1, row, 20].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#F4B084"));
                        ws.Cells[row, 1, row, 20].Style.Font.Bold = true;
                        ws.Cells[row, 1, row, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        modelRange = "A5:T" + (lastrow).ToString();
                        var modelTable = ws.Cells[modelRange];
                        // Assign borders
                        modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A5:M" + (lastrow).ToString()].AutoFitColumns();
                        ws.Cells["P5:T" + (lastrow).ToString()].AutoFitColumns();
                    }
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    Logger.WriteDebugLog("Shiftwise Operator Performance report generated sucessfully.");
                    Generated = "Generated";

                }
                else
                    Generated = "Nodatafound";
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog(ex.Message);
            }
            return Generated;
        }

        #endregion

        #region --- Hydrotest Report-------------
        internal static string HydroTestReport(DateTime fromDate, DateTime toDate, string process, string machineId)
        {
            string Generated = "";
            try
            {
                DataTable dt = new DataTable();
                string Source = string.Empty, destination = string.Empty, template = string.Empty;
                string ReportName = "HydroTestReport.xlsx";
                Source = Util.GetReportPath(ReportName);
                template = "HydroTestReport_" + DateTime.Now + ".xlsx";
                destination = Path.Combine(appPath, "Temp", SafeFileName(template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("HydroTestReport.xlsx - \n " + Source);
                }
                FileInfo newFile = new FileInfo(Source);
                ExcelPackage Excel = new ExcelPackage(newFile, true);
                var ws = Excel.Workbook.Worksheets[0];

                dt = TMPTrakDataBase.GetHydroTestData(fromDate, toDate, process, machineId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ws.Cells["A3"].Value = fromDate.ToString("dd-MM-yyyy HH:mm:ss");
                    ws.Cells["C3"].Value = toDate.ToString("dd-MM-yyyy HH:mm:ss");
                    ws.Cells["E3"].Value = process;
                    ws.Cells["F3"].Value = machineId;

                    string modelRange = string.Empty;
                    int row = 7, colCount = 1, headercol = 8;
                    int colno = headercol;

                    var headerevent = dt.AsEnumerable().Select(x => new { EventID = x.Field<string>("EventID"), EventDesc = x.Field<string>("EventDescription") }).Distinct().ToList();

                    var distList = dt.AsEnumerable().Select(x => new { Date = x.Field<dynamic>("CycleStart"), Machine = x.Field<string>("MachineID"), Component = x.Field<string>("ComponentID"), Operation = x.Field<string>("OperationNo") }).Distinct().ToList();

                    for (int i = 0; i < headerevent.Count; i++)
                    {
                        ws.InsertColumn(headercol, 1, headercol);
                        ws.Cells[5, headercol, 6, headercol].Merge = true;
                        ws.Cells[5, headercol++].Value = headerevent[i].EventDesc;
                    }
                    ws.DeleteColumn(headercol);

                    headercol = headercol + 3;
                    if (process.Equals("Casing", StringComparison.OrdinalIgnoreCase))
                    {
                        ws.InsertColumn(headercol, 1, headercol);
                        ws.Cells[5, headercol, 6, headercol].Merge = true;
                        ws.Cells[5, headercol, 6, headercol].Value = "Holding Time";
                        headercol++;
                        ws.DeleteColumn(headercol);
                    }
                    else
                    {
                        ws.InsertColumn(headercol, 2, headercol);
                        ws.Cells[5, headercol, 6, headercol].Merge = true;
                        ws.Cells[5, headercol, 6, headercol].Value = "Body Test Holding Time";
                        headercol++;
                        ws.Cells[5, headercol, 6, headercol].Merge = true;
                        ws.Cells[5, headercol, 6, headercol].Value = "Shut off Test Holding Time";
                        headercol++;
                        ws.DeleteColumn(headercol);
                    }

                    for (int distC = 0; distC < distList.Count; distC++)
                    {
                        colCount = 1;

                        DataTable dataRows = dt.AsEnumerable().Where(x => x.Field<string>("MachineID") == distList[distC].Machine && x.Field<string>("ComponentID") == distList[distC].Component && x.Field<string>("OperationNo") == distList[distC].Operation && x.Field<dynamic>("CycleStart") == distList[distC].Date).CopyToDataTable();
                        var firstRow = dataRows.AsEnumerable().FirstOrDefault();

                        ws.Cells[row, colCount].Value = Util.GetDateTime(firstRow["CycleStart"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        colCount++;
                        ws.Cells[row, colCount].Value = firstRow["MachineID"].ToString();
                        colCount++;
                        ws.Cells[row, colCount].Value = firstRow["MachineDescription"].ToString();
                        colCount++;
                        ws.Cells[row, colCount].Value = firstRow["ComponentID"].ToString();
                        colCount++;
                        ws.Cells[row, colCount].Value = firstRow["ComponentDescription"].ToString();
                        colCount++;
                        ws.Cells[row, colCount].Value = firstRow["OperationNo"].ToString();
                        colCount++;
                        ws.Cells[row, colCount].Value = firstRow["EmployeeID"].ToString();
                        colCount++;

                        for (int evval = 0; evval < headerevent.Count; evval++)
                        {
                            ws.Cells[row, colCount].Value = dataRows.AsEnumerable().Where(x => x.Field<string>("EventDescription").Equals(headerevent[evval].EventDesc)).Select(x => x.Field<string>("EventVal")).FirstOrDefault();
                            colCount++;
                        }
                        setTimeSpanFormat("hh:mm:ss", ws, row, colCount, firstRow["StdCycleTime"].ToString());
                        colCount++;
                        setTimeSpanFormat("hh:mm:ss", ws, row, colCount, firstRow["ActCycleTime"].ToString());
                        colCount++;
                        ws.Cells[row, colCount].Value = Convert.ToDouble(firstRow["Efficiency"].ToString());
                        colCount++;
                        setTimeSpanFormat("hh:mm:ss", ws, row, colCount, firstRow["HoldingTime1"].ToString());
                        colCount++;
                        if (process.Equals("MPV", StringComparison.OrdinalIgnoreCase))
                        {
                            setTimeSpanFormat("hh:mm:ss", ws, row, colCount, firstRow["HoldingTime2"].ToString());
                            colCount++;
                        }
                        ws.Cells[row, colCount].Value = firstRow["Result"].ToString();
                        row++;
                    }

                    int lastrow = row - 1;
                    var modelTable = ws.Cells[7, 1, lastrow, colCount];
                    // Assign borders
                    modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[7, 1, lastrow, colCount].AutoFitColumns();


                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    Logger.WriteDebugLog("Shiftwise Operator Performance report generated sucessfully.");
                    Generated = "Generated";

                }
                else
                    Generated = "Nodatafound";
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog(ex.Message);
            }
            return Generated;
        }
        #endregion

        #region -----  Shiftwise DownTime Details AutoTech
        public static string ShiftwiseDownTimeDetailsAutoTech(DateTime StartDate, DateTime EndDate, string PlantId, string cellIDs, string machineIDs)
        {
            string Generated = "";
            try
            {
                string reportName = "", strtTime = "", endTime = "";
                reportName = "ShiftwiseDownTimeDetails_AutoTech.xlsx";
                strtTime = StartDate.ToString("dd-MMM-yyyy");
                endTime = EndDate.ToString("dd-MMM-yyyy");
                DataTable dt = TMPTrakDataBase.GetShiftwiseDownTimeDetailsAutoTechData(StartDate, EndDate, PlantId, cellIDs, machineIDs);
                string src, dst = string.Empty;
                src = Util.GetReportPath(reportName);
                string tempfileName = "ShiftwiseDownTimeDetails_" + DateTime.Now + ".xlsx";
                dst = Path.Combine(appPath, "Temp", SafeFileName(tempfileName));

                if (!File.Exists(src))
                {
                    Logger.WriteDebugLog("ShiftwiseDownTimeDetails_AutoTech Report Template Not Found at the Path - \n " + src);
                }

                FileInfo newFile = new FileInfo(src);
                ExcelPackage excelPackage = new ExcelPackage(newFile, true);

                if (dt != null && dt.Rows.Count > 0)
                {

                    var distMachineID = dt.AsEnumerable().Select(k => k["MachineID"].ToString()).Distinct();


                    var firstWsDts = excelPackage.Workbook.Worksheets[0];
                    firstWsDts.Cells["B5"].Value = strtTime;
                    firstWsDts.Cells["E5"].Value = endTime;
                    firstWsDts.Cells["H5"].Value = PlantId;

                    foreach (string machine in distMachineID)
                    {
                        int rowCount = 6;
                        int startRowCount = rowCount;
                        int cellCount = 1;
                        int startCellCount = cellCount;
                        var worksheet = excelPackage.Workbook.Worksheets.Add(machine, firstWsDts);

                        DataTable dtMachine = dt.AsEnumerable().Where(k => k["MachineID"].ToString() == machine).CopyToDataTable();
                        var distDate = dtMachine.AsEnumerable().Select(k => k.Field<DateTime>("PDate")).Distinct().ToList();
                        var distShift = dtMachine.AsEnumerable().Select(k => k["ShiftName"].ToString()).Distinct().ToList();
                        var distDownID = dtMachine.AsEnumerable().Select(k => k["DownID"].ToString()).Distinct().ToList();
                        //int lastColCount = 
                        cellCount = 2;
                        //for (int i = 0; i < distDate.Count(); i++)
                        //{
                        //    startCellCount = cellCount;
                        //    worksheet.Cells[7, cellCount].Value = distDate[i].ToString("dd-MM-yyyy");
                        //    for (int j = 0; j < distShift.Count; j++)
                        //    {
                        //        worksheet.Cells[8, cellCount].Value = distShift[j];
                        //        cellCount++;
                        //    }
                        //    worksheet.Cells[7, cellCount, 7, cellCount - 1].Merge = true;
                        //}
                        rowCount = startRowCount = 10;
                        for (int downC = 0; downC < distDownID.Count; downC++)
                        {
                            cellCount = 2;
                            worksheet.Cells[rowCount, 1].Value = distDownID[downC];
                            for (int dateC = 0; dateC < distDate.Count(); dateC++)
                            {
                                DateTime pDate = distDate[dateC];
                                if (downC == 0)
                                {
                                    startCellCount = cellCount;
                                    worksheet.Cells[7, cellCount].Value = pDate.ToString("dd-MM-yyyy");
                                    worksheet.Cells[7, cellCount, 7, cellCount + distShift.Count() - 1].Merge = true;
                                }
                                for (int j = 0; j < distShift.Count; j++)
                                {
                                    if (downC == 0)
                                    {
                                        worksheet.Cells[8, cellCount].Value = distShift[j];
                                        worksheet.Cells[9, cellCount].Value = dtMachine.AsEnumerable().Where(k => k.Field<DateTime>("PDate") == pDate && k["ShiftName"].ToString() == distShift[j]).Select(k => k["ShiftTimeInMin"].ToString()).FirstOrDefault();
                                    }
                                    string shiftValue = dtMachine.AsEnumerable().Where(k => k.Field<DateTime>("PDate") == pDate && k["ShiftName"].ToString() == distShift[j] && k["DownID"].ToString() == distDownID[downC]).Select(k => k["DownTime"].ToString()).FirstOrDefault();
                                    if (!string.IsNullOrEmpty(shiftValue))
                                    {
                                        setTimeSpanFormat("hh:mm:ss", worksheet, rowCount, cellCount, shiftValue);
                                    }
                                    cellCount++;
                                }
                            }
                            rowCount++;
                        }
                        worksheet.Cells[9, 1, rowCount, cellCount].AutoFitColumns();
                        setThinBorder(worksheet, 7, 1, rowCount - 1, cellCount - 1);
                    }
                    excelPackage.Workbook.Worksheets.Delete(0);
                    DownloadMultipleFile(dst, excelPackage.GetAsByteArray());
                    Generated = "Generated";
                }
                else
                    Generated = "nodatafound";

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return Generated;
        }
        #endregion

        #region  -- GK Incentive Report---
        internal static string GenerateOperatorIncentiveReportGK(DateTime fromDate, DateTime toDate, string Operator)
        {
            string Generated = "";
            try
            {
                DataTable dt = TMPTrakDataBase.GetIncentiveReportGK(fromDate, toDate, Operator);
                string Source = string.Empty, Destination = string.Empty, Template = string.Empty;
                string ReportName = "OperatorIncentiveReport_GK.xlsx";
                Source = Util.GetReportPath(ReportName);
                Template = "IncentiveReport_GK_" + DateTime.Now + ".xlsx";
                Destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("IncentiveReportGK Template Not Found at the Path - \n " + Source);
                }
                FileInfo newFile = new FileInfo(Source);
                ExcelPackage excel = new ExcelPackage(newFile, true);
                ExcelWorksheet ws = excel.Workbook.Worksheets[0];
                ws.Cells["B2"].Value = fromDate.ToString("dd-MM-yyyy");
                ws.Cells["D2"].Value = toDate.ToString("dd-MM-yyyy");
                int col = 1, row = 4;
                if (dt.Rows.Count > 0 && dt != null)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        col = 1;
                        ws.Cells[row, col].Value = item["slno"];
                        col++;
                        ws.Cells[row, col].Value = item["OperatorName"];
                        col++;
                        ws.Cells[row, col].Value = item["AverageEfficiency"];
                        col++;
                        ws.Cells[row, col].Value = item["Absent"];
                        col++;
                        ws.Cells[row, col].Value = item["RejectionPercentage"];
                        col++;
                        ws.Cells[row, col].Value = item["RejectionReason"];
                        col++;
                        ws.Cells[row, col].Value = item["ProductionIncentive"];
                        row++;
                    }
                    int lastrow = row - 1;
                    var modelTable = ws.Cells[4, 1, lastrow, col];
                    // Assign borders
                    modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //modelTable.AutoFitColumns();


                    DownloadMultipleFile(Destination, excel.GetAsByteArray());
                    Logger.WriteDebugLog("Incentive report generated sucessfully.");
                    Generated = "Generated";
                }
                else
                    Generated = "Nodatafound";

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return Generated;
        }

        #endregion
    }
}