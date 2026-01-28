using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.MachineConnect.Model;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.PTA;

namespace Web_TPMTrakDashboard.MachineConnect
{
    public partial class SpindleParameterDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string shiftStartTime, shiftEndTime, shiftname;
                shiftStartTime = DataBaseAccessPTA.CurrentStartEndTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), out shiftEndTime, out shiftname);
                txtFromDateTime.Text = Util.GetDateTime(shiftStartTime).ToString("dd-MM-yyyy HH:mm:ss");
                txtToDateTime.Text = Util.GetDateTime(shiftEndTime).ToString("dd-MM-yyyy HH:mm:ss");
                BindPlant();
            }
        }
        private void BindPlant()
        {
            try
            {
                ddlPlant.DataSource = BindCockpitView.ViewPlantToDisplay();
                ddlPlant.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> getShiftDate()
        {
            List<string> list = new List<string>();
            try
            {
                string shiftStartTime, shiftEndTime, shiftname;
                shiftStartTime = DataBaseAccessPTA.CurrentStartEndTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), out shiftEndTime, out shiftname);
                list.Add(Util.GetDateTime(shiftStartTime).ToString("dd-MM-yyyy HH:mm:ss"));
                list.Add(Util.GetDateTime(shiftEndTime).ToString("dd-MM-yyyy HH:mm:ss"));
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return list;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> getMachineData(string plant)
        {
            List<string> list = new List<string>();
            try
            {
                list = VDGDataBaseAccess.GetMachinesbyPlantCell(plant, "");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return list;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> getAxisNoData(string machine)
        {
            List<string> list = new List<string>();
            try
            {
                list = MachineConnectDBAccess.getAxisNumberData(machine);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return list;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static SpindleChartEntity getChartData(string plant, string machine, string axisNo, string fromDate, string toDate)
        {
            SpindleChartEntity chartData = new SpindleChartEntity();
            try
            {
                chartData = MachineConnectDBAccess.getSpindleParameterDashboardDetails(fromDate, toDate, machine, axisNo);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return chartData;
        }
    }
}