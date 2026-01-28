using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.SKS.Model;

namespace Web_TPMTrakDashboard.SKS
{
    public partial class ProdDownRejectionData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindShiftID();
                BindMachineID();
                btnView_Click(null, null);
            }
        }
        private void BindShiftID()
        {
            try
            {
                List<string> list = DataBaseAccess.shiftdetail();
                list.Insert(0, "All");
                ddlShift.DataSource = list;
                ddlShift.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindMachineID()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllMachines("");
                list.Insert(0, "All");
                ddlMachineId.DataSource = list;
                ddlMachineId.DataBind();
                BindPartID();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindPartID()
        {
            try
            {
                List<string> list = DataBaseAccess.GetComponentIDByMachine(ddlMachineId.SelectedValue);
                list.Insert(0, "All");
                //ddlPartID.DataSource = list;
                //ddlPartID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void ddlMachineId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPartID();
        }

        protected void btnProduction_Click(object sender, EventArgs e)
        {
            hdnSelectedMenu.Value = "Production";
            BindProductionData();
        }
        private void BindProductionData()
        {
            try
            {
                //DataTable dt = SKSDatabaseAccess.getProdDownRejectionData(txtFromDate.Text, txtToDate.Text, ddlShift.SelectedValue, ddlMachineId.SelectedValue, ddlPartID.SelectedValue, "Production");
                DataTable dt = SKSDatabaseAccess.getProdDownRejectionData(txtFromDate.Text, txtToDate.Text, ddlShift.SelectedValue, ddlMachineId.SelectedValue, "", "Production", txtPartDescSearch.Text);
                Session["ProdDataSKS"] = dt;
                lvProductionData.DataSource = dt;
                lvProductionData.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void btnDown_Click(object sender, EventArgs e)
        {
            hdnSelectedMenu.Value = "Down";
            BindDownData();
        }
        private void BindDownData()
        {
            try
            {
                DataTable dt = SKSDatabaseAccess.getProdDownRejectionData(txtFromDate.Text, txtToDate.Text, ddlShift.SelectedValue, ddlMachineId.SelectedValue, "", "Down", txtPartDescSearch.Text);
                Session["DownDataSKS"] = dt;
                lvDownData.DataSource = dt;
                lvDownData.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void btnRejection_Click(object sender, EventArgs e)
        {
            hdnSelectedMenu.Value = "Rejection";
            BindRejectionData();
        }
        private void BindRejectionData()
        {
            try
            {
                DataTable dt = SKSDatabaseAccess.getProdDownRejectionData(txtFromDate.Text, txtToDate.Text, ddlShift.SelectedValue, ddlMachineId.SelectedValue, "", "Rejection", txtPartDescSearch.Text);
                Session["RejectionDataSKS"] = dt;
                lvRejectionData.DataSource = dt;
                lvRejectionData.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindProductionData();
            BindDownData();
            BindRejectionData();
        }

        private void BindPaginationData()
        {
            try
            {
                DataTable dt = new DataTable();
                if (hdnSelectedMenu.Value.Equals("Rejection", StringComparison.OrdinalIgnoreCase))
                {
                    dt = Session["RejectionDataSKS"] as DataTable;
                    lvRejectionData.DataSource = dt;
                    lvRejectionData.DataBind();
                }
                else if (hdnSelectedMenu.Value.Equals("Down", StringComparison.OrdinalIgnoreCase))
                {
                    dt = Session["DownDataSKS"] as DataTable;
                    lvDownData.DataSource = dt;
                    lvDownData.DataBind();
                }
                else
                {
                    dt = Session["ProdDataSKS"] as DataTable;
                    lvProductionData.DataSource = dt;
                    lvProductionData.DataBind();

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void lvProductionDataPager_PreRender(object sender, EventArgs e)
        {
            BindPaginationData();
        }
        protected void lvDownDataPager_PreRender(object sender, EventArgs e)
        {
            BindPaginationData();
        }
        protected void lvRejectionDataPager_PreRender(object sender, EventArgs e)
        {
            BindPaginationData();
        }

    }
}