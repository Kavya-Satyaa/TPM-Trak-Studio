using BusinessClassLibrary;
using Elmah;
using ModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class CreateSchedule_Old : System.Web.UI.Page
    {
        static List<DTOShcheduleReport> list = null;
        string SlNo = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                //SessionClear.ClearSession();
                //list = TMPTrakDataBase.GetDataForcmbReports();
                //BindReportType();
                //BindAllFields();
                //GetExportType();
                //BindPlantId();
                //BindOperatorInfo();
                //BindShiftData();
                //BindDdlRptOn();
                //BindDdllstReportOnEvery();
                //BindDataGrid();
                //ddlEmailMethod_SelectedIndexChanged(null, null);
                Binddata();
            }
        }

        public void Binddata()
        {
            list = TMPTrakDataBase.GetDataForcmbReports();
            BindReportType();
            BindAllFields();
            GetExportType();
            BindPlantId();
            BindLineIds();
            BindMachines();
            BindOperatorInfo();
            BindShiftData();
            BindDdlRptOn();
            BindDdllstReportOnEvery();
            BindDataGrid();
            ddlEmailMethod_SelectedIndexChanged(null, null);
        }

        private void BindReportType()
        {
            try
            {
                if (list == null) return;
                ddlReports.DataSource = list;
                ddlReports.DataValueField = "Reports";
                ddlReports.DataTextField = "Reports";
                ddlReports.DataBind();
                if (ddlReports.SelectedValue != null)
                {
                    var vv = list.Where(item => item.Reports == ddlReports.SelectedValue.ToString()).FirstOrDefault();
                    HidtemplateName.Value = vv.ReportTemplate;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void GetExportType()
        {
            try
            {
                if (ddlReports.SelectedValue == null || list == null) return;
                var exportType = list.Where(item => item.Reports == ddlReports.SelectedValue.ToString()).FirstOrDefault();
                string exportList = exportType.ExportType;
                List<string> TagIds = exportList.Split(',').ToList();
                ddlExportTypes.DataSource = TagIds;
                ddlExportTypes.DataBind();
                ddlPlantId.Enabled = exportType.PlantIDAll;
                ddlMachineId.Enabled = exportType.MachineAll;
                ddlOperator.Enabled = exportType.OperatorAll;
                ddlShift.Enabled = exportType.ShiftIdAll;
                ddlRptOn.Enabled = exportType.RunReportOnVisibity;
                ddlRptForEvery.Enabled = exportType.RunReportforEveryVisibility;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        #region "Bind Plant Id"
        private void BindPlantId()
        {
            try
            {
                List<string> lstPlantData = BindCockpitView.ViewPlantToDisplay();
                ddlPlantId.DataSource = lstPlantData;
                ddlPlantId.DataBind();
                ddlPlantId.Items.Insert(0, "ALL");
                ddlPlantId_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Machine Id"
        private void BindMachines()
        {
            try
            {
                //ddlMachineId.Items.Clear();// = null;
                var allMachineName = VDGDataBaseAccess.GetAllMachines(ddlPlantId.SelectedValue.ToString(), ddlLineID.SelectedValue.ToString());
                allMachineName.Insert(0, "ALL");
                if (allMachineName != null && allMachineName.Count > 0)
                {
                    ddlMachineId.DataSource = allMachineName;
                    ddlMachineId.DataBind();
                }
            }
            catch (Exception ex)
            {   
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Line Id"
        private void BindLineIds()
        {
            try
            {
                ddlLineID.Items.Clear();// = null;
                var allLineIds = VDGDataBaseAccess.GetAllLines(ddlPlantId.SelectedValue.ToString());
                allLineIds.Insert(0, "ALL");
                if (allLineIds != null && allLineIds.Count > 0)
                {
                    ddlLineID.DataSource = allLineIds;
                    ddlLineID.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Shift Data------------------------"
        private void BindShiftData()
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    var listShift = list.Select(item => item.ShiftId).Distinct().ToList();
                    if (listShift.Count > 0)
                    {
                        string shift = listShift[0].ToString();
                        List<string> TagIds = shift.Split(',').ToList();
                        TagIds.Insert(0, "All");
                        ddlShift.DataSource = TagIds;
                        ddlShift.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        #endregion

        #region "Bind Operator Info"
        private void BindOperatorInfo()
        {
            try
            {
                List<EmployeeInfo> lstData = BindCockpitView.ViewEmployeeInfo();
                lstData.Insert(0, new EmployeeInfo { EmpID = "All", EmpName = "All" });
                #region "Binding Operator Dropdown"
                ddlOperator.DataSource = lstData;
                ddlOperator.DataValueField = "EmpID";
                ddlOperator.DataTextField = "EmpName";
                ddlOperator.DataBind();
                #endregion
                #region "Binding Email IDs"
                ddlEmailIds.DataSource = lstData.Where(x => x.EmpEmailId != null && !string.IsNullOrEmpty(x.EmpEmailId)).ToList();
                ddlEmailIds.DataValueField = "EmpEmailId";
                ddlEmailIds.DataTextField = "EmpEmailId";
                ddlEmailIds.DataBind();
                ddlEmailIds.SelectedIndex = 0;
                #endregion
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        private void BindDdlRptOn()
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    var listRptOn = list.Select(item => item.RunreportOn).Distinct().ToList();
                    if (listRptOn.Count > 0)
                    {
                        string ReportOn = listRptOn[0].ToString();
                        List<string> listReport = ReportOn.Split(',').ToList();
                        ddlRptOn.DataSource = listReport;
                        ddlRptOn.DataBind();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void BindDdllstReportOnEvery()
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    var lstReportOnEvery = list.Select(item => item.RunReportOnEvery).Distinct().ToList();
                    if (lstReportOnEvery.Count > 0)
                    {
                        string ReportOnEvery = lstReportOnEvery[0].ToString();
                        List<string> listReportonEvery = ReportOnEvery.Split(',').ToList();
                        ddlRptForEvery.DataSource = listReportonEvery;
                        ddlRptForEvery.DataBind();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void BindAllFields()
        {
            string server = string.Empty; string port = string.Empty; string EmailId = string.Empty; string password = string.Empty;
            ddlEmailMethod.Text = TMPTrakDataBase.GetEmailCredentials(out server, out port, out EmailId, out password);
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtServer.Text = server;
            txtPort.Text = port;
            txtFromEmailId.Text = EmailId;
            txtPassword.Attributes["value"] = password;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool InsertDataToDataBase(string date, string emailMethod, string server, string port, string fromEmailID, string password)
        {
            bool suc = false;
            try
            {
                TMPTrakDataBase.Inserttomailserver(emailMethod, server,port);
                TMPTrakDataBase.Inserttomail(fromEmailID, password);
                TMPTrakDataBase.Inserttomail(date);
                //TMPTrakDataBase.InsertEmailSettings(emailMethod, date, server, port, fromEmailID, password);
                suc = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return suc;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool InsertScheduleToDb(string exportType, string TemplateName, string machineId, string OperatorId, string PlantId, string LineId, string ReportForEveryDay, string Reportid, string Reports, string ShiftId, string ExportPath, bool ChkEmail, string lstAddCC, string lstAddTo, string lstAddBCC, string slno, string ExportName)
        {
            bool isSucc = false;
            try
            {
                int rptID = 0;
                if (Reportid.Equals("TodaysData"))
                    rptID = 0;
                if (Reportid.Equals("One Day Before Data"))
                    rptID = 1;
                if (Reportid.Equals("Two Day Before Data"))
                    rptID = 2;
                if (Reportid.Equals("Three Day Before Data"))
                    rptID = 3;
                ShiftId = ShiftId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ShiftId;
                lstAddTo = lstAddTo.Replace(",", ";");
                lstAddTo = lstAddTo.TrimEnd(';');
                lstAddCC = lstAddCC.Replace(",", ";");
                lstAddCC = lstAddCC.TrimEnd(';');
                if (!Directory.Exists(ExportPath))
                {
                    return false;
                }
                TMPTrakDataBase.InsertOrUpdateScheduleData(ExportName, TemplateName, exportType, machineId.Equals("ALL", StringComparison.OrdinalIgnoreCase) ? string.Empty : machineId, OperatorId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : OperatorId, PlantId.Equals("ALL", StringComparison.OrdinalIgnoreCase) ? string.Empty : PlantId, LineId.Equals("ALL", StringComparison.OrdinalIgnoreCase) ? string.Empty : LineId, ReportForEveryDay, "", rptID, Reports, ShiftId, ExportPath, ChkEmail, lstAddCC, lstAddBCC, lstAddTo, slno, out isSucc);
                if (isSucc)
                {
                    //CreateSchedule cs = new CreateSchedule();
                    //cs.gridScheduleRpt.DataSource = TMPTrakDataBase.AllDataOfScheduleReport();
                    //cs.gridScheduleRpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return isSucc;
        }

        protected void ddlReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlReports.SelectedValue == null || list == null) return;
                var vv = list.Where(item => item.Reports == ddlReports.SelectedValue.ToString()).FirstOrDefault();
                HidtemplateName.Value = vv.ReportTemplate;
                if (ddlExportTypes.Items.Contains(new ListItem(vv.ExportType)))
                    ddlExportTypes.Text = vv.ExportType;
                //cmbMachineId.Text = vv.Machine;
                if (ddlOperator.Items.Contains(new ListItem(vv.Operator)))
                    ddlOperator.Text = vv.Operator;
                if (ddlPlantId.Items.Contains(new ListItem(vv.PlantID)))
                    ddlPlantId.Text = vv.PlantID;
                if (ddlRptForEvery.Items.Contains(new ListItem(vv.RunReportOnEvery)))
                    ddlRptForEvery.Text = vv.RunReportOnEvery;
                if (ddlRptOn.Items.Contains(new ListItem(vv.RunreportOn)))
                    ddlRptOn.Text = vv.RunreportOn;
                ddlPlantId.Enabled = vv.PlantIDAll;
                ddlMachineId.Enabled = vv.MachineAll; ;
                ddlOperator.Enabled = vv.OperatorAll;
                ddlShift.Enabled = vv.ShiftIdAll;
                ddlLineID.Enabled = vv.GroupIDAll;
                ddlRptOn.Enabled = vv.RunReportOnVisibity;
                ddlRptForEvery.Enabled = vv.RunReportforEveryVisibility;
                BindDataGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindLineIds();

            if (ckhEnableEmail.Checked)
            {
                addTo.Attributes.Add("disabled", "enable");
                removeTo.Attributes.Add("disabled", "enable");
                addCc.Attributes.Add("disabled", "enable");
                removeCc.Attributes.Add("disabled", "enable");
            }
            else
            {
                addTo.Attributes.Add("disabled", "disabled");
                removeTo.Attributes.Add("disabled", "disabled");
                addCc.Attributes.Add("disabled", "disabled");
                removeCc.Attributes.Add("disabled", "disabled");
            }
        }



        private void BindDataGrid()
        {
            try
            {
                //gridScheduleRpt.AutoGenerateColumns = false;
                DataTable dt = TMPTrakDataBase.AllDataOfScheduleReport();
                if (dt.Rows.Count <= 0)
                {
                    dt.Columns.Add("ReportFileName");
                    dt.Columns.Add("ExportType");
                    dt.Columns.Add("ExportFileName");
                    dt.Rows.Add();
                }
                gridScheduleRpt.DataSource = dt;
                gridScheduleRpt.DataBind();
            }
            catch (Exception)
            {
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                bool isSucc = false;
                int Reportid = 0;
                if (ddlRptOn.SelectedValue.ToString() == "Today's Data")
                    Reportid = 0;
                if (ddlRptOn.SelectedValue.ToString() == "One Day Before Data")
                    Reportid = 1;
                if (ddlRptOn.SelectedValue.ToString() == "Two Day Before Data")
                    Reportid = 2;
                if (ddlRptOn.SelectedValue.ToString() == "Three Day Before Data")
                    Reportid = 3;

                StringBuilder AddTo = new StringBuilder();
                StringBuilder AddCC = new StringBuilder();
                StringBuilder AddBCC = new StringBuilder();
                if (ckhEnableEmail.Checked)
                {
                    if (lbAddTo.Items.Count <= 0)
                    {
                        return;
                    }
                    foreach (var item in lbAddTo.Items)
                    {
                        AddTo.Append(item);
                        AddTo.Append(",");
                    }
                    foreach (var item in lbAddCc.Items)
                    {
                        AddCC.Append(item);
                        AddCC.Append(",");
                    }
                    //foreach (var item in lbAddBCc.Items)
                    //{
                    //    AddBCC.Append(item);
                    //    AddBCC.Append(",");
                    //}
                }
                if (!Directory.Exists(txtExportPath.Text))
                {
                    lblMessages.Text = "Please add Correct Path";
                    return;
                }
                TMPTrakDataBase.InsertOrUpdateScheduleData(txtExportName.Text, HidtemplateName.Value, ddlExportTypes.Text, ddlMachineId.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) ? string.Empty : ddlMachineId.Text, ddlOperator.Text.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlOperator.Text, ddlPlantId.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) ? string.Empty : ddlPlantId.Text, ddlLineID.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) ? string.Empty : ddlLineID.Text, ddlRptForEvery.Text,"",Reportid, ddlReports.Text, ddlShift.Text, txtExportPath.Text, ckhEnableEmail.Checked, AddCC.ToString(), AddBCC.ToString(), AddTo.ToString(), txtSlNo.Text.Trim(), out isSucc);

                if (isSucc)
                    BindDataGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string[] files = Directory.GetFiles(fbd.SelectedPath);

                    //System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
                }
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetScheduleData()
        {
            DataSet ds = new DataSet();
            try
            {
                DataTable dt = TMPTrakDataBase.AllDataOfScheduleReport();
                dt.TableName = "Schedules";
                ds.Tables.Add(dt);
            }
            catch (Exception)
            {
            }
            return ds.GetXml();
        }

        protected void ddlEmailMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedValue = ddlEmailMethod.SelectedItem.ToString();
            var v1 = TMPTrakDataBase.GetEmailForEmailSettings();
            if (v1 != null)
            {
                txtFromEmailId.Text = v1.ToString();
            }
            else { txtFromEmailId.Text = string.Empty; }

            var v2 = TMPTrakDataBase.GetPortForEmailSettings(selectedValue);
            if (v2 != null)
            {
                if (txtPort == null) { return; }
                txtPort.Text = v2.ToString();
            }
            else { txtFromEmailId.Text = string.Empty; }

            var v3 = TMPTrakDataBase.GetServerForEmailSettings(selectedValue);
            if (v3 != null)
            {
                if (txtServer == null) { return; }
                txtServer.Text = v3.ToString();
            }
            else { txtServer.Text = string.Empty; }

            var v4 = TMPTrakDataBase.GetPasswordForEmailSettings();
            if (v4 != null)
            {
                if (txtPassword == null) { return; }
                txtPassword.Text = v4.ToString();
            }
            else { txtPassword.Text = string.Empty; }

            var v5 = TMPTrakDataBase.getReleasingDate();
            if (v5 != null)
            {
                { if (txtDate == null) return; }
                txtDate.Text = v5.ToString();
            }
            else
            {
                txtDate.Text = string.Empty;
            }
        }

        protected void btndelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtExportName.Text.Trim()) && !string.IsNullOrEmpty(ddlReports.SelectedValue.ToString()))
            {
                TMPTrakDataBase.DeleteCreateSchedule(ddlReports.SelectedValue.ToString(), txtExportName.Text.Trim(), ddlPlantId.Text, ddlOperator.Text, ddlMachineId.Text, ddlLineID.Text, ddlShift.Text, ddlRptForEvery.Text);
                lblMessages.Text = "Delete Successful";
                ResetAllOptions();
                //Binddata(); 
            }
            else
                lblMessages.Text = " Cannot be deleted because Export Name or Report Name is empty";
            //Binddata();
            BindDataGrid();
            if (ckhEnableEmail.Checked)
            {
                addTo.Attributes.Remove("disabled");
                removeTo.Attributes.Remove("disabled");
                addCc.Attributes.Remove("disabled");
                removeCc.Attributes.Remove("disabled");
            }
            else
            {
                addTo.Attributes.Add("disabled", "disabled");
                removeTo.Attributes.Add("disabled", "disabled");
                addCc.Attributes.Add("disabled", "disabled");
                removeCc.Attributes.Add("disabled", "disabled");
            }
        }

        private void ResetAllOptions()
        {
            ddlReports.SelectedIndex = 0;
            ddlExportTypes.SelectedIndex = 0;
            txtExportName.Text = string.Empty;
            txtExportPath.Text = string.Empty;
            ddlPlantId.SelectedIndex = 0;
            ddlLineID.SelectedIndex = 0;
            ddlMachineId.SelectedIndex = 0;
            ddlOperator.SelectedIndex = 0;
            ddlShift.SelectedIndex = 0;
            ddlRptOn.SelectedIndex = 0;
            ddlRptForEvery.SelectedIndex = 0;
        }

        protected void ddlLineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachines();
            if (ckhEnableEmail.Checked)
            {
                addTo.Attributes.Add("disabled", "enable");
                removeTo.Attributes.Add("disabled", "enable");
                addCc.Attributes.Add("disabled", "enable");
                removeCc.Attributes.Add("disabled", "enable");
            }
            else
            {
                addTo.Attributes.Add("disabled", "disabled");
                removeTo.Attributes.Add("disabled", "disabled");
                addCc.Attributes.Add("disabled", "disabled");
                removeCc.Attributes.Add("disabled", "disabled");
            }
        }
    }
}