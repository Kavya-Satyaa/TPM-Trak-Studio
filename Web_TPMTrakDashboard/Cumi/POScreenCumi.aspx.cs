using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Cumi.Model;

namespace Web_TPMTrakDashboard.Cumi
{
    public partial class POScreenCumi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindData();
            }
        }
        private void BindData()
        {
            try
            {
                List<CumiPOScreen> list = CumiDBAccess.getPOScreenData(txtFromDate.Text,txtToDate.Text,txtComponentID.Text,txtPO.Text);
                lvPOScreen.DataSource = list;
                lvPOScreen.DataBind();
                Session["POScreenData"] = list;
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }
    }
}