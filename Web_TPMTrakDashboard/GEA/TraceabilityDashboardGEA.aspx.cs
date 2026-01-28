using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;
using Web_TPMTrakDashboard.GEA.Models;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class TraceabilityDashboardGEA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static TraceabilityDashboardEntity getTraceabilityData(string viewType)
        {
            TraceabilityDashboardEntity dashboardData = new TraceabilityDashboardEntity();
            try
            {
                dashboardData = GEADatabaseAccess.getMaterialTraceabilityDashboardDetails(viewType);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return dashboardData;
        }
    }
}