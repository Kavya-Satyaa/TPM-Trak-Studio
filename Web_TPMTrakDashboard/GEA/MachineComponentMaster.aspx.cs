using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class MachineComponentMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachineID(ddlMachineId,true);
               
                BindMachineComponentInfo();
            }
        }

        private void BindMachineID(DropDownList ddl, bool isAllRequired)
        {
            try
            {
                List<string> machineIds = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllMacForPlant("");
                if (isAllRequired)
                {
                    machineIds.Insert(0, "All");
                }
                ddl.DataSource = machineIds;
                ddl.DataBind();
                ddl.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        private void BindComponent()
        {
            try
            {
                DropDownList ddlmachine = gvMachineComponentData.FooterRow.FindControl("ddlMachine_Footer") as DropDownList;
                List<string> machineIds = GEADatabaseAccess.GetComponents(ddlmachine.SelectedValue);
                DropDownList ddl = gvMachineComponentData.FooterRow.FindControl("ddlComponet_Footer") as DropDownList;
                ddl.DataSource = machineIds;
                ddl.DataBind();
                ddl.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindMachineComponentInfo();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            gvMachineComponentData.FooterRow.Visible = true;
            btnNew.Visible = false;
            btnSave.Visible = true;
            btnCancel.Visible = true;
            DropDownList ddl = gvMachineComponentData.FooterRow.FindControl("ddlMachine_Footer") as DropDownList;
            BindMachineID(ddl, false);
            HelperClassGeneric.setDropdownValue(ddl, ddlMachineId.SelectedValue);
            BindComponent();
        }

        protected void ddlMachineId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindComponent();
        }
        private void BindMachineComponentInfo()
        {
            try
            {
                string machineid = ddlMachineId.SelectedValue == null ? "" : ddlMachineId.SelectedValue.ToString();
                if (machineid == "")
                {
                    lblMessages.ForeColor = Color.Green;
                    lblMessages.Text = "Machine ID required.";
                }
                List<MachineCompnentInfo> machineCompnentInfos = GEADatabaseAccess.GetMachineComponentDetails(machineid);
                if (machineCompnentInfos.Count == 0)
                {
                    machineCompnentInfos.Add(new MachineCompnentInfo());
                }
                gvMachineComponentData.DataSource = machineCompnentInfos;
                gvMachineComponentData.DataBind();

                gvMachineComponentData.FooterRow.Visible = false;
                btnNew.Visible = true;
                btnSave.Visible = false;
                btnCancel.Visible = false;

            }
            catch(Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        public class MachineCompnentInfo
        {
            public string MachineID { get; set; }
            public string Component { get; set; }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsDeleted = false;
                if (gvMachineComponentData.Rows.Count > 0)
                {
                    foreach (GridViewRow row in gvMachineComponentData.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkSelect = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                            if (chkSelect.Checked)
                            {
                                MachineCompnentInfo data = new MachineCompnentInfo();
                                data.MachineID = (row.Cells[1].FindControl("lblMachine") as Label).Text;
                                data.Component = (row.Cells[1].FindControl("lblComponent") as Label).Text;
                                IsDeleted = GEADatabaseAccess.DeleteMachineComponentInfor(data);
                                if (!IsDeleted) break;
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('No data selected for deletion.')", true);
                    return;
                }
                if (IsDeleted)
                {
                    lblMessages.ForeColor = Color.Green;
                    lblMessages.Text = "Records deleted successfully.";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Error while deleting data.')", true);
                    return;
                }
                BindMachineComponentInfo();
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            BindMachineComponentInfo();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                MachineCompnentInfo data = new MachineCompnentInfo();
                data.MachineID = (gvMachineComponentData.FooterRow.FindControl("ddlMachine_Footer") as DropDownList).SelectedValue;
                data.Component = (gvMachineComponentData.FooterRow.FindControl("ddlComponet_Footer") as DropDownList).SelectedValue;
                if (data.MachineID == "" || data.Component == "")
                {
                    lblMessages.ForeColor = Color.Red;
                    lblMessages.Text = "Machine ID and Component is required.";
                    return;
                }
                string success = GEADatabaseAccess.InsertUpdateMachineComponentDetails(data);
                if (success == "Exists")
                {
                    lblMessages.ForeColor = Color.Red;
                    lblMessages.Text = "Machine ID and Component already exists.";
                    return;
                }
                else if (success == "Inserted")
                {
                    lblMessages.ForeColor = Color.Green;
                    lblMessages.Text = "Record inserted successfully.";
                }
                else
                {
                    lblMessages.ForeColor = Color.Red;
                    lblMessages.Text = "Failed to insert records.";
                    return;
                }
                BindMachineComponentInfo();
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void ddlMachine_Footer_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindComponent();
        }
    }
    
}