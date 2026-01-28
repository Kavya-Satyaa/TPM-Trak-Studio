using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Pitti
{
    public partial class DailyChecklistReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachineID();
            }
        }

        private void BindMachineID()
        {
            try
            {
                List<string> MachinesList = DataBaseAccess.GetAllMachinesFromMaster();
                ddlMachineID.DataSource = MachinesList;
                ddlMachineID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachineID: " + ex.Message);
            }
        }
       
        protected void btnExport_Click(object sender, EventArgs e)
        {
            string MachineID = "";
            try
            {
                if (ddlMachineID.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase))
                    MachineID = "";
                else
                    MachineID = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(ddlMachineID);

                string res = GenerateReport_Pitti.GenerateDailyChecksheetReport_Pitti(MachineID, txtYear.Text.Trim(), txtMonth.Text.Trim());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}