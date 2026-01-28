using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;
using Web_TPMTrakDashboard.GEA.Models;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class WeeklyChecklistMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                if (!IsPostBack)
                {
                    SessionClear.ClearSession();
                    BindLineIDs();
                    BindMachineIDs();
                    BindYears();
                    BindFrequencies();
                    LoadAllWeeks();
                    BindWeeklyChecklistData();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void BindLineIDs()
        {
            try
            {
                List<string> lstLineData = GEADatabaseAccess.GetLineIDsForPlant("");
                if (lstLineData != null && lstLineData.Count > 0)
                {
                    ddlLineID.DataSource = lstLineData;
                    ddlLineID.DataBind();
                    ddlLineID.Items.Insert(0, new ListItem
                    {
                        Text = "Line All",
                        Value = "All"
                    });
                    ddlLineID.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private void BindMachineIDs()
        {
            try
            {
                string LineId = ddlLineID.SelectedItem != null ? ddlLineID.SelectedValue : "";
                if (!string.IsNullOrEmpty(LineId))
                {
                    if (LineId.Equals("All", StringComparison.OrdinalIgnoreCase)) LineId = "";
                    List<string> lstMachineIDs = GEADatabaseAccess.GetAllMachineByPlantandGroup("", LineId);
                    if (lstMachineIDs != null && lstMachineIDs.Count > 0)
                    {
                        ddlMachineId.DataSource = lstMachineIDs;
                        ddlMachineId.DataBind();
                        ddlMachineId.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private void BindYears()
        {
            try
            {
                List<int> lstYears = Enumerable.Range(DateTime.Now.Year - 9, 10).Reverse().ToList();
                if (lstYears != null && lstYears.Count > 0)
                {
                    ddlYear.DataSource = lstYears;
                    ddlYear.DataBind();
                    ddlMachineId.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private void BindFrequencies()
        {
            try
            {
                List<FrequencyEntity> frequencies = GEADatabaseAccess.GetAllFrequencies();
                if (frequencies != null && frequencies.Count > 0)
                {
                    if (frequencies.Any(x => x.Frequency.Equals("Daily")))
                        frequencies.Remove(frequencies.Where(x => x.Frequency.Equals("Daily")).First());
                    ddlFrequency.DataSource = frequencies;
                    ddlFrequency.DataTextField = "Frequency";
                    ddlFrequency.DataValueField = "FreqID";
                    ddlFrequency.DataBind();
                    ddlFrequency.Items.Insert(0, new ListItem
                    {
                        Text = "Frequency All",
                        Value = "All"
                    });
                    ddlFrequency.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private void LoadAllWeeks()
        {
            try
            {
                List<WeekEntity> weeksList = GetListOfWeeks();
                if (weeksList != null && weeksList.Count > 0)
                {
                    ddlWeek.DataSource = weeksList;
                    ddlWeek.DataTextField = "WeekName";
                    ddlWeek.DataValueField = "WeekNumber";
                    ddlWeek.DataBind();
                }
            }
            catch (Exception)
            {
                lblMessages.ForeColor = Color.DarkRed;
                lblMessages.Text = "Error loading weeks.";
            }
        }

        private List<WeekEntity> GetListOfWeeks()
        {
            List<WeekEntity> weekDetails = new List<WeekEntity>();
            for (int i = 1; i <= 52; i++)
            {
                WeekEntity weekEntity = new WeekEntity
                {
                    WeekName = "Week" + i,
                    WeekNumber = i
                };
                weekDetails.Add(weekEntity);
            }
            return weekDetails;
        }

        private void BindWeeklyChecklistData()
        {
            DataTable dtWeeklyChecklistData = new DataTable();
            try
            {
                string LineId = ddlLineID.SelectedItem != null ? ddlLineID.SelectedValue : "";
                string MachineID = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                string Year = ddlYear.SelectedItem != null ? ddlYear.SelectedItem.ToString() : "";
                string Frequency = ddlFrequency.SelectedItem != null ? ddlFrequency.SelectedItem.ToString() : "";
                if (!string.IsNullOrEmpty(LineId) && !string.IsNullOrEmpty(MachineID) && !string.IsNullOrEmpty(Frequency))
                {
                    if (LineId.Equals("All", StringComparison.OrdinalIgnoreCase)) LineId = "";
                    if (Frequency.Equals("Frequency All", StringComparison.OrdinalIgnoreCase)) Frequency = "";
                    Session["LineID"] = LineId; Session["Year"] = Year;
                    Session["MachineID"] = MachineID; Session["Frequency"] = Frequency;
                    dtWeeklyChecklistData = GEADatabaseAccess.GetWeeklyChecklistData(LineId, MachineID, Year, Frequency);
                    Session["FreqData"] = dtWeeklyChecklistData;
                    if (dtWeeklyChecklistData != null && dtWeeklyChecklistData.Rows.Count > 0)
                    {
                        dtWeeklyChecklistData = dtWeeklyChecklistData.AsEnumerable().OrderBy(x => x.Field<int>("ActivityID")).CopyToDataTable();
                        List<string> spanHeaders = dtWeeklyChecklistData.Columns.Cast<DataColumn>().Select(x => x.ColumnName.Contains("-") ? x.ColumnName.Split('-')[0] : x.ColumnName).ToList();
                        List<string> headers = dtWeeklyChecklistData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                        Session["SpanHeaders"] = spanHeaders;
                        Session["Headers"] = headers;
                        Session["WeeklyChklistData"] = dtWeeklyChecklistData;
                        GridWeeklyMaintChklist.Columns.Clear();
                        foreach (string header in headers.Take(7))
                        {
                            if (!header.Equals("Machineid") && !header.Equals("Frequency") && !header.Equals("FreqID"))
                            {
                                if (header.Equals("Chekpoints"))
                                {
                                    TemplateField templateField = new TemplateField();
                                    templateField.HeaderText = "Check Points";
                                    templateField.ControlStyle.Width = Unit.Pixel(240);
                                    templateField.ItemTemplate = new LabelTemplateGenerator(ListItemType.Item, header);
                                    GridWeeklyMaintChklist.Columns.Add(templateField);
                                }
                                else
                                {
                                    BoundField boundField = new BoundField();
                                    boundField.HeaderText = header;
                                    boundField.DataField = header;
                                    GridWeeklyMaintChklist.Columns.Add(boundField);
                                }
                            }
                        }
                        foreach (string header in headers.Skip(7))
                        {
                            TemplateField boundField = new TemplateField
                            {
                                HeaderText = header.Split('-')[1],
                                ItemTemplate = new TemplateGenerator(ListItemType.Item, header, MachineID, Year, Frequency)
                            };
                            GridWeeklyMaintChklist.Columns.Add(boundField);
                        }
                        GridWeeklyMaintChklist.DataSource = dtWeeklyChecklistData;
                        GridWeeklyMaintChklist.DataBind();
                    }
                    else
                    {
                        GridWeeklyMaintChklist.DataSource = null;
                        GridWeeklyMaintChklist.DataBind();
                    }
                }
                else
                {
                    GridWeeklyMaintChklist.DataSource = null;
                    GridWeeklyMaintChklist.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        private void ClearGridViewColumns()
        {
            if (GridWeeklyMaintChklist.Columns.Count > 7)
            {
                int count = GridWeeklyMaintChklist.Columns.Count - 7;
                for (int i = 0; i < count; i++)
                {
                    GridWeeklyMaintChklist.Columns.RemoveAt(7);
                }
            }
        }

        protected void GridWeeklyMaintChklist_DataBound(object sender, EventArgs e)
        {
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            List<string> headers = Session["SpanHeaders"] as List<string>;
            if (headers != null && headers.Count > 0)
            {
                Dictionary<string, int> headerCounts = headers.GroupBy(x => x).Select(x => new { Month = x.Key, Count = x.Count() }).ToDictionary(x => x.Month, x => x.Count);
                foreach (KeyValuePair<string, int> keyValuePair in headerCounts)
                {
                    if (!keyValuePair.Key.Equals("Machineid") && !keyValuePair.Key.Equals("Frequency") && !keyValuePair.Key.Equals("FreqID"))
                    {
                        if (keyValuePair.Key.Equals("ActivityID") || keyValuePair.Key.Equals("Chekpoints") || keyValuePair.Key.Equals("Method") || keyValuePair.Key.Equals("Criteria"))
                        {
                            TableHeaderCell cellHeaderGroup = new TableHeaderCell
                            {
                                Text = "",
                                ColumnSpan = keyValuePair.Value
                            };
                            row.Controls.Add(cellHeaderGroup);
                        }
                        else
                        {
                            TableHeaderCell cellHeaderGroup = new TableHeaderCell
                            {
                                Text = keyValuePair.Key,
                                ColumnSpan = keyValuePair.Value
                            };
                            row.Controls.Add(cellHeaderGroup);
                        }
                    }
                }
                row.BackColor = ColorTranslator.FromHtml("#5391CA");
                if (GridWeeklyMaintChklist.HeaderRow != null)
                    GridWeeklyMaintChklist.HeaderRow.Parent.Controls.AddAt(0, row);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            bool IsGenerated = false;
            try
            {
                string LineId = Session["LineID"] as string;
                string MachineID = Session["MachineID"] as string;
                string Year = Session["Year"] as string;
                string Frequency = Session["Frequency"] as string;
                IsGenerated = GEADatabaseAccess.GenerateActivityForFrequency(LineId, MachineID, Year, Frequency);
                if (IsGenerated)
                {
                    lblMessages.ForeColor = Color.DarkGreen;
                    lblMessages.Text = "Activity generated successfully.";
                    BindWeeklyChecklistData();
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string MonthName = hdfMonthName.Value;
                if (!string.IsNullOrEmpty(MonthName))
                {
                    int OldMonthNo = GetMonthNoFromName(MonthName);
                    string MachineId = Session["MachineID"] as string;
                    string ActID = hdfActId.Value;
                    string Activity = hdfActivity.Value;
                    string FreqID = hdfFreqId.Value;
                    string Year = hdfYear.Value;
                    string OldWeekNo = hdfOldWeekNo.Value;
                    string NewWeekNo = ddlWeek.SelectedItem != null ? ddlWeek.SelectedItem.Text : "";
                    if (!string.IsNullOrEmpty(NewWeekNo))
                    {
                        bool IsAvailable = GEADatabaseAccess.IsWeekAvailable(MachineId, ActID, FreqID, Year, NewWeekNo);
                        if (IsAvailable)
                        {
                            int NewMonthNo = GetMonthNoFromWeekNo(NewWeekNo, Year);
                            if (!NewMonthNo.Equals(0))
                            {
                                bool IsUpdated = GEADatabaseAccess.UpdateScheduledWeek(MachineId, ActID, Activity, FreqID, Year, OldMonthNo, NewMonthNo, OldWeekNo, NewWeekNo);
                                if (IsUpdated)
                                {
                                    lblMessages.ForeColor = Color.DarkGreen;
                                    lblMessages.Text = "Update successfull.";
                                    ClientScript.RegisterStartupScript(typeof(Page), "success", "Activity generated successfully.", true);
                                }
                                else
                                {
                                    lblMessages.ForeColor = Color.DarkRed;
                                    lblMessages.Text = "Error saving data.";
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('This task is already assigned for the selected week. Please select any available week.')", true);
                        }
                        BindWeeklyChecklistData();
                    }
                    else
                    {
                        lblMessages.ForeColor = Color.DarkRed;
                        lblMessages.Text = "Error saving data.";
                    }
                }
            }
            catch (Exception)
            {
                lblStatus.ForeColor = Color.DarkRed;
                lblStatus.Text = "Error saving data.";
            }
        }

        private int GetMonthNoFromWeekNo(string newWeekNo, string year)
        {
            int MonthNo = 0;
            try
            {
                List<string> headers = Session["Headers"] as List<string>;
                if (headers != null)
                {
                    if (headers.Any(x=>x.Contains(newWeekNo)))
                    {
                        string monthName = headers.Where(x => x.Contains(newWeekNo)).First().Split('-')[0];
                        MonthNo = GetMonthNoFromName(monthName);
                    }

                }
                else
                {
                    DateTime tDt = new DateTime(Convert.ToInt32(year), 1, 1);
                    tDt.AddDays((Convert.ToInt32(newWeekNo.Substring(4)) - 1) * 7);
                    for (int i = 0; i <= 365; ++i)
                    {
                        int tWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(tDt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                        if (tWeek == Convert.ToInt32(newWeekNo.Substring(4)))
                        {
                            MonthNo = tDt.Month;
                            break;
                        }
                        tDt = tDt.AddDays(1);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return MonthNo;
        }

        private int GetMonthNoFromName(string monthName)
        {
            int monthNumber = 0;
            monthNumber = DateTime.ParseExact(monthName, "MMM", CultureInfo.CurrentCulture).Month;
            return monthNumber;
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindWeeklyChecklistData();
        }

        protected void ddlLineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineIDs();
        }

        protected void ddlSortOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //DataTable dt = Session["FreqData"] as DataTable;
                //switch(ddlSortOrder.SelectedValue.ToString())
                //{
                //    case "ActivityID":
                //        dt = dt.AsEnumerable().OrderBy(x => x.Field<int>("ActivityID")).CopyToDataTable();
                //        GridWeeklyMaintChklist.DataSource = dt;
                //        GridWeeklyMaintChklist.DataBind();
                //        break;
                //    case "ChecklistPoints":
                //        dt = dt.AsEnumerable().OrderBy(x => x.Field<string>("chekpoints")).CopyToDataTable();
                //        GridWeeklyMaintChklist.DataSource = dt;
                //        GridWeeklyMaintChklist.DataBind();
                //        break;
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
    }

    public class TemplateGenerator : ITemplate // Class inheriting ITemplate
    {
        ListItemType type;
        string columnName;
        string MachineId;
        string Year;
        string FreqId;

        public TemplateGenerator(ListItemType t, string cN, string machineId, string year, string freq)
        {
            type = t;
            columnName = cN;
            MachineId = machineId;
            Year = year;
            FreqId = freq;
        }

        // Override InstantiateIn() method
        void ITemplate.InstantiateIn(System.Web.UI.Control container)
        {
            switch (type)
            {
                case ListItemType.Item:
                    HyperLink hyprLnk = new HyperLink();
                    hyprLnk.CssClass = "WeekScheduler";
                    hyprLnk.Target = "_blank";
                    hyprLnk.DataBinding += new EventHandler(hyprLnk_DataBinding);
                    hyprLnk.Font.Bold = true;
                    hyprLnk.Font.Size = new FontUnit(FontSize.Large);
                    container.Controls.Add(hyprLnk);
                    break;
            }
        }

        // The DataBinding event of your controls
        void hyprLnk_DataBinding(object sender, EventArgs e)
        {
            HyperLink hyprlnk = (HyperLink)sender;
            GridViewRow container = (GridViewRow)hyprlnk.NamingContainer;
            object bindValue = System.Web.UI.DataBinder.Eval(container.DataItem, columnName);
            if (bindValue != DBNull.Value)
            {
                string val = bindValue.ToString();
                if (val.Equals("1"))
                {
                    hyprlnk.Text = "O";
                    hyprlnk.Attributes.Add("style", "cursor:pointer");
                    hyprlnk.Attributes.Add("MachineID", MachineId);
                    hyprlnk.Attributes.Add("FreqID", System.Web.UI.DataBinder.Eval(container.DataItem, "FreqID").ToString());
                    hyprlnk.Attributes.Add("Frequency", System.Web.UI.DataBinder.Eval(container.DataItem, "Frequency").ToString());
                    hyprlnk.Attributes.Add("Year", Year);
                    hyprlnk.Attributes.Add("ActivityID", System.Web.UI.DataBinder.Eval(container.DataItem, "ActivityID").ToString());
                    hyprlnk.Attributes.Add("CheckPoints", System.Web.UI.DataBinder.Eval(container.DataItem, "Chekpoints").ToString());
                    hyprlnk.Attributes.Add("WeekNo", columnName);
                }
                else
                {
                    hyprlnk.Text = "";
                }
            }
        }
    }

    public class LabelTemplateGenerator : ITemplate // Class inheriting ITemplate
    {
        ListItemType type;
        string columnName;

        public LabelTemplateGenerator(ListItemType t, string cN)
        {
            type = t;
            columnName = cN;
        }

        // Override InstantiateIn() method
        void ITemplate.InstantiateIn(System.Web.UI.Control container)
        {
            switch (type)
            {
                case ListItemType.Item:
                    Label label = new Label();
                    label.DataBinding += new EventHandler(label_DataBinding);
                    container.Controls.Add(label);
                    break;
            }
        }

        // The DataBinding event of your controls
        void label_DataBinding(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            GridViewRow container = (GridViewRow)label.NamingContainer;
            object bindValue = System.Web.UI.DataBinder.Eval(container.DataItem, columnName);
            if (bindValue != DBNull.Value)
            {
                label.Text = bindValue.ToString();
            }
        }
    }

    public class WeekEntity
    {
        public string WeekName { get; set; }
        public int WeekNumber { get; set; }
    }
}