using ModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.Advik
{
    public partial class DownCodeByStation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                else
                    SessionClear.ClearSession();
                BindMachineID();
            }
        }

        private void BindMachineID()
        {
            try
            {
                List<string> MachineID = DataBaseAccess.AdvikDatabaseAccess.GetMachines("");
                ddlMachineID.DataSource = MachineID;
                ddlMachineID.DataBind();
                btnView_Click(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindAssignedMachineIDs((ddlMachineID.SelectedValue == null) ? "" : ddlMachineID.SelectedValue.ToString());
            BindUnAssignedMachineIDs((ddlMachineID.SelectedValue == null) ? "" : ddlMachineID.SelectedValue.ToString());
        }

        private void BindUnAssignedMachineIDs(string MachineID)
        {
            List<string> UnassignedMachineID = DataBaseAccess.AdvikDatabaseAccess.GetAssignedAndUnAssignedDowns("select distinct downid  from [downcodeinformation] where downid NOT in (select distinct downid from MachineDownCode)", MachineID);
            if (chkaveliable.Items.Count > 0) chkaveliable.Items.Clear();
            if (UnassignedMachineID.Count <= 0) return;
            foreach (string strMachine in UnassignedMachineID)
            {
                chkaveliable.Items.Add(strMachine);
            }
        }

        private void BindAssignedMachineIDs(string MachineID)
        {
            List<string> assignedMachineID = DataBaseAccess.AdvikDatabaseAccess.GetAssignedAndUnAssignedDowns("select distinct downid from MachineDownCode where MachineID = @MachineId", MachineID);
            if (chkassigned.Items.Count > 0) chkassigned.Items.Clear();
            if (assignedMachineID.Count <= 0) return;
            foreach (string strMachine in assignedMachineID)
            {
                chkassigned.Items.Add(strMachine);
            }
        }

        protected void btnassign_Click(object sender, EventArgs e)
        {
            if (chkaveliable.SelectedIndex > -1)
            {
                foreach (ListItem item in chkaveliable.Items)
                {
                    if (item.Selected)
                    {
                        string selectedValue = item.Value;
                        DataBaseAccess.AdvikDatabaseAccess.AddDownToMachineID(selectedValue, (ddlMachineID.SelectedValue.ToString()));
                    }
                }
            }
            BindAssignedMachineIDs(ddlMachineID.SelectedValue.ToString());
            BindUnAssignedMachineIDs("");
        }

        protected void btnunassign_Click(object sender, EventArgs e)
        {
            if (chkassigned.SelectedIndex > -1)
            {
                foreach (ListItem item in chkassigned.Items)
                {
                    if (item.Selected)
                    {
                        string selectedValue = item.Value;
                        DataBaseAccess.AdvikDatabaseAccess.DeleteDownToMachine(selectedValue, ddlMachineID.SelectedValue.ToString());
                    }
                }
            }
            BindAssignedMachineIDs(ddlMachineID.SelectedValue.ToString());
            BindUnAssignedMachineIDs("");
        }
    }
}