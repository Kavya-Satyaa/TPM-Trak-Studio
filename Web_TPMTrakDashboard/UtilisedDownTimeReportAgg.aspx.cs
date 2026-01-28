using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class UtilisedDownTimeReportAgg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPlant();
                BindShift();
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                ddlType_SelectedIndexChanged(null, null);
                BindData();
            }
        }
        private void BindPlant()
        {
            try
            {
                List<string> lstPlantData = BindCockpitView.ViewPlantToDisplay();
                if (lstPlantData.Count > 0)
                {
                    lstPlantData.Insert(0, "All");
                }
                ddlPlant.DataSource = lstPlantData;
                ddlPlant.DataBind();
                BindCell();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindCell()
        {
            try
            {
                List<string> lstCellId = BindCockpitView.ViewCellsToDisplay(ddlPlant.SelectedValue);
                lbCell.DataSource = lstCellId;
                lbCell.DataBind();
                foreach(ListItem item in lbCell.Items)
                {
                    item.Selected = true;
                }
                BindMachine();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = DataBaseAccess.getMachineIDListForScreen(ddlPlant.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue, DataBaseAccess.getCellIDWithSeparator(lbCell), "ReportLive");
                lbMachine.DataSource = list;
                lbMachine.DataBind();
                foreach (ListItem item in lbMachine.Items)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindShift()
        {
            try
            {
                List<string> list = BindCockpitView.GetAllShift();
                if (list.Count > 0)
                {
                    list.Insert(0, "All");
                }
                ddlShift.DataSource = list;
                ddlShift.DataBind();
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
                string plant = ddlPlant.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue;
                DataTable dt = DataBaseAccess.getUtilisedDownTimeReportDetails(plant, DataBaseAccess.getCellIDWithSeparator(lbCell), DataBaseAccess.getMachineIDWithSeparator(lbMachine), txtFromDate.Text, txtToDate.Text, ddlShift.SelectedValue, ddlType.SelectedValue, txtYear.Text, txtMonth.Text);
                UtilisedDowntimeReportChartEntity data = new UtilisedDowntimeReportChartEntity();
                if (dt.Rows.Count > 0)
                {

                    data.Category = dt.AsEnumerable().Select(k => k["MachineID"].ToString()).ToList();
                    List<UtilisedDowntimeReportChartSeriesEntity> seriesList = new List<UtilisedDowntimeReportChartSeriesEntity>();
                    seriesList.Add(getSeriesValue(dt, "MgmtLossInSec", "Management Loss", "#ffff00"));
                    seriesList.Add(getSeriesValue(dt, "DownTimeInSec", "Down Time", "#ff5252"));
                    seriesList.Add(getSeriesValue(dt, "UtilisedTimeInSec", "Utilised Time", "#4dff4d"));
                    data.series = seriesList;
                }
                Session["UtilisedDownReportChart"] = data;
                Session["UtilisedDownReportChartDT"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "BindChart", " BindChart();", true);
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
                string Generated = TPMTrakGenerateReportNewDll.UtilisedAndDowntimeReportAgg(ddlPlant.SelectedValue, DataBaseAccess.getCellIDWithSeparator(lbCell), DataBaseAccess.getMachineIDWithSeparator(lbMachine), txtFromDate.Text, txtToDate.Text, ddlShift.SelectedValue, ddlType.SelectedValue, txtYear.Text, txtMonth.Text, Session["UtilisedDownReportChartDT"] as DataTable);
                if (Generated.Equals("", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "alert('Error generating report.Please Try Again.');", true);
                else if (Generated.Equals("NodataFound", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNodataformo", "alert('No Data Found');", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private UtilisedDowntimeReportChartSeriesEntity getSeriesValue(DataTable dt, string seriesColName, string seriesName,string seriesColor)
        {
            UtilisedDowntimeReportChartSeriesEntity seriesData = new UtilisedDowntimeReportChartSeriesEntity();
            try
            {
                seriesData.name = seriesName;
                seriesData.data = dt.AsEnumerable().Select(k => Convert.ToDouble(k[seriesColName].ToString())).ToList();
                seriesData.color = seriesColor;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return seriesData;
        }
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tdShiftContent.Visible = false;
                tdShiftHeader.Visible = false;
                tdToDateHeader.Visible = false;
                tdToDateContent.Visible = false;
                tdFromDateHeader.Visible = false;
                tdFromDateContent.Visible = false;
                tdYearMonthHeader.Visible = false;
                tdYearMonthContent.Visible = false;
                tdFromDateHeader.InnerText = "From Date";
                if (ddlType.SelectedValue.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                {
                    tdFromDateHeader.Visible = true;
                    tdFromDateContent.Visible = true;
                    tdFromDateHeader.InnerText = "Date";
                    tdShiftContent.Visible = true;
                    tdShiftHeader.Visible = true;
                }
                else if (ddlType.SelectedValue.Equals("Month", StringComparison.OrdinalIgnoreCase))
                {
                    tdYearMonthHeader.Visible = true;
                    tdYearMonthContent.Visible = true;
                }
                else
                {
                    tdFromDateHeader.Visible = true;
                    tdFromDateContent.Visible = true;
                    tdToDateHeader.Visible = true;
                    tdToDateContent.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCell();
        }

        protected void lbCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachine();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "openlistbox", "stayMultiselectedList('cell');", true);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static UtilisedDowntimeReportChartEntity getChartData()
        {
            UtilisedDowntimeReportChartEntity data = HttpContext.Current.Session["UtilisedDownReportChart"] as UtilisedDowntimeReportChartEntity;
            return data;
        }


    }
}