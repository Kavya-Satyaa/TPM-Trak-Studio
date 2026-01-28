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
using Web_TPMTrakDashboard.GEA.Andon_GEA.Model;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;

namespace Web_TPMTrakDashboard.GEA.Andon_GEA
{
    public partial class GEA : System.Web.UI.Page
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
            // Interval = Utility.ProductionStatusInterval;
            if (!IsPostBack)
            {
                bindPlantID();
                bindPOWithLeadLag();
                poInterval.Enabled = false;
            }
        }
        public void bindPOWithLeadLag()
        {
            screenName = "postatuswithLeadLag";
            refreshScreenName = "postatuswithLeadLag";

            DataTable dtSetting = GEADatabaseAccess.getSettingDetails();
            spanWelcome.InnerText = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "ScrollingText").Select(rows => rows.Field<string>("ValueInText2")).ToList()[0];
            headerName.InnerText = "Production Status";

            Interval = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "FlipInterval").Select(rows => rows.Field<int>("ValueInInt")).ToList()[0];
            int refreshInt = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "RefreshInterval").Select(rows => rows.Field<int>("ValueInInt")).ToList()[0];
            decimal refreshTick = Math.Ceiling(refreshInt / Convert.ToDecimal(Interval));
            Session["RefreshInterval"] = refreshTick;

            BindPOData("LeadLag");
            //  BindLeadingLaggingMachine();
            BindLeadingMachine();
            BindLaggingMachine();
            displayLaggingAndLeadingMachine();
            poInterval.Enabled = true;
            poInterval.Interval = Interval * 1000;

        }
        public void bindPlantID()
        {
            List<string> list = GEADatabaseAccess.getPlantID();
            ddlPlantID.DataSource = list;
            ddlPlantID.DataBind();
            //ddlPlantID.Items.Insert(0, new ListItem("All", "All"));
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
                    }
                }

            }
            //for (int i = 0; i < tbl.Rows.Count; i++)
            //{
            //    if (tbl.Rows[i]["ValueInBool"].ToString() == "0")
            //    {
            //        (listview.FindControl("th" + tbl.Rows[i]["LanguageSpecified"].ToString()) as HtmlControl).Visible = false;
            //        for (int j = 0; j < listview.Items.Count; j++)
            //        {
            //            (listview.Items[j].FindControl("td" + tbl.Rows[i]["LanguageSpecified"].ToString()) as HtmlControl).Visible = false;
            //        }
            //    }

            //}
        }
        void BindPOData(string param)
        {
            try
            {
                inputToBindData = param;
                listPOData = GEADatabaseAccess.gridMachineStatusLoad("", ddlPlantID.SelectedValue == null ? "" : ddlPlantID.SelectedValue, "1st Screen");

                noOfRows = 5;
                if (listPOData != null && listPOData.Count > 0)
                {
                    DataTable dtSetting = GEADatabaseAccess.getSettingDetails();
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


                    //DataTable result = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "POColumn").CopyToDataTable();
                    //showHideListviewColumn(lvPOData, result);
                    List<AndonSettingData> result = GEADatabaseAccess.getColumnData();
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

                    DataTable dtSetting = GEADatabaseAccess.getSettingDetails();
                    //DataTable result = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "POColumn").CopyToDataTable();
                    //showHideListviewColumn(lvPOData, result);
                    List<AndonSettingData> result = GEADatabaseAccess.getColumnData();
                    showHideListviewColumn(lvPOData, result);

                    Session["NoOfSkipRows"] = Convert.ToInt32(Session["NoOfSkipRows"].ToString()) + 1;
                    //spanWelcome.InnerText = Session["NoOfSkipRows"].ToString() + ".Welcome to .......";
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
                    if (refreshScreenName == "POStatusWithChart")
                    {
                        BindPOData("LeadLag");
                        refreshScreenName = "postatuswithLeadLag";
                    }
                    else if (refreshScreenName == "postatuswithLeadLag")
                    {
                        BindPOData("charts");
                        refreshScreenName = "POStatusWithChart";
                    }
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

        void BindLeadingMachine()
        {
            try
            {
                List<LeadingMachine> listData = new List<LeadingMachine>();
                listData = GEADatabaseAccess.gridLeadingmachinesLoad("", ddlPlantID.SelectedValue == null ? "" : ddlPlantID.SelectedValue, "1st Screen");
                lvleadingMachine.DataSource = listData;
                lvleadingMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While Binding Leading Machine Details " + ex.Message);
            }
        }
        private LeadingMachine getLeadingMachine(string item, string turning, string turnmill)
        {
            LeadingMachine data = new LeadingMachine();
            return data;
        }
        void BindLaggingMachine()
        {
            try
            {
                List<LeadingMachine> listData = new List<LeadingMachine>();
                listData = GEADatabaseAccess.gridLaggingmachinesLoad("", ddlPlantID.SelectedValue == null ? "" : ddlPlantID.SelectedValue, "1st Screen");
                lvLaggingMachine.DataSource = listData;
                lvLaggingMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While Binding Lagging Machine Details " + ex.Message);
            }
        }
        private void BindLeadingLaggingMachine()
        {
            try
            {
                List<LeadingMachine> listData = new List<LeadingMachine>();
                List<LeadingMachine> listLaggingData;
                listData = GEADatabaseAccess.gridLeadingLaggingMachinesLoad("", ddlPlantID.SelectedValue == null ? "" : ddlPlantID.SelectedValue, "1st Screen", out listLaggingData);
                lvleadingMachine.DataSource = listData;
                lvleadingMachine.DataBind();
                lvLaggingMachine.DataSource = listLaggingData;
                lvLaggingMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While Binding Leading Machine Details " + ex.Message);
            }
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
            DataTable dt = GEADatabaseAccess.getSettingDetails();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["ValueInText2"].ToString() == "Header" && dt.Rows[i]["ValueInText"].ToString() == "FontSize")
                {
                    size.Add(Convert.ToInt32(dt.Rows[i]["ValueInInt"].ToString()));
                }
                if (dt.Rows[i]["ValueInText2"].ToString() == "Content" && dt.Rows[i]["ValueInText"].ToString() == "FontSize")
                {
                    size.Add(Convert.ToInt32(dt.Rows[i]["ValueInInt"].ToString()));
                }
            }
            return size;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static ObservableCollection<OEEData> getOEEChartData(string plant)
        {
            ObservableCollection<OEEData> listData = new ObservableCollection<OEEData>();
            listData = Utility.ShiftOrDaywise == 1 ? GEADatabaseAccess.GetOEEData("", plant, "3rd Screen") : GEADatabaseAccess.GetOEEData("", plant, "3rd Screen");
            return listData;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static int getNumberOfImagesVideo()
        {
            //DirectoryInfo diInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ImageVideoSilder/"));
            int count =0;
            return count;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static ChartSeries getDownTimeChartData(string plant)
        {
            ChartSeries chartSeries = new ChartSeries();
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
                DownTimeParetoData = Utility.ShiftOrDaywise == 1 ? GEADatabaseAccess.GetDownParetoData("", plant, "3rd Screen") : GEADatabaseAccess.GetDownParetoData("", plant, "3rd Screen");
                LineSeries.type = "spline";
                LineSeries.name = "DownTime";
                LineSeries.color = "#86ba35";
                LineSeries.yAxis = 1;
                LineSeries.zIndex = 1;
                ColumnSeries.type = "column";
                ColumnSeries.name = "DownTime";
                ColumnSeries.color = "#1ba1e2";
                ColumnSeries.yAxis = 0;
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
                    LineSeries.data = lineData;
                    LineSeries.marker = lineMark;
                    ColumnSeries.data = lineData;
                }
                else
                {
                    lineData = new double[1] { 0 };
                    category = new string[1] { "" };
                    Marker lineMark = new Marker();
                    lineMark.radius = 0;
                    chartSeries.Category = category;
                    LineSeries.data = lineData;
                    LineSeries.marker = lineMark;
                    ColumnSeries.data = lineData;
                }
                series.Add(LineSeries);
                series.Add(ColumnSeries);
                chartSeries.series = series;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While getting  down time chart data " + ex.Message);
            }
            return chartSeries;
        }

        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (imgBtnSwitch.ToolTip == "Switch to ANDON Mode")
            {
                BindPOData("LeadLag");
                BindLeadingMachine();
                BindLaggingMachine();
                //BindLeadingLaggingMachine();
                displayLaggingAndLeadingMachine();
                poInterval.Enabled = false;
            }
            else
            {
                bindPOWithLeadLag();
            }
        }

        protected void btnSwitch_Click(object sender, EventArgs e)
        {
            //if (btnSwitch.Text == "Switch to ANDON Mode")
            //{
            //    bindPOWithLeadLag();
            //    btnSwitch.Text = "Switch to DESKTOP Mode";
            //}
            //else
            //{
            //    BindPOData("LeadLag");
            //    BindLeadingMachine();
            //    BindLaggingMachine();
            //    displayLaggingAndLeadingMachine();
            //    poInterval.Enabled = false;
            //    btnSwitch.Text = "Switch to ANDON Mode";
            //}
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
                BindPOData("LeadLag");
                BindLeadingMachine();
                BindLaggingMachine();
                // BindLeadingLaggingMachine();
                displayLaggingAndLeadingMachine();
                poInterval.Enabled = false;
                imgBtnSwitch.ToolTip = "Switch to ANDON Mode";
                imgBtnSwitch.ImageUrl = "Images/andon.jpg";
            }
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

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void setScreenName()
        {
            screenName = "postatuswithLeadLag";
        }
    }
}