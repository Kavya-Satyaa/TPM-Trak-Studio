using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GenericAndon.Model;

namespace Web_TPMTrakDashboard.KTASpindle
{
    public partial class AndonSettingsKTA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["connectionString"] == null)
            {
                Response.Redirect("../SignIn.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    setCompanyLogo();
                    BindPlant();
                    BindCellID();
                    BindSettings();
                }
            }
        }

        private void setCompanyLogo()
        {
            try
            {
                string ImagesPath = "~/CompanyLogo/";
                var dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(ImagesPath));

                List<string> fileNames = (from fileInfo in dir.GetFiles() select fileInfo.Name).ToList();
                if (fileNames.Count > 0)
                {
                    CompanyLogo.ImageUrl = ImagesPath + fileNames[0];
                }
                else
                {
                    CompanyLogo.ImageUrl = "Image/companyIcon.png";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("setCompanyLogo: " + ex);
            }
        }

        private void BindSettings()
        {
            try
            {
                SettingsViewEntityStore entity = DBAccess.ViewSettings();
                if (entity != null)
                {
                    ddlPlant.SelectedValue = entity.PlantToDisplay;
                    txtAndonTitle.Text = entity.AndonTitle;
                    ddlFontName.SelectedValue = entity.FontFamily;
                    ddlFontStyle.SelectedValue = entity.FontStyle;
                    ddlDataRefreshInterval.SelectedValue = entity.DataDisplayInterval.ToString();
                    ddlScreenFlipInterval.SelectedValue = entity.ScreenFlipInterval.ToString();
                    ddlDate.SelectedValue = entity.DateFormat;
                    ddlTime.SelectedValue = entity.TimeFormat;
                }

                SettingsViewEntityStore viewSettings = DBAccess.ViewSettingsDetails(ddlCellID.SelectedValue);
                if(viewSettings!=null)
                {
                    txtProductionMachineFontSize.Text = viewSettings.MachineFontSize;
                    ddlHeaderFontSize.SelectedValue = viewSettings.KTAHeaderFontSize;
                    ddlContentFontSize.SelectedValue = viewSettings.KTAContentFontSize;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindSettings" + ex);
            }
        }

        private void BindPlant()
        {
            try
            {
                List<string> plants = DBAccess.GetPlantID();
                ddlPlant.DataSource = plants;
                ddlPlant.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Bind Plant ID: " + ex.Message);
            }
        }

        private void BindCellID()
        {
            try
            {
                List<string> Cells = AndonDBAccess.getCellID("");
                ddlCellID.DataSource = Cells;
                ddlCellID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Bind Cell ID: " + ex.ToString());
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string isSuccessful = string.Empty;

                DBAccess.UpdateAndonSettings("StoreAndonSetting", "ANDonTitle", txtAndonTitle.Text.ToString(), out isSuccessful);
                DBAccess.UpdateAndonSettings("StoreAndonSetting", "PlantToDisplay", ddlPlant.SelectedValue.ToString(), out isSuccessful);
                DBAccess.UpdateAndonSettings("StoreAndonSetting", "DataDisplayInterval", ddlDataRefreshInterval.SelectedValue.ToString(), out isSuccessful);
                DBAccess.UpdateAndonSettings("StoreAndonSetting", "ScreenFlipInterval", ddlScreenFlipInterval.SelectedValue.ToString(), out isSuccessful);
                DBAccess.UpdateAndonSettings("StoreAndonSetting", "FontFamily", ddlFontName.SelectedValue.ToString(), out isSuccessful);
                DBAccess.UpdateAndonSettings("StoreAndonSetting", "FontStyle", ddlFontStyle.SelectedValue.ToString(), out isSuccessful);
                DBAccess.UpdateAndonSettings("StoreAndonSetting", "DateFormatForHeader", ddlDate.SelectedValue.ToString(), out isSuccessful);
                DBAccess.UpdateAndonSettings("StoreAndonSetting", "TimeFormatForHeader", ddlTime.SelectedValue.ToString(), out isSuccessful);

                if (isSuccessful.Equals("Successful"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmessage", "alert('Succesfully Updated')", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnAddon_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoreAndonKTA.aspx");
        }

        protected void btnSettingsSave_Click(object sender, EventArgs e)
        {
            try
            {
                int index = 0;
                string Column = string.Empty, customColumn = string.Empty, SortOrder = string.Empty, chkVal = string.Empty, TextAlign = string.Empty, LabelFontSize = string.Empty, DataFontSize = string.Empty, isSuccessful = string.Empty;
               
                DBAccess.UpdateGridSettingData("StoreAndonSetting", "MachineNameFontSize", txtProductionMachineFontSize.Text.ToString() , string.Empty, "true", string.Empty, string.Empty, string.Empty, ddlCellID.SelectedValue.ToString(), out isSuccessful);
                DBAccess.UpdateGridSettingData("StoreAndonSetting", "HeaderFontSize", string.Empty, string.Empty, "true", string.Empty,ddlHeaderFontSize.SelectedValue , string.Empty, ddlCellID.SelectedValue.ToString(), out isSuccessful);
                DBAccess.UpdateGridSettingData("StoreAndonSetting", "ContentFontSize", string.Empty, string.Empty, "true", string.Empty,string.Empty, ddlContentFontSize.SelectedValue, ddlCellID.SelectedValue.ToString(), out isSuccessful);

                if (isSuccessful.Equals("Successful"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Succesfully Updated')", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSettings();
        }

        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellID();
            ddlCellID_SelectedIndexChanged(null, null);
        }
    }
}