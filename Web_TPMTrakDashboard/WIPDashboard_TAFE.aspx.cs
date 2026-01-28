using OfficeOpenXml;
using OfficeOpenXml.Style;
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
    public partial class WIPDashboard_TAFE : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindType();
                BindPartNo();
                BindGrids();
            }
        }
        private void BindPartNo()
        {
            try
            {
                List<string> list =DataBaseAccess.GetWIPPartNo();
                list.Insert(0, "ALL");
                ddlPartNo.DataSource = list;
                ddlPartNo.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindType()
        {
            try
            {
                List<string> list =DataBaseAccess.GetWIPMachineType();
                ddlType.DataSource = list;
                ddlType.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindGrids()
        {
            try
            {
                DateTime Fromdate = Util.GetDateTime(txtFromDate.Text);
                DateTime Todate = Util.GetDateTime(txtToDate.Text);
                if (ddlWIPProcess.SelectedValue.ToString().Equals("Completed PDI Process", StringComparison.OrdinalIgnoreCase))
                {
                    divComplete.Visible = true;
                    divWaiting.Visible = false;
                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();
                    DataTable dt3 = new DataTable();
                    DataTable dt = DataBaseAccess.GetWIPCompleteDetals(out dt1,out dt2,out dt3, Fromdate, Todate, ddlType.SelectedValue.ToString(), ddlPartNo.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : ddlPartNo.SelectedValue.ToString(), ddlWIPProcess.SelectedValue.ToString());
                    lvCompleteDetails.DataSource = dt3;
                    lvCompleteDetails.DataBind();
                    if (!(dt.Rows.Count > 0))
                    {
                        divmachine.Style["height"] = "10vh";
                        if (!(dt1.Rows.Count > 0))
                        {
                            divtester.Style["height"] = "10vh";
                            if (!(dt2.Rows.Count > 0))
                            {
                                divPDI.Style["height"] = "10vh";
                            }
                        }
                        else
                        {
                            divtester.Style["height"] = "40vh";
                            divPDI.Style["height"] = "40vh";
                        }
                    }
                    lvmachine.DataSource = dt;
                    lvmachine.DataBind();
                    lvTester.DataSource = dt1;
                    lvTester.DataBind();
                    lvPDI.DataSource = dt2;
                    lvPDI.DataBind();
                    Session["Machine"] = dt;
                    Session["Tester"] = dt1;
                    Session["PDI"] = dt2;
                    Session["CompleteDetails"] = dt3;
                }
                else
                {
                    DataTable dt = new DataTable();
                    List<WIPDashboardMachineDetails> list = DataBaseAccess.GetWIPMachineDetails(out dt, Fromdate, Todate, ddlType.SelectedValue.ToString(), ddlPartNo.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : ddlPartNo.SelectedValue.ToString(), ddlWIPProcess.SelectedValue.ToString());
                    List<WIPDashboardWIPDetails> list1 = new List<WIPDashboardWIPDetails>();
                    WIPDashboardWIPDetails data = null;
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtRow in dt.Rows)
                        {
                            data = new WIPDashboardWIPDetails();
                            data.HeatCodeNumber = dtRow["HeatCodeNumber"].ToString();
                            data.MachineID = dtRow["MachineID"].ToString();
                            data.PartNo = dtRow["PartNumber"].ToString();
                            data.OperationNo = dtRow["OperationNo"].ToString();
                            data.CompletedProcess = dtRow["CompletedProcess"].ToString();
                            data.NextProcess = dtRow["WIPProcess"].ToString();
                            data.Remarks = dtRow["Remarks"].ToString();
                            list1.Add(data);
                        }
                    }
                    lvWIPDetails.DataSource = list1;
                    lvWIPDetails.DataBind();
                    lvCountDetails.DataSource = list;
                    lvCountDetails.DataBind();
                    Session["CountDetails"] = list;
                    Session["WIPDetails"] = list1;
                    divWaiting.Visible = true;
                    divComplete.Visible = false;
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindGrids();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string Filename = "";
                if (ddlWIPProcess.SelectedValue.ToString().Equals("Completed PDI Process", StringComparison.OrdinalIgnoreCase))
                {
                    Filename = "WIPReportComplete_TAFE.xlsx";
                }
                else
                {
                    Filename = "WIPReport_TAFE.xlsx";
                }
                string Source = GetReportPath(Filename);
                string Template = "WIPReport_TAFE" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("WIPReport_TAFE template does not found at - \n " + Source);
                }
                else
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[1];
                    
                    int startrow = 7;
                    if (ddlWIPProcess.SelectedValue.ToString().Equals("Completed PDI Process", StringComparison.OrdinalIgnoreCase))
                    {
                        worksheet.Cells["B3"].Value = Convert.ToDateTime(txtFromDate.Text).ToString("dd-MM-yyyy");
                        worksheet.Cells["B4"].Value = Convert.ToDateTime(txtToDate.Text).ToString("dd-MM-yyyy");
                        worksheet.Cells["H3"].Value = ddlPartNo.SelectedValue.ToString();
                        worksheet.Cells["H4"].Value = ddlWIPProcess.SelectedValue.ToString();
                        DataTable dt = Session["Machine"] as DataTable;
                        DataTable dt1= Session["Tester"] as DataTable;
                        DataTable dt2= Session["PDI"] as DataTable;
                        DataTable dt3= Session["CompleteDetails"] as DataTable;
                        if (dt3.Rows.Count > 0)
                        {
                            foreach(DataRow dtRow in dt3.Rows)
                            {
                                worksheet.Cells[startrow, 1].Value =dtRow["MachineID"].ToString();
                                worksheet.Cells[startrow, 2].Value = dtRow["PartNumber"].ToString();
                                worksheet.Cells[startrow, 3].Value = dtRow["Ratio"].ToString();
                                worksheet.Cells[startrow, 4].Value = dtRow["version"].ToString();
                                worksheet.Cells[startrow, 5].Value = dtRow["CompletedQty"].ToString();
                                startrow++;
                            }
                            setThinBorder(worksheet, 7, 1, startrow - 1, 5);
                        }
                        
                        startrow = 7;
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtRow in dt.Rows)
                            {
                                worksheet.Cells[startrow, 7].Value = dtRow["PartNumber"].ToString();
                                worksheet.Cells[startrow, 8].Value = dtRow["CompletedQty"].ToString();
                                startrow++;
                            }
                            setThinBorder(worksheet, 7, 7, startrow - 1, 8);
                        }
                        
                        startrow = 7;
                        if (dt1.Rows.Count > 0)
                        {
                            foreach (DataRow dtRow in dt1.Rows)
                            {
                                worksheet.Cells[startrow, 10].Value = dtRow["SerialNo"].ToString();
                                worksheet.Cells[startrow, 11].Value = dtRow["HeatCode"].ToString();
                                worksheet.Cells[startrow, 12].Value = dtRow["PartNumber"].ToString();
                                worksheet.Cells[startrow, 13].Value = dtRow["MachineType"].ToString();
                                startrow++;
                            }
                            setThinBorder(worksheet, 7, 10, startrow - 1, 13);
                        }
                        
                        startrow = 7;
                        if (dt2.Rows.Count > 0)
                        {
                            foreach (DataRow dtRow in dt2.Rows)
                            {
                                worksheet.Cells[startrow, 15].Value = dtRow["SerialNo"].ToString();
                                worksheet.Cells[startrow, 16].Value = dtRow["HeatCodeNumber"].ToString();
                                worksheet.Cells[startrow, 17].Value = dtRow["HeatCodeType"].ToString();
                                worksheet.Cells[startrow, 18].Value = dtRow["PartNumber"].ToString();
                                startrow++;
                            }
                            setThinBorder(worksheet, 7, 15, startrow - 1, 18);
                        }
                        
                        startrow = 7;
                    }
                    else
                    {
                        worksheet.Cells["B3"].Value = Convert.ToDateTime(txtFromDate.Text).ToString("dd-MM-yyyy");
                        worksheet.Cells["B4"].Value = Convert.ToDateTime(txtToDate.Text).ToString("dd-MM-yyyy");
                        worksheet.Cells["G3"].Value = ddlPartNo.SelectedValue.ToString();
                        worksheet.Cells["G4"].Value = ddlWIPProcess.SelectedValue.ToString();
                        List<WIPDashboardMachineDetails> list = Session["CountDetails"] as List<WIPDashboardMachineDetails>;
                        if (list.Count > 0)
                        {
                            foreach (var data in list)
                            {
                                worksheet.Cells[startrow, 1].Value = data.MachineID;
                                worksheet.Cells[startrow, 2].Value = data.PartNo;
                                worksheet.Cells[startrow, 3].Value = data.OperationNo;
                                worksheet.Cells[startrow, 4].Value = data.Quantity;
                                startrow++;
                            }
                            setThinBorder(worksheet, 7, 1, startrow - 1, 4);
                        }
                       
                        startrow = 7;
                        worksheet.Cells[5, 6].Value = "WIP Details";
                        List<WIPDashboardWIPDetails> list1 = Session["WIPDetails"] as List<WIPDashboardWIPDetails>;
                        if (list1.Count > 0)
                        {
                            foreach (var data in list1)
                            {
                                worksheet.Cells[startrow, 6].Value = data.HeatCodeNumber;
                                worksheet.Cells[startrow, 7].Value = data.MachineID;
                                worksheet.Cells[startrow, 8].Value = data.PartNo;
                                worksheet.Cells[startrow, 9].Value = data.OperationNo;
                                worksheet.Cells[startrow, 10].Value = data.CompletedProcess;
                                worksheet.Cells[startrow, 11].Value = data.NextProcess;
                                worksheet.Cells[startrow, 12].Value = data.Remarks;
                                startrow++;
                            }
                            setThinBorder(worksheet, 7, 6, startrow - 1, 12);
                        }
                        
                    }
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        static string appPath = HttpContext.Current.Server.MapPath("~/Reports/TPMTrakReport");
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
    }
}