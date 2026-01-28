using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Pooja.Model;

namespace Web_TPMTrakDashboard.Pooja
{
    public partial class PoojaParameterMaster : System.Web.UI.Page
    {
        private static int deleteRowIndex = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnCancel.Visible = false;
                gvParameterList.ShowFooter = false;
                BindParameterGrid();
            }
        }
        private void BindParameterGrid()
        {
            List<PoojaParameterMasterDetails> list = PoojaDBAccess.GetPoojaParameterMasterDetails();
            if (list.Count > 0)
            {
                gvParameterList.DataSource = list;
                gvParameterList.DataBind();
            }
            else
            {
                list.Add(new PoojaParameterMasterDetails { ParameterID = "", ParameterDesc = "" });
                gvParameterList.DataSource = list;
                gvParameterList.DataBind();
                gvParameterList.Rows[0].Visible = false;
            }

        }

        protected void btnSaveParamSetting_Click(object sender, EventArgs e)
        {
            bool IsSaved = false;
            try
            {
                PoojaParameterMasterDetails data = new PoojaParameterMasterDetails();
                if (gvParameterList.Rows.Count > 0)
                {
                    foreach (GridViewRow row in gvParameterList.Rows)
                    {
                        data.ParameterID = (row.Cells[1].FindControl("lblParameterID") as Label).Text;
                        if (data.ParameterID == "")
                        {
                            //HelperClassGeneric.openWarningModal(this, "Parameter is required");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "NoData", "alert('Parameter is required')", true);
                            return;
                        }
                        data.ParameterDesc = (row.Cells[2].FindControl("txtParamDesc") as TextBox).Text;
                        data.SortOrder = Convert.ToInt32((row.Cells[3].FindControl("txtSortOrder") as TextBox).Text);
                        data.IsEnabled = Convert.ToBoolean((row.Cells[4].FindControl("chkIsEnabled") as CheckBox).Checked);
                        IsSaved = PoojaDBAccess.UpdateParameterSettings(data);
                        if (!IsSaved) break;
                    }
                    BindParameterGrid();
                    HelperClassGeneric.openSuccessModal(this, "Data Saved Successfully.");
                }
                gvParameterList.ShowFooter = false;
                
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void lbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int index = ((sender as LinkButton).NamingContainer as GridViewRow).DataItemIndex;
                ViewState["DeleteRowIndex"] = index;
                deleteRowIndex = index;
                int DeleteRowIndex = deleteRowIndex;
                PoojaParameterMasterDetails data = new PoojaParameterMasterDetails();
                data.ParameterID = (gvParameterList.Rows[DeleteRowIndex].FindControl("lblParameterID") as Label).Text;
                string result = PoojaDBAccess.DeleterPoojaAndonParameter(data);
                if (result == "Deleted")
                {
                    HelperClassGeneric.openSuccessModal(this, "Data Deleted Successfully.");
                }
                else
                {
                    HelperClassGeneric.openErrorModal(this, "Deletion failed");
                    return;
                }
                BindParameterGrid();
                ViewState["DeleteRowIndex"] = -1;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void lbAdd_Click(object sender, EventArgs e)
        {
            try
            {
                PoojaParameterMasterDetails data = new PoojaParameterMasterDetails();
                data.ParameterID = (gvParameterList.FooterRow.FindControl("txtParameterID") as TextBox).Text;
                if (data.ParameterID == "")
                {
                    HelperClassGeneric.openWarningModal(this, "Parameter is required");
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "NoData", "alert('Parameter is required')", true);
                    return;
                }
                data.ParameterDesc = (gvParameterList.FooterRow.FindControl("txtParameterDesc") as TextBox).Text;
                data.SortOrder = Convert.ToInt32((gvParameterList.FooterRow.FindControl("txtSort") as TextBox).Text);
                data.IsEnabled = Convert.ToBoolean((gvParameterList.FooterRow.FindControl("chkIsEnable") as CheckBox).Checked);
                bool IsSaved = PoojaDBAccess.UpdateParameterSettings(data);
                if (!IsSaved)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NoData", "alert('No data is saved.')", true);
                }
                gvParameterList.ShowFooter = false;
                btnCancel.Visible = false;
                btnNew.Visible = true;
                BindParameterGrid();
                HelperClassGeneric.openSuccessModal(this, "Data Added Successfully.");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnNew_ServerClick(object sender, EventArgs e)
        {
            btnNew.Visible = false;
            btnCancel.Visible = true;
            gvParameterList.ShowFooter = true;
            BindParameterGrid();
        }

        protected void btnCancel_ServerClick(object sender, EventArgs e)
        {
            btnCancel.Visible = false;
            btnNew.Visible = true;
            gvParameterList.ShowFooter = false;
            BindParameterGrid();
        }
    }
}