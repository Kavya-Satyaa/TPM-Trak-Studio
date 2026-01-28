using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.LnTOdisha.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.LnTOdisha
{
    public partial class PMTransactionLnT : System.Web.UI.Page
    {
        List<ActivityInfoEntity> activityInfoList = new List<ActivityInfoEntity>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PMTransactionData"] = null;
                hdnLoginDisplayStatus.Value = "show";
                BindMachineIds();
                //BindFrequency(ddlFrequency, false);
                //BindCategory();
                hdnCurrentDateTime.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //hdnCurrentPrevNextParam.Value = "current";
                // txtYear.Text = DateTime.Now.Year.ToString();
                //txtFromDateExport.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                //txtToDateExport.Text = DateTime.Now.ToString("dd-MM-yyyy");
                DateTime now = DateTime.Now;
                DateTime startDate = new DateTime(now.Year, now.Month, 1);
                txtFromDate.Text = startDate.ToString("dd-MM-yyyy");
                txtToDate.Text = startDate.AddMonths(1).AddDays(-1).ToString("dd-MM-yyyy");
                BindAtivityTransactionDetails();
            }
        }
        //private void BindCategory()
        //{
        //    try
        //    {
        //        List<string> list = DataBaseAccess.getPMActivityCategory();
        //        if (list.Count > 0)
        //        {
        //            list.Insert(0, "All");
        //        }
        //        ddlCategory.DataSource = list;
        //        ddlCategory.DataBind();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
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
        //private void BindFrequency(DropDownList ddl, bool IsAllRequired)
        //{
        //    try
        //    {
        //        List<ListItem> list = DataBaseAccess.getFrequencyValueForActivityMasters();
        //        if (IsAllRequired)
        //        {
        //            list.Insert(0, new ListItem { Text = "All", Value = "" });
        //        }
        //        Session["PMFrequency"] = list;
        //        ddl.DataSource = list;
        //        ddl.DataTextField = "Text";
        //        ddl.DataValueField = "Text";
        //        ddl.DataBind();

        //        cbFrequency.DataSource = list;
        //        cbFrequency.DataValueField = "Value";
        //        cbFrequency.DataTextField = "Text";
        //        cbFrequency.DataBind();
        //        foreach (ListItem item in cbFrequency.Items)
        //        {
        //            item.Selected = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        private void BindAtivityTransactionDetails()
        {
            DataTable dt_HeaderDetails = new DataTable();
            DataTable dt_Remarks = new DataTable();
            string startDate = "", endDate = "";
            try
            {
                #region --old Code--
                //string frequency = ddlFrequency.SelectedItem.Text;
                //string frequencyValue = "";
                //if (Session["PMFrequency"] != null)
                //{
                //    List<ListItem> listItems = (List<ListItem>)Session["PMFrequency"];
                //    frequencyValue = listItems.Where(x => x.Text.Equals(frequency, StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).FirstOrDefault();
                //}
                //if (frequency.Equals("3 Month", StringComparison.OrdinalIgnoreCase) || frequency.Equals("6 Month", StringComparison.OrdinalIgnoreCase))
                //{
                //    int monthNo = DataBaseAccess.getFinancialsMonth();
                //    DateTime startDateTime = new DateTime(Convert.ToInt32(txtYear.Text), monthNo, 1);
                //    DateTime endDateTime = startDateTime.AddMonths(12).AddDays(-1);
                //    startDate = startDateTime.ToString("dd-MM-yyyy");
                //    endDate = endDateTime.ToString("dd-MM-yyyy");
                //}
                //else
                //{

                //}
                //DataTable dtFileData = LnTOdishaDBAccess.getActivityFileInformationDetails(ddlMachineId.SelectedValue, frequency);
                //Session["ActivityFileData"] = dtFileData;
                //if (ddlFrequency.SelectedValue.Equals("shift", StringComparison.OrdinalIgnoreCase))
                //{
                //    string tempCheckpoint = "";
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        if (dt.Rows[i]["Activity"].ToString() != tempCheckpoint)
                //        {
                //            dt.Rows[i]["RowSpanCount"] = dt.AsEnumerable().Where(k => k["Activity"].ToString().Equals(dt.Rows[i]["Activity"].ToString(), StringComparison.OrdinalIgnoreCase)).Distinct().Count();
                //        }
                //        else
                //        {
                //            dt.Rows[i]["RowSpanCount"] = 0;
                //        }
                //        tempCheckpoint = dt.Rows[i]["Activity"].ToString();
                //    }
                //}


                // Extra Columns 

                //tempfield = new TemplateField();
                //tempfield.HeaderText = "Criteria";
                //gvPMTransactionDetails.Columns.Add(tempfield);

                //tempfield = new TemplateField();
                //tempfield.HeaderText = "Category";
                //gvPMTransactionDetails.Columns.Add(tempfield);

                //tempfield = new TemplateField();
                //tempfield.HeaderText = "Last Update By";
                //gvPMTransactionDetails.Columns.Add(tempfield);

                //tempfield = new TemplateField();
                //tempfield.HeaderText = "Shift";
                //gvPMTransactionDetails.Columns.Add(tempfield);


                //tempfield = new TemplateField();
                //tempfield.HeaderText = "File Name";
                //gvPMTransactionDetails.Columns.Add(tempfield);
                #endregion

                startDate = txtFromDate.Text;
                endDate = txtToDate.Text;

                DataTable dt = LnTOdishaDBAccess.getActivityTransactionData(ddlMachineId.SelectedValue, startDate, endDate, out dt_Remarks, out dt_HeaderDetails);
                Session["dt_Remarks"] = dt_Remarks;

                DataTable dtFileData = LnTOdishaDBAccess.getActivityFileInformationDetails_LnT(ddlMachineId.SelectedValue);
                Session["ActivityFileData"] = dtFileData;
                activityInfoList = LnTOdishaDBAccess.GetAllActivityForGrid_LnT(ddlMachineId.SelectedValue);
                Session["PMTransactionData"] = dt;
                gvPMTransactionDetails.Columns.Clear();
                TemplateField tempfield = new TemplateField();
                tempfield.HeaderText = "Sl. No.";
                gvPMTransactionDetails.Columns.Add(tempfield);

                tempfield = new TemplateField();
                tempfield.HeaderText = "Activity";
                gvPMTransactionDetails.Columns.Add(tempfield);

                tempfield = new TemplateField();
                tempfield.HeaderText = "Frequency";
                gvPMTransactionDetails.Columns.Add(tempfield);

                tempfield = new TemplateField();
                tempfield.HeaderText = "Last Update";
                gvPMTransactionDetails.Columns.Add(tempfield);

                dt.Columns.Add("LastUpdated", typeof(string));

                dt.AsEnumerable().ToList<DataRow>().ForEach(x =>
                {
                    x["LastUpdated"] = string.IsNullOrEmpty(x["LastUpdate"].ToString()) ? "" : Convert.ToDateTime(x["LastUpdate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                });

                if (dt.Columns.IndexOf("SaveFlag") == -1)
                {
                    var dRow1 = dt.NewRow();
                    var dRow2 = dt.NewRow();
                    dRow1["ActivityName"] = "Team Leader:"; 
                    dRow2["ActivityName"] = "CRAFTS";

                    var LastUpdatedTeamLeadName = dt_HeaderDetails.AsEnumerable().Where(x => Convert.ToBoolean(x["Status"]) == true).FirstOrDefault();
                    var LastUpdatedCraftsName = dt_HeaderDetails.AsEnumerable().Where(x => Convert.ToBoolean(x["Status"]) == true).FirstOrDefault();
                    if (LastUpdatedTeamLeadName != null)
                        dRow1["LastUpdated"] = LastUpdatedTeamLeadName["TeamLeader"].ToString();
                    if (LastUpdatedCraftsName != null)
                        dRow2["LastUpdated"] = LastUpdatedCraftsName["CrewMemberName"].ToString();


                    for (int i = 6; i < dt.Columns.Count-2; i++)
                    {
                        string ColumnDate = Util.GetDateTime(dt.Columns[i].ColumnName.ToString()).ToString("dd-MM-yyyy");
                        tempfield = new TemplateField();
                        tempfield.HeaderText = ColumnDate;
                        gvPMTransactionDetails.Columns.Add(tempfield);

                        dRow1[dt.Columns[i].ColumnName.ToString()] = dt_HeaderDetails.Rows.Count > 0 ? (dt_HeaderDetails.AsEnumerable().Where(x => Convert.ToDateTime(x["PMDate"].ToString()).ToString("dd-MM-yyyy") == ColumnDate).Select(x => x["TeamLeader"].ToString()).FirstOrDefault().ToString()) : "";

                        dRow2[dt.Columns[i].ColumnName.ToString()] = dt_HeaderDetails.Rows.Count > 0 ? (dt_HeaderDetails.AsEnumerable().Where(x => Convert.ToDateTime(x["PMDate"].ToString()).ToString("dd-MM-yyyy") == ColumnDate).Select(x => x["CrewMemberName"].ToString()).FirstOrDefault().ToString()) : "";

                    }
                    dt.Rows.InsertAt(dRow1, 0);
                    dt.Rows.InsertAt(dRow2, 1);
                }
                else
                    dt.Rows.RemoveAt(0);
                gvPMTransactionDetails.DataSource = dt;
                gvPMTransactionDetails.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                //hdnCurrentDateTime.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //hdnCurrentPrevNextParam.Value = "current";
                Session["PMTransactionData"] = null;
                BindAtivityTransactionDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            BindAtivityTransactionDetails();
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["PMTransactionData"] == null)
                {
                    BindAtivityTransactionDetails();
                }
                DataTable sourceDt = Session["PMTransactionData"] as DataTable;
                List<DateTime> lst = new List<DateTime>();
                for (int i = 6; i < sourceDt.Columns.Count - 2; i++)
                {
                    DateTime colDt = new DateTime();
                    DateTime.TryParse(sourceDt.Columns[i].ColumnName, out colDt);
                    lst.Add(Util.GetDateTime(colDt.ToString()));
                }
                if (lst.Count == 0)
                {
                    return;
                }
                DateTime currentSelectedDate = lst.Min();
                //if (lst.Count >= 11)
                //{
                //    hdnDateForLastPreviousNextRecord.Value = currentSelectedDate.ToString();
                //}
                hdnCurrentDateTime.Value = currentSelectedDate.ToString();
                hdnCurrentPrevNextParam.Value = "Previous";
                BindAtivityTransactionDetails();
            }
            catch (Exception ex)
            { }

        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["PMTransactionData"] == null)
                {
                    BindAtivityTransactionDetails();
                }
                DataTable sourceDt = Session["PMTransactionData"] as DataTable;
                List<DateTime> lst = new List<DateTime>();
                for (int i = 6; i < sourceDt.Columns.Count - 2; i++)
                {
                    DateTime colDt = new DateTime();
                    DateTime.TryParse(sourceDt.Columns[i].ColumnName, out colDt);
                    lst.Add(Util.GetDateTime(colDt.ToString()));
                }
                if (lst.Count > 0)
                {
                    hdnCurrentPrevNextParam.Value = "Next";
                }
                else
                {
                    return;
                }

                BindAtivityTransactionDetails();
            }
            catch (Exception ex)
            { }

        }

        protected void gvPMTransactionDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string uniqueCode = "", activity = "", ActivityID = (e.Row.DataItem as DataRowView).Row["ActivityID"].ToString(), SINO = (e.Row.DataItem as DataRowView).Row["SINO"].ToString();
                    DataTable dtFileData = Session["ActivityFileData"] as DataTable;

                    Label label = new Label();
                    label.ID = "lblSlNo";
                    label.Text = SINO;
                    e.Row.Cells[0].Controls.Add(label);

                    HiddenField hdn = new HiddenField();
                    hdn.ID = "hdnActivityID";
                    hdn.Value = ActivityID;
                    uniqueCode = hdn.Value;
                    e.Row.Cells[1].Controls.Add(hdn);

                    label = new Label();
                    label.ID = "lblActivity";
                    activity = (e.Row.DataItem as DataRowView).Row["ActivityName"].ToString();
                    label.Text = activity;
                    

                     label = new Label();
                    //label.Attributes.Add("class", "lnkFileName");
                    activity = (e.Row.DataItem as DataRowView).Row["ActivityName"].ToString();
                    label.Text = activity;
                    string hasFile = "False";
                    if (activityInfoList.Where(x => x.ActivityID == ActivityID).Select(x => x.ActivityHasFile).FirstOrDefault())
                    {
                        label.Style["color"] = "#0abaff";
                        hasFile = "True";
                    }
                    //label.Attributes.Add("onclick", "return showActivityFile(this);");
                    e.Row.Cells[1].Attributes.Add("onclick", "return showActivityFile(this);");
                    e.Row.Cells[1].Controls.Add(label);

                    hdn = new HiddenField();
                    hdn.ID = "hfHasFile";
                    hdn.Value = hasFile;
                    e.Row.Cells[1].Controls.Add(hdn);


                    //FileUpload fileUpload = new FileUpload();
                    //fileUpload.CssClass = "fileUploader";
                    //fileUpload.Attributes.Add("onchange", "FUChange(this);");
                    //e.Row.Cells[1].Controls.Add(fileUpload);

                    string fileNameOld = "", fileInBase64Old = "";
                    if (dtFileData.Rows.Count > 0)
                    {
                        var row = dtFileData.AsEnumerable().Where(k => k["ActivityID"].ToString().Equals(ActivityID, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (row != null && !string.IsNullOrEmpty(row["FileName"].ToString()))
                        {
                            fileNameOld = row["FileName"].ToString();
                            byte[] bytes = (byte[])row["ActivityFile"];
                            fileInBase64Old = Convert.ToBase64String(bytes);
                        }
                    }
                    hdn = new HiddenField();
                    hdn.ID = "hdnFileNameOld";
                    hdn.Value = fileNameOld;
                    e.Row.Cells[1].Controls.Add(hdn);
                    hdn = new HiddenField();
                    hdn.ID = "hdnFileNameInBase64Old";
                    hdn.Value = fileInBase64Old;
                    e.Row.Cells[1].Controls.Add(hdn);

                   

                    label = new Label();
                    label.ID = "lblFrequency";
                    label.Text = (e.Row.DataItem as DataRowView).Row["Frequency"].ToString();
                    e.Row.Cells[2].Controls.Add(label);

                    label = new Label();
                    label.ID = "lblLastUpdate";
                    label.Text = (e.Row.DataItem as DataRowView).Row["LastUpdated"].ToString();
                    e.Row.Cells[3].Controls.Add(label);


                    LinkButton linkBtn = new LinkButton();
                    int datatbleColStartCount = 6;
                    for (int col = 4; col < e.Row.Cells.Count; col++)
                    {
                        string remark = "", Action = "";

                        DataTable dtExtraFields = Session["dt_Remarks"] as DataTable;
                        if (dtExtraFields.Rows.Count > 0)
                        {
                            string headerName = gvPMTransactionDetails.HeaderRow.Cells[col].Text;
                            var row = dtExtraFields.AsEnumerable().Where(x => (x["ActivityID"].ToString() == ActivityID && Util.GetDateTime(x["PMDate"].ToString()).ToString("dd-MM-yyyy") == headerName)).FirstOrDefault();
                            if (row != null)
                            {
                                remark = row["Remarks"].ToString();
                                Action = row["Action"].ToString();
                            }
                        }

                        e.Row.Cells[col].Attributes["title"] = (Action.Equals("Text", StringComparison.OrdinalIgnoreCase) ? remark : Action);

                        label = new Label();
                        label.CssClass = "lblRemarks hiddenlbl";
                        label.Text = remark;
                        e.Row.Cells[col].Controls.Add(label);

                        label = new Label();
                        label.CssClass = "lblAction hiddenlbl";
                        label.Text = Action;
                        e.Row.Cells[col].Controls.Add(label);
                        label = new Label();
                        label.ID = "lblPMStatus";
                        string status = (e.Row.DataItem as DataRowView).Row[datatbleColStartCount].ToString();

                        e.Row.Cells[col].Controls.Add(label);
                        if (status == "Pending")
                        {
                            label.Text = "P";
                            label.ForeColor = Color.Red;
                            label.Font.Bold = true;
                            label.Font.Underline = true;
                        }
                        else if (status == "Completed")
                        {
                            label.Text = "C";
                            label.ForeColor = Color.Green;
                            label.Font.Bold = true;
                            linkBtn = new LinkButton();
                            linkBtn.ID = "lnkRemarksTargetTime";
                            //linkBtn.CssClass = "glyphicon glyphicon-list-alt activityInfoLink";
                            //linkBtn.OnClientClick = "return showActivityRemarksAndTargetTime(this);";
                            e.Row.Cells[col].Controls.Add(linkBtn);
                        }
                        else if (status == "UnderLine")
                        {
                            label.Text = "U";
                            label.ForeColor = Color.FromArgb(247, 184, 10);
                            label.Font.Bold = true;
                        }
                        else
                            label.Text = status;

                        HiddenField hdUpdate = new HiddenField();
                        hdUpdate.ID = "hdnUpdate";
                        hdUpdate.Value = "";
                        e.Row.Cells[col].Controls.Add(hdUpdate);
                        datatbleColStartCount++;
                    }
                }

                #region -- OLD CODE --
                //else if (e.Row.RowType == DataControlRowType.Footer)
                //{

                //    //e.Row.Cells[0].Text = "Document";
                //    //e.Row.Cells[0].ColumnSpan = 6;
                //    //for (int i = 1; i < 7; i++)
                //    //{
                //    //    e.Row.Cells[0].Style.Add("display", "none");
                //    //}

                //    for (int col = 4; col < e.Row.Cells.Count; col++)
                //    {
                //        FileUpload fileUpload = new FileUpload();
                //        fileUpload.CssClass = "fileUploader";
                //        fileUpload.Attributes.Add("onchange", "FUChange(this);");
                //        e.Row.Cells[col].Controls.Add(fileUpload);

                //        string headerName = gvPMTransactionDetails.HeaderRow.Cells[col].Text;

                //        HiddenField hdn = new HiddenField();
                //        hdn.ID = "hdnFileName";
                //        hdn.Value = "";
                //        e.Row.Cells[col].Controls.Add(hdn);
                //        hdn = new HiddenField();
                //        hdn.ID = "hdnFileNameInBase64";
                //        hdn.Value = "";
                //        e.Row.Cells[col].Controls.Add(hdn);



                //        DataTable dtFileData = Session["ActivityFileData"] as DataTable;
                //        string fileNameOld = "", fileInBase64Old = "";
                //        if (dtFileData.Rows.Count > 0)
                //        {
                //            var row = dtFileData.AsEnumerable().Where(k => k["DisplayActivityStart"].ToString().Equals(headerName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                //            if (row != null && !string.IsNullOrEmpty(row["FileName"].ToString()))
                //            {
                //                fileNameOld = row["FileName"].ToString();
                //                byte[] bytes = (byte[])row["FileData"];
                //                fileInBase64Old = Convert.ToBase64String(bytes);
                //            }
                //        }
                //        hdn = new HiddenField();
                //        hdn.ID = "hdnFileNameOld";
                //        hdn.Value = fileNameOld;
                //        e.Row.Cells[col].Controls.Add(hdn);
                //        hdn = new HiddenField();
                //        hdn.ID = "hdnFileNameInBase64Old";
                //        hdn.Value = fileInBase64Old;
                //        e.Row.Cells[col].Controls.Add(hdn);

                //        HtmlAnchor linkButton = new HtmlAnchor();
                //        linkButton.Attributes.Add("class", "lnkFileName");
                //        linkButton.InnerText = fileNameOld;
                //        linkButton.Title = fileNameOld;
                //        linkButton.Attributes.Add("onclick", "return showActivityFile(this);");
                //        e.Row.Cells[col].Controls.Add(linkButton);
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static bool getActvityTransactionAuthorization(string username, string password)
        {
            bool IsAuthorized = false;
            try
            {
                IsAuthorized = DataBaseAccess.getActvityTransactionLoginAuthorization(username, password);
            }
            catch (Exception ex)
            {

            }
            return IsAuthorized;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string getFile(string activityid, string machineid)
        {
            string pdfUrl = "";
            try
            {
                byte[] filepath = DataBaseAccess.GetActivityFilePathFromDB(Convert.ToInt32(activityid), machineid);
                string base64String = Convert.ToBase64String(filepath, 0, filepath.Length);
                pdfUrl = "data:application/pdf;base64," + base64String;
            }
            catch (Exception ex)
            {

            }
            return pdfUrl;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static bool saveActvityTransactionData(string activity, string frequency, string header, string machineid, string updatedBy, string shiftId, string criteria, string status, string action, string remarks, string category, string shiftName)
        {
            bool isUpdated = false;

            try
            {
                DateTime logicalDateTime = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")));
                TimeSpan logicalStartTime = new TimeSpan(logicalDateTime.Hour, logicalDateTime.Minute, logicalDateTime.Second);
                if (frequency.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                {
                    List<string> shiftTimings = DataBaseAccess.GetCurrentShiftTime(shiftName, Convert.ToDateTime(header).ToString("yyyy-MM-dd"));
                    if (shiftTimings.Count > 0)
                    {
                        logicalDateTime = Util.GetDateTime(shiftTimings[0]);
                        logicalStartTime = new TimeSpan(logicalDateTime.Hour, logicalDateTime.Minute, logicalDateTime.Second);
                    }
                }

                DateTime.TryParseExact(header, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime activityTS);
                activityTS = activityTS.Add(logicalStartTime);
                isUpdated = DataBaseAccess.saveActvityTransactionDetails(activity, frequency, activityTS.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), machineid, updatedBy, shiftId, criteria, status, action, remarks, category);

            }
            catch (Exception ex)
            { }
            return isUpdated;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static int saveActivityFileDetails(string machineID, string frequency, string header, string fileName, string fileInBase64)
        {
            int result = 0;

            try
            {
                result = DataBaseAccess.insertActivityFileInformationDetails(machineID, frequency, header, fileName, fileInBase64);
            }
            catch (Exception ex)
            { }
            return result;
        }
        protected void btnActivityTransactionExport_Click(object sender, EventArgs e)
        {
            string Generated = "";
            try
            {
                //string Year = (string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? DateTime.Now.Year : Util.GetDateTime(txtFromDate.Text.Trim()).Year).ToString();
                Generated = LnTGenerateReport.GeneratePMReport(ddlMachineId.SelectedValue.Trim(), "", txtFromDate.Text, txtToDate.Text);

                if (Generated.Equals("TemplateNotFound", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openWarningToastrModal(this, "Template Not Found.");
                else if (Generated.Equals("NodataFound", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openWarningToastrModal(this, "No Date Found.");
                else if (Generated.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openSuccessModal(this, "Generated Succesfully.");
                BindAtivityTransactionDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}