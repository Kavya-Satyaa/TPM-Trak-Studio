using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.PradeepMetals
{
    public partial class ComputerCellAssociation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserName"] == null || Session["connectionString"] == null)
                    Response.Redirect("../SignIn.aspx", false);
                BindComputerName();
                BindPlantID();
                BindCellID();
                BindData();
            }
        }

        private void BindComputerName()
        {
            List<string> list = DBAccessPradeepMetals.GetComputerNames();

            StringBuilder builder = new System.Text.StringBuilder();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    builder.Append(String.Format("<option value='{0}'>", list[i].ToString()));
                }
            }
            dlComputerName.InnerHtml = builder.ToString();
        }


        private void BindPlantID()
        {
            try
            {
                List<string> list = DBAccessPradeepMetals.GetPlantID();
                if (list.Count > 0)
                {
                    list.Insert(0, "All");
                }
                ddlPlantID.DataSource = list;
                ddlPlantID.DataBind();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindCellID()
        {
            try
            {
                List<string> list = DBAccessPradeepMetals.GetCellID(ddlPlantID.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantID.SelectedValue);
                if (list.Count > 0)
                {
                    list.Insert(0, "All");
                }
                ddlCellID.DataSource = list;
                ddlCellID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindData()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DBAccessPradeepMetals.GetComputerCellAssociationData(txtComputerName.Text.Trim(), ddlPlantID.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantID.SelectedValue, ddlCellID.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCellID.SelectedValue);
                gvCellInfo.DataSource = dt;
                gvCellInfo.DataBind();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void BtnView_Click(object sender, EventArgs e)
        {
            try
            {
                BindData();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            bool updated = false;
            try
            {
                foreach(GridViewRow row in gvCellInfo.Rows)
                {
                    string PlantID = (row.FindControl("lblPlantID") as Label).Text.Trim();
                    string CellID = (row.FindControl("lblCellID") as Label).Text.Trim();
                    if ((row.FindControl("chkAssigned") as CheckBox).Checked)
                    {
                        updated = DBAccessPradeepMetals.UpdateComputerCellAssociationSettings(txtComputerName.Text.Trim(), PlantID, CellID, "Insert");
                    }
                    else
                    {
                        updated = DBAccessPradeepMetals.UpdateComputerCellAssociationSettings(txtComputerName.Text.Trim(), PlantID, CellID, "delete");
                    }
                }
                if (updated)
                {
                    HelperClassGeneric.openUpdateSuccessModal(this);
                }
                else
                {
                    HelperClassGeneric.openInsertErrorModal(this);
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}