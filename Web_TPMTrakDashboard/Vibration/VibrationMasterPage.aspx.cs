using Elmah;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Vibration;

namespace Web_TPMTrakDashboard
{
    public partial class VibrationMasterPage : System.Web.UI.Page
    {
        List<string> MachineidList = new List<string>();
        List<VibrationSettingsData> ListData = new List<VibrationSettingsData>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                Session["VibrationSettingsMachineid"] = null;
                SessionClear.ClearSession();
                ddlMachine.DataSource = getMachineIDList();
                ddlMachine.DataBind();
                ddlMachine_SelectedIndexChanged(null, null);
                bindGrid();
            }
        }
        private List<string> getMachineIDList()
        {
            List<string> MachineidList = new List<string>();
            try
            {
                if (Session["VibrationSettingsMachineid"] == null)
                {
                    MachineidList = DataBaseAccess.getmachineidfromplant("");
                    Session["VibrationSettingsMachineid"] = MachineidList;
                }

                MachineidList = Session["VibrationSettingsMachineid"] as List<string>;
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return MachineidList;
        }
        protected void ddlMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlComponent.DataSource = VibrationDataBaseAccess.GetAllComponentsbyMachine(ddlMachine.SelectedValue == "All" ? "" : ddlMachine.SelectedValue);
                ddlComponent.DataBind();
                if (ddlComponent.Items.Count > 0)
                {
                    ddlComponent.Items.Insert(0, "All");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            bindGrid();
        }
        #region BindGrid
        private void bindGrid()
        {
            try
            {
                List<string> MachineidList = getMachineIDList();
                List<ListItem> parameterList = new List<ListItem>();
                string parameter = "";
                if (ConfigurationManager.AppSettings["VibrationParameterEnable"].Equals("1"))
                {
                    foreach (ListItem item in ddlParameterId.Items)
                    {
                        parameterList.Add(item);
                    }
                    parameter = ddlParameterId.SelectedValue == "All" ? "" : ddlParameterId.SelectedValue;
                }
                ListData = VibrationDataBaseAccess.GetVibrationSettingsData(MachineidList,parameterList,ddlMachine.SelectedValue=="All"?"": ddlMachine.SelectedValue, ddlComponent.SelectedValue == "All" ? "" : ddlComponent.SelectedValue, parameter);
                Session["VibrationSettingsData"] = ListData;
                lstsettings.DataSource = ListData;
                lstsettings.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        #endregion

        #region Add data
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnAdd.Text.Equals("Add", StringComparison.OrdinalIgnoreCase))
                {
                    btnAdd.Text = "Cancel";
                    List<string> MAchineidList = getMachineIDList();
                    if (MAchineidList.Contains("All") == true)
                    {
                        MAchineidList.Remove("All");
                    }
                    //MAchineidList.RemoveAt(0);
                    List<VibrationSettingsData> ListDatanew = Session["VibrationSettingsData"] as List<VibrationSettingsData>;
                    VibrationSettingsData data = new VibrationSettingsData();
                    data.MachineID = "";
                    data.MachineIDList = MAchineidList;
                    List<string> componentList = VibrationDataBaseAccess.GetAllComponentsbyMachine(MAchineidList[0].ToString());
                    //data.CompID = "";
                    //data.CompIDList = componentList;
                    data.DangerUSl = "";
                    data.cmpIDddlVisible = true;
                    data.cmbIDlblVisible = false;
                    data.MachineIDddlVisible = true;
                    data.MachineIDlblVisible = false;
                    data.lblOpVisible = false;
                    data.ParameterddlVisible = true;
                    data.ParameterlblVisible = false;
                    data.WarningUSL = "";
                    data.hidval = "true";
                    ListDatanew.Insert(0, data);
                    lstsettings.DataSource = ListDatanew;
                    lstsettings.DataBind();
                    ddlMachineID_SelectedIndexChanged(null, null);
                }
                else
                {
                    btnAdd.Text = "Add";
                    bindGrid();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        #endregion

        #region delete vibration settings

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            bool delete = false;
            try
            {

                foreach (ListViewItem datalist in lstsettings.Items)
                {

                    bool deletestatus = (datalist.FindControl("deletecheckbox") as CheckBox).Checked;
                    if (deletestatus)
                    {
                        string MachineID = (datalist.FindControl("lblMachineID") as Label).Text;
                        string ComponentID = (datalist.FindControl("lblcmp") as Label).Text;
                        string Operation = (datalist.FindControl("lblOp") as Label).Text;
                        string Parameter = "";
                        if (ConfigurationManager.AppSettings["VibrationParameterEnable"].Equals("1"))
                        {
                            Parameter = (datalist.FindControl("hdnParameterValue") as HiddenField).Value;
                        }
                        delete = VibrationDataBaseAccess.DeleteVibrationSettings(MachineID, ComponentID, Operation, Parameter);

                    }
                }
                if (delete)
                {
                    bindGrid(); btnAdd.Text = "Add";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageDeleted", "messageDeleted();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageUNDelete", "messageUNDelete();", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        #endregion

        #region Save vibration settings

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool saved = false;
            try
            {
                foreach (ListViewItem datalist in lstsettings.Items)
                {
                    string hidval = (datalist.FindControl("hiddedfield") as HiddenField).Value;
                    string MachineID = (datalist.FindControl("lblMachineID") as Label).Text;
                    string ComponentID = (datalist.FindControl("lblcmp") as Label).Text;
                    string Operation = (datalist.FindControl("lblOp") as Label).Text;
                    string WarningUSL = (datalist.FindControl("txtxWarningUSL") as TextBox).Text;
                    string DangerUSL = (datalist.FindControl("txtDangerUSL") as TextBox).Text;
                    string MVvalue = (datalist.FindControl("txtMValue") as TextBox).Text;
                    string NVvalue = (datalist.FindControl("txtNValue") as TextBox).Text;
                    string Parameter = "";
                    if (ConfigurationManager.AppSettings["VibrationParameterEnable"].Equals("1"))
                    {
                        Parameter = (datalist.FindControl("hdnParameterValue") as HiddenField).Value;
                    }
                    if (hidval.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(MachineID))
                        {
                            MachineID = (datalist.FindControl("ddlMachineID") as DropDownList).SelectedValue.ToString();
                            ComponentID = (datalist.FindControl("ddlComponentID") as DropDownList).SelectedValue.ToString();
                            Operation = (datalist.FindControl("txtid") as TextBox).Text;
                            if (ConfigurationManager.AppSettings["VibrationParameterEnable"].Equals("1"))
                            {
                                Parameter = (datalist.FindControl("ddlParameter") as DropDownList).SelectedValue;
                            }
                        }
                        if (!(string.IsNullOrEmpty(MachineID) || string.IsNullOrEmpty(ComponentID) || string.IsNullOrEmpty(Operation) || string.IsNullOrEmpty(DangerUSL) || string.IsNullOrEmpty(WarningUSL)))
                        {
                            ComponentID = ComponentID.Trim(); Operation = Operation.Trim(); WarningUSL = WarningUSL.Trim(); DangerUSL = DangerUSL.Trim(); MVvalue = MVvalue.Trim();
                            saved = VibrationDataBaseAccess.SaveVibrationSettings(MachineID, ComponentID, Operation, WarningUSL, DangerUSL, MVvalue, NVvalue, Parameter);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "alert('Please Fill the data to Save!!');", true);
                            return;
                        }

                    }
                }
                if (saved)
                {
                    bindGrid();
                    btnAdd.Text = "Add";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageSaved", "messageSaved();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageUNSaved", "messageUNSaved();", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        #endregion

        #region Machine selectionchange
        protected void ddlMachineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = sender as DropDownList;
            foreach (ListViewItem data in lstsettings.Items)
            {
                string hidval = (data.FindControl("hiddedfield") as HiddenField).Value;
                string abc = string.Empty;
                if (hidval.Equals("true"))
                {
                    string MachineID = (data.FindControl("lblMachineID") as Label).Text;
                    if (string.IsNullOrEmpty(MachineID))
                    {
                        if (ddl != null)
                            abc = ddl.SelectedValue.ToString();
                        else
                            abc = getMachineIDList()[0].ToString();
                        List<string> componentList = VibrationDataBaseAccess.GetAllComponentsbyMachine(abc);
                        DropDownList ddlcomp = (data.FindControl("ddlComponentID") as DropDownList);
                        TextBox txtOp = (data.FindControl("txtid") as TextBox); txtOp.Visible = true;
                        ddlcomp.Visible = true;
                        ddlcomp.DataSource = componentList;
                        ddlcomp.DataBind();
                    }
                }
            }
        }
        #endregion

     
    }

    #region Vibration Entity
    public class VibrationSettingsData
    {
        public string MachineID { get; set; }
        public List<string> MachineIDList { get; set; }
        public string CompID { get; set; }
        public string OperationID { get; set; }
        public string WarningUSL { get; set; }
        public string DangerUSl { get; set; }
        public string hidval { get; set; }
        public bool MachineIDlblVisible { get; set; }
        public bool cmbIDlblVisible { get; set; }
        public bool lblOpVisible { get; set; }
        public bool cmpIDddlVisible { get; set; }
        public bool MachineIDddlVisible { get; set; }
        public bool ParameterlblVisible { get; set; }
        public bool ParameterddlVisible { get; set; }
        public string MValue { get; set; }
        public string NValue { get; set; }
        public string Parameter { get; set; }
        public string ParameterValue { get; set; }
    }
    #endregion
}