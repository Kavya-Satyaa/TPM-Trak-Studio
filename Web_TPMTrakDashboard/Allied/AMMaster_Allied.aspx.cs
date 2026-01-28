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
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Allied
{
    public partial class AMMaster_Allied : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachines();
                BindRevNo();
                lblCopySourceMAchineID.Text = ddlMachineID.SelectedValue;
                List<string> machineList = new List<string>();
                foreach (ListItem item in ddlMachineID.Items)
                {
                    if (!item.Value.Equals(lblCopySourceMAchineID.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        machineList.Add(item.Value);
                    }
                }
                lbCopyDestMachineID.DataSource = machineList;
                lbCopyDestMachineID.DataBind();
                lblCopyFrequency.Text = ddlFrequency.SelectedValue.ToString();
                ddlCopyFrequency.SelectedValue= ddlFrequency.SelectedValue.ToString();
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
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindRevNo()
        {
            try
            {
                //List<ListItem> list = AlliedDBAccess.GetRevNos(ddlMachineID.SelectedValue.ToString(), ddlFrequency.SelectedValue.ToString());
                //ddlRevNo.DataSource = list;
                //ddlRevNo.DataTextField = "Text";
                //ddlRevNo.DataValueField = "Value";
                //ddlRevNo.DataBind();
                string RevID = "";
                lblRevisionNo.Text = AlliedDBAccess.GetMaxRevNo(ddlMachineID.SelectedValue.ToString(), ddlFrequency.SelectedValue.ToString(),out RevID);
                hdnrevID.Value = RevID;
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
                List<AMMAsterData> list = AlliedDBAccess.GetAMMasterData(ddlMachineID.SelectedValue.ToString(), ddlFrequency.SelectedValue.ToString(), hdnrevID.Value!=""?Convert.ToInt32(hdnrevID.Value):1);
                lvGrid.DataSource = list;
                lvGrid.DataBind();
                if (list.Count > 0)
                {
                    //ddlRevNo.SelectedValue = list[0].RevID.ToString();
                    txtRevDate.Text = list[0].RevDate!=""? list[0].RevDate:DateTime.Now.ToString("dd-MM-yyyy");
                    txtRefNo.Text = list[0].RefNo;
                }
                Session["MasterCount"] = list.Count;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void linkeditRow_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewDataItem).DataItemIndex);
                txtCheckpointID.Text = (lvGrid.Items[rowIndex].FindControl("lblCheckpointID") as Label).Text;
                txtDescription.Text = (lvGrid.Items[rowIndex].FindControl("lblDesc") as Label).Text;
                txtCategoryID.Text = (lvGrid.Items[rowIndex].FindControl("lblCategoryID") as Label).Text;
                txtCategoryDesc.Text = (lvGrid.Items[rowIndex].FindControl("lblCategoryDesc") as Label).Text;

                txtCheckpointID.Enabled = false;
                hfNewEdit.Value = "EDIT";
                newEditModalTitle.InnerText = "Edit AM Master Data";
                HelperClass.openAddEditModal(this, "newEditModal");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
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

        protected void btnNewEditSave_Click(object sender, EventArgs e)
        {
            try
            {
                AMMAsterData data = new AMMAsterData();
                data.MachineID = ddlMachineID.SelectedValue.ToString();
                data.Frequency = ddlFrequency.SelectedValue.ToString();
                //data.RevID = ddlRevNo.SelectedValue.ToString()==""?"1":ddlRevNo.SelectedValue.ToString();
                //data.RevNo = ddlRevNo.SelectedValue.ToString() == "" ? 1 :Convert.ToInt32(ddlRevNo.SelectedValue.ToString());
                data.RevNo = lblRevisionNo.Text == "" ? "1" : lblRevisionNo.Text;
                data.RevID = hdnrevID.Value == "" ? 1 : Convert.ToInt32(hdnrevID.Value);
                DateTime Date = DateTime.Now;
                if (txtRevDate.Text == "")
                {
                    Date = Util.GetDateTime(DateTime.Now.ToString("MM-dd-yyyy"));
                    data.RevDate = Convert.ToDateTime(Date).ToString("dd-MM-yyyy");
                }
                else
                {
                    Date = Util.GetDateTime(txtRevDate.Text);
                    data.RevDate = Convert.ToDateTime(Date).ToString("yyyy-MM-dd");
                }
                //data.RevDate = txtRevDate.Text==""?DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"):Convert.ToDateTime(txtRevDate.Text).ToString("yyyy-MM-dd");
                data.RefNo = txtRefNo.Text;
                if (txtCheckpointID.Text == "")
                {
                    HelperClass.openWarningToastrModal(this, "Please enter Checkpoint ID");
                    HelperClass.clearModal(this);
                    return;
                }
                data.CheckpointID = txtCheckpointID.Text;
                data.CheckpointDesc = txtDescription.Text;
                data.CategoryID = txtCategoryID.Text;
                data.CategoryDesc = txtCategoryDesc.Text;
                string success = AlliedDBAccess.SaveAMMasterData(data,hfNewEdit.Value);
                if (success == "INSERTED")
                {
                    HelperClass.openInsertSuccessModal(this);
                    HelperClass.clearModal(this);
                    BindRevNo();
                    BindGrid();
                }
                else if (success == "UPDATED")
                {
                    HelperClass.openUpdateSuccessModal(this);
                    HelperClass.clearModal(this);
                    BindRevNo();
                    BindGrid();
                }
                else if (success == "CheckpointID already exists")
                {
                    HelperClass.openWarningToastrModal(this, success);
                    HelperClass.clearModal(this);
                    return;
                }
                else if(success == "")
                {
                    HelperClass.openInsertErrorModal(this);
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnDeleteConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                AMMAsterData data = new AMMAsterData();
                int RowIndex = (int)ViewState["DeleteRowIndex"];
                data.MachineID = ddlMachineID.SelectedValue.ToString();
                data.Frequency = ddlFrequency.SelectedValue.ToString();
                data.CheckpointID = (lvGrid.Items[RowIndex].FindControl("lblCheckpointID") as Label).Text;
                string success = AlliedDBAccess.DeleteAMMasterData(data);
                if (success == "DELETED")
                {
                    HelperClass.openDeleteSuccessModal(this);
                    HelperClass.clearModal(this);
                    BindGrid();
                }
                else
                {
                    HelperClass.openDeleteErrorModal(this);
                    return;
                }
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
                lblCopySourceMAchineID.Text = ddlMachineID.SelectedValue;
                List<string> machineList = new List<string>();
                foreach (ListItem item in ddlMachineID.Items)
                {
                    if (!item.Value.Equals(lblCopySourceMAchineID.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        machineList.Add(item.Value);
                    }
                }
                lbCopyDestMachineID.DataSource = machineList;
                lbCopyDestMachineID.DataBind();
                lblCopyFrequency.Text = ddlFrequency.SelectedValue.ToString();
                ddlCopyFrequency.SelectedValue = ddlFrequency.SelectedValue.ToString();
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlFrequency.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase)|| ddlFrequency.SelectedValue.ToString().Equals("Monthly", StringComparison.OrdinalIgnoreCase))
                {
                    if (Convert.ToInt32(Session["MasterCount"].ToString()) >= 8)
                    {
                        HelperClass.openWarningToastrModal(this, "Can not add more than 8 Checkpoints");
                        return;
                    }
                }
                else if (ddlFrequency.SelectedValue.ToString().Equals("Weekly", StringComparison.OrdinalIgnoreCase))
                {
                    if (Convert.ToInt32(Session["MasterCount"].ToString()) >= 12)
                    {
                        HelperClass.openWarningToastrModal(this, "Can not add more than 12 Checkpoints");
                        return;
                    }
                }
                txtCheckpointID.Text = "";
                txtDescription.Text = "";
                txtCategoryID.Text = "";
                txtCategoryDesc.Text = "";

                txtCheckpointID.Enabled = true;
                hfNewEdit.Value = "NEW";
                newEditModalTitle.InnerText = "Add AM Master Data";
                HelperClass.openAddEditModal(this, "newEditModal");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnSaveRevisionNo_Click(object sender, EventArgs e)
        {
            try
            {
                Session["NewRevNo"] = txtRevNoNew.Text.Trim();
                if (txtRevNoNew.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "openRevNoModal", "$('#NewRevNoModal').modal('show');", true);
                    HelperClass.openWarningToastrModal(this, "Please enter Revision Number.");
                }
                if (lblRevisionNo.Text==txtRevNoNew.Text.Trim())
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "openRevNoModal", "$('#NewRevNoModal').modal('show');", true);
                    HelperClass.openWarningToastrModal(this, "Duplicate Revision Number found.");
                }
                else
                {
                    AlliedDBAccess.SaveRevisionNo(txtRevNoNew.Text.Trim(), ddlMachineID.SelectedValue.ToString(), ddlFrequency.SelectedValue.ToString());
                    BindRevNo();
                }
                BindGrid();
                HelperClass.clearModal(this);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnCopyConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string sourceMachine = lblCopySourceMAchineID.Text;
                string destinationMachine = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbCopyDestMachineID);
                string result = AlliedDBAccess.CopyAMasterData_Allied(sourceMachine, destinationMachine,ddlCopyFrequency.SelectedValue.ToString());
                if (result == "Copied")
                {
                    HelperClassGeneric.openSuccessModal(this, "Record copied Successfully.");
                    HelperClassGeneric.clearModal(this);
                }
                else
                {
                    HelperClassGeneric.openModal(this, "copyModal", false);
                    HelperClassGeneric.openInsertErrorModal(this);
                    return;
                }
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        static readonly string appPath = HttpContext.Current.Server.MapPath("~/Allied");
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
        public static string SafeFileName(string name)
        {
            StringBuilder str = new StringBuilder(name);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.ToString();
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
                        DataTable dt = GetInspectionMaterData(savedFileName);
                        if (dt.Rows.Count > 0)
                        {
                            AlliedDBAccess.ImportAMMasterDatatoTemp(dt);
                            string success = AlliedDBAccess.ImportAMMasterData();
                            if (success == "Imported")
                            {
                                HelperClass.openSuccessModal(this);
                                BindRevNo();
                                BindGrid();
                            }
                            else
                            {
                                HelperClass.openWarningToastrModal(this, "Import Failed");
                                return;
                            }
                        }
                    }
                }
                else
                {
                    HelperClass.openWarningToastrModal(this, "Please choose a file to import.");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private DataTable GetInspectionMaterData(string FileName)
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
                        //IDD	MachineID	CheckpointID	CheckpointDescription	RefNo	RevID	RevNo	RevDate	CategoryID	Category	Frequency
                        var wsData = workbook.Worksheets[1];
                        dt.Columns.Add("MachineID", typeof(string));
                        dt.Columns.Add("CheckpointID", typeof(string));
                        dt.Columns.Add("CheckpointDescription", typeof(string));
                        dt.Columns.Add("RefNo", typeof(string));
                        dt.Columns.Add("RevID", typeof(string));
                        dt.Columns.Add("RevNo", typeof(int));
                        dt.Columns.Add("RevDate", typeof(DateTime));
                        dt.Columns.Add("CategoryID", typeof(string));
                        dt.Columns.Add("Category", typeof(string));
                        dt.Columns.Add("Frequency", typeof(string));
                        int startrow = 3;
                        int lastRow = GetLastUsedRow(wsData);
                        for (var rownum = startrow; rownum <= lastRow; rownum++)
                        {
                            string MachineID = wsData.Cells[rownum, 1].Value != null ? wsData.Cells[rownum, 1].Value.ToString() : "";
                            string CheckpointID = wsData.Cells[rownum, 3].Value != null ? wsData.Cells[rownum, 3].Value.ToString() : "";
                            string CheckpointDescription = wsData.Cells[rownum, 4].Value != null ? wsData.Cells[rownum, 4].Value.ToString() : "";
                            string RefNo = wsData.Cells[rownum, 7].Value != null ? wsData.Cells[rownum, 7].Value.ToString() : "";
                            string RevID = wsData.Cells[rownum,5].Value != null ? wsData.Cells[rownum, 5].Value.ToString() : "1";
                            string RevDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            if (wsData.Cells[rownum,6].Value.ToString()!=null && wsData.Cells[rownum, 6].Value.ToString() != "")
                            {
                                string Date1 = wsData.Cells[rownum, 6].Value.ToString();
                                if (double.TryParse(Date1, out double excelValue))
                                {
                                    double excelDateValue = Convert.ToDouble(excelValue);
                                    DateTime startDate = new DateTime(1900, 1, 1);
                                    DateTime date = startDate.AddDays(excelDateValue - 2); // Subtracting 2 to account for Excel's date system
                                    RevDate = date.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    RevDate = Convert.ToDateTime(wsData.Cells[rownum, 6].Value.ToString()).ToString("yyyy-MM-dd");
                                }
                            }
                            //string RevDate = wsData.Cells[rownum, 6].Value != null ? wsData.Cells[rownum, 6].Value.ToString() : "";
                            string CategoryID = wsData.Cells[rownum, 8].Value != null ? wsData.Cells[rownum, 8].Value.ToString() : "";
                            string Category = wsData.Cells[rownum, 9].Value != null ? wsData.Cells[rownum,9].Value.ToString() : "1";
                            string Frequency = wsData.Cells[rownum, 2].Value != null ? wsData.Cells[rownum,2].Value.ToString() : "1";

                            var dtRow = dt.NewRow();
                            dtRow[0] = MachineID;
                            dtRow[1] = CheckpointID;
                            dtRow[2] = CheckpointDescription;
                            dtRow[3] = RefNo;
                            dtRow[4] = RevID;
                            dtRow[5] = Convert.ToInt32(RevID);
                            //dtRow[6] = RevDate==""? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : Convert.ToDateTime(txtRevDate.Text).ToString("yyyy-MM-dd HH:mm:ss");
                            dtRow[6] = RevDate;
                            dtRow[7] = CategoryID;
                            dtRow[8] = Category;
                            dtRow[9] = Frequency;
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
                Filename = "AMMasterImport_Allied.xlsx";
                string Source = string.Empty;
                Source = GetReportPath(Filename);
                string Template = string.Empty;
                Template = "ChecksheetMasterImportTemp" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("ChecksheetMasterImportTemp- \n " + Source);
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

        protected void ddlFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRevNo();
        }

        protected void ddlMachineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRevNo();
        }
    }
}