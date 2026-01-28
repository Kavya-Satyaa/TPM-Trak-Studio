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

namespace Web_TPMTrakDashboard.HighWay
{
    public class HighwayGenerateReports
    {
        static readonly string appPath = HttpContext.Current.Server.MapPath("~/Highway");
        #region "Get Report Template File Path"
        public static string GetReportPath(string reportName)
        {
            string src = string.Empty;
            if (HttpContext.Current.Session["Language"] == null)
                src = Path.Combine(appPath, "ReportTemplate", reportName);
            else
            {
                if (HttpContext.Current.Session["Language"].ToString() != "en")
                    src = Path.Combine(appPath, "ReportTemplate-" + HttpContext.Current.Session["Language"].ToString() + "", reportName);
                else
                    src = Path.Combine(appPath, "ReportTemplate", reportName);
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
        private static void setBorderThin(ExcelWorksheet workSheet, int fromRow, int fromCol, int toRow, int toCol)
        {
            try
            {
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private static void setBorderThin_White(ExcelWorksheet workSheet, int fromRow, int fromCol, int toRow, int toCol)
        {
            try
            {
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Color.SetColor(System.Drawing.Color.FromArgb(0, 32, 96));
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Color.SetColor(System.Drawing.Color.FromArgb(0, 32, 96));
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Color.SetColor(System.Drawing.Color.FromArgb(0, 32, 96));
                workSheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.FromArgb(0, 32, 96));
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        public static string GenerateInspectionReportOfShaft(DataTable dt, DataTable dtOperator, string Component, string Shift, string Date, string Operation, string RevID, string HeatNo, string DieNo)
        {
            string Generated = "";
            try
            {
                string Filename = "InspectionReportOfShaft.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "InspectionReportOfShaft" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("InspectionReportOfShaft template does not exists at - " + Source);
                    Generated = "";
                }
                else
                {
                    int rowStart = 10;
                    int headerColStart = 9, colStart = 1, k = 1;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    if (dt.Rows.Count > 0)
                    {
                        var workSheet = Excel.Workbook.Worksheets["Sheet1"];
                        #region-------Headers---------------------------------------------------------------------------
                        workSheet.Cells["B5"].Value = Component;
                        workSheet.Cells["D5"].Value = DBAccess.GetComponentName(Component);
                        for (int i = 18; i < dt.Columns.Count; i++)
                        {
                            var value = dt.Columns[i].ColumnName.ToString().Split('$');
                            //workSheet.Cells[7, headerColStart].Value = "Time : " + value[0].ToString() + "" + "\nDie No :" + value[1].ToString() + "  " + "Heat No :" + value[2].ToString();
                            workSheet.Cells[7, headerColStart].Value = "Time : \n" + value[0].ToString();
                            workSheet.Cells[8, headerColStart].Value = "Die No :\n" + value[1].ToString();
                            workSheet.Cells[9, headerColStart].Value ="Heat No :\n" + value[2].ToString();
                            headerColStart++;
                        }
                        workSheet.Cells[6, 1, 9, headerColStart - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[6, 1, 9, headerColStart - 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 32, 96));
                        workSheet.Cells[6, 1, 9, headerColStart - 1].Style.Font.Color.SetColor(Color.White);
                        workSheet.Cells[6, 9, 6, headerColStart - 1].Merge = true;
                        workSheet.Cells[6, 9].Value = "Observations";
                        setBorderThin_White(workSheet, 6, 1, 9, headerColStart - 1);
                        workSheet.Cells["J1"].Value =Date;
                        workSheet.Cells["L1"].Value = Shift;
                        workSheet.Cells["J2"].Value = DieNo;
                        workSheet.Cells["L2"].Value = RevID;
                        workSheet.Cells["J3"].Value = Operation;
                        workSheet.Cells["L3"].Value = HeatNo;
                        #endregion------------------------------------------------------------------------------------------------
                        foreach (DataRow dtRow in dt.Rows)
                        {
                            colStart = 1;
                            workSheet.Cells[rowStart, colStart].Value = k++;
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = dtRow["BalNo"].ToString();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = dtRow["CharacteristicID"].ToString();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = dtRow["Characteristic"].ToString();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = dtRow["Specification"].ToString();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = dtRow["InspectionMethod"].ToString();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = dtRow["Frequency"].ToString();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = dtRow["Freq_Qty"].ToString();
                            colStart++;
                            for (int i = 18; i < dt.Columns.Count; i++)
                            {
                                workSheet.Cells[rowStart, colStart].Value = dtRow[dt.Columns[i].ColumnName.ToString()].ToString();
                                colStart++;
                            }
                            rowStart++;
                        }
                        //workSheet.Cells[9, 1, rowStart, colStart].AutoFitColumns();
                        colStart--;
                        workSheet.Cells[rowStart, 1].Value = "Remarks:-" + (dtOperator.Rows.Count > 0 ? dtOperator.Rows[0]["Remarks"].ToString() : "");
                        workSheet.Cells[rowStart + 1, 1].Value = "QA Inspector";
                        workSheet.Cells[rowStart + 1, 3].Value = "H.O.D (Production)";
                        workSheet.Cells[rowStart + 1, 5].Value = "H.O.D (QA)";
                      
                        if (dtOperator.Rows.Count > 0)
                        {
                            workSheet.Cells[rowStart + 1, 2].Value = dtOperator.Rows[0]["InspectedBy"].ToString() != "" ? dtOperator.Rows[0]["InspectedBy"].ToString() + "-" + dtOperator.Rows[0]["InspectedTS"].ToString() : "";
                            workSheet.Cells[rowStart + 1, 4].Value = dtOperator.Rows[0]["Production_HOD_ID"].ToString() != "" ? dtOperator.Rows[0]["Production_HOD_ID"].ToString() + "-" + dtOperator.Rows[0]["Production_HOD_TS"].ToString() : "";
                            workSheet.Cells[rowStart + 1, 6].Value = dtOperator.Rows[0]["QA_HOD_ID"].ToString() != "" ? dtOperator.Rows[0]["QA_HOD_ID"].ToString() + "-" + dtOperator.Rows[0]["QA_HOD_TS"].ToString() : "";
                        }

                        setBorderThin(workSheet, 6, 1, rowStart + 1, colStart);
                    }
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    Generated = "Generated";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return Generated;
        }
        public static string GenerateMachineStartUpReport(DataTable dt,DataTable dtOperator,DataTable dtSupervisor, List<string> DayList, List<string> Shiftlist, string MachineID, string ComponentID, string OperationNo, int Year, int Month)
        {
            string Generated = "";
            try
            {
                string Filename = "MachineShaftReport.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "MachineShaftReport" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("InspectionReportOfShaft template does not exists at - " + Source);
                    Generated = "";
                }
                else
                {
                    int rowStart = 10;
                    int headerColStart = 4, colStart = 1, k = 1, a = 0;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    if (dt.Rows.Count > 0)
                    {
                        var workSheet = Excel.Workbook.Worksheets["Sheet1"];
                        #region-------Headers---------------------------------------------------------------------------

                        if (a == 0)
                        {
                            for (int i = 0; i < DayList.Count; i++)
                            {
                                workSheet.Cells[8, headerColStart, 8, headerColStart + Shiftlist.Count - 1].Merge = true;
                                workSheet.Cells[8, headerColStart].Value = DayList[i];
                                foreach (var shift in Shiftlist)
                                {
                                    workSheet.Cells[9, headerColStart].Value = shift;
                                    headerColStart++;
                                }
                            }
                            a++;
                        }
                        workSheet.Cells[8, 1, 9, headerColStart - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[8, 1, 9, headerColStart - 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 32, 96));
                        workSheet.Cells[8, 1, 9, headerColStart - 1].Style.Font.Color.SetColor(Color.White);
                        workSheet.Cells["B5"].Value = OperationNo;
                        workSheet.Cells["B6"].Value = OperationNo;
                        workSheet.Cells["B7"].Value = ComponentID;
                        workSheet.Cells["D5"].Value = DBAccess.GetMachineName(MachineID);
                        workSheet.Cells["D6"].Value = MachineID;
                        workSheet.Cells["D7"].Value = Month;

                        #endregion------------------------------------------------------------------------------------
                        foreach (DataRow dtRow in dt.Rows)
                        {
                            colStart = 1;
                            workSheet.Cells[rowStart, colStart].Value = k++;
                            colStart++;
                            //workSheet.Cells[rowStart, colStart].Value = dtRow["CharacteristicID"].ToString();
                            //colStart++;
                            workSheet.Cells[rowStart, colStart].Value = dtRow["Characteristics"].ToString();
                            colStart++;
                            workSheet.Cells[rowStart, colStart].Value = dtRow["PointsToBeChecked"].ToString();
                            colStart++;
                            for (int c = 4; c < dt.Columns.Count; c++)
                            {
                                workSheet.Cells[rowStart, colStart].Value = dtRow[dt.Columns[c].ColumnName.ToString()].ToString();
                                colStart++;
                            }
                            rowStart++;
                        }
                        workSheet.Cells[rowStart, 1, rowStart + 1, 2].Merge = true;
                        workSheet.Cells[rowStart, 1].Value = "SIGNATURE";
                        workSheet.Cells[rowStart, 3].Value = "OPERATOR";
                        workSheet.Cells[rowStart + 1, 3].Value = "SUPERVISOR";
                        
                        if (dtOperator.Rows.Count > 0)
                        {
                            colStart = 4;
                            for (int l = 4; l < dtOperator.Columns.Count; l++)
                            {
                                workSheet.Cells[rowStart, colStart].Value = dtOperator.Rows[0][l].ToString();
                                colStart++;
                            }
                        }
                        if (dtSupervisor.Rows.Count > 0)
                        {
                            colStart = 4;
                            for (int l = 4; l < dtSupervisor.Columns.Count; l++)
                            {
                                workSheet.Cells[rowStart + 1, colStart].Value = dtSupervisor.Rows[0][l].ToString();
                                colStart++;
                            }
                        }
                        setBorderThin(workSheet, 8, 1, rowStart + 1, colStart - 1);

                    }

                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    Generated = "Generated";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return Generated;
        }
    }
}