using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class Locations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<LocationDetails> GetAllLocationDetails()
        {
            List<LocationDetails> locationsList = new List<LocationDetails>();
            try
            {
                string connstr = ConfigurationManager.ConnectionStrings[0].ConnectionString;
                if (!string.IsNullOrEmpty(connstr))
                {
                    SqlConnection sqlConnection = new SqlConnection(connstr);
                    SqlCommand cmd = new SqlCommand("select * from LocationDetails", sqlConnection);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandTimeout = 240;
                    sqlConnection.Open();
                    SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            locationsList.Add(new LocationDetails()
                            {
                                Name = reader["Name"] != null ? reader["Name"].ToString() : "",
                                IconImage = reader["IconImage"] != null ? reader["IconImage"].ToString() : "",
                                Address = reader["Address"] != null ? reader["Address"].ToString() : "",
                                Content = reader["Content"] != null ? reader["Content"].ToString() : "",
                                Details = (reader["Address"] != null ? reader["Address"].ToString() : "") + (reader["Content"] != null ? reader["Content"].ToString() : ""),
                                Latitude = reader["Latitude"] != null ? Convert.ToDouble(reader["Latitude"]) : 0.0000,
                                Longitude = reader["Longitude"] != null ? Convert.ToDouble(reader["Longitude"]) : 0.0000,
                                dbName = reader["DBName"] != null ? reader["DBName"].ToString() : "",
                                OEEString = !string.IsNullOrEmpty(reader["DBName"].ToString()) ? GetPlantOEE(reader["DBName"].ToString()) : ""
                            });
                        }
                    }
                    if (reader != null) reader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"Error : {ex.Message}");
            }
            return locationsList;
        }

        private static string GetPlantOEE(string dbName)
        {
            string oeeString = string.Empty;
            try
            {
                List<double> oeeList = new List<double>();
                if (ConfigurationManager.ConnectionStrings[dbName] != null)
                {
                    string connstr = ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
                    if (!string.IsNullOrEmpty(connstr))
                    {
                        SqlConnection sqlConnection = new SqlConnection(connstr);
                        SqlCommand cmd = new SqlCommand("s_GetANDONHelpCodeDetails_ADVIK", sqlConnection);
                        cmd.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@PlantID", "");
                        cmd.Parameters.AddWithValue("@GroupID", "");
                        cmd.Parameters.AddWithValue("@Param", "");
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandTimeout = 240;
                        sqlConnection.Open();
                        SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        if(reader.NextResult())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    if (reader["OEE"] != null)
                                    {
                                        if (Convert.ToDouble(reader["OEE"].ToString()) != 0)
                                        {
                                            oeeString += $"<strong>{reader["PlantID"]} :</strong> {Math.Round(Convert.ToDouble(reader["OEE"]), 2)}%<br>";
                                        }
                                    }
                                }
                            }
                        }                        
                        if (reader != null) reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"Error : {ex.Message}");
            }
            return oeeString;
        }
    }
}