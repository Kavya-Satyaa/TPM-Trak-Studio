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
using Web_TPMTrakDashboard.Bajaj.Model;

namespace Web_TPMTrakDashboard.Allied
{
    public partial class AMTransaction_Allied : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachines();
                txtMonth.Text = DateTime.Now.ToString("MM");
                txtYear.Text = DateTime.Now.ToString("yyyy");
                BindGrid();
            }
        }
        private void BindMachines()
        {
            try
            {
                List<string> list = AlliedDBAccess.GetMachineIDs();
                ddlMachineID.DataSource = list;
                ddlMachineID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindGrid()
        {
            try
            {
                string Result = "";
                DataTable dt1 = new DataTable();
                DataTable dt = AlliedDBAccess.GetAMTransactiondata_Allied(ddlMachineID.SelectedValue.ToString(), Convert.ToInt32(txtYear.Text), Convert.ToInt32(txtMonth.Text), out dt1, ddlFrequency.SelectedValue.ToString());
                List<AMTransactionData> list = new List<AMTransactionData>();
                AMTransactionData data = null; int k = 0;
                List<AMTransactionData> innerList = new List<AMTransactionData>();
                AMTransactionData innerdata = null;
                if (dt.Columns.IndexOf("SaveFlag") != -1)
                {
                    Result = dt.Rows[0]["SaveFlag"].ToString();
                    lvDetails.DataSource = list;
                    lvDetails.DataBind();
                }
                if (!string.IsNullOrEmpty(Result))
                {
                    HelperClass.openWarningToastrModal(this, Result);
                    return;
                }
                else
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (k == 0)
                        {
                            data = new AMTransactionData();
                            data.CheckpointID = "Sl No";
                            data.CheckpointDesc = "Checkpoint";
                            data.HeaderVisibility = true;
                            innerList = new List<AMTransactionData>();
                            for (int i = 7; i < dt.Columns.Count; i++)
                            {
                                innerdata = new AMTransactionData();
                                innerdata.HearderValue = dt.Columns[i].ColumnName.ToString();
                                innerdata.HeaderVisibility = true;
                                innerList.Add(innerdata);
                            }
                            data.transactionData = innerList;
                            list.Add(data);
                        }
                        foreach (DataRow dtrow in dt.Rows)
                        {

                            data = new AMTransactionData();
                            data.CheckpointID = dtrow["CheckpointID"].ToString();
                            data.CheckpointDesc = dtrow["CheckpointDescription"].ToString();
                            data.RefNo = dtrow["RefNo"].ToString();
                            data.RevID = dtrow["RevID"].ToString();
                            data.RevDate = dtrow["RevDate"].ToString();
                            data.ContentVisibility = true;
                            innerList = new List<AMTransactionData>();
                            for (int i = 7; i < dt.Columns.Count; i++)
                            {
                                innerdata = new AMTransactionData();
                                innerdata.HearderValue = dtrow[dt.Columns[i].ColumnName.ToString()].ToString();
                                innerdata.ContentVisibility = true;
                                innerList.Add(innerdata);
                            }
                            data.transactionData = innerList;
                            list.Add(data);
                            k++;
                        }
                        data = new AMTransactionData();
                        innerList = new List<AMTransactionData>();
                        data.ApproveVisibility = true;
                        if (dt1 != null && dt1.Rows.Count > 0)
                        {
                            for (int l = 1; l < dt1.Columns.Count; l++)
                            {
                                innerdata = new AMTransactionData();
                                innerdata.ContentValue = dt1.Rows[0][l].ToString();
                                innerdata.ContentVisibility = true;
                                innerList.Add(innerdata);
                            }
                        }
                        data.transactionData = innerList;
                        list.Add(data);

                    }

                }
                lvDetails.DataSource = list;
                lvDetails.DataBind();
                Session["Data"] = list;
                Session["OperatorData"] = dt1;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        static readonly string appPath = HttpContext.Current.Server.MapPath("~/Allied");
        #region "Get Report Template File Path"
        public static string GetReportPath(string reportName)
        {
            string src = string.Empty;
            if (HttpContext.Current.Session["Language"] == null)
                src = Path.Combine(appPath, "Template", reportName);
            else
            {
                if (HttpContext.Current.Session["Language"].ToString() != "en")
                    src = Path.Combine(appPath, "Template-" + HttpContext.Current.Session["Language"].ToString() + "", reportName);
                else
                    src = Path.Combine(appPath, "Template", reportName);
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
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string Filename = "MaintenanceReport_Allied.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "MaintenanceReport_Allied" + DateTime.Now + ".xlsx";
                string destination = Path.Combine(appPath, "GeneratedReports", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("MaintenanceReport_Allied template does not exists at - " + Source);
                }
                else
                {
                    int rowStart = 7;
                    int headerColStart = 3, colStart = 1, k = 1;
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    List<AMTransactionData> list = Session["Data"] as List<AMTransactionData>;
                    DataTable dtOperator = Session["OperatorData"] as DataTable;
                    if (list.Count > 0)
                    {
                        var workSheet = Excel.Workbook.Worksheets["Sheet1"];
                        for (int i = 0; i < list.Count; i++)
                        {
                            colStart = 1;
                            if (i == 0)
                            {
                                if (list[0].transactionData.Count > 0)
                                {
                                    for (int j = 0; j < list[0].transactionData.Count; j++)
                                    {
                                        workSheet.Cells[6, headerColStart].Value = list[0].transactionData[j].HearderValue.ToString();
                                        headerColStart++;
                                    }
                                }
                            }
                            else
                            {
                                workSheet.Cells[rowStart, colStart].Value = list[i].CheckpointID;
                                colStart++;
                                workSheet.Cells[rowStart, colStart].Value = list[i].CheckpointDesc;
                                colStart++;
                                for (int j = 0; j < list[i].transactionData.Count; j++)
                                {
                                    workSheet.Cells[rowStart, colStart].Value = list[i].transactionData[j].HearderValue.ToString();
                                    colStart++;
                                }
                                rowStart++;
                            }
                            k++;
                        }
                        colStart = 3;
                        workSheet.Cells[rowStart-1, 2].Value = "Operator";
                        if (dtOperator.Rows.Count > 0)
                        {
                            for (int l = 1; l < dtOperator.Columns.Count; l++)
                            {
                                workSheet.Cells[rowStart-1, colStart].Value = dtOperator.Rows[0][l].ToString();
                                colStart++;
                            }
                        }
                        setBorderThin(workSheet, 6, 1, rowStart - 1, colStart - 1);
                        if (ddlFrequency.SelectedValue.ToString() == "Daily")
                        {
                            workSheet.Cells["C1"].Value = "Daily Maintenance Report";
                            workSheet.Cells["F4"].Value = "Month :";
                            workSheet.Cells["G4"].Value = txtMonth.Text;
                        }
                        else if (ddlFrequency.SelectedValue.ToString() == "Weekly")
                        {
                            workSheet.Cells["C1"].Value = "Weekly Maintenance Report";
                        }
                        else if (ddlFrequency.SelectedValue.ToString() == "Monthly")
                        {
                            workSheet.Cells["C1"].Value = "Monthly Maintenance Report";
                        }
                        workSheet.Cells["P1"].Value = list[1].RefNo;
                        workSheet.Cells["P2"].Value = list[1].RevID;
                        workSheet.Cells["P3"].Value = list[1].RevDate;
                        workSheet.Cells["B4"].Value = ddlMachineID.SelectedValue.ToString();
                        workSheet.Cells["E4"].Value = txtYear.Text;
                        DownloadMultipleFile(destination, Excel.GetAsByteArray());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlFrequency.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase))
                {
                    tdMonth.Visible = true;
                    txtMonth.Visible = true;
                }
                else
                {
                    tdMonth.Visible = false;
                    txtMonth.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}