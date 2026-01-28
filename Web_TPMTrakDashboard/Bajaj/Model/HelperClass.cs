using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.Bajaj.Model
{
    public class HelperClass
    {
        public static void openWarningToastrModal(Page page, string msg)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "openToastrWarning", "toasterWarningMsg('" + msg + ".','');", true);
        }
        public static void openModal(Page page, string modalid, bool isAnimationRequired)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "openModal", "openModal('" + modalid + "', '" + isAnimationRequired + "');", true);
        }
        public static void openAddEditModal(Page page, string modalid)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "openAddEditModal", "openAddEditModals('" + modalid + "');", true);
        }
        public static void openInsertErrorModal(Page page)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "error", "openErrorModal_1('Failed to insert record.');", true);
        }
        public static void openInsertSuccessModal(Page page)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "successMsg", "showSuccessMsg('Record saved Successfully.','');", true);
        }
        public static void openSuccessModal(Page page)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "successMsg", "showSuccessMsg('Approved Successfully.','');", true);
        }

        public static void openUpdateSuccessModal(Page page)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "successMsg", "showSuccessMsg('Record updated Successfully.','');", true);
        }
        public static void clearModal(Page page)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "confirmModal", "clearAllModalScreen();", true);
        }
        public static void openWarningModal(Page page, string msg)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "openWarning", "openWarningModal_1('" + msg + ".');", true);
        }
        public static void openErrorModal(Page page, string msg)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "openWarning", "openErrorModal_1('" + msg + ".');", true);
        }
        public static void ShowHideActionColumn(ListView listView, bool isVisible)
        {
            try
            {
                if (listView.Items.Count > 0)
                {
                    if (isVisible)
                    {
                        listView.FindControl("thAction").Visible = true;
                    }
                    else
                    {
                        listView.FindControl("thAction").Visible = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ShowHideActionColumn: " + ex.Message);
            }
        }
        public static void openDeleteSuccessModal(Page page)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "successMsg", "showSuccessMsg('Record deleted Successfully.','');", true);
        }
        public static void openDeleteErrorModal(Page page)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "error", "openErrorModal_1('Failed to delete record.');", true);
        }
        public static void openFunction(Page page, string funationame)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), funationame, funationame + "();", true);
        }
    }
}