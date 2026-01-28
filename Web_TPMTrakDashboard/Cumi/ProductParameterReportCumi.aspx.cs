using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Cumi.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Cumi
{
    public partial class ProductParameterReportCumi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    BindPlant();
                    BindShift();
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindPlant()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlantsForPlantInfo();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();

                BindMachine();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPlant: " + ex.Message);
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = CumiDBAccess.GetAllMachinedByPlant(ddlPlant.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue);
                lbMachine.DataSource = list;
                lbMachine.DataBind();
                if (lbMachine.Items.Count > 0)
                {
                    foreach (ListItem item in lbMachine.Items)
                    {
                        item.Selected = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachine: " + ex.Message);
            }
        }
        private void BindShift()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllShifts("");
                lbShiftID.DataSource = list;
                lbShiftID.DataBind();
                if (lbShiftID.Items.Count > 0)
                {
                    foreach (ListItem item in lbShiftID.Items)
                    {
                        item.Selected = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachine: " + ex.Message);
            }
        }
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachine();
        }
        private void BindData()
        {
            try
            {

                DateTime fromdate = Util.GetDateTime(txtFromDate.Text);
                DateTime toDate = Util.GetDateTime(txtToDate.Text);
                DataTable dt = CumiDBAccess.GetProductParameterReportDetails(fromdate, toDate, ddlPlant.SelectedValue, HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMachine), HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbShiftID));
                dt.Columns.Add("DateInString", typeof(string));
                if (dt.Rows.Count > 0)
                {
                    dt.AsEnumerable().ToList<DataRow>().ForEach(k =>
                    {
                        k["DateInString"] = Convert.IsDBNull(k["Date"]) ? "" : Convert.ToDateTime(k["Date"].ToString()).ToString("yyyy-MM-dd");
                    });
                }
                lvProductReport.DataSource = dt;
                lvProductReport.DataBind();
                Session["ProductReportDt"] = dt;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ProductReportDt"] == null)
                {
                    return;
                }
                DataTable dt = Session["ProductReportDt"] as DataTable;
                DateTime fromdate = Util.GetDateTime(txtFromDate.Text);
                DateTime toDate = Util.GetDateTime(txtToDate.Text);
                string machinedisplay = "";
                string machine = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMachine);
                if (machine.Split(',').Count().Equals(lbMachine.Items.Count))
                    machinedisplay = "All";
                else
                    machinedisplay = machine;
                TMPTrakGenerateReport.GenerateCumiProductParameterReport(dt, fromdate, toDate, "", HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbShiftID), ddlPlant.SelectedValue);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}