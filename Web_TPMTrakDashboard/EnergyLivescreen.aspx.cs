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
	public partial class EnergyLivescreen : System.Web.UI.Page
	{
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
					bindmachineid();
					bindnodeid();
					bindgrid();
				}
			}
			catch (Exception ex)
			{
				ErrorSignal.FromCurrentContext().Raise(ex);
			}
		}

		private void bindnodeid()
		{
			try
			{
				List<string> NodeId = DataBaseAccess.GetAllNodesForMachine("");
				if (NodeId != null && NodeId.Count > 0)
				{
					NodeId.Insert(0, "All");
					ddlNodeID.DataSource = NodeId;
					ddlNodeID.DataBind();
				}
			}
			catch (Exception ex)
			{
				Logger.WriteErrorLog(ex.Message);
				throw;
			}
		}

		private void bindmachineid()
		{
			try
			{
				List<string> MachineId = DataBaseAccess.GetMachineInfo_nodeinterface();
				if (MachineId != null && MachineId.Count > 0)
				{
					MachineId.Insert(0, "All");
					ddlMachineID.DataSource = MachineId;
					ddlMachineID.DataBind();
				}
			}
			catch (Exception ex)
			{
				Logger.WriteErrorLog(ex.Message);
				throw;
			}
		}

		private void bindgrid()
		{
			try
			{
				DataTable dt = new DataTable();
				dt = DataBaseAccess.GetEnergyLivedata("", "", "Live");
				if (dt != null && dt.Rows.Count > 0)
				{
					listviewlivescreen.DataSource = dt;
					listviewlivescreen.DataBind();
				}
			}
			catch (Exception ex)
			{
				Logger.WriteErrorLog(ex.Message);
				throw;
			}

		}

		protected void timer_Tick(object sender, EventArgs e)
		{
			bindgrid();
		}

		protected void btnview_Click(object sender, EventArgs e)
		{
			DataTable dt = new DataTable();
			dt = DataBaseAccess.GetEnergyLivedata(ddlMachineID.SelectedValue.ToString(), ddlNodeID.SelectedValue.ToString(), "Live");
			if (dt != null && dt.Rows.Count > 0)
			{
				listviewlivescreen.DataSource = dt;
				listviewlivescreen.DataBind();
			}
			else
			{
				listviewlivescreen.DataSource = dt;
				listviewlivescreen.DataBind();
			}
		}

		protected void timerDataChange_Tick(object sender, EventArgs e)
		{
			try
			{
				bindgrid();
			}
			catch (Exception ex)
			{
				ErrorSignal.FromCurrentContext().Raise(ex);
			}
		}

		protected void chkAutoBox_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				timerDataChange.Interval = Convert.ToInt32(DataBaseAccess.AutoRefreshData);
				if (chkAutoBox.Checked)
					timerDataChange.Enabled = true;
				else
					timerDataChange.Enabled = false;
			}
			catch (Exception ex)
			{
				ErrorSignal.FromCurrentContext().Raise(ex);
			}
		}
	}
	public class listviewheader
	{
		
	}
}