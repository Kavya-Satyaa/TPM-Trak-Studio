using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Providers.Entities;
using System.Web.UI.WebControls;
using System.Windows.Media.Media3D;
using Web_TPMTrakDashboard.AlertModule.Models;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.ShantiIron;

namespace Web_TPMTrakDashboard.AlertModule
{
    public class DataBaseAccess
    {
        #region AddConsumer

        internal static List<string> GetPlantDetails()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> List = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct Plantid from PlantInformation", conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    List.Add(rdr["Plantid"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return List;
        }

        internal static List<string> GetEmployeeDetails(string PlantID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> List = new List<string>();
            try
            {
                cmd = new SqlCommand("Select distinct EmployeeID from plantemployee where (PlantID=@PlantID or @PlantID='') order by employeeid", conn);
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    List.Add(rdr["EmployeeID"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return List;
        }

        internal static string GetEmployeeDetailsForEmpID(string employeeID, out string phone)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> List = new List<string>();
            string email = "";
            phone = "";
            try
            {
                cmd = new SqlCommand("select * from employeeinformation where Employeeid=@empID", conn);
                cmd.Parameters.AddWithValue("@empID", employeeID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    email = rdr["Email"].ToString();
                    phone = rdr["phone"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return email;
        }

        internal static void SaveGSMSetting(bool enabled)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();

            try
            {
                string sqlQuery = @"if not exists(select ValueInText,ValueInInt from ShopDefaults where Parameter='AlertMethod' and ValueInText='GSMMODEM')
                                        begin
	                                        insert into ShopDefaults(Parameter,ValueInText,ValueInInt,UpdatedTS)
	                                        values('AlertMethod','GSMMODEM',@ValueInInt,@UpdatedTS)
                                        end
                                        else
                                        begin
	                                        update ShopDefaults set ValueInInt=@ValueInInt,UpdatedTS=@UpdatedTS where Parameter='AlertMethod' and ValueInText='GSMMODEM'
                                        end";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@ValueInInt", enabled == true ? 1 : 0);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetPlantIDs: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }

        internal static List<VisibilityEntity> GetAllEnabledBit()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<VisibilityEntity> List = new List<VisibilityEntity>();
            try
            {
               
                cmd = new SqlCommand("select ValueInText,ValueInInt from ShopDefaults where Parameter = 'AlertMethod'", conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    VisibilityEntity entity = new VisibilityEntity();
                    entity.Text = rdr["ValueInText"].ToString();
                    entity.Value = rdr["ValueInInt"].ToString().Equals("1") ? true:false;
                    List.Add(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return List;
        }

        internal static List<AddConsumerEntity> GetConsumerInfo()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            List<AddConsumerEntity> consumerInfoList = new List<AddConsumerEntity>();
            int i = 1;
            try
            {
                string sqlQuery = "Select PlantID, UserID, Email1, Phone1,ChatID from Alert_Consumers";
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    AddConsumerEntity consumerInfo = new AddConsumerEntity();
                    consumerInfo.SLNO = i++;
                    consumerInfo.PlantID = rdr["PlantID"].ToString();
                    consumerInfo.UserID = rdr["UserID"].ToString();
                    consumerInfo.Email = rdr["Email1"].ToString();
                    consumerInfo.Phone = rdr["Phone1"].ToString();
                    consumerInfo.ChatID = rdr["ChatID"].ToString();
                    consumerInfoList.Add(consumerInfo);
                }

                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
            }

            return consumerInfoList;
        }

        internal static void GetGSMModemSettings(out bool enabled)
        {
                SqlConnection sqlConn = ConnectionManager.GetConnection();
                SqlDataReader rdr = null;
                enabled = false;
                try
                {
                    string sqlQuery = "select ValueInText2, ValueInInt from ShopDefaults where Parameter = 'AlertMethod' and ValueInText = 'GSMMODEM'";
                    SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        enabled = rdr["ValueInInt"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase) ? true : false;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog("GetPlantIDs: " + ex.ToString());
                }
                finally
                {
                    if (sqlConn != null) sqlConn.Close();
                    if (rdr != null) rdr.Close();
                }
        }

        internal static void UpdateConsumerInfo(string plantId, string userId, string email, string phone,string chatid, out bool isUpdated)
        {
            isUpdated = false;
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(@"if not  exists(select * from [Alert_Consumers] where  [UserID]=@UserID AND PlantID=@PlantID) 
BEGIN INSERT INTO [dbo].[Alert_Consumers]([PlantID], [UserID], [Email1], [Phone1],[ChatID]) VALUES(@PlantID, @UserID, @Email1, @Phone1,@chatid) END 
ELSE BEGIN update [Alert_Consumers] set [PlantID]=@PlantID, [Email1]=@Email1, [Phone1]=@Phone1, [ChatID]=@chatid where [UserID]=@UserID AND PlantID=@PlantID END", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue(@"PlantID", plantId);
                cmd.Parameters.AddWithValue(@"UserID", userId);
                cmd.Parameters.AddWithValue(@"Email1", email);
                cmd.Parameters.AddWithValue(@"Phone1", phone);
                cmd.Parameters.AddWithValue(@"chatid", chatid);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    isUpdated = true;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
            }
        }

        internal static string GetShiftID(string shiftname)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string ShiftID=string.Empty;
            try
            {
                cmd = new SqlCommand("select distinct shiftid From shiftdetails where Running=1 and ShiftName=@shiftname", conn);
                cmd.Parameters.AddWithValue("@shiftname", shiftname);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ShiftID= rdr["shiftid"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return ShiftID;
        }

        internal static void deleteConsumerInfo(string userID, out bool isSuccessful)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            string query = @"DELETE FROM [Alert_Consumers] where  [UserID]=@UserID";
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@UserID", userID);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    isSuccessful = true;
                }
                else
                {
                    isSuccessful = false;
                }
            }
            catch (Exception)
            {
                isSuccessful = false;
            }
            finally
            {
                if (con != null) con.Close();
            }
        }

        

        #endregion

        #region ShiftAllocation

        internal static List<string> shiftdetail()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> shiftdata = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct ShiftName From  shiftdetails where Running=1", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        shiftdata.Add(rdr["ShiftName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return shiftdata;
        }

        internal static void DeleteAlertShiftAllocation(string date1, string date7, string consumer)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("delete from Alert_UserShiftAllocation where UserID=@UserID and ShiftDate >=@fromdate and ShiftDate<=@todate", conn);
                cmd.Parameters.AddWithValue("@UserID", consumer);
                cmd.Parameters.AddWithValue("@fromdate", date1);
                cmd.Parameters.AddWithValue("@todate", date7);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static List<string> GetShiftID()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> shiftdata = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct shiftid From  shiftdetails where Running=1", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        shiftdata.Add(rdr["shiftid"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return shiftdata;
        }

