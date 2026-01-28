using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik.DataBaseAccess;
using Web_TPMTrakDashboard.Advik.Models;

namespace Web_TPMTrakDashboard.Advik
{
    public partial class HelpRequestSetting : System.Web.UI.Page
    {
        bool isFilterdRule = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpContext.Current.Session["HelpRequestMachines"] = null;
                Session["EmpDetails"] = null;
                isFilterdRule = false;
                bindAllPlants();
                bindAllMachineIDs();
                bindAllHelpRequest();
                bindAllHelpActions();
                bindEmployees();
                populatePhoneNo();
                bindThresold();
                bindFilteredPlantId();
                bindFilterMachineIDs();
                bindFilteredHelpRequest();
                bindFilteredHelpAction();
                bindFilteredEmployees();
                bindGridData();
                //btnSearch_Click(null, null);
                btnDelete.Enabled = false;
            }
        }
        private void bindAllPlants()
        {
            try
            {
                List<string> plantIds = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllPlants();
                // List<string> plantIds = AdvikDatabaseAccess.GetPlantIds("");
                plantIds.Remove("All");
                ddlPlant.DataSource = plantIds;
                ddlPlant.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }

        }
        private void bindAllMachineIDs()
        {
            try
            {
                string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString();
                ddlMachineId.DataSource = AdvikDatabaseAccess.GetMachines(plant);
                ddlMachineId.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        private void bindAllHelpRequest()
        {
            try
            {
                List<string> allHealthRequest = new List<string>();
                allHealthRequest = AdvikDatabaseAccess.getAllCallType();
                ddlHelpRequest.DataSource = allHealthRequest;
                ddlHelpRequest.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        private void bindAllHelpActions()
        {
            try
            {
                List<string> allHelpActions = new List<string>();
                allHelpActions = AdvikDatabaseAccess.GetHelpActionId();
                ddlAction.DataSource = allHelpActions;
                ddlAction.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }

        }
        private void bindEmployees()
        {
            try
            {
                string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString();
                List<EmployeeDetail> employeeDetails = new List<EmployeeDetail>();
                employeeDetails = AdvikDatabaseAccess.GetEmployeeDetail(plant);

                Session["EmpDetails"] = employeeDetails;
                txt1stlvlSMS.Items.Clear();
                txt2ndlvlSMS.Items.Clear();
                txt3rdlvlSMS.Items.Clear();
                ddlEmployeeID.DataSource = employeeDetails;
                ddlEmployeeID.DataValueField = "EmployeeId";
                ddlEmployeeID.DataTextField = "EmployeeId";
                ddlEmployeeID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        private void populatePhoneNo()
        {
            try
            {
                txtEmpMblNo.Text = string.Empty;
                string employeeToBeSearched = ddlEmployeeID.SelectedItem == null ? "" : ddlEmployeeID.SelectedItem.ToString();
                EmployeeDetail ed = new EmployeeDetail();
                List<EmployeeDetail> employeeDetails = null;
                if (Session["EmpDetails"] == null)
                {
                    string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString();
                    employeeDetails = AdvikDatabaseAccess.GetEmployeeDetail(plant);
                }
                else
                {
                    employeeDetails = (List<EmployeeDetail>)Session["EmpDetails"];
                }
                ed = employeeDetails.Find(delegate (EmployeeDetail p) { return p.EmployeeId == employeeToBeSearched; });
                if (ed != null && ed.MobNo != null)
                {
                    txtEmpMblNo.Text = ed.MobNo;
                }
                else
                {
                    txtEmpMblNo.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }

        }
        private void bindThresold()
        {
            try
            {
                ddl2ndlvlThreshold.Items.Clear();
                ddl3rdlvlThreshold.Items.Clear();
                ddl3rdlvlThreshold.Items.Add("10");
                ddl2ndlvlThreshold.Items.Add("5");
                for (int i = 1; i < 24; i++)
                {
                    string item = Convert.ToString(5 + i * 5);
                    ddl2ndlvlThreshold.Items.Add(item);
                    ddl3rdlvlThreshold.Items.Add(Convert.ToString(10 + i * 5));
                }
                //if (ddl2ndlvlThreshold.Items.Count > 0)
                //{
                //    cmbThresold.SelectedIndex = 0;
                //}
                //if (cmbThresold3rdLvl.Items.Count > 0)
                //{
                //    cmbThresold3rdLvl.SelectedIndex = 0;
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }

        }
        private void bindFilteredPlantId()
        {
            try
            {
                List<string> plantIds = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllPlants();
                // List<string> plantIds = AdvikDatabaseAccess.GetPlantIds("");
                ddlFilterPlantID.DataSource = plantIds;
                ddlFilterPlantID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        private void bindFilterMachineIDs()
        {
            try
            {
                string plant = ddlFilterPlantID.SelectedValue == null ? "" : ddlFilterPlantID.SelectedItem.ToString();
                ddlFilterMachineID.DataSource = AdvikDatabaseAccess.GetMachines(plant);
                ddlFilterMachineID.DataBind();
                if (HttpContext.Current.Session["HelpRequestMachines"] == null)
                {
                    List<string> helprequestmachines = new List<string>();
                    foreach (ListItem item in ddlFilterMachineID.Items)
                    {
                        item.Selected = true;
                        helprequestmachines.Add(item.Value);
                    }
                    HttpContext.Current.Session["HelpRequestMachines"] = helprequestmachines;
                }
                else
                {
                    List<string> helprequestmachines = (List<string>)HttpContext.Current.Session["HelpRequestMachines"];
                    foreach (ListItem item in ddlFilterMachineID.Items)
                    {
                        if (helprequestmachines.Contains(item.Value))
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        private void bindFilteredHelpRequest()
        {

            try
            {
                List<string> allHealthRequest = new List<string>();
                allHealthRequest = AdvikDatabaseAccess.getAllCallType();
                allHealthRequest.Insert(0, "All");
                ddlFilterHelprequest.DataSource = allHealthRequest;
                ddlFilterHelprequest.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        private void bindFilteredHelpAction()
        {
            try
            {
                List<string> allHelpActions = new List<string>();
                allHelpActions = AdvikDatabaseAccess.GetHelpActionId();
                allHelpActions.Insert(0, "All");
                ddlFilterAction.DataSource = allHelpActions;
                ddlFilterAction.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        private void bindFilteredEmployees()
        {
            try
            {
                string plant = ddlFilterPlantID.SelectedValue == null ? "" : ddlFilterPlantID.SelectedItem.ToString();
                List<EmployeeDetail> employeeDetails = new List<EmployeeDetail>();
                employeeDetails = AdvikDatabaseAccess.GetEmployeeDetail(plant);
                EmployeeDetail employeeDetail = new EmployeeDetail();
                employeeDetail.EmployeeId = "All";
                employeeDetail.MobNo = "";
                employeeDetails.Insert(0, employeeDetail);
                // employeeDetails.Add(employeeDetail);
                ddlFilterEmployeeID.DataSource = employeeDetails;
                ddlFilterEmployeeID.DataValueField = "EmployeeId";
                ddlFilterEmployeeID.DataTextField = "EmployeeId";
                ddlFilterEmployeeID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void bindGridData()
        {
            try
            {
                if (isFilterdRule == false)
                {
                    if (ddlFilterMachineID.Items.Count > 0)
                    {
                        foreach (ListItem item in ddlFilterMachineID.Items)
                        {
                            item.Selected = true;
                        }
                    }
                }
                List<HelpRequestSettingDetails> helpRequestSettingDetailsList = new List<HelpRequestSettingDetails>();
                HelpRequestSettingDetails helpRequestSettingDetails = null;
                List<EmployeeDetail> employeeDetails = null;
                //if (Session["EmpDetails"] == null)
                //{
                //    string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString();
                employeeDetails = AdvikDatabaseAccess.GetEmployeeDetail("All");
                //    Session["EmpDetails"] = employeeDetails;
                //}
                //else
                //{
                //    employeeDetails = (List<EmployeeDetail>)Session["EmpDetails"];
                //}
                SqlDataReader dr = AdvikDatabaseAccess.GetHelpRequestRule(ddlFilterPlantID.SelectedValue == null ? "" : ddlFilterPlantID.SelectedItem.ToString(), ddlFilterMachineID, ddlFilterHelprequest.SelectedValue == null ? "" : ddlFilterHelprequest.SelectedItem.ToString(), ddlFilterAction.SelectedValue == null ? "" : ddlFilterAction.SelectedItem.ToString(), ddlFilterEmployeeID.SelectedValue == null ? "" : ddlFilterEmployeeID.SelectedItem.ToString(), isFilterdRule);
                EmployeeDetail empMob = null;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        helpRequestSettingDetails = new HelpRequestSettingDetails();
                        helpRequestSettingDetails.PlantID = dr["PlantID"].ToString();
                        helpRequestSettingDetails.MachineID = dr["machineID"].ToString();
                        helpRequestSettingDetails.RequestType = dr["Help_Description"].ToString();
                        helpRequestSettingDetails.Action = dr["Action"].ToString();


                        string[] empIds = dr["MobileNo"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        List<string> empIDVal = new List<string>();
                        foreach (var item in empIds)
                        {
                            empMob = employeeDetails.Find(p => p.EmployeeId.Trim() == item.Trim());
                            empIDVal.Add(string.Format("{0} ({1})", item, (empMob) != null ? empMob.MobNo : string.Empty));
                        }
                        helpRequestSettingDetails.Level1Empid = string.Join(",", empIDVal.ToArray());


                        if (!Convert.IsDBNull(dr["Level2Threshold"])) { helpRequestSettingDetails.Level2Threshold = Convert.ToString(Convert.ToInt32(dr["Level2Threshold"]) / 60); }


                        empIds = dr["Level2MobNo"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        empIDVal = new List<string>();
                        foreach (var item in empIds)
                        {
                            empMob = employeeDetails.Find(p => p.EmployeeId.Trim() == item.Trim());
                            empIDVal.Add(string.Format("{0} ({1})", item, (empMob) != null ? empMob.MobNo : string.Empty));
                        }

                        helpRequestSettingDetails.Level2Empid = string.Join(",", empIDVal.ToArray());

                        helpRequestSettingDetails.Message = dr["Message"].ToString();
                        helpRequestSettingDetails.SlNo = dr["SlNo"].ToString();
                        if (!Convert.IsDBNull(dr["Level3Threshold"])) { helpRequestSettingDetails.Level3Threshold = Convert.ToString(Convert.ToInt32(dr["Level3Threshold"]) / 60); }


                        empIds = dr["Level3MobNo"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        empIDVal = new List<string>();
                        foreach (var item in empIds)
                        {
                            empMob = employeeDetails.Find(p => p.EmployeeId.Trim() == item.Trim());
                            empIDVal.Add(string.Format("{0} ({1})", item, (empMob) != null ? empMob.MobNo : string.Empty));
                        }
                        empMob = employeeDetails.Find(p => p.EmployeeId == Convert.ToString(dr["Level3MobNo"]));
                        helpRequestSettingDetails.Level3Empid = string.Join(",", empIDVal.ToArray());
                        helpRequestSettingDetailsList.Add(helpRequestSettingDetails);
                    }
                    gvHelpRequestDetails.DataSource = helpRequestSettingDetailsList;
                    gvHelpRequestDetails.DataBind();
                }
                else
                {
                    gvHelpRequestDetails.DataSource = helpRequestSettingDetailsList;
                    gvHelpRequestDetails.DataBind();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindAllMachineIDs();
            bindEmployees();
            populatePhoneNo();
        }

        protected void ddlEmployeeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            populatePhoneNo();
        }

        protected void btnNewRule_Click(object sender, EventArgs e)
        {
            txt1stlvlSMS.Items.Clear();
            txt2ndlvlSMS.Items.Clear();
            txtMsg.Text = string.Empty;
            bindAllPlants();
            bindAllMachineIDs();
            bindAllHelpRequest();
            bindAllHelpActions();
            bindEmployees();
            populatePhoneNo();
            bindThresold();
            bindGridData();
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            btn1stlvlAdd.Enabled = true;
            btn1stlvlRemove.Enabled = true;
            btn2ndlvlAdd.Enabled = true;
            btn2ndlvlRemove.Enabled = true;
            btn3rdlvlAdd.Enabled = true;
            btn3rdlvlRemove.Enabled = true;
            //chkAll.Checked = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            removeEntries();
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            btnSearch_Click(null, null);
        }
        private void removeEntries()
        {
            string actionStatus = "Added";
            if (ddlPlant.Items.Count == 0)
            {
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = "Please select Plant ID.";
                ddlPlant.Focus();
                return;
            }
            int machineidCount = 0;
            foreach (ListItem item in ddlMachineId.Items)
            {
                if (item.Selected)
                {
                    //btn1stlvlAdd_Click(null, null);
                    machineidCount++;
                }
            }
            if (machineidCount == 0)
            {
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = "Please Select Machine ID.";
                ddlMachineId.Focus();
                return;
            }
            if (ddlHelpRequest.Items.Count == 0)
            {
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = "Please select HelpRequest ID.";
                ddlHelpRequest.Focus();
                return;
            }

            if (ddlAction.Items.Count == 0)
            {
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = "Please select Action ID.";
                ddlAction.Focus();
                return;
            }
            string smslvl1st = "";
            foreach (ListItem item in txt1stlvlSMS.Items)
            {
                if (smslvl1st == "")
                {
                    smslvl1st = item.Text;
                }
                else
                {
                    smslvl1st += "\n" + item.Text;
                }

            }
            if (smslvl1st == string.Empty || smslvl1st == "")
            {
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = "Please Add First level Mob No.";
                txt1stlvlSMS.Focus();
                return;

            }
            if (txtMsg.Text == string.Empty)
            {
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = "Please Enter message.";
                txtMsg.Focus();
                return;
            }

            List<string> selectedMachines = new List<string>();
            string UpdatedMachines = string.Empty;
            string mobNoLevel1 = string.Empty;
            string mobNoLevel2 = string.Empty;
            string mobNoLevel3 = string.Empty;
            foreach (ListItem item in ddlMachineId.Items)
            {
                if (item.Selected)
                {
                    selectedMachines.Add(item.Value);
                }
            }
            string smslvl2nd = "";
            foreach (ListItem item in txt2ndlvlSMS.Items)
            {
                if (smslvl2nd == "")
                {
                    smslvl2nd = item.Text;
                }
                else
                {
                    smslvl2nd += "\n" + item.Text;
                }
            }
            string smslvl3rd = "";
            foreach (ListItem item in txt3rdlvlSMS.Items)
            {
                if (smslvl3rd == "")
                {
                    smslvl3rd = item.Text;
                }
                else
                {
                    smslvl3rd += "\n" + item.Text;
                }

            }
            string[] mobNo1Temp = smslvl1st.Replace('\n', ':').Split(':');
            string[] mobNo2Temp = smslvl2nd.Replace('\n', ':').Split(':');
            string[] mobNo3Temp = smslvl3rd.Replace('\n', ':').Split(':');
            for (int i = 0; i < mobNo1Temp.Length; i++)
            {
                if (i % 2 == 0)
                {
                    if (mobNoLevel1 != string.Empty)
                    {
                        mobNoLevel1 = mobNoLevel1 + "," + mobNo1Temp[i].ToString().Trim();
                    }
                    else
                    {
                        mobNoLevel1 = mobNo1Temp[i].ToString().Trim();
                    }
                }

            }

            for (int i = 0; i < mobNo2Temp.Length; i++)
            {
                if (i % 2 == 0)
                {
                    if (mobNoLevel2 != string.Empty)
                    {
                        mobNoLevel2 = mobNoLevel2 + "," + mobNo2Temp[i].ToString().Trim();
                    }
                    else
                    {
                        mobNoLevel2 = mobNo2Temp[i].ToString().Trim();
                    }
                }

            }
            for (int i = 0; i < mobNo3Temp.Length; i++)
            {
                if (i % 2 == 0)
                {
                    if (mobNoLevel3 != string.Empty)
                    {
                        mobNoLevel3 = mobNoLevel3 + "," + mobNo3Temp[i].ToString().Trim();
                    }
                    else
                    {
                        mobNoLevel3 = mobNo3Temp[i].ToString().Trim();
                    }
                }

            }

            if (string.IsNullOrEmpty(mobNoLevel2) && !string.IsNullOrEmpty(mobNoLevel3))
            {
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = "Inordered to enable 3rd level SMS , 2nd level SMS should be filled !!!";
                return;
            }

            int threholdinSeconds = 0, SecondLvlthreholdinSeconds = 0;

            if (!string.IsNullOrEmpty(mobNoLevel2))
            {
                threholdinSeconds = Convert.ToInt32(ddl2ndlvlThreshold.Text) * 60;
            }

            if (!string.IsNullOrEmpty(mobNoLevel3))
            {
                SecondLvlthreholdinSeconds = Convert.ToInt32(ddl3rdlvlThreshold.Text) * 60;
            }

            if (!string.IsNullOrEmpty(mobNoLevel3) && !string.IsNullOrEmpty(mobNoLevel2))
            {
                // threholdinSeconds = Convert.ToInt32(ddl2ndlvlThreshold.Text) * 60;
                //SecondLvlthreholdinSeconds = Convert.ToInt32(ddl3rdlvlThreshold.Text) * 60;
                if (SecondLvlthreholdinSeconds < threholdinSeconds)
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "3rd level threshold should be greater than 2nd level threshold !!!";
                    return;
                }
            }



            foreach (string machine in selectedMachines)
            {
                if (AdvikDatabaseAccess.CheckBusinessruleExistence(ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString(), machine, ddlHelpRequest.SelectedValue == null ? "" : ddlHelpRequest.SelectedItem.ToString(), ddlAction.SelectedValue == null ? "" : ddlAction.SelectedItem.ToString()))
                {
                    actionStatus = "Updated";
                    break;
                }

            }

            foreach (string machine in selectedMachines)
            {
                AdvikDatabaseAccess.DeleteBusinessRulesBulk(ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString(), machine, ddlHelpRequest.SelectedValue == null ? "" : ddlHelpRequest.SelectedItem.ToString(), ddlAction.SelectedValue == null ? "" : ddlAction.SelectedItem.ToString());
                if (UpdatedMachines != string.Empty)
                {
                    UpdatedMachines = UpdatedMachines + ", " + machine;
                }
                else
                {
                    UpdatedMachines = machine;
                }
            }



            AdvikDatabaseAccess.AddBusinessRules(ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString(), selectedMachines, ddlHelpRequest.SelectedValue == null ? "" : ddlHelpRequest.SelectedItem.ToString(), ddlAction.SelectedValue == null ? "" : ddlAction.SelectedItem.ToString(), mobNoLevel1, mobNoLevel2, mobNoLevel3, threholdinSeconds, SecondLvlthreholdinSeconds, txtMsg.Text);
            if (UpdatedMachines != string.Empty)
            {
                lblMessages.ForeColor = System.Drawing.Color.Green;
                lblMessages.Text = string.Format("Record {0} for {1} Machines.\n For Help Request : {2} \n Action : {3}", actionStatus, UpdatedMachines, ddlHelpRequest.SelectedValue == null ? "" : ddlHelpRequest.SelectedItem.ToString(), ddlAction.SelectedValue == null ? "" : ddlAction.SelectedItem.ToString());
            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string deletedMachines = string.Empty;
            List<string> selectedMachines = new List<string>();
            string mobNoLevel1 = string.Empty;
            string mobNoLevel2 = string.Empty;
            foreach (ListItem item in ddlMachineId.Items)
            {
                if (item.Selected)
                {
                    selectedMachines.Add(item.Value);
                }
            }
            if (selectedMachines.Count > 0)
            {
                foreach (string machine in selectedMachines)
                {
                    AdvikDatabaseAccess.DeleteBusinessRulesBulk(ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString(), machine, ddlHelpRequest.SelectedValue == null ? "" : ddlHelpRequest.SelectedItem.ToString(), ddlAction.SelectedValue == null ? "" : ddlAction.SelectedItem.ToString());
                    if (deletedMachines != string.Empty)
                    {
                        deletedMachines = deletedMachines + ", " + machine;
                    }
                    else
                    {
                        deletedMachines = machine;
                    }
                }
            }
            if (deletedMachines != string.Empty)
            {
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = string.Format("Record deleted for {0} Machines.", deletedMachines);
            }

            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            bindGridData();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            isFilterdRule = true;
            bindGridData();
        }
        protected void gvHelpRequestDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index = gvHelpRequestDetails.SelectedRow.RowIndex;
                string slno = (gvHelpRequestDetails.SelectedRow.FindControl("hfSlNo") as HiddenField).Value;
                EditRule(slno);
                //if (isAdmin)
                //{
                btnDelete.Enabled = true;
                //}
                btnSave.Enabled = true;
                bindFilterMachineIDs();
                ddlEmployeeID_SelectedIndexChanged(null, null);
                foreach (GridViewRow row in gvHelpRequestDetails.Rows)
                {
                    if (row.RowIndex == gvHelpRequestDetails.SelectedIndex)
                    {
                        row.BackColor = ColorTranslator.FromHtml("#18228c");
                    }
                    else
                    {
                        row.BackColor = ColorTranslator.FromHtml("#202648"); /*ColorTranslator.FromHtml("#FFFFFF");*/
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;

            }

        }
        private void EditRule(string rulNo)
        {
            SqlDataReader sdr = AdvikDatabaseAccess.GetBusinessRule(rulNo);
            List<EmployeeDetail> employeeDetails = null;
            if (Session["EmpDetails"] == null)
            {
                string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString();
                employeeDetails = AdvikDatabaseAccess.GetEmployeeDetail(plant);
                Session["EmpDetails"] = employeeDetails;
            }
            else
            {
                employeeDetails = (List<EmployeeDetail>)Session["EmpDetails"];
            }
            try
            {
                if (sdr.HasRows)
                {
                    sdr.Read();
                    string[] tempEmpIds1 = sdr["MobileNo"].ToString().Split(',');
                    string[] tempEmpIds2 = sdr["Level2MobNo"].ToString().Split(',');
                    string[] tempEmpIds3 = sdr["Level3MobNo"].ToString().Split(',');
                    string level1Mob = string.Empty;
                    string level2Mob = string.Empty;
                    string level3Mob = string.Empty;
                    ddlPlant.Text = sdr["PlantID"].ToString();
                    bindAllMachineIDs();
                    bindEmployees();
                    foreach (ListItem item in ddlMachineId.Items)
                    {
                        item.Selected = false;
                    }
                    foreach (ListItem item in ddlMachineId.Items)
                    {
                        if (item.Text == sdr["machineID"].ToString().Trim())
                        {
                            item.Selected = true;
                        }
                    }
                    foreach (string str in tempEmpIds1)
                    {
                        EmployeeDetail ed = new EmployeeDetail();
                        ed = employeeDetails.Find(delegate (EmployeeDetail p) { return p.EmployeeId.Trim() == str.Trim(); });
                        if (ed != null)
                        {
                            if (level1Mob == string.Empty)
                            {
                                level1Mob = str + ":" + ed.MobNo;
                            }
                            else
                            {
                                level1Mob = level1Mob + "\n" + str + ":" + ed.MobNo;
                            }
                        }
                    }
                    foreach (string str in tempEmpIds2)
                    {
                        EmployeeDetail ed = new EmployeeDetail();
                        ed = employeeDetails.Find(delegate (EmployeeDetail p) { return p.EmployeeId.Trim() == str.Trim(); });
                        if (ed != null)
                        {
                            if (level2Mob == string.Empty)
                            {
                                level2Mob = str + ":" + ed.MobNo;
                            }
                            else
                            {
                                level2Mob = level2Mob + "\n" + str + ":" + ed.MobNo;
                            }
                        }
                    }
                    foreach (string str in tempEmpIds3)
                    {
                        EmployeeDetail ed = new EmployeeDetail();
                        ed = employeeDetails.Find(delegate (EmployeeDetail p) { return p.EmployeeId.Trim() == str.Trim(); });
                        if (ed != null)
                        {
                            if (level3Mob == string.Empty)
                            {
                                level3Mob = str + ":" + ed.MobNo;
                            }
                            else
                            {
                                level3Mob = level3Mob + "\n" + str + ":" + ed.MobNo;
                            }
                        }
                    }
                    ddlHelpRequest.Text = sdr["Help_Description"].ToString();
                    ddlAction.Text = sdr["Action"].ToString();
                    List<string> lvl1stsmslist = new List<string>();
                    foreach (string str in tempEmpIds1)
                    {
                        EmployeeDetail ed = new EmployeeDetail();
                        ed = employeeDetails.Find(delegate (EmployeeDetail p) { return p.EmployeeId.Trim() == str.Trim(); });
                        if (ed != null)
                        {
                            lvl1stsmslist.Add(str + ":" + ed.MobNo);
                        }
                    }
                    List<string> lvl2ndsmslist = new List<string>();
                    foreach (string str in tempEmpIds2)
                    {
                        EmployeeDetail ed = new EmployeeDetail();
                        ed = employeeDetails.Find(delegate (EmployeeDetail p) { return p.EmployeeId.Trim() == str.Trim(); });
                        if (ed != null)
                        {
                            lvl2ndsmslist.Add(str + ":" + ed.MobNo);
                        }
                    }
                    List<string> lvl3rdsmslist = new List<string>();
                    foreach (string str in tempEmpIds3)
                    {
                        EmployeeDetail ed = new EmployeeDetail();
                        ed = employeeDetails.Find(delegate (EmployeeDetail p) { return p.EmployeeId.Trim() == str.Trim(); });
                        if (ed != null)
                        {
                            lvl3rdsmslist.Add(str + ":" + ed.MobNo);
                        }
                    }
                    txt1stlvlSMS.DataSource = lvl1stsmslist;
                    txt1stlvlSMS.SelectedValue = null;
                    txt1stlvlSMS.DataBind();
                    txt2ndlvlSMS.DataSource = lvl2ndsmslist;
                    txt2ndlvlSMS.SelectedValue = null;
                    txt2ndlvlSMS.DataBind();
                    txt3rdlvlSMS.DataSource = lvl3rdsmslist;
                    txt3rdlvlSMS.SelectedValue = null;
                    txt3rdlvlSMS.DataBind();
                    txtMsg.Text = sdr["Message"].ToString();
                    bool existsinddl2ndThresold = false;
                    Int64 Level2Threshold = 0;
                    foreach (ListItem item in ddl2ndlvlThreshold.Items)
                    {
                        if (sdr["Level2Threshold"].ToString() != "")
                        {
                            Level2Threshold = Convert.ToInt64(sdr["Level2Threshold"].ToString()) / 60;
                        }
                        if (item.Text == Level2Threshold.ToString())
                        {
                            existsinddl2ndThresold = true;
                            break;
                        }
                    }
                    if (existsinddl2ndThresold)
                    {
                        ddl2ndlvlThreshold.Text = Level2Threshold.ToString();
                    }
                    else
                    {
                        ddl2ndlvlThreshold.SelectedIndex = 0;
                    }
                    bool existsinddl3rdThresold = false;
                    Int64 level3value = 0;
                    foreach (ListItem item in ddl3rdlvlThreshold.Items)
                    {
                        if (sdr["Level3Threshold"].ToString() != "")
                        {
                            level3value = Convert.ToInt64(sdr["Level3Threshold"].ToString()) / 60;
                        }
                        if (item.Text == level3value.ToString())
                        {
                            existsinddl3rdThresold = true;
                            break;
                        }
                    }
                    if (existsinddl3rdThresold)
                    {
                        ddl3rdlvlThreshold.Text = level3value.ToString();
                    }
                    else
                    {
                        ddl3rdlvlThreshold.SelectedIndex = 0;
                    }
                    //ddl2ndlvlThreshold.Text = sdr["Level2Threshold"].ToString() == "0" ? ddl2ndlvlThreshold.Text : sdr["Level2Threshold"].ToString();
                    //ddl3rdlvlThreshold.Text = sdr["Level3Threshold"].ToString() == "0" ? ddl3rdlvlThreshold.Text : sdr["Level3Threshold"].ToString();

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                lblMessages.Text = ex.Message;
                ddl2ndlvlThreshold.Text = ddl2ndlvlThreshold.Text;
                ddl3rdlvlThreshold.Text = ddl3rdlvlThreshold.Text;
            }
            finally
            {
                if (sdr != null) sdr.Close();
            }
        }

        protected void gvHelpRequestDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvHelpRequestDetails, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }
        protected void btn1stlvlAdd_Click(object sender, EventArgs e)
        {
            try
            {
                List<EmployeeDetail> employeeDetails = null;
                List<string> selected = new List<string>();
                if (Session["EmpDetails"] == null)
                {
                    string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString();
                    employeeDetails = AdvikDatabaseAccess.GetEmployeeDetail(plant);
                    Session["EmpDetails"] = employeeDetails;
                }
                else
                {
                    employeeDetails = (List<EmployeeDetail>)Session["EmpDetails"];
                }
                if (txtEmpMblNo.Text == string.Empty)
                {
                    return;
                }
                foreach (ListItem item in txt1stlvlSMS.Items)
                {
                    selected.Add(item.Text);
                    //if (item.Text.Contains(txtEmpMblNo.Text))
                    //{
                    //    lblMessages.ForeColor = System.Drawing.Color.Red;
                    //    lblMessages.Text = "This Mobile No already exists.";
                    //    return;
                    //}

                }
                //if (txt1stlvlSMS.Text.IndexOf("\n") == 0)
                //{
                //    txt1stlvlSMS.Text = string.Empty;
                //}
                //if (txt1stlvlSMS.Text != string.Empty)
                //{
                //    txt1stlvlSMS.Text = Environment.NewLine;
                //}
                EmployeeDetail ed = new EmployeeDetail();
                ed = employeeDetails.Find(delegate (EmployeeDetail p) { return p.MobNo == txtEmpMblNo.Text && p.EmployeeId == ddlEmployeeID.SelectedValue.ToString(); });
                string Text = ed.EmployeeId + ":" + txtEmpMblNo.Text;

                selected.Add(Text);
                txt1stlvlSMS.DataSource = selected;
                txt1stlvlSMS.SelectedValue = null;
                txt1stlvlSMS.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;

            }
        }

        protected void btn1stlvlRemove_Click(object sender, EventArgs e)
        {
            //var items = (from ListItem li in txt1stlvlSMS.Items).ToList<ListItem>();
            foreach (ListItem item in txt1stlvlSMS.Items)
            {
                //if (item.Selected)
                //{
                txt1stlvlSMS.Items.Remove(item);
                return;
                //}
            }
        }
        protected void btn2ndlvlAdd_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> selected = new List<string>();
                List<EmployeeDetail> employeeDetails = null;
                if (Session["EmpDetails"] == null)
                {
                    string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString();
                    employeeDetails = AdvikDatabaseAccess.GetEmployeeDetail(plant);
                    Session["EmpDetails"] = employeeDetails;
                }
                else
                {
                    employeeDetails = (List<EmployeeDetail>)Session["EmpDetails"];
                }
                if (txtEmpMblNo.Text == string.Empty)
                {
                    return;
                }
                foreach (ListItem item in txt2ndlvlSMS.Items)
                {
                    selected.Add(item.Text);
                    //if (item.Text.Contains(txtEmpMblNo.Text))
                    //{
                    //    lblMessages.ForeColor = System.Drawing.Color.Red;
                    //    lblMessages.Text = "This Mobile No already exists.";
                    //    return;
                    //}

                }
                //if (txt2ndlvlSMS.Text.IndexOf("\n") == 0)
                //{
                //    txt2ndlvlSMS.Text = string.Empty;
                //}
                //if (txt2ndlvlSMS.Text != string.Empty)
                //{
                //    txt2ndlvlSMS.Text = Environment.NewLine;
                //}
                EmployeeDetail ed = new EmployeeDetail();
                ed = employeeDetails.Find(delegate (EmployeeDetail p) { return p.MobNo == txtEmpMblNo.Text && p.EmployeeId == ddlEmployeeID.SelectedValue.ToString(); });
                string Text = ed.EmployeeId + ":" + txtEmpMblNo.Text;

                selected.Add(Text);
                txt2ndlvlSMS.DataSource = selected;
                txt2ndlvlSMS.SelectedValue = null;
                txt2ndlvlSMS.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void btn2ndlvlRemove_Click(object sender, EventArgs e)
        {

            //var items = (from ListItem li in txt2ndlvlSMS.Items where li.Selected == true select li).ToList<ListItem>();
            foreach (ListItem item in txt2ndlvlSMS.Items)
            {

                txt2ndlvlSMS.Items.Remove(item);
                return;
                //}
            }
            //int firstcharindex = rtbMob2.GetFirstCharIndexOfCurrentLine();
            //int currentline = rtbMob2.GetLineFromCharIndex(firstcharindex);
            //string currentlinetext = rtbMob2.Lines[currentline];
            //rtbMob2.Select(firstcharindex, currentlinetext.Length);

            //rtbMob2.Text = rtbMob2.Text.Remove(rtbMob2.SelectionStart, rtbMob2.SelectionLength);
            //rtbMob2.Text = rtbMob2.Text.Replace("\n\n", "\n");
            //rtbMob2.Text = rtbMob2.Text.Trim();

        }

        protected void btn3rdlvlAdd_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> selected = new List<string>();
                List<EmployeeDetail> employeeDetails = null;
                if (Session["EmpDetails"] == null)
                {
                    string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString();
                    employeeDetails = AdvikDatabaseAccess.GetEmployeeDetail(plant);
                    Session["EmpDetails"] = employeeDetails;
                }
                else
                {
                    employeeDetails = (List<EmployeeDetail>)Session["EmpDetails"];
                }
                if (txtEmpMblNo.Text == string.Empty)
                {
                    return;
                }
                foreach (ListItem item in txt3rdlvlSMS.Items)
                {
                    selected.Add(item.Text);
                    //if (item.Text.Contains(txtEmpMblNo.Text))
                    //{
                    //    lblMessages.ForeColor = System.Drawing.Color.Red;
                    //    lblMessages.Text = "This Mobile No already exists.";
                    //    return;
                    //}

                }
                //if (txt3rdlvlSMS.Text.IndexOf("\n") == 0)
                //{
                //    txt3rdlvlSMS.Text = string.Empty;
                //}
                //if (txt3rdlvlSMS.Text != string.Empty)
                //{
                //    txt3rdlvlSMS.Text = Environment.NewLine;
                //}

                EmployeeDetail ed = new EmployeeDetail();
                ed = employeeDetails.Find(delegate (EmployeeDetail p) { return p.MobNo == txtEmpMblNo.Text && p.EmployeeId == ddlEmployeeID.SelectedValue.ToString(); });
                string Text = ed.EmployeeId + ":" + txtEmpMblNo.Text;
                selected.Add(Text);
                txt3rdlvlSMS.DataSource = selected;
                txt3rdlvlSMS.SelectedValue = null;
                txt3rdlvlSMS.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }

        }

        protected void btn3rdlvlRemove_Click(object sender, EventArgs e)
        {

            //var items = (from ListItem li in txt3rdlvlSMS.Items where li.Selected == true select li).ToList<ListItem>();
            foreach (ListItem item in txt3rdlvlSMS.Items)
            {

                txt3rdlvlSMS.Items.Remove(item);
                return;
            }
            //int firstcharindex = richMob3.GetFirstCharIndexOfCurrentLine();
            //int currentline = richMob3.GetLineFromCharIndex(firstcharindex);
            //string currentlinetext = richMob3.Lines[currentline];
            //richMob3.Select(firstcharindex, currentlinetext.Length);

            //richMob3.Text = richMob3.Text.Remove(richMob3.SelectionStart, richMob3.SelectionLength);
            //richMob3.Text = richMob3.Text.Replace("\n\n", "\n");
            //richMob3.Text = richMob3.Text.Trim();
        }
    }

}