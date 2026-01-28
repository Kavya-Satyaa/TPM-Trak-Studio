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
    public partial class HourlyEnergyData : System.Web.UI.Page
    {
		DateTime FromDate = DateTime.Now.Date;
		DateTime ToDate = DateTime.Now.Date;
		public static DataTable hourlyEnergyData = new DataTable();
        string shift = "A";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "showLoader();", true);
            if (!IsPostBack)
            {
                if (Request.QueryString["machineId"] != "")
                {
                    Session["MachineId"] = Request.QueryString["machineId"].ToString();
                    Session["NodeID"] = Request.QueryString["NodeID"].ToString();
                    shift = Request.QueryString["Shift"].ToString();
                    string fromDate = Request.QueryString["FromDate"].ToString();
                    string toDate = Request.QueryString["ToDate"].ToString();
                    lblTitle.Text = " Hourly Energy Data For - " + Session["MachineId"] + " (" + Session["NodeID"].ToString() + ")";
                    BindHourlyEnergyDataGrid(Session["MachineId"].ToString(), Session["NodeID"].ToString(), fromDate, toDate, shift, "Hour");
                    lblDate.Text = "    " + "Date : " + Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy");
                    lblShift.Text = "    " + "Shift : " + shift;
                }
            }
        }

        private void BindHourlyEnergyDataGrid(string machineId, string nodeId, string fromDate, string toDate, string shift, string parameter)
        {
			FromDate = Util.GetDateTime(fromDate);
			ToDate = Util.GetDateTime(toDate);
			hourlyEnergyData = DataBaseAccess.GetEnergyData(FromDate, ToDate, shift, machineId, "", nodeId, parameter, "", "", "S_GetSONA_EnergyCockpitDetails");
            gridViewHourlyEnergyData.DataSource = hourlyEnergyData;
            gridViewHourlyEnergyData.DataBind();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<EnergyChartData> GetHourlyEnergyChartData(string param)
        {
            double val = 0;
            List<EnergyChartData> hourlyEnergyChartDataList = new List<EnergyChartData>();
            EnergyChartData energyChartData = null;
            try
            {
                foreach (DataRow row in hourlyEnergyData.Rows)
                {
                    energyChartData = new EnergyChartData();
                    energyChartData.name = row["ShiftHourID"].ToString();
                    if (Double.TryParse(row["KW"].ToString(), out val))
                    {
                        energyChartData.y = val;
                    }
                    else
                    {
                        energyChartData.y = 0;
                    }
                    hourlyEnergyChartDataList.Add(energyChartData);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return hourlyEnergyChartDataList;
        }
    }
}