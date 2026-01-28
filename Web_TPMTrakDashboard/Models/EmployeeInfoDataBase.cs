using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public class EmployeeInfoDataBase
    {
        //Employee Information
        internal static List<EmployeeInfoModel> GetAllEmpDetails(string employeeid, string param)
        {
            List<EmployeeInfoModel> list = new List<EmployeeInfoModel>();
            SqlConnection conn = ConnectionManager.GetConnection();

            try
            {
                SqlDataReader sdr = null;
                SqlCommand cmd = new SqlCommand(@"s_ViewEmployeeMaster", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeID", employeeid);
                cmd.Parameters.AddWithValue("@Param", param);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        EmployeeInfoModel EmpVals = new EmployeeInfoModel();
                        if (!Convert.IsDBNull(sdr["Employeeid"]))
                        {
                            EmpVals.employeeid = Convert.ToString(sdr["Employeeid"]);
                        }

                        if (!Convert.IsDBNull(sdr["employeeno"]))
                        {
                            EmpVals.employeeno = Convert.ToInt32(sdr["employeeno"]);
                        }
                        if (!Convert.IsDBNull(sdr["Name"]))
                        {
                            EmpVals.Name = Convert.ToString(sdr["Name"]);
                        }
                        if (!Convert.IsDBNull(sdr["designation"]))
                        {
                            EmpVals.designation = Convert.ToString(sdr["designation"]);
                        }
                        if (!Convert.IsDBNull(sdr["qualification"]))
                        {
                            EmpVals.qualification = Convert.ToString(sdr["qualification"]);
                        }
                        if (!Convert.IsDBNull(sdr["address1"]))
                        {
                            EmpVals.address1 = Convert.ToString(sdr["address1"]);
                        }

                        if (!Convert.IsDBNull(sdr["phone"]))
                        {
                            EmpVals.phone = Convert.ToString(sdr["phone"]);
                        }


                        if (!Convert.IsDBNull(sdr["interfaceid"]))
                        {
                            EmpVals.interfaceid = sdr["interfaceid"].ToString();
                        }
                        if (!Convert.IsDBNull(sdr["company_default"]))
                        {
                            EmpVals.company_default = Convert.ToBoolean(sdr["company_default"]);
                        }
                        if (!Convert.IsDBNull(sdr["Email"]))
                        {
                            EmpVals.Email = Convert.ToString(sdr["Email"]);
                        }

                        if (!Convert.IsDBNull(sdr["Plants"]))
                        {
                            EmpVals.PlantID = sdr["Plants"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();
                            EmpVals.SMSTextTemplate = String.Join(",", EmpVals.PlantID.ToList());//sdr["Plants"].ToString();
                        }
                        if (!Convert.IsDBNull(sdr["EmployeeRole"]))
                        {
                            EmpVals.EmployeeRole = Convert.ToString(sdr["EmployeeRole"]);
                        }
                        //if (!Convert.IsDBNull(sdr["Groups"]))
                        //{
                        //	EmpVals.CellID = sdr["Groups"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();
                        //	EmpVals.CellIDTextTemplate = String.Join(",", EmpVals.CellID.ToList());
                        //}
                        if(!Convert.IsDBNull(sdr["status"]))
                        {
                            EmpVals.IsActiveStatus = sdr["status"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase) ? true : false;
                        }
                        if (!Convert.IsDBNull(sdr["upassword"]))
                        {
                            EmpVals.Password = Convert.ToString(sdr["upassword"]);
                        }
                        list.Add(EmpVals);
                    }
                }
                else
                {
                    list = null;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static void DeleteEmpPlant(string EmployeeID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                var query = "delete from [dbo].[PlantEmployee] where EmployeeID=@EmployeeID";
                cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SaveSelectionsForProd: " + ex.Message.ToString());
            }
            finally
            {
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
            }
        }

        internal static void SaveSelectionsForPlant(List<string> Plant, string EmployeeID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                foreach (var s in Plant)
                {
                    string query = @"if not exists(select * from PlantEmployee where PlantID=@PlantID and EmployeeID=@EmployeeID)
                    BEGIN
                    insert into [dbo].[PlantEmployee](PlantID,EmployeeID)
                    Values(@PlantID,@EmployeeID)
                    END";
                    cmd = new SqlCommand(query, sqlConn);

                    cmd.Parameters.AddWithValue("@PlantID", s.Trim());
                    cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SaveSelectionsForProd: " + ex.Message.ToString());
            }
            finally
            {
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
            }
        }

        internal static int SaveEmployeeInformation(string Employeeid, int employeeno, string name, string designation, string qualification,
              string address1, string address2, string phone, int operate, int setting, string maintain, bool status, bool isadmin,
              string upassword, string interfaceid, int Company_default, string Email, string EmployeeRole,string param, out bool isSuccessOrFailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int recordsAffected = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("s_InsertEmployeeMaster", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", param);
                cmd.Parameters.AddWithValue("@EmployeeID", Employeeid);
                cmd.Parameters.AddWithValue("@employeeno", employeeno);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@designation", designation);
                cmd.Parameters.AddWithValue("@qualification", qualification);
                cmd.Parameters.AddWithValue("@address1", address1);
                cmd.Parameters.AddWithValue("@address2", address2);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@operate", operate);
                cmd.Parameters.AddWithValue("@setting", setting);
                cmd.Parameters.AddWithValue("@maintain", maintain);
                //cmd.Parameters.AddWithValue("@status", status);
                //cmd.Parameters.AddWithValue("@isadmin", isadmin);
                cmd.Parameters.AddWithValue("@upassword", upassword);
                cmd.Parameters.AddWithValue("@interfaceid", interfaceid);
                cmd.Parameters.AddWithValue("@Company_default", Company_default);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Role", EmployeeRole);
                cmd.Parameters.AddWithValue("@status", status.Equals(true)? 1:2);
                recordsAffected = cmd.ExecuteNonQuery();
                isSuccessOrFailure = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return recordsAffected;
        }

        internal static bool CheckExistingEmployeeInAutodata(string InterfaceID, out string StartTime, out string EndTime)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            Boolean isPresent = false;
            string adminVal = string.Empty;

            StartTime = string.Empty;
            EndTime = string.Empty;
            try
            {
                string query = @"select E.employeeid as name,A.opr as interid,min(sttime) as strt,max(ndtime) as ndt from autodata A inner join 
                employeeinformation E on A.opr=E.interfaceid where A.opr=@InterfaceID group by A.opr,E.employeeid";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@InterfaceID", InterfaceID);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    StartTime = rdr["strt"].ToString();
                    EndTime = rdr["ndt"].ToString();
                }
                if (rdr.HasRows)
                {
                    isPresent = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return isPresent;

        }

        internal static int deleteEmployeeInformation(string EmployeeID, string InterfaceID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int recordDeleted = 0;
            try
            {
                string sqlQuery = "delete from EmployeeInformation where InterfaceID = @InterfaceID";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@InterfaceID", InterfaceID);
                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                recordDeleted = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return recordDeleted;
        }
    }
}