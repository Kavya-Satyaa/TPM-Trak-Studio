using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Elmah;
using Web_TPMTrakDashboard;

namespace SonaAndon.Models
{
	public class DataBase
	{
		//#region ------Bind Plant To Display----------
		//public static List<string> ViewPlantToDisplay()
		//{
		//	SqlConnection sqlConn = ConnectionManager.GetConnection();
		//	if (sqlConn.State == ConnectionState.Closed) return null;
		//	SqlCommand cmd = null;
		//	SqlDataReader sdr = null;
		//	List<string> lstPlantData = new List<string>();
		//	try
		//	{
		//		cmd = new SqlCommand(@"[s_GetLookups]", sqlConn);
		//		cmd.CommandType = System.Data.CommandType.StoredProcedure;
		//		cmd.Parameters.AddWithValue("@name", "Plant");
		//		cmd.CommandTimeout = 120;
		//		sdr = cmd.ExecuteReader();
		//		if (sdr.HasRows)
		//		{
		//			while (sdr.Read())
		//			{
		//				lstPlantData.Add(sdr["Plantid"].ToString());
		//			}
		//		}
		//		sdr.Close();
		//	}
		//	catch (Exception ex)
		//	{
		//		ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
		//		throw;
		//	}
		//	finally
		//	{
		//		if (sdr != null) sdr.Close();
		//		if (sqlConn != null) sqlConn.Close();
		//	}
		//	return lstPlantData;
		//}

		//#endregion

		//#region --------------Bind Shift Data--------------
		//internal static List<string> GetAllShift()
		//{
		//	SqlConnection sqlConn = ConnectionManager.GetConnection();
		//	List<string> shiftList = new List<string>();
		//	try
		//	{
		//		SqlCommand cmd = new SqlCommand(@"select * from shiftDetails where running = 1", sqlConn);
		//		//shiftList.Add("All");
		//		SqlDataReader rdr = cmd.ExecuteReader();
		//		while (rdr.Read())
		//		{
		//			shiftList.Add(rdr["shiftName"].ToString());
		//		}
		//		rdr.Close();
		//	}
		//	catch (Exception ex)
		//	{
		//		ErrorSignal.FromCurrentContext().Raise(ex);
		//	}
		//	finally
		//	{
		//		if (sqlConn != null) sqlConn.Close();
		//	}
		//	return shiftList;
		//}
		//#endregion

		//#region "Populate Current Shift"
		//internal static string GetShift()
		//{
		//	string shift = string.Empty;

		//	SqlConnection sqlConn = ConnectionManager.GetConnection();
		//	if (sqlConn.State == ConnectionState.Closed) return null;
		//	try
		//	{
		//		SqlCommand cmd = new SqlCommand(@"[s_GetCurrentShift]", sqlConn);
		//		cmd.CommandType = System.Data.CommandType.StoredProcedure;
		//		cmd.CommandTimeout = 120;
		//		SqlDataReader sdr = cmd.ExecuteReader();
		//		if (sdr.HasRows)
		//		{
		//			while (sdr.Read())
		//			{
		//				shift = sdr["ShiftName"].ToString();
		//			}
		//		}
		//		sdr.Close();
		//	}
		//	catch (Exception ex)
		//	{
		//		Logger.WriteErrorLog(ex.ToString());
		//		Logger.WriteErrorLog(ex.ToString());
		//		throw;
		//	}
		//	finally
		//	{
		//		if (sqlConn != null) sqlConn.Close();
		//	}
		//	return shift;

		//}
		//#endregion

