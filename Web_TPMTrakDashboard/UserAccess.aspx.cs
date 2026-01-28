using BusinessClassLibrary;
using ModelClassLibrary;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.AlertModule;

namespace Web_TPMTrakDashboard
{
    public partial class UserAccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            else if (!IsPostBack)
            {
                BindPlant();
                BindEmployee();
                BindGrid();

            }
        }

        private void BindLandingPage()
        {
            try
            {
                string username = cmbUserID.SelectedValue;
                //string username = Session["UserName"] != null ? Session["UserName"].ToString() : string.Empty;
                //string password = Session["Password"] != null ? Session["Password"].ToString() : string.Empty;
                string LandingPage = Models.DataBaseAccess.BindLandingPage(username);
                ddlPages.SelectedValue = LandingPage;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindEmployee()
        {
            try
            {
                List<string> lstPlantData = BindCockpitView.GetAllUserData();
                cmbUserID.DataSource = lstPlantData;
                cmbUserID.DataBind();
                cmbUserID.SelectedValue = Session["UserName"] != null ? Session["UserName"].ToString().ToUpper() : "PCT";
                GetPasswordAndAdminInfo();
            }
            catch (Exception ex)
            {
                // ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void GetPasswordAndAdminInfo()
        {
            UserAccessModel Employee = BindCockpitView.GetEmployeeDetails(cmbUserID.SelectedValue.ToString());
            if (Employee != null)
            {
                txtPassword.Text = Employee.Password;
                txtPassword.Attributes["value"] = txtPassword.Text;
                chkAdmin.Checked = Employee.Admin;
                lblUserName.Text = Employee.UserName;
            };
        }

        private void BindPlant()
        {
            try
            {
                List<string> lstPlantData = BindCockpitView.ViewPlantToDisplay();
                cmbPlantID.DataSource = lstPlantData;
                cmbPlantID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindGrid()
        {
            List<UserAccessEntity> userAccessEntities = new List<UserAccessEntity>();
            try
            {
                BindLandingPage();
                DataTable dt = DataBaseAccess.GetUseraccess(cmbUserID.SelectedValue.ToString());
                dt = dt.AsEnumerable().OrderBy(x => x["DomainName"].ToString()).CopyToDataTable();
                string menu = dt.Rows[0]["domain"].ToString();
                bool menucheck = true;
                UserAccessEntity entity = new UserAccessEntity();
                entity.MenuName = menu;
                entity.DomainName = dt.Rows[0]["DomainName"].ToString();
                List<SubData> DataList = new List<SubData>();

                var DomainList = dt.AsEnumerable().Select(x => x["DomainName"].ToString()).Distinct().ToList();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubData data = new SubData();
                    if (!menu.Equals(dt.Rows[i]["domain"].ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        entity.MenuCheck = menucheck;
                        entity.Values = DataList;
                        entity.DomainName = dt.Rows[i - 1]["DomainName"].ToString();
                        if (entity.DomainName.Contains("Energy"))
                        {
                            if (System.Web.Configuration.WebConfigurationManager.AppSettings["EnergyModule"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                                entity.Visibility = true;
                            else
                                entity.Visibility = false;
                        }
                        else if (entity.DomainName.Contains("Help"))
                        {
                            if (System.Web.Configuration.WebConfigurationManager.AppSettings["HelpRequest"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                                entity.Visibility = true;
                            else
                                entity.Visibility = false;
                        }
                        else if (entity.DomainName.Contains("Alert"))
                        {
                            if (System.Web.Configuration.WebConfigurationManager.AppSettings["AlertPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                                entity.Visibility = true;
                            else
                                entity.Visibility = false;
                        }
                        else if (entity.DomainName.Contains("SPC"))
                        {
                            if (System.Web.Configuration.WebConfigurationManager.AppSettings["ShowSPCWebMenu"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                                entity.Visibility = true;
                            else
                                entity.Visibility = false;
                        }
                        else if (entity.DomainName.Contains("MachineConnectWeb"))
                        {
                            if (System.Web.Configuration.WebConfigurationManager.AppSettings["MachineConnectPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                                entity.Visibility = true;
                            else
                                entity.Visibility = false;
                        }
                        else if (entity.DomainName.Equals("PTA", StringComparison.OrdinalIgnoreCase))
                        {
                            if (System.Web.Configuration.WebConfigurationManager.AppSettings["PTAPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                                entity.Visibility = true;
                            else
                                entity.Visibility = false;
                        }
                        else if (entity.DomainName.Equals("VED", StringComparison.OrdinalIgnoreCase))
                        {
                            if (System.Web.Configuration.WebConfigurationManager.AppSettings["VEDIndustries"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                                entity.Visibility = true;
                            else
                                entity.Visibility = false;
                        }
                        else
                        {
                            entity.Visibility = true;
                        }

                        entity.MenuName = dt.Rows[i - 1]["domain"].ToString();
                        userAccessEntities.Add(entity);
                        entity = new UserAccessEntity();
                        DataList = new List<SubData>();
                        menucheck = true;
                        menu = dt.Rows[i]["domain"].ToString();
                    }
                    data.TableValueChecked = dt.Rows[i]["IsVisible"].ToString() == "True" ? true : false;
                    if (menucheck)
                    {
                        menucheck = data.TableValueChecked;
                    }
                    data.TableValueName = dt.Rows[i]["displayText"].ToString();
                    data.TextValueCode = dt.Rows[i]["Code"].ToString();
                    data.MenuCodeName = dt.Rows[i]["domain"].ToString();
                    data.MenuName = dt.Rows[i]["DomainName"].ToString();
                    DataList.Add(data);
                }
                entity.MenuName = dt.Rows[dt.Rows.Count - 1]["domain"].ToString();
                entity.DomainName = dt.Rows[dt.Rows.Count - 1]["DomainName"].ToString();
                //if (entity.DomainName.Contains("Energy"))
                //{
                //    if (System.Web.Configuration.WebConfigurationManager.AppSettings["EnergyModule"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                //    {
                //        entity.Visibility = true;
                //    }
                //    else
                //    {
                //        entity.Visibility = false;
                //    }
                //}
                //else if (entity.DomainName.Contains("Help"))
                //{
                //    if (System.Web.Configuration.WebConfigurationManager.AppSettings["EnergyModule"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                //    {
                //        entity.Visibility = true;
                //    }
                //    else
                //    {
                //        entity.Visibility = false;
                //    }
                //}
                //else
                //{
                //    entity.Visibility = true;
                //}
                entity.MenuCheck = menucheck;
                entity.Values = DataList;
                if (entity.DomainName.Contains("Energy"))
                {
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["EnergyModule"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.Visibility = true;
                    }
                    else
                    {
                        entity.Visibility = false;
                    }
                }
                else if (entity.DomainName.Contains("Help"))
                {
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["HelpRequest"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.Visibility = true;
                    }
                    else
                    {
                        entity.Visibility = false;
                    }
                }
                else if (entity.DomainName.Contains("Alert"))
                {
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["AlertPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.Visibility = true;
                    }
                    else
                    {
                        entity.Visibility = false;
                    }
                }
                else if (entity.DomainName.Contains("SPC"))
                {
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["ShowSPCWebMenu"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.Visibility = true;
                    }
                    else
                    {
                        entity.Visibility = false;
                    }
                }
                else if (entity.DomainName.Contains("MachineConnectWeb"))
                {
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["MachineConnectPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.Visibility = true;
                    }
                    else
                    {
                        entity.Visibility = false;
                    }
                }
                else if (entity.DomainName.Equals("PTA", StringComparison.OrdinalIgnoreCase))
                {
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["PTAPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.Visibility = true;
                    }
                    else
                    {
                        entity.Visibility = false;
                    }
                }
                else
                {
                    entity.Visibility = true;
                }
                userAccessEntities.Add(entity);
                if (userAccessEntities != null && userAccessEntities.Count > 0)
                {
                    if (userAccessEntities.Any(x => x.DomainName.Equals("Advik Module")))
                    {
                        if (!System.Web.Configuration.WebConfigurationManager.AppSettings["AdvikPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            userAccessEntities.Remove(userAccessEntities.Where(x => x.DomainName.Equals("Advik Module")).FirstOrDefault());
                        }
                    }
                    if (userAccessEntities.Any(x => x.DomainName.Equals("Bajaj Madule")))
                    {
                        if (!(System.Web.Configuration.WebConfigurationManager.AppSettings["BajajPages"].ToString() == "1"))
                            userAccessEntities.Remove(userAccessEntities.Where(x => x.DomainName.Equals("Bajaj Madule")).FirstOrDefault());
                    }
                    if (userAccessEntities.Any(x => x.DomainName.Equals("LnTOdisha")))
                    {
                        if (!(System.Web.Configuration.WebConfigurationManager.AppSettings["LnTOdishaPages"].ToString() == "1"))
                            userAccessEntities.Remove(userAccessEntities.Where(x => x.DomainName.Equals("LnTOdisha")).FirstOrDefault());
                    }
                    if (userAccessEntities.Any(x => x.DomainName.Equals("Vulakn Machine Shop")))
                    {
                        if (!(System.Web.Configuration.WebConfigurationManager.AppSettings["VulkanMachineShopPages"].ToString() == "1"))
                            userAccessEntities.Remove(userAccessEntities.Where(x => x.DomainName.Equals("Vulakn Machine Shop")).FirstOrDefault());
                    }
                    if (userAccessEntities.Any(x => x.DomainName.Equals("Vulakn Machine Shop")))
                    {
                        if (!(System.Web.Configuration.WebConfigurationManager.AppSettings["VulkanPages"].ToString() == "1"))
                            userAccessEntities.Remove(userAccessEntities.Where(x => x.DomainName.Equals("Vulkan Module")).FirstOrDefault());
                    }
                    if (userAccessEntities.Any(x => x.DomainName.Equals("Pradeep Metals")))
                    {
                        if (!(System.Web.Configuration.WebConfigurationManager.AppSettings["PradeepMetalPages"].ToString() == "1"))
                            userAccessEntities.Remove(userAccessEntities.Where(x => x.DomainName.Equals("Pradeep Metals")).FirstOrDefault());
                    }
                    if (userAccessEntities.Any(x => x.DomainName.Equals("Pitti")))
                    {
                        if (!(System.Web.Configuration.WebConfigurationManager.AppSettings["PittiPages"].ToString() == "1"))
                            userAccessEntities.Remove(userAccessEntities.Where(x => x.DomainName.Equals("Pitti")).FirstOrDefault());
                    }
                    if (userAccessEntities.Any(x => x.DomainName.Equals("Denso Module")))
                    {
                        if (!(System.Web.Configuration.WebConfigurationManager.AppSettings["DensoPages"].ToString() == "1"))
                            userAccessEntities.Remove(userAccessEntities.Where(x => x.DomainName.Equals("Denso Module")).FirstOrDefault());
                    }
                    if (userAccessEntities.Any(x => x.DomainName.Equals("GEA")) || userAccessEntities.Any(x => x.DomainName.Equals("NonMachining-EshopX", StringComparison.OrdinalIgnoreCase)) || userAccessEntities.Any(x => x.DomainName.Equals("Process", StringComparison.OrdinalIgnoreCase)) || userAccessEntities.Any(x => x.DomainName.Equals("Report Role", StringComparison.OrdinalIgnoreCase)))
                    {
                        if (!(System.Web.Configuration.WebConfigurationManager.AppSettings["GEAPages"].ToString() == "1"))
                        {
                            userAccessEntities.Remove(userAccessEntities.Where(x => x.DomainName.Equals("GEA")).FirstOrDefault());
                            userAccessEntities.Remove(userAccessEntities.Where(x => x.DomainName.Equals("NonMachining-EshopX")).FirstOrDefault());
                            userAccessEntities.Remove(userAccessEntities.Where(x => x.DomainName.Equals("Process")).FirstOrDefault());
                            userAccessEntities.Remove(userAccessEntities.Where(x => x.DomainName.Equals("Report Role")).FirstOrDefault());
                        }
                    }
                    if (userAccessEntities.Any(x => x.DomainName.Equals("KTA")))
                    {
                        if (!(System.Web.Configuration.WebConfigurationManager.AppSettings["KTASpindlePages"].ToString() == "1"))
                            userAccessEntities.Remove(userAccessEntities.Where(x => x.DomainName.Equals("KTA")).FirstOrDefault());
                    }
                    if (userAccessEntities.Any(x => x.DomainName.Equals("Machine Connect Web")))
                    {
                        if (!(System.Web.Configuration.WebConfigurationManager.AppSettings["MachineConnectPages"].ToString() == "1"))
                            userAccessEntities.Remove(userAccessEntities.Where(x => x.DomainName.Equals("Machine Connect Web")).FirstOrDefault());
                    }
                    if (userAccessEntities.Any(x => x.DomainName.Equals("VED")))
                    {
                        if (!(System.Web.Configuration.WebConfigurationManager.AppSettings["VEDIndustries"].ToString() == "1"))
                            userAccessEntities.Remove(userAccessEntities.Where(x => x.DomainName.Equals("VED")).FirstOrDefault());
                    }
                }
                lstUserAccess.DataSource = userAccessEntities;
                lstUserAccess.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void chkmenu_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = sender as CheckBox;
                bool checkedall = true;
                foreach (ListViewItem items in lstUserAccess.Items)
                {
                    CheckBox lst = items.FindControl("chkmenu") as CheckBox;
                    ListView lstv = items.FindControl("lstview1") as ListView;
                    if (lst.Text.Equals(chk.Text))
                    {
                        foreach (ListViewItem item in lstv.Items)
                        {
                            string menuname = (item.FindControl("hidmenuname") as HiddenField).Value;
                            lst = item.FindControl("chkTable") as CheckBox;
                            lst.Checked = chk.Checked;
                        }
                    }
                    if (checkedall)
                    {
                        checkedall = lst.Checked;
                    }

                }
                chkSelectAll.Checked = checkedall;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void chkTable_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = sender as CheckBox;
                foreach (ListViewItem items in lstUserAccess.Items)
                {
                    CheckBox lst = items.FindControl("chkmenu") as CheckBox;
                    ListView lstv = items.FindControl("lstview1") as ListView;
                    bool allchecked = true;
                    foreach (ListViewItem item in lstv.Items)
                    {
                        CheckBox chk2 = (item.FindControl("chkTable") as CheckBox);
                        string menuname = (item.FindControl("hidmenuname") as HiddenField).Value;
                        if (lst.Text == menuname)
                        {
                            if (allchecked)
                            {
                                allchecked = chk2.Checked;
                            }
                        }
                    }
                    lst.Checked = allchecked;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem items in lstUserAccess.Items)
                {
                    CheckBox lst = items.FindControl("chkmenu") as CheckBox;
                    lst.Checked = chkSelectAll.Checked;
                    ListView lstv = items.FindControl("lstview1") as ListView;
                    foreach (ListViewItem item in lstv.Items)
                    {

                        lst = item.FindControl("chkTable") as CheckBox;
                        lst.Checked = chkSelectAll.Checked;

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string successFailure = "";
            //BindCockpitView.DeleteDataUserAccessRights(cmbUserID.SelectedValue.ToString(), out successFailure);
            try
            {
                foreach (ListViewItem items in lstUserAccess.Items)
                {
                    ListView lstv = items.FindControl("lstview1") as ListView;
                    int i = 0;
                    foreach (ListViewItem item in lstv.Items)
                    {

                        string menuname = (item.FindControl("HiddenField2") as HiddenField).Value;
                        if (i == 0)
                        {
                            BindCockpitView.DeleteDataUserAccessRights(cmbUserID.SelectedValue.ToString(), menuname, out successFailure);
                            i++;
                        }
                        CheckBox lst = (item.FindControl("chkTable") as CheckBox);
                        bool checkedlst = lst.Checked;
                        string code = (item.FindControl("HiddenField1") as HiddenField).Value;
                        string Checklistname = lst.Text;
                        if (checkedlst)
                            BindCockpitView.InsertDataUserAccessRights(menuname, code, cmbUserID.SelectedValue.ToString(), out successFailure);
                        if (successFailure.Equals("Successfull"))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('UserAccess Saved Successfully')", true);
                            Session["UserAccessData"] = null;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(txtPassword.Text))
                    BindCockpitView.InsertAsadmin(cmbUserID.SelectedValue.ToString(), chkAdmin.Checked, txtPassword.Text);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            BindEmployee();
            BindGrid();
        }

        protected void cmbUserID_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetPasswordAndAdminInfo();
            BindGrid();
        }

        protected void btnPageSave_Click(object sender, EventArgs e)
        {
            try
            {
                string userid = cmbUserID.SelectedItem.Text;
                string password = txtPassword.Text;
                Models.DataBaseAccess.SaveDashboardDetails(userid, ddlPages.SelectedValue);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }

    public class UserAccessEntity
    {
        public string MenuName { get; set; }
        public bool Visibility { get; set; }
        public bool MenuCheck { get; set; }
        public string DomainName { get; set; }
        public List<SubData> Values { get; set; }
    }

    public class SubData
    {
        public string TableValueName { get; set; }
        public bool TableValueChecked { get; set; }
        public string MenuName { get; set; }
        public string MenuCodeName { get; set; }
        public string TextValueCode { get; set; }
    }

}