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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class TracebilityReport : System.Web.UI.Page
    {
        private System.Data.DataTable NewDatatable = new DataTable();
        private System.Data.DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindTracebilityDetails("","","");
            }
        }

        private void BindTracebilityDetails(string Param,string PartNo,string SerialNo)
        {
            try
            {
                dt = GEADatabaseAccess.GetTracebilityReport(Param,PartNo,SerialNo);
                lvTracebilityGridDetails.DataSource = dt;
                lvTracebilityGridDetails.DataBind();
                Session["MainGrid"] = dt;

                for (int i= 0;i < lvTracebilityGridDetails.Items.Count;i++)
                {
                    HtmlGenericControl dataList = (lvTracebilityGridDetails.Items[i].FindControl("dlPartNo") as HtmlGenericControl);
                    TextBox textBox = (lvTracebilityGridDetails.Items[i].FindControl("txtPartNo") as TextBox);
                    textBox.Attributes.Add("list", dataList.ClientID);
                    HiddenField lbl = lvTracebilityGridDetails.Items[i].FindControl("hdnParamID") as HiddenField;
                    DataTable dt = GEADatabaseAccess.getPartNolst(lbl.Value);
                    if(dt.Rows.Count>0)
                    {
                        List<string> PartNoList = dt.AsEnumerable().Select(x => x.Field<string>("PartNo")).Distinct().ToList();
                        var builder = new System.Text.StringBuilder();
                        if (PartNoList.Count > 0)
                        {
                            for (int k = 0; k < PartNoList.Count; k++)
                            {
                                builder.Append(String.Format("<option value='{0}'>", PartNoList[k].ToString()));
                            }
                        }
                        dataList.InnerHtml = builder.ToString();
                    }
                    
                    DataTable slNodt = GEADatabaseAccess.getSerialNolst(lbl.Value);
                    if(slNodt.Rows.Count>0)
                    {
                        List<string> SerialNoList = slNodt.AsEnumerable().Select(x => x.Field<string>("SerialNo")).Distinct().ToList();
                        HtmlGenericControl SerialdataList = (lvTracebilityGridDetails.Items[i].FindControl("dlSerialNo") as HtmlGenericControl);
                        TextBox textSerialBox = (lvTracebilityGridDetails.Items[i].FindControl("txtSerialNo") as TextBox);
                        textSerialBox.Attributes.Add("list", SerialdataList.ClientID);
                        var Sbuilder = new System.Text.StringBuilder();
                        if (SerialNoList.Count > 0)
                        {
                            for (int k = 0; k < SerialNoList.Count; k++)
                            {
                                Sbuilder.Append(String.Format("<option value='{0}'>", SerialNoList[k].ToString()));
                            }
                        }
                        SerialdataList.InnerHtml = Sbuilder.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int RowIndex = Convert.ToInt32(((sender as Button).NamingContainer as ListViewDataItem).DataItemIndex);
                string Description= (lvTracebilityGridDetails.Items[RowIndex].FindControl("hdnParamID") as HiddenField).Value;
                string MaterialID= (lvTracebilityGridDetails.Items[RowIndex].FindControl("txtPartNo") as TextBox).Text;
                string SerialNo= (lvTracebilityGridDetails.Items[RowIndex].FindControl("txtSerialNo") as TextBox).Text;
                DataTable dt = GEADatabaseAccess.GetTracebilityReport(Description,MaterialID,SerialNo);
                string MachineNo = dt.AsEnumerable().Where(x => x.Field<dynamic>("ParameterID") == Description).Select(x => x.Field<dynamic>("FabricationNumber")).FirstOrDefault();
                string MachineString = "";
                if(!String.IsNullOrEmpty(MachineNo))
                {
                    string[] machineSplit = MachineNo.Split(',');
                    int count = 1;
                    for (int i = 0; i < machineSplit.Length; i++)
                    {
                        if(count < 5)
                        {
                            MachineString = MachineString + "," + machineSplit[i];
                            count++;
                        }
                        else
                        {
                            MachineString = MachineString + "," + machineSplit[i];
                            MachineString = MachineString + "@";
                            count = 1;
                        }
                        //MachineString = MachineString + "," + machineSplit[i];
                        //if ((MachineString.Split(',').Length) % 5 == 0)
                        //{
                        //    MachineString = MachineString + "," + machineSplit[i];
                        //    MachineString = MachineString + "@";
                        //}
                    }
                    MachineString = MachineString.TrimStart(',').Replace("@,", "<br />").Replace("@", "");
                    (lvTracebilityGridDetails.Items[RowIndex].FindControl("lblMachineNo") as Label).Text = MachineString;
                }
                else
                    (lvTracebilityGridDetails.Items[RowIndex].FindControl("lblMachineNo") as Label).Text = string.Empty;

                NewDatatable = Session["GRidTable"] == null ? (DataTable)Session["MainGrid"] :(DataTable)Session["GRidTable"];
                foreach (DataRow dr in NewDatatable.Rows) 
                {
                    if (dr["ParameterID"].Equals(Description)) 
                    {
                        dr["PartNo"] = MaterialID;
                        dr["SerialNo"] = SerialNo;
                        dr["FabricationNumber"] = MachineNo;
                    }
                }
                Session["GRidTable"] = NewDatatable;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void lvTracebilityGridDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {

                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                   
                    HiddenField lbl = e.Item.FindControl("hdnParamID") as HiddenField;
                    TextBox textBox = e.Item.FindControl("txtPartNo") as TextBox;
                    if(lbl.Value.Equals("Main Motor", StringComparison.OrdinalIgnoreCase) || lbl.Value.Equals("Secondary Motor", StringComparison.OrdinalIgnoreCase))
                    {
                        textBox.Enabled = false;
                    }
                    else
                    {
                        textBox.Enabled = true;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
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
                string Filename = "TracebilityReport.xlsx";
                string Source = string.Empty;
                Source = GetReportPath(Filename);
                string Template = string.Empty;
                Template = "TracebilityReport" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Tracebility Report- \n " + Source);
                }
                if (Session["GRidTable"] != null)
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    ExcelWorksheet workSheet = Excel.Workbook.Worksheets[0];
                    DataTable dt = Session["GRidTable"] as DataTable;
                    if (dt.Rows.Count > 0 && dt != null)
                    {
                        int row = 6, col = 1;
                        foreach (DataRow item in dt.Rows)
                        {
                            col = 1;
                            workSheet.Cells[row, col].Value = item["ParameterDescription"];
                            col++;
                            workSheet.Cells[row, col].Value = item["PartNo"];
                            col++;
                            workSheet.Cells[row, col].Value = item["SerialNo"];
                            col++;

                            string MachineNo = item["FabricationNumber"].ToString();
                            string MachineString = string.Empty;
                            string[] machineSplit = MachineNo.Split(',');
                            int count = 1;
                            for (int i = 0; i < machineSplit.Length; i++)
                            {
                                if (count < 5)
                                {
                                    MachineString = MachineString + "," + machineSplit[i];
                                    count++;
                                }
                                else
                                {
                                    MachineString = MachineString + "," + machineSplit[i];
                                    MachineString = MachineString + "@";
                                    count = 1;
                                }
                            }
                            MachineString = MachineString.TrimStart(',').Replace("@,", "\n").Replace("@", "");
                            workSheet.Cells[row, col].Value = MachineString;
                            row++;
                        }
                    }
                    DownloadMultipleFile(destination, Excel.GetAsByteArray());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}