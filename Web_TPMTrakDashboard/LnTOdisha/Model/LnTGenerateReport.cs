using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Web_TPMTrakDashboard.Models;
using static Web_TPMTrakDashboard.LnTOdisha.Model.LnTOdishaDTO;

namespace Web_TPMTrakDashboard.LnTOdisha.Model
{
    public class LnTGenerateReport
    {
        static string appPath = HttpContext.Current.Server.MapPath("~/LnTOdisha/ReportTemplate");
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
        internal static string GeneratePMReport(string machine, string year, string FromDate, string ToDate)
        {
            string generated = "";
            DataTable dt = new DataTable();
            DataTable dt_HeaderData = new DataTable();
            DataTable dt_LastChecked = new DataTable();
            int i = 0;
            try
            {
                string Filename = "PMReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "PMReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("PMReport template does not found at - \n " + Source);
                    generated = "TemplateNotFound";
                }
                else
                {
                    dt = LnTOdishaDBAccess.getPMReportDetails(machine, year, FromDate, ToDate, out dt_HeaderData, out dt_LastChecked);
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets["Sheet1"];
                    int rowpos = 8;
                    int col = 1;
                    if (dt.Rows.Count > 0 && dt.Columns.IndexOf("SaveFlag") == -1)
                    {
                        string PrevCategory = dt.Rows[0]["Category"].ToString();
                        string CurCategory = "";
                        worksheet.Cells[rowpos, 1].Value = PrevCategory;
                        worksheet.Cells[rowpos, 1].Style.Font.Bold = true;
                        worksheet.Cells[rowpos, 1].Style.Font.UnderLineType = ExcelUnderLineType.Single;
                        worksheet.Cells[rowpos, 1].Style.Font.UnderLine = true;
                        worksheet.Cells[1, 1].Value = machine;
                        worksheet.Cells[2, 1].Value = "Machine No:- " + dt.Rows[0]["MachineInterfaceID"].ToString();
                        worksheet.Cells[1, 3, 1, 9].Merge = true;
                        worksheet.Cells[1, 3].Value = "PM ACTIVITY CARRIED OUT " + (string.IsNullOrEmpty(year) ? $"Between {FromDate} and {ToDate}" : $"IN FY-{year}");
                        rowpos++;
                        foreach (DataRow row in dt.Rows)
                        {
                            CurCategory = row["Category"].ToString();
                            //var CategoryList = 
                            if (PrevCategory != CurCategory)
                            {
                                worksheet.Cells[rowpos, 1, rowpos, 2].Merge = true;
                                worksheet.Cells[rowpos, 1].Value = CurCategory;
                                worksheet.Cells[rowpos, 1].Style.Font.Bold = true;
                                worksheet.Cells[rowpos, 1].Style.Font.UnderLineType = ExcelUnderLineType.Single;
                                worksheet.Cells[rowpos, 1].Style.Font.UnderLine = true;

                                PrevCategory = CurCategory;
                                rowpos++;
                            }
                            col = 7;
                            for (int j = 8; j < dt.Columns.Count - 1; j++)
                            {
                                if (i == 0)
                                {
                                    string ColumnName = dt.Columns[j].ColumnName;
                                    //var HeaderList = ColumnName.Split('_').ToList();
                                    //if (HeaderList != null)
                                    {
                                        var HeaderDetails = dt_HeaderData.AsEnumerable().Where(x => Util.GetDateTime(x["PMDate"].ToString()).ToString("yyyy-MM-dd") == ColumnName).Select(x => new { TLName = x["TeamLeader"].ToString(), CrewMemberName = x["CrewMemberName"].ToString(), PMStatus = x["PMType"].ToString() }).FirstOrDefault();

                                        worksheet.Cells[3, col].Value = HelperClassGeneric.getAbbreviatedMonthName(Util.GetDateTime(ColumnName).Month.ToString());
                                        //PM PLan Date
                                        worksheet.Cells[4, col].Value = HeaderDetails.PMStatus.Equals("NoPlan", StringComparison.OrdinalIgnoreCase) ? "" : Util.GetDateTime(ColumnName).ToString("dd-MM-yyyy");
                                        //PM Done Date
                                        //worksheet.Cells[5, col].Value = HeaderDetails.PMStatus.Equals("NoPlan", StringComparison.OrdinalIgnoreCase) ? "" : (string.IsNullOrEmpty(ColumnName) ? "" : Util.GetDateTime(ColumnName).ToString("dd-MM-yyyy"));

                                        worksheet.Cells[3, col, 4, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        worksheet.Cells[3, col, 4, col].Style.Font.Bold = true;
                                        worksheet.Cells[5, col].Value = HeaderDetails.TLName;
                                        worksheet.Cells[6, col].Value = HeaderDetails.CrewMemberName;
                                    }

                                }
                                worksheet.Cells[rowpos, col].Value = string.IsNullOrEmpty(row[j].ToString()) ? "" : row[j].ToString();
                                col++;
                            }

                            worksheet.Cells[rowpos, 1].Value = (++i).ToString(); //row["SINO"].ToString();
                            worksheet.Cells[rowpos, 2].Value = row["ActivityName"].ToString();
                            worksheet.Cells[rowpos, 3].Value = row["AllotedTime"].ToString();
                            worksheet.Cells[rowpos, 4].Value = row["Frequency"].ToString();
                            worksheet.Cells[rowpos, 5].Value = string.IsNullOrEmpty(row["LastChecked"].ToString()) ? "" : row["LastChecked"].ToString();
                            worksheet.Cells[rowpos, 6].Value = ""; // row["TodayPlan"].ToString();
                            rowpos++;
                        }
                        worksheet.Cells[4, 5].Value = dt_LastChecked.Rows.Count > 0 ? Util.GetDateTime(dt_LastChecked.Rows[0]["LastCheckedDate"].ToString()).ToString("dd-MM-yyyy") : "";
                        worksheet.Cells[5, 5].Value = dt_LastChecked.Rows.Count > 0 ? dt_LastChecked.Rows[0]["LastCheckedTeamLeader"].ToString() : "";
                        worksheet.Cells[6, 5].Value = dt_LastChecked.Rows.Count > 0 ? dt_LastChecked.Rows[0]["LastCheckedCrewMemberName"].ToString() : "";
                        for (int a = 1; a < col; a++)
                            worksheet.Column(a).AutoFit();
                        setThinBorder(worksheet, 1, 1, rowpos - 1, col - 1);
                        DownloadMultipleFile(destination, Excel.GetAsByteArray());
                        generated = "Generated";
                    }

                    else
                        generated = "NodataFound";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return generated;
        }

        internal static string GeneratePMActivityYearlyGenerationReport(string Year, string MachineID)
        {
            string generated = "";
            DataTable dt = new DataTable();
            DataTable dt_Status = new DataTable();
            string PrevYear = "", NextYear = "";
            try
            {
                string Filename = "PMActivityGenerationReport_LnTOdisha.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "PMActivityGenerationReport_LnTOdisha" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("PMActivityGenerationReport_LnTOdisha template does not found at - \n " + Source);
                    generated = "TemplateNotFound";
                }
                else
                {
                    dt = LnTOdishaDBAccess.getPMGenerationYearlySummaryDetails(Year, "", MachineID, out dt_Status);
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[1];
                    int rowpos = 4, col = 1, j = 1;

                    if (dt.Rows.Count > 0 && dt.Columns.IndexOf("SaveFlag") == -1)
                    {
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            col = 1;
                            worksheet.Cells[rowpos, col].Value = (j).ToString();
                            col++;
                            worksheet.Cells[rowpos, col].Value = dt.Rows[k]["MachineID"].ToString();
                            col++;
                            for (int i = 1; i < dt.Columns.Count; i++)
                            {

                                string PlanStatus = "", PlanDate = "";
                                if (j == 1)
                                {
                                    var arr = dt.Columns[i].ColumnName.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                                    worksheet.Cells[3, col].Value = HelperClassGeneric.getAbbreviatedMonthName(arr[0].Trim());
                                    if (i == 1)
                                        PrevYear = NextYear = arr[1].ToString().Trim();
                                    else
                                    {
                                        NextYear = arr[1].ToString().Trim();

                                    }
                                }

                                if (dt_Status.Rows[k][i].ToString().Equals("no plan", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (string.IsNullOrEmpty(dt.Rows[k][i].ToString()))
                                        PlanStatus = "X";
                                    PlanDate = "";
                                }
                                else if (string.IsNullOrEmpty(dt_Status.Rows[k][i].ToString()))
                                {
                                    PlanStatus = "";
                                    PlanDate = "";
                                }
                                else
                                {
                                    PlanDate = "(" + Util.GetDateTime(dt.Rows[k][i].ToString()).ToString("dd-MM-yyyy") + ")";
                                    PlanStatus = dt_Status.Rows[k][i].ToString();
                                }

                                worksheet.Cells[rowpos, col++].Value = (PlanStatus + "            " + PlanDate).Trim();
                            }
                            rowpos++;
                            j++;
                        }
                        worksheet.Column(2).Width = (double)(20);
                        for (int a = 3; a < col; a++)
                        {
                            worksheet.Column(a).Width = (double)(15);
                            worksheet.Column(a).Style.WrapText = true;
                            worksheet.Column(a).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        for (int b = 4; b < rowpos; b++)
                        {
                            worksheet.Row(b).Height = (double)(48);
                        }
                        worksheet.Cells[1, 1].Value = "PM ACTIVITY GENERATION DETAILS FOR THE YEAR: " + PrevYear + "-" + NextYear;

                        setThinBorder(worksheet, 1, 1, rowpos - 1, col - 1);
                        DownloadMultipleFile(destination, Excel.GetAsByteArray());
                        generated = "Generated";
                    }
                    else
                    {
                        generated = "No Data Found";
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GeneratePMActivityYearlyGenerationReport= " + ex.Message);
            }
            return generated;
        }

    }
}