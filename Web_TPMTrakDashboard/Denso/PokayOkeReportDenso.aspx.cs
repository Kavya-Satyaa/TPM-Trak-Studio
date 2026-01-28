using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Denso.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Denso
{
    public partial class PokayOkeReportDenso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtYear.Text = DateTime.Now.Year.ToString();
                HelperClassGeneric.setDropdownValue(ddlMonth, DateTime.Now.ToString("MM"));
                BindMachine();
                BindData();
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllMachines("");
                ddlMultiMachineId.DataSource = list;
                ddlMultiMachineId.DataBind();
                ddlMultiMachineId.SelectedIndex = 0;
                foreach (ListItem item in ddlMultiMachineId.Items)
                {
                    item.Selected = true;
                }
                //ddlMachine.DataSource = list;
                //ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindData()
        {
            try
            {
                string machineId = "";
                foreach (ListItem item in ddlMultiMachineId.Items)
                {
                    if (item.Selected)
                    {
                        if (machineId == "")
                            machineId += item.Value;
                        else
                            machineId += "," + item.Value;
                    }
                }
                DataTable dt = DensoDBAccess.getPokayOkeTransactionDetails(machineId, txtYear.Text, ddlMonth.SelectedValue, "Report_CheckSheetDetails");
                int currentWeekNumber = DataBaseAccess.getCurrentWeekNumber();
                List<PokeyOkeTransEntity> list = new List<PokeyOkeTransEntity>();
                var distMachine = dt.AsEnumerable().Select(k => k["MachineID"].ToString()).Distinct().ToList();
                var distYear = dt.AsEnumerable().Select(k => k["Year"].ToString()).Distinct().ToList();
                var distWeekNo = dt.AsEnumerable().Select(k => k["WeekNo"].ToString()).Distinct().ToList();

                PokeyOkeTransEntity headerData = new PokeyOkeTransEntity();
                for (int machineC = 0; machineC < distMachine.Count; machineC++)
                {

                    DataTable dtMachines = dt.AsEnumerable().Where(k => k["MachineID"].ToString().Equals(distMachine[machineC], StringComparison.OrdinalIgnoreCase)).CopyToDataTable();
                  
                    var distChekpoint = dtMachines.AsEnumerable().Select(k => k["Checkpoint"].ToString()).Distinct().ToList();
                    PokeyOkeTransEntity mainData = new PokeyOkeTransEntity();
                   
                    int i = 0;
                    foreach (string checkpoint in distChekpoint)
                    {
                        int headerRowInsertFlag = 0;
                        var firstRow = dtMachines.AsEnumerable().Where(k => k["Checkpoint"].ToString().Equals(checkpoint, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        mainData = new PokeyOkeTransEntity();
                        mainData.SlNo = (machineC + 1).ToString();
                        mainData.MachineID = firstRow["MachineID"].ToString();
                        mainData.PokayOkeItem = firstRow["Item"].ToString();
                        mainData.Function = firstRow["Function"].ToString();
                        mainData.CheckMethod = firstRow["CheckPoint"].ToString();
                        mainData.CheckInterval = firstRow["CheckInterval"].ToString();
                        mainData.RevID = firstRow["RevID"].ToString();
                        mainData.RevNo = firstRow["RevNo"].ToString();
                        mainData.RevDate = firstRow["RevDate"].ToString();
                        mainData.RevisedBy = firstRow["RevisedBy"].ToString();
                        mainData.CheckpointType = firstRow["ChecklistType"].ToString();
                        mainData.SortOrder = firstRow["SortOrder"].ToString();
                        mainData.ContentVisibility = true;
                        if (i == 0)
                        {
                            mainData.RowSpan = distChekpoint.Count().ToString();
                        }
                        else
                        {
                            mainData.RowSpanDisplay = "none";
                        }
                        if (machineC == 0 && i == 0)
                        {
                            headerRowInsertFlag = 1;
                        }
                        if (headerRowInsertFlag == 1)
                        {
                            headerData.HeaderVisibility = true;
                        }

                        List<PokeyOkeWeekEntity> weekList = new List<PokeyOkeWeekEntity>();
                        List<PokeyOkeWeekEntity> weekListtHeader = new List<PokeyOkeWeekEntity>();
                        foreach (string weekNo in distWeekNo)
                        {
                            PokeyOkeWeekEntity weekData = new PokeyOkeWeekEntity();

                            weekData.WeekNumber = weekNo;
                            var dataValueRows = dtMachines.AsEnumerable().Where(k => k["CheckPoint"].ToString().Equals(checkpoint, StringComparison.OrdinalIgnoreCase) && k["WeekNo"].ToString() == weekNo).FirstOrDefault();
                            weekData.ContentVisibility = true;
                            weekData.CheckpointType = mainData.CheckpointType;
                            if (dataValueRows != null)
                            {
                                weekData.ActualValue = dataValueRows["Value"].ToString();
                                if (weekData.ActualValue == "OK" || weekData.ActualValue == "Yes")
                                {
                                    weekData.CheckpointTypeBackColor = "green";
                                    weekData.CheckpointTypeForeColor = "white";
                                }
                                if (weekData.ActualValue == "NG" || weekData.ActualValue == "No")
                                {
                                    weekData.CheckpointTypeBackColor = "red";
                                    weekData.CheckpointTypeForeColor = "white";
                                }

                                if (weekData.CheckpointType.Equals("Image", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (dataValueRows["ImageData"].ToString() == "")
                                    {
                                        weekData.FileInBase64 = "";
                                    }
                                    else
                                    {
                                        weekData.FileName = dataValueRows["ImageName"].ToString();
                                        byte[] bytes = (byte[])dataValueRows["ImageData"];
                                        weekData.FileInBase64 = Convert.ToBase64String(bytes);
                                    }
                                }
                            }
                            if (Convert.ToInt32(weekNo) == currentWeekNumber)
                            {
                                weekData.ControlEnabled = true;
                            }
                            else
                            {
                                weekData.ControlEnabled = false;
                            }
                            weekList.Add(weekData);
                            if (headerRowInsertFlag == 1)
                            {
                                PokeyOkeWeekEntity weekDataHeader = new PokeyOkeWeekEntity();
                                weekDataHeader.HeaderVisibility = true;
                                weekDataHeader.WeekNumber = "Week-" + weekNo;
                                weekListtHeader.Add(weekDataHeader);
                            }
                        }

                        mainData.WeekList = weekList;
                        if (headerRowInsertFlag == 1)
                        {
                            headerData.WeekList = weekListtHeader;
                            list.Add(headerData);
                        }
                        list.Add(mainData);
                        i++;
                    }
                }
                lvChecklist.DataSource = list;
                lvChecklist.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void lvChecksheetWeek_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                //if (e.Item.ItemType == ListViewItemType.DataItem)
                //{
                //    string checkpointType = (e.Item.FindControl("hdnCheckpointType") as HiddenField).Value;
                //    string actualValue = (e.Item.FindControl("hdnActualValue") as HiddenField).Value;
                //    if (checkpointType == "Checkbox")
                //    {
                //        if (actualValue.Equals("true", StringComparison.OrdinalIgnoreCase))
                //        {
                //            (e.Item.FindControl("chkValue") as CheckBox).Checked = true;
                //        }
                //    }
                //    else if (checkpointType == "Yes-No")
                //    {
                //        HelperClassGeneric.setDropdownValue((e.Item.FindControl("ddlValue") as DropDownList), actualValue);
                //    }
                //}
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
                string ReportStatus = "";
                string machineId = "";
                foreach (ListItem item in ddlMultiMachineId.Items)
                {
                    if (item.Selected)
                    {
                        if (machineId == "")
                            machineId += item.Value;
                        else
                            machineId += "," + item.Value;
                    }
                }
                DataTable dt = DensoDBAccess.getPokayOkeTransactionDetails(machineId, txtYear.Text, ddlMonth.SelectedValue, "Report_CheckSheetDetails");
                if (dt.Rows.Count != 0)
                {
                    ReportStatus = ReportsDenso.GetPokayOkeReport(machineId, txtYear.Text, ddlMonth.SelectedValue);
                }
                if (ReportStatus.Equals("Failed", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageFailure", "messageFailure();", true);
                else if (ReportStatus.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageSuccess", "messageSuccess();", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageFailure", "messageFailure();", true);
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}