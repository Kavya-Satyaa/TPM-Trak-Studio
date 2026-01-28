using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.KTASpindle
{
    public partial class MachineScheduleMaster : System.Web.UI.Page
    {
        private static int selectedIDD = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    Response.Redirect("~/SignIn.aspx", false);
                }
                if (!IsPostBack)
                {
                    lbStatus.Items[0].Selected = true;
                    lbStatus.Items[1].Selected = true;
                    txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    //txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                    BindPlantID();
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindPlantID()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlants();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();
                BindCellID();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindCellID()
        {
            try
            {
                List<string> list = DBAccess.getCellAssignedForEmployee(ddlPlant.SelectedValue, Session["UserName"].ToString());
                if (list.Count > 1)
                {
                    list.Insert(0, "All");
                }
                ddlCell.DataSource = list;
                ddlCell.DataBind();
                BindMachineID();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindMachineID()
        {
            try
            {
                string cell = "";
                if (ddlCell.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (ListItem item in ddlCell.Items)
                    {
                        if (cell == "")
                        {
                            cell += "'" + item.Value + "'";
                        }
                        else
                        {
                            cell += ",'" + item.Value + "'";
                        }
                    }
                }
                else
                {
                    cell = "'" + ddlCell.SelectedValue + "'";
                }
                List<ListItem> list = DBAccess.getMachineIdInterfaceIDByPlantCell(ddlPlant.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue, cell, false);
                if (list.Count > 0)
                {
                    list.Insert(0, new ListItem() { Text = "All", Value = "All" });
                }
                ddlMachine.DataSource = list;
                ddlMachine.DataTextField = "Text";
                ddlMachine.DataValueField = "Value";
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellID();
        }

        protected void ddlCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineID();
        }
        private void BindPlantIDFooter()
        {
            try
            {
                DropDownList ddl = gvScheduleCreate.FooterRow.FindControl("ddlPlantNew") as DropDownList;
                List<string> list = DataBaseAccess.GetAllPlants();
                if (list.Count > 0)
                {
                    list.Remove("All");
                }
                ddl.DataSource = list;
                ddl.DataBind();
                ddl.SelectedValue = string.IsNullOrEmpty(ddlPlant.SelectedValue) ? list[0] : ddlPlant.SelectedValue;
                BindCellIDFooter();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindCellIDFooter()
        {
            try
            {
                string plant = (gvScheduleCreate.FooterRow.FindControl("ddlPlantNew") as DropDownList).SelectedValue;
                DropDownList ddl = gvScheduleCreate.FooterRow.FindControl("ddlCellNew") as DropDownList;
                List<string> list = DBAccess.getCellAssignedForEmployee(plant, Session["UserName"].ToString());
                ddl.DataSource = list;
                ddl.DataBind();
                ddl.SelectedValue = string.IsNullOrEmpty(ddlCell.SelectedValue) ? list[0] : ddlCell.SelectedValue;
                BindMachineIDFooter();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindMachineIDFooter()
        {
            try
            {
                string plant = (gvScheduleCreate.FooterRow.FindControl("ddlPlantNew") as DropDownList).SelectedValue;
                string cell = "'" + (gvScheduleCreate.FooterRow.FindControl("ddlCellNew") as DropDownList).SelectedValue + "'";
                DropDownList ddl = (gvScheduleCreate.FooterRow.FindControl("ddlMachineNew") as DropDownList);
                List<ListItem> list = DBAccess.getMachineIdInterfaceIDByPlantCell(plant, cell, true);
                ddl.DataSource = list;
                ddl.DataTextField = "Text";
                ddl.DataValueField = "Value";
                ddl.DataBind();
                ddl.SelectedItem.Text = string.IsNullOrEmpty(ddlMachine.SelectedValue) ? list[0].Value : ddlMachine.SelectedValue;
                BindComponentIDFooter();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindComponentIDFooter()
        {
            try
            {
                string machine = (gvScheduleCreate.FooterRow.FindControl("ddlMachineNew") as DropDownList).SelectedItem.Text;
                //DropDownList ddl = (gvScheduleCreate.FooterRow.FindControl("ddlComponentNew") as DropDownList);
                HtmlGenericControl ddl = (gvScheduleCreate.FooterRow.FindControl("dlComponentNew") as HtmlGenericControl);
                List<ListItem> list = DBAccess.getComponentIDWithInterface(machine);
                //ddl.DataSource = list;
                //ddl.DataTextField = "Text";
                //ddl.DataValueField = "Value";
                //ddl.DataBind();
                var builder = new System.Text.StringBuilder();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {

                        string str = "<option style='font-weight:unset'>" + list[i].Text.ToString() + "</option><input type='hidden' class='compHdn' value='" + list[i].Value.ToString() + "'/>";
                        builder.Append(str);

                    }
                }
                ddl.InnerHtml = builder.ToString();
                BindOperationFooter();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindOperationFooter()
        {
            try
            {
                string machine = (gvScheduleCreate.FooterRow.FindControl("ddlMachineNew") as DropDownList).SelectedItem.Text;
                string compID = (gvScheduleCreate.FooterRow.FindControl("txtComponentNew") as TextBox).Text;
                DropDownList ddl = (gvScheduleCreate.FooterRow.FindControl("ddlOperationNew") as DropDownList);
                List<ListItem> list = DBAccess.getOperationWithInterface(machine, compID);
                ddl.DataSource = list;
                ddl.DataTextField = "Text";
                ddl.DataValueField = "Value";
                ddl.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void ddlPlantNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellIDFooter();
        }

        protected void ddlCellNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineIDFooter();
        }


        protected void ddlMachineNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindComponentIDFooter();
        }
        protected void ddlComponentNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOperationFooter();
        }
        protected void txtComponentNew_TextChanged(object sender, EventArgs e)
        {
            BindOperationFooter();
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                BindPlantIDFooter();
                BindCellIDFooter();
                BindMachineIDFooter();
                BindData();
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
                string cellID = "";
                if (ddlCell.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (ListItem item in ddlCell.Items)
                    {
                        if (cellID == "")
                            cellID += "'" + item.Value + "'";
                        else
                            cellID += ",'" + item.Value + "'";
                    }
                }
                else
                {
                    cellID= "'" + ddlCell.SelectedValue + "'";
                }
                string machineID = "";
                if (ddlMachine.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (ListItem item in ddlMachine.Items)
                    {
                        if (machineID == "")
                            machineID += "'" + item.Value + "'";
                        else
                            machineID += ",'" + item.Value + "'";
                    }
                }
                else
                {
                    machineID = "'" + ddlMachine.SelectedValue + "'";
                }
                string status = "";
                foreach (ListItem item in lbStatus.Items)
                {
                    if (item.Selected)
                    {
                        if (status == "")
                            status += "'" + item.Value + "'";
                        else
                            status += ",'" + item.Value + "'";
                    }
                }
                List<ScheduleMasterEntity> list = DBAccess.getScheduleMasterDetails(ddlPlant.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue, cellID, machineID, txtCompIDSearch1.Text, txtCompIDSearch2.Text, txtCompDescSearch1.Text, txtCompDescSearch2.Text, status, txtFromDate.Text, txtToDate.Text);
                int flag = 0;
                if (list.Count == 0)
                {
                    flag = 1;
                    list.Add(new ScheduleMasterEntity());
                }
                gvScheduleCreate.DataSource = list;
                gvScheduleCreate.DataBind();
                (gvScheduleCreate.FooterRow.FindControl("txtScheduleDate") as TextBox).Text = DateTime.Now.ToString("dd-MM-yyyy");
                Session["ScheduleDataList"] = list;
                if (flag == 1)
                {
                    gvScheduleCreate.Rows[0].Visible = false;
                }
                BindPlantIDFooter();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void gvScheduleCreate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton lnkDelete = e.Row.FindControl("lnkDelete") as LinkButton;
                    LinkButton lnkClose = e.Row.FindControl("lnkClose") as LinkButton;
                    string status = (e.Row.FindControl("lblStatus") as Label).Text;
                    bool deleteVisible = false, closeVisible = false;
                    if (Session["AdminData"].ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!status.Equals("Running", StringComparison.OrdinalIgnoreCase))
                        {
                            deleteVisible = true;
                        }
                        if (status.Equals("Running", StringComparison.OrdinalIgnoreCase))
                        {
                            closeVisible = true;
                        }
                    }
                    if (lnkDelete != null)
                    {
                        lnkDelete.Visible = deleteVisible;
                    }
                    if (lnkClose != null)
                    {
                        lnkClose.Visible = closeVisible;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lnkInsert_Click = " + ex.Message);
            }
        }


        protected void lnkInsert_Click(object sender, EventArgs e)
        {
            try
            {
                ScheduleMasterEntity data = new ScheduleMasterEntity();
                data.Plant = (gvScheduleCreate.FooterRow.FindControl("ddlPlantNew") as DropDownList).SelectedValue;
                data.Cell = (gvScheduleCreate.FooterRow.FindControl("ddlCellNew") as DropDownList).SelectedValue;
                data.MachineInterfaceID = (gvScheduleCreate.FooterRow.FindControl("ddlMachineNew") as DropDownList).SelectedValue;
                data.Machine = (gvScheduleCreate.FooterRow.FindControl("ddlMachineNew") as DropDownList).SelectedItem.Text;
                //data.CompInterfaceId = (gvScheduleCreate.FooterRow.FindControl("ddlComponentNew") as DropDownList).SelectedValue;
                //data.Component = (gvScheduleCreate.FooterRow.FindControl("ddlComponentNew") as DropDownList).SelectedItem.Text;
                data.CompInterfaceId = (gvScheduleCreate.FooterRow.FindControl("hdnCompNew") as HiddenField).Value;
                data.Component = (gvScheduleCreate.FooterRow.FindControl("txtComponentNew") as TextBox).Text;
                data.OpnInterfaceId = (gvScheduleCreate.FooterRow.FindControl("ddlOperationNew") as DropDownList).SelectedValue;
                data.Operation = (gvScheduleCreate.FooterRow.FindControl("ddlOperationNew") as DropDownList).SelectedItem.Text;
                data.ScheduleDate = (gvScheduleCreate.FooterRow.FindControl("txtScheduleDate") as TextBox).Text;
                data.Priorityno = (gvScheduleCreate.FooterRow.FindControl("txtPriority") as TextBox).Text;
                data.ScheduleDate = VDGDataBaseAccess.GetLogicalDayStart(data.ScheduleDate);
                data.UpdatedBy = Session["UserName"].ToString();
                data.Status = "2";

                List<ScheduleMasterEntity> list = Session["ScheduleDataList"] as List<ScheduleMasterEntity>;
                if (string.IsNullOrEmpty(data.Priorityno))
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Priority cannot be Empty!!");
                    return;
                }

                if (list.AsEnumerable().Where(x => x.Machine == data.Machine).Where(x => x.Priorityno != null && x.Priorityno.Equals(data.Priorityno)).Count() > 0)
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Priority No Already Exists!!");
                    return;
                }

                string result = DBAccess.saveScheduleMasterDetails(data);
                if (result == "Inserted Successfully")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Schedule Created Successfully.')", true);
                }
                else if (result == "Already Exists")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openWarningModal('This Schedule already exists.')", true);
                    return;
                }
                else if (result == "Running Schedule")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openWarningModal('This Schedule is Running.')", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openErrorModal('Insertion failed.')", true);
                    return;
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lnkInsert_Click = " + ex.Message);
            }
        }

        protected void lnkClose_Click(object sender, EventArgs e)
        {
            try
            {
                selectedIDD = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "confirmModal", "openConfirmModal('closeConfirmModal')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lnkClose_Click = " + ex.Message);
            }
        }



        protected void btnCloseConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                ScheduleMasterEntity data = new ScheduleMasterEntity();
                data.ScheduleDateTime = (gvScheduleCreate.Rows[selectedIDD].FindControl("hdnScheduleDateTime") as HiddenField).Value;
                data.Plant = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblPlant") as Label).Text;
                data.Cell = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblCell") as Label).Text;
                data.Machine = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblMachine") as Label).Text;
                data.Component = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblComponent") as Label).Text;
                data.Operation = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblOperation") as Label).Text;
                data.Status = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblStatus") as Label).Text;
                int result = DBAccess.updateScheduleMasterDetails(data);
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Closed Successfully.')", true);
                }
                selectedIDD = -1;
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnCloseConfirm_Click = " + ex.Message);
            }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            try
            {
                selectedIDD = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "confirmModal", "openConfirmModal('deleteConfirmModal')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lnkDelete_Click = " + ex.Message);
            }
        }
        protected void btnDeleteConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                ScheduleMasterEntity data = new ScheduleMasterEntity();
                data.ScheduleDateTime = (gvScheduleCreate.Rows[selectedIDD].FindControl("hdnScheduleDateTime") as HiddenField).Value;
                data.Plant = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblPlant") as Label).Text;
                data.Cell = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblCell") as Label).Text;
                data.Machine = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblMachine") as Label).Text;
                data.Component = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblComponent") as Label).Text;
                data.Operation = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblOperation") as Label).Text;
                data.Status = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblStatus") as Label).Text;
                int result = DBAccess.deleteScheduleMasterDetails(data);
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Deleted Successfully.')", true);
                }
                selectedIDD = -1;
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnDeleteConfirm_Click = " + ex.Message);
            }
        }

        protected void gvScheduleCreate_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvScheduleCreate.PageIndex = e.NewPageIndex;
                if (Session["ScheduleDataList"] != null)                                
                {
                    gvScheduleCreate.DataSource = Session["ScheduleDataList"] as List<ScheduleMasterEntity>;
                    gvScheduleCreate.DataBind();
                    (gvScheduleCreate.FooterRow.FindControl("txtScheduleDate") as TextBox).Text = DateTime.Now.ToString("dd-MM-yyyy");
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void gvScheduleCreate_PreRender(object sender, EventArgs e)
        {
            try
            {
                GridView grid = (GridView)sender;
                if (grid != null)
                {
                    GridViewRow pagerRow = (GridViewRow)grid.BottomPagerRow;
                    if (pagerRow != null)
                    {
                        pagerRow.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ScheduleDataList"] == null)
                {
                    return;
                }
                string status = "";
                int i = 0;
                foreach (ListItem item in lbStatus.Items)
                {
                    if (item.Selected)
                    {
                        if (status == "")
                        {
                            status += item.Value;
                        }
                        else
                        {
                            status += "," + item.Value;
                        }
                    }
                }
                List<ScheduleMasterEntity> list = Session["ScheduleDataList"] as List<ScheduleMasterEntity>;
                KTAGenerateReport.generateScheduleMasterReport(ddlPlant.SelectedValue, ddlCell.SelectedValue, ddlMachine.SelectedValue, status, txtCompIDSearch1.Text + "*" + txtCompIDSearch2.Text + "*", txtCompDescSearch1.Text + "*" + txtCompDescSearch2.Text + "*",txtFromDate.Text,txtToDate.Text, list);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnUpdateStore_Click(object sender, EventArgs e)
        {
            ScheduleMasterEntity data = new ScheduleMasterEntity();
            bool IsUpdated = false;
            try
            {
                List<ScheduleMasterEntity> updatedPriorityList = new List<ScheduleMasterEntity>();
                foreach (GridViewRow row in gvScheduleCreate.Rows)
                {
                    if ((row.FindControl("lblStatus") as Label).Text.Equals("New", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty((row.FindControl("txtPriority") as TextBox).Text))
                    {
                        updatedPriorityList.Add(new ScheduleMasterEntity()
                        {
                            Machine = (row.FindControl("lblMachine") as Label).Text,
                            Priorityno = (row.FindControl("txtPriority") as TextBox).Text
                        });
                    }
                }
                foreach (GridViewRow row in gvScheduleCreate.Rows)
                {
                    if((row.FindControl("hdnUpdate") as HiddenField).Value.ToString().Equals("update", StringComparison.OrdinalIgnoreCase))
                    {
                        data.Plant = (row.FindControl("lblPlant") as Label).Text;
                        data.Cell = (row.FindControl("lblCell") as Label).Text;
                        data.MachineInterfaceID = (row.FindControl("lblMachine") as Label).Text;
                        data.Machine = (row.FindControl("lblMachine") as Label).Text;
                        data.Component = (row.FindControl("lblComponent") as Label).Text;
                        data.OpnInterfaceId = (row.FindControl("lblOperation") as Label).Text;
                        data.Operation = (row.FindControl("lblOperation") as Label).Text;
                        data.ScheduleDate = (row.FindControl("lblDate") as Label).Text;
                        data.Priorityno = (row.FindControl("txtPriority") as TextBox).Text;

                        List<ScheduleMasterEntity> list = Session["ScheduleDataList"] as List<ScheduleMasterEntity>;
                        if (string.IsNullOrEmpty(data.Priorityno))
                        {
                            HelperClassGeneric.openWarningToastrModal(this, "Priority cannot be Empty!!");
                            return;
                        }

                        if (list.AsEnumerable().Where(x => x.Machine == data.Machine).Where(x => x.Priorityno != null && x.Priorityno.Equals(data.Priorityno)).Count() > 0 && updatedPriorityList.AsEnumerable().Where(x => x.Machine == data.Machine).Where(x => x.Priorityno.ToString().Equals(data.Priorityno)).Count() > 1)
                        {
                            HelperClassGeneric.openWarningToastrModal(this, "Priority No Already Exists!!");
                            (row.FindControl("txtPriority") as TextBox).Text = "";
                            return;
                        }
                        IsUpdated = DBAccess.UpdateStoreSchedulePriorityKTA(data); 
                    }
                }
                if (IsUpdated)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Updated Successfully.')", true);
                }
                BindData();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}