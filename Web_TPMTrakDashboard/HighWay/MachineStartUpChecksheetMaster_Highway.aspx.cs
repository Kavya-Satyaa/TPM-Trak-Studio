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

namespace Web_TPMTrakDashboard.HighWay
{
    public partial class MachineStartUpChecksheetMaster_Highway : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnCancel.Visible = false;
                BindMachineID();
                lblCopySourceMachine.Text = ddlMachineID.SelectedValue;
                List<string> machineList = new List<string>();
                foreach (ListItem item in ddlMachineID.Items)
                {
                    if (!item.Value.Equals(lblCopySourceMachine.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        machineList.Add(item.Value);
                    }
                }
                lbCopyDestMachine.DataSource = machineList;
                lbCopyDestMachine.DataBind();
                BindGrid();
            }
        }
        private void BindMachineID()
        {
            try
            {
                List<string> list = DBAccess.GetMachines();
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
                List<MachineStartupChecksheetMasterData> list = DBAccess.GetMachineStartUpMasterData(ddlMachineID.SelectedValue.ToString());
                lvGrid.DataSource = list;
                lvGrid.DataBind();
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
                lblCopySourceMachine.Text = ddlMachineID.SelectedValue;
                List<string> machineList = new List<string>();
                foreach (ListItem item in ddlMachineID.Items)
                {
                    if (!item.Value.Equals(lblCopySourceMachine.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        machineList.Add(item.Value);
                    }
                }
                lbCopyDestMachine.DataSource = machineList;
                lbCopyDestMachine.DataBind();
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
                //txtCharacteristicID.Text = "";
                txtDescription.Text = "";
                txtPointsToBeChecked.Text = "";
                txtSortorder.Text = "";
                //txtCharacteristicID.Enabled = true;
                newEditModalTitle.InnerText = "Add Machine Checksheet Master";
                HelperClass.openAddEditModal(this, "newEditModal");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {

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
                //int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewDataItem).DataItemIndex);
                //txtCharacteristicID.Text = (lvGrid.Items[rowIndex].FindControl("lblCharacteristicID") as Label).Text;
                //txtDescription.Text = (lvGrid.Items[rowIndex].FindControl("lblDescription") as Label).Text;
                //txtPointsToBeChecked.Text = (lvGrid.Items[rowIndex].FindControl("lblPointsToBeChecked") as Label).Text;
                //ddlDataType.SelectedValue = (lvGrid.Items[rowIndex].FindControl("lbldataType") as Label).Text;
                //txtSortorder.Text = (lvGrid.Items[rowIndex].FindControl("lblSortOrder") as Label).Text;

                //txtCharacteristicID.Enabled = false;
                //newEditModalTitle.InnerText = "Edit Machine Startup Checksheet Details";
                //HelperClass.openAddEditModal(this, "newEditModal");
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
                MachineStartupChecksheetMasterData data = new MachineStartupChecksheetMasterData();
                string MachineID = ddlMachineID.SelectedValue.ToString();
                //data.CharacteristicID = txtCharacteristicID.Text;
                data.Description = txtDescription.Text;
                data.PointsToBeChecked = txtPointsToBeChecked.Text;
                data.DataType = ddlDataType.SelectedValue.ToString();
                if (data.Description == "")
                {
                    HelperClass.openWarningToastrModal(this, "Please enter Description");
                    return;
                }
                else if (data.PointsToBeChecked == "")
                {
                    HelperClass.openWarningToastrModal(this, "Please enter Checkpoints");
                    return;
                }
                data.SortOrder = Convert.ToInt32(txtSortorder.Text);
                string success = DBAccess.SaveMachineStartupMasterData(data, MachineID);
                if (success == "Inserted")
                {
                    HelperClass.openInsertSuccessModal(this);
                    HelperClass.clearModal(this);
                    BindGrid();
                }
                else if (success == "Updated")
                {
                    HelperClass.openUpdateSuccessModal(this);
                    HelperClass.clearModal(this);
                    BindGrid();
                }
                else
                {
                    HelperClass.openAddEditModal(this, "newEditModal");
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
                MachineStartupChecksheetMasterData data = new MachineStartupChecksheetMasterData();
                int RowIndex = (int)ViewState["DeleteRowIndex"];
                //data.CharacteristicID = (lvGrid.Items[RowIndex].FindControl("lblCharacteristicID") as Label).Text;
                data.Description = (lvGrid.Items[RowIndex].FindControl("lblDescription") as Label).Text;
                data.PointsToBeChecked = (lvGrid.Items[RowIndex].FindControl("lblPointsToBeChecked") as Label).Text;
                string MachineID = ddlMachineID.SelectedValue.ToString();
                string success = DBAccess.DeleteMachineStartupMasterData(data, MachineID);
                if (success == "Deleted")
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string success = ""; int flag = 1;
            try
            {
                foreach (ListViewItem row in lvGrid.Items)
                {
                    MachineStartupChecksheetMasterData data = new MachineStartupChecksheetMasterData();
                    string MachineID = ddlMachineID.SelectedValue.ToString();
                    data.Description = (row.FindControl("lblDescription") as Label).Text;
                    data.PointsToBeChecked = (row.FindControl("lblPointsToBeChecked") as Label).Text;
                    data.DataType = (row.FindControl("lbldataType") as Label).Text;
                    data.SortOrder = Convert.ToInt32((row.FindControl("txSortOrder") as TextBox).Text);
                    int sortOrderr = Convert.ToInt32((row.FindControl("hdnTextOrder") as HiddenField).Value);
                    if (data.SortOrder != sortOrderr)
                    {
                        success = DBAccess.SaveMachineStartupMasterData(data, MachineID);
                        flag++;
                    }
                }
                if (flag == 1)
                {
                    HelperClass.openWarningToastrModal(this, "Please Edit Records to Update");
                    return;
                }
                if (success == "Inserted")
                {
                    HelperClass.openInsertSuccessModal(this);
                    HelperClass.clearModal(this);
                    BindGrid();
                }
                else if (success == "Updated")
                {
                    HelperClass.openUpdateSuccessModal(this);
                    HelperClass.clearModal(this);
                    BindGrid();
                }
                else
                {
                    HelperClass.openInsertErrorModal(this);
                    return;
                }
                lvGrid.EditIndex = -1;
                BindGrid();
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
                        DataTable dt = GetCheckheetMaterData(savedFileName);
                        if (dt.Rows.Count > 0)
                        {
                            DBAccess.ImportChecksheetMasterData(dt);
                            string success = DBAccess.ImportChecksheetData();
                            //if (success != "")
                            //{
                            HelperClass.openSuccessModal(this);
                            BindGrid();
                            //}
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
        private DataTable GetCheckheetMaterData(string FileName)
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
                        // IDD MachineID   CharacteristicID Characteristics PointsToBeChecked DataType    SortOrder
                        var wsData = workbook.Worksheets[1];
                        dt.Columns.Add("MachineID", typeof(string));
                        dt.Columns.Add("CharacteristicID", typeof(string));
                        dt.Columns.Add("Characteristics", typeof(string));
                        dt.Columns.Add("PointsToBeChecked", typeof(string));
                        dt.Columns.Add("DataType", typeof(string));
                        dt.Columns.Add("SortOrder", typeof(int));
                        int startrow = 2;
                        int lastRow = GetLastUsedRow(wsData);
                        for (var rownum = startrow; rownum <= lastRow; rownum++)
                        {
                            string Description = wsData.Cells[rownum, 1].Value != null ? wsData.Cells[rownum, 1].Value.ToString() : "";
                            string PointsToBeChecked = wsData.Cells[rownum, 2].Value != null ? wsData.Cells[rownum, 2].Value.ToString() : "";
                            string DataType = wsData.Cells[rownum, 3].Value != null ? wsData.Cells[rownum, 3].Value.ToString() : "";
                            int SortOrder = wsData.Cells[rownum, 4].Value != null ? Convert.ToInt32(wsData.Cells[rownum, 4].Value.ToString()) : 1;
                            var dtRow = dt.NewRow();
                            dtRow[0] = ddlMachineID.SelectedValue.ToString();
                            dtRow[1] = "";
                            dtRow[2] = Description;
                            dtRow[3] = PointsToBeChecked;
                            dtRow[4] = DataType==""? "OK/NotOK":DataType;
                            dtRow[5] = SortOrder;
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
        static readonly string appPath = HttpContext.Current.Server.MapPath("~/Highway");
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
        public static string SafeFileName(string name)
        {
            StringBuilder str = new StringBuilder(name);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.ToString();
        }
        protected void btnTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                string Filename = string.Empty;
                string templatefile = string.Empty;
                Filename = "ChecksheetMasterImportTemp.xlsx";
                string Source = string.Empty;
                Source = HighwayGenerateReports.GetReportPath(Filename);
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

        protected void btnCopyConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string sourceMachine = lblCopySourceMachine.Text;
                string destinationMachine = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbCopyDestMachine);
                string result = DBAccess.CopyChecksheetData(sourceMachine, destinationMachine);
                if (result == "Inserted")
                {
                    HelperClassGeneric.openSuccessModal(this, "Record copied Successfully.");
                    HelperClassGeneric.clearModal(this);
                }
                else if(result== "Destination Machine Already Exist.")
                {
                    HelperClassGeneric.openWarningToastrModal(this, result);
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
    }
}