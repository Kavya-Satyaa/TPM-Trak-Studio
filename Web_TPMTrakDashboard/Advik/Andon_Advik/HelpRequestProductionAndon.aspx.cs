using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.ObjectModel;
using System.Runtime.Caching;
using System.IO;
using System.Text;
using Web_TPMTrakDashboard.Advik.Andon_Advik.Model;
using Web_TPMTrakDashboard.Advik.DataBaseAccess;
using System.Threading.Tasks;

namespace Web_TPMTrakDashboard.Advik.Andon_Advik
{
    public partial class HelpRequestProductionAndon : System.Web.UI.Page
    {
        List<PODetails> listPOData = new List<PODetails>();
        int count = 0, noOfRows, flips = 0, Interval = 0;
        decimal refreshData = 0;
        public static string screenName = "";
        public static string refreshScreenName = "";
        public static string inputToBindData = "";
        ObjectCache cache = MemoryCache.Default;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Cache.Remove("AdvikPOData");
                Cache.Remove("AdvikLeadingDetails");
                Cache.Remove("AdvikLaggingDetails");
                Cache.Remove("AdvikchartDetails");

                bindPlantID();
                imgBtnSwitch.ToolTip = "Switch to ANDON Mode";
                imgBtnSwitch_Click(null, null);
               // bindCellID();
                setCompanyLogo();
            }
        }

        private void bindCellID()
        {
            try
            {
                string plant = ddlPlantID.SelectedValue == null ? "" : ddlPlantID.SelectedItem.ToString();
                List<string> GetCell = AdvikDatabaseAccess.GetCell(plant);
                GetCell.RemoveAt(0);
               // ddlCellID.DataSource = GetCell;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
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
        public void bindPOWithLeadLag()
        {
            try
            {
                screenName = "POStatusWithChart";
                refreshScreenName = "POStatusWithChart";

                DataTable dtSetting = AdvikDatabaseAccess.getSettingDetails();
                if (dtSetting.Rows.Count > 0)
                {
                    spanWelcome.InnerText = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "ScrollingText").Select(rows => rows.Field<string>("ValueInText2")).ToList()[0];
                    headerName.InnerText = "Production Status";

                    Interval = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "FlipInterval").Select(rows => rows.Field<int>("ValueInInt")).ToList()[0];
                    //   int refreshInt = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "RefreshInterval").Select(rows => rows.Field<int>("ValueInInt")).ToList()[0];
                    int refreshInt;
                    string flipImagefloder = "";
                    try
                    {
                        flipImagefloder = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "RefreshInterval").Select(rows => rows.Field<int>("ValueInInt")).ToList()[0].ToString();
                    }
                    catch (Exception ex)
                    {
                        flipImagefloder = "";
                    }

                    if (flipImagefloder == null || flipImagefloder == "")
                    {
                        refreshInt = 15000000;
                    }
                    else
                    {
                        refreshInt = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "RefreshInterval").Select(rows => rows.Field<int>("ValueInInt")).ToList()[0];
                    }
                    decimal refreshTick = Math.Ceiling(refreshInt / Convert.ToDecimal(Interval));
                    Session["RefreshInterval"] = refreshTick;

                    BindPOData("charts");
                    displayCharts();
                    poInterval.Enabled = true;
                    poInterval.Interval = Interval * 1000;

                }
            }
            catch(Exception ex1)
            { }

        }
        public void bindPlantID()
        {
            List<string> list = AdvikDatabaseAccess.getPlantID();
            ddlPlantID.DataSource = list;
            ddlPlantID.DataBind();
        }
        public void displayLaggingAndLeadingMachine()
        {
            poContainer.Visible = true;
            laggingMachineContainer.Visible = true;
            leadingMachineContainer.Visible = true;
            downTimeContainer.Visible = false;
            oeeConatiner.Visible = false;
            imageVideoConatiner.Visible = false;
        }
        public void displayCharts()
        {
            poContainer.Visible = true;
            laggingMachineContainer.Visible = false;
            leadingMachineContainer.Visible = false;
            downTimeContainer.Visible = true;
            oeeConatiner.Visible = true;
            imageVideoConatiner.Visible = false;
        }
        public void displayImageVideoSlider()
        {
            poInterval.Enabled = false;
            poContainer.Visible = false;
            laggingMachineContainer.Visible = false;
            leadingMachineContainer.Visible = false;
            downTimeContainer.Visible = false;
            oeeConatiner.Visible = false;
            imageVideoConatiner.Visible = true;
        }
        private void showHideListviewColumn(ListView listview, List<AndonSettingData> list)
        {
            try
            {
                foreach (AndonSettingData data in list)
                {
                    if (data.Visibility == false)
                    {
                        (listview.FindControl("th" + data.Column) as HtmlControl).Visible = false;
                        for (int j = 0; j < listview.Items.Count; j++)
                        {
                            (listview.Items[j].FindControl("td" + data.Column) as HtmlControl).Visible = false;
                        }
                    }
                    else
                    {
                        HtmlControl control = (listview.FindControl("th" + data.Column) as HtmlControl);
                        int flag = 0;
                        if (control.HasControls())
                        {
                            foreach (Control c in control.Controls)
                            {
                                if (c is HtmlImage)
                                {
                                    flag = 1;
                                }
                            }
                            if (flag == 0)
                            {
                                (listview.FindControl("th" + data.Column) as HtmlTableCell).InnerText = data.CustomColumn;
                            }
                            else
                            {
                                (listview.FindControl("lbl" + data.Column) as HtmlGenericControl).InnerText = data.CustomColumn;
                            }
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                
            }
          
        }

        void BindPOData(string param)
        {
            try
            {
                inputToBindData = param;

                TimeSpan ts = new TimeSpan(0, 0, 300);
                if (Cache["AdvikPOData"] == null)
                {
                    listPOData = AdvikDatabaseAccess.gridMachineStatusLoad("", ddlPlantID.SelectedValue == null ? "" : ddlPlantID.SelectedValue,  "1st Screen");
                    Cache["AdvikPOData"] = listPOData;
                    Cache.Insert("AdvikPOData", listPOData, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);
                }
                else

                {
                    listPOData = (List<PODetails>)Cache["AdvikPOData"];
                }

                noOfRows = 5;
                if (listPOData != null && listPOData.Count > 0)
                {
                    DataTable dtSetting = AdvikDatabaseAccess.getSettingDetails();
                    IEnumerable<int> rowzz = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "NoOfRows").Select(rows => rows.Field<int>("ValueInInt"));
                    foreach (int rows in rowzz)
                    {
                        noOfRows = rows;
                    }
                    Session["showImages"] = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "ShowImage").Select(rows => rows.Field<int>("ValueInBool")).ToList()[0];
                    Session["showVideo"] = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "ShowVideo").Select(rows => rows.Field<int>("ValueInBool")).ToList()[0];
                    count = listPOData.Count;
                    flips = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(count) / noOfRows));
                    //IEnumerable<PODetails> data = listPOData.Take(noOfRows * 1);
                    lvPOData.DataSource = listPOData;
                    lvPOData.DataBind();

                    List<AndonSettingData> result = AdvikDatabaseAccess.getColumnData();
                    showHideListviewColumn(lvPOData, result);


                    decimal.TryParse(Session["RefreshInterval"].ToString(), out refreshData);
                    Session["RefreshInterval"] = refreshData - 1;

                    Session["POFlips"] = flips;
                    Session["POData"] = listPOData;
                    Session["Rows"] = noOfRows;
                    Session["NoOfSkipRows"] = 1;
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "bindNoOfRows", "setPORowsBasedOnHeight();", true);
                //if (screenName == "POStatusWithChart")
                //{
                //    displayCharts();
                //    headerName.InnerText = "Production Status";
                //    ScriptManager.RegisterStartupScript(this, GetType(), "bind", "bindCharts();", true);
                //}
                if (param == "charts")
                {
                    displayCharts();
                    headerName.InnerText = "Production Status";
                    ScriptManager.RegisterStartupScript(this, GetType(), "bind", "bindCharts();", true);
                }
                else
                {
                    displayLaggingAndLeadingMachine();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While Binding PO Details " + ex.Message);
            }

        }

        protected void poInterval_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Session["POFlips"] == null)
                {
                    poInterval.Enabled = false;
                    return;
                }
                int.TryParse(Session["POFlips"].ToString(), out flips);
                if (Convert.ToDecimal(Session["RefreshInterval"].ToString()) <= 0)
                {
                    displayImageVideoSlider();
                    headerName.InnerText = "Images / Videos";
                    bindImageVideo();
                    return;
                }
                if (flips > 1)
                {
                    int.TryParse(Session["Rows"].ToString(), out noOfRows);
                    listPOData = (List<PODetails>)Session["POData"];

                    IEnumerable<PODetails> data = listPOData.Skip(noOfRows * Convert.ToInt32(Session["NoOfSkipRows"].ToString())).Take(noOfRows);
                    lvPOData.DataSource = data;
                    lvPOData.DataBind();

                    DataTable dtSetting = AdvikDatabaseAccess.getSettingDetails();
                   
                    List<AndonSettingData> result = AdvikDatabaseAccess.getColumnData();
                    showHideListviewColumn(lvPOData, result);

                    Session["NoOfSkipRows"] = Convert.ToInt32(Session["NoOfSkipRows"].ToString()) + 1;
                 
                    flips--;
                    Session["POFlips"] = flips;
                    //if(screenName== "POStatusWithChart")
                    //{
                    //    displayCharts();
                    //    headerName.InnerText = "Production Status";
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "bind", "bindCharts()", true);
                    //}
                    if (inputToBindData == "charts")
                    {
                        displayCharts();
                        headerName.InnerText = "Production Status";
                        ScriptManager.RegisterStartupScript(this, GetType(), "bind", "bindCharts()", true);
                    }
                    else
                    {
                        displayLaggingAndLeadingMachine();
                    }
                    decimal.TryParse(Session["RefreshInterval"].ToString(), out refreshData);
                    Session["RefreshInterval"] = refreshData - 1;
                }
                else if (Convert.ToDecimal(Session["RefreshInterval"].ToString()) > 0)
                {
                    //if (refreshScreenName == "POStatusWithChart")
                    //{
                        BindPOData("charts");
                        refreshScreenName = "POStatusWithChart";
                    //}
                    //else if (refreshScreenName == "postatuswithLeadLag")
                    //{
                    //    BindPOData("charts");
                    //    refreshScreenName = "POStatusWithChart";
                    //}
                }
                else if (screenName == "POStatusWithChart" || Convert.ToDecimal(Session["RefreshInterval"].ToString()) <= 0)
                {
                    displayImageVideoSlider();
                    headerName.InnerText = "Images / Videos";
                    bindImageVideo();
                }
                else
                {
                    screenName = "POStatusWithChart";
                    refreshScreenName = "POStatusWithChart";
                    BindPOData("charts");
                    //Response.Redirect("POStatusWithChart.aspx");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Timer tick event " + ex.Message);
            }
        }

        private PODetails getPODetails(string machine, string status, string print, string setting, string alram, string user, string ae, string aeback, string pe, string peback, string oee, string oeeback, string plan, string act)
        {
            PODetails po1 = new PODetails();
            po1.Machine = machine;
            po1.Status = status;
            po1.Component = print;
            po1.Setting = setting;
            po1.Alaram = alram;
            po1.User = user;
            po1.AE = ae;
            po1.AEBackColor = aeback;
            po1.PE = pe;
            po1.PEBackColor = peback;
            po1.OEE = oee;
            po1.OEEBackColor = oeeback;
            po1.Plan = plan;
            po1.Act = act;
            return po1;
        }

        private LeadingMachine getLeadingMachine(string item, string turning, string turnmill)
        {
            LeadingMachine data = new LeadingMachine();
            return data;
        }
       
       
        private LaggingMachine getLaggingMachine(string item, string turning, string turnmill)
        {
            LaggingMachine data = new LaggingMachine();
            data.Item = item;
            data.IG1500 = turning;
            data.Turnmill02 = turnmill;
            return data;
        }


        public void bindImageVideo()
        {
            cache.Remove("CarouselInnerHtml");
            cache.Remove("CarouselIndicatorsHtml");
            if (Convert.ToInt32(Session["showVideo"].ToString()) == 0 && Convert.ToInt32(Session["showImages"].ToString()) == 0)
            {
                btnPost_Click(null, null);
            }
            else
            {
                BindSliderImage();
            }

        }
        private void BindSliderImage()
        {

            try
            {
                if (Session["showImages"] == null)
                {
                    Session["showImages"] = 1;
                }
                if (Session["showVideo"] == null)
                {
                    Session["showVideo"] = 1;
                }
                int im = Convert.ToInt32(Session["showImages"].ToString());
                int vi = Convert.ToInt32(Session["showVideo"].ToString());
                if (cache["CarouselInnerHtml"] != null && cache["CarouselIndicatorsHtml"] != null)
                {
                    //use the cached html
                    ltlCarouselImages.Text = cache["CarouselInnerHtml"].ToString();
                    ltlCarouselIndicators.Text = cache["CarouselIndicatorsHtml"].ToString();
                }
                else
                {
                    //get a list of images from the folder
                    const string imagesPath = "ImageVideoSilder/";// "~/Image/Slideshow/";
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

                                if ((result == ".mp4") || (result == ".wmv") || (result == ".avi") || (result == ".mov") || (result == ".qt") || (result == ".yuv") || (result == ".mkv") ||
                                    (result == ".webm") || (result == ".flv") || (result == ".ogg"))
                                {
                                    //carouselInnerHtml.AppendFormat("<video  class='slide-image embed-responsive-item center-block makeStyle' id='v{0}' alt='Slide #{0}' playsinline='playsinline' controls autoplay>\r\n", i + 1);
                                    if (Convert.ToInt32(Session["showVideo"].ToString()) == 1)
                                    {
                                        if (carouselInnerHtml.ToString().Contains("active"))
                                        {
                                            carouselInnerHtml.AppendLine("<div class='item'>");
                                        }
                                        else
                                        {
                                            carouselInnerHtml.AppendLine("<div class='item active'>");
                                        }
                                        //carouselInnerHtml.AppendLine(i == 0 ? "<div class='item active'>" : "<div class='item'>");
                                        carouselInnerHtml.AppendFormat("<video  class='slide-image embed-responsive-item center-block makeStyle' id='v{0}' alt='Slide #{0}' playsinline='playsinline' muted='muted' autoplay='autoplay' controls>\r\n", i + 1);
                                        carouselInnerHtml.AppendFormat("<source src='{0}' type='video/mp4'>\r\n", imagesPath + fileNames[i]);
                                        carouselInnerHtml.AppendLine("</video>");
                                        carouselInnerHtml.AppendLine("</div>");
                                        indicatorsHtml.AppendLine(i == 0 ? @"<li data-target='#myCarousel' data-slide-to='" + i + "' class='active'></li>" : @"<li data-target='#myCarousel' data-slide-to='" + i + "' class=''></li>");
                                    }
                                }
                                else
                                {
                                    //img-fluid img-thumbnail
                                    if (Convert.ToInt32(Session["showImages"].ToString()) == 1)
                                    {
                                        if (carouselInnerHtml.ToString().Contains("active"))
                                        {
                                            carouselInnerHtml.AppendLine("<div class='item'>");
                                        }
                                        else
                                        {
                                            carouselInnerHtml.AppendLine("<div class='item active'>");
                                        }
                                        // carouselInnerHtml.AppendLine(i == 0 ? "<div class='item active'>" : "<div class='item'>");
                                        carouselInnerHtml.AppendLine("<img class='img-responsive img-rounded center-block makeStyle' src='" + imagesPath + fileNames[i] + "' alt='Slide #" + (i + 1) + "'>");
                                        carouselInnerHtml.AppendLine("</div>");
                                        indicatorsHtml.AppendLine(i == 0 ? @"<li data-target='#myCarousel' data-slide-to='" + i + "' class='active'></li>" : @"<li data-target='#myCarousel' data-slide-to='" + i + "' class=''></li>");
                                    }
                                }

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
                    else
                    {
                        btnPost_Click(null, null);
                    }
                }
            }
            catch (Exception)
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
                //ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<int> getFontSize()
        {
            List<int> size = new List<int>();
            DataTable dt = AdvikDatabaseAccess.getSettingDetails();
            if (dt.Rows.Count > 0)
            {
                var header = dt.AsEnumerable().Where(x => x.Field<string>("ValueInText2") == "Header" && x.Field<string>("ValueInText") == "FontSize").Select(x => x.Field<int>("ValueInInt")).ToList();
                size.Add(Convert.ToInt32(header[0]));
                var content = dt.AsEnumerable().Where(x => x.Field<string>("ValueInText2") == "Content" && x.Field<string>("ValueInText") == "FontSize").Select(x => x.Field<int>("ValueInInt")).ToList();
                size.Add(Convert.ToInt32(content[0]));
            }
            return size;
        }

        
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static int getNumberOfImagesVideo()
        {
            //DirectoryInfo diInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ImageVideoSilder/"));
            int count = 0;
            return count;
        }
       
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static ChartDta getChartData(string plant)
        {
            ChartDta chartdata = new ChartDta();
            ChartSeries chartSeries = new ChartSeries();
            ObservableCollection<OEEData> downpantOeedata = new ObservableCollection<OEEData>();
            TimeSpan ts = new TimeSpan(0, 0, 300);
            if (HttpContext.Current.Cache["AdvikchartDetails"] == null)
            {
                try
                {
                    List<DataSeries> series = new List<DataSeries>();
                    DataSeries LineSeries = new DataSeries();
                    DataSeries ColumnSeries = new DataSeries();
                    double[] lineData;
                    string[] category;
                    List<double[]> columnData = new List<double[]>();
                    chartSeries.Title = "Down Time Report";
                    ObservableCollection<DownTimeParetoDATAModel> DownTimeParetoData = new ObservableCollection<DownTimeParetoDATAModel>();
                    DownTimeParetoData = AdvikDatabaseAccess.GetChartsData("", plant, "", out downpantOeedata);
                    chartdata.OEEData = downpantOeedata;
                    LineSeries.type = "pareto";
                    LineSeries.name = "DownTime";
                    LineSeries.color = "#86ba35";
                    LineSeries.yAxis = 1;
                    LineSeries.zIndex = 10;
                    LineSeries.baseSeries = 1;
                    ColumnSeries.type = "column";
                    ColumnSeries.name = "DownTime";
                    ColumnSeries.color = "#1ba1e2";
                   // ColumnSeries.yAxis = 0;
                    ColumnSeries.zIndex = 2;
                    if (DownTimeParetoData.Count > 0)
                    {
                        lineData = new double[DownTimeParetoData.Count];
                        category = new string[DownTimeParetoData.Count];
                        for (int i = 0; i < DownTimeParetoData.Count; i++)
                        {
                            lineData[i] = DownTimeParetoData[i].YValue;
                            category[i] = DownTimeParetoData[i].XValue;
                        }
                        Marker lineMark = new Marker();
                        lineMark.radius = 5;
                        chartSeries.Category = category;
                       // LineSeries.data = lineData;
                       // LineSeries.marker = lineMark;
                        ColumnSeries.data = lineData;
                    }
                    else
                    {
                        lineData = new double[1] { 0 };
                        category = new string[1] { "" };
                        Marker lineMark = new Marker();
                        lineMark.radius = 0;
                        chartSeries.Category = category;
                      //  LineSeries.data = lineData;
                     //   LineSeries.marker = lineMark;
                        ColumnSeries.data = lineData;
                    }
                    series.Add(LineSeries);
                    series.Add(ColumnSeries);
                    chartSeries.series = series;
                    chartdata.DownTimeData = chartSeries;
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog("While getting  down time chart data " + ex.Message);
                }
                HttpContext.Current.Cache["AdvikchartDetails"] = chartdata;
                HttpContext.Current.Cache.Insert("AdvikchartDetails", chartdata, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);
            }
            else
            {
                chartdata = (ChartDta)HttpContext.Current.Cache["AdvikchartDetails"];
            }
            return chartdata;
        }

        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            //bindCellID();
            Cache.Remove("AdvikPOData");
            Cache.Remove("AdvikLeadingDetails");
            Cache.Remove("AdvikLaggingDetails");
            Cache.Remove("AdvikchartDetails");
            poInterval.Enabled = false;
            imgBtnSwitch.ToolTip = "Switch to ANDON Mode";
            imgBtnSwitch_Click(null, null);
           // setCompanyLogo();
            //if (imgBtnSwitch.ToolTip == "Switch to ANDON Mode")
            //{
            //    //BindPOData("LeadLag");
            //    ////  BindLeadingMachine();
            //    //// BindLaggingMachine();
            //    //BindLeadingLaggingMachine();
            //    //displayLaggingAndLeadingMachine();
            //    bindPOWithLeadLag();
            //}
            //else
            //{
            //    bindPOWithLeadLag();
            //    poInterval.Enabled = false;
            //}
        }

        protected void btnSwitch_Click(object sender, EventArgs e)
        {
            
        }

        protected void imgBtnSwitch_Click(object sender, ImageClickEventArgs e)
        {
            if (imgBtnSwitch.ToolTip == "Switch to ANDON Mode")
            {
                bindPOWithLeadLag();
                imgBtnSwitch.ToolTip = "Switch to DESKTOP Mode";
                imgBtnSwitch.ImageUrl = "Images/desktop1.png";
            }
            else
            {
                //  BindPOData("LeadLag");
                bindPOWithLeadLag();
                poInterval.Enabled = false;
                imgBtnSwitch.ToolTip = "Switch to ANDON Mode";
                imgBtnSwitch.ImageUrl = "Images/andon.jpg";
            }
        }
        public void setchartsDataToCache()
        {
            TimeSpan ts = new TimeSpan(0, 0, 300);
            #region ---------- chart data ---------------
            ChartDta chartdata = new ChartDta();
            ChartSeries chartSeries = new ChartSeries();
            ObservableCollection<OEEData> downpantOeedata = new ObservableCollection<OEEData>();
            try
            {
                List<DataSeries> series = new List<DataSeries>();
                DataSeries LineSeries = new DataSeries();
                DataSeries ColumnSeries = new DataSeries();
                double[] lineData;
                string[] category;
                List<double[]> columnData = new List<double[]>();
                chartSeries.Title = "Down Time Report";
                ObservableCollection<DownTimeParetoDATAModel> DownTimeParetoData = new ObservableCollection<DownTimeParetoDATAModel>();
                DownTimeParetoData = AdvikDatabaseAccess.GetChartsData("", ddlPlantID.SelectedValue == null ? "" : ddlPlantID.SelectedValue, "", out downpantOeedata);
                chartdata.OEEData = downpantOeedata;
                LineSeries.type = "pareto";
                LineSeries.name = "DownTime";
                LineSeries.color = "#86ba35";
                LineSeries.yAxis = 1;
                LineSeries.zIndex = 10;
                LineSeries.baseSeries = 1;
                ColumnSeries.type = "column";
                ColumnSeries.name = "DownTime";
                ColumnSeries.color = "#1ba1e2";
                // ColumnSeries.yAxis = 0;
                ColumnSeries.zIndex = 2;
                if (DownTimeParetoData.Count > 0)
                {
                    lineData = new double[DownTimeParetoData.Count];
                    category = new string[DownTimeParetoData.Count];
                    for (int i = 0; i < DownTimeParetoData.Count; i++)
                    {
                        lineData[i] = DownTimeParetoData[i].YValue;
                        category[i] = DownTimeParetoData[i].XValue;
                    }
                    Marker lineMark = new Marker();
                    lineMark.radius = 5;
                    chartSeries.Category = category;
                    // LineSeries.data = lineData;
                    // LineSeries.marker = lineMark;
                    ColumnSeries.data = lineData;
                }
                else
                {
                    lineData = new double[1] { 0 };
                    category = new string[1] { "" };
                    Marker lineMark = new Marker();
                    lineMark.radius = 0;
                    chartSeries.Category = category;
                    //  LineSeries.data = lineData;
                    //   LineSeries.marker = lineMark;
                    ColumnSeries.data = lineData;
                }
                series.Add(LineSeries);
                series.Add(ColumnSeries);
                chartSeries.series = series;
                chartdata.DownTimeData = chartSeries;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While getting  down time chart data " + ex.Message);
            }
            HttpContext.Current.Cache["AdvikchartDetails"] = chartdata;
            HttpContext.Current.Cache.Insert("AdvikchartDetails", chartdata, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);

            #endregion
        }
     
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void setNoOfRowsToSession(int rows, int rowsLength)
        {
            HttpContext.Current.Session["Rows"] = rows;
            int flips = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(rowsLength) / rows));
            HttpContext.Current.Session["POFlips"] = flips;
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            bindPOWithLeadLag();
        }

        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cache.Remove("AdvikPOData");
            //Cache.Remove("AdvikLeadingDetails");
            //Cache.Remove("AdvikLaggingDetails");
            //Cache.Remove("AdvikchartDetails");
            //poInterval.Enabled = false;
            //imgBtnSwitch.ToolTip = "Switch to ANDON Mode";
            //imgBtnSwitch_Click(null, null);
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void setScreenName()
        {
            screenName = "POStatusWithChart";
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static  void addDataToCacheMemory(string plant)
        {
            var hhtp = HttpContext.Current;
            System.Web.Hosting.HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                // return ThumbnailHelper.CreateThumbnail(plant);
                return Task.Factory.StartNew(() =>
                {
                    try
                    {
                        //HttpContext.Current = (HttpContext)hhtp;

                        TimeSpan ts = new TimeSpan(0, 0, 300);
                        List<PODetails> listPOData1 = new List<PODetails>();
                        listPOData1 = AdvikDatabaseAccess.gridMachineStatusLoad("", plant, "1st Screen");
                        // HttpContext.Current.Session["SPOData"] = listPOData1;
                        hhtp.Cache["AdvikPOData"] = listPOData1;
                        hhtp.Cache.Insert("AdvikPOData", listPOData1, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);

                        #region ---------- chart data ---------------
                         ChartDta chartdata = new ChartDta();
                        ChartSeries chartSeries = new ChartSeries();
                        ObservableCollection<OEEData> downpantOeedata = new ObservableCollection<OEEData>();
                        try
                        {
                            List<DataSeries> series = new List<DataSeries>();
                            DataSeries LineSeries = new DataSeries();
                            DataSeries ColumnSeries = new DataSeries();
                            double[] lineData;
                            string[] category;
                            List<double[]> columnData = new List<double[]>();
                            chartSeries.Title = "Down Time Report";
                            ObservableCollection<DownTimeParetoDATAModel> DownTimeParetoData = new ObservableCollection<DownTimeParetoDATAModel>();
                            DownTimeParetoData =AdvikDatabaseAccess.GetChartsData("", plant, "", out downpantOeedata);
                            chartdata.OEEData = downpantOeedata;
                            LineSeries.type = "pareto";
                            LineSeries.name = "DownTime";
                            LineSeries.color = "#86ba35";
                            LineSeries.yAxis = 1;
                            LineSeries.zIndex = 10;
                            LineSeries.baseSeries = 1;
                            ColumnSeries.type = "column";
                            ColumnSeries.name = "DownTime";
                            ColumnSeries.color = "#1ba1e2";
                            // ColumnSeries.yAxis = 0;
                            ColumnSeries.zIndex = 2;
                            if (DownTimeParetoData.Count > 0)
                            {
                                lineData = new double[DownTimeParetoData.Count];
                                category = new string[DownTimeParetoData.Count];
                                for (int i = 0; i < DownTimeParetoData.Count; i++)
                                {
                                    lineData[i] = DownTimeParetoData[i].YValue;
                                    category[i] = DownTimeParetoData[i].XValue;
                                }
                                Marker lineMark = new Marker();
                                lineMark.radius = 5;
                                chartSeries.Category = category;
                                // LineSeries.data = lineData;
                                // LineSeries.marker = lineMark;
                                ColumnSeries.data = lineData;
                            }
                            else
                            {
                                lineData = new double[1] { 0 };
                                category = new string[1] { "" };
                                Marker lineMark = new Marker();
                                lineMark.radius = 0;
                                chartSeries.Category = category;
                                //  LineSeries.data = lineData;
                                //   LineSeries.marker = lineMark;
                                ColumnSeries.data = lineData;
                            }
                            series.Add(LineSeries);
                            series.Add(ColumnSeries);
                            chartSeries.series = series;
                            chartdata.DownTimeData = chartSeries;
                        }
                        catch (Exception ex)
                        {
                            //Logger.WriteErrorLog("While getting  down time chart data " + ex.Message);
                        }
                        hhtp.Cache["AdvikchartDetails"] = chartdata;
                        hhtp.Cache.Insert("AdvikchartDetails", chartdata, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);



                        #endregion
                     }
                    catch (Exception ex)
                    {
                       // Logger.WriteErrorLog(ex.Message);
                    }
                });

               // return ttak;
            });

        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static long getCacheIntervalValue()
        {
            DataTable dtSetting = AdvikDatabaseAccess.getSettingDetails();
            int refreshInt = 0;
            if (dtSetting.Rows.Count > 0)
            {
                refreshInt = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "DataRefreshInterval").Select(rows => rows.Field<int>("ValueInInt")).ToList()[0];
            }
          
            return refreshInt * 1000;
        }
    }
}