using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.HighWay
{
    public partial class AndonSettings_Highway : System.Web.UI.Page
    {
        public static AndonParameters_Highway settings = new AndonParameters_Highway();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HttpContext.Current.Request.Cookies["ComputerName"] != null)
                {
                    lblComputerName.Text = HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString();
                }
                    BindGrid();
            }
        }
        private void BindGrid()
        {
            try
            {
                DataTable dt = DBAccess.GetAndonSettingData(lblComputerName.Text);
                if (dt.Rows.Count > 0)
                {
                    txtMainHeaderfontsize.Text = dt.AsEnumerable().Where(x => x["ValueInText"].ToString().Equals("MainHeaderFontSize", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault();
                    //txtSubheaderFontsize.Text = dt.AsEnumerable().Where(x => x.Field<string>("ValueInText").ToString().Equals("SubHeaderFontSize", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault();
                    txtEfficiencyFontsize.Text = dt.AsEnumerable().Where(x => x.Field<string>("ValueInText").ToString().Equals("EfficiencyFontSize", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault();
                    txtEfficiencyHeaderFontsize.Text = dt.AsEnumerable().Where(x => x.Field<string>("ValueInText").ToString().Equals("EfficiencyHeaderFontSize", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault();
                    //txtSubheaderContent.Text = dt.AsEnumerable().Where(x => x.Field<string>("ValueInText").ToString().Equals("SubHeaderText", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault();
                    txtRefreshInterval.Text = dt.AsEnumerable().Where(x => x.Field<string>("ValueInText").ToString().Equals("DataRefreshInterval", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnSAve_Click(object sender, EventArgs e)
        {
            try
            {
                string MainHeaderFontsize = DBAccess.SaveAndonSettings_Highway("HighwayAndon", "MainHeaderFontSize", txtMainHeaderfontsize.Text);
                //string SubHeaderFontsize = DBAccess.SaveAndonSettings_Highway("HighwayAndon", "SubHeaderFontSize", txtSubheaderFontsize.Text);
                string EfficiencyFontsize = DBAccess.SaveAndonSettings_Highway("HighwayAndon", "EfficiencyFontSize", txtEfficiencyFontsize.Text);
                string EfficiencyHeaderFontsize = DBAccess.SaveAndonSettings_Highway("HighwayAndon", "EfficiencyHeaderFontSize", txtEfficiencyHeaderFontsize.Text);
                //string SubHeaderText = DBAccess.SaveAndonSettings_Highway("HighwayAndon", "SubHeaderText", txtSubheaderContent.Text);
                string DatarefreshInterval = DBAccess.SaveAndonSettings_Highway("HighwayAndon", "DataRefreshInterval", txtRefreshInterval.Text);
                if (MainHeaderFontsize ==""|| EfficiencyFontsize=="" ||  DatarefreshInterval=="")
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Failed to Update!!!");
                }
                else
                {
                    HelperClassGeneric.openSuccessModal(this, "Updated Successfully");
                    BindGrid();
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnAndon_Click(object sender, EventArgs e)
        {
            Response.Redirect("Andon_Highway_New.aspx");
        }
    }
}