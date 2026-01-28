using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik.Andon_Advik.Model;
using Web_TPMTrakDashboard.Advik.DataBaseAccess;

namespace Web_TPMTrakDashboard.Advik.Andon_Advik
{
    public partial class AndonSetting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setCompanyLogo();
                bindSettingData();
            }
        }
        private void setCompanyLogo()
        {
            //const string imagesPath = "~/CompanyLogo/";// "~/Image/Slideshow/";
            //var dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(imagesPath));

            ////filtering to jpgs, but ideally not required
            //List<string> fileNames = (from flInfo in dir.GetFiles() select flInfo.Name).ToList();
            //if (fileNames.Count > 0)
            //{
            //    Image2.ImageUrl = imagesPath + fileNames[0];
            //}
            //else
            //{
            //    Image2.ImageUrl = "Image/companyIcon.png";
            //}
            Image2.ImageUrl = Web_TPMTrakDashboard.Models.Util.getCompanyLogoPath();
        }
        private void bindSettingData()
        {
            try
            {
                DataTable dt = AdvikDatabaseAccess.getSettingDetails();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["ValueInText2"].ToString() == "Header" && dt.Rows[i]["ValueInText"].ToString() == "FontSize")
                    {
                        headerFontSz.Text = dt.Rows[i]["ValueInInt"].ToString();
                    }
                    if (dt.Rows[i]["ValueInText2"].ToString() == "Content" && dt.Rows[i]["ValueInText"].ToString() == "FontSize")
                    {
                        contentFontSz.Text = dt.Rows[i]["ValueInInt"].ToString();
                    }
                    if (dt.Rows[i]["ValueInText"].ToString() == "NoOfRows")
                    {
                        txtNoOfRows.Text = dt.Rows[i]["ValueInInt"].ToString();
                    }
                    if (dt.Rows[i]["ValueInText"].ToString() == "ShowImage")
                    {
                        if (dt.Rows[i]["ValueInBool"].ToString() == "1")
                        {
                            chkShowImage.Checked = true;
                        }
                    }
                    if (dt.Rows[i]["ValueInText"].ToString() == "ShowVideo")
                    {
                        if (dt.Rows[i]["ValueInBool"].ToString() == "1")
                        {
                            chkShowVideo.Checked = true;
                        }
                    }
                    if (dt.Rows[i]["ValueInText"].ToString() == "ScrollingText")
                    {
                        txtScrolling.Text = dt.Rows[i]["ValueInText2"].ToString();
                    }
                    if (dt.Rows[i]["ValueInText"].ToString() == "FlipInterval")
                    {
                        txtFlipInterval.Text = dt.Rows[i]["ValueInInt"].ToString();
                    }
                    //if (dt.Rows[i]["ValueInText"].ToString() == "ShowDataBy")
                    //{
                    //    ddlDailyWeekly.SelectedValue = dt.Rows[i]["ValueInText2"].ToString();
                    //}
                    if (dt.Rows[i]["ValueInText"].ToString() == "RefreshInterval")
                    {
                        txtRefreshInterval.Text = dt.Rows[i]["ValueInInt"].ToString();
                    }
                    if (dt.Rows[i]["ValueInText"].ToString() == "DataRefreshInterval")
                    {
                        txtDataRefreshInterval.Text = dt.Rows[i]["ValueInInt"].ToString();
                    }
                    //if (dt.Rows[i]["ValueInText"].ToString() == "ImagePath")
                    //{
                    //    txtImagePath.Text = dt.Rows[i]["ValueInText2"].ToString();
                    //}
                    //if (dt.Rows[i]["ValueInText"].ToString() == "VideoPath")
                    //{
                    //    txtVideoPath.Text = dt.Rows[i]["ValueInText2"].ToString();
                    //}
                }
                //DataTable result = dt.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "POColumn").CopyToDataTable();
                //DataTable firstTable = result.AsEnumerable().Take(6).CopyToDataTable();
                //DataTable secondTable = result.Rows.Cast<System.Data.DataRow>().Skip(6).Take(6).CopyToDataTable();
                //chkListviewColumns.DataSource = firstTable;
                //chkListviewColumns.DataTextField = "ValueInText2";
                //chkListviewColumns.DataValueField = "ValueInBool";
                //chkListviewColumns.DataBind();
                //foreach (ListItem item in chkListviewColumns.Items)
                //{
                //    if (item.Value == "1")
                //    {
                //        item.Selected = true;
                //    }
                //}

                //chkListviewColumns2.DataSource = secondTable;
                //chkListviewColumns2.DataTextField = "ValueInText2";
                //chkListviewColumns2.DataValueField = "ValueInBool";
                //chkListviewColumns2.DataBind();
                //foreach (ListItem item in chkListviewColumns2.Items)
                //{
                //    if (item.Value == "1")
                //    {
                //        item.Selected = true;
                //    }
                //}
                List<AndonSettingData> listColumn = AdvikDatabaseAccess.getColumnData();
                lvPOColummsData.DataSource = listColumn;
                lvPOColummsData.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While binding andon setting" + ex.Message);
            }
        }

        protected void applyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int header = 20, content = 16, noofRows = 6, showImage = 0, showVideo = 0;
               
                string imagePath = "", videoPath = "", scrollingText = "", showDataDayWeek = "";
                string flipInterval = "", refreshInterval = "", dataRefreshInterval = "";
                if (hdnshowToastr.Value == "update")
                {
                    hdnshowToastr.Value = "";
                    if (hdnImage.Value == "addimage")
                    {
                        if (imageFileUpload.HasFile)
                        {
                            int flag = 0;
                            foreach (HttpPostedFile postedFile in imageFileUpload.PostedFiles)
                            {
                                string extension = Path.GetExtension(postedFile.FileName).ToLower();
                                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                                {
                                    flag = 1;
                                    break;
                                }
                            }
                            if (flag == 1)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "bind", "openErrorModal('Invalid image file.');", true);
                                return;
                            }
                        }
                        string imageNames = string.Empty;
                        if (imageFileUpload.HasFile)
                        {
                            foreach (HttpPostedFile postedFile in imageFileUpload.PostedFiles)
                            {
                                string fileName = Path.GetFileName(postedFile.FileName);
                                postedFile.SaveAs(Server.MapPath("~/Advik/Andon_Advik/ImageVideoSilder/") + fileName);
                                imageNames += fileName + ",";
                            }
                            if (imageNames != string.Empty)
                            {
                                imageNames = imageNames.Remove(imageNames.Length - 1, 1);
                            }
                        }
                    }
                    if (hdnVideo.Value == "addvideo")
                    {
                        if (videoFileUpload.HasFile)
                        {
                            int flag = 0;
                            foreach (HttpPostedFile postedFile in videoFileUpload.PostedFiles)
                            {
                                string result = Path.GetExtension(postedFile.FileName);
                                if ((result != ".mp4") && (result != ".wmv") && (result != ".avi") && (result != ".mov") && (result != ".qt") && (result != ".yuv") && (result != ".mkv") && (result != ".webm") && (result != ".flv") && (result != ".ogg"))
                                {
                                    flag = 1;
                                    break;
                                }
                            }
                            if (flag == 1)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "bind", "openErrorModal('Invalid video file.');", true);
                                return;
                            }
                        }
                        if (videoFileUpload.HasFile)
                        {
                            foreach (HttpPostedFile postedFile in videoFileUpload.PostedFiles)
                            {
                                string fileName = Path.GetFileName(postedFile.FileName);
                                postedFile.SaveAs(Server.MapPath("~/Advik/Andon_Advik/ImageVideoSilder/") + fileName);
                            }
                        }
                    }

                    header = Convert.ToInt32(headerFontSz.Text);
                    content = Convert.ToInt32(contentFontSz.Text);
                    noofRows = Convert.ToInt32(txtNoOfRows.Text);

                    showImage = Convert.ToInt32(chkShowImage.Checked);
                    showVideo = Convert.ToInt32(chkShowVideo.Checked);
                    scrollingText = txtScrolling.Text;
                    flipInterval = txtFlipInterval.Text.Trim();
                   // showDataDayWeek = ddlDailyWeekly.SelectedValue;
                    refreshInterval = txtRefreshInterval.Text.Trim();
                    dataRefreshInterval =txtDataRefreshInterval.Text.Trim();
                    //DBAccess.updateSettingData(machine, status, component, setting, timer, user, ae, pe, oee, plan, act, emoji, header, content, noofRows, imagePath, videoPath, showImage, showVideo, scrollingText, flipInterval);
                    bool result1;
                    AdvikDatabaseAccess.updateSettingData(header, content, noofRows, imagePath, videoPath, showImage, showVideo, scrollingText, flipInterval,  refreshInterval, dataRefreshInterval, out result1);
                    List<AndonSettingData> listDetails = new List<AndonSettingData>();
                    for (int i = 0; i < lvPOColummsData.Items.Count; i++)
                    {
                        AndonSettingData data = new AndonSettingData();
                        data.Column = (lvPOColummsData.Items[i].FindControl("lblColumn") as Label).Text;
                        data.CustomColumn = (lvPOColummsData.Items[i].FindControl("txtCustomName") as TextBox).Text;
                        data.Visibility = (lvPOColummsData.Items[i].FindControl("chkColumnVisibility") as CheckBox).Checked;
                        listDetails.Add(data);
                    }
                    bool result2;
                    AdvikDatabaseAccess.setPOColumnSettings(listDetails, out result2);
                    bindSettingData();
                    if (result1 == true && result2 == true)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "bindNoOfRows", "showpop5('Items are saved successfully');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "bindNoOfRows", "showpop5('Failed');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Apply andon setting" + ex.Message);
            }

        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<string> getImageDetails()
        {
            List<string> images = new List<string>();
            DirectoryInfo diInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Advik/Andon_Advik/ImageVideoSilder/"));
            FileInfo[] files = diInfo.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                string extension = files[i].Extension.ToLower();
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                {
                    string filePath = HttpContext.Current.Server.MapPath("~/Advik/Andon_Advik/ImageVideoSilder/" + files[i].ToString());
                    if (File.Exists(filePath))
                    {
                        images.Add("ImageVideoSilder/" + files[i].Name);
                    }
                }
            }
            return images;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void deleteImages(string[] names)
        {
            DirectoryInfo diInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Advik/Andon_Advik/ImageVideoSilder/"));
            FileInfo[] files = diInfo.GetFiles();
            for (int j = 0; j < names.Length; j++)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name == names[j])
                    {
                        string filePath = HttpContext.Current.Server.MapPath("~/Advik/Andon_Advik/ImageVideoSilder/" + files[i].ToString());
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }
                }
            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<string> getVideoDetails()
        {
            List<string> video = new List<string>();
            DirectoryInfo diInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Advik/Andon_Advik/ImageVideoSilder/"));
            FileInfo[] files = diInfo.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                string result = files[i].Extension.ToLower();
                if ((result == ".mp4") || (result == ".wmv") || (result == ".avi") || (result == ".mov") || (result == ".qt") || (result == ".yuv") || (result == ".mkv") || (result == ".webm") || (result == ".flv") || (result == ".ogg"))
                {
                    string filePath = HttpContext.Current.Server.MapPath("~/Advik/Andon_Advik/ImageVideoSilder/" + files[i].ToString());
                    if (File.Exists(filePath))
                    {
                        video.Add("ImageVideoSilder/" + files[i].Name);
                    }
                }
            }
            return video;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void deleteVideos(string[] names)
        {
            DirectoryInfo diInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Advik/Andon_Advik/ImageVideoSilder/"));
            FileInfo[] files = diInfo.GetFiles();
            for (int j = 0; j < names.Length; j++)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name == names[j])
                    {
                        string filePath = HttpContext.Current.Server.MapPath("~/Advik/Andon_Advik/ImageVideoSilder/" + files[i].ToString());
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }
                }
            }
        }

        protected void homeBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("HelpRequestProductionAndon.aspx");
        }
        protected void cancelBtn_ServerClick(object sender, EventArgs e)
        {
            bindSettingData();
        }
    }
}