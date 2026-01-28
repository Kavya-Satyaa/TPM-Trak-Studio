using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class QualityIncomingReport : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                SessionClear.ClearSession();
                BindMachineIDs();
                BindProdOrders();
                BindMaterialIDs();
                BindOperationIDs(ddlMaterialID.SelectedItem != null ? ddlMaterialID.SelectedValue : "");
                BindInsPlanNumbers(ddlMaterialID.SelectedItem != null ? ddlMaterialID.SelectedValue : "", ddlOperationNumber.SelectedItem != null ? ddlOperationNumber.SelectedValue : "");
                if (IsAllParamsLoaded())
                    await LoadQualityIncomingReportData();
            }
        }

        private void BindMachineIDs()
        {
            try
            {
                List<string> listMachineIds = GEADatabaseAccess.GetAllQualityTransactionMachines();
                if (listMachineIds != null && listMachineIds.Count > 0)
                {
                    ddlMachineID.DataSource = listMachineIds;
                    ddlMachineID.DataBind();
                    ddlMachineID.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void BindProdOrders()
        {
            try
            {
                List<string> listProdOrders = GEADatabaseAccess.GetAllQualityTransactionProdOrders();
                if (listProdOrders != null && listProdOrders.Count > 0)
                {
                    ddlPONumber.DataSource = listProdOrders;
                    ddlPONumber.DataBind();
                    ddlPONumber.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void BindMaterialIDs()
        {
            try
            {
                List<string> listMaterialIDs = GEADatabaseAccess.GetAllQualityTransactionMaterialIds();
                if (listMaterialIDs != null && listMaterialIDs.Count > 0)
                {
                    ddlMaterialID.DataSource = listMaterialIDs;
                    ddlMaterialID.DataBind();
                    ddlMaterialID.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void BindOperationIDs(string materialID)
        {
            try
            {
                List<string> listOperationNos = GEADatabaseAccess.GetAllQualityTransactionOpnForMaterialID(materialID);
                if (listOperationNos != null && listOperationNos.Count > 0)
                {
                    ddlOperationNumber.DataSource = listOperationNos;
                    ddlOperationNumber.DataBind();
                    ddlOperationNumber.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void BindInsPlanNumbers(string materialID, string opnNum)
        {
            try
            {
                List<string> listInsPlanNumbers = GEADatabaseAccess.GetAllQualityTransactionPlanNoForMaterialAndOpn(materialID, opnNum);
                if (listInsPlanNumbers != null && listInsPlanNumbers.Count > 0)
                {
                    ddlInsPlanNumber.DataSource = listInsPlanNumbers;
                    ddlInsPlanNumber.DataBind();
                    ddlInsPlanNumber.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private bool IsAllParamsLoaded()
        {
            bool AllParamsAvailable = false;
            if (string.IsNullOrEmpty(ddlMachineID.SelectedItem != null ? ddlMachineID.SelectedValue : ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Machine ID is not available.')", true);
            }
            else if (string.IsNullOrEmpty(ddlPONumber.SelectedItem != null ? ddlPONumber.SelectedValue : ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Prod. Order is not available.')", true);
            }
            else if (string.IsNullOrEmpty(ddlMaterialID.SelectedItem != null ? ddlMaterialID.SelectedValue : ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Material ID is not available.')", true);
            }
            else if (string.IsNullOrEmpty(ddlOperationNumber.SelectedItem != null ? ddlOperationNumber.SelectedValue : ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Operation Number is not available.')", true);
            }
            else if (string.IsNullOrEmpty(ddlInsPlanNumber.SelectedItem != null ? ddlInsPlanNumber.SelectedValue : ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Inspection Plan Number is not available.')", true);
            }
            else
            {
                AllParamsAvailable = true;
            }
            return AllParamsAvailable;
        }

        private void BindQualityIncomingReportData()
        {
            try
            {
                FillQualityIncomingDescription();
                BindQualityIncomingReportGrid();
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void FillQualityIncomingDescription()
        {
            throw new NotImplementedException();
        }

        private void BindQualityIncomingReportGrid()
        {
            throw new NotImplementedException();
        }

        private Task LoadQualityIncomingReportData()
        {
            return Task.Run(() => { BindQualityIncomingReportData(); });
        }
    }
}