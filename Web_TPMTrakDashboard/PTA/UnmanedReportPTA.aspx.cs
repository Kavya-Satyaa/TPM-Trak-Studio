using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.PTA
{
    public partial class UnmanedReportPTA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtYear.Text = DateTime.Now.ToString("yyyy");
                txtMonth.Text = DateTime.Now.ToString("MM");
                BindPlant();
                BindShift();
                btnView_Click(null, null);
            }
        }
        private void BindShift()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllShifts("");
                if (list.Count > 0)
                {
                    list.Remove("All");
                }
                ddlShift.DataSource = list;
                ddlShift.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindPlant()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlants();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();
                ddlPlant_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = VDGDataBaseAccess.GetAllMachines(ddlPlant.SelectedValue);
                if (list.Count > 0)
                {
                    list.Insert(0, "All");
                }
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindSummaryData();
        }
        private void BindSummaryData()
        {
            try
            {
                tdBack.Visible = false;
                tdExport.Visible = false;
                summaryContainer.Visible = true;
                detailContainer.Visible = false;
                DateTime fromDate = Util.GetDateTime("01-" + txtMonth.Text + "-" + txtYear.Text);
                DateTime toDate = fromDate.AddMonths(1).AddDays(-1);
                List<UnmanedReportEntity> list = DataBaseAccessPTA.getUnmanedReportSummary(fromDate.ToString("dd-MM-yyyy"), toDate.ToString("dd-MM-yyyy"), ddlPlant.SelectedValue, ddlMachine.SelectedValue, ddlShift.SelectedValue);
                lvSummary.DataSource = list;
                lvSummary.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string ReportStatus = "";
                DateTime fromDate = Util.GetDateTime("01-" + txtMonth.Text + "-" + txtYear.Text);
                DateTime toDate = fromDate.AddMonths(1).AddDays(-1);

                List<UnmanedReportEntity> list = Session["UnmanedDetails"] as List<UnmanedReportEntity>;
                ReportStatus = PTAGenerateReport.GenerateUnmanedDetailReport(fromDate.ToString("dd-MM-yyyy"), toDate.ToString("dd-MM-yyyy"), ddlShift.SelectedValue, list);
                if (ReportStatus.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openSuccessModal(this, "Report Generated");
                else if (ReportStatus.Equals("NoData", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openWarningToastrModal(this, "No data found");
                else
                    HelperClassGeneric.openWarningToastrModal(this, "Try again");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachine();
        }

        protected void lnkMachine_Click(object sender, EventArgs e)
        {
            try
            {
                tdBack.Visible = true;
                tdExport.Visible = true;
                summaryContainer.Visible = false;
                detailContainer.Visible = true;
                var listItem = ((sender as LinkButton).NamingContainer as ListViewDataItem);
                string machine = (listItem.FindControl("lnkMachine") as LinkButton).Text;
                DateTime fromDate = Util.GetDateTime("01-" + txtMonth.Text + "-" + txtYear.Text);
                DateTime toDate = fromDate.AddMonths(1).AddDays(-1);
                List<UnmanedReportEntity> list = DataBaseAccessPTA.getUnmanedReportDetails(fromDate.ToString("dd-MM-yyyy"), toDate.ToString("dd-MM-yyyy"), "", machine, ddlShift.SelectedValue);
                Session["UnmanedDetails"] = list;
                lvDetails.DataSource = list;
                lvDetails.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            btnView_Click(null, null);
        }
    }
}