		//#region "Bind Andon Page Gridview"
		//public static List<DataModel> BindAndonGridView(string date, string plantId, string shift)
		//{
		//	SqlConnection conn = ConnectionManager.GetConnection();
		//	List<DataModel> data = new List<DataModel>();
		//	try
		//	{
		//		Logger.WriteErrorLog("Calling proc............." + date + "    " + plantId + "shft " + shift);
		//		SqlCommand cmd = new SqlCommand(@"[s_GetBajaMotors_ProdDownReport]", conn);
		//		cmd.CommandType = System.Data.CommandType.StoredProcedure;
		//		cmd.CommandTimeout = 120;
		//		cmd.Parameters.AddWithValue("@StartDate", date);
		//		cmd.Parameters.AddWithValue("@Shift", shift);
		//		cmd.Parameters.AddWithValue("@PlantID", plantId);
		//		cmd.Parameters.AddWithValue("@MachineID", "");
		//		cmd.Parameters.AddWithValue("@Param", "Andon");
		//		SqlDataReader sdr = cmd.ExecuteReader();
		//		if (sdr.HasRows)
		//		{
		//			Logger.WriteErrorLog("result found");
		//			while (sdr.Read())
		//			{
		//				DataModel obj = new DataModel();
		//				string mc = sdr["MachineID"].ToString();
		//				obj.MachineID = mc;   //mc.Substring(0, 6);
		//				obj.Operation = sdr["Operation"].ToString();
		//				if (sdr["Actual"] != DBNull.Value)
		//					obj.Actual = Convert.ToInt32(sdr["Actual"]);
		//				if (sdr["Target"] != DBNull.Value)
		//					obj.Target = Convert.ToInt32(sdr["Target"]);
		//				if (sdr["MachineStatus"].ToString().Equals("Yellow", StringComparison.OrdinalIgnoreCase))
		//					obj.MachineStatus = "Images/Status/Yellow.png";
		//				else if (sdr["MachineStatus"].ToString().Equals("Green", StringComparison.OrdinalIgnoreCase))
		//					obj.MachineStatus = "Images/Status/Green.png";
		//				else if (sdr["MachineStatus"].ToString().Equals("Red", StringComparison.OrdinalIgnoreCase))
		//					obj.MachineStatus = "Images/Status/Red.png";
		//				else
		//					obj.MachineStatus = "Images/Status/Deault.png";
		//				if (sdr["OEE"] != DBNull.Value)
		//					obj.OEE = Convert.ToDecimal(sdr["OEE"]);
		//				if (sdr["PE"] != DBNull.Value)
		//					obj.PE = Convert.ToDecimal(sdr["PE"]);
		//				if (sdr["AE"] != DBNull.Value)
		//					obj.AE = Convert.ToDecimal(sdr["AE"]);
		//				obj.Component = sdr["Component"].ToString();
		//				data.Add(obj);
		//			}
		//		}
		//		else
		//		{
		//			Logger.WriteErrorLog("result NOT found");
		//		}
		//		sdr.Close();
		//	}
		//	catch (Exception ex)
		//	{
		//		Logger.WriteErrorLog(ex.ToString());
		//		ErrorSignal.FromCurrentContext().Raise(ex);
		//		throw;
		//	}
		//	finally
		//	{
		//		if (conn != null) conn.Close();
		//	}
		//	return data;
		//}
		//#endregion

		//#region ----------Globel View Setting Part--------------

