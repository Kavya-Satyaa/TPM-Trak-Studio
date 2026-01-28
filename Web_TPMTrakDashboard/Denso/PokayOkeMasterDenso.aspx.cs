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
    public partial class PokayOkeMasterDenso : System.Web.UI.Page
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
                BindRevNumber();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindRevNumber()
        {
            try
            {
                List<ListItem> list = DensoDBAccess.getPokayOkeChecksheetRevisionNumber(ddlMachine.SelectedValue);
                ddlRevNo.DataSource = list;
                ddlRevNo.DataTextField = "Text";
                ddlRevNo.DataValueField = "Value";
                ddlRevNo.DataBind();
                if (list.Count > 0)
                {
                    ddlRevNo.Visible = true;
                    txtRevNo.Visible = false;
                    btnNewRevision.Visible = true;
                }
                else
                {
                    ddlRevNo.Visible = false;
                    txtRevNo.Visible = true;
                    btnNewRevision.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRevNumber();
        }
        private void BindData()
        {
            try
            {
                string revID = "";
                if (ddlRevNo.Visible)
                {
                    revID = ddlRevNo.SelectedValue;
                }
                List<PokayOkeMasterEntity> list = DensoDBAccess.getPokayOkeChecksheetMasterEntity(ddlMachine.SelectedValue, revID);
                int flag = 0;
                if (list.Count == 0)
                {
                    flag = 1;
                    list.Add(new PokayOkeMasterEntity());
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
                lblCopySourceMachine.Text = ddlMachine.SelectedValue;
                List<string> machineList = new List<string>();
                foreach (ListItem item in ddlMachine.Items)
                {
                    if (!item.Value.Equals(lblCopySourceMachine.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        machineList.Add(item.Value);
                    }
                }
                lbCopyDestMachine.DataSource = machineList;
                lbCopyDestMachine.DataBind();
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
                    ddl = (e.Row.FindControl("ddlCheckPointType") as DropDownList);
                    HelperClassGeneric.setDropdownValue(ddl, (e.Row.FindControl("hdnCheckPointType") as HiddenField).Value);
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
                PokayOkeMasterEntity data = new PokayOkeMasterEntity();
                if (gvChecklist.FooterRow.Visible)
                {
                    var row = gvChecklist.FooterRow;
                    data.MachineID = ddlMachine.SelectedValue;
                    data.PokayOkeItem = (row.FindControl("txtPokayOkeItem") as TextBox).Text;
                    data.Function = (row.FindControl("txtFunction") as TextBox).Text;
                    data.CheckMethod = (row.FindControl("txtCheckMethod") as TextBox).Text;
                    data.CheckInterval = (row.FindControl("txtCheckInterval") as TextBox).Text;
                    data.RevisedBy = Session["UserName"].ToString();
                    data.CheckPointType = (row.FindControl("ddlCheckPointType") as DropDownList).SelectedValue;
                    data.SortOrder = (row.FindControl("txtSortOrder") as TextBox).Text;

                    if (ddlRevNo.Visible)
                    {
                        data.RevNo = ddlRevNo.SelectedItem.Text;
                        data.RevID = ddlRevNo.SelectedValue;
                    }
                    else
                    {
                        data.RevNo = txtRevNo.Text;
                    }
                    string success = DensoDBAccess.insertUpdatePokayOkeChecksheetMasterEntity(data);
                    if (success == "Updated")
                    {
                        HelperClassGeneric.openUpdateSuccessModal(this);
                    }
                    else if (success == "Inserted")
                    {
                        if (txtRevNo.Visible)
                        {
                            BindRevNumber();
                        }
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
                        data = new PokayOkeMasterEntity();
                        var row = gvChecklist.Rows[i];
                        if ((row.FindControl("hdnUpdate") as HiddenField).Value.Equals("Update", StringComparison.OrdinalIgnoreCase))
                        {
                            data.MachineID = (row.FindControl("hdnMachineID") as HiddenField).Value;
                            data.PokayOkeItem = (row.FindControl("lblPokayOkeItem") as Label).Text;
                            data.Function = (row.FindControl("txtFunction") as TextBox).Text;
                            data.CheckMethod = (row.FindControl("txtCheckMethod") as TextBox).Text;
                            data.CheckInterval = (row.FindControl("txtCheckInterval") as TextBox).Text;
                            data.CheckPointType = (row.FindControl("ddlCheckPointType") as DropDownList).SelectedValue;
                            data.SortOrder = (row.FindControl("txtSortOrder") as TextBox).Text;
                            data.RevisedBy = Session["UserName"].ToString();
                            data.RevNo = (row.FindControl("hdnRevNo") as HiddenField).Value;
                            data.RevID = (row.FindControl("hdnRevID") as HiddenField).Value;
                            string success = DensoDBAccess.insertUpdatePokayOkeChecksheetMasterEntity(data);
                            if (success == "Updated")
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

        protected void btnCopyConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string sourceMachine = lblCopySourceMachine.Text;
                string destinationMachine = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbCopyDestMachine);
                string result = DensoDBAccess.copyPokayOkeChecksheetMasterDetails(sourceMachine, destinationMachine);
                if (result == "error")
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
                BindRevNumber();
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnInsertNewRevNumber_Click(object sender, EventArgs e)
        {

        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < gvChecklist.Rows.Count; i++)
                {
                    var row = gvChecklist.Rows[i];
                    if ((row.FindControl("chkSelect") as CheckBox).Checked)
                    {
                        PokayOkeMasterEntity data = new PokayOkeMasterEntity();
                        data.MachineID = (row.FindControl("hdnMachineID") as HiddenField).Value;
                        data.PokayOkeItem = (row.FindControl("lblPokayOkeItem") as Label).Text;
                        data.CheckMethod = (row.FindControl("txtCheckMethod") as TextBox).Text;
                        data.RevID = (row.FindControl("hdnRevID") as HiddenField).Value;
                        if (DensoDBAccess.isPokayOkeChecksheetMasterEntity(data))
                        {
                            HelperClassGeneric.openWarningModal(this, "Transaction started for Checkpoint " + data.PokayOkeItem + ". Please create new Rev No.");
                            return;
                        }
                    }
                }
                int deleteCount = 0;
                for (int i = 0; i < gvChecklist.Rows.Count; i++)
                {
                    var row = gvChecklist.Rows[i];
                    if ((row.FindControl("chkSelect") as CheckBox).Checked)
                    {
                        PokayOkeMasterEntity data = new PokayOkeMasterEntity();
                        data.MachineID = (row.FindControl("hdnMachineID") as HiddenField).Value;
                        data.PokayOkeItem = (row.FindControl("lblPokayOkeItem") as Label).Text;
                        data.CheckMethod = (row.FindControl("txtCheckMethod") as TextBox).Text;
                        data.RevID = (row.FindControl("hdnRevID") as HiddenField).Value;
                        string success = DensoDBAccess.deletePokayOkeChecksheetMasterEntity(data);
                        if (success == "Deleted")
                        {
                            deleteCount++;
                        }
                    }
                }
                if (deleteCount > 0)
                {
                    HelperClassGeneric.openDeleteSuccessModal(this);
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}