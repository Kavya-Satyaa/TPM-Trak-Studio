using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.MahindraAndMahindra
{
    public partial class SPC_PPGraphView : System.Web.UI.Page
    {
        public static List<DTO> processParamDashboardData = null;
        public string machineId = string.Empty;
        string FileName = string.Empty;
        public string parameterID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Request.QueryString["prm"] != null)
            //{
            //    string mid = Request.QueryString["prm"].ToString();
            //    char[] data = mid.ToCharArray();
            //    Base64Decoder myDecoder = new Base64Decoder(data);
            //    StringBuilder sb = new StringBuilder();
            //    byte[] temp = myDecoder.GetDecoded();
            //    sb.Append(Encoding.UTF8.GetChars(temp));
            //    machineId = sb.ToString();
            //}
            if (!IsPostBack)
            {
                if (Page.ClientQueryString.Length > 0)
                {
                    if (Request.QueryString["MachineID"] != null)
                    {
                        machineId = Request.QueryString["MachineID"].ToString();
                    }
                    if (Request.QueryString["ParameterID"] != null)
                    {
                        parameterID = Request.QueryString["ParameterID"].ToString();
                    }
                }
                Session["MnMParameterListGraphView"] = null;
                string shiftStart = Web_TPMTrakDashboard.Models.DataBaseAccess.GetCurrentShiftStart(out string endTime);
                txtFromDate.Text = Util.GetDateTime(shiftStart).ToString("yyyy-MM-dd HH:mm:ss");
                txtToDate.Text = Util.GetDateTime(endTime).ToString("yyyy-MM-dd HH:mm:ss");
                BindMachineID();
            }
        }

        private void BindMachineID()
        {
            try
            {
                List<string> MachineID = new List<string>();
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["AlliedPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    MachineID = DataBaseAccess.GetAllMachinesFromMaster();
                }
                else
                {
                    MachineID = VDGDataBaseAccess.GetMachinesbyPlantCell("", "");
                }
                if (machineId != null && MachineID.Count > 0)
                {
                    ddlMachineID.DataSource = MachineID;
                    ddlMachineID.DataBind();
                    if (machineId != "")
                    {
                        HelperClassGeneric.setDropdownValue(ddlMachineID, machineId);
                    }
                    else
                    {
                        HelperClassGeneric.setDropdownValue(ddlMachineID, "Diesel Generator");
                    }
                }

                BindProcessParameterData();
                //if (Parameters != null && Parameters.Count > 0)
                //{
                //    lstParameters.DataSource = Parameters;
                //    lstParameters.DataBind();
                //    int maxcoint = 4;
                //    for (int i = 0; i < lstParameters.Items.Count; i++)
                //    {
                //        if (i == maxcoint)
                //        {
                //            break;
                //        }
                //        else
                //        {
                //            lstParameters.Items[i].Selected = true;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindProcessParameterData()
        {
            try
            {
                Dictionary<string, string> Parameters = MandMDataBaseAccess.GetAllParameterIDsandName(ddlMachineID.SelectedValue);
                Session["MnMParameterListGraphView"] = Parameters;
                if (Parameters != null && Parameters.Count > 0)
                {
                    lstParameters.DataSource = Parameters;
                    lstParameters.DataTextField = "Value";
                    lstParameters.DataValueField = "Key";
                    lstParameters.DataBind();
                    if (machineId != "")
                    {
                        HelperClassGeneric.setListBoxValue(lstParameters, parameterID);
                    }
                    else
                    {
                        int maxcoint = 4;
                        for (int i = 0; i < lstParameters.Items.Count; i++)
                        {
                            if (i == maxcoint)
                            {
                                break;
                            }
                            else
                            {
                                lstParameters.Items[i].Selected = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        #region First Parameter Data
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ChartSeries GetFirstParameter(string Parameter, string MachineID, string StartDate, string EndDate, bool IsMarkerEnabled)
        {
            double[] Data = null;
            List<double[]> DataListData = new List<double[]>();
            ChartSeries Chartseries = new ChartSeries();
            DataSeries LineSeries = new DataSeries();
            PlotLines plotLineData = new PlotLines();
            Chartseries.PlotLines = new List<PlotLines>();
            Chartseries.Title = GetParameterDisplaytext(Parameter);

            Chartseries.CycleStartEnd = new List<CycleTimes>();
            LineSeries.tooltip = new Tooltipdata();
            ParameterEntity MainData = new ParameterEntity();

            try
            {
                DataTable ParameterData = new DataTable();
                //if (Parameter.Equals("SpindleVibration", StringComparison.OrdinalIgnoreCase) || Parameter.Equals("SpindleLoad", StringComparison.OrdinalIgnoreCase))
                //{
                //    ParameterData = MongoDBAccess.getParameterData(StartDate, EndDate, Parameter,MachineID);
                //}
                //else
                //{
                ParameterData = MandMDataBaseAccess.GetParameterisedData(MachineID, Parameter, StartDate, EndDate);
                //}
                DataTable CycleStart = MandMDataBaseAccess.GetParameterisedData(MachineID, "CycleStart", StartDate, EndDate);
                DataTable CycleEnd = MandMDataBaseAccess.GetParameterisedData(MachineID, "CycleEnd", StartDate, EndDate);
                if (IsMarkerEnabled)
                {
                    foreach (DataRow row in CycleStart.Rows)
                    {
                        CycleTimes cycledata = new CycleTimes();
                        plotLineData = new PlotLines();
                        plotLineLabelParam labelParam = new plotLineLabelParam();
                        labelParam.text = "Start";
                        labelParam.rotation = 90;
                        labelParam.align = "left";
                        labelParam.y = 10;
                        labelParam.x = 10;
                        plotLineData.color = "Green";
                        plotLineData.width = 2;
                        DateTime dt = DateTime.Now;
                        dt = Convert.ToDateTime(row["ParameterValue"]);
                        plotLineData.value = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        plotLineData.zIndex = 150;
                        plotLineData.label = labelParam;
                        Chartseries.PlotLines.Add(plotLineData);
                        cycledata.Start = row["UpdatedtimeStamp"].ToString();
                        Chartseries.CycleStartEnd.Add(cycledata);
                    }
                    foreach (DataRow row in CycleEnd.Rows)
                    {
                        CycleTimes cycledata = new CycleTimes();
                        plotLineData = new PlotLines();
                        plotLineLabelParam labelParam = new plotLineLabelParam();
                        labelParam.text = "End";
                        labelParam.rotation = -90;
                        labelParam.align = "right";
                        labelParam.y = 10;
                        labelParam.x = 10;
                        plotLineData.color = "Red";
                        plotLineData.width = 2;
                        DateTime dt = DateTime.Now;
                        dt = Convert.ToDateTime(row["UpdatedtimeStamp"]);
                        plotLineData.value = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        plotLineData.zIndex = 150;
                        plotLineData.label = labelParam;
                        Chartseries.PlotLines.Add(plotLineData);
                        cycledata.End = row["UpdatedtimeStamp"].ToString();
                        Chartseries.CycleStartEnd.Add(cycledata);
                    }
                }
                foreach (DataRow Row in ParameterData.Rows)
                {
                    Data = new double[2];
                    DateTime dt = DateTime.Now;
                    dt = Convert.ToDateTime(Row["UpdatedtimeStamp"].ToString());
                    Data[1] = double.Parse(Row["ParameterValue"].ToString());
                    Data[0] = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                    DataListData.Add(Data);
                }
                LineSeries.color = "Black";
                LineSeries.name = "Value";
                LineSeries.step = false;
                LineSeries.type = "line";
                LineSeries.data = DataListData;
                Marker mak = new Marker();
                mak.enabled = false;
                mak.radius = 5;
                mak.symbol = "circle";
                LineSeries.marker = mak;
                Chartseries.series = new List<DataSeries>();
                Chartseries.series.Add(LineSeries);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return Chartseries;
        }

        private static string GetParameterDisplaytext(string parameter)
        {
            //if (HttpContext.Current.Session["MnMParameterListGraphView"] == null)
            //{
            //    displayText = parameter;
            //}
            //else
            //{
            //    Dictionary<string, string> dictparam= (HttpContext.Current.Session["MnMParameterListGraphView"] as Dictionary<string, string>);
            //    displayText = dictparam[parameter];
            //}
            return HttpContext.Current.Session["MnMParameterListGraphView"] == null ? parameter : ((HttpContext.Current.Session["MnMParameterListGraphView"] as Dictionary<string, string>)[parameter]);
        }
        #endregion

        #region ----- Mongo Parameter Chart Data --------

        #endregion

        #region Second Parameter Data
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ChartSeries GetSecondParameter(string Parameter, string MachineID, string StartDate, string EndDate, bool IsMarkerEnabled)
        {
            #region Hardcoded
            //ParameterEntity Data = new ParameterEntity();
            //try
            //{
            //    DataTable ParameterData = MandMDataBaseAccess.GetParameterisedData(MachineID, Parameter, Util.GetDateTime(StartDate).ToString("yyyy-MM-dd HH:mm:ss"), Util.GetDateTime(EndDate).ToString("yyyy-MM-dd HH:mm:ss"));
            //    Data.Title = Parameter;
            //    Data.Catergory = new List<string>();
            //    Data.Data = new List<double>();
            //    //Data.Catergory = new List<string> { "12-12-2020", "13-12-2020", "14-12-2020", "15-12-2020", "16-12-2020" };
            //    //Data.Data = new List<double> { 10, 20, 30, 40, 50 };
            //    if (ParameterData.Rows.Count > 0)
            //    {
            //        foreach (DataRow row in ParameterData.Rows)
            //        {
            //            Data.Catergory.Add(row["UpdatedtimeStamp"].ToString());
            //            Data.Data.Add(Convert.ToDouble(row["ParameterValue"].ToString()));
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Logger.WriteErrorLog(ex.ToString());
            //}
            //return Data;
            #endregion
            double[] Data = null;
            List<double[]> DataListData = new List<double[]>();
            ChartSeries Chartseries = new ChartSeries();
            DataSeries LineSeries = new DataSeries();
            PlotLines plotLineData = new PlotLines();
            Chartseries.PlotLines = new List<PlotLines>();
            Chartseries.Title = GetParameterDisplaytext(Parameter);
            Chartseries.CycleStartEnd = new List<CycleTimes>();
            LineSeries.tooltip = new Tooltipdata();
            ParameterEntity MainData = new ParameterEntity();

            try
            {
                DataTable ParameterData = new DataTable();
                //if (Parameter.Equals("SpindleVibration", StringComparison.OrdinalIgnoreCase) || Parameter.Equals("SpindleLoad", StringComparison.OrdinalIgnoreCase))
                //{
                //    ParameterData = MongoDBAccess.getParameterData(StartDate, EndDate, Parameter, MachineID);
                //}
                //else
                //{
                ParameterData = MandMDataBaseAccess.GetParameterisedData(MachineID, Parameter, StartDate, EndDate);
                // }
                DataTable CycleStart = MandMDataBaseAccess.GetParameterisedData(MachineID, "CycleStart", StartDate, EndDate);
                DataTable CycleEnd = MandMDataBaseAccess.GetParameterisedData(MachineID, "CycleEnd", StartDate, EndDate);
                if (IsMarkerEnabled)
                {
                    foreach (DataRow row in CycleStart.Rows)
                    {
                        CycleTimes cycledata = new CycleTimes();
                        plotLineData = new PlotLines();
                        plotLineLabelParam labelParam = new plotLineLabelParam();
                        labelParam.text = "Start";
                        labelParam.rotation = 90;
                        labelParam.align = "left";
                        labelParam.y = 10;
                        labelParam.x = 10;
                        plotLineData.color = "Green";
                        plotLineData.width = 2;
                        DateTime dt = DateTime.Now;
                        dt = Convert.ToDateTime(row["ParameterValue"]);
                        plotLineData.value = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        plotLineData.zIndex = 150;
                        plotLineData.label = labelParam;
                        Chartseries.PlotLines.Add(plotLineData);
                        cycledata.Start = row["UpdatedtimeStamp"].ToString();
                        Chartseries.CycleStartEnd.Add(cycledata);
                    }
                    foreach (DataRow row in CycleEnd.Rows)
                    {
                        CycleTimes cycledata = new CycleTimes();
                        plotLineData = new PlotLines();
                        plotLineLabelParam labelParam = new plotLineLabelParam();
                        labelParam.text = "End";
                        labelParam.rotation = -90;
                        labelParam.align = "right";
                        labelParam.y = 10;
                        labelParam.x = 10;
                        plotLineData.color = "Red";
                        plotLineData.width = 2;
                        DateTime dt = DateTime.Now;
                        dt = Convert.ToDateTime(row["UpdatedtimeStamp"]);
                        plotLineData.value = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        plotLineData.zIndex = 150;
                        plotLineData.label = labelParam;
                        Chartseries.PlotLines.Add(plotLineData);
                        cycledata.End = row["UpdatedtimeStamp"].ToString();
                        Chartseries.CycleStartEnd.Add(cycledata);
                    }
                }
                foreach (DataRow Row in ParameterData.Rows)
                {
                    Data = new double[2];
                    DateTime dt = DateTime.Now;
                    dt =Convert.ToDateTime(Row["UpdatedtimeStamp"].ToString());
                    //Data[1] = double.Parse(Row["ParameterValue"].ToString());
                    Data[1] =DateTime.TryParse(Row["ParameterValue"].ToString(),out DateTime value)?value.ToOADate():double.Parse(Row["ParameterValue"].ToString());
                    Data[0] = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                    DataListData.Add(Data);
                }
                LineSeries.color = "Black";
                LineSeries.name = "Value";
                LineSeries.step = false;
                LineSeries.type = "line";
                LineSeries.data = DataListData;
                Marker mak = new Marker();
                mak.enabled = true;
                mak.radius = 5;
                mak.symbol = "circle";
                LineSeries.marker = mak;
                Chartseries.series = new List<DataSeries>();
                Chartseries.series.Add(LineSeries);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return Chartseries;
        }
        #endregion

        #region Third Parameter Data
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ChartSeries GetThirdParameter(string Parameter, string MachineID, string StartDate, string EndDate, bool IsMarkerEnabled)
        {
            #region Hardcoded
            //ParameterEntity Data = new ParameterEntity();
            //try
            //{
            //    DataTable ParameterData = MandMDataBaseAccess.GetParameterisedData(MachineID, Parameter, StartDate, EndDate);
            //    Data.Title = Parameter;
            //    Data.Catergory = new List<string>();
            //    Data.Data = new List<double>();
            //    if (ParameterData.Rows.Count > 0)
            //    {
            //        foreach (DataRow row in ParameterData.Rows)
            //        {
            //            Data.Catergory.Add(row["UpdatedtimeStamp"].ToString());
            //            Data.Data.Add(Convert.ToDouble(row["ParameterValue"].ToString()));
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Logger.WriteErrorLog(ex.ToString());
            //}

            //return Data;
            #endregion
            double[] Data = null;
            List<double[]> DataListData = new List<double[]>();
            ChartSeries Chartseries = new ChartSeries();
            DataSeries LineSeries = new DataSeries();
            PlotLines plotLineData = new PlotLines();
            Chartseries.PlotLines = new List<PlotLines>();
            Chartseries.Title = GetParameterDisplaytext(Parameter);
            Chartseries.CycleStartEnd = new List<CycleTimes>();
            LineSeries.tooltip = new Tooltipdata();
            ParameterEntity MainData = new ParameterEntity();
            try
            {
                DataTable ParameterData = new DataTable();
                //if (Parameter.Equals("SpindleVibration", StringComparison.OrdinalIgnoreCase) || Parameter.Equals("SpindleLoad", StringComparison.OrdinalIgnoreCase))
                //{
                //    ParameterData = MongoDBAccess.getParameterData(StartDate, EndDate, Parameter, MachineID);
                //}
                //else
                //{
                ParameterData = MandMDataBaseAccess.GetParameterisedData(MachineID, Parameter, StartDate, EndDate);
                //  }
                DataTable CycleStart = MandMDataBaseAccess.GetParameterisedData(MachineID, "CycleStart", StartDate, EndDate);
                DataTable CycleEnd = MandMDataBaseAccess.GetParameterisedData(MachineID, "CycleEnd", StartDate, EndDate);
                if (IsMarkerEnabled)
                {
                    foreach (DataRow row in CycleStart.Rows)
                    {
                        CycleTimes cycledata = new CycleTimes();
                        plotLineData = new PlotLines();
                        plotLineLabelParam labelParam = new plotLineLabelParam();
                        labelParam.text = "Start";
                        labelParam.rotation = 90;
                        labelParam.align = "left";
                        labelParam.y = 10;
                        labelParam.x = 10;
                        plotLineData.color = "Green";
                        plotLineData.width = 2;
                        DateTime dt = DateTime.Now;
                        dt = Convert.ToDateTime(row["ParameterValue"]);
                        plotLineData.value = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        plotLineData.zIndex = 150;
                        plotLineData.label = labelParam;
                        Chartseries.PlotLines.Add(plotLineData);
                        cycledata.Start = row["UpdatedtimeStamp"].ToString();
                        Chartseries.CycleStartEnd.Add(cycledata);
                    }
                    foreach (DataRow row in CycleEnd.Rows)
                    {
                        CycleTimes cycledata = new CycleTimes();
                        plotLineData = new PlotLines();
                        plotLineLabelParam labelParam = new plotLineLabelParam();
                        labelParam.text = "End";
                        labelParam.rotation = -90;
                        labelParam.align = "right";
                        labelParam.y = 10;
                        labelParam.x = 10;
                        plotLineData.color = "Red";
                        plotLineData.width = 2;
                        DateTime dt = DateTime.Now;
                        dt = Convert.ToDateTime(row["UpdatedtimeStamp"]);
                        plotLineData.value = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        plotLineData.zIndex = 150;
                        plotLineData.label = labelParam;
                        Chartseries.PlotLines.Add(plotLineData);
                        cycledata.End = row["UpdatedtimeStamp"].ToString();
                        Chartseries.CycleStartEnd.Add(cycledata);
                    }
                }
                foreach (DataRow Row in ParameterData.Rows)
                {
                    Data = new double[2];
                    DateTime dt = DateTime.Now;
                    dt = Convert.ToDateTime(Row["UpdatedtimeStamp"].ToString());
                    Data[1] = double.Parse(Row["ParameterValue"].ToString());
                    Data[0] = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                    DataListData.Add(Data);
                }
                LineSeries.color = "Black";
                LineSeries.name = "Value";
                LineSeries.step = false;
                LineSeries.type = "line";
                LineSeries.data = DataListData;
                Marker mak = new Marker();
                mak.enabled = true;
                mak.radius = 5;
                mak.symbol = "circle";
                LineSeries.marker = mak;
                Chartseries.series = new List<DataSeries>();
                Chartseries.series.Add(LineSeries);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return Chartseries;
        }
        #endregion

        #region Fourth Parameter Data
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ChartSeries GetFourthParameter(string Parameter, string MachineID, string StartDate, string EndDate, bool IsMarkerEnabled)
        {
            #region Hardcoded
            //ParameterEntity Data = new ParameterEntity();
            //try
            //{
            //    DataTable ParameterData = MandMDataBaseAccess.GetParameterisedData(MachineID, Parameter, StartDate, EndDate);

            //    Data.Title = Parameter;
            //    Data.Catergory = new List<string>();
            //    Data.Data = new List<double>();
            //    if (ParameterData.Rows.Count > 0)
            //    {
            //        foreach (DataRow row in ParameterData.Rows)
            //        {
            //            Data.Catergory.Add(row["UpdatedtimeStamp"].ToString());
            //            Data.Data.Add(Convert.ToDouble(row["ParameterValue"].ToString()));
            //        }
            //        //Data.Catergory = ParameterData.AsEnumerable().Select(x => x.Field<DateTime>("")).ToList();
            //        //Data.Data = ParameterData.AsEnumerable().Select(x => x.Field<Double>("")).ToList();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Logger.WriteErrorLog(ex.ToString());
            //}
            //return Data;

            #endregion
            double[] Data = null;
            List<double[]> DataListData = new List<double[]>();
            ChartSeries Chartseries = new ChartSeries();
            DataSeries LineSeries = new DataSeries();
            PlotLines plotLineData = new PlotLines();
            Chartseries.PlotLines = new List<PlotLines>();
            Chartseries.Title = GetParameterDisplaytext(Parameter);
            Chartseries.CycleStartEnd = new List<CycleTimes>();
            LineSeries.tooltip = new Tooltipdata();
            ParameterEntity MainData = new ParameterEntity();

            try
            {
                DataTable ParameterData = new DataTable();
                //if (Parameter.Equals("SpindleVibration", StringComparison.OrdinalIgnoreCase) || Parameter.Equals("SpindleLoad", StringComparison.OrdinalIgnoreCase))
                //{
                //    ParameterData = MongoDBAccess.getParameterData(StartDate, EndDate, Parameter, MachineID);
                //}
                //else
                //{
                ParameterData = MandMDataBaseAccess.GetParameterisedData(MachineID, Parameter, StartDate, EndDate);
                //}
                DataTable CycleStart = MandMDataBaseAccess.GetParameterisedData(MachineID, "CycleStart", StartDate, EndDate);
                DataTable CycleEnd = MandMDataBaseAccess.GetParameterisedData(MachineID, "CycleEnd", StartDate, EndDate);
                if (IsMarkerEnabled)
                {
                    foreach (DataRow row in CycleStart.Rows)
                    {
                        CycleTimes cycledata = new CycleTimes();
                        plotLineData = new PlotLines();
                        plotLineLabelParam labelParam = new plotLineLabelParam();
                        labelParam.text = "Start";
                        labelParam.rotation = 90;
                        labelParam.align = "left";
                        labelParam.y = 10;
                        labelParam.x = 10;
                        plotLineData.color = "Green";
                        plotLineData.width = 2;
                        DateTime dt = DateTime.Now;
                        dt = Convert.ToDateTime(row["ParameterValue"]);
                        plotLineData.value = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        plotLineData.zIndex = 150;
                        plotLineData.label = labelParam;
                        Chartseries.PlotLines.Add(plotLineData);
                        cycledata.Start = row["UpdatedtimeStamp"].ToString();
                        Chartseries.CycleStartEnd.Add(cycledata);
                    }
                    foreach (DataRow row in CycleEnd.Rows)
                    {
                        CycleTimes cycledata = new CycleTimes();
                        plotLineData = new PlotLines();
                        plotLineLabelParam labelParam = new plotLineLabelParam();
                        labelParam.text = "End";
                        labelParam.rotation = -90;
                        labelParam.align = "right";
                        labelParam.y = 10;
                        labelParam.x = 10;
                        plotLineData.color = "Red";
                        plotLineData.width = 2;
                        DateTime dt = DateTime.Now;
                        dt = Convert.ToDateTime(row["UpdatedtimeStamp"]);
                        plotLineData.value = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        plotLineData.zIndex = 150;
                        plotLineData.label = labelParam;
                        Chartseries.PlotLines.Add(plotLineData);
                        cycledata.End = row["UpdatedtimeStamp"].ToString();
                        Chartseries.CycleStartEnd.Add(cycledata);
                    }
                }
                foreach (DataRow Row in ParameterData.Rows)
                {
                    Data = new double[2];
                    DateTime dt = DateTime.Now;
                    dt = Convert.ToDateTime(Row["UpdatedtimeStamp"].ToString());
                    Data[1] = double.Parse(Row["ParameterValue"].ToString());
                    Data[0] = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                    DataListData.Add(Data);
                }
                LineSeries.color = "Black";
                LineSeries.name = "Value";
                LineSeries.step = false;
                LineSeries.type = "line";
                LineSeries.data = DataListData;
                Marker mak = new Marker();
                mak.enabled = true;
                mak.radius = 5;
                mak.symbol = "circle";
                LineSeries.marker = mak;
                Chartseries.series = new List<DataSeries>();
                Chartseries.series.Add(LineSeries);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return Chartseries;
        }
        #endregion



        protected void GraphView_CheckedChanged(object sender, EventArgs e)
        {
            //if(GraphView.Checked)
            //{
            //    mainContainer.Visible = false;
            //    Charts.Visible = true;
            //}
            //else
            //{
            //    mainContainer.Visible = true;
            //    Charts.Visible = false;
            //}
        }

        protected void lstParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = 0;
            foreach (ListItem itm in lstParameters.Items)
            {
                if (itm.Selected)
                    count++;
                if (count > 4)
                {
                    itm.Selected = false;
                    break;
                }
            }
        }

        protected void ddlMachineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindProcessParameterData();
        }
    }
}