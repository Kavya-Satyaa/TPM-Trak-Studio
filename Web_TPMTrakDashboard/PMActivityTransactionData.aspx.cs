using OfficeOpenXml;
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
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class PMActivityTransactionData : System.Web.UI.Page
    {
        List<ActivityInfoEntity> activityInfoList = new List<ActivityInfoEntity>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PMTransactionData"] = null;
                hdnLoginDisplayStatus.Value = "show";
                BindMachineIds();
                BindFrequency(ddlFrequency, false);
                BindCategory();
                hdnCurrentDateTime.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                hdnCurrentPrevNextParam.Value = "current";
                txtYear.Text = DateTime.Now.Year.ToString();
                txtFromDateExport.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDateExport.Text = DateTime.Now.ToString("dd-MM-yyyy");
                DateTime now = DateTime.Now;
                DateTime startDate = new DateTime(now.Year, now.Month, 1);
                txtFromDate.Text = startDate.ToString("dd-MM-yyyy");
                txtToDate.Text = startDate.AddMonths(1).AddDays(-1).ToString("dd-MM-yyyy");
                BindAtivityTransactionDetails();
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["CumiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    btnActivityTransactionExport.Visible = false;
                }
                else
                {
                    btnActivityTransactionExport.Visible = true;
                }
            }
        }
        private void BindCategory()
        {
            try
            {
                List<string> list = DataBaseAccess.getPMActivityCategory();
                if (list.Count > 0)
                {
                    list.Insert(0, "All");
                }
                ddlCategory.DataSource = list;
                ddlCategory.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        private void BindMachineIds()
        {
            try
            {
                List<string> machines = DataBaseAccess.GetMachineInfoForPM();
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
                List<ListItem> list = DataBaseAccess.getFrequencyValueForActivityMasters();
                if (IsAllRequired)
                {
                    list.Insert(0, new ListItem { Text = "All", Value = "" });
                }
                Session["PMFrequency"] = list;
                ddl.DataSource = list;
                ddl.DataTextField = "Text";
                ddl.DataValueField = "Text";
                ddl.DataBind();

                cbFrequency.DataSource = list;
                cbFrequency.DataValueField = "Value";
                cbFrequency.DataTextField = "Text";
                cbFrequency.DataBind();
                foreach (ListItem item in cbFrequency.Items)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void BindAtivityTransactionDetails()
        {
            try
            {

                string frequency = ddlFrequency.SelectedItem.Text;
                //  string frequencyValue = ddlFrequency.SelectedValue;
                string frequencyValue = "";
                if (Session["PMFrequency"] != null)
                {
                    List<ListItem> listItems = (List<ListItem>)Session["PMFrequency"];
                    frequencyValue = listItems.Where(x => x.Text.Equals(frequency, StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).FirstOrDefault();
                }
                DataTable dtEntraFields = new DataTable();
                string startDate = "", endDate = "";
                if (frequency.Equals("3 Month", StringComparison.OrdinalIgnoreCase) || frequency.Equals("6 Month", StringComparison.OrdinalIgnoreCase))
                {
                    int monthNo = DataBaseAccess.getFinancialsMonth();
                    DateTime startDateTime = new DateTime(Convert.ToInt32(txtYear.Text), monthNo, 1);
                    DateTime endDateTime = startDateTime.AddMonths(12).AddDays(-1);
                    startDate = startDateTime.ToString("dd-MM-yyyy");
                    endDate = endDateTime.ToString("dd-MM-yyyy");
                }
                else
                {
                    startDate = txtFromDate.Text;
                    endDate = txtToDate.Text;
                }
                DataTable dt = DataBaseAccess.getActivityTransactionData(ddlMachineId.SelectedValue, frequency, frequencyValue, startDate, endDate, ddlCategory.SelectedValue, out dtEntraFields);
                Session["DtExtraFields"] = dtEntraFields;
                DataTable dtFileData = DataBaseAccess.getActivityFileInformationDetails(ddlMachineId.SelectedValue, frequency);
                Session["ActivityFileData"] = dtFileData;

                activityInfoList = DataBaseAccess.GetAllActivityForGrid("", ddlMachineId.SelectedValue);
                //if (dt.Rows.Count == 0)
                //{
                //    if (Session["PMTransactionData"] != null)
                //    {
                //        dt = Session["PMTransactionData"] as DataTable;
                //    }
                //}
                dt.Columns.Add("RowSpanCount", typeof(int));
                if (ddlFrequency.SelectedValue.Equals("shift", StringComparison.OrdinalIgnoreCase))
                {
                    string tempCheckpoint = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Activity"].ToString() != tempCheckpoint)
                        {
                            dt.Rows[i]["RowSpanCount"] = dt.AsEnumerable().Where(k => k["Activity"].ToString().Equals(dt.Rows[i]["Activity"].ToString(), StringComparison.OrdinalIgnoreCase)).Distinct().Count();
                        }
                        else
                        {
                            dt.Rows[i]["RowSpanCount"] = 0;
                        }
                        tempCheckpoint = dt.Rows[i]["Activity"].ToString();
                    }
                }
                Session["PMTransactionData"] = dt;
                //Session["PMTransactionRamrksData"] = dtRamrks;
                //Session["PMTransactionTargetTimeData"] = dtTargetTime;
                gvPMTransactionDetails.Columns.Clear();
                string[] columnNames = null;
                TemplateField tempfield = new TemplateField();
                tempfield.HeaderText = "Sl. No.";
                gvPMTransactionDetails.Columns.Add(tempfield);

                tempfield = new TemplateField();
                tempfield.HeaderText = "Activity";
                gvPMTransactionDetails.Columns.Add(tempfield);

                tempfield = new TemplateField();
                tempfield.HeaderText = "Criteria";
                gvPMTransactionDetails.Columns.Add(tempfield);

                tempfield = new TemplateField();
                tempfield.HeaderText = "Category";
                gvPMTransactionDetails.Columns.Add(tempfield);

                tempfield = new TemplateField();
                tempfield.HeaderText = "Last Update";
                gvPMTransactionDetails.Columns.Add(tempfield);

                tempfield = new TemplateField();
                tempfield.HeaderText = "Last Update By";
                gvPMTransactionDetails.Columns.Add(tempfield);

                tempfield = new TemplateField();
                tempfield.HeaderText = "Shift";
                gvPMTransactionDetails.Columns.Add(tempfield);


                //tempfield = new TemplateField();
                //tempfield.HeaderText = "File Name";
                //gvPMTransactionDetails.Columns.Add(tempfield);
                for (int i = 11; i < dt.Columns.Count - 1; i++)
                {
                    tempfield = new TemplateField();
                    tempfield.HeaderText = dt.Columns[i].ColumnName.ToString();
                    gvPMTransactionDetails.Columns.Add(tempfield);
                }
                gvPMTransactionDetails.DataSource = dt;
                gvPMTransactionDetails.DataBind();
            }
            catch (Exception ex)
            {

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
                for (int i = 5; i < sourceDt.Columns.Count - 1; i++)
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
                for (int i = 5; i < sourceDt.Columns.Count - 1; i++)
                {
                    DateTime colDt = new DateTime();
                    DateTime.TryParse(sourceDt.Columns[i].ColumnName, out colDt);
                    lst.Add(Util.GetDateTime(colDt.ToString()));
                }
                if (lst.Count > 0)
                {
                    DateTime currentSelectedDate = lst.Max();
                    if (ddlFrequency.SelectedItem.Text.ToLower().Contains("month"))
                    {
                        currentSelectedDate = currentSelectedDate.AddMonths(1).AddSeconds(-1);
                    }
                    if (ddlFrequency.SelectedItem.Text.ToLower().Contains("year"))
                    {
                        currentSelectedDate = currentSelectedDate.AddYears(1).AddSeconds(-1);
                    }
                    hdnCurrentDateTime.Value = currentSelectedDate.ToString();
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
                    string uniqueCode = "", activity = "";
                    Label label = new Label();
                    label.ID = "lblSlNo";
                    label.Text = (e.Row.DataItem as DataRowView).Row["Sl No"].ToString();
                    e.Row.Cells[0].Controls.Add(label);

                    label = new Label();
                    label.ID = "lblActivity";
                    activity = (e.Row.DataItem as DataRowView).Row["Activity"].ToString();
                    label.Text = activity;
                    string hasFile = "False";
                    if (activityInfoList.Where(x => x.ActivityID == (e.Row.DataItem as DataRowView).Row["ActivityID"].ToString()).Select(x => x.ActivityHasFile).FirstOrDefault())
                    {
                        label.ForeColor = Color.FromName("#0abaff");
                        hasFile = "True";
                    }
                    e.Row.Cells[1].Controls.Add(label);

                    HiddenField hdn = new HiddenField();
                    hdn.ID = "hdnActivityID";
                    hdn.Value = (e.Row.DataItem as DataRowView).Row["ActivityID"].ToString();
                    uniqueCode = hdn.Value;
                    e.Row.Cells[1].Controls.Add(hdn);

                    hdn = new HiddenField();
                    hdn.ID = "hfHasFile";
                    hdn.Value = hasFile;
                    uniqueCode = hdn.Value;
                    e.Row.Cells[1].Controls.Add(hdn);

                    hdn = new HiddenField();
                    hdn.ID = "hdnCriteria";
                    hdn.Value = (e.Row.DataItem as DataRowView).Row["Criteria"].ToString();
                    e.Row.Cells[1].Controls.Add(hdn);

                    hdn = new HiddenField();
                    hdn.ID = "hdnCategory";
                    hdn.Value = (e.Row.DataItem as DataRowView).Row["Category"].ToString();
                    e.Row.Cells[1].Controls.Add(hdn);

                    hdn = new HiddenField();
                    hdn.ID = "hdnFrequency";
                    hdn.Value = (e.Row.DataItem as DataRowView).Row["Frequency"].ToString();
                    e.Row.Cells[1].Controls.Add(hdn);

                    //label = new Label();
                    //label.ID = "lblFrequency";
                    //label.Text = (e.Row.DataItem as DataRowView).Row["Frequency"].ToString();
                    //e.Row.Cells[2].Controls.Add(label);
                    label = new Label();
                    label.ID = "lblCriteria";
                    label.Text = (e.Row.DataItem as DataRowView).Row["Criteria"].ToString();
                    e.Row.Cells[2].Controls.Add(label);

                    label = new Label();
                    label.ID = "lblCategory";
                    label.Text = (e.Row.DataItem as DataRowView).Row["Category"].ToString();
                    e.Row.Cells[3].Controls.Add(label);

                    label = new Label();
                    label.ID = "lblLastUpdate";
                    label.Text = (e.Row.DataItem as DataRowView).Row["LastUpdate"].ToString();
                    e.Row.Cells[4].Controls.Add(label);

                    label = new Label();
                    label.ID = "lblLastUpdatedBy";
                    label.Text = (e.Row.DataItem as DataRowView).Row["LastUpdatedBy"].ToString();
                    e.Row.Cells[5].Controls.Add(label);

                    label = new Label();
                    label.ID = "lblShift";
                    label.Text = (e.Row.DataItem as DataRowView).Row["ShiftName"].ToString();
                    e.Row.Cells[6].Controls.Add(label);
                    hdn = new HiddenField();
                    hdn.ID = "hdnShiftID";
                    hdn.Value = (e.Row.DataItem as DataRowView).Row["ShiftID"].ToString();
                    e.Row.Cells[6].Controls.Add(hdn);

                    if (ddlFrequency.SelectedValue.Equals("shift", StringComparison.OrdinalIgnoreCase))
                    {
                        int rowSpan = Convert.ToInt32((e.Row.DataItem as DataRowView).Row["RowSpanCount"]);
                        if (rowSpan == 0)
                        {
                            e.Row.Cells[0].Style.Add("display", "none");
                            e.Row.Cells[1].Style.Add("display", "none");
                            e.Row.Cells[2].Style.Add("display", "none");
                            e.Row.Cells[3].Style.Add("display", "none");
                            e.Row.Cells[4].Style.Add("display", "none");
                            e.Row.Cells[5].Style.Add("display", "none");
                        }
                        else
                        {
                            e.Row.Cells[0].RowSpan = rowSpan;
                            e.Row.Cells[1].RowSpan = rowSpan;
                            e.Row.Cells[2].RowSpan = rowSpan;
                            e.Row.Cells[3].RowSpan = rowSpan;
                            e.Row.Cells[4].RowSpan = rowSpan;
                            e.Row.Cells[5].RowSpan = rowSpan;
                        }
                    }

                    LinkButton linkBtn = new LinkButton();
                    int datatbleColStartCount = 11;
                    for (int col = 7; col < e.Row.Cells.Count; col++)
                    {
                        label = new Label();
                        label.ID = "lblPMStatus";
                        string status = (e.Row.DataItem as DataRowView).Row[datatbleColStartCount].ToString();
                        label.Text = status;
                        e.Row.Cells[col].Controls.Add(label);



                        if (status == "P")
                        {
                            label.ForeColor = System.Drawing.Color.Red;
                            label.Font.Bold = true;
                        }
                        else if (status == "C")
                        {
                            label.ForeColor = System.Drawing.Color.Green;
                            label.Font.Bold = true;
                            linkBtn = new LinkButton();
                            linkBtn.ID = "lnkRemarksTargetTime";
                            //linkBtn.CssClass = "glyphicon glyphicon-list-alt activityInfoLink";
                            //linkBtn.OnClientClick = "return showActivityRemarksAndTargetTime(this);";
                            e.Row.Cells[col].Controls.Add(linkBtn);
                        }
                        else if (status == "U")
                        {
                            label.ForeColor = System.Drawing.Color.FromArgb(247, 184, 10);
                            label.Font.Bold = true;
                        }

                        string remark = "", action = "", actionstatus = "";

                        DataTable dtExtraFields = Session["DtExtraFields"] as DataTable;
                        if (dtExtraFields.Rows.Count > 0)
                        {
                            string headerName = gvPMTransactionDetails.HeaderRow.Cells[col].Text;
                            var row = dtExtraFields.AsEnumerable().Where(k => k["DisplayActivityStart"].ToString().Equals(headerName, StringComparison.OrdinalIgnoreCase) && k["Activity"].ToString().Equals(activity, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                            if (row != null)
                            {
                                remark = row["Remarks"].ToString();
                                action = row["Action"].ToString();
                                actionstatus = row["Status"].ToString();
                            }
                        }
                        label = new Label();
                        label.CssClass = "lblRemarks hiddenlbl";
                        label.Text = remark;
                        //label.Style.Add("visibility", "hidden");
                        e.Row.Cells[col].Controls.Add(label);

                        label = new Label();
                        label.CssClass = "lblAction hiddenlbl";
                        label.Text = action;
                        //label.Style.Add("visibility", "hidden");
                        e.Row.Cells[col].Controls.Add(label);

                        label = new Label();
                        label.CssClass = "lblActionStatus hiddenlbl";
                        label.Text = actionstatus;
                        //label.Style.Add("visibility", "hidden");
                        e.Row.Cells[col].Controls.Add(label);


                        label = new Label();
                        label.CssClass = "lblEntryStatus hiddenlbl";
                        label.Text = "";
                        //label.Style.Add("visibility", "hidden");
                        e.Row.Cells[col].Controls.Add(label);

                        datatbleColStartCount++;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {

                    //e.Row.Cells[0].Text = "Document";
                    //e.Row.Cells[0].ColumnSpan = 6;
                    //for (int i = 1; i < 7; i++)
                    //{
                    //    e.Row.Cells[0].Style.Add("display", "none");
                    //}

                    for (int col = 7; col < e.Row.Cells.Count; col++)
                    {
                        FileUpload fileUpload = new FileUpload();
                        fileUpload.CssClass = "fileUploader";
                        fileUpload.Attributes.Add("onchange", "FUChange(this);");
                        e.Row.Cells[col].Controls.Add(fileUpload);

                        string headerName = gvPMTransactionDetails.HeaderRow.Cells[col].Text;

                        HiddenField hdn = new HiddenField();
                        hdn.ID = "hdnFileName";
                        hdn.Value = "";
                        e.Row.Cells[col].Controls.Add(hdn);
                        hdn = new HiddenField();
                        hdn.ID = "hdnFileNameInBase64";
                        hdn.Value = "";
                        e.Row.Cells[col].Controls.Add(hdn);



                        DataTable dtFileData = Session["ActivityFileData"] as DataTable;
                        string fileNameOld = "", fileInBase64Old = "";
                        if (dtFileData.Rows.Count > 0)
                        {
                            var row = dtFileData.AsEnumerable().Where(k => k["DisplayActivityStart"].ToString().Equals(headerName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                            if (row != null)
                            {
                                fileNameOld = row["FileName"].ToString();
                                byte[] bytes = (byte[])row["FileData"];
                                fileInBase64Old = Convert.ToBase64String(bytes);
                            }
                        }
                        hdn = new HiddenField();
                        hdn.ID = "hdnFileNameOld";
                        hdn.Value = fileNameOld;
                        e.Row.Cells[col].Controls.Add(hdn);
                        hdn = new HiddenField();
                        hdn.ID = "hdnFileNameInBase64Old";
                        hdn.Value = fileInBase64Old;
                        e.Row.Cells[col].Controls.Add(hdn);

                        HtmlAnchor linkButton = new HtmlAnchor();
                        linkButton.Attributes.Add("class", "lnkFileName");
                        linkButton.InnerText = fileNameOld;
                        linkButton.Title = fileNameOld;
                        linkButton.Attributes.Add("onclick", "return showActivityFile(this);");
                        e.Row.Cells[col].Controls.Add(linkButton);
                    }
                }
            }
            catch (Exception ex)
            {

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
                //if (targetTime != "")
                //{
                //    var spliData = targetTime.Split('.');
                //    string hours = "00";
                //    string minutes = "00";
                //    for (int i = 0; i < spliData.Length; i++)
                //    {
                //        if (i == 0)
                //        {
                //            if (spliData[i] != "")
                //            {
                //                if (spliData[i].Length == 1)
                //                {
                //                    hours = "0" + spliData[i];
                //                }
                //                else
                //                {
                //                    hours = spliData[i];
                //                }
                //            }
                //        }
                //        else if (i == 1)
                //        {
                //            if (spliData[i] != "")
                //            {
                //                if (spliData[i].Length == 1)
                //                {
                //                    minutes = spliData[i] + "0";
                //                }
                //                else
                //                {
                //                    minutes = spliData[i];
                //                }
                //            }
                //        }
                //    }
                //    targetTime = ((Convert.ToInt32(hours) * 60 * 60) + (Convert.ToInt32(minutes) * 60)).ToString();
                //}
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
                if (frequency == "Daily" || frequency == "Weekly" || frequency == "15 Days" || frequency == "1 Month" || frequency == "3 Month" || frequency == "6 Month" || frequency == "Shift")
                {
                    DateTime activityTS = Convert.ToDateTime(header);
                    activityTS = activityTS.Add(logicalStartTime);
                    //isUpdated = CumiDBAccess.saveActvityTransactionDetails(activity, frequency, Convert.ToDateTime(header).AddHours(6).ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), machineid, updatedBy, shiftId, criteria, status, action, remarks, category);
                    isUpdated = DataBaseAccess.saveActvityTransactionDetails(activity, frequency, activityTS.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), machineid, updatedBy, shiftId, criteria, status, action, remarks, category);
                }
                else if (frequency == "1 Year" || frequency == "2 Year")
                {
                    DateTime activityTS = Convert.ToDateTime("Jan " + header);
                    activityTS = activityTS.Add(logicalStartTime);
                    //isUpdated = CumiDBAccess.saveActvityTransactionDetails(activity, frequency, Convert.ToDateTime("Jan " + header).AddHours(6).ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), machineid, updatedBy, shiftId, criteria, status, action, remarks, category);
                    isUpdated = DataBaseAccess.saveActvityTransactionDetails(activity, frequency, activityTS.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), machineid, updatedBy, shiftId, criteria, status, action, remarks, category);
                }
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
        static string appPath = HttpContext.Current.Server.MapPath("");
        public static string GetReportPath(string reportName)
        {
            string src;
            src = Path.Combine(appPath, "Reports", "ReportTemplates", reportName);
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
            try
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
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }


        }
        protected void btnExportConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string sourcePath = "";
                string templatefile = string.Empty;
                string Filename = "PreventiveMaintenanceReport.xlsx";

                string Source = string.Empty;
                Source = GetReportPath(Filename);
                sourcePath = Source;
                string Template = string.Empty;
                Template = "PreventiveMaintenanceReport_" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "Reports", "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("While Exporting Signature Comparison Template- \n " + Source);
                }

                DateTime fromDate = Util.GetDateTime(txtFromDateExport.Text);
                DateTime toDate = Util.GetDateTime(txtToDateExport.Text);

                FileInfo newFile = new FileInfo(Source);
                ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                int i = 0;
                string machineid = ddlMachineId.SelectedValue;
                foreach (ListItem item in cbFrequency.Items)
                {
                    if (item.Selected)
                    {
                        int frequencyValue = Convert.ToInt32(item.Value);
                        if (i == 0)
                        {
                            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[1];
                            workSheet.Name = item.ToString();
                            workSheet.Cells["A1"].Value = "PREVENTIVE MAINTENANCE REPORT - " + item.ToString().ToUpper();
                            workSheet.Cells["B5"].Value = fromDate.ToString("dd-MM-yyyy");
                            workSheet.Cells["G5"].Value = toDate.ToString("dd-MM-yyyy");
                            PlotDataToExcel(workSheet, machineid, item.Text.ToString(), fromDate.ToString("yyyy-MM-dd hh:mm:ss"), toDate.ToString("yyyy-MM-dd hh:mm:ss"), frequencyValue);
                            workSheet.Cells.AutoFitColumns();
                        }
                        else
                        {
                            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add(item.ToString());
                            if (item.ToString().Equals("Weekly", StringComparison.OrdinalIgnoreCase))
                            {
                                MergeExcelCells(workSheet, "PREVENTIVE MAINTENANCE REPORT - WEEKLY");
                                PlotDataToExcel(workSheet, machineid, item.ToString(), fromDate.ToString("yyyy-MM-dd hh:mm:ss"), toDate.ToString("yyyy-MM-dd hh:mm:ss"), frequencyValue);
                            }
                            else if (item.ToString().Equals("15 Days", StringComparison.OrdinalIgnoreCase))
                            {
                                MergeExcelCells(workSheet, "PREVENTIVE MAINTENANCE REPORT - 15 DAYS");
                                PlotDataToExcel(workSheet, machineid, item.ToString(), fromDate.ToString("yyyy-MM-dd hh:mm:ss"), toDate.ToString("yyyy-MM-dd hh:mm:ss"), frequencyValue);
                            }
                            else if (item.ToString().Equals("1 Month", StringComparison.OrdinalIgnoreCase))
                            {
                                MergeExcelCells(workSheet, "PREVENTIVE MAINTENANCE REPORT - 1 MONTH");
                                PlotDataToExcel(workSheet, machineid, item.ToString(), fromDate.ToString("yyyy-MM-dd hh:mm:ss"), toDate.ToString("yyyy-MM-dd hh:mm:ss"), frequencyValue);
                            }
                            else if (item.ToString().Equals("3 Month", StringComparison.OrdinalIgnoreCase))
                            {
                                MergeExcelCells(workSheet, "PREVENTIVE MAINTENANCE REPORT - 3 MONTH");
                                PlotDataToExcel(workSheet, machineid, item.ToString(), fromDate.ToString("yyyy-MM-dd hh:mm:ss"), toDate.ToString("yyyy-MM-dd hh:mm:ss"), frequencyValue);
                            }
                            else if (item.ToString().Equals("6 Month", StringComparison.OrdinalIgnoreCase))
                            {
                                MergeExcelCells(workSheet, "PREVENTIVE MAINTENANCE REPORT - 6 MONTH");
                                PlotDataToExcel(workSheet, machineid, item.ToString(), fromDate.ToString("yyyy-MM-dd hh:mm:ss"), toDate.ToString("yyyy-MM-dd hh:mm:ss"), frequencyValue);
                            }
                            else if (item.ToString().Equals("1 Year", StringComparison.OrdinalIgnoreCase))
                            {
                                MergeExcelCells(workSheet, "PREVENTIVE MAINTENANCE REPORT - 1 YEAR");
                                PlotDataToExcel(workSheet, machineid, item.ToString(), fromDate.ToString("yyyy-MM-dd hh:mm:ss"), toDate.ToString("yyyy-MM-dd hh:mm:ss"), frequencyValue);
                            }
                            else if (item.ToString().Equals("2 Year", StringComparison.OrdinalIgnoreCase))
                            {
                                MergeExcelCells(workSheet, "PREVENTIVE MAINTENANCE REPORT - 2 YEAR");
                                PlotDataToExcel(workSheet, machineid, item.ToString(), fromDate.ToString("yyyy-MM-dd hh:mm:ss"), toDate.ToString("yyyy-MM-dd hh:mm:ss"), frequencyValue);
                            }
                            else if (item.ToString().Equals("5 Year", StringComparison.OrdinalIgnoreCase))
                            {
                                MergeExcelCells(workSheet, "PREVENTIVE MAINTENANCE REPORT - 5 YEAR");
                                PlotDataToExcel(workSheet, machineid, item.ToString(), fromDate.ToString("yyyy-MM-dd hh:mm:ss"), toDate.ToString("yyyy-MM-dd hh:mm:ss"), frequencyValue);
                            }
                            else if (item.ToString().Equals("Shift", StringComparison.OrdinalIgnoreCase))
                            {
                                MergeExcelCells(workSheet, "PREVENTIVE MAINTENANCE REPORT - SHIFT");
                                PlotDataToExcel(workSheet, machineid, item.ToString(), fromDate.ToString("yyyy-MM-dd hh:mm:ss"), toDate.ToString("yyyy-MM-dd hh:mm:ss"), frequencyValue);
                            }
                            else
                            {
                                MergeExcelCells(workSheet, "PREVENTIVE MAINTENANCE REPORT - DAILY");
                                PlotDataToExcel(workSheet, machineid, "Daily", fromDate.ToString("yyyy-MM-dd hh:mm:ss"), toDate.ToString("yyyy-MM-dd hh:mm:ss"), 1);
                            }
                            workSheet.Cells.AutoFitColumns();
                        }
                        i++;
                    }
                }
                DownloadFile(destination, excelPackage.GetAsByteArray());

            }
            catch (Exception ex)
            {

            }
        }

        private void MergeExcelCells(ExcelWorksheet workSheet, string sheetTitle)
        {
            workSheet.Cells[1, 1, 3, 15].Merge = true;
            workSheet.Cells["A1"].Value = sheetTitle;
            workSheet.Cells["A1"].Style.Font.Size = 20;
            workSheet.Cells["A1"].Style.Font.Bold = true;
            workSheet.Cells["A1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            workSheet.Cells["A1"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#3465A4"));
            workSheet.Cells["A1"].Style.Font.Color.SetColor(System.Drawing.Color.White);
            workSheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            workSheet.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        }

        private void PlotDataToExcel(ExcelWorksheet workSheet, string machineId, string frequency, string startTime, string endTime, int freqValue)
        {

            DataTable dt = new DataTable();
            dt = DataBaseAccess.GetPreventiveMaintenanceExportData(machineId, frequency, startTime, endTime, freqValue);

            List<string> cols = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            cols.Remove("Act"); cols.Remove("Frequency"); cols.Remove("ActivityID"); cols.Remove("ShiftID");
            if (!frequency.Equals("Shift", StringComparison.OrdinalIgnoreCase))
            {
                cols.Remove("ShiftName");
            }
            int row = 8; int pos = 1;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (string colName in cols)
                {
                    workSheet.Cells[row - 1, pos].Value = colName;
                    workSheet.Cells[row - 1, pos].Style.Font.Bold = true;
                    workSheet.Cells[row - 1, pos].Style.Font.Size = 12;
                    workSheet.Cells[row - 1, pos].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    workSheet.Cells[row - 1, pos].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    workSheet.Cells[row - 1, pos].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2080D0"));
                    workSheet.Cells[row - 1, pos].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    pos++;
                }
                pos = 0;
                //foreach (DataRow rowItem in dt.Rows)
                //{
                //    for (int j = 1; j <= cols.Count; j++)
                //    {
                //        workSheet.Cells[row, j].Value = rowItem[cols[j - 1]];
                //        workSheet.Cells[row, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                //    }
                //    row++;
                //}
                foreach (DataRow rowItem in dt.Rows)
                {
                    for (int j = 1; j <= cols.Count; j++)
                    {
                        if (cols[j - 1] == "LastUpdate")
                        {
                            workSheet.Cells[row, j].Value = rowItem[cols[j - 1]].ToString() == "" ? "" : Util.GetDateTime(rowItem[cols[j - 1]].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        }
                        else
                        {
                            workSheet.Cells[row, j].Value = rowItem[cols[j - 1]];
                        }

                        workSheet.Cells[row, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    row++;
                }
            }
        }
    }
}