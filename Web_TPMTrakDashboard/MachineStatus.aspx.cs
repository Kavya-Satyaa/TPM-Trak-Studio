using BusinessClassLibrary;
using MachineStatusPage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
namespace Web_TPMTrakDashboard
{
    public partial class MachineStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                BindPlant();
            }
        }


        #region "Bind Plant Info"
        private void BindPlant()
        {
            try
            {
                var lstPlantData = BindCockpitView.ViewPlantToDisplay();
                ddlPlantId.DataSource = lstPlantData;
                ddlPlantId.DataBind();
                ddlPlantId.Items.Insert(0, new ListItem
                {
                    Text = GetGlobalResourceObject("CommanResource","PlantAll").ToString(),
                    Value = "All"
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region "Bind Machine Info"
        [WebMethod]
        public static List<string> GetMachineInfo(string plantId)
        {
            return VDGDataBaseAccess.GetAllMachines(plantId);
        }
        #endregion

        #region "Bind Machine Status Info"
        [WebMethod]
        public static List<MachineData> BindMachineStatus(string plantId, string machineId)
        {
            if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
                plantId = "";
            if (machineId.Equals("All", StringComparison.OrdinalIgnoreCase) || machineId.Equals("null", StringComparison.OrdinalIgnoreCase))
                machineId = "";

            return DataBaseAccess.MachineStatusInfo(plantId, machineId); 
        }
        #endregion
    }
}