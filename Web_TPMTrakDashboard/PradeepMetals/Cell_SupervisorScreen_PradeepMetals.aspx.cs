using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;

namespace Web_TPMTrakDashboard.PradeepMetals
{
    public partial class Cell_SupervisorScreen_PradeepMetals : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPlantID();
                BindCellSupervisors();
            }
        }
        private void BindPlantID()
        {
            try
            {
                List<string> list = DBAccessPradeepMetals.GetPlantID();
                list.Insert(0, "ALL");
                ddlPlantID.DataSource = list;
                ddlPlantID.DataBind();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindCellSupervisors()
        {
            try
            {
                List<CellSupervisorScreen> list = DBAccessPradeepMetals.GetCellSupervisorData(ddlPlantID.SelectedValue == "ALL" ? "" : ddlPlantID.SelectedValue);
                gvCellSupervisorDetails.DataSource = list;
                gvCellSupervisorDetails.DataBind();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellSupervisors();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isDBHited = false;
                for (int i = 0; i < gvCellSupervisorDetails.Rows.Count; i++)
                {
                    //if ((gvStudentDetails.Rows[i].FindControl("hfUpdated") as HiddenField).Value.Equals("1", StringComparison.OrdinalIgnoreCase))
                    //{
                    CellSupervisorScreen data = new CellSupervisorScreen();
                    data.CellID = (gvCellSupervisorDetails.Rows[i].FindControl("lblCellID") as Label).Text;
                    data.Shift = (gvCellSupervisorDetails.Rows[i].FindControl("lblShift") as Label).Text;
                    data.Supervisor = (gvCellSupervisorDetails.Rows[i].FindControl("ddlSupervisors") as DropDownList).SelectedValue;
                    if (data.Supervisor == "")
                    {
                        string successs = DBAccessPradeepMetals.DeleteCellSupervisor(data);
                    }
                    string success = DBAccessPradeepMetals.SaveCellSupervisors(data);
                    isDBHited = true;
                    //}

                }
                if (isDBHited)
                {
                    BindCellSupervisors();
                    HelperClass.openInsertSuccessModal(this);
                }
                else
                {
                    HelperClass.openWarningModal(this, "No record updated.");
                }
                
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindCellSupervisors();
        }

        protected void gvCellSupervisorDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlSupervisor = (DropDownList)e.Row.FindControl("ddlSupervisors");
                CellSupervisorScreen item = (CellSupervisorScreen)e.Row.DataItem;
                List<string> list= DBAccessPradeepMetals.GetSupervisors();
                list.Insert(0, "");
                ddlSupervisor.DataSource = list;
                ddlSupervisor.DataBind();
                ddlSupervisor.SelectedValue = item.Supervisor.ToString();
            }
        }
    }
}