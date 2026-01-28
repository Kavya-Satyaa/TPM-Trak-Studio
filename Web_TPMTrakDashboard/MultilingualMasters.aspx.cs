using Elmah;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class MultilingualMasters : System.Web.UI.Page
    {
        public List<ColumnViewSetting> settings = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            settings = DataBaseAccess.BindSettingPage("ComponentInformation", Session["Language"] == null ? "en" : Session["Language"].ToString());
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["timeFormat"] = DownCodeInfoDataBase.GetShopdefaultsTimeFormat();

                string interfaceDataType = DataBaseAccess.GetType("ComponentInfoInterfaceIdDataType");
                hfInterfaceDataType.Value = String.IsNullOrEmpty(interfaceDataType) ? "Numeric" : interfaceDataType;

                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                else
                    SessionClear.ClearSession();

                BindDownCodeGrid();
                BindComponentInformation();
                ddloption_SelectedIndexChanged(null, null);
            }
        }

        #region --- Down Time Codes Grid ---
        private void BindDownCodeGrid()
        {
            List<DownCodesModel> lstDownCodes = new List<DownCodesModel>();
            try
            {
                string timeFormat = string.Empty;
                if (HttpContext.Current.Session["timeFormat"] == null)
                    timeFormat = DownCodeInfoDataBase.GetShopdefaultsTimeFormat();
                else
                    timeFormat = HttpContext.Current.Session["timeFormat"].ToString();

                lstDownCodes = DownCodeInfoDataBase.GetAllDownCodeInfoPE("", "", "", "Searchdownid", timeFormat);
                if (lstDownCodes == null)
                {
                    lstDownCodes = new List<DownCodesModel>();
                    DownCodesModel data = new DownCodesModel();
                    lstDownCodes.Add(data);
                }

                lvDownCodesInfo.DataSource = lstDownCodes;
                lvDownCodesInfo.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindDownCodeGrid: {ex.Message}");
            }
        }
        protected void btnUpdateDownCodeInfo_Click(object sender, EventArgs e)
        {
            string msg = "";
            try
            {
                foreach (ListViewDataItem item in lvDownCodesInfo.Items)
                {
                    if ((item.FindControl("hdfInterface") as HiddenField).Value.Equals("update", StringComparison.OrdinalIgnoreCase))
                    {
                        string DownID = (item.FindControl("lblDownTime") as Label).Text.Trim();
                        string InterfaceID = (item.FindControl("lblInterfaceid") as Label).Text.Trim();
                        string DownCategory = (item.FindControl("lblCategory") as Label).Text.Trim();
                        string Description = (item.FindControl("txtDescription") as TextBox).Text.Trim();
                        string DescriptionInHindi = (item.FindControl("DownDescriptionInHindi") as TextBox).Text.Trim();

                        DownCodeInfoDataBase.InsertUpdateDownCodeInformationPE(DownID, InterfaceID, Description, DescriptionInHindi, DownCategory, out msg);
                    }
                }
                if (!string.IsNullOrEmpty(msg))
                {
                    msg = HttpContext.GetGlobalResourceObject("CommanResource", "Recordsupdatedsuccessfully").ToString();
                    HelperClassGeneric.openSuccessModal(this, msg);
                    BindDownCodeGrid();
                }
                else
                    HelperClassGeneric.openWarningModal(this, "save Failed. Try Again!");
                
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        #endregion

        #region --- Component Information Grid ---
        private void BindComponentInformation()
        {
            try
            {
                DataTable dt = DataBaseAccess.GetComponentDetailsPE(txtComponentSearch.Text.Trim());
                dt.Columns.Add("InterfaceIDInt", typeof(int));
                if (dt.Rows.Count > 0)
                {
                    lvComponentInfo.DataSource = dt;
                    lvComponentInfo.DataBind();

                    if (dt.Rows.Count <= 500)
                        lvComponentInfo.FindControl("DataPager1").Visible = false;
                }
                else
                {
                    lvComponentInfo.DataSource = null;
                    lvComponentInfo.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void lvComponentInfo_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            try
            {
                DataPager dp = (DataPager)lvComponentInfo.FindControl("DataPager1");

                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                BindComponentInformation();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void BtnView_Click(object sender, EventArgs e)
        {
            BindComponentInformation();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string msg = "";
            try
            {
                foreach (ListViewDataItem item in lvComponentInfo.Items)
                {
                    if (item.ItemType == ListViewItemType.DataItem)
                    {
                        if ((item.FindControl("hdfInterface") as HiddenField).Value.ToString().Equals("update", StringComparison.OrdinalIgnoreCase))
                        {
                            string componetid = (item.FindControl("lblComp") as Label).Text.Trim();
                            string interfaceid = (item.FindControl("lblInterfaceID") as Label).Text.Trim();
                            string customer = (item.FindControl("lblCustomer") as Label).Text.ToString();
                            string description = (item.FindControl("txtDescription") as TextBox).Text.Trim();
                            string descInHindi = (item.FindControl("txtDescriptionInHindi") as TextBox).Text.Trim();
                            bool isSucessFailure = false;
                            DataBaseAccess.InsertOrUpdateComponentIdDetailsPE(componetid, interfaceid, customer, description, descInHindi, out isSucessFailure);

                            if (isSucessFailure)
                            {
                                msg = "Records updated successfully.";
                                msg = HttpContext.GetLocalResourceObject("~/ComponentInformation.aspx", "Recordsupdatedsuccessfully").ToString();
                            }
                            else
                            {
                                msg = HttpContext.GetLocalResourceObject("~/ComponentInformation.aspx", "Pleaseenterrequiredvaluestoupdate").ToString();
                            }
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenSuccessToastr", "successMsg('Records Saved Succefully.');", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                HelperClass.openWarningModal(this, ex.Message.ToString());
            }
            finally
            {
                BindComponentInformation();
            }
        }

        protected void txtComponentSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                BindComponentInformation();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        #endregion

        #region --- Rejection & Rework ---
        private void BindRejectionGrid()
        {
            try
            {
                DataTable dt = DataBaseAccess.RejectionReason();

                lvRejection.DataSource = dt;
                lvRejection.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"RejectionGrid: {ex.Message}");
            }
        }
        private void BindReworkInformation()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DataBaseAccess.ReworkReason();
                lvRework.DataSource = dt;
                lvRework.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"ReworkGrid: {ex.Message}");
            }
        }

        #region "Save Update Rejection Info"
        protected void btnsaveRejection_Click(object sender, EventArgs e)
        {
            string successfailure = "";
            try
            {
                if (ddloption.SelectedValue.Equals("Rejection", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (ListViewDataItem item in lvRejection.Items)
                    {
                        if ((item.FindControl("hdfInterfaceID") as HiddenField).Value.Equals("Update", StringComparison.OrdinalIgnoreCase))
                        {
                            string ID = (item.FindControl("lblRejectionReason") as Label).Text.Trim();
                            string InterfaceID = (item.FindControl("lblRejectioncategory") as Label).Text.Trim();
                            string Category = (item.FindControl("lblRejectioncategory") as Label).Text.Trim();
                            string Descripttion = (item.FindControl("txtDescription") as TextBox).Text.Trim();
                            string DescriptionInHindi = (item.FindControl("txtDescriptionInHindi") as TextBox).Text.Trim();

                            successfailure = Checkinsertupdate(successfailure, ID, Descripttion, DescriptionInHindi, Category, InterfaceID, "");
                        }
                    }
                }
                else if (ddloption.SelectedValue.Equals("Rework", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (ListViewDataItem item in lvRework.Items)
                    {
                        if ((item.FindControl("hdfInterfaceID") as HiddenField).Value.Equals("Update", StringComparison.OrdinalIgnoreCase))
                        {
                            string ID = (item.FindControl("lblReworkReson") as Label).Text.Trim();
                            string InterfaceID = (item.FindControl("lblReworkInterfaceID") as Label).Text.Trim();
                            string Category = (item.FindControl("lblReworkCategory") as Label).Text.Trim();
                            string Descripttion = (item.FindControl("txtDescription") as TextBox).Text.Trim();
                            string DescriptionInHindi = (item.FindControl("txtDescriptionInHindi") as TextBox).Text.Trim();

                            successfailure = Reworkcheckinsertupdate(successfailure, ID, Descripttion, DescriptionInHindi, Category, InterfaceID);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(successfailure))
                {
                    HelperClassGeneric.openSuccessModal(this, "Successfully Saved.");
                    BindRejectionGrid();
                }
                else
                    HelperClassGeneric.openErrorModal(this, "Save Failed! Try Again.");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private static string Reworkcheckinsertupdate(string sucessfailure, string Rew_reason, string Rew_desc, string Rew_DescInHindi, string Rew_cat, string Rew_interface)
        {
            bool isValidEntry = DataBaseAccess.CheckReworkinterfaceid(Rew_interface, Rew_reason);
            if (!isValidEntry)
            {
                bool presentReworkId = DataBaseAccess.CheckpresentReworkId(Rew_reason);
                if (presentReworkId)
                {
                    DataBaseAccess.UpdateDataForReworkPE(Rew_cat, Rew_desc, Rew_DescInHindi, Rew_reason, Rew_interface, out sucessfailure);
                }
                else
                {
                    DataBaseAccess.insertDataForReworkPE(Rew_reason, Rew_desc, Rew_DescInHindi, Rew_cat, Rew_interface, out sucessfailure);
                }
            }
            return sucessfailure;
        }

        private static string Checkinsertupdate(string sucessfailure, string Rej_reason, string Rej_desc, string DescriptionInHindi, string Rej_cat, string Rej_interface, string subcategory)
        {
            bool isValidEntry = DataBaseAccess.Checkinterfaceid(Rej_interface, Rej_reason);
            if (!isValidEntry)
            {
                bool checkRejectionId = DataBaseAccess.CheckRejectionId(Rej_reason);
                if (checkRejectionId)
                {
                    DataBaseAccess.UpdateDataPE(Rej_cat, Rej_reason, Rej_desc, DescriptionInHindi, Rej_interface, subcategory, out sucessfailure);
                }
                else
                {
                    DataBaseAccess.insertDataPE(Rej_cat, Rej_desc, DescriptionInHindi, Rej_reason, Rej_interface, subcategory, out sucessfailure);
                }
            }
            return sucessfailure;
        }
        #endregion

        #endregion

        protected void ddloption_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddloption.SelectedValue.Equals("Rejection", StringComparison.OrdinalIgnoreCase))
                {
                    lvRejection.Visible = true;
                    lvRework.Visible = false;
                    BindRejectionGrid();
                }
                else if(ddloption.SelectedValue.Equals("Rework", StringComparison.OrdinalIgnoreCase))
                {
                    lvRejection.Visible = false;
                    lvRework.Visible = true;
                    BindReworkInformation();
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}