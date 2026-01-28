using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessClassLibrary;
using ModelClassLibrary;

namespace Web_TPMTrakDashboard
{
    public partial class ReworkPage : System.Web.UI.Page
    {
        public string chartType = string.Empty;
        static string strShift, plantId, strMonth, componentId, employeeId, strYear, strDay, CellID, machineId, view = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                if (Request.QueryString["machineId"] != null)
                {
                    Session["MachineId"] = Request.QueryString["machineId"].ToString();
                    Session["ShiftId"] = Request.QueryString["shiftId"].ToString();
                    Session["Year"] = Request.QueryString["year"].ToString();
                    Session["Month"] = Request.QueryString["month"].ToString();
                    Session["Day"] = Request.QueryString["day"].ToString();
                    Session["componentId"] = Request.QueryString["componentId"].ToString();
                    Session["employeeId"] = Request.QueryString["employeeId"].ToString();
                    Session["plantId"] = Request.QueryString["plantId"].ToString();
                    Session["CellID"] = Request.QueryString["CellID"].ToString();
                    Session["chartType"] = Request.QueryString["chartType"].ToString();
                    Session["View"] = Request.QueryString["View"].ToString();
                    chartType = Session["chartType"].ToString() == "column" ? "1" : "0";
                }
            }
            strShift = HttpContext.Current.Session["ShiftId"].ToString();
            plantId = HttpContext.Current.Session["plantId"].ToString();
            strMonth = HttpContext.Current.Session["Month"].ToString();
            componentId = HttpContext.Current.Session["componentId"].ToString();
            employeeId = HttpContext.Current.Session["employeeId"].ToString();
            strYear = HttpContext.Current.Session["Year"].ToString();
            strDay = HttpContext.Current.Session["Day"].ToString();
            CellID = HttpContext.Current.Session["CellID"].ToString();
            machineId = HttpContext.Current.Session["MachineId"].ToString();
            view = HttpContext.Current.Session["View"].ToString();

