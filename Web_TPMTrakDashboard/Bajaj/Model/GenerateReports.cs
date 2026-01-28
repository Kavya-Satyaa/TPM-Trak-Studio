using Elmah;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Bajaj.Model
{
    public class GenerateReports
    {
        static readonly string appPath = HttpContext.Current.Server.MapPath("~/Bajaj/Reports");

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
        internal static bool GenerateJHReport(string plant, string group, string machine, string year, string month, string revno, string revdate, string manager, string grpleader)
        {
            bool successfull = false;
            try
            {
                string Filename = "JHReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "JHReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("JH Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    List<JHReportDetails> dailyDetails = new List<JHReportDetails>();
                    List<JHReportDetails> weeklyDetails = new List<JHReportDetails>();
                    List<JHReportDetails> quaterlyDetails = new List<JHReportDetails>();
                    if (HttpContext.Current.Session["JHDailyDetails"] != null)
                    {
                        dailyDetails = (List<JHReportDetails>)HttpContext.Current.Session["JHDailyDetails"];
                    }
                    if (HttpContext.Current.Session["JHWeeklyDetails"] != null)
                    {
                        weeklyDetails = (List<JHReportDetails>)HttpContext.Current.Session["JHWeeklyDetails"];
                    }
                    if (HttpContext.Current.Session["JHQuaterlyDetails"] != null)
                    {
                        quaterlyDetails = (List<JHReportDetails>)HttpContext.Current.Session["JHQuaterlyDetails"];
                    }
                    string interfaceid = DataBaseAccess.getMachineinterdaceid(machine);
                    int rowStart = 5;
                    int rowStartCopy = rowStart;
                    int colStart = 1;
                    int noOfColumn = 40;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var wsDnWDetails = Excel.Workbook.Worksheets[1];
                    wsDnWDetails.Cells["C2"].Value = group;
                    wsDnWDetails.Cells["C3"].Value = machine;
                    wsDnWDetails.Cells["G3"].Value = interfaceid;
                    wsDnWDetails.Cells["K2"].Value = month;
                    wsDnWDetails.Cells["K3"].Value = manager;
                    wsDnWDetails.Cells["Q3"].Value = grpleader;
                    wsDnWDetails.Cells["W2"].Value = revno;
                    wsDnWDetails.Cells["W3"].Value = revdate;


                    #region Deaily Details
                    for (int i = 0; i < dailyDetails.Count; i++)
                    {
                        if (i == 0)
                        {
                            HeaderSet(wsDnWDetails, rowStart, colStart, "Check Point No.");
                            colStart++;
                            HeaderSet(wsDnWDetails, rowStart, colStart, "Route No.");
                            colStart++;
                            HeaderSet(wsDnWDetails, rowStart, colStart, "Item");
                            colStart++;
                            HeaderSet(wsDnWDetails, rowStart, colStart, "Check Point");
                            colStart++;
                            HeaderSet(wsDnWDetails, rowStart, colStart, "Standard");
                            colStart++;
                            HeaderSet(wsDnWDetails, rowStart, colStart, "If Not Ok");
                            colStart++;
                            HeaderSet(wsDnWDetails, rowStart, colStart, "Method");
                            colStart++;
                            HeaderSet(wsDnWDetails, rowStart, colStart, "Freq.");
                            colStart++;
                            HeaderSet(wsDnWDetails, rowStart, colStart, "Day");
                            colStart++;
                            for (int j = 0; j < dailyDetails[i].FrequencyDetails.Count; j++)
                            {
                                HeaderSet(wsDnWDetails, rowStart, colStart, dailyDetails[i].FrequencyDetails[j].Value);
                                colStart++;
                            }
                            rowStart++;
                            noOfColumn = colStart - 1;
                            continue;
                        }
                        colStart = 1;
                        wsDnWDetails.Cells[rowStart, colStart].Value = dailyDetails[i].CheckPointNo;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = dailyDetails[i].RouteNo;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = dailyDetails[i].Item;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = dailyDetails[i].CheckPoint;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = dailyDetails[i].Standard;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = dailyDetails[i].IfNotOk;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = dailyDetails[i].Method;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = dailyDetails[i].Frequency;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = dailyDetails[i].DayToDisplay;
                        colStart++;
                        for (int j = 0; j < dailyDetails[i].FrequencyDetails.Count; j++)
                        {
                            string value = dailyDetails[i].FrequencyDetails[j].Value;
                            wsDnWDetails.Cells[rowStart, colStart].Value = (value == "." ? "" : value);
                            colStart++;
                        }

                        rowStart++;
                    }
                    #endregion

                    #region Weekly Details
                    for (int i = 0; i < weeklyDetails.Count; i++)
                    {
                        colStart = 1;
                        wsDnWDetails.Cells[rowStart, colStart].Value = weeklyDetails[i].CheckPointNo;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = weeklyDetails[i].RouteNo;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = weeklyDetails[i].Item;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = weeklyDetails[i].CheckPoint;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = weeklyDetails[i].Standard;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = weeklyDetails[i].IfNotOk;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = weeklyDetails[i].Method;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = weeklyDetails[i].Frequency;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = weeklyDetails[i].DayToDisplay;
                        colStart++;
                        for (int j = 0; j < weeklyDetails[i].FrequencyDetails.Count; j++)
                        {
                            string value = weeklyDetails[i].FrequencyDetails[j].Value;
                            wsDnWDetails.Cells[rowStart, colStart].Value = (value == "." ? "" : value);
                            wsDnWDetails.Cells[rowStart, colStart, rowStart, colStart = colStart + weeklyDetails[i].FrequencyDetails[j].MergeColumnNo - 1].Merge = true;
                            colStart++;

                        }

                        rowStart++;
                    }
                    #endregion

                    if (dailyDetails.Count > 0 || weeklyDetails.Count > 0)
                    {
                        wsDnWDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].AutoFitColumns();
                        wsDnWDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        wsDnWDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        wsDnWDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        wsDnWDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }


                    var wsQDetails = Excel.Workbook.Worksheets[2];
                    wsQDetails.Cells["C2"].Value = group;
                    wsQDetails.Cells["C3"].Value = machine;
                    wsQDetails.Cells["G3"].Value = interfaceid;
                    wsQDetails.Cells["K2"].Value = year;
                    wsQDetails.Cells["K3"].Value = manager;
                    wsQDetails.Cells["Q3"].Value = grpleader;
                    wsQDetails.Cells["W2"].Value = revno;
                    wsQDetails.Cells["W3"].Value = revdate;
                    rowStart = 5;
                    rowStartCopy = rowStart;
                    colStart = 1;
                    noOfColumn = 40;

                    #region Quaterly Details
                    for (int i = 0; i < quaterlyDetails.Count; i++)
                    {
                        if (i == 0)
                        {
                            HeaderSet(wsQDetails, rowStart, colStart, "Check Point No.");
                            colStart++;
                            HeaderSet(wsQDetails, rowStart, colStart, "Route No.");
                            colStart++;
                            HeaderSet(wsQDetails, rowStart, colStart, "Item");
                            colStart++;
                            HeaderSet(wsQDetails, rowStart, colStart, "Check Point");
                            colStart++;
                            HeaderSet(wsQDetails, rowStart, colStart, "Standard");
                            colStart++;
                            HeaderSet(wsQDetails, rowStart, colStart, "If Not Ok");
                            colStart++;
                            HeaderSet(wsQDetails, rowStart, colStart, "Method");
                            colStart++;
                            HeaderSet(wsQDetails, rowStart, colStart, "Freq.");
                            colStart++;
                            HeaderSet(wsQDetails, rowStart, colStart, "Day");
                            colStart++;
                            for (int j = 0; j < quaterlyDetails[i].FrequencyDetails.Count; j++)
                            {
                                HeaderSet(wsQDetails, rowStart, colStart, quaterlyDetails[i].FrequencyDetails[j].Value);
                                colStart++;
                            }
                            rowStart++;
                            noOfColumn = colStart - 1;
                            continue;
                        }
                        colStart = 1;
                        wsQDetails.Cells[rowStart, colStart].Value = quaterlyDetails[i].CheckPointNo;
                        colStart++;
                        wsQDetails.Cells[rowStart, colStart].Value = quaterlyDetails[i].RouteNo;
                        colStart++;
                        wsQDetails.Cells[rowStart, colStart].Value = quaterlyDetails[i].Item;
                        colStart++;
                        wsQDetails.Cells[rowStart, colStart].Value = quaterlyDetails[i].CheckPoint;
                        colStart++;
                        wsQDetails.Cells[rowStart, colStart].Value = quaterlyDetails[i].Standard;
                        colStart++;
                        wsQDetails.Cells[rowStart, colStart].Value = quaterlyDetails[i].IfNotOk;
                        colStart++;
                        wsQDetails.Cells[rowStart, colStart].Value = quaterlyDetails[i].Method;
                        colStart++;
                        wsQDetails.Cells[rowStart, colStart].Value = quaterlyDetails[i].Frequency;
                        colStart++;
                        wsQDetails.Cells[rowStart, colStart].Value = quaterlyDetails[i].DayToDisplay;
                        colStart++;
                        for (int j = 0; j < quaterlyDetails[i].FrequencyDetails.Count; j++)
                        {
                            string value = quaterlyDetails[i].FrequencyDetails[j].Value;
                            wsQDetails.Cells[rowStart, colStart].Value = (value == "." ? "" : value);
                            wsQDetails.Cells[rowStart, colStart, rowStart, colStart = colStart + quaterlyDetails[i].FrequencyDetails[j].MergeColumnNo - 1].Merge = true;
                            colStart++;

                        }

                        rowStart++;
                    }
                    #endregion
                    if (quaterlyDetails.Count > 0)
                    {
                        wsQDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].AutoFitColumns();
                        wsQDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        wsQDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        wsQDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        wsQDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                   

                    //System.Drawing.Image img = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(getGEALogoPath()));
                    //ExcelPicture pic = wrkshtYearlyMaintenanceChklist.Drawings.AddPicture("geaLogo", img);
                    //pic.SetPosition(0, 5, 0, 5);
                    //pic.SetSize(170, 70);
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    successfull = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GenerateJHReport: " + ex.Message);

            }
            return successfull;
        }

        internal static bool GenerateOperatorMessageReport(string plant, string machine, string fromDate, string toDate)
        {
            bool successfull = false;
            try
            {
                string Filename = "OperatorMessage.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "OperatorMessage" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Operator Message template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    List<OperatorMessageDetails> oprMdgDetails = new List<OperatorMessageDetails>();
                    if (HttpContext.Current.Session["OperatorMesssageDetails"] != null)
                    {
                        oprMdgDetails = (List<OperatorMessageDetails>)HttpContext.Current.Session["OperatorMesssageDetails"];
                    }
                   
                    int rowStart = 9;
                    int rowStartCopy = rowStart-1;
                    int colStart = 1;
                    int noOfColumn = 4;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var wsDnWDetails = Excel.Workbook.Worksheets[1];
                    wsDnWDetails.Cells["B4"].Value = plant;
                    wsDnWDetails.Cells["D4"].Value = machine;
                    wsDnWDetails.Cells["B6"].Value = fromDate;
                    wsDnWDetails.Cells["D6"].Value = toDate;
                    for (int i = 0; i < oprMdgDetails.Count; i++)
                    {
                        colStart = 1;
                        wsDnWDetails.Cells[rowStart, colStart].Value = oprMdgDetails[i].AlarmNo;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = oprMdgDetails[i].AlarmDate;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = oprMdgDetails[i].AlarmMessage;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = oprMdgDetails[i].GroupNo;
                        rowStart++;
                    }
                   

                    wsDnWDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].AutoFitColumns();
                    wsDnWDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    wsDnWDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    wsDnWDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    wsDnWDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                 
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    successfull = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GenerateOperatorMessageReport: " + ex.Message);

            }
            return successfull;
        }

        internal static bool GenerateFocasToolLifeReport(string plant, string machine, string fromDate, string toDate)
        {
            bool successfull = false;
            try
            {
                string Filename = "ToolChangeReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "ToolChangeReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "ToolChangeReport", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Focas Tool Life template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    List<FocasToolLifeDetails> toolifeDetails = new List<FocasToolLifeDetails>();
                    if (HttpContext.Current.Session["ToolLifeDetails"] != null)
                    {
                        toolifeDetails = (List<FocasToolLifeDetails>)HttpContext.Current.Session["ToolLifeDetails"];
                    }

                    int rowStart = 6;
                    int rowStartCopy = rowStart-1;
                    int colStart = 1;
                    int noOfColumn = 11;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var wsDnWDetails = Excel.Workbook.Worksheets[1];
                    wsDnWDetails.Cells["B3"].Value = fromDate;
                    wsDnWDetails.Cells["G3"].Value = toDate;
                    for (int i = 0; i < toolifeDetails.Count; i++)
                    {
                        colStart = 1;
                        wsDnWDetails.Cells[rowStart, colStart].Value = toolifeDetails[i].MachineID;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = toolifeDetails[i].ProgramNo;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = toolifeDetails[i].ToolNo;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = toolifeDetails[i].ToolDesc;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = toolifeDetails[i].NoOfTimeChanged;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = toolifeDetails[i].ChangeTime;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = toolifeDetails[i].Type;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = toolifeDetails[i].ToolTarget;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = toolifeDetails[i].ToolActual;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = toolifeDetails[i].RemainingToolLife;
                        colStart++;
                        wsDnWDetails.Cells[rowStart, colStart].Value = toolifeDetails[i].PartsCount;
                        colStart++;
                        rowStart++;
                    }


                    wsDnWDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].AutoFitColumns();
                    wsDnWDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    wsDnWDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    wsDnWDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    wsDnWDetails.Cells[rowStartCopy, 1, rowStart - 1, noOfColumn].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    successfull = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GenerateFocasToolLifeReport: " + ex.Message);

            }
            return successfull;
        }
        private static void HeaderSet(ExcelWorksheet ws, int row, int col, string value)
        {
            ws.Cells[row, col].Value = value;
            ws.Cells[row, col].Style.Font.Size = 11;
            ws.Cells[row, col].Style.Font.Bold = true;
            //ws.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
        }
    }
}