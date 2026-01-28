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
    public partial class ReworkDetails : System.Web.UI.Page
    {
        public static DataTable dtReworkDetails = new DataTable();
        string FromDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string ToDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserName"] == null || !Request.IsAuthenticated)
                {
                    Response.Redirect("~/SignIn");
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "showLoader();", true);
                if (!IsPostBack)
                {
                    txtFromDate.Text = VDGDataBaseAccess.GetLogicalDayStart(FromDate);
                    txtToDate.Text = VDGDataBaseAccess.GetLogicalDayEnd(ToDate);
                    BindMachineID();
                    BindReworkDetails(txtFromDate.Text, txtToDate.Text, ddlMachineID.SelectedValue == "All" ? "" : ddlMachineID.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void BindMachineID()
        {
            List<string> machineIds = new List<string>();
            try
            {
                machineIds = DataBaseAccess.GetAllEnabledMachines();
                if (!machineIds.Contains("All"))
                    machineIds.Insert(0, "All");
                ddlMachineID.DataSource = machineIds;
                ddlMachineID.DataBind();
                ddlMachineID.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private void BindReworkDetails(string fromDate, string toDate, string machineID)
        {
            try
            {
                dtReworkDetails = DataBaseAccess.GetAllReworkDetails(fromDate, toDate, machineID);
                ReworkDetailsGrid.DataSource = dtReworkDetails;
                ReworkDetailsGrid.DataBind();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "hideLoader();", true);
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "showLoader();", true);
            FromDate = Util.GetDateTime(txtFromDate.Text).ToString("yyyy-MM-dd HH:mm:ss");
            ToDate = Util.GetDateTime(txtToDate.Text).ToString("yyyy-MM-dd HH:mm:ss");
            BindReworkDetails(FromDate, ToDate, ddlMachineID.SelectedValue == "All" ? "" : ddlMachineID.SelectedValue);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "hideLoader();", true);
        }

		protected void btnExport_Click(object sender, EventArgs e)
		{
			try
			{
				bool successful = false;
				DateTime Fromdate = DateTime.Now;
				DateTime.TryParse(txtFromDate.Text, out Fromdate);
				DateTime Todate = DateTime.Now;
				DateTime.TryParse(txtToDate.Text, out Todate);
				if (dtReworkDetails != null && dtReworkDetails.Rows.Count > 0)
				{
					successful=TMPTrakGenerateReport.ReworkDetailReport(Fromdate, Todate, ddlMachineID.SelectedValue.ToString(), dtReworkDetails);
					if(successful)
						lblMessages.Text = "Export Successful";
					else
						lblMessages.Text = "Export Unsuccessful";
				}
				else
				{
					lblMessages.Text = "No data to export";
				}
			}
			catch (Exception ex)
			{
				Logger.WriteErrorLog(ex.Message);
				throw;
			}
		}
	}
}