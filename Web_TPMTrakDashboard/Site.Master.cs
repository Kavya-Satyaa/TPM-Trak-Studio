using BusinessClassLibrary;
using Microsoft.AspNet.FriendlyUrls.Resolvers;
using ModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;
        public AppUISettings model = null;
        public List<UserAccessModel> useAccessData = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }
            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {

            //var isMobile = WebFormsFriendlyUrlResolver.IsMobileView(new HttpContextWrapper(Context));
            //string CurrentView = isMobile ? "Mobile" : "Desktop";
            //Logger.WriteDebugLog("View: " + CurrentView);
            try
            {
                #region "User Access Rights Work"
                if (Session["UserName"] == null)
                {
                    Response.Redirect("~/SignIn.aspx", false);
                }
                else
                {
                    if (Session["UserAccessData"] == null)
                        Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"].ToString());
                    else
                        useAccessData = Session["UserAccessData"] as List<UserAccessModel>;
                    if (useAccessData != null)
                    {
                        //dashboard.Visible = useAccessData.Where(ss => ss.Code.Equals("Dashboard", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        //MachineNodeInterface.Visible = useAccessData.Where(ss => ss.Code.Equals("Machinenodeinfo", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        //MachineNodeInterface.Visible = true;
                        //Commented for hiding these three screens. To be uncommented while showing

                        #region Historical Analytics
                        dashboard.Visible = useAccessData.Where(ss => ss.Code.Equals("dashboard", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("HA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        aggregatediconic.Visible = useAccessData.Where(ss => ss.Code.Equals("aggregatediconic", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        aggregatedTable.Visible = useAccessData.Where(ss => ss.Code.Equals("aggregatedTable", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["TAFEPages"].ToString() == "1")
                        {
                            SuppCode.Visible = true;
                            TafeAnalyticsMenu.Visible = true;
                            reportform.Visible = false;
                            aggregatedreport.Visible = false;
                        }
                        else
                        {
                            aggregatedreport.Visible = useAccessData.Where(ss => ss.Code.Equals("aggregatedreport", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        }
                        utiliseddowntimereportagg.Visible = useAccessData.Where(ss => ss.Code.Equals("UtilisedDownTimeDashboard", StringComparison.OrdinalIgnoreCase) & ss.Domain.Equals("HA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["GlobePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            Rejection.Visible = false;
                            //Rejection_Globe.Visible = useAccessData.Where(ss => ss.Code.Equals("Rejection", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            Rejection_Globe.Visible = true;
                        }
                        else
                        {
                            Rejection_Globe.Visible = false;
                            Rejection.Visible = useAccessData.Where(ss => ss.Code.Equals("Rejection", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        }
                        machinedownreasontimematrixagg.Visible = useAccessData.Where(ss => ss.Code.Equals("DownTimeMatrixAgg", StringComparison.OrdinalIgnoreCase) & ss.Domain.Equals("HA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        #endregion

                        #region Live Analytics
                        ionicview.Visible = useAccessData.Where(ss => ss.Code.Equals("IconicView", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        tableview.Visible = useAccessData.Where(ss => ss.Code.Equals("TableView", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        vdgscreen.Visible = useAccessData.Where(ss => ss.Code.Equals("CycleAnalytics", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        shiftproductioncounthourlywithdowncode.Visible = useAccessData.Where(ss => ss.Code.Equals("HourlyTracking", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        machinedownreasontimematrix.Visible = useAccessData.Where(ss => ss.Code.Equals("DownTimeMatrix", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        machinestatus.Visible = useAccessData.Where(ss => ss.Code.Equals("MachineStatus", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        reportform.Visible = useAccessData.Where(ss => ss.Code.Equals("Reports", StringComparison.OrdinalIgnoreCase) & ss.Domain.Equals("CP", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        OEE.Visible = useAccessData.Where(ss => ss.Code.Equals("OEE", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        RunTimeCharts.Visible = useAccessData.Where(ss => ss.Code.Equals("RunTimeCharts", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        alarmhistory.Visible = useAccessData.Where(ss => ss.Code.Equals("AlarmHistory", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        pmactivitytransactiondata.Visible = useAccessData.Where(ss => ss.Code.Equals("PMTransaction", StringComparison.OrdinalIgnoreCase) && !ss.Domain.Equals("Shanti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        ScrapEntryScreen.Visible = useAccessData.Where(ss => ss.Code.Equals("ScrapEntryScreen", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        PPDashboard.Visible = useAccessData.Where(ss => ss.Code.Equals("ProcessParameterDashboard", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("CP", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        spc_ppgraphview.Visible = useAccessData.Where(ss => ss.Code.Equals("ProcessParameterDashboard", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("CP", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        dgdashboardallied.Visible = useAccessData.Where(ss => ss.Code.Equals("DGDashboardAllied", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("CP", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        hydrostaticdashboardallied.Visible = useAccessData.Where(ss => ss.Code.Equals("HydroStaticDashboardAllied", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("CP", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();

                        wipdashboard_tafe.Visible = useAccessData.Where(ss => ss.Code.Equals("WIP_Dashboard", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("CP", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["KunAeroPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            workordermaster_kunaero.Visible = useAccessData.Where(ss => ss.Code.Equals("WorkOrderDetails_KunAero", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        }
                        else
                        {
                            workordermaster_kunaero.Visible = false;
                        }
                        #endregion

                        #region Smart Shop
                        smartshop.Visible = useAccessData.Where(ss => ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Any(ss => ss.Selected);
                        shiftdetails.Visible = useAccessData.Where(ss => ss.Code.Equals("ShiftDetails", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["ShantiIronPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            machineinfoshanthi.Visible = useAccessData.Where(ss => ss.Code.Equals("MachineInfo", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            machineinformation.Visible = false;
                        }
                        else
                        {
                            machineinfoshanthi.Visible = false;
                            machineinformation.Visible = useAccessData.Where(ss => ss.Code.Equals("MachineInfo", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        }

                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            tpmMaster.Visible = true;
                        }
                        else
                        {
                            tpmMaster.Visible = false;
                        }
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["PoojaPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            poojaparametermaster.Visible = useAccessData.Where(ss => ss.Code.Equals("ParameterMasterPooja", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            poojaparameter_grademaster.Visible = useAccessData.Where(ss => ss.Code.Equals("ParamererGradeDataPooja", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        }
                        else
                        {
                            poojaparametermaster.Visible = false;
                            poojaparameter_grademaster.Visible = false;
                        }
                        componentinformation.Visible = useAccessData.Where(ss => ss.Code.Equals("ComponentInfo", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        customerinformation.Visible = useAccessData.Where(ss => ss.Code.Equals("CustomerInfo", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        employeeinformation.Visible = useAccessData.Where(ss => ss.Code.Equals("EmployeeInfo", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        InspectionMasterShanti.Visible = useAccessData.Where(ss => ss.Code.Equals("InspectionMaster", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        downtimecodes.Visible = useAccessData.Where(ss => ss.Code.Equals("DownTimeCodes", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        rejectioncodes.Visible = useAccessData.Where(ss => ss.Code.Equals("RejectionCodes", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        processparameterconfig.Visible = useAccessData.Where(ss => ss.Code.Equals("ProcessParameterSetting", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        liToolSequence.Visible = useAccessData.Where(ss => ss.Code.Equals("ToolSequence", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        PlannedDownTime.Visible = useAccessData.Where(ss => ss.Code.Equals("PlannedDownTime", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();


                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["ShantiIronPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            modifieddataForShanti.Visible = useAccessData.Where(ss => ss.Code.Equals("ModifiedData", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            modifieddata.Visible = false;
                        }
                        else
                        {
                            modifieddata.Visible = useAccessData.Where(ss => ss.Code.Equals("ModifiedData", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            modifieddataForShanti.Visible = false;
                        }


                        loaddefinition.Visible = useAccessData.Where(ss => ss.Code.Equals("LoadDefinition", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        jobcard.Visible = useAccessData.Where(ss => ss.Code.Equals("JobCard", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        targetoee.Visible = useAccessData.Where(ss => ss.Code.Equals("TargetOEE", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        weekdefinition.Visible = useAccessData.Where(ss => ss.Code.Equals("weekdefinition", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        plantinformation.Visible = useAccessData.Where(ss => ss.Code.Equals("plantinformation", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        cell_definitions.Visible = useAccessData.Where(ss => ss.Code.Equals("cell_definitions", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        CreateSchedule.Visible = useAccessData.Where(ss => ss.Code.Equals("CreateSchedule", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        Aggregation.Visible = useAccessData.Where(ss => ss.Code.Equals("Aggregation", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        PMMasterPage.Visible = useAccessData.Where(ss => ss.Code.Equals("PMMaster", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        PMActivityDateGeneration.Visible = useAccessData.Where(ss => ss.Code.Equals("PMGeneration", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        UploadtoHMI.Visible = useAccessData.Where(ss => ss.Code.Equals("UploadtoHMI", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        screenmachineassociation.Visible = useAccessData.Where(ss => ss.Code.Equals("ScreenMachineAss", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        modbusregisterscreen.Visible = useAccessData.Where(ss => ss.Code.Equals("ModbusRegisterScreen", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        if (ConfigurationManager.AppSettings["AceDesignersPage"].ToString() == "1")
                        {
                            scheduledataace.Visible = useAccessData.Where(ss => ss.Code.Equals("ScheduleDataAce", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            ImportScheduleErrorData.Visible = useAccessData.Where(ss => ss.Code.Equals("ScheduleErrorDataAce", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            EventLogScreen.Visible = useAccessData.Where(ss => ss.Code.Equals("EventLogsDataAce", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            AceTPMToSAPProdRejDetails.Visible = true;
                        }
                        BajajJHMaster.Visible = useAccessData.Where(ss => ss.Code.Equals("JHMaster", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        BajajPPMaster.Visible = useAccessData.Where(ss => ss.Code.Equals("PPMaster", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        TafePDIMaster.Visible = useAccessData.Where(ss => ss.Code.Equals("TafePDIMaster", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        ItemStdCycleTimeMaster.Visible = useAccessData.Where(ss => ss.Code.Equals("ItemStdCycleTimeMaster", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["PrecisionEngPages"].ToString() == "1")
                        {
                            multilingualmasters.Visible = useAccessData.Where(ss => ss.Code.Equals("PrecisionHindiDescMasters", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        }
                        else
                        {
                            multilingualmasters.Visible = false;
                        }
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["RexnordPages"].ToString() == "1")
                        {
                            scheduleimport_rexnord.Visible = useAccessData.Where(ss => ss.Code.Equals("ScheduleImport_Rexnord", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            workordertracking_rexnord.Visible = useAccessData.Where(ss => ss.Code.Equals("WorkOrderTracking_Rexnord", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        }
                        else
                        {
                            scheduleimport_rexnord.Visible = false;
                            workordertracking_rexnord.Visible = false;
                        }
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["DensoPages"].ToString() == "1")
                        {
                            modifieddowndenso.Visible = useAccessData.Where(ss => ss.Code.Equals("ModifiedDownDenso", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        }
                        else
                        {
                            modifieddowndenso.Visible = false;
                        }
                        #endregion

                        genericAndon.Visible = useAccessData.Where(ss => ss.Code.Equals("GenericAndon", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        productionandonnew.Visible = useAccessData.Where(ss => ss.Code.Equals("ProductionAndon", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        productionandontableview.Visible = useAccessData.Where(ss => ss.Code.Equals("ProductionAndonTableView", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        TafeHeatTreatmentSchedule.Visible = useAccessData.Where(ss => ss.Code.Equals("HeatTreatmentMasterSchedule", StringComparison.OrdinalIgnoreCase) & ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();

                        HMISetting.Visible = useAccessData.Where(ss => ss.Code.Equals("HMI_Setting", StringComparison.OrdinalIgnoreCase) & ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();




                        #region AdvikPages
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["AdvikPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            liAdvikAndon.Visible = useAccessData.Where(ss => ss.Code.Equals("HelpRequestAndon", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            assemblylineandon.Visible = useAccessData.Where(ss => ss.Code.Equals("AssemblyLineAndon", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            pmchecklistmaster.Visible = useAccessData.Where(ss => ss.Code.Equals("PMCheckListMaster", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            jhDashboard.Visible = useAccessData.Where(ss => ss.Code.Equals("JHDashboard", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            pmMaster.Visible = useAccessData.Where(ss => ss.Code.Equals("PMMaster", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            PmDashboard.Visible = useAccessData.Where(ss => ss.Code.Equals("PMDashboard", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            // PmHelprequest.Visible = useAccessData.Where(ss => ss.Code.Equals("AdvikHelpRequestReport", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            //  HelpRequestSetting.Visible = useAccessData.Where(ss => ss.Code.Equals("AdvikHelpRequestSetting", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            // EmployeeShiftAssociation.Visible = useAccessData.Where(ss => ss.Code.Equals("EmployeeShiftAssociation", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            HolidayPage.Visible = useAccessData.Where(ss => ss.Code.Equals("HolidayList", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            AuditDateScheduler.Visible = useAccessData.Where(ss => ss.Code.Equals("AuditDateScheduler", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            liAdvikReport.Visible = useAccessData.Where(ss => ss.Code.Equals("AdvikReport", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            string userRole = Web_TPMTrakDashboard.Advik.DataBaseAccess.AdvikDatabaseAccess.getEmployeeRole(Session["UserName"].ToString());
                            if (userRole == "Production Supervisor")
                            {
                                liJHSupervisorObservation.Visible = true;
                                liJHProductionHeadObservation.Visible = false;
                            }
                            else
                            if (userRole == "Production Head")
                            {
                                liJHProductionHeadObservation.Visible = true;
                                liJHSupervisorObservation.Visible = false;
                            }
                            else
                            {
                                liJHProductionHeadObservation.Visible = false;
                                liJHSupervisorObservation.Visible = false;
                            }
                        }
                        #endregion

                        #region HelpRequest
                        //liHelpRequest.Visible = useAccessData.Where(ss => ss.Code.Equals("HelpRequest", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        liHelpRequestSettings.Visible = useAccessData.Where(ss => ss.Code.Equals("HelpRequestSettings", StringComparison.OrdinalIgnoreCase) && (ss.Domain.Equals("HelpRequset", StringComparison.OrdinalIgnoreCase) || ss.Domain.Equals("HR", StringComparison.OrdinalIgnoreCase))).Select(ss => ss.Selected).FirstOrDefault();
                        liHelpRequestReport.Visible = useAccessData.Where(ss => ss.Code.Equals("HelpRequestReport", StringComparison.OrdinalIgnoreCase) && (ss.Domain.Equals("HelpRequset", StringComparison.OrdinalIgnoreCase) || ss.Domain.Equals("HR", StringComparison.OrdinalIgnoreCase))).Select(ss => ss.Selected).FirstOrDefault();
                        liEmpShiftAllocation.Visible = useAccessData.Where(ss => ss.Code.Equals("EmpShiftAllocation", StringComparison.OrdinalIgnoreCase) && (ss.Domain.Equals("HelpRequset", StringComparison.OrdinalIgnoreCase) || ss.Domain.Equals("HR", StringComparison.OrdinalIgnoreCase))).Select(ss => ss.Selected).FirstOrDefault();
                        //liHelpRequest.Visible = liHelpRequestSettings.Visible ? (liHelpRequestReport.Visible ? (liEmpShiftAllocation.Visible ? true : false) : false) : false;
                        Logger.WriteDebugLog(liEmpShiftAllocation.Visible.ToString());
                        if (liHelpRequestSettings.Visible || liHelpRequestReport.Visible || liEmpShiftAllocation.Visible)
                        {
                            if (System.Web.Configuration.WebConfigurationManager.AppSettings["HelpRequest"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                            {
                                liHelpRequest.Visible = true;
                            }
                            else
                            {
                                liHelpRequest.Visible = false;
                            }
                            //liHelpRequest.Visible = true;
                        }
                        else
                        {
                            liHelpRequest.Visible = false;
                        }
                        #endregion

                        #region AlertModule
                        LiAddConsumer.Visible = useAccessData.Where(ss => ss.Code.Equals("AddConsumer", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        LiDefineRules.Visible = useAccessData.Where(ss => ss.Code.Equals("DefineRule", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        LiMapruletomachine.Visible = useAccessData.Where(ss => ss.Code.Equals("Mapruletomachine", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        LiMapruletoconsumer.Visible = useAccessData.Where(ss => ss.Code.Equals("Mapruletoconsumer", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        Lishiftallocation.Visible = useAccessData.Where(ss => ss.Code.Equals("ShiftAllocation", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        LiSMSsettings.Visible = useAccessData.Where(ss => ss.Code.Equals("SMSSettings", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        LiSMSstatus.Visible = useAccessData.Where(ss => ss.Code.Equals("SMSStatus", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        if (LiAddConsumer.Visible || LiDefineRules.Visible || LiMapruletomachine.Visible || LiMapruletoconsumer.Visible || Lishiftallocation.Visible || LiSMSsettings.Visible || LiSMSstatus.Visible)
                        {
                            if (System.Web.Configuration.WebConfigurationManager.AppSettings["AlertPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                            {
                                AlertModuleMainMenu.Visible = true;
                            }
                            else
                            {
                                AlertModuleMainMenu.Visible = false;
                            }
                        }
                        else
                        {
                            AlertModuleMainMenu.Visible = false;
                        }
                        #endregion

                        #region Settings
                        //ApplicationSettings.Visible = useAccessData.Where(ss => ss.Code.Equals("Applicationsettings", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        //SmartShopDefaults.Visible = useAccessData.Where(ss => ss.Code.Equals("SmartShopSettings", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        //Cockpitdefaults.Visible = useAccessData.Where(ss => ss.Code.Equals("Cockpitsettings", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        //BandColors.Visible = useAccessData.Where(ss => ss.Code.Equals("OEQEPEcolor", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        #endregion

                        #region OtherPages


                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["ShowSPCWebMenu"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            if (useAccessData.Where(ss => ss.Code.Equals("ShowSPCWebMenu", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault() || useAccessData.Where(ss => ss.Code.Equals("SPCModule", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                            {
                                liSpcWeb.Visible = true;
                            }
                            else
                            {
                                liSpcWeb.Visible = false;
                            }
                        }
                        else
                        {
                            liSpcWeb.Visible = false;
                        }
                        //liHelpRequest.Visible = useAccessData.Where(ss => ss.Code.Equals("HelpRequest", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        #endregion

                        #region GEA
                        gea.Visible = useAccessData.Where(ss => ss.Code.Equals("GEAAndon", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        dailychecklistmaster.Visible = useAccessData.Where(ss => ss.Code.Equals("dailychecklistmaster", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        inspectionmaster.Visible = useAccessData.Where(ss => ss.Code.Equals("inspectionmaster", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        qualityincomingmaster.Visible = useAccessData.Where(ss => ss.Code.Equals("qualityincomingmaster", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        productionscheduler.Visible = useAccessData.Where(ss => ss.Code.Equals("productionscheduler", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        weeklychecklisttransaction.Visible = useAccessData.Where(ss => ss.Code.Equals("weeklychecklisttransaction", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        dailychecklistreport.Visible = useAccessData.Where(ss => ss.Code.Equals("dailychecklistreport", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        weeklychecklistreport.Visible = useAccessData.Where(ss => ss.Code.Equals("weeklychecklistreport", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        selfinspectionreport.Visible = useAccessData.Where(ss => ss.Code.Equals("selfinspectionreport", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        tracebilityreport.Visible = useAccessData.Where(ss => ss.Code.Equals("TracebilityReport", StringComparison.OrdinalIgnoreCase) & ss.Domain.Equals("GEA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        zclfgmdreport.Visible = useAccessData.Where(ss => ss.Code.Equals("ZCLFGMDReport", StringComparison.OrdinalIgnoreCase) & ss.Domain.Equals("GEA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();

                        geasuboperations.Visible = useAccessData.Where(ss => ss.Code.Equals("SubActivity", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        geamasterdata.Visible = useAccessData.Where(ss => ss.Code.Equals("SubActivityMaster", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        receiptcompletiontargetmastergea.Visible = useAccessData.Where(ss => ss.Code.Equals("ReceiptCompletionTargetMasterGEA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();

                        targetscreengea.Visible = useAccessData.Where(ss => ss.Code.Equals("OperatorTargetGEA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        geaautoschedulemaster.Visible = useAccessData.Where(ss => ss.Code.Equals("AssemblyScheduleMasterGEA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        geachildpartmaster.Visible = useAccessData.Where(ss => ss.Code.Equals("ChilePartMasterGEA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        traceabilitydashboardgea.Visible = useAccessData.Where(ss => ss.Code.Equals("TraceabilityDashboardGEA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        gea_materialtracking.Visible = useAccessData.Where(ss => ss.Code.Equals("MaterialTrackingEntryGEA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        // qualityincomingreport.Visible = useAccessData.Where(ss => ss.Code.Equals("qualityincomingreport", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();

                        if (gea.Visible)
                            GeaAndonSubMenu.Visible = true;
                        else
                            GeaAndonSubMenu.Visible = false;

                        if (geasuboperations.Visible || geamasterdata.Visible || targetscreengea.Visible || geaautoschedulemaster.Visible || receiptcompletiontargetmastergea.Visible)
                            GeaMasterSubMenu.Visible = true;
                        else
                            GeaMasterSubMenu.Visible = false;

                        if (dailychecklistmaster.Visible || weeklychecklisttransaction.Visible)
                            GeaMaintenanceSubMenu.Visible = true;
                        else
                            GeaMaintenanceSubMenu.Visible = false;

                        if (qualityincomingmaster.Visible || inspectionmaster.Visible)
                            GeaQualitySubMenu.Visible = true;
                        else
                            GeaQualitySubMenu.Visible = false;

                        if (productionscheduler.Visible)
                            GeaProductionSubMenu.Visible = true;
                        else
                            GeaProductionSubMenu.Visible = false;

                        if (geachildpartmaster.Visible || gea_materialtracking.Visible || traceabilitydashboardgea.Visible)
                            GeaProductionStoresMenu.Visible = true;
                        else
                            GeaProductionStoresMenu.Visible = false;

                        #endregion

                            #region Vulkan
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["VulkanPages"] != null)
                        {
                            if (System.Web.Configuration.WebConfigurationManager.AppSettings["VulkanPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                            {
                                liVulkanAndon.Visible = true;
                            }
                            else
                            {
                                liVulkanAndon.Visible = false;
                            }
                        }
                        else
                        {
                            liVulkanAndon.Visible = false;
                        }
                        #endregion

                        #region EnergyModulePages
                        energymoduledashboard.Visible = useAccessData.Where(ss => ss.Code.Equals("EMDashboard", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        powerprofile.Visible = useAccessData.Where(ss => ss.Code.Equals("EMPowerProfile", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        livedata.Visible = useAccessData.Where(ss => ss.Code.Equals("EMLive", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        energyreportpage.Visible = useAccessData.Where(ss => ss.Code.Equals("EMReports", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        ernergymodulesettings.Visible = useAccessData.Where(ss => ss.Code.Equals("EMSettings", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        liEnergyMachineMaster.Visible = useAccessData.Where(ss => ss.Code.Equals("EnerygyMachineMaster", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        liEnergyMachineNode.Visible = useAccessData.Where(ss => ss.Code.Equals("EnerygyMachineNodeMaster", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        if (energymoduledashboard.Visible || powerprofile.Visible || livedata.Visible || energyreportpage.Visible || ernergymodulesettings.Visible || liEnergyMachineMaster.Visible || liEnergyMachineNode.Visible)
                        {
                            if (System.Web.Configuration.WebConfigurationManager.AppSettings["EnergyModule"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                            {
                                EnergyModuleMainMenu.Visible = true;
                            }
                            else
                            {
                                EnergyModuleMainMenu.Visible = false;
                            }
                            //EnergyModuleMainMenu.Visible = true;
                        }
                        else
                        {
                            EnergyModuleMainMenu.Visible = false;
                        }

                        #endregion

                        #region 

                        //if (liEnergyMachineMaster.Visible || liEnergyMachineNode.Visible)
                        //{
                        //    liEnergyMaster.Visible = true;
                        //}
                        //else
                        //    liEnergyMaster.Visible = false;
                        #endregion

                        #region
                        if (ConfigurationManager.AppSettings["JagdevPage"].ToString() == "1")
                        {
                            jagdevANODN.Visible = true;
                        }
                        else
                        {
                            jagdevANODN.Visible = false;
                        }
                        #endregion

                        #region ShantiPages
                        shantiandon.Visible = useAccessData.Where(ss => ss.Code.Equals("ShantiAndon", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Shanti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        shantifinalinspection.Visible = useAccessData.Where(ss => ss.Code.Equals("FinalInspection", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Shanti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        SerialNumberDashboard.Visible = useAccessData.Where(ss => ss.Code.Equals("SlNoDashboard", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Shanti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        //InspectionMasterShanti.Visible = useAccessData.Where(ss => ss.Code.Equals("InspectionMaster", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Shanti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        ShanthiShiftEmployee.Visible = useAccessData.Where(ss => ss.Code.Equals("OprSupAllocation", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Shanti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        OperationBypass.Visible = useAccessData.Where(ss => ss.Code.Equals("CompOpBypass", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Shanti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        //shantialaramhistory.Visible = useAccessData.Where(ss => ss.Code.Equals("AlarmHistory", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Shanti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        DownTimeQualification.Visible = useAccessData.Where(ss => ss.Code.Equals("DownModification", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Shanti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        PMCheckListTransaction.Visible = useAccessData.Where(ss => ss.Code.Equals("PMTransaction", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Shanti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        PMSettings.Visible = useAccessData.Where(ss => ss.Code.Equals("ShantiPMMaster", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Shanti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        PackingStation.Visible = useAccessData.Where(ss => ss.Code.Equals("PackingStation", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Shanti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        CoolantTopUpMaster.Visible = useAccessData.Where(ss => ss.Code.Equals("CoolantTopUp", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Shanti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        QualityGate.Visible = useAccessData.Where(ss => ss.Code.Equals("QualityGate", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Shanti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        FPAandCMMData.Visible = useAccessData.Where(ss => ss.Code.Equals("FPACMMData", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Shanti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        MPIGate.Visible = useAccessData.Where(ss => ss.Code.Equals("MPIGate", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Shanti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();


                        #endregion

                        #region --- SKS -----
                        schedulemastersks.Visible = useAccessData.Where(ss => ss.Code.Equals("ScheduleMasterSKS", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("SKS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        proddownrejectiondata.Visible = useAccessData.Where(ss => ss.Code.Equals("ProdDownRejectERPSKS", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("SKS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        if (schedulemastersks.Visible || proddownrejectiondata.Visible)
                        {
                            SKSMainMenu.Visible = true;
                        }
                        else
                        {
                            SKSMainMenu.Visible = false;
                        }
                        #endregion

                        #region --- KTA -----
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            machineschedulemaster.Visible = useAccessData.Where(ss => ss.Code.Equals("ScheduleCompOpnMaster", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            plantcellemployeeassociationkta.Visible = useAccessData.Where(ss => ss.Code.Equals("CellEmployeeAssociationKTA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            partfamilymaster.Visible = useAccessData.Where(ss => ss.Code.Equals("PartFamilyMaster", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("KTA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            documentview.Visible = useAccessData.Where(ss => ss.Code.Equals("DocumentView", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("KTA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            updatecomponentstore.Visible = useAccessData.Where(ss => ss.Code.Equals("UpdateComponentStore", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("KTA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            inspectiontransaction.Visible = useAccessData.Where(ss => ss.Code.Equals("InspectionTransaction", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("KTA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            storeandonkta.Visible = useAccessData.Where(ss => ss.Code.Equals("StoreAndon", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("KTA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            computermachineassociationsettings.Visible = useAccessData.Where(ss => ss.Code.Equals("ComputerMachineAssociationSettings", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            shiftdetailskta.Visible = useAccessData.Where(ss => ss.Code.Equals("ShiftInformation", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("KTA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            machineshifttypeassociation.Visible = useAccessData.Where(ss => ss.Code.Equals("MachineShiftAssociation", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("KTA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();

                            if (machineschedulemaster.Visible || plantcellemployeeassociationkta.Visible || inspectionmasterkta.Visible || partfamilymaster.Visible || documentview.Visible || updatecomponentstore.Visible || inspectiontransaction.Visible || storeandonkta.Visible || computermachineassociationsettings.Visible || shiftdetailskta.Visible || machineshifttypeassociation.Visible)
                            {
                                KTAMainMenu.Visible = true;
                            }
                            else
                            {
                                KTAMainMenu.Visible = false;
                            }
                        }
                        else
                        {
                            KTAMainMenu.Visible = false;
                        }
                        #endregion

                        #region --- Advik 184 (Panth) -----
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["Advik184Pages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            jhchecklistmasteradvik184.Visible = useAccessData.Where(ss => ss.Code.Equals("JHMasterAdvik184", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Advik184", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            jhdashboardadvik184.Visible = useAccessData.Where(ss => ss.Code.Equals("JHDashboardAdvik184", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Advik184", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            finalinspectiondashboardadvik.Visible = useAccessData.Where(ss => ss.Code.Equals("FIDashboardAdvik184", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Advik184", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            finalinspectionreportadvik.Visible = useAccessData.Where(ss => ss.Code.Equals("FIReportAdvik184", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Advik184", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            plantstatusandon.Visible = useAccessData.Where(ss => ss.Code.Equals("PlantStatusAndonAdvik184", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Advik184", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            traceabilityreportadvik.Visible = useAccessData.Where(ss => ss.Code.Equals("TraceabilityAdvik184", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Advik184", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            parametermasteradvik.Visible = useAccessData.Where(ss => ss.Code.Equals("ParameterMasterAdvik184", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Advik184", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            settingadvik184.Visible = useAccessData.Where(ss => ss.Code.Equals("SettingAdvik184", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Advik184", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            pokayokemasteradvik184.Visible = useAccessData.Where(ss => ss.Code.Equals("SettingAdvik184", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Advik184", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            pokayokedashboardadvik184.Visible = useAccessData.Where(ss => ss.Code.Equals("SettingAdvik184", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Advik184", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            if (finalinspectiondashboardadvik.Visible || finalinspectionreportadvik.Visible || plantstatusandon.Visible || traceabilityreportadvik.Visible || parametermasteradvik.Visible || jhchecklistmasteradvik184.Visible || jhdashboardadvik184.Visible || settingadvik184.Visible)
                            {
                                Advik184MainMenu.Visible = true;
                            }
                            else
                            {
                                Advik184MainMenu.Visible = false;
                            }
                        }
                        else
                        {
                            Advik184MainMenu.Visible = false;
                        }
                        #endregion

                        #region --- Bajaj -----
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["BajajPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            //BajajJHMaster.Visible = true;
                            //BajajPPMaster.Visible = true;
                            BajajAnalyticsMainMenu.Visible = true;
                        }
                        else
                        {
                            //BajajJHMaster.Visible = false;
                            //BajajPPMaster.Visible = false;
                            BajajAnalyticsMainMenu.Visible = false;
                        }

                        #endregion

                        #region ----Pooja ---
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["PoojaPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            poojaANDON.Visible = true;
                        }
                        else
                        {
                            poojaANDON.Visible = false;
                        }
                        #endregion

                        #region --- Cumi  -----
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["CumiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            CumiAnalyticsMainMenu.Visible = true;
                            planentryscreen.Visible = useAccessData.Where(ss => ss.Code.Equals("PlanEntryScreen", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Cumi", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            processparametermasterscreen.Visible = useAccessData.Where(ss => ss.Code.Equals("ProcessParameterMasterScreen", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Cumi", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            ppmachineanalytics.Visible = useAccessData.Where(ss => ss.Code.Equals("ProcessParameterTrend", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Cumi", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            processparameterspc.Visible = useAccessData.Where(ss => ss.Code.Equals("EnergySPCTrend", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Cumi", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            lossreportcumi.Visible = useAccessData.Where(ss => ss.Code.Equals("LossReport", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Cumi", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            spctargetmastercumi.Visible = useAccessData.Where(ss => ss.Code.Equals("SPCTargetMaster", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Cumi", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            processparameterreport.Visible = useAccessData.Where(ss => ss.Code.Equals("ProcessParameterReport", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Cumi", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            postatus.Visible = useAccessData.Where(ss => ss.Code.Equals("POStatusReport", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Cumi", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            qualityrejectionreport.Visible = useAccessData.Where(ss => ss.Code.Equals("QualityReport", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Cumi", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            productparameterreportcumi.Visible = useAccessData.Where(ss => ss.Code.Equals("ProductParameterReport", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Cumi", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                        }
                        else
                        {
                            CumiAnalyticsMainMenu.Visible = false;
                        }

                        #endregion

                        #region --- MachineConnectPages  -----
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["MachineConnectPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            machineconnectsetting.Visible = true;
                            operatormessage.Visible = useAccessData.Where(ss => ss.Code.Equals("OperatorMessage", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("MachineConnectWeb", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            BajajFocasToolLife.Visible = useAccessData.Where(ss => ss.Code.Equals("FocasToolLife", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("MachineConnectWeb", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            spindleparameterdashboard.Visible = useAccessData.Where(ss => ss.Code.Equals("SpindleParameterDashboard", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("MachineConnectWeb", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            alarmhistory.Visible = useAccessData.Where(ss => ss.Code.Equals("AlarmHistory", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("MachineConnectWeb", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            productionanalytics.Visible = useAccessData.Where(ss => ss.Code.Equals("ProductionAnalytics", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("MachineConnectWeb", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            operationhistory.Visible = useAccessData.Where(ss => ss.Code.Equals("OperationHistory", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("MachineConnectWeb", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            wearoffsethistory.Visible = useAccessData.Where(ss => ss.Code.Equals("WearOffsetHistory", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("MachineConnectWeb", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            parameterdetailslog.Visible= useAccessData.Where(ss => ss.Code.Equals("ProgramChangesDashboard", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("MachineConnectWeb", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            programuploaddashboard.Visible = useAccessData.Where(ss => ss.Code.Equals("ProgramUploadHistory", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("MachineConnectWeb", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            //programtransferpage.Visible = useAccessData.Where(ss => ss.Code.Equals("ProgramTransfer", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("MachineConnectWeb", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            programtransferpage.Visible = false;
                            if (operatormessage.Visible || BajajFocasToolLife.Visible || spindleparameterdashboard.Visible || alarmhistory.Visible || productionanalytics.Visible || wearoffsethistory.Visible || operationhistory.Visible || programtransferpage.Visible || parameterdetailslog.Visible || programuploaddashboard.Visible)
                            {
                                MachineConnectMainMenu.Visible = true;
                            }
                            else
                            {
                                MachineConnectMainMenu.Visible = false;
                            }
                        }
                        else
                        {
                            MachineConnectMainMenu.Visible = false;
                            machineconnectsetting.Visible = false;
                        }
                        #endregion

                        #region --- PTA  -----
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["PTAPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            plantandmachineviewpta.Visible = useAccessData.Where(ss => ss.Code.Equals("PlantAndMachineViewPTA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            operatoreffyreportpta.Visible = useAccessData.Where(ss => ss.Code.Equals("OperatorEffyReportPTA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            unmanedreportpta.Visible = useAccessData.Where(ss => ss.Code.Equals("UnmanedReportPTA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            if (plantandmachineviewpta.Visible || operatoreffyreportpta.Visible || unmanedreportpta.Visible)
                            {
                                PTAMainMenu.Visible = true;
                            }
                            else
                            {
                                PTAMainMenu.Visible = false;
                            }
                        }
                        else
                        {
                            PTAMainMenu.Visible = false;
                        }
                        #endregion

                        #region --- Denso  -----
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["DensoPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            DensoMainMenu.Visible = true;
                            dailychecklistmasterdenso.Visible = useAccessData.Where(ss => ss.Code.Equals("DailyChecklistMasterDenso", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            dailycheckpointtransactiondenso.Visible = useAccessData.Where(ss => ss.Code.Equals("DailyCheckpointTransactionDenso", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            dailycheckpointreportdenso.Visible = useAccessData.Where(ss => ss.Code.Equals("DailyCheckPointReportDenso", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            staticaccuracymasterdenso.Visible = useAccessData.Where(ss => ss.Code.Equals("StaticAccuracyMasterDenso", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            staticaccuracytransactiondenso.Visible = useAccessData.Where(ss => ss.Code.Equals("StaticAccuracyTransactionDenso", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            staticaccuracyreportdenso.Visible = useAccessData.Where(ss => ss.Code.Equals("StaticAccuracyReportDenso", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            pokayokemasterdenso.Visible = useAccessData.Where(ss => ss.Code.Equals("PokayOkeMasterDenso", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            pokayoketransactiondenso.Visible = useAccessData.Where(ss => ss.Code.Equals("PokayOkeTransactionDenso", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            pokayokereportdenso.Visible = useAccessData.Where(ss => ss.Code.Equals("PokayOkeReportDenso", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            firoverviewdatadenso.Visible = useAccessData.Where(ss => ss.Code.Equals("FIROverviewDataDenso", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            fiveschecksheetmasterdenso.Visible = useAccessData.Where(ss => ss.Code.Equals("FiveSChecksheetMasterDenso", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            fiveschecksheettransdenso.Visible = useAccessData.Where(ss => ss.Code.Equals("FiveSChecksheetTransDenso", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            fiveschecksheetreportdenso.Visible = useAccessData.Where(ss => ss.Code.Equals("FiveSChecksheetReportDenso", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            if (dailychecklistmasterdenso.Visible || dailycheckpointtransactiondenso.Visible || dailycheckpointreportdenso.Visible || staticaccuracymasterdenso.Visible || staticaccuracytransactiondenso.Visible || staticaccuracyreportdenso.Visible || pokayokemasterdenso.Visible || pokayoketransactiondenso.Visible || pokayokereportdenso.Visible || firoverviewdatadenso.Visible || fiveschecksheetmasterdenso.Visible || fiveschecksheettransdenso.Visible || fiveschecksheetreportdenso.Visible)
                            {
                                DensoMainMenu.Visible = true;
                            }
                            else
                            {
                                DensoMainMenu.Visible = false;
                            }

                        }
                        else
                        {
                            DensoMainMenu.Visible = false;
                        }

                        #endregion

                        #region -- Pradeep Metals --
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["PradeepMetalPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {

                            schedulingscreenpradeepmetals.Visible = useAccessData.Where(ss => ss.Code.Equals("SchedulingScreenPradeepMetals", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("PM", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            cell_supervisorscreen_pradeepmetals.Visible = useAccessData.Where(ss => ss.Code.Equals("CellSupervisorAssoPradeepMetals", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("PM", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            pradeepmetalsreports.Visible = useAccessData.Where(ss => ss.Code.Equals("PradeepMetalsReports", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("PM", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            computercellassociation.Visible = useAccessData.Where(ss => ss.Code.Equals("ComputerCellAssociationPrdeepMetals", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("PM", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();

                            if (schedulingscreenpradeepmetals.Visible || cell_supervisorscreen_pradeepmetals.Visible || pradeepmetalsreports.Visible || computercellassociation.Visible)
                            {
                                PradeepMetalsMainMenu.Visible = true;
                            }
                            else
                            {
                                PradeepMetalsMainMenu.Visible = false;
                            }
                        }
                        else
                        {
                            PradeepMetalsMainMenu.Visible = false;
                        }
                        #endregion

                        #region ---- Pitti Engineering ----
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["PittiPages"].ToString() == "1")
                        {
                            dailychecklistmasterpitti.Visible = useAccessData.Where(ss => ss.Code.Equals("DailyChecklistMasterPitti", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Pitti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            //dailychecklistreportpitti.Visible = useAccessData.Where(ss => ss.Code.Equals("DailyChecklistReportPitti", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Pitti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            pmchecklistmasterpitti.Visible = useAccessData.Where(ss => ss.Code.Equals("PMChecklistMasterPitti", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Pitti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            //pmchecklistreportpitti.Visible = useAccessData.Where(ss => ss.Code.Equals("PMChecklistReportPitti", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Pitti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            workordertrackingpitti.Visible = useAccessData.Where(ss => ss.Code.Equals("WorkOrderTrackingDashboardPitti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            reports_pitti.Visible = useAccessData.Where(ss => ss.Code.Equals("Reports_Pitti", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Pitti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            if (dailychecklistmasterpitti.Visible || pmchecklistmasterpitti.Visible || workordertrackingpitti.Visible || reports_pitti.Visible)
                                PittiMainMenu.Visible = true;
                            else
                                PittiMainMenu.Visible = false;

                        }
                        else
                        {
                            PittiMainMenu.Visible = false;
                        }
                        #endregion

                        #region ---- Vulkan ----
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["VulkanPages"].ToString() == "1")
                        {
                            dailychecklistmastervulkan.Visible = useAccessData.Where(ss => ss.Code.Equals("DailyChecklistMasterVulkan", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Vulkan", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            pmmastervulkan.Visible = useAccessData.Where(ss => ss.Code.Equals("PMMasterVulkan", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Vulkan", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            methodinstrumentimagesuploadpage.Visible = useAccessData.Where(ss => ss.Code.Equals("MethodInstrumentImagesUploadPage", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Vulkan", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            planningsheet_vulkan.Visible = useAccessData.Where(ss => ss.Code.Equals("PlanningSheetVulkan", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Vulkan", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            pmreport_vulkan.Visible = useAccessData.Where(ss => ss.Code.Equals("PreventiveMaintenanceReportVulkan", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Vulkan", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            dailychecklisttransactionvulkan.Visible = useAccessData.Where(ss => ss.Code.Equals("DailyChecklistApprovalVulkan", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Vulkan", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            Session["UserRole"] = DataBaseAccess.getEmployeeRoleVulkanMS(Session["UserName"].ToString());
                            if (dailychecklistmastervulkan.Visible || pmmastervulkan.Visible || methodinstrumentimagesuploadpage.Visible || planningsheet_vulkan.Visible || pmreport_vulkan.Visible || dailychecklisttransactionvulkan.Visible)
                                VulkanMainMenu.Visible = true;
                            else
                                VulkanMainMenu.Visible = false;
                        }
                        else
                        {
                            VulkanMainMenu.Visible = false;
                        }
                        #endregion

                        #region --- Machine Shop Pages  -----
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["VulkanMachineShopPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            vulkanmachineshopandon.Visible = true;

                            plantcellemployeeassvulkan.Visible = useAccessData.Where(ss => ss.Code.Equals("EmployeeCellAssVulaknMachineShop", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            machineschedulemastervulkan.Visible = useAccessData.Where(ss => ss.Code.Equals("ScheduleCompOpnVulaknMachineShop", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            inspectionmastervulkan.Visible = useAccessData.Where(ss => ss.Code.Equals("InspectionMasterVulkanMachineShop", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            inspectiontransactionvulkan.Visible = useAccessData.Where(ss => ss.Code.Equals("InspectionTransactionVulkanMachineShop", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            vulkanpmmaster.Visible = useAccessData.Where(ss => ss.Code.Equals("PMMasterVulkanMachineShop", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            vulkanpmapprovaldashboard.Visible = useAccessData.Where(ss => ss.Code.Equals("PMApprovalDashboardVulkanMachineShop", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            toollifemastervulkan.Visible = useAccessData.Where(ss => ss.Code.Equals("ToolLifeMasterVulkanMachineShop", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            Session["UserRole"] = DataBaseAccess.getEmployeeRoleVulkanMS(Session["UserName"].ToString());
                            if (plantcellemployeeassvulkan.Visible || machineschedulemastervulkan.Visible || inspectionmastervulkan.Visible || inspectiontransactionvulkan.Visible || vulkanpmmaster.Visible || vulkanpmapprovaldashboard.Visible || vulkanmachineshopandon.Visible || toollifemastervulkan.Visible)
                            {
                                VulkanMSMainMenu.Visible = true;
                            }
                            else
                            {
                                VulkanMSMainMenu.Visible = false;
                            }
                        }
                        else
                        {
                            VulkanMSMainMenu.Visible = false;
                        }
                        #endregion

                        #region --- LnT Odisha ---
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["LnTOdishaPages"].ToString() == "1")
                        {
                            pmmasterlnt.Visible = useAccessData.Where(ss => ss.Code.Equals("PMMasterlnt", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("LnTOdisha", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            pmgenerationyearlysummarylnt.Visible = useAccessData.Where(ss => ss.Code.Equals("PMGenerationYearlySummarylnt", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("LnTOdisha", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            pmchecklistprintoutlnt.Visible = useAccessData.Where(ss => ss.Code.Equals("PMChecklistPrintOutlnt", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("LnTOdisha", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            pmtransactionlnt.Visible = useAccessData.Where(ss => ss.Code.Equals("PMTransactionlnt", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("LnTOdisha", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();

                            if (pmmasterlnt.Visible || pmgenerationyearlysummarylnt.Visible || pmchecklistprintoutlnt.Visible || pmtransactionlnt.Visible)
                                LnTOdishaMainMenu.Visible = true;
                        }
                        else
                        {
                            LnTOdishaMainMenu.Visible = false;
                        }

                        #endregion

                        #region --Precision Engg---
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["PrecisionEngPages"].ToString() == "1")
                        {
                            maintenancegroupmaster.Visible = useAccessData.Where(ss => ss.Code.Equals("MaintenanceGroupMaster", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Precision", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            dailychecksheetmaster.Visible = useAccessData.Where(ss => ss.Code.Equals("DailyMaintenanceCheckSheet", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Precision", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();

                            if (maintenancegroupmaster.Visible || dailychecksheetmaster.Visible)
                                liprecisionengg.Visible = true;
                        }
                        else
                            liprecisionengg.Visible = false;

                        #endregion

                        #region----Highway----
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["HighwayPages"].ToString() == "1")
                        {
                            machinestartupchecksheetmaster_highway.Visible = useAccessData.Where(ss => ss.Code.Equals("MachienChecksheetMaster", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Highway", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            machinestartupchecksheettransaction_highway.Visible = useAccessData.Where(ss => ss.Code.Equals("MachienChecksheetApproval", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Highway", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            inspectionreportofshaftmaster_highway.Visible = useAccessData.Where(ss => ss.Code.Equals("InspectionMaster", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Highway", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            inspectionreportofshafttransaction_highway.Visible = useAccessData.Where(ss => ss.Code.Equals("InspectionApproval", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Highway", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            serialnotrackingdetails_highway.Visible = useAccessData.Where(ss => ss.Code.Equals("TraceabilityDashboard", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Highway", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            inspectiontracking_highway.Visible = useAccessData.Where(ss => ss.Code.Equals("ScannedStatus", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Highway", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            andon_highway_new.Visible = useAccessData.Where(ss => ss.Code.Equals("HighwayAndon", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Highway", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            if (machinestartupchecksheetmaster_highway.Visible || machinestartupchecksheettransaction_highway.Visible || inspectionreportofshaftmaster_highway.Visible || inspectionreportofshafttransaction_highway.Visible|| andon_highway_new.Visible|| serialnotrackingdetails_highway.Visible|| inspectiontracking_highway.Visible)
                                HighwayMainMenu.Visible = true;
                        }
                        else
                        {
                            HighwayMainMenu.Visible = false;
                        }
                        #endregion
                        #region----Allied----
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["AlliedPages"].ToString() == "1")
                        {
                            amtransaction_allied.Visible = useAccessData.Where(ss => ss.Code.Equals("AM_Transaction", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Allied", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            ammaster_allied.Visible = useAccessData.Where(ss => ss.Code.Equals("AM_Master", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("Allied", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            if (amtransaction_allied.Visible || ammaster_allied.Visible)
                                AlliedMainMenu.Visible = true;
                        }
                        else
                        {
                            AlliedMainMenu.Visible = false;
                        }
                        #endregion


                        #region------VED------
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["VEDIndustries"].ToString() == "1")
                        {
                            amtransaction_ved.Visible = useAccessData.Where(ss => ss.Code.Equals("AM_Transaction", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("VED", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            ammaster_ved.Visible = useAccessData.Where(ss => ss.Code.Equals("AM_Master", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("VED", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();
                            schedulemaster_ved.Visible = useAccessData.Where(ss => ss.Code.Equals("VED_ScheduleScreen", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("VED", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();

                            andonved.Visible = useAccessData.Where(ss => ss.Code.Equals("AndonVed", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("VED", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();

                            if (amtransaction_ved.Visible || ammaster_ved.Visible || schedulemaster_ved.Visible || andonved.Visible)
                                VEDMainMenu.Visible = true;
                        }
                        else
                        {
                            VEDMainMenu.Visible = false;
                        }
                        #endregion

                        if (Session["AdminData"] != null && Session["AdminData"].ToString().Equals("Admin"))
                        {
                            useraccessrights.Visible = true;
                            settingpage.Visible = true;
                        }
                        else
                        {
                            settingpage.Visible = false;
                            useraccessrights.Visible = false;
                        }
                    }
                    showhide();
                }
                #endregion

                model = DataBaseAccess.ViewAppUISettings();
                if (!IsPostBack)
                {
                    //showhide();
                    // Set Anti-XSRF token
                    ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                    ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;

                    //const string imagesPath = "~/CompanyLogo/";// "~/Image/Slideshow/";
                    //var dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(imagesPath));
                    //Util.show16losses = DataBaseAccess.check16lossesshow();
                    ////filtering to jpgs, but ideally not required
                    //List<string> fileNames = (from flInfo in dir.GetFiles() select flInfo.Name).ToList();
                    //if (fileNames.Count > 0)
                    //{
                    //    Image1.ImageUrl = imagesPath + fileNames[0];
                    //}
                    //else
                    //{
                    //    Image1.ImageUrl = "Image/companyIcon.png";
                    //}
                    // Image1.ImageUrl = Util.getCompanyLogoPath();
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["CumiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        Image1.ImageUrl = "~/Cumi/Images/Murugappa_Group_Logo.png";
                        toggle.Src = "~/Cumi/Images/CumiLogo.png";
                        divAmitLog.Visible = true;

                        string backimg = "/Cumi/Images/CUMIBackground1.jpg";
                        //string backimg = "/TPMTrak_Web/Cumi/Images/CUMIBackground1.jpg";     // this is for using in IIS Hosting
                        dddd.Attributes.Add("style", "background-image: url(" + backimg + "); background-repeat: no-repeat; background-size: cover;; height: 200vh;");
                    }
                    else
                    {
                        Image1.ImageUrl = Util.getCompanyLogoPath();
                        toggle.Src = "~/Images/logo/AMITLogo.png";
                        divAmitLog.Visible = false;
                        //dddd.Attributes.Add("style", "background-color:  #202648");

                    }
                }
                else
                {
                    // Validate the Anti-XSRF token
                    if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                        || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                    {
                        throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                    }
                }
                Session["fontSize"] = model.FontSize;
                // c.Visible = false; // only for Sona
                processparameterconfig.Visible = false;//only for METSO
                                                       //MachineNodeInterface.Visible = true; //for SONA
                                                       //energyDashboard.Visible = false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        public void showhide()
        {
            try
            {
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["ShowProcessParameterDashboard"].ToString() == "1")
                {
                    processParameterDashboard.Visible = true;
                }
                else if (System.Web.Configuration.WebConfigurationManager.AppSettings["ShowProcessParameterDashboard"].ToString() == "0")
                {
                    processParameterDashboard.Visible = false;
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["IndiaNippon"].ToString() == "1")
                {
                    liNipponIndia.Visible = true;
                }
                else if (System.Web.Configuration.WebConfigurationManager.AppSettings["IndiaNippon"].ToString() == "0")
                {
                    liNipponIndia.Visible = false;
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["ShowProcessparameterconfig"].ToString() == "1")
                {
                    processparameterconfig.Visible = true;
                }
                else if (System.Web.Configuration.WebConfigurationManager.AppSettings["ShowProcessparameterconfig"].ToString() == "0")
                {
                    processparameterconfig.Visible = false;
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["ShowSpindleRuntimeChart"].ToString() == "1")
                {
                    SpindleRuntimeChart.Visible = true;
                }
                else if (System.Web.Configuration.WebConfigurationManager.AppSettings["ShowSpindleRuntimeChart"].ToString() == "0")
                {
                    SpindleRuntimeChart.Visible = false;
                }

                if (System.Web.Configuration.WebConfigurationManager.AppSettings["ShowEnergyDashboard"].ToString() == "1")
                {
                    energyDashboard.Visible = true;
                    EnergyScreens.Visible = true;
                }
                else if (System.Web.Configuration.WebConfigurationManager.AppSettings["ShowEnergyDashboard"].ToString() == "0")
                {
                    energyDashboard.Visible = false;
                    EnergyScreens.Visible = false;
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["Mangalreports"].ToString() == "1")
                {
                    specialreports.Visible = true;
                }
                else if (System.Web.Configuration.WebConfigurationManager.AppSettings["Mangalreports"].ToString() == "0")
                {
                    specialreports.Visible = false;
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["sonapages"].ToString() == "1")
                {
                    MachineNodeInterface.Visible = true;
                    MachineSubsystemDetails.Visible = true;
                    tpm2sap.Visible = true;
                    reworkDetails.Visible = true;
                    SonaAndon.Visible = true;
                    PartWeightPage.Visible = true;
                    LiveScreen.Visible = true;
                    Libreakdownlosses.Visible = true;
                }
                else if (System.Web.Configuration.WebConfigurationManager.AppSettings["sonapages"].ToString() == "0")
                {
                    MachineNodeInterface.Visible = false;
                    MachineSubsystemDetails.Visible = false;
                    reworkDetails.Visible = false;
                    tpm2sap.Visible = false;
                    SonaAndon.Visible = false;
                    LiveScreen.Visible = false;
                    PartWeightPage.Visible = false;
                    Libreakdownlosses.Visible = false;
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["BOSCHPages"].ToString() == "1")
                {
                    FlowMeterMasterPage.Visible = true;
                    FlowMeterDashboard.Visible = true;
                }
                else if (System.Web.Configuration.WebConfigurationManager.AppSettings["BOSCHPages"].ToString() == "0")
                {
                    FlowMeterMasterPage.Visible = false;
                    FlowMeterDashboard.Visible = false;
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["TAFEPages"].ToString() == "1")
                {
                    SuppCode.Visible = true;
                    TafeAnalyticsMenu.Visible = true;
                }
                else if (System.Web.Configuration.WebConfigurationManager.AppSettings["TAFEPages"].ToString() == "0")
                {
                    SuppCode.Visible = false;
                    TafeAnalyticsMenu.Visible = false;
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["VibrationPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    VibrationDashboard.Visible = true;
                    VibrationMasterPage.Visible = true;
                }
                else if (System.Web.Configuration.WebConfigurationManager.AppSettings["VibrationPages"].ToString().Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    VibrationDashboard.Visible = false;
                    VibrationMasterPage.Visible = false;
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["ShantiIronPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    ShantiAnalyticsMainMenu.Visible = true;

                }
                else
                {
                    // inspectionmastermenu.Visible = false;
                    ShantiAnalyticsMainMenu.Visible = false;
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["GEAPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    GeaAnalyticsMainMenu.Visible = true;
                }
                else
                {
                    GeaAnalyticsMainMenu.Visible = false;
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["AdvikPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    AdvikAnalyticsMainMenu.Visible = true;
                }
                else
                {
                    AdvikAnalyticsMainMenu.Visible = false;
                }

                if (System.Web.Configuration.WebConfigurationManager.AppSettings["MivinPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase) || System.Web.Configuration.WebConfigurationManager.AppSettings["RexnordPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    liInspectionMaster.Visible = true;
                }
                else
                {
                    liInspectionMaster.Visible = false;
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["MandMPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    liMahindraMahindra.Visible = true;
                }
                else
                {
                    liMahindraMahindra.Visible = false;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (ConfigurationManager.AppSettings["SPCWeb_Url"] != null)
                {
                    string SpcWeb_url = ConfigurationManager.AppSettings["SPCWeb_Url"].ToString();
                    if (!IsPostBack)
                    {
                        if (!string.IsNullOrEmpty(SpcWeb_url) && SpcWeb_url.Contains("http"))
                        {
                            if (Session["UserName"] != null && Session["Password"] != null)
                            {
                                string url = $"{SpcWeb_url}?key={Base64Encode(Session["UserName"].ToString())}**{Base64Encode(Session["Password"].ToString())}";
                                linkSPC.HRef = url;
                            }
                            else
                            {
                                linkSPC.HRef = SpcWeb_url;
                            }
                        }
                        else
                        {
                            linkSPC.HRef = "#";
                            Logger.WriteDebugLog("Invalid SPC module Url. Please check url in config file. Url must be a valid http url.");
                        }
                    }
                }
                else
                {
                    linkSPC.HRef = "#";
                    Logger.WriteDebugLog("AppSetting parameter SPCWeb_Url does not exists in app.config. Define SPC module Url in config file.");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog(ex.Message);
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = new byte[0];
            try
            {
                if (!string.IsNullOrEmpty(plainText))
                    plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = new byte[0];
            try
            {
                if (!string.IsNullOrEmpty(base64EncodedData))
                    base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}