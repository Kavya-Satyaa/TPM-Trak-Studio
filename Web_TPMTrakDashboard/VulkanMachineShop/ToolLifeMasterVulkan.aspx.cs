using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.VulkanMachineShop.Model;

namespace Web_TPMTrakDashboard.VulkanMachineShop
{
    public partial class ToolLifeMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["MachinesList"] = null;
                Session["InterfaceIDList"] = null;
                BindMachineID();
                BindToolLifeData();
            }
        }

        private void BindMachineID()
        {
            try
            {
                List<string> list = VulkanMSDBAccess.GetMachineID();
                ddlMachineID.DataSource = list;
                ddlMachineID.DataBind();
                ddlMachineID.Items.Insert(0, new ListItem { Text = "All", Value = "" });
                Session["MachinesList"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindMachineID: {ex.Message}");
            }
        }

        private void BindToolLifeData()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = VulkanMSDBAccess.GetToolLifeMasterData(ddlMachineID.SelectedValue, txtToolNo.Text.Trim());
                if (!(dt.Rows.Count > 0))
                    hdnNew.Value = "New";
                else
                    hdnNew.Value = "";
                lvToolLifeMaster.DataSource = dt;
                lvToolLifeMaster.DataBind();
                Session["InterfaceIDList"] = dt.AsEnumerable().Select(x => x["InterfaceID"].ToString()).Distinct().ToList();

                var ddlMachine = (lvToolLifeMaster.InsertItem.FindControl("ddlMachineID") as DropDownList);
                if (ddlMachine != null)
                {
                    if (Session["MachinesList"] != null)
                    {
                        ddlMachine.DataSource = Session["MachinesList"] as List<string>;
                        ddlMachine.DataBind();
                        if (!string.IsNullOrEmpty(ddlMachineID.SelectedValue))
                            ddlMachine.SelectedValue = ddlMachineID.SelectedValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindToolLifeData: {ex.Message}");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ToolLifeEntity data = new ToolLifeEntity();
            int result = 0, result_Update = 0, UpdatedRows = 0;
            string DuplicateInterfaceID = "";
            try
            {
                if (hdnNew.Value.Equals("New", StringComparison.OrdinalIgnoreCase))
                {
                    data = new ToolLifeEntity();
                    data.MachineID = (lvToolLifeMaster.InsertItem.FindControl("ddlMachineID") as DropDownList).SelectedValue.Trim();
                    data.ToolNo = (lvToolLifeMaster.InsertItem.FindControl("txtToolNo") as TextBox).Text.Trim();
                    data.InterfaceID = (lvToolLifeMaster.InsertItem.FindControl("txtInterfaceID") as TextBox).Text.Trim();
                    data.ToolType = (lvToolLifeMaster.InsertItem.FindControl("txtToolType") as TextBox).Text.Trim();
                    data.ToolSpecification = (lvToolLifeMaster.InsertItem.FindControl("txtToolSpec") as TextBox).Text.Trim();
                    data.ToolFeed = (lvToolLifeMaster.InsertItem.FindControl("txtToolFeed") as TextBox).Text.Trim();
                    data.NoOfEdges = (lvToolLifeMaster.InsertItem.FindControl("txtNoOfEdges") as TextBox).Text.Trim();
                    data.ToolLifeInMeter = (lvToolLifeMaster.InsertItem.FindControl("txtToolLifeInMeter") as TextBox).Text.Trim();

                    result += VulkanMSDBAccess.SaveToolLifeMasterData(data, out DuplicateInterfaceID);

                    if (result > 0)
                        HelperClassGeneric.openSuccessModal(this, "Added Successfully.");
                    else
                        HelperClassGeneric.openWarningToastrModal(this, "Insertion Failed. Try Again!");
                }
                foreach (ListViewDataItem item in lvToolLifeMaster.Items)
                {
                    if ((item.FindControl("hdnUpdate") as HiddenField).Value.Trim().Equals("Update", StringComparison.OrdinalIgnoreCase))
                    {
                        UpdatedRows++;
                        data = new ToolLifeEntity();
                        data.MachineID = (item.FindControl("lblMachineID") as Label).Text.Trim();
                        data.ToolNo = (item.FindControl("lblToolNo") as Label).Text.Trim();
                        data.InterfaceID = (item.FindControl("txtInterfaceID") as TextBox).Text.Trim();
                        data.ToolType = (item.FindControl("txtToolType") as TextBox).Text.Trim();
                        data.ToolSpecification = (item.FindControl("txtToolSpec") as TextBox).Text.Trim();
                        data.ToolFeed = (item.FindControl("txtToolfeed") as TextBox).Text.Trim();
                        data.NoOfEdges = (item.FindControl("txtNoOfEdges") as TextBox).Text.Trim();
                        data.ToolLifeInMeter = (item.FindControl("txtToolLifeInMeter") as TextBox).Text.Trim();

                        result_Update += VulkanMSDBAccess.SaveToolLifeMasterData(data, out DuplicateInterfaceID);

                        if (string.IsNullOrEmpty(DuplicateInterfaceID))
                            DuplicateInterfaceID = data.InterfaceID;
                        else
                            DuplicateInterfaceID += ", " + data.InterfaceID;

                    }
                }
                if (!hdnNew.Value.Equals("New", StringComparison.OrdinalIgnoreCase))
                {
                    if (UpdatedRows == 0)
                        HelperClassGeneric.openWarningModal(this, "No records are Updated on Page.");
                    else if (result_Update == UpdatedRows)
                        HelperClassGeneric.openSuccessModal(this, "Updated Successfully.");
                    else if (!string.IsNullOrEmpty(DuplicateInterfaceID))
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertDuplicateInterfaceID", "alert('Duplicate Interface ID Found: " + DuplicateInterfaceID + "');", true);
                    else
                        HelperClassGeneric.openWarningModal(this, "Erro! Try Again.");
                }
                BindToolLifeData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"btnSave_Click: {ex.Message}");
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindToolLifeData();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int deletedRows = 0, RowsToDelete = 0;
            string MachineID = "", ToolNo = "";
            try
            {
                foreach (ListViewDataItem item in lvToolLifeMaster.Items)
                {
                    if ((item.FindControl("chkDelete") as CheckBox).Checked)
                    {
                        RowsToDelete++;
                        MachineID = (item.FindControl("lblMachineID") as Label).Text.Trim();
                        ToolNo = (item.FindControl("lblToolNo") as Label).Text.Trim();

                        deletedRows += VulkanMSDBAccess.DeleteToolLifeMasterData(MachineID, ToolNo);
                    }
                }
                if (RowsToDelete == deletedRows)
                    HelperClassGeneric.openDeleteSuccessModal(this);
                else
                    HelperClassGeneric.openWarningToastrModal(this, "Delete Falied. Try Again!");
                BindToolLifeData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"btnDelete_Click: {ex.Message}");
            }
        }
    }
}