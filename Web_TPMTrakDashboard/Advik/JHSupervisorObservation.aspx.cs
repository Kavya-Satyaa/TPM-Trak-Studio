using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik.DataBaseAccess;
using Web_TPMTrakDashboard.Advik.Models;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Advik
{
    public partial class JHSupervisorObservation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserName"] == null)
                {
                    Response.Redirect("~/SignIn.aspx", false);
                }
                bindAllPlants();
                BindCellIDs();
                bindAllMachines();
                bindShift();
                DateTime currentDate = DateTime.Now;
                DateTime beginDayOfWeek, endDayOfWeek;
                beginDayOfWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek + 1);
                endDayOfWeek = currentDate.AddDays(6 - (int)currentDate.DayOfWeek);
                txtFromDate.Text = beginDayOfWeek.ToString("dd-MM-yyyy");
                txtToDate.Text = endDayOfWeek.ToString("dd-MM-yyyy");
                ddlJHType.SelectedValue = "Manual";
                bindJHSupervisorData();

            }
        }
        private void bindAllPlants()
        {
            try
            {
                ddlPlant.DataSource = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllPlants();
                ddlPlant.DataBind();
                //ddlPlantId.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommanResource", "PlantAll").ToString(), "All"));
                //ddlPlantId_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }

        }
        private void bindAllMachines()
        {
            try
            {
                string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedItem.ToString();
                string Cell = ddlCell.SelectedValue == null ? "" : ddlCell.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedItem.ToString();
                ddlMachineId.DataSource = AdvikDatabaseAccess.GetMachinesbyPlantCell(plant, Cell);
                ddlMachineId.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }

        }
        private void bindShift()
        {
            try
            {
                List<string> allShift = BindCockpitView.GetAllShift();
                ddlShifts.DataSource = allShift;
                ddlShifts.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }

        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellIDs();
        }
        protected void ddlCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindAllMachines();
        }
        private void BindCellIDs()
        {
            try
            {
                string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString();
                List<string> GetCell = AdvikDatabaseAccess.GetCell(plant);
                ddlCell.DataSource = GetCell;
                ddlCell.DataBind();
                bindAllMachines();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void bindJHSupervisorData()
        {
            try
            {

                string plant = ddlPlant.SelectedValue == null || ddlPlant.SelectedValue == "All" ? "" : ddlPlant.SelectedItem.ToString();
                string GroupID = ddlCell.SelectedValue == null || ddlCell.SelectedValue == "All" ? "" : ddlCell.SelectedItem.ToString();
                string machineID = "", shift = "";
                foreach (ListItem item in ddlMachineId.Items)
                {
                    if (item.Selected)
                    {
                        machineID += item.Value + ",";
                    }
                }
                shift = ddlShifts.SelectedValue;
                string JStype = ddlJHType.SelectedValue == null ? "" : ddlJHType.SelectedValue.ToString();
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;
                DateTime.TryParse(txtFromDate.Text, out fromDate);
                fromDate = Util.GetDateTime(txtFromDate.Text + " 11:00:00");
                toDate = Util.GetDateTime(txtToDate.Text + " 11:00:00");
                List<JHDashboardDetails> listJHDashboardDetails = AdvikDatabaseAccess.getJHSupervisorObservationdetails(plant, machineID, shift, JStype, fromDate, toDate, GroupID);
                lvSupervisorObs.DataSource = listJHDashboardDetails;
                lvSupervisorObs.DataBind();
                Session["JHSupervisorObservationDetails"] = listJHDashboardDetails;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            bindJHSupervisorData();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < lvSupervisorObs.Items.Count; i++)
                {
                    JHDashboardDetails data = new JHDashboardDetails();
                    data.Machine = (lvSupervisorObs.Items[i].FindControl("lblMachineID") as Label).Text;
                    data.JHType = (lvSupervisorObs.Items[i].FindControl("lblJHType") as Label).Text;
                    data.Date = (lvSupervisorObs.Items[i].FindControl("hdnAuditDate") as HiddenField).Value;
                    data.Shift = (lvSupervisorObs.Items[i].FindControl("lblShift") as Label).Text;
                    data.SupervisorName = Session["UserName"].ToString();
                    data.SupervisorStatus = "NOT OK";
                    if ((lvSupervisorObs.Items[i].FindControl("hdnSupUpdate") as HiddenField).Value == "update")
                    {
                        (lvSupervisorObs.Items[i].FindControl("hdnSupUpdate") as HiddenField).Value = "";
                        if ((lvSupervisorObs.Items[i].FindControl("chkSupObs") as CheckBox).Checked)
                        {
                            data.SupervisorStatus = "OK";
                        }
                        int success = AdvikDatabaseAccess.SaveSupervisorDetails(data);
                        if (success == 0)
                        {
                            lblMessages.ForeColor = System.Drawing.Color.Red;
                            lblMessages.Text = "Failed to save the remarks of data " + i;
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                            break;
                        }
                    }
                }
                bindJHSupervisorData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }

        }



        protected void sendEmail_Click(object sender, EventArgs e)
        {
            try
            {
                string shift ="";
                string RunReportForEvery = "";
                string ReportName = "";
                int res = AdvikDatabaseAccess.SaveProdSupEmailsDetails(shift, RunReportForEvery, ReportName);
            }
            catch (Exception ex)
            { }
        }
    }
}