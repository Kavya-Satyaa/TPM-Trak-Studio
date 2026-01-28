using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Web_TPMTrakDashboard.AlertModule;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.ShantiIron
{
    public partial class OperationBypass : System.Web.UI.Page
    {
        List<OperationByPassEntity> DataEntityList = new List<OperationByPassEntity>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!(IsPostBack))
            {
               
                BindComponent();
                LoadData();
            }
        }

        private void BindComponent()
        {
            ddlComponent.DataSource = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllComp();
            ddlComponent.DataBind();
            ddlOperation.DataSource = Web_TPMTrakDashboard.Models.DataBaseAccess.GetOperation("", ddlComponent.SelectedValue.ToString());
            ddlOperation.DataBind();
        }

        private void LoadData()
        {
            try
            {
                DataEntityList = Web_TPMTrakDashboard.Models.DataBaseAccess.GetDataForGrid(txtComp.Text, txtDate.Text);
                Session["OperationBypassDataList"] = DataEntityList;
                if (DataEntityList != null && DataEntityList.Count > 0)
                {
                    GridOperationBypass.DataSource = DataEntityList;
                    GridOperationBypass.DataBind();
                }
                else
                {
                    //DataEntityList.Add(new OperationByPassEntity { ComponentID = "", OperationNo = "", EffectiveFromDate = "", EffectiveToDate = "" });
                    GridOperationBypass.DataSource = new List<OperationByPassEntity>();
                    GridOperationBypass.DataBind();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridOperationBypass.Rows)
            {
                string ComponentID = (row.FindControl("lblComponentID") as System.Web.UI.WebControls.Label).Text;
                string Operation = (row.FindControl("lblOperationNo") as System.Web.UI.WebControls.Label).Text;
                string EffectivelblFromDate = ((row.FindControl("txtEffectivefrom") as System.Web.UI.WebControls.TextBox).Text);
                string EffectivelblToDate = ((row.FindControl("txtEffectiveto") as System.Web.UI.WebControls.TextBox).Text);
                string Hid = ((row.FindControl("hid") as System.Web.UI.WebControls.HiddenField).Value);
                if (Hid.Equals("Update", StringComparison.OrdinalIgnoreCase))
                {
                    DateTime EffectiveFromDate = DateTime.Now;
                    DateTime EffectiveToDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(EffectivelblFromDate))
                    {
                        EffectiveFromDate = Util.GetDateTime(EffectivelblFromDate);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select Effective From Date')", true);
                        return;
                    }
                    if (!string.IsNullOrEmpty(EffectivelblToDate))
                    {
                        EffectiveToDate = Util.GetDateTime(EffectivelblToDate);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select Effective To Date')", true);
                        return;
                    }
                    if (EffectiveToDate <= EffectiveFromDate)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Effective to should be greater than Effective from')", true);
                        return;
                    }
                    else
                    {
                        Web_TPMTrakDashboard.Models.DataBaseAccess.SaveComponentOperationByPass(ComponentID, Operation, EffectiveFromDate, EffectiveToDate);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Saved SUCCESSFULLY')", true);
                        LoadData();
                    }
                }
                //if (!(EffectivelblFromDate == EffectiveFromDate) || !(EffectivelblToDate == EffectiveToDate))
                //{

                //}
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string FooterCompID = ddlComponent.SelectedValue.ToString();
            string FooterOpnNo = ddlOperation.SelectedValue.ToString();
            string Effectivefrom = txtFooterEffectivefrom.Text;
            DateTime FooterEffectiveFromDate = DateTime.Now;
            DateTime FooterEffectiveToDate = DateTime.Now;
            string Effectiveto = txtFooterEffectiveto.Text;
            bool cansave = false;
            if (string.IsNullOrEmpty(Effectivefrom))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select Effective From Date')", true);
                return;
            }
            else
            {
                FooterEffectiveFromDate = Util.GetDateTime(Effectivefrom);
            }
            if (string.IsNullOrEmpty(Effectiveto))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select Effective To Date')", true);
                return;
            }
            else
            {
                FooterEffectiveToDate = Util.GetDateTime(Effectiveto);
            }
            if (FooterEffectiveToDate <= FooterEffectiveFromDate)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Effective to should be greater than Effective from')", true);
                return;
            }
            else if (Session["OperationBypassDataList"] != null)
            {
                FooterEffectiveFromDate = Util.GetDateTime(Effectivefrom);
                FooterEffectiveToDate = Util.GetDateTime(Effectiveto);
                DataEntityList = Session["OperationBypassDataList"] as List<OperationByPassEntity>;
                List<OperationByPassEntity> DataEntityListnew = DataEntityList.Where(x => (x.ComponentID == FooterCompID) && x.OperationNo == FooterOpnNo).ToList();
                if (DataEntityListnew != null && DataEntityListnew.Count > 0)
                {
                    foreach (OperationByPassEntity entity in DataEntityListnew)
                    {
                        DateTime GridLines = Util.GetDateTime(entity.EffectiveToDate);
                        if (!string.IsNullOrEmpty(entity.EffectiveToDate))
                        {
                            if (FooterEffectiveFromDate> Util.GetDateTime(entity.EffectiveToDate))
                            {
                                cansave = true;
                            }
                            else
                            {
                                cansave = false;
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Effective From date should be more than the effective date of previous effective to date')", true);
                                return;
                            }
                        }
                    }
                }
                if(cansave)
                {
                    Web_TPMTrakDashboard.Models.DataBaseAccess.SaveComponentOperationByPass(FooterCompID, FooterOpnNo, FooterEffectiveFromDate, FooterEffectiveToDate);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Saved SUCCESSFULLY')", true);
                    LoadData();
                }
            }
            else
            {
                foreach (GridViewRow row in GridOperationBypass.Rows)
                {
                    if (FooterCompID.Equals((row.FindControl("lblComponentID") as System.Web.UI.WebControls.Label).Text) && FooterOpnNo.Equals((row.FindControl("lblOperationNo") as System.Web.UI.WebControls.Label).Text))
                    {
                        if (string.IsNullOrEmpty((row.FindControl("txtEffectiveto") as System.Web.UI.WebControls.Label).Text)) return;
                        if (FooterEffectiveFromDate > Util.GetDateTime((row.FindControl("txtEffectiveto") as System.Web.UI.WebControls.TextBox).Text))
                        {
                            cansave = true;
                        }
                        else
                        {
                            cansave = false;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Effective From date should be more than the effective date of previous effective to date')", true);
                            return;
                        }
                    }
                }
                if(cansave)
                {
                    Web_TPMTrakDashboard.Models.DataBaseAccess.SaveComponentOperationByPass(FooterCompID, FooterOpnNo, FooterEffectiveFromDate, FooterEffectiveToDate);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Saved SUCCESSFULLY')", true);
                    LoadData();
                }
            }
        }

        protected void GridOperationBypass_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void ddlComponent_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlComponent.SelectedValue != null)
            {
                ddlOperation.DataSource = Web_TPMTrakDashboard.Models.DataBaseAccess.GetOperation("", ddlComponent.SelectedValue.ToString());
                ddlOperation.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void GridOperationBypass_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string ComponentID = string.Empty, Operation = string.Empty, EffectiveFrom = string.Empty, EffectiveTo = string.Empty;
            int RowIndex = 0; bool IsUpdated = false;
            try
            {
                switch (e.CommandName)
                {
                    case ("DeleteRow"):
                        RowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                        ComponentID = (GridOperationBypass.Rows[RowIndex].FindControl("lblComponentID") as System.Web.UI.WebControls.Label).Text;
                        Operation = (GridOperationBypass.Rows[RowIndex].FindControl("lblOperationNo") as System.Web.UI.WebControls.Label).Text;
                        EffectiveFrom = (GridOperationBypass.Rows[RowIndex].FindControl("lbleffectivefromdate") as System.Web.UI.WebControls.Label).Text;
                        EffectiveTo = (GridOperationBypass.Rows[RowIndex].FindControl("lbleffectivetodate") as System.Web.UI.WebControls.Label).Text;
                        if (!(string.IsNullOrEmpty(EffectiveFrom) && string.IsNullOrEmpty(EffectiveTo)))
                        {
                            Web_TPMTrakDashboard.Models.DataBaseAccess.DeleteOperationBypass(ComponentID, Operation, Util.GetDateTime(EffectiveFrom), Util.GetDateTime(EffectiveTo));
                            GridOperationBypass.EditIndex = -1;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Deleted Successfully')", true);
                        }
                        LoadData();
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    public class OperationByPassEntity
    {
        public string SLNO { get; set; }
        public string ComponentID { get; set; }
        public string OperationNo { get; set; }
        public string EffectiveFromDate { get; set; }
        public string EffectiveToDate { get; set; }
    }
}