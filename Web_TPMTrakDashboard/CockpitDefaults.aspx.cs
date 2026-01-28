using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class CockpitDefaults : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadCockpitDefaultSettings();
            }
        }

        private void loadCockpitDefaultSettings()
        {
            try
            {
                CockpitDefaultsValues cockpitDefaultsValues = DataBaseAccess.GetCockpitDefaultsValues();
                ddlTimeIn.SelectedValue = cockpitDefaultsValues.TimeIn;
                ddlDefaultDisplay.SelectedValue = cockpitDefaultsValues.DefaultDisplay;
                ddlMachines.SelectedValue = cockpitDefaultsValues.TpmEnbMac;
                //if (cockpitDefaultsValues.TpmEnbMac.Equals("E"))
                //    ddlMachines.SelectedValue = "TPM Track Enabled Machines Only";
                //else
                //    ddlMachines.SelectedValue = "All Machines";
                ddlVDG_PCD.SelectedValue = cockpitDefaultsValues.VDG_PCD;
                ddlVDG_CD.SelectedValue = cockpitDefaultsValues.VDG_CD;
                ddlDisplayTime.SelectedValue = cockpitDefaultsValues.DisplayTime;
                ddlIVDO.SelectedValue = cockpitDefaultsValues.IVDO;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataBaseAccess.SaveCockpitDefaultsSettings("TimeFormat", ddlTimeIn.SelectedValue);
                DataBaseAccess.SaveCockpitDefaultsSettings("DefaultShift", ddlDefaultDisplay.SelectedValue);
                DataBaseAccess.SaveCockpitDefaultsSettings("DisplayTTFormat", ddlDisplayTime.SelectedValue);
                DataBaseAccess.SaveCockpitDefaultsSettings("DisplayinIconicView", ddlIVDO.SelectedValue);
                DataBaseAccess.SaveCockpitDefaultsSettings("VDG-CycleDefinition", ddlVDG_PCD.SelectedItem.Text, Convert.ToInt32(ddlVDG_PCD.SelectedValue));
                DataBaseAccess.SaveCockpitDefaultsSettings("VDG-Componentsetting", ddlVDG_CD.SelectedItem.Text, Convert.ToInt32(ddlVDG_CD.SelectedValue));
                DataBaseAccess.saveMachineSetting(ddlMachines.SelectedValue);
                //if (ddlMachines.SelectedItem.Text.Equals("TPM Track Enabled Machines Only"))
                //    DataBaseAccess.saveMachineSetting("E");
                //else
                //    DataBaseAccess.saveMachineSetting("D");
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "Success", "openToast()", true);
                loadCockpitDefaultSettings();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}