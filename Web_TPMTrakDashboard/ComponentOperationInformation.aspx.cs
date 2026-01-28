using BusinessClassLibrary;
using ModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.KTASpindle;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.ShantiIron.Model;

namespace Web_TPMTrakDashboard
{
    public partial class ComponentOperationInformation : System.Web.UI.Page
    {
        int RowIndex = 0;
        int FinishOperation = 0;
        string TimeFormatVal = string.Empty;//string.Empty;
        List<string> allMachines = DataBaseAccess.GetAllMacForPlant("");
        string componetid = string.Empty;
        string intefaceID = string.Empty;
        static NetworkConnection nc = null;
        public List<UserAccessModel> useAccessData = null;
        public static string admin = "none";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Language"] == null || Session["connectionString"] == null)
            {
                Response.Redirect("~/SignIn.aspx", false);
                return;
            }
            if (!IsPostBack)
            {
                try
                {

                    if (Session["UserAccessData"] == null)
                        Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"].ToString());
                    else
                        useAccessData = Session["UserAccessData"] as List<UserAccessModel>;
                    if (useAccessData != null && !useAccessData.Where(ss => ss.Code.Equals("EnableWriteAccessForCOP", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                    {
                        tdOperations.Visible = false;
                        tdAssignCO.Visible = false;
                    }
                    else
                    {
                        tdOperations.Visible = true;
                        tdAssignCO.Visible = true;
                    }

                    if (ConfigurationManager.AppSettings["PAMSPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        List<string> list = DataBaseAccess.GetPAMSProcess();
                        list.Insert(0, "");
                        ddlProcess.DataSource = list;
                        ddlProcess.DataBind();

                        tdProcessName.Visible = true;
                        tdProcessControl.Visible = true;
                    }
                    else
                    {
                        tdProcessName.Visible = false;
                        tdProcessControl.Visible = false;
                    }
                    if (ConfigurationManager.AppSettings["PittiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        tdInputCodeName.Visible = true;
                        tdInputCodeControl.Visible = true;
                        tdOutputCodeName.Visible = true;
                        tdOutputCodeControl.Visible = true;
                        tdIsManualOpnName.Visible = true;
                        tdIsManualOpnControl.Visible = true;
                    }
                    else
                    {
                        tdInputCodeName.Visible = false;
                        tdInputCodeControl.Visible = false;
                        tdOutputCodeName.Visible = false;
                        tdOutputCodeControl.Visible = false;
                        tdIsManualOpnName.Visible = false;
                        tdIsManualOpnControl.Visible = false;
                    }
                    Session["COPTimeFormat"] = (VDGDataBaseAccess.GetShopdefaultsTimeFormat()).ToLower();
                    componetid = Request.QueryString["name"].ToString();
                    Session["intefaceID"] = intefaceID = Request.QueryString["intefaceID"].ToString();
                    ComponentInfogrd(componetid);
                    if (!DataBaseAccess.GetStatusOfSubOperator())
                    {
                        txtsubop.Text = "1";
                        txtsubop.Enabled = false;
                    }
                    if (WebConfigurationManager.AppSettings["FinishedComponent"].Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        chkFinishOperation.Visible = true;
                        lblFinishOpr.Visible = true;
                        componentgrd.Columns[11].Visible = true;
                    }
                    else
                    {
                        chkFinishOperation.Visible = false;
                        lblFinishOpr.Visible = false;
                        componentgrd.Columns[11].Visible = false;
                    }
                    if (WebConfigurationManager.AppSettings["MinLULThreshold"].Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        tdMinLULThreHeader.Visible = true;
                        tdMinLULThreContent.Visible = true;

                    }
                    else
                    {
                        tdMinLULThreHeader.Visible = false;
                        tdMinLULThreContent.Visible = false;
                    }
                    if (ConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("0", StringComparison.OrdinalIgnoreCase))
                        btnCreateFolder.Visible = false;
                    else
                        btnCreateFolder.Visible = true;
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog($"Page_Load: {ex.Message}");
                }
            }

            // btnUpdateStdTime.Visible = false;
        }


        #region -------------Grid Binding---------------
        public void ComponentInfogrd(string componentId)
        {
            try
            {
                //txtsearch.Text = componentId.ToString();
                DataTable dt = DataBaseAccess.GetComponentDetails(componentId, "", "ViewCompOpnInfo");
                if (!dt.Columns.Contains("IncentiveTime"))
                {
                    dt.Columns.Add("IncentiveTime", typeof(string));
                }
                componentgrd.DataSource = dt;
                componentgrd.DataBind();

                //onsorting purpose
                Session["dirState"] = dt;
                Session["SortByValue"] = "operationno";
                Session["sortdr"] = "Asc";

                //end sorting 
                SetTimeFormat();
                BindPlantCell();
                
                DataTable componentDt = DataBaseAccess.GetAllComponentz();
                if (componentDt != null && componentDt.Rows.Count > 0)
                {
                    ddlComponentID.DataSource = componentDt;
                    ddlComponentID.DataTextField = "ComponentId";
                    ddlComponentID.DataValueField = "ComponentId";
                    ddlComponentID.DataBind();
                    HelperClassGeneric.setDropdownValue(ddlComponentID, componentId);
                    DataTable dt1 = componentgrd.DataSource as DataTable;
                    if (dt1 != null)
                    {
                        SetValuesToControls(dt1, 0, 0);
                    }
                    else
                    {
                        btnnew_Click(null, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"ComponentInfogrd: {ex.Message}");
            }
        }

        private void SetTimeFormat()
        {
            try
            {
                TimeFormatVal = DataBaseAccess.GetTimeFormat();
                if (TimeFormatVal == "ss")
                {
                    lgStandartTime.InnerText = GetLocalResourceObject("StdTimeSec").ToString();
                    lblstdTime.Text = GetLocalResourceObject("StdSetupSec").ToString();
                }
                else if (TimeFormatVal == "mm")
                {
                    lgStandartTime.InnerText = GetLocalResourceObject("StdTimeMin").ToString();
                    lblstdTime.Text = GetLocalResourceObject("StdSetupMin").ToString();
                }
                else
                {
                    lgStandartTime.InnerText = GetLocalResourceObject("StdTimeSec").ToString();
                    lblstdTime.Text = GetLocalResourceObject("StdSetupSec").ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"SetTimeFormat: {ex.Message}");
            }
        }
        #endregion

        #region -------------Plant & Machine---------------
        private void ComponentInformation_Shown()
        {
            try
            {
                List<string> allPlants = DataBaseAccess.GetAllPlants();

                if (allPlants != null && allPlants.Count > 0)
                {
                    ddlPlantId.DataSource = allPlants;
                    ddlPlantId.DataBind();
                    //allMachines = DataBaseAccess.GetAllMacForPlant(ddlPlantId.Text);
                    //if (allMachines != null && allMachines.Count > 0)
                    //{
                    //    ddlMultiDownID.DataSource = allMachines;
                    //    ddlMultiDownID.DataBind();
                    //}             
                    BindMachines();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"ComponentInformation_Shown: {ex.Message}");
            }
        }
        private void BindMachines()
        {
            try
            {
                allMachines = DataBaseAccess.GetAllMacForPlant_COP(ddlPlantId.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantId.SelectedValue.ToString());
                if (allMachines != null && allMachines.Count > 0)
                {
                    ddlMultiDownID.DataSource = allMachines;
                    ddlMultiDownID.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        #endregion


        #region ----Plant ,Cell & MAachine for KTA---

        private void BindPlantCell()
        {
            try
            {
                List<string> allPlants = DataBaseAccess.GetAllPlants();
                if (allPlants != null && allPlants.Count > 0)
                {
                    ddlPlantId.DataSource = allPlants;
                    ddlPlantId.DataBind();
                    BindCellMachine();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindCellMachine()
        {
            try
            {
                if(!string.IsNullOrEmpty(ddlPlantId.Text))
                {
                    List<string> allcellID = DataBaseAccess.GetAllGroupId(ddlPlantId.Text);
                    ddlMultiCellID.DataSource = allcellID;
                    ddlMultiCellID.DataBind();
                    BindMachinesForPlantCell();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        internal static string getCellIDWithSeparator(ListBox listBox)
        {
            string cell = "";
            try
            {
                foreach (ListItem item in listBox.Items)
                {
                    if (item.Selected)
                    {
                        if (cell == "")
                            cell += item.Value;
                        else
                            cell += "," + item.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getCellIDWithSeparator = " + ex.ToString());
            }
            finally
            {
            }
            return cell;
        }
        private void BindMachinesForPlantCell()
        {
            try
            {
                string plantId = ddlPlantId.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantId.SelectedValue;
                string cellId = DataBaseAccess.getCellIDWithSeparator(ddlMultiCellID);
                ddlMultiDownID.DataSource = DataBaseAccess.getMachineIDListForScreen(plantId, cellId, "");
                ddlMultiDownID.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        #endregion

        private string BindCellBasedOnPlantMAchine(string MachineID, string PlantID)
        {
            string CellID = "";
            try
            {
                CellID = DataBaseAccess.getCellID(MachineID, PlantID);
                if (CellID != "")
                {
                    ddlMultiCellID.SelectedValue = CellID;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindCellBasedOnPlantMAchine: {ex.Message}");
            }
            return CellID;
        }

        #region -------------Save and setValue from textbox ---------------
        private void SetValuesToControls(DataTable dt, int index, int selectedRowIndex)
        {
            try
            {
                double standardLoad = 0.0;
                double standardCycleTime = 0.0;
                Int64 loadUnloadVal = 0;
                if (dt == null || dt.Rows.Count < 0) return;
                dt.Columns["CycleTime"].ReadOnly = false;
                dt.Columns["LoadUnload"].ReadOnly = false;
                dt.Columns["MachiningTime"].ReadOnly = false;
                allMachines = DataBaseAccess.GetAllMacForPlant("");
                if (allMachines != null && allMachines.Count > 0)
                {
                    ddlMultiDownID.DataSource = allMachines;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GetStdCycleAndLoadUnloadTimeForGrid(dt.Rows[i]["Machineid"].ToString(), dt.Rows[i]["Componentid"].ToString(), dt.Rows[i]["OperationNo"].ToString(), out standardLoad, out standardCycleTime, out loadUnloadVal);
                    dt.Rows[i]["MachiningTime"] = standardCycleTime.ToString();
                    dt.Rows[i]["LoadUnload"] = standardLoad.ToString();

                    var val = standardCycleTime + standardLoad;
                    dt.Rows[i]["CycleTime"] = val.ToString();
                    dt.Rows[i]["LoadUnloadThreshold"] = loadUnloadVal.ToString();
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    btnassign.Visible = true;
                    btnUpdateStdTime.Visible = true;
                    RowIndex = index;
                    txtopn_no.Text = dt.Rows[RowIndex]["OperationNo"].ToString();
                    txtdrawing.Text = dt.Rows[RowIndex]["DrawingNo"].ToString();
                    txtdescription.Text = dt.Rows[RowIndex]["Description"].ToString();
                    txtsubop.Text = dt.Rows[RowIndex]["SubOperations"].ToString();
                    txtinterfaceid.Text = dt.Rows[RowIndex]["InterfaceID"].ToString();
                    txttarget.Text = dt.Rows[RowIndex]["TargetPercent"].ToString();
                    txtprice.Text = dt.Rows[RowIndex]["Price"].ToString();
                    ddlMultiDownID.SelectedValue = dt.Rows[RowIndex]["machineid"].ToString();
                    txtmachinetime.Text = dt.Rows[RowIndex]["MachiningTime"].ToString();
                    txtCycleTime.Text = dt.Rows[RowIndex]["CycleTime"].ToString();
                    txtloadunload.Text = dt.Rows[RowIndex]["LoadUnload"].ToString();
                    txtsetuptime.Text = dt.Rows[RowIndex]["StdSetUpTime"].ToString();
                    txtloadunloadthreshold.Text = dt.Rows[RowIndex]["LoadUnloadThreshold"].ToString();
                    txtcuttingtime.Text = dt.Rows[RowIndex]["MachiningTimeThreshold"].ToString();
                    txtminLoadUnloadThreshold.Text = dt.Rows[RowIndex]["MinLoadUnloadThreshold"].ToString();
                    txtScIThreshold.Text = dt.Rows[RowIndex]["SCIThreshold"].ToString();
                    txtDCLThreshold.Text = dt.Rows[RowIndex]["DCLThreshold"].ToString();
                    txtIncentiveTime.Text = dt.Rows[RowIndex]["IncentiveTime"].ToString();
                    txtopn_no.ReadOnly = true;
                    txtinterfaceid.ReadOnly = true;
                    txtCycleTime.ReadOnly = true;
                    ddlMultiCellID.Attributes.Add("disabled", "true");
                    ddlMultiDownID.Attributes.Add("disabled", "true");
                    ddlPlantId.Attributes.Add("disabled", "true");
                    if (dt.Rows[RowIndex]["FinishedOperation"] != null && dt.Rows[RowIndex]["FinishedOperation"] != System.DBNull.Value)
                    {
                        if (Convert.ToInt32(dt.Rows[RowIndex]["FinishedOperation"]).Equals(0))
                            chkFinishOperation.Checked = false;
                        else if (Convert.ToInt32(dt.Rows[RowIndex]["FinishedOperation"]).Equals(1))
                            chkFinishOperation.Checked = true;
                        else
                            chkFinishOperation.Checked = false;
                    }
                    else
                        chkFinishOperation.Checked = false;

                    if (ConfigurationManager.AppSettings["PAMSPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        string process = dt.Rows[RowIndex]["Process"].ToString();
                        if (ddlProcess.Items.FindByValue(process) != null)
                        {
                            ddlProcess.SelectedValue = process;
                        }
                    }

                    if (ConfigurationManager.AppSettings["PittiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        txtInputCode.Text = dt.Rows[RowIndex]["InputCode"].ToString();
                        txtOutputCode.Text = dt.Rows[RowIndex]["OutputCode"].ToString();
                        chkIsManualOperation.Checked = dt.Rows[RowIndex]["OperationType"].ToString().Equals("Manual", StringComparison.OrdinalIgnoreCase) ? true : false;
                    }

                    List<string> machinelist = dt.AsEnumerable().Where(k => k.Field<int>("operationno") == Convert.ToInt32(txtopn_no.Text)).Select(k => k.Field<string>("machineid")).ToList();
                    lbUpdateTimeMachine.DataSource = machinelist;
                    lbUpdateTimeMachine.DataBind();

                    foreach (GridViewRow row in componentgrd.Rows)
                    {

                        row.BackColor = Color.Transparent;
                        row.Attributes.Add("rowselected", "false");
                    }
                    GridViewRow row1 = componentgrd.Rows[selectedRowIndex];
                    row1.BackColor = Color.Lime;
                    row1.Attributes.Add("rowselected", "true");
                }
                else
                {
                    lbUpdateTimeMachine.DataSource = null;
                    lbUpdateTimeMachine.DataBind();
                    btnnew_Click(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"SetValuesToControls: {ex.Message}");
            }
        }
        #endregion

        #region -------------GetStdCycleAndLoadUnloadTimeForGrid---------------
        private void GetStdCycleAndLoadUnloadTimeForGrid(String Machineid, string Componentid, string OperatioNo, out Double standardLoad, out Double standardCycleTime, out Int64 loadUnloadVal)
        {
            standardLoad = 0.0;
            standardCycleTime = 0.0;
            string Machiningtime = string.Empty;
            string CycleTime = string.Empty;
            string loadUnload = string.Empty;
            loadUnloadVal = 0;
            try
            {
                TimeFormatVal = DataBaseAccess.GetTimeFormat();
                if (!string.IsNullOrEmpty(TimeFormatVal))
                {
                    if (TimeFormatVal.Equals("ss"))
                    {
                        DataBaseAccess.GetAllStdTimes(Machineid, Componentid, OperatioNo, out CycleTime, out Machiningtime, out loadUnload);
                        standardLoad = Math.Round((Convert.ToDouble(CycleTime) - Convert.ToDouble(Machiningtime)), 3);
                        standardCycleTime = Math.Round(Convert.ToDouble(Machiningtime), 3);
                        loadUnloadVal = (Convert.ToInt64(loadUnload));
                        txtloadunload.Text = standardLoad.ToString();
                        txtmachinetime.Text = standardCycleTime.ToString();
                        txtloadunloadthreshold.Text = loadUnload.ToString();
                    }
                    else
                    {
                        DataBaseAccess.GetAllStdTimes(Machineid, Componentid, OperatioNo, out CycleTime, out Machiningtime, out loadUnload);

                        if (!string.IsNullOrEmpty(CycleTime) && !string.IsNullOrEmpty(Machiningtime))
                        {
                            standardLoad = Math.Round((Convert.ToDouble(CycleTime) - Convert.ToDouble(Machiningtime)), 3);
                            standardCycleTime = Math.Round((Convert.ToDouble(Machiningtime)), 3);
                        }
                        if (standardLoad > 0)
                        {
                            standardLoad = Math.Round(standardLoad / 60.0, 3);
                        }
                        if (standardCycleTime > 0)
                        {
                            standardCycleTime = Math.Round(standardCycleTime / 60.0, 3);
                        }

                        if (Convert.ToDouble(loadUnload) > 0)
                        {
                            loadUnloadVal = (Convert.ToInt64(loadUnload)) / 60;

                        }

                        txtloadunload.Text = standardLoad.ToString();
                        txtmachinetime.Text = standardCycleTime.ToString();
                        txtloadunloadthreshold.Text = loadUnloadVal.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetStdCycleAndLoadUnloadTimeForGrid: {ex.Message}");
            }
        }
        #endregion

        #region -------------componentgrd_RowEditing---------------
        protected void componentgrd_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                int rowIndex = e.NewEditIndex;
                // int rowsindex = componentgrd.SelectedRow.RowIndex;
                if (rowIndex > -1)
                {
                    //txtopn_no.Enabled = false;
                    //txtinterfaceid.Enabled = false;

                    //ddlMultiDownID.Enabled = false;
                    //ddlMultiDownID.Enabled = false;
                    //ddlPlantId.Enabled = false;

                    ddlPlantId.SelectedIndex = -1;
                    DataTable dt = componentgrd.DataSource as DataTable;
                    RowIndex = rowIndex;

                    int j = rowIndex;

                    if (componentgrd.DataSource != null)
                    {
                        string val = componentgrd.Rows[rowIndex].Cells[3].ToString();
                        int i = 0;
                        SetValuesToControls(dt, RowIndex, RowIndex);
                        if (ConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!string.IsNullOrEmpty(val))
                            {
                                BindCellBasedOnPlantMAchine(val, ddlPlantId.SelectedValue);
                                BindMachinesForPlantCell();
                            }
                        }
                        else
                        {
                            allMachines = DataBaseAccess.GetAllMacForPlant("");
                            if (allMachines != null && allMachines.Count > 0)
                            {
                                ddlMultiDownID.DataSource = allMachines;
                            }
                        }
                        for (int index = 0; index < ddlMultiDownID.Items.Count; ++index)
                        {
                            ddlMultiDownID.SelectedIndex = 0;//SelectedItem.Text = "ALL";
                        }
                        if (i < ddlMultiDownID.Items.Count)
                        {
                            foreach (var item in ddlMultiDownID.Items)
                            {
                                if (item.Equals(val))
                                {
                                    ddlMultiDownID.SelectedIndex = i; //(i, true);
                                    break;
                                }
                                i++;

                                if (item.ToString() == val)
                                {
                                    ddlMultiDownID.SelectedIndex = i; //(i, true);
                                    break;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(ddlMultiDownID.SelectedItem.ToString()))
                        {
                            string Plantid = DataBaseAccess.GetPlantID(ddlMultiDownID.SelectedItem.ToString());
                            ddlPlantId.SelectedValue = Plantid;

                            if (ConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                            {
                                BindCellBasedOnPlantMAchine(ddlMultiDownID.SelectedItem.ToString(), ddlPlantId.SelectedValue.ToString());
                            }
                        }
                        if (chkFinishOperation.Checked)
                            FinishOperation = 1;
                        else
                            FinishOperation = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"componentgrd_RowEditing: {ex.Message}");
            }
        }
        #endregion

        #region -------------OnRowDataBound---------------
        protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["ondblclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.componentgrd, "Select$" + e.Row.RowIndex);
                    e.Row.Attributes["style"] = "cursor:pointer";

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"OnRowDataBound: {ex.Message}");
            }
        }
        #endregion

        #region ------------componentgrd_SelectedIndexChanged---------------
        protected void componentgrd_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // int rowIndex = e.NewEditIndex;
                int rowsindex = componentgrd.SelectedRow.RowIndex;
                string componentid = ddlComponentID.SelectedValue;

                //foreach (GridViewRow row in componentgrd.Rows)
                //{

                //    row.BackColor = Color.Transparent;
                //    row.Attributes.Add("rowselected", "false");
                //}
                //GridViewRow row1 = componentgrd.SelectedRow;
                //row1.BackColor = Color.Lime;
                //row1.Attributes.Add("rowselected","true");

                componentgrd.SelectedRow.Focus();
                if (rowsindex > -1)
                {
                    //txtopn_no.Enabled = false;
                    //txtinterfaceid.Enabled = false;

                    //ddlMultiDownID.Enabled = false;
                    //ddlMultiDownID.Enabled = false;
                    //ddlPlantId.Enabled = false;

                    ddlPlantId.SelectedIndex = -1;
                    //DataTable dt2 = componentgrd.DataSource as DataTable;
                    DataTable dt = DataBaseAccess.GetComponentDetails(componentid, "", "ViewCompOpnInfo");
                    if (Session["sortdr"] != null && Session["sortdr"].ToString().Equals("Desc", StringComparison.OrdinalIgnoreCase))
                    {
                        dt.AsEnumerable().OrderByDescending(x => x[$"{Session["SortByValue"].ToString()}"]);
                    }
                    else
                    {
                        dt.AsEnumerable().OrderBy(x => x[$"{ Session["SortByValue"].ToString()}"]);
                    }
                    string opertaionNo = (componentgrd.Rows[componentgrd.SelectedIndex].Cells[0].FindControl("lbl") as Label).Text;
                    string machineID = componentgrd.Rows[componentgrd.SelectedIndex].Cells[2].Text;

                    for (int matchIndex = 0; matchIndex < dt.Rows.Count; matchIndex++)
                    {
                        if (dt.Rows[matchIndex]["machineid"].ToString() == machineID && dt.Rows[matchIndex]["operationno"].ToString() == opertaionNo)
                        {
                            RowIndex = matchIndex;
                            break;
                        }
                    }

                    //RowIndex = rowsindex;
                    //componentgrd.Rows[componentgrd.SelectedIndex].Attributes["style"] = "focus:Lime;";

                    int j = rowsindex;

                    if (dt != null)
                    {

                        string val = componentgrd.Rows[componentgrd.SelectedIndex].Cells[2].Text;//componentgrd.Rows[rowsindex].Cells[3].Text();
                        int i = 0;
                        SetValuesToControls(dt, RowIndex, rowsindex);
                        //List<string> allMachines = DataBaseAccess.GetAllMacForPlant("");
                        if (allMachines != null && allMachines.Count > 0)
                        {
                            ddlMultiDownID.DataSource = allMachines;
                        }
                        for (int index = 0; index < ddlMultiDownID.Items.Count; ++index)
                        {
                            ddlMultiDownID.SelectedIndex = 0;//SelectedItem.Text = "ALL";
                        }
                        if (i < ddlMultiDownID.Items.Count)
                        {
                            foreach (var item in ddlMultiDownID.Items)
                            {
                                if (item.Equals(val))
                                {
                                    ddlMultiDownID.SelectedIndex = RowIndex; //(i, true);
                                    break;
                                }
                                if (item.ToString() == val)
                                {
                                    ddlMultiDownID.SelectedValue = val;
                                    //ddlMultiDownID.SelectedIndex = RowIndex; //(i, true);
                                    break;
                                }
                                i++;
                            }
                        }
                        if (!string.IsNullOrEmpty(ddlMultiDownID.SelectedItem.ToString()))
                        {
                            string Plantid = DataBaseAccess.GetPlantID(ddlMultiDownID.SelectedItem.ToString());
                            if (Plantid != "")
                            {
                                ddlPlantId.SelectedValue = Plantid;
                            }

                            BindCellBasedOnPlantMAchine(ddlMultiDownID.SelectedItem.ToString(), ddlPlantId.SelectedValue.ToString());
                        }
                    }
                }
                if (chkFinishOperation.Checked)
                    FinishOperation = 1;
                else
                    FinishOperation = 0;
            }
            catch (Exception ex)
            { }
        }

        #endregion

        #region -------------select ComponentID---------------
        protected void btncomponent_Click(object sender, EventArgs e)
        {
            //int Index = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex;
            //LinkButton compid = (LinkButton)ComponentIdgrd.Rows[Index].FindControl("btn_componentid");

            //string str = compid.Text;
            //ComponentInfogrd(str);
        }
        #endregion

        #region -------------btnnew_Click---------------
        protected void btnnew_Click(object sender, EventArgs e)
        {
            try
            {
                txtopn_no.Text = string.Empty;
                txtdrawing.Text = string.Empty;
                txtdescription.Text = string.Empty;
                txtsubop.Text = "1";
                txtinterfaceid.Text = string.Empty;
                txttarget.Text = "100";
                txtprice.Text = "1";
                txtmachinetime.Text = "10";
                txtsetuptime.Text = string.Empty;
                txtCycleTime.Text = string.Empty;
                txtcuttingtime.Text = string.Empty;
                txtloadunload.Text = string.Empty;
                txtloadunloadthreshold.Text = string.Empty;
                txtopn_no.ReadOnly = false;
                txtinterfaceid.ReadOnly = false;
                txtCycleTime.ReadOnly = true;
                ddlMultiDownID.Attributes.Remove("disabled");
                ddlPlantId.Attributes.Remove("disabled");
                ddlMultiCellID.Attributes.Remove("disabled");
                btnassign.Visible = false;
                chkFinishOperation.Checked = false;
                txtminLoadUnloadThreshold.Text = string.Empty;
                btnUpdateStdTime.Visible = false;
                txtInputCode.Text = "";
                txtOutputCode.Text = "";
                chkIsManualOperation.Checked = false;
                txtScIThreshold.Text = string.Empty;
                txtDCLThreshold.Text = string.Empty;
                txtIncentiveTime.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"btnnew_Click: {ex.Message}");
            }
        }
        #endregion

        #region ------------btnassign_Click---------------
        protected void btnassign_Click(object sender, EventArgs e)
        {
            try
            {
                int c = componentgrd.Rows.Count;
                if (c > 0)
                {

                    txtopn_no.ReadOnly = false;
                    txtinterfaceid.ReadOnly = false;
                    txtCycleTime.ReadOnly = true;
                    ddlMultiDownID.Attributes.Remove("disabled");
                    ddlPlantId.Attributes.Remove("disabled");
                    ddlMultiCellID.Attributes.Remove("disabled");
                }
                else
                {
                    btnassign.Visible = false;
                    btnUpdateStdTime.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }

        }
        #endregion

        #region ------------btnsave_Click---------------
        protected void btnsave_Click(object sender, EventArgs e)
        {
            bool isSuccessFailure = false;
            //bool IsOperationFinished = false;
            bool rowAdded = false;
            int subop = 0;
            double subop2 = 0;
            string MachineIds = string.Empty;

            foreach (ListItem item in ddlMultiDownID.Items) //(var item in ddlMultiDownID.SelectedValue)
            {
                if (item.Selected)
                {
                    MachineIds += item.Value + ",";
                }
            }

            #region Validations for interface
            if (string.IsNullOrEmpty(txtinterfaceid.Text.ToString()))
            {
                HelperClassGeneric.openWarningModal(this, GetLocalResourceObject("InterfaceIDcannotbeempty").ToString());
                return;
            }
            if (Convert.ToUInt32(txtinterfaceid.Text.ToString()) <= 0)
            {
                HelperClassGeneric.openWarningModal(this, "Operation Id Cannot be ZERO or Negative");
                return;
            }

            if (string.IsNullOrEmpty(txtopn_no.Text.ToString()))
            {
                HelperClassGeneric.openWarningModal(this, GetLocalResourceObject("OperationNocannotbeempty").ToString());
                return;
            }
            if (ddlMultiDownID.SelectedIndex < 1 && ddlMultiDownID.SelectedValue == "")
            {
                HelperClassGeneric.openWarningModal(this, GetLocalResourceObject("Pleaseselectamachine").ToString());
                return;
            }

            if (int.TryParse(txtsubop.Text.ToString(), out subop))
            {
                if (subop < 1)
                {
                    HelperClassGeneric.openWarningModal(this, GetLocalResourceObject("Suboperationmustbegreaterthan0").ToString());
                    return;
                }
            }
            else
            {
                HelperClassGeneric.openWarningModal(this, GetLocalResourceObject("Suboperationmustbeapositiveintegerotherthan0").ToString());
                return;
            }

            if (double.TryParse(txtmachinetime.Text.ToString(), out subop2))
            {
                if (subop == 0)
                {
                    HelperClassGeneric.openWarningModal(this, GetLocalResourceObject("Machiningtimemustbegreaterthan0").ToString());
                    return;
                }
            }
            else
            {
                HelperClassGeneric.openWarningModal(this, GetLocalResourceObject("Machiningtimemustbeapositiveintegerotherthan0").ToString());
                return;
            }
            #endregion

            try
            {
                if (Session["UserName"] == null)
                {
                    Response.Redirect("LoginPage.aspx", false);
                }
                if (chkFinishOperation.Checked)
                    FinishOperation = 1;
                else
                    FinishOperation = 0;
                if (ConfigurationManager.AppSettings["GEAPages"].Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    if (FinishOperation.Equals(1))
                    {
                        DataBaseAccess.RemoveFinishOperation(ddlComponentID.SelectedValue);
                    }
                }
                else
                {
                    if (FinishOperation.Equals(1))
                    {
                        foreach (ListItem item in ddlMultiDownID.Items) //(var item in ddlMultiDownID.SelectedValue)
                        {
                            if (item.Selected)
                                DataBaseAccess.RemoveFinishOperation(ddlComponentID.SelectedValue, item.Value);

                        }
                    }
                }
                double MachineTime = 0, LoadUnloadTime = 0;
                double.TryParse(txtmachinetime.Text.ToString().Trim(), out MachineTime);
                double.TryParse(txtloadunload.Text.ToString().Trim(), out LoadUnloadTime);

                //int MachineTime = 0, LoadUnloadTime = 0;
                //int.TryParse(txtmachinetime.Text.ToString().Trim(), out MachineTime);
                //int.TryParse(txtloadunload.Text.ToString().Trim(), out LoadUnloadTime);

                if (string.IsNullOrEmpty(txtloadunloadthreshold.Text))
                {
                    HelperClassGeneric.openWarningModal(this, "LoadUnload threshold can not be Empty.");
                    return;
                }

                DataBaseAccess.InsertOrUpdateComponentIdDetails(ddlComponentID.SelectedValue, "", "", txtdescription.Text.ToString(), "", "", "", txtopn_no.Text.ToString(), MachineIds,
                txtprice.Text.ToString(), (MachineTime + LoadUnloadTime).ToString().Trim(), txtdrawing.Text.ToString(), txtinterfaceid.Text.ToString(), txtloadunloadthreshold.Text.ToString(),
                txtmachinetime.Text.ToString(), txtsubop.Text.ToString(), txtsetuptime.Text.ToString(), txtcuttingtime.Text.ToString(), txttarget.Text.ToString(),
                 Session["UserName"].ToString(), "", "", "", string.IsNullOrEmpty(txtScIThreshold.Text.Trim()) ? "0" : txtScIThreshold.Text.Trim(), string.IsNullOrEmpty(txtDCLThreshold.Text.Trim()) ? "0" : txtDCLThreshold.Text.Trim(), "", "", "", "", "", "", FinishOperation, "InsertCompOpnInfo", txtminLoadUnloadThreshold.Text, ddlProcess.SelectedValue, "", txtInputCode.Text, txtOutputCode.Text, chkIsManualOperation.Checked, "", txtIncentiveTime.Text, out isSuccessFailure);
                if (isSuccessFailure)
                {
                    rowAdded = true;
                }
                else
                {
                    HelperClassGeneric.openErrorModal(this, GetLocalResourceObject("CouldnotSavetheDetails").ToString());
                }
                if (rowAdded)
                {

                    ComponentInfogrd(ddlComponentID.SelectedValue);
                    HelperClassGeneric.openSuccessModal(this, GetLocalResourceObject("DetailsaddedUpdatedsuccessfully").ToString());
                    //btnSearchComponent_Click(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error - \n" + ex.ToString());
                throw;
            }

        }
        #endregion

        #region ------------btndelete_Click---------------
        protected void btndelete_Click(object sender, EventArgs e)
        {

            bool isSuccessOrFailure = false;
            string MachineIds = string.Empty;
            string interfaceid = Session["intefaceID"].ToString();
            foreach (ListItem item in ddlMultiDownID.Items) //(var item in ddlMultiDownID.SelectedValue)
            {
                if (item.Selected)
                {
                    MachineIds += item.Value + ",";
                }
            }
            try
            {
                if (componentgrd.Rows.Count <= 0)
                {
                    HelperClassGeneric.openWarningToastrModal(this,GetLocalResourceObject("NoDataToDelete").ToString());
                    return;
                }
                DataTable dt = componentgrd.DataSource as DataTable;
                #region
                if (componentgrd.Rows.Count > 0)
                {
                    if (interfaceid != "")
                    {
                        DataBaseAccess.DeleteComponentOperationDetails("DeleteOpnInfo", ddlComponentID.SelectedValue, interfaceid, txtinterfaceid.Text, txtopn_no.Text, ddlMultiDownID.SelectedValue, out isSuccessOrFailure);
                    }
                    else
                    {
                        Response.Redirect("ComponentInformation.aspx", false);
                    }


                }
                if (isSuccessOrFailure == true)
                {
                    ComponentInfogrd(ddlComponentID.SelectedValue);
                    //btnnew_Click(null, EventArgs.Empty);
                }

                #endregion
            }
            catch (Exception ex)
            {
                HelperClassGeneric.openErrorModal(this, ex.Message);
                Logger.WriteErrorLog("component operation delete " + ex.Message);
            }

        }
        #endregion

        protected void componentgrd_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtrslt = (DataTable)Session["dirState"];
            if (dtrslt.Rows.Count > 0)
            {
                if (Convert.ToString(Session["sortdr"]) == "Asc")
                {
                    dtrslt.DefaultView.Sort = e.SortExpression + " Desc";
                    Session["SortByValue"] = e.SortExpression;
                    Session["sortdr"] = "Desc";
                }
                else
                {
                    dtrslt.DefaultView.Sort = e.SortExpression + " Asc";
                    Session["SortByValue"] = e.SortExpression;
                    Session["sortdr"] = "Asc";
                }
                componentgrd.DataSource = dtrslt;
                componentgrd.DataBind();
            }
        }

        protected void txtloadunload_TextChanged(object sender, EventArgs e)
        {
            double val = 0;
            if (double.TryParse(txtloadunload.Text, out val))
            {
                txtCycleTime.Text = (Convert.ToDouble(txtmachinetime.Text) + val).ToString();
            }
        }

        protected void txtmachinetime_TextChanged(object sender, EventArgs e)
        {
            double val = 0;
            if (double.TryParse(txtmachinetime.Text, out val))
            {
                txtCycleTime.Text = (Convert.ToDouble(txtloadunload.Text) + val).ToString();
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static COPSTDTimeData GetUpdateStdTimes(string machineId, string component, string operation)
        {
            COPSTDTimeData data = new COPSTDTimeData();
            try
            {
                string TimeFormatVal = (VDGDataBaseAccess.GetShopdefaultsTimeFormat()).ToLower();
                string Machiningtime = string.Empty;
                string CycleTime = string.Empty;
                Double standardCycleTime = 0;
                Double standardLoad = 0;
                if (!string.IsNullOrEmpty(TimeFormatVal))
                {
                    if (TimeFormatVal == ("ss"))
                    {
                        data.TimeFormat = "Standard Time (in seconds)";
                        VDGDataBaseAccess.GetAllStdTimes(machineId, component, operation, out CycleTime, out Machiningtime);
                        standardLoad = Math.Round((Convert.ToDouble(CycleTime) - Convert.ToDouble(Machiningtime)), 3);
                        standardCycleTime = Math.Round(Convert.ToDouble(Machiningtime), 3);
                        data.LoadUnLoad = standardLoad.ToString();
                        data.CycleTime = standardCycleTime.ToString();
                    }
                    else
                    {
                        data.TimeFormat = "Standard Time (in minutes)";
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
                        data.LoadUnLoad = standardLoad.ToString();
                        data.CycleTime = standardCycleTime.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return data;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string updateStandardTimeData(string machineId, string component, string operation, string stdCycleTime, string stdLoadTime)
        {
            double cycle = 0;
            double loadUnload = 0, stdloadunload = 0;
            string IsSuccessfull = string.Empty;
            string TimeFormatVal = (VDGDataBaseAccess.GetShopdefaultsTimeFormat()).ToLower();
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
            VDGDataBaseAccess.UpdateAllStdTimes(cycle.ToString(), stdloadunload.ToString(), machineId.ToString(), component, operation, false, out IsSuccessfull);
            return IsSuccessfull;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string getTimeFormat()
        {
            string timeFormat = "ss";
            try
            {
                if (HttpContext.Current.Session["COPTimeFormat"] == null)
                {
                    HttpContext.Current.Session["COPTimeFormat"] = (VDGDataBaseAccess.GetShopdefaultsTimeFormat()).ToLower();

                }
                timeFormat = HttpContext.Current.Session["COPTimeFormat"].ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getTimeFormat =" + ex.Message);
            }
            return timeFormat;
        }

        protected void btnStdTimeUpdateConfirm_Click(object sender, EventArgs e)
        {

            ComponentInfogrd(ddlComponentID.SelectedValue);
        }

        protected void ddlMultiCellID_SelectedIndexChanged(object sender, EventArgs e)
        {

            BindMachinesForPlantCell();
        }

        private string FindInvalidCharacter(string param)
        {
            string regSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex rg = new Regex(string.Format("[{0}]", Regex.Escape(regSearch)));
            string foldernname = rg.Replace(param, "");
            return foldernname;
        }

        private void CreateFolder(string FolderName, string path)
        {
            try
            {
                string FolderPath = Path.Combine(path, FolderName);
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }
                string OpDesc = FindInvalidCharacter(txtdescription.Text);
                string OpPath = Path.Combine(FolderPath, txtopn_no.Text + "_" + OpDesc);
                if (!Directory.Exists(OpPath))
                {
                    Directory.CreateDirectory(OpPath);
                    if (FolderName.Equals("Program", StringComparison.OrdinalIgnoreCase))
                    {
                        string StdProgPath = Path.Combine(OpPath, "Standard Software Program");
                        if (!Directory.Exists(StdProgPath))
                        {
                            Directory.CreateDirectory(StdProgPath);
                        }
                        string MachineProgPath = Path.Combine(OpPath, "Proven Machine Program");
                        if (!Directory.Exists(MachineProgPath))
                        {
                            Directory.CreateDirectory(MachineProgPath);
                        }
                    }
                    ViewState["Status"] = 1;
                }
                else
                {
                    ViewState["Status"] = 0;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnCreateFolder_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["Status"] = null;
                string userame = System.Web.Configuration.WebConfigurationManager.AppSettings["KTAUserName"].ToString();
                string password = System.Web.Configuration.WebConfigurationManager.AppSettings["KTAPassword"].ToString();
                string RootPath = DBAccess.GetRootPath("RootPath");

                if (!string.IsNullOrEmpty(RootPath))
                {
                    if (!string.IsNullOrEmpty(userame) && !string.IsNullOrEmpty(password))
                    {
                        try
                        {
                            nc = new NetworkConnection(RootPath, new NetworkCredential(userame, password));
                        }
                        catch (Exception ex)
                        {
                            if (nc != null)
                                nc.Dispose();
                            Logger.WriteErrorLog(ex.ToString());
                            HelperClassGeneric.openErrorModal(this, ex.ToString());
                        }
                        finally
                        {
                            if (nc != null)
                                nc.Dispose();
                        }
                    }

                    string Comp = FindInvalidCharacter(ddlComponentID.SelectedValue);

                    //string Comp = txtsearch.Text.Replace("/", "~");
                    string path = Path.Combine(RootPath, Comp);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    CreateFolder("Drawing", path);
                    CreateFolder("Program", path);
                    CreateFolder("Tools", path);
                    CreateFolder("Fixture", path);
                    CreateFolder("Gauge", path);
                    CreateFolder("Inspection", path);
                    if (ViewState["Status"].ToString().Equals("1"))
                    {
                        HelperClassGeneric.openSuccessModal(this, "Folder Created!!");
                    }
                    else
                    {
                        HelperClassGeneric.openWarningToastrModal(this, "Folder Already Exists");
                    }
                }
                else
                    HelperClassGeneric.openWarningToastrModal(this, "Please Add Root Folder!!");
            }
            catch (Exception ex)
            {
                if (nc != null)
                    nc.Dispose();
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (nc != null)
                    nc.Dispose();
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                ComponentInfogrd(ddlComponentID.SelectedValue);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellMachine();
        }
    }
    public class COPSTDTimeData
    {
        public string LoadUnLoad { get; set; }
        public string CycleTime { get; set; }
        public string TimeFormat { get; set; }
        public List<string> MachineId { get; set; }
    }
}