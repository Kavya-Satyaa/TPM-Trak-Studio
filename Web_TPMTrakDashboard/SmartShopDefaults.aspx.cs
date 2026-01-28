using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class SmartShopDefaults : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AssignDefaultValues();
            }
        }



        private void AssignPlannedDownTimes()
        {
            ddlIgnoreCount.SelectedValue = DataBaseAccess.GetPlannedDownTimeValues("Ignore_Count_4m_PLD");
            ddlIgnoreDowntime.SelectedValue = DataBaseAccess.GetPlannedDownTimeValues("Ignore_Dtime_4m_PLD");
            ddlIgnoreProductionTime.SelectedValue = DataBaseAccess.GetPlannedDownTimeValues("Ignore_Ptime_4m_PLD");
            ddlIgnoreAvgCylcetime.SelectedValue = DataBaseAccess.GetPlannedDownTimeValues("Ignore_AvgCycletime_4m_PLD");
        }


        private void AssignDefaultValues()
        {
            ShopDefaultsValues shopDefaultsValues = DataBaseAccess.GetShopDefaultsValues();


            if (shopDefaultsValues.TimeFormat != null)
                ddlTimeFormat.SelectedValue = shopDefaultsValues.TimeFormat;
            if (shopDefaultsValues.TargetForm != null)
                ddlTargetFrom.SelectedValue = shopDefaultsValues.TargetForm;
            if (shopDefaultsValues.CycleIgnoreThreshold != null)
                ddlCycleIgnoreThreshold.SelectedValue = (shopDefaultsValues.CycleIgnoreThreshold);
            if (shopDefaultsValues.MinLuForLr >= 0)
                txtMinLUForLR.Text = shopDefaultsValues.MinLuForLr.ToString();
            if (shopDefaultsValues.Jobcardupdate != null)
                ddlJCUpdate.SelectedValue = (shopDefaultsValues.Jobcardupdate);
            if (shopDefaultsValues.FiancialyearFrom > 0 && shopDefaultsValues.FiancialyearFrom <= 12)
                ddlFinancialYearFrom.SelectedValue = (shopDefaultsValues.FiancialyearFrom).ToString();
            if (shopDefaultsValues.SmartData != null)
                ddlSmartdata.SelectedValue = (shopDefaultsValues.SmartData);
            if (shopDefaultsValues.JobCardSetting != null)
                ddlJCSetting.SelectedValue = (shopDefaultsValues.JobCardSetting);
            if (shopDefaultsValues.SmartTransAutoCloseTime != 0)
                txtSACTime.Text = shopDefaultsValues.SmartTransAutoCloseTime.ToString();
            if (shopDefaultsValues.InterlockTime != 0)
                txtInterLockTime.Text = shopDefaultsValues.InterlockTime.ToString();
            if (shopDefaultsValues.EMweightsDisplay != null)
                ddlEWDisplay.SelectedValue = (shopDefaultsValues.EMweightsDisplay);
            if (shopDefaultsValues.SmartAgentShutDown != null)
                txtSASTime.Text = shopDefaultsValues.SmartAgentShutDown;
            if (shopDefaultsValues.SubOperation != null)
                ddlsuboperation.SelectedValue = shopDefaultsValues.SubOperation;
            chkQEEnabled.Checked = shopDefaultsValues.QEEnabled;
            ddlCompInfoDataType.SelectedValue = string.IsNullOrEmpty(shopDefaultsValues.CompInterfaceIDDataType) ? "Numeric" : shopDefaultsValues.CompInterfaceIDDataType;
            bool checkPlantOee, checkTimeConsolidated;
            DataBaseAccess.setCheckBoxListChecked(out checkPlantOee, out checkTimeConsolidated);

            cblShowZRReport.Items.FindByValue("Plant OEE").Selected = checkPlantOee;
            cblShowZRReport.Items.FindByValue("Time Consolidated").Selected = checkTimeConsolidated;

            txtType1Threshold.Text = DataBaseAccess.GetThreshold("Type1Threshold");
            txtType11Threshold.Text = DataBaseAccess.GetThreshold("Type11Threshold");
            txtType40Threshold.Text = DataBaseAccess.GetThreshold("Type40Threshold");
            AssignPlannedDownTimes();

            if (System.Web.Configuration.WebConfigurationManager.AppSettings["AnjaliPages"].ToString() == "1")
            {
                operatoeIncentiveSettingDiv.Visible = true;
                DataTable dtOprInsentiveReport = DataBaseAccess.getOprInsentiveReportSetting("OperatorIncentiveReport");
                if (dtOprInsentiveReport.Rows.Count > 0)
                {
                    txtOprIncentiveOpnRate.Text = dtOprInsentiveReport.AsEnumerable().Where(k => k.Field<string>("ValueInText") == "OperationRate").Select(k => k.Field<int>("ValueInInt")).FirstOrDefault().ToString();
                    txtOprIncentiveHourlyIncentive.Text = dtOprInsentiveReport.AsEnumerable().Where(k => k.Field<string>("ValueInText") == "HourlyIncentive").Select(k => k.Field<int>("ValueInInt")).FirstOrDefault().ToString();
                    txtOprIncentiveShiftIncentiveInRs.Text = dtOprInsentiveReport.AsEnumerable().Where(k => k.Field<string>("ValueInText") == "ShiftIncentiveInRs").Select(k => k.Field<int>("ValueInInt")).FirstOrDefault().ToString();
                    txtOprIncentiveShiftTarget.Text = dtOprInsentiveReport.AsEnumerable().Where(k => k.Field<string>("ValueInText") == "ShiftTarget").Select(k => k.Field<int>("ValueInInt")).FirstOrDefault().ToString();
                }
            }
            else
            {
                operatoeIncentiveSettingDiv.Visible = false;
            }
            if (System.Web.Configuration.WebConfigurationManager.AppSettings["RexnordPages"].ToString() == "1")
            {
                txtLoadUnloadInSeconds.Text = shopDefaultsValues.LoadUnloadInSeconds.ToString();
                txtLoadUnloadThreshold.Text = shopDefaultsValues.LoadUnloadThreshold.ToString();
            }
            DataTable dtShopDefault = DataBaseAccess.getOprInsentiveReportSetting("LiveReport");
            if (dtShopDefault.Rows.Count > 0)
            {
                string ddlValue = dtShopDefault.AsEnumerable().Where(k => k.Field<string>("ValueInText") == "DailyAndShift_Diff").Select(k => k.Field<string>("ValueInText2")).FirstOrDefault().ToString();
                if (ddlLiveReportDailyDiff.Items.FindByValue(ddlValue) != null)
                {
                    ddlLiveReportDailyDiff.SelectedValue = ddlValue;
                }
                ddlValue = dtShopDefault.AsEnumerable().Where(k => k.Field<string>("ValueInText") == "DailyAndShift_Target").Select(k => k.Field<string>("ValueInText2")).FirstOrDefault().ToString();
                if (ddlLiveReportDailyTarget.Items.FindByValue(ddlValue) != null)
                {
                    ddlLiveReportDailyTarget.SelectedValue = ddlValue;
                }
            }
            dtShopDefault = DataBaseAccess.getOprInsentiveReportSetting("Report");
            if (dtShopDefault.Rows.Count > 0)
            {
                string ddlValue = dtShopDefault.AsEnumerable().Where(k => k.Field<string>("ValueInText") == "StdSetupEffVisible").Select(k => k.Field<string>("ValueInText2")).FirstOrDefault().ToString();
                if (ddlStdSetupEffVisible.Items.FindByValue(ddlValue) != null)
                {
                    ddlStdSetupEffVisible.SelectedValue = ddlValue;
                }
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            if (ValidateTextboxes() && ValidateEfficiencyFields())
            {

                DataBaseAccess.SaveShopDefaultSettings("TimeInFormat", ddlTimeFormat.SelectedValue);
                DataBaseAccess.SaveShopDefaultSettings("SubOperation", ddlsuboperation.SelectedValue.ToString());
                DataBaseAccess.SaveShopDefaultSettings("TargetFrom", ddlTargetFrom.SelectedValue);
                DataBaseAccess.SaveShopDefaultSettings("Smartdata Settings", ddlSmartdata.SelectedValue);
                DataBaseAccess.SaveShopDefaultSettings("JobCardUpdate", ddlJCUpdate.SelectedValue);
                DataBaseAccess.SaveShopDefaultSettings("Job Card Settings", ddlJCSetting.SelectedValue);
                DataBaseAccess.SaveCockpitDefaultsSettings("Ignore_Ptime_4m_PLD", ddlIgnoreProductionTime.SelectedValue);
                DataBaseAccess.SaveCockpitDefaultsSettings("Ignore_Dtime_4m_PLD", ddlIgnoreDowntime.SelectedValue);
                DataBaseAccess.SaveCockpitDefaultsSettings("Ignore_Count_4m_PLD", ddlIgnoreCount.SelectedValue);
                DataBaseAccess.SaveCockpitDefaultsSettings("Ignore_AvgCycletime_4m_PLD", ddlIgnoreAvgCylcetime.SelectedValue);
                DataBaseAccess.SaveShopDefaultSettings("FinancialYearFrom", ddlFinancialYearFrom.SelectedItem.Text, Convert.ToInt32(ddlFinancialYearFrom.SelectedValue));
                DataBaseAccess.SaveShopDefaultSettings("EMWeightsDisplay", ddlEWDisplay.SelectedValue);
                DataBaseAccess.SaveShopDefaultSettings("CycleIgnoreThreshold", ddlCycleIgnoreThreshold.SelectedValue);
                DataBaseAccess.SaveShopDefaultSettings("SmartAgent_Shutdown", txtSASTime.Text);
                DataBaseAccess.SaveShopDefaultSettingsForTextbox("MinLUForLR", Convert.ToInt32(txtMinLUForLR.Text));
                DataBaseAccess.SaveShopDefaultSettingsForTextbox("MachineInterLockTime", Convert.ToInt32(txtInterLockTime.Text));
                DataBaseAccess.SaveShopDefaultSettingsForTextbox("SmartTrans_Application_shutdowntime", Convert.ToInt32(txtSACTime.Text));
                DataBaseAccess.SaveShopDefaultSettingsForValueinText2("ANDONStatusThreshold", "Type1Threshold", txtType1Threshold.Text);
                DataBaseAccess.SaveShopDefaultSettingsForValueinText2("ANDONStatusThreshold", "Type11Threshold", txtType11Threshold.Text);
                DataBaseAccess.SaveShopDefaultSettingsForValueinText2("ANDONStatusThreshold", "Type40Threshold", txtType40Threshold.Text);
                DataBaseAccess.SaveShopDefaultSettingsForValueInInt("QEVisibility", chkQEEnabled.Checked ? "1" : "0");
                DataBaseAccess.insertUpdateShopDefaultSettingsForValueInText2("LiveReport", "DailyAndShift_Diff", ddlLiveReportDailyDiff.SelectedValue);
                DataBaseAccess.insertUpdateShopDefaultSettingsForValueInText2("LiveReport", "DailyAndShift_Target", ddlLiveReportDailyTarget.SelectedValue);
                DataBaseAccess.insertUpdateShopDefaultSettingsForValueInText2("Report", "StdSetupEffVisible", ddlStdSetupEffVisible.SelectedValue);
                DataBaseAccess.SaveShopDefaultSettings("ComponentInfoInterfaceIdDataType", ddlCompInfoDataType.SelectedValue);
                foreach (ListItem listItem in cblShowZRReport.Items)
                {
                    DataBaseAccess.SetDataForCheckBox(listItem.Value, listItem.Selected);
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["AnjaliPages"].ToString() == "1")
                {
                    DataBaseAccess.SaveShopDefaultSettingsForValueInInt("OperatorIncentiveReport", "OperationRate", txtOprIncentiveOpnRate.Text);
                    DataBaseAccess.SaveShopDefaultSettingsForValueInInt("OperatorIncentiveReport", "HourlyIncentive", txtOprIncentiveHourlyIncentive.Text);
                    DataBaseAccess.SaveShopDefaultSettingsForValueInInt("OperatorIncentiveReport", "ShiftIncentiveInRs", txtOprIncentiveShiftIncentiveInRs.Text);
                    DataBaseAccess.SaveShopDefaultSettingsForValueInInt("OperatorIncentiveReport", "ShiftTarget", txtOprIncentiveShiftTarget.Text);
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["RexnordPages"].ToString() == "1")
                {
                    DataBaseAccess.SaveShopDefaultSettings("LoadUnloadThreshold", txtLoadUnloadThreshold.Text);
                    DataBaseAccess.SaveShopDefaultSettings("LoadUnloadInSeconds", txtLoadUnloadInSeconds.Text);

                }
                AssignDefaultValues();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "openSuccessToast2()", true);
            }
            else
            {
                AssignDefaultValues();
            }

        }

        private bool ValidateEfficiencyFields()
        {
            if (Convert.ToInt32(txtMinLUForLR.Text) > 100)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MaxValue1", "openWarningToast2('MinLUForLr value can not be greater than 100..!!')", true);
                return false;
            }
            else if (Convert.ToInt32(txtInterLockTime.Text) > 100)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MaxValue2", "openWarningToast2('InterLockTime value can not be greater than 100..!!')", true);
                return false;
            }
            else if (Convert.ToInt32(txtSACTime.Text) > 100)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MaxValue3", "openWarningToast2('SmartTrans Auto CloseTime value can not be greater than 100..!!')", true);
                return false;
            }
            else if (Convert.ToInt32(txtType11Threshold.Text) > 100)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MaxValue4", "openWarningToast2('Type11Threshold value can not be greater than 100..!!')", true);
                return false;
            }
            else if (Convert.ToInt32(txtType1Threshold.Text) > 100)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MaxValue5", "openWarningToast2('Type1Threshold value can not be greater than 100..!!')", true);
                return false;
            }
            else if (Convert.ToInt32(txtType40Threshold.Text) > 100)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MaxValue6", "openWarningToast2('Type40Threshold value can not be greater than 100..!!')", true);
                return false;
            }
            return true;
        }

        private bool ValidateTextboxes()
        {
            if (txtMinLUForLR.Text.Equals(string.Empty))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "EmptyValue1", "openWarningToast2('Please enter MinLUForLR value!!!')", true);
                return false;
            }

            if (txtSACTime.Text.Equals(string.Empty))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "EmptyValue2", "openWarningToast2('Please enter SmartTrans Auto Closetime value!!!')", true);
                return false;
            }
            if (txtInterLockTime.Text.Equals(string.Empty))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "EmptyValue3", "openWarningToast2('Please enter InerLockTime value!!!')", true);
                return false;
            }
            if (txtType11Threshold.Text.Equals(string.Empty))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "EmptyValue4", "openWarningToast2('Please enter Type11Threshold value!!!')", true);
                return false;
            }
            if (txtType1Threshold.Text.Equals(string.Empty))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "EmptyValue5", "openWarningToast2('Please enter Type1Threshold value!!!')", true);
                return false;
            }
            if (txtType40Threshold.Text.Equals(string.Empty))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "EmptyValue6", "openWarningToast2('Please enter Type40Threshold value!!!')", true);
                return false;
            }
            return true;
        }


    }
}