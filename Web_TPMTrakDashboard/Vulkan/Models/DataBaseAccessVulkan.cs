using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Vulkan.Models
{
    public class DataBaseAccessVulkan
    {
        internal static List<MachineAndonData> getMachineAndonData()
        {
            List<MachineAndonData> list = new List<MachineAndonData>();
            MachineAndonData data = null;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            try
            {
                cmd = new SqlCommand("[dbo].[S_GetAndonDetails_Vulkan]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                // cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue(@"Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                // cmd.Parameters.AddWithValue(@"Date", "2020-10-10 14:14:45");
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data = new MachineAndonData();
                        data.Machineid = reader["MachineID"].ToString();
                        data.Part = reader["Part"].ToString();
                        data.BatchNo = reader["BatchNo"].ToString();
                        data.ExpectedProd = reader["ExpectedProduction"].ToString();
                        data.ActualProd = reader["ProdCount"].ToString();
                        data.OEE = reader["OEE"].ToString();
                        if (Convert.ToDouble(data.OEE) >= Convert.ToDouble(reader["OEGreen"].ToString()))
                        {
                            data.OEEFontColor = "Green";
                            data.OEEBackColor = "backWhite";
                        }
                        else if (Convert.ToDouble(data.OEE) <= Convert.ToDouble(reader["OEred"].ToString()))
                        {
                            data.OEEFontColor = "Red";
                            data.OEEBackColor = "backWhite";
                        }
                        else
                        {
                            data.OEEFontColor = "Black";
                            data.OEEBackColor = "backYellow";
                        }

                        data.DownTime = reader["DownTime"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (reader != null) reader.Close();
            }
            return list;
        }

        internal static List<string> GetPlantID()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct PlantID from PlantInformation", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read()) { list.Add(sdr["PlantID"].ToString()); }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetPlantID: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static List<string> GetMachineIDs(string PlantID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct MachineID from PlantMachine where PlantID=@PlantID", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["MachineID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetMachineIDs: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static List<MetInsEntity> GetCLImagesList(string Param)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<MetInsEntity> list = new List<MetInsEntity>();
            try
            {
                cmd = new SqlCommand(@"select * from ImageReferenceDetails_Vulkan where ImageName like CONCAT(@Param,'%')", conn);
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        MetInsEntity entity = new MetInsEntity();
                        entity.imgName = sdr["ImageName"].ToString().Split(' ').ToList()[1];
                        entity.RefID = sdr["RefID"].ToString();
                        list.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetCLImagesList: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        #region ---- AM Master ---
        internal static List<string> GetRevisionNo(string MachineID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct RevID from AM_Master_Vulkan where MachineID=@MachineID", conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read()) { list.Add(sdr["RevID"].ToString()); }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetRevisionNo: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static void SaveRevisionNo(string PlantID, string MachineID, string Frequency, string RevID) //, string RevNo, string RevisionNoNew, string DocNo, string IssueDate, string RevDate
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_GetAMCheckSheetDetails_Vulkan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@Department", PlantID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@RevID", RevID);
                if (!string.IsNullOrEmpty(Frequency))
                    cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.Parameters.AddWithValue("@Param", "Save_NewRevDetails");
                //cmd.Parameters.AddWithValue("@RevNoNew", RevisionNoNew);
                //cmd.Parameters.AddWithValue("@RevDate", RevDate);
                //cmd.Parameters.AddWithValue("@DocNo", DocNo);
                //cmd.Parameters.AddWithValue("@IssueDate", IssueDate);
                //cmd.Parameters.AddWithValue("@RevDate", RevDate);
                //cmd.Parameters.AddWithValue("@RevNo", RevNo);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SaveRevisionNo: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static List<CLVulkanEntity> GetCLGridData(string PlantID, string MachineID, string RevisionNo, out string DocNo, out string IssueDate, out string RevDate)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<CLVulkanEntity> list = new List<CLVulkanEntity>();
            DataTable dt = new DataTable();
            DocNo = "";
            IssueDate = "";
            RevDate = "";
            try
            {
                cmd = new SqlCommand(@"S_GetAMCheckSheetDetails_Vulkan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@Department", PlantID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@RevID", RevisionNo);
                cmd.Parameters.AddWithValue("@Param", "MasterView");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    DocNo = dt.Rows[0]["DocNo"].ToString();
                    IssueDate = Util.GetDateTime(dt.Rows[0]["IssueDate"].ToString()).ToString("dd-MM-yyyy");
                    RevDate = Util.GetDateTime(dt.Rows[0]["RevDate"].ToString()).ToString("dd-MM-yyyy");
                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        CLVulkanEntity entity = new CLVulkanEntity();

                        entity.SlNo = row["CheckpointID"].ToString();
                        entity.CheckPoint = row["CheckpointItem"].ToString();
                        entity.Requirement = row["Requirement"].ToString();
                        entity.Method = row["Method"].ToString();
                        entity.Instrument = row["Instrument"].ToString();
                        entity.ActionPlan = row["ActionPlan"].ToString();
                        entity.ChecklistType = row["AMType"].ToString();
                        list.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetCLGridData: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static int SaveCLData(string PlantID, string MachineID, string RevNo, string CheckpointID, string CheckPointDesc, string Requirement, string Method, string Instrument, string ActionPlan, string AMType, string IssueDate, string RevDate, string DocNo, string Param1, out string DuplicateID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string SaveResult = "";
            int isSaved = 0;
            DuplicateID = "";
            try
            {
                cmd = new SqlCommand(@"S_GetAMCheckSheetDetails_Vulkan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@RevNo", RevNo);
                cmd.Parameters.AddWithValue("@RevID", RevNo);
                cmd.Parameters.AddWithValue("@CheckpointID", CheckpointID);
                cmd.Parameters.AddWithValue("@CheckpointItem", CheckPointDesc);
                cmd.Parameters.AddWithValue("@Requirement", Requirement);
                cmd.Parameters.AddWithValue("@Method", Method);
                cmd.Parameters.AddWithValue("@Instrument", Instrument);
                cmd.Parameters.AddWithValue("@Frequency", "Daily");
                cmd.Parameters.AddWithValue("@ActionPlan", ActionPlan);
                cmd.Parameters.AddWithValue("@Department", PlantID);
                cmd.Parameters.AddWithValue("@IssueDate", Util.GetDateTime(IssueDate));
                cmd.Parameters.AddWithValue("@RevDate", Util.GetDateTime(RevDate));
                cmd.Parameters.AddWithValue("@DocNo", DocNo);
                cmd.Parameters.AddWithValue("@AMType", AMType);
                cmd.Parameters.AddWithValue("@Param", "MasterSave");
                cmd.Parameters.AddWithValue("@Param1", Param1);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                        SaveResult = sdr["SaveFlag"].ToString();

                    if (SaveResult.Equals("Inserted", StringComparison.OrdinalIgnoreCase) || SaveResult.Equals("Updated", StringComparison.OrdinalIgnoreCase))
                        isSaved = 1;
                    else if (SaveResult.Equals("Checkpoint already exists!", StringComparison.OrdinalIgnoreCase))
                        DuplicateID = SaveResult;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SaveCLData: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return isSaved;
        }

        internal static int DeleteCLValue(string CheckPointID, string MachineID, string RevNo, string PlantID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int isDeleted = 0;
            try
            {
                cmd = new SqlCommand(@"S_GetAMCheckSheetDetails_Vulkan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@CheckpointID", CheckPointID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@RevID", RevNo);
                cmd.Parameters.AddWithValue("@Param", "MasterDelete");
                isDeleted = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("DeleteCLValue: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return isDeleted;
        }

        internal static string CopyCLData(string SrcMachineID, string DestMachineIDs)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string result = string.Empty;
            try
            {
                cmd = new SqlCommand(@"S_GetAMCheckSheetDetails_Vulkan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OldMachineID", SrcMachineID);
                //cmd.Parameters.AddWithValue("@RevID", SrcRevNo);
                cmd.Parameters.AddWithValue("@MachineID", DestMachineIDs);
                cmd.Parameters.AddWithValue("@Param", "CopyFromOldToNewMachine");
                cmd.CommandTimeout = 120;

                result = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("CopyCLData: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return result;
        }
        #endregion

        #region --- PM Master ---
        internal static List<string> GetRevisionNo_PM(string PlantID, string MachineID, string Frequency)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct RevID from pm_master_vulkan where MachineID=@MachineID and Department=@PlantID and Frequency=@Frequency", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["RevID"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static List<CLVulkanEntity> GetCLGridData_PM(string PlantID, string MachineID, string RevNo, string Frequency, out string DocNo, out string IssueDate, out string RevDate)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<CLVulkanEntity> list = new List<CLVulkanEntity>();
            DocNo = "";
            IssueDate = "";
            RevDate = "";
            try
            {
                cmd = new SqlCommand(@"S_GetPMCheckSheetDetails_Vulkan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Department", PlantID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@RevID", RevNo);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.Parameters.AddWithValue("@Param", "MasterView");
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);

                    DocNo = dt.Rows[0]["DocNo"].ToString();
                    IssueDate = Util.GetDateTime(dt.Rows[0]["IssueDate"].ToString()).ToString("dd-MM-yyyy");
                    RevDate = Util.GetDateTime(dt.Rows[0]["RevDate"].ToString()).ToString("dd-MM-yyyy");

                    foreach (DataRow row in dt.Rows)
                    {
                        CLVulkanEntity entity = new CLVulkanEntity();

                        entity.SlNo = row["CheckpointID"].ToString();
                        entity.CheckPoint = row["CheckpointItem"].ToString();
                        entity.Requirement = row["Requirement"].ToString();
                        entity.Method = row["Method"].ToString();
                        entity.Instrument = row["Instrument"].ToString();
                        entity.Observation = row["Observation"].ToString();
                        var msg = row["IsImageRequired"].ToString();
                        entity.IsImageRequired = row["IsImageRequired"].ToString() == "True" ? true : false;
                        entity.ReferenceImageName = row["RefImageName"].ToString();

                        list.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetCLGridData_PM: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static int SaveCLData_PM(string PlantID, string MachineID, string RevID, string Frequency, string DocNo, string IssueDate, string RevDate, CLVulkanEntity data, string Param1, out string DuplicateMsg)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int isSaved = 0;
            DuplicateMsg = "";
            string SaveResult = "";
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"S_GetPMCheckSheetDetails_Vulkan", conn);
                cmd.Parameters.AddWithValue("@Department", PlantID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@RevID", RevID);
                cmd.Parameters.AddWithValue("@RevNo", RevID);
                cmd.Parameters.AddWithValue("@DocNo", DocNo);
                cmd.Parameters.AddWithValue("@IssueDate", Util.GetDateTime(IssueDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@RevDate", Util.GetDateTime(RevDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.Parameters.AddWithValue("@CheckpointID", data.SlNo);
                cmd.Parameters.AddWithValue("@CheckpointItem", data.CheckPoint);
                cmd.Parameters.AddWithValue("@Requirement", data.Requirement);
                cmd.Parameters.AddWithValue("@RefImageData", data.ReferenceImageData);
                cmd.Parameters.AddWithValue("@RefImageName", data.ReferenceImageName);
                cmd.Parameters.AddWithValue("@Method", data.Method);
                cmd.Parameters.AddWithValue("@Instrument", data.Instrument);
                cmd.Parameters.AddWithValue("@Observation", data.Observation);
                cmd.Parameters.AddWithValue("@IsImageRequired", data.IsImageRequired == true ? 1 : 0);
                cmd.Parameters.AddWithValue("@Param", "MasterSave");
                cmd.Parameters.AddWithValue("@Param1", Param1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                        SaveResult = sdr["SaveFlag"].ToString();
                    if (SaveResult.Equals("Inserted", StringComparison.OrdinalIgnoreCase) || SaveResult.Equals("Updated", StringComparison.OrdinalIgnoreCase))
                        isSaved = 1;
                    else if (SaveResult.Equals("Checkpoint already exists!", StringComparison.OrdinalIgnoreCase))
                        DuplicateMsg = SaveResult;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SaveCLData_PM: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return isSaved;
        }

        internal static int DeleteCLValue_PM(string CheckPointID, string MachineID, string RevNo, string PlantID, string Frequency)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int isDeleted = 0;
            try
            {
                cmd = new SqlCommand(@"S_GetPMCheckSheetDetails_Vulkan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@CheckpointID", CheckPointID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@RevID", RevNo);
                cmd.Parameters.AddWithValue("@Department", PlantID);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.Parameters.AddWithValue("@Param", "MasterDelete");
                isDeleted = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("DeleteCLValue_PM: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return isDeleted;
        }

        internal static string CopyCLData_PM(string SrcMachineID, string DestMachineIDs)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string result = string.Empty;
            try
            {
                cmd = new SqlCommand(@"S_GetPMCheckSheetDetails_Vulkan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OldMachineID", SrcMachineID);
                //cmd.Parameters.AddWithValue("@RevID", SrcRevNo);
                cmd.Parameters.AddWithValue("@MachineID", DestMachineIDs);
                cmd.Parameters.AddWithValue("@Param", "CopyFromOldToNewMachine");
                cmd.CommandTimeout = 120;

                result = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("CopyCLData: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return result;
        }

        internal static void SaveRevisionNo_PM(string PlantID, string MachineID, string Frequency, string RevID) //, string RevNo, string RevisionNoNew, string DocNo, string IssueDate, string RevDate
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_GetPMCheckSheetDetails_Vulkan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@Department", PlantID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@RevID", RevID);
                if (!string.IsNullOrEmpty(Frequency))
                    cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.Parameters.AddWithValue("@Param", "Save_NewRevDetails");
                //cmd.Parameters.AddWithValue("@RevNoNew", RevisionNoNew);
                cmd.Parameters.AddWithValue("@RevDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //cmd.Parameters.AddWithValue("@DocNo", DocNo);
                //cmd.Parameters.AddWithValue("@IssueDate", IssueDate);
                //cmd.Parameters.AddWithValue("@RevDate", RevDate);
                cmd.Parameters.AddWithValue("@RevNo", RevID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SaveRevisionNo: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }


        #endregion

        #region ----- Method & Instrument Master -----
        internal static bool SaveMethodAndInstrumentMasterData(string ImageName, byte[] ImageData, string RefID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool isInserted = false;
            try
            {
                cmd = new SqlCommand(@"if not exists (select * from ImageReferenceDetails_Vulkan where RefID=@RefID and ImageName=@ImageName)
	begin
		insert into ImageReferenceDetails_Vulkan(ImageData, ImageName, RefID) values(@ImageData, @ImageName, @RefID)
		select 'inserted' as SaveFlag
	end
else
	begin
		select 'Exists' as SaveFlag
	end	
", conn);
                cmd.Parameters.AddWithValue("@ImageData", ImageData);
                cmd.Parameters.AddWithValue("@ImageName", ImageName);
                cmd.Parameters.AddWithValue("@RefID", RefID);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                string res = cmd.ExecuteScalar().ToString();
                isInserted = res.Equals("inserted", StringComparison.OrdinalIgnoreCase) || res.Equals("exists", StringComparison.OrdinalIgnoreCase) ? true : false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SaveMethodAndInstrumentMasterData: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return isInserted;
        }

        internal static List<MetInsEntity> GetMetInsData(string Param)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<MetInsEntity> list = new List<MetInsEntity>();
            string appPath = (Param.Equals("Method", StringComparison.OrdinalIgnoreCase)) ? HttpContext.Current.Server.MapPath("~/Vulkan/Images/Method/") : HttpContext.Current.Server.MapPath("~/Vulkan/Images/Instrument/");
            try
            {
                cmd = new SqlCommand(@"select * from ImageReferenceDetails_Vulkan where ImageName like CONCAT(@Param,'%')", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@Param", Param);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    int i = 0;
                    while (sdr.Read())
                    {
                        MetInsEntity entity = new MetInsEntity();

                        entity.SlNo = (++i).ToString();
                        entity.imgName = sdr["ImageName"].ToString().Split(' ').ToList()[1];
                        entity.imgUrl = "data:image/jpeg;base64," + Convert.ToBase64String(sdr["ImageData"] as byte[]);
                        entity.RefID = sdr["RefID"].ToString();
                        list.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetMetInsData: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }
        #endregion

        #region ---- AM Approval Dashboard ----
        internal static List<DailyCLTransactionEntity> GetAMTransctionData(string PlantID, string MachineID, int Year, int Month)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            List<DailyCLTransactionEntity> list = new List<DailyCLTransactionEntity>();
            int clName = 0;
            try
            {
                cmd = new SqlCommand(@"S_GetAMCheckSheetDetails_Vulkan", conn);
                cmd.Parameters.AddWithValue("@Department", PlantID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Year", Year);
                cmd.Parameters.AddWithValue("@Month", Month);
                cmd.Parameters.AddWithValue("@Param", "ReportView");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt1.Load(sdr);
                    dt2.Load(sdr);
                    if (dt.Columns.IndexOf("SaveFlag") == -1)
                    {
                        List<string> list_Holiday = new List<string>();
                        if (dt2.Rows.Count > 0)
                            list_Holiday = dt2.AsEnumerable().Where(x => Util.GetDateTime(x["Holiday"].ToString()).Month == Month).Select(x => Convert.ToDateTime(x["Holiday"].ToString()).ToString("dd")).ToList();

                        if (dt.Rows.Count > 0)
                        {
                            DailyCLTransactionEntity li = new DailyCLTransactionEntity();
                            li.HeaderVisibility = true;
                            List<DateColumnsEntity> entity = new List<DateColumnsEntity>();
                            foreach (DataColumn column in dt.Columns)
                            {
                                clName = 0;
                                int.TryParse(column.ColumnName, out clName);
                                if (clName > 0)
                                {
                                    DateColumnsEntity en = new DateColumnsEntity();
                                    en.DayValue = clName.ToString();
                                    en.HeaderVisibility = true;
                                    entity.Add(en);
                                }
                            }
                            li.DateColumns = entity;
                            li.DateColSpan = (entity.Count);

                            list.Add(li);
                            int i = 0, centerRow = dt.Rows.Count / 2 + 1;
                            foreach (DataRow row in dt.Rows)
                            {
                                DailyCLTransactionEntity li_CL = new DailyCLTransactionEntity();
                                li_CL.ContentVisibility = true;
                                li_CL.ColSpanContent = 1;

                                li_CL.SlNo = row["CheckpointID"].ToString();
                                li_CL.CheckpointDesc = row["CheckpointItem"].ToString();
                                li_CL.Requirement = row["Requirement"].ToString();
                                li_CL.ActionPlan = row["ActionPlan"].ToString();
                                li_CL.SignRowVisibility = true;
                                string method = row["Method"].ToString();
                                if (!string.IsNullOrEmpty(method))
                                {
                                    var arr = method.Split(',').ToList();

                                    if (arr.Count > 0)
                                    {
                                        foreach (var a in arr)
                                            li_CL.MethodList.Add(DataBaseAccessVulkan.GetMethodImageData(a.Trim()));
                                    }
                                }
                                string Instrument = row["Instrument"].ToString().Trim();
                                if (!string.IsNullOrEmpty(Instrument))
                                {
                                    var arr = Instrument.Split(',').ToList();

                                    if (arr.Count > 0)
                                    {
                                        foreach (var a in arr)
                                            li_CL.InstrumentList.Add(DataBaseAccessVulkan.GetMethodImageData(a.Trim()));
                                    }
                                }
                                foreach (DataColumn column in dt.Columns)
                                {
                                    clName = 0;
                                    int.TryParse(column.ColumnName.Trim(), out clName);
                                    if (clName > 0)
                                    {
                                        DateColumnsEntity enn = new DateColumnsEntity();
                                        if (list_Holiday.Contains(clName.ToString()))
                                        {
                                            if (i == centerRow - 1)
                                                enn.DailyCLValue = dt2.AsEnumerable().Where(x => Util.GetDateTime(x["Holiday"].ToString()).ToString("dd").Contains(clName.ToString())).Select(x => x["Reason"].ToString()).FirstOrDefault();
                                            enn.DayValueVisibility = true;
                                            enn.DayValue = clName.ToString();
                                            enn.BackgroundClass = "HolidayClass";
                                        }
                                        else
                                        {
                                            enn.DailyCLValue = string.IsNullOrEmpty(row[column].ToString()) ? "" : (row[column].ToString().Equals("Done", StringComparison.OrdinalIgnoreCase) ? "&#10004;" : "&#10008;"); // row[column].ToString();
                                            enn.DailyCLValueClass = string.IsNullOrEmpty(row[column].ToString()) ? "" : (row[column].ToString().Equals("Done", StringComparison.OrdinalIgnoreCase) ? "green" : "red");
                                            enn.DayValueVisibility = true;
                                            enn.DayValue = clName.ToString();
                                        }
                                        li_CL.DateColumns.Add(enn);
                                    }
                                }
                                list.Add(li_CL);
                                i++;
                            }

                            DailyCLTransactionEntity FooterEntity1 = new DailyCLTransactionEntity();
                            FooterEntity1.ContentVisibility = true;
                            FooterEntity1.ActionPlan = "CHECKED BY SIGN: ";


                            DailyCLTransactionEntity FooterEntity2 = new DailyCLTransactionEntity();
                            FooterEntity2.ContentVisibility = true;
                            FooterEntity2.ActionPlan = "APPROVE: ";
                            int Currdate = DateTime.Now.Day;

                            foreach (DataColumn column1 in dt.Columns)
                            {
                                clName = 0;
                                int.TryParse(column1.ColumnName, out clName);
                                if (clName > 0)
                                {
                                    DateColumnsEntity en1 = new DateColumnsEntity();
                                    if (list_Holiday.Contains(clName.ToString()))
                                        en1.BackgroundClass = "HolidayClass";
                                    else
                                    {
                                        var OperatorSign = dt1.AsEnumerable().Where(x => Convert.ToDateTime(x["Date"].ToString()).Day == clName).Select(x => $"{x["OperatorID"].ToString()}[{(string.IsNullOrEmpty(x["OperatorTS"].ToString()) ? "" : Util.GetDateTime(x["OperatorTS"].ToString()).ToString("dd-MM-yyyy HH:mm:ss"))}]").FirstOrDefault();
                                        en1.DailyCLValue = OperatorSign != null ? OperatorSign : "";
                                    }

                                    en1.DayValueVisibility = true;
                                    FooterEntity1.DateColumns.Add(en1);


                                    en1 = new DateColumnsEntity();
                                    if (list_Holiday.Contains(clName.ToString()))
                                    {
                                        en1.BackgroundClass = "HolidayClass";
                                        en1.DayValueVisibility = true;
                                        en1.DailyCLValue = dt2.AsEnumerable().Where(x => Util.GetDateTime(x["Holiday"].ToString()).ToString("dd").Contains(clName.ToString())).Select(x => x["Reason"].ToString()).FirstOrDefault();
                                    }
                                    else
                                    {
                                        var ApprovedRow = dt1.AsEnumerable().Where(x => Util.GetDateTime(x["Date"].ToString()).Day == clName && !string.IsNullOrEmpty(x["SupervisorID"].ToString().Trim())).Select(x => $"{x["SupervisorID"].ToString()}[{(string.IsNullOrEmpty(x["SupervisorTS"].ToString()) ? "" : Util.GetDateTime(x["SupervisorTS"].ToString()).ToString("dd-MM-yyyy HH:mm:ss"))}]").FirstOrDefault();
                                        if (ApprovedRow == null)
                                        {
                                            if (HttpContext.Current.Session["UserRole"].ToString().Equals("Supervisor", StringComparison.OrdinalIgnoreCase))
                                            {
                                                if (Month < DateTime.Now.Month)
                                                {
                                                    en1.ApproveVisibility = true;
                                                    en1.DayValue = clName.ToString();
                                                    en1.ApprovedEnabled = true;
                                                }
                                                else if (Month == DateTime.Now.Month && clName <= Currdate)
                                                {
                                                    en1.ApproveVisibility = true;
                                                    en1.DayValue = clName.ToString();
                                                    en1.ApprovedEnabled = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            en1.DailyCLValue = ApprovedRow.ToString();
                                            en1.DayValueVisibility = true;
                                        }
                                    }
                                    FooterEntity2.DateColumns.Add(en1);
                                }
                            }

                            list.Add(FooterEntity1);
                            list.Add(FooterEntity2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetAMTransctionData: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static MetInsEntity GetMethodImageData(string RefID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            MetInsEntity en = new MetInsEntity();
            try
            {
                cmd = new SqlCommand(@"select * from ImageReferenceDetails_Vulkan where RefID=@RefID", conn);
                cmd.Parameters.AddWithValue("@RefID", RefID);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        en.imgUrl = "data:image/jpeg;base64," + Convert.ToBase64String(sdr["ImageData"] as byte[]);
                        en.imgInByte = sdr["ImageData"] as byte[];
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetMethodImageData: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return en;
        }

        internal static string SubmitApprovedDetail(string MachineID, string SupervisorID, string ApprovedDate)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string Result = string.Empty;
            try
            {
                cmd = new SqlCommand(@"if exists (select * from AM_HeaderDetails_Vulkan where MachineID=@MachineID and [Date]=@ApprovedDate)
begin
update AM_HeaderDetails_Vulkan set SupervisorID=@SupervisorID, SupervisorTS=@SupervisorTS where MachineID=@MachineID and [Date]=@ApprovedDate
select 'Approved' as SaveFlag
end
else
begin
select 'NoTransactionFound' as SaveFlag
end", conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@ApprovedDate", Util.GetDateTime(ApprovedDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@SupervisorID", SupervisorID);
                cmd.Parameters.AddWithValue("@SupervisorTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                Result = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SubmitApprovedDetail: " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return Result;
        }
        #endregion

        #region -- AM Report --
        internal static DataTable GetReportData(string MachineID, string Year, string Month, out DataTable dtSign, out DataTable dtHoliday)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dtCLData = new DataTable();
            dtSign = new DataTable();
            dtHoliday = new DataTable();
            try
            {
                cmd = new SqlCommand("S_GetAMCheckSheetDetails_Vulkan", conn);
                cmd.Parameters.AddWithValue("@Param", "ReportView");
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Year", Year);
                cmd.Parameters.AddWithValue("@Month", Month);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dtCLData.Load(sdr);
                    dtSign.Load(sdr);
                    dtHoliday.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetReportData: {ex}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dtCLData;
        }
        #endregion


        #region--------------Planning Sheet _Vulkan---------------------------
        internal static DataTable GetPlanningSheetData(string frequency, int year, int Month)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"S_GetPlanDetails_Vulkan", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Frequency", frequency);
                if (frequency == "Daily" || frequency == "15Days")
                {
                    cmd.Parameters.AddWithValue("@Month", Month);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Month", "");
                }
                //cmd.Parameters.AddWithValue("@Month", Month);
                //cmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(Date).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Param", "View");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return dt;
        }
        internal static string SaveStartDate(string frequency, string Date)
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_GetPlanDetails_Vulkan", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Frequency", frequency);
                cmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(Date).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Year", Convert.ToDateTime(Date).ToString("yyyy"));
                cmd.Parameters.AddWithValue("@Param", "Save");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = sdr["SaveFlag"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return success;
        }
        internal static List<string> GetMachinesForPlanningSheet()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct machineid from machineinformation", con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["machineid"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return list;
        }
        internal static List<PostPoneGridData> GetPostPoneInfo(string Frequency)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<PostPoneGridData> list = new List<PostPoneGridData>();
            try
            {
                cmd = new SqlCommand(@"select * from PlanDetails_Vulkan where Frequency=@Frequency and ActivityDate>=GETDATE()", con);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        PostPoneGridData data = new PostPoneGridData();
                        data.MachineID = sdr["MachineID"].ToString();
                        data.ActivityDate = sdr["ActivityDate"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        internal static string SavePostPoneDate(string machineID, DateTime CurrentDate, DateTime PostPoneDate, string Frequency)
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_GetPlanDetails_Vulkan", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@StartDate", CurrentDate);
                cmd.Parameters.AddWithValue("@PostPoneDate", PostPoneDate);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.Parameters.AddWithValue("@Param", "PostPone");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = sdr["SaveFlag"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return success;
        }
        #endregion
        #region-----------------PM Report Vulkan--------------------------------
        internal static DataTable GetPMReportData(string Machine, string Frequency, int Year, int Month, out DataTable dt2)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            dt2 = new DataTable();
            try
            {
                cmd = new SqlCommand(@"S_GetPMCheckSheetDetails_Vulkan", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", Machine);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.Parameters.AddWithValue("@Year", Year);
                cmd.Parameters.AddWithValue("@Month", Month);
                cmd.Parameters.AddWithValue("@Param", "ReportView");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt2.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return dt;
        }
        #endregion
    }
}