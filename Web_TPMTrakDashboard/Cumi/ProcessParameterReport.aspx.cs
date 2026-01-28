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
    public partial class ProcessParameterReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
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

        private void BindData()
        {
            try
            {
                BindShiftDateTime(out DateTime fromDate, out DateTime toDate);
                List<ProcessParameterEntity> list = CumiDBAccess.GetProcessParameterReport(ddlMachineId.SelectedValue.ToString(), ddlshift.SelectedValue, fromDate, toDate);
                if(list.Count>0)
                {
                    lvProcessParameterReport.DataSource = list;
                    lvProcessParameterReport.DataBind();
                    Session["ProcessParameter"] = list;
                }
                else
                {
                    lvProcessParameterReport.DataSource = new List<ProcessParameterEntity>();
                    lvProcessParameterReport.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void BindShiftDateTime(out DateTime fromDate,out DateTime toDate)
        {
            var shift = CockpitDataBaseAccess.GetShiftTime(ddlshift.SelectedValue.ToString(), txtDate.Text);
            fromDate = Util.GetDateTime(shift[0]);
            toDate = Util.GetDateTime(shift[1]);
        }

        private void BindShift()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllShifts("");
                if (list.Contains("All")) list.Remove("All");
                ddlshift.DataSource = list;
                ddlshift.DataBind();

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
                ddlMachineId.DataSource = list;
                ddlMachineId.DataBind();
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

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ProcessParameter"] == null)
                {
                    return;
                }
                BindShiftDateTime(out DateTime fromDate, out DateTime toDate);
                List<ProcessParameterEntity> list = Session["ProcessParameter"] as List<ProcessParameterEntity>;
                CumiGenerateReport.generateProcessParameterReport(fromDate, toDate, ddlPlant.SelectedValue, ddlMachineId.SelectedValue,ddlshift.SelectedValue, list);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ExportProcessParameter: " + ex.Message);
            }
        }
    }
}