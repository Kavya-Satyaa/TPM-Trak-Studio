using Elmah;
using OfficeOpenXml2;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.EnergyModule;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class ZCLFGMD_Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindFabricationNo();
                Binddata("");
            }
        }

        private void BindFabricationNo()
        {
            try
            {
                DataTable dt = GEADatabaseAccess.getFabricationNumber();
                List<string> List = dt.AsEnumerable().Select(x => x.Field<string>("FabricationNo")).Distinct().ToList();
                var builder = new System.Text.StringBuilder();
                if (List.Count > 0)
                {
                    for (int i = 0; i < List.Count; i++)
                    {
                        builder.Append(String.Format("<option value='{0}'>", List[i].ToString()));
                    }
                }
                dlFabNo.InnerHtml = builder.ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            lblFabNo.Text = "FabricationNo Number: "+ txtFabNo.Text;
            Binddata(txtFabNo.Text.ToString());
            txtFabNo.Text = string.Empty;
        }

        private void Binddata(string FabNo)
        {
            try
            {
                DataTable dt = GEADatabaseAccess.GetZCLFGMDReportDetails(FabNo);
                if(dt.Columns.Count>0 && dt!=null)
                {
                    lvZCLFGMDGridDetails.DataSource = dt;
                    lvZCLFGMDGridDetails.DataBind();
                    Session["ZCLFGMD"] = dt;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        static string appPath = HttpContext.Current.Server.MapPath("~/GEA/Reports");
        private string GetReportPath(string reportName)
        {
            string src;
            if (HttpContext.Current.Session["Language"] == null)
                src = Path.Combine(appPath, "ReportTemplates", reportName);
            else
            {
                if (HttpContext.Current.Session["Language"].ToString() != "en")
                    src = Path.Combine(appPath, "ReportTemplates-" + HttpContext.Current.Session["Language"].ToString() + "", reportName);
                else
                    src = Path.Combine(appPath, "ReportTemplates", reportName);
            }
            return src;
        }
        private string SafeFileName(string name)
        {
            StringBuilder str = new StringBuilder(name);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.ToString();
        }
        #region "Down Load File"
        private static void DownloadMultipleFile(string fileName, byte[] byteArray)
        {
            try
            {

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Path.GetFileName(fileName) + "\"");
                HttpContext.Current.Response.OutputStream.Write(byteArray, 0, byteArray.Length);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                //HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        #endregion
        
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string templatefile = string.Empty;
                string Filename = "ZCLFGMDReport.xlsx";
                string Source = string.Empty;
                Source = GetReportPath(Filename);
                string Template = string.Empty;
                Template = "ZCLFGMDReport" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("ZCLFGMD Report- \n " + Source);
                }
                if(Session["ZCLFGMD"] !=null)
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    ExcelWorksheet workSheet = Excel.Workbook.Worksheets[0];
                    DataTable dt = Session["ZCLFGMD"] as DataTable;
                    if (dt.Rows.Count > 0 && dt != null)
                    {
                        workSheet.Cells["B3"].Value = lblFabNo.Text.Split(':')[1];
                        int row = 5, col = 1;
                        foreach(DataRow item in dt.Rows)
                        {
                            col = 1;
                            workSheet.Cells[row,col].Value = item["Parameterdescription"];
                            col++;
                            workSheet.Cells[row, col].Value = item["ParameterValue"];
                            row++;
                        }
                    }
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
    }
}