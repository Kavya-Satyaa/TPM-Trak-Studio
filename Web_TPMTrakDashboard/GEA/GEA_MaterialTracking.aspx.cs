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
    public partial class GEA_MaterialTracking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["EmployeeList"] = null;
                string endDate = "";
                string startDate = GEADatabaseAccess.getWeekStartAndEndDate(DateTime.Now.ToString("dd-MM-yyyy"), out endDate);
                txtFromDate.Text = Util.GetDateTime(startDate).ToString("dd-MM-yyyy");
                txtToDate.Text = Util.GetDateTime(endDate).ToString("dd-MM-yyyy");
                //BindMachines();
                BindGrid();
            }
        }
        private void BindMachines()
        {
            try
            {

                List<string> Machines = Web_TPMTrakDashboard.Models.DataBaseAccess.GetMachineIDForPlant("");
                //ddlMachineID.DataSource = Machines;
                //ddlMachineID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindEmployee(DropDownList ddl, string value)
        {
            try
            {
                List<ListItem> Machines = new List<ListItem>();
                if (Session["EmployeeList"] == null)
                {
                    Session["EmployeeList"] = Machines = GEADatabaseAccess.GetAllStoresEmployeesID();
                }
                Machines = Session["EmployeeList"] as List<ListItem>;
                ddl.DataSource = Machines;
                ddl.DataTextField = "Text";
                ddl.DataValueField = "Value";
                ddl.DataBind();
                ddl.Items.Insert(0, "");
                if (value != "")
                {
                    HelperClassGeneric.setDropdownValue(ddl, value);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindGrid()
        {
            try
            {
                List<MaterialTracking_GEA> list = GEADatabaseAccess.GetMaterialTracking_GEA(txtFromDate.Text, txtToDate.Text, ddlStatus.SelectedValue);
                lvMaterial.DataSource = list;
                lvMaterial.DataBind();
                Session["TrackingData"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                for (int i = 0; i < lvMaterial.Items.Count; i++)
                {
                    var row = lvMaterial.Items[i];
                    if ((row.FindControl("chkReceiptCompletion") as CheckBox).Visible)
                    {
                        if ((row.FindControl("chkReceiptCompletion") as CheckBox).Checked)
                        {
                            MaterialTracking_GEA data = new MaterialTracking_GEA();
                            data.DateOfSchedule = (row.FindControl("lblDateOfSchedule") as Label).Text;
                            data.WhoHasReceived = (row.FindControl("ddlWhoReceived") as DropDownList).SelectedValue;

                            if (data.WhoHasReceived == "" || data.DateOfSchedule == "")
                            {
                                continue;
                            }
                            data.MachineID = (row.FindControl("lblMachineID") as Label).Text;

                            data.ProductionOrderNo = (row.FindControl("lblProductionOrderNo") as Label).Text;
                            data.seriesNo = (row.FindControl("lblSeriesNo") as Label).Text;
                            data.CycleTime = (row.FindControl("hdnCycleTime") as HiddenField).Value;
                            data.CompInterfaceID = (row.FindControl("hdnCompInterfaceID") as HiddenField).Value;
                            data.CompID = (row.FindControl("lblComponentID") as Label).Text;
                            data.CompDesc = (row.FindControl("lblCompDesc") as Label).Text;
                            result += GEADatabaseAccess.SaveMaterialTracking(data);
                        }
                    }
                }
                if (result > 0)
                {
                    HelperClassGeneric.openInsertSuccessModal(this);
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void lvMaterial_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    DropDownList ddl = (e.Item.FindControl("ddlWhoReceived") as DropDownList);
                    Label label = (e.Item.FindControl("lblWhoReceived") as Label);
                    CheckBox checkBox = (e.Item.FindControl("chkReceiptCompletion") as CheckBox);
                    Label labelReceiptCompletion = (e.Item.FindControl("lblReceiptCompletion") as Label);
                    if ((e.Item.FindControl("hdnWhoReceived") as HiddenField).Value != "")
                    {
                        ddl.Visible = false;
                        checkBox.Visible = false;
                    }
                    else
                    {
                        label.Visible = false;
                        ddl.Enabled = false;
                        labelReceiptCompletion.Visible = false;
                        string value = (e.Item.FindControl("hdnWhoReceived") as HiddenField).Value;
                        BindEmployee(ddl, value);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void chkReceiptCompletion_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                var item = (sender as CheckBox).NamingContainer as ListViewItem;
                if ((item.FindControl("chkReceiptCompletion") as CheckBox).Checked)
                {
                    (item.FindControl("ddlWhoReceived") as DropDownList).Enabled = true;
                }
                else
                {
                    (item.FindControl("ddlWhoReceived") as DropDownList).Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                List<MaterialTracking_GEA> list = Session["TrackingData"] as List<MaterialTracking_GEA>;
                if (list.Count > 0)
                {

                    bool successfull = GEAGenerateReport.GenerateMaterialTrackingData(txtFromDate.Text, txtToDate.Text, list);
                    if (successfull)
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ExportSuccess", "alert('Export Successful.')", true);
                    else
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ExportError", "alert('Error. Export Unsuccessful.')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('No data to export.')", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}