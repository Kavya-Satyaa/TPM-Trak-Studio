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

namespace Web_TPMTrakDashboard.Pitti
{
    public class GenerateReport_Pitti
    {
        static string appPath = HttpContext.Current.Server.MapPath("~/Pitti");
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

        public static string GenerateDailyChecksheetReport_Pitti(string MachineID, string Year, string Month)
        {
            string successfull = "";
            DataTable dt1 = new DataTable();
            try
            {
                string Filename = "DailyCheckListReport_Pitti.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "DailyCheckListReport_Pitti" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("MaintenanceMachineLevelDownTimePareto Report template does not exists at - " + Source);
                    successfull = "TemplateNotFound";
                }
                else
                {
                    DataTable dt = DataBaseAccess.GetCheckPointReportData_Pitti(MachineID, Convert.ToInt32(Year), Convert.ToInt32(Month), out dt1);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        FileInfo file = new FileInfo(Source);
                        ExcelPackage excel = new ExcelPackage(file);
                        
                        var MachinesList = dt.AsEnumerable().Select(x => x["MachineID"].ToString()).Distinct().ToList();
                        Image logo = Image.FromFile(HttpContext.Current.Server.MapPath("~/Pitti/Logo/") + "PittiLogo.png");
                        excel.Workbook.Worksheets.Delete("Sheet1");
                        foreach (var Machine in MachinesList)
                        {
                            int i = 1, rowStart = 7, colStart = 1;
                            ExcelWorksheet sheet = excel.Workbook.Worksheets.Add(i.ToString());
                            sheet.Name = Machine.ToString().Trim();
                            #region ---- Headers -----
                            sheet.Cells[5, 1, 6, 1].Merge = true;
                            sheet.Cells[5, 2, 6, 2].Merge = true;
                            sheet.Cells[5, 3, 6, 3].Merge = true;
                            sheet.Cells[5, 4, 6, 4].Merge = true;
                            sheet.Cells[5, 1].Value = "SL NO.";
                            sheet.Cells[5, 2].Value = "CHECK POINTS";
                            sheet.Cells[5, 3].Value = "Standard";
                            sheet.Cells[5, 4].Value = "Frequency";
                            var Days = dt.Columns.Count - 5;
                            sheet.Cells[5, 5, 5, 4 + Days].Merge = true;
                            sheet.Cells[5, 5].Value = "Date -";
                            sheet.Cells[5, 1, 5, 5].Style.Font.Bold = true;
                            int HeaderColStart = 5;
                            for(int a = 5; a < dt.Columns.Count; a++)
                            {
                                sheet.Cells[6, HeaderColStart].Value = dt.Columns[a].ColumnName.ToString();
                                HeaderColStart++;
                            }
                            sheet.Cells[5, 1, 5, 4 + Days].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet.Cells[4, 1, 4, 10].Merge = true;
                            sheet.Cells[4, 1].Value = "MACHINE: " + dt1.AsEnumerable().Where(x => x["MachineID"].ToString().Equals(Machine, StringComparison.OrdinalIgnoreCase)).Select(x => x["Description"].ToString()).FirstOrDefault();
                            sheet.Cells[4, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            sheet.Cells[4, 11, 4, 4 + Days].Merge = true;
                            sheet.Cells[4,11].Value = "MACHINE NO: " + Machine.ToString();
                            sheet.Cells[4, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            sheet.Cells[4, 1, 4, 4 + Days].Style.Font.Size = 11;
                            sheet.Cells[4, 1, 4, 4 + Days].Style.Font.Bold = true;

                            sheet.Cells[3, 2, 3, 23].Merge = true;
                            sheet.Cells[3,2].Value = "DAILY MAINTENANCE CHECK LIST FOR THE MONTH OF : " + HelperClassGeneric.getAbbreviatedMonthName(Month);
                            sheet.Cells[3, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet.Cells[3, 2].Style.Font.Size = 13;
                            sheet.Cells[3,2].Style.Font.Bold = true;

                            sheet.Cells[2, 2, 2, 23].Merge = true;
                            sheet.Cells[2, 2].Value = "MAINTENNACE DEPARTMENT - MACHINE SHOP";
                            sheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet.Cells[2, 2].Style.Font.Size = 15;
                            sheet.Cells[2, 2].Style.Font.Bold = true;

                            sheet.Cells[1, 2, 1, 23].Merge = true;
                            sheet.Cells[1, 2].Value = "PITTI ENGINEERING LIMITED, PLANT - II";
                            sheet.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet.Cells[1, 2].Style.Font.Size = 16;
                            sheet.Cells[1, 2].Style.Font.Bold = true;

                            sheet.Cells[1, 24, 2, 4 + Days].Merge = true;
                            sheet.Cells[1, 24].Value = "Ref. No: " + dt1.AsEnumerable().Where(x => x["MachineID"].ToString().Equals(Machine, StringComparison.OrdinalIgnoreCase)).Select(x => x["RefNo"].ToString()).FirstOrDefault() + "\n Rev. No: " + dt1.AsEnumerable().Where(x => x["MachineID"].ToString().Equals(Machine, StringComparison.OrdinalIgnoreCase)).Select(x => x["RevNo"].ToString()).FirstOrDefault();

                            var picture = sheet.Drawings.AddPicture("LOGO", logo);
                            picture.SetPosition(0, 0, 0, 0);


                            #endregion

                            var Machine_row = dt1.AsEnumerable().Where(x => x["MachineID"].ToString().Equals(Machine, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                            DataTable Machine_dt = dt.AsEnumerable().Where(x => x["MachineID"].ToString().Equals(Machine, StringComparison.OrdinalIgnoreCase)).CopyToDataTable();
                            int LastRow = Machine_dt.Rows.Count + rowStart;
                            Machine_dt.AsEnumerable().OrderBy(x => Convert.ToInt32(x["SerialNo"].ToString().Trim()));
                            foreach (DataRow row in Machine_dt.Rows)
                            {
                                colStart = 1;
                                sheet.Cells[rowStart, colStart].Value = row["SerialNo"].ToString();
                                colStart++;
                                sheet.Cells[rowStart, colStart].Value = row["CheckpointDescription"].ToString();
                                colStart++;
                                sheet.Cells[rowStart, colStart].Value = row["Standard"].ToString();
                                colStart++;
                                sheet.Cells[rowStart, colStart].Value = row["Frequency"].ToString();
                                for (int k = 5; k < dt.Columns.Count; k++)
                                {
                                    colStart++;
                                    var arr = row[k].ToString().Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                    if (arr.Count > 1)
                                    {

                                    }
                                    sheet.Cells[rowStart, colStart].Value = string.IsNullOrEmpty(row[k].ToString()) ? "" : arr[0];
                                    if (rowStart == 7 && k == 5)
                                    {
                                        sheet.Cells[LastRow, 1, LastRow, 4].Merge = true;
                                        sheet.Cells[LastRow, 1].Value = "Sign Of Operator";
                                        sheet.Cells[LastRow + 1, 1, LastRow + 1, 4].Merge = true;
                                        sheet.Cells[LastRow + 1, 1].Value = "Sign Of Supervisor";
                                    }
                                    sheet.Cells[LastRow, colStart].Value = string.IsNullOrEmpty(row[k].ToString()) ? "" : arr[1];
                                    sheet.Cells[LastRow + 1, colStart].Value = Machine_row["SupervisorID"].ToString();
                                }
                                rowStart++;
                            }
                            sheet.Column(2).Width = Convert.ToDouble(40);
                            sheet.Cells[7, 2, rowStart - 1, 2].Style.WrapText = true;
                            sheet.Cells[7, 2, rowStart - 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            sheet.Column(3).Width = Convert.ToDouble(10);
                            sheet.Cells[7, 3, rowStart - 1, 3].Style.WrapText = true;
                            sheet.Cells[7, 3, rowStart - 1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            for(int b = 7; b < rowStart; b++)
                            {
                                sheet.Row(b).Height = Convert.ToDouble(45);
                            }
                            setBorderThin(sheet, 1, 1, LastRow + 1, 4 + Days);
                        }

                        DownloadMultipleFile(destination, excel.GetAsByteArray());
                        successfull = "Generated";
                    }
                    else
                    {
                        successfull = "NoData";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return successfull;
        }
    }
}