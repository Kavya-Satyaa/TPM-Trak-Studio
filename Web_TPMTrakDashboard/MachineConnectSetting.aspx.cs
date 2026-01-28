using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class MachineConnectSetting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                SessionClear.ClearSession();
                BindDAshboardSetting();
                BindServiceSetting();
            }
        }
        private void BindDAshboardSetting()
        {
            try
            {
                string Extensions = CockpitDataBaseAccess.bindExtensionCombobox();
                List<string> TagIds = Extensions.Split(',').ToList<string>();
                ddlProgramFileExtension.DataSource = TagIds;
                ddlProgramFileExtension.DataBind();

                MachineConnectEntity mcList = CockpitDataBaseAccess.GetFocusSettingDetails();
                if(mcList!=null)
                {
                    txtProgramFolderPath.Text = mcList.ProgramPath;
                    ddlProgramFileExtension.SelectedValue = mcList.ProgramFileExtension;
                    ddlStoppageThreshold.SelectedValue = mcList.StoppageThreshold;
                    txtOpHistoryFolderPath.Text = mcList.OperationHistoryFolderPath;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindServiceSetting()
        {
            try
            {
                MachineConnectEntity mcList = CockpitDataBaseAccess.GetServiceDetails("ServiceData");
                if (mcList != null)
                {
                    ddlLiveData.SelectedValue = mcList.LiveDataInterval;
                    ddlalarmData.SelectedValue = mcList.AlarmDataInterval;
                    ddlSpindleData.SelectedValue = mcList.SpindleDataInterval;
                    ddlOperationHistoryData.SelectedValue = mcList.OperationHistoryDataInterval;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                result = CockpitDataBaseAccess.SaveMachineConnectDashboardSetting(txtProgramFolderPath.Text, ddlProgramFileExtension.SelectedValue.ToString(), ddlStoppageThreshold.SelectedValue.ToString(), txtOpHistoryFolderPath.Text);
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);
                }
                BindDAshboardSetting();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnServiceSetting_Click(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                result = CockpitDataBaseAccess.SaveMachineConnectServiceSetting(ddlLiveData.SelectedValue.ToString(), "LiveDataInterval", "ServiceData");
                result += CockpitDataBaseAccess.SaveMachineConnectServiceSetting(ddlSpindleData.SelectedValue.ToString(), "SpindleDataInterval", "ServiceData");
                result += CockpitDataBaseAccess.SaveMachineConnectServiceSetting(ddlalarmData.SelectedValue.ToString(), "AlarmDataInterval", "ServiceData");
                result += CockpitDataBaseAccess.SaveMachineConnectServiceSetting(ddlOperationHistoryData.SelectedValue.ToString(), "OperationHistoryDataInterval", "ServiceData");
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNotOk", "messageNotOk();", true);
                }
                BindServiceSetting();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
    }

    public class MachineConnectEntity
    {
        public string LiveDataInterval { get; set; }
        public string SpindleDataInterval { get; set; }
        public string AlarmDataInterval { get; set; }
        public string OperationHistoryDataInterval { get; set; }
        public string ProgramPath { get; set; }
        public string ProgramFileExtension { get; set; }
        public string StoppageThreshold { get; set; }
        public string OperationHistoryFolderPath { get; set; }
    }
}