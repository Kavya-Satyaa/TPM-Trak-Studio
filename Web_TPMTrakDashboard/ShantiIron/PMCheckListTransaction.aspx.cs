using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.ShantiIron
{
    public partial class PMCheckListTransaction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!(IsPostBack))
            {
                // BindPlant();
                //BindCell();
                BindMachineID();
                LoadItems();
            }
        }

        //private void BindPlant()
        //{
        //    try
        //    {
        //        List<string> PlantID = BindCockpitView.ViewPlantToDisplay();
        //        if (PlantID != null && PlantID.Count > 0)
        //        {
        //            ddlplantID.DataSource = PlantID;
        //            ddlplantID.DataBind();
        //            ddlplantID.SelectedIndex = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex);
        //    }
        //}

        //private void BindCell()
        //{
        //    try
        //    {
        //        List<string> CellID = BindCockpitView.ViewCellsToDisplay(ddlplantID.SelectedValue.ToString());
        //        if (CellID != null && CellID.Count > 0)
        //        {
        //            ddlcellID.DataSource = CellID;
        //            ddlcellID.DataBind();
        //            ddlcellID.SelectedIndex = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.ToString());
        //    }
        //}

        private void BindMachineID()
        {
            List<string> MachineID = VDGDataBaseAccess.GetMachinesbyPlantCell("", ConfigurationManager.AppSettings["ShanthiCell"].ToString());
            ddlMachineID.DataSource = MachineID;
            ddlMachineID.DataBind();
        }

        private void LoadItems()
        {
            try
            {

                string Enddate = "";
                string PMStartDate = AlertModule.DataBaseAccess.GetPMDates(ddlMachineID.SelectedValue.ToString(), out Enddate);
                if (string.IsNullOrEmpty(PMStartDate)) btnUpdatePMDate.Visible = true;
                else btnUpdatePMDate.Visible = false;
                List<PMTransactionEntity> Entity = new List<PMTransactionEntity>();
                if (!(string.IsNullOrEmpty(PMStartDate) && string.IsNullOrEmpty(Enddate)))
                    Entity = AlertModule.DataBaseAccess.GetPMTransactionData(ddlMachineID.SelectedValue.ToString(), PMStartDate, Enddate);
                if (Entity != null && Entity.Count > 0)
                {
                    gridViewTransaction.DataSource = Entity;
                    gridViewTransaction.DataBind();
                }
                else
                {
                    gridViewTransaction.DataSource = Entity;
                    gridViewTransaction.DataBind();
                }

                txtDate.Enabled = string.IsNullOrEmpty(PMStartDate) ? true : false;
                txtDate.Text = string.IsNullOrEmpty(PMStartDate) ? "" : Convert.ToDateTime(PMStartDate).ToString("dd-MM-yyyy HH:mm");

                txtPMENdDate.Text = string.IsNullOrEmpty(Enddate) ? DateTime.Now.ToString("dd-MM-yyyy HH:mm") : Convert.ToDateTime(Enddate).ToString("dd-MM-yyyy HH:mm");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        //protected void ddlplantID_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindCell();
        //}

        //protected void ddlcellID_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindMachineID();
        //}

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            LoadItems();
        }

        protected void btnPostponePMDate_Click(object sender, EventArgs e)
        {
            DateTime Enddate = Util.GetDateTime(txtPMENdDate.Text);
            DateTime Startdate = Util.GetDateTime(txtDate.Text);
            if (Enddate > DateTime.Now)
            {
                AlertModule.DataBaseAccess.UpdatePMEndDate(ddlMachineID.SelectedValue.ToString(), Enddate, Startdate, "Postponed");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Date Postponded Successfully')", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Postpone')", true);
            LoadItems();
        }

        protected void btnSaveTransaction_Click(object sender, EventArgs e)
        {
            string Enddate = "";
            string PMStartDate = AlertModule.DataBaseAccess.GetPMDates(ddlMachineID.SelectedValue.ToString(), out Enddate);

            if (Convert.ToDateTime(Enddate) >= DateTime.Now && DateTime.Now > Convert.ToDateTime(PMStartDate))
            {
                if (DateTime.Now > Convert.ToDateTime(PMStartDate))
                {
                    if (!(string.IsNullOrEmpty(PMStartDate) || string.IsNullOrEmpty(Enddate)))
                    {
                        string MachineID = string.Empty, IDD = string.Empty, Catagory = string.Empty, Items = string.Empty, OkNotOK = string.Empty, Remarks = string.Empty;
                        foreach (GridViewRow Row in gridViewTransaction.Rows)
                        {
                            MachineID = (Row.FindControl("lblMachine") as HiddenField).Value;
                            IDD = (Row.FindControl("hidIDD") as HiddenField).Value;
                            Catagory = (Row.FindControl("lblCategory") as Label).Text;
                            Items = (Row.FindControl("txtItem") as Label).Text;
                            Remarks = (Row.FindControl("txtRemarks") as TextBox).Text;
                            OkNotOK = (Row.FindControl("chkOK") as CheckBox).Checked == true ? "OK" : (Row.FindControl("chkNotOK") as CheckBox).Checked == true ? "NotOK" : "";
                            if (string.IsNullOrEmpty(OkNotOK))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please fill all the transaction to proceed')", true);
                                return;
                            }
                            AlertModule.DataBaseAccess.UpdateTransaction(MachineID, Catagory, Items, Remarks, OkNotOK, IDD);
                        }
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "ModelNextDateShow()", true);
                        AlertModule.DataBaseAccess.UpdatePMDatainSchedule(string.IsNullOrEmpty(MachineID) ? ddlMachineID.SelectedValue.ToString() : MachineID, "PM Report (Phantom Cell)");
                        LoadItems();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('PLease save PM Start Date and EndDate before saving transaction')", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot insert data if start date is Greater than today!!')", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Has to postpone Next Date before saving transaction')", true);
            }
        }



        protected void btnUpdatePMDate_Click(object sender, EventArgs e)
        {

            DateTime Enddate = Util.GetDateTime(txtPMENdDate.Text); bool success = false;
            string Startdate = AlertModule.DataBaseAccess.GetLastPMDAte(ddlMachineID.SelectedValue.ToString());
            bool status = false;
            if (string.IsNullOrEmpty(Startdate))
                status = true;
            else
                status = AlertModule.DataBaseAccess.checkPMCheckliststatus(Util.GetDateTime(txtDate.Text), Convert.ToDateTime(Startdate), ddlMachineID.SelectedValue.ToString());
            if (status)
            {

                if (string.IsNullOrEmpty(Startdate) && Enddate > Util.GetDateTime(txtDate.Text))
                {

                    success = AlertModule.DataBaseAccess.UpdatePMEndDate(ddlMachineID.SelectedValue.ToString(), Enddate, Util.GetDateTime(txtDate.Text), "Update");

                }
                else if (Enddate > DateTime.Now && Enddate > Convert.ToDateTime(Startdate))
                {
                    success = AlertModule.DataBaseAccess.UpdatePMEndDate(ddlMachineID.SelectedValue.ToString(), Enddate, Convert.ToDateTime(Startdate), "Update");
                }
                if (success)
                {
                    LoadItems();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('PM Date'sSaved!!')", true);
                }

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Fill all transaction before saving!!')", true);
                return;
            }
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            bool success = false, status = false;
            DateTime Enddate = DateTime.Now;
            if (string.IsNullOrEmpty(txtNextPmCheck.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Date Cannot be Empty!!')", true);
                return;
            }
            else
            {
                DateTime Startdate = DateTime.Now;
                Enddate = Util.GetDateTime(txtNextPmCheck.Text);
                if (Enddate > DateTime.Now)
                {
                    success = AlertModule.DataBaseAccess.UpdatePMEndDate(ddlMachineID.SelectedValue.ToString(), Enddate, Startdate, "Update");
                }
                if (success)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('PM Date'sSaved!!')", true);
                    LoadItems();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('PM Start Date Greater than Next date!!')", true);
                    return;
                }
            }
        }


        protected void chkOK_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            int rowindex = 0;
            string[] arr = chk.ClientID.Split('_');
            string abc = arr[arr.Length - 1];
            if (!string.IsNullOrEmpty(abc))
                int.TryParse(abc, out rowindex);
            if (chk.Checked)
                (gridViewTransaction.Rows[rowindex].FindControl("chkNotOK") as CheckBox).Checked = false;
        }

        protected void chkNotOK_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            int rowindex = 0;
            string[] arr = chk.ClientID.Split('_');
            string abc = arr[arr.Length - 1];
            if (!string.IsNullOrEmpty(abc))
                int.TryParse(abc, out rowindex);
            if (chk.Checked)
                (gridViewTransaction.Rows[rowindex].FindControl("chkOK") as CheckBox).Checked = false;
        }
    }

    public class PMTransactionEntity
    {
        public string MachineID { get; set; }
        public string IDD { get; set; }
        public string Category { get; set; }
        public string Items { get; set; }
        public bool OkData { get; set; }
        public bool NotOkData { get; set; }
        public string Remarks { get; set; }
    }
}