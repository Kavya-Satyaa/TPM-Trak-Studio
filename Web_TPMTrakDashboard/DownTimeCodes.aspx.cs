using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class DownTimeCodes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpContext.Current.Session["catagory"] = null;
                BindCategory();
                Session["timeFormat"] = DownCodeInfoDataBase.GetShopdefaultsTimeFormat();
                if (WebConfigurationManager.AppSettings["SKSPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase)){
                    btnDownLoss.Visible = true;
                }
                else
                {
                    btnDownLoss.Visible = false;
                }
            }
        }

        #region "Bind Category"
        private void BindCategory()
        {
            try
            {
                List<string> catagorylst = DownCodeInfoDataBase.GetDownCategoryInformation();
                Session["catagory"] = catagorylst;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }
        #endregion

        #region "Get Top Down Time Info"
        [WebMethod]
        public static List<DownCodesModel> DownCodesInfo()
        {
            List<DownCodesModel> lstDownCodes = new List<DownCodesModel>();
            try
            {
                string timeFormat = string.Empty;
                if (HttpContext.Current.Session["timeFormat"] == null)
                    timeFormat = DownCodeInfoDataBase.GetShopdefaultsTimeFormat();
                else
                    timeFormat = HttpContext.Current.Session["timeFormat"].ToString();
                lstDownCodes = DownCodeInfoDataBase.GetAllDownCodeInfo("", "", "", "Searchdownid", timeFormat);
                List<string> catagorylst = null;
                if (HttpContext.Current.Session["catagory"] == null)

                    catagorylst = DownCodeInfoDataBase.GetDownCategoryInformation();
                else
                    catagorylst = HttpContext.Current.Session["catagory"] as List<string>;

                List<string> employeeID = DataBaseAccess.GetEmployeesWithName();
                if (lstDownCodes != null)
                {
                    foreach (var item in lstDownCodes)
                    {
                        item.ListCatagory = catagorylst;
                        item.ListEmployeeID = employeeID;
                    }
                }
                if (lstDownCodes == null)
                {
                    lstDownCodes = new List<DownCodesModel>();
                    DownCodesModel data = new DownCodesModel();
                    data.ListCatagory = catagorylst;
                    data.ListEmployeeID = employeeID;
                    lstDownCodes.Add(data);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return lstDownCodes;
        }
        #endregion

        #region "Save Top Down Time Info"
        [WebMethod]
        public static string SaveDownTimeInfo(DownCodesModel model)
        {
            string msg = null;
            try
            {
                string timeFormat = string.Empty;
                if (HttpContext.Current.Session["timeFormat"] == null)
                    timeFormat = DownCodeInfoDataBase.GetShopdefaultsTimeFormat();
                else
                    timeFormat = HttpContext.Current.Session["timeFormat"].ToString();
                if (timeFormat == "ss")//timeFormat
                {
                    DownCodeInfoDataBase.InsertUpdateDownCodeInformation(model.downid, model.interfaceid, model.downdescription,
                        model.Catagory, model.Availeffy, false, model.Threshold, model.prodeffy, model.ThresholdfrmCO, model.Owner, "Insert", out msg,model.IgnoreForRuntimeTarget);
                }
                else
                {
                    DownCodeInfoDataBase.InsertUpdateDownCodeInformation(model.downid, model.interfaceid, model.downdescription,
                        model.Catagory, true, true, model.Threshold * 60.00, model.prodeffy, model.ThresholdfrmCO, model.Owner, "Insert", out msg,model.IgnoreForRuntimeTarget);
                }
                msg = HttpContext.GetGlobalResourceObject("CommanResource", "Recordsupdatedsuccessfully").ToString();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
            return msg;
        }
        #endregion

        #region "Update Top Down Time Info"
        [WebMethod]
        public static string UpdateDownTimeInfo(DownCodesModel model)
        {
            string msg = null;
            try
            {
                string timeFormat = string.Empty;
                if (HttpContext.Current.Session["timeFormat"] == null)
                    timeFormat = DownCodeInfoDataBase.GetShopdefaultsTimeFormat();
                else
                    timeFormat = HttpContext.Current.Session["timeFormat"].ToString();
                foreach (var item in model.ListDownCode)
                {
                    item.ThresholdfrmCO = item.Availeffy == true ? item.ThresholdfrmCO : false;
                    if (timeFormat == "ss")//timeFormat
                    {
                        DownCodeInfoDataBase.InsertUpdateDownCodeInformation(item.downid, item.interfaceid, item.downdescription,
                            item.Catagory, item.Availeffy, false, item.Threshold, item.prodeffy, item.ThresholdfrmCO, item.Owner, "Insert", out msg,item.IgnoreForRuntimeTarget);
                    }
                    else
                    {
                        DownCodeInfoDataBase.InsertUpdateDownCodeInformation(item.downid, item.interfaceid, item.downdescription,
                            item.Catagory, item.Availeffy, false, item.Threshold * 60.00, item.prodeffy, item.ThresholdfrmCO, item.Owner, "Insert", out msg,item.IgnoreForRuntimeTarget);
                    }
                }
                msg = HttpContext.GetGlobalResourceObject("CommanResource", "Recordsupdatedsuccessfully").ToString();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
            return msg;
        }
        #endregion

        #region "Delete Top Down Time Info"
        [WebMethod]
        public static string getDownIDDeleteStatus(string downcode, string interfaceid)
        {
            string result = "";
            try
            {
                result = DownCodeInfoDataBase.deleteDownCodeDetails(downcode, interfaceid);
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
            return result;

        }
        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string success = TMPTrakGenerateReport.GenerateDowntimeCodesReport();

                if (success.Equals("Template Not Found", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openWarningModal(this, "Template not found");
                else if (success.Equals("Data Not Found", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openWarningToastrModal(this, "Data not found");
                else 
                    HelperClassGeneric.openWarningModal(this, "ERROR! Generating Report. TRY AGAIN!");

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

        }
    }
}