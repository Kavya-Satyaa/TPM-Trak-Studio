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
    public partial class PokayOkeTransactionDenso : System.Web.UI.Page
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
                DataTable dt = DensoDBAccess.getPokayOkeTransactionDetails(ddlMachine.SelectedValue, txtYear.Text, ddlMonth.SelectedValue, "TransactionView");
                int currentWeekNumber = DataBaseAccess.getCurrentWeekNumber();
                List<PokeyOkeTransEntity> list = new List<PokeyOkeTransEntity>();
                var distMachine = dt.AsEnumerable().Select(k => k["MachineID"].ToString()).Distinct().ToList();
                var distYear = dt.AsEnumerable().Select(k => k["Year"].ToString()).Distinct().ToList();
                var distWeekNo = dt.AsEnumerable().Select(k => k["WeekNo"].ToString()).Distinct().ToList();

                PokeyOkeTransEntity headerData = new PokeyOkeTransEntity();
                for (int machineC = 0; machineC < distMachine.Count; machineC++)
                {

                    DataTable dtMachines = dt.AsEnumerable().Where(k => k["MachineID"].ToString().Equals(distMachine[machineC], StringComparison.OrdinalIgnoreCase)).CopyToDataTable();
                    //mainData.RowSpan = dtMachines.Rows.Count.ToString();
                    var distChekpoint = dtMachines.AsEnumerable().Select(k => k["Item"].ToString()).Distinct().ToList();
                    PokeyOkeTransEntity mainData = new PokeyOkeTransEntity();

                    int i = 0;
                    foreach (string checkpoint in distChekpoint)
                    {
                        int headerRowInsertFlag = 0;
                        var firstRow = dtMachines.AsEnumerable().Where(k => k["Item"].ToString().Equals(checkpoint, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        mainData = new PokeyOkeTransEntity();
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
                            var dataValueRows = dtMachines.AsEnumerable().Where(k => k["Item"].ToString().Equals(checkpoint, StringComparison.OrdinalIgnoreCase) && k["WeekNo"].ToString() == weekNo).FirstOrDefault();
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
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    string checkpointType = (e.Item.FindControl("hdnCheckpointType") as HiddenField).Value;
                    string actualValue = (e.Item.FindControl("hdnActualValue") as HiddenField).Value;
                    var dd = e.Item.FindControl("ddlValue") as DropDownList;
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
                        //else if (actualValue == "")
                        //{
                        //    actualValue = "None";
                        //}
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

                    PokeyOkeTransEntity data = new PokeyOkeTransEntity();
                    data.MachineID = ddlMachine.SelectedValue;
                    data.PokayOkeItem = (item.FindControl("lblPokayOkeItem") as Label).Text;
                    data.Function = (item.FindControl("lblFunction") as Label).Text;
                    data.CheckMethod = (item.FindControl("lblCheckMethod") as Label).Text;
                    data.CheckInterval = (item.FindControl("lblCheckInterval") as Label).Text;
                    data.RevID = (item.FindControl("hdnRevID") as HiddenField).Value;
                    data.RevNo = (item.FindControl("hdnRevNo") as HiddenField).Value;
                    data.RevDate = (item.FindControl("hdnRevDate") as HiddenField).Value;
                    data.RevisedBy = (item.FindControl("hdnRevisedBy") as HiddenField).Value;
                    data.SortOrder = (item.FindControl("hdnSortOrder") as HiddenField).Value;
                    data.CheckpointType = (item.FindControl("hdnCheckpointType") as HiddenField).Value;
                    data.Year = txtYear.Text;
                    data.Month = ddlMonth.Text;
                    ListView lvWeek = item.FindControl("lvChecksheetWeek") as ListView;
                    foreach (ListViewItem weekItem in lvWeek.Items)
                    {
                        if (HelperClassGeneric.getCheckboxValue((weekItem.FindControl("hdnControlEnabled") as HiddenField).Value))
                        {
                            data.WeekNo = (weekItem.FindControl("hdnWeekNo") as HiddenField).Value;
                            data.UpdatedBy = Session["UserName"].ToString();
                            if (data.CheckpointType.Equals("Text", StringComparison.OrdinalIgnoreCase))
                            {
                                data.ActualValue = (weekItem.FindControl("txtValue") as TextBox).Text;
                            }
                            else if (data.CheckpointType.Equals("OK-NG", StringComparison.OrdinalIgnoreCase))
                            {
                                data.ActualValue = (weekItem.FindControl("ddlValue") as DropDownList).SelectedValue;
                            }
                            else if (data.CheckpointType.Equals("Checkbox", StringComparison.OrdinalIgnoreCase))
                            {
                                data.ActualValue = (weekItem.FindControl("chkValue") as CheckBox).Checked.ToString();
                            }
                            else if (data.CheckpointType.Equals("Image", StringComparison.OrdinalIgnoreCase))
                            {
                                data.FileInBase64 = (weekItem.FindControl("hdnFileInBase64") as HiddenField).Value;
                                if (data.FileInBase64 != "")
                                {
                                    byte[] drawinginbytes = System.Convert.FromBase64String(data.FileInBase64.Substring(data.FileInBase64.LastIndexOf(',') + 1));
                                    data.FileInByte = drawinginbytes;
                                    data.FileName = (weekItem.FindControl("hdnFileName") as HiddenField).Value;
                                }
                            }
                            string result = ""; string prevValue = (weekItem.FindControl("hdnActualValue") as HiddenField).Value;
                            if (data.CheckpointType.Equals("OK-NG", StringComparison.OrdinalIgnoreCase) || data.CheckpointType.Equals("Yes-No", StringComparison.OrdinalIgnoreCase))
                            {
                                if (prevValue == "" && data.ActualValue != "None")
                                {
                                    result = DensoDBAccess.insertUpdatePokayOkeTransactionDetails(data);
                                }
                                else if (prevValue != "" && ((weekItem.FindControl("hdnActualValue") as HiddenField).Value) != data.ActualValue)
                                {
                                    result = DensoDBAccess.insertUpdatePokayOkeTransactionDetails(data);
                                }
                            }
                            else if (data.CheckpointType == "Checkbox")
                            {
                                if (prevValue == "" && data.ActualValue != "False")
                                {
                                    result = DensoDBAccess.insertUpdatePokayOkeTransactionDetails(data);
                                }
                                else if (prevValue != "" && ((weekItem.FindControl("hdnActualValue") as HiddenField).Value) != data.ActualValue)
                                {
                                    result = DensoDBAccess.insertUpdatePokayOkeTransactionDetails(data);
                                }
                            }
                            else
                            {
                                if (((weekItem.FindControl("hdnActualValue") as HiddenField).Value) != data.ActualValue)
                                {
                                    result = DensoDBAccess.insertUpdatePokayOkeTransactionDetails(data);
                                }
                            }
                            if (result == "Updated" || result == "Inserted")
                            {
                                updateCount++;
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