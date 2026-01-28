using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class RejectionCodes : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private static string Reworkcheckinsertupdate(string sucessfailure, string Rew_reason, string Rew_desc, string Rew_cat, string Rew_interface)
        {
            bool isValidEntry = DataBaseAccess.CheckReworkinterfaceid(Rew_interface, Rew_reason);
            if (!isValidEntry)
            {
                bool presentReworkId = DataBaseAccess.CheckpresentReworkId(Rew_reason);
                if (presentReworkId)
                {
                    DataBaseAccess.UpdateDataForRework(Rew_cat, Rew_desc, Rew_reason, Rew_interface, out sucessfailure); //change by Pawan
                }
                else
                {
                    DataBaseAccess.insertDataForRework(Rew_reason, Rew_desc, Rew_cat, Rew_interface, out sucessfailure);
                }
            }
            return sucessfailure;
        }

        private static string Checkinsertupdate(string sucessfailure, string Rej_reason, string Rej_desc, string Rej_cat, string Rej_interface, string subcategory)
        {
            bool isValidEntry = DataBaseAccess.Checkinterfaceid(Rej_interface, Rej_reason);
            if (!isValidEntry)
            {
                bool checkRejectionId = DataBaseAccess.CheckRejectionId(Rej_reason);
                if (checkRejectionId)
                {
                    DataBaseAccess.UpdateData(Rej_cat, Rej_reason, Rej_desc, Rej_interface, subcategory, out sucessfailure);
                }
                else
                {
                    DataBaseAccess.insertData(Rej_cat, Rej_desc, Rej_reason, Rej_interface, subcategory, out sucessfailure);
                }
            }
            return sucessfailure;
        }

        #region "Binding Rejection Information"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<RejectionAndReworkinModel> RejectionInfo()
        {
            List<string> listrejCatagory = DataBaseAccess.databindforCatagory();
            List<RejectionAndReworkinModel> lisCatSubCat = DataBaseAccess.getCatagorySubCatgoryList();
            HttpContext.Current.Session["CatSubCategoryList"] = lisCatSubCat;
            DataTable dt = DataBaseAccess.RejectionReason();
            List<RejectionAndReworkinModel> componentgrddata = new List<RejectionAndReworkinModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                RejectionAndReworkinModel component = new RejectionAndReworkinModel();
                component.Rejectionid = dt.Rows[i]["rejectionid"].ToString(); ;
                component.Interfaceid = dt.Rows[i]["interfaceid"].ToString();
                component.Catagory = dt.Rows[i]["Catagory"].ToString();
                component.Description = dt.Rows[i]["rejectiondescription"].ToString();
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["GlobePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    component.SubCatagory = dt.Rows[i]["SubCategory"].ToString();
                    if (component.Catagory != "")
                    {
                        component.SubCatgoryList = lisCatSubCat.Where(k => k.Catagory == component.Catagory).Select(k => k.SubCatagory).ToList();
                    }
                }
                componentgrddata.Add(component);
            }
            foreach (var item in componentgrddata)
            {
                item.ListCategory = listrejCatagory;
            }
            return componentgrddata;
        }
        #endregion

        #region "Binding Rework Information"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<RejectionAndReworkinModel> ReworkInformation()
        {
            List<string> listrejCatagory = DataBaseAccess.databindforCatagoryForRework();
            DataTable dt = DataBaseAccess.ReworkReason();
            List<RejectionAndReworkinModel> componentgrddata = new List<RejectionAndReworkinModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                RejectionAndReworkinModel component = new RejectionAndReworkinModel();
                component.Reworkid = dt.Rows[i]["Reworkid"].ToString(); ;
                component.Interfaceid = dt.Rows[i]["Reworkinterfaceid"].ToString();
                component.Catagory = dt.Rows[i]["ReworkCatagory"].ToString();
                component.Description = dt.Rows[i]["Reworkdescription"].ToString();
                componentgrddata.Add(component);
            }
            foreach (var item in componentgrddata)
            {
                item.ListCategory = listrejCatagory;
            }
            return componentgrddata;
        }
        #endregion

        #region "Save Update Rejection Info"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveUpdateRejectionInfo(RejectionAndReworkinModel model)
        {
            string msg = "";
            bool isSuccessFailure = false;
            try
            {
                string sucessfailure = string.Empty;
                foreach (var item in model.ListRejectionRework)
                {
                    sucessfailure = Checkinsertupdate(sucessfailure, item.Rejectionid, item.Description, item.Catagory, item.Interfaceid, item.SubCatagory);
                }
                if (isSuccessFailure == true)
                {
                    msg = "Records save/update successfully";
                }
                else if (isSuccessFailure == false)
                {
                    msg = "Records save/update successfully";
                }
                // Componentinfo();

            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            return msg;

        }
        #endregion

        #region "Save Update Rework Info"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveUpdateReworkInfo(RejectionAndReworkinModel model)
        {
            string msg = "";
            bool isSuccessFailure = false;
            try
            {
                string sucessfailure = string.Empty;
                foreach (var item in model.ListRejectionRework)
                {
                    sucessfailure = Reworkcheckinsertupdate(sucessfailure, item.Reworkid, item.Description, item.Catagory, item.Interfaceid);
                }
                if (isSuccessFailure == true)
                {
                    msg = "Records save/update successfully";
                }
                else if (isSuccessFailure == false)
                {
                    msg = "Records save/update successfully";
                }
                // Componentinfo();

            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            return msg;

        }
        #endregion

        #region "Delete Rejection Info"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DeleteRejectionInfo(string deleteId)
        {
            string msg = "";
            try
            {
                string sucessfailure = string.Empty;
                bool isValidEntry = DataBaseAccess.CheckRejectionId(deleteId);
                if (isValidEntry)
                {
                    DataBaseAccess.DeleteDataForRejectionReason(deleteId, out sucessfailure);
                    if (sucessfailure.Equals("Successfull"))
                    {
                        msg = "Data Deleted Succesfully !!";
                    }
                }
                else
                {
                    msg = "No Record  Deleted !!";
                }
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            return msg;

        }
        #endregion

        #region "Delete Rework Info"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DeleteReworkInfo(string deleteId)
        {
            string msg = "";
            try
            {
                string sucessfailure = string.Empty;
                bool isValidEntry = DataBaseAccess.CheckpresentReworkId(deleteId);
                if (isValidEntry)
                {
                    DataBaseAccess.DeleteDataForReworkReason(deleteId, out sucessfailure);
                    if (sucessfailure.Equals("Successfull"))
                    {
                        msg = "Data Deleted Succesfully !!";
                    }
                }
                else
                {
                    msg = "No Record  Deleted !!";
                }
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            return msg;

        }
        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string success = "";
            try
            {
                if(ddloption.SelectedValue.Equals("Rejection", StringComparison.OrdinalIgnoreCase))
                 success = TMPTrakGenerateReport.GenerateRejectionCodesReport();
                else if(ddloption.SelectedValue.Equals("Rework", StringComparison.OrdinalIgnoreCase))
                    success = TMPTrakGenerateReport.GenerateReworkCodesReport();
                try
                {
                    if (success.Equals("Template Not Found", StringComparison.OrdinalIgnoreCase))
                    {
                        HelperClassGeneric.openWarningToastrModal(this, "Template not found");
                    }
                    else if (success.Equals("Data Not Found", StringComparison.OrdinalIgnoreCase))
                    {
                        HelperClassGeneric.openWarningToastrModal(this, "Data not found");
                    }
                    else
                    {
                        HelperClassGeneric.openErrorModal(this, "Erro Generating Report! Try Again.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool isGEAPageEnabled()
        {
            bool isGEAEnabled = false;
            try
            {
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["GlobePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    isGEAEnabled = true;
                }
            }
            catch (Exception ex)
            {
                isGEAEnabled = false;
            }
            return isGEAEnabled;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> getSubCategoryListForCat(string category)
        {
            List<string> list = new List<string>();
            try
            {
                List<RejectionAndReworkinModel> catSubCatList = new List<RejectionAndReworkinModel>();
                if (HttpContext.Current.Session["CatSubCategoryList"] == null)
                {
                    catSubCatList = DataBaseAccess.getCatagorySubCatgoryList();
                }
                catSubCatList = HttpContext.Current.Session["CatSubCategoryList"] as List<RejectionAndReworkinModel>;
                list = catSubCatList.Where(k => k.Catagory == category).Select(k => k.SubCatagory).ToList();
            }
            catch (Exception ex)
            {
            }
            return list;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> getCategoryList()
        {
            List<string> list = new List<string>();
            try
            {
                List<RejectionAndReworkinModel> catSubCatList = new List<RejectionAndReworkinModel>();
                if (HttpContext.Current.Session["CatSubCategoryList"] == null)
                {
                    catSubCatList = DataBaseAccess.getCatagorySubCatgoryList();
                }
                catSubCatList = HttpContext.Current.Session["CatSubCategoryList"] as List<RejectionAndReworkinModel>;
                list = catSubCatList.Select(k => k.Catagory).Distinct().ToList();
            }
            catch (Exception ex)
            {
            }
            return list;
        }
    }
}















#region
//if (selectedoption == "Rejection Reason")
//            {
//                string sucessfailure = string.Empty;



//bool isValidEntry = DataBaseAccess.Checkinterfaceid();
//                    if (!isValidEntry) //|| presentrejectionId)
//                    {
//                        bool checkRejectionId = DataBaseAccess.CheckRejectionId();
//                        if (checkRejectionId)
//                        {
//                            DataBaseAccess.UpdateData(CmbCatagory.Text, cmbReworkRejectionCode.Text, txtDescription.Text, txtInterfaceId.Text, out sucessfailure);
//                            if (sucessfailure.Equals("Successfull"))
//                            {
//                                //CustomDialogBox Cdb = new CustomDialogBox("Information Message", " Data updated Successfully !!"); //Change by Pawan
//                                //Cdb.Show();
//                            }
//                            //DatagridDataLoad();
//                        }
//                        else
//                        {
//                            DataBaseAccess.insertData(CmbCatagory.Text, txtDescription.Text, cmbReworkRejectionCode.Text, txtInterfaceId.Text, out sucessfailure);
//                            if (sucessfailure.Equals("Successfull"))
//                            {
//                                //CustomDialogBox Cdb = new CustomDialogBox("Information Message", " Data Inserted Successfully !!");
//                                //Cdb.Show();
//                            }
//                            //DatagridDataLoad();
//                        }
//                    }
//                    else
//                    {
//                        //CustomDialogBox Cdb = new CustomDialogBox("Error Message", "This Interfaceid Already Exist For Another Rejection Code");
//                        //Cdb.Show();
//                        return;
//                    }
//                }

#endregion