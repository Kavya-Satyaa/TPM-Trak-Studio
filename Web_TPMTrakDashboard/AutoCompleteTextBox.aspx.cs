using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard
{
    public partial class AutoCompleteTextBox : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static List<Employees> getEmployees(string EmpName)
        {
            List<Employees> empObj = new List<Employees>();
            string cs = ConfigurationManager.ConnectionStrings[0].ToString();
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    using (SqlCommand com = new SqlCommand())
                    {
                        com.CommandText = string.Format("select EmpID,EmpName,EmpSalary from Employe where EmpName like '{0}%'", EmpName);
                        com.Connection = con;
                        con.Open();
                        SqlDataReader sdr = com.ExecuteReader();
                        Employees emp = null;
                        while (sdr.Read())
                        {
                            emp = new Employees();
                            emp.EmpDbKey = Convert.ToInt32(sdr["EmpID"]);
                            emp.EmpName = Convert.ToString(sdr["EmpName"]);
                            emp.Address = Convert.ToDouble(sdr["EmpSalary"]);
                            empObj.Add(emp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error {0}", ex.Message);
            }
            return empObj;
        }

        [Serializable]
        public class Employees
        {
            public int EmpDbKey { get; set; }
            public string EmpName { get; set; }
            public double Address { get; set; }
        }
    }
}