using Elmah;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik.DataBaseAccess;
using Web_TPMTrakDashboard.Advik.Models;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Advik
{
    public partial class PMCheckListMaster : System.Web.UI.Page
    {
        List<PMChecklistMasterEntity> masterDatas = new List<PMChecklistMasterEntity>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnCancel.Enabled = false;
                BindDropdowns();
                btnView_Click(null, EventArgs.Empty);
            }
        }

        private void BindDropdowns()
        {
            ddlPlants.DataSource = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllPlants();
            ddlPlants.DataBind();
            string plant = ddlPlants.SelectedValue == null ? "" : ddlPlants.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlants.SelectedItem.ToString();
            List<string> GetCell = AdvikDatabaseAccess.GetCell(plant);
            ddlGroup.DataSource = GetCell;
            ddlGroup.DataBind();
            string Cell = ddlGroup.SelectedValue == null ? "" : ddlGroup.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlGroup.SelectedItem.ToString();
            ddlMachines.DataSource = AdvikDatabaseAccess.GetMachinesbyPlantCell(plant, Cell);
            ddlMachines.DataBind();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindGvChecklistMaster();
        }

        private void BindGvChecklistMaster()
        {
            string selectedMachine = ddlMachines.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? string.Empty : ddlMachines.SelectedValue;
            string jhType = ddlJHType.SelectedValue;
            masterDatas = AdvikDatabaseAccess.GetChecklistMasterDatas(selectedMachine, jhType);
            if (masterDatas != null && masterDatas.Count > 0)
            {
                gvChecklistMaster.DataSource = masterDatas;
                gvChecklistMaster.DataBind();
            }
            else
            {
                PMChecklistMasterEntity masterData = new PMChecklistMasterEntity();
                masterDatas.Add(masterData);
                gvChecklistMaster.DataSource = masterDatas;
                gvChecklistMaster.DataBind();
                gvChecklistMaster.Rows[0].Visible = false;
            }
            var footerRow = gvChecklistMaster.FooterRow;
            footerRow.Enabled = false;
        }

        protected void ddlPlants_SelectedIndexChanged(object sender, EventArgs e)
        {
            string plant = ddlPlants.SelectedValue == null ? "" : ddlPlants.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlants.SelectedItem.ToString();
            List<string> GetCell = AdvikDatabaseAccess.GetCell(plant);
            ddlGroup.DataSource = GetCell;
            ddlGroup.DataBind();
            string Cell = ddlGroup.SelectedValue == null ? "" : ddlGroup.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlGroup.SelectedItem.ToString();
            ddlMachines.DataSource = AdvikDatabaseAccess.GetMachinesbyPlantCell(plant, Cell);
            ddlMachines.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnAdd.Text.Equals("New", StringComparison.OrdinalIgnoreCase))
                {
                    btnAdd.Text = "Add";
                    btnSave.Enabled = false;
                    btnCancel.Enabled = true;
                    var footerRow = gvChecklistMaster.FooterRow;
                    footerRow.Enabled = true;
                }
                else
                {
                    btnAdd.Text = "New";
                    btnCancel.Enabled = false;
                    btnSave.Enabled = true;

                    string machineId = ddlMachines.SelectedValue;
                    GridViewRow gridViewRow = gvChecklistMaster.FooterRow;
                    PMChecklistMasterEntity data = new PMChecklistMasterEntity();
                    data.ChecklistID = (gridViewRow.FindControl("txtChecklistID") as TextBox).Text;
                    //string checklistItem = (gridViewRow.FindControl("txtChecklistItem") as TextBox).Text;
                    data.McArea = (gridViewRow.FindControl("txtMcAreaNew") as TextBox).Text;
                    data.Location = (gridViewRow.FindControl("txtLocationNew") as TextBox).Text;
                    data.StdCondition = (gridViewRow.FindControl("txtConditionNew") as TextBox).Text;
                    data.CheckingMethod = (gridViewRow.FindControl("txtCheckingMethodNew") as TextBox).Text;
                    //int noOfCycles = Convert.ToInt32((gridViewRow.FindControl("txtNoOfCycles") as TextBox).Text);
                    data.IsEnabled = (gridViewRow.FindControl("chkIsEnabled") as CheckBox).Checked;
                    data.JHType = (gridViewRow.FindControl("ddlFooterJHType") as DropDownList).SelectedValue;
                    int sortOrder = Convert.ToInt32((gridViewRow.FindControl("txtSortOrder") as TextBox).Text);

                    AdvikDatabaseAccess.SaveChecklistMasterData(machineId, data, sortOrder);
                    BindGvChecklistMaster();

                    lblMessages.ForeColor = System.Drawing.Color.Green;
                    lblMessages.Text = "Record Saved Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            TextBox txtGrid = new TextBox();
            Label lblGrid = new Label();
            CheckBox chkGrid = new CheckBox();
            try
            {
                if (btnSave.Text == "Edit")
                {
                    btnSave.Text = "Update";
                    btnCancel.Enabled = true;
                    btnAdd.Enabled = false;
                    foreach (GridViewRow gridViewRow in gvChecklistMaster.Rows)
                    {
                        if (gridViewRow.RowType != DataControlRowType.Header && gridViewRow.RowType != DataControlRowType.Footer)
                        {
                            //lblGrid = gridViewRow.FindControl("lblIChecklistItem") as Label;
                            //lblGrid.Visible = false;
                            lblGrid = gridViewRow.FindControl("lblMcArea") as Label;
                            lblGrid.Visible = false;
                            lblGrid = gridViewRow.FindControl("lblLocation") as Label;
                            lblGrid.Visible = false;
                            lblGrid = gridViewRow.FindControl("lblCondition") as Label;
                            lblGrid.Visible = false;
                            lblGrid = gridViewRow.FindControl("lblCheckingMethod") as Label;
                            lblGrid.Visible = false;
                            //lblGrid = gridViewRow.FindControl("lblINoOfCycles") as Label;
                            //lblGrid.Visible = false;
                            lblGrid = gridViewRow.FindControl("lblSortOrder") as Label;
                            lblGrid.Visible = false;

                            //txtGrid = gridViewRow.FindControl("txtIChecklistItem") as TextBox;
                            //txtGrid.Visible = true;
                            txtGrid = gridViewRow.FindControl("txtMcAreaEdit") as TextBox;
                            txtGrid.Visible = true;
                            txtGrid = gridViewRow.FindControl("txtLocationEdit") as TextBox;
                            txtGrid.Visible = true;
                            txtGrid = gridViewRow.FindControl("txtConditionEdit") as TextBox;
                            txtGrid.Visible = true;
                            txtGrid = gridViewRow.FindControl("txtCheckingMethodEdit") as TextBox;
                            txtGrid.Visible = true;
                            //txtGrid = gridViewRow.FindControl("txtINoOfCycles") as TextBox;
                            //txtGrid.Visible = true;
                            txtGrid = gridViewRow.FindControl("txtISortOrder") as TextBox;
                            txtGrid.Visible = true;

                            chkGrid = gridViewRow.FindControl("chkEnbl") as CheckBox;
                            chkGrid.Enabled = true;
                        }
                    }
                }
                else
                {
                    int updatedRow = 0;
                    btnSave.Text = "Edit";
                    btnCancel.Enabled = false;
                    btnAdd.Enabled = true;
                    string machineId = ddlMachines.SelectedValue;

                    foreach (GridViewRow gridViewRow in gvChecklistMaster.Rows)
                    {
                        if (gridViewRow.RowType != DataControlRowType.Header && gridViewRow.RowType != DataControlRowType.Footer)
                        {
                            string isUpdated = (gridViewRow.FindControl("hdfUpdate") as HiddenField).Value;
                            if (isUpdated.Equals("Update", StringComparison.OrdinalIgnoreCase))
                            {
                                PMChecklistMasterEntity data = new PMChecklistMasterEntity();
                                data.ChecklistID = (gridViewRow.FindControl("lblIChecklistId") as Label).Text;
                                //string checklistItem = (gridViewRow.FindControl("txtIChecklistItem") as TextBox).Text;
                                data.McArea = (gridViewRow.FindControl("txtMcAreaEdit") as TextBox).Text;
                                data.Location = (gridViewRow.FindControl("txtLocationEdit") as TextBox).Text;
                                data.StdCondition = (gridViewRow.FindControl("txtConditionEdit") as TextBox).Text;
                                data.CheckingMethod = (gridViewRow.FindControl("txtCheckingMethodEdit") as TextBox).Text;
                                //int noOfCycles = Convert.ToInt32((gridViewRow.FindControl("txtINoOfCycles") as TextBox).Text);
                                data.IsEnabled = (gridViewRow.FindControl("chkEnbl") as CheckBox).Checked;
                                data.JHType = (gridViewRow.FindControl("lblJHType") as Label).Text;

                                if (data.ChecklistID.Trim() == "")
                                {
                                    lblMessages.ForeColor = System.Drawing.Color.Red;
                                    lblMessages.Text = "Please enter Checklist ID.";
                                    btnSave.Text = "Update";
                                    btnCancel.Enabled = true;
                                    btnAdd.Enabled = false;
                                    return;
                                }
                                if (data.McArea.Trim() == "")
                                {
                                    lblMessages.ForeColor = System.Drawing.Color.Red;
                                    lblMessages.Text = "Please enter Mc Area.";
                                    btnSave.Text = "Update";
                                    btnCancel.Enabled = true;
                                    btnAdd.Enabled = false;
                                    return;
                                }
                                if (data.Location.Trim() == "")
                                {
                                    lblMessages.ForeColor = System.Drawing.Color.Red;
                                    lblMessages.Text = "Please enter Location.";
                                    btnSave.Text = "Update";
                                    btnCancel.Enabled = true;
                                    btnAdd.Enabled = false;
                                    return;
                                }
                                if (data.StdCondition.Trim() == "")
                                {
                                    lblMessages.ForeColor = System.Drawing.Color.Red;
                                    lblMessages.Text = "Please enter Std Condition.";
                                    btnSave.Text = "Update";
                                    btnCancel.Enabled = true;
                                    btnAdd.Enabled = false;
                                    return;
                                }
                                if (data.CheckingMethod.Trim() == "")
                                {
                                    lblMessages.ForeColor = System.Drawing.Color.Red;
                                    lblMessages.Text = "Please enter Checking Method.";
                                    btnSave.Text = "Update";
                                    btnCancel.Enabled = true;
                                    btnAdd.Enabled = false;
                                    return;
                                }
                                if ((gridViewRow.FindControl("txtISortOrder") as TextBox).Text.Trim() == "")
                                {
                                    lblMessages.ForeColor = System.Drawing.Color.Red;
                                    lblMessages.Text = "Please enter Sort Order.";
                                    btnSave.Text = "Update";
                                    btnCancel.Enabled = true;
                                    btnAdd.Enabled = false;
                                    return;
                                }

                                int sortOrder = Convert.ToInt32((gridViewRow.FindControl("txtISortOrder") as TextBox).Text);

                                AdvikDatabaseAccess.SaveChecklistMasterData(machineId, data, sortOrder);
                                updatedRow++;
                            }
                        }
                        //lblGrid = gridViewRow.FindControl("lblIChecklistItem") as Label;
                        //lblGrid.Visible = true;
                        lblGrid = gridViewRow.FindControl("lblMcArea") as Label;
                        lblGrid.Visible = true;
                        lblGrid = gridViewRow.FindControl("lblLocation") as Label;
                        lblGrid.Visible = true;
                        lblGrid = gridViewRow.FindControl("lblCondition") as Label;
                        lblGrid.Visible = true;
                        lblGrid = gridViewRow.FindControl("lblCheckingMethod") as Label;
                        lblGrid.Visible = true;
                        //lblGrid = gridViewRow.FindControl("lblINoOfCycles") as Label;
                        //lblGrid.Visible = true;
                        lblGrid = gridViewRow.FindControl("lblSortOrder") as Label;
                        lblGrid.Visible = true;

                        //txtGrid = gridViewRow.FindControl("txtIChecklistItem") as TextBox;
                        //txtGrid.Visible = false;
                        txtGrid = gridViewRow.FindControl("txtMcAreaEdit") as TextBox;
                        txtGrid.Visible = false;
                        txtGrid = gridViewRow.FindControl("txtLocationEdit") as TextBox;
                        txtGrid.Visible = false;
                        txtGrid = gridViewRow.FindControl("txtConditionEdit") as TextBox;
                        txtGrid.Visible = false;
                        txtGrid = gridViewRow.FindControl("txtCheckingMethodEdit") as TextBox;
                        txtGrid.Visible = false;
                        //txtGrid = gridViewRow.FindControl("txtINoOfCycles") as TextBox;
                        //txtGrid.Visible = false;
                        txtGrid = gridViewRow.FindControl("txtISortOrder") as TextBox;
                        txtGrid.Visible = false;

                        chkGrid = gridViewRow.FindControl("chkEnbl") as CheckBox;
                        chkGrid.Enabled = false;
                    }
                    BindGvChecklistMaster();

                    lblMessages.ForeColor = System.Drawing.Color.Green;
                    lblMessages.Text = updatedRow + " Row(s) Updated Successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            string fileName = string.Empty;
            DataTable dtChecklistMaster = new DataTable();
            try
            {
                if (fuToImport.HasFile)
                {
                    fileName = fuToImport.FileName;
                    if (Path.GetExtension(fileName) != ".xlsx")
                    {
                        Logger.WriteErrorLog("No valid excel(.xlsx) File selected to Import");
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = "Please choose a valid excel(.xlsx) file.";
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    }
                    else
                    {
                        if (!Directory.Exists(Server.MapPath("ImportedFiles")))
                        {
                            Directory.CreateDirectory(Server.MapPath("ImportedFiles"));
                        }
                        else
                        {
                            int rowInserted; bool isOutOfBoundary; bool isCheckIdOrSortString;
                            string savedFileName = Server.MapPath("ImportedFiles//" + fileName);
                            fuToImport.SaveAs(savedFileName);
                            dtChecklistMaster = GetChecklistMasterDataFromFile(savedFileName, out rowInserted, out isOutOfBoundary, out isCheckIdOrSortString);
                            if (dtChecklistMaster != null && dtChecklistMaster.Rows.Count > 0)
                            {
                                if (!IsEmptyCellAvailable(dtChecklistMaster))
                                {
                                    AdvikDatabaseAccess.LoadChecklistMasterDataToDB(dtChecklistMaster);

                                    BindGvChecklistMaster();
                                    Logger.WriteErrorLog(fileName + "imported successfully.");
                                    lblMessages.ForeColor = System.Drawing.Color.Green;
                                    lblMessages.Text = rowInserted + " record(s) from " + fileName + " imported successfully.";
                                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                                }
                                else
                                {
                                    Logger.WriteErrorLog("Import failed,Empty Cell(s) found in excel file");
                                    lblMessages.ForeColor = System.Drawing.Color.Red;
                                    lblMessages.Text = "Import failed,Empty Cell(s) found in excel file";
                                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                                }
                            }
                            else if (isCheckIdOrSortString)
                            {
                                Logger.WriteErrorLog("Import failed. Checklist ID should be Interger .");
                                lblMessages.ForeColor = System.Drawing.Color.Red;
                                lblMessages.Text = "Import failed. Checklist ID should be Interger .";
                                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                            }
                            else if (isOutOfBoundary)
                            {
                                Logger.WriteErrorLog("Import failed. Mc Area or Location or Std Condition or Checking Method length can not be greater than 14 characters .");
                                lblMessages.ForeColor = System.Drawing.Color.Red;
                                lblMessages.Text = "Import failed. Mc Area or Location or Std Condition or Checking Method length can not be greater than 14 characters .";
                                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                            }
                            else
                            {
                                Logger.WriteErrorLog("Import failed,No data in excel file");
                                lblMessages.ForeColor = System.Drawing.Color.Red;
                                lblMessages.Text = "Import failed,No data/Data already present in excel file";
                                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                            }
                        }
                    }
                }
                else
                {
                    Logger.WriteErrorLog("No File selected to Import");
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "No File selected to Import";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }

        private bool IsEmptyCellAvailable(DataTable dtChecklistMaster)
        {
            bool isAvailable = false;
            foreach (DataRow row in dtChecklistMaster.Rows)
            {
                string machineId = row.Field<string>("MachineID");
                string checklistId = row.Field<string>("ChecklistID");
                //int noOfCycles = row.Field<int>("NoOfCycles");
                string jhType = row.Field<string>("JHType");
                //string checklistItem = row.Field<string>("ChecklistItem");
                string mcArea = row.Field<string>("McArea");
                string location = row.Field<string>("Location");
                string stdCondition = row.Field<string>("StdCondition");
                string checkingMethod = row.Field<string>("CheckingMethod");
                int SortOrder = row.Field<int>("SortOrder");
                if (string.IsNullOrEmpty(machineId))
                {
                    isAvailable = true;
                    break;
                }
                if (string.IsNullOrEmpty(checklistId))
                {
                    isAvailable = true;
                    break;
                }
                //if (string.IsNullOrEmpty(checklistItem))
                //{
                //    isAvailable = true;
                //    break;
                //}
                if (string.IsNullOrEmpty(mcArea))
                {
                    isAvailable = true;
                    break;
                }
                if (string.IsNullOrEmpty(location))
                {
                    isAvailable = true;
                    break;
                }
                if (string.IsNullOrEmpty(stdCondition))
                {
                    isAvailable = true;
                    break;
                }
                if (string.IsNullOrEmpty(checkingMethod))
                {
                    isAvailable = true;
                    break;
                }
                if (string.IsNullOrEmpty(jhType))
                {
                    isAvailable = true;
                    break;
                }
                if (string.IsNullOrEmpty(SortOrder.ToString()))
                {
                    isAvailable = true;
                    break;
                }
                //if (noOfCycles == int.MinValue)
                //{
                //    isAvailable = true;
                //    break;
                //}
            }
            return isAvailable;
        }

        private DataTable GetChecklistMasterDataFromFile(string fileName, out int rowInserted, out bool isOutOfBoundary, out bool isCheckIDString)
        {
            isOutOfBoundary = false;
            isCheckIDString = false;
            rowInserted = 0;
            DataTable dtChecklistMaster = new DataTable();
            using (var pck = new ExcelPackage())
            {
                using (var stream = File.OpenRead(fileName))
                {
                    pck.Load(stream);
                }
                var workBook = pck.Workbook;
                if (workBook != null)
                {
                    try
                    {
                        var wsChecklistMaster = workBook.Worksheets[1];
                        dtChecklistMaster.Columns.Add("MachineID", typeof(string));
                        dtChecklistMaster.Columns.Add("ChecklistID", typeof(string));
                        //dtChecklistMaster.Columns.Add("ChecklistItem", typeof(string));
                        dtChecklistMaster.Columns.Add("McArea", typeof(string));
                        dtChecklistMaster.Columns.Add("Location", typeof(string));
                        dtChecklistMaster.Columns.Add("StdCondition", typeof(string));
                        dtChecklistMaster.Columns.Add("CheckingMethod", typeof(string));
                        //dtChecklistMaster.Columns.Add("NoOfCycles", typeof(int));
                        dtChecklistMaster.Columns.Add("IsEnabled", typeof(bool));
                        dtChecklistMaster.Columns.Add("SortOrder", typeof(int));
                        dtChecklistMaster.Columns.Add("JHType", typeof(string));

                        int startRow = 3;
                        int lastRowSortOrder = 0;
                        int lastRow = GetLastUsedRow(wsChecklistMaster);
                        for (var rowNum = startRow; rowNum <= lastRow; rowNum++)
                        {
                            string machineID = wsChecklistMaster.Cells[rowNum, 1].Value == null ? "" : wsChecklistMaster.Cells[rowNum, 1].Value.ToString();
                            string checklistID = wsChecklistMaster.Cells[rowNum, 2].Value == null ? "" : wsChecklistMaster.Cells[rowNum, 2].Value.ToString();
                            //string checklistItem = wsChecklistMaster.Cells[rowNum, 3].Value == null ? "" : wsChecklistMaster.Cells[rowNum, 3].Value.ToString();
                            string mcarea = wsChecklistMaster.Cells[rowNum, 3].Value == null ? "" : wsChecklistMaster.Cells[rowNum, 3].Value.ToString();
                            string Location = wsChecklistMaster.Cells[rowNum, 4].Value == null ? "" : wsChecklistMaster.Cells[rowNum, 4].Value.ToString();
                            string StdCondition = wsChecklistMaster.Cells[rowNum, 5].Value == null ? "" : wsChecklistMaster.Cells[rowNum, 5].Value.ToString();
                            string CheckingMethod = wsChecklistMaster.Cells[rowNum, 6].Value == null ? "" : wsChecklistMaster.Cells[rowNum, 6].Value.ToString();
                            //int NoOfCycle = wsChecklistMaster.Cells[rowNum, 4].Value == null ? int.MinValue : Convert.ToInt32(wsChecklistMaster.Cells[rowNum, 4].Value.ToString());
                            int sortOrder = wsChecklistMaster.Cells[rowNum, 7].Value == null ? lastRowSortOrder + 1 : Convert.ToInt32(wsChecklistMaster.Cells[rowNum, 7].Value.ToString());
                            string jhType = wsChecklistMaster.Cells[rowNum, 8].Value == null ? "" : wsChecklistMaster.Cells[rowNum, 8].Value.ToString();
                            if (!AdvikDatabaseAccess.CheckForRowPresence(machineID, checklistID, jhType))
                                continue;

                            var tblRow = dtChecklistMaster.NewRow();
                            int chkid;
                            if (!int.TryParse(checklistID.Trim(), out chkid))
                            {
                                isCheckIDString = true;
                                return null;
                            }
                            if (mcarea.Length > 14 || Location.Length > 14 || StdCondition.Length > 14 || CheckingMethod.Length > 14)
                            {
                                isOutOfBoundary = true;
                                return null;
                            }


                            tblRow[0] = machineID;
                            tblRow[1] = checklistID;
                            //tblRow[2] = checklistItem.Length > 20 ? checklistItem.Substring(0, 20) : checklistItem;
                            tblRow[2] = mcarea;
                            tblRow[3] = Location;
                            tblRow[4] = StdCondition;
                            tblRow[5] = CheckingMethod;
                            //tblRow[3] = NoOfCycle;
                            tblRow[6] = true;
                            tblRow[7] = sortOrder;
                            tblRow[8] = jhType;
                            dtChecklistMaster.Rows.Add(tblRow);
                            rowInserted++;
                            lastRowSortOrder = Convert.ToInt32(tblRow[7].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteDebugLog("Error while importing Checklist Master excel file : " + ex.Message);
                    }
                }
            }
            return dtChecklistMaster;
        }
        private static int GetLastUsedRow(ExcelWorksheet sheet)
        {
            var row = sheet.Dimension.End.Row;
            while (row >= 1)
            {
                var range = sheet.Cells[row, 1, row, sheet.Dimension.End.Column];
                if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                {
                    break;
                }
                row--;
            }
            return row;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool IsRowPresent(string machineID, string checklistID, string jhType)
        {
            bool isNotpresent = false;
            try
            {
                isNotpresent = AdvikDatabaseAccess.CheckForRowPresence(machineID, checklistID, jhType);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.Message);
            }
            return isNotpresent;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnAdd.Enabled)
            {
                btnAdd.Text = "New";
                btnCancel.Enabled = false;
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Text = "Edit";
                btnCancel.Enabled = false;
                btnAdd.Enabled = true;
                TextBox txtGrid = new TextBox();
                Label lblGrid = new Label();
                DropDownList ddlGrid = new DropDownList();
                CheckBox chkGrid = new CheckBox();
                foreach (GridViewRow gridViewRow in gvChecklistMaster.Rows)
                {
                    if (gridViewRow.RowType != DataControlRowType.Header && gridViewRow.RowType != DataControlRowType.Footer)
                    {
                        //lblGrid = gridViewRow.FindControl("lblIChecklistItem") as Label;
                        //lblGrid.Visible = true;
                        lblGrid = gridViewRow.FindControl("lblMcArea") as Label;
                        lblGrid.Visible = true;
                        lblGrid = gridViewRow.FindControl("lblLocation") as Label;
                        lblGrid.Visible = true;
                        lblGrid = gridViewRow.FindControl("lblCondition") as Label;
                        lblGrid.Visible = true;
                        lblGrid = gridViewRow.FindControl("lblCheckingMethod") as Label;
                        lblGrid.Visible = true;
                        //lblGrid = gridViewRow.FindControl("lblINoOfCycles") as Label;
                        //lblGrid.Visible = true;
                        lblGrid = gridViewRow.FindControl("lblSortOrder") as Label;
                        lblGrid.Visible = true;

                        //txtGrid = gridViewRow.FindControl("txtIChecklistItem") as TextBox;
                        //txtGrid.Visible = false;
                        txtGrid = gridViewRow.FindControl("txtMcAreaEdit") as TextBox;
                        txtGrid.Visible = false;
                        txtGrid = gridViewRow.FindControl("txtLocationEdit") as TextBox;
                        txtGrid.Visible = false;
                        txtGrid = gridViewRow.FindControl("txtConditionEdit") as TextBox;
                        txtGrid.Visible = false;
                        txtGrid = gridViewRow.FindControl("txtCheckingMethodEdit") as TextBox;
                        txtGrid.Visible = false;
                        //txtGrid = gridViewRow.FindControl("txtINoOfCycles") as TextBox;
                        //txtGrid.Visible = false;
                        txtGrid = gridViewRow.FindControl("txtISortOrder") as TextBox;
                        txtGrid.Visible = false;

                        chkGrid = gridViewRow.FindControl("chkEnbl") as CheckBox;
                        chkGrid.Enabled = false;
                    }
                }
            }
            BindGvChecklistMaster();
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string plant = ddlPlants.SelectedValue == null ? "" : ddlPlants.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlants.SelectedItem.ToString();
            string Cell = ddlGroup.SelectedValue == null ? "" : ddlGroup.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlGroup.SelectedItem.ToString();
            ddlMachines.DataSource = AdvikDatabaseAccess.GetMachinesbyPlantCell(plant, Cell);
            ddlMachines.DataBind();
        }
    }
}