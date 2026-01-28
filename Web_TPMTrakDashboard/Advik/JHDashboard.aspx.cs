using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik.DataBaseAccess;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Advik.Models;
using System.Drawing;

namespace Web_TPMTrakDashboard.Advik
{
    public partial class JHDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Session["UserName"] == null)
                //{
                //    Response.Redirect("~/SignIn.aspx", false);
                //}
                //hdnEmpRole.Value = AdvikDatabaseAccess.getEmployeeRole(Session["UserName"].ToString());
                bindAllPlants();
                BindCellIDs();
                bindAllMachines();
                bindShift();
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                bindJHDashboard();
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
                var allShift = BindCockpitView.GetAllShift();
                if (allShift != null && allShift.Count > 0)
                {
                    ddlShift.DataSource = allShift;
                    ddlShift.DataBind();

                    //ddlShift.Items.Insert(0, new ListItem
                    //{
                    //    Text = GetGlobalResourceObject("CommanResource", "ShiftAll").ToString(),
                    //    Value = "All"
                    //});

                }
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

        private void bindJHDashboard()
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
                foreach (ListItem item in ddlShift.Items)
                {
                    if (item.Selected)
                    {
                        shift += item.Value + ",";
                    }
                }
                string JStype = ddlJHType.SelectedValue == null ? "" : ddlJHType.SelectedValue.ToString();
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;
                DateTime.TryParse(txtFromDate.Text, out fromDate);
                fromDate = Util.GetDateTime(txtFromDate.Text + " 11:00:00");
                toDate = Util.GetDateTime(txtToDate.Text + " 11:00:00");
                List<JHDashboardDetails> listJHDashboardDetails = AdvikDatabaseAccess.getJHDashboarddetails(plant, machineID, shift, JStype, fromDate, toDate, GroupID);
                gvJHDashboard.DataSource = listJHDashboardDetails;
                gvJHDashboard.DataBind();
                Session["JHDashboardDetails"] = listJHDashboardDetails;
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
            bindJHDashboard();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string empid = Session["UserName"].ToString();
                for (int i = 0; i < gvJHDashboard.Rows.Count; i++)
                {
                    // string chkparam = "";
                    // string remarksPram = "";
                    string machineid = (gvJHDashboard.Rows[i].FindControl("lblMachine") as Label).Text;
                    //string jhactivity = (gvJHDashboard.Rows[i].FindControl("lbkJHActivity") as Label).Text;
                    string jhactivity = (gvJHDashboard.Rows[i].FindControl("hdnChecklistID") as HiddenField).Value;
                    string jhtype = (gvJHDashboard.Rows[i].FindControl("lblJHType") as Label).Text;
                    string remarks = (gvJHDashboard.Rows[i].FindControl("txtRemarks") as TextBox).Text;
                   
                    if ((gvJHDashboard.Rows[i].FindControl("hdnRemarksUpdate") as HiddenField).Value == "update")
                    {
                        (gvJHDashboard.Rows[i].FindControl("hdnRemarksUpdate") as HiddenField).Value = "";
                        int success = AdvikDatabaseAccess.SaveRemarks(machineid, jhactivity, jhtype, remarks, "", "", "UpdateRemarks");
                        if (success == 0)
                        {
                            lblMessages.ForeColor = System.Drawing.Color.Red;
                            lblMessages.Text = "Failed to save the remarks of row " + i;
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                            break;
                        }
                    }
                }
                bindJHDashboard();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            bool successfull = false;
            List<JHDashboardDetails> listJHDetails = new List<JHDashboardDetails>();
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
                foreach (ListItem item in ddlShift.Items)
                {
                    if (item.Selected)
                    {
                        shift += item.Value + ",";
                    }
                }
                string JStype = ddlJHType.SelectedValue == null ? "" : ddlJHType.SelectedValue == "" ? "All" : ddlJHType.SelectedValue.ToString();
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;
                DateTime.TryParse(txtFromDate.Text, out fromDate);
                fromDate = Util.GetDateTime(txtFromDate.Text + " 11:00:00");
                toDate = Util.GetDateTime(txtToDate.Text + " 11:00:00");
                if (Session["JHDashboardDetails"] != null)
                {
                    listJHDetails = Session["JHDashboardDetails"] as List<JHDashboardDetails>;
                }
                else
                {

                    listJHDetails = AdvikDatabaseAccess.getJHDashboarddetails(plant, machineID, shift, JStype, fromDate, toDate, GroupID);
                }
                if (listJHDetails != null && listJHDetails.Count > 0)
                {
                    Session["JHDashboardDetails"] = listJHDetails;
                    machineID = machineID == "" ? "All" : machineID;
                    shift = shift == "" ? "All" : shift;
                    successfull = AdvikGenerateReports.GenerateJHDashboardReport(machineID, shift, JStype, fromDate, toDate, listJHDetails);
                    if (!successfull)
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "Error. Export Unsuccessful.";
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    }
                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('Export Successful.')", true);
                    //  else
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('Error. Export Unsuccessful.')", true);
                }
                else
                {
                    lblMessages.ForeColor = Color.Red;
                    lblMessages.Text = "No data to export";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }

            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }

        protected void ddlCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindAllMachines();
        }
    }
}