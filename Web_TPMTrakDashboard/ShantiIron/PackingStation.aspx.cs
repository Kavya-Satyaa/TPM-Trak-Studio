using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.ShantiIron.Model;
using Web_TPMTrakDashboard.Models;
using System.Data;
using System.Net;
using System.IO;

namespace Web_TPMTrakDashboard.ShantiIron
{
    public partial class PackingStation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null)
            {
                Response.Redirect("~/SignIn.aspx", false);
            }
            if (!IsPostBack)
            {
                txtDateSearch.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }

        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static FIDatList getSlnoPSData(string slno, string plant, string cell)
        {
            FIDatList finallist = new FIDatList();
            List<FIDataDetails> flist;
            DataTable dt = ShantiDataBaseAccess.getPSDataForSlno(slno, plant, cell, out flist);
            List<FIDataList> list = BindDataToList(dt);
            finallist.FICheckList = list;
           // finallist.FISlnoDetails = flist;
            return finallist;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static FIDatList getDatePSData(string date, string plant, string cell)
        {
            DateTime date1 = DateTime.Now;
            if (date != "")
            {
                //date = Convert.ToDateTime(input);
                date1 = Util.GetDateTime(date + " 11:00:00");
            }
            date = VDGDataBaseAccess.GetLogicalDayStart(date1.ToString("yyyy-MM-dd"));
            FIDatList finallist = new FIDatList();
            List<FIDataDetails> flist;
            DataTable dt = ShantiDataBaseAccess.getPSDataForDate(date, plant, cell, out flist);

            List<FIDataList> list = BindDataToList(dt);
            finallist.FICheckList = list;
            //finallist.FISlnoDetails = flist;
            return finallist;
        }
        public static List<FIDataList> BindDataToList(DataTable dt)
        {
            List<FIDataList> list = new List<FIDataList>();
            try
            {
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    FIDataList fidata = new FIDataList();
                    List<FIData> filist = new List<FIData>();
                    //string status = "";
                    //status = dt.Rows[row]["status"].ToString();
                    FIData data = null;
                    //  bool takecolumnValue = false;

                    //for Serial number
                    data = new FIData();

                    data.Parameter = "Serial Number";
                    data.Value = dt.Rows[row]["compslno"].ToString();
                    data.Type = "lbl";
                    data.ControlIDOrClass = "lblSlno";
                    data.ComponentID = dt.Rows[row]["ComponentID"].ToString();
                    filist.Add(data);

                    //for comp id
                    data = new FIData();
                    data.Parameter = "Component ID";
                    data.Value = dt.Rows[row]["ComponentID"].ToString();
                    data.Type = "lbl";
                    data.ControlIDOrClass = "lblComponentID";
                    data.ComponentID = dt.Rows[row]["ComponentID"].ToString();
                    filist.Add(data);

                    bool is80completed = false;
                    // for checklist
                    FIData op100Data = null; 
                    for (int i = 8; i < dt.Columns.Count; i++)
                    {
                        if (i >= 8)
                        {
                            data = new FIData();
                            // takecolumnValue = true;
                            data.Parameter = dt.Columns[i].ColumnName;
                            data.Value = dt.Rows[row][i].ToString();
                            data.Type = "chkReadOnly";
                            //data.ControlIDOrClass = "chkReadOnly";
                        }
                        if (data.Parameter.IndexOf("op100", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            op100Data= new FIData();
                            op100Data = data;
                        }
                        else
                        {
                            filist.Add(data);
                        }
                           
                    }
                    if (op100Data != null)
                    {
                        filist.Add(op100Data);
                    }

                    ////for date of inspection
                    //data = new FIData();
                    //data.Parameter = "Date of Inspection";
                    //string inspectiondate = "";
                    //if (!dt.Rows[row]["InspectionDate"].Equals(DBNull.Value) || dt.Rows[row]["InspectionDate"].ToString() != "")
                    //{
                    //    inspectiondate = Convert.ToDateTime(dt.Rows[row]["InspectionDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    //}
                    //data.Value = inspectiondate;
                    //data.Type = "lbl";
                    //data.ControlIDOrClass = "lblDate";
                    //filist.Add(data);

                    //for Batch Report
                    data = new FIData();
                    data.Parameter = "Batch Report";
                    data.Value = dt.Rows[row]["BatchReportName"].ToString();
                    data.Type = "link";
                    data.ControlIDOrClass = "lblbatchreportname";
                    filist.Add(data);

                    //for  checked by
                    data = new FIData();
                    data.Parameter = "PDI Report";
                    data.Value = dt.Rows[row]["PDIReportName"].ToString();
                    data.Type = "link";
                    data.ControlIDOrClass = "lblPDIreportname";
                    filist.Add(data);

                    ////for remarks
                    //data = new FIData();
                    //data.Parameter = "Remarks";
                    //data.Value = dt.Rows[row]["Remarks"].ToString();
                    //data.Type = "txt";
                    //data.ControlIDOrClass = "txtRemarks";
                    //filist.Add(data);

                    fidata.FIData = filist;
                    list.Add(fidata);
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string getCuurentDate()
        {
            string curentDate = DateTime.Now.ToString("yyyy-MM-dd");
            return curentDate;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static int savePackingStationData(string slno, string comp, string remarkvalue, string statusvalue)
        {
            int result;
            if (HttpContext.Current.Session["UserName"] != null)
            {
                string operatorid = HttpContext.Current.Session["UserName"].ToString();
                result = ShantiDataBaseAccess.savePackingStationDetails(slno, comp, remarkvalue, statusvalue, operatorid);

            }
            else
            {
                result = 0;
            }
            return result;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<string> getPlantData()
        {
            List<string> result = DataBaseAccess.GetAllPlants();
            result.RemoveAt(0);
            return result;
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<string> getCellID(string plant)
        {
            List<string> result = ShantiDataBaseAccess.GetAllGroupIDsForFI(plant);
            return result;
        }
        static string appPath = HttpContext.Current.Server.MapPath("");
        public static string GetReportPath(string reportName)
        {
            string src;
            src = Path.Combine(appPath, "PackingStationFiles", reportName);
            return src;
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = hdnFilenName.Value;
                if (filename != string.Empty)
                {
                    string filePath=GetReportPath(filename);
                    if (!File.Exists(filePath))
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('File not found.')", true);
                        return;
                    }
                     
                    WebClient req = new WebClient();
                    HttpResponse response = HttpContext.Current.Response;
                   // string filePath = "/ShantiIron/PackingStationFiles/" + filename;
                    response.Clear();
                    response.ClearContent();
                    response.ClearHeaders();
                    response.Buffer = true;
                    response.AddHeader("Content-Disposition", "attachment;filename="+ filename);
                    //string path = Server.MapPath(filePath);
                    byte[] data = req.DownloadData(filePath);
                    response.BinaryWrite(data);
                    response.End();
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string getScannedSlnoFromTbl()
        {
            string partnumber;
            string slno = ShantiDataBaseAccess.getScannedSlnoFromTable("PackingStation",out partnumber);
            return slno;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static int setSlnoStatus(string slno)
        {
            int result = ShantiDataBaseAccess.updateSlnoStatus("PackingStation", slno);
            return result;
        }
    }
}