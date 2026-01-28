using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.PrecisionEngineering
{
    public partial class MaintenanceGroupMaster : System.Web.UI.Page
    {
        private static string Group_ID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                else
                    SessionClear.ClearSession();
                BindGroupInfo();
            }
        }

        private void BindGroupInfo()
        {
            try
            {
                
                List<GroupDefintion> list = DataBaseAccess.GetMaintenanceGroupDetails_Precision();
                if (list.Count > 0 && list != null)
                {
                    gvGroupMaster.DataSource = list;
                    gvGroupMaster.DataBind();

                    GridViewRow row = gvGroupMaster.Rows[0];
                    row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    Group_ID = ((Label)gvGroupMaster.Rows[row.RowIndex].Cells[1].FindControl("lblGroupID")).Text;
                    Session["Group_ID"] = Group_ID;
                    BindAssignedMachineIDs(string.IsNullOrEmpty(Group_ID) ? "" : Group_ID);
                    BindUnAssignedMachineIDs(string.IsNullOrEmpty(Group_ID) ? "" : Group_ID);
                    btnCancel_Click(null, null);
                }
                else
                {
                    list.Add(new GroupDefintion { GroupID = "", GroupDesc = "" });
                    gvGroupMaster.DataSource = list;
                    gvGroupMaster.DataBind();
                    gvGroupMaster.Rows[0].Visible = false;
                    btnNew_Click(null, null);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            hdnFieldType.Value = "Insert";
            btnCancel.Visible = true;
            btnNew.Visible = false;
            gvGroupMaster.FooterRow.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            hdnFieldType.Value = "Update";
            btnCancel.Visible = false;
            btnNew.Visible = true;
            gvGroupMaster.FooterRow.Visible = false;
            (gvGroupMaster.FooterRow.FindControl("txtfooterGroupID") as TextBox).Text = string.Empty;
            (gvGroupMaster.FooterRow.FindControl("txtfooterGroupDesc") as TextBox).Text = string.Empty;
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string GroupID = string.Empty, GroupDesc = string.Empty;
                int isUpdated = 0;
                if (hdnFieldType.Value.Equals("Insert", StringComparison.OrdinalIgnoreCase))
                {
                    GroupID = (gvGroupMaster.FooterRow.FindControl("txtfooterGroupID") as TextBox).Text;
                    GroupDesc = (gvGroupMaster.FooterRow.FindControl("txtfooterGroupDesc") as TextBox).Text;
                    if (string.IsNullOrEmpty(GroupID))
                    {
                        HelperClassGeneric.openWarningToastrModal(this, "GroupID cannot be Empty!!");
                        return;
                    }
                    foreach (GridViewRow row in gvGroupMaster.Rows)
                    {
                        if (GroupID.Equals((row.FindControl("lblGroupID") as Label).Text))
                        {
                            HelperClassGeneric.openWarningToastrModal(this, "GroupID Already Exists!!");
                            return;
                        }
                    }
                    isUpdated = DataBaseAccess.SaveMaintenanceGroup_Precision(GroupID, GroupDesc);
                }
                else
                {
                    for (int i = 0; i < gvGroupMaster.Rows.Count; i++)
                    {
                        var row = gvGroupMaster.Rows[i];
                        if ((row.FindControl("hdnUpdate") as HiddenField).Value == "update")
                        {
                            GroupID = (row.FindControl("lblGroupID") as Label).Text;
                            GroupDesc = (row.FindControl("txtGroupDesc") as TextBox).Text;
                            isUpdated += DataBaseAccess.SaveMaintenanceGroup_Precision(GroupID, GroupDesc);
                        }
                    }
                }
                if (isUpdated > 0)
                {
                    HelperClassGeneric.openSuccessModal(this, "Saved Successfully.");
                }
                BindGroupInfo();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindAssignedMachineIDs(string GroupID)
        {
            try
            {
                List<string> assignedMachineID = DataBaseAccess.GetAssignedAndUnAssignedMachineIds("select distinct MachineID from MaintenanceGroupMachine_Precision where GroupID=@PlantID", GroupID);
                if (chkassigned.Items.Count > 0) chkassigned.Items.Clear();
                if (assignedMachineID.Count <= 0) return;
                foreach (string strMachine in assignedMachineID)
                {
                    chkassigned.Items.Add(strMachine);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindUnAssignedMachineIDs(string GroupID)
        {
            try
            {
                List<string> UnassignedMachineID = DataBaseAccess.GetAssignedAndUnAssignedMachineIds("select distinct machineID  from [machineinformation] where machineID NOT in (select distinct machineid from MaintenanceGroupMachine_Precision)", GroupID);
                if (chkaveliable.Items.Count > 0) chkaveliable.Items.Clear();
                if (UnassignedMachineID.Count <= 0) return;
                foreach (string strMachine in UnassignedMachineID)
                {
                    chkaveliable.Items.Add(strMachine);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }

        }

        protected void btnassign_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkaveliable.SelectedIndex > -1)
                {
                    foreach (ListItem item in chkaveliable.Items)
                    {
                        if (item.Selected)
                        {
                            string selectedValue = item.Value;
                            DataBaseAccess.AddMachineToGroupID(selectedValue, string.IsNullOrEmpty(Session["Group_ID"].ToString()) ? Group_ID : Session["Group_ID"].ToString());
                        }
                    }
                }
                BindAssignedMachineIDs(string.IsNullOrEmpty(Session["Group_ID"].ToString()) ? Group_ID : Session["Group_ID"].ToString());
                BindUnAssignedMachineIDs("");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }

        }

        protected void btnunassign_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkassigned.SelectedIndex > -1)
                {
                    foreach (ListItem item in chkassigned.Items)
                    {
                        if (item.Selected)
                        {
                            string selectedValue = item.Value;
                            DataBaseAccess.DeleteMachineToGroup(selectedValue, string.IsNullOrEmpty(Session["Group_ID"].ToString()) ? Group_ID : Session["Group_ID"].ToString());
                        }
                    }
                }
                BindAssignedMachineIDs(string.IsNullOrEmpty(Session["Group_ID"].ToString()) ? Group_ID : Session["Group_ID"].ToString());
                BindUnAssignedMachineIDs("");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void gvGroupMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var SelectButton = e.Row.FindControl("btnSelect") as Button;
                    SelectButton.ToolTip = GetGlobalResourceObject("CommanResource", "SelectToolTip").ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void gvGroupMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    foreach (GridViewRow row in gvGroupMaster.Rows)
                    {
                        if (row.RowIndex == index)
                        {
                            row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                            Group_ID = ((Label)gvGroupMaster.Rows[row.RowIndex].Cells[1].FindControl("lblGroupID")).Text;
                            Session["Group_ID"] = Group_ID;
                            BindAssignedMachineIDs(string.IsNullOrEmpty(Group_ID) ? "" : Group_ID);
                            BindUnAssignedMachineIDs(string.IsNullOrEmpty(Group_ID) ? "" : Group_ID);
                        }
                        else
                        {
                            row.BackColor = System.Drawing.Color.Transparent;
                            var SelectButton = row.FindControl("btnSelect") as Button;
                            SelectButton.ToolTip = GetGlobalResourceObject("CommanResource", "SelectToolTip").ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
    }
}