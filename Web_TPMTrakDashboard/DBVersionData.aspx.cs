using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class DBVersionData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblPackageVersion.Text = SoftwareDBVersion.SoftwareVersion;
                lblDBVersion.Text = SoftwareDBVersion.DbVersion;
                lblScriptName.Text = SoftwareDBVersion.ScriptName;
                Session["DBVersionList"] = null;
                BindData();
            }
        }
        private void BindData()
        {
            try
            {
                List<DBVersionEntity> list = DataBaseAccess.getDBVersionDetails();
                gvDBVersion.DataSource = list;
                gvDBVersion.DataBind();
                Session["DBVersionList"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void gvDBVersion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDBVersion.PageIndex = e.NewPageIndex;
                if (Session["DBVersionList"] != null)
                {
                    gvDBVersion.DataSource = Session["DBVersionList"] as List<DBVersionEntity>;
                    gvDBVersion.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void gvDBVersion_PreRender(object sender, EventArgs e)
        {
            try
            {
                GridView grid = (GridView)sender;
                if (grid != null)
                {
                    GridViewRow pagerRow = (GridViewRow)grid.BottomPagerRow;
                    if (pagerRow != null)
                    {
                        pagerRow.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            { }
        }
    }
}