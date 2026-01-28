using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.VulkanMachineShop.Model;

namespace Web_TPMTrakDashboard.VulkanMachineShop
{
    public partial class VulkanMachineShopAndon : System.Web.UI.Page
    {
        int Count = 0, Rows, Flips = 0, Interval = 0, RowsToTake = 0;
        public static int refreshInterval = Convert.ToInt32(WebConfigurationManager.AppSettings["VulkanMachineShopAndonInterval"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["VulkanAndon"] = this;
            if (!IsPostBack)
            {
                Session["MachineFlips"] = null;
                Session["MachineShopAndonDataVulkan"] = null;
                Session["Rows"] = null;
                Session["NoOfSkipRows"] = null;
                BindMachineData();

                machineInterval.Interval = refreshInterval;
                machineInterval.Enabled = true;
            }
        }

        private void BindMachineData()
        {
            try
            {
                List<MachineAndonData> list = new List<MachineAndonData>();
                list = VulkanMSDBAccess.GetMachineAndonData("", "", "");
                if (list.Count > 0)
                {
                    list.AsEnumerable().OrderBy(x => x.ParametersData.OrderBy(y => y.SortOrder));
                    lvMachineData.DataSource = list;
                    lvMachineData.DataBind();
                    Session["MachineShopAndonDataVulkan"] = list;
                    Session["RowsToTake"] = 1;
                    Session["Flips"] = Session["Rows"] = list.Count;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SetFlipInterval", "SetFlipIntervalBoxWidth();", true);
                    lblRefreshTime.Text = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindMachineData: {ex}");
            }
        }

        protected void machineInterval_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Session["MachineFlips"] == null)
                {
                    BindMachineData();
                    machineInterval.Enabled = false;
                    machineInterval.Interval = refreshInterval;
                    machineInterval.Enabled = true;
                    return;
                }
                int.TryParse(Session["MachineFlips"].ToString(), out Flips);
                if (Flips > 1)
                {
                    int.TryParse(Session["Rows"].ToString(), out Rows);
                    int.TryParse(Session["RowsToTake"].ToString(), out RowsToTake);
                    List<MachineAndonData> list = (List<MachineAndonData>)Session["MachineShopAndonDataVulkan"];
                    IEnumerable<MachineAndonData> data = list.Skip(Rows * RowsToTake).Take(Rows);
                    lvMachineData.DataSource = data;
                    lvMachineData.DataBind();
                    Flips--;
                    RowsToTake++;
                    Session["MachineFlips"] = Flips;
                    Session["RowsToTake"] = RowsToTake;
                    lblRefreshTime.Text = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
                }
                else
                {
                    BindMachineData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"machineInterval_Tick: {ex}");
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