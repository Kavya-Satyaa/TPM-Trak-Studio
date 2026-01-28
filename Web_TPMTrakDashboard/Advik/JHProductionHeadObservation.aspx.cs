using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik.DataBaseAccess;
using Web_TPMTrakDashboard.Advik.Models;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Advik
{
    public partial class JHProductionHeadObservation : System.Web.UI.Page
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
                //txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                //txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtYear.Text = DateTime.Now.Year.ToString();
                txtMonth.Text = DateTime.Now.Month.ToString("00");
                ddlJHType.SelectedValue = "Manual";
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
                int noOfSelectedShift = 0;
                foreach (ListItem item in ddlShift.Items)
                {
                    if (item.Selected)
                    {
                        noOfSelectedShift++;
                        shift += item.Value + ",";
                    }
                }
                string JStype = ddlJHType.SelectedValue == null ? "" : ddlJHType.SelectedValue.ToString();
                // DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;
                //DateTime.TryParse(txtFromDate.Text, out fromDate);
                //fromDate = Util.GetDateTime(txtFromDate.Text + " 11:00:00");
                //toDate = Util.GetDateTime(txtToDate.Text + " 11:00:00");
                string fromDateTxt = txtYear.Text + "-" + txtMonth.Text + "-01";
                DateTime fromDate = Convert.ToDateTime(fromDateTxt);
                List<JHDashboardDetails> listJHDashboardDetails = AdvikDatabaseAccess.getJHProdHeadObservationDetails(plant, machineID, shift, JStype, fromDate, "", GroupID, noOfSelectedShift == 0 ? ddlShift.Items.Count : noOfSelectedShift);

                lvProdHeadData.DataSource = listJHDashboardDetails;
                lvProdHeadData.DataBind();

                Session["JHProdHeadObservationDetails"] = listJHDashboardDetails;
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
                if (txtMonth.Text == "")
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Please select Month.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    return;
                }
                if (txtYear.Text == "")
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Please select Year.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    return;
                }
                if (lvProdHeadData.Items.Count > 0)
                {
                    int noOfSelectedShift = Convert.ToInt32((lvProdHeadData.Items[0].FindControl("hdnNoOoSelectedShift") as HiddenField).Value);
                    if ((lvProdHeadData.Items[0].FindControl("hdnProdHeadUpdate") as HiddenField).Value == "update")
                    {

                        (lvProdHeadData.Items[0].FindControl("hdnProdHeadUpdate") as HiddenField).Value = "";
                        string chkValue = "NOT OK";
                        if ((lvProdHeadData.Items[0].FindControl("chkProdHeadObs") as CheckBox).Checked)
                        {
                            chkValue = "OK";
                        }
                        int insertionFailed = 0;
                       List<string> machinelist= AdvikDatabaseAccess.GetMachinesbyPlantCell("", "");
                        //foreach (ListItem machine in ddlMachineId.Items)
                        foreach (string machine in machinelist)
                        {
                            string machineID = machine;
                            foreach (ListItem shift in ddlShift.Items)
                            {
                                string shiftID = shift.Value;
                                string jhtype = "";
                                if (ddlJHType.SelectedValue == "All" || ddlJHType.SelectedValue == "")
                                {
                                    for (int i = 0; i < 2; i++)
                                    {
                                        if (i == 0)
                                        {
                                            jhtype = "Auto";
                                            JHDashboardDetails data = new JHDashboardDetails();
                                            data.Machine = machineID;
                                            data.JHType = jhtype;
                                            // data.AuditDate = (lvProdHeadData.Items[i].FindControl("hdnAuditDate") as HiddenField).Value;
                                            data.Status = chkValue;
                                            data.ProdHeadName = Session["UserName"].ToString();
                                            data.Shift = shiftID;
                                            data.AuditDate = txtYear.Text + "-" + txtMonth.Text + "-" + DateTime.Now.ToString("dd");
                                            int success = AdvikDatabaseAccess.SaveProductionHeadObservationDetails(data);
                                            if (success == 0)
                                            {
                                                lblMessages.ForeColor = System.Drawing.Color.Red;
                                                lblMessages.Text = "Failed to save the data.";
                                                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                                                insertionFailed = 1;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            jhtype = "Manual";
                                            JHDashboardDetails data = new JHDashboardDetails();
                                            data.Machine = machineID;
                                            data.JHType = jhtype;
                                            // data.AuditDate = (lvProdHeadData.Items[i].FindControl("hdnAuditDate") as HiddenField).Value;
                                            data.Status = chkValue;
                                            data.ProdHeadName = Session["UserName"].ToString();
                                            data.Shift = shiftID;
                                            data.AuditDate = txtYear.Text + "-" + txtMonth.Text + "-" + DateTime.Now.ToString("dd");
                                            int success = AdvikDatabaseAccess.SaveProductionHeadObservationDetails(data);
                                            if (success == 0)
                                            {
                                                lblMessages.ForeColor = System.Drawing.Color.Red;
                                                lblMessages.Text = "Failed to save the data.";
                                                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                                                insertionFailed = 1;
                                                break;
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    jhtype = ddlJHType.SelectedValue;
                                    JHDashboardDetails data = new JHDashboardDetails();
                                    data.Machine = machineID;
                                    data.JHType = jhtype;
                                    // data.AuditDate = (lvProdHeadData.Items[i].FindControl("hdnAuditDate") as HiddenField).Value;
                                    data.Status = chkValue;
                                    data.ProdHeadName = Session["UserName"].ToString();
                                    data.Shift = shiftID;
                                    data.AuditDate = txtYear.Text + "-" + txtMonth.Text + "-" + DateTime.Now.ToString("dd");
                                    int success = AdvikDatabaseAccess.SaveProductionHeadObservationDetails(data);
                                    if (success == 0)
                                    {
                                        lblMessages.ForeColor = System.Drawing.Color.Red;
                                        lblMessages.Text = "Failed to save the data.";
                                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                                        insertionFailed = 1;
                                        break;
                                    }
                                }
                                if (insertionFailed == 1)
                                {
                                    break;
                                }
                            }
                            if (insertionFailed == 1)
                            {
                                break;
                            }
                        }
                    }
                }
                bindJHDashboard();
                //if (lvProdHeadData.Items.Count > 0)
                //{
                //    int noOfSelectedShift = Convert.ToInt32((lvProdHeadData.Items[0].FindControl("hdnNoOoSelectedShift") as HiddenField).Value);
                //    if ((lvProdHeadData.Items[0].FindControl("hdnProdHeadUpdate") as HiddenField).Value == "update")
                //    {

                //        (lvProdHeadData.Items[0].FindControl("hdnProdHeadUpdate") as HiddenField).Value = "";
                //        string chkValue = "NOT OK";
                //        if ((lvProdHeadData.Items[0].FindControl("chkProdHeadObs") as CheckBox).Checked)
                //        {
                //            chkValue = "OK";
                //        }
                //        for(int i=0;i< lvProdHeadData.Items.Count;i++)
                //        {
                //            JHDashboardDetails data = new JHDashboardDetails();
                //            data.Machine= (lvProdHeadData.Items[i].FindControl("lblMachineID") as Label).Text;
                //            data.JHType= (lvProdHeadData.Items[i].FindControl("lblJHType") as Label).Text;
                //            data.AuditDate = (lvProdHeadData.Items[i].FindControl("hdnAuditDate") as HiddenField).Value;
                //            data.Status = chkValue;
                //            data.ProdHeadName = Session["UserName"].ToString();
                //            data.Shift= (lvProdHeadData.Items[i].FindControl("lblShift") as Label).Text;
                //            int success = AdvikDatabaseAccess.SaveProductionHeadObservationDetails(data);
                //            if (success == 0)
                //            {
                //                lblMessages.ForeColor = System.Drawing.Color.Red;
                //                lblMessages.Text = "Failed to save the data of row " + i;
                //                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                //                break;
                //            }
                //        }
                //    }
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
                string shift = "";
                string RunReportForEvery = "";
                string ReportName = "";
                int res = AdvikDatabaseAccess.SaveProdSupEmailsDetails(shift, RunReportForEvery, ReportName);
            }
            catch (Exception ex)
            { }
        }
    }
}