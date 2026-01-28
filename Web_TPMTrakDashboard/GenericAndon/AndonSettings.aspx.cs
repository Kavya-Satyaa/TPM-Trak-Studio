using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GenericAndon.Model;
using System.Data;

namespace Web_TPMTrakDashboard.Andon_settings
{
    public partial class AndonSettings : System.Web.UI.Page
    {
        string Bad = null;
        string Good = null;
        string Moderate = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["connectionString"] == null)
                Response.Redirect("../SignIn.aspx", false);
            else
            {
                if (!IsPostBack)
                {
                    setCompanyLogo();
                    BindSettings();
                    //BindPlant();
                    BindCellID();
                    BinddataGrid();
                    BindFrequency();

                    if (HttpContext.Current.Request.Cookies["ComputerName"] != null)
                    {
                        lblComputerName.Text = HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString();
                        string RunOption = AndonDBAccess.GetRunOption(lblComputerName.Text, "ComputerRunOption");
                        ddlRunOptions.SelectedValue = RunOption == "" ? "RunByCell" : RunOption;

                        string Freq = AndonDBAccess.GetFrequency(lblComputerName.Text, "AndonFrequency", "CockpitAndonFrequency");
                        if (!string.IsNullOrEmpty(Freq))
                        {
                            if (ddlFrequency.Items.FindByValue(Freq) != null)
                                ddlFrequency.SelectedValue = Freq;
                        }

                        DataTable dt = AndonDBAccess.GetBoxDimensions(HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString());

                        chkUseCustomWidth.Checked = dt.AsEnumerable().Where(x => x["ValueInText"].ToString().Equals("UseCustomWidth", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInBool"].ToString()).FirstOrDefault() == "1" ? true : false;
                        txtBoxWidth.Text = dt.AsEnumerable().Where(x => x["ValueInText"].ToString().Equals("BoxWidth", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault();
                        txtLeftMargin.Text = dt.AsEnumerable().Where(x => x["ValueInText"].ToString().Equals("LeftMargin", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault();
                        txtTopMargin.Text = dt.AsEnumerable().Where(x => x["ValueInText"].ToString().Equals("TopMargin", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault();

                    }

                    if (WebConfigurationManager.AppSettings["KachMotors"].ToString() == "1")
                    {
                        trFrequency.Visible = false;
                        trEmoji.Visible = false;
                    }
                    else
                    {
                        trFrequency.Visible = true;
                        trEmoji.Visible = true;
                    }
                    EffyColorEntity values = AndonDBAccess.GetCockpitBackColorValues();
                    SetColorValues(values);
                }
            }

        }

        private void BindFrequency()
        {
            try
            {
                List<ListItem> list = new List<ListItem>();
                list.Add(new ListItem() { Value = "Last One", Text = "Latest One Hour" });
                list.Add(new ListItem() { Value = "Two Hours", Text = "Latest Two Hours" });
                list.Add(new ListItem() { Value = "Three Hours", Text = "Latest Three Hours" });
                list.Add(new ListItem() { Value = "Shift", Text = "Shift" });
                list.Add(new ListItem() { Value = "Day", Text = "Day" });
                ddlFrequency.DataSource = list;
                ddlFrequency.DataTextField = "Text";
                ddlFrequency.DataValueField = "Value";
                ddlFrequency.DataBind();
                if (Request.Cookies["AndonFrequency"] == null)
                {
                    Response.Cookies["AndonFrequency"].Value = ddlFrequency.SelectedValue;
                    Response.Cookies["AndonFrequency"].Expires = DateTime.MaxValue;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindFrequency_CockpitAndon: {ex}");
            }
        }
        private void setCompanyLogo()
        {

            try
            {
                string imagesPath = "~/CompanyLogo/";
                var dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(imagesPath));

                //filtering to jpgs, but ideally not required
                List<string> fileNames = (from flInfo in dir.GetFiles() select flInfo.Name).ToList();
                if (fileNames.Count > 0)
                {
                    companyLogo.ImageUrl = imagesPath + fileNames[0];
                }
                else
                {
                    companyLogo.ImageUrl = "Image/companyIcon.png";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("setCompanyLogo = " + ex.Message);
            }
        }
        private void SetColorValues(EffyColorEntity values)
        {
            try
            {
                txtGood.Text = values.GoodColor.Substring(3);
                txtModerate.Text = values.ModerateColor.Substring(3);
                txtBad.Text = values.BadColor.Substring(3);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindPlant()
        {
            try
            {
                List<string> plants = AndonDBAccess.getPlantID();
                //ddlPlant.DataSource = plants;
                //ddlPlant.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindCellID()
        {
            try
            {
                ddlCellID.DataSource = AndonDBAccess.getCellID("");
                ddlCellID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindSettings()
        {
            try
            {
                string computerName = string.Empty;
                if (HttpContext.Current.Request.Cookies["ComputerName"] != null)
                {
                    computerName = HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString().Trim();
                }
                SettingsViewEntity entity = AndonDBAccess.ViewSettings(computerName, "s_GetAndonUISettings", "AndonCockpitAppSettings");

                if (entity != null)
                {
                    txtAndonTitle.Text = entity.AndonTitle;
                    ddlFontName.SelectedValue = entity.FontFamily;
                    ddlFontStyle.SelectedValue = entity.FontStyle;
                    ddlDataRefreshInterval.SelectedValue = entity.DataDisplayInterval;
                    ddlScreenFlipInterval.SelectedValue = entity.ScreenFlipInterval;
                    if (entity.MsgBlockEnabled == 1)
                    {
                        chkMsgBlock.Checked = true;
                    }
                    if (entity.ShowFooterBlock == 1)
                    {
                        chkFooterBlock.Checked = true;
                    }
                    if (entity.ShowSmileyBlock == 1)
                    {
                        chkShowEmoji.Checked = true;
                    }
                    ddlEmojiSize.SelectedValue = entity.ShowSmileyBlockSize;
                    if (entity.EnableImage == 1)
                    {
                        chkImage.Checked = true;
                        txtPathImg.Enabled = true;
                    }
                    else
                    {
                        txtPathImg.Enabled = false;
                        chkImage.Checked = false;
                    }
                    if (entity.EnableVideo == 1)
                    {
                        chkVideo.Checked = true;
                        txtPathVideo.Enabled = true;
                    }
                    else
                    {
                        chkVideo.Checked = false;
                        txtPathVideo.Enabled = false;
                    }
                    ddlSlideShow.SelectedValue = entity.ImageFlipInterval;
                    ddlDate.SelectedValue = entity.DateFormat;
                    ddlTime.SelectedValue = entity.TimeFormat;
                    ddlViewType.SelectedValue = entity.PoojaViewType;
                    if (entity.ShowCurvedBoxes == 1)
                    {
                        chkcurvedboxes.Checked = true;
                    }
                    string[] orders = entity.OrderBy.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (orders.Length > 1)
                    {
                        ddlsortByName.SelectedValue = orders[0];
                        ddlSortOrder.SelectedValue = orders[1] == "asc" ? "Ascending" : "Descending";
                    }
                    //  ddlShowOnMonitor.SelectedValue = entity.PrimaryScreen;
                    txtScrollingText.Text = entity.ScrollingText;
                    txtPathImg.Text = entity.ImageFilePath;
                    txtPathVideo.Text = entity.VideoFilePath;

                    if (WebConfigurationManager.AppSettings["PoojaAndonMelting"].ToString() == "1")
                    {
                        trPoojaViewType.Visible = true;
                    }
                    else
                    {
                        trPoojaViewType.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BinddataGrid()
        {
            string computerName = "";
            List<AndonDefaultsEntity> entities = new List<AndonDefaultsEntity>();
            if (HttpContext.Current.Request.Cookies["ComputerName"] != null)
            {
                computerName = HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString();
            }
            try
            {
                if (WebConfigurationManager.AppSettings["KachMotors"].ToString() == "1")
                    entities = AndonDBAccess.GetAndonViewSettingsData(ddlCellID.SelectedValue.ToString(), computerName, "AndonParameter_KM");
                else
                    entities = AndonDBAccess.GetAndonViewSettingsData(ddlCellID.SelectedValue.ToString(), computerName, "CockpitAndonOrder");

                if (entities.Count > 0 && entities != null)
                {
                    lblProductionMachineFontsize.Text = entities.AsEnumerable().Where(x => x.ValueInText.Equals("ProductionMachineNameFontSize", StringComparison.OrdinalIgnoreCase)).Select(x => x.LabelFontSize).FirstOrDefault();
                    entities = entities.AsEnumerable().Where(x => x.ValueInText != "ProductionMachineNameFontSize").ToList();
                    gridviewSetting.DataSource = entities;
                    gridviewSetting.DataBind();
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
                string computerName = string.Empty;
                if (HttpContext.Current.Request.Cookies["ComputerName"] != null)
                {
                    computerName = HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString();
                }
                string Pathimg = string.Empty, PathVideo = string.Empty;
                int ChkMsg = 0, ShowSmiley = 0, ShowFooter = 0, ShowImage = 0, ShowVideo = 0, CurvedBox = 0;
                string sortType = ddlsortByName.SelectedValue.ToString();
                string sortOrder = ddlSortOrder.SelectedValue.ToString() == "Ascending" ? "asc" : "desc";
                if (chkMsgBlock.Checked == true)
                {
                    ChkMsg = 1;
                }

                if (chkShowEmoji.Checked == true)
                {
                    ShowSmiley = 1;
                }
                if (chkFooterBlock.Checked == true) ShowFooter = 1;
                if (chkImage.Checked == true) ShowImage = 1;
                if (chkVideo.Checked == true) ShowVideo = 1;
                if (chkcurvedboxes.Checked == true) CurvedBox = 1;
                string sortBy = string.Concat(sortType, " ", sortOrder);
                string isSuccessfull = string.Empty;

                if (chkImage.Checked)
                {
                    if (!System.IO.Directory.Exists(txtPathImg.Text))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Enter Correct Image Path')", true);
                        return;
                    }
                    else
                    {
                        Pathimg = txtPathImg.Text;
                    }
                }
                if (chkVideo.Checked)
                {
                    if (!System.IO.Directory.Exists(txtPathVideo.Text))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Enter Correct Video Path')", true);
                        return;
                    }
                    else
                    {
                        PathVideo = txtPathVideo.Text;
                    }
                }

                SettingsViewEntity entity = new SettingsViewEntity();
                entity.AndonTitle = txtAndonTitle.Text.Trim();
                entity.FontFamily = ddlFontName.SelectedValue;
                entity.FontStyle = ddlFontStyle.SelectedValue;
                //entity.PlantToDisplay = ddlPlant.SelectedValue;
                entity.DataDisplayInterval = ddlDataRefreshInterval.SelectedValue;
                entity.ScreenFlipInterval = ddlScreenFlipInterval.SelectedValue;
                entity.MsgBlockEnabled = ChkMsg;
                entity.ShowFooterBlock = ShowFooter;
                entity.ShowSmileyBlock = ShowSmiley;
                entity.ShowSmileyBlockSize = ddlEmojiSize.SelectedValue;
                entity.EnableImage = ShowImage;
                entity.EnableVideo = ShowVideo;
                entity.ImageFilePath = Pathimg;
                entity.VideoFilePath = PathVideo;
                entity.ImageFlipInterval = ddlSlideShow.SelectedValue;
                entity.DateFormat = ddlDate.SelectedValue;
                entity.TimeFormat = ddlTime.SelectedValue;
                entity.ShowCurvedBoxes = CurvedBox;
                entity.OrderBy = sortBy;
                entity.ScrollingText = txtScrollingText.Text.Trim();

                AndonDBAccess.UpdateAndonSettings(entity, "AndonCockpitAppSettings", computerName, out isSuccessfull);


                if (WebConfigurationManager.AppSettings["PoojaAndonMelting"].ToString() == "1")
                {
                    AndonDBAccess.insertUpdatePoojaAndonSetting("AndonCockpitAppSettings", "PoojaViewType", ddlViewType.SelectedValue);
                }

                if (isSuccessfull.Equals("Successfull"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Successfully Updated')", true);
                    return;
                }
                BindSettings();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnSavesetting_Click(object sender, EventArgs e)
        {
            try
            {
                string computerName = "";
                if (HttpContext.Current.Request.Cookies["ComputerName"] != null)
                {
                    computerName = HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString();
                }

                int index = 0;
                string Column = string.Empty, customColName = string.Empty, isSuccessfull = string.Empty, TextAlign = string.Empty;
                string Sortorder = string.Empty, LabelFontSize = string.Empty, DataFontSize = string.Empty, chkval = string.Empty;
                foreach (GridViewRow rows in gridviewSetting.Rows)
                {
                    if (rows.RowType == DataControlRowType.DataRow)
                    {
                        Column = (gridviewSetting.Rows[index].FindControl("lblColumn") as Label).Text.ToString();
                        customColName = (gridviewSetting.Rows[index].FindControl("lblCustomColumnName") as TextBox).Text.ToString();
                        Sortorder = (gridviewSetting.Rows[index].FindControl("lblSortOrder") as TextBox).Text.ToString();
                        chkval = (gridviewSetting.Rows[index].FindControl("chkVisibility") as CheckBox).Checked.ToString();
                        TextAlign = (gridviewSetting.Rows[index].FindControl("ddlTextAlign") as DropDownList).SelectedValue.ToString();
                        LabelFontSize = (gridviewSetting.Rows[index].FindControl("lblLabelFontSize") as TextBox).Text.ToString();
                        DataFontSize = (gridviewSetting.Rows[index].FindControl("lblDataFontSize") as TextBox).Text.ToString();

                        if (WebConfigurationManager.AppSettings["KachMotors"].ToString() == "1")
                            AndonDBAccess.UpdateGridSettingData("AndonParameter_KM", Column, customColName, Sortorder, chkval, TextAlign, LabelFontSize, DataFontSize, ddlCellID.SelectedValue.ToString(), computerName, out isSuccessfull);
                        else
                            AndonDBAccess.UpdateGridSettingData("CockpitAndonOrder", Column, customColName, Sortorder, chkval, TextAlign, LabelFontSize, DataFontSize, ddlCellID.SelectedValue.ToString(), computerName, out isSuccessfull);
                    }
                    index++;
                }
                if (WebConfigurationManager.AppSettings["KachMotors"].ToString() == "1")
                    AndonDBAccess.UpdateGridSettingData("AndonParameter_KM", "ProductionMachineNameFontSize", string.Empty, string.Empty, "true", string.Empty, lblProductionMachineFontsize.Text.ToString(), string.Empty, ddlCellID.SelectedValue.ToString(), computerName, out isSuccessfull);
                else
                    AndonDBAccess.UpdateGridSettingData("CockpitAndonOrder", "ProductionMachineNameFontSize", string.Empty, string.Empty, "true", string.Empty, lblProductionMachineFontsize.Text.ToString(), string.Empty, ddlCellID.SelectedValue.ToString(), computerName, out isSuccessfull);
                AndonDBAccess.UpdateGridSettingData("CockpitBackColor", "GoodColor", "#FF" + txtGood.Text.ToUpper(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, ddlCellID.SelectedValue.ToString(), "", out isSuccessfull);
                AndonDBAccess.UpdateGridSettingData("CockpitBackColor", "ModerateColor", "#FF" + txtModerate.Text.ToUpper(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, ddlCellID.SelectedValue.ToString(), "", out isSuccessfull);
                AndonDBAccess.UpdateGridSettingData("CockpitBackColor", "BadColor", "#FF" + txtBad.Text.ToUpper(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, ddlCellID.SelectedValue.ToString(), "", out isSuccessfull);
                if (isSuccessfull.Equals("Successfull"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Successfully Updated')", true);
                }

                BinddataGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void chkShowEmoji_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkShowEmoji.Checked == true)
                {
                    ddlEmojiSize.Enabled = true;
                }
                else
                {
                    ddlEmojiSize.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void chkImage_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkImage.Checked == true)
                {
                    txtPathImg.Enabled = true;
                    ddlSlideShow.Enabled = true;
                }
                else
                {
                    txtPathImg.Enabled = false;
                    ddlSlideShow.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void chkVideo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkVideo.Checked == true)
                {
                    txtPathVideo.Enabled = true;
                }
                else
                {
                    txtPathVideo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnAndon_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["AndonPage"].ToString().ToLower().Contains("productionandonnew"))
                    Response.Redirect("ProductionAndonNew.aspx");
                else
                    Response.Redirect("AndonPage.aspx");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"btnAndon_Click: {ex.Message}");
            }
        }

        protected void gridviewSetting_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string value = (e.Row.FindControl("hdnTextAlign") as HiddenField).Value;
                    (e.Row.FindControl("ddlTextAlign") as DropDownList).SelectedValue = value;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BinddataGrid();
        }

        protected void btnSaveComputerName_Click(object sender, EventArgs e)
        {
            bool success = true;
            try
            {
                success &= AndonDBAccess.SaveRunOption(lblComputerName.Text, ddlRunOptions.SelectedValue.ToString(), "ComputerRunOption");
                success &= AndonDBAccess.SaveBoxDimension(lblComputerName.Text, "", "UseCustomWidth", chkUseCustomWidth.Checked);
                success &= AndonDBAccess.SaveBoxDimension(lblComputerName.Text, txtBoxWidth.Text, "BoxWidth");
                success &= AndonDBAccess.SaveBoxDimension(lblComputerName.Text, txtLeftMargin.Text, "LeftMargin");
                success &= AndonDBAccess.SaveBoxDimension(lblComputerName.Text, txtTopMargin.Text, "TopMargin");
                success &= AndonDBAccess.SaveAndonFrquency(lblComputerName.Text, ddlFrequency.SelectedValue, "CockpitAndonFrequency");
                if (success)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Succesfully Updated')", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}