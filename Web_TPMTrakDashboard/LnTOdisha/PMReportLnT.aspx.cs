using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.LnTOdisha.Model;
using System.Data;
using static Web_TPMTrakDashboard.LnTOdisha.Model.LnTOdishaDTO;

namespace Web_TPMTrakDashboard.LnTOdisha
{
    public partial class PMReportLnT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachine();
                txtYear.Text = DateTime.Now.ToString("yyyy");
                BindData();
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = LnTOdishaDBAccess.GetMachineInfoForPM();
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindData()
        {
            //try
            //{
            //    List<PMReportLnTEntity> list = new List<PMReportLnTEntity>();
            //    //DataTable dt = LnTOdishaDBAccess.gePMReportDetails(ddlMachine.SelectedValue, txtYear.Text, txtMonth.Text);
            //    if (dt.Rows.Count > 0)
            //    {
            //        var distActivity = dt.AsEnumerable().Select(k => k["Activity"].ToString()).Distinct().ToList();
            //        var distMonth = dt.AsEnumerable().Select(k => k["Month"].ToString()).Distinct().ToList();
            //        var firstRow = dt.Rows[0];
            //        PMReportLnTEntity header1 = new PMReportLnTEntity();
            //        PMReportLnTEntity header2 = new PMReportLnTEntity();
            //        PMReportLnTEntity header3 = new PMReportLnTEntity();
            //        PMReportLnTEntity header4 = new PMReportLnTEntity();
            //        PMReportLnTEntity header5 = new PMReportLnTEntity();
            //        header1.Activity = "ISO Doc. No.:PP/PMD/PM/1";
            //        header1.AllotedTime = "Alloted time for executing";
            //        header1.RowSpan = "5";
            //        header1.Frequency = "Frequency";
            //        header1.LastChecked = "LAST CHECKED";
            //        header1.TodayPlan = "Today's PM";

            //        header2.Activity = "Revision-1 Dated 28/05/2013";
            //        header2.RowSpan = "0";
            //        header2.Frequency = "";
            //        header2.LastChecked = firstRow["LastDate"].ToString();

            //        header3.Activity = "TEAM LEADER";
            //        header3.RowSpan = "0";
            //        header3.Frequency = "";
            //        header2.LastChecked = firstRow["LastTL"].ToString();

            //        header4.Activity = "CRAFTS:";
            //        header4.RowSpan = "0";
            //        header4.Frequency = "";
            //        header2.LastChecked = firstRow["LastCraft"].ToString();

            //        header4.Activity = "ACTIVITY:";
            //        header4.RowSpan = "0";
            //        header4.Frequency = "";

            //        List<PMReportMonthLnTEntity> monthHeaderData1List = new List<PMReportMonthLnTEntity>();
            //        List<PMReportMonthLnTEntity> monthHeaderData2List = new List<PMReportMonthLnTEntity>();
            //        List<PMReportMonthLnTEntity> monthHeaderData3List = new List<PMReportMonthLnTEntity>();
            //        List<PMReportMonthLnTEntity> monthHeaderData4List = new List<PMReportMonthLnTEntity>();
            //        List<PMReportMonthLnTEntity> monthHeaderData5List = new List<PMReportMonthLnTEntity>();
            //        foreach (string month in distMonth)
            //        {
            //            var row = dt.AsEnumerable().Where(k => k["Month"].ToString() == month).FirstOrDefault();
            //            PMReportMonthLnTEntity headerMonthData = new PMReportMonthLnTEntity();
            //            headerMonthData.MonthValue = month;
            //            monthHeaderData1List.Add(headerMonthData);

            //            headerMonthData = new PMReportMonthLnTEntity();
            //            headerMonthData.MonthValue = row["PlanDate"].ToString();
            //            monthHeaderData2List.Add(headerMonthData);

            //            headerMonthData = new PMReportMonthLnTEntity();
            //            headerMonthData.MonthValue = row["TL"].ToString();
            //            monthHeaderData3List.Add(headerMonthData);

            //            headerMonthData = new PMReportMonthLnTEntity();
            //            headerMonthData.MonthValue = row["Craft"].ToString();
            //            monthHeaderData4List.Add(headerMonthData);

            //            headerMonthData = new PMReportMonthLnTEntity();
            //            headerMonthData.MonthValue = "";
            //            monthHeaderData5List.Add(headerMonthData);
            //        }
            //        header1.MonthData = monthHeaderData1List;
            //        header2.MonthData = monthHeaderData2List;
            //        header3.MonthData = monthHeaderData3List;
            //        header4.MonthData = monthHeaderData4List;
            //        header5.MonthData = monthHeaderData5List;


            //        list.Add(header1);
            //        list.Add(header2);
            //        list.Add(header3);
            //        list.Add(header4);
            //        list.Add(header5);

            //        var distCategory = dt.AsEnumerable().Select(k => k["Category"].ToString()).Distinct().ToList();
            //        string tempCategory = "";
            //        foreach (string activity in distActivity)
            //        {
            //            PMReportLnTEntity data = new PMReportLnTEntity();
            //            var row = dt.AsEnumerable().Where(k => k["Activity"].ToString() == activity).FirstOrDefault();

            //            if (tempCategory != row["Catgeory"].ToString())
            //            {
            //                data = new PMReportLnTEntity();
            //                data.Activity = row["Catgeory"].ToString();
            //                data.CategoryEnabled = true;
            //                list.Add(data);
            //            }
            //            tempCategory = row["Catgeory"].ToString();

            //            data = new PMReportLnTEntity();
            //            data.Activity = row["Activity"].ToString();
            //            data.AllotedTime = row["AllotedTime"].ToString();
            //            data.Frequency = row["Frequnecy"].ToString();
            //            data.LastChecked = row["LastCheckedValue"].ToString();
            //            data.TodayPlan = row["TodayValue"].ToString();
            //            List<PMReportMonthLnTEntity> monthDataList = new List<PMReportMonthLnTEntity>();
            //            foreach (string month in distMonth)
            //            {
            //                var monthRow = dt.AsEnumerable().Where(k => k["Activity"].ToString() == activity && k["Month"].ToString() == month).FirstOrDefault();
            //                PMReportMonthLnTEntity monthData = new PMReportMonthLnTEntity();
            //                monthData.MonthValue = monthRow["ActualValue"].ToString();
            //                monthDataList.Add(monthData);

            //            }
            //            data.MonthData = monthDataList;
            //            list.Add(data);
            //        }
            //    }
            //    lvChecklist.DataSource = list;
            //    lvChecklist.DataBind();
            //    Session["PMReportData"] = list;
            //}
            //catch (Exception ex)
            //{
            //    Logger.WriteErrorLog(ex);
            //}
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string ReportStatus = "";
            try
            {
                if (HttpContext.Current.Session["PMReportData"] == null)
                {
                    return;
                }
                List<PMReportLnTEntity> list = HttpContext.Current.Session["PMReportData"] as List<PMReportLnTEntity>;
                ReportStatus = LnTGenerateReport.GeneratePMReport(ddlMachine.SelectedValue, txtYear.Text,"","");
                if (ReportStatus.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openSuccessModal(this, "Report Generated");
                else if (ReportStatus.Equals("NoData", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openWarningToastrModal(this, "No data found");
                else
                    HelperClassGeneric.openWarningToastrModal(this, "Try again");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}