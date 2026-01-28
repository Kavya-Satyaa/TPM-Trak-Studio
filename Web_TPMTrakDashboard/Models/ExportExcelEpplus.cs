using Elmah;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Web_TPMTrakDashboard.Models
{
    public class ExportExcelEpplus
    {
        static string appPath = HttpContext.Current.Server.MapPath("~/Reports");

        public static string VDGEventReportGenerate_Hawkins(DataTable dt, string fromadate, string todate, string Machine)
        {
            string src; string destination = string.Empty;
            try
            {
                string template = "VDGEventReport_Hawkins.xlsx";
                src = Path.Combine(appPath, "ReportTemplates", template);
                DataTable eventdata = HttpContext.Current.Session["eventdatasource"] as DataTable;
                string Filename = "VDGEventReport_Hawkins.xlsx";
                string Template = string.Empty;
                Template = "VDGEventReport_Hawkins" + DateTime.Now + ".xlsx";

                destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                string timeformat = DataBaseAccess.getTimeFormatFromCockpit();
                if (!File.Exists(src))
                {
                    Logger.WriteDebugLog("Tool Wise Cycle Time- \n " + src);
                }
                FileInfo newFile = new FileInfo(src);
                ExcelPackage pck = new ExcelPackage(newFile, true);
                if (eventdata.Rows.Count > 0)
                {

                    var worksheet = pck.Workbook.Worksheets[1];
                    worksheet.Cells["B5"].Value = fromadate;
                    worksheet.Cells["E5"].Value = todate;
                    worksheet.Cells["H5"].Value = Machine;
                    int startrow = 9;
                    for (int i = 0; i < eventdata.Rows.Count; i++)
                    {
                        worksheet.Cells[startrow, 1].Value = eventdata.Rows[i]["componentid"];
                        worksheet.Cells[startrow, 2].Value = eventdata.Rows[i]["operationno"];
                        worksheet.Cells[startrow, 3].Value = eventdata.Rows[i]["EventID"];
                        worksheet.Cells[startrow, 4].Value = eventdata.Rows[i]["EventDescription"];
                        worksheet.Cells[startrow, 5].Value = Convert.ToDateTime(eventdata.Rows[i]["EventTS"]).ToString();
                        worksheet.Cells[startrow, 6].Value = eventdata.Rows[i]["Employeeid"];
                        worksheet.Cells[startrow, 7].Value = eventdata.Rows[i]["OprName"];
                        //worksheet.Cells[startrow, 7].Value =Convert.ToDateTime(eventdata.Rows[i][12]).ToString();

                        //worksheet.Cells[startrow, 7].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;

                        startrow++;
                    }

                    worksheet.Cells[8, 1, startrow, 7].AutoFitColumns();

                    setBorderThin(worksheet, 8, 1, startrow - 1, 7);
                }

                DownloadMultipleFile(destination, pck.GetAsByteArray());

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {

            }
            return destination;
        }
        public static void setBorderThin(ExcelWorksheet sheet, int fromRow, int fromcol, int toRow, int toCol)
        {
            try
            {
                var modelTable3 = sheet.Cells[fromRow, fromcol, toRow, toCol];
                modelTable3.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTable3.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        public static string VDGReportGenerate(List<string> column, DataTable dt, string fromadate, string todate, string Machine, string reportName, List<string> colHeader)
        {
            string src, dst = string.Empty;
            try
            {
                string timeFormat = DataBaseAccess.getTimeFormatFromCockpit();
                string tempalate = "TPM-Trak Report Template.xlsx";
                if (HttpContext.Current.Session["Language"] == null)
                    src = Path.Combine(appPath, "ReportTemplates", tempalate);
                else
                {
                    if (HttpContext.Current.Session["Language"].ToString() != "en")
                        src = Path.Combine(appPath, "ReportTemplates-" + HttpContext.Current.Session["Language"].ToString() + "", tempalate);
                    else
                        src = Path.Combine(appPath, "ReportTemplates", tempalate);
                }
                //src = Path.Combine(appPath, "ReportTemplates", tempalate);
                string tempfileName = reportName + "_" + Machine + "_" + Guid.NewGuid() + ".xlsx";
                dst = Path.Combine(appPath, "Temp", SafeFileName(tempfileName));

                if (!File.Exists(src))
                {
                    Logger.WriteDebugLog("OEE Template Not Found at the Path - \n " + src);
                    return "OEE Template Not Found at the Path - \n " + src;
                }
                FileInfo newFile = new FileInfo(src);
                ExcelPackage pck = new ExcelPackage(newFile, true);
                var wsDt = pck.Workbook.Worksheets[1];


                int rowPos = 14;
                int startChar = 65;

                for (int k = 0; k < column.Count; k++)
                {
                    var val = (char)startChar;
                    var range = string.Format(val + "{0}" + ":" + val + "{1}", 12, 13);
                    wsDt.Cells[range].Merge = true;
                    wsDt.Cells[range].Value = column[k].ToString();
                    wsDt.Cells[range].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    wsDt.Cells[range].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(22, 54, 92));
                    wsDt.Cells[range].Style.Font.Color.SetColor(Color.White);
                    wsDt.Cells[range].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    wsDt.Cells[range].Style.Border.Right.Color.SetColor(Color.White);
                    wsDt.Cells[range].Style.Font.Bold = true;
                    wsDt.Cells[range].Style.Font.Size = 12;
                    wsDt.Cells[range].Style.Font.Name = "Times New Roman";
                    wsDt.Cells[range].Style.WrapText = true;
                    startChar++;
                }

                int col = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    col = 1;
                    foreach (string item in colHeader)
                    {
                        if (item.ToString().Equals("StartTime", StringComparison.OrdinalIgnoreCase) || item.ToString().Equals("EndTime", StringComparison.OrdinalIgnoreCase))
                        {
                            if (dt.Rows[i][item] != DBNull.Value)
                                wsDt.Cells[rowPos, col].Value = Convert.ToDateTime(dt.Rows[i][item]).ToString("dd-MM-yyyy HH:mm:ss");
                        }
                        else if ((reportName.Equals("Production Data",StringComparison.OrdinalIgnoreCase) && (item.ToString().Equals("CycleTime", StringComparison.OrdinalIgnoreCase) || item.ToString().Equals("LoadUnloadTime", StringComparison.OrdinalIgnoreCase) || item.ToString().Equals("PDT", StringComparison.OrdinalIgnoreCase) || item.ToString().Equals("In_Cycle_DownTime", StringComparison.OrdinalIgnoreCase) || item.ToString().Equals("PDT", StringComparison.OrdinalIgnoreCase) || item.ToString().Equals("SpindleCycleTime", StringComparison.OrdinalIgnoreCase))) 
                            
                            || (reportName.Equals("Down Time Data", StringComparison.OrdinalIgnoreCase) && (item.ToString().Equals("DownTime", StringComparison.OrdinalIgnoreCase) || item.ToString().Equals("DownThreshold", StringComparison.OrdinalIgnoreCase))))
                        {
                            if (timeFormat.Equals("hh:mm:ss"))
                            {
                                //TimeSpan timeSpan = TimeSpan.Parse(dt.Rows[i][item].ToString());
                                var value = dt.Rows[i][item].ToString().Split(':');
                                TimeSpan timeSpan = new TimeSpan(int.Parse(value[0]),    // hours
                                                                 int.Parse(value[1]),    // minutes
                                                                 int.Parse(value[2]));
                                wsDt.Cells[rowPos, col].Value = timeSpan;
                                wsDt.Cells[rowPos, col].Style.Numberformat.Format = "[h]:mm:ss";
                            }
                            else
                            {
                                wsDt.Cells[rowPos, col].Value = string.IsNullOrEmpty(dt.Rows[i][item].ToString()) ? 0 : Convert.ToDouble(dt.Rows[i][item].ToString());
                            }
                        
                        }
                        else
                        {
                            wsDt.Cells[rowPos, col].Value = dt.Rows[i][item];
                        }
                        col++;
                    }
                    //wsDt.Cells[rowPos, 1].Value = dt.Rows[i][0];
                    //wsDt.Cells[rowPos, 2].Value = dt.Rows[i][1];
                    //wsDt.Cells[rowPos, 3].Value = dt.Rows[i][2];
                    //wsDt.Cells[rowPos, 4].Value = dt.Rows[i][3];
                    //wsDt.Cells[rowPos, 5].Value = dt.Rows[i][4];
                    //wsDt.Cells[rowPos, 6].Value = dt.Rows[i][5];
                    //wsDt.Cells[rowPos, 7].Value = dt.Rows[i][6];
                    //wsDt.Cells[rowPos, 8].Value = dt.Rows[i][7];
                    //wsDt.Cells[rowPos, 9].Value = dt.Rows[i][8];

                    //wsDt.Cells[rowPos, 10].Value = dt.Rows[i][9];
                    //wsDt.Cells[rowPos, 11].Value = dt.Rows[i][10];
                    //wsDt.Cells[rowPos, 12].Value = dt.Rows[i][11];
                    //wsDt.Cells[rowPos, 13].Value = dt.Rows[i][12];
                    rowPos++;
                }

                //  setFontStylesForCells(wsDt, dt.Rows.Count + 15, 14, column.Count);

                using (var range = wsDt.Cells[14, 1, dt.Rows.Count + 13, column.Count])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(251, 251, 251));
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Hair;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Hair;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Hair;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    range.Style.Font.Size = 11;
                    range.Style.Font.Color.SetColor(Color.Black);
                    //(ExcelBorderStyle.Medium, Color.Red);
                }

                rowPos = 14;
                if (reportName.Equals("Production&DownData",StringComparison.OrdinalIgnoreCase))
                {
                    for (int i = 0; i < dt.Rows.Count; i++) 
                    {
                        foreach (string item in colHeader)
                        {
                            if(item.Equals("DownTime",StringComparison.OrdinalIgnoreCase))
                            {
                                if (!string.IsNullOrEmpty(dt.Rows[i][item].ToString()) && dt.Rows[i][item].ToString() != "&nbsp;")
                                {
                                    wsDt.Cells[rowPos, 1, rowPos, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    wsDt.Cells[rowPos, 1, rowPos, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(237, 135, 135));
                                }
                            }
                        }
                        rowPos++;
                    }
                }
                    
                //wsDt.Column(6).Width = 25;
                //wsDt.Column(7).Width = 25;
                wsDt.Column(2).AutoFit();
                //wsDt.Column(6).Style.Numberformat.Format = "dd-MMM-yyyy hh:mm:ss AM/PM";
                //wsDt.Column(7).Style.Numberformat.Format = "dd-MMM-yyyy hh:mm:ss AM/PM";

                wsDt.Cells[1, 8].Value = "Report Date : " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                wsDt.Cells[5, 3].Value = fromadate;
                wsDt.Cells[7, 3].Value = todate;
                wsDt.Cells[1, 4].Value = Machine + " - " + reportName;
                wsDt.Cells[wsDt.Dimension.Address].AutoFitColumns(20, 100);
                //var fi = new FileInfo(dst);
                //if (fi.Exists)
                //{
                //    fi.Delete();
                //}
                //pck.SaveAs(fi);
                //if (colHeader.Any(x => x.Equals("PartsCount", StringComparison.OrdinalIgnoreCase)))
                //{
                //    int index = colHeader.IndexOf("PartsCount");
                //    wsDt.Column(index + 1).Style.Numberformat.Format = "Number";
                //}
                DownloadMultipleFile(dst, pck.GetAsByteArray());
                Logger.WriteDebugLog("Report generated sucessfully.");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {

            }
            return dst;

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
                // HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //Logger.WriteErrorLog("GENERATED ERROR : \n" + "Report generation Failed Error: " + ex.ToString());
            }
        }

        public static string SafeFileName(string name)
        {
            StringBuilder str = new StringBuilder(name);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.ToString();
        }

        internal static string ParameterReportGenerate(DataTable dtTemp, string fromDate, string toDate, string machineId, List<string> columnNames)
        {
            string source, destination = string.Empty;
            try
            {
                string tempalate = "ProcessParameterReport.xlsx";
                if (HttpContext.Current.Session["Language"] == null)
                    source = Path.Combine(appPath, "TPMTrakReport", tempalate);
                else
                {
                    if (HttpContext.Current.Session["Language"].ToString() != "en")
                        source = Path.Combine(appPath, "TPMTrakReport-" + HttpContext.Current.Session["Language"].ToString() + "", tempalate);
                    else
                        source = Path.Combine(appPath, "TPMTrakReport", tempalate);
                }
                string tempfileName = "ProcessParameterReport" + "_" + machineId + "_" + Guid.NewGuid() + ".xlsx";
                destination = Path.Combine(appPath, "Temp", SafeFileName(tempfileName));

                if (!File.Exists(source))
                {
                    Logger.WriteDebugLog("Process Parameter Template Not Found at the Path - \n " + source);
                    return "OEE Template Not Found at the Path - \n " + source;
                }

                FileInfo newFile = new FileInfo(source);
                ExcelPackage pck = new ExcelPackage(newFile, true);
                var wsDt = pck.Workbook.Worksheets[1];

                int rowPos = 9;
                using (var range = wsDt.Cells[9, 1, dtTemp.Rows.Count + 8, dtTemp.Columns.Count - 2])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(251, 251, 251));
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Hair;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Hair;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Hair;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.Font.Size = 11;
                    range.Style.Font.Color.SetColor(Color.Black);
                }

                int col = 1;
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    col = 1;
                    foreach (string item in columnNames)
                    {
                        wsDt.Cells[rowPos, col].Value = dtTemp.Rows[i][item];
                        col++;
                    }
                    rowPos++;
                }

                wsDt.Cells[5, 2].Value = machineId;
                wsDt.Cells[5, 6].Value = fromDate;
                wsDt.Cells[5, 10].Value = toDate;

                DownloadMultipleFile(destination, pck.GetAsByteArray());
                Logger.WriteDebugLog("Report generated sucessfully.");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return destination;
        }

        public static string ExportScrapAnalysis(DataTable dt, string PlantID, string GroupID, string MachineID, string Category, string SubCategory, string Description, string RejectionID, DateTime fromDate, DateTime Todate, string Format)
        {
            string src, dst = string.Empty;
            try
            {
                DataView dv = dt.DefaultView;
                if (dv != null && dv.Table.Rows.Count > 0)
                {
                    dv.Sort = "RejQty desc";
                    dt = dv.ToTable();
                }
                string tempalate = "ScrapAnalysis.xlsx";
                if (HttpContext.Current.Session["Language"] == null)
                    src = Path.Combine(appPath, "TPMTrakReport", tempalate);
                else
                {
                    if (HttpContext.Current.Session["Language"].ToString() != "en")
                        src = Path.Combine(appPath, "TPMTrakReport-" + HttpContext.Current.Session["Language"].ToString() + "", tempalate);
                    else
                        src = Path.Combine(appPath, "TPMTrakReport", tempalate);
                }
                string tempfileName = "ScrapAnalysis" + "_" + Guid.NewGuid() + ".xlsx";
                dst = Path.Combine(appPath, "Temp", SafeFileName(tempfileName));

                if (!File.Exists(src))
                {
                    Logger.WriteDebugLog("Template Not Found at the Path - \n " + src);
                    return "Template Not Found at the Path - \n " + src;
                }
                RejectionID = RejectionID.Replace("\"", string.Empty).Replace("'", string.Empty);
                MachineID = MachineID.Replace("\"", string.Empty).Replace("'", string.Empty);
                FileInfo newFile = new FileInfo(src);
                ExcelPackage pck = new ExcelPackage(newFile, true);
                var wsDt = pck.Workbook.Worksheets[1];

                double val = 0.0;
                //wsDt.Cells["B3"].Style.WrapText = true;
                //wsDt.Cells["H2"].Style.WrapText = true;
                int rowPos = 37;
                int col = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    wsDt.Cells[rowPos, 1].Value = col;

                    //if (Format.Equals("Rejection", StringComparison.OrdinalIgnoreCase))
                    //{
                        wsDt.Cells[rowPos, 2].Value = dt.Rows[i][1];
                        wsDt.Cells[rowPos, 3].Value = dt.Rows[i][3];
                        wsDt.Cells[rowPos, 4].Value = dt.Rows[i][2];
                        val += Convert.ToDouble(dt.Rows[i][3].ToString());
                    //}
                    //else
                    //{
                    //    wsDt.Cells[rowPos, 2].Value = dt.Rows[i][2];
                    //    wsDt.Cells[rowPos, 3].Value = dt.Rows[i][5];
                    //    wsDt.Cells[rowPos, 4].Value = dt.Rows[i][4];
                    //    val += Convert.ToDouble(dt.Rows[i][5].ToString());
                    //}

                    wsDt.Cells[rowPos, 5].Value = val;
                    //wsDt.Cells[rowPos, 2].Style.WrapText = true;
                    col++; rowPos++;




                }
                var chart = (ExcelBarChart)wsDt.Drawings.AddChart("", eChartType.ColumnClustered);
                //ExcelBarChart chart = (ExcelBarChart)wsDt.Drawings.AddChart("Status", eChartType.BarClustered);
                chart.SetSize(1200, 600);
                chart.SetPosition(100, 10);
                //chart.Title.Text = result;
                ExcelChartSerie ch1 = chart.Series.Add(ExcelRange.GetAddress(37, 4, rowPos, 4), ExcelRange.GetAddress(37, 2, rowPos, 2));
                ExcelChart chart2 = chart.PlotArea.ChartTypes.Add(eChartType.Line);
                ExcelChartSerie ch2 = chart2.Series.Add(ExcelRange.GetAddress(37, 5, rowPos, 5), ExcelRange.GetAddress(37, 2, rowPos, 2));
                ch1.Header = "Rejection";
                chart2.UseSecondaryAxis = true;
                ch2.Header = "Pareto";
                chart.VaryColors = true;
                (ch1 as ExcelBarChartSerie).DataLabel.ShowValue = true;
                (ch1 as ExcelBarChartSerie).DataLabel.Position = eLabelPosition.Top;
                (ch2 as ExcelLineChartSerie).DataLabel.ShowValue = true;
                (ch2 as ExcelLineChartSerie).DataLabel.Position = eLabelPosition.Top;
                (ch2 as ExcelLineChartSerie).MarkerSize = 5;
                (ch2 as ExcelLineChartSerie).Marker = eMarkerStyle.Circle;

                if (dt != null && dt.Rows.Count > 0)
                {
                    using (var range = wsDt.Cells[37, 1, (37 + dt.Rows.Count + 6), 4])
                    {
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(251, 251, 251));
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                        range.Style.Border.Top.Style = ExcelBorderStyle.Hair;
                        range.Style.Border.Left.Style = ExcelBorderStyle.Hair;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Hair;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        range.Style.Font.Size = 11;
                        range.Style.Font.Color.SetColor(Color.Black);
                        //(ExcelBorderStyle.Medium, Color.Red);
                    }
                }
                SetChartPointRandomColors(chart, 0);
                chart.YAxis.Title.Text = "Rejection Quantity";
                chart2.YAxis.Title.Text = "Cum  %";
                chart.XAxis.Title.Text = "Rejection Code";
                wsDt.Cells[wsDt.Dimension.Address].AutoFitColumns(20, 100);
                wsDt.Cells["B2"].Value = PlantID == string.Empty ? "All" : PlantID;
                wsDt.Cells["E2"].Value = GroupID == string.Empty ? "All" : GroupID;
                wsDt.Cells["H2"].Value = MachineID == string.Empty ? "All" : MachineID;
                wsDt.Cells["J2"].Value = Category == string.Empty ? "All" : Category;
                wsDt.Cells["B3"].Value = RejectionID == string.Empty ? "All" : RejectionID;
                if (Format.Equals("Globe"))
                {
                    wsDt.Cells["L2"].Value = SubCategory == string.Empty ? "All" : SubCategory;
                    wsDt.Cells["N2"].Value = Description == string.Empty ? "All" : Description;
                }
                else
                {
                    wsDt.Cells["K2"].Value = "";
                    wsDt.Cells["M2"].Value = "";
                    wsDt.Cells["L2"].Value = "";
                    wsDt.Cells["N2"].Value = "";
                }
                wsDt.Cells["E3"].Value = fromDate.ToString("dd-MM-yyyy");
                wsDt.Cells["H3"].Value = Todate.ToString("dd-MM-yyyy");
                DownloadMultipleFile(dst, pck.GetAsByteArray());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return dst;
        }

        public static void SetChartPointRandomColors(ExcelChart chart, int serieNumber)
        {
            var chartXml = chart.ChartXml;

            var nsa = chart.WorkSheet.Drawings.NameSpaceManager.LookupNamespace("a");
            var nsuri = chartXml.DocumentElement.NamespaceURI;

            var nsm = new XmlNamespaceManager(chartXml.NameTable);
            nsm.AddNamespace("a", nsa);
            nsm.AddNamespace("c", nsuri);

            var serieNode = chart.ChartXml.SelectSingleNode(@"c:chartSpace/c:chart/c:plotArea/c:barChart/c:ser[c:idx[@val='" + serieNumber + "']]", nsm);
            var serie = chart.Series[serieNumber];
            var points = serie.Series.Length;
            var rand = new Random(serieNumber);

            for (var i = 1; i <= points; i++)
            {
                var dPt = chartXml.CreateNode(XmlNodeType.Element, "dPt", nsuri);
                var idx = chartXml.CreateNode(XmlNodeType.Element, "idx", nsuri);
                var att = chartXml.CreateAttribute("val", nsuri);
                att.Value = i.ToString();
                idx.Attributes.Append(att);
                dPt.AppendChild(idx);

                var srgbClr = chartXml.CreateNode(XmlNodeType.Element, "srgbClr", nsa);
                att = chartXml.CreateAttribute("val");

                //Generate a random color - override with own logic to specify
                var color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                att.Value = $"{color.R:X2}{color.G:X2}{color.B:X2}";
                srgbClr.Attributes.Append(att);

                var solidFill = chartXml.CreateNode(XmlNodeType.Element, "solidFill", nsa);
                solidFill.AppendChild(srgbClr);

                var spPr = chartXml.CreateNode(XmlNodeType.Element, "spPr", nsuri);
                spPr.AppendChild(solidFill);

                dPt.AppendChild(spPr);
                serieNode.AppendChild(dPt);
            }
        }
    }
}