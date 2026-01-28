using Elmah;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Vibration;

namespace Web_TPMTrakDashboard
{
    public partial class VibrationDashboard : System.Web.UI.Page
    {
        public static TempPresChartData tempData = new TempPresChartData();
        public static TempPresChartData presData = new TempPresChartData();
        public static DataTable dtTemp = new DataTable();
        public static DataTable dtPres = new DataTable();
        public static string lastviewedTimeVibration = string.Empty;
        public static string componentID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(IsPostBack))
            {
                HttpContext.Current.Session["lastviewedTimeVibration"] = "";
                string endTime = string.Empty;
                string startTime = VibrationDataBaseAccess.GetCurrentShiftStart(out endTime);
                txtFromDate.Text = DateTime.Now.AddMinutes(-30).ToString("yyyy-MM-dd HH:mm:ss");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                BindMachines();
                try
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (lstParameters.Items[i] != null)
                        {
                            lstParameters.Items[i].Selected = true;
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
        }

        private void BindMachines()
        {
            try
            {
                var allMachineName = VibrationDataBaseAccess.GetAllMachines("All");
                if (allMachineName != null && allMachineName.Count > 0)
                {
                    ddlMachineId.DataSource = allMachineName;
                    ddlMachineId.DataBind();
                    ddlMachineId.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> BindCycleStartTime(string startDT, string endDT, string MachineID, string param)
        {
            List<string> cycleStartTime = new List<string>();
            try
            {
                DateTime startDate = Util.GetDateTime(startDT);
                DateTime endDate = Util.GetDateTime(endDT);
                cycleStartTime = VibrationDataBaseAccess.GetAllCycleStartTimes(startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), MachineID, param);
                cycleStartTime.Insert(0, "All");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return cycleStartTime;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<ChartSeries> GetChartData(Header header)
        {

            List<ChartSeries> listChartSeries = new List<ChartSeries>();
            double[] DataError = null;
            double[] DataWarning = null;
            double[] Data = null;
            try
            {
                DataTable VibrationDt = new DataTable();
                DataTable CycleVibrationDt = new DataTable();
                VibrationDt = VibrationDataBaseAccess.Vibrationdata(header.FromDate, header.ToDate, header.MachineId, header.Parameter, out CycleVibrationDt);

                var distParameter = VibrationDt.AsEnumerable().Select(k => k.Field<string>("ParameterID")).Distinct().ToList();
                HttpContext.Current.Session["VibrationData"] = VibrationDt;
                for (int paramCount = 0; paramCount < distParameter.Count; paramCount++)
                {
                    string parameter = distParameter[paramCount];
                    if (!ConfigurationManager.AppSettings["VibrationParameterEnable"].Equals("1"))
                    {
                        parameter = "Vibration";
                    }
                    ChartSeries Chartseries = new ChartSeries();
                    Chartseries.Parameter = parameter;
                    DataTable VibrationData = VibrationDt.AsEnumerable().Where(k => k.Field<string>("ParameterID") == distParameter[paramCount]).CopyToDataTable();
                    DataTable CycleVibration = new DataTable();
                    if (CycleVibrationDt != null)
                    {
                        if (CycleVibrationDt.Rows.Count > 0)
                        {
                            //CycleVibration = CycleVibrationDt.AsEnumerable().Where(k => k.Field<string>("ParameterID") == distParameter[paramCount]).CopyToDataTable();
                            CycleVibration = CycleVibrationDt;
                        }
                    }

                    List<double[]> DataListData = new List<double[]>();
                    List<double[]> DataListError = new List<double[]>();
                    List<double[]> DataListWarning = new List<double[]>();
                    DataSeries LineSeries = new DataSeries();
                    DataSeries WarningSeries = new DataSeries();
                    DataSeries ErrorSeries = new DataSeries();
                    PlotLinesVibration plotLineData = new PlotLinesVibration();
                    //List<PlotLinesVibration> plotLinesList = new List<PlotLinesVibration>();
                    //StartEndCycleTimeVibration CycleStartEnd = new StartEndCycleTimeVibration();
                    Chartseries.PlotLines = new List<PlotLinesVibration>();
                    List<string> Machineid = new List<string>();
                    Tooltipdata tooltips = new Tooltipdata();
                    List<string> ComponentID = new List<string>();
                    List<string> Operationno = new List<string>();
                    List<double> Total_M_Observation = new List<double>();
                    List<double> ApplyRuleFor_N_Observation = new List<double>();
                    Chartseries.Title = "Vibration Analysis";
                    Chartseries.CycleStartEnd = new List<CycleTimes>();
                    LineSeries.tooltip = new Tooltipdata();

                  

                    foreach (DataRow row in CycleVibration.Rows)
                    {
                        CycleTimes cycledata = new CycleTimes();

                        plotLineData = new PlotLinesVibration();
                        plotLineLabelParamVibration labelParam = new plotLineLabelParamVibration();
                        labelParam.text = "Start";
                        labelParam.rotation = 90;
                        labelParam.align = "left";
                        labelParam.y = 10;
                        labelParam.x = 10;
                        plotLineData.color = "Green";
                        plotLineData.width = 2;
                        DateTime dt = DateTime.Now;
                        dt = (DateTime)(row["CycleStart"]);
                        plotLineData.value = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        plotLineData.zIndex = 150;
                        plotLineData.label = labelParam;
                        Chartseries.PlotLines.Add(plotLineData);

                        plotLineData = new PlotLinesVibration();
                        labelParam = new plotLineLabelParamVibration();
                        labelParam.text = "End";
                        labelParam.rotation = -90;
                        labelParam.align = "right";
                        labelParam.y = 10;
                        labelParam.x = 10;
                        plotLineData.color = "Red";
                        plotLineData.width = 2;
                        dt = (DateTime)(row["CycleEnd"]);
                        plotLineData.value = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        plotLineData.zIndex = 150;
                        plotLineData.label = labelParam;
                        Chartseries.PlotLines.Add(plotLineData);
                        if (paramCount == 0)
                        {
                            cycledata.Start = row["CycleStart"].ToString();
                            cycledata.End = row["CycleEnd"].ToString();
                            Chartseries.CycleStartEnd.Add(cycledata);
                        }
                    }
                    if (paramCount == 0)
                    {
                        if (header.Param == "View")
                        {
                            Chartseries.CycleStartEnd.Insert(0, new CycleTimes { Start = "All", End = "All" });
                        }
                    }
                    if (VibrationData != null && VibrationData.Rows.Count > 0)
                    {
                        HttpContext.Current.Session["lastviewedTimeVibration"] = (DateTime)VibrationData.Rows[VibrationData.Rows.Count - 1]["UpdatedTS"];
                        foreach (DataRow Row in VibrationData.Rows)
                        {
                            Data = new double[2]; DataError = new double[2]; DataWarning = new double[2];
                            DateTime dt = DateTime.Now;
                            dt = (DateTime)Row["UpdatedTS"];
                            Data[1] = double.Parse(Row["ActualValue"].ToString());
                            Data[0] = DataError[0] = DataWarning[0] = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                            DataError[1] = Row["UpperErrorLimit"].ToString() == "" ? 0.00 : double.Parse(Row["UpperErrorLimit"].ToString());
                            DataWarning[1] = Row["UpperWarningLimit"].ToString() == "" ? 0.00 : double.Parse(Row["UpperWarningLimit"].ToString());
                            DataListData.Add(Data);
                            DataListError.Add(DataError);
                            DataListWarning.Add(DataWarning);

                            string MNValue = string.IsNullOrEmpty(Row["Total_M_Observation"].ToString()) ? "0" : Row["Total_M_Observation"].ToString();
                            Total_M_Observation.Add(double.Parse(MNValue));
                            MNValue = string.IsNullOrEmpty(Row["ApplyRuleFor_N_Observation"].ToString()) ? "0" : Row["ApplyRuleFor_N_Observation"].ToString();
                            ApplyRuleFor_N_Observation.Add(double.Parse(MNValue));
                            Machineid.Add(Row["Machineid"].ToString());
                            Operationno.Add(Row["Operationno"].ToString());
                            ComponentID.Add(Row["ComponentID"].ToString());
                        }
                        ErrorSeries.color = "#D0312E";
                        ErrorSeries.step = true;
                        ErrorSeries.type = "area";
                        ErrorSeries.name = "Error";
                        ErrorSeries.data = DataListError;
                        ErrorSeries.marker = new Marker();
                        ErrorSeries.marker.enabled = false;
                        WarningSeries.color = "#51BA1A";
                        WarningSeries.step = true;
                        WarningSeries.type = "area";
                        WarningSeries.name = "Warning";
                        WarningSeries.data = DataListWarning;
                        WarningSeries.marker = new Marker();
                        WarningSeries.marker.enabled = false;
                        tooltips.Machineid = Machineid;
                        tooltips.ComponentID = ComponentID;
                        tooltips.Operationno = Operationno;
                        tooltips.ApplyRuleFor_N_Observation = ApplyRuleFor_N_Observation;
                        tooltips.Total_M_Observation = Total_M_Observation;
                        LineSeries.tooltip = tooltips;
                        LineSeries.color = "Black";
                        LineSeries.name = Chartseries.Parameter + " Data";
                        LineSeries.step = false;
                        LineSeries.type = "line";
                        LineSeries.data = DataListData;
                        Marker mak = new Marker();
                        mak.enabled = true;
                        mak.radius = 5;
                        mak.symbol = "circle";
                        LineSeries.marker = mak;
                        LineSeries.marker.enabled = true;
                        Chartseries.series = new List<DataSeries>();
                        Chartseries.series.Add(ErrorSeries); Chartseries.series.Add(WarningSeries); Chartseries.series.Add(LineSeries);
                    }
                    listChartSeries.Add(Chartseries);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                //if(!(VibrationData!=null && VibrationData.Rows.Count>0))
                //{
                //    Chartseries = new ChartSeries();
                //}
            }
            return listChartSeries;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<refreshdata> GetChartDataRefresh(Header header)
        {
            List<refreshdata> refreshDataList = new List<refreshdata>();
            try
            {
                DataTable VibrationDt = new DataTable();
                DataTable CycleVibration = new DataTable();
                VibrationDt = VibrationDataBaseAccess.GetDataRefresh(header.MachineId, header.FromDate, header.ToDate, header.Parameter, out CycleVibration);
                HttpContext.Current.Session["VibrationData"] = VibrationDt;
                var distParameter = VibrationDt.AsEnumerable().Select(k => k.Field<string>("ParameterID")).Distinct().ToList();

                for (int paramCount = 0; paramCount < distParameter.Count; paramCount++)
                {
                    string parameter = distParameter[paramCount];
                    if (!ConfigurationManager.AppSettings["VibrationParameterEnable"].Equals("1"))
                    {
                        parameter = "Vibration";
                    }
                    DataTable VibrationData = VibrationDt.AsEnumerable().Where(k => k.Field<string>("ParameterID") == distParameter[paramCount]).CopyToDataTable();
                    List<double[]> DataListData = new List<double[]>();
                    List<double[]> DataListError = new List<double[]>();
                    List<double[]> DataListWarning = new List<double[]>();
                    ChartSeries Chartseries = new ChartSeries();
                    DataSeries LineSeries = new DataSeries();
                    DataSeries WarningSeries = new DataSeries();
                    DataSeries ErrorSeries = new DataSeries();
                    PlotLinesVibration plotLineData = new PlotLinesVibration();
                    //List<PlotLinesVibration> plotLinesList = new List<PlotLinesVibration>();
                    //StartEndCycleTimeVibration CycleStartEnd = new StartEndCycleTimeVibration();
                    Chartseries.PlotLines = new List<PlotLinesVibration>();


                    Tooltipdata tooltips = new Tooltipdata();
                    List<string> Machineid = new List<string>();
                    List<string> ComponentID = new List<string>();
                    List<string> Operationno = new List<string>();
                    List<double> Total_M_Observation = new List<double>();
                    List<double> ApplyRuleFor_N_Observation = new List<double>();
                    Chartseries.Title = "Vibration Analysis";
                    LineSeries.tooltip = new Tooltipdata();
                    Chartseries.CycleStartEnd = new List<CycleTimes>();


                    #region oldcode
                    //try
                    //{
                    //    VibrationData = VibrationDataBaseAccess.GetDataRefresh(header.MachineId, header.FromDate, header.ToDate, out CycleVibration);
                    //    foreach (DataRow row in CycleVibration.Rows)
                    //    {
                    //        CycleTimes cycledata = new CycleTimes();

                    //        plotLineData = new PlotLinesVibration();
                    //        plotLineLabelParamVibration labelParam = new plotLineLabelParamVibration();
                    //        labelParam.text = "Start";
                    //        labelParam.rotation = 90;
                    //        labelParam.align = "left";
                    //        labelParam.y = 10;
                    //        labelParam.x = 10;
                    //        plotLineData.color = "Green";
                    //        plotLineData.width = 2;
                    //        DateTime dt = DateTime.Now;
                    //        dt = (DateTime)(row["CycleStart"]);
                    //        plotLineData.value = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                    //        plotLineData.zIndex = 150;
                    //        plotLineData.label = labelParam;
                    //        Chartseries.PlotLines.Add(plotLineData);

                    //        plotLineData = new PlotLinesVibration();
                    //        labelParam = new plotLineLabelParamVibration();
                    //        labelParam.text = "End";
                    //        labelParam.rotation = -90;
                    //        labelParam.align = "right";
                    //        labelParam.y = 10;
                    //        labelParam.x = 10;
                    //        plotLineData.color = "Red";
                    //        plotLineData.width = 2;
                    //        dt = (DateTime)(row["CycleEnd"]);
                    //        plotLineData.value = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                    //        plotLineData.zIndex = 150;
                    //        plotLineData.label = labelParam;
                    //        Chartseries.PlotLines.Add(plotLineData);
                    //        cycledata.Start = row["CycleStart"].ToString();
                    //        cycledata.End = row["CycleEnd"].ToString();
                    //        Chartseries.CycleStartEnd.Add(cycledata);
                    //    }



                    //    if (VibrationData != null && VibrationData.Rows.Count > 0)
                    //    {
                    //        foreach (DataRow Row in VibrationData.Rows)
                    //        {
                    //            Data = new double[2]; DataError = new double[2]; DataWarning = new double[2];
                    //            DateTime dt = DateTime.Now;
                    //            dt = (DateTime)(Row["UpdatedTS"]);
                    //            Data[1] = double.Parse(Row["ActualValue"].ToString());
                    //            Data[0] = DataError[0] = DataWarning[0] = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                    //            DataError[1] = Row["UpperErrorLimit"].ToString() == "" ? 0.00 : double.Parse(Row["UpperErrorLimit"].ToString());
                    //            DataWarning[1] = Row["UpperWarningLimit"].ToString() == "" ? 0.00 : double.Parse(Row["UpperWarningLimit"].ToString());
                    //            DataListData.Add(Data);
                    //            DataListError.Add(DataError);
                    //            DataListWarning.Add(DataWarning);

                    //            string MNValue = string.IsNullOrEmpty(Row["Total_M_Observation"].ToString()) ? "0" : Row["Total_M_Observation"].ToString();
                    //            Total_M_Observation.Add(double.Parse(MNValue));
                    //            MNValue = string.IsNullOrEmpty(Row["ApplyRuleFor_N_Observation"].ToString()) ? "0" : Row["ApplyRuleFor_N_Observation"].ToString();
                    //            ApplyRuleFor_N_Observation.Add(double.Parse(MNValue));
                    //            Machineid.Add(Row["Machineid"].ToString());
                    //            Operationno.Add(Row["Operationno"].ToString());
                    //            ComponentID.Add(Row["ComponentID"].ToString());
                    //        }
                    //        ErrorSeries.color = "Red";
                    //        ErrorSeries.step = true;
                    //        ErrorSeries.type = "area";
                    //        ErrorSeries.name = "Error";
                    //        ErrorSeries.data = DataListError;

                    //        WarningSeries.color = "Green";
                    //        WarningSeries.step = true;
                    //        WarningSeries.type = "area";
                    //        WarningSeries.name = "Warning";
                    //        WarningSeries.data = DataListWarning;
                    //        tooltips.Machineid = Machineid;
                    //        tooltips.ComponentID = ComponentID;
                    //        tooltips.Operationno = Operationno;
                    //        tooltips.ApplyRuleFor_N_Observation = ApplyRuleFor_N_Observation;
                    //        tooltips.Total_M_Observation = Total_M_Observation;
                    //        LineSeries.tooltip = tooltips;
                    //        LineSeries.color = "Black";
                    //        LineSeries.name = "Vibration Data";
                    //        LineSeries.step = false;
                    //        LineSeries.type = "line";
                    //        LineSeries.data = DataListData;
                    //        Chartseries.series = new List<DataSeries>();
                    //        Chartseries.series.Add(ErrorSeries); Chartseries.series.Add(WarningSeries); Chartseries.series.Add(LineSeries);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    Logger.WriteErrorLog(ex.Message);
                    //}
                    //finally
                    //{
                    //    if (!(VibrationData != null && VibrationData.Rows.Count > 0))
                    //    {
                    //        Chartseries = new ChartSeries();
                    //    }
                    //}
                    #endregion

                    refreshdata RefreshData = new refreshdata();
                    refreshEntity RefreshEntity = new refreshEntity();
                    RefreshData.PlotLines = new List<PlotLinesVibration>();
                    RefreshData.CycleStartEndrefresh = new List<CycleTimes>();
                    List<double> time = new List<double>();
                    List<double> Vibrationdata = new List<double>();
                    List<double> Errordata = new List<double>();
                    List<double> WarningData = new List<double>();
                    List<refreshEntity> ListEntity = new List<refreshEntity>();

                    RefreshData.ParameterID = parameter;
                    foreach (DataRow row in CycleVibration.Rows)
                    {
                        CycleTimes cycledata = new CycleTimes();
                        plotLineData = new PlotLinesVibration();
                        plotLineLabelParamVibration labelParam = new plotLineLabelParamVibration();
                        labelParam.text = "Start";
                        labelParam.rotation = 90;
                        labelParam.align = "left";
                        labelParam.y = 10;
                        labelParam.x = 10;
                        plotLineData.color = "Green";
                        plotLineData.width = 2;
                        DateTime dt = DateTime.Now;
                        dt = (DateTime)(row["CycleStart"]);
                        plotLineData.value = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        plotLineData.zIndex = 150;
                        plotLineData.label = labelParam;
                        RefreshData.PlotLines.Add(plotLineData);

                        plotLineData = new PlotLinesVibration();
                        labelParam = new plotLineLabelParamVibration();
                        labelParam.text = "End";
                        labelParam.rotation = -90;
                        labelParam.align = "right";
                        labelParam.y = 10;
                        labelParam.x = 10;
                        plotLineData.color = "Red";
                        plotLineData.width = 2;
                        dt = (DateTime)(row["CycleEnd"]);
                        plotLineData.value = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        plotLineData.zIndex = 150;
                        plotLineData.label = labelParam;
                        RefreshData.PlotLines.Add(plotLineData);
                        cycledata.Start = row["CycleStart"].ToString();
                        cycledata.End = row["CycleEnd"].ToString();
                        RefreshData.CycleStartEndrefresh.Add(cycledata);
                    }
                    if (VibrationData != null && VibrationData.Rows.Count > 0)
                    {
                        foreach (DataRow Row in VibrationData.Rows)
                        {
                            //Data = new double[2]; DataError = new double[2]; DataWarning = new double[2];
                            DateTime dt = DateTime.Now;
                            dt = (DateTime)(Row["UpdatedTS"]);
                            //Data[1] = double.Parse(Row["ActualValue"].ToString());

                            //Data[0] = DataError[0] = DataWarning[0] = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                            double timeval = ((double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);
                            time.Add(timeval);
                            Vibrationdata.Add(double.Parse(Row["ActualValue"].ToString()));
                            Errordata.Add(Row["UpperErrorLimit"].ToString() == "" ? 0.00 : double.Parse(Row["UpperErrorLimit"].ToString()));
                            WarningData.Add(Row["UpperWarningLimit"].ToString() == "" ? 0.00 : double.Parse(Row["UpperWarningLimit"].ToString()));
                            //RefreshEntity.time = (double)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                            ////DataError[1] = Row["UpperErrorLimit"].ToString() == "" ? 0.00 : double.Parse(Row["UpperErrorLimit"].ToString());
                            //RefreshEntity.ErrorData = Row["UpperErrorLimit"].ToString() == "" ? 0.00 : double.Parse(Row["UpperErrorLimit"].ToString());
                            ////DataWarning[1] = Row["UpperWarningLimit"].ToString() == "" ? 0.00 : double.Parse(Row["UpperWarningLimit"].ToString());
                            //RefreshEntity.WarningData = Row["UpperWarningLimit"].ToString() == "" ? 0.00 : double.Parse(Row["UpperWarningLimit"].ToString());
                            ////DataListData.Add(Data);
                            //DataListError.Add(DataError);
                            //DataListWarning.Add(DataWarning);
                            string MNValue = string.IsNullOrEmpty(Row["Total_M_Observation"].ToString()) ? "0" : Row["Total_M_Observation"].ToString();
                            Total_M_Observation.Add(double.Parse(MNValue));
                            MNValue = string.IsNullOrEmpty(Row["ApplyRuleFor_N_Observation"].ToString()) ? "0" : Row["ApplyRuleFor_N_Observation"].ToString();
                            ApplyRuleFor_N_Observation.Add(double.Parse(MNValue));

                            Machineid.Add(Row["Machineid"].ToString());
                            Operationno.Add(Row["Operationno"].ToString());
                            ComponentID.Add(Row["ComponentID"].ToString());




                        }

                        RefreshEntity.Machineid = Machineid;
                        RefreshEntity.ComponentID = ComponentID;
                        RefreshEntity.Operationno = Operationno;
                        RefreshEntity.time = time;
                        RefreshEntity.vibrationdata = Vibrationdata;
                        RefreshEntity.ErrorData = Errordata;
                        RefreshEntity.WarningData = WarningData;
                        RefreshEntity.ApplyRuleFor_N_Observation = ApplyRuleFor_N_Observation;
                        RefreshEntity.Total_M_Observation = Total_M_Observation;
                        RefreshData.Refreshdata = RefreshEntity;
                    }
                    refreshDataList.Add(RefreshData);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return refreshDataList;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool isParameterIdRequired()
        {
            bool  isParameterEnabled = false;
            try
            {
                if (ConfigurationManager.AppSettings["VibrationParameterEnable"].Equals("1"))
                {
                    isParameterEnabled = true;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return isParameterEnabled;
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            bool successfull = false;
            DataTable dtVibrationData = new DataTable();
            DataTable CycleVibration = new DataTable();
            try
            {
                string MachineID = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                string from_date = !string.IsNullOrEmpty(txtFromDate.Text) ? txtFromDate.Text : "";
                string to_date = !string.IsNullOrEmpty(txtToDate.Text) ? txtToDate.Text : "";
                string Parameter = "";
                for (int i = 0; i < lstParameters.Items.Count; i++)
                {
                    if (Parameter == "")
                    {
                        Parameter += lstParameters.Items[i].Value;
                    }
                    else
                    {
                        Parameter += "," + lstParameters.Items[i].Value;
                    }
                }
                if (HttpContext.Current.Session["VibrationData"] != null)
                {
                    dtVibrationData = HttpContext.Current.Session["VibrationData"] as DataTable;
                }
                else
                {
                    dtVibrationData = VibrationDataBaseAccess.Vibrationdata(from_date, to_date, MachineID, Parameter, out CycleVibration);
                    HttpContext.Current.Session["VibrationData"] = dtVibrationData;
                }
                if (dtVibrationData != null && dtVibrationData.Rows.Count > 0)
                {
                    successfull = VibrationGenerateReport.GenerateVibrationDataReport(MachineID, from_date, to_date, dtVibrationData);
                    if (successfull)
                    {
                        lblMessages.Text = "Vibration data exported successfully.";
                        lblMessages.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblMessages.Text = "Error. Export failed.";
                        lblMessages.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lblMessages.Text = "No data to export.";
                    lblMessages.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
                lblMessages.ForeColor = Color.Red;
            }
        }
    }
}