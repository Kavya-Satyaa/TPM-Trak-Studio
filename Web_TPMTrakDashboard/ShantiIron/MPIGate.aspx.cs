using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.ShantiIron.Model;

namespace Web_TPMTrakDashboard.ShantiIron
{
    public partial class MPIGate : System.Web.UI.Page
    {
        private static string CompSlnoValidationResult = "";
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string serialNumberValidation(string PartNumber, string RevNumber, string PartName, string SupplierCode, string SerialNumber, string HeatCode)
        {
            string tempString = "";
            try
            {
                tempString = ShantiDataBaseAccess.insertMPIGateSlnoDetails(PartNumber, RevNumber, PartName, SupplierCode, SerialNumber, HeatCode, HttpContext.Current.Session["UserName"] == null ? "" : HttpContext.Current.Session["UserName"].ToString(), "", "", "insert");
                if (tempString == "insertionError")
                {
                    Logger.WriteDebugLog("MPI: Insertion Failed");
                    return tempString;
                }
                HttpContext.Current.Session["MPIInsertedTime"] = tempString;
                string interfaceId = "", opnID = "";
                DataTable dtMachineInfo = ShantiDataBaseAccess.getMPIMachineDetails();
                foreach (DataRow row in dtMachineInfo.Rows)
                {
                    interfaceId = row["InterfaceID"].ToString();
                    opnID = row["OpnID"].ToString();
                }
                string STDATE = DateTime.Now.ToString("yyyyMMdd");
                string STTIME = DateTime.Now.ToString("HHmmss");

                //START-28-21-[3539598-DX00]-20-N112F-20200606-103850-END
                string DataString = string.Format("START-28-{0}-[{1}-{2}]-{3}-{4}-{5}-{6}-END", interfaceId, PartNumber, RevNumber, opnID.ToString(), SerialNumber, STDATE, STTIME);
                Logger.WriteDebugLog(string.Format("MPI: Req Validation string : {0}", DataString));
                tempString = ShantiDataBaseAccess.GetStatus28Type(DataString.Trim());
                Logger.WriteDebugLog("MPI: Req Validation Result" + tempString);
                CompSlnoValidationResult = "2";
                if (!string.IsNullOrEmpty(tempString))
                {
                    CompSlnoValidationResult = getValidationResult(tempString.ToString(), out tempString);
                }
                ShantiDataBaseAccess.insertMPIGateSlnoDetails(PartNumber, RevNumber, PartName, SupplierCode, SerialNumber, HeatCode, HttpContext.Current.Session["UserName"] == null ? "" : HttpContext.Current.Session["UserName"].ToString(), HttpContext.Current.Session["MPIInsertedTime"].ToString(), CompSlnoValidationResult, "updateCompValidation");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return tempString;
        }
        private static string getValidationResult(string validationResult, out string actualResult)
        {
            string result = "0";
            actualResult = "0";
            // '<' + @SlNo + '-' + @component + '@' + @PrevOpnFromAutodata + '#' + '4>' --(Rejection)
            //split the string and get the result
            if (validationResult.Contains("#"))
            {
                var strs = validationResult.Split(new char[] { '<', '>', '@', '#' }, StringSplitOptions.RemoveEmptyEntries);
                result = strs[strs.Length - 1].Trim();
            }
            actualResult = result;
            return result == "1" ? "1" : "2";
        }



        static NetworkConnection nc = null;
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string sendCSVFile(string PartNumber, string RevNumber, string PartName, string SupplierCode, string SerialNumber, string HeatCode)
        {
            string result = "";
            try
            {
                string userame = "";
                string password = "";
                string reqFolderPath = "";
                string operationList = "";
                userame = System.Web.Configuration.WebConfigurationManager.AppSettings["MPIGateUsername"].ToString();
                password = System.Web.Configuration.WebConfigurationManager.AppSettings["MPIGatePassword"].ToString();
                reqFolderPath = System.Web.Configuration.WebConfigurationManager.AppSettings["MPIGatePath"].ToString();
                if (!string.IsNullOrEmpty(userame) && !string.IsNullOrEmpty(password))
                {
                    try
                    {
                        nc = new NetworkConnection(reqFolderPath, new NetworkCredential(userame, password));

                        StringBuilder sb = new StringBuilder();
                        // string value = PartNumber + ", " + SerialNumber + ", " + CompSlnoValidationResult;
                        string value = PartNumber + "-" + RevNumber + "-" + PartName + "-" + SupplierCode + "-" + SerialNumber + "-" + HeatCode;
                        if (value != "")
                        {

                            string fileName = "ACK_MPI_" + PartNumber + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
                            sb.Append(value);
                            StreamWriter file = new StreamWriter(reqFolderPath + fileName);
                            file.WriteLine(sb.ToString());
                            file.Close();
                            Logger.WriteDebugLog("MPI CSV File generated successfully");
                        }
                    }
                    catch (Exception exx)
                    {
                        result = "fail";
                        //networkConnection = false;
                        if (nc != null)
                            nc.Dispose();
                        Logger.WriteErrorLog(exx.ToString());
                    }
                    finally
                    {
                        if (nc != null)
                            nc.Dispose();
                    }
                }
                else
                {
                    try
                    {
                        // result = "enterusername";

                        StringBuilder sb = new StringBuilder();
                        // string value = PartNumber + ", " + SerialNumber + ", " + CompSlnoValidationResult;
                        string value = PartNumber + "-" + RevNumber + "-" + PartName + "-" + SupplierCode + "-" + SerialNumber + "-" + HeatCode;
                        if (value != "")
                        {

                            string fileName = "ACK_MPI_" + PartNumber + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
                            sb.Append(value);
                            StreamWriter file = new StreamWriter(reqFolderPath + fileName);
                            file.WriteLine(sb.ToString());
                            file.Close();
                            Logger.WriteDebugLog("MPI CSV File generated successfully");
                        }
                    }
                    catch (Exception exx)
                    {
                        result = "fail";
                        Logger.WriteErrorLog(exx);
                    }
                }
                string ackFileGenerationResult = "2";
                if (result == "")
                {
                    ackFileGenerationResult = "1";
                }
                ShantiDataBaseAccess.insertMPIGateSlnoDetails(PartNumber, RevNumber, PartName, SupplierCode, SerialNumber, HeatCode, HttpContext.Current.Session["UserName"] == null ? "" : HttpContext.Current.Session["UserName"].ToString(), HttpContext.Current.Session["MPIInsertedTime"].ToString(), ackFileGenerationResult, "updateACK");
            }
            catch (Exception ex)
            {
                result = "fail";
                Logger.WriteErrorLog(ex.ToString());
            }
            return result;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<string> getComponent(string SerialNumber)
        {
            List<string> listComp = new List<string>();
            try
            {
                List<MPIGateData> list = new List<MPIGateData>();
                list = ShantiDataBaseAccess.getMPISlnoDetails(SerialNumber);
                HttpContext.Current.Session["MPIGateSlnoData"] = list;
                list = HttpContext.Current.Session["MPIGateSlnoData"] as List<MPIGateData>;
                listComp = list.Select(k => k.Component).Distinct().ToList();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return listComp;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<string> getHeatCode(string SerialNumber, string Component)
        {
            List<string> listHeat = new List<string>();
            try
            {
                List<MPIGateData> list = new List<MPIGateData>();
                if (HttpContext.Current.Session["MPIGateSlnoData"] == null)
                {
                    list = ShantiDataBaseAccess.getMPISlnoDetails(SerialNumber);
                    HttpContext.Current.Session["MPIGateSlnoData"] = list;
                }
                list = HttpContext.Current.Session["MPIGateSlnoData"] as List<MPIGateData>;
                listHeat = list.Where(k => k.Component == Component).Select(k => k.HeatCode).Distinct().ToList();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return listHeat;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static MPIGateData getSlnoDetails(string SerialNumber, string Component, string HeatCode)
        {
            MPIGateData data = new MPIGateData();
            try
            {

                List<MPIGateData> list = new List<MPIGateData>();
                if (HttpContext.Current.Session["MPIGateSlnoData"] == null)
                {
                    list = ShantiDataBaseAccess.getMPISlnoDetails(SerialNumber);
                    HttpContext.Current.Session["MPIGateSlnoData"] = list;
                }
                list = HttpContext.Current.Session["MPIGateSlnoData"] as List<MPIGateData>;
                data = list.Where(k => k.SerialNumber == SerialNumber && k.Component == Component && k.HeatCode == HeatCode).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return data;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static bool PartSlnoExistenceValidation(string PartNumber, string RevNumber, string PartName, string SupplierCode, string SerialNumber, string HeatCode)
        {
            bool isExist = false;
            try
            {
                isExist = ShantiDataBaseAccess.isMPIGateSlnoExist(PartNumber, RevNumber, PartName, SupplierCode, SerialNumber, HeatCode);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return isExist;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<MPIGateData> getAllGeneratedSlnoDetails()
        {
            List<MPIGateData> list = new List<MPIGateData>();
            try
            {
                list = ShantiDataBaseAccess.getAllMPIGeneratedSlnoDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return list;
        }
    }
}