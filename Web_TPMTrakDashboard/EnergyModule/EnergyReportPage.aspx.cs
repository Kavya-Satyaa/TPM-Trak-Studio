using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.EnergyModule.Models;

namespace Web_TPMTrakDashboard.EnergyModule
{
    public partial class EnergyReportPage : System.Web.UI.Page
    {
        string isGenerated = "NotGenerated";
        List<string> allCellIDs = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtFromDateTime.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy HH:mm:ss");
                txtToDateTime.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                txtFromMonth.Text = DateTime.Now.AddMonths(-1).ToString("MMM-yyyy");
                txtToMonth.Text = DateTime.Now.ToString("MMM-yyyy");

                ddlPlantId.DataSource = DataBaseAccess.GetAllPlants();
                ddlPlantId.DataBind();
                ddlShift.DataSource = DataBaseAccess.GetAllShift();
                ddlShift.DataBind();
                BindCellIDs();
                allCellIDs = DataBaseAccess.GetCellIDs("");
                if (allCellIDs.Count > 0)
                {
                    trCell.Visible = true;
                }
                else
                {
                    trCell.Visible = false;
                }
                //if (System.Web.Configuration.WebConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                //{
                //    trCell.Visible = true;
                //}
                //else
                //{
                //    trCell.Visible = false;
                //}
                ddlPlantId_SelectedIndexChanged(null, EventArgs.Empty);
                ddlFormat_SelectedIndexChanged(null, EventArgs.Empty);
            }
        }

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellIDs();
            //if (System.Web.Configuration.WebConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
            allCellIDs = DataBaseAccess.GetCellIDs("");
            if (allCellIDs.Count > 0)
            {
                ddlMachineIDs.DataSource = DataBaseAccess.GetAllEM_Machines_Kiswok(ddlPlantId.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantId.SelectedValue.ToString(), ddlCellID.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCellID.SelectedValue.ToString(), "Report", ddlMachineType.SelectedValue.ToString());
                ddlMachineIDs.DataBind();
                foreach (ListItem item in ddlMachineIDs.Items)
                {
                    item.Selected = true;
                }
            }
            else
            {
                ddlMachineIDs.DataSource = DataBaseAccess.GetAllEM_Machines(ddlPlantId.SelectedValue, ddlMachineType.SelectedValue.ToString());
                ddlMachineIDs.DataBind();
                foreach (ListItem item in ddlMachineIDs.Items)
                {
                    item.Selected = true;
                }
            }
        }

        protected void ddlFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HideAllTimePeriods();
                if (ddlFormat.SelectedValue.Equals("Day", StringComparison.OrdinalIgnoreCase))
                {
                    txtFromDate.Visible = true;
                    txtToDate.Visible = true;
                    trTopEnergyConsumption.Visible = true;
                    trShift.Visible = false;
                }
                else if (ddlFormat.SelectedValue.Equals("Month", StringComparison.OrdinalIgnoreCase))
                {
                    txtFromMonth.Visible = true;
                    txtToMonth.Visible = true;
                    trShift.Visible = false;
                }
                else if (ddlFormat.SelectedValue.Equals("Time Consolidated", StringComparison.OrdinalIgnoreCase))
                {
                    txtFromDateTime.Visible = true;
                    txtToDateTime.Visible = true;
                    trTopEnergyConsumption.Visible = true;
                    trShift.Visible = false;
                }
                else
                {
                    txtFromDate.Visible = true;
                    txtToDate.Visible = true;
                    ddlShift.Enabled = true;
                    trShift.Visible = true;
                    trTopEnergyConsumption.Visible = true;
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void HideAllTimePeriods()
        {
            txtFromDate.Visible = false;
            txtFromDateTime.Visible = false;
            txtFromMonth.Visible = false;
            txtToDate.Visible = false;
            txtToDateTime.Visible = false;
            txtToMonth.Visible = false;
            ddlShift.Enabled = false;
            trTopEnergyConsumption.Visible = false;
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (ValidateFeilds()) return;
            string selectedMachines = string.Empty;
            if (ddlMachineIDs != null && !ddlMachineIDs.Text.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                //selectedMachines = ddlMachineIDs.Text.Split(new string[] { "," }, StringSplitOptions.None);
                //selectedMachines = selectedMachines.Take(selectedMachines.Count()).ToArray();
                foreach (ListItem item in ddlMachineIDs.Items)
                {
                    if (item.Selected)
                    {
                        if (selectedMachines == string.Empty)
                            selectedMachines = "'" + item.Value + "'";
                        else
                            selectedMachines = selectedMachines + ",'" + item.Value.ToString().Trim() + "'";
                    }
                    //selectedMachines.Add(item.Value);

                }
            }


            DataTable dtEnergyDate = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            try
            {
                string startDate = string.Empty;
                string endDate = string.Empty;
                string EnergySource = DataBaseAccess.GetEnergySourceFor_EMData();
                
                if (ddlReportType.SelectedValue.Equals("Format - I", StringComparison.OrdinalIgnoreCase))
                {
                    int showTopEnergyConsumption = 0;
                    if (chkTopEnergyConsumption.Checked)
                    {
                        showTopEnergyConsumption = 1;
                    }
                    if (ddlFormat.SelectedValue.Equals("Month"))
                    {
                        showTopEnergyConsumption = 0;
                        DateTime fromDate = DateTime.Now.Date;
                        DateTime toDate = DateTime.Now.Date;
                        //startDate = DataBaseAccess.GetLogicalDay(txtFromMonth.Text);
                        //endDate = DataBaseAccess.GetLogicalDayEnd(txtToMonth.Text);
                        fromDate = DateTime.ParseExact(txtFromMonth.Text, "MMM-yyyy", CultureInfo.InvariantCulture);
                        toDate = DateTime.ParseExact(txtToMonth.Text, "MMM-yyyy", CultureInfo.InvariantCulture);
                        startDate = fromDate.ToString("yyyy-MM-dd");
                        endDate = toDate.ToString("yyyy-MM-dd");
                        dtEnergyDate = DataBaseAccess.GetMonthWiseEnergyDataToExport(startDate, endDate, ddlPlantId.SelectedValue, selectedMachines, ddlShift.SelectedValue, ddlFormat.SelectedValue, ddlMachineType.SelectedValue.ToString(), showTopEnergyConsumption, out dt2, out dt3);
                    }
                    else if (ddlFormat.SelectedValue.Equals("Time Consolidated"))
                    {
                        DateTime fromDate = DateTime.Now.Date;
                        DateTime toDate = DateTime.Now.Date;
                        //startDate = DataBaseAccess.GetLogicalDay(txtFromDateTime.Text);
                        //endDate = DataBaseAccess.GetLogicalDayEnd(txtToDateTime.Text);
                        fromDate = DateTime.ParseExact(txtFromDateTime.Text, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        toDate = DateTime.ParseExact(txtToDateTime.Text, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        startDate = fromDate.ToString("yyyy-MM-dd HH:mm:ss");
                        endDate = toDate.ToString("yyyy-MM-dd HH:mm:ss");
                        dtEnergyDate = DataBaseAccess.GetEnergyDataToExport(startDate, endDate, ddlPlantId.SelectedValue, selectedMachines, ddlShift.SelectedValue, ddlFormat.SelectedValue, ddlMachineType.SelectedValue.ToString(), showTopEnergyConsumption);
                    }
                    else
                    {
                        //startDate = DataBaseAccess.GetLogicalDay(txtFromDate.Text);
                        //endDate = DataBaseAccess.GetLogicalDayEnd(txtToDate.Text);
                        DateTime dateTime1 = DateTime.ParseExact(txtFromDate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        DateTime dateTime2 = DateTime.ParseExact(txtToDate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        startDate = dateTime1.ToString("yyyy-MM-dd");
                        endDate = dateTime2.ToString("yyyy-MM-dd");
                        dtEnergyDate = DataBaseAccess.GetEnergyDataToExport(startDate, endDate, ddlPlantId.SelectedValue, selectedMachines, ddlShift.SelectedValue, ddlFormat.SelectedValue, ddlMachineType.SelectedValue.ToString(), showTopEnergyConsumption);
                    }

                    if (dtEnergyDate.Columns.Count <= 0)
                    {
                        isGenerated = "NodataFound";

                    }
                    else
                    {
                        List<GridSettings> gridSettings = DataBaseAccess.GetGridInformation(ddlMachineType.SelectedValue.ToString());
                        foreach (GridSettings settings in gridSettings)
                        {
                            if (!settings.Visibility)
                            {
                                if (!ddlFormat.SelectedValue.Equals("Month"))
                                    if (dtEnergyDate.Columns.Contains(settings.ColumnName))
                                    {
                                        dtEnergyDate.Columns.Remove(settings.ColumnName);
                                    }
                            }
                        }
                        if (!ddlFormat.SelectedValue.Equals("Month"))
                        {
                            if (ddlMachineType.SelectedValue.ToString().Equals("Non-Machine EM"))
                            {
                                //dtEnergyDate.Columns.Remove("Cutting_Time");
                                dtEnergyDate.Columns.Remove("CompOpn");
                            }
                            dtEnergyDate.Columns.Remove("Cutting_Time");
                        }
                        if (dtEnergyDate.Columns.Contains("ROWNO"))
                        {
                            dtEnergyDate.Columns.Remove("ROWNO");
                        }
                        if (!ddlFormat.SelectedValue.Equals("Time Consolidated"))
                        {
                            //if (dtEnergyDate.Columns.Contains("StartTime"))
                            //{
                            //    dtEnergyDate.Columns.Remove("StartTime");
                            //}
                            if (dtEnergyDate.Columns.Contains("EndTime"))
                            {
                                dtEnergyDate.Columns.Remove("EndTime");
                            }
                        }
                        if (ddlMachineType.SelectedValue.ToString().Equals("Non-Machine EM", StringComparison.OrdinalIgnoreCase))
                        {
                            if (dtEnergyDate.Columns.Contains("ProdTime_KWH"))
                            {
                                dtEnergyDate.Columns.Remove("ProdTime_KWH");
                            }
                            if (dtEnergyDate.Columns.Contains("DownTime_KWH"))
                            {
                                dtEnergyDate.Columns.Remove("DownTime_KWH");
                            }
                            if (dtEnergyDate.Columns.Contains("KWHPerComponent"))
                            {
                                dtEnergyDate.Columns.Remove("KWHPerComponent");
                            }
                        }
                        if (EnergySource.Equals("FromEnergyTable", StringComparison.OrdinalIgnoreCase))
                        {
                            if (dtEnergyDate.Columns.Contains("ProdTime_KWH"))
                            {
                                dtEnergyDate.Columns.Remove("ProdTime_KWH");
                            }
                            if (dtEnergyDate.Columns.Contains("DownTime_KWH"))
                            {
                                dtEnergyDate.Columns.Remove("DownTime_KWH");
                            }
                        }
                        if(ddlFormat.SelectedValue.Equals("Time Consolidated", StringComparison.OrdinalIgnoreCase))
                        {
                            if (dtEnergyDate.Columns.Contains("StartTime"))
                            {
                                dtEnergyDate.Columns.Remove("StartTime");
                            }
                            if (dtEnergyDate.Columns.Contains("EndTime"))
                            {
                                dtEnergyDate.Columns.Remove("EndTime");
                            }
                        }
                        isGenerated = ReportGenerator.GenerateEnergyReport(ddlFormat.SelectedValue.ToString(), "Format - I", dtEnergyDate, dt2, dt3, startDate, endDate, ddlPlantId.SelectedValue.ToString(), selectedMachines, ddlShift.SelectedValue.ToString(), ddlFormat.SelectedValue.ToString(), ddlMachineType.SelectedValue.ToString());
                    }
                    if (isGenerated.Equals("NotGenerated", StringComparison.OrdinalIgnoreCase))
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);
                    else if (isGenerated.Equals("NodataFound", StringComparison.OrdinalIgnoreCase))
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNodata", "messageNodata();", true);
                    else if (isGenerated.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "messageOk();", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }

        }
        private bool ValidateFeilds()
        {
            //if (chkListMachine.SelectedItems.Count == 0)
            //{
            //    CustomDialogBox frm = new CustomDialogBox("Error Message", "Please Select Machine ID.");
            //    frm.Show();
            //    chkListMachine.Focus();
            //    return true;
            //}

            if (CompareDates())
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageWarning", "messageWarning('From-Date cannot be greater than To-Date.');", true);
                return true;
            }

            if (CheckDateValues())
            {
                //if (ddlFormat.SelectedValue.Equals("Shift"))
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageWarning", "messageWarning('Date-Time Period Cannot be greater than 7 Days.');", true);
                //}

                //else if (ddlFormat.SelectedValue.Equals("Month"))
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageWarning", "messageWarning('Date Period Cannot be greater than 1 Year.');", true);
                //}

                //else
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageWarning", "messageWarning('Date-Time Period Cannot be greater than 31 Days.');", true);
                //}

                return true;
            }

            return false;
        }

        private bool CompareDates()
        {
            bool isDateGreater = false;
            try
            {
                string dateVal1 = string.Empty;
                string dateVal2 = string.Empty;

                if (ddlFormat.SelectedValue.Equals("Shift") || ddlFormat.SelectedValue.Equals("Day"))
                {
                    dateVal1 = txtFromDate.Text.ToString();
                    dateVal2 = txtToDate.Text.ToString();
                }

                else if (ddlFormat.SelectedValue.Equals("Month"))
                {
                    dateVal1 = txtFromMonth.Text.ToString();
                    dateVal2 = txtToMonth.Text.ToString();
                }
                else
                {
                    dateVal1 = txtFromDateTime.Text.ToString();
                    dateVal2 = txtToDateTime.Text.ToString();
                }

                DateTime dt1 = Util.GetDateTime(dateVal1);
                DateTime dt2 = Util.GetDateTime(dateVal2);

                if (dt1 > dt2)
                {
                    isDateGreater = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return isDateGreater;
        }

        private bool CheckDateValues()
        {
            bool isDateGreater = false;
            try
            {
                string dateVal1 = txtFromDate.Text.ToString();
                string dateVal2 = txtToDate.Text.ToString();

                if (ddlFormat.SelectedValue.Equals("Shift") || ddlFormat.SelectedValue.Equals("Day"))
                {
                    dateVal1 = txtFromDate.Text.ToString();
                    dateVal2 = txtToDate.Text.ToString();
                }

                else if (ddlFormat.SelectedValue.Equals("Month"))
                {
                    dateVal1 = txtFromMonth.Text.ToString();
                    dateVal2 = txtToMonth.Text.ToString();
                }
                else
                {
                    dateVal1 = txtFromDateTime.Text.ToString();
                    dateVal2 = txtToDateTime.Text.ToString();
                }


                DateTime dt1 = Util.GetDateTime(dateVal1);
                DateTime dt2 = Util.GetDateTime(dateVal2);
                var val = (dt2 - dt1).Days;
                if (ddlFormat.SelectedValue.Equals("Shift"))
                {
                    if (val > 7)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageWarning", "messageWarning('Date-Time Period Cannot be greater than 7 Days.');", true);
                        isDateGreater = true;
                        return isDateGreater;
                    }
                }
                else if (ddlFormat.SelectedValue.Equals("Month"))
                {
                    if (val > 366)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageWarning", "messageWarning('Date Period Cannot be greater than 1 Year.');", true);
                        isDateGreater = true;
                        return isDateGreater;
                    }
                }
                else
                {
                    if (val > 31)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageWarning", "messageWarning('Date-Time Period Cannot be greater than 31 Days.');", true);
                        isDateGreater = true;
                        return isDateGreater;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return isDateGreater;
        }

        protected void ddlMachineType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (System.Web.Configuration.WebConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                allCellIDs = DataBaseAccess.GetCellIDs("");
                if (allCellIDs.Count > 0)
                {
                    ddlMachineIDs.DataSource = DataBaseAccess.GetAllEM_Machines_Kiswok(ddlPlantId.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantId.SelectedValue.ToString(), ddlCellID.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCellID.SelectedValue.ToString(), "Report", ddlMachineType.SelectedValue.ToString());
                    ddlMachineIDs.DataBind();
                    foreach (ListItem item in ddlMachineIDs.Items)
                    {
                        item.Selected = true;
                    }
                }
                else
                {
                    ddlMachineIDs.DataSource = DataBaseAccess.GetAllEM_Machines(ddlPlantId.SelectedValue, ddlMachineType.SelectedValue.ToString());
                    ddlMachineIDs.DataBind();
                    foreach (ListItem item in ddlMachineIDs.Items)
                    {
                        item.Selected = true;
                    }
                }
                if (ddlMachineType.SelectedValue.ToString().Equals("Non-Machine EM", StringComparison.OrdinalIgnoreCase))
                {
                    trCell.Visible = false;
                    trPlant.Visible = false;
                }
                else
                {
                    trCell.Visible = true;
                    trPlant.Visible = true;
                }
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
                List<string> list = DataBaseAccess.GetCellIDs(ddlPlantId.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantId.SelectedValue.ToString());
                ddlCellID.DataSource = list;
                ddlCellID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (System.Web.Configuration.WebConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                allCellIDs = DataBaseAccess.GetCellIDs("");
                if (allCellIDs.Count > 0)
                {
                    ddlMachineIDs.DataSource = DataBaseAccess.GetAllEM_Machines_Kiswok(ddlPlantId.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantId.SelectedValue.ToString(), ddlCellID.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCellID.SelectedValue.ToString(), "Report", ddlMachineType.SelectedValue.ToString());
                    ddlMachineIDs.DataBind();
                    foreach (ListItem item in ddlMachineIDs.Items)
                    {
                        item.Selected = true;
                    }
                }
                else
                {
                    ddlMachineIDs.DataSource = DataBaseAccess.GetAllEM_Machines(ddlPlantId.SelectedValue, ddlMachineType.SelectedValue.ToString());
                    ddlMachineIDs.DataBind();
                    foreach (ListItem item in ddlMachineIDs.Items)
                    {
                        item.Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}