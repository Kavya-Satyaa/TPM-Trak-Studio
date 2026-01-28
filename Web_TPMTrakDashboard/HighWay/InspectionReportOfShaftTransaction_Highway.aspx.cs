using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.HighWay
{
    public partial class InspectionReportOfShaftTransaction_Highway : System.Web.UI.Page
    {
        string ProductionHOD = "";
        string QAHOD = "";
        string QAInspector = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachines();
                BindComponents();
                BindOperations();
                BindShift();
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindDieNo();
                BindHeatNo();
                BindRevID();
                BindGrid();
            }
        }
        private void BindGrid()
        {
            try
            {
                string result = "";
                DataTable dt = new DataTable();DataTable dtOperator = new DataTable();
                if (ddlComponentID.SelectedValue.ToString() == "")
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Please select Component ID");
                    return;
                }
                else if (ddlRevID.SelectedValue.ToString() == "")
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Please select Rev ID");
                    return;
                }
                else if (ddlOperationNo.SelectedValue.ToString() == "")
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Please select Operation No");
                    return;
                }
                List<InspectionReportofShaftApprovalData> list = DBAccess.GetInspectionReportTransactionData(ddlMachineID.SelectedValue.ToString(), ddlComponentID.SelectedValue.ToString(), ddlOperationNo.SelectedValue.ToString(), ddlShift.SelectedValue.ToString(), txtFromDate.Text, ddlDieNo.SelectedValue.ToString().Equals("ALL",StringComparison.OrdinalIgnoreCase)?"": ddlDieNo.SelectedValue.ToString(), ddlHeatNo.SelectedValue.ToString().Equals("ALL",StringComparison.OrdinalIgnoreCase)?"": ddlHeatNo.SelectedValue.ToString(), Convert.ToInt32(ddlRevID.SelectedValue.ToString()),out result,out dt,out dtOperator);
                if (!string.IsNullOrEmpty(result))
                    HelperClass.openWarningToastrModal(this, result);
                lvApproveDetails.DataSource = list;
                lvApproveDetails.DataBind();
                Session["InspectionReportData"] = dt;
                Session["InspectionReportOperatorData"] = dtOperator;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindRevID()
        {
            try
            {
                List<ListItem> list = DBAccess.GetRevID(ddlComponentID.SelectedValue.ToString(), ddlOperationNo.SelectedValue.ToString());
                ddlRevID.DataSource = list;
                ddlRevID.DataTextField = "Text";
                ddlRevID.DataValueField = "Value";
                ddlRevID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindMachines()
        {
            try
            {
                List<string> list = DBAccess.GetMachines();
                ddlMachineID.DataSource = list;
                ddlMachineID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindComponents()
        {
            try
            {
                List<string> list = DBAccess.GetComponentID(ddlMachineID.SelectedValue.ToString());
                ddlComponentID.DataSource = list;
                ddlComponentID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindOperations()
        {
            try
            {
                List<string> list = DBAccess.GetOperationIDs(ddlMachineID.SelectedValue.ToString(), ddlComponentID.SelectedValue.ToString());
                ddlOperationNo.DataSource = list;
                ddlOperationNo.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindHeatNo()
        {
            try
            {
                List<string> list = DBAccess.GetHeatNo(ddlMachineID.SelectedValue.ToString(), ddlComponentID.SelectedValue.ToString(), ddlOperationNo.SelectedValue.ToString(), ddlShift.SelectedValue.ToString(), txtFromDate.Text);
                list.Insert(0, "ALL");
                ddlHeatNo.DataSource = list;
                ddlHeatNo.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindDieNo()
        {
            try
            {
                List<string> list = DBAccess.GetDieNo(ddlMachineID.SelectedValue.ToString(), ddlComponentID.SelectedValue.ToString(), ddlOperationNo.SelectedValue.ToString(), ddlShift.SelectedValue.ToString(), txtFromDate.Text);
                list.Insert(0, "ALL");
                ddlDieNo.DataSource = list;
                ddlDieNo.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindShift()
        {
            try
            {
                List<ListItem> list = DBAccess.GetShiftID();
                ddlShift.DataTextField = "Text";
                ddlShift.DataValueField = "Text";
                ddlShift.DataSource = list;
                ddlShift.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlMachineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindComponents();
                BindOperations(); BindDieNo(); BindHeatNo();BindRevID();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlComponentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindOperations(); BindDieNo(); BindHeatNo();BindRevID();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlOperationNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindDieNo(); BindHeatNo();BindRevID();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindDieNo(); BindHeatNo();BindRevID();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = Session["InspectionReportData"] as DataTable;
                DataTable dtOperator = Session["InspectionReportOperatorData"] as DataTable;
                string Generated = HighwayGenerateReports.GenerateInspectionReportOfShaft(dt,dtOperator,ddlComponentID.SelectedValue.ToString(),ddlShift.SelectedValue,txtFromDate.Text,ddlOperationNo.SelectedValue,ddlRevID.SelectedValue,ddlHeatNo.SelectedValue,ddlDieNo.SelectedValue);
                if (Generated == "")
                {
                    HelperClass.openWarningToastrModal(this, "Error in Generating Report");
                }
                else if(Generated== "Generated")
                {
                    HelperClass.openSuccessModal(this);
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnApproveConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string result = DBAccess.SaveInspectionApproveData(ddlMachineID.SelectedValue.ToString(), ddlComponentID.SelectedValue.ToString(), ddlOperationNo.SelectedValue.ToString(), ddlShift.SelectedValue.ToString(),txtFromDate.Text, ddlDieNo.SelectedValue.ToString().Equals("ALL",StringComparison.OrdinalIgnoreCase)?"" : ddlDieNo.SelectedValue.ToString(), ddlHeatNo.SelectedValue.ToString().Equals("ALL",StringComparison.OrdinalIgnoreCase)?"": ddlHeatNo.SelectedValue.ToString(), Session["ProductionHOD"].ToString(), Session["QAHOD"].ToString(), Session["QAInspector"].ToString(),Session["Role"].ToString());
                if (result.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openSuccessModal(this, "Approved");
                    HelperClass.clearModal(this);
                }
                else
                    HelperClassGeneric.openWarningModal(this, string.IsNullOrEmpty(result) ? "ERROR! Try again." : result);
                BindGrid();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnQAApprove_Click(object sender, EventArgs e)
        {
            try
            {
                Session["ProductionHOD"] = "";
                Session["QAHOD"] = Session["UserName"].ToString(); Session["QAInspector"] = ""; Session["Role"] = "QAHOD";
                HelperClass.openAddEditModal(this, "ConfirmModal");
              
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnProductionApprove_Click(object sender, EventArgs e)
        {
            try
            {
                Session["ProductionHOD"] = Session["UserName"].ToString(); Session["Role"] = "ProductionHOD";
                Session["QAHOD"] =""; Session["QAInspector"] = "";
                HelperClass.openAddEditModal(this, "ConfirmModal");
              
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                BindShift();
                BindHeatNo();
                BindDieNo();
                BindRevID();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnInspectorApprove_Click(object sender, EventArgs e)
        {
            try
            {
                Session["ProductionHOD"] = "";
                Session["QAHOD"] = ""; Session["QAInspector"] = Session["UserName"].ToString(); Session["Role"] = "Inspector";
                HelperClass.openAddEditModal(this, "ConfirmModal");
               
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}