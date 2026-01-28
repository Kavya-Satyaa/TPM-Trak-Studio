using Elmah;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;
using Web_TPMTrakDashboard.GEA.Models;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class QualityIncomingMaster : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                if (!IsPostBack)
                {
                    Session["QualityCompOpnList"] = null;
                    SessionClear.ClearSession();
                    BindComponentIDs();
                    BindOperationIDs();
                    BindInsPlanNumbers();
                    lblIQCName.Text = "Latest IQC: " + GEADatabaseAccess.GetLatestIQC("Quality");
                    await LoadQualityIncomingMasterData();
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
                if (Session["QualityCompOpnList"] == null)
                {
                    list = GEADatabaseAccess.getCompOpnForQuality();
                    Session["QualityCompOpnList"] = list;
                }
                list = Session["QualityCompOpnList"] as List<CompOpnEntity>;
            }
            catch(Exception ex)
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
                //lblIQCName.Text = GEADatabaseAccess.GetLatestIQCNumber("Quality");
                List<string> listInsPlanNumbers = GEADatabaseAccess.GetInsPlanNumbersQuality(ddlCompID.SelectedValue.ToString());
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

        private void BindQualityIncomingMasterData()
        {
            List<QualityIncomingMasterEntity> qualityIncomingMasterData = new List<QualityIncomingMasterEntity>();
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
                    qualityIncomingMasterData = GEADatabaseAccess.GetQualityIncomingMasterData(CompID, OpnID, InsPlan);
                    if (qualityIncomingMasterData != null && qualityIncomingMasterData.Count > 0)
                    {
                        Session["QualityIncomingMasterData"] = qualityIncomingMasterData;
                        GridQualityIncomingMaster.DataSource = qualityIncomingMasterData;
                        GridQualityIncomingMaster.DataBind();
                    }
                    else
                    {
                        Session["QualityIncomingMasterData"] = null;
                        GridQualityIncomingMaster.DataSource = new List<QualityIncomingMasterEntity>();
                        GridQualityIncomingMaster.DataBind();
                    }
                }
                else
                {
                    GridQualityIncomingMaster.DataSource = new List<QualityIncomingMasterEntity>();
                    GridQualityIncomingMaster.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private Task LoadQualityIncomingMasterData()
        {
            return Task.Run(() => { BindQualityIncomingMasterData(); });
        }

        protected void GridQualityIncomingMaster_RowDataBound(object sender, GridViewRowEventArgs e)
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
            }
        }

        protected async void btnView_ClickAsync(object sender, EventArgs e)
        {
            await LoadQualityIncomingMasterData();
        }

        protected async void btnImportData_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                await ImportQualityIncomingData();
                BindInsPlanNumbers();
                await LoadQualityIncomingMasterData();
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private Task ImportQualityIncomingData()
        {
            bool IsSaved = false;
            Task importTask = Task.Run(() =>
            {
                if (FileUploadQualityMaster.HasFile)
                {
                    if (Path.GetExtension(FileUploadQualityMaster.FileName) == ".xlsx")
                    {
                        lblMessages.Text = string.Empty;
                        DataTable dtQualityMasterData = new DataTable();
                        ExcelPackage Excel = new ExcelPackage(FileUploadQualityMaster.PostedFile.InputStream);
                        var worksheetQualityMaster = Excel.Workbook.Worksheets.First();
                        if (worksheetQualityMaster.Dimension.End.Column != 13)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Please select valid Template.')", true);
                            return;
                        }
                        foreach (var columnHeader in worksheetQualityMaster.Cells[1, 1, 1, worksheetQualityMaster.Dimension.End.Column])
                            dtQualityMasterData.Columns.Add(columnHeader.Text.Trim());
                        for (int rowNum = 2; rowNum <= worksheetQualityMaster.Dimension.End.Row; rowNum++)
                        {
                            var worksheetRow = worksheetQualityMaster.Cells[rowNum, 1, rowNum, worksheetQualityMaster.Dimension.End.Column];
                            if (worksheetRow != null)
                            {
                                DataRow dtQualityMasterDataRow = dtQualityMasterData.Rows.Add();
                                foreach (var qualityMasterData in worksheetRow)
                                    dtQualityMasterDataRow[qualityMasterData.Start.Column - 1] = qualityMasterData.Text.Trim();
                            }
                        }
                        if (dtQualityMasterData != null && dtQualityMasterData.Rows.Count > 0)
                        {
                            dtQualityMasterData.Columns[0].ColumnName = "ComponentID";
                            dtQualityMasterData.Columns[1].ColumnName = "OperationNum";
                            dtQualityMasterData.Columns[2].ColumnName = "InsParamID";
                            dtQualityMasterData.Columns[3].ColumnName = "InsParamName";
                            dtQualityMasterData.Columns[4].ColumnName = "InsPlanNumber";
                            dtQualityMasterData.Columns[5].ColumnName = "LSL";
                            dtQualityMasterData.Columns[6].ColumnName = "USL";
                            dtQualityMasterData.Columns[7].ColumnName = "SetValue";
                            dtQualityMasterData.Columns[8].ColumnName = "UOM";
                            dtQualityMasterData.Columns[9].ColumnName = "DataType";
                            dtQualityMasterData.Columns[10].ColumnName = "IsMandatory";
                            dtQualityMasterData.Columns[11].ColumnName = "IsEnabled";
                            dtQualityMasterData.Columns[12].ColumnName = "SortOrder";
                            //List<string> InspPlan = GEADatabaseAccess.GetInsPlanNumbers("Quality");
                            //foreach (string insplanNo in InspPlan)
                            //{
                            //    try
                            //    {
                            //        dtQualityMasterData.AsEnumerable().Where(x => x.Field<string>("InsPlanNumber").Equals(insplanNo)).ToList().ForEach(row => row.Delete());
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Logger.WriteErrorLog(ex.Message);
                            //    }
                            //}
                            foreach (DataRow qualityMasterDataRow in dtQualityMasterData.Rows)
                            {
                                if (string.IsNullOrEmpty(qualityMasterDataRow["LSL"].ToString()))
                                {
                                    qualityMasterDataRow["LSL"] = 0;
                                }
                                if (string.IsNullOrEmpty(qualityMasterDataRow["USL"].ToString()))
                                {
                                    qualityMasterDataRow["USL"] = 0;
                                }
                                if (string.IsNullOrEmpty(qualityMasterDataRow["SortOrder"].ToString()))
                                {
                                    qualityMasterDataRow["SortOrder"] = 0;
                                }
                                if (!IsDataRowValid(qualityMasterDataRow))
                                {
                                    IsSaved = false;
                                    return;
                                }
                            }
                            foreach (DataRow qualityMasterDataRow in dtQualityMasterData.Rows)
                            {
                                IsSaved = GEADatabaseAccess.UpdateQualityMasterData(qualityMasterDataRow["ComponentID"].ToString(), qualityMasterDataRow["OperationNum"].ToString(), qualityMasterDataRow["InsParamID"].ToString(), qualityMasterDataRow["InsParamName"].ToString(), qualityMasterDataRow["InsPlanNumber"].ToString(), qualityMasterDataRow["LSL"].ToString(), qualityMasterDataRow["USL"].ToString(), qualityMasterDataRow["SetValue"].ToString(), qualityMasterDataRow["UOM"].ToString(), qualityMasterDataRow["DataType"].ToString(), qualityMasterDataRow["IsMandatory"].ToString(), qualityMasterDataRow["IsEnabled"].ToString(), qualityMasterDataRow["SortOrder"].ToString());
                                if (!IsSaved) break;
                            }
                            if (IsSaved)
                            {
                                lblMessages.ForeColor = Color.Green;
                                lblMessages.Text = "Data imported successfully.";
                            }
                            else
                            {
                                lblMessages.ForeColor = Color.Red;
                                lblMessages.Text = "Data import failed.";
                                return;
                            }
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
            });
            return importTask;
        }

        protected void ddlCompID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOperationIDs();
            BindInsPlanNumbers();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            List<QualityIncomingMasterEntity> qualityIncomingMasterData = new List<QualityIncomingMasterEntity>();
            try
            {
                string CompID = Session["CompID"] as string;
                string OpnID = Session["OpnID"] as string;
                string InsPlan = Session["InsPlan"] as string;
                if (Session["InspectionMasterData"] != null)
                    qualityIncomingMasterData = Session["QualityIncomingMasterData"] as List<QualityIncomingMasterEntity>;
                else
                {
                    qualityIncomingMasterData = GEADatabaseAccess.GetQualityIncomingMasterData(CompID, OpnID, InsPlan);
                }
                if (btnNew.Text.Equals("New"))
                {
                    qualityIncomingMasterData.Add(new QualityIncomingMasterEntity { ID = 0, CharacteristicID = "", CharacteristicCode = "", InsPlanNumber = InsPlan, LSL = "", USL = "", SetValue = "", UOM = "", DataType = "", IsMandatory = false, IsEnabled = true });
                    GridQualityIncomingMaster.DataSource = qualityIncomingMasterData;
                    GridQualityIncomingMaster.DataBind();
                    btnNew.Text = "Cancel";
                }
                if (btnNew.Text.Equals("Cancel"))
                {
                    GridQualityIncomingMaster.DataSource = qualityIncomingMasterData;
                    GridQualityIncomingMaster.DataBind();
                    btnNew.Text = "New";
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected async void btnCancel_ClickAsync(object sender, EventArgs e)
        {
            await LoadQualityIncomingMasterData();
        }

        protected async void btnDelete_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                bool IsDeleted = false;
                if (GridQualityIncomingMaster.Rows.Count > 0)
                {
                    foreach (GridViewRow row in GridQualityIncomingMaster.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkSelect = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                            if (chkSelect.Checked)
                            {
                                string Idd = (row.Cells[1].FindControl("lblIDD") as Label).Text;
                                if (!Idd.Equals("0"))
                                {
                                    IsDeleted = GEADatabaseAccess.DeleteQualityMasterData(Idd);
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
                await LoadQualityIncomingMasterData();
                BindInsPlanNumbers();
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected async void btnSave_ClickAsync(object sender, EventArgs e)
        {
            bool IsSaved = false;
            try
            {
                bool IsValid = true;
                if (GridQualityIncomingMaster.Rows.Count > 0)
                {
                    foreach (GridViewRow row in GridQualityIncomingMaster.Rows)
                    {
                        string hdfUpdate = ((HiddenField)row.FindControl("hdfUpdate")).Value;
                        if (hdfUpdate == "Update")
                        {
                            string Idd = (row.Cells[1].FindControl("lblIDD") as Label).Text;
                            string CompID = Session["CompID"] != null ? Session["CompID"].ToString() : (ddlCompID.SelectedItem != null ? ddlCompID.SelectedValue : "");
                            string OpnID = Session["OpnID"] != null ? Session["OpnID"].ToString() : (ddlOperationNum.SelectedItem != null ? ddlOperationNum.SelectedValue : "");
                            string InsParamID = (row.Cells[2].FindControl("txtInsParamID") as TextBox).Text;
                            string InsParameter = (row.Cells[3].FindControl("txtInsParameter") as TextBox).Text;
                            string InsPlan = Session["InsPlan"] != null ? Session["InsPlan"].ToString() : (ddlInsPlanNumber.SelectedItem != null ? ddlInsPlanNumber.SelectedValue : "");
                            string LSL = (row.Cells[4].FindControl("txtLSL") as TextBox).Text;
                            string USL = (row.Cells[5].FindControl("txtUSL") as TextBox).Text;
                            string SetValue = (row.Cells[6].FindControl("txtSetValue") as TextBox).Text;
                            string UOM = (row.Cells[7].FindControl("txtUOM") as TextBox).Text;
                            string DataType = (row.Cells[8].FindControl("ddlDataType") as DropDownList).SelectedValue;
                            string IsMandatory = (row.Cells[9].FindControl("chkIsMandatory") as CheckBox).Checked.ToString();
                            string IsEnabled = (row.Cells[10].FindControl("chkIsEnabled") as CheckBox).Checked.ToString();
                            string sequence = (row.Cells[11].FindControl("txtSequence") as TextBox).Text;
                            //List<string> InspPlan = GEADatabaseAccess.GetInsPlanNumbers("Quality");
                            //if(InspPlan.Contains(InsPlan))
                            //{
                            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Ins. Plan Number  already exists for some.')", true);
                            //    return;
                            //}
                            if (!IsDataValid(Idd, CompID, OpnID, InsParamID, InsPlan, InsParameter, LSL, USL, DataType, IsMandatory, IsEnabled))
                            {
                                IsSaved = false;
                                break;
                            }
                            else if (GEADatabaseAccess.IsQualityKeyColumnExists(CompID, OpnID, InsPlan, InsParamID))
                            {

                                IsValid = false;
                            }
                            IsSaved = GEADatabaseAccess.UpdateQualityMasterData(CompID, OpnID, InsParamID, InsParameter, InsPlan, LSL, USL, SetValue, UOM, DataType, IsMandatory, IsEnabled, sequence);
                            if (!IsSaved) break;
                        }
                    }
                    if (IsSaved)
                    {
                        //if (!IsValid)
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('ComponentID, OperationNum, Ins. Plan Number and Ins. Param ID already exists for some.')", true);
                        lblMessages.ForeColor = Color.Green;
                        lblMessages.Text = "Data updated successfully.";
                    }
                    await LoadQualityIncomingMasterData();
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
        protected async void btnSave2_ClickAsync(object sender, EventArgs e)
        {
            bool IsSaved = false;
            try
            {
                bool IsValid = true;
                if (GridQualityIncomingMaster.Rows.Count > 0)
                {
                    foreach (GridViewRow row in GridQualityIncomingMaster.Rows)
                    {
                        string hdfUpdate = ((HiddenField)row.FindControl("hdfUpdate")).Value;
                        if (hdfUpdate == "Update")
                        {
                            string Idd = (row.Cells[1].FindControl("lblIDD") as Label).Text;
                            if (ddlCompID.SelectedValue == null) return;
                            string CompID = ddlCompID.SelectedValue.ToString();
                            if (ddlOperationNum.SelectedValue == null) return;
                            string OpnID = ddlOperationNum.SelectedValue.ToString();
                            string InsParamID = (row.Cells[2].FindControl("txtInsParamID") as TextBox).Text;
                            string InsParameter = (row.Cells[3].FindControl("txtInsParameter") as TextBox).Text;
                            //string InsPlan = Session["InsPlan"] != null ? Session["InsPlan"].ToString() : (ddlInsPlanNumber.SelectedItem != null ? ddlInsPlanNumber.SelectedValue : "");
                            string InsPlan = txtinspectionPlanNumber.Text;
                            string LSL = (row.Cells[4].FindControl("txtLSL") as TextBox).Text;
                            string USL = (row.Cells[5].FindControl("txtUSL") as TextBox).Text;
                            string SetValue = (row.Cells[6].FindControl("txtSetValue") as TextBox).Text;
                            string UOM = (row.Cells[7].FindControl("txtUOM") as TextBox).Text;
                            string DataType = (row.Cells[8].FindControl("ddlDataType") as DropDownList).SelectedValue;
                            string IsMandatory = (row.Cells[9].FindControl("chkIsMandatory") as CheckBox).Checked.ToString();
                            string IsEnabled = (row.Cells[10].FindControl("chkIsEnabled") as CheckBox).Checked.ToString();
                            string sequence = (row.Cells[11].FindControl("txtSequence") as TextBox).Text;
                            List<string> InspPlan = GEADatabaseAccess.GetInsPlanNumbers("Quality");
                            if ((InspPlan.Where(x => x.Equals(txtinspectionPlanNumber.Text)).Count() > 0))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Ins. Plan Number  already exists for some Component Operation.')", true);
                                return;
                            }
                            if (!IsDataValid(Idd, CompID, OpnID, InsParamID, InsPlan, InsParameter, LSL, USL, DataType, IsMandatory, IsEnabled))
                            {
                                IsSaved = false;
                                break;
                            }
                            else if (GEADatabaseAccess.IsQualityKeyColumnExists(CompID, OpnID, InsPlan, InsParamID))
                            {
                                
                                IsValid = false;
                            }
                            IsSaved = GEADatabaseAccess.UpdateQualityMasterData(CompID, OpnID, InsParamID, InsParameter, InsPlan, LSL, USL, SetValue, UOM, DataType, IsMandatory, IsEnabled, sequence);
                            if (!IsSaved) break;
                        }
                    }
                    if (IsSaved)
                    {
                        if (!IsValid)
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('ComponentID, OperationNum, Ins. Plan Number and Ins. Param ID already exists for some.')", true);
                        lblMessages.ForeColor = Color.Green;
                        lblMessages.Text = "Data updated successfully.";
                    }
                    BindInsPlanNumbers();
                    await LoadQualityIncomingMasterData();
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

        private bool IsDataRowValid(DataRow qualityMasterDataRow)
        {
            bool IsValid = false;
            List<string> dataTempList = new List<string>() { "Text", "Numeric", "Ok/NotOk", "Yes/No" };
            try
            {
                if (string.IsNullOrEmpty(qualityMasterDataRow["ComponentID"].ToString()) || string.IsNullOrEmpty(qualityMasterDataRow["OperationNum"].ToString()) || string.IsNullOrEmpty(qualityMasterDataRow["InsParamID"].ToString()) || string.IsNullOrEmpty(qualityMasterDataRow["InsPlanNumber"].ToString()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('ComponentID, OperationNum, InsPlanNumber and InsParamID all are mandatory.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(qualityMasterDataRow["OperationNum"].ToString()) && !int.TryParse(qualityMasterDataRow["OperationNum"].ToString(), out int res))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('OperationNum is mandatory and must be an integer.')", true);
                    IsValid = false;
                }
                else if (!GEADatabaseAccess.IsCompOpnExists(qualityMasterDataRow["ComponentID"].ToString(), qualityMasterDataRow["OperationNum"].ToString()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('ComponentID " + qualityMasterDataRow["ComponentID"].ToString() + " and OperationNum " + qualityMasterDataRow["OperationNum"].ToString() + " mentioned in excel does not exists in cop .')", true);
                    IsValid = false;
                }
                else if (string.IsNullOrEmpty(qualityMasterDataRow["DataType"].ToString()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('DataTemplate is mandatory.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(qualityMasterDataRow["DataType"].ToString()) && !dataTempList.Any(x => x.Equals(qualityMasterDataRow["DataType"].ToString(), StringComparison.OrdinalIgnoreCase)))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('DataTemplate can only contain Text, Numeric, Ok/NotOk and Yes/No.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(qualityMasterDataRow["IsEnabled"].ToString()) && !qualityMasterDataRow["IsEnabled"].ToString().Equals("True", StringComparison.OrdinalIgnoreCase) && !qualityMasterDataRow["IsEnabled"].ToString().Equals("False", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('IsEnabled can only contain True or False.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(qualityMasterDataRow["IsMandatory"].ToString()) && !qualityMasterDataRow["IsMandatory"].ToString().Equals("True", StringComparison.OrdinalIgnoreCase) && !qualityMasterDataRow["IsMandatory"].ToString().Equals("False", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Is Mandatory can only contain True or False.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(qualityMasterDataRow["LSL"].ToString()) && !double.TryParse(qualityMasterDataRow["LSL"].ToString(), out double value))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('LSL is mandatory and must be a real number.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(qualityMasterDataRow["USL"].ToString()) && !double.TryParse(qualityMasterDataRow["USL"].ToString(), out double val))
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

        private bool IsDataValid(string idd, string compID, string opnID, string insParamID, string insPlan, string insParameter, string lSL, string uSL, string dataType, string isMandatory, string isEnabled)
        {
            bool IsValid = false;
            List<string> dataTempList = new List<string>() { "Text", "Numeric", "Ok/NotOk", "Yes/No" };
            try
            {
                if (string.IsNullOrEmpty(compID) || string.IsNullOrEmpty(opnID) || string.IsNullOrEmpty(insParamID) || string.IsNullOrEmpty(insPlan))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('ComponentID, OperationNum, Ins Plan Number and Ins Param ID all are mandatory.')", true);
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
                else if (string.IsNullOrEmpty(dataType))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Data Type is mandatory.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(dataType) && !dataTempList.Any(x => x.Equals(dataType, StringComparison.OrdinalIgnoreCase)))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('DataTemplate can only contain Text, Numeric, Ok/NotOk and Yes/No.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(isEnabled) && !isEnabled.Equals("True", StringComparison.OrdinalIgnoreCase) && !isEnabled.Equals("False", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('IsEnabled can only contain True or False.')", true);
                    IsValid = false;
                }
                else if (!string.IsNullOrEmpty(isMandatory) && !isMandatory.Equals("True", StringComparison.OrdinalIgnoreCase) && !isMandatory.Equals("False", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Is Mandatory can only contain True or False.')", true);
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
                else if (idd.Equals("0"))
                {
                    if (GEADatabaseAccess.IsQualityKeyColumnExists(compID, opnID, insPlan, insParamID))
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
        protected async void btnCopyInspectionPlan_Click(object sender, EventArgs e)
        {
            bool IsSaved = false;
            bool comppresent = false;
            try
            {
                List<string> InspPlan = GEADatabaseAccess.GetInsPlanNumbers("Quality");
                if (InspPlan.Where(x => x.Equals(txtNewInspectionPlan.Text)).Count() > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Ins. Plan Number  already exists for some Component Operation.')", true);
                    return;
                }
                if (!string.IsNullOrEmpty(txtNewInspectionPlan.Text))
                {
                    if (GridQualityIncomingMaster.Rows.Count > 0)
                    {
                        foreach (GridViewRow row in GridQualityIncomingMaster.Rows)
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
                                string SetValue = (row.Cells[6].FindControl("txtSetValue") as TextBox).Text;
                                string USL = (row.Cells[5].FindControl("txtUSL") as TextBox).Text;
                                string UOM = (row.Cells[7].FindControl("txtUOM") as TextBox).Text;
                                string DataType = (row.Cells[8].FindControl("ddlDataType") as DropDownList).SelectedValue;
                                string IsMandatory = (row.Cells[9].FindControl("chkIsMandatory") as CheckBox).Checked.ToString();
                                string IsEnabled = (row.Cells[10].FindControl("chkIsEnabled") as CheckBox).Checked.ToString();
                                string sequence = (row.Cells[10].FindControl("txtSequence") as TextBox).Text;


                                if (!IsDataValid(Idd, CompID, OpnID,  InsParamID, InsPlan, InsParameter, LSL, USL, DataType, IsMandatory, IsEnabled))
                                {
                                    IsSaved = false;
                                    break;
                                }

                                IsSaved = GEADatabaseAccess.UpdateQualityMasterData(CompID, OpnID, InsParamID, InsParameter, InsPlan, LSL, USL, SetValue, UOM, DataType, IsMandatory, IsEnabled, sequence);
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
                        //if (!comppresent)
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('ComponentID, OperationNum, Ins. Plan Number and Ins. Param Name already exists.')", true);
                        BindInsPlanNumbers();
                        await LoadQualityIncomingMasterData();
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
        protected void btnDeleteInspection_Click(object sender, EventArgs e)
        {

        }
    }
}