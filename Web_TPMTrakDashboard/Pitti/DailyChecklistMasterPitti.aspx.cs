using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Pitti
{
    public partial class DailyChecklistMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachineID();
                BindSupervisor();
                BindHeaderDetails();
                BindCheckpointGrid();
            }
        }

        public void BindMachineID()
        {
            List<string> list = new List<string>();
            try
            {
                list = DataBaseAccess.GetAllMachinesFromMaster();

                ddlMachineID.DataSource = list;
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
                    list_Names = list.Values.ToList<string>();

                ddlSupervisor.DataSource = list_Names;
                ddlSupervisor.DataBind();

                Session["SuperVisorData"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindSupervisor: " + ex.Message);
            }
        }

        private void BindHeaderDetails()
        {
            try
            {
                DataTable dt = DataBaseAccess.GetHeaderDetails_Pitti(ddlMachineID.SelectedValue);
                if (dt.Rows.Count > 0)
                {
                    txtRefNo.Text = dt.Rows[0]["RefNo"].ToString();
                    txtRevNo.Text = dt.Rows[0]["RevNo"].ToString();
                    ddlSupervisor.SelectedValue = dt.Rows[0]["SupervisorID"].ToString();
                }
                else
                {
                    txtRefNo.Text = "";
                    txtRevNo.Text = "";
                    ddlSupervisor.SelectedValue = ddlSupervisor.Items[0].Value;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindHeaderDetails: " + ex.Message);
            }
        }

        private void BindCheckpointGrid()
        {
            List<DailyChecklistEntity_Pitti> list = new List<DailyChecklistEntity_Pitti>();
            try
            {
                list = DataBaseAccess.GetDailyCheckpointData_Pitti(ddlMachineID.SelectedValue, txtRefNo.Text.Trim(), txtRevNo.Text.Trim());

                lvCheckGrid.DataSource = list;
                lvCheckGrid.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void BtnView_Click(object sender, EventArgs e)
        {
            BindCheckpointGrid();
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool isInserted = false;
            try
            {
                string MachineID = ddlMachineID.SelectedValue;

                if (hdnHeaderValues.Value.Equals("Update", StringComparison.OrdinalIgnoreCase))
                {
                    string ReferenceNo = txtRefNo.Text.Trim();
                    string RevisionNo = txtRevNo.Text.Trim();
                    string Supervisor = ddlSupervisor.SelectedValue;

                    isInserted = DataBaseAccess.SaveDailyCheckHeaderData(MachineID, ReferenceNo, RevisionNo, Supervisor);
                }
                DailyChecklistEntity_Pitti data = new DailyChecklistEntity_Pitti();
                if (hdnValueNewEdit.Value.Trim().Equals("New", StringComparison.OrdinalIgnoreCase))
                {
                    var firstRow = (lvCheckGrid.InsertItem.FindControl("trInsertItemTemplate"));
                    data.CheckPoint = (firstRow.FindControl("txtSlNo") as TextBox).Text.Trim();
                    data.CheckPointDesc = (firstRow.FindControl("txtCheckPointDesc") as TextBox).Text.Trim();
                    data.Standard = (firstRow.FindControl("txtStandard") as TextBox).Text.Trim();
                    data.Frequency = (firstRow.FindControl("ddlFrequency") as DropDownList).SelectedValue;
                    data.SortOrder = (firstRow.FindControl("txtSortOrder") as TextBox).Text.Trim();

                    isInserted = DataBaseAccess.SaveDailyCheckMasterData(data, MachineID);
                    hdnValueNewEdit.Value = "";
                }

                foreach (var item in lvCheckGrid.Items)
                {
                    data = new DailyChecklistEntity_Pitti();

                    if ((item.FindControl("hdnUpdate") as HiddenField).Value.Equals("Updated", StringComparison.OrdinalIgnoreCase))
                    {
                        data.CheckPoint = (item.FindControl("lblSlNo") as Label).Text.Trim();
                        data.CheckPointDesc = (item.FindControl("txtCheckPointDesc") as TextBox).Text.Trim();
                        data.Standard = (item.FindControl("txtStandard") as TextBox).Text.Trim();
                        data.Frequency = (item.FindControl("ddlFrequency") as DropDownList).SelectedValue;
                        data.SortOrder = (item.FindControl("txtSortOrder") as TextBox).Text.Trim();

                        isInserted = DataBaseAccess.SaveDailyCheckMasterData(data, MachineID);
                    }
                }

                if (isInserted)
                {
                    HelperClassGeneric.openSuccessModal(this, "Saved Succesfully");
                }
                BindHeaderDetails();
                BindCheckpointGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void ddlMachineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSourceMachineID.Text = ddlMachineID.SelectedValue;
            BindHeaderDetails();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string MachineID = string.Empty, isDeleted = "";
            try
            {
                MachineID = ddlMachineID.SelectedValue;
                foreach (var item in lvCheckGrid.Items)
                {
                    if ((item.FindControl("chkDelete") as CheckBox).Checked)
                    {
                        string CheckPointID = (item.FindControl("lblSlNo") as Label).Text.Trim();

                        isDeleted = DataBaseAccess.DeleteCheckpointMasterData(MachineID, CheckPointID);
                    }

                }

                if (isDeleted == "" || isDeleted == "Deleted")
                {
                    HelperClassGeneric.openSuccessModal(this, "Deleted Succesfully");
                    HelperClassGeneric.clearModal(this);
                }

                BindCheckpointGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenCopy", "$('#CLCopyModal').modal('hide');", true);
                    HelperClassGeneric.openWarningModal(this, "Select Atleast One Dest. Machine");
                    return;
                }
                res = DataBaseAccess.CopyRevData("S_Get_AM_MaintenanceDetails_Pitti", DestMachines, txtSourceMachineID.Text.Trim());
                if (res > 0)
                    HelperClassGeneric.openSuccessModal(this, "Copied Successfully");
                else
                    HelperClassGeneric.openWarningToastrModal(this, "Could not be Copied. Try Again!");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"btnCopyModal_Click: {ex.Message}");
            }
        }
    }
}