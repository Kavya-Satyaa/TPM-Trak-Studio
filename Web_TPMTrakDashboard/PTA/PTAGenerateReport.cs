using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Excel = Microsoft.Office.Interop.Excel;

namespace Web_TPMTrakDashboard.PTA
{
    public class PTAGenerateReport
    {
        static string appPath = HttpContext.Current.Server.MapPath("~/PTA/ReportTemplate");
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
        internal static string GenerateOperatorEffyReport(List<OperatorEffyReportEntity> OprDataExcel, string operatorID, string fromDate, string toDate)
        {
            string generated = "";
            try
            {
                string Filename = "OperatorEfficiency.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "OperatorEfficiency" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("OperatorEfficiency template does not found at - \n " + Source);
                }
                else
                {
                    if (OprDataExcel.Count == 0)
                    {
                        generated = "NoData";
                        return generated;
                    }
                    //Excel.Application xlApp = null;
                    Excel.Workbook xlWorkBook = null;
                    //Excel.Worksheet xlWorkSheet = null;
                    string xlRange = "";

                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    //xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                    var xlWorkSheet = Excel.Workbook.Worksheets[1];


                    int row = 4, col = 0, rowcount = 0;
                    int machinerowstart = 3, machinerowend = 3;
                    string shift = string.Empty;
                    DateTime date = new DateTime();
                    int pid = 0;
                    List<double> lst_netusemin = new List<double>();
                    List<double> lst_totMin = new List<double>();
                    double netloss = 0;
                    int alternateRow = 0;

                    xlWorkSheet.Cells[1, 1].Value = "From Date";
                    xlRange = "B1:C1";
                    xlWorkSheet.Cells[xlRange].Merge = true;
                    xlWorkSheet.Cells[1, 2].Value = fromDate;
                    xlWorkSheet.Cells[1, 4].Value = "To Date";
                    xlWorkSheet.Cells["E1:F1"].Merge = true;
                    xlWorkSheet.Cells[1, 5].Value = toDate;
                    xlWorkSheet.Cells["G1:L1"].Merge = true;
                    xlWorkSheet.Cells[1, 7].Value = "Operator Efficiency Report";
                    xlWorkSheet.Cells[1, 13].Value = "Operator";
                    xlWorkSheet.Cells[1, 14].Value = operatorID;
                    foreach (OperatorEffyReportEntity oprDataVal in OprDataExcel)
                    {
                        #region Rowcount > 0

                        if (rowcount > 0)
                        {
                            //if ((oprDataVal.Pdate.ToString() == date) && (oprDataVal.Shift.ToString() == shift))

                            if (oprDataVal.Pdate == date)
                            {
                                machinerowend = row;
                                xlWorkSheet.Cells[row, col + 1].Value = string.Format("{0:dd-MMM-yyyy}", oprDataVal.Pdate);    /* Date */
                                xlWorkSheet.Cells[row, col + 2].Value = oprDataVal.Shift;    /* Shift */
                                xlWorkSheet.Cells[row, col + 3].Value = oprDataVal.Machine;  /* Machine */
                                xlWorkSheet.Cells[row, col + 4].Value = oprDataVal.ProdTime; /* Production Time */
                                xlWorkSheet.Cells[row, col + 5].Value = oprDataVal.DwnTime;  /* DownTime */
                                xlWorkSheet.Cells[row, col + 6].Value = oprDataVal.Others;   /* Others */
                                xlWorkSheet.Cells[row, col + 7].Value = oprDataVal.AE;       /* AE */
                                xlWorkSheet.Cells[row, col + 8].Value = oprDataVal.PE;       /* OE */
                                xlWorkSheet.Cells[row, col + 9].Value = oprDataVal.OE;       /* PE */

                                /* Net useful min */
                                lst_netusemin.Add((Convert.ToDouble(oprDataVal.ProdTime) * Convert.ToDouble(oprDataVal.PE)) / 100);
                                xlWorkSheet.Cells[row, col + 10].Value = lst_netusemin[rowcount];

                                /* Total min */
                                lst_totMin.Add(Convert.ToDouble(oprDataVal.ProdTime) + Convert.ToDouble(oprDataVal.DwnTime));
                                xlWorkSheet.Cells[row, col + 11].Value = lst_totMin[rowcount];

                                /* Blended OEE */
                                if (lst_totMin.Sum() > 0)
                                    //xlWorkSheet.Cells[row, col + 12] = lst_netusemin.Sum() / lst_totMin.Sum();
                                    xlWorkSheet.Cells[machinerowstart, col + 12].Value = lst_netusemin.Sum() / lst_totMin.Sum();

                                /* Net benefit normalized to better machine */
                                /* maxTot is used to get the max value for TotalMinutes and to same to devided by the Useful min. */
                                int maxTot_exl = lst_totMin.IndexOf(lst_totMin.Max());
                                if (lst_totMin[0] > 0)
                                    //xlWorkSheet.Cells[row, col + 13] = (lst_netusemin.Sum() - maxTot_exl) / maxTot_exl;
                                    xlWorkSheet.Cells[machinerowstart, col + 13].Value = (lst_netusemin.Sum() - lst_totMin[maxTot_exl]) / lst_totMin[maxTot_exl];

                                /* Net Loss from 80% benchmark (min) */
                                netloss = (lst_totMin.Sum()) * 0.8 - (lst_netusemin.Sum());
                                //xlWorkSheet.Cells[row, col + 14] = netloss;
                                xlWorkSheet.Cells[machinerowstart, col + 14].Value = Math.Round(netloss, 0);

                                /* Net Loss (%) because of 1 operator less */
                                if (lst_totMin.Sum() > 0)
                                {
                                    //xlWorkSheet.Cells[row, col + 15] = (netloss / (lst_totMin.Sum() * 0.8));
                                    double xyz = lst_totMin.Sum() * 0.8;
                                    xyz = netloss / xyz;
                                    xlWorkSheet.Cells[machinerowstart, col + 15].Value = netloss / (lst_totMin.Sum() * 0.8);
                                }
                                xlWorkSheet.Cells["A" + machinerowstart.ToString() + ":" + "A" + machinerowend.ToString()].Merge = true;
                                //xlRange = xlApp.get_Range();
                                //xlRange.Merge(Missing.Value);
                                xlWorkSheet.Cells["B" + machinerowstart.ToString() + ":" + "B" + machinerowend.ToString()].Merge = true;
                                //xlRange = xlApp.get_Range("B" + machinerowstart, "B" + machinerowend);
                                //xlRange.Merge(Missing.Value);
                                //xlRange = xlApp.get_Range("J" + machinerowstart, "J" + machinerowend);
                                //xlRange.Merge(Missing.Value);
                                //xlRange = xlApp.get_Range("K" + machinerowstart, "K" + machinerowend);
                                //xlRange.Merge(Missing.Value);
                                xlWorkSheet.Cells["L" + machinerowstart.ToString() + ":" + "L" + machinerowend.ToString()].Merge = true;
                                //xlRange = xlApp.get_Range("L" + machinerowstart, "L" + machinerowend);
                                //xlRange.Merge(Missing.Value);
                                xlWorkSheet.Cells["M" + machinerowstart.ToString() + ":" + "M" + machinerowend.ToString()].Merge = true;
                                //xlRange = xlApp.get_Range("M" + machinerowstart, "M" + machinerowend);
                                //xlRange.Merge(Missing.Value);
                                xlWorkSheet.Cells["N" + machinerowstart.ToString() + ":" + "N" + machinerowend.ToString()].Merge = true;
                                //xlRange = xlApp.get_Range("N" + machinerowstart, "N" + machinerowend);
                                //xlRange.Merge(Missing.Value);
                                xlWorkSheet.Cells["O" + machinerowstart.ToString() + ":" + "O" + machinerowend.ToString()].Merge = true;
                                //xlRange = xlApp.get_Range("O" + machinerowstart, "O" + machinerowend);
                                //xlRange.Merge(Missing.Value);

                                //Range backcolor_range = xlWorkSheet.geta("A" + machinerowstart, "O" + machinerowend + "");
                                //if (alternateRow == 0)
                                //    backcolor_range.Interior.ColorIndex = 40;
                                //else
                                //    backcolor_range.Interior.ColorIndex = 00;
                                xlWorkSheet.Cells["A" + machinerowstart + ":O" + machinerowend].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                if (alternateRow == 0)
                                {
                                    //backcolor_range.Interior.ColorIndex = 40;
                                    Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#ffcc99");
                                    xlWorkSheet.Cells["A" + machinerowstart + ":O" + machinerowend].Style.Fill.BackgroundColor.SetColor(colFromHex);
                                }
                                else
                                {
                                    // backcolor_range.Interior.ColorIndex = 00;
                                    Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#CCFFFF");
                                    xlWorkSheet.Cells["A" + machinerowstart + ":O" + machinerowend].Style.Fill.BackgroundColor.SetColor(colFromHex);
                                }
                                row++;
                            }
                            else
                            {
                                rowcount = 0;
                                lst_netusemin.Clear();
                                lst_totMin.Clear();

                                /* Appears first time when DATE and Shift are not same with previous record */
                                date = oprDataVal.Pdate;
                                shift = oprDataVal.Shift;
                                xlWorkSheet.Cells[row, col + 1].Value = string.Format("{0:dd-MMM-yyyy}", oprDataVal.Pdate);    /* Date */
                                xlWorkSheet.Cells[row, col + 2].Value = oprDataVal.Shift;    /* Shift */
                                xlWorkSheet.Cells[row, col + 3].Value = oprDataVal.Machine;  /* Machine */
                                xlWorkSheet.Cells[row, col + 4].Value = oprDataVal.ProdTime; /* Production Time */
                                xlWorkSheet.Cells[row, col + 5].Value = oprDataVal.DwnTime;  /* DownTime */
                                xlWorkSheet.Cells[row, col + 6].Value = oprDataVal.Others;   /* Others */
                                xlWorkSheet.Cells[row, col + 7].Value = oprDataVal.AE;       /* AE */
                                xlWorkSheet.Cells[row, col + 8].Value = oprDataVal.PE;       /* OE */
                                xlWorkSheet.Cells[row, col + 9].Value = oprDataVal.OE;       /* PE */

                                /* Net useful min */
                                lst_netusemin.Add((Convert.ToDouble(oprDataVal.ProdTime) * Convert.ToDouble(oprDataVal.PE)) / 100);
                                xlWorkSheet.Cells[row, col + 10].Value = lst_netusemin[rowcount];

                                /* Total min */
                                lst_totMin.Add(Convert.ToDouble(oprDataVal.ProdTime) + Convert.ToDouble(oprDataVal.DwnTime));
                                xlWorkSheet.Cells[row, col + 11].Value = lst_totMin[rowcount];

                                /* Blended OEE */
                                if (lst_totMin.Sum() > 0)
                                    xlWorkSheet.Cells[row, col + 12].Value = lst_netusemin.Sum() / lst_totMin.Sum();

                                /* Net benefit normalized to better machine */
                                if (lst_totMin[0] > 0)
                                    xlWorkSheet.Cells[row, col + 13].Value = (lst_netusemin.Sum() - lst_totMin[0]) / lst_totMin[0];

                                /* Net Loss from 80% benchmark (min) */
                                netloss = (lst_totMin.Sum()) * 0.8 - (lst_netusemin.Sum());
                                xlWorkSheet.Cells[row, col + 14].Value = Math.Round(netloss, 0);

                                /* Net Loss (%) because of 1 operator less */
                                if (lst_totMin.Sum() > 0)
                                    xlWorkSheet.Cells[row, col + 15].Value = Math.Round((netloss / (lst_totMin.Sum() * 0.8)), 0);

                                machinerowstart = row;
                                if (alternateRow == 0) alternateRow = 1; else alternateRow = 0;
                                //Excel.Range backcolor_range = (Range)xlWorkSheet.SelectedRange("A" + machinerowstart, "O" + machinerowstart + "");
                                xlWorkSheet.Cells["A" + machinerowstart + ":O" + machinerowstart].Style.Fill.PatternType = ExcelFillStyle.Solid;

                                if (alternateRow == 0)
                                {
                                    //backcolor_range.Interior.ColorIndex = 40;
                                    Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#ffcc99");
                                    xlWorkSheet.Cells["A" + machinerowstart + ":O" + machinerowstart].Style.Fill.BackgroundColor.SetColor(colFromHex);
                                }
                                else
                                {
                                    // backcolor_range.Interior.ColorIndex = 00;
                                    Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#CCFFFF");
                                    xlWorkSheet.Cells["A" + machinerowstart + ":O" + machinerowstart].Style.Fill.BackgroundColor.SetColor(colFromHex);
                                }
                                row++;
                            }

                        }
                        #endregion

                        else /* Prints only if Row is 0, */
                        {
                            date = oprDataVal.Pdate;
                            shift = oprDataVal.Shift;
                            xlWorkSheet.Cells[row, col + 1].Value = string.Format("{0:dd-MMM-yyyy}", oprDataVal.Pdate);    /* Date */
                            xlWorkSheet.Cells[row, col + 2].Value = oprDataVal.Shift;    /* Shift */
                            xlWorkSheet.Cells[row, col + 3].Value = oprDataVal.Machine;  /* Machine */
                            xlWorkSheet.Cells[row, col + 4].Value = oprDataVal.ProdTime; /* Production Time */
                            xlWorkSheet.Cells[row, col + 5].Value = oprDataVal.DwnTime;  /* DownTime */
                            xlWorkSheet.Cells[row, col + 6].Value = oprDataVal.Others;   /* Others */
                            xlWorkSheet.Cells[row, col + 7].Value = oprDataVal.AE;       /* AE */
                            xlWorkSheet.Cells[row, col + 8].Value = oprDataVal.PE;       /* OE */
                            xlWorkSheet.Cells[row, col + 9].Value = oprDataVal.OE;       /* PE */

                            /* Net useful min */
                            lst_netusemin.Add((Convert.ToDouble(oprDataVal.ProdTime) * Convert.ToDouble(oprDataVal.PE)) / 100);
                            xlWorkSheet.Cells[row, col + 10].Value = lst_netusemin[rowcount];

                            /* Total min */
                            lst_totMin.Add(Convert.ToDouble(oprDataVal.ProdTime) + Convert.ToDouble(oprDataVal.DwnTime));
                            xlWorkSheet.Cells[row, col + 11].Value = lst_totMin[rowcount];

                            /* Blended OEE */
                            if (lst_totMin.Sum() > 0)
                                xlWorkSheet.Cells[row, col + 12].Value = lst_netusemin.Sum() / lst_totMin.Sum();

                            /* Net benefit normalized to better machine */
                            if (lst_totMin[0] > 0)
                                xlWorkSheet.Cells[row, col + 13].Value = (lst_netusemin.Sum() - lst_totMin[0]) / lst_totMin[0];

                            /* Net Loss from 80% benchmark (min) */
                            netloss = (lst_totMin.Sum()) * 0.8 - (lst_netusemin.Sum());
                            xlWorkSheet.Cells[row, col + 14].Value = Math.Round(netloss, 0);

                            /* Net Loss (%) because of 1 operator less */
                            if (lst_totMin.Sum() > 0)
                                xlWorkSheet.Cells[row, col + 15].Value = Math.Round((netloss / (lst_totMin.Sum() * 0.8)), 0);

                            machinerowstart = row;
                            alternateRow = 0;
                            row++;
                        }
                        rowcount++;

                        #region Plotting Border for Sheeet 
                        //Range firstRow = (Range)xlWorkSheet.Cells[3, 1];
                        //firstRow.Activate();
                        setThinBorder(xlWorkSheet, 3, 1, machinerowend, 15);
                        //Excel.Range range = xlWorkSheet.Application.get_Range("A3", "O" + machinerowend + "");

                        /* Left Border */
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThin;
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].ColorIndex = XlColorIndex.xlColorIndexAutomatic;

                        /* Right Border */
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThin;
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].ColorIndex = XlColorIndex.xlColorIndexAutomatic;

                        /* Top */
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlThin;
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].ColorIndex = XlColorIndex.xlColorIndexAutomatic;

