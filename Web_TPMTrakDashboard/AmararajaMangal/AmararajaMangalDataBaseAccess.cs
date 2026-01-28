using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using static Web_TPMTrakDashboard.AmararajaMangal.DTO;

namespace Web_TPMTrakDashboard.AmararajaMangal
{
    public class AmararajaMangalDataBaseAccess
    {
        #region "---UploadtoHMI----"
        internal static List<CockpitViewSettingClass> getShopDefaultValue(string parameter)
        {
            List<CockpitViewSettingClass> list = new List<CockpitViewSettingClass>();
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from ShopDefaults where Parameter=@parameter", Conn);
                cmd.Parameters.AddWithValue("@parameter", parameter);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    CockpitViewSettingClass data = new CockpitViewSettingClass();
                    data.Parameter = rdr["Parameter"].ToString();
                    data.ValueInText = rdr["ValueInText"].ToString();
                    if (!string.IsNullOrEmpty(rdr["ValueInInt"].ToString()))
                    {
                        data.ValueInInt = Convert.ToInt32(rdr["ValueInInt"].ToString());
                    }
                    else
                    {
                        data.ValueInInt = 0;
                    }
                    list.Add(data);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return list;
        }
        internal static List<string> GetPlantID()
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<string> entities = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"Select distinct PlantId from Plantinformation", Conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    entities.Add(rdr["PlantId"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return entities;
        }

        internal static List<string> GetMachineID(string plantID)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<string> machlist = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select distinct MI.MachineId from machineinformation MI inner join plantmachine PIn on MI.machineid = PIn.machineid where MI.tpmtrakenabled=1  and PIn.plantid=@plantID", Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@plantID", plantID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    machlist.Add(rdr["MachineId"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return machlist;
        }

        internal static List<string> GetCategory()
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<string> Catlst = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select distinct DownCategory from DownCategoryInformation ", Conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Catlst.Add(rdr["DownCategory"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return Catlst;
        }
        internal static List<string> GetRejectionCategory()
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<string> Catlst = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select distinct Catagory from rejectioncodeinformation ", Conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Catlst.Add(rdr["Catagory"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return Catlst;
        }
        internal static List<HMIEntity> GetAvailableCompDetails(string machineID)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<HMIEntity> Complst = new List<HMIEntity>();
            try
            {
                //SqlCommand cmd = new SqlCommand(@"SELECT DISTINCT T.MachineId,T.description, T.InterfaceId,ROW_NUMBER() OVER(ORDER BY T.MachineId,T.Componentid,T.InterfaceId) AS row,T.Componentid FROM( select DISTINCT MI.MachineId AS MachineId,CI.description AS description, CI.InterfaceId AS InterfaceId, CI.Componentid AS Componentid  from componentinformation CI INNER join ComponentOperationPricing COP on CI.Componentid = COP.Componentid INNER JOIN MachineInformation MI on MI.MachineId = COP.MachineID where MI.MachineID in (" + machineID + ") and MI.TpmTrakenabled =1 )T ORDER BY T.MachineId,T.Componentid,T.InterfaceId", Conn);
                SqlCommand cmd = new SqlCommand(@"SELECT DISTINCT T.MachineId,T.description, T.InterfaceId,ROW_NUMBER() OVER(ORDER BY T.MachineId,T.Componentid,T.InterfaceId) AS row,
T.Componentid FROM( select DISTINCT MI.MachineId AS MachineId,CI.description AS description, CI.InterfaceId AS InterfaceId, CI.Componentid AS Componentid 
from componentinformation CI INNER join ComponentOperationPricing COP on CI.Componentid = COP.Componentid INNER JOIN MachineInformation MI on MI.MachineId = COP.MachineID 
where MI.MachineID =@MachineID and MI.TpmTrakenabled =1 
and CI.InterfaceID not in (select distinct CompInterfaceID from MachineWiseComponentMaster where MachineID=@MachineID)
)T ORDER BY T.MachineId,T.Componentid,T.InterfaceId", Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    HMIEntity ent = new HMIEntity();
                    ent.Slno = Convert.ToInt16(rdr["row"]);
                    ent.MachineID = rdr["MachineId"].ToString();
                    ent.InterfaceID = rdr["InterfaceId"].ToString();
                    ent.ComponentID = rdr["Componentid"].ToString();
                    ent.Description = rdr["description"].ToString();
                    Complst.Add(ent);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return Complst;
        }

        internal static List<HMIEntity> GetAvailableDownInfo(string downcat, string machine)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<HMIEntity> Downlst = new List<HMIEntity>();
            try
            {
                //SqlCommand cmd = new SqlCommand(@"select ROW_NUMBER() OVER(ORDER BY DownID) AS row, '' as MachineId,
                //    dci.InterfaceID, dci.DownID,dci.Catagory,dci.downdescription from dbo.downcodeinformation dci where dci.Catagory in (" + downcat + ") and isnumeric(interfaceid)>0 ORDER BY DownID", Conn);
                SqlCommand cmd = new SqlCommand(@"select ROW_NUMBER() OVER(ORDER BY DownID) AS row, '' as MachineId,
                    dci.InterfaceID, dci.DownID,dci.Catagory,dci.downdescription from dbo.downcodeinformation dci where dci.Catagory in (" + downcat + ") and isnumeric(interfaceid)>0 and dci.downid not in (select distinct downid from MachineWiseDownMaster where MachineID=@machine) ORDER BY DownID", Conn);
                cmd.Parameters.AddWithValue("@machine", machine);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    HMIEntity ent = new HMIEntity();
                    ent.Slno = Convert.ToInt16(rdr["row"]);
                    ent.MachineID = rdr["MachineId"].ToString();
                    ent.InterfaceID = rdr["InterfaceId"].ToString();
                    ent.DownID = rdr["DownID"].ToString();
                    ent.Description = rdr["downdescription"].ToString();
                    ent.DownCategory = rdr["Catagory"].ToString();
                    Downlst.Add(ent);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return Downlst;
        }
        internal static List<HMIEntity> GetAvailableRejectionCodeInfo(string rejCat, string machine)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<HMIEntity> Downlst = new List<HMIEntity>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select ROW_NUMBER() OVER(ORDER BY rejectionid) AS row, '' as MachineId,
                    dci.InterfaceID, dci.rejectionid,dci.Catagory,dci.rejectiondescription from dbo.rejectioncodeinformation dci where 
					dci.Catagory in (" + rejCat + ") and isnumeric(interfaceid)>0 and dci.rejectionid not in  (select distinct RejectionID from MachineWiseRejectionMaster where MachineID = @machine) ORDER BY RejectionID", Conn);
                cmd.Parameters.AddWithValue("@machine", machine);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    HMIEntity ent = new HMIEntity();
                    ent.Slno = Convert.ToInt16(rdr["row"]);
                    ent.MachineID = rdr["MachineId"].ToString();
                    ent.InterfaceID = rdr["InterfaceId"].ToString();
                    ent.RejectionID = rdr["rejectionid"].ToString();
                    ent.Description = rdr["rejectiondescription"].ToString();
                    ent.RejectionCategory = rdr["Catagory"].ToString();
                    Downlst.Add(ent);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return Downlst;
        }
        internal static List<HMIEntity> GetAvailableCustomerDetails(string machineID)
        {
            List<HMIEntity> list = new List<HMIEntity>();
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select ROW_NUMBER() OVER(ORDER BY c.CustomerID) AS row,  c.customerid, c.customername from customerinformation c
where c.customerid not in (select distinct CustomerID from MachineWiseCustomerMaster where MachineID=@machine)", Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machine", machineID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    HMIEntity ent = new HMIEntity();
                    ent.Slno = Convert.ToInt16(rdr["row"]);
                    ent.CustomerId = rdr["customerid"].ToString();
                    ent.CustomerName = rdr["customername"].ToString();
                    list.Add(ent);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return list;
        }

        internal static List<HMIEntity> GetCategoryInfo(string machine)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<HMIEntity> Catlst = new List<HMIEntity>();
            try
            {
                //SqlCommand cmd = new SqlCommand(@"select ROW_NUMBER() OVER(ORDER BY DownCategory) AS row, '' as MachineId,
                //                                dci.DownCategory,dci.Description from dbo.DownCategoryInformation dci", Conn);
                SqlCommand cmd = new SqlCommand(@"select ROW_NUMBER() OVER(ORDER BY DownCategory) AS row, '' as MachineId,
                                                dci.DownCategory,dci.Description from dbo.DownCategoryInformation dci
												where dci.DownCategory not in (select distinct category from MachineWiseCategoryMaster where MachineID=@machine)", Conn);
                cmd.Parameters.AddWithValue("@machine", machine);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    HMIEntity ent = new HMIEntity();
                    ent.Slno = Convert.ToInt16(rdr["row"]);
                    ent.MachineID = rdr["MachineId"].ToString();
                    ent.DownCategory = rdr["DownCategory"].ToString();
                    ent.Description = rdr["Description"].ToString();
                    Catlst.Add(ent);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return Catlst;
        }
        internal static List<HMIEntity> GetRejectionCategoryInfo(string machine)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<HMIEntity> Catlst = new List<HMIEntity>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"SELECT distinct DENSE_RANK() OVER (ORDER BY Catagory) row,Catagory
FROM rejectioncodeinformation where Catagory not in (select distinct Category from MachineWiseRejectionCategoryMaster where MachineID=@machine)", Conn);
                cmd.Parameters.AddWithValue("@machine", machine);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    HMIEntity ent = new HMIEntity();
                    ent.Slno = Convert.ToInt16(rdr["row"]);
                    ent.MachineID = "";
                    ent.RejectionCategory = rdr["Catagory"].ToString();
                    Catlst.Add(ent);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return Catlst;
        }


        internal static void inserttoDownmaster(string downcat, string downID, string downdesc, string downInterfaceID, string machID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            bool status = false;
            try
            {
                string sqlQuery = @"if not exists(select * from MachineWiseDownMaster where DownID=@DownID and MachineID=@machineID and DownCategory=@Downcategory)
                                    Begin
	                                    insert into MachineWiseDownMaster(MachineID,DownCategory,DownID,DownInterfaceID,DownDescription)values(@machineID,@Downcategory,@DownID,@DownInterfaceID,@DownDescription)
                                    End";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machineID", machID);
                cmd.Parameters.AddWithValue("@Downcategory", downcat);
                cmd.Parameters.AddWithValue("@DownID", downID);
                cmd.Parameters.AddWithValue("@DownInterfaceID", downInterfaceID);
                cmd.Parameters.AddWithValue("@DownDescription", downdesc);
                rdr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
        }
        internal static void inserttoRejectionCodeMaster(string category, string rejectionID, string description, string rejInterfaceID, string machID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            bool status = false;
            try
            {
                string sqlQuery = @"if not exists(select * from MachineWiseRejectionMaster where RejectionID=@RejectionID and MachineID=@MachineID and RejectionCategory=@RejectionCategory)
Begin
	insert into MachineWiseRejectionMaster(MachineID,RejectionCategory,RejectionID,RejectionInterfaceID,RejectionDescription)
	values(@MachineID,@RejectionCategory,@RejectionID,@RejectionInterfaceID,@RejectionDescription)
 End";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machID);
                cmd.Parameters.AddWithValue("@RejectionCategory", category);
                cmd.Parameters.AddWithValue("@RejectionID", rejectionID);
                cmd.Parameters.AddWithValue("@RejectionInterfaceID", rejInterfaceID);
                cmd.Parameters.AddWithValue("@RejectionDescription", description);
                rdr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
        }
        internal static List<HMIEntity> GetMachineWiseDownMaster(string machineID, string downcat)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<HMIEntity> Machwiselst = new List<HMIEntity>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select ROW_NUMBER() OVER(ORDER BY DownID) AS row,* from MachineWiseDownMaster where MachineId=@machineID and DownCategory in (" + downcat + ")", Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machineID", machineID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    HMIEntity ent = new HMIEntity();
                    ent.Slno = Convert.ToInt16(rdr["row"]);
                    ent.MachineID = rdr["MachineId"].ToString();
                    ent.InterfaceID = rdr["DownInterfaceId"].ToString();
                    ent.DownID = rdr["DownID"].ToString();
                    ent.Description = rdr["downdescription"].ToString();
                    ent.DownCategory = rdr["DownCategory"].ToString();
                    Machwiselst.Add(ent);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return Machwiselst;
        }
        internal static List<HMIEntity> GetMachineWiseRejectionCodeMaster(string machineID, string rejectionCat)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<HMIEntity> Machwiselst = new List<HMIEntity>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select ROW_NUMBER() OVER(ORDER BY RejectionID) AS row,* from MachineWiseRejectionMaster where MachineID=@machineID and RejectionCategory in (" + rejectionCat + ")", Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machineID", machineID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    HMIEntity ent = new HMIEntity();
                    ent.Slno = Convert.ToInt16(rdr["row"]);
                    ent.MachineID = rdr["MachineID"].ToString();
                    ent.InterfaceID = rdr["RejectionInterfaceID"].ToString();
                    ent.RejectionID = rdr["RejectionID"].ToString();
                    ent.Description = rdr["RejectionDescription"].ToString();
                    ent.RejectionCategory = rdr["RejectionCategory"].ToString();
                    Machwiselst.Add(ent);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return Machwiselst;
        }

