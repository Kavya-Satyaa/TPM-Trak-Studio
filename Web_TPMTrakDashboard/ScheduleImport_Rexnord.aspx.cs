using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class ScheduleImport_Rexnord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }
        private void BindGrid()
        {
            try
            {
                DataTable dt = DataBaseAccess.GetSheduleData_Rexnord();
                gvData.DataSource = dt;
                gvData.DataBind();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnImport_Click(object sender, EventArgs e)
        {
            string fileName = string.Empty;
            try
            {
                if (fileUploader.HasFile)
                {
                    fileName = fileUploader.FileName;
                    if (Path.GetExtension(fileName) != ".xlsx" && Path.GetExtension(fileName) != ".xls")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "RecordsTextopenModaladded", "openWarningModal('Please choose the valid .xlsx or .xls file');", true);
                        return;
                    }
                    else
                    {
                        if (!Directory.Exists(Server.MapPath("ImportedFiles")))
                        {
                            Directory.CreateDirectory(Server.MapPath("ImportedFiles"));
                        }
                        string Errormsg = "";
                        string savedFileName = Server.MapPath("ImportedFiles//" + fileName);
                        fileUploader.SaveAs(savedFileName);
                        DataTable dt = GetScheduleDataFromExcelFile(savedFileName);
                        if (dt.Rows.Count > 0)
                        {
                            DataBaseAccess.SaveImportDataToTemp_Rexnord(dt);
                            string Result = DataBaseAccess.ImportScheduledata_Rexnord();
                            if (Result != "")
                            {
                                HelperClassGeneric.openSuccessModal(this, "Data imported successfully");
                                BindGrid();
                                Errormsg = "";
                            }
                            else
                            {
                                HelperClassGeneric.openInsertErrorModal(this);
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "RecordsTextopenModaladded", "openWarningModal('Please choose a file to import.');", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private DataTable GetScheduleDataFromExcelFile(string FileName)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var pck = new ExcelPackage())
                {
                    using (var stream = File.OpenRead(FileName))
                    {
                        pck.Load(stream);
                    }
                    var workbook = pck.Workbook;

                    if (workbook != null)
                    {
                        var wsData = workbook.Worksheets[1];
                        //OperationDesc WorkCenter  SerialNo WorkOrderNo Qty ComponentID ComponentDesc Operation   P_Number Action  Confirmation CA1 Unit1 CA2 Unit2 CA3 Unit3 SystemStatus    SetupTime ProcessingTime1 ProcessingTime2
                        dt.Columns.Add("OperationDesc", typeof(string));
                        dt.Columns.Add("WorkCenter", typeof(string));
                        dt.Columns.Add("SerialNo", typeof(string));
                        dt.Columns.Add("WorkOrderNo", typeof(string));
                        dt.Columns.Add("Qty", typeof(string));
                        dt.Columns.Add("ComponentID", typeof(int));
                        dt.Columns.Add("ComponentDesc", typeof(string));
                        dt.Columns.Add("Operation", typeof(int));
                        dt.Columns.Add("ProcessingTime", typeof(double));
                        int startrow = 2;
                        int lastRow = GetLastUsedRow(wsData);
                        for (var rownum = startrow; rownum <= lastRow; rownum++)
                        {
                            string Operationdesc = "";
                            if (wsData.Cells[rownum, 1].Value != null)
                            {
                                Operationdesc = wsData.Cells[rownum, 1].Value.ToString();
                            }
                            string WorkCenter = "";
                            if (wsData.Cells[rownum, 2].Value != null)
                            {
                                WorkCenter = wsData.Cells[rownum, 2].Value.ToString();
                            }
                            string SerialNo = "";
                            if (wsData.Cells[rownum, 3].Value != null)
                            {
                                SerialNo = wsData.Cells[rownum, 3].Value.ToString();
                            }
                            string WorkOrderNo = "";
                            if (wsData.Cells[rownum, 4].Value != null)
                            {
                                WorkOrderNo = wsData.Cells[rownum, 4].Value.ToString();
                            }
                            string Qty = "";
                            if (wsData.Cells[rownum, 5].Value != null)
                            {
                                Qty = wsData.Cells[rownum, 5].Value.ToString();
                            }
                            int CompID = 0;
                            if (wsData.Cells[rownum, 6].Value != null)
                            {
                                CompID = Convert.ToInt32(wsData.Cells[rownum, 6].Value.ToString());
                            }
                            string CompDesc = "";
                            if (wsData.Cells[rownum, 7].Value != null)
                            {
                                CompDesc = wsData.Cells[rownum, 7].Value.ToString();
                            }
                            int Operation = 0;
                            if (wsData.Cells[rownum, 8].Value != null)
                            {
                                Operation = Convert.ToInt32(wsData.Cells[rownum, 8].Value.ToString());
                            }
                            double ProcessingTime = 0;
                            if (wsData.Cells["NP"+rownum].Value != null)
                            {
                                ProcessingTime = Convert.ToDouble(wsData.Cells["NP"+rownum].Value.ToString());
                            }
                            var dtRow = dt.NewRow();
                            dtRow[0] = Operationdesc;
                            dtRow[1] = WorkCenter;
                            dtRow[2] = SerialNo;
                            dtRow[3] = WorkOrderNo;
                            dtRow[4] = Qty;
                            dtRow[5] = CompID;
                            dtRow[6] = CompDesc;
                            dtRow[7] = Operation;
                            dtRow[8] = ProcessingTime;
                            dt.Rows.Add(dtRow);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return dt;
        }
        protected void btnTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                string Filename = string.Empty;
                string templatefile = string.Empty;
                Filename = "ScheduleImportTemplate_Rexnord.xlsx";
                string Source = string.Empty;
                Source = GetReportPath(Filename);
                string Template = string.Empty;
                Template = "ScheduleImportTemplate_Rexnord" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("ScheduleImportTemplate_Rexnord- \n " + Source);
                }
                FileInfo newFile = new FileInfo(Source);
                ExcelPackage Excel = new ExcelPackage(newFile, true);
                DownloadFile(destination, Excel.GetAsByteArray());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        static string appPath = HttpContext.Current.Server.MapPath("~/Reports/TPMTrakReport");
        public static string GetReportPath(string reportName)
        {
            string src;
            src = Path.Combine(appPath, reportName);
            return src;
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
        private static int GetLastUsedRow(ExcelWorksheet sheet)
        {
            var row = sheet.Dimension.End.Row;
            while (row >= 1)
            {
                var range = sheet.Cells[row, 1, row, sheet.Dimension.End.Column];
                if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                {
                    break;
                }
                row--;
            }
            return row;
        }
    }
}