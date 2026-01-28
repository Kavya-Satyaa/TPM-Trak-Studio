using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.KTASpindle
{
    public partial class AndonSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Binddata();
            }
          
        }

        private void Binddata()
        {
            try
            {
                List<SettingsEntity> entities = new List<SettingsEntity>();
                entities = DBAccess.GetSettingsData();
                ddlHeaderFontsize.SelectedValue = entities[0].HeaderFontSize.ToString();
                ddlContentFontSize.SelectedValue = entities[0].ContentFontSize.ToString();
                flipTime.Text = entities[0].FlipInterval.ToString();
                downCode.Text = entities[0].TopDownCode.ToString();
                ddlDisplayType.SelectedValue = entities[0].DisplayType.ToString();

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
                if (ddlHeaderFontsize != null && ddlContentFontSize != null && !string.IsNullOrEmpty(flipTime.Text) && !string.IsNullOrEmpty(downCode.Text) && ddlDisplayType != null)
                {
                    UpdateData("", ddlHeaderFontsize.SelectedValue.ToString(), "Header Font Size", "int");
                    UpdateData("", ddlContentFontSize.SelectedValue.ToString(), "Content Font Size", "int");
                    UpdateData("", flipTime.Text, "Flip Time", "int");
                    UpdateData("", downCode.Text, "Top DownCode", "int");
                    UpdateData(ddlDisplayType.SelectedValue.ToString(), "", "Display Type", "string");
                }
                Binddata();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
           
        }

        private void UpdateData(string ValueInText2, string ValueInInt, string ValueInText,string datatype)
        {
            try
            {
                DBAccess.SaveSettings("KTAWebAndon", ValueInText, ValueInText2, ValueInInt, datatype);

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    
    }
}