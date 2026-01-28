using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.PTA
{
    public partial class DayAndShiftViewPTA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["MachineDayChartData"] = null;
                HttpContext.Current.Session["ShiftProductionData"] = null;
                if (Page.ClientQueryString.Length > 0)
                {
                    if (Request.QueryString["plantId"] != null)
                    {
                        txtDate.Text = Request.QueryString["date"].ToString();
                        hdnPlantID.Value = Request.QueryString["plantId"].ToString();
                        BindShift();
                        BindMachine();
                        HelperClassGeneric.setDropdownValue(ddlMachine, Request.QueryString["machineID"].ToString());
                        ddlViewType_SelectedIndexChanged(null, null);
                        btnView_Click(null, null);
                    }
                }
                else
                {
                    DayViewContainer.Visible = false;
                    ShiftViewContainer.Visible = false;
                }
            }
        }
        private void BindShift()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllShifts("");
                ddlShift.DataSource = list;
                ddlShift.DataBind();
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
                List<string> list = VDGDataBaseAccess.GetAllMachines(hdnPlantID.Value);
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
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
                DayViewContainer.Visible = false;
                ShiftViewContainer.Visible = false;
                Session["MachineDayChartData"] = null;
                HttpContext.Current.Session["ShiftProductionData"] = null;
                if (ddlViewType.SelectedValue.Equals("Day", StringComparison.OrdinalIgnoreCase))
                {
                    BindDayData();
                }
                else if (ddlViewType.SelectedValue.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                {
                    BindShiftData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindDayData()
        {
            try
            {
                DayViewContainer.Visible = true;
                PlantMachineViewEntity data = DataBaseAccessPTA.getDaywiseMachineProductionTimeDetails(txtDate.Text, hdnPlantID.Value, ddlMachine.SelectedValue, ddlShift.SelectedValue);
                lblProductionTime.Text = data.ProductionTime;
                lblNonProductionTime.Text = data.NonProductionTime;
                lblLoadUnload.Text = data.LoadUnLoad;
                lblMachineStoppage.Text = data.MachineStoppage;
                lblNoOfParts.Text = data.ActualParts;
                lblTargetRevenue.Text = data.TargetRevenue;
                Session["MachineDayChartData"] = data;


                List<MachineStoppageEntity> list = DataBaseAccessPTA.getDaywiseMachineStoppageDetails(txtDate.Text, hdnPlantID.Value, ddlMachine.SelectedValue, ddlShift.SelectedValue);
                lvMachineStoppage.DataSource = list;
                lvMachineStoppage.DataBind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "BindDayChart();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void BindShiftData()
        {
            try
            {
                ShiftViewContainer.Visible = true;
                List<PlantMachineViewEntity> list = DataBaseAccessPTA.getShiftwiseMachineProductionTimeDetails(txtDate.Text, hdnPlantID.Value, ddlMachine.SelectedValue);
                Session["ShiftProductionData"] = list;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "BindShiftData();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tdShiftHeader.Visible = false;
                tdShiftContent.Visible = false;
                if (ddlViewType.SelectedValue.Equals("Day", StringComparison.OrdinalIgnoreCase))
                {
                    tdShiftHeader.Visible = true;
                    tdShiftContent.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static PlantMachineViewEntity getDayChartData()
        {
            PlantMachineViewEntity data = new PlantMachineViewEntity();
            try
            {
                if (HttpContext.Current.Session["MachineDayChartData"] != null)
                {
                    data = HttpContext.Current.Session["MachineDayChartData"] as PlantMachineViewEntity;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return data;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<PlantMachineViewEntity> getShiftProductionData()
        {
            List<PlantMachineViewEntity> list = new List<PlantMachineViewEntity>();
            try
            {
                if (HttpContext.Current.Session["ShiftProductionData"] != null)
                {
                    list = HttpContext.Current.Session["ShiftProductionData"] as List<PlantMachineViewEntity>;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return list;
        }
    }
}