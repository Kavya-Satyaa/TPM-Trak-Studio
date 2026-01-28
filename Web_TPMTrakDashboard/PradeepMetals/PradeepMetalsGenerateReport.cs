using OfficeOpenXml2;
using OfficeOpenXml2.Drawing.Chart;
using OfficeOpenXml2.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.PradeepMetals
{
    public class PradeepMetalsGenerateReport
    {
        static readonly string appPath = HttpContext.Current.Server.MapPath("~/PradeepMetals/Reports");
        static string appPathForReportOutput = HttpContext.Current.Server.MapPath("~/PradeepMetals/Reports/ReportsOutput/");
        static string reportName = "";
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
                src = Path.Combine(appPath, "Templates", reportName);
            else
            {
                if (HttpContext.Current.Session["Language"].ToString() != "en")
                    src = Path.Combine(appPath, "Templates-" + HttpContext.Current.Session["Language"].ToString() + "", reportName);
                else
                    src = Path.Combine(appPath, "Templates", reportName);
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

        internal static string GenerateMaintenanceMachineLevelDowntimePareto(string machineID, DateTime fromDate, DateTime toDate, string ReportType, string DownCategory)
        {
            string successfull = "";
            try
            {
                string Filename = "MaintenanceMachineLevelDownTimeParetoReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "MaintenanceMachineLevelDownTimeParetoReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("MaintenanceMachineLevelDownTimePareto Report template does not exists at - " + Source);
                    successfull = "TemplateNotFound";
                }
                else
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    int a = 0;
                    if (ReportType.Contains("MaintenanceMachineLevelDowntimePareto"))
                    {

                        Excel.Workbook.Worksheets[a].Name = "Maintenance Machine Level Downtime Pareto Report";
                        var worksheet = Excel.Workbook.Worksheets[a];

                        DataTable dt2 = null;
                        DataTable dt3 = null;
                        DataTable dt4 = null;
                        DataTable dt1 = DBAccessPradeepMetals.MachineLevelDownTimePareto(machineID, fromDate, toDate, DownCategory, out dt2, out dt3, out dt4);
                        DataTable dtMachines = new DataTable();
                        DataTable dtMaintenance = new DataTable();
                        var values = dt1.AsEnumerable().Select(x => x.Field<string>("machineid")).Distinct().ToList();
                        int rowStart = 4;
                        int i = 1, temp_col = 1;
                        temp_col = dt2.Columns.Count - 5;
                        worksheet.Cells[3, 11, 3, 11 + temp_col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[3, 11, 3, 11 + temp_col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(198, 224, 180));
                        worksheet.Cells[2, 11, 2, 11 + temp_col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[2, 11, 2, 11 + temp_col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(112, 173, 71));
                        worksheet.Cells[2, 11].Value = "Top Contributors";
                        worksheet.Cells[2, 11, 2, 11 + temp_col].Merge = true;
                        worksheet.Cells[1, 1, 1, 11 + temp_col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1, 1, 11 + temp_col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));
                        worksheet.Cells[1, 1].Value = "MAINTENANCE MACHINE LEVEL DOWNTIME PARETO";
                        worksheet.Cells[1, 1, 1, 11 + temp_col].Merge = true;

                        for (int j = 0; j < values.Count; j++)
                        {
                            dtMachines = dt1.AsEnumerable().Where(x => x.Field<string>("machineid").Equals(values[j])).CopyToDataTable();
                            foreach (DataRow dtRow in dtMachines.Rows)
                            {

                                dtMaintenance = dt2.AsEnumerable().Where(x => x.Field<string>("machineid").Equals(dtRow["machineid"].ToString())).CopyToDataTable();
                                int col = 11;
                                foreach (DataRow dtrow in dtMaintenance.Rows)
                                {
                                    for (int k = 5; k < dtMaintenance.Columns.Count; k++)
                                    {
                                        if (i == 1)
                                        {
                                            worksheet.Cells[rowStart - 1, col].Value = dtMaintenance.Columns[k].ColumnName.ToString();
                                        }
                                        worksheet.Cells[rowStart, col].Value = string.IsNullOrEmpty(dtrow[k].ToString()) ? 0 : HelperClassGeneric.getDoubleValueFromString(dtrow[k].ToString());
                                        col++;
                                    }
                                }

                                temp_col = col;

                                worksheet.Cells[rowStart, 1].Value = i++;
                                worksheet.Cells[rowStart, 2].Value = dtRow["machineId"].ToString();
                                worksheet.Cells[rowStart, 3].Value = dtRow["description"].ToString();
                                worksheet.Cells[rowStart, 4].Value = dtRow["GroupID"].ToString();
                                worksheet.Cells[rowStart, 5].Value = dtRow["Machinewiseowner"].ToString();
                                worksheet.Cells[rowStart, 6].Value = dtRow["NPD_PROD"].ToString();
                                worksheet.Cells[rowStart, 7].Value = Convert.ToDouble(dtRow["downtime_min"].ToString());
                                worksheet.Cells[rowStart, 8].Value = Convert.ToDouble(dtRow["Cumulative_min"].ToString());
                                worksheet.Cells[rowStart, 9].Value = HelperClassGeneric.getDoubleValueFromString(dtRow["cumulativeper"].ToString()) / 100;
                                worksheet.Cells[rowStart, 9].Style.Numberformat.Format = "#0%";
                                worksheet.Cells[rowStart, 10].Value = HelperClassGeneric.getDoubleValueFromString(dtRow["downtimeper"].ToString());
                                rowStart++;
                            }
                            //rowStart++;
                            if (j == values.Count - 1)
                            {
                                worksheet.Cells[rowStart, 2, rowStart, 16].Style.Font.Bold.ToString();
                                worksheet.Cells[rowStart, 2, rowStart, 16].Style.Font.Bold = true;
                                //worksheet.Cells[rowStart, 2, rowStart, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[rowStart, 2, rowStart, temp_col].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                                worksheet.Cells[rowStart, 2, rowStart, temp_col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[rowStart, 2, rowStart, temp_col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(40, 127, 191));
                                worksheet.Cells[rowStart, 2].Value = "Total";
                                foreach (DataRow dtTotal in dt3.Rows)
                                {
                                    worksheet.Cells[rowStart, 7].Value = HelperClassGeneric.getDoubleValueFromString(dtTotal["totaldowntime_min"].ToString());
                                }
                                int col = 11;
                                for (int k = 0; k < dt4.Rows.Count; k++)
                                {
                                    worksheet.Cells[rowStart, col].Value = HelperClassGeneric.getDoubleValueFromString(dt4.Rows[k]["downidsum_min"].ToString());
                                    col++;
                                }
                                temp_col = col;
                            }
                        }
                        var chart = (ExcelBarChart)worksheet.Drawings.AddChart("paretoChart", eChartType.ColumnClustered);
                        if (dt1.Rows.Count > 15)
                        {
                            chart.SetSize(50 * dt1.Rows.Count - 1, 400);
                        }
                        else
                        {
                            chart.SetSize(1000, 400);
                        }
                        chart.SetPosition(rowStart + 3, 0, 1, 0);
                        ExcelChartSerie barchart = chart.Series.Add(ExcelRange.GetAddress(4, 7, rowStart - 1, 7), ExcelRange.GetAddress(4, 3, rowStart - 1, 3));
                        ExcelLineChart chart2 = (ExcelLineChart)chart.PlotArea.ChartTypes.Add(eChartType.Line);
                        ExcelLineChartSerie linechart = chart2.Series.Add(ExcelRange.GetAddress(4, 9, rowStart - 1, 9), ExcelRange.GetAddress(4, 3, rowStart - 1, 3)) as ExcelLineChartSerie;
                        barchart.Header = "Down Time";
                        chart2.UseSecondaryAxis = true;
                        chart2.YAxis.MaxValue = 1;
                        linechart.Header = "Cumulative";
                        //linechart.DataLabel.Position = eLabelPosition.Top;
                        linechart.DataLabel.ShowValue = false;
                        barchart.Fill.Color = Color.FromArgb(0, 112, 192);
                        //linechart.Marker = eMarkerStyle.Circle;
                        //linechart.MarkerLineColor = Color.FromArgb(237, 125, 49);
                        //linechart.MarkerSize = 5;
                        //linechart.DataLabel.Fill.Color = Color.FromArgb(255, 192, 0);
                        chart.VaryColors = true;
                        chart.YAxis.Title.Text = "DownTime(mins)";
                        chart.Title.Text = "Maintenance - Machinewise [System level] Pareto" + Util.GetDateTime(fromDate.ToString()).ToString("dd.MM.yyyy") + " to " + Util.GetDateTime(toDate.ToString()).ToString("dd.MM.yyyy");
                        chart.DataLabel.ShowValue = true;
                        chart.DataLabel.Format = "00";
                        chart2.DataLabel.ShowValue = true;
                        chart.GapWidth = 3;
                        chart.DataLabel.Position = eLabelPosition.InEnd;
                        chart.XAxis.TextBody.Rotation = (-90);
                        var axis = chart.YAxis;
                        axis.Title.Rotation = (270);

                        worksheet.Cells[2, 2].Value = Util.GetDateTime(fromDate.ToString()).ToString("dd.MM.yyyy") + "" + " to " + "" + Util.GetDateTime(toDate.ToString()).ToString("dd.MM.yyyy");
                        setBorderThin(worksheet, 3, 1, rowStart, temp_col);
                        a++;
                    }
                    //a++;
                    #region -------Sub System Level Maintenance Report-------
                    if (ReportType.Contains("MaintenanceSubSystemLevelDowntimePareto"))
                    {
                        if (a == 0)
                        {
                            Excel.Workbook.Worksheets.Delete(a);
                        }

                        Excel.Workbook.Worksheets.Add("worksheet2");
                        Excel.Workbook.Worksheets[a].Name = "Maintenance SubSystem Level Downtime Pareto Report";
                        var worksheet = Excel.Workbook.Worksheets[a];
                        string WeekStart = "";
                        string WeekEnd = "";
                        //string year1 = Util.GetDateTime(fromDate).ToString("yyyy");
                        //string month1 = Util.GetDateTime(fromDate).ToString("MM");
                        //string year2 = Util.GetDateTime(fromDate).ToString("yyyy");
                        //string month2 = Util.GetDateTime(fromDate).ToString("MM");
                        worksheet.Cells[3, 1].Value = "Week -->";
                        worksheet.Cells[4, 1].Value = "Down ID";
                        worksheet.Cells[4, 2].Value = "Downtime In Mins";
                        worksheet.Cells[4, 3].Value = "Cumulative";
                        worksheet.Cells[4, 4].Value = "Downtime In %";
                        worksheet.Cells[4, 5].Value = "Cumulative %";
                        worksheet.Cells[4, 1, 4, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[4, 1, 4, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                        worksheet.Row(4).Height = Convert.ToDouble(27);
                        for (int b = 1; b < 6; b++)
                        {
                            worksheet.Column(b).Width = Convert.ToDouble(15);
                        }


                        DataTable dt2 = null;
                        DataTable dt3 = null;
                        DataTable dt4 = null;
                        DataTable dt1 = DBAccessPradeepMetals.MaintenanceSubsystemLevelDownTime(machineID, fromDate, toDate, DownCategory, out dt2, out dt3, out dt4);
                        DataTable dtDownIDs = new DataTable();
                        DataTable dtWeeks = new DataTable();


                        if (dt1.Rows.Count > 0)
                        {
                            var len = (dt4.Rows[0][0].ToString()).Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                            WeekStart = (dt4.Rows[0][0].ToString()).Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries)[0];

                            var downIDs = dt1.AsEnumerable().Select(x => x.Field<string>("downid")).Distinct().ToList();
                            int rowstart = 5; int col = 6;
                            for (int i = 0; i < downIDs.Count; i++)
                            {
                                dtDownIDs = dt1.AsEnumerable().Where(x => x.Field<string>("downid").Equals(downIDs[i])).CopyToDataTable();
                                foreach (DataRow dtRow in dtDownIDs.Rows)
                                {
                                    worksheet.Cells[rowstart, 1].Value = dtRow["downid"].ToString();
                                    worksheet.Cells[rowstart, 2].Value = HelperClassGeneric.getDoubleValueFromString(dtRow["downtimetotal_min"].ToString());
                                    worksheet.Cells[rowstart, 3].Value = HelperClassGeneric.getDoubleValueFromString(dtRow["cumulative_min"].ToString());
                                    worksheet.Cells[rowstart, 4].Value = HelperClassGeneric.getDoubleValueFromString(dtRow["downtimepercentage"].ToString());
                                    worksheet.Cells[rowstart, 5].Value = HelperClassGeneric.getDoubleValueFromString(dtRow["cumulativepercentage"].ToString()) / 100;
                                    worksheet.Cells[rowstart, 5].Style.Numberformat.Format = "#0%";
                                    dtWeeks = dt3.AsEnumerable().Where(x => x.Field<string>("downid").Equals(dtRow["downid"].ToString())).CopyToDataTable();
                                    var distWeeks = dt4.AsEnumerable().Select(k => k["weekfinaldata"].ToString()).Distinct().ToList();
                                    foreach (string week in distWeeks)
                                    {
                                        if (i == 0)
                                        {

                                            string input = week;
                                            worksheet.Cells[200, col].Value = input;
                                            string[] parts = input.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                                            int value = int.Parse(parts[1]);
                                            string remainingValue = Util.GetDateTime(parts[0].Split(' ')[0].Trim()).ToString("dd.MM.yyyy") + " to " + Util.GetDateTime(parts[0].Split(' ')[2].Trim()).ToString("dd.MM.yyyy");
                                            //int value = int.Parse(input.Substring(0, input.IndexOf("[")));
                                            worksheet.Cells[3, col, 4, col].Style.Font.Bold = true;
                                            worksheet.Cells[3, col, 4, col].Style.Font.Color.SetColor(Color.FromArgb(0, 0, 0));
                                            worksheet.Cells[3, col, 4, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                            worksheet.Cells[3, col, 4, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(189, 215, 238));
                                            worksheet.Cells[3, col].Value = value;
                                            worksheet.Cells[4, col].Value = remainingValue;
                                            worksheet.Column(col).Width = Convert.ToDouble(17);
                                            worksheet.Cells[4, col].Style.WrapText = true;
                                            col++;
                                        }
                                    }
                                    col = 6;
                                    if (dtWeeks.Columns.Count > 0)
                                    {
                                        foreach (DataRow dtrow in dtWeeks.Rows)
                                        {
                                            for (int k = 1; k < dtrow.Table.Columns.Count; k++)
                                            {
                                                worksheet.Cells[rowstart, col].Value = Convert.ToDouble(dtrow[k].ToString());
                                                col++;
                                            }
                                            col = 1;
                                        }
                                        rowstart++;
                                    }
                                    else
                                    {

                                    }
                                }
                                if (i == downIDs.Count - 1)
                                {
                                    worksheet.Cells[rowstart, 1, rowstart + 1, col].Style.Font.Bold.ToString();
                                    worksheet.Cells[rowstart, 1, rowstart + 1, col].Style.Font.Bold = true;
                                    worksheet.Cells[rowstart, 1, rowstart, col].Style.Font.Color.SetColor(Color.FromArgb(0, 0, 0));
                                    worksheet.Cells[rowstart, 1, rowstart, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[rowstart, 1, rowstart, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 228, 214));
                                    worksheet.Cells[rowstart, 1].Value = "Total";
                                    worksheet.Cells[rowstart + 1, 1].Style.Font.Color.SetColor(Color.FromArgb(0, 0, 0));
                                    worksheet.Cells[rowstart + 1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[rowstart + 1, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));
                                    worksheet.Cells[rowstart + 1, 1].Value = "13 WMvag";
                                    foreach (DataRow dtTotal in dt2.Rows)
                                    {
                                        worksheet.Cells[rowstart, 2].Value = dtTotal["sumtotaldowntime"].ToString();
                                    }
                                    col = 6;
                                    for (int k = 0; k < dt4.Rows.Count; k++)
                                    {
                                        worksheet.Cells[rowstart, col].Style.Font.Color.SetColor(Color.FromArgb(0, 0, 0));
                                        worksheet.Cells[rowstart, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[rowstart, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 228, 214));
                                        worksheet.Cells[rowstart, col].Value = Convert.ToDouble(dt4.Rows[k]["downtimesum"].ToString());
                                        worksheet.Cells[rowstart + 1, col].Style.Font.Color.SetColor(Color.FromArgb(0, 0, 0));
                                        worksheet.Cells[rowstart + 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[rowstart + 1, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));
                                        worksheet.Cells[rowstart + 1, col].Value = Convert.ToDouble(dt4.Rows[k]["downtimeavg"].ToString());
                                        col++;
                                    }
                                }
                                //worksheet.Cells[2, 6, 2, col].Merge = true;
                                worksheet.Cells[2, 6].Value = "TOTAL MAINTENANCE DOWNTIME TREND SUBSYSTEMWISE ";
                            }
                            //col--;
                            //FAILURE VS DOWNTIME----------------------
                            var chart1 = (ExcelBarChart)worksheet.Drawings.AddChart("paretoChart2", eChartType.ColumnClustered);

                            chart1.SetSize(800, 400);
                            chart1.SetPosition(rowstart + 3, 0, 15, 0);
                            ExcelChartSerie barchart = null;
                            ExcelLineChartSerie linechart = null;

                            ExcelLineChart chart2 = (ExcelLineChart)chart1.PlotArea.ChartTypes.Add(eChartType.Line);
                            if (5 + rowstart - 1 <= 15)
                            {
                                barchart = chart1.Series.Add(ExcelRange.GetAddress(5, 2, rowstart - 1, 2), ExcelRange.GetAddress(5, 1, rowstart - 1, 1));
                                linechart = chart2.Series.Add(ExcelRange.GetAddress(5, 5, rowstart - 1, 5), ExcelRange.GetAddress(5, 1, rowstart - 1, 1)) as ExcelLineChartSerie;
                            }
                            else
                            {
                                barchart = chart1.Series.Add(ExcelRange.GetAddress(5, 2, 20, 2), ExcelRange.GetAddress(5, 1, 20, 1));
                                linechart = chart2.Series.Add(ExcelRange.GetAddress(5, 5, 20, 5), ExcelRange.GetAddress(5, 1, 20, 1)) as ExcelLineChartSerie;
                            }
                            barchart.Header = "Down Time";
                            chart2.UseSecondaryAxis = true;
                            chart2.YAxis.MaxValue = 1;
                            linechart.Header = "Cumulative";
                            //linechart.DataLabel.Position = eLabelPosition.Top;
                            linechart.DataLabel.ShowValue = false;
                            //linechart.Marker = eMarkerStyle.Circle;
                            barchart.Fill.Color = Color.FromArgb(0, 112, 192);
                            //linechart.MarkerLineColor = Color.FromArgb(237, 125, 49);
                            //linechart.MarkerSize = 5;
                            //linechart.DataLabel.Fill.Color = Color.FromArgb(255, 192, 0);
                            chart1.VaryColors = true;
                            chart1.YAxis.Title.Text = "DownTime(mins)";
                            chart1.Title.Text = "Maitenance Downtime Pareto Subsystem Level " + Util.GetDateTime(fromDate.ToString()).ToString("dd.MM.yyyy") + " to " + Util.GetDateTime(toDate.ToString()).ToString("dd.MM.yyyy");
                            chart1.DataLabel.ShowValue = true;
                            chart2.DataLabel.ShowValue = true;
                            chart2.Legend.Position = eLegendPosition.Bottom;
                            chart1.DataLabel.Format = "00";
                            chart1.GapWidth = 5;
                            chart1.DataLabel.Position = eLabelPosition.InEnd;
                            chart1.XAxis.TextBody.Rotation = (-90);
                            barchart.Fill.Color = Color.FromArgb(0, 112, 192);
                            var axis = chart1.YAxis;
                            axis.Title.Rotation = (270);

                            //WEEK VS AVg DOWNTIME-------------
                            var chart3 = worksheet.Drawings.AddChart("ScatterChart2", eChartType.Line);
                            chart3.SetPosition(rowstart + 3, 0, 0, 0);
                            chart3.SetSize(600, 400);
                            var series = chart3.Series.Add(ExcelRange.GetAddress(rowstart + 1, 6, rowstart + 1, col - 1), ExcelRange.GetAddress(3, 6, 4, col - 1));
                            var trendline = series.TrendLines.Add(eTrendLine.Linear);
                            trendline.DisplayEquation = false;
                            trendline.DisplayRSquaredValue = false;
                            //ExcelChartTrendline line1 = series.TrendLines[0];
                            chart3.Title.Text = "PML Maintenance downtime 13 Week(s) mavg";
                            chart3.Legend.Remove();
                            chart3.XAxis.Title.Text = "Week";
                            chart3.YAxis.Title.Text = "Down Time(mins)";
                            chart3.XAxis.TextBody.Rotation = (-90);
                            axis = chart3.YAxis;
                            axis.Title.Rotation = (270);

                            //WEEK VS TOTAL DOWNTIME----------------
                            var chart4 = worksheet.Drawings.AddChart("LineChart2", eChartType.Line);
                            chart4.SetPosition(rowstart + 3, 0, 8, 0);
                            chart4.SetSize(600, 400);
                            var series2 = chart4.Series.Add(ExcelRange.GetAddress(rowstart, 6, rowstart, col - 1), ExcelRange.GetAddress(3, 6, 4, col - 1));
                            var trendline2 = series2.TrendLines.Add(eTrendLine.Linear);
                            trendline2.DisplayEquation = false;
                            trendline2.DisplayRSquaredValue = false;
                            chart4.Title.Text = "Weekwise Maintenance downtime Week " + worksheet.Cells[3, 6].Value.ToString() + " to Week " + worksheet.Cells[3, col - 1].Value.ToString();
                            chart4.Legend.Remove();
                            chart4.XAxis.Title.Text = "Week";
                            chart4.YAxis.Title.Text = "Down Time(mins)";
                            chart4.XAxis.TextBody.Rotation = (-90);
                            axis = chart4.YAxis;
                            axis.Title.Rotation = (270);

                            worksheet.Cells[2, 2, 2, 5].Merge = true;
                            worksheet.Cells[2, 1, 2, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[2, 1, 2, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                            worksheet.Cells[2, 1].Value = Util.GetDateTime(fromDate.ToString()).ToString("dd.MM.yyyy") + " - " + Util.GetDateTime(toDate.ToString()).ToString("dd.MM.yyyy");

                            worksheet.Cells[1, 1, 1, col].Merge = true;
                            worksheet.Cells[1, 1, 1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[1, 1, 1, col].Style.Font.Color.SetColor(Color.FromArgb(0, 0, 0));
                            worksheet.Cells[1, 1, 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, 1, 1, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));
                            worksheet.Cells[1, 1].Value = "MAINTENANCE - SUBSYSTEM LEVEL DOWNTIME PARETO";
                            setBorderThin(worksheet, 1, 1, rowstart + 1, col);
                        }
                    }

                    #endregion

                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    successfull = "Generated";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return successfull;
        }

        internal static string GenerateOverAllOEETrendReport(string machineID, DateTime FromDate, DateTime ToDate, string CellID)
        {
            string successful = "";
            try
            {
                string Filename = "OVERALLPMLOEETRENDReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "OVERALLPMLOEETRENDReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("OVERALLPMLOEETREND Report template does not exists at - " + Source);
                    successful = "TemplateNotFound";
                }
                else
                {
                    int a = 0;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);

                    #region OVER ALL OEE TREND
                    Excel.Workbook.Worksheets[a].Name = "Overall PML OEE";
                    var worksheet = Excel.Workbook.Worksheets[a];
                    DataTable dt = DBAccessPradeepMetals.OEEReportData(machineID, FromDate, ToDate, CellID, "OEE");
                    int col = 3;
                    foreach (DataColumn ColumnName in dt.Columns)
                    {
                        string input = ColumnName.ToString();
                        string[] parts = input.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                        int value = int.Parse(parts[0]);
                        string remainingValue = parts[1];
                        worksheet.Cells[100, col].Value = value + "    " + (Util.GetDateTime(remainingValue.Split(' ')[0].Trim()).ToString("dd.MM.yyyy") + " to " + Util.GetDateTime(remainingValue.Split(' ')[2].Trim()).ToString("dd.MM.yyyy"));
                        worksheet.Cells[3, col].Value = value;
                        worksheet.Cells[4, col].Value = (Util.GetDateTime(remainingValue.Split(' ')[0].Trim()).ToString("dd.MM.yyyy") + " to " + Util.GetDateTime(remainingValue.Split(' ')[2].Trim()).ToString("dd.MM.yyyy"));
                        worksheet.Cells[5, col].Value = HelperClassGeneric.getDoubleValueFromString(dt.Rows[0][ColumnName].ToString()) / 100;
                        worksheet.Cells[5, col].Style.Numberformat.Format = "#0%";
                        col++;
                    }
                    var chartList = worksheet.Drawings.ToList();

                    if (chartList.Count > 0)
                    {
                        ExcelBarChart chart = worksheet.Drawings[0] as ExcelBarChart;
                        chart.SetPosition(6, 0, 0, 0);
                        chart.SetSize(650, 400);
                        chart.Title.Text = "PML Overall OEE " + Util.GetDateTime(FromDate.ToString()).ToString("dd.MM.yyyy") + " to " + Util.GetDateTime(ToDate.ToString()).ToString("dd.MM.yyyy");
                        chart.XAxis.Title.Text = "Week Number";
                        chart.YAxis.Title.Text = "OEE %";
                        chart.YAxis.MaxValue = 1;
                        chart.DataLabel.ShowValue = true;
                        chart.DataLabel.ShowLeaderLines = true;
                        //chart.DataLabel.TextBody.UnderLine = eUnderLineType.Single;
                        chart.XAxis.TextBody.Rotation = (-90);
                        chart.YAxis.RemoveGridlines();

                        ExcelRange chartValues = worksheet.Cells[5, 3, 5, col - 1];
                        ExcelRange chartLabels = worksheet.Cells[3, 3, 4, col - 1];
                        ExcelChartSerie serie = chart.Series.Add(chartValues, chartLabels);
                        serie.TrendLines.Add(eTrendLine.Linear);
                        serie.Fill.Color = Color.FromArgb(0, 112, 192);

                        var axis1 = chart.YAxis;
                        axis1.Title.Rotation = (270);
                    }

                    col--;
                    worksheet.Cells[1, 1, 1, col].Merge = true;
                    worksheet.Cells[1, 1, 1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, col].Style.Font.Color.SetColor(Color.FromArgb(0, 0, 0));
                    worksheet.Cells[1, 1, 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 1, 1, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));
                    worksheet.Cells[1, 1].Value = "OVERALL PML OEE TREND";
                    double columnwidth = 25;
                    for (int j = 3; j <= col + 1; j++)
                    {
                        worksheet.Column(j).Width = columnwidth;
                    }
                    worksheet.Cells[100, 3, 100, col - 1].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    setBorderThin(worksheet, 1, 1, 5, col);
                    worksheet.Cells[3, col].AutoFitColumns();
                    a++;
                    #endregion

                    #region --------- PML PE TREND ------
                    Excel.Workbook.Worksheets[a].Name = "PERFORMANCE TREND";
                    var worksheet2 = Excel.Workbook.Worksheets[a];
                    DataTable dt2 = DBAccessPradeepMetals.OEEReportData(machineID, FromDate, ToDate, CellID, "PE");
                    int col_PE = 3;
                    foreach (DataColumn ColumnName in dt2.Columns)
                    {
                        string input = ColumnName.ToString();
                        string[] parts = input.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                        int value = int.Parse(parts[0]);
                        string remainingValue = parts[1];
                        worksheet2.Cells[100, col_PE].Value = value + "    " + (Util.GetDateTime(remainingValue.Split(' ')[0].Trim()).ToString("dd.MM.yyyy") + " to " + Util.GetDateTime(remainingValue.Split(' ')[2].Trim()).ToString("dd.MM.yyyy"));
                        worksheet2.Cells[3, col_PE].Value = value;
                        worksheet2.Cells[4, col_PE].Value = (Util.GetDateTime(remainingValue.Split(' ')[0].Trim()).ToString("dd.MM.yyyy") + " to " + Util.GetDateTime(remainingValue.Split(' ')[2].Trim()).ToString("dd.MM.yyyy"));
                        worksheet2.Cells[5, col_PE].Value = HelperClassGeneric.getDoubleValueFromString(dt2.Rows[0][ColumnName].ToString()) / 100;
                        worksheet2.Cells[5, col_PE].Style.Numberformat.Format = "#0%";
                        col_PE++;
                    }

                    var chartList1 = worksheet.Drawings.ToList();

                    if (chartList1.Count > 0)
                    {
                        ExcelBarChart chart1 = worksheet2.Drawings[0] as ExcelBarChart;

                        chart1.SetPosition(6, 0, 0, 0);
                        chart1.SetSize(650, 400);
                        chart1.Title.Text = "PML Overall PE " + Util.GetDateTime(FromDate.ToString()).ToString("dd.MM.yyyy") + " to " + Util.GetDateTime(ToDate.ToString()).ToString("dd.MM.yyyy");
                        chart1.XAxis.Title.Text = " Week Number";
                        chart1.YAxis.Title.Text = "PE %";
                        chart1.DataLabel.ShowValue = true;
                        chart1.YAxis.MaxValue = 1;
                        chart1.XAxis.TextBody.Rotation = (-90);
                        var axis2 = chart1.YAxis;
                        axis2.Title.Rotation = (270);
                        // Set the data range for the chart
                        ExcelRange chartValues_PE = worksheet2.Cells[5, 3, 5, col_PE - 1];
                        ExcelRange chartLabels_PE = worksheet2.Cells[3, 3, 4, col_PE - 1];
                        ExcelChartSerie serie_PE = chart1.Series.Add(chartValues_PE, chartLabels_PE);
                        serie_PE.TrendLines.Add(eTrendLine.Linear);
                        serie_PE.Fill.Color = Color.FromArgb(0, 112, 192);
                    }
                    col_PE--;
                    worksheet2.Cells[1, 1, 1, col_PE].Merge = true;
                    worksheet2.Cells[1, 1, 1, col_PE].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet2.Cells[1, 1, 1, col_PE].Style.Font.Color.SetColor(Color.FromArgb(0, 0, 0));
                    worksheet2.Cells[1, 1, 1, col_PE].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet2.Cells[1, 1, 1, col_PE].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));
                    worksheet2.Cells[1, 1].Value = "OVERALL PML PERFORMANCE TREND";
                    double columnwidth_PE = 25;
                    for (int m = 3; m <= col_PE + 1; m++)
                    {
                        worksheet2.Column(m).Width = columnwidth_PE;
                    }
                    worksheet.Cells[100, 3, 100, col - 1].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    setBorderThin(worksheet2, 1, 1, 5, col_PE);

                    worksheet2.Cells[3, col_PE].AutoFitColumns();
                    worksheet2.Cells[3, 3, 3, col_PE].Style.Font.Bold = true;
                    worksheet2.Cells[3, 3, 3, col_PE].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet2.Cells[4, 3, 4, col_PE].Style.Font.Bold = true;
                    worksheet2.Cells[4, 3, 4, col_PE].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    a++;
                    #endregion

                    #region ------------ PML AE TREND REPORT --------
                    Excel.Workbook.Worksheets[a].Name = "AVAILABILITY TREND";
                    var worksheet3 = Excel.Workbook.Worksheets[a];
                    DataTable dt3 = DBAccessPradeepMetals.OEEReportData(machineID, FromDate, ToDate, CellID, "AE");
                    int col_AE = 3;
                    foreach (DataColumn ColumnName in dt3.Columns)
                    {
                        string input = ColumnName.ToString();
                        string[] parts = input.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                        int value = int.Parse(parts[0]);
                        string remainingValue = parts[1];
                        worksheet3.Cells[100, col_AE].Value = value + "    " + (Util.GetDateTime(remainingValue.Split(' ')[0].Trim()).ToString("dd.MM.yyyy") + " to " + Util.GetDateTime(remainingValue.Split(' ')[2].Trim()).ToString("dd.MM.yyyy"));
                        worksheet3.Cells[3, col_AE].Value = value;
                        worksheet3.Cells[4, col_AE].Value = Util.GetDateTime(remainingValue.Split(' ')[0].Trim()).ToString("dd.MM.yyyy") + " to " + Util.GetDateTime(remainingValue.Split(' ')[2].Trim()).ToString("dd.MM.yyyy");
                        worksheet3.Cells[5, col_AE].Value = HelperClassGeneric.getDoubleValueFromString(dt3.Rows[0][ColumnName].ToString()) / 100;
                        worksheet3.Cells[5, col_AE].Style.Numberformat.Format = "#0%";
                        col_AE++;
                    }

                    var chartlist3 = worksheet3.Drawings.ToList();
                    if (chartlist3.Count > 0)
                    {
                        ExcelBarChart chart2 = worksheet3.Drawings[0] as ExcelBarChart;
                        chart2.SetPosition(6, 0, 0, 0);
                        chart2.SetSize(650, 400);
                        chart2.Title.Text = "PML Overall AE " + Util.GetDateTime(FromDate.ToString()).ToString("dd.MM.yyyy") + " to " + Util.GetDateTime(ToDate.ToString()).ToString("dd.MM.yyyy");
                        chart2.XAxis.Title.Text = "Week Number";
                        chart2.YAxis.Title.Text = "AE %";
                        chart2.YAxis.MaxValue = 1;
                        chart2.DataLabel.ShowValue = true;
                        chart2.XAxis.TextBody.Rotation = (-90);

                        var axis3 = chart2.YAxis;
                        axis3.Title.Rotation = (270);
                        // Set the data range for the chart
                        ExcelRange chartValues_AE = worksheet3.Cells[5, 3, 5, col_AE - 1];
                        ExcelRange chartLabels_AE = worksheet3.Cells[3, 3, 4, col_AE - 1];
                        ExcelChartSerie serie_AE = chart2.Series.Add(chartValues_AE, chartLabels_AE);
                        serie_AE.TrendLines.Add(eTrendLine.Linear);
                        serie_AE.Fill.Color = Color.FromArgb(0, 112, 192);
                    }
                    col_AE--;
                    worksheet3.Cells[1, 1, 1, col_AE].Merge = true;
                    worksheet3.Cells[1, 1, 1, col_AE].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet3.Cells[1, 1, 1, col_AE].Style.Font.Color.SetColor(Color.FromArgb(0, 0, 0));
                    worksheet3.Cells[1, 1, 1, col_AE].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet3.Cells[1, 1, 1, col_AE].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));
                    worksheet3.Cells[1, 1].Value = "OVERALL PML AVAILABILITY TREND";
                    double columnwidth_AE = 25;
                    for (int k = 3; k <= col_AE + 1; k++)
                    {
                        worksheet3.Column(k).Width = columnwidth_AE;
                    }
                    worksheet.Cells[100, 3, 100, col - 1].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    setBorderThin(worksheet3, 1, 1, 5, col_AE);
                    worksheet3.Cells[3, col_AE].AutoFitColumns();

                    worksheet3.Cells[3, 3, 3, col_PE].Style.Font.Bold = true;
                    worksheet3.Cells[3, 3, 3, col_PE].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet3.Cells[4, 3, 4, col_PE].Style.Font.Bold = true;
                    worksheet3.Cells[4, a, 4, col_PE].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    a++;
                    #endregion

                    #region ---------------- PML PARETO DOWNTIME REASON REPORT -------------------------

                    //Excel.Workbook.Worksheets.Add("Sheet4");
                    Excel.Workbook.Worksheets[a].Name = "PMLParetoDowntimeReasonreport";
                    var worksheet4 = Excel.Workbook.Worksheets[a];
                    //worksheet4.Cells[5, 1].Value = "Category";
                    //worksheet4.Cells[5, 2].Value = "Reason Code";
                    worksheet4.Cells[5, 1].Value = "Reason";
                    worksheet4.Cells[5, 2].Value = "Total PML,CNC/VMC Downtime";
                    worksheet4.Cells[5, 2].Style.WrapText = true;
                    worksheet4.Cells[5, 3].Value = "Cumulative";
                    worksheet4.Cells[5, 4].Value = "Cumulative %";
                    worksheet4.Cells[5, 5].Value = "Downtime %";

                    worksheet4.Cells[5, 1, 5, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet4.Cells[5, 1, 5, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(221, 235, 247));


                    DataTable dt_DR2 = null;
                    DataTable dt_DR3 = null;
                    DataTable dt_DR4 = null;
                    DataTable dt_DR1 = DBAccessPradeepMetals.ParetoDownTimeReason(machineID, FromDate, ToDate, out dt_DR2, out dt_DR3, out dt_DR4);
                    DataTable dtWeeks = new DataTable();
                    //var distCategory = dt_DR1.AsEnumerable().Select(k => k["Category"].ToString()).Distinct().ToList();
                    //var distdownID = dt_DR1.AsEnumerable().Select(k => k["downid"].ToString()).Distinct().ToList();

                    int rowstart = 6; int i = 0;
                    //if (distCategory.Count > 0)
                    //{
                    //    for (int category = 0; category < distCategory.Count; category++)
                    //    {
                    //        DataTable dtCategories = dt_DR1.AsEnumerable().Where(k => k["Category"].ToString().Equals(distCategory[category])).CopyToDataTable();
                    var distDownid = dt_DR1.AsEnumerable().Select(k => k["downid"].ToString()).Distinct().ToList();
                    //worksheet4.Cells[rowstart, 1].Value = distCategory[category];
                    //worksheet4.Cells[rowstart, 1].Style.TextRotation = 90;
                    //worksheet4.Cells[rowstart, 1, rowstart + (distDownid.Count - 1), 1].Merge = true;
                    //worksheet4.Cells[rowstart, 1, rowstart + (distDownid.Count - 1), 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //worksheet4.Cells[rowstart, 1, rowstart + (distDownid.Count - 1), 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                    //worksheet4.Cells[rowstart, 1, rowstart + (distDownid.Count - 1), 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //worksheet4.Cells[rowstart, 1, rowstart + (distDownid.Count - 1), 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    foreach (string downid in distDownid)
                    {
                        var firstRow = dt_DR1.AsEnumerable().Where(k => k["downid"].ToString().Equals(downid, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        //worksheet4.Cells[rowstart, 2].Value = firstRow["ReasonCode"];
                        worksheet4.Cells[rowstart, 1].Value = firstRow["downid"];
                        worksheet4.Cells[rowstart, 2].Value = HelperClassGeneric.getDoubleValueFromString(firstRow["Total_Downtime"].ToString());
                        worksheet4.Cells[rowstart, 3].Value = HelperClassGeneric.getDoubleValueFromString(firstRow["cumulative"].ToString());
                        worksheet4.Cells[rowstart, 4].Value = HelperClassGeneric.getDoubleValueFromString(firstRow["cumulativeper"].ToString()) / 100;
                        worksheet4.Cells[rowstart, 4].Style.Numberformat.Format = "#0%";
                        worksheet4.Cells[rowstart, 5].Value = firstRow["downtimeper"];
                        dtWeeks = dt_DR3.AsEnumerable().Where(x => x.Field<string>("downid").Equals(downid)).CopyToDataTable();
                        var distWeeks = dt_DR4.AsEnumerable().Select(k => k["weekfinaldata"].ToString()).Distinct().ToList();
                        col = 6;
                        if (dtWeeks.Columns.Count > 0)
                        {
                            foreach (DataRow dtrow in dtWeeks.Rows)
                            {
                                for (int k = 4; k < dtWeeks.Columns.Count; k++)
                                {
                                    worksheet4.Cells[rowstart, col].Value = HelperClassGeneric.getDoubleValueFromString(dtrow[k].ToString());
                                    col++;
                                }
                                //col = 1;
                            }
                        }
                        rowstart++;
                        col = 6;
                        foreach (string week in distWeeks)
                        {

                            if (i == 0)
                            {
                                string input = week;
                                string[] parts = input.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                                int value = int.Parse(parts[1]);
                                string remainingValue = parts[0];
                                //string input = week;
                                //int value = int.Parse(input.Substring(0, input.IndexOf("[")));
                                worksheet4.Cells[4, col, 5, col].Style.Font.Bold = true;
                                worksheet4.Cells[4, col, 5, col].Style.Font.Color.SetColor(Color.FromArgb(0, 0, 0));
                                worksheet4.Cells[4, col, 5, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet4.Cells[4, col, 5, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(189, 215, 238));
                                worksheet4.Cells[4, col].Value = value;
                                worksheet4.Cells[5, col].Value = Util.GetDateTime(remainingValue.Split(' ')[0].Trim()).ToString("dd.MM.yyyy") + " to " + Util.GetDateTime(remainingValue.Split(' ')[2].Trim()).ToString("dd.MM.yyyy");
                                worksheet4.Cells[5, col].Style.WrapText = true;
                                worksheet4.Cells[5, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                col++;
                            }
                        }
                        i++;
                    }
                    //if (category == distCategory.Count - 1)
                    //{
                    worksheet4.Cells[rowstart, 1, rowstart + 1, col].Style.Font.Bold.ToString();
                    worksheet4.Cells[rowstart, 1, rowstart + 1, col].Style.Font.Bold = true;
                    worksheet4.Cells[rowstart, 1, rowstart, col].Style.Font.Color.SetColor(Color.FromArgb(0, 0, 0));
                    worksheet4.Cells[rowstart, 1, rowstart, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet4.Cells[rowstart, 1, rowstart, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 228, 214));
                    worksheet4.Cells[rowstart, 1].Value = "Total";
                    worksheet4.Cells[rowstart + 1, 1].Style.Font.Color.SetColor(Color.FromArgb(0, 0, 0));
                    worksheet4.Cells[rowstart + 1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet4.Cells[rowstart + 1, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));
                    worksheet4.Cells[rowstart + 1, 1].Value = "13 WMvag";

                    foreach (DataRow dtTotal in dt_DR2.Rows)
                    {
                        worksheet4.Cells[rowstart, 2].Value = dtTotal["totaldowntimesum"].ToString();
                    }

                    col = 6;
                    for (int k = 0; k < dt_DR4.Rows.Count; k++)
                    {
                        worksheet4.Cells[rowstart, col].Style.Font.Color.SetColor(Color.FromArgb(0, 0, 0));
                        worksheet4.Cells[rowstart, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet4.Cells[rowstart, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 228, 214));
                        worksheet4.Cells[rowstart, col].Value = Convert.ToDouble(dt_DR4.Rows[k]["downtimesum"].ToString());
                        worksheet4.Cells[rowstart + 1, col].Style.Font.Color.SetColor(Color.FromArgb(0, 0, 0));
                        worksheet4.Cells[rowstart + 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet4.Cells[rowstart + 1, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));
                        worksheet4.Cells[rowstart + 1, col].Value = Convert.ToDouble(dt_DR4.Rows[k]["downtimeavg"].ToString());
                        col++;
                    }
                    //}

                    //}
                    worksheet4.Cells[3, 5, 3, col - 1].Merge = true;
                    worksheet4.Cells[3, 5].Value = "DOWN TIME(mins) ";
                    worksheet4.Cells[3, 5, 3, col - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet4.Cells[3, 5, 3, col - 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(189, 215, 238));
                    //}

                    for (int k = 1; k < col; k++)
                    {
                        worksheet4.Column(k).Width = Convert.ToDouble(15);
                    }


                    col--;


                    //13WMA Graph start
                    var li = worksheet4.Drawings.ToList();
                    int c = li.Count;
                    var chart3 = worksheet4.Drawings[0] as ExcelLineChart;
                    chart3.SetPosition(rowstart + 3, 0, 0, 0);
                    chart3.SetSize(600, 400);
                    var series2 = chart3.Series.Add(ExcelRange.GetAddress(rowstart + 1, 6, rowstart + 1, col), ExcelRange.GetAddress(4, 6, 5, col));
                    series2.TrendLines.Add(eTrendLine.Linear);
                    chart3.Title.Text = "Downtime 13 WMA";
                    chart3.Legend.Remove();
                    chart3.XAxis.Title.Text = "Week";
                    chart3.YAxis.Title.Text = "Down Time(mins)";
                    var axisy = chart3.YAxis;
                    axisy.Title.Rotation = (270);
                    //end



                    var chart4 = (ExcelBarChart)worksheet4.Drawings.AddChart("paretoChart4", eChartType.ColumnClustered);
                    if (rowstart > 10)
                    {
                        chart4.SetSize(20 * rowstart, 400);
                    }
                    else
                    {
                        chart4.SetSize(600, 400);
                    }

                    ExcelChartSerie barchart = null;
                    ExcelLineChart chart5 = (ExcelLineChart)chart4.PlotArea.ChartTypes.Add(eChartType.Line);
                    ExcelLineChartSerie linechart = null;
                    if (5 + rowstart <= 15)
                    {
                        barchart = chart4.Series.Add(ExcelRange.GetAddress(6, 2, rowstart - 1, 2), ExcelRange.GetAddress(6, 1, rowstart - 1, 1));
                        linechart = chart5.Series.Add(ExcelRange.GetAddress(6, 4, rowstart - 1, 4), ExcelRange.GetAddress(6, 1, rowstart - 1, 1)) as ExcelLineChartSerie;
                    }
                    else
                    {
                        barchart = chart4.Series.Add(ExcelRange.GetAddress(6, 2, 20, 2), ExcelRange.GetAddress(6, 1, 20, 1));
                        linechart = chart5.Series.Add(ExcelRange.GetAddress(6, 4, 20, 4), ExcelRange.GetAddress(6, 1, 20, 1)) as ExcelLineChartSerie;
                    }

                    chart4.SetPosition(rowstart + 3, 0, 10, 0);

                    barchart.Header = "Total PML, CNC/VMC Downtime";
                    chart5.UseSecondaryAxis = true;
                    linechart.Header = "Cumulative";
                    linechart.DataLabel.ShowValue = false;
                    chart5.YAxis.MaxValue = 1;
                    barchart.Fill.Color = Color.FromArgb(0, 112, 192);
                    //linechart.DataLabel.ShowValue = true;
                    //linechart.Marker = eMarkerStyle.Circle;
                    //linechart.MarkerLineColor = Color.FromArgb(237, 125, 49);
                    //linechart.MarkerSize = 5;
                    //linechart.DataLabel.Fill.Color = Color.FromArgb(255, 192, 0);
                    chart4.VaryColors = true;
                    chart4.YAxis.Title.Text = "DownTime(mins)";
                    chart4.Title.Text = "Downtime Pareto in Mins " + Util.GetDateTime(FromDate.ToString()).ToString("dd.MM.yyyy") + " to " + Util.GetDateTime(ToDate.ToString()).ToString("dd.MM.yyyy");
                    chart4.DataLabel.ShowValue = true;
                    chart5.DataLabel.ShowValue = true;
                    chart4.DataLabel.Format = "0";
                    chart4.GapWidth = 15;
                    chart4.DataLabel.Position = eLabelPosition.InEnd;
                    chart4.XAxis.TextBody.Rotation = (-90);
                    var axis = chart4.YAxis;
                    axis.Title.Rotation = (270);

                    chart4.Legend.Remove();


                    worksheet4.Cells[1, 1, 1, col].Merge = true;
                    worksheet4.Cells[1, 1, 1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet4.Cells[1, 1, 1, col].Style.Font.Color.SetColor(Color.FromArgb(0, 0, 0));
                    worksheet4.Cells[1, 1, 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet4.Cells[1, 1, 1, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));
                    worksheet4.Cells[3, 2].Value = "Period: " + Util.GetDateTime(FromDate.ToString()).ToString("dd.MM.yyyy") + " to " + Util.GetDateTime(ToDate.ToString()).ToString("dd.MM.yyyy");
                    worksheet4.Cells[1, 1, 4, 2].Style.Font.Bold = true;
                    worksheet4.Cells[1, 1].Value = "PML - PARETO - Downtime Reasons CNC+VMC";
                    if (col != 8)
                    {
                        worksheet4.Cells[3, 1].Value = "Period: " + Util.GetDateTime(FromDate.ToString()).ToString("dd.MM.yyyy") + " to " + Util.GetDateTime(ToDate.ToString()).ToString("dd.MM.yyyy") + " [Week " + worksheet4.Cells[4, 8].Value.ToString() + " to Week " + worksheet4.Cells[4, col - 1].Value.ToString() + "]";
                        //worksheet4.Cells[3, 2].Style.WrapText = true;
                    }

                    setBorderThin(worksheet4, 1, 1, rowstart + 1, col);
                    #endregion

                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    successful = "Generated";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return successful;
        }

        internal static string GenerateCNCProductionReport(DateTime startDate, DateTime Enddate, string machineid, string cellid, string shift)
        {
            string successful = "";
            try
            {
                string Filename = "ProductionReportCNC.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "ProductionReportCNC" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                string PrevMachineShift = "", CurrMachineShift = "";
                int count = 0;
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("ProductionReportCNC Report template does not exists at - " + Source);
                    successful = "TemplateNotFound";
                }
                else
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[0];
                    DataTable dt2 = null;
                    DataTable dt3 = null;
                    DataTable dt1 = DBAccessPradeepMetals.ProductionReportCNCData(startDate, Enddate, machineid, cellid, shift, out dt2, out dt3);
                    if (dt1.Rows.Count > 1 && dt1.Columns.Count > 1)
                    {
                        var distinctDetails = dt1.AsEnumerable().Select(x => new { date = x.Field<DateTime>("Date").ToString("dd-MM-yyyy"), shift = x.Field<string>("Shiftname") }).Distinct();
                        distinctDetails = distinctDetails.AsEnumerable().OrderBy(x => x.date.ToString()).ThenBy(x => x.shift.ToString()).ToList();

                        int rowstart = 6, b = 0, col = 1;
                        PrevMachineShift = $"{dt1.Rows[0]["MachineID"].ToString()}_{dt1.Rows[0]["Date"].ToString()}_{dt1.Rows[0]["ShiftName"].ToString()}";//_{dt1.Rows[0]["GroupID"].ToString()}
                        foreach (var item in distinctDetails)
                        {
                            var distMachines = dt1.AsEnumerable().Where(x => x.Field<DateTime>("Date").ToString("dd-MM-yyyy") == item.date && x.Field<string>("shiftname") == item.shift).Select(x => x["machineid"].ToString()).ToList();
                            var machine = "";
                            var machineWiseList = dt1.AsEnumerable().Where(k => k.Field<DateTime>("Date").ToString("dd-MM-yyyy").Equals(item.date, StringComparison.OrdinalIgnoreCase) && k.Field<string>("shiftname").ToString().Equals(item.shift,StringComparison.OrdinalIgnoreCase)).CopyToDataTable();
                            machineWiseList = machineWiseList.AsEnumerable().OrderBy(x => x["ShiftID"]).CopyToDataTable();
                            foreach (DataRow firstRow in machineWiseList.Rows)
                            {
                                machine = firstRow["machineid"].ToString();
                                CurrMachineShift = $"{machine}_{firstRow["Date"].ToString()}_{firstRow["ShiftName"].ToString()}";
                                worksheet.Cells[rowstart, 1].Value = firstRow["MachineID"].ToString();
                                worksheet.Cells[rowstart, 2].Value = firstRow["GroupID"].ToString();
                                worksheet.Cells[rowstart, 3].Value = Util.GetDateTime(firstRow["Date"].ToString()).ToString("yyyy-MM-dd");
                                worksheet.Cells[rowstart, 4].Value = firstRow["ShiftName"];
                                worksheet.Cells[rowstart, 5].Value = firstRow["StdShiftHrs"];
                                worksheet.Cells[rowstart, 6].Value = firstRow["ComponentID"];
                                worksheet.Cells[rowstart, 7].Value = firstRow["WorkOrderNo"];
                                worksheet.Cells[rowstart, 8].Value = firstRow["LotCode"];
                                worksheet.Cells[rowstart, 9].Value = firstRow["PlannedProdQty"];
                                worksheet.Cells[rowstart, 10].Value = firstRow["OperationNo"];
                                worksheet.Cells[rowstart, 11].Value = firstRow["StdCycleTime"];
                                worksheet.Cells[rowstart, 12].Value = firstRow["ActualProdQty"];

                                col = 13;
                                for (int i = 22; i < dt1.Columns.Count; i++)
                                {
                                    if (b == 0)
                                    {
                                        worksheet.Cells[rowstart - 1, col].Value = dt1.Columns[i].ColumnName.ToString();
                                    }
                                    worksheet.Cells[rowstart, col].Value = HelperClassGeneric.getDoubleValueFromString(firstRow[i].ToString());
                                    col++;
                                }

                                worksheet.Cells[rowstart, 13, rowstart, col].AutoFitColumns();

                                if (b == 0)
                                {
                                    worksheet.Cells[rowstart - 1, col].Value = "Total BreakDown Time";
                                    worksheet.Cells[rowstart, col].Value = HelperClassGeneric.getDoubleValueFromString(firstRow["TotalBreakDownTime"].ToString());
                                    col++;
                                    worksheet.Cells[rowstart - 1, col].Value = "Rejection Qty.";
                                    worksheet.Cells[rowstart, col].Value = firstRow["RejectionQty"];
                                    col++;
                                    worksheet.Cells[rowstart - 1, col].Value = "Rework Qty.";
                                    worksheet.Cells[rowstart, col].Value = firstRow["ReworkQty"];
                                    col++;
                                    worksheet.Cells[rowstart - 1, col].Value = "Actual Prod. Hours";
                                    worksheet.Cells[rowstart, col].Value = firstRow["ActualProductionHour"];
                                    col++;
                                    worksheet.Cells[rowstart - 1, col].Value = "Available Hours from Prod.";
                                    worksheet.Cells[rowstart, col].Value = firstRow["AvailableHoursForProduction"];
                                    col++;
                                    worksheet.Cells[rowstart - 1, col].Value = "AE";
                                    worksheet.Cells[rowstart, col].Value = firstRow["AE"];
                                    col++;
                                    worksheet.Cells[rowstart - 1, col].Value = "PE";
                                    worksheet.Cells[rowstart, col].Value = firstRow["PE"];
                                    col++;
                                    worksheet.Cells[rowstart - 1, col].Value = "QE";
                                    worksheet.Cells[rowstart, col].Value = firstRow["QE"];
                                    col++;
                                    worksheet.Cells[rowstart - 1, col].Value = "OEE";
                                    worksheet.Cells[rowstart, col].Value = firstRow["OEE"];
                                    col++;

                                    for (int a = 0; a < 6; a++)
                                        worksheet.Cells[rowstart - count, col - a, rowstart, col - a].Merge = true;
                                    count++;

                                    worksheet.Cells[rowstart - 1, 13, rowstart - 1, col - 1].AutoFilter = true;
                                    worksheet.Cells[rowstart - 1, 13, rowstart - 1, col - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[rowstart - 1, 13, rowstart - 1, col - 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));

                                    worksheet.Cells[1, 1, 1, col - 1].Merge = true;
                                    worksheet.Cells[1, 1, 1, col - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[1, 1, 1, col - 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                                    worksheet.Cells[1, 1].Value = "CNC Production Report";
                                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    b++;
                                }
                                else
                                {
                                    worksheet.Cells[rowstart, col].Value = HelperClassGeneric.getDoubleValueFromString(firstRow["TotalBreakDownTime"].ToString());
                                    col++;
                                    worksheet.Cells[rowstart, col].Value = firstRow["RejectionQty"];
                                    col++;
                                    worksheet.Cells[rowstart, col].Value = firstRow["ReworkQty"];
                                    col++;
                                    worksheet.Cells[rowstart, col].Value = firstRow["ActualProductionHour"];
                                    col++;
                                    worksheet.Cells[rowstart, col].Value = firstRow["AvailableHoursForProduction"];
                                    col++;
                                    worksheet.Cells[rowstart, col].Value = firstRow["AE"];
                                    col++;
                                    worksheet.Cells[rowstart, col].Value = firstRow["PE"];
                                    col++;
                                    worksheet.Cells[rowstart, col].Value = firstRow["QE"];
                                    col++;
                                    worksheet.Cells[rowstart, col].Value = firstRow["OEE"];

                                    if (PrevMachineShift == CurrMachineShift)
                                    {
                                        for (int a = 0; a < 6; a++)
                                            worksheet.Cells[rowstart - count, col - a, rowstart, col - a].Merge = true;
                                        count++;
                                    }
                                    else
                                    {
                                        count = 1;
                                        PrevMachineShift = CurrMachineShift;
                                    }
                                    col++;
                                }

                                rowstart++;
                            }
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            var row = dt2.Rows[0];
                            col = 13;
                            foreach (DataColumn column in dt2.Columns)
                            {
                                worksheet.Cells[rowstart, col].Value = HelperClassGeneric.getDoubleValueFromString(row[column].ToString());
                                col++;
                            }
                        }
                        if (dt3.Rows.Count > 0)
                        {
                            var row1 = dt3.Rows[0];
                            worksheet.Cells[rowstart, 9].Value = HelperClassGeneric.getDoubleValueFromString(row1["PlannedProdQty"].ToString());
                            worksheet.Cells[rowstart, 12].Value = HelperClassGeneric.getDoubleValueFromString(row1["ActualProdQty"].ToString());
                            worksheet.Cells[rowstart, col].Value = HelperClassGeneric.getDoubleValueFromString(row1["TotalBreakDownTime"].ToString());
                            worksheet.Cells[rowstart, ++col].Value = HelperClassGeneric.getDoubleValueFromString(row1["RejectionQty"].ToString());
                            worksheet.Cells[rowstart, ++col].Value = HelperClassGeneric.getDoubleValueFromString(row1["ReworkQty"].ToString());
                            worksheet.Cells[rowstart, ++col].Value = HelperClassGeneric.getDoubleValueFromString(row1["ActualProductionHour"].ToString());
                            worksheet.Cells[rowstart, ++col].Value = HelperClassGeneric.getDoubleValueFromString(row1["AvailableHoursForProduction"].ToString());
                            worksheet.Cells[rowstart, ++col].Value = HelperClassGeneric.getDoubleValueFromString(row1["AE"].ToString());
                            worksheet.Cells[rowstart, ++col].Value = HelperClassGeneric.getDoubleValueFromString(row1["PE"].ToString());
                            worksheet.Cells[rowstart, ++col].Value = HelperClassGeneric.getDoubleValueFromString(row1["QE"].ToString());
                            worksheet.Cells[rowstart, ++col].Value = HelperClassGeneric.getDoubleValueFromString(row1["OEE"].ToString());
                            worksheet.Cells[rowstart, 1].Value = "Summary";
                            worksheet.Cells[rowstart, 1].Style.Font.Bold = true;
                            worksheet.Cells[rowstart, 1, rowstart, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowstart, 1, rowstart, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                        }
                        setBorderThin(worksheet, 1, 1, rowstart, col);
                    }
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    successful = "Generated";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return successful;
        }

        public static string GetScheduleScreenReport(List<PMSceduleScreenEntity> list)
        {
            string isGenerated = "";
            try
            {
                string Filename = "ScheduleScreenReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "ScheduleScreenReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Schedule Screen Report template does not exists at - " + Source);
                    isGenerated = "TemplateNotFound";
                }
                else
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[0];
                    worksheet.Name = "ScheduleReport(Completed&Hold)";

                    int rowStart = 3, colStart = 1;

                    foreach (var li in list)
                    {
                        colStart = 1;
                        worksheet.Cells[rowStart, colStart].Value = li.MachineID;
                        colStart++;
                        worksheet.Cells[rowStart, colStart].Value = li.PartNumber;
                        colStart++;
                        worksheet.Cells[rowStart, colStart].Value = li.JobNumber;
                        colStart++;
                        worksheet.Cells[rowStart, colStart].Value = li.LotCode;
                        colStart++;
                        worksheet.Cells[rowStart, colStart].Value = li.OperationNo;
                        colStart++;
                        worksheet.Cells[rowStart, colStart].Value = li.ScheduleDate;
                        colStart++;
                        worksheet.Cells[rowStart, colStart].Value = HelperClassGeneric.getDoubleValueFromString(li.PlannedQuantity);
                        colStart++;
                        worksheet.Cells[rowStart, colStart].Value = li.ScheduleStatus;
                        rowStart++;
                    }

                    worksheet.Cells[1, 8].AutoFilter = true;
                    setBorderThin(worksheet, 3, 1, rowStart - 1, colStart);
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    isGenerated = "Successful";

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return isGenerated;
        }
    }
}