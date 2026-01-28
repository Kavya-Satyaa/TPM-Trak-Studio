using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.VulkanMachineShop.Model;

namespace Web_TPMTrakDashboard.VulkanMachineShop
{
    public partial class VulkanPMMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["NewRevID"] = null;
                BindMachineID();
                BindPMChecksheetData();

            }
        }

        private void BindMachineID()
        {
            try
            {
                List<string> list = new List<string>();
                list = VulkanMSDBAccess.GetMachineID();
                ddlMachineID.DataSource = list;
                ddlMachineID.DataBind();

                txtSourceMachineID.Text = ddlMachineID.SelectedValue;
                txtSourceMachineID.ReadOnly = true;

                lbMultiMachineID.DataSource = list;
                lbMultiMachineID.DataBind();
                ddlMachineID_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindMachineID: {ex.Message}");
            }
        }

        private void BindRevID()
        {
            try
            {
                List<string> list = new List<string>();
                list = VulkanMSDBAccess.GetRevisionID(ddlMachineID.SelectedValue, ddlFrequency.SelectedValue);
                if (list.Count > 0)
                {
                    txtRevNo.Visible = false;
                    ddlRevNo.Visible = true;
                    btnCreateRevisionNo.Visible = true;
                    btnNew.Visible = true;
                    ddlRevNo.DataSource = list;
                    ddlRevNo.DataBind();
                    if (Session["NewRevID"] != null)
                        ddlRevNo.SelectedValue = Session["NewRevID"].ToString();
                }
                else
                {
                    txtRevNo.Visible = true;
                    btnCreateRevisionNo.Visible = false;
                    ddlRevNo.Visible = false;
                    btnNew.Visible = false;
                }

                string DocNo = "", RevDate = "";
                DocNo = VulkanMSDBAccess.GetPMDocDetails(ddlMachineID.SelectedValue, ddlFrequency.SelectedValue, out RevDate);
                txtDocNo.Text = DocNo;
                txtRevDate.Text = string.IsNullOrEmpty(RevDate) ? "" : Util.GetDateTime(RevDate).ToString("dd-MM-yyyy");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindRevID: {ex.Message}");
            }
        }

        private void BindPMChecksheetData()
        {
            List<PMMasterEntity> list = new List<PMMasterEntity>();
            try
            {
                list = VulkanMSDBAccess.GetPMMasterData(ddlMachineID.SelectedValue, ddlRevNo.SelectedValue, ddlFrequency.SelectedValue);
                List<string> RespoList = new List<string>();
                if (Session["RespoList"] == null)
                    RespoList = VulkanMSDBAccess.GetResponsibilityValues();
                else
                    RespoList = Session["RespoList"] as List<string>;
                if (list.Count > 0)
                {
                    list.ForEach(x =>
                    {
                        x.dropdownvalue_Responsibility = RespoList;
                    });
                }
                else
                {
                    hdnNew.Value = "New";
                }
                lvChecksheetData.DataSource = list;
                lvChecksheetData.DataBind();


                var ddlFreq = (lvChecksheetData.InsertItem.FindControl("ddlFrequency") as DropDownList);
                if (ddlFreq != null)
                    ddlFreq.SelectedValue = string.IsNullOrEmpty(ddlFrequency.SelectedValue) ? "Daily" : ddlFrequency.SelectedValue;

                var ddlResp = (lvChecksheetData.InsertItem.FindControl("ddlResponsibility") as DropDownList);
                if (ddlResp != null)
                    ddlResp.SelectedValue = ddlFreq.SelectedValue.ToLower().Equals("daily", StringComparison.OrdinalIgnoreCase) ? "operator" : "maintenance";

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindGrid: {ex.Message}");
            }
        }

        protected void ddlMachineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string ApprovedBy = "", VerifiedBy = "";
                txtPreparedby.Text = VulkanMSDBAccess.GetHeaderDataPM(ddlMachineID.SelectedValue, out ApprovedBy, out VerifiedBy);
                txtApprovedBy.Text = ApprovedBy;
                txtVerifiedBy.Text = VerifiedBy;
                txtSourceMachineID.Text = ddlMachineID.SelectedValue;
                BindRevID();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"ddlMachineID_SelectedIndexChanged: {ex.Message}");
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindPMChecksheetData();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            PMMasterEntity entity = new PMMasterEntity();
            try
            {
                string MachineID = "", RevNo = "", DocNo = "", RevDate="";
                int result = 0;
                MachineID = ddlMachineID.SelectedValue;
                if (txtRevNo.Visible)
                {
                    if (string.IsNullOrEmpty(txtRevNo.Text))
                    {
                        HelperClassGeneric.openWarningModal(this, "Rev ID Cannot be Empty.!");
                        return;
                    }
                    RevNo = txtRevNo.Text.Trim();
                }
                else
                    RevNo = ddlRevNo.SelectedValue;
                DocNo = txtDocNo.Text.Trim();

                if (hdnHeaderUpdate.Value.Equals("Update", StringComparison.OrdinalIgnoreCase))
                {
                    result += VulkanMSDBAccess.SavePMHeaderDetails(ddlMachineID.SelectedValue, ddlRevNo.SelectedValue, txtPreparedby.Text, txtApprovedBy.Text, txtVerifiedBy.Text);
                    hdnHeaderUpdate.Value = "";
                }

                if (hdnNew.Value.Equals("new", StringComparison.OrdinalIgnoreCase))
                {
                    entity = new PMMasterEntity();
                    entity.CheckpointID = (lvChecksheetData.InsertItem.FindControl("txtCheckpointID") as TextBox).Text.Trim();
                    entity.CheckpointDesc = (lvChecksheetData.InsertItem.FindControl("txtCheckpointDesc") as TextBox).Text.Trim();
                    entity.Particular = (lvChecksheetData.InsertItem.FindControl("txtParticular") as TextBox).Text.Trim();
                    entity.Frequency = /*(lvChecksheetData.InsertItem.FindControl("ddlFrequency") as DropDownList).SelectedValue.Trim().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : */(lvChecksheetData.InsertItem.FindControl("ddlFrequency") as DropDownList).SelectedValue.Trim();
                    entity.ControlMethod = (lvChecksheetData.InsertItem.FindControl("txtControlMethod") as TextBox).Text.Trim();
                    entity.Responsibility = (lvChecksheetData.InsertItem.FindControl("ddlResponsibility") as DropDownList).SelectedValue.Trim();
                    if (string.IsNullOrEmpty(entity.CheckpointID))
                    {
                        HelperClassGeneric.openWarningModal(this, "Checkpoint ID Cannot be Empty");
                        return;
                    }
                    result += VulkanMSDBAccess.SavePMMasterData(entity, MachineID, RevNo, DocNo, RevDate);
                }

                foreach (ListViewDataItem item in lvChecksheetData.Items)
                {
                    if ((item.FindControl("hdnUpdate") as HiddenField).Value.Equals("update", StringComparison.OrdinalIgnoreCase))
                    {
                        entity = new PMMasterEntity();
                        entity.CheckpointID = (item.FindControl("lblCheckpointID") as Label).Text.Trim();
                        entity.CheckpointDesc = (item.FindControl("txtCheckpointDesc") as TextBox).Text.Trim();
                        entity.Particular = (item.FindControl("txtParticular") as TextBox).Text.Trim();
                        entity.Frequency = /*(item.FindControl("ddlFrequency") as DropDownList).SelectedValue.Trim().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : */(item.FindControl("ddlFrequency") as DropDownList).SelectedValue.Trim();
                        entity.ControlMethod = (item.FindControl("txtControlMethod") as TextBox).Text.Trim();
                        entity.Responsibility = (item.FindControl("ddlResponsibility") as DropDownList).SelectedValue.Trim();

                        result += VulkanMSDBAccess.SavePMMasterData(entity, MachineID, RevNo, DocNo, RevDate);
                    }
                }

                if (result > 0)
                {
                    HelperClassGeneric.openSuccessModal(this, "Saved Successfully.");
                    BindRevID();
                    BindPMChecksheetData();
                }
                else
                    HelperClassGeneric.openErrorModal(this, "Try Again!");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int result = 0, itemforDeletion = 0;
            try
            {
                foreach (ListViewDataItem item in lvChecksheetData.Items)
                {
                    if ((item.FindControl("chkDelete") as CheckBox).Checked)
                    {
                        itemforDeletion++;
                        string frequency = (item.FindControl("ddlFrequency") as DropDownList).SelectedValue.ToString();

                        result += VulkanMSDBAccess.DeletePMMasterData(ddlMachineID.SelectedValue, ddlRevNo.SelectedValue, (item.FindControl("lblCheckpointID") as Label).Text.Trim(), frequency);
                    }
                }
                if (result > 0)
                {
                    HelperClassGeneric.openSuccessModal(this, "Deleted Successfully.");
                    BindPMChecksheetData();
                }
                else if (itemforDeletion > 0)
                    HelperClassGeneric.openWarningToastrModal(this, "Select Rows for Deletion.");
                else
                    HelperClassGeneric.openWarningToastrModal(this, "Error! Try again.");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnDelete_Click: " + ex.Message);
            }
        }

        protected void lvChecksheetData_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    var freqddl = (e.Item.FindControl("ddlFrequency") as DropDownList);
                    freqddl.SelectedValue = (e.Item.FindControl("hdnFrequency") as HiddenField).Value;

                    var respddl = (e.Item.FindControl("ddlResponsibility") as DropDownList);
                    respddl.SelectedValue = freqddl.SelectedValue.ToLower().Equals("daily", StringComparison.OrdinalIgnoreCase) ? "operator" : "maintenance";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lvChecksheetData_ItemDataBound: " + ex.Message);
            }
        }

        protected void btnSaveRevisionNo_Click(object sender, EventArgs e)
        {
            int res = 0;
            try
            {
                if (ddlRevNo.Items.FindByValue(txtRevNoNew.Text.Trim()) != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "openRevPopup", "OpenRevNoPopUp();", true);
                    HelperClassGeneric.openWarningModal(this, "Rev No already exists!");
                    return;
                }
                res = VulkanMSDBAccess.SaveNewRevisionNo(ddlMachineID.SelectedValue, txtRevNoNew.Text.Trim());

                if (res > 0)
                {
                    HelperClassGeneric.openSuccessModal(this, "Created Successfully");
                    Session["NewRevID"] = txtRevNoNew.Text.Trim();
                    BindRevID();
                }
                else
                    HelperClassGeneric.openErrorModal(this, "Could't be created. Try Again!");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnSaveRevisionNo_Click:" + ex.Message);
            }
        }

        protected void btnCopyModal_Click(object sender, EventArgs e)
        {
            int res = 0;
            try
            {
                string DestMachines = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMultiMachineID);
                if (string.IsNullOrEmpty(DestMachines))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenCopy", "openCopyModal();", true);
                    HelperClassGeneric.openWarningModal(this, "Select Atleast One Dest. Machine");
                    return;
                }
                res = VulkanMSDBAccess.CopyRevData(DestMachines, ddlMachineID.SelectedValue);
                if (res > 0)
                    HelperClassGeneric.openSuccessModal(this, "Copied Successfully");
                else
                    HelperClassGeneric.openErrorModal(this, "Could not be Copied. Try Again!");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"btnCopyModal_Click: {ex.Message}");
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            Logger.WriteDebugLog("Import click");
            try
            {
                DataTable dt = new DataTable();
                PMMasterEntity data = new PMMasterEntity();
                string errorMsg = "";
                if (fuImport.HasFile)
                {
                    Logger.WriteDebugLog("Directorycheck");
                    string fileName = fuImport.FileName.ToString();
                    if (!Directory.Exists(Server.MapPath("PMMasterEntryImportFiles")))
                    {
                        Directory.CreateDirectory(Server.MapPath("PMMasterEntryImportFiles"));
                        Logger.WriteDebugLog("Created Directory");
                    }
                    else
                    {
                        Logger.WriteDebugLog("Directory exists");
                    }
                    fuImport.SaveAs($"{Server.MapPath("PMMasterEntryImportFiles")}\\{fileName}");
                    var vv = $"{Server.MapPath("PMMasterEntryImportFiles")}\\{fileName}";
                    Logger.WriteDebugLog(vv);
                    using (var excel = new ExcelPackage())
                    {
                        using (var stream = File.OpenRead($"{Server.MapPath("PMMasterEntryImportFiles")}\\{fileName}"))
                            excel.Load(stream);
                        var wb = excel.Workbook;
                        if (wb != null)
                        {
                            try
                            {
                                var ws = wb.Worksheets[1];
                                dt.Clear();
                                dt.Columns.Add("MachineID", typeof(string));
                                dt.Columns.Add("CheckpointID", typeof(string));
                                dt.Columns.Add("Particular", typeof(string));
                                dt.Columns.Add("CheckpointItem", typeof(string));
                                dt.Columns.Add("Frequency", typeof(string));
                                dt.Columns.Add("ControlMethod", typeof(string));
                                dt.Columns.Add("Responsibility", typeof(string));
                                dt.Columns.Add("RevID", typeof(string));
                                dt.Columns.Add("RevNo", typeof(string));
                                dt.Columns.Add("RevDate", typeof(string));
                                dt.Columns.Add("DocNo", typeof(string));
                                string RevID = string.Empty, DocNo = string.Empty, RevNo = string.Empty, RevDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                int rowStart = 4, colStart = 1, LastUsedRow = GetLastUsedRow(ws); ;
                                for (int i = rowStart; i <= LastUsedRow; i++)
                                {
                                    colStart = 1;
                                    data = new PMMasterEntity();

                                    if (string.IsNullOrEmpty(ws.Cells[i, colStart].Value.ToString().Trim()))
                                    {
                                        if (string.IsNullOrEmpty(errorMsg))
                                            errorMsg = "Error Inserting data at Rows:" + i.ToString();
                                        else
                                            errorMsg += ", " + i.ToString();
                                        continue;
                                    }
                                    else
                                        data.MachineID = ws.Cells[i, colStart].Value.ToString().Trim();
                                    colStart++;

                                    if (string.IsNullOrEmpty(ws.Cells[i, colStart].Value.ToString().Trim()))
                                    {
                                        if (string.IsNullOrEmpty(errorMsg))
                                            errorMsg = "Error Inserting data at Rows:" + i.ToString();
                                        else
                                            errorMsg += ", " + i.ToString();
                                        continue;
                                    }
                                    else
                                        data.CheckpointID = ws.Cells[i, colStart].Value.ToString().Trim();
                                    colStart++;
                                    data.Particular = ws.Cells[i, colStart].Value.ToString().Trim();
                                    colStart++;
                                    data.CheckpointDesc = ws.Cells[i, colStart].Value.ToString().Trim();
                                    colStart++;
                                    string Freq = ws.Cells[i, colStart].Value.ToString().Trim();
                                    if (Freq.ToLower() == "daily" || Freq.ToLower() == "weekly" || Freq.ToLower() == "monthly")
                                        data.Frequency = ws.Cells[i, colStart].Value.ToString().Trim();
                                    else
                                    {
                                        if (string.IsNullOrEmpty(errorMsg))
                                            errorMsg = "Error Inserting data at Rows:" + i.ToString();
                                        else
                                            errorMsg += ", " + i.ToString();
                                        continue;
                                    }
                                    colStart++;
                                    data.ControlMethod = ws.Cells[i, colStart].Value.ToString().Trim();
                                    colStart++;
                                    data.Responsibility = ws.Cells[i, colStart].Value.ToString().Trim();
                                    colStart++;
                                    RevNo = RevID = ws.Cells[i, colStart].Value.ToString().Trim();
                                    colStart++;
                                    DocNo = ws.Cells[i, colStart].Value.ToString().Trim();


                                    var tableRow = dt.NewRow();
                                    tableRow["MachineID"] = data.MachineID;
                                    tableRow["CheckpointID"] = data.CheckpointID;
                                    tableRow["Particular"] = data.Particular;
                                    tableRow["CheckpointItem"] = data.CheckpointDesc;
                                    tableRow["Frequency"] = data.Frequency;
                                    tableRow["ControlMethod"] = data.ControlMethod;
                                    tableRow["Responsibility"] = data.Responsibility;
                                    tableRow["RevID"] = RevID;
                                    tableRow["RevNo"] = RevNo;
                                    tableRow["DocNo"] = DocNo;
                                    tableRow["RevDate"] = RevDate;
                                    dt.Rows.Add(tableRow);

                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteErrorLog(ex.Message);
                            }
                            if (dt.Rows.Count > 0)
                            {
                                string res = VulkanMSDBAccess.SaveImportDataToTemp_PMMaster(dt);
                                if (!string.IsNullOrEmpty(res))
                                {
                                    if (!string.IsNullOrEmpty(errorMsg))
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "warningModal", "toasterWarningMsg('" + errorMsg + "','')", true);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success Modal", "showSuccessMsg('Successully Imported.','')", true);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Please Select a file to import");
                }

                BindRevID();
                BindPMChecksheetData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"btnImport_Click: {ex.Message}");
            }
        }

        protected void lnkDownloadTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                string res = VulkanMSGenerateReport.DownloadPMMasterEntryImportFile();
                if (res.Equals("Downloaded", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Success Modal", "showSuccessMsg('Successully Downloaded.','')", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Warning Modal", "toasterWarningMsg('Error.! Try Again.','')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"lnkDownloadTemplate_Click: {ex.Message}");
            }
        }

        private int GetLastUsedRow(ExcelWorksheet worksheet)
        {
            var row = worksheet.Dimension.End.Row;
            while (row >= 1)
            {
                var range = worksheet.Cells[row, 1, row, worksheet.Dimension.End.Column];
                if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                {
                    break;
                }
                row--;
            }
            return row;
        }

        protected void ddlFrequency_SelectedIndexChanged1(object sender, EventArgs e)
        {
            try
            {
                var ddlFreq = (sender as DropDownList);
                var ddlResp = ((sender as DropDownList).Parent.FindControl("ddlResponsibility") as DropDownList);
                if (ddlResp != null)
                {
                    ddlResp.SelectedValue = ddlFreq.SelectedValue.ToLower().Equals("daily", StringComparison.OrdinalIgnoreCase) ? "operator" : "maintenance";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"ddlFrequency_SelectedIndexChanged1: {ex.Message}");
            }
        }
    }
}