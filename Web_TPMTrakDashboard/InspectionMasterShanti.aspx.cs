using Elmah;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class InspectionMasterShanti : System.Web.UI.Page
    {
        static string machID = "";
        static string ComponentID = "";
        static string OperationID = "";
        static string charID = "";
        static string charCode = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachineId();
                BindComponentId();
                BindOperationtId();
                GetInspectionData();

                if (!ConfigurationManager.AppSettings["MivinPages"].Equals("1"))
                {
                    GridView1.Columns[13].Visible = false;
                    GridView1.Columns[14].Visible = false;
                }
            }
        }

        #region "Bind Machine Id"
        private void BindMachineId()
        {
            try
            {
                List<string> lstMachineID = TMPTrakDataBase.GetMachineIdList();
                ddlMachine.DataSource = lstMachineID;
                ddlMachine.DataBind();
               // ddlMachine.Items.Insert(0, new ListItem("MachineAll", ""));
                if (ConfigurationManager.AppSettings["ShantiIronPages"].Equals("1"))
                {
                    ddlMachine.Items.Add("QA Gate");
                }
                GetMachineData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
                //ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        #endregion

        #region "Bind Component Id"
        private void BindComponentId()
        {
            try
            {
                List<string> lstComponentID = TMPTrakDataBase.GetComponentIdList();
                ddlComponent.DataSource = lstComponentID;
                ddlComponent.DataBind();
                //ddlComponent.Items.Insert(0, new ListItem("ComponentAll", ""));
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
                //ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        #endregion

        #region "Bind Operation Id"
        private void BindOperationtId()
        {
            try
            {
                List<string> lstOperationID = TMPTrakDataBase.GetOperationIdList(ddlComponent.SelectedValue.ToString());
                ddlOperation.DataSource = lstOperationID;
                ddlOperation.DataBind();
                ddlOperation.Items.Insert(0, new ListItem("OperationAll", ""));
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex);
            }
        }
        #endregion

        public void GetInspectionData()
        {
            try
            {
                List<InspectionMasterData> masterData = new List<InspectionMasterData>();
                string machineID = ddlMachine.SelectedItem.Text == "MachineAll" ? "" : ddlMachine.SelectedItem.Text;
                string CompID = ddlComponent.SelectedItem.Text == "ComponentAll" ? "" : ddlComponent.SelectedItem.Text;
                string OprID = ddlOperation.SelectedItem.Text == "OperationAll" ? "" : ddlOperation.SelectedItem.Text;
                masterData = TMPTrakDataBase.GetInspectionMasterData(machineID, CompID, OprID);

                if (masterData != null && masterData.Count > 0)
                {
                    GridView1.DataSource = masterData;
                    GridView1.DataBind();

                    
                }
                else
                {
                    masterData.Add(new InspectionMasterData { DataTemplate = "Ok", InspectedBy = "Operator" });
                    GridView1.DataSource = masterData;
                    GridView1.DataBind();
                    GridView1.Rows[0].Visible = false;
                }
                Session["InpectionData"] = masterData;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            if (ddlMachine.SelectedValue.ToString().Equals("", StringComparison.OrdinalIgnoreCase))
            {
                GridView1.Columns[0].Visible = true;
            }
            else
            {
                GridView1.Columns[0].Visible = false;
            }
            if (ddlComponent.SelectedValue.ToString().Equals("", StringComparison.OrdinalIgnoreCase))
            {
                GridView1.Columns[1].Visible = true;
            }
            else
            {
                GridView1.Columns[1].Visible = false;
            }
            if (ddlOperation.SelectedValue.ToString().Equals("", StringComparison.OrdinalIgnoreCase))
            {
                GridView1.Columns[2].Visible = true;
            }
            else
            {
                GridView1.Columns[2].Visible = false;
            }
            GetInspectionData();
        }

        private void GetMachineData()
        {
            List<string> CopyMachinelst = TMPTrakDataBase.GetMachineIdList();
            CopyMachinelst = CopyMachinelst.AsEnumerable().Where(x => x != ddlMachine.SelectedValue.ToString()).ToList();
            ddlMultiMachineId.DataSource = CopyMachinelst;
            ddlMultiMachineId.DataBind();

        //protected void btnNew_Click(object sender, EventArgs e)
        //{
        //    GridView1.ShowFooter = true;
        //    GetInspectionData();
        //    btnCancel.Visible = true;
        //    btnNew.Visible = false;
        //}

        //protected void btnCancel_Click(object sender, EventArgs e)
        //{
        //    GridView1.ShowFooter = false;
        //    GetInspectionData();
        //    btnCancel.Visible = false;
        //    btnNew.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    InspectionMasterData inspectionData = new InspectionMasterData();
                    inspectionData.MachineID = (GridView1.Rows[i].FindControl("hdnMachineID") as HiddenField).Value;
                    inspectionData.ComponentID = (GridView1.Rows[i].FindControl("hdnCompID") as HiddenField).Value;
                    inspectionData.OperationID = (GridView1.Rows[i].FindControl("hdnOperationID") as HiddenField).Value;
                    inspectionData.CharID = (GridView1.Rows[i].FindControl("txtCharID") as Label).Text;
                    inspectionData.CharCode = (GridView1.Rows[i].FindControl("txtCharactersticCode") as TextBox).Text;
                    //inspectionData.CharCode = (GridView1.Rows[i].FindControl("txtCharCode") as Label).Text;
                    inspectionData.specification = (GridView1.Rows[i].FindControl("txtSpecification") as TextBox).Text;
                    inspectionData.LSL = (GridView1.Rows[i].FindControl("txtLSL") as TextBox).Text;
                    inspectionData.USL = (GridView1.Rows[i].FindControl("txtUSL") as TextBox).Text;
                    inspectionData.UOM = (GridView1.Rows[i].FindControl("txtUOM") as TextBox).Text;

                    inspectionData.UpperWarningZone = (GridView1.Rows[i].FindControl("txtUpperWarningZone") as TextBox).Text;
                    inspectionData.UpperOperatingZone = (GridView1.Rows[i].FindControl("txtUpperOperatingZone") as TextBox).Text;
                    inspectionData.LowerOperatingZone = (GridView1.Rows[i].FindControl("txtLowerOperatingZone") as TextBox).Text;
                    inspectionData.LowerWarningZone = (GridView1.Rows[i].FindControl("txtLowerWarningZone") as TextBox).Text;
                    inspectionData.SampleSize = (GridView1.Rows[i].FindControl("txtSampleSize") as TextBox).Text;

                    inspectionData.DataTemplate = (GridView1.Rows[i].FindControl("ddlDataTemp") as DropDownList).SelectedValue.ToString();
                    inspectionData.InspectedBy = (GridView1.Rows[i].FindControl("ddlInspectedBy") as DropDownList).SelectedValue.ToString();
                    inspectionData.IsEnabled = (GridView1.Rows[i].FindControl("chkEnabled") as CheckBox).Checked;
                    if (ConfigurationManager.AppSettings["MivinPages"].Equals("1"))
                    {
                        inspectionData.Channel = (GridView1.Rows[i].FindControl("txtChannel") as TextBox).Text;
                        inspectionData.InputMethod = (GridView1.Rows[i].FindControl("ddlInputMethod") as DropDownList).SelectedValue.ToString();
                    }
                    if (string.IsNullOrEmpty(inspectionData.MachineID))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please provide Machineid", true);
                        return;
                    }

                    else if (string.IsNullOrEmpty(inspectionData.ComponentID))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please provide Machineid')", true);
                        return;
                    }

                    else if (string.IsNullOrEmpty(inspectionData.CharID))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please provide CharID')", true);
                        return;
                    }

                    else if (string.IsNullOrEmpty(inspectionData.OperationID))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please provide OperationID')", true);
                        return;
                    }

                    validations(inspectionData);
                    TMPTrakDataBase.InsertUpdateData(inspectionData);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Success" + 1, "<script>showpop('Inspection Data successfully updated!!')</script>", false);

                }
                GetInspectionData();
            }
            catch (Exception ex)
            {
                Console.Write("Error while inserting data - " + ex.Message);
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //GridView1.ShowFooter = false;
            machID = (GridView1.Rows[e.RowIndex].FindControl("hdnMachineID") as HiddenField).Value;
            ComponentID = (GridView1.Rows[e.RowIndex].FindControl("hdnCompID") as HiddenField).Value;
            OperationID = (GridView1.Rows[e.RowIndex].FindControl("hdnOperationID") as HiddenField).Value;
            charID = (GridView1.Rows[e.RowIndex].FindControl("txtCharID") as Label).Text;
            if (GridView1.Rows[e.RowIndex].FindControl("txtCharactersticCode") is TextBox)
                charCode = (GridView1.Rows[e.RowIndex].FindControl("txtCharactersticCode") as TextBox).Text;
            else
                charID = "";

            ScriptManager.RegisterStartupScript(this, GetType(), "RecordsTextopenModaladded", "openConfirmModal('Are you sure, you want to delete this record?');", true);
        }

        protected void saveConfirmYes_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string result = "";
                TMPTrakDataBase.DeleteInspectionData(machID, ComponentID, OperationID, charID, charCode, out result);
                GridView1.EditIndex = -1;
                GetInspectionData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {

                    if ((GridView1.FooterRow.FindControl("txtCharIDfooter") as TextBox).Text == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select Machineid ,Comp and Operation')", true);
                        return;
                    }
                    else
                    {
                        string MachineID = ddlMachine.SelectedValue.ToString() == "MachineAll" ? "" : ddlMachine.SelectedValue.ToString();
                        string ComponentID = ddlComponent.SelectedValue.ToString() == "ComponentAll" ? "" : ddlComponent.SelectedValue.ToString();
                        string OperationID = ddlOperation.SelectedValue.ToString() == "OperationAll" ? "" : ddlOperation.SelectedValue.ToString();

                        InspectionMasterData inspectionData = new InspectionMasterData();
                        inspectionData.MachineID = MachineID;
                        inspectionData.ComponentID = ComponentID;
                        inspectionData.OperationID = OperationID;
                        inspectionData.CharID = (GridView1.FooterRow.FindControl("txtCharIDfooter") as TextBox).Text;
                        inspectionData.CharCode = (GridView1.FooterRow.FindControl("txtCharCodefooter") as TextBox).Text;
                        inspectionData.specification = (GridView1.FooterRow.FindControl("txtSpecificationfooter") as TextBox).Text;
                        inspectionData.LSL = (GridView1.FooterRow.FindControl("txtLSLfooter") as TextBox).Text;
                        inspectionData.USL = (GridView1.FooterRow.FindControl("txtUSLfooter") as TextBox).Text;
                        inspectionData.UOM = (GridView1.FooterRow.FindControl("txtUOMfooter") as TextBox).Text;

                        inspectionData.UpperOperatingZone = (GridView1.FooterRow.FindControl("txtUpperOperatingZonefooter") as TextBox).Text;
                        inspectionData.UpperWarningZone = (GridView1.FooterRow.FindControl("txtUpperWarningZonefooter") as TextBox).Text;
                        inspectionData.LowerOperatingZone = (GridView1.FooterRow.FindControl("txtLowerOperatingZonefooter") as TextBox).Text;
                        inspectionData.LowerWarningZone = (GridView1.FooterRow.FindControl("txtLowerWarningZonefooter") as TextBox).Text;
                        inspectionData.SampleSize = (GridView1.FooterRow.FindControl("txtSampleSizefooter") as TextBox).Text;

                        inspectionData.DataTemplate = (GridView1.FooterRow.FindControl("ddlDataTempfooter") as DropDownList).SelectedValue.ToString();
                        inspectionData.InspectedBy = (GridView1.FooterRow.FindControl("ddlInspectedByfooter") as DropDownList).SelectedValue.ToString();

                        inspectionData.IsEnabled = (GridView1.FooterRow.FindControl("chkEnabledfooter") as CheckBox).Checked;
                        if (ConfigurationManager.AppSettings["MivinPages"].Equals("1"))
                        {
                            inspectionData.InputMethod = (GridView1.FooterRow.FindControl("ddlFooterInputMethod") as DropDownList).SelectedValue.ToString();
                            inspectionData.Channel = (GridView1.FooterRow.FindControl("txtFooterChannel") as TextBox).Text;
                        }
                        if (string.IsNullOrEmpty(inspectionData.MachineID) || string.IsNullOrEmpty(inspectionData.ComponentID) || string.IsNullOrEmpty(inspectionData.OperationID) || string.IsNullOrEmpty(inspectionData.CharID))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select Machineid ,Comp ,Operation and Characteristic ID')", true);
                            return;
                        }
                        validations(inspectionData);

                        TMPTrakDataBase.InsertUpdateData(inspectionData);


                        // GridView1.ShowFooter = false;
                        GetInspectionData();

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Success" + 1, "<script>showpop('Inspection Data saved successfully!!')</script>", false);

                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write("Error while inserting data - " + ex.Message);
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            DataTable dtQ_Check = new DataTable();

            if (fileupload.HasFile)
            {
                string fileName = fileupload.FileName;
                if (Path.GetExtension(fileName) != ".xlsx")
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning" + 1, "<script>showpop1('Please choose a valid excel(.xlsx) file.')</script>", false);
                    return;
                }
                else
                {
                    Directory.CreateDirectory(Server.MapPath("ImportedFiles"));
                    string savedFileName = Server.MapPath("ImportedFiles//" + fileName);
                    fileupload.SaveAs(savedFileName);
                    dtQ_Check = GetDataTableFromFile(fileupload);
                    if (dtQ_Check != null && dtQ_Check.Rows.Count > 0)
                    {
                        List<InspectionMasterData> ListmasterData = new List<InspectionMasterData>();
                        for (int i = 0; i < dtQ_Check.Rows.Count; i++)
                        {




                            InspectionMasterData masterData = new InspectionMasterData();
                            masterData.MachineID = dtQ_Check.Rows[i]["MachineID"].ToString();
                            masterData.ComponentID = dtQ_Check.Rows[i]["ComponentID"].ToString();
                            masterData.OperationID = dtQ_Check.Rows[i]["OperationID"].ToString();
                            masterData.CharID = dtQ_Check.Rows[i]["CharID"].ToString();
                            masterData.CharCode = dtQ_Check.Rows[i]["CharCode"].ToString();
                            masterData.specification = dtQ_Check.Rows[i]["Specification"].ToString();
                            masterData.LSL = dtQ_Check.Rows[i]["LSL"].ToString();
                            masterData.USL = dtQ_Check.Rows[i]["USL"].ToString();
                            masterData.UOM = dtQ_Check.Rows[i]["UOM"].ToString();

                            masterData.UpperWarningZone = dtQ_Check.Rows[i]["UpperWarningZone"].ToString();
                            masterData.UpperOperatingZone = dtQ_Check.Rows[i]["UpperOperatingZone"].ToString();
                            masterData.LowerWarningZone = dtQ_Check.Rows[i]["LowerWarningZone"].ToString();
                            masterData.LowerOperatingZone = dtQ_Check.Rows[i]["LowerOperatingZone"].ToString();


                            try
                            {
                                Convert.ToDouble(masterData.LSL);
                                Convert.ToDouble(masterData.USL);
                                Convert.ToDouble(masterData.UpperOperatingZone);
                                Convert.ToDouble(masterData.UpperWarningZone);
                                Convert.ToDouble(masterData.LowerOperatingZone);
                                Convert.ToDouble(masterData.LowerWarningZone);

                            }
                            catch (Exception)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot import because LSL USL are not in the correct format')", true);
                                return;
                            }
                            validations(masterData);
                            if (string.IsNullOrEmpty(masterData.UOM))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('UOM Cannot be empty')", true);
                                return;
                            }
                            masterData.DataTemplate = dtQ_Check.Rows[i]["DataTemplate"].ToString();
                            masterData.SampleSize = dtQ_Check.Rows[i]["SampleSize"].ToString();
                            masterData.InspectedBy = dtQ_Check.Rows[i]["InspectedBy"].ToString();
                            masterData.Channel = dtQ_Check.Rows[i]["Channel"].ToString();
                            masterData.InputMethod = dtQ_Check.Rows[i]["InputMethod"].ToString();
                            if (dtQ_Check.Rows[i]["IsEnabled"].ToString() == "0")
                            {
                                masterData.IsEnabled = false;
                            }
                            else
                            {
                                masterData.IsEnabled = true;
                            }
                            ListmasterData.Add(masterData);
                            TMPTrakDataBase.InsertUpdateData(masterData);
                        }
                        GetInspectionData();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "EmptyFileopenModal", "openWarningModal('Import failed. Empty excel file.')", true);
                        return;
                    }
                }
            }

        }

        private void validations(InspectionMasterData masterData)
        {
            try
            {
                double LSL = 0.0, USL = 0.0, UWL = 0.0, LWL = 0.0, UOL = 0.0, LOL = 0.0, Mean = 0.0;
                if (!(double.TryParse(masterData.specification, out Mean)))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Specification Mean is Mandatory')", true);
                    return;
                }
                if ((double.TryParse(masterData.LSL, out LSL) && double.TryParse(masterData.USL, out USL)))
                {
                    if (USL.Equals("0"))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('USL cannoy be Zero')", true);
                        return;
                    }
                    else if (USL.Equals("0"))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('USL cannoy be Zero')", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('LSL and USL are Mandatory')", true);
                    return;
                }
                if (double.TryParse(masterData.UpperOperatingZone, out UOL) && double.TryParse(masterData.UpperWarningZone, out UWL) && double.TryParse(masterData.LowerWarningZone, out LWL) && double.TryParse(masterData.LowerOperatingZone, out LOL) && UOL != 0 && LOL != 0 && UWL != 0 && LWL != 0)
                {

                    if (LSL > LWL)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Lower Warning Zone Should be greater than LSL')", true);
                        return;
                    }
                    else if (LWL > LOL)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Lower operating Zone Should be greater than Lower Warning Zone')", true);
                        return;
                    }

                    else if (LOL > Mean)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Upper operating Zone Should be greater than Specification')", true);
                        return;
                    }
                    else if (Mean > UOL)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Specidication Should be greater than Specification')", true);
                        return;
                    }
                    else if (UOL > UWL)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Upper Warning Zone Should be greater than Upper Operating Zone')", true);
                        return;
                    }
                    else if (UWL > USL)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('USL Should be greater than Upper Warning Zone')", true);
                        return;
                    }
                }
                else
                {
                    if (double.TryParse(masterData.UpperOperatingZone, out UOL) && double.TryParse(masterData.UpperOperatingZone, out UOL) && UOL != 0 && LOL != 0)
                    {
                        if (UOL == 0 && UWL == 0)
                        {
                            return;
                        }
                        if (Mean > UOL)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Specidication Should be greater than Specification')", true);
                            return;
                        }
                        if (UOL > UWL)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Specidication Should be greater than Specification')", true);
                            return;
                        }
                        else if (USL < UWL)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('USL Should be greater than Upper Warning Zone')", true);
                            return;
                        }
                    }
                    if (double.TryParse(masterData.LowerWarningZone, out LWL) && double.TryParse(masterData.UpperWarningZone, out UWL) && LWL != 0 && UWL != 0)
                    {
                        if (LWL == 0 && UWL == 0)
                        {
                            return;
                        }
                        if (LSL > LWL)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Lower Warning Zone Should be greater than LSL')", true);
                            return;
                        }
                        else if (USL < UWL)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('USL Should be greater than Upper Warning Zone')", true);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void importClick(object sender, EventArgs e)
        {
            //if (globe_Check.HasFile)
            //{
            //    importFile();
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning" + 1, "<script>showpop('Please choose Excel file to import')</script>", false);
            //}
        }

        public void importFile()
        {
            try
            {
                //DataTable dtQ_Check = new DataTable();
                //if (globe_Check.HasFile)
                //{

                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Warning" + 1, "<script>showpop('Please choose a file to import.')</script>", false);
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private DataTable GetDataTableFromFile(FileUpload fileName)
        {
            DataTable dtMasterData = new DataTable();

            ExcelPackage Excel = new ExcelPackage(fileName.PostedFile.InputStream);
            var worksheet = Excel.Workbook.Worksheets.First();
            try
            {
                List<string> excelData = new List<string>();
                dtMasterData.Columns.Add("MachineID");
                dtMasterData.Columns.Add("ComponentID");
                dtMasterData.Columns.Add("OperationID");
                dtMasterData.Columns.Add("CharID");
                dtMasterData.Columns.Add("CharCode");
                dtMasterData.Columns.Add("Specification");
                dtMasterData.Columns.Add("LSL");
                dtMasterData.Columns.Add("USL");
                dtMasterData.Columns.Add("UOM");

                dtMasterData.Columns.Add("UpperWarningZone");
                dtMasterData.Columns.Add("UpperOperatingZone");
                dtMasterData.Columns.Add("LowerWarningZone");
                dtMasterData.Columns.Add("LowerOperatingZone");

                dtMasterData.Columns.Add("DataTemplate");
                dtMasterData.Columns.Add("InspectedBy");
                dtMasterData.Columns.Add("IsEnabled");
                dtMasterData.Columns.Add("SampleSize");
                dtMasterData.Columns.Add("Channel");
                dtMasterData.Columns.Add("InputMethod");

                int i = 0;
                for (i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                {
                    if (worksheet.Cells[i, 1].Value != null && !string.IsNullOrEmpty(worksheet.Cells[i, 1].Value.ToString()))
                    {
                        DataRow dtrow = dtMasterData.Rows.Add();
                        dtrow["MachineID"] = worksheet.Cells[i, 1].Value.ToString();
                        dtrow["ComponentID"] = worksheet.Cells[i, 2].Value.ToString();
                        dtrow["OperationID"] = worksheet.Cells[i, 3].Value.ToString();
                        dtrow["CharID"] = worksheet.Cells[i, 4].Value.ToString();
                        dtrow["CharCode"] = worksheet.Cells[i, 5].Value.ToString();
                        dtrow["LSL"] = worksheet.Cells[i, 6].Value != null ? worksheet.Cells[i, 6].Value.ToString() : "0";
                        dtrow["LowerWarningZone"] = worksheet.Cells[i, 7].Value != null ? worksheet.Cells[i, 7].Value.ToString() : "0";
                        dtrow["LowerOperatingZone"] = worksheet.Cells[i, 8].Value != null ? worksheet.Cells[i, 8].Value.ToString() : "0";
                        dtrow["Specification"] = worksheet.Cells[i, 9].Value != null ? worksheet.Cells[i, 9].Value.ToString() : "";
                        dtrow["UpperOperatingZone"] = worksheet.Cells[i, 10].Value != null ? worksheet.Cells[i, 10].Value.ToString() : "0";
                        dtrow["UpperWarningZone"] = worksheet.Cells[i, 11].Value != null ? worksheet.Cells[i, 11].Value.ToString() : "0";
                        dtrow["USL"] = worksheet.Cells[i, 12].Value != null ? worksheet.Cells[i, 12].Value.ToString() : "0";
                        dtrow["UOM"] = worksheet.Cells[i, 13].Value != null ? worksheet.Cells[i, 13].Value.ToString() : "0";


                        dtrow["DataTemplate"] = worksheet.Cells[i, 14].Value != null ? worksheet.Cells[i, 14].Value.ToString() : "Text";
                        dtrow["SampleSize"] = worksheet.Cells[i, 15].Value != null ? worksheet.Cells[i, 15].Value.ToString() : "0";
                        dtrow["InspectedBy"] = worksheet.Cells[i, 16].Value != null ? worksheet.Cells[i, 16].Value.ToString() : "Both";
                        dtrow["IsEnabled"] = worksheet.Cells[i, 17].Value != null ? worksheet.Cells[i, 17].Value : 0;
                        dtrow["Channel"] = worksheet.Cells[i, 18].Value == null ? "" : worksheet.Cells[i, 18].Value.ToString();
                        dtrow["InputMethod"] = worksheet.Cells[i, 19].Value != null ? worksheet.Cells[i, 19].Value.ToString() : "";
                        //dtMasterData.Rows.Add(dtrow);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while importing excel file : " + ex.Message);
            }
            return dtMasterData;
        }

        protected void ddlMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(ddlMachine.SelectedValue=="QA Gate")
                {
                    if (ConfigurationManager.AppSettings["ShantiIronPages"].Equals("1"))
                    {
                        List<string> list = new List<string>();
                        list.Add("65");
                        ddlOperation.DataSource = list;
                        ddlOperation.DataBind();
                    }
                    
                }
                else
                {
                    BindOperationtId();
                }
                GetMachineData();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            bool status = false;
            List<InspectionMasterData> data = (List<InspectionMasterData>)Session["InpectionData"];
            if( data!=null && data.Count>0)
            {
                string MachineID = string.Empty;
                foreach (System.Web.UI.WebControls.ListItem Item in ddlMultiMachineId.Items)
                {
                    MachineID += Item.Selected ? "'" + Item.Value + "'," : "";
                }
                MachineID = MachineID.Trim(',');
                InspectionMasterData inspectionData = new InspectionMasterData();
                inspectionData.MachineID = ddlMachine.SelectedValue.ToString();
                inspectionData.SelectedMachineID = MachineID;
                inspectionData.ComponentID = ddlComponent.SelectedValue.ToString();
                if (ddlOperation.SelectedValue.ToString().Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select Operation ID')", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "CopyCancelmode", "HideCopyModel();", true);
                }
                else
                {
                    inspectionData.OperationID = ddlOperation.SelectedValue.ToString();
                    TMPTrakDataBase.CopyData(inspectionData, out status);
                }
            
                if(status)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Copied Successfully!')", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "Copyviewmode", "CopyView();", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No data to Copy')", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "CopyCancelmode", "HideCopyModel();", true);
            }
            GetInspectionData();
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chckheader = (CheckBox)GridView1.HeaderRow.FindControl("chkSelectAll");
            if (chckheader.Checked == true)
            {
                foreach (GridViewRow row in GridView1.Rows)
                {
                    CheckBox CheckBoxchckrw = (CheckBox)row.FindControl("chkSelect");
                    CheckBoxchckrw.Checked = true;
                }
                
            }
            else
            {
                foreach (GridViewRow row in GridView1.Rows)
                {
                    CheckBox CheckBoxchckrw = (CheckBox)row.FindControl("chkSelect");
                    CheckBoxchckrw.Checked = false;
                }
            }
        }

        public static string SafeFileName(string name)
        {
            StringBuilder str = new StringBuilder(name);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.ToString();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string appPath = HttpContext.Current.Server.MapPath("~/Reports");
                string src, dst = string.Empty;
                string reportName = "InspectionMasterReport.xlsx";
                src = Util.GetReportPath(reportName);

                string tempfileName = "InspectionMasterReport" + "_" + Guid.NewGuid() + ".xlsx";
                dst = Path.Combine(appPath, "Temp", SafeFileName(tempfileName));

                if (!File.Exists(src))
                {
                    Logger.WriteDebugLog("Inspection Master report Log by day-Excel - \n " + src);
                }
                FileInfo newFile = new FileInfo(src);
                ExcelPackage pck = new ExcelPackage(newFile, true);
                var wsDts = pck.Workbook.Worksheets["Sheet1"];
                int row = 2,j=1;
                List<InspectionMasterData> data = (List<InspectionMasterData>)Session["InpectionData"];
                if(data.Count>0 && data!=null)
                {
                    foreach(InspectionMasterData item in data)
                    {
                        j = 1;
                        wsDts.Cells[row, j].Value = item.MachineID;
                        j++;
                        wsDts.Cells[row, j].Value = item.ComponentID;
                        j++;
                        wsDts.Cells[row, j].Value = item.OperationID;
                        j++;
                        wsDts.Cells[row, j].Value = item.CharID;
                        j++;
                        wsDts.Cells[row, j].Value = item.CharCode;
                        j++;
                        wsDts.Cells[row, j].Value = item.LSL;
                        j++;
                        wsDts.Cells[row, j].Value = item.LowerWarningZone;
                        j++;
                        wsDts.Cells[row, j].Value = item.LowerOperatingZone;
                        j++;
                        wsDts.Cells[row, j].Value = item.specification;
                        j++;
                        wsDts.Cells[row, j].Value = item.UpperOperatingZone;
                        j++;
                        wsDts.Cells[row, j].Value = item.UpperWarningZone;
                        j++;
                        wsDts.Cells[row, j].Value = item.USL;
                        j++;
                        wsDts.Cells[row, j].Value = item.UOM;
                        j++;
                        wsDts.Cells[row, j].Value = item.DataTemplate;
                        j++;
                        wsDts.Cells[row, j].Value = item.SampleSize;
                        j++;
                        wsDts.Cells[row, j].Value = item.InspectedBy;
                        j++;
                        wsDts.Cells[row, j].Value = item.IsEnabled;
                        j++;
                        wsDts.Cells[row, j].Value = item.Channel;
                        j++;
                        wsDts.Cells[row, j].Value = item.InputMethod;
                        row++;
                    }
                    row--;
                    wsDts.Cells[2, 1, row, j].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDts.Cells[2, 1, row, j].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDts.Cells[2, 1, row, j].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDts.Cells[2, 1, row, j].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    wsDts.Cells[2, 1, row, j].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    wsDts.Cells[2, 1, row, j].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    wsDts.Cells[2, 1, row, j].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    wsDts.Cells[2, 1, row, j].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    DownloadMultipleFile(dst, pck.GetAsByteArray());
                    Logger.WriteDebugLog("Inspection Master Report.");
                }
            }
            catch (Exception)
            {
                
            }
        }

        #region "Down Load File"
        private static void DownloadMultipleFile(string fileName, byte[] byteArray)
        {
            try
            {

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Path.GetFileName(fileName) + "\"");
                HttpContext.Current.Response.OutputStream.Write(byteArray, 0, byteArray.Length);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                //HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //Logger.WriteErrorLog("GENERATED ERROR : \n" + "Report generation Failed Error: " + ex.ToString());
            }
        }
        #endregion

        protected void btnCopy_Click1(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Copyviewmode", "CopyView();", true);
            GetMachineData();
        }

        protected void ddlComponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOperationtId();
        }
    }
}