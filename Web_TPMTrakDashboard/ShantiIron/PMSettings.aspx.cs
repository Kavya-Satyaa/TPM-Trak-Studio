using BusinessClassLibrary;
using OfficeOpenXml.ConditionalFormatting;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik.Andon_Advik.Model;
using Web_TPMTrakDashboard.AlertModule;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.ShantiIron
{
    public partial class PMSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!(IsPostBack))
            {
                //BindPlant();
                //BindCell();
                BindMachineID();
                LoadData();
            }
        }

        //private void BindPlant()
        //{
        //    try
        //    {
        //        List<string> PlantID = BindCockpitView.ViewPlantToDisplay();
        //        if (PlantID != null && PlantID.Count > 0)
        //        {
        //            ddlplantID.DataSource = PlantID;
        //            ddlplantID.DataBind();
        //            ddlplantID.SelectedIndex = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex);
        //    }
        //}

        //private void BindCell()
        //{
        //    try
        //    {
        //        List<string> CellID = BindCockpitView.ViewCellsToDisplay(ddlplantID.SelectedValue.ToString());
        //        if (CellID != null && CellID.Count > 0)
        //        {
        //            ddlcellID.DataSource = CellID;
        //            ddlcellID.DataBind();
        //            ddlcellID.SelectedIndex = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.ToString());
        //    }
        //}

        private void LoadData()
        {
            try
            {
                List<PMSettingsEntity> Entity = AlertModule.DataBaseAccess.GetPMRuleData(ddlMachineID.SelectedValue.ToString());
                if (Entity != null && Entity.Count > 0)
                {
                    grdPMChecklist.DataSource = Entity;
                    grdPMChecklist.DataBind();
                }
                else
                {
                    Entity.Add(new PMSettingsEntity { Rule = "" });
                    grdPMChecklist.DataSource = Entity;
                    grdPMChecklist.DataBind();
                    grdPMChecklist.Rows[0].Visible = false;
                }
                BindCategory();
                LoadItems();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void LoadItems()
        {
            try
            {
                List<PMSettingsItemEntity> Entity = AlertModule.DataBaseAccess.GetPMItemsData(ddlMachineID.SelectedValue.ToString(), ddlCategory.SelectedValue.ToString());
                if (Entity != null && Entity.Count > 0)
                {
                    gridViewItems.DataSource = Entity;
                    gridViewItems.DataBind();
                }
                else
                {
                    Entity.Add(new PMSettingsItemEntity { Rule = "", Items = "" });
                    gridViewItems.DataSource = Entity;
                    gridViewItems.DataBind();
                    gridViewItems.Rows[0].Visible = false;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void BindCategory()
        {
            try
            {
                List<string> Category = Web_TPMTrakDashboard.AlertModule.DataBaseAccess.getCategory(ddlMachineID.SelectedValue.ToString());
                Category.Insert(0, "All");
                ddlCategory.DataSource = Category;
                ddlCategory.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        //protected void ddlplantID_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindCell();
        //}

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlcellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineID();
        }

        private void BindMachineID()
        {
            List<string> MachineID = VDGDataBaseAccess.GetMachinesbyPlantCell("", ConfigurationManager.AppSettings["ShanthiCell"].ToString());
            ddlMachineID.DataSource = MachineID;
            ddlMachineID.DataBind();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            LoadItems();
        }

        protected void gridViewItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string MachineID = string.Empty;
                string Category = string.Empty;
                string Items = string.Empty;
                string OldItem = string.Empty;
                string ID = string.Empty;
                int RowIndex = 0;
                switch (e.CommandName)
                {
                    case "AddRow":
                        MachineID = ddlMachineID.SelectedValue.ToString();
                        Items = (gridViewItems.FooterRow.FindControl("footerItem") as TextBox).Text;
                        ID = "";
                        Category = (gridViewItems.FooterRow.FindControl("ddlFooterCategory") as DropDownList).SelectedValue.ToString();
                        OldItem = Items;
                        if (!(string.IsNullOrEmpty(Category) || string.IsNullOrEmpty(MachineID) && string.IsNullOrEmpty(Items)))
                        {
                            AlertModule.DataBaseAccess.UpdateDeleteItems(MachineID, Category, Items, OldItem, "Update", ID);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Category and Item Inserted Successfully')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Items and Category Cannot be Empty')", true);
                            return;
                        }
                        break;
                    case "UpdateRow":
                        RowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                        MachineID = ddlMachineID.SelectedValue.ToString();
                        Items = (gridViewItems.Rows[RowIndex].FindControl("txtItem") as TextBox).Text;
                        ID = (gridViewItems.Rows[RowIndex].FindControl("IDD") as HiddenField).Value;
                        OldItem = (gridViewItems.Rows[RowIndex].FindControl("hidItems") as HiddenField).Value;
                        Category = (gridViewItems.Rows[RowIndex].FindControl("lblCategory") as Label).Text;
                        if (!(string.IsNullOrEmpty(Category) || string.IsNullOrEmpty(MachineID) && string.IsNullOrEmpty(Items)))
                        {
                            Web_TPMTrakDashboard.AlertModule.DataBaseAccess.UpdateDeleteItems(MachineID, Category, Items, OldItem, "Update", ID);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Category and Item updated Successfully')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Items and Category Cannot be Empty')", true);
                            return;
                        }
                        break;
                    case "DeleteRow":
                        RowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                        MachineID = ddlMachineID.SelectedValue.ToString();
                        Items = (gridViewItems.Rows[RowIndex].FindControl("txtItem") as TextBox).Text;
                        ID = (gridViewItems.Rows[RowIndex].FindControl("IDD") as HiddenField).Value;
                        OldItem = (gridViewItems.Rows[RowIndex].FindControl("hidItems") as HiddenField).Value;
                        Category = (gridViewItems.Rows[RowIndex].FindControl("lblCategory") as Label).Text;
                        if (!(string.IsNullOrEmpty(Category) || string.IsNullOrEmpty(MachineID) && string.IsNullOrEmpty(Items)))
                        {
                            Web_TPMTrakDashboard.AlertModule.DataBaseAccess.UpdateDeleteItems(MachineID, Category, Items, OldItem, "Delete", ID);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Deleted Successfully')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Items and Category Cannot be Empty')", true);
                            return;
                        }
                        break;
                }
                LoadData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }

        }

        protected void grdPMChecklist_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string MachineID = string.Empty;
                string ID = string.Empty;
                string Category = string.Empty;
                string OldCategory = string.Empty;
                int RowIndex = 0;
                switch (e.CommandName)
                {
                    case "AddRow":

                        MachineID = ddlMachineID.SelectedValue.ToString();
                        Category = (grdPMChecklist.FooterRow.FindControl("txtFooterRule") as TextBox).Text;
                        OldCategory = Category;
                        ID = "";
                        if (!(string.IsNullOrEmpty(Category) || string.IsNullOrEmpty(MachineID)))
                        {
                            Web_TPMTrakDashboard.AlertModule.DataBaseAccess.UpdateDeleteCategory(MachineID, Category, OldCategory, "Update", ID);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Category Inserted Successfully')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Category Cannot be Empty')", true);
                            return;
                        }
                        break;
                    case "UpdateRow":
                        RowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                        MachineID = ddlMachineID.SelectedValue.ToString();
                        Category = (grdPMChecklist.Rows[RowIndex].FindControl("txtRule") as TextBox).Text;
                        ID = (grdPMChecklist.Rows[RowIndex].FindControl("IDD") as HiddenField).Value;
                        OldCategory = (grdPMChecklist.Rows[RowIndex].FindControl("hidRule") as HiddenField).Value;
                        if (!(string.IsNullOrEmpty(Category) || string.IsNullOrEmpty(MachineID)))
                        {
                            Web_TPMTrakDashboard.AlertModule.DataBaseAccess.UpdateCategory(MachineID, OldCategory, Category);
                            Web_TPMTrakDashboard.AlertModule.DataBaseAccess.UpdateDeleteCategory(MachineID, Category, OldCategory, "Update", ID);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Category Updated Successfully')", true);

                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Category Cannot be Empty')", true);
                            return;
                        }

                        break;
                    case "DeleteRow":
                        RowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                        MachineID = ddlMachineID.SelectedValue.ToString();
                        Category = (grdPMChecklist.Rows[RowIndex].FindControl("txtRule") as TextBox).Text;
                        OldCategory = (grdPMChecklist.Rows[RowIndex].FindControl("hidRule") as HiddenField).Value;
                        ID = (grdPMChecklist.Rows[RowIndex].FindControl("IDD") as HiddenField).Value;
                        if (!(string.IsNullOrEmpty(Category) || string.IsNullOrEmpty(MachineID)))
                        {
                            if (!Web_TPMTrakDashboard.AlertModule.DataBaseAccess.CheckCategoryPresent(Category, MachineID))
                            {
                                Web_TPMTrakDashboard.AlertModule.DataBaseAccess.UpdateDeleteCategory(MachineID, Category, OldCategory, "Delete", ID);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Category Deleted Successfully')", true);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Category Has Items Please delete Items before Category!!')", true);
                                return;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Category Cannot be Empty')", true);
                            return;
                        }
                        break;
                }
                LoadData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void gridViewItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList drp = e.Row.FindControl("ddlFooterCategory") as DropDownList;
                drp.DataSource = Web_TPMTrakDashboard.AlertModule.DataBaseAccess.getCategory(ddlMachineID.SelectedValue.ToString());
                drp.DataBind();
            }
        }
    }

    public class PMSettingsEntity
    {
        public string Rule { get; set; }
        public string ID { get; set; }
    }

    public class PMSettingsItemEntity
    {
        public string Rule { get; set; }
        public string Items { get; set; }
        public string ID { get; set; }
    }
}