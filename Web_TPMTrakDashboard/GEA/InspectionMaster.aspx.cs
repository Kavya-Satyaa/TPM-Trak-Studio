using Elmah;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;
using Web_TPMTrakDashboard.GEA.Models;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class InspectionMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                if (!IsPostBack)
                {
                    Session["InspectionCompOpnList"] = null;
                    SessionClear.ClearSession();
                    BindComponentIDs();
                    BindOperationIDs();
                    BindInsPlanNumbers();
                    lblIQCName.Text = " Latest IQC: " + GEADatabaseAccess.GetLatestIQC("");
                    LoadInspectionMasterData();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void BindComponentIDs()
        {
            try
            {
                //List<string> listComponentIds = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllComponents();
                //if (listComponentIds != null && listComponentIds.Count > 0)
                //{
                //    if (listComponentIds.Contains("All")) listComponentIds.Remove("All");
                //    ddlCompID.DataSource = listComponentIds;
                //    ddlCompID.DataBind();
                //    ddlCompID.SelectedIndex = 0;
                //}
                List<CompOpnEntity> listComponentIds = getCompOpnList();
                if (listComponentIds != null && listComponentIds.Count > 0)
                {
                    ddlCompID.DataSource = listComponentIds.Select(k => k.Component).Distinct().ToList();
                    ddlCompID.DataBind();
                    ddlCompID.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        private List<CompOpnEntity> getCompOpnList()
        {
            List<CompOpnEntity> list = new List<CompOpnEntity>();
            try
            {
                if (Session["InspectionCompOpnList"] == null)
                {
                    list = GEADatabaseAccess.getCompOpnForInspection();
                    Session["InspectionCompOpnList"] = list;
                }
                list = Session["InspectionCompOpnList"] as List<CompOpnEntity>;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return list;
        }
        private void BindOperationIDs()
        {
            try
            {
                //string compId = ddlCompID.SelectedItem != null ? ddlCompID.SelectedValue : "";
                //List<string> listOpeartions = Web_TPMTrakDashboard.Models.DataBaseAccess.GetOperation("", compId);
                //if (listOpeartions != null && listOpeartions.Count > 0)
                //{
                //    ddlOperationNum.DataSource = listOpeartions;
                //    ddlOperationNum.DataBind();
                //    ddlOperationNum.SelectedIndex = 0;
                //}
                List<CompOpnEntity> list = getCompOpnList();
                ddlOperationNum.DataSource = list.Where(k => k.Component == ddlCompID.SelectedValue).Select(k => k.Operation).Distinct().ToList();
                ddlOperationNum.DataBind();
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void BindInsPlanNumbers()
        {
            try
            {
                List<string> listInsPlanNumbers = GEADatabaseAccess.GetInsPlanNumbers(ddlCompID.SelectedValue.ToString(), ddlOperationNum.SelectedValue.ToString());
                if (listInsPlanNumbers != null && listInsPlanNumbers.Count > 0)
                {
                    ddlInsPlanNumber.DataSource = listInsPlanNumbers;
                    ddlInsPlanNumber.DataBind();
                    ddlInsPlanNumber.SelectedIndex = 0;
                }
                else
                {
                    ddlInsPlanNumber.DataSource = new List<string>();
                    ddlInsPlanNumber.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void LoadInspectionMasterData()
        {
            List<InspectionMasterEntity> InspectionMasterData = new List<InspectionMasterEntity>();
            try
            {
                string CompID = ddlCompID.SelectedItem != null ? ddlCompID.SelectedValue : "";
                string OpnID = ddlOperationNum.SelectedItem != null ? ddlOperationNum.SelectedValue : "";
                string InsPlan = ddlInsPlanNumber.SelectedItem != null ? ddlInsPlanNumber.SelectedValue : "";
                if (!string.IsNullOrEmpty(CompID) && !string.IsNullOrEmpty(OpnID))
                {
                    Session["CompID"] = CompID;
                    Session["OpnID"] = OpnID;
                    Session["InsPlan"] = InsPlan;
                    InspectionMasterData = GEADatabaseAccess.GetInspectionMasterData(CompID, OpnID, InsPlan);
                    if (InspectionMasterData != null && InspectionMasterData.Count > 0)
                    {
                        Session["InspectionMasterData"] = InspectionMasterData;
                        GridInspectionMaster.DataSource = InspectionMasterData;
                        GridInspectionMaster.DataBind();
                    }
                    else
                    {
                        Session["InspectionMasterData"] = null;
                        GridInspectionMaster.DataSource = new List<InspectionMasterEntity>();
                        GridInspectionMaster.DataBind();
                    }
                }
                else
                {
                    GridInspectionMaster.DataSource = new List<InspectionMasterEntity>();
                    GridInspectionMaster.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        protected void GridInspectionMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string Idd = (e.Row.Cells[1].FindControl("lblIDD") as Label).Text;
                    TextBox txtInsParamID = e.Row.Cells[2].FindControl("txtInsParamID") as TextBox;
                    if (txtInsParamID != null)
                    {
                        if (Idd.Equals("0"))
                            txtInsParamID.Enabled = true;
                        else
                            txtInsParamID.Enabled = false;
                    }
                    string dataTemplate = (e.Row.FindControl("hdnDataTemplate") as HiddenField).Value;
                    DropDownList ddl = (e.Row.FindControl("ddlDataTemplate") as DropDownList);
                    if (ddl != null)
                    {
                        if (ddl.Items.FindByValue(dataTemplate) != null)
                        {
                            ddl.SelectedValue = dataTemplate;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            LoadInspectionMasterData();
        }

        protected void btnImportData_Click(object sender, EventArgs e)
        {
            bool IsSaved = false;
            try
            {
                if (FileUploadInsMaster.HasFile)
                {
                    if (Path.GetExtension(FileUploadInsMaster.FileName) == ".xlsx")
                    {
                        lblMessages.Text = string.Empty;
                        DataTable dtInsMasterData = new DataTable();
                        ExcelPackage Excel = new ExcelPackage(FileUploadInsMaster.PostedFile.InputStream);
                        var worksheetInsMaster = Excel.Workbook.Worksheets.First();
                        if (worksheetInsMaster.Dimension.End.Column != 14)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Please select valid Template.')", true);
                            return;
                        }
                        foreach (var columnHeader in worksheetInsMaster.Cells[1, 1, 1, worksheetInsMaster.Dimension.End.Column])
                            dtInsMasterData.Columns.Add(columnHeader.Text.Trim());
                        for (int rowNum = 2; rowNum <= worksheetInsMaster.Dimension.End.Row; rowNum++)
                        {
                            var worksheetRow = worksheetInsMaster.Cells[rowNum, 1, rowNum, worksheetInsMaster.Dimension.End.Column];
                            if (worksheetRow != null)
                            {
                                DataRow dtScheduleDataRow = dtInsMasterData.Rows.Add();
                                foreach (var scheduleData in worksheetRow)
                                    dtScheduleDataRow[scheduleData.Start.Column - 1] = scheduleData.Text.Trim();
                            }
                        }
                        if (dtInsMasterData != null && dtInsMasterData.Rows.Count > 0)
                        {
                            dtInsMasterData.Columns[0].ColumnName = "ComponentID";
                            dtInsMasterData.Columns[1].ColumnName = "OperationNum";
                            dtInsMasterData.Columns[2].ColumnName = "InsPlanNumber";
                            dtInsMasterData.Columns[3].ColumnName = "InsParamID";
                            dtInsMasterData.Columns[4].ColumnName = "InsParamName";
                            dtInsMasterData.Columns[5].ColumnName = "LSL";
                            dtInsMasterData.Columns[6].ColumnName = "USL";
                            dtInsMasterData.Columns[7].ColumnName = "UOM";
                            dtInsMasterData.Columns[8].ColumnName = "DataTemplate";
                            dtInsMasterData.Columns[9].ColumnName = "AppliesToOperator";
                            dtInsMasterData.Columns[10].ColumnName = "MandatoryForOperator";
                            dtInsMasterData.Columns[11].ColumnName = "AppliesToQuality";
                            dtInsMasterData.Columns[12].ColumnName = "MandatoryForQuality";
                            dtInsMasterData.Columns[13].ColumnName = "IsEnabled";
                            //List<string> InspPlan = GEADatabaseAccess.GetInsPlanNumbers("");
                            //foreach (string insplanNo in InspPlan)
                            //{
                            //    dtInsMasterData.AsEnumerable().Where(x => x.Field<string>("InsPlanNumber").Equals(insplanNo)).ToList().ForEach(row => row.Delete());
                            //}
                            foreach (DataRow scheduleDataRow in dtInsMasterData.Rows)
                            {
                                if (string.IsNullOrEmpty(scheduleDataRow["LSL"].ToString()))
                                {
                                    scheduleDataRow["LSL"] = 0;
                                }
                                if (string.IsNullOrEmpty(scheduleDataRow["USL"].ToString()))
                                {
                                    scheduleDataRow["USL"] = 0;
                                }
                                if (!IsDataRowValid(scheduleDataRow))
                                {
                                    IsSaved = false;
                                    break;
                                }
                                IsSaved = GEADatabaseAccess.UpdateInsMasterData("", scheduleDataRow["ComponentID"].ToString(), scheduleDataRow["OperationNum"].ToString(), scheduleDataRow["InsPlanNumber"].ToString(), scheduleDataRow["InsParamID"].ToString(), scheduleDataRow["InsParamName"].ToString(), scheduleDataRow["LSL"].ToString(), scheduleDataRow["USL"].ToString(), scheduleDataRow["UOM"].ToString(), scheduleDataRow["DataTemplate"].ToString(), scheduleDataRow["AppliesToOperator"].ToString(), scheduleDataRow["MandatoryForOperator"].ToString(), scheduleDataRow["AppliesToQuality"].ToString(), scheduleDataRow["MandatoryForQuality"].ToString(), scheduleDataRow["IsEnabled"].ToString());
                                if (!IsSaved) break;
                            }
                            if (IsSaved)
                            {
                                lblMessages.ForeColor = Color.Green;
                                lblMessages.Text = "Data imported successfully.";
                            }
                            BindInsPlanNumbers();
                            LoadInspectionMasterData();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('No data found in excel file.')", true);
                            return;
                        }
                    }
                    else
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "Wrong file format. File to be imported must be a xlsx excel file.";
                    }
                }
                else
                {
                    lblMessages.ForeColor = Color.Red;
                    lblMessages.Text = "No file chosen for import.";
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            List<InspectionMasterEntity> InspectionMasterData = new List<InspectionMasterEntity>();
            try
            {
                string CompID = Session["CompID"] as string;
                string OpnID = Session["OpnID"] as string;
                string InsPlan = Session["InsPlan"] as string;
                if (Session["InspectionMasterData"] != null)
                    InspectionMasterData = Session["InspectionMasterData"] as List<InspectionMasterEntity>;
                else
                {
                    InspectionMasterData = GEADatabaseAccess.GetInspectionMasterData(CompID, OpnID, InsPlan);
                }
                if (btnNew.Text.Equals("New"))
                {
                    InspectionMasterData.Add(new InspectionMasterEntity { ID = 0, InsPlanNumber = InsPlan, InsParamID = "", InsParameter = "", LSL = "", USL = "", UOM = "", DataTemplate = "", AppliesToOperator = false, MandatoryForOperator = false, AppliesToQuality = false, MandatoryForQuality = false, IsEnabled = true });
                    GridInspectionMaster.DataSource = InspectionMasterData;
                    GridInspectionMaster.DataBind();
                    btnNew.Text = "Cancel";
                }
                if (btnNew.Text.Equals("Cancel"))
                {
                    GridInspectionMaster.DataSource = InspectionMasterData;
                    GridInspectionMaster.DataBind();
                    btnNew.Text = "New";
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnSave2_Click(object sender, EventArgs e)
        {
            bool IsSaved = false;
            try
            {
                if (GridInspectionMaster.Rows.Count > 0)
                {
                    foreach (GridViewRow row in GridInspectionMaster.Rows)
                    {
                        string hdfUpdate = ((HiddenField)row.FindControl("hdfUpdate")).Value;
                        if (hdfUpdate == "Update")
                        {
                            string Idd = (row.Cells[1].FindControl("lblIDD") as Label).Text;
                            if (ddlCompID.SelectedValue == null) return;
                            string CompID = ddlCompID.SelectedValue.ToString();
                            if (ddlOperationNum.SelectedValue == null) return;
                            string OpnID = ddlOperationNum.SelectedValue.ToString();
                            string InsPlan = txtinspectionPlanNumber.Text;
                            string InsParamID = (row.Cells[2].FindControl("txtInsParamID") as TextBox).Text;
                            string InsParameter = (row.Cells[3].FindControl("txtInsParameter") as TextBox).Text;
                            string LSL = (row.Cells[4].FindControl("txtLSL") as TextBox).Text;
                            string USL = (row.Cells[5].FindControl("txtUSL") as TextBox).Text;
                            string UOM = (row.Cells[6].FindControl("txtUOM") as TextBox).Text;
                            string DataTemplate = (row.Cells[7].FindControl("ddlDataTemplate") as DropDownList).SelectedValue;
                            string AppliesToOperator = (row.Cells[8].FindControl("chkAppliesToOperator") as CheckBox).Checked.ToString();
                            string MandatoryForOperator = (row.Cells[9].FindControl("chkMandatoryForOperator") as CheckBox).Checked.ToString();
                            string AppliesToQuality = (row.Cells[10].FindControl("chkAppliesToQuality") as CheckBox).Checked.ToString();
                            string MandatoryForQuality = (row.Cells[11].FindControl("chkMandatoryForQuality") as CheckBox).Checked.ToString();
                            string IsEnabled = (row.Cells[12].FindControl("chkIsEnabled") as CheckBox).Checked.ToString();
                            List<string> InspPlan = GEADatabaseAccess.GetInsPlanNumbers("");
                            if ((InspPlan.Where(x => x.Equals(txtinspectionPlanNumber.Text)).Count() > 0))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Ins. Plan Number  already exists for some Component Operation.')", true);
                                return;
                            }
                            if (!IsDataValid(Idd, CompID, OpnID, InsPlan, InsParamID, InsParameter, LSL, USL, DataTemplate, IsEnabled))
                            {
                                IsSaved = false;
                                break;
                            }
                            IsSaved = GEADatabaseAccess.UpdateInsMasterData("", CompID, OpnID, InsPlan, InsParamID, InsParameter, LSL, USL, UOM, DataTemplate, AppliesToOperator, MandatoryForOperator, AppliesToQuality, MandatoryForQuality, IsEnabled);
                            if (!IsSaved) break;
                        }
                    }
                    if (IsSaved)
                    {
                        lblMessages.ForeColor = Color.Green;
                        lblMessages.Text = "Data updated successfully.";
                        BindInsPlanNumbers();
                    }
                    LoadInspectionMasterData();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NoData", "alert('No data to save.')", true);
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool IsSaved = false;
            try
            {
                if (GridInspectionMaster.Rows.Count > 0)
                {
                    foreach (GridViewRow row in GridInspectionMaster.Rows)
                    {
                        string hdfUpdate = ((HiddenField)row.FindControl("hdfUpdate")).Value;
                        if (hdfUpdate == "Update")
                        {
                            string Idd = (row.Cells[1].FindControl("lblIDD") as Label).Text;
                            string CompID = Session["CompID"] != null ? Session["CompID"].ToString() : (ddlCompID.SelectedItem != null ? ddlCompID.SelectedValue : "");
                            string OpnID = Session["OpnID"] != null ? Session["OpnID"].ToString() : (ddlOperationNum.SelectedItem != null ? ddlOperationNum.SelectedValue : "");
                            string InsPlan = Session["InsPlan"] != null ? Session["InsPlan"].ToString() : (ddlInsPlanNumber.SelectedItem != null ? ddlInsPlanNumber.SelectedValue : "");
                            string InsParamID = (row.Cells[2].FindControl("txtInsParamID") as TextBox).Text;
                            string InsParameter = (row.Cells[3].FindControl("txtInsParameter") as TextBox).Text;
                            string LSL = (row.Cells[4].FindControl("txtLSL") as TextBox).Text;
                            string USL = (row.Cells[5].FindControl("txtUSL") as TextBox).Text;
                            string UOM = (row.Cells[6].FindControl("txtUOM") as TextBox).Text;
                            string DataTemplate = (row.Cells[7].FindControl("ddlDataTemplate") as DropDownList).SelectedValue;
                            string AppliesToOperator = (row.Cells[8].FindControl("chkAppliesToOperator") as CheckBox).Checked.ToString();
                            string MandatoryForOperator = (row.Cells[9].FindControl("chkMandatoryForOperator") as CheckBox).Checked.ToString();
                            string AppliesToQuality = (row.Cells[10].FindControl("chkAppliesToQuality") as CheckBox).Checked.ToString();
                            string MandatoryForQuality = (row.Cells[11].FindControl("chkMandatoryForQuality") as CheckBox).Checked.ToString();
                            string IsEnabled = (row.Cells[12].FindControl("chkIsEnabled") as CheckBox).Checked.ToString();
                            if (!IsDataValid(Idd, CompID, OpnID, InsPlan, InsParamID, InsParameter, LSL, USL, DataTemplate, IsEnabled))
                            {
                                IsSaved = false;
                                break;
                            }
                            IsSaved = GEADatabaseAccess.UpdateInsMasterData("", CompID, OpnID, InsPlan, InsParamID, InsParameter, LSL, USL, UOM, DataTemplate, AppliesToOperator, MandatoryForOperator, AppliesToQuality, MandatoryForQuality, IsEnabled);
                            if (!IsSaved) break;
                        }
                    }
                    if (IsSaved)
                    {
                        lblMessages.ForeColor = Color.Green;
                        lblMessages.Text = "Data updated successfully.";
                        BindInsPlanNumbers();
                    }
                    LoadInspectionMasterData();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NoData", "alert('No data to save.')", true);
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsDeleted = false;
                if (GridInspectionMaster.Rows.Count > 0)
                {
                    foreach (GridViewRow row in GridInspectionMaster.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkSelect = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                            if (chkSelect.Checked)
                            {
                                string Idd = (row.Cells[1].FindControl("lblIDD") as Label).Text;
                                if (!Idd.Equals("0"))
                                {
                                    IsDeleted = GEADatabaseAccess.DeleteInspectionMasterData(Idd);
                                    if (!IsDeleted) break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('No data selected for deletion.')", true);
                }
                if (IsDeleted)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Data deleted successfully.')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Error while deleting data.')", true);
                }
                LoadInspectionMasterData();
                BindInsPlanNumbers();
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnCopyInspectionPlan_Click(object sender, EventArgs e)
        {
            bool IsSaved = false;
            bool comppresent = false;
            try
            {
                List<string> InspPlan = GEADatabaseAccess.GetInsPlanNumbers("Quality");
                if ((InspPlan.Where(x => x.Equals(txtNewInspectionPlan.Text)).Count() > 0))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Ins. Plan Number  already exists for some Component Operation.')", true);
                    return;
                }
                if (!string.IsNullOrEmpty(txtNewInspectionPlan.Text))
                {
                    if (GridInspectionMaster.Rows.Count > 0)
                    {
                        foreach (GridViewRow row in GridInspectionMaster.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                string Idd = (row.Cells[1].FindControl("lblIDD") as Label).Text;
                                string CompID = Session["CompID"] != null ? Session["CompID"].ToString() : (ddlCompID.SelectedItem != null ? ddlCompID.SelectedValue : "");
                                string OpnID = Session["OpnID"] != null ? Session["OpnID"].ToString() : (ddlOperationNum.SelectedItem != null ? ddlOperationNum.SelectedValue : "");
                                string InsPlan = txtNewInspectionPlan.Text;
                                string InsParamID = (row.Cells[2].FindControl("txtInsParamID") as TextBox).Text;
                                string InsParameter = (row.Cells[3].FindControl("txtInsParameter") as TextBox).Text;
                                string LSL = (row.Cells[4].FindControl("txtLSL") as TextBox).Text;
                                string USL = (row.Cells[5].FindControl("txtUSL") as TextBox).Text;
                                string UOM = (row.Cells[6].FindControl("txtUOM") as TextBox).Text;
                                string DataTemplate = (row.Cells[7].FindControl("ddlDataTemplate") as DropDownList).SelectedValue;
                                string AppliesToOperator = (row.Cells[8].FindControl("chkAppliesToOperator") as CheckBox).Checked.ToString();
                                string MandatoryForOperator = (row.Cells[9].FindControl("chkMandatoryForOperator") as CheckBox).Checked.ToString();
                                string AppliesToQuality = (row.Cells[10].FindControl("chkAppliesToQuality") as CheckBox).Checked.ToString();
                                string MandatoryForQuality = (row.Cells[11].FindControl("chkMandatoryForQuality") as CheckBox).Checked.ToString();
                                string IsEnabled = (row.Cells[12].FindControl("chkIsEnabled") as CheckBox).Checked.ToString();

                                if (!IsDataValid(Idd, CompID, OpnID, InsPlan, InsParamID, InsParameter, LSL, USL, DataTemplate, IsEnabled))
                                {
                                    IsSaved = false;
                                    break;
                                }

                                IsSaved = GEADatabaseAccess.UpdateInsMasterData("", CompID, OpnID, InsPlan, InsParamID, InsParameter, LSL, USL, UOM, DataTemplate, AppliesToOperator, MandatoryForOperator, AppliesToQuality, MandatoryForQuality, IsEnabled);
                                if (!IsSaved) break;
                                if (GEADatabaseAccess.IsInsMasterKeyColumnExists(CompID, OpnID, InsPlan, InsParamID)) ;
                                {
                                    comppresent = false;
                                }
                            }
                        }
                        if (IsSaved)
                        {
                            lblMessages.ForeColor = Color.Green;
                            lblMessages.Text = "Data updated successfully.";
                        }
                        //if(!comppresent)
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('ComponentID, OperationNum, Ins. Plan Number and Ins. Param Name already exists.')", true);
                        BindInsPlanNumbers();
                        LoadInspectionMasterData();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('No data to copy.')", true);
                    }
                }
                else
                {
                    lblMessages.ForeColor = Color.Red;
                    lblMessages.Text = "Please enter a new inspection plan to copy.";
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LoadInspectionMasterData();
        }

        protected void ddlCompID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOperationIDs();
            BindInsPlanNumbers();
        }

        private bool IsDataRowValid(DataRow scheduleDataRow)
        {
            bool IsValid = false;
            List<string> insByList = new List<string>() { "Operator", "Quality", "Both" };
            List<string> dataTempList = new List<string>() { "1 Text Value", "2 Text Value", "1 Numeric Value", "2 Numeric Value", "Ok/NotOk", "Yes/No" };
            try
            {
                if (string.IsNullOrEmpty(scheduleDataRow["ComponentID"].ToString()) || string.IsNullOrEmpty(scheduleDataRow["OperationNum"].ToString()) || string.IsNullOrEmpty(scheduleDataRow["InsParamID"].ToString()) || string.IsNullOrEmpty(scheduleDataRow["InsPlanNumber"].ToString()) || string.IsNullOrEmpty(scheduleDataRow["InsParamName"].ToString()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('ComponentID, OperationNum, InsPlanNumber, InsParamID and InsParamName all are mandatory.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(scheduleDataRow["OperationNum"].ToString()) && !int.TryParse(scheduleDataRow["OperationNum"].ToString(), out int res))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('OperationNum is mandatory and must be an integer.')", true);
                    IsValid = false;
                }
                else if (!GEADatabaseAccess.IsCompOpnExists(scheduleDataRow["ComponentID"].ToString(), scheduleDataRow["OperationNum"].ToString()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('ComponentID(" + scheduleDataRow["ComponentID"].ToString() + ") and OperationNum (" + scheduleDataRow["OperationNum"].ToString() + ") mentioned in excel does not exists in cop.')", true);
                    IsValid = false;
                }
                else if (string.IsNullOrEmpty(scheduleDataRow["DataTemplate"].ToString()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('DataTemplate is mandatory.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(scheduleDataRow["DataTemplate"].ToString()) && !dataTempList.Any(x => x.Equals(scheduleDataRow["DataTemplate"].ToString(), StringComparison.OrdinalIgnoreCase)))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('DataTemplate can only contain 1 Text Value, 2 Text Value, 1 Numeric Value, 2 Numeric Value, Ok/NotOk and Yes/No.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(scheduleDataRow["IsEnabled"].ToString()) && !scheduleDataRow["IsEnabled"].ToString().Equals("True", StringComparison.OrdinalIgnoreCase) && !scheduleDataRow["IsEnabled"].ToString().Equals("False", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('IsEnabled can only contain True or False.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(scheduleDataRow["LSL"].ToString()) && !double.TryParse(scheduleDataRow["LSL"].ToString(), out double value))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('LSL is mandatory and must be a real number.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(scheduleDataRow["USL"].ToString()) && !double.TryParse(scheduleDataRow["USL"].ToString(), out double val))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('USL is mandatory and must be a real number.')", true);
                    IsValid = false;
                }
                else
                    IsValid = true;
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
            return IsValid;
        }

        private bool IsDataValid(string Idd, string compID, string opnID, string insPlan, string insParamID, string insParameter, string lSL, string uSL, string dataTemplate, string isEnabled)
        {
            bool IsValid = false;
            List<string> insByList = new List<string>() { "Operator", "Quality", "Both" };
            List<string> dataTempList = new List<string>() { "1 Text Value", "2 Text Value", "1 Numeric Value", "2 Numeric Value", "Ok/NotOk", "Yes/No" };
            try
            {
                if (string.IsNullOrEmpty(compID) || string.IsNullOrEmpty(opnID) || string.IsNullOrEmpty(insParamID) || string.IsNullOrEmpty(insPlan) || string.IsNullOrEmpty(insParameter))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('ComponentID, OperationNum, InsPlanNumber, InsParamID and InsParamName all are mandatory.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(opnID) && !int.TryParse(opnID, out int res))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('OperationNum is mandatory and must be an integer.')", true);
                    IsValid = false;
                }
                else if (!GEADatabaseAccess.IsCompOpnExists(compID, opnID))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('ComponentID and OperationNum mentioned in excel does not exists in cop.')", true);
                    IsValid = false;
                }
                else if (string.IsNullOrEmpty(dataTemplate))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('DataTemplate is mandatory.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(dataTemplate) && !dataTempList.Any(x => x.Equals(dataTemplate, StringComparison.OrdinalIgnoreCase)))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('DataTemplate can only contain 1 Text Value, 2 Text Value, 1 Numeric Value, 2 Numeric Value, Ok/NotOk and Yes/No.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(isEnabled) && !isEnabled.Equals("True", StringComparison.OrdinalIgnoreCase) && !isEnabled.Equals("False", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('IsEnabled can only contain True or False.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(lSL) && !double.TryParse(lSL, out double value))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('LSL is mandatory and must be a real number.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(uSL) && !double.TryParse(uSL, out double val))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('USL is mandatory and must be a real number.')", true);
                    IsValid = false;
                }
                else if (Idd.Equals(0))
                {
                    if (GEADatabaseAccess.IsInsMasterKeyColumnExists(compID, opnID, insPlan, insParamID))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('ComponentID, OperationNum, Ins. Plan Number and Ins. Param Name already exists.')", true);
                        IsValid = false;
                    }
                    else
                        IsValid = true;
                }
                else
                    IsValid = true;
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
            return IsValid;
        }

        protected void ddlOperationNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindInsPlanNumbers();
        }

        protected void btnComponent_Click(object sender, EventArgs e)
        {
            List<string> Component = new List<string>();
            if (txtComponent != null)
            {
                // Component = GEADatabaseAccess.Getcomponentbydatecomponent(txtComponent.Text.ToString());
                List<CompOpnEntity> list = getCompOpnList();
                Component = list.Where(k => k.Component.Contains(txtComponent.Text)).Select(k => k.Component).Distinct().ToList();
                ddlCompID.DataSource = Component;
                ddlCompID.DataBind();
                BindOperationIDs();
                BindInsPlanNumbers();
            }
        }
    }
}