            if (view.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
            {
                machineId = HttpContext.Current.Session["MachineId"].ToString();
            }
            else if (view.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
            {
                plantId = machineId;
                machineId = "";
            }
            else if (view.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
            {
                componentId = machineId;
                machineId = "";
            }
            else if (view.Equals("CellWiseView", StringComparison.OrdinalIgnoreCase))
            {
                CellID = machineId;
                machineId = "";
            }
            else if (view.Equals("OperatorwiseView", StringComparison.OrdinalIgnoreCase))
            {
                employeeId = machineId;
                machineId = "";
            }
        }

        #region "Bind Category Chart Data"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Chart<Series> GetRejectedCategoryChart(string param, string type)
        {
            string date = string.Empty;
            var chart = new Chart<Series>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = HttpContext.GetLocalResourceObject("~/ReworkPage.aspx", "ReworkbyReworkCategory").ToString(),
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };
            if (type == "")
            {
                type = HttpContext.Current.Session["chartType"].ToString();
            }

            DataTable dtYear = new DataTable();
            DataTable dtMonth = new DataTable();
            DataTable dtDay = new DataTable();
            DataTable dtShift = new DataTable();
            List<string> categoryData = new List<string>();
            List<string> monthData = new List<string>();
            string[] monthName = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };

            if (strShift.Equals("All", StringComparison.OrdinalIgnoreCase))
                strShift = "";

            if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
                plantId = "";

            if (componentId.Equals("All", StringComparison.OrdinalIgnoreCase))
                componentId = "";
            if (employeeId.Equals("All", StringComparison.OrdinalIgnoreCase))
                employeeId = "";

            #region "Month Condition------------"
            if (strMonth.Equals("Jan", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "01";
            }
            else if (strMonth.Equals("Feb", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "02";
            }
            else if (strMonth.Equals("Mar", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "03";
            }
            else if (strMonth.Equals("Apr", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "04";
            }
            else if (strMonth.Equals("May", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "05";
            }
            else if (strMonth.Equals("Jun", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "06";
            }
            else if (strMonth.Equals("Jul", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "07";
            }
            else if (strMonth.Equals("Aug", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "08";
            }
            else if (strMonth.Equals("Sep", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "09";
            }
            else if (strMonth.Equals("Oct", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "10";
            }
            else if (strMonth.Equals("Nov", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "11";
            }
            else if (strMonth.Equals("Dec", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "12";
            }
            #endregion
            try
            {
                bool colorByPoint = false;
                colorByPoint = type == "column" ? true : false;
                chart.series = new List<Series>();
                chart.series.Add(new Series
                {
                    name = machineId,
                    type = type,
                    colorByPoint = colorByPoint,
                    data = new List<Data>()
                });
                if (strYear != "" && strMonth != "" && strDay != "")
                {
                    #region "Day Wise Information-------------"
                    date = strYear + "-" + strMonth + "-" + strDay;
                    dtMonth = BindCockpitView.BindRejRewDowntimeInfo(date, strShift, plantId, machineId, "currentDAY", param, componentId, CellID, employeeId, view);

                    foreach (DataRow item in dtMonth.Rows)
                    {
                        categoryData.Add(item["Catagory"].ToString());
                        chart.series[0].data.Add(new Data
                        {
                            name = item["Catagory"].ToString(),
                            y = Convert.ToDecimal(item["TotalQty"].ToString()),
                            drilldown = item["Catagory"].ToString() + "-" + Convert.ToDateTime(item["PDate"]).ToString("MM") + "-" + strDay,
                            afterTitel = item["Catagory"].ToString() + " (" + strYear + "-" + strMonth + "-" + strDay + ")",
                            beforeTitle = ""
                        });
                    }
                    //-------------------------------Shift Wise Info-----------------------------------
                    chart.drilldown = new List<DrildownSeries>();
                    int i = 0;
                    dtShift = BindCockpitView.BindRejRewDowntimeInfo(date, strShift, plantId, machineId, "SHIFT", param, componentId, CellID, employeeId, view);
                    foreach (var catagory in categoryData)
                    {
                        chart.drilldown.Add(new DrildownSeries
                        {
                            name = catagory,
                            type = type,
                            id = catagory + "-" + strMonth + "-" + strDay,
                            data = new List<DrildownData>(),
                        });
                        DataRow[] results = dtShift.Select("Month = " + strMonth + " AND Catagory ='" + catagory + "' AND Day ='" + strDay + "'");
                        foreach (DataRow drshift in results)
                        {
                            chart.drilldown[i].data.Add(new DrildownData
                            {
                                name = drshift["Shift"].ToString(),
                                y = Convert.ToDecimal(drshift["TotalQty"].ToString()),
                                afterTitel = drshift["Catagory"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["Shift"].ToString() + ")",
                                beforeTitle = "Rework by Rework Category" + " : " + drshift["Catagory"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")"
                            }); //, drilldown = drshift["Catagory"].ToString() + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString()
                        }
                        i++;
                    }
                    #endregion
                }
                else if (strYear != "" && strMonth != "")
                {
                    #region "month Wise Information ------------"
                    date = strYear + "-" + strMonth + "-" + "01";
                    dtMonth = BindCockpitView.BindRejRewDowntimeInfo(date, strShift, plantId, machineId, "currentMONTH", param, componentId, CellID, employeeId, view);
                    foreach (DataRow item in dtMonth.Rows)
                    {
                        categoryData.Add(item["Catagory"].ToString());
                        chart.series[0].data.Add(new Data
                        {
                            name = item["Catagory"].ToString(),
                            y = Convert.ToDecimal(item["TotalQty"].ToString()),
                            drilldown = item["Catagory"].ToString(),
                            afterTitel = item["Catagory"].ToString() + " (" + strYear + "-" + strMonth + ")",
                            beforeTitle = ""
                        });
                    }

                    //---------------------------Day Wise Start---------------------------------
                    chart.drilldown = new List<DrildownSeries>();
                    int i = 0;
                    dtDay = BindCockpitView.BindRejRewDowntimeInfo(date, strShift, plantId, machineId, "DAY", param, componentId, CellID, employeeId, view);
                    foreach (var catagory in categoryData)
                    {
                        chart.drilldown.Add(new DrildownSeries
                        {
                            name = catagory,
                            type = type,
                            id = catagory,
                            data = new List<DrildownData>(),
                        });
                        DataRow[] results = dtDay.Select("Month = " + strMonth + " AND Catagory ='" + catagory + "'");
                        foreach (DataRow dr in results)
                        {
                            chart.drilldown[i].data.Add(new DrildownData
                            {
                                name = "Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                                y = Convert.ToDecimal(dr["TotalQty"].ToString()),
                                drilldown = dr["Catagory"].ToString() + "/" + strMonth + "/" + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                                afterTitel = dr["Catagory"].ToString() + " (" + strYear + "-" + strMonth + "-" + Convert.ToDateTime(dr["PDate"]).ToString("dd") + ")",
                                beforeTitle = "Rework by Rework Category" + " : " + dr["Catagory"].ToString() + " (" + strYear + "-" + strMonth + ")"
                            });
                            monthData.Add(Convert.ToDateTime(dr["PDate"]).ToString("dd"));
                        }
                        i++;
                    }
                    //--------------------------Shift Wise Information-----------------------------
                    monthData = (from w in monthData select w).Distinct().ToList();
                    dtShift = BindCockpitView.BindRejRewDowntimeInfo(date, strShift, plantId, machineId, "SHIFT", param, componentId, CellID, employeeId, view);
                    foreach (var catagory in categoryData)
                    {
                        foreach (var day in monthData)
                        {
                            chart.drilldown.Add(new DrildownSeries
                            {
                                name = catagory,
                                type = type,
                                id = catagory + "/" + strMonth + "/" + day,
                                data = new List<DrildownData>(),
                            });
                            DataRow[] results = dtShift.Select("Month = " + strMonth + " AND Catagory ='" + catagory + "' AND Day ='" + day + "'");
                            foreach (DataRow drshift in results)
                            {
                                chart.drilldown[i].data.Add(new DrildownData
                                {
                                    name = drshift["Shift"].ToString(),
                                    y = Convert.ToDecimal(drshift["TotalQty"].ToString()),
                                    afterTitel = drshift["Catagory"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["Shift"].ToString() + ")",
                                    beforeTitle = "Rework by Rework Category" + " : " + drshift["Catagory"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")",
                                }); //, drilldown = drshift["Catagory"].ToString() + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString()
                            }
                            i++;
                        }
                    }
                    #endregion
                }
                else if (strYear != "")
                {
                    #region "Year Wise Information -----------"
                    date = strYear + "-" + "01" + "-" + "01";
                    dtYear = BindCockpitView.BindRejRewDowntimeInfo(date, strShift, plantId, machineId, "YEAR", param, componentId, CellID, employeeId, view);

                    //------------------------Year Wise Start--------------------------                    
                    foreach (DataRow item in dtYear.Rows)
                    {
                        categoryData.Add(item["Catagory"].ToString());
                        chart.series[0].data.Add(new Data
                        {
                            name = item["Catagory"].ToString(),
                            y = Convert.ToDecimal(item["TotalQty"].ToString()),
                            drilldown = item["Catagory"].ToString(),
                            afterTitel = item["Catagory"].ToString() + " (" + strYear + ")",
                            beforeTitle = ""
                        });
                    }
                    //---------------------------Month Wise Start----------------------------

                    dtMonth = BindCockpitView.BindRejRewDowntimeInfo(date, strShift, plantId, machineId, "MONTH", param, componentId, CellID, employeeId, view);
                    chart.drilldown = new List<DrildownSeries>();
                    int i = 0;
                    foreach (var catagory in categoryData)
                    {
                        chart.drilldown.Add(new DrildownSeries
                        {
                            name = catagory,
                            type = type,
                            id = catagory,
                            data = new List<DrildownData>(),
                        });
                        DataRow[] results = dtMonth.Select("Catagory = '" + catagory + "'");
                        if (results != null)
                        {
                            foreach (DataRow drmonth in results)
                            {
                                chart.drilldown[i].data.Add(new DrildownData
                                {
                                    name = drmonth["NameOftheMonth"].ToString(),
                                    y = Convert.ToDecimal(drmonth["TotalQty"].ToString()),
                                    drilldown = drmonth["Catagory"].ToString() + "-" + Convert.ToDateTime(drmonth["Startdate"]).ToString("MM"),
                                    afterTitel = drmonth["Catagory"].ToString() + " (" + strYear + "-" + Convert.ToDateTime(drmonth["Startdate"]).ToString("MM") + ")",
                                    beforeTitle = "Rework by Rework Category" + " : " + drmonth["Catagory"].ToString() + " (" + strYear + ")"
                                });
                            }
                            i++;
                        }
                    }
                    //---------------------------Day Wise Start---------------------------------
                    dtDay = BindCockpitView.BindRejRewDowntimeInfo(date, strShift, plantId, machineId, "DAY", param, componentId, CellID, employeeId, view);
                    foreach (var catagory in categoryData)
                    {
                        foreach (var month in monthName)
                        {
                            chart.drilldown.Add(new DrildownSeries
                            {
                                name = catagory,
                                type = type,
                                id = catagory + "-" + month,
                                data = new List<DrildownData>(),
                            });
                            DataRow[] results = dtDay.Select("Month = " + month + " AND Catagory ='" + catagory + "'");
                            if (results != null)
                            {
                                foreach (DataRow dr in results)
                                {
                                    chart.drilldown[i].data.Add(new DrildownData
                                    {
                                        name = "Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                                        y = Convert.ToDecimal(dr["TotalQty"].ToString()),
                                        drilldown = dr["Catagory"].ToString() + "#@$&*" + month + "#@$&*" + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                                        afterTitel = dr["Catagory"].ToString() + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(dr["PDate"]).ToString("dd") + ")",
                                        beforeTitle = "Rework by Rework Category" + " : " + dr["Catagory"].ToString() + " (" + strYear + "-" + month + ")"
                                    });
                                }
                                i++;
                            }
                        }
                    }

                    //-----------------
                    //date = string.Empty;
                    //foreach (var machine in machineData)
                    //{
                    //    foreach (var month in monthName)
                    //    {
                    //        date = strYear + "-" + month + "-" + "01";
                    //        dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "DAY", param);
                    //        foreach (DataRow day in dtDay.Rows)
                    //        {
                    //            chart.drilldown.Add(new DrildownSeries
                    //            {
                    //                id = machine + "/" + month + "/" + Convert.ToDateTime(day["PDate"]).ToString("dd"),
                    //                data = new List<DrildownData>(),
                    //            });
                    //            date = strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd");
                    //            dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "SHIFT", param);
                    //            foreach (DataRow drshift in dtShift.Rows)
                    //            {
                    //                chart.drilldown[i].data.Add(new DrildownData { name = drshift["Shift"].ToString(), y = Convert.ToDecimal(drshift["Parameter"].ToString()) });
                    //            }
                    //            i++;
                    //        }
                    //    }
                    //}
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return chart;
        }
        #endregion

        #region "Bind Code Chart Data"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Chart<Series> GetRejectedCodeChart(string param, string type, string category)
        {
            string date = string.Empty;
            var chart = new Chart<Series>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = HttpContext.GetLocalResourceObject("~/ReworkPage.aspx", "ReworkbyReworkCode").ToString(),
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };
            if (type == "")
            {
                type = HttpContext.Current.Session["chartType"].ToString();
            }

            DataTable dtYear = new DataTable();
            DataTable dtMonth = new DataTable();
            DataTable dtDay = new DataTable();
            DataTable dtShift = new DataTable();
            List<string> codeData = new List<string>();
            List<string> categoryData = new List<string>();
            List<string> monthData = new List<string>();
            string[] monthName = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };

            if (strShift.Equals("All", StringComparison.OrdinalIgnoreCase))
                strShift = "";

            if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
                plantId = "";

            if (componentId.Equals("All", StringComparison.OrdinalIgnoreCase))
                componentId = "";
            if (employeeId.Equals("All", StringComparison.OrdinalIgnoreCase))
                employeeId = "";

            #region "Month Condition------------"
            if (strMonth.Equals("Jan", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "01";
            }
            else if (strMonth.Equals("Feb", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "02";
            }
            else if (strMonth.Equals("Mar", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "03";
            }
            else if (strMonth.Equals("Apr", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "04";
            }
            else if (strMonth.Equals("May", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "05";
            }
            else if (strMonth.Equals("Jun", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "06";
            }
            else if (strMonth.Equals("Jul", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "07";
            }
            else if (strMonth.Equals("Aug", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "08";
            }
            else if (strMonth.Equals("Sep", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "09";
            }
            else if (strMonth.Equals("Oct", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "10";
            }
            else if (strMonth.Equals("Nov", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "11";
            }
            else if (strMonth.Equals("Dec", StringComparison.OrdinalIgnoreCase))
            {
                strMonth = "12";
            }
            #endregion
            try
            {
                bool colorByPoint = false;
                colorByPoint = type == "column" ? true : false;
                chart.series = new List<Series>();
                chart.series.Add(new Series
                {
                    name = category == "" ? machineId : category,
                    type = type,
                    colorByPoint = colorByPoint,
                    data = new List<Data>()
                });
                if (strYear != "" && strMonth != "" && strDay != "")
                {
                    #region "Day Wise Information-------------"
                    date = strYear + "-" + strMonth + "-" + strDay;
                    dtMonth = BindCockpitView.BindRejRewDowntimeInfo(date, strShift, plantId, machineId, "currentday", param, componentId, CellID, employeeId, view);
                    DataRow[] catMonth = null;
                    if (category == "")
                        catMonth = dtMonth.Select();
                    else
                        catMonth = dtMonth.Select("Catagory='" + category + "'");
                    foreach (DataRow item in catMonth)
                    {
                        codeData.Add(item["Code"].ToString());
                        chart.series[0].data.Add(new Data
                        {
                            name = item["Code"].ToString(),
                            y = Convert.ToDecimal(item["Qty"].ToString()),
                            drilldown = Convert.ToDateTime(item["PDate"]).ToString("MM") + "/" + strDay + "/" + item["Code"].ToString(),
                            afterTitel = item["Code"].ToString() + " (" + strYear + "-" + strMonth + "-" + strDay + ")",
                            beforeTitle = ""
                        });
                    }
                    //-------------------------------Shift Wise Info-----------------------------------
                    chart.drilldown = new List<DrildownSeries>();
                    int i = 0;
                    dtShift = BindCockpitView.BindRejRewDowntimeInfo(date, strShift, plantId, machineId, "SHIFT", param, componentId, CellID, employeeId, view);
                    foreach (var code in codeData)
                    {
                        chart.drilldown.Add(new DrildownSeries
                        {
                            name = code,
                            type = type,
                            id = strMonth + "/" + strDay + "/" + code,
                            data = new List<DrildownData>(),
                        });
                        DataRow[] results = null;
                        if (category == "")
                            results = dtShift.Select("Month = " + strMonth + " AND Code ='" + code + "' AND Day ='" + strDay + "'");
                        else
                            results = dtShift.Select("Month = " + strMonth + " AND Code ='" + code + "' AND Day ='" + strDay + "' AND Catagory='" + category + "'");
                        foreach (DataRow drshift in results)
                        {
                            chart.drilldown[i].data.Add(new DrildownData
                            {
                                name = drshift["Shift"].ToString(),
                                y = Convert.ToDecimal(drshift["Qty"].ToString()),
                                afterTitel = drshift["Code"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["Shift"].ToString() + ")",
                                beforeTitle = "Rework by Rework Code" + " : " + drshift["Code"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")"
                            });//, drilldown = drshift["Code"].ToString() + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString()
                        }
                        i++;
                    }
                    #endregion
                }
                else if (strYear != "" && strMonth != "")
                {
                    #region "month Wise Information ------------"
                    date = strYear + "-" + strMonth + "-" + "01";
                    dtMonth = BindCockpitView.BindRejRewDowntimeInfo(date, strShift, plantId, machineId, "currentMONTH", param, componentId, CellID, employeeId, view);
                    DataRow[] catMonth = null;
                    if (category == "")
                        catMonth = dtMonth.Select();
                    else
                        catMonth = dtMonth.Select("Catagory='" + category + "'");
                    foreach (DataRow item in catMonth)
                    {
                        codeData.Add(item["Code"].ToString());
                        chart.series[0].data.Add(new Data
                        {
                            name = item["Code"].ToString(),
                            y = Convert.ToDecimal(item["Qty"].ToString()),
                            drilldown = item["Code"].ToString(),
                            afterTitel = item["Code"].ToString() + " (" + strYear + "-" + strMonth + ")",
                            beforeTitle = ""
                        });
                    }

                    //---------------------------Day Wise Start---------------------------------
                    chart.drilldown = new List<DrildownSeries>();
                    int i = 0;
                    dtDay = BindCockpitView.BindRejRewDowntimeInfo(date, strShift, plantId, machineId, "DAY", param, componentId, CellID, employeeId, view);
                    foreach (var code in codeData)
                    {
                        chart.drilldown.Add(new DrildownSeries
                        {
                            name = code,
                            type = type,
                            id = code,
                            data = new List<DrildownData>(),
                        });
                        DataRow[] results = null;
                        if (category == "")
                            results = dtDay.Select("Month = " + strMonth + " AND Code ='" + code + "'");
                        else
                            results = dtDay.Select("Month = " + strMonth + " AND Code ='" + code + "' AND Catagory='" + category + "'");
                        foreach (DataRow dr in results)
                        {
                            chart.drilldown[i].data.Add(new DrildownData
                            {
                                name = "Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                                y = Convert.ToDecimal(dr["Qty"].ToString()),
                                drilldown = dr["Code"].ToString() + "/" + strMonth + "/" + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                                afterTitel = dr["Code"].ToString() + " (" + strYear + "-" + strMonth + "-" + Convert.ToDateTime(dr["PDate"]).ToString("dd") + ")",
                                beforeTitle = "Rework by Rework Code" + " : " + dr["Code"].ToString() + " (" + strYear + "-" + strMonth + ")"
                            });
                            monthData.Add(Convert.ToDateTime(dr["PDate"]).ToString("dd"));
                        }
                        i++;
                    }
                    //--------------------------Shift Wise Information-----------------------------
                    monthData = (from w in monthData select w).Distinct().ToList();
                    dtShift = BindCockpitView.BindRejRewDowntimeInfo(date, strShift, plantId, machineId, "SHIFT", param, componentId, CellID, employeeId, view);
                    foreach (var code in codeData)
                    {
                        foreach (var day in monthData)
                        {
                            chart.drilldown.Add(new DrildownSeries
                            {
                                name = code,
                                type = type,
                                id = code + "/" + strMonth + "/" + day,
                                data = new List<DrildownData>(),
                            });
                            DataRow[] results = null;
                            if (category == "")
                                results = dtShift.Select("Month = " + strMonth + " AND Code ='" + code + "' AND Day ='" + day + "'");
                            else
                                results = dtShift.Select("Month = " + strMonth + " AND Code ='" + code + "' AND Day ='" + day + "' AND Catagory='" + category + "'");
                            foreach (DataRow drshift in results)
                            {
                                chart.drilldown[i].data.Add(new DrildownData
                                {
                                    name = drshift["Shift"].ToString(),
                                    y = Convert.ToDecimal(drshift["Qty"].ToString()),
                                    afterTitel = drshift["Code"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + "-" + drshift["Shift"].ToString() + ")",
                                    beforeTitle = "Rework by Rework Code" + " : " + drshift["Code"].ToString() + " (" + strYear + "-" + drshift["Month"].ToString() + "-" + drshift["Day"].ToString() + ")",
                                }); //, drilldown = drshift["Code"].ToString() + "/" + drshift["Month"].ToString() + "/" + drshift["Day"].ToString() + "/" + drshift["Shift"].ToString() 
                            }
                            i++;
                        }
                    }
                    #endregion
                }
                else if (strYear != "")
                {
                    #region "Year Wise Information -----------"
                    date = strYear + "-" + "01" + "-" + "01";
                    dtYear = BindCockpitView.BindRejRewDowntimeInfo(date, strShift, plantId, machineId, "YEAR", param, componentId, CellID, employeeId, view);
                    DataRow[] catYear = null;
                    if (category == "")
                        catYear = dtYear.Select();
                    else
                        catYear = dtYear.Select("Catagory='" + category + "'");
                    //------------------------Year Wise Start--------------------------                   
                    foreach (DataRow item in catYear)
                    {
                        codeData.Add(item["Code"].ToString());
                        chart.series[0].data.Add(new Data
                        {
                            name = item["Code"].ToString(),
                            y = Convert.ToDecimal(item["Qty"].ToString()),
                            drilldown = item["Code"].ToString(),
                            afterTitel = item["Code"].ToString() + " (" + strYear + ")",
                            beforeTitle = ""
                        });
                    }
                    //---------------------------Month Wise Start----------------------------

                    dtMonth = BindCockpitView.BindRejRewDowntimeInfo(date, strShift, plantId, machineId, "MONTH", param, componentId, CellID, employeeId, view);
                    chart.drilldown = new List<DrildownSeries>();
                    int i = 0;
                    foreach (var code in codeData)
                    {
                        chart.drilldown.Add(new DrildownSeries
                        {
                            name = code,
                            type = type,
                            id = code,
                            data = new List<DrildownData>(),
                        });
                        DataRow[] results = null;
                        if (category == "")
                            results = dtMonth.Select("Code = '" + code + "'");
                        else
                            results = dtMonth.Select("Code = '" + code + "' AND Catagory='" + category + "'");
                        if (results != null)
                        {
                            foreach (DataRow drmonth in results)
                            {
                                chart.drilldown[i].data.Add(new DrildownData
                                {
                                    name = drmonth["NameOftheMonth"].ToString(),
                                    y = Convert.ToDecimal(drmonth["Qty"].ToString()),
                                    drilldown = drmonth["Code"].ToString() + "-" + Convert.ToDateTime(drmonth["Startdate"]).ToString("MM"),
                                    afterTitel = drmonth["Code"].ToString() + " (" + strYear + "-" + Convert.ToDateTime(drmonth["Startdate"]).ToString("MM") + ")",
                                    beforeTitle = "Rework by Rework Code" + " : " + drmonth["Code"].ToString() + " (" + strYear + ")"
                                });
                            }
                            i++;
                        }
                    }
                    //---------------------------Day Wise Start---------------------------------
                    dtDay = BindCockpitView.BindRejRewDowntimeInfo(date, strShift, plantId, machineId, "DAY", param, componentId, CellID, employeeId, view);
                    foreach (var code in codeData)
                    {
                        foreach (var month in monthName)
                        {
                            chart.drilldown.Add(new DrildownSeries
                            {
                                name = code,
                                type = type,
                                id = code + "-" + month,
                                data = new List<DrildownData>(),
                            });
                            DataRow[] results = null;
                            if (category == "")
                                results = dtDay.Select("Month = " + month + " AND Code ='" + code + "'");
                            else
                                results = dtDay.Select("Month = " + month + " AND Code ='" + code + "' AND Catagory='" + category + "'");
                            if (results != null)
                            {
                                foreach (DataRow dr in results)
                                {
                                    chart.drilldown[i].data.Add(new DrildownData
                                    {
                                        name = "Day" + " " + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                                        y = Convert.ToDecimal(dr["Qty"].ToString()),
                                        drilldown = dr["Code"].ToString() + "#@$&*" + month + "#@$&*" + Convert.ToDateTime(dr["PDate"]).ToString("dd"),
                                        afterTitel = dr["Code"].ToString() + " (" + strYear + "-" + month + "-" + Convert.ToDateTime(dr["PDate"]).ToString("dd") + ")",
                                        beforeTitle = "Rework by Rework Code" + " : " + dr["Code"].ToString() + " (" + strYear + "-" + month + ")"
                                    });
                                }
                                i++;
                            }
                        }
                    }

                    //-----------------
                    //date = string.Empty;
                    //foreach (var machine in machineData)
                    //{
                    //    foreach (var month in monthName)
                    //    {
                    //        date = strYear + "-" + month + "-" + "01";
                    //        dtDay = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "DAY", param);
                    //        foreach (DataRow day in dtDay.Rows)
                    //        {
                    //            chart.drilldown.Add(new DrildownSeries
                    //            {
                    //                id = machine + "/" + month + "/" + Convert.ToDateTime(day["PDate"]).ToString("dd"),
                    //                data = new List<DrildownData>(),
                    //            });
                    //            date = strYear + "-" + month + "-" + Convert.ToDateTime(day["PDate"]).ToString("dd");
                    //            dtShift = BindCockpitView.BindDashBoardGraph(date, strShift, plantId, machine, "SHIFT", param);
                    //            foreach (DataRow drshift in dtShift.Rows)
                    //            {
                    //                chart.drilldown[i].data.Add(new DrildownData { name = drshift["Shift"].ToString(), y = Convert.ToDecimal(drshift["Parameter"].ToString()) });
                    //            }
                    //            i++;
                    //        }
                    //    }
                    //}
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return chart;
        }
        #endregion

        #region "Shift Wise Category and Code Chart Data---------"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Chart<Series> GetShiftChartData(string strMonth, string strDay, string param, string type, string catagory, string code)
        {
            var xasis = catagory == "" ? code : catagory;
            string date = string.Empty;
            var chart = new Chart<Series>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = (param == "ReworkCategory" ? HttpContext.GetLocalResourceObject("~/ReworkPage.aspx", "ReworkbyReworkCategory").ToString() : HttpContext.GetLocalResourceObject("~/ReworkPage.aspx", "ReworkbyReworkCode").ToString()) + " : " + xasis + " (" + strYear + "-" + strMonth + "-" + strDay + ")",
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix"
            };
            DataTable dtYear = new DataTable();
            if (strShift.Equals("All", StringComparison.OrdinalIgnoreCase))
                strShift = "";
            if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
                plantId = "";

            if (componentId.Equals("All", StringComparison.OrdinalIgnoreCase))
                componentId = "";
            if (employeeId.Equals("All", StringComparison.OrdinalIgnoreCase))
                employeeId = "";

            try
            {
                date = strYear + "-" + strMonth + "-" + strDay;
                dtYear = BindCockpitView.BindRejRewDowntimeInfo(date, strShift, plantId, machineId, "CurrentSHIFT", param, componentId, CellID, employeeId, view);
                if (type == "")
                {
                    type = HttpContext.Current.Session["chartType"].ToString();
                }
                //------------------------Year Wise Start--------------------------
                chart.series = new List<Series>();
                chart.series.Add(new Series
                {
                    type = type,
                    name = machineId + '(' + date + ')',
                    data = new List<Data>()
                });
                DataRow[] results = null;
                if (catagory != "")
                    results = dtYear.Select("Month = " + strMonth + " AND Catagory ='" + catagory + "' AND Day ='" + strDay + "'");
                else
                    results = dtYear.Select("Month = " + strMonth + " AND Code ='" + code + "' AND Day ='" + strDay + "'");
                foreach (DataRow item in results)
                {
                    if (catagory != "")
                        chart.series[0].data.Add(new Data { name = item["Shift"].ToString(), y = Convert.ToDecimal(item["TotalQty"].ToString()) }); //, drilldown = item["MachineID"].ToString() + "/" + item["Month"].ToString() + "/" + item["Day"].ToString() + "/" + item["Shift"].ToString()
                    else
                        chart.series[0].data.Add(new Data { name = item["Shift"].ToString(), y = Convert.ToDecimal(item["Qty"].ToString()) }); //, drilldown = item["MachineID"].ToString() + "/" + item["Month"].ToString() + "/" + item["Day"].ToString() + "/" + item["Shift"].ToString()
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //_r.Next(100)
            return chart;
        }
        #endregion
    }
}