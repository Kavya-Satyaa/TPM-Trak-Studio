using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Pooja.Model;

namespace Web_TPMTrakDashboard.Pooja
{
    public partial class PoojaANDON : System.Web.UI.Page
    {
        public AndonSettingDetails settings = null;
        public static int refreshInterval = 0;
        int rows = 0;
        int flips = 0;
        int rowsToTake = 0;

        protected void Page_Init(object sender, EventArgs e)
        {

            settings = GetANDONSettingsDetails(true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Image1.ImageUrl = Util.getCompanyLogoPath();
                BindPlant();
                BindCell();
            }
        }

        private void BindProductionDetails()
        {
            try
            {
                settings = GetANDONSettingsDetails(false);


                DateTime currentDateTime = DateTime.Now;
                var currentShiftVals = Web_TPMTrakDashboard.Models.DataBaseAccess.GetCurrentShiftVals("[s_GetCurrentShiftTime]");
                string shift = currentShiftVals[2];
                lblDate.InnerText = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                lblShift.InnerText = shift;

                List<ProductionDetails> list = PoojaDBAccess.GetANDONDetails(currentDateTime, shift, ddlPlant.SelectedValue, ddlCell.SelectedValue);

                lvProductionDetails.DataSource = list;
                lvProductionDetails.DataBind();

                HtmlControl control = (lvProductionDetails.FindControl("thMachineHeaderName") as HtmlControl);
                (lvProductionDetails.FindControl("thMachineHeaderName") as HtmlTableCell).InnerText = settings.MachineHeaderName;
                control = (lvProductionDetails.FindControl("thComponentAndOpnHeaderName") as HtmlControl);
                (lvProductionDetails.FindControl("thComponentAndOpnHeaderName") as HtmlTableCell).InnerText = settings.ComponentAndOpnHeaderName;
                control = (lvProductionDetails.FindControl("thOEEHeaderName") as HtmlControl);
                (lvProductionDetails.FindControl("thOEEHeaderName") as HtmlTableCell).InnerText = settings.OEEHeaderName;
                control = (lvProductionDetails.FindControl("thDownTimeHeaderName") as HtmlControl);
                (lvProductionDetails.FindControl("thDownTimeHeaderName") as HtmlTableCell).InnerText = settings.DownTimeHeaderName;
                control = (lvProductionDetails.FindControl("thOperatorHeaderName") as HtmlControl);
                (lvProductionDetails.FindControl("thOperatorHeaderName") as HtmlTableCell).InnerText = settings.OperatorHeaderName;
                control = (lvProductionDetails.FindControl("thProductionTargetHeaderName") as HtmlControl);
                (lvProductionDetails.FindControl("thProductionTargetHeaderName") as HtmlTableCell).InnerText = settings.ProductionTargetHeaderName;
                control = (lvProductionDetails.FindControl("thActualHeaderName") as HtmlControl);
                (lvProductionDetails.FindControl("thActualHeaderName") as HtmlTableCell).InnerText = settings.ActualHeaderName;
                control = (lvProductionDetails.FindControl("thStatusHeaderName") as HtmlControl);
                (lvProductionDetails.FindControl("thStatusHeaderName") as HtmlTableCell).InnerText = settings.StatusHeaderName;


                if (list.Count > 0)
                {
                    rows = flips = list.Count;
                    rowsToTake = 1;
                    Session["Flips"] = flips;
                    Session["PoojaProductionDetails"] = list;
                    Session["RowsToTake"] = rowsToTake;
                    Session["Rows"] = rows;
                    rowsToTake++;
                }
                flipInterval.Enabled = true;
                flipInterval.Interval = settings.ScreenFlipInterval * 1000;


                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetFlipInterval", "SetFlipInterval();", true);
                //if (settings.NoOfMachinesToDisplay <= 0)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "SetFlipInterval", "SetFlipInterval();", true);
                //}
                //else
                //{
                //    HttpContext.Current.Session["Rows"] = settings.NoOfMachinesToDisplay;
                //    int flips = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(HttpContext.Current.Session["Flips"]) / settings.NoOfMachinesToDisplay));
                //    HttpContext.Current.Session["Flips"] = flips;
                //}

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindProductionDetails: " + ex.Message);
            }
        }

        protected void flipInterval_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Session["Flips"] == null)
                {
                    BindProductionDetails();

                }
                int.TryParse(Session["Flips"].ToString(), out flips);
                if (flips > 1)
                {
                    int.TryParse(Session["RowsToTake"].ToString(), out rowsToTake);
                    int.TryParse(Session["Rows"].ToString(), out rows);
                    List<ProductionDetails> list = (List<ProductionDetails>)Session["PoojaProductionDetails"];
                    int skipRows = rows * rowsToTake;
                    lvProductionDetails.DataSource = list.Skip(skipRows).Take(rows);
                    lvProductionDetails.DataBind();
                    flips--;
                    rowsToTake++;
                    Session["Flips"] = flips;
                    Session["RowsToTake"] = rowsToTake;
                }
                else
                {
                    BindProductionDetails();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("flipInterval_Tick = " + ex.Message);
            }
        }
        private static AndonSettingDetails GetANDONSettingsDetails(bool sessionMakeNull)
        {
            AndonSettingDetails settings = null;
            try
            {
                if (sessionMakeNull)
                {
                    HttpContext.Current.Session["ANDONSettingsDetails"] = null;
                }
                if (HttpContext.Current.Session["ANDONSettingsDetails"] == null)
                {
                    HttpContext.Current.Session["ANDONSettingsDetails"] = settings = PoojaDBAccess.GetANDONSettings();
                }
                else
                {
                    settings = (AndonSettingDetails)HttpContext.Current.Session["ANDONSettingsDetails"];
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetANDONSettingsDetails: " + ex.Message);
            }
            return settings;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void SetFlipIntervalToSession(int displayRowsCount)
        {
            try
            {
                //AndonSettingDetails settings = GetANDONSettingsDetails(false);
                //if (settings.NoOfMachinesToDisplay <= 0)
                //{
                HttpContext.Current.Session["Rows"] = displayRowsCount;
                int flips = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(HttpContext.Current.Session["Flips"]) / displayRowsCount));
                HttpContext.Current.Session["Flips"] = flips;
                //}
                //else
                //{
                //    HttpContext.Current.Session["Rows"] = settings.NoOfMachinesToDisplay;
                //    int flips = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(HttpContext.Current.Session["Flips"]) / settings.NoOfMachinesToDisplay));
                //    HttpContext.Current.Session["Flips"] = flips;
                //}

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SetFlipIntervalToSession (Pooja) : " + ex.Message);
            }
        }

        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindPlant();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlPlant_SelectedIndexChanged : " + ex.Message);
            }
        }

        protected void ddlCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindProductionDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlCell_SelectedIndexChanged : " + ex.Message);
            }
        }
        private void BindPlant()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlantsForPlantInfo();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();

                BindCell();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPlant: " + ex.Message);
            }
        }
        private void BindCell()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllGroupId(ddlPlant.SelectedValue);
                ddlCell.DataSource = list;
                ddlCell.DataBind();

                BindProductionDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindCell: " + ex.Message);
            }
        }
    }
}