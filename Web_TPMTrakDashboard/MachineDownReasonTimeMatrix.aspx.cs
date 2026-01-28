using BusinessClassLibrary;
using Elmah;
using ModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class MachineDownReasonTimeMatrix : System.Web.UI.Page
    {
        public int DateTimeIntarvel = 31;
        public int fontSize = 20;
        public static string MachineColName = "";
        string defaultShift = string.Empty;
        //string culture = ""
        protected void Page_Load(object sender, EventArgs e)
        {
            //culture = Convert.ToString(HttpContext.Current.Session["Language"]);
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                
                Session["DownParetoData"] = null;
                BindDayShift();
                gettimings();
                BindPlantId();
                if (ConfigurationManager.AppSettings["DensoPages"].ToString().Equals("1"))
                {
                    tdPredefinedTime.Visible = true;tdPredefinedTimeh.Visible = true;
                }
                else
                {
                    tdPredefinedTime.Visible = false; tdPredefinedTimeh.Visible = false;
                }
                if (ConfigurationManager.AppSettings["sonapages"].ToString().Equals("1"))
                    MachineColName = "machineDescription";
                else
                    MachineColName = "MachineID";
                BindCellId(ddlPlantId.SelectedItem == null ? "" : ddlPlantId.SelectedItem.Text);
                BindDownIdInfo();
                string datetime = "";
                if (Session["FromDate"] != null && Session["ToDate"] != null)
                {
                    txtFromDate.Text = Convert.ToDateTime(Session["FromDate"].ToString()).ToString("dd-MM-yyyy HH:mm");
                    txtToDate.Text = Convert.ToDateTime(Session["ToDate"].ToString()).ToString("dd-MM-yyyy HH:mm");
                }
                else
                {
                    datetime = VDGDataBaseAccess.GetLogicalDayStart(DateTime.Now.ToString());
                    txtFromDate.Text = Convert.ToDateTime(datetime).ToString("dd-MM-yyyy HH:mm");
                    datetime = VDGDataBaseAccess.GetLogicalDayEnd(DateTime.Now.ToString());
                    txtToDate.Text = Convert.ToDateTime(datetime).ToString("dd-MM-yyyy HH:mm");
                }
                RegisterAsyncTask(new PageAsyncTask(GetAsync));
                hdfValue.Value = "OK";
            }
            DateTimeIntarvel = Convert.ToInt32(ConfigurationManager.AppSettings["LiveDownTimeDashboardInterval"].ToString());
            fontSize = Convert.ToInt32(Session["fontSize"]);
        }
        private void BindDayShift()
        {
            try
            {
                List<string> lstPlantData = CockpitDataBaseAccess.GetAllPredefinedShifts();
                ddlDayShift.DataSource = lstPlantData;
                ddlDayShift.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindDayShift: " + ex.Message);
            }
        }
        private void gettimings()
        {
            try
            {
                defaultShift = DataBaseAccess.GetDefaultCockpitDefaultShift();

                if (defaultShift.Equals("PreviousShift"))
                {
                    var prevShiftVals = DataBaseAccess.GetCurrentOrPreviousShiftVals("[s_GetPreviousShift]");
                    if (prevShiftVals != null)
                    {
                        SetDateTimeToControl(prevShiftVals);
                        DateTime logicalDayStart = Convert.ToDateTime(prevShiftVals[2]);
                        string ddlValue = "Today";
                        if (logicalDayStart.Date != DateTime.Now.Date)
                        {
                            ddlValue = "Yesterday";
                        }
                        ddlValue = ddlValue + " - " + prevShiftVals[3];
                        HelperClassGeneric.setDropdownValue(ddlDayShift, ddlValue);
                    }
                }
                else if (defaultShift.Equals("CurrentShift"))
                {
                    var currShiftVals = DataBaseAccess.GetCurrentOrPreviousShiftVals("[s_GetCurrentShift]");
                    if (currShiftVals != null)
                    {
                        SetDateTimeToControl(currShiftVals);
                        string ddlValue = "Today" + " - " + currShiftVals[3];
                        HelperClassGeneric.setDropdownValue(ddlDayShift, ddlValue);
                    }
                }
                else if (defaultShift.Equals("Last 24Hrs"))
                {
                    string fromdate = DateTime.Today.AddDays(-1).ToString("dd-MM-yyyy");
                    txtFromDate.Text = fromdate + DateTime.Now.ToString("HH:mm:ss");
                    string todate = DateTime.Today.ToString("dd-MM-yyyy");
                    txtToDate.Text = todate + DateTime.Now.ToString("HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void SetDateTimeToControl(List<string> shiftVals)
        {
            txtFromDate.Text = Convert.ToDateTime(shiftVals[0]).ToString("dd-MM-yyyy HH:mm:ss");
            txtToDate.Text = Convert.ToDateTime(shiftVals[1]).ToString("dd-MM-yyyy HH:mm:ss");
        }
        #region "Bind Plant Id"
        private void BindPlantId()
        {
            try
            {
                List<string> lstPlantData = BindCockpitView.ViewPlantToDisplay();
                ddlPlantId.DataSource = lstPlantData;
                ddlPlantId.DataBind();
                ddlPlantId.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommanResource", "PlantAll").ToString(), "All"));
                ddlPlantId_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Cell Id"
        private void BindCellId(string plantId)
        {
            try
            {
                List<string> lstCellId = BindCockpitView.ViewCellsToDisplay(plantId == "Plant All" ? "" : plantId);
                ddlCellID.DataSource = lstCellId;
                ddlCellID.DataBind();
                ddlCellID.Items.Insert(0, new ListItem
                {
                    Text = GetGlobalResourceObject("CommanResource", "CellAll").ToString(),
                    Value = "All"
                });
                ddlCellID.SelectedIndex = 0;
                ddlCellID_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Machine Id"
        private void BindMachines(string cellId)
        {
            List<string> allMachineName = new List<string>();
            try
            {
                ddlMachineId.Items.Clear();
                if (string.IsNullOrEmpty(cellId))
                {
                    allMachineName = VDGDataBaseAccess.GetAllMachines(ddlPlantId.SelectedValue.ToString());
                }
                else
                {
                    allMachineName = CockpitDataBaseAccess.GetMachinesForCell(cellId, ddlPlantId.SelectedValue.ToString());
                }
                if (allMachineName != null && allMachineName.Count > 0)
                {
                    ddlMachineId.DataSource = allMachineName;
                    ddlMachineId.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        public List<string> BindMachineData(string cellId)
        {
            List<string> allMachineName = new List<string>();
            try
            {
                ddlMachineId.Items.Clear();
                if (string.IsNullOrEmpty(cellId))
                {
                    allMachineName = VDGDataBaseAccess.GetAllMachines(ddlPlantId.SelectedValue.ToString());
                }
                else
                {
                    allMachineName = CockpitDataBaseAccess.GetMachinesForCell(cellId, ddlPlantId.SelectedValue.ToString());
                }
                if (allMachineName != null && allMachineName.Count > 0)
                {
                    ddlMachineId.DataSource = allMachineName;
                    ddlMachineId.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
            return allMachineName;
        }
        #endregion

        #region "Bind DownId Information"
        private void BindDownIdInfo()
        {
            try
            {
                var DownId = TMPTrakDataBase.GetDownIdInfo(Util.show16losses);
                if (DownId != null && DownId.Count > 0)
                {
                    ddlMultiDownID.DataSource = DownId;
                    ddlMultiDownID.DataBind();
                }
                if (ConfigurationManager.AppSettings["SSWLPages"].ToString() == "1")
                {
                    foreach (ListItem item in ddlMultiDownID.Items)
                    {
                        if (item.Value.ToUpper().Contains("NO_DATA"))
                        {
                            item.Selected = true;
                            chkExclude.Checked = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Sync Await use both gridview"
        private async Task GetAsync()
        {
            try
            {
                Util.SetCultureForThread();
                string downcode = GetLocalResourceObject("DownCode").ToString();
                string Total = GetLocalResourceObject("Total").ToString();
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;
                DateTime.TryParse(txtFromDate.Text, out fromDate);
                fromDate = Util.GetDateTime(txtFromDate.Text);
                toDate = Util.GetDateTime(txtToDate.Text);
                Session["FromDate"] = fromDate.ToString("yyyy-MM-dd HH:mm:ss");
                Session["ToDate"] = toDate.ToString("yyyy-MM-dd HH:mm:ss");

                //if ((toDate - fromDate).TotalDays > 30)
                //{
                //    lblMessages.Text = "Difference between to date and from date cannot be more than 31 days.";
                //    return;
                //}
                //else
                //{
                string listMachine = "", machineSelection = "";
                string plantId, cellId = string.Empty;
                //int ShowNoOfvalues = ddlTopDownReasons.SelectedItem != null ? Convert.ToInt32(ddlTopDownReasons.SelectedValue) : 10;
                #region "Machine selection"
                foreach (ListItem item in ddlMachineId.Items)
                {
                    if (item.Selected)
                    {
                        listMachine += item.Value + "$@#";
                    }
                }
                if (listMachine != "")
                {
                    string[] result = listMachine.Split(new string[] { "$@#" }, StringSplitOptions.None);
                    result = result.Take(result.Count() - 1).ToArray();
                    machineSelection = string.Join(",", result.ToArray());
                }
                #endregion

                string downId = "", listDownId = "";
                int Exclude = 0;

                #region "DownId Selection "
                foreach (ListItem item in ddlMultiDownID.Items)
                {
                    if (item.Selected)
                    {
                        listDownId += item.Value + "$@#";
                    }
                }
                if (listDownId != "")
                {
                    string[] result = listDownId.Split(new string[] { "$@#" }, StringSplitOptions.None);
                    result = result.Take(result.Count() - 1).ToArray();
                    downId = string.Join(",", result.ToArray());
                }
                if (chkExclude.Checked)
                    Exclude = 1;
                else
                    Exclude = 0;
                #endregion

                if (ddlPlantId.SelectedItem != null)
                {
                    if (ddlPlantId.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase))
                        plantId = "";
                    else
                        plantId = ddlPlantId.SelectedValue.ToString();
                }
                else
                {
                    plantId = "";
                }

                if (ddlCellID.SelectedItem != null)
                {
                    if (ddlCellID.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase))
                        cellId = "";
                    else
                        cellId = ddlCellID.SelectedValue.ToString();
                }
                else
                {
                    cellId = "";
                }
                if (ConfigurationManager.AppSettings["sonapages"].ToString().Equals("1"))
                    MachineColName = "machineDescription";
                else
                    MachineColName = "MachineID";
                DataTable downTimeWise = null; DataTable downTimeFreq = null;

                var vardownTime = TMPTrakDataBase.GetDownTimeAndFreqData(fromDate, toDate, machineSelection, plantId, downId, Exclude, "DTime", cellId);
                var vardownTimeFreq = TMPTrakDataBase.GetDownTimeAndFreqData(fromDate, toDate, machineSelection, plantId, downId, Exclude, "DFreq", cellId);
                await Task.WhenAll(vardownTime, vardownTimeFreq).ConfigureAwait(false);

                //if (vardownTime.Result.Rows.Count > 0)
                //    Session["DownTime"] = downTimeWise = vardownTime.Result.AsEnumerable().Take(ShowNoOfvalues).CopyToDataTable();
                //else
                //    Session["DownTime"] = downTimeWise = vardownTime.Result;
                //if (vardownTimeFreq.Result.Rows.Count > 0)
                //    Session["DownTimeFreq"] = downTimeFreq = vardownTimeFreq.Result.AsEnumerable().Take(ShowNoOfvalues).CopyToDataTable();
                //else
                //    Session["DownTimeFreq"] = downTimeFreq = vardownTimeFreq.Result;
                Session["DownTime"] = downTimeWise = vardownTime.Result;
                Session["DownTimeFreq"] = downTimeFreq = vardownTimeFreq.Result;

                //------Down Time-wise--------- 
                Dictionary<string, int> dicValue = new Dictionary<string, int>();
                DataView view = new DataView(downTimeWise);
                DataTable headerValues = new DataTable();
                if (view != null && view.Table.Rows.Count > 0)
                    headerValues = view.ToTable(true, MachineColName);

                //Logger.WriteDebugLog(System.Threading.Thread.CurrentThread.CurrentUICulture.ToString());
                DataTable Values = new DataTable();
                Values.Columns.Add("DownCode", typeof(string));

                for (int i = 0; i < headerValues.Rows.Count; i++)
                {
                    string strValue = headerValues.Rows[i][MachineColName].ToString();
                    Values.Columns.Add(strValue, typeof(string));
                }
                Values.Columns.Add("Total", typeof(string));

                int totalDown = 0;
                string downCode = string.Empty;
                DataRow row = null;
                TimeSpan t = TimeSpan.MinValue;
                string answer = string.Empty;
                for (int i = 0; i < downTimeWise.Rows.Count; i++)
                {
                    if (downTimeWise.Rows[i]["DownCode"].ToString() != downCode)
                    {

                        row = Values.NewRow();
                        Values.Rows.Add(row);
                        Values.Rows[Values.Rows.Count - 1]["DownCode"] = downTimeWise.Rows[i]["DownCode"];
                    }
                    if (Convert.ToInt32(downTimeWise.Rows[i]["DownTime"]) > 0)
                    {
                        t = TimeSpan.FromSeconds(Convert.ToInt32(downTimeWise.Rows[i]["DownTime"]));
                        int seconds = Convert.ToInt32(downTimeWise.Rows[i]["DownTime"]);
                        string h = string.Format("{0:00}:{1:00}:{2:00}", seconds / 3600, (seconds / 60) % 60, seconds % 60);
                        answer = string.Format("{0:00}:{1:00}:{2:00}",
                           (int)t.TotalHours,
                                 t.Minutes,
                                 t.Seconds);
                        Values.Rows[Values.Rows.Count - 1][downTimeWise.Rows[i][MachineColName].ToString()] = answer;
                    }
                    else
                        Values.Rows[Values.Rows.Count - 1][downTimeWise.Rows[i][MachineColName].ToString()] = string.Empty;

                    if (Convert.ToInt32(downTimeWise.Rows[i]["TotalDown"]) > 0)
                    {
                        t = TimeSpan.FromSeconds(Convert.ToInt32(downTimeWise.Rows[i]["TotalDown"].ToString()));
                        answer = string.Format("{0:00}:{1:00}:{2:00}",
                                 (int)t.TotalHours,
                                 t.Minutes,
                                 t.Seconds);
                        Values.Rows[Values.Rows.Count - 1]["Total"] = answer;
                    }
                    else
                        Values.Rows[Values.Rows.Count - 1]["Total"] = string.Empty;
                    downCode = downTimeWise.Rows[i]["DownCode"].ToString();
                    if (dicValue.ContainsKey(downTimeWise.Rows[i][MachineColName].ToString()))
                        dicValue[downTimeWise.Rows[i][MachineColName].ToString()] = Convert.ToInt32(downTimeWise.Rows[i]["TotalMachine"]);
                    else
                        dicValue.Add(downTimeWise.Rows[i][MachineColName].ToString(), Convert.ToInt32(downTimeWise.Rows[i]["TotalMachine"]));
                    //if (dicValue.ContainsKey(downTimeWise.Rows[i][MachineColName].ToString()))
                    //    dicValue[downTimeWise.Rows[i][MachineColName].ToString()] = totalMachineDown;
                    //else
                    //    dicValue.Add(downTimeWise.Rows[i][MachineColName].ToString(), Convert.ToInt32(downTimeWise.Rows[i]["TotalMachine"]));
                }
                totalDown = 0;
                Values.Rows.Add(Total);
                // answer = string.Empty;
                for (int i = 0; i < dicValue.Count; i++)
                {
                    totalDown += dicValue[dicValue.Keys.ElementAt(i)];
                    t = TimeSpan.FromSeconds(dicValue[dicValue.Keys.ElementAt(i)]);
                    answer = string.Format("{0:00}:{1:00}:{2:00}",
                                   (int)t.TotalHours,
                                   t.Minutes,
                                   t.Seconds);
                    Values.Rows[Values.Rows.Count - 1][i + 1] = answer;
                }
                if (Values.Rows.Count > 0)
                {
                    t = TimeSpan.FromSeconds(totalDown);
                    answer = string.Format("{0:00}:{1:00}:{2:00}",
                                   (int)t.TotalHours,
                                   t.Minutes,
                                   t.Seconds);
                    Values.Rows[Values.Rows.Count - 1]["Total"] = answer;
                }

                Values.Columns["DownCode"].ColumnName = downcode;
                Values.Columns["Total"].ColumnName = Total;
                //------Down Time-wise--------- 
                gridTimeWiseInfo.DataSource = Values;
                gridTimeWiseInfo.DataBind();

                ////-------Down Time Freq.---------
                totalDown = 0;
                //Values.Rows.Clear();
                Values = new DataTable();
                Values.Columns.Add("DownCode", typeof(string));

                for (int i = 0; i < headerValues.Rows.Count; i++)
                {
                    string strValue = headerValues.Rows[i][MachineColName].ToString();
                    Values.Columns.Add(strValue, typeof(string));
                }
                Values.Columns.Add("Total", typeof(string));
                row = null;
                downCode = string.Empty;
                dicValue = new Dictionary<string, int>();
                for (int i = 0; i < downTimeWise.Rows.Count; i++)
                {
                    if (downTimeWise.Rows[i]["DownCode"].ToString() != downCode)
                    {
                        row = Values.NewRow();
                        Values.Rows.Add(row);
                        Values.Rows[Values.Rows.Count - 1]["DownCode"] = downTimeWise.Rows[i]["DownCode"];
                        //totalDown = 0;
                    }
                    //totalValue = downTimeWise.Rows[i]["DownFreq"] != DBNull.Value ? Convert.ToInt32(downTimeWise.Rows[i]["DownFreq"]) : 0;
                    //totalValue = totalValue + totalValue;
                    Values.Rows[Values.Rows.Count - 1][downTimeWise.Rows[i][MachineColName].ToString()] = downTimeWise.Rows[i]["DownFreq"];
                    downCode = downTimeWise.Rows[i]["DownCode"].ToString();
                    //totalDown += Convert.ToInt32(downTimeWise.Rows[i]["DownFreq"]);
                    Values.Rows[Values.Rows.Count - 1]["Total"] = downTimeWise.Rows[i]["TotalDownFreq"];

                    if (dicValue.ContainsKey(downTimeWise.Rows[i][MachineColName].ToString()))
                        dicValue[downTimeWise.Rows[i][MachineColName].ToString()] = Convert.ToInt32(downTimeWise.Rows[i]["TotalMachineFreq"]);
                    else
                        dicValue.Add(downTimeWise.Rows[i][MachineColName].ToString(), Convert.ToInt32(downTimeWise.Rows[i]["TotalMachineFreq"]));
                }

                Values.Rows.Add(Total);
                // answer = string.Empty;
                for (int i = 0; i < dicValue.Count; i++)
                {
                    totalDown += dicValue[dicValue.Keys.ElementAt(i)];
                    Values.Rows[Values.Rows.Count - 1][i + 1] = dicValue[dicValue.Keys.ElementAt(i)];
                }
                if (Values.Rows.Count > 0)
                {
                    Values.Rows[Values.Rows.Count - 1]["Total"] = totalDown;
                }
                Values.Columns["DownCode"].ColumnName = downcode;
                Values.Columns["Total"].ColumnName = Total;
                gridTimeWiseFreq.DataSource = Values;
                gridTimeWiseFreq.DataBind();



                ///----------------------------------------------------------------------------------
                ///---------------------------------Down Time Freq-----------------------------------
                ///
                dicValue = new Dictionary<string, int>();
                totalDown = 0;
                view = new DataView(downTimeFreq);
                if (view != null && view.Table.Rows.Count > 0)
                    headerValues = view.ToTable(true, MachineColName);
                Values = new DataTable();
                Values.Columns.Add("DownCode", typeof(string));
                for (int i = 0; i < headerValues.Rows.Count; i++)
                {
                    string strValue = headerValues.Rows[i][MachineColName].ToString();
                    Values.Columns.Add(strValue, typeof(string));
                }
                Values.Columns.Add("Total", typeof(string));
                downCode = string.Empty;
                row = null;
                downCode = string.Empty;
                for (int i = 0; i < downTimeFreq.Rows.Count; i++)
                {
                    if (downTimeFreq.Rows[i]["DownCode"].ToString() != downCode)
                    {
                        row = Values.NewRow();
                        Values.Rows.Add(row);
                        Values.Rows[Values.Rows.Count - 1]["DownCode"] = downTimeFreq.Rows[i]["DownCode"];
                        //totalDown = 0;
                    }
                    Values.Rows[Values.Rows.Count - 1][downTimeFreq.Rows[i][MachineColName].ToString()] = downTimeFreq.Rows[i]["DownFreq"];
                    downCode = downTimeFreq.Rows[i]["DownCode"].ToString();
                    //  totalDown += Convert.ToInt32(downTimeFreq.Rows[i]["DownFreq"]);
                    Values.Rows[Values.Rows.Count - 1]["Total"] = downTimeFreq.Rows[i]["TotalDownFreq"];

                    if (dicValue.ContainsKey(downTimeFreq.Rows[i][MachineColName].ToString()))
                        dicValue[downTimeFreq.Rows[i][MachineColName].ToString()] = Convert.ToInt32(downTimeFreq.Rows[i]["TotalMachineFreq"]);
                    else
                        dicValue.Add(downTimeFreq.Rows[i][MachineColName].ToString(), Convert.ToInt32(downTimeFreq.Rows[i]["TotalMachineFreq"]));
                }
                Values.Rows.Add(Total);
                // answer = string.Empty;
                for (int i = 0; i < dicValue.Count; i++)
                {
                    totalDown += dicValue[dicValue.Keys.ElementAt(i)];
                    Values.Rows[Values.Rows.Count - 1][i + 1] = dicValue[dicValue.Keys.ElementAt(i)];
                }
                if (Values.Rows.Count > 0)
                {
                    Values.Rows[Values.Rows.Count - 1]["Total"] = totalDown;
                }
                Values.Columns["DownCode"].ColumnName = downcode;
                Values.Columns["Total"].ColumnName = Total;
                gridFreqWiseInfo.DataSource = Values;
                gridFreqWiseInfo.DataBind();
                //}
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Time-wise Information Data"
        private void BindTimeWiseInfo()
        {
            try
            {
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;

                fromDate = Util.GetDateTime(txtFromDate.Text);
                toDate = Util.GetDateTime(txtToDate.Text);


                string listMachine = "", machineSelection = "";
                string plantId, cellId = string.Empty;
                #region "Machine selection"
                foreach (ListItem item in ddlMachineId.Items)
                {
                    if (item.Selected)
                    {
                        listMachine += item.Value + "$@#";
                    }
                }
                if (listMachine != "")
                {
                    string[] result = listMachine.Split(new string[] { "$@#" }, StringSplitOptions.None);
                    result = result.Take(result.Count() - 1).ToArray();
                    machineSelection = string.Join(",", result.ToArray());
                }
                #endregion

                string downId = "", listDownId = "";
                int Exclude = 0;

                #region "DownId Selection "
                foreach (ListItem item in ddlMultiDownID.Items)
                {
                    if (item.Selected)
                    {
                        listDownId += item.Value + "$@#";
                    }
                }
                if (listDownId != "")
                {
                    string[] result = listDownId.Split(new string[] { "$@#" }, StringSplitOptions.None);
                    result = result.Take(result.Count() - 1).ToArray();
                    downId = string.Join(",", result.ToArray());
                }
                if (chkExclude.Checked)
                    Exclude = 1;
                else
                    Exclude = 0;
                #endregion

                if (ddlPlantId.SelectedItem != null)
                {
                    if (ddlPlantId.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase))
                        plantId = "";
                    else
                        plantId = ddlPlantId.SelectedValue.ToString();
                }
                else
                {
                    plantId = "";
                }

                if (ddlCellID.SelectedItem != null)
                {
                    if (ddlCellID.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase))
                        cellId = "";
                    else
                        cellId = ddlCellID.SelectedValue.ToString();
                }
                else
                {
                    cellId = "";
                }

                DataTable dt = TMPTrakDataBase.MachineDownTimeMatrix(fromDate, toDate, machineSelection, plantId, downId, Exclude, "DTime", cellId, "s_GetDownTimeMatrixfromAutoData", "");
                Session["DownTime"] = dt;
                DataView view = new DataView(dt);
                DataTable headerValues = view.ToTable(true, MachineColName);
                DataTable Values = new DataTable();
                Values.Columns.Add("DownCode", typeof(string));
                for (int i = 0; i < headerValues.Rows.Count; i++)
                {
                    string strValue = headerValues.Rows[i][MachineColName].ToString();
                    Values.Columns.Add(strValue, typeof(string));
                }

                string downCode = string.Empty;

                //int newRowId = -1;
                DataRow row = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DownCode"].ToString() != downCode)
                    {
                        row = Values.NewRow();
                        Values.Rows.Add(row);
                        Values.Rows[Values.Rows.Count - 1]["DownCode"] = dt.Rows[i]["DownCode"];
                    }
                    //row[0] = dt.Rows[i]["DownCode"];
                    //Values.Rows.InsertAt(row, 0);
                    // Values.Rows[Values.Rows.Count - 1][dt.Rows[i]["DownCode"].ToString()] = dt.Rows[i]["DownCode"];
                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToInt32(dt.Rows[i]["DownTime"]));

                    string answer = string.Format("{0}:{1}:{2}",
                                    t.TotalHours.ToString("00"),
                                    t.Minutes.ToString("00"),
                                    t.Seconds.ToString("00"));
                    Values.Rows[Values.Rows.Count - 1][dt.Rows[i][MachineColName].ToString()] = answer;
                    //Values.Rows[Values.Rows.Count][dt.Rows[i]["MachineID"].ToString()] = dt.Rows[i]["DownTime"];

                    downCode = dt.Rows[i]["DownCode"].ToString();

                }


                //------Down Time-wise--------- 
                gridTimeWiseInfo.DataSource = Values;
                gridTimeWiseInfo.DataBind();



                ////-------Down Time Freq.---------
                Values.Rows.Clear();
                row = null;
                downCode = string.Empty;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DownCode"].ToString() != downCode)
                    {
                        row = Values.NewRow();
                        Values.Rows.Add(row);
                        Values.Rows[Values.Rows.Count - 1]["DownCode"] = dt.Rows[i]["DownCode"];
                    }
                    Values.Rows[Values.Rows.Count - 1][dt.Rows[i][MachineColName].ToString()] = dt.Rows[i]["DownFreq"];
                    downCode = dt.Rows[i]["DownCode"].ToString();
                }

                gridTimeWiseFreq.DataSource = Values;
                gridTimeWiseFreq.DataBind();


                ///-----------------------------
                ///---------------------------------Down Freq-----------------------------------
                //DataTable dt2 = TMPTrakDataBase.MachineDownTimeMatrix(fromDate, toDate, machineSelection, plantId, downId, Exclude, "DFreq");

                //gridFreqWiseInfo.DataSource = dt2;
                //gridFreqWiseInfo.DataBind();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Down Time Freq Wise "
        private void BindDownTimeFreqWise()
        {
            try
            {
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;
                fromDate = Util.GetDateTime(txtFromDate.Text);
                toDate = Util.GetDateTime(txtToDate.Text);

                string listMachine = "", machineSelection = "";
                string plantId, cellId = string.Empty;
                #region "Machine selection"
                foreach (ListItem item in ddlMachineId.Items)
                {
                    if (item.Selected)
                    {
                        listMachine += item.Value + "$@#";
                    }
                }
                if (listMachine != "")
                {
                    string[] result = listMachine.Split(new string[] { "$@#" }, StringSplitOptions.None);
                    result = result.Take(result.Count() - 1).ToArray();
                    machineSelection = string.Join(",", result.ToArray());
                }
                #endregion

                string downId = "", listDownId = "";
                int Exclude = 0;

                #region "DownId Selection "
                foreach (ListItem item in ddlMultiDownID.Items)
                {
                    if (item.Selected)
                    {
                        listDownId += item.Value + "$@#";
                    }
                }
                if (listDownId != "")
                {
                    string[] result = listDownId.Split(new string[] { "$@#" }, StringSplitOptions.None);
                    result = result.Take(result.Count() - 1).ToArray();
                    downId = string.Join(",", result.ToArray());
                }
                if (chkExclude.Checked)
                    Exclude = 1;
                else
                    Exclude = 0;
                #endregion

                if (ddlPlantId.SelectedItem != null)
                {
                    if (ddlPlantId.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase))
                        plantId = "";
                    else
                        plantId = ddlPlantId.SelectedValue.ToString();
                }
                else
                {
                    plantId = "";
                }

                if (ddlCellID.SelectedItem != null)
                {
                    if (ddlCellID.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase))
                        cellId = "";
                    else
                        cellId = ddlCellID.SelectedValue.ToString();
                }
                else
                {
                    cellId = "";
                }

                DataTable dt = TMPTrakDataBase.MachineDownTimeMatrix(fromDate, toDate, machineSelection, plantId, downId, Exclude, "DFreq", cellId, "s_GetDownTimeMatrixfromAutoData", "");
                Session["DownTimeFreq"] = dt;
                DataView view = new DataView(dt);
                DataTable headerValues = view.ToTable(true, "MachineID");
                DataTable Values = new DataTable();
                Values.Columns.Add("DownCode", typeof(string));
                for (int i = 0; i < headerValues.Rows.Count; i++)
                {
                    string strValue = headerValues.Rows[i]["MachineID"].ToString();
                    Values.Columns.Add(strValue, typeof(string));
                }

                string downCode = string.Empty;
                DataRow row = null;
                downCode = string.Empty;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DownCode"].ToString() != downCode)
                    {
                        row = Values.NewRow();
                        Values.Rows.Add(row);
                        Values.Rows[Values.Rows.Count - 1]["DownCode"] = dt.Rows[i]["DownCode"];
                    }
                    Values.Rows[Values.Rows.Count - 1][dt.Rows[i]["MachineID"].ToString()] = dt.Rows[i]["DownFreq"];
                    downCode = dt.Rows[i]["DownCode"].ToString();
                }

                gridFreqWiseInfo.DataSource = Values;
                gridFreqWiseInfo.DataBind();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellId(ddlPlantId.SelectedItem == null ? "" : ddlPlantId.SelectedItem.Text);
            hdfValue.Value = "NotOk";
        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            Session["DownParetoData"] = null;
            RegisterAsyncTask(new PageAsyncTask(GetAsync));
            hdfValue.Value = "OK";
        }

        #region "Get MCs by Top-5 Downs frist Graph"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ChartData<SeriesTimeWise> MCsbyTopDownsChart(string plantID, string machineId, string startFromDate, string startToDate, string downID, bool exclude, int downReasons, string CellID)
        {
            var chart = new ChartData<SeriesTimeWise>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = "XAxisTitle",
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };

            try
            {
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;
                fromDate = Util.GetDateTime(startFromDate);
                toDate = Util.GetDateTime(startToDate);

                string plantId = string.Empty;
                #region "Machine selection"
                if (machineId == "null")
                {
                    machineId = "";
                }
                #endregion

                string downId = "";
                int Exclude = 0;

                #region "DownId Selection "
                if (downID != "null")
                {
                    string[] result = downID.Split(new string[] { "," }, StringSplitOptions.None);
                    result = result.Take(result.Count()).ToArray();
                    downId = string.Join(",", result.ToArray());
                }
                if (exclude)
                    Exclude = 1;
                else
                    Exclude = 0;
                #endregion

                if (plantID.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    plantId = "";
                DataTable dt = null;
                if (HttpContext.Current.Session["DownTime"] == null)
                    dt = TMPTrakDataBase.MachineDownTimeMatrix(fromDate, toDate, machineId, plantId, downId, Exclude, "DTime", CellID, "s_GetDownTimeMatrixfromAutoData", "");
                else
                    dt = HttpContext.Current.Session["DownTime"] as DataTable;
                DataView view = new DataView(dt);
                DataTable headerValues = view.ToTable(true, MachineColName);
                DataTable Values = new DataTable();
                Values.Columns.Add("DownCode", typeof(string));
                for (int i = 0; i < headerValues.Rows.Count; i++)
                {
                    string strValue = headerValues.Rows[i][MachineColName].ToString();
                    Values.Columns.Add(strValue, typeof(int));
                }

                string downCode = string.Empty;
                DataRow row = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DownCode"].ToString() != downCode)
                    {
                        row = Values.NewRow();
                        Values.Rows.Add(row);
                        Values.Rows[Values.Rows.Count - 1]["DownCode"] = dt.Rows[i]["DownCode"];
                    }
                    //TimeSpan t = TimeSpan.FromSeconds(Convert.ToInt32(dt.Rows[i]["DownTime"]));
                    //string answer = string.Format("{0}",
                    //                t.TotalMinutes.ToString("00"));
                    Values.Rows[Values.Rows.Count - 1][dt.Rows[i][MachineColName].ToString()] = Convert.ToInt32(dt.Rows[i]["DownTime"]);
                    downCode = dt.Rows[i]["DownCode"].ToString();
                }

                chart.seriesTimeWise = new List<SeriesTimeWise>();
                List<string> cat = Values.AsEnumerable().Select(r => r.Field<string>("DownCode")).Take(downReasons).ToList();
                chart.categories = cat;

                for (int i = 1; i < Values.Columns.Count; i++)
                {
                    SeriesTimeWise seriesLs1 = new SeriesTimeWise();
                    string datatext = Values.Columns[i].ToString();
                    //var sss = Values.AsEnumerable().Select(r => r.Field<int>(datatext)).Take(5).ToList(); 
                    //List<int> data1 = Values.AsEnumerable().Select(r => r.Field<int>("58019_JUNKER")).Take(5).ToList();
                    seriesLs1.name = datatext;
                    seriesLs1.data = Values.AsEnumerable().Select(r => r.Field<int?>(datatext) ?? 0).Take(downReasons).ToList();
                    chart.seriesTimeWise.Add(seriesLs1);
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }

            return chart;
        }
        #endregion

        #region "Get MCs by Top-5 Downs second Graph"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ChartData<SeriesTimeWise> TopDownbyMCChart(string plantID, string machineId, string startFromDate, string startToDate, string downID, bool exclude, int downReasons, string CellID)
        {
            var chart = new ChartData<SeriesTimeWise>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = "XAxisTitle",
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };

            try
            {
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;

                fromDate = Util.GetDateTime(startFromDate);
                toDate = Util.GetDateTime(startToDate);


                string plantId = string.Empty;
                #region "Machine selection"
                if (machineId == "null")
                {
                    machineId = "";
                }
                #endregion

                string downId = "";
                int Exclude = 0;

                #region "DownId Selection "
                if (downID != "null")
                {
                    string[] result = downID.Split(new string[] { "," }, StringSplitOptions.None);
                    result = result.Take(result.Count()).ToArray();
                    downId = string.Join(",", result.ToArray());
                }
                if (exclude)
                    Exclude = 1;
                else
                    Exclude = 0;
                #endregion

                if (plantID.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    plantId = "";
                DataTable dt = null;
                if (HttpContext.Current.Session["DownTime"] == null)
                    dt = TMPTrakDataBase.MachineDownTimeMatrix(fromDate, toDate, machineId, plantId, downId, Exclude, "DTime", CellID, "s_GetDownTimeMatrixfromAutoData", "");
                else
                    dt = HttpContext.Current.Session["DownTime"] as DataTable;
                DataView view = new DataView(dt);
                DataTable headerValues = view.ToTable(true, MachineColName);
                DataTable Values = new DataTable();
                Values.Columns.Add("DownCode", typeof(string));
                for (int i = 0; i < headerValues.Rows.Count; i++)
                {
                    string strValue = headerValues.Rows[i][MachineColName].ToString();
                    Values.Columns.Add(strValue, typeof(int));
                }

                string downCode = string.Empty;
                DataRow row = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DownCode"].ToString() != downCode)
                    {
                        row = Values.NewRow();
                        Values.Rows.Add(row);
                        Values.Rows[Values.Rows.Count - 1]["DownCode"] = dt.Rows[i]["DownCode"];
                    }
                    Values.Rows[Values.Rows.Count - 1][dt.Rows[i][MachineColName].ToString()] = dt.Rows[i]["DownTime"];
                    downCode = dt.Rows[i]["DownCode"].ToString();
                }

                chart.seriesTimeWise = new List<SeriesTimeWise>();

                List<string> catData = new List<string>();
                for (int i = 1; i < Values.Columns.Count; i++)
                {
                    string cat = Values.Columns[i].ToString();
                    catData.Add(cat);
                }
                chart.categories = catData;
                bool flag = true;
                for (int i = 0; i < Values.Rows.Count; i++)
                {
                    SeriesTimeWise seriesLs1 = new SeriesTimeWise();
                    if (i == downReasons)
                    {
                        flag = false;
                        break;
                    }
                    if (flag)
                    {
                        var datatext = Values.Rows[i][0].ToString();
                        seriesLs1.name = Values.Rows[i][0].ToString();
                        seriesLs1.data = Values.Rows[i].ItemArray.Skip(1).OfType<int>().ToList();
                        chart.seriesTimeWise.Add(seriesLs1);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }

            return chart;
        }
        #endregion

        #region "Get MCs by Top-5 Downs Time Freq third Graph"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ChartData<SeriesTimeWise> MCsbyTopDownFreqChart(string plantID, string machineId, string startFromDate, string startToDate, string downID, bool exclude, int downReasons, string CellID)
        {
            var chart = new ChartData<SeriesTimeWise>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = "XAxisTitle",
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };

            try
            {
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;

                fromDate = Util.GetDateTime(startFromDate);
                toDate = Util.GetDateTime(startToDate);


                string plantId = string.Empty;
                #region "Machine selection"
                if (machineId == "null")
                {
                    machineId = "";
                }
                #endregion

                string downId = "";
                int Exclude = 0;

                #region "DownId Selection "
                if (downID != "null")
                {
                    string[] result = downID.Split(new string[] { "," }, StringSplitOptions.None);
                    result = result.Take(result.Count()).ToArray();
                    downId = string.Join(",", result.ToArray());
                }
                if (exclude)
                    Exclude = 1;
                else
                    Exclude = 0;
                #endregion

                if (plantID.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    plantId = "";
                DataTable dt = null;
                if (HttpContext.Current.Session["DownTimeFreq"] == null)
                    dt = TMPTrakDataBase.MachineDownTimeMatrix(fromDate, toDate, machineId, plantId, downId, Exclude, "DFreq", CellID, "s_GetDownTimeMatrixfromAutoData", "");
                else
                    dt = HttpContext.Current.Session["DownTimeFreq"] as DataTable;
                DataView view = new DataView(dt);
                DataTable headerValues = view.ToTable(true, MachineColName);
                DataTable Values = new DataTable();
                Values.Columns.Add("DownCode", typeof(string));
                for (int i = 0; i < headerValues.Rows.Count; i++)
                {
                    string strValue = headerValues.Rows[i][MachineColName].ToString();
                    Values.Columns.Add(strValue, typeof(int));
                }

                string downCode = string.Empty;
                DataRow row = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DownCode"].ToString() != downCode)
                    {
                        row = Values.NewRow();
                        Values.Rows.Add(row);
                        Values.Rows[Values.Rows.Count - 1]["DownCode"] = dt.Rows[i]["DownCode"];
                    }
                    Values.Rows[Values.Rows.Count - 1][dt.Rows[i][MachineColName].ToString()] = Convert.ToInt32(dt.Rows[i]["DownFreq"]);
                    downCode = dt.Rows[i]["DownCode"].ToString();
                }

                chart.seriesTimeWise = new List<SeriesTimeWise>();
                List<string> cat = Values.AsEnumerable().Select(r => r.Field<string>("DownCode")).Take(downReasons).ToList();
                chart.categories = cat;

                for (int i = 1; i < Values.Columns.Count; i++)
                {
                    SeriesTimeWise seriesLs1 = new SeriesTimeWise();
                    string datatext = Values.Columns[i].ToString();
                    seriesLs1.name = datatext;
                    seriesLs1.data = Values.AsEnumerable().Select(r => r.Field<int?>(datatext) ?? 0).Take(downReasons).ToList();
                    chart.seriesTimeWise.Add(seriesLs1);
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }

            return chart;
        }
        #endregion

        #region "Get MCs by Top-5 Downs Time Freq fourth Graph"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ChartData<SeriesTimeWise> TopDownFreqbyMCsChart(string plantID, string machineId, string startFromDate, string startToDate, string downID, bool exclude, int downReasons, string CellID)
        {
            var chart = new ChartData<SeriesTimeWise>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = "XAxisTitle",
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };

            try
            {
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;

                fromDate = Util.GetDateTime(startFromDate);
                toDate = Util.GetDateTime(startToDate);


                string plantId = string.Empty;
                #region "Machine selection"
                if (machineId == "null")
                {
                    machineId = "";
                }
                #endregion

                string downId = "";
                int Exclude = 0;

                #region "DownId Selection "
                if (downID != "null")
                {
                    string[] result = downID.Split(new string[] { "," }, StringSplitOptions.None);
                    result = result.Take(result.Count()).ToArray();
                    downId = string.Join(",", result.ToArray());
                }
                if (exclude)
                    Exclude = 1;
                else
                    Exclude = 0;
                #endregion

                if (plantID.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    plantId = "";
                DataTable dt = null;
                if (HttpContext.Current.Session["DownTimeFreq"] == null)
                    dt = TMPTrakDataBase.MachineDownTimeMatrix(fromDate, toDate, machineId, plantId, downId, Exclude, "DFreq", CellID, "s_GetDownTimeMatrixfromAutoData", "");
                else
                    dt = HttpContext.Current.Session["DownTimeFreq"] as DataTable;
                DataView view = new DataView(dt);
                DataTable headerValues = view.ToTable(true, MachineColName);
                DataTable Values = new DataTable();
                Values.Columns.Add("DownCode", typeof(string));
                for (int i = 0; i < headerValues.Rows.Count; i++)
                {
                    string strValue = headerValues.Rows[i][MachineColName].ToString();
                    Values.Columns.Add(strValue, typeof(int));
                }

                string downCode = string.Empty;
                DataRow row = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DownCode"].ToString() != downCode)
                    {
                        row = Values.NewRow();
                        Values.Rows.Add(row);
                        Values.Rows[Values.Rows.Count - 1]["DownCode"] = dt.Rows[i]["DownCode"];
                    }
                    Values.Rows[Values.Rows.Count - 1][dt.Rows[i][MachineColName].ToString()] = dt.Rows[i]["DownFreq"];
                    downCode = dt.Rows[i]["DownCode"].ToString();
                }

                chart.seriesTimeWise = new List<SeriesTimeWise>();

                List<string> catData = new List<string>();
                for (int i = 1; i < Values.Columns.Count; i++)
                {
                    string cat = Values.Columns[i].ToString();
                    catData.Add(cat);
                }
                chart.categories = catData;
                bool flag = true;
                for (int i = 0; i < Values.Rows.Count; i++)
                {
                    SeriesTimeWise seriesLs1 = new SeriesTimeWise();
                    if (i == downReasons)
                    {
                        flag = false;
                        break;
                    }
                    if (flag)
                    {
                        var datatext = Values.Rows[i][0].ToString();
                        seriesLs1.name = Values.Rows[i][0].ToString();
                        seriesLs1.data = Values.Rows[i].ItemArray.Skip(1).OfType<int>().ToList();
                        chart.seriesTimeWise.Add(seriesLs1);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }

            return chart;
        }
        #endregion

        #region "Get Top Down Time Pareto Chart "
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Chart<DrildownData> TopDownTimeParetoChart(string plantID, string machineId, string startFromDate, string startToDate, string downID, bool exclude, int downReasons, string CellID)
        {
            var chart = new Chart<DrildownData>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = "XAxisTitle",
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };

            try
            {
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;

                fromDate = Util.GetDateTime(startFromDate);
                toDate = Util.GetDateTime(startToDate);


                string plantId = string.Empty;
                #region "Machine selection"
                if (machineId == "null")
                {
                    machineId = "";
                }
                #endregion

                string downId = "";
                int Exclude = 0;

                #region "DownId Selection "
                if (downID != "null")
                {
                    string[] result = downID.Split(new string[] { "," }, StringSplitOptions.None);
                    result = result.Take(result.Count()).ToArray();
                    downId = string.Join(",", result.ToArray());
                }
                if (exclude)
                    Exclude = 1;
                else
                    Exclude = 0;
                #endregion

                if (plantID.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    plantId = "";
                else
                    plantId = plantID;
                DataTable dt = null;
                if (HttpContext.Current.Session["DownParetoData"] == null)
                    HttpContext.Current.Session["DownParetoData"] = dt = TMPTrakDataBase.MachineDownTimeMatrix(fromDate, toDate, machineId, plantId, downId, Exclude, "DLoss_By_Catagory", CellID, "s_GetDownTimeMatrixfromAutoData", "");
                else
                    dt = HttpContext.Current.Session["DownParetoData"] as DataTable;
                DataView dv = dt.DefaultView;
                dv.Sort = "DownTime desc";
                DataTable sortedDT = dv.ToTable();

                chart.series = new List<DrildownData>();
                List<string> cat = sortedDT.AsEnumerable().Select(r => r.Field<string>("DownID")).ToList();

                //cat.Add("Other");
                chart.categories = new List<string>();
                for (int i = 0; i < cat.Count; i++)
                {
                    string value = cat[i];
                    if (i == downReasons)
                    {
                        chart.categories.Add("Other");
                        chart.categories.Add(value);
                    }
                    else
                        chart.categories.Add(value);
                }
                //chart.series1 = new List<DrildownData>();

                decimal decVal = 0;
                bool flag = true;
                for (int i = 0; i < sortedDT.Rows.Count; i++)
                {
                    DrildownData obj = new DrildownData();
                    if (i == downReasons)
                    {
                        flag = false;
                        List<double> declistVal = sortedDT.AsEnumerable().Select(r => r.Field<double>("DownTime")).Skip(downReasons).ToList();
                        for (int j = 0; j < declistVal.Count; j++)
                        {
                            decVal += Convert.ToDecimal(declistVal[j]);
                        }
                        decimal myNumber = decVal;
                        obj.y = decimal.Round(myNumber, 2);
                        obj.name = "Other";
                        obj.drilldown = "Other";
                        chart.series.Add(obj);
                        break;
                    }
                    if (flag)
                    {
                        decVal += Convert.ToDecimal(sortedDT.Rows[i]["DownTime"].ToString());
                        decimal myNumber = Convert.ToDecimal(sortedDT.Rows[i]["DownTime"].ToString());
                        obj.y = decimal.Round(myNumber, 2);
                    }
                    chart.series.Add(obj);
                }
                //decVal = 0;
                //flag = true;
                //for (int i = 0; i < sortedDT.Rows.Count; i++)
                //{
                //    DrildownData obj = new DrildownData();
                //    if (i == downReasons)
                //    {
                //        flag = false;
                //        List<double> declistVal = sortedDT.AsEnumerable().Select(r => r.Field<double>("DownTime")).Skip(downReasons).ToList();
                //        for (int j = 0; j < declistVal.Count; j++)
                //        {
                //            decVal += Convert.ToDecimal(declistVal[j]);
                //        }
                //        decimal myNumber = decVal;
                //        obj.y = decimal.Round(myNumber, 2);
                //        obj.name = "Other";
                //        obj.drilldown = "Other";
                //        chart.series1.Add(obj);
                //        break;
                //    }
                //    if (flag)
                //    {
                //        decVal += Convert.ToDecimal(sortedDT.Rows[i]["DownTime"].ToString());
                //        decimal myNumber = Convert.ToDecimal(sortedDT.Rows[i]["DownTime"].ToString());
                //        obj.y = decimal.Round(myNumber, 2);
                //        obj.name = sortedDT.Rows[i]["DownID"].ToString();
                //    }
                //    chart.series1.Add(obj);
                //}

                var rows = sortedDT.AsEnumerable().Skip(downReasons).ToList();
                chart.drilldown = new List<DrildownSeries>();
                chart.drilldown.Add(new DrildownSeries
                {
                    name = "Other",
                    id = "Other",
                    data = new List<DrildownData>(),
                });
                foreach (var machine in rows)
                {
                    chart.drilldown[0].data.Add(new DrildownData
                    {
                        name = machine[1].ToString(),
                        y = Convert.ToDecimal(machine[2].ToString()),
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
            return chart;
        }
        #endregion

        //#region "Get Catagory by Down Time Chart "
        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static Chart<Series> CatagoryByDownTimeParetoChart(string plantID, string machineId, string startFromDate, string startToDate, string downID, bool exclude, int downReasons)
        //{
        //    var chart = new Chart<Series>
        //    {
        //        Title = "TITLE",
        //        Subtitle = "SubTitle",
        //        XAxisTitle = "XAxisTitle",
        //        YAxisTitle = "YAxisTitle",
        //        YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
        //    };

        //    try
        //    {
        //        DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;
        //        fromDate = DateTime.Parse(startFromDate, new CultureInfo("en-GB"));
        //        toDate = DateTime.Parse(startToDate, new CultureInfo("en-GB"));

        //        string plantId = string.Empty;
        //        #region "Machine selection"
        //        if (machineId == "null")
        //        {
        //            machineId = "";
        //        }
        //        #endregion

        //        string downId = "";
        //        int Exclude = 0;

        //        #region "DownId Selection "
        //        if (downID != "null")
        //        {
        //            string[] result = downID.Split(new string[] { "," }, StringSplitOptions.None);
        //            result = result.Take(result.Count()).ToArray();
        //            downId = string.Join(",", result.ToArray());
        //        }
        //        if (exclude)
        //            Exclude = 1;
        //        else
        //            Exclude = 0;
        //        #endregion

        //        if (plantID.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase))
        //            plantId = "";
        //        DataTable dt = null;
        //        if (HttpContext.Current.Session["DownParetoData"] == null)
        //            HttpContext.Current.Session["DownParetoData"] = dt = TMPTrakDataBase.MachineDownTimeMatrix(fromDate, toDate, machineId, plantId, downId, Exclude, "DLoss_By_Catagory");
        //        else
        //            dt = HttpContext.Current.Session["DownParetoData"] as DataTable;
        //        DataTable sortedDT = new DataTable();

        //        chart.series = new List<Series>();

        //        //chart.series.Add(new Series
        //        //{
        //        //    name = "Pareto",
        //        //    type = "pareto",
        //        //    yAxis = 1,
        //        //    zIndex = 10,
        //        //    baseSeries = 1,
        //        //});
        //        chart.series.Add(new Series
        //        {
        //            name = "Catagory",
        //            type = "column",
        //            colorByPoint = true,
        //            // zIndex = 2,
        //            data = new List<Data>()
        //        });

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            //sortedDT = sortedDT.AsEnumerable().GroupBy(r => new { Col1 = r["Catagory"] }).Select(g => g.OrderBy(r => r["Catagory"]).First()).CopyToDataTable();
        //            sortedDT = dt.AsEnumerable()
        //                //.OrderBy(r => r.Field<double>("DownTime"))
        //                             .GroupBy(r => r.Field<string>("Catagory").ToUpper())
        //                             .OrderBy(g => g.Max(r => r.Field<double>("DownTime")))
        //                             .Select(g =>
        //                             {
        //                                 var row = dt.NewRow();
        //                                 row["Catagory"] = g.Key.ToUpper();
        //                                 row["DownTime"] = g.Sum(r => r.Field<double>("DownTime"));
        //                                 return row;
        //                             }).CopyToDataTable();
        //        }
        //        //DataView dv = sortedDT.DefaultView;
        //        //dv.Sort = "DownTime asc";
        //        //sortedDT = dv.ToTable();
        //        //List<string> cat = sortedDT.AsEnumerable().Select(r => r.Field<string>("Catagory")).ToList();
        //        //chart.categories = cat;
        //        for (int i = 0; i < sortedDT.Rows.Count; i++)
        //        {
        //            Data obj = new Data();
        //            decimal myNumber = Convert.ToDecimal(sortedDT.Rows[i]["DownTime"].ToString());
        //            obj.y = decimal.Round(myNumber, 2);
        //            obj.name = sortedDT.Rows[i]["Catagory"].ToString();
        //            obj.drilldown = sortedDT.Rows[i]["Catagory"].ToString();
        //            chart.series[0].data.Add(obj);
        //        }

        //        chart.drilldown = new List<DrildownSeries>();
        //        for (int i = 0; i < sortedDT.Rows.Count; i++)
        //        {
        //            chart.drilldown.Add(new DrildownSeries
        //            {
        //                type = "column",
        //                name = sortedDT.Rows[i]["Catagory"].ToString(),
        //                id = sortedDT.Rows[i]["Catagory"].ToString(),
        //                data = new List<DrildownData>(),
        //            });
        //            var results = dt.Select("Catagory = '" + sortedDT.Rows[i]["Catagory"].ToString() + "'").OrderBy(dr => dr[2]);
        //            if (results != null)
        //            {
        //                foreach (DataRow drCode in results)
        //                {
        //                    chart.drilldown[i].data.Add(new DrildownData
        //                    {
        //                        name = drCode["DownID"].ToString(),
        //                        y = Convert.ToDecimal(drCode["DownTime"].ToString()),
        //                    });
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorSignal.FromCurrentContext().Raise(ex);
        //        //lblMessages.Text = ex.ToString();
        //        Logger.WriteErrorLog(ex.ToString());
        //    }

        //    return chart;
        //}
        //#endregion

        #region "Get Top Down Time Pareto Chart "
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Chart<DrildownData> TopDownTimeParetoChartColAndPie(string plantID, string machineId, string startFromDate, string startToDate, string downID, bool exclude, int downReasons, string CellID)
        {
            var chart = new Chart<DrildownData>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = "XAxisTitle",
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };

            try
            {
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;


                fromDate = Util.GetDateTime(startFromDate);
                toDate = Util.GetDateTime(startToDate);


                string plantId = string.Empty;
                #region "Machine selection"
                if (machineId == "null")
                {
                    machineId = "";
                }
                #endregion

                string downId = "";
                int Exclude = 0;

                #region "DownId Selection "
                if (downID != "null")
                {
                    string[] result = downID.Split(new string[] { "," }, StringSplitOptions.None);
                    result = result.Take(result.Count()).ToArray();
                    downId = string.Join(",", result.ToArray());
                }
                if (exclude)
                    Exclude = 1;
                else
                    Exclude = 0;
                #endregion

                if (plantID.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    plantId = "";
                else
                    plantId = plantID;
                DataTable dt = null;
                if (HttpContext.Current.Session["DownParetoData"] == null)
                    HttpContext.Current.Session["DownParetoData"] = dt = TMPTrakDataBase.MachineDownTimeMatrix(fromDate, toDate, machineId, plantId, downId, Exclude, "DLoss_By_Catagory", CellID, "s_GetDownTimeMatrixfromAutoData", "");
                else
                    dt = HttpContext.Current.Session["DownParetoData"] as DataTable;
                DataView dv = dt.DefaultView;
                dv.Sort = "DownTime desc";
                DataTable sortedDT = dv.ToTable();

                chart.series = new List<DrildownData>();
                List<string> cat = sortedDT.AsEnumerable().Select(r => r.Field<string>("DownID")).ToList();

                //cat.Add("Other");
                chart.categories = new List<string>();
                for (int i = 0; i < cat.Count; i++)
                {
                    string value = cat[i];
                    if (i == downReasons)
                    {
                        chart.categories.Add("Other");
                        chart.categories.Add(value);
                    }
                    else
                        chart.categories.Add(value);
                }

                decimal decVal = 0;
                bool flag = true;
                for (int i = 0; i < sortedDT.Rows.Count; i++)
                {
                    DrildownData obj = new DrildownData();
                    if (i == downReasons)
                    {
                        flag = false;
                        List<double> declistVal = sortedDT.AsEnumerable().Select(r => r.Field<double>("DownTime")).Skip(downReasons).ToList();
                        for (int j = 0; j < declistVal.Count; j++)
                        {
                            decVal += Convert.ToDecimal(declistVal[j]);
                        }
                        decimal myNumber = decVal;
                        obj.y = decimal.Round(myNumber, 2);
                        obj.name = "Other";
                        obj.drilldown = "Other";
                        chart.series.Add(obj);
                        chart.categories.Add("Other");
                        break;
                    }
                    if (flag)
                    {
                        //decVal += Convert.ToDecimal(sortedDT.Rows[i]["DownTime"].ToString());
                        decimal myNumber = Convert.ToDecimal(sortedDT.Rows[i]["DownTime"].ToString());
                        obj.y = decimal.Round(myNumber, 2);
                        obj.name = sortedDT.Rows[i]["DownID"].ToString();
                    }
                    chart.series.Add(obj);
                    // chart.categories.Add(sortedDT.Rows[i]["DownTime"].ToString());
                }

                //chart.series1 = new List<DrildownData>();

                //decVal = 0;
                //flag = true;
                //for (int i = 0; i < sortedDT.Rows.Count; i++)
                //{
                //    DrildownData obj = new DrildownData();
                //    if (i == downReasons)
                //    {
                //        flag = false;
                //        List<double> declistVal = sortedDT.AsEnumerable().Select(r => r.Field<double>("DownTime")).Skip(downReasons).ToList();
                //        for (int j = 0; j < declistVal.Count; j++)
                //        {
                //            decVal += Convert.ToDecimal(declistVal[j]);
                //        }
                //        decimal myNumber = decVal;
                //        obj.y = decimal.Round(myNumber, 2);
                //        obj.name = "Other";
                //        obj.drilldown = "Other";
                //        chart.series1.Add(obj);
                //        // chart.categories.Add("Other");
                //        break;
                //    }
                //    if (flag)
                //    {
                //        decVal += Convert.ToDecimal(sortedDT.Rows[i]["DownTime"].ToString());
                //        decimal myNumber = Convert.ToDecimal(sortedDT.Rows[i]["DownTime"].ToString());
                //        obj.y = decimal.Round(myNumber, 2);
                //        obj.name = sortedDT.Rows[i]["DownID"].ToString();
                //        // chart.categories.Add(sortedDT.Rows[i]["DownID"].ToString());
                //    }
                //    chart.series1.Add(obj);
                //}

                var rows = sortedDT.AsEnumerable().Skip(downReasons).ToList();
                chart.drilldown = new List<DrildownSeries>();
                chart.drilldown.Add(new DrildownSeries
                {
                    //type = "column",
                    name = "Other",
                    id = "Other",
                    colorByPoint = true,
                    data = new List<DrildownData>(),
                    // categories = new List<string>(),
                });
                foreach (var machine in rows)
                {
                    chart.drilldown[0].data.Add(new DrildownData
                    {
                        name = machine[1].ToString(),
                        y = Convert.ToDecimal(machine[2].ToString()),
                    });
                    //chart.drilldown[0].categories.Add(machine[1].ToString());
                }

                //rows = sortedDT.AsEnumerable().Skip(downReasons).ToList();
                chart.drilldown.Add(new DrildownSeries
                {
                    colorByPoint = true,
                    id = "Other",
                    name = "Other",
                    //type = "pie",
                    data = new List<DrildownData>(),
                    //categories = new List<string>(),
                });
                foreach (var machine in rows)
                {
                    chart.drilldown[1].data.Add(new DrildownData
                    {
                        name = machine[1].ToString(),
                        y = Convert.ToDecimal(machine[2].ToString()),
                    });
                    //chart.drilldown[1].categories.Add(machine[1].ToString());
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
            return chart;
        }
        #endregion

        #region "Get Catagory by Down Time Chart "
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Chart<Series> CatagoryByDownTimeParetoChart(string plantID, string machineId, string startFromDate, string startToDate, string downID, bool exclude, int downReasons, string CellID)
        {
            var chart = new Chart<Series>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = "XAxisTitle",
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };

            try
            {
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;


                fromDate = Util.GetDateTime(startFromDate);
                toDate = Util.GetDateTime(startToDate);


                string plantId = string.Empty;
                #region "Machine selection"
                if (machineId == "null")
                {
                    machineId = "";
                }
                #endregion

                string downId = "";
                int Exclude = 0;

                #region "DownId Selection "
                if (downID != "null")
                {
                    string[] result = downID.Split(new string[] { "," }, StringSplitOptions.None);
                    result = result.Take(result.Count()).ToArray();
                    downId = string.Join(",", result.ToArray());
                }
                if (exclude)
                    Exclude = 1;
                else
                    Exclude = 0;
                #endregion

                if (plantID.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    plantId = "";
                else
                    plantId = plantID;
                DataTable dt = null;
                if (HttpContext.Current.Session["DownParetoData"] == null)
                    HttpContext.Current.Session["DownParetoData"] = dt = TMPTrakDataBase.MachineDownTimeMatrix(fromDate, toDate, machineId, plantId, downId, Exclude, "DLoss_By_Catagory", CellID, "s_GetDownTimeMatrixfromAutoData", "");
                else
                    dt = HttpContext.Current.Session["DownParetoData"] as DataTable;
                DataTable sortedDT = new DataTable();

                chart.series = new List<Series>();

                //chart.series.Add(new Series
                //{
                //    name = "Pareto",
                //    type = "pareto",
                //    yAxis = 1,
                //    zIndex = 10,
                //    baseSeries = 1,
                //});
                chart.series.Add(new Series
                {
                    name = HttpContext.GetLocalResourceObject("~/MachineDownReasonTimeMatrix.aspx", "Category").ToString() == null ? "Category" : HttpContext.GetLocalResourceObject("~/MachineDownReasonTimeMatrix.aspx", "Category").ToString(),
                    type = "column",
                    colorByPoint = true,
                    zIndex = 2,
                    data = new List<Data>()
                });

                if (dt != null && dt.Rows.Count > 0)
                {
                    //sortedDT = sortedDT.AsEnumerable().GroupBy(r => new { Col1 = r["Catagory"] }).Select(g => g.OrderBy(r => r["Catagory"]).First()).CopyToDataTable();
                    sortedDT = dt.AsEnumerable()
                                     //.OrderBy(r => r.Field<double>("DownTime"))
                                     .GroupBy(r => r.Field<string>("Catagory").ToUpper())
                                     .OrderByDescending(g => g.Max(r => r.Field<double>("DownTime")))
                                     .Select(g =>
                                     {
                                         var row = dt.NewRow();
                                         row["Catagory"] = g.Key.ToUpper();
                                         row["DownTime"] = g.Sum(r => r.Field<double>("DownTime"));
                                         return row;
                                     }).CopyToDataTable();
                }
                //List<string> cat = sortedDT.AsEnumerable().Select(r => r.Field<string>("Catagory")).ToList();
                //chart.categories = new List<string>();
                //for (int i = 0; i < cat.Count; i++)
                //{
                //    string value = cat[i];
                //    chart.categories.Add(value);
                //}
                //DataView dv = sortedDT.DefaultView;
                //dv.Sort = "DownTime asc";
                //sortedDT = dv.ToTable();
                //List<string> cat = sortedDT.AsEnumerable().Select(r => r.Field<string>("Catagory")).ToList();
                //chart.categories = cat;
                for (int i = 0; i < sortedDT.Rows.Count; i++)
                {
                    Data obj = new Data();
                    decimal myNumber = Convert.ToDecimal(sortedDT.Rows[i]["DownTime"].ToString());
                    obj.y = decimal.Round(myNumber, 2);
                    obj.name = sortedDT.Rows[i]["Catagory"].ToString();
                    obj.drilldown = sortedDT.Rows[i]["Catagory"].ToString();
                    chart.series[0].data.Add(obj);
                }

                chart.drilldown = new List<DrildownSeries>();
                for (int i = 0; i < sortedDT.Rows.Count; i++)
                {
                    chart.drilldown.Add(new DrildownSeries
                    {
                        colorByPoint = true,
                        type = "column",
                        name = sortedDT.Rows[i]["Catagory"].ToString(),
                        id = sortedDT.Rows[i]["Catagory"].ToString(),
                        data = new List<DrildownData>(),
                    });
                    var results = dt.Select("Catagory = '" + sortedDT.Rows[i]["Catagory"].ToString() + "'").OrderByDescending(dr => dr[2]);
                    if (results != null)
                    {
                        foreach (DataRow drCode in results)
                        {
                            chart.drilldown[i].data.Add(new DrildownData
                            {
                                name = drCode["DownID"].ToString(),
                                y = Convert.ToDecimal(drCode["DownTime"].ToString()),
                            });
                            //chart.categories.Add(drCode["DownID"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
            return chart;
        }

        #endregion

        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachines(ddlCellID.SelectedItem == null ? "" : ddlCellID.SelectedItem.Text);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetCellIdData(string PlantId)
        {
            List<string> lstCellId = new List<string>();
            try
            {
                lstCellId = BindCockpitView.ViewCellsToDisplay(PlantId == "Plant All" || PlantId == "All" ? "" : PlantId);
                lstCellId.Insert(0, "All");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return lstCellId;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetMachineIdData(string CellId, string PlantId)
        {
            List<string> lstMachineId = new List<string>();
            try
            {
                if (string.IsNullOrEmpty(CellId))
                {
                    lstMachineId = VDGDataBaseAccess.GetAllMachines(PlantId);
                }
                else
                {
                    lstMachineId = CockpitDataBaseAccess.GetMachinesForCell(CellId, PlantId);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return lstMachineId;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                var TheBrowserWidth = "1920";
                var TheBrowserHeight = "900";
                int Exclude = 0;
                if (chkExclude.Checked)
                    Exclude = 1;
                else
                    Exclude = 0;
                string machineId = DataBaseAccess.getMachineIDWithSeparator(ddlMachineId);
                string downId = DataBaseAccess.getMachineIDWithSeparator(ddlMultiDownID);
                string Generated = TPMTrakGenerateReportNewDll.MachineDownTimeMatrix(Util.GetDateTime(txtFromDate.Text), ddlPlantId.SelectedValue.ToString(), machineId, Util.GetDateTime(txtToDate.Text), downId, Exclude, TheBrowserWidth, TheBrowserHeight, ddlTopDownReasons.SelectedValue.ToString(), "", "standard", "TimeAndFreqWise");
                if (Generated.Equals("", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "alert('Error generating report.Please Try Again.');", true);
                else if (Generated.Equals("NodataFound", StringComparison.OrdinalIgnoreCase))
                {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNodataformo", "alert('No Data Found');", true);
                }
                //else if (Generated.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "messageOk();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlDayShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedShift = ddlDayShift.SelectedValue.ToString();
                string logicalDayStart = string.Empty, logicalDayEnd = string.Empty;
                if (selectedShift.Contains("Today"))
                {
                    if (ddlDayShift.SelectedValue.Equals("Today - All"))
                    {
                        logicalDayStart = CockpitDataBaseAccess.GetLogicalDay(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("dd-MM-yyyy HH:mm");

                        logicalDayEnd = CockpitDataBaseAccess.GetLogicalDayEnd(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("dd-MM-yyyy HH:mm");
                    }

                    else
                    {
                        int index = selectedShift.IndexOf('-');
                        var shift = CockpitDataBaseAccess.GetCurrentShiftTime(selectedShift.Substring(index + 1).Trim());
                        if (shift != null)
                        {
                            logicalDayStart = shift[0];
                            logicalDayEnd = shift[1];

                            txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("dd-MM-yyyy HH:mm");

                            txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("dd-MM-yyyy HH:mm");
                        }
                    }
                }
                else
                {
                    if (ddlDayShift.SelectedValue.Equals("Yesterday - All"))
                    {
                        logicalDayStart = CockpitDataBaseAccess.GetLogicalDay(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
                        txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("dd-MM-yyyy HH:mm");
                        logicalDayEnd = CockpitDataBaseAccess.GetLogicalDayEnd(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
                        txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("dd-MM-yyyy HH:mm");
                    }
                    else
                    {
                        int index = selectedShift.IndexOf('-');
                        var shift = CockpitDataBaseAccess.GetCurrentShiftTime(selectedShift.Substring(index + 1).Trim());
                        if (shift != null)
                        {
                            logicalDayStart = shift[0];
                            logicalDayEnd = shift[1];
                            txtFromDate.Text = Convert.ToDateTime(logicalDayStart).AddDays(-1).ToString("dd-MM-yyyy HH:mm");
                            txtToDate.Text = Convert.ToDateTime(logicalDayEnd).AddDays(-1).ToString("dd-MM-yyyy HH:mm");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
    }
}