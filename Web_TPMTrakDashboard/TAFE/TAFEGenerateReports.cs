using Elmah;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.TAFE
{
    public class TAFEGenerateReports
    {
        static readonly string appPath = HttpContext.Current.Server.MapPath("~/TAFE/Reports");

        public static string SafeFileName(string name)
        {
            StringBuilder str = new StringBuilder(name);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.ToString();
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

        #region "Plan Vs Actual Report - TAFE"
        internal static DataTable GetPlanVsActualData(string plantId, string lineId, string date, out DataTable dtPlanVsActualDataCumulative)
        {
            dtPlanVsActualDataCumulative = new DataTable();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable planVsActualDataDaywise = new DataTable();
            SqlDataReader rdr = null;
            try
            {
                if (lineId.Equals("Line All", StringComparison.OrdinalIgnoreCase)) lineId = "";
                SqlCommand cmd = new SqlCommand(@"s_GetTafe_PlanV/sActualReport", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", date);
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@Groupid", lineId);
                cmd.CommandTimeout = 120;
                rdr = cmd.ExecuteReader();
                planVsActualDataDaywise.Load(rdr);
                dtPlanVsActualDataCumulative.Load(rdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return planVsActualDataDaywise;
        }
        #endregion

        #region "Hold Report - TAFE"
        internal static string GenerateHoldReport(DateTime fromDate, DateTime toDate, string lineId, string machineId)
        {
            string generated = string.Empty;
            string Filename = "HoldReport_Tafe.xlsx";
            string Source = GetReportPath(Filename);
            string Template = "HoldReport_Tafe" + DateTime.Now + ".xlsx";
            string destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
            DataTable dtHoldReportData = new DataTable();
            try
            {
                dtHoldReportData = TafeDataBaseAccess.GetHoldReportData(fromDate, toDate, lineId, machineId);
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Engery Report- \n " + Source);
                }

                if (dtHoldReportData != null && dtHoldReportData.Rows.Count > 0)
                {
                    try
                    {
                        FileInfo newFile = new FileInfo(Source);
                        ExcelPackage Excel = new ExcelPackage(newFile, true);
                        var worksheet = Excel.Workbook.Worksheets[1];
                        int row = 7, col = 1;
                        worksheet.Cells["B4"].Value = fromDate.ToString("yyyy-MM-dd");
                        worksheet.Cells["D4	"].Value = toDate.AddDays(-1).ToString("yyyy-MM-dd");
                        foreach (DataRow item in dtHoldReportData.Rows)
                        {
                            col = 1;
                            worksheet.Cells[row, col].Value = item["PlantID"];
                            col++;
                            worksheet.Cells[row, col].Value = item["machineid"];
                            col++;
                            worksheet.Cells[row, col].Value = item["ShiftName"];
                            col++;
                            worksheet.Cells[row, col].Value = item["Employeeid"];
                            col++;
                            worksheet.Cells[row, col].Value = item["componentid"];
                            col++;
                            worksheet.Cells[row, col].Value = item["SupplierCode"];
                            col++;
                            worksheet.Cells[row, col].Value = item["HeatCode"];
                            col++;
                            worksheet.Cells[row, col].Value = item["BatchCode"];
                            col++;
                            worksheet.Cells[row, col].Value = item["description"];
                            col++;
                            worksheet.Cells[row, col].Value = item["compslno"];
                            col++;
                            worksheet.Cells[row, col].Value = item["QualityTS"];
                            col++;
                            worksheet.Cells[row, col].Value = item["Remark"];
                            col++;
                            worksheet.Cells[row, col].Value = item["OperatorRemarks"];
                            row++;
                        }
                        row--;
                        worksheet.Cells[4, 1, row, col + 1].AutoFitColumns();
                        worksheet.Cells[7, 1, row, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                        DownloadMultipleFile(destination, Excel.GetAsByteArray());
                        Logger.WriteDebugLog("Hold Report Generated.");
                        generated = "Generated";
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteErrorLog(ex.Message);
                        throw;
                    }
                }
                else
                {
                    generated = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog(ex.Message);
                throw;
            }
            return generated;
        }

        #endregion

        #region "CategoryWise OEE And Loss Time Report - TAFE"
        internal static string GenerateOEEAndLossTimeReport(DateTime fromDate, string machineId)
        {
            string generated = string.Empty;
            string Filename = "OEEAndLosstimeDetails_Tafe.xlsx";
            string Source = GetReportPath(Filename);
            string Template = "OEEAndLosstimeDetails_Tafe" + DateTime.Now + ".xlsx";
            string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
            DataSet oeeAndLosstimeDetailsDataSet = new DataSet();
            try
            {
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("OEE And Loss Time Report template does not exists at - " + Source);
                }
                else
                {
                    oeeAndLosstimeDetailsDataSet = TafeDataBaseAccess.GetOEEAndLosstimeDetails(fromDate, machineId);
                    if (oeeAndLosstimeDetailsDataSet != null && oeeAndLosstimeDetailsDataSet.Tables.Count > 1)
                    {
                        int CatNoProdCount = 0, CatBreakdownCount = 0, CatSpeedlossCount = 0;
                        int CategoryColNum = 4, RowNum = 4;
                        FileInfo newFile = new FileInfo(Source);
                        ExcelPackage Excel = new ExcelPackage(newFile, true);
                        var worksheet = Excel.Workbook.Worksheets[1];
                        worksheet.Cells["V1"].Value = fromDate.ToString("dd-MM-yyyy");
                        DataTable dtCategoryDetails = oeeAndLosstimeDetailsDataSet.Tables[0];
                        DataTable dtDaywiseOeeAndLosstimeDetails = oeeAndLosstimeDetailsDataSet.Tables[1];
                        DataTable dtDaywiseTotalOeeAndLosstimeDetails = oeeAndLosstimeDetailsDataSet.Tables[2];
                        DataTable dtTotalOeeAndLosstimeDetails = oeeAndLosstimeDetailsDataSet.Tables[3];
                        if (dtCategoryDetails != null && dtCategoryDetails.Rows.Count > 0)
                        {
                            List<string> listCatNoProdDetails = dtCategoryDetails.AsEnumerable().Where(x => x.Field<string>("MainCatagory").Equals("No production")).Select(x => x.Field<string>("Catagory")).ToList();
                            List<string> listCatBreakdownDetails = dtCategoryDetails.AsEnumerable().Where(x => x.Field<string>("MainCatagory").Equals("Breakdown")).Select(x => x.Field<string>("Catagory")).ToList();
                            List<string> listCatSpeedlossDetails = dtCategoryDetails.AsEnumerable().Where(x => x.Field<string>("MainCatagory").Equals("Speed loss")).Select(x => x.Field<string>("Catagory")).ToList();
                            CatNoProdCount = listCatNoProdDetails.Count;
                            CatBreakdownCount = listCatBreakdownDetails.Count;
                            CatSpeedlossCount = listCatSpeedlossDetails.Count;
                            if (CatNoProdCount > 0)
                            {
                                foreach (string category in listCatNoProdDetails)
                                {
                                    worksheet.Cells[RowNum, CategoryColNum].Value = category;
                                    CategoryColNum++;
                                }
                            }
                            CategoryColNum = 9;
                            if (CatBreakdownCount > 0)
                            {
                                foreach (string category in listCatBreakdownDetails)
                                {
                                    worksheet.Cells[RowNum, CategoryColNum].Value = category;
                                    CategoryColNum++;
                                }
                            }
                            CategoryColNum = 14;
                            if (CatSpeedlossCount > 0)
                            {
                                foreach (string category in listCatSpeedlossDetails)
                                {
                                    worksheet.Cells[RowNum, CategoryColNum].Value = category;
                                    CategoryColNum++;
                                }
                            }
                        }

                        RowNum = 5;
                        int ColNum = 1, CategoryCount = 1;
                        if (dtDaywiseOeeAndLosstimeDetails != null && dtDaywiseOeeAndLosstimeDetails.Rows.Count > 0)
                        {
                            foreach (DataRow dataRow in dtDaywiseOeeAndLosstimeDetails.Rows)
                            {
                                ColNum = 1; CategoryCount = 1;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["MachineID"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["AvlTotalTime"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["AvlTime"]; ColNum++;
                                for (int i = 1; i <= CatNoProdCount; i++)
                                {
                                    worksheet.Cells[RowNum, ColNum].Value = Convert.ToDouble(dataRow["C" + CategoryCount]);
                                    ColNum++; CategoryCount++;
                                }
                                ColNum = 8;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["LoadingTime"]; ColNum++;
                                for (int i = 1; i <= CatBreakdownCount; i++)
                                {
                                    worksheet.Cells[RowNum, ColNum].Value = Convert.ToDouble(dataRow["C" + CategoryCount]);
                                    ColNum++; CategoryCount++;
                                }
                                ColNum = 13;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["OperatingTime"]; ColNum++;
                                for (int i = 1; i <= CatSpeedlossCount; i++)
                                {
                                    worksheet.Cells[RowNum, ColNum].Value = Convert.ToDouble(dataRow["C" + CategoryCount]);
                                    ColNum++; CategoryCount++;
                                }
                                ColNum = 18;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["NetOperatingTime"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["Hold"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["RejMat"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["RejPro"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["ValuableOperatingTime"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["AEffy"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["PEffy"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["QEffy"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["OEffy"];
                                RowNum++;
                            }
                        }

                        if (dtDaywiseTotalOeeAndLosstimeDetails != null && dtDaywiseTotalOeeAndLosstimeDetails.Rows.Count > 0)
                        {
                            foreach (DataRow dataRow in dtDaywiseTotalOeeAndLosstimeDetails.Rows)
                            {
                                ColNum = 1; CategoryCount = 1;
                                worksheet.Cells[RowNum, ColNum].Value = "Total/Avgt"; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["Tot_AvlTotalTime"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["Tot_AvlTime"]; ColNum++;
                                for (int i = 1; i <= CatNoProdCount; i++)
                                {
                                    worksheet.Cells[RowNum, ColNum].Value = Convert.ToDouble(dataRow["C" + CategoryCount]);
                                    ColNum++; CategoryCount++;
                                }
                                ColNum = 8;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["LoadingTime"]; ColNum++;
                                for (int i = 1; i <= CatBreakdownCount; i++)
                                {
                                    worksheet.Cells[RowNum, ColNum].Value = Convert.ToDouble(dataRow["C" + CategoryCount]);
                                    ColNum++; CategoryCount++;
                                }
                                ColNum = 13;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["OperatingTime"]; ColNum++;
                                for (int i = 1; i <= CatSpeedlossCount; i++)
                                {
                                    worksheet.Cells[RowNum, ColNum].Value = Convert.ToDouble(dataRow["C" + CategoryCount]);
                                    ColNum++; CategoryCount++;
                                }
                                ColNum = 18;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["NetOperatingTime"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["Tot_Hold"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["Tot_RejMat"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["Tot_RejPro"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["ValuableOperatingTime"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["Tot_AEffy"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["Tot_PEffy"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["Tot_QEffy"]; ColNum++;
                                worksheet.Cells[RowNum, ColNum].Value = dataRow["Tot_OEffy"];
                                RowNum++;
                            }
                        }

                        RowNum = 17;
                        if (dtTotalOeeAndLosstimeDetails != null && dtTotalOeeAndLosstimeDetails.Rows.Count > 0)
                        {
                            foreach (DataRow dataRow in dtTotalOeeAndLosstimeDetails.Rows)
                            {
                                worksheet.Cells[RowNum, 1].Value = string.Format("Available Time: {0:0.##}% of total time", dataRow["AvailableTime"]).ToUpper();
                                worksheet.Cells[RowNum, 23].Value = string.Format("Plant Closure: {0:0.##}% of total time", dataRow["PlantClosureTime"]).ToUpper();
                                worksheet.Cells[RowNum + 1, 1].Value = string.Format("Loading Time: {0:0.##}% of total time", dataRow["LoadingTime"]).ToUpper();
                                //worksheet.Cells[RowNum + 1, 14].Value = string.Format("Others (P, A, M, RM): {0:0.##}% of total time", dataRow["Others"]).ToUpper();
                                worksheet.Cells[RowNum + 1, 18].Value = string.Format("No Prdn Planned: {0:0.##}% of total time", dataRow["NoPrdnPlanned"]).ToUpper();
                                worksheet.Cells[RowNum + 2, 1].Value = string.Format("Operating Time: {0:0.##}% of loading time", dataRow["OperatingTime"]).ToUpper();
                                worksheet.Cells[RowNum + 2, 10].Value = string.Format("Downtime: {0:0.##}% of Avl. Time", dataRow["DownTime"]).ToUpper();
                                worksheet.Cells[RowNum + 3, 1].Value = string.Format("Net Opt. Time: {0:0.##}% of loading Time", dataRow["NetOperatingTime"]).ToUpper();
                                worksheet.Cells[RowNum + 3, 8].Value = string.Format("Speed Loss: {0:0.##}% of Avl. Time", dataRow["SpeedLoss"]).ToUpper();
                                worksheet.Cells[RowNum + 4, 1].Value = string.Format("Val. Time: {0:0.##}% of loading Time", dataRow["ValuableOperatingTime"]).ToUpper();
                                worksheet.Cells[RowNum + 4, 6].Value = string.Format("Quality Loss: {0:0.##}% of Avl. Time", dataRow["QualityLoss"]).ToUpper();
                                RowNum++;
                            }
                        }

                        CategoryColNum = 4;
                        for (int i = CategoryColNum; i < CategoryColNum + 4; i++)
                        {
                            if (i >= CategoryColNum + CatNoProdCount)
                                worksheet.Column(i).Hidden = true;
                        }
                        CategoryColNum = 9;
                        for (int i = CategoryColNum; i < CategoryColNum + 4; i++)
                        {
                            if (i >= CategoryColNum + CatBreakdownCount)
                                worksheet.Column(i).Hidden = true;
                        }
                        CategoryColNum = 14;
                        for (int i = CategoryColNum; i < CategoryColNum + 4; i++)
                        {
                            if (i >= CategoryColNum + CatSpeedlossCount)
                                worksheet.Column(i).Hidden = true;
                        }

                        DownloadMultipleFile(destination, Excel.GetAsByteArray());
                        Logger.WriteDebugLog("CategoryWise OEE and Loss Time Report Generated Successfully.");
                        generated = "Generated";
                    }
                    else
                    {
                        generated = "NoDataFound";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog(ex.Message);
                throw;
            }
            return generated;
        }
        #endregion

        #region "Categorywise Rejection Reports - TAFE"
        internal static string RejectionReport(DateTime fromDate, DateTime toDate, string PlantID, string LineID, string Category)
        {
            string Generated = string.Empty;
            try
            {
                string reportName = "";
                string src, dst = string.Empty;
                reportName = "TAFE_MaterialRejectionReport.xlsx";
                src = GetReportPath(reportName);
                string tempfileName = "TAFE_MaterialRejectionReport" + "_" + Guid.NewGuid() + ".xlsx";
                dst = Path.Combine(appPath, "Temp", SafeFileName(tempfileName));
                if (!File.Exists(src))
                {
                    Logger.WriteDebugLog("TAFE_MaterialRejectionReport - \n " + src);
                }
                FileInfo newFile = new FileInfo(src);
                ExcelPackage pck = new ExcelPackage(newFile, true);
                var wsDts = pck.Workbook.Worksheets[1];
                wsDts.Cells["B4"].Value = fromDate;

                wsDts.Cells["H4"].Value = LineID;
                wsDts.Cells["F4"].Value = PlantID;
                wsDts.Cells["J4"].Value = Category;
                PlantID = PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID;
                LineID = LineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : LineID;
                DataTable dtRejection = TafeDataBaseAccess.GetRejectionReportData(fromDate, toDate, PlantID, LineID, Category);
                wsDts.Cells["D4"].Value = toDate.AddDays(-1);
                int row = 9;
                if (dtRejection != null && dtRejection.Rows.Count > 0)
                {

                    foreach (DataRow dtRow in dtRejection.Rows)
                    {
                        wsDts.Cells[row, 1].Value = dtRow["Rejdate"];
                        wsDts.Cells[row, 2].Value = dtRow["ShiftName"];
                        wsDts.Cells[row, 3].Value = dtRow["Machineid"];
                        wsDts.Cells[row, 4].Value = dtRow["Employeeid"];
                        wsDts.Cells[row, 5].Value = dtRow["BatchCode"];
                        wsDts.Cells[row, 6].Value = dtRow["compslno"];
                        wsDts.Cells[row, 7].Value = dtRow["HeatCode"];
                        wsDts.Cells[row, 8].Value = dtRow["SupplierCode"];
                        wsDts.Cells[row, 9].Value = dtRow["componentid"];
                        wsDts.Cells[row, 10].Value = dtRow["description"];
                        wsDts.Cells[row, 11].Value = dtRow["Rejection_Qty"];
                        wsDts.Cells[row, 12].Value = dtRow["DefectObserved"];
                        wsDts.Cells[row, 13].Value = dtRow["MST"];
                        wsDts.Cells[row, 14].Value = dtRow["Remark"];
                        wsDts.Cells[row, 15].Value = dtRow["Scrap"];
                        wsDts.Cells[row, 16].Value = dtRow["Rew"];
                        wsDts.Cells[row, 17].Value = dtRow["Seg"];
                        wsDts.Cells[row, 18].Value = dtRow["AccUO"];
                        wsDts.Cells[row, 19].Value = dtRow["Rating"];
                        wsDts.Cells[row, 20].Value = dtRow["RootCause"];
                        wsDts.Cells[row, 21].Value = dtRow["ActiontoBeTaken"];
                        wsDts.Cells[row, 22].Value = dtRow["Targetdate"].ToString() != "" ? (DateTime.Parse(dtRow["Targetdate"].ToString()).Year == 1900 ? "" : dtRow["Targetdate"]) : "";
                        row++;
                    }
                    row--;
                    wsDts.Cells[4, 1, row, 24].AutoFitColumns();
                    wsDts.Cells[9, 1, row, 24].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDts.Cells[9, 1, row, 24].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDts.Cells[9, 1, row, 24].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDts.Cells[9, 1, row, 24].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDts.Cells[9, 1, row, 24].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    wsDts.Cells[9, 1, row, 24].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    wsDts.Cells[9, 1, row, 24].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    wsDts.Cells[9, 1, row, 24].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    DownloadMultipleFile(dst, pck.GetAsByteArray());
                    Generated = "Generated";
                }
                else
                {
                    Generated = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return Generated;
        }
        #endregion

        #region "Batchwise Report - TAFE"
        internal static string BatchWiseReport(DateTime fromDate, string PlantID, string LineID, string PartID, string Category)
        {
            string generated = "";
            try
            {

                string reportName = "";
                string src, dst = string.Empty;
                reportName = "Tafe_BatchWiseReport.xlsx";
                src = GetReportPath(reportName);
                string tempfileName = "Tafe_BatchWiseReport" + "_" + Guid.NewGuid() + ".xlsx";
                dst = Path.Combine(appPath, "Temp", SafeFileName(tempfileName));
                if (!File.Exists(src))
                {
                    Logger.WriteDebugLog("Tafe_BatchWiseReport - \n " + src);
                }
                FileInfo newFile = new FileInfo(src);
                ExcelPackage pck = new ExcelPackage(newFile, true);
                var wsDts = pck.Workbook.Worksheets[1];
                wsDts.Cells["C5"].Value = fromDate;
                wsDts.Cells["F5"].Value = PlantID;
                wsDts.Cells["J5"].Value = LineID;
                wsDts.Cells["N5"].Value = PartID;
                wsDts.Cells["R5"].Value = Category;
                PlantID = PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID;
                LineID = LineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : LineID;
                DataTable dtGraph = TafeDataBaseAccess.GetBatchWiseGraphDateReport(fromDate, PlantID, LineID, PartID, Category);
                DataTable dtBatchwise = TafeDataBaseAccess.GetBatchWiseDataReport(fromDate, PlantID, LineID, PartID, Category);
                int graphrow = 10, datarow = 33;
                if (Category.Equals("Material", StringComparison.OrdinalIgnoreCase))
                {
                    wsDts.Cells["A1"].Value = "BATCH WISE MATERIAL REJECTION REPORT";
                }
                else if (Category.Equals("Process", StringComparison.OrdinalIgnoreCase))
                {
                    wsDts.Cells["A1"].Value = "BATCH WISE Process REJECTION REPORT";
                }
                string Partdescription = TafeDataBaseAccess.Getdescription(PartID);
                if (dtGraph != null && dtGraph.Rows.Count > 0)
                {
                    foreach (DataRow dtrow in dtGraph.Rows)
                    {
                        wsDts.Cells[graphrow, 30].Value = dtrow["Batchcode"];
                        wsDts.Cells[graphrow, 31].Value = dtrow["RejectionPercent"];
                        graphrow++;
                    }


                    var chart = (ExcelBarChart)wsDts.Drawings.AddChart("ColChart", eChartType.ColumnClustered);
                    chart.SetSize(1180, 450);
                    chart.SetPosition(5, 30, 0, 30);
                    chart.Title.Text = "Batch Wise report";
                    chart.XAxis.Title.Text = "BATCH";
                    chart.YAxis.Title.Text = "REJECTION IN PERCENT";
                    chart.Series.Add(ExcelRange.GetAddress(10, 31, graphrow - 1, 31), ExcelRange.GetAddress(10, 30, graphrow - 1, 30));
                    chart.Series[0].Header = Partdescription;
                }
                if (dtBatchwise != null && dtBatchwise.Rows.Count > 0)
                {
                    bool exists = dtBatchwise.AsEnumerable().Where(c => c.Field<string>("Type").Equals("OK", StringComparison.OrdinalIgnoreCase)).Count() > 0;
                    DataTable dtOK = new DataTable(); DataTable dtRej = new DataTable();
                    if (exists)
                        dtOK = dtBatchwise.Rows.Cast<DataRow>().Where(x => x["Type"].ToString().Equals("OK", StringComparison.OrdinalIgnoreCase)).CopyToDataTable();
                    exists = dtBatchwise.AsEnumerable().Where(c => c.Field<string>("Type").Equals("Rejection", StringComparison.OrdinalIgnoreCase)).Count() > 0;
                    if (exists)
                        dtRej = dtBatchwise.Rows.Cast<DataRow>().Where(x => x["Type"].ToString().Equals("Rejection", StringComparison.OrdinalIgnoreCase)).CopyToDataTable();
                    wsDts.Cells[datarow - 1, 2, datarow - 1, 4].Merge = true;
                    wsDts.Cells[datarow - 1, 2].Value = "Part : " + Partdescription;
                    wsDts.Cells[datarow - 1, 5].Value = "Total";
                    int col = 5;
                    for (int i = 5; i < dtBatchwise.Columns.Count; i++)
                    {
                        wsDts.Cells[32, (i + 1)].Value = dtBatchwise.Columns[i].ColumnName.ToString();
                        col++;
                    }
                    wsDts.Cells[datarow - 1, 2, datarow - 1, (col)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    wsDts.Cells[datarow - 1, 2, datarow - 1, (col)].Style.Font.Color.SetColor(Color.Blue);
                    wsDts.Cells[datarow - 1, 2, datarow - 1, (col)].Style.Font.Bold = true;
                    wsDts.Cells[datarow, 2].Value = "Status";
                    wsDts.Cells[datarow, 3].Value = "OK";
                    wsDts.Cells[datarow, 4].Value = "SupplierCode";
                    wsDts.Cells[datarow, 2, datarow, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    wsDts.Cells[datarow, 2, datarow, 4].Style.Font.Bold = true;

                    datarow++;
                    if (dtOK.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dtOK.Rows)
                        {
                            wsDts.Cells[datarow, 2].Value = dtrow["BatchStatus"];
                            wsDts.Cells[datarow, 3].Value = dtrow["BatchCode"];
                            wsDts.Cells[datarow, 4].Value = dtrow["Suppliercode"];
                            wsDts.Cells[datarow, 5].Value = dtrow["TotalQty"];
                            datarow++;
                        }
                        wsDts.Cells[datarow, 2, datarow, 4].Merge = true;
                        wsDts.Cells[datarow, 2].Value = "Sum for status";
                        wsDts.Cells[datarow, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        wsDts.Cells[datarow, 2].Style.Font.Bold = true;
                        wsDts.Cells[datarow, 5].Formula = "=SUM(E34:E" + (datarow - 1) + ")";
                        for (int k = 5; k < dtOK.Columns.Count; k++)
                        {
                            string Index = GetExcelColumnName(k + 1);
                            string formula = "=SUM(" + Index + 34 + ":" + Index + (datarow - 1) + ")";
                            wsDts.Cells[datarow, (k + 1)].Formula = formula;
                        }
                    }
                    else
                        wsDts.Cells[datarow, 5].Value = 0;
                    int sumokrow = datarow;
                    datarow = datarow + 2;
                    wsDts.Cells[datarow, 2].Value = "Status";
                    wsDts.Cells[datarow, 3].Value = "REJ";
                    wsDts.Cells[datarow, 4].Value = "SupplierCode";
                    wsDts.Cells[datarow, 2, datarow, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    wsDts.Cells[datarow, 2, datarow, 4].Style.Font.Bold = true;
                    datarow++;
                    int start = datarow;
                    if (dtRej.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtRej.Rows.Count; i++)
                        {
                            wsDts.Cells[datarow, 2].Value = dtRej.Rows[i]["BatchStatus"];
                            wsDts.Cells[datarow, 3].Value = dtRej.Rows[i]["BatchCode"];
                            wsDts.Cells[datarow, 4].Value = dtRej.Rows[i]["Suppliercode"];
                            wsDts.Cells[datarow, 5].Value = dtRej.Rows[i]["TotalQty"];
                            for (int k = 5; k < col; k++)
                            {
                                wsDts.Cells[datarow, (k + 1)].Value = string.IsNullOrEmpty(dtRej.Rows[i][k].ToString()) ? 0 : dtRej.Rows[i][k];
                            }
                            datarow++;
                        }
                        wsDts.Cells[datarow, 5].Formula = "=SUM(E" + start + ":E" + (datarow - 1) + ")";
                    }
                    else
                        wsDts.Cells[datarow, 5].Value = 0;
                    wsDts.Cells[datarow, 2, datarow, 4].Merge = true;
                    wsDts.Cells[datarow, 2].Value = "Sum for status";


                    for (int k = 5; k < dtRej.Columns.Count; k++)
                    {
                        string Index = GetExcelColumnName(k + 1);
                        string formula = "=SUM(" + Index + start + ":" + Index + (datarow - 1) + ")";
                        wsDts.Cells[datarow, (k + 1)].Formula = formula;
                    }

                    wsDts.Cells[datarow, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    wsDts.Cells[datarow, 2].Style.Font.Bold = true;
                    int sumrejrow = datarow;
                    datarow = datarow + 2;
                    wsDts.Cells[datarow, 2, datarow, 4].Merge = true;
                    wsDts.Cells[datarow, 2].Value = "Sum for Parts for Month";
                    wsDts.Cells[datarow, 5].Formula = "=SUM(E" + sumokrow + ",E" + sumrejrow + ")";
                    for (int k = 5; k < dtRej.Columns.Count; k++)
                    {
                        string Index = GetExcelColumnName(k + 1);
                        string formula = "=SUM(" + Index + sumokrow + "," + Index + sumrejrow + ")";
                        wsDts.Cells[datarow, (k + 1)].Formula = formula;
                    }
                    wsDts.Cells[datarow, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    wsDts.Cells[datarow, 2].Style.Font.Bold = true;
                    wsDts.Cells[32, 1, datarow, col].AutoFitColumns();
                    wsDts.Cells[33, 2, datarow, col].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDts.Cells[33, 2, datarow, col].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDts.Cells[33, 2, datarow, col].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDts.Cells[33, 2, datarow, col].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDts.Cells[33, 2, datarow, col].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    wsDts.Cells[33, 2, datarow, col].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    wsDts.Cells[33, 2, datarow, col].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    wsDts.Cells[33, 2, datarow, col].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    DownloadMultipleFile(dst, pck.GetAsByteArray());
                    generated = "Generated";
                }
                else
                {
                    generated = "NoDataFound";
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return generated;
        }
        #endregion

        #region "Machine History Report - TAFE"
        internal static string GenerateMachineHistoryReport(DateTime fromDate, DateTime toDate, string machineId)
        {
            string generated = "";
            string Source = string.Empty, destination = string.Empty, Template = string.Empty;
            string Filename = "MachineHistoryReport_Tafe.xlsx";
            Source = GetReportPath(Filename);
            Template = "MachineHistoryReport_Tafe_" + DateTime.Now.ToString("dd/MMM/yyyy HH:mm") + ".xlsx";
            destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
            DataTable dtMachineHistoryData = new DataTable();
            List<MachineHistory> MachineHistoryData = TafeDataBaseAccess.GetMachineHistoryDatas(fromDate, toDate, machineId);
            if (!File.Exists(Source))
            {
                Logger.WriteDebugLog("MachineHistoryReport_Tafe template does not found at - \n " + Source);
            }

            if (MachineHistoryData != null && MachineHistoryData.Count > 0)
            {
                try
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[1];
                    int row = 7, col = 1;
                    worksheet.Cells["B4"].Value = fromDate.ToString("yyyy-MM-dd");
                    worksheet.Cells["D4	"].Value = toDate.ToString("yyyy-MM-dd");
                    foreach (MachineHistory item in MachineHistoryData)
                    {
                        col = 1;
                        worksheet.Cells[row, col].Value = item.MachineID;
                        col++;
                        worksheet.Cells[row, col].Value = item.DownCode;
                        col++;
                        worksheet.Cells[row, col].Value = item.KindOfProblem;
                        col++;
                        worksheet.Cells[row, col].Value = item.Reason;
                        col++;
                        worksheet.Cells[row, col].Value = item.DownCategory;
                        col++;
                        worksheet.Cells[row, col].Value = item.BreakDownStart;
                        col++;
                        worksheet.Cells[row, col].Value = item.BreakDownEnd;
                        col++;
                        worksheet.Cells[row, col].Value = item.ActionToResolve;
                        col++;
                        worksheet.Cells[row, col].Value = item.ActionProposed;
                        col++;
                        worksheet.Cells[row, col].Value = item.TimeLost;
                        col++;
                        worksheet.Cells[row, col].Value = item.ElapsedTime;
                        col++;
                        worksheet.Cells[row, col].Value = item.Severity;
                        row++;
                    }
                    row--;
                    worksheet.Cells[4, 1, row, col + 1].AutoFitColumns();
                    worksheet.Cells[7, 1, row, col].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[7, 1, row, col].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[7, 1, row, col].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[7, 1, row, col].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[7, 1, row, col].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    worksheet.Cells[7, 1, row, col].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    worksheet.Cells[7, 1, row, col].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    worksheet.Cells[7, 1, row, col].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    Logger.WriteDebugLog("Machine Histtory Report Generated.");
                    generated = "Generated";
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.Message);
                    throw;
                }
            }
            else
            {
                generated = "NoDataFound";
            }
            return generated;
        }
        #endregion

        #region LineMeterReport
        internal static string LineMeter(DateTime fromDate, string LineID)
        {
            string Generarted = "";
            try
            {
                string generated = string.Empty;
                string reportName = "";
                string src, dst = string.Empty;
                reportName = "LineMeterReportTafe.xlsx";
                src = GetReportPath(reportName);
                string tempfileName = "LineMeterReportTafe" + "_" + Guid.NewGuid() + ".xlsx";
                dst = Path.Combine(appPath, "Temp", SafeFileName(tempfileName));
                if (!File.Exists(src))
                {
                    Logger.WriteDebugLog("LineMeterReportTafe - \n " + src);
                }

                FileInfo newFile = new FileInfo(src);
                ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                ExcelWorksheet ws = excelPackage.Workbook.Worksheets[1];

                ws.Cells["A1"].Value = "Date: " + fromDate.ToString("dd-MM-yyyy");
                ws.Cells["Z1"].Value = "Line-Id";
                ws.Cells["AC1"].Value = LineID;
                string Fromdate = TafeDataBaseAccess.Gellogicalmonthstart(fromDate);
                string toDate = TafeDataBaseAccess.GellogicalmonthEnd(fromDate);
                DataTable dt = TafeDataBaseAccess.GetLinemeterData(LineID, Convert.ToDateTime(Fromdate).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"));

                #region OldCode
                //ws.Name = "Linemeter Graph";
                //ws.Cells["A1"].Value = string.Format("Line meter - {0}", machineId);
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    int rowno = 2;
                //    int initrowno = rowno;
                //    foreach (DataRow row in dt.Rows)
                //    {
                //        ws.Cells[rowno, 1].Value = row["TargetCount"];
                //        ws.Cells[rowno, 2].Value = row["ActualCount"];
                //        ws.Cells[rowno, 3].Value = row["TenPercent"];
                //        ws.Cells[rowno, 4].Value = row["NegativeTenPercent"];
                //        ws.Cells[rowno, 5].Value = row["DelayCount"];
                //        ws.Cells[rowno, 6].Value = fromDate.AddDays(rowno - initrowno).Day;
                //        rowno += 1;
                //    }
                //    rowno -= 1;
                //    ws.Cells[initrowno, 1, rowno, 6].Style.Font.Color.SetColor(Color.White);
                //    int pixelleft = 10;

                //    var chart = (ExcelLineChart)ws.Drawings.AddChart(string.Format("linechart{0}", 0), eChartType.Line);

                //    chart.SetSize(1200, 600);
                //    chart.SetPosition(100, pixelleft);
                //    chart.Title.Text = machineId;
                //    ExcelChartSerie ch3 = chart.Series.Add(ExcelRange.GetAddress(initrowno, 3, rowno, 3), ExcelRange.GetAddress(initrowno, 6, rowno, 6));
                //    ExcelChartSerie ch1 = chart.Series.Add(ExcelRange.GetAddress(initrowno, 1, rowno, 1), ExcelRange.GetAddress(initrowno, 6, rowno, 6));
                //    ExcelChartSerie ch4 = chart.Series.Add(ExcelRange.GetAddress(initrowno, 4, rowno, 4), ExcelRange.GetAddress(initrowno, 6, rowno, 6));
                //    ExcelChartSerie ch2 = chart.Series.Add(ExcelRange.GetAddress(initrowno, 2, rowno, 2), ExcelRange.GetAddress(initrowno, 6, rowno, 6));
                //    ExcelChart chart2 = chart.PlotArea.ChartTypes.Add(eChartType.ColumnClustered);

                //    ExcelChartSerie ch5 = chart2.Series.Add(ExcelRange.GetAddress(initrowno, 5, rowno, 5), ExcelRange.GetAddress(initrowno, 6, rowno, 6));
                //    ch1.Header = "Target";
                //    ch2.Header = "Actual";
                //    ch3.Header = "+10%";
                //    ch4.Header = "-10%";
                //    ch5.Header = "Delay";
                //    chart.DataLabel.ShowValue = true;
                #endregion

                if (dt != null && dt.Rows.Count > 0)
                {
                    int col = 2;
                    foreach (DataRow Row in dt.Rows)
                    {
                        ws.Cells[3, col].Value = Convert.ToDateTime(Row["Day"].ToString());
                        ws.Cells[4, col].Value = Convert.ToDouble(Row["TargetCount"].ToString());
                        ws.Cells[5, col].Value = Convert.ToDouble(Row["ActualCount"].ToString());
                        ws.Cells[6, col].Value = Convert.ToDouble(Row["DelayCount"].ToString());
                        ws.Cells[7, col].Value = Convert.ToDouble(Row["TenPercent"].ToString());
                        ws.Cells[8, col].Value = Convert.ToDouble(Row["NegativeTenPercent"].ToString());
                        ws.Cells[9, col].Value = Convert.ToDouble(Row["LoadingHours"].ToString());
                        ws.Cells[10, col].Value = Convert.ToDouble(Row["NoOfManpower"].ToString());
                        ws.Cells[12, col].Value = Convert.ToDouble(Row["Okdays"].ToString());
                        if ((Convert.ToDouble(Row["Okdays"].ToString())).Equals(1))
                        {
                            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#00B050");
                            ws.Cells[5, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[5, col].Style.Fill.BackgroundColor.SetColor(colFromHex);
                        }

                        ws.Cells[11, col].Value = Convert.ToDouble(Row["LineEfficiency"].ToString());
                        col++;
                    }
                    for (int days = dt.Rows.Count + 1; days <= 31; days++)
                    {
                        ws.Column(days + 1).Hidden = true;

                    }
                    DownloadMultipleFile(dst, excelPackage.GetAsByteArray());
                    Logger.WriteDebugLog("Line Meter Report Generated.");
                    generated = "Generated";
                }
                else
                {
                    Generarted = "NoDataFound";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog(ex.ToString());
                throw;
            }
            return Generarted;
        }
        #endregion


        #region "Plan Vs Actual Report - TAFE"
        internal static bool GeneratePlanVsActualReport(string PlantId, string LineId, string Date, DataTable dtPlanVsActualDataDaywise, DataTable dtPlanVsActualDataCumulative)
        {
            bool successfull;
            try
            {
                string Filename = "PlanVsActualReport_Tafe.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "PlanVsActualReport_Tafe" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Plan Vs Actual Report template does not exists at - " + Source);
                    successfull = false;
                }
                else
                {
                    int rowStart = 8;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheetCumulative = Excel.Workbook.Worksheets[1];
                    worksheetCumulative.Cells["B4"].Value = PlantId;
                    worksheetCumulative.Cells["E4"].Value = LineId;

                    worksheetCumulative.Cells["D6"].Value = "MTD(" + Convert.ToDateTime(dtPlanVsActualDataCumulative.Rows[0]["LastAggDateForMonth"].ToString()).ToString("dd-MMM-yyyy") + ")";
                    worksheetCumulative.Cells["J6"].Value = "YTD(" + Convert.ToDateTime(dtPlanVsActualDataCumulative.Rows[0]["LastAggDateForYear"].ToString()).ToString("dd-MMM-yyyy") + ")";
                    foreach (DataRow dataRow in dtPlanVsActualDataCumulative.Rows)
                    {
                        worksheetCumulative.Cells[rowStart, 1].Value = dataRow["PartName"];
                        worksheetCumulative.Cells[rowStart, 2].Value = dataRow["PartID"];
                        worksheetCumulative.Cells[rowStart, 3].Value = dataRow["StdCycletime"];
                        worksheetCumulative.Cells[rowStart, 4].Value = dataRow["ScheduledQtyMTD"];
                        worksheetCumulative.Cells[rowStart, 5].Value = dataRow["ActualQtyMTD"];
                        worksheetCumulative.Cells[rowStart, 6].Value = dataRow["HoldQtyMTD"];
                        worksheetCumulative.Cells[rowStart, 7].Value = dataRow["DelayQtyMTD"];
                        worksheetCumulative.Cells[rowStart, 8].Value = dataRow["RejMaterialMTD"];
                        worksheetCumulative.Cells[rowStart, 9].Value = dataRow["RejProcessMTD"];
                        worksheetCumulative.Cells[rowStart, 10].Value = dataRow["ScheduledQtyYTD"];
                        worksheetCumulative.Cells[rowStart, 11].Value = dataRow["ActualQtyYTD"];
                        worksheetCumulative.Cells[rowStart, 12].Value = dataRow["HoldQtyYTD"];
                        worksheetCumulative.Cells[rowStart, 13].Value = dataRow["DelayQtyYTD"];
                        worksheetCumulative.Cells[rowStart, 14].Value = dataRow["RejMaterialYTD"];
                        worksheetCumulative.Cells[rowStart, 15].Value = dataRow["RejProcessYTD"];

                        rowStart++;
                    }
                    worksheetCumulative.Cells["G4"].Value = "";
                    var worksheetDaywise = Excel.Workbook.Worksheets[2];
                    worksheetDaywise.Cells["B4"].Value = PlantId;
                    worksheetDaywise.Cells["E4"].Value = LineId;
                    rowStart = 8;
                    foreach (DataRow dataRow in dtPlanVsActualDataDaywise.Rows)
                    {
                        worksheetDaywise.Cells[rowStart, 1].Value = dataRow["Pdate"];
                        worksheetDaywise.Cells[rowStart, 1].Style.Numberformat.Format = "dd-MMM-yyyy";
                        worksheetDaywise.Cells[rowStart, 2].Value = dataRow["Line"];
                        worksheetDaywise.Cells[rowStart, 3].Value = dataRow["PartName"];
                        worksheetDaywise.Cells[rowStart, 4].Value = dataRow["PartID"];
                        worksheetDaywise.Cells[rowStart, 5].Value = dataRow["StdCycletime"];
                        worksheetDaywise.Cells[rowStart, 6].Value = dataRow["ScheduledQty"];
                        worksheetDaywise.Cells[rowStart, 7].Value = dataRow["ActualQty"];
                        worksheetDaywise.Cells[rowStart, 8].Value = dataRow["HoldQty"];
                        worksheetDaywise.Cells[rowStart, 9].Value = dataRow["DelayQty"];
                        worksheetDaywise.Cells[rowStart, 10].Value = dataRow["RejMaterial"];
                        worksheetDaywise.Cells[rowStart, 11].Value = dataRow["RejProcess"];

                        rowStart++;
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