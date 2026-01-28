using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Vulkan.Models;

namespace Web_TPMTrakDashboard.Vulkan
{
    public partial class DailyCheckListMaster : System.Web.UI.Page
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
                list = DataBaseAccessVulkan.GetMachineIDs(ddlPlantID.SelectedValue.Trim());
                ddlMachineID.DataSource = list;
                ddlMachineID.DataBind();

                txtSourceMachineID.Text = ddlMachineID.SelectedValue;
                lbMultiMachineID.DataSource = list.Except(new[] { txtSourceMachineID.Text });
                lbMultiMachineID.DataBind();
                //BindDestinationMachineIDForCopy();
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
                list = DataBaseAccessVulkan.GetRevisionNo(ddlMachineID.SelectedValue);

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
            string DocNo, RevDate, IssueDate;
            try
            {
                Session["MethodList"] = DataBaseAccessVulkan.GetCLImagesList("Method");
                Session["InstrumentList"] = DataBaseAccessVulkan.GetCLImagesList("Instrument");

                List<CLVulkanEntity> list = new List<CLVulkanEntity>();
                string RevNo = string.Empty;
                if (txtRevNo.Visible)
                    RevNo = txtRevNo.Text.Trim();
                else
                    RevNo = ddlRevNo.SelectedValue;
                list = DataBaseAccessVulkan.GetCLGridData(ddlPlantID.SelectedValue, ddlMachineID.SelectedValue, RevNo, out DocNo, out RevDate, out IssueDate);

                txtDocNo.Text = DocNo;
                txtIssueDate.Text = IssueDate;
                txtRevDate.Text = RevDate;

                lvDailyCLGrid.DataSource = list;
                lvDailyCLGrid.DataBind();
                if (!(list.Count > 0))
                    hdnNew.Value = "New";
                else
                    hdnNew.Value = "";

                var insertItemTemplate = lvDailyCLGrid.InsertItem;
                if (insertItemTemplate != null)
                {
                    var dropdownMethod = insertItemTemplate.FindControl("lbMethod") as ListBox;
                    if (Session["MethodList"] != null && dropdownMethod != null)
                    {
                        dropdownMethod.DataSource = Session["MethodList"] as List<MetInsEntity>;
                        dropdownMethod.DataTextField = "imgName";
                        dropdownMethod.DataValueField = "RefID";
                        dropdownMethod.DataBind();
                    }
                    var dropDownInstrument = insertItemTemplate.FindControl("lbInstrument") as ListBox;
                    if (Session["InstrumentList"] != null && dropDownInstrument != null)
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

        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindMachines();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void ddlMachineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtSourceMachineID.Text = ddlMachineID.SelectedValue;
                var list = ddlMachineID.Items.Cast<ListItem>().Select(x => x.Text).ToList();

                if (list != null)
                {
                    lbMultiMachineID.DataSource = list.Except(new[] { txtSourceMachineID.Text }).ToList();
                    lbMultiMachineID.DataBind();
                }
                BindRevisionNo();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("ddlMachineID_SelectedIndexChanged= " + ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindGrid();
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

                    var dropdownCheklistType = (e.Item.FindControl("ddlChecklistType") as DropDownList);
                    if (dropdownCheklistType != null && !string.IsNullOrEmpty((e.Item.FindControl("hdnChecklistType") as HiddenField).Value))
                    {
                        dropdownCheklistType.SelectedValue = (e.Item.FindControl("hdnChecklistType") as HiddenField).Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string CheckPointDesc, Requirement, Method, Instrument, ActionPlan, CheckPointid, AMType;
            int rowstoUpdate = 0, UpdatedRow = 0;
            string DuplicateID = "";
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
                }
                else
                    RevisionNo = ddlRevNo.SelectedValue;

                string MachineID = ddlMachineID.SelectedValue;
                string PlantID = ddlPlantID.SelectedValue;
                if (hdnNew.Value.Equals("New", StringComparison.OrdinalIgnoreCase))
                {
                    hdnNew.Value = "";
                    var item = lvDailyCLGrid.InsertItem;

                    CheckPointid = (item.FindControl("txtSlNo") as TextBox).Text.Trim();
                    CheckPointDesc = (item.FindControl("txtCheckPoint") as TextBox).Text.Trim();
                    Requirement = (item.FindControl("txtRequirement") as TextBox).Text.Trim();
                    Method = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(item.FindControl("lbMethod") as ListBox);
                    Instrument = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(item.FindControl("lbInstrument") as ListBox);
                    ActionPlan = (item.FindControl("txtActionPlan") as TextBox).Text.Trim();
                    AMType = (item.FindControl("ddlChecklistType") as DropDownList).SelectedValue.Trim();
                    UpdatedRow = DataBaseAccessVulkan.SaveCLData(PlantID, MachineID, RevisionNo, CheckPointid, CheckPointDesc, Requirement, Method, Instrument, ActionPlan, AMType, txtIssueDate.ToString(), txtRevDate.ToString(), txtDocNo.Text, "New", out DuplicateID);
                    if (!string.IsNullOrEmpty(DuplicateID))
                    {
                        HelperClassGeneric.openWarningModal(this, DuplicateID);
                        return;
                    }
                    if (UpdatedRow > 0)
                        HelperClassGeneric.openInsertSuccessModal(this);
                    else
                        HelperClassGeneric.openInsertErrorModal(this);
                }
                else
                {
                    foreach (ListViewDataItem item in lvDailyCLGrid.Items)
                    {
                        if ((item.FindControl("hdnUpdate") as HiddenField).Value.Trim().Equals("update", StringComparison.OrdinalIgnoreCase))
                        {
                            rowstoUpdate++;
                            CheckPointid = (item.FindControl("lblSlNo") as Label).Text.Trim();
                            CheckPointDesc = (item.FindControl("lblCheckPoint") as TextBox).Text.Trim();
                            Requirement = (item.FindControl("txtRequirement") as TextBox).Text.Trim();
                            Method = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(item.FindControl("lbMethod") as ListBox);
                            Instrument = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(item.FindControl("lbInstrument") as ListBox);
                            ActionPlan = (item.FindControl("txtActionPlan") as TextBox).Text.Trim();
                            AMType = (item.FindControl("ddlChecklistType") as DropDownList).SelectedValue.Trim();
                            UpdatedRow += DataBaseAccessVulkan.SaveCLData(PlantID, MachineID, RevisionNo, CheckPointid, CheckPointDesc, Requirement, Method, Instrument, ActionPlan, AMType, txtIssueDate.ToString(), txtRevDate.ToString(), txtDocNo.Text, "Update", out DuplicateID);
                        }
                    }
                    if (rowstoUpdate == 0)
                        HelperClassGeneric.openWarningToastrModal(this, "No rows are Updated.");
                    else if (rowstoUpdate == UpdatedRow)
                        HelperClassGeneric.openSuccessModal(this, "Updated Successfully.");
                    else
                        HelperClassGeneric.openWarningToastrModal(this, "Save Failed. Try Again!");

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
            string MachineID = string.Empty, RevNo = string.Empty;
            int isDeleted = 0, rowstodelete = 0;
            try
            {
                foreach (ListViewDataItem item in lvDailyCLGrid.Items)
                {
                    if ((item.FindControl("chkDelete") as CheckBox).Checked)
                    {
                        rowstodelete++;
                        string CheckPointID = (item.FindControl("lblSlNo") as Label).Text.Trim();
                        MachineID = ddlMachineID.SelectedValue;
                        RevNo = ddlRevNo.SelectedValue;
                        isDeleted = DataBaseAccessVulkan.DeleteCLValue(CheckPointID, MachineID, RevNo, ddlPlantID.SelectedValue);
                    }
                }
                if (isDeleted == rowstodelete)
                    HelperClassGeneric.openDeleteSuccessModal(this);
                else
                    HelperClassGeneric.openWarningToastrModal(this, "Deletion failed. Try Again!");

                BindRevisionNo();
                BindGrid();
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
                if (ddlRevNo.Items.FindByValue(txtRevNoNew.Text.Trim()) != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "openCopyModal", "$('#NewRevNoModal').modal('show');", true);
                    HelperClassGeneric.openWarningToastrModal(this, "Duplicate Revision Number found.");
                }
                else
                {
                    DataBaseAccessVulkan.SaveRevisionNo(ddlPlantID.SelectedValue, ddlMachineID.SelectedValue, "", txtRevNoNew.Text.Trim());
                    BindRevisionNo();
                }
                BindGrid();
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
                string SrcMachineID = txtSourceMachineID.Text;
                string SrcRevisionNo = ddlRevNo.SelectedValue;

                string DestMachineID = string.Empty;
                DestMachineID = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMultiMachineID);
                if (string.IsNullOrEmpty(DestMachineID))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "CopyModal", "$('#CLCopyModal').modal('show');", true);
                    HelperClassGeneric.openWarningToastrModal(this, "Select atleast one Destination Machine.");
                    return;
                }
                string Res = DataBaseAccessVulkan.CopyCLData(SrcMachineID, DestMachineID);

                if (Res.Equals("Inserted", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openSuccessModal(this, "Data Copied Successfully");
                    BindGrid();
                }
                else
                {
                    HelperClassGeneric.openWarningModal(this, "Data Copying Failed.");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "CopyModal", "$('#CLCopyModal').modal('show');", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}