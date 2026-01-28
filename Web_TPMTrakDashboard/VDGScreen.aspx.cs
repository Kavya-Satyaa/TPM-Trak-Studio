using ChartDirector;
using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using ModelClassLibrary;
using BusinessClassLibrary;
using System.Drawing;
using System.Configuration;

namespace Web_TPMTrakDashboard
{
    public partial class VDGScreen : System.Web.UI.Page
    {
        public static List<ColumnViewSetting> colHeaderVDGProd = new List<ColumnViewSetting>();
        public static List<ColumnViewSetting> colHeaderVDGDown = new List<ColumnViewSetting>();
        public static List<ColumnViewSetting> colHeaderVDGProdandDown = new List<ColumnViewSetting>();
        public static List<string> formats = new List<string>() { "dd-MM-yyyy HH:mm:ss", "yyyy-MM-dd HH:mm:ss" };
        public List<UserAccessModel> useAccessData = null;
        public int fontSize = 20;
        public string admin = "none";
        public int isModifiedIndexx = 0, EndTimeIndex = 0;
        Stopwatch stopwatch = new Stopwatch();
        protected void Page_UnLoad(object sender, EventArgs e)
        {
            stopwatch.Stop();
            Logger.WriteDebugLog("Page Un Load Completed : " + stopwatch.Elapsed.TotalSeconds);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            stopwatch.Start();
            Logger.WriteDebugLog("Page Load STARTED : ------------------------------------------------- ");
            try
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                fontSize = Convert.ToInt32(Session["fontSize"]);
                if (!IsPostBack)
                {
                    Chart.setLicenseCode("P5DG3ZGU8YH2JHR2C67C5BA1");
                    BindPlantID();
                    BindCellID();
                    BindMachines();
                    //if (Session["AdminData"] != null)
                    //    admin = Session["AdminData"].ToString() == "Admin" ? "" : "none";
                    if (Session["UserAccessData"] == null)
                        Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"].ToString());
                    else
                        useAccessData = Session["UserAccessData"] as List<UserAccessModel>;
                    if (useAccessData != null && !useAccessData.Where(ss => ss.Code.Equals("UpdateVDGCmponent", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                        admin = "none";
                    else
                        admin = "";

                    //if (System.Web.Configuration.WebConfigurationManager.AppSettings["TAFEPages"].ToString() == "0")
                    //{

                    //    gridDownTimeData.Columns.RemoveAt(1);
                    //    gridProductionData.Columns.RemoveAt(1);

                    //}
                    //if (System.Web.Configuration.WebConfigurationManager.AppSettings["sonapages"].ToString() == "0")
                    //{

                    //    gridDownTimeData.Columns.RemoveAt(12);
                    //    gridDownTimeData.Columns.RemoveAt(12);
                    //    gridDownTimeData.Columns.RemoveAt(12);
                    //    gridDownTimeData.Columns.RemoveAt(12);
                    //    gridDownTimeData.Columns.RemoveAt(12);
                    //    gridProductionData.Columns.RemoveAt(12);
                    //    gridProductionData.Columns.RemoveAt(12);
                    //    gridProductionData.Columns.RemoveAt(12);
                    //    gridProductionData.Columns.RemoveAt(12);
                    //    gridProductionData.Columns.RemoveAt(12);

                    //}

                    BindGrids();
                    hdfValueMange.Value = "fristTime";
                    hdfPaging.Value = "";
                    string passDate = string.Empty, shiftID = string.Empty;
                    // var machineID = Request.QueryString["machineId"].ToString();
                    string fromDate = string.Empty, toDate = string.Empty;
                    DateTime date = DateTime.Now;
                    DateTime fromDt = DateTime.Now;
                    DateTime toDt = DateTime.Now;

                    if (Request.QueryString["Page"] != "table")
                    {


                        if (Request.QueryString["shiftId"] != null)
                        {
                            shiftID = Request.QueryString["shiftId"].ToString();
                            passDate = Request.QueryString["date"].ToString();
                            Session["MachineId"] = ddlMachineId.SelectedValue = Request.QueryString["machineId"].ToString();
                        }

                        Session["MachineId"] = ddlMachineId.SelectedValue;
                        DateTime formateDate = DateTime.Now.Date;


                        if (passDate.Trim() == "")
                        {
                            if (Session["FromDate"] != null && Session["ToDate"] != null)
                            {

                                //txtFromDate.Text = Convert.ToDateTime(Session["FromDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss") ;
                                txtFromDate.Text = Util.GetDateTime(Session["FromDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");

                                //txtToDate.Text = Convert.ToDateTime(Session["ToDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                                txtToDate.Text = Util.GetDateTime(Session["ToDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");

                            }
                            else
                            {

                                fromDate = VDGDataBaseAccess.GetLogicalDayStart(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                txtFromDate.Text = Util.GetDateTime(fromDate).ToString("dd-MM-yyyy HH:mm");
                                toDate = VDGDataBaseAccess.GetLogicalDayEnd(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                txtToDate.Text = Util.GetDateTime(toDate).ToString("dd-MM-yyyy HH:mm");
                            }
                        }
                        else
                        {
                            List<string> shiftTimings = DataBaseAccess.GetCurrentShiftTime(shiftID, passDate.Trim());
                            if (shiftTimings != null && shiftTimings.Count > 0)
                            {
                                fromDate = shiftTimings.First();
                                txtFromDate.Text = Convert.ToDateTime(fromDate).ToString("dd-MM-yyyy HH:mm:ss");
                                toDate = shiftTimings.Last();
                                txtToDate.Text = Convert.ToDateTime(toDate).ToString("dd-MM-yyyy HH:mm:ss");
                            }
                            //fromDate = VDGDataBaseAccess.GetLogicalDayStart1(passDate.Trim());
                            //txtFromDate.Text = Convert.ToDateTime(fromDate).ToString("dd-MM-yyyy HH:mm:ss");
                            //toDate = VDGDataBaseAccess.GetLogicalDayEnd1(passDate.Trim());
                            //txtToDate.Text = Convert.ToDateTime(toDate).ToString("dd-MM-yyyy HH:mm:ss");
                        }
                    }
                    else
                    {
                        if (Request.QueryString["shiftId"] != null)
                        {
                            string frg = string.Empty;
                            Console.WriteLine(frg);

                            shiftID = Request.QueryString["shiftId"].ToString();
                            fromDate = Request.QueryString["fromDate"].ToString();
                            toDate = Request.QueryString["toDate"].ToString();
                            Session["MachineId"] = ddlMachineId.SelectedValue = Request.QueryString["machineId"].ToString();
                        }
                        Session["MachineId"] = ddlMachineId.SelectedValue;


                        fromDt = Util.GetDateTime(fromDate);
                        //DateTime.TryParse(fromDate, out date);
                        //{
                        //    fromDt = date;
                        //}
                        toDt = Util.GetDateTime(toDate);
                        //DateTime.TryParse(toDate, out date);
                        //{
                        //    toDt = date;
                        //}
                        // double dif =Math.Abs((fromDt - toDt).TotalDays);
                        //double dif = (toDt - fromDt).TotalDays;
                        //if (dif > 7)
                        //{
                        //    fromDt = toDt.AddDays(-7);
                        //}
                        Session["FromDate"] = txtFromDate.Text = fromDt.ToString("dd-MM-yyyy HH:mm");
                        //DateTime.TryParse(toDate, out date);
                        Session["ToDate"] = txtToDate.Text = toDt.ToString("dd-MM-yyyy HH:mm");

                    }
                    GetDetailsForCockPits();
                    //stopwatch.Stop();
                    Logger.WriteDebugLog("Page Load Completed : " + stopwatch.Elapsed.TotalSeconds);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }

        }

        private void BindGrids()
        {

            try
            {
                List<string> colPropertyNames = new List<string>();
                List<string> colPropertyNamesFromDB = new List<string>();
                string Languagespecified = Session["Language"] == null ? "en" : Session["Language"].ToString();
                List<string> colHeaderNames = DataBaseAccess.GetVDGColHeader("WebTPMTrakVDGProduction", Languagespecified, out colPropertyNamesFromDB);
                colHeaderNames = putRemarksLast(colHeaderNames, colPropertyNamesFromDB, out colPropertyNames); //changed13112021
                for (int i = colPropertyNames.Count - 1; i >= 0; i--)
                {
                    //if (!colPropertyNames[i].Equals("Remarks", StringComparison.OrdinalIgnoreCase)) //changed13112021
                    //{
                    BoundField boundField = new BoundField();
                    string dataField = colPropertyNames[i].ToString();
                    if (colPropertyNames[i].ToString().Equals("StartTime", StringComparison.OrdinalIgnoreCase))
                    {
                        dataField = "StartTimeToDisplay";
                    }
                    else if (colPropertyNames[i].ToString().Equals("EndTime", StringComparison.OrdinalIgnoreCase))
                    {
                        dataField = "EndTimeToDisplay";
                    }
                    boundField.DataField = dataField;
                    boundField.HeaderText = colHeaderNames[i].ToString();
                    gridProductionData.Columns.Insert(0, boundField);
                    //}
                }

                colHeaderNames = DataBaseAccess.GetVDGColHeader("WebTPMTrakVDGDownTime", Languagespecified, out colPropertyNamesFromDB);
                colHeaderNames = putRemarksLast(colHeaderNames, colPropertyNamesFromDB, out colPropertyNames); //changed13112021
                for (int i = colPropertyNames.Count - 1; i >= 0; i--)
                {
                    //if (!colPropertyNames[i].Equals("Remarks", StringComparison.OrdinalIgnoreCase)) //changed13112021
                    //{
                    BoundField boundField = new BoundField();
                    string dataField = colPropertyNames[i].ToString();
                    if (colPropertyNames[i].ToString().Equals("StartTime", StringComparison.OrdinalIgnoreCase))
                    {
                        dataField = "StartTimeToDisplay";
                    }
                    else if (colPropertyNames[i].ToString().Equals("EndTime", StringComparison.OrdinalIgnoreCase))
                    {
                        dataField = "EndTimeToDisplay";
                    }
                    boundField.DataField = dataField;
                    boundField.HeaderText = colHeaderNames[i].ToString();
                    gridDownTimeData.Columns.Insert(0, boundField);
                    // }
                }

                colHeaderNames = DataBaseAccess.GetVDGColHeader("WebTPMTrakVDGProductionandDown", Languagespecified, out colPropertyNamesFromDB);
                colHeaderNames = putRemarksLast(colHeaderNames, colPropertyNamesFromDB, out colPropertyNames);
                for (int i = colPropertyNames.Count - 1; i >= 0; i--)
                {
                    BoundField boundField = new BoundField();
                    string dataField = colPropertyNames[i].ToString();
                    boundField.DataField = dataField;
                    boundField.HeaderText = colHeaderNames[i].ToString();
                    gvProductionDownData.Columns.Insert(0, boundField);
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        private List<string> putRemarksLast(List<string> colHeaderNames, List<string> colPropertyNames, out List<string> colPropertyNamesFinal) //changed13112021
        {
            colPropertyNamesFinal = colPropertyNames;
            List<string> colHeaderNamesFinal = colHeaderNames;
            try
            {
                int i = 0;
                var ActionTakenColumn = colPropertyNames.Where(k => k.Equals("ActionTaken", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                {
                    if (ActionTakenColumn != null)
                    {
                        if (ActionTakenColumn.Count() > 0)
                        {
                            colPropertyNames.Remove("ActionTaken");
                            colPropertyNames.Insert(colPropertyNames.Count - i, "ActionTaken");
                            colHeaderNames.Remove("ActionTaken");
                            colHeaderNames.Insert(colHeaderNames.Count - i, "ActionTaken");
                            colPropertyNamesFinal = colPropertyNames;
                            colHeaderNamesFinal = colHeaderNames;
                            i++;
                        }
                    }
                }
                var result = colPropertyNames.Where(k => k.Equals("Remarks", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                {
                    if (result != null)
                    {
                        if (result.Count() > 0)
                        {
                            colPropertyNames.Remove("Remarks");
                            colPropertyNames.Insert(colPropertyNames.Count - i, "Remarks");
                            colHeaderNames.Remove("Remarks");
                            colHeaderNames.Insert(colHeaderNames.Count - i, "Remarks");
                            colPropertyNamesFinal = colPropertyNames;
                            colHeaderNamesFinal = colHeaderNames;
                            i++;
                        }
                    }
                }
                var IsModified = colPropertyNames.Where(k => k.Equals("IsModified", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                {
                    if (result != null)
                    {
                        if (result.Count() > 0)
                        {
                            colPropertyNames.Remove("IsModified");
                            colPropertyNames.Insert(colPropertyNames.Count - i, "IsModified");
                            colHeaderNames.Remove("IsModified");
                            colHeaderNames.Insert(colHeaderNames.Count - i, "IsModified");
                            colPropertyNamesFinal = colPropertyNames;
                            colHeaderNamesFinal = colHeaderNames;
                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return colHeaderNamesFinal;
        }
        private List<ColumnViewSetting> putRemarksLast2(List<ColumnViewSetting> colHeaderVDG) //changed13112021
        {
            try
            {
                int i = 0;
                ColumnViewSetting ActionTakenColumn = colHeaderVDG.Where(k => k.ValueInText.Equals("ActionTaken", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                {
                    if (ActionTakenColumn != null)
                    {
                        if (!string.IsNullOrEmpty(ActionTakenColumn.ValueInText))
                        {
                            ColumnViewSetting temp = ActionTakenColumn;
                            colHeaderVDG.Remove(temp);
                            colHeaderVDG.Insert(colHeaderVDG.Count - i, temp);
                            i++;
                        }
                    }
                }

                ColumnViewSetting result = colHeaderVDG.Where(k => k.ValueInText.Equals("Remarks", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                {
                    if (result != null)
                    {
                        if (!string.IsNullOrEmpty(result.ValueInText))
                        {
                            ColumnViewSetting temp = result;
                            colHeaderVDG.Remove(temp);
                            colHeaderVDG.Insert(colHeaderVDG.Count - i, temp);
                            i++;
                        }
                    }
                }
                ColumnViewSetting isModifiedColumn = colHeaderVDG.Where(k => k.ValueInText.Equals("IsModified", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                {
                    if (isModifiedColumn != null)
                    {
                        if (!string.IsNullOrEmpty(isModifiedColumn.ValueInText))
                        {
                            ColumnViewSetting temp = isModifiedColumn;
                            colHeaderVDG.Remove(temp);
                            colHeaderVDG.Insert(colHeaderVDG.Count - i, temp);
                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return colHeaderVDG;
            }
            return colHeaderVDG;
        }
       
        private void GridColumnSettings()
        {
            try
            {
                bool isModifiedVisible = false;string IsModifiedColumnVisibilityupdate = "";
                isModifiedVisible= DataBaseAccess.IsModifiedColumnVisibility("EnableProductionLog");
                if (isModifiedVisible)
                {
                    IsModifiedColumnVisibilityupdate = DataBaseAccess.UpdateModifiedColumnVisibility_CockpitDefaults("WebTPMTrakVDGProduction", "IsModified", 1);
                }
                else
                {
                    IsModifiedColumnVisibilityupdate = DataBaseAccess.UpdateModifiedColumnVisibility_CockpitDefaults("WebTPMTrakVDGProduction", "IsModified", 0);
                }
                isModifiedVisible = DataBaseAccess.IsModifiedColumnVisibility("EnableDownLog");
                if (isModifiedVisible)
                {
                    IsModifiedColumnVisibilityupdate = DataBaseAccess.UpdateModifiedColumnVisibility_CockpitDefaults("WebTPMTrakVDGDownTime", "IsModified", 1);
                }
                else
                {
                    IsModifiedColumnVisibilityupdate = DataBaseAccess.UpdateModifiedColumnVisibility_CockpitDefaults("WebTPMTrakVDGDownTime", "IsModified", 0);
                }
                if (Session["HeaderDownGrid"] == null && Session["HeaderProdGrid"] == null && Session["HeaderProdDownGrid"] == null)
                {
                    Session["HeaderProdGrid"] = DataBaseAccess.BindSettingPage("WebTPMTrakVDGProduction", Session["Language"] == null ? "en" : Session["Language"].ToString());
                    Session["HeaderDownGrid"] = DataBaseAccess.BindSettingPage("WebTPMTrakVDGDownTime", Session["Language"] == null ? "en" : Session["Language"].ToString());
                    Session["HeaderProdDownGrid"] = DataBaseAccess.BindSettingPage("WebTPMTrakVDGProductionandDown", Session["Language"] == null ? "en" : Session["Language"].ToString());
                    colHeaderVDGProd = Session["HeaderProdGrid"] as List<ColumnViewSetting>;
                    colHeaderVDGDown = Session["HeaderDownGrid"] as List<ColumnViewSetting>;
                    colHeaderVDGProdandDown = Session["HeaderProdDownGrid"] as List<ColumnViewSetting>;
                }
                else
                {
                    colHeaderVDGProd = Session["HeaderProdGrid"] as List<ColumnViewSetting>;
                    colHeaderVDGDown = Session["HeaderDownGrid"] as List<ColumnViewSetting>;
                    colHeaderVDGProdandDown = Session["HeaderProdDownGrid"] as List<ColumnViewSetting>;
                }
                colHeaderVDGProd = putRemarksLast2(colHeaderVDGProd); //changed13112021
                colHeaderVDGDown = putRemarksLast2(colHeaderVDGDown);
                colHeaderVDGProdandDown = putRemarksLast2(colHeaderVDGProdandDown);
                int visibleCellCount = 0;
                List<ListItem> visibleCellList = new List<ListItem>();
                for (int i = 0; i < colHeaderVDGProd.Count; i++)
                {
                    gridProductionData.Columns[i].HeaderText = colHeaderVDGProd[i].ValueInText2;
                    gridProductionData.Columns[i].Visible = colHeaderVDGProd[i].ValueInBool;
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["ShantiIronPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!ddlMachineId.SelectedValue.Equals("Leak Test Machine", StringComparison.OrdinalIgnoreCase))
                        {
                            if (colHeaderVDGProd[i].ValueInText == "LeakTestRemarks" || colHeaderVDGProd[i].ValueInText == "Result")
                            {
                                gridProductionData.Columns[i].Visible = false;
                            }
                        }
                    }
                    else
                    {
                        if (colHeaderVDGProd[i].ValueInText == "LeakTestRemarks" || colHeaderVDGProd[i].ValueInText == "Result")
                        {
                            gridProductionData.Columns[i].Visible = false;
                        }
                    }
                    if (colHeaderVDGProd[i].ValueInText == "InputCode" || colHeaderVDGProd[i].ValueInText == "OutputCode")
                    {
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["PittiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            gridProductionData.Columns[i].Visible = true;
                        }
                        else
                        {
                            gridProductionData.Columns[i].Visible = false;
                        }
                    }
                    if (gridProductionData.Columns[i].Visible)
                    {
                        visibleCellList.Add(new ListItem() { Text = colHeaderVDGProd[i].ValueInText, Value = visibleCellCount.ToString() });
                        visibleCellCount++;

                    }
                    if (ConfigurationManager.AppSettings["AlliedPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        string MachineDesc = DataBaseAccess.GetMachineDescription(ddlMachineId.SelectedValue.ToString());
                        if (!MachineDesc.Equals("Hydro Static Machine", StringComparison.OrdinalIgnoreCase))
                        {
                            if (colHeaderVDGProd[i].ValueInText == "ActHoldingTime" || colHeaderVDGProd[i].ValueInText == "ActTestPressure")
                            {
                                gridProductionData.Columns[i].Visible = false;
                            }
                        }

                    }
                }
                Session["EnabledCellListForContext"] = visibleCellList;
                for (int j = 0; j < colHeaderVDGDown.Count; j++)
                {
                    gridDownTimeData.Columns[j].HeaderText = colHeaderVDGDown[j].ValueInText2;
                    gridDownTimeData.Columns[j].Visible = colHeaderVDGDown[j].ValueInBool;
                    if (colHeaderVDGDown[j].ValueInText == "InputCode" || colHeaderVDGDown[j].ValueInText == "OutputCode")
                    {
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["PittiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            gridDownTimeData.Columns[j].Visible = true;
                        }
                        else
                        {
                            gridDownTimeData.Columns[j].Visible = false;
                        }
                    }
                }

                for (int j = 0; j < colHeaderVDGProdandDown.Count; j++)
                {
                    gvProductionDownData.Columns[j].HeaderText = colHeaderVDGProdandDown[j].ValueInText2;
                    gvProductionDownData.Columns[j].Visible = colHeaderVDGProdandDown[j].ValueInBool;
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private static Double CalculateSpeedLoadRatio(string stdVal, string avgVal)
        {
            Double Val = 0.0;
            string TimeFormatVal = (VDGDataBaseAccess.GetCockpitTimeFormat()).ToLower();
            try
            {
                if (!string.IsNullOrEmpty(TimeFormatVal))
                {
                    if (TimeFormatVal.Equals("ss"))
                    {
                        Val = Math.Round((Convert.ToDouble(stdVal) / Convert.ToDouble(avgVal)), 2);
                    }
                    else if (TimeFormatVal.Equals("mm"))
                    {
                        Val = Math.Round((Convert.ToDouble(stdVal) * 60) / (Convert.ToDouble(avgVal) * 60), 2);
                    }
                    else if (TimeFormatVal.Equals("hh"))
                    {
                        Val = Math.Round((Convert.ToDouble(stdVal) * 3600) / (Convert.ToDouble(avgVal) * 3600), 2);
                    }
                    else if (TimeFormatVal.Equals("hh:mm:ss"))
                    {
                        Double stdValue = 0.0;
                        Double avgValue = 0.0;
                        string[] arrStdVal = (stdVal.ToString()).Split(':');
                        if (Convert.ToInt32(arrStdVal.Length) > 2)
                            stdValue = (Convert.ToDouble(arrStdVal[0]) * 3600) + (Convert.ToDouble(arrStdVal[1]) * 60) + (Convert.ToDouble(arrStdVal[2]));

                        string[] arrAvgVal = (avgVal.ToString()).Split(':');
                        if (Convert.ToInt32(arrAvgVal.Length) > 2)
                            avgValue = (Convert.ToDouble(arrAvgVal[0]) * 3600) + (Convert.ToDouble(arrAvgVal[1]) * 60) + (Convert.ToDouble(arrAvgVal[2]));

                        Val = Math.Round((stdValue / avgValue), 2);
                        if (Val.ToString() == "NaN" || Val.ToString() == "Infinity")
                            Val = 0.0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return Val;
        }

        #region "Refresh Click Event"
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            hdfPaging.Value = "";
            hdfValueMange.Value = "fristTime";
            GetDetailsForCockPits();
        }
        #endregion

        private void GetDetailsForCockPits()
        {
            try
            {
                Session["EfficiencyPeChart"] = null;
                Session["EfficiencyAeChart"] = null;
                Session["EfficiencyOEEChart"] = null;
                Session["proddatasource"] = null;
                Session["downdatasource"] = null;
                Session["eventdatasource"] = null;
                Session["ProductionAndDownDataSource"] = null;
                GridColumnSettings();


                RegisterAsyncTask(new PageAsyncTask(GetAsync));
                //RegisterAsyncTask(new PageAsyncTask(AsyncChartViewerData));
                //SetMachineDataValues();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        #region "Set Machine data setting value"
        private void SetMachineDataValues()
        {
            try
            {

                if (Session["EfficiencyPeChart"] == null && Session["EfficiencyAeChart"] == null && Session["EfficiencyOEEChart"] == null)
                {
                    DataTable dt = new DataTable();
                    DateTime fromDate = DateTime.Now.Date;
                    DateTime toDate = DateTime.Now.Date;
                    fromDate = Util.GetDateTime(txtFromDate.Text.Trim());
                    toDate = Util.GetDateTime(txtToDate.Text.Trim());
                    //if (!DateTime.TryParseExact(txtFromDate.Text.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDate))
                    //{
                    //    lblMessages.Text = "Please enter valid from date format.";
                    //    return;
                    //}
                    //if (!DateTime.TryParseExact(txtToDate.Text.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out toDate))
                    //{
                    //    lblMessages.Text = "Please enter valid to date format.";
                    //    return;
                    //}
                    if (CheckDateRange())
                    {
                        dt = VDGDataBaseAccess.GetMachineData("s_GetCockpitData_WithTempTable_eshopx", fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), "", ddlMachineId.SelectedValue.ToString());
                    }
                    else
                    {
                        //dt = VDGDataBaseAccess.GetMachineData("[s_GetCockpitData_eshopx]", fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), "", ddlMachineId.SelectedValue.ToString());
                        dt = VDGDataBaseAccess.GetMachineData("s_GetCockpitData_WithTempTable_eshopx", fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), "", ddlMachineId.SelectedValue.ToString());
                    }

                    ChartViewerfromData(dt);
                }
                else
                {
                    ChartViewerSession();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Set Machine data setting value"
        private async Task AsyncChartViewerData()
        {
            try
            {
                if (Session["EfficiencyPeChart"] == null && Session["EfficiencyAeChart"] == null && Session["EfficiencyOEEChart"] == null)
                {
                    DataTable dt = new DataTable();
                    DateTime fromDate = DateTime.Now.Date;
                    DateTime toDate = DateTime.Now.Date;
                    fromDate = Util.GetDateTime(txtFromDate.Text.Trim());
                    toDate = Util.GetDateTime(txtToDate.Text.Trim());
                    //if (!DateTime.TryParseExact(txtFromDate.Text.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDate))
                    //{
                    //    lblMessages.Text = "Please enter valid from date format.";
                    //    return;
                    //}
                    //if (!DateTime.TryParseExact(txtToDate.Text.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out toDate))
                    //{
                    //    lblMessages.Text = "Please enter valid to date format.";
                    //    return;
                    //}
                    Task<DataTable> chartViewrDataAsyncTask = null;
                    if (CheckDateRange())
                    {
                        chartViewrDataAsyncTask = VDGDataBaseAccess.GetProductionAndDownData("s_GetCockpitData_WithTempTable_eshopx", fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), ddlMachineId.SelectedValue.ToString());
                        await Task.WhenAll(chartViewrDataAsyncTask).ConfigureAwait(false);
                        dt = chartViewrDataAsyncTask.Result;
                    }
                    else
                    {
                        //chartViewrDataAsyncTask = VDGDataBaseAccess.GetProductionAndDownData("[s_GetCockpitData_eshopx]", fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), ddlMachineId.SelectedValue.ToString());
                        chartViewrDataAsyncTask = VDGDataBaseAccess.GetProductionAndDownData("s_GetCockpitData_WithTempTable_eshopx", fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), ddlMachineId.SelectedValue.ToString());
                        await Task.WhenAll(chartViewrDataAsyncTask).ConfigureAwait(false);
                        dt = chartViewrDataAsyncTask.Result;
                    }
                    ChartViewerfromData(dt);
                }
                else
                {
                    ChartViewerSession();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region ------------Bind Event Data GridView-------------
        private void BindEventGrid()
        {
            try
            {
                DateTime fromDate = DateTime.Now.Date;
                DateTime toDate = DateTime.Now.Date;
                fromDate = Util.GetDateTime(txtFromDate.Text.Trim());
                toDate = Util.GetDateTime(txtToDate.Text.Trim());
                DataTable EventData = null;
                if (Session["eventdatasource"] == null)
                {
                    EventData = VDGDataBaseAccess.GetEventData(fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), ddlMachineId.SelectedValue.ToString());
                    Session["eventdatasource"] = EventData;
                }
                else
                {
                    EventData = Session["eventdatasource"] as DataTable;
                }
                if (EventData.Rows.Count > 0)
                {
                    gridEventData.AllowPaging = true;
                    gridEventData.PageSize = VDGDataBaseAccess.PageSize;
                    gridEventData.DataSource = EventData;
                    gridEventData.DataBind();
                }
                else
                {
                    gridEventData.DataSource = EventData;
                    gridEventData.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region ---------- Production & Down Data -----------
        private void BindProductionAndDownData()
        {
            try
            {
                DateTime fromDate = DateTime.Now.Date;
                DateTime toDate = DateTime.Now.Date;
                fromDate = Util.GetDateTime(txtFromDate.Text.Trim());
                toDate = Util.GetDateTime(txtToDate.Text.Trim());
                DataTable ProductionDownData = null;
                if (Session["ProductionAndDownDataSource"] == null)
                {
                    ProductionDownData = VDGDataBaseAccess.GetProductionDownGridData(fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), ddlMachineId.SelectedValue.ToString());
                    Session["ProductionAndDownDataSource"] = ProductionDownData;
                }
                else
                {
                    ProductionDownData = Session["ProductionAndDownDataSource"] as DataTable;
                }
                if (ProductionDownData.Rows.Count > 0)
                {
                    Session["PageProdDownIndex"] = 0;
                    gvProductionDownData.AllowPaging = true;
                    gvProductionDownData.PageSize = VDGDataBaseAccess.PageSize;
                    gvProductionDownData.DataSource = ProductionDownData;
                    gvProductionDownData.DataBind();
                }
                else
                {
                    gvProductionDownData.DataSource = ProductionDownData;
                    gvProductionDownData.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindProductionAndDownData: {ex.Message}");
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "----------Bind Production Data Gridview ------------"
        private void BindProductionGrid()
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                DateTime fromDate = DateTime.Now.Date;
                DateTime toDate = DateTime.Now.Date;
                fromDate = Util.GetDateTime(txtFromDate.Text.Trim());
                toDate = Util.GetDateTime(txtToDate.Text.Trim());
                //if (!DateTime.TryParseExact(txtFromDate.Text.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDate))
                //{
                //    lblMessages.Text = "Please enter valid from date format.";
                //    return;
                //}
                //if (!DateTime.TryParseExact(txtToDate.Text.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out toDate))
                //{
                //    lblMessages.Text = "Please enter valid to date format.";
                //    return;
                //}
                stopwatch.Start();
                DataTable productionData = null;
                if (Session["proddatasource"] == null)
                {
                    productionData = VDGDataBaseAccess.GetProductionAndDownDataGraph("[s_GetCockpitProductionData_ESHOPX]", fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), ddlMachineId.SelectedValue.ToString());
                    Session["proddatasource"] = productionData;
                }
                else
                {
                    productionData = Session["proddatasource"] as DataTable;
                }


                if (productionData.Rows.Count > 0)
                {
                    gridProductionData.AllowPaging = true;
                    gridProductionData.PageSize = VDGDataBaseAccess.PageSize;
                    gridProductionData.DataSource = productionData;
                    gridProductionData.DataBind();
                    foreach (GridViewRow oItem in gridProductionData.Rows)
                    {
                        string getFirstColValue = oItem.Cells[0].Text;
                        oItem.Attributes.Add("class", "pdrow");
                        oItem.Attributes.Add("id", "pdrow" + getFirstColValue);
                    }
                }
                else
                {
                    gridProductionData.DataSource = productionData;
                    gridProductionData.DataBind();
                }
                stopwatch.Stop();
                Logger.WriteDebugLog("BindProductionGrid : " + stopwatch.Elapsed.TotalSeconds);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Sync Await use both gridview"
        private async Task GetAsync()
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                DateTime fromDate = DateTime.Now.Date;
                DateTime toDate = DateTime.Now.Date;
                fromDate = Util.GetDateTime(txtFromDate.Text.Trim());
                toDate = Util.GetDateTime(txtToDate.Text.Trim());
                Session["FromDate"] = fromDate.ToString("yyyy-MM-dd HH:mm:ss");
                Session["ToDate"] = toDate.ToString("yyyy-MM-dd HH:mm:ss");
                //if (!DateTime.TryParseExact(txtFromDate.Text.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDate))
                //{
                //    lblMessages.Text = "Please enter valid from date format.";
                //    return;
                //}
                //if (!DateTime.TryParseExact(txtToDate.Text.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out toDate))
                //{
                //    lblMessages.Text = "Please enter valid to date format.";
                //    return;
                //}
                stopwatch.Start();
                DataTable productionData = null; DataTable dt = null; DataTable downTimeData = null;
                Task<DataTable> chartViewrDataAsyncTask = null;
                if (Session["proddatasource"] == null || Session["downdatasource"] == null || Session["EfficiencyPeChart"] == null || Session["EfficiencyAeChart"] == null || Session["EfficiencyOEEChart"] == null)
                {
                    var productionDataAsyncTask = VDGDataBaseAccess.GetProductionAndDownData("[s_GetCockpitProductionData_ESHOPX]", fromDate.ToString("yyyy-MM-dd HH:mm:ss"), toDate.ToString("yyyy-MM-dd HH:mm:ss"), ddlMachineId.SelectedValue.ToString());
                    var downTimeDataAsycTask = VDGDataBaseAccess.GetProductionAndDownData("[s_GetCockpitDownData_eshopx]", fromDate.ToString("yyyy-MM-dd HH:mm:ss"), toDate.ToString("yyyy-MM-dd HH:mm:ss"), ddlMachineId.SelectedValue.ToString());
                    if (CheckDateRange())
                    {
                        chartViewrDataAsyncTask = VDGDataBaseAccess.GetProductionAndDownData("s_GetCockpitData_WithTempTable_eshopx", fromDate.ToString("yyyy-MM-dd HH:mm:ss"), toDate.ToString("yyyy-MM-dd HH:mm:ss"), ddlMachineId.SelectedValue.ToString());
                    }
                    else
                    {
                        //chartViewrDataAsyncTask = VDGDataBaseAccess.GetProductionAndDownData("[s_GetCockpitData_eshopx]", fromDate.ToString("yyyy-MM-dd HH:mm:ss"), toDate.ToString("yyyy-MM-dd HH:mm:ss"), ddlMachineId.SelectedValue.ToString());
                        chartViewrDataAsyncTask = VDGDataBaseAccess.GetProductionAndDownData("s_GetCockpitData_WithTempTable_eshopx", fromDate.ToString("yyyy-MM-dd HH:mm:ss"), toDate.ToString("yyyy-MM-dd HH:mm:ss"), ddlMachineId.SelectedValue.ToString());
                    }
                    await Task.WhenAll(productionDataAsyncTask, downTimeDataAsycTask, chartViewrDataAsyncTask).ConfigureAwait(false);
                    dt = chartViewrDataAsyncTask.Result;
                    productionData = productionDataAsyncTask.Result;
                    ChartViewerfromData(dt);
                    downTimeData = downTimeDataAsycTask.Result;
                    Session["downdatasource"] = downTimeDataAsycTask.Result;
                    Session["proddatasource"] = productionDataAsyncTask.Result;
                }
                else
                {
                    productionData = Session["proddatasource"] as DataTable;
                    downTimeData = Session["downdatasource"] as DataTable;
                    ///---------------------Chart Viewer-------------------------------
                    ChartViewerSession();
                }



                if (productionData.Rows.Count > 0)
                {
                    Session["PageProductionIndex"] = 0;
                    gridProductionData.AllowPaging = true;
                    Session["PageSize"] = gridProductionData.PageSize = VDGDataBaseAccess.PageSize;
                    gridProductionData.DataSource = productionData;
                    gridProductionData.DataBind();
                    foreach (GridViewRow oItem in gridProductionData.Rows)
                    {
                        string getFirstColValue = oItem.Cells[0].Text;
                        oItem.Attributes.Add("class", "pdrow");
                        oItem.Attributes.Add("id", "pdrow" + getFirstColValue);
                    }
                }
                else
                {
                    gridProductionData.DataSource = productionData;
                    gridProductionData.DataBind();
                }
                stopwatch.Stop();
                Logger.WriteDebugLog("BindProductionGrid : " + stopwatch.Elapsed.TotalSeconds);

                //-----------------------------------Down Gridview Data--------------------------------------                
                stopwatch.Start();
                if (downTimeData.Rows.Count > 0)
                {
                    Session["PageDownTimeIndex"] = 0;
                    gridDownTimeData.AllowPaging = true;
                    Session["PageSize"] = gridDownTimeData.PageSize = VDGDataBaseAccess.PageSize;
                    gridDownTimeData.DataSource = downTimeData;
                    gridDownTimeData.DataBind();
                    foreach (GridViewRow oItem in gridDownTimeData.Rows)
                    {
                        string getFirstColValue = oItem.Cells[0].Text;
                        oItem.Attributes.Add("class", "dtdrow");
                        oItem.Attributes.Add("id", "dtdrow" + getFirstColValue);
                    }
                }
                else
                {
                    gridDownTimeData.DataSource = downTimeData;
                    gridDownTimeData.DataBind();
                }
                //--------------------------------Bind Chart Viewer Data----------------------


                stopwatch.Stop();
                Logger.WriteDebugLog("BindDownTimeGrid : " + stopwatch.Elapsed.TotalSeconds);

                if (ConfigurationManager.AppSettings["HawkinsPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    BindEventGrid();
                }

                if (ConfigurationManager.AppSettings["ShowProductionandDownDataInVDG"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    BindProductionAndDownData();
                }

            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.Message);
                lblMessages.Text = ex.Message;
            }
        }

        private void ChartViewerfromData(DataTable dt)
        {
            string windowSize = string.Empty;
            int chartSize = 155;
            if (Session["WindowSize"] == null)
                windowSize = "1280";
            else
                windowSize = Session["WindowSize"].ToString();
            if (Convert.ToInt32(windowSize) > 1280)
            {
                chartSize = 195;
            }
            decimal value = decimal.Zero;
            lblTextBox1.Text = dt.Rows[0]["TotalTime"].ToString();//[13].ToString();
            lblTextBox2.Text = dt.Rows[0]["DownTime"].ToString();
            lblTextBox3.Text = dt.Rows[0]["Components"].ToString();
            if (decimal.TryParse(dt.Rows[0]["TurnOver"].ToString(), out value))
                lblTextBox4.Text = Math.Round(value, 2).ToString();
            lblTextBox5.Text = Math.Round(Convert.ToDecimal(dt.Rows[0]["ReturnPerHour"]), 2).ToString();
            lblTextBox6.Text = Math.Round(Convert.ToDecimal(dt.Rows[0]["ReturnPerHourTotal"]), 2).ToString();

            double PEVal = Convert.ToInt64(dt.Rows[0]["ProductionEfficiency"]);
            double AEVal = Convert.ToInt64(dt.Rows[0]["AvailabilityEfficiency"]);
            double OEVal = Convert.ToInt64(dt.Rows[0]["OverAllEfficiency"]);
            double QEVal = Convert.ToInt64(dt.Rows[0]["QualityEfficiency"]);

            PlotEfficiencyChart("PE", chartSize, 98, PEVal);
            PlotEfficiencyChart("AE", chartSize, 98, AEVal);
            PlotEfficiencyChart("OEE", chartSize, 98, OEVal);
            PlotEfficiencyChart("QE", chartSize, 98, QEVal);
            Session["EfficiencyPeChart"] = PEVal;
            Session["EfficiencyAeChart"] = AEVal;
            Session["EfficiencyOEEChart"] = OEVal;
            Session["EfficiencyQEChart"] = QEVal;
        }

        private void ChartViewerSession()
        {
            string windowSize = string.Empty;
            int chartSize = 155;
            if (Session["WindowSize"] == null)
                windowSize = "1280";
            else
                windowSize = Session["WindowSize"].ToString();
            if (Convert.ToInt32(windowSize) > 1280)
            {
                chartSize = 195;
            }
            double PEVal = Convert.ToInt64(Session["EfficiencyPeChart"]);
            double AEVal = Convert.ToInt64(Session["EfficiencyAeChart"]);
            double OEVal = Convert.ToInt64(Session["EfficiencyOEEChart"]);
            double QEVal = Convert.ToInt64(Session["EfficiencyQEChart"]);
            PlotEfficiencyChart("PE", chartSize, 98, PEVal);
            PlotEfficiencyChart("AE", chartSize, 98, AEVal);
            PlotEfficiencyChart("OEE", chartSize, 98, OEVal);
            PlotEfficiencyChart("QE", chartSize, 98, QEVal);
        }
        #endregion

        #region "--------Bind Down Time Data Gridview --------"
        private void BindDownTimeGrid()
        {
            Stopwatch stopwatch = new Stopwatch();
            try
            {

                DateTime fromDate = DateTime.Now.Date;
                DateTime toDate = DateTime.Now.Date;
                fromDate = Util.GetDateTime(txtFromDate.Text.Trim());
                toDate = Util.GetDateTime(txtToDate.Text.Trim());
                //if (!DateTime.TryParseExact(txtFromDate.Text.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDate))
                //{
                //    lblMessages.Text = "Please enter valid from date format.";
                //    return;
                //}
                //if (!DateTime.TryParseExact(txtToDate.Text.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out toDate))
                //{
                //    lblMessages.Text = "Please enter valid to date format.";
                //    return;
                //}
                DataTable downTimeData = null;
                stopwatch.Start();
                if (Session["downdatasource"] == null)
                {
                    downTimeData = VDGDataBaseAccess.GetProductionAndDownDataGraph("[s_GetCockpitDownData_eshopx]", fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), ddlMachineId.SelectedValue.ToString());
                    Session["downdatasource"] = downTimeData;
                }
                else
                {
                    downTimeData = Session["downdatasource"] as DataTable;
                }

                if (downTimeData.Rows.Count > 0)
                {
                    gridDownTimeData.AllowPaging = true;
                    gridDownTimeData.PageSize = VDGDataBaseAccess.PageSize;
                    gridDownTimeData.DataSource = downTimeData;
                    gridDownTimeData.DataBind();
                    foreach (GridViewRow oItem in gridDownTimeData.Rows)
                    {
                        string getFirstColValue = oItem.Cells[0].Text;
                        oItem.Attributes.Add("class", "dtdrow");
                        oItem.Attributes.Add("id", "dtdrow" + getFirstColValue);
                    }
                }
                else
                {
                    gridDownTimeData.DataSource = downTimeData;
                    gridDownTimeData.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }

            stopwatch.Stop();
            Logger.WriteDebugLog("BindDownTimeGrid : " + stopwatch.Elapsed.TotalSeconds);
        }
        #endregion

        #region "Bind Plant,Cell,Machine ID"
        private void BindPlantID()
        {
            try
            {
                List<string> list = VDGDataBaseAccess.GetPlantID_VDG();
                list.Insert(0, "ALL");
                ddlPlantID.DataSource = list;
                ddlPlantID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindCellID()
        {
            try
            {
                List<string> list = VDGDataBaseAccess.GetCellID_VDG(ddlPlantID.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantID.SelectedValue.ToString());
                list.Insert(0, "ALL");
                ddlCellID.DataSource = list;
                ddlCellID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindMachines()
        {
            try
            {
                Logger.WriteDebugLog("Loading machines");
                //var allMachineName = VDGDataBaseAccess.GetAllMachines("All");
                var allMachineName = VDGDataBaseAccess.GetMachineID_VDG(ddlPlantID.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantID.SelectedValue.ToString(), ddlCellID.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : ddlCellID.SelectedValue.ToString());
                Logger.WriteDebugLog("Loaded machines = " + allMachineName.Count.ToString());
                if (allMachineName != null && allMachineName.Count > 0)
                {
                    ddlMachineId.DataSource = allMachineName;
                    ddlMachineId.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Plot Efficiency Chart-----------"
        public void PlotEfficiencyChart(string chartName, int wt, int ht, double value)
        {
            try
            {
                AngularMeter m = new AngularMeter(wt, ht - 0, 0xE7E7E7, 0x404040);
                m.setMeterColors(0x404040, 0x000000, 0x404040);
                m.setMeter(wt / 2, ht - 10, 75, -90, 90);
                m.setRoundedFrame(0x404040, 5);
                m.setScale(0, 100, 20, 10, 1);

                m.addZone(50, 100, 0x32CD32); // 50 - 100 as red green 
                m.addZone(30, 50, 0xFFFF00); // 30 - 50 as yellow zone 
                m.addZone(0, 30, 0xFF6347); //  00 - 30 as red zone

                m.setLineWidth(0, 2, 0);
                m.addText(wt - 20, 11, m.formatValue(value, "0") + "%", "Arial Bold", 9.0F, 0xffffff, Chart.Center, 360).setBackground(0x404040, 0, -1);
                if (value > 100) value = 100;
                // Add a semi-transparent black (80000000) pointer at the specified value
                m.addPointer(value, unchecked((int)0xF3F3F3), 808080);

                m.addText(wt / 2, 70, chartName, "Arial Bold", 9.0F, 0x000000, Chart.Center, 360);
                m.addRing(0, 0, 0xffffff);
                m.setLabelStyle("Arial Bold", 8, 0x000000);

                if (chartName.Equals("PE")) PEChart.Image = m.makeWebImage(Chart.PNG);
                else if (chartName.Equals("AE")) AEChart.Image = m.makeWebImage(Chart.PNG);
                else if (chartName.Equals("OEE")) OEChart.Image = m.makeWebImage(Chart.PNG);
                else if (chartName.Equals("QE")) QEChart.Image = m.makeWebImage(Chart.PNG);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        private bool CheckDateRange()
        {
            bool isDateInRange = false;
            try
            {
                DateTime dt1 = Util.GetDateTime(txtFromDate.Text);
                DateTime dt2 = Util.GetDateTime(txtToDate.Text);

                var hours = (dt2 - dt1).TotalHours;
                if (Math.Abs(hours) <= 48)
                {
                    isDateInRange = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
            return isDateInRange;
        }

        #region "Get Production Data Graph"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ChartData<SeriesStackVDG> GetProductionDataGraph(string machineId, string strfromDate, string strtoDate, string windowSize)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var ChartData = new ChartData<SeriesStackVDG>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = "XAxisTitle",
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };
            HttpContext.Current.Session["WindowSize"] = windowSize;
            DateTime fromDate = DateTime.Now.Date;
            DateTime toDate = DateTime.Now.Date;
            fromDate = Util.GetDateTime(strfromDate.Trim());
            toDate = Util.GetDateTime(strtoDate.Trim());
            //if (!DateTime.TryParseExact(strfromDate.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDate))
            //{
            //    return new ChartData<SeriesStack>();
            //}
            //if (!DateTime.TryParseExact(strtoDate.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out toDate))
            //{
            //    return new ChartData<SeriesStack>();
            //}
            //fromDate = DateTime.Parse(strfromDate.Trim(), new CultureInfo("en-GB"));
            //toDate = DateTime.Parse(strtoDate.Trim(), new CultureInfo("en-GB"));
            DataTable Tempdt = new DataTable();
            DataTable dt = new DataTable();
            if (HttpContext.Current.Session["proddatasource"] == null)
                Tempdt = VDGDataBaseAccess.GetProductionAndDownDataGraph("[s_GetCockpitProductionData_ESHOPX]", fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), machineId);
            else
                Tempdt = HttpContext.Current.Session["proddatasource"] as DataTable;

            if (Tempdt.Rows.Count > 0)
                dt = Tempdt.AsEnumerable().Where(x => x.Field<string>("Remarks") != "In Progress Cycle").Distinct().Any() ? Tempdt.AsEnumerable().Where(x => x.Field<string>("Remarks") != "In Progress Cycle").Distinct().CopyToDataTable() : new DataTable();

            ChartData.series = new List<SeriesStackVDG>();
            ChartData.categories1 = new List<int>();
            ChartData.series.Add(new SeriesStackVDG { name = HttpContext.GetLocalResourceObject("~/VDGScreen.aspx", "StdLoadUnload").ToString(), type = "spline", data = new List<decimal?>() });
            ChartData.series.Add(new SeriesStackVDG { name = HttpContext.GetLocalResourceObject("~/VDGScreen.aspx", "StdCycleTime").ToString(), type = "spline", data = new List<decimal?>() });
            ChartData.series.Add(new SeriesStackVDG { name = "Std. Cycle Time", type = "spline", data = new List<decimal?>() });
            ChartData.series.Add(new SeriesStackVDG { name = HttpContext.GetLocalResourceObject("~/VDGScreen.aspx", "ActLoadUnload").ToString(), type = "column", data = new List<decimal?>() });
            ChartData.series.Add(new SeriesStackVDG { name = HttpContext.GetLocalResourceObject("~/VDGScreen.aspx", "ActCycleTime").ToString(), type = "column", data = new List<decimal?>() });
            ChartData.series.Add(new SeriesStackVDG { name = "Act. Cycle Time", type = "column", data = new List<decimal?>() });
            int count = 1;

            var pageSize = HttpContext.Current.Session["PageSize"] == null ? VDGDataBaseAccess.PageSize : int.Parse(HttpContext.Current.Session["PageSize"].ToString());
            int pageIndex = HttpContext.Current.Session["PageProductionIndex"] == null ? 0 : int.Parse(HttpContext.Current.Session["PageProductionIndex"].ToString());
            IEnumerable<DataRow> dtRows = dt.AsEnumerable().Skip(pageSize * pageIndex).Take(pageSize);
            count = pageSize * pageIndex + 1;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow datarow in dtRows)
                {
                    decimal value = decimal.Zero;
                    ChartData.categories1.Add(count);
                    if (decimal.TryParse(datarow["stdlLoadUnloadTime"].ToString(), out value))
                        ChartData.series[0].data.Add(value);
                    if (decimal.TryParse(datarow["stdMcTime"].ToString(), out value))
                        ChartData.series[1].data.Add(value);
                    if (decimal.TryParse(datarow["stdTotalTime"].ToString(), out value))
                        ChartData.series[2].data.Add(value);
                    if (decimal.TryParse(datarow["actLoadUnloadTime"].ToString(), out value))
                        ChartData.series[3].data.Add(value);
                    if (decimal.TryParse(datarow["actMcTime"].ToString(), out value))
                        ChartData.series[4].data.Add(value);
                    if (decimal.TryParse(datarow["actTotalTime"].ToString(), out value))
                        ChartData.series[5].data.Add(value);
                    count++;
                }
            }
            for (int i = dt.Rows.Count; i < 20; i++)
            {
                decimal? value = null;
                ChartData.categories1.Add(count);
                ChartData.series[0].data.Add(value);
                ChartData.series[1].data.Add(value);
                ChartData.series[2].data.Add(value);
                ChartData.series[3].data.Add(value);
                ChartData.series[4].data.Add(value);
                ChartData.series[5].data.Add(value);
                count++;
            }
            stopwatch.Stop();
            Logger.WriteDebugLog("GetProductionDataGraph : " + stopwatch.Elapsed.TotalSeconds);
            return ChartData;
        }
        #endregion

        #region "Get Down Time Data Graph"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ChartData<SeriesStackVDGDown> GetDownTimeDataGraph(string machineId, string strfromDate, string strtoDate)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var ChartData = new ChartData<SeriesStackVDGDown>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = "XAxisTitle",
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };
            DateTime fromDate = DateTime.Now.Date;
            DateTime toDate = DateTime.Now.Date;
            fromDate = Util.GetDateTime(strfromDate.Trim());
            toDate = Util.GetDateTime(strtoDate.Trim());
            //if (!DateTime.TryParseExact(strfromDate.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDate))
            //{
            //    return new ChartData<SeriesStack>();
            //}
            //if (!DateTime.TryParseExact(strtoDate.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out toDate))
            //{
            //    return new ChartData<SeriesStack>();
            //}
            DataTable dt = null;
            if (HttpContext.Current.Session["downdatasource"] == null)
                dt = VDGDataBaseAccess.GetProductionAndDownDataGraph("[s_GetCockpitDownData_eshopx]", fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), machineId);
            else
                dt = HttpContext.Current.Session["downdatasource"] as DataTable;

            int count = 1;
            var pageSize = HttpContext.Current.Session["PageSize"] == null ? VDGDataBaseAccess.PageSize : int.Parse(HttpContext.Current.Session["PageSize"].ToString());
            int pageIndex = HttpContext.Current.Session["PageDownTimeIndex"] == null ? 0 : int.Parse(HttpContext.Current.Session["PageDownTimeIndex"].ToString());
            IEnumerable<DataRow> dtRows = dt.AsEnumerable().Skip(pageSize * pageIndex).Take(pageSize);
            count = pageSize * pageIndex + 1;
            ChartData.series = new List<SeriesStackVDGDown>();
            ChartData.categories1 = new List<int>();
            ChartData.series.Add(new SeriesStackVDGDown { name = HttpContext.GetLocalResourceObject("~/VDGScreen.aspx", "DownTime").ToString(), type = "column", data = new List<SeriesStackVDGDownData>(), color = "#FF0000" });
            ChartData.series.Add(new SeriesStackVDGDown { name = HttpContext.GetLocalResourceObject("~/VDGScreen.aspx", "PDT").ToString(), type = "column", data = new List<SeriesStackVDGDownData>(), color = "#FF40FF" }); ;
            ChartData.series.Add(new SeriesStackVDGDown { name = "No_Data", type = "column", data = new List<SeriesStackVDGDownData>(), color = "#2076BF" });

            if (dt != null && dt.Rows.Count > 0)
            {
                int chartXAxisValue = 0;
                foreach (DataRow datarow in dtRows)
                {
                    decimal value = decimal.Zero;
                    ChartData.categories1.Add(count);
                    if (decimal.TryParse(datarow["DownTimeForGraph"].ToString(), out value))
                    {
                        SeriesStackVDGDownData downChartData = new SeriesStackVDGDownData();
                        downChartData.y = value;
                        downChartData.x = chartXAxisValue;
                        if (isPageEnabled("CumiPages"))
                        {
                            if (datarow["DownID"].ToString().Equals("NO_DATA", StringComparison.OrdinalIgnoreCase))
                            {
                                ChartData.series[2].data.Add(downChartData);
                            }
                            else
                            {
                                ChartData.series[0].data.Add(downChartData);
                            }
                        }
                        else
                        {
                            ChartData.series[0].data.Add(downChartData);
                        }

                    }
                    if (decimal.TryParse(datarow["PDTForGraph"].ToString(), out value))
                    {
                        SeriesStackVDGDownData downChartData = new SeriesStackVDGDownData();
                        downChartData.y = value;
                        downChartData.x = chartXAxisValue;
                        ChartData.series[1].data.Add(downChartData);
                    }
                    count++;
                    chartXAxisValue++;
                }
            }
            for (int i = dt.Rows.Count; i < 20; i++)
            {
                try
                {
                    decimal? value = null;
                    ChartData.categories1.Add(count);
                    SeriesStackVDGDownData downChartData = new SeriesStackVDGDownData();
                    downChartData.y = value;
                    ChartData.series[0].data.Add(downChartData);
                    ChartData.series[1].data.Add(downChartData);
                    ChartData.series[2].data.Add(downChartData);
                    count++;
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.Message);
                }

            }
            if (!isPageEnabled("CumiPages"))
            {
                ChartData.series.RemoveAt(2);
            }
            stopwatch.Stop();
            Logger.WriteDebugLog("GetDownTimeDataGraph : " + stopwatch.Elapsed.TotalSeconds);
            return ChartData;
        }
        //public static ChartData<SeriesStackVDGDown> GetDownTimeDataGraph(string machineId, string strfromDate, string strtoDate)
        //{
        //    Stopwatch stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    var ChartData = new ChartData<SeriesStackVDGDown>
        //    {
        //        Title = "TITLE",
        //        Subtitle = "SubTitle",
        //        XAxisTitle = "XAxisTitle",
        //        YAxisTitle = "YAxisTitle",
        //        YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
        //    };
        //    DateTime fromDate = DateTime.Now.Date;
        //    DateTime toDate = DateTime.Now.Date;
        //    fromDate = Util.GetDateTime(strfromDate.Trim());
        //    toDate = Util.GetDateTime(strtoDate.Trim());
        //    //if (!DateTime.TryParseExact(strfromDate.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDate))
        //    //{
        //    //    return new ChartData<SeriesStack>();
        //    //}
        //    //if (!DateTime.TryParseExact(strtoDate.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out toDate))
        //    //{
        //    //    return new ChartData<SeriesStack>();
        //    //}
        //    DataTable dt = null;
        //    if (HttpContext.Current.Session["downdatasource"] == null)
        //        dt = VDGDataBaseAccess.GetProductionAndDownDataGraph("[s_GetCockpitDownData_eshopx]", fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), machineId);
        //    else
        //        dt = HttpContext.Current.Session["downdatasource"] as DataTable;

        //    int count = 1;
        //    var pageSize = HttpContext.Current.Session["PageSize"] == null ? VDGDataBaseAccess.PageSize : int.Parse(HttpContext.Current.Session["PageSize"].ToString());
        //    int pageIndex = HttpContext.Current.Session["PageDownTimeIndex"] == null ? 0 : int.Parse(HttpContext.Current.Session["PageDownTimeIndex"].ToString());
        //    IEnumerable<DataRow> dtRows = dt.AsEnumerable().Skip(pageSize * pageIndex).Take(pageSize);
        //    count = pageSize * pageIndex + 1;
        //    ChartData.series = new List<SeriesStackVDGDown>();
        //    ChartData.categories1 = new List<int>();
        //    ChartData.series.Add(new SeriesStackVDGDown { name = HttpContext.GetLocalResourceObject("~/VDGScreen.aspx", "DownTime").ToString(), type = "column", data = new List<SeriesStackVDGDownData>() });
        //    ChartData.series.Add(new SeriesStackVDGDown { name = HttpContext.GetLocalResourceObject("~/VDGScreen.aspx", "PDT").ToString(), type = "column", data = new List<SeriesStackVDGDownData>() });

        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        foreach (DataRow datarow in dtRows)
        //        {
        //            decimal value = decimal.Zero;
        //            ChartData.categories1.Add(count);
        //            if (decimal.TryParse(datarow["DownTimeForGraph"].ToString(), out value))
        //            {
        //                SeriesStackVDGDownData downChartData = new SeriesStackVDGDownData();
        //                downChartData.y = value;
        //                if (isPageEnabled("CumiPages"))
        //                {
        //                    if (datarow["DownID"].ToString().Equals("NO_DATA", StringComparison.OrdinalIgnoreCase))
        //                    {
        //                        downChartData.color = "#2076BF";
        //                    }
        //                    else
        //                    {
        //                        downChartData.color = "#FF0000";
        //                    }
        //                }
        //                else
        //                {
        //                    downChartData.color = "#FF0000";
        //                }
        //                ChartData.series[0].data.Add(downChartData);
        //            }
        //            if (decimal.TryParse(datarow["PDTForGraph"].ToString(), out value))
        //            {
        //                SeriesStackVDGDownData downChartData = new SeriesStackVDGDownData();
        //                downChartData.y = value;
        //                downChartData.color = "#FF40FF";
        //                ChartData.series[1].data.Add(downChartData);
        //            }
        //            count++;
        //        }
        //    }
        //    for (int i = dt.Rows.Count; i < 20; i++)
        //    {
        //        try
        //        {
        //            decimal? value = null;
        //            ChartData.categories1.Add(count);
        //            SeriesStackVDGDownData downChartData = new SeriesStackVDGDownData();
        //            downChartData.y = value;
        //            ChartData.series[0].data.Add(downChartData);
        //            ChartData.series[1].data.Add(downChartData);
        //            count++;
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.WriteErrorLog(ex.Message);
        //        }

        //    }
        //    stopwatch.Stop();
        //    Logger.WriteDebugLog("GetDownTimeDataGraph : " + stopwatch.Elapsed.TotalSeconds);
        //    return ChartData;
        //}
        #endregion

        #region "Bind Component Data"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> BindComponentData(string machineId, string strfromDate, string strtoDate)
        {
            List<string> list = null;
            try
            {
                DateTime fromDate = DateTime.Now.Date;
                DateTime toDate = DateTime.Now.Date;
                fromDate = Util.GetDateTime(strfromDate.Trim());
                toDate = Util.GetDateTime(strtoDate.Trim());
                //if (!DateTime.TryParseExact(strfromDate.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDate))
                //{
                //    return new List<string>();
                //}
                //if (!DateTime.TryParseExact(strtoDate.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out toDate))
                //{
                //    return new List<string>();
                //}
                list = VDGDataBaseAccess.GetComponentOperationForMachine(fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), machineId.ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return list;
        }
        #endregion

        #region "Get Component Performance Data "
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static VDGComponentValues ComponentPerformanceData(string machineId, string strfromDate, string strtoDate, string component)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            VDGComponentValues vals = new VDGComponentValues();
            try
            {
                DateTime fromDate = DateTime.Now.Date;
                DateTime toDate = DateTime.Now.Date;
                fromDate = Util.GetDateTime(strfromDate.Trim());
                toDate = Util.GetDateTime(strtoDate.Trim());
                //if (!DateTime.TryParseExact(strfromDate.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDate))
                //{
                //    return new VDGComponentValues();
                //}
                //if (!DateTime.TryParseExact(strtoDate.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out toDate))
                //{
                //    return new VDGComponentValues();
                //}

                if (!string.IsNullOrEmpty(component))
                {
                    string[] arr = component.Split('|');
                    if (arr[0] != "null")
                    {
                        vals = VDGDataBaseAccess.GetCockpitComponentData(fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), arr[0].ToString(), arr[1].ToString(), machineId.ToString());
                        vals.SpeedRatio = CalculateSpeedLoadRatio(vals.StCycleTime, vals.AvgCycleTime).ToString();
                        vals.LoadRatio = CalculateSpeedLoadRatio(vals.StLoadTime, vals.AvgLoadTime).ToString();
                    }
                }
                stopwatch.Stop();
                Logger.WriteDebugLog("ComponentPerformanceData : " + stopwatch.Elapsed.TotalSeconds);

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return vals;
        }
        #endregion

        #region "Get Update Std Times"

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetUpdateStdTimes(string machineId, string componentId)
        {
            List<string> list = new List<string>();
            try
            {
                string TimeFormatVal = (VDGDataBaseAccess.GetShopdefaultsTimeFormat()).ToLower();
                string[] componentVal = componentId.Split('|');
                string component = componentVal[0].Trim();
                string operation = componentVal[1].Trim();
                string Machiningtime = string.Empty;
                string CycleTime = string.Empty;
                Double standardCycleTime = 0;
                Double standardLoad = 0;


                if (!string.IsNullOrEmpty(TimeFormatVal))
                {
                    if (TimeFormatVal == ("ss"))
                    {
                        list.Add("Standard Time (in seconds)");
                        VDGDataBaseAccess.GetAllStdTimes(machineId, component, operation, out CycleTime, out Machiningtime);
                        standardLoad = Math.Round((Convert.ToDouble(CycleTime) - Convert.ToDouble(Machiningtime)), 3);
                        standardCycleTime = Math.Round(Convert.ToDouble(Machiningtime), 3);
                        list.Add(standardLoad.ToString());
                        list.Add(standardCycleTime.ToString());
                    }
                    else
                    {
                        list.Add("Standard Time (in minutes)");
                        VDGDataBaseAccess.GetAllStdTimes(machineId, component, operation, out CycleTime, out Machiningtime);
                        standardLoad = Math.Round((Convert.ToDouble(CycleTime) - Convert.ToDouble(Machiningtime)), 3);
                        standardCycleTime = Math.Round((Convert.ToDouble(Machiningtime)), 3);
                        if (standardLoad > 0)
                        {
                            standardLoad = Math.Round(standardLoad / 60.0, 3);
                        }
                        if (standardCycleTime > 0)
                        {
                            standardCycleTime = Math.Round(standardCycleTime / 60.0, 3);
                        }
                        list.Add(standardLoad.ToString());
                        list.Add(standardCycleTime.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return list;
        }
        #endregion

        #region "Update Standard Time Data"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string updateStandardTimeData(string machineId, string componentId, string stdCycleTime, string stdLoadTime, bool updateAllMachines)
        {
            double cycle = 0;
            double loadUnload = 0, stdloadunload = 0;
            string IsSuccessfull = string.Empty;
            string TimeFormatVal = (VDGDataBaseAccess.GetShopdefaultsTimeFormat()).ToLower();
            string[] componentVal = componentId.Split('|');
            string component = componentVal[0].Trim();
            string operation = componentVal[1].Trim();

            #region CycleTime
            if (!string.IsNullOrEmpty(TimeFormatVal))
            {
                if (TimeFormatVal == ("ss"))
                    cycle = Convert.ToDouble(stdCycleTime.ToString());
                else
                    cycle = Convert.ToDouble(stdCycleTime.ToString()) * 60;
            }
            #endregion

            #region LoadUnload
            if (!string.IsNullOrEmpty(TimeFormatVal))
            {
                if (TimeFormatVal == ("ss"))
                    loadUnload = Convert.ToDouble(stdLoadTime.ToString());
                else
                    loadUnload = Convert.ToDouble(stdLoadTime.ToString()) * 60;
            }
            #endregion

            stdloadunload = cycle + loadUnload;
            VDGDataBaseAccess.UpdateAllStdTimes(cycle.ToString(), stdloadunload.ToString(), machineId.ToString(), component, operation, updateAllMachines, out IsSuccessfull);
            return IsSuccessfull;
        }
        #endregion

        #region "----------Page Index Change in gridview ----------"
        protected void gridProductionData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            hdfValueMange.Value = "";
            RegisterAsyncTask(new PageAsyncTask(AsyncChartViewerData));//SetMachineDataValues();
            Session["PageProductionIndex"] = gridProductionData.PageIndex = e.NewPageIndex;
            gridProductionData.DataBind();
            BindProductionGrid();
            hdfPaging.Value = "ProductionPaging";
        }
        #endregion

        #region "------------Page Inddexing in Down Time Data--------"
        protected void gridDownTimeData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            hdfValueMange.Value = "";
            RegisterAsyncTask(new PageAsyncTask(AsyncChartViewerData));//SetMachineDataValues();
            Session["PageDownTimeIndex"] = gridDownTimeData.PageIndex = e.NewPageIndex;
            gridDownTimeData.DataBind();
            BindDownTimeGrid();
            hdfPaging.Value = "DownTimePaging";
        }
        #endregion

        protected void gridProductionData_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                hdfPaging.Value = "";
                hdfValueMange.Value = "";
                string SortDir = string.Empty;
                //GridColumnSettings();
                RegisterAsyncTask(new PageAsyncTask(AsyncChartViewerData));//SetMachineDataValues();
                //GetDetailsSecondTime();
                DataTable productionData = Session["proddatasource"] as DataTable;
                if (productionData == null)
                {
                    BindProductionGrid();
                    productionData = Session["proddatasource"] as DataTable;
                }
                if (productionShorting == SortDirection.Ascending)
                {
                    productionShorting = SortDirection.Descending;
                    SortDir = "Desc";
                }
                else
                {
                    productionShorting = SortDirection.Ascending;
                    SortDir = "Asc";
                }
                if (productionData != null && productionData.Rows.Count > 0)
                {
                    DataView sortedView = new DataView(productionData);
                    sortedView.Sort = e.SortExpression + " " + SortDir;
                    gridProductionData.DataSource = sortedView;
                    gridProductionData.DataBind();
                    int id = 1;
                    foreach (GridViewRow oItem in gridProductionData.Rows)
                    {
                        string getFirstColValue = oItem.Cells[0].Text;
                        oItem.Attributes.Add("class", "pdrow");
                        oItem.Attributes.Add("id", "pdrow" + getFirstColValue);// id.ToString());
                        id++;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }

        }
        public SortDirection eventShorting
        {
            get
            {
                if (Session["eventdatasourceSort"] == null)
                {
                    Session["eventdatasourceSort"] = SortDirection.Ascending;
                }
                return (SortDirection)Session["eventdatasourceSort"];
            }
            set
            {
                Session["eventdatasourceSort"] = value;
            }
        }
        public SortDirection ProductionAndDownSorting
        {
            get
            {
                if (Session["ProductionAndDownDataSourceSort"] == null)
                {
                    Session["ProductionAndDownDataSourceSort"] = SortDirection.Ascending;
                }
                return (SortDirection)Session["ProductionAndDownDataSourceSort"];
            }
            set
            {
                Session["ProductionAndDownDataSourceSort"] = value;
            }
        }

        public SortDirection productionShorting
        {
            get
            {
                if (Session["proddatasourceSort"] == null)
                {
                    Session["proddatasourceSort"] = SortDirection.Ascending;
                }
                return (SortDirection)Session["proddatasourceSort"];
            }
            set
            {
                Session["proddatasourceSort"] = value;
            }
        }

        public SortDirection downTimeShorting
        {
            get
            {
                if (Session["downdatasourceSort"] == null)
                {
                    Session["downdatasourceSort"] = SortDirection.Ascending;
                }
                return (SortDirection)Session["downdatasourceSort"];
            }
            set
            {
                Session["downdatasourceSort"] = value;
            }
        }

        protected void gridDownTimeData_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                hdfPaging.Value = "";
                hdfValueMange.Value = "";
                string SortDir = string.Empty;
                //GridColumnSettings();
                RegisterAsyncTask(new PageAsyncTask(AsyncChartViewerData));//SetMachineDataValues();
                //GetDetailsSecondTime();
                DataTable downTimeData = Session["downdatasource"] as DataTable;
                if (downTimeData == null)
                {
                    BindDownTimeGrid();
                    downTimeData = Session["downdatasource"] as DataTable;
                }
                if (downTimeShorting == SortDirection.Ascending)
                {
                    downTimeShorting = SortDirection.Descending;
                    SortDir = "Desc";
                }
                else
                {
                    downTimeShorting = SortDirection.Ascending;
                    SortDir = "Asc";
                }
                if (downTimeData != null && downTimeData.Rows.Count > 0)
                {
                    DataView sortedView = new DataView(downTimeData);
                    sortedView.Sort = e.SortExpression + " " + SortDir;
                    gridDownTimeData.DataSource = sortedView;
                    gridDownTimeData.DataBind();
                    int id = 1;
                    foreach (GridViewRow oItem in gridDownTimeData.Rows)
                    {
                        string getFirstColValue = oItem.Cells[0].Text;
                        oItem.Attributes.Add("class", "dtdrow");
                        oItem.Attributes.Add("id", "dtdrow" + getFirstColValue);
                        id++;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnOkRemarks_Click(object sender, EventArgs e)
        {
            bool IsRemarksUpdated = false;
            try
            {
                VDGDataBaseAccess.UpdateRemarks(txtRemarksArea.Text.Trim(), txtActionTaken.Text.Trim(), hdfRemarks.Value.ToString(), out IsRemarksUpdated);
                lblMessages.Text = GetLocalResourceObject("RemarksUpdatedSuccessfullyforID").ToString() + " - " + hdfRemarks.Value.ToString();
                if (hdfValue.Value.ToString().Equals("Down Time Data", StringComparison.OrdinalIgnoreCase))
                {
                    Session["downdatasource"] = null;
                    BindDownTimeGrid();
                }
                else if (hdfValue.Value.ToString().Equals("Production Data", StringComparison.OrdinalIgnoreCase))
                {
                    Session["proddatasource"] = null;
                    BindProductionGrid();
                }
                hdfPaging.Value = "";
                hdfValueMange.Value = "";
                RegisterAsyncTask(new PageAsyncTask(AsyncChartViewerData));// SetMachineDataValues();
                lblMessages.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        protected void Export_Data_Click(object sender, EventArgs e)
        {
            try
            {
                string ComponentVal = string.Empty;
                string OperationVal = string.Empty;
                string reportName = string.Empty;
                hdfValueMange.Value = "";
                hdfPaging.Value = "";
                DataTable dt = null;
                List<string> columnNames = new List<string>();
                DateTime fromDate = DateTime.Now.Date;
                DateTime toDate = DateTime.Now.Date;
                List<ColumnViewSetting> colHeader = new List<ColumnViewSetting>();
                List<string> colText = new List<string>();
                fromDate = Util.GetDateTime(txtFromDate.Text.Trim());
                toDate = Util.GetDateTime(txtToDate.Text.Trim());
                //if (!DateTime.TryParseExact(txtFromDate.Text.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDate))
                //{
                //    lblMessages.Text = "Please enter valid from date format.";
                //    return;
                //}
                //if (!DateTime.TryParseExact(txtToDate.Text.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt", "dd-MMM-yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out toDate))
                //{
                //    lblMessages.Text = "Please enter valid to date format.";
                //    return;
                //}

                if (hdfValue.Value == "Production Data")
                {
                    //for (int i = 0; i < gridProductionData.Columns.Count; i++)
                    //{
                    //    columnNames.Add(gridProductionData.Columns[i].HeaderText);
                    //}
                    if (Session["HeaderProdGrid"] == null)
                    {
                        Session["HeaderProdGrid"] = DataBaseAccess.BindSettingPage("WebTPMTrakVDGProduction", Session["Language"] == null ? "en" : Session["Language"].ToString());
                        colHeader = Session["HeaderProdGrid"] as List<ColumnViewSetting>;
                    }
                    else
                    {
                        colHeader = Session["HeaderProdGrid"] as List<ColumnViewSetting>;
                    }
                    foreach (var item in colHeader)
                    {
                        if (item.ValueInBool)
                        {
                            if (System.Web.Configuration.WebConfigurationManager.AppSettings["ShantiIronPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                            {
                                if (!ddlMachineId.SelectedValue.Equals("Leak Test Machine", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (item.ValueInText == "LeakTestRemarks" || item.ValueInText == "Result")
                                    {
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                if (item.ValueInText == "LeakTestRemarks" || item.ValueInText == "Result")
                                {
                                    continue;
                                }
                            }
                            if (item.ValueInText == "InputCode" || item.ValueInText == "OutputCode")
                            {
                                if ((System.Web.Configuration.WebConfigurationManager.AppSettings["PittiPages"].ToString().Equals("0", StringComparison.OrdinalIgnoreCase)))
                                    continue;
                            }
                            columnNames.Add(item.ValueInText2);
                            colText.Add(item.ValueInText);
                        }
                    }
                    reportName = hdfValue.Value;
                    //  dt = gridProductionData.DataSource as DataTable;
                    dt = Session["proddatasource"] as DataTable;
                    if (dt == null)
                    {
                        BindProductionGrid();
                        dt = Session["proddatasource"] as DataTable;
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string rpt = ExportExcelEpplus.VDGReportGenerate(columnNames, dt, fromDate.ToString("dd-MM-yyyy HH:mm:ss"), toDate.ToString("dd-MM-yyyy HH:mm:ss"), ddlMachineId.SelectedValue.ToString(), reportName, colText);
                        //rpt.WriteGridDataToExcel();
                    }
                    else
                    {
                        // ErrorSignal.FromCurrentContext().Raise(ex);
                        lblMessages.Text = GetLocalResourceObject("NoDatatoExport").ToString();
                    }

                }
                else if (hdfValue.Value == "Down Time Data")
                {
                    //for (int i = 0; i < gridDownTimeData.Columns.Count; i++)
                    //{
                    //    columnNames.Add(gridDownTimeData.Columns[i].HeaderText);
                    //}
                    if (Session["HeaderDownGrid"] == null)
                    {
                        Session["HeaderDownGrid"] = DataBaseAccess.BindSettingPage("WebTPMTrakVDGDownTime", Session["Language"] == null ? "en" : Session["Language"].ToString());
                        colHeader = Session["HeaderDownGrid"] as List<ColumnViewSetting>;
                    }
                    else
                    {
                        colHeader = Session["HeaderDownGrid"] as List<ColumnViewSetting>;
                    }
                    foreach (var item in colHeader)
                    {
                        if (item.ValueInBool)
                        {
                            if (item.ValueInText == "InputCode" || item.ValueInText == "OutputCode")
                            {
                                if ((System.Web.Configuration.WebConfigurationManager.AppSettings["PittiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase)))
                                {
                                    columnNames.Add(item.ValueInText2);
                                    colText.Add(item.ValueInText);
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                columnNames.Add(item.ValueInText2);
                                colText.Add(item.ValueInText);
                            }
                        }
                    }
                    reportName = hdfValue.Value;
                    // dt = gridDownTimeData.DataSource as DataTable;
                    dt = Session["downdatasource"] as DataTable;
                    if (dt == null)
                    {
                        BindDownTimeGrid();
                        dt = Session["downdatasource"] as DataTable;
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string rpt = ExportExcelEpplus.VDGReportGenerate(columnNames, dt, fromDate.ToString("dd-MM-yyyy HH:mm:ss"), toDate.ToString("dd-MM-yyyy HH:mm:ss"), ddlMachineId.SelectedValue.ToString(), reportName, colText);
                        //rpt.WriteGridDataToExcel();
                    }
                    else
                    {
                        // ErrorSignal.FromCurrentContext().Raise(ex);
                        lblMessages.Text = GetLocalResourceObject("NoDatatoExport").ToString();
                    }

                }
                else if (hdfValue.Value == "Event Data")
                {

                    dt = Session["eventdatasource"] as DataTable;
                    if (dt == null)
                    {
                        BindEventGrid();
                        dt = Session["eventdatasource"] as DataTable;
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string report = ExportExcelEpplus.VDGEventReportGenerate_Hawkins(dt, fromDate.ToString("dd-MM-yyyy HH:mm:ss"), toDate.ToString("dd-MM-yyyy HH:mm:ss"), ddlMachineId.SelectedValue.ToString());

                    }

                }
                else if (hdfValue.Value == "Production&DownData")
                {
                    if (Session["HeaderProdDownGrid"] == null)
                    {
                        Session["HeaderProdDownGrid"] = DataBaseAccess.BindSettingPage("WebTPMTrakVDGProductionandDown", Session["Language"] == null ? "en" : Session["Language"].ToString());
                        colHeader = Session["HeaderProdDownGrid"] as List<ColumnViewSetting>;
                    }
                    else
                    {
                        colHeader = Session["HeaderProdDownGrid"] as List<ColumnViewSetting>;
                    }
                    foreach (var item in colHeader)
                    {
                        if (item.ValueInBool)
                        {
                            columnNames.Add(item.ValueInText2);
                            colText.Add(item.ValueInText);
                        }
                    }
                    reportName = hdfValue.Value;
                    dt = Session["ProductionAndDownDataSource"] as DataTable;
                    if (dt == null)
                    {
                        BindProductionAndDownData();
                        dt = Session["ProductionAndDownDataSource"] as DataTable;
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string rpt = ExportExcelEpplus.VDGReportGenerate(columnNames, dt, fromDate.ToString("dd-MM-yyyy HH:mm:ss"), toDate.ToString("dd-MM-yyyy HH:mm:ss"), ddlMachineId.SelectedValue.ToString(), reportName, colText);
                    }
                    else
                    {
                        // ErrorSignal.FromCurrentContext().Raise(ex);
                        lblMessages.Text = GetLocalResourceObject("NoDatatoExport").ToString();
                    }
                }
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    string rpt = ExportExcelEpplus.VDGReportGenerate(columnNames, dt, fromDate.ToString("dd-MM-yyyy HH:mm:ss"), toDate.ToString("dd-MM-yyyy HH:mm:ss"), ddlMachineId.SelectedValue.ToString(), reportName, colText);
                //    //rpt.WriteGridDataToExcel();
                //}
                //else
                //{
                //    // ErrorSignal.FromCurrentContext().Raise(ex);
                //    lblMessages.Text = GetLocalResourceObject("NoDatatoExport").ToString();
                //}

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }

        }

        protected void btnGetComponent_Click(object sender, EventArgs e)
        {

            try
            {
                if (txtNoofComponents.Text.Length > 7) return;
                hdfValueMange.Value = "";
                hdfPaging.Value = "";
                int value;
                int invalid = 1; ;
                int.TryParse(txtNoofComponents.Text, out value);

                if (!int.TryParse(txtNoofComponents.Text, out value))
                {
                    lblMessages.Text = GetLocalResourceObject("PleaseenteronlyNumbers").ToString();
                    invalid = 0;
                }

                if (value <= 0 || value > 100)
                {
                    lblMessages.Text = GetLocalResourceObject("Pleaseenterthenumberbetween1to100").ToString();
                    invalid = 0;
                }
                if (invalid == 1)
                {
                    DateTime toDate, fromDate;
                    fromDate = VDGDataBaseAccess.GetComponentsStartEndTime(ddlMachineId.SelectedValue, txtNoofComponents.Text, out toDate);
                    txtFromDate.Text = fromDate.ToString("dd-MM-yyyy HH:mm:ss");
                    txtToDate.Text = toDate.ToString("dd-MM-yyyy HH:mm:ss");
                    btnRefresh_Click(null, null);
                    txtNoofComponents.Text = "";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
            hdnvalueGetN.Value = "modalCloseGetN";
            hdfHideValue.Value = "";
            // ScriptManager.RegisterStartupScript(this, this.GetType(), "afterPostback", "modalGetcompClose()", true);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ToolWiseCycleTimeData getRequiredCellNumberForContext()
        {
            ToolWiseCycleTimeData data = new ToolWiseCycleTimeData();
            try
            {
                if (HttpContext.Current.Session["EnabledCellListForContext"] != null)
                {
                    List<ListItem> list = HttpContext.Current.Session["EnabledCellListForContext"] as List<ListItem>;
                    if (list.Count > 0)
                    {
                        data.CycleStartTime = getEnabledColumnNumber(list, "StartTime");
                        data.CycleEndTime = getEnabledColumnNumber(list, "EndTime");
                        data.SlNo = getEnabledColumnNumber(list, "PartSlno");
                        data.ComponentID = getEnabledColumnNumber(list, "ComponentID");
                        data.OperationNum = getEnabledColumnNumber(list, "OperationNo");
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getRequiredCellNumberForContext =" + ex.Message);
            }
            return data;
        }
        private static string getEnabledColumnNumber(List<ListItem> list, string colname)
        {
            string colNumber = "";
            try
            {
                var item = list.Where(k => k.Text == colname).ToList();
                if (item != null)
                {
                    if (item.Count > 0)
                    {
                        colNumber = item[0].Value;
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return colNumber;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool isCustomPageEnabled()
        {
            bool pageEnabled = false;
            try
            {
                if (ConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase) || ConfigurationManager.AppSettings["VulkanPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    pageEnabled = true;

                }
                //if(ConfigurationManager.AppSettings["VulkanPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                //{
                //    isVulkanPagesEnabled = true;
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("isCustomPageEnabled =" + ex.Message);
            }
            return pageEnabled;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool isPageEnabled(string pageName)
        {
            bool pageEnabled = false;
            try
            {
                if (ConfigurationManager.AppSettings[pageName].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    pageEnabled = true;

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("isPageEnabled =" + ex.Message);
            }
            return pageEnabled;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string getCycleTimeUpdateValidationResult(string userName, string passWord)
        {
            string result = "";
            try
            {
                List<UserAccessModel> useAccessData = new List<UserAccessModel>();
                if (HttpContext.Current.Session["UserAccessData"] == null)
                    HttpContext.Current.Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(HttpContext.Current.Session["UserName"].ToString());
                else
                    useAccessData = HttpContext.Current.Session["UserAccessData"] as List<UserAccessModel>;
                if (useAccessData.Where(ss => ss.Code.Equals("VDGCycleTimeUpdateAccess", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                {
                    string UserValid = DataBaseAccess.CheckEmployeeDetail(userName, passWord);
                    if (!string.IsNullOrWhiteSpace(UserValid))
                    {
                        result = "Success";
                    }
                    else
                    {
                        result = "Invalid Username or Password.";
                    }
                }
                else
                    result = "Authentication failed.";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return result;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetRemarksID(string SlNo, string Param) //changed13112021
        {
            List<string> list = new List<string>();
            try
            {
                string autodataID = "", remarks = "", ActionTaken = "";
                if (Param == "ProductionData")
                {
                    if (HttpContext.Current.Session["proddatasource"] != null)
                    {
                        DataTable productionData = HttpContext.Current.Session["proddatasource"] as DataTable;
                        var rows = productionData.AsEnumerable().Where(x => x["SerialNO"].ToString() == SlNo).ToList().FirstOrDefault();
                        if (rows != null)
                        {
                            autodataID = rows["id"].ToString();
                            remarks = rows["Remarks"].ToString();
                        }
                    }
                }
                else
                {
                    if (HttpContext.Current.Session["downdatasource"] != null)
                    {
                        DataTable downDara = HttpContext.Current.Session["downdatasource"] as DataTable;
                        var rows = downDara.AsEnumerable().Where(x => x["SerialNO"].ToString() == SlNo).ToList().FirstOrDefault();
                        if (rows != null)
                        {
                            autodataID = rows["id"].ToString();
                            remarks = rows["Remarks"].ToString();
                            if (downDara.Columns.IndexOf("ActionTaken") != -1)
                                ActionTaken = rows["ActionTaken"].ToString();
                        }
                    }
                }
                list.Add(autodataID);
                list.Add(remarks);
                list.Add(ActionTaken);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetRemarksID =" + ex.Message);
            }
            return list;
        }

        protected void gridEventData_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                string SortDir = string.Empty;
                DataTable EventData = Session["eventdatasource"] as DataTable;
                if (EventData == null)
                {
                    BindEventGrid();
                    EventData = Session["eventdatasource"] as DataTable;
                }
                if (eventShorting == SortDirection.Ascending)
                {
                    eventShorting = SortDirection.Descending;
                    SortDir = "Desc";
                }
                else
                {
                    eventShorting = SortDirection.Ascending;
                    SortDir = "Asc";
                }
                if (EventData != null && EventData.Rows.Count > 0)
                {
                    DataView sortedView = new DataView(EventData);
                    sortedView.Sort = e.SortExpression + " " + SortDir;
                    gridEventData.DataSource = sortedView;
                    gridEventData.DataBind();
                    int id = 1;
                    //foreach (GridViewRow oItem in gridEventData.Rows)
                    //{
                    //    string getFirstColValue = oItem.Cells[0].Text;
                    //    oItem.Attributes.Add("class", "pdrow");
                    //    oItem.Attributes.Add("id", "pdrow" + getFirstColValue);// id.ToString());
                    //    id++;
                    //}
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex); ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        protected void gridEventData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            hdfValueMange.Value = "";
            RegisterAsyncTask(new PageAsyncTask(AsyncChartViewerData));//SetMachineDataValues();
            Session["PageEventIndex"] = gridEventData.PageIndex = e.NewPageIndex;
            gridEventData.DataBind();
            BindEventGrid();
            //hdfPaging.Value = "EventPaging";
        }

        protected void gridProductionData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        if (e.Row.Cells[i].Text.Trim().Equals("Start Time", StringComparison.OrdinalIgnoreCase))
                            Session["StartTimeIndex"] = i;
                        else if (e.Row.Cells[i].Text.Trim().Equals("End Time", StringComparison.OrdinalIgnoreCase))
                            Session["EndTimeIndex"] = i;
                        else if (e.Row.Cells[i].Text.Trim().Equals("IsModified", StringComparison.OrdinalIgnoreCase))
                        {
                            Session["isModifiedIndex"] = i;
                            isModifiedIndexx = i;
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int StartTimeIndex = Convert.ToInt32(Session["StartTimeIndex"]);
                    int EndTimeIndex = Convert.ToInt32(Session["EndTimeIndex"]);
                    DateTime dateValue;

                    if (DateTime.TryParseExact(e.Row.Cells[StartTimeIndex].Text, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                        e.Row.Cells[StartTimeIndex].Text = dateValue.ToString("dd-MM-yyyy hh:mm:ss tt");
                    if (DateTime.TryParseExact(e.Row.Cells[EndTimeIndex].Text, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                        e.Row.Cells[EndTimeIndex].Text = dateValue.ToString("dd-MM-yyyy hh:mm:ss tt");
                    int isModifiedIndex = Convert.ToInt32(Session["isModifiedIndex"]);
                    string backColor = DataBaseAccess.GetModifiedDataBackColor();
                    bool isProductionLog = DataBaseAccess.GetModifiedDataLog("EnableProductionLog");
                    if (!string.IsNullOrEmpty(e.Row.Cells[isModifiedIndex].Text.Trim()) && e.Row.Cells[isModifiedIndex].Text != "&nbsp;")
                    {
                        if (e.Row.Cells[isModifiedIndex].Text.Trim().Equals("True", StringComparison.OrdinalIgnoreCase))
                        {
                            if (isProductionLog)
                            {
                                e.Row.Attributes["style"] = "background-color:#" + backColor + "";
                                HiddenField hdnVal = new HiddenField();
                                hdnVal.Value = e.Row.Cells[isModifiedIndex].Text;
                                HyperLink hyperlink = new HyperLink();
                                hyperlink.Text = e.Row.Cells[isModifiedIndex].Text;
                                hyperlink.NavigateUrl = "#"; // Set the URL to "#" to prevent the page from reloading
                                hyperlink.Attributes["onclick"] = "openPopup('" + e.Row.Cells[0].Text + "','ProductionData'); return false;";
                                e.Row.Cells[isModifiedIndex].Controls.Add(hyperlink);
                                e.Row.Cells[isModifiedIndex].Controls.Add(hdnVal);
                                e.Row.Cells[isModifiedIndex].CssClass = "contains-hyperlink";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"gridProductionData_RowDataBound: {ex.Message}");
            }
        }

        protected void gvProductionDownData_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                string SortDir = string.Empty;
                DataTable ProductionAndDownData = Session["ProductionAndDownDataSource"] as DataTable;
                if (ProductionAndDownData == null)
                {
                    BindProductionAndDownData();
                    ProductionAndDownData = Session["ProductionAndDownDataSource"] as DataTable;
                }
                if (ProductionAndDownSorting == SortDirection.Ascending)
                {
                    ProductionAndDownSorting = SortDirection.Descending;
                    SortDir = "Desc";
                }
                else
                {
                    ProductionAndDownSorting = SortDirection.Ascending;
                    SortDir = "Asc";
                }
                if (ProductionAndDownData != null && ProductionAndDownData.Rows.Count > 0)
                {
                    DataView sortedView = new DataView(ProductionAndDownData);
                    sortedView.Sort = e.SortExpression + " " + SortDir;
                    gvProductionDownData.DataSource = sortedView;
                    gvProductionDownData.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.Text = ex.Message;
            }
        }

        protected void gvProductionDownData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            hdfValueMange.Value = "";
            RegisterAsyncTask(new PageAsyncTask(AsyncChartViewerData));//SetMachineDataValues();
            Session["PageProdDownIndex"] = gvProductionDownData.PageIndex = e.NewPageIndex;
            gvProductionDownData.DataBind();
            BindProductionAndDownData();
        }

        protected void gvProductionDownData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        if (e.Row.Cells[i].Text.Trim().Equals("Start Time", StringComparison.OrdinalIgnoreCase))
                            Session["StartTimeIndex"] = i;
                        else if (e.Row.Cells[i].Text.Trim().Equals("End Time", StringComparison.OrdinalIgnoreCase))
                            Session["EndTimeIndex"] = i;
                        else if (e.Row.Cells[i].Text.Trim().Equals("Down Time", StringComparison.OrdinalIgnoreCase))
                            Session["DownTimeIndex"] = i;
                        else if (e.Row.Cells[i].Text.Trim().Equals("IsModified", StringComparison.OrdinalIgnoreCase))
                        {
                            Session["isModifiedIndex"] = i;
                            isModifiedIndexx = i;
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int StartTimeIndex = Convert.ToInt32(Session["StartTimeIndex"]);
                    int EndTimeIndex = Convert.ToInt32(Session["EndTimeIndex"]);
                    int DownTimeIndex = Convert.ToInt32(Session["DownTimeIndex"]);
                    DateTime dateValue;
                    if (DateTime.TryParseExact(e.Row.Cells[StartTimeIndex].Text, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                        e.Row.Cells[StartTimeIndex].Text = dateValue.ToString("dd-MM-yyyy hh:mm:ss tt");
                    if (DateTime.TryParseExact(e.Row.Cells[EndTimeIndex].Text, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                        e.Row.Cells[EndTimeIndex].Text = dateValue.ToString("dd-MM-yyyy hh:mm:ss tt");

                    if (!string.IsNullOrEmpty(e.Row.Cells[DownTimeIndex].Text.Trim()) && e.Row.Cells[DownTimeIndex].Text != "&nbsp;")
                    {
                        e.Row.Attributes["style"] = "background-color: #ed8787";
                    }
                    int isModifiedIndex = Convert.ToInt32(Session["isModifiedIndex"]);
                    string backColor = DataBaseAccess.GetModifiedDataBackColor();
                    bool isProductionLog = DataBaseAccess.GetModifiedDataLog("EnableProductionLog");
                    bool isDownLog = DataBaseAccess.GetModifiedDataLog("EnableDownLog");
                    if (!string.IsNullOrEmpty(e.Row.Cells[isModifiedIndex].Text.Trim()) && e.Row.Cells[isModifiedIndex].Text != "&nbsp;")
                    {
                        if (e.Row.Cells[isModifiedIndex].Text.Trim().Equals("True", StringComparison.OrdinalIgnoreCase))
                        {
                            if (isProductionLog || isDownLog)
                            {
                                e.Row.Attributes["style"] = "background-color:#" + backColor + "";
                                HiddenField hdnVal = new HiddenField();
                                hdnVal.Value = e.Row.Cells[isModifiedIndex].Text;
                                HyperLink hyperlink = new HyperLink();
                                hyperlink.Text = e.Row.Cells[isModifiedIndex].Text;
                                hyperlink.NavigateUrl = "#"; // Set the URL to "#" to prevent the page from reloading
                                hyperlink.Attributes["onclick"] = "openPopup('" + e.Row.Cells[0].Text + "','ProductionDownData'); return false;";
                                e.Row.Cells[isModifiedIndex].Controls.Add(hyperlink);
                                e.Row.Cells[isModifiedIndex].Controls.Add(hdnVal);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"gridProductionData_RowDataBound: {ex.Message}");
            }
        }

        protected void gridDownTimeData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        if (e.Row.Cells[i].Text.Trim().Equals("IsModified", StringComparison.OrdinalIgnoreCase))
                        {
                            Session["isModifiedIndex"] = i;
                            isModifiedIndexx = i;
                        }
                        else if (e.Row.Cells[i].Text.Trim().Equals("End Time", StringComparison.OrdinalIgnoreCase))
                        {
                            Session["EndTimeIndex"] = i;
                            EndTimeIndex = i;
                        }

                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int isModifiedIndex = Convert.ToInt32(Session["isModifiedIndex"]);
                    int EndTimeIndexx = Convert.ToInt32(Session["EndTimeIndex"]);
                    string backColor = DataBaseAccess.GetModifiedDataBackColor();
                    bool isDownLog = DataBaseAccess.GetModifiedDataLog("EnableDownLog");
                    if (!string.IsNullOrEmpty(e.Row.Cells[EndTimeIndexx].Text.Trim()) && e.Row.Cells[EndTimeIndexx].Text != "&nbsp;")
                    {
                        if(e.Row.Cells[EndTimeIndexx].Text.Trim().Equals("01-01-1900 00:00:00", StringComparison.OrdinalIgnoreCase))
                        {
                            e.Row.Cells[EndTimeIndexx].Text = "";
                        }
                    }
                    if (!string.IsNullOrEmpty(e.Row.Cells[isModifiedIndex].Text.Trim()) && e.Row.Cells[isModifiedIndex].Text != "&nbsp;")
                    {
                        if (e.Row.Cells[isModifiedIndex].Text.Trim().Equals("True", StringComparison.OrdinalIgnoreCase))
                        {
                            if (isDownLog)
                            {
                                e.Row.Attributes["style"] = "background-color:#" + backColor + "";
                                HiddenField hdnVal = new HiddenField();
                                hdnVal.Value = e.Row.Cells[isModifiedIndex].Text;
                                HyperLink hyperlink = new HyperLink();
                                hyperlink.Text = e.Row.Cells[isModifiedIndex].Text;
                                hyperlink.NavigateUrl = "#"; // Set the URL to "#" to prevent the page from reloading
                                hyperlink.Attributes["onclick"] = "openPopup('" + e.Row.Cells[0].Text + "','DownData'); return false;";
                                e.Row.Cells[isModifiedIndex].Controls.Add(hyperlink);
                                e.Row.Cells[isModifiedIndex].Controls.Add(hdnVal);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<ModifiedData_VDG> GetModifiedDataID_VDG(string SlNo, string Param)
        {
            List<ModifiedData_VDG> list = new List<ModifiedData_VDG>();
            string autodataID = "";
            try
            {
                if (Param == "ProductionData")
                {
                    if (HttpContext.Current.Session["proddatasource"] != null)
                    {
                        DataTable productionData = HttpContext.Current.Session["proddatasource"] as DataTable;
                        var rows = productionData.AsEnumerable().Where(x => x["SerialNO"].ToString() == SlNo).ToList().FirstOrDefault();
                        if (rows != null)
                        {
                            autodataID = rows["id"].ToString();
                        }
                    }
                }
                else if (Param == "DownData")
                {
                    if (HttpContext.Current.Session["downdatasource"] != null)
                    {
                        DataTable downDara = HttpContext.Current.Session["downdatasource"] as DataTable;
                        var rows = downDara.AsEnumerable().Where(x => x["SerialNO"].ToString() == SlNo).ToList().FirstOrDefault();
                        if (rows != null)
                        {
                            autodataID = rows["id"].ToString();
                        }
                    }
                }
                else if (Param == "ProductionDownData")
                {
                    if (HttpContext.Current.Session["ProductionAndDownDataSource"] != null)
                    {
                        DataTable downDara = HttpContext.Current.Session["ProductionAndDownDataSource"] as DataTable;
                        var rows = downDara.AsEnumerable().Where(x => x["SerialNO"].ToString() == SlNo).ToList().FirstOrDefault();
                        if (rows != null)
                        {
                            autodataID = rows["ID"].ToString();
                        }
                    }
                }
                list = DataBaseAccess.GetModifiedDataHistory_VDG(autodataID, Param);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetRemarksID =" + ex.Message);
            }
            return list;
        }

        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellID();
            BindMachines();
        }

        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachines();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetModifiedBackColor()
        {
            string backColor = "";
            try
            {
                bool isProductionLog = DataBaseAccess.GetModifiedDataLog("EnableProductionLog");
                bool isDownLog = DataBaseAccess.GetModifiedDataLog("EnableDownLog");
                if (isProductionLog || isDownLog)
                {
                    backColor = DataBaseAccess.GetModifiedDataBackColor();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return backColor;
        }
    }
}