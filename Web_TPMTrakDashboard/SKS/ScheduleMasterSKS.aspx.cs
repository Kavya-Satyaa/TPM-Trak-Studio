using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.SKS.Model;

namespace Web_TPMTrakDashboard.SKS
{
    public partial class ScheduleMasterSKS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachineID();
                BindData();
            }
        }
        private void BindMachineID()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllMachines("");
                ddlMachineId.DataSource = list;
                ddlMachineId.DataBind();
                BindPartID();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindPartID()
        {
            try
            {
                List<string> list = DataBaseAccess.GetComponentIDByMachine(ddlMachineId.SelectedValue);
                if (list.Count > 0)
                {
                    list.Insert(0, "All");
                }
                ddlPartID.DataSource = list;
                ddlPartID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindData()
        {
            try
            {
                string status = "";
                foreach (ListItem item in lbStatus.Items)
                {
                    if (item.Selected)
                    {
                        if (status == "")
                        {
                            status += "'" + item.Value + "'";
                        }
                        else
                        {
                            status += ",'" + item.Value + "'";
                        }
                    }
                }
                List<ScheduleEntity> list = SKSDatabaseAccess.getScheduleMasterDetails(ddlMachineId.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlMachineId.SelectedValue, ddlPartID.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPartID.SelectedValue, status, txtWorkOrder.Text);
                gvScheduleData.DataSource = list;
                gvScheduleData.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void ddlMachineId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPartID();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }
        protected void btnChangePriority_Click(object sender, EventArgs e)
        {
            try
            {
                List<ScheduleEntity> list = new List<ScheduleEntity>();
                foreach (GridViewRow row in gvScheduleData.Rows)
                {
                    if ((row.FindControl("chkSelect") as CheckBox).Checked)
                    {
                        ScheduleEntity data = new ScheduleEntity();
                        data.UserPriority = (row.FindControl("lblUserPriority") as Label).Text;
                        data.SchedulePriority = (row.FindControl("lblSchedulePriority") as Label).Text;
                        data.MachineID = (row.FindControl("lblMachineID") as Label).Text;
                        data.WorkOrder = (row.FindControl("lblWorkOrder") as Label).Text;
                        data.PartID = (row.FindControl("lblPartID") as Label).Text;
                        data.PartDesc = (row.FindControl("lblPartDesc") as Label).Text;
                        data.OperationNo = (row.FindControl("lblOperationNo") as Label).Text;
                        data.Status = (row.FindControl("lblStatus") as Label).Text;
                        list.Add(data);
                    }
                }
                gvChangePriority.DataSource = list;
                gvChangePriority.DataBind();
                radioMoveToEnd.Checked = true;
                radioMoveBefore.Checked = false;
                ddlPriorities.DataSource = SKSDatabaseAccess.getAllSchedulePrioritiesKeyValue(ddlMachineId.SelectedValue);
                ddlPriorities.DataBind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openChangePriorityModal", "openChangePriorityModal();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void btnSavePriority_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvChangePriority.Rows)
                {
                    ScheduleEntity data = new ScheduleEntity();
                    data.UserPriority = (row.FindControl("lblUserPriority") as Label).Text;
                    data.SchedulePriority = (row.FindControl("lblSchedulePriority") as Label).Text;
                    data.MachineID = (row.FindControl("lblMachineID") as Label).Text;
                    data.WorkOrder = (row.FindControl("lblWorkOrder") as Label).Text;
                    data.PartID = (row.FindControl("lblPartID") as Label).Text;
                    data.PartDesc = (row.FindControl("lblPartDesc") as Label).Text;
                    data.OperationNo = (row.FindControl("lblOperationNo") as Label).Text;
                    data.Status = (row.FindControl("lblStatus") as Label).Text;
                    if (radioMoveToEnd.Checked)
                    {
                        //int current_max_User_priority = 0, new_User_priority = 0;
                        //int current_priority = Convert.ToInt32(lblPrty.Text);
                        //int current_User_priority = Convert.ToInt32(UserlblPrty.Text);
                        //int current_max_priority = GEADatabaseAccess.GetMaxSchedulePriority(out current_max_User_priority);
                        //int new_priority = current_max_priority + 1;
                        //new_User_priority = current_max_User_priority + 1;
                        //IsChanged = GEADatabaseAccess.ChangeSchedulePriority(Idd, current_priority, new_priority, "MoveToEnd", current_User_priority, new_User_priority);
                    }
                    else if (radioMoveBefore.Checked)
                    {
                        //if (ddlPriorities.SelectedItem != null)
                        //{
                        //    int current_priority = Convert.ToInt32(lblPrty.Text);
                        //    int new_priority = Convert.ToInt32(ddlPriorities.SelectedValue);
                        //    int current_User_priority = Convert.ToInt32(UserlblPrty.Text);
                        //    int new_User_priority = Convert.ToInt32(ddlPriorities.SelectedItem.Text);
                        //    IsChanged = GEADatabaseAccess.ChangeSchedulePriority(Idd, current_priority, new_priority, "MoveBefore", current_User_priority, new_User_priority);
                        //}
                    }
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }


    }
}