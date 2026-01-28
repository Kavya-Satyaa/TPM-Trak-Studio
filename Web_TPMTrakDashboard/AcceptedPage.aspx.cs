using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessClassLibrary;
using Elmah;

namespace Web_TPMTrakDashboard
{
	public partial class AcceptedPage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["connectionString"] == null)
				Response.Redirect("~/SignIn.aspx", false);
			if (!IsPostBack)
			{
				if (Request.QueryString["machineId"] != "")
				{
					Session["MachineId"] = Request.QueryString["machineId"].ToString();
					Session["ShiftId"] = Request.QueryString["shiftId"].ToString();
					Session["Year"] = Request.QueryString["year"].ToString();
					Session["Month"] = Request.QueryString["month"].ToString();
					Session["Day"] = Request.QueryString["day"].ToString();
					Session["componentId"] = Request.QueryString["componentId"].ToString();
					Session["employeeId"] = Request.QueryString["employeeId"].ToString();
					Session["plantId"] = Request.QueryString["plantId"].ToString();
                    Session["CellID"] = Request.QueryString["CellID"].ToString();
                    Session["View"] = Request.QueryString["View"].ToString();
					//Session["MachineDescription"] = Request.QueryString["machinedescription"].ToString();
					BindGridViewHeader();
					BindGridView();
				}
			}
		}

		private void BindGridViewHeader()
		{
			string ViewType = Session["View"].ToString();
			List<HeaderInf> listOfHeaders = new List<HeaderInf>();
			listOfHeaders.Add(new HeaderInf { HeaderName = GetLocalResourceObject("PlantID").ToString(), BounField = "PlantID" });
			listOfHeaders.Add(new HeaderInf { HeaderName = GetLocalResourceObject("MachineID").ToString(), BounField = "MachineID" });
			//if (ViewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
			//{
			//	listOfHeaders.Add(new HeaderInf { HeaderName = GetLocalResourceObject("MachineDescription").ToString(), BounField = "MachineDescription" });
			//}
			listOfHeaders.Add(new HeaderInf { HeaderName = GetLocalResourceObject("ComponentId").ToString(), BounField = "ComponentId" });
			listOfHeaders.Add(new HeaderInf { HeaderName = GetLocalResourceObject("Operation").ToString(), BounField = "Operationno" });
			listOfHeaders.Add(new HeaderInf { HeaderName = GetLocalResourceObject("OperatorID").ToString(), BounField = "OperatorID" });
			listOfHeaders.Add(new HeaderInf { HeaderName = GetLocalResourceObject("AcceptedParts").ToString(), BounField = "AcceptedParts" });
			listOfHeaders.Add(new HeaderInf { HeaderName = GetLocalResourceObject("AvgCycleTime").ToString(), BounField = "AvgCycletime" });
			//(new HeaderInf() { "PlantID", "MachineID", "ComponentId", "OperatorID", "Operationno", "AcceptedParts", "AvgCycletime" });
			for (int i = 0; i < listOfHeaders.Count; i++)
			{
				BoundField boundfield = new BoundField();
				if (i == 5)
				{
					boundfield.DataFormatString = "{0:N2}";
					boundfield.HtmlEncode = false;
				}
				if (ViewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
				{
					if (i != 0 && i != 1)
					{
						boundfield.DataField = listOfHeaders[i].BounField.ToString();
						boundfield.HeaderText = listOfHeaders[i].HeaderName.ToString();
						gridviewAcceptedData.Columns.Add(boundfield);
					}
				}
				if (ViewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
				{
					if (i != 0 && i != 1 && i != 4)
					{
						boundfield.DataField = listOfHeaders[i].BounField.ToString();
						boundfield.HeaderText = listOfHeaders[i].HeaderName.ToString();
						gridviewAcceptedData.Columns.Add(boundfield);
					}
				}
                if (ViewType.Equals("CellwiseView", StringComparison.OrdinalIgnoreCase))
                {
                    if (i != 0 && i != 1 && i != 4)
                    {
                        boundfield.DataField = listOfHeaders[i].BounField.ToString();
                        boundfield.HeaderText = listOfHeaders[i].HeaderName.ToString();
                        gridviewAcceptedData.Columns.Add(boundfield);
                    }
                }
                if (ViewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
				{
					if (i != 0 && i != 2)
					{
						boundfield.DataField = listOfHeaders[i].BounField.ToString();
						boundfield.HeaderText = listOfHeaders[i].HeaderName.ToString();
						gridviewAcceptedData.Columns.Add(boundfield);
					}
				}
				if (ViewType.Equals("OperatorwiseView", StringComparison.OrdinalIgnoreCase))
				{
					if (i != 0 && i != 4)
					{
						boundfield.DataField = listOfHeaders[i].BounField.ToString();
						boundfield.HeaderText = listOfHeaders[i].HeaderName.ToString();
						gridviewAcceptedData.Columns.Add(boundfield);
					}
				}
			}
		}

		private void BindGridView()
		{
			try
			{

				string strShift = Session["ShiftId"].ToString(), plantId = Session["plantId"].ToString(), strMonth = Session["Month"].ToString(),
					componentId = Session["componentId"].ToString(), employeeId = Session["employeeId"].ToString(), strYear = Session["Year"].ToString(),
					strDay = Session["Day"].ToString(), machineId = Session["MachineId"].ToString(), ViewType = Session["View"].ToString(),CellID= Session["CellID"].ToString();

				if (ViewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
				{
					ViewType = "machinewise";
				}
				else if (ViewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
				{
					ViewType = "plantwise";
					plantId = machineId;
					machineId = "";
				}
				else if (ViewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
				{
					ViewType = "componentwise";
					componentId = machineId;
					machineId = "";
				}
				else if (ViewType.Equals("OperatorwiseView", StringComparison.OrdinalIgnoreCase))
				{
					ViewType = "operatorwise";
					employeeId = machineId;
					machineId = "";
				}
                else if (ViewType.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
                {
                    ViewType = "cellwise";
                    CellID = machineId;
                    machineId = "";
                }
                else if (ViewType.Equals("OperatorwiseView", StringComparison.OrdinalIgnoreCase))
                {
                    ViewType = "operatorwise";
                    employeeId = machineId;
                    machineId = "";
                }
                if (strShift.Equals("All", StringComparison.OrdinalIgnoreCase))
					strShift = "";
				if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
					plantId = "";

				if (componentId.Equals("All", StringComparison.OrdinalIgnoreCase))
					componentId = "";
				if (employeeId.Equals("All", StringComparison.OrdinalIgnoreCase))
					employeeId = "";

				#region "Month condition------------------"
				if (strMonth.Equals("Jan", StringComparison.OrdinalIgnoreCase))
					strMonth = "01";
				else if (strMonth.Equals("Feb", StringComparison.OrdinalIgnoreCase))
					strMonth = "02";
				else if (strMonth.Equals("Mar", StringComparison.OrdinalIgnoreCase))
					strMonth = "03";
				else if (strMonth.Equals("Apr", StringComparison.OrdinalIgnoreCase))
					strMonth = "04";
				else if (strMonth.Equals("May", StringComparison.OrdinalIgnoreCase))
					strMonth = "05";
				else if (strMonth.Equals("Jun", StringComparison.OrdinalIgnoreCase))
					strMonth = "06";
				else if (strMonth.Equals("Jul", StringComparison.OrdinalIgnoreCase))
					strMonth = "07";
				else if (strMonth.Equals("Aug", StringComparison.OrdinalIgnoreCase))
					strMonth = "08";
				else if (strMonth.Equals("Sep", StringComparison.OrdinalIgnoreCase))
					strMonth = "09";
				else if (strMonth.Equals("Oct", StringComparison.OrdinalIgnoreCase))
					strMonth = "10";
				else if (strMonth.Equals("Nov", StringComparison.OrdinalIgnoreCase))
					strMonth = "11";
				else if (strMonth.Equals("Dec", StringComparison.OrdinalIgnoreCase))
					strMonth = "12";
				#endregion
				DataTable dt = new DataTable();
				if (strYear != "" && strMonth != "" && strDay != "")
				{
					strYear = strYear + "-" + strMonth + "-" + strDay;
					dt = BindCockpitView.AcceptedInformationData(strYear, strShift, plantId, machineId, "DAY", componentId, CellID, employeeId, ViewType);
				}
				else if (strYear != "" && strMonth != "")
				{
					strYear = strYear + "-" + strMonth + "-" + "01";
					dt = BindCockpitView.AcceptedInformationData(strYear, strShift, plantId, machineId, "MONTH", componentId, CellID, employeeId, ViewType);
				}
				else if (strYear != "")
				{
					strYear = strYear + "-" + "01" + "-" + "01";
					dt = BindCockpitView.AcceptedInformationData(strYear, strShift, plantId, machineId, "YEAR", componentId, CellID, employeeId, ViewType);
				}
				gridviewAcceptedData.DataSource = dt;
				gridviewAcceptedData.DataBind();
			}
			catch (Exception ex)
			{
				ErrorSignal.FromCurrentContext().Raise(ex);
				lblMessage.Text = ex.ToString();
			}
		}
	}
	class HeaderInf
	{
		public string HeaderName { get; set; }
		public string BounField { get; set; }
	}
}