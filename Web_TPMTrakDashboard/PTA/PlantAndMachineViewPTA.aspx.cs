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
    public partial class PlantAndMachineViewPTA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPlant();
                txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                btnView_Click(null, null);
            }
        }
        private void BindPlant()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlants();
                list.Remove("All");
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();
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
                MachineViewContainer.Visible = false;
                PlantViewContainer.Visible = false;
                if (ddlViewType.SelectedValue.Equals("Plant", StringComparison.OrdinalIgnoreCase))
                {
                    BindPlantData();
                }
                else if (ddlViewType.SelectedValue.Equals("Machine", StringComparison.OrdinalIgnoreCase))
                {
                    BindMachineData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindPlantData()
        {
            try
            {
                lblMachineCount.Text = DataBaseAccessPTA.getPlantViewMachineCount(ddlPlant.SelectedValue);
                PlantViewContainer.Visible = true;
                PlantMachineViewEntity data = DataBaseAccessPTA.getPlantViewDetails(txtDate.Text, ddlPlant.SelectedValue);
                lblProductionTime.Text = data.ProductionTime;
                lblNonProductionTime.Text = data.NonProductionTime;
                lblLoadUnload.Text = data.LoadUnLoad;
                lblMachineStoppage.Text = data.MachineStoppage;
                lblNoOfParts.Text = data.ActualParts;
                lblTargetRevenue.Text = data.TargetRevenue;
                Session["PlantChartData"] = data;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "BindPlantChart();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindMachineData()
        {
            try
            {
                MachineViewContainer.Visible = true;
                List<PlantMachineViewEntity> list = DataBaseAccessPTA.getMachineViewDetails(txtDate.Text, ddlPlant.SelectedValue);
                lvMachineData.DataSource = list;
                lvMachineData.DataBind();
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
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static PlantMachineViewEntity getPlantChartData()
        {
            PlantMachineViewEntity data = new PlantMachineViewEntity();
            try
            {
                if (HttpContext.Current.Session["PlantChartData"] != null)
                {
                    data = HttpContext.Current.Session["PlantChartData"] as PlantMachineViewEntity;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return data;
        }
    }
}