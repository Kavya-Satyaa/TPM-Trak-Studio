using Elmah;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.VulkanMachineShop
{
    public partial class InspectionMasterVulkan : System.Web.UI.Page
    {
        static string ComponentID = "";
        static string OperationID = "";
        static string charID = "";
        static string charCode = "";
        static string appPath = HttpContext.Current.Server.MapPath("~/VulkanMachineShop/ReportTemplate");
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindComponentId();
                GetInspectionData();
            }
        }


        private void BindComponentId()
        {
            try
            {
                List<string> lstComponentID = TMPTrakDataBase.GetComponentIdList();
                ddlComponent.DataSource = lstComponentID;
                ddlComponent.DataBind();
                BindOperationtId();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindOperationtId()
        {
            try
            {
                List<string> lstOperationID = TMPTrakDataBase.GetOperationIdList(ddlComponent.SelectedValue.ToString());
                ddlOperation.DataSource = lstOperationID;
                ddlOperation.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlComponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOperationtId();
        }

        public void GetInspectionData()
        {
            try
            {
                btnEdit.Visible = true;
                List<InspectionMasterData> masterData = new List<InspectionMasterData>();
                string CompID = ddlComponent.SelectedItem.Text == "ComponentAll" ? "" : ddlComponent.SelectedItem.Text;
                string OprID = ddlOperation.SelectedItem.Text == "OperationAll" ? "" : ddlOperation.SelectedItem.Text;
                lblComp.Text ="Component : "+ CompID;
                masterData = TMPTrakDataBase.GetInspectionMasterData("",CompID, OprID);
                if (masterData != null && masterData.Count > 0)
                {
                    GridView1.DataSource = masterData;
                    GridView1.DataBind();
                }
                else
                {
                    btnEdit.Visible = false;
                    masterData.Add(new InspectionMasterData { DataTemplate = "Ok/NotOk", InspectedBy = "Operator" });
                    GridView1.DataSource = masterData;
                    GridView1.DataBind();
                    GridView1.Rows[0].Visible = false;
                }
                foreach (GridViewRow gv in GridView1.Rows)
                {
                    gv.Cells[2].Enabled = false;
                    gv.Cells[3].Enabled = false;
                    gv.Cells[4].Enabled = false;
                    gv.Cells[7].Enabled = false;
                    gv.Cells[10].Enabled = false;
                    gv.Cells[11].Enabled = false;
                    gv.Cells[12].Enabled = false;
                    gv.Cells[13].Enabled = false;
                    gv.Cells[14].Enabled = false;
                }
                btnSave.Visible = false;
                Session["InpectionData"] = masterData;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            GetInspectionData();
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            DataTable dtQ_Check = new DataTable();
            int flag = 0;
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
                            masterData.MachineID = string.Empty;
                            masterData.ComponentID = dtQ_Check.Rows[i]["ComponentID"].ToString().Trim();
                            masterData.OperationID = dtQ_Check.Rows[i]["OperationID"].ToString().Trim();
                            masterData.CharID = dtQ_Check.Rows[i]["CharID"].ToString().Trim();
                            masterData.CharCode = dtQ_Check.Rows[i]["CharCode"].ToString().Trim();
                            masterData.specification = dtQ_Check.Rows[i]["Specification"].ToString().Trim();
                            masterData.LSL = dtQ_Check.Rows[i]["LSL"].ToString().Trim();
                            masterData.USL = dtQ_Check.Rows[i]["USL"].ToString().Trim();

                            masterData.UpperWarningZone = string.Empty;
                            masterData.UpperOperatingZone = string.Empty;
                            masterData.LowerWarningZone = string.Empty;
                            masterData.LowerOperatingZone = string.Empty;
                            masterData.UOM = string.Empty;
                            masterData.InspectedBy = string.Empty;
                            try
                            {
                                Convert.ToDouble(masterData.LSL);
                                Convert.ToDouble(masterData.USL);
                            }
                            catch (Exception)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot import because LSL USL are not in the correct format')", true);
                                return;
                            }
                            masterData.DataTemplate = dtQ_Check.Rows[i]["DataTemplate"].ToString().Trim();
                            masterData.SampleSize = dtQ_Check.Rows[i]["SampleSize"].ToString();
                            if (dtQ_Check.Rows[i]["IsEnabled"].ToString() == "0")
                            {
                                masterData.IsEnabled = false;
                            }
                            else
                            {
                                masterData.IsEnabled = true;
                            }
                            masterData.SortOrder= dtQ_Check.Rows[i]["SortOrder"].ToString().Trim();
                            flag = validations(masterData);
                            if (flag <= 0)
                            {
                                ListmasterData.Add(masterData);
                                TMPTrakDataBase.InsertUpdateData(masterData);
                            }
                        }
                        if (flag <= 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Inspection Data successfully Inserted!!')", true);
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

        private DataTable GetDataTableFromFile(FileUpload fileName)
        {
            DataTable dtMasterData = new DataTable();
            ExcelPackage Excel = new ExcelPackage(fileName.PostedFile.InputStream);
            var worksheet = Excel.Workbook.Worksheets.First();
            try
            {
                List<string> excelData = new List<string>();
                dtMasterData.Columns.Add("ComponentID");
                dtMasterData.Columns.Add("OperationID");
                dtMasterData.Columns.Add("CharID");
                dtMasterData.Columns.Add("CharCode");
                dtMasterData.Columns.Add("Specification");
                dtMasterData.Columns.Add("LSL");
                dtMasterData.Columns.Add("USL");
                dtMasterData.Columns.Add("DataTemplate");
                dtMasterData.Columns.Add("IsEnabled");
                dtMasterData.Columns.Add("SampleSize");
                dtMasterData.Columns.Add("SortOrder");

                int i = 0;
                for (i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                {
                    if (worksheet.Cells[i, 1].Value != null && !string.IsNullOrEmpty(worksheet.Cells[i, 1].Value.ToString()))
                    {
                        DataRow dtrow = dtMasterData.Rows.Add();
                        dtrow["ComponentID"] = worksheet.Cells[i, 1].Value.ToString();
                        dtrow["OperationID"] = worksheet.Cells[i, 2].Value.ToString();
                        dtrow["CharID"] = worksheet.Cells[i, 3].Value.ToString();
                        dtrow["CharCode"] = worksheet.Cells[i, 4].Value.ToString();
                        dtrow["LSL"] = worksheet.Cells[i, 5].Value != null ? worksheet.Cells[i, 5].Value.ToString() : "0";
                        dtrow["Specification"] = worksheet.Cells[i, 6].Value != null ? worksheet.Cells[i, 6].Value.ToString() : "";
                        dtrow["USL"] = worksheet.Cells[i, 7].Value != null ? worksheet.Cells[i, 7].Value.ToString() : "0";
                        dtrow["DataTemplate"] = worksheet.Cells[i, 8].Value != null ? worksheet.Cells[i, 8].Value.ToString() : "Text";
                        dtrow["SampleSize"] = worksheet.Cells[i, 9].Value != null ? worksheet.Cells[i, 9].Value.ToString() : "0";
                        dtrow["IsEnabled"] = worksheet.Cells[i, 10].Value != null ? worksheet.Cells[i, 10].Value : 0;
                        dtrow["SortOrder"] = worksheet.Cells[i, 11].Value != null ? worksheet.Cells[i, 11].Value.ToString() : "0";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while importing excel file : " + ex.Message);
            }
            return dtMasterData;
        }

        private int validations(InspectionMasterData masterData)
        {
            int flag = 0;
            try
            {
                double LSL = 0.0, USL = 0.0, UWL = 0.0, LWL = 0.0, UOL = 0.0, LOL = 0.0, Mean = 0.0;
                if (!(double.TryParse(masterData.specification, out Mean)))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Specification Mean is Mandatory')", true);
                    flag = 1;
                }
                if(string.IsNullOrEmpty(masterData.DataTemplate))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Data Template is Mandatory')", true);
                    flag = 1;
                }
                if ((double.TryParse(masterData.LSL, out LSL) && double.TryParse(masterData.USL, out USL)))
                {
                    if (USL.Equals("0"))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('USL cannot be Zero')", true);
                        flag = 1;
                    }
                    else if (USL.Equals("0"))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('USL cannot be Zero')", true);
                        flag = 1;
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('LSL and USL are Mandatory')", true);
                    flag = 1;
                }
                if (LSL > Mean)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Specification Should be greater than LSL')", true);
                    flag = 1;
                }
                else if (Mean > USL)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('USL greater than Specification')", true);
                    flag = 1;
                }

                //if (double.TryParse(masterData.UpperOperatingZone, out UOL) && double.TryParse(masterData.UpperWarningZone, out UWL) && double.TryParse(masterData.LowerWarningZone, out LWL) && double.TryParse(masterData.LowerOperatingZone, out LOL) && UOL != 0 && LOL != 0 && UWL != 0 && LWL != 0)
                //{

                //    if (LSL > LWL)
                //    {
                //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Lower Warning Zone Should be greater than LSL')", true);
                //        flag = 1;
                //    }
                //    else if (LWL > LOL)
                //    {
                //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Lower operating Zone Should be greater than Lower Warning Zone')", true);
                //        flag = 1;
                //    }

                //    else if (LOL > Mean)
                //    {
                //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Specification Should be greater than Lower Operating Zone')", true);
                //        flag = 1;
                //    }
                //    else if (Mean > UOL)
                //    {
                //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Upper Operating Zone Should be greater than Specification')", true);
                //        flag = 1;
                //    }
                //    else if (UOL > UWL)
                //    {
                //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Upper Warning Zone Should be greater than Upper Operating Zone')", true);
                //        flag = 1;
                //    }
                //    else if (UWL > USL)
                //    {
                //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('USL Should be greater than Upper Warning Zone')", true);
                //        flag = 1;
                //    }
                //}
                //else
                //{
                //    if (double.TryParse(masterData.UpperOperatingZone, out UOL) && double.TryParse(masterData.UpperOperatingZone, out UOL) && UOL != 0 && LOL != 0)
                //    {
                //        if (UOL == 0 && UWL == 0)
                //        {
                //            flag = 1;
                //        }
                //        if (Mean > UOL)
                //        {
                //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Upper Operating Zone Should be greater than Specification')", true);
                //            flag = 1;
                //        }
                //        if (UOL > UWL)
                //        {
                //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Upper Warning Zone Should be greater than Upper Operating Zone')", true);
                //            flag = 1;
                //        }
                //        else if (USL < UWL)
                //        {
                //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('USL Should be greater than Upper Warning Zone')", true);
                //            flag = 1;
                //        }
                //    }
                //    if (double.TryParse(masterData.LowerWarningZone, out LWL) && double.TryParse(masterData.UpperWarningZone, out UWL) && LWL != 0 && UWL != 0)
                //    {
                //        if (LWL == 0 && UWL == 0)
                //        {
                //            flag = 1;
                //        }
                //        if (LSL > LWL)
                //        {
                //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Lower Warning Zone Should be greater than LSL')", true);
                //            flag = 1;
                //        }
                //        else if (USL < UWL)
                //        {
                //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('USL Should be greater than Upper Warning Zone')", true);
                //            flag = 1;
                //        }
                //    }
                //}

            }
            catch (Exception ex)
            {

            }
            return flag;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int flag = 0;
            try
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    InspectionMasterData inspectionData = new InspectionMasterData();
                    inspectionData.MachineID = string.Empty;
                    inspectionData.ComponentID = (GridView1.Rows[i].FindControl("hdnCompID") as HiddenField).Value;
                    inspectionData.OperationID = (GridView1.Rows[i].FindControl("hdnOperationID") as HiddenField).Value;
                    inspectionData.CharID = (GridView1.Rows[i].FindControl("txtCharID") as TextBox).Text;
                    inspectionData.CharCode = (GridView1.Rows[i].FindControl("txtCharactersticCode") as TextBox).Text;
                    inspectionData.specification = (GridView1.Rows[i].FindControl("txtSpecification") as TextBox).Text;
                    inspectionData.LSL = (GridView1.Rows[i].FindControl("txtLSL") as TextBox).Text;
                    inspectionData.USL = (GridView1.Rows[i].FindControl("txtUSL") as TextBox).Text;
                    inspectionData.UpperWarningZone = string.Empty;
                    inspectionData.UpperOperatingZone = string.Empty;
                    inspectionData.LowerOperatingZone = string.Empty;
                    inspectionData.LowerWarningZone = string.Empty;
                    inspectionData.SampleSize = (GridView1.Rows[i].FindControl("txtSampleSize") as TextBox).Text;
                    inspectionData.DataTemplate = (GridView1.Rows[i].FindControl("ddlDataTemp") as DropDownList).SelectedValue.ToString();
                    inspectionData.IsEnabled = (GridView1.Rows[i].FindControl("chkEnabled") as CheckBox).Checked;
                    inspectionData.SortOrder= (GridView1.Rows[i].FindControl("txtSortOrder") as TextBox).Text;
                    inspectionData.UOM = string.Empty;
                    inspectionData.InspectedBy = string.Empty;
                    if (string.IsNullOrEmpty(inspectionData.ComponentID))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please provide Component ID')", true);
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

                    flag+= validations(inspectionData);
                    if(flag<=0)
                    {
                        TMPTrakDataBase.InsertUpdateData(inspectionData);
                    }
                }
                if (flag<=0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Inspection Data successfully updated!!')", true);
                }
                GetInspectionData();
            }
            catch (Exception ex)
            {
                Console.Write("Error while inserting data - " + ex.Message);
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int flag = 0;
                if (e.CommandName.Equals("AddNew"))
                {

                    if ((GridView1.FooterRow.FindControl("txtCharIDfooter") as TextBox).Text == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Enter CharacterID')", true);
                        return;
                    }
                    else
                    {
                        string ComponentID = ddlComponent.SelectedValue.ToString() == "ComponentAll" ? "" : ddlComponent.SelectedValue.ToString();
                        string OperationID = ddlOperation.SelectedValue.ToString() == "OperationAll" ? "" : ddlOperation.SelectedValue.ToString();

                        InspectionMasterData inspectionData = new InspectionMasterData();
                        inspectionData.MachineID = string.Empty;
                        inspectionData.ComponentID = ComponentID;
                        inspectionData.OperationID = OperationID;
                        inspectionData.CharID = (GridView1.FooterRow.FindControl("txtCharIDfooter") as TextBox).Text;
                        inspectionData.CharCode = (GridView1.FooterRow.FindControl("txtCharCodefooter") as TextBox).Text;
                        inspectionData.specification = (GridView1.FooterRow.FindControl("txtSpecificationfooter") as TextBox).Text;
                        inspectionData.LSL = (GridView1.FooterRow.FindControl("txtLSLfooter") as TextBox).Text;
                        inspectionData.USL = (GridView1.FooterRow.FindControl("txtUSLfooter") as TextBox).Text;
                        inspectionData.UpperOperatingZone = string.Empty;
                        inspectionData.UpperWarningZone = string.Empty;
                        inspectionData.LowerOperatingZone = string.Empty;
                        inspectionData.LowerWarningZone = string.Empty;
                        inspectionData.SampleSize = (GridView1.FooterRow.FindControl("txtSampleSizefooter") as TextBox).Text;
                        inspectionData.DataTemplate = (GridView1.FooterRow.FindControl("ddlDataTempfooter") as DropDownList).SelectedValue.ToString();
                        inspectionData.IsEnabled = (GridView1.FooterRow.FindControl("chkEnabledfooter") as CheckBox).Checked;
                        inspectionData.SortOrder = (GridView1.FooterRow.FindControl("txtSortOrderfooter") as TextBox).Text;
                        inspectionData.UOM = string.Empty;
                        inspectionData.InspectedBy = string.Empty;
                        if (string.IsNullOrEmpty(inspectionData.ComponentID) || string.IsNullOrEmpty(inspectionData.OperationID) || string.IsNullOrEmpty(inspectionData.CharID))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select Comp ,Operation and Characteristic ID')", true);
                            return;
                        }
                        flag = validations(inspectionData);

                        if (flag <= 0)
                        {
                            TMPTrakDataBase.InsertUpdateData(inspectionData);
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Inspection Data successfully updated!!')", true);

                        }
                        GetInspectionData();

                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write("Error while inserting data - " + ex.Message);
            }
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ComponentID = (GridView1.Rows[e.RowIndex].FindControl("hdnCompID") as HiddenField).Value;
            OperationID = (GridView1.Rows[e.RowIndex].FindControl("hdnOperationID") as HiddenField).Value;
            charID = (GridView1.Rows[e.RowIndex].FindControl("txtCharID") as TextBox).Text;
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
                TMPTrakDataBase.DeleteInspectionData("", ComponentID, OperationID, charID, charCode, out result);
                GridView1.EditIndex = -1;
                GetInspectionData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
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
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string src, dst = string.Empty;
                string reportName = "InspectionMaster_Vulkan.xlsx";
                src = GetReportPath(reportName);

                string tempfileName = "InspectionMasterReport" + "_" + Guid.NewGuid() + ".xlsx";
                dst = Path.Combine(appPath, "Temp", SafeFileName(tempfileName));

                if (!File.Exists(src))
                {
                    Logger.WriteDebugLog("Inspection Master report Log by day-Excel - \n " + src);
                }
                FileInfo newFile = new FileInfo(src);
                ExcelPackage pck = new ExcelPackage(newFile, true);
                var wsDts = pck.Workbook.Worksheets["Sheet1"];
                int row = 2, j = 1;
                List<InspectionMasterData> data = (List<InspectionMasterData>)Session["InpectionData"];
                if (data.Count > 0 && data != null)
                {
                    foreach (InspectionMasterData item in data)
                    {
                        j = 1;
                        wsDts.Cells[row, j].Value = item.ComponentID;
                        j++;
                        wsDts.Cells[row, j].Value = item.OperationID;
                        j++;
                        wsDts.Cells[row, j].Value = item.CharID;
                        j++;
                        wsDts.Cells[row, j].Value = item.CharCode;
                        j++;
                        wsDts.Cells[row, j].Value = TMPTrakGenerateReport.getDoubleValue(item.LSL);
                        j++;
                        //wsDts.Cells[row, j].Value = TMPTrakGenerateReport.getDoubleValue(item.LowerWarningZone);
                        //j++;
                        //wsDts.Cells[row, j].Value = TMPTrakGenerateReport.getDoubleValue(item.LowerOperatingZone);
                        //j++;
                        wsDts.Cells[row, j].Value = TMPTrakGenerateReport.getDoubleValue(item.specification);
                        j++;
                        //wsDts.Cells[row, j].Value = TMPTrakGenerateReport.getDoubleValue(item.UpperOperatingZone);
                        //j++;
                        //wsDts.Cells[row, j].Value = TMPTrakGenerateReport.getDoubleValue(item.UpperWarningZone);
                        //j++;
                        wsDts.Cells[row, j].Value = TMPTrakGenerateReport.getDoubleValue(item.USL);
                        j++;
                        wsDts.Cells[row, j].Value = item.DataTemplate;
                        j++;
                        wsDts.Cells[row, j].Value = TMPTrakGenerateReport.getDoubleValue(item.SampleSize);
                        j++;
                        wsDts.Cells[row, j].Value = item.IsEnabled;
                        j++;
                        wsDts.Cells[row, j].Value = HelperClassGeneric.getIntValueFromString(item.SortOrder);
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
                    wsDts.Cells[1, 1, row, j].AutoFitColumns();
                    DownloadMultipleFile(dst, pck.GetAsByteArray());
                    Logger.WriteDebugLog("Inspection Master Report.");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void GetComponentData()
        {
            List<string> CopyComplst = TMPTrakDataBase.GetComponentIdList();
            CopyComplst = CopyComplst.AsEnumerable().Where(x => x != ddlComponent.SelectedValue.ToString()).ToList();
            ddlMultiComponent.DataSource = CopyComplst;
            ddlMultiComponent.DataBind();
        }
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Copyviewmode", "CopyView();", true);       
                GetComponentData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnSaveCopy_Click(object sender, EventArgs e)
        {
            bool status = false;
            List<InspectionMasterData> data = (List<InspectionMasterData>)Session["InpectionData"];
            if (data != null && data.Count > 0)
            {
                string ComponentID = string.Empty;
                foreach (System.Web.UI.WebControls.ListItem Item in ddlMultiComponent.Items)
                {
                    ComponentID += Item.Selected ? "'" + Item.Value + "'," : "";
                }
                ComponentID = ComponentID.Trim(',');
                InspectionMasterData inspectionData = new InspectionMasterData();
                inspectionData.ComponentID = ddlComponent.SelectedValue.ToString();
                inspectionData.SelectedComponentID = ComponentID;
                if (ddlOperation.SelectedValue.ToString().Equals(""))
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "warningMsg", "showWarningMsg('Please select Operation ID')", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "CopyCancelmode", "HideCopyModel();", true);
                }
                else
                {
                    inspectionData.OperationID = ddlOperation.SelectedValue.ToString();
                    TMPTrakDataBase.CopyComponentData(inspectionData, out status);
                }

                if (status)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Copied Successfully.')", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "CopyCancelmode", "HideCopyModel();", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "CopyCancelmode", "HideCopyModel();", true);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "warningMsg", "showWarningMsg('No data to Copy')", true);
            }
            GetInspectionData();
        }

       
        public static string GetReportPath(string reportName)
        {
            string src;
            src = Path.Combine(appPath, reportName);
            return src;
        }
        protected void btnTemplateExport_Click(object sender, EventArgs e)
        {
            try
            {
                string Filename = string.Empty;
                string templatefile = string.Empty;
                Filename = "ImportTemplate_Inspection.xlsx";
                string Source = GetReportPath(Filename);
                string Template = string.Empty;
                Template = "ImportInspectionTemplate_" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("ImportInspectionTemplate- \n " + Source);
                }
                FileInfo newFile = new FileInfo(Source);
                ExcelPackage Excel = new ExcelPackage(newFile, true);
                DownloadMultipleFile(destination, Excel.GetAsByteArray());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if(GridView1.Rows.Count>0)
                {
                    btnSave.Visible = true;
                    foreach (GridViewRow gv in GridView1.Rows)
                    {
                        gv.Cells[2].Enabled = true;
                        gv.Cells[3].Enabled = true;
                        gv.Cells[4].Enabled = true;
                        gv.Cells[7].Enabled = true;
                        gv.Cells[10].Enabled = true;
                        gv.Cells[11].Enabled = true;
                        gv.Cells[12].Enabled = true;
                        gv.Cells[13].Enabled = true;
                        gv.Cells[14].Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
    }
}