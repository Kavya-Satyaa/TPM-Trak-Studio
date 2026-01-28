using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class PlantInformation : System.Web.UI.Page
    {
        DataTable dtPlantInfo = new DataTable();
        string Plant_ID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Header.DataBind();
            if (!IsPostBack)
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                else
                    SessionClear.ClearSession();
                BindPlantInformation();
            }
        }

        private void BindPlantInformation()
        {
            dtPlantInfo = DataBaseAccess.GetAllPlantsInfo();
            GridPlantInfo.DataSource = dtPlantInfo;
            GridPlantInfo.DataBind();
            if (dtPlantInfo.Rows.Count > 0)
            {
                GridViewRow row = GridPlantInfo.Rows[0];
                row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                Plant_ID = ((TextBox)GridPlantInfo.Rows[row.RowIndex].Cells[1].FindControl("txtPlantID")).Text;
                Session["Plant_ID"] = Plant_ID;
                BindAssignedMachineIDs(string.IsNullOrEmpty(Plant_ID) ? "" : Plant_ID);
                BindUnAssignedMachineIDs(string.IsNullOrEmpty(Plant_ID) ? "" : Plant_ID);
            }
        }

        private void BindAssignedMachineIDs(string PlantId)
        {
            List<string> assignedMachineID = DataBaseAccess.GetAssignedAndUnAssignedMachineIds("select distinct MachineID from PlantMachine where PlantID = @PlantId", PlantId);
            if (chkassigned.Items.Count > 0) chkassigned.Items.Clear();
            if (assignedMachineID.Count <= 0) return;
            foreach (string strMachine in assignedMachineID)
            {
                chkassigned.Items.Add(strMachine);
            }
        }

        private void BindUnAssignedMachineIDs(string PlantId)
        {
            List<string> UnassignedMachineID = DataBaseAccess.GetAssignedAndUnAssignedMachineIds("select distinct machineID  from [machineinformation] where machineID NOT in (select distinct MachineID from PlantMachine)", PlantId);
            if (chkaveliable.Items.Count > 0) chkaveliable.Items.Clear();
            if (UnassignedMachineID.Count <= 0) return;
            foreach (string strMachine in UnassignedMachineID)
            {
                chkaveliable.Items.Add(strMachine);
            }
        }

        protected void btnassign_Click(object sender, EventArgs e)
        {
            if (chkaveliable.SelectedIndex > -1)
            {
                foreach (ListItem item in chkaveliable.Items)
                {
                    if (item.Selected)
                    {
                        string selectedValue = item.Value;
                        DataBaseAccess.AddMachineToPlant(selectedValue, string.IsNullOrEmpty(Session["Plant_ID"].ToString()) ? Plant_ID : Session["Plant_ID"].ToString());
                    }
                }
            }
            BindAssignedMachineIDs(string.IsNullOrEmpty(Session["Plant_ID"].ToString()) ? Plant_ID : Session["Plant_ID"].ToString());
            BindUnAssignedMachineIDs("");
        }

        protected void btnunassign_Click(object sender, EventArgs e)
        {
            if (chkassigned.SelectedIndex > -1)
            {
                foreach (ListItem item in chkassigned.Items)
                {
                    if (item.Selected)
                    {
                        string selectedValue = item.Value;
                        DataBaseAccess.DeleteMachineToPlant(selectedValue, string.IsNullOrEmpty(Session["Plant_ID"].ToString()) ? Plant_ID : Session["Plant_ID"].ToString());
                    }
                }
            }
            BindAssignedMachineIDs(string.IsNullOrEmpty(Session["Plant_ID"].ToString()) ? Plant_ID : Session["Plant_ID"].ToString());
            BindUnAssignedMachineIDs("");
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            btncancel.Visible = true;
            btnNew.Visible = false;
            AddNewRowToGrid();
        }

        private void AddNewRowToGrid()
        {
            BindPlantInformation();
            DataTable dt = new DataTable();
            dt.Columns.Add("PlantID");
            dt.Columns.Add("Description");
            dt.Columns.Add("PlantCode");
            foreach (GridViewRow gvRow in GridPlantInfo.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["PlantID"] = ((TextBox)gvRow.FindControl("txtPlantID")).Text;
                dr["Description"] = ((TextBox)gvRow.FindControl("txtPlantDesc")).Text;
                dr["PlantCode"] = ((TextBox)gvRow.FindControl("txtPlantCode")).Text;
                dt.Rows.Add(dr);
            }

            DataRow dr1 = dt.NewRow();
            dr1["PlantID"] = "";
            dr1["Description"] = "";
            dr1["PlantCode"] = "";
            dt.Rows.Add(dr1);
            GridPlantInfo.DataSource = dt;
            GridPlantInfo.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool res = false;
            int rowcount = 0, i = 1, Rowsvalue = -1;
            string PlantId = string.Empty, PlantDesc = string.Empty, PlantCode = string.Empty, Msg = string.Empty;
            rowcount = GridPlantInfo.Rows.Count;
            foreach (GridViewRow row in GridPlantInfo.Rows)
            {
                Rowsvalue++;
                PlantId = ((TextBox)row.FindControl("txtPlantID")).Text;
                PlantDesc = ((TextBox)row.FindControl("txtPlantDesc")).Text;
                PlantCode = ((TextBox)row.FindControl("txtPlantCode")).Text;
                for(int j=0;j<GridPlantInfo.Rows.Count;j++)
                {
                    if(!(j == Rowsvalue))
                    {
                        if(PlantCode == (((TextBox)GridPlantInfo.Rows[j].FindControl("txtPlantCode")).Text))
                        {
                            lblMessages.ForeColor = System.Drawing.Color.Red;
                            lblMessages.Text = "Dulpicate Plant Code Cannot be inserted";
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                            return;
                        }
                    }
                }
                if (i == rowcount)
                {
                    if (Convert.ToInt32(PlantCode) <= 0)
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = "Plant Code Cannot be zero";
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                        return;
                    }
                    if (!string.IsNullOrEmpty(PlantId) && !string.IsNullOrEmpty(PlantDesc) && !string.IsNullOrEmpty(PlantCode))
                    {
                        res = DataBaseAccess.UpdatePlantInfo(PlantId, PlantDesc, PlantCode, out Msg);
                    }
                    else
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = GetGlobalResourceObject("CommanResource", "EnterAllValues").ToString();
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    }
                }
                else
                {
                    if(Convert.ToInt32(PlantCode) <= 0)
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text ="Plant Code Cannot be zero";
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                        return;
                    }
                    if (!string.IsNullOrEmpty(PlantId) && !string.IsNullOrEmpty(PlantDesc) && !string.IsNullOrEmpty(PlantCode))
                    {
                        res = DataBaseAccess.UpdatePlantInfo(PlantId, PlantDesc, PlantCode, out Msg);
                    }
                    else
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = GetGlobalResourceObject("CommanResource", "EnterAllValues").ToString();
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    }
                }
                if (res)
                {
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                    lblMessages.Text = GetGlobalResourceObject("CommanResource", "RecordUpdateSuccessfully").ToString();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }
                i++;
            }
            BindPlantInformation();
            btncancel.Visible = false;
            btnNew.Visible = true;
            SetFocus(btnSave);
        }

        protected void GridPlantInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var SelectButton = e.Row.FindControl("btnSelect") as Button;
                SelectButton.ToolTip = GetGlobalResourceObject("CommanResource", "SelectToolTip").ToString();
            }
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            try
            {
                btnNew.Visible = true;
                btncancel.Visible = false;
                BindPlantInformation();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
        }

        protected void GridPlantInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                foreach (GridViewRow row in GridPlantInfo.Rows)
                {
                    if (row.RowIndex == index)
                    {
                        row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                        Plant_ID = ((TextBox)GridPlantInfo.Rows[row.RowIndex].Cells[1].FindControl("txtPlantID")).Text;
                        Session["Plant_ID"] = Plant_ID;
                        BindAssignedMachineIDs(string.IsNullOrEmpty(Plant_ID) ? "" : Plant_ID);
                        BindUnAssignedMachineIDs(string.IsNullOrEmpty(Plant_ID) ? "" : Plant_ID);
                    }
                    else
                    {
                        row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                        var SelectButton = row.FindControl("btnSelect") as Button;
                        SelectButton.ToolTip = GetGlobalResourceObject("CommanResource", "SelectToolTip").ToString();
                    }
                }
            }
        }
    }
}