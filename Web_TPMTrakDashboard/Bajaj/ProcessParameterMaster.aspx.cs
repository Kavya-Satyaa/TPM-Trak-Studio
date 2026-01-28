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
    public partial class ProcessParameterMaster : System.Web.UI.Page
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

                if (System.Web.Configuration.WebConfigurationManager.AppSettings["BajajPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    if (Session["UserAccessData"] == null)
                        Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"].ToString());
                    else
                        useAccessData = Session["UserAccessData"] as List<ModelClassLibrary.UserAccessModel>;

                    ViewState["isModifiable"] = useAccessData.Where(ss => ss.Code.Equals("PPMasterModification", StringComparison.OrdinalIgnoreCase) & ss.Domain.Equals("Bajaj", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                }
                else
                {
                    ViewState["isModifiable"] = true;
                }

                BindParameterId();
                BindSourceType();
                BindMachine();
                BindPPDetails();
            }
        }

        private void BindMachine()
        {
            try
            {
                List<string> list = new List<string>();
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["AlliedPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    list = DataBaseAccess.GetAllMachinesFromMaster();
                }
                else
                {
                    list = DataBaseAccess.GetAllMachines("");
                }
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachine: " + ex.Message);
            }
        }
        private void BindSourceType()
        {
            try
            {
                List<string> list = BajajDBAccess.GetAllGroupIDs();
                ddlSourceType.DataSource = list;
                ddlSourceType.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindSourceType: " + ex.Message);
            }
        }
        private void BindParameterId()
        {
            try
            {
                List<string> list = BajajDBAccess.GetAllParameterIDs();
                ddlParameterId.DataSource = list;
                ddlParameterId.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindParameterId: " + ex.Message);
            }
        }
        private string GetParameterName()
        {
            string value = "";
            try
            {
                value = BajajDBAccess.GetSelectedParameterName(ddlParameterId.SelectedValue);

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindParameterName: " + ex.Message);
            }
            return value;
        }

        private void BindValues()
        {
            try
            {
                List<string> list = new List<string>();
                list.Add("string");
                list.Add("Numeric");
                list.Add("bool");
                ddlDBDataType.DataSource = list;
                ddlDBDataType.DataBind();

                list = new List<string> { "500 ms", "1 Sec", "1 Min", "2 Min", "5 Min", "10 Min", "15 Min", "20 Min", "1 Hr" };
                ddlFrequency.DataSource = list;
                ddlFrequency.DataBind();

                list = new List<string> { "Real", "bool", "byte", "short", "int", "none" };
                ddlSourceDataType.DataSource = list;
                ddlSourceDataType.DataBind();

                list = new List<string> { "DuringCycle", "continuous" };
                ddlPollingType.DataSource = list;
                ddlPollingType.DataBind();

                list = new List<string> { "Text", "High/Low" };
                ddlTemplateType.DataSource = list;
                ddlTemplateType.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindValues: " + ex.Message);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindPPDetails();
        }
        private void BindPPDetails()
        {
            try
            {
                ViewState["Machine"] = ddlMachine.SelectedValue;
                List<ProcessParameterMasterDetails> list = BajajDBAccess.GetProcessParameterMasterDetails(ViewState["Machine"].ToString(), (bool)ViewState["isModifiable"]);

                lvPPMasterDetails.DataSource = list;
                lvPPMasterDetails.DataBind();

                if ((bool)ViewState["isModifiable"])
                {
                    btnAddPPMasterDetails.Visible = true;
                }
                else
                {
                    btnAddPPMasterDetails.Visible = false;
                }

                //string ctype = BajajDBAccess.GetControllerType(ViewState["Machine"].ToString());
                //if (ctype == "SIEMENS")
                //{
                txtParameterId.Visible = true;
                ddlParameterId.Visible = false;
                //}
                //else
                //{
                //    txtParameterId.Visible = false;
                //    ddlParameterId.Visible = true;
                //}

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPPDetails: " + ex.Message);
            }
        }
        protected void lvPPMasterDetails_DataBound(object sender, EventArgs e)
        {
            try
            {
                HelperClass.ShowHideActionColumn(lvPPMasterDetails, (bool)ViewState["isModifiable"]);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lvPPMasterDetails_ItemDataBound: " + ex.Message);
            }

        }
        protected void btnAddPPMasterDetails_Click(object sender, EventArgs e)
        {
            try
            {
                txtParameterId.Text = "";
                txtDisplayText.Text = "";
                txtDataReadAddress.Text = "";
                txtHighRedLimit.Text = "";
                txtLowRedLimit.Text = "";
                txtHighGreenLimit.Text = "";
                txtLowGreenLimit.Text = "";
                txtUnit.Text = "";
                txtSortOrder.Text = "";
                cbIsVisible.Checked = true;
                cbIsDashboardVisible.Checked = false;
                cbIsGraphVisible.Checked = false;
                cbIsMobileVisible.Checked = false;
                cbIsAlertVisible.Checked = false;
                hfAutoID.Value = "";

                BindValues();

                txtParameterId.Enabled = true;
                ddlParameterId.Enabled = true;

                hfPPNewEdit.Value = "New";
                btnPPDetailsSave.Text = "ADD";
                modalTitle.InnerText = "Add Process Parameter Details";

                HelperClass.openAddEditModal(this, "neweditPPModal");
                HelperClass.openFunction(this, "TemplateTypeChange");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnAddPPMasterDetails_Click: " + ex.Message);
            }
        }

        protected void lbEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewDataItem).DataItemIndex);
                string parameterid = (lvPPMasterDetails.Items[rowIndex].FindControl("lblParameterId") as Label).Text;
                if (txtParameterId.Visible)
                {
                    txtParameterId.Text = parameterid;
                }
                else
                {
                    if (ddlParameterId.Items.FindByValue(parameterid) != null)
                    {
                        ddlParameterId.SelectedValue = parameterid;
                    }
                }
                txtDisplayText.Text = (lvPPMasterDetails.Items[rowIndex].FindControl("lblDisplayText") as Label).Text;
                txtDataReadAddress.Text = (lvPPMasterDetails.Items[rowIndex].FindControl("lblDataReadAddress") as Label).Text;
                txtHighRedLimit.Text = (lvPPMasterDetails.Items[rowIndex].FindControl("lblHighRedLimit") as Label).Text;
                txtLowRedLimit.Text = (lvPPMasterDetails.Items[rowIndex].FindControl("lblLowRedLimit") as Label).Text;
                txtHighGreenLimit.Text = (lvPPMasterDetails.Items[rowIndex].FindControl("lblHighGreenLimit") as Label).Text;
                txtLowGreenLimit.Text = (lvPPMasterDetails.Items[rowIndex].FindControl("lblLowGreenLimit") as Label).Text;
                txtUnit.Text = (lvPPMasterDetails.Items[rowIndex].FindControl("lblUnit") as Label).Text;
                cbIsVisible.Checked = (lvPPMasterDetails.Items[rowIndex].FindControl("cbIsVisible") as CheckBox).Checked;
                cbIsDashboardVisible.Checked = (lvPPMasterDetails.Items[rowIndex].FindControl("cbIsDashboardVisible") as CheckBox).Checked;
                cbIsGraphVisible.Checked = (lvPPMasterDetails.Items[rowIndex].FindControl("cbIsGraphVisible") as CheckBox).Checked;
                cbIsMobileVisible.Checked = (lvPPMasterDetails.Items[rowIndex].FindControl("cbIsMobileVisible") as CheckBox).Checked;
                cbIsAlertVisible.Checked = (lvPPMasterDetails.Items[rowIndex].FindControl("cbIsAlertVisible") as CheckBox).Checked;
                txtSortOrder.Text = (lvPPMasterDetails.Items[rowIndex].FindControl("lblSortOrder") as Label).Text;
                hfAutoID.Value = (lvPPMasterDetails.Items[rowIndex].FindControl("hfAutoID") as HiddenField).Value;

                BindValues();


                string value = (lvPPMasterDetails.Items[rowIndex].FindControl("lblSourceType") as Label).Text;
                if (ddlSourceType.Items.FindByValue(value) != null)
                {
                    ddlSourceType.SelectedValue = value;
                }
                value = (lvPPMasterDetails.Items[rowIndex].FindControl("lblSourceDataType") as Label).Text;
                if (ddlSourceDataType.Items.FindByValue(value) != null)
                {
                    ddlSourceDataType.SelectedValue = value;
                }
                value = (lvPPMasterDetails.Items[rowIndex].FindControl("lblPollingType") as Label).Text;
                if (ddlPollingType.Items.FindByValue(value) != null)
                {
                    ddlPollingType.SelectedValue = value;
                }
                value = (lvPPMasterDetails.Items[rowIndex].FindControl("lblFrequency") as Label).Text;
                if (ddlFrequency.Items.FindByValue(value) != null)
                {
                    ddlFrequency.SelectedValue = value;
                }
                value = (lvPPMasterDetails.Items[rowIndex].FindControl("lblTemplateType") as Label).Text;
                if (ddlTemplateType.Items.FindByValue(value) != null)
                {
                    ddlTemplateType.SelectedValue = value;
                }
                value = (lvPPMasterDetails.Items[rowIndex].FindControl("lblDBDataType") as Label).Text;
                if (ddlDBDataType.Items.FindByValue(value) != null)
                {
                    ddlDBDataType.SelectedValue = value;
                }

                txtParameterId.Enabled = false;
                ddlParameterId.Enabled = false;

                hfPPNewEdit.Value = "Edit";
                btnPPDetailsSave.Text = "UPDATE";
                modalTitle.InnerText = "Edit Process Parameter Details";

                HelperClass.openAddEditModal(this, "neweditPPModal");
                HelperClass.openFunction(this, "TemplateTypeChange");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lbEdit_Click: " + ex.Message);
            }
        }

        protected void btnPPDetailsSave_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessParameterMasterDetails data = new ProcessParameterMasterDetails();
                data.IDD = hfAutoID.Value;
                data.MachineId = ViewState["Machine"].ToString();
                if (txtParameterId.Visible)
                {
                    data.ParameterId = txtParameterId.Text;
                    data.ParameterName = data.ParameterId;
                }
                else
                {
                    data.ParameterId = ddlParameterId.SelectedValue;
                    data.ParameterName = GetParameterName();
                }

                data.DisplayText = txtDisplayText.Text;
                data.SourceType = ddlSourceType.SelectedValue;
                data.DataReadAddress = txtDataReadAddress.Text;
                data.SourceDataType = ddlSourceDataType.SelectedValue;
                data.PollingType = ddlPollingType.SelectedValue;
                data.Frequency = ddlFrequency.SelectedValue;
                data.HighRedLimit = txtHighRedLimit.Text;
                data.LowRedLimit = txtLowRedLimit.Text;
                data.HighGreenLimit = txtHighGreenLimit.Text;
                data.LowGreenLimit = txtLowGreenLimit.Text;
                data.Unit = txtUnit.Text;
                data.TemplateType = ddlTemplateType.SelectedValue;
                data.DBDataType = ddlDBDataType.SelectedValue;
                data.IsVisible = cbIsVisible.Checked;
                data.IsDashboardVisible = cbIsDashboardVisible.Checked;
                data.IsGraphVisible = cbIsGraphVisible.Checked;
                data.IsMobileVisible = cbIsMobileVisible.Checked;
                data.IsAlertVisible = cbIsAlertVisible.Checked;
                data.SortOrder = string.IsNullOrEmpty(txtSortOrder.Text) ? 0 : Convert.ToInt32(txtSortOrder.Text);
                data.NewOrEdit = hfPPNewEdit.Value;
                string success = BajajDBAccess.SaveProcessParameterMasteDetails(data);
                if (success == "Exist")
                {
                    HelperClass.openModal(this, "neweditPPModal", false);
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
                    HelperClass.openModal(this, "neweditPPModal", false);
                    HelperClass.openInsertErrorModal(this);
                    return;
                }
                BindPPDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnPPDetailsSave_Click: " + ex.Message);
            }
        }

        protected void lbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["DeleteRowIndex"] = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewDataItem).DataItemIndex);
                string parametr = (lvPPMasterDetails.Items[(int)ViewState["DeleteRowIndex"]].FindControl("lblParameterId") as Label).Text;
                string deletemsg = "Are you sure, you want to delete Parameter Id:" + parametr;
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
                string idd = (lvPPMasterDetails.Items[(int)ViewState["DeleteRowIndex"]].FindControl("hfAutoID") as HiddenField).Value;
                bool result;
                BajajDBAccess.DeleteProcessParameterMasterData(idd, out result);
                if (result)
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
                BindPPDetails();
                ViewState["DeleteRowIndex"] = -1;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnDeleteConfirm_Click: " + ex.Message);
            }
        }
    }
}