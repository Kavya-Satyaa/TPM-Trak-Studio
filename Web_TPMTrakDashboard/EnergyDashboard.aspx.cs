using BusinessClassLibrary;
using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
	public partial class EnergyDashboard : System.Web.UI.Page
	{
		public static DataTable dtEnergyData = new DataTable();
		DateTime FromDate = DateTime.Now.Date;
		DateTime ToDate = DateTime.Now.Date;
		public static string view = string.Empty;
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
					txtFromDate.Text = VDGDataBaseAccess.GetLogicalDayStart(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
					txtToDate.Text = VDGDataBaseAccess.GetLogicalDayEnd(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
					view = ddlView.SelectedValue;
					BindPlantID();
					BindShiftData();
					IndicateEnergyCost();
					BindEnergyGridHeader();
				}
			}
			catch (Exception ex)
			{
				ErrorSignal.FromCurrentContext().Raise(ex);
				lblMessages.Text = ex.Message;
			}
		}

		private void BindPlantID()
		{
			try
			{
				List<string> lstPlantData = BindCockpitView.ViewPlantToDisplay();
				ddlPlantId.DataSource = lstPlantData;
				ddlPlantId.DataBind();
				ddlPlantId.Items.Insert(0, new ListItem { Text = "All Plant" });
				ddlPlantId.SelectedIndex = 0;
			}
			catch (Exception ex)
			{
				lblMessages.Text = ex.Message;
			}
		}

		private void BindShiftData()
		{
			try
			{
				List<string> lstPlantData = CockpitDataBaseAccess.GetAllPredefinedShifts();
				ddlShift.DataSource = lstPlantData;
				ddlShift.DataBind();
				ddlShift.SelectedIndex = 3;
			}
			catch (Exception ex)
			{
				lblMessages.Text = ex.Message;
			}
		}

		private void IndicateEnergyCost()
		{
			try
			{
				string CostPerKWH = string.Empty;
				CostPerKWH = DataBaseAccess.GetCostPerKWH();
				string unitCostMsg = DataBaseAccess.GetUnitCostMessage();
				//lblCostDescription.Text = string.Format(unitCostMsg, CostPerKWH);
			}
			catch (Exception ex)
			{
				ErrorSignal.FromCurrentContext().Raise(ex);
				lblMessages.Text = ex.Message;
			}
		}

		private void BindEnergyGridHeader()
		{
			EnergyGridView.Columns.Clear();
			try
			{
				List<string> colPropertyNames = new List<string>();
				List<string> colHeaderNames = DataBaseAccess.GetEnergyColumnsToBind(out colPropertyNames);
				string[] navFieldMachine = new string[1] { "MachineID" };
				string[] navFieldNode = new string[1] { "NodeID" };
				HyperLinkField hlinkField = new HyperLinkField();
				hlinkField.HeaderText = "Machine ID";
				hlinkField.DataNavigateUrlFields = navFieldMachine;
				hlinkField.DataTextField = "MachineID";
				hlinkField.Target = "_blank";
				EnergyGridView.Columns.Insert(0, hlinkField);
				for (int i = 0; i < colHeaderNames.Count; i++)
				{
					if (colPropertyNames[i].ToString() != "MachineID")
					{
						if (colPropertyNames[i].ToString() == "NodeID")
						{
							if (ddlView.SelectedValue.Equals("Node View"))
							{
								HyperLinkField hlinkField1 = new HyperLinkField();
								hlinkField1.HeaderText = colHeaderNames[i].ToString();
								hlinkField1.DataNavigateUrlFields = navFieldNode;
								hlinkField1.DataTextField = "NodeID";
								hlinkField1.Target = "_blank";
								EnergyGridView.Columns.Insert(0, hlinkField1);
							}
						}
						else
						{
							BoundField boundField = new BoundField();
							boundField.DataField = colPropertyNames[i].ToString();
							boundField.HeaderText = colHeaderNames[i].ToString();
							EnergyGridView.Columns.Add(boundField);
						}
					}
				}
				BindEnergyGridData();
			}
			catch (Exception ex)
			{
				ErrorSignal.FromCurrentContext().Raise(ex);
				lblMessages.Text = ex.Message;
			}
		}

		private void BindEnergyGridData()
		{
			FromDate = Util.GetDateTime(txtFromDate.Text);
			ToDate = Util.GetDateTime(txtToDate.Text);
			if ((ToDate - FromDate).TotalDays > 30)
			{
				lblMessages.Text = "Difference between to date and from date cannot be more than 31 days.";
				return;
			}
			else
			{
				string selectedPlant = ddlPlantId.SelectedItem.ToString() == "All Plant" ? "" : ddlPlantId.SelectedItem.ToString();
				string selectedMachine = "";
				string shift = "";
				if (ddlView.SelectedValue.Equals("Node View"))
				{
					dtEnergyData = DataBaseAccess.GetEnergyData(FromDate, ToDate, shift, selectedMachine, selectedPlant, "", "", "node", "", "S_GetSONA_EnergyCockpitDetails");
				}
				else if (ddlView.SelectedValue.Equals("Machine View"))
				{
					dtEnergyData = DataBaseAccess.GetEnergyData(FromDate, ToDate, shift, selectedMachine, selectedPlant, "", "", "machine", "", "S_GetSONA_EnergyCockpitDetails");
				}
				EnergyGridView.DataSource = dtEnergyData;
				EnergyGridView.DataBind();
				showhide(ddlView.SelectedValue.ToString(),dtEnergyData);
				//Session.Add("Energydashboard", ddlcolumnselection.SelectedValue);
				Session["EnergydashboardParam"] = ddlcolumnselection.SelectedValue;
			}
			if(ddlcolumnselection.SelectedValue !=null)
				Session["EnergydashboardParam"] = ddlcolumnselection.SelectedValue;
		}

		private void showhide(string ViewType,DataTable dt)
		{
			if(ViewType.Equals("Node View", StringComparison.OrdinalIgnoreCase))
			{
				if (ddlcolumnselection.SelectedValue.Equals("Phase2Neutral (R2N, Y2N, G2N)", StringComparison.OrdinalIgnoreCase))
				{					
					HideColumn("Volt12", "Volt23", "Volt31", "PF1", "PF2", "PF3", "Ampere1", "Ampere2", "Ampere3");
				}
				else if (ddlcolumnselection.SelectedValue.Equals("Phase2Phase (R2Y, Y2G, G2R)", StringComparison.OrdinalIgnoreCase))
				{
					HideColumn("Volt1", "Volt2", "Volt3", "PF1", "PF2", "PF3", "Ampere1", "Ampere2", "Ampere3");
				}
				else if (ddlcolumnselection.SelectedValue.Equals("PF (PF1, PF2, PF3)", StringComparison.OrdinalIgnoreCase))
				{
					HideColumn("Volt12", "Volt23", "Volt31", "Volt1", "Volt2", "Volt3", "Ampere1", "Ampere2", "Ampere3");
				}
				else if (ddlcolumnselection.SelectedValue.Equals("Current (A1, A2, A3)", StringComparison.OrdinalIgnoreCase))
				{
					HideColumn("Volt12", "Volt23", "Volt31", "PF1", "PF2", "PF3", "Volt1", "Volt2", "Volt3");
				}
			}
			else if(ViewType.Equals("Machine View", StringComparison.OrdinalIgnoreCase))
			{
				if (ddlcolumnselection.SelectedValue.Equals("Phase2Neutral (R2N, Y2N, G2N)", StringComparison.OrdinalIgnoreCase))
				{
					HideColumn("Volt12", "Volt23", "Volt31", "PF1", "PF2", "PF3", "Ampere1", "Ampere2", "Ampere3");
				}
				else if (ddlcolumnselection.SelectedValue.Equals("Phase2Phase (R2Y, Y2G, G2R)", StringComparison.OrdinalIgnoreCase))
				{
					HideColumn("Volt1", "Volt2", "Volt3", "PF1", "PF2", "PF3", "Ampere1", "Ampere2", "Ampere3");
				}
				else if (ddlcolumnselection.SelectedValue.Equals("PF (PF1, PF2, PF3)", StringComparison.OrdinalIgnoreCase))
				{
					HideColumn("Volt12", "Volt23", "Volt31", "Volt1", "Volt2", "Volt3", "Ampere1", "Ampere2", "Ampere3");
				}
				else if (ddlcolumnselection.SelectedValue.Equals("Current (A1, A2, A3)", StringComparison.OrdinalIgnoreCase))
				{
					HideColumn("Volt12", "Volt23", "Volt31", "PF1", "PF2", "PF3", "Volt1", "Volt2", "Volt3");
				}
			}
		}

		private void HideColumn(string v1, string v2, string v3, string v4, string v5, string v6, string v7, string v8, string v9)
		{
			foreach (DataControlField col in EnergyGridView.Columns)
			{
				BoundField bfield = col as BoundField;
				if(bfield!=null)
				{
					string datafield = bfield.DataField;
					if (datafield == v1)
					{
						bfield.Visible = false;
					}
					else if (datafield == v2)
					{
						bfield.Visible = false;
					}
					else if (datafield == v3)
					{
						bfield.Visible = false;
					}
					else if (datafield == v4)
					{
						bfield.Visible = false;
					}
					else if (datafield == v5)
					{
						bfield.Visible = false;
					}
					else if (datafield == v6)
					{
						bfield.Visible = false;
					}
					else if (datafield == v7)
					{
						bfield.Visible = false;
					}
					else if (datafield == v8)
					{
						bfield.Visible = false;
					}
					else if (datafield == v9)
					{
						bfield.Visible = false;
					}
				}
				
			}
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public static List<EnergyChartData> GetEnergyChartData(string plantId, string fromDate, string todate)
		{
			double val = 0;
			List<EnergyChartData> energyChartDataList = new List<EnergyChartData>();
			EnergyChartData energyChartData = null;
			try
			{
				//energyChartDataList = DataBaseAccess.GetEnergyChartData(plantId, fromDate, todate);
				foreach (DataRow row in dtEnergyData.Rows)
				{
					energyChartData = new EnergyChartData();
					if (view.Equals("Machine View"))
					{
						energyChartData.name = string.IsNullOrEmpty(row["MachineID"].ToString()) ? "" : row["MachineID"].ToString();
					}
					else
					{
						energyChartData.name = string.IsNullOrEmpty(row["NodeID"].ToString()) ? "" : row["NodeID"].ToString();
					}
					if (Double.TryParse(row["Energy"].ToString(), out val))
					{
						energyChartData.y = val;
					}
					else
					{
						energyChartData.y = 0;
					}
					energyChartDataList.Add(energyChartData);
				}
			}
			catch (Exception ex)
			{
				ErrorSignal.FromCurrentContext().Raise(ex);
			}
			return energyChartDataList;
		}

		protected void ddlShift_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				string selectedShift = ddlShift.SelectedValue.ToString();
				string logicalDayStart = string.Empty, logicalDayEnd = string.Empty;
				if (selectedShift.Contains("Today"))
				{
					if (ddlShift.SelectedValue.Equals("Today - All"))
					{
						logicalDayStart = CockpitDataBaseAccess.GetLogicalDay(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
						txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("yyyy-MM-dd HH:mm:ss");
						logicalDayEnd = CockpitDataBaseAccess.GetLogicalDayEnd(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
						txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("yyyy-MM-dd HH:mm:ss");
					}
					else
					{
						int index = selectedShift.IndexOf('-');
						var shift = CockpitDataBaseAccess.GetCurrentShiftTime(selectedShift.Substring(index + 1).Trim());
						if (shift != null)
						{
							logicalDayStart = shift[0];
							logicalDayEnd = shift[1];
							txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("yyyy-MM-dd HH:mm:ss");
							txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("yyyy-MM-dd HH:mm:ss");
						}
					}
				}
				else
				{
					if (ddlShift.SelectedValue.Equals("Yesterday - All"))
					{
						logicalDayStart = CockpitDataBaseAccess.GetLogicalDay(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
						txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("yyyy-MM-dd HH:mm:ss");
						logicalDayEnd = CockpitDataBaseAccess.GetLogicalDayEnd(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
						txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("yyyy-MM-dd HH:mm:ss");
					}
					else
					{
						int index = selectedShift.IndexOf('-');
						var shift = CockpitDataBaseAccess.GetCurrentShiftTime(selectedShift.Substring(index + 1).Trim());
						if (shift != null)
						{
							logicalDayStart = shift[0];
							logicalDayEnd = shift[1];

							txtFromDate.Text = Convert.ToDateTime(logicalDayStart).AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
							txtToDate.Text = Convert.ToDateTime(logicalDayEnd).AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
						}
					}
				}
			}
			catch (Exception ex)
			{
				ErrorSignal.FromCurrentContext().Raise(ex);
				lblMessages.Text = ex.ToString();
				Logger.WriteErrorLog(ex.ToString());
			}
		}

		protected void EnergyGridView_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				if (ddlView.SelectedValue == "Machine View")
				{
					HyperLink hlink = e.Row.Cells[0].Controls[0] as HyperLink;
					string machineId = hlink.Text;
					string nodeId = "";
					hlink.NavigateUrl = "~/DaywiseEnergyData.aspx?MachineID=" + machineId + "&NodeID=" + nodeId + "&FromDate=" + txtFromDate.Text + "&ToDate=" + txtToDate.Text + "&view=machine";
				}
				if (ddlView.SelectedValue == "Node View")
				{
					HyperLink hlink1 = e.Row.Cells[0].Controls[0] as HyperLink;
					string nodeId1 = hlink1.Text;
					string machineId1 = "";
					hlink1.NavigateUrl = "~/DaywiseEnergyData.aspx?MachineID=" + machineId1 + "&NodeID=" + nodeId1 + "&FromDate=" + txtFromDate.Text + "&ToDate=" + txtToDate.Text + "&view=node";

					HyperLink hlink = e.Row.Cells[1].Controls[0] as HyperLink;
					string machineId = hlink.Text;
					string nodeId = "";
					hlink.NavigateUrl = "~/DaywiseEnergyData.aspx?MachineID=" + machineId + "&NodeID=" + nodeId + "&FromDate=" + txtFromDate.Text + "&ToDate=" + txtToDate.Text + "&view=node";
				}
			}
		}

		protected void btnProcess_Click(object sender, EventArgs e)
		{
			string startDate = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd HH:mm:ss");
			string endDate = Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd HH:mm:ss");
			view = ddlView.SelectedValue;
			FromDate = Util.GetDateTime(startDate);
			ToDate = Util.GetDateTime(endDate);
			if ((ToDate - FromDate).TotalDays > 30)
			{
				lblMessages.Text = "Difference between to date and from date cannot be more than 31 days.";
				return;
			}
			else
			{
				BindEnergyGridHeader();
				BindEnergyGridData();
				ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "GetEnergyChartData();", true);
			}
		}

		protected void ddlView_SelectedIndexChanged(object sender, EventArgs e)
		{
			view = ddlView.SelectedValue;
		}

		protected void btnExport_Click(object sender, EventArgs e)
		{
			//DateTime fromDate = Convert.ToDateTime(VDGDataBaseAccess.GetLogicalDayStart(txtFromDate.Text));
			//DateTime toDate = Convert.ToDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtToDate.Text));
			//if(dtEnergyData !=null && dtEnergyData.Rows.Count >0)
			//	TMPTrakGenerateReport.EnergyReport(fromDate, toDate,dtEnergyData);
		}
	}
}