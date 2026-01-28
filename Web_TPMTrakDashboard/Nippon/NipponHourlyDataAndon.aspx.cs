using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.Nippon
{
    public partial class NipponHourlyDataAndon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        private void LoadData()
        {
            try
            {
                List<string> CellID = BindCockpitView.ViewCellsToDisplay("");
                CellID.Insert(0, "All");
                drpcellid.DataSource = CellID;
                drpcellid.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<NipponAndonHourlyDataEntity> GETDATA(string CellID)
        {
            List<NipponAndonHourlyDataEntity> Entity = new List<NipponAndonHourlyDataEntity>();
            try
            {
                Entity.Add(new NipponAndonHourlyDataEntity { MachineID = "646", Production = "1200", OEE = "81.00%", Productivity = "90%", Avaliability = "90%", Quality = "100" });
                Entity.Add(new NipponAndonHourlyDataEntity { MachineID = "627", Production = "1020", OEE = "81.00%", Productivity = "90%", Avaliability = "90%", Quality = "100" });
                Entity.Add(new NipponAndonHourlyDataEntity { MachineID = "555", Production = "900", OEE = "81.00%", Productivity = "90%", Avaliability = "90%", Quality = "100" });
                Entity.Add(new NipponAndonHourlyDataEntity { MachineID = "668", Production = "1200", OEE = "81.00%", Productivity = "90%", Avaliability = "90%", Quality = "100" });
                Entity.Add(new NipponAndonHourlyDataEntity { MachineID = "484", Production = "350", OEE = "81.00%", Productivity = "90%", Avaliability = "90%", Quality = "100" });
                Entity.Add(new NipponAndonHourlyDataEntity { MachineID = "484A", Production = "76", OEE = "81.00%", Productivity = "90%", Avaliability = "90%", Quality = "100" });
                
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return Entity;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static void RefreshData()
        {
            try
            {
                HttpContext.Current.Session["EffAndonRowsToTake"] = 5;
                HttpContext.Current.Session["EffRowTaken"] = 0;
                HttpContext.Current.Session["EffAndonCount"] = 5;
                HttpContext.Current.Session["EffAndonData"] = null;
                HttpContext.Current.Session["ShiftName"] = "";
            }

            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
    }
}