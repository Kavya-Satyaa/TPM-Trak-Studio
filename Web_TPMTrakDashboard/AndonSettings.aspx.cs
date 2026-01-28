using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class AndonSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetPlantData()
        {
            List<string> Plantid = new List<string>();
            try
            {
                Plantid = BindCockpitView.ViewPlantToDisplay();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return Plantid;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static AndonSettingsData GetData()
        {
            AndonSettingsData Data = new AndonSettingsData();
            try
            {
                Data = DataBaseAccess.GetAndonData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return Data;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveDate(string PlantID, string RefreshInterval, string Title, string Type)
        {
            string msg = "";
            try
            {
                msg = DataBaseAccess.SaveAndonSettingsdata(PlantID, RefreshInterval, Title, Type);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return msg;
        }
    }

    public class AndonSettingsData
    {
        public string PlantID { get; set; }
        public int RefreshData { get; set; }
        public string AndonTitle { get; set; }
        public string Type { get; set; }
    }
}