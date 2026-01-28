using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik184.Models;

namespace Web_TPMTrakDashboard.Advik184
{
    public partial class ParameterMasterAdvik : System.Web.UI.Page
    {
        private static int selectedIDD = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachineID();
                BindData();
            }
        }
        private void BindMachineID()
        {
            try
            {
                List<string> list = AdvikDatabaseAccess.getMachineFromTwoTable();
                ddlMachine.DataSource = list.Where(k => !k.ToLower().Contains("laser")).ToList();
                ddlMachine.DataBind();
                BindComponentId();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindComponentId()
        {
            try
            {

                ddlComponent.DataSource = Web_TPMTrakDashboard.Models.DataBaseAccess.GetComponentIDByMachine(getMachineId());
                ddlComponent.DataBind();
                BindOperationNo();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindOperationNo()
        {
            try
            {
                List<string> list = new List<string>();
                if (ddlMachine.SelectedValue.ToLower().Contains("final"))
                {
                    list.Add("100");
                }
                else
                {
                    list = Web_TPMTrakDashboard.Models.DataBaseAccess.GetOperation(ddlMachine.SelectedValue, ddlComponent.SelectedValue);
                }
                ddlOperation.DataSource = list;
                ddlOperation.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private string getMachineId()
        {
            string machine = ddlMachine.SelectedValue;
            try
            {
                if (ddlMachine.SelectedValue.ToLower().Contains("final"))
                {
                    for (int i = 0; i < ddlMachine.Items.Count; i++)
                    {
                        if (ddlMachine.Items[i].Value.ToLower().Contains("rpm"))
                        {
                            machine = ddlMachine.Items[i].Value;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return machine;
        }
        protected void ddlMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindComponentId();
        }

        protected void ddlComponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOperationNo();
        }

        private void BindData()
        {
            try
            {
                List<ParameterMasterEntity> list = AdvikDatabaseAccess.getParameterMasterData(ddlMachine.SelectedValue, ddlComponent.SelectedValue, ddlOperation.SelectedValue);
                int flag = 0;
                if (list.Count == 0)
                {
                    list.Add(new ParameterMasterEntity());
                    flag = 1;
                }
                gvParameter.DataSource = list;
                gvParameter.DataBind();
                if (flag == 1)
                {
                    gvParameter.Rows[0].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }



        protected void lnkInsert_Click(object sender, EventArgs e)
        {
            try
            {
                ParameterMasterEntity data = new ParameterMasterEntity();
                data.MachineID = ddlMachine.SelectedValue;
                data.ComponentID = ddlComponent.SelectedValue;
                data.OperationNo = ddlOperation.SelectedValue;
                data.ParameterID = (gvParameter.FooterRow.FindControl("txtParameterID") as TextBox).Text;
                data.ParameterName = (gvParameter.FooterRow.FindControl("txtParameterName") as TextBox).Text;
                data.LSL = (gvParameter.FooterRow.FindControl("txtLSL") as TextBox).Text;
                data.USL = (gvParameter.FooterRow.FindControl("txtUSL") as TextBox).Text;
                data.Unit = (gvParameter.FooterRow.FindControl("txtUnit") as TextBox).Text;
                data.RegisterAddress = (gvParameter.FooterRow.FindControl("txtRegisterAddress") as TextBox).Text;
                data.MinRegAdd = (gvParameter.FooterRow.FindControl("txtMinRegisterAddress") as TextBox).Text;
                data.MaxRegAdd = (gvParameter.FooterRow.FindControl("txtMaxRegisterAddress") as TextBox).Text;
                data.ID = "-1";
                string result = AdvikDatabaseAccess.saveParameterMasterData(data);
                if (result.Equals("Save", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Saved Successfully.');", true);
                }
                else if (result.Equals("RegisterAddress Exists", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openWarningModal('RegisterAddress already exists.');", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openErrorModal('Insertion failed.');", true);
                    return;
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lnkInsert_Click = " + ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int flag = 0;
                for (int i = 0; i < gvParameter.Rows.Count; i++)
                {
                    var row = gvParameter.Rows[i];
                    if ((row.FindControl("hdnUpdate") as HiddenField).Value.Equals("update", StringComparison.OrdinalIgnoreCase))
                    {

                        (row.FindControl("hdnUpdate") as HiddenField).Value = "";
                        ParameterMasterEntity data = new ParameterMasterEntity();
                        data.MachineID = (row.FindControl("hdnMachineID") as HiddenField).Value;
                        data.ComponentID = (row.FindControl("hdnComponentID") as HiddenField).Value;
                        data.OperationNo = (row.FindControl("hdnOperationNo") as HiddenField).Value;
                        data.ParameterID = (row.FindControl("lblParameterID") as Label).Text;
                        data.ParameterName = (row.FindControl("txtParameterName") as TextBox).Text;
                        data.LSL = (row.FindControl("txtLSL") as TextBox).Text;
                        data.USL = (row.FindControl("txtUSL") as TextBox).Text;
                        data.Unit = (row.FindControl("txtUnit") as TextBox).Text;
                        data.RegisterAddress = (row.FindControl("txtRegisterAddress") as TextBox).Text;
                        data.ID = (row.FindControl("hdnIDD") as HiddenField).Value;
                        data.MinRegAdd = (row.FindControl("txtMinRegisterAddress") as TextBox).Text;
                        data.MaxRegAdd = (row.FindControl("txtMaxRegisterAddress") as TextBox).Text;
                        string result = AdvikDatabaseAccess.saveParameterMasterData(data);
                        if (result.Equals("Save", StringComparison.OrdinalIgnoreCase))
                        {
                            flag++;
                        }
                        else if (result.Equals("RegisterAddress Exists", StringComparison.OrdinalIgnoreCase))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openWarningModal('RegisterAddress " + data.RegisterAddress + " already exists.')", true);
                            return;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openErrorModal('Insertion failed.')", true);
                            return;
                        }
                    }
                }
                if (flag > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Saved Successfully.')", true);
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnSave_Click = " + ex.Message);
            }
        }
        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            try
            {
                selectedIDD = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "confirmModal", "openConfirmModal('deleteConfirmModal')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lnkDelete_Click = " + ex.Message);
            }
        }
        protected void btnDeleteConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string idd = (gvParameter.Rows[selectedIDD].FindControl("hdnIDD") as HiddenField).Value;
                int result = AdvikDatabaseAccess.deleteParameterMasterData(idd);
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Deleted Successfully.')", true);
                }
                selectedIDD = -1;
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnDeleteConfirm_Click = " + ex.Message);
            }
        }
    }
}