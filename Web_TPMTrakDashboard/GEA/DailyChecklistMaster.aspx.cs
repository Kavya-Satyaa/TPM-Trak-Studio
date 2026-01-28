using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;
using Web_TPMTrakDashboard.GEA.Models;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class DailyChecklistMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                if (!IsPostBack)
                {
                    SessionClear.ClearSession();
                    BindLineIDs();
                    BindMachineIDs();
                    BindFrequencies();
                    LoadMaintenanceChecklistData();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void BindLineIDs()
        {
            try
            {
                List<string> lstLineData = GEADatabaseAccess.GetLineIDsForPlant("");
                if (lstLineData != null && lstLineData.Count > 0)
                {
                    ddlLineID.DataSource = lstLineData;
                    ddlLineID.DataBind();
                    ddlLineID.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private void BindMachineIDs()
        {
            try
            {
                string LineId = ddlLineID.SelectedItem != null ? ddlLineID.SelectedValue : "";
                if (!string.IsNullOrEmpty(LineId))
                {
                    if (LineId.Equals("All", StringComparison.OrdinalIgnoreCase)) LineId = "";
                    List<string> lstMachineIDs = GEADatabaseAccess.GetAllMachineByPlantandGroup("", LineId);
                    if (lstMachineIDs != null && lstMachineIDs.Count > 0)
                    {
                        ddlMachineId.DataSource = lstMachineIDs;
                        ddlMachineId.DataBind();
                        ddlMachineId.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private void BindFrequencies()
        {
            try
            {
                List<FrequencyEntity> frequencies = GEADatabaseAccess.GetAllFrequencies();
                if (frequencies != null && frequencies.Count > 0)
                {
                    ddlFrequency.DataSource = frequencies;
                    ddlFrequency.DataTextField = "Frequency";
                    ddlFrequency.DataValueField = "FreqID";
                    ddlFrequency.DataBind();
                    ddlFrequency.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private void LoadMaintenanceChecklistData()
        {
            List<MaintenanceChecklistEntity> MaintenanceChecklistData = new List<MaintenanceChecklistEntity>();
            try
            {
                string MachineID = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                string Frequency = ddlFrequency.SelectedItem != null ? ddlFrequency.SelectedValue : "";
                if (!string.IsNullOrEmpty(MachineID) && !string.IsNullOrEmpty(Frequency))
                {
                    if (MachineID.Equals("All", StringComparison.OrdinalIgnoreCase)) MachineID = "";
                    if (Frequency.Equals("All", StringComparison.OrdinalIgnoreCase)) Frequency = "";
                    Session["MachineID"] = MachineID;
                    Session["Frequency"] = Frequency;
                    MaintenanceChecklistData = GEADatabaseAccess.GetMaintenanceChklistData(MachineID, Frequency);
                    if (MaintenanceChecklistData != null && MaintenanceChecklistData.Count > 0)
                    {
                        Session["MaintChklistData"] = MaintenanceChecklistData;
                        GridChecklistMaster.DataSource = MaintenanceChecklistData;
                        GridChecklistMaster.DataBind();
                    }
                    else
                    {
                        Session["MaintChklistData"] = null;
                        GridChecklistMaster.DataSource = new List<MaintenanceChecklistEntity>();
                        GridChecklistMaster.DataBind();
                    }
                }
                else
                {
                    GridChecklistMaster.DataSource = new List<MaintenanceChecklistEntity>();
                    GridChecklistMaster.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        protected void GridChecklistMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string Idd = (e.Row.Cells[1].FindControl("lblIDD") as Label).Text;
                TextBox txtCheckPointID = e.Row.Cells[2].FindControl("txtCheckPointID") as TextBox;
                if (txtCheckPointID != null)
                {
                    if (Idd.Equals("0"))
                        txtCheckPointID.Enabled = true;
                    else
                        txtCheckPointID.Enabled = false;
                }
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            LoadMaintenanceChecklistData();
        }

        protected void ddlLineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineIDs();
        }

        protected void GridChecklistMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridChecklistMaster.EditIndex = e.NewEditIndex;
            LoadMaintenanceChecklistData();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            List<MaintenanceChecklistEntity> MaintenanceChecklistData = new List<MaintenanceChecklistEntity>();
            try
            {
                string MachineID = Session["MachineID"] as string;
                string Frequency = Session["Frequency"] as string;
                if (Session["MaintChklistData"] != null)
                    MaintenanceChecklistData = Session["MaintChklistData"] as List<MaintenanceChecklistEntity>;
                else
                {
                    MaintenanceChecklistData = GEADatabaseAccess.GetMaintenanceChklistData(MachineID, Frequency);
                }
                if (btnNew.Text.Equals("New"))
                {
                    MaintenanceChecklistData.Add(new MaintenanceChecklistEntity { IDD = 0, ActivityID = 0, Activity = "", Criteria = "", FreqID = Convert.ToInt32(Frequency), IsEnabled = false, MachineID = MachineID, Method = "Manual", TemplateType = "Text" });
                    GridChecklistMaster.DataSource = MaintenanceChecklistData;
                    GridChecklistMaster.DataBind();
                    btnNew.Text = "Cancel";
                }
                if (btnNew.Text.Equals("Cancel"))
                {
                    GridChecklistMaster.DataSource = MaintenanceChecklistData;
                    GridChecklistMaster.DataBind();
                    btnNew.Text = "New";
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (GridChecklistMaster.Rows.Count > 0)
                {
                    foreach (GridViewRow row in GridChecklistMaster.Rows)
                    {
                        string hdfUpdate = ((HiddenField)row.FindControl("hdfUpdate")).Value;
                        if (hdfUpdate == "Update")
                        {
                            string Idd = (row.Cells[1].FindControl("lblIDD") as Label).Text;
                            string ActivityId = (row.Cells[2].FindControl("txtCheckPointID") as TextBox).Text;
                            string Activity = (row.Cells[3].FindControl("txtCheckPoints") as TextBox).Text;
                            string FreqID = Session["Frequency"] != null ? Session["Frequency"].ToString() : (ddlFrequency.SelectedItem != null ? ddlFrequency.SelectedValue : "");
                            string MachineID = Session["MachineID"] != null ? Session["MachineID"].ToString() : (ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "");
                            string Method = (row.Cells[4].FindControl("ddlMethod") as DropDownList).Text;
                            string Criteria = (row.Cells[5].FindControl("txtCriteria") as TextBox).Text;
                            string TempType = (row.Cells[6].FindControl("ddlTemplateType") as DropDownList).Text;
                            int IsEnabled = Convert.ToInt32((row.Cells[7].FindControl("chkIsEnabled") as CheckBox).Checked);
                            if (string.IsNullOrEmpty(ActivityId))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Check point ID cannot be empty or zero.')", true);
                                return;
                            }
                            else if (Convert.ToInt32(ActivityId) <= 0)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Check point ID cannot be zero or negative.')", true);
                                return;
                            }
                            else if (string.IsNullOrEmpty(Activity))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Activity cannot be empty.')", true);
                                return;
                            }
                            else if (string.IsNullOrEmpty(FreqID))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Frequency cannot be empty.')", true);
                                return;
                            }
                            else if (string.IsNullOrEmpty(MachineID))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Machine ID cannot be empty.')", true);
                                return;
                            }
                            else if (string.IsNullOrEmpty(Method))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Method cannot be empty.')", true);
                                return;
                            }
                            else if (string.IsNullOrEmpty(TempType))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Template type cannot be empty.')", true);
                                return;
                            }
                            else if (Idd.Equals("0"))
                            {
                                if (GEADatabaseAccess.IsChecklistMasterKeyExists(ActivityId, MachineID, FreqID))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Entered Check Point ID already exists for selected machine and frequency.')", true);
                                    return;
                                }
                                else
                                {
                                    GEADatabaseAccess.UpdateMaintenanceChecklist(ActivityId, Activity, FreqID, MachineID, Method, Criteria, TempType, IsEnabled);
                                }
                            }
                            else
                            {
                                GEADatabaseAccess.UpdateMaintenanceChecklist(ActivityId, Activity, FreqID, MachineID, Method, Criteria, TempType, IsEnabled);
                            }
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Data updated successfully.')", true);
                    LoadMaintenanceChecklistData();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NoData", "alert('No data to save.')", true);
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsDeleted = false;
                if (GridChecklistMaster.Rows.Count > 0)
                {
                    foreach (GridViewRow row in GridChecklistMaster.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkSelect = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                            if (chkSelect.Checked)
                            {
                                string Idd = (row.Cells[1].FindControl("lblIDD") as Label).Text;
                                if (!Idd.Equals("0"))
                                {
                                    IsDeleted = GEADatabaseAccess.DeleteMaintenanceChklistData(Idd);
                                    if (!IsDeleted) break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('No data selected for deletion.')", true);
                }
                if (IsDeleted)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Data deleted successfully.')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Error while deleting data.')", true);
                }
                LoadMaintenanceChecklistData();
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LoadMaintenanceChecklistData();
        }

        protected void btnApplyRules_Click(object sender, EventArgs e)
        {
            Response.Redirect(@"~/GEA/WeeklyChecklistMaster.aspx");
        }
    }
}