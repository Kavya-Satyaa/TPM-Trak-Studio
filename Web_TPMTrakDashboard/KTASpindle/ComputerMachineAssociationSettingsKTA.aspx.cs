using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GenericAndon.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.KTASpindle
{
    public partial class ComputerMachineAssociationSettingsKTA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindComputerName();
                BindPlantID();
                BindCellID();
                BindGrid();
                BindDisplayScreen();
            }

        }

        private void BindComputerName()
        {
            List<string> list = DBAccess.GetComputerNames();

            StringBuilder builder = new System.Text.StringBuilder();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    builder.Append(String.Format("<option value='{0}'>", list[i].ToString()));
                }
            }
            dlComputerName.InnerHtml = builder.ToString();
        }

        private void BindPlantID()
        {
            List<string> plantID = new List<string>();
            try
            {
                plantID = AndonDBAccess.getPlantID();
                ddlPlantID.DataSource = plantID;
                ddlPlantID.DataBind();
                ddlPlantID.Items.Insert(0, "All");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindCellID()
        {
            List<string> cellID = new List<string>();
            try
            {
                cellID = AndonDBAccess.getCellID(ddlPlantID.SelectedValue.ToString().Equals("All") ? "" : ddlPlantID.SelectedValue.ToString());
                ddlCellID.DataSource = cellID;
                ddlCellID.DataBind();
                ddlCellID.Items.Insert(0, "All");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

        }

        private void BindDisplayScreen()
        {
            string ComputerName = txtComputerName.Text;
            try
            {
                DataTable dt = DBAccess.GetDisplayScreenData(ComputerName);
                lvDisplayOptions.DataSource = dt;
                lvDisplayOptions.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellID();
        }

        protected void BtnView_Click(object sender, EventArgs e)
        {
            Session["ComputerName"] = txtComputerName.Text.Trim(); ;
            BindGrid();
            BindDisplayScreen();
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string computerName = txtComputerName.Text.Trim();
                Session["ComputerName"] = computerName;
                bool success = false;
                if (computerName != "")
                {
                    if (ddlRunOptions.SelectedValue == "RunByCell")
                    {
                        foreach (GridViewRow row in gvMachineDetailsCellWise.Rows)
                        {
                            string CellID = (row.FindControl("lblCellID") as Label).Text;
                            if ((row.FindControl("chkAssigned") as CheckBox).Checked)
                            {
                                success = DBAccess.UpdateInsertMachineComputerData("ComputerCellAssociation", computerName, CellID);
                            }
                            else
                            {
                                success = DBAccess.DeleteMachineComputerData("ComputerCellAssociation", computerName, CellID);
                            }
                        }
                        if (success)
                        {
                            HelperClassGeneric.openSuccessModal(this, "Saved Successfully");
                        }
                        else
                        {
                            HelperClassGeneric.openInsertErrorModal(this);
                        }
                    }
                    else if (ddlRunOptions.SelectedValue == "RunByMachine")
                    {

                        //DBAccess.SaveRunOptions(computerName, ddlRunOptions.SelectedValue);
                        foreach (GridViewRow row in gvMachineDetails.Rows)
                        {
                            string machineID = (row.FindControl("lblMachineID") as Label).Text;
                            if ((row.FindControl("chkAssigned") as CheckBox).Checked)
                            {
                                success = DBAccess.UpdateInsertMachineComputerData("ComputerMachineAssociation", computerName, machineID);
                            }
                            else
                            {
                                success = DBAccess.DeleteMachineComputerData("ComputerMachineAssociation", computerName, machineID);
                            }
                        }
                        if (success)
                        {
                            HelperClassGeneric.openSuccessModal(this, "Saved Successfully");
                        }
                        else
                        {
                            HelperClassGeneric.openInsertErrorModal(this);
                        }
                    }

                    foreach (ListViewDataItem item in lvDisplayOptions.Items)
                    {
                        string DisplayOption = (item.FindControl("lblOption") as Label).Text;
                        if ((item.FindControl("chkAssigned") as CheckBox).Checked)
                        {
                            success = DBAccess.InsertDisplayScreenSettings(computerName, DisplayOption);
                        }
                        else
                        {
                            success = DBAccess.DeleteDisplayScreenSettings(computerName, DisplayOption);
                        }

                    }
                }
                else
                {
                    HelperClassGeneric.openErrorModal(this, "Computer Name can't be empty");
                }

                BindGrid();
                BindDisplayScreen();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void BindGrid()
        {
            try
            {
                //string RunOption = AndonDBAccess.GetRunOption(txtComputerName.Text);
                //ddlRunOptions.SelectedValue = RunOption;

                if (Session["ComputerName"] != null)
                    txtComputerName.Text = Session["ComputerName"].ToString();
                DataTable dt = new DataTable();
                if (ddlRunOptions.SelectedValue == "RunByCell")
                {
                    dt = DBAccess.ComputerMachineAssociationSettingsData("ComputerCellAssociation", ddlPlantID.SelectedValue.ToString().Equals("All") ? "" : ddlPlantID.SelectedValue.ToString(), "", txtComputerName.Text);
                    gvMachineDetailsCellWise.DataSource = dt;
                    gvMachineDetailsCellWise.DataBind();
                    gvMachineDetails.Visible = false;
                    gvMachineDetailsCellWise.Visible = true;
                }
                else
                {
                    dt = DBAccess.ComputerMachineAssociationSettingsData("ComputerMachineAssociation", ddlPlantID.SelectedValue.ToString().Equals("All") ? "" : ddlPlantID.SelectedValue.ToString(), ddlCellID.SelectedValue.ToString().Equals("All") ? "" : ddlCellID.SelectedValue.ToString(), txtComputerName.Text);
                    gvMachineDetails.DataSource = dt;
                    gvMachineDetails.DataBind();
                    gvMachineDetailsCellWise.Visible = false;
                    gvMachineDetails.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlRunOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                if (ddlRunOptions.SelectedValue == "RunByCell")
                {
                    dt = DBAccess.ComputerMachineAssociationSettingsData("ComputerCellAssociation", ddlPlantID.SelectedValue.ToString().Equals("All") ? "" : ddlPlantID.SelectedValue.ToString(), "", txtComputerName.Text);
                    gvMachineDetailsCellWise.DataSource = dt;
                    gvMachineDetailsCellWise.DataBind();
                    gvMachineDetails.Visible = false;
                    gvMachineDetailsCellWise.Visible = true;

                }
                else
                {
                    dt = DBAccess.ComputerMachineAssociationSettingsData("ComputerMachineAssociation", ddlPlantID.SelectedValue.ToString().Equals("All") ? "" : ddlPlantID.SelectedValue.ToString(), ddlCellID.SelectedValue.ToString().Equals("All") ? "" : ddlCellID.SelectedValue.ToString(), txtComputerName.Text);
                    gvMachineDetails.DataSource = dt;
                    gvMachineDetails.DataBind();
                    gvMachineDetailsCellWise.Visible = false;
                    gvMachineDetails.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}