using BusinessClassLibrary;
using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Cumi.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class EmployeeInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                Session["PlantData"] = null;
                Session["EmployeeInfo"] = null;
                BindPlantId();
                GetData("");
            }
        }

        private void GetData(string search)
       {
            try
            {
                List<EmployeeInfoModel> EmpInfo = new List<EmployeeInfoModel>();
                if (Session["EmployeeInfo"] == null)
                {
                    List<EmployeeInfoModel> EmpInfoFromDB = EmployeeInfoDataBase.GetAllEmpDetails(search, "ViewEmployeeInfo");
                    if (ddlField.SelectedValue.Equals("EmployeeID", StringComparison.OrdinalIgnoreCase))
                    {
                        EmpInfo = EmpInfoFromDB.Where(k => k.employeeid.ToLower().Contains(txtFieldSearch.Text.ToLower())).ToList();
                    }
                    else if (ddlField.SelectedValue.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    {
                        EmpInfo = EmpInfoFromDB.Where(k => k.Name.ToLower().Contains(txtFieldSearch.Text.ToLower())).ToList();
                    }
                    else if (ddlField.SelectedValue.Equals("InterfaceID", StringComparison.OrdinalIgnoreCase))
                    {
                        EmpInfo = EmpInfoFromDB.Where(k => k.interfaceid.ToLower().Contains(txtFieldSearch.Text.ToLower())).ToList();
                    }
                    Session["EmployeeInfo"] = EmpInfo;
                }
                else
                    EmpInfo = Session["EmployeeInfo"] as List<EmployeeInfoModel>;
                lvEmployeeInfo.DataSource = EmpInfo;
                lvEmployeeInfo.DataBind();

                if(EmpInfo.Count <= 100)
                {
                   lvEmployeeInfo.FindControl("Tr3").Visible = false;
                }
                else
                {
                    lvEmployeeInfo.FindControl("Tr3").Visible = true;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }

        }

        protected void lvHoliday_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lvEmployeeInfo.EditIndex = e.NewEditIndex;
            GetData("");
        }

        protected void DeleteDownCat(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkRemove = (LinkButton)sender;
                string StartTime = string.Empty;
                string EndTime = string.Empty;
                bool val = EmployeeInfoDataBase.CheckExistingEmployeeInAutodata(lnkRemove.CommandName.ToString(), out StartTime, out EndTime);
                if (val)
                {
                    lblMessages.Text = "Record Exists In Auto Data,Employee Cannot be Removed";
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else
                {
                    EmployeeInfoDataBase.deleteEmployeeInformation(lnkRemove.CommandArgument.ToString(), lnkRemove.CommandName.ToString());
                    lblMessages.Text = "Details deleted successfully.";
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                }
                Session["EmployeeInfo"] = null;
                GetData("");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }

        protected void lvEmployeeInfo_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            try
            {
                string employeeid = ((Label)(lvEmployeeInfo.Items[e.ItemIndex].FindControl("lblemployeeid"))).Text.Trim();
                string interfaceid = ((TextBox)(lvEmployeeInfo.Items[e.ItemIndex].FindControl("txtinterfaceid"))).Text.Trim();
                string StartTime = string.Empty;
                string EndTime = string.Empty;
                bool val = EmployeeInfoDataBase.CheckExistingEmployeeInAutodata(interfaceid, out StartTime, out EndTime);
                if (val)
                {
                    lblMessages.Text = "Record Exists In Auto Data,Employee Cannot be Removed";
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else
                {

                    EmployeeInfoDataBase.deleteEmployeeInformation(employeeid, interfaceid);

                    lblMessages.Text = "Details deleted successfully.";
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                }
                Session["EmployeeInfo"] = null;
                lvEmployeeInfo.EditIndex = -1;
                GetData("");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }

        protected void lvEmployeeInfo_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lvEmployeeInfo.EditIndex = -1;
            GetData("");
        }

        protected void lvEmployeeInfo_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            try
            {
                string employeeid = ((TextBox)e.Item.FindControl("txtemployeeid")).Text;
                string Name = ((TextBox)e.Item.FindControl("txtName")).Text;
                string interfaceid = ((TextBox)e.Item.FindControl("txtinterfaceid")).Text;
                string designation = ((TextBox)e.Item.FindControl("txtdesignation")).Text;
                string qualification = ((TextBox)e.Item.FindControl("txtqualification")).Text;
                string address = ((TextBox)e.Item.FindControl("txtaddress")).Text;
                string phone = ((TextBox)e.Item.FindControl("txtphone")).Text;
                string Email = ((TextBox)e.Item.FindControl("txtEmail")).Text;
                bool chkCompDef = ((CheckBox)e.Item.FindControl("chkEdit")).Checked;
                ListBox ddlMultiPlantId = ((ListBox)e.Item.FindControl("ddlMultiPlantId")) as ListBox;
                DropDownList ddlEmployeeRole = (e.Item.FindControl("ddlEmployeeRole") as DropDownList);
                string EmpRole = ddlEmployeeRole.SelectedItem != null ? ddlEmployeeRole.SelectedValue : "";
                bool IsActive = ((CheckBox)e.Item.FindControl("chkIsActiveEdit")).Checked;
                string SMSTextTemplate = "", plantSelect = "";
                foreach (ListItem item in ddlMultiPlantId.Items)
                {
                    if (item.Selected)
                    {
                        SMSTextTemplate += item.Value + "$@#";
                    }
                }
                if (SMSTextTemplate != "")
                {
                    string[] result = SMSTextTemplate.Split(new string[] { "$@#" }, StringSplitOptions.None);
                    result = result.Take(result.Count() - 1).ToArray();
                    plantSelect = string.Join(",", result.ToArray());
                }
                EmployeeInfoDataBase.DeleteEmpPlant(employeeid);
                EmployeeInfoDataBase.SaveSelectionsForPlant(plantSelect.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList(), employeeid);
                bool isSuccessFailure;
                EmployeeInfoDataBase.SaveEmployeeInformation(employeeid, 0, Name, designation, qualification,
                   address, "", phone, 0, 0, "", IsActive, false, "", interfaceid, (chkCompDef == true ? 1 : 0), Email, EmpRole, "InsertEmpInfo", out isSuccessFailure);
                lblMessages.ForeColor = System.Drawing.Color.Green;
                lblMessages.Text = "Details added successfully !!";
                lvEmployeeInfo.EditIndex = -1;
                Session["EmployeeInfo"] = null;
                GetData("");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }

        protected void lvEmployeeInfo_PreRender(object sender, EventArgs e)
        {
            GetData("");
        }

        protected void lvEmployeeInfo_ItemUpdating(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem row in lvEmployeeInfo.Items)
                {
                    string employeeid = ((Label)row.FindControl("lblemployeeid")).Text;
                    string Name = ((TextBox)row.FindControl("txtName")).Text;
                    string interfaceid = ((TextBox)row.FindControl("txtinterfaceid")).Text;
                    string designation = ((TextBox)row.FindControl("txtdesignation")).Text;
                    string qualification = ((TextBox)row.FindControl("txtqualification")).Text;
                    string address = ((TextBox)row.FindControl("txtaddress")).Text;
                    string phone = ((TextBox)row.FindControl("txtphone")).Text;
                    string Email = ((TextBox)row.FindControl("txtEmail")).Text;
                    bool chkCompDef = ((CheckBox)row.FindControl("chkEdit")).Checked;
                    string role = ((DropDownList)row.FindControl("ddlEmployeeRole")).SelectedValue;
                    ListBox ddlMultiPlantId = ((ListBox)row.FindControl("ddlMultiPlantId")) as ListBox;
                    DropDownList ddlEmployeeRole = (row.FindControl("ddlEmployeeRole") as DropDownList);
                    string EmpRole = ddlEmployeeRole.SelectedItem != null ? ddlEmployeeRole.SelectedValue : "";
                    bool IsActive = ((CheckBox)row.FindControl("chkIsActiveEdit")).Checked;
                    string SMSTextTemplate = "", plantSelect = "";
                    foreach (ListItem item in ddlMultiPlantId.Items)
                    {
                        if (item.Selected)
                        {
                            SMSTextTemplate += item.Value + "$@#";
                        }
                    }
                    if (SMSTextTemplate != "")
                    {
                        string[] result = SMSTextTemplate.Split(new string[] { "$@#" }, StringSplitOptions.None);
                        result = result.Take(result.Count() - 1).ToArray();
                        plantSelect = string.Join(",", result.ToArray());
                    }

                    string hdfUpade = ((HiddenField)row.FindControl("hdfUpade")).Value;
                    bool isSuccessFailure;
                    if (hdfUpade.Equals("update", StringComparison.OrdinalIgnoreCase))
                    {
                        EmployeeInfoDataBase.DeleteEmpPlant(employeeid);
                        EmployeeInfoDataBase.SaveSelectionsForPlant(plantSelect.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList(), employeeid);
                        EmployeeInfoDataBase.SaveEmployeeInformation(employeeid, 0, Name, designation, qualification,
                           address, "", phone, 0, 0, "", IsActive, false, "", interfaceid, (chkCompDef == true ? 1 : 0), Email, EmpRole, "InsertEmpInfo", out isSuccessFailure);
                    }
                }
                lblMessages.ForeColor = System.Drawing.Color.Green;
                lblMessages.Text = "Details updated successfully !!";
                lvEmployeeInfo.EditIndex = -1;
                Session["EmployeeInfo"] = null;
                GetData("");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }

        #region "Bind Plant Id"
        private void BindPlantId()
        {
            try
            {
                List<string> lstPlantData = null;
                if (Session["PlantData"] == null)
                    Session["PlantData"] = lstPlantData = DataBaseAccess.GetAllPlantsForPlantInfo();

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }
        #endregion

        protected void OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                ListBox ddlPlantID = (e.Item.FindControl("ddlMultiPlantId") as ListBox);
                DropDownList ddlEmployeeRole = (e.Item.FindControl("ddlEmployeeRole") as DropDownList);
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["AdvikPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    ddlEmployeeRole.Items.Add(new ListItem("Production Supervisor", "Production Supervisor"));
                    ddlEmployeeRole.Items.Add(new ListItem("Production Head", "Production Head"));
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["GEAPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    ddlEmployeeRole.Items.Add(new ListItem("Machining and Assembly", "MandA"));
                    ddlEmployeeRole.Items.Add(new ListItem("Assembly Operator", "AssemblyOperator"));
                    ddlEmployeeRole.Items.Add(new ListItem("Testing Operator", "TestingOperator"));
                    ddlEmployeeRole.Items.Add(new ListItem("Packing Operator", "PackingOperator"));
                    ddlEmployeeRole.Items.Add(new ListItem("Logistic Supervisor", "LogisticSupervisor"));
                    ddlEmployeeRole.Items.Add(new ListItem("QA Admin with Supervisor", "QAEngineerwithSupervisor"));
                    ddlEmployeeRole.Items.Add(new ListItem("Maintainance Engg", "MaintainanceEngineer"));
                    ddlEmployeeRole.Items.Add(new ListItem("Store Assistant", "Store Assistant"));
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["AlliedPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    ddlEmployeeRole.Items.Add(new ListItem("Security", "Security"));
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["BajajPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    ddlEmployeeRole.Items.Add(new ListItem("Manager", "Manager"));
                    ddlEmployeeRole.Items.Add(new ListItem("Group Leader", "Group Leader"));
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["PittiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    ddlEmployeeRole.Items.Add(new ListItem("Sr. Engineer", "Sr Engineer"));
                    ddlEmployeeRole.Items.Add(new ListItem("Manager", "Manager"));
                    ddlEmployeeRole.Items.Add(new ListItem("HOD", "HOD"));
                    ddlEmployeeRole.Items.Add(new ListItem("Maintenance Manager", "MaintenanceManager"));
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["VulkanMachineShopPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    ddlEmployeeRole.Items.Add(new ListItem("PM Engineer", "PM Engineer"));
                    ddlEmployeeRole.Items.Add(new ListItem("Maintenance", "Maintenance"));
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["CumiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    CumiDBAccess.BindRoleFromEmployeeInfo(ddlEmployeeRole);
                }
                Label hdfEmpRole = (e.Item.FindControl("hdfEmployeeRole") as Label);
                if (ddlPlantID != null)
                {
                    List<string> lstPlantData = null;
                    if (Session["PlantData"] == null)
                        BindPlantId();
                    else
                        lstPlantData = Session["PlantData"] as List<string>;
                    ddlPlantID.DataSource = lstPlantData;
                    ddlPlantID.DataBind();
                    Label hdfPlantID = (e.Item.FindControl("hdfPlantID") as Label);
                    string value = hdfPlantID.Text;
                    string[] result = value.Split(new string[] { "," }, StringSplitOptions.None);

                    foreach (string item in result)
                    {
                        if (item != "")
                            ddlPlantID.Items.FindByValue(item).Selected = true;
                        //ddlPlantID.SelectedValue = item;
                    }
                }
                if (ddlEmployeeRole != null)
                {
                    string EmpRole = hdfEmpRole.Text;
                    ddlEmployeeRole.SelectedValue = EmpRole;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }

        protected void lvEmployeeInfo_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if ((e.Item != null) && (e.Item.ItemType == ListViewItemType.InsertItem))
                {
                    ListBox ddlPlantID = (e.Item.FindControl("ddlMultiPlantId") as ListBox);
                    if (ddlPlantID != null)
                    {
                        List<string> lstPlantData = null;
                        if (Session["PlantData"] == null)
                            BindPlantId();
                        else
                            lstPlantData = Session["PlantData"] as List<string>;
                        ddlPlantID.DataSource = lstPlantData;
                        ddlPlantID.DataBind();
                        //Label lblPlantID = (e.Item.FindControl("lblPlantID") as Label);
                    }
                    DropDownList ddlEmpRole = (e.Item.FindControl("ddlEmployeeRole") as DropDownList);
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["AdvikPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        ddlEmpRole.Items.Add(new ListItem("Production Supervisor", "Production Supervisor"));
                        ddlEmpRole.Items.Add(new ListItem("Production Head", "Production Head"));
                    }
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["GEAPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        ddlEmpRole.Items.Add(new ListItem("Machining and Assembly", "MandA"));
                        ddlEmpRole.Items.Add(new ListItem("Assembly Operator", "AssemblyOperator"));
                        ddlEmpRole.Items.Add(new ListItem("Testing Operator", "TestingOperator"));
                        ddlEmpRole.Items.Add(new ListItem("Packing Operator", "PackingOperator"));
                        ddlEmpRole.Items.Add(new ListItem("Logistic Supervisor", "LogisticSupervisor"));
                        ddlEmpRole.Items.Add(new ListItem("QA Admin with Supervisor", "QAEngineerwithSupervisor"));
                        ddlEmpRole.Items.Add(new ListItem("Maintainance Engg", "MaintainanceEngineer"));
                        ddlEmpRole.Items.Add(new ListItem("Store Assistant", "Store Assistant"));
                    }
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["AlliedPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        ddlEmpRole.Items.Add(new ListItem("Security", "Security"));
                    }
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["BajajPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        ddlEmpRole.Items.Add(new ListItem("Manager", "Manager"));
                        ddlEmpRole.Items.Add(new ListItem("Group Leader", "Group Leader"));
                    }
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["PittiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        ddlEmpRole.Items.Add(new ListItem("Sr. Engineer", "Sr Engineer"));
                        ddlEmpRole.Items.Add(new ListItem("Manager", "Manager"));
                        ddlEmpRole.Items.Add(new ListItem("HOD", "HOD"));
                        ddlEmpRole.Items.Add(new ListItem("Maintenance Manager", "MaintenanceManager"));
                    }
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["VulkanMachineShopPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        ddlEmpRole.Items.Add(new ListItem("PM Engineer", "PM Engineer"));
                        ddlEmpRole.Items.Add(new ListItem("Maintenance", "Maintenance"));
                    }
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["CumiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        CumiDBAccess.BindRoleFromEmployeeInfo(ddlEmpRole);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string condition = hdfCondition.Value;
            string employeeid = string.Empty, Name = string.Empty, designation = string.Empty, qualification = string.Empty, address = string.Empty,
                phone = string.Empty, Email = string.Empty, SMSTextTemplate = string.Empty, plantSelect = string.Empty;
            bool chkCompDef = false, isSuccessFailure = false, chkIsActive = false;
            string EmpRole = string.Empty;
            string interfaceid = "";
            string password = string.Empty;
            ListBox ddlMultiPlantId = null;
            try
            {
                if (condition.Equals("Save", StringComparison.OrdinalIgnoreCase) || condition.Equals("Update", StringComparison.OrdinalIgnoreCase))
                {

                    //TextBox textBox = (TextBox)this.lvEmployeeInfo.InsertItem.FindControl("txtemployeeid");
                    employeeid = ((TextBox)this.lvEmployeeInfo.InsertItem.FindControl("txtemployeeid")).Text;
                    Name = ((TextBox)this.lvEmployeeInfo.InsertItem.FindControl("txtName")).Text;
                    interfaceid = ((TextBox)this.lvEmployeeInfo.InsertItem.FindControl("txtinterfaceid")).Text;
                    designation = ((TextBox)this.lvEmployeeInfo.InsertItem.FindControl("txtdesignation")).Text;
                    qualification = ((TextBox)this.lvEmployeeInfo.InsertItem.FindControl("txtqualification")).Text;
                    address = ((TextBox)this.lvEmployeeInfo.InsertItem.FindControl("txtaddress")).Text;
                    phone = ((TextBox)this.lvEmployeeInfo.InsertItem.FindControl("txtphone")).Text;
                    Email = ((TextBox)this.lvEmployeeInfo.InsertItem.FindControl("txtEmail")).Text;
                    chkCompDef = ((CheckBox)this.lvEmployeeInfo.InsertItem.FindControl("chkEdit")).Checked;
                    ddlMultiPlantId = ((ListBox)this.lvEmployeeInfo.InsertItem.FindControl("ddlMultiPlantId")) as ListBox;
                    DropDownList ddlEmployeeRole = (this.lvEmployeeInfo.InsertItem.FindControl("ddlEmployeeRole") as DropDownList);
                    EmpRole = ddlEmployeeRole.SelectedItem != null ? ddlEmployeeRole.SelectedValue : "";
                    chkIsActive = ((CheckBox)this.lvEmployeeInfo.InsertItem.FindControl("chkIsActiveEdit")).Checked;

                    foreach (ListItem item in ddlMultiPlantId.Items)
                    {
                        if (item.Selected)
                        {
                            SMSTextTemplate += item.Value + "$@#";
                        }
                    }
                    if (SMSTextTemplate != "")
                    {
                        string[] result = SMSTextTemplate.Split(new string[] { "$@#" }, StringSplitOptions.None);
                        result = result.Take(result.Count() - 1).ToArray();
                        plantSelect = string.Join(",", result.ToArray());
                    }
                    if (!string.IsNullOrEmpty(employeeid))
                    {
                        EmployeeInfoDataBase.DeleteEmpPlant(employeeid);
                        EmployeeInfoDataBase.SaveSelectionsForPlant(plantSelect.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList(), employeeid);

                        EmployeeInfoDataBase.SaveEmployeeInformation(employeeid, 0, Name, designation, qualification,
                           address, "", phone, 0, 0, "", chkIsActive, false, "", interfaceid, (chkCompDef == true ? 1 : 0), Email, EmpRole, "InsertEmpInfo", out isSuccessFailure);
                    }

                }
                //if (condition.Equals("Update", StringComparison.OrdinalIgnoreCase))
                //{
                //    Update(ref employeeid, ref Name, ref interfaceid, ref designation, ref qualification, ref address, ref phone, ref Email, ref chkCompDef, ref ddlMultiPlantId, ref SMSTextTemplate, ref plantSelect, ref isSuccessFailure);
                //}

                Update(ref employeeid, ref Name, ref interfaceid, ref designation, ref qualification, ref address, ref phone, ref Email, ref chkCompDef, ref ddlMultiPlantId, ref SMSTextTemplate, ref plantSelect, ref EmpRole, ref isSuccessFailure, ref chkIsActive, ref password);

                lblMessages.ForeColor = System.Drawing.Color.Green;
                lblMessages.Text = "Details added/Updated successfully !!";
                Session["EmployeeInfo"] = null;
                GetData("");
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        #region "Update and Save Method"
        private void Update(ref string employeeid, ref string Name, ref string interfaceid, ref string designation, ref string qualification, ref string address, ref string phone,
          ref string Email, ref bool chkCompDef, ref ListBox ddlMultiPlantId, ref string SMSTextTemplate, ref string plantSelect, ref string EmpRole, ref bool isSuccessFailure, ref bool chkIsActive, ref string password)
        {
            try
            {
                foreach (ListViewItem row in lvEmployeeInfo.Items)
                {
                    string hdfUpade = ((HiddenField)row.FindControl("hdfUpade")).Value;
                    if (hdfUpade.Equals("update", StringComparison.OrdinalIgnoreCase))
                    {
                        employeeid = ((Label)row.FindControl("lblemployeeid")).Text;
                        Name = ((TextBox)row.FindControl("txtName")).Text;
                        interfaceid = ((TextBox)row.FindControl("txtinterfaceid")).Text;
                        designation = ((TextBox)row.FindControl("txtdesignation")).Text;
                        qualification = ((TextBox)row.FindControl("txtqualification")).Text;
                        address = ((TextBox)row.FindControl("txtaddress")).Text;
                        phone = ((TextBox)row.FindControl("txtphone")).Text;
                        Email = ((TextBox)row.FindControl("txtEmail")).Text;
                        chkCompDef = ((CheckBox)row.FindControl("chkEdit")).Checked;
                        DropDownList ddlEmployeeRole = (row.FindControl("ddlEmployeeRole") as DropDownList);
                        EmpRole = ddlEmployeeRole.SelectedItem != null ? ddlEmployeeRole.SelectedValue : "";
                        chkIsActive = ((CheckBox)row.FindControl("chkIsActiveEdit")).Checked;
                        ddlMultiPlantId = ((ListBox)row.FindControl("ddlMultiPlantId")) as ListBox;
                        SMSTextTemplate = ""; plantSelect = "";
                        foreach (ListItem item in ddlMultiPlantId.Items)
                        {
                            if (item.Selected)
                            {
                                SMSTextTemplate += item.Value + "$@#";
                            }
                        }
                        if (SMSTextTemplate != "")
                        {
                            string[] result = SMSTextTemplate.Split(new string[] { "$@#" }, StringSplitOptions.None);
                            result = result.Take(result.Count() - 1).ToArray();
                            plantSelect = string.Join(",", result.ToArray());
                        }
                        isSuccessFailure = false;
                        password = ((HiddenField)row.FindControl("hfPassword")).Value;
                        if (!string.IsNullOrEmpty(employeeid))
                        {
                            EmployeeInfoDataBase.DeleteEmpPlant(employeeid);
                            EmployeeInfoDataBase.SaveSelectionsForPlant(plantSelect.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList(), employeeid);
                            EmployeeInfoDataBase.SaveEmployeeInformation(employeeid, 0, Name, designation, qualification,
                               address, "", phone, 0, 0, "", chkIsActive, false, password, interfaceid, (chkCompDef == true ? 1 : 0), Email, EmpRole, "InsertEmpInfo", out isSuccessFailure);
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        #endregion

        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Session["EmployeeInfo"] = null;
        //        GetData(searchEmployee.Value.Trim());
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.Message);
        //    }
        //}
        protected void btnView_Click(object sender, EventArgs e)
        {
            Session["EmployeeInfo"] = null;
            GetData("");
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            string success = "";
            try
            {
                success = TMPTrakGenerateReport.GenerateEmployeeInfoReport(ddlField.SelectedValue, txtFieldSearch.Text.Trim());
                try
                {
                    if (success.Equals("Template Not Found", StringComparison.OrdinalIgnoreCase))
                    {
                        HelperClassGeneric.openWarningToastrModal(this, "Template not found");
                    }
                    else if (success.Equals("Data Not Found", StringComparison.OrdinalIgnoreCase))
                    {
                        HelperClassGeneric.openWarningToastrModal(this, "Data not found");
                    }
                    else
                    {
                        HelperClassGeneric.openErrorModal(this, "Template not found");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}