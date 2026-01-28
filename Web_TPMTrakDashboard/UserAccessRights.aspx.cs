using BusinessClassLibrary;
using ModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard
{
    public partial class UserAccessRights : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);

            if (!IsPostBack)
            {
                if (ConfigurationManager.AppSettings["UserAccessEshopx"].ToString().Equals("0"))
                {
                    divEshopx.Visible = false;
                }
                else
                {
                    divEshopx.Visible = true;
                }
                if (ConfigurationManager.AppSettings["UserAccessEshopxHelpReq"].ToString().Equals("0"))
                {
                    diveshopxHelpreq.Visible = false;
                }
                else
                {
                    diveshopxHelpreq.Visible = true;
                }
                BindPlantId();
                BindUserData();
                if (ConfigurationManager.AppSettings["UserAccessEshopx"].ToString().Equals("0"))
                {
                    divEshopx.Visible = false;
                }
                else
                {
                    divEshopx.Visible = true;
                }
                if (ConfigurationManager.AppSettings["UserAccessEshopxHelpReq"].ToString().Equals("0"))
                {
                    diveshopxHelpreq.Visible = false;
                }
                else
                {
                    diveshopxHelpreq.Visible = true;
                }
            }
        }

        #region "Bind Plant Id"
        private void BindPlantId()
        {
            try
            {
                List<string> lstPlantData = BindCockpitView.ViewPlantToDisplay();
                ddlPlantID.DataSource = lstPlantData;
                ddlPlantID.DataBind();
            }
            catch (Exception ex)
            {
                // ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind User Id"
        private void BindUserData()
        {
            try
            {
                List<string> lstPlantData = BindCockpitView.GetAllUserData();
                ddlUserId.DataSource = lstPlantData;
                ddlUserId.DataBind();
                ddlUserId.SelectedValue = Session["UserName"] != null ? Session["UserName"].ToString().ToUpper() : "PCT";
                GetPasswordAndAdminInfo();
            }
            catch (Exception ex)
            {
                // ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Password and Admin value"
        private void GetPasswordAndAdminInfo()
        {
            UserAccessModel Employee = BindCockpitView.GetEmployeeDetails(ddlUserId.SelectedValue.ToString());
            if (Employee != null)
            {
                txtPassword.Text = Employee.Password;
                txtPassword.Attributes["value"] = txtPassword.Text;
                chkAdmin.Checked = Employee.Admin;
            }
        }
        #endregion

        #region "Bind Dashboard Details"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<UserAccessModel> BindUserAccessData(string userID)
        {
            return BindCockpitView.bindListUserAccess(userID); 
        }
        #endregion

        #region "Get Password Info Data"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static UserAccessModel GetPasswordInfo(string userID)
        {
            return BindCockpitView.GetEmployeeDetails(userID);
        }
        #endregion

        #region "Bind Dashboard Details"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveUserAccessData(UserAccessModel model)
        {
            HttpContext.Current.Session["UserAccessData"] = null;
            string successFailure, SuccessFailure = string.Empty;
            BindCockpitView.DeleteDataUserAccessRights(model.UserID,"", out successFailure);
            foreach (var item in model.ListUserDataInfo)
            {
                BindCockpitView.InsertDataUserAccessRights(item.Domain, item.Code, model.UserID, out successFailure);
            }
            if (model.Password != "")
                BindCockpitView.InsertAsadmin(model.UserID, model.Admin, model.Password);
            return successFailure;
        }
        #endregion
    }
}