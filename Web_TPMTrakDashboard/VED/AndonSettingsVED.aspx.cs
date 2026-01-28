using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GenericAndon.Model;
using Web_TPMTrakDashboard.VED.Model;

namespace Web_TPMTrakDashboard.Andon
{
    public partial class AndonSettingsVED : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["connectionString"] == null)
                Response.Redirect("../SignIn.aspx", false);
            else
            {
                if (!IsPostBack)
                {
                    if (Request.Cookies["ComputerName"] != null)
                        txtDeviceName.Text = Request.Cookies["ComputerName"].Value.ToString();
                    setCompanyLogo();
                    BindFrequency();
                    BindSettings();
                    BindScreensData();
                    ddlSettingType_SelectedIndexChanged(null, null);


                    string RunOption = AndonDBAccess.GetRunOption(txtDeviceName.Text, "ComputerRunOption");
                    ddlRunOptions.SelectedValue = RunOption == "" ? "RunByCell" : RunOption;

                    string Freq = AndonDBAccess.GetFrequency(txtDeviceName.Text, "AndonFrequency", "CockpitAndonFrequency");
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

        private void BindSettings()
        {
            try
            {

                string Freq = AndonDBAccess.GetFrequency(txtDeviceName.Text, "AndonFrequency", "CockpitAndonFrequency");
                if (!string.IsNullOrEmpty(Freq))
                {
                    if (ddlFrequency.Items.FindByValue(Freq) != null)
                        ddlFrequency.SelectedValue = Freq;
                }
                AndonSettingEntityVED entity = DBAccessVED.ViewAndonSettings(txtDeviceName.Text, "AndonCockpitAppSettings");

                if (entity != null)
                {
                    ddlFontName.SelectedValue = entity.FontFamily;
                    ddlFontStyle.SelectedValue = entity.FontStyle;
                    ddlDataRefreshInterval.SelectedValue = entity.DataRefreshInterval.ToString();
                    ddlScreenFlipInterval.SelectedValue = entity.ScreenFlipInterval.ToString();
                    txtPathImg.Text = entity.ImagePath;
                    txtPathVideo.Text = entity.VideoPath;

                    if (entity.ImageEnabled == 1)
                    {
                        chkImage.Checked = true;
                        txtPathImg.Enabled = true;
                    }
                    else
                    {
                        txtPathImg.Enabled = false;
                        chkImage.Checked = false;
                    }
                    if (entity.VideoEnabled == 1)
                    {
                        chkVideo.Checked = true;
                        txtPathVideo.Enabled = true;
                    }
                    else
                    {
                        chkVideo.Checked = false;
                        txtPathVideo.Enabled = false;
                    }
                    ddlSlideShow.SelectedValue = entity.SlideshowInterval.ToString();

                    chkshowFooter.Checked = entity.FooterBlockEnabled == 1 ? true : false;
                    chkShowMessage.Checked = entity.MsgBlockEnabled == 1 ? true : false;
                    txtScrollingMessage.Text = entity.ScrollingMsg;

                    chkShowEmoji.Checked = entity.EmojiEnabled == 1 ? true : false;
                    if (ddlEmojiSize.Items.FindByValue(entity.EmojiSize.ToString()) != null)
                        ddlEmojiSize.SelectedValue = entity.EmojiSize.ToString();

                    chkShowCurvedBoxes.Checked = entity.ShowCurvedBoxes == 1 ? true : false;

                    ddlSortOrder.SelectedValue = entity.orderby;
                    ddlsortByName.SelectedValue = entity.Sortorder;

                    if (ddlDate.Items.FindByValue(entity.DateFormat) != null)
                        ddlDate.SelectedValue = entity.DateFormat;
                    if (ddlTime.Items.FindByValue(entity.TimeFormat) != null)
                        ddlTime.SelectedValue = entity.TimeFormat;

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindScreensData()
        {
            try
            {
                List<ScreenEntityVED> list = new List<ScreenEntityVED>();
                list = DBAccessVED.GetAndonSettingsScreensData(txtDeviceName.Text, new AndonSettingEntityVED());

                gvScreenList.DataSource = list;
                gvScreenList.DataBind();


                List<AndonFontSettingEntity> fontSettingList = new List<AndonFontSettingEntity>();
                fontSettingList = DBAccessVED.GetFontSettingsAndon(txtDeviceName.Text);

                if (fontSettingList != null && fontSettingList.Count > 0)
                {
                    txtDonutChartFontSize.Text = fontSettingList.Where(x => x.ValueInText == "OEEChartFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                    txtPieChartFontSize.Text = fontSettingList.Where(x => x.ValueInText == "PieChartFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                    txtColumnxAxisFontSize.Text = fontSettingList.Where(x => x.ValueInText == "ColumnChartxAxisFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                    txtColumnyAxisFontSize.Text = fontSettingList.Where(x => x.ValueInText == "ColumnChartyAxisFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                    txtColumnDatalabelFontSize.Text = fontSettingList.Where(x => x.ValueInText == "ColumnChartdataLabelsFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                    txtParetoxAxisFontSize.Text = fontSettingList.Where(x => x.ValueInText == "ParetoChartxAxisFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                    txtParetoyAxisFontSize.Text = fontSettingList.Where(x => x.ValueInText == "ParetoChartyAxisFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                    txtParetoColumnDataLabelFontSize.Text = fontSettingList.Where(x => x.ValueInText == "ParetoChartColumnDatalabelsFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                    txtParetoDataLabelFontSize.Text = fontSettingList.Where(x => x.ValueInText == "ParetoChartParetoDatalabelsFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                    txttabledataHeaderFontSize.Text = fontSettingList.Where(x => x.ValueInText == "TabledataHeaderfontsize").Select(x => x.ValueInInt).FirstOrDefault();
                    txttabledatacontentfontsize.Text = fontSettingList.Where(x => x.ValueInText == "TabledataContentfontsize").Select(x => x.ValueInInt).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindScreensData= " + ex.ToString());
            }
        }

        private void BindCockpitParameters()
        {
            string computerName = "";
            List<AndonDefaultsEntity> entities = new List<AndonDefaultsEntity>();
            if (HttpContext.Current.Request.Cookies["ComputerName"] != null)
            {
                computerName = HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString();
            }
            try
            {
                entities = AndonDBAccess.GetAndonViewSettingsData(ddlCellID.SelectedValue.ToString(), computerName, "CockpitAndonOrder");

                if (entities.Count > 0 && entities != null)
                {
                    txtMachineFontSize.Text = entities.AsEnumerable().Where(x => x.ValueInText.Equals("ProductionMachineNameFontSize", StringComparison.OrdinalIgnoreCase)).Select(x => x.LabelFontSize).FirstOrDefault();
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

        protected void btnScreenSave_Click(object sender, EventArgs e)
        {
            ScreenEntityVED data = null;
            int result = 0;
            try
            {
                string UpdatedTS = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (ddlSettingType.SelectedValue.Equals("ScreenSetting"))
                {
                    
                    foreach (GridViewRow row in gvScreenList.Rows)
                    {
                        data = new ScreenEntityVED();
                        data.Parameter = "AndonScreen_VED";
                        data.ValueInText = row.Cells[0].Text;
                        data.ScreenName = (row.FindControl("txtScreenName") as TextBox).Text;
                        data.IsVisible = (row.FindControl("chkIsVisible") as CheckBox).Checked;

                        result += DBAccessVED.SaveAndonSettingsScreensData(data, txtDeviceName.Text, UpdatedTS);

                    }

                    if (result > 0)
                        ScriptManager.RegisterStartupScript(this, GetType(), "succesflag", "alert('Saved Successfully!');", true);
                    else
                        ScriptManager.RegisterStartupScript(this, GetType(), "errorsflag", "alert('Failed to save data. Please, try again.');", true);

                    string Param = "FontSettings_VEDAndon";
                    DBAccessVED.SaveFontSettingData(Param, "OEEChartFontSize", txtDonutChartFontSize.Text, txtDeviceName.Text, UpdatedTS);
                    DBAccessVED.SaveFontSettingData(Param, "PieChartFontSize", txtPieChartFontSize.Text, txtDeviceName.Text, UpdatedTS);
                    DBAccessVED.SaveFontSettingData(Param, "ColumnChartxAxisFontSize", txtColumnxAxisFontSize.Text, txtDeviceName.Text, UpdatedTS);
                    DBAccessVED.SaveFontSettingData(Param, "ColumnChartyAxisFontSize", txtColumnyAxisFontSize.Text, txtDeviceName.Text, UpdatedTS);
                    DBAccessVED.SaveFontSettingData(Param, "ColumnChartdataLabelsFontSize", txtColumnDatalabelFontSize.Text, txtDeviceName.Text, UpdatedTS);
                    DBAccessVED.SaveFontSettingData(Param, "ParetoChartxAxisFontSize", txtParetoxAxisFontSize.Text, txtDeviceName.Text, UpdatedTS);
                    DBAccessVED.SaveFontSettingData(Param, "ParetoChartyAxisFontSize", txtParetoyAxisFontSize.Text, txtDeviceName.Text, UpdatedTS);
                    DBAccessVED.SaveFontSettingData(Param, "ParetoChartColumnDatalabelsFontSize", txtParetoColumnDataLabelFontSize.Text, txtDeviceName.Text, UpdatedTS);
                    DBAccessVED.SaveFontSettingData(Param, "ParetoChartParetoDatalabelsFontSize", txtParetoDataLabelFontSize.Text, txtDeviceName.Text, UpdatedTS);
                    DBAccessVED.SaveFontSettingData(Param, "TabledataHeaderfontsize", txttabledataHeaderFontSize.Text, txtDeviceName.Text, UpdatedTS);
                    DBAccessVED.SaveFontSettingData(Param, "TabledataContentfontsize", txttabledatacontentfontsize.Text, txtDeviceName.Text, UpdatedTS);

                    BindScreensData();
                }
                else if (ddlSettingType.SelectedValue.Equals("CockpitAndonParameters"))
                {
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


                            AndonDBAccess.UpdateGridSettingData("CockpitAndonOrder", Column, customColName, Sortorder, chkval, TextAlign, LabelFontSize, DataFontSize, ddlCellID.SelectedValue.ToString(), txtDeviceName.Text, out isSuccessfull);
                        }
                        index++;
                    }

                    AndonDBAccess.UpdateGridSettingData("CockpitAndonOrder", "ProductionMachineNameFontSize", string.Empty, string.Empty, "true", string.Empty, txtMachineFontSize.Text.ToString(), string.Empty, ddlCellID.SelectedValue.ToString(), txtDeviceName.Text, out isSuccessfull);

                    if (isSuccessfull.Equals("Successfull"))
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Successfully Updated')", true);
                    else
                        ScriptManager.RegisterStartupScript(this, GetType(), "errorsflag", "alert('Failed to save data. Please, try again.');", true);

                    BindCockpitParameters();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnScreenSave_Click= " + ex.ToString());
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string isSuccessfull = string.Empty;
            try
            {
                string Pathimg = string.Empty, PathVideo = string.Empty;
                int ShowImage = 0, ShowVideo = 0;

                if (chkImage.Checked == true) ShowImage = 1;
                if (chkVideo.Checked == true) ShowVideo = 1;

                if (chkImage.Checked)
                {
                    if (!Directory.Exists(txtPathImg.Text))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Enter Correct Image Path.')", true);
                        return;
                    }
                    else
                        Pathimg = txtPathImg.Text;
                }
                if (chkVideo.Checked)
                {
                    if (!Directory.Exists(txtPathVideo.Text))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Enter Correct Video Path.')", true);
                        return;
                    }
                    else
                        PathVideo = txtPathVideo.Text;
                }

                AndonSettingEntityVED entity = new AndonSettingEntityVED();
                entity.FontFamily = ddlFontName.SelectedValue;
                entity.FontStyle = ddlFontStyle.SelectedValue;
                entity.DataRefreshInterval = Convert.ToInt32(ddlDataRefreshInterval.SelectedValue);
                entity.ScreenFlipInterval = Convert.ToInt32(ddlScreenFlipInterval.SelectedValue);
                entity.ImageEnabled = ShowImage;
                entity.VideoEnabled = ShowVideo;
                entity.ImagePath = Pathimg;
                entity.VideoPath = PathVideo;
                entity.SlideshowInterval = Convert.ToInt32(ddlSlideShow.SelectedValue);
                entity.EmojiEnabled = chkShowEmoji.Checked ? 1 : 0;
                entity.EmojiSize = Convert.ToInt32(ddlEmojiSize.SelectedValue);
                entity.FooterBlockEnabled = chkshowFooter.Checked ? 1 : 0;
                entity.MsgBlockEnabled = chkShowMessage.Checked ? 1 : 0;
                entity.ScrollingMsg = txtScrollingMessage.Text;
                entity.DateFormat = ddlDate.SelectedValue;
                entity.TimeFormat = ddlTime.SelectedValue;
                entity.Sortorder = ddlsortByName.SelectedValue;
                entity.orderby = ddlSortOrder.SelectedValue;
                entity.ShowCurvedBoxes = chkShowCurvedBoxes.Checked ? 1 : 0;

                DBAccessVED.UpdateAndonSettings(entity, "AndonCockpitAppSettings", txtDeviceName.Text, out isSuccessfull);

                AndonDBAccess.SaveAndonFrquency(txtDeviceName.Text, ddlFrequency.SelectedValue, "CockpitAndonFrequency");

                if (isSuccessfull.Equals("Saved"))
                {
                    BindSettings();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Data saved successfully.')", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Failed to save data. Please, try again.')", true);
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
                    ddlSlideShow.Enabled = true;
                }
                else
                {
                    txtPathVideo.Enabled = false;
                    ddlSlideShow.Enabled = false;
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
                Response.Redirect("AndonVED.aspx");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"btnAndon_Click: {ex.Message}");
            }
        }

        protected void ddlSettingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSettingType.SelectedValue.Equals("ScreenSetting"))
                {
                    gvScreenList.Visible = true;
                    trCellID.Visible = false;
                    trMachineFontSize.Visible = false;
                    gridviewSetting.Visible = false;
                    divScreenFontSettings.Visible = true;
                    BindScreensData();
                }
                else if (ddlSettingType.SelectedValue.Equals("CockpitAndonParameters"))
                {
                    gvScreenList.Visible = false;
                    trCellID.Visible = true;
                    trMachineFontSize.Visible = true;
                    gridviewSetting.Visible = true;
                    divScreenFontSettings.Visible = false;
                    BindCellID();
                    ddlCellID_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlSettingType_SelectedIndexChanged= " + ex.ToString());
            }
        }

        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindCockpitParameters();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlCellID_SelectedIndexChanged= " + ex.ToString());
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

        protected void btnSaveComputerName_Click(object sender, EventArgs e)
        {
            bool success = true;
            try
            {
                success &= AndonDBAccess.SaveRunOption(txtDeviceName.Text, ddlRunOptions.SelectedValue.ToString(), "ComputerRunOption");
                success &= AndonDBAccess.SaveBoxDimension(txtDeviceName.Text, "", "UseCustomWidth", chkUseCustomWidth.Checked);
                success &= AndonDBAccess.SaveBoxDimension(txtDeviceName.Text, txtBoxWidth.Text, "BoxWidth");
                success &= AndonDBAccess.SaveBoxDimension(txtDeviceName.Text, txtLeftMargin.Text, "LeftMargin");
                success &= AndonDBAccess.SaveBoxDimension(txtDeviceName.Text, txtTopMargin.Text, "TopMargin");
                if (success)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Succesfully Updated.')", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Save failed. Try again later.')", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

        }

    }
}