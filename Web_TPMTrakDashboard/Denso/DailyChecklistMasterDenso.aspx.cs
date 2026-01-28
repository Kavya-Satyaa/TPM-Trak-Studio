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
    public partial class DailyChecklistMasterDenso : System.Web.UI.Page
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
                List<ListItem> list = DensoDBAccess.getDailyChecklistRevisionNumber(ddlMachine.SelectedValue);
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
                List<DailyChecklistMasterEntity> list = DensoDBAccess.getDailyChecklistMasterEntity(ddlMachine.SelectedValue, revID);
                int flag = 0;
                if (list.Count == 0)
                {
                    flag = 1;
                    list.Add(new DailyChecklistMasterEntity());
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
                    ddl = (e.Row.FindControl("ddlFrequency") as DropDownList);
                    HelperClassGeneric.setDropdownValue(ddl, (e.Row.FindControl("hdnFrequency") as HiddenField).Value);
                    ddl = (e.Row.FindControl("ddlChecklistType") as DropDownList);
                    HelperClassGeneric.setDropdownValue(ddl, (e.Row.FindControl("hdnChecklistType") as HiddenField).Value);
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
                DailyChecklistMasterEntity data = new DailyChecklistMasterEntity();
                if (gvChecklist.FooterRow.Visible)
                {
                    var row = gvChecklist.FooterRow;
                    data.MachineID = ddlMachine.SelectedValue;
                    data.Category = (row.FindControl("txtCategory") as TextBox).Text;
                    data.ChecklistID = (row.FindControl("txtChecklistID") as TextBox).Text;
                    data.JudgementCriteria = (row.FindControl("txtJudgementCriteria") as TextBox).Text;
                    data.Method = (row.FindControl("txtMethod") as TextBox).Text;
                    data.Cycle = (row.FindControl("txtCycle") as TextBox).Text;
                    data.PersonInCharge = (row.FindControl("txtPersonInCharge") as TextBox).Text;
                    data.Frequency = (row.FindControl("ddlFrequency") as DropDownList).SelectedValue;
                    data.FormatNumber = (row.FindControl("txtFormatNumber") as TextBox).Text;
                    data.RevisedBy = Session["UserName"].ToString();
                    data.ChecklistDesc = (row.FindControl("txtChecklistDesc") as TextBox).Text;
                    data.ChecklistType = (row.FindControl("ddlChecklistType") as DropDownList).SelectedValue;
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
                    string success = DensoDBAccess.insertUpdateDailyChecklistMasterEntity(data);
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
                        data = new DailyChecklistMasterEntity();
                        var row = gvChecklist.Rows[i];
                        if ((row.FindControl("hdnUpdate") as HiddenField).Value.Equals("Update", StringComparison.OrdinalIgnoreCase))
                        {
                            data.MachineID = (row.FindControl("hdnMachineID") as HiddenField).Value;
                            data.Category = (row.FindControl("lblCategory") as Label).Text;
                            data.ChecklistID = (row.FindControl("lblChecklistID") as Label).Text;
                            data.ChecklistDesc = (row.FindControl("txtChecklistDesc") as TextBox).Text;
                            data.ChecklistType = (row.FindControl("ddlChecklistType") as DropDownList).SelectedValue;
                            data.SortOrder = (row.FindControl("txtSortOrder") as TextBox).Text;
                            data.JudgementCriteria = (row.FindControl("txtJudgementCriteria") as TextBox).Text;
                            data.Method = (row.FindControl("txtMethod") as TextBox).Text;
                            data.Cycle = (row.FindControl("txtCycle") as TextBox).Text;
                            data.PersonInCharge = (row.FindControl("txtPersonInCharge") as TextBox).Text;
                            data.Frequency = (row.FindControl("ddlFrequency") as DropDownList).SelectedValue;
                            data.FormatNumber = (row.FindControl("txtFormatNumber") as TextBox).Text;
                            data.RevisedBy = Session["UserName"].ToString();
                            data.RevNo = (row.FindControl("hdnRevNo") as HiddenField).Value;
                            data.RevID = (row.FindControl("hdnRevID") as HiddenField).Value;
                            string success = DensoDBAccess.insertUpdateDailyChecklistMasterEntity(data);
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
                string result = DensoDBAccess.copyDailyChecklistMasterDetails(sourceMachine, destinationMachine);
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
                    DailyChecklistMasterEntity data = new DailyChecklistMasterEntity();
                    var row = gvChecklist.Rows[i];
                    if ((row.FindControl("chkSelect") as CheckBox).Checked)
                    {
                        data.MachineID = (row.FindControl("hdnMachineID") as HiddenField).Value;
                        data.ChecklistID = (row.FindControl("lblChecklistID") as Label).Text;
                        data.RevNo = (row.FindControl("hdnRevNo") as HiddenField).Value;
                        data.RevID = (row.FindControl("hdnRevID") as HiddenField).Value;
                        if (DensoDBAccess.isDailyChecklistTransactionStarted(data))
                        {
                            HelperClassGeneric.openWarningModal(this, "Transaction started for Checkpoint " + data.ChecklistID + ". Please create new Rev No.");
                            return;
                        }
                    }
                }
                int deleteCount = 0;
                for (int i = 0; i < gvChecklist.Rows.Count; i++)
                {
                    DailyChecklistMasterEntity data = new DailyChecklistMasterEntity();
                    var row = gvChecklist.Rows[i];
                    if ((row.FindControl("chkSelect") as CheckBox).Checked)
                    {
                        data.MachineID = (row.FindControl("hdnMachineID") as HiddenField).Value;
                        data.Category = (row.FindControl("lblCategory") as Label).Text;
                        data.ChecklistID = (row.FindControl("lblChecklistID") as Label).Text;
                        data.RevNo = (row.FindControl("hdnRevNo") as HiddenField).Value;
                        data.RevID = (row.FindControl("hdnRevID") as HiddenField).Value;
                        string success = DensoDBAccess.deleteDailyChecklistMasterEntity(data);
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