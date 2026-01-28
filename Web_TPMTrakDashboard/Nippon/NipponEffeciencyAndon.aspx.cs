using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Nippon.DataBaseAccess;
using Web_TPMTrakDashboard.Nippon.Model;

namespace Web_TPMTrakDashboard.Nippon
{
    public partial class NipponEffeciencyAndon : System.Web.UI.Page
    {
        public int FlipInterval = Convert.ToInt32(ConfigurationManager.AppSettings["IndiaNipponFlip"].ToString());
        public int RefreshDataInt = Convert.ToInt32(ConfigurationManager.AppSettings["IndiaNipponRefresh"].ToString()); 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null)
            {
                Response.Redirect("~/SignIn.aspx", false);
            }
            else if (!IsPostBack)
            {
                HttpContext.Current.Session["ANdonScreen"] = "FirstScreen";
                HttpContext.Current.Session["AndonData"] = null;
                LoadData();
                setCompanyLogo();
            }
        }

        private void LoadData()
        {
            try
            {
                List<string> CellID = new List<string>();
                CellID = BindCockpitView.ViewCellsToDisplay("");
                CellID = CellID == null ? new List<string>(): CellID;
                {
                    CellID.Insert(0, "All");
                    drpcellid.DataSource = CellID;
                    drpcellid.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void setCompanyLogo()
        {
            //const string imagesPath = "~/CompanyLogo/";// "~/Image/Slideshow/";
            //var dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(imagesPath));

            ////filtering to jpgs, but ideally not required
            //List<string> fileNames = (from flInfo in dir.GetFiles() select flInfo.Name).ToList();
            //if (fileNames.Count > 0)
            //{
            //    Image2.ImageUrl = imagesPath + fileNames[0];
            //}
            //else
            //{
            //    Image2.ImageUrl = "Image/companyIcon.png";
            //}
            Image2.ImageUrl = Web_TPMTrakDashboard.Models.Util.getCompanyLogoPath();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static NIPPONAndonEntity GETDATA(string CellID, int ScreenHeight)
        {
            ScreenHeight = ScreenHeight - 500;
            DataTable dt = new DataTable();
            List<NipponAndonHourlyDataEntity> EntityFirst = new List<NipponAndonHourlyDataEntity>();
            NipponAndonEfficiencyEntity Entity = new NipponAndonEfficiencyEntity();
            NIPPONAndonEntity AndonEntity = new NIPPONAndonEntity();
            try
            {
                CellID = CellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : CellID;
                string screen = HttpContext.Current.Session["ANdonScreen"] == null ? "FirstScreen" : HttpContext.Current.Session["ANdonScreen"].ToString();
                var prevShiftVals = Web_TPMTrakDashboard.Models.DataBaseAccess.GetCurrentShiftVals("[s_GetCurrentShiftTime]");
                DataTable MainData = new DataTable();
                DataTable DtProcuctionDaTa = new DataTable();
                string strDate = Convert.ToDateTime(prevShiftVals[0]).ToString("yyyy-MM-dd HH:mm:ss");
                string endDate = Convert.ToDateTime(prevShiftVals[1]).ToString("yyyy-MM-dd HH:mm:ss");
                int rowstotake = 0, RowsTaken = 0, Count = 0;
                HttpContext.Current.Session["ShiftName"] = prevShiftVals[2];
                AndonEntity.Screen = screen;
                switch (screen)
                {
                    case "FirstScreen":
                        if (HttpContext.Current.Session["AndonData"] == null)
                        {

                            MainData = Web_TPMTrakDashboard.Models.DataBaseAccess.GetNipponIndiaAndon(strDate, endDate, CellID, out DtProcuctionDaTa);
                            HttpContext.Current.Session["AndonData"] = MainData;
                            HttpContext.Current.Session["AndonDataProd"] = DtProcuctionDaTa;
                        }
                        else
                        {
                            MainData = HttpContext.Current.Session["AndonData"] as DataTable;
                            DtProcuctionDaTa = HttpContext.Current.Session["AndonDataProd"] as DataTable;
                        }


                        AndonEntity.FirstScreen = EntityFirst;
                        DataTable NwwDtProcuctionDaTa = new DataTable();
                        rowstotake = HttpContext.Current.Session["EffAndonRowsToTake"] == null ? 5 : Convert.ToInt32(HttpContext.Current.Session["EffAndonRowsToTake"]);
                        RowsTaken = HttpContext.Current.Session["EffRowTaken"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["EffRowTaken"]);
                        if (DtProcuctionDaTa.Rows.Count > 0)
                        {
                            rowstotake = ScreenHeight / DtProcuctionDaTa.Rows.Count;
                            //NwwDtProcuctionDaTa = DtProcuctionDaTa.AsEnumerable().Skip(RowsTaken).Take(rowstotake).CopyToDataTable();
                            NwwDtProcuctionDaTa = DtProcuctionDaTa.AsEnumerable().Skip(0).Take(DtProcuctionDaTa.Rows.Count).CopyToDataTable();
                            List<string> MachineList = NwwDtProcuctionDaTa.AsEnumerable().Select(x => x.Field<string>("MachineID")).Distinct().ToList();
                            foreach (string Machine in MachineList)
                            {
                                NipponAndonHourlyDataEntity data = new NipponAndonHourlyDataEntity();
                                data.MachineID = Machine;
                                data.Shift = prevShiftVals[2];
                                data.Production = MainData.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(Machine)).Select(x => x.Field<double>("Components")).SingleOrDefault().ToString();
                                data.Avaliability = MainData.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(Machine)).Select(x => x.Field<double>("AvailabilityEfficiency")).SingleOrDefault().ToString();
                                data.Productivity = MainData.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(Machine)).Select(x => x.Field<double>("ProductionEfficiency")).SingleOrDefault().ToString();
                                data.OEE = MainData.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(Machine)).Select(x => x.Field<double>("OverallEfficiency")).SingleOrDefault().ToString();

                                data.color = (MainData.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(Machine)).Select(x => x.Field<double>("OverallEfficiency")).SingleOrDefault()) <= 50 ? "red" : (MainData.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(Machine)).Select(x => x.Field<double>("OverallEfficiency")).SingleOrDefault()) <= 75 ? "yellow" : "green";
                                data.backcolor = data.color.Equals("red") ? "white" : "black";
                                data.Quality = MainData.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(Machine)).Select(x => x.Field<double>("QualityEfficiency")).SingleOrDefault().ToString();
                                data.ActualData = NwwDtProcuctionDaTa.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(Machine)).Select(x => x.Field<int>("Actual")).ToList();
                                data.TargetData = NwwDtProcuctionDaTa.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(Machine)).Select(x => x.Field<int>("Target")).ToList();
                                //List<Double> oeeDataList=data.OEEData = NwwDtProcuctionDaTa.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(Machine)).Select(x => x.Field<Double>("OverAllEfficiency")).ToList<Double>();
                                List<Double> oeeDataList = new List<Double>();
                                oeeDataList= NwwDtProcuctionDaTa.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(Machine)).Select(x => x.Field<Double>("OverAllEfficiency")).ToList();
                                //for (int i = 0; i < oeeDataList.Count; i++)
                                //{
                                //    oeeDataList[i] = Math.Round(oeeDataList[i], 2);
                                //}
                                data.OEEData = oeeDataList;
                                List<string> OEEDataBackColor = new List<string>();
                                if (oeeDataList.Count > 0)
                                {
                                    for (int i = 0; i < oeeDataList.Count; i++)
                                    {
                                        string backColor = "";
                                        if (oeeDataList[i] <= 50)
                                        {
                                            backColor = "red";
                                        }
                                        else if (oeeDataList[i] <= 75)
                                        {
                                            backColor = "yellow";
                                        }
                                        else
                                        {
                                            backColor = "green";
                                        }
                                        OEEDataBackColor.Add(backColor);
                                    }
                                }
                                data.OEEDataBackColor = OEEDataBackColor;
                                data.CummulativeTarget = MainData.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(Machine)).Select(x => x.Field<int>("Target")).SingleOrDefault().ToString();
                                EntityFirst.Add(data);
                            }
                            RowsTaken += NwwDtProcuctionDaTa.Rows.Count;
                        }
                        else
                        {
                            rowstotake = 0;
                            EntityFirst = new List<NipponAndonHourlyDataEntity>();
                        }
                        if (RowsTaken >= DtProcuctionDaTa.Rows.Count)
                        {
                            HttpContext.Current.Session["EffAndonData"] = null;
                            HttpContext.Current.Session["MachineImageAndonData"] = null;
                            HttpContext.Current.Session["ANdonScreen"] = "SecondScreen";
                            RowsTaken = 0;
                        }
                        HttpContext.Current.Session["EffRowTaken"] = RowsTaken;
                        break;
                    case "SecondScreen":
                        if (HttpContext.Current.Session["EffAndonData"] == null)
                        {

                            dt = CockpitDataBaseAccess.GetMachineCockpitDataForPlantCell("s_GetCockpitData_WithTempTable_eshopx", strDate, endDate, CellID);
                            HttpContext.Current.Session["EffAndonData"] = dt;
                        }
                        else
                        {
                            dt = HttpContext.Current.Session["EffAndonData"] as DataTable;
                        }
                        if (dt != null && dt.Rows.Count > 0)
                        {

                            rowstotake = HttpContext.Current.Session["EffAndonRowsToTake"] == null ? 5 : Convert.ToInt32(HttpContext.Current.Session["EffAndonRowsToTake"]);
                            RowsTaken = HttpContext.Current.Session["EffRowTaken"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["EffRowTaken"]);
                            Count = HttpContext.Current.Session["EffAndonCount"] == null ? 5 : Convert.ToInt32(HttpContext.Current.Session["EffAndonCount"]);
                            DataTable dtnew = dt.AsEnumerable().Skip(RowsTaken).Take(rowstotake).CopyToDataTable();
                            RowsTaken += rowstotake;
                            if (RowsTaken >= dt.Rows.Count)
                            {
                                HttpContext.Current.Session["ANdonScreen"] = "FirstScreen";
                                HttpContext.Current.Session["AndonData"] = null;
                                HttpContext.Current.Session["MachineImageAndonData"] = null;
                                RowsTaken = 0;
                            }
                            HttpContext.Current.Session["EffRowTaken"] = RowsTaken;
                            Entity.MachineList = dtnew.AsEnumerable().Select(x => x.Field<string>("MachineId")).ToList();
                            Entity.Screen = "SecondScreen";
                            Entity.Shift = HttpContext.Current.Session["ShiftName"] == null ? "" : HttpContext.Current.Session["ShiftName"].ToString();
                            string avgEffy= dtnew.AsEnumerable().Average(x => x.Field<double>("OverAllEfficiency")).ToString();
                            Entity.OverallOEE = string.IsNullOrEmpty(avgEffy) ? "0" : Math.Round(Convert.ToDecimal(avgEffy), 0).ToString();
                            avgEffy = dtnew.AsEnumerable().Average(x => x.Field<double>("AvailabilityEfficiency")).ToString();
                            Entity.OverallAE = string.IsNullOrEmpty(avgEffy) ? "0" : Math.Round(Convert.ToDecimal(avgEffy), 0).ToString();
                            avgEffy = dtnew.AsEnumerable().Average(x => x.Field<double>("ProductionEfficiency")).ToString();
                            Entity.OverallPE = string.IsNullOrEmpty(avgEffy) ? "0" : Math.Round(Convert.ToDecimal(avgEffy), 0).ToString();
                            avgEffy = dtnew.AsEnumerable().Average(x => x.Field<double>("QualityEfficiency")).ToString();
                            Entity.OverallQE = string.IsNullOrEmpty(avgEffy) ? "0" : Math.Round(Convert.ToDecimal(avgEffy), 0).ToString();
                            Entity.AEList = new List<NipponAndonIndividualMachineAE>();
                            Entity.PEList = new List<NipponAndonIndividualMachinePE>();
                            Entity.QEList = new List<NipponAndonIndividualMachineQE>();
                            Entity.OEEList = new List<NipponAndonIndividualMachineOEE>();
                            List<NipponAndonIndividualMachineOEE> OEEEnt = new List<NipponAndonIndividualMachineOEE>();
                            List<NipponAndonIndividualMachineQE> QEEnt = new List<NipponAndonIndividualMachineQE>();
                            List<NipponAndonIndividualMachinePE> PEEnt = new List<NipponAndonIndividualMachinePE>();
                            List<NipponAndonIndividualMachineAE> AEEnt = new List<NipponAndonIndividualMachineAE>();
                            foreach (string Machine in Entity.MachineList)
                            {
                                AEEnt.Add(new NipponAndonIndividualMachineAE { MachineID = Machine, AE = dtnew.AsEnumerable().Select(x => x.Field<double>("AvailabilityEfficiency")).ToList() });
                                PEEnt.Add(new NipponAndonIndividualMachinePE { MachineID = Machine, PE = dtnew.AsEnumerable().Select(x => x.Field<double>("ProductionEfficiency")).ToList() });
                                QEEnt.Add(new NipponAndonIndividualMachineQE { MachineID = Machine, QE = dtnew.AsEnumerable().Select(x => x.Field<double>("QualityEfficiency")).ToList() });
                                OEEEnt.Add(new NipponAndonIndividualMachineOEE { MachineID = Machine, OEE = dtnew.AsEnumerable().Select(x => x.Field<double>("OverAllEfficiency")).ToList() });
                            }
                            Entity.AEList = AEEnt;
                            Entity.PEList = PEEnt;
                            Entity.QEList = QEEnt;
                            Entity.OEEList = OEEEnt;
                        }
                        else
                        {
                            HttpContext.Current.Session["ANdonScreen"] = "FirstScreen";
                            HttpContext.Current.Session["AndonData"] = null;
                            HttpContext.Current.Session["MachineImageAndonData"] = null;
                            RowsTaken = 0;
                        }
                        AndonEntity.SecondScreen = Entity;
                        break;
                    case "ThirdScreen":
                        MachineAndonDetails machineAndonDetails = new MachineAndonDetails();
                        List<AndonSettingData> listAndonDetails = new List<AndonSettingData>();
                        List<AndonSettingData> listMachineDetails = new List<AndonSettingData>();
                        List<AndonSettingData> listBackImgDetails = new List<AndonSettingData>();
                        List<AndonSettingData> listFinalMachine = new List<AndonSettingData>();
                        string backImagePath = "";
                        if (HttpContext.Current.Session["MachineImageAndonData"] == null)
                        {
                            listAndonDetails = DBAccess.getAndonMachineDetails();
                            HttpContext.Current.Session["MachineImgPathAndonData"] = listAndonDetails;
                            dt = CockpitDataBaseAccess.GetMachineCockpitDataForPlantCell("s_GetCockpitData_WithTempTable_eshopx", strDate, endDate, CellID);
                            HttpContext.Current.Session["MachineImageAndonData"] = dt;
                        }
                        else
                        {
                            dt = HttpContext.Current.Session["MachineImageAndonData"] as DataTable;
                            if (HttpContext.Current.Session["MachineImgPathAndonData"] == null)
                            {
                                listAndonDetails = DBAccess.getAndonMachineDetails();
                            }
                            else
                            {
                                listAndonDetails = HttpContext.Current.Session["MachineImgPathAndonData"] as List<AndonSettingData>;
                            }
                        }
                        if (dt.Rows.Count > 0)
                        {
                            HttpContext.Current.Session["ANdonScreen"] = "FirstScreen";
                            HttpContext.Current.Session["AndonData"] = null;
                            HttpContext.Current.Session["EffAndonData"] = null;
                            listMachineDetails = listAndonDetails.Where(k => k.Parameter == "NipponAndonMachineSetting").ToList();
                            listBackImgDetails = listAndonDetails.Where(k => k.Parameter == "NipponAndonSetting" && k.MachineID == "BackgroundImage").ToList();
                            if (listBackImgDetails.Count > 0)
                            {
                                backImagePath = listBackImgDetails[0].ImagePath;
                            }
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                AndonSettingData data = new AndonSettingData();
                                data.MachineID = dt.Rows[i]["MachineID"].ToString();
                                double OEE= Convert.ToDouble(dt.Rows[i]["OverallEfficiency"].ToString());
                                data.FontColor = OEE <= 50 ? "red" : OEE <= 75 ? "yellow" : "green";
                                data.ProdutionOEE = "Production:" + dt.Rows[i]["Components"].ToString() + "  OEE: " + dt.Rows[i]["OverallEfficiency"].ToString() + "%";
                                data.ImagePath = "";
                                if (listMachineDetails.Count > 0)
                                {
                                    var machineimges = listMachineDetails.Where(k => k.MachineID == data.MachineID).Select(k => k.ImagePath).ToList();
                                    if (machineimges != null)
                                    {
                                        if (machineimges.Count > 0)
                                        {
                                            data.ImagePath = listMachineDetails.Where(k => k.MachineID == data.MachineID).Select(k => k.ImagePath).ToList().First();
                                        }
                                    }
                                }
                                listFinalMachine.Add(data);
                            }
                         
                        }
                        else
                        {
                            HttpContext.Current.Session["ANdonScreen"] = "FirstScreen";
                            HttpContext.Current.Session["AndonData"] = null;
                            HttpContext.Current.Session["EffAndonData"] = null;
                            RowsTaken = 0;
                        }
                        machineAndonDetails.MachineDetails = listFinalMachine;
                        machineAndonDetails.BackgroundImgPath = backImagePath;
                        machineAndonDetails.Shift = HttpContext.Current.Session["ShiftName"] == null ? "" : HttpContext.Current.Session["ShiftName"].ToString();
                        AndonEntity.ThirdScreen = machineAndonDetails;
                        break;
                }


            }

            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return AndonEntity;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static void RefreshData()
        {
            try
            {
                HttpContext.Current.Session["EffAndonRowsToTake"] = 5;
                HttpContext.Current.Session["EffRowTaken"] = 0;
                HttpContext.Current.Session["EffAndonCount"] = 5;
                HttpContext.Current.Session["AndonData"] = null;
                HttpContext.Current.Session["EffAndonData"] = null;
                HttpContext.Current.Session["MachineImageAndonData"] = null;
                HttpContext.Current.Session["MachineImgPathAndonData"] = null;
                HttpContext.Current.Session["ShiftName"] = "";
                HttpContext.Current.Session["ANdonScreen"] = "FirstScreen";
            }

            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

    }

    public class NIPPONAndonEntity
    {
        public NipponAndonEfficiencyEntity SecondScreen { get; set; }
        public List<NipponAndonHourlyDataEntity> FirstScreen { get; set; }

        public MachineAndonDetails ThirdScreen { get; set; }
        public string Screen { get; set; }
    }
    public class MachineAndonDetails
    {
        public List<AndonSettingData> MachineDetails { get; set; }
        public string BackgroundImgPath { get; set; }
        public string Shift { get; set; }
    }
    public class NipponAndonEfficiencyEntity
    {
        public List<string> MachineList { get; set; }
        public string MachineID { get; set; }
        public string Screen { get; set; }
        public string Shift { get; set; }
        public string OverallOEE { get; set; }
        public string OverallAE { get; set; }
        public string OverallQE { get; set; }
        public string OverallPE { get; set; }

        public List<NipponAndonIndividualMachineAE> AEList { get; set; }
        public List<NipponAndonIndividualMachinePE> PEList { get; set; }
        public List<NipponAndonIndividualMachineQE> QEList { get; set; }
        public List<NipponAndonIndividualMachineOEE> OEEList { get; set; }
    }

    public class NipponAndonIndividualMachineOEE
    {
        public string MachineID { get; set; }
        public List<double> OEE { get; set; }
    }
    public class NipponAndonIndividualMachinePE
    {
        public string MachineID { get; set; }
        public List<double> PE { get; set; }
    }
    public class NipponAndonIndividualMachineQE
    {
        public string MachineID { get; set; }
        public List<double> QE { get; set; }
    }
    public class NipponAndonIndividualMachineAE
    {
        public string MachineID { get; set; }
        public List<double> AE { get; set; }
    }
    public class NipponAndonHourlyDataEntity
    {
        public string MachineID { get; set; }
        public string Screen { get; set; }
        public string Shift { get; set; }
        public string color { get; set; }
        public string backcolor { get; set; }
        public List<int> ActualData { get; set; }
        public List<int> TargetData { get; set; }
        public List<Double> OEEData { get; set; }
        public List<string> OEEDataBackColor { get; set; }
        public string Production { get; set; }
        public string OEE { get; set; }
        public string Productivity { get; set; }
        public string Avaliability { get; set; }
        public string Quality { get; set; }
        public string CummulativeTarget { get; set; }
    }

    public class TargetActualValuesEntity
    {
        public int ActualValue { get; set; }
        public int TargetValue { get; set; }
    }
}