		//internal static AppPageSetting ViewAppPageSettings(string Param)
		//{
		//	SqlConnection sqlConn = ConnectionManager.GetConnection();
		//	AppPageSetting settings = new AppPageSetting();
		//	try
		//	{
		//		SqlCommand cmd = new SqlCommand("select [ValueInText], [ValueInText2] from dbo.AndonDefaults where [Parameter] = @Param", sqlConn);
		//		cmd.Parameters.AddWithValue("@Param", Param);
		//		SqlDataReader sdr = cmd.ExecuteReader();
		//		if (sdr.HasRows)
		//		{
		//			while (sdr.Read())
		//			{
		//				if (sdr["ValueInText"].Equals("BajajAndon_ComponentSetting") || sdr["ValueInText"].Equals("BajajLED_ComponentSetting"))
		//				{
		//					settings.ShowDataFor = (sdr["ValueInText2"]).ToString();
		//				}
		//				else if (sdr["ValueInText"].Equals("BajajAndon_DataInterval") || sdr["ValueInText"].Equals("BajajLED_DataInterval"))
		//				{
		//					settings.ShowDataBy = (sdr["ValueInText2"]).ToString();
		//				}
		//				else if (sdr["ValueInText"].Equals("BajajAndon_TargetSetting") || sdr["ValueInText"].Equals("BajajLED_TargetSetting"))
		//				{
		//					settings.Target = (sdr["ValueInText2"]).ToString();
		//				}
		//				else if (sdr["ValueInText"].Equals("BajajAndon_CalculateTargetTill") || sdr["ValueInText"].Equals("BajajLED_CalculateTargetTill"))
		//				{
		//					settings.CalculateTarget = (sdr["ValueInText2"]).ToString();
		//				}
		//			}
		//		}
		//		sdr.Close();
		//	}
		//	catch (Exception ex)
		//	{
		//		ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
		//		throw;
		//	}
		//	finally
		//	{
		//		if (sqlConn != null) sqlConn.Close();
		//	}
		//	return settings;
		//}
		//#endregion

		//#region "Update Application Page Setting Records "
		//internal static int SaveAppPageSettings(string Param, string valueInText2, string valueInText)
		//{
		//	SqlConnection sqlConn = ConnectionManager.GetConnection();
		//	int result;
		//	try
		//	{
		//		SqlCommand cmd = new SqlCommand("update AndonDefaults set  [ValueInText2] = @ValueInText2 where [Parameter] = @Param and ValueInText = @ValueInText", sqlConn);
		//		cmd.Parameters.AddWithValue("@Param", Param);
		//		cmd.Parameters.AddWithValue("@ValueInText2", valueInText2);
		//		cmd.Parameters.AddWithValue("@ValueInText", valueInText);
		//		result = cmd.ExecuteNonQuery();
		//	}
		//	catch (Exception ex)
		//	{
		//		ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
		//		throw;
		//	}
		//	finally
		//	{
		//		if (sqlConn != null) sqlConn.Close();
		//	}
		//	return result;
		//}
		//#endregion

		//#region "-------Bind Column View Setting------"
		//internal static List<ColumnViewSetting> BindCockpitSetting()
		//{
		//	SqlConnection sqlConn = ConnectionManager.GetConnection();
		//	List<ColumnViewSetting> lstModelData = new List<ColumnViewSetting>();
		//	try
		//	{
		//		SqlCommand cmd = new SqlCommand("select * from dbo.AndonDefaults where [Parameter] = @Param order by ValueInInt", sqlConn);
		//		cmd.Parameters.AddWithValue("@param", "BajajAndonViewColumn");
		//		cmd.CommandTimeout = 120;
		//		SqlDataReader rdr = cmd.ExecuteReader();
		//		while (rdr.Read())
		//		{
		//			ColumnViewSetting modelData = new ColumnViewSetting();
		//			modelData.Parameter = rdr["Parameter"].ToString();
		//			modelData.ValueInText = rdr["ValueInText"].ToString();
		//			modelData.ValueInText2 = rdr["ValueInText2"].ToString();
		//			if (string.IsNullOrEmpty(rdr["ValueInInt"].ToString()))
		//				modelData.ValueInInt = 0;
		//			else
		//				modelData.ValueInInt = (int)rdr["ValueInInt"];
		//			if (string.IsNullOrEmpty(rdr["ValueInBool"].ToString()) || rdr["ValueInBool"].ToString() == "1")
		//				modelData.ValueInBool = true;
		//			else
		//				modelData.ValueInBool = false;

		//			lstModelData.Add(modelData);
		//		}
		//		rdr.Close();
		//	}
		//	catch (Exception ex)
		//	{
		//		ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
		//		throw;
		//	}
		//	finally
		//	{
		//		if (sqlConn != null) sqlConn.Close();
		//	}
		//	return lstModelData;
		//}
		//#endregion

