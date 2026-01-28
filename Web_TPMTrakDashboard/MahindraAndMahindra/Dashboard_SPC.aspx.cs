using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.MahindraAndMahindra
{
    public partial class Dashboard_SPC : System.Web.UI.Page
    {
        public static ObservableCollection<DTO> processParamDashboardData = null;
        public static string machineId = string.Empty;
        string FileName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["prm"] != null)
            {
                string mid = Request.QueryString["prm"].ToString();
                char[] data = mid.ToCharArray();
                Base64Decoder myDecoder = new Base64Decoder(data);
                StringBuilder sb = new StringBuilder();
                byte[] temp = myDecoder.GetDecoded();
                sb.Append(Encoding.UTF8.GetChars(temp));
                machineId = sb.ToString();
            }
            else
            {
                machineId = ConfigurationManager.AppSettings["MachineID"] != null ? ConfigurationManager.AppSettings["MachineID"].ToString() : "";
            }
            if (!IsPostBack)
            {
                BindProcessParamDashboard(machineId);
                titleHeading.Text = machineId;
            }
        }

        public void BindProcessParamDashboard(string SelectedMachine)
        {
            FileName = Path.Combine(Server.MapPath(@"\Data"), DateTime.Now.ToString("yyyy-MM-dd") + ".json");
            if (File.Exists(FileName))
            {
                var result = File.ReadAllText(FileName);
                ProcessParameterData procParamData = JsonConvert.DeserializeObject<ProcessParameterData>(result);
                //processParamDashboardData = new List<DTO>();
                //processParamDashboardData = procParamData.lstProcessParameters;
                //if (processParamDashboardData != null && processParamDashboardData.Count > 0)
                //{
                //    listViewProcessParams.DataSource = processParamDashboardData;
                //    listViewProcessParams.DataBind();
                //}
            }
            else
            {
                var directory = new DirectoryInfo(Server.MapPath(@"\Data"));
                if (directory.Exists)
                {
                    if (directory.GetFiles().Length > 0)
                    {
                        FileName = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First().FullName;
                        var result = File.ReadAllText(FileName);
                        ProcessParameterData procParamData = JsonConvert.DeserializeObject<ProcessParameterData>(result);
                        //processParamDashboardData = new List<DTO>();
                        //processParamDashboardData = procParamData.lstProcessParameters;
                        //if (processParamDashboardData != null && processParamDashboardData.Count > 0)
                        //{
                        //    listViewProcessParams.DataSource = processParamDashboardData;
                        //    listViewProcessParams.DataBind();
                        //}
                    }
                }
            }
        }

        protected void UpdateTimer_Tick(object sender, EventArgs e)
        {
            BindProcessParamDashboard(machineId);
        }
    }
}