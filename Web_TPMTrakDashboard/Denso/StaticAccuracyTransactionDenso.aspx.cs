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
    public partial class StaticAccuracyTransactionDenso : System.Web.UI.Page
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
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
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
                DataTable dt = DensoDBAccess.getStaticAccuracyTransactionDetails(ddlMachine.SelectedValue, txtYear.Text, ddlMonth.SelectedValue, "TransactionView");

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


        protected void lvCheckpointValue_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    string checkpointType = (e.Item.FindControl("hdnCheckpointType") as HiddenField).Value;
                    string actualValue = (e.Item.FindControl("hdnActualValue") as HiddenField).Value;
                    if (checkpointType == "Checkbox")
                    {
                        if (actualValue.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            (e.Item.FindControl("chkValue") as CheckBox).Checked = true;
                        }
                    }
                    else if (checkpointType == "OK-NG" || checkpointType == "Yes-No")
                    {
                        if (actualValue == "Yes" || actualValue == "OK")
                        {
                            actualValue = "OK";
                        }
                        else if (actualValue == "NG" || actualValue == "No")
                        {
                            actualValue = "NG";
                        }
                        HelperClassGeneric.setDropdownValue((e.Item.FindControl("ddlValue") as DropDownList), actualValue);
                    }
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
                int updateCount = 0;
                int i = 0;
                foreach (ListViewItem item in lvChecklist.Items)
                {
                    if (i == 0)
                    {
                        i++;
                        continue;
                    }

                    StaticAccuracyTransEntity data = new StaticAccuracyTransEntity();
                    data.MachineID = ddlMachine.SelectedValue;
                    data.Checkpoint = (item.FindControl("lblCheckpoint") as Label).Text;
                    data.SortOrder = (item.FindControl("lblSortOrder") as Label).Text;
                    data.Year = txtYear.Text;
                    data.CheckpointType = (item.FindControl("hdnCheckpointType") as HiddenField).Value;
                    ListView lvMonth = item.FindControl("lvChecksheetMonth") as ListView;
                    foreach (ListViewItem dayShiftItem in lvMonth.Items)
                    {
                        data.Month = (dayShiftItem.FindControl("hdnMonth") as HiddenField).Value;
                        ListView lvActualValue = dayShiftItem.FindControl("lvCheckpointValue") as ListView;
                        foreach (ListViewItem valueItem in lvActualValue.Items)
                        {
                            if (HelperClassGeneric.getCheckboxValue((valueItem.FindControl("hdnControlEnabled") as HiddenField).Value))
                            {
                                data.WeekNo = (valueItem.FindControl("hdnWeekNo") as HiddenField).Value;
                                data.UpdatedBy = Session["UserName"].ToString();
                                if (data.CheckpointType.Equals("Text", StringComparison.OrdinalIgnoreCase))
                                {
                                    data.ActualValue = (valueItem.FindControl("txtValue") as TextBox).Text;
                                }
                                else if (data.CheckpointType.Equals("OK-NG", StringComparison.OrdinalIgnoreCase))
                                {
                                    data.ActualValue = (valueItem.FindControl("ddlValue") as DropDownList).SelectedValue;
                                }
                                else if (data.CheckpointType.Equals("Checkbox", StringComparison.OrdinalIgnoreCase))
                                {
                                    data.ActualValue = (valueItem.FindControl("chkValue") as CheckBox).Checked.ToString();
                                }
                                else if (data.CheckpointType.Equals("Image", StringComparison.OrdinalIgnoreCase))
                                {
                                    data.FileInBase64 = (valueItem.FindControl("hdnFileInBase64") as HiddenField).Value;
                                    if (data.FileInBase64 != "")
                                    {
                                        byte[] drawinginbytes = System.Convert.FromBase64String(data.FileInBase64.Substring(data.FileInBase64.LastIndexOf(',') + 1));
                                        data.FileInByte = drawinginbytes;
                                        data.FileName = (valueItem.FindControl("hdnFileName") as HiddenField).Value;
                                    }
                                }
                                string result = ""; string prevValue = (valueItem.FindControl("hdnActualValue") as HiddenField).Value;
                                if (data.CheckpointType.Equals("OK-NG", StringComparison.OrdinalIgnoreCase) || data.CheckpointType.Equals("Yes-No", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (prevValue == "" && data.ActualValue != "None")
                                    {
                                        result = DensoDBAccess.insertUpdateStaticAccuracyTransactionDetails(data);
                                    }
                                    else if (prevValue != "" && ((valueItem.FindControl("hdnActualValue") as HiddenField).Value) != data.ActualValue)
                                    {
                                        result = DensoDBAccess.insertUpdateStaticAccuracyTransactionDetails(data);
                                    }
                                }
                                else if (data.CheckpointType == "Checkbox")
                                {
                                    if (prevValue == "" && data.ActualValue != "False")
                                    {
                                        result = DensoDBAccess.insertUpdateStaticAccuracyTransactionDetails(data);
                                    }
                                    else if (prevValue != "" && ((valueItem.FindControl("hdnActualValue") as HiddenField).Value) != data.ActualValue)
                                    {
                                        result = DensoDBAccess.insertUpdateStaticAccuracyTransactionDetails(data);
                                    }
                                }
                                else
                                {
                                    if (((valueItem.FindControl("hdnActualValue") as HiddenField).Value) != data.ActualValue)
                                    {
                                        result = DensoDBAccess.insertUpdateStaticAccuracyTransactionDetails(data);
                                    }
                                }
                                if (result == "Updated" || result == "Inserted")
                                {
                                    updateCount++;
                                }
                            }
                        }
                    }
                    i++;
                }
                if (updateCount > 0)
                {
                    HelperClassGeneric.openUpdateSuccessModal(this);
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

    }
}