		//#region "Update View Column Setting Records "
		//internal static int UpdateColumnViewSettings(string Param, string valueInText2, string valueInText, int ValueInBool)
		//{
		//	SqlConnection sqlConn = ConnectionManager.GetConnection();
		//	int result;
		//	try
		//	{
		//		SqlCommand cmd = new SqlCommand("update AndonDefaults set  [ValueInText2] = @ValueInText2 , [ValueInBool] = @ValueInBool where [Parameter] = @Param and ValueInText = @ValueInText", sqlConn);
		//		cmd.Parameters.AddWithValue("@Param", Param);
		//		cmd.Parameters.AddWithValue("@ValueInText2", valueInText2);
		//		cmd.Parameters.AddWithValue("@ValueInText", valueInText);
		//		cmd.Parameters.AddWithValue("@ValueInBool", ValueInBool);
		//		result = cmd.ExecuteNonQuery();
		//	}
		//	catch (Exception ex)
		//	{
		//		ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
		//		throw;
		//	}
		//	finally
		//	{
		//		if (sqlConn != null) sqlConn.Close();
		//	}
		//	return result;
		//}
		//#endregion

		//#region "Table Header Informaction"
		//internal static List<string> tableHeaderData(out List<string> listOfColNames)
		//{
		//	SqlConnection sqlConn = ConnectionManager.GetConnection();
		//	List<string> list = new List<string>();
		//	listOfColNames = new List<string>();

		//	try
		//	{
		//		SqlCommand cmd = new SqlCommand("select * from [AndonDefaults] where parameter= @param and ValueInBool=1 order by ValueInInt ", sqlConn);
		//		cmd.Parameters.AddWithValue("@param", "BajajAndonViewColumn");
		//		cmd.CommandTimeout = 120;
		//		SqlDataReader rdr = cmd.ExecuteReader();
		//		while (rdr.Read())
		//		{
		//			list.Add(rdr["ValueInText2"].ToString());
		//			listOfColNames.Add(rdr["ValueInText"].ToString());
		//		}
		//		rdr.Close();
		//	}
		//	catch (Exception ex)
		//	{
		//		//Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
		//		ErrorSignal.FromCurrentContext().Raise(ex);
		//		throw;
		//	}
		//	finally
		//	{
		//		if (sqlConn != null) sqlConn.Close();
		//	}
		//	return list;
		//}
		//#endregion

		//#region "Table Item Information Data"
		//internal static DataTable GetMachinetableview(string date, string plantId, string shift)
		//{
		//	SqlConnection conn = ConnectionManager.GetConnection();
		//	DataTable dt = new DataTable();
		//	try
		//	{
		//		SqlCommand cmd = new SqlCommand(@"[s_GetBajaMotors_ProdDownReport]", conn);
		//		cmd.CommandType = System.Data.CommandType.StoredProcedure;
		//		cmd.CommandTimeout = 120;
		//		cmd.Parameters.AddWithValue("@StartDate", date);
		//		cmd.Parameters.AddWithValue("@Shift", shift);
		//		cmd.Parameters.AddWithValue("@PlantID", plantId);
		//		cmd.Parameters.AddWithValue("@MachineID", "");
		//		cmd.Parameters.AddWithValue("@Param", "Andon");
		//		SqlDataReader sdr = cmd.ExecuteReader();
		//		//if (sdr.HasRows)
		//		{
		//			dt.Load(sdr);
		//			foreach (DataRow row in dt.Rows)
		//			{
		//				if (row["MachineStatus"].ToString().Equals("Yellow", StringComparison.OrdinalIgnoreCase))
		//					row["MachineStatus"] = "Images/Status/Yellow.png";
		//				else if (row["MachineStatus"].ToString().Equals("Green", StringComparison.OrdinalIgnoreCase))
		//					row["MachineStatus"] = "Images/Status/Green.png";
		//				else if (row["MachineStatus"].ToString().Equals("Red", StringComparison.OrdinalIgnoreCase))
		//					row["MachineStatus"] = "Images/Status/Red.png";
		//				else
		//					row["MachineStatus"] = "Images/Status/Deault.png";
		//			}
		//			dt.AcceptChanges();
		//		}
		//		sdr.Close();

