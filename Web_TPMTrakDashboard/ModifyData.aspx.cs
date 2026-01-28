using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class ModifyData : System.Web.UI.Page
    {
        DateTime StartTime = DateTime.Now;
        DateTime EndTime = DateTime.Now;
        string MachineID = string.Empty;
        List<string> DownCodeList = new List<string>();
        List<string> DownIdList = new List<string>();
        List<string> ComponentList = new List<string>();
        List<string> OperatorIdList = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindAllPlants();
                BindAllMachineIds();
                ddlDataType.SelectedIndex = 0;
                SetLogicalDates();
            }
        }

        private void BindAllPlants()
        {
            try
            {
                List<string> allPlantIds = new List<string>();
                allPlantIds = DataBaseAccess.GetAllPlants();
                if (allPlantIds != null && allPlantIds.Count > 1)
                {
                    ddlPlantID.DataSource = allPlantIds;
                    ddlPlantID.DataBind();
                    ddlPlantID.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
        }

        private void BindAllMachineIds()
        {
            try
            {
                if (ddlPlantID.SelectedItem != null)
                {
                    List<string> allMachineIds = new List<string>();
                    allMachineIds = DataBaseAccess.GetAllMachinesForPlant(ddlPlantID.SelectedItem.ToString());
                    if (allMachineIds != null && allMachineIds.Count > 0)
                    {
                        ddlMachineID.DataSource = allMachineIds;
                        ddlMachineID.DataBind();
                        ddlMachineID.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
        }

        private void SetLogicalDates()
        {
            string logicalDayStart = VDGDataBaseAccess.GetLogicalDayStart(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("dd-MMM-yyyy HH:mm");
            string logicalDayEnd = VDGDataBaseAccess.GetLogicalDayEnd(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("dd-MMM-yyyy HH:mm");
        }

        private void LoadDataGrids()
        {
            try
            {
                DataTable dtGridData = new DataTable();
                StartTime = Util.GetDateTime(txtFromDate.Text);
                EndTime = Util.GetDateTime(txtToDate.Text);
                MachineID = ddlMachineID.SelectedItem != null ? ddlMachineID.Text.Split('<')[0].Trim() : "";
                if (ddlDataType.SelectedItem.ToString().Equals("Production Data", StringComparison.OrdinalIgnoreCase))
                {
                    datagridProductionData.Visible = true;
                    datagridDownData.Visible = false;
                    if (!string.IsNullOrEmpty(MachineID))
                        dtGridData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineID, StartTime, EndTime, "Production Data");
                    if (dtGridData.Rows.Count > 0)
                    {
                        datagridProductionData.DataSource = dtGridData;
                        datagridProductionData.DataBind();
                    }
                    else
                        datagridProductionData.DataSource = null;
                }
                else if (ddlDataType.SelectedItem.ToString().Equals("Down Data", StringComparison.OrdinalIgnoreCase))
                {
                    datagridProductionData.Visible = false;
                    datagridDownData.Visible = true;
                    if (!string.IsNullOrEmpty(MachineID))
                        dtGridData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineID, StartTime, EndTime, "Down Data");
                    if (dtGridData.Rows.Count > 0)
                    {
                        datagridDownData.DataSource = dtGridData;
                        datagridDownData.DataBind();
                    }
                    else
                        datagridDownData.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
        }

        private void BindProductionData()
        {
            try
            {
                DataTable dtProductionData = new DataTable();
                StartTime = Util.GetDateTime(txtFromDate.Text);
                EndTime = Util.GetDateTime(txtToDate.Text);
                MachineID = ddlMachineID.SelectedItem != null ? ddlMachineID.Text.Split('<')[0].Trim() : "";
                if (Session["ProdcutionData"] == null)
                    Session["ProdcutionData"] = dtProductionData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineID, StartTime, EndTime, "Production Data");
                else
                    dtProductionData = Session["ProdcutionData"] as DataTable;
                datagridProductionData.DataSource = dtProductionData;
                datagridProductionData.DataBind();
                datagridProductionData.Visible = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
        }

        private void BindDownData()
        {
            try
            {
                DataTable dtDownData = new DataTable();
                StartTime = Util.GetDateTime(txtFromDate.Text);
                EndTime = Util.GetDateTime(txtToDate.Text);
                MachineID = ddlMachineID.SelectedItem != null ? ddlMachineID.Text.Split('<')[0].Trim() : "";
                if (Session["DownData"] == null)
                    Session["DownData"] = dtDownData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineID, StartTime, EndTime, "Down Data");
                else
                    dtDownData = Session["DownData"] as DataTable;
                datagridDownData.DataSource = dtDownData;
                datagridDownData.DataBind();
                datagridDownData.Visible = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            LoadDataGrids();
        }

        protected void datagridProductionData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                LoadDataInSession();
                HiddenField hdfComponent = (e.Row.FindControl("hdfComponentProd") as HiddenField);
                DropDownList ComponentDropdown = (e.Row.FindControl("ddlComponentProd") as DropDownList);
                ComponentDropdown.DataSource = ComponentList;
                ComponentDropdown.DataBind();
                ComponentDropdown.SelectedValue = hdfComponent.Value;

                HiddenField hdfOperator = (e.Row.FindControl("hdfOperatorProd") as HiddenField);
                DropDownList OperatorDropdown = (e.Row.FindControl("ddlOperatorProd") as DropDownList);
                OperatorDropdown.DataSource = OperatorIdList;
                OperatorDropdown.DataBind();
                OperatorDropdown.SelectedValue = hdfOperator.Value;
            }
        }

        protected void datagridProductionData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            datagridProductionData.PageIndex = e.NewPageIndex;
            BindProductionData();
        }

        protected void datagridDownData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                LoadDataInSession();
                HiddenField hdfComponent = (e.Row.FindControl("hdfComponentDown") as HiddenField);
                DropDownList ComponentDropdown = (e.Row.FindControl("ddlComponentDown") as DropDownList);
                ComponentDropdown.DataSource = ComponentList;
                ComponentDropdown.DataBind();
                ComponentDropdown.SelectedValue = hdfComponent.Value;

                HiddenField hdfOperator = (e.Row.FindControl("hdfOperatorDown") as HiddenField);
                DropDownList OperatorDropdown = (e.Row.FindControl("ddlOperatorDown") as DropDownList);
                OperatorDropdown.DataSource = OperatorIdList;
                OperatorDropdown.DataBind();
                OperatorDropdown.SelectedValue = hdfOperator.Value;

                HiddenField hdfDowncode = (e.Row.FindControl("hdfDownCode") as HiddenField);
                DropDownList ddlDowncode = (e.Row.FindControl("ddlDowncodeDown") as DropDownList);
                ddlDowncode.DataSource = DownCodeList;
                ddlDowncode.DataBind();
                ddlDowncode.SelectedValue = hdfDowncode.Value;
            }
        }

        protected void datagridDownData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            datagridDownData.PageIndex = e.NewPageIndex;
            BindDownData();
        }

        #region Store Data in Session
        private void LoadDataInSession()
        {
            if (Session["Downcode"] == null)
                Session["Downcode"] = DownCodeList = DataBaseAccess.GetAllDownCode();
            else
                DownCodeList = Session["Downcode"] as List<string>;
            if (Session["ComponentIDs"] == null)
                Session["ComponentIDs"] = ComponentList = DataBaseAccess.GetAllComp();
            else
                ComponentList = Session["ComponentIDs"] as List<string>;
            if (Session["OperatorIDs"] == null)
                Session["OperatorIDs"] = OperatorIdList = DataBaseAccess.GetAllEmployees();
            else
                OperatorIdList = Session["OperatorIDs"] as List<string>;
        }
        #endregion
    }
}