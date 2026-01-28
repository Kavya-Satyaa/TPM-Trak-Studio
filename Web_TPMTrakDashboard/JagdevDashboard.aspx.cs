using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class JagdevDashboard : System.Web.UI.Page
    {
        static int TickCount = 0,SkipCount=0;
        static bool DesktopMode = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["JAndonData"] = null;
                //bindAndonData();
                //imgBtnSwitch.ToolTip = "Switch to ANDON Mode";
                //imgBtnSwitch_Click(null, null);
                
                modeToggle.Checked = true;
                btnmodaldeToggle_Click(null, null);
            }
            //this.modeToggle.Attributes["onchange"] += new System.EventHandler(this.modeToggle_ServerChange);
           // this.modeToggle.Attributes["onchange"] = "UploadSignalFile(this)";
        }
        public static string[] formats = new string[] { "d/MM/yyyy", "d/M/yy", "dd/M/yyyy", "dd-MM-yy", "dd/MM/yy", "d-M-yy", "d-MM-yy", "d/M/yyyy", "dd/MM/yyyy", "MM/dd/yyyy", "yyyy/MM/dd", "DD/MM/yyyy", "dd/MMM/yyyy", "dd-MM-yyyy HH:mm:ss", "dd-MM-yyyy HH:mm", "dd-MM-yyyy", "dd-MMM-yyyy", "dd-MMM-yyyy HH:mm", "dd-MMM-yyyy HH:mm:ss", "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "dd-MM-yyyyTHH:mm:ss", "dd-MM-yyyyTHH:mm", "dd-MMM-yyyyTHH:mm", "dd-MMM-yyyyTHH:mm:ss", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mm" };
       
        private void bindAndonData()
        {
            try
            {
                List<JDataset> jDatasets = new List<JDataset>();
                List<string> distinctShift = new List<string>();
                List<string> distinctCell = new List<string>();
                List<ListItem> totalParameters = getParameters();
                List<ListItem> parameters = new List<ListItem>();
                string Date = "";
                if (DesktopMode)
                {
                    // string date = txtDate.Text;
                    DateTime selectedDate = Util.GetDateTime(txtDate.Text + " 00:00:00");
                 
                    DateTime selectedDateforComparison;
                    DateTime currentDate;
                    DateTime.TryParseExact(txtDate.Text.Trim(), formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out selectedDateforComparison);
                    DateTime.TryParseExact(DateTime.Now.ToString("dd-MM-yyyy"), formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out currentDate);

                    if (selectedDateforComparison < currentDate || selectedDateforComparison > currentDate)
                    {
                        Date = selectedDate.ToString("dd-MM-yyyy");
                        jDatasets = TMPTrakDataBase.GetJagdevDashboardData(selectedDate, "DesktopMode");
                    }
                    else
                    {
                        DateTime dt = Util.GetDateTime(TMPTrakDataBase.GetLogicalDayStart(DateTime.Now.ToString()));
                        Date = dt.ToString("dd-MM-yyyy");
                        jDatasets = TMPTrakDataBase.GetJagdevDashboardData(dt, "AndonMode");
                    }
                
                    parameters = totalParameters.Skip(2).Take(1).ToList();
                }
                else
                {
                    if (TickCount > totalParameters.Count)
                    {
                        TickCount = 0;
                        SkipCount = 0;
                        Session["JAndonData"] = null;
                    }
                    DateTime dt = Util.GetDateTime(TMPTrakDataBase.GetLogicalDayStart(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    Date = dt.ToString("dd-MM-yyyy");
                    if (Session["JAndonData"] == null)
                    {
                        jDatasets = TMPTrakDataBase.GetJagdevDashboardData(dt, "AndonMode");
                        Session["JAndonData"] = jDatasets;
                    }
                    else
                    {
                        jDatasets = (List<JDataset>)Session["JAndonData"];
                    }
                    parameters = totalParameters.Skip(SkipCount).Take(TickCount == 0 ? totalParameters.Count : 1).ToList();

                }
              

                distinctShift = jDatasets.Select(x => x.ShiftId).Distinct().ToList();
                distinctCell = jDatasets.Select(x => x.CellId).Distinct().ToList();
                List<JParameters> parameterList = new List<JParameters>();
                JParameters parameter = null;
                if (jDatasets.Count > 0)
                {
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        parameter = new JParameters();
                        parameter.ParameterId = parameters[i].Text;
                        parameter.Date = Date;
                        if (parameters.Count == 1)
                        {
                            parameter.DivWidth = "97%";
                        }
                        else
                        {
                            parameter.DivWidth = "48%";
                        }
                        List<JShiftDetails> shiftList = new List<JShiftDetails>();
                        JShiftDetails shiftData = null;
                        foreach (string shift in distinctShift)
                        {
                            shiftData = new JShiftDetails();
                            shiftData.ShiftId = shift;

                            List<JCellDetails> cellList = new List<JCellDetails>();
                            JCellDetails cellData = null;
                            foreach (string cell in distinctCell)
                            {
                                cellData = new JCellDetails();
                                cellData.CellId = cell;
                                if (parameters.Count == 1)
                                {
                                    cellData.JMachineDetailsHeight = "200px";
                                }
                                else
                                {
                                    cellData.JMachineDetailsHeight = "unset";
                                }
                                var data = jDatasets.Where(x => x.ShiftId == shift && x.CellId == cell).ToList();
                                List<JMachineDetails> machineList = new List<JMachineDetails>();
                                JMachineDetails machineData = null;
                                int machineCount = 1;
                                int machineIncreament = 0;
                                foreach (var item in data)
                                {
                                    if ((machineIncreament) % 6 == 0)
                                    {
                                        if (machineIncreament != 0)
                                        {
                                            //if (parameters.Count == 1)
                                            //{
                                            //    machineData.JMachineDetailsHeight = "200px";
                                            //}
                                            //else
                                            //{
                                            //    machineData.JMachineDetailsHeight = "unset";
                                            //}
                                            machineList.Add(machineData);
                                        }
                                        machineData = new JMachineDetails();
                                    }
                              
                                    string backcolor = "white";
                                    string color = "black";
                                    if (parameters[i].Value == "Eff")
                                    {
                                        backcolor = getBackColor(item.EffBackColor, out color);
                                        parameter.EffLegend = "table-row";
                                        parameter.OEELegend = "none";
                                        parameter.DowntimeLegend = "none";
                                        parameter.RejLegend = "none";
                                        parameter.DesktopDowntimeLegend = "none";
                                    }
                                    else if (parameters[i].Value == "OEE")
                                    {
                                        backcolor = getBackColor(item.OEEBackColor, out color);
                                        parameter.EffLegend = "none";
                                        parameter.OEELegend = "table-row";
                                        parameter.DowntimeLegend = "none";
                                        parameter.RejLegend = "none";
                                        parameter.DesktopDowntimeLegend = "none";
                                    }
                                    else if (parameters[i].Value == "Downtime")
                                    {
                                        backcolor = getBackColor(item.DowntimeBackColor, out color);
                                        parameter.EffLegend = "none";
                                        parameter.OEELegend = "none";
                                        parameter.RejLegend = "none";
                                        if (DesktopMode)
                                        {
                                            parameter.DowntimeLegend = "none";
                                            parameter.DesktopDowntimeLegend = "table-row";
                                        }
                                        else
                                        {
                                            parameter.DowntimeLegend = "table-row";
                                            parameter.DesktopDowntimeLegend = "none";
                                        }

                                    }
                                    else
                                    {
                                        backcolor = getBackColor(item.RejectionBackColor, out color);
                                        parameter.EffLegend = "none";
                                        parameter.OEELegend = "none";
                                        parameter.DowntimeLegend = "none";
                                        parameter.RejLegend = "table-row";
                                        parameter.DesktopDowntimeLegend = "none";
                                    }
                                    if (machineCount == 1)
                                    {
                                        machineData.MachineID1 = item.MachineID;
                                        machineData.BackColor1 = backcolor;
                                        machineData.ForeColor1 = color;
                                        machineCount++;
                                    }
                                    else if (machineCount == 2)
                                    {
                                        machineData.MachineID2 = item.MachineID;
                                        machineData.BackColor2 = backcolor;
                                        machineData.ForeColor2 = color;
                                        machineCount++;
                                    }
                                    else if (machineCount == 3)
                                    {
                                        machineData.MachineID3 = item.MachineID;
                                        machineData.BackColor3 = backcolor;
                                        machineData.ForeColor3 = color;
                                        machineCount++;
                                    }
                                    else if (machineCount == 4)
                                    {
                                        machineData.MachineID4 = item.MachineID;
                                        machineData.BackColor4 = backcolor;
                                        machineData.ForeColor4 = color;
                                        machineCount++;
                                    }
                                    else if (machineCount == 5)
                                    {
                                        machineData.MachineID5 = item.MachineID;
                                        machineData.BackColor5 = backcolor;
                                        machineData.ForeColor5 = color;
                                        machineCount++;
                                    }
                                    else
                                    {
                                        machineData.MachineID6 = item.MachineID;
                                        machineData.BackColor6 = backcolor;
                                        machineData.ForeColor6 = color;
                                        machineCount = 1;
                                    }

                                    //machineData.MachineID = item.MachineID;
                                    //machineData.BackColor = item.EffBackColor;
                                    if (machineIncreament == data.Count-1)
                                    {
                                        //if (parameters.Count == 1)
                                        //{
                                        //    machineData.JMachineDetailsHeight = "200px";
                                        //}
                                        //else
                                        //{
                                        //    machineData.JMachineDetailsHeight = "unset";
                                        //}
                                        machineList.Add(machineData);
                                    }
                                    machineIncreament++;
                                }
                                cellData.JMachineDetails = machineList;
                                cellList.Add(cellData);
                            }
                            shiftData.JCellDetails = cellList;
                            shiftList.Add(shiftData);
                        }

                        parameter.JShiftDetails = shiftList;
                        parameterList.Add(parameter);

                        lvAndonData.DataSource = parameterList;
                        lvAndonData.DataBind();
                        // Session["JDashboardData"] = parameterList;
                    }
                }
                else
                {
                    lvAndonData.Items.Clear();
                    lvAndonData.DataSource = null ;
                    lvAndonData.DataBind();
                    //lvAndonData.EmptyDataTemplate.
                }
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "setStyle", "setMachineTableHeight();", true);
            }
            catch(Exception ex)
            {

            }
        }
       private string getBackColor(string color,out string foreColor)
        {
            string outputColor = "white";
            foreColor = "black";
            try
            {
                if (color == "Red")
                {
                    outputColor = "red";
                    foreColor = "white";
;                }
                else if(color == "Green")
                {
                    outputColor = "green";
                    foreColor = "black";
                }
                else if (color == "Blue")
                {
                    outputColor = "#008ae6";
                    foreColor = "black";
                }
                else if (color == "Orange")
                {
                    outputColor = "orange";
                    foreColor = "black";
                }
                else if (color == "Purple")
                {
                    outputColor = "#7e6991";
                    foreColor = "white";
                }
                else if (color == "Peachpuff")
                {
                    outputColor = "#fcd5b4";
                    foreColor = "black";
                }
                else if(color== "Yellow")
                {
                    outputColor = "yellow";
                    foreColor = "black";
                }
                else if (color == "Pink")
                {
                    outputColor = "#ff00ff";
                    foreColor = "white";
                }
            }
            catch(Exception ex)
            {

            }
            return outputColor;
        }
        private List<ListItem> getParameters()
        {
            List<ListItem> listItemList = new List<ListItem>();
            listItemList.Add(new ListItem("M/c Shop Prod Eff.", "Eff"));
            listItemList.Add(new ListItem("M/c Shop OEE", "OEE"));
            listItemList.Add(new ListItem("M/c shop downtime", "Downtime"));
            listItemList.Add(new ListItem("M/c Shop Rej", "Rej"));
            return listItemList;
        }

        //protected void imgBtnSwitch_Click(object sender, ImageClickEventArgs e)
        //{
        //    if (imgBtnSwitch.ToolTip == "Switch to ANDON Mode")
        //    {
        //        TickCount = 0;
        //        SkipCount = 0;
        //        dashboardInterval.Enabled = true;
        //        dashboardInterval.Interval = 10000;
        //        DesktopMode = false;
        //        bindAndonData();
        //        imgBtnSwitch.ToolTip = "Switch to DESKTOP Mode";
        //        imgBtnSwitch.ImageUrl = "Images/desktop1.png";
               
        //    }
        //    else
        //    {
        //        DesktopMode = true;
        //        bindAndonData();
        //        dashboardInterval.Enabled = false;
        //        imgBtnSwitch.ToolTip = "Switch to ANDON Mode";
        //        imgBtnSwitch.ImageUrl = "Images/andon.jpg";
        //    }
        //}

     
        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            bindAndonData();
        }

        protected void btnmodaldeToggle_Click(object sender, EventArgs e)
        {
            if (modeToggle.Checked)
            {
                int JagadevaFlipInterval = 30;
                TickCount = 0;
                SkipCount = 0;
                dashboardInterval.Enabled = true;
                int.TryParse(ConfigurationManager.AppSettings["JagadevaAndonFlipInterval"].ToString(), out JagadevaFlipInterval);
                dashboardInterval.Interval = JagadevaFlipInterval;
                //dashboardInterval.Enabled = false;
                DesktopMode = false;
                bindAndonData();
                //imgBtnSwitch.ToolTip = "Switch to DESKTOP Mode";
                //imgBtnSwitch.ImageUrl = "Images/desktop1.png";
                tdDate.Visible = false;
                tdButton.Visible = false;
            }
            else
            {
                tdDate.Visible = true;
                tdButton.Visible = true;
                txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                dashboardInterval.Enabled = false;
                DesktopMode = true;
                bindAndonData();
               
                //imgBtnSwitch.ToolTip = "Switch to ANDON Mode";
                //imgBtnSwitch.ImageUrl = "Images/andon.jpg";
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                bindAndonData();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "unblockLoader", "$.unblockUI({ });", true);
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "unblockLoader", "$.unblockUI({ });", true);
            }
          
        }

        protected void dashboardInterval_Tick(object sender, EventArgs e)
        {
            try
            {
                TickCount++;
                if (TickCount == 1)
                {
                    SkipCount = 0;
                }
                bindAndonData();
                SkipCount++;
            }
            catch(Exception ex)
            {

            }
        }
    }
   
}