		//	}
		//	catch (Exception ex)
		//	{
		//		ErrorSignal.FromCurrentContext().Raise(ex);
		//		throw;
		//	}
		//	finally
		//	{
		//		if (conn != null) conn.Close();
		//	}
		//	return dt;
		//}
		//#endregion

		#region SonaAndon
		internal static string GetShiftstart(out string ShiftEnd)
		{
			ShiftEnd = "";
			string Shiftstart = string.Empty;
			SqlConnection conn = ConnectionManager.GetConnection();
			SqlCommand cmd = null;
			SqlDataReader rdr = null;
			try
			{
				cmd = new SqlCommand("s_GetCurrentShift", conn);
				cmd.CommandType = CommandType.StoredProcedure;
				rdr = cmd.ExecuteReader();
				if (rdr.HasRows)
				{
					while (rdr.Read())
					{
						Shiftstart = rdr["StartTime"].ToString();
						ShiftEnd = rdr["EndTime"].ToString();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.WriteErrorLog(ex.Message);
				throw;
			}
			finally
			{
				if (conn != null) conn.Close();
				if (rdr != null) rdr.Close();
			}
			return Shiftstart;
		}

		internal static List<SonaAndonEntity> GetSonaAndondata(string ShiftStart)
		{
			List<SonaAndonEntity> SonadAndondataList = new List<SonaAndonEntity>();
			SonaAndonEntity SonaAndondata = null;
			SqlConnection conn = ConnectionManager.GetConnection();
			SqlCommand cmd = null;
			SqlDataReader rdr = null;
			try
			{
				//cmd = new SqlCommand("S_GetSONA_AndonProductionStatus", conn);
                cmd = new SqlCommand("S_GetSONA_AndonProdAndDownStatus", conn);
                cmd.Parameters.AddWithValue("@StartTime", ShiftStart);
				cmd.Parameters.AddWithValue("@EndTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 90;
				rdr = cmd.ExecuteReader();
				if (rdr.HasRows)
				{
					while (rdr.Read())
					{
						SonaAndondata = new SonaAndonEntity();
						SonaAndondata.WorkCenter = rdr["MachineID"].ToString();
						SonaAndondata.MachineDescription = rdr["MachineDescription"].ToString();
						SonaAndondata.PartNumber = rdr["Component"].ToString();
						SonaAndondata.DownReason = rdr["ProcessStatus"].ToString();
                        SonaAndondata.Scheduleqty = rdr["ScheduledQty"].ToString() == "0" ? "" : rdr["ScheduledQty"].ToString();
                        SonaAndondata.Target = rdr["TargetQty"].ToString() == "0" ? "" : rdr["TargetQty"].ToString();
                        SonaAndondata.Completedtime = rdr["Components"].ToString() == "0" ? "" : rdr["Components"].ToString();
                        SonaAndondata.OEE = rdr["OverallEfficiency"].ToString() == "0" ? "" : rdr["OverallEfficiency"].ToString();
                        SonaAndondata.Status = rdr["MCStatus"].ToString();
						if (rdr["WheelStatus"].ToString().Equals("Running", StringComparison.OrdinalIgnoreCase))
						{
							SonaAndondata.OKNOT = "Running";
							SonaAndondata.Image = "~/SonaAndon/Images/MacStatus/Running.gif";
							SonaAndondata.Visibility = "true";
						}
							
						else if(rdr["WheelStatus"].ToString().Equals("Stopped", StringComparison.OrdinalIgnoreCase))
						{
							SonaAndondata.OKNOT = "Stopped";
							SonaAndondata.Image = "~/SonaAndon/Images/MacStatus/Stopped.gif";
							SonaAndondata.Visibility = "true";
						}
						else
							SonaAndondata.Visibility = "false";
						SonadAndondataList.Add(SonaAndondata);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.WriteErrorLog(ex.Message);
				throw;
			}
			finally
			{
				if (conn != null) conn.Close();
				if (rdr != null) rdr.Close();
			}
			return SonadAndondataList;
		}

		internal static List<fontstyling> GetFontstyling()
		{
			List<fontstyling> fontstyling = new List<fontstyling>();
			fontstyling fontstyle = null;
			SqlConnection conn = ConnectionManager.GetConnection();
			SqlCommand cmd = null;
			SqlDataReader rdr = null;
			try
			{
				cmd = new SqlCommand("Select * from [AndonDefaults] where Parameter='AndonSonaSettings'", conn);
				rdr = cmd.ExecuteReader();
				if(rdr.HasRows)
				{
					fontstyle = new fontstyling();
					while (rdr.Read())
					{
						switch(rdr["ValueInText"].ToString())
						{
							case "Font Family":
								fontstyle.fontfamily = rdr["ValueInText2"].ToString();
								break;
							case "Font Size":
								fontstyle.fontsize = Convert.ToInt32(rdr["ValueInText2"].ToString());
								break;
							case "Font Style":
								fontstyle.fontstyle = rdr["ValueInText2"].ToString();
								break;
							case "Number of Rows":
								fontstyle.Rows = Convert.ToInt16(rdr["ValueInText2"].ToString());
								break;
							case "Header Font Style":
								fontstyle.Headerfontsize = Convert.ToInt16(rdr["ValueInText2"].ToString());
								break;
							case "Header Color":
								fontstyle.HeaderColor = rdr["ValueInText2"].ToString();
								break;
							case "Row Background Color":
								fontstyle.RowColor = rdr["ValueInText2"].ToString();
								break;
							case "Alternative Row Color":
								fontstyle.AlternativeRowColor = rdr["ValueInText2"].ToString();
								break;
						}
					}
					fontstyling.Add(fontstyle);
				}
				else
				{
					fontstyle = new fontstyling();
					fontstyle.fontfamily = "Verdana";	
					fontstyle.fontsize = 17;
					fontstyle.fontstyle = "Normal";
					fontstyle.Rows = 10;
					fontstyle.Headerfontsize = 20;
					fontstyle.HeaderColor = "#2665B2";
					fontstyle.RowColor = "#CBEFFF";
					fontstyle.AlternativeRowColor = "#F5F5F5";
					fontstyling.Add(fontstyle);
				}
			}
			catch (Exception ex)
			{
				Logger.WriteErrorLog(ex.Message);
				throw;
			}
			finally
			{
				if (conn != null) conn.Close();
				if (rdr != null) rdr.Close();
			}
			return fontstyling;
		}

		internal static void SaveFontstyling(string Parameter,string ValueInText,string ValueInText2)
		{
			SqlConnection conn = ConnectionManager.GetConnection();
			SqlCommand cmd = null;
			string query = @" if not exists (select * from [AndonDefaults] where Parameter = @Parameter and ValueInText = @ValueInText)
								BEGIN
								Insert into [AndonDefaults](Parameter,ValueInText,ValueInText2)
								Values (@Parameter,@ValueInText,@ValueInText2)
								 END
									ELSE BEGIN
								 UPDATE  [AndonDefaults] Set ValueInText=@ValueInText , ValueInText2=@ValueInText2 where Parameter=@Parameter
								 END";
			try
			{
				cmd = new SqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@Parameter", Parameter);
				cmd.Parameters.AddWithValue("@ValueInText", ValueInText);
				cmd.Parameters.AddWithValue("@ValueInText2", ValueInText2);
				cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				Logger.WriteErrorLog(ex.Message);
				throw;
			}
			finally
			{
				if (conn != null) conn.Close();
			}
		}
		#endregion
	}
}