using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Vulkan.Models;

namespace Web_TPMTrakDashboard.Vulkan
{
    public partial class PM_Master : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["NewRevNo"] = null;
                BindPlantID();
                BindGrid();
            }
        }
        private void BindPlantID()
        {
            try
            {
                List<string> list = DataBaseAccessVulkan.GetPlantID();

                ddlPlantID.DataSource = list;
                ddlPlantID.DataBind();

                ddlPlantID_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindMachines()
        {
            try
            {
                List<string> list = new List<string>();
                list = DataBaseAccessVulkan.GetMachineIDs(ddlPlantID.SelectedValue);
                ddlMachineID.DataSource = list;
                ddlMachineID.DataBind();
                lbMultiMachineID.DataSource = list;
                lbMultiMachineID.DataBind();

                ddlMachineID_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindRevisionNo()
        {
            try
            {
                List<string> list = new List<string>();
                list = DataBaseAccessVulkan.GetRevisionNo_PM(ddlPlantID.SelectedValue, ddlMachineID.SelectedValue, ddlFrequeny.SelectedValue);
                if (list.Count > 0)
                {
                    ddlRevNo.DataSource = list;
                    ddlRevNo.DataBind();
                    txtRevNo.Visible = false;
                    ddlRevNo.Visible = true;
                    btnCreateRevisionNo.Visible = true;
                    ddlRevNo.SelectedValue = ddlRevNo.Items[ddlRevNo.Items.Count - 1].Value;
                }
                else
                {
                    ddlRevNo.Visible = false;
                    txtRevNo.Visible = true;
                    btnCreateRevisionNo.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void BindGrid()
        {
            string DocNo, IssueDate, RevDate;
            try
            {
                Session["MethodList"] = DataBaseAccessVulkan.GetCLImagesList("Method");
                Session["InstrumentList"] = DataBaseAccessVulkan.GetCLImagesList("Instrument");
                txtSrcMachineID.Text = ddlMachineID.SelectedValue;
                List<CLVulkanEntity> list = new List<CLVulkanEntity>();
                string RevNo = string.Empty;
                if (txtRevNo.Visible)
                    RevNo = txtRevNo.Text.Trim();
                else
                    RevNo = ddlRevNo.SelectedValue;
                list = DataBaseAccessVulkan.GetCLGridData_PM(ddlPlantID.SelectedValue, ddlMachineID.SelectedValue, RevNo, ddlFrequeny.SelectedValue, out DocNo, out IssueDate, out RevDate);
                txtDocNo.Text = DocNo;
                txtIssueDate.Text = Util.GetDateTime(IssueDate).ToString("dd-MM-yyyy");
                txtRevisionDate.Text = Util.GetDateTime(RevDate).ToString("dd-MM-yyyy");

                lvCLGrid.DataSource = list;
                lvCLGrid.DataBind();

                var insertItemTemplate = lvCLGrid.InsertItem;
                if (insertItemTemplate != null)
                {
                    var dropdownMethod = insertItemTemplate.FindControl("lbMethod") as ListBox;
                    if (dropdownMethod != null && Session["MethodList"] != null)
                    {
                        dropdownMethod.DataSource = Session["MethodList"] as List<MetInsEntity>;
                        dropdownMethod.DataTextField = "imgName";
                        dropdownMethod.DataValueField = "RefID";
                        dropdownMethod.DataBind();
                    }

                    var dropDownInstrument = insertItemTemplate.FindControl("lbInstrument") as ListBox;
                    if (dropDownInstrument != null && Session["InstrumentList"] != null)
                    {
                        dropDownInstrument.DataSource = Session["InstrumentList"] as List<MetInsEntity>;
                        dropDownInstrument.DataTextField = "imgName";
                        dropDownInstrument.DataValueField = "RefID";
                        dropDownInstrument.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlMachineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFrequeny_SelectedIndexChanged(null, null);
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int isSaved = 0;
            int rowstoUpdate = 0, UpdatedRow = 0;
            string DuplicateMsg = "";
            try
            {
                string RevisionNo = string.Empty;
                if (txtRevNo.Visible)
                {
                    RevisionNo = txtRevNo.Text.Trim();
                    btnCreateRevisionNo.Visible = true;
                    if (txtRevNo.Text == "")
                    {
                        HelperClassGeneric.openWarningModal(this, "Revision Number can not be Empty.");
                        return;
                    }
                    if (txtDocNo.Text == "")
                    {
                        HelperClassGeneric.openWarningModal(this, "Document Number can not be Empty.");
                        return;
                    }
                }
                else
                {
                    RevisionNo = ddlRevNo.SelectedValue;
                }
                string MachineID = ddlMachineID.SelectedValue;
                string PlantID = ddlPlantID.SelectedValue;
                string Frequency = ddlFrequeny.SelectedValue;
                CLVulkanEntity entity = new CLVulkanEntity();
                if (hdnNew.Value.Equals("New", StringComparison.OrdinalIgnoreCase))
                {
                    hdnNew.Value = "";
                    var item = lvCLGrid.InsertItem;
                    entity = new CLVulkanEntity();
                    entity.SlNo = (item.FindControl("txtSlNo") as TextBox).Text.Trim();
                    entity.CheckPoint = (item.FindControl("txtCheckPoint") as TextBox).Text.Trim();
                    entity.Requirement = (item.FindControl("txtRequirement") as TextBox).Text.Trim();
                    entity.Method = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(item.FindControl("lbMethod") as ListBox);
                    entity.Instrument = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(item.FindControl("lbInstrument") as ListBox);
                    entity.Observation = (item.FindControl("txtObservation") as TextBox).Text.Trim();
                    if ((item.FindControl("fuReferenceImage") as FileUpload).HasFile)
                    {
                        entity.ReferenceImageName = (item.FindControl("fuReferenceImage") as FileUpload).FileName.ToString();

                        FileUpload fupload = (item.FindControl("fuReferenceImage") as FileUpload);
                        var file = fupload.PostedFile;
                        byte[] imgBytes = new byte[file.ContentLength];
                        Stream fs = file.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        imgBytes = br.ReadBytes((Int32)fs.Length);
                        entity.ReferenceImageData = imgBytes;
                    }
                    entity.IsImageRequired = (item.FindControl("chkImageRequired") as CheckBox).Checked;
                    if (entity.SlNo == "")
                    {
                        HelperClassGeneric.openWarningModal(this, "Checkpoint ID Can not be Empty");
                        return;
                    }
                    isSaved = DataBaseAccessVulkan.SaveCLData_PM(PlantID, MachineID, RevisionNo, Frequency, txtDocNo.Text, txtIssueDate.ToString(), txtRevisionDate.ToString(), entity, "New", out DuplicateMsg);
                    if (!string.IsNullOrEmpty(DuplicateMsg))
                    {
                        HelperClassGeneric.openWarningModal(this, DuplicateMsg);
                        return;
                    }
                    if (isSaved > 0)
                        HelperClassGeneric.openInsertSuccessModal(this);
                    else
                        HelperClassGeneric.openInsertErrorModal(this);
                }
                else
                {
                    foreach (ListViewDataItem item in lvCLGrid.Items)
                    {
                        if ((item.FindControl("hdnUpdate") as HiddenField).Value.Trim().Equals("update", StringComparison.OrdinalIgnoreCase))
                        {
                            rowstoUpdate++;
                            entity = new CLVulkanEntity();
                            entity.SlNo = (item.FindControl("lblSlNo") as Label).Text.Trim();
                            entity.CheckPoint = (item.FindControl("lblCheckPoint") as TextBox).Text.Trim();
                            entity.Requirement = (item.FindControl("txtRequirement") as TextBox).Text.Trim();
                            entity.Method = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(item.FindControl("lbMethod") as ListBox);
                            entity.Instrument = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(item.FindControl("lbInstrument") as ListBox);
                            entity.Observation = (item.FindControl("txtObservation") as TextBox).Text.Trim();
                            if ((item.FindControl("fuReferenceImage") as FileUpload).HasFile)
                            {
                                entity.ReferenceImageName = (item.FindControl("fuReferenceImage") as FileUpload).FileName.ToString();

                                FileUpload fupload = (item.FindControl("fuReferenceImage") as FileUpload);
                                var file = fupload.PostedFile;
                                byte[] imgBytes = new byte[file.ContentLength];
                                Stream fs = file.InputStream;
                                BinaryReader br = new BinaryReader(fs);
                                imgBytes = br.ReadBytes((Int32)fs.Length);
                                entity.ReferenceImageData = imgBytes;
                            }
                            else
                                entity.ReferenceImageName = (item.FindControl("lblReferenceImage") as LinkButton).Text.Trim();

                            entity.IsImageRequired = (item.FindControl("chkImageRequired") as CheckBox).Checked;
                            isSaved += DataBaseAccessVulkan.SaveCLData_PM(PlantID, MachineID, RevisionNo, Frequency, txtDocNo.Text, txtIssueDate.ToString(), txtRevisionDate.ToString(), entity, "Update", out DuplicateMsg);
                        }
                    }
                    if (rowstoUpdate == 0)
                        HelperClassGeneric.openWarningToastrModal(this, "No Rows are Updated.");
                    else if (isSaved>0)
                        HelperClassGeneric.openUpdateSuccessModal(this);
                    else
                        HelperClassGeneric.openErrorModal(this, "Update Failed. Try Again!");
                }

                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int RowsTobeDeleted = 0, DeletedRows = 0;
            try
            {
                foreach (ListViewDataItem item in lvCLGrid.Items)
                {
                    if ((item.FindControl("chkDelete") as CheckBox).Checked)
                    {
                        RowsTobeDeleted++;
                        string checkPointID = (item.FindControl("lblSlNo") as Label).Text.Trim();
                        DeletedRows += DataBaseAccessVulkan.DeleteCLValue_PM(checkPointID, ddlMachineID.SelectedValue, ddlRevNo.SelectedValue, ddlPlantID.SelectedValue, ddlFrequeny.SelectedValue);
                    }
                }
                if (RowsTobeDeleted == 0)
                    HelperClassGeneric.openWarningToastrModal(this, "No Rows are selected for deletion");
                else if (RowsTobeDeleted == DeletedRows)
                    HelperClassGeneric.openDeleteSuccessModal(this);
                else
                    HelperClassGeneric.openErrorModal(this, "ERROR! Deletion Failed. Try Again");
                BindRevisionNo();
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachines();
        }

        protected void lvDailyCLGrid_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    var dropdownMethod = (e.Item.FindControl("lbMethod") as ListBox);
                    if (Session["MethodList"] != null && dropdownMethod != null)
                    {
                        dropdownMethod.DataSource = Session["MethodList"] as List<MetInsEntity>;
                        dropdownMethod.DataTextField = "imgName";
                        dropdownMethod.DataValueField = "RefID";
                        dropdownMethod.DataBind();

                        List<string> Methods = ((e.Item.FindControl("hdnMethod") as HiddenField).Value).ToString().Split(',').ToList();
                        foreach (ListItem item in dropdownMethod.Items)
                        {
                            if (Methods.Contains(item.Value))
                            {
                                item.Selected = true;
                            }
                        }
                    }
                    var dropdownInstrument = (e.Item.FindControl("lbInstrument") as ListBox);
                    if (Session["InstrumentList"] != null && dropdownInstrument != null)
                    {
                        dropdownInstrument.DataSource = Session["InstrumentList"] as List<MetInsEntity>;
                        dropdownInstrument.DataTextField = "imgName";
                        dropdownInstrument.DataValueField = "RefID";
                        dropdownInstrument.DataBind();

                        List<string> Methods = ((e.Item.FindControl("hdnInstrment") as HiddenField).Value).ToString().Split(',').ToList();
                        foreach (ListItem item in dropdownInstrument.Items)
                        {
                            if (Methods.Contains(item.Value))
                            {
                                item.Selected = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnCopyModal_Click(object sender, EventArgs e)
        {
            try
            {
                string DestMachineID = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMultiMachineID);
                if (string.IsNullOrEmpty(DestMachineID))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "çopymodal", "  $('#CLCopyModal').modal('show');", true);
                    HelperClassGeneric.openWarningToastrModal(this, "kindly, Select atleast one dest. Machine to Copy.");
                    return;
                }
                string Res = DataBaseAccessVulkan.CopyCLData_PM(txtSrcMachineID.Text.Trim(), DestMachineID);
                if (Res.Equals("Inserted", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openSuccessModal(this, "Copied Data Successfully");
                    BindGrid();
                }
                else
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Copying Data Failed.");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "CopyModal", "openCopyModal();", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnSaveRevisionNo_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlRevNo.Items.FindByValue(txtRevNoNew.Text) != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "revnumbermodal", " $('#NewRevNoModal').modal('show');", true);
                    HelperClassGeneric.openWarningToastrModal(this, "Duplicate Rev Number Found.");
                    return;
                }
                Session["NewRevNo"] = txtRevNoNew.Text.Trim();
                DataBaseAccessVulkan.SaveRevisionNo_PM(ddlPlantID.SelectedValue, ddlMachineID.SelectedValue, ddlFrequeny.SelectedValue, txtRevNoNew.Text.Trim());
                BindRevisionNo();
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void lblReferenceImage_Click(object sender, EventArgs e)
        {
            try
            {
                //int index = ((sender as LinkButton).NamingContainer as ListViewDataItem).DataItemIndex;

                //ListViewDataItem item = lvCLGrid.Items[index];

                //item.FindControl("")
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void ddlFrequeny_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRevisionNo();
        }
    }
}