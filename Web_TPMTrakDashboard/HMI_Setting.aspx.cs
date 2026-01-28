using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Media;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class HMI_Setting : System.Web.UI.Page
    {
        static List<HMIMachine> machines = new List<HMIMachine>();
        static HMIMachine _selectedMachine = null;
        TcpClient tcpClient = null;
        ModbusIpMaster master = default(ModbusIpMaster);
        static AdminSetting adminSetting = new AdminSetting();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachineDDL();
                btnView_Click(null, EventArgs.Empty);
            }
        }

        private void BindMachineDDL()
        {
            machines = DataBaseAccess.GetTPMTrakEnabledMachines();

            ddlMachine.DataSource = machines.Select(x => x.MachineID).ToList();
            ddlMachine.DataBind();
            ddlMachine.SelectedIndex = 0;
        }

        protected void btnBCD_Click(object sender, EventArgs e)
        {
            if (lblError.Text != string.Empty) return;
            if (sender is Button)
            {
                var btn = sender as Button;
                var setting = btn.ID;
                var param = btn.Text;
                switch (setting)
                {
                    case "btnBCD":
                        btn.Text = param = param.Equals("ENABLE") ? "DISABLE" : "ENABLE";
                        adminSetting.BCD = param.Equals("ENABLE") ? true : false;
                        btn.BackColor = adminSetting.BCD ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                        break;
                    case "btnICD":
                        btn.Text = param = param.Equals("ENABLE") ? "DISABLE" : "ENABLE";

                        adminSetting.ICD = param.Equals("ENABLE") ? true : false;
                        btn.BackColor = adminSetting.ICD ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                        break;
                    case "btnScheduleBypass":
                        btn.Text = param = param.Equals("ENABLE") ? "DISABLE" : "ENABLE";

                        adminSetting.ScheduleBypass = param.Equals("ENABLE") ? true : false;
                        btn.BackColor = adminSetting.ScheduleBypass ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                        break;
                    case "btnShiftAlarm":
                        btn.Text = param = param.Equals("ENABLE") ? "DISABLE" : "ENABLE";

                        adminSetting.ShiftAlarm = param.Equals("ENABLE") ? true : false;
                        btn.BackColor = adminSetting.ShiftAlarm ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                        break;
                    case "btnEmpIDValidBypass":
                        btn.Text = param = param.Equals("ENABLE") ? "DISABLE" : "ENABLE";

                        adminSetting.EmpIDValidBypass = param.Equals("ENABLE") ? true : false;
                        btn.BackColor = adminSetting.EmpIDValidBypass ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                        break;
                    case "btnScheduleMode":
                        btn.Text = param = param.Equals("AUTO") ? "MANUAL" : "AUTO";

                        adminSetting.ScheduleMode = param.Equals("AUTO") ? true : false;
                        btn.BackColor = adminSetting.ScheduleMode ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                        break;
                    case "btnRework":
                        btn.Text = param = param.Equals("ENABLE") ? "DISABLE" : "ENABLE";

                        adminSetting.Rework = param.Equals("ENABLE") ? true : false;
                        btn.BackColor = adminSetting.Rework ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                        break;

                }

            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lblError.Text != string.Empty) return;
            if (_selectedMachine != null)
            {
                if (Pinging(_selectedMachine.IpAddress, _selectedMachine.PortNo))
                {
                    try
                    {
                        if (master == null)
                        {
                            tcpClient = new TcpClient(_selectedMachine.IpAddress, _selectedMachine.PortNo);  //Port no is always 502
                            tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                            master = ModbusIpMaster.CreateIp(tcpClient);
                            master.Transport.Retries = 4;
                            master.Transport.ReadTimeout = 4000;
                            master.Transport.WriteTimeout = 4000;
                            master.Transport.WaitToRetryMilliseconds = 2000;
                        }

                        if (master != null)
                        {
                            WriteAdminSetting();
                            ReadAdminSetting();
                            InitializeSettings();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Admin Settings Updated Successfully')", true);
                            //lblError.Attributes.Add("visibility", "collapse");
                            lblError.Text = string.Empty;
                        }

                    }
                    catch (Exception)
                    {
                        if (master != null)
                        {
                            master.Dispose();
                            master = null;
                        }
                        if (tcpClient != null)
                        {
                            tcpClient.Close();
                            tcpClient = null;
                        }
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Failed To Update Admin Settings')", true);
                        SetSettingsGUI(false);

                    }
                    finally
                    {
                        if (master != null)
                        {
                            master.Dispose();
                            master = null;
                        }
                        if (tcpClient != null)
                        {
                            tcpClient.Close();
                            tcpClient = null;
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", $"alert('Failed To Connect PLC. {_selectedMachine.IpAddress}:{_selectedMachine.PortNo}')", true);

                    lblError.Text = $"Error: Failed To Connect PLC - {_selectedMachine.MachineID}({_selectedMachine.IpAddress}:{_selectedMachine.PortNo})";
                    //lblError.Attributes.Add("visibility", "visible");
                    SetSettingsGUI(false);

                }
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            _selectedMachine = machines.Where(x => x.MachineID.Equals(ddlMachine.SelectedValue)).FirstOrDefault();

            if (_selectedMachine != null)
            {
                if (Pinging(_selectedMachine.IpAddress, _selectedMachine.PortNo))
                {
                    try
                    {
                        if (master == null)
                        {
                            tcpClient = new TcpClient(_selectedMachine.IpAddress, _selectedMachine.PortNo);  //Port no is always 502
                            tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                            master = ModbusIpMaster.CreateIp(tcpClient);
                            master.Transport.Retries = 4;
                            master.Transport.ReadTimeout = 4000;
                            master.Transport.WriteTimeout = 4000;
                            master.Transport.WaitToRetryMilliseconds = 2000;
                        }

                        if (master != null)
                        {
                            ReadAdminSetting();
                            ReadOldPassword();
                            InitializeSettings();
                            HMIheading.InnerText = $"HMI Setting ({_selectedMachine.MachineID})";
                            //HMIheading.Attributes.Remove("value");
                            //HMIheading.Attributes.Add("value", $"HMI Setting ({_selectedMachine.MachineID})");
                            //lblError.Attributes.Remove("value");
                            //lblError.Attributes.Add("value","");
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Admin Settings Fetched Successfully')", true);
                            //lblError.Attributes.Add("visibility", "collapse");
                            lblError.Text = string.Empty;
                            SettingPanel.Visible = true;

                        }

                    }
                    catch (Exception ex)
                    {
                        if (master != null)
                        {
                            master.Dispose();
                            master = null;
                        }
                        if (tcpClient != null)
                        {
                            tcpClient.Close();
                            tcpClient = null;
                        }
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Failed To Fetch Admin Settings')", true);
                        lblError.Text = $"{DateTime.Now:dd-MMM-yyyy HH:mm:ss}: Failed To Fetch Admin Settings - {ex.Message}({_selectedMachine.IpAddress}:{_selectedMachine.PortNo})";
                        SetSettingsGUI(false);

                        //lblError.Attributes.Add("visibility", "visible");
                    }
                    finally
                    {
                        if (master != null)
                        {
                            master.Dispose();
                            master = null;
                        }
                        if (tcpClient != null)
                        {
                            tcpClient.Close();
                            tcpClient = null;
                        }

                    }
                }
                else
                {
                    HMIheading.InnerText = $"HMI Setting";
                    lblError.Text = $"{DateTime.Now:dd-MMM-yyyy HH:mm:ss}: Failed To Connect PLC - {_selectedMachine.MachineID}({_selectedMachine.IpAddress}:{_selectedMachine.PortNo})";

                    //lblError.Attributes.Add("visibility", "visible");
                    //lblError.Attributes.Remove("value");
                    //lblError.Attributes.Add("value", $"{DateTime.Now:dd-MMM-yyyy HH:mm:ss}: Failed To Connect PLC - {_selectedMachine.MachineID}({_selectedMachine.IpAddress}:{_selectedMachine.PortNo})");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", $"alert('Failed To Connect PLC. {_selectedMachine.IpAddress}:{_selectedMachine.PortNo}')", true);
                    SetSettingsGUI(false);
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closeLoading", "closeLoading();", true);
            }
        }

        private void SetSettingsGUI(bool status)
        {
            if (status)
            {
                btnBCD.Enabled = status;
                btnICD.Enabled = status;
                btnShiftAlarm.Enabled = status;
                btnScheduleBypass.Enabled = status;
                btnScheduleMode.Enabled = status;
                btnRework.Enabled = status;
                btnEmpIDValidBypass.Enabled = status;
                txtBCDThreshold.Enabled = status;
                txtICDThreshold.Enabled = status;
                txtReworkPercent.Enabled = status;
                btnUpdate.Enabled = status;

                txtOldPassword.Enabled = status;
                txtNewPassword.Enabled = status;
                txtConfirmPassword.Enabled = status;
                btnUpdatePassword.Enabled = status;
                return;
            }

            btnBCD.Enabled = false;
            btnICD.Enabled = false;
            btnShiftAlarm.Enabled = false;
            btnScheduleBypass.Enabled = false;
            btnScheduleMode.Enabled = false;
            btnRework.Enabled = false;
            btnEmpIDValidBypass.Enabled = false;
            txtBCDThreshold.Enabled = false;
            txtICDThreshold.Enabled = false;
            txtReworkPercent.Enabled = false;
            btnUpdate.Enabled = false;

            btnBCD.BackColor = System.Drawing.Color.Gray;
            btnICD.BackColor = System.Drawing.Color.Gray;
            btnShiftAlarm.BackColor = System.Drawing.Color.Gray;
            btnScheduleBypass.BackColor = System.Drawing.Color.Gray;
            btnScheduleMode.BackColor = System.Drawing.Color.Gray;
            btnRework.BackColor = System.Drawing.Color.Gray;
            btnEmpIDValidBypass.BackColor = System.Drawing.Color.Gray;

            txtOldPassword.Enabled = false;
            txtNewPassword.Enabled = false;
            txtConfirmPassword.Enabled = false;
            btnUpdatePassword.Enabled = false;
        }

        private void ReadAdminSetting()
        {
            adminSetting.BCD = master.ReadHoldingRegisters((ushort)1411, (ushort)1)[0] == 1;
            adminSetting.ICD = master.ReadHoldingRegisters((ushort)1413, (ushort)1)[0] == 1;
            adminSetting.ShiftAlarm = master.ReadHoldingRegisters((ushort)1415, (ushort)1)[0] == 1;
            adminSetting.ScheduleBypass = master.ReadHoldingRegisters((ushort)1416, (ushort)1)[0] == 1;
            adminSetting.EmpIDValidBypass = master.ReadHoldingRegisters((ushort)1417, (ushort)1)[0] == 1;
            adminSetting.ScheduleMode = master.ReadHoldingRegisters((ushort)1418, (ushort)1)[0] == 1;
            adminSetting.Rework = master.ReadHoldingRegisters((ushort)1419, (ushort)1)[0] == 1;

            adminSetting.BCDThreshold = master.ReadHoldingRegisters((ushort)1412, (ushort)1)[0];
            adminSetting.ICDThreshold = master.ReadHoldingRegisters((ushort)1414, (ushort)1)[0];
            adminSetting.ReworkPercent = GetFloat(master.ReadHoldingRegisters((ushort)1420, (ushort)2));
        }
        private void ReadOldPassword()
        {
            adminSetting.oldPassword = master.ReadHoldingRegisters((ushort)1402, (ushort)1)[0];
        }
        private void WriteAdminSetting()
        {
            master.WriteSingleRegister((ushort)1411, adminSetting.BCD ? (ushort)1 : (ushort)2);
            master.WriteSingleRegister((ushort)1413, adminSetting.ICD ? (ushort)1 : (ushort)2);
            master.WriteSingleRegister((ushort)1415, adminSetting.ShiftAlarm ? (ushort)1 : (ushort)2);
            master.WriteSingleRegister((ushort)1416, adminSetting.ScheduleBypass ? (ushort)1 : (ushort)2);
            master.WriteSingleRegister((ushort)1417, adminSetting.EmpIDValidBypass ? (ushort)1 : (ushort)2);
            master.WriteSingleRegister((ushort)1418, adminSetting.ScheduleMode ? (ushort)1 : (ushort)2);
            master.WriteSingleRegister((ushort)1419, adminSetting.Rework ? (ushort)1 : (ushort)2);

            master.WriteSingleRegister((ushort)1412, (ushort)adminSetting.BCDThreshold);
            master.WriteSingleRegister((ushort)1414, (ushort)adminSetting.ICDThreshold);

            master.WriteMultipleRegisters((ushort)1420, floatToUshortArray(adminSetting.ReworkPercent));

            master.WriteSingleRegister((ushort)1410, (ushort)1);


        }
        private void WriteNewPassword()
        {
            master.WriteSingleRegister((ushort)1401, adminSetting.NewPassword);

            master.WriteSingleRegister((ushort)1400, (ushort)1);

        }

        private void InitializeSettings()
        {
            SetSettingsGUI(true);
            txtBCDThreshold.Text = adminSetting.BCDThreshold.ToString();
            txtICDThreshold.Text = adminSetting.ICDThreshold.ToString();
            txtReworkPercent.Text = adminSetting.ReworkPercent.ToString();
            txtOldPassword.Text = adminSetting.oldPassword.ToString();
            //txtOldPassword.Attributes.Add("value", adminSetting.oldPassword.ToString());

            btnBCD.Text = adminSetting.BCD ? "ENABLE" : "DISABLE";
            btnICD.Text = adminSetting.ICD ? "ENABLE" : "DISABLE";
            btnShiftAlarm.Text = adminSetting.ShiftAlarm ? "ENABLE" : "DISABLE";
            btnScheduleBypass.Text = adminSetting.ScheduleBypass ? "ENABLE" : "DISABLE";
            btnEmpIDValidBypass.Text = adminSetting.EmpIDValidBypass ? "ENABLE" : "DISABLE";
            btnScheduleMode.Text = adminSetting.ScheduleMode ? "AUTO" : "MANUAL";
            btnRework.Text = adminSetting.Rework ? "ENABLE" : "DISABLE";


            btnBCD.BackColor = adminSetting.BCD ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            btnICD.BackColor = adminSetting.ICD ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            btnShiftAlarm.BackColor = adminSetting.ShiftAlarm ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            btnScheduleBypass.BackColor = adminSetting.ScheduleBypass ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            btnEmpIDValidBypass.BackColor = adminSetting.EmpIDValidBypass ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            btnScheduleMode.BackColor = adminSetting.ScheduleMode ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            btnRework.BackColor = adminSetting.Rework ? System.Drawing.Color.Green : System.Drawing.Color.Red;
        }

        private bool Pinging(string ipAddress, int portNo)
        {
            bool isSuccess = false;
            Ping netMon = default(Ping);
            try
            {
                netMon = new Ping();
                PingReply reply = netMon.Send(ipAddress, 3000);
                if (reply.Status == IPStatus.Success)
                    isSuccess = true;
                else
                    isSuccess = false;
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return isSuccess;
        }
        public static ushort[] floatToUshortArray(float val)
        {
            ushort[] value = null;
            try
            {
                byte[] bytes = BitConverter.GetBytes(val);
                ushort upper = BitConverter.ToUInt16(bytes, 0);
                ushort lower = BitConverter.ToUInt16(bytes, 2);
                value = new ushort[] { upper, lower };
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return value;
        }
        private float GetFloat(ushort[] input)
        {
            byte[] b = { (byte)(input[0] & 0xff), (byte)(input[0] >> 8), (byte)(input[1] & 0xff), (byte)(input[1] >> 8) };
            return System.BitConverter.ToSingle(b, 0);
        }

        protected void txtThreshold_TextChanged(object sender, EventArgs e)
        {
            if (lblError.Text != string.Empty) return;
            if (sender is TextBox)
            {
                var text = (sender as TextBox).Text;

                if (int.TryParse(text, out int threshold))
                {
                    switch ((sender as TextBox).ID)
                    {
                        case "txtBCDThreshold":
                            adminSetting.BCDThreshold = threshold;
                            break;
                        case "txtICDThreshold":
                            adminSetting.ICDThreshold = threshold;
                            break;
                        case "txtReworkPercent":
                            adminSetting.ReworkPercent = threshold;
                            break;
                    }

                }
                else if (float.TryParse(text, out float percent))
                {
                    switch ((sender as TextBox).ID)
                    {
                        case "txtReworkPercent":
                            adminSetting.ReworkPercent = percent;
                            break;
                    }

                }
                else
                {
                    return;
                }
            }
        }

        protected void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            if (lblError.Text != string.Empty) return;
            if (txtOldPassword.Text==string.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Enter Old Password')", true);
                return;
            }
            if(txtNewPassword.Text == string.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Enter New Password')", true);
                return;
            }
            if(!txtConfirmPassword.Text.Equals(txtNewPassword.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('New Password and Confirm Password Does not Match')", true);
                return;
            }

            //if(ushort.TryParse(txtOldPassword.Text, out ushort oPass))
            //{
            //    if (!oPass.Equals(adminSetting.oldPassword))
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Old Password Mismatch, Can not change password')", true);
            //        return;
            //    }
            //    adminSetting.oldPassword = oPass;
            //}
            

            _selectedMachine = machines.Where(x => x.MachineID.Equals(ddlMachine.SelectedValue)).FirstOrDefault();

            if (_selectedMachine != null)
            {
                if (Pinging(_selectedMachine.IpAddress, _selectedMachine.PortNo))
                {
                    try
                    {
                        if (master == null)
                        {
                            tcpClient = new TcpClient(_selectedMachine.IpAddress, _selectedMachine.PortNo);  //Port no is always 502
                            tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                            master = ModbusIpMaster.CreateIp(tcpClient);
                            master.Transport.Retries = 4;
                            master.Transport.ReadTimeout = 4000;
                            master.Transport.WriteTimeout = 4000;
                            master.Transport.WaitToRetryMilliseconds = 2000;
                        }

                        if (master != null)
                        {
                            WriteNewPassword();
                            ReadOldPassword();
                            txtOldPassword.Text = adminSetting.NewPassword.ToString();
                            //txtOldPassword.Attributes.Add("value", adminSetting.NewPassword.ToString());
                            //txtOldPassword.TextMode = TextBoxMode.Password;
                            txtNewPassword.Text = string.Empty;
                            txtConfirmPassword.Text = string.Empty;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Password Updated Successfully')", true);
                            lblError.Text = string.Empty;
                            SetSettingsGUI(true);


                        }

                    }
                    catch (Exception ex)
                    {
                        if (master != null)
                        {
                            master.Dispose();
                            master = null;
                        }
                        if (tcpClient != null)
                        {
                            tcpClient.Close();
                            tcpClient = null;
                        }
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Failed To Update Password')", true);
                        lblError.Text = $"{DateTime.Now:dd-MMM-yyyy HH:mm:ss}: Failed To Update Password ({_selectedMachine.IpAddress}:{_selectedMachine.PortNo}) - {ex.Message}";
                        //lblError.Attributes.Add("visibility", "visible");
                        SetSettingsGUI(false);

                    }
                    finally
                    {
                        if (master != null)
                        {
                            master.Dispose();
                            master = null;
                        }
                        if (tcpClient != null)
                        {
                            tcpClient.Close();
                            tcpClient = null;
                        }

                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", $"alert('Failed To Connect PLC. {_selectedMachine.IpAddress}:{_selectedMachine.PortNo})", true);
                    lblError.Text = $"Error: Failed To Connect PLC - {_selectedMachine.MachineID}({_selectedMachine.IpAddress}:{_selectedMachine.PortNo})";
                    SetSettingsGUI(false);
                    //lblError.Attributes.Add("visibility", "visible");
                }
            }
        }

        protected void txtNewPassword_TextChanged(object sender, EventArgs e)
        {
            adminSetting.NewPassword = ushort.Parse(txtNewPassword.Text);
        }
    }

    public class HMIMachine
    {
        public string MachineName { get; set; }
        public string MachineID { get; set; }
        public string IpAddress { get; set; }
        public int PortNo { get; set; }

    }

    public class AdminSetting
    {
        public bool BCD { get; set; } = false;
        public int BCDThreshold { get; set; } = 0;
        public bool ICD { get; set; } = false;
        public int ICDThreshold { get; set; } = 0;
        public bool ShiftAlarm { get; set; } = false;
        public bool ScheduleBypass { get; set; } = false;
        public bool EmpIDValidBypass { get; set; } = false;
        public bool ScheduleMode { get; set; } = false;
        public bool Rework { get; set; } = false;
        public float ReworkPercent { get; set; } = 0.0f;

        public ushort oldPassword { get; set; }
        public ushort NewPassword { get; set; }
    }

}