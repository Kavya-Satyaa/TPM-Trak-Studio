using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class DaywiseEnergyData : System.Web.UI.Page
    {
        DateTime FromDate = DateTime.Now.Date;
        DateTime ToDate = DateTime.Now.Date;
		string NodeIDPropertyName ="";
		string NodeIdColumnHeaderName = "";
        string ViewMode = string.Empty;
        public static DataTable dayWiseEnergyData = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "showLoader();", true);
            if (!IsPostBack)
            {
                Session["MachineId"] = Request.QueryString["machineId"].ToString();
                Session["NodeID"] = Request.QueryString["NodeID"].ToString();
				if(Session["EnergydashboardParam"] !=null)
					ddlcolumnselection.SelectedValue = Session["EnergydashboardParam"].ToString();
				string fromDate = Request.QueryString["FromDate"].ToString();
                string toDate = Request.QueryString["ToDate"].ToString();
                Session["View"] = ViewMode = Request.QueryString["view"].ToString();
                txtFromDate.Text = fromDate;
                txtToDate.Text = toDate;
				BindHeader();
				if (Session["View"] != null)
				{
					if (Session["View"].ToString().Equals("machine", StringComparison.OrdinalIgnoreCase))
					{
						lblTitle.Text = " Daywise Energy Data For Machine - " + Session["MachineId"];
						BindDaywiseEnergyGrid(Session["MachineId"].ToString(), "", fromDate, toDate, "Day", Session["View"].ToString());
						tdNodeId.Visible = false;
						tdddlNodeId.Visible = false;
					}
					if (Session["View"].ToString().Equals("node", StringComparison.OrdinalIgnoreCase))
					{
						if (!string.IsNullOrEmpty(Session["NodeID"].ToString()))
						{
							lblTitle.Text = " Daywise Energy Data For Node - " + Session["NodeID"];
							BindDaywiseEnergyGrid("", Session["NodeID"].ToString(), fromDate, toDate, "Day", Session["View"].ToString());
							tdMachineId.Visible = false;
							tdddlMachineId.Visible = false;
						}
						if (!string.IsNullOrEmpty(Session["MachineId"].ToString()))
						{
							lblTitle.Text = " Daywise Energy Data For Machine - " + Session["MachineId"];
							BindDaywiseEnergyGrid(Session["MachineId"].ToString(), "", fromDate, toDate, "Day", Session["View"].ToString());
							tdNodeId.Visible = false;
							tdddlNodeId.Visible = false;
						}
					}
					BindMachineID();
					BindNodeID();
				}
			}
        }

        private void BindMachineID()
        {
            List<string> machineIds = new List<string>();
            try
            {
                machineIds = DataBaseAccess.GetAllEnabledMachines();
                ddlMachineId.DataSource = machineIds;
                ddlMachineId.DataBind();
                ddlMachineId.SelectedIndex = machineIds.Contains(Session["MachineId"].ToString()) ? machineIds.IndexOf(Session["MachineId"].ToString()) : 0;
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private void BindNodeID()
        {
            try
            {
                List<string> nodesList = DataBaseAccess.GetAllNodesForMachine("");
                ddlNodeId.DataSource = nodesList;
                ddlNodeId.DataBind();
                ddlNodeId.SelectedIndex = nodesList.Contains(Session["NodeID"].ToString()) ? nodesList.IndexOf(Session["NodeID"].ToString()) : 0;
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private void BindDaywiseEnergyGrid(string machineId, string nodeId, string fromDate, string toDate, string parameter, string view)
        {
			
            ViewMode = view;
            FromDate = Util.GetDateTime(fromDate);
            ToDate = Util.GetDateTime(toDate);
			BoundField boundField = new BoundField();
			boundField.DataField = NodeIDPropertyName;
			boundField.HeaderText = NodeIdColumnHeaderName;
			dayWiseEnergyData = DataBaseAccess.GetEnergyData(FromDate, ToDate, "", machineId, "", nodeId, parameter, view, "", "S_GetSONA_EnergyCockpitDetails");
			if (ViewMode.Equals("machine", StringComparison.OrdinalIgnoreCase))
			{
				if (gridViewDaywiseEnergyData.Columns.OfType<BoundField>().Any(x => x.DataField.Equals("NodeID")))
					gridViewDaywiseEnergyData.Columns.Remove(boundField);
			}
			if (ViewMode.Equals("node", StringComparison.OrdinalIgnoreCase))
			{
				if (!string.IsNullOrEmpty(nodeId))
				{
					if (gridViewDaywiseEnergyData.Columns.OfType<BoundField>().Any(x => x.DataField.Equals("NodeID")))
						gridViewDaywiseEnergyData.Columns.Remove(boundField);
				}
				if (!string.IsNullOrEmpty(machineId))
				{
					if (!gridViewDaywiseEnergyData.Columns.OfType<BoundField>().Any(x => x.DataField.Equals("NodeID")))
						gridViewDaywiseEnergyData.Columns.Insert(1, boundField);
				}
			}
			showhide(ddlcolumnselection.SelectedValue.ToString(), dayWiseEnergyData);
			gridViewDaywiseEnergyData.DataSource = dayWiseEnergyData;
            gridViewDaywiseEnergyData.DataBind();
		}

		private void BindHeader()
		{
			List<string> colPropertyNames = new List<string>();
			List<string> colHeaderNames = DataBaseAccess.GetEnergyColumnsToBind(out colPropertyNames);
			for (int i = 0; i < colHeaderNames.Count; i++)
			{
				if(colPropertyNames[i].ToString() != "NodeID")
				{
					BoundField boundField = new BoundField();
					boundField.DataField = colPropertyNames[i].ToString();
					boundField.HeaderText = colHeaderNames[i].ToString();
					gridViewDaywiseEnergyData.Columns.Add(boundField);
				}
				else
				{
					NodeIDPropertyName = colPropertyNames[i].ToString();
					NodeIdColumnHeaderName = colHeaderNames[i].ToString();
				}
			}
		}

		private void showhide(string ViewType, DataTable dt)
		{
			if (ddlcolumnselection.SelectedValue.ToString().Equals("Phase2Neutral (R2N, Y2N, G2N)", StringComparison.OrdinalIgnoreCase))
			{

				HideColumn("Volt12", "Volt23", "Volt31", "PF1", "PF2", "PF3", "Ampere1", "Ampere2", "Ampere3", "Volt1", "Volt2", "Volt3");
				
			}
			else if (ddlcolumnselection.SelectedValue.ToString().Equals("Phase2Phase (R2Y, Y2G, G2R)", StringComparison.OrdinalIgnoreCase))
			{
				HideColumn("Volt1", "Volt2", "Volt3", "PF1", "PF2", "PF3", "Ampere1", "Ampere2", "Ampere3", "Volt12", "Volt23", "Volt31");
				
			}
			else if (ddlcolumnselection.SelectedValue.ToString().Equals("PF (PF1, PF2, PF3)", StringComparison.OrdinalIgnoreCase))
			{
				HideColumn("Volt12", "Volt23", "Volt31", "Volt1", "Volt2", "Volt3", "Ampere1", "Ampere2", "Ampere3", "PF1", "PF2", "PF3");
				
			}
			else if (ddlcolumnselection.SelectedValue.ToString().Equals("Current (A1, A2, A3)", StringComparison.OrdinalIgnoreCase))
			{
				HideColumn("Volt12", "Volt23", "Volt31", "PF1", "PF2", "PF3", "Volt1", "Volt2", "Volt3", "Ampere1", "Ampere2", "Ampere3");
				
			}
		}

		private void HideColumn(string v1, string v2, string v3, string v4, string v5, string v6, string v7, string v8, string v9,string show1 , string show2 , string show3)
		{
			foreach (DataControlField col in gridViewDaywiseEnergyData.Columns)
			{
				BoundField bfield = col as BoundField;
				if (bfield != null)
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
					else if (datafield == show1)
					{
						bfield.Visible = true;
					}
					else if (datafield == show2)
					{
						bfield.Visible = true;
					}
					else if (datafield == show3)
					{
						bfield.Visible = true;
					}
				}

			}
		}

		protected void gridViewDaywiseEnergyData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ViewMode.Equals("machine"))
                {
                    HyperLink hlink = e.Row.Cells[0].Controls[0] as HyperLink;
                    string machineId = Session["MachineId"].ToString();
                    string nodeId = "";
                    string fromDate = Convert.ToDateTime(dayWiseEnergyData.Rows[e.Row.RowIndex][4]).ToString("yyyy-MM-dd HH:mm:ss");
                    string toDate = Convert.ToDateTime(dayWiseEnergyData.Rows[e.Row.RowIndex][5]).ToString("yyyy-MM-dd HH:mm:ss");
                    hlink.NavigateUrl = "~/EnergyShiftWiseData.aspx?MachineID=" + machineId + "&NodeID=" + nodeId + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&view=machine";
                }
                if (ViewMode.Equals("node"))
                {
                    if (!string.IsNullOrEmpty(Session["MachineId"].ToString()))
                    {
                        HyperLink hlink = e.Row.Cells[0].Controls[0] as HyperLink;
                        string machineId = Session["MachineId"].ToString();
                        string nodeId = "";
                        string fromDate = Convert.ToDateTime(dayWiseEnergyData.Rows[e.Row.RowIndex][5]).ToString("yyyy-MM-dd HH:mm:ss");
                        string toDate = Convert.ToDateTime(dayWiseEnergyData.Rows[e.Row.RowIndex][6]).ToString("yyyy-MM-dd HH:mm:ss");
                        hlink.NavigateUrl = "~/EnergyShiftWiseData.aspx?MachineID=" + machineId + "&NodeID=" + nodeId + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&view=node";
                    }
                    if (!string.IsNullOrEmpty(Session["NodeID"].ToString()))
                    {
                        HyperLink hlink = e.Row.Cells[0].Controls[0] as HyperLink;
                        string machineId = "";
                        string nodeId = Session["NodeID"].ToString();
                        string fromDate = Convert.ToDateTime(dayWiseEnergyData.Rows[e.Row.RowIndex][5]).ToString("yyyy-MM-dd HH:mm:ss");
                        string toDate = Convert.ToDateTime(dayWiseEnergyData.Rows[e.Row.RowIndex][6]).ToString("yyyy-MM-dd HH:mm:ss");
                        hlink.NavigateUrl = "~/EnergyShiftWiseData.aspx?MachineID=" + machineId + "&NodeID=" + nodeId + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&view=node";
                    }
                }
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<EnergyChartData> GetDaywiseEnergyChartData(string param)
        {
            double val = 0;
            List<EnergyChartData> daywiseEnergyChartDataList = new List<EnergyChartData>();
            EnergyChartData energyChartData = null;
            try
            {
                foreach (DataRow row in dayWiseEnergyData.Rows)
                {
                    energyChartData = new EnergyChartData();
                    energyChartData.name = Convert.ToDateTime(row["StartTime"]).ToString("dd-MMM-yyyy");
                    if (Double.TryParse(row["Energy"].ToString(), out val))
                    {
                        energyChartData.y = val;
                    }
                    else
                    {
                        energyChartData.y = 0;
                    }
                    daywiseEnergyChartDataList.Add(energyChartData);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return daywiseEnergyChartDataList;
        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            ViewMode = Session["View"].ToString();
            Session["MachineId"] = ddlMachineId.SelectedItem.Text;
            Session["NodeID"] = ddlNodeId.SelectedItem.Text;
            if (ViewMode.Equals("machine", StringComparison.OrdinalIgnoreCase))
            {
                lblTitle.Text = " Daywise Energy Data For Machine - " + Session["MachineId"];
                BindDaywiseEnergyGrid(Session["MachineId"].ToString(), "", txtFromDate.Text, txtToDate.Text, "Day", ViewMode);
            }
            if (ViewMode.Equals("node", StringComparison.OrdinalIgnoreCase))
            {
                if (tdNodeId.Visible && tdddlNodeId.Visible)
                {
                    lblTitle.Text = " Daywise Energy Data For Node - " + Session["NodeID"];
                    BindDaywiseEnergyGrid("", Session["NodeID"].ToString(), txtFromDate.Text, txtToDate.Text, "Day", ViewMode);
                }
                if (tdMachineId.Visible && tdddlMachineId.Visible)
                {
                    lblTitle.Text = " Daywise Energy Data For Machine - " + Session["MachineId"];
                    BindDaywiseEnergyGrid(Session["MachineId"].ToString(), "", txtFromDate.Text, txtToDate.Text, "Day", ViewMode);
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "GetDaywiseEnergyChartData();", true);
        }
    }
}