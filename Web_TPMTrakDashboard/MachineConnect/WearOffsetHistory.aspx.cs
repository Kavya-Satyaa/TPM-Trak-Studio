using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.MachineConnect.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.MachineConnect
{
    public partial class WearOffsetHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindMachine();
                BindData();
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = MachineConnectDBAccess.getMachinesOfDNCEnabled();
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
                ddlMachine_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOffSetNo();
        }
        private void BindOffSetNo()
        {
            try
            {
                List<string> list = new List<string>();
                string machineType = MachineConnectDBAccess.getMachineType(ddlMachine.SelectedValue);
                hdnMachineType.Value = machineType;
                if (machineType.Contains("Turning"))
                {
                    list.Add("Wear");
                    list.Add("Geom");
                }
                else
                {
                    list.Add("ALL");
                }
                ddlOffsetType.DataSource = list;
                ddlOffsetType.DataBind();
                ddlOffsetType_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }



        protected void ddlOffsetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int offsetRange = MachineConnectDBAccess.getOffsetRange(hdnMachineType.Value, ddlOffsetType.SelectedValue);
                List<string> list = new List<string>();
                for (int i = 1; i <= offsetRange; i++)
                {
                    list.Add(i.ToString());
                }
                lbOffsetRange.DataSource = list;
                lbOffsetRange.DataBind();
                foreach (ListItem item in lbOffsetRange.Items)
                {
                    item.Selected = true;
                }
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
        private void BindData()
        {
            try
            {
                string param1 = "Dashboard";
                if (chkTopView.Checked)
                {
                    param1 = "top20";
                }
                string wearOffset = string.Empty;
                int checkedCount = 0;
                foreach (ListItem item in lbOffsetRange.Items)
                {
                    if (item.Selected)
                    {
                        if (wearOffset == string.Empty)
                        {
                            wearOffset = item.Value;
                        }
                        else
                        {
                            wearOffset = wearOffset + "," + item.Value;
                        }
                        checkedCount++;
                    }
                }
                if (checkedCount == lbOffsetRange.Items.Count)
                {
                    wearOffset = "";
                }
                bool enableChart = false;
                if (checkedCount == 1)
                {
                    enableChart = true;
                }
                hdnChartView.Value = enableChart.ToString();
                WearOffsetChartEntity chartData = new WearOffsetChartEntity();
                List<WearOffsetEntity> list = MachineConnectDBAccess.getWearOffsetHistoryDetails(txtFromDate.Text, txtToDate.Text, ddlMachine.SelectedValue, wearOffset, ddlOffsetType.SelectedValue, param1, "View", enableChart, out chartData);
                gvOffsetData.DataSource = list;
                gvOffsetData.DataBind();
                Session["OffsetData"] = list;
                if (hdnMachineType.Value.Contains("Machining"))
                {
                    gvOffsetData.HeaderRow.Cells[4].Text = "Geom(H)";
                    gvOffsetData.HeaderRow.Cells[5].Text = "Wear(H)";
                    gvOffsetData.HeaderRow.Cells[6].Text = "Geom(D)";
                    gvOffsetData.HeaderRow.Cells[7].Text = "Wear(D)";
                    chartData.LeftChartTitle = " GeomOffset-X";
                    chartData.RightChartTitle = " WearOffset-H";
                }
                else
                {
                    if (ddlOffsetType.SelectedValue.Equals("Wear"))
                    {
                        gvOffsetData.HeaderRow.Cells[4].Text = "WearOffsetX";
                        gvOffsetData.HeaderRow.Cells[5].Text = "WearOffsetZ";
                        gvOffsetData.HeaderRow.Cells[6].Text = "WearOffsetR";
                        gvOffsetData.HeaderRow.Cells[7].Text = "WearOffsetT";
                        chartData.LeftChartTitle = " WearOffset-X";
                        chartData.RightChartTitle = " WearOffset-Z";
                    }
                    else
                    {
                        gvOffsetData.HeaderRow.Cells[4].Text = "GeomOffsetX";
                        gvOffsetData.HeaderRow.Cells[5].Text = "GeomOffsetZ";
                        gvOffsetData.HeaderRow.Cells[6].Text = "GeomOffsetR";
                        gvOffsetData.HeaderRow.Cells[7].Text = "GeomOffsetT";
                        chartData.LeftChartTitle = " GeomOffset-X";
                        chartData.RightChartTitle = " GeomOffset-Z";
                    }

                }
                Session["OffsetChartData"] = chartData;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "chartData", "BindChart();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            string ReportStatus = "";
            try
            {
                if (HttpContext.Current.Session["OffsetData"] == null)
                {
                    return;
                }
                List<WearOffsetEntity> list = HttpContext.Current.Session["OffsetData"] as List<WearOffsetEntity>;
                ReportStatus = MachineConnectGenerateReport.GenerateOffsetHistoryReport(txtFromDate.Text, txtToDate.Text, ddlMachine.SelectedValue, list);
                if (ReportStatus.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openSuccessModal(this, "Report Generated");
                else if (ReportStatus.Equals("NoData", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openWarningToastrModal(this, "No data found");
                else
                    HelperClassGeneric.openWarningToastrModal(this, "Try again");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static WearOffsetChartEntity getChartData()
        {
            WearOffsetChartEntity chartData = new WearOffsetChartEntity();
            try
            {
                if (HttpContext.Current.Session["OffsetChartData"] != null)
                {
                    chartData = HttpContext.Current.Session["OffsetChartData"] as WearOffsetChartEntity;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return chartData;
        }
    }
}