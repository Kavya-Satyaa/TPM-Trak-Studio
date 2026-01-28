using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Nippon
{
    public partial class Rejection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ReasonDataEntity GetReasonData()
        {
            ReasonDataEntity reasonDataEntity = new ReasonDataEntity();
            try
            {
                reasonDataEntity.Catergory = new List<string>();

                DataTable dt = TMPTrakDataBase.MachineDownTimeMatrix(Convert.ToDateTime("2021-03-01 06:00:00"), Convert.ToDateTime("2021-03-21 08:30:00"), "", "", "", 0, "DLoss_By_Catagory", "", "s_GetDownTimeMatrixfromAutoData", "");
                DataTable dtnew = dt.AsEnumerable().OrderByDescending(x => x.Field<double>("DownTime")).CopyToDataTable();
                reasonDataEntity.Data = new List<double>();
                reasonDataEntity.Catergory = dtnew.AsEnumerable().Select(x => x.Field<string>("DownID")).ToList();
                reasonDataEntity.Data = dtnew.AsEnumerable().Select(x => x.Field<double>("DownTime")).ToList();
                reasonDataEntity.Title = "Down-Reason";
                reasonDataEntity.XaxisLabel = "Down Reason";
                reasonDataEntity.PointerLabel = " 'Down Reason : <b>{point.y:.1f} %</b>'";


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return reasonDataEntity;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static PieSeries GetReasonPieData()
        {
            PieSeries reasonDataEntity = new PieSeries();
            try
            {
                DataTable dt = TMPTrakDataBase.MachineDownTimeMatrix(Convert.ToDateTime("2021-03-01 06:00:00"), Convert.ToDateTime("2021-03-21 08:30:00"), "", "", "", 0, "DLoss_By_Catagory", "", "s_GetDownTimeMatrixfromAutoData", "");
                DataTable dtnew = dt.AsEnumerable().OrderByDescending(x => x.Field<double>("DownTime")).CopyToDataTable();
                reasonDataEntity.Data = new List<PIEData>();
                //reasonDataEntity.Catergory = dtnew.AsEnumerable().Select(x => x.Field<string>("DownID")).ToList();
                //reasonDataEntity.Data = dtnew.AsEnumerable().Select(x => x.Field<double>("DownTime")).ToList();
                foreach(DataRow row in dtnew.Rows)
                {
                    reasonDataEntity.Data.Add(new PIEData { name = row["DownID"].ToString(), y = Convert.ToDouble(row["DownTime"]) });

                }
                reasonDataEntity.Title = "Down-Reason";
                reasonDataEntity.XaxisLabel = "Down Reason";
                reasonDataEntity.PointerLabel = " 'Down Reason : <b>{point.y:.1f} %</b>'";


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return reasonDataEntity;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ReasonDataEntity GetCategoryData()
        {
            ReasonDataEntity reasonDataEntity = new ReasonDataEntity();
            try
            {
                reasonDataEntity.Catergory = new List<string>();

                DataTable dt = TMPTrakDataBase.MachineDownTimeMatrix(Convert.ToDateTime("2021-03-01 06:00:00"), Convert.ToDateTime("2021-03-21 08:30:00"), "", "", "", 0, "DLoss_By_Catagory", "", "s_GetDownTimeMatrixfromAutoData", "");
                DataTable dtnew = dt.AsEnumerable().GroupBy(r => r.Field<string>("Catagory").ToUpper()).OrderByDescending(g => g.Max(r => r.Field<double>("DownTime"))).Select(g =>
                {
                    var row = dt.NewRow();
                    row["Catagory"] = g.Key.ToUpper();
                    row["DownTime"] = g.Sum(r => r.Field<double>("DownTime"));
                    return row;
                }).CopyToDataTable();
                reasonDataEntity.Data = new List<double>();
                reasonDataEntity.Catergory = dtnew.AsEnumerable().Select(x => x.Field<string>("Catagory")).ToList();
                reasonDataEntity.Data = dtnew.AsEnumerable().Select(x => x.Field<double>("DownTime")).ToList();
                reasonDataEntity.Title = "Down-Reason";
                reasonDataEntity.XaxisLabel = "Down Reason";
                reasonDataEntity.PointerLabel = " 'Down Reason : <b>{point.y:.1f} %</b>'";


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return reasonDataEntity;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static PieSeries GetCategoryPieData()
        {
            PieSeries reasonDataEntity = new PieSeries();
            try
            {
               
                DataTable dt = TMPTrakDataBase.MachineDownTimeMatrix(Convert.ToDateTime("2021-03-01 06:00:00"), Convert.ToDateTime("2021-03-21 08:30:00"), "", "", "", 0, "DLoss_By_Catagory", "", "s_GetDownTimeMatrixfromAutoData", "");
                DataTable dtnew = dt.AsEnumerable().GroupBy(r => r.Field<string>("Catagory").ToUpper()).OrderByDescending(g => g.Max(r => r.Field<double>("DownTime"))).Select(g =>
                {
                    var row = dt.NewRow();
                    row["Catagory"] = g.Key.ToUpper();
                    row["DownTime"] = g.Sum(r => r.Field<double>("DownTime"));
                    return row;
                }).CopyToDataTable();
                reasonDataEntity.Data = new List<PIEData>();
                foreach(DataRow Row in dtnew.Rows)
                {
                    reasonDataEntity.Data.Add(new PIEData { name = Row["Catagory"].ToString(), y = Convert.ToDouble(Row["DownTime"]) });
                }
                //reasonDataEntity.Data = dtnew.AsEnumerable().Select(x => x.Field<double>("DownTime")).ToList();
                reasonDataEntity.Title = "Down-Reason";
                reasonDataEntity.XaxisLabel = "Down Reason";
                reasonDataEntity.PointerLabel = " 'Down Reason : <b>{point.y:.1f} %</b>'";


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return reasonDataEntity;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ReasonDataEntity GetSubCategoryData()
        {
            ReasonDataEntity reasonDataEntity = new ReasonDataEntity();
            try
            {
                reasonDataEntity.Catergory = new List<string>();

                DataTable dt = TMPTrakDataBase.MachineDownTimeMatrix(Convert.ToDateTime("2021-03-01 06:00:00"), Convert.ToDateTime("2021-03-21 08:30:00"), "", "", "", 0, "DLoss_By_Catagory", "", "s_GetDownTimeMatrixfromAutoData", "");
                DataTable dtnew = dt.AsEnumerable().OrderByDescending(x => x.Field<double>("DownTime")).CopyToDataTable();
                reasonDataEntity.Data = new List<double>();
                reasonDataEntity.Catergory = new List<string> { "SubCat1", "SubCat2", "SubCat3", "SubCat4", "SubCat5", "SubCat6" };
                reasonDataEntity.Data = new List<double> { 1234, 960, 412, 123, 96, 54 };
                reasonDataEntity.Title = "Down-Reason";
                reasonDataEntity.XaxisLabel = "Down Reason";
                reasonDataEntity.PointerLabel = " 'Down Reason : <b>{point.y:.1f} %</b>'";


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return reasonDataEntity;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static PieSeries GetSubCategoryPieData()
        {
            PieSeries reasonDataEntity = new PieSeries();
            try
            {
        

                DataTable dt = TMPTrakDataBase.MachineDownTimeMatrix(Convert.ToDateTime("2021-03-01 06:00:00"), Convert.ToDateTime("2021-03-21 08:30:00"), "", "", "", 0, "DLoss_By_Catagory", "", "s_GetDownTimeMatrixfromAutoData", "");
                DataTable dtnew = dt.AsEnumerable().OrderByDescending(x => x.Field<double>("DownTime")).CopyToDataTable();
                reasonDataEntity.Data = new List<PIEData>();
                reasonDataEntity.Title = "Down-Reason";
                reasonDataEntity.XaxisLabel = "Down Reason";
                reasonDataEntity.PointerLabel = " 'Down Reason : <b>{point.y:.1f} %</b>'";


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return reasonDataEntity;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ReasonDataEntity GetSubCategoryDataByCategory(string Category)
        {
            ReasonDataEntity reasonDataEntity = new ReasonDataEntity();
            try
            {
                reasonDataEntity.Catergory = new List<string>();

                DataTable dt = TMPTrakDataBase.MachineDownTimeMatrix(Convert.ToDateTime("2021-03-01 06:00:00"), Convert.ToDateTime("2021-03-21 08:30:00"), "", "", "", 0, "DLoss_By_Catagory", "", "s_GetDownTimeMatrixfromAutoData", "");
                DataTable dtnew = dt.AsEnumerable().OrderByDescending(x => x.Field<double>("DownTime")).CopyToDataTable();
                reasonDataEntity.Data = new List<double>();
                reasonDataEntity.Catergory = new List<string> { "SubCat1", "SubCat2", "SubCat3", "SubCat4", "SubCat5", "SubCat6" };
                reasonDataEntity.Data = new List<double> { 1234, 960, 412, 123, 96, 54 };
                reasonDataEntity.Series = new List<Series>();
                reasonDataEntity.Series.Add(new Series { name = "SubCat1", y =1234, drilldown = "SubCat1" });
                reasonDataEntity.Series.Add(new Series { name = "SubCat2", y =960, drilldown = "SubCat2" });
                reasonDataEntity.Series.Add(new Series { name = "SubCat3", y =412, drilldown = "SubCat3" });
                reasonDataEntity.Series.Add(new Series { name = "SubCat4", y =123, drilldown = "SubCat4" });
                reasonDataEntity.Series.Add(new Series { name = "SubCat5", y =96, drilldown = "SubCat5" });
                reasonDataEntity.Series.Add(new Series { name = "SubCat6", y =54, drilldown = "SubCat6" });
                reasonDataEntity.Title = "Down-Reason";
                reasonDataEntity.XaxisLabel = "Down Reason";
                reasonDataEntity.PointerLabel = " 'Down Reason : <b>{point.y:.1f} %</b>'";

                reasonDataEntity.DrillDownData = new List<DrildownSeries>();
                reasonDataEntity.DrillDownData.Add(new DrildownSeries { name = "SubCat1", id = "SubCat1", data = new List<DrildownData>()});
                reasonDataEntity.DrillDownData.Add(new DrildownSeries { name = "SubCat2", id = "SubCat2", data = new List<DrildownData>()});
                reasonDataEntity.DrillDownData.Add(new DrildownSeries { name = "SubCat3", id = "SubCat3", data = new List<DrildownData>()});
                reasonDataEntity.DrillDownData.Add(new DrildownSeries { name = "SubCat4", id = "SubCat4", data = new List<DrildownData>()});
                reasonDataEntity.DrillDownData.Add(new DrildownSeries { name = "SubCat5", id = "SubCat5", data = new List<DrildownData>()});
                reasonDataEntity.DrillDownData.Add(new DrildownSeries { name = "SubCat6", id = "SubCat6", data = new List<DrildownData>()});
                foreach (DrildownSeries dls in reasonDataEntity.DrillDownData)
                {
                    dls.data.Add(new DrildownData { name = "NO_DATA", y = 555.42 });
                    dls.data.Add(new DrildownData { name = "Defect Analysis", y = 423.13 });
                    dls.data.Add(new DrildownData { name = "Inspection", y = 331.64 });
                    dls.data.Add(new DrildownData { name = "Breakdown", y = 213.45 });
                    dls.data.Add(new DrildownData { name = "No Man Power", y = 192.64 });
                    dls.data.Add(new DrildownData { name = "Lunch", y = 101.74 });
                    dls.data.Add(new DrildownData { name = "Meeting", y = 86.32 });
                    dls.data.Add(new DrildownData { name = "No Schedule", y = 53.42 });
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return reasonDataEntity;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ReasonDataEntity GetSubCategoryPieDataByCategory(string Category)
        {
            ReasonDataEntity reasonDataEntity = new ReasonDataEntity();
            try
            {
                reasonDataEntity.Catergory = new List<string>();

                DataTable dt = TMPTrakDataBase.MachineDownTimeMatrix(Convert.ToDateTime("2021-03-01 06:00:00"), Convert.ToDateTime("2021-03-21 08:30:00"), "", "", "", 0, "DLoss_By_Catagory", "", "s_GetDownTimeMatrixfromAutoData", "");
                DataTable dtnew = dt.AsEnumerable().OrderByDescending(x => x.Field<double>("DownTime")).CopyToDataTable();
                reasonDataEntity.Data = new List<double>();
                reasonDataEntity.Catergory = new List<string> { "SubCat1", "SubCat2", "SubCat3", "SubCat4", "SubCat5", "SubCat6" };
                reasonDataEntity.Data = new List<double> { 1234, 960, 412, 123, 96, 54 };
                reasonDataEntity.Series = new List<Series>();
                reasonDataEntity.Series.Add(new Series { name = "SubCat1", y = 1234, drilldown = "SubCat1" });
                reasonDataEntity.Series.Add(new Series { name = "SubCat2", y = 960, drilldown = "SubCat2" });
                reasonDataEntity.Series.Add(new Series { name = "SubCat3", y = 412, drilldown = "SubCat3" });
                reasonDataEntity.Series.Add(new Series { name = "SubCat4", y = 123, drilldown = "SubCat4" });
                reasonDataEntity.Series.Add(new Series { name = "SubCat5", y = 96, drilldown = "SubCat5" });
                reasonDataEntity.Series.Add(new Series { name = "SubCat6", y = 54, drilldown = "SubCat6" });
                reasonDataEntity.Title = "Down-Reason";
                reasonDataEntity.XaxisLabel = "Down Reason";
                reasonDataEntity.PointerLabel = " 'Down Reason : <b>{point.y:.1f} %</b>'";

                reasonDataEntity.DrillDownData = new List<DrildownSeries>();
                reasonDataEntity.DrillDownData.Add(new DrildownSeries { name = "SubCat1", id = "SubCat1", data = new List<DrildownData>() });
                reasonDataEntity.DrillDownData.Add(new DrildownSeries { name = "SubCat2", id = "SubCat2", data = new List<DrildownData>() });
                reasonDataEntity.DrillDownData.Add(new DrildownSeries { name = "SubCat3", id = "SubCat3", data = new List<DrildownData>() });
                reasonDataEntity.DrillDownData.Add(new DrildownSeries { name = "SubCat4", id = "SubCat4", data = new List<DrildownData>() });
                reasonDataEntity.DrillDownData.Add(new DrildownSeries { name = "SubCat5", id = "SubCat5", data = new List<DrildownData>() });
                reasonDataEntity.DrillDownData.Add(new DrildownSeries { name = "SubCat6", id = "SubCat6", data = new List<DrildownData>() });
                foreach (DrildownSeries dls in reasonDataEntity.DrillDownData)
                {
                    dls.data.Add(new DrildownData { name = "NO_DATA", y = 555.42 });
                    dls.data.Add(new DrildownData { name = "Defect Analysis", y = 423.13 });
                    dls.data.Add(new DrildownData { name = "Inspection", y = 331.64 });
                    dls.data.Add(new DrildownData { name = "Breakdown", y = 213.45 });
                    dls.data.Add(new DrildownData { name = "No Man Power", y = 192.64 });
                    dls.data.Add(new DrildownData { name = "Lunch", y = 101.74 });
                    dls.data.Add(new DrildownData { name = "Meeting", y = 86.32 });
                    dls.data.Add(new DrildownData { name = "No Schedule", y = 53.42 });
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return reasonDataEntity;
        }
    }



    public class ReasonDataEntity : SeriesData
    {

        public string Title { get; set; }
        public string XaxisLabel { get; set; }
        public string PointerLabel { get; set; }
        public List<Series> Series { get; set; }
        public List<DrildownSeries> DrillDownData { get; set; }
    }

    public class Series
    {
        public string name { get; set; }
        public double y { get; set; }
        public string drilldown { get; set; }
    }
    public class SeriesData
    {
        public List<string> Catergory { get; set; }
        public List<double> Data { get; set; }
    }
    public class DrildownSeries
    {
      
        public string name { get; set; }
      
        public string id { get; set; }
        public double y { get; set; }
       
        public List<DrildownData> data { get; set; }
    }
    public class DrildownData
    {
        public string name { get; set; }
        public double y { get; set; }
    
    }

    public class PIEData
    {
        public string name { get; set; }
        public double y { get; set; }
    }
    public class PieSeries
    {
        public string Title { get; set; }
        public string XaxisLabel { get; set; }
        public string PointerLabel { get; set; }
        public List<PIEData> Data { get; set; }
    }
}