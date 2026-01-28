using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.Nippon
{
    public partial class Demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region YEAR DATA
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static YearDataEntity GetYearDATA()
        {
            YearDataEntity Entity = new YearDataEntity();
            try
            {
                Entity.Data = new List<double> { 45.12, 99.65, 98.96, 50.54, 80.22 };
                Entity.Catergory = new List<string> { "2021", "2020", "2019", "2018", "2017" };
                Entity.Title = " Year wise OEE";
                Entity.XaxisLabel = "OEE";
                Entity.PointerLabel = " 'OEE : <b>{point.y:.1f} %</b>'";

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return Entity;
        }
        #endregion

        #region Month Data
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static MonthDataEntity GetMonthDATA(string Year)
        {
            MonthDataEntity MonthDataEntity = new MonthDataEntity();
            try
            {
                MonthDataEntity.Year = Year;
                MonthDataEntity.Title = " Month wise OEE For " + Year ;
                MonthDataEntity.XaxisLabel = "OEE";
                MonthDataEntity.PointerLabel = " 'OEE : <b>{point.y:.1f} %</b>'";
                MonthDataEntity.Catergory = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec" };
                switch (Year)
                {
                    case "2017":
                        MonthDataEntity.Data = new List<double> { 45.12, 99.65, 98.96, 50.54, 80.22, 45.12, 99.65, 98.96, 50.54, 80.22, 11.22, 22.46 };
                        break;
                    case "2018":
                        MonthDataEntity.Data = new List<double> { 55.12, 89.65, 88.96, 60.54, 70.22, 55.12, 89.65, 88.96, 60.54, 70.22, 21.22, 32.46 };
                        break;
                    case "2019":
                        MonthDataEntity.Data = new List<double> { 32, 99.65, 98.96, 50.54, 80.22, 45.12, 99.65, 98.96, 50.54, 80.22, 11.22, 22.46 };
                        break;
                    case "2020":
                        MonthDataEntity.Data = new List<double> { 45.12, 9.65, 8.96, 0.54, 0.22, 5.12, 9.65, 8.96, 0.54, 0.22, 51.22, 22.46 };
                        break;
                    case "2021":
                        MonthDataEntity.Data = new List<double> { 21, 88, 88.96, 40.54, 70.22, 45.12, 99.65, 98.96, 50.54, 80.22, 11.22, 22.46 };
                        break;

                }


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return MonthDataEntity;
        }
        #endregion

        #region DayData
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static DateDataEntity GetDayDATA(string Month, string Year)
        {
            DateDataEntity DayDataEntity = new DateDataEntity();
            try
            {
                DayDataEntity.Year = Year;
                DayDataEntity.Month = Month;
                DayDataEntity.Title = " Day wise OEE For " + Month + "-" + Year;
                DayDataEntity.XaxisLabel = "OEE";
                DayDataEntity.PointerLabel = " 'OEE : <b>{point.y:.1f} %</b>'";
                DayDataEntity.Catergory = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
                DayDataEntity.Data = new List<double> { 45.12, 99.65, 98.96, 50.54, 80.22, 45.12, 99.65, 98.96, 50.54, 80.22, 11.22, 22.46, 45.12, 99.65, 98.96, 50.54, 80.22, 45.12, 99.65, 98.96, 50.54, 80.22, 11.22, 22.46, 45.12, 99.65, 98.96, 50.54, 80.22, 45.12 };
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return DayDataEntity;
        }
        #endregion

        #region OEEDAta
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static EfficiencyData GetOEEDATA(string DateType, string Asset)
        {
            EfficiencyData OEEDataEntity = new EfficiencyData();
            try
            {
                OEEDataEntity.Type = "OEE";
                OEEDataEntity.DateType = DateType;
                OEEDataEntity.Title = "OEE" + DateType ;
                OEEDataEntity.XaxisLabel = "OEE";
                OEEDataEntity.PointerLabel = " 'OEE : <b>{point.y:.1f} %</b>'";
                switch(DateType)
                {
                    case "Year":
                        OEEDataEntity.Catergory = new List<string> { "2021", "2020", "2019", "2018", "2017" };
                        OEEDataEntity.Data = new List<double> { 45.12, 99.65, 98.96, 50.54, 80.22};
                        break;
                    case "Month":
                        OEEDataEntity.Catergory = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec" };
                        OEEDataEntity.Data = new List<double> { 45.12, 99.65, 98.96, 50.54, 80.22, 45.12, 99.65, 98.96, 50.54, 80.22, 11.22, 22.46 };
                        break;
                    case "Day":
                        OEEDataEntity.Catergory = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
                        OEEDataEntity.Data = new List<double> { 45.12, 99.65, 98.96, 50.54, 80.22, 45.12, 99.65, 98.96, 50.54, 80.22, 11.22, 22.46, 45.12, 99.65, 98.96, 50.54, 80.22, 45.12, 99.65, 98.96, 50.54, 80.22, 11.22, 22.46, 45.12, 99.65, 98.96, 50.54, 80.22, 45.12 };
                        break;
                    case "Week":
                        break;
                }
               
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return OEEDataEntity;
        }
        #endregion

        #region AEDAta
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static EfficiencyData GetAEDATA(string DateType, string Asset)
        {
            EfficiencyData OEEDataEntity = new EfficiencyData();
            try
            {
                OEEDataEntity.Type = "AE";
                OEEDataEntity.DateType = DateType;
                OEEDataEntity.Title = "AE For " + DateType;
                OEEDataEntity.XaxisLabel = "AE";
                OEEDataEntity.PointerLabel = " 'AE : <b>{point.y:.1f} %</b>'";
                switch (DateType)
                {
                    case "Year":
                        OEEDataEntity.Catergory = new List<string> { "2021", "2020", "2019", "2018", "2017" };
                        OEEDataEntity.Data = new List<double> { 45.12, 99.65, 98.96, 50.54, 80.22 };
                        break;
                    case "Month":
                        OEEDataEntity.Catergory = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec" };
                        OEEDataEntity.Data = new List<double> { 45.12, 99.65, 98.96, 50.54, 80.22, 45.12, 99.65, 98.96, 50.54, 80.22, 11.22, 22.46 };
                        break;
                    case "Day":
                        OEEDataEntity.Catergory = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
                        OEEDataEntity.Data = new List<double> { 45.12, 99.65, 98.96, 50.54, 80.22, 45.12, 99.65, 98.96, 50.54, 80.22, 11.22, 22.46, 45.12, 99.65, 98.96, 50.54, 80.22, 45.12, 99.65, 98.96, 50.54, 80.22, 11.22, 22.46, 45.12, 99.65, 98.96, 50.54, 80.22, 45.12 };
                        break;
                    case "Week":
                        break;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return OEEDataEntity;
        }
        #endregion

        #region PEDAta
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static EfficiencyData GetPEDATA(string DateType, string Asset)
        {
            EfficiencyData OEEDataEntity = new EfficiencyData();
            try
            {
                OEEDataEntity.Type = "PE";
                OEEDataEntity.DateType = DateType;
                OEEDataEntity.Title = " PE For " + DateType;
                OEEDataEntity.XaxisLabel = "PE";
                OEEDataEntity.PointerLabel = " 'PE : <b>{point.y:.1f} %</b>'";
                switch (DateType)
                {
                    case "Year":
                        OEEDataEntity.Catergory = new List<string> { "2021", "2020", "2019", "2018", "2017" };
                        OEEDataEntity.Data = new List<double> { 45.12, 99.65, 98.96, 50.54, 80.22 };
                        break;
                    case "Month":
                        OEEDataEntity.Catergory = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec" };
                        OEEDataEntity.Data = new List<double> { 45.12, 99.65, 98.96, 50.54, 80.22, 45.12, 99.65, 98.96, 50.54, 80.22, 11.22, 22.46 };
                        break;
                    case "Day":
                        OEEDataEntity.Catergory = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
                        OEEDataEntity.Data = new List<double> { 45.12, 99.65, 98.96, 50.54, 80.22, 45.12, 99.65, 98.96, 50.54, 80.22, 11.22, 22.46, 45.12, 99.65, 98.96, 50.54, 80.22, 45.12, 99.65, 98.96, 50.54, 80.22, 11.22, 22.46, 45.12, 99.65, 98.96, 50.54, 80.22, 45.12 };
                        break;
                    case "Week":
                        break;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return OEEDataEntity;
        }
        #endregion

        #region QEDAta
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static EfficiencyData GetQEDATA(string DateType, string Asset)
        {
            EfficiencyData OEEDataEntity = new EfficiencyData();
            try
            {
                OEEDataEntity.Type = "QE";
                OEEDataEntity.DateType = DateType;
                OEEDataEntity.Title = "QE For " + DateType;
                OEEDataEntity.XaxisLabel = "QE";
                OEEDataEntity.PointerLabel = " 'QE : <b>{point.y:.1f} %</b>'";
                switch (DateType)
                {
                    case "Year":
                        OEEDataEntity.Catergory = new List<string> { "2021", "2020", "2019", "2018", "2017" };
                        OEEDataEntity.Data = new List<double> { 45.12, 99.65, 98.96, 50.54, 80.22 };
                        break;
                    case "Month":
                        OEEDataEntity.Catergory = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec" };
                        OEEDataEntity.Data = new List<double> { 45.12, 99.65, 98.96, 50.54, 80.22, 45.12, 99.65, 98.96, 50.54, 80.22, 11.22, 22.46 };
                        break;
                    case "Day":
                        OEEDataEntity.Catergory = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
                        OEEDataEntity.Data = new List<double> { 45.12, 99.65, 98.96, 50.54, 80.22, 45.12, 99.65, 98.96, 50.54, 80.22, 11.22, 22.46, 45.12, 99.65, 98.96, 50.54, 80.22, 45.12, 99.65, 98.96, 50.54, 80.22, 11.22, 22.46, 45.12, 99.65, 98.96, 50.54, 80.22, 45.12 };
                        break;
                    case "Week":
                        break;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return OEEDataEntity;
        }
        #endregion
    }

    public class YearDataEntity
    {
        public List<string> Catergory { get; set; }
        public string Title { get; set; }
        public string XaxisLabel { get; set; }
        public string PointerLabel { get; set; }
        public List<double> Data { get; set; }
    }

    public class EfficiencyData : YearDataEntity
    {
        public string Type { get; set; }
        public string DateType { get; set; }
    }

    public class MonthDataEntity : YearDataEntity
    {
        public string Year { get; set; }
    }
    public class DateDataEntity : YearDataEntity
    {
        public string Year { get; set; }
        public string Month { get; set; }
    }
}