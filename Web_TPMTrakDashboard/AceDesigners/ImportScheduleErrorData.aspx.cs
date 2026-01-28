using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.AceDesigners.Model;

namespace Web_TPMTrakDashboard.AceDesigners
{
    public partial class ImportScheduleErrorData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ScheduleErrorData"] = null;
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                btnDateView_Click(null, null);
            }
        }
        protected void btnDateView_Click(object sender, EventArgs e)
        {
            hdnViewType.Value = "Date";
            BindData();
        }
        protected void btnCompPOView_Click(object sender, EventArgs e)
        {
            hdnViewType.Value = "PO";
            BindData();
        }
        private void BindData()
        {
            try
            {
                DataTable dt = new DataTable();
                List<ScheduleImportErrorEntity> list = AceDatabaseAccess.getScheduleImportErrorMsgDetails(txtFromDate.Text, txtToDate.Text, txtPOSearch.Text, hdnViewType.Value);
                gvErrorData.DataSource = list;
                gvErrorData.DataBind();
                Session["ScheduleErrorData"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ScheduleErrorData"] == null)
                {
                    return;
                }
                List<ScheduleImportErrorEntity> list = Session["ScheduleErrorData"] as List<ScheduleImportErrorEntity>;
                AceGenerateReport.generateScheduleErrorReport(txtFromDate.Text, txtToDate.Text, txtPOSearch.Text, hdnViewType.Value, list);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void gvErrorData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvErrorData.PageIndex = e.NewPageIndex;
                if (Session["ScheduleErrorData"] != null)
                {
                    gvErrorData.DataSource = Session["ScheduleErrorData"] as List<ScheduleImportErrorEntity>;
                    gvErrorData.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void gvErrorData_PreRender(object sender, EventArgs e)
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
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}