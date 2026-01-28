using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class GEASubOperations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Language"] == null || Session["connectionString"] == null || Session["UserName"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            else if (!IsPostBack)
            {
                BindStations();
                BindData();
            }
        }

        private void BindStations()
        {
            try
            {
                List<string> Station = DataBaseAccess.GEADatabaseAccess.GetStationDetails();
                List<string> SubActivity = DataBaseAccess.GEADatabaseAccess.GetSubActivityDetails();
                ddlStations.DataSource = Station;
                ddlStations.DataBind();
                ddlStation.DataSource = Station;
                ddlStation.DataBind();
                ddlComponent.DataSource = SubActivity;
                ddlComponent.DataBind();
                ddlComponents.DataSource = SubActivity;
                ddlComponents.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindData()
        {
            try
            {
                if (ddlStations.SelectedValue != null)
                {
                    List<SupOperation> SupOperationEntityData = DataBaseAccess.GEADatabaseAccess.GetSupOperation(ddlStations.SelectedValue.ToString(), ddlComponent.SelectedValue.ToString());
                    if (SupOperationEntityData != null && SupOperationEntityData.Count > 0)
                    {
                        gridviewsuboperation.DataSource = SupOperationEntityData;
                        gridviewsuboperation.DataBind();
                    }
                    else
                    {
                        gridviewsuboperation.DataSource = SupOperationEntityData;
                        gridviewsuboperation.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void InsertData(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlStation.SelectedValue.ToString()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Station Cannot be Empty')", true);
                    return;
                }
                if (string.IsNullOrEmpty(ddlComponents.SelectedValue.ToString()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Material ID Cannot be Empty')", true);
                    return;
                }
                else if (string.IsNullOrEmpty(txtOperation.Text.ToString()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Operation Cannot be Empty')", true);
                    return;
                }
                else if (string.IsNullOrEmpty(txtSubOperation.Text.ToString()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Activity Cannot be Empty')", true);
                    return;
                }
                else if (string.IsNullOrEmpty(txtCycleTime.Text.ToString()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('CycleTime Cannot be Empty')", true);
                    return;
                }
                else
                {
                    SupOperation Entity = new SupOperation
                    {
                        Station = ddlStation.SelectedValue.ToString(),
                        MaterialID = ddlComponents.SelectedValue.ToString(),
                        Operation = txtOperation.Text,
                        SubOperation = txtSubOperation.Text,
                        Checked = chkIsEnabled.Checked,
                        CycleTime = txtCycleTime.Text
                    };
                    DataBaseAccess.GEADatabaseAccess.SaveSubOperationData(Entity);
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        protected void UpdateData(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtUpdateSubOperation.Text.ToString()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Acticity Cannot be Empty')", true);
                    return;
                }
                else if (string.IsNullOrEmpty(txtUpdateCycleTime.Text.ToString()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('CycleTime Cannot be Empty')", true);
                    return;
                }
                else
                {
                    SupOperation Entity = new SupOperation
                    {
                        Station = txtUpdateStation.Text,
                        MaterialID = ddlComponents.Text,
                        Operation = txtUpdateOperation.Text,
                        SubOperation = txtUpdateSubOperation.Text,
                        Checked = chkIsUpdateEnabled.Checked,
                        CycleTime = txtUpdateCycleTime.Text
                    };
                    DataBaseAccess.GEADatabaseAccess.SaveSubOperationData(Entity);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Saved Successfully')", true);
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow Row in gridviewsuboperation.Rows)
                {
                    if (string.IsNullOrEmpty((Row.FindControl("lblCycleTime") as TextBox).Text))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('CycleTime Cannot be Empty')", true);
                        return;
                    }
                    SupOperation Entity = new SupOperation
                    {
                        Station = (Row.FindControl("lblstation") as Label).Text,
                        MaterialID = (Row.FindControl("lblMaterialID") as Label).Text,
                        Operation = (Row.FindControl("lblOperation") as Label).Text,
                        SubOperation = (Row.FindControl("lblSubOperation") as Label).Text,
                        Checked = (Row.FindControl("chkChecked") as CheckBox).Checked,
                        CycleTime = (Row.FindControl("lblCycleTime") as TextBox).Text,
                    };
                    DataBaseAccess.GEADatabaseAccess.SaveSubOperationData(Entity);

                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Saved Successfully')", true);
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

            try
            {
                foreach (GridViewRow Row in gridviewsuboperation.Rows)
                {
                    if ((Row.FindControl("chkDelete") as CheckBox).Checked)
                    {
                        SupOperation Entity = new SupOperation();
                        Entity.MaterialID = (Row.FindControl("lblMaterialID") as Label).Text;
                        Entity.Station = (Row.FindControl("lblstation") as Label).Text;
                        Entity.Operation = (Row.FindControl("lblOperation") as Label).Text;
                        Entity.SubOperation = (Row.FindControl("lblSubOperation") as Label).Text;
                        if (DataBaseAccess.GEADatabaseAccess.DeleteSubOperation(Entity))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Deleted Successfully')", true);
                            
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Activity is already in Use')", true);
                            
                        }
                    }
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
    }
    public class SupOperation
    {
        public string SubOperation { get; set; }
        public string MaterialID { get; set; }
        public string CycleTime { get; set; }
        public string Station { get; set; }
        public string Operation { get; set; }
        public bool Checked { get; set; }

    }
}