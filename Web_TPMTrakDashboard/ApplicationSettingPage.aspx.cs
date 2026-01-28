using Elmah;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.KTASpindle;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class ApplicationSettingPage : System.Web.UI.Page
    {
        DataTable dtMachineSortOrder = new DataTable();
        DataTable dtMachineStatusColorCodes = new DataTable();
        bool losses = Util.show16losses;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                else
                    ddlLanguage.SelectedValue = Session["Language"].ToString();
                SessionClear.ClearSession();
                ddlSettingLst_SelectedIndexChanged(null, null);
                BindTPMTrakDown();
                BindColumnViewDropdown();
            }
        }

        private void HideAllDiv()
        {
            try
            {
                divMachineEfficiencyColor.Visible = false;
                divColumnViewSetting.Visible = false;
                divMachineStatusColor.Visible = false;
                divApplicationSetting.Visible = false;
                divCompanyLogo.Visible = false;
                divMachineSortOrder.Visible = false;
                divDashboardDefaultView.Visible = false;
                divHistAnalyticsCockpitDefView.Visible = false;
                divLiveAnalyticsCockpitDefView.Visible = false;
                MultilingualSetting.Visible = false;
                divProgramTransferSetting.Visible = false;
                divEshopxSetting.Visible = false;
                divCockpitBackColor.Visible = false;
                divDataCollection.Visible = false;
                divModifiedDataSettings.Visible = false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void BindColumnViewDropdown()
        {
            try
            {
                if (ConfigurationManager.AppSettings["ShowProductionandDownDataInVDG"].ToString() == "1")
                    ddlGridPages.Items.Insert(6, new ListItem { Text = "Cycle Analytics - Production and Down View", Value = "WebTPMTrakVDGProductionandDown" });
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void ddlSettingLst_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HideAllDiv();
                btnSave.Visible = true;
                if (ddlSettingLst.SelectedValue.ToString().Equals("MachineEfficiencyColor", StringComparison.OrdinalIgnoreCase))
                    SetColorValues();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("ColumnView", StringComparison.OrdinalIgnoreCase))
                    BindGridview();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("MachineStatusColor", StringComparison.OrdinalIgnoreCase))
                    BindStatusColorCodesGrid();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("ApplicationSetting", StringComparison.OrdinalIgnoreCase))
                    AppSettingData();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("CompanyLogoUpload", StringComparison.OrdinalIgnoreCase))
                {
                    divCompanyLogo.Visible = true;
                    btnSave.Visible = false;
                }
                else if (ddlSettingLst.SelectedValue.ToString().Equals("MachineCustomSortOrder", StringComparison.OrdinalIgnoreCase))
                    BindgvMachineSortOrder();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("DashboardDefaultView", StringComparison.OrdinalIgnoreCase))
                    setDashboarddefaulttpevalues();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("HistoricalAnalyticsCockpitDefaultView", StringComparison.OrdinalIgnoreCase))
                {
                    divHistAnalyticsCockpitDefView.Visible = true;
                    BindLiveAHistoricalData();
                }
                else if (ddlSettingLst.SelectedValue.ToString().Equals("LiveAnalyticsCockpitDefaultView", StringComparison.OrdinalIgnoreCase))
                {
                    divLiveAnalyticsCockpitDefView.Visible = true;
                    BindLiveAHistoricalData();
                }
                else if (ddlSettingLst.SelectedValue.ToString().Equals("UpdatePageStatics", StringComparison.OrdinalIgnoreCase))
                    BindResourceFile();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("EshopxSetting", StringComparison.OrdinalIgnoreCase))
                    BindEshopxSetting();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("CockpitBackColorSetting", StringComparison.OrdinalIgnoreCase))
                    BindCockpitBackColorSetting();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("EfficiencyColor", StringComparison.OrdinalIgnoreCase))
                {

                    string URL = "OEQEPEColor.aspx";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Effcolor", "OpenEfficiencyColorView('" + URL + "')", true);
                }
                else if (ddlSettingLst.SelectedValue.ToString().Equals("DataCollection", StringComparison.OrdinalIgnoreCase))
                    BindDataCollection();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("ModifiedDataSettings", StringComparison.OrdinalIgnoreCase))
                    BindModifiedDataSettings();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlSettingLst.SelectedValue.ToString().Equals("MachineEfficiencyColor", StringComparison.OrdinalIgnoreCase))
                    SaveColorValues();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("ColumnView", StringComparison.OrdinalIgnoreCase))
                    SaveColumnSetting_Click();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("MachineStatusColor", StringComparison.OrdinalIgnoreCase))
                    btnSaveColorCodes_Click();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("ApplicationSetting", StringComparison.OrdinalIgnoreCase))
                    btnSaveGeneralPageSetting_Click();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("MachineCustomSortOrder", StringComparison.OrdinalIgnoreCase))
                    MachineSortsave_Click();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("DashboardDefaultView", StringComparison.OrdinalIgnoreCase))
                    btnDashboardDefaultsave_Click();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("HistoricalAnalyticsCockpitDefaultView", StringComparison.OrdinalIgnoreCase))
                    btnHAViewSave_Click();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("LiveAnalyticsCockpitDefaultView", StringComparison.OrdinalIgnoreCase))
                    btnLAViewSave_Click();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("UpdatePageStatics", StringComparison.OrdinalIgnoreCase))
                    btnMultilingualSave_Click();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("EshopxSetting", StringComparison.OrdinalIgnoreCase))
                    btnEshopxSave_Click();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("CockpitBackColorSetting", StringComparison.OrdinalIgnoreCase))
                    SaveCockpitBackColorSetting();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("DataCollection", StringComparison.OrdinalIgnoreCase))
                    SaveDataCollection();
                else if (ddlSettingLst.SelectedValue.ToString().Equals("ModifiedDataSettings", StringComparison.OrdinalIgnoreCase))
                    SaveModifiedDataSettings();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
           
        }

    
        #region --MAchineEffColor ---
        private void SetColorValues()
        {
            try
            {
                divMachineEfficiencyColor.Visible = true;
                ICockpitStyle values = CockpitDataBaseAccess.GetCockpitBackColorValues();
                txtGood.Text = values.GoodRunning.Substring(3);
                txtModerate.Text = values.ModeratelyRunning.Substring(3);
                txtBad.Text = values.BadlyRunning.Substring(3);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void SaveColorValues()
        {
            int result = 0;
            try
            {
                result = CockpitDataBaseAccess.UpdateColorSettings("CockpitBackColor", "GoodColor", "#FF" + txtGood.Text.ToUpper());
                result = CockpitDataBaseAccess.UpdateColorSettings("CockpitBackColor", "ModerateColor", "#FF" + txtModerate.Text.ToUpper());
                result = CockpitDataBaseAccess.UpdateColorSettings("CockpitBackColor", "BadColor", "#FF" + txtBad.Text.ToUpper());
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        #endregion

        #region --- Column view setting---
        private void BindGridview()
        {
            try
            {
                divColumnViewSetting.Visible = true;
                grdViewAndonView.Columns[2].Visible = false;
                grdViewAndonView.DataSource = DataBaseAccess.BindSettingPage("WebTPMTrak", Session["Language"] == null ? "en" : Session["Language"].ToString());
                grdViewAndonView.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        
        private void SaveColumnSetting_Click()
        {
            try
            {
                string valueInText = string.Empty, valueInText2 = string.Empty, valueinint = string.Empty;
                int result = 0, valueInBool, valueInInt = 0;
                foreach (GridViewRow row in grdViewAndonView.Rows)
                {
                    valueInText = ((Label)row.FindControl("lblValueInText")).Text;
                    valueInText2 = ((TextBox)row.FindControl("txtValueInText2")).Text;
                    valueInBool = ((CheckBox)row.FindControl("chkSelect")).Checked == true ? 1 : 0;
                    if (ddlGridPages.SelectedValue == "TPMTrakWebEnergyViewColumnSettings")
                        valueInBool = 1;
                    if (ddlGridPages.SelectedValue == "CockpitGridColumn" || ddlGridPages.SelectedValue == "WebTPMTrakTableView" || ddlGridPages.SelectedValue.ToString().Equals("WebCockpitGridColumnAggregate", StringComparison.OrdinalIgnoreCase))
                    {
                        valueinint = ((TextBox)row.FindControl("txtValueInInt")).Text;
                        valueInInt = Convert.ToInt32(valueinint);
                        result = DataBaseAccess.UpdateColumnViewSettingsIonic(ddlGridPages.SelectedValue, valueInText2, valueInText, valueInBool, valueInInt, ddlLanguage.SelectedValue.ToString());
                    }
                    else
                    {
                        result = DataBaseAccess.UpdateColumnViewSettings(ddlGridPages.SelectedValue, valueInText2, valueInText, valueInBool, ddlLanguage.SelectedValue.ToString());
                    }

                }
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk();", true);
                    Session["Language"] = ddlLanguage.SelectedValue.ToString();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        protected void ddlGridPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Selected = ddlGridPages.SelectedValue;
            if (ddlGridPages.SelectedItem.Text == "Live Analytics - Iconic View" || ddlGridPages.SelectedItem.Text == "Live Analytics - Table View" || ddlGridPages.SelectedValue.ToString() == "WebCockpitGridColumnAggregate")
            {
                grdViewAndonView.Columns[2].Visible = true;
            }
            else
            {
                grdViewAndonView.Columns[2].Visible = false;
            }
            if (ddlGridPages.SelectedItem.Text == "Energy Dashboard" || Selected == "ComponentInformation" || Selected == "EnergyLiveData")
            {
                grdViewAndonView.Columns[3].Visible = false;
            }
            else
                grdViewAndonView.Columns[3].Visible = true;
            grdViewAndonView.DataSource = DataBaseAccess.BindSettingPage(Selected, ddlLanguage.SelectedValue.ToString());
            grdViewAndonView.DataBind();
        }

        protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Selected = ddlGridPages.SelectedValue;
            grdViewAndonView.DataSource = DataBaseAccess.BindSettingPage(Selected, ddlLanguage.SelectedValue.ToString());
            grdViewAndonView.DataBind();
        }
        #endregion

        #region  --Machine StatusColor----
        private void BindStatusColorCodesGrid()
        {
            try
            {
                divMachineStatusColor.Visible = true;
                dtMachineStatusColorCodes = DataBaseAccess.GetMachineStatusColorCodes();
                if (dtMachineStatusColorCodes != null && dtMachineStatusColorCodes.Rows.Count > 0)
                {
                    GridViewColorCodes.DataSource = dtMachineStatusColorCodes;
                    GridViewColorCodes.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
        }

        private void btnSaveColorCodes_Click()
        {
            bool IsUpdated = false;
            try
            {
                foreach (GridViewRow row in GridViewColorCodes.Rows)
                {
                    string Status = ((Label)row.FindControl("lblStatus")).Text;
                    string Color = ((TextBox)row.FindControl("txtColorPicker")).Text;
                    DataBaseAccess.UpdateMachineStatusColorCodes(Status, Color, out IsUpdated);
                }
                if (IsUpdated)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        #endregion

        #region ---Application Setting ---
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMonth.SelectedValue = "None";
            ddlDate.SelectedValue = "None";
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlYear.SelectedValue = "None";
            ddlDate.SelectedValue = "None";
        }

        protected void ddlDate_SelectedIndexChanged1(object sender, EventArgs e)
        {
            ddlYear.SelectedValue = "None";
            ddlMonth.SelectedValue = "None";
        }
        private void AppSettingData()
        {
            try
            {
                divApplicationSetting.Visible = true;
                BindFontSize();
                BindCockpitFontSize();
                BindFontFamily();
                BindFontStyle();
                BindSmileyImageSize();
                BindTPMTrakDown();
                AppUISettings model = DataBaseAccess.ViewAppUISettings();
                if (model != null)
                {
                    if (!string.IsNullOrEmpty(model.FontSize))
                        ddlFontSize.SelectedValue = model.FontSize;
                    if (!string.IsNullOrEmpty(model.FontFamily))
                        ddlFontFamily.SelectedValue = model.FontFamily;
                    if (!string.IsNullOrEmpty(model.FontStyle))
                        ddlFontStyle.SelectedValue = model.FontStyle;
                    if (!string.IsNullOrEmpty(model.DownTime))
                        ddlDowntime.SelectedValue = model.DownTime;
                    if (!string.IsNullOrEmpty(model.ShowSmileyImage) && (model.ShowSmileyImage == "1"))
                        chkShowSmileyImg.Checked = true;
                    else
                        chkShowSmileyImg.Checked = false;
                    if (!string.IsNullOrEmpty(model.SmileyImageSize))
                        ddlSmileImageSize.SelectedValue = model.SmileyImageSize;
                    if (!string.IsNullOrEmpty(model.CockpitFontSize))
                        ddlCockpitFontSize.SelectedValue = model.CockpitFontSize;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void btnSaveGeneralPageSetting_Click()
        {
            string fontSize = string.Empty, fontFamily = string.Empty, fontStyle = string.Empty,
                downTime = string.Empty, showImage = string.Empty, imageSize = string.Empty, downTimeText;
            int result = 0;
            try
            {
                fontSize = ddlFontSize.SelectedValue;
                result = DataBaseAccess.applicationSettings("TPMTrakAppSettings", "FormFontSize", fontSize, "");
                fontFamily = ddlFontFamily.SelectedValue;
                result = DataBaseAccess.applicationSettings("TPMTrakAppSettings", "FontFamily", fontFamily, "");
                fontStyle = ddlFontStyle.SelectedValue;
                result = DataBaseAccess.applicationSettings("TPMTrakAppSettings", "FontStyle", fontStyle, "");
                downTime = ddlDowntime.SelectedValue;
                result = DataBaseAccess.applicationSettings("TPMTrakAppSettings", "Downtime", downTime, "");

                result = DataBaseAccess.applicationSettings("TPMTrakAppSettings", "CockpitFontSize", ddlCockpitFontSize.SelectedValue.ToString(), "");
                showImage = chkShowSmileyImg.Checked.ToString();
                result = DataBaseAccess.applicationSettings("TPMTrakAppSettings", "ShowSmileyImage", showImage == "True" ? "1" : "0", "");

                if (ddlSmileImageSize.SelectedIndex > 0)
                    imageSize = ddlSmileImageSize.SelectedValue.ToString();
                else
                    imageSize = "";
                result = DataBaseAccess.applicationSettings("TPMTrakAppSettings", "SmileyImageSize", imageSize, "");
                downTimeText = DataBaseAccess.GetDownTimeText(ddlLanguage.SelectedValue.ToString());
                downTime = ddlDowntime.SelectedValue.ToString();
                string[] words = downTimeText.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);//.Split("/");
                downTimeText = words[0] + " " + "(" + downTime + ")";
                result = DataBaseAccess.applicationSettings("WebTPMTrak", "Downtime/Stoppage(min/hh:mm)", downTimeText, ddlLanguage.SelectedValue.ToString());
                result = DataBaseAccess.applicationSettings("ExcludeTPMTrakDown", "", checkboxtoHide16losses.Checked == true ? "1" : "0", "");
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk();", true);
                    Util.show16losses = checkboxtoHide16losses.Checked;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);
                }
                ddlGridPages.SelectedIndex = 0;
                AppSettingData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        #region -----Font Size------------
        private void BindFontSize()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "8",
                Value = "8"
            });
            items.Add(new SelectListItem
            {
                Text = "9",
                Value = "9"
            });
            items.Add(new SelectListItem
            {
                Text = "10",
                Value = "10"
            });
            items.Add(new SelectListItem
            {
                Text = "11",
                Value = "11"
            });
            items.Add(new SelectListItem
            {
                Text = "12",
                Value = "12"
            });
            items.Add(new SelectListItem
            {
                Text = "13",
                Value = "13"
            });
            items.Add(new SelectListItem
            {
                Text = "14",
                Value = "14"
            });
            items.Add(new SelectListItem
            {
                Text = "15",
                Value = "15"
            });
            items.Add(new SelectListItem
            {
                Text = "16",
                Value = "16"
            });
            items.Add(new SelectListItem
            {
                Text = "17",
                Value = "17"
            });
            items.Add(new SelectListItem
            {
                Text = "18",
                Value = "18"
            });
            items.Add(new SelectListItem
            {
                Text = "19",
                Value = "19"
            });
            items.Add(new SelectListItem
            {
                Text = "20",
                Value = "20"
            });


            ddlFontSize.DataSource = items;
            ddlFontSize.DataValueField = "Value";
            ddlFontSize.DataTextField = "Text";
            ddlFontSize.DataBind();
            ddlFontSize.Items.Insert(0, new ListItem
            {
                Text = "<-Select->",
                Value = "0"
            });
        }
        #endregion
        #region -----Font Size------------
        private void BindCockpitFontSize()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "10",
                Value = "10"
            });
            items.Add(new SelectListItem
            {
                Text = "11",
                Value = "11"
            });
            items.Add(new SelectListItem
            {
                Text = "12",
                Value = "12"
            });
            items.Add(new SelectListItem
            {
                Text = "13",
                Value = "13"
            });
            items.Add(new SelectListItem
            {
                Text = "14",
                Value = "14"
            });
            items.Add(new SelectListItem
            {
                Text = "15",
                Value = "15"
            });
            items.Add(new SelectListItem
            {
                Text = "16",
                Value = "16"
            });
            items.Add(new SelectListItem
            {
                Text = "17",
                Value = "17"
            });
            items.Add(new SelectListItem
            {
                Text = "18",
                Value = "18"
            });
            items.Add(new SelectListItem
            {
                Text = "19",
                Value = "19"
            });
            items.Add(new SelectListItem
            {
                Text = "20",
                Value = "20"
            });
            items.Add(new SelectListItem
            {
                Text = "21",
                Value = "21"
            });
            items.Add(new SelectListItem
            {
                Text = "22",
                Value = "22"
            });
            items.Add(new SelectListItem
            {
                Text = "23",
                Value = "23"
            });
            items.Add(new SelectListItem
            {
                Text = "24",
                Value = "24"
            });
            items.Add(new SelectListItem
            {
                Text = "25",
                Value = "25"
            });
            items.Add(new SelectListItem
            {
                Text = "26",
                Value = "26"
            });
            items.Add(new SelectListItem
            {
                Text = "27",
                Value = "27"
            });
            items.Add(new SelectListItem
            {
                Text = "28",
                Value = "28"
            });
            items.Add(new SelectListItem
            {
                Text = "29",
                Value = "29"
            });
            items.Add(new SelectListItem
            {
                Text = "30",
                Value = "30"
            });

            ddlCockpitFontSize.DataSource = items;
            ddlCockpitFontSize.DataValueField = "Value";
            ddlCockpitFontSize.DataTextField = "Text";
            ddlCockpitFontSize.DataBind();
            ddlCockpitFontSize.Items.Insert(0, new ListItem
            {
                Text = "<-Select->",
                Value = "0"
            });
        }
        #endregion
        #region -----Bind Font Family----------------
        private void BindFontFamily()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "Arial",
                Value = "Arial"
            });
            items.Add(new SelectListItem
            {
                Text = "Baskerville Old Face",
                Value = "Baskerville Old Face"
            });
            items.Add(new SelectListItem
            {
                Text = "Bodoni MT",
                Value = "Bodoni MT"
            });
            items.Add(new SelectListItem
            {
                Text = "Book Antiqua",
                Value = "Book Antiqua"
            });
            items.Add(new SelectListItem
            {
                Text = "Calibri",
                Value = "Calibri"
            });
            items.Add(new SelectListItem
            {
                Text = "Cambria",
                Value = "Cambria"
            });
            items.Add(new SelectListItem
            {
                Text = "Constantia",
                Value = "Constantia"
            });
            items.Add(new SelectListItem
            {
                Text = "Consolas",
                Value = "Consolas"
            });
            items.Add(new SelectListItem
            {
                Text = "Cursive",
                Value = "Cursive"
            });
            items.Add(new SelectListItem
            {
                Text = "Garamond",
                Value = "Garamond"
            });
            items.Add(new SelectListItem
            {
                Text = "Goudy Old Style",
                Value = "Goudy Old Style"
            });
            items.Add(new SelectListItem
            {
                Text = "High Tower Text",
                Value = "High Tower Text"
            });
            items.Add(new SelectListItem
            {
                Text = "Segoe UI",
                Value = "Segoe UI"
            });
            items.Add(new SelectListItem
            {
                Text = "Times New Roman",
                Value = "Times New Roman"
            });
            items.Add(new SelectListItem
            {
                Text = "Tw Cen MT",
                Value = "Tw Cen MT"
            });
            items.Add(new SelectListItem
            {
                Text = "Open Sans",
                Value = "Open Sans"
            });
            items.Add(new SelectListItem
            {
                Text = "Lato",
                Value = "Lato"
            });
            items.Add(new SelectListItem
            {
                Text = "Old Standard TT",
                Value = "Old Standard TT"
            });
            items.Add(new SelectListItem
            {
                Text = "Abril Fatface",
                Value = "Abril Fatface"
            });
            items.Add(new SelectListItem
            {
                Text = "PT Serif",
                Value = "PT Serif"
            });
            items.Add(new SelectListItem
            {
                Text = "Ubuntu",
                Value = "Ubuntu"
            });
            items.Add(new SelectListItem
            {
                Text = "Vollkorn",
                Value = "Vollkorn"
            });
            items.Add(new SelectListItem
            {
                Text = "PT Mono",
                Value = "PT Mono"
            });
            items.Add(new SelectListItem
            {
                Text = "Gravitas One",
                Value = "Gravitas One"
            });
            items.Add(new SelectListItem
            {
                Text = "Montserrat",
                Value = "Montserrat"
            });
            items.Add(new SelectListItem
            {
                Text = "Raleway",
                Value = "Raleway"
            });
            items.Add(new SelectListItem
            {
                Text = "Merriweather",
                Value = "Merriweather"
            });
            items.Add(new SelectListItem
            {
                Text = "Roboto",
                Value = "Roboto"
            });
            items.Add(new SelectListItem
            {
                Text = "Roboto Mono",
                Value = "Roboto Mono"
            });
            items.Add(new SelectListItem
            {
                Text = "Roboto Condensed",
                Value = "Roboto Condensed"
            });
            items.Add(new SelectListItem
            {
                Text = "Roboto Slab",
                Value = "Roboto Slab"
            });
            items.Add(new SelectListItem
            {
                Text = "Arvo",
                Value = "Arvo"
            });
            items.Add(new SelectListItem
            {
                Text = "Verdana",
                Value = "Verdana"
            });
            items = items.OrderBy(ss => ss.Text).ToList();
            ddlFontFamily.DataSource = items;
            ddlFontFamily.DataValueField = "Value";
            ddlFontFamily.DataTextField = "Text";
            ddlFontFamily.DataBind();
            ddlFontFamily.Items.Insert(0, new ListItem
            {
                Text = "<-Select->",
                Value = "0"
            });
        }
        public class SelectListItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }
        #endregion
        #region -----Font Style------------
        private void BindFontStyle()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "Regular",
                Value = "Regular"
            });
            items.Add(new SelectListItem
            {
                Text = "Bold",
                Value = "Bold"
            });
            items.Add(new SelectListItem
            {
                Text = "Italic",
                Value = "Italic"
            });
            ddlFontStyle.DataSource = items;
            ddlFontStyle.DataValueField = "Value";
            ddlFontStyle.DataTextField = "Text";
            ddlFontStyle.DataBind();
            ddlFontStyle.Items.Insert(0, new ListItem
            {
                Text = "<-Select->",
                Value = "0"
            });
        }
        #endregion
        #region -----Bind Smiley Image Size------
        private void BindSmileyImageSize()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "50",
                Value = "50"
            });
            items.Add(new SelectListItem
            {
                Text = "55",
                Value = "55"
            });
            items.Add(new SelectListItem
            {
                Text = "60",
                Value = "60"
            });
            items.Add(new SelectListItem
            {
                Text = "65",
                Value = "65"
            });
            items.Add(new SelectListItem
            {
                Text = "70",
                Value = "70"
            });
            items.Add(new SelectListItem
            {
                Text = "75",
                Value = "75"
            });
            items.Add(new SelectListItem
            {
                Text = "80",
                Value = "80"
            });
            items.Add(new SelectListItem
            {
                Text = "85",
                Value = "85"
            });
            items.Add(new SelectListItem
            {
                Text = "90",
                Value = "90"
            });
            items.Add(new SelectListItem
            {
                Text = "95",
                Value = "95"
            });
            items.Add(new SelectListItem
            {
                Text = "100",
                Value = "100"
            });

            ddlSmileImageSize.DataSource = items;
            ddlSmileImageSize.DataValueField = "Value";
            ddlSmileImageSize.DataTextField = "Text";
            ddlSmileImageSize.DataBind();
            ddlSmileImageSize.Items.Insert(0, new ListItem
            {
                Text = "<-Select->",
                Value = "0"
            });
        }
        #endregion

        private void BindTPMTrakDown()
        {
            checkboxtoHide16losses.Checked = Util.show16losses;
        }
        #endregion

        #region  -- Machine sort order ---
        private void BindgvMachineSortOrder()
        {
            try
            {
                divMachineSortOrder.Visible = true;
                dtMachineSortOrder = DataBaseAccess.GetMachineSortOrderData();
                if (dtMachineSortOrder != null && dtMachineSortOrder.Rows.Count > 0)
                {
                    gvMachineSortOrder.DataSource = dtMachineSortOrder;
                    gvMachineSortOrder.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);

            }
        }

        private void MachineSortsave_Click()
        {
            bool saved = false;
            try
            {

                foreach (GridViewRow item in gvMachineSortOrder.Rows)
                {
                    saved = DataBaseAccess.SaveMachineSortOrder((item.FindControl("lblMachine") as Label).Text, (item.FindControl("txtSortOrder") as TextBox).Text);
                }
                if (saved)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk();", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);

                }
                BindgvMachineSortOrder();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

        }

        #endregion

        #region --Dashboard Default view---
        private void setDashboarddefaulttpevalues()
        {
            try
            {
                divDashboardDefaultView.Visible = true;
                string getyeartype = DataBaseAccess.GetType("DashboadYearType");
                ddlYear.SelectedValue = getyeartype;
                getyeartype = DataBaseAccess.GetType("DashboadMonthType");
                ddlMonth.SelectedValue = getyeartype;
                getyeartype = DataBaseAccess.GetType("DashboadDateType");
                ddlDate.SelectedValue = getyeartype;
              
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void btnDashboardDefaultsave_Click()
        {
            try
            {
                if (ddlYear.SelectedValue != null && ddlMonth.SelectedValue != null && ddlDate.SelectedValue != null)
                {
                    DataBaseAccess.SaveDashboarddetails(ddlYear.SelectedValue.ToString(), "DashboadYearType");
                    DataBaseAccess.SaveDashboarddetails(ddlMonth.SelectedValue.ToString(), "DashboadMonthType");
                    DataBaseAccess.SaveDashboarddetails(ddlDate.SelectedValue.ToString(), "DashboadDateType");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        #endregion

        #region  --- Hist and Live analytics cockpit default view---
        private void BindLiveAHistoricalData()
        {
            try
            {
                List<CockpitViewSettingClass> list = DataBaseAccess.getHistoricalLiveDefaultViewData();
                if (list.Count > 0)
                {
                    ddlHAIonicView.SelectedValue = list.Where(l => l.Parameter == "HistoricalDefaultView" && l.ValueInText == "Ionic").Select(l => l.ValueInText2).ToList()[0];
                    ddlHATableView.SelectedValue = list.Where(l => l.Parameter == "HistoricalDefaultView" && l.ValueInText == "Table").Select(l => l.ValueInText2).ToList()[0];
                    ddlLAIonicView.SelectedValue = list.Where(l => l.Parameter == "LiveDefaultView" && l.ValueInText == "Ionic").Select(l => l.ValueInText2).ToList()[0];
                    ddlLATableView.SelectedValue = list.Where(l => l.Parameter == "LiveDefaultView" && l.ValueInText == "Table").Select(l => l.ValueInText2).ToList()[0];
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void btnHAViewSave_Click()
        {
            try
            {
                DataBaseAccess.SaveHistoricalLiveDefaultViewdata("HistoricalDefaultView", "Ionic", ddlHAIonicView.SelectedValue);
                DataBaseAccess.SaveHistoricalLiveDefaultViewdata("HistoricalDefaultView", "Table", ddlHATableView.SelectedValue);
                BindLiveAHistoricalData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void btnLAViewSave_Click()
        {
            try
            {
                DataBaseAccess.SaveHistoricalLiveDefaultViewdata("LiveDefaultView", "Ionic", ddlLAIonicView.SelectedValue);
                DataBaseAccess.SaveHistoricalLiveDefaultViewdata("LiveDefaultView", "Table", ddlLATableView.SelectedValue);
                BindLiveAHistoricalData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        #endregion

        #region  ---Multilingual setting ---
        private void BindResourceFile()
        {
            MultilingualSetting.Visible = true;
            ddlPageName.Items.Clear();
            string resourcespath = string.Empty;
            List<string> data = new List<string>();
            //data.Add(Request.PhysicalApplicationPath + "App_GlobalResources");
            data.Add(Request.PhysicalApplicationPath + "App_LocalResources");

            foreach (string item in data)
            {
                resourcespath = item;
                string[] result;
                DirectoryInfo dirInfo = new DirectoryInfo(resourcespath);
                foreach (FileInfo filInfo in dirInfo.GetFiles())
                {
                    if (filInfo.Extension == ".resx")
                    {
                        string filename = filInfo.Name;
                        result = filename.Split(new string[] { "." }, StringSplitOptions.None);
                        if (ddlmultlang.SelectedValue.ToString() == "zh")
                        {
                            if (result.Contains("zh"))
                            {
                                //if (result[2].Equals(ddlLanguage.SelectedValue.ToString(), StringComparison.OrdinalIgnoreCase))
                                ddlPageName.Items.Add(filename);
                            }
                        }
                        if (ddlmultlang.SelectedValue.ToString() == "en")
                        {
                            if (!result.Contains("zh"))
                            {
                                ddlPageName.Items.Add(filename);
                            }
                        }
                    }
                }
            }

            ddlPageName.Items.Insert(0, GetGlobalResourceObject("CommanResource", "ALL").ToString());
        }

        private void btnMultilingualSave_Click()
        {
            Hashtable data = new Hashtable();
            for (int i = 0; i < gridViewResource.Rows.Count; i++)
            {
                Label lblKey = (Label)gridViewResource.Rows[i].FindControl("lblKey");
                TextBox txtValue = ((TextBox)gridViewResource.Rows[i].FindControl("txtValue"));
                HiddenField hdfCondition = ((HiddenField)gridViewResource.Rows[i].FindControl("hdfCondition"));
                if (hdfCondition.Value == "update")
                    data.Add(lblKey.Text, txtValue.Text);
            }
            string filename = ddlPageName.SelectedValue.ToString();
            string[] result = ddlPageName.SelectedValue.Split(new string[] { "." }, StringSplitOptions.None);
            filename = Request.PhysicalApplicationPath + "App_LocalResources\\" + filename;
            UpdateResourceFile(data, filename);
            // ShowData();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResourceMsgok", "ResourceMsgok();", true);
        }

        protected void ddlLanguage1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindResourceFile();
        }

        protected void ddlPageName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPageName.SelectedIndex != 0)
            {
                ShowData();
            }
        }

        private void ShowData()
        {
            string filename = string.Empty;
            string[] result = ddlPageName.SelectedValue.Split(new string[] { "." }, StringSplitOptions.None);
            //if (!result.Contains("CommanResource"))
            //{
            //    filename = Request.PhysicalApplicationPath +
            //     "App_LocalResources\\" + ddlPageName.SelectedValue.ToString();
            //}
            //else
            //{
            //    filename = Request.PhysicalApplicationPath +
            //     "App_GlobalResources\\" + ddlPageName.SelectedValue.ToString();
            //}
            filename = Request.PhysicalApplicationPath +
                "App_LocalResources\\" + ddlPageName.SelectedValue.ToString();
            Stream stream = new FileStream(filename, FileMode.Open,
                FileAccess.Read, FileShare.Read);
            ResXResourceReader RrX = new ResXResourceReader(stream);
            IDictionaryEnumerator RrEn = RrX.GetEnumerator();
            SortedList slist = new SortedList();
            while (RrEn.MoveNext())
            {
                slist.Add(RrEn.Key, RrEn.Value);
            }
            RrX.Close();
            stream.Dispose();
            gridViewResource.DataSource = slist;
            gridViewResource.DataBind();
        }

        public static void UpdateResourceFile(Hashtable data, String path)
        {
            Hashtable resourceEntries = new Hashtable();
            //Get existing resources //UpdatableResXResourceProvider
            ResXResourceReader reader = new ResXResourceReader(path);
            if (reader != null)
            {
                IDictionaryEnumerator id = reader.GetEnumerator();
                foreach (DictionaryEntry d in reader)
                {
                    if (d.Value == null)
                        resourceEntries.Add(d.Key.ToString(), "");
                    else
                        resourceEntries.Add(d.Key.ToString(), d.Value.ToString());
                }
                reader.Close();
            }
            //Modify resources here...
            foreach (String key in data.Keys)
            {
                if (!resourceEntries.ContainsKey(key))
                {
                    String value = data[key].ToString();
                    if (value == null)
                        value = "";
                    resourceEntries.Add(key, value);
                }
                else
                {
                    String value = data[key].ToString();
                    if (value == null)
                        value = "";
                    resourceEntries.Remove(key);
                    resourceEntries.Add(key, data[key].ToString());
                }
            }
            //Write the combined resource file
            ResXResourceWriter resourceWriter = new ResXResourceWriter(path);
            foreach (String key in resourceEntries.Keys)
            {
                resourceWriter.AddResource(key, resourceEntries[key]);
            }
            resourceWriter.Generate();
            resourceWriter.Close();
        }
        #endregion


        #region -----Eshopx setting---
        private void BindEshopxSetting()
        {
            divEshopxSetting.Visible = true;
            string Path = DBAccess.GetRootPath("RootPath");
            txtPathVal.Text = Path;
        }
        private void btnEshopxSave_Click()
        {
            try
            {
                int success = 0;
                success = DBAccess.SaveRootFolderDetails(txtPathVal.Text, "RootPath");
                if (success > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk();", true); ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);
                }
                BindEshopxSetting();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        #endregion

        #region  --Cockpit Back color---
        private void BindCockpitBackColorSetting()
        {
            try
            {
                divCockpitBackColor.Visible = true;
                chkAE.Checked = false; chkPE.Checked = false; chkOEE.Checked = false; ChkOperatorPE.Checked = false;
                string BackColorParam = CockpitDataBaseAccess.GetParamForCockpitBackColor();
                switch (BackColorParam)
                {
                    case "AE":
                        chkAE.Checked = true;
                        break;
                    case "PE":
                        chkPE.Checked = true;
                        break;
                    case "OEE":
                        chkOEE.Checked = true;
                        break;
                    case "OperatorPE":
                        ChkOperatorPE.Checked = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void SaveCockpitBackColorSetting()
        {
            try
            {
                int res = 0;
                res = CockpitDataBaseAccess.SaveCockpitBackColor(chkAE.Checked ? chkAE.Text : chkPE.Checked ? chkPE.Text : chkOEE.Checked ? chkOEE.Text : ChkOperatorPE.Checked ? ChkOperatorPE.Text : "", "CockpitBackColorParam");
                if (res > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);
                }
                BindCockpitBackColorSetting();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        #endregion

        #region  -- DataCollection ---
        private void BindDataCollection()
        {
            try
            {
                divDataCollection.Visible = true;
                List<string> list = new List<string> { "Y", "N" };
                ddlSerialNo.DataSource = list;
                ddlSerialNo.DataBind();
                ddlHeatCode.DataSource = list;
                ddlHeatCode.DataBind();
                ddlWorkOrder.DataSource = list;
                ddlWorkOrder.DataBind();

                DataCollectionEntity DCval = CockpitDataBaseAccess.GetDataCollectionDetails();
                ddlSerialNo.SelectedValue = DCval.SerialNo;
                ddlHeatCode.SelectedValue = DCval.HeatCode;
                ddlWorkOrder.SelectedValue = DCval.WorkOrder;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void SaveDataCollection()
        {
            try
            {
                int res = 0;
                res = CockpitDataBaseAccess.SaveDataCollectionValue(ddlSerialNo.SelectedValue.ToString(), ddlWorkOrder.SelectedValue.ToString(), ddlHeatCode.SelectedValue.ToString());
                if (res > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);
                }
                BindDataCollection();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        #endregion

        #region "Uploade Image "
        protected void btnUploadeHide_Click(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.WebControls.Image Imag = (System.Web.UI.WebControls.Image)(this.Master.FindControl("Image1"));
                if (FileUpload1.HasFile)
                {
                    string strpath = System.IO.Path.GetExtension(FileUpload1.FileName);
                    strpath = strpath.ToLower().Trim();
                    if (strpath != ".jpg" && strpath != ".jpeg" && strpath != ".gif" && strpath != ".png")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);
                    }
                    else
                    {
                        string dirUrl = "CompanyLogo";
                        string dirPath = Server.MapPath(dirUrl);
                        if (!Directory.Exists(dirPath))
                        {
                            Directory.CreateDirectory(dirPath);
                        }
                        Array.ForEach(Directory.GetFiles(dirPath), File.Delete);

                        // save the file to the Specifyed folder  
                        string fileUrl = dirUrl + "/" + Path.GetFileName(FileUpload1.PostedFile.FileName);
                        FileUpload1.PostedFile.SaveAs(Server.MapPath(fileUrl));
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk();", true);

                        const string imagesPath = "~/CompanyLogo/";// "~/Image/Slideshow/";
                        var dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(imagesPath));

                        //filtering to jpgs, but ideally not required
                        List<string> fileNames = (from flInfo in dir.GetFiles() select flInfo.Name).ToList();
                        if (fileNames.Count > 0)
                        {
                            Imag.ImageUrl = imagesPath + fileNames[0];
                        }
                        else
                        {
                            Imag.ImageUrl = "Image/companyIcon.png";
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            Session["GeneralSettings"] = null;
        }
        #endregion
        #region--------Modified Data Settings----
        private void BindModifiedDataSettings()
        {
            try
            {
                divModifiedDataSettings.Visible = true;
                List<string> list = new List<string> { "Y", "N" };
                //ddlisModified.DataSource = list;
                //ddlisModified.DataBind();
                ddlProductionLog.DataSource = list;
                ddlProductionLog.DataBind();
                ddlDownLog.DataSource = list;
                ddlDownLog.DataBind();
                //ddlIsModified.DataSource = list;
                //ddlIsModified.DataBind();
                DataTable dt = CockpitDataBaseAccess.GetModifiedSettingsData();
                if (dt.Rows.Count > 0)
                {
                    ddlProductionLog.SelectedValue = dt.AsEnumerable().Where(x => x.Field<string>("Parameter").ToString().Equals("EnableProductionLog", StringComparison.OrdinalIgnoreCase)).Select(x => x.Field<string>("ValueInText").ToString()).FirstOrDefault();
                    ddlDownLog.SelectedValue = dt.AsEnumerable().Where(x => x.Field<string>("Parameter").ToString().Equals("EnableDownLog", StringComparison.OrdinalIgnoreCase)).Select(x => x.Field<string>("ValueInText").ToString()).FirstOrDefault();
                    ddlChartType.SelectedValue = dt.AsEnumerable().Where(x => x.Field<string>("Parameter").ToString().Equals("HistoricalDashboardChartType", StringComparison.OrdinalIgnoreCase)).Select(x => x.Field<string>("ValueInText").ToString()).FirstOrDefault();
                    //ddlIsModified.SelectedValue = dt.AsEnumerable().Where(x => x.Field<string>("Parameter").ToString().Equals("IsModifiedData", StringComparison.OrdinalIgnoreCase)).Select(x => x.Field<string>("ValueInText").ToString()).FirstOrDefault();
                    txtColorPicker.Text = dt.AsEnumerable().Where(x => x.Field<string>("Parameter").ToString().Equals("VDGModifiedDataBackColor", StringComparison.OrdinalIgnoreCase)).Select(x => x.Field<string>("ValueInText").ToString()).FirstOrDefault();
                    var charts = (dt.AsEnumerable().Where(x => x.Field<string>("Parameter").ToString().Equals("HistoricalDashboardCombinedCharts", StringComparison.OrdinalIgnoreCase)).Select(x => x.Field<string>("ValueInText").ToString()).FirstOrDefault()).Split(',');
                    foreach(var d in charts)
                    {
                        ListItem item = lbCharts.Items.FindByValue(d);
                        if (item != null)
                        {
                            item.Selected = true;
                        }
                        //lbCharts.SelectedValue = d;
                    }
                    if (ddlChartType.SelectedValue.ToString() == "Combined")
                    {
                        lbCharts.Enabled = true;
                    }
                    else
                    {
                        lbCharts.Enabled = false;
                    }
                }
                
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void SaveModifiedDataSettings()
        {
            try
            {
                int resProductionLog = CockpitDataBaseAccess.SavemodifiedDataSettings("EnableProductionLog", ddlProductionLog.SelectedValue.ToString());
                int resDownLog = CockpitDataBaseAccess.SavemodifiedDataSettings("EnableDownLog", ddlDownLog.SelectedValue.ToString());
                int resChartType = CockpitDataBaseAccess.SavemodifiedDataSettings("HistoricalDashboardChartType", ddlChartType.SelectedValue.ToString());
                int resBackColor = CockpitDataBaseAccess.SavemodifiedDataSettings("VDGModifiedDataBackColor",txtColorPicker.Text);
                //int isModified = CockpitDataBaseAccess.SavemodifiedDataSettings("IsModifiedData", ddlIsModified.SelectedValue.ToString());
                var charts = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbCharts);
                int combinedCharts = CockpitDataBaseAccess.SavemodifiedDataSettings("HistoricalDashboardCombinedCharts", charts);
                if (resProductionLog>0&& resDownLog>0&& resChartType>0&& resBackColor > 0&& combinedCharts>0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);
                }
                BindModifiedDataSettings();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        #endregion

        protected void ddlChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlChartType.SelectedValue.ToString() == "Combined")
                {
                    lbCharts.Enabled = true;
                }
                else
                {
                    lbCharts.Enabled = false;
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}