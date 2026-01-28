using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik.DataBaseAccess;
using Web_TPMTrakDashboard.Advik.Models;

namespace Web_TPMTrakDashboard.Advik
{
    public partial class AssemblyLineAndon : System.Web.UI.Page
    {
        private int refreshInterval = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ConfigurationManager.AppSettings["AssemblyLineAndonRefreshInterval"] != null)
                    int.TryParse(ConfigurationManager.AppSettings["AssemblyLineAndonRefreshInterval"].ToString(), out refreshInterval);
                BindPlantID();
                bindCellID();
                BindMachineParametersData();
                if (refreshInterval > 0)
                {
                    timerAndonRefresh.Interval = refreshInterval * 1000;
                    timerAndonRefresh.Enabled = true;
                }
            }
        }

        private void bindCellID()
        {
            try
            {
                string plant = ddlPlantID.SelectedValue == null ? "" : ddlPlantID.SelectedItem.ToString();
                List<string> GetCell = AdvikDatabaseAccess.GetCell(plant);
                GetCell.RemoveAt(0);
                ddlGroupID.DataSource = GetCell;
                ddlGroupID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        public void BindPlantID()
        {
            try
            {
                List<string> listPlantID = AdvikDatabaseAccess.GetPlantIDs();
                if (listPlantID != null && listPlantID.Count > 0)
                {
                    ddlPlantID.DataSource = listPlantID;
                    ddlPlantID.DataBind();
                    if (Request.Cookies["PlantID"] != null)
                    {
                        string plant_id = Request.Cookies["PlantID"].Value;
                        ddlPlantID.SelectedValue = plant_id;
                    }
                    else
                    {
                        ddlPlantID.SelectedIndex = 0;
                    }
                    SetPlantIDCookie();
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void SetPlantIDCookie()
        {
            try
            {
                HttpCookie cookie = new HttpCookie("PlantID");
                cookie.Value = ddlPlantID.SelectedValue;
                cookie.Expires = DateTime.Now.AddHours(24);
                Response.SetCookie(cookie);
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void BindMachineParametersData()
        {
            try
            {
                string plantID = ddlPlantID.SelectedItem != null ? ddlPlantID.SelectedValue : "";
                if (!string.IsNullOrEmpty(plantID))
                {
                    List<AssemblyLineDataEntity> andonDataList = AdvikDatabaseAccess.GetAdvikAssemblyLineAndonData(plantID);
                    AndonPartsCountDataEntity andonPartsCountData = AdvikDatabaseAccess.GetAndonPartsCountData(plantID);
                    if (andonDataList != null && andonDataList.Count > 0)
                    {
                        lstMachinewiseAndonData.DataSource = andonDataList;
                        lstMachinewiseAndonData.DataBind();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "dimensions", "ApplyAndonItemDimensions();", true);
                    }
                    if (andonPartsCountData != null && andonPartsCountData.AndonPartsCountDataList != null && andonPartsCountData.AndonPartsCountDataList.Count > 0)
                    {
                        lstAndonPartsCount.DataSource = andonPartsCountData.AndonPartsCountDataList;
                        lstAndonPartsCount.DataBind();
                        lblDateShift.Text = andonPartsCountData.DateShift;
                        (lstAndonPartsCount.FindControl("lblTGT") as Label).Text = andonPartsCountData.TGT.ToString();
                        (lstAndonPartsCount.FindControl("lblACT") as Label).Text = andonPartsCountData.ACT.ToString();
                        (lstAndonPartsCount.FindControl("lblThroughput") as Label).Text = andonPartsCountData.Throughput;
                        lblMonthMTD.Text = $"{DateTime.Now:MMMM yyyy} (MTD)";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "graphs", "GetAllDowntimeAndEfficiencyDetails();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static DowntimeAndEfficiencyEntity GetDowntimeAndEfficiencyDetails(string plantID, string CellID)
        {
            DowntimeAndEfficiencyEntity downtimeAndEfficiencyDetails = null;
            try
            {
                downtimeAndEfficiencyDetails = AdvikDatabaseAccess.GetAllDowntimeAndEfficiencyDetails(plantID, CellID);
                if (downtimeAndEfficiencyDetails != null)
                {
                    if (downtimeAndEfficiencyDetails.DownTimeDetails.Count > 0)
                        downtimeAndEfficiencyDetails.DownTimeDetails.Take(10);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return downtimeAndEfficiencyDetails;
        }

        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlPlantID.SelectedValue))
            {
                bindCellID();
                BindMachineParametersData();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "graphs", "GetAllDowntimeAndEfficiencyDetails();", true);
                if (Request.Cookies["PlantID"] != null)
                    Request.Cookies["PlantID"].Value = ddlPlantID.SelectedValue;
                else
                    SetPlantIDCookie();
            }
        }

        protected void timerAndonRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                BindMachineParametersData();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "graphs", "GetAllDowntimeAndEfficiencyDetails();", true);
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void ddlGroupID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlPlantID.SelectedValue) && !string.IsNullOrEmpty(ddlGroupID.SelectedValue))
            {
                BindMachineParametersData();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "graphs", "GetAllDowntimeAndEfficiencyDetails();", true);
                if (Request.Cookies["PlantID"] != null && Request.Cookies["PlantID"] != null)
                {
                    Request.Cookies["PlantID"].Value = ddlPlantID.SelectedValue;
                    Request.Cookies["GroupID"].Value = ddlGroupID.SelectedValue;
                }
                else
                    SetPlantIDCookie();
            }
        }
    }
}