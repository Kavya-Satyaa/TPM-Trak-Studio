using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Cumi.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Cumi
{
    public partial class ProcessParameterDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindPlant();
                BindShift();
                BindDefMachine();
                BindDefParameters();
                cbAutorefresh.Checked = true;
                //btnView_Click(null, null);
            }
        }
        private void BindShift()
        {
            try
            {
                List<PDTData> shiftDetails = DataBaseAccess.getShiftTimeDetails(DateTime.Now);
                List<ListItem> list = new List<ListItem>();
                foreach (PDTData data in shiftDetails)
                {
                    list.Add(new ListItem() { Text = data.ShiftName, Value = data.FromDateTime + ";;" + data.ToDateTime });
                }
                ddlShift.DataSource = list;
                ddlShift.DataTextField = "Text";
                ddlShift.DataValueField = "Value";
                ddlShift.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindShift: " + ex.Message);
            }
        }
        private void BindPlant()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlantsForPlantInfo();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPlant: " + ex.Message);
            }
        }
        private void BindDefMachine()
        {
            try
            {
                List<string> list = CumiDBAccess.GetAllMachinedByPlant(ddlPlant.SelectedValue);
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPlant: " + ex.Message);
            }
        }
        private void BindDefParameters()
        {
            try
            {
                List<MachineParameters> list = CumiDBAccess.GetAllParametersByMachines(ddlMachine.SelectedValue);
                ToConvertListToDataTable convert = new ToConvertListToDataTable();
                DataTable dt = convert.ToDataTable(list);
                ddlParameters.DataSource = dt;
                ddlParameters.DataBind();
                ddlParameters.DataTextField = "DisplayText";
                ddlParameters.DataValueField = "ParameterID";
                ddlParameters.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPlant: " + ex.Message);
            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<string> BindMachine(string plant)
        {
            List<string> list = new List<string>();
            try
            {
                list = CumiDBAccess.GetAllMachinedByPlant(plant);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachine: " + ex.Message);
            }
            return list;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<MachineParameters> BindParameters(string machines)
        {
            List<MachineParameters> list = new List<MachineParameters>();
            try
            {
                // to do remove hard coded
                //machines = machines == "" ? "225_1 PRESS" : machines;
                list = CumiDBAccess.GetAllParametersByMachines(machines);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindParameters: " + ex.Message);
            }
            return list;
        }
        //protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        BindMachine();

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog("ddlPlant_SelectedIndexChanged: " + ex.Message);
        //    }
        //}
        //private void BindPPDetails()
        //{

        //    try
        //    {
        //DataTable dt = new DataTable();
        ////list.Add(new ProcessParametersInfo() { ParameterName = "Hyd. Pressure Top", MinMaxVisibility = "table-row", MinValue = "20", MaxValue = "30" });
        ////list.Add(new ProcessParametersInfo() { ParameterName = "Ram Stroke Top", ValueVisibility = "table-row", ParameterValue = "20" });
        //string shiftvalue = ddlShift.SelectedValue;
        //string fromdate = Regex.Split(ddlShift.SelectedValue, ";;")[0];
        //string todate = Regex.Split(ddlShift.SelectedValue, ";;")[1];
        //List<ProcessParametersInfo> list = CumiDBAccess.GetProcessParameterDetails(ddlPlant.SelectedValue, "'" + ddlMachine.SelectedValue + "'", fromdate, todate, ddlShift.SelectedItem.ToString(), out dt);

        //lvParameterDetails.DataSource = list;
        //lvParameterDetails.DataBind();

        //DataTable dtTotal = new DataTable();
        //DataTable dtRightTotal = new DataTable();
        //DataTable dtRightTotal_1 = new DataTable();
        //DataTable dt1 = CumiDBAccess.GetProductionRejectionDetails("[SP_ShiftProductionAndRejectionData_CUMI]", "RejectionScreen", "Report", ddlPlant.SelectedValue, "'" + ddlMachine.SelectedValue + "'", fromdate, todate, ddlShift.SelectedItem.ToString(), "", "", out dtTotal, out dtRightTotal, out dtRightTotal_1);


        //list = new List<ProcessParametersInfo>();
        //list.Add(new ProcessParametersInfo() { ParameterName = "Product Description", Value = dt.AsEnumerable().Select(x => x.Field<string>("Componentid")).FirstOrDefault() });
        //list.Add(new ProcessParametersInfo() { ParameterName = "Production Shift Total", Value = dt1.AsEnumerable().Sum(x => Convert.ToDouble(x.Field<double>("ProdQty"))).ToString() });
        //list.Add(new ProcessParametersInfo() { ParameterName = "Emp ID 1", Value = dt.AsEnumerable().Select(x => x.Field<string>("OperatorName")).FirstOrDefault() });
        //if (dt.Rows.Count > 0)
        //{
        //    list.Add(new ProcessParametersInfo() { ParameterName = "Emp ID 2", Value = dt.AsEnumerable().Select(x => x.Field<string>("OperatorName")).LastOrDefault() });
        //}
        //else
        //{
        //    list.Add(new ProcessParametersInfo() { ParameterName = "Emp ID 2", Value = "" });
        //}

        //list.Add(new ProcessParametersInfo() { ParameterName = "Shift Loss Summation", Value = dt1.AsEnumerable().Sum(x => Convert.ToDouble(x.Field<double>("Qty"))).ToString() });


        //lvOtherDetails.DataSource = list;
        //lvOtherDetails.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog("BindPPDetails: " + ex.Message);
        //    }
        //}

        //protected void btnView_Click(object sender, EventArgs e)
        //{
        //    BindPPDetails();
        //}

        //protected void rblViewType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindPPDetails();
        //}
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<ProcessParametersInfo> GetParameterDetails(string shift, string shiftdate, string plant, string machine, string parameters, string date, bool autoRefresh)
        {
            ProcessParametersInfo list = new ProcessParametersInfo();
            List<ProcessParametersInfo> machinelist = new List<ProcessParametersInfo>();
            try
            {

                if (machine != "" && machine != null)
                {
                    string shiftvalue = shift;
                    //string fromdate = Regex.Split(shiftdate, ";;")[0];
                    //string todate = Regex.Split(shiftdate, ";;")[1];


                    string fromdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string todate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    if (!autoRefresh)
                    {
                        fromdate = date + " " + (Regex.Split(shiftdate, ";;")[0]).Split(' ')[1];
                        todate = date + " " + (Regex.Split(shiftdate, ";;")[1]).Split(' ')[1];
                    }
                    else
                    {
                        fromdate = DateTime.Now.ToString("yyyy-MM-dd") + " " + (Regex.Split(shiftdate, ";;")[0]).Split(' ')[1];
                        todate = DateTime.Now.ToString("yyyy-MM-dd") + " " + (Regex.Split(shiftdate, ";;")[1]).Split(' ')[1];
                    }

                    DataTable dt = new DataTable();
                    DataTable dtParam6 = new DataTable();

                    //List<MachineProcessParametersInfo> MachineParameters = new List<MachineProcessParametersInfo>();

                    //List<MachineProcessParametersInfo> MachineParameters = CumiDBAccess.GetMachineProcessParameterDetails(plant, machine, fromdate, todate, shift, out dt);

                    //var machines = machine.Split(',');

                    //for (int i = 0; i < machines.Length; i++)
                    //{
                    List<ProcessParametersInfo> paramlistt = CumiDBAccess.GetProcessParameterDetails(plant, machine, parameters, fromdate, todate, shift, "", out dt, out dtParam6);

                    var machinesss = machine.Split(',');
                    foreach (var mac in machinesss)
                    {
                        list = new ProcessParametersInfo();
                        list.MachineId = mac;

                        list.ParameterDetails = paramlistt.Where(x => x.MachineId == mac).OrderBy(k => k.ParameterId).ToList();

                        DataTable dtTotal = new DataTable();
                        DataTable dtRightTotal = new DataTable();
                        DataTable dtRightTotal_1 = new DataTable();
                        DataTable dt1 = CumiDBAccess.GetProductionRejectionDetails("[SP_ShiftProductionAndRejectionData_CUMI]", "RejectionScreen", "Report", plant, "'" + mac + "'", date, date, shift, "", "", true, out dtTotal, out dtRightTotal, out dtRightTotal_1);


                        List<ProcessParametersInfo> paramlist = new List<ProcessParametersInfo>();
                        paramlist.Add(new ProcessParametersInfo() { ParameterName = "Product Description", Value = dt.AsEnumerable().Select(x => x.Field<string>("CompDescription")).FirstOrDefault() });
                        paramlist.Add(new ProcessParametersInfo() { ParameterName = "Production Shift Total", Value = dt1.AsEnumerable().Sum(x => Convert.ToDouble(x.Field<double>("ProdQty"))).ToString() });
                        paramlist.Add(new ProcessParametersInfo() { ParameterName = "Emp ID 1", Value = dt.AsEnumerable().Select(x => x.Field<string>("OperatorName")).FirstOrDefault() });
                        if (dt.Rows.Count > 0)
                        {
                            paramlist.Add(new ProcessParametersInfo() { ParameterName = "Emp ID 2", Value = dt.AsEnumerable().Select(x => x.Field<string>("OperatorName")).LastOrDefault() });
                        }
                        else
                        {
                            paramlist.Add(new ProcessParametersInfo() { ParameterName = "Emp ID 2", Value = "" });
                        }

                        paramlist.Add(new ProcessParametersInfo() { ParameterName = "Shift Loss Summation", Value = dt1.AsEnumerable().Sum(x => Convert.ToDouble(x.Field<double>("Qty"))).ToString() });

                        list.OtherDetails = paramlist;
                        machinelist.Add(list);
                    }

                    //}
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetComponentDetails: " + ex.Message);
            }
            return machinelist;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetCurrentShift()
        {
            string currentShift = "";
            try
            {

                List<ListItem> shiftDetails = CumiDBAccess.GetCurrentShiftDetails();
                if (shiftDetails.Count > 0)
                {
                    currentShift = Util.GetDateTime(shiftDetails.Where(x => x.Text == "ShiftStart").Select(x => x.Value).FirstOrDefault()).ToString("yyyy-MM-dd HH:mm:ss") + ";;" + Util.GetDateTime(shiftDetails.Where(x => x.Text == "ShiftEnd").Select(x => x.Value).FirstOrDefault()).ToString("yyyy-MM-dd HH:mm:ss");
                    // currentShift = shiftDetails.Where(x => x.Text == "ShiftName").Select(x => x.Value).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetCurrentShift: " + ex.Message);
            }
            return currentShift;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static int GetInterval()
        {
            int interval = 10;
            try
            {
                interval = 1000 * Convert.ToInt32(WebConfigurationManager.AppSettings["CumiPPDashboardInterval"].ToString());

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetInterval: " + ex.Message);
            }
            return interval;
        }

        //protected void ddlMachine_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    List<MachineParameters> list = new List<MachineParameters>();
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        //dt = CumiDBAccess.GetAllParametersByMachines(ddlMachine.SelectedValue);

        //        ddlParameters.DataSource = dt;
        //        ddlParameters.DataBind();
        //        ddlParameters.DataTextField = "DisplayText";
        //        ddlParameters.DataValueField = "ParameterID";
        //        ddlParameters.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog("BindParameters: " + ex.Message);
        //    }
        //}

        //protected void ddlMachine_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //List<MachineParameters> list = new List<MachineParameters>();
        //DataTable dt = new DataTable();
        //    try
        //    {
        //        dt = CumiDBAccess.GetAllParametersByMachines(ddlMachine.SelectedValue);

        //        ddlParameters.DataSource = dt;
        //        ddlParameters.DataBind();
        //        ddlParameters.DataTextField = "DisplayText";
        //        ddlParameters.DataValueField = "ParameterID";
        //        ddlParameters.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog("BindParameters: " + ex.Message);
        //    }
        //    //return list;
        //}
    }
}