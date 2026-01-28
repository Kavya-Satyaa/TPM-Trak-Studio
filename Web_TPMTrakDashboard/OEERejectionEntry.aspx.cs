using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class OEERejectionEntry : System.Web.UI.Page
    {
        List<string> shiftDetails = new List<string>();
        List<string> machineIds = new List<string>();
        string selectedShift = string.Empty;
        string selectedMachine = string.Empty;
        DateTime selectedTimePeriod = new DateTime();
        string logicalDayStart = string.Empty;
        string logicalDayEnd = string.Empty;
        string Rejectioncount = string.Empty;
        string shiftproductin = string.Empty;
        string ComponentProduction = string.Empty;
        string OperationProduction = string.Empty;
        string OperatorName = string.Empty;
        string IsSuccessfull = string.Empty;
        string rejType = "Rejection";
        string WorkOrderNumber = string.Empty;
        List<string> rejectionCat = new List<string>();
        List<string> rejCode = new List<string>();
        int RowIndexProduction = 0;
        string defaultMachine = string.Empty;
        string LotNumber = string.Empty;

        TaskScheduler uiThreadScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                machineIds = DataBaseAccess.GetAllEnabledMachines();
                ddlMachine.DataSource = machineIds;
                ddlMachine.DataBind();

                txtTimePeriod.Text = DateTime.Now.ToString("dd-MM-yyyy");

                ddlShift.DataSource = DataBaseAccess.GetAllShifts("OEE");
                ddlShift.DataBind();
                selectedShift = ddlShift.SelectedItem.Value.ToString();

                btnView_Click(null, EventArgs.Empty);
            }

        }

        private void SelectRejectionData()
        {

            try
            {
                RowIndexProduction = Convert.ToInt32(Session["selectedProductionRow"]);
                if (gvProductionCount.Rows.Count > 0 && gvProductionCount.DataSource != null)
                {
                    Rejectioncount = (gvProductionCount.Rows[RowIndexProduction].FindControl("lblPartsCount") as Label).Text;
                    shiftproductin = (gvProductionCount.Rows[RowIndexProduction].FindControl("lblShift") as Label).Text;
                    ComponentProduction = (gvProductionCount.Rows[RowIndexProduction].FindControl("lblComponent") as Label).Text;
                    WorkOrderNumber = (gvProductionCount.Rows[RowIndexProduction].FindControl("lblWorkOrderNumber") as Label).Text;
                    OperationProduction = (gvProductionCount.Rows[RowIndexProduction].FindControl("lblOperation") as Label).Text;
                    OperatorName = (gvProductionCount.Rows[RowIndexProduction].FindControl("lblOperator") as Label).Text;
                    LotNumber = (gvProductionCount.Rows[RowIndexProduction].FindControl("lblLotNumber") as Label).Text;
                    Session["Rejectioncount"] = Rejectioncount;
                    Session["shiftproductin"] = shiftproductin;
                    Session["ComponentProduction"] = ComponentProduction;
                    Session["WorkOrderNumber"] = WorkOrderNumber;
                    Session["OperationProduction"] = OperationProduction;
                    Session["OperatorName"] = OperatorName;
                    Session["LotNumber"] = LotNumber;
                    lblRejInform.Text = GetGlobalResourceObject("CommanResource", "RejectionDescription").ToString() + " : " + shiftproductin + "/" + ComponentProduction + "/" + OperationProduction + "/" + OperatorName + " ";
                }
                else
                {
                    Rejectioncount = string.Empty;
                    shiftproductin = string.Empty;
                    ComponentProduction = string.Empty;
                    WorkOrderNumber = string.Empty;
                    OperationProduction = string.Empty;
                    OperatorName = string.Empty;
                    LotNumber = string.Empty;
                    Session["Rejectioncount"] = Rejectioncount;
                    Session["shiftproductin"] = shiftproductin;
                    Session["ComponentProduction"] = ComponentProduction;
                    Session["WorkOrderNumber"] = WorkOrderNumber;
                    Session["OperationProduction"] = OperationProduction;
                    Session["OperatorName"] = OperatorName;
                    Session["LotNumber"] = LotNumber;
                    lblRejInform.Text = GetGlobalResourceObject("CommanResource", "RejectionDescription").ToString();

                }
                ViewRejectiongriddata(rejType);
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog(ex.Message);
            }

        }
        private void ViewRejectiongriddata(string Type)
        {
            try
            {
                string DayStart1 = Session["logicalStart"].ToString();

                shiftproductin = Session["shiftproductin"].ToString();
                ComponentProduction = Session["ComponentProduction"].ToString();
                OperationProduction = Session["OperationProduction"].ToString();
                OperatorName = Session["OperatorName"].ToString();
                selectedMachine = Session["selectedMachine"].ToString();
                LotNumber = Session["LotNumber"].ToString();
                DataTable dt = DataBaseAccess.getrejectiondata(DayStart1, shiftproductin, ComponentProduction, OperationProduction, OperatorName, selectedMachine, "", "", Type);
                if (dt.Rows.Count > 0)
                {
                    gvRejectionCount.DataSource = dt;
                    gvRejectionCount.DataBind();
                }
                else if (gvProductionCount.Rows.Count > 0)
                {

                    DataRow dataRow = dt.NewRow();
                    dataRow.SetField("rejectionid", "1");
                    dt.Rows.Add(dataRow);
                    gvRejectionCount.DataSource = dt;
                    gvRejectionCount.DataBind();
                    gvRejectionCount.Rows[0].Visible = false;
                }
                else
                {
                    DataRow dataRow = dt.NewRow();
                    dataRow.SetField("rejectionid", "1");
                    dt.Rows.Add(dataRow);
                    gvRejectionCount.DataSource = dt;
                    gvRejectionCount.DataBind();
                    gvRejectionCount.Rows[0].Visible = false;
                    gvRejectionCount.FooterRow.Visible = false;
                }

            }
            catch (Exception Ex)
            {
                Logger.WriteErrorLog(Ex.Message);
            }
        }

        //protected void btnNewNSave_Click(object sender, EventArgs e)
        //{
        //    //if (string.IsNullOrEmpty(Rejectioncount))
        //    //{
        //    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "No Parts", "alert('Parts count is empty. Cannot Add New Row.')", true);
        //    //    return;
        //    //}
        //    //if (gvRejectionCount.DataSource == null)
        //    //{
        //    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Empty Grid", "alert('Grid is Empty, Cannot Add New Row.')", true);
        //    //    return;
        //    //}
        //    if (btnNewNSave.Text.Equals("New", StringComparison.OrdinalIgnoreCase))
        //    {
        //        gvRejectionCount.ShowFooter = true;
        //        gvRejectionCount.FooterRow.Visible = true;
        //        //gvRejectionCount.SelectedIndex = gvRejectionCount.Rows.Count - 1;
        //        btnNewNSave.Text = "Save";
        //        btnNewNSave.ToolTip = "Save";
        //        btnDeleteNCancel.Visible = true;
        //    }
        //    else
        //    {

        //        string rejCat = (gvRejectionCount.FooterRow.FindControl("ddlRejCat") as DropDownList).SelectedItem.Value.ToString();
        //        string rejCode = (gvRejectionCount.FooterRow.FindControl("ddlRejCode") as DropDownList).SelectedItem.Value.ToString();
        //        string count = (gvRejectionCount.FooterRow.FindControl("txtCount") as TextBox).Text.ToString();
        //        if (string.Empty.Equals(count))
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "Emptybox", "alert('Please insert Rejection Count.')", true);
        //            return;
        //        }
        //        DataTable dt = gvRejectionCount.DataSource as DataTable;
        //        if (dt == null || dt.Rows.Count <= 0) return;
        //        int TotalCount = 0;
        //        string data2 = string.Empty;
        //        int res = 0;
        //        int result = 0;
        //        int indexOfCountColumn = 2;
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            if (row[indexOfCountColumn] != null)
        //            {
        //                if (row[indexOfCountColumn].ToString() != string.Empty)
        //                    data2 = row[indexOfCountColumn].ToString();
        //                int.TryParse(data2, out res);
        //                {
        //                    result = res;
        //                }
        //                {
        //                    TotalCount = TotalCount + result;
        //                }
        //            }
        //        }
        //        if (Rejectioncount != string.Empty)
        //        {
        //            if (TotalCount > Convert.ToInt32(Rejectioncount))
        //            {
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "InvalidCount", "alert('Rejection Count is not valid.')", true);
        //            }
        //        }
        //        DateTime DayStart = Convert.ToDateTime(logicalDayStart);
        //        string DayStart1 = DayStart.ToString("yyyy-MMM-dd");
        //        DataBaseAccess.SaveRejectionData(DayStart1, shiftproductin, selectedMachine, ComponentProduction, OperationProduction, OperatorName, rejCode, Convert.ToInt16(count), 0, WorkOrderNumber, "insert", out IsSuccessfull);
        //        if (IsSuccessfull.Equals("Success", StringComparison.OrdinalIgnoreCase))
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "alert('Data Inserted Successfully.')", true);
        //            gvRejectionCount.ShowFooter = false;
        //            btnNewNSave.Text = "New";
        //            btnNewNSave.ToolTip = "New";
        //            btnDeleteNCancel.Text = "Delete";
        //            btnDeleteNCancel.ToolTip = "Delete";
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Failure", "alert('Data Insertion Failed!')", true);
        //        }
        //        //ReadValuesForShift();
        //        ViewRejectiongriddata(rejType);
        //    }
        //}

        ////protected void btnDeleteNCancel_Click(object sender, EventArgs e)
        ////{
        ////    if (btnDeleteNCancel.Text.Equals("Cancel", StringComparison.OrdinalIgnoreCase))
        ////    {
        ////        gvRejectionCount.FooterRow.Visible = false;
        ////        btnDeleteNCancel.Visible = false;
        ////        btnNewNSave.Text = "New";
        ////        btnNewNSave.ToolTip = "New";
        ////    }
        ////    else
        ////    {
        ////        if (gvRejectionCount.SelectedValue != null)
        ////        {
        ////            int id = Convert.ToInt32((gvRejectionCount.SelectedRow.FindControl("lblID") as Label).Text);
        ////            if (DataBaseAccess.DeleteRejectionEntry(id))
        ////            {
        ////                ScriptManager.RegisterStartupScript(this, this.GetType(), "DeleteSuccessfull", "alert('Deleted Successfully')", true);
        ////                //ReadValuesForShift();
        ////            }
        ////            else
        ////            {
        ////                ScriptManager.RegisterStartupScript(this, this.GetType(), "DeleteUnsuccessfull", "alert('Delete Operation Failed!')", true);
        ////            }
        ////        }
        ////        else
        ////        {
        ////            ScriptManager.RegisterStartupScript(this, this.GetType(), "Notselected", "alert('Please first select a Rejection Count.')", true);
        ////            return;
        ////        }
        ////    }
        ////}

        protected void gvProductionCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvProductionCount.Rows)
                {
                    if (row.RowIndex == gvProductionCount.SelectedIndex)
                    {
                        row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                        row.ToolTip = string.Empty;
                    }
                    else
                    {
                        row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                        row.ToolTip = "Click to select this row.";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            GridViewRow selectedRow = gvProductionCount.SelectedRow;
            try
            {
                rejType = "Rejection";
                Session["selectedProductionRow"] = selectedRow.RowIndex;
                RowIndexProduction = Convert.ToInt32(Session["selectedProductionRow"]);
                if (gvProductionCount.Rows[RowIndexProduction].Cells[4].Text != null && gvProductionCount.Rows[RowIndexProduction].Cells[5].Text != null)
                {
                    Rejectioncount = (gvProductionCount.Rows[RowIndexProduction].FindControl("lblPartsCount") as Label).Text;
                    shiftproductin = (gvProductionCount.Rows[RowIndexProduction].FindControl("lblShift") as Label).Text;
                    ComponentProduction = (gvProductionCount.Rows[RowIndexProduction].FindControl("lblComponent") as Label).Text;
                    WorkOrderNumber = (gvProductionCount.Rows[RowIndexProduction].FindControl("lblWorkOrderNumber") as Label).Text;
                    OperationProduction = (gvProductionCount.Rows[RowIndexProduction].FindControl("lblOperation") as Label).Text;
                    OperatorName = (gvProductionCount.Rows[RowIndexProduction].FindControl("lblOperator") as Label).Text;
                    LotNumber = (gvProductionCount.Rows[RowIndexProduction].FindControl("lblLotNumber") as Label).Text;
                    lblRejInform.Text = GetGlobalResourceObject("CommanResource", "RejectionDescription").ToString() + " : " + shiftproductin + "/" + ComponentProduction + "/" + OperationProduction + "/" + OperatorName + " ";

                    Session["Rejectioncount"] = Rejectioncount;
                    Session["shiftproductin"] = shiftproductin;
                    Session["ComponentProduction"] = ComponentProduction;
                    Session["WorkOrderNumber"] = WorkOrderNumber;
                    Session["OperationProduction"] = OperationProduction;
                    Session["OperatorName"] = OperatorName;
                    Session["LotNumber"] = LotNumber;
                }

                ViewRejectiongriddata("Rejection");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

        }

        protected void gvProductionCount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvProductionCount, "Select$" + e.Row.RowIndex);
                    e.Row.ToolTip = "Click to select this row.";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void gvRejectionCount_DataBound(object sender, EventArgs e)
        {
            try
            {
                rejectionCat = DataBaseAccess.GetRejCatagory("catagory");
                if (rejectionCat.Count > 0)
                {
                    DropDownList ddlRCat = gvRejectionCount.FooterRow.FindControl("ddlRejCat") as DropDownList;
                    ddlRCat.DataSource = rejectionCat;
                    ddlRCat.DataBind();
                }
                rejCode = DataBaseAccess.GetRejCatagory("code");
                if (rejCode.Count > 0)
                {
                    DropDownList ddlRCode = gvRejectionCount.FooterRow.FindControl("ddlRejCode") as DropDownList;
                    ddlRCode.DataSource = rejCode;
                    ddlRCode.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void cbAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbAutoRefresh.Checked)
                {

                    timerToAutoRefresh.Enabled = true;
                    timerToAutoRefresh.Interval = 10000;
                    //ScriptManager.RegisterStartupScript(this, GetType(), "showDiv", "document.getElementById('overlay').style.display = 'block';", true);                
                }
                else
                {
                    timerToAutoRefresh.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }



        protected void gvRejectionCount_RowDataBound(object sender, GridViewRowEventArgs e)
        {


        }

        protected void gvRejectionCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvRejectionCount.Rows)
                {
                    if (row.RowIndex == gvRejectionCount.SelectedIndex)
                    {
                        row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                        row.ToolTip = string.Empty;
                    }
                    else
                    {
                        row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                        row.ToolTip = "Click to select this row.";
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
            try
            {
                bool timerOff = false;
                if (timerToAutoRefresh.Enabled)
                {
                    timerToAutoRefresh.Enabled = false;
                    timerOff = true;
                }
                Session["selectedProductionRow"] = 0;
                selectedShift = ddlShift.SelectedItem.Value.ToString().Equals("Today - All", StringComparison.OrdinalIgnoreCase)?"": ddlShift.SelectedItem.Value.ToString();
                Session["selectedShift"] = selectedShift;
                Session["selectedMachine"] = ddlMachine.SelectedItem.Value.ToString();
                selectedMachine = Session["selectedMachine"].ToString();
                selectedTimePeriod = DateTime.ParseExact(txtTimePeriod.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string shidtStartTime = "", shiftEndTime = "";
                logicalDayStart = DataBaseAccess.GetLogicalDay(selectedTimePeriod.ToString("yyyy-MM-dd hh:mm:ss"));
                logicalDayEnd = DataBaseAccess.GetLogicalDayEnd(selectedTimePeriod.ToString("yyyy-MM-dd hh:mm:ss"));
                if (selectedShift.Equals("Today - All", StringComparison.OrdinalIgnoreCase))
                {
                    //logicalDayStart = DataBaseAccess.GetLogicalDay(selectedTimePeriod.ToString("yyyy-MM-dd HH:mm:ss"));
                    //logicalDayEnd = DataBaseAccess.GetLogicalDayEnd(selectedTimePeriod.ToString("yyyy-MM-dd HH:mm:ss"));
                    shidtStartTime = DataBaseAccess.GetLogicalDay(selectedTimePeriod.ToString("yyyy-MM-dd hh:mm:ss"));
                    shiftEndTime = DataBaseAccess.GetLogicalDayEnd(selectedTimePeriod.ToString("yyyy-MM-dd hh:mm:ss"));
                }
                else
                {
                    shiftDetails = DataBaseAccess.GetCurrentShiftTime(selectedShift, selectedTimePeriod.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (shiftDetails != null)
                    {
                        //logicalDayStart = shiftDetails[0];
                        //logicalDayEnd = shiftDetails[1];
                        shidtStartTime = shiftDetails[0];
                        shiftEndTime = shiftDetails[1];
                    }
                }
                Session["logicalStart"] = logicalDayStart;
                Session["logicalEnd"] = logicalDayEnd;
                Session["ShiftStartTime"] = shidtStartTime;
                Session["ShiftEndTime"] = shiftEndTime;
                string downtime = DataBaseAccess.GetDownTime(selectedMachine, shidtStartTime, shiftEndTime);
                if (string.IsNullOrEmpty(downtime))
                {
                    lblDownTime.Text = GetGlobalResourceObject("CommanResource", "DownTime").ToString() + " (00:00:00)";
                }
                else
                {
                    lblDownTime.Text = GetGlobalResourceObject("CommanResource", "DownTime").ToString() + " (" + downtime + ")";
                }

                //bindcockpit();
                BindDownTime();
                BindProdCount();
                SelectRejectionData();
                if (timerOff)
                {
                    timerToAutoRefresh.Enabled = true;
                    timerToAutoRefresh.Interval = 10000;
                }
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "hideDiv", "document.getElementById('overlay').style.display = 'none'", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hideDiv", "$.unblockUI({});", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        //public async Task bindcockpit()
        //{
        //    List<CockpitValues> cockpitValues = new List<CockpitValues>();
        //    cockpitValues =await Task.Run(()=> DataBaseAccess.GetCockPitValues(selectedMachine, logicalDayStart, logicalDayEnd));
        //    lvMachineValues.DataSource = cockpitValues;
        //    lvMachineValues.DataBind();
        //}
        public void BindDownTime()
        {
            try
            {
                DataTable allDowns = new DataTable();
                allDowns = DataBaseAccess.GetDownsForTheMachine(Session["selectedMachine"].ToString(), Session["ShiftStartTime"].ToString(), Session["ShiftEndTime"].ToString());
                gvDownTime.DataSource = allDowns;
                gvDownTime.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        public void BindProdCount()
        {
            try
            {
                DataTable allProductions = new DataTable();
                allProductions = DataBaseAccess.GetProductionsForTheMachine(Session["selectedMachine"].ToString(), Session["selectedShift"].ToString(), Session["logicalStart"].ToString(), Session["logicalEnd"].ToString());
                if (!allProductions.Columns.Contains("LotNumber"))
                {
                    allProductions.Columns.Add("LotNumber", typeof(string));
                }
                gvProductionCount.DataSource = allProductions;
                gvProductionCount.DataBind();
                if (!DataBaseAccess.WorkOrderVisibility())
                {
                    gvProductionCount.Columns[8].Visible = false;
                }
                if (WebConfigurationManager.AppSettings["SKSPages"].ToString() == "1")
                {
                    gvProductionCount.Columns[9].Visible = true;
                }
                else
                {
                    gvProductionCount.Columns[9].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void timerToAutoRefresh_Tick(object sender, EventArgs e)
        {

            btnView_Click(null, EventArgs.Empty);
        }

        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Rejectioncount = Session["Rejectioncount"].ToString();
                if (string.IsNullOrEmpty(Rejectioncount))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "No Parts", "alert('Parts count is empty. Cannot Add New Row.')", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hideDiv", "$.unblockUI({});", true);
                    return;
                }
                string rejCat = (gvRejectionCount.FooterRow.FindControl("ddlRejCat") as DropDownList).SelectedItem.Value.ToString();
                string rejCode = (gvRejectionCount.FooterRow.FindControl("ddlRejCode") as DropDownList).SelectedItem.Value.ToString();
                string count = (gvRejectionCount.FooterRow.FindControl("txtCount") as TextBox).Text.ToString();
                if (string.Equals(rejCat, "Select Catagory", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Emptybox", "alert('Please select a Rejection Catagory.')", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hideDiv", "$.unblockUI({});", true);
                    return;
                }
                else if (string.Equals(rejCode, "Select Code", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Emptybox", "alert('Please select a Rejection Code.')", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hideDiv", "$.unblockUI({});", true);
                    return;
                }
                else if (string.Empty.Equals(count))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Emptybox", "alert('Please insert RejectionCount.')", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hideDiv", "$.unblockUI({});", true);
                    return;
                }
                int totalCount = 0;
                int rejcount = 0;
                foreach (GridViewRow row in gvRejectionCount.Rows)
                {
                    if (!string.IsNullOrEmpty((row.FindControl("lblCount") as Label).Text))
                    {
                        rejcount = Convert.ToInt32((row.FindControl("lblCount") as Label).Text);
                        totalCount = totalCount + rejcount;
                    }

                }
                if ((totalCount + Convert.ToInt32(count)) > Convert.ToInt32(Rejectioncount))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "InvalidRejCount", "alert('Rejection Count can not be greater than Parts Count.')", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hideDiv", "$.unblockUI({});", true);
                    return;
                }
                DateTime DayStart = DateTime.Now;
                DateTime.TryParse(Session["logicalStart"].ToString(), out DayStart);
                string DayStart1 = DayStart.ToString("yyyy-MM-dd");
                shiftproductin = Session["shiftproductin"].ToString();
                ComponentProduction = Session["ComponentProduction"].ToString();
                OperationProduction = Session["OperationProduction"].ToString();
                OperatorName = Session["OperatorName"].ToString();
                selectedMachine = Session["selectedMachine"].ToString();
                WorkOrderNumber = Session["WorkOrderNumber"].ToString();
                LotNumber = Session["LotNumber"].ToString();
                DataBaseAccess.SaveRejectionData(DayStart1, shiftproductin, selectedMachine, ComponentProduction, OperationProduction, OperatorName, rejCode, Convert.ToInt16(count), 0, WorkOrderNumber, "insert", out IsSuccessfull, LotNumber);

                if (IsSuccessfull.Equals("Success", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "alert('Data Inserted Successfully.')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Failure", "alert('Data Insertion Failed!')", true);
                }
                BindProdCount();
                SelectRejectionData();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hideDiv", "$.unblockUI({});", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as GridViewRow).RowIndex);
                GridViewRow row = gvRejectionCount.Rows[rowIndex];
                int id = Convert.ToInt32((row.FindControl("lblID") as HiddenField).Value);
                string RejCode = (row.FindControl("lblRejCode") as Label).Text;
                if (DataBaseAccess.DeleteRejectionEntry(id, RejCode))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "DeleteSuccessfull", "alert('Deleted Successfully')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "DeleteUnsuccessfull", "alert('Delete Operation Failed!')", true);
                }
                BindProdCount();
                SelectRejectionData();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hideDiv", "$.unblockUI({});", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlRejCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlRejCatagory = gvRejectionCount.FooterRow.FindControl("ddlRejCat") as DropDownList;
                string selectedCatagory = ddlRejCatagory.SelectedItem.Value.ToString();
                rejCode = DataBaseAccess.GetSelectedRejCatagory(selectedCatagory);
                if (rejCode.Count > 0)
                {
                    DropDownList ddlRCode = gvRejectionCount.FooterRow.FindControl("ddlRejCode") as DropDownList;
                    ddlRCode.DataSource = rejCode;
                    ddlRCode.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }


    }

    public class CockpitValues
    {
        public string LabelText { get; set; }
        public string LabelValue { get; set; }
        public string ForeColor { get; set; }
        public string BackColor { get; set; }

    }


}
