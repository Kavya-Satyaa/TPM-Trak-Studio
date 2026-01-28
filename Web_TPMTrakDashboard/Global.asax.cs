using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Web_TPMTrakDashboard;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterOpenAuth();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //JobScheduler.Start();       
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
            // Dil ayarları cookie'den okunuyor.

            //string language = "en-us";

            ////Detect User's Language.
            //if (Request.UserLanguages != null)
            //{
            //    //Set the Language.
            //    language = Request.UserLanguages[0];
            //}

            //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(language);

            //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);


        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            string culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["Language"] != null)
            {
                culture = Convert.ToString(HttpContext.Current.Session["Language"]);
            }
            else
            {
                if (Request.UserLanguages != null)
                {
                    culture = Request.UserLanguages[0];
                }
            }
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo(culture);
            //cultureInfo.DateTimeFormat.ShortDatePattern = "dd-MMM-yy";
            //cultureInfo.DateTimeFormat.DateSeparator = "-";
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
    }
}
