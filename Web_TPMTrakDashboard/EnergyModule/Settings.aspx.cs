using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.EnergyModule.Models;

namespace Web_TPMTrakDashboard.EnergyModule
{
    public partial class Settings : System.Web.UI.Page
    {
        List<string> allplantid = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSettingsInformations();

            }
        }

        private void BindSettingsInformations()
        {
            try
            {
                BindGridSettingsInformations();
                BindPlants();
                BindCells();
                BindTargetSettingsInformations();
                BindCostData();
                BindPlantIDs();
                BindEnergySource();
                if (DataBaseAccess.GetSelectedValues("EM_PlantID") != "" && DataBaseAccess.GetSelectedValues("EM_PlantID") != null)
                {
                    ddlPlantID.SelectedValue = DataBaseAccess.GetSelectedValues("EM_PlantID");
                }
                BindCellIDs();
                if (DataBaseAccess.GetSelectedValues("EM_CellID") != "" && DataBaseAccess.GetSelectedValues("EM_CellID") != null)
                {
                    ddlCellID.SelectedValue = DataBaseAccess.GetSelectedValues("EM_CellID");
                }
                if (DataBaseAccess.GetSelectedValues("EM_Type") != "" && DataBaseAccess.GetSelectedValues("EM_Type") != null)
                {
                    ddlMachinType.SelectedValue = DataBaseAccess.GetSelectedValues("EM_Type");
                }
                if (ddlTargetMachiningType.SelectedValue.ToString().Equals("Non-Machine EM", StringComparison.OrdinalIgnoreCase))
                {
                    lblTargetCell.Visible = false;
                    ddlCell.Visible = false;
                }
                else
                {
                    lblTargetCell.Visible = true;
                    ddlCell.Visible = true;
                }
                if (ddlMachinType.SelectedValue.ToString().Equals("Non-Machine EM", StringComparison.OrdinalIgnoreCase))
                {
                    Celltd.Visible = false;
                    ddlCellID.Visible = false;
                    Planttd.Visible = false;
                    ddlPlantID.Visible = false;
                }
                else
                {
                    Celltd.Visible = true;
                    ddlCellID.Visible = true;
                    Planttd.Visible = true;
                    ddlPlantID.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindCostData()
        {
            try
            {
                List<ShopDefaultEntity> list = DataBaseAccess.getShopDefaultValueSetting("CostPerKWH");
                txtCostPerKWH.Text = list.Where(k => k.Parameter.Equals("CostPerKWH", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindEnergySource()
        {
            try
            {
                List<ShopDefaultEntity> list = DataBaseAccess.getShopDefaultValueSetting("EnergySource");
                ddlEnergySource.SelectedValue = list.Where(k => k.Parameter.Equals("EnergySource", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindPlants()
        {
            try
            {
                allplantid = DataBaseAccess.GetAllPlants();
                if (allplantid.Count > 0)
                {
                    ddlPlant.DataSource = allplantid;
                    ddlPlant.DataBind();
                    ddlPlant.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindTargetSettingsInformations()
        {
            try
            {

                if (string.IsNullOrEmpty(ddlPlant.SelectedValue.ToString())) return;
                DataTable dt = DataBaseAccess.GetEnergyTargetDetails(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString(), ddlCell.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedValue.ToString(), ddlTargetMachiningType.SelectedValue.ToString());
                foreach (DataColumn col in dt.Columns)
                {
                    col.ReadOnly = false;
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    gvTargetSettings.DataSource = dt;
                    gvTargetSettings.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

        }

        private void BindGridSettingsInformations()
        {
            List<GridSettings> gridSettings = DataBaseAccess.GetGridInformation(ddlMachineType.SelectedValue.ToString());
            gvGridColumnSettings.DataSource = gridSettings;
            gvGridColumnSettings.DataBind();
        }

        protected void btnUpdateTarget_Click(object sender, EventArgs e)
        {
            bool isSuccessFailure = false;
            try
            {
                foreach (GridViewRow row in gvTargetSettings.Rows)
                {
                    string machineid = (row.FindControl("lblMachineId") as Label).Text;
                    string target = (row.FindControl("txtKwh") as TextBox).Text;
                    DataBaseAccess.SaveEnergyTargetDetails(machineid, target, out isSuccessFailure);
                }
                BindTargetSettingsInformations();
                if (!isSuccessFailure)
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageTargetNotOk();", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "messageTargetOk();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnUpdateColVisibility_Click(object sender, EventArgs e)
        {
            int count = 0;
            try
            {
                foreach (GridViewRow row in gvGridColumnSettings.Rows)
                {
                    string columnName = (row.FindControl("lblGridColumn") as Label).Text;
                    string columnText = (row.FindControl("txtColumnText") as TextBox).Text;
                    bool visibility = (row.FindControl("cbVisibility") as CheckBox).Checked;
                    int sortOrder = Convert.ToInt32((row.FindControl("txtSortOrder") as TextBox).Text);
                    count = DataBaseAccess.SaveGridColumnsSettingsVals(ddlMachineType.SelectedValue.ToString(), columnName, columnText, visibility, sortOrder);
                }
                BindGridSettingsInformations();
                if (count < 0)
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageGridNotOk();", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "messageGridOk();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

        }


        protected void gvTargetSettings_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox textBox = e.Row.FindControl("txtKwh") as TextBox;
                textBox.TextMode = TextBoxMode.Number;

            }
        }

        protected void ddlMachineType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridSettingsInformations();
        }

        protected void btnCostSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataBaseAccess.saveShopDefaultValueInText("CostPerKWH", txtCostPerKWH.Text);
                BindCostData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindPlantIDs()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlants();
                list.Remove("All");
                ddlPlantID.DataSource = list;
                ddlPlantID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindCellIDs()
        {
            try
            {
                List<string> list = DataBaseAccess.GetCellIDs(ddlPlantID.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantID.SelectedValue.ToString());
                list.Remove("All");
                ddlCellID.DataSource = list;
                ddlCellID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellIDs();
        }

        protected void btnDefaultSettingSave_Click(object sender, EventArgs e)
        {
            try
            {
                string success = "";
                success = DataBaseAccess.SaveSelectedValues("EM_PlantID", ddlPlantID.SelectedValue.ToString());
                if (success != "")
                {
                    success = DataBaseAccess.SaveSelectedValues("EM_CellID", ddlCellID.SelectedValue.ToString());
                    if (success != "")
                    {
                        success = DataBaseAccess.SaveSelectedValues("EM_Type", ddlMachinType.SelectedValue.ToString());
                        if (success != "")
                        {
                            HelperClassEM.openSuccessModal(this, "Saved Successfully.");
                            return;
                        }
                        else
                        {
                            HelperClassEM.openErrorModal(this, "Save Failed!!!");
                            return;
                        }
                    }
                    else
                    {
                        HelperClassEM.openErrorModal(this, "Save Failed!!!");
                        return;
                    }
                }
                else
                {
                    HelperClassEM.openErrorModal(this, "Failed!!!");
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindTargetSettingsInformations();
        }
        private void BindCells()
        {
            try
            {
                List<string> list = DataBaseAccess.GetCellIDs(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString());
                ddlCell.DataSource = list;
                ddlCell.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCells();
        }

        protected void ddlTargetMachiningType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlTargetMachiningType.SelectedValue.ToString().Equals("Non-Machine EM", StringComparison.OrdinalIgnoreCase))
                {
                    lblTargetCell.Visible = false;
                    ddlCell.Visible = false;
                }
                else
                {
                    lblTargetCell.Visible = true;
                    ddlCell.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlMachinType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlMachinType.SelectedValue.ToString().Equals("Non-Machine EM", StringComparison.OrdinalIgnoreCase))
                {
                    Celltd.Visible = false;
                    ddlCellID.Visible = false;
                    Planttd.Visible = false;
                    ddlPlantID.Visible = false;
                }
                else
                {
                    Celltd.Visible = true;
                    ddlCellID.Visible = true;
                    Planttd.Visible = true;
                    ddlPlantID.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnEnergySource_Click(object sender, EventArgs e)
        {
            try
            {
                DataBaseAccess.saveShopDefaultValueInText("EnergySource", ddlEnergySource.SelectedValue.ToString());
                BindEnergySource();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }       
    }
}