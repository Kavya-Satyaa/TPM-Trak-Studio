using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.TAFE
{
    public partial class TafeMachineHistory : System.Web.UI.Page
    {
        string isGenerated = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindAllDropdowns();
                btnView_Click(null, null);
            }
        }

        private void BindAllDropdowns()
        {
            List<string> allMachines = TafeDataBaseAccess.GetAllMachines();
            ddlMachineId.DataSource = allMachines;
            ddlMachineId.DataBind();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime FromDateTime = Util.GetDateTime(txtFromDate.Text);
                DateTime ToDateTime = Util.GetDateTime(txtToDate.Text);
                string fromdate = DataBaseAccess.GetLogicalDay(FromDateTime.ToString("yyyy-MM-dd 13:00:00"));
                string todate = DataBaseAccess.GetLogicalDayEnd(ToDateTime.ToString("yyyy-MM-dd 13:00:00"));

                string machineId = ddlMachineId.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? string.Empty : ddlMachineId.SelectedValue.ToString();
                BindgvMacHistory(FromDateTime, ToDateTime, machineId);
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private void BindgvMacHistory(DateTime fromDateTime, DateTime toDateTime, string machineId)
        {
            try
            {
                List<MachineHistory> allMacHistories = TafeDataBaseAccess.GetMachineHistoryDatas(fromDateTime, toDateTime, machineId);
                if (allMacHistories != null && allMacHistories.Count > 0)
                {
                    gvMacHistory.DataSource = allMacHistories;
                    gvMacHistory.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        protected void gvMacHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HiddenField hdfKindofProb = e.Row.FindControl("hdfKindOfProblem") as HiddenField;
                    HiddenField hdfDownCategory = e.Row.FindControl("hdfDownCategory") as HiddenField;
                    HiddenField hdfSeverity = e.Row.FindControl("hdfSeverity") as HiddenField;
                    DropDownList ddlKindOfProblem = (e.Row.FindControl("ddlKindOfProblem") as DropDownList);
                    DropDownList ddlDownCategory = (e.Row.FindControl("ddlBreakdownCategory") as DropDownList);
                    DropDownList ddlSeverity = (e.Row.FindControl("ddlSeverity") as DropDownList);
                    if (ddlKindOfProblem != null)
                    {
                        if (!string.IsNullOrEmpty(hdfKindofProb.Value))
                            ddlKindOfProblem.SelectedValue = hdfKindofProb.Value;
                        else
                            ddlKindOfProblem.SelectedIndex = 0;
                    }
                    if (ddlDownCategory != null)
                    {
                        if (!string.IsNullOrEmpty(hdfDownCategory.Value))
                            ddlDownCategory.SelectedValue = hdfDownCategory.Value;
                        else
                            ddlDownCategory.SelectedIndex = 0;
                    }
                    if (ddlSeverity != null)
                    {
                        if (!string.IsNullOrEmpty(hdfSeverity.Value))
                            ddlSeverity.SelectedValue = hdfSeverity.Value;
                        else
                            ddlSeverity.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool IsSuccessfull = false;
            string updateConf = string.Empty, machineID = string.Empty, downCode = string.Empty, breakDownStart = string.Empty, reason = string.Empty, actionToResolve = string.Empty, actionProposed = string.Empty, severity = string.Empty, kindOfProblem = string.Empty, downCategory = string.Empty;
            try
            {
                foreach (GridViewRow gridViewRow in gvMacHistory.Rows)
                {
                    updateConf = (gridViewRow.FindControl("hdfUpate") as HiddenField).Value;
                    if (updateConf.Equals("update", StringComparison.OrdinalIgnoreCase))
                    {
                        machineID = (gridViewRow.FindControl("lblMacID") as Label).Text;
                        downCode = (gridViewRow.FindControl("lblDownCode") as Label).Text;
                        kindOfProblem = (gridViewRow.FindControl("ddlKindOfProblem") as DropDownList).SelectedValue;
                        downCategory = (gridViewRow.FindControl("ddlBreakdownCategory") as DropDownList).SelectedValue;
                        breakDownStart = (gridViewRow.FindControl("lblStartDateTime") as Label).Text;
                        reason = (gridViewRow.FindControl("txtReason") as TextBox).Text;
                        actionToResolve = (gridViewRow.FindControl("txtActionToResolve") as TextBox).Text;
                        actionProposed = (gridViewRow.FindControl("txtActionProposed") as TextBox).Text;
                        severity = (gridViewRow.FindControl("ddlSeverity") as DropDownList).SelectedValue;
                        IsSuccessfull = TafeDataBaseAccess.SaveMachineHistoryData(machineID, downCode, kindOfProblem, downCategory, breakDownStart, reason, actionToResolve, actionProposed, severity);
                        if (!IsSuccessfull)
                        {
                            lblMessages.Text = "Error. Data not saved.";
                            lblMessages.ForeColor = System.Drawing.Color.Red;
                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);
                            break;
                        }
                    }
                }
                if (IsSuccessfull)
                {
                    lblMessages.Text = "Data saved successfully.";
                    lblMessages.ForeColor = System.Drawing.Color.DarkGreen;
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageSaveSuccess();", true);
                }
                btnView_Click(null, null);
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string machineId = ddlMachineId.SelectedValue == "All" ? string.Empty : ddlMachineId.SelectedValue;
            DateTime fromDate = DateTime.Now.Date;
            DateTime toDate = DateTime.Now.Date;
            fromDate = Util.GetDateTime(txtFromDate.Text);
            toDate = Util.GetDateTime(txtToDate.Text);
            string startDate = DataBaseAccess.GetLogicalDay(fromDate.ToString("yyyy-MM-dd 13:00:00"));
            string endDate = DataBaseAccess.GetLogicalDayEnd(toDate.ToString("yyyy-MM-dd 13:00:00"));

            isGenerated = TAFEGenerateReports.GenerateMachineHistoryReport(fromDate, toDate, machineId);

            if (isGenerated.Equals("", StringComparison.OrdinalIgnoreCase))
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);
            else if (isGenerated.Equals("NoDataFound", StringComparison.OrdinalIgnoreCase))
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNodata", "messageNodata();", true);
            else if (isGenerated.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "messageOk();", true);
        }
    }
}