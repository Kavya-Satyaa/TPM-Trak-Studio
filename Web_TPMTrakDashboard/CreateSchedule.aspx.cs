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
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;


namespace Web_TPMTrakDashboard
{
    public partial class CreateSchedule : System.Web.UI.Page
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
                Binddata();
                menu1.Style.Add("display", "none");
                btnEmailSetting.Attributes.Add("class", "btn btn-default btn-md");
                btnCreateSchedule.Attributes.Add("class", "btn btn-info btn-md");

            }

        }

        public void Binddata()
        {
            list = TMPTrakDataBase.GetDataForcmbReports();
            BindReportType();
            BindAllFields();
            GetExportType();
            BindPlantId();
            BindGroupId();
            BindMachines();
            BindOperatorInfo();
            BindShiftData();
            BindDdlRptOn();
            BindDdllstReportOnEvery();
            BindDataGrid();
            ddlEmailMethod_SelectedIndexChanged(null, null);
        }

        #region "Bind ReportName"
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
                    lblTemplate.Text = vv.ReportTemplate;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        #endregion

        #region "Bind ExportType"
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
                ddlGroupId.Enabled = exportType.GroupIDAll;
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
        #endregion

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

        #region " Bind Group Id"
        private void BindGroupId()
        {
            try
            {
                ddlGroupId.Items.Clear();
                var allLineIds = BindCockpitView.GetAllLines(ddlPlantId.SelectedItem.Text.ToString());
                allLineIds.Insert(0, "ALL");
                if (allLineIds != null && allLineIds.Count > 0)
                {
                    ddlGroupId.DataSource = allLineIds;
                    ddlGroupId.DataBind();
                }
                //List<string> lstGroupData = BindCockpitView.ViewGroupToDisplay();
                //ddlGroupId.DataSource = lstGroupData;
                //ddlGroupId.DataBind();
                //ddlGroupId.Items.Insert(0, "ALL");
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
                ddlMachineId.Items.Clear();// = null;
                if (ddlReports.SelectedValue.ToString().Equals("FlowMeterReport", StringComparison.OrdinalIgnoreCase))
                {
                    var FlowMeterMachLst = VDGDataBaseAccess.GetMachineIDsLst(ddlPlantId.SelectedValue.ToString());
                    FlowMeterMachLst.Insert(0, "ALL");
                    if (FlowMeterMachLst!=null && FlowMeterMachLst.Count>0)
                    {
                        ddlMachineId.DataSource = FlowMeterMachLst;
                        ddlMachineId.DataBind();
                    }
                }
                else
                {
                    var allMachineName = VDGDataBaseAccess.GetAllMachines(ddlPlantId.SelectedItem.Text.ToString(), ddlGroupId.SelectedItem.Text.ToString());
                    allMachineName.Insert(0, "ALL");
                    if (allMachineName != null && allMachineName.Count > 0)
                    {
                        ddlMachineId.DataSource = allMachineName;
                        ddlMachineId.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Shift Data"
        private void BindShiftData()
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    var listShift = DataBaseAccess.GetAllShifts("");
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
                //ddlEmailIds.SelectedIndex = 0;
                #endregion
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Run Report"
        private void BindDdlRptOn()
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    var listRptOnReports = list.Where(item => item.Reports == ddlReports.SelectedValue.ToString()).ToList();
                    var listRptOn = listRptOnReports.Select(item => item.RunreportOn).Distinct().ToList();
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
        #endregion

        #region "Bind Run Report for every day"
        private void BindDdllstReportOnEvery()
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    var listReportOnEveryReports = list.Where(item => item.Reports == ddlReports.SelectedValue.ToString()).ToList();
                    var lstReportOnEvery = listReportOnEveryReports.Select(item => item.RunReportOnEvery).Distinct().ToList();
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
        #endregion

        #region"Bind Email Settings"
        private void BindAllFields()
        {
            string server = string.Empty; string port = string.Empty; string EmailId = string.Empty; string password = string.Empty;
            ddlEmailMethod.Text = TMPTrakDataBase.GetEmailCredentials(out server, out port, out EmailId, out password);
            txtDate.Text = Convert.ToDateTime(TMPTrakDataBase.getReleasingDate()).ToString("yyyy-MM-dd");
            txtServer.Text = server;
            txtPort.Text = port;
            txtFromEmailId.Text = EmailId;
            txtPassword.Attributes["value"] = password;
        }
        #endregion

        #region "Bind Gridview values"
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
                foreach(DataRow item in dt.Rows)
                {
                    item["Machine"] = item["Machine"].ToString().Equals("") ? "ALL" : item["Machine"];
                    item["Shift"] = item["Shift"].ToString().Equals("") ? "All" : item["Shift"];
                    item["PlantID"] = item["PlantID"].ToString().Equals("") ? "ALL" : item["PlantID"];
                    item["GroupID"] = item["GroupID"].ToString().Equals("") ? "ALL" : item["GroupID"];
                    item["Operator"] = item["Operator"].ToString().Equals("") ? "All" : item["Operator"];
                }
                gridScheduleRpt.DataSource = dt;
                gridScheduleRpt.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        #endregion

        private void EmailCheck()
        {
            //if (ckhEnableEmail.Checked)
            //{
            //    addTo.Attributes.Add("disabled", "enable");
            //    removeTo.Attributes.Add("disabled", "enable");
            //    addCc.Attributes.Add("disabled", "enable");
            //    removeCc.Attributes.Add("disabled", "enable");
            //}
            //else
            //{
            //    addTo.Attributes.Add("disabled", "disabled");
            //    removeTo.Attributes.Add("disabled", "disabled");
            //    addCc.Attributes.Add("disabled", "disabled");
            //    removeCc.Attributes.Add("disabled", "disabled");
            //}

        }
        protected void ddlReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlReports.SelectedValue == null || list == null) return;
                var vv = list.Where(item => item.Reports == ddlReports.SelectedValue.ToString()).FirstOrDefault();
                HidtemplateName.Value = vv.ReportTemplate;
                lblTemplate.Text = vv.ReportTemplate;
                string exportList = vv.ExportType;
                List<string> TagIds = exportList.Split(',').ToList();
                ddlExportTypes.DataSource = TagIds;
                ddlExportTypes.DataBind();
                if (ddlExportTypes.Items.Contains(new ListItem(vv.ExportType)))
                    ddlExportTypes.Text = vv.ExportType;
                //cmbMachineId.Text = vv.Machine;
                if (ddlOperator.Items.Contains(new ListItem(vv.Operator)))
                    ddlOperator.Text = vv.Operator;
                if (ddlPlantId.Items.Contains(new ListItem(vv.PlantID)))
                    ddlPlantId.Text = vv.PlantID;
                if (ddlGroupId.Items.Contains(new ListItem(vv.GroupID)))
                    ddlGroupId.Text = vv.GroupID;
                if (ddlRptForEvery.Items.Contains(new ListItem(vv.RunReportOnEvery)))
                    ddlRptForEvery.Text = vv.RunReportOnEvery;
                if (ddlRptOn.Items.Contains(new ListItem(vv.RunreportOn)))
                    ddlRptOn.Text = vv.RunreportOn;
                txtSubject.Text = vv.Subject;
                EmailCheck();

                //if (ckhEnableEmail.Checked)
                //{
                //    addTo.Attributes.Remove("disabled");
                //    removeTo.Attributes.Remove("disabled");
                //    addCc.Attributes.Remove("disabled");
                //    removeCc.Attributes.Remove("disabled");
                //}
                //else
                //{
                //    addTo.Attributes.Add("disabled", "disabled");
                //    removeTo.Attributes.Add("disabled", "disabled");
                //    addCc.Attributes.Add("disabled", "disabled");
                //    removeCc.Attributes.Add("disabled", "disabled");
                //}
                BindDdlRptOn();
                BindDdllstReportOnEvery();
                BindMachines();
                ddlPlantId.Enabled = vv.PlantIDAll;
                ddlGroupId.Enabled = vv.GroupIDAll;
                ddlMachineId.Enabled = vv.MachineAll;
                ddlOperator.Enabled = vv.OperatorAll;
                ddlShift.Enabled = vv.ShiftIdAll;
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
            BindGroupId();
            BindMachines();
            EmailCheck();
        }

        protected void ddlGroupId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachines();
            EmailCheck();
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
            if (v2 != null && v2 != "")
            {
                txtPort.Visible = true;
                lblPort.Visible = true;
                txtPort.Text = v2.ToString();
            }
            else
            {
                txtPort.Visible = false;
                lblPort.Visible = false;
            }

            var v3 = TMPTrakDataBase.GetServerForEmailSettings(selectedValue);

            if (ddlEmailMethod.SelectedItem.Text == "SMTP")
            {
                if (txtServer == null) { return; }
                lblServer.Text = "Server";
                txtServer.Text = v3.ToString();
            }
            else
            {
                lblServer.Text = "EWS Url";
                txtServer.Text = v3.ToString();
            }

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
            try
            {
                if (!string.IsNullOrEmpty(txtExportName.Text.Trim()) && !string.IsNullOrEmpty(ddlReports.SelectedValue.ToString()))
                {
                    TMPTrakDataBase.DeleteCreateSchedule(ddlReports.SelectedItem.Text, txtExportName.Text.Trim(), ddlPlantId.SelectedItem.Text, ddlGroupId.SelectedItem.Text, ddlOperator.SelectedItem.Text, ddlMachineId.SelectedItem.Text, ddlShift.SelectedItem.Text, ddlRptForEvery.SelectedItem.Text);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "alert('Delete Successful...')", true);
                    lblMessages.Text = "Delete Successful";
                    Binddata();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Success" + 1, "<script>showpop('Cannot be deleted because Export Name or Report Name is empty.')</script>", false);
                    lblMessages.Text = " Cannot be deleted because Export Name or Report Name is empty";
                }
                Binddata();
                txtExportName.Text = "";
                txtExportPath.Text = "";
                ckhEnableEmail.Checked = false;
                lbAddTo.Items.Clear();
                lbAddCc.Items.Clear();
                EmailCheck();
                //if (ckhEnableEmail.Checked)
                //{
                //    addTo.Attributes.Remove("disabled");
                //    removeTo.Attributes.Remove("disabled");
                //    addCc.Attributes.Remove("disabled");
                //    removeCc.Attributes.Remove("disabled");
                //}
                //else
                //{
                //    addTo.Attributes.Add("disabled", "disabled");
                //    removeTo.Attributes.Add("disabled", "disabled");
                //    removeTo.Attributes.Add("disabled", "disabled");
                //    addCc.Attributes.Add("disabled", "disabled");
                //    removeCc.Attributes.Add("disabled", "disabled");
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

        }

        protected void gridScheduleRpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gridScheduleRpt.SelectedRow;

            ddlReports.SelectedValue = row.Cells[1].Text;
            #region " assign values"
            ddlReports_SelectedIndexChanged(null, null);
            if (row.Cells[2].Text != "&nbsp;" && row.Cells[2].Text != "")
            {
                if (row.Cells[2].Text.Equals("Excel"))
                {
                    ddlExportTypes.SelectedValue = "excel";
                }
                else
                {
                    ddlExportTypes.SelectedValue = "pdf";
                }

            }
            else
            {
                ddlExportTypes.SelectedValue = "";
            }
            //if (row.Cells[2].Text == "0")
            //{
            //    ddlExportTypes.SelectedIndex = 1;
            //}
            //else
            //{
            //    ddlExportTypes.SelectedIndex = 0;
            //}
            foreach(ListItem item in ddlEmailIds.Items)
            {
                item.Selected = false;
            }

            txtExportName.Text = row.Cells[3].Text;
            txtExportPath.Text = row.Cells[4].Text;
            ddlReports_SelectedIndexChanged(null, null);
            if (row.Cells[8].Text != "&nbsp;" && row.Cells[8].Text != "")
            {
                ddlPlantId.SelectedValue = row.Cells[8].Text;
            }
            else
            {
                ddlPlantId.SelectedValue = "ALL";
            }
            if (row.Cells[5].Text != "&nbsp;" && row.Cells[5].Text != "")
            {
                ddlMachineId.SelectedValue = row.Cells[5].Text;
            }
            else
            {
                ddlMachineId.SelectedValue = "ALL";
            }
            if (row.Cells[6].Text != "&nbsp;" && row.Cells[6].Text != "")
            {
                ddlShift.SelectedValue = row.Cells[6].Text;
            }
            else
            {
                ddlShift.SelectedValue = "All";
            }
            if (row.Cells[7].Text != "&nbsp;" && row.Cells[7].Text != "")
            {
                ddlOperator.SelectedValue = row.Cells[7].Text;
            }
            else
            {
                ddlOperator.SelectedValue = "All";
            }

            if (row.Cells[9].Text != "&nbsp;" && row.Cells[9].Text != "")
            {
                ddlGroupId.SelectedValue = row.Cells[9].Text;
            }
            else
            {
                ddlGroupId.SelectedValue = "ALL";
            }
            //ddlRptOn.SelectedItem.Text = row.Cells[]

            if (row.Cells[14].Text != "&nbsp;" && row.Cells[14].Text != "")
            {
                ddlRptForEvery.SelectedValue = row.Cells[14].Text;
            }
            else
            {
                ddlRptForEvery.SelectedValue = "";
            }

            if (row.Cells[10].Text == "True")
            {
                ckhEnableEmail.Checked = true;
                EmailCheck();
                //if (ckhEnableEmail.Checked)
                //{
                //    addTo.Attributes.Remove("disabled");
                //    removeTo.Attributes.Remove("disabled");
                //    addCc.Attributes.Remove("disabled");
                //    removeCc.Attributes.Remove("disabled");
                //}
                //else
                //{
                //    addTo.Attributes.Add("disabled", "disabled");
                //    removeTo.Attributes.Add("disabled", "disabled");
                //    addCc.Attributes.Add("disabled", "disabled");
                //    removeCc.Attributes.Add("disabled", "disabled");
                //}
                if (row.Cells[11].Text != "&nbsp;" && row.Cells[11].Text != "")
                {
                        List<string> addTo = new List<string>();
                        string[] array = row.Cells[11].Text.Split(';');
                        foreach (string ID in array)
                        {
                            string IDs = ID.Trim() + ";";
                            addTo.Add(IDs);

                            if (ID!= "")
                            {
                                ddlEmailIds.Items.FindByValue(ID.Trim()).Selected = true;
                            }
                        }
                    lbAddTo.DataSource = addTo;
                    lbAddTo.DataBind();
                }
                else
                {
                    lbAddTo.DataSource = "";
                    lbAddTo.DataBind();
                }
                if (row.Cells[12].Text != "&nbsp;" && row.Cells[12].Text != "")
                {
                    List<string> addCc = new List<string>();
                    string[] array = row.Cells[12].Text.Split(';');
                    foreach (string ID in array)
                    {
                        string IDs = ID.Trim() + ";";
                        addCc.Add(IDs);
                        if (ID != "")
                        {
                            ddlEmailIds.Items.FindByValue(ID.Trim()).Selected = true;
                        }
                    }
                    lbAddCc.DataSource = addCc;
                    lbAddCc.DataBind();
                }
                else
                {
                    lbAddCc.DataSource = "";
                    lbAddCc.DataBind();
                }
            }
            else
            {
                ckhEnableEmail.Checked = false;
            }

            if (row.Cells[15].Text == "0")
                ddlRptOn.SelectedIndex = 0;
            if (row.Cells[15].Text == "1")
                ddlRptOn.SelectedIndex = 1;
            if (row.Cells[15].Text == "2")
                ddlRptOn.SelectedIndex = 2;
            if (row.Cells[15].Text == "3")
                ddlRptOn.SelectedIndex = 3;

            txtSubject.Text = row.Cells[17].Text;
            #endregion

            if (row.Cells[16].Text != "&nbsp;" && row.Cells[16].Text != "")
            {
                HidtemplateName.Value = row.Cells[16].Text;
                lblTemplate.Text = row.Cells[16].Text;
            }

        }

        protected void btnCreateSchedule_Click(object sender, EventArgs e)
        {
            menu0.Style.Add("display", "block");
            menu1.Style.Add("display", "none");
            btnEmailSetting.Attributes.Add("class", "btn btn-default btn-md");
            btnCreateSchedule.Attributes.Add("class", "btn btn-info btn-md");

            //background - color:whitesmoke; color: aqua
            //btnEmailSetting.Attributes.Add("style", "background-color:black");
            //btnEmailSetting.Attributes.Add("style", "color:white");

        }

        protected void btnEmailSetting_Click(object sender, EventArgs e)
        {
            menu0.Style.Add("display", "none");
            menu1.Style.Add("display", "block");
            btnEmailSetting.Attributes.Add("class", "btn btn-info btn-md");
            btnCreateSchedule.Attributes.Add("class", "btn btn-default btn-md");
            //btnCreateSchedule.Attributes.Add("style", "background-color:black");
            //btnCreateSchedule.Attributes.Add("style", "color:white");
        }

        protected void btnScheduleSave_Click(object sender, EventArgs e)
        {
            try
            {
                //gridScheduleRpt.Attributes.Remove("selectedRow");
                bool isSucc = false;
                int Reportid = 0;
                if (ddlRptOn.SelectedIndex == 0)
                    Reportid = 0;
                if (ddlRptOn.SelectedIndex == 1)
                    Reportid = 1;
                if (ddlRptOn.SelectedIndex == 2)
                    Reportid = 2;
                if (ddlRptOn.SelectedIndex == 3)
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
                            if (item.ToString().Contains(";"))
                                continue;
                            AddTo.Append(",");
                    }

                    foreach (var item in lbAddCc.Items)
                    {
                        AddCC.Append(item);
                        if (item.ToString().Contains(";"))
                            continue;
                        AddCC.Append(",");
                    }

                }
                AddTo.Replace(",", ";"); AddCC.Replace(",", ";");
                if (!Directory.Exists(txtExportPath.Text))
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Success" + 1, "<script>showpop('Please add Correct Path.')</script>", false);
                    lblMessages.Text = "Please add Correct Path.";
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "Success" + 1, "showpop('Please add Correct Path.','Info')", false);


                    return;
                }
                TMPTrakDataBase.InsertOrUpdateScheduleData(txtExportName.Text, HidtemplateName.Value, ddlExportTypes.SelectedItem.Text, ddlMachineId.SelectedItem.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) ? string.Empty : ddlMachineId.SelectedItem.Text, ddlOperator.SelectedItem.Text.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlOperator.SelectedItem.Text, ddlPlantId.SelectedItem.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) ? string.Empty : ddlPlantId.SelectedItem.Text, ddlGroupId.SelectedItem.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) ? string.Empty : ddlGroupId.SelectedItem.Text, ddlRptForEvery.SelectedItem.Text,txtSubject.Text, Reportid, ddlReports.SelectedItem.Text, ddlShift.SelectedItem.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) ? string.Empty : ddlShift.SelectedItem.Text, txtExportPath.Text, ckhEnableEmail.Checked, AddCC.ToString().TrimEnd(','), AddBCC.ToString(), AddTo.ToString().TrimEnd(','), txtSlNo.Text.Trim(), out isSucc);

                if (isSucc)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Success" + 1, "<script>showpop('Successfully Saved !!')</script>", false);
                    lblMessages.Text = "Successfully Saved !!";
                    Binddata();
                    //BindDataGrid();
                    txtExportName.Text = "";
                    txtExportPath.Text = "";
                    txtSubject.Text = "";
                    ckhEnableEmail.Checked = false;
                    lbAddTo.Items.Clear();
                    lbAddCc.Items.Clear();
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

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void addTo_Click(object sender, EventArgs e)
        {
            var selectedMail = ddlEmailIds;
            List<string> selectedEmail = new List<string>();
            foreach (ListItem item in selectedMail.Items)
            {
                if (item.Selected)
                {
                    selectedEmail.Add(item.Value);
                }
            }
            var abc = selectedEmail;
            lbAddTo.DataSource = selectedEmail;
            lbAddTo.DataBind();

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

        protected void removeTo_Click(object sender, EventArgs e)
        {
            var items = (from ListItem li in lbAddTo.Items where li.Selected == true select li).ToList<ListItem>();
            foreach (ListItem item in items)
            {
                if (item.Selected)
                {
                    lbAddTo.Items.Remove(item);
                }
            }
            //lbAddTo.Items.Clear();
        }

        protected void addCc_Click(object sender, EventArgs e)
        {
            var selectedMail = ddlEmailIds;
            List<string> selectedEmail = new List<string>();
            foreach (ListItem item in selectedMail.Items)
            {
                if (item.Selected)
                {
                    selectedEmail.Add(item.Value);
                }
            }
            var abc = selectedEmail;
            lbAddCc.DataSource = selectedEmail;
            lbAddCc.DataBind();

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

        protected void removeCc_Click(object sender, EventArgs e)
        {
            var items = (from ListItem li in lbAddCc.Items where li.Selected == true select li).ToList<ListItem>();
            foreach (ListItem item in items)
            {
                if (item.Selected)
                {
                    lbAddCc.Items.Remove(item);
                }
            }
            //lbAddCc.Items.Clear();
        }

        protected void btnEmailSave_Click(object sender, EventArgs e)
        {
            try
            {
                var date = txtDate.Text;
                DateTime selecteddate = Util.GetDateTime(date);
                if ((DateTime.Now - selecteddate).Days > 3)
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "", false);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "alert('Selected Date Cannot be greater than 3 days !!')", true);
                    txtDate.Focus();
                    return;
                }
                var emailMethod = ddlEmailMethod.SelectedItem.Text;
                var server = txtServer.Text;
                var port = txtPort.Text;
                var fromEmailID = txtFromEmailId.Text;
                var password = txtPassword.Text;
                TMPTrakDataBase.Inserttomailserver(emailMethod, server, port);
                TMPTrakDataBase.Inserttomail(fromEmailID, password);
                TMPTrakDataBase.Inserttomail(selecteddate.ToString("yyyy-MM-dd 13:00:00"));
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "Success" + 1, "<script>showpop('Successfully Saved !!')</script>", false);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "alert('Successfully Saved !!')", true);
                //TMPTrakDataBase.InsertEmailSettings(emailMethod, date, server, port, fromEmailID, password);
                BindAllFields();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void gridScheduleRpt_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridScheduleRpt.PageIndex = e.NewPageIndex;
            BindDataGrid();
        }
    }
}