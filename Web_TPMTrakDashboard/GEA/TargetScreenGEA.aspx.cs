using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;
using Web_TPMTrakDashboard.GEA.Models;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class TargetScreenGEA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPlantID();
                BindData();
            }
        }
        private void BindData()
        {
            try
            {
                List<TargetValues> Values = GEADatabaseAccess.GetTargetScreenData(ddlPlantID.SelectedValue.ToString(),txtEmployeeName.Text);
                //List<TargetValues> List = Values.Where(x => x.EmployeeName.StartsWith(txtEmployeeName.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                lvEmpTargets.DataSource = Values;
                lvEmpTargets.DataBind();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                TargetValues data = new TargetValues();
                int result = 0;
                for (int i = 0; i < lvEmpTargets.Items.Count; i++)
                {
                    var row = lvEmpTargets.Items[i];
                    data.PlantID= ddlPlantID.SelectedValue.ToString();
                    data.EmployeeID= (row.FindControl("lblEmpID") as Label).Text;
                    data.TargetValue= (row.FindControl("txtTargetValue") as TextBox).Text;
                    result += GEADatabaseAccess.SaveTargetScreenData(data);
                }
                if (result > 0)
                {
                    HelperClassGeneric.openInsertSuccessModal(this);
                    BindData();
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void BindPlantID()
        {

            try
            {
                List<string> PlantIds = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllPlants();
                if (PlantIds != null && PlantIds.Count > 0)
                {
                    PlantIds.RemoveAt(0);
                    ddlPlantID.DataSource = PlantIds;
                    ddlPlantID.DataBind();
                    ddlPlantID.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        
    }
}