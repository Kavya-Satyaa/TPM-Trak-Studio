using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik.DataBaseAccess;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.ShantiIron.Model;

namespace Web_TPMTrakDashboard.ShantiIron
{
    public partial class DownTimeQualification : System.Web.UI.Page
    {
        List<string> downInterfaceID = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindAllPlants();
                BindDayShift();
                setlogicaldates();
                if (ddlCell.Items.FindByValue("Phantom") != null)
                {
                    ddlCell.SelectedValue = "Phantom";
                    bindAllMachines();
                }
                btnview_Click(null, null);
            }
        }
        private void BindDayShift()
        {
            try
            {
                List<string> lstPlantData = CockpitDataBaseAccess.GetAllPredefinedShifts();
                ddlDayShift.DataSource = lstPlantData;
                ddlDayShift.DataBind();
                //ddlDayShift.SelectedIndex = 3;
            }
            catch (Exception ex)
            {
                //lblMessages.Text = ex.Message;
            }
        }
        private void setlogicaldates()
        {
            string endtime;
            string starttime = ShantiDataBaseAccess.GetShiftstart(out endtime);
            txtFromDate.Text = Convert.ToDateTime(starttime).ToString("dd-MMM-yyyy HH:mm");
            txtToDate.Text = Convert.ToDateTime(endtime).ToString("dd-MMM-yyyy HH:mm");
        }
        private void bindAllPlants()
        {
            try
            {
                ddlplant.DataSource = BindCockpitView.ViewPlantToDisplay();
                ddlplant.DataBind();
                ddlplant.Items.Insert(0, new ListItem("All", "All"));
                BindCellIDs();
            }
            catch (Exception ex)
            {
            }

        }
        private void BindCellIDs()
        {
            try
            {
                string plant = ddlplant.SelectedValue == null || ddlplant.SelectedValue=="All" ? "" : ddlplant.SelectedItem.ToString();
                List<string> GetCell = BindCockpitView.ViewCellsToDisplay(plant);
                ddlCell.DataSource = GetCell;
                ddlCell.DataBind();
                ddlCell.Items.Insert(0, new ListItem("All", "All"));
                bindAllMachines();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void bindAllMachines()
        {
            try
            {
                string plant = ddlplant.SelectedValue == null ? "" : ddlplant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlplant.SelectedItem.ToString();
                string Cell = ddlCell.SelectedValue == null ? "" : ddlCell.SelectedItem.ToString();
                ddlmachine.DataSource = CockpitDataBaseAccess.GetMachinesForCell(Cell, plant); 
                ddlmachine.DataBind();
                ddlmachine.Items.Insert(0, new ListItem("All", "All"));
            }
            catch (Exception ex)
            {
            }

        }

        protected void ddlplant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellIDs();
        }
        protected void ddlCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindAllMachines();
        }

        protected void btnview_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlplant.SelectedValue.ToString()))
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "showalert", "HideLoader();alert('Please select Plant ID.')", true);
                return;
            }
            if (string.IsNullOrEmpty(ddlCell.SelectedValue.ToString()))
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "showalert", "HideLoader();alert('Please select Cell ID.')", true);
                return;
            }
            if (string.IsNullOrEmpty(ddlmachine.SelectedValue.ToString()))
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "showalert", "HideLoader();alert('Please select Machine ID.')", true);
                return;
            }
            if (string.IsNullOrEmpty(txtFromDate.Text.ToString()))
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "showalert", "HideLoader();alert('Please select From DateTime.')", true);
                return;
            }
            if (string.IsNullOrEmpty(txtToDate.Text.ToString()))
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "showalert", "HideLoader();alert('Please select To DateTime.')", true);
                return;
            }
            DateTime StartTime = Util.GetDateTime(txtFromDate.Text.ToString());
            DateTime EndTime = Util.GetDateTime(txtToDate.Text.ToString());
            if ((EndTime - StartTime).Days > 7)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "showalert", "HideLoader();alert('Time interval cannot be more than 7 days.')", true);
                return;
            }
            BindDownData();
        }
        private void BindDownData()
        {
            try
            {
                string plant= ddlplant.SelectedValue == null ? "" : ddlplant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlplant.SelectedItem.ToString();
                string Cell = ddlCell.SelectedValue == null ? "" : ddlCell.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedItem.ToString();
                string machine = ddlmachine.SelectedValue == null ? "" : ddlmachine.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlmachine.SelectedItem.ToString();
                DownDataBind();
                List<DownTimeData> list = ShantiDataBaseAccess.getDownTimeQualificationDetails(plant,Cell,machine,txtFromDate.Text,txtToDate.Text);
                downdatagrid.DataSource = list;
                downdatagrid.DataBind();
                var downcodes= list.Select(o =>  o.DownInterfaceID ).Distinct().ToList();
                ddlfrmdown.DataSource = downcodes;
                ddlfrmdown.DataBind();
                BindFilterDownCode();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "loaderhide", "HideLoader()", true);
            }
            catch(Exception ex)
            { }
        }
        private void DownDataBind()
        {
            try
            {
                downInterfaceID = DataBaseAccess.GetAllParameterWithInteface(DateTime.Now, DateTime.Now, "", "", "", "", "", "", "", "Downid");
            }
            catch(Exception ex)
            { }
        }
        private void BindFilterDownCode()
        {
            try
            {
                DateTime StartTime = Util.GetDateTime(txtFromDate.Text.ToString());
                DateTime EndTime = Util.GetDateTime(txtToDate.Text.ToString());
                string MachineID = ddlmachine.SelectedValue == null ? "" : ddlmachine.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlmachine.SelectedItem.ToString();
                //ddlfrmdown.DataSource = ShantiDataBaseAccess.GetDownCodeFromAutodata(StartTime, EndTime,  MachineID);
                //ddlfrmdown.DataBind();
                ddldown_to.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineID, "", "", "", "", "", "Downid");
                ddldown_to.DataBind();
            }
            catch (Exception ex)
            { }
            finally
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "loaderhide", "HideLoader()", true);
            }
        }
        protected void downdatagrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdndowncode = (e.Row.FindControl("hdnDownCode") as HiddenField);
                DropDownList ddldowncode = (e.Row.FindControl("ddlDownCode") as DropDownList);
                ddldowncode.DataSource = downInterfaceID;
                ddldowncode.DataBind();

                if (ddldowncode.Items.FindByValue(hdndowncode.Value) == null)
                {
                    ddldowncode.Items.Add(new ListItem(hdndowncode.Value, hdndowncode.Value));
                }
                ddldowncode.SelectedValue = hdndowncode.Value;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlplant.Text.ToString())) return;
                if (string.IsNullOrEmpty(ddlmachine.Text.ToString())) return;
                try
                {
                    bool IsUpdated = false;
                    foreach (GridViewRow row in downdatagrid.Rows)
                    {
                        HiddenField hdfvalue = (HiddenField)row.FindControl("hdfdownsavecheck");
                        if (hdfvalue.Value.Equals("updated", StringComparison.OrdinalIgnoreCase))
                        {
                            DownTimeData data = new DownTimeData();
                            data.AutodataID = (row.FindControl("hdnAutoDataID") as HiddenField).Value;
                            data.DownInterfaceID = ((DropDownList)row.FindControl("ddlDownCode")).SelectedValue;
                            int rowCount; 
                            ShantiDataBaseAccess.UpdateDownCodedata(data, out IsUpdated,out rowCount);
                        }
                    }
                    if (IsUpdated)
                    {
                        BindDownData();
                        lblMessages.Text = "Record Updated Successfully.";
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.ToString());
                    throw;
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "loaderhide", "HideLoader()", true);
            }
            catch (Exception ex)
            {

            }
        }

        protected void downupdate_Click(object sender, EventArgs e)
        {
            int rowCount, rowCount1 = 0;
            foreach (GridViewRow row in downdatagrid.Rows)
            {
                DownTimeData data = new DownTimeData();
                data.AutodataID = (row.FindControl("hdnAutoDataID") as HiddenField).Value;
                data.DownInterfaceID = ddldown_to.SelectedValue.ToString().Split('<')[0].Trim();
                bool IsUpdated;
                ShantiDataBaseAccess.UpdateDownCodedata(data, out IsUpdated, out rowCount);
                if (IsUpdated)
                {
                    rowCount1++;
                }
            }
            lblMessages.Text = rowCount1 + " Row Updated Successfully.";
            BindDownData();
        }
        protected void ddlDayShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedShift = ddlDayShift.SelectedValue.ToString();
                string logicalDayStart = string.Empty, logicalDayEnd = string.Empty;
                if (selectedShift.Contains("Today"))
                {
                    if (ddlDayShift.SelectedValue.Equals("Today - All"))
                    {
                        logicalDayStart = CockpitDataBaseAccess.GetLogicalDay(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("dd-MMM-yyyy HH:mm");
                        //dtpTimeFrom.Value = Convert.ToDateTime(logicalDayStart);

                        logicalDayEnd = CockpitDataBaseAccess.GetLogicalDayEnd(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("dd-MMM-yyyy HH:mm");
                        //dtpTimeTo.Value = Convert.ToDateTime(logicalDayEnd);
                    }

                    else
                    {
                        int index = selectedShift.IndexOf('-');
                        var shift = CockpitDataBaseAccess.GetCurrentShiftTime(selectedShift.Substring(index + 1).Trim());
                        if (shift != null)
                        {
                            logicalDayStart = shift[0];
                            logicalDayEnd = shift[1];

                            txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("dd-MMM-yyyy HH:mm");
                            //dtpTimeFrom.Value = Convert.ToDateTime(logicalDayStart);

                            txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("dd-MMM-yyyy HH:mm");
                            //dtpTimeTo.Value = Convert.ToDateTime(logicalDayEnd);
                        }
                    }
                }
                else
                {

                    if (ddlDayShift.SelectedValue.Equals("Yesterday - All"))
                    {
                        logicalDayStart = CockpitDataBaseAccess.GetLogicalDay(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
                        txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("dd-MMM-yyyy HH:mm");
                        //dtpTimeFrom.Value = Convert.ToDateTime(logicalDayStart);

                        logicalDayEnd = CockpitDataBaseAccess.GetLogicalDayEnd(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
                        txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("dd-MMM-yyyy HH:mm");
                        //dtpTimeTo.Value = Convert.ToDateTime(logicalDayEnd);
                    }

                    else
                    {
                        int index = selectedShift.IndexOf('-');
                        var shift = CockpitDataBaseAccess.GetCurrentShiftTime(selectedShift.Substring(index + 1).Trim());
                        if (shift != null)
                        {
                            logicalDayStart = shift[0];
                            logicalDayEnd = shift[1];

                            txtFromDate.Text = Convert.ToDateTime(logicalDayStart).AddDays(-1).ToString("dd-MMM-yyyy HH:mm");
                            //dtpTimeFrom.Value = Convert.ToDateTime(logicalDayStart);

                            txtToDate.Text = Convert.ToDateTime(logicalDayEnd).AddDays(-1).ToString("dd-MMM-yyyy HH:mm");
                            //dtpTimeTo.Value = Convert.ToDateTime(logicalDayEnd);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
    }
}