using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik184.Models;

namespace Web_TPMTrakDashboard.Advik184
{
    public partial class PokayokeMasterAdvik184 : System.Web.UI.Page
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
                ddlMachine.DataSource = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllMachines("");
                ddlMachine.DataBind();
                //BindComponentId();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        //private void BindComponentId()
        //{
        //    try
        //    {
        //        ddlComponent.DataSource = DataBaseAccess.GetComponentIDByMachine(ddlMachine.SelectedValue);
        //        ddlComponent.DataBind();
        //        BindOperationNo();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.Message);
        //    }
        //}
        //private void BindOperationNo()
        //{
        //    try
        //    {
        //        ddlOperation.DataSource = DataBaseAccess.GetOperation(ddlMachine.SelectedValue, ddlComponent.SelectedValue);
        //        ddlOperation.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.Message);
        //    }
        //}
        //protected void ddlMachine_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindComponentId();
        //}

        //protected void ddlComponent_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindOperationNo();
        //}

        private void BindData()
        {
            try
            {
                List<PokayokeMasterEntity> list = AdvikDatabaseAccess.getPokayokeMasterData(ddlMachine.SelectedValue, "", "");
                int flag = 0;
                if (list.Count == 0)
                {
                    list.Add(new PokayokeMasterEntity());
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
                PokayokeMasterEntity data = new PokayokeMasterEntity();
                data.MachineID = ddlMachine.SelectedValue;
                //data.ComponentID = ddlComponent.SelectedValue;
                //data.OperationNo = ddlOperation.SelectedValue;
                data.PokayokeID = (gvParameter.FooterRow.FindControl("txtPokayokeID") as TextBox).Text;
                data.PokayokeName = (gvParameter.FooterRow.FindControl("txtPokayokeName") as TextBox).Text;
                data.RegisterID = (gvParameter.FooterRow.FindControl("txtRegisterID") as TextBox).Text;
                data.ID = "-1";
                string result = AdvikDatabaseAccess.savePokayokeMasterData(data);
                if (result.Equals("Save", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Saved Successfully.')", true);
                }
                else if (result.Equals("RegisterID Exists", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openWarningModal('RegisterID already exists.')", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openErrorModal('Insertion failed.')", true);
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
                        PokayokeMasterEntity data = new PokayokeMasterEntity();
                        data.MachineID = (row.FindControl("hdnMachineID") as HiddenField).Value;
                        //data.ComponentID = (row.FindControl("hdnComponentID") as HiddenField).Value;
                        // data.OperationNo = (row.FindControl("hdnOperationNo") as HiddenField).Value;
                        data.PokayokeID = (row.FindControl("lblPokayokeID") as Label).Text;
                        data.PokayokeName = (row.FindControl("txtPokayokeName") as TextBox).Text;
                        data.RegisterID = (row.FindControl("txtRegisterID") as TextBox).Text;
                        data.ID = (row.FindControl("hdnIDD") as HiddenField).Value;
                        string result = AdvikDatabaseAccess.savePokayokeMasterData(data);
                        if (result.Equals("Save", StringComparison.OrdinalIgnoreCase))
                        {
                            flag++;
                        }
                        else if (result.Equals("RegisterID Exists", StringComparison.OrdinalIgnoreCase))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openWarningModal('RegisterAddress " + data.RegisterID + " already exists.')", true);
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
                int result = AdvikDatabaseAccess.deletePokayokeMasterData(idd);
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