using Elmah;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class SettingPage : System.Web.UI.Page
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
                BindGridview();
                PageLoadData();
                AppSettingData();
                setdown();
                BindStatusColorCodesGrid();
                BindLiveAHistoricalData();
            }
        }

        private void setdown()
        {
            checkboxtoHide16losses.Checked = Util.show16losses;
        }
        #region "-------Populate General Setting Data----------"
        private void AppSettingData()
        {
            try
            {
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
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        private void BindGridview()
        {
            try
            {
                grdViewAndonView.Columns[2].Visible = false;
                grdViewAndonView.DataSource = DataBaseAccess.BindSettingPage("WebTPMTrak", Session["Language"] == null ? "en" : Session["Language"].ToString());
                grdViewAndonView.DataBind();
                //BindgvMachineSortOrder();

            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void BindgvMachineSortOrder()
        {
            try
            {
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
                throw;
            }
        }

        private void PageLoadData()
        {

            // BindColorSetting();
            //BindPlantDisplayData();
            BindFontFamily();
            BindFontStyle();
            BindFontSize();
            setDashboarddefaulttpevalues();
            //BindComponentInfoDetails();
            BindCockpitFontSize();
            grdViewAndonView.Visible = true;
            ICockpitStyle values = CockpitDataBaseAccess.GetCockpitBackColorValues();
            SetColorValues(values);
            BindSmileyImageSize();
            BindAndonData();
            BindColumnViewDropdown();
        }

        private void BindColumnViewDropdown()
        {
            try
            {
                if (ConfigurationManager.AppSettings["ShowProductionandDownDataInVDG"].ToString() == "1")
                    ddlGridPages.Items.Insert(5,new ListItem { Text = "Cycle Analytics-Production and Down View", Value = "WebTPMTrakVDGProductionandDown" });
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void BindAndonData()
        {
            try
            {
              
                txtAndonTitle.Text = DataBaseAccess.GetAndonSettingsData("Andon", "AndonTitle");
                ddlAndonType.SelectedValue = DataBaseAccess.GetAndonSettingsData("Andon", "AndonType");
                ddlAndonDataRefreshInterval.SelectedValue = DataBaseAccess.GetAndonSettingsData("Andon", "AndonRefreshInterval");
                ddlAndonFlipInterval.SelectedValue = DataBaseAccess.GetAndonSettingsData("Andon", "AndonFlipInterval");
                ddlAndonFlipInterval.SelectedValue = DataBaseAccess.GetAndonSettingsData("Andon", "AndonPageSizeTableView");

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void setDashboarddefaulttpevalues()
        {
            try
            {
                string getyeartype = DataBaseAccess.GetType("DashboadYearType");
                ddlYear.SelectedValue = getyeartype;
                getyeartype = DataBaseAccess.GetType("DashboadMonthType");
                ddlMonth.SelectedValue = getyeartype;
                getyeartype = DataBaseAccess.GetType("DashboadDateType");
                ddlDate.SelectedValue = getyeartype;
                //getyeartype = DataBaseAccess.GetType("LandingPage");
                //if (ddlLandingPage.Items.FindByValue(getyeartype) != null)
                //{
                //    ddlLandingPage.SelectedValue = getyeartype;
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnSaveColumnSetting_Click(object sender, EventArgs e)
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
                    //BindIconiGridview();
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                    lblMessages.Text = GetGlobalResourceObject("CommanResource", "RecordUpdateSuccessfully").ToString();
                    Session["Language"] = ddlLanguage.SelectedValue.ToString();
                }
                else
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = GetGlobalResourceObject("CommanResource", "RecorddoesnotbeUpdate").ToString();
                }
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnSaveGeneralPageSetting_Click(object sender, EventArgs e)
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
                if (result > 0)
                {
                    //BindIconiGridview();
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                    lblMessages.Text = GetGlobalResourceObject("CommanResource", "RecordUpdateSuccessfully").ToString();
                }
                else
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = GetGlobalResourceObject("CommanResource", "RecorddoesnotbeUpdate").ToString();
                }
                ddlGridPages.SelectedIndex = 0;
                BindGridview();
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }


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

        protected void ddlGridPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Selected = ddlGridPages.SelectedValue;
            if (ddlGridPages.SelectedItem.Text == "Iconic View" || ddlGridPages.SelectedItem.Text == "Table View" || ddlGridPages.SelectedValue.ToString() == "WebCockpitGridColumnAggregate")
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
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = GetLocalResourceObject("OnlyImageFormats(jpgpnggif)areAccepted").ToString();
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
                        lblMessages.ForeColor = System.Drawing.Color.Green;
                        lblMessages.Text = GetLocalResourceObject("UploadedSuccessfully").ToString();

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
                    lblMessages.ForeColor = Color.Red;
                    lblMessages.Text = GetLocalResourceObject("PlzUploadingFile").ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
            Session["GeneralSettings"] = null;
        }
        #endregion

        protected void btnSaveColor_Click(object sender, EventArgs e)
        {
            int result = 0;
            try
            {
                result = CockpitDataBaseAccess.UpdateColorSettings("CockpitBackColor", "GoodColor", "#FF" + txtGood.Text.ToUpper());
                result = CockpitDataBaseAccess.UpdateColorSettings("CockpitBackColor", "ModerateColor", "#FF" + txtModerate.Text.ToUpper());
                result = CockpitDataBaseAccess.UpdateColorSettings("CockpitBackColor", "BadColor", "#FF" + txtBad.Text.ToUpper());
                //result = CockpitDataBaseAccess.UpdateColorSettings("CockpitBackColor", "CockPitLabelBackColor", txtLabelsColor.Value);
                //result = CockpitDataBaseAccess.UpdateColorSettings("CockpitBackColor", "CockpitLabelTextColor", txtLabelsText.Value);
                if (result > 0)
                {
                    //BindIconiGridview();
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                    lblMessages.Text = GetGlobalResourceObject("CommanResource", "RecordUpdateSuccessfully").ToString();
                }
                else
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = GetGlobalResourceObject("CommanResource", "RecorddoesnotbeUpdate").ToString();
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

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


        private void SetColorValues(ICockpitStyle values)
        {
            try
            {
                txtGood.Text = values.GoodRunning.Substring(3);
                txtModerate.Text = values.ModeratelyRunning.Substring(3);
                txtBad.Text = values.BadlyRunning.Substring(3);
                //txtLabelsColor.Value = values.CockpitLabelBackColor;
                //txtLabelsText.Value = values.CockpitLabelTextColor;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        protected void checkboxtoHide16losses_CheckedChanged(object sender, EventArgs e)
        {
            if (checkboxtoHide16losses != null)
            {
                DataBaseAccess.Check16losses(checkboxtoHide16losses.Checked);
                Util.show16losses = checkboxtoHide16losses.Checked;
            }
        }
        protected void btnMachineSortOrder_Click(object sender, EventArgs e)
        {
            //gvMachineSortOrder.Visible = true;
            //ClientScript.RegisterStartupScript(this.GetType(), "pop", "openModal();", true);
            //ScriptManager.RegisterStartupScript(this, GetType(), "popGridView", "openModal();", true);
            BindgvMachineSortOrder();
        }

        protected void save_Click(object sender, EventArgs e)
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "alert('Saved!')", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "alert('Not Saved! Please Try Again.')", true);

                }

                BindgvMachineSortOrder();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

        }

        private void BindStatusColorCodesGrid()
        {
            try
            {
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

        protected void btnSaveColorCodes_Click(object sender, EventArgs e)
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
                    lblMessages.ForeColor = Color.Green;
                    lblMessages.Text = GetGlobalResourceObject("CommanResource", "RecordUpdateSuccessfully").ToString();
                }
                else
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = GetGlobalResourceObject("CommanResource", "RecorddoesnotbeUpdate").ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
        }

        protected void btncolorsave_Click(object sender, EventArgs e)
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

        protected void btnAndonSave_Click(object sender, EventArgs e)
        {
            try
            {
                string AndonType = ""; string RefreshInterval = "", AndonFlipInterval="",PageSize="";
                string AndonTitle = txtAndonTitle.Text;
                if (ddlAndonType.SelectedValue != null)
                    AndonType = ddlAndonType.SelectedValue.ToString();
                if (ddlAndonDataRefreshInterval.SelectedValue != null)
                    RefreshInterval = ddlAndonDataRefreshInterval.SelectedValue.ToString();
                if (ddlAndonFlipInterval.SelectedValue != null)
                    RefreshInterval = ddlAndonFlipInterval.SelectedValue.ToString();
                if (ddlAndonPageSize.SelectedValue != null)
                    PageSize = ddlAndonPageSize.SelectedValue.ToString();
                if (!(string.IsNullOrEmpty(AndonTitle) || string.IsNullOrEmpty(AndonType) || string.IsNullOrEmpty(RefreshInterval)))
                {
                    DataBaseAccess.saveAndonTitledetails("Andon", "AndonTitle", AndonTitle);
                    DataBaseAccess.saveAndonTitledetails("Andon","AndonType", AndonType);
                    DataBaseAccess.saveAndonTitledetails("Andon","AndonRefreshInterval", RefreshInterval);
                    DataBaseAccess.saveAndonTitledetails("Andon", "AndonFlipInterval", AndonFlipInterval); 
                        DataBaseAccess.saveAndonTitledetails("Andon", "AndonPageSizeTableView", PageSize);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
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

            }
        }
        protected void btnHAViewSave_Click(object sender, EventArgs e)
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

        protected void btnLAViewSave_Click(object sender, EventArgs e)
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
        //protected void btnLandingPageSave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataBaseAccess.SaveDashboarddetails(ddlLandingPage.SelectedValue, "LandingPage");
        //        setDashboarddefaulttpevalues();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.Message);
        //    }
        //}

        //protected void btnCompInfoSave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataBaseAccess.SaveComponentInfodetails(ddlCompInfoDataType.SelectedValue, "ComponentInfoInterfaceIdDataType");
        //        BindComponentInfoDetails();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.Message);
        //    }
        //}
        //private void BindComponentInfoDetails()
        //{
        //    try
        //    {
        //        string value = DataBaseAccess.GetType("ComponentInfoInterfaceIdDataType");
        //        if (ddlCompInfoDataType.Items.FindByValue(value) != null)
        //        {
        //            ddlCompInfoDataType.SelectedValue = value;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog("BindComponentInfoDetails: " + ex.Message);
        //    }
        //}
    }
}