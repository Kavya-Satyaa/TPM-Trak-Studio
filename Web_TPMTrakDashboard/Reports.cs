using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace TPM_TRAK_AnalyticsWebReports
{
    public class Reports
    {
        public static bool MISReport(string Path, string Destination, List<string> allShift, string PlantID, DataTable ShiftwiseData, DataTable DownTable, DateTime fromDate, DateTime toDate)
        {
            bool Saved = false;
            FileInfo newFile = new FileInfo(Path);
            ExcelPackage Excel = new ExcelPackage(newFile, true);
            bool colorshading = false;
            string machineName = string.Empty, partNumber = string.Empty, currentShift = "A";
            int rowStart = 9, colStart = 1, firstRow = 0, count = 0;
            var worksheet = Excel.Workbook.Worksheets[1];
            int rowindex = 9;
            if (worksheet != null)
            {
                worksheet.Cells["B5"].Value = fromDate.ToString("yyyy-MM-dd");
                worksheet.Cells["D5"].Value = toDate.ToString("yyyy-MM-dd");
                if (!string.IsNullOrEmpty(PlantID) && !PlantID.Equals("All", StringComparison.OrdinalIgnoreCase))
                {
                    worksheet.Cells["E5"].Value = "Plant-ID";
                    worksheet.Cells["F5"].Value = PlantID;
                }

                worksheet.Cells["E6"].Value = allShift[0] + " + " + allShift[1] + " + " + allShift[2];
                worksheet.Cells["J6"].Value = allShift[0];
                worksheet.Cells["V6"].Value = allShift[1];
                DateTime DateCheck = DateTime.Now;
                List<string> checkmachinecomponent = new List<string>();
                worksheet.Cells["AH6"].Value = allShift[2];
                worksheet.Cells["AT6"].Value = "Downtime Details - " + allShift[0] + " + " + allShift[1] + " + " + allShift[2];
                if (ShiftwiseData != null && ShiftwiseData.Rows.Count > 0)
                {
                    try
                    {
                        if (DownTable != null && DownTable.Rows.Count > 0)
                        {
                            rowStart = 8; colStart = 46;
                            foreach (DataRow drDownId in DownTable.Rows)
                            {
                                worksheet.Cells[rowStart, colStart].Value = drDownId["downid"];
                                worksheet.Cells[rowStart, colStart].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                                colStart++;
                            }
                        }
                        rowStart = 9; machineName = ShiftwiseData.Rows[0]["MachineID"].ToString();
                        foreach (DataRow drShiftWiseData in ShiftwiseData.Rows)
                        {
                            string value = drShiftWiseData["Pdate"].ToString() + drShiftWiseData["MachineID"].ToString() + drShiftWiseData["Component"].ToString();
                            if (!(checkmachinecomponent.Exists(item => item.Equals(value))))
                            {
                                worksheet.Cells[rowStart, 1].Value = drShiftWiseData["Pdate"];
                                worksheet.Cells[rowStart, 2].Value = drShiftWiseData["MachineID"];
                                worksheet.Cells[rowStart, 3].Value = drShiftWiseData["MachineDescription"];
                                worksheet.Cells[rowStart, 4].Value = drShiftWiseData["Component"];
                                worksheet.Cells[rowStart, 5].Value = drShiftWiseData["ScheduleQty"];
                                worksheet.Cells[rowStart, 6].Value = drShiftWiseData["ReservedQty"];
                                worksheet.Cells[rowStart, 7].Value = drShiftWiseData["ProductionQty"];
                                worksheet.Cells[rowStart, 8].Value = drShiftWiseData["Diff"];
                                worksheet.Cells[rowStart, 9].Value = drShiftWiseData["DaywiseLE"];
                                worksheet.Cells[rowStart, 46].Value = getdouble(drShiftWiseData["A"].ToString());
                                worksheet.Cells[rowStart, 47].Value = getdouble(drShiftWiseData["B"].ToString());
                                worksheet.Cells[rowStart, 48].Value = getdouble(drShiftWiseData["C"].ToString());
                                worksheet.Cells[rowStart, 49].Value = getdouble(drShiftWiseData["D"].ToString());
                                worksheet.Cells[rowStart, 50].Value = getdouble(drShiftWiseData["E"].ToString());
                                worksheet.Cells[rowStart, 51].Value = getdouble(drShiftWiseData["F"].ToString());
                                worksheet.Cells[rowStart, 52].Value = getdouble(drShiftWiseData["G"].ToString());
                                worksheet.Cells[rowStart, 53].Value = getdouble(drShiftWiseData["H"].ToString());
                                worksheet.Cells[rowStart, 54].Value = getdouble(drShiftWiseData["I"].ToString());
                                worksheet.Cells[rowStart, 55].Value = getdouble(drShiftWiseData["J"].ToString());
                                worksheet.Cells[rowStart, 56].Value = getdouble(drShiftWiseData["K"].ToString());
                                worksheet.Cells[rowStart, 57].Value = getdouble(drShiftWiseData["L"].ToString());
                                worksheet.Cells[rowStart, 58].Value = getdouble(drShiftWiseData["M"].ToString());
                                worksheet.Cells[rowStart, 59].Value = getdouble(drShiftWiseData["N"].ToString());
                                worksheet.Cells[rowStart, 60].Value = getdouble(drShiftWiseData["O"].ToString());
                                worksheet.Cells[rowStart, 61].Value = getdouble(drShiftWiseData["P"].ToString());
                            }
                            currentShift = drShiftWiseData["Shift"].ToString();
                            if (currentShift.Equals(allShift[0]))
                            {
                                count++;
                                PlotShiftValues(drShiftWiseData, rowStart, worksheet, 10, rowindex);
                            }
                            if (currentShift.Equals(allShift[1]))
                            {
                                PlotShiftValues(drShiftWiseData, rowStart, worksheet, 22, rowindex);
                            }
                            if (currentShift.Equals(allShift[2]))
                            {
                                PlotShiftValues(drShiftWiseData, rowStart, worksheet, 34, rowindex);
                            }
                            rowStart++;
                            if (drShiftWiseData["MachineID"].ToString() != machineName)
                            {
                                count = 0; rowindex = rowStart - 1;
                            }

                            checkmachinecomponent.Add(value);
                            machineName = drShiftWiseData["MachineID"].ToString();
                        }
                        machineName = ShiftwiseData.Rows[0]["MachineID"].ToString();
                        DateTime.TryParse(ShiftwiseData.Rows[0]["Pdate"].ToString(), out DateCheck);
                        partNumber = ShiftwiseData.Rows[0]["Component"].ToString();
                        firstRow = 9; count = 9;
                        foreach (DataRow drShiftWiseData in ShiftwiseData.Rows)
                        {
                            colStart = 1;
                            if (drShiftWiseData[1].ToString() != machineName || DateCheck != Convert.ToDateTime(drShiftWiseData["Pdate"].ToString()))
                            {
                                if (firstRow != count)
                                {
                                    worksheet.Cells[firstRow, 2, count - 1, 2].Merge = true;
                                    worksheet.Cells[firstRow, 3, count - 1, 3].Merge = true;
                                    worksheet.Cells[firstRow, 1, count - 1, 1].Merge = true;
                                    worksheet.Cells[firstRow, 9, count - 1, 9].Merge = true;
                                    worksheet.Cells[firstRow, 19, count - 1, 19].Merge = true;
                                    worksheet.Cells[firstRow, 31, count - 1, 31].Merge = true;
                                    worksheet.Cells[firstRow, 43, count - 1, 43].Merge = true;
                                    if (colorshading)
                                    {
                                        worksheet.Cells[firstRow, 1, count - 1, 61].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[firstRow, 1, count - 1, 61].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                                        colorshading = false;
                                    }
                                    else
                                    {
                                        worksheet.Cells[firstRow, 1, count - 1, 61].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[firstRow, 1, count - 1, 61].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                                        colorshading = true;
                                    }
                                }
                                firstRow = count;
                            }
                            count++;
                            DateTime.TryParse(drShiftWiseData["Pdate"].ToString(), out DateCheck);
                            machineName = drShiftWiseData[1].ToString();
                        }
                        worksheet.Cells[firstRow, 2, count - 1, 2].Merge = true;
                        worksheet.Cells[firstRow, 3, count - 1, 3].Merge = true;
                        worksheet.Cells[firstRow, 1, count - 1, 1].Merge = true;
                        worksheet.Cells[firstRow, 9, count - 1, 9].Merge = true;
                        firstRow = 9; count = 9;
                        machineName = ShiftwiseData.Rows[0]["MachineID"].ToString();
                        DateTime.TryParse(ShiftwiseData.Rows[0]["MachineID"].ToString(), out DateCheck);
                        foreach (DataRow drShiftWiseData in ShiftwiseData.Rows)
                        {
                            if (drShiftWiseData[1].ToString() != machineName || drShiftWiseData[3].ToString() != partNumber || DateCheck != Convert.ToDateTime(drShiftWiseData["Pdate"].ToString()))
                            {
                                if (firstRow != count)
                                {
                                    worksheet.Cells[firstRow, 4, count - 1, 4].Merge = true;
                                    worksheet.Cells[firstRow, 5, count - 1, 5].Merge = true;
                                    worksheet.Cells[firstRow, 6, count - 1, 6].Merge = true;
                                    worksheet.Cells[firstRow, 7, count - 1, 7].Merge = true;
                                    worksheet.Cells[firstRow, 8, count - 1, 8].Merge = true;
                                    for (int col = 46; col < 62; col++)
                                    {
                                        worksheet.Cells[firstRow, col, count - 1, col].Merge = true;
                                    }

                                }
                                firstRow = count;
                            }

                            count++;
                            partNumber = drShiftWiseData[3].ToString();
                            machineName = drShiftWiseData[1].ToString();
                            DateTime.TryParse(drShiftWiseData["Pdate"].ToString(), out DateCheck);
                        }
                        worksheet.Cells[firstRow, 4, count - 1, 4].Merge = true;
                        worksheet.Cells[firstRow, 5, count - 1, 5].Merge = true;
                        worksheet.Cells[firstRow, 6, count - 1, 6].Merge = true;
                        worksheet.Cells[firstRow, 7, count - 1, 7].Merge = true;
                        worksheet.Cells[firstRow, 8, count - 1, 8].Merge = true;
                        for (int col = 46; col < 62; col++)
                        {
                            worksheet.Cells[firstRow, col, count - 1, col].Merge = true;
                        }

                        firstRow = 9; count = 9; string Dieno = ShiftwiseData.Rows[0]["DieNo"].ToString();
                        string shift = ShiftwiseData.Rows[0]["Shift"].ToString();
                        foreach (DataRow rows in ShiftwiseData.Rows)
                        {
                            if ((!(Dieno.Equals(rows["DieNo"].ToString(), StringComparison.OrdinalIgnoreCase))) || (!(shift.Equals(rows["Shift"].ToString(), StringComparison.OrdinalIgnoreCase))) && (!string.IsNullOrEmpty(rows["DieNo"].ToString())) && !(rows["DieNo"].ToString().Equals("0", StringComparison.OrdinalIgnoreCase)))
                            {
                                if (firstRow != count)
                                {
                                    if (shift.Equals(allShift[0]))
                                    {
                                        worksheet.Cells[firstRow, 14, count - 1, 14].Merge = true;
                                    }
                                    if (shift.Equals(allShift[1]))
                                    {
                                        worksheet.Cells[firstRow, 26, count - 1, 26].Merge = true;
                                    }
                                    if (shift.Equals(allShift[2]))
                                    {
                                        worksheet.Cells[firstRow, 38, count - 1, 38].Merge = true;
                                    }
                                }
                                firstRow = count;
                            }
                            if (string.IsNullOrEmpty(rows["DieNo"].ToString()) || rows["DieNo"].ToString().Equals("0", StringComparison.OrdinalIgnoreCase))
                            {
                                firstRow = count;
                            }
                            count++;
                            Dieno = rows["DieNo"].ToString();
                            shift = rows["Shift"].ToString();
                        }
                        worksheet.Cells[5, 1, count - 1, 63].AutoFitColumns();

                        worksheet = Styling(worksheet, 5, 1, count, 61);
                        //Excel.SaveAs(newFile);
                        DownloadMultipleFile(Destination, Excel.GetAsByteArray());
                        Saved = true;
                    }

                    catch (Exception ex)
                    {
                        Logger.WriteErrorLog(ex.Message);
                        Saved = false;
                    }
                }
            }
            return Saved;
        }

        private static void PlotShiftValues(DataRow drShiftWiseData, int rowStart, ExcelWorksheet worksheet, int colIndex, int shiftstart)
        {
            worksheet.Cells[rowStart, colIndex].Value = drShiftWiseData["Component"];
            worksheet.Cells[rowStart, colIndex + 1].Value = drShiftWiseData["ProdQty"];
            worksheet.Cells[rowStart, colIndex + 2].Value = drShiftWiseData["FGBatchID"];
            worksheet.Cells[rowStart, colIndex + 3].Value = drShiftWiseData["DieNo"];
            if (!string.IsNullOrEmpty(drShiftWiseData["RunningDieLife"].ToString()))
                worksheet.Cells[rowStart, colIndex + 4].Value = Convert.ToInt32(drShiftWiseData["RunningDieLife"].ToString());
            worksheet.Cells[rowStart, colIndex + 5].Value = drShiftWiseData["AvgDieLife"];
            worksheet.Cells[rowStart, colIndex + 6].Value = drShiftWiseData["MaxDieLife"];
            worksheet.Cells[rowStart, colIndex + 7].Value = drShiftWiseData["StdTime"];
            worksheet.Cells[rowStart, colIndex + 8].Value = drShiftWiseData["ActTime"];
            worksheet.Cells[rowStart, colIndex + 9].Value = drShiftWiseData["ShiftwiseLE"];
            worksheet.Cells[rowStart, colIndex + 10].Value = getdouble(drShiftWiseData["Downtime"].ToString());
            worksheet.Cells[rowStart, colIndex + 11].Value = drShiftWiseData["TopDowntime"];
            if (colIndex == 22)
            {
                worksheet.Cells[shiftstart, 31].Value = drShiftWiseData["ShiftwiseLE"];
            }
            if (colIndex == 34)
            {
                worksheet.Cells[shiftstart, 43].Value = drShiftWiseData["ShiftwiseLE"];
            }
        }

        public static bool PlotProdReportMachinewiseFormat1Shift(string Path, string Destination, string fromDate, string toDate, string shift, string plantId, DataTable shiftwiseProdData, bool Workorder, DataTable totalCountData, string timeFormat, bool isQERequired)
        {
            string machineShift = string.Empty;
            string prevMachineShift = string.Empty;
            int count = 0;
            bool Saved = false;
            FileInfo newFile = new FileInfo(Path);
            ExcelPackage Excel = new ExcelPackage(newFile, true);
            int rowStart = 8;
            var worksheet = Excel.Workbook.Worksheets[1];

            worksheet.PrinterSettings.PaperSize = ePaperSize.A3;
            worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
            worksheet.PrinterSettings.HorizontalCentered = true;
            worksheet.PrinterSettings.FitToWidth = 1;
            worksheet.PrinterSettings.FitToHeight = 0;
            worksheet.PrinterSettings.FitToPage = true;

            if (worksheet != null)
            {
                if (!string.IsNullOrEmpty(plantId) && plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
                {
                    worksheet.Cells["B4"].Value = "All";
                }
                worksheet.Cells["B5"].Value = fromDate;
                worksheet.Cells["G5"].Value = toDate;
                worksheet.Cells["B4"].Value = plantId == "" ? "All" : plantId;
                worksheet.Cells["G4"].Value = shift;
                try
                {
                    if (shiftwiseProdData != null && shiftwiseProdData.Rows.Count > 0)
                    {
                        prevMachineShift = shiftwiseProdData.Rows[0]["MachineID"].ToString() + shiftwiseProdData.Rows[0]["Shift"].ToString() + shiftwiseProdData.Rows[0]["Day"].ToString();
                        foreach (DataRow row in shiftwiseProdData.Rows)
                        {
                            worksheet.Cells[rowStart, 1].Value = Convert.ToDateTime(row["Day"]).ToString("dd-MM-yyyy");
                            worksheet.Cells[rowStart, 2].Value = row["Shift"];
                            worksheet.Cells[rowStart, 3].Value = row["MachineID"];
                            worksheet.Cells[rowStart, 4].Value = row["OperatorName"];
                            worksheet.Cells[rowStart, 5].Value = row["WorkorderNo"];
                            worksheet.Cells[rowStart, 6].Value = row["ComponentID"];
                            worksheet.Cells[rowStart, 7].Value = row["OperationNo"];
                            worksheet.Cells[rowStart, 8].Value = row["ProdCount"];
                            worksheet.Cells[rowStart, 9].Value = row["RejCount"];
                            worksheet.Cells[rowStart, 10].Value = row["AcceptedParts"];
                            setTimeSpanFormat(timeFormat, worksheet, rowStart, 11, row["UtilisedTime"].ToString());
                            //worksheet.Cells[rowStart, 10].Value = row["StdCycleTime"];
                            //worksheet.Cells[rowStart, 11].Value = row["AvgCycleTime"];
                            setTimeSpanFormat(timeFormat, worksheet, rowStart, 12, row["StdCycleTime"].ToString());
                            setTimeSpanFormat(timeFormat, worksheet, rowStart, 13, row["AvgCycleTime"].ToString());
                            worksheet.Cells[rowStart, 14].Value = Math.Round(Convert.ToDouble(row["CycleEffy"]), 2);
                            //worksheet.Cells[rowStart, 13].Value = row["StdLoadUnload"];
                            //worksheet.Cells[rowStart, 14].Value = row["AvgLoadUnload"];
                            setTimeSpanFormat(timeFormat, worksheet, rowStart, 15, row["StdLoadUnload"].ToString());
                            setTimeSpanFormat(timeFormat, worksheet, rowStart, 16, row["AvgLoadUnload"].ToString());
                            worksheet.Cells[rowStart, 17].Value = Math.Round(Convert.ToDouble(row["LoadUnloadEffy"]), 2);
                            //worksheet.Cells[rowStart, 16].Value = row["DownTime"];
                            setTimeSpanFormat(timeFormat, worksheet, rowStart, 18, row["DownTime"].ToString());
                            worksheet.Cells[rowStart, 19].Value = Math.Round(Convert.ToDouble(row["AEffy"]), 2);
                            worksheet.Cells[rowStart, 20].Value = Math.Round(Convert.ToDouble(row["PEffy"]), 2);
                            worksheet.Cells[rowStart, 21].Value = Math.Round(Convert.ToDouble(row["QEffy"]), 2);
                            worksheet.Cells[rowStart, 22].Value = Math.Round(Convert.ToDouble(row["OEffy"]), 2);
                           // worksheet.Cells[rowStart, 23].Value = row["RejectionReason"];
                            machineShift = row["MachineID"].ToString() + row["Shift"].ToString() + row["Day"].ToString();
                            if (machineShift == prevMachineShift)
                            {
                                worksheet.Cells[rowStart - count, 11, rowStart, 11].Merge = true;
                                worksheet.Cells[rowStart - count, 18, rowStart, 18].Merge = true;
                                worksheet.Cells[rowStart - count, 19, rowStart, 19].Merge = true;
                                worksheet.Cells[rowStart - count, 20, rowStart, 20].Merge = true;
                                worksheet.Cells[rowStart - count, 21, rowStart, 21].Merge = true;
                                worksheet.Cells[rowStart - count, 22, rowStart, 22].Merge = true;
                                count++;
                            }
                            else
                            {
                                prevMachineShift = machineShift;
                                count = 1;
                            }
                            worksheet.Row(rowStart).Height = 40;
                            rowStart++;
                        }
                        if (totalCountData != null && totalCountData.Rows.Count > 0)
                        {
                            foreach (DataRow row in totalCountData.Rows)
                            {
                                worksheet.Cells[rowStart, 1, rowStart, 7].Merge = true;
                                worksheet.Cells[rowStart, 1, rowStart, 7].Value = "Total";
                                worksheet.Cells[rowStart, 8].Value = row["ProdCount"];
                                worksheet.Cells[rowStart, 9].Value = row["RejCount"];
                                worksheet.Cells[rowStart, 10].Value = row["AcceptedParts"];
                                worksheet.Cells[rowStart, 11].Value = row["UtilisedTime"];
                                worksheet.Cells[rowStart, 12].Value = row["StdCycleTime"];
                                worksheet.Cells[rowStart, 13].Value = row["AvgCycleTime"];
                                worksheet.Cells[rowStart, 14].Value = Math.Round(Convert.ToDouble(row["CycleEffy"]), 2);
                                worksheet.Cells[rowStart, 15].Value = row["StdLoadUnload"];
                                worksheet.Cells[rowStart, 16].Value = row["AvgLoadUnload"];
                                worksheet.Cells[rowStart, 17].Value = Math.Round(Convert.ToDouble(row["LoadUnloadEffy"]), 2);
                                worksheet.Cells[rowStart, 18].Value = row["DownTime"];
                                worksheet.Cells[rowStart, 19].Value = Math.Round(Convert.ToDouble(row["AEffy"]), 2);
                                worksheet.Cells[rowStart, 20].Value = Math.Round(Convert.ToDouble(row["PEffy"]), 2);
                                worksheet.Cells[rowStart, 21].Value = Math.Round(Convert.ToDouble(row["QEffy"]), 2);
                                worksheet.Cells[rowStart, 22].Value = Math.Round(Convert.ToDouble(row["OEffy"]), 2);
                                worksheet.Row(rowStart).Style.Font.Size = 11;
                                worksheet.Row(rowStart).Style.Font.Bold = true;
                                worksheet.Cells[rowStart, 1, rowStart, 22].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[rowStart, 1, rowStart, 22].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 213, 180));
                                rowStart++;
                            }
                        }
                        worksheet.Cells[8, 1, rowStart, 2].AutoFitColumns();
                        worksheet.Column(3).Style.WrapText = true;
                        worksheet.Column(3).Width = 30;
                        worksheet.Column(6).Style.WrapText = true;
                        worksheet.Column(6).Width = 33;
                        worksheet.Column(4).Style.WrapText = true;
                        worksheet.Column(4).Width = 25;
                        worksheet.Cells[7, 11, rowStart, 22].AutoFitColumns();
                        if (!Workorder)
                        {
                            worksheet.Column(5).Hidden = true;
                            //worksheet.Column(2).Hidden = true;
                            //worksheet.Cells["C4"].Value = fromDate;
                            //worksheet.Cells["D4"].Value = "Shift :";
                            //worksheet.Cells["E4"].Value = shift;
                            //worksheet.Cells["F4"].Value = "Plant :";
                            //worksheet.Cells["B4"].Value = plantId == "" ? "All" : plantId;


                        }
                        else
                        {
                            //worksheet.Cells["B4"].Value = fromDate;
                            //worksheet.Cells["D4"].Value = shift;
                            //worksheet.Cells["F4"].Value = plantId;
                            //worksheet.Cells["B4"].Value = "";

                        }
                        //Excel.SaveAs(newFile);
                        rowStart--;
                        //worksheet.Cells[5, 1, rowStart, 15].AutoFitColumns();
                        worksheet = Styling(worksheet, 8, 1, rowStart, 22);


                        if (isQERequired == false)
                        {
                            worksheet.Column(21).Hidden = true;
                        }

                        DownloadMultipleFile(Destination, Excel.GetAsByteArray());
                        Saved = true;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.Message);
                    Saved = false;
                }
            }
            return Saved;
        }

        public static bool PlotDantalProductionReport_agg(string Path, string Destination, string fromDate, string toDate, string plantId,string cellId, string machineId, DataTable ProdData, string timeFormat, bool isQERequired,DataTable dt_downcodes,string plant)
        {
            string machineShift = string.Empty;
            string prevMachineShift = string.Empty;
            bool Saved = false;
            FileInfo newFile = new FileInfo(Path);
            ExcelPackage Excel = new ExcelPackage(newFile, true);
            int row = 0;
            var worksheet = Excel.Workbook.Worksheets[1];

            //worksheet.PrinterSettings.PaperSize = ePaperSize.A3;
            //worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
            //worksheet.PrinterSettings.HorizontalCentered = true;
            //worksheet.PrinterSettings.FitToWidth = 1;
            //worksheet.PrinterSettings.FitToHeight = 0;
            //worksheet.PrinterSettings.FitToPage = true;

            if (worksheet != null)
            {
                worksheet.Cells["B2"].Value = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy");
                worksheet.Cells["D2"].Value = Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy"); 
                try
                {
                    //if (!string.IsNullOrEmpty(plantId) && plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    worksheet.Cells["F2"].Value = "All";
                    //}
                    //else
                    //{
                        worksheet.Cells["F2"].Value = plant;
                    //}
                    worksheet.Cells["H2"].Value = cellId;
                    worksheet.Cells[2, 10, 2, 17].Merge = true;
                    //worksheet.Cells["J2"].Value = machineId;

                    var rowheader = 3;
                    var colheader = 14;
                    foreach (DataRow item in dt_downcodes.Rows)
                    {
                        worksheet.Cells[rowheader, colheader].Value = item["downid"].ToString();
                        colheader++;
                    }
                    if (ProdData != null && ProdData.Rows.Count > 0)
                    {
                        row = 4;
                        int colCount = 1;
                        var distCell = ProdData.AsEnumerable().Select(k => k.Field<string>("GroupID")).Distinct().ToList();
                        for (int i = 0; i < ProdData.Rows.Count; i++)
                        {
                            colCount = 1;
                            worksheet.Cells[row, colCount].Value = Convert.ToDateTime(ProdData.Rows[i]["Date"]).ToString("dd/MM/yyyy");
                            colCount++;
                            worksheet.Cells[row, colCount].Value = ProdData.Rows[i]["GroupID"];
                            colCount++;
                            worksheet.Cells[row, colCount].Value = ProdData.Rows[i]["MachineId"];
                            colCount++;
                            worksheet.Cells[row, colCount].Value = ProdData.Rows[i]["MachineDescription"];
                            colCount++;
                            worksheet.Cells[row, colCount].Value = ProdData.Rows[i]["shift"];
                            colCount++;
                            worksheet.Cells[row, colCount].Value = Math.Round(Convert.ToDouble(ProdData.Rows[i]["Target"].ToString()), 2);
                            colCount++;
                            worksheet.Cells[row, colCount].Value = Math.Round(Convert.ToDouble(ProdData.Rows[i]["Actual"].ToString()), 2);
                            colCount++;
                            worksheet.Cells[row, colCount].Value = Math.Round(Convert.ToDouble(ProdData.Rows[i]["Diff"].ToString()), 2);
                            colCount++;
                            if (!(ProdData.Rows[i]["AE"] is DBNull))
                                worksheet.Cells[row, colCount].Value = Convert.ToDouble(ProdData.Rows[i]["AE"]);
                            colCount++;
                            if (!(ProdData.Rows[i]["PE"] is DBNull))
                                worksheet.Cells[row, colCount].Value = Convert.ToDouble(ProdData.Rows[i]["PE"]);
                            colCount++;
                            //if (!(dt.Rows[i]["QualityEfficiency"] is DBNull))
                            //    wsDts.Cells[row, colCount].Value = Convert.ToDouble(dt.Rows[i]["QualityEfficiency"]);
                            //colCount++;
                            if (!(ProdData.Rows[i]["OEE"] is DBNull))
                                worksheet.Cells[row, colCount].Value = Convert.ToDouble(ProdData.Rows[i]["OEE"]);
                            colCount++;
                            setTimeSpanFormat(timeFormat, worksheet, row, colCount, ProdData.Rows[i]["UtilisedTime"].ToString());
                            colCount++;
                            setTimeSpanFormat(timeFormat, worksheet, row, colCount, ProdData.Rows[i]["DownTime"].ToString());
                            colCount++;
                            setTimeSpanFormat(timeFormat, worksheet, row, colCount, ProdData.Rows[i]["A"].ToString());
                            colCount++;
                            setTimeSpanFormat(timeFormat, worksheet, row, colCount, ProdData.Rows[i]["B"].ToString());
                            colCount++;
                            setTimeSpanFormat(timeFormat, worksheet, row, colCount, ProdData.Rows[i]["C"].ToString());
                            colCount++;
                            setTimeSpanFormat(timeFormat, worksheet, row, colCount, ProdData.Rows[i]["D"].ToString());
                            colCount++;
                            row++;
                        }

                        //if (isCellDifined == false)
                        //{
                        //    worksheet.Column(1).Hidden = true;
                        //}
                        //if (isQERequired == false)
                        //{
                        //    worksheet.Column(21).Hidden = true;
                        //}
                        setThinBorder(worksheet, 4, 1, row - 1, 17);
                        DownloadMultipleFile(Destination, Excel.GetAsByteArray());
                        Logger.WriteDebugLog("Production Dantal Report generated sucessfully.");
                        Saved = true;
                    }
                    else
                        Saved = false;
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.Message);
                    Saved = false;
                }
            }
            return Saved;
        }

        private static void setTimeSpanFormat(string timeFormat, ExcelWorksheet worksheet, int rowPos, int colPos, string value)
        {
            try
            {
                if (timeFormat.Equals("hh:mm:ss"))
                {
                    //TimeSpan timeSpan = TimeSpan.Parse(dt.Rows[i][item].ToString());
                    var valueSplit = value.Split(':');
                    TimeSpan timeSpan = new TimeSpan(int.Parse(valueSplit[0]),    // hours
                                                     int.Parse(valueSplit[1]),    // minutes
                                                     int.Parse(valueSplit[2]));
                    worksheet.Cells[rowPos, colPos].Value = timeSpan;
                    worksheet.Cells[rowPos, colPos].Style.Numberformat.Format = "[h]:mm:ss";
                }
                else
                {
                    worksheet.Cells[rowPos, colPos].Value = string.IsNullOrEmpty(value) ? 0 : Convert.ToDouble(value);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("setTimeSpanFormat = " + ex.Message);
            }
        }
        public static bool PlotProdReportMachinewiseFormat1Day(string Path, string Destination, string fromDate, string toDate, string shift, string plantId, DataTable daywiseProdData, bool WorkOrder, string timeFormat, bool isQERequired, DataTable daywiseProdDataSummary)
        {
            bool Saved = false;
            FileInfo newFile = new FileInfo(Path);
            ExcelPackage Excel = new ExcelPackage(newFile, true);
            int rowStart = 7;
            var worksheet = Excel.Workbook.Worksheets[1];
            worksheet.PrinterSettings.PaperSize = ePaperSize.A3;
            worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
            worksheet.PrinterSettings.HorizontalCentered = true;
            worksheet.PrinterSettings.FitToWidth = 1;
            worksheet.PrinterSettings.FitToHeight = 0;
            worksheet.PrinterSettings.FitToPage = true;
            if (worksheet != null)
            {
                //worksheet.Cells["B4"].Value = day;
                worksheet.Cells["B4"].Value = fromDate;
                worksheet.Cells["F4"].Value = toDate;
                try
                {
                    if (daywiseProdData != null && daywiseProdData.Rows.Count > 0)
                    {
                        var distMachineID = daywiseProdData.AsEnumerable().Select(k => new { date = k.Field<DateTime>("Day"), machineID = k.Field<string>("MachineID") }).Distinct().ToList();
                        // int shiftMergeCount = 0;
                        for (int machineCount = 0; machineCount < distMachineID.Count; machineCount++)
                        {
                            var dtRows = daywiseProdData.AsEnumerable().Where(k => k.Field<string>("MachineID") == distMachineID[machineCount].machineID && k.Field<DateTime>("Day") == distMachineID[machineCount].date);
                            if (dtRows != null)
                            {
                                int mergeCount = dtRows.Count();
                                int rowCount = 0;
                                foreach (var row in dtRows)
                                {
                                    worksheet.Cells[rowStart, 1].Value = Convert.ToDateTime(row["Day"]).ToString("dd-MM-yyyy");
                                    worksheet.Cells[rowStart, 2].Value = row["MachineID"];
                                    worksheet.Cells[rowStart, 3].Value = row["OperatorName"];
                                    worksheet.Cells[rowStart, 4].Value = row["WorkorderNo"];
                                    worksheet.Cells[rowStart, 5].Value = row["ComponentID"];
                                    worksheet.Cells[rowStart, 6].Value = row["OperationNo"];
                                    worksheet.Cells[rowStart, 7].Value = row["Shift"];
                                    worksheet.Cells[rowStart, 8].Value = row["ProdCount"];
                                    worksheet.Cells[rowStart, 9].Value = row["RejCount"];
                                    worksheet.Cells[rowStart, 10].Value = row["AcceptedParts"];
                                    if (rowCount == 0)
                                    {
                                        setTimeSpanFormat(timeFormat, worksheet, rowStart, 11, row["UtilisedTime"].ToString());
                                    }
                                    //worksheet.Cells[rowStart, 8].Value = row["StdCycleTime"];
                                    //worksheet.Cells[rowStart, 9].Value = row["AvgCycleTime"];
                                    setTimeSpanFormat(timeFormat, worksheet, rowStart, 12, row["StdCycleTime"].ToString());
                                    setTimeSpanFormat(timeFormat, worksheet, rowStart, 13, row["AvgCycleTime"].ToString());
                                    worksheet.Cells[rowStart, 14].Value = Math.Round(Convert.ToDouble(row["CycleEffy"]), 2);
                                    //worksheet.Cells[rowStart, 13].Value = row["StdLoadUnload"];
                                    //worksheet.Cells[rowStart, 14].Value = row["AvgLoadUnload"];
                                    setTimeSpanFormat(timeFormat, worksheet, rowStart, 15, row["StdLoadUnload"].ToString());
                                    setTimeSpanFormat(timeFormat, worksheet, rowStart, 16, row["AvgLoadUnload"].ToString());
                                    worksheet.Cells[rowStart, 17].Value = Math.Round(Convert.ToDouble(row["LoadUnloadEffy"]), 2);
                                    // worksheet.Cells[rowStart, 16].Value = row["DownTime"];
                                    if (rowCount == 0)
                                    {
                                        setTimeSpanFormat(timeFormat, worksheet, rowStart, 18, row["DownTime"].ToString());
                                    }
                                    if (rowCount == 0)
                                    {
                                        worksheet.Cells[rowStart, 19].Value = Math.Round(Convert.ToDouble(row["AEffy"]), 2);
                                    }
                                    if (rowCount == 0)
                                    {
                                        worksheet.Cells[rowStart, 20].Value = Math.Round(Convert.ToDouble(row["PEffy"]), 2);
                                    }
                                    if (rowCount == 0)
                                    {
                                        worksheet.Cells[rowStart, 21].Value = Math.Round(Convert.ToDouble(row["QEffy"]), 2);
                                    }
                                    if (rowCount == 0)
                                    {
                                        worksheet.Cells[rowStart, 22].Value = Math.Round(Convert.ToDouble(row["OEffy"]), 2);
                                    }
                                    //worksheet.Cells[rowStart, 23].Value = row["RejectionReason"];
                                    worksheet.Row(rowStart).Height = 40;
                                    rowStart++;
                                    rowCount++;
                                }
                                worksheet.Cells[rowStart - mergeCount, 11, rowStart - 1, 11].Merge = true;
                                worksheet.Cells[rowStart - mergeCount, 18, rowStart - 1, 18].Merge = true;
                                worksheet.Cells[rowStart - mergeCount, 19, rowStart - 1, 19].Merge = true;
                                worksheet.Cells[rowStart - mergeCount, 20, rowStart - 1, 20].Merge = true;
                                worksheet.Cells[rowStart - mergeCount, 21, rowStart - 1, 21].Merge = true;
                                worksheet.Cells[rowStart - mergeCount, 22, rowStart - 1, 22].Merge = true;
                            }
                        }
                        if (daywiseProdDataSummary.Rows.Count > 0)
                        {
                            worksheet.Cells[rowStart, 1].Value = "Total";
                            worksheet.Cells[rowStart, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[rowStart, 1, rowStart, 22].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowStart, 1, rowStart, 22].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 228, 214));
                            worksheet.Row(rowStart).Style.Font.Bold = true;
                            worksheet.Cells[rowStart, 1, rowStart, 7].Merge = true;
                            var firstRow = daywiseProdDataSummary.Rows[0];
                            worksheet.Cells[rowStart, 8].Value = firstRow["ProdCount"];
                            worksheet.Cells[rowStart, 9].Value = firstRow["RejCount"];
                            worksheet.Cells[rowStart, 10].Value = firstRow["AcceptedParts"];
                            worksheet.Cells[rowStart, 11].Value = firstRow["UtilisedTime"].ToString();
                            worksheet.Cells[rowStart, 18].Value = firstRow["DownTime"].ToString();
                            worksheet.Cells[rowStart, 19].Value = Convert.ToDouble(firstRow["AvgAEffy"]);
                            worksheet.Cells[rowStart, 20].Value = Convert.ToDouble(firstRow["AvgPEffy"]);
                            worksheet.Cells[rowStart, 21].Value = Convert.ToDouble(firstRow["AvgQEffy"]);
                            worksheet.Cells[rowStart, 22].Value = Convert.ToDouble(firstRow["AvgOEEffy"]);
                        }
                        //Excel.SaveAs(newFile);
                        worksheet.Cells[7, 1, rowStart, 1].AutoFitColumns();
                        worksheet.Column(2).Width = 30;
                        worksheet.Column(2).Style.WrapText = true;
                        worksheet.Column(3).Width = 25;
                        worksheet.Column(3).Style.WrapText = true;
                        worksheet.Column(5).Width = 33;
                        worksheet.Column(5).Style.WrapText = true;
                        worksheet.Cells[7, 6, rowStart, 7].AutoFitColumns();
                        worksheet.Cells[7, 11, rowStart, 22].AutoFitColumns();
                        if (!WorkOrder)
                        {
                            worksheet.Column(4).Hidden = true;
                            //worksheet.Cells["C4"].Value = day;
                        }
                        else
                        {
                            //worksheet.Cells["B4"].Value = day;

                        }
                        worksheet = Styling(worksheet, 7, 1, rowStart, 22);
                        if (isQERequired == false)
                        {
                            worksheet.Column(21).Hidden = true;
                        }
                        DownloadMultipleFile(Destination, Excel.GetAsByteArray());
                        Saved = true;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.Message);
                    Saved = false;
                }
            }
            return Saved;
        }

        public static bool PlotProdReportMachinewiseFormat2DayForConfidental(string Path, string Destination, string fromDate, string toDate, string shift, string plantId, DataTable daywiseProdData, bool WorkOrder, bool isQERequired)
        {
            bool Saved = false;
            FileInfo newFile = new FileInfo(Path);
            ExcelPackage Excel = new ExcelPackage(newFile, true);
            var worksheet = Excel.Workbook.Worksheets[1];
            if (worksheet != null)
            {
                try
                {
                    worksheet.Cells["B3"].Value = fromDate;
                    worksheet.Cells["D3"].Value = toDate;
                    worksheet.Cells["F3"].Value = plantId == "" ? "All" : plantId;
                    if (daywiseProdData != null && daywiseProdData.Rows.Count > 0)
                    {
                        int rowCount = 6;
                        int cellCount = 1;
                        for (int i = 0; i < daywiseProdData.Rows.Count; i++)
                        {
                            cellCount = 1;
                            worksheet.Cells[rowCount, cellCount].Value = daywiseProdData.Rows[i]["Pdate"].ToString() == "" ? daywiseProdData.Rows[i]["Pdate"].ToString() : Convert.ToDateTime(daywiseProdData.Rows[i]["Pdate"].ToString()).ToString("yyyy-MM-dd");
                            cellCount++;
                            worksheet.Cells[rowCount, cellCount].Value = daywiseProdData.Rows[i]["MachineID"].ToString();
                            cellCount++;
                            worksheet.Cells[rowCount, cellCount].Value = daywiseProdData.Rows[i]["description"].ToString();
                            cellCount++;
                            worksheet.Cells[rowCount, cellCount].Value = Convert.ToDouble(daywiseProdData.Rows[i]["AEffy"].ToString());
                            cellCount++;
                            worksheet.Cells[rowCount, cellCount].Value = Convert.ToDouble(daywiseProdData.Rows[i]["PEffy"].ToString());
                            cellCount++;
                            worksheet.Cells[rowCount, cellCount].Value = Convert.ToDouble(daywiseProdData.Rows[i]["QEffy"].ToString());
                            cellCount++;
                            worksheet.Cells[rowCount, cellCount].Value = Convert.ToDouble(daywiseProdData.Rows[i]["OEffy"].ToString());
                            cellCount++;
                            worksheet.Cells[rowCount, cellCount].Value = Convert.ToDouble(daywiseProdData.Rows[i]["ProdCount"].ToString());
                            cellCount++;
                            setTimeSpanFormat("hh:mm:ss", worksheet, rowCount, cellCount, daywiseProdData.Rows[i]["UtilisedTime"].ToString());
                            //worksheet.Cells[rowCount, cellCount].Value = daywiseProdData.Rows[i]["UtilisedTime"].ToString();
                            cellCount++;
                            setTimeSpanFormat("hh:mm:ss", worksheet, rowCount, cellCount, daywiseProdData.Rows[i]["DownTime"].ToString());
                            // worksheet.Cells[rowCount, cellCount].Value = daywiseProdData.Rows[i]["DownTime"].ToString();
                            rowCount++;
                        }
                        for (int i = 1; i <= cellCount; i++)
                        {
                            worksheet.Column(i).AutoFit();
                        }
                        setThinBorder(worksheet, 6, 1, rowCount - 1, cellCount);
                        if (isQERequired == false)
                        {
                            worksheet.Column(6).Hidden = true;
                        }
                        DownloadMultipleFile(Destination, Excel.GetAsByteArray());
                        Saved = true;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.Message);
                    Saved = false;
                }
            }
            return Saved;
        }
        private static void setThinBorder(ExcelWorksheet worksheet, int fromRow, int fromColumn, int toRow, int toColumn)
        {
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
        }

        public static bool PlotProdReportMachinewiseConsolidated(string machineId, string Path, string Destination, string fromdate, string todate, DataTable consolidatedProdReportData, DataTable totalData, bool isQERequired, string timeFormat)
        {
            bool Saved = false;
            FileInfo newFile = new FileInfo(Path);
            ExcelPackage Excel = new ExcelPackage(newFile, true);
            int rowStart = 7;
            var worksheet = Excel.Workbook.Worksheets[1];
            if (worksheet != null)
            {
                worksheet.Cells["B4"].Value = fromdate;
                worksheet.Cells["D4"].Value = todate;
                try
                {
                    if (consolidatedProdReportData != null && consolidatedProdReportData.Rows.Count > 0)
                    {
                        foreach (DataRow row in consolidatedProdReportData.Rows)
                        {
                            worksheet.Cells[rowStart, 1].Value = row["MachineID"];
                            worksheet.Cells[rowStart, 2].Value = row["ProdCount"];
                            worksheet.Cells[rowStart, 3].Value = row["AcceptedParts"];
                            worksheet.Cells[rowStart, 4].Value = row["RejCount"];
                            worksheet.Cells[rowStart, 5].Value = row["Rework"];
                            setTimeSpanFormat(timeFormat, worksheet, rowStart, 6, row["totaltime"].ToString());
                            setTimeSpanFormat(timeFormat, worksheet, rowStart, 7, row["UtilisedTime"].ToString());
                            setTimeSpanFormat(timeFormat, worksheet, rowStart, 8, row["DownTime"].ToString());
                            worksheet.Cells[rowStart, 9].Value = row["AEffy"];
                            worksheet.Cells[rowStart, 10].Value = row["PEffy"];
                            worksheet.Cells[rowStart, 11].Value = row["QEffy"];
                            worksheet.Cells[rowStart, 12].Value = row["OEffy"];

                            rowStart++;
                        }
                        foreach (DataRow row in totalData.Rows)
                        {
                            worksheet.Cells[rowStart, 1].Value = "Total/Average";
                            if (machineId.Equals("All", StringComparison.OrdinalIgnoreCase) || machineId == "")
                            {
                                worksheet.Cells[rowStart, 2].Value = row["ProdCount"];
                                worksheet.Cells[rowStart, 3].Value = row["AcceptedParts"];
                                worksheet.Cells[rowStart, 4].Value = row["RejCount"];
                                worksheet.Cells[rowStart, 5].Value = row["Rework"];
                                worksheet.Cells[rowStart, 6].Value = row["Totaltime"];
                                worksheet.Cells[rowStart, 7].Value = row["UtilisedTime"];
                                worksheet.Cells[rowStart, 8].Value = row["DownTime"];
                                worksheet.Cells[rowStart, 9].Value = row["AEffy"];
                                worksheet.Cells[rowStart, 10].Value = row["PEffy"];
                                worksheet.Cells[rowStart, 11].Value = row["QEffy"];
                                worksheet.Cells[rowStart, 12].Value = row["OEffy"];

                            }
                            else
                            {

                                worksheet.Cells[rowStart, 2].Value = row["ProdCount"];
                                worksheet.Cells[rowStart, 3].Value = row["AcceptedParts"];
                                worksheet.Cells[rowStart, 4].Value = row["RejCount"];
                                worksheet.Cells[rowStart, 5].Value = row["Rework"];
                                worksheet.Cells[rowStart, 6].Value = row["CustomTotaltime"];
                                worksheet.Cells[rowStart, 7].Value = row["CustomUtilisedTime"];
                                worksheet.Cells[rowStart, 8].Value = row["CustomDownTime"];
                                worksheet.Cells[rowStart, 9].Value = row["AEffy"];
                                worksheet.Cells[rowStart, 10].Value = row["PEffy"];
                                worksheet.Cells[rowStart, 11].Value = row["QEffy"];
                                worksheet.Cells[rowStart, 12].Value = row["OEffy"];
                            }
                            worksheet.Cells[rowStart, 1, rowStart, 12].Style.Font.Bold = true;
                            worksheet.Cells[rowStart, 1, rowStart, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowStart, 1, rowStart, 12].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 213, 180));
                            rowStart++;
                        }
                        rowStart--;
                        worksheet = Styling(worksheet, 7, 1, rowStart, 11);
                        if (isQERequired == false)
                        {
                            worksheet.Column(11).Hidden = true;
                        }

                        DownloadMultipleFile(Destination, Excel.GetAsByteArray());
                        //Excel.SaveAs(newFile);
                        Saved = true;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.Message);
                    Saved = false;
                }
            }
            return Saved;
        }

        private static ExcelWorksheet Styling(ExcelWorksheet worksheet, int rowstart, int columnstart, int rowend, int columnend)
        {
            worksheet.Cells[rowstart, columnstart, rowend, columnend].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[rowstart, columnstart, rowend, columnend].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[rowstart, columnstart, rowend, columnend].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[rowstart, columnstart, rowend, columnend].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[rowstart, columnstart, rowend, columnend].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[rowstart, columnstart, rowend, columnend].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
            worksheet.Cells[rowstart, columnstart, rowend, columnend].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
            worksheet.Cells[rowstart, columnstart, rowend, columnend].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
            worksheet.Cells[rowstart, columnstart, rowend, columnend].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
            return worksheet;
        }

        private static Double getdouble(string data)
        {
            double Value = 0.00;
            double.TryParse(data, out Value);
            return Value;
        }

        public static bool PlotAggregatedComparisonReportDaily(string Path, string Destination, string fromdate, string todate, DataTable aggregatedComparisonData, Dictionary<string, EfficiencyEntity> machineEfficiencyDetails, bool isQERequired)
        {
            string machineID = string.Empty;
            int startRow = 3;
            DataTable dtMachinewiseEffCompData = new DataTable();
            bool Saved = false;
            FileInfo newFile = new FileInfo(Path);
            ExcelPackage Excel = new ExcelPackage(newFile, true);
            try
            {
                for (int i = 1; i < machineEfficiencyDetails.Count; i++)
                {
                    ExcelWorksheet worksheet = CopyWorksheet(Excel.Workbook, Excel.Workbook.Worksheets[1].Name, machineEfficiencyDetails.ElementAt(i).Key);
                }
                for (int j = 0; j < machineEfficiencyDetails.Count; j++)
                {
                    ExcelWorksheet worksheet = Excel.Workbook.Worksheets[j + 1];
                    if (worksheet != null)
                    {
                        startRow = 3;
                        machineID = machineEfficiencyDetails.ElementAt(j).Key;
                        worksheet.Cells["D1"].Value = machineID;
                        worksheet.Cells["F1"].Value = fromdate;
                        worksheet.Cells["H1"].Value = todate;
                        if (j.Equals(0)) worksheet.Name = machineID;
                        ReviseDailyExcelChartFormulas(worksheet);
                        dtMachinewiseEffCompData = aggregatedComparisonData.AsEnumerable().Where(x => x.Field<string>("MachineID") == machineID).CopyToDataTable();
                        foreach (DataRow row in dtMachinewiseEffCompData.Rows)
                        {
                            worksheet.Cells["B" + startRow].Value = row["OEffy"];
                            worksheet.Cells["C" + startRow].Value = machineEfficiencyDetails[machineID].OE;
                            worksheet.Cells["F" + startRow].Value = row["PEffy"];
                            worksheet.Cells["G" + startRow].Value = machineEfficiencyDetails[machineID].PE;
                            worksheet.Cells["D" + startRow].Value = row["AEffy"];
                            worksheet.Cells["E" + startRow].Value = machineEfficiencyDetails[machineID].AE;
                            worksheet.Cells["H" + startRow].Value = row["QEffy"];
                            worksheet.Cells["I" + startRow].Value = machineEfficiencyDetails[machineID].QE;
                            startRow++;
                        }
                        for (int chartC = 0; chartC < 4; chartC++)
                        {
                            ExcelBarChart barChart = worksheet.Drawings[chartC] as ExcelBarChart;
                            for (int i = 0; i < 2; i++)
                            {
                                ExcelChartSerie aa = barChart.Series[i];
                                int endDate = Convert.ToDateTime(todate).Day + 2;
                                string chartSeries = aa.Series;
                                chartSeries = chartSeries.Remove(chartSeries.Length - 2, 2);
                                aa.Series = chartSeries + endDate.ToString();
                            }
                        }
                        if (isQERequired == false)
                        {

                            worksheet.Drawings.Remove(worksheet.Drawings["Chart 3"]);
                            worksheet.Drawings["Chart 4"].SetPosition(34, 0, 0, 5);
                        }
                    }
                }
                DownloadMultipleFile(Destination, Excel.GetAsByteArray());
                Saved = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                Saved = false;
            }
            return Saved;
        }

        public static bool PlotAggregatedComparisonReportMonthly(string Path, string Destination, string fromdate, string todate, DataTable aggregatedComparisonData, List<EfficiencyEntity> machineEfficiencyDetails, List<string> machineIDList, List<ListItem> efficiencyList)
        {
            string machineID = string.Empty;
            int startRow = 4;
            DataTable dtMachinewiseEffCompDataMonthly = new DataTable();
            bool Saved = false;
            FileInfo newFile = new FileInfo(Path);
            ExcelPackage Excel = new ExcelPackage(newFile, true);
            try
            {
                for (int i = 1; i < machineIDList.Count; i++)
                {
                    ExcelWorksheet worksheet = CopyWorksheet(Excel.Workbook, Excel.Workbook.Worksheets[1].Name, machineIDList[i]);
                }
                for (int j = 0; j < machineIDList.Count; j++)
                {
                    ExcelWorksheet worksheet = Excel.Workbook.Worksheets[j + 1];
                    if (worksheet != null)
                    {
                        startRow = 4;
                        machineID = machineIDList[j];
                        worksheet.Cells["F1"].Value = machineID;
                        worksheet.Cells["I1"].Value = fromdate;
                        worksheet.Cells["K1"].Value = todate;
                        if (j.Equals(0)) worksheet.Name = machineID;
                        ReviseMonthlyExcelChartFormulas(worksheet);
                        dtMachinewiseEffCompDataMonthly = aggregatedComparisonData.AsEnumerable().Where(x => x.Field<string>("MachineID") == machineID).CopyToDataTable();
                        foreach (DataRow row in dtMachinewiseEffCompDataMonthly.Rows)
                        {
                            worksheet.Cells["Q" + startRow].Value = Convert.ToDateTime(row["StartDate"]).ToString("MMM");
                            worksheet.Cells["R" + startRow].Value = row["OEffy"];
                            worksheet.Cells["T" + startRow].Value = row["PEffy"];
                            worksheet.Cells["V" + startRow].Value = row["AEffy"];
                            worksheet.Cells["X" + startRow].Value = row["QEffy"];
                            List<EfficiencyEntity> machineMonthTargetDetails = machineEfficiencyDetails.Where(k => k.MachineID == machineID && k.StartDate == Convert.ToDateTime(row["StartDate"])).ToList();
                            if (machineMonthTargetDetails != null)
                            {
                                if (machineMonthTargetDetails.Count > 0)
                                {
                                    if (!machineMonthTargetDetails[0].OE.Equals(0))
                                        worksheet.Cells["S" + startRow].Value = machineMonthTargetDetails[0].OE;
                                    //worksheet.Cells["T" + startRow].Value = row["PEffy"];
                                    if (!machineMonthTargetDetails[0].PE.Equals(0))
                                        worksheet.Cells["U" + startRow].Value = machineMonthTargetDetails[0].PE;
                                    //worksheet.Cells["V" + startRow].Value = row["AEffy"];
                                    if (!machineMonthTargetDetails[0].AE.Equals(0))
                                        worksheet.Cells["W" + startRow].Value = machineMonthTargetDetails[0].AE;
                                    //worksheet.Cells["X" + startRow].Value = row["QEffy"];
                                    if (!machineMonthTargetDetails[0].QE.Equals(0))
                                        worksheet.Cells["Y" + startRow].Value = machineMonthTargetDetails[0].QE;
                                }
                            }
                            //if (!machineEfficiencyDetails[machineID].OE.Equals(0))
                            //    worksheet.Cells["S" + startRow].Value = machineEfficiencyDetails[machineID].OE;
                            //worksheet.Cells["T" + startRow].Value = row["PEffy"];
                            //if (!machineEfficiencyDetails[machineID].PE.Equals(0))
                            //    worksheet.Cells["U" + startRow].Value = machineEfficiencyDetails[machineID].PE;
                            //worksheet.Cells["V" + startRow].Value = row["AEffy"];
                            //if (!machineEfficiencyDetails[machineID].AE.Equals(0))
                            //    worksheet.Cells["W" + startRow].Value = machineEfficiencyDetails[machineID].AE;
                            //worksheet.Cells["X" + startRow].Value = row["QEffy"];
                            //if (!machineEfficiencyDetails[machineID].QE.Equals(0))
                            //    worksheet.Cells["Y" + startRow].Value = machineEfficiencyDetails[machineID].QE;
                            startRow++;
                        }

                    }

                    int chartWidth = 550, chartHeight = 350;
                    int rowPos = 2, colPos = 1, columnNo = 0;
                    bool isFirstChart = true;
                    if (efficiencyList.Where(x => x.Value == "1").ToList().Count == 1)
                    {
                        chartWidth = 1050;
                        chartHeight = 600;
                    }else if (efficiencyList.Where(x => x.Value == "1").ToList().Count == 2)
                    {
                        chartWidth = 950;
                        chartHeight = 350;
                    }

                    foreach (ListItem items in efficiencyList)
                    {
                        var chart = (dynamic)null;
                        if (items.Text.Equals("AE", StringComparison.OrdinalIgnoreCase))
                        {
                            if (items.Value.Equals("1", StringComparison.OrdinalIgnoreCase))
                            {
                                chart = worksheet.Drawings["Chart 4"] as ExcelBarChart;
                                columnNo++;
                            }
                            else
                            {
                                worksheet.Drawings.Remove(worksheet.Drawings["Chart 4"]);
                                continue;
                            }

                        }
                        else if (items.Text.Equals("PE", StringComparison.OrdinalIgnoreCase))
                        {
                            if (items.Value.Equals("1", StringComparison.OrdinalIgnoreCase))
                            {
                                chart = worksheet.Drawings["Chart 5"] as ExcelBarChart;
                                columnNo++;
                            }
                            else
                            {
                                worksheet.Drawings.Remove(worksheet.Drawings["Chart 5"]);
                                continue;
                            }
                        }
                        else if (items.Text.Equals("QE", StringComparison.OrdinalIgnoreCase))
                        {
                            if (items.Value.Equals("1", StringComparison.OrdinalIgnoreCase))
                            {
                                chart = worksheet.Drawings["Chart 6"] as ExcelBarChart;
                                columnNo++;
                            }
                            else
                            {
                                worksheet.Drawings.Remove(worksheet.Drawings["Chart 6"]);
                                continue;
                            }
                        }
                        else if (items.Text.Equals("OE", StringComparison.OrdinalIgnoreCase))
                        {
                            if (items.Value.Equals("1", StringComparison.OrdinalIgnoreCase))
                            {
                                chart = worksheet.Drawings["Chart 2"] as ExcelBarChart;
                                columnNo++;
                            }
                            else
                            {
                                worksheet.Drawings.Remove(worksheet.Drawings["Chart 2"]);
                                continue;
                            }
                        }
                        if (columnNo % 2 != 0 && !isFirstChart) // for not first chart and first column chart
                        {
                            colPos = 1;
                            rowPos = rowPos + 20;
                        }
                        if (isFirstChart)
                        {
                            isFirstChart = false;
                        }
                        if (columnNo % 2 == 0) // for second column chart
                        {
                            if(efficiencyList.Where(x => x.Value == "1").ToList().Count == 2)
                            {
                                colPos = 1;
                                rowPos = rowPos + 20;
                            }
                            else
                            {
                                colPos = colPos + 8;
                            }
                        }
                        chart.SetSize(chartWidth, chartHeight);
                        chart.SetPosition(rowPos, 10, colPos, 10);
                    }
                    //if (isQERequired == false)
                    //{
                    //    worksheet.Drawings.Remove(worksheet.Drawings["Chart 6"]);
                    //}
                }

                DownloadMultipleFile(Destination, Excel.GetAsByteArray());
                Saved = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                Saved = false;
            }
            return Saved;
        }

        private static void ReviseDailyExcelChartFormulas(ExcelWorksheet worksheet)
        {
            ExcelBarChart OeeChart = worksheet.Drawings[0] as ExcelBarChart;
            ExcelBarChart PeChart = worksheet.Drawings[1] as ExcelBarChart;
            ExcelBarChart AeChart = worksheet.Drawings[2] as ExcelBarChart;
            ExcelBarChart QeChart = worksheet.Drawings[3] as ExcelBarChart;
            OeeChart.Series[0].Series = string.Format(@"'{0}'", worksheet.Name) + "!$B$3:$B$33";
            OeeChart.Series[1].Series = string.Format(@"'{0}'", worksheet.Name) + "!$C$3:$C$33";
            PeChart.Series[0].Series = string.Format(@"'{0}'", worksheet.Name) + "!$F$3:$F$33";
            PeChart.Series[1].Series = string.Format(@"'{0}'", worksheet.Name) + "!$G$3:$G$33";
            AeChart.Series[0].Series = string.Format(@"'{0}'", worksheet.Name) + "!$D$3:$D$33";
            AeChart.Series[1].Series = string.Format(@"'{0}'", worksheet.Name) + "!$E$3:$E$33";
            QeChart.Series[0].Series = string.Format(@"'{0}'", worksheet.Name) + "!$H$3:$H$33";
            QeChart.Series[1].Series = string.Format(@"'{0}'", worksheet.Name) + "!$I$3:$I$33";
        }

        private static void ReviseMonthlyExcelChartFormulas(ExcelWorksheet worksheet)
        {
            ExcelBarChart OeeChart = worksheet.Drawings[0] as ExcelBarChart;
            ExcelBarChart AeChart = worksheet.Drawings[1] as ExcelBarChart;
            ExcelBarChart PeChart = worksheet.Drawings[2] as ExcelBarChart;
            ExcelBarChart QeChart = worksheet.Drawings[3] as ExcelBarChart;
            OeeChart.Series[0].HeaderAddress = new ExcelAddressBase(worksheet.Name, 3, 18, 3, 18);
            OeeChart.Series[0].Series = string.Format(@"'{0}'", worksheet.Name) + "!$R$4:$R$15";
            OeeChart.Series[0].XSeries = string.Format(@"'{0}'", worksheet.Name) + "!$Q$4:$Q$15";
            OeeChart.Series[1].HeaderAddress = new ExcelAddressBase(worksheet.Name, 3, 19, 3, 19);
            OeeChart.Series[1].Series = string.Format(@"'{0}'", worksheet.Name) + "!$S$4:$S$15";
            OeeChart.Series[1].XSeries = string.Format(@"'{0}'", worksheet.Name) + "!$Q$4:$Q$15";
            AeChart.Series[0].HeaderAddress = new ExcelAddressBase(worksheet.Name, 3, 22, 3, 22);
            AeChart.Series[0].Series = string.Format(@"'{0}'", worksheet.Name) + "!$V$4:$V$15";
            AeChart.Series[0].XSeries = string.Format(@"'{0}'", worksheet.Name) + "!$Q$4:$Q$15";
            AeChart.Series[1].HeaderAddress = new ExcelAddressBase(worksheet.Name, 3, 23, 3, 23);
            AeChart.Series[1].Series = string.Format(@"'{0}'", worksheet.Name) + "!$W$4:$W$15";
            AeChart.Series[1].XSeries = string.Format(@"'{0}'", worksheet.Name) + "!$Q$4:$Q$15";
            PeChart.Series[0].HeaderAddress = new ExcelAddressBase(worksheet.Name, 3, 20, 3, 20);
            PeChart.Series[0].Series = string.Format(@"'{0}'", worksheet.Name) + "!$T$4:$T$15";
            PeChart.Series[0].XSeries = string.Format(@"'{0}'", worksheet.Name) + "!$Q$4:$Q$15";
            PeChart.Series[1].HeaderAddress = new ExcelAddressBase(worksheet.Name, 3, 21, 3, 21);
            PeChart.Series[1].Series = string.Format(@"'{0}'", worksheet.Name) + "!$U$4:$U$15";
            PeChart.Series[1].XSeries = string.Format(@"'{0}'", worksheet.Name) + "!$Q$4:$Q$15";
            QeChart.Series[0].HeaderAddress = new ExcelAddressBase(worksheet.Name, 3, 24, 3, 24);
            QeChart.Series[0].Series = string.Format(@"'{0}'", worksheet.Name) + "!$X$4:$X$15";
            QeChart.Series[0].XSeries = string.Format(@"'{0}'", worksheet.Name) + "!$Q$4:$Q$15";
            QeChart.Series[1].HeaderAddress = new ExcelAddressBase(worksheet.Name, 3, 25, 3, 25);
            QeChart.Series[1].Series = string.Format(@"'{0}'", worksheet.Name) + "!$Y$4:$Y$15";
            QeChart.Series[1].XSeries = string.Format(@"'{0}'", worksheet.Name) + "!$Q$4:$Q$15";
        }

        private static ExcelWorksheet CopyWorksheet(ExcelWorkbook workbook, string existingWorksheetName, string newWorksheetName)
        {
            ExcelWorksheet worksheet = workbook.Worksheets.Copy(existingWorksheetName, newWorksheetName);
            return worksheet;
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
                //HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog("GENERATED ERROR : \n" + "Report generation Failed Error: " + ex.ToString());
            }
        }
        #endregion
    }
}
