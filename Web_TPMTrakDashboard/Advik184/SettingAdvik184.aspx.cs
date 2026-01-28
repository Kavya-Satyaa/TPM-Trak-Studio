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
    public partial class SettingAdvik184 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        private void BindData()
        {
            try
            {
                DataTable dt = AdvikDatabaseAccess.getAdvikSettingDetails();
                if (dt.Rows.Count > 0)
                {
                    txtThresholdTime.Text = dt.AsEnumerable().Where(k => k.Field<string>("ValueInText") == "ThresholdTime").Select(k => k.Field<int>("ValueInInt")).FirstOrDefault().ToString();
                    txtThresholdColor.Text = dt.AsEnumerable().Where(k => k.Field<string>("ValueInText") == "ThresholdColor").Select(k => k.Field<string>("ValueInText2")).FirstOrDefault().ToString().Substring(1);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                result += AdvikDatabaseAccess.saveShopDefaultValueInIntByText("Advik184", "ThresholdTime", txtThresholdTime.Text);
                result += AdvikDatabaseAccess.saveShopDefaultValueInText2ByText("Advik184", "ThresholdColor", "#" + txtThresholdColor.Text);
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Saved Successfully.')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openErrorModal('Insertion failed.')", true);
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}