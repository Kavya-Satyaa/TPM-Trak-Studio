using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GenericAndon.Model;

namespace Web_TPMTrakDashboard.GenericAndon
{
    public partial class SlideShowControl : System.Web.UI.UserControl
    {
        public static AndonSettingEntity settings = null;
        private static int totalImageCount = 0, totalVideoCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        private void callNextScreen()
        {
            try
            {
                //flipInterval.Enabled = false;
                ((AndonPage)this.Page).showNextControl();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindCockpitData = " + ex.Message);
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            callNextScreen();
        }

        public void BindSlideShowData()
        {
            try
            {
                if (Cache[$"BindCacheData{HttpContext.Current.Session.SessionID}"] != null)
                {
                    AllAdnonEntity andonData = (AllAdnonEntity)Cache[$"BindCacheData{HttpContext.Current.Session.SessionID}"];
                    settings = andonData.AdnonSetting;
                    if (settings.EnableImage == true || settings.EnableVideo == true)
                    {
                        List<AndonDefaultsEntity> imgVideoList = new List<AndonDefaultsEntity>();
                        if (settings.EnableImage == true)
                        {
                            //var dir = new DirectoryInfo(settings.ImagePath);
                            List<string> fileNames = Directory.GetFiles(settings.ImagePath).ToList();
                            if (fileNames.Count > 0)
                            {
                                for (int i = 0; i < fileNames.Count; i++)
                                {
                                    var dotSpint = fileNames[i].Split('.');
                                    string extension = dotSpint[dotSpint.Length - 1];
                                    if (extension.Equals("jpeg", StringComparison.OrdinalIgnoreCase) || extension.Equals("png", StringComparison.OrdinalIgnoreCase) || extension.Equals("jpg", StringComparison.OrdinalIgnoreCase))
                                    {
                                        AndonDefaultsEntity data = new AndonDefaultsEntity();
                                        byte[] fileByte = System.IO.File.ReadAllBytes(fileNames[i]);
                                        if (fileByte != null)
                                        {
                                            data.ValueInText = Convert.ToBase64String(fileByte);
                                            data.Parameter = fileNames[i];
                                            imgVideoList.Add(data);
                                        }
                                    }
                                }
                            }
                        }
                        if (settings.EnableVideo == true)
                        {
                            var dir = new DirectoryInfo(settings.VideoPath);
                            //List<string> fileNames = (from flInfo in dir.GetFiles("*.*", SearchOption.TopDirectoryOnly) select flInfo.Name).ToList();
                            List<string> fileNames = Directory.GetFiles(settings.VideoPath).ToList();
                            if (fileNames.Count > 0)
                            {
                                for (int i = 0; i < fileNames.Count; i++)
                                {
                                    AndonDefaultsEntity data = new AndonDefaultsEntity();
                                    byte[] fileByte = System.IO.File.ReadAllBytes(fileNames[i]);
                                    if (fileByte != null)
                                    {
                                        data.Parameter = fileNames[i];
                                        data.ValueInText = Convert.ToBase64String(fileByte);

                                        imgVideoList.Add(data);
                                    }
                                }
                            }
                        }
                        int slideCount = 0;
                        var carouselInnerHtml = new StringBuilder();
                        var indicatorsHtml = new StringBuilder(@"<ol class='carousel-indicators'>");
                        for (int i = 0; i < imgVideoList.Count; i++)
                        {
                            var result = Path.GetExtension(imgVideoList[i].Parameter).ToLower();// fileNames[i].Substring(fileNames[i].LastIndexOf(".") + 1);
                            if (!result.ToString().Equals(".scc", StringComparison.OrdinalIgnoreCase))
                            {

                                if ((result == ".mp4") || (result == ".wmv") || (result == ".avi") || (result == ".mov") || (result == ".qt") || (result == ".yuv") || (result == ".mkv") ||
                                    (result == ".webm") || (result == ".flv") || (result == ".ogg"))
                                {
                                    if (carouselInnerHtml.ToString().Contains("active"))
                                    {
                                        carouselInnerHtml.AppendLine("<div class='item' data-slide='" + slideCount + "'>");
                                    }
                                    else
                                    {
                                        carouselInnerHtml.AppendLine("<div class='item active' data-slide='" + slideCount + "' >");
                                    }
                                    carouselInnerHtml.AppendFormat("<video  class='slide-image embed-responsive-item center-block makeStyle' id='v{0}' alt='Slide #{0}' playsinline='playsinline'  autoplay='autoplay' controls>\r\n", slideCount + 1);
                                    carouselInnerHtml.AppendFormat("<source src='{0}' type='video/mp4'>\r\n", "data:video/mp4;base64," + imgVideoList[i].ValueInText);
                                    carouselInnerHtml.AppendLine("</video>");
                                    carouselInnerHtml.AppendLine("</div>");
                                    indicatorsHtml.AppendLine(slideCount == 0 ? @"<li data-target='#myCarousel' data-slide-to='" + slideCount + "' class='active'></li>" : @"<li data-target='#myCarousel' data-slide-to='" + slideCount + "' class=''></li>");

                                }
                                else
                                {

                                    if (carouselInnerHtml.ToString().Contains("active"))
                                    {
                                        carouselInnerHtml.AppendLine("<div class='item' data-slide='" + slideCount + "'>");
                                    }
                                    else
                                    {
                                        carouselInnerHtml.AppendLine("<div class='item active' data-slide='" + slideCount + "'>");
                                    }
                                    // carouselInnerHtml.AppendLine(i == 0 ? "<div class='item active'>" : "<div class='item'>");
                                    carouselInnerHtml.AppendLine("<img class='img-responsive img-rounded center-block makeStyle' src='data:image/png;base64," + imgVideoList[i].ValueInText + "' alt='Slide #" + (slideCount + 1) + "'>");
                                    carouselInnerHtml.AppendLine("</div>");
                                    indicatorsHtml.AppendLine(slideCount == 0 ? @"<li data-target='#myCarousel' data-slide-to='" + slideCount + "' class='active'></li>" : @"<li data-target='#myCarousel' data-slide-to='" + slideCount + "' class=''></li>");

                                }
                                slideCount++;
                            }
                        }
                        indicatorsHtml.AppendLine("</ol>");

                        ltlCarouselImages.Text = carouselInnerHtml.ToString();
                        ltlCarouselIndicators.Text = indicatorsHtml.ToString();
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "setImages", "showImageVideo();", true);
                       // ScriptManager.RegisterClientScriptBlock(this, typeof(string), "reportStyles", "showImages1()", false);
                    }
                    else
                    {
                        callNextScreen();
                    }
                }
                else
                {
                    callNextScreen();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindSlideShowData = " + ex.Message);
            }
        }
    }
}