        internal static DataTable getAlertShiftAllocation(string Plantid, DateTime Fromdate, string param)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                //sqlConn.Open();
                //DateTime dateTime;
                SqlCommand cmd = new SqlCommand("s_Alert_ShiftAllocation", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Plantid", Plantid);

                cmd.Parameters.AddWithValue("@Fromdate", Fromdate.ToString("yyyy-MM-dd HH:mm:ss"));

                cmd.Parameters.AddWithValue("@param", param);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dt.AcceptChanges();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getAlertShiftAllocation: " + ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }


        internal static void SaveAlertUserShiftAllocation(string date, string shiftID, string consumer)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("insert into Alert_UserShiftAllocation(UserID,ShiftDate,ShiftID) values(@UserID,@ShiftDate,@shiftID)", conn);
                cmd.Parameters.AddWithValue("@UserID", consumer);
                cmd.Parameters.AddWithValue("@shiftID", shiftID);
                cmd.Parameters.AddWithValue("@ShiftDate", date);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static DateTime GetUserShiftAllocationDate(DateTime date, string opt, string user)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            DateTime dt = date;
            try
            {
                SqlDataReader sdr = null;
                SqlCommand cmd = null;
                if (opt.Equals("Prev"))
                {
                    cmd = new SqlCommand(@"
select ShiftDate
from (
  SELECT UserID, ShiftDate, ShiftID,
  ROW_NUMBER() OVER (PARTITION BY userid ORDER BY ShiftDate DESC) rn
  FROM Alert_usershiftallocation
  WHERE ShiftDate < @dt AND UserID=@uid
) tmp
where rn=1", conn);
                }
                else
                {
                    cmd = new SqlCommand(@"
select ShiftDate
from (
  SELECT UserID, ShiftDate, ShiftID,
  ROW_NUMBER() OVER (PARTITION BY userid ORDER BY ShiftDate ASC) rn
  FROM Alert_usershiftallocation
  WHERE ShiftDate > @dt AND UserID=@uid
) tmp
where rn=1", conn);
                }
                cmd.Parameters.AddWithValue("@dt", date);
                cmd.Parameters.AddWithValue("@uid", user);

                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    dt = Convert.ToDateTime(sdr["ShiftDate"]);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return dt;
        }

        #endregion

        #region SMSSettings
        internal static COMPortSettings GetCOMPortSettings()
        {
            //SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["TPMConnectionString"].ConnectionString);
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            COMPortSettings entity = new COMPortSettings();
            string SMSSettings_Entity = string.Empty;
            string[] cmbvaluse = null;
            try
            {
                string sqlQuery = "select SMSPortNo, SMSSettings,SMSFlowControl from SmartAgentActions";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    entity.PortNo = rdr["SMSPortNo"].ToString();
                    SMSSettings_Entity = rdr["SMSSettings"].ToString();
                    cmbvaluse = SMSSettings_Entity.Split(',');
                    entity.BandRate = cmbvaluse[0];
                    entity.Opt2 = cmbvaluse[1];
                    entity.Opt3 = cmbvaluse[2];
                    entity.Opt4 = cmbvaluse[3];
                    entity.FlowControl = rdr["SMSFlowControl"].ToString();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetSMSSettings: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return entity;
        }

        //public static string GetEmailServer(string EmailMethod, out string SMTPServer, out string SMSPortNo)
        //{
        //    SqlConnection Con = ConnectionManager.GetConnection();
        //    SqlCommand cmd = null;
        //    SMTPServer = string.Empty; SMSPortNo = string.Empty; string EMailMethod = string.Empty;
        //    SqlDataReader dr = null;
        //    //while (dr == null)
        //    //{
        //    try
        //    {
        //        cmd = new SqlCommand("Select ValueinText,ValueinText2,valueinint from ShopDefaults where Parameter ='EmailServer' and (ValueinText=@valueInText or @valueInText='')", Con);
        //        cmd.Parameters.AddWithValue("@ValueinText", EmailMethod);
        //        dr = cmd.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            EMailMethod = dr["ValueinText"].ToString();
        //            SMTPServer = dr["ValueinText2"].ToString();
        //            SMSPortNo = dr["Valueinint"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.Message);
        //    }
        //    //}
        //    return EmailMethod;
        //}
        public static string GetEmailServer(string EmailMethod, out string SMTPServer, out string SMSPortNo)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SMTPServer = string.Empty; SMSPortNo = string.Empty; string EMailMethod = string.Empty;
            SqlDataReader dr = null;
            //while (dr == null)
            //{
            try
            {
                cmd = new SqlCommand("Select ValueinText,ValueinText2,valueinint from ShopDefaults where Parameter ='EmailServer'", Con);
                cmd.Parameters.AddWithValue("@ValueinText", EmailMethod);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    EMailMethod = dr["ValueinText"].ToString();
                    SMTPServer = dr["ValueinText2"].ToString();
                    SMSPortNo = dr["Valueinint"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            //}
            return EMailMethod;
        }

        public static string GetMailCredentials(string EmailMethod, out string Password)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            Password = string.Empty;
            string UserID = string.Empty;
            SqlCommand cmd = null;


            SqlDataReader sdr = null;

            //while (sdr == null)
            //{
            try
            {
                cmd = new SqlCommand("Select * from ShopDefaults where Parameter ='EmailCredentials' ", Con);
                cmd.Parameters.AddWithValue("@ValueinText", EmailMethod);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    UserID = sdr["ValueInText"].ToString();
                    Password = sdr["ValueInText2"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            //}
            return UserID;
        }

        internal static bool GetMailEnableSSL()
        {

            bool val = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            try
            {
                SqlDataReader sdr = null;
                SqlCommand cmd = new SqlCommand(@"Select top 1 valueinInt from ShopDefaults where Parameter ='EmailEnableSSL' ", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.Text;

                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    if (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["valueinInt"]))
                        {
                            val = Convert.ToInt32(sdr["valueinInt"].ToString()).Equals(0) ? false : true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return val;
        }

        public static string GetSMSAPIAddress(string method,out string SMSMethod)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            string Apitext = string.Empty; SMSMethod = string.Empty;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand("Select * from ShopDefaults where Parameter ='SMSMethod' and ValueInText=@method", Con);
                cmd.Parameters.AddWithValue("@method", method);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Apitext = sdr["ValueInText2"].ToString();
                    SMSMethod = sdr["ValueInText"].ToString();
                }
            }
            catch (Exception ex)
            {

                Logger.WriteErrorLog(ex.Message);
            }
            return Apitext;
        }

        public static void InsertSMSMethod(string SMSMethod, string smsMethoddropdownVal, string smsApiAddress, out bool isSuccessfull)
        {
            isSuccessfull = false;
            SqlConnection Con = null;
            try
            {
                Con = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"if not exists(Select * from ShopDefaults where Parameter = @parameter)
                                                    BEGIN
                                                    insert into Shopdefaults(Parameter,ValueInText,ValueInText2,UpdatedTS)
                                                    Values(@parameter,@ValueInText,@ValueInText2,@UpdatedTS)
                                                    END
                                                    else
                                                    BEGIN
                                                    update Shopdefaults set ValueInText=@ValueInText,ValueInText2=@ValueInText2,UpdatedTS=@UpdatedTS 
                                                    where Parameter=@Parameter
                                                    END ", Con);
                cmd.Parameters.AddWithValue("@ValueinText", smsMethoddropdownVal);
                cmd.Parameters.AddWithValue("@ValueinText2", smsApiAddress);
                cmd.Parameters.AddWithValue("@parameter", SMSMethod);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                isSuccessfull = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SaveSmsSettings: " + ex.ToString());
            }
            finally
            {
                if (Con != null) Con.Close();
            }
        }

        internal static void SaveSmsSettings(string portNo, string flowControl, string settings)
        {
            //SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["TPMConnectionString"].ConnectionString);
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                //sqlConn.Open();
                string sqlQuery = @"select count(*) from SmartAgentActions";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {

                    sqlQuery = @"update SmartAgentActions set SMSPortNo=@smsportno, SMSSettings=@smssettings,SMSFlowControl=@smsflowcontrol";
                    cmd = new SqlCommand(sqlQuery, sqlConn);
                    cmd.Parameters.AddWithValue("@smsportno", portNo);
                    cmd.Parameters.AddWithValue("@smsflowcontrol", flowControl);
                    cmd.Parameters.AddWithValue("@smssettings", settings);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    sqlQuery = @"insert into SmartAgentActions (SMSPortNo, SMSSettings,SMSFlowControl) values (@smsportno, @smssettings,@smsflowcontrol)";
                    cmd = new SqlCommand(sqlQuery, sqlConn);
                    cmd.Parameters.AddWithValue("@smsportno", portNo);
                    cmd.Parameters.AddWithValue("@smsflowcontrol", flowControl);
                    cmd.Parameters.AddWithValue("@smssettings", settings);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                Logger.WriteErrorLog("SaveSmsSettings: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static void GetSMSSettings(out string SMSPortNo, out string SMSSettings, out string SMSFlowControl)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SMSPortNo = "1";
            SMSSettings = "9600,N,8,1";
            SMSFlowControl = "None";
            try
            {
                string sqlQuery = "select SMSPortNo, SMSSettings,SMSFlowControl from SmartAgentActions";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    SMSPortNo = rdr["SMSPortNo"].ToString();
                    SMSSettings = rdr["SMSSettings"].ToString();
                    SMSFlowControl = rdr["SMSFlowControl"].ToString();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetSMSSettings: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }

        public static void InsertEmailSettings(string EmailMethod, string servername, int port, string parameter, out bool isSuccessfull)
        {
            isSuccessfull = false;
            SqlConnection Con = null;
            try
            {

                Con = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"if not exists(Select * from ShopDefaults where Parameter = @parameter)
                                                    BEGIN
                                                    insert into Shopdefaults(Parameter,ValueInText,ValueInText2,valueInInt,UpdatedTS)
                                                    Values(@parameter,@ValueInText,@ValueInText2,@valueInInt,@UpdatedTS)
                                                    END
                                                    else
                                                    BEGIN
                                                    update Shopdefaults set ValueInText=@ValueInText ,ValueInText2=@ValueInText2,ValueInInt=@ValueInInt,UpdatedTS=@UpdatedTS
                                                    where Parameter=@Parameter
                                                    END ", Con);
                cmd.Parameters.AddWithValue("@ValueinText", EmailMethod);
                cmd.Parameters.AddWithValue("@ValueInText2", servername);
                cmd.Parameters.AddWithValue("@ValueInInt", port);
                cmd.Parameters.AddWithValue("@parameter", parameter);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                isSuccessfull = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (Con != null) Con.Close();
            }
        }
        public static bool isEmailMethodExistInSMSMethod(string method)
        {
            bool isEmailMethod = false;
            SqlConnection Con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand("Select * from ShopDefaults where Parameter ='SMSMethod' and ValueInText=@method", Con);
                cmd.Parameters.AddWithValue("@method", method);
                sdr = cmd.ExecuteReader();
                if(sdr.HasRows)
                {
                    isEmailMethod = true;
                }
            }
            catch (Exception ex)
            {

                Logger.WriteErrorLog(ex.Message);
            }
            return isEmailMethod;
        }

        #endregion

        #region SMSStatus

        internal static List<SMSStatusEmtity> GetSMSStatus(DateTime StartDate, DateTime EndDate, ref int NoOfMobileNo, ref int TotalNoofmessage, ref int sum)
        {
            sum = 0;
            List<SMSStatusEmtity> list = new List<SMSStatusEmtity>();
            SqlConnection sqlConn = null;
            SqlDataReader rdr = null; NoOfMobileNo = 0; TotalNoofmessage = 0;
            try
            {
                sqlConn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand("s_GetAlert_Notification_Report", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                rdr = cmd.ExecuteReader();
                cmd.CommandTimeout = 120;
                int x = 1;
                int length = 0;
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        SMSStatusEmtity d = new SMSStatusEmtity();
                        d.DateTime = Convert.ToDateTime(rdr["CreatedTime"].ToString());
                        d.MobileNumber = rdr["MobileNumber"].ToString();
                        d.Message = rdr["Message"].ToString();
                        length = d.Message.Length;
                        int Q = length / 153;
                        int R = length % 153;
                        if (R == 0)
                            d.NoOfMessageSent = Q;
                        else
                            d.NoOfMessageSent = Q + 1;
                        d.SLNO = x++;
                        sum = sum + d.NoOfMessageSent;
                        list.Add(d);
                    }
                }
                rdr.NextResult();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        int.TryParse(rdr["NoOfMobileNumber"].ToString(), out NoOfMobileNo);
                        int.TryParse(rdr["TotalNoOfMessage"].ToString(), out TotalNoofmessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }

        #endregion

        #region --------Define Rule----
        internal static List<DefineRuleDetails> getDefineRuleDetails()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<DefineRuleDetails> defineRuleDetailsList = new List<DefineRuleDetails>();
            DefineRuleDetails defineRule = null;
            try
            {
                cmd = new SqlCommand("select * from [Alert_Rules]", con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        try
                        {
                            defineRule = new DefineRuleDetails();
                            defineRule.RuleId = sdr["RuleID"].ToString();
                            defineRule.Description = sdr["Description"].ToString();
                            defineRule.Parameter = sdr["Parameter"].ToString();
                            defineRule.Compare = sdr["Compare"].ToString();
                            defineRule.Threshold = sdr["Threshold"].ToString();
                            defineRule.ThresholdUnit = sdr["ThresholdUnit"].ToString();
                            defineRule.ThresholdValue = sdr["Threshold"].ToString() + " " + sdr["ThresholdUnit"].ToString();
                            defineRule.EvaluateEvery = sdr["EvaluateEvery"].ToString();
                            defineRule.EvaluateEveryUnit = sdr["EvaluateEveryUnit"].ToString();
                            defineRule.EvaluateEveryValue = sdr["EvaluateEvery"].ToString() + " " + sdr["EvaluateEveryUnit"].ToString();
                            if (!sdr["Enabled"].Equals(DBNull.Value))
                            {
                                defineRule.Enable = Convert.ToBoolean(sdr["Enabled"]);
                            }
                            defineRule.AppliesTo = sdr["AppliesTo"].ToString();
                            if (!sdr["SMSEnabled"].Equals(DBNull.Value))
                            {
                                defineRule.SMSEnable = Convert.ToBoolean(sdr["SMSEnabled"]);
                            }
                            if (!sdr["EmailEnabled"].Equals(DBNull.Value))
                            {
                                defineRule.EmailEnable = Convert.ToBoolean(sdr["EmailEnabled"]);
                            }
                            defineRule.Message = sdr["SMSTextTemplate"].ToString();
                            if (!sdr["TelegramEnabled"].Equals(DBNull.Value))
                            {
                                defineRule.TelegramEnabled = Convert.ToBoolean(sdr["TelegramEnabled"]);
                            }
                            if (!sdr["MobileEnabled"].Equals(DBNull.Value))
                            {
                                defineRule.MobileEnabled = Convert.ToBoolean(sdr["MobileEnabled"]);
                            }
                            defineRuleDetailsList.Add(defineRule);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog("Error in getting Define Rule details - " + ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in getting Define Rule details - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return defineRuleDetailsList;
        }

        internal static int InsertUpdateDefineRuleDetails(DefineRuleDetails data, string param)
        {
            int success = 0;
            string query = "";
            if (param == "Insert")
            {
                query = @"insert into [Alert_Rules]([Description]
      ,[Parameter]
      ,[Compare]
      ,[Threshold]
      ,[ThresholdUnit]
      ,[EvaluateEvery]
      ,[EvaluateEveryUnit]
      ,[Enabled]
      ,[AppliesTo]
      ,[SMSTextTemplate]
      ,[RuleID]
      ,[SMSEnabled]
      ,[EmailEnabled]
      ,[TelegramEnabled]
      ,[MobileEnabled]) Values 
      (@Description
      ,@Parameter
      ,@Compare
      ,@threshold
      ,@Thresholdunit
      ,@EvaluateEvery
      ,@EvaluateEveryUnit
      ,@Enabled
      ,@AppliesTo
      ,@SMSTextTemplate
      ,@RuleID
      ,@SMSEnabled
      ,@EmailEnabled
      ,@TelegramEnable
      ,@MobileEnable)";
            }
            if (param == "Update")
            {
                query = @"update [Alert_Rules] set 
      [Description]=@Description
      ,[Parameter]=@Parameter
      ,[Compare]=@Compare
      ,[Threshold]=@threshold
      ,[ThresholdUnit]=@Thresholdunit
      ,[EvaluateEvery]=@EvaluateEvery
      ,[EvaluateEveryUnit]=@EvaluateEveryUnit
      ,[Enabled]=@Enabled
      ,[AppliesTo]=@AppliesTo
      ,[SMSTextTemplate]=@SMSTextTemplate 
      ,[SMSEnabled]=@SMSEnabled
      ,[EmailEnabled]=@EmailEnabled
      ,[TelegramEnabled]=@TelegramEnable
      ,[MobileEnabled]=@MobileEnable
      where [RuleID]=@RuleID";
            }

            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                cmd.Parameters.AddWithValue(@"RuleID", data.RuleId);
                cmd.Parameters.AddWithValue(@"Description", data.Description);
                cmd.Parameters.AddWithValue(@"Parameter", data.Parameter);
                cmd.Parameters.AddWithValue(@"Compare", data.Compare);
                cmd.Parameters.AddWithValue(@"threshold", data.Threshold);
                cmd.Parameters.AddWithValue(@"ThresholdUnit", data.ThresholdUnit);
                cmd.Parameters.AddWithValue(@"EvaluateEvery", data.EvaluateEvery);
                cmd.Parameters.AddWithValue(@"EvaluateEveryUnit", data.EvaluateEveryUnit);
                cmd.Parameters.AddWithValue(@"Enabled", data.Enable);
                cmd.Parameters.AddWithValue(@"AppliesTo", data.AppliesTo);
                //cmd.Parameters.AddWithValue(@"EmailSubjectTemplate", EmailSubjectTemplate);
                //cmd.Parameters.AddWithValue(@"EmailBodyTemplate", EmailBodyTemplate);
                cmd.Parameters.AddWithValue(@"SMSTextTemplate", data.Message);
                cmd.Parameters.AddWithValue(@"SMSEnabled", data.SMSEnable);
                cmd.Parameters.AddWithValue(@"EmailEnabled", data.EmailEnable);
                cmd.Parameters.AddWithValue(@"TelegramEnable", data.TelegramEnabled);
                cmd.Parameters.AddWithValue(@"MobileEnable", data.MobileEnabled);
                success = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("PRIMARY KEY"))
                {
                    success = -2;
                }
                else
                {
                    success = 0;
                    Logger.WriteErrorLog("While inserting/updating Rule Define details" + ex.ToString());
                }

            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
            return success;
        }
        internal static int deleteRuleId(string ruleid)
        {
            int success = 0;
            string query = @"DELETE FROM Alert_Rules WHERE RuleID=@ruleid";

            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                cmd.Parameters.AddWithValue("@ruleid", ruleid);
                success = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                success = 0;
                Logger.WriteErrorLog("While deleting RuleID" + ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
            return success;
        }
        internal static List<ListItem> getProductionMasterData()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<ListItem> list = new List<ListItem>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"SELECT * from Alert_Defaults where Parameter = 'ProdDetailCol'", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new ListItem() { Value= rdr["ValueInText1"].ToString(),Text= rdr["ValueInText2"].ToString() });
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getProductionMasterData: " + ex.ToString());
                //throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static List<ListItem> getProcessParameterMasterData()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<ListItem> list = new List<ListItem>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"SELECT * from ProcessParameterMaster_BajajIoT where AlertView = 1", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new ListItem() { Value = rdr["ParameterID"].ToString(), Text = rdr["ParameterName"].ToString() });
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getProductionMasterData: " + ex.ToString());
                //throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static List<ListItem> getProductionAndProcessParameterDetailsForRule(string ruleid,string param)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<ListItem> list = new List<ListItem>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"SELECT * from Alert_Defaults where Parameter = @param and ValueInText3 = @ruleid", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@param", param);
                cmd.Parameters.AddWithValue("@ruleid", ruleid);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new ListItem() { Value = rdr["ValueInText1"].ToString(), Text = rdr["ValueInText2"].ToString() });
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getProductionDetailsForRule: " + ex.ToString());
                //throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static void saveProdtionDetailsForRuleID(List<ListItem> list, string ruleid,string param)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                string query = @"DELETE FROM Alert_Defaults WHERE ValueInText3=@ruleid and Parameter=@param";
                SqlCommand cmd;
                cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ruleid", ruleid);
                cmd.Parameters.AddWithValue("@param", param);
                cmd.ExecuteNonQuery();

                foreach (ListItem item in list)
                {
                    query = @"INSERT INTO Alert_Defaults(Parameter,ValueInText1,ValueInText2,ValueInText3) VALUES (@param, @dbval, @displayval, @ruleid)";
                    cmd = new SqlCommand(query, sqlConn);
                    cmd.Parameters.AddWithValue("@dbval", item.Value);
                    cmd.Parameters.AddWithValue("@displayval", item.Text);
                    cmd.Parameters.AddWithValue("@ruleid", ruleid);
                    cmd.Parameters.AddWithValue("@param", param);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("saveProdtionDetailsForRuleID: " + ex.Message.ToString());
            }
            finally
            {
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
            }
        }

        #endregion
        #region ------Map Rules To Machines----
        internal static List<string> GetAllRules()
        {
            List<string> rules = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            try
            {
                cmd = new SqlCommand("SELECT RuleID FROM Alert_Rules", conn);
                cmd.CommandType = CommandType.Text;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rules.Add(reader["RuleID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return rules;
        }
        internal static DataTable GetMapRulesToMachineDetails()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            DataTable dtRuleMachine = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("s_Alert_ViewRulesToMachine", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@param", "View");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dtRuleMachine);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in fetching Emp Station Association data - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return dtRuleMachine;
        }

        internal static void ClearRuleMap() ////
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand("DELETE FROM Alert_AssignRulesToMachine", con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ClearRuleMap: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        internal static void InsertRuleMap(Dictionary<string, List<string>> rule_dct) ////
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                foreach (var d in rule_dct)
                {
                    foreach (var rule in d.Value)
                    {
                        cmd = new SqlCommand("INSERT INTO Alert_AssignRulesToMachine VALUES(@machid, @ruleid)", con);
                        cmd.Parameters.AddWithValue("@machid", d.Key);
                        cmd.Parameters.AddWithValue("@ruleid", rule);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("InsertRuleMap: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        internal static void UpdateAlertMachineMap()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string sqlQuery = @"delete from Alert_AssignRulesToUser
                                where not exists 
                                (
                                select 1 
                                from Alert_AssignRulesToMachine arm
                                where Alert_AssignRulesToUser.machineid = arm.machineid and Alert_AssignRulesToUser.ruleid=arm.ruleid
                                )";

            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("UpdateAlertMachineMap: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        #endregion

        #region ----------------- Map Rule to Consumer -----------------------
        internal static List<string> GetPlantIDs()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> Plants = new List<string>();
            //Plants.Add("All"); // g: test
            try
            {
                //sqlConn.Open();
                //string sqlQuery = "select distinct PlantID from PlantInformation";
                string sqlQuery = "select distinct PlantID from Plantmachine";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Plants.Add(rdr["PlantID"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetPlantIDs: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return Plants;
        }
        internal static List<string> GetMachineid(string Plantid)
        {
            Plantid = Plantid.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Plantid;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> MachineID = new List<string>();
            try
            {
                //sqlConn.Open();
                string sqlQuery = @"select MI.Machineid from machineinformation MI inner join plantMachine PM on MI.Machineid=PM.Machineid where (PM.Plantid= @plantid or @plantid='')";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@plantid", Plantid);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    MachineID.Add(rdr["MachineID"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetMachineid: " + ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return MachineID;
        }
        internal static DataTable getDataForRuleDataTable(string Plantid, string RuleID, string param)
        {
            //SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["TPMConnectionString"].ConnectionString);
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                //sqlConn.Open();
                //SqlCommand cmd = new SqlCommand("Alert_RuleView2", sqlConn); // g
                SqlCommand cmd = new SqlCommand("s_Alert_RuleView", sqlConn); // g
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Plantid", Plantid);
                cmd.Parameters.AddWithValue("@RuleID", RuleID);
                cmd.Parameters.AddWithValue("@param", param);
                SqlDataReader rdr = cmd.ExecuteReader();
                //if (rdr.HasRows)
                //{
                dt.Load(rdr);
                dt.AcceptChanges();
                //}
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getDataForRuleDataTable: " + ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }
        internal static List<string> GetUserid(string plantid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> UserID = new List<string>();
            try
            {
                //sqlConn.Open();
                //string sqlQuery = "select UserID from [Alert_Consumers] order by SlNo ";
                string sqlQuery = "select userid from Alert_Consumers where plantid=@plantid ";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@plantid", plantid);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    UserID.Add(rdr["UserID"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetUserid: " + ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return UserID;
        }
        internal static List<string> GetRule()
        {
            //SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["TPMConnectionString"].ConnectionString);
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> Plants = new List<string>();
            try
            {
                //sqlConn.Open();
                string sqlQuery = "select distinct RuleID from Alert_Rules";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Plants.Add(rdr["RuleID"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetRule: " + ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return Plants;
        }
        internal static DataTable getDataForMachineDataTable(string Plantid, string MachineID, string param)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("s_Alert_MachineView", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@param", param);

                //SqlCommand cmd = new SqlCommand("s_Alert_ConsumerView", sqlConn);
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Plantid", Plantid);
                //cmd.Parameters.AddWithValue("@ConsumerID", MachineID);
                //cmd.Parameters.AddWithValue("@param", param);

                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dt.AcceptChanges();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getDataForMachineDataTable: " + ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }

        internal static DataTable getDataForConsumerDataTable(string Plantid, string ConsumerID, string param)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand("s_Alert_ConsumerView", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Plantid", Plantid);
                cmd.Parameters.AddWithValue("@ConsumerID", ConsumerID);
                cmd.Parameters.AddWithValue("@param", param);

                //SqlCommand cmd = new SqlCommand("s_Alert_MachineView", sqlConn);
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@MachineID", ConsumerID);
                //cmd.Parameters.AddWithValue("@param", param);

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dt.AcceptChanges();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getDataForConsumerDataTable: " + ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }
        internal static void deletedataRuleMachine(string UserID, string MachineID, string RuleID)
        {

            //SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["TPMConnectionString"].ConnectionString);
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                //sqlConn.Open();
                string sqlQuery = string.Empty;
                SqlCommand cmd = null;
                if (!string.IsNullOrEmpty(UserID))
                {
                    sqlQuery = "delete from Alert_AssignRulesToUser where UserID = @userID";
                    cmd = new SqlCommand(sqlQuery, sqlConn);
                    cmd.Parameters.AddWithValue("@userID", UserID);
                }
                else if (!string.IsNullOrEmpty(MachineID))
                {
                    sqlQuery = "delete from Alert_AssignRulesToUser where MachineID = @MachineID";
                    cmd = new SqlCommand(sqlQuery, sqlConn);
                    cmd.Parameters.AddWithValue("@MachineID", MachineID);
                }
                else if (!string.IsNullOrEmpty(RuleID))
                {
                    sqlQuery = "delete from Alert_AssignRulesToUser where RuleID = @RuleID";
                    cmd = new SqlCommand(sqlQuery, sqlConn);
                    cmd.Parameters.AddWithValue("@RuleID", RuleID);
                }
                int x = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("deletedataRuleMachine: " + ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static void insertintoAlert_AssignRulesToUser(string MachineID, string RuleID, string UserID, out bool isSuccessful)
        {
            isSuccessful = false;
            //SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["TPMConnectionString"].ConnectionString);
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                //sqlConn.Open();
                string sqlQuery = @"insert into Alert_AssignRulesToUser(Machineid,RuleID,UserID) Values(@Machineid,@RuleID,@UserID)";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@Machineid", MachineID);
                cmd.Parameters.AddWithValue("@RuleID", RuleID);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                int x = cmd.ExecuteNonQuery();
                if (x > 0)
                {
                    isSuccessful = true;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("insertintoAlert_AssignRulesToUser: " + ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static void deleteAssignRulesToUser(string plantID, string ruleID)
        {
            //plantID = plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID; // g:
            // delete all the rows where machineid belongs to the plantID
            //SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["TPMConnectionString"].ConnectionString);
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                //sqlConn.Open();
                string sqlQuery = string.Empty;
                SqlCommand cmd = null;

                sqlQuery = "delete from Alert_AssignRulesToUser where ruleID = @ruleID and MachineID in ( select machineid from plantmachine where plantID = @plantID or @plantID = '')"; // g:
                cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@plantID", plantID);
                cmd.Parameters.AddWithValue("@ruleID", ruleID);
                int x = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("deleteAssignRulesToUser: " + ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static List<Tuple<string, string>> GetMachRuleID(string plantid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<Tuple<string, string>> lst = new List<Tuple<string, string>>();

            try
            {
                SqlCommand cmd = new SqlCommand("select rm.MachineID, RuleID from Alert_AssignRulesToMachine rm inner join plantmachine pm on rm.machineid = pm.machineid and pm.plantid = @plantid", sqlConn);
                cmd.Parameters.AddWithValue("@plantid", plantid);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string m_id = reader.GetString(0).Trim();
                    string r_id = reader.GetString(1).Trim();
                    lst.Add(Tuple.Create(m_id, r_id));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetMachRuleID: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return lst;
        }
        #endregion

        #region New Alert Settings

        #region SMS Settings
        internal static string GetSMSAPISettings(out bool enabled)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string API = string.Empty;
            SqlDataReader rdr = null;
            enabled = false;
            try
            {
                string sqlQuery = @"select ValueInText2, ValueInInt from ShopDefaults where Parameter = 'AlertMethod' and ValueInText = 'INTERNET GATEWAY'";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    API = (rdr["ValueInText2"].ToString());
                    enabled = rdr["ValueInInt"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase) ? true : false;
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetPlantIDs: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return API;
        }

        internal static void SaveSMSAPISettings(string API, bool enabled)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();

            try
            {
                string sqlQuery = @"if not exists(select ValueInText,ValueInInt from ShopDefaults where Parameter='AlertMethod' and ValueInText='INTERNET GATEWAY')
                                        begin
	                                        insert into ShopDefaults(Parameter,ValueInText,ValueInText2,ValueInInt,UpdatedTS)
	                                        values('AlertMethod','INTERNET GATEWAY',@ValueInText2,@ValueInInt,@UpdatedTS)
                                        end
                                        else
                                        begin
	                                        update ShopDefaults set ValueInText2=@ValueInText2 ,ValueInInt=@ValueInInt,UpdatedTS=@UpdatedTS where Parameter='AlertMethod' and ValueInText='INTERNET GATEWAY'
                                        end";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@ValueInText2", API);
                cmd.Parameters.AddWithValue("@ValueInInt", enabled == true ? 1 : 0);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetPlantIDs: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        #endregion

        #region Telegram Settings
        internal static string GetTelegramAPISettings(out bool enabled)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string API = string.Empty;
            SqlDataReader rdr = null;
            enabled = false;
            try
            {
                string sqlQuery = "select ValueInText2, ValueInInt from ShopDefaults where Parameter = 'AlertMethod' and ValueInText = 'TELEGRAM'";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    API = (rdr["ValueInText2"].ToString());
                    enabled = rdr["ValueInInt"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase) ? true : false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetPlantIDs: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return API;
        }
        internal static void SaveTelegramAPISettings(string API, bool enabled)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();

            try
            {
                string sqlQuery = @"if not exists(select ValueInText,ValueInInt from ShopDefaults where Parameter='AlertMethod' and ValueInText='TELEGRAM')
                                        begin
	                                        insert into ShopDefaults(Parameter,ValueInText,ValueInText2,ValueInInt,UpdatedTS)
	                                        values('AlertMethod','TELEGRAM',@ValueInText2,@ValueInInt,@UpdatedTS)
                                        end
                                        else
                                        begin
	                                        update ShopDefaults set ValueInText2=@ValueInText2 ,ValueInInt=@ValueInInt,UpdatedTS=@UpdatedTS where Parameter='AlertMethod' and ValueInText='TELEGRAM'
                                        end";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@ValueInText2", API);
                cmd.Parameters.AddWithValue("@ValueInInt", enabled == true ? 1 : 0);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetPlantIDs: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        #endregion

        #region MobileSettings
        internal static bool GetMobileNotificationPushSettings()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            bool enabled = false;
            SqlDataReader rdr = null;
            try
            {
                string sqlQuery = "select  ValueInInt from ShopDefaults where Parameter = 'AlertMethod' and ValueInText = 'MOBILE'";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    enabled = rdr["ValueInInt"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase) ? true : false;
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetPlantIDs: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return enabled;
        }

        internal static void SaveMobileNotificationPushSettings(bool Enabled)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                string sqlQuery = @"if not exists(select ValueInText,ValueInInt from ShopDefaults where Parameter='AlertMethod' and ValueInText='MOBILE')
                                        begin
                                            insert into ShopDefaults(Parameter, ValueInText, ValueInText2, ValueInInt,UpdatedTS)

                                            values('AlertMethod', 'MOBILE', @ValueInText2, @ValueInInt,@UpdatedTS)
                                        end
                                        else
                    begin
                        update ShopDefaults set ValueInText2 = @ValueInText2 ,ValueInInt = @ValueInInt,UpdatedTS=@UpdatedTS where Parameter = 'AlertMethod' and ValueInText = 'MOBILE'
                                        end";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@ValueInText2", "");
                cmd.Parameters.AddWithValue("@ValueInInt", Enabled == true ? 1 : 0);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetPlantIDs: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        #endregion

        #region EMAIL Settings
        internal static bool GETEmailData()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            bool enabled = false;
            SqlDataReader rdr = null;
            try
            {
                string sqlQuery = "select  ValueInInt from ShopDefaults where Parameter = 'AlertMethod' and ValueInText = 'EMAIL'";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    enabled = rdr["ValueInInt"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase) ? true : false;
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetPlantIDs: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return enabled;
        }
        internal static void SaveEmailData(bool Enabled)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                string sqlQuery = @"if not exists(select ValueInText,ValueInInt from ShopDefaults where Parameter='AlertMethod' and ValueInText='EMAIL')
                                        begin
                                            insert into ShopDefaults(Parameter, ValueInText, ValueInText2, ValueInInt,UpdatedTS)

                                            values('AlertMethod', 'EMAIL', @ValueInText2, @ValueInInt,@UpdatedTS)
                                        end
                                        else
                    begin
                        update ShopDefaults set ValueInText2 = @ValueInText2 ,ValueInInt = @ValueInInt,UpdatedTS=@UpdatedTS where Parameter = 'AlertMethod' and ValueInText = 'EMAIL'
                                        end";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@ValueInText2", "");
                cmd.Parameters.AddWithValue("@ValueInInt", Enabled == true ? 1 : 0);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetPlantIDs: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        #endregion

        #endregion


        internal static DataTable GetUseraccess(string uid)
        {
            DataTable dt = new DataTable();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {

                cmd = new SqlCommand(@"ss_UserAccessRights", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user", uid);
                cmd.Parameters.AddWithValue("@param", "WebView");
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }

        #region PMCHeckList
        internal static List<PMSettingsEntity> GetPMRuleData(string MachineID)
        {
            List<PMSettingsEntity> EntityList = new List<PMSettingsEntity>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = string.Empty;
            query = @"select * from PM_MachineCategory_Shanti  where Machine=@MachineID";
            try
            {

                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    EntityList.Add(new PMSettingsEntity { Rule = sdr["Category"].ToString(), ID = sdr["ID"].ToString() });
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return EntityList;
        }


        internal static void UpdateDeleteCategory(string machineID, string category, string OldCategory, string type, string IDD)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"";
            switch (type)
            {
                case "Delete":
                    query = @"delete from PM_MachineCategory_Shanti where Machine=@MachineID and Category=@OldCategory";
                    break;
                case "Update":
                    query = @"if not exists (select * from PM_MachineCategory_Shanti where Machine=@MachineID and Category=@OldCategory)
                                begin
                                insert into PM_MachineCategory_Shanti (Machine,Category)
                                values(@MachineID,@Category)
                                end
                                else
                                begin
                                update PM_MachineCategory_Shanti set Category=@Category where Machine=@MachineID and Category=@OldCategory
                                end";
                    break;
            }
            try
            {

                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@ID", IDD);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@OldCategory", OldCategory);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static List<string> getCategory(string MachineID)
        {
            List<string> CategoryList = new List<string>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {

                cmd = new SqlCommand(@"select * from PM_MachineCategory_Shanti  where Machine=@MachineID", conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                    CategoryList.Add(sdr["Category"].ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return CategoryList;
        }

        internal static List<PMSettingsItemEntity> GetPMItemsData(string MachineID, string Category)
        {
            List<PMSettingsItemEntity> EntityList = new List<PMSettingsItemEntity>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = Category.Equals("All", StringComparison.OrdinalIgnoreCase) ? @"select * from PM_Items_Shanti  where Machine=@MachineID" : @"select * from PM_Items_Shanti  where Machine=@MachineID and Category=@Category";


            try
            {

                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Category", Category);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                    EntityList.Add(new PMSettingsItemEntity { Rule = sdr["Category"].ToString(), Items = sdr["Items"].ToString(), ID = sdr["ID"].ToString() });
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return EntityList;
        }


        internal static void UpdateDeleteItems(string machineID, string category, string items, string OldItems, string Type, string IDD)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"";
            switch (Type)
            {
                case "Delete":
                    query = @"Delete from PM_Items_Shanti where Machine=@MachineID and Category=@Category and Items=@OldItems";
                    break;
                case "Update":
                    query = @"if not exists(select * from PM_Items_Shanti  where Machine=@MachineID and Category=@Category and Items=@OldItems)
                                    begin
                                    insert into PM_Items_Shanti (Machine,Category,Items)
                                    values(@MachineID,@Category,@Items)
                                    end
                                    else
                                    begin
                                    update PM_Items_Shanti set Items=@Items  where Machine=@MachineID and Category=@Category and Items=@OldItems end";
                    break;
            }
            try
            {

                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@ID", IDD);
                cmd.Parameters.AddWithValue("@Items", items);
                cmd.Parameters.AddWithValue("@OldItems", OldItems);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        #endregion

        #region PMCheckListTransaction

        internal static bool UpdatePMEndDate(string MachineID, DateTime enddate, DateTime startdate, string Type)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool success = false;
            string query = string.Empty;
            switch (Type)
            {
                case "Update":
                    query = @"insert into PM_Dates_Shanti (Machine,PMStartDate,PMEndDate)
                            values(@MachineID,@PMStartDate,@PMEndDate)";
                    break;
                case "Postponed":
                    query = @"if exists (select * from PM_Dates_Shanti where Machine=@MachineID and PMStartDate=@PMStartDate)
                                    begin
                                    update PM_Dates_Shanti set PMEndDate=@PMEndDate where Machine=@MachineID and  PMStartDate=@PMStartDate
                                    end";
                    break;
            }

            try
            {
                startdate = Type.Equals("Postponed", StringComparison.OrdinalIgnoreCase) ? startdate : DateTime.Now.AddMinutes(1);
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@PMEndDate", enddate.ToString("yyyy-MM-dd HH:mm:00"));
                cmd.Parameters.AddWithValue("@PMStartDate", startdate.ToString("yyyy-MM-dd HH:mm:00"));
                cmd.ExecuteNonQuery();
                success = true;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return success;
        }


        internal static List<PMTransactionEntity> GetPMTransactionData(string MachineID, string PMDate, string EndDate)
        {
            List<PMTransactionEntity> EntityList = new List<PMTransactionEntity>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string ok = string.Empty;
            string query = string.Empty;
            query = @"[S_GetPMTransaction_Shanti]";
            try
            {

                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Machine", MachineID);
                cmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(PMDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", Convert.ToDateTime(EndDate).ToString("yyyy-MM-dd HH:mm:ss"));
                //cmd.Parameters.AddWithValue("@PMDate", Convert.ToDateTime(PMDate).ToString("yyyy-MM-dd HH:mm:ss"));
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    PMTransactionEntity Entity = new PMTransactionEntity();
                    Entity.Category = sdr["Category"].ToString();
                    Entity.Items = sdr["Items"].ToString();
                    Entity.MachineID = sdr["Machine"].ToString();
                    Entity.Remarks = sdr["Remarks"].ToString();
                    if (!sdr["Ok/NotOk"].Equals(DBNull.Value))
                    {
                        if (sdr["Ok/NotOk"].ToString().Equals("OK"))
                        {
                            Entity.NotOkData = false;
                            Entity.OkData = true;
                        }
                        else if (sdr["Ok/NotOk"].ToString().Equals("NotOK"))
                        {
                            Entity.NotOkData = true;
                            Entity.OkData = false;
                        }
                    }
                    Entity.IDD = sdr["ID"].ToString();
                    EntityList.Add(Entity);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return EntityList;
        }

        internal static string GetPMDates(string MachineID, out string enddate)
        {
            string LastDate = string.Empty; ;
            SqlDataReader sdr = null;
            enddate = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = string.Empty;
            query = @"select top 1 * from PM_Dates_Shanti where machine=@MachineID order by id desc";
            try
            {

                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    if (!sdr["PMStartDate"].Equals(DBNull.Value))
                        LastDate = sdr["PMStartDate"].ToString();
                    if (!sdr["PMEndDate"].Equals(DBNull.Value))
                        enddate = sdr["PMEndDate"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return LastDate;
        }

        internal static void UpdateTransaction(string machineID, string catagory, string items, string remarks, string okNotOK, string IDD)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = string.Empty;

            query = @"if not exists( select * from PM_Transaction_Shanti where ID=@IDD)
                                begin
                                insert into PM_Transaction_Shanti(Machine,Category,Items,[Ok/NotOk],Remarks,UpdatedTS)
                                values(@machine,@category,@Items,@Value,@Remarks,@UpdatedTS)
                                end
                                else
                                begin
                                update PM_Transaction_Shanti set [Ok/NotOk]=@Value ,Remarks=@Remarks where ID=@IDD
                                end";
            try
            {

                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@machine", machineID);
                cmd.Parameters.AddWithValue("@category", catagory);
                cmd.Parameters.AddWithValue("@Items", items);
                cmd.Parameters.AddWithValue("@Remarks", remarks);
                cmd.Parameters.AddWithValue("@Value", okNotOK);
                cmd.Parameters.AddWithValue("@IDD", IDD);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:00"));
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static string GetLastPMDAte(string MachineID)
        {
            string EntityList = string.Empty; ;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = string.Empty;
            query = @"select top 1 * from PM_Dates_Shanti where machine=@MachineID order by id desc";
            try
            {

                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    EntityList = sdr["PMEndDate"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return EntityList;
        }

        internal static bool checkPMCheckliststatus(DateTime PMDate, DateTime EndDate, string MachineID)
        {
            bool status = false;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string ok = string.Empty;
            string query = string.Empty;
            query = @"[S_GetPMTransaction_Shanti]";
            try
            {

                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Machine", MachineID);
                cmd.Parameters.AddWithValue("@StartDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                //cmd.Parameters.AddWithValue("@PMDate", Convert.ToDateTime(PMDate).ToString("yyyy-MM-dd HH:mm:ss"));
                sdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(sdr);
                foreach (DataRow dtrow in dt.Rows)
                {
                    status = string.IsNullOrEmpty(dtrow["Ok/NotOk"].ToString()) ? false : true;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return status;
        }


        internal static bool CheckCategoryPresent(string category,string MachineID)
        {
            bool present = false;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;

            try
            {

                cmd = new SqlCommand(@"select * from PM_Items_Shanti  where Machine=@MachineID and Category=@Category", conn);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                    present = true;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return present;
        }


        internal static void UpdateCategory(string MachineID, string oldCategory, string category)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"if exists (select * from PM_Items_Shanti where Machine=@MachineID and Category=@OldCategory)
                                begin
                                    update PM_Items_Shanti set Category=@Category where Machine=@MachineID and Category=@OldCategory
                                end";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@OldCategory", oldCategory);
                cmd.ExecuteNonQuery();
              

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static void UpdatePMDatainSchedule(string machineID, string ReportFileName)
        {
            
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"if exists(select * from ScheduledReports where ReportName = @ReportFileName)
                                  begin
                                      update ScheduledReports set Machine = @MachineID ,RunHistory = @RunHistory where ReportName = @ReportFileName
                                  end";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@ReportFileName", ReportFileName);
                cmd.Parameters.AddWithValue("@RunHistory", DBNull.Value);
                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        #endregion

        #region Tafe HeatTreatment ---
        internal static DataTable GetRatioVersionInformation()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select * from Ratio_VersionAssociation_Tafe";
                cmd = new SqlCommand(sqlQuery, sqlConn);
                SqlDataReader sdr = cmd.ExecuteReader();

                dt.Load(sdr);
                dt.GetChanges();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error log -\n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }

        internal static void insertUpdateRatioVersionInfo(string ratio, string version, out string sucessfailure)
        {
            sucessfailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"if not exists(select * from Ratio_VersionAssociation_Tafe where Ratio=@Ratio and Version=@Version)
                begin
                Insert into Ratio_VersionAssociation_Tafe(Ratio,Version)values(@Ratio,@Version)
                End", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Ratio", ratio);
                cmd.Parameters.AddWithValue("@Version", version);
                cmd.ExecuteNonQuery();
                sucessfailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessfailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }

        internal static void DeleteRatioVersionAssociation(string ratio, string version, out string sucessfailure)
        {
            sucessfailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("delete from Ratio_VersionAssociation_Tafe where Ratio=@Ratio and Version=@Version", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Ratio", ratio);
                cmd.Parameters.AddWithValue("@Version", version);
                cmd.ExecuteNonQuery();
                sucessfailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessfailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }

        internal static void insertRatioVersionPartAssociation(string Ratio, string version, string partno, string type, out string sucessfailure)
        {
            sucessfailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"if not exists(select * from Ratio_Version_PartAssociation_Tafe where Ratio=@Ratio and Version=@Version and PartNumber=@PartNumber and PartType=@PartType)
                begin
                Insert into Ratio_Version_PartAssociation_Tafe(Ratio,Version,PartNumber,PartType)values(@Ratio,@Version,@PartNumber,@PartType)
                End", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Ratio", Ratio);
                cmd.Parameters.AddWithValue("@Version", version);
                cmd.Parameters.AddWithValue("@PartNumber", partno);
                cmd.Parameters.AddWithValue("@PartType", type);
                cmd.ExecuteNonQuery();
                sucessfailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessfailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }


        internal static DataTable GetAllRatioVersion_PartDetails()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select * from Ratio_Version_PartAssociation_Tafe";
                cmd = new SqlCommand(sqlQuery, sqlConn);
                SqlDataReader sdr = cmd.ExecuteReader();

                dt.Load(sdr);
                dt.GetChanges();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error log -\n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }


        internal static void DeleteRatioVersionPartAssociation(string ratio, string version, string partNo, string PartType, out string sucessfailure)
        {
            sucessfailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("delete from Ratio_Version_PartAssociation_Tafe where Ratio=@Ratio and Version=@Version and PartNumber=@Partno and PartType=@PartType", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Ratio", ratio);
                cmd.Parameters.AddWithValue("@Version", version);
                cmd.Parameters.AddWithValue("@Partno", partNo);
                cmd.Parameters.AddWithValue("@PartType", PartType);
                cmd.ExecuteNonQuery();
                sucessfailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessfailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }

        internal static List<string> GetPartTypeBasedOnPartNo(string PartNo)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> PartType = new List<string>() ;
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = @"select distinct m1.description from componentoperationpricing c1 inner join machineinformation m1 on m1.machineid=c1.machineid
                 where componentid = @Comp and m1.description in ('Pinion', 'Crown')";
                cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Comp", PartNo);
                SqlDataReader sdr = cmd.ExecuteReader();
                while(sdr.Read())
                {
                    PartType.Add(sdr["description"].ToString());
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error log -\n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return PartType;
        }

        #endregion

    }
}