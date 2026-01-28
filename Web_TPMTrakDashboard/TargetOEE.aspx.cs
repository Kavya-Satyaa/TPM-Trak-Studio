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
	public partial class TargetOEE : System.Web.UI.Page
	{

		public bool isSuccess = false;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Bindmachinedropdown();
				txtToDate.Text = DateTime.Now.ToString("yyyy");
				DataforOEE();
			}
		}

		private void Bindmachinedropdown()
		{
			List<string> MachineID = DataBaseAccess.GetAllMacForPlant("");
			if (MachineID != null && MachineID.Count > 0)
			{
				MachineID.Insert(0, "All");
				ddlMachineID.DataSource = MachineID;
				ddlMachineID.DataBind();
			}
		}

		private void DataforOEE()
		{
			try
			{
				string Dateyear = txtToDate.Text;
				DataTable dt1 = DataBaseAccess.SearchOEETarget(Dateyear + "-01-01", ddlMachineID.SelectedValue == "All" ? "" : ddlMachineID.SelectedValue, "view", ddlEfficiencies.SelectedValue);
				if (dt1 != null)
				{
					Session["oeetargetval"] = grdOEE.DataSource = dt1;
					grdOEE.DataBind();

				}
			}
			catch(Exception ex)
            {
				Logger.WriteErrorLog(ex.Message);
            }
		}


		private void CheckRowstate(string MachineID, DateTime StartDate, DateTime EndDate, string TargetValue)
		{
			try
			{

				string sucessFailure = string.Empty;
				DataBaseAccess.insertUpdateTargetOEE(MachineID, "InsertEffTarget", StartDate.ToString("yyyy-MM-dd HH:mm:ss"), EndDate.ToString("yyyy-MM-dd HH:mm:ss"), TargetValue, ddlEfficiencies.SelectedValue, out sucessFailure);
				if (sucessFailure.Equals("Successfull"))
				{
					lblMessages.Text = "Record Update Successfully  ";
					lblMessages.ForeColor = System.Drawing.Color.White;
					DataforOEE();
				}
			}

			catch (Exception ex)
			{
				lblMessages.Text = ex.Message;
				lblMessages.ForeColor = System.Drawing.Color.Red;
			}
		}
		protected void btnsearch_Click(object sender, EventArgs e)
		{
			try
			{
				if (txtToDate.Text.ToString() != "")
				{
					DataforOEE();
				}
				else
				{
					lblMessages.Text = "Please Select Year";
					lblMessages.ForeColor = System.Drawing.Color.Red;
				}
			}
			catch (Exception ex)
			{
				lblMessages.Text = ex.Message;
				lblMessages.ForeColor = System.Drawing.Color.Red;
			}
		}

		protected void btnsave_Click(object sender, EventArgs e)
		{
			//string sucessfailure = string.Empty;
			//string Startdate, EndDate = string.Empty;
			string TargetOE = string.Empty;
			//DateTime now = DateTime.Parse(txtToDate.Text);
			//var startDate = new DateTime(now.Year, now.Month, 1);
			//var endDate = startDate.AddMonths(1).AddDays(-1);
			//Startdate = startDate.ToString("yyyy-MM-dd");
			//EndDate = endDate.ToString("yyyy-MM-dd");
			if (txtToDate.Text.ToString() == "")
			{
				lblMessages.Text = "Please Select Year";
				lblMessages.ForeColor = System.Drawing.Color.Red;
			}
			int year = Convert.ToInt32(txtToDate.Text);
			DateTime startDate = DateTime.Now, endDate = DateTime.Now;
			try
			{
				foreach (GridViewRow row in grdOEE.Rows)
				{
					HiddenField hdfvalue = (HiddenField)row.FindControl("hdfcheckupdate");
					if (hdfvalue.Value.Equals("updated", StringComparison.OrdinalIgnoreCase))
					{
						string machineid = ((Label)row.FindControl("lblMachine")).Text;

						string lbljan = ((Label)row.FindControl("lbljan")).Text;
						string lblfeb = ((Label)row.FindControl("lblfeb")).Text;
						string lblmar = ((Label)row.FindControl("lblmar")).Text;
						string lblapr = ((Label)row.FindControl("lblapr")).Text;
						string lblmay = ((Label)row.FindControl("lblmay")).Text;
						string lbljune = ((Label)row.FindControl("lbljune")).Text;
						string lbljuly = ((Label)row.FindControl("lbljuly")).Text;
						string lblaug = ((Label)row.FindControl("lblaug")).Text;
						string lblsept = ((Label)row.FindControl("lblsept")).Text;
						string lbloct = ((Label)row.FindControl("lbloct")).Text;
						string lblnov = ((Label)row.FindControl("lblnov")).Text;
						string lbldec = ((Label)row.FindControl("lbldec")).Text;

						string strjan = ((TextBox)row.FindControl("txtjan")).Text;
						string strfeb = ((TextBox)row.FindControl("txtfeb")).Text;
						string strmar = ((TextBox)row.FindControl("txtmar")).Text;
						string strapr = ((TextBox)row.FindControl("txtapr")).Text;
						string strmay = ((TextBox)row.FindControl("txtmay")).Text;
						string strjune = ((TextBox)row.FindControl("txtjune")).Text;
						string strjuly = ((TextBox)row.FindControl("txtjuly")).Text;
						string straug = ((TextBox)row.FindControl("txtaug")).Text;
						string strsept = ((TextBox)row.FindControl("txtsept")).Text;
						string stroct = ((TextBox)row.FindControl("txtoct")).Text;
						string strnov = ((TextBox)row.FindControl("txtnov")).Text;
						string strdec = ((TextBox)row.FindControl("txtdec")).Text;

						if (strjan != lbljan)
						{
							TargetOE = strjan;
							if(Convert.ToInt32(TargetOE)>100)
							{

							}
							startDate = new DateTime(year, 1, 1);
							endDate = startDate.AddMonths(1).AddDays(-1);
							CheckRowstate(machineid, startDate, endDate, TargetOE);
						}
						if (strfeb != lblfeb)
						{
							TargetOE = strfeb;
							startDate = new DateTime(year, 2, 1);
							endDate = startDate.AddMonths(1).AddDays(-1);
							CheckRowstate(machineid, startDate, endDate, TargetOE);
						}
						if (strmar != lblmar)
						{
							TargetOE = strmar;
							startDate = new DateTime(year, 3, 1);
							endDate = startDate.AddMonths(1).AddDays(-1);
							CheckRowstate(machineid, startDate, endDate, TargetOE);
						}
						if (strapr != lblapr)
						{
							TargetOE = strapr;
							startDate = new DateTime(year, 4, 1);
							endDate = startDate.AddMonths(1).AddDays(-1);
							CheckRowstate(machineid, startDate, endDate, TargetOE);
						}

						if (strmay != lblmay)
						{
							TargetOE = strmay;
							startDate = new DateTime(year, 5, 1);
							endDate = startDate.AddMonths(1).AddDays(-1);
							CheckRowstate(machineid, startDate, endDate, TargetOE);
						}
						if (strjune != lbljune)
						{
							TargetOE = strjune;
							startDate = new DateTime(year, 6, 1);
							endDate = startDate.AddMonths(1).AddDays(-1);
							CheckRowstate(machineid, startDate, endDate, TargetOE);
						}
						if (strjuly != lbljuly)
						{
							TargetOE = strjuly;
							startDate = new DateTime(year, 7, 1);
							endDate = startDate.AddMonths(1).AddDays(-1);
							CheckRowstate(machineid, startDate, endDate, TargetOE);
						}
						if (straug != lblaug)
						{
							TargetOE = straug;
							startDate = new DateTime(year, 8, 1);
							endDate = startDate.AddMonths(1).AddDays(-1);
							CheckRowstate(machineid, startDate, endDate, TargetOE);
						}
						if (strsept != lblsept)
						{
							TargetOE = strsept;
							startDate = new DateTime(year, 9, 1);
							endDate = startDate.AddMonths(1).AddDays(-1);
							CheckRowstate(machineid, startDate, endDate, TargetOE);
						}
						if (stroct != lbloct)
						{
							TargetOE = stroct;
							startDate = new DateTime(year, 10, 1);
							endDate = startDate.AddMonths(1).AddDays(-1);
							CheckRowstate(machineid, startDate, endDate, TargetOE);
						}
						if (strnov != lblnov)
						{
							TargetOE = strnov;
							startDate = new DateTime(year, 11, 1);
							endDate = startDate.AddMonths(1).AddDays(-1);
							CheckRowstate(machineid, startDate, endDate, TargetOE);
						}
						if (strdec != lbldec)
						{
							TargetOE = strdec;
							startDate = new DateTime(year, 12, 1);
							endDate = startDate.AddMonths(1).AddDays(-1);
							CheckRowstate(machineid, startDate, endDate, TargetOE);
						}

					}
				}
			}
			catch (Exception ex)
			{
				lblMessages.Text = ex.Message;
			}


		}
	}
}
