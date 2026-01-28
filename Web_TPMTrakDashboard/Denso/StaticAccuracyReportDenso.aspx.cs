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
    public partial class StaticAccuracyReportDenso : System.Web.UI.Page
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
                //foreach (string strMachine in list)
                //{
                //    chkMachine.Items.Add(strMachine);
                //}
                //chkMachine.DataSource = list;
                //chkMachine.DataBind();
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
                string monthName = ddlMonth.SelectedValue.ToString();
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
                DataTable dt = DensoDBAccess.getStaticAccuracyTransactionDetails(machineId, txtYear.Text, monthName.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : monthName, "Report");
                List<StaticAccuracyTransEntity> list = new List<StaticAccuracyTransEntity>();
                var distMachine = dt.AsEnumerable().Select(k => k["MachineID"].ToString()).Distinct().ToList();
                var distYear = dt.AsEnumerable().Select(k => k["Year"].ToString()).Distinct().ToList();
                var distMonth = dt.AsEnumerable().Select(k => k["Month"].ToString()).Distinct().ToList();

                int currentWeekNumber = DataBaseAccess.getCurrentWeekNumber();
                StaticAccuracyTransEntity headerData = new StaticAccuracyTransEntity();
                for (int machineC = 0; machineC < distMachine.Count; machineC++)
                {

                    DataTable dtMachines = dt.AsEnumerable().Where(k => k["MachineID"].ToString().Equals(distMachine[machineC], StringComparison.OrdinalIgnoreCase)).CopyToDataTable();

                    StaticAccuracyTransEntity mainData = new StaticAccuracyTransEntity();
                    var distChekpoint = dtMachines.AsEnumerable().Select(k => k["Checkpoint"].ToString()).Distinct().ToList();
                    int i = 0;
                    //mainData.RowSpan = distChekpoint.Count().ToString();
                    foreach (string checkpoint in distChekpoint)
                    {
                        int headerRowInsertFlag = 0;

                        var firstRow = dtMachines.AsEnumerable().Where(k => k["Checkpoint"].ToString().Equals(checkpoint, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        mainData = new StaticAccuracyTransEntity();
                        mainData.Checkpoint = firstRow["Checkpoint"].ToString();
                        mainData.SortOrder = firstRow["CheckpointID"].ToString();
                        mainData.MachineID = firstRow["MachineID"].ToString();
                        mainData.CheckpointType = firstRow["CheckpointType"].ToString();
                        mainData.ContentVisibility = true;
                        if (i == 0)
                        {
                            mainData.RowSpan = distChekpoint.Count().ToString();
                        }
                        else
                        {
                            mainData.MachineDisplay = "none";
                        }
                        if (machineC == 0 && i == 0)
                        {
                            headerRowInsertFlag = 1;
                        }
                        if (headerRowInsertFlag == 1)
                        {
                            headerData.HeaderVisibility = true;
                        }

                        List<StaticAccuracyMonthEntity> monthList = new List<StaticAccuracyMonthEntity>();
                        List<StaticAccuracyMonthEntity> monthListHeader = new List<StaticAccuracyMonthEntity>();
                        foreach (string month in distMonth)
                        {
                            StaticAccuracyMonthEntity monthData = new StaticAccuracyMonthEntity();
                            StaticAccuracyMonthEntity monthDataHeader = new StaticAccuracyMonthEntity();
                            monthData.Month = month;
                            if (headerRowInsertFlag == 1)
                            {
                                monthDataHeader.Month = month;
                                monthDataHeader.DisplayMonth = HelperClassGeneric.getAbbreviatedMonthName(month);
                            }

                            List<StaticAccuracyTransValueEntity> valueList = new List<StaticAccuracyTransValueEntity>();
                            List<StaticAccuracyTransValueEntity> valueListHeader = new List<StaticAccuracyTransValueEntity>();
                            var dataValueRows = dtMachines.AsEnumerable().Where(k => k["Checkpoint"].ToString().Equals(checkpoint, StringComparison.OrdinalIgnoreCase) && k["Month"].ToString() == month).ToList();
                            foreach (DataRow valueRow in dataValueRows)
                            {
                                StaticAccuracyTransValueEntity valueData = new StaticAccuracyTransValueEntity();
                                valueData.ContentVisibility = true;
                                valueData.ActualValue = valueRow["Value"].ToString();
                                valueData.CheckpointType = mainData.CheckpointType;
                                if (valueData.ActualValue == "OK" || valueData.ActualValue == "Yes")
                                {
                                    valueData.CheckpointTypeBackColor = "green";
                                    valueData.CheckpointTypeForeColor = "white";
                                }
                                if (valueData.ActualValue == "NG" || valueData.ActualValue == "No")
                                {
                                    valueData.CheckpointTypeBackColor = "red";
                                    valueData.CheckpointTypeForeColor = "white";
                                }
                                valueData.WeekNumber = valueRow["WeekNo"].ToString();
                                if (Convert.ToInt32(valueData.WeekNumber) == currentWeekNumber)
                                {
                                    valueData.ControlEnabled = true;
                                }
                                else
                                {
                                    valueData.ControlEnabled = false;
                                }
                                if (valueData.CheckpointType.Equals("Image", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (valueRow["ImageData"].ToString() == "")
                                    {
                                        valueData.FileInBase64 = "";
                                    }
                                    else
                                    {
                                        valueData.FileName = valueRow["ImageName"].ToString();
                                        byte[] bytes = (byte[])valueRow["ImageData"];
                                        valueData.FileInBase64 = Convert.ToBase64String(bytes);
                                    }
                                }
                                valueList.Add(valueData);

                                if (headerRowInsertFlag == 1)
                                {
                                    StaticAccuracyTransValueEntity valueDataHeader = new StaticAccuracyTransValueEntity();
                                    valueDataHeader.HeaderVisibility = true;
                                    valueDataHeader.WeekNumber = "W" + valueRow["WeekNo"].ToString();
                                    valueListHeader.Add(valueDataHeader);
                                }
                            }
                            monthData.ValueList = valueList;
                            monthList.Add(monthData);

                            monthDataHeader.ValueList = valueListHeader;
                            monthListHeader.Add(monthDataHeader);
                        }

                        mainData.MonthList = monthList;

                        if (headerRowInsertFlag == 1)
                        {
                            headerData.MonthList = monthListHeader;
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
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            try
            {
                string ReportStatus = "";
                string monthName = ddlMonth.SelectedValue.ToString();
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
                dt = DensoDBAccess.getStaticAccuracyTransactionDetails(machineId, txtYear.Text, monthName.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : monthName, "Report");
                if (dt.Rows.Count != 0)
                {
                    ReportStatus = ReportsDenso.GetStaticAccuracyReport(machineId, txtYear.Text, monthName.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : monthName);
                }
                if (ReportStatus.Equals("Failed", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageFailure", "messageFailure();", true);
                else if (ReportStatus.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageSuccess", "messageSuccess();", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageFailure", "messageFailure();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void lvCheckpointValue_ItemDataBound(object sender, ListViewItemEventArgs e)
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
    }
}