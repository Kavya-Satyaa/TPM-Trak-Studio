using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Bajaj
{
    public partial class JHReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtYear.Text = DateTime.Now.ToString("yyyy");
                txtMonth.Text = DateTime.Now.ToString("MM");
                BindPlant();
                BindJHDetails();
                HelperClass.openFunction(this, "setActiveSubmenuValue");
            }

        }
        private void BindPlant()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlantsForPlantInfo();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();

                BindCell();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPlant: " + ex.Message);
            }
        }
        private void BindCell()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllGroupId(ddlPlant.SelectedValue);
                ddlCell.DataSource = list;
                ddlCell.DataBind();

                BindMachine();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindCell: " + ex.Message);
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllMachinedByLineandGroup(ddlPlant.SelectedValue, ddlCell.SelectedValue);
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();


                // BindMachineRevisionNo();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachine: " + ex.Message);
            }
        }
        //private void BindMachineRevisionNo()
        //{
        //    try
        //    {
        //        //List<ListItem> list = BajajDBAccess.GetRevisionDetailsForMachine(ddlMachine.SelectedValue);
        //        //ddlRevNo.DataSource = list;
        //        //ddlRevNo.DataTextField = "Text";
        //        //ddlRevNo.DataValueField = "Value";
        //        //ddlRevNo.DataBind();

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog("BindMachineRevisionNo: " + ex.Message);
        //    }
        //}
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindCell();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlPlant_SelectedIndexChanged: " + ex.Message);
            }
        }

        protected void ddlCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindMachine();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlCell_SelectedIndexChanged: " + ex.Message);
            }
        }

        //protected void ddlMachine_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        BindMachineRevisionNo();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog("ddlMachine_SelectedIndexChanged: " + ex.Message);
        //    }
        //}
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindJHDetails();
        }
        private void BindJHDetails()
        {
            try
            {
                ViewState["Plant"] = ddlPlant.SelectedValue;
                ViewState["Cell"] = ddlCell.SelectedValue;
                ViewState["Machine"] = ddlMachine.SelectedValue;
                ViewState["Year"] = txtYear.Text;
                ViewState["Month"] = txtMonth.Text;
                DataTable dtDailyDetails = new DataTable();
                DataTable dtWeeklyDetails = new DataTable();
                DataTable dtQuaterlyDetails = new DataTable();
                dtDailyDetails = BajajDBAccess.GetJHReportDetails(ViewState["Machine"].ToString(), ViewState["Year"].ToString(), ViewState["Month"].ToString(), out dtWeeklyDetails, out dtQuaterlyDetails);

                var revLevelDetails = (dynamic)null;
                if (dtDailyDetails.Rows.Count > 0)
                {
                    revLevelDetails = dtDailyDetails.AsEnumerable().Select(x => new { manager = x.Field<string>("Manager"), grpleader = x.Field<string>("GroupLeader"), revno = x.Field<string>("RevNo"), revdate = x.Field<DateTime>("RevDate").ToString("dd-MM-yyyy") }).FirstOrDefault();
                }
                else if (dtWeeklyDetails.Rows.Count > 0)
                {
                    revLevelDetails = dtWeeklyDetails.AsEnumerable().Select(x => new { manager = x.Field<string>("Manager"), grpleader = x.Field<string>("GroupLeader"), revno = x.Field<string>("RevNo"), revdate = x.Field<DateTime>("RevDate").ToString("dd-MM-yyyy") }).FirstOrDefault();
                }
                else if (dtQuaterlyDetails.Rows.Count > 0)
                {
                    revLevelDetails = dtQuaterlyDetails.AsEnumerable().Select(x => new { manager = x.Field<string>("Manager"), grpleader = x.Field<string>("GroupLeader"), revno = x.Field<string>("RevNo"), revdate = x.Field<DateTime>("RevDate").ToString("dd-MM-yyyy") }).FirstOrDefault();
                }
                if (revLevelDetails != null)
                {
                    ViewState["Manager"] = revLevelDetails.manager;
                    ViewState["GroupLeader"] = revLevelDetails.grpleader;
                    ViewState["RevNo"] = revLevelDetails.revno;
                    ViewState["RevDate"] = revLevelDetails.revdate;

                    lblManager.Text = ViewState["Manager"].ToString();
                    lblGrpLeader.Text = ViewState["GroupLeader"].ToString();
                    lblRevNo.Text = ViewState["RevNo"].ToString();
                    lblRevDate.Text = ViewState["RevDate"].ToString();
                }



                var distinctCheckPoints = dtDailyDetails.AsEnumerable().Select(x => x.Field<string>("Checkpoint")).Distinct();
                var distinctDetails = dtDailyDetails.AsEnumerable().Select(x => new { date = x.Field<DateTime>("Date").ToString("dd") }).Distinct();

                int width = 110;
                List<JHReportDetails> dlist = new List<JHReportDetails>();
                JHReportDetails dataContentDetails = null;
                JHReportDetails dataHeaderDetails = new JHReportDetails();
                int i = 0;
                foreach (string chk in distinctCheckPoints)
                {
                    if (i == 0)
                    {
                        dataHeaderDetails.CheckPointsHeaderVisibility = "table";
                        dataHeaderDetails.CheckPointsContentVisibility = "none";
                    }

                    var checkpointdetails = dtDailyDetails.AsEnumerable().Where(x => x.Field<string>("Checkpoint") == chk).Select(x => new { checkpointno = x.Field<int>("CheckpointNo"), routeno = x.Field<int>("RouteNo"), clirtvalue = x.Field<string>("C_L_I_R_T"), item = x.Field<string>("Item"), standard = x.Field<string>("Standard"), ifnotol = x.Field<string>("If_NotOK"), method = x.Field<string>("Method"), frequency = x.Field<string>("Frequency"), day = x.Field<string>("Day") }).FirstOrDefault();

                    dataContentDetails = new JHReportDetails();
                    dataContentDetails.CheckPointNo = checkpointdetails.checkpointno.ToString();
                    dataContentDetails.CheckPoint = chk; ;
                    dataContentDetails.RouteNo = checkpointdetails.routeno.ToString();
                    dataContentDetails.C_L_I_RT_Value = checkpointdetails.clirtvalue;
                    dataContentDetails.Item = checkpointdetails.item;
                    dataContentDetails.Standard = checkpointdetails.standard;
                    dataContentDetails.IfNotOk = checkpointdetails.ifnotol;
                    dataContentDetails.Method = checkpointdetails.method;
                    dataContentDetails.Frequency = checkpointdetails.frequency;
                    dataContentDetails.DayToDisplay = checkpointdetails.day;
                    dataContentDetails.CheckPointsContentVisibility = "table";

                    List<JHFrequencyDetails> frequencyHeaderList = new List<JHFrequencyDetails>();
                    List<JHFrequencyDetails> frequencyContentList = new List<JHFrequencyDetails>();

                    foreach (var distinctItem in distinctDetails)
                    {
                        if (i == 0)
                        {
                            frequencyHeaderList.Add(new JHFrequencyDetails() { Value = distinctItem.date, ValueHeaderVisisbility = "table-row", Width = width + "px" });
                        }

                        var freqdetails = dtDailyDetails.AsEnumerable().Where(x => x.Field<string>("Checkpoint") == chk && x.Field<DateTime>("Date").ToString("dd") == distinctItem.date).Select(x => new { value = x.Field<string>("Value") }).FirstOrDefault();

                        JHFrequencyDetails freqContentDetails = new JHFrequencyDetails();
                        if (string.IsNullOrEmpty(freqdetails.value))
                        {
                            freqContentDetails.Value = ".";
                            freqContentDetails.ValueContentColor = "transparent";
                        }
                        else
                        {
                            freqContentDetails.Value = freqdetails.value;
                            freqContentDetails.ValueContentColor = "unset";
                        }
                        freqContentDetails.Width = width + "px";
                        freqContentDetails.ValueContentVisisbility = "table-row";
                        frequencyContentList.Add(freqContentDetails);

                    }
                    if (i == 0)
                    {
                        dataHeaderDetails.FrequencyDetails = frequencyHeaderList;
                        dlist.Add(dataHeaderDetails);
                        i++;
                    }
                    dataContentDetails.FrequencyDetails = frequencyContentList;
                    dlist.Add(dataContentDetails);

                }
                Session["JHDailyDetails"] = dlist;


                List<JHReportDetails> wlist = new List<JHReportDetails>();
                distinctCheckPoints = dtWeeklyDetails.AsEnumerable().Select(x => x.Field<string>("Checkpoint")).Distinct();
                var distinctWeekDetails = dtWeeklyDetails.AsEnumerable().Select(x => new { week = x.Field<int>("WeekNo") }).Distinct();
                foreach (string chk in distinctCheckPoints)
                {

                    var checkpointdetails = dtWeeklyDetails.AsEnumerable().Where(x => x.Field<string>("Checkpoint") == chk).Select(x => new { checkpointno = x.Field<int>("CheckpointNo"), routeno = x.Field<int>("RouteNo"), clirtvalue = x.Field<string>("C_L_I_R_T"), item = x.Field<string>("Item"), standard = x.Field<string>("Standard"), ifnotol = x.Field<string>("If_NotOK"), method = x.Field<string>("Method"), frequency = x.Field<string>("Frequency"), day = x.Field<string>("Day") }).FirstOrDefault();

                    dataContentDetails = new JHReportDetails();
                    dataContentDetails.CheckPointNo = checkpointdetails.checkpointno.ToString();
                    dataContentDetails.CheckPoint = chk; ;
                    dataContentDetails.RouteNo = checkpointdetails.routeno.ToString();
                    dataContentDetails.C_L_I_RT_Value = checkpointdetails.clirtvalue;
                    dataContentDetails.Item = checkpointdetails.item;
                    dataContentDetails.Standard = checkpointdetails.standard;
                    dataContentDetails.IfNotOk = checkpointdetails.ifnotol;
                    dataContentDetails.Method = checkpointdetails.method;
                    dataContentDetails.Frequency = checkpointdetails.frequency;
                    dataContentDetails.DayToDisplay = checkpointdetails.day;
                    dataContentDetails.CheckPointsContentVisibility = "table";


                    List<JHFrequencyDetails> frequencyContentList = new List<JHFrequencyDetails>();
                    foreach (var distinctItem in distinctWeekDetails)
                    {
                        var freqdetails = dtWeeklyDetails.AsEnumerable().Where(x => x.Field<string>("Checkpoint") == chk && x.Field<int>("WeekNo") == distinctItem.week).Select(x => new { value = x.Field<string>("Value"), weekstart = x.Field<DateTime>("WeekStart").ToString("dd-MM-yyyy"), weekend = x.Field<DateTime>("WeekEnd").ToString("dd-MM-yyyy") }).FirstOrDefault();

                        JHFrequencyDetails freqContentDetails = new JHFrequencyDetails();
                        if (string.IsNullOrEmpty(freqdetails.value))
                        {
                            freqContentDetails.Value = ".";
                            freqContentDetails.ValueContentColor = "transparent";
                        }
                        else
                        {
                            freqContentDetails.Value = freqdetails.value;
                            freqContentDetails.ValueContentColor = "unset";
                        }
                        if (distinctItem.week == distinctWeekDetails.Select(x => x.week).LastOrDefault())
                        {
                            double dateDiffInDays = ((Util.GetDateTime(freqdetails.weekend) - Util.GetDateTime(freqdetails.weekstart)).TotalDays + 1);
                            freqContentDetails.Width = (width * (dateDiffInDays)) + "px";
                            freqContentDetails.MergeColumnNo = Convert.ToInt32(dateDiffInDays);
                        }
                        else
                        {
                            freqContentDetails.Width = (width * 7) + "px";
                            freqContentDetails.MergeColumnNo = 7;
                        }

                        freqContentDetails.ValueContentVisisbility = "table-row";
                        frequencyContentList.Add(freqContentDetails);

                    }
                    dataContentDetails.FrequencyDetails = frequencyContentList;
                    wlist.Add(dataContentDetails);

                }
                Session["JHWeeklyDetails"] = wlist;

                List<JHReportDetails> list = new List<JHReportDetails>();
                list.AddRange(dlist);
                list.AddRange(wlist);

                lvDandWReportDetails.DataSource = list;
                lvDandWReportDetails.DataBind();



                distinctCheckPoints = dtQuaterlyDetails.AsEnumerable().Select(x => x.Field<string>("Checkpoint")).Distinct();
                var distinctMonthDetails = dtQuaterlyDetails.AsEnumerable().Select(x => new { month = x.Field<int>("QuarterVal").ToString() }).Distinct();

                list = new List<JHReportDetails>();
                dataContentDetails = null;
                dataHeaderDetails = new JHReportDetails();
                i = 0;
                foreach (string chk in distinctCheckPoints)
                {
                    if (i == 0)
                    {
                        dataHeaderDetails.CheckPointsHeaderVisibility = "table";
                        dataHeaderDetails.CheckPointsContentVisibility = "none";
                    }

                    var checkpointdetails = dtQuaterlyDetails.AsEnumerable().Where(x => x.Field<string>("Checkpoint") == chk).Select(x => new { checkpointno = x.Field<int>("CheckpointNo"), routeno = x.Field<int>("RouteNo"), clirtvalue = x.Field<string>("C_L_I_R_T"), item = x.Field<string>("Item"), standard = x.Field<string>("Standard"), ifnotol = x.Field<string>("If_NotOK"), method = x.Field<string>("Method"), frequency = x.Field<string>("Frequency"), day = x.Field<string>("Day"), dayno = x.Field<int>("DayNo"), month = x.Field<int>("MasterMonthNo") }).FirstOrDefault();

                    dataContentDetails = new JHReportDetails();
                    dataContentDetails.CheckPointNo = checkpointdetails.checkpointno.ToString();
                    dataContentDetails.CheckPoint = chk; ;
                    dataContentDetails.RouteNo = checkpointdetails.routeno.ToString();
                    dataContentDetails.C_L_I_RT_Value = checkpointdetails.clirtvalue;
                    dataContentDetails.Item = checkpointdetails.item;
                    dataContentDetails.Standard = checkpointdetails.standard;
                    dataContentDetails.IfNotOk = checkpointdetails.ifnotol;
                    dataContentDetails.Method = checkpointdetails.method;
                    dataContentDetails.Frequency = checkpointdetails.frequency;
                    DateTimeFormatInfo mfi = new DateTimeFormatInfo();
                    string strMonthName = mfi.GetMonthName(Convert.ToInt32(checkpointdetails.month)).ToString();
                    string daytodisplay = checkpointdetails.dayno + (string.IsNullOrEmpty(checkpointdetails.dayno.ToString()) ? "" : checkpointdetails.dayno.ToString() == "1" ? "st" : checkpointdetails.dayno.ToString() == "2" ? "nd" : checkpointdetails.dayno.ToString() == "3" ? "rd" : "th") + " " + checkpointdetails.day + " of " + strMonthName;
                    dataContentDetails.DayToDisplay = daytodisplay;
                    dataContentDetails.CheckPointsContentVisibility = "table";

                    List<JHFrequencyDetails> frequencyHeaderList = new List<JHFrequencyDetails>();
                    List<JHFrequencyDetails> frequencyContentList = new List<JHFrequencyDetails>();

                    if (i == 0)
                    {
                        string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                        for (int mon = 0; mon < months.Length; mon++)
                        {
                            frequencyHeaderList.Add(new JHFrequencyDetails() { Value = months[mon], Width = width + "px", ValueHeaderVisisbility = "table-row" });
                        }
                    }

                    foreach (var distinctItem in distinctMonthDetails)
                    {
                        //if (i == 0)
                        //{
                        //    string[] months = {"Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec" };
                        //    for(int mon=0; mon < months.Length; mon++)
                        //    {
                        //        frequencyHeaderList.Add(new JHFrequencyDetails() { Value = months[mon], Width = width + "px", ValueHeaderVisisbility = "table-row" });
                        //    }
                        //}

                        var freqdetails = dtQuaterlyDetails.AsEnumerable().Where(x => x.Field<string>("Checkpoint") == chk && x.Field<int>("QuarterVal").ToString() == distinctItem.month).Select(x => new { value = x.Field<string>("Value") }).FirstOrDefault();

                        JHFrequencyDetails freqContentDetails = new JHFrequencyDetails();
                        if (string.IsNullOrEmpty(freqdetails.value))
                        {
                            freqContentDetails.Value = ".";
                            freqContentDetails.ValueContentColor = "transparent";
                        }
                        else
                        {
                            freqContentDetails.Value = freqdetails.value;
                            freqContentDetails.ValueContentColor = "unset";
                        }
                        freqContentDetails.Width = (width * 3) + "px";
                        freqContentDetails.MergeColumnNo = 3;
                        freqContentDetails.ValueContentVisisbility = "table-row";
                        frequencyContentList.Add(freqContentDetails);

                    }
                    if (i == 0)
                    {
                        dataHeaderDetails.FrequencyDetails = frequencyHeaderList;
                        list.Add(dataHeaderDetails);
                        i++;
                    }
                    dataContentDetails.FrequencyDetails = frequencyContentList;
                    list.Add(dataContentDetails);

                }

                Session["JHQuaterlyDetails"] = list;
                lvQReportDetails.DataSource = list;
                lvQReportDetails.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindJHReportDetails: " + ex.Message);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                bool result = GenerateReports.GenerateJHReport(ViewState["Plant"].ToString(), ViewState["Cell"].ToString(), ViewState["Machine"].ToString(), ViewState["Year"].ToString(), ViewState["Month"].ToString(), ViewState["RevNo"].ToString(), ViewState["RevDate"].ToString(), ViewState["Manager"].ToString(), ViewState["GroupLeader"].ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnExport_Click: " + ex.Message);
            }
        }
    }
}