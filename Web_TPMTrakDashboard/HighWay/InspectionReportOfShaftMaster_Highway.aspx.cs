using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.HighWay
{
    public partial class InspectionReportOfShaftMaster_Highway : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachines();
                BindComponent();
                BindOperation();
                BindRev();
                lblCopySourceComponent.Text = ddlComponent.SelectedValue;
                List<string> componentList = new List<string>();
                foreach (ListItem item in ddlComponent.Items)
                {
                    componentList.Add(item.Value);
                }
                lbCopyDestComponent.DataSource = componentList;
                lbCopyDestComponent.DataBind();
                lblCopyOperation.Text = ddlOperation.SelectedValue;
                List<string> operationList = new List<string>();
                foreach (ListItem item in ddlOperation.Items)
                {
                    operationList.Add(item.Value);
                }
                lbCopyOperation.DataSource = operationList;
                lbCopyOperation.DataBind();
                BindGrid();
            }
        }
        private void BindMachines()
        {
            try
            {
                List<string> list = DBAccess.GetMachines();
                //ddlMachineID.DataSource = list;
                //ddlMachineID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindComponent()
        {
            try
            {
                List<string> list = DBAccess.GetComponents();
                ddlComponent.DataSource = list;
                ddlComponent.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindOperation()
        {
            try
            {
                List<string> list = DBAccess.GetOperations(ddlComponent.SelectedValue.ToString());
                ddlOperation.DataSource = list;
                ddlOperation.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindRev()
        {
            try
            {
                //List<ListItem> list = DBAccess.GetRevID(ddlComponent.SelectedValue.ToString(), ddlOperation.SelectedValue.ToString());
                //ddlRevNo.DataSource = list;
                //ddlRevNo.DataTextField = "Text";
                //ddlRevNo.DataValueField = "Value";
                //ddlRevNo.DataBind();
                hdnRevisionID.Value = DBAccess.GetMaxRevID(ddlComponent.SelectedValue.ToString(), ddlOperation.SelectedValue.ToString());
                lblRevisionID.Text = DBAccess.GetMaxRevNo(ddlComponent.SelectedValue.ToString(), ddlOperation.SelectedValue.ToString(), hdnRevisionID.Value);
                if (lblRevisionID.Text == "")
                {
                    btnCreateRevisionNo.Visible = false;
                }
                else
                {
                    btnCreateRevisionNo.Visible = true;
                }
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
                List<InspectionReportofShaftmasterData> list = DBAccess.GetInspectionMasterData(ddlComponent.SelectedValue.ToString(), ddlOperation.SelectedValue.ToString(), hdnRevisionID.Value);
                lvGrid.DataSource = list;
                lvGrid.DataBind();
                if (list.Count > 0)
                {
                    txtDocNo.Text = list[0].DocNo;
                    txtRevDate.Text = list[0].RevDate != "" ? Convert.ToDateTime(list[0].RevDate).ToString("dd-MM-yyyy") : "";
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
                lblCopySourceComponent.Text = ddlComponent.SelectedValue;
                List<string> componentList = new List<string>();
                foreach (ListItem item in ddlComponent.Items)
                {
                    //if (!item.Value.Equals(lblCopySourceComponent.Text, StringComparison.OrdinalIgnoreCase))
                    //{
                    componentList.Add(item.Value);
                    //}
                }
                lbCopyDestComponent.DataSource = componentList;
                lbCopyDestComponent.DataBind();
                lblCopyOperation.Text = ddlOperation.SelectedValue;
                List<string> operationList = new List<string>();
                foreach (ListItem item in ddlOperation.Items)
                {
                    //if (!item.Value.Equals(lblCopyOperation.Text, StringComparison.OrdinalIgnoreCase))
                    //{
                    operationList.Add(item.Value);
                    //}
                }
                lbCopyOperation.DataSource = operationList;
                lbCopyOperation.DataBind();
                BindRev();
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
                //Characteristics,BalNo,Specification,InspectionMethod,Frequency,RevID,RevNo,RevDate,DocNo,UpdatedTS,Freq_Quantity
                txtCharacteristicID.Text = "";
                txtDescription.Text = "";
                txtBalNo.Text = "";
                txtSpecification.Text = "";
                txtInspectionMethod.Text = "";
                txtFrequency.Text = "";
                txtFrequencyQty.Text = "";
                txtSortOrder.Text = "";

                txtCharacteristicID.Enabled = true;
                newEditModalTitle.InnerText = "Add Inspection Characteristic";
                HelperClass.openAddEditModal(this, "newEditModal");
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
                InspectionReportofShaftmasterData data = new InspectionReportofShaftmasterData();
                data.CharacteristicID = txtCharacteristicID.Text;
                data.Description = txtDescription.Text;
                data.BalNo = txtBalNo.Text;
                data.Specification = txtSpecification.Text;
                data.InspectionMethod = txtInspectionMethod.Text;
                data.Frequency = txtFrequency.Text;
                data.FrequencyQty = txtFrequencyQty.Text==""?1:Convert.ToSingle(txtFrequencyQty.Text);
                data.RevID = hdnRevisionID.Value != "" ? Convert.ToInt32(hdnRevisionID.Value) : 1;
                data.DataType = ddlDataType.SelectedValue.ToString();
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
                
                data.DocNo = txtDocNo.Text;
                data.SortOrder = txtSortOrder.Text;
                string success = DBAccess.SaveInspectionMasterData(data, ddlComponent.SelectedValue.ToString(), ddlOperation.SelectedValue.ToString());
                if (success == "Inserted")
                {
                    HelperClass.openInsertSuccessModal(this);
                    HelperClass.clearModal(this);
                    BindRev();
                    BindGrid();

                }
                else if (success == "Updated")
                {
                    HelperClass.openUpdateSuccessModal(this);
                    HelperClass.clearModal(this);
                    BindRev();
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
                InspectionReportofShaftmasterData data = new InspectionReportofShaftmasterData();
                int RowIndex = (int)ViewState["DeleteRowIndex"];
                data.CharacteristicID = (lvGrid.Items[RowIndex].FindControl("lblCharacteristicID") as Label).Text;
                string success = DBAccess.DeleteInspectionMasterData(data, ddlComponent.SelectedValue.ToString(), ddlOperation.SelectedValue.ToString(), hdnRevisionID.Value);
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

        protected void linkeditRow_Click(object sender, EventArgs e)
        {
            try
            {
                //Characteristics,BalNo,Specification,InspectionMethod,Frequency,RevID,RevNo,RevDate,DocNo,UpdatedTS,Freq_Quantity
                int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewDataItem).DataItemIndex);
                txtCharacteristicID.Text = (lvGrid.Items[rowIndex].FindControl("lblCharacteristicID") as Label).Text;
                txtDescription.Text = (lvGrid.Items[rowIndex].FindControl("lblDesc") as Label).Text;
                txtBalNo.Text = (lvGrid.Items[rowIndex].FindControl("lblBalNo") as Label).Text;
                txtSpecification.Text = (lvGrid.Items[rowIndex].FindControl("lblSpec") as Label).Text;
                txtInspectionMethod.Text = (lvGrid.Items[rowIndex].FindControl("lblInspectionMethod") as Label).Text;
                txtFrequency.Text = (lvGrid.Items[rowIndex].FindControl("lblFrequency") as Label).Text;
                txtFrequencyQty.Text = (lvGrid.Items[rowIndex].FindControl("lblQty") as Label).Text;
                ddlDataType.SelectedValue = (lvGrid.Items[rowIndex].FindControl("lbldataType") as Label).Text;
                txtSortOrder.Text = (lvGrid.Items[rowIndex].FindControl("lblSortOrder") as Label).Text;
                txtCharacteristicID.Enabled = false;
                newEditModalTitle.InnerText = "Edit Inspection Report Of Shaft Details";
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
                //if (ddlRevNo.Items.FindByValue(txtRevNoNew.Text.Trim()) != null)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "openRevNoModal", "$('#NewRevNoModal').modal('show');", true);
                //    HelperClass.openWarningToastrModal(this, "Duplicate Revision Number found.");
                //}
                if (lblRevisionID.Text==txtRevNoNew.Text.Trim())
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "openRevNoModal", "$('#NewRevNoModal').modal('show');", true);
                    HelperClass.openWarningToastrModal(this, "Duplicate Revision Number found.");
                }
                else
                {
                    DBAccess.SaveRevisionNo(txtRevNoNew.Text.Trim(), ddlComponent.SelectedValue.ToString(), ddlOperation.SelectedValue.ToString());
                    BindRev();
                }
                BindGrid();
                HelperClass.clearModal(this);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void ddlRevNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                List<InspectionReportofShaftmasterData> list = DBAccess.GetInspectionMasterData(ddlComponent.SelectedValue.ToString(), ddlOperation.SelectedValue.ToString(), lblRevisionID.Text);
                txtDocNo.Text = list[0].DocNo;
                txtRevDate.Text = Convert.ToDateTime(list[0].RevDate).ToString("dd-MM-yyyy");
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
                        DataTable dt = GetInspectionMaterData(savedFileName);
                        if (dt.Rows.Count > 0)
                        {
                            DBAccess.ImportInspectionMasterData(dt);
                            string success = DBAccess.ImportInspectionData();
                            HelperClass.openSuccessModal(this);
                            BindRev();
                            BindGrid();
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
                        //IDD MachineID   ComponentID OperationNo CharacteristicID Characteristics BalNo Specification   InspectionMethod Frequency   RevID RevNo   RevDate DocNo   UpdatedTS Freq_Quantity   DataType SortOrder
                        var wsData = workbook.Worksheets[1];
                        dt.Columns.Add("MachineID", typeof(string));
                        dt.Columns.Add("ComponentID", typeof(string));
                        dt.Columns.Add("OperationNo", typeof(int));
                        dt.Columns.Add("CharacteristicID", typeof(string));
                        dt.Columns.Add("Characteristics", typeof(string));
                        dt.Columns.Add("BalNo", typeof(string));
                        dt.Columns.Add("Specification", typeof(string));
                        dt.Columns.Add("InspectionMethod", typeof(string));
                        dt.Columns.Add("Frequency", typeof(string));
                        dt.Columns.Add("RevID", typeof(int));
                        dt.Columns.Add("RevNo", typeof(string));
                        dt.Columns.Add("RevDate", typeof(DateTime));
                        dt.Columns.Add("DocNo", typeof(string));
                        dt.Columns.Add("UpdatedTS", typeof(DateTime));
                        dt.Columns.Add("Freq_Quantity", typeof(float));
                        dt.Columns.Add("DataType", typeof(string));
                        dt.Columns.Add("SortOrder", typeof(int));
                        int startrow = 2;
                        int lastRow = GetLastUsedRow(wsData);
                        for (var rownum = startrow; rownum <= lastRow; rownum++)
                        {
                            string characteristicID = wsData.Cells[rownum, 1].Value != null ? wsData.Cells[rownum, 1].Value.ToString() : "";
                            string Description = wsData.Cells[rownum, 2].Value != null ? wsData.Cells[rownum, 2].Value.ToString() : "";
                            string BalNo = wsData.Cells[rownum, 3].Value != null ? wsData.Cells[rownum, 3].Value.ToString() : "";
                            string Specification = wsData.Cells[rownum, 4].Value != null ? wsData.Cells[rownum, 4].Value.ToString() : "";
                            string InspectionMethod = wsData.Cells[rownum, 5].Value != null ? wsData.Cells[rownum, 5].Value.ToString() : "";
                            string DataType = wsData.Cells[rownum, 6].Value != null ? wsData.Cells[rownum, 6].Value.ToString() : "";
                            string Frequency = wsData.Cells[rownum, 7].Value != null ? wsData.Cells[rownum, 7].Value.ToString() : "";
                            float FreqQty = wsData.Cells[rownum, 8].Value != null ? Convert.ToSingle(wsData.Cells[rownum, 8].Value.ToString()) : 1;
                            int SortOrder = wsData.Cells[rownum, 9].Value != null ? Convert.ToInt32(wsData.Cells[rownum, 9].Value.ToString()) : 1;
                            var dtRow = dt.NewRow();
                            dtRow[0] = "";
                            dtRow[1] = ddlComponent.SelectedValue.ToString();
                            dtRow[2] = ddlOperation.SelectedValue.ToString();
                            dtRow[3] = characteristicID;
                            dtRow[4] = Description;
                            dtRow[5] = BalNo;
                            dtRow[6] = Specification;
                            dtRow[7] = InspectionMethod;
                            dtRow[8] = Frequency;
                            dtRow[9] = Convert.ToInt32(lblRevisionID.Text==""?"1": lblRevisionID.Text);
                            dtRow[10] = lblRevisionID.Text==""?"1":lblRevisionID.Text;
                            DateTime Date = Util.GetDateTime(txtRevDate.Text);
                            dtRow[11] = txtRevDate.Text==""?DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"):Convert.ToDateTime(Date).ToString("yyyy-MM-dd HH:mm:ss");
                            dtRow[12] = txtDocNo.Text;
                            dtRow[13] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            dtRow[14] = FreqQty;
                            dtRow[15] = DataType;
                            dtRow[16] = SortOrder;
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
                Filename = "InspectionMasterImportTemp.xlsx";
                string Source = string.Empty;
                Source = HighwayGenerateReports.GetReportPath(Filename);
                string Template = string.Empty;
                Template = "InspectionMasterImportTemp" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "Temp", HighwayGenerateReports.SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("InspectionMasterImportTemp- \n " + Source);
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

        protected void btnCopyConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string sourceComponent = lblCopySourceComponent.Text;
                string destinationcomponent = lbCopyDestComponent.SelectedValue.ToString();
                string sourceOperation = lblCopyOperation.Text;
                string destinationOperation = lbCopyOperation.SelectedValue.ToString();
                string result = DBAccess.CopyInspectionData(sourceComponent, sourceOperation, destinationcomponent, destinationOperation);
                if (result == "")
                {
                    HelperClassGeneric.openModal(this, "copyModal", false);
                    HelperClassGeneric.openInsertErrorModal(this);
                    return;
                }
                else
                {
                    HelperClassGeneric.openSuccessModal(this, "Record copied Successfully.");
                    HelperClassGeneric.clearModal(this);
                }
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlComponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOperation();
            BindRev();
        }

        protected void ddlOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRev();
        }

        protected void lbCopyDestComponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                List<string> list = DBAccess.GetOperations(lbCopyDestComponent.SelectedValue.ToString());
                lbCopyOperation.DataSource = list;
                lbCopyOperation.DataBind();
                HelperClassGeneric.openModal(this, "copyModal", false);
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}