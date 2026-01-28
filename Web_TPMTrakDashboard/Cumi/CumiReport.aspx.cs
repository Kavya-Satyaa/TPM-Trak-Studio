using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Cumi.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Cumi
{
    public partial class CumiReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindReportType();
                BindEmploee();
                BindPlant();
                BindShift();
                BindFrequency();
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindItemCode();
                ddlReportType_SelectedIndexChanged(null, null);
            }
        }

        private void BindFrequency()
        {
            try
            {
                List<string> list = new List<string> { "3 Month", "6 Month" };
                lstFrequency.DataSource = list;
                lstFrequency.DataBind();
                for (int i = 0; i < lstFrequency.Items.Count; i++)
                {
                    lstFrequency.Items[i].Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindFrequency: " + ex.Message);
            }
        }

        private void BindItemCode()
        {
            try
            {
                List<string> list = DataBaseAccess.getcomponentbydatecomponent(Util.GetDateTime(txtFromDate.Text).ToString("yyyy-MM-dd"),Util.GetDateTime(txtToDate.Text).ToString("yyyy-MM-dd"),"");
                lbItemCode.DataSource = list;
                lbItemCode.DataBind();
                for (int i = 0; i < lbItemCode.Items.Count; i++)
                {
                    lbItemCode.Items[i].Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPONumber: " + ex.Message);
            }
        }

        private void BindEmploee()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllEmployees();
                if (list.Contains("All")) list.Remove("All");
                lbEmployee.DataSource = list;
                lbEmployee.DataBind();
                for(int i = 0; i < lbEmployee.Items.Count; i++)
                {
                    lbEmployee.Items[i].Selected = true;
                }
                //if (lbEmployee.Items.Count > 0)
                //{
                //    lbEmployee.Items[0].Selected = true;
                //}

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindEmploee: " + ex.Message);
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
                List<string> list = CumiDBAccess.GetAllMachinedByPlant(ddlPlant.SelectedValue);
                lbMachine.DataSource = list;
                lbMachine.DataBind();
                for (int i = 0; i < lbMachine.Items.Count; i++)
                {
                    lbMachine.Items[i].Selected = true;
                }

                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
                //if (lbMachine.Items.Count > 0)
                //{
                //    lbMachine.Items[0].Selected = true;
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachine: " + ex.Message);
            }
        }
        private void BindShift()
        {
            try
            {
                //List<PDTData> shiftDetails = DataBaseAccess.getShiftTimeDetails(DateTime.Now);
                //List<ListItem> list = new List<ListItem>();
                //foreach (PDTData data in shiftDetails)
                //{
                //    list.Add(new ListItem() { Text = data.ShiftName, Value = data.FromDateTime + ";;" + data.ToDateTime });
                //}
                //lbShift.DataSource = list;
                //lbShift.DataTextField = "Text";
                //lbShift.DataValueField = "Value";
                //lbShift.DataBind();
                List<string> list = DataBaseAccess.GetAllShifts("");
                if (list.Contains("All")) list.Remove("All");
                lbShift.DataSource = list;
                lbShift.DataBind();

                for (int i = 0; i < lbShift.Items.Count; i++)
                {
                    lbShift.Items[i].Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindShift: " + ex.Message);
            }
        }
        private void BindReportType()
        {
            try
            {
                List<ListItem> list = new List<ListItem>();
                list.Add(new ListItem() { Text = "Production - Shift Wise Report", Value = "ProductionShiftWise" });
                list.Add(new ListItem() { Text = "Production - Daily Wise Report", Value = "ProductionDailyWise" });
                list.Add(new ListItem() { Text = "Hydraulic Temp & Pressure Report", Value = "HydraulicTempPressure" });
                list.Add(new ListItem() { Text = "Cooling Tower Report", Value = "CoolingTower" });
                list.Add(new ListItem() { Text = "OEE Report", Value = "OEEReport" });
                list.Add(new ListItem() { Text = "Motor Status Report", Value = "MotorStatus" });
                list.Add(new ListItem() { Text = "Energy Report", Value = "EnergyReport" });
                list.Add(new ListItem() { Text = "Product Parameter Report", Value = "ProductParameterReport" });
                list.Add(new ListItem() { Text = "PM - Quarterly Compliance Report", Value = "QuarterlyCompliance" });
                list.Add(new ListItem() { Text = "PM - Frequencywise Report", Value = "FrequencywiseReport" });
                list.Add(new ListItem() { Text = "PM - Shift Activity Report", Value = "ShiftPMActivityReport" });

                //list.Add(new ListItem() { Text = "Quality - Shift Wise", Value = "RejectionShiftWise" });

                ddlReportType.DataSource = list;
                ddlReportType.DataTextField = "Text";
                ddlReportType.DataValueField = "Value";
                ddlReportType.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindReportType: " + ex.Message);
            }
        }

        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachine();
        }
        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string retporttype = ddlReportType.SelectedValue;

                trFromDate.Visible = false;
                trToDate.Visible = false;
                trDate.Visible = false;
                trEmployee.Visible = false;
                trShift.Visible = false;
                trMachine.Visible = false;
                trPlant.Visible = false;
                trItemCode.Visible = false;
                trYear.Visible = false;
                trFrequency.Visible = false;
                trddMachine.Visible = false;

                if (retporttype.Equals("ProductionDailyWise", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trEmployee.Visible = true;
                    trMachine.Visible = true;
                    trPlant.Visible = true;
                }
                else if (retporttype.Equals("ProductionShiftWise", StringComparison.OrdinalIgnoreCase))
                {
                    trDate.Visible = true;
                    trEmployee.Visible = true;
                    trShift.Visible = true;
                    trMachine.Visible = true;
                    trPlant.Visible = true;
                }
                else if(retporttype.Equals("HydraulicTempPressure", StringComparison.OrdinalIgnoreCase))
                {
                    trDate.Visible = true;
                    trEmployee.Visible = false;
                    trShift.Visible = true;
                    trMachine.Visible = true;
                    trPlant.Visible = true;
                }
                else if (retporttype.Equals("CoolingTower", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trEmployee.Visible = false;
                    trShift.Visible = true;
                    trMachine.Visible = false;
                    trPlant.Visible = false;
                }
                else if (retporttype.Equals("OEEReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trEmployee.Visible = false;
                    trShift.Visible = true;
                    trMachine.Visible = true;
                    trPlant.Visible = true;
                }
                else if(retporttype.Equals("MotorStatus",StringComparison.OrdinalIgnoreCase))
                {
                    trDate.Visible = true;
                    trEmployee.Visible = false;
                    trShift.Visible = true;
                    trMachine.Visible = true;
                    trPlant.Visible = true;
                }
                else if (retporttype.Equals("EnergyReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trEmployee.Visible = false;
                    trShift.Visible = false;
                    trMachine.Visible = true;
                    trPlant.Visible = true;
                    trItemCode.Visible = true;
                    BindItemCode();
                }
                else if(retporttype.Equals("ProductParameterReport",StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trEmployee.Visible = false;
                    trShift.Visible = true;
                    trMachine.Visible = true;
                    trPlant.Visible = true;
                }
                else if (retporttype.Equals("QuarterlyCompliance", StringComparison.OrdinalIgnoreCase))
                {
                    trEmployee.Visible = false;
                    trMachine.Visible = true;
                    trPlant.Visible = true;
                    trYear.Visible = true;
                }
                else if (retporttype.Equals("FrequencywiseReport", StringComparison.OrdinalIgnoreCase))
                {
                    trYear.Visible = true;
                    trFrequency.Visible = true;
                    trMachine.Visible = true;
                    trPlant.Visible = true;
                }
                else if (retporttype.Equals("ShiftPMActivityReport", StringComparison.OrdinalIgnoreCase))
                {
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                    trddMachine.Visible = true;
                    trPlant.Visible = true;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlReportType_SelectedIndexChanged: " + ex.Message);
            }
        }
        public static string GetMultiselectInputValue(string value)
        {
            string output = "";
            try
            {
                if (!String.IsNullOrEmpty(value))
                {
                    output = "'" + Regex.Replace(value, ",", "','") + "'";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetMultiselectInputValue" + ex.Message);
            }
            return output;
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string Generated = "";
                string machinedisplay = "",Frequency="";
                string machine = GetMultiselectInputValue(String.Join(",", (lbMachine.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Value).ToArray())));
                string emp = GetMultiselectInputValue(String.Join(",", (lbEmployee.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Value).ToArray())));
                string shift = GetMultiselectInputValue(String.Join(",", (lbShift.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Value).ToArray())));
                string comp= GetMultiselectInputValue(String.Join(",", (lbItemCode.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Value).ToArray())));
                DateTime fromdate = Util.GetDateTime(txtFromDate.Text);
                DateTime toDate= Util.GetDateTime(txtToDate.Text);

                if (machine.Split(',').Count().Equals(lbMachine.Items.Count))
                    machinedisplay = "All";
                else
                    machinedisplay = machine;

                string retporttype = ddlReportType.SelectedValue;
                if (retporttype.Equals("ProductionDailyWise", StringComparison.OrdinalIgnoreCase))
                {
                    DataTable dt = CumiDBAccess.GetProductionDayWiseReportDetails("'" + ddlPlant.SelectedValue + "'", machine, txtFromDate.Text, txtToDate.Text, emp);

                    Generated = TMPTrakGenerateReport.GenerateCumiProductionDaywiseReport(dt, ddlPlant.SelectedValue);
                }
                else if (retporttype.Equals("ProductionShiftWise", StringComparison.OrdinalIgnoreCase))
                {

                    DataTable dt = CumiDBAccess.GetProductionShiftWiseReportDetails("'" + ddlPlant.SelectedValue + "'", machine, txtDate.Text, emp, shift);

                    Generated = TMPTrakGenerateReport.GenerateCumiProductionShiftwiseReport(dt, ddlPlant.SelectedValue);
                }
                else if (retporttype.Equals("HydraulicTempPressure", StringComparison.OrdinalIgnoreCase))
                {
                    DataTable dt = CumiDBAccess.GetHydraulicTempPressureReportDetails("'" + ddlPlant.SelectedValue + "'", machine, txtDate.Text, shift);
                    Generated = TMPTrakGenerateReport.GenerateCumiHydraulicTempPressureReport(dt, ddlPlant.SelectedValue, machinedisplay, txtDate.Text, shift);
                }
                else if (retporttype.Equals("CoolingTower", StringComparison.OrdinalIgnoreCase))
                {
                    DataTable dt = CumiDBAccess.GetCoolingTowerReportDetails(fromdate,toDate, shift);
                    Generated = TMPTrakGenerateReport.GenerateCumiCoolingTowerReport(dt,fromdate,toDate, shift);
                }
                else if (retporttype.Equals("OEEReport", StringComparison.OrdinalIgnoreCase))
                {
                    DataTable dt = CumiDBAccess.GetOEEReportDetails(fromdate, toDate, "'" + ddlPlant.SelectedValue + "'", machine, shift);
                    DataTable DownList = CumiDBAccess.GetDownList();
                    Generated = TMPTrakGenerateReport.GenerateCumiOEEReport(DownList,dt, fromdate, toDate, machinedisplay, shift);
                }
                else if(retporttype.Equals("MotorStatus",StringComparison.OrdinalIgnoreCase))
                {
                    DataTable dt = CumiDBAccess.GetMotorStatusReportDetails(txtDate.Text, "'" + ddlPlant.SelectedValue + "'", machine, shift);
                    Generated = TMPTrakGenerateReport.GenerateCumiMotorStatusReport(dt, txtDate.Text, machinedisplay, shift,ddlPlant.SelectedValue);
                }
                else if (retporttype.Equals("EnergyReport", StringComparison.OrdinalIgnoreCase))
                {
                    fromdate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(txtFromDate.Text));
                    toDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtToDate.Text));
                    DataTable dt = CumiDBAccess.GetEnergyReportDetails(fromdate,toDate, "'" + ddlPlant.SelectedValue + "'", machine, comp);
                    Generated = TMPTrakGenerateReport.GenerateCumiEnergyReport(dt, fromdate, toDate, ddlPlant.SelectedValue, machinedisplay, comp);
                }
                else if (retporttype.Equals("ProductParameterReport", StringComparison.OrdinalIgnoreCase))
                {
                    DataTable dt = CumiDBAccess.GetProductParameterReportDetails(fromdate,toDate, "'" + ddlPlant.SelectedValue + "'", machine, shift);
                    Generated = TMPTrakGenerateReport.GenerateCumiProductParameterReport(dt, fromdate, toDate, machinedisplay, shift, ddlPlant.SelectedValue);
                }
                else if (retporttype.Equals("QuarterlyCompliance", StringComparison.OrdinalIgnoreCase))
                {
                    string machineIDs = String.Join(",", (lbMachine.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Value).ToArray()));
                    DataTable dt = CumiDBAccess.GetQuarterlyComplianceDetails(Convert.ToInt32(txtYearOnly.Text), "'" + ddlPlant.SelectedValue + "'", machineIDs);
                    Generated = TMPTrakGenerateReport.GenerateCumiQuarterlyComplianceReport(dt, Convert.ToInt32(txtYearOnly.Text), machinedisplay);
                }
                else if (retporttype.Equals("FrequencywiseReport", StringComparison.OrdinalIgnoreCase))
                {
                    machine = String.Join(",", (lbMachine.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Value).ToArray()));
                    Frequency= String.Join(",", (lstFrequency.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Value).ToArray()));
                    DataTable dt = CumiDBAccess.GetFrequencywiseReportDetails(machine, txtYearOnly.Text, Frequency);
                    Generated = TMPTrakGenerateReport.GenerateFrequencywiseReportCumi(dt);
                }
                else if (retporttype.Equals("ShiftPMActivityReport", StringComparison.OrdinalIgnoreCase))
                {
                    fromdate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(txtFromDate.Text));
                    toDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtToDate.Text));
                    DataTable dt = CumiDBAccess.GetShiftPMActivityReportDetails(fromdate, toDate, "'" + ddlPlant.SelectedValue + "'", ddlMachine.SelectedValue);
                    Generated = TMPTrakGenerateReport.GenerateCumiShiftPMActivityReport(dt, txtFromDate.Text, txtToDate.Text, ddlMachine.SelectedValue);
                }


                if (Generated.Equals("", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Error");
                }
                else if (Generated.Equals("NodataFound", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openWarningToastrModal(this, "No Data Found");
                }
                else if (Generated.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                {
                    //HelperClass.ope(this, "No Data Found");
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnExport_Click: " + ex.Message);
            }
        }

        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            if(ddlReportType.SelectedValue.Equals("EnergyReport", StringComparison.OrdinalIgnoreCase))
            {
                BindItemCode();
            }
        }

        protected void txtToDate_TextChanged(object sender, EventArgs e)
        {
            if (ddlReportType.SelectedValue.Equals("EnergyReport", StringComparison.OrdinalIgnoreCase))
            {
                BindItemCode();
            }
        }
    }
}