using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.AlertModule
{
    public partial class SMSSettings : System.Web.UI.Page
    {

        bool isSuccessfull = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(IsPostBack))
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                else
                    SessionClear.ClearSession();
                SMSAPISettings("View");
                TelegramSettings("View");
                MobileSettings("View");
                EmailSettings("View");
                COMPortSettings("View");
                BindPortNums(); 
                //cmbEmailMethod.SelectedIndex = 0;
            }
        }

        private void Binddata()
        {
            string SMSPortNo = string.Empty;
            string SMSSettings = string.Empty;
            string SMSFlowControl = string.Empty;
            DataBaseAccess.GetSMSSettings(out SMSPortNo, out SMSSettings, out SMSFlowControl);
            ddlCOMPortNo.SelectedValue = SMSPortNo;
            
            List<string> baudratelst = new List<string>(){
                                       "110",
                                       "300",
                                       "600",
                                       "1200",
                                       "2400",
                                       "4800",
                                       "9600",
                                       "14400",
                                       "19200",
                                       "28800",
                                       "38400",
                                       "56000",
                                       "57600",
                                       "115200",
                                       "128000",
                                       "256000"
                                   };
            List<string> opt2 = new List<string>() { "Even", "Odd", "None", "Mark", "Space" };
            List<string> opt3 = new List<string>() { "4", "5", "6", "7", "8" };
            List<string> opt4 = new List<string>() { "1", "1.5", "2" };

            List<string> flowControl = new List<string>() { "Xon/Xoff", "Hardware", "None" };
            List<string> smsMethod = new List<string>() { "GSM Modem", "Internet Gateway", "Telegram", "No SMS" };

            ddlFlowControl.SelectedIndex = flowControl.IndexOf(SMSFlowControl);

            string[] slst = SMSSettings.Split(',');
            ddlSettingsVal1.SelectedIndex = baudratelst.IndexOf(slst[0]);
            ddlSettingsVal2.SelectedIndex = opt2.IndexOf(slst[1]);
            ddlSettingsVal3.SelectedIndex = opt3.IndexOf(slst[2]);
            ddlSettingsVal4.SelectedIndex = opt4.IndexOf(slst[3]);


        }

        private void COMPortSettings(string Param)
        {
            try
            {
                switch (Param)
                {
                    case "View":
                        Binddata();
                        DataBaseAccess.GetGSMModemSettings(out bool Enabled);
                        chkGSMModem.Checked = Enabled;

                        break;
                    case "Save":
                        DataBaseAccess.SaveGSMSetting(chkGSMModem.Checked);
                        if(chkGSMModem.Checked)
                        {
                            DataBaseAccess.InsertSMSMethod("SMSMethod", "GSM MODEM", txtApiLink.Text.ToString(), out isSuccessfull);
                        }
                        else
                            DataBaseAccess.InsertSMSMethod("SMSMethod", "", txtApiLink.Text.ToString(), out isSuccessfull);
                        string settings = String.Join(",", new string[] { ddlSettingsVal1.SelectedValue.ToString(), ddlSettingsVal2.SelectedValue.ToString().ToString(), ddlSettingsVal3.SelectedValue.ToString(), ddlSettingsVal4.SelectedValue.ToString() });
                        string portNo = ddlCOMPortNo.SelectedValue.ToString();
                        //Newly Added
                        string flowControl = ddlFlowControl.SelectedValue.ToString();
                        DataBaseAccess.SaveSmsSettings(portNo, flowControl, settings);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        
        [WebMethod]
        public static List<VisibilityEntity> GetDetails()
        {
            List<VisibilityEntity> visibilityEntities = DataBaseAccess.GetAllEnabledBit();
            return visibilityEntities;

        }

        private void BindPortNums()
        {
            List<string> portNums = new List<string>();
            for (int i = 1; i < 16; i++)
            {
                portNums.Add(i.ToString());
            }
            ddlCOMPortNo.DataSource = portNums;
            ddlCOMPortNo.DataBind();
        }

        private void SMSAPISettings(string Param)
        {
            try
            {
                switch (Param)
                {
                    case "View":
                        txtApiLink.Text = DataBaseAccess.GetSMSAPISettings(out bool Enabled);
                        chkApiEnabled.Checked = Enabled;
                        break;
                    case "Save":
                        DataBaseAccess.SaveSMSAPISettings(txtApiLink.Text.Trim(), chkApiEnabled.Checked);

                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void TelegramSettings(string Param)
        {
            try
            {
                switch (Param)
                {
                    case "View":
                        txtTelegramApiLink.Text = DataBaseAccess.GetTelegramAPISettings(out bool Enabled);
                        chkTelegramEnabled.Checked = Enabled;
                        break;
                    case "Save":
                        DataBaseAccess.SaveTelegramAPISettings(txtTelegramApiLink.Text.Trim(), chkTelegramEnabled.Checked);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }


        private void EmailSettings(string Param)
        {
            try
            {
                switch (Param)
                {
                    case "View":
                        chkEnabledEmailSettings.Checked = DataBaseAccess.GETEmailData();
                        string SMTPServer = string.Empty, PortNO = String.Empty, Password = String.Empty, UserID = String.Empty;
                        cmbEmailMethod.SelectedValue = DataBaseAccess.GetEmailServer(cmbEmailMethod.SelectedValue.ToString(), out SMTPServer, out PortNO);
                        txtSMTPSrrver.Text = SMTPServer;
                        txtPort.Text = PortNO;
                        txtUserID.Text = DataBaseAccess.GetMailCredentials(cmbEmailMethod.SelectedValue.ToString(), out Password);
                        txtPassword.Attributes["value"] = Password;
                        chkSSlEnable.Checked = DataBaseAccess.GetMailEnableSSL();
                        cmbEmailMethod_SelectedIndexChanged(null, null);
                        break;
                    case "Save":
                        try
                        {
                            int val = 0;
                            if (string.IsNullOrEmpty(txtSMTPSrrver.Text.ToString()))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Sever Cannot be empty!!!')", true);
                                return;
                            }

                            string Value = cmbEmailMethod.SelectedValue.ToString();
                            if (Value != "SMTP")
                            {
                                if (string.IsNullOrEmpty(txtUserID.Text.ToString()))
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ser ID/Password cannot be empty.')", true);
                                    return;
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(txtPort.Text.ToString()))
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Port Number cannot be empty." + Environment.NewLine + "Possible port number are 25, 587(SSL required).')", true);
                                    return;
                                }
                                if (chkSSlEnable.Checked == true) val = 1;
                                DataBaseAccess.InsertEmailSettings(string.Empty, string.Empty, val, "EmailEnableSSL", out isSuccessfull);
                            }
                            if (string.IsNullOrEmpty(txtPort.Text.ToString())) txtPort.Text = "587";
                            DataBaseAccess.InsertEmailSettings(Value, txtSMTPSrrver.Text.ToString(), Convert.ToInt32(txtPort.Text.ToString()), "EmailServer", out isSuccessfull);
                            DataBaseAccess.InsertEmailSettings(txtUserID.Text.ToString(), txtPassword.Text.ToString(), Convert.ToInt32(txtPort.Text.ToString()), "EmailCredentials", out isSuccessfull);
                            //DataBaseAccess.InsertSMSMethod("SMSMethod", "No SMS", "", out isSuccessfull);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog(ex.ToString());
                            return;
                        }
                        DataBaseAccess.SaveEmailData(chkEnabledEmailSettings.Checked);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }


        private void MobileSettings(string Param)
        {
            try
            {
                switch (Param)
                {
                    case "View":
                        chkMobileEnabled.Checked = DataBaseAccess.GetMobileNotificationPushSettings();
                        break;
                    case "Save":
                        DataBaseAccess.SaveMobileNotificationPushSettings(chkMobileEnabled.Checked);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void BindEmailSettings()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void cmbEmailMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (cmbEmailMethod.SelectedValue.ToString())
                {
                    case "SMTP":
                        chkSSlEnable.Visible = true;
                        lblEnableSSL.Visible = true;
                        lblPorts.Visible = true;
                        txtPort.Visible = true;
                        lblServer.Text = "SMTP Server";
                        break;
                    case "EWS":
                        chkSSlEnable.Visible = false;
                        lblEnableSSL.Visible = false;
                        lblPorts.Visible = false;
                        txtPort.Visible = false;
                        lblServer.Text = "EMS URL";
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        #region OLDCODE

        //    protected void btnSaveAPIAddress_Click(object sender, EventArgs e)
        //    {
        //        string SMSApiAddress = string.Empty; bool isSuccessfull = false;
        //        try
        //        {
        //            SMSApiAddress = txtsmsAPIAddress.Text;
        //            if (string.IsNullOrEmpty(SMSApiAddress))
        //            {
        //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please provide APi address')", true);
        //                txtApiLink.Focus();
        //                return;
        //            }
        //            else
        //            {
        //                DataBaseAccess.InsertSMSMethod("SMSMethod", cmbsmsMethod.SelectedValue.ToString(), SMSApiAddress, out isSuccessfull);

        //                if (isSuccessfull)
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Data Saved')", true);
        //                }
        //            }
        //            BindAPISettings();
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.WriteErrorLog(ex.ToString());
        //        }
        //    }

        //    protected void btnSaveEmailSettings_Click(object sender, EventArgs e)
        //    {
        //        bool isSuccessfull = false;
        //        try
        //        {
        //            int val = 0;
        //            if (string.IsNullOrEmpty(txtSMTPSrrver.Text.ToString()))
        //            {
        //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Provide SMTP Server')", true);
        //                return;
        //            }

        //            string Value = cmbEmailMethod.SelectedValue.ToString();
        //            if (Value != "SMTP")
        //            {
        //                if (string.IsNullOrEmpty(txtUserID.Text.ToString()))
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Provide  Add User-ID')", true);
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                if (string.IsNullOrEmpty(txtSmtpPortNo.Text.ToString()))
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Provide Port No')", true);
        //                    return;
        //                }
        //                if (chkSSlEnable.Checked) val = 1;
        //                DataBaseAccess.InsertEmailSettings(string.Empty, string.Empty, val, "EmailEnableSSL", out isSuccessfull);
        //            }
        //            if (string.IsNullOrEmpty(txtSmtpPortNo.Text.ToString())) txtSmtpPortNo.Text = "587";
        //            DataBaseAccess.InsertEmailSettings(Value, txtSMTPSrrver.Text.ToString(), Convert.ToInt32(txtSmtpPortNo.Text.ToString()), "EmailServer", out isSuccessfull);
        //            DataBaseAccess.InsertEmailSettings(txtUserID.Text.ToString(), txtPassword.Text.ToString(), Convert.ToInt32(txtSmtpPortNo.Text.ToString()), "EmailCredentials", out isSuccessfull);
        //            if (!DataBaseAccess.isEmailMethodExistInSMSMethod(cmbsmsMethod.SelectedValue.ToString() == "NoSMS" ? "No SMS" : cmbsmsMethod.SelectedValue.ToString()))
        //            {
        //                string method = cmbsmsMethod.SelectedValue == "NoSMS" ? "No SMS" : cmbsmsMethod.SelectedValue;
        //                DataBaseAccess.InsertSMSMethod("SMSMethod", method, "", out isSuccessfull);
        //            }
        //            if (isSuccessfull)
        //            {
        //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Data Saved')", true);
        //            }
        //            BindEmailSettings();
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.WriteErrorLog(ex.ToString());
        //        }
        //    }

        //    protected void btnPortSettings_Click(object sender, EventArgs e)
        //    {
        //        bool isSuccessfull = false;
        //        try
        //        {
        //            string settings = String.Join(",", new string[] { cmbbandRate.SelectedValue.ToString(), cmbOpt2.SelectedValue.ToString().ToString(), cmbOpt3.SelectedValue.ToString(), cmbOpt4.SelectedValue.ToString() });

        //            DataBaseAccess.InsertSMSMethod("SMSMethod", cmbsmsMethod.SelectedValue.ToString(), txtsmsAPIAddress.Text.ToString(), out isSuccessfull);
        //            if (!isSuccessfull)
        //            {
        //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Data Not saved')", true);
        //                return;
        //            }
        //            string portNo = cmbPortNo.SelectedItem.ToString();
        //            //Newly Added
        //            string flowControl = cmbFlowControl.SelectedValue.ToString();
        //            DataBaseAccess.SaveSmsSettings(portNo, flowControl, settings);
        //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Data saved Successfully')", true);
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.WriteErrorLog(ex.ToString());
        //        }
        //    }
        //}
        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Saved successfully!!!')", true);
            SMSAPISettings("Save");
            TelegramSettings("Save");
            MobileSettings("Save");
            EmailSettings("Save");
            COMPortSettings("Save");
            SMSAPISettings("View");
            TelegramSettings("View");
            MobileSettings("View");
            EmailSettings("View");
            COMPortSettings("View");
        }
    }
    public class COMPortSettings
    {
        public string PortNo { get; set; }
        public string FlowControl { get; set; }
        public string BandRate { get; set; }
        public string Opt2 { get; set; }
        public string Opt3 { get; set; }
        public string Opt4 { get; set; }
    }
}