using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class AggregationPage : System.Web.UI.Page
    {
        public string UserName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Language"] == null || Session["connectionString"] == null || Session["UserName"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            else if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                UserName = Session["UserName"] == null ? "" : Session["UserName"].ToString();
                BindPlantID();
                BindGrid();
            }
        }

        private void BindPlantID()
        {
            try
            {
                List<string> PlantID = DataBaseAccess.GetAllPlants();
                cmbPlantId.DataSource = PlantID;
                cmbPlantId.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindGrid()
        {
            try
            {
                string PlantID = cmbPlantId.SelectedValue == null ? "" : (cmbPlantId.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase)) ? "" : cmbPlantId.SelectedValue.ToString();
                List<AggregatedDataEntry> Entity = DataBaseAccess.GetAggregatedData(PlantID);
                //Entity.Add(new AggregatedDataEntry { AggData = "N", MachineID = "CNC-01", LastAggDate = "2020-07-29" });
                gridview.DataSource = Entity;
                gridview.DataBind();
                UserName = Session["UserName"] == null ? "" : Session["UserName"].ToString();
                chkMachineAll_CheckedChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnAgregate_Click(object sender, EventArgs e)
        {
            string MachineID = string.Empty; bool status = false;
            foreach (GridViewRow row in gridview.Rows)
            {
                if ((row.FindControl("chkSelect") as CheckBox).Checked)
                {
                    DateTime FromDate = Util.GetDateTime(txtFromDate.Text);
                    DateTime ToDate = Util.GetDateTime(txtToDate.Text);
                    MachineID = "'" + (row.FindControl("MachineID") as Label).Text + "'";
                    string LastAggregatedDate = "";
                    if (string.IsNullOrEmpty((row.FindControl("lblLastAggDate") as Label).Text))
                    {
                        LastAggregatedDate = "";
                    }
                    else
                    {
                        LastAggregatedDate = Util.GetDateTime((row.FindControl("lblLastAggDate") as Label).Text).ToString("yyyy-MM-dd");
                    }
                    status = DataBaseAccess.AggregateData(MachineID, FromDate, ToDate, LastAggregatedDate, cmbPlantId.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : cmbPlantId.SelectedValue.ToString());


                }
            }
            if (status)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Aggregation Successfully for Machine:')", true);
                chkMachineAll.Checked = false;

            }
            UserName = Session["UserName"] == null ? "" : Session["UserName"].ToString();
            BindGrid();
        }

        protected void chkMachineAll_CheckedChanged(object sender, EventArgs e)
        {

            foreach (GridViewRow row in gridview.Rows)
            {
                CheckBox drp = row.FindControl("chkSelect") as CheckBox;
                drp.Checked = chkMachineAll.Checked;
            }
        }

        protected void cmbPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
            UserName = Session["UserName"] == null ? "" : Session["UserName"].ToString();
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            bool verify = Session["UserName"] == null ? false : DataBaseAccess.Getverification(Session["UserName"].ToString());
            if (verify)
            {
                foreach (GridViewRow row in gridview.Rows)
                {
                    if ((row.FindControl("chkSelect") as CheckBox).Checked)
                    {
                        string MachineID = (row.FindControl("MachineID") as Label).Text;
                        string LastAggregatedDate = "";
                        if (string.IsNullOrEmpty((row.FindControl("lblLastAggDate") as Label).Text))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Cannot delete aggregation when aggregation has not happened)", true);
                        }
                        else
                        {
                            LastAggregatedDate = Util.GetDateTime(txtFromDate.Text).ToString("yyyy-MM-dd");
                        }
                        DataBaseAccess.DeleteAggregatedData(@"Delete from shiftrejectiondetails where [id] in (select [id] from shiftproductiondetails where machineid ='" + MachineID + "' and pdate >='" + LastAggregatedDate + "')", MachineID, LastAggregatedDate);
                        DataBaseAccess.DeleteAggregatedData(@"Delete from shiftreworkdetails where [id] in (select [id] from shiftproductiondetails where machineid ='" + MachineID + "' and pdate >='" + LastAggregatedDate + "')", MachineID, LastAggregatedDate);
                        DataBaseAccess.DeleteAggregatedData(@"Delete from shiftproductiondetails where machineid ='" + MachineID + "' and pdate >='" + LastAggregatedDate + "'", MachineID, LastAggregatedDate);
                        DataBaseAccess.DeleteAggregatedData(@"Delete from shiftdowntimedetails where machineid ='" + MachineID + "' and ddate >='" + LastAggregatedDate + "'", MachineID, LastAggregatedDate);
                        DataBaseAccess.DeleteAggregatedData(@"Delete from shiftaggtrail where machineid='" + MachineID + "' and aggdate>='" + LastAggregatedDate + "'", MachineID, LastAggregatedDate);

                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Aggregation Deleted Successfully for machine')", true);
                BindGrid();
                UserName = Session["UserName"] == null ? "" : Session["UserName"].ToString();
            }
            txtPassword.Text = "";
        }
    }

    public class AggregatedDataEntry
    {
        public string MachineID { get; set; }
        public bool AggData { get; set; }
        public string LastAggDate { get; set; }
    }
}