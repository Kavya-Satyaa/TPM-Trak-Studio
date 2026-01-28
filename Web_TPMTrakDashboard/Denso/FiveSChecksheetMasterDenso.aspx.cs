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
    public partial class FiveSChecksheetMasterDenso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ShiftList"] = null;
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
        private void BindShift(ListBox listbox, string value)
        {
            try
            {
                List<ListItem> shiftList = new List<ListItem>();
                if (Session["ShiftList"] == null)
                {
                    Session["ShiftList"] = shiftList = DataBaseAccess.GetAllShiftIds();
                }
                shiftList = Session["ShiftList"] as List<ListItem>;
                listbox.DataSource = shiftList;
                listbox.DataTextField = "Text";
                listbox.DataValueField = "Value";
                listbox.DataBind();
                if (value != "")
                {
                    var shifts = value.Split(',').ToList();
                    foreach (string shift in shifts)
                    {
                        if (listbox.Items.FindByValue(shift) != null)
                        {
                            listbox.Items.FindByValue(shift).Selected = true;
                        }
                    }
                }
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
                List<FiveSChecksheetMasterEntity> list = DensoDBAccess.getFiveSChecksheetMasterDetails(ddlMachine.SelectedValue);
                int flag = 0;
                if (list.Count == 0)
                {
                    flag = 1;
                    list.Add(new FiveSChecksheetMasterEntity());
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
                FiveSChecksheetMasterEntity data = new FiveSChecksheetMasterEntity();
                if (gvChecklist.FooterRow.Visible)
                {
                    var row = gvChecklist.FooterRow;
                    data.MachineID = ddlMachine.SelectedValue;
                    data.Checkpoint = (row.FindControl("txtCheckpoint") as TextBox).Text;
                    data.SortOrder = (row.FindControl("txtSortOrder") as TextBox).Text;
                    data.CheckpointType = (row.FindControl("ddlChecklistType") as DropDownList).SelectedValue;
                    ListBox listBox = (row.FindControl("lbShift") as ListBox);
                    string shifts = "";
                    if (listBox.Visible)
                    {
                        foreach (ListItem item in listBox.Items)
                        {
                            if (item.Selected)
                            {
                                if (shifts == "") shifts += item.Value; else shifts += "," + item.Value;
                            }
                        }
                    }
                    data.Shifts = shifts;
                    string success = DensoDBAccess.insertUpdateFiveSChecksheetMasterEntity(data);
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
                        data = new FiveSChecksheetMasterEntity();
                        var row = gvChecklist.Rows[i];
                        if ((row.FindControl("hdnUpdate") as HiddenField).Value.Equals("Update", StringComparison.OrdinalIgnoreCase))
                        {
                            data.MachineID = (row.FindControl("hdnMachineID") as HiddenField).Value;
                            data.Checkpoint = (row.FindControl("lblCheckpoint") as Label).Text;
                            data.SortOrder = (row.FindControl("txtSortOrder") as TextBox).Text;
                            data.CheckpointType = (row.FindControl("ddlChecklistType") as DropDownList).SelectedValue;
                            ListBox listBox = (row.FindControl("lbShift") as ListBox);
                            string shifts = "";
                            if (listBox.Visible)
                            {
                                foreach (ListItem item in listBox.Items)
                                {
                                    if (item.Selected)
                                    {
                                        if (shifts == "") shifts += item.Value; else shifts += "," + item.Value;
                                    }
                                }
                            }
                            data.Shifts = shifts;
                            string success = DensoDBAccess.insertUpdateFiveSChecksheetMasterEntity(data);
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
                    FiveSChecksheetMasterEntity data = new FiveSChecksheetMasterEntity();
                    var row = gvChecklist.Rows[i];
                    if ((row.FindControl("chkSelect") as CheckBox).Checked)
                    {
                        data.MachineID = (row.FindControl("hdnMachineID") as HiddenField).Value;
                        data.Checkpoint = (row.FindControl("lblCheckpoint") as Label).Text;
                        data.SortOrder = (row.FindControl("txtSortOrder") as TextBox).Text;
                        string success = DensoDBAccess.deleteFiveSChecksheetMasterEntity(data);
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

                    BindShift((e.Row.FindControl("lbShift") as ListBox), (e.Row.FindControl("hdnShifts") as HiddenField).Value);
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    BindShift(e.Row.FindControl("lbShift") as ListBox, "");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}