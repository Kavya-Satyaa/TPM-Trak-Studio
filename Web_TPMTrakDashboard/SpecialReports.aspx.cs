using BusinessClassLibrary;
using Elmah;
using ModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
	public partial class SpecialReports : System.Web.UI.Page
	{
		public List<UserAccessModel> useAccessData = null;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["connectionString"] == null)
				Response.Redirect("~/SignIn.aspx", false);
			if (!IsPostBack)
			{
				SessionClear.ClearSession();
				if (Session["UserAccessData"] == null)
					Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"] != null ? Session["UserName"].ToString() : "PCT");
				else
					useAccessData = Session["UserAccessData"] as List<UserAccessModel>;
				pageload();
			}
		}

		private void pageload()
		{
			BindPlantId();
			BindMachines();
			BindShiftData();
			if (ConfigurationManager.AppSettings["Mangalreports"].Equals("1", StringComparison.OrdinalIgnoreCase))
			{
				ddlReportType.Items.Add(new ListItem("Hourly Report", "HourlyReport"));
				ddlReportType.Items.Add(new ListItem("Down Time", "Downtime"));
				trToDate.Visible = false;
				trShift.Visible = false;
			}
		}

		private void BindPlantId()
		{
			try
			{

				List<string> lstPlantData = BindCockpitView.ViewPlantToDisplay();
				ddlPlantId.DataSource = lstPlantData;
				ddlPlantId.DataBind();
			}
			catch (Exception ex)
			{
				ErrorSignal.FromCurrentContext().Raise(ex);
				lblMessages.Text = ex.Message;
			}
		}

		#region "Bind Machine Id"
		private void BindMachines()
		{
			try
			{
				ddlMachineID.Items.Clear();// = null;
				var allMachineName = VDGDataBaseAccess.GetAllMachines(ddlPlantId.SelectedValue.ToString());// AccessReportData.GetAllMachines(ddlPlantId.SelectedItem.ToString());
				if (allMachineName != null && allMachineName.Count > 0)
				{
					ddlMachineID.DataSource = allMachineName;
					ddlMachineID.DataBind();
				}
			}
			catch (Exception ex)
			{
				ErrorSignal.FromCurrentContext().Raise(ex);
				lblMessages.Text = ex.Message;
			}
		}
		#endregion

		protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		protected void ddlMachineID_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
		{
			trMachine.Visible = true;
			trPlant.Visible = true;
			trFromDate.Visible = true;
			trToDate.Visible = true;
			if (ddlReportType.SelectedValue.ToString().Equals("HourlyReport", StringComparison.OrdinalIgnoreCase))
			{
				trToDate.Visible = false;
			}
			else if (ddlReportType.SelectedValue.ToString().Equals("Downtime", StringComparison.OrdinalIgnoreCase))
			{
				trMachine.Visible = false;
				trPlant.Visible = false;
			}
		}

		protected void btnGenerate_Click(object sender, EventArgs e)
		{

			string machineId = ddlMachineID.SelectedIndex >= 0 ? ddlMachineID.SelectedValue : string.Empty;
			string plantId = ddlPlantId.SelectedIndex >= 0 ? ddlPlantId.SelectedValue : string.Empty;
			//string shift = ddlShift.SelectedIndex >= 0 ? ddlShift.SelectedValue : string.Empty;
			DateTime fromDate = DateTime.Now.Date;
			DateTime toDate = DateTime.Now.Date;
			fromDate = Convert.ToDateTime(VDGDataBaseAccess.GetLogicalDayStart(txtFromDate.Text));
			toDate = Convert.ToDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtToDate.Text));
			if (ddlReportType.SelectedValue.ToString().Equals("Downtime", StringComparison.OrdinalIgnoreCase) )
			{
				TMPTrakGenerateReport.MangalDowntimeReport(fromDate, toDate);
			}
			if (ddlReportType.SelectedValue.ToString().Equals("HourlyReport", StringComparison.OrdinalIgnoreCase))
			{
				btnGenerate.Attributes.Add("onclick", "hourglass();");
				TMPTrakGenerateReport.MangalHourlychart(fromDate, machineId, plantId);
			}
		}

		#region "Bind Shift Data"
		private void BindShiftData()
		{
			try
			{
				var allShift = BindCockpitView.GetAllShift();
				if (allShift != null && allShift.Count > 0)
				{
					ddlShift.DataSource = allShift;
					ddlShift.DataBind();
					ddlShift.Items.Insert(0, new ListItem
					{
						Text = GetGlobalResourceObject("CommanResource", "ShiftAll").ToString(),
						Value = "All"
					});

				}
			}
			catch (Exception ex)
			{
				ErrorSignal.FromCurrentContext().Raise(ex);
				lblMessages.Text = ex.Message;
			}
		}
		#endregion
		protected void ddlShift_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
	}
}