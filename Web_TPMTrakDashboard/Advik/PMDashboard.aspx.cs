using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik.DataBaseAccess;
using Web_TPMTrakDashboard.Advik.Models;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Advik
{
    public partial class PMDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindAllPlants();
                bindCellID();
                bindAllMachines();
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                bindPHDashboardDetails();
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

        private void bindCellID()
        {
            try
            {
                string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString();
                List<string> GetCell = AdvikDatabaseAccess.GetCell(plant);
                ddlGroup.DataSource = GetCell;
                ddlGroup.DataBind();
                bindAllMachines();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void bindAllMachines()
        {
            try
            {
                string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedItem.ToString();
                string Cell = ddlGroup.SelectedValue == null ? "" : ddlGroup.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlGroup.SelectedItem.ToString();
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

        private void bindPHDashboardDetails()
        {
            try
            {

                string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString();
                string GroupID = ddlGroup.SelectedValue == null ? "" : ddlGroup.SelectedItem.ToString();
                string machineID = "", shift = "";
                foreach (ListItem item in ddlMachineId.Items)
                {
                    if (item.Selected)
                    {
                        machineID += item.Value + ",";
                    }
                }
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;
                DateTime.TryParse(txtFromDate.Text, out fromDate);
                fromDate = Util.GetDateTime(txtFromDate.Text + " 11:00:00");
                toDate = Util.GetDateTime(txtToDate.Text + " 11:00:00");
                List<PMMasterDetails> listPMDashboardDetails = AdvikDatabaseAccess.getPMDashboarddetails(machineID, fromDate, toDate, plant, GroupID);
                gvPMDashboardDetails.DataSource = listPMDashboardDetails;
                gvPMDashboardDetails.DataBind();
                Session["PMDashboardDetails"] = listPMDashboardDetails;
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
            bindPHDashboardDetails();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string machineid = ddlMachineId.SelectedValue == null ? "" : ddlMachineId.SelectedItem.ToString();
                string updatedby = "";
                for (int i=0;i< gvPMDashboardDetails.Rows.Count; i++)
                {
                    string PMid = (gvPMDashboardDetails.Rows[i].FindControl("hfPMId") as HiddenField).Value;
                    string status= (gvPMDashboardDetails.Rows[i].FindControl("txtStatus") as TextBox).Text;
                    int success = AdvikDatabaseAccess.SavePHTransactionDetails(machineid, PMid, status, updatedby);
                    if (success == 0)
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = "Failed to save the Status of row " + i;
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    }
                }
                bindPHDashboardDetails();
            }catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }

        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindCellID();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            bool successfull = false;
            List<PMMasterDetails> listPMDetails = new List<PMMasterDetails>();
            try
            {
                string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString();
                string GroupID = ddlGroup.SelectedValue == null ? "" : ddlGroup.SelectedItem.ToString();
                string machineID = "", shift = "";
                foreach (ListItem item in ddlMachineId.Items)
                {
                    if (item.Selected)
                    {
                        machineID += item.Value + ",";
                    }
                }
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;
                DateTime.TryParse(txtFromDate.Text, out fromDate);
                fromDate = Util.GetDateTime(txtFromDate.Text + " 11:00:00");
                toDate = Util.GetDateTime(txtToDate.Text + " 11:00:00");
                if (Session["PMDashboardDetails"] != null)
                {
                    listPMDetails = Session["PMDashboardDetails"] as List<PMMasterDetails>;
                }
                else
                {

                    listPMDetails = AdvikDatabaseAccess.getPMDashboarddetails(machineID, fromDate, toDate, plant, GroupID);
                }
                if (listPMDetails != null && listPMDetails.Count > 0)
                {
                    Session["PMDashboardDetails"] = listPMDetails;
                    machineID = machineID == "" ? "All" : machineID;
                    successfull = AdvikGenerateReports.GeneratePMDashboardReport(plant, machineID, fromDate, toDate, listPMDetails);
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

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindAllMachines();
        }
    }
}