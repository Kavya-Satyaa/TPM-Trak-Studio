using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.ShantiIron.Model;

namespace Web_TPMTrakDashboard.ShantiIron
{
    public partial class CoolantTopUpMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                btnCancel.Visible = false;
                btnInsert.Visible = false;
                BindPlantData();
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                btnView_Click(null, null);
            }
        }
        private void BindPlantData()
        {
            try
            {
                List<string> list= DataBaseAccess.GetAllPlants();
                list.RemoveAt(0);
                ddlPlantId.DataSource = list;
                ddlPlantId.DataBind();
                BindCellData();
            }
            catch(Exception ex)
            {
            }
        }
        private void BindCellData()
        {
            try
            {
                ddlCellID.DataSource = ShantiDataBaseAccess.GetAllGroupIDs(ddlPlantId.SelectedValue == "All" ? "" : ddlPlantId.SelectedValue);
                ddlCellID.DataBind();
                ddlCellID.Items.Insert(0, new ListItem("All"));
                BindMachineData();
            }
            catch (Exception ex)
            {
            }
        }
        private void BindMachineData()
        {
            try
            {
                ddlMachineID.DataSource = VDGDataBaseAccess.GetMachinesbyPlantCell(ddlPlantId.SelectedValue == "All" ? "" : ddlPlantId.SelectedValue, ddlCellID.SelectedValue == "All" ? "" : ddlCellID.SelectedValue);
                ddlMachineID.DataBind();
                ddlMachineID.Items.Insert(0, new ListItem("All"));
            }
            catch (Exception ex)
            {
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                BindData();
            }
            catch(Exception ex)
            {

            }
        }
        private void BindData()
        {
            try
            {
                string plant = ddlPlantId.SelectedValue == "All" ? "" : ddlPlantId.SelectedValue;
                string machine = ddlMachineID.SelectedValue == "All" ? "" : ddlMachineID.SelectedValue;
                string cell = ddlCellID.SelectedValue == "All" ? "" : ddlCellID.SelectedValue;

                string fromDate = txtFromDate.Text;
                string toDate = txtToDate.Text;
                fromDate= Web_TPMTrakDashboard.Models.VDGDataBaseAccess.GetLogicalDayStart(fromDate);
                toDate = Web_TPMTrakDashboard.Models.VDGDataBaseAccess.GetLogicalDayEnd(toDate);
                List<CoolantTopUpData> list = ShantiDataBaseAccess.getCoolantTopUpData(plant, cell, machine,fromDate,toDate);
                lvCoolantTopUp.DataSource = list;
                lvCoolantTopUp.DataBind();
                lvCoolantTopUp.InsertItem.Visible = false;
            }
            catch(Exception ex)
            {

            }
        }

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindCellData();
            }
            catch(Exception ex)
            { }
        }
        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindMachineData();
            }
            catch (Exception ex)
            { }
        }
        protected void ddlPlantIdNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlPlant = lvCoolantTopUp.InsertItem.FindControl("ddlPlantIdNew") as DropDownList;
                DropDownList ddl = lvCoolantTopUp.InsertItem.FindControl("ddlMachineNew") as DropDownList;
                ddl.DataSource = ShantiDataBaseAccess.GetAllMachinesForPlant(ddlPlant.SelectedValue == "All" ? "" : ddlPlant.SelectedValue);
                ddl.DataBind();
                ddl = lvCoolantTopUp.InsertItem.FindControl("ddlOperatorNew") as DropDownList;
                ddl.DataSource = ShantiDataBaseAccess.getOperatorForPlant(ddlPlant.SelectedValue == "All" ? "" : ddlPlant.SelectedValue);
                ddl.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;
                btnCancel.Visible = true;
                btnInsert.Visible = true;
                btnNew.Visible = false;
                lvCoolantTopUp.InsertItem.Visible = true;
            }
            catch(Exception ex)
            {

            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = true;
                btnCancel.Visible = false;
                btnInsert.Visible = false;
                btnNew.Visible = true;
                lvCoolantTopUp.InsertItem.Visible = false;
            }
            catch (Exception ex)
            { }
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                btnCancel.Visible = false;
                btnInsert.Visible = false;
                btnNew.Visible = true;
                CoolantTopUpData data = new CoolantTopUpData();
                // data.PlantID = (lvCoolantTopUp.InsertItem.FindControl("ddlPlantIdNew") as DropDownList).SelectedValue;
                data.PlantID =ddlPlantId.SelectedValue;
                data.CellID = (lvCoolantTopUp.InsertItem.FindControl("ddlCellNew") as DropDownList).SelectedValue;
                data.MachineID = (lvCoolantTopUp.InsertItem.FindControl("ddlMachineNew") as DropDownList).SelectedValue;
                data.Operator = (lvCoolantTopUp.InsertItem.FindControl("ddlOperatorNew") as DropDownList).SelectedValue;
                data.TopUpValue = (lvCoolantTopUp.InsertItem.FindControl("txtTopUpNew") as TextBox).Text;
                data.TopUpDatetime = (lvCoolantTopUp.InsertItem.FindControl("txtTopUpDateTimeNew") as TextBox).Text;
                data.Remarks = (lvCoolantTopUp.InsertItem.FindControl("txtremarksNew") as TextBox).Text;
                if(data.PlantID=="")
                {
                    btnNew_Click(null,null);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('Please select Plant ID.')", true);
                    return;
                }
                if (data.CellID == "")
                {
                    btnNew_Click(null, null);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('Please select Cell ID.')", true);
                    return;
                }
                if (data.MachineID == "")
                {
                    btnNew_Click(null, null);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('Please select Machine ID.')", true);
                    return;
                }
                if (data.Operator == "")
                {
                    btnNew_Click(null, null);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('Please select Operator.')", true);
                    return;
                }
                if (data.TopUpValue == "")
                {
                    btnNew_Click(null, null);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('Please enter Top-Up.')", true);
                    return;
                }
                if (data.TopUpDatetime == "")
                {
                    btnNew_Click(null, null);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('Please select Top-Up Datetime.')", true);
                    return;
                }
                int result= ShantiDataBaseAccess.insertUpdateCoolantTopUpData(data);
                BindData();
                btnCancel_Click(null, null);
            }
            catch (Exception ex)
            {

            }
        }

      

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < lvCoolantTopUp.Items.Count; i++)
                {
                    CoolantTopUpData data = new CoolantTopUpData();
                    //data.PlantID = (lvCoolantTopUp.Items[i].FindControl("lblPlantId") as Label).Text;
                    data.PlantID = (lvCoolantTopUp.Items[i].FindControl("hdnPlant") as HiddenField).Value;
                    data.CellID = (lvCoolantTopUp.Items[i].FindControl("lblCellID") as Label).Text;
                    data.MachineID = (lvCoolantTopUp.Items[i].FindControl("lblMachineID") as Label).Text;
                    data.Operator = (lvCoolantTopUp.Items[i].FindControl("lblOperator") as Label).Text;
                    data.TopUpValue = (lvCoolantTopUp.Items[i].FindControl("lblTopUpValue") as Label).Text;
                    data.TopUpDatetime = (lvCoolantTopUp.Items[i].FindControl("lblTopUpDatetime") as Label).Text;
                    data.Remarks = (lvCoolantTopUp.Items[i].FindControl("txtRemarks") as TextBox).Text;
                    if (data.PlantID == "")
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('Please select Plant ID.')", true);
                        return;
                    }
                    if (data.CellID == "")
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('Please select Cell ID.')", true);
                        return;
                    }
                    if (data.MachineID == "")
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('Please select Machine ID.')", true);
                        return;
                    }
                    if (data.Operator == "")
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('Please select Operator.')", true);
                        return;
                    }
                    if (data.TopUpValue == "")
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('Please enter Top-Up.')", true);
                        return;
                    }
                    if (data.TopUpDatetime == "")
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('Please select Top-Up Datetime.')", true);
                        return;
                    }
                    int result = ShantiDataBaseAccess.insertUpdateCoolantTopUpData(data);
                    if(result<=0)
                    {
                        return;
                    }
                }
                BindData();
            }
            catch (Exception ex)
            {

            }
        }

        protected void lvCoolantTopUp_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if ((e.Item != null) && (e.Item.ItemType == ListViewItemType.InsertItem))
                {
                    //DropDownList ddlPlant = e.Item.FindControl("ddlPlantIdNew") as DropDownList;
                    //List<string> plantList = DataBaseAccess.GetAllPlants();
                    //plantList.RemoveAt(0);
                    //ddlPlant.DataSource = plantList;
                    //ddlPlant.DataBind();
                    DropDownList ddl = e.Item.FindControl("ddlCellNew") as DropDownList;
                    ddl.DataSource = ShantiDataBaseAccess.GetAllGroupIDs(ddlPlantId.SelectedValue == "All" ? "" : ddlPlantId.SelectedValue);
                    ddl.DataBind();
                    string cellId = ddl.SelectedValue;

                    ddl = e.Item.FindControl("ddlMachineNew") as DropDownList;
                    ddl.DataSource = VDGDataBaseAccess.GetMachinesbyPlantCell(ddlPlantId.SelectedValue == "All" ? "" : ddlPlantId.SelectedValue, cellId=="All"?"":cellId);
                    ddl.DataBind();
                    ddl = lvCoolantTopUp.InsertItem.FindControl("ddlOperatorNew") as DropDownList;
                    ddl.DataSource = ShantiDataBaseAccess.getOperatorForPlant(ddlPlantId.SelectedValue == "All" ? "" : ddlPlantId.SelectedValue);
                    ddl.DataBind();
                    (e.Item.FindControl("txtTopUpDateTimeNew") as TextBox).Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void lvCoolantTopUp_ItemInserting(object sender, ListViewInsertEventArgs e)
        {

        }

        protected void ddlCellNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                DropDownList ddl = lvCoolantTopUp.InsertItem.FindControl("ddlCellNew") as DropDownList;
               // ddl.DataSource = ShantiDataBaseAccess.GetAllGroupIDs(ddlPlantId.SelectedValue == "All" ? "" : ddlPlantId.SelectedValue);
                //ddl.DataBind();
                string cellId = ddl.SelectedValue;
                ddl = lvCoolantTopUp.InsertItem.FindControl("ddlMachineNew") as DropDownList;
                ddl.DataSource = VDGDataBaseAccess.GetMachinesbyPlantCell(ddlPlantId.SelectedValue == "All" ? "" : ddlPlantId.SelectedValue, cellId == "All" ? "" : cellId);
                ddl.DataBind();
                //ddl = lvCoolantTopUp.InsertItem.FindControl("ddlOperatorNew") as DropDownList;
                //ddl.DataSource = ShantiDataBaseAccess.getOperatorForPlant(ddlPlantId.SelectedValue == "All" ? "" : ddlPlantId.SelectedValue);
                //ddl.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlPlantId.SelectedValue == "")
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('Please select Plant ID.')", true);
                    return;
                }
                if (ddlCellID.SelectedValue == "")
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('Please select Cell ID.')", true);
                    return;
                }
                if (ddlMachineID.SelectedValue == "")
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('Please select Machine ID.')", true);
                    return;
                }
                if (txtFromDate.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('Please select From Date.')", true);
                    return;
                }
                if (txtToDate.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('Please select To Date.')", true);
                    return;
                }
                ShantiReports.coolantTopUpReport(ddlPlantId.SelectedValue=="All"?"": ddlPlantId.SelectedValue,ddlCellID.SelectedValue=="All"?"": ddlCellID.SelectedValue, ddlMachineID.SelectedValue=="All"?"":ddlMachineID.SelectedValue,txtFromDate.Text,txtToDate.Text);
            }
            catch(Exception ex)
            {

            }
        }
    }
}