        internal static int getComponentCount(string type, string machine)
        {
            int countlst = 0;
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                string query = "";
                if (type.Equals("Component", StringComparison.OrdinalIgnoreCase))
                {
                    query = @"select count(*)as RowCounts from MachineWiseComponentMaster where MachineID=@machineid";
                }
                else if (type.Equals("DownCode", StringComparison.OrdinalIgnoreCase))
                {
                    query = @"select count(*)as RowCounts from MachineWiseDownMaster  where MachineID=@machineid";
                }
                else if (type.Equals("Category", StringComparison.OrdinalIgnoreCase))
                {
                    query = @"select count(*)as RowCounts from MachineWiseCategoryMaster  where MachineID=@machineid";
                }
                else if (type.Equals("Customer", StringComparison.OrdinalIgnoreCase))
                {
                    query = @"select count(*)as RowCounts from MachineWiseCustomerMaster  where MachineID=@machineid";
                }
                else if (type.Equals("RejectionCategory", StringComparison.OrdinalIgnoreCase))
                {
                    query = @"select count(*)as RowCounts from MachineWiseRejectionCategoryMaster  where MachineID=@machineid";
                }
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machineid", machine);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    countlst = Convert.ToInt32(rdr["RowCounts"]);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return countlst;
        }
        internal static List<ListItem> getAssignedDownCodeCount(string machine, string category,string type)
        {
            List<ListItem> list = new List<ListItem>();
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                string query = "";
                if(type== "RejectionCode")
                {
                    query = @"SELECT RejectionCategory as category, COUNT(RejectionCategory) as CatCount 
FROM MachineWiseRejectionMaster where RejectionCategory in (" + category + ") and MachineID=@machineid GROUP BY RejectionCategory;";
                }
                else
                {
                    query = @"SELECT DownCategory  as category, COUNT(DownCategory) as CatCount 
FROM MachineWiseDownMaster where DownCategory in (" + category + ") and MachineID=@machineid GROUP BY DownCategory;";
                }
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machineid", machine);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ListItem item = new ListItem();
                    item.Text = rdr["category"].ToString();
                    item.Value = rdr["CatCount"].ToString();
                    list.Add(item);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return list;
        }
        internal static void inserttoComponentMaster(string machineID, string componentID, string interfaceID, string desc)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            bool status = false;
            try
            {
                string sqlQuery = @"if not exists(select * from MachineWiseComponentMaster where Component=@Component and MachineID=@machineID)
                                    Begin
	                                    insert into MachineWiseComponentMaster(MachineID,Component,CompInterfaceID,ComponentDescription)values(@machineID,@Component,@CompInterfaceID,@CompDesc)
                                    End";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machineID", machineID);
                cmd.Parameters.AddWithValue("@Component", componentID);
                cmd.Parameters.AddWithValue("@CompInterfaceID", interfaceID);
                cmd.Parameters.AddWithValue("@CompDesc", desc);
                rdr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static void inserttoCustomerMaster(string machineID, string customerID, string customerName)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            bool status = false;
            try
            {
                string sqlQuery = @"if not exists(select * from MachineWiseCustomerMaster where CustomerID=@customerID and MachineID=@machineID)
Begin
	 insert into MachineWiseCustomerMaster(MachineID,CustomerID,CustomerName)values(@machineID,@customerID,@customerName)
End";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machineID", machineID);
                cmd.Parameters.AddWithValue("@customerID", customerID);
                cmd.Parameters.AddWithValue("@customerName", customerName);
                rdr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static List<HMIEntity> GetMachineWiseComponentMaster(string machineID)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<HMIEntity> ComponentMasterlst = new List<HMIEntity>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select ROW_NUMBER() OVER(ORDER BY MachineId,Component,CompInterfaceID) AS row,* from MachineWiseComponentMaster where MachineId=@machineID", Conn);
                cmd.Parameters.AddWithValue("@machineID", machineID);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    HMIEntity ent = new HMIEntity();
                    ent.Slno = Convert.ToInt16(rdr["row"]);
                    ent.MachineID = rdr["MachineId"].ToString();
                    ent.InterfaceID = rdr["CompInterfaceID"].ToString();
                    ent.ComponentID = rdr["Component"].ToString();
                    ent.Description = rdr["ComponentDescription"].ToString();
                    ComponentMasterlst.Add(ent);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return ComponentMasterlst;
        }
        internal static List<HMIEntity> GetMachineWiseCustomerMaster(string machineID)
        {
            List<HMIEntity> list = new List<HMIEntity>();
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select ROW_NUMBER() OVER(ORDER BY MachineId,CustomerID,CustomerName) AS row,* from MachineWiseCustomerMaster where MachineId=@machineID", Conn);
                cmd.Parameters.AddWithValue("@machineID", machineID);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    HMIEntity ent = new HMIEntity();
                    ent.Slno = Convert.ToInt16(rdr["row"]);
                    ent.MachineID = rdr["MachineId"].ToString();
                    ent.CustomerId = rdr["CustomerID"].ToString();
                    ent.CustomerName = rdr["CustomerName"].ToString();
                    list.Add(ent);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return list;
        }
        internal static void DeleteDownInformation(string machineID, string downID, string downcat, out string sucessfailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            sucessfailure = string.Empty;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("delete from MachineWiseDownMaster where DownID=@DownID and MachineID=@machineID and DownCategory=@Downcategory", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@machineID", machineID);
                cmd.Parameters.AddWithValue("@DownID", downID);
                cmd.Parameters.AddWithValue("@Downcategory", downcat);
                cmd.ExecuteNonQuery();
                sucessfailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessfailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static void DeleteRejectionCodeInformation(string machineID, string rejID, string rejCat, out string sucessfailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            sucessfailure = string.Empty;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("delete from MachineWiseRejectionMaster where RejectionID=@RejectionID and MachineID=@machineID and RejectionCategory=@RejectionCategory", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@machineID", machineID);
                cmd.Parameters.AddWithValue("@RejectionID", rejID);
                cmd.Parameters.AddWithValue("@RejectionCategory", rejCat);
                cmd.ExecuteNonQuery();
                sucessfailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessfailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static void DeleteCatagoryInformation(string machineID, string catagory, out string sucessfailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            sucessfailure = string.Empty;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("delete from MachineWiseCategoryMaster where MachineID=@machineID and Category=@category", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@machineID", machineID);
                cmd.Parameters.AddWithValue("@category", catagory);
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
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static void DeleteRejectionCatagoryInformation(string machineID, string catagory, out string sucessfailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            sucessfailure = string.Empty;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("delete from [dbo].[MachineWiseRejectionCategoryMaster] where MachineID=@machineID and Category=@category", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@machineID", machineID);
                cmd.Parameters.AddWithValue("@category", catagory);
                cmd.ExecuteNonQuery();
                sucessfailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessfailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static void DeleteComponentInformation(string machineID, string component, out string sucessfailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            sucessfailure = string.Empty;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("delete from MachineWiseComponentMaster where MachineID=@machineID and Component=@component", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@machineID", machineID);
                cmd.Parameters.AddWithValue("@component", component);
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
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static void DeleteCustomerInformation(string machineID, string customerID, out string sucessfailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            sucessfailure = string.Empty;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("delete from MachineWiseCustomerMaster where MachineID=@machineID and CustomerID=@CustomerID", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@machineID", machineID);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
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
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static void inserttoCategoryMaster(string catagory, string machineID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            bool status = false;
            try
            {
                string sqlQuery = @"if not exists(select * from MachineWiseCategoryMaster where Category=@category and MachineID=@machineID)
                                    Begin
	                                    insert into MachineWiseCategoryMaster(MachineID,Category)values(@machineID,@category)
                                    End";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machineID", machineID);
                cmd.Parameters.AddWithValue("@category", catagory);
                rdr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static void insertToRejectionCategoryMaster(string catagory, string machineID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            bool status = false;
            try
            {
                string sqlQuery = @"if not exists(select * from MachineWiseRejectionCategoryMaster where Category=@category and MachineID=@machineID)
Begin
	insert into MachineWiseRejectionCategoryMaster(MachineID,Category)values(@machineID,@category)
 End";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machineID", machineID);
                cmd.Parameters.AddWithValue("@category", catagory);
                rdr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static List<HMIEntity> GetMachineWiseCategoryMaster(string machineID)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<HMIEntity> CategoryMasterlst = new List<HMIEntity>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select ROW_NUMBER() OVER(ORDER BY MachineId,Category,CategoryInterfaceID) AS row,* from MachineWiseCategoryMaster where MachineId=@machineID", Conn);
                cmd.Parameters.AddWithValue("@machineID", machineID);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    HMIEntity ent = new HMIEntity();
                    ent.Slno = Convert.ToInt16(rdr["row"]);
                    ent.MachineID = rdr["MachineId"].ToString();
                    ent.InterfaceID = rdr["CategoryInterfaceID"].ToString();
                    ent.DownCategory = rdr["Category"].ToString();
                    CategoryMasterlst.Add(ent);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return CategoryMasterlst;
        }
        internal static List<HMIEntity> GetMachineWiseRejectionCategoryMaster(string machineID)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<HMIEntity> CategoryMasterlst = new List<HMIEntity>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select ROW_NUMBER() OVER(ORDER BY MachineId,Category,CategoryInterfaceID) AS row,* from MachineWiseRejectionCategoryMaster where MachineID=@machineID", Conn);
                cmd.Parameters.AddWithValue("@machineID", machineID);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    HMIEntity ent = new HMIEntity();
                    ent.Slno = Convert.ToInt16(rdr["row"]);
                    ent.MachineID = rdr["MachineID"].ToString();
                    ent.InterfaceID = rdr["CategoryInterfaceID"].ToString();
                    ent.RejectionCategory = rdr["Category"].ToString();
                    CategoryMasterlst.Add(ent);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return CategoryMasterlst;
        }
        #endregion

        #region ----- Scrap Entry Screen ----
        internal static DataTable getScrapEntryDetails(string machineID, string date, string shift)
        {
            DataTable dt = new DataTable();
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"[dbo].[S_GetScrapEntryScreen_AmarRaj]", Conn);
                cmd.Parameters.AddWithValue("@Machine", machineID);
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(date).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shift", shift);
                cmd.Parameters.AddWithValue("@param", "View");
                cmd.CommandType = CommandType.StoredProcedure;
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return dt;
        }
        internal static int insertScrapEntryDetails(ScrapEntryData data)
        {
            int result = 0;
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"[dbo].[S_GetScrapEntryScreen_AmarRaj]", Conn);
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(data.Date).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shift", data.Shift);
                cmd.Parameters.AddWithValue("@LayOutScrap", data.LayoutScrap);
                cmd.Parameters.AddWithValue("@DesignScrap", data.DesignScrap);
                cmd.Parameters.AddWithValue("@TotalScrap", data.TotalScrap);
                cmd.Parameters.AddWithValue("@param", "Save");
                cmd.CommandType = CommandType.StoredProcedure;
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return result;
        }
        #endregion
    }
}