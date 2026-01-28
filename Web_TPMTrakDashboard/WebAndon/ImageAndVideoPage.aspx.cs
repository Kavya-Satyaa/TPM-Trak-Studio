using Elmah;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.WebAndon
{
    public partial class ImageAndVideoPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || !Request.IsAuthenticated || Session["connectionString"] == null)
            {
                Session["UserName"] = AndonCockpitView.GetDefaultANDONUser();//SATYA
                Response.Redirect("../SignIn.aspx", false);
            }
            else
            {
                Button btnMode = (Button)Page.Master.FindControl("btnToggel");
                btnMode.Visible = false;
                BindSliderImage();
            }            
        }

        private void BindSliderImage()
        {
            ObjectCache cache = MemoryCache.Default;
            try
            {
                if (cache["CarouselInnerHtml"] != null && cache["CarouselIndicatorsHtml"] != null)
                {
                    //use the cached html
                    ltlCarouselImages.Text = cache["CarouselInnerHtml"].ToString();
                    ltlCarouselIndicators.Text = cache["CarouselIndicatorsHtml"].ToString();
                }
                else
                {
                    //get a list of images from the folder
                    const string imagesPath = "../Image/Slideshow/";// "~/Image/Slideshow/";
                    var dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(imagesPath));
                    //filtering to jpgs, but ideally not required
                    List<string> fileNames = (from flInfo in dir.GetFiles("*.*", SearchOption.TopDirectoryOnly) select flInfo.Name).ToList();
                    //List<string> fileNames = (from flInfo in dir.GetFiles() where flInfo.Name.EndsWith(".jpg") select flInfo.Name).ToList();
                    if (fileNames.Count > 0)
                    {
                        var carouselInnerHtml = new StringBuilder();
                        var indicatorsHtml = new StringBuilder(@"<ol class='carousel-indicators'>");
                        //loop through and build up the html for indicators + images                      
                        for (int i = 0; i < fileNames.Count; i++)
                        {
                            var result = Path.GetExtension(fileNames[i]).ToLower();// fileNames[i].Substring(fileNames[i].LastIndexOf(".") + 1);
                            if (!result.ToString().Equals(".scc", StringComparison.OrdinalIgnoreCase))
                            {
                                carouselInnerHtml.AppendLine(i == 0 ? "<div class='item active'>" : "<div class='item'>");
                                if ((result == ".mp4") || (result == ".wmv") || (result == ".avi") || (result == ".mov") || (result == ".qt") || (result == ".yuv") || (result == ".mkv") ||
                                    (result == ".webm") || (result == ".flv") || (result == ".ogg"))
                                {
                                    carouselInnerHtml.AppendFormat("<video class='slide-image embed-responsive-item center-block makeStyle' id='v{0}' alt='Slide #{0}' controls>\r\n", i + 1);
                                    carouselInnerHtml.AppendFormat("<source src='{0}' type='video/mp4'>\r\n", imagesPath + fileNames[i]);
                                    carouselInnerHtml.AppendLine("</video>");
                                }
                                else
                                {
                                    //img-fluid img-thumbnail
                                    carouselInnerHtml.AppendLine("<img class='img-responsive img-rounded center-block makeStyle' src='" + imagesPath + fileNames[i] + "' alt='Slide #" + (i + 1) + "'>");
                                }
                                carouselInnerHtml.AppendLine("</div>");
                                indicatorsHtml.AppendLine(i == 0 ? @"<li data-target='#myCarousel' data-slide-to='" + i + "' class='active'></li>" : @"<li data-target='#myCarousel' data-slide-to='" + i + "' class=''></li>");
                            }
                        }
                        //close tag                      
                        indicatorsHtml.AppendLine("</ol>");
                        //stick the html in the literal tags and the cache
                        CacheItemPolicy policy = new CacheItemPolicy();
                        //update
                        policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(10);
                        cache.Set("CarouselInnerHtml", carouselInnerHtml.ToString(), policy);
                        cache.Set("CarouselIndicatorsHtml", indicatorsHtml.ToString(), policy);


                        ltlCarouselImages.Text = carouselInnerHtml.ToString();
                        ltlCarouselIndicators.Text = indicatorsHtml.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                //something is dodgy so flush the cache
                if (cache["CarouselInnerHtml"] != null)
                {
                    Cache.Remove("CarouselInnerHtml");
                }
                if (cache["CarouselIndicatorsHtml"] != null)
                {
                    Cache.Remove("CarouselIndicatorsHtml");
                }
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }
    }
}