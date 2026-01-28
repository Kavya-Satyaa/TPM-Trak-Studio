using BusinessClassLibrary;
using Elmah;
using ModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class PPMPage : System.Web.UI.Page
    {
        public string ViewType = string.Empty;
        public string Type = string.Empty;
        public string maxvalue = ConfigurationManager.AppSettings["ShowMaxPPMType"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                if (Request.QueryString["machineId"] != "")
                {
                    Session["ppmMachineId"] = ViewType = Request.QueryString["machineId"].ToString();
                    Session["ppmShiftId"] = Request.QueryString["shiftId"].ToString();
                    Session["ppmYear"] = Request.QueryString["year"].ToString();
                    Session["ppmMonth"] = Request.QueryString["month"].ToString();
                    Session["ppmDay"] = Request.QueryString["day"].ToString();
                    Session["ppmcomponentId"] = Request.QueryString["componentId"].ToString();
                    Session["ppmemployeeId"] = Request.QueryString["employeeId"].ToString();
                    Session["ppmplantId"] = Request.QueryString["plantId"].ToString();
                    Session["ppmView"] = Type = Request.QueryString["View"].ToString();
                    Session["ppmCell"] = Request.QueryString["Cell"].ToString();
                    //Session["MachineDescription"] = Request.QueryString["machinedescription"].ToString();

                    Bindgrid();
                }
            }
        }

        private void Bindgrid()
        {
            string strShift = Session["ppmShiftId"].ToString(), plantId = Session["ppmplantId"].ToString(), strMonth = Session["ppmMonth"].ToString(),
                componentId = Session["ppmcomponentId"].ToString(), employeeId = Session["ppmemployeeId"].ToString(), strYear = Session["ppmYear"].ToString(),
                strDay = Session["ppmDay"].ToString(), machineId = Session["ppmMachineId"].ToString(), ViewType = Session["ppmView"].ToString(), cell = Session["ppmCell"].ToString();
            try
            {
                string strDate = strYear + " " + strMonth + " " + strDay;
                if (ViewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                {
                    ViewType = "machinewise";
                    gridPPM.Columns[1].Visible = false;
                    gridPPM.Columns[1].ItemStyle.Width = 0;
                }
                else if (ViewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                {
                    ViewType = "plantwise";
                    plantId = machineId;
                    machineId = "";
                    gridPPM.Columns[1].Visible = false;
                    gridPPM.Columns[1].ItemStyle.Width = 0;
                }
                else if (ViewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                {
                    ViewType = "componentwise";
                    componentId = machineId;
                    machineId = "";

                }
                else if (ViewType.Equals("OperatorwiseView", StringComparison.OrdinalIgnoreCase))
                {
                    ViewType = "operatorwise";
                    employeeId = machineId;
                    machineId = "";
                    gridPPM.Columns[1].Visible = false;
                    gridPPM.Columns[1].ItemStyle.Width = 0;
                }
                else if (ViewType.Equals("CellwiseView", StringComparison.OrdinalIgnoreCase))
                {
                    ViewType = "Cellwise";
                    employeeId = "";
                    cell = machineId;
                    machineId = "";
                    gridPPM.Columns[1].Visible = false;
                    gridPPM.Columns[1].ItemStyle.Width = 0;
                }

                if (strShift.Equals("All", StringComparison.OrdinalIgnoreCase))
                    strShift = "";
                if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
                    plantId = "";

                if (componentId.Equals("All", StringComparison.OrdinalIgnoreCase))
                    componentId = "";
                if (employeeId.Equals("All", StringComparison.OrdinalIgnoreCase))
                    employeeId = "";

                #region "Month condition------------------"
                if (strMonth.Equals("Jan", StringComparison.OrdinalIgnoreCase))
                    strMonth = "01";
                else if (strMonth.Equals("Feb", StringComparison.OrdinalIgnoreCase))
                    strMonth = "02";
                else if (strMonth.Equals("Mar", StringComparison.OrdinalIgnoreCase))
                    strMonth = "03";
                else if (strMonth.Equals("Apr", StringComparison.OrdinalIgnoreCase))
                    strMonth = "04";
                else if (strMonth.Equals("May", StringComparison.OrdinalIgnoreCase))
                    strMonth = "05";
                else if (strMonth.Equals("Jun", StringComparison.OrdinalIgnoreCase))
                    strMonth = "06";
                else if (strMonth.Equals("Jul", StringComparison.OrdinalIgnoreCase))
                    strMonth = "07";
                else if (strMonth.Equals("Aug", StringComparison.OrdinalIgnoreCase))
                    strMonth = "08";
                else if (strMonth.Equals("Sep", StringComparison.OrdinalIgnoreCase))
                    strMonth = "09";
                else if (strMonth.Equals("Oct", StringComparison.OrdinalIgnoreCase))
                    strMonth = "10";
                else if (strMonth.Equals("Nov", StringComparison.OrdinalIgnoreCase))
                    strMonth = "11";
                else if (strMonth.Equals("Dec", StringComparison.OrdinalIgnoreCase))
                    strMonth = "12";
                #endregion
                DataTable dt = new DataTable();
                if (strYear != "" && strMonth != "" && strDay != "")
                {
                    strYear = strYear + "-" + strMonth + "-" + strDay;
                    dt = BindCockpitView.PPMFirstLeveldata(strYear, strShift, plantId, machineId, "DAY", componentId, employeeId, ViewType, cell, "");
                }
                else if (strYear != "" && strMonth != "")
                {
                    strYear = strYear + "-" + strMonth + "-" + "01";
                    dt = BindCockpitView.PPMFirstLeveldata(strYear, strShift, plantId, machineId, "MONTH", componentId, employeeId, ViewType, cell, "");
                }
                else if (strYear != "")
                {
                    strYear = strYear + "-" + "01" + "-" + "01";
                    dt = BindCockpitView.PPMFirstLeveldata(strYear, strShift, plantId, machineId, "YEAR", componentId, employeeId, ViewType, cell, "");
                }
                DataColumnCollection col = dt.Columns;
                if (!col.Contains("ComponentID"))
                {
                    dt.Columns.Add("ComponentID");
                }
                if (!col.Contains("MachineID"))
                {
                    dt.Columns.Add("MachineID");
                }
                if (!col.Contains("Operationno"))
                {
                    dt.Columns.Add("Operationno");
                }
                gridPPM.DataSource = dt;
                gridPPM.DataBind();
                if (ViewType.Equals("Componentwise", StringComparison.OrdinalIgnoreCase))
                {
                    gridPPM.Columns[0].Visible = false;
                    gridPPM.Columns[1].Visible = true;
                    IgnoreMachineID.Visible = true;
                    lblMachineID.Visible = true;
                    lblComponentID.Visible = false;
                    IgnoreComponentID.Visible = false;
                    if (IgnoreMachineID.Checked)
                        gridPPM.Columns[1].Visible = false;
                    if (IgnoreOperation.Checked)
                        gridPPM.Columns[2].Visible = false;
                }
                else
                {
                    gridPPM.Columns[1].Visible = false;
                    gridPPM.Columns[0].Visible = true;
                    IgnoreMachineID.Visible = false;
                    lblMachineID.Visible = false;
                    lblComponentID.Visible = true;
                    IgnoreComponentID.Visible = true;
                    if (IgnoreComponentID.Checked)
                        gridPPM.Columns[0].Visible = false;
                    if (IgnoreOperation.Checked)
                        gridPPM.Columns[2].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static PPMDATA Getdata(string SelectedValue, string OperationNo, string Type)
        {
            var chart = new PPMDATA
            {

            };
            DataTable List = new DataTable();
            try
            {
                OperationNo = OperationNo.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : OperationNo;
                int maxColvalue = Convert.ToInt32(ConfigurationManager.AppSettings["ShowMaxPPMType"].ToString());
                string MachineID = HttpContext.Current.Session["ppmMachineId"].ToString();
                string Date = HttpContext.Current.Session["ppmDay"].ToString();
                string Month = HttpContext.Current.Session["ppmMonth"].ToString();
                string Year = HttpContext.Current.Session["ppmYear"].ToString();
                string ShiftID = HttpContext.Current.Session["ppmShiftId"].ToString();
                string PlantId = HttpContext.Current.Session["ppmplantId"].ToString();
                string Component = HttpContext.Current.Session["ppmcomponentId"].ToString();
                string EmployeeID = HttpContext.Current.Session["ppmemployeeId"].ToString();
                string View = HttpContext.Current.Session["ppmView"].ToString();
                string Cell = HttpContext.Current.Session["ppmCell"].ToString();
                string Selectedvalue = SelectedValue;
                Type = HttpContext.Current.Session["ppmView"].ToString();
                if (Type.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                {
                    View = "machinewise";
                    Component = Selectedvalue;
                }
                else if (Type.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                {
                    View = "plantwise";
                    PlantId = MachineID;
                    MachineID = "";
                    Component = Selectedvalue;
                }
                else if (Type.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                {
                    View = "componentwise";
                    Component = MachineID;
                    
                    MachineID = Selectedvalue;
                }
                else if (Type.Equals("OperatorwiseView", StringComparison.OrdinalIgnoreCase))
                {
                    View = "operatorwise";
                    EmployeeID = MachineID;
                    MachineID = ""; 
                    Component = Selectedvalue;
                }
                else if (Type.Equals("CellwiseView", StringComparison.OrdinalIgnoreCase))
                {
                    View = "Cellwise";
                    Cell = MachineID;
                    MachineID = ""; 
                    Component = Selectedvalue;
                }
                #region "Month condition------------------"
                if (Month.Equals("Jan", StringComparison.OrdinalIgnoreCase))
                    Month = "01";
                else if (Month.Equals("Feb", StringComparison.OrdinalIgnoreCase))
                    Month = "02";
                else if (Month.Equals("Mar", StringComparison.OrdinalIgnoreCase))
                    Month = "03";
                else if (Month.Equals("Apr", StringComparison.OrdinalIgnoreCase))
                    Month = "04";
                else if (Month.Equals("May", StringComparison.OrdinalIgnoreCase))
                    Month = "05";
                else if (Month.Equals("Jun", StringComparison.OrdinalIgnoreCase))
                    Month = "06";
                else if (Month.Equals("Jul", StringComparison.OrdinalIgnoreCase))
                    Month = "07";
                else if (Month.Equals("Aug", StringComparison.OrdinalIgnoreCase))
                    Month = "08";
                else if (Month.Equals("Sep", StringComparison.OrdinalIgnoreCase))
                    Month = "09";
                else if (Month.Equals("Oct", StringComparison.OrdinalIgnoreCase))
                    Month = "10";
                else if (Month.Equals("Nov", StringComparison.OrdinalIgnoreCase))
                    Month = "11";
                else if (Month.Equals("Dec", StringComparison.OrdinalIgnoreCase))
                    Month = "12";
                #endregion

                if (Year != "" && Month != "" && Date != "")
                {
                    Year = Year + "-" + Month + "-" + Date;
                    List = DataBaseAccess.PPMSecondLeveldata(Year, ShiftID, PlantId, MachineID, "DAY", Component, EmployeeID, View, OperationNo, Cell);
                }
                else if (Year != "" && Month != "")
                {
                    Year = Year + "-" + Month + "-" + "01";
                    List = DataBaseAccess.PPMSecondLeveldata(Year, ShiftID, PlantId, MachineID, "MONTH", Component, EmployeeID, View, OperationNo, Cell);
                }
                else if (Year != "")
                {
                    Year = Year + "-" + "01" + "-" + "01";
                    List = DataBaseAccess.PPMSecondLeveldata(Year, ShiftID, PlantId, MachineID, "YEAR", Component, EmployeeID, View, OperationNo, Cell);
                }
                DataColumnCollection col = List.Columns;
                if (!col.Contains("ComponentID"))
                {
                    List.Columns.Add("ComponentID");
                }
                if (!col.Contains("MachineID"))
                {
                    List.Columns.Add("MachineID");
                }
                if (!col.Contains("Operationno"))
                {
                    List.Columns.Add("Operationno");
                }
                DataView dv = List.DefaultView;
                dv.Sort = "PPM desc";
                DataTable sortedDT = dv.ToTable();

                List<string> cat = sortedDT.AsEnumerable().Select(r => r.Field<string>("RejectionID")).ToList();


                //cat.Add("Other");
                chart.categories = new List<string>();
                for (int i = 0; i < cat.Count; i++)
                {
                    //if (i == maxColvalue)
                    //{
                    //    chart.categories.Add("Others");
                    //}
                    string value = cat[i];
                    if (!string.IsNullOrEmpty(value))
                        chart.categories.Add(value);
                }
                if (maxColvalue > cat.Count)
                {
                    maxColvalue = cat.Count;
                }
                List<double> Rejectiondata = sortedDT.AsEnumerable().Select(r => r.Field<double>("PPM")).ToList();
                chart.series = new List<double>();
                for (int i = 0; i < chart.categories.Count; i++)
                {
                    double myNumber = Convert.ToDouble(Rejectiondata[i].ToString());
                    chart.series.Add(myNumber);
                }

                List<double> RejectionQty = sortedDT.AsEnumerable().Select(r => r.Field<double>("RejCount")).ToList();
                chart.RejQty = new List<double>();
                for (int i = 0; i < chart.categories.Count; i++)
                {
                    double myNumber = Convert.ToDouble(RejectionQty[i].ToString());
                    chart.RejQty.Add(myNumber);
                }

                chart.Drilldown = new List<double>();
                for (int i = maxColvalue; i < Rejectiondata.Count; i++)
                {
                    double myNumber = Convert.ToDouble(Rejectiondata[i].ToString());
                    chart.Drilldown.Add(myNumber);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return chart;
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string Param = "";
            string strShift = Session["ppmShiftId"].ToString(), plantId = Session["ppmplantId"].ToString(), strMonth = Session["ppmMonth"].ToString(),
               componentId = Session["ppmcomponentId"].ToString(), employeeId = Session["ppmemployeeId"].ToString(), strYear = Session["ppmYear"].ToString(),
               strDay = Session["ppmDay"].ToString(), machineId = Session["ppmMachineId"].ToString(), ViewType = Session["ppmView"].ToString(), cell = Session["ppmcell"].ToString();
            try
            {
                string strDate = strYear + " " + strMonth + " " + strDay;
                if (ViewType.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                {
                    ViewType = "machinewise";
                    gridPPM.Columns[1].Visible = false;
                    gridPPM.Columns[1].ItemStyle.Width = 0;
                }
                else if (ViewType.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                {
                    ViewType = "plantwise";
                    plantId = machineId;
                    machineId = "";
                    gridPPM.Columns[1].Visible = false;
                    gridPPM.Columns[1].ItemStyle.Width = 0;
                }
                else if (ViewType.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                {
                    ViewType = "componentwise";
                    componentId = machineId;
                    machineId = "";

                }
                else if (ViewType.Equals("OperatorwiseView", StringComparison.OrdinalIgnoreCase))
                {
                    ViewType = "operatorwise";
                    employeeId = machineId;
                    machineId = "";
                    gridPPM.Columns[1].Visible = false;
                    gridPPM.Columns[1].ItemStyle.Width = 0;
                }
                else if (ViewType.Equals("CellwiseView", StringComparison.OrdinalIgnoreCase))
                {
                    ViewType = "Cellwise";
                    cell = machineId;
                    employeeId = "";
                    machineId = "";

                    gridPPM.Columns[1].Visible = false;
                    gridPPM.Columns[1].ItemStyle.Width = 0;
                }
                if (ViewType.Equals("componentwise", StringComparison.OrdinalIgnoreCase))
                {
                    if (IgnoreMachineID.Checked && IgnoreOperation.Checked)
                    {
                        Param = "IgnoreMachineOperation";
                    }
                    else if (IgnoreOperation.Checked)
                    {
                        Param = "IgnoreOperation";
                    }
                    else if (IgnoreMachineID.Checked)
                    {
                        Param = "IgnoreMachine";
                    }
                    else
                    {
                        Param = "";
                    }
                }
                else
                {
                    if (IgnoreComponentID.Checked && IgnoreOperation.Checked)
                    {
                        Param = "IgnoreComponentOperation";
                    }
                    else if (IgnoreOperation.Checked)
                    {
                        Param = "IgnoreOperation";
                    }
                    else if (IgnoreComponentID.Checked)
                    {
                        Param = "IgnoreComponent";
                    }
                    else
                    {
                        Param = "";
                    }
                }
                if (strShift.Equals("All", StringComparison.OrdinalIgnoreCase))
                    strShift = "";
                if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
                    plantId = "";

                if (componentId.Equals("All", StringComparison.OrdinalIgnoreCase))
                    componentId = "";
                if (employeeId.Equals("All", StringComparison.OrdinalIgnoreCase))
                    employeeId = "";

                #region "Month condition------------------"
                if (strMonth.Equals("Jan", StringComparison.OrdinalIgnoreCase))
                    strMonth = "01";
                else if (strMonth.Equals("Feb", StringComparison.OrdinalIgnoreCase))
                    strMonth = "02";
                else if (strMonth.Equals("Mar", StringComparison.OrdinalIgnoreCase))
                    strMonth = "03";
                else if (strMonth.Equals("Apr", StringComparison.OrdinalIgnoreCase))
                    strMonth = "04";
                else if (strMonth.Equals("May", StringComparison.OrdinalIgnoreCase))
                    strMonth = "05";
                else if (strMonth.Equals("Jun", StringComparison.OrdinalIgnoreCase))
                    strMonth = "06";
                else if (strMonth.Equals("Jul", StringComparison.OrdinalIgnoreCase))
                    strMonth = "07";
                else if (strMonth.Equals("Aug", StringComparison.OrdinalIgnoreCase))
                    strMonth = "08";
                else if (strMonth.Equals("Sep", StringComparison.OrdinalIgnoreCase))
                    strMonth = "09";
                else if (strMonth.Equals("Oct", StringComparison.OrdinalIgnoreCase))
                    strMonth = "10";
                else if (strMonth.Equals("Nov", StringComparison.OrdinalIgnoreCase))
                    strMonth = "11";
                else if (strMonth.Equals("Dec", StringComparison.OrdinalIgnoreCase))
                    strMonth = "12";
                #endregion
                if (strYear != "" && strMonth != "" && strDay != "")
                {
                    strYear = strYear + "-" + strMonth + "-" + strDay;
                    dt = BindCockpitView.PPMFirstLeveldata(strYear, strShift, plantId, machineId, "DAY", componentId, employeeId, ViewType, cell, Param);

                }
                else if (strYear != "" && strMonth != "")
                {
                    strYear = strYear + "-" + strMonth + "-" + "01";
                    dt = BindCockpitView.PPMFirstLeveldata(strYear, strShift, plantId, machineId, "MONTH", componentId, employeeId, ViewType, cell, Param);
                }
                else if (strYear != "")
                {
                    strYear = strYear + "-" + "01" + "-" + "01";
                    dt = BindCockpitView.PPMFirstLeveldata(strYear, strShift, plantId, machineId, "YEAR", componentId, employeeId, ViewType, cell, Param);
                }
                DataColumnCollection col = dt.Columns;
                if (!col.Contains("ComponentID"))
                {
                    dt.Columns.Add("ComponentID");
                }
                if (!col.Contains("MachineID"))
                {
                    dt.Columns.Add("MachineID");
                }
                if (!col.Contains("Operationno"))
                {
                    dt.Columns.Add("Operationno");
                }
                gridPPM.Columns[0].Visible = true;
                gridPPM.Columns[1].Visible = true;
                gridPPM.Columns[2].Visible = true;
                if (ViewType.Equals("Componentwise", StringComparison.OrdinalIgnoreCase))
                {
                    gridPPM.Columns[0].Visible = false;
                    IgnoreMachineID.Visible = true;
                    IgnoreComponentID.Visible = false;
                    lblMachineID.Visible = true;
                    lblComponentID.Visible = false;
                    if (IgnoreMachineID.Checked)
                        gridPPM.Columns[1].Visible = false;
                    if (IgnoreOperation.Checked)
                        gridPPM.Columns[2].Visible = false;
                }
                else
                {
                    gridPPM.Columns[1].Visible = false;
                    IgnoreMachineID.Visible = false;
                    IgnoreComponentID.Visible = true;
                    lblMachineID.Visible = false;
                    lblComponentID.Visible = true;
                    if (IgnoreComponentID.Checked)
                        gridPPM.Columns[0].Visible = false;
                    if (IgnoreOperation.Checked)
                        gridPPM.Columns[2].Visible = false;
                }
                gridPPM.DataSource = dt;
                gridPPM.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }

    public class PPMDATA
    {
        public List<string> categories { get; set; }
        public List<double> Drilldown { get; set; }
        public List<double> series { get; set; }
        public List<double> RejQty { get; set; }
    }
}