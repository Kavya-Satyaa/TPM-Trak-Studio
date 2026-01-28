using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Advik
{
    public partial class AdvikReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DisbleAlltr();
                PageLoad();
            }
        }

        private void DisbleAlltr()
        {
            trCellId.Visible = false;
            trPlant.Visible = false;
            trMachine.Visible = false;
            trFromDate.Visible = false;
            trToDate.Visible = false;
            trmonthlydate.Visible = false;
            trShift.Visible = false;
        }

        private void PageLoad()
        {
            try
            {
                trCellId.Visible = true;
                trPlant.Visible = true;
                trMachine.Visible = true;
                trmonthlydate.Visible = true;
                trShift.Visible = true;
                ddlReportType.SelectedIndex = 0;
                BindPlant();
                BindShift();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }


        private void BindPlant()
        {
            List<string> lstPlantData = BindCockpitView.ViewPlantToDisplay();
            if (lstPlantData != null && lstPlantData.Count > 0)
            {
                ddlPlantId.DataSource = lstPlantData;
                ddlPlantId.DataBind();
                BindCell();
            }
        }

        private void BindCell()
        {
            if (ddlPlantId != null && ddlPlantId.SelectedValue != null)
            {
                List<string> lstCell = BindCockpitView.ViewCellsToDisplay(ddlPlantId.SelectedValue.ToString());
                if (lstCell != null && lstCell.Count > 0)
                {
                    ddlCellID.DataSource = lstCell;
                    ddlCellID.DataBind();
                    BindMachine();
                }
            }
        }

        private void BindMachine()
        {
            if (ddlCellID != null && ddlPlantId != null && ddlPlantId.SelectedValue != null && ddlCellID.SelectedValue != null)
            {
                List<string> lstMachineID = DataBaseAccess.AdvikDatabaseAccess.GetMachinesbyPlantCell(ddlPlantId.SelectedValue.ToString(), ddlCellID.SelectedValue.ToString());
                if (lstMachineID != null && lstMachineID.Count > 0)
                {
                    lstMachineID.Insert(0, "All");
                    ddlMachineId.DataSource = lstMachineID;
                    ddlMachineId.DataBind();
                }
            }
        }

        private void BindShift()
        {
            List<string> ShiftDetails = BindCockpitView.GetAllShift();
            ShiftDetails.Insert(0, "All");
            if (ShiftDetails != null && ShiftDetails.Count > 0)
            {
                ddlShift.DataSource = ShiftDetails;
                ddlShift.DataBind();
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetVal()
        {
            if (HttpContext.Current.Session["ReportGenerated"] != null)
                return HttpContext.Current.Session["ReportGenerated"].ToString();
            else
                return "Ended";

        }

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCell();
        }

        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachine();
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            string Generated = string.Empty;

            try
            {
                DateTime fromDate = DateTime.Now;
                string PlantID = ddlPlantId.SelectedValue == null ? "" : ddlPlantId.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantId.SelectedValue.ToString();
                string CellID = ddlCellID.SelectedValue == null ? "" : ddlCellID.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : ddlCellID.SelectedValue.ToString();
                string MachineID = ddlMachineId.SelectedValue == null ? "" : ddlMachineId.SelectedValue.ToString();
                if (ddlReportType.SelectedValue.ToString().Equals("JHChecklistReport", StringComparison.OrdinalIgnoreCase))
                {
                    string date = "01-" + txtMonth.Text + "-" + txtYear.Text;
                    fromDate = Util.GetDateTime(date);
                    Generated = AdvikGenerateReports.JHCheckListReport(PlantID, CellID, MachineID, ddlShift.SelectedValue.ToString(), fromDate);
                }

                if (Generated.Equals("", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "messageNotOk();", true);
                else if (Generated.Equals("NodataFound", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNodata", "messageNodata();", true);
                }
                else if (Generated.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "messageOk();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PageLoad();
        }
    }
}