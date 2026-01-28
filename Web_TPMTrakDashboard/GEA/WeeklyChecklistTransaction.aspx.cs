using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class WeeklyChecklistTransaction : System.Web.UI.Page
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
                    BindChecklistTransactionData();
                    if (!GEADatabaseAccess.checkroleMaintainnce(Session["UserName"].ToString()))
                    {
                        btnMainainanceChecklistSave.Visible = false;
                    }
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

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetWeekDate()
        {
            return GEADatabaseAccess.GetWeekNumber();
        }

        private void BindChecklistTransactionData()
        {
            DataTable dtWeeklyChklistTransactionData = new DataTable();
            try
            {
                string LineId = ddlLineID.SelectedItem != null ? ddlLineID.SelectedValue : "";
                string MachineID = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                if (!string.IsNullOrEmpty(LineId) && !string.IsNullOrEmpty(MachineID))
                {
                    if (LineId.Equals("All", StringComparison.OrdinalIgnoreCase)) LineId = "";
                    Session["LineID"] = LineId;
                    Session["MachineID"] = MachineID;
                    dtWeeklyChklistTransactionData = GEADatabaseAccess.GetWeeklyChklistTransactionData(LineId, MachineID);
                    if (dtWeeklyChklistTransactionData != null && dtWeeklyChklistTransactionData.Rows.Count > 0)
                    {
                        List<string> spanHeaders = dtWeeklyChklistTransactionData.Columns.Cast<DataColumn>().Select(x => x.ColumnName.Contains("-") ? x.ColumnName.Split('-')[0] : x.ColumnName).ToList();
                        List<string> headers = dtWeeklyChklistTransactionData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                        Session["SpanHeaders"] = spanHeaders;
                        Session["WeeklyTransactionChklistData"] = dtWeeklyChklistTransactionData;
                        GridWeeklyMaintTransaction.Columns.Clear();
                        foreach (string header in headers)
                        {
                            if (headers.IndexOf(header) < 7)
                            {
                                if (!header.Equals("Machineid") && !header.Equals("Frequency") && !header.Equals("FreqID"))
                                {
                                    TemplateField templateField = new TemplateField();
                                    templateField.HeaderText = header.Equals("Chekpoints") ? "Check Points" : header;
                                    templateField.ControlStyle.Width = header.Equals("Chekpoints") ? Unit.Pixel(240) : Unit.Pixel(150);
                                    templateField.ItemTemplate = new LabelTemplateGenerator(ListItemType.Item, header);
                                    GridWeeklyMaintTransaction.Columns.Add(templateField);
                                }
                            }
                            else
                            {
                                TemplateField templateField = new TemplateField
                                {
                                    HeaderText = header.Split('-')[1],
                                    ItemTemplate = new ImageTemplateGenerator(ListItemType.Item, header)
                                };
                                GridWeeklyMaintTransaction.Columns.Add(templateField);
                            }
                        }
                        GridWeeklyMaintTransaction.DataSource = dtWeeklyChklistTransactionData;
                        GridWeeklyMaintTransaction.DataBind();
                    }
                    else
                    {
                        GridWeeklyMaintTransaction.DataSource = null;
                        GridWeeklyMaintTransaction.DataBind();
                    }
                }
                else
                {
                    GridWeeklyMaintTransaction.DataSource = null;
                    GridWeeklyMaintTransaction.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        protected void GridWeeklyMaintTransaction_DataBound(object sender, EventArgs e)
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
                                ColumnSpan = keyValuePair.Value,
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
                if (GridWeeklyMaintTransaction.HeaderRow != null)
                    GridWeeklyMaintTransaction.HeaderRow.Parent.Controls.AddAt(0, row);
            }
        }

        protected void ddlLineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineIDs();
        }

        protected void btnSaveWeekNumber(object sender, EventArgs e)
        {
            bool IsSaved = false;
            try
            {
                //var currentCulture = CultureInfo.CurrentCulture;
                //var weekno = currentCulture.Calendar.GetWeekOfYear(DateTime.Now, currentCulture.DateTimeFormat.CalendarWeekRule, currentCulture.DateTimeFormat.FirstDayOfWeek);
                string weekno = GEADatabaseAccess.GetWeekNumber();
                if (!GEADatabaseAccess.CheckforMaintainanceData(weekno, ddlMachineId.SelectedValue.ToString())) 
                {
                    lblMessages.Text = "Supervisor Data are yet to be saved !!";
                    btnView_Click(null, null);
                    Logger.WriteDebugLog("ChecklistMaintainance= WeekNo: " + weekno + " (Supervisor Data are yet to be saved)");
                    return;
                }
                if (!GEADatabaseAccess.CheckForAlreadyLogin(weekno, DateTime.Now.Year, ddlMachineId.SelectedValue.ToString()))
                {
                    GEADatabaseAccess.SaveWeekEntry(weekno, DateTime.Now.Year, ddlMachineId.SelectedValue.ToString(), Session["userName"].ToString());
                    lblMessages.Text = "Data Saved Successfully!!";
                    Logger.WriteDebugLog("ChecklistMaintainance= WeekNo: " + weekno + ", Machine: "+ ddlMachineId.SelectedValue.ToString() + " (Data Saved Successfully)");
                }
                else
                {
                    lblMessages.Text = "Data is already Saved for the machine";
                    Logger.WriteDebugLog("ChecklistMaintainance= WeekNo: " + weekno + " (Data is already Saved for the machine)");
                }
                btnView_Click(null, null);
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool IsSaved = false;
            try
            {
                string LineId = Session["LineID"] as string;
                string macId = Session["MachineID"] as string;
                string freqId = hdfFreqID.Value;
                string actId = hdfActivityID.Value;
                //string weekNo = hdfWeekNo.Value.Replace("Week","");
                string weekNo = hdfWeekNo.Value;
                string actValue = ddlInspection.SelectedItem != null ? ddlInspection.SelectedValue : "";
                string remarks = txtRemarks.Text;
                if (!string.IsNullOrEmpty(actValue))
                {
                    IsSaved = GEADatabaseAccess.UpdateChecklistTransactionData(LineId, macId, freqId, actId, weekNo, actValue, remarks);
                    if (IsSaved)
                    {
                        lblMessages.ForeColor = Color.Green;
                        lblMessages.Text = "Data saved successfully.";
                    }
                    else
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "Error while saving data.";
                    }
                }
                else
                {
                    lblMessages.ForeColor = Color.Red;
                    lblMessages.Text = "Inspection value cannot be empty.";
                }
                BindChecklistTransactionData();
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindChecklistTransactionData();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetChecklistRemarks(string machineID, string freqId, string actId, string weekNo)
        {
            return GEADatabaseAccess.GetChecklistRemarks(machineID, freqId, actId, weekNo);
        }
    }

    public class ImageTemplateGenerator : ITemplate // Class inheriting ITemplate
    {
        ListItemType type;
        string columnName;

        public ImageTemplateGenerator(ListItemType t, string cN)
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
                    System.Web.UI.WebControls.Image image = new System.Web.UI.WebControls.Image();
                    image.CssClass = "InspectionStatus";
                    image.DataBinding += new EventHandler(image_DataBinding);
                    container.Controls.Add(image);
                    break;
            }
        }

        // The DataBinding event of your controls
        void image_DataBinding(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.Image image = (System.Web.UI.WebControls.Image)sender;
            GridViewRow container = (GridViewRow)image.NamingContainer;
            object bindValue = System.Web.UI.DataBinder.Eval(container.DataItem, columnName);
            if (bindValue != DBNull.Value)
            {
                string value = bindValue.ToString();
                if (value.Equals("1"))
                {
                    image.ImageUrl = @"NotifyIcons/NotifyIcon_Green.gif";
                    image.ToolTip = "Done";
                }
                else if (value.Equals("2"))
                {
                    image.ImageUrl = @"NotifyIcons/NotifyIcon_Red.png";
                    image.ToolTip = "Not Done";
                }
                else if (value.Equals("3"))
                {
                    image.ImageUrl = @"NotifyIcons/NotifyIcon_Blue.png";
                    image.ToolTip = "Check Done & Replaced";
                }
                else if (value.Equals("4"))
                {
                    image.ImageUrl = @"NotifyIcons/NotifyIcon_Black.png";
                    image.ToolTip = "Not Available";
                }
                else if (value.Equals("5"))
                {
                    image.ImageUrl = @"NotifyIcons/NotifyIcon_Brown.png";
                    image.ToolTip = "Not Attempted";
                }
                image.Attributes.Add("MachineID", System.Web.UI.DataBinder.Eval(container.DataItem, "Machineid").ToString());
                image.Attributes.Add("ActivityID", System.Web.UI.DataBinder.Eval(container.DataItem, "ActivityID").ToString());
                image.Attributes.Add("CheckPoints", System.Web.UI.DataBinder.Eval(container.DataItem, "Chekpoints").ToString());
                image.Attributes.Add("Frequency", System.Web.UI.DataBinder.Eval(container.DataItem, "Frequency").ToString());
                image.Attributes.Add("FreqID", System.Web.UI.DataBinder.Eval(container.DataItem, "FreqID").ToString());
                image.Attributes.Add("WeekNo", columnName.Split('-')[1]);
                image.Attributes.Add("InsID", value);
            }
        }
    }
}