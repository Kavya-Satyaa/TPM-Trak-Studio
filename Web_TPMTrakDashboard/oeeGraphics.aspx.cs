using Elmah;
using ModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class oeeGraphics : System.Web.UI.Page
    {
        string MachineID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                if (Request.QueryString["machineId"] != "")
                {
                    Session["MachineId"] = Request.QueryString["machineId"].ToString();
                    MachineID = Request.QueryString["machineId"].ToString();
                }
                BindMachines();
                btnRefresh_Click(null, null);
            }
        }

        private void BindMachines()
        {
            try
            {
                var allMachineName = VDGDataBaseAccess.GetAllMachines("All");
                if (allMachineName != null && allMachineName.Count > 0)
                {
                    ddlMachineId.DataSource = allMachineName;
                    ddlMachineId.DataBind();
                    if (allMachineName.Contains(MachineID))
                    {
                        ddlMachineId.SelectedValue = MachineID;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            string fromDt = string.Empty, toDt = string.Empty;
            DateTime date = DateTime.Now;
            DateTime fDate = DateTime.Now;
            DateTime tDate = DateTime.Now;
            try
            {

                if (!IsPostBack)
                {
                    if (Request.QueryString["machineId"] != "")
                    {
                        fromDt = Request.QueryString["fromDate"].ToString();
                        toDt = Request.QueryString["toDate"].ToString();
                        ddlMachineId.SelectedValue = Request.QueryString["machineId"].ToString();
                    }

                    // txtFromDate.Text=
                    fDate = Util.GetDateTime(fromDt);
                    tDate = Util.GetDateTime(toDt);
                    //DateTime.TryParse(fromDt, out date);
                    //{
                    //    fDate = date;
                    //}
                    //DateTime.TryParse(toDt, out date);
                    //{
                    //    tDate = date;
                    //}
                    txtFromDate.Text = fDate.ToString("dd-MM-yyyy HH:mm");
                    txtToDate.Text = tDate.ToString("dd-MM-yyyy HH:mm");
                }
                double PE, AE, OEE, NOT, UT, ML, DT, TT, QE, RejCN, CN;
                string selectedMachine = string.Empty;
                DataTable dt = new DataTable();
                DateTime fromDate = DateTime.Now.Date;
                DateTime toDate = DateTime.Now.Date;
                fromDate = Util.GetDateTime(txtFromDate.Text.Trim());
                //if (!DateTime.TryParseExact(txtFromDate.Text.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDate))
                //{
                //    lblMessages.Text = "Please enter valid from date format.";
                //    return;
                //}
                //if (!DateTime.TryParseExact(txtToDate.Text.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out toDate))
                //{
                //    lblMessages.Text = "Please enter valid to date format.";
                //    return;
                //}
                toDate = Util.GetDateTime(txtToDate.Text.Trim());
                selectedMachine = ddlMachineId.SelectedValue;
                //if (CheckDateRange())
                //{
                Session["OEEGraph"] = dt = VDGDataBaseAccess.GetMachineData("s_GetCockpitData_WithTempTable_eshopx", fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), "", ddlMachineId.SelectedValue.ToString());
                //}
                //else
                //{
                //    Session["OEEGraph"] = dt = VDGDataBaseAccess.GetMachineData("s_GetCockpitData_eshopx", fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), "", ddlMachineId.SelectedValue.ToString());
                //}
                PE = (Math.Round(Convert.ToDouble(dt.Rows[0]["ProductionEfficiency"]), 2));
                AE = (Math.Round(Convert.ToDouble(dt.Rows[0]["AvailabilityEfficiency"]), 2));
                OEE = (Math.Round(Convert.ToDouble(dt.Rows[0]["OverAllEfficiency"]), 2));
                QE = (Math.Round(Convert.ToDouble(dt.Rows[0]["QualityEfficiency"]), 2));
                NOT = (Math.Round(Convert.ToDouble(dt.Rows[0]["CN"]) / 60, 2));
                CN = (Math.Round(Convert.ToDouble(dt.Rows[0]["Components"]), 2));
                RejCN = (Math.Round(Convert.ToDouble(dt.Rows[0]["RejCount"]), 2));
                double PDT = ChangeToMinutes((dt.Rows[0]["totalPDT"].ToString()));
                UT = Convert.ToDouble(dt.Rows[0]["UtilisedTime"]) / 60.0;
                ML = ChangeToMinutes((dt.Rows[0]["NetManagementLoss"]).ToString());
                //DT = ChangeToMinutes((dt.Rows[0]["DownTime"]).ToString());
                DT = ChangeToMinutes((dt.Rows[0]["NetDownTime"]).ToString());
                TT = ChangeToMinutes((dt.Rows[0]["TotalTime"]).ToString());
                //double actaulDownTime = DT - ML;
                double actaulDownTime = DT;
                lblTTMin.Text = TT.ToString("##.##");
                lblTT.Text = "TT=" + "(" + txtToDate.Text + ")" + "-" + "(" + txtFromDate.Text + ")";

                lblUTMin.Text = Math.Round(Math.Round(Convert.ToDecimal(UT), 2), 2).ToString();
                lblDTMin.Text = Math.Round(actaulDownTime, 2).ToString();
                //lblDT.Text = "DT =" + lblPPTMin.Text + "-" + lblOTMin.Text;

                lblPartProduced.Text = CN.ToString();
                lblRejectedParts.Text = RejCN.ToString();
                lblSPTMin.Text = NOT.ToString();
                //lblSPT.Text = "Σ (Standard Cycle Time * part count)";
                lblPLMin.Text = (TT - (UT + actaulDownTime)) == 0 ? "0" : (TT - (UT + actaulDownTime)).ToString("##.##");
                lblPL.Text = "PL =" + lblTTMin.Text + "- (" + lblUTMin.Text + "+" + lblDTMin.Text + ")";

                lblAEMin.Text = (AE / 100).ToString();
                lblAE.Text = "AE = [(" + lblTTMin.Text + " - " + lblPLMin.Text + ") - (" + lblDTMin.Text + ") / (" + lblTTMin.Text + " - " + lblPLMin.Text + " )]";
                lblAEInPerc.Text = AE.ToString();

                lblPEMin.Text = (PE / 100).ToString();
                lblPE.Text = "PE = [(" + lblSPTMin.Text + ")/" + lblUTMin.Text + "]";
                lblPEInPerc.Text = PE.ToString();

                lblQEMin.Text = (QE / 100).ToString();
                lblQE.Text = "QE = [(" + CN.ToString() + " - " + RejCN.ToString() + ")/" + CN.ToString() + "]";
                lblQEInPerc.Text = QE.ToString();

                lblOEEMin.Text = OEE.ToString();
                lblOEE.Text = "OEE = [" + lblAEMin.Text + "*" + lblPEMin.Text + "*" + lblQEMin.Text + "] * 100";
                lblOEEInPerc.Text = OEE.ToString();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        //protected void btnRefresh_Click(object sender, EventArgs e)
        //{
        //    string fromDt = string.Empty, toDt = string.Empty;
        //    DateTime date = DateTime.Now;
        //    DateTime fDate = DateTime.Now;
        //    DateTime tDate = DateTime.Now;
        //    try
        //    {

        //        if (!IsPostBack)
        //        {
        //            if (Request.QueryString["machineId"] != "")
        //            {
        //                fromDt = Request.QueryString["fromDate"].ToString();
        //                toDt = Request.QueryString["toDate"].ToString();
        //                ddlMachineId.SelectedValue = Request.QueryString["machineId"].ToString();
        //            }

        //            // txtFromDate.Text=
        //            fDate = Util.GetDateTime(fromDt);
        //            tDate = Util.GetDateTime(toDt);
        //            //DateTime.TryParse(fromDt, out date);
        //            //{
        //            //    fDate = date;
        //            //}
        //            //DateTime.TryParse(toDt, out date);
        //            //{
        //            //    tDate = date;
        //            //}
        //            txtFromDate.Text = fDate.ToString("dd-MM-yyyy HH:mm");
        //            txtToDate.Text = tDate.ToString("dd-MM-yyyy HH:mm");
        //        }
        //        double PE, AE, OEE, NOT, UT, ML, DT, TT, QE, RejCN,CN;
        //        string selectedMachine = string.Empty;
        //        DataTable dt = new DataTable();
        //        DateTime fromDate = DateTime.Now.Date;
        //        DateTime toDate = DateTime.Now.Date;
        //        fromDate = Util.GetDateTime(txtFromDate.Text.Trim());
        //        //if (!DateTime.TryParseExact(txtFromDate.Text.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDate))
        //        //{
        //        //    lblMessages.Text = "Please enter valid from date format.";
        //        //    return;
        //        //}
        //        //if (!DateTime.TryParseExact(txtToDate.Text.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out toDate))
        //        //{
        //        //    lblMessages.Text = "Please enter valid to date format.";
        //        //    return;
        //        //}
        //        toDate = Util.GetDateTime(txtToDate.Text.Trim());
        //        selectedMachine = ddlMachineId.SelectedValue;
        //        //if (CheckDateRange())
        //        //{
        //            Session["OEEGraph"] = dt = VDGDataBaseAccess.GetMachineData("s_GetCockpitData_WithTempTable_eshopx", fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), "", ddlMachineId.SelectedValue.ToString());
        //        //}
        //        //else
        //        //{
        //        //    Session["OEEGraph"] = dt = VDGDataBaseAccess.GetMachineData("s_GetCockpitData_eshopx", fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), "", ddlMachineId.SelectedValue.ToString());
        //        //}
        //        PE = (Math.Round(Convert.ToDouble(dt.Rows[0]["ProductionEfficiency"]), 2));
        //        AE = (Math.Round(Convert.ToDouble(dt.Rows[0]["AvailabilityEfficiency"]), 2));
        //        OEE = (Math.Round(Convert.ToDouble(dt.Rows[0]["OverAllEfficiency"]), 2));
        //        QE = (Math.Round(Convert.ToDouble(dt.Rows[0]["QualityEfficiency"]), 2));
        //        NOT = (Math.Round(Convert.ToDouble(dt.Rows[0]["CN"]) / 60, 2));
        //        CN = (Math.Round(Convert.ToDouble(dt.Rows[0]["Components"]), 2));
        //        RejCN = (Math.Round(Convert.ToDouble(dt.Rows[0]["RejCount"]), 2));
        //        double PDT = ChangeToMinutes((dt.Rows[0]["totalPDT"].ToString()));
        //        UT = Convert.ToDouble(dt.Rows[0]["UtilisedTime"]) / 60.0;
        //        ML = ChangeToMinutes((dt.Rows[0]["NetManagementLoss"]).ToString());
        //        DT = ChangeToMinutes((dt.Rows[0]["DownTime"]).ToString());
        //        TT = ChangeToMinutes((dt.Rows[0]["TotalTime"]).ToString());
        //        double val = DT - ML;
        //        lblTTMin.Text = TT.ToString("##.##");
        //        lblPLMin.Text = (TT - (UT + val)).ToString("##.##");
        //        lblPPTMin.Text = (TT - (TT - (UT + val))).ToString("##.##");
        //        lblDTMin.Text = Math.Round(val, 2).ToString();
        //        lblOTMin.Text = Math.Round(Math.Round(Convert.ToDecimal(UT), 2), 2).ToString();
        //        lblSLMin.Text = NOT.ToString();
        //        lblSL.Text = "Σ (Standard Cycle Time * part count)";

        //        //lblNOTMin.Text = Math.Round(Convert.ToDecimal(NOT), 2).ToString();
        //        //lblEOTMin.Text = Math.Round(Convert.ToDecimal(NOT), 2).ToString();
        //        lblAE.Text = "AE =" + lblOTMin.Text + "/" + "(" + lblOTMin.Text + "+" + lblDTMin.Text + ")";
        //        lblPE.Text = "PE =" + lblSLMin.Text + "/" + lblOTMin.Text;
        //        lblDT.Text = "DT =" + lblPPTMin.Text + "-" + lblOTMin.Text;
        //        lblAEMin.Text = AE.ToString();
        //        lblPEMin.Text = PE.ToString();
        //        lblOEEMin.Text = OEE.ToString();

        //        lblQEMin.Text = QE.ToString();
        //        //lblQE.Text = "QE = (" + CN.ToString() + ")/(" + CN.ToString() + "+" + RejCN.ToString() + ")";
        //        lblQE.Text = "QE = (" + CN.ToString() + " - " + RejCN.ToString() + ")/" + CN.ToString();
        //        //lblSL.Text = "SL =" + lblOTMin.Text + "-" + lblNOTMin.Text;
        //        lblPPT.Text = "PPT =" + lblTTMin.Text + "-" + lblPLMin.Text;
        //        lblOEE.Text = "OEE =" + lblAEMin.Text + "*" + lblPEMin.Text + "*" +lblQEMin.Text;
        //        //lblEOT.Text = "EOT =" + lblOTMin.Text + "-" + lblSLMin.Text;
        //        //lblNOT.Text = "NOT =" + lblOTMin.Text + "-" + lblSLMin.Text;
        //        lblPL.Text = "PL =" + lblTTMin.Text + "- (" + lblOTMin.Text + "+" + lblDTMin.Text + ")";
        //        lblTT.Text = "TT=" + "(" + txtToDate.Text + ")" + "-" + "(" + txtFromDate.Text + ")";



        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorSignal.FromCurrentContext().Raise(ex);
        //        lblMessages.Text = ex.Message;
        //    }
        //}



        private bool CheckDateRange()
        {
            bool isDateInRange = false;
            try
            {
                DateTime dt1 = Util.GetDateTime(txtFromDate.Text);
                DateTime dt2 = Util.GetDateTime(txtToDate.Text);

                var hours = (dt2 - dt1).TotalHours;
                if (Math.Abs(hours) <= 48)
                {
                    isDateInRange = true;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
            return isDateInRange;
        }

        private double ChangeToMinutes(string val)
        {
            double timeVal = 0.0;
            try
            {
                string TimeFormatVal = (VDGDataBaseAccess.GetCockpitTimeFormat()).ToLower();
                if (!string.IsNullOrEmpty(TimeFormatVal))
                {
                    if (TimeFormatVal.Equals("ss"))
                    {
                        timeVal = Convert.ToDouble(val) / 60;
                    }
                    else if (TimeFormatVal.Equals("mm"))
                    {
                        timeVal = Convert.ToDouble(val);
                    }
                    else if (TimeFormatVal.Equals("hh"))
                    {
                        timeVal = Convert.ToDouble(val) * 60;
                    }
                    else if (TimeFormatVal.Equals("hh:mm:ss"))
                    {
                        string[] arr = val.Split(':');
                        //timeVal = Math.Round(Convert.ToDouble(arr[0]) * 60 + Convert.ToDouble(arr[1]), 2);
                        timeVal = Math.Round(Convert.ToDouble(arr[0]) * 60 + Convert.ToDouble(arr[1]) + (Convert.ToDouble(arr[2]) / 60), 2);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
            return timeVal;// Math.Round(timeVal, 2);                    
        }

        private static double ChangeToMinutes2(string val)
        {
            double timeVal = 0.0;
            try
            {
                string TimeFormatVal = (VDGDataBaseAccess.GetCockpitTimeFormat()).ToLower();
                if (!string.IsNullOrEmpty(TimeFormatVal))
                {
                    if (TimeFormatVal.Equals("ss"))
                    {
                        timeVal = Convert.ToDouble(val) / 60;
                    }
                    else if (TimeFormatVal.Equals("mm"))
                    {
                        timeVal = Convert.ToDouble(val);
                    }
                    else if (TimeFormatVal.Equals("hh"))
                    {
                        timeVal = Convert.ToDouble(val) * 60;
                    }
                    else if (TimeFormatVal.Equals("hh:mm:ss"))
                    {
                        string[] arr = val.Split(':');
                        timeVal = Math.Round(Convert.ToDouble(arr[0]) * 60 + Convert.ToDouble(arr[1]), 2);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return timeVal;// Math.Round(timeVal, 2);                    
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Chart<SeriesBar> GetOEEChartData(string machineId, string strfromDate, string strtoDate)
        {
            var chart = new Chart<SeriesBar>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = "XAxisTitle",
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };

            try
            {

                DataTable dt = new DataTable();
                DateTime fromDate = DateTime.Now.Date;
                DateTime toDate = DateTime.Now.Date;
                fromDate = Util.GetDateTime(strfromDate.Trim());
                toDate = Util.GetDateTime(strtoDate.Trim());
                //if (!DateTime.TryParseExact(strfromDate.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDate))
                //{
                //    return new Chart<SeriesBar>();
                //}
                //if (!DateTime.TryParseExact(strtoDate.Trim(), new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MMM/yyyy", "dd-MMM-yyyy hh:mm tt" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out toDate))
                //{
                //    return new Chart<SeriesBar>();
                //}

                bool isDateInRange = false;

                DateTime dt1 = Util.GetDateTime(strfromDate);
                DateTime dt2 = Util.GetDateTime(strtoDate);

                var hours = (dt2 - dt1).TotalHours;
                if (Math.Abs(hours) <= 48)
                {
                    isDateInRange = true;
                }



                if (isDateInRange)
                {
                    if (HttpContext.Current.Session["OEEGraph"] == null)
                        dt = VDGDataBaseAccess.GetMachineData("s_GetCockpitData_WithTempTable_eshopx", fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), "", machineId);
                    else
                        dt = HttpContext.Current.Session["OEEGraph"] as DataTable;
                }
                else
                {
                    if (HttpContext.Current.Session["OEEGraph"] == null)
                        dt = VDGDataBaseAccess.GetMachineData("s_GetCockpitData_eshopx", fromDate.ToSQLDateTimeFormat(), toDate.ToSQLDateTimeFormat(), "", machineId);
                    else
                        dt = HttpContext.Current.Session["OEEGraph"] as DataTable;
                }
                chart.series = new List<SeriesBar>();
                chart.categories = new List<string> { "Total Time", "PPT/PL", "OT/DT" };
                //chart.categories = new List<string> { "Total Time", "PPT/PL", "OT/DT", "NOT/SL", "EOT" };


                if (dt != null)
                {

                    double value;
                    string value2;
                    double PE = (Math.Round(Convert.ToDouble(dt.Rows[0]["ProductionEfficiency"]), 2));
                    double AE = (Math.Round(Convert.ToDouble(dt.Rows[0]["AvailabilityEfficiency"]), 2));
                    double OEE = (Math.Round(Convert.ToDouble(dt.Rows[0]["OverAllEfficiency"]), 2));
                    double NOT = (Math.Round(Convert.ToDouble(dt.Rows[0][7]) / 60, 2));
                    double PDT = ChangeToMinutes2((dt.Rows[0]["totalPDT"].ToString()));
                    double UT = (Math.Round(Convert.ToDouble(dt.Rows[0]["UtilisedTime"]) / 60, 2));
                    double ML = ChangeToMinutes2((dt.Rows[0]["NetManagementLoss"]).ToString());
                    double DT = ChangeToMinutes2((dt.Rows[0]["DownTime"]).ToString());
                    double TT = ChangeToMinutes2((dt.Rows[0]["TotalTime"]).ToString());
                    decimal val = (Math.Round(Convert.ToDecimal(DT), 2) - Math.Round(Convert.ToDecimal(ML), 2));

                    if ((dt.Rows[0]["TotalTime"]).ToString() != "")
                    {
                        value = ChangeToMinutes2((dt.Rows[0]["TotalTime"]).ToString());
                        chart.series.Add(new SeriesBar { name = "TotalTime", data = new List<double> { value, 0, 0, 0, 0 }, color = "#6666FF" });
                    }
                    value2 = Math.Round((Convert.ToDecimal(TT) - (Convert.ToDecimal(TT) - (Convert.ToDecimal(UT) + val))), 2).ToString();
                    if (double.TryParse(value2, out value))
                        chart.series.Add(new SeriesBar { name = "PPT", data = new List<double> { 0, value, 0, 0, 0 }, color = "#6666FF" });
                    value2 = Math.Round(Math.Round(Convert.ToDecimal(UT), 2), 2).ToString();
                    //if (double.TryParse(value2, out value))
                    //    chart.series.Add(new SeriesBar { name = "OT", data = new List<double> { 0, 0, value, 0, 0 }, color = "#6666FF" });
                    //value2 = Math.Round(Convert.ToDecimal(NOT), 2).ToString();
                    //if (double.TryParse(value2, out value))
                    //    chart.series.Add(new SeriesBar { name = "NOT", data = new List<double> { 0, 0, 0, value, 0 }, color = "#6666FF" });
                    //value2 = Math.Round(Convert.ToDecimal(NOT), 2).ToString();
                    //if (double.TryParse(value2, out value))
                    //    chart.series.Add(new SeriesBar { name = "EOT", data = new List<double> { 0, 0, 0, 0, value }, color = "#6666FF" });



                    //if ((dt.Rows[0]["TotalTime"]).ToString() != "")
                    //{
                    //    value = ChangeToMinutes2((dt.Rows[0]["TotalTime"]).ToString());
                    //    chart.series.Add(new SeriesBar { name = "", data = new List<double> { value, 0, 0, 0, 0 } });
                    //}
                    //value2 = Math.Round((Convert.ToDecimal(TT) - (Convert.ToDecimal(UT) + val)), 2).ToString();
                    if (double.TryParse(value2, out value))
                        chart.series.Add(new SeriesBar { name = "PL", data = new List<double> { 0, value, 0, 0, 0 }, color = "#AAAAFF" });
                    value = ChangeToMinutes2((dt.Rows[0]["DownTime"]).ToString());
                    //if (double.TryParse(value2, out value))
                    chart.series.Add(new SeriesBar { name = "DT", data = new List<double> { 0, 0, value, 0, 0 }, color = "#AAAAFF" });
                    value2 = Math.Round((Math.Round(Convert.ToDecimal(UT), 2) - Math.Round(Convert.ToDecimal(NOT), 2)), 2).ToString();
                    //if (double.TryParse(value2, out value))
                    //    chart.series.Add(new SeriesBar { name = "SL", data = new List<double> { 0, 0, 0, value, 0 }, color = "#AAAAFF" });
                    //value2 = Math.Round(Convert.ToDecimal(NOT), 2).ToString();
                    //if (double.TryParse(value2, out value))
                    //    chart.series.Add(new SeriesBar { name = "", data = new List<double> { 0, 0, 0, 0, value } });
                }




            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            //var data = new List<DateTime>();
            return chart;
        }
    }
}