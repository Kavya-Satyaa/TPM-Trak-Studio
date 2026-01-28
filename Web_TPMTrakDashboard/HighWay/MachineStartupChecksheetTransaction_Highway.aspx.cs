using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.HighWay
{
    public partial class MachineStartupChecksheetTransaction_Highway : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtYear.Text = DateTime.Now.ToString("yyyy");
                txtMonth.Text = DateTime.Now.ToString("MMM");
                BindMachines();
                BindComponents();
                BindOperations();
                BindGrid();
            }
        }
        private void BindGrid()
        {
            try
            {
                DataTable dt = new DataTable(); List<string> shiftList = new List<string>();
                List<string> daylist = new List<string>(); DataTable dtOperator = new DataTable();
                DataTable dtSupervisor = new DataTable();
                List<MachineStartupApprovalData> list = DBAccess.GetMachineStartupTransactionData(ddlMachineID.SelectedValue.ToString(), lblComponent.Text, lblOperation.Text, txtYear.Text, txtMonth.Text, out dt, out string Message,out daylist,out shiftList,out dtOperator,out dtSupervisor);
                if (Message.Trim().Equals("NoRecordsFound", StringComparison.OrdinalIgnoreCase))
                    HelperClass.openWarningToastrModal(this, "No Data Found");
                lvApprovalGrid.DataSource = list;
                lvApprovalGrid.DataBind();
                Session["MachineApprovalData"] = dt;
                Session["dtOperator"] = dtOperator;
                Session["dtSupervisor"] = dtSupervisor;
                Session["DayList"] = daylist;
                Session["ShiftList"] = shiftList;
            }
            catch(Exception ex)
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
                List<string> list = DBAccess.GetComponents_Machine(ddlMachineID.SelectedValue.ToString());
                if (list.Count > 0)
                {
                    lblComponent.Text = list[0].ToString();
                }
                else
                {
                    lblComponent.Text = "";
                }
                //ddlComponentID.DataSource = list;
                //ddlComponentID.DataBind();
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
                List<string> list = DBAccess.GetOperations_Machine(ddlMachineID.SelectedValue.ToString(), lblComponent.Text);
                lblOperation.Text =list.Count>0? list[0].ToString():"";
                //ddlOperationNo.DataSource = list;
                //ddlOperationNo.DataBind();
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
                BindOperations();
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
                BindOperations();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = Session["MachineApprovalData"] as DataTable;
                List<string> daylist= Session["DayList"] as List<string>;
                List<string> shiftList = Session["ShiftList"] as List<string>;
                int year = Convert.ToInt32(txtYear.Text);
                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                int monthNumber = DateTime.ParseExact(txtMonth.Text, "MMM", CultureInfo.InvariantCulture).Month;
                DataTable dtOperator = Session["dtOperator"] as DataTable;
                DataTable dtSupervisor = Session["dtSupervisor"] as DataTable;
                string Generated = HighwayGenerateReports.GenerateMachineStartUpReport(dt,dtOperator,dtSupervisor,daylist,shiftList, ddlMachineID.SelectedValue.ToString(), lblComponent.Text, lblOperation.Text, year, monthNumber);
                if (Generated == "")
                {
                    HelperClass.openWarningToastrModal(this, "Error in Generating Report");
                }
                else if (Generated == "Generated")
                {
                    HelperClass.openSuccessModal(this);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                Session["Date"] = ((sender as Button).Parent.FindControl("hdndate") as HiddenField).Value;
                Session["Shift"] = ((sender as Button).Parent.FindControl("hdnValue") as HiddenField).Value;
                HelperClass.openAddEditModal(this, "ConfirmModal");
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
                string result = DBAccess.ApproveMachineStartUpData(ddlMachineID.SelectedValue.ToString(), lblComponent.Text, lblOperation.Text, Session["Shift"].ToString(), Session["Date"].ToString(), txtMonth.Text,txtYear.Text);
                if (result.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openSuccessModal(this, "Approved");
                    HelperClassGeneric.clearModal(this);
                }
                else
                {
                    HelperClassGeneric.openWarningModal(this, string.IsNullOrEmpty(result) ? "ERROR! Try again." : result);
                    return;
                }
                BindGrid();
                HelperClass.clearModal(this);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

    }
}