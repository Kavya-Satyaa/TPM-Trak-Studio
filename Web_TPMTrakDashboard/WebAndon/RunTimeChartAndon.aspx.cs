using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.WebAndon
{
    public partial class RunTimeChartAndon : System.Web.UI.Page
    {
        public string mode = string.Empty, PlantID = string.Empty, Date = string.Empty, Shift = string.Empty, LineID = string.Empty;
        public SettingsGUI settings = null;
        public int TimerVal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ConnectionString"] == null)
            {

            }
            else
            {
                try
                {
                    settings = new SettingsGUI();
                    DropDownList drpPlantName = (DropDownList)Page.Master.FindControl("ddlPlantName");
                    TextBox txtDate = (TextBox)Page.Master.FindControl("txtDate");
                    DropDownList drpLine = (DropDownList)Page.Master.FindControl("ddlLine");
                    //DropDownList drpShift = (DropDownList)Page.Master.FindControl("ddlShift");
                    Button btnProcess = (Button)Page.Master.FindControl("btnProcess");
                    Button btnSwitchMode = (Button)Page.Master.FindControl("btnToggel");
                    mode = Session["Mode"].ToString();
                    PlantID = drpPlantName.SelectedValue.ToString();
                    Session["AndonDate"] = Date = txtDate.Text;
                    LineID = drpLine.SelectedValue.ToString();
                   // Shift = drpShift.SelectedValue.ToString();
                    btnProcess.Click += new EventHandler(btnProcess_Click);
                    btnSwitchMode.Click += new EventHandler(btnToggel_Click);
                    drpPlantName.SelectedIndexChanged += new EventHandler(drp_SelectedIndexChanged);
                    if(mode.Equals("ANDON",StringComparison.OrdinalIgnoreCase))
                    {
                        drpPlantName.Visible = true; drpLine.Visible = true; txtDate.Visible = false; //drpShift.Visible = false;btnProcess.Visible = false;
                        btnToggel_Click(null, null);
                    }
                    else if(mode.Equals("Desktop", StringComparison.OrdinalIgnoreCase))
                    {
                        drpPlantName.Visible = true; drpLine.Visible = true; txtDate.Visible = true; //drpShift.Visible = true; btnProcess.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.Message);
                }

            }
        }

        private void btnToggel_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnSwitchMode = (Button)Page.Master.FindControl("btnToggel");
                DropDownList drpPlantName = (DropDownList)Page.Master.FindControl("ddlPlantName");
                TextBox txtDate = (TextBox)Page.Master.FindControl("txtDate");
                DropDownList drpLine = (DropDownList)Page.Master.FindControl("ddlLine");
                DropDownList drpShift = (DropDownList)Page.Master.FindControl("ddlShift");
                Date = txtDate.Text;
                string Type = btnSwitchMode.Text;
                if (drpPlantName != null && txtDate != null && drpLine != null && drpShift != null)
                {
                    if (Type.Equals("DESKTOP", StringComparison.OrdinalIgnoreCase))
                    {
                        DateTime fromdate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(Date));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "BindChart", "BindChart()", true);
                    }
                    else
                    {
                        //List<RuntimeDataEntity> Data = new List<RuntimeDataEntity>();
                        DateTime fromdate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        DateTime todate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        //Data = CockpitDataBaseAccess.GetRuntimechartData(fromdate, todate, Shift, PlantID, "", "");
                        Session["TimerVal"] = 0;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "BindChart", "BindTimerChart()", true);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        void drp_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList drp = (DropDownList)Page.Master.FindControl("ddlPlantName");
                DropDownList drpLine = (DropDownList)Page.Master.FindControl("ddlLine");
                if (drp != null && drpLine != null)
                {
                    Session["PlantId"] = drp.SelectedValue.ToString();
                    Session["LineID"] = drpLine.SelectedValue.ToString();
                    hdfGroupId.Value = drpLine.SelectedValue.ToString();
                }

                BindChart();

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void BindChart()
        {
            Session["TimerVal"] = 0;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "BindChart", "BindChart()", true);
        }

        void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                DropDownList drpPlantName = (DropDownList)Page.Master.FindControl("ddlPlantName");
                DropDownList drpLine = (DropDownList)Page.Master.FindControl("ddlLine");
                if (drpPlantName != null)
                {
                    Session["PlantId"] = drpPlantName.SelectedValue.ToString();
                    Session["LineID"] = drpLine.SelectedValue.ToString();
                    hdfGroupId.Value = drpLine.SelectedValue.ToString();
                }
                //BindChart();

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static RunTimeChartEntity GetRuntimeData(string PlantID, string Date, string Shift, string LineID, int Height)
        {
            List<RuntimeDataEntity> Data = new List<RuntimeDataEntity>();
            RunTimeChartEntity Chart = new RunTimeChartEntity();
            Chart.series = new Series();
            Series series = new Series();
            series.data = new List<data>();
            data seriesdata = new data();
            List<data> seriesdatalist = new List<data>();
            Chart.category = new List<string>();
            try
            {
                Date = Date == "" ? HttpContext.Current.Session["AndonDate"].ToString() : Date;
                Date = Date == "" ? DateTime.Now.ToString() : Date;
                DateTime fromdate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(Date));
                DateTime todate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(Date));
                int MaxVal = 0;
                Data = CockpitDataBaseAccess.GetRuntimechartData(fromdate, todate, Shift, PlantID, "", "",out MaxVal);
                
                if (Data != null && Data.Count > 0)
                {
                    int rows = (Convert.ToInt16(Height) / MaxVal);
                    int paging = MaxVal / rows;
                    int startindex = 0; int Endindex = rows;
                    Data = Data.Where(x => x.MachineID >= startindex).ToList();
                    Data = Data.Where(x => x.MachineID < Endindex).ToList();
                    if (Data != null && Data.Count > 0)
                    {
                        string MachineID = Data[0].MachineName.ToString(); Chart.category.Add(MachineID);
                        foreach (RuntimeDataEntity Entity in Data)
                        {
                            seriesdata = new data();
                            if (!(MachineID == Entity.MachineName))
                            {
                                MachineID = Entity.MachineName;
                                Chart.category.Add(MachineID);
                            }
                            seriesdata.y = Entity.MachineID;
                            seriesdata.x = (double)(Entity.StartDate - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                            seriesdata.x2 = (double)(Entity.EndDate - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                            seriesdata.color = Entity.Color.ToLower();
                            seriesdata.status = Entity.Status;
                            seriesdatalist.Add(seriesdata);
                        }
                        series.data = seriesdatalist;
                        Chart.series = series;
                        Chart.series.pointWidth = 70;
                        Chart.series.turboThreshold = seriesdatalist.Count;
                        Chart.series.borderColor = "gray";
                    }
                    Chart.MaxValue = paging;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return Chart;
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static RunTimeChartEntity GetRuntimeDatabyID(string PlantID, string Date, string Shift, string ID, int Height)
        {
            List<RuntimeDataEntity> Data = new List<RuntimeDataEntity>();
            int id = Convert.ToInt32(ID);
            RunTimeChartEntity Chart = new RunTimeChartEntity();
            Chart.series = new Series();
            Series series = new Series();
            series.data = new List<data>();
            data seriesdata = new data();
            List<data> seriesdatalist = new List<data>();
            Chart.category = new List<string>();
            try
            {
                DateTime fromdate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(Date));
                DateTime todate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(Date));int maxvalue = 0;
                Data = CockpitDataBaseAccess.GetRuntimechartData(fromdate, todate, Shift, PlantID, "", "",out maxvalue);
                
                if (Data != null && Data.Count > 0)
                {
                    int rows = (Convert.ToInt16(Height) / maxvalue);
                    int startindex = 0; int Endindex = rows;
                    if (id != 0)
                    {
                        startindex = ((id * (rows)) - 1);
                        Endindex = (startindex + rows);
                    }
                    Data = Data.Where(x => x.MachineID >= startindex).ToList();
                    Data = Data.Where(x => x.MachineID < Endindex).ToList();
                    int i = 0; string MachineID = Data[0].MachineName.ToString(); Chart.category.Add(MachineID);
                    foreach (RuntimeDataEntity Entity in Data)
                    {
                        seriesdata = new data();
                        if (!(MachineID == Entity.MachineName))
                        {
                            i++;
                            MachineID = Entity.MachineName;
                            Chart.category.Add(MachineID);
                        }
                        seriesdata.y = i;
                        seriesdata.x = (double)(Entity.StartDate - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        seriesdata.x2 = (double)(Entity.EndDate - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        seriesdata.color = Entity.Color.ToLower();
                        seriesdata.status = Entity.Status;
                        seriesdatalist.Add(seriesdata);
                    }
                    Chart.MaxValue = maxvalue;
                    series.data = seriesdatalist;
                    Chart.series = series;
                    Chart.series.pointWidth = 70;
                    Chart.series.turboThreshold = seriesdatalist.Count;
                    Chart.series.borderColor = "gray";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return Chart;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static RunTimeChartEntity GetRuntimeDataAndon(string PlantID, string Shift, string LineID, string Timerval, int Height)
        {
            List<RuntimeDataEntity> Data = new List<RuntimeDataEntity>();
            int id = HttpContext.Current.Session["TimervVal"]!=null? Convert.ToInt32(HttpContext.Current.Session["TimervVal"].ToString()): Convert.ToInt32(Timerval);
            
          
            RunTimeChartEntity Chart = new RunTimeChartEntity();
            Chart.series = new Series();
            Series series = new Series();
            series.data = new List<data>();
            data seriesdata = new data();
            List<data> seriesdatalist = new List<data>();
            Chart.category = new List<string>();
            try
            {
                DateTime fromdate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                DateTime todate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));int maxvalue = 0;
                Data = CockpitDataBaseAccess.GetRuntimechartData(fromdate, todate, Shift, PlantID, "", "",out maxvalue);
               
                if (Data != null && Data.Count > 0)
                {
                    int rows = (Convert.ToInt16(Height) / maxvalue);
                    int startindex = 0; int Endindex = rows;
                    if (id != 0)
                    {
                        startindex = ((id * (rows)) - 1);
                        Endindex = (startindex + rows);
                    }
                    int val = Data.Max(x => x.MachineID);
                    if (Endindex > val)
                    {
                        Endindex = val;
                        HttpContext.Current.Session["TimervVal"] = 0;
                    }
                    else
                        HttpContext.Current.Session["TimervVal"] = (Convert.ToInt32(Timerval) + 1);
                    Data = Data.Where(x => x.MachineID >= startindex).ToList();
                    Data = Data.Where(x => x.MachineID < Endindex).ToList();
                    if (Data != null && Data.Count > 0)
                    {
                        int i = 0; string MachineID = Data[0].MachineName.ToString(); Chart.category.Add(MachineID);
                        foreach (RuntimeDataEntity Entity in Data)
                        {
                            seriesdata = new data();
                            if (!(MachineID == Entity.MachineName))
                            {
                                i++;
                                MachineID = Entity.MachineName;
                                Chart.category.Add(MachineID);
                            }
                            seriesdata.y = i;
                            seriesdata.x = (double)(Entity.StartDate - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                            seriesdata.x2 = (double)(Entity.EndDate - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                            seriesdata.color = Entity.Color.ToLower();
                            seriesdata.status = Entity.Status;
                            seriesdatalist.Add(seriesdata);
                        }
                        series.data = seriesdatalist;
                        Chart.series = series;
                        Chart.series.pointWidth = 70;
                        Chart.series.turboThreshold = seriesdatalist.Count;
                        Chart.series.borderColor = "gray";
                        Chart.MaxValue = maxvalue;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return Chart;
        }


    }

    public class RuntimeDataEntity
    {
        public string MachineName { get; set; }
        public string Color { get; set; }
        public string Status { get; set; }
        public int MachineID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}