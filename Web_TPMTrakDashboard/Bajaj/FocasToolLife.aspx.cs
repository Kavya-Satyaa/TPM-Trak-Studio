using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Bajaj
{
    public partial class FocasToolLife : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindPlant();
                BindToolLifeDetails();
            }
        }
        private void BindPlant()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlantsForPlantInfo();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();

                BindCell();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPlant: " + ex.Message);
            }
        }
        private void BindCell()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllGroupId(ddlPlant.SelectedValue);
                ddlCell.DataSource = list;
                ddlCell.DataBind();

                BindMachine();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindCell: " + ex.Message);
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllMachinedByLineandGroup(ddlPlant.SelectedValue, ddlCell.SelectedValue);
                if (list.Count > 0)
                {
                    list.Insert(0, "All");
                }
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachine: " + ex.Message);
            }
        }
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindCell();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlPlant_SelectedIndexChanged: " + ex.Message);
            }
        }

        protected void ddlCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindMachine();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlCell_SelectedIndexChanged: " + ex.Message);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindToolLifeDetails();
        }

        private void BindToolLifeDetails()
        {
            try
            {
                ViewState["FromDate"] = txtFromDate.Text;
                ViewState["ToDate"] = txtToDate.Text;
                ViewState["Machine"] = ddlMachine.SelectedValue;
                ViewState["Plant"] = ddlPlant.SelectedValue;
                List<FocasToolLifeDetails> list = BajajDBAccess.GetToolLifeDetails(ViewState["FromDate"].ToString(), ViewState["ToDate"].ToString(), ViewState["Plant"].ToString(), ViewState["Machine"].ToString());
                lvToolLifeDetails.DataSource = list;
                lvToolLifeDetails.DataBind();
                Session["ToolLifeDetails"] = list;

            }catch(Exception ex)
            {
                Logger.WriteErrorLog("BindToolLifeDetails: " + ex.Message);
            }
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                bool result = GenerateReports.GenerateFocasToolLifeReport(ViewState["Plant"].ToString(), ViewState["Machine"].ToString(),ViewState["FromDate"].ToString(), ViewState["ToDate"].ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnExport_Click: " + ex.Message);
            }
        }
    }
}