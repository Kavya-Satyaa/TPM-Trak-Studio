using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public static class CachedData
    {
        private static readonly Dictionary<string, DataTable> CockpitTableViewData = new Dictionary<string, DataTable>();
        public static void RefreshData()
        {

            var _fromDate = CockpitDataBaseAccess.GetLogicalDay(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            var _toDate = CockpitDataBaseAccess.GetLogicalDayEnd(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            var key = _fromDate + _toDate;
            var dtValue = CockpitDataBaseAccess.GetTabelCockpitDetails("[s_GetCockpitData_WithTempTable_eshopx]", _fromDate, _toDate, "", "","","","");
            if (CockpitTableViewData.ContainsKey(key)) CockpitTableViewData[key] = dtValue;
            else
            {
                CockpitTableViewData.Clear();
                CockpitTableViewData.Add(key, dtValue);
            }
        }

        public static DataTable getChachedData(string fromDate, string toDate)
        {
            DataTable dt = null;
            var key = fromDate + toDate;
            if (CockpitTableViewData.ContainsKey(key))
                return CockpitTableViewData[key];
            return dt;

        }

    }
}