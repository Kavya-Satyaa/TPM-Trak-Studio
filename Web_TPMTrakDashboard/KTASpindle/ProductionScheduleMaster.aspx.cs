using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.KTASpindle
{
    public partial class ProductionScheduleMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindComponent()
        {
            try
            {
                ddlNEComponent.DataSource = DBAccess.getComponentID();
                ddlNEComponent.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindWorkOrderData()
        {
            try
            {
                List<ScheduleMasterEntity> list = DBAccess.getPOScheduleMasterDetails(txtWorkOrderSearch.Text, txtComponentSearch.Text, ddlStatus.SelectedValue);
                gvWorkOrder.DataSource = list;
                gvWorkOrder.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindWorkOrderData = "+ex.Message);
            }
        }
        private void BindWorkOrderRoutingData()
        {
            try
            {
                routingContainer.Visible = true;
                gvWORouting.DataSource = DBAccess.getPOScheduleRoutingDetails(txtNEWorkOrder.Text,ddlNEComponent.SelectedValue);
                gvWORouting.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindWorkOrderRoutingData =" + ex.Message);
            }
        }
        protected void btnWoNew_Click(object sender, EventArgs e)
        {
            try
            {
                headerWO.InnerText = "Add WorkOrder";
                hdnNEStatusWO.Value = "";
                txtNEWorkOrder.Enabled = true;
                txtNEWorkOrderDate.Enabled = true;
                ddlNEComponent.Enabled = true;
                txtNEWorkOrder.Text = "";
                txtNEWorkOrderQty.Text = "";
                txtNEWorkOrderDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                routingContainer.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openModal", "openModal('addEditInfo');", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void lblWOEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as GridViewRow).RowIndex);
                headerWO.InnerText = "Edit WorkOrder";
                hdnNEStatusWO.Value = "Edit";
                txtNEWorkOrder.Enabled = false;
                txtNEWorkOrderDate.Enabled = false;
                ddlNEComponent.Enabled = false;
                txtNEWorkOrder.Text = (gvWorkOrder.Rows[rowIndex].FindControl("lblWorkOrderNumber") as Label).Text;
                txtNEWorkOrderQty.Text = (gvWorkOrder.Rows[rowIndex].FindControl("lblQuantity") as Label).Text;
                txtNEWorkOrderDate.Text = (gvWorkOrder.Rows[rowIndex].FindControl("lblWorkOrderDate") as Label).Text;
                string value = (gvWorkOrder.Rows[rowIndex].FindControl("lblComponent") as Label).Text;
                if (ddlNEComponent.Items.FindByValue(value) != null)
                {
                    ddlNEComponent.SelectedValue = value;
                }
                BindWorkOrderRoutingData();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openModal", "openModal('addEditInfo');", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void lblWODelete_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }



        protected void btnWOSave_Click(object sender, EventArgs e)
        {
            try
            {
                ScheduleMasterEntity data = new ScheduleMasterEntity();
                data.WorkOrder = txtNEWorkOrder.Text;
                data.WorkOrderDate = txtNEWorkOrderDate.Text;
                data.Quantity = txtNEWorkOrderQty.Text;
                data.Component = ddlNEComponent.SelectedValue;
               
                string Success = DBAccess.insertUpdatePOScheduleMasterDetails(data, hdnNEStatusWO.Value);
                if (Success == "Exist")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "openModal", "openModal('addEditInfo');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "openWarning", "openWarningModal('Component ID already exists.');", true);
                    return;
                }
                else if (Success == "Inserted")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "successMsg", "showSuccessMsg('Record saved Successfully.','');", true);
                }
                else if (Success == "Updated")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "successMsg", "showSuccessMsg('Record updated Successfully.','');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "openModal", "openModal('addEditInfo');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "openErrorModal('Failed to insert record.');", true);
                    return;
                }
                if (routingContainer.Visible)
                {
                    DBAccess.deleteRoutingDetails(txtNEWorkOrder.Text, ddlNEComponent.SelectedValue);
                    for (int i = 0; i < gvWORouting.Rows.Count; i++)
                    {
                        var row = gvWORouting.Rows[i];
                        if ((row.FindControl("chkSelect") as CheckBox).Checked)
                        {
                            data = new ScheduleMasterEntity();
                            data.WorkOrder = txtNEWorkOrder.Text;
                            data.Component = (row.FindControl("lblComponent") as Label).Text;
                            data.Machine = (row.FindControl("lblMachine") as Label).Text;
                            data.CompInterfaceId = (row.FindControl("hdnCompInterfaceID") as HiddenField).Value;
                            data.OpnInterfaceId = (row.FindControl("hdnOpnInterfaceID") as HiddenField).Value;
                            data.Operation = (row.FindControl("lblOperation") as Label).Text;
                            data.Quantity = (row.FindControl("txtQuantity") as TextBox).Text;
                            Success = DBAccess.insertUpdatePOScheduleRoutingDetails(data, hdnNEStatusWO.Value);
                        }
                    }
                }
                BindWorkOrderRoutingData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}