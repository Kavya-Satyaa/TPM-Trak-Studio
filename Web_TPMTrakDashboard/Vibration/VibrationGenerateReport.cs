using Elmah;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Web_TPMTrakDashboard.Vibration
{
    public class VibrationGenerateReport
    {
        static readonly string appPath = HttpContext.Current.Server.MapPath("~/Vibration/Reports");
        static string appPathForReportOutput = HttpContext.Current.Server.MapPath("~/Vibration/Reports/GeneratedReports/");

        public VibrationGenerateReport()
        {
            CreateReportOutput();
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

        private static void CreateReportOutput()
        {
            if (!Directory.Exists(appPathForReportOutput))
            {
                Directory.CreateDirectory(appPathForReportOutput);
            }
        }

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

        #region Vibration Data Report - TAFE
        internal static bool GenerateVibrationDataReport(string machineID, string from_date, string to_date, DataTable VibrationDt)
        {
            bool successfull = false;
            try
            {
                string Filename = "VibrationDataReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "VibrationDataReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Vibration Data Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                    var distParameter = VibrationDt.AsEnumerable().Select(k => k.Field<string>("ParameterID")).Distinct().ToList();
                    var firstSheet = excelPackage.Workbook.Worksheets[1];
                    for (int paramCount = 0; paramCount < distParameter.Count; paramCount++)
                    {
                        int rowStart = 32;
                        int val = 0;
                        double result = 0.0;
                        string parameter = distParameter[paramCount];
                        if (!ConfigurationManager.AppSettings["VibrationParameterEnable"].Equals("1"))
                        {
                            parameter = "Vibration";
                        }
                        excelPackage.Workbook.Worksheets.Copy(firstSheet.Name, parameter);
                        var wrkshtVibrationData = excelPackage.Workbook.Worksheets[parameter];
                        DataTable dtVibrationData = VibrationDt.AsEnumerable().Where(k => k.Field<string>("ParameterID") == distParameter[paramCount]).CopyToDataTable();
                        wrkshtVibrationData.Cells["J4"].Value = machineID;
                        wrkshtVibrationData.Cells["B4"].Value = from_date;
                        wrkshtVibrationData.Cells["F4"].Value = to_date;
                        if (dtVibrationData != null && dtVibrationData.Rows.Count > 0)
                        {
                            foreach (DataRow dataRow in dtVibrationData.Rows)
                            {
                                wrkshtVibrationData.Cells[rowStart, 1].Value = dataRow["ComponentID"].ToString();
                                wrkshtVibrationData.Cells[rowStart, 2].Value = dataRow["Operationno"].ToString();
                                if (dataRow["ActualValue"] != null && !string.IsNullOrEmpty(dataRow["ActualValue"].ToString()))
                                {
                                    if (double.TryParse(dataRow["ActualValue"].ToString(), out result))
                                    {
                                        wrkshtVibrationData.Cells[rowStart, 3].Value = Math.Round(result, 4);
                                    }
                                    else
                                    {
                                        wrkshtVibrationData.Cells[rowStart, 3].Value = dataRow["ActualValue"].ToString();
                                    }
                                }
                                if (dataRow["UpperWarningLimit"] != null && !string.IsNullOrEmpty(dataRow["UpperWarningLimit"].ToString()))
                                {
                                    if (double.TryParse(dataRow["UpperWarningLimit"].ToString(), out result))
                                    {
                                        wrkshtVibrationData.Cells[rowStart, 4].Value = result;
                                    }
                                    else
                                    {
                                        wrkshtVibrationData.Cells[rowStart, 4].Value = dataRow["UpperWarningLimit"].ToString();
                                    }
                                }
                                if (dataRow["UpperErrorLimit"] != null && !string.IsNullOrEmpty(dataRow["UpperErrorLimit"].ToString()))
                                {
                                    if (double.TryParse(dataRow["UpperErrorLimit"].ToString(), out result))
                                    {
                                        wrkshtVibrationData.Cells[rowStart, 5].Value = result;
                                    }
                                    else
                                    {
                                        wrkshtVibrationData.Cells[rowStart, 5].Value = dataRow["UpperErrorLimit"].ToString();
                                    }
                                }
                                if (dataRow["Total_M_Observation"] != null && !string.IsNullOrEmpty(dataRow["Total_M_Observation"].ToString()))
                                {
                                    if (int.TryParse(dataRow["Total_M_Observation"].ToString(), out val))
                                    {
                                        wrkshtVibrationData.Cells[rowStart, 6].Value = val;
                                    }
                                    else
                                    {
                                        wrkshtVibrationData.Cells[rowStart, 6].Value = dataRow["Total_M_Observation"].ToString();
                                    }
                                }
                                if (dataRow["ApplyRuleFor_N_Observation"] != null && !string.IsNullOrEmpty(dataRow["ApplyRuleFor_N_Observation"].ToString()))
                                {
                                    if (int.TryParse(dataRow["ApplyRuleFor_N_Observation"].ToString(), out val))
                                    {
                                        wrkshtVibrationData.Cells[rowStart, 7].Value = val;
                                    }
                                    else
                                    {
                                        wrkshtVibrationData.Cells[rowStart, 7].Value = dataRow["ApplyRuleFor_N_Observation"].ToString();
                                    }
                                }
                                wrkshtVibrationData.Cells[rowStart, 8].Value = dataRow["UpdatedTS"].ToString();
                                wrkshtVibrationData.Cells[rowStart, 8].Style.Numberformat.Format = "Date";
                                rowStart++;
                            }
                            ExcelLineChart VibrationChart = wrkshtVibrationData.Drawings[0] as ExcelLineChart;
                            if (VibrationChart != null)
                            {
                                VibrationChart.Title.Text = parameter + " Graph";
                                VibrationChart.Series[0].Series = string.Format(@"'{0}'", wrkshtVibrationData.Name) + "!$E$32:$E$" + (rowStart - 1).ToString();
                                VibrationChart.Series[1].Series = string.Format(@"'{0}'", wrkshtVibrationData.Name) + "!$D$32:$D$" + (rowStart - 1).ToString();
                                VibrationChart.Series[2].Series = string.Format(@"'{0}'", wrkshtVibrationData.Name) + "!$C$32:$C$" + (rowStart - 1).ToString();
                                VibrationChart.Series[0].XSeries = string.Format(@"'{0}'", wrkshtVibrationData.Name) + "!$H$32:$H$" + (rowStart - 1).ToString();
                                VibrationChart.Series[1].XSeries = string.Format(@"'{0}'", wrkshtVibrationData.Name) + "!$H$32:$H$" + (rowStart - 1).ToString();
                                VibrationChart.Series[2].XSeries = string.Format(@"'{0}'", wrkshtVibrationData.Name) + "!$H$32:$H$" + (rowStart - 1).ToString();
                            }
                        }
                    }
                    if (excelPackage.Workbook.Worksheets.Count > 1) excelPackage.Workbook.Worksheets.Delete(firstSheet);
                    DownloadMultipleFile(destination, excelPackage.GetAsByteArray());
                    successfull = true;
                }
            }
            catch (Exception ex)
            {
                successfull = false;
                Logger.WriteErrorLog(ex.Message);
            }
            return successfull;
        }
        #endregion
    }
}