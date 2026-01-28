using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class DownCodeLossInfoo : System.Web.UI.Page
    {
        protected int SuccMessage
        {
            get;
            private set;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDownCategory();
                BindDownID();
                BindGrid();
            }
        }
        private void BindGrid()
        {
            try
            {
                List<DownIDSubLossInfoData> list = DownCodeInfoDataBase.GetSubLossMasterData(ddlDownCategory.SelectedValue.ToString(), ddlDownID.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : ddlDownID.SelectedValue.ToString());

                if (list.Count == 0)
                {
                    list.Add(new DownIDSubLossInfoData { DownID = "", SubLossID = "", SubLossDescription = "" });
                    gvGrid.DataSource = list;
                    gvGrid.DataBind();
                    gvGrid.Rows[0].Visible = false;
                }
                else
                {
                    gvGrid.DataSource = list;
                    gvGrid.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindDownCategory()
        {
            try
            {
                List<string> list = DownCodeInfoDataBase.GetDownCategories();
                ddlDownCategory.DataSource = list;
                ddlDownCategory.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindDownID()
        {
            try
            {
                List<string> list = DownCodeInfoDataBase.GetDownIDs(ddlDownCategory.SelectedValue.ToString());
                list.Insert(0, "ALL");
                ddlDownID.DataSource = list;
                ddlDownID.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            try
            {
                int rowindex = Convert.ToInt32(((sender as LinkButton).NamingContainer as GridViewRow).RowIndex);
                string DownID = (gvGrid.Rows[rowindex].FindControl("lblDownCate") as Label).Text;
                //string SublossID = (gvGrid.Rows[rowindex].FindControl("lblSublossid") as Label).Text;
                string SublossInterfaceID = (gvGrid.Rows[rowindex].FindControl("lblSublossInterfaceid") as Label).Text;
                string success = DownCodeInfoDataBase.DeleteSubLossMasterData(ddlDownCategory.SelectedValue.ToString(), DownID, SublossInterfaceID);
                if (success == "Deleted")
                {
                    SuccMessage = 2;
                    BindGrid();
                }
                else
                {
                    HelperClassGeneric.openDeleteErrorModal(this);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvGrid.Rows)
                {
                    string downid = ((Label)row.FindControl("lblDownCate")).Text;
                    string sublossdesc = ((TextBox)row.FindControl("txtEditSublossDesc")).Text;
                    //string SublossID = ((Label)row.FindControl("lblSublossid")).Text;
                    string SublossInterfaceID = ((Label)row.FindControl("lblSublossInterfaceid")).Text;
                    string hideSublossDesc = ((HiddenField)row.FindControl("hdfSublossDesc")).Value;
                    if (sublossdesc != hideSublossDesc)
                    {
                        string success = DownCodeInfoDataBase.SaveSubLossMasterData(ddlDownCategory.SelectedValue.ToString(), "", downid, sublossdesc, SublossInterfaceID,"Update");
                    }
                }
                SuccMessage = 1;
                gvGrid.EditIndex = -1;
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string DownID = (gvGrid.FooterRow.FindControl("ddlid") as DropDownList).SelectedValue;
                //string SublossID = (gvGrid.FooterRow.FindControl("txtSublossID") as TextBox).Text;
                string SublossInterfaceID = (gvGrid.FooterRow.FindControl("txtSublossInterfaceID") as TextBox).Text;
                string sublossDesc = (gvGrid.FooterRow.FindControl("txtSublossDesc") as TextBox).Text;
                //if (SublossID == "")
                //{
                //    HelperClassGeneric.openWarningToastrModal(this, "Please enter Subloss ID");
                //    return;
                //}
                if (SublossInterfaceID == "")
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Please enter SublossInterfaceID");
                    return;
                }
                else if (sublossDesc == "")
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Please enter Subloss Description");
                    return;
                }
                string success = DownCodeInfoDataBase.SaveSubLossMasterData(ddlDownCategory.SelectedValue.ToString(), "", DownID, sublossDesc, SublossInterfaceID,"Insert");
                if (success == "INSERTED" || success == "UPDATED")
                {
                    SuccMessage = 1;
                    BindGrid();
                }
                else if (success == "Interface ID already exist")
                {
                    SuccMessage = 3;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlDownCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindDownID();
                DropDownList ddlDownid = gvGrid.FooterRow.Cells[0].FindControl("ddlid") as DropDownList;
                ddlDownid.DataSource = DownCodeInfoDataBase.GetDownIDs(ddlDownCategory.SelectedValue.ToString());
                ddlDownid.DataBind();
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void gvGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList ddlDownid = e.Row.Cells[0].FindControl("ddlid") as DropDownList;
                    ddlDownid.DataSource = DownCodeInfoDataBase.GetDownIDs(ddlDownCategory.SelectedValue.ToString());
                    ddlDownid.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlDownID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}