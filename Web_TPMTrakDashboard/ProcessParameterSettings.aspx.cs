using BusinessClassLibrary;
using ModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.ProcessParameter;

namespace Web_TPMTrakDashboard
{
    public partial class ProcessParameterSettings : System.Web.UI.Page
    {
        List<UserAccessModel> useAccessData;
        List<string> DataType = new List<string>();
        List<string> DisplayTemp = new List<string>();
        List<ProcessParaMeterSettingsEntity> ProcessParameterSettingsList = new List<ProcessParaMeterSettingsEntity>();
       
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                SessionClear.ClearSession();
                if (Session["UserAccessData"] == null)
                    Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"] != null ? Session["UserName"].ToString() : "PCT");
                else
                    useAccessData = Session["UserAccessData"] as List<UserAccessModel>;
                
                BindGrid();
            }
        }

        private void BindGrid()
            {
            Session["DataType"] = DataType = new List<string>() { "Boolen", "Int", "Decimal", "Text" };
            Session["DisplayTemp"] = DisplayTemp = new List<string>() { "Yes/No", "Value/Text", "Min/Max" };
            List<string> ParameterID = new List<string> { "P1", "P2", "P3", "P4", "P5", "P6", "P7", "P8" };
            Session["ParameterID"] = ParameterID;
            Session["ProcessParameterSettingsList"] = ProcessParameterSettingsList =ProcessParameterDataBaseAccess.GetProcessParameterSettingsData(DataType, DisplayTemp,ParameterID);
            if (ProcessParameterSettingsList != null && ProcessParameterSettingsList.Count > 0)
            {
                lstviewprocessparameter.DataSource = ProcessParameterSettingsList;
                lstviewprocessparameter.DataBind();
            }
            else
            {
                lstviewprocessparameter.DataSource = new List<ProcessParaMeterSettingsEntity>();
                lstviewprocessparameter.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnAdd.Text.Equals("Add", StringComparison.OrdinalIgnoreCase))
                {
                    btnAdd.Text = "Cancel";
                    if (Session["ProcessParameterSettingsList"] != null)
                        ProcessParameterSettingsList = Session["ProcessParameterSettingsList"] as List<ProcessParaMeterSettingsEntity>;
                    if (ProcessParameterSettingsList != null)
                    {

                        ProcessParaMeterSettingsEntity data = new ProcessParaMeterSettingsEntity();
                        data.Slno = ProcessParameterSettingsList.Count + 1;
                        if (Session["DataType"] != null)
                        {
                            data.DatatypeList = Session["DataType"] as List<string>;
                            data.Datatype = data.DatatypeList[0];
                        }
                        if (Session["DisplayTemp"] != null)
                        {
                            data.DisplayTemplateList = Session["DisplayTemp"] as List<string>;
                            data.DisplayTemplate = data.DisplayTemplateList[0];
                        }
                        if (Session["ParameterID"] != null)
                        {
                            data.ParameterIDList = Session["ParameterID"] as List<string>;
                            data.ParameterID = data.ParameterIDList[0];
                        }
                        data.ShowDatadate = false;
                        data.ShowUnit = false;
                        data.Enable = false;
                        data.MachineID = "";
                        data.Unit = "";
                        data.DisplayHeader = "";
                        data.GreenRange = "";
                        data.YellowHigher = "";
                        data.YellowLower = "";
                        data.RedHigher = "";
                        data.RedLower = "";
                        data.PlcAddress = "";
                        data.DisplayOrder = "";
                        ProcessParameterSettingsList.Insert(0, data);
                        lstviewprocessparameter.DataSource = null;
                        lstviewprocessparameter.DataSource = ProcessParameterSettingsList;

                        lstviewprocessparameter.DataBind();
                    }
                    else
                    {
                        ProcessParaMeterSettingsEntity data = new ProcessParaMeterSettingsEntity();
                        data.Slno = ProcessParameterSettingsList.Count + 1;
                        if (Session["DataType"] != null)
                        {
                            data.DatatypeList = Session["DataType"] as List<string>;
                            data.Datatype = data.DatatypeList[0];
                        }
                        if (Session["DisplayTemp"] != null)
                        {
                            data.DisplayTemplateList = Session["DisplayTemp"] as List<string>;
                            data.DisplayTemplate = data.DisplayTemplateList[0];
                        }
                        if (Session["ParameterID"] != null)
                        {
                            data.ParameterIDList = Session["ParameterID"] as List<string>;
                            data.ParameterID = data.ParameterIDList[0];
                        }
                        data.ShowDatadate = false;
                        data.ShowUnit = false;
                        data.Enable = false;
                        data.MachineID = "";
                        data.Unit = "";
                        data.DisplayHeader = "";
                        data.GreenRange = "";
                        data.YellowHigher = "";
                        data.YellowLower = "";
                        data.RedHigher = "";
                        data.RedLower = "";
                        data.PlcAddress = "";
                        data.DisplayOrder = "";
                        ProcessParameterSettingsList.Insert(0, data);
                        lstviewprocessparameter.DataSource = null;
                        lstviewprocessparameter.DataSource = ProcessParameterSettingsList;
                        lstviewprocessparameter.DataBind();
                    }
                }
                else
                {
                    btnAdd.Text = "Add";
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstviewprocessparameter != null && lstviewprocessparameter.GroupItemCount > 0)
                {
                    foreach (ListViewItem Items in lstviewprocessparameter.Items)
                    {
                        string MachineID = (Items.FindControl("MachineID") as TextBox).Text;
                        string ParameterID = (Items.FindControl("ParameterID") as DropDownList).SelectedValue.ToString();
                        string DisplayHeader = (Items.FindControl("DisplayHeader") as TextBox).Text;
                        string PlcAddress = (Items.FindControl("PlcAddress") as TextBox).Text;
                        string Datatype = (Items.FindControl("Datatype") as DropDownList).SelectedValue.ToString();
                        string Unit = (Items.FindControl("Unit") as TextBox).Text;
                        bool ShowUnit = (Items.FindControl("ShowUnit") as CheckBox).Checked;
                        bool ShowDataDate = (Items.FindControl("ShowDataDate") as CheckBox).Checked;
                        string GreenRange = (Items.FindControl("GreenRange") as TextBox).Text;
                        string YellowHigher = (Items.FindControl("YellowHigher") as TextBox).Text;
                        string YellowLower = (Items.FindControl("YellowLower") as TextBox).Text;
                        string RedHigher = (Items.FindControl("RedHigher") as TextBox).Text;
                        string RedLower = (Items.FindControl("RedLower") as TextBox).Text;
                        string DisplayOrder= (Items.FindControl("DisplayOrder") as TextBox).Text;
                        bool IsEnabled = (Items.FindControl("IsEnabled") as CheckBox).Checked;
                        string DisplayTemplate = (Items.FindControl("DisplayTemplate") as DropDownList).SelectedValue.ToString();
                        ProcessParameterDataBaseAccess.SaveProcessParameter(MachineID, ParameterID, DisplayHeader, PlcAddress, Datatype, Unit, ShowDataDate, ShowUnit, GreenRange, YellowHigher, YellowLower, RedHigher, RedLower, IsEnabled, DisplayOrder, DisplayTemplate);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {

            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstviewprocessparameter != null && lstviewprocessparameter.GroupItemCount > 0)
                {
                    foreach (ListViewItem Items in lstviewprocessparameter.Items)
                    {
                        bool delete = (Items.FindControl("delcheckbox") as CheckBox).Checked;
                       
                        if (delete)
                        {
                            string idd = (Items.FindControl("idd") as HiddenField).Value;
                            ProcessParameterDataBaseAccess.DeletProcessParameterSettings(idd);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {

            }
        }
    }
    public class ProcessParaMeterSettingsEntity
    {
        public int idd { get; set; }
        public int Slno { get; set; }
        public string MachineID { get; set; }
        public string ParameterID { get; set; }
        public List<string> ParameterIDList { get; set; }
        public string DisplayHeader { get; set; }
        public string PlcAddress { get; set; }
        public string Datatype { get; set; }
        public List<string> DatatypeList { get; set; }
        public string Unit { get; set; }
        public bool ShowUnit { get; set; }
        public bool ShowDatadate { get; set; }
        public string GreenRange { get; set; }
        public string YellowHigher { get; set; }
        public string YellowLower { get; set; }
        public string RedHigher { get; set; }
        public string RedLower { get; set; }
        public bool Enable { get; set; }
        public string DisplayOrder { get; set; }
        public string DisplayTemplate { get; set; }
        public List<string> DisplayTemplateList { get; set; }
        
        
    }
}