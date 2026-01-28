using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.ShantiIron.Model;

namespace Web_TPMTrakDashboard.ShantiIron
{
    public partial class QualityGate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindComponentID();
            }
        }
        private void BindComponentID()
        {
            try
            {
                DataTable CmpList = DataBaseAccess.GetAllComponentz();
                ddlCompId.DataSource = CmpList.AsEnumerable().Select(x => x.Field<string>("ComponentId")).ToList();
                ddlCompId.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static QualityGateSlnoStatusData getSlnoStatus(string slno,string partNum)
        {
            QualityGateSlnoStatusData data = new QualityGateSlnoStatusData();
            try
            {

                data= ShantiDataBaseAccess.getQualityGateSlnoStatus(slno, partNum);
            }
            catch(Exception ex)
            {

            }
            return data;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<MeasurementDetails> getSerialNumberOperationData(string slno,string partnum)
        {
            List<MeasurementDetails> list = new List<MeasurementDetails>();
            try
            {
                list = ShantiDataBaseAccess.getSerialNumberOperationDetails(slno,partnum);
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<MeasurementDetails> getSlnoManualInpsectionDetails(string slno,string partnum)
        {
            List<MeasurementDetails> list = new List<MeasurementDetails>();
            try
            {
                list = ShantiDataBaseAccess.getSlnoManualInpsectionDetails(slno, partnum);
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string saveManualInpectionData(string slno,string componentid,string characteristicID, string value,string machineid,string operation,string characteristiccode,string selectedlsl,string selectedusl,string selectedunit,string mean,string remarks,string dataType)
        {
            string result = "";
            try
            {
                string userID = "";
                if (HttpContext.Current.Session["UserName"] != null)
                {
                    userID = HttpContext.Current.Session["UserName"].ToString();
                }
                result = ShantiDataBaseAccess.saveManualInpectionDetails(slno,componentid, characteristicID, value, userID,machineid, operation, characteristiccode, selectedlsl, selectedusl, selectedunit, mean, remarks, dataType);
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<string> getScannedSlnoFromTbl()
        {
            List<string> list = new List<string>();
            try
            {
                string partnumber;
                string slno = ShantiDataBaseAccess.getScannedSlnoFromTable("QualityGate", out partnumber);
                list.Add(slno);
                list.Add(partnumber);
            }
            catch(Exception ex)
            {

            }
            return list;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static int setSlnoStatus(string slno)
        {
            int result = ShantiDataBaseAccess.updateSlnoStatus("QualityGate", slno);
            return result;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string saveProcessStartEndData(string slno,string partno,string param,string processStartTime)
        {
            string result = "";
            try
            {
                string userID = "";
                if (HttpContext.Current.Session["UserName"] != null)
                {
                    userID = HttpContext.Current.Session["UserName"].ToString();
                }
                result = ShantiDataBaseAccess.saveQualityGateProcessStartEndDetails(slno, partno, param, userID, processStartTime);
            }
            catch(Exception ex)
            {

            }
            return result;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string saveProcessStatusDetails(string slno, string partnum, string starttime, string processStatus)
        {
            string result = "";
            try
            {
                result = ShantiDataBaseAccess.saveQualityGateProcessStatusDetails(slno, partnum, starttime, processStatus);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string getSlnoCMMFileData(string slno, string partnum,string fileName)
        {
            string path = "";
            try
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath("QualityGateCMMFiles")))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("QualityGateCMMFiles"));
                }
                if (fileName != "")
                {
                    //if (Directory.Exists(HttpContext.Current.Server.MapPath("QualityGateCMMFiles/" + fileName)))
                    //{
                    path = "QualityGateCMMFiles/" + fileName;
                   // }
                }
                else
                {
                    QualityGateSlnoStatusData data = ShantiDataBaseAccess.getQualityGateSlnoStatus(slno, partnum);
                    if (data.CMMFileName != "")
                    {
                        fileName = data.CMMFileName;
                        //if (Directory.Exists(HttpContext.Current.Server.MapPath("QualityGateCMMFiles/" + fileName)))
                        //{
                        path = "QualityGateCMMFiles/" + fileName;
                        // }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return path;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static bool isPartNumberAndSlnoExistsInSPCAutoData(string slno, string partnum)
        {
            bool result = false;
            try
            {
                result = ShantiDataBaseAccess.isPartNumberAndSlnoExistsInSPCAutoTbl(slno, partnum);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}