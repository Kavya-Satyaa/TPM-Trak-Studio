using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class OEQEPEColor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static void SaveData(string PEGreen, string PERed, string AEGreen, string AERed, string OEGreen, string OERed, string QEGreen, string QERed, string ViewType,string OperatorPEGreen,string OperatorPERed)
        {
            try
            {
                if (!(string.IsNullOrEmpty(PEGreen) && string.IsNullOrEmpty(AEGreen) && string.IsNullOrEmpty(QEGreen) && string.IsNullOrEmpty(OEGreen) && string.IsNullOrEmpty(PERed) && string.IsNullOrEmpty(AERed) && string.IsNullOrEmpty(OERed) && string.IsNullOrEmpty(QERed) && string.IsNullOrEmpty(OperatorPEGreen) && string.IsNullOrEmpty(OperatorPERed)))
                {
                    DataBaseAccess.SaveColorData(PEGreen, PERed, AEGreen, AERed, OEGreen, OERed, QEGreen, QERed, ViewType,OperatorPEGreen,OperatorPERed);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Colorvalues GetData(string ViewType)
        {
            Colorvalues values = new Colorvalues();
            try
            {
                values = DataBaseAccess.GetColors(ViewType);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return values;
        }
    }

    public class Colorvalues
    {
        public string PEGreen { get; set; }
        public string PERed { get; set; }
        public string AEGreen { get; set; }
        public string AERed { get; set; }
        public string OEGreen { get; set; }
        public string OERed { get; set; }
        public string QEGreen { get; set; }
        public string QERed { get; set; }
        public string OperatorPEGreen { get; set; }
        public string OperatorPERed { get; set; }

    }

}