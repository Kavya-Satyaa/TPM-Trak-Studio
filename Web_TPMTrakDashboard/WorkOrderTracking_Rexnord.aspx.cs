using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class WorkOrderTracking_Rexnord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindGrid();
            }
        }
        private void BindGrid()
        {
            try
            {
                List<WorkOrderTrackingData_Rexnord> list = DataBaseAccess.GetWorkOrderTrackingData_Rexnord(txtFromDate.Text, txtToDate.Text, txtWorkOrderSearch.Text, txtSerialNoSearch.Text);
                lvGridData.DataSource = list;
                lvGridData.DataBind();
                Session["WorkOrderData"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void lnkClearDate_Click(object sender, EventArgs e)
        {
            try
            {
                txtFromDate.Text = "";
                txtToDate.Text = "";
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
            string Generated = "";
            List<WorkOrderTrackingData_Rexnord> list = new List<WorkOrderTrackingData_Rexnord>();
            try
            {
                if (Session["WorkOrderData"] != null)
                    list = Session["WorkOrderData"] as List<WorkOrderTrackingData_Rexnord>;

                Generated = TMPTrakGenerateReport.GenerateWorkOrderReportRexnord(list, txtFromDate.Text, txtToDate.Text);

                if (Generated.Equals("TemplateNotFound", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openWarningModal(this, "Template Not Found.");
                else if (Generated.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openSuccessModal(this, "Download Successful.");
                else if (Generated.Equals("NoDataFound"))
                    HelperClassGeneric.openErrorModal(this, "ERROR! Try Again.");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "hideDiv", "$.unblockUI({});", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"btnExport_Click: {ex.Message}");
            }
        }

        protected void lvGridData_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    var OperationType = (e.Item.FindControl("hdnOperationType") as HiddenField).Value;
                    if (OperationType.Equals("Manual", StringComparison.OrdinalIgnoreCase))
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}