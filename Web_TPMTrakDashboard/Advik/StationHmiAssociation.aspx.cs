using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.Advik
{
    public partial class StationHmiAssociation : System.Web.UI.Page
    {
        DataTable dtHMIInfo = new DataTable();
        string HMI_ID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Header.DataBind();
            if (!IsPostBack)
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                else
                    SessionClear.ClearSession();
                BindHMIInformation();
            }
        }

        private void BindHMIInformation()
        {
            dtHMIInfo = DataBaseAccess.AdvikDatabaseAccess.GetAllHMIInfo();
            GridHMIInfo.DataSource = dtHMIInfo;
            GridHMIInfo.DataBind();
            if (dtHMIInfo.Rows.Count > 0)
            {
                GridViewRow row = GridHMIInfo.Rows[0];
                row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                HMI_ID = ((TextBox)GridHMIInfo.Rows[row.RowIndex].Cells[1].FindControl("txtHMIID")).Text;
                Session["HMI_ID"] = HMI_ID;
                BindAssignedMachineIDs(string.IsNullOrEmpty(HMI_ID) ? "" : HMI_ID);
                BindUnAssignedMachineIDs(string.IsNullOrEmpty(HMI_ID) ? "" : HMI_ID);
            }
        }

        private void BindAssignedMachineIDs(string HMIId)
        {
            List<string> assignedMachineID = DataBaseAccess.AdvikDatabaseAccess.GetAssignedAndUnAssignedMachineIds("select distinct MachineID from HMIMachine where HMIID = @HMIId", HMIId);
            if (chkassigned.Items.Count > 0) chkassigned.Items.Clear();
            if (assignedMachineID.Count <= 0) return;
            foreach (string strMachine in assignedMachineID)
            {
                chkassigned.Items.Add(strMachine);
            }
        }

        private void BindUnAssignedMachineIDs(string HMIId)
        {
            List<string> UnassignedMachineID = DataBaseAccess.AdvikDatabaseAccess.GetAssignedAndUnAssignedMachineIds("select distinct machineID  from [machineinformation] where machineID NOT in (select distinct MachineID from HMIMachine)", HMIId);
            if (chkaveliable.Items.Count > 0) chkaveliable.Items.Clear();
            if (UnassignedMachineID.Count <= 0) return;
            foreach (string strMachine in UnassignedMachineID)
            {
                chkaveliable.Items.Add(strMachine);
            }
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
                        DataBaseAccess.AdvikDatabaseAccess.DeleteMachineToHMI(selectedValue, string.IsNullOrEmpty(Session["HMI_ID"].ToString()) ? HMI_ID : Session["HMI_ID"].ToString());
                    }
                }
            }
            BindAssignedMachineIDs(string.IsNullOrEmpty(Session["HMI_ID"].ToString()) ? HMI_ID : Session["HMI_ID"].ToString());
            BindUnAssignedMachineIDs("");
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
                        DataBaseAccess.AdvikDatabaseAccess.AddMachineToHMI(selectedValue, string.IsNullOrEmpty(Session["HMI_ID"].ToString()) ? HMI_ID : Session["HMI_ID"].ToString());
                    }
                }
            }
            BindAssignedMachineIDs(string.IsNullOrEmpty(Session["HMI_ID"].ToString()) ? HMI_ID : Session["HMI_ID"].ToString());
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
            BindHMIInformation();
            DataTable dt = new DataTable();
            dt.Columns.Add("HMIID");
            dt.Columns.Add("Description");
            dt.Columns.Add("HMICode");
            foreach (GridViewRow gvRow in GridHMIInfo.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["HMIID"] = ((TextBox)gvRow.FindControl("txtHMIID")).Text;
                dr["Description"] = ((TextBox)gvRow.FindControl("txtHMIDesc")).Text;
                dr["HMICode"] = ((TextBox)gvRow.FindControl("txtHMICode")).Text;
                dt.Rows.Add(dr);
            }

            DataRow dr1 = dt.NewRow();
            dr1["HMIID"] = "";
            dr1["Description"] = "";
            dr1["HMICode"] = "";
            dt.Rows.Add(dr1);
            GridHMIInfo.DataSource = dt;
            GridHMIInfo.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool res = false;
            int rowcount = 0, i = 1, Rowsvalue = -1;
            string HMIId = string.Empty, HMIDesc = string.Empty, HMICode = string.Empty, Msg = string.Empty;
            rowcount = GridHMIInfo.Rows.Count;
            foreach (GridViewRow row in GridHMIInfo.Rows)
            {
                Rowsvalue++;
                HMIId = ((TextBox)row.FindControl("txtHMIID")).Text;
                HMIDesc = ((TextBox)row.FindControl("txtHMIDesc")).Text;
                HMICode = ((TextBox)row.FindControl("txtHMICode")).Text;
                for (int j = 0; j < GridHMIInfo.Rows.Count; j++)
                {
                    if (!(j == Rowsvalue))
                    {
                        if (HMICode == (((TextBox)GridHMIInfo.Rows[j].FindControl("txtHMICode")).Text))
                        {
                            lblMessages.ForeColor = System.Drawing.Color.Red;
                            lblMessages.Text = "Dulpicate HMI Code Cannot be inserted";
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                            return;
                        }
                    }
                }
                if (i == rowcount)
                {
                    if (Convert.ToInt32(HMICode) <= 0)
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = "HMI Code Cannot be zero";
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                        return;
                    }
                    if (!string.IsNullOrEmpty(HMIId) && !string.IsNullOrEmpty(HMIDesc) && !string.IsNullOrEmpty(HMICode))
                    {
                        res = DataBaseAccess.AdvikDatabaseAccess.UpdateHMIInfo(HMIId, HMIDesc, HMICode, out Msg);
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
                    if (Convert.ToInt32(HMICode) <= 0)
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = "HMI Code Cannot be zero";
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                        return;
                    }
                    if (!string.IsNullOrEmpty(HMIId) && !string.IsNullOrEmpty(HMIDesc) && !string.IsNullOrEmpty(HMICode))
                    {
                        res = DataBaseAccess.AdvikDatabaseAccess.UpdateHMIInfo(HMIId, HMIDesc, HMICode, out Msg);
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
            BindHMIInformation();
            btncancel.Visible = false;
            btnNew.Visible = true;
            SetFocus(btnSave);
        }

        protected void GridHMIInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var SelectButton = e.Row.FindControl("btnSelect") as Button;
                SelectButton.ToolTip = GetGlobalResourceObject("CommanResource", "SelectToolTip").ToString();
            }
        }

        protected void GridHMIInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                foreach (GridViewRow row in GridHMIInfo.Rows)
                {
                    if (row.RowIndex == index)
                    {
                        row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                        HMI_ID = ((TextBox)GridHMIInfo.Rows[row.RowIndex].Cells[1].FindControl("txtHMIID")).Text;
                        Session["HMI_ID"] = HMI_ID;
                        BindAssignedMachineIDs(string.IsNullOrEmpty(HMI_ID) ? "" : HMI_ID);
                        BindUnAssignedMachineIDs(string.IsNullOrEmpty(HMI_ID) ? "" : HMI_ID);
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

        protected void btncancel_Click(object sender, EventArgs e)
        {
            try
            {
                btnNew.Visible = true;
                btncancel.Visible = false;
                BindHMIInformation();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
        }



    }
}