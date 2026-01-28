using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class DataAnalysis : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
          {
                BindMachines();
                ddlComponent.SelectedValue = Request.QueryString["machineId"];
                txtFromDate.Text = Request.QueryString["fromdate"];
                txtToDate.Text = Request.QueryString["Todate"];
                PageLoadData();
            }
        }

        #region "Bind Machine Id"
        private void BindMachines()
        {
            try
            {
                var allMachineName = VDGDataBaseAccess.GetAllMachines("All");
                if (allMachineName != null && allMachineName.Count > 0)
                {
                    ddlComponent.DataSource = allMachineName;
                    ddlComponent.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "-----------------Bind Auto Data-------------"
        private void BindAutoData()
        {
            DateTime fromDate = DateTime.Now.Date;
            DateTime toDate = DateTime.Now.Date;
            fromDate = Util.GetDateTime(txtFromDate.Text.Trim());
            toDate = Util.GetDateTime(txtToDate.Text.Trim());
           
            DataTable autoData = null;

            if (Session["autodatasource"] == null)
            {
                autoData = VDGDataBaseAccess.GetAnalysedAutoData(fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), ddlComponent.SelectedValue.ToString(), "AutoData");
                Session["autodatasource"] = autoData;
            }
            else
                autoData = Session["autodatasource"] as DataTable;

            //-------------------Grid Auto Data-----------------
            if (autoData != null && autoData.Rows.Count > 0)
            {
                gridAutoData.DataSource = autoData;
                gridAutoData.DataBind();
            }
            else
            {
                gridAutoData.DataSource = null;
                gridAutoData.DataBind();
            }
        }
        #endregion

        #region "----------------Page Load Data--------------"
        private void PageLoadData()
        {
            Session["autodatasource"] = null;
            Session["rowdatasource"] = null;
            BindAutoData();
            BindRowData();
            SetValue();
        }
        #endregion

        #region "Bind Row Data"
        private void BindRowData()
        {
            DateTime fromDate = DateTime.Now.Date;
            DateTime toDate = DateTime.Now.Date;
            fromDate = Util.GetDateTime(txtFromDate.Text.Trim());
            toDate = Util.GetDateTime(txtToDate.Text.Trim());
            DataTable rawData = null;

            if (Session["rowdatasource"] == null)
            {
                rawData = VDGDataBaseAccess.GetAnalysedAutoData(fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), ddlComponent.SelectedValue.ToString(), "RawData");
                Session["rowdatasource"] = rawData;
            }
            else
                rawData = Session["rowdatasource"] as DataTable;


            //-----------------Grid Row Data--------------------
            if (rawData != null && rawData.Rows.Count > 0)
            {
                gridRowData.DataSource = rawData;
                gridRowData.DataBind();
            }
            else
            {
                gridRowData.DataSource = null;
                gridRowData.DataBind();
            }

        }
        private void SetTextToLabels(VDGDataAnalysis vals)
        {
            try
            {
                lblProgramStart.Text = vals.ProgramStart;
                lblProductionRecord.Text = vals.ProductionRecord;
                lblStoppageRecord.Text = vals.DownRecord;
                lblInCycleStoppage.Text = vals.InCycleDownRecord;

                lblProductionRecordAuto.Text = vals.ProductionRecordStart;
                lblProductionRecordEnd.Text = vals.ProductionRecordEnded;
                lblStoppageRecordStart.Text = vals.DownRecordStarted;
                lblStoppageRecordEnd.Text = vals.DownRecordEnded;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Set Valu----------------------"
        private void SetValue()
        {
            DateTime fromDate = DateTime.Now.Date;
            DateTime toDate = DateTime.Now.Date;
            fromDate = Util.GetDateTime(txtFromDate.Text.Trim());
            toDate = Util.GetDateTime(txtToDate.Text.Trim());
            VDGDataAnalysis vals = VDGDataBaseAccess.GetValsForAutoAndRawData(fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), ddlComponent.SelectedValue.ToString(), "statistics");
            if (vals != null)
            {
                SetTextToLabels(vals);
            }
        }
        #endregion


        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            PageLoadData();
        }

        protected void OnPageIndexChangingRowData(object sender, GridViewPageEventArgs e)
        {
            gridRowData.PageIndex = e.NewPageIndex;
            BindRowData();
        }

        protected void OnPageIndexChangingAutoData(object sender, GridViewPageEventArgs e)
        {
            gridAutoData.PageIndex = e.NewPageIndex;
            BindAutoData();
        }

        public SortDirection dir
        {
            get
            {
                if (Session["dirState"] == null)
                {
                    Session["dirState"] = SortDirection.Ascending;
                }
                return (SortDirection)Session["dirState"];
            }
            set
            {
                Session["dirState"] = value;
            }

        }

        protected void gridAutoData_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = null;
            dt = Session["autodatasource"] as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                string SortDir = string.Empty;
                if (dir == SortDirection.Ascending)
                {
                    dir = SortDirection.Descending;
                    SortDir = "Desc";
                }
                else
                {
                    dir = SortDirection.Ascending;
                    SortDir = "Asc";
                }
                DataView sortedView = new DataView(dt);
                sortedView.Sort = e.SortExpression + " " + SortDir;
                gridAutoData.DataSource = sortedView;
                gridAutoData.DataBind();
            }
            else
            {
                BindAutoData();
            }
        }

        protected void gridrowData_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = null;
            dt = Session["rowdatasource"] as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                string SortDir = string.Empty;
                if (dir == SortDirection.Ascending)
                {
                    dir = SortDirection.Descending;
                    SortDir = "Desc";
                }
                else
                {
                    dir = SortDirection.Ascending;
                    SortDir = "Asc";
                }
                DataView sortedView = new DataView(dt);
                sortedView.Sort = e.SortExpression + " " + SortDir;
                gridRowData.DataSource = sortedView;
                gridRowData.DataBind();
            }
            else
            {
                BindRowData();
            }
        }

    }
}