using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;

namespace DataBaseClassLibrary
{
    public class ConnectionManager
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings[0].ToString();

        public static SqlConnection GetConnection()
        {
            bool writeDown = false;
            DateTime dt = DateTime.Now;
            SqlConnection conn = null;
            if (HttpContext.Current.Session["connectionString"] == null)
                conn = new SqlConnection(connectionString);
            else
            {
                connectionString = HttpContext.Current.Session["connectionString"] as string;
                connectionString = ConfigurationManager.ConnectionStrings[connectionString].ToString();
                conn = new SqlConnection(connectionString);
            }
            do
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    if (writeDown == false)
                    {
                        Logger.WriteErrorLog(ex.Message);
                        dt = DateTime.Now.AddSeconds(60);
                        writeDown = true;
                    }
                    if (dt < DateTime.Now)
                    {
                        Logger.WriteErrorLog(ex.Message);
                        break;
                    }
                    Thread.Sleep(1000);
                }

            } while (conn.State != ConnectionState.Open);
            return conn;
        }
    }
}
