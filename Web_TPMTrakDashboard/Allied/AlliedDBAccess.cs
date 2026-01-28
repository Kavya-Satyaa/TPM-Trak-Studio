using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Util = Web_TPMTrakDashboard.Models.Util;

namespace Web_TPMTrakDashboard.Allied
{
    public class AlliedDBAccess
    {
        #region---------DGDashboardDetails Allied----------------
        internal static List<string> GetMachines()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<string> Machines = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select distinct MachineID from DGEventDetails_Allied", con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Machines.Add(sdr["MachineID"].ToString());
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
            return Machines;
        }

        internal static DataTable GetDashboarddetails(string machineID, string fromdate, string todate)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"select MachineID,EventStarttime,EventEndtime,dbo.f_FormatTime (isnull(EventRuntime,0),'hh:mm:ss') as ActualTime
from DGEventDetails_Allied 
where MachineID=@MachineID and EventStarttime>=@DGStartTime and EventStarttime<=@DGEndTime order by EventStarttime asc", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@DGStartTime", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(fromdate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@DGEndTime", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(todate)).ToString("yyyy-MM-dd HH:mm:ss"));
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
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
            return dt;
        }
        #endregion


        #region UIN Tracking Dahsboard Allied
        public static DataTable GetHydrostaticDashboardData_Allied(string PlantID, string GroupID, string UinNo, string Date)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            //List<UINDashboardData> uinDashboardData = new List<UINDashboardData>();
            try
            {
                SqlCommand cmd = new SqlCommand("S_Get_UinNumberTrackingDetails_Allied", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                cmd.Parameters.AddWithValue("@GroupID", GroupID);
                cmd.Parameters.AddWithValue("@UinNo", UinNo);
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(Date).ToString("yyyy-MM-dd"));
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

            return dt;
        }
        #endregion
        #region------------------------------------------------------------------AM Master Allied---------------------------------------------------------------------------------
        internal static List<string> GetMachineIDs()
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select distinct machineid from machineinformation", con);
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
        internal static List<ListItem> GetRevNos(string MachineID, string Frequency)
        {
            List<ListItem> list = new List<ListItem>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select distinct RevID,RevNo from AM_Master_Allied where MachineID=@MachineID and Frequency=@Frequency order by RevID desc", con);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(new ListItem { Text = sdr["RevNo"].ToString(), Value = sdr["RevID"].ToString() });
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
        internal static string GetMaxRevID(string MachineID, string Frequency)
        {
            string RevNo = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select MAX(RevID) as RevID from AM_Master_Allied where MachineID=@MachineID and Frequency=@Frequency", con);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        RevNo = sdr["RevID"].ToString();
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
            return RevNo;
        }
        internal static string GetMaxRevNo(string MachineID, string Frequency,out string REvID)
        {
            string RevNo = ""; REvID = "";
             SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"SELECT TOP 1 RevNo,RevID
FROM AM_Master_Allied
WHERE RevID = (SELECT MAX(RevID) FROM AM_Master_Allied WHERE MachineID = @MachineID AND Frequency = @Frequency)
  AND MachineID = @MachineID
  AND Frequency = @Frequency", con);
                //cmd.Parameters.AddWithValue("@RevID", RevID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        RevNo = sdr["RevNo"].ToString();
                        REvID = sdr["RevID"].ToString();
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
            return RevNo;
        }
        internal static List<AMMAsterData> GetAMMasterData(string MachineID, string Frequency, int RevID,string param="")
        {
            List<AMMAsterData> list = new List<AMMAsterData>();
            AMMAsterData data = null;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select * from AM_Master_Allied where MachineID=@MachineID and Frequency=@Frequency and RevID=@RevID order by CheckpointID asc", con);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.Parameters.AddWithValue("@RevID", RevID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new AMMAsterData();
                        data.CheckpointID = sdr["CheckpointID"].ToString();
                        data.CheckpointDesc = sdr["CheckpointDescription"].ToString();
                        data.CategoryID = sdr["CategoryID"].ToString();
                        data.CategoryDesc = sdr["Category"].ToString();
                        data.RevDate = Convert.ToDateTime(sdr["RevDate"].ToString()).ToString("dd-MM-yyyy");
                        data.RevNo = sdr["RevNo"].ToString();
                        data.RefNo = sdr["RefNo"].ToString();
                        if(Frequency=="Daily" && param=="VED")
                        {
                            data.DuedateVisibility = false;
                        }
                        else if((Frequency == "Weekly" && param=="VED") || (Frequency=="Monthly" && param=="VED"))
                        {
                            data.DuedateVisibility = true;
                            data.DueDate = Util.GetDateTime( sdr["DueDate"].ToString()).ToString("dd-MM-yyyy");
                        }
                        
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
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return list;
        }
        internal static string SaveAMMasterData(AMMAsterData data,string Param)
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                //                cmd = new SqlCommand(@"if not exists(select * from AM_Master_Allied where MachineID=@MachineID and Frequency=@Frequency and CheckpointID=@CheckpointID)
                //	BEGIN
                //		INSERT INTO AM_Master_Allied(MachineID,CheckpointID,CheckpointDescription,RefNo,RevID,RevNo,RevDate,CategoryID,Category,Frequency)
                //		VALUES(@MachineID,@CheckpointID,@CheckpointDescription,@RefNo,@RevID,@RevNo,@RevDate,@CategoryID,@Category,@Frequency)
                //		select 'INSERTED' as SAVEFLAG
                //	END
                //else
                //	BEGIN
                //		UPDATE AM_Master_Allied set CheckpointDescription=@CheckpointDescription, RefNo=@RefNo,RevID=@RevID,RevNo=@RevNo,RevDate=@RevDate,CategoryID=@CategoryID,Category=@Category
                //		WHERE MachineID=@MachineID and Frequency=@Frequency and CheckpointID=@CheckpointID
                //		select 'UPDATED' as SAVEFLAG
                //	END", con);
                cmd = new SqlCommand(@"if not exists(select * from AM_Master_Allied where MachineID=@MachineID and Frequency=@Frequency and CheckpointID=@CheckpointID)
	BEGIN
		INSERT INTO AM_Master_Allied(MachineID,CheckpointID,CheckpointDescription,RefNo,RevID,RevNo,RevDate,CategoryID,Category,Frequency)
		VALUES(@MachineID,@CheckpointID,@CheckpointDescription,@RefNo,@RevID,@RevNo,@RevDate,@CategoryID,@Category,@Frequency)
		select 'INSERTED' as SAVEFLAG
	END
else
	BEGIN
		if @Param='NEW'
			BEGIN
				select 'CheckpointID already exists' as SAVEFLAG
			END
		else
			BEGIN
				UPDATE AM_Master_Allied set CheckpointDescription=@CheckpointDescription, RefNo=@RefNo,RevID=@RevID,RevNo=@RevNo,RevDate=@RevDate,CategoryID=@CategoryID,Category=@Category
				WHERE MachineID=@MachineID and Frequency=@Frequency and CheckpointID=@CheckpointID
				select 'UPDATED' as SAVEFLAG
			END
	END", con);
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@CheckpointID", data.CheckpointID);
                cmd.Parameters.AddWithValue("@CheckpointDescription", data.CheckpointDesc);
                cmd.Parameters.AddWithValue("@RefNo", data.RefNo);
                cmd.Parameters.AddWithValue("@RevID", data.RevID);
                cmd.Parameters.AddWithValue("@RevNo", data.RevNo);
                cmd.Parameters.AddWithValue("@RevDate", data.RevDate);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@Category", data.CategoryDesc);
                cmd.Parameters.AddWithValue("@Frequency", data.Frequency);
                cmd.Parameters.AddWithValue("@Param", Param);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = sdr["SAVEFLAG"].ToString();
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
        internal static string DeleteAMMasterData(AMMAsterData data)
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"delete from AM_Master_Allied where MachineID=@MachineID and Frequency=@Frequency and CheckpointID=@CheckpointID
select 'DELETED' as SAVEFLAG", con);
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@CheckpointID", data.CheckpointID);
                cmd.Parameters.AddWithValue("@Frequency", data.Frequency);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = sdr["SAVEFLAG"].ToString();
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
        internal static string ImportAMMasterData()
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_GetAMCheckSheetDetails_Allied", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", "Import");
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
            return success;
        }
        internal static void ImportAMMasterDatatoTemp(DataTable dt)
        {
            SqlBulkCopy bulkCopy = default(SqlBulkCopy);
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                //IDD	MachineID	CheckpointID	CheckpointDescription	RefNo	RevID	RevNo	RevDate	CategoryID	Category	Frequency
                bulkCopy = new SqlBulkCopy(con);
                bulkCopy.BulkCopyTimeout = 300;
                bulkCopy.DestinationTableName = "[dbo].[Temp_AM_Master_Allied]";
                bulkCopy.ColumnMappings.Add("MachineID", "MachineID");
                bulkCopy.ColumnMappings.Add("CheckpointID", "CheckpointID");
                bulkCopy.ColumnMappings.Add("CheckpointDescription", "CheckpointDescription");
                bulkCopy.ColumnMappings.Add("RefNo", "RefNo");
                bulkCopy.ColumnMappings.Add("RevID", "RevID");
                bulkCopy.ColumnMappings.Add("RevNo", "RevNo");
                bulkCopy.ColumnMappings.Add("RevDate", "RevDate");
                bulkCopy.ColumnMappings.Add("CategoryID", "CategoryID");
                bulkCopy.ColumnMappings.Add("Category", "Category");
                bulkCopy.ColumnMappings.Add("Frequency", "Frequency");
                bulkCopy.NotifyAfter = 20;
                bulkCopy.SqlRowsCopied += delegate (object sender, SqlRowsCopiedEventArgs e)
                {
                    Logger.WriteDebugLog(string.Format("Row insertion Notifed : {0} rows copied to Table dbo.Temp_AM_Master_Allied .", e.RowsCopied));
                };

                bulkCopy.WriteToServer(dt);
                if (bulkCopy != null) bulkCopy.Close();
                if (con != null) con.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (bulkCopy != null) bulkCopy.Close();
                if (con != null) con.Close();
            }
        }
        internal static void SaveRevisionNo(string RevID, string MachineID, string Frequency)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_GetAMCheckSheetDetails_Allied", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@RevNo", RevID);
                cmd.Parameters.AddWithValue("@RevDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.Parameters.AddWithValue("@Param", "Save_NewRevDetails");
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
        internal static string CopyAMasterData_Allied(string SourceMachine, string destinationMachine, string Frequency)
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_GetAMCheckSheetDetails_Allied", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", destinationMachine);
                cmd.Parameters.AddWithValue("@OldMachineID", SourceMachine);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.Parameters.AddWithValue("@Param", "CopyOldMachineToNewMachine");
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
        internal static DataTable GetAMTransactiondata_Allied(string MachineID, int Year, int Month, out DataTable dt1, string Frequency)
        {
            DataTable dt = new DataTable();
            dt1 = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_GetAMCheckSheetDetails_Allied", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@YearNo", Year);
                cmd.Parameters.AddWithValue("@MonthNo", Month);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.Parameters.AddWithValue("@Param", "Reportview");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.AcceptChanges();
                    dt1.Load(sdr);
                    dt1.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return dt;
        }
        #endregion-----------------------------------------------------------------------------------------------------------------------------------------------------------------

    }
}