using OfficeOpenXml;
using OfficeOpenXml.Drawing;
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
using Web_TPMTrakDashboard.ShantiIron.Model;

namespace Web_TPMTrakDashboard.ShantiIron
{
    public class ShantiReports
    {
        static string appPath = HttpContext.Current.Server.MapPath("");
        public static string GetReportPath(string reportName)
        {
            string src;
            src = Path.Combine(appPath, "ReportTemplate", reportName);
            return src;
        }
        private static void DownloadFile(string filename, byte[] bytearray)
        {

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Path.GetFileName(filename) + "\"");
            HttpContext.Current.Response.OutputStream.Write(bytearray, 0, bytearray.Length);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();

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
        private static void setBorderThin(ExcelWorksheet sheet, int fromRow, int fromcol, int toRow, int toCol)
        {
            var modelTable3 = sheet.Cells[fromRow, fromcol, toRow, toCol];
            modelTable3.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }
        private static void setBorderDouble(ExcelWorksheet sheet, int fromRow, int fromcol, int toRow, int toCol)
        {
            var modelTable5 = sheet.Cells[fromRow, fromcol, toRow, toCol];
            modelTable5.Style.Border.Top.Style = ExcelBorderStyle.Double;
            modelTable5.Style.Border.Left.Style = ExcelBorderStyle.Double;
            modelTable5.Style.Border.Right.Style = ExcelBorderStyle.Double;
            modelTable5.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
        }
        private static Color getTblHeaderCell()
        {
            return Color.FromArgb(255, 204, 153);
        }
        private static Color subHeaderCellBackColor()
        {
            return Color.FromArgb(204, 255, 204);
        }
        internal static void slnoDashboardDetails(string slno, string componentId,string date,string plantid,string groupid)
        {
            try
            {
                //string logicalStart = VDGDataBaseAccess.GetLogicalDayStart(Util.GetDateTime(date).ToString("yyyy-MM-dd HH:mm:ss"));
                // string logicalEnd = VDGDataBaseAccess.GetLogicalDayEnd(Util.GetDateTime(date).ToString("yyyy-MM-dd HH:mm:ss"));
                string templatefile = string.Empty;
                string Filename = "SerialNumberDetails.xlsx";

                string Source = string.Empty;
                Source = GetReportPath(Filename);
                string Template = string.Empty;
                Template = "SerialNumberDetails_" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Serial Number Dashboard Details Report- \n " + Source);
                }

                List<AlarmDetails> listAlarmDetails = ShantiDataBaseAccess.getAlarmDetails("", "", "", "", slno, componentId);
                List<MeasurementDetails> listLeakTestMcMeasureDetails;
                List<MeasurementDetails> listLaserMcMeasureDetails;
                List<MeasurementDetails> listMeasurementDetails = ShantiDataBaseAccess.getMeasurementDetailsForReport("", "", "", "", slno, componentId, out listLeakTestMcMeasureDetails, out listLaserMcMeasureDetails);
                //List<InspectionDetails> listInspectionDetails = new List<InspectionDetails>();
                //get Inspection Data
                DataTable dtinspectionDetails = ShantiDataBaseAccess.getInspectionDetailsForReport("", "", "", "", slno, componentId, plantid, groupid);
                DataTable dtinspectionDetailsFor90 = ShantiDataBaseAccess.getInspectionDetailsForOpn90("", "", "", "", slno, componentId, plantid, groupid);
                //for (int i = 0; i < dtinspectionDetails.Rows.Count; i++)
                //{
                //    InspectionDetails inspectionDetails = new InspectionDetails();
                //    inspectionDetails.SlNo = dtinspectionDetails.Rows[i]["compslno"].ToString();
                //    inspectionDetails.ComponentID = dtinspectionDetails.Rows[i]["ComponentID"].ToString();
                //    inspectionDetails.InspectionDate = dtinspectionDetails.Rows[i]["InspectionDate"].ToString();
                //    inspectionDetails.Status = dtinspectionDetails.Rows[i]["status"].ToString();
                //    inspectionDetails.Remarks = dtinspectionDetails.Rows[i]["Remarks"].ToString();
                //    inspectionDetails.ChekedBy = dtinspectionDetails.Rows[i]["CheckedBy"].ToString();
                //    inspectionDetails.DimentionalStatus = dtinspectionDetails.Rows[i]["DimentionalStatus"].ToString();
                //    inspectionDetails.Value = dtinspectionDetails.Rows[i][dtinspectionDetails.Columns.Count - 1].ToString();
                //    inspectionDetails.OperationName = dtinspectionDetails.Rows[i]["OperationNo"].ToString();
                //    for ()
                //        listInspectionDetails.Add(inspectionDetails);
                //}
                List<InspectionDetails> listMPIDetails = ShantiDataBaseAccess.getMPIInspectionDetails("", "", "", "", slno, componentId);


                if (listAlarmDetails.Count > 0 || listMeasurementDetails.Count > 0 || dtinspectionDetails.Rows.Count > 0 || listMPIDetails.Count > 0)
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var exelworksheet = Excel.Workbook.Worksheets[1];

                    int rowCount = 1;
                    int headerColumn = 4;
                    exelworksheet.Row(rowCount).Height = 20;
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Merge = true;
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Value = "Serial Number Details";
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Style.Font.Bold = true;
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Style.Font.Size = 14;
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Style.Font.Color.SetColor(Color.FromArgb(32, 55, 100));
                    setBorderDouble(exelworksheet, rowCount, 1, rowCount, headerColumn);
                    rowCount = 3;
                    if (HttpContext.Current.Session["SlnoDashboardData"] != null)
                    {
                        List<SerialNumberDashboardEntity> slnoDetails = HttpContext.Current.Session["SlnoDashboardData"] as List<SerialNumberDashboardEntity>;
                        List<OperationDetails> operationList = slnoDetails.Where(k => k.SerialNumber == slno && k.ComponentID == componentId).Select(k => k.OperatioList).ToList()[0] as List<OperationDetails>;
                        //List<OperationDetails> operationList = operationListTemp[0] as List<OperationDetails>;


                        int rowCountForNextOpn = rowCount;
                        for (int opCount = 0; opCount < operationList.Count; opCount++)
                        {
                            //var exelworksheet = Excel.Workbook.Worksheets[1];
                            //if (opCount>0)
                            //{
                            //    Excel.Workbook.Worksheets.Add("Sheet" + (opCount + 1));
                            //    exelworksheet = Excel.Workbook.Worksheets[opCount + 1];
                            //}
                            //int rowCount = 1;
                            //int headerColumn = 4;
                            //exelworksheet.Row(rowCount).Height = 20;
                            //exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Merge = true;
                            //exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Value = "Serial Number Details";
                            //exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Style.Font.Bold = true;
                            //exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            //exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Style.Font.Size = 14;
                            //exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Style.Font.Color.SetColor(Color.FromArgb(32, 55, 100));
                            //setBorderDouble(exelworksheet, rowCount, 1, rowCount, headerColumn);
                            //rowCount = 3;
                            if (opCount > 0)
                            {
                                rowCount = rowCount + 2;
                            }
                            rowCountForNextOpn = rowCount;
                            string operationName = operationList[opCount].OperationName;
                            string machine = operationList.Where(k => k.OperationName == operationName).Select(k => k.Machine).ToList()[0];
                            string opnDescription = operationList.Where(k => k.OperationName == operationName).Select(k => k.OperationNameWithDescription).ToList()[0];
                            exelworksheet.Cells[rowCount, 1].Value = "Machine: " + machine;
                            exelworksheet.Cells[rowCount, 1].Style.Font.Bold = true;
                            exelworksheet.Cells[rowCount, 1, rowCount, 2].Merge = true;
                            exelworksheet.Cells[rowCount, 3].Value = "Operation: " + opnDescription;
                            exelworksheet.Cells[rowCount, 3].Style.Font.Bold = true;
                            exelworksheet.Cells[rowCount, 3, rowCount, 4].Merge = true;
                            exelworksheet.Row(rowCount).Style.Font.Size = 13;
                            exelworksheet.Row(rowCount).Style.Font.Color.SetColor(Color.FromArgb(35, 24, 142));
                            setBorderThin(exelworksheet, rowCount, 1, rowCount, 4);
                           
                            //Bind Alarm Details
                            if (listAlarmDetails.Count > 0)
                            {
                                List<AlarmDetails> alarmDetailsForOpn = listAlarmDetails.Where(k => k.MachineID == machine).ToList();
                                if (alarmDetailsForOpn.Count > 0)
                                {
                                    rowCount++;
                                    int startRow = rowCount;
                                    exelworksheet.Cells[rowCount, 1].Value = "Alarm";
                                    exelworksheet.Cells[rowCount, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    exelworksheet.Cells[rowCount, 1].Style.Fill.BackgroundColor.SetColor(subHeaderCellBackColor());
                                    exelworksheet.Cells[rowCount, 1].Style.Font.Bold = true;
                                    exelworksheet.Cells[rowCount, 1].Style.Font.Size = 13;
                                    exelworksheet.Cells[rowCount, 1, rowCount, 4].Merge = true;
                                    rowCount++;
                                    exelworksheet.Cells[rowCount, 1].Value = "Machine ID";
                                    exelworksheet.Cells[rowCount, 2].Value = "Alarm No";
                                    exelworksheet.Cells[rowCount, 3].Value = "Description";
                                    exelworksheet.Cells[rowCount, 4].Value = "Alarm Start Time";
                                    exelworksheet.Row(rowCount).Style.Font.Bold = true;
                                    exelworksheet.Row(rowCount).Style.Font.Size = 12;
                                    exelworksheet.Cells[rowCount, 1, rowCount, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    exelworksheet.Cells[rowCount, 1, rowCount, 4].Style.Fill.BackgroundColor.SetColor(getTblHeaderCell());
                                    for (int i = 0; i < alarmDetailsForOpn.Count; i++)
                                    {
                                        rowCount++;
                                        exelworksheet.Cells[rowCount, 1].Value = alarmDetailsForOpn[i].MachineID;
                                        exelworksheet.Cells[rowCount, 2].Value = alarmDetailsForOpn[i].AlarmNo;
                                        exelworksheet.Cells[rowCount, 3].Value = alarmDetailsForOpn[i].Desciption;
                                        exelworksheet.Cells[rowCount, 4].Value = alarmDetailsForOpn[i].AlarmStartTime;
                                        exelworksheet.Row(rowCount).Style.Font.Size = 12;
                                    }
                                    setBorderThin(exelworksheet, startRow, 1, rowCount, 4);
                                }
                                else
                                {
                                    rowCount--;
                                }
                            }
                           

                            //Bind Measurent Details - Leak Test Machine
                            bool isMachineLeakOrLaser = false;
                            if (listLeakTestMcMeasureDetails.Count > 0)
                            {
                                if (string.Equals(machine, "Leak Test Machine", StringComparison.OrdinalIgnoreCase))
                                {
                                    isMachineLeakOrLaser = true;
                                    List<MeasurementDetails> measurementDataForOpn = listLeakTestMcMeasureDetails.Where(k => k.OperationName == operationName).ToList();
                                    if (measurementDataForOpn.Count > 0)
                                    {
                                        rowCount = rowCount + 2;
                                        int startRow = rowCount;
                                        exelworksheet.Cells[rowCount, 1].Value = "Measurement";
                                        exelworksheet.Cells[rowCount, 1].Style.Font.Bold = true;
                                        exelworksheet.Cells[rowCount, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        exelworksheet.Cells[rowCount, 1].Style.Fill.BackgroundColor.SetColor(subHeaderCellBackColor());
                                        exelworksheet.Cells[rowCount, 1].Style.Font.Size = 13;
                                        exelworksheet.Cells[rowCount, 1, rowCount, 2].Merge = true;
                                        rowCount++;
                                        exelworksheet.Cells[rowCount, 1].Value = "Result";
                                        exelworksheet.Cells[rowCount, 2].Value = "Remark";
                                        exelworksheet.Cells[rowCount, 1, rowCount, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        exelworksheet.Cells[rowCount, 1, rowCount, 2].Style.Fill.BackgroundColor.SetColor(getTblHeaderCell());
                                        exelworksheet.Row(rowCount).Style.Font.Bold = true;
                                        exelworksheet.Row(rowCount).Style.Font.Size = 12;
                                        for (int i = 0; i < measurementDataForOpn.Count; i++)
                                        {
                                            rowCount++;
                                            exelworksheet.Cells[rowCount, 1].Value = measurementDataForOpn[i].Result;
                                            exelworksheet.Cells[rowCount, 2].Value = measurementDataForOpn[i].LeakTestRemarks;
                                            exelworksheet.Row(rowCount).Style.Font.Size = 12;
                                        }
                                        setBorderThin(exelworksheet, startRow, 1, rowCount, 2);
                                    }
                                }
                            }

                            //Bind Measurent Details - Laser Machine
                            if (listLaserMcMeasureDetails.Count > 0)
                            {
                                if (string.Equals(machine, "Laser Marking Machine Phantom", StringComparison.OrdinalIgnoreCase) || string.Equals(machine, "Laser Marking Machine Compact x", StringComparison.OrdinalIgnoreCase))
                                {
                                    isMachineLeakOrLaser = true;
                                    List<MeasurementDetails> measurementDataForOpn = listLaserMcMeasureDetails.Where(k => k.OperationName == operationName).ToList();
                                    if (measurementDataForOpn.Count > 0)
                                    {
                                        rowCount = rowCount + 2;
                                        int startRow = rowCount;
                                        exelworksheet.Cells[rowCount, 1].Value = "Measurement";
                                        exelworksheet.Cells[rowCount, 1].Style.Font.Bold = true;
                                        exelworksheet.Cells[rowCount, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        exelworksheet.Cells[rowCount, 1].Style.Fill.BackgroundColor.SetColor(subHeaderCellBackColor());
                                        exelworksheet.Cells[rowCount, 1].Style.Font.Size = 13;
                                        exelworksheet.Cells[rowCount, 1, rowCount, 4].Merge = true;
                                        rowCount++;
                                        exelworksheet.Cells[rowCount, 1].Value = "Scanned Data";
                                        exelworksheet.Cells[rowCount, 2].Value = "Marking Data";
                                        exelworksheet.Cells[rowCount, 3].Value = "Marking Status";
                                        exelworksheet.Cells[rowCount, 4].Value = "Status";
                                        exelworksheet.Row(rowCount).Style.Font.Bold = true;
                                        exelworksheet.Row(rowCount).Style.Font.Size = 12;
                                        exelworksheet.Cells[rowCount, 1, rowCount, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        exelworksheet.Cells[rowCount, 1, rowCount, 4].Style.Fill.BackgroundColor.SetColor(getTblHeaderCell());
                                        for (int i = 0; i < measurementDataForOpn.Count; i++)
                                        {
                                            rowCount++;
                                            exelworksheet.Cells[rowCount, 1].Value = measurementDataForOpn[i].ScannedData;
                                            exelworksheet.Cells[rowCount, 2].Value = measurementDataForOpn[i].MarkingData;
                                            exelworksheet.Cells[rowCount, 3].Value = measurementDataForOpn[i].MarkingStatus;
                                            exelworksheet.Cells[rowCount, 4].Value = measurementDataForOpn[i].Status;
                                            exelworksheet.Row(rowCount).Style.Font.Size = 12;
                                        }
                                        setBorderThin(exelworksheet, startRow, 1, rowCount, 4);
                                    }
                                }
                            }
                            //Bind Measurent Details
                            if (!isMachineLeakOrLaser)
                            {
                                if (listMeasurementDetails.Count > 0)
                                {
                                    List<MeasurementDetails> measurementDataForOpn = listMeasurementDetails.Where(k => k.OperationName == operationName).ToList();
                                    if (measurementDataForOpn.Count > 0)
                                    {
                                        rowCount = rowCount + 2;
                                        int startRow = rowCount;
                                        exelworksheet.Cells[rowCount, 1].Value = "Measurement";
                                        exelworksheet.Cells[rowCount, 1].Style.Font.Bold = true;
                                        exelworksheet.Cells[rowCount, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        exelworksheet.Cells[rowCount, 1].Style.Fill.BackgroundColor.SetColor(subHeaderCellBackColor());
                                        exelworksheet.Cells[rowCount, 1].Style.Font.Size = 13;
                                        exelworksheet.Cells[rowCount, 1, rowCount, 9].Merge = true;
                                        rowCount++;
                                        exelworksheet.Cells[rowCount, 1].Value = "Component ID";
                                        exelworksheet.Cells[rowCount, 2].Value = "Characteristic ID";
                                        exelworksheet.Cells[rowCount, 3].Value = "Characteristic Code";
                                        exelworksheet.Cells[rowCount, 4].Value = "LSL";
                                        exelworksheet.Cells[rowCount, 5].Value = "USL";
                                        exelworksheet.Cells[rowCount, 6].Value = "Unit";
                                        exelworksheet.Cells[rowCount, 7].Value = "Value";
                                        exelworksheet.Cells[rowCount, 8].Value = "Time Stamp";
                                        exelworksheet.Cells[rowCount, 9].Value = "Specification Mean";
                                        int measurLastCol = 9;
                                        if (string.Equals(machine, "QA Gate", StringComparison.OrdinalIgnoreCase))
                                        {
                                            exelworksheet.Cells[rowCount, 10].Value = "Remarks";
                                            measurLastCol = 10;
                                        }
                                        exelworksheet.Row(rowCount).Style.Font.Bold = true;
                                        exelworksheet.Row(rowCount).Style.Font.Size = 12;
                                        exelworksheet.Cells[rowCount, 1, rowCount, measurLastCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        exelworksheet.Cells[rowCount, 1, rowCount, measurLastCol].Style.Fill.BackgroundColor.SetColor(getTblHeaderCell());
                                        for (int i = 0; i < measurementDataForOpn.Count; i++)
                                        {
                                            rowCount++;
                                            exelworksheet.Cells[rowCount, 1].Value = measurementDataForOpn[i].ComponentID;
                                            exelworksheet.Cells[rowCount, 2].Value = measurementDataForOpn[i].CharacteristicID;
                                            exelworksheet.Cells[rowCount, 3].Value = measurementDataForOpn[i].CharecteristicCode;
                                            exelworksheet.Cells[rowCount, 4].Value = measurementDataForOpn[i].LSL;
                                            exelworksheet.Cells[rowCount, 5].Value = measurementDataForOpn[i].USL;
                                            exelworksheet.Cells[rowCount, 6].Value = measurementDataForOpn[i].Unit;
                                            exelworksheet.Cells[rowCount, 7].Value = measurementDataForOpn[i].Value;
                                            exelworksheet.Cells[rowCount, 8].Value = measurementDataForOpn[i].TimeStamp;
                                            exelworksheet.Cells[rowCount, 9].Value = measurementDataForOpn[i].SpecificationMean;
                                            if (string.Equals(machine, "QA Gate", StringComparison.OrdinalIgnoreCase))
                                            {
                                                exelworksheet.Cells[rowCount, 10].Value = measurementDataForOpn[i].Remarks;
                                            }
                                            exelworksheet.Row(rowCount).Style.Font.Size = 12;
                                        }
                                        setBorderThin(exelworksheet, startRow, 1, rowCount, measurLastCol);
                                    }
                                }
                            }

                            //Bind Inspection Details
                            if (operationName == "90")
                            {
                                if (dtinspectionDetailsFor90.Rows.Count > 0)
                                {
                                    rowCount = rowCount + 2;
                                    int startRow = rowCount;
                                    exelworksheet.Cells[rowCount, 1].Value = "Inpsection";
                                    exelworksheet.Cells[rowCount, 1].Style.Font.Bold = true;
                                    exelworksheet.Cells[rowCount, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    exelworksheet.Cells[rowCount, 1].Style.Fill.BackgroundColor.SetColor(subHeaderCellBackColor());
                                    exelworksheet.Cells[rowCount, 1].Style.Font.Size = 13;
                                    exelworksheet.Cells[rowCount, 1, rowCount, 3].Merge = true;
                                    rowCount++;
                                    exelworksheet.Cells[rowCount, 1].Value = "Serial Number";
                                    exelworksheet.Cells[rowCount, 2].Value = "Component ID";
                                    exelworksheet.Cells[rowCount, 3].Value = "Status";
                                    exelworksheet.Cells[rowCount, 1, rowCount, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    exelworksheet.Cells[rowCount, 1, rowCount, 3].Style.Fill.BackgroundColor.SetColor(getTblHeaderCell());
                                    exelworksheet.Row(rowCount).Style.Font.Bold = true;
                                    exelworksheet.Row(rowCount).Style.Font.Size = 12;
                                    foreach (DataRow rows in dtinspectionDetailsFor90.Rows)
                                    {
                                        rowCount++;
                                        exelworksheet.Cells[rowCount, 1].Value = rows["compslno"];
                                        exelworksheet.Cells[rowCount, 2].Value = rows["ComponentID"];
                                       // exelworksheet.Cells[rowCount, 3].Value = rows["status"].ToString();
                                        if (rows["status"].ToString() == "Rejected")
                                        {
                                            // exelworksheet.Cells[rowCount, 3].Value = rows["DimentionalStatus"];
                                            System.Drawing.Image fiuncheck = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath("~/Images/Wrong.jpg"));
                                            ExcelPicture pic4 = exelworksheet.Drawings.AddPicture("DSWrong" + opCount, fiuncheck);
                                            pic4.SetPosition(rowCount - 1, 2, 2, 2);
                                            pic4.SetSize(17, 17);
                                        }
                                        else
                                        {
                                            // exelworksheet.Cells[rowCount, 3].Value = rows["DimentionalStatus"];
                                            System.Drawing.Image fiuncheck = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath("~/Images/Right.jpg"));
                                            ExcelPicture pic4 = exelworksheet.Drawings.AddPicture("DSRight" + opCount, fiuncheck);
                                            pic4.SetPosition(rowCount - 1, 2, 2, 2);
                                            pic4.SetSize(17, 17);
                                        }
                                        exelworksheet.Row(rowCount).Style.Font.Size = 12;
                                    }
                                    setBorderThin(exelworksheet, startRow, 1, rowCount, 3);
                                }
                            }
                            else if (dtinspectionDetails.Rows.Count > 0)
                            {
                                var inspectionDataForOpn = dtinspectionDetails.AsEnumerable().Where(k => k.Field<string>("operationid") == operationName);
                                if (inspectionDataForOpn.Count() > 0)
                                {
                                    rowCount = rowCount + 2;
                                    int startRow = rowCount;
                                    exelworksheet.Cells[rowCount, 1].Value = "Inpsection";
                                    exelworksheet.Cells[rowCount, 1].Style.Font.Bold = true;
                                    exelworksheet.Cells[rowCount, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    exelworksheet.Cells[rowCount, 1].Style.Fill.BackgroundColor.SetColor(subHeaderCellBackColor());
                                    exelworksheet.Cells[rowCount, 1].Style.Font.Size = 13;
                                    exelworksheet.Cells[rowCount, 1, rowCount, 4].Merge = true;
                                    rowCount++;
                                    exelworksheet.Cells[rowCount, 1].Value = "Serial Number";
                                    exelworksheet.Cells[rowCount, 2].Value = "Component ID";
                                    exelworksheet.Cells[rowCount, 3].Value = "Dimensional OK OPN " + operationName;
                                    exelworksheet.Cells[rowCount, 4].Value = "Visual OK OPN " + operationName;
                                    exelworksheet.Cells[rowCount, 1, rowCount, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    exelworksheet.Cells[rowCount, 1, rowCount, 4].Style.Fill.BackgroundColor.SetColor(getTblHeaderCell());
                                    exelworksheet.Row(rowCount).Style.Font.Bold = true;
                                    exelworksheet.Row(rowCount).Style.Font.Size = 12;
                                    foreach (DataRow rows in inspectionDataForOpn)
                                    {
                                        rowCount++;
                                        exelworksheet.Cells[rowCount, 1].Value = rows["compslno"];
                                        exelworksheet.Cells[rowCount, 2].Value = rows["ComponentID"];
                                        if (rows["DimentionalStatus"].ToString() == "Green")
                                        {
                                            // exelworksheet.Cells[rowCount, 3].Value = rows["DimentionalStatus"];
                                            System.Drawing.Image fiuncheck = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath("~/Images/Right.jpg"));
                                            ExcelPicture pic4 = exelworksheet.Drawings.AddPicture("DSRight" + opCount, fiuncheck);
                                            pic4.SetPosition(rowCount - 1, 2, 2, 2);
                                            pic4.SetSize(17, 17);
                                        }
                                        else
                                        {
                                            // exelworksheet.Cells[rowCount, 3].Value = rows["DimentionalStatus"];
                                            System.Drawing.Image fiuncheck = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath("~/Images/Wrong.jpg"));
                                            ExcelPicture pic4 = exelworksheet.Drawings.AddPicture("DSWrong" + opCount, fiuncheck);
                                            pic4.SetPosition(rowCount - 1, 2, 2, 2);
                                            pic4.SetSize(17, 17);
                                        }
                                        if (rows[opnDescription].ToString() == "Green")
                                        {
                                            // exelworksheet.Cells[rowCount, 4].Value = rows[opnDescription].ToString();
                                            System.Drawing.Image fiuncheck = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath("~/Images/Right.jpg"));
                                            ExcelPicture pic4 = exelworksheet.Drawings.AddPicture("VRight" + opCount, fiuncheck);
                                            pic4.SetPosition(rowCount - 1, 2, 3, 2);
                                            pic4.SetSize(17, 17);
                                        }
                                        else
                                        {
                                            //exelworksheet.Cells[rowCount, 4].Value = rows[opnDescription].ToString();
                                            System.Drawing.Image fiuncheck = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath("~/Images/Wrong.jpg"));
                                            ExcelPicture pic4 = exelworksheet.Drawings.AddPicture("VWrong" + opCount, fiuncheck);
                                            pic4.SetPosition(rowCount - 1, 2, 3, 2);
                                            pic4.SetSize(17, 17);
                                        }
                                        exelworksheet.Row(rowCount).Style.Font.Size = 12;
                                        break;
                                    }
                                    setBorderThin(exelworksheet, startRow, 1, rowCount, 4);
                                }
                            }
                           

                            //Bind MPI Details for 'crack detection'
                            string opnDesc = opnDescription.ToLower();
                            if (opnDesc.Contains("crack detection"))
                            {
                                if (listMPIDetails.Count > 0)
                                {
                                    List<InspectionDetails> mpiDtailsForOpn = listMPIDetails.Where(k => k.OperationName == operationName).ToList();
                                    if (listMPIDetails.Count > 0)
                                    {
                                        rowCount = rowCount + 2;
                                        int startRow = rowCount;
                                        exelworksheet.Cells[rowCount, 1].Value = "MPI";
                                        exelworksheet.Cells[rowCount, 1].Style.Font.Bold = true;
                                        exelworksheet.Cells[rowCount, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        exelworksheet.Cells[rowCount, 1].Style.Fill.BackgroundColor.SetColor(subHeaderCellBackColor());
                                        exelworksheet.Cells[rowCount, 1].Style.Font.Size = 13;
                                        exelworksheet.Cells[rowCount, 1, rowCount, 6].Merge = true;
                                        rowCount++;
                                        exelworksheet.Cells[rowCount, 1].Value = "Manual Insp/Result";
                                        exelworksheet.Cells[rowCount, 2].Value = "Camera Insp/Result";
                                        exelworksheet.Cells[rowCount, 3].Value = "Camera Pic's Link";
                                        exelworksheet.Cells[rowCount, 4].Value = "De-Mag Level";
                                        exelworksheet.Cells[rowCount, 5].Value = "Remarks";
                                        exelworksheet.Cells[rowCount, 6].Value = "Visual Insp/Result";
                                        exelworksheet.Row(rowCount).Style.Font.Bold = true;
                                        exelworksheet.Row(rowCount).Style.Font.Size = 12;
                                        exelworksheet.Cells[rowCount, 1, rowCount, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        exelworksheet.Cells[rowCount, 1, rowCount, 6].Style.Fill.BackgroundColor.SetColor(getTblHeaderCell());
                                        for (int i = 0; i < listMPIDetails.Count; i++)
                                        {
                                            rowCount++;
                                            exelworksheet.Cells[rowCount, 1].Value = listMPIDetails[i].ManualInsResult;
                                            exelworksheet.Cells[rowCount, 2].Value = listMPIDetails[i].CameraInsResult;
                                            exelworksheet.Cells[rowCount, 3].Value = listMPIDetails[i].CameraPicLink;
                                            exelworksheet.Cells[rowCount, 4].Value = listMPIDetails[i].DeMagLevel;
                                            exelworksheet.Cells[rowCount, 5].Value = listMPIDetails[i].MPIRemark;
                                            exelworksheet.Cells[rowCount, 6].Value = listMPIDetails[i].VisualInsResult;
                                            exelworksheet.Row(rowCount).Style.Font.Size = 12;
                                        }
                                        setBorderThin(exelworksheet, startRow, 1, rowCount, 6);
                                    }
                                }
                            }

                            if(rowCount<= rowCountForNextOpn)
                            {
                                rowCount++;
                            }
                        }
                        for (int i = 1; i <= 10; i++)
                        {
                            exelworksheet.Column(i).AutoFit();
                        }
                        DownloadFile(destination, Excel.GetAsByteArray());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        internal static void coolantTopUpReport(string plant, string cell,string machine, string fromDate, string toDate)
        {
            try
            {
                string fromDate1 = Web_TPMTrakDashboard.Models.VDGDataBaseAccess.GetLogicalDayStart(fromDate);
                string toDate1 = Web_TPMTrakDashboard.Models.VDGDataBaseAccess.GetLogicalDayEnd(toDate);
                //string logicalStart = VDGDataBaseAccess.GetLogicalDayStart(Util.GetDateTime(date).ToString("yyyy-MM-dd HH:mm:ss"));
                // string logicalEnd = VDGDataBaseAccess.GetLogicalDayEnd(Util.GetDateTime(date).ToString("yyyy-MM-dd HH:mm:ss"));
                string templatefile = string.Empty;
                string Filename = "CoolantTopUpReport.xlsx";

                string Source = string.Empty;
                Source = GetReportPath(Filename);
                string Template = string.Empty;
                Template = "CoolantTopUpReport_" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Coolant Top-Up Report- \n " + Source);
                }

                List<CoolantTopUpData> listCoolantData = ShantiDataBaseAccess.getCoolantTopUpReportData(plant,cell, machine, fromDate1, toDate1);
                List<CoolantTopUpData> listRawData = ShantiDataBaseAccess.getCoolantTopUpData(plant, cell, machine, fromDate1, toDate1);
                if (listCoolantData.Count > 0)
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var exelworksheet = Excel.Workbook.Worksheets[1];

                    int rowCount = 1;
                    int headerColumn = 10;
                    exelworksheet.Row(rowCount).Height = 20;
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Merge = true;
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Value = "Coolant Top-Up Report";
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Style.Font.Bold = true;
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Style.Font.Size = 14;
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Style.Font.Color.SetColor(Color.FromArgb(32, 55, 100));
                    setBorderDouble(exelworksheet, rowCount, 1, rowCount, headerColumn);
                    rowCount = 3;
                    exelworksheet.Cells[rowCount, 1].Value = "Plant ID";
                    exelworksheet.Cells[rowCount, 1].Style.Font.Bold = true;
                    exelworksheet.Cells[rowCount, 2].Value = plant == "" ? "All" : plant;
                    exelworksheet.Cells[rowCount, 3].Value = "Cell ID";
                    exelworksheet.Cells[rowCount, 3].Style.Font.Bold = true;
                    exelworksheet.Cells[rowCount, 4].Value = cell == "" ? "All" : cell;
                    exelworksheet.Cells[rowCount, 5].Value = "Machine ID";
                    exelworksheet.Cells[rowCount, 5].Style.Font.Bold = true;
                    exelworksheet.Cells[rowCount, 6].Value = machine == "" ? "All" : machine;
                    exelworksheet.Cells[rowCount, 7].Value = "From Date";
                    exelworksheet.Cells[rowCount, 7].Style.Font.Bold = true;
                    exelworksheet.Cells[rowCount, 8].Value = ShantiDataBaseAccess.GetDateTime(fromDate).ToString("dd-MM-yyyy");
                    exelworksheet.Cells[rowCount, 9].Value = "To Date";
                    exelworksheet.Cells[rowCount, 9].Style.Font.Bold = true;
                    exelworksheet.Cells[rowCount, 10].Value = ShantiDataBaseAccess.GetDateTime(toDate).ToString("dd-MM-yyyy");


                    rowCount = 5;
                    exelworksheet.Cells[rowCount, 1].Value = "Plant ID";
                    exelworksheet.Cells[rowCount, 2].Value = "Cell ID";
                    exelworksheet.Cells[rowCount, 3].Value = "Machine ID";
                    exelworksheet.Cells[rowCount, 4].Value = "Top-Up (Liter)";
                    exelworksheet.Cells[rowCount, 1, rowCount, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    exelworksheet.Cells[rowCount, 1, rowCount, 4].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 204, 153));
                    exelworksheet.Row(rowCount).Style.Font.Bold = true;
                    exelworksheet.Row(rowCount).Style.Font.Size = 13;
                    rowCount++;
                    var distPlant = listCoolantData.Select(k => new { k.PlantID }).Distinct().ToList();
                    for (int plantCount = 0; plantCount < distPlant.Count; plantCount++)
                    {
                        int plantStartRow = rowCount;
                        var distCell = listCoolantData.Where(k => k.PlantID == distPlant[plantCount].PlantID).Select(k => k.CellID).Distinct().ToList();
                        for (int cellCount = 0; cellCount < distCell.Count; cellCount++)
                        {
                            //merge cell id
                            var distMachine = listCoolantData.Where(k => k.PlantID == distPlant[plantCount].PlantID && k.CellID == distCell[cellCount]).ToList();
                            exelworksheet.Cells[rowCount, 2].Value = distCell[cellCount];
                            exelworksheet.Cells[rowCount, 2, rowCount + distMachine.Count - 1, 2].Merge = true;

                            for (int machineCount = 0; machineCount < distMachine.Count; machineCount++)
                            {
                                exelworksheet.Cells[rowCount, 3].Value = distMachine[machineCount].MachineID;
                                double machineVal = 0;
                                if (distMachine[machineCount].TopUpValue != "" && distMachine[machineCount].TopUpValue != null)
                                {
                                    machineVal = Convert.ToDouble(distMachine[machineCount].TopUpValue);
                                }
                                exelworksheet.Cells[rowCount, 4].Value = machineVal;
                                rowCount++;
                            }

                            //add total cell value
                            exelworksheet.Cells[rowCount, 2].Value = "Total";
                            exelworksheet.Cells[rowCount, 2].Style.Font.Bold = true;
                            exelworksheet.Cells[rowCount, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            exelworksheet.Cells[rowCount, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            exelworksheet.Cells[rowCount, 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(204, 255, 204));
                            exelworksheet.Cells[rowCount, 2, rowCount, 3].Merge = true;
                            string cellValue = listCoolantData.Where(k => k.PlantID == distPlant[plantCount].PlantID && k.CellID == distCell[cellCount]).Select(k => k.TotalCell).ToList()[0];
                            double cellVal=0;
                           if(cellValue != "" && cellValue != null)
                            {
                                cellVal = Convert.ToDouble(cellValue);
                            }
                            exelworksheet.Cells[rowCount, 4].Value = cellVal;
                            exelworksheet.Cells[rowCount, 4].Style.Font.Bold = true;
                            rowCount++;
                        }
                        //plant id merge cell
                        exelworksheet.Cells[plantStartRow, 1].Value = distPlant[plantCount].PlantID;
                        exelworksheet.Cells[plantStartRow, 1, rowCount - 1, 1].Merge = true;
                    }
                    //add plant total value
                    exelworksheet.Row(rowCount).Style.Font.Bold = true;
                    exelworksheet.Cells[rowCount, 1].Value = "Total";
                    exelworksheet.Cells[rowCount, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    exelworksheet.Cells[rowCount, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    exelworksheet.Cells[rowCount, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(204,255,204));
                    exelworksheet.Cells[rowCount, 1, rowCount, 3].Merge = true;
                    double plantVal = 0;
                    if (listCoolantData[0].TotalPlant != "" && listCoolantData[0].TotalPlant != null)
                    {
                        plantVal = Convert.ToDouble(listCoolantData[0].TotalPlant);
                    }
                    exelworksheet.Cells[rowCount,4].Value = plantVal;
                    setBorderThin(exelworksheet, 5, 1, rowCount, 4);
                    for (int i = 1; i <= 10; i++)
                    {
                        exelworksheet.Column(i).AutoFit();
                    }
                    exelworksheet.Column(1).AutoFit();

                    //RawData
                    exelworksheet = Excel.Workbook.Worksheets[2];
                    rowCount = 1;
                    headerColumn = 10;
                    exelworksheet.Row(rowCount).Height = 20;
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Merge = true;
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Value = "Coolant Top-Up Report";
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Style.Font.Bold = true;
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Style.Font.Size = 14;
                    exelworksheet.Cells[rowCount, rowCount, rowCount, headerColumn].Style.Font.Color.SetColor(Color.FromArgb(32, 55, 100));
                    setBorderDouble(exelworksheet, rowCount, 1, rowCount, headerColumn);
                    rowCount = 3;
                    exelworksheet.Cells[rowCount, 1].Value = "Plant ID";
                    exelworksheet.Cells[rowCount, 1].Style.Font.Bold = true;
                    exelworksheet.Cells[rowCount, 2].Value = plant == "" ? "All" : plant;
                    exelworksheet.Cells[rowCount, 3].Value = "Cell ID";
                    exelworksheet.Cells[rowCount, 3].Style.Font.Bold = true;
                    exelworksheet.Cells[rowCount, 4].Value = cell == "" ? "All" : cell;
                    exelworksheet.Cells[rowCount, 5].Value = "Machine ID";
                    exelworksheet.Cells[rowCount, 5].Style.Font.Bold = true;
                    exelworksheet.Cells[rowCount, 6].Value = machine == "" ? "All" : machine;
                    exelworksheet.Cells[rowCount, 7].Value = "From Date";
                    exelworksheet.Cells[rowCount, 7].Style.Font.Bold = true;
                    exelworksheet.Cells[rowCount, 8].Value =  ShantiDataBaseAccess.GetDateTime(fromDate).ToString("dd-MM-yyyy");
                    exelworksheet.Cells[rowCount, 9].Value = "To Date";
                    exelworksheet.Cells[rowCount, 9].Style.Font.Bold = true;
                    exelworksheet.Cells[rowCount, 10].Value = ShantiDataBaseAccess.GetDateTime(toDate).ToString("dd-MM-yyyy");
                    rowCount = 5;
                    setBorderThin(exelworksheet, rowCount, 1, rowCount + listRawData.Count, 6);
                    exelworksheet.Cells[rowCount, 1].Value = "Plant ID";
                    exelworksheet.Cells[rowCount, 2].Value = "Cell ID";
                    exelworksheet.Cells[rowCount, 3].Value = "Machine ID";
                    exelworksheet.Cells[rowCount, 4].Value = "Top-Up (Liter)";
                    exelworksheet.Cells[rowCount, 5].Value = "Top-Up Datetime";
                    exelworksheet.Cells[rowCount, 6].Value = "Remarks";
                    exelworksheet.Cells[rowCount, 1, rowCount, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    exelworksheet.Cells[rowCount, 1, rowCount, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 204, 153));
                    exelworksheet.Row(rowCount).Style.Font.Bold = true;
                    exelworksheet.Row(rowCount).Style.Font.Size = 13;
                    rowCount++;
                    
                    for (int i = 0; i < listRawData.Count; i++)
                    {
                        exelworksheet.Cells[rowCount, 1].Value = listRawData[i].PlantID;
                        exelworksheet.Cells[rowCount, 2].Value = listRawData[i].CellID;
                        exelworksheet.Cells[rowCount, 3].Value = listRawData[i].MachineID;
                        exelworksheet.Cells[rowCount, 4].Value = listRawData[i].TopUpValue;
                        exelworksheet.Cells[rowCount, 5].Value = listRawData[i].TopUpDatetime;
                        exelworksheet.Cells[rowCount, 6].Value = listRawData[i].Remarks;
                        rowCount++;
                    }
                    
                    for (int i = 1; i <= 10; i++)
                    {
                        exelworksheet.Column(i).AutoFit();
                    }
                    DownloadFile(destination, Excel.GetAsByteArray());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}