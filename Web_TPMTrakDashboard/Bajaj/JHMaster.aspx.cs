using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Bajaj
{
    public partial class JHMaster : System.Web.UI.Page
    {
        public List<ModelClassLibrary.UserAccessModel> useAccessData = null;
        //public bool isModifiable = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserName"] == null)
                {
                    Response.Redirect("~/SignIn.aspx", false);
                    return;
                }
                if (Session["UserAccessData"] == null)
                    Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"].ToString());
                else
                    useAccessData = Session["UserAccessData"] as List<ModelClassLibrary.UserAccessModel>;

                ViewState["isModifiable"] = useAccessData.Where(ss => ss.Code.Equals("JMMasterModification", StringComparison.OrdinalIgnoreCase) & ss.Domain.Equals("Bajaj", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                BindRoleDetails();
                BindPlant();
                BindJHDetails();
            }
        }
        private void BindRoleDetails()
        {
            try
            {
                List<string> list = BajajDBAccess.GetRoleDetails("Manager");
                list.Insert(0, "");
                ddlManager.DataSource = list;
                ddlManager.DataBind();

                list = BajajDBAccess.GetRoleDetails("Group Leader");
                list.Insert(0, "");
                ddlGroupLeader.DataSource = list;
                ddlGroupLeader.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindRoleDetails: " + ex.Message);
            }
        }
        private void BindPlant()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlantsForPlantInfo();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();

                BindCell();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPlant: " + ex.Message);
            }
        }
        private void BindCell()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllGroupId(ddlPlant.SelectedValue);
                ddlCell.DataSource = list;
                ddlCell.DataBind();

                BindMachine();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindCell: " + ex.Message);
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllMachinedByLineandGroup(ddlPlant.SelectedValue, ddlCell.SelectedValue);
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();

                BindMachineInterfaceID();
                BindMachineRevisionNo();
                BindParameters();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachine: " + ex.Message);
            }
        }
        private void BindMachineInterfaceID()
        {
            try
            {
                string value = DataBaseAccess.getMachineinterdaceid(ddlMachine.SelectedValue);
                lblMachineInterfaceID.Text = value;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachineInterfaceID: " + ex.Message);
            }
        }

        private void BindMachineRevisionNo()
        {
            try
            {
                List<ListItem> list = BajajDBAccess.GetRevisionDetailsForMachine(ddlMachine.SelectedValue);
                ddlRevNo.DataSource = list;
                ddlRevNo.DataTextField = "Text";
                ddlRevNo.DataValueField = "Value";
                ddlRevNo.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachineRevisionNo: " + ex.Message);
            }
        }
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindCell();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlPlant_SelectedIndexChanged: " + ex.Message);
            }
        }

        protected void ddlCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindMachine();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlCell_SelectedIndexChanged: " + ex.Message);
            }
        }

        protected void ddlMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindMachineInterfaceID();
                BindMachineRevisionNo();
                BindParameters();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlMachine_SelectedIndexChanged: " + ex.Message);
            }
        }

        private void BindParameters()
        {
            try
            {
                List<ListItem> list = BajajDBAccess.GetParametersForMachine(ddlMachine.SelectedValue);
                ddlParameter.DataSource = list;
                ddlParameter.DataTextField = "Text";
                ddlParameter.DataValueField = "Value";
                ddlParameter.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindParameters: " + ex.Message);
            }
        }
        private void BindValues()
        {
            try
            {
                List<string> list = new List<string>();

                list.Add("Leads to Defect");
                list.Add("Leads to Break down");
                list.Add("Leads to Accident");
                ddlRelatedTo.DataSource = list;
                ddlRelatedTo.DataBind();

                list = new List<string>();
                list.Add("D");
                list.Add("W");
                list.Add("Q");
                ddlFrequency.DataSource = list;
                ddlFrequency.DataBind();

                list = new List<string>();
                list.Add("C");
                list.Add("L");
                list.Add("I");
                list.Add("RT");
                cblC_L_I_RT_Value.DataSource = list;
                cblC_L_I_RT_Value.DataBind();

                list = new List<string>();
                list.Add("Auto");
                list.Add("Manual");
                ddlMethodType.DataSource = list;
                ddlMethodType.DataBind();

                list = new List<string>();
                list.Add("Alpha Numeric");
                list.Add("Numeric");
                ddlDataType.DataSource = list;
                ddlDataType.DataBind();

                list = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                ddlDay.DataSource = list;
                ddlDay.DataBind();

                list = new List<string>() { "1", "2", "3", "4" };
                ddlDayNo.DataSource = list;
                ddlDayNo.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindValues: " + ex.Message);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindJHDetails();
        }
        private void BindJHDetails()
        {
            try
            {
                ViewState["Machine"] = ddlMachine.SelectedValue;
                ViewState["MachineInterfaceID"] = lblMachineInterfaceID.Text;
                ViewState["RevNo"] = ddlRevNo.SelectedItem == null ? "" : ddlRevNo.SelectedItem.ToString();
                ViewState["RevID"] = ddlRevNo.SelectedValue;
                List<JHMasterDetails> list = BajajDBAccess.GetJHMasterDetails(ViewState["Machine"].ToString(), ViewState["RevNo"].ToString(), (bool)ViewState["isModifiable"]);

                lvJHMasterDetails.DataSource = list;
                lvJHMasterDetails.DataBind();

                Session["MasterData"] = list;

                hfRevID.Value = list.Select(x => x.RevID).FirstOrDefault();
                txtRevNo.Text = list.Select(x => x.RevNo).FirstOrDefault();
                txtRevDate.Text = list.Select(x => x.RevDate).FirstOrDefault();
                lblManager.Text = list.Select(x => x.Manager).FirstOrDefault();
                lblGroupLeader.Text = list.Select(x => x.GroupLeader).FirstOrDefault();

                if ((bool)ViewState["isModifiable"])
                {
                    btnAddJHMasterDetails.Visible = true;
                    if (list.Count > 0)
                    {
                        btnCopyToMachine.Visible = true;
                    }
                    else
                    {
                        btnCopyToMachine.Visible = false;
                    }


                    if (string.IsNullOrEmpty(ViewState["RevNo"].ToString()))
                    {
                        btnNewRevisin.Visible = false;
                        btnAddEditRevLevelData.Visible = false;
                    }
                    else
                    {
                        btnNewRevisin.Visible = true;
                        btnAddEditRevLevelData.Visible = true;
                    }
                }
                else
                {
                    btnAddJHMasterDetails.Visible = false;
                    btnCopyToMachine.Visible = false;
                    btnNewRevisin.Visible = false;
                    btnAddEditRevLevelData.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindJHDetails: " + ex.Message);
            }
        }
        protected void lvJHMasterDetails_DataBound(object sender, EventArgs e)
        {
            try
            {
                HelperClass.ShowHideActionColumn(lvJHMasterDetails, (bool)ViewState["isModifiable"]);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lvJHMasterDetails_DataBound: " + ex.Message);
            }

        }

        protected void btnAddJHMasterDetails_Click(object sender, EventArgs e)
        {
            try
            {
                txtCheckPointNo.Text = "";
                txtRouteNo.Text = "";
                txtItem.Text = "";
                txtCheckPoint.Text = "";
                txtStandard.Text = "";
                txtIfNotOk.Text = "";
                txtMethod.Text = "";
                txtMachineCondition.Text = "";
                txtTime.Text = "";
                txtRemarks.Text = "";
                txtMin.Text = "";
                txtMax.Text = "";
                txtUnit.Text = "";
                hfDrawing.Value = "";
                hfDrawingName.Value = "";
                lblDrawingName.Text = "";
                clearDrawing.Visible = false;

                BindValues();

                ddlFrequency_SelectedIndexChanged(null, null);


                if (hfFromCreateRevision.Value == "Yes")
                {
                    txtRevNo.Enabled = true;
                    txtRevDate.Enabled = true;
                    txtRevNo.Text = "";
                    txtRevDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    hfRevID.Value = "";
                    hfJHNewEdit.Value = "NewRev";
                }
                else
                {
                    if (string.IsNullOrEmpty(ddlRevNo.SelectedValue))
                    {
                        txtRevNo.Enabled = true;
                        txtRevDate.Enabled = true;

                        txtRevNo.Text = "";
                        txtRevDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                        hfRevID.Value = "";
                    }
                    else
                    {
                        txtRevNo.Enabled = false;
                        txtRevDate.Enabled = false;

                        List<JHMasterDetails> list = new List<JHMasterDetails>();
                        if (Session["MasterData"] != null)
                        {
                            list = (List<JHMasterDetails>)Session["MasterData"];
                        }

                        hfRevID.Value = list.Select(x => x.RevID).FirstOrDefault();
                        txtRevNo.Text = list.Select(x => x.RevNo).FirstOrDefault();
                        txtRevDate.Text = list.Select(x => x.RevDate).FirstOrDefault();
                    }

                    hfJHNewEdit.Value = "New";
                }



                txtCheckPointNo.ReadOnly = false;

               
                btnJHDetailsSave.Text = "ADD";
                modalTitle.InnerText = "Add JH Details";

                HelperClass.openAddEditModal(this, "neweditJHModal");
                HelperClass.openFunction(this, "MethodTypeChange");


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnAddJHMasterDetails_Click: " + ex.Message);
            }
        }
        protected void lbEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewDataItem).DataItemIndex);
                txtCheckPointNo.Text = (lvJHMasterDetails.Items[rowIndex].FindControl("lblCheckPointNo") as Label).Text;
                txtRouteNo.Text = (lvJHMasterDetails.Items[rowIndex].FindControl("lblRouteNo") as Label).Text;
                txtItem.Text = (lvJHMasterDetails.Items[rowIndex].FindControl("lblItem") as Label).Text;
                txtCheckPoint.Text = (lvJHMasterDetails.Items[rowIndex].FindControl("lblCheckPoint") as Label).Text;
                txtStandard.Text = (lvJHMasterDetails.Items[rowIndex].FindControl("lblStandard") as Label).Text;
                txtIfNotOk.Text = (lvJHMasterDetails.Items[rowIndex].FindControl("lblIfNotOk") as Label).Text;
                txtMethod.Text = (lvJHMasterDetails.Items[rowIndex].FindControl("lblMethod") as Label).Text;
                txtMachineCondition.Text = (lvJHMasterDetails.Items[rowIndex].FindControl("lblMachineCondition") as Label).Text;
                txtTime.Text = (lvJHMasterDetails.Items[rowIndex].FindControl("lblTime") as Label).Text;
                txtRemarks.Text = (lvJHMasterDetails.Items[rowIndex].FindControl("lblRemarks") as Label).Text;
                txtMin.Text = (lvJHMasterDetails.Items[rowIndex].FindControl("lblMin") as Label).Text;
                txtMax.Text = (lvJHMasterDetails.Items[rowIndex].FindControl("lblMax") as Label).Text;
                txtUnit.Text = (lvJHMasterDetails.Items[rowIndex].FindControl("lblUnit") as Label).Text;
                hfDrawing.Value = (lvJHMasterDetails.Items[rowIndex].FindControl("hfDrawingInBase64") as HiddenField).Value;
                hfDrawingName.Value = (lvJHMasterDetails.Items[rowIndex].FindControl("lbDrawing") as LinkButton).Text;
                lblDrawingName.Text = hfDrawingName.Value;
                if (lblDrawingName.Text != "")
                {
                    clearDrawing.Visible = true;
                }
                else
                {
                    clearDrawing.Visible = false;
                }

                BindValues();


                string value = (lvJHMasterDetails.Items[rowIndex].FindControl("lblFrequency") as Label).Text;
                if (ddlFrequency.Items.FindByValue(value) != null)
                {
                    ddlFrequency.SelectedValue = value;
                }
                value = (lvJHMasterDetails.Items[rowIndex].FindControl("hfDay") as HiddenField).Value;
                if (ddlDay.Items.FindByValue(value) != null)
                {
                    ddlDay.SelectedValue = value;
                }
                value = (lvJHMasterDetails.Items[rowIndex].FindControl("hfDayNo") as HiddenField).Value;
                if (ddlDayNo.Items.FindByValue(value) != null)
                {
                    ddlDayNo.SelectedValue = value;
                }
                value = (lvJHMasterDetails.Items[rowIndex].FindControl("hfRelatedTo") as HiddenField).Value;
                if (ddlRelatedTo.Items.FindByValue(value) != null)
                {
                    ddlRelatedTo.SelectedValue = value;
                }
                value = (lvJHMasterDetails.Items[rowIndex].FindControl("lblC_L_I_RT_Value") as Label).Text;
                if (value != "")
                {
                    string[] values = value.Split(',');
                    foreach (string val in values)
                    {
                        foreach (ListItem item in cblC_L_I_RT_Value.Items)
                        {
                            if (item.Value == val)
                            {
                                item.Selected = true;
                                break;
                            }
                        }
                    }
                }
                value = (lvJHMasterDetails.Items[rowIndex].FindControl("lblMethodType") as Label).Text;
                if (ddlMethodType.Items.FindByValue(value) != null)
                {
                    ddlMethodType.SelectedValue = value;
                }
                value = (lvJHMasterDetails.Items[rowIndex].FindControl("hfParameterID") as HiddenField).Value;
                if (ddlParameter.Items.FindByValue(value) != null)
                {
                    ddlParameter.SelectedValue = value;
                }
                value = (lvJHMasterDetails.Items[rowIndex].FindControl("lblDataType") as Label).Text;
                if (ddlDataType.Items.FindByValue(value) != null)
                {
                    ddlDataType.SelectedValue = value;
                }

                ddlFrequency_SelectedIndexChanged(null, null);

                txtRevNo.Enabled = false;
                txtRevDate.Enabled = false;

                txtCheckPointNo.ReadOnly = false;

                hfJHNewEdit.Value = "Edit";
                btnJHDetailsSave.Text = "UPDATE";
                modalTitle.InnerText = "Edit JH Details";

                HelperClass.openAddEditModal(this, "neweditJHModal");
                HelperClass.openFunction(this, "MethodTypeChange");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lbEdit_Click: " + ex.Message);
            }

        }
        protected void btnJHDetailsSave_Click(object sender, EventArgs e)
        {
            try
            {
                JHMasterDetails data = new JHMasterDetails();
                data.RevID = hfRevID.Value;
                data.RevNo = txtRevNo.Text;
                data.RevDate = txtRevDate.Text;
                data.Machine = ViewState["Machine"].ToString();
                data.MachineInterfaceID = ViewState["MachineInterfaceID"].ToString();
                data.CheckPointNo = txtCheckPointNo.Text;
                data.RouteNo = txtRouteNo.Text;
                data.RelatedTo = ddlRelatedTo.SelectedValue;
                data.Frequency = ddlFrequency.SelectedValue;
                string value = "";
                foreach (ListItem item in cblC_L_I_RT_Value.Items)
                {
                    if (item.Selected)
                    {

                        if (value == "")
                        {
                            value = item.Value;
                        }
                        else
                        {
                            value += "," + item.Value;
                        }
                    }
                }
                data.C_L_I_RT_Value = value;
                data.Item = txtItem.Text;
                data.CheckPoint = txtCheckPoint.Text;
                data.Standard = txtStandard.Text;
                data.IfNotOk = txtIfNotOk.Text;
                data.Method = txtMethod.Text;
                if (data.Frequency == "D")
                {
                    data.Day = lblDay.Text;
                    data.DayNo = "";
                    data.Month = "";
                }
                else if (data.Frequency == "W")
                {
                    data.Day = ddlDay.SelectedValue;
                    data.DayNo = "";
                    data.Month = "";
                }
                else if (data.Frequency == "Q")
                {
                    data.Day = ddlDay.SelectedValue;
                    data.DayNo = ddlDayNo.SelectedValue;
                    data.Month = txtMonth.Text;
                }

                data.MachineCondition = txtMachineCondition.Text;
                data.Time = txtTime.Text;
                data.Remarks = txtRemarks.Text;
                data.Min = txtMin.Text;
                data.Max = txtMax.Text;
                data.Unit = txtUnit.Text;
                data.MethodType = ddlMethodType.SelectedValue;
                if (data.MethodType == "Auto")
                {
                    data.ParameterID = ddlParameter.SelectedValue;
                    data.ParameterName = ddlParameter.SelectedItem == null ? "" : ddlParameter.SelectedItem.ToString();
                }
                else
                {
                    data.ParameterID = "";
                    data.ParameterName = "";
                }

                data.DataType = ddlDataType.SelectedValue;
                if (hfDrawing.Value == "")
                {

                }
                else
                {
                    string drawing = hfDrawing.Value;
                    byte[] drawinginbytes = System.Convert.FromBase64String(drawing.Substring(drawing.LastIndexOf(',') + 1));
                    data.Drawing = drawinginbytes;
                    data.DrawingName = hfDrawingName.Value;
                }
                data.NewOrEdit = hfJHNewEdit.Value;
                data.Param = "Save";
                string success = BajajDBAccess.SaveJHMasterDetails(data);
                if (success == "Exist")
                {
                    HelperClass.openModal(this, "neweditJHModal", false);
                    HelperClass.openWarningModal(this, "Record already exists.");
                    return;
                }
                else if (success == "Inserted")
                {
                    HelperClass.openInsertSuccessModal(this);
                    HelperClass.clearModal(this);
                }
                else if (success == "Updated")
                {

                    HelperClass.openUpdateSuccessModal(this);
                    HelperClass.clearModal(this);
                }
                else
                {
                    HelperClass.openModal(this, "neweditJHModal", false);
                    HelperClass.openInsertErrorModal(this);
                    return;
                }
                if (ddlRevNo.SelectedValue.Length <= 0 || hfFromCreateRevision.Value == "Yes")
                {
                    BindMachineRevisionNo();
                    hfFromCreateRevision.Value = "";
                }
                BindJHDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnJHDetailsSave_Click: " + ex.Message);
            }
        }

        protected void btnNewRevisin_Click(object sender, EventArgs e)
        {
            try
            {
                hfNewRevOrCopyToMc.Value = "Revision";
                btnCreateNewRev.Visible = true;

                tdMachineLbl.Visible = false;
                tdMachineValue.Visible = false;
                tdRevDateLbl.Visible = true;
                tdRevDateValue.Visible = true;
                txtRevNo_RevOrCopyToMc.Visible = true;
                ddlRevNo_RevOrCopyToMc.Visible = false;
                tdExistingRevOrNewRev.Visible = false;

                BindJHDetailsFoNewRevOrCopyToMc();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnNewRevisin_Click: " + ex.Message);
            }
        }

        protected void btnCopyToMachine_Click(object sender, EventArgs e)
        {
            try
            {

                hfNewRevOrCopyToMc.Value = "CopyToMachine";
                btnCreateNewRev.Visible = false;

                List<string> list = new List<string>();
                foreach (ListItem item in ddlMachine.Items)
                {
                    if (ViewState["Machine"].ToString() != item.Value)
                    {
                        list.Add(item.Value);
                    }

                }
                ddlMachine_RevOrCopyToMc.DataSource = list;
                ddlMachine_RevOrCopyToMc.DataBind();

                tdMachineLbl.Visible = true;
                tdMachineValue.Visible = true;
                ddlMachine_RevOrCopyToMc_SelectedIndexChanged(null, null);
             

                BindJHDetailsFoNewRevOrCopyToMc();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnCopyToMachine_Click: " + ex.Message);
            }
        }
        private void BindJHDetailsFoNewRevOrCopyToMc()
        {
            try
            {
                List<JHMasterDetails> list = BajajDBAccess.GetJHMasterDetails(ViewState["Machine"].ToString(), ViewState["RevNo"].ToString(), (bool)ViewState["isModifiable"]);
                lvRevOrCopyToMcDetails.DataSource = list;
                lvRevOrCopyToMcDetails.DataBind();

                txtRevNo_RevOrCopyToMc.Text = "";
                txtRevDate_RevOrCopyToMc.Text = DateTime.Now.ToString("dd-MM-yyyy");

                HelperClass.openAddEditModal(this, "revOrCopyToMcModal");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindAuditDetailsForNewTemplateOrRev: " + ex.Message);
            }
        }

        protected void btnSaveNewRevOrCopyToMc_Click(object sender, EventArgs e)
        {
            try
            {
                string machine = "", param = "", copytype = "", revNo="" ;

                if (hfNewRevOrCopyToMc.Value == "Revision")
                {
                    machine = ViewState["Machine"].ToString();
                    param = "NewRev";
                    revNo= txtRevNo_RevOrCopyToMc.Text;
                }
                else
                {
                    machine = ddlMachine_RevOrCopyToMc.SelectedValue;
                    param = "CopyFromOneMachineToAnother";
                    if (cbNewRevNo.Checked || !cbNewRevNo.Visible)
                    {
                        copytype = "New";
                        revNo = txtRevNo_RevOrCopyToMc.Text;
                    }
                    else
                    {
                        copytype = "Existing";
                        revNo = ddlRevNo.SelectedItem==null?"": ddlRevNo.SelectedItem.ToString();
                    }
                }

                int j = 0;
                for (int i = 0; i < lvRevOrCopyToMcDetails.Items.Count; i++)
                {
                    if ((lvRevOrCopyToMcDetails.Items[i].FindControl("cbSelect") as CheckBox).Checked)
                    {
                        JHMasterDetails data = new JHMasterDetails();
                        data.Machine = machine;
                        data.OldMachine = ViewState["Machine"].ToString();
                        data.OldRevNo = ViewState["RevNo"].ToString();
                        data.RevNo = revNo;
                        data.RevDate = txtRevDate_RevOrCopyToMc.Text;
                        data.CheckPoint = (lvRevOrCopyToMcDetails.Items[i].FindControl("lblCheckPoint") as Label).Text;
                        data.Frequency = (lvRevOrCopyToMcDetails.Items[i].FindControl("lblFrequency") as Label).Text;
                        data.Param = param;
                        data.CopyType = copytype;
                        if (j == 0)
                        {
                            data.NewOrEdit = "0";

                        }
                        string success = BajajDBAccess.SaveJHMasterDetails(data);
                        if (j == 0)
                        {
                            if (success == "Exist")
                            {
                                HelperClass.openModal(this, "revOrCopyToMcModal", false);
                                HelperClass.openWarningModal(this, "Revision No. already exists.");
                                return;
                            }
                            j++;
                        }
                    }
                }

                if (hfNewRevOrCopyToMc.Value == "Revision")
                {
                    BindMachineRevisionNo();
                    BindJHDetails();
                }

                HelperClass.openInsertSuccessModal(this);
                HelperClass.clearModal(this);

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnSaveNewRevOrCopyToMc_Click: " + ex.Message);
            }
        }
        protected void btnCreateNewRev_Click(object sender, EventArgs e)
        {
            try
            {
                HelperClass.clearModal(this);
                hfFromCreateRevision.Value = "Yes";
                
                btnAddJHMasterDetails_Click(null, null);
                //txtCapNo.Text = "";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnCreateNewRev_Click: " + ex.Message);
            }
        }

        protected void ddlMachine_RevOrCopyToMc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string machine = ddlMachine_RevOrCopyToMc.SelectedValue;
                List<ListItem> list = BajajDBAccess.GetRevisionDetailsForMachine(machine);
                if (list.Count > 0)
                {
                    ddlRevNo_RevOrCopyToMc.DataSource = list;
                    ddlRevNo_RevOrCopyToMc.DataBind();

                    tdExistingRevOrNewRev.Visible = true;

                    cbNewRevNo.Visible = true;
                }
                else
                {
                    tdExistingRevOrNewRev.Visible = false;
                    cbNewRevNo.Visible = false;
                }

                cbNewRevNo.Checked = false;

                cbNewRevNo_CheckedChanged(null, null);

                if (sender != null)
                {
                    HelperClass.openModal(this, "revOrCopyToMcModal", false);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("revOrCopyToMcModal: " + ex.Message);
            }
        }

        protected void cbNewRevNo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbNewRevNo.Checked || !cbNewRevNo.Visible)
                {
                 
                    ddlRevNo_RevOrCopyToMc.Visible = false;
                    txtRevNo_RevOrCopyToMc.Visible = true;
                    tdRevDateLbl.Visible = true;
                    tdRevDateValue.Visible = true;
                }
                else
                {
                    ddlRevNo_RevOrCopyToMc.Visible = true;
                    txtRevNo_RevOrCopyToMc.Visible = false;
                    tdRevDateLbl.Visible = false;
                    tdRevDateValue.Visible = false;
                }

                if (sender != null)
                {
                    HelperClass.openModal(this, "revOrCopyToMcModal", false);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("cbNewRevNo_CheckedChanged: " + ex.Message);
            }
        }
        protected void lbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["DeleteRowIndex"] = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewDataItem).DataItemIndex);
                string parametr = (lvJHMasterDetails.Items[(int)ViewState["DeleteRowIndex"]].FindControl("lblCheckPoint") as Label).Text;
                string deletemsg = "Are you sure, you want to delete Check Point:" + parametr;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmModal", "openDeleteConfirmModal('" + deletemsg + "')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lbDelete_Click: " + ex.Message);
            }
        }

        protected void btnDeleteConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                JHMasterDetails data = new JHMasterDetails();
                data.Machine = ViewState["Machine"].ToString();
                data.RevNo = ViewState["RevNo"].ToString();
                data.CheckPoint = (lvJHMasterDetails.Items[(int)ViewState["DeleteRowIndex"]].FindControl("lblCheckPoint") as Label).Text;
                data.Frequency = (lvJHMasterDetails.Items[(int)ViewState["DeleteRowIndex"]].FindControl("lblFrequency") as Label).Text;
                data.Param = "Delete";
                string result = BajajDBAccess.SaveJHMasterDetails(data);
                if (result == "Deleted")
                {

                    HelperClass.openDeleteSuccessModal(this);
                    HelperClass.clearModal(this);
                }
                else
                {
                    HelperClass.clearModal(this);
                    HelperClass.openDeleteErrorModal(this);
                    return;
                }
                BindJHDetails();
                ViewState["DeleteRowIndex"] = -1;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnDeleteConfirm_Click: " + ex.Message);
            }

        }

        protected void btnAddEditRevLevelData_Click(object sender, EventArgs e)
        {
            try
            {
                List<JHMasterDetails> list = new List<JHMasterDetails>();
                if (Session["MasterData"] != null)
                {
                    list = (List<JHMasterDetails>)Session["MasterData"];

                    string value = list.Select(x => x.Manager).FirstOrDefault();
                    if (ddlManager.Items.FindByValue(value) != null)
                    {
                        ddlManager.SelectedValue = value;
                    }
                    value = list.Select(x => x.GroupLeader).FirstOrDefault();
                    if (ddlGroupLeader.Items.FindByValue(value) != null)
                    {
                        ddlGroupLeader.SelectedValue = value;
                    }
                    HelperClass.openAddEditModal(this, "neweditRevLevelModal");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnAddEditTemplateData_Click: " + ex.Message);
            }
        }

        protected void btnRevLevelDataSave_Click(object sender, EventArgs e)
        {
            try
            {
                JHMasterDetails data = new JHMasterDetails();
                data.Machine = ViewState["Machine"].ToString();
                data.RevNo = ViewState["RevNo"].ToString();
                data.Param = "UpdateManagerGroupLeader";
                data.Manager = ddlManager.SelectedValue;
                data.GroupLeader = ddlGroupLeader.SelectedValue;
                string success = BajajDBAccess.SaveJHMasterDetails(data);
                if (success == "Updated")
                {
                    HelperClass.openUpdateSuccessModal(this);
                    HelperClass.clearModal(this);
                }
                else
                {
                    HelperClass.openModal(this, "neweditRevLevelModal", false);
                    HelperClass.openInsertErrorModal(this);
                    return;
                }
                BindJHDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnTemplateDataSave_Click: " + ex.Message);
            }
        }

        protected void ddlFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string frequency = ddlFrequency.SelectedValue;
                if (frequency == "D")
                {
                    lblDay.Visible = true;
                    ddlDayNo.Visible = false;
                    ddlDay.Visible = false;
                    txtMonth.Visible = false;
                }
                else if (frequency == "W")
                {
                    lblDay.Visible = false;
                    ddlDayNo.Visible = false;
                    ddlDay.Visible = true;
                    txtMonth.Visible = false;
                }
                else if (frequency == "Q")
                {
                    lblDay.Visible = false;
                    ddlDayNo.Visible = true;
                    ddlDay.Visible = true;
                    txtMonth.Visible = true;
                }
                if (sender != null)
                {
                    HelperClass.openModal(this, "neweditJHModal", false);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlFrequency_SelectedIndexChanged: " + ex.Message);
            }
        }


    }
}