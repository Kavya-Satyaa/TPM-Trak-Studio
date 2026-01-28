using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.KTASpindle;

namespace Web_TPMTrakDashboard
{
    public partial class PartFamilyMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindData(txtPartFamilysSearch.Text);
            }
        }

        private void BindData(string partfam)
        {
            try
            {
                DataTable dt = DBAccess.GetPartFamilyDetails(partfam);
                if(dt!=null  && dt.Rows.Count>0)
                {
                    lvPartFamilyMasterDetails.DataSource = dt;
                    lvPartFamilyMasterDetails.DataBind();
                }
                else
                {
                    lvPartFamilyMasterDetails.DataSource = new DataTable();
                    lvPartFamilyMasterDetails.DataBind();
                }
                Session["PartFamDetails"] = dt;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                txtPartFamID.Text = string.Empty;
                txtPartDesc.Text = string.Empty;

                txtPartFamID.Enabled = true;

                hfPartFamNewEdit.Value = "New";
                btnPartFamDetailsSave.Text = "ADD";
                modalTitle.InnerText = "Add Part Family Details";

                HelperClass.openAddEditModal(this, "neweditPartFamModal");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnAddPartFamilyMasterDetails_Click: " + ex.Message);
            }
        }

        protected void lbEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewDataItem).DataItemIndex);              
                txtPartFamID.Text = (lvPartFamilyMasterDetails.Items[rowIndex].FindControl("lblPartFamID") as Label).Text;
                txtPartDesc.Text = (lvPartFamilyMasterDetails.Items[rowIndex].FindControl("lblPartFamDesc") as Label).Text;
                
                txtPartFamID.Enabled = false;

                hfPartFamNewEdit.Value = "Edit";
                btnPartFamDetailsSave.Text = "UPDATE";
                modalTitle.InnerText = "Edit Part Family Details";

                HelperClass.openAddEditModal(this, "neweditPartFamModal");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lbEdit_Click: " + ex.Message);
            }
        }

        protected void lbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["DeleteRowIndex"] = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewDataItem).DataItemIndex);
                string parametr = (lvPartFamilyMasterDetails.Items[(int)ViewState["DeleteRowIndex"]].FindControl("lblPartFamID") as Label).Text;
                string deletemsg = "Are you sure, you want to delete Part No.:" + parametr;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmModal", "openDeleteConfirmModal('" + deletemsg + "')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lbDelete_Click: " + ex.Message);
            }
        }

        protected void txtPartFamilysSearch_TextChanged(object sender, EventArgs e)
        {
            BindData(txtPartFamilysSearch.Text);
        }

        protected void btnPartFamDetailsSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = Session["PartFamDetails"] as DataTable;
                if(btnPartFamDetailsSave.Text.Equals("ADD",StringComparison.OrdinalIgnoreCase))
                {
                    if (dt.AsEnumerable().Where(x => x.Field<string>("PartFamily").Equals(txtPartFamID.Text)).Count() > 0)
                    {
                        HelperClass.openModal(this, "neweditPartFamModal", false);
                        HelperClass.openWarningModal(this, "Part Family ID already exists!!");
                        txtPartFamID.Text = string.Empty;
                        txtPartDesc.Text = string.Empty;
                        return;
                    }
                }
                
                int success = DBAccess.SavePartFamilyMasterDetails(txtPartFamID.Text,txtPartDesc.Text,hfPartFamNewEdit.Value,"Save");
                if (success > 0 )
                {
                    HelperClass.openInsertSuccessModal(this);
                    HelperClass.clearModal(this);
                }
                else
                {
                    HelperClass.openModal(this, "neweditPartFamModal", false);
                    HelperClass.openInsertErrorModal(this);
                    return;
                }
                BindData(txtPartFamilysSearch.Text);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnPartDetailsSave_Click: " + ex.Message);
            }
        }

        protected void btnDeleteConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = (int)ViewState["DeleteRowIndex"];
                string PartFamID = (lvPartFamilyMasterDetails.Items[rowIndex].FindControl("lblPartFamID") as Label).Text;
                int success = DBAccess.SavePartFamilyMasterDetails(PartFamID,"","", "Delete");
                if (success >0)
                {
                    HelperClass.openDeleteSuccessModal(this);
                    HelperClass.clearModal(this);
                }
                else
                {
                    HelperClass.clearModal(this);
                    HelperClass.openDeleteErrorModal(this);
                    return;
                }
                BindData(txtPartFamilysSearch.Text);
                ViewState["DeleteRowIndex"] = -1;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnDeleteConfirm_Click: " + ex.Message);
            }
        }
    }
}