using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.LnTOdisha.Model;
using static Web_TPMTrakDashboard.LnTOdisha.Model.LnTOdishaDTO;
using System.Data;
using OfficeOpenXml2;
using System.Text;

namespace Web_TPMTrakDashboard.LnTOdisha
{
    public partial class PMMasterLnT : System.Web.UI.Page
    {
        List<ListItem> listFrequency = new List<ListItem>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ShiftList"] = null;
                Session["PMCategory"] = null;
                BindMachineIds();
                BindFrequency(ddlFrequency, true);
                btnView_Click(null, null);
            }
        }
        private void BindCategory(DropDownList ddl, string value)
        {
            try
            {
                List<string> list = new List<string>();
                if (Session["PMCategory"] == null)
                {
                    Session["PMCategory"] = list = LnTOdishaDBAccess.getPMActivityCategoryLnT();
                }
                list = Session["PMCategory"] as List<string>;
                ddl.DataSource = list;
                ddl.DataBind();
                if (ddl.Items.Count > 0)
                {
                    ddl.Items.Insert(0, "");
                }
                if (value != "")
                {
                    HelperClassGeneric.setDropdownValue(ddl, value);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindShift(ListBox listbox, string value)
        {
            try
            {
                List<ListItem> shiftList = new List<ListItem>();
                if (Session["ShiftList"] == null)
                {
                    Session["ShiftList"] = shiftList = DataBaseAccess.GetAllShiftIds();
                }
                shiftList = Session["ShiftList"] as List<ListItem>;
                listbox.DataSource = shiftList;
                listbox.DataTextField = "Text";
                listbox.DataValueField = "Value";
                listbox.DataBind();
                if (value != "")
                {
                    var shifts = value.Split(',').ToList();
                    foreach (string shift in shifts)
                    {
                        if (listbox.Items.FindByValue(shift) != null)
                        {
                            listbox.Items.FindByValue(shift).Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindMachineIds()
        {
            try
            {
                List<string> machines = LnTOdishaDBAccess.GetMachineInfoForPM();
                ddlMachineId.DataSource = machines;
                ddlMachineId.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        private void BindFrequency(DropDownList ddl, bool IsAllRequired)
        {
            try
            {
                List<ListItem> list = DataBaseAccess.getFrequencyForActivityMasters();
                if (IsAllRequired)
                {
                    list.Insert(0, new ListItem { Text = "All", Value = "" });
                }
                ddl.DataSource = list;
                ddl.DataTextField = "Text";
                ddl.DataValueField = "Value";
                ddl.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                BindActivityDetails();
            }
            catch (Exception ex)
            {

            }
        }
        private void BindActivityDetails()
        {
            try
            {
                listFrequency = DataBaseAccess.getFrequencyForActivityMasters();
                List<ActivityInfoLnTEntity> activityInfoList = new List<ActivityInfoLnTEntity>();
                activityInfoList = LnTOdishaDBAccess.getPMActivityMasterDetails(ddlFrequency.SelectedItem.ToString(), ddlMachineId.SelectedValue.ToString());
                int flag = 0;
                if (activityInfoList.Count == 0)
                {
                    flag = 1;
                    activityInfoList.Add(new ActivityInfoLnTEntity());
                }
                gvActivityDetails.DataSource = activityInfoList;
                gvActivityDetails.DataBind();
                BindCategory((gvActivityDetails.FooterRow.FindControl("ddlCategory") as DropDownList), "");
                if (flag == 1)
                {
                    gvActivityDetails.Rows[0].Visible = false;
                }
                gvActivityDetails.FooterRow.Visible = false;
                btnNew.Visible = true;
                btnCancel.Visible = false;
            }
            catch (Exception ex)
            {

            }
        }
        protected void gvActivityDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string hdnValue = "";
                    DropDownList ddl = new DropDownList();

                    hdnValue = (e.Row.FindControl("hdnFrequencyID") as HiddenField).Value;
                    ddl = (e.Row.FindControl("ddlFrequency") as DropDownList);
                    ddl.DataSource = listFrequency;
                    ddl.DataValueField = "Value";
                    ddl.DataTextField = "Text";
                    ddl.DataBind();
                    if (hdnValue != "")
                    {
                        if (ddl.Items.FindByValue(hdnValue) != null)
                        {
                            ddl.SelectedValue = hdnValue;
                        }

                    }


                    hdnValue = (e.Row.FindControl("hdnCategory") as HiddenField).Value;
                    BindCategory((e.Row.FindControl("ddlCategory") as DropDownList), hdnValue);

                    hdnValue = (e.Row.FindControl("hfIsActivityHasFile") as HiddenField).Value;
                    //if (Convert.ToBoolean(hdnValue) == true)
                    //{
                    //    (e.Row.FindControl("lbUploadedFile") as LinkButton).Visible = true;
                    //}
                    //else
                    //{
                    //    (e.Row.FindControl("lbUploadedFile") as LinkButton).Visible = false;
                    //}

                    string freq = (e.Row.FindControl("hdnFrequency") as HiddenField).Value;
                    ListBox listBox = (e.Row.FindControl("lbShift") as ListBox);
                    hdnValue = (e.Row.FindControl("hdnShifts") as HiddenField).Value;
                    BindShift(listBox, hdnValue);
                    if (freq.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                    {
                        listBox.Visible = true;
                    }
                    else
                    {
                        listBox.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            gvActivityDetails.FooterRow.Visible = true;
            gvActivityDetails.FooterRow.Focus();
            btnNew.Visible = false;
            btnCancel.Visible = true;
            DropDownList ddl = gvActivityDetails.FooterRow.FindControl("ddlFrequency") as DropDownList;
            BindFrequency(ddl, false);
            BindShift(gvActivityDetails.FooterRow.FindControl("lbShift") as ListBox, "");
            ddlFrequency_SelectedIndexChangedFooter(null, null);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "No Parts", "setScrollToBottotm()", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool IsUpdated = false;
            try
            {
                ActivityInfoLnTEntity activityInfoEntity = new ActivityInfoLnTEntity();
                if (gvActivityDetails.FooterRow.Visible)
                {
                    activityInfoEntity = new ActivityInfoLnTEntity();
                    bool hasFile = false;
                    string fileName = "";
                    byte[] contents = null;

                    activityInfoEntity.MachineID = ddlMachineId.SelectedValue;
                    activityInfoEntity.Activity = (gvActivityDetails.FooterRow.FindControl("txtActvity") as TextBox).Text;
                    activityInfoEntity.FrequencyID = (gvActivityDetails.FooterRow.FindControl("ddlFrequency") as DropDownList).SelectedValue;
                    activityInfoEntity.Frequency = (gvActivityDetails.FooterRow.FindControl("ddlFrequency") as DropDownList).SelectedItem.ToString();
                    activityInfoEntity.AllotedTime = (gvActivityDetails.FooterRow.FindControl("txtAllotedTime") as TextBox).Text.Trim().ToString();
                    if (string.IsNullOrEmpty(activityInfoEntity.Activity))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "No Parts", "alert('Activity cannot be null or empty. !!')", true);
                        return;
                    }
                    if (string.IsNullOrEmpty(activityInfoEntity.Frequency))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "No Parts", "alert('Frequency cannot be null or empty. !!')", true);
                        return;
                    }
                    activityInfoEntity.AllotedTime = (gvActivityDetails.FooterRow.FindControl("txtAllotedTime") as TextBox).Text;
                    activityInfoEntity.Criteria = (gvActivityDetails.FooterRow.FindControl("txtCriteria") as TextBox).Text;
                    ListBox listBox = (gvActivityDetails.FooterRow.FindControl("lbShift") as ListBox);
                    string shifts = "";
                    if (listBox.Visible)
                    {
                        foreach (ListItem item in listBox.Items)
                        {
                            if (item.Selected)
                            {
                                if (shifts == "") shifts += item.Value; else shifts += "," + item.Value;
                            }
                        }
                    }
                    activityInfoEntity.Shifts = shifts;
                    activityInfoEntity.Category = (gvActivityDetails.FooterRow.FindControl("ddlCategory") as DropDownList).SelectedValue;
                    FileUpload fileUpload = (gvActivityDetails.FooterRow.FindControl("fileUpload") as FileUpload);
                    if (fileUpload.HasFile)
                    {
                        if (Path.GetExtension(fileUpload.FileName) != ".pdf")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "No Parts", "alert('Select only pdf file.')", true);
                            return;
                        }
                        hasFile = true;
                        fileName = System.IO.Path.GetFileName(fileUpload.FileName);
                        Stream fs = fileUpload.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(fs); //reads the binary files  
                        contents = br.ReadBytes((Int32)fs.Length);


                        //FileStream fStream = File.OpenRead(path);
                        //contents = new byte[fStream.Length];
                        //fStream.Read(contents, 0, (int)fStream.Length);
                        //fStream.Close();

                    }
                    LnTOdishaDBAccess.InsertPMActivtyMasterDetails(Convert.ToInt32(activityInfoEntity.ActivityID), activityInfoEntity.Activity, activityInfoEntity.FrequencyID, fileName, contents, hasFile, activityInfoEntity.MachineID, activityInfoEntity.Criteria, activityInfoEntity.Shifts, activityInfoEntity.Category, activityInfoEntity.AllotedTime, out IsUpdated);
                }
                else
                {
                    for (int i = 0; i < gvActivityDetails.Rows.Count; i++)
                    {
                        if ((gvActivityDetails.Rows[i].FindControl("hdnUpdate") as HiddenField).Value == "update")
                        {
                            activityInfoEntity = new ActivityInfoLnTEntity();
                            bool hasFile = false;
                            string fileName = "";
                            byte[] contents = null;

                            activityInfoEntity.MachineID = (gvActivityDetails.Rows[i].FindControl("hfMachineID") as HiddenField).Value;
                            activityInfoEntity.Activity = (gvActivityDetails.Rows[i].FindControl("txtActvity") as TextBox).Text;
                            activityInfoEntity.ActivityID = (gvActivityDetails.Rows[i].FindControl("hfActivityID") as HiddenField).Value;
                            activityInfoEntity.FrequencyID = (gvActivityDetails.Rows[i].FindControl("ddlFrequency") as DropDownList).SelectedValue;
                            activityInfoEntity.Frequency = (gvActivityDetails.Rows[i].FindControl("ddlFrequency") as DropDownList).SelectedItem.ToString();
                            activityInfoEntity.AllotedTime = (gvActivityDetails.Rows[i].FindControl("txtAllotedTime") as TextBox).Text;
                            activityInfoEntity.Criteria = (gvActivityDetails.Rows[i].FindControl("txtCriteria") as TextBox).Text;
                            ListBox listBox = (gvActivityDetails.Rows[i].FindControl("lbShift") as ListBox);
                            string shifts = "";
                            if (listBox.Visible)
                            {
                                foreach (ListItem item in listBox.Items)
                                {
                                    if (item.Selected)
                                    {
                                        if (shifts == "") shifts += item.Value; else shifts += "," + item.Value;
                                    }
                                }
                            }
                            activityInfoEntity.Shifts = shifts;
                            activityInfoEntity.Category = (gvActivityDetails.Rows[i].FindControl("ddlCategory") as DropDownList).SelectedValue;
                            if (string.IsNullOrEmpty(activityInfoEntity.Activity))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "No Parts", "alert('Activity cannot be null or empty. !!')", true);
                                return;
                            }
                            if (string.IsNullOrEmpty(activityInfoEntity.Frequency))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "No Parts", "alert('Frequency cannot be null or empty. !!')", true);
                                return;
                            }
                            FileUpload fileUpload = (gvActivityDetails.Rows[i].FindControl("fileUpload") as FileUpload);
                            if (fileUpload.HasFile)
                            {
                                if (Path.GetExtension(fileUpload.FileName) != ".pdf")
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "No Parts", "alert('Select only pdf file.')", true);
                                    return;
                                }
                                hasFile = true;
                                fileName = System.IO.Path.GetFileName(fileUpload.FileName);
                                Stream fs = fileUpload.PostedFile.InputStream;
                                BinaryReader br = new BinaryReader(fs); //reads the binary files  
                                contents = br.ReadBytes((Int32)fs.Length);

                                //fileName = System.IO.Path.GetFileName((gvActivityDetails.Rows[i].FindControl("fileUpload") as FileUpload).FileName);
                                //FileStream fStream = File.OpenRead((gvActivityDetails.Rows[i].FindControl("fileUpload") as FileUpload).FileName);
                                //contents = new byte[fStream.Length];
                                //fStream.Read(contents, 0, (int)fStream.Length);
                                //fStream.Close();
                            }
                            LnTOdishaDBAccess.InsertPMActivtyMasterDetails(Convert.ToInt32(activityInfoEntity.ActivityID), activityInfoEntity.Activity, activityInfoEntity.FrequencyID, fileName, contents, hasFile, activityInfoEntity.MachineID, activityInfoEntity.Criteria, activityInfoEntity.Shifts, activityInfoEntity.Category, activityInfoEntity.AllotedTime, out IsUpdated);
                        }
                    }
                }

                if (IsUpdated)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "No Parts", "alert('Details added / Updated successfully.')", true);
                    BindActivityDetails();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            BindActivityDetails();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool isDeleted = false;
                for (int i = 0; i < gvActivityDetails.Rows.Count; i++)
                {
                    if ((gvActivityDetails.Rows[i].FindControl("chkDeleteSelection") as CheckBox).Checked)
                    {
                        LnTOdishaDBAccess.DeletePMActivityMasterDetails(Convert.ToInt32((gvActivityDetails.Rows[i].FindControl("hfActivityID") as HiddenField).Value), (gvActivityDetails.Rows[i].FindControl("hfMachineID") as HiddenField).Value, out isDeleted);
                    }
                }
                if (isDeleted)
                {
                    BindActivityDetails();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "No Parts", "alert('Selected record  deleted successfully.')", true);

                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void lbUploadedFile_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton linkButton = (LinkButton)sender;
                GridViewRow gridViewRow = (GridViewRow)linkButton.NamingContainer;
                string activity = (gridViewRow.FindControl("txtActvity") as TextBox).Text;
                string activityId = (gridViewRow.FindControl("hfActivityID") as HiddenField).Value;
                string machineId = (gridViewRow.FindControl("hfMachineID") as HiddenField).Value;
                //string filepath = DataBaseAccess.GetActivityFilePathFromDB(Convert.ToInt32(activityId));
                byte[] filepath = DataBaseAccess.GetActivityFilePathFromDB(Convert.ToInt32(activityId), machineId);


                //Response.Clear();
                //Response.Buffer = true;
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-disposition", "attachment;filename=d.pdf"); // to open file prompt Box open or Save file  
                //Response.Charset = "";
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.BinaryWrite((byte[])filepath);
                //Response.End();

                string base64String = Convert.ToBase64String(filepath, 0, filepath.Length);
                var pdfUrl = "data:application/pdf;base64," + base64String;
                iframeDocument.Attributes.Add("src", pdfUrl);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "No Parts", "openFileUploadModal()", true);
                //frame.Attributes.Add("src", "D:\\d.pdf");
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = ((sender as DropDownList).NamingContainer as GridViewRow);
                if ((row.FindControl("ddlFrequency") as DropDownList).SelectedItem.Text.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                {
                    (row.FindControl("lbShift") as ListBox).Visible = true;
                }
                else
                {
                    (row.FindControl("lbShift") as ListBox).Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlFrequency_SelectedIndexChangedFooter(object sender, EventArgs e)
        {
            try
            {
                if ((gvActivityDetails.FooterRow.FindControl("ddlFrequency") as DropDownList).SelectedItem.Text.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                {
                    (gvActivityDetails.FooterRow.FindControl("lbShift") as ListBox).Visible = true;
                }
                else
                {
                    (gvActivityDetails.FooterRow.FindControl("lbShift") as ListBox).Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnPMCategory_Click(object sender, EventArgs e)
        {

        }

        protected void BtnImport_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            int InsertedRows = 0;
            try
            {
                if (FileImport.HasFile)
                {
                    string fileName = FileImport.FileName.ToString();

                    if (!Directory.Exists(Server.MapPath("ImportFilesLnTOdisha")))
                    {
                        Directory.CreateDirectory(Server.MapPath("ImportFilesLnTOdisha"));
                    }
                    FileImport.SaveAs(Server.MapPath("ImportFilesLnTOdisha") + "\\" + fileName);
                    string errorMsg = "";
                    GetExcelDataToList(fileName, out InsertedRows, out dt, out errorMsg);

                    if (dt.Rows.Count > 0)
                    {
                        LnTOdishaDBAccess.InsertPMMasterImportData(dt);

                        LnTOdishaDBAccess.BulkInsertPMActivityData();

                        if (errorMsg != "")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "openWarningModalsss", "openWarningModalMsg('" + errorMsg + "');", true);
                        }
                        else
                        {
                            HelperClassGeneric.openSuccessModal(this, "Import Successful.");
                        }

                        BindActivityDetails();
                    }
                    else
                    {
                        HelperClassGeneric.openWarningToastrModal(this, "No Data to Import.");
                    }
                }
                else
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Import file is not Selected.");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private string GetExcelDataToList(string fileName, out int InsertedRows, out DataTable dt, out string errorMsg)
        {
            InsertedRows = 0;
            dt = new DataTable();
            errorMsg = "";
            List<string> list_Categories = new List<string>();
            using (var excel = new ExcelPackage())
            {
                using (var stream = File.OpenRead(Server.MapPath("ImportFilesLnTOdisha") + "\\" + fileName))
                {
                    excel.Load(stream);
                }
                var wb = excel.Workbook;
                if (wb != null)
                {
                    try
                    {
                        if (Session["PMCategory"] != null)
                        {
                            list_Categories = Session["PMCategory"] as List<string>;
                        }

                        var wsScheduleData = wb.Worksheets[0];
                        dt.Columns.Add("MachineID", typeof(string));
                        dt.Columns.Add("Activity", typeof(string));
                        dt.Columns.Add("Frequency", typeof(string));
                        dt.Columns.Add("Category", typeof(string));
                        dt.Columns.Add("Criteria", typeof(string));
                        dt.Columns.Add("AllotedTime", typeof(string));

                        int rowStart = 2;
                        int LastRow = GetLastUsedRow(wsScheduleData);
                        var temp_machineID = "";
                        for (int i = rowStart; i <= LastRow; i++)
                        {
                            string machineID = "";
                            string Activity = "";
                            string Frequency = "";
                            string Category = "";
                            string Criteria = "";
                            string AllottedTime = "";
                            int cols = 1;

                            if (wsScheduleData.Cells[i, cols].Value == null)
                            {
                                errorMsg += $"Row: {i}[Machine ID is missing], ";
                                continue;
                            }
                            else
                            {
                                machineID = wsScheduleData.Cells[i, cols].Value.ToString().Trim();
                            }
                            cols++;

                            if (wsScheduleData.Cells[i, cols].Value == null)
                            {
                                errorMsg += $"Row: {i}[Activity is missing], ";
                                continue;
                            }
                            else
                                Activity = wsScheduleData.Cells[i, cols].Value.ToString().Trim();
                            cols++;

                            Frequency = wsScheduleData.Cells[i, cols].Value.ToString().Trim();
                            cols++;

                            if (wsScheduleData.Cells[i, cols].Value == null)
                            {
                                errorMsg += $"Row: {i}[Category is missing], ";
                                continue;
                            }
                            else
                            {
                                Category = wsScheduleData.Cells[i, cols].Value.ToString().Trim();

                                if (list_Categories.IndexOf(Category) == -1)
                                {
                                    errorMsg += $"Row: {i}[Category: {Category} is Not present in Master], ";
                                    continue;
                                }
                            }
                            cols++;

                            Criteria = wsScheduleData.Cells[i, cols].Value.ToString().Trim();
                            cols++;

                            AllottedTime = wsScheduleData.Cells[i, cols].Value.ToString().Trim();

                            var tableRow = dt.NewRow();
                            tableRow[0] = machineID;
                            tableRow[1] = Activity;
                            tableRow[2] = Frequency;
                            tableRow[3] = Category;
                            tableRow[4] = Criteria;
                            tableRow[5] = AllottedTime;
                            dt.Rows.Add(tableRow);
                            InsertedRows++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteErrorLog("GetExcelDataToList: " + ex.Message);
                    }
                }
            }
            return "Successful";
        }

        private int GetLastUsedRow(ExcelWorksheet worksheet)
        {
            var row = worksheet.Dimension.End.Row;
            while (row >= 1)
            {
                var range = worksheet.Cells[row, 1, row, worksheet.Dimension.End.Column];
                if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                {
                    break;
                }
                row--;
            }

            return row;
        }

        protected void btnTemplateExport_Click(object sender, EventArgs e)
        {
            try
            {
                string Filename = string.Empty;
                string templatefile = string.Empty;
                Filename = "PMMaster_Import_LnTOdisha.xlsx";
                string Source = string.Empty;
                Source = GetReportPath(Filename);
                string Template = string.Empty;
                Template = "PMMaster_Import_LnTOdisha" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("PMMaster_Import_LnTOdisha- \n " + Source);
                }
                FileInfo newFile = new FileInfo(Source);
                ExcelPackage Excel = new ExcelPackage(newFile, true);
                DownloadFile(destination, Excel.GetAsByteArray());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        static string appPath = HttpContext.Current.Server.MapPath("~/LnTOdisha");
        public static string GetReportPath(string reportName)
        {
            string src;
            src = Path.Combine(appPath, "ReportTemplate", reportName);
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
        private static void DownloadFile(string filename, byte[] bytearray)
        {

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Path.GetFileName(filename) + "\"");
            HttpContext.Current.Response.OutputStream.Write(bytearray, 0, bytearray.Length);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }
    }
}