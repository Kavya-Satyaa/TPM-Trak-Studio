using ChartDirector;
using Elmah;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Web_TPMTrakDashboard.Advik.DataBaseAccess;
using Web_TPMTrakDashboard.Advik.Models;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.WebAndon;

namespace Web_TPMTrakDashboard.Advik
{
    public class AdvikGenerateReports
    {
        internal static void setWorkSheetSetting(ExcelWorksheet wksheet)
        {
            wksheet.PrinterSettings.Orientation = eOrientation.Landscape;
            wksheet.PrinterSettings.FitToPage = true;
            wksheet.PrinterSettings.FitToWidth = 1;
            wksheet.PrinterSettings.FitToHeight = 0;

        }
        static readonly string appPath = HttpContext.Current.Server.MapPath("~/Advik");
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
        public static string SafeFileName(string name)
        {
            StringBuilder str = new StringBuilder(name);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.ToString();
        }

        #region --- JHDashboard report----
        internal static bool GenerateJHDashboardReport(string machineID, string shift, string JHType, DateTime fromDate, DateTime todate, List<JHDashboardDetails> listJHDetails)
        {
            bool successfull = false;
            try
            {
                string Filename = "JHDashboardReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "JHDashboardReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("JH Dashboard Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    int rowStart;
                    int colStart = 1;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    //Excel.Workbook.Worksheets.Delete("Sheet1");
                    if (listJHDetails != null && listJHDetails.Count > 0)
                    {

                        var workSheet = Excel.Workbook.Worksheets[1];
                        setWorkSheetSetting(workSheet);
                        rowStart = 4;
                        string machinename = workSheet.Cells["A1"].Value.ToString() + ": " + machineID;
                        workSheet.Cells["A1"].Value = machinename;
                        if (machinename.Length > 86)
                        {
                            workSheet.Cells["A1"].Style.WrapText = true;
                            //workSheet.Column(1).Width = 200;
                            decimal noOfRows = Math.Ceiling((decimal)machinename.Length / 86);
                            int noOfRowsinInt = (int)Math.Round(noOfRows, 0);
                            workSheet.Row(1).Height = noOfRowsinInt * 13;
                        }
                        workSheet.Cells["H1"].Value = workSheet.Cells["H1"].Value.ToString() + ": " + shift;
                        workSheet.Cells["I1"].Value = workSheet.Cells["I1"].Value.ToString() + ": " + JHType;
                        workSheet.Cells["A3"].Value = workSheet.Cells["A3"].Value.ToString() + ": " + fromDate.ToString("dd-MM-yyyy");
                        workSheet.Cells["C3"].Value = workSheet.Cells["C3"].Value.ToString() + ": " + todate.ToString("dd-MM-yyyy");
                        for (int i = 0; i < listJHDetails.Count; i++)
                        {
                            if (i == 0)
                            {
                                workSheet.Cells[rowStart, colStart].Value = "Date";
                                workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = "Shift";
                                workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = "Machine ID";
                                workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                                colStart++;
                                //workSheet.Cells[rowStart, colStart].Value = "JH Activity";
                                //workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                                //workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                //workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                                workSheet.Cells[rowStart, colStart].Value = "Mc Area";
                                workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = "Location";
                                workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = "Std Condition";
                                workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = "Checking Method";
                                workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = "JH Type";
                                workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = "Status";
                                workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                                colStart++;
                                rowStart++;
                            }
                            colStart = 1;
                            workSheet.Cells[rowStart, colStart].Value = listJHDetails[i].Date;
                            // workSheet.Column(colStart).AutoFit();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listJHDetails[i].Shift;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listJHDetails[i].Machine;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            //workSheet.Cells[rowStart, colStart].Value = listJHDetails[i].JHActivity;
                            //workSheet.Column(colStart).AutoFit();
                            //colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listJHDetails[i].McArea;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listJHDetails[i].Location;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listJHDetails[i].StdCondition;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listJHDetails[i].CheckingMethod;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listJHDetails[i].JHType;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listJHDetails[i].Status;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            rowStart++;
                        }
                        int colForBorder = 9;
                        workSheet.Cells[4, 1, rowStart - 1, colForBorder].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[4, 1, rowStart - 1, colForBorder].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[4, 1, rowStart - 1, colForBorder].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[4, 1, rowStart - 1, colForBorder].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[4, 1, rowStart - 1, colForBorder].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        workSheet.Cells[4, 1, rowStart - 1, colForBorder].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        workSheet.Cells[4, 1, rowStart - 1, colForBorder].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                        workSheet.Cells[4, 1, rowStart - 1, colForBorder].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    }
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    successfull = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return successfull;
        }

        internal static string JHCheckListReport(string plantID, string cellID, string machineID, string Shift, DateTime fromDate)
        {
            string Generated = string.Empty;
            try
            {
                string Filename = "JHCheckListReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "JHCheckListReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("JHCheckListReport Report template does not exists at - " + Source);
                    Generated = "";
                }
                else
                {
                    FileInfo newFile = new FileInfo(Source);
                    int worsheetcount = 2;
                    DateTime Enddate = fromDate.AddMonths(1);
                    int totaldays = (Enddate - fromDate).Days;
                    int i = 1;
                    List<string> MachineIDList = new List<string>();
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var MainWorkSheet = Excel.Workbook.Worksheets[1];
                    if (machineID.Equals("All"))
                        MachineIDList = DataBaseAccess.AdvikDatabaseAccess.GetMachinesbyPlantCell(plantID, cellID);
                    else
                        MachineIDList.Add(machineID);
                    foreach (string Machine in MachineIDList)
                    {
                        System.Data.DataSet ChecklistDataset = AdvikDatabaseAccess.Getchecklistdata(Machine, Shift, fromDate);
                        if (ChecklistDataset != null && ChecklistDataset.Tables.Count > 0)
                        {
                            for (int table = 0; table < ChecklistDataset.Tables.Count; table = table + 2)
                            {
                                int col = 1;
                                DataTable dtshiftval = ChecklistDataset.Tables[table];
                                int Row = 8;
                                DataTable dtoprsupval = ChecklistDataset.Tables[table + 1];
                                if (dtoprsupval != null && dtshiftval != null && dtshiftval.Rows.Count > 0 && dtshiftval.Rows.Count > 0)
                                {
                                    Excel.Workbook.Worksheets.Add(Machine + " ( " + dtoprsupval.Rows[0]["ShiftName"].ToString() + " )", MainWorkSheet);
                                    var workSheet = Excel.Workbook.Worksheets[worsheetcount]; worsheetcount++;
                                    if (dtshiftval != null && dtshiftval.Rows.Count > 0)
                                    {
                                        foreach (DataRow dataRow in dtshiftval.Rows)
                                        {
                                            workSheet.Cells[Row, 1].Value = col++;
                                            workSheet.Cells[Row, 2].Value = dataRow["McArea"];
                                            workSheet.Cells[Row, 3].Value = dataRow["Location"];
                                            workSheet.Cells[Row, 4].Value = dataRow["Item"];
                                            workSheet.Cells[Row, 5].Value = dataRow["CheckFor"];
                                            workSheet.Cells[Row, 6].Value = dataRow["StdCondition"];
                                            workSheet.Cells[Row, 7].Value = dataRow["CheckingMethod"];
                                            workSheet.Cells[Row, 8].Value = dataRow["AffectOnQ"];
                                            while (i <= totaldays)
                                            {
                                                workSheet.Cells[Row, (i + 8)].Value = dataRow[i.ToString()]; i++;
                                            }
                                            i = 1;
                                            Row++;
                                        }
                                        workSheet.Cells[8, 1, Row - 1, 39].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        workSheet.Cells[8, 1, Row - 1, 39].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        workSheet.Cells[8, 1, Row - 1, 39].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        workSheet.Cells[8, 1, Row - 1, 39].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        workSheet.Cells[8, 1, Row - 1, 39].Style.Border.Top.Color.SetColor(Color.Black);
                                        workSheet.Cells[8, 1, Row - 1, 39].Style.Border.Bottom.Color.SetColor(Color.Black);
                                        workSheet.Cells[8, 1, Row - 1, 39].Style.Border.Right.Color.SetColor(Color.Black);
                                        workSheet.Cells[8, 1, Row - 1, 39].Style.Border.Left.Color.SetColor(Color.Black);
                                        workSheet.Cells[8, 1, Row - 1, 39].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        workSheet.Cells[8, 1, Row - 1, 39].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#C6E0B4"));
                                        workSheet.Cells[Row, 8].Value = "Operator";
                                        workSheet.Cells[Row + 1, 8].Value = "Supervisor";
                                        workSheet.Cells[Row + 2, 8].Value = "Production Head";
                                        string ProdHead = string.Empty;
                                        string ProdHeadTS = string.Empty;

                                        #region HardCoded
                                        //int adddays = 0, firstadd = 0; ;
                                        //List<string> supval = new List<string>();
                                        //DataTable newdt = new DataTable();
                                        //DayOfWeek weekstart = fromDate.DayOfWeek;
                                        //string name = string.Empty;
                                        //switch (weekstart)
                                        //{
                                        //    case DayOfWeek.Monday:
                                        //        firstadd = 3;
                                        //        adddays = 4;
                                        //        break;
                                        //    case DayOfWeek.Tuesday:
                                        //        firstadd = 2;
                                        //        adddays = 4;
                                        //        break;
                                        //    case DayOfWeek.Wednesday:
                                        //        firstadd = 1;
                                        //        adddays = 4;
                                        //        break;
                                        //    case DayOfWeek.Thursday:
                                        //        firstadd = 4;
                                        //        adddays = 3;
                                        //        break;
                                        //    case DayOfWeek.Friday:
                                        //        firstadd = 3;
                                        //        adddays = 3;
                                        //        break;
                                        //    case DayOfWeek.Saturday:
                                        //        firstadd = 2;
                                        //        adddays = 3;
                                        //        break;
                                        //    case DayOfWeek.Sunday:
                                        //        firstadd = 1;
                                        //        adddays = 3;
                                        //        break;
                                        //}
                                        //newdt = dtoprsupval.AsEnumerable().Take(firstadd).CopyToDataTable();
                                        //if ((newdt.AsEnumerable().Where(x => x.Field<string>("SupervisorName") != null).Count() > 0))
                                        //{
                                        //    workSheet.Cells[Row + 1, 9].Value = newdt.AsEnumerable().Where(x => x.Field<string>("SupervisorName") != null).Select(x => x.Field<string>("SupervisorName")).Distinct().First().ToString() + "  (" + newdt.AsEnumerable().Where(x => x.Field<DateTime?>("SupervisorTS") != null).Select(x => x.Field<DateTime?>("SupervisorTS")).Distinct().First().ToString() + " )";
                                        //}
                                        //int Col = 9; Col = Col + firstadd;
                                        //workSheet.Cells[Row + 1, 9, Row + 1, Col - 1].Merge = true;
                                        //while (Col < 39)
                                        //{
                                        //    if (adddays == 3)
                                        //    {
                                        //        if (Col >= 39) Col = 39;
                                        //        workSheet.Cells[Row + 1, Col, Row + 1, (Col + adddays - 1)].Merge = true;
                                        //        newdt = dtoprsupval.AsEnumerable().Skip(firstadd).Take(adddays).CopyToDataTable();
                                        //        if ((newdt.AsEnumerable().Where(x => x.Field<string>("SupervisorName") != null).Count() > 0))
                                        //        {
                                        //            workSheet.Cells[Row + 1, Col].Value = newdt.AsEnumerable().Where(x => x.Field<string>("SupervisorName") != null).Select(x => x.Field<string>("SupervisorName")).Distinct().First().ToString() + "  (" + newdt.AsEnumerable().Where(x => x.Field<DateTime?>("SupervisorTS") != null).Select(x => x.Field<DateTime?>("SupervisorTS")).Distinct().First().ToString() + " )";
                                        //        }
                                        //        Col = Col + adddays;
                                        //        adddays = 4; firstadd += adddays;

                                        //    }
                                        //    else if (adddays == 4)
                                        //    {
                                        //        if (Col >= 39) Col = 39;
                                        //        workSheet.Cells[Row + 1, Col, Row + 1, (Col + adddays - 1)].Merge = true;
                                        //        newdt = dtoprsupval.AsEnumerable().Skip(firstadd).Take(adddays).CopyToDataTable();
                                        //        if ((newdt.AsEnumerable().Where(x => x.Field<string>("SupervisorName") != null).Count() > 0))
                                        //        {
                                        //            workSheet.Cells[Row + 1, Col].Value = newdt.AsEnumerable().Where(x => x.Field<string>("SupervisorName") != null).Select(x => x.Field<string>("SupervisorName")).Distinct().First().ToString() + "  (" + newdt.AsEnumerable().Where(x => x.Field<DateTime?>("SupervisorTS") != null).Select(x => x.Field<DateTime?>("SupervisorTS")).Distinct().First().ToString() + " )";
                                        //        }
                                        //        Col = Col + adddays;
                                        //        adddays = 3; firstadd += adddays;
                                        //    }
                                        //}
                                        //if (!(Col >= 39))
                                        //    workSheet.Cells[Row + 1, Col, Row + 1, 39].Merge = true;
                                        #endregion
                                        string Name = dtoprsupval.Rows[0]["SupervisorName"].ToString();
                                        string Timestamp = dtoprsupval.Rows[0]["SupervisorTS"].ToString();
                                        //workSheet.Cells[Row + 1, 9].Value = dtoprsupval.Rows[0]["SupervisorTS"].ToString() + " ( " +dtoprsupval.Rows[0]["SupervisorTS"].ToString() + " )";
                                        int Col = 9; int tillmerge = 9;
                                        foreach (DataRow dataRow in dtoprsupval.Rows)
                                        {

                                            workSheet.Cells[Row, Col].Value = string.IsNullOrEmpty(dataRow["OperatorName"].ToString()) ? "" : dataRow["OperatorName"].ToString();
                                            //if (!(Timestamp.Equals(dataRow["SupervisorTS"].ToString()) && Name.Equals(dataRow["SupervisorName"].ToString())))
                                            //{
                                            //    Timestamp = dataRow["SupervisorTS"].ToString();
                                            //    Name = dataRow["SupervisorName"].ToString();
                                            //    if (!(string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Timestamp)))
                                            //        workSheet.Cells[Row + 1, tillmerge].Value = Name + Environment.NewLine + Util.GetDateTime(Timestamp).ToString("dd-MM-yyyy");
                                            //    if (Col != tillmerge)
                                            //        workSheet.Cells[Row + 1, tillmerge, Row + 1, Col - 1].Merge = true;

                                            //    tillmerge = Col;

                                            //}
                                            //Col++;
                                            if (!string.IsNullOrEmpty(dataRow["SupervisorTS"].ToString()))
                                                workSheet.Cells[Row + 1, Col].Value = Name + Environment.NewLine + Util.GetDateTime(Timestamp).ToString("dd-MM-yyyy");
                                            //if (!string.IsNullOrEmpty(Timestamp))
                                            //    workSheet.Cells[Row + 1, tillmerge].Value = dataRow["SupervisorName"].ToString() + Environment.NewLine + Util.GetDateTime(dataRow["SupervisorTS"].ToString()).ToString("dd-MM-yyyy");
                                            ProdHead = string.IsNullOrEmpty(dataRow["ProdHeadName"].ToString()) ? ProdHead : dataRow["ProdHeadName"].ToString();
                                            ProdHeadTS = string.IsNullOrEmpty(dataRow["ProdHeadTS"].ToString()) ? ProdHeadTS : dataRow["ProdHeadTS"].ToString();
                                            Col++;
                                        }
                                        Timestamp = "";
                                        Name = "";
                                        //if (tillmerge != 39)
                                        //    workSheet.Cells[Row + 1, tillmerge, Row + 1, 39].Merge = true;
                                        Col--;
                                        workSheet.Cells[Row + 2, 9, Row + 2, 39].Merge = true;
                                        workSheet.Row(Row + 1).Height = 36;
                                        workSheet.Row(Row + 2).Height = 36;
                                        if (!string.IsNullOrEmpty(ProdHead))
                                            workSheet.Cells[Row + 2, 9].Value = ProdHead + Environment.NewLine + Util.GetDateTime(ProdHeadTS).ToString("dd-MM-yyyy");
                                        workSheet.Cells[Row + 1, 9, Row + 2, 39].Style.WrapText = true;
                                        workSheet.Cells[Row + 1, 9, Row + 2, 39].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        workSheet.Cells[Row + 1, 9, Row + 2, 39].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        workSheet.Cells[Row, 8, Row + 2, 8].Style.Font.Bold = true;
                                        workSheet.Cells[Row, 8, Row + 2, 39].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        workSheet.Cells[Row, 8, Row + 2, 39].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        workSheet.Cells[Row, 8, Row + 2, 39].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        workSheet.Cells[Row, 8, Row + 2, 39].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        workSheet.Cells[Row, 8, Row + 2, 39].Style.Border.Top.Color.SetColor(Color.Black);
                                        workSheet.Cells[Row, 8, Row + 2, 39].Style.Border.Bottom.Color.SetColor(Color.Black);
                                        workSheet.Cells[Row, 8, Row + 2, 39].Style.Border.Right.Color.SetColor(Color.Black);
                                        workSheet.Cells[Row, 8, Row + 2, 39].Style.Border.Left.Color.SetColor(Color.Black);
                                        workSheet.Cells[Row, 8, Row, 39].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        workSheet.Cells[Row, 8, Row, 39].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#70AD47"));
                                        workSheet.Cells[Row + 1, 8, Row + 1, 39].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        workSheet.Cells[Row + 1, 8, Row + 1, 39].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFD966"));
                                        workSheet.Cells[Row + 2, 8, Row + 2, 39].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        workSheet.Cells[Row + 2, 8, Row + 2, 39].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#F4B084"));
                                        workSheet.Cells["C5"].Value = Machine;
                                        workSheet.Cells["G5"].Value = cellID;
                                        workSheet.Cells["J5"].Value = fromDate.ToString("MMM-yyyy");
                                        //workSheet.Name = dtoprsupval.Rows[0]["ShiftName"].ToString();
                                    }
                                }

                            }
                        }
                    }
                    if (Excel.Workbook.Worksheets.Count > 1)
                    {
                        Excel.Workbook.Worksheets.Delete(1);
                        DownloadMultipleFile(destination, Excel.GetAsByteArray());
                        Generated = "Generated";
                    }
                    else
                        Generated = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return Generated;
        }
        #endregion


        #region -----PMDashboard report----
        internal static bool GeneratePMDashboardReport(string plant, string machineID, DateTime fromDate, DateTime todate, List<PMMasterDetails> listPMDetails)
        {
            bool successfull = false;
            try
            {
                string Filename = "PHDashboardReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "PHDashboardReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("PM Dashboard Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    int rowStart;
                    int colStart = 1;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    if (listPMDetails != null && listPMDetails.Count > 0)
                    {

                        var workSheet = Excel.Workbook.Worksheets[1];
                        setWorkSheetSetting(workSheet);
                        rowStart = 4;
                        workSheet.Cells["A1"].Value = workSheet.Cells["A1"].Value.ToString() + ": " + plant;
                        workSheet.Cells["C1"].Value = workSheet.Cells["C1"].Value.ToString() + ": " + machineID;
                        workSheet.Cells["A3"].Value = workSheet.Cells["A3"].Value.ToString() + ": " + fromDate.ToString("dd-MM-yyyy");
                        workSheet.Cells["C3"].Value = workSheet.Cells["C3"].Value.ToString() + ": " + todate.ToString("dd-MM-yyyy");
                        for (int i = 0; i < listPMDetails.Count; i++)
                        {
                            if (i == 0)
                            {
                                workSheet.Cells[rowStart, colStart].Value = "Machine ID";
                                workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = "PM Activity";
                                workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = "No. Of Cycle";
                                workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = "Status";
                                workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                                colStart++;
                                //workSheet.Cells[rowStart, colStart].Value = "Operator";
                                //workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                                //workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                //workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                                //colStart++;
                                workSheet.Cells[rowStart, colStart].Value = "Updated DateTime";
                                workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                                rowStart++;
                            }
                            colStart = 1;
                            workSheet.Cells[rowStart, colStart].Value = listPMDetails[i].MachineID;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listPMDetails[i].PMActivity;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listPMDetails[i].NoOfCycle;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listPMDetails[i].Status;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listPMDetails[i].Updatedts;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            rowStart++;
                        }
                        workSheet.Cells[4, 1, rowStart - 1, 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[4, 1, rowStart - 1, 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[4, 1, rowStart - 1, 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[4, 1, rowStart - 1, 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[4, 1, rowStart - 1, 5].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        workSheet.Cells[4, 1, rowStart - 1, 5].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        workSheet.Cells[4, 1, rowStart - 1, 5].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                        workSheet.Cells[4, 1, rowStart - 1, 5].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    }
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    successfull = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return successfull;
        }


        #endregion

        #region -----Help Request Details report----
        internal static bool GenerateHelpRequestReport(DateTime fromDate, DateTime todate, string plantname, List<HelpRequestReportDetails> listHRDetails, List<HelpRequestReportDetails> listavgCallTypeDetails)
        {
            bool successfull = false;
            try
            {
                string Filename = "HelpReqDetailsReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "HelpReqDetailsReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Help Request Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    int rowStart;
                    int colStart = 1;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    //Excel.Workbook.Worksheets.Delete("Sheet1");
                    if (listHRDetails != null && listHRDetails.Count > 0)
                    {

                        var workSheet = Excel.Workbook.Worksheets[1];
                        setWorkSheetSetting(workSheet);
                        workSheet.Cells["B2"].Value = fromDate.ToString("dd-MM-yyyy");
                        workSheet.Cells["E2"].Value = todate.ToString("dd-MM-yyyy");
                        workSheet.Cells["A4"].Value = plantname.TrimEnd(',');
                        colStart = 5;
                        int tempcolStart = colStart;
                        for (int j = 0; j < listavgCallTypeDetails.Count; j++)
                        {
                            rowStart = 5;
                            workSheet.Cells[rowStart, colStart].Value = listavgCallTypeDetails[j].RequestType;
                            workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                            workSheet.Cells[rowStart, colStart].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheet.Cells[rowStart, colStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[rowStart, colStart].Style.Font.Size = 8;
                            workSheet.Cells[rowStart, colStart].Style.Font.Bold = true;
                            rowStart++;
                            workSheet.Cells[rowStart, colStart].Value = listavgCallTypeDetails[j].AvgAckTimeFromTrigger;
                            workSheet.Cells[rowStart, colStart].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheet.Cells[rowStart, colStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rowStart++;
                            workSheet.Cells[rowStart, colStart].Value = listavgCallTypeDetails[j].AvgResetTimeFRomTrigger;
                            workSheet.Cells[rowStart, colStart].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheet.Cells[rowStart, colStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rowStart++;
                            workSheet.Cells[rowStart, colStart].Value = listavgCallTypeDetails[j].AvgAckOperatorTimeFromTrigger;
                            workSheet.Cells[rowStart, colStart].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheet.Cells[rowStart, colStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                        }
                        workSheet.Cells[5, tempcolStart, 8, listavgCallTypeDetails.Count + 4].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[5, tempcolStart, 8, listavgCallTypeDetails.Count + 4].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[5, tempcolStart, 8, listavgCallTypeDetails.Count + 4].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[5, tempcolStart, 8, listavgCallTypeDetails.Count + 4].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[5, tempcolStart, 8, listavgCallTypeDetails.Count + 4].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        workSheet.Cells[5, tempcolStart, 8, listavgCallTypeDetails.Count + 4].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        workSheet.Cells[5, tempcolStart, 8, listavgCallTypeDetails.Count + 4].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                        workSheet.Cells[5, tempcolStart, 8, listavgCallTypeDetails.Count + 4].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);

                        rowStart = 10;
                        bool showPlantId = false;
                        if (plantname.TrimEnd(',').Contains(','))
                        {
                            showPlantId = true;
                        }
                        int borderHeadCol = 0;
                        for (int i = 0; i < listHRDetails.Count; i++)
                        {
                            if (i == 0)
                            {

                                colStart = 1;
                                workSheet.Cells[rowStart, colStart].Value = "Date";
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                                colStart++;
                                if (showPlantId)
                                {
                                    workSheet.Cells[rowStart, colStart].Value = "Plant ID";

                                    workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                                    colStart++;
                                }
                                workSheet.Cells[rowStart, colStart].Value = "Machine";

                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = "Shift";

                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = "Request Type";
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = "Requested Time";
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = "Ack. Time";
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = "Reset Time";
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                                colStart++;

                                if (System.Web.Configuration.WebConfigurationManager.AppSettings["AckByOpreratorTime"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                                {
                                    workSheet.Cells[rowStart, colStart].Value = "Ack by Operator Time";
                                    workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                                    colStart++;
                                }
                                workSheet.Cells[rowStart, colStart].Value = "Ack. Time from Trigger (min)";
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                                workSheet.Column(colStart).Width = 24;
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = "Reset Time from Trigger (min)";
                                workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                                workSheet.Column(colStart).Width = 24;
                                colStart++;
                                if (System.Web.Configuration.WebConfigurationManager.AppSettings["AckByOpreratorTimeFromTrigger"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                                {
                                    workSheet.Cells[rowStart, colStart].Value = "Ack. by Operator from Trigger (min)";
                                    workSheet.Cells[rowStart, colStart].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    workSheet.Cells[rowStart, colStart].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                                    workSheet.Column(colStart).Width = 24;
                                    colStart++;
                                }
                                rowStart++;
                                //int borderHeadCol = 11;
                                //if (showPlantId)
                                //{
                                //    borderHeadCol = 12;
                                //}
                                borderHeadCol = colStart - 1;
                                workSheet.Cells[10, 1, rowStart - 1, borderHeadCol].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                workSheet.Cells[10, 1, rowStart - 1, borderHeadCol].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                workSheet.Cells[10, 1, rowStart - 1, borderHeadCol].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                workSheet.Cells[10, 1, rowStart - 1, borderHeadCol].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                workSheet.Cells[10, 1, rowStart - 1, borderHeadCol].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                                workSheet.Cells[10, 1, rowStart - 1, borderHeadCol].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                                workSheet.Cells[10, 1, rowStart - 1, borderHeadCol].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                                workSheet.Cells[10, 1, rowStart - 1, borderHeadCol].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                            }
                            colStart = 1;
                            workSheet.Cells[rowStart, colStart].Value = listHRDetails[i].ShiftDate;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            if (showPlantId)
                            {
                                workSheet.Cells[rowStart, colStart].Value = listHRDetails[i].PlantID;
                                workSheet.Column(colStart).AutoFit();
                                colStart++;
                            }
                            workSheet.Cells[rowStart, colStart].Value = listHRDetails[i].MachineID;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listHRDetails[i].ShiftName;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listHRDetails[i].RequestType;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listHRDetails[i].RequestedTime;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listHRDetails[i].AckTime;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listHRDetails[i].ResetTime;
                            workSheet.Column(colStart).AutoFit();
                            colStart++;
                            if (System.Web.Configuration.WebConfigurationManager.AppSettings["AckByOpreratorTime"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                            {
                                workSheet.Cells[rowStart, colStart].Value = listHRDetails[i].AckOperatorTime;
                                workSheet.Column(colStart).AutoFit();
                                colStart++;
                            }
                            workSheet.Cells[rowStart, colStart].Value = listHRDetails[i].AckTimeFromTrigger;
                            // workSheet.Column(colStart).AutoFit();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = listHRDetails[i].ResetTimeFRomTrigger;
                            // workSheet.Column(colStart).AutoFit();
                            colStart++;
                            if (System.Web.Configuration.WebConfigurationManager.AppSettings["AckByOpreratorTimeFromTrigger"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                            {
                                workSheet.Cells[rowStart, colStart].Value = listHRDetails[i].AckOperatorTimeFromTrigger;
                                //  workSheet.Column(colStart).AutoFit();
                                colStart++;
                            }
                            rowStart++;
                        }
                        //int borderCol = 11;
                        //if (showPlantId)
                        //{
                        //    borderCol = 12;
                        //}
                        workSheet.Cells[11, 1, rowStart - 1, borderHeadCol].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[11, 1, rowStart - 1, borderHeadCol].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[11, 1, rowStart - 1, borderHeadCol].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[11, 1, rowStart - 1, borderHeadCol].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[11, 1, rowStart - 1, borderHeadCol].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        workSheet.Cells[11, 1, rowStart - 1, borderHeadCol].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        workSheet.Cells[11, 1, rowStart - 1, borderHeadCol].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                        workSheet.Cells[11, 1, rowStart - 1, borderHeadCol].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    }
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    successfull = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            return successfull;
        }
        #endregion
    }
}