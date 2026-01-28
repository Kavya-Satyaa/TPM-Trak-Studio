using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;
using Web_TPMTrakDashboard.GEA.Models;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class GEAAutoScheduleMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["AssemblyMachineList"] = null;
                BindMachineID();
                btnView_Click(null, null);
            }
        }
        private void BindMachineID()
        {
            try
            {
                List<string> list = new List<string>();
                if (ddlType.SelectedValue.Equals("StoresSetting", StringComparison.OrdinalIgnoreCase))
                {
                    list = GEADatabaseAccess.getMachineIDBasedOnAssembly("");
                }
                else if (ddlType.SelectedValue.Equals("AutoSchedule", StringComparison.OrdinalIgnoreCase))
                {
                    list = GEADatabaseAccess.getMachineIDBasedOnAssembly("'Stores','QualityInhouse'");
                }
                ddlMachineId.DataSource = list;
                ddlMachineId.DataBind();
                BindComponentId();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindComponentId()
        {
            try
            {
                List<ListItem> list = new List<ListItem>();
                if (ddlType.SelectedValue.Equals("AutoSchedule", StringComparison.OrdinalIgnoreCase))
                {
                    //list = GEADatabaseAccess.getAssemblyProcessComponentID();
                    list = GEADatabaseAccess.GetComponentIDWithDescByMachine(ddlMachineId.SelectedValue);
                }
                else if (ddlType.SelectedValue.Equals("StoresSetting", StringComparison.OrdinalIgnoreCase))
                {
                    list = GEADatabaseAccess.GetComponentIDWithDescByMachine(ddlMachineId.SelectedValue);
                }
                lbCompID.DataSource = list;
                lbCompID.DataTextField = "Text";
                lbCompID.DataValueField = "Value";
                lbCompID.DataBind();
                Session["ComponentList"] = list;
                //if (ddlType.SelectedValue.Equals("StoresSetting", StringComparison.OrdinalIgnoreCase))
                //{
                //    int count = 1;
                //    foreach (ListItem item in lbCompID.Items)
                //    {
                //        if (count <= 20)
                //        {
                //            item.Selected = true;
                //        }
                //        else
                //        {
                //            break;
                //        }
                //        count++;
                //    }
                //}
                BindOperationNo();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void lnkCompSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ComponentList"] != null)
                {
                    List<ListItem> list = Session["ComponentList"] as List<ListItem>;
                    if (list.Count > 0)
                    {
                        list = list.Where(k => k.Text.ToLower().Contains(txtCompSearch.Text.ToLower())).ToList();
                        lbCompID.DataSource = list;
                        lbCompID.DataTextField = "Text";
                        lbCompID.DataValueField = "Value";
                        lbCompID.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindOperationNo()
        {
            try
            {
                List<string> list = GEADatabaseAccess.getOperationNoByMultiComp(ddlMachineId.SelectedValue, HelperClassGeneric.getListBoxValueWithCommaSeparator(lbCompID));
                if (list.Count > 0)
                {
                    list.Insert(0, "All");
                }
                ddlOpnNo.DataSource = list;
                ddlOpnNo.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void lbCompID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindOperationNo();
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "openModal", "openMultiselectPopup();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindAutoScheduleData()
        {
            try
            {
                gvAutoSchedule.Visible = true;
                gvStoresSetting.Visible = false;
                List<AutoScheduleMasterEntity> list = GEADatabaseAccess.getAutoScheduleMasterDetails(HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbCompID), ddlOpnNo.SelectedValue, ddlMachineId.SelectedValue);
                gvAutoSchedule.DataSource = list;
                gvAutoSchedule.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindAssemblyMachine(RadioButtonList radioButtonList, string value)
        {
            try
            {
                string process = GEADatabaseAccess.getProcessOfMachine(ddlMachineId.SelectedValue);
                List<string> list = new List<string>();
                if (process.Equals("QualityInhouse", StringComparison.OrdinalIgnoreCase))
                {
                    list = GEADatabaseAccess.getAssemblyMachines("'Balancing'");
                }
                else if (process.Equals("Stores", StringComparison.OrdinalIgnoreCase))
                {
                    list = GEADatabaseAccess.getAssemblyMachines("'Assembly'");

                }
                if (list.Count > 0)
                {
                    list.Add("None");
                }
                //if (Session["AssemblyMachineList"] == null)
                //{
                //    list = GEADatabaseAccess.getAssemblyMachines();
                //    if (list.Count > 0)
                //    {
                //        list.Add("None");
                //    }
                //    Session["AssemblyMachineList"] = list;
                //}
                //list = Session["AssemblyMachineList"] as List<string>;
                if (radioButtonList != null)
                {
                    radioButtonList.DataSource = list;
                    radioButtonList.DataBind();
                    if (value == "") value = "None";
                    if (value != "")
                    {
                        if (radioButtonList.Items.FindByValue(value) != null)
                        {
                            radioButtonList.SelectedValue = value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void gvAutoSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    RadioButtonList radioButtonList = e.Row.FindControl("rbAutoScheduleMachineID") as RadioButtonList;
                    string value = (e.Row.FindControl("hdnAutoScheduleMachineID") as HiddenField).Value;
                    BindAssemblyMachine(radioButtonList, value);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlType.SelectedValue.Equals("AutoSchedule", StringComparison.OrdinalIgnoreCase))
                {
                    int count = 0;
                    foreach (GridViewRow row in gvAutoSchedule.Rows)
                    {
                        if ((row.FindControl("hdnUpdate") as HiddenField).Value.Equals("update", StringComparison.OrdinalIgnoreCase))
                        {

                            AutoScheduleMasterEntity data = new AutoScheduleMasterEntity();
                            data.ComponentID = (row.FindControl("lblCompID") as Label).Text;
                            data.OperationNo = (row.FindControl("lblOpnNo") as Label).Text;
                            data.MachineID = (row.FindControl("rbAutoScheduleMachineID") as RadioButtonList).SelectedValue;
                            data.OldMachineID = (row.FindControl("hdnExistMachineID") as HiddenField).Value;
                            if (data.OldMachineID == "") data.OldMachineID = "None";
                            string result = GEADatabaseAccess.saveAutoScheduleMasterDetauls(data);
                            if (result == "Deleted" || result == "Updated" || result == "Inserted" || result == "Not Exists")
                            {
                                count++;
                            }
                            else
                            {
                                HelperClassGeneric.openWarningModal(this, result);
                                return;
                            }
                             (row.FindControl("hdnUpdate") as HiddenField).Value = "";
                        }
                    }
                    if (count > 0)
                    {
                        HelperClassGeneric.openInsertSuccessModal(this);
                        BindAutoScheduleData();
                    }
                }
                else if (ddlType.SelectedValue.Equals("StoresSetting", StringComparison.OrdinalIgnoreCase))
                {
                    int count = 0;
                    foreach (GridViewRow row in gvStoresSetting.Rows)
                    {
                        AutoScheduleMasterEntity data = new AutoScheduleMasterEntity();
                        data.ComponentID = (row.FindControl("lblCompID") as Label).Text;
                        data.OperationNo = (row.FindControl("lblOpnNo") as Label).Text;
                        data.MachineID = (row.FindControl("lblMachineID") as Label).Text;
                        data.ToBeSentToStores = (row.FindControl("chkToBeSentToStores") as CheckBox).Checked;
                        int result = GEADatabaseAccess.saveToBeSentToStoreMasterDetails(data);
                        if (result > 0)
                        {
                            count++;
                        }
                        else
                        {
                            HelperClassGeneric.openErrorModal(this, "Updation failed.");
                            return;
                        }
                    }
                    if (count > 0)
                    {
                        HelperClassGeneric.openInsertSuccessModal(this);
                        BindStoresData();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }


        protected void btnView_Click(object sender, EventArgs e)
        {
            if (ddlType.SelectedValue.Equals("AutoSchedule", StringComparison.OrdinalIgnoreCase))
            {
                BindAutoScheduleData();
            }
            else if (ddlType.SelectedValue.Equals("StoresSetting", StringComparison.OrdinalIgnoreCase))
            {
                BindStoresData();
            }

        }

        #region ----- Stores Setting --------
        private void BindStoresData()
        {
            try
            {
                gvStoresSetting.Visible = true;
                gvAutoSchedule.Visible = false;
                List<AutoScheduleMasterEntity> list = GEADatabaseAccess.getToBeSentToStoreMasterDetails(HelperClassGeneric.getListBoxValueWithCommaSeparator(lbCompID), ddlOpnNo.SelectedValue, ddlMachineId.SelectedValue);
                gvStoresSetting.DataSource = list;
                gvStoresSetting.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        #endregion

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCompSearch.Text = "";
            BindMachineID();
        }

        protected void ddlMachineId_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCompSearch.Text = "";
            BindComponentId();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                HelperClassGeneric.clearListBoxValue(lbCompID);
                btnView_Click(null, null);
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}