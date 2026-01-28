using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.ShantiIron.Model;

namespace Web_TPMTrakDashboard.KTASpindle
{
    public partial class CompOpDocumentView : System.Web.UI.Page
    {
        private string RootPath;
        static NetworkConnection nc = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string Component = Uri.UnescapeDataString(Request.QueryString["CompID"]);
                string Op = Uri.UnescapeDataString(Request.QueryString["OperationNo"]);
                string Desc = Uri.UnescapeDataString(Request.QueryString["OpDesc"]);

                lblComp.Text = Component;
                lblOp.Text = Op;
                BindData(Component, Op, Desc);
            }
        }

        private void BindNetworkConnection()
        {
            try
            {
                string userame = System.Web.Configuration.WebConfigurationManager.AppSettings["KTAUserName"].ToString();
                string password = System.Web.Configuration.WebConfigurationManager.AppSettings["KTAPassword"].ToString();

                RootPath = DBAccess.GetRootPath("RootPath");

                if (!string.IsNullOrEmpty(userame) && !string.IsNullOrEmpty(password))
                {
                    try
                    {
                        nc = new NetworkConnection(RootPath, new NetworkCredential(userame, password));
                    }
                    catch (Exception ex)
                    {
                        if (nc != null)
                            nc.Dispose();
                        Logger.WriteErrorLog(ex.ToString());
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
        }

        private void BindData(string Component,string op,string Desc)
        {
            try
            {

                BindNetworkConnection();

                string ComponentID = string.Empty;
                List<DocumentEntity> list = new List<DocumentEntity>();

                List<string> fileNames = new List<string>();
                string regSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                Regex rg = new Regex(string.Format("[{0}]", Regex.Escape(regSearch)));
                string Comp = rg.Replace(Component, "");
                hdnComp.Value = Comp;

                Desc = rg.Replace(Desc, "");
                string Op_Desc = op + "_" + Desc;
                hdnOpDesc.Value = Op_Desc;

                string path = Path.Combine(RootPath, Comp);
                if (Directory.Exists(path))
                {
                    DocumentEntity entity = new DocumentEntity();
                    entity.DrawingData = new List<DocumentEntity>();
                    entity.ProgramData = new List<DocumentEntity>();
                    entity.ToolsData = new List<DocumentEntity>();
                    entity.FixtureData = new List<DocumentEntity>();
                    entity.GaugeData = new List<DocumentEntity>();
                    entity.InspectionData = new List<DocumentEntity>(); 
                    entity.ProvenProgramData = new List<DocumentEntity>();
                    entity.StandardProgramData = new List<DocumentEntity>();


                    string DrawingPath = Path.Combine(path, "Drawing", Op_Desc);
                    if (Directory.Exists(DrawingPath))
                    {
                        var dir = new DirectoryInfo(DrawingPath);
                        fileNames = (from flInfo in dir.GetFiles() where (flInfo.Extension == ".pdf" || flInfo.Extension == ".txt") select flInfo.Name).ToList();
                        foreach (var item in fileNames)
                        {
                            entity.DrawingData.Add(new DocumentEntity { DrawingDocFileName = item });
                        }
                    }
                    string ProgramPath = Path.Combine(path, "Program", Op_Desc);
                    if (Directory.Exists(ProgramPath))
                    {
                        var dir = new DirectoryInfo(ProgramPath);
                        fileNames = (from flInfo in dir.GetFiles() where (flInfo.Extension == ".pdf" || flInfo.Extension == ".txt") select flInfo.Name).ToList();
                        foreach (var item in fileNames)
                        {
                            entity.ProgramData.Add(new DocumentEntity { ProgramDocFileName = item });
                        }
                    }
                    string ToolsPath = Path.Combine(path, "Tools", Op_Desc);
                    if (Directory.Exists(ToolsPath))
                    {
                        var dir = new DirectoryInfo(ToolsPath);
                        fileNames = (from flInfo in dir.GetFiles() where (flInfo.Extension == ".pdf" || flInfo.Extension == ".txt") select flInfo.Name).ToList();
                        foreach (var item in fileNames)
                        {
                            entity.ToolsData.Add(new DocumentEntity { ToolsDocFileName = item });
                        }
                    }
                    string FixturePath = Path.Combine(path, "Fixture", Op_Desc);
                    if (Directory.Exists(FixturePath))
                    {
                        var dir = new DirectoryInfo(FixturePath);
                        fileNames = (from flInfo in dir.GetFiles() where (flInfo.Extension == ".pdf" || flInfo.Extension == ".txt") select flInfo.Name).ToList();
                        foreach (var item in fileNames)
                        {
                            entity.FixtureData.Add(new DocumentEntity { FixtureDocFileName = item });
                        }
                    }
                    string GaugePath = Path.Combine(path, "Gauge", Op_Desc);
                    if (Directory.Exists(GaugePath))
                    {
                        var dir = new DirectoryInfo(GaugePath);
                        fileNames = (from flInfo in dir.GetFiles() where (flInfo.Extension == ".pdf" || flInfo.Extension == ".txt") select flInfo.Name).ToList();
                        foreach (var item in fileNames)
                        {
                            entity.GaugeData.Add(new DocumentEntity { GaugeDocFileName = item });
                        }
                    }
                    string InspectionPath = Path.Combine(path, "Inspection", Op_Desc);
                    if (Directory.Exists(InspectionPath))
                    {
                        var dir = new DirectoryInfo(InspectionPath);
                        fileNames = (from flInfo in dir.GetFiles() where (flInfo.Extension == ".pdf" || flInfo.Extension == ".txt") select flInfo.Name).ToList();
                        foreach (var item in fileNames)
                        {
                            entity.InspectionData.Add(new DocumentEntity { InspectionDocFileName = item });
                        }

                    }
                    string ProvenProgramPath = Path.Combine(path, "Program", Op_Desc, "Proven Machine Program");
                    if (Directory.Exists(ProvenProgramPath))
                    {
                        var dir = new DirectoryInfo(ProvenProgramPath);
                        fileNames = (from flInfo in dir.GetFiles() where (flInfo.Extension==".pdf" || flInfo.Extension==".txt") select flInfo.Name).ToList();
                        foreach (var item in fileNames)
                        {
                            entity.ProvenProgramData.Add(new DocumentEntity { ProvenProgramDocFileName = item });
                        }

                    }
                    string StandardProgramPath = Path.Combine(path, "Program", Op_Desc, "Standard Software Program");
                    if (Directory.Exists(StandardProgramPath))
                    {
                        var dir = new DirectoryInfo(StandardProgramPath);
                        fileNames = (from flInfo in dir.GetFiles() where (flInfo.Extension == ".pdf" || flInfo.Extension == ".txt") select flInfo.Name).ToList();
                        foreach (var item in fileNames)
                        {
                            entity.StandardProgramData.Add(new DocumentEntity { StandardProgramDocFileName = item });
                        }

                    }

                    if (Directory.Exists(DrawingPath) || Directory.Exists(ProgramPath) || Directory.Exists(ToolsPath) || Directory.Exists(FixturePath) || Directory.Exists(GaugePath) || Directory.Exists(InspectionPath) || Directory.Exists(ProvenProgramPath) || Directory.Exists(StandardProgramPath))
                    {
                        entity.ComponentID = Component;
                        entity.operationno = op;
                        list.Add(entity);
                    }
                    lvDocumentUpload.DataSource = list;
                    lvDocumentUpload.DataBind();
                }


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("View Details:" + ex.ToString());
            }
        }

        protected void lnkBackBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoreAndonKTA.aspx");
        }

        protected void lnFileName_Click(object sender, EventArgs e)
        {
            try
            {
                BindNetworkConnection();

                LinkButton lnk = sender as LinkButton;
                string Filepath = string.Empty;
                RootPath = DBAccess.GetRootPath("RootPath");
                string FileName = documentName.Text = lnk.Text;
                int flag = 0;

                if (hdnUpdate.Value.ToString().Equals("drawing"))
                {
                    Filepath = Path.Combine(RootPath, hdnComp.Value, "Drawing", hdnOpDesc.Value, FileName);
                }
                //if (hdnUpdate.Value.ToString().Equals("program"))
                //{
                //    Filepath = Path.Combine(RootPath, hdnComp.Value, "Program", hdnOpDesc.Value, FileName);
                //}
                if (hdnUpdate.Value.ToString().Equals("tools"))
                {
                    Filepath = Path.Combine(RootPath, hdnComp.Value, "Tools", hdnOpDesc.Value, FileName);
                }
                if (hdnUpdate.Value.ToString().Equals("fixture"))
                {
                    Filepath = Path.Combine(RootPath, hdnComp.Value, "Fixture", hdnOpDesc.Value, FileName);
                }
                if (hdnUpdate.Value.ToString().Equals("gauge"))
                {
                    Filepath = Path.Combine(RootPath, hdnComp.Value, "Gauge", hdnOpDesc.Value, FileName);
                }
                if (hdnUpdate.Value.ToString().Equals("inspection"))
                {
                    Filepath = Path.Combine(RootPath, hdnComp.Value, "Inspection", hdnOpDesc.Value, FileName);
                }
                if (hdnUpdate.Value.ToString().Equals("ProvenProgram"))
                {
                    Filepath = Path.Combine(RootPath, hdnComp.Value, "Program", hdnOpDesc.Value, "Proven Machine Program", FileName);
                    if (FileName.Contains(".txt"))
                    {
                        Process.Start("notepad.exe", Filepath);
                        flag = 1;
                    }
                }
                if (hdnUpdate.Value.ToString().Equals("StandardProgram"))
                {
                    Filepath = Path.Combine(RootPath, hdnComp.Value, "Program", hdnOpDesc.Value, "Standard Software Program", FileName);
                    if (FileName.Contains(".txt"))
                    {
                       // Process.Start("notepad.exe",Filepath);
                        flag = 1;
                    }
                }
                if (flag == 0)
                {
                    byte[] fileByte = File.ReadAllBytes(Filepath);
                    string pdfUrl = Convert.ToBase64String(fileByte);
                    iframeText.Visible = false;
                    iframeDocument.Visible = true;
                    iframeDocument.Attributes["src"] = "data:application/pdf; base64," + pdfUrl;
                }
                else
                {

                    string fileContent = File.ReadAllText(Filepath);
                    string htmlContent = fileContent.Replace(Environment.NewLine, "<br/>");
                    iframeDocument.Visible = false;
                    iframeText.Visible = true;
                    iframeText.Attributes["srcdoc"] = htmlContent;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            
        }


    }
}