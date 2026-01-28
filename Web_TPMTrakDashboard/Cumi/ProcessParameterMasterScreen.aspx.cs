using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Cumi.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Cumi
{
    public partial class ProcessParameterMasterScreen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachine();
                BindPPDetails();
            }
        }
        private void BindComponent()
        {
            try
            {
                //List<string> list = DataBaseAccess.GetAllComp(ddlMachine.SelectedValue);
                //ddlComponent.DataSource = list;
                //ddlComponent.DataBind();

                
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindComponent: " + ex.Message);
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllMachines("");
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();

              //  BindComponent();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachine: " + ex.Message);
            }
        }
        private void BindValues()
        {
            try
            {
                List<string> list = new List<string>();

                list = new List<string> { "Real","int", "none" };
                ddlSourceDataType.DataSource = list;
                ddlSourceDataType.DataBind();

                list = new List<string> { "1 Value", "1 Value With High/Low", "2 Value", "2 Value With High/Low" };
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
                //ViewState["Component"] = ddlComponent.SelectedValue;
                //ViewState["Component"].ToString()
                List<ProcessParameterMasterDetails> list = CumiDBAccess.GetProcessParameterMasterDetails(ViewState["Machine"].ToString(),"" );

                lvPPMasterDetails.DataSource = list;
                lvPPMasterDetails.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPPDetails: " + ex.Message);
            }
        }
        protected void btnAddPPMasterDetails_Click(object sender, EventArgs e)
        {
            try
            {
                txtParameterId.Text = "";
                txtDisplayText.Text = "";
                txtLowValue.Text = "";
                txtHighValue.Text = "";
                txtUnit.Text = "";
                txtParameterRegister.Text = "";
                txtMaxRegister.Text = "";
                txtMinRegister.Text = "";
                txtSortOrder.Text = "";
                cbIsVisible.Checked = true;
                hfAutoID.Value = "";

                if (ddlChartType.Items.Count > 0)
                {
                    ddlChartType.SelectedIndex = 0;
                }

                BindValues();

                txtParameterId.Enabled = true;

                hfPPNewEdit.Value = "New";
                btnPPDetailsSave.Text = "ADD";
                modalTitle.InnerText = "Add Process Parameter Details";

                Bajaj.Model.HelperClass.openAddEditModal(this, "neweditPPModal");
                Bajaj.Model.HelperClass.openFunction(this, "TemplateTypeChange");
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
                txtParameterId.Text = (lvPPMasterDetails.Items[rowIndex].FindControl("lblParameterId") as Label).Text;
                txtParameterRegister.Text = (lvPPMasterDetails.Items[rowIndex].FindControl("lblParameterRegister") as Label).Text;
                txtDisplayText.Text = (lvPPMasterDetails.Items[rowIndex].FindControl("lblDisplayText") as Label).Text;
                txtUnit.Text = (lvPPMasterDetails.Items[rowIndex].FindControl("lblUnit") as Label).Text;
                txtLowValue.Text = (lvPPMasterDetails.Items[rowIndex].FindControl("lblLowerValue") as Label).Text;
                txtHighValue.Text = (lvPPMasterDetails.Items[rowIndex].FindControl("lblHigherValue") as Label).Text;
                txtMinRegister.Text = (lvPPMasterDetails.Items[rowIndex].FindControl("lblMinRegister") as Label).Text;
                txtMaxRegister.Text = (lvPPMasterDetails.Items[rowIndex].FindControl("lblMaxRegister") as Label).Text;
                cbIsVisible.Checked = (lvPPMasterDetails.Items[rowIndex].FindControl("cbIsVisible") as CheckBox).Checked;
                txtSortOrder.Text = (lvPPMasterDetails.Items[rowIndex].FindControl("lblSortOrder") as Label).Text;
                hfAutoID.Value = (lvPPMasterDetails.Items[rowIndex].FindControl("hfAutoID") as HiddenField).Value;

                BindValues();

                string value = (lvPPMasterDetails.Items[rowIndex].FindControl("lblSourceDataType") as Label).Text;
                if (ddlSourceDataType.Items.FindByValue(value) != null)
                {
                    ddlSourceDataType.SelectedValue = value;
                }
                value = (lvPPMasterDetails.Items[rowIndex].FindControl("lblTemplateType") as Label).Text;
                if (ddlTemplateType.Items.FindByValue(value) != null)
                {
                    ddlTemplateType.SelectedValue = value;
                }
                value = (lvPPMasterDetails.Items[rowIndex].FindControl("lblChartType") as Label).Text;
                if (ddlChartType.Items.FindByValue(value) != null)
                {
                    ddlChartType.SelectedValue = value;
                }

                txtParameterId.Enabled = false;

                hfPPNewEdit.Value = "Edit";
                btnPPDetailsSave.Text = "UPDATE";
                modalTitle.InnerText = "Edit Process Parameter Details";

                Bajaj.Model.HelperClass.openAddEditModal(this, "neweditPPModal");
                Bajaj.Model.HelperClass.openFunction(this, "TemplateTypeChange");
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
                //data.ComponentId = ViewState["Component"].ToString();
                data.ParameterId = txtParameterId.Text;
                data.ParameterName = data.ParameterId;
                data.DisplayText = txtDisplayText.Text;
                data.ParameterRegister = txtParameterRegister.Text;
                data.SourceDataType = ddlSourceDataType.SelectedValue;
                data.Unit = txtUnit.Text;
                data.MinRegister = txtMinRegister.Text;
                data.MaxRegister = txtMaxRegister.Text;
                data.LowerValue = txtLowValue.Text;
                data.HigherValue = txtHighValue.Text;
                data.TemplateType = ddlTemplateType.SelectedValue;
                data.ChartType = ddlChartType.SelectedValue;
                data.IsVisible = cbIsVisible.Checked;
                data.SortOrder = string.IsNullOrEmpty(txtSortOrder.Text) ? 0 : Convert.ToInt32(txtSortOrder.Text);
                data.NewOrEdit = hfPPNewEdit.Value;
                string success = CumiDBAccess.SaveProcessParameterMasteDetails(data);
                if (success == "Exist")
                {
                    Bajaj.Model.HelperClass.openModal(this, "neweditPPModal", false);
                    Bajaj.Model.HelperClass.openWarningModal(this, "Record already exists.");
                    return;
                }
                else if (success == "Inserted")
                {
                    Bajaj.Model.HelperClass.openInsertSuccessModal(this);
                    Bajaj.Model.HelperClass.clearModal(this);
                }
                else if (success == "Updated")
                {

                    Bajaj.Model.HelperClass.openUpdateSuccessModal(this);
                    Bajaj.Model.HelperClass.clearModal(this);
                }
                else
                {
                    Bajaj.Model.HelperClass.openModal(this, "neweditPPModal", false);
                    Bajaj.Model.HelperClass.openInsertErrorModal(this);
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
                CumiDBAccess.DeleteProcessParameterMasterData(idd, out result);
                if (result)
                {

                    Bajaj.Model.HelperClass.openDeleteSuccessModal(this);
                    Bajaj.Model.HelperClass.clearModal(this);
                }
                else
                {
                    Bajaj.Model.HelperClass.clearModal(this);
                    Bajaj.Model.HelperClass.openDeleteErrorModal(this);
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

        protected void ddlMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindComponent();
        }
    }
}