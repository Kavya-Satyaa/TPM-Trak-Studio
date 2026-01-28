using BusinessClassLibrary;
using ModelClassLibrary;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.KTASpindle;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class Component_Information : System.Web.UI.Page
    {
        public List<ColumnViewSetting> settings = null;
        public List<UserAccessModel> useAccessData = null;
        protected void Page_Init(object sender, EventArgs e)
        {

            settings = DataBaseAccess.BindSettingPage("ComponentInformation", Session["Language"] == null ? "en" : Session["Language"].ToString());
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string interfaceDataType = DataBaseAccess.GetType("ComponentInfoInterfaceIdDataType");
                hfInterfaceDataType.Value = String.IsNullOrEmpty(interfaceDataType) ? "Numeric" : interfaceDataType;

                if (Session["UserAccessData"] == null)
                    Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"].ToString());
                else
                    useAccessData = Session["UserAccessData"] as List<UserAccessModel>;

                if (useAccessData != null && !useAccessData.Where(ss => ss.Code.Equals("EnableWriteAccessForCOP", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                {
                    tdOperations.Visible = false;
                    masterFS.Visible = false;
                }
                else
                {
                    tdOperations.Visible = true;
                    masterFS.Visible = true;
                }


                ddlSortOrderName.SelectedValue = "InterfaceID";
                ddlSortOrderType.SelectedValue = "desc";
                BindComponentInformation();
            }
        }

        private void BindComponentInformation()
        {
            try
            {
                trWeight.Visible = false;
                trPartType.Visible = false;
                if (WebConfigurationManager.AppSettings["PittiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    trPartType.Visible = true;
                }
                if (WebConfigurationManager.AppSettings["CumiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase) || WebConfigurationManager.AppSettings["RNGupta"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    trWeight.Visible = true;
                }
                List<string> Customer = new List<string>();
                List<string> PartFamily = new List<string>();
                List<componentInformation> list = new List<componentInformation>();
                Customer = DataBaseAccess.GetAllCustomers();

                Session["Customers"] = Customer;
                ddlCustomer.DataSource = Customer;
                ddlCustomer.DataBind();


                DataTable dt = DataBaseAccess.GetComponentDetails("", "", "ViewComponentInfo");
                dt.Columns.Add("InterfaceIDInt", typeof(int));
                if (dt.Rows.Count > 0)
                {
                    if (ddlSearchDropdown.SelectedValue.Equals("componentid", StringComparison.OrdinalIgnoreCase))
                    {
                        if (dt.AsEnumerable().Where(x => x.Field<string>("componentid").ToLower().Contains(txtComponentSearch.Text.Trim().ToLower())).Count() > 0)
                            dt = dt.AsEnumerable().Where(x => x.Field<string>("componentid").ToLower().Contains(txtComponentSearch.Text.Trim().ToLower())).CopyToDataTable();
                        else
                            dt = new DataTable();
                    }
                    else if (ddlSearchDropdown.SelectedValue.Equals("InterfaceID", StringComparison.OrdinalIgnoreCase))
                    {
                        if (dt.AsEnumerable().Where(x => x.Field<dynamic>("InterfaceID").ToLower().Contains(txtComponentSearch.Text.Trim().ToLower())).Count() > 0)
                            dt = dt.AsEnumerable().Where(x => x.Field<dynamic>("InterfaceID").ToLower().Contains(txtComponentSearch.Text.Trim().ToLower())).CopyToDataTable();
                        else
                            dt = new DataTable();
                    }
                    else if (ddlSearchDropdown.SelectedValue.Equals("Description", StringComparison.OrdinalIgnoreCase))
                    {
                        if (dt.AsEnumerable().Where(x => x.Field<dynamic>("description").ToLower().Contains(txtComponentSearch.Text.Trim().ToLower())).Count() > 0)
                            dt = dt.AsEnumerable().Where(x => x.Field<dynamic>("description").ToLower().Contains(txtComponentSearch.Text.Trim().ToLower())).CopyToDataTable();
                        else
                            dt = new DataTable();
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (ddlSortOrderName.SelectedValue.Equals("InterfaceID", StringComparison.OrdinalIgnoreCase))
                        {
                            if (hfInterfaceDataType.Value.Equals("Numeric", StringComparison.OrdinalIgnoreCase))
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    try
                                    {
                                        row["InterfaceIDInt"] = string.IsNullOrEmpty(row["InterfaceID"].ToString()) ? 0 : Convert.ToInt64(row["InterfaceID"].ToString());
                                    }
                                    catch (Exception ex)
                                    {
                                        continue;
                                    }
                                }

                                if (ddlSortOrderType.SelectedValue.Equals("Asc", StringComparison.OrdinalIgnoreCase))
                                    dt = dt.AsEnumerable().OrderBy(x => x["InterfaceIDInt"]).CopyToDataTable();
                                else

                                    dt = dt.AsEnumerable().OrderByDescending(x => x["InterfaceIDInt"]).CopyToDataTable();
                            }
                            else
                            {
                                if (ddlSortOrderType.SelectedValue.Equals("Asc", StringComparison.OrdinalIgnoreCase))
                                    dt = dt.AsEnumerable().OrderBy(x => x.Field<string>("InterfaceID")).CopyToDataTable();
                                else
                                    dt = dt.AsEnumerable().OrderByDescending(x => x.Field<string>("InterfaceID")).CopyToDataTable();
                            }
                        }

                        else if (ddlSortOrderName.SelectedValue.Equals("ComponentID", StringComparison.OrdinalIgnoreCase))
                        {
                            if (ddlSortOrderType.SelectedValue.Equals("Asc", StringComparison.OrdinalIgnoreCase))
                                dt = dt.AsEnumerable().OrderBy(x => x.Field<string>("componentid")).CopyToDataTable();
                            else
                                dt = dt.AsEnumerable().OrderByDescending(x => x.Field<string>("componentid")).CopyToDataTable();
                        }

                        if (WebConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("0", StringComparison.OrdinalIgnoreCase))
                        {
                            var column = dt.Columns.Cast<DataColumn>().Where(k => k.ColumnName == "PartFamily").FirstOrDefault();
                            if (column == null)
                            {
                                dt.Columns.Add("PartFamily", typeof(string));
                            }
                        }
                        var columnResult = dt.Columns.Cast<DataColumn>().Where(k => k.ColumnName == "PartType").FirstOrDefault();
                        if (columnResult == null)
                        {
                            dt.Columns.Add("PartType", typeof(string));
                            dt.AsEnumerable().ToList<DataRow>().ForEach(x =>
                            {
                                x["PartType"] = string.Empty;
                            });
                        }

                        dt.Columns.Add("PartFamilyIsVisible", typeof(bool));
                        dt.Columns.Add("WeightColumnVisible", typeof(bool));
                        dt.Columns.Add("PartTypeIsVisible", typeof(bool));

                        dt.AsEnumerable().ToList<DataRow>().ForEach(x =>
                        {
                            x["PartFamilyIsVisible"] = false;
                            x["WeightColumnVisible"] = false;
                            x["PartTypeIsVisible"] = WebConfigurationManager.AppSettings["PittiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase) ? true : false;
                        });

                        if (WebConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            hdnKTAFlag.Value = "1";
                            dt.AsEnumerable().ToList<DataRow>().ForEach(x =>
                            {
                                x["PartFamilyIsVisible"] = true;
                            });

                            PartFamily = DataBaseAccess.GetallPartFamilies();
                            Session["PartFamily"] = PartFamily;
                            ddlPartFamily.DataSource = PartFamily;
                            ddlPartFamily.DataBind();

                        }

                        if (WebConfigurationManager.AppSettings["CumiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase) || WebConfigurationManager.AppSettings["RNGupta"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            dt.AsEnumerable().ToList<DataRow>().ForEach(x =>
                            {
                                x["WeightColumnVisible"] = true;
                            });
                        }

                        //foreach (DataRow row in dt.Rows)
                        //{
                        //    componentInformation entity = new componentInformation();

                        //    entity.Componentid = row["componentid"].ToString();
                        //    entity.Description = row["description"].ToString();
                        //    entity.Interfaceid = row["InterfaceID"].ToString();
                        //    entity.Customer = row["customerid"].ToString();
                        //    entity.PartFamily = row["PartFamily"].ToString();
                        //    entity.Weight = row["InputWeight"].ToString();

                        //    list.Add(entity);
                        //}
                        lvComponentInfo.DataSource = dt;
                        lvComponentInfo.DataBind();

                        if (WebConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            lvComponentInfo.FindControl("thPartFamilyColumn").Visible = true;
                        }
                        else
                        {
                            lvComponentInfo.FindControl("thPartFamilyColumn").Visible = false;
                        }
                        if (WebConfigurationManager.AppSettings["CumiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase) || WebConfigurationManager.AppSettings["RNGupta"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            lvComponentInfo.FindControl("thWeightColumn").Visible = true;
                        }
                        else
                        {
                            lvComponentInfo.FindControl("thWeightColumn").Visible = false;
                        }
                        if (WebConfigurationManager.AppSettings["PittiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            lvComponentInfo.FindControl("thPartTypeColumn").Visible = true;
                        }
                        else
                        {
                            lvComponentInfo.FindControl("thPartTypeColumn").Visible = false;
                        }

                        if (dt.Rows.Count <= 500)
                            lvComponentInfo.FindControl("DataPager1").Visible = false;
                        else
                            lvComponentInfo.FindControl("DataPager1").Visible = true;
                    }
                    else
                    {
                        lvComponentInfo.DataSource = null;
                        lvComponentInfo.DataBind();
                    }
                }
                else
                {
                    lvComponentInfo.DataSource = null;
                    lvComponentInfo.DataBind();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void lvComponentInfo_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (Session["Customers"] != null)
                {

                    if (e.Item != null)
                    {
                        var dataItem = e.Item as ListViewDataItem;

                        var dropdown = (dataItem.FindControl("ddlCustomer") as DropDownList);

                        dropdown.DataSource = Session["Customers"] as List<string>;
                        dropdown.DataBind();

                        dropdown.SelectedValue = (dataItem.FindControl("hdfCustomer") as HiddenField).Value.ToString();
                    }
                }
                if (Session["PartFamily"] != null)
                {
                    if (e.Item != null)
                    {
                        var dataItem = e.Item as ListViewDataItem;
                        var dropdown = (dataItem.FindControl("ddlPartFamily") as DropDownList);
                        dropdown.DataSource = Session["PartFamily"] as List<string>;
                        dropdown.DataBind();

                        dropdown.SelectedValue = (dataItem.FindControl("hdfPartFamily") as HiddenField).Value.ToString();
                    }
                }
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    if (e.Item != null)
                    {
                        var dataItem = e.Item as ListViewDataItem;
                        var dropdown = (dataItem.FindControl("ddlPartType") as DropDownList);
                        if (dropdown != null)
                        {
                            string value = (dataItem.FindControl("hdnPartType") as HiddenField).Value.ToString();
                            if (value == "")
                            {
                                dropdown.Items.Insert(0, "");
                            }
                            else
                            {
                                HelperClassGeneric.setDropdownValue(dropdown, value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string msg = "";
            try
            {
                foreach (ListViewDataItem item in lvComponentInfo.Items)
                {
                    if (item.ItemType == ListViewItemType.DataItem)
                    {
                        if ((item.FindControl("hdfInterface") as HiddenField).Value.ToString().Equals("update", StringComparison.OrdinalIgnoreCase))
                        {
                            string componetid = (item.FindControl("lblComp") as Label).Text.Trim();
                            string interfaceid = (item.FindControl("txtIntefaceID") as TextBox).Text.Trim();
                            string customer = (item.FindControl("ddlCustomer") as DropDownList).SelectedValue.ToString();
                            string description = (item.FindControl("txtDescription") as TextBox).Text.Trim();
                            string PartFamily = (item.FindControl("ddlPartFamily") as DropDownList).SelectedValue.ToString();
                            string PartType = (item.FindControl("ddlPartType") as DropDownList).SelectedValue.ToString();
                            string Weight = (item.FindControl("txtWeight") as TextBox).Text.Trim();
                            bool isSucessFailure = false;
                            DataBaseAccess.InsertOrUpdateComponentIdDetails(componetid, interfaceid, customer, description, "", Weight, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "InsertCompInfo", "", "", PartFamily, "", "", false, PartType, "", out isSucessFailure);

                            if (isSucessFailure)
                            {
                                msg = "Records updated successfully.";
                                msg = HttpContext.GetLocalResourceObject("~/ComponentInformation.aspx", "Recordsupdatedsuccessfully").ToString();
                            }
                            else
                            {
                                msg = HttpContext.GetLocalResourceObject("~/ComponentInformation.aspx", "Pleaseenterrequiredvaluestoupdate").ToString();
                            }
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenSuccessToastr", "successMsg('Records Saved Succefully.');", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                HelperClass.openWarningModal(this, ex.Message.ToString());
            }
            finally
            {
                BindComponentInformation();
            }
        }

        protected void lvComponentInfo_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            try
            {
                DataPager dp = (DataPager)lvComponentInfo.FindControl("DataPager1");

                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                BindComponentInformation();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnNewSave_Click(object sender, EventArgs e)
        {
            string res = "";
            bool IsSuccess = false;
            try
            {
                if (txtComponentID.Text == "" || txtInterfaceID.Text == "")
                {
                    res = "Component ID/Interface ID Cant be empty";
                    HelperClass.openWarningModal(this, res);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenNewentryscreen", "OpenNewEntryScreen();", true);
                    return;
                }
                DataBaseAccess.InsertOrUpdateComponentIdDetails(txtComponentID.Text.Trim(), txtInterfaceID.Text.Trim(), ddlCustomer.SelectedValue, txtDescription.Text.Trim(), "", txtNewWeight.Text.Trim(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "InsertCompInfo", "", "", ddlPartFamily.SelectedValue, "", "", false, ddlPartType.SelectedValue, "", out IsSuccess);
                if (IsSuccess)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenSuccessToastr", "successMsg('Component Created.');", true);
                }
                txtComponentID.Text = "";
                txtInterfaceID.Text = "";
                txtDescription.Text = "";
                txtNewWeight.Text = "";
                if (ddlCustomer.Items.Count > 0)
                {
                    ddlCustomer.SelectedValue = ddlCustomer.Items[0].Value;
                }
                BindComponentInformation();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                res = ex.Message;

                if (!string.IsNullOrEmpty(res))
                {
                    HelperClass.openWarningModal(this, res);
                }
            }
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            string errorMsg = "";
            try
            {
                ListViewDataItem ditem = (sender as Button).NamingContainer as ListViewDataItem;
                string componetid = (ditem.FindControl("lblComp") as Label).Text.Trim();
                string interfaceid = (ditem.FindControl("txtIntefaceID") as TextBox).Text.Trim();

                DataBaseAccess.DeleteComponentOperationDetails("DeleteCompInfo", componetid, interfaceid, "", "", "", out IsSuccess);
                if (IsSuccess)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "successtoaster", "successMsg('Successfully Deleted.');", true);
                }
                BindComponentInformation();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                errorMsg = ex.Message;

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    HelperClass.openWarningModal(this, errorMsg);
                }
            }
        }

        protected void BtnView_Click(object sender, EventArgs e)
        {
            BindComponentInformation();
        }



        #region "Binding CustomerList"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<componentInformation> GetAllCustomersList()
        {
            List<componentInformation> list = new List<componentInformation>();
            componentInformation complst = new componentInformation();
            if (System.Web.Configuration.WebConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
            {
                DataTable pdt = DBAccess.GetPartFamilyDetails("");
                List<string> PartFamily = pdt.AsEnumerable().Select(x => x.Field<string>("PartFamily")).Distinct().ToList();
                PartFamily.Insert(0, "None");
                complst.listPart = PartFamily;
            }
            complst.ListCustomer = DataBaseAccess.GetAllCustomers();
            list.Add(complst);
            return list;
        }
        #endregion

        #region "Binding Componentinfo"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<componentInformation> Componentinfo(string sortordername, string sortordertype)
        {
            List<componentInformation> componentgrddata = new List<componentInformation>();
            try
            {
                List<string> allCustomers = DataBaseAccess.GetAllCustomers();
                List<string> PartFamily = new List<string>();
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    DataTable pdt = DBAccess.GetPartFamilyDetails("");
                    PartFamily = pdt.AsEnumerable().Select(x => x.Field<string>("PartFamily")).Distinct().ToList();
                    PartFamily.Insert(0, "None");
                }

                //DataTable dtCopy = new DataTable();
                //dtCopy = DataBaseAccess.GetComponentDetails("", "", "ViewComponentInfo");

                // DataTable dt = new DataTable();
                DataTable dt = DataBaseAccess.GetComponentDetails("", "", "ViewComponentInfo");
                //if (dtCopy.Rows.Count > 0)
                //{
                //    //dt = dtCopy.Clone();
                //    DataColumn dtColumn;
                //    dtColumn = new DataColumn();
                //    dtColumn.DataType = typeof(string);
                //    dtColumn.ColumnName = "componentid";
                //    dt.Columns.Add(dtColumn);

                //    dtColumn = new DataColumn();
                //    dtColumn.DataType = typeof(long);
                //    dtColumn.ColumnName = "InterfaceID";
                //    dt.Columns.Add(dtColumn);

                //    dtColumn = new DataColumn();
                //    dtColumn.DataType = typeof(string);
                //    dtColumn.ColumnName = "customerid";
                //    dt.Columns.Add(dtColumn);

                //    dtColumn = new DataColumn();
                //    dtColumn.DataType = typeof(string);
                //    dtColumn.ColumnName = "description";
                //    dt.Columns.Add(dtColumn);

                //    //dt.Columns["InterfaceID"].DataType = typeof(Int64);
                //    //dt.Columns["InterfaceID"].MaxLength;
                //    //foreach (DataRow row in dtCopy.Rows)
                //    //{
                //    //    dt.ImportRow(row);
                //    //}
                //    for (int i = 0; i < dtCopy.Rows.Count; i++)
                //    {
                //        DataRow rows = dt.NewRow();
                //        rows["componentid"] = dtCopy.Rows[i]["componentid"].ToString();
                //        if (dtCopy.Rows[i]["InterfaceID"].ToString() != "")
                //        {
                //            rows["InterfaceID"] = Convert.ToInt64(dtCopy.Rows[i]["InterfaceID"].ToString().Trim());
                //        }
                //        rows["customerid"] = dtCopy.Rows[i]["customerid"].ToString();
                //        rows["description"] = dtCopy.Rows[i]["description"].ToString();
                //        dt.Rows.Add(rows);
                //    }
                //    if (string.IsNullOrEmpty(sortordername))
                //    {
                //        sortordername = "componentid";
                //    }
                //    if (string.IsNullOrEmpty(sortordertype))
                //    {
                //        sortordertype = "asc";
                //    }
                //    if (sortordername != "")
                //    {
                //        if (string.Equals(sortordertype, "desc", StringComparison.OrdinalIgnoreCase))
                //        {
                //            dt = dt.AsEnumerable().OrderBy(k => k[sortordername]).Reverse().CopyToDataTable();
                //        }
                //        else
                //        {
                //            dt = dt.AsEnumerable().OrderBy(k => k[sortordername]).CopyToDataTable();
                //        }
                //    }
                //}
                string interfaceidType = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    componentInformation component = new componentInformation();
                    component.Componentid = dt.Rows[i]["componentid"].ToString();
                    component.Interfaceid = dt.Rows[i]["InterfaceID"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[i]["InterfaceID"].ToString()))
                    {
                        try
                        {
                            component.InterfaceidInInt = Convert.ToInt64(dt.Rows[i]["InterfaceID"].ToString());
                        }
                        catch (Exception ex)
                        {
                            interfaceidType = "string";
                        }
                    }
                    component.Customer = dt.Rows[i]["customerid"].ToString();
                    component.Description = dt.Rows[i]["description"].ToString();
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["CumiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase) || WebConfigurationManager.AppSettings["RNGupta"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        component.Weight = dt.Rows[i]["InputWeight"].ToString();
                    }

                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        component.PartFamily = dt.Rows[i]["PartFamily"].ToString();
                    }

                    componentgrddata.Add(component);
                }
                foreach (var item in componentgrddata)
                {
                    item.ListCustomer = allCustomers;
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        item.listPart = PartFamily;
                    }
                }

                if (string.IsNullOrEmpty(sortordername))
                {
                    sortordername = "componentid";
                }
                if (string.IsNullOrEmpty(sortordertype))
                {
                    sortordertype = "asc";
                }
                if (string.Equals(sortordername, "componentid", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(sortordertype, "desc", StringComparison.OrdinalIgnoreCase))
                    {
                        componentgrddata = componentgrddata.OrderByDescending(k => k.Componentid).ToList();
                    }
                    else
                    {
                        componentgrddata = componentgrddata.OrderBy(k => k.Componentid).ToList();
                    }
                }
                else if (string.Equals(sortordername, "InterfaceID", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(sortordertype, "desc", StringComparison.OrdinalIgnoreCase))
                    {
                        if (interfaceidType == "string")
                        {
                            componentgrddata = componentgrddata.OrderByDescending(k => k.Interfaceid).ToList();
                        }
                        else
                        {
                            componentgrddata = componentgrddata.OrderByDescending(k => k.InterfaceidInInt).ToList();
                        }
                    }
                    else
                    {
                        if (interfaceidType == "string")
                        {
                            componentgrddata = componentgrddata.OrderBy(k => k.Interfaceid).ToList();
                        }
                        else
                        {
                            componentgrddata = componentgrddata.OrderBy(k => k.InterfaceidInInt).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return componentgrddata;
        }
        #endregion

        #region "UpdateComponentInfo"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string UpdateComponentInfo(componentInformation model)
        {
            string msg = HttpContext.GetLocalResourceObject("~/ComponentInformation.aspx", "Recordsnotupdatedsuccessfully").ToString();

            bool isSuccessFailure = false;
            try
            {
                //string param = "Update";
                foreach (var item in model.ListComponet)
                {
                    DataBaseAccess.InsertOrUpdateComponentIdDetails(item.Componentid.Trim(), item.Interfaceid.ToString(), item.Customer.ToString(),
                        item.Description.ToString(), "", item.Weight, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "InsertCompInfo", "", "", item.PartFamily, "", "", false, "", "", out isSuccessFailure);
                }
                if (isSuccessFailure == true)
                {
                    msg = "Records updated successfully.";
                    msg = HttpContext.GetLocalResourceObject("~/ComponentInformation.aspx", "Recordsupdatedsuccessfully").ToString();
                }
                else if (isSuccessFailure == false)
                {
                    msg = HttpContext.GetLocalResourceObject("~/ComponentInformation.aspx", "Pleaseenterrequiredvaluestoupdate").ToString();
                }
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            return msg;

        }
        #endregion

        #region "DeleteComponentInfo"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DeleteComponentInfo(componentInformation model)
        {
            string msg = HttpContext.GetLocalResourceObject("~/ComponentInformation.aspx", "Recordsnotdeleted").ToString();
            bool isSuccessOrFailure = false;
            try
            {
                if (model.ListComponet.Count >= 1)
                {
                    foreach (var item in model.ListComponet)
                    {
                        DataBaseAccess.DeleteComponentOperationDetails("DeleteCompInfo", item.Componentid.Trim(), item.Interfaceid.ToString(), "", "", "", out isSuccessOrFailure);
                    }
                    Componentinfo("", "");
                    msg = HttpContext.GetLocalResourceObject("~/ComponentInformation.aspx", "Recordsnotdeleted").ToString();
                }
                else
                {
                    msg = HttpContext.GetLocalResourceObject("~/ComponentInformation.aspx", "Rowisnotselected").ToString();
                }
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
            return msg;

        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string getComponentIDDeleteStatus(string componentid, string interfaceid)
        {
            string result = "";
            try
            {
                int deleteResult = DataBaseAccess.deleteComponentDetails(componentid, interfaceid);
                if (deleteResult > 0)
                {
                    result = "success";
                }
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
            return result;

        }
        #endregion
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool isCustomerPageEnabled(string customerPageKeyName)
        {
            bool isAMPageEnabled = false;
            try
            {
                if (System.Web.Configuration.WebConfigurationManager.AppSettings[customerPageKeyName].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    isAMPageEnabled = true;
                }
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
            return isAMPageEnabled;
        }

        static string appPath = HttpContext.Current.Server.MapPath("~/Reports");
        public static string GetReportPath(string reportName)
        {
            string src;
            src = Path.Combine(appPath, "TPMTrakReport", reportName);
            return src;
        }
        public static string SafeFileName(string name)
        {
            StringBuilder str = new StringBuilder(name);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.ToString();
        }
        private static void DownloadFile(string filename, byte[] bytearray)
        {

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Path.GetFileName(filename) + "\"");
            HttpContext.Current.Response.OutputStream.Write(bytearray, 0, bytearray.Length);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }
        //protected void btnexport_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string templatefile = string.Empty;
        //        string Filename = "ComponentOperationReport.xlsx";
        //        DataTable dt = new DataTable();
        //        DataTable dtCopy = DataBaseAccess.getComponentOperationDetails(search.Value);
        //        string Source = string.Empty;
        //        Source = GetReportPath(Filename);
        //        string Template = string.Empty;
        //        Template = "ComponentOperationReport_" + DateTime.Now + ".xlsx";
        //        string destination = string.Empty;
        //        destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
        //        if (!File.Exists(Source))
        //        {
        //            Logger.WriteDebugLog("Component Operation Report- \n " + Source);
        //        }
        //        if (dtCopy.Rows.Count > 0)
        //        {
        //            FileInfo newFile = new FileInfo(Source);
        //            ExcelPackage Excel = new ExcelPackage(newFile, true);
        //            var exelworksheet = Excel.Workbook.Worksheets["Sheet1"];

        //            DataColumn dtColumn;
        //            dtColumn = new DataColumn();
        //            dtColumn.DataType = typeof(string);
        //            dtColumn.ColumnName = "machineid";
        //            dt.Columns.Add(dtColumn);

        //            dtColumn = new DataColumn();
        //            dtColumn.DataType = typeof(long);
        //            dtColumn.ColumnName = "CompInterfaceID";
        //            dt.Columns.Add(dtColumn);


        //            dtColumn = new DataColumn();
        //            dtColumn.DataType = typeof(string);
        //            dtColumn.ColumnName = "componentid";
        //            dt.Columns.Add(dtColumn);

        //            dtColumn = new DataColumn();
        //            dtColumn.DataType = typeof(string);
        //            dtColumn.ColumnName = "CompDesc";
        //            dt.Columns.Add(dtColumn);

        //            dtColumn = new DataColumn();
        //            dtColumn.DataType = typeof(string);
        //            dtColumn.ColumnName = "OpnInterfaceID";
        //            dt.Columns.Add(dtColumn);

        //            dtColumn = new DataColumn();
        //            dtColumn.DataType = typeof(string);
        //            dtColumn.ColumnName = "operationno";
        //            dt.Columns.Add(dtColumn);

        //            dtColumn = new DataColumn();
        //            dtColumn.DataType = typeof(string);
        //            dtColumn.ColumnName = "OpnDesc";
        //            dt.Columns.Add(dtColumn);

        //            dtColumn = new DataColumn();
        //            dtColumn.DataType = typeof(Double);
        //            dtColumn.ColumnName = "machiningtime";
        //            dt.Columns.Add(dtColumn);

        //            dtColumn = new DataColumn();
        //            dtColumn.DataType = typeof(Double);
        //            dtColumn.ColumnName = "cycletime";
        //            dt.Columns.Add(dtColumn);

        //            dtColumn = new DataColumn();
        //            dtColumn.DataType = typeof(Double);
        //            dtColumn.ColumnName = "loadunload";
        //            dt.Columns.Add(dtColumn);

        //            dtColumn = new DataColumn();
        //            dtColumn.DataType = typeof(string);
        //            dtColumn.ColumnName = "CompInterfaceIDInString";
        //            dt.Columns.Add(dtColumn);
        //            //dt = dtCopy.Clone();
        //            //dt.Columns["CompInterfaceID"].DataType = typeof(Int64);
        //            //foreach (DataRow row in dtCopy.Rows)
        //            //{
        //            //    dt.ImportRow(row);
        //            //}
        //            string interfaceIDType = "";
        //            for (int i = 0; i < dtCopy.Rows.Count; i++)
        //            {
        //                DataRow rows = dt.NewRow();
        //                rows["machineid"] = dtCopy.Rows[i]["machineid"].ToString();

        //                if (dtCopy.Rows[i]["CompInterfaceID"].ToString() != "")
        //                {
        //                    rows["CompInterfaceIDInString"] = dtCopy.Rows[i]["CompInterfaceID"].ToString();
        //                    try
        //                    {
        //                        rows["CompInterfaceID"] = Convert.ToInt64(dtCopy.Rows[i]["CompInterfaceID"].ToString().Trim());
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        interfaceIDType = "string";
        //                    }

        //                }
        //                rows["componentid"] = dtCopy.Rows[i]["componentid"].ToString();
        //                rows["CompDesc"] = dtCopy.Rows[i]["CompDesc"].ToString();
        //                rows["OpnInterfaceID"] = dtCopy.Rows[i]["OpnInterfaceID"].ToString();
        //                rows["operationno"] = dtCopy.Rows[i]["operationno"].ToString();
        //                rows["OpnDesc"] = dtCopy.Rows[i]["OpnDesc"].ToString();
        //                if (dtCopy.Rows[i]["machiningtime"].ToString() != "")
        //                {
        //                    rows["machiningtime"] = Convert.ToDouble(dtCopy.Rows[i]["machiningtime"].ToString().Trim());
        //                }
        //                if (dtCopy.Rows[i]["cycletime"].ToString() != "")
        //                {
        //                    rows["cycletime"] = Convert.ToDouble(dtCopy.Rows[i]["cycletime"].ToString().Trim());
        //                }
        //                if (dtCopy.Rows[i]["loadunload"].ToString() != "")
        //                {
        //                    rows["loadunload"] = Convert.ToDouble(dtCopy.Rows[i]["loadunload"].ToString().Trim());
        //                }
        //                dt.Rows.Add(rows);
        //            }
        //            string sortordername = ddlSortOrderName.SelectedValue;
        //            string sortordertype = ddlSortOrderType.SelectedValue;
        //            if (string.IsNullOrEmpty(ddlSortOrderName.SelectedValue))
        //            {
        //                sortordername = "componentid";
        //            }
        //            if (string.Equals(sortordername, "InterfaceID", StringComparison.OrdinalIgnoreCase))
        //            {
        //                if (interfaceIDType == "string")
        //                {
        //                    sortordername = "CompInterfaceIDInString";
        //                }
        //                else
        //                {
        //                    sortordername = "CompInterfaceID";
        //                }
        //            }
        //            if (string.IsNullOrEmpty(sortordertype))
        //            {
        //                sortordertype = "asc";
        //            }
        //            if (sortordername != "")
        //            {
        //                if (string.Equals(sortordertype, "desc", StringComparison.OrdinalIgnoreCase))
        //                {
        //                    dt = dt.AsEnumerable().OrderBy(k => k[sortordername]).Reverse().CopyToDataTable();
        //                }
        //                else
        //                {
        //                    dt = dt.AsEnumerable().OrderBy(k => k[sortordername]).CopyToDataTable();
        //                }
        //            }

        //            int rowCount = 3;
        //            int cellCount = 1;
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                cellCount = 1;
        //                exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["machineid"].ToString();
        //                cellCount++;
        //                exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["CompInterfaceIDInString"].ToString();
        //                cellCount++;
        //                exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["componentid"].ToString();
        //                cellCount++;
        //                exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["CompDesc"].ToString();
        //                cellCount++;
        //                exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["OpnInterfaceID"].ToString();
        //                cellCount++;
        //                exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["operationno"].ToString();
        //                cellCount++;
        //                exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["OpnDesc"].ToString();
        //                cellCount++;
        //                exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["machiningtime"].ToString();
        //                cellCount++;
        //                exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["cycletime"].ToString();
        //                cellCount++;
        //                exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["loadunload"].ToString();
        //                rowCount++;
        //            }
        //            setBorderThin(exelworksheet, 3, 1, dt.Rows.Count + 2, cellCount);
        //            for (int i = 1; i <= cellCount; i++)
        //            {
        //                exelworksheet.Column(i).AutoFit();
        //            }
        //            DownloadFile(destination, Excel.GetAsByteArray());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.Message);
        //    }
        //}
        private bool BindCompanyData()
        {
            bool CompanyStatus = DataBaseAccess.GetCompanyName();
            return CompanyStatus;
        }
        protected void btnexport_Click(object sender, EventArgs e)
        {
            try
            {
                string templatefile = string.Empty;
                string Filename = string.Empty;
                bool CompanyStatus = BindCompanyData();
                if (ConfigurationManager.AppSettings["AlliedPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    Filename = "ComponentOperationReportFormat_allied.xlsx";
                }
                else if (CompanyStatus.Equals(true))
                {
                    Filename = "ComponentOperationReportFormat2_KTA.xlsx";
                }
                else
                {
                    Filename = "ComponentOperationReportFormat2.xlsx";
                }


                DataTable dt = new DataTable();
                DataTable dtCopy = DataBaseAccess.getComponentOperationDetails(txtComponentSearch.Text.Trim());
                string Source = string.Empty;
                Source = GetReportPath(Filename);
                string Template = string.Empty;
                Template = "ComponentOperationReportFormat2_" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Component Operation Report- \n " + Source);
                }
                if (dtCopy.Rows.Count > 0)
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var exelworksheet = Excel.Workbook.Worksheets["Sheet1"];

                    DataColumn dtColumn;
                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(string);
                    dtColumn.ColumnName = "machineid";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(long);
                    dtColumn.ColumnName = "CompInterfaceID";
                    dt.Columns.Add(dtColumn);


                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(string);
                    dtColumn.ColumnName = "componentid";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(string);
                    dtColumn.ColumnName = "CompDesc";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(string);
                    dtColumn.ColumnName = "OpnInterfaceID";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(string);
                    dtColumn.ColumnName = "operationno";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(string);
                    dtColumn.ColumnName = "OpnDesc";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(Double);
                    dtColumn.ColumnName = "cycletime";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(Double);
                    dtColumn.ColumnName = "loadunload";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(Double);
                    dtColumn.ColumnName = "MachiningTime";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(string);
                    dtColumn.ColumnName = "CompInterfaceIDInString";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(Double);
                    dtColumn.ColumnName = "price";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(string);
                    dtColumn.ColumnName = "drawingno";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(int);
                    dtColumn.ColumnName = "SubOperations";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(Double);
                    dtColumn.ColumnName = "StdSetupTime";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(int);
                    dtColumn.ColumnName = "MachiningTimeThreshold";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(int);
                    dtColumn.ColumnName = "TargetPercent";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(string);
                    dtColumn.ColumnName = "customerid";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(Double);
                    dtColumn.ColumnName = "SCIThreshold";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(Double);
                    dtColumn.ColumnName = "DCLThreshold";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(int);
                    dtColumn.ColumnName = "FinishedOperation";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(Double);
                    dtColumn.ColumnName = "MinLoadUnloadThreshold";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(int);
                    dtColumn.ColumnName = "loadUnloadTimeThreshold";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(Double);
                    dtColumn.ColumnName = "IncentiveTime";
                    dt.Columns.Add(dtColumn);

                    if (CompanyStatus)
                    {
                        dtColumn = new DataColumn();
                        dtColumn.DataType = typeof(string);
                        dtColumn.ColumnName = "CellID";
                        dt.Columns.Add(dtColumn);

                        dtColumn = new DataColumn();
                        dtColumn.DataType = typeof(string);
                        dtColumn.ColumnName = "PartFamily";
                        dt.Columns.Add(dtColumn);
                    }

                    if (ConfigurationManager.AppSettings["AlliedPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        dtColumn = new DataColumn();
                        dtColumn.DataType = typeof(int);
                        dtColumn.ColumnName = "StdTestPressure";
                        dt.Columns.Add(dtColumn);

                        dtColumn = new DataColumn();
                        dtColumn.DataType = typeof(int);
                        dtColumn.ColumnName = "StdHoldingTime";
                        dt.Columns.Add(dtColumn);
                    }
                    //dt = dtCopy.Clone();
                    //dt.Columns["CompInterfaceID"].DataType = typeof(Int64);
                    //foreach (DataRow row in dtCopy.Rows)
                    //{
                    //    dt.ImportRow(row);
                    //}
                    string interfaceIDType = "";
                    for (int i = 0; i < dtCopy.Rows.Count; i++)
                    {
                        DataRow rows = dt.NewRow();
                        rows["machineid"] = dtCopy.Rows[i]["machineid"].ToString();

                        if (dtCopy.Rows[i]["CompInterfaceID"].ToString() != "")
                        {
                            rows["CompInterfaceIDInString"] = dtCopy.Rows[i]["CompInterfaceID"].ToString();
                            try
                            {
                                rows["CompInterfaceID"] = Convert.ToInt64(dtCopy.Rows[i]["CompInterfaceID"].ToString().Trim());
                            }
                            catch (Exception ex)
                            {
                                interfaceIDType = "string";
                            }

                        }
                        rows["componentid"] = dtCopy.Rows[i]["componentid"].ToString();
                        rows["CompDesc"] = dtCopy.Rows[i]["CompDesc"].ToString();
                        rows["OpnInterfaceID"] = dtCopy.Rows[i]["OpnInterfaceID"].ToString();
                        rows["operationno"] = dtCopy.Rows[i]["operationno"].ToString();
                        rows["OpnDesc"] = dtCopy.Rows[i]["OpnDesc"].ToString();
                        if (dtCopy.Rows[i]["cycletime"].ToString() != "")
                        {
                            rows["cycletime"] = Convert.ToDouble(dtCopy.Rows[i]["cycletime"].ToString().Trim());
                        }
                        if (dtCopy.Rows[i]["loadunload"].ToString() != "")
                        {
                            rows["loadunload"] = Convert.ToDouble(dtCopy.Rows[i]["loadunload"].ToString().Trim());
                        }
                        if (dtCopy.Rows[i]["MachiningTime"].ToString() != "")
                        {
                            rows["MachiningTime"] = Convert.ToDouble(dtCopy.Rows[i]["MachiningTime"].ToString().Trim());
                        }
                        if (!string.IsNullOrEmpty(dtCopy.Rows[i]["price"].ToString()))
                        {
                            rows["price"] = Convert.ToDouble(dtCopy.Rows[i]["price"].ToString().Trim());
                        }
                        rows["drawingno"] = dtCopy.Rows[i]["drawingno"].ToString();
                        if (!string.IsNullOrEmpty(dtCopy.Rows[i]["SubOperations"].ToString()))
                        {
                            rows["SubOperations"] = Convert.ToInt32(dtCopy.Rows[i]["SubOperations"].ToString().Trim());
                        }
                        if (!string.IsNullOrEmpty(dtCopy.Rows[i]["StdSetupTime"].ToString()))
                        {
                            rows["StdSetupTime"] = Convert.ToDouble(dtCopy.Rows[i]["StdSetupTime"].ToString().Trim());
                        }
                        if (!string.IsNullOrEmpty(dtCopy.Rows[i]["MachiningTimeThreshold"].ToString()))
                        {
                            rows["MachiningTimeThreshold"] = Convert.ToDouble(dtCopy.Rows[i]["MachiningTimeThreshold"].ToString().Trim());
                        }
                        if (!string.IsNullOrEmpty(dtCopy.Rows[i]["TargetPercent"].ToString()))
                        {
                            rows["TargetPercent"] = Convert.ToInt32(dtCopy.Rows[i]["TargetPercent"].ToString().Trim());
                        }
                        rows["customerid"] = dtCopy.Rows[i]["customerid"].ToString();
                        if (!string.IsNullOrEmpty(dtCopy.Rows[i]["SCIThreshold"].ToString()))
                        {
                            rows["SCIThreshold"] = Convert.ToDouble(dtCopy.Rows[i]["SCIThreshold"].ToString().Trim());
                        }
                        if (!string.IsNullOrEmpty(dtCopy.Rows[i]["DCLThreshold"].ToString()))
                        {
                            rows["DCLThreshold"] = Convert.ToDouble(dtCopy.Rows[i]["DCLThreshold"].ToString().Trim());
                        }
                        if (!string.IsNullOrEmpty(dtCopy.Rows[i]["FinishedOperation"].ToString()))
                        {
                            rows["FinishedOperation"] = Convert.ToInt32(dtCopy.Rows[i]["FinishedOperation"].ToString().Trim());
                        }
                        if (!string.IsNullOrEmpty(dtCopy.Rows[i]["MinLoadUnloadThreshold"].ToString()))
                        {
                            rows["MinLoadUnloadThreshold"] = Convert.ToDouble(dtCopy.Rows[i]["MinLoadUnloadThreshold"].ToString().Trim());
                        }
                        if (!string.IsNullOrEmpty(dtCopy.Rows[i]["loadUnloadTimeThreshold"].ToString()))
                        {
                            rows["loadUnloadTimeThreshold"] = Convert.ToInt32(dtCopy.Rows[i]["loadUnloadTimeThreshold"].ToString().Trim());
                        }
                        if (!string.IsNullOrEmpty(dtCopy.Rows[i]["IncentiveTime"].ToString()))
                        {
                            rows["IncentiveTime"] = Convert.ToInt32(dtCopy.Rows[i]["IncentiveTime"].ToString().Trim());
                        }
                        if (CompanyStatus)
                        {
                            if (!string.IsNullOrEmpty(dtCopy.Rows[i]["GroupID"].ToString()))
                            {
                                rows["CellID"] = dtCopy.Rows[i]["GroupID"].ToString();
                            }
                            if (!string.IsNullOrEmpty(dtCopy.Rows[i]["PartFamily"].ToString()))
                            {
                                rows["PartFamily"] = dtCopy.Rows[i]["PartFamily"].ToString();
                            }
                        }

                        if (ConfigurationManager.AppSettings["AlliedPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!string.IsNullOrEmpty(dtCopy.Rows[i]["StdTestPressure"].ToString()))
                            {
                                rows["StdTestPressure"] = dtCopy.Rows[i]["StdTestPressure"].ToString();
                            }
                            if (!string.IsNullOrEmpty(dtCopy.Rows[i]["StdHoldingTime"].ToString()))
                            {
                                rows["StdHoldingTime"] = dtCopy.Rows[i]["StdHoldingTime"].ToString();
                            }
                        }
                        dt.Rows.Add(rows);
                    }
                    string sortordername = ddlSortOrderName.SelectedValue;
                    string sortordertype = ddlSortOrderType.SelectedValue;
                    if (string.IsNullOrEmpty(ddlSortOrderName.SelectedValue))
                    {
                        sortordername = "componentid";
                    }
                    if (string.Equals(sortordername, "InterfaceID", StringComparison.OrdinalIgnoreCase))
                    {
                        if (interfaceIDType == "string")
                        {
                            sortordername = "CompInterfaceIDInString";
                        }
                        else
                        {
                            sortordername = "CompInterfaceID";
                        }
                    }
                    if (string.IsNullOrEmpty(sortordertype))
                    {
                        sortordertype = "asc";
                    }
                    if (sortordername != "")
                    {
                        if (string.Equals(sortordertype, "desc", StringComparison.OrdinalIgnoreCase))
                        {
                            dt = dt.AsEnumerable().OrderBy(k => k[sortordername]).Reverse().CopyToDataTable();
                        }
                        else
                        {
                            dt = dt.AsEnumerable().OrderBy(k => k[sortordername]).CopyToDataTable();
                        }
                    }

                    int rowCount = 2;
                    int cellCount = 1;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cellCount = 1;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["componentid"].ToString();
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["CompInterfaceIDInString"].ToString();
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["CompDesc"].ToString();
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = Convert.ToInt32(dt.Rows[i]["operationno"]);
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = Convert.ToInt32(dt.Rows[i]["OpnInterfaceID"]);
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["OpnDesc"].ToString();
                        if (CompanyStatus)
                        {
                            cellCount++;
                            exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["CellID"].ToString();
                            cellCount++;
                            exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["PartFamily"].ToString();

                        }
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["machineid"].ToString();
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["MachiningTime"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["loadunload"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["cycletime"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["MachiningTimeThreshold"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["loadUnloadTimeThreshold"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["StdSetupTime"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["price"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["drawingno"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["SubOperations"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["TargetPercent"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["customerid"].ToString();
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["SCIThreshold"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["DCLThreshold"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["FinishedOperation"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["MinLoadUnloadThreshold"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["IncentiveTime"];

                        if (ConfigurationManager.AppSettings["AlliedPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            cellCount++;
                            exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["StdTestPressure"];
                            cellCount++;
                            exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["StdHoldingTime"];
                        }
                        rowCount++;
                    }
                    setBorderThin(exelworksheet, 2, 1, rowCount - 1, cellCount);
                    //for (int i = 1; i <= cellCount; i++)
                    //{
                    exelworksheet.Column(1).AutoFit();
                    exelworksheet.Column(3).AutoFit();
                    exelworksheet.Column(6).AutoFit();
                    //}

                    DownloadFile(destination, Excel.GetAsByteArray());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void setBorderThin(ExcelWorksheet sheet, int fromRow, int fromcol, int toRow, int toCol)
        {
            var modelTable3 = sheet.Cells[fromRow, fromcol, toRow, toCol];
            modelTable3.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        protected void btnTemplateExport_Click(object sender, EventArgs e)
        {
            try
            {
                bool CompanyStatus = BindCompanyData();
                string Filename = string.Empty;
                string templatefile = string.Empty;
                if (ConfigurationManager.AppSettings["AlliedPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    Filename = "ImportCOPTemplate_Allied.xlsx";
                }
                else if (CompanyStatus)
                {
                    Filename = "ImportCOPTemplate_KTA.xlsx";
                }
                else
                {
                    Filename = "ImportCOPTemplate.xlsx";
                }
                string Source = string.Empty;
                Source = GetReportPath(Filename);
                string Template = string.Empty;
                Template = "ImportCOPTemplate_" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("ImportCOPTemplate- \n " + Source);
                }
                FileInfo newFile = new FileInfo(Source);
                ExcelPackage Excel = new ExcelPackage(newFile, true);
                DownloadFile(destination, Excel.GetAsByteArray());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void lnkImportPumpFile_Click(object sender, EventArgs e)
        {
            try
            {

                int success = 0;
                DataTable dtCOP = new DataTable();
                if (fuPumpImport.HasFile)
                {
                    string fileName = fuPumpImport.FileName;
                    if (Path.GetExtension(fileName) != ".xlsx" && Path.GetExtension(fileName) != ".xls")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "RecordsTextopenModaladded", "alert('Please choose the valid .xlsx or .xls file');", true);
                        return;
                    }
                    else
                    {
                        if (!Directory.Exists(Server.MapPath("ImportedFiles")))
                        {
                            Directory.CreateDirectory(Server.MapPath("ImportedFiles"));
                        }
                        string savedFileName = Server.MapPath("ImportedFiles//" + fileName);
                        fuPumpImport.SaveAs(savedFileName);
                        string Errormsg = GetDataTableFromFile(savedFileName, out dtCOP);
                        if (Errormsg != "")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "warning", "alert('" + Errormsg + "')", true);
                            return;
                        }
                        VDGDataBaseAccess.deleteTempCOPMasterDetailsRecords();
                        if (dtCOP.Rows.Count > 0)
                        {

                            string errorResult = VDGDataBaseAccess.saveImportedCOPDataToTempTable(dtCOP, chkUpdateComponent.Checked);
                            if (errorResult != "")
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "RecordsTextopenModaladded", "alert('" + errorResult + "');", true);

                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "RecordsTextopenModaladded", "alert('Data Imported Successfully.');", true);
                                BindComponentInformation();
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "RecordsTextopenModaladded", "alert('Import failed. Empty excel file.');", true);
                            return;
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "warning", "alert('Please choose a file to import.');", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private string GetDataTableFromFile(string fileName, out DataTable dt)
        {
            string Errormsg = "";
            dt = new DataTable();
            try
            {
                string pono = "";
                using (var pck = new ExcelPackage())
                {
                    using (var stream = File.OpenRead(fileName))
                    {
                        pck.Load(stream);
                    }
                    var workBook = pck.Workbook;
                    if (workBook != null)
                    {
                        try
                        {
                            var worksheet = workBook.Worksheets[1];
                            dt = new DataTable(worksheet.Name);
                            int totalCols = worksheet.Dimension.End.Column;
                            int totalRows = GetLastUsedRow(worksheet);
                            int startRow = 2;
                            ExcelRange wsRow;
                            DataRow dr;

                            //foreach (var firstRowCell in worksheet.Cells[1, 1, 1, totalCols])
                            //{
                            //    dt.Columns.Add(firstRowCell.Text.Trim());
                            //}
                            //dt.Columns["Machine"].ColumnName = "CNCMachine";
                            //dt.Columns["LoadingUnloadingTime"].ColumnName = "LoadingUnloading";
                            //dt.Columns["MachiningTime"].ColumnName = "CycleTime";
                            dt.Columns.Add("ItemNo");
                            dt.Columns.Add("Iteminterfaceid");
                            dt.Columns.Add("Itemdescription");
                            dt.Columns.Add("Operationno");
                            dt.Columns.Add("Opninterfaceid");
                            dt.Columns.Add("Opndescription");
                            dt.Columns.Add("CNC M/C");
                            dt.Columns.Add("MachiningTime");
                            dt.Columns.Add("LoadUnloadTime");
                            dt.Columns.Add("CycleTime");
                            dt.Columns.Add("MachiningTimeThreshold");
                            dt.Columns.Add("LoadUnloadTimeThreshold");
                            dt.Columns.Add("StdSetupTime");
                            dt.Columns.Add("Price");
                            dt.Columns.Add("Drawingno");
                            dt.Columns.Add("SubOperations");
                            dt.Columns.Add("TargetPercent");
                            dt.Columns.Add("customerid");
                            dt.Columns.Add("SCIThreshold");
                            dt.Columns.Add("DCLThreshold");
                            dt.Columns.Add("FinishedOperation");
                            dt.Columns.Add("MinLoadUnloadThreshold");
                            dt.Columns.Add("IncentiveTime");
                            dt.Columns.Add("ID");
                            //dt.Columns.Add("ID");
                            int i = 0;
                            for (int rowNum = startRow; rowNum <= totalRows; rowNum++)
                            {
                                wsRow = worksheet.Cells[rowNum, 1, rowNum, totalCols];
                                dr = dt.NewRow();
                                foreach (var cell in wsRow)
                                {
                                    if (string.IsNullOrEmpty(cell.Text.Trim()))
                                    {
                                        dr[cell.Start.Column - 1] = DBNull.Value;
                                    }
                                    else
                                    {
                                        dr[cell.Start.Column - 1] = cell.Text;
                                    }
                                }

                                dt.Rows.Add(dr);
                                dt.Rows[i]["Id"] = i + 1;
                                i++;
                            }
                            Errormsg = validate(dt);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteDebugLog("Error while importing Pump Master Details : " + ex.Message);
                            return "Failed to Import Data.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog("Error while importing Pump Master Details= " + ex.Message);
                return "Failed to Import Data.";
            }
            return Errormsg;
        }
        private string validate(DataTable dt)
        {
            string result = "";
            try
            {
                DataTable dtCOP = new DataTable();
                DataTable dtComp = VDGDataBaseAccess.getCompOperationMasterData(out dtCOP);
                List<string> DuplicatesRows = new List<string>();
                List<string> BlankItemNo = new List<string>();
                List<string> OperationNo = new List<string>();
                string RowIndex = string.Empty;
                string dupValue = string.Empty;
                foreach (DataRow row in dt.Rows)
                {
                    String Itemvalue = row["Iteminterfaceid"].ToString();
                    String Oprvalue = row["Opninterfaceid"].ToString();
                    RowIndex = "Row: " + row["Id"].ToString();
                    if (String.IsNullOrEmpty(Itemvalue))
                    {
                        BlankItemNo.Add(RowIndex);
                    }
                    if (String.IsNullOrEmpty(Oprvalue))
                    {
                        OperationNo.Add(RowIndex);
                    }
                    string commID = row["ItemNo"].ToString();
                    string commDesc = row["ItemDescription"].ToString();
                    if (commID.Length > 50)
                    {
                        result = string.Format("ItemNumber " + commID + " length cannot be more than 50.");
                        return result;
                    }
                    if (!String.IsNullOrEmpty(Itemvalue))
                    {
                        if (hfInterfaceDataType.Value == "Numeric")
                        {
                            var isNumeric = int.TryParse(Itemvalue, out int n);
                            if (!isNumeric)
                            {
                                result = string.Format("Iteminterfaceid " + Itemvalue + " should be numeric");
                                return result;
                            }
                        }

                    }
                    if (commDesc.Length > 100)
                    {
                        result = string.Format("ItemDescription " + commDesc + "  length cannot be more than 100.");
                        return result;
                    }
                    if (dtComp != null)
                    {
                        if (!String.IsNullOrEmpty(Itemvalue))
                        {
                            var existRow = dtComp.AsEnumerable().Where(k => k.Field<string>("InterfaceID").Equals(Itemvalue, StringComparison.OrdinalIgnoreCase) && (!k.Field<string>("componentid").Equals(commID, StringComparison.OrdinalIgnoreCase))).FirstOrDefault();
                            if (existRow != null)
                            {
                                result = string.Format("Item Interface ID " + Itemvalue + " already exists.");
                                return result;
                            }
                        }
                    }
                    if (dtCOP != null)
                    {
                        if (!String.IsNullOrEmpty(Itemvalue) && !String.IsNullOrEmpty(Oprvalue))
                        {
                            int opnNo = 0;
                            if (!String.IsNullOrEmpty(row["OperationNo"].ToString()))
                            {
                                opnNo = Convert.ToInt32(row["OperationNo"].ToString());
                            }
                            var existRow = dtCOP.AsEnumerable().Where(k => k.Field<string>("InterfaceID").Equals(Oprvalue, StringComparison.OrdinalIgnoreCase) && k.Field<string>("componentid").Equals(commID, StringComparison.OrdinalIgnoreCase) && k.Field<int>("operationno") != opnNo).FirstOrDefault();
                            if (existRow != null)
                            {
                                result = string.Format("Operation Interface ID " + Oprvalue + " already exists.");
                                return result;
                            }
                        }

                    }
                }
                var ItemMsg = string.Join(", ", BlankItemNo.ToArray());
                var OprMsg = string.Join(", ", OperationNo.ToArray());
                if (BlankItemNo.Count > 0 || OperationNo.Count > 0)
                {
                    if (OperationNo != null)
                    {
                        if (OperationNo.Count > 0)
                        {
                            result = string.Format(" Operation Interface IDs cannot be blank for row :  {0}  ", OprMsg);
                        }
                    }
                    if (BlankItemNo != null)

                    {
                        if (BlankItemNo.Count > 0)
                        {
                            result = string.Format(" Item Interface IDs cannot be blank for row : {0}", ItemMsg);
                        }
                    }
                    return result;
                }

                var duplicates = from a in dt.AsEnumerable()
                                 select
                                 new { SrNo = a["ID"], ItemNumber = a["ItemNo"], OperationNo = a["OperationNo"], CNCMachine = a["CNC M/C"] } into temp
                                 group temp by new { temp.ItemNumber, temp.OperationNo, temp.CNCMachine } into grouped
                                 where grouped.Count() > 1
                                 select grouped.Select(g => new { g.SrNo, g.ItemNumber, g.OperationNo, g.CNCMachine });

                foreach (var d in duplicates)
                {
                    foreach (var dup in d)
                    {
                        dupValue = "Row: " + dup.SrNo + ", Machine: " + dup.CNCMachine + ", Component: " + dup.ItemNumber + ", Operation Name: " + dup.OperationNo;
                        DuplicatesRows.Add(dupValue);
                    }
                }
                if (DuplicatesRows.Count > 0)
                {
                    var message = string.Join(" -- ", DuplicatesRows.ToArray());
                    if (message != null || message.Count() > 0)
                    {
                        result = string.Format("Duplicate entry(s) found, Please correct it before import : {0} ", message);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Logger.WriteErrorLog(ex.Message);
            }
            return result;
        }

        private static int GetLastUsedRow(ExcelWorksheet sheet)
        {
            var row = sheet.Dimension.End.Row;
            while (row >= 1)
            {
                var range = sheet.Cells[row, 1, row, sheet.Dimension.End.Column];
                if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                {
                    break;
                }
                row--;
            }
            return row;
        }

        protected void txtComponentSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                BindComponentInformation();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindComponentInformation();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}