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

namespace Web_TPMTrakDashboard.PrecisionEngineering
{
    public partial class DailyCheckSheetMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                else
                    SessionClear.ClearSession();
                BindMachineID();
                lblCopySourceMachine.Text = ddlMachineID.SelectedValue;
                List<string> machineList = new List<string>();
                foreach (ListItem item in ddlMachineID.Items)
                {
                    if (!item.Value.Equals(lblCopySourceMachine.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        machineList.Add(item.Value);
                    }
                }
                lbCopyDestMachine.DataSource = machineList;
                lbCopyDestMachine.DataBind();
                BindData();
            }
        }

        private void BindGroupID()
        {
            try
            {
                List<GroupDefintion> list = DataBaseAccess.GetMaintenanceGroupDetails_Precision();
                //ddlGroupID.DataSource = list.AsEnumerable().Select(x => x.GroupID).Distinct().ToList();
                //ddlGroupID.DataBind();
                //ddlGroupID.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void BindMachineID()
        {
            try
            {
                List<string> list = DataBaseAccess.GetMachines();
                ddlMachineID.DataSource = list;
                ddlMachineID.DataBind();
                ddlMachineID.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void BindData()
        {
            try
            {
                List<DailyCheckSheet_Precision> list = DataBaseAccess.GetDailyMaintenanceDetails_Precision(ddlMachineID.SelectedValue.ToString());
                if (list.Count > 0 && list != null)
                {
                    lvDailyCheckSheetMaster.DataSource = list;
                    lvDailyCheckSheetMaster.DataBind();
                }
                else
                {
                    list.Add(new DailyCheckSheet_Precision { });
                    lvDailyCheckSheetMaster.DataSource = list;
                    lvDailyCheckSheetMaster.DataBind();
                    lvDailyCheckSheetMaster.Items[0].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void lnkCheckPoint_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkRemove = (LinkButton)sender;
                int Val = DataBaseAccess.deleteCheckPointID_Precision(lnkRemove.CommandArgument.ToString(),ddlMachineID.SelectedValue.ToString());
                if (Val > 0)
                {
                    HelperClassGeneric.openSuccessModal(this, "Deleted successfully!!");
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int IsUpdated = 0;
                DailyCheckSheet_Precision data = new DailyCheckSheet_Precision();
                //data.GroupID = ddlGroupID.SelectedValue.ToString();
                data.MachineID = ddlMachineID.SelectedValue.ToString();
                if (hdfCondition.Value.Equals("Save", StringComparison.OrdinalIgnoreCase))
                {
                    data.CheckPointID = (lvDailyCheckSheetMaster.InsertItem.FindControl("txtCheckPointID") as TextBox).Text;
                    data.CheckPointDesc = (lvDailyCheckSheetMaster.InsertItem.FindControl("txtInsertCheckPointDesc") as TextBox).Text;
                    data.CheckPointDescInHindi = (lvDailyCheckSheetMaster.InsertItem.FindControl("txtInsertCheckPointDescInHindi") as TextBox).Text;
                    data.FrequencyOrder = (lvDailyCheckSheetMaster.InsertItem.FindControl("ddlFrequency") as DropDownList).SelectedValue;
                    data.Frequency = (lvDailyCheckSheetMaster.InsertItem.FindControl("ddlFrequency") as DropDownList).SelectedItem.Text;
                    if (string.IsNullOrEmpty(data.CheckPointID))
                    {
                        HelperClassGeneric.openWarningToastrModal(this, "Check Point ID cannot be Empty!!");
                        return;
                    }
                    foreach (ListViewItem row in lvDailyCheckSheetMaster.Items)
                    {
                        if (data.CheckPointID.Equals((row.FindControl("lblCheckPointID") as Label).Text))
                        {
                            HelperClassGeneric.openWarningToastrModal(this, "Check PointID Already Exists!!");
                            return;
                        }
                    }
                    IsUpdated = DataBaseAccess.SaveDailyMaintenanceCheckSheet(data);
                }
                else
                {
                    foreach (ListViewItem row in lvDailyCheckSheetMaster.Items)
                    {
                        string hdfUpade = ((HiddenField)row.FindControl("hdfUpdate")).Value;
                        if (hdfUpade.Equals("update", StringComparison.OrdinalIgnoreCase))
                        {
                            data.CheckPointID = (row.FindControl("lblCheckPointID") as Label).Text;
                            data.CheckPointDesc = (row.FindControl("txtCheckPointDesc") as TextBox).Text;
                            data.CheckPointDescInHindi = (row.FindControl("txtCheckPointDescInHindi") as TextBox).Text;
                            data.Frequency = (row.FindControl("ddlFrequency") as DropDownList).SelectedItem.Text;
                            data.FrequencyOrder = (row.FindControl("ddlFrequency") as DropDownList).SelectedValue;
                            IsUpdated = DataBaseAccess.SaveDailyMaintenanceCheckSheet(data);
                        }
                    }
                }

                if (IsUpdated > 0)
                {
                    HelperClassGeneric.openSuccessModal(this, "Saved Successfully.");
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        //protected void ddlGroupID_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindData();
        //}

        static string appPath = HttpContext.Current.Server.MapPath("~/PrecisionEngineering/ReportTemplates");
        public static string GetReportPath(string reportName)
        {
            string src;
            src = Path.Combine(appPath, reportName);
            return src;
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
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        #endregion
        protected void btnTemplateExport_Click(object sender, EventArgs e)
        {
            try
            {
                string Filename = string.Empty;
                string templatefile = string.Empty;
                Filename = "ImportTemplate_DailyCheckSheetMaster.xlsx";
                string Source = GetReportPath(Filename);
                string Template = string.Empty;
                Template = "ImportDailyCheckSheetMaster_" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("ImportDailyCheckSheetMasterTemplate- \n " + Source);
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

        protected void lnkImport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
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
                        dt = GetDataTableFromFile(fileupload);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            List<DailyCheckSheet_Precision> ListmasterData = new List<DailyCheckSheet_Precision>();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DailyCheckSheet_Precision masterData = new DailyCheckSheet_Precision();
                                masterData.MachineID = dt.Rows[i]["MachineID"].ToString().Trim();
                                masterData.CheckPointID = dt.Rows[i]["CheckPointID"].ToString().Trim();
                                masterData.CheckPointDesc = dt.Rows[i]["CheckPointDesc"].ToString().Trim();
                                masterData.CheckPointDescInHindi = dt.Rows[i]["CheckPointDescInHindi"].ToString().Trim();
                                masterData.Frequency = dt.Rows[i]["Frequency"].ToString().Trim();
                                if (masterData.Frequency == "Daily")
                                {
                                    masterData.FrequencyOrder ="1";
                                }
                                else if (masterData.Frequency == "Weekly")
                                {
                                    masterData.FrequencyOrder ="2";
                                }
                                else if (masterData.Frequency == "Monthly")
                                {
                                    masterData.FrequencyOrder ="3";
                                }
                                if (string.IsNullOrEmpty(masterData.MachineID))
                                {
                                    HelperClassGeneric.openWarningToastrModal(this, "Machine ID cannot be Empty!!");
                                    return;
                                }
                                if (string.IsNullOrEmpty(masterData.CheckPointID))
                                {
                                    HelperClassGeneric.openWarningToastrModal(this, "Check Point ID cannot be Empty!!");
                                    return;
                                }
                                ListmasterData.Add(masterData);
                                flag+=DataBaseAccess.SaveDailyMaintenanceCheckSheet(masterData);
                            }
                            if (flag > 0)
                            {
                                HelperClassGeneric.openSuccessModal(this, "Data Saved Successfully!!");
                            }
                            BindData();
                        }
                        else
                        {
                            HelperClassGeneric.openWarningToastrModal(this, "Import failed. Empty excel file!!");
                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
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
                //dtMasterData.Columns.Add("GroupID");
                dtMasterData.Columns.Add("MachineID");
                dtMasterData.Columns.Add("CheckPointID");
                dtMasterData.Columns.Add("CheckPointDesc");
                dtMasterData.Columns.Add("CheckPointDescInHindi");
                dtMasterData.Columns.Add("Frequency");
                //dtMasterData.Columns.Add("FreqWiseSortOrder");

                int i = 0;
                for (i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                {
                    if (worksheet.Cells[i, 1].Value != null && !string.IsNullOrEmpty(worksheet.Cells[i, 1].Value.ToString()))
                    {
                        DataRow dtrow = dtMasterData.Rows.Add();
                        //dtrow["GroupID"] = worksheet.Cells[i, 1].Value.ToString();
                        dtrow["MachineID"] = worksheet.Cells[i, 1].Value.ToString();
                        dtrow["CheckPointID"] = worksheet.Cells[i, 2].Value.ToString();
                        dtrow["CheckPointDesc"] = worksheet.Cells[i, 3].Value.ToString();
                        dtrow["CheckPointDescInHindi"] = worksheet.Cells[i, 4].Value!=""? "": worksheet.Cells[i, 4].Value.ToString();
                        dtrow["Frequency"] = worksheet.Cells[i, 5].Value.ToString();
                        //dtrow["FreqWiseSortOrder"] = worksheet.Cells[i, 6].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while importing excel file : " + ex.Message);
            }
            return dtMasterData;
        }

        protected void ddlMachineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCopySourceMachine.Text = ddlMachineID.SelectedValue;
            List<string> machineList = new List<string>();
            foreach (ListItem item in ddlMachineID.Items)
            {
                if (!item.Value.Equals(lblCopySourceMachine.Text, StringComparison.OrdinalIgnoreCase))
                {
                    machineList.Add(item.Value);
                }
            }
            lbCopyDestMachine.DataSource = machineList;
            lbCopyDestMachine.DataBind();
            BindData();
        }

        protected void lvDailyCheckSheetMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    DropDownList ddlFrequency = e.Item.FindControl("ddlFrequency") as DropDownList;
                    HiddenField hdnFrequencyOrder = e.Item.FindControl("hdnFrequencyOrder") as HiddenField;
                    ddlFrequency.SelectedValue = hdnFrequencyOrder.Value;
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnCopyConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string sourceMachine = lblCopySourceMachine.Text;
                string destinationMachine = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbCopyDestMachine);
                int result = DataBaseAccess.CopyDailyMaintenanceCheckSheetData(sourceMachine, destinationMachine);
                if (result >0)
                {
                    HelperClassGeneric.openSuccessModal(this, "Record copied Successfully.");
                    HelperClassGeneric.clearModal(this);
                }
                else
                {
                    HelperClassGeneric.openModal(this, "copyModal", false);
                    HelperClassGeneric.openInsertErrorModal(this);
                    return;
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}