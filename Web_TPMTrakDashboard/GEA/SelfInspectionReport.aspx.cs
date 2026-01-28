using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;
using Web_TPMTrakDashboard.GEA.Models;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class SelfInspectionReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindProductionOrderNos();
                BindOperationNos();
                BindSelfInspectionData();
            }
        }
        private void BindProductionOrderNos()
        {
            try
            {
                List<string> listPONo = GEADatabaseAccess.GetProductionNumber(txtprodOrder.Text,txtCompSearch.Text);
                if (listPONo != null && listPONo.Count > 0)
                {
                    ddlPONumber.DataSource = listPONo;
                    ddlPONumber.DataBind();
                    ddlPONumber.SelectedIndex = 0;
                }
                else
                {
                    ddlPONumber.DataSource = new List<string>();
                    ddlPONumber.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }
        private void BindOperationNos()
        {
            try
            {
                string PONo = ddlPONumber.SelectedItem != null ? ddlPONumber.SelectedValue : "";
                if (!string.IsNullOrEmpty(PONo))
                {
                    //if (LineId.Equals("All", StringComparison.OrdinalIgnoreCase)) LineId = "";
                    List<string> listOperationNo = GEADatabaseAccess.GetAllOperationNoByPONo(PONo);
                    if (listOperationNo != null && listOperationNo.Count > 0)
                    {
                        ddlOperationNumber.DataSource = listOperationNo;
                        ddlOperationNumber.DataBind();
                        ddlOperationNumber.SelectedIndex = 0;
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }
        private void BindSelfInspectionData()
        {
            try
            {
                string PONo = ddlPONumber.SelectedItem != null ? ddlPONumber.SelectedValue : "";
                string OperationNo = ddlOperationNumber.SelectedItem != null ? ddlOperationNumber.SelectedValue : "";
                if (!string.IsNullOrEmpty(PONo) && !string.IsNullOrEmpty(OperationNo))
                {
                    List<SelfInspectionDetails> descriptionList = new List<SelfInspectionDetails>();
                    List<SelfInspectionDetails> gridList = new List<SelfInspectionDetails>();
                    gridList = GEADatabaseAccess.GetSelfInpectionReportData(PONo, OperationNo, out descriptionList);
                    if (descriptionList != null && descriptionList.Count > 0)
                    {
                        lblDescription.Text = descriptionList.Select(x => x.Description).First();
                        lblPartNo.Text = descriptionList.Select(x => x.PartNumber).First();
                        lblPONo.Text = descriptionList.Select(x => x.ProductionOrder).First();
                        lblMachineId.Text = descriptionList.Select(x => x.MachineID).First();
                        lblPalnNo.Text = descriptionList.Select(x => x.PlanNumber).First();
                        lblDrawingNo.Text = descriptionList.Select(x => x.DrawingNumber).First();
                        lblSerialNo.Text = descriptionList.Select(x => x.SerailNumber).First();
                        lblOpeartionNo.Text = descriptionList.Select(x => x.OperationNumber).First();
                        tblSelfInspectionDescription.Visible = true;
                        Session["SelfInspectionReportDescriptionDetails"] = descriptionList;
                    }
                    else
                    {
                        lblDescription.Text = "";
                        lblPartNo.Text = "";
                        lblPONo.Text = "";
                        lblMachineId.Text = "";
                        lblPalnNo.Text = "";
                        lblDrawingNo.Text = "";
                        lblSerialNo.Text = "";
                        lblOpeartionNo.Text = "";
                        tblSelfInspectionDescription.Visible = false;
                        Session["SelfInspectionReportDescriptionDetails"] = null;
                    }
                    if (gridList != null && gridList.Count > 0)
                    {
                        GridSelfInspectionReport.DataSource = gridList;
                        GridSelfInspectionReport.DataBind();
                        Session["SelfInspectionReportData"] = gridList;
                    }
                    else
                    {
                        GridSelfInspectionReport.DataSource = null;
                        GridSelfInspectionReport.DataBind();
                        Session["SelfInspectionReportData"] = null;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }


        }
        protected void Export_Click(object sender, EventArgs e)
        {

            bool successfull = false;
            List<SelfInspectionDetails> descriptionList = new List<SelfInspectionDetails>();
            List<SelfInspectionDetails> gridList = new List<SelfInspectionDetails>();
            try
            {
                string PONo = ddlPONumber.SelectedItem != null ? ddlPONumber.SelectedValue : "";
                string OperationNo = ddlOperationNumber.SelectedItem != null ? ddlOperationNumber.SelectedValue : "";
                if (Session["SelfInspectionReportDescriptionDetails"] != null && Session["SelfInspectionReportData"] != null)
                {
                    descriptionList = (List<SelfInspectionDetails>)Session["SelfInspectionReportDescriptionDetails"];
                    gridList = (List<SelfInspectionDetails>)Session["SelfInspectionReportData"];
                }
                else
                {
                    if (!string.IsNullOrEmpty(PONo) && !string.IsNullOrEmpty(OperationNo))
                    {

                        gridList = GEADatabaseAccess.GetSelfInpectionReportData(PONo, OperationNo, out descriptionList);
                    }
                }
                if ((descriptionList != null && descriptionList.Count > 0) && (gridList != null && gridList.Count > 0))
                {
                    Session["SelfInspectionReportDescriptionDetails"] = descriptionList;
                    Session["SelfInspectionReportData"] = gridList;
                    successfull = GEAGenerateReport.GenerateSelfInspectionReport(descriptionList, gridList);
                    if (successfull)
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('Export Successful.')", true);
                    else
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('Error. Export Unsuccessful.')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('No data to export.')", true);
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.DarkRed;
                lblMessages.Text = ex.Message;
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindSelfInspectionData();
        }

        protected void ddlPONumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOperationNos();
        }

        protected void btnComponent_Click(object sender, EventArgs e)
        {
            BindProductionOrderNos();
            BindOperationNos();
        }
    }
}