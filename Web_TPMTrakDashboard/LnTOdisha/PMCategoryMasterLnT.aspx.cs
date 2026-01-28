using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.LnTOdisha.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.LnTOdisha
{
    public partial class PMCategoryMasterLnT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindCategoryData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Page_Load:" + ex.Message);
            }
        }

        private void BindCategoryData()
        {
            DataTable dt = new DataTable();
            bool IsDataAvailable = true;
            try
            {
                dt = LnTOdishaDBAccess.GetPMCategoryMasterData();
                if (dt.Rows.Count == 0)
                {
                    dt.Columns.Add("PMCategoryName", typeof(string));
                    dt.Columns.Add("SortOrder", typeof(string));

                    var newRow = dt.NewRow();
                    newRow["PMCategoryName"] = "";
                    newRow["SortOrder"] = "";
                    dt.Rows.Add(newRow);
                    IsDataAvailable = false;
                }
                gvPMcategoryMaster.DataSource = dt;
                gvPMcategoryMaster.DataBind();

                if (!IsDataAvailable)
                    gvPMcategoryMaster.Rows[0].Visible = false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindCategoryData: {ex.Message}");
            }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            string isDeleted = "";
            try
            {
                string CategoryID = (((sender as LinkButton).NamingContainer as GridViewRow).FindControl("lblPMCategory") as Label).Text.Trim();
                isDeleted = LnTOdishaDBAccess.DeletePMCategoryMaster(CategoryID);
                if (isDeleted.Equals("Deleted", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openDeleteSuccessModal(this);
                else
                    HelperClassGeneric.openDeleteErrorModal(this);
                BindCategoryData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"lnkDelete_Click: {ex.Message}");
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string isSaved = "";
            try
            {
                var footerRow = gvPMcategoryMaster.FooterRow;
                 isSaved = LnTOdishaDBAccess.SavePMCateogryMasterData((footerRow.FindControl("txtPMCategory") as TextBox).Text.Trim(), (footerRow.FindControl("txtSortOrder") as TextBox).Text.Trim());
                if (isSaved.Equals("Inserted/Updated", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openSuccessModal(this, "Saved Successfully.");
                else
                    HelperClassGeneric.openWarningToastrModal(this, "Save Failed. TRY Agian!");
                BindCategoryData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"btnAdd_Click: {ex.Message}");
            }
        }
    }
}