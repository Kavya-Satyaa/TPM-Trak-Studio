using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.EnergyModule.Models;

namespace Web_TPMTrakDashboard.EnergyModule
{
    public partial class LiveData : System.Web.UI.Page
    {
        public List<Web_TPMTrakDashboard.Models.ColumnViewSetting> settings = null;
        List<string> allCellIDs = new List<string>();
        protected void Page_Init(object sender, EventArgs e)
        {

            settings = Web_TPMTrakDashboard.Models.DataBaseAccess.BindSettingPage("EnergyLiveData", Session["Language"] == null ? "en" : Session["Language"].ToString());
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPlantIDs();
                BindCellIDs();
                allCellIDs = DataBaseAccess.GetCellIDs("");
                if (allCellIDs.Count > 0)
                {
                    tdCell.Visible = true;
                    ddlCell.Visible = true;
                }
                else
                {
                    tdCell.Visible = false;
                    ddlCell.Visible = false;
                }
                BindLiveDataGrid();
            }
        }

        private void BindLiveDataGrid()
        {
            try
            {
                List<LiveDataCs> liveDatas = new List<LiveDataCs>();
                //if (System.Web.Configuration.WebConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                allCellIDs= DataBaseAccess.GetCellIDs("");
                if (allCellIDs.Count > 0)
                {
                    liveDatas = DataBaseAccess.GetDataLiveData("day", "live", "Technolivescreen", ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString(), ddlCell.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedValue.ToString(),ddlMachineType.SelectedValue.ToString());
                }
                else
                {
                    liveDatas = DataBaseAccess.GetDataLiveData("day", "live", "Technolivescreen", ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString(),"",ddlMachineType.SelectedValue.ToString());
                }

                foreach (DataControlField col in gvLiveData.Columns)
                {
                    if (col.AccessibleHeaderText != "")
                    {
                        string columnName = settings.Where(x => x.ValueInText == col.AccessibleHeaderText).Select(x => x.ValueInText2).FirstOrDefault();
                        if (columnName == null)
                        {
                            col.Visible = false;
                        }
                        else
                        {
                            col.HeaderText = columnName;
                        }
                    }

                }

                gvLiveData.DataSource = liveDatas;
                gvLiveData.DataBind();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            
        }

        protected void timerToAutoRefresh_Tick(object sender, EventArgs e)
        {
            BindLiveDataGrid();
        }

        protected void cbAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutoRefresh.Checked)
            {

                timerToAutoRefresh.Enabled = true;
                timerToAutoRefresh.Interval = 1000 * Convert.ToInt32(WebConfigurationManager.AppSettings["EnergyAutoRefreshInterval"].ToString());
            }
            else
            {
                timerToAutoRefresh.Enabled = false;
            }
            BindLiveDataGrid();
        }
        private void BindPlantIDs()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlants();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindCellIDs()
        {
            try
            {
                List<string> list = DataBaseAccess.GetCellIDs(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString());
                ddlCell.DataSource = list;
                ddlCell.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellIDs();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindLiveDataGrid();
        }

        protected void ddlMachineType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlMachineType.SelectedValue.ToString().Equals("Non-Machine EM", StringComparison.OrdinalIgnoreCase))
                {
                    tdCell.Visible = false;
                    tdPlant.Visible = false;
                    ddlPlant.Visible = false;
                    ddlCell.Visible = false;
                }
                else
                {
                    tdCell.Visible = true;
                    tdPlant.Visible = true;
                    ddlPlant.Visible = true;
                    ddlCell.Visible = true;
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}