                        /* Bottom */
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThin;
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = XlColorIndex.xlColorIndexAutomatic;

                        /* Row header Seperator */
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].Weight = XlBorderWeight.xlThin;
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].ColorIndex = XlColorIndex.xlColorIndexAutomatic;

                        /* Column header Seperator */
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].Weight = XlBorderWeight.xlThin;
                        //range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                        #endregion

                    }

                    int btmRow = row + 2;
                    xlWorkSheet.Cells[btmRow, 3].Value = "** Others -> Losses not attributable to Operator";
                    xlWorkSheet.Cells["C" + btmRow + ":" + "H" + btmRow].Merge = true;
                    //xlRange = xlApp.get_Range("C" + btmRow, "H" + btmRow);
                    //xlRange.Merge(Missing.Value);

                    xlWorkSheet.Cells["C" + (btmRow + 1) + ":" + "H" + (btmRow + 1)].Merge = true;
                    //xlRange = xlApp.get_Range("C" + (btmRow + 1), "H" + (btmRow + 1));
                    //xlRange.Merge(Missing.Value);
                    xlWorkSheet.Cells[btmRow + 1, 3].Value = "**Pe -> Assume 80% and 70% for this example";

                    xlWorkSheet.Cells["C" + (btmRow + 2) + ":" + "H" + (btmRow + 2)].Merge = true;
                    //xlRange = xlApp.get_Range("C" + (btmRow + 2), "H" + (btmRow + 2));
                    //xlRange.Merge(Missing.Value);
                    xlWorkSheet.Cells[btmRow + 2, 3].Value = "Benchmark -> Assume 80% for this example";

                    //xlRange = xlApp.get_Range("B" + btmRow, "G" + (btmRow + 2));
                    //xlRange.Merge(Missing.Value);
                    //Excel.Range bottomrange = (Excel.Range)xlWorkSheet.Cells[row + 2, 3];
                    //bottomrange.Activate();
                    //Excel.Range rangebtm = xlWorkSheet.Application.get_Range("C" + btmRow, "H" + (btmRow + 2) + "");
                    setThinBorder(xlWorkSheet, btmRow, 3, (btmRow + 2), 8);
                    /* Left Border */
                    //rangebtm.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                    //rangebtm.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;
                    //rangebtm.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;

                    /* Right Border */
                    //rangebtm.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                    //rangebtm.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThin;
                    //rangebtm.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;

                    /* Top */
                    //rangebtm.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                    //rangebtm.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
                    //rangebtm.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;

                    /* Bottom */
                    //rangebtm.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    //rangebtm.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                    //rangebtm.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;

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


        internal static string GenerateUnmanedDetailReport(string fromDate, string toDate, string shift, List<UnmanedReportEntity> details)
        {
            string generated = "";
            try
            {
                string Filename = "OperatorMachineReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "OperatorMachineReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("OperatorMachineReport template does not found at - \n " + Source);
                }
                else
                {
                    if (details.Count == 0)
                    {
                        generated = "NoData";
                        return generated;
                    }
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[1];


                    worksheet.Cells["B2"].Value = fromDate;
                    worksheet.Cells["E2"].Value = toDate;
                    worksheet.Cells["J2"].Value = shift;


                    string date = string.Empty;
                    int interior = 1, row1 = 0, row = 0;
                    //object[,] arr1 = new object[details.Count, 10];
                    int rowCount = 5;
                    for (int r = 0; r < details.Count; r++)
                    {
                        int c = 0;
                        if (date != Convert.ToString(details[r].Date) || date == string.Empty)
                        {

                            row1 = r + 5;
                            //arr1[r, c] = details[r].Date;
                            date = details[r].Date.ToString();
                            interior = interior + 1;
                            worksheet.Cells[rowCount, c + 1].Value = details[r].Date;
                            int mergeCount = details.Where(k => k.Date == details[r].Date).ToList().Count();
                            if (mergeCount > 1)
                            {
                                worksheet.Cells[rowCount, c + 1, rowCount + mergeCount - 1, c + 1].Merge = true;
                            }
                        }
                        c = c + 1;
                        //arr1[r, c] = details[r].MachineID;
                        worksheet.Cells[rowCount, c + 1].Value = details[r].MachineID;
                        c = c + 1;

                        row = r + 5;
                        //wrksht.get_Range((Excel.Range)wrksht.Cells[row1, 1], (Excel.Range)wrksht.Cells[row, 1]).Merge(false);
                        //wrksht.get_Range((Excel.Range)wrksht.Cells[row1, 2], (Excel.Range)wrksht.Cells[row, 2]).Merge(false);

                        //arr1[r, c] = details[r].Component;
                        worksheet.Cells[rowCount, c + 1].Value = details[r].Component;
                        c = c + 1;

                        //arr1[r, c] = details[r].Operation;
                        worksheet.Cells[rowCount, c + 1].Value = details[r].Operation;
                        c = c + 1;


                        //arr1[r, c] = details[r].Operator;
                        worksheet.Cells[rowCount, c + 1].Value = details[r].Operator;
                        c = c + 1;


                        //arr1[r, c] = details[r].BatchStart;
                        worksheet.Cells[rowCount, c + 1].Value = details[r].BatchStart;
                        c = c + 1;


                        //arr1[r, c] = details[r].BatchEnd;
                        worksheet.Cells[rowCount, c + 1].Value = details[r].BatchEnd;
                        c = c + 1;


                        //arr1[r, c] = details[r].NoOfComponents;
                        worksheet.Cells[rowCount, c + 1].Value = details[r].NoOfComponents;
                        c = c + 1;

                        //arr1[r, c] = details[r].UtilisedTime;
                        worksheet.Cells[rowCount, c + 1].Value = details[r].UtilisedTime;
                        c = c + 1;

                        //arr1[r, c] = details[r].DownTime;
                        worksheet.Cells[rowCount, c + 1].Value = details[r].DownTime;
                        c = c + 1;
                        if (interior % 2 == 0)
                        {
                            //wrksht.get_Range((Excel.Range)wrksht.Cells[row, 1], (Excel.Range)wrksht.Cells[row1, 10]).Interior.ColorIndex = 40;
                            worksheet.Cells[row1, 1, row, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#ffcc99");
                            worksheet.Cells[row1, 1, row, 10].Style.Fill.BackgroundColor.SetColor(colFromHex);
                        }
                        rowCount++;
                    }

                    //Excel.Range c1 = (Excel.Range)worksheet.Cells[5, 1];
                    //Excel.Range c2 = (Excel.Range)worksheet.Cells[5 + details.Count - 1, 10];
                    //Excel.Range range1 = worksheet.get_Range(c1, c2);
                    //range1.Value2 = arr1;

                    setThinBorder(worksheet, 5, 1, details.Count + 5, 10);

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
    }
}