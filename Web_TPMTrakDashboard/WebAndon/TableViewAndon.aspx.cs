using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.WebAndon
{
    public partial class TableViewAndon : System.Web.UI.Page
    {
        public SettingsGUI settings = null;
        public string TableBorderSetting { get; set; }
        string Mode = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                settings = new SettingsGUI();
                DropDownList drpPlantName = (DropDownList)Page.Master.FindControl("ddlPlantName");
                DropDownList drpLine = (DropDownList)Page.Master.FindControl("ddlLine");
                TextBox txtDate = (TextBox)Page.Master.FindControl("txtDate");

               // DropDownList drpShift = (DropDownList)Page.Master.FindControl("ddlShift");
                Button btnProcess = (Button)Page.Master.FindControl("btnProcess");
                drpPlantName.Visible = true; drpLine.Visible = true;
                if (Session["AdminData"] == null || !Request.IsAuthenticated)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                else
                {
                    if (Session["Mode"] != null && Session["Mode"].ToString().Equals("DESKTOP", StringComparison.OrdinalIgnoreCase))
                    {
                        txtDate.Visible = true;
                        //drpShift.Visible = true;
                        btnProcess.Visible = true;
                        btnProcess.Click += new EventHandler(btnProcess_Click);
                    }
                    else
                    {
                        txtDate.Visible = false;
                        //drpShift.Visible = false;
                        btnProcess.Visible = false;
                        drpPlantName.SelectedIndexChanged += new EventHandler(drp_SelectedIndexChanged);
                    }
                    if (!IsPostBack)
                    {
                        if (Request.QueryString["mode"] != null && !string.IsNullOrEmpty(Request.QueryString["mode"].ToString()))
                        {
                            Mode = Request.QueryString["mode"];
                            if (!string.IsNullOrEmpty(Mode))
                            {
                                Session["Mode"] = Mode.ToString().ToUpper();
                            }
                        }
                        else
                        {
                            Session["Mode"] = hdfMode.Value = Session["Mode"] == null ? "DESKTOP" : Session["Mode"].ToString().ToUpper();
                        }

                        BindTableData();
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        #region "Click Process Button"
        void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                DropDownList drpPlantName = (DropDownList)Page.Master.FindControl("ddlPlantName");
                DropDownList drpLine = (DropDownList)Page.Master.FindControl("ddlPlantName");
                if (drpPlantName != null && drpLine != null)
                {
                    Session["PlantId"] = drpPlantName.SelectedValue.ToString();
                    Session["LineID"] = drpLine.SelectedValue.ToString();
                }
                gridviewTableData.PageIndex = 0;
                BindListItem();
                if (Session["Mode"] != null && Session["Mode"].ToString().Equals("ANDON", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AfterPostback", "StartFromBegin()", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AfterPostback", "StartDesktopMode()", true);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Plant Id Selection Change"
        void drp_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList drp = (DropDownList)Page.Master.FindControl("ddlPlantName");
                DropDownList drpLine = (DropDownList)Page.Master.FindControl("ddlLine");
                if (drp != null && drpLine != null)
                {
                    Session["PlantId"] = drp.SelectedValue.ToString();
                    Session["LineID"] = drpLine.SelectedValue.ToString();
                }
                gridviewTableData.PageIndex = 0;
                BindListItem();
                if (Session["Mode"] != null && Session["Mode"].ToString().Equals("ANDON", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AfterPostback", "StartFromBegin()", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AfterPostback", "StartDesktopMode()", true);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        private void BindTableData()
        {
            try
            {
                List<string> listOfColNames = new List<string>();
                List<string> val = DataBaseAccess.GetOrderedTableView(out listOfColNames, Session["UserName"] == null ? "none" : Session["UserName"].ToString());//
                for (int i = 0; i < listOfColNames.Count; i++)
                {
                    BoundField boundfield = new BoundField();

                    boundfield.DataField = listOfColNames[i].ToString();
                    boundfield.HeaderText = val[i].ToString();
                    if (listOfColNames[i].Equals("OverAllEfficiency", StringComparison.OrdinalIgnoreCase) || listOfColNames[i].Equals("ProductionEfficiency", StringComparison.OrdinalIgnoreCase) ||
                        listOfColNames[i].Equals("AvailabilityEfficiency", StringComparison.OrdinalIgnoreCase) || listOfColNames[i].Equals("PrevOEE", StringComparison.OrdinalIgnoreCase))
                    {
                        boundfield.DataFormatString = "{0:F0}";
                    }
                    //boundfield.HeaderStyle.Width = new Unit("100px");
                    gridviewTableData.Columns.Add(boundfield);
                }

                BindListItem();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void BindListItem()
        {
            try
            {
                DateTime fromdate = DateTime.Now;
                DateTime todate = DateTime.Now;
                DataTable dt = new DataTable();
                DropDownList drp = (DropDownList)Page.Master.FindControl("ddlPlantName");
                DropDownList drpLine = (DropDownList)Page.Master.FindControl("ddlLine");
                var selectValue = drp.SelectedValue.ToString();
                var Line = drpLine.SelectedValue.ToString();
                if (selectValue == "All")
                    selectValue = "";
                if (Line.Equals("All", StringComparison.OrdinalIgnoreCase) || Line.Equals("LineAll", StringComparison.OrdinalIgnoreCase))
                    Line = "";
                TextBox txtDate = (TextBox)Page.Master.FindControl("txtDate");
                DropDownList drpShift = (DropDownList)Page.Master.FindControl("ddlShift");
                string data = string.Empty, shift = string.Empty;
                DateTime shiftDate = DateTime.Now.Date;
                string mode = "DesktopMode";
                if (Session["Mode"] != null && Session["Mode"].ToString().Equals("DESKTOP", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(txtDate.Text))
                    {
                        string shiftdate = DateTime.Now.ToString("yyyy-MM-dd");//BindCockpitView.GetLogicalDayStart(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                        todate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(fromdate.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                    else
                    {
                        fromdate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(txtDate.Text));
                        todate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtDate.Text));
                      
                    }
                }
                else
                {
                    mode = "AndonMode";
                    todate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(fromdate.ToString("yyyy-MM-dd HH:mm:ss")));
                }


                dt = DataBaseAccess.GetMachinetableview("s_GetCockpitData_WithTempTable_eshopx", fromdate.ToString("yyyy-MM-dd HH:mm:ss"), todate.ToString("yyyy-MM-dd HH:mm:ss"), shift, "",
                    selectValue, Line, "MachinewiseDetails", Session["UserName"] == null ? "none" : Session["UserName"].ToString(), mode);

                if (Session["Mode"] != null && Session["Mode"].ToString().Equals("ANDON", StringComparison.OrdinalIgnoreCase))
                {
                    int dataRefreshInterval = 0;
                    int.TryParse(settings.TableUISetting.ScreenFlipInterval, out dataRefreshInterval);
                    timerDataChange.Interval = dataRefreshInterval * 1000;
                    gridviewTableData.AllowPaging = true;
                    timerDataChange.Enabled = true;
                    gridviewTableData.PageSize = settings.TableUISetting.PageSizeTableView == "" ? 10 : int.Parse(settings.TableUISetting.PageSizeTableView);
                }
                gridviewTableData.DataSource = dt;
                gridviewTableData.DataBind();
                if (settings.TableUISetting.FormFontSize != "")
                {
                    gridviewTableData.HeaderStyle.Font.Size = new FontUnit(settings.TableUISetting.FontSizeOuterTab);
                    foreach (DataControlField item in gridviewTableData.Columns)
                    {
                        item.ItemStyle.Font.Size = new FontUnit(settings.TableUISetting.FontSizeInerTab);
                    }
                }
                if (Session["Mode"] == null || Session["Mode"].ToString().Equals("DESKTOP", StringComparison.OrdinalIgnoreCase))
                {
                    timerDataChange.Enabled = false;
                    gridviewTableData.UseAccessibleHeader = true;
                    if (dt != null && dt.Rows.Count > 0)
                        gridviewTableData.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        protected void gridviewTableData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int colIndex = GetColumnIndexByDBName(this.gridviewTableData, "ProductionEfficiency");
                    if (colIndex != -1)
                    {
                        DataRow row = ((DataRowView)e.Row.DataItem).Row;
                        String name = row.Field<String>("Pecolor");
                        e.Row.Cells[colIndex].BackColor = GetColorFrom(name); //System.Drawing.Color.FromName(name);
                    }
                    int colIndex1 = GetColumnIndexByDBName(this.gridviewTableData, "OverAllEfficiency");
                    if (colIndex1 != -1)
                    {
                        DataRow row = ((DataRowView)e.Row.DataItem).Row;
                        String name = row.Field<String>("Oeecolor");
                        e.Row.Cells[colIndex1].BackColor = GetColorFrom(name);// System.Drawing.Color.FromName(name);
                    }
                    int colIndex2 = GetColumnIndexByDBName(this.gridviewTableData, "AvailabilityEfficiency");
                    if (colIndex2 != -1)
                    {
                        DataRow row = ((DataRowView)e.Row.DataItem).Row;
                        String name = row.Field<String>("Aecolor");
                        e.Row.Cells[colIndex2].BackColor = GetColorFrom(name); //System.Drawing.Color.FromName(name);
                    }
                    int colIndex3 = GetColumnIndexByDBName(this.gridviewTableData, "QualityEfficiency");
                    if (colIndex3 != -1)
                    {
                        DataRow row = ((DataRowView)e.Row.DataItem).Row;
                        String name = row.Field<String>("Qecolor");
                        e.Row.Cells[colIndex3].BackColor = GetColorFrom(name);
                        //System.Drawing.Color.FromName(name);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        public Color GetColorFrom(string colorName)
        {
            try
            {
                if (colorName.Equals("Green", StringComparison.OrdinalIgnoreCase))
                {
                    return System.Drawing.ColorTranslator.FromHtml("#" + settings.ColorUISetting.GoodColor.Substring(3));
                }
                else if (colorName.Equals("Yellow", StringComparison.OrdinalIgnoreCase))
                {
                    return System.Drawing.ColorTranslator.FromHtml("#" + settings.ColorUISetting.ModerateColor.Substring(3));
                }
                else if (colorName.Equals("Red", StringComparison.OrdinalIgnoreCase))
                {
                    return System.Drawing.ColorTranslator.FromHtml("#" + settings.ColorUISetting.BadColor.Substring(3));
                }
                return Color.White;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
        }

        static public int GetColumnIndexByDBName(GridView aGridView, String ColumnText)
        {
            try
            {
                BoundField DataColumn;
                for (int Index = 0; Index < aGridView.Columns.Count; Index++)
                {
                    DataColumn = aGridView.Columns[Index] as BoundField;

                    if (DataColumn != null)
                    {
                        if (DataColumn.DataField == ColumnText)
                            return Index;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
        }

        static public DataControlFieldCell GetCellByName(GridViewRow Row, String CellName)
        {
            try
            {
                foreach (DataControlFieldCell Cell in Row.Cells)
                {
                    if (Cell.ContainingField.ToString() == CellName)
                        return Cell;
                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
        }

        protected void timerDataChange_Tick(object sender, EventArgs e)
        {
            try
            {
                if (gridviewTableData.PageIndex == gridviewTableData.PageCount - 1)
                    gridviewTableData.PageIndex = 0;
                else
                    gridviewTableData.PageIndex = gridviewTableData.PageIndex + 1;
                ////timerDataChange.Enabled = false;
                //gridviewTableData.PageIndex++;
                BindListItem();
                if (Session["Mode"] != null && Session["Mode"].ToString().Equals("ANDON", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AfterPostback", "StartFromBegin()", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AfterPostback", "StartDesktopMode()", true);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

    }
}