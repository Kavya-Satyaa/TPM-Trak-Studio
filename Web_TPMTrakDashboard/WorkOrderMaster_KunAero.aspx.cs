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
    public partial class WorkOrderMaster_KunAero : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                //txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                //BindWorkOrderID();
                //BindWorkOrderNumber();
                //BindFY();
                BindGrid();
            }
        }
        //private void BindWorkOrderID()
        //{
        //    try
        //    {
        //        List<string> list = DataBaseAccess.GetWorkOrderID_KunAero();
        //        list.Insert(0, "ALL");
        //        ddlWorkOrderID.DataSource = list;
        //        ddlWorkOrderID.DataBind();
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex);
        //    }
        //}
        //private void BindFY()
        //{
        //    try
        //    {
        //        List<string> list = DataBaseAccess.GetWorkOrderFY_KunAero();
        //        lbFyear.DataSource = list;
        //        lbFyear.DataBind();
        //        foreach(ListItem item in lbFyear.Items)
        //        {
        //            item.Selected = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex);
        //    }
        //}
        //private void BindWorkOrderNumber()
        //{
        //    try
        //    {
        //        List<string> list = DataBaseAccess.GetWorkOrderNo_KunAero();
        //        list.Insert(0, "ALL");
        //        ddlWorkOrderNo.DataSource = list;
        //        ddlWorkOrderNo.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex);
        //    }
        //}
        private void BindGrid()
        {
            try
            {
                DataTable dt = DataBaseAccess.GetWorkOrderData_Kunaero(txtWorkOrder.Text, txtFromDate.Text, txtToDate.Text);
                //string FY = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbFyear);
                //DataTable dt = DataBaseAccess.GetWorkOrderData_Kunaero_New(ddlWorkOrderNo.SelectedValue.ToString().Equals("ALL",StringComparison.OrdinalIgnoreCase)?"": ddlWorkOrderNo.SelectedValue.ToString(), txtDate.Text, FY, ddlWorkOrderID.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : ddlWorkOrderID.SelectedValue.ToString());
                List<WorkOrderData_KunAero> list = new List<WorkOrderData_KunAero>();
                WorkOrderData_KunAero data = null;
                int i = 1;
                foreach(DataRow dtRow in dt.Rows)
                {
                    data = new WorkOrderData_KunAero();
                    data.SlNo = i++;
                    data.WorkOrderFY = dtRow["WorkOrderFY"].ToString();
                    data.WorkOrderID = dtRow["WorkOrderID"].ToString();
                    data.WorkOrderNumber = dtRow["WorkOrderNumber"].ToString();
                    data.WorkOrderDate =Convert.ToDateTime(dtRow["WorkOrderDate"].ToString()).ToString("dd-MM-yyyy");
                    data.WorkOrderQty = dtRow["WorkOrderQty"].ToString();
                    list.Add(data);
                }
                //if (!(list.Count > 0))
                //{
                //    list.Add(new WorkOrderData_KunAero() { WorkOrderNumber = "", WorkOrderDate = "", WorkOrderQty = "" });
                //    //gvGrid.DataSource = list;
                //    //gvGrid.DataBind();
                //    //gvGrid.Rows[0].Visible = false;
                //    //gvGrid.FooterRow.Visible = true;
                //    lvGrid.DataSource = list;
                //    lvGrid.DataBind();
                //    lvGrid.InsertItem.Visible = true;
                //}
                //else
                //{
                    //gvGrid.DataSource = list;
                    //gvGrid.DataBind();
                    lvGrid.DataSource = list;
                    lvGrid.DataBind();
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void linkDeleteRow_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["DeleteRowIndex"] = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewDataItem).DataItemIndex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmModal", "openDeleteConfirmModal()", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void linkAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                string workorderID = (lvGrid.InsertItem.FindControl("txtID") as TextBox).Text;
                string workorderFY = (lvGrid.InsertItem.FindControl("txtFY") as TextBox).Text;
                string workordernumber = (lvGrid.InsertItem.FindControl("txtWorkOrderNo") as TextBox).Text;
                string workorderDate = (lvGrid.InsertItem.FindControl("txtWorkOrderDate") as TextBox).Text;
                string workorderqty = (lvGrid.InsertItem.FindControl("txtWorkOrderQty") as TextBox).Text;
                if (workorderFY == "")
                {
                    HelperClassGeneric.openWarningModal(this, "Please enter workorder Financial Year");
                    return;
                }
                else if (workorderID == "")
                {
                    HelperClassGeneric.openWarningModal(this, "Please enter workorder ID");
                    return;
                }
                else if (workordernumber == "")
                {
                    HelperClassGeneric.openWarningModal(this, "Please enter workorder Number");
                    return;
                }
                else if (workorderDate=="")
                {
                    HelperClassGeneric.openWarningModal(this, "Please enter workorder Date");
                    return;
                }
                else if (workorderqty=="")
                {
                    HelperClassGeneric.openWarningModal(this, "Please enter workorder Qty");
                    return;
                }
                string success = DataBaseAccess.SaveWorkOrderData_Kunaero(workordernumber, workorderDate, workorderqty,workorderID,workorderFY);
                if (success != "")
                {
                    HelperClassGeneric.openInsertSuccessModal(this);
                    //BindFY();
                    //BindWorkOrderID();
                    //BindWorkOrderNumber();
                    BindGrid();
                }
                else
                {
                    HelperClassGeneric.openInsertErrorModal(this);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                string Filename = string.Empty;
                string templatefile = string.Empty;
                Filename = "WorkOrderDetailsTemplate_KunAero.xlsx";
                string Source = string.Empty;
                Source = GetReportPath(Filename);
                string Template = string.Empty;
                Template = "WorkOrderDetailsTemplate_KunAero" + DateTime.Now + ".xlsx";
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
                        DataTable dt = GetWorkOrderDataFromExcelFile(savedFileName);
                        if (dt.Rows.Count > 0)
                        {
                            DataBaseAccess.ImportWorkOrderData_KunAero(dt);
                            HelperClassGeneric.openSuccessModal(this, "Data imported successfully");
                            //BindFY();
                            //BindWorkOrderID();
                            //BindWorkOrderNumber();
                            BindGrid();
                        }
                    }
                }
                else
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Please choose a file to import.");
                    //ScriptManager.RegisterStartupScript(this, GetType(), "RecordsTextopenModaladded", "openWarningModal('Please choose a file to import.');", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private DataTable GetWorkOrderDataFromExcelFile(string FileName)
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
                        //ID	WorkOrderFY	WorkOrderID	WorkOrderNumber	WorkOrderDate	WorkOrderQty	UpdatedTS
                        dt.Columns.Add("WorkOrderNumber", typeof(string));
                        dt.Columns.Add("WorkOrderDate", typeof(DateTime));
                        dt.Columns.Add("WorkOrderQty", typeof(int));
                        dt.Columns.Add("UpdatedTS", typeof(DateTime));
                        dt.Columns.Add("WorkOrderFY", typeof(string));
                        dt.Columns.Add("WorkOrderID", typeof(string));
                        int startrow = 4;
                        int lastRow = GetLastUsedRow(wsData);
                        for (var rownum = startrow; rownum <= lastRow; rownum++)
                        {
                            string WorkOrderFY = wsData.Cells[rownum, 2].Value != null ? wsData.Cells[rownum, 2].Value.ToString() : "";
                            string WorkOrderID= wsData.Cells[rownum, 3].Value != null ? wsData.Cells[rownum, 3].Value.ToString() : "";
                            string WorkOrderNumber = "";
                            if (wsData.Cells[rownum, 4].Value != null)
                            {
                                WorkOrderNumber = wsData.Cells[rownum, 4].Value.ToString();
                            }
                            string WorkOrderDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            if (wsData.Cells[rownum, 5].Value.ToString() != null)
                            {
                                string Date1 = wsData.Cells[rownum, 5].Value.ToString();
                                if (double.TryParse(Date1, out double excelValue))
                                {
                                    double excelDateValue = Convert.ToDouble(excelValue);
                                    DateTime startDate = new DateTime(1900, 1, 1);
                                    DateTime date = startDate.AddDays(excelDateValue - 2); 
                                    WorkOrderDate = date.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    WorkOrderDate = Convert.ToDateTime(wsData.Cells[rownum, 2].Value.ToString()).ToString("yyyy-MM-dd");
                                }
                            }
                            int WorkOrderQty = 0;
                            if (wsData.Cells[rownum, 6].Value != null)
                            {
                                WorkOrderQty = Convert.ToInt32(wsData.Cells[rownum, 6].Value.ToString());
                            }
                            var dtRow = dt.NewRow();
                            dtRow[0] = WorkOrderNumber;
                            dtRow[1] = WorkOrderDate;
                            dtRow[2] = WorkOrderQty;
                            dtRow[3] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                            dtRow[4] = WorkOrderFY;
                            dtRow[5] = WorkOrderID;
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

        protected void btnDeleteConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = Convert.ToInt32(ViewState["DeleteRowIndex"].ToString());
                string workorderID = (lvGrid.Items[rowIndex].FindControl("lblID") as Label).Text;
                string workorderFY = (lvGrid.Items[rowIndex].FindControl("lblFY") as Label).Text;
                string workordernumber = (lvGrid.Items[rowIndex].FindControl("lblWorkOrder") as Label).Text;
                string workorderDate = (lvGrid.Items[rowIndex].FindControl("lblWorkOrderDate") as Label).Text;
                string workorderqty = (lvGrid.Items[rowIndex].FindControl("lblWorkOrderQty") as Label).Text;
                string success = DataBaseAccess.DeleteWorkOrderData_Kunaero(workordernumber, workorderDate, workorderqty,workorderID,workorderFY);
                if (success != "")
                {
                    HelperClassGeneric.openDeleteSuccessModal(this);
                    HelperClassGeneric.clearModal(this);
                    //BindFY();
                    //BindWorkOrderID();
                    //BindWorkOrderNumber();
                    BindGrid();
                }
                else
                {
                    HelperClassGeneric.clearModal(this);
                    HelperClassGeneric.openInsertErrorModal(this);
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        //protected void lvGrid_ItemCommand(object sender, ListViewCommandEventArgs e)
        //{
        //    try
        //    {
        //        if (e.CommandName == "DeleteRow")
        //        {
        //            ViewState["DeleteRowIndex"] = Convert.ToInt32(e.CommandArgument);
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmModal", "openDeleteConfirmModal()", true);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex);
        //    }
        //}
    }
}