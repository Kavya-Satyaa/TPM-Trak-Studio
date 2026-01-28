using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Vulkan.Models;

namespace Web_TPMTrakDashboard.Vulkan
{
    public partial class MachineAndon : System.Web.UI.Page
    {
        int count = 0, noOfRows, flips = 0, Interval = 0, rowsToTake = 0;
        public static int refreshInterval = Convert.ToInt32(WebConfigurationManager.AppSettings["VulkanMachineAndonInterval"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["MachineFlips"] = null;
                Session["MachineAndonData"] = null;
                Session["Rows"] = null;
                Session["NoOfSkipRows"] = null;
                BindMachineData();

                machineInteral.Interval = refreshInterval;
                machineInteral.Enabled = true;
            }
        }
        private void BindMachineData()
        {
            try
            {
              
                List<MachineAndonData> list = DataBaseAccessVulkan.getMachineAndonData();
                if (list.Count > 0)
                {
                    lvMachineData.DataSource = list;
                    lvMachineData.DataBind();
                    //Session["POFlips"] = flips;
                    Session["MachineAndonData"] = list;
                    //Session["Rows"] = noOfRows;
                    Session["NoOfSkipRows"] = 1;
                    ScriptManager.RegisterStartupScript(this, GetType(), "bindNoOfRows", "setBoxBasedOnHeight();", true);
                    lblRefreshTime.Text = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog($"BindMachineData: {ex}");
            }
        }

        protected void machineInteral_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Session["MachineFlips"] == null)
                {
                    BindMachineData();
                    machineInteral.Enabled = false;
                    machineInteral.Interval = refreshInterval;
                    machineInteral.Enabled = true;
                    return;
                }
                int.TryParse(Session["MachineFlips"].ToString(), out flips);
                if (flips > 1)
                {
                    int.TryParse(Session["Rows"].ToString(), out noOfRows);
                    int.TryParse(Session["NoOfSkipRows"].ToString(), out rowsToTake);
                    List<MachineAndonData>  list = (List<MachineAndonData>)Session["MachineAndonData"];
                    IEnumerable<MachineAndonData> data = list.Skip(noOfRows * rowsToTake).Take(noOfRows);
                    lvMachineData.DataSource = data;
                    lvMachineData.DataBind();
                    flips--;
                    rowsToTake++;
                    Session["MachineFlips"] = flips;
                    Session["NoOfSkipRows"] = rowsToTake;
                    lblRefreshTime.Text = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
                }
                else
                {
                    BindMachineData();
                }
            }
            catch (Exception ex)
            {

            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void setNoOfDivsToSession(int divs, int divsLength)
        {
            HttpContext.Current.Session["Rows"] = divs;
            int flips = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(divsLength) / divs));
            HttpContext.Current.Session["MachineFlips"] = flips;
        }
    }
}