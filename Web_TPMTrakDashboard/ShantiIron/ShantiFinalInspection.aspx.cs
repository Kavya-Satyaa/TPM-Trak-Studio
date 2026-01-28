using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.ShantiIron.Model;
using Web_TPMTrakDashboard.Models;
using System.Data;
using System.Text;
using System.IO;
using System.Net;

namespace Web_TPMTrakDashboard.ShantiIron
{
    public partial class ShantiFinalInspection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null)
            {
                Response.Redirect("~/SignIn.aspx", false);
            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static FIDatList getSlnoFIData(string slno, string plant, string cell)
        {
            FIDatList finallist = new FIDatList();
            List<FIDataDetails> flist;
            DataTable dt = ShantiDataBaseAccess.getFIDataForSlno(slno, plant, cell, out flist);
            List<FIDataList> list = BindDataToList(dt);
            finallist.FICheckList = list;
            finallist.FISlnoDetails = flist;
            HttpContext.Current.Session["ShantiFIData"] = finallist;
            return finallist;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static FIDatList getDateFIData(string date, string plant, string cell)
        {
            FIDatList finallist = new FIDatList();
            List<FIDataDetails> flist;
            DataTable dt = ShantiDataBaseAccess.getFIDataForDate(date, plant, cell, out flist);

            List<FIDataList> list = BindDataToList(dt);
           
            finallist.FICheckList = list;
            finallist.FISlnoDetails = flist;
            HttpContext.Current.Session["ShantiFIData"] = finallist;
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
                    string status = "";
                    status = dt.Rows[row]["status"].ToString();
                   if (dt.Rows[row]["PDIReportFileName"].ToString() != "" && dt.Rows[row]["PDIReportFileName"].ToString() != null)
                    {
                        if (status != "Approved" && status != "Rejected" && status != "Rework")
                        {
                            status = "Generated"; //File Generated
                        }
                    }
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
                    for (int i = 8; i < dt.Columns.Count; i++)
                    {
                        if (i >= 8)
                        {
                            data = new FIData();
                            // takecolumnValue = true;
                            data.Parameter = dt.Columns[i].ColumnName;
                            if (data.Parameter.IndexOf("op90", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                data.Value = dt.Rows[row][i].ToString();
                                data.Type = "chkRead";
                                data.ControlIDOrClass = "chkApprove";
                                if (status == "Approved" || status == "Rejected" || status == "Generated" || status == "Rework")
                                {
                                    data.Approval = "true";
                                }
                                else
                                {
                                    if (is80completed == true)
                                    {
                                        data.Approval = "false";
                                    }
                                    else
                                    {
                                        data.Approval = "true";
                                    }
                                }

                            }
                            else
                            {
                                data.Value = dt.Rows[row][i].ToString();
                                data.Type = "chkReadOnly";
                                data.ControlIDOrClass = "chkReadOnly";
                                if (data.Parameter.IndexOf("op80", StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    if (data.Value == "Green" || data.Value == "Red" || data.Value == "Nan")
                                    {
                                        is80completed = true;
                                    }
                                }
                            }
                        }
                        filist.Add(data);
                    }
                    //for date of inspection
                    data = new FIData();
                    data.Parameter = "Date of Inspection";
                    string inspectiondate = "";
                    if (!dt.Rows[row]["InspectionDate"].Equals(DBNull.Value) || dt.Rows[row]["InspectionDate"].ToString() != "")
                    {
                        inspectiondate = Convert.ToDateTime(dt.Rows[row]["InspectionDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    data.Value = inspectiondate;
                    data.Type = "lbl";
                    data.ControlIDOrClass = "lblDate";
                    filist.Add(data);

                    //for  checked by
                    data = new FIData();
                    data.Parameter = "Checked By";
                    data.Value = dt.Rows[row]["CheckedBy"].ToString();
                    data.Type = "lbl";
                    data.ControlIDOrClass = "lblCheckedBy";
                    filist.Add(data);

                    //for remarks
                    data = new FIData();
                    data.Parameter = "Remarks";
                    data.Value = dt.Rows[row]["Remarks"].ToString();
                    data.Type = "txt";
                    data.ControlIDOrClass = "txtRemarks";
                    if (status == "Approved" || status == "Rejected") //|| status == "Generated"
                    {
                        data.Approval = "true";
                    }
                    else
                    {
                        data.Approval = "false";
                    }
                    filist.Add(data);

                    //for PDIReport
                    data = new FIData();
                    data.Parameter = "PDI Report";
                    data.Value = dt.Rows[row]["PDIReportFileName"].ToString();
                    data.Type = "link";
                    data.ControlIDOrClass = "lblPDIreportname";
                    filist.Add(data);

                    fidata.Status = "";
                    if (status == "Approved" || status == "Rejected")
                    {
                        fidata.ApproveVisibility = "hidden";
                        fidata.RejectVisibility = "hidden";
                        fidata.SaveVisibility = "hidden";
                        fidata.ReworkVisibility = "hidden";
                        fidata.Status = status;
                    }
                    else
                    {
                        if (status == "Generated" || status=="Rework")
                        {
                            fidata.ApproveVisibility = "visible";
                            fidata.RejectVisibility = "visible";
                            fidata.SaveVisibility = "hidden";
                            fidata.ReworkVisibility = "visible";
                        }
                        else
                        {
                            fidata.ApproveVisibility = "hidden";
                            fidata.RejectVisibility = "hidden";
                            fidata.SaveVisibility = "visible";
                            fidata.ReworkVisibility = "hidden";
                        }
                        //if (is80completed == false)
                        //{
                        //    fidata.ApproveVisibility = "hidden";
                        //    fidata.RejectVisibility = "hidden";
                        //}
                        //else
                        //{
                        //    if (status == "Generated")
                        //    {
                        //        fidata.ApproveVisibility = "visible";
                        //        fidata.RejectVisibility = "visible";
                        //    }
                        //    else
                        //    {
                        //        fidata.ApproveVisibility = "hidden";
                        //        fidata.RejectVisibility = "hidden";
                        //    }
                        //}
                        //if (status == "Generated")
                        //{
                        //    fidata.SaveVisibility = "hidden";
                        //}
                        //else
                        //{
                        //    fidata.SaveVisibility = "visible";
                        //}  
                    }
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
        public static int saveFIData(string slno, string comp, string remarkvalue, string statusvalue, string chk90)
        {
            int result;
            if (HttpContext.Current.Session["UserName"] != null)
            {
                string operatorid = HttpContext.Current.Session["UserName"].ToString();
                result = ShantiDataBaseAccess.saveFinalInspcetionDetails(slno, comp, remarkvalue, statusvalue, operatorid, chk90);

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

        private static FIData insertFIdata(string parameter, string value, string type, string id)
        {
            FIData data = new FIData();
            data.Parameter = parameter;
            data.Value = value;
            data.Type = type;
            data.ControlIDOrClass = id;
            return data;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<string> getCellID(string plant)
        {
            List<string> result = ShantiDataBaseAccess.GetAllGroupIDsForFI(plant);
            return result;
        }
       static NetworkConnection nc = null;
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {

                string userame = System.Web.Configuration.WebConfigurationManager.AppSettings["UsernameForShantiFI"].ToString();
                string password = System.Web.Configuration.WebConfigurationManager.AppSettings["PasswordForShantiFI"].ToString();
                string reqFolderPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FolderPathForShantiFI"].ToString();
                if (!string.IsNullOrEmpty(userame) && !string.IsNullOrEmpty(password))
                {
                    try
                    {
                        nc = new NetworkConnection(reqFolderPath, new NetworkCredential(userame, password));
                        // networkConnection = true;
                        FIDatList listsss = (FIDatList)HttpContext.Current.Session["ShantiFIData"];
                        StringBuilder sb = new StringBuilder();
                        FIDatList finallist = new FIDatList();
                        List<FIDataDetails> flslnoIssueist;
                        DataTable dt = ShantiDataBaseAccess.getFIDataForSlno(hdnSlno.Value, hdnPlantID.Value, hdnCellID.Value, out flslnoIssueist);
                        List<FIDataList> filist = BindDataToList(dt);
                        if (filist.Count > 0)
                        {
                            string fileName = "FinalInspection_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".csv";
                            //Response.Clear();
                            //Response.ClearContent();
                            //Response.ContentType = "application/x-msexcel";
                            //Response.AddHeader("content-disposition", @"attachment; filename=FinalInspection.csv");
                            //Response.Write(sb.ToString());
                            for (int i = 0; i < filist.Count; i++)
                            {
                                if (filist[i].FIData[0].Value == hdnSlno.Value && filist[i].FIData[0].ComponentID == hdnCompID.Value)
                                {
                                    fileName = "FinalInspectionData_"+ filist[i].FIData[0].ComponentID + "_" + filist[i].FIData[0].Value + ".csv";
                                    string separetor = ",";
                                    for (int j = 0; j < filist[i].FIData.Count; j++)
                                    {
                                        if (j == (filist[i].FIData.Count - 1))
                                        {
                                            sb.Append(filist[i].FIData[j].Parameter);
                                        }
                                        else
                                        {
                                            sb.Append(filist[i].FIData[j].Parameter + separetor);
                                        }
                                    }
                                    sb.Append("\r\n");
                                    for (int j = 0; j < filist[i].FIData.Count; j++)
                                    {
                                        string value = filist[i].FIData[j].Value;
                                        if (filist[i].FIData[j].Type== "chkRead" || filist[i].FIData[j].Type == "chkReadOnly")
                                        {
                                            if(filist[i].FIData[j].Value== "Green")
                                            {
                                                value = "1";
                                            }
                                            else if (filist[i].FIData[j].Value == "Red")
                                            {
                                                value = "2";
                                            }
                                            else if (filist[i].FIData[j].Value == "Nan")
                                            {
                                                value = "3";
                                            }
                                            else
                                            {
                                                value = "";
                                            }
                                        }
                                        if (j == filist[i].FIData.Count - 1)
                                        {
                                            sb.Append(value);
                                        }
                                        else
                                        {
                                            sb.Append(value + separetor);
                                        }

                                    }
                                    break;
                                }

                            }
                            //Response.Write(sb.ToString());
                            //Response.Flush();
                            //Response.Close();
                            //Response.End();

                            //StreamWriter file = new StreamWriter(@"C:\ShantiExportFile\" + fileName + ".csv");
                            StreamWriter file = new StreamWriter(reqFolderPath + fileName);
                            file.WriteLine(sb.ToString());
                            file.Close();
                        }
                    }
                    catch (Exception exx)
                    {
                        //networkConnection = false;
                        if (nc != null)
                            nc.Dispose();
                        Logger.WriteErrorLog(exx.ToString());
                    }
                    finally
                    {
                        if (nc != null)
                            nc.Dispose();
                    }
                }


            }
            catch (Exception ex)
            {

            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string sendCSVFile(string slno,string componentid,string cell,string plant)
        {
            string result = "fail";
            try
            {
                string userame = "";
                string password = "";
                string reqFolderPath = "";
                string operationList = "";
                if (cell.Equals("Phantom",StringComparison.OrdinalIgnoreCase))
                {
                    userame = System.Web.Configuration.WebConfigurationManager.AppSettings["UsernameForShantiFIPhantom"].ToString();
                    password = System.Web.Configuration.WebConfigurationManager.AppSettings["PasswordForShantiFIPhantom"].ToString();
                    reqFolderPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FolderPathForShantiFIPhantom"].ToString();
                    operationList = "60";
                }
                else if (cell.Equals("Compact X", StringComparison.OrdinalIgnoreCase))
                {
                    userame = System.Web.Configuration.WebConfigurationManager.AppSettings["UsernameForShantiFICompactX"].ToString();
                    password = System.Web.Configuration.WebConfigurationManager.AppSettings["PasswordForShantiFICompactX"].ToString();
                    reqFolderPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FolderPathForShantiFICompactX"].ToString();
                    operationList = "20,30,40";
                }
               
                if (!string.IsNullOrEmpty(userame) && !string.IsNullOrEmpty(password))
                {
                    try
                    {
                        nc = new NetworkConnection(reqFolderPath, new NetworkCredential(userame, password));
                        result = "connectionSuc";
                        // networkConnection = true;
                        FIDatList listsss = (FIDatList)HttpContext.Current.Session["ShantiFIData"];
                        StringBuilder sb = new StringBuilder();
                        FIDatList finallist = new FIDatList();
                        List<FIDataDetails> flslnoIssueist;
                        DataTable dt = ShantiDataBaseAccess.getFIDataForSlno(slno, plant, cell, out flslnoIssueist);
                        List<FIDataList> filist = BindDataToList(dt);
                        if (filist.Count > 0)
                        {
                            string fileName = "FinalInspectionData_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".csv";
                            //Response.Clear();
                            //Response.ClearContent();
                            //Response.ContentType = "application/x-msexcel";
                            //Response.AddHeader("content-disposition", @"attachment; filename=FinalInspection.csv");
                            //Response.Write(sb.ToString());
                            string fileNameOpn = "FinalInspectionData_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".csv";
                            string separetor = ",";
                            for (int i = 0; i < filist.Count; i++)
                            {
                                if (filist[i].FIData[0].Value == slno && filist[i].FIData[0].ComponentID == componentid)
                                {
                                    result = "matched";
                                    fileName = "FinalInspectionData_" + filist[i].FIData[0].ComponentID + "_" + filist[i].FIData[0].Value + ".csv";
                                    fileNameOpn= "FinalInspectionOpnData_" + filist[i].FIData[0].ComponentID + "_" + filist[i].FIData[0].Value + ".csv";
                                    for (int j = 0; j < filist[i].FIData.Count; j++)
                                    {
                                        if (filist[i].FIData[j].ControlIDOrClass == "lblPDIreportname")
                                        {
                                            continue;
                                        }
                                        if (j == (filist[i].FIData.Count - 1))
                                        {
                                            sb.Append(filist[i].FIData[j].Parameter);
                                        }
                                        else
                                        {
                                            sb.Append(filist[i].FIData[j].Parameter + separetor);
                                        }
                                    }
                                    sb.Append("\r\n");
                                    for (int j = 0; j < filist[i].FIData.Count; j++)
                                    {
                                        string value = filist[i].FIData[j].Value;
                                        if (filist[i].FIData[j].ControlIDOrClass == "lblPDIreportname")
                                        {
                                            continue;
                                        }
                                        if (filist[i].FIData[j].Type == "chkRead" || filist[i].FIData[j].Type == "chkReadOnly")
                                        {
                                            if (filist[i].FIData[j].Value == "Green")
                                            {
                                                value = "1";
                                            }
                                            else if (filist[i].FIData[j].Value == "Red")
                                            {
                                                value = "2";
                                            }
                                            else if (filist[i].FIData[j].Value == "Nan")
                                            {
                                                value = "3";
                                            }
                                            else
                                            {
                                                value = "";
                                            }
                                        }
                                        if (j == filist[i].FIData.Count - 1)
                                        {
                                            sb.Append(value);
                                        }
                                        else
                                        {
                                            sb.Append(value + separetor);
                                        }

                                    }
                                    break;
                                }

                            }
                            //Response.Write(sb.ToString());
                            //Response.Flush();
                            //Response.Close();
                            //Response.End();
                            //StreamWriter file = new StreamWriter(@"C:\ShantiExportFile\" + fileName + ".csv");
                            StreamWriter file = new StreamWriter(reqFolderPath + fileName);
                            file.WriteLine(sb.ToString());
                            file.Close();

                            StreamWriter operationFile = new StreamWriter(reqFolderPath + fileNameOpn);
                            DataTable operationTbl= ShantiDataBaseAccess.getSpcOperationData(operationList,componentid,slno);
                            StringBuilder operationSB = new StringBuilder();
                            operationSB.Append("Operation No,Component ID,Characteristic ID,Characteristic Code,LSL,USL,Unit,Value,Time Stamp,Specification Mean\r\n");
                            foreach (DataRow row in operationTbl.Rows)
                            {
                                operationSB.Append(row["OperationNo"].ToString() + separetor);
                                operationSB.Append(row["Componentid"].ToString() + separetor);
                                operationSB.Append(row["CharacteristicID"].ToString() + separetor);
                                operationSB.Append(row["CharacteristicCode"].ToString() + separetor);
                                operationSB.Append(row["LSL"].ToString() + separetor);
                                operationSB.Append(row["USL"].ToString() + separetor);
                                operationSB.Append(row["Unit"].ToString() + separetor);
                                operationSB.Append(row["Value"].ToString() + separetor);
                                operationSB.Append(row["Timestamp"].ToString() + separetor);
                                operationSB.Append(row["SpecificationMean"].ToString() + separetor);
                                operationSB.Append("\r\n");
                            }
                            operationFile.WriteLine(operationSB.ToString());
                            operationFile.Close();
                            Logger.WriteDebugLog("CSV File Download Result = "+result);
                        }
                    }
                    catch (Exception exx)
                    {
                        //networkConnection = false;
                        if (nc != null)
                            nc.Dispose();
                        Logger.WriteErrorLog(exx.ToString());
                    }
                    finally
                    {
                        if (nc != null)
                            nc.Dispose();
                    }
                }


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return result;
        }
        static string appPath = HttpContext.Current.Server.MapPath("");
        public static string GetReportPath(string reportName)
        {
            string src;
            src = Path.Combine(appPath, "PackingStationFiles", reportName);
            return src;
        }
        protected void btnExportFile_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = hdnFilenName.Value;
                if (filename != string.Empty)
                {
                    string filePath = GetReportPath(filename);
                    if (!File.Exists(filePath))
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "warning", "openWarningModal('File not found.')", true);
                        return;
                    }

                    WebClient req = new WebClient();
                    HttpResponse response = HttpContext.Current.Response;
                    response.Clear();
                    response.ClearContent();
                    response.ClearHeaders();
                    response.Buffer = true;
                    response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    //string path = Server.MapPath(filePath);
                    byte[] data = req.DownloadData(filePath);
                    response.BinaryWrite(data);
                    response.End();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static bool getFileGeneratedStatus(string slno,string componentid)
        {
           bool isFileGenerated = ShantiDataBaseAccess.isFinalInspectionFileGenerated(slno,componentid);
            return isFileGenerated;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string getScannedSlnoFromTbl()
        {
            string partnumber;
            string slno = ShantiDataBaseAccess.getScannedSlnoFromTable("FinalInspection",out partnumber);
            return slno;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static int setSlnoStatus(string slno)
        {
            int result = ShantiDataBaseAccess.updateSlnoStatus("FinalInspection", slno);
            return result;
        }
    }
}