using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik184.Models;

namespace Web_TPMTrakDashboard.Advik184
{
    public partial class PlantStatusAndon : System.Web.UI.Page
    {
        public static string thresholdColor = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                BindLine();
                BindData();
                timer1.Enabled = true;
            }
        }
        public void BindLine()
        {
            List<string> list = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllPlants();
            if (list.Count > 0)
            {
                list.Remove("All");
            }
            ddlPlant.DataSource = list;
            ddlPlant.DataBind();
        }
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
        private void BindData()
        {
            try
            {
                if(timer1==null || timer1.Enabled == false)
                {
                    timer1.Enabled = true;
                }
                List<FinalInspectionEnity> list = new List<FinalInspectionEnity>();
                list = AdvikDatabaseAccess.getPlantStatusAndon(ddlPlant.SelectedValue);
                lvStatusData.DataSource = list;
                lvStatusData.DataBind();
                DataTable dtSetting = AdvikDatabaseAccess.getAdvikSettingDetails();
                if (dtSetting.Rows.Count > 0)
                {
                    thresholdColor = dtSetting.AsEnumerable().Where(k => k.Field<string>("ValueInText") == "ThresholdColor").Select(k => k.Field<string>("ValueInText2")).FirstOrDefault().ToString();
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void timer1_Tick(object sender, EventArgs e)
        {
            BindData();
        }
    }
}