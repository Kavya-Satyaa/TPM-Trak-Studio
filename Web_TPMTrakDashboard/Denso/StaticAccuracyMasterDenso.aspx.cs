using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Denso.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Denso
{
    public partial class StaticAccuracyMasterDenso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachine();
                BindData();
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllMachines("");
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void BindData()
        {
            try
            {
                List<StaticAccuracyMasterEntity> list = DensoDBAccess.getStaticAccuracyMasterDetails(ddlMachine.SelectedValue);
                int flag = 0;
                if (list.Count == 0)
                {
                    flag = 1;
                    list.Add(new StaticAccuracyMasterEntity());
                }
                gvChecklist.DataSource = list;
                gvChecklist.DataBind();
                if (flag == 1)
                {
                    gvChecklist.Rows[0].Visible = false;
                }
                gvChecklist.FooterRow.Visible = false;
                btnNew.Visible = true;
                btnCancel.Visible = false;
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
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }

        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            gvChecklist.FooterRow.Visible = true;
            btnNew.Visible = false;
            btnCancel.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ScrollBottom", "setScrollToBottotm()", true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            BindData();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                StaticAccuracyMasterEntity data = new StaticAccuracyMasterEntity();
                if (gvChecklist.FooterRow.Visible)
                {
                    var row = gvChecklist.FooterRow;
                    data.MachineID = ddlMachine.SelectedValue;
                    data.Checkpoint = (row.FindControl("txtCheckpoint") as TextBox).Text;
                    data.SortOrder = (row.FindControl("txtSortOrder") as TextBox).Text;
                    data.CheckpointType = (row.FindControl("ddlChecklistType") as DropDownList).SelectedValue;

                    string success = DensoDBAccess.insertUpdateStaticAccuracyMasterEntity(data);
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
                    for (int i = 0; i < gvChecklist.Rows.Count; i++)
                    {
                        data = new StaticAccuracyMasterEntity();
                        var row = gvChecklist.Rows[i];
                        if ((row.FindControl("hdnUpdate") as HiddenField).Value.Equals("Update", StringComparison.OrdinalIgnoreCase))
                        {
                            data.MachineID = (row.FindControl("hdnMachineID") as HiddenField).Value;
                            data.Checkpoint = (row.FindControl("lblCheckpoint") as Label).Text;
                            data.SortOrder = (row.FindControl("txtSortOrder") as TextBox).Text;
                            data.CheckpointType = (row.FindControl("ddlChecklistType") as DropDownList).SelectedValue;
                            string success = DensoDBAccess.insertUpdateStaticAccuracyMasterEntity(data);
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
                BindData();
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
                for (int i = 0; i < gvChecklist.Rows.Count; i++)
                {
                    StaticAccuracyMasterEntity data = new StaticAccuracyMasterEntity();
                    var row = gvChecklist.Rows[i];
                    if ((row.FindControl("chkSelect") as CheckBox).Checked)
                    {
                        data.MachineID = (row.FindControl("hdnMachineID") as HiddenField).Value;
                        data.Checkpoint = (row.FindControl("lblCheckpoint") as Label).Text;
                        data.SortOrder = (row.FindControl("txtSortOrder") as TextBox).Text;
                        string success = DensoDBAccess.deleteStaticAccuracyMasterEntity(data);
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
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void gvChecklist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddl = new DropDownList();
                    ddl = (e.Row.FindControl("ddlChecklistType") as DropDownList);
                    HelperClassGeneric.setDropdownValue(ddl, (e.Row.FindControl("hdnCheckpointType") as HiddenField).Value);
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}