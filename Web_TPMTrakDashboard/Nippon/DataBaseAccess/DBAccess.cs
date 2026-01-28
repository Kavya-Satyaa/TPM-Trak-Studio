using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Web_TPMTrakDashboard.Nippon.Model;

namespace Web_TPMTrakDashboard.Nippon.DataBaseAccess
{
    public class DBAccess
    {
        public static List<AndonSettingData> getAndonMachineSettingDetails()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<AndonSettingData> list = new List<AndonSettingData>();
            AndonSettingData data = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select m.machineid,c.Parameter,c.ValueInText2 from machineinformation m inner join MachinewiseSortOrder ms on m.machineid=ms.Machineid  left join
CockpitDefaults c on m.machineid = c.ValueInText order by ms.SortOrder", con);
                // cmd.Parameters.AddWithValue("@valueintext", valueintext);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new AndonSettingData();
                        data.Parameter = sdr["Parameter"].ToString();
                        data.MachineID = sdr["machineid"].ToString();
                        if (!string.IsNullOrEmpty(sdr["ValueInText2"].ToString()))
                        {
                            data.ImagePath = "~/Nippon/AndonMachineImages/" + sdr["ValueInText2"].ToString();
                        }
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        public static List<AndonSettingData> getAndonSettingDetails()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<AndonSettingData> list = new List<AndonSettingData>();
            AndonSettingData data = null;
            try
            {
                SqlCommand cmd = new SqlCommand("select * from CockpitDefaults where  Parameter in ('NipponAndonSetting')", con);
               // cmd.Parameters.AddWithValue("@valueintext", valueintext);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new AndonSettingData();
                        data.Parameter = sdr["Parameter"].ToString();
                        data.ValueInText = sdr["ValueInText"].ToString();
                        if (!string.IsNullOrEmpty(sdr["ValueInText2"].ToString()))
                        {
                            data.ImagePath = "~/Nippon/AndonMachineImages/" + sdr["ValueInText2"].ToString();
                        }
                        data.SortOrder = sdr["ValueInInt"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        public static int saveAndonMachineDetails(AndonSettingData data)
        {
            int result = 0;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"if exists(select * from CockpitDefaults where Parameter='NipponAndonMachineSetting' and ValueInText=@valueintext)
begin
  update CockpitDefaults set ValueInText2=@valueintext2 where Parameter='NipponAndonMachineSetting' and ValueInText=@valueintext
end
else
begin
 insert into CockpitDefaults(Parameter,ValueInText,ValueInText2) values('NipponAndonMachineSetting',@valueintext,@valueintext2)
end", con);
                 cmd.Parameters.AddWithValue("@valueintext", data.MachineID);
                cmd.Parameters.AddWithValue("@valueintext2", data.ImageName);
              //  cmd.Parameters.AddWithValue("@valueinint", data.SortOrder);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return result;
        }
        public static int saveAndonBackgroungImageDetails(AndonSettingData data)
        {
            int result=0;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"if exists(select * from CockpitDefaults where Parameter='NipponAndonSetting' and ValueInText='BackgroundImage')
begin
  update CockpitDefaults set ValueInText2=@valueintext2 where Parameter='NipponAndonSetting' and ValueInText='BackgroundImage'
end
else
begin
 insert into CockpitDefaults(Parameter,ValueInText,ValueInText2) values('NipponAndonSetting','BackgroundImage',@valueintext2)
end", con);
                cmd.Parameters.AddWithValue("@valueintext2", data.ImageName);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return result;
        }
        public static List<AndonSettingData> getAndonMachineDetails()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<AndonSettingData> list = new List<AndonSettingData>();
            AndonSettingData data = null;
            try
            {
                SqlCommand cmd = new SqlCommand("select * from CockpitDefaults where  Parameter in ('NipponAndonMachineSetting','NipponAndonSetting')", con);
                // cmd.Parameters.AddWithValue("@valueintext", valueintext);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new AndonSettingData();
                        data.Parameter = sdr["Parameter"].ToString();
                        data.MachineID = sdr["ValueInText"].ToString();
                        if (!string.IsNullOrEmpty(sdr["ValueInText2"].ToString()))
                        {
                            data.ImagePath = "AndonMachineImages/" + sdr["ValueInText2"].ToString();
                        }
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
    }
}