using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.TAFE
{
    public partial class PDIMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachine();
                BindPDIDetails();
            }
        }

        private void BindMachine()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllMachines("");
                //var list1 = list.Where(x => list.Any(y=>y.Contains("MGTL") || list.Contains("VTL")).ToList();
                var list1 = list.Where(x => x.Contains("MGTL") || x.Contains("VTL")).ToList();
                ddlMachine.DataSource = list1;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachine: " + ex.Message);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindPDIDetails();
        }
        private void BindPDIDetails()
        {
            try
            {
                ViewState["Machine"] = ddlMachine.SelectedValue;
                List<PDIDetails> list = TafeDataBaseAccess.GetPDIMasterDetails(ViewState["Machine"].ToString());

                lvPDIMasterDetails.DataSource = list;
                lvPDIMasterDetails.DataBind();

                List<ListItem> componentDetails = TafeDataBaseAccess.GetComponentMachine(ViewState["Machine"].ToString());
                Session["PDIComponentDetailsForMachine"] = componentDetails;
                ddlPartNo.DataSource = componentDetails;
                ddlPartNo.DataTextField = "Value";
                ddlPartNo.DataValueField = "Value";
                ddlPartNo.DataBind();

                ddlPartNo_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPDIDetails: " + ex.Message);
            }
        }
        protected void btnAddPDIMasterDetails_Click(object sender, EventArgs e)
        {
            try
            {
                //txtPartNo.Text = "";
                //txtPartDesc.Text = "";
                txtPartName.Text = "";
                txtPartType.Text = "";
                //  txtOperationNo.Text = "";
                txtMaterial.Text = "";
                txtIssuedNo.Text = "";
                txtDocNo.Text = "";
                txtVersion.Text = "";
                hfImage.Value = "";
                hfImageName.Value = "";
                lblImageName.Text = "";
                clearImage.Visible = false;

                ddlPartNo.Enabled = true;
                ddlOperationNo.Enabled = true;
                txtVersion.Enabled = true;
                ddlType.Enabled = true;

                hfPDINewEdit.Value = "New";
                btnPDIDetailsSave.Text = "ADD";
                modalTitle.InnerText = "Add PDI Details";

                HelperClass.openAddEditModal(this, "neweditPDIModal");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnAddPDIMasterDetails_Click: " + ex.Message);
            }
        }

        protected void lbEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewDataItem).DataItemIndex);
                string value = (lvPDIMasterDetails.Items[rowIndex].FindControl("lblPartNo") as Label).Text;
                if (ddlPartNo.Items.FindByValue(value) != null)
                {
                    ddlPartNo.SelectedValue = value;
                }
                //txtPartDesc.Text = (lvPDIMasterDetails.Items[rowIndex].FindControl("lblPartDesc") as Label).Text;
                ddlPartNo_SelectedIndexChanged(null, null);
                txtPartName.Text = (lvPDIMasterDetails.Items[rowIndex].FindControl("lblPartName") as Label).Text;
                txtPartType.Text = (lvPDIMasterDetails.Items[rowIndex].FindControl("lblPartType") as Label).Text;
                value = (lvPDIMasterDetails.Items[rowIndex].FindControl("lblOperationNo") as Label).Text;
                if (ddlOperationNo.Items.FindByValue(value) != null)
                {
                    ddlOperationNo.SelectedValue = value;
                }
                txtMaterial.Text = (lvPDIMasterDetails.Items[rowIndex].FindControl("lblMaterial") as Label).Text;
                txtIssuedNo.Text = (lvPDIMasterDetails.Items[rowIndex].FindControl("lblIssuedNo") as Label).Text;
                txtDocNo.Text = (lvPDIMasterDetails.Items[rowIndex].FindControl("lblDocNo") as Label).Text;
                txtVersion.Text = (lvPDIMasterDetails.Items[rowIndex].FindControl("lblVersion") as Label).Text;
                value = (lvPDIMasterDetails.Items[rowIndex].FindControl("lblType") as Label).Text;
                if (ddlType.Items.FindByValue(value) != null)
                {
                    ddlType.SelectedValue = value;
                }
                hfImage.Value = (lvPDIMasterDetails.Items[rowIndex].FindControl("hfImageInBase64") as HiddenField).Value;
                hfImageName.Value = (lvPDIMasterDetails.Items[rowIndex].FindControl("lbImage") as LinkButton).Text;
                lblImageName.Text = hfImageName.Value;
                if (lblImageName.Text != "")
                {
                    clearImage.Visible = true;
                }
                else
                {
                    clearImage.Visible = false;
                }

                ddlPartNo.Enabled = false;
                ddlOperationNo.Enabled = false;
                txtVersion.Enabled = false;
                ddlType.Enabled = false;

                hfPDINewEdit.Value = "Edit";
                btnPDIDetailsSave.Text = "UPDATE";
                modalTitle.InnerText = "Edit PDI Details";

                HelperClass.openAddEditModal(this, "neweditPDIModal");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lbEdit_Click: " + ex.Message);
            }
        }

        protected void btnPDIDetailsSave_Click(object sender, EventArgs e)
        {
            try
            {

                PDIDetails data = new PDIDetails();
                data.MachineID = ViewState["Machine"].ToString();
                data.PartNo = ddlPartNo.SelectedValue;
                data.PartDesc = txtPartDesc.Text;
                data.PartName = txtPartName.Text;
                data.PartType = txtPartType.Text;
                data.OperationNo = ddlOperationNo.SelectedValue;
                data.Material = txtMaterial.Text;
                data.IssuedNo = txtIssuedNo.Text;
                data.DocNo = txtDocNo.Text;
                data.Version = txtVersion.Text;
                data.Type = ddlType.SelectedValue;
                data.NewOrEdit = hfPDINewEdit.Value;
                if (!string.IsNullOrEmpty(hfImage.Value))
                {
                    string image = hfImage.Value;
                    byte[] imageinbytes = System.Convert.FromBase64String(image.Substring(image.LastIndexOf(',') + 1));
                    data.Image = imageinbytes;
                    data.ImageName = hfImageName.Value;
                }
                data.Param = "Save";
                string success = TafeDataBaseAccess.SavePDIMasteDetails(data);
                if (success == "Exist")
                {
                    HelperClass.openModal(this, "neweditPDIModal", false);
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
                    HelperClass.openModal(this, "neweditPDIModal", false);
                    HelperClass.openInsertErrorModal(this);
                    return;
                }
                BindPDIDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnPDIDetailsSave_Click: " + ex.Message);
            }
        }
        protected void ddlPartNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                List<ListItem> componentDetails = new List<ListItem>();
                if (Session["PDIComponentDetailsForMachine"] == null)
                {
                    componentDetails = TafeDataBaseAccess.GetComponentMachine(ViewState["Machine"].ToString());
                }
                else
                {
                    componentDetails = (List<ListItem>)Session["PDIComponentDetailsForMachine"];
                }

                txtPartDesc.Text = componentDetails.Where(x => x.Value == ddlPartNo.SelectedValue).Select(x => x.Text).FirstOrDefault();

                BindOperationDetails();
                if (sender != null)
                {
                    HelperClass.openModal(this, "neweditPDIModal", false);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlPartNo_SelectedIndexChanged: " + ex.Message);
            }
        }
        private void BindOperationDetails()
        {
            try
            {
                List<string> list = DataBaseAccess.GetOperation(ViewState["Machine"].ToString(), ddlPartNo.SelectedValue);
                ddlOperationNo.DataSource = list;
                ddlOperationNo.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindOperationDetails: " + ex.Message);
            }
        }
        protected void lbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["DeleteRowIndex"] = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewDataItem).DataItemIndex);
                string parametr = (lvPDIMasterDetails.Items[(int)ViewState["DeleteRowIndex"]].FindControl("lblPartNo") as Label).Text;
                string deletemsg = "Are you sure, you want to delete Part No.:" + parametr;
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
                PDIDetails data = new PDIDetails();
                int rowIndex = (int)ViewState["DeleteRowIndex"];
                data.MachineID = ViewState["Machine"].ToString();
                data.PartNo = (lvPDIMasterDetails.Items[rowIndex].FindControl("lblPartNo") as Label).Text;
                data.OperationNo = (lvPDIMasterDetails.Items[rowIndex].FindControl("lblOperationNo") as Label).Text;
                data.Version = (lvPDIMasterDetails.Items[rowIndex].FindControl("lblVersion") as Label).Text;
                data.Type = (lvPDIMasterDetails.Items[rowIndex].FindControl("lblType") as Label).Text;
                data.Param = "Delete";
                string success = TafeDataBaseAccess.SavePDIMasteDetails(data);
                if (success == "Deleted")
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
                BindPDIDetails();
                ViewState["DeleteRowIndex"] = -1;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnDeleteConfirm_Click: " + ex.Message);
            }
        }


    }
}