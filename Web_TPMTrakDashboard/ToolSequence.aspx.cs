using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class ToolSequence : System.Web.UI.Page
    {
        public static int deleteRowIndex = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachineID();
                btnView_Click(null, null);
            }
        }
        private void BindMachineID()
        {
            try
            {
                List<string> machine = DataBaseAccess.GetAllEnabledMachines();
                ddlMachineID.DataSource = machine;
                ddlMachineID.DataBind();
                BindComponentID();
            }
            catch (Exception ex)
            {

            }
        }
        private void BindComponentID()
        {
            try
            {
                List<string> list = DataBaseAccess.GetComponentIDByMachine(ddlMachineID.SelectedValue);
                ddlComponentID.DataSource = list;
                ddlComponentID.DataBind();
                BindOperationNumber();
            }
            catch (Exception ex)
            {

            }
        }
        private void BindOperationNumber()
        {
            try
            {
                List<string> list = DataBaseAccess.GetOperation(ddlMachineID.SelectedValue, ddlComponentID.SelectedValue);
                ddlOperationNo.DataSource = list;
                ddlOperationNo.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        protected void ddlMachineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindComponentID();
        }

        protected void ddlComponentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOperationNumber();
        }
        private void BindToolData()
        {
            try
            {
                List<ToolSequanceData> list = DataBaseAccess.getToolSequenceDetails(ddlMachineID.SelectedValue, ddlComponentID.SelectedValue, ddlOperationNo.SelectedValue);
                lvToolData.DataSource = list;
                lvToolData.DataBind();
                var insertItem = lvToolData.InsertItem;

                if(System.Web.Configuration.WebConfigurationManager.AppSettings["VulkanMachineShopPages"].ToString() != "1")
                {
                    insertItem.FindControl("tdToolGPLNew").Visible = false;
                    insertItem.FindControl("tdDepthOfCutNew").Visible = false;
                    insertItem.FindControl("tdFeedMMMinNew").Visible = false;
                    insertItem.FindControl("tdFeedToothNew").Visible = false;
                    insertItem.FindControl("tdNoOfCuttingEdgesNew").Visible = false;
                    insertItem.FindControl("tdNoOfCutNew").Visible = false;

                    lvToolData.FindControl("thToolGPL").Visible = false;
                    lvToolData.FindControl("thDepthOfCut").Visible = false;
                    lvToolData.FindControl("thFeedMMMin").Visible = false;
                    lvToolData.FindControl("thFeedTooth").Visible = false;
                    lvToolData.FindControl("thNoOfCuttingEdges").Visible = false;
                    lvToolData.FindControl("thNoOfCut").Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindToolData();
        }

        protected void gvToolData_RowEditing(object sender, GridViewEditEventArgs e)
        {
            lvToolData.EditIndex = e.NewEditIndex;
            BindToolData();
        }

        protected void gvToolData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            
        }

        protected void gvToolData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            var Item = lvToolData.InsertItem;
            if (Item != null)
            {
                ToolSequanceData data = new ToolSequanceData();
                data.MachineID = ddlMachineID.SelectedValue;
                data.ComponentID = ddlComponentID.SelectedValue;
                data.OperationNumber = ddlOperationNo.SelectedValue;
                data.ToolNumber = (Item.FindControl("txtToolNoNew") as TextBox).Text;
                data.ToolDescription = (Item.FindControl("txtToolDescNew") as TextBox).Text;
                data.IdealUsage = (Item.FindControl("txtIdealUsageNew") as TextBox).Text;
                data.Offset = (Item.FindControl("txtOffsetNew") as TextBox).Text;
                data.ToolHolder = (Item.FindControl("txtToolHolderNew") as TextBox).Text;
                data.RPM = (Item.FindControl("txtRPMNew") as TextBox).Text;
                data.Target = (Item.FindControl("txtTargetNew") as TextBox).Text;
                data.DownCode = (Item.FindControl("txtDowncodeNew") as TextBox).Text;
                data.Notes = (Item.FindControl("txtNotesNew") as TextBox).Text;

                if (System.Web.Configuration.WebConfigurationManager.AppSettings["VulkanMachineShopPages"].ToString() == "1")
                {

                    data.ToolGPL = (Item.FindControl("txtToolGPLNew") as TextBox).Text;
                    data.DepthOfCut = (Item.FindControl("txtDepthOfCutNew") as TextBox).Text;
                    data.FeedMM_Min = (Item.FindControl("txtFeedMMMinNew") as TextBox).Text;
                    data.FeedTooth = (Item.FindControl("txtFeedToothNew") as TextBox).Text;
                    data.NoOfCuttingEdges = (Item.FindControl("txtNoOfCuttingEdgesNew") as TextBox).Text;
                    data.NoOfCut = (Item.FindControl("txtNoOfCutNew") as TextBox).Text;
                }
                data.Sequence = "-1";
                if (data.MachineID.Trim() == "" || data.MachineID == null)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please select Machine ID.')", true);
                    return;
                }
                if (data.ComponentID.Trim() == "" || data.ComponentID == null)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please select Component ID.')", true);
                    return;
                }
                if (data.OperationNumber.Trim() == "" || data.OperationNumber == null)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please select Operation Number.')", true);
                    return;
                }
                if (data.ToolNumber.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please enter Tool Number.')", true);
                    return;
                }
                if (data.ToolDescription.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please enter Tool Description.')", true);
                    return;
                }
                if (data.IdealUsage.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please enter Ideal Usage.')", true);
                    return;
                }
                if (data.Target.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please enter Target.')", true);
                    return;
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["VulkanMachineShopPages"].ToString() == "1")
                {

                    if (data.ToolGPL.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please enter Tool GPL.')", true);
                        return;
                    }
                    if (data.DepthOfCut.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please enter Depth of Cut.')", true);
                        return;
                    }
                    if (data.FeedMM_Min.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please enter Feed MM/Min.')", true);
                        return;
                    }
                    if (data.FeedTooth.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please enter Feed Tooth.')", true);
                        return;
                    }
                    if (data.NoOfCuttingEdges.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please enter No of Cutting Edge.')", true);
                        return;
                    }
                    if (data.NoOfCut.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please enter No of Cut.')", true);
                        return;
                    }
                }

                int res = DataBaseAccess.insertOrUpdateToolSequenceDetails(data);
                BindToolData();
                if (res == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openErrorModal('Insertion failed.')", true);
                    return;
                }
            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                deleteRowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewDataItem).DataItemIndex);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openCloseGridConfirmModal('show')", true);
            }
            catch (Exception ex) { }
        }
        protected void yesGridBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ListViewDataItem row = lvToolData.Items[deleteRowIndex];
                ToolSequanceData data = new ToolSequanceData();
                data.MachineID = (row.FindControl("hdnMachineID") as HiddenField).Value;
                data.ComponentID = (row.FindControl("hdnCompID") as HiddenField).Value;
                data.OperationNumber = (row.FindControl("hdnOperationNumber") as HiddenField).Value;
                data.Sequence = (row.FindControl("lblSequence") as Label).Text;
                data.ToolNumber = (row.FindControl("lblToolNo") as Label).Text;
                int res = DataBaseAccess.deleteToolSequenceDetails(data);
                BindToolData();
                if (res == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openErrorModal('Deletion failed.')", true);
                    return;
                }
            }
            catch (Exception ex) { }
        }

        protected void lvToolData_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lvToolData.EditIndex = e.NewEditIndex;
            BindToolData();
        }

        protected void lvToolData_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            try
            {
                ToolSequanceData data = new ToolSequanceData();
                data.MachineID = (lvToolData.Items[e.ItemIndex].FindControl("hdnMachineID") as HiddenField).Value;
                data.ComponentID = (lvToolData.Items[e.ItemIndex].FindControl("hdnCompID") as HiddenField).Value;
                data.OperationNumber = (lvToolData.Items[e.ItemIndex].FindControl("hdnOperationNumber") as HiddenField).Value;
                data.Sequence = (lvToolData.Items[e.ItemIndex].FindControl("lblSequence") as Label).Text;
                data.ToolNumber = (lvToolData.Items[e.ItemIndex].FindControl("lblToolNo") as Label).Text;
                data.ToolDescription = (lvToolData.Items[e.ItemIndex].FindControl("txtToolDescEdit") as TextBox).Text;
                data.IdealUsage = (lvToolData.Items[e.ItemIndex].FindControl("txtIdealUsageEdit") as TextBox).Text;
                data.Offset = (lvToolData.Items[e.ItemIndex].FindControl("txtOffsetEdit") as TextBox).Text;
                data.ToolHolder = (lvToolData.Items[e.ItemIndex].FindControl("txtToolHolderEdit") as TextBox).Text;
                data.RPM = (lvToolData.Items[e.ItemIndex].FindControl("txtRPMEdit") as TextBox).Text;
                data.Target = (lvToolData.Items[e.ItemIndex].FindControl("txtTargetEdit") as TextBox).Text;
                data.DownCode = (lvToolData.Items[e.ItemIndex].FindControl("txtDowncodeEdit") as TextBox).Text;
                data.Notes = (lvToolData.Items[e.ItemIndex].FindControl("txtNotesEdit") as TextBox).Text;

                if (System.Web.Configuration.WebConfigurationManager.AppSettings["VulkanMachineShopPages"].ToString() == "1")
                {

                    data.ToolGPL = (lvToolData.Items[e.ItemIndex].FindControl("txtToolGPLEdit") as TextBox).Text;
                    data.DepthOfCut = (lvToolData.Items[e.ItemIndex].FindControl("txtDepthOfCutEdit") as TextBox).Text;
                    data.FeedMM_Min = (lvToolData.Items[e.ItemIndex].FindControl("txtFeedMMMinEdit") as TextBox).Text;
                    data.FeedTooth = (lvToolData.Items[e.ItemIndex].FindControl("txtFeedToothEdit") as TextBox).Text;
                    data.NoOfCuttingEdges = (lvToolData.Items[e.ItemIndex].FindControl("txtNoOfCuttingEdgesEdit") as TextBox).Text;
                    data.NoOfCut = (lvToolData.Items[e.ItemIndex].FindControl("txtNoOfCutEdit") as TextBox).Text;
                }

                if (data.MachineID.Trim() == "" || data.MachineID == null)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please select Machine ID.')", true);
                    return;
                }
                if (data.ComponentID.Trim() == "" || data.ComponentID == null)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please select Component ID.')", true);
                    return;
                }
                if (data.OperationNumber.Trim() == "" || data.OperationNumber == null)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please select Operation Number.')", true);
                    return;
                }
                if (data.ToolNumber.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please enter Tool Number.')", true);
                    return;
                }
                if (data.ToolDescription.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please enter Tool Description.')", true);
                    return;
                }
                if (data.IdealUsage.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please enter Ideal Usage.')", true);
                    return;
                }
                if (data.Target.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openWarningModal('Please enter Target.')", true);
                    return;
                }
                int res = DataBaseAccess.insertOrUpdateToolSequenceDetails(data);
                lvToolData.EditIndex = -1;
                BindToolData();
                if (res == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DelRow", "openErrorModal('Updation failed.')", true);
                    return;
                }
                else
                {
                    HelperClassGeneric.openSuccessModal(this, "Updated Successfuly.");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void lvToolData_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lvToolData.EditIndex = -1;
            BindToolData();
        }
    }
}