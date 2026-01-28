using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class DownTimeActionTakenDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDownReasons();
                BindDataGrid();
            }
        }

        private void BindDownReasons()
        {
            try
            {
                if (Session["MachineId"] != null)
                {
                    List<string> list = DataBaseAccess.GetDownIDMachineWise(Session["MachineID"].ToString());
                    if (list.Count > 0)
                    {
                        ddlDownReasons.DataSource = list;
                        ddlDownReasons.DataBind();

                        if (Session["DownID"] != null)
                        {
                            ddlDownReasons.SelectedValue = Session["DownID"].ToString();
                            lblDownReason.Text = "Down Time Reason: " + ddlDownReasons.SelectedValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindDataGrid()
        {
            List<DownTimeActionTakenEntity> list = new List<DownTimeActionTakenEntity>();
            try
            {
                string machineID = "", Year = "", Month = "", StartDate = "", EndDate = "", MonthClicked = "", Day = "";
                if (Session["MachineId"] != null)
                {
                    machineID = Session["MachineId"].ToString();
                }
                if (Session["Year"] != null)
                {

                    Year = Session["Year"].ToString();
                    Month = Session["Month"].ToString();
                    if (Month != "")
                    {

                        StartDate = Convert.ToDateTime(DateTime.ParseExact("01-" + Month + "-" + Year, "dd-MM-yyyy", null)).ToString("yyyy-MM-dd HH:mm:ss");
                        var Date = DateTime.DaysInMonth(Convert.ToInt32(Year), Convert.ToInt32(Month));
                        EndDate = Convert.ToDateTime(DateTime.ParseExact(Date.ToString() + "-" + Month + "-" + Year, "dd-MM-yyyy", null)).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        if (Session["MonthClicked"] != null)
                            MonthClicked = Session["MonthClicked"].ToString();
                        if (MonthClicked != "")
                        {
                            Day = Session["Day"].ToString();
                            if (Day != "")
                            {
                                StartDate = Convert.ToDateTime(DateTime.ParseExact(Day + MonthClicked + "-" + Year, "dd-MMM-yyyy", null)).ToString("yyyy-MM-dd HH:mm:ss");
                                EndDate = Convert.ToDateTime(DateTime.ParseExact((Convert.ToInt32(Day) + 1).ToString() + "-" + MonthClicked + "-" + Year, "dd-MMM-yyyy", null)).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else
                            {
                                StartDate = Convert.ToDateTime(DateTime.ParseExact("01-" + MonthClicked + "-" + Year, "dd-MMM-yyyy", null)).ToString("yyyy-MM-dd HH:mm:ss");

                                var Date = DateTime.DaysInMonth(Convert.ToInt32(Year), Convert.ToInt32(HelperClassGeneric.getMonthNumberFromAbbMonthName(MonthClicked)));
                                EndDate = Convert.ToDateTime(DateTime.ParseExact(Date.ToString() + "-" + MonthClicked + "-" + Year, "dd-MMM-yyyy", null)).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }
                        else
                        {
                            StartDate = Convert.ToDateTime(DateTime.ParseExact("01-01-" + Year, "dd-MM-yyyy", null)).ToString("yyyy-MM-dd HH:mm:ss");
                            var dat = "31-12-" + Year;
                            EndDate = Convert.ToDateTime(DateTime.ParseExact(dat, "dd-MM-yyyy", null)).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                }

                lblDownReason.Text = "Down Time Reason: " + ddlDownReasons.SelectedValue.Trim();
                lblStartDate.Text = "From Date: " + Convert.ToDateTime(StartDate).ToString("dd-MM-yyyy");
                lblEndDate.Text = "To Date: " + Convert.ToDateTime(EndDate).ToString("dd-MM-yyyy");
                list = DataBaseAccess.GetDownTimeActionTakenScreenData(ddlDownReasons.SelectedValue.Trim(), machineID, StartDate, EndDate);
                gvDownTimeActionTaken.DataSource = list;
                gvDownTimeActionTaken.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void ddlDownReasons_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDataGrid();
        }
    }
}