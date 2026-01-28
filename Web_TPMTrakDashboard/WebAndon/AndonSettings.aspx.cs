using BusinessClassLibrary;
using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.WebAndon
{
    public partial class MachineSetting : System.Web.UI.Page
    {
        string prm = string.Empty, color = string.Empty;
        public SettingsGUI settings = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                settings = new SettingsGUI();
                if (Session["AdminData"] == null || !Request.IsAuthenticated || Session["connectionString"] == null)
                {
                    FormsAuthentication.RedirectToLoginPage();
                    Response.Redirect("../SignIn.aspx", false);
                }
                else
                {
                    if (!IsPostBack)
                    {
                        Button btnMode = (Button)Page.Master.FindControl("btnToggel");
                        btnMode.Visible = false;
                        if (Session["AdminData"] != null && Session["AdminData"].ToString().Equals("NonAdmin", StringComparison.OrdinalIgnoreCase))
                        {
                            txtAndonTitle.Enabled = false;
                            btnSaveColor.Enabled = false;
                            btnReset.Enabled = false;
                            hdfNonAdmin.Value = "NonAdmin";
                            tdGeneralHide.Visible = false;
                            tdAndonHide.Visible = false;
                            tdApplication.Attributes.Add("class", "col-lg-8");
                        }
                        PageLoadData();
                        lblMessages.Text = string.Empty;

                    }
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        #region -------Page Load Data------
        private void PageLoadData()
        {

            BindColorSetting();
            BindPlantDisplayData();
            BindFontFamily();
            BindFontStyle();
            BindDataRefreshInterval();
            ///--------Iconic Setting-------///
            BindIconiGridview();
            BindFontSizeIconicView();
            BindMachineFlip();
            BindSmileyImageSize();

            //---------Populate General Setting Data-------
            PopulateGeneralSettingData();

            //----------Iconic Populate Data--------\
            PopulateIconicSettingData();

        }
        #endregion

        #region -----Iconic Gridview Up and Down Button -------------
        protected void MoveIconicGridViewRows(object sender, EventArgs e)
        {
            try
            {
                Button btnUp = (Button)sender;
                GridViewRow row = (GridViewRow)btnUp.NamingContainer;
                // Get all items except the one selected  
                var rows = grdViewIconicView.Rows.Cast<GridViewRow>().Where(a => a != row).ToList();
                switch (btnUp.CommandName)
                {
                    case "Up":
                        //If First Item, insert at end (rotating positions)
                        if (row.RowIndex.Equals(0))
                            rows.Add(row);
                        else
                            rows.Insert(row.RowIndex - 1, row);
                        break;
                    case "Down":
                        //If Last Item, insert at beginning (rotating positions)
                        if (row.RowIndex.Equals(grdViewIconicView.Rows.Count - 1))
                            rows.Insert(0, row);
                        else
                            rows.Insert(row.RowIndex + 1, row);
                        break;
                }
                grdViewIconicView.DataSource = rows.Select(a => new
                {
                    ValueInText = ((Label)a.FindControl("lblValueInText")).Text,
                    Parameter = ((HiddenField)a.FindControl("hdfParameter")).Value,
                    ValueInText2 = ((TextBox)a.FindControl("txtValueInText2")).Text,
                    ValueInInt = ((HiddenField)a.FindControl("hdfValueInInt")).Value,
                    ValueInBool = ((CheckBox)a.FindControl("chkSelect")).Checked,
                }).ToList();
                grdViewIconicView.DataBind();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region --------Save Setting Data-------------
        protected void btnSaveSetting_Click(object sender, EventArgs e)
        {
            try
            {
                string message = string.Empty, paramter = string.Empty;
                if (hdfParameter.Value == "GeneralSetting")
                {
                    if (AndonCockpitView.GetCompany == "1")
                        paramter = "TPMAnalyticsBWebAndonGobalSettings";
                    else
                        paramter = "TPMAnalyticsWebAndonGobalSettings";
                    AndonCockpitView.UpdateApplicationUISettings("", txtAndonTitle.Text, ddlPlantToDisplay.SelectedValue, ddlRefreshInterval.SelectedValue,
                        "", "", ddlFontFamily.SelectedValue, ddlFontStyle.SelectedValue, "", "", "", "", paramter, "",
                        out message, Session["UserName"] == null ? "none" : Session["UserName"].ToString(), "", ddlDefaultPredefinedTimePeriod.SelectedValue);
                    Session["GeneralSettings"] = null;
                }
                if (hdfParameter.Value == "IconicSetting")
                {
                    if (AndonCockpitView.GetCompany == "1")
                        paramter = "TPMAnalyticsBWebAndonIconicViewSettings";
                    else
                        paramter = "TPMAnalyticsWebAndonIconicViewSettings";
                    AndonCockpitView.UpdateApplicationUISettings(ddlFontSizeMachineBox.SelectedValue, "", "", "", ddlMachineFlip.SelectedValue,
                       chkShowSmileyImg.Checked == true ? "1" : "0", "", "", ddlSmileImageSize.SelectedValue, "", "", "", paramter,
                       chkIconicEnableImgVdo.Checked == true ? "1" : "0", out message, Session["UserName"] == null ? "none" : Session["UserName"].ToString(),
                        "0", "");
                    Session["IconicViewSettings"] = null;
                }
                if (hdfParameter.Value == "ColorSetting")
                {
                    AndonCockpitView.UpdateMachineColorSetting(txtGood.Text, txtModerate.Text, txtBad.Text, "", "", out message, Session["UserName"] == null ? "none" : Session["UserName"].ToString());
                    Session["ColorViewSettings"] = null;
                }
                if (message.Equals("Successfull"))
                {
                    prm = "Record Save Successfull !!";
                    color = "green";
                }
                else
                {
                    prm = "Record does not Save Successfull !!";
                    color = "red";
                }
                if (message.Equals("Successfull"))
                {
                    prm = "Record Save Successfull !!";
                    color = "green";
                }
                else
                {
                    prm = "Record does not Save Successfull !!";
                    color = "red";
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AfterPostback", "SaveRecordsFun('" + prm + "','" + color + "')", true);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "-------Populate General Setting Data----------"
        private void PopulateGeneralSettingData()
        {
            try
            {
                AppUISettings model = AndonCockpitView.ViewApplicationUISettings(Session["UserName"] == null ? "none" : Session["UserName"].ToString());
                if (model != null)
                {
                    if (!string.IsNullOrEmpty(model.AndonTitle))
                        txtAndonTitle.Text = model.AndonTitle;
                    if (!string.IsNullOrEmpty(model.PlantToDisplay))
                        ddlPlantToDisplay.SelectedValue = model.PlantToDisplay;
                    if (!string.IsNullOrEmpty(model.DataDisplayInterval))
                        ddlRefreshInterval.SelectedValue = model.DataDisplayInterval;
                    if (!string.IsNullOrEmpty(model.FontFamily))
                        ddlFontFamily.SelectedValue = model.FontFamily;
                    if (!string.IsNullOrEmpty(model.FontStyle))
                        ddlFontStyle.SelectedValue = model.FontStyle;
                    if (!string.IsNullOrEmpty(model.DefaultPredefinedTimePeriod))
                        ddlDefaultPredefinedTimePeriod.SelectedValue = model.DefaultPredefinedTimePeriod;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region --------Populate Iconic Setting Data-----
        private void PopulateIconicSettingData()
        {
            try
            {
                IconicUISetting model = AndonCockpitView.ViewIconicUISettings(Session["UserName"] == null ? "none" : Session["UserName"].ToString());
                if (model != null)
                {
                    if (!string.IsNullOrEmpty(model.ScreenFlipInterval))
                        ddlMachineFlip.SelectedValue = model.ScreenFlipInterval;
                    if (model.ShowSmileyBlock == 0)
                        chkShowSmileyImg.Checked = false;
                    else
                        chkShowSmileyImg.Checked = true;

                    if (!string.IsNullOrEmpty(model.ShowSmileyBlockSize))
                        ddlSmileImageSize.SelectedValue = model.ShowSmileyBlockSize;
                    if (!string.IsNullOrEmpty(model.FormFontSize))
                        ddlFontSizeMachineBox.SelectedValue = model.FormFontSize;
                    if (!string.IsNullOrEmpty(model.EnableImageVideo) && (model.EnableImageVideo == "1"))
                        chkIconicEnableImgVdo.Checked = true;
                    else
                        chkIconicEnableImgVdo.Checked = false;
                    //if (!string.IsNullOrEmpty(model.EnableDashBoard) && (model.EnableDashBoard == "1"))
                    //    chkEnableDashboard.Checked = true;
                    //else
                    //    chkEnableDashboard.Checked = false;

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "View Machine Color Setting"
        private void BindColorSetting()
        {
            try
            {
                ColorUISetting model = AndonCockpitView.ViewColorSettingData(Session["UserName"] == null ? "none" : Session["UserName"].ToString());
                if (model != null)
                {
                    if (!string.IsNullOrEmpty(model.GoodColor))
                        txtGood.Text = model.GoodColor.Substring(3);
                    if (!string.IsNullOrEmpty(model.ModerateColor))
                        txtModerate.Text = model.ModerateColor.Substring(3);
                    if (!string.IsNullOrEmpty(model.BadColor))
                        txtBad.Text = model.BadColor.Substring(3);
                    //if (!string.IsNullOrEmpty(model.CockPitLabelBackColor))
                    //    txtCockpit1.Text = model.CockPitLabelBackColor.Substring(3);
                    //if (!string.IsNullOrEmpty(model.CockpitLabelTextColor))
                    //    txtCockpit2.Text = model.CockpitLabelTextColor.Substring(3);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region -------Bind Plant To Display --------
        private void BindPlantDisplayData()
        {
            try
            {
                List<string> lstPlantData = BindCockpitView.ViewPlantToDisplay();
                ddlPlantToDisplay.DataSource = lstPlantData;
                ddlPlantToDisplay.DataBind();
                ddlPlantToDisplay.Items.Insert(0, new ListItem
                {
                    Text = "All",
                    Value = "All"
                });
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region------Bind Iconic View GridView----------
        private void BindIconiGridview()
        {
            try
            {
                grdViewIconicView.DataSource = AndonCockpitView.BindIconicCockpitSetting(Session["UserName"] == null ? "none" : Session["UserName"].ToString());
                grdViewIconicView.DataBind();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region------- Reset Data-------------
        protected void btnReset_Click(object sender, EventArgs e)
        {
            lblMessages.Text = string.Empty;
            BindIconiGridview();
            BindColorSetting();
            PopulateIconicSettingData();
        }
        #endregion


        #region -----Save Iconic Gridview Setting Data Color -----------------
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string message = string.Empty;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                int orderNo = 0;
                if (hdfParameter.Value == "IconicSetting")
                {
                    foreach (GridViewRow row in grdViewIconicView.Rows)
                    {
                        SqlCommand cmd = new SqlCommand(@"[s_GetWebAndonColumnSettings]", sqlConn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ValueInText", ((Label)row.FindControl("lblValueInText")).Text);
                        cmd.Parameters.AddWithValue("@ValueInText2", ((TextBox)row.FindControl("txtValueInText2")).Text);
                        cmd.Parameters.AddWithValue("@ValueInInt", orderNo++);
                        cmd.Parameters.AddWithValue("@ValueInBool", ((CheckBox)row.FindControl("chkSelect")).Checked == true ? 1 : 0);
                        if (AndonCockpitView.GetCompany == "1")
                            cmd.Parameters.AddWithValue("@param", "UpdateTPMAnalyticsIconicView");
                        else
                            cmd.Parameters.AddWithValue("@param", "UpdateTPMAnalyticsBIconicView");
                        cmd.Parameters.AddWithValue("@User", Session["UserName"] == null ? "none" : Session["UserName"].ToString());
                        cmd.ExecuteNonQuery();
                    }
                    Session["IconicViewSettings"] = null;
                    message = "Successfull";
                    BindIconiGridview();
                }
                if (message.Equals("Successfull"))
                {
                    prm = "Record Save Successfull !!";
                    color = "green";
                }
                else
                {
                    prm = "Record does not Save Successfull !!";
                    color = "red";
                }
                Session["GeneralSettings"] = null;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AfterPostback", "SaveRecordsFun('" + prm + "','" + color + "')", true);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        #endregion

        #region ------Bind Machine Flip Interval--------
        private void BindMachineFlip()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "10",
                Value = "10"
            });
            items.Add(new SelectListItem
            {
                Text = "20",
                Value = "20"
            });
            items.Add(new SelectListItem
            {
                Text = "30",
                Value = "30"
            });
            items.Add(new SelectListItem
            {
                Text = "40",
                Value = "40"
            });
            items.Add(new SelectListItem
            {
                Text = "50",
                Value = "50"
            });
            items.Add(new SelectListItem
            {
                Text = "60",
                Value = "60"
            });
            items.Add(new SelectListItem
            {
                Text = "70",
                Value = "70"
            });
            items.Add(new SelectListItem
            {
                Text = "80",
                Value = "80"
            });
            items.Add(new SelectListItem
            {
                Text = "90",
                Value = "90"
            });
            items.Add(new SelectListItem
            {
                Text = "100",
                Value = "100"
            });
            items.Add(new SelectListItem
            {
                Text = "110",
                Value = "110"
            });
            items.Add(new SelectListItem
            {
                Text = "120",
                Value = "120"
            });
            //------Iconic View Flip Interval---------
            ddlMachineFlip.DataSource = items;
            ddlMachineFlip.DataValueField = "Value";
            ddlMachineFlip.DataTextField = "Text";
            ddlMachineFlip.DataBind();
            ddlMachineFlip.Items.Insert(0, new ListItem
            {
                Text = "<-Select->",
                Value = "0"
            });
        }
        #endregion

        #region ------Bind Data Refresh Interval--------
        private void BindDataRefreshInterval()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "30",
                Value = "30"
            });
            items.Add(new SelectListItem
            {
                Text = "40",
                Value = "40"
            });
            items.Add(new SelectListItem
            {
                Text = "50",
                Value = "50"
            });
            items.Add(new SelectListItem
            {
                Text = "60",
                Value = "60"
            });
            items.Add(new SelectListItem
            {
                Text = "70",
                Value = "70"
            });
            items.Add(new SelectListItem
            {
                Text = "80",
                Value = "80"
            });
            items.Add(new SelectListItem
            {
                Text = "90",
                Value = "90"
            });
            items.Add(new SelectListItem
            {
                Text = "100",
                Value = "100"
            });
            items.Add(new SelectListItem
            {
                Text = "110",
                Value = "110"
            });
            items.Add(new SelectListItem
            {
                Text = "120",
                Value = "120"
            });
            items.Add(new SelectListItem
            {
                Text = "130",
                Value = "130"
            });
            items.Add(new SelectListItem
            {
                Text = "140",
                Value = "140"
            });
            items.Add(new SelectListItem
            {
                Text = "150",
                Value = "150"
            });
            items.Add(new SelectListItem
            {
                Text = "160",
                Value = "160"
            });
            items.Add(new SelectListItem
            {
                Text = "170",
                Value = "170"
            });
            items.Add(new SelectListItem
            {
                Text = "180",
                Value = "180"
            });

            ddlRefreshInterval.DataSource = items;
            ddlRefreshInterval.DataValueField = "Value";
            ddlRefreshInterval.DataTextField = "Text";
            ddlRefreshInterval.DataBind();
            ddlRefreshInterval.Items.Insert(0, new ListItem
            {
                Text = "<-Select->",
                Value = "0"
            });
        }
        #endregion

        #region -----------Font Size Machine Box-------------
        private void BindFontSizeIconicView()
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
            items.Add(new SelectListItem
            {
                Text = "31",
                Value = "31"
            });
            items.Add(new SelectListItem
            {
                Text = "32",
                Value = "32"
            });
            items.Add(new SelectListItem
            {
                Text = "33",
                Value = "33"
            });
            items.Add(new SelectListItem
            {
                Text = "34",
                Value = "34"
            });
            items.Add(new SelectListItem
            {
                Text = "35",
                Value = "35"
            });
            items.Add(new SelectListItem
            {
                Text = "36",
                Value = "36"
            });
            items.Add(new SelectListItem
            {
                Text = "37",
                Value = "37"
            });
            items.Add(new SelectListItem
            {
                Text = "38",
                Value = "38"
            });
            items.Add(new SelectListItem
            {
                Text = "39",
                Value = "39"
            });
            items.Add(new SelectListItem
            {
                Text = "40",
                Value = "40"
            });

            //--------Font Size Iconic View---
            ddlFontSizeMachineBox.DataSource = items;
            ddlFontSizeMachineBox.DataValueField = "Value";
            ddlFontSizeMachineBox.DataTextField = "Text";
            ddlFontSizeMachineBox.DataBind();
            ddlFontSizeMachineBox.Items.Insert(0, new ListItem
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
            items.Add(new SelectListItem
            {
                Text = "105",
                Value = "105"
            });
            items.Add(new SelectListItem
            {
                Text = "110",
                Value = "110"
            });
            items.Add(new SelectListItem
            {
                Text = "115",
                Value = "115"
            });
            items.Add(new SelectListItem
            {
                Text = "120",
                Value = "120"
            });
            items.Add(new SelectListItem
            {
                Text = "125",
                Value = "125"
            });
            items.Add(new SelectListItem
            {
                Text = "130",
                Value = "130"
            });
            items.Add(new SelectListItem
            {
                Text = "135",
                Value = "135"
            });
            items.Add(new SelectListItem
            {
                Text = "140",
                Value = "140"
            });
            items.Add(new SelectListItem
            {
                Text = "145",
                Value = "145"
            });
            items.Add(new SelectListItem
            {
                Text = "150",
                Value = "150"
            });
            items.Add(new SelectListItem
            {
                Text = "155",
                Value = "155"
            });
            items.Add(new SelectListItem
            {
                Text = "160",
                Value = "160"
            });
            items.Add(new SelectListItem
            {
                Text = "165",
                Value = "165"
            });
            items.Add(new SelectListItem
            {
                Text = "170",
                Value = "170"
            });
            items.Add(new SelectListItem
            {
                Text = "175",
                Value = "175"
            });
            items.Add(new SelectListItem
            {
                Text = "180",
                Value = "180"
            });
            items.Add(new SelectListItem
            {
                Text = "185",
                Value = "185"
            });
            items.Add(new SelectListItem
            {
                Text = "190",
                Value = "190"
            });
            items.Add(new SelectListItem
            {
                Text = "195",
                Value = "195"
            });
            items.Add(new SelectListItem
            {
                Text = "200",
                Value = "200"
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

        #region "Uploade Image "
        protected void btnUploadeHide_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUpload1.HasFile)
                {
                    string strpath = System.IO.Path.GetExtension(FileUpload1.FileName);
                    strpath = strpath.ToLower().Trim();
                    if (strpath != ".jpg" && strpath != ".jpeg" && strpath != ".gif" && strpath != ".png")
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = "Only image formats (jpg, png, gif) are accepted ";
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
                        lblMessages.Text = "Uploaded Successfully";
                    }
                }
                else
                {
                    lblMessages.ForeColor = Color.Red;
                    lblMessages.Text = "Plz Uploading File";
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
    }
}
public class SelectListItem
{
    public string Text { get; set; }
    public string Value { get; set; }
}