using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.KTASpindle;

namespace Web_TPMTrakDashboard
{
    public partial class EshopxSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            try
            {
                string Path = DBAccess.GetRootPath("RootPath");
                txtPathVal.Text = Path;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int success = 0;
                success = DBAccess.SaveRootFolderDetails(txtPathVal.Text, "RootPath");
                if(success>0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Saved Successfully.')", true);
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
    }
}