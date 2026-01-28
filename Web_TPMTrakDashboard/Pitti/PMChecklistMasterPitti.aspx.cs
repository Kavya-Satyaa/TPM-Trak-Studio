using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Pitti
{
    public partial class PMChecklistMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachineID();
                BindSupervisor();
                PreparedBy();
                ReviewedBy();
                ApprovedBy();
                BindMintenanceManagers();
                BindPMMasterData();
            }
        }
        public void BindMachineID()
        {
            List<string> list = new List<string>();
            try
            {
                list = DataBaseAccess.GetAllMachinesFromMaster();
                ddlMachineID.DataSource = list;
                //list.Insert(0,"All");
                ddlMachineID.DataBind();
                txtSourceMachineID.Text = ddlMachineID.SelectedValue;
                lbMultiMachineID.DataSource = list;
                lbMultiMachineID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachineID: " + ex.Message);
            }
        }
        private void BindSupervisor()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            try
            {
                list = DataBaseAccess.GetSuperVisorInfo_Pitti();
                List<string> list_Names = new List<string>();
                if (list.Count > 0)
                    list_Names = list.Values.ToList() as List<string>;

                ddlSupervisor.DataSource = list_Names;
                ddlSupervisor.DataBind();

                Session["SuperVisorData"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindSupervisor: " + ex.Message);
            }
        }
        private void PreparedBy()
        {
            List<ListItem> list = new List<ListItem>();
            try
            {
                list = DataBaseAccess.GetSrEngineers();
                ddlPreparedBy.DataSource = list;
                ddlPreparedBy.DataTextField = "Text";
                ddlPreparedBy.DataValueField = "Value";
                ddlPreparedBy.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void ReviewedBy()
        {
            List<ListItem> list = new List<ListItem>();
            try
            {
                list = DataBaseAccess.GetManagers();
                ddlReviewedBy.DataSource = list;
                ddlReviewedBy.DataTextField = "Text";
                ddlReviewedBy.DataValueField = "Value";
                ddlReviewedBy.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void ApprovedBy()
        {
            List<ListItem> list = new List<ListItem>();
            try
            {
                list = DataBaseAccess.GetHODs();
                ddlApprovedBy.DataSource = list;
                ddlApprovedBy.DataTextField = "Text";
                ddlApprovedBy.DataValueField = "Value";
                ddlApprovedBy.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindMintenanceManagers()
        {
            List<ListItem> list = new List<ListItem>();
            try
            {
                list = DataBaseAccess.GetMaintenanceMnagers();
                ddlMaintManager.DataSource = list;
                ddlMaintManager.DataTextField = "Text";
                ddlMaintManager.DataValueField = "Value";
                ddlMaintManager.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindPMMasterData()
        {
            try
            {
                txtSourceMachineID.Text = ddlMachineID.SelectedValue;
                List<PreventiveMaintenanceChecksheet_Pitti> list = DataBaseAccess.GetPMMasterData(ddlMachineID.SelectedValue, ddlFrequency.SelectedValue);
                int flag = 0;
                if (list.Count == 0)
                {
                    flag = 1;
                    list.Add(new PreventiveMaintenanceChecksheet_Pitti());
                }
                gvPMMaster.DataSource = list;
                gvPMMaster.DataBind();
                if (flag == 1)
                {
                    gvPMMaster.Rows[0].Visible = false;
                }
                gvPMMaster.FooterRow.Visible = false;
                btnNew.Visible = true;
                btnCancel.Visible = false;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void BtnView_Click(object sender, EventArgs e)
        {
            BindPMMasterData();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                gvPMMaster.FooterRow.Visible = true;
                btnNew.Visible = false;
                btnCancel.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ScrollBottom", "setScrollToBottotm()", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            BindPMMasterData();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isInserted = DataBaseAccess.SavePMCheckSheetHeaderData(ddlMachineID.SelectedValue.ToString(), ddlPreparedBy.SelectedValue.ToString(), ddlReviewedBy.SelectedValue.ToString(), ddlApprovedBy.SelectedValue.ToString(), txtRefNo.Text, txtRevNo.Text, ddlSupervisor.SelectedValue.ToString(), ddlMaintManager.SelectedValue.ToString(), ddlFrequency.SelectedValue.ToString());
                PreventiveMaintenanceChecksheet_Pitti data = new PreventiveMaintenanceChecksheet_Pitti();
                if (gvPMMaster.FooterRow.Visible)
                {
                    var row = gvPMMaster.FooterRow;
                    data.MachineID = ddlMachineID.SelectedValue;
                    data.CategoryID = (row.FindControl("txtCategoryID") as TextBox).Text;
                    data.Category = (row.FindControl("ddlCategory") as DropDownList).SelectedValue;
                    data.CheckpointID = (row.FindControl("txtCheckPointID") as TextBox).Text;
                    data.Description = (row.FindControl("txtDescription") as TextBox).Text;
                    data.JudgementalCriteria = (row.FindControl("txtJudgementCriteria") as TextBox).Text;
                    data.ResourcesNeeded = (row.FindControl("txtResourceNeeded") as TextBox).Text;
                    data.Frequency = (row.FindControl("ddlFrequency") as DropDownList).SelectedValue;
                    data.SortOrder = (row.FindControl("txtSortOrder") as TextBox).Text;
                    data.Duration = (row.FindControl("txtDuration") as TextBox).Text;

                    string success = DataBaseAccess.SavePMChecksheetMasterData(data);
                    if (success.Equals("Updated", StringComparison.OrdinalIgnoreCase))
                    {
                        HelperClassGeneric.openUpdateSuccessModal(this);
                    }
                    else if (success.Equals("Inserted", StringComparison.OrdinalIgnoreCase))
                    {
                        HelperClassGeneric.openInsertSuccessModal(this);
                    }
                    else
                    {
                        HelperClassGeneric.openInsertErrorModal(this);
                        return;
                    }
                }
                else
                {
                    int updatedCount = 0;
                    for (int i = 0; i < gvPMMaster.Rows.Count; i++)
                    {
                        data = new PreventiveMaintenanceChecksheet_Pitti();
                        var row = gvPMMaster.Rows[i];
                        if ((row.FindControl("hdnUpdate") as HiddenField).Value.Equals("Update", StringComparison.OrdinalIgnoreCase))
                        {
                            data.MachineID = ddlMachineID.SelectedValue;
                            data.CategoryID = (row.FindControl("lblCategoryID") as Label).Text;
                            data.Category = (row.FindControl("ddCategory") as DropDownList).SelectedValue;
                            data.CheckpointID = (row.FindControl("lblCheckpointID") as Label).Text;
                            data.Description = (row.FindControl("txDescription") as TextBox).Text;
                            data.JudgementalCriteria = (row.FindControl("txJudgementCriteria") as TextBox).Text;
                            data.ResourcesNeeded = (row.FindControl("txResourcesNeeded") as TextBox).Text;
                            data.Frequency = (row.FindControl("ddFrequency") as DropDownList).SelectedValue;
                            data.SortOrder = (row.FindControl("txSortOrder") as TextBox).Text;
                            data.Duration = (row.FindControl("txDuration") as TextBox).Text;
                            string success = DataBaseAccess.SavePMChecksheetMasterData(data);
                            if (success.Equals("Updated", StringComparison.OrdinalIgnoreCase))
                            {
                                updatedCount++;
                            }
                        }
                    }
                    if (updatedCount > 0)
                    {
                        HelperClassGeneric.openUpdateSuccessModal(this);
                    }
                }
                BindPMMasterData();
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
                int deleteCount = 0;
                for (int i = 0; i < gvPMMaster.Rows.Count; i++)
                {
                    PreventiveMaintenanceChecksheet_Pitti data = new PreventiveMaintenanceChecksheet_Pitti();
                    var row = gvPMMaster.Rows[i];
                    if ((row.FindControl("chkSelect") as CheckBox).Checked)
                    {
                        data.MachineID = ddlMachineID.SelectedValue;
                        data.CheckpointID = (row.FindControl("lblCheckpointID") as Label).Text;
                        data.CategoryID = (row.FindControl("lblCategoryID") as Label).Text;
                        string success = DataBaseAccess.DeletePMMasterData(data, ddlFrequency.SelectedValue);
                        if (success.Equals("Deleted", StringComparison.OrdinalIgnoreCase))
                        {
                            deleteCount++;
                        }
                    }
                }
                if (deleteCount > 0)
                {
                    HelperClassGeneric.openDeleteSuccessModal(this);
                    HelperClassGeneric.clearModal(this);
                }
                else
                {
                    HelperClassGeneric.clearModal(this);
                    HelperClassGeneric.openDeleteErrorModal(this);
                    return;
                }
                BindPMMasterData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void gvPMMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlCategory = (e.Row.FindControl("ddCategory") as DropDownList);
                    HelperClassGeneric.setDropdownValue(ddlCategory, (e.Row.FindControl("hdnCategory") as HiddenField).Value);
                    DropDownList ddlFrequency = (e.Row.FindControl("ddFrequency") as DropDownList);
                    HelperClassGeneric.setDropdownValue(ddlFrequency, (e.Row.FindControl("hdnFrequency") as HiddenField).Value);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
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
                res = DataBaseAccess.CopyRevData("S_Get_PM_MaintenanceDetails_Pitti", DestMachines, txtSourceMachineID.Text.Trim());
                if (res > 0)
                    HelperClassGeneric.openSuccessModal(this, "Copied Successfully");
                else
                    HelperClassGeneric.openErrorModal(this, "Could't be Copied. Try Again!");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"btnCopyModal_Click: {ex.Message}");
            }
        }
    }
}