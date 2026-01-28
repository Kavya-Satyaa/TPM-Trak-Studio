using BusinessClassLibrary;
using Elmah;
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
    public partial class PPMGraph : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                if (Request.QueryString["SelectedValue"] != "")
                {
                    Session["SelectedValue"] = Request.QueryString["SelectedValue"].ToString();
                    Session["Accepted"] = Request.QueryString["Accepted"].ToString();
                    Session["Rejected"] = Request.QueryString["Rejected"].ToString();
                }
            }
        }

        
    }

}