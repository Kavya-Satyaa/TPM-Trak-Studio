using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik184.Models;

namespace Web_TPMTrakDashboard.Advik184
{
    public partial class ModelMasterAdvik : System.Web.UI.Page
    {
        private static int selectedIDD = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            try
            {
                List<ModelMasterEntity> list = AdvikDatabaseAccess.getModelMasterData();
                int flag = 0;
                if (list.Count == 0)
                {
                    list.Add(new ModelMasterEntity());
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
                ModelMasterEntity data = new ModelMasterEntity();
                data.ModelID = (gvParameter.FooterRow.FindControl("txtModel") as TextBox).Text;
                data.ModelName = (gvParameter.FooterRow.FindControl("txtModelName") as TextBox).Text;
                data.ID = "-1";
                string result = AdvikDatabaseAccess.saveModelMasterData(data);
                if (result.Equals("Save", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Saved Successfully.')", true);
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
                        ModelMasterEntity data = new ModelMasterEntity();
                        data.ModelID = (row.FindControl("lblModel") as Label).Text;
                        data.ModelName = (row.FindControl("txtModelName") as TextBox).Text;
                        data.ID = (row.FindControl("hdnIDD") as HiddenField).Value;
                        string result = AdvikDatabaseAccess.saveModelMasterData(data);
                        if (result.Equals("Save", StringComparison.OrdinalIgnoreCase))
                        {
                            flag++;
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
                int result = AdvikDatabaseAccess.deleteModelMasterData(idd);
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