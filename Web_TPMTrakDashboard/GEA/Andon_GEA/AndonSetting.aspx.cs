using BusinessClassLibrary;
using ModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.Andon_GEA.Model;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;

namespace Web_TPMTrakDashboard.GEA.Andon_GEA
{
    public partial class AndonSetting : System.Web.UI.Page
    {
        public List<UserAccessModel> useAccessData = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["UserName"] == null)
            //{
            //    Response.Redirect("~/SignIn.aspx", false);
            //}
            //else
            if (!IsPostBack)
            {
                if (Session["UserAccessData"] == null)
                    Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"].ToString());
                else
                    useAccessData = Session["UserAccessData"] as List<UserAccessModel>;
                if (useAccessData.AsEnumerable().Where(x => x.Domain.Equals("GEA")).Where(x => x.Code.Equals("AndonSettings")).Select(x => x.Selected == true).SingleOrDefault())
                {
                    Session["ImageVideoSortData"] = null;
                    setCompanyLogo();
                    bindSettingData();
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "bind", "openErrorModal('No Authentication for the page.');", true);
                    //    homeBtn_Click(null, null);
                    //}

                }
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
        private static List<CockpitEntity> getImageVideoSortDetail()
        {
            List<CockpitEntity> list = new List<CockpitEntity>();
            try
            {
                if (HttpContext.Current.Session["ImageVideoSortData"] == null)
                {
                    list = GEADatabaseAccess.getAndonImageVideoSortOrderDetails();
                    HttpContext.Current.Session["ImageVideoSortData"] = list;
                }
                list = HttpContext.Current.Session["ImageVideoSortData"] as List<CockpitEntity>;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return list;
        }
        private static int getImageVideoSort(string name)
        {
            int sortOrder = 0;
            try
            {
                List<CockpitEntity> list = getImageVideoSortDetail();
                sortOrder = list.Where(k => k.ValueInText2 == name).Select(k => k.ValueInInt).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return sortOrder;
        }
        private void bindSettingData()
        {
            try
            {
                DataTable dt = GEADatabaseAccess.getSettingDetails();
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
                    if (dt.Rows[i]["ValueInText"].ToString() == "ShowDataBy")
                    {
                        ddlDailyWeekly.SelectedValue = dt.Rows[i]["ValueInText2"].ToString();
                    }
                    //if (dt.Rows[i]["ValueInText"].ToString() == "RefreshInterval")
                    //{
                    //    txtRefreshInterval.Text = dt.Rows[i]["ValueInInt"].ToString();
                    //}
                    if (dt.Rows[i]["ValueInText"].ToString() == "DataRefreshInterval")
                    {
                        txtDataRefreshInterval.Text = dt.Rows[i]["ValueInInt"].ToString();
                    }
                    if (dt.Rows[i]["ValueInText"].ToString() == "ShowDecanterDataBy")
                    {
                        if (ddlShiftDayType.Items.FindByValue(dt.Rows[i]["ValueInText2"].ToString()) != null)
                        {
                            ddlShiftDayType.SelectedValue = dt.Rows[i]["ValueInText2"].ToString();
                        }
                    }
                    if (dt.Rows[i]["ValueInText"].ToString() == "ImageVideoFlipInterval")
                    {
                        txtImageVideoFlipInterval.Text = dt.Rows[i]["ValueInInt"].ToString();
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
                List<AndonSettingData> listColumn = GEADatabaseAccess.getColumnData();
                lvPOColummsData.DataSource = listColumn;
                lvPOColummsData.DataBind();
                chkPOWithLeadLag.Checked = false;
                chkPOWithCharts.Checked = false;
                chkDecanterMachine.Checked = false;
                if (Request.Cookies["POWithLeadLag"] != null)
                {
                    if (Request.Cookies["POWithLeadLag"].Value.ToString().Trim() == "1")
                    {
                        chkPOWithLeadLag.Checked = true;
                    }
                }
                if (Request.Cookies["POWithCharts"] != null)
                {
                    if (Request.Cookies["POWithCharts"].Value.ToString().Trim() == "1")
                    {
                        chkPOWithCharts.Checked = true;
                    }
                }
                if (Request.Cookies["DecanterMachienShop"] != null)
                {
                    if (Request.Cookies["DecanterMachienShop"].Value.ToString().Trim() == "1")
                    {
                        chkDecanterMachine.Checked = true;
                    }
                }
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
                                postedFile.SaveAs(Server.MapPath("~/GEA/Andon_GEA/ImageVideoSilder/") + fileName);
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
                                postedFile.SaveAs(Server.MapPath("~/GEA/Andon_GEA/ImageVideoSilder/") + fileName);
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
                    showDataDayWeek = ddlDailyWeekly.SelectedValue;
                    //refreshInterval = txtRefreshInterval.Text.Trim();
                    dataRefreshInterval = txtDataRefreshInterval.Text.Trim();
                    //DBAccess.updateSettingData(machine, status, component, setting, timer, user, ae, pe, oee, plan, act, emoji, header, content, noofRows, imagePath, videoPath, showImage, showVideo, scrollingText, flipInterval);
                    bool result1;
                    GEADatabaseAccess.updateSettingData(header, content, noofRows, imagePath, videoPath, showImage, showVideo, scrollingText, flipInterval, showDataDayWeek, refreshInterval, dataRefreshInterval, out result1);
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
                    GEADatabaseAccess.setPOColumnSettings(listDetails, out result2);
                    //Response.Cookies["POWithLeadLag"].Expires = DateTime.Now.AddSeconds(-1);
                    //Response.Cookies["POWithCharts"].Expires = DateTime.Now.AddSeconds(-1);
                    //Response.Cookies["DecanterMachienShop"].Expires = DateTime.Now.AddSeconds(-1);
                    Response.Cookies["POWithLeadLag"].Value = chkPOWithLeadLag.Checked ? "1" : "0";
                    Response.Cookies["POWithLeadLag"].Expires = DateTime.MaxValue;
                    Response.Cookies["POWithCharts"].Value = chkPOWithCharts.Checked ? "1" : "0";
                    Response.Cookies["POWithCharts"].Expires = DateTime.MaxValue;
                    Response.Cookies["DecanterMachienShop"].Value = chkDecanterMachine.Checked ? "1" : "0";
                    Response.Cookies["DecanterMachienShop"].Expires = DateTime.MaxValue;
                    //bindSettingData();

                    GEADatabaseAccess.saveAndonValueInText2("ShowDecanterDataBy", ddlShiftDayType.SelectedValue);
                    GEADatabaseAccess.saveAndonValueInIntByText("ImageVideoFlipInterval", txtImageVideoFlipInterval.Text);

                    if (result1 == true && result2 == true)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "bindNoOfRows", "showpop5('Items are saved successfully');", true);
                        Session["ShowAndonSaveModal"] = "1";
                        //after save cockies data not showing properly so used Response.Redirect("AndonSetting.aspx");. Used Session to show save modal 
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "bindNoOfRows", "showpop5('Failed');", true);
                        Session["ShowAndonSaveModal"] = "0";
                    }
                    Response.Redirect("AndonSetting.aspx");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Apply andon setting" + ex.Message);
            }

        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<CockpitEntity> getImageDetails()
        {
            List<CockpitEntity> imageList = new List<CockpitEntity>();
            DirectoryInfo diInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/GEA/Andon_GEA/ImageVideoSilder/"));
            FileInfo[] files = diInfo.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                string extension = files[i].Extension.ToLower();
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                {
                    string filePath = HttpContext.Current.Server.MapPath("~/GEA/Andon_GEA/ImageVideoSilder/" + files[i].ToString());
                    if (File.Exists(filePath))
                    {
                        CockpitEntity data = new CockpitEntity();
                        data.Parameter = "ImageVideoSilder/" + files[i].Name;
                        data.ValueInInt = getImageVideoSort(files[i].Name);
                        imageList.Add(data);
                    }
                }
            }
            if (imageList.Count > 0)
            {
                imageList = imageList.OrderBy(k => k.ValueInInt).ToList();
            }
            return imageList;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void deleteImages(string[] names)
        {
            DirectoryInfo diInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/GEA/Andon_GEA/ImageVideoSilder/"));
            FileInfo[] files = diInfo.GetFiles();
            for (int j = 0; j < names.Length; j++)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name == names[j])
                    {
                        string filePath = HttpContext.Current.Server.MapPath("~/GEA/Andon_GEA/ImageVideoSilder/" + files[i].ToString());
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            deleteSortOrderDetails(files[i].Name);
                        }
                    }
                }
            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<CockpitEntity> getVideoDetails()
        {
            List<CockpitEntity> videoList = new List<CockpitEntity>();
            DirectoryInfo diInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/GEA/Andon_GEA/ImageVideoSilder/"));
            FileInfo[] files = diInfo.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                string result = files[i].Extension.ToLower();
                if ((result == ".mp4") || (result == ".wmv") || (result == ".avi") || (result == ".mov") || (result == ".qt") || (result == ".yuv") || (result == ".mkv") || (result == ".webm") || (result == ".flv") || (result == ".ogg"))
                {
                    string filePath = HttpContext.Current.Server.MapPath("~/GEA/Andon_GEA/ImageVideoSilder/" + files[i].ToString());
                    if (File.Exists(filePath))
                    {
                        CockpitEntity data = new CockpitEntity();
                        data.Parameter = "ImageVideoSilder/" + files[i].Name;
                        data.ValueInInt = getImageVideoSort(files[i].Name);
                        videoList.Add(data);
                    }
                }
            }
            if (videoList.Count > 0)
            {
                videoList = videoList.OrderBy(k => k.ValueInInt).ToList();
            }
            return videoList;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void deleteVideos(string[] names)
        {
            DirectoryInfo diInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/GEA/Andon_GEA/ImageVideoSilder/"));
            FileInfo[] files = diInfo.GetFiles();
            for (int j = 0; j < names.Length; j++)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name == names[j])
                    {
                        string filePath = HttpContext.Current.Server.MapPath("~/GEA/Andon_GEA/ImageVideoSilder/" + files[i].ToString());
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            deleteSortOrderDetails(files[i].Name);
                        }
                    }
                }
            }
        }
        private static int deleteSortOrderDetails(string name)
        {
            int result = 0;
            try
            {
                result = GEADatabaseAccess.deleteAndonImageVideoSortOrderDetails(name);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return result;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static int saveImageVideoSortOrder(string name, string order)
        {
            int result = 0;
            try
            {
                result = GEADatabaseAccess.insertImageVideoSortOrderDetails(name, order);
                HttpContext.Current.Session["ImageVideoSortData"] = null;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return result;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string getSavePopUpValueFromSession()
        {
            string result = "";
            try
            {
                if (HttpContext.Current.Session["ShowAndonSaveModal"] != null)
                {
                    result = HttpContext.Current.Session["ShowAndonSaveModal"].ToString();
                    HttpContext.Current.Session["ShowAndonSaveModal"] = null;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return result;
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