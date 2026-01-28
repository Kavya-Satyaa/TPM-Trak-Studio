using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class DocumentView : System.Web.UI.Page
    {
        private string RootPath;
        static NetworkConnection nc = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindPlant();
                    BindCellID();
                    BindComponent();
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindPlant()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlants();
                list.RemoveAt(0);
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();
                BindCellID();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindCellID()
        {
            try
            {
                List<string> list = DBAccess.getCellAssignedForEmployee(ddlPlant.SelectedValue, Session["UserName"].ToString());
                ddlCell.DataSource = list;
                ddlCell.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellID();
            ddlCell_SelectedIndexChanged(null, null);
        }

        protected void ddlCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindComponent();
        }

        private string BindMutliComponent()
        {
            string ComponentID = string.Empty;
            int count = 0;
            try
            {
                foreach (ListItem Item in ddlMultiComponent.Items)
                {
                    if(Item.Selected)
                    {
                        count++;
                        ComponentID += "'" + Item.Value + "',";
                    }
                    else
                    {
                        ComponentID += "";
                    }
                    if(count>5)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "showWarningMsg('Component selected cannot be greater than 5')", true);
                        Item.Selected = false;
                       
                    }
                }
                ComponentID = ComponentID.Trim(',');
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return ComponentID;
        }

        private void BindComponent()
        {
            try
            {
                int i = 1;
                string Component = string.Empty;
                List<string> Complst = TMPTrakDataBase.GetComponentIdListBasedOnPlantCell(ddlPlant.SelectedValue.ToString(),ddlCell.SelectedValue.ToString());
                ddlMultiComponent.DataSource = Complst;
                ddlMultiComponent.DataBind();
                
                foreach(ListItem item in ddlMultiComponent.Items)
                {
                    if(i<=5)
                    {
                        //item.Selected = true;
                        Component += "'" + item.Value + "',";
                        i++;
                    }
                }
                Component = Component.Trim(',');
                Bindoperation(Component);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void Bindoperation(string Comp)
        {
            try
            {
                List<string> OpnLst = TMPTrakDataBase.GetOperationNo(Comp);
                ddlOperation.DataSource = OpnLst;
                ddlOperation.DataBind();
                Session["OperationList"] = OpnLst;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private string FindInvalidCharacter(string param)
        {
            string regSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex rg = new Regex(string.Format("[{0}]", Regex.Escape(regSearch)));
            string foldernname = rg.Replace(param, "");
            return foldernname;
        }

        private void BindData()
        {
            try
            {
                BindNetworkConnection();

                string ComponentID = string.Empty;
                List<DocumentEntity> list = new List<DocumentEntity>();
                foreach (ListItem Item in ddlMultiComponent.Items)
                {
                    if (Item.Selected)
                    {
                        List<string> fileNames = new List<string>();
                        string Comp = FindInvalidCharacter(Item.Value);

                        //string Comp = Item.Value.Replace("/", "~");
                        string path = Path.Combine(RootPath, Comp);

                        //string opNo = TMPTrakDataBase.GetOperationNoByComp("'"+Item.Value+"'");
                        List<string> opLst= Session["OperationList"] as List<string>;

                        if (Directory.Exists(path))
                        {
                            foreach(var op in opLst)
                            {
                                DocumentEntity entity = new DocumentEntity();
                                entity.DrawingData = new List<DocumentEntity>();
                                entity.ProgramData = new List<DocumentEntity>();
                                entity.ToolsData = new List<DocumentEntity>();
                                entity.FixtureData = new List<DocumentEntity>();
                                entity.GaugeData = new List<DocumentEntity>();
                                entity.InspectionData = new List<DocumentEntity>();

                                if (op != ddlOperation.SelectedValue)
                                {
                                    continue;
                                }

                                string OpDesc = DBAccess.GetOperationDesc(Item.Value, op);
                                OpDesc = FindInvalidCharacter(OpDesc);
                                string Op_desc = op + "_" + OpDesc;

                                string DrawingPath = Path.Combine(path, "Drawing", Op_desc);
                                if (Directory.Exists(DrawingPath))
                                {
                                    var dir = new DirectoryInfo(DrawingPath);
                                    fileNames = (from flInfo in dir.GetFiles() select flInfo.Name).ToList();
                                    foreach (var item in fileNames)
                                    {
                                        entity.DrawingData.Add(new DocumentEntity { DrawingDocFileName = item });
                                    }
                                }
                                string ProgramPath = Path.Combine(path, "Program", Op_desc);
                                if (Directory.Exists(ProgramPath))
                                {
                                    var dir = new DirectoryInfo(ProgramPath);
                                    fileNames = (from flInfo in dir.GetFiles() select flInfo.Name).ToList();
                                    foreach (var item in fileNames)
                                    {
                                        entity.ProgramData.Add(new DocumentEntity { ProgramDocFileName = item });
                                    }
                                }
                                string ToolsPath = Path.Combine(path, "Tools", Op_desc);
                                if (Directory.Exists(ToolsPath))
                                {
                                    var dir = new DirectoryInfo(ToolsPath);
                                    fileNames = (from flInfo in dir.GetFiles() select flInfo.Name).ToList();
                                    foreach (var item in fileNames)
                                    {
                                        entity.ToolsData.Add(new DocumentEntity { ToolsDocFileName = item });
                                    }
                                }
                                string FixturePath = Path.Combine(path, "Fixture", Op_desc);
                                if (Directory.Exists(FixturePath))
                                {
                                    var dir = new DirectoryInfo(FixturePath);
                                    fileNames = (from flInfo in dir.GetFiles() select flInfo.Name).ToList();
                                    foreach (var item in fileNames)
                                    {
                                        entity.FixtureData.Add(new DocumentEntity { FixtureDocFileName = item });
                                    }
                                }
                                string GaugePath = Path.Combine(path, "Gauge", Op_desc);
                                if (Directory.Exists(GaugePath))
                                {
                                    var dir = new DirectoryInfo(GaugePath);
                                    fileNames = (from flInfo in dir.GetFiles() select flInfo.Name).ToList();
                                    foreach (var item in fileNames)
                                    {
                                        entity.GaugeData.Add(new DocumentEntity { GaugeDocFileName = item });
                                    }
                                }
                                string InspectionPath = Path.Combine(path, "Inspection", Op_desc);
                                if (Directory.Exists(InspectionPath))
                                {
                                    var dir = new DirectoryInfo(InspectionPath);
                                    fileNames = (from flInfo in dir.GetFiles() select flInfo.Name).ToList();
                                    foreach (var item in fileNames)
                                    {
                                        entity.InspectionData.Add(new DocumentEntity { InspectionDocFileName = item });
                                    }

                                }

                                if (Directory.Exists(DrawingPath) || Directory.Exists(ProgramPath) || Directory.Exists(ToolsPath) || Directory.Exists(FixturePath) || Directory.Exists(GaugePath) || Directory.Exists(InspectionPath))
                                {
                                    entity.ComponentID = Item.Value;
                                    entity.operationno = op;
                                    list.Add(entity);
                                }

                            }
                        }
                     }
                }
                lvDocumentUpload.DataSource = list;
                lvDocumentUpload.DataBind();
            }
            catch (Exception ex)
            {
                if (nc != null)
                    nc.Dispose();
                Logger.WriteErrorLog("View Details:" + ex.ToString());
                HelperClassGeneric.openWarningToastrModal(this, "Network Connection Failed to Open");
            }
            finally
            {
                if (nc != null)
                    nc.Dispose();
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void ddlMultiComponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            string comp = BindMutliComponent();
            Bindoperation(comp);
            ScriptManager.RegisterStartupScript(this,this.GetType(), "open", "StayMultiselectedList();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "search", "BindSearchVal();", true);
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


        protected void lnDocumentView_Click(object sender, EventArgs e)
        {
            try
            {
                BindNetworkConnection();

                //RootPath = DBAccess.GetRootPath("RootPath");
                string path = string.Empty;
                LinkButton linkButton = (LinkButton)sender;
                string filename= linkButton.Text;

                ListViewItem nestedlistViewItem = (ListViewItem)linkButton.NamingContainer;
                ListView parentListView = (ListView)nestedlistViewItem.NamingContainer;
                ListViewItem mainlistViewItem = (ListViewItem)parentListView.NamingContainer;
                string Comp = (mainlistViewItem.FindControl("lblComponent") as Label).Text;
                string Op = (mainlistViewItem.FindControl("lblOpnID") as Label).Text;
                string OpDesc = DBAccess.GetOperationDesc(Comp, Op);

                Comp = FindInvalidCharacter(Comp);
                OpDesc = FindInvalidCharacter(OpDesc);
                string Op_desc = Op + "_" + OpDesc;

                if (hdnUpdate.Value.ToString().Equals("drawing"))
                {
                    path = Path.Combine(RootPath, Comp, "Drawing", Op_desc, filename);
                }
                if (hdnUpdate.Value.ToString().Equals("program"))
                {
                    path = Path.Combine(RootPath, Comp, "Program", Op_desc, filename);
                }
                if (hdnUpdate.Value.ToString().Equals("tools"))
                {
                    path = Path.Combine(RootPath, Comp, "Tools", Op_desc, filename);
                }
                if (hdnUpdate.Value.ToString().Equals("fixture"))
                {
                    path = Path.Combine(RootPath, Comp, "Fixture", Op_desc, filename);
                }
                if (hdnUpdate.Value.ToString().Equals("gauge"))
                {
                    path = Path.Combine(RootPath, Comp, "Gauge", Op_desc, filename);
                }
                if (hdnUpdate.Value.ToString().Equals("inspection"))
                {
                    path = Path.Combine(RootPath, Comp, "Inspection", Op_desc, filename);
                }

                //foreach (var item in lvDocumentUpload.Items)
                //{
                //    string Component = (item.FindControl("lblComponent") as Label).Text;
                //    Comp = FindInvalidCharacter(Component);

                //    Op = (item.FindControl("lblOpnID") as Label).Text;
                //    string OpDesc = DBAccess.GetOperationDesc(Component, Op);
                //    OpDesc = FindInvalidCharacter(OpDesc);
                //    string Op_desc = Op + "_" + OpDesc;

                //    if (hdnUpdate.Value.ToString().Equals("drawing"))
                //    {
                //        path = Path.Combine(RootPath, Comp, "Drawing", Op_desc, filename);
                //    }
                //    if (hdnUpdate.Value.ToString().Equals("program"))
                //    {
                //        path = Path.Combine(RootPath, Comp, "Program", Op_desc, filename);
                //    }
                //    if (hdnUpdate.Value.ToString().Equals("tools"))
                //    {
                //        path = Path.Combine(RootPath, Comp, "Tools", Op_desc, filename);
                //    }
                //    if (hdnUpdate.Value.ToString().Equals("fixture"))
                //    {
                //        path = Path.Combine(RootPath, Comp, "Fixture", Op_desc, filename);
                //    }
                //    if (hdnUpdate.Value.ToString().Equals("gauge"))
                //    {
                //        path = Path.Combine(RootPath, Comp, "Gauge", Op_desc, filename);
                //    }
                //    if (hdnUpdate.Value.ToString().Equals("inspection"))
                //    {
                //        path = Path.Combine(RootPath, Comp, "Inspection", Op_desc, filename);
                //    }
                //}

                System.IO.FileInfo file = new System.IO.FileInfo(path);
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, (int)fs.Length);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        protected void lnkClear_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in ddlMultiComponent.Items)
                {
                    item.Selected = false;
                }
                Bindoperation("");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
    }
}