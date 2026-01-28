using BusinessClassLibrary;
using Elmah;
using ModelClassLibrary;
using System;
using System.Collections.Generic;
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
    public partial class Rejection_Globe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                Session["RejectionParetoData"] = null;
                BindPlantId();
                ddlPlantId_SelectedIndexChanged(null, null);
                BindCategory();
                ddlCategory_SelectedIndexChanged(null, null);
               // BindSubCategory();
                //BindDesCategory();
               // BindGroups();
               // BindMachines();
               // BindDownIdInfo();
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.AddDays(+1).ToString("dd-MM-yyyy");
               // BindComponent();
               // BindOperation();
                hdfValue.Value = "OK";
                if(lbComponentID.Items.Count>0)
                {
                    lbComponentID.Items[0].Selected = true;
                }
                if (ddlMultiDownID.Items.Count > 0)
                {
                    foreach(ListItem item in ddlMultiDownID.Items)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        private void BindOperation()
        {
            try
            {
                //List<string> Operation = DataBaseAccess.GetOperation(ddlMachineId.SelectedValue == "All" ? "" : ddlMachineId.SelectedValue.ToString(), ddlComponentID.SelectedValue == "All" ? "" : ddlComponentID.SelectedValue.ToString());
                List<string> Operation = DataBaseAccess.GetOperationForMultiComponent(ddlMachineId.SelectedValue == "All" ? "" : ddlMachineId.SelectedValue.ToString(), getComponentId());
                Operation.Insert(0, "All");
                ddlOperation.DataSource = Operation;
                ddlOperation.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }

        }
        private string getComponentId()
        {
            string compid = "";
            try
            {
                int i = 0;
                foreach (ListItem item in lbComponentID.Items)
                {
                    if (item.Selected)
                    {
                        if (i == 0)
                        {
                            compid += "'" + item.Value + "'";
                        }
                        else
                        {
                            compid += ",'" + item.Value + "'";
                        }
                        i++;
                    }
                }
            }
            catch (Exception e)
            { }
            return compid;
        }

        private void BindComponent()
        {
            try
            {
                List<string> ComponentID = DataBaseAccess.GetComponentIDByMachine(ddlMachineId.SelectedValue == "All" ? "" : ddlMachineId.SelectedValue.ToString());
                //ComponentID.Insert(0, "All");
                //ddlComponentID.DataSource = ComponentID;
                //ddlComponentID.DataBind();
                lbComponentID.DataSource = ComponentID;
                lbComponentID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
        }

        #region "Bind Plant Id"
        private void BindPlantId()
        {
            try
            {
                List<string> lstPlantData = BindCockpitView.ViewPlantToDisplay();
                ddlPlantId.DataSource = lstPlantData;
                ddlPlantId.DataBind();
                ddlPlantId.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommanResource", "PlantAll").ToString(), "ALL"));
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Group"
        private void BindGroups()
        {
            try
            {
                List<string> groups = DataBaseAccess.GetAllGroupId(ddlPlantId.SelectedValue != "ALL" ? ddlPlantId.SelectedValue.ToString() : string.Empty);
                if (groups.Count > 0)
                    groups.Insert(0, "All");
                ddlGroup.DataSource = groups;
                ddlGroup.DataBind();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Machine Id"
        private void BindMachines()
        {
            try
            {
                ddlMachineId.Items.Clear();// = null;
                var allMachineName = DataBaseAccess.GetAllMachinedByLineandGroup(ddlPlantId.SelectedValue != "ALL" ? ddlPlantId.SelectedValue.ToString() : string.Empty, ddlGroup.SelectedValue != "All" ? ddlGroup.SelectedValue.ToString() : string.Empty);// AccessReportData.GetAllMachines(ddlPlantId.SelectedItem.ToString());
                allMachineName.Insert(0, "All");
                if (allMachineName != null && allMachineName.Count > 0)
                {
                    ddlMachineId.DataSource = allMachineName;
                    ddlMachineId.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind DownId Information"
        private void BindDownIdInfo()
        {
            try
            {
                //foreach (ListItem item in ddlMachineId.Items)
                //{
                //    if (item.Selected)
                //    {
                //        listMachine += item.Value + "$@#";
                //    }
                //}
                //if (listMachine != "")
                //{
                //    string[] result = listMachine.Split(new string[] { "$@#" }, StringSplitOptions.None);
                //    result = result.Take(result.Count() - 1).ToArray();
                //    SalesManCode = "'" + string.Join("','", result.ToArray()) + "'";
                //}
                var DownId = DataBaseAccess.GetMachinewiseRejectionInfo(ddlSubCategory.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlSubCategory.SelectedValue.ToString(), ddlCategory.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCategory.SelectedValue.ToString(), ddlDesCategory.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlDesCategory.SelectedValue);//TMPTrakDataBase.GetDownIdInfo();
                if (DownId != null)
                {
                    ddlMultiDownID.DataSource = DownId;
                    ddlMultiDownID.DataBind();
                }

                //var subCat = DataBaseAccess.GetSubCatList(ddlCategory.SelectedValue != "All" ? ddlCategory.SelectedValue.ToString() : string.Empty);
                //if (subCat != null)
                //{
                //    ddlSubCategory.DataSource = subCat;
                //    ddlSubCategory.DataBind();
                //    //ddlSubCategory.Items.Insert(0, new ListItem("All", "All"));
                //}

                //var desCat = DataBaseAccess.GetDesCatList(ddlCategory.SelectedValue != "All" ? ddlCategory.SelectedValue.ToString() : string.Empty, ddlSubCategory.SelectedValue != "All" ? ddlCategory.SelectedValue.ToString() : string.Empty);
                //if (desCat != null)
                //{
                //    ddlDesCategory.DataSource = desCat;
                //    ddlDesCategory.DataBind();
                //    //ddlDesCategory.Items.Insert(0, new ListItem("All", "All"));
                //}
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Category"
        private void BindCategory()
        {
            try
            {
                List<string> groups = DataBaseAccess.databindforCatagorys();
                ddlCategory.DataSource = groups;
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("All", "All"));
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind SubCategory"
        private void BindSubCategory()
        {
            try
            {
                string Category = ddlCategory.SelectedValue.ToString().Equals("all", StringComparison.OrdinalIgnoreCase) ? "" : ddlCategory.SelectedValue.ToString();
                List<string> groups = DataBaseAccess.databindforSubCatagorys(Category);
                ddlSubCategory.DataSource = groups;
                ddlSubCategory.DataBind();
                ddlSubCategory.Items.Insert(0, new ListItem("All", "All"));
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind DesCategory"
        private void BindDesCategory()
        {
            try
            {
                string Category = ddlCategory.SelectedValue.ToString().Equals("all", StringComparison.OrdinalIgnoreCase) ? "" : ddlCategory.SelectedValue.ToString();
                string SubCategory = ddlSubCategory.SelectedValue.ToString().Equals("all", StringComparison.OrdinalIgnoreCase) ? "" : ddlSubCategory.SelectedValue.ToString();
                List<string> groups = DataBaseAccess.databindforDesCatagorys(Category, SubCategory);
                ddlDesCategory.DataSource = groups;
                ddlDesCategory.DataBind();
                ddlDesCategory.Items.Insert(0, new ListItem("All", "All"));
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        //protected void ddlMultiDownID_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindDownIdInfo();
        //    hdfValue.Value = "NotOk";
        //}

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGroups();
            BindMachines();
            BindComponent();
            BindOperation();
            hdfValue.Value = "NotOk";
        }
        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachines();
            BindComponent();
            BindOperation();
            //BindDownIdInfo();
            hdfValue.Value = "NotOk";
        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            Session["DownParetoData"] = null;
            hdfValue.Value = "OK";
        }

        protected void ddlMachineId_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindDownIdInfo();
            BindComponent();
            BindOperation();
            hdfValue.Value = "NotOk";
        }

        protected void btnHideExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;
                fromDate = Util.GetDateTime(txtFromDate.Text);
                toDate = Util.GetDateTime(txtToDate.Text);

                string machineSelection = "",
                    plantID = string.Empty, Category = string.Empty, Groupid = string.Empty, machineId = string.Empty, ComponentID = string.Empty, RejCode = string.Empty, Operation = string.Empty, SubCategory = string.Empty, RejDescription = string.Empty;
                //#region "Machine selection"
                //foreach (ListItem item in ddlMachineId.Items)
                //{
                //    if (item.Selected)
                //    {
                //        listMachine += item.Value + "$@#";
                //    }
                //}
                //if (listMachine != "")
                //{
                //    string[] result = listMachine.Split(new string[] { "$@#" }, StringSplitOptions.None);
                //    result = result.Take(result.Count() - 1).ToArray();
                //    machineSelection = string.Join(",", result.ToArray());
                //}
                //#endregion

                string downId = "", listDownId = "";

                #region "DownId Selection "
                foreach (ListItem item in ddlMultiDownID.Items)
                {
                    if (item.Selected)
                    {
                        listDownId += item.Value + "$@#";
                    }
                }
                if (listDownId != "")
                {
                    string[] result = listDownId.Split(new string[] { "$@#" }, StringSplitOptions.None);
                    result = result.Take(result.Count() - 1).ToArray();
                    downId = string.Join(",", result.Select(item => "'" + item + "'"));
                }
                #endregion

                plantID = ddlPlantId.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantId.SelectedValue.ToString();
                Groupid = ddlGroup.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlGroup.SelectedValue.ToString();
                machineId = ddlMachineId.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlMachineId.SelectedValue.ToString();
                //ComponentID = ddlComponentID.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlComponentID.SelectedValue.ToString();
                ComponentID = getComponentId();
                Operation = ddlOperation.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlOperation.SelectedValue.ToString();
                Category = ddlCategory.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCategory.SelectedValue.ToString();
                RejCode = ddlMultiDownID.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlMultiDownID.SelectedValue.ToString();
                SubCategory = ddlSubCategory.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlSubCategory.SelectedValue.ToString();
                RejDescription = ddlDesCategory.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlDesCategory.SelectedValue.ToString();
                if (HttpContext.Current.Session["RejectionParetoData"] == null)
                    dt = DataBaseAccess.MachineRejectionCodeInfo(fromDate.Date, toDate.Date, machineId, Groupid, plantID, ComponentID, Operation, Category, SubCategory, RejDescription, RejCode);
                else
                    dt = HttpContext.Current.Session["RejectionParetoData"] as DataTable;
                string rpt = ExportExcelEpplus.ExportScrapAnalysis(dt, plantID, Groupid, machineSelection, Category, SubCategory, RejDescription, downId, fromDate.Date, toDate.Date, "Globe");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }

        }

        #region "Get Top Down Time Pareto Chart "
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Chart<DrildownData> RejectionChartInfo(string plantID, string Groupid, string machineId, string startFromDate, string startToDate, string ComponentID, string Operation, string Category, string SubCategory, string RejDescription, string downReasonsstring, string RejCode)
        {
            var chart = new Chart<DrildownData>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = "XAxisTitle",
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix",
            };

            try
            {
                int downReasons = Convert.ToInt32(downReasonsstring);
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;


                fromDate = Util.GetDateTime(startFromDate);
                toDate = Util.GetDateTime(startToDate);


                string plantId = string.Empty;
                #region "Machine selection"
                if (machineId == "null")
                {
                    machineId = "";
                }
                #endregion

                #region "DownId Selection "
                RejCode = RejCode.Equals("null") ? "" : RejCode;
                //if (!string.IsNullOrEmpty(RejCode))
                //{
                //    string[] result = RejCode.Split(new string[] { "," }, StringSplitOptions.None);
                //    RejCode = string.Empty;
                //    foreach(string item in result)
                //    {
                //        RejCode += "'" + item + "',";
                //    }
                //    RejCode = RejCode.TrimEnd(',');
                //}

                #endregion

                plantID = plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID;
                Groupid = Groupid.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Groupid;
                machineId = machineId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineId;
               // ComponentID = ComponentID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ComponentID;
                Operation = Operation.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Operation;
                Category = Category.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Category;
                RejCode = RejCode.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : RejCode;
                SubCategory = SubCategory.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : SubCategory;
                RejDescription = RejDescription.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : RejDescription;
                DataTable dt = null;
                //if (HttpContext.Current.Session["RejectionParetoData"] == null)
                //   
                HttpContext.Current.Session["RejectionParetoData"] = dt = DataBaseAccess.MachineRejectionCodeInfo(fromDate.Date, toDate.Date, plantId, Groupid, machineId,getSplittedInputValue(ComponentID), Operation, Category, SubCategory, RejDescription, RejCode);
                //else
                //    dt = HttpContext.Current.Session["RejectionParetoData"] as DataTable;
                DataTable sortedDT = new DataTable();
                DataView dv = dt.DefaultView;
                if (dv != null && dv.Table.Rows.Count > 0)
                {
                    dv.Sort = "RejQty desc";
                    sortedDT = dv.ToTable();
                }

                chart.series = new List<DrildownData>();
                List<string> cat = sortedDT.AsEnumerable().Select(r => r.Field<string>("RejReason")).ToList();

                //cat.Add("Other");
                chart.categories = new List<string>();
                for (int i = 0; i < cat.Count; i++)
                {
                    string value = cat[i];
                    if (i == downReasons)
                    {
                        chart.categories.Add("Other");
                        chart.categories.Add(value);
                    }
                    else
                        chart.categories.Add(value);
                }

                decimal decVal = 0;
                bool flag = true;
                for (int i = 0; i < sortedDT.Rows.Count; i++)
                {
                    DrildownData obj = new DrildownData();
                    if (i == downReasons)
                    {
                        flag = false;
                        List<double> declistVal = sortedDT.AsEnumerable().Select(r => r.Field<double>("RejQty")).Skip(downReasons).ToList();
                        for (int j = 0; j < declistVal.Count; j++)
                        {
                            decVal += Convert.ToDecimal(declistVal[j]);
                        }
                        decimal myNumber = decVal;
                        obj.y = decimal.Round(myNumber, 2);
                        obj.name = "Other";
                        obj.drilldown = "Other";
                        chart.series.Add(obj);
                        // chart.categories.Add("Other");
                        break;
                    }
                    if (flag)
                    {
                        //decVal += Convert.ToDecimal(sortedDT.Rows[i]["RejQty"].ToString());
                        decimal myNumber = Convert.ToDecimal(sortedDT.Rows[i]["RejQty"].ToString());
                        obj.y = decimal.Round(myNumber, 2);
                        obj.name = sortedDT.Rows[i]["RejReason"].ToString();
                    }
                    chart.series.Add(obj);
                    // chart.categories.Add(sortedDT.Rows[i]["DownTime"].ToString());
                }

                var rows = sortedDT.AsEnumerable().Skip(downReasons).ToList();
                chart.drilldown = new List<DrildownSeries>();
                chart.drilldown.Add(new DrildownSeries
                {
                    //type = "column",
                    name = "Other",
                    id = "Other",
                    colorByPoint = true,
                    data = new List<DrildownData>(),
                    // categories = new List<string>(),
                });
                foreach (var machine in rows)
                {
                    chart.drilldown[0].data.Add(new DrildownData
                    {
                        name = machine[1].ToString(),
                        y = Convert.ToDecimal(machine[3].ToString()),
                    });
                    //chart.drilldown[0].categories.Add(machine[1].ToString());
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
            return chart;
        }
        #endregion

        #region "Get Top Drill down Chart "
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ChartSeries<Series1> RejectionDrillDownInfo(string plantID, string Groupid, string machineId, string startFromDate, string startToDate, string ComponentID, string Operation, string Category, string SubCategory, string DesCategory, string downReasonsstring, string RejCode)
        {
            var chart = new ChartSeries<Series1>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = "Rejection Chart",
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix",
            };

            try
            {
                int downReasons = Convert.ToInt32(downReasonsstring);
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;

                fromDate = Util.GetDateTime(startFromDate);
                toDate = Util.GetDateTime(startToDate);

                string plantId = string.Empty;
                #region "Machine selection"
                if (machineId == "null")
                {
                    machineId = "";
                }
                #endregion
                //string downId = "";
                #region "DownId Selection "
                RejCode = RejCode.Equals("null") ? "" : RejCode;
                //if (!string.IsNullOrEmpty(RejCode))
                //{
                //    string[] result = RejCode.Split(new string[] { "," }, StringSplitOptions.None);
                //    RejCode = string.Empty;
                //    foreach(string item in result)
                //    {
                //        RejCode += "'" + item + "',";
                //    }
                //    RejCode = RejCode.TrimEnd(',');
                //}

                #endregion

                plantID = plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID;
                Groupid = Groupid.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Groupid;
                machineId = machineId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineId;
               // ComponentID = ComponentID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ComponentID;
                Operation = Operation.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Operation;
                Category = Category.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Category;
                SubCategory = SubCategory.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : SubCategory;
                DesCategory = DesCategory.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : DesCategory;
                RejCode = RejCode.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : RejCode;

                ComponentID = getSplittedInputValue(ComponentID);
                DataTable dtbyCategory = DataBaseAccess.GetChartInfo(fromDate.Date, toDate.Date, plantId, Groupid, machineId, ComponentID, Operation, Category, SubCategory, DesCategory, RejCode, "ByRejCategory");
                DataTable dtbySubcategory = DataBaseAccess.GetChartInfo(fromDate.Date, toDate.Date, plantId, Groupid, machineId, ComponentID, Operation, Category, SubCategory, DesCategory, RejCode, "ByRejSubCategory");
                DataTable dtbyDiscription = DataBaseAccess.GetChartInfo(fromDate.Date, toDate.Date, plantId, Groupid, machineId, ComponentID, Operation, Category, SubCategory, DesCategory, RejCode, "ByRejDesciption");

                //List<ChartData> data = new List<ChartData>();

                List<string> categoryData = new List<string>();
                List<string> SubcategoryData = new List<string>();


                bool colorByPoint = false;
                var type = "column";
                colorByPoint = type == "column" ? true : false;
                chart.series = new List<Series1>();
                chart.series.Add(new Series1
                {
                    name = "Rejection Chart",
                    type = type,
                    colorByPoint = colorByPoint,
                    data = new List<ChartData>()
                });

                if (dtbyCategory != null && dtbyCategory.Rows.Count > 0)
                {
                    List<string> categories = new List<string>();
                    foreach (DataRow dtRow in dtbyCategory.Rows)
                    {
                        if (Convert.ToDouble(dtRow["RejQty"].ToString()) != 0)
                        {
                            categoryData.Add(dtRow["RejCategory"].ToString());
                            chart.series[0].data.Add(new ChartData
                            {
                                name = dtRow["RejCategory"].ToString(),
                                y = Convert.ToDouble(dtRow["RejQty"].ToString()),
                                drilldown = dtRow["RejCategory"].ToString(),
                                afterTitle = dtRow["RejCategory"].ToString(),
                                beforeTitle = "Rejection Chart",
                                Title = dtRow["RejCategory"].ToString()

                            });
                        }


                    }
                    chart.categories = new List<string>();
                    chart.categories = categoryData;

                    chart.drilldown = new List<DrildownSeries1>();
                    int i = 0;

                    if (dtbySubcategory != null && dtbySubcategory.Rows.Count > 0)
                    {

                        foreach (var catagory in categoryData)
                        {
                            chart.drilldown.Add(new DrildownSeries1
                            {
                                name = catagory,
                                type = type,
                                id = catagory,
                                data = new List<DrildownData1>(),
                            });
                            foreach (DataRow dtRow in dtbySubcategory.Rows)
                            {
                                if (Convert.ToDouble(dtRow["RejQty"].ToString()) != 0)
                                {
                                    SubcategoryData.Add(dtRow["RejSubcategory"].ToString());
                                    if (catagory == dtRow["RejCategory"].ToString())
                                    {
                                        chart.drilldown[i].data.Add(new DrildownData1
                                        {
                                            name = dtRow["RejSubcategory"].ToString(),
                                            y = Convert.ToDouble(dtRow["RejQty"].ToString()),
                                            drilldown = dtRow["RejSubcategory"].ToString(),
                                            afterTitle = dtRow["RejCategory"] + "*-*" + dtRow["RejSubcategory"].ToString(),
                                            beforeTitle = dtRow["RejCategory"].ToString(),
                                            Title = "(" + dtRow["RejCategory"] + ") - (" + dtRow["RejSubcategory"].ToString() + ")",

                                        });
                                    }
                                }


                            }
                            i++;
                        }
                    }


                    if (dtbyDiscription != null && dtbyDiscription.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtbySubcategory.Rows.Count; j++)
                        {
                            DataRow[] result = dtbyDiscription.Select("RejCategory = '" + dtbySubcategory.Rows[j]["RejCategory"] + "' AND RejSubcategory = '" + dtbySubcategory.Rows[j]["RejSubcategory"] + "'");
                            chart.drilldown.Add(new DrildownSeries1
                            {
                                name = dtbySubcategory.Rows[j]["RejCategory"].ToString(),
                                type = type,
                                id = dtbySubcategory.Rows[j]["RejSubcategory"].ToString(),
                                data = new List<DrildownData1>(),
                            });

                            foreach (DataRow dtRow in result)
                            {
                                //foreach (var catagory in SubcategoryData)
                                //{


                                if (Convert.ToDouble(dtRow["RejQty"].ToString()) != 0)
                                {

                                    chart.drilldown[i].data.Add(new DrildownData1
                                    {
                                        name = dtRow["RejDesciption"].ToString(),
                                        y = Convert.ToDouble(dtRow["RejQty"].ToString()),
                                        drilldown = "",
                                        afterTitle = dtRow["RejCategory"] + "*-*" + dtRow["RejSubcategory"] + "*-*" + dtRow["RejDesciption"].ToString(),
                                        beforeTitle = dtRow["RejCategory"] + "*-*" + dtRow["RejSubcategory"].ToString(),
                                        Title = "(" + dtRow["RejCategory"] + ") - (" + dtRow["RejSubcategory"] + ") - (" + dtRow["RejDesciption"].ToString() + ")",

                                    });

                                }

                            }
                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
            return chart;
        }
        #endregion

        #region "Get category Drill down Chart "
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ChartSeries<Series1> GetCategoryDrillDownChart(string plantID, string Groupid, string machineId, string startFromDate, string startToDate, string ComponentID, string Operation, string Category, string SubCategory, string DesCategory, string downReasonsstring, string RejCode, string Param)
        {
            var chart = new ChartSeries<Series1>
            {
                Title = "TITLE",
                Subtitle = "SubTitle",
                XAxisTitle = "Rejection Chart",
                YAxisTitle = "YAxisTitle",
                YAxisTooltipValueSuffix = "YAxisTooltipValueSuffix",
            };

            try
            {
                int downReasons = Convert.ToInt32(downReasonsstring);
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;

                fromDate = Util.GetDateTime(startFromDate);
                toDate = Util.GetDateTime(startToDate);

                string plantId = string.Empty;
                #region "Machine selection"
                if (machineId == "null")
                {
                    machineId = "";
                }
                #endregion
                #region "DownId Selection "
                RejCode = RejCode.Equals("null") ? "" : RejCode;


                #endregion

                plantID = plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID;
                Groupid = Groupid.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Groupid;
                machineId = machineId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineId;
                //ComponentID = ComponentID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ComponentID;
                Operation = Operation.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Operation;
                Category = Category.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Category;
                SubCategory = SubCategory.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : SubCategory;
                DesCategory = DesCategory.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : DesCategory;
                RejCode = RejCode.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : RejCode;
                //string param = "ByRejCategory";
                ComponentID = getSplittedInputValue(ComponentID);
                DataTable dt = DataBaseAccess.GetChartInfo(fromDate.Date, toDate.Date, plantId, Groupid, machineId, ComponentID, Operation, Category, SubCategory, DesCategory, RejCode, Param);


                //List<ChartData> data = new List<ChartData>();

                bool colorByPoint = false;
                var type = "column";
                colorByPoint = type == "column" ? true : false;
                chart.series = new List<Series1>();
                chart.series.Add(new Series1
                {
                    name = "Rejection Code",
                    type = type,
                    colorByPoint = colorByPoint,
                    data = new List<ChartData>()
                });

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<string> categories = new List<string>();
                    foreach (DataRow dtRow in dt.Rows)
                    {
                        if (Convert.ToDouble(dtRow["RejQty"].ToString()) != 0)
                        {
                            categories.Add(dtRow["RejReason"].ToString());
                            chart.series[0].data.Add(new ChartData
                            {
                                name = dtRow["RejReason"].ToString(),
                                y = Convert.ToDouble(dtRow["RejQty"].ToString()),
                                drilldown = "",
                                afterTitle = "Description Category",
                                beforeTitle = "SubCategory",
                                Title = "Rejection Code",
                            });
                        }

                    }
                    chart.categories = new List<string>();
                    chart.categories = categories;
                }


            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
            return chart;
        }
        #endregion

        protected void ddlComponentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOperation();
        }
        protected void lbComponentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOperation();
        }
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSubCategory();
            BindDesCategory();
            BindDownIdInfo();
            hdfValue.Value = "NotOk";
        }

        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDesCategory();
            BindDownIdInfo();
            hdfValue.Value = "NotOk";
        }

        protected void ddlDesCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDownIdInfo();
            hdfValue.Value = "NotOk";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<TreeChartEntitySeries> getTreeData(string plantID, string Groupid, string machineId, string startFromDate, string startToDate, string ComponentID, string Operation, string Category, string SubCategory, string DesCategory, string downReasonsstring, string RejCode)
        {
            List<TreeChartEntitySeries> treeSeriesList = new List<TreeChartEntitySeries>();
            try
            {
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;
                fromDate = Util.GetDateTime(startFromDate);
                toDate = Util.GetDateTime(startToDate);
                plantID = plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID;
                Groupid = Groupid.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Groupid;
                machineId = machineId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineId;
                Operation = Operation.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Operation;
                Category = Category.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Category;
                SubCategory = SubCategory.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : SubCategory;
                DesCategory = DesCategory.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : DesCategory;
                List<TreeDataEntity> list = DataBaseAccess.getTreeChartData(fromDate.Date, toDate.Date, plantID, Groupid, machineId, getSplittedInputValue(ComponentID), Operation, Category, SubCategory, DesCategory, RejCode);
                HttpContext.Current.Session["GlobeTreeData"] = list;
                
                if (list.Count > 0)
                {
   
                    var distCategory = list.Select(k => new { k.Catogory, k.CatogoryQty }).Distinct().ToList();
                    if (distCategory != null)
                    {
                        foreach(var catagory in distCategory)
                        {
                            TreeChartEntitySeries treeSeries = new TreeChartEntitySeries();
                            treeSeries.nodes = new List<node>();
                            treeSeries.levels = new List<levels>();
                            treeSeries.data = new List<string[]>();
                            string[] data = new string[2];
                            List<int> listSubCatCount = new List<int>();
                            treeSeries.levels.Add(new levels
                            {
                                level = 0,
                                color = "#dd99ff"
                            });
                            //get distinct component id for category
                            var distComponent = list.Where(k => k.Catogory == catagory.Catogory).Select(k => new { k.Component, k.ComponentQty }).Distinct().ToList();
                            if (distComponent != null)
                            {
                                treeSeries.Width = distComponent.Count.ToString();
                                foreach (var component in distComponent)
                                {
                                    data = new string[2];
                                    data[0] = catagory.Catogory;
                                    data[1] = component.Component;
                                    treeSeries.data.Add(data);
                                    treeSeries.nodes.Add(new node
                                    {
                                        id = component.Component,
                                        name = component.Component,
                                        title = component.ComponentQty,
                                        layout = "hanging",
                                    });

                                    //get distinct sub category id for category and component
                                    var distSubCategory = list.Where(k => k.Catogory == catagory.Catogory && k.Component == component.Component).Select(k => new { k.SubCatogory, k.SubCatogoryQty }).Distinct().ToList();
                                   
                                    if (distSubCategory != null)
                                    {
                                        listSubCatCount.Add(distSubCategory.Count);
                                        int subCatCount = 0;
                                        StyleData style = new StyleData();
                                        style.textDecoration = "underline";
                                        DataLabels dataLabels = new DataLabels();
                                        dataLabels.style = style;
                                        foreach (var subcategory in distSubCategory)
                                        {

                                            data = new string[2];
                                            data[0] = component.Component;
                                            data[1] = subcategory.SubCatogory+ component.Component;
                                            treeSeries.data.Add(data);
                                            treeSeries.nodes.Add(new node
                                            {
                                                id = subcategory.SubCatogory + component.Component,
                                                name = subcategory.SubCatogory,
                                                title = subcategory.SubCatogoryQty,
                                                layout = "normal",
                                                Categoty = catagory.Catogory,
                                                Component = component.Component,
                                               dataLabels= dataLabels
                                            });
                                            subCatCount++;
                                        }
                                        treeSeries.levels.Add(new levels
                                        {
                                            level = 2,
                                            color = "#6eaeeb"
                                        });
                                    }
                                }
                                treeSeries.levels.Add(new levels
                                {
                                    level = 1,
                                    color = "#e0e004"
                                });
                            }
                            treeSeries.nodes.Add(new node
                            {
                                id = catagory.Catogory,
                                name = catagory.Catogory,
                                title = catagory.CatogoryQty,
                                layout = "normal",
                            });
                            treeSeries.Hieght = listSubCatCount.Max().ToString();
                            treeSeries.title = catagory.Catogory;
                            treeSeriesList.Add(treeSeries);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return treeSeriesList;
        }
        private static string getSplittedInputValue(string value)
        {
            string result = "";
            try
            {
                var splitary = value.Split(',');
                foreach(var data in splitary)
                {
                    if(result=="")
                    {
                        result += "'" + data + "'";
                    }
                    else
                    {
                        result += ",'" + data + "'";
                    }
                }
            }
            catch(Exception ex)
            { }
            return result;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<TreeChartEntitySeries> getDescCodeTreeData(string Category, string Component, string SubCategory,string SubCategoryQty)
        {
            List<TreeChartEntitySeries> treeSeriesList = new List<TreeChartEntitySeries>();
            try
            {
                if (HttpContext.Current.Session["GlobeTreeData"] != null)
                {
                    List<TreeDataEntity> list= HttpContext.Current.Session["GlobeTreeData"] as List<TreeDataEntity>;
                    int count = 0;
                    if (list.Count > 0)
                    {
                        TreeChartEntitySeries treeSeries = new TreeChartEntitySeries();
                        treeSeries.nodes = new List<node>();
                        treeSeries.levels = new List<levels>();
                        treeSeries.data = new List<string[]>();
                        string[] data = new string[2];
                        List<int> listRejCodeCount = new List<int>();
                        treeSeries.levels.Add(new levels
                        {
                            level = 0,
                            color = "#6eaeeb"
                        });
                        var distDescription = list.Where(k=>k.Catogory==Category && k.Component==Component && k.SubCatogory==SubCategory).Select(k => new { k.Description, k.DescriptionQty }).Distinct().ToList();
                        if (distDescription != null)
                        {
                            treeSeries.Width = distDescription.Count.ToString();
                            foreach (var description in distDescription)
                            {
                                data = new string[2];
                                data[0] = SubCategory;
                                data[1] = description.Description;
                                treeSeries.data.Add(data);
                                treeSeries.nodes.Add(new node
                                {
                                    id = description.Description,
                                    name = description.Description,
                                    title = description.DescriptionQty,
                                    layout = "hanging",
                                });

                                var distCode = list.Where(k => k.Catogory == Category && k.Component == Component && k.SubCatogory == SubCategory && k.Description == description.Description).Select(k => new { k.Code, k.CodeQty }).Distinct().ToList();
                                
                                if (distCode != null)
                                {
                                    listRejCodeCount.Add(distCode.Count);
                                    foreach (var code in distCode)
                                    {
                                        data = new string[2];
                                        data[0] = description.Description;
                                        data[1] = code.Code+ count;
                                        treeSeries.data.Add(data);
                                        treeSeries.nodes.Add(new node
                                        {
                                            id = code.Code + count,
                                            name = code.Code,
                                            title = code.CodeQty,
                                            layout = "normal",
                                        });
                                        count++;
                                    }
                                }
                               
                               
                            }
                            treeSeries.levels.Add(new levels
                            {
                                level = 1,
                                color = "#ffa500"
                            });
                            treeSeries.levels.Add(new levels
                            {
                                level = 2,
                                color = "#a6a4a4"
                            });
                            treeSeries.nodes.Add(new node
                            {
                                id = SubCategory,
                                name = SubCategory,
                                title = SubCategoryQty,
                                layout = "normal",
                            });
                            treeSeries.title = SubCategory;
                            treeSeries.Hieght = listRejCodeCount.Max().ToString();
                            treeSeriesList.Add(treeSeries);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return treeSeriesList;
        }
    }
}