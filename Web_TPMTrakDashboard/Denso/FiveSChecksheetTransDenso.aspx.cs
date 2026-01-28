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
    public partial class FiveSChecksheetTransDenso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtYear.Text = DateTime.Now.Year.ToString();
                HelperClassGeneric.setDropdownValue(ddlMonth, DateTime.Now.ToString("MM"));
                BindMachine();
                BindWeekNo();
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
        private void BindWeekNo()
        {
            try
            {
                List<string> list = DataBaseAccess.getWeekNumbersOfYear(txtYear.Text);
                ddlWeekNo.DataSource = list;
                ddlWeekNo.DataBind();
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
                DataTable dt = DensoDBAccess.getFiveSCheckpointTransactionDetails(ddlMachine.SelectedValue, txtYear.Text, ddlMonth.SelectedValue, "TransactionView");

                List<FiveSChecksheetTransEntity> list = new List<FiveSChecksheetTransEntity>();
                var distDate = dt.AsEnumerable().Select(k => k.Field<DateTime>("Date")).Distinct().ToList();
                var distCheckpoints = dt.AsEnumerable().Select(k => k["CheckItem"].ToString()).Distinct().ToList();
                //var distShift = dt.AsEnumerable().Where(k => !string.IsNullOrEmpty(k["ShiftID"].ToString())).Select(k => k["ShiftID"].ToString()).Distinct().ToList();

                FiveSChecksheetTransEntity headerData = new FiveSChecksheetTransEntity();
                int i = 0;
                int slNoCount = 1;
                foreach (string checkPoint in distCheckpoints)
                {
                    var distShift = dt.AsEnumerable().Where(k => k["CheckItem"].ToString().Equals(checkPoint, StringComparison.OrdinalIgnoreCase)).Select(k => k["ShiftID"].ToString()).Distinct().ToList();

                    int checkpointC = 0;
                    string cycleTimeDisplay = "";
                    int cycleTimeListDataUpdateIndex = 0;
                    foreach (string shift in distShift)
                    {
                        if (cycleTimeDisplay == "")
                        {
                            cycleTimeListDataUpdateIndex = list.Count();
                        }
                        FiveSChecksheetTransEntity mainData = new FiveSChecksheetTransEntity();
                        DataRow firstRow = dt.AsEnumerable().Where(k => k["CheckItem"].ToString().Equals(checkPoint, StringComparison.OrdinalIgnoreCase) && k["ShiftID"].ToString() == shift).FirstOrDefault();
                        mainData.ChecklistID = firstRow["CheckItem"].ToString();
                        cycleTimeDisplay += firstRow["ShiftName"].ToString() + "(By " + Util.GetDateTime(firstRow["ShiftEnd"].ToString()).ToString("HH:mm") + ") <br/>";

                        mainData.Cycle = firstRow["Cycle"].ToString();
                        mainData.SortOrder = firstRow["SortOrder"].ToString();
                        mainData.CheckpointType = firstRow["ChecklistType"].ToString();
                        mainData.ContentVisibility = true;
                        mainData.Shift = shift;
                        mainData.ShiftName = firstRow["ShiftName"].ToString();
                        if (i == 0)
                        {
                            headerData.HeaderVisibility = true;
                        }
                        if (checkpointC == 0)
                        {
                            mainData.RowSpan = distShift.Count.ToString();
                            checkpointC++;

                            mainData.SlNo = slNoCount.ToString();
                            slNoCount++;
                        }
                        else
                        {
                            mainData.RowDisplay = "none";
                        }

                        List<FiveSChecksheetShiftEntity> valueList = new List<FiveSChecksheetShiftEntity>();
                        List<FiveSChecksheetShiftEntity> valueListHeader = new List<FiveSChecksheetShiftEntity>();
                        foreach (DateTime date in distDate)
                        {
                            FiveSChecksheetShiftEntity valueData = new FiveSChecksheetShiftEntity();
                            DataRow valueRow = null;

                            valueRow = dt.AsEnumerable().Where(k => k.Field<DateTime>("Date") == date && k["ShiftID"].ToString() == shift && k["CheckItem"].ToString() == checkPoint).FirstOrDefault();

                            if (date.Date == DateTime.Now.Date)
                            {
                                valueData.ControlEnabled = true;
                            }
                            else
                            {
                                valueData.ControlEnabled = false;
                            }
                            valueData.ContentVisibility = true;
                            valueData.CheckpointType = mainData.CheckpointType;
                            valueData.Date = date.ToString("dd-MM-yyyy");
                            if (valueRow != null)
                            {
                                valueData.ActualValue = valueRow["Value"].ToString();
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
                            }
                            valueList.Add(valueData);

                            if (i == 0)
                            {
                                FiveSChecksheetShiftEntity headerValueData = new FiveSChecksheetShiftEntity();
                                headerValueData.HeaderVisibility = true;
                                headerValueData.Day = date.Day.ToString();
                                valueListHeader.Add(headerValueData);
                            }
                        }

                        mainData.ValueList = valueList;

                        if (i == 0)
                        {
                            headerData.ValueList = valueListHeader;
                            list.Add(headerData);
                        }
                        list.Add(mainData);
                        i++;
                    }
                    list[cycleTimeListDataUpdateIndex == 0 ? 1 : cycleTimeListDataUpdateIndex].CycleTimeDisplay = cycleTimeDisplay;
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
                    else if (checkpointType == "OK-NG"||checkpointType=="Yes-No")
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
                        continue; //skip 
                    }

                    FiveSChecksheetTransEntity data = new FiveSChecksheetTransEntity();
                    data.MachineID = ddlMachine.SelectedValue;
                    data.ChecklistID = (item.FindControl("lblChecklistID") as Label).Text;
                    data.Cycle = (item.FindControl("hdnCycle") as HiddenField).Value;
                    data.Shift = (item.FindControl("hdnShiftID") as HiddenField).Value;
                    data.SortOrder = (item.FindControl("hdnSortOrder") as HiddenField).Value;
                    data.CheckpointType = (item.FindControl("hdnChecklistType") as HiddenField).Value;


                    ListView lvActualValue = item.FindControl("lvCheckpointValue") as ListView;
                    foreach (ListViewItem valueItem in lvActualValue.Items)
                    {
                        //string day = (valueItem.FindControl("hdnDay") as HiddenField).Value;

                        if (HelperClassGeneric.getCheckboxValue((valueItem.FindControl("hdnControlEnabled") as HiddenField).Value))
                        {
                            DateTime date = Util.GetDateTime((valueItem.FindControl("hdnDate") as HiddenField).Value);
                            data.Year = date.Year.ToString();
                            data.Month = date.Month.ToString();

                            data.Date = date.ToString("yyyy-MM-dd");
                            data.InspectedBy = Session["UserName"].ToString();
                            if (data.CheckpointType.Equals("Text", StringComparison.OrdinalIgnoreCase))
                            {
                                data.ActualValue = (valueItem.FindControl("txtValue") as TextBox).Text;
                            }
                            else if (data.CheckpointType.Equals("OK-NG", StringComparison.OrdinalIgnoreCase)||data.CheckpointType.Equals("Yes-No",StringComparison.OrdinalIgnoreCase))
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
                                    result = DensoDBAccess.insertUpdateFiveSCheckpointTransactionEntity(data);
                                }
                                else if (prevValue != "" && ((valueItem.FindControl("hdnActualValue") as HiddenField).Value) != data.ActualValue)
                                {
                                    result = DensoDBAccess.insertUpdateFiveSCheckpointTransactionEntity(data);
                                }
                            }
                            else if (data.CheckpointType == "Checkbox")
                            {
                                if (prevValue == "" && data.ActualValue != "False")
                                {
                                    result = DensoDBAccess.insertUpdateFiveSCheckpointTransactionEntity(data);
                                }
                                else if (prevValue != "" && ((valueItem.FindControl("hdnActualValue") as HiddenField).Value) != data.ActualValue)
                                {
                                    result = DensoDBAccess.insertUpdateFiveSCheckpointTransactionEntity(data);
                                }
                            }
                            else
                            {
                                if (((valueItem.FindControl("hdnActualValue") as HiddenField).Value) != data.ActualValue)
                                {
                                    result = DensoDBAccess.insertUpdateFiveSCheckpointTransactionEntity(data);
                                }
                            }
                            if (result == "Updated" || result == "Inserted")
                            {
                                updateCount++;
                            }
                        }
                        else
                        {
                            i++;
                            continue;
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