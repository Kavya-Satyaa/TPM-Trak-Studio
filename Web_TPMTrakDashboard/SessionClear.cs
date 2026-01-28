using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard
{
    public class SessionClear
    {
        public static void ClearSession()
        {
            HttpContext.Current.Session["EfficiencyPeChart"] = null;
            HttpContext.Current.Session["EfficiencyAeChart"] = null;
            HttpContext.Current.Session["EfficiencyOEEChart"] = null;
            HttpContext.Current.Session["proddatasource"] = null;
            HttpContext.Current.Session["downdatasource"] = null;
            HttpContext.Current.Session["HeaderProdGrid"] = null;
            HttpContext.Current.Session["HeaderDownGrid"] = null;
            HttpContext.Current.Session["HeaderProdDownGrid"] = null;
        }
    }
}