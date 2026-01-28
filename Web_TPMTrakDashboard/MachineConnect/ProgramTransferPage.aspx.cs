using FocasLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using System.Net.NetworkInformation;
using System.Text;
using System.Resources;
using System.Globalization;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Configuration;
using System.Web;

namespace Web_TPMTrakDashboard.MachineConnect
{
    public partial class ProgramTransferPage : Page
    {
        string localDirectory = string.Empty;
        private const string MachineFolderPath = "//CNC_MEM/USER/PATH1/";

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Initial Machine Settings
                ViewState["MachineData_DNCIP"] = null;
                ViewState["MachineData_DNCIPPort"] = null;
                ViewState["MachineData_OPCUAUrl"] = null;
                ViewState["MachineData_ProgramFoldersEnabled"] = null;

                //Program Transfer Settings
                ViewState["Setting_FolderPath"] = null;
                ViewState["Setting_FilePathExtension"] = null;

                //Local Path Data
                ViewState["LocalData_TempFilePath"] = null;
                ViewState["LocalData_FileData"] = null;

                //Machine Path Data
                ViewState["MachineData_TreeViewMachine"] = MachineFolderPath;
                ViewState["MachineData_OpenFileName"] = null;
                ViewState["MachineData_SelectedProgForUpdateDelete"] = null;

                //List of Programs
                Session["ProgramsFromCNC"] = null;
                Session["ProgramsFromCNC_SortOrder"] = null;

                btnUploadProgs.Text = "Upload >>>";
                btnDownloadProgs.Text = "Download <<<";
                btnDeleteProg.Text = "Delete Program";

                ddlPlantID.DataSource = DataBaseAccess.GetPlantsFromDB();
                ddlPlantID.DataBind();

                ddlPlantID_SelectedIndexChanged(null, null);

                ddlExtension.DataSource = new List<string> { ".txt", ".nc" };
                ddlExtension.DataBind();

                DataBaseAccess.LoadInitialProgramTransferSettings(out string Path, out string FileFormat);
                ViewState["Setting_FolderPath"] = txtPrgFolderPath.Text = Path;
                if (FileFormat.Equals(".txt",StringComparison.OrdinalIgnoreCase) || FileFormat.Equals(".nc",StringComparison.OrdinalIgnoreCase))
                    ViewState["Setting_FilePathExtension"] = ddlExtension.SelectedValue = FileFormat.ToLower();
            }
        }
        protected void treeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                ViewState["LocalData_TempFilePath"] = treeComputerDir.SelectedValue;
                string[] files = Directory.GetFiles(treeComputerDir.SelectedValue);
                List<ProgramTransferModel> list = new List<ProgramTransferModel>();
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    ProgramTransferModel programTransfer = new ProgramTransferModel()
                    {
                        FileNames = fileName
                    };
                    list.Add(programTransfer);
                }
                lvLocalFiles.DataSource = list;
                lvLocalFiles.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("treeView1_SelectedNodeChanged: " + ex.ToString());
            }
        }
        protected void btnCNCDir_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlPlantID.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT1", "HideLoader();", true);
                    HelperClassGeneric.openWarningToastrModal(this, "Please select the appropriate PLANT ID");
                    return;
                }
                if (string.IsNullOrEmpty(ddlMachineID.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT1", "HideLoader();", true);
                    HelperClassGeneric.openWarningToastrModal(this, "Please select the appropriate MACHINE ID");
                    return;
                }
                DataBaseAccess.GetProgTransferMachineDetailsFromDB(ddlMachineID.SelectedValue, out string DNCIP, out string DNCIPPort, out string OPCUAUrl, out bool ProgramFoldersEnabled);
                ViewState["MachineData_DNCIP"] = DNCIP;
                ViewState["MachineData_DNCIPPort"] = DNCIPPort;
                ViewState["MachineData_OPCUAUrl"] = OPCUAUrl;
                ViewState["MachineData_ProgramFoldersEnabled"] = ProgramFoldersEnabled;

                if (string.IsNullOrEmpty(ViewState["Setting_FolderPath"].ToString()))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT1", "HideLoader();", true);
                    HelperClassGeneric.openWarningToastrModal(this, "Local Folder path cannot be empty");
                    return;
                }
                else
                {
                    try
                    {
                        thLocal1.InnerText = $"Computer ({ddlMachineID.SelectedValue})";
                        string InitialFolderPath = Path.Combine(ViewState["Setting_FolderPath"].ToString(),ddlPlantID.SelectedValue, ddlMachineID.SelectedValue);
                        treeComputerDir.Nodes.Clear();
                        if (!Directory.Exists(InitialFolderPath))
                        {
                            Directory.CreateDirectory(InitialFolderPath);
                        }
                        localDirectory = InitialFolderPath;

                        TreeNode rootNode = new TreeNode(ddlMachineID.SelectedValue, localDirectory)
                        {
                            ImageUrl = "~/Images/foldericon.png"
                        };
                        treeComputerDir.Nodes.Add(rootNode);
                        PopulateTreeView(localDirectory, rootNode);

                        treeComputerDir.ExpandAll();

                        ViewState["LocalData_TempFilePath"] = localDirectory;
                        string[] files = Directory.GetFiles(localDirectory);
                        List<ProgramTransferModel> list = new List<ProgramTransferModel>();
                        foreach (string file in files)
                        {
                            string fileName = Path.GetFileName(file);
                            ProgramTransferModel programTransfer = new ProgramTransferModel()
                            {
                                FileNames = fileName
                            };
                            list.Add(programTransfer);
                        }
                        lvLocalFiles.DataSource = list;
                        lvLocalFiles.DataBind();
                    }
                    catch(Exception ex)
                    {
                        Logger.WriteErrorLog("LocalFolderCreation btnCNCClick: " + ex.Message);
                        ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT1", "HideLoader();", true);
                        HelperClassGeneric.openWarningToastrModal(this, ex.Message);
                        return;
                    }
                }

                Ping p = new Ping();
                PingReply reply = p.Send(DNCIP, 2000);
                if (reply.Status != IPStatus.Success)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT1", "HideLoader();", true);
                    HelperClassGeneric.openErrorModal(this, $"{reply.Status} {DNCIP}");
                    return;
                }

                if (OPCUAUrl == string.Empty)
                {
                    ushort focusHandle = ConnectToCNC(DNCIP, DNCIPPort);
                    if (focusHandle != 0)
                    {
                        treeViewMachineFolders.Nodes.Clear();
                        TreeNode root = new TreeNode("CNC_MEM", "CNC_MEM")
                        {
                            ImageUrl = "~/Images/foldericon.png"
                        };
                        treeViewMachineFolders.Nodes.Add(root);
                        PopulateTreeViewMachine("//CNC_MEM/", root, focusHandle);

                        treeViewMachineFolders.ExpandAll();
                        TraverseTreeViewNodes(treeViewMachineFolders.Nodes);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT1", "HideLoader();", true);
                    HelperClassGeneric.openErrorModal(this, $"Operation Not applicable for this machine");
                    return;
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT1", "HideLoader();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnCNCDir_Click: " + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT1", "HideLoader();", true);
            }
        }
        protected void lbProgNo_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewItem).DataItemIndex);
                txtProgData.ReadOnly = true;
                btnSave.Visible = false;
                txtProgData.Text = DownloadProgram(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), (lvMachinePrograms.Items[rowIndex].FindControl("lbProgNo") as LinkButton).Text, string.Empty, false, (lvMachinePrograms.Items[rowIndex].FindControl("lblComments") as Label).Text, ViewState["MachineData_TreeViewMachine"].ToString(), Convert.ToBoolean(ViewState["MachineData_ProgramFoldersEnabled"]), true, ViewState["Setting_FilePathExtension"].ToString(),string.Empty);
                HelperClassGeneric.openModal(this, "progViewModal", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lbProgNo_Click: " + ex.ToString());
            }
        }
        protected void lbFiles_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewItem).DataItemIndex);
                string fileName = (lvLocalFiles.Items[rowIndex].FindControl("lbFiles") as LinkButton).Text;
                ViewState["MachineData_OpenFileName"] = fileName;

                if (ViewState["LocalData_TempFilePath"] != null)
                {
                    string localFileData = File.ReadAllText(Path.Combine(ViewState["LocalData_TempFilePath"].ToString(), fileName));
                    txtProgData.ReadOnly = false;
                    btnSave.Visible = true;
                    HelperClassGeneric.openModal(this, "progViewModal", true);
                    if (localFileData != null)
                    {
                        txtProgData.Text = localFileData;
                    }
                    else
                    {
                        txtProgData.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lbFiles_Click: " + ex.ToString());
            }
        }
        protected void btnProgViewmodalCancel_Click(object sender, EventArgs e)
        {
            try
            {
                HelperClassGeneric.clearModal(this);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnProgViewmodalCancel_Click: " + ex.ToString());
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["LocalData_TempFilePath"] != null)
                {
                    File.WriteAllText(Path.Combine(ViewState["LocalData_TempFilePath"].ToString(), ViewState["MachineData_OpenFileName"].ToString()), txtProgData.Text);
                    HelperClassGeneric.clearModal(this);
                    HelperClassGeneric.openSuccessModal(this, "File content updated successfully");
                }
                else
                {
                    HelperClassGeneric.openErrorModal(this, "Failed to update the file!!");
                    HelperClassGeneric.clearModal(this);
                }
            }
            catch (IOException iex)
            {
                HelperClassGeneric.clearModal(this);
                HelperClassGeneric.openErrorModal(this, iex.Message);

                Logger.WriteErrorLog("btnSave_Click: " + iex.ToString());
            }
            catch (Exception ex)
            {
                HelperClassGeneric.clearModal(this);
                HelperClassGeneric.openErrorModal(this, ex.Message);

                Logger.WriteErrorLog("btnSave_Click: " + ex.ToString());
            }
        }
        protected void treeViewMachineFolders_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["MachineData_OPCUAUrl"] != null && ViewState["MachineData_OPCUAUrl"].ToString() == string.Empty)
                {
                    
                    List<ProgramTransferModel> listProgramFromCNC = ReadAllPrograms(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), 2, treeViewMachineFolders.SelectedValue, Convert.ToBoolean(ViewState["MachineData_ProgramFoldersEnabled"]));
                    Session["ProgramsFromCNC"] = listProgramFromCNC;
                    ViewState["MachineData_TreeViewMachine"] = treeViewMachineFolders.SelectedValue;
                    short mainProgram = ReadRunningProgramNumber(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), out short subProgram);
                    foreach(ProgramTransferModel prg in listProgramFromCNC)
                    {
                        if (prg.ProgNo.Equals($"O{mainProgram}"))
                            prg.RowColor = "#90ee90";
                        else
                            prg.RowColor = "#f5f5f5";
                    }
                    lvMachinePrograms.DataSource = listProgramFromCNC;
                    lvMachinePrograms.DataBind();
                    if((lvMachinePrograms.FindControl("lblMachinePath") as Label)!=null)
                        (lvMachinePrograms.FindControl("lblMachinePath") as Label).Text = treeViewMachineFolders.SelectedValue;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("treeViewMachineFolders_SelectedNodeChanged: " + ex.ToString());
            }
        }
        protected void btnCompareProg_Click(object sender, EventArgs e)
        {
            //TextBox1.Text = CompareTexts(out string one);
            //TextBox2.Text = File.ReadAllText("D:\\Repos\\ProgramTransfer\\CNC-08\\New folder\\New folder\\New Text Document.txt");
            //HelperClassGeneric.openModal(this, "progCompareModal", true);
        }
        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewItem).DataItemIndex);
                string progNo = (lvMachinePrograms.Items[rowIndex].FindControl("lbProgNo") as LinkButton).Text;
                string progComments = (lvMachinePrograms.Items[rowIndex].FindControl("lblComments") as Label).Text;
                DownloadProgram(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), progNo, string.Empty, false, progComments, ViewState["MachineData_TreeViewMachine"].ToString(), Convert.ToBoolean(ViewState["MachineData_ProgramFoldersEnabled"]), false, ViewState["Setting_FilePathExtension"].ToString(), ViewState["LocalData_TempFilePath"].ToString());
                HelperClassGeneric.openSuccessModal(this, "Program saved locally");

                if (ViewState["LocalData_TempFilePath"] != null)
                {
                    string[] files = Directory.GetFiles(ViewState["LocalData_TempFilePath"].ToString());
                    List<ProgramTransferModel> list = new List<ProgramTransferModel>();
                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileName(file);
                        ProgramTransferModel programTransfer = new ProgramTransferModel()
                        {
                            FileNames = fileName
                        };
                        list.Add(programTransfer);
                    }
                    lvLocalFiles.DataSource = list;
                    lvLocalFiles.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lnkDownload_Click: " + ex.ToString());
            }
            
        }
        protected void btnSettings_Click(object sender, EventArgs e)
        {
            try
            {
                DataBaseAccess.LoadInitialProgramTransferSettings(out string Path, out string FileFormat);
                ViewState["Setting_FolderPath"] = txtPrgFolderPath.Text = Path;
                if (FileFormat.Equals(".txt", StringComparison.OrdinalIgnoreCase) || FileFormat.Equals(".nc", StringComparison.OrdinalIgnoreCase))
                    ViewState["Setting_FilePathExtension"] = ddlExtension.SelectedValue = FileFormat.ToLower();
                HelperClassGeneric.openModal(this, "progTransSettings", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnSettings_Click: " + ex.ToString());
            }
        }
        protected void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                if (DataBaseAccess.UpdatePTSettingtoDB(txtPrgFolderPath.Text, ddlExtension.SelectedValue))
                {
                    HelperClassGeneric.clearModal(this);
                    HelperClassGeneric.openSuccessModal(this, "Settings Updated Successfully");
                }
                else
                {
                    HelperClassGeneric.clearModal(this);
                    HelperClassGeneric.openErrorModal(this, "Failed to Update ");
                }
                ViewState["Setting_FilePathExtension"] = ddlExtension.SelectedValue;
                ViewState["Setting_FolderPath"] = txtPrgFolderPath.Text;
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("btnSaveSettings_Click: " + ex.ToString());
            }
        }
        protected void btnCancelSettings_Click(object sender, EventArgs e)
        {
            HelperClassGeneric.clearModal(this);
        }
        protected void lnkUpload_Click(object sender, EventArgs e)
        {
            try
            {
                int rowindex = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewItem).DataItemIndex);
                Ping p = new Ping();
                PingReply reply = p.Send(ViewState["MachineData_DNCIP"].ToString(), 2000);
                if (reply.Status != IPStatus.Success)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT1", "HideLoader();", true);
                    HelperClassGeneric.openErrorModal(this, $"{reply.Status} {ViewState["MachineData_DNCIP"]}");
                    return;
                }
                if (ViewState["MachineData_OPCUAUrl"] != null && ViewState["MachineData_OPCUAUrl"].ToString() == string.Empty)
                {
                    short mainProgram;
                    mainProgram = ReadRunningProgramNumber(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), out short subProgram);
                    string selectedProgram = Path.GetFileNameWithoutExtension((lvLocalFiles.Items[rowindex].FindControl("lbFiles") as LinkButton).Text);
                    ViewState["tempSelectedProg"] = selectedProgram;
                    if (selectedProgram.IndexOfAny(new char[] { '(' }) >= 0)
                        selectedProgram = selectedProgram.Substring(0, selectedProgram.IndexOfAny(new char[] { '(' }));
                    ViewState["MachineData_SelectedProgForUpdateDelete"] = selectedProgram;
                    bool status = CurrentlyRunning(mainProgram, subProgram, selectedProgram);
                    if (status)
                    {
                        Logger.WriteDebugLog(string.Format("Selected program " + "{0}" + " is currently running. Unable to upload.", selectedProgram));
                        HelperClassGeneric.openErrorModal(this, $"Selected program {selectedProgram} is currently running. Upload Operation Cancelled");
                        return;
                    }
                    status = CheckProgramExistence(selectedProgram, (List<ProgramTransferModel>)Session["ProgramsFromCNC"]);
                    if (status)
                    {
                        HelperClassGeneric.openModal(this, "ReplaceConfirmationModal", true);
                    }
                    else
                    {
                        PreUploadMethod();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lnkUpload_Click: " + ex.ToString());
            }
        }
        protected void btnYes_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> UploadProgramLst = new List<string>();
                for (int i = 0; i < lvLocalFilesModal.Items.Count; i++)
                {
                    if(!(lvLocalFilesModal.Items[i].FindControl("cbxProgsLocalModal") as CheckBox).Checked)
                    {
                        continue;
                    }
                    string selectedProgram = Path.GetFileNameWithoutExtension((lvLocalFilesModal.Items[i].FindControl("lbFilesModal") as Label).Text);
                    UploadProgramLst.Add(selectedProgram);
                    ViewState["tempSelectedProg"] = selectedProgram;
                    if (selectedProgram.IndexOfAny(new char[] { '(' }) >= 0)
                        selectedProgram = selectedProgram.Substring(0, selectedProgram.IndexOfAny(new char[] { '(' }));
                    ViewState["MachineData_SelectedProgForUpdateDelete"] = selectedProgram;
                    DeletePrograms(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), selectedProgram, ViewState["MachineData_TreeViewMachine"].ToString(), (bool)ViewState["MachineData_ProgramFoldersEnabled"]);
                    string filePath = Path.Combine(ViewState["LocalData_TempFilePath"].ToString(), ViewState["tempSelectedProg"].ToString() + ViewState["Setting_FilePathExtension"].ToString());
                    StringBuilder strPrograms = new StringBuilder();
                    strPrograms.Append("%");
                    if (!File.Exists(filePath))
                    {
                        continue;
                    }
                    string tempProgram = File.ReadAllText(filePath);
                    strPrograms.Append(tempProgram.Replace('%', ' '));
                    strPrograms.Append("%");
                    UploadProgram(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), strPrograms.ToString(), ViewState["MachineData_TreeViewMachine"].ToString(), (bool)ViewState["MachineData_ProgramFoldersEnabled"]);
                }

                HelperClassGeneric.clearModal(this);
                List<ProgramTransferModel> listProgramFromCNC = ReadAllPrograms(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), 2, ViewState["MachineData_TreeViewMachine"].ToString(), (bool)ViewState["MachineData_ProgramFoldersEnabled"]);
                Session["ProgramsFromCNC"] = listProgramFromCNC;
                short mainProgram = ReadRunningProgramNumber(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), out short subProgram);
                foreach (ProgramTransferModel prg in listProgramFromCNC)
                {
                    if (prg.ProgNo.Equals($"O{mainProgram}"))
                        prg.RowColor = "#90ee90";
                    else
                        prg.RowColor = "#f5f5f5";
                }
                lvMachinePrograms.DataSource = listProgramFromCNC;
                lvMachinePrograms.DataBind();

                if (UploadProgramLst.Count > 0)
                {
                    foreach (string ProgramNo in UploadProgramLst)
                    {
                        DataBaseAccess.SaveProgramUploadDetails(ddlMachineID.SelectedValue.ToString(), HttpContext.Current.Session["UserName"].ToString(), ProgramNo, "Upload");
                    }

                }

                HelperClassGeneric.openSuccessModal(this, "Upload Operation Completed");
                ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT5", "HideLoader();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnYes_Click: " + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT5", "HideLoader();", true);
            }
        }
        protected void btnNo_Click(object sender, EventArgs e)
        {
            try
            {
                HelperClassGeneric.clearModal(this);
                List<ProgramTransferModel> listProgramFromCNC = ReadAllPrograms(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), 2, ViewState["MachineData_TreeViewMachine"].ToString(), (bool)ViewState["MachineData_ProgramFoldersEnabled"]);
                Session["ProgramsFromCNC"] = listProgramFromCNC;
                short mainProgram = ReadRunningProgramNumber(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), out short subProgram);
                foreach (ProgramTransferModel prg in listProgramFromCNC)
                {
                    if (prg.ProgNo.Equals($"O{mainProgram}"))
                        prg.RowColor = "#90ee90";
                    else
                        prg.RowColor = "#f5f5f5";
                }
                lvMachinePrograms.DataSource = listProgramFromCNC;
                lvMachinePrograms.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT5", "HideLoader();", true);
                HelperClassGeneric.openSuccessModal(this, "Upload Operation Completed");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnNo_Click: " + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT5", "HideLoader();", true);
            }
        }
        protected void btnYesDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool success = true;
                HelperClassGeneric.clearModal(this);
                for (int i = 0; i < lvMachinePrograms.Items.Count; i++)
                {
                    if (!(lvMachinePrograms.Items[i].FindControl("cbxProgs") as CheckBox).Checked)
                    {
                        continue;
                    }
                    string prgNo = (lvMachinePrograms.Items[i].FindControl("lbProgNo") as LinkButton).Text;
                    success &= DeletePrograms(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), prgNo, ViewState["MachineData_TreeViewMachine"].ToString(), (bool)ViewState["MachineData_ProgramFoldersEnabled"]);
                }

                List<ProgramTransferModel> listProgramFromCNC = ReadAllPrograms(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), 2, ViewState["MachineData_TreeViewMachine"].ToString(), (bool)ViewState["MachineData_ProgramFoldersEnabled"]);
                Session["ProgramsFromCNC"] = listProgramFromCNC;
                short mainProgram = ReadRunningProgramNumber(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), out short subProgram);
                foreach (ProgramTransferModel prg in listProgramFromCNC)
                {
                    if (prg.ProgNo.Equals($"O{mainProgram}"))
                        prg.RowColor = "#90ee90";
                    else
                        prg.RowColor = "#f5f5f5";
                }
                lvMachinePrograms.DataSource = listProgramFromCNC;
                lvMachinePrograms.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT5", "HideLoader();", true);
                if (success)
                    HelperClassGeneric.openSuccessModal(this, "Delete Operation Sucessful");
                else
                    HelperClassGeneric.openErrorModal(this, "Failed to delete Some/All Programs");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT5", "HideLoader();", true);
                Logger.WriteErrorLog("btnYesDelete_Click: " + ex.ToString());
            }
        }
        protected void btnNoDelete_Click(object sender, EventArgs e)
        {
            try
            {
                HelperClassGeneric.clearModal(this);
                HelperClassGeneric.openWarningToastrModal(this, "Delete Operation Cancelled");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnNoDelete_Click: " + ex.ToString());
            }
        }
        protected void lnkdltList_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewItem).DataItemIndex);
                ViewState["MachineData_SelectedProgForUpdateDelete"] = (lvMachinePrograms.Items[rowIndex].FindControl("lbProgNo") as LinkButton).Text;
                HelperClassGeneric.openModal(this, "DeleteConfirmationModal", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lnkdltList_Click: " + ex.ToString());
            }
        }
        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlPlantID.SelectedIndex != -1)
                {
                    ddlMachineID.DataSource = DataBaseAccess.GetMachinesForAParticularPlant(ddlPlantID.SelectedValue);
                    ddlMachineID.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlPlantID_SelectedIndexChanged: " + ex.ToString());
            }
        }
        protected void btnSortPrg_Click(object sender, EventArgs e)
        {
            try
            {
                if (SortTheListView("prg", out string order))
                {
                    if (order != string.Empty)
                    {
                        if (order.Equals("Asc"))
                            HelperClassGeneric.openSuccessModal(this, "List Sorted in Asc. Order of Prog No.");
                        else
                            HelperClassGeneric.openSuccessModal(this, "List Sorted in Desc. Order of Prog No.");
                    }
                }
                else
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Failed to sort");
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnSortMod_Click(object sender, EventArgs e)
        {
            try
            {
                if (SortTheListView("Mod", out string order))
                {
                    if (order != string.Empty)
                    {
                        if (order.Equals("Asc"))
                            HelperClassGeneric.openSuccessModal(this, "List Sorted in Asc. Order of Date");
                        else
                            HelperClassGeneric.openSuccessModal(this, "List Sorted in Desc. Order of Date");
                    }
                }
                else
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Failed to sort");
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnUploadProgs_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> UploadProgramLst = new List<string>();
                Ping p = new Ping();
                PingReply reply = p.Send(ViewState["MachineData_DNCIP"].ToString(), 2000);
                if (reply.Status != IPStatus.Success)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT1", "HideLoader();", true);
                    HelperClassGeneric.openErrorModal(this, $"{reply.Status} {ViewState["MachineData_DNCIP"]}");
                    return;
                }
                if (ViewState["MachineData_OPCUAUrl"] != null && ViewState["MachineData_OPCUAUrl"].ToString() == string.Empty)
                {
                    List<ProgramTransferModel> programsHold = new List<ProgramTransferModel>();
                    short mainProgram = ReadRunningProgramNumber(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), out short subProgram);
                    int count = 0;
                    for (int i = 0; i < lvLocalFiles.Items.Count; i++)
                    {
                        if (!(lvLocalFiles.Items[i].FindControl("cbxProgsLocal") as CheckBox).Checked)
                        {
                            continue;
                        }
                        count++;
                        string selectedProgram = Path.GetFileNameWithoutExtension((lvLocalFiles.Items[i].FindControl("lbFiles") as LinkButton).Text);
                        UploadProgramLst.Add(selectedProgram);
                        ViewState["tempSelectedProg"] = selectedProgram;
                        if (selectedProgram.IndexOfAny(new char[] { '(' }) >= 0)
                            selectedProgram = selectedProgram.Substring(0, selectedProgram.IndexOfAny(new char[] { '(' }));
                        ViewState["MachineData_SelectedProgForUpdateDelete"] = selectedProgram;
                        if (CurrentlyRunning(mainProgram, subProgram, selectedProgram))
                        {
                            programsHold.Add(new ProgramTransferModel { ProgNo = ViewState["tempSelectedProg"].ToString(), RowColor = "#Ff0000", RowEnabled = false });
                            continue;
                        }
                        if (CheckProgramExistence(selectedProgram, (List<ProgramTransferModel>)Session["ProgramsFromCNC"]))
                        {
                            programsHold.Add(new ProgramTransferModel { ProgNo = ViewState["tempSelectedProg"].ToString(), RowColor = "#f5f5f5", RowEnabled = true });
                            continue;
                        }
                        else
                        {
                            string filePath = Path.Combine(ViewState["LocalData_TempFilePath"].ToString(), ViewState["tempSelectedProg"].ToString() + ViewState["Setting_FilePathExtension"].ToString());
                            StringBuilder strPrograms = new StringBuilder();
                            strPrograms.Append("%");
                            if (!File.Exists(filePath))
                            {
                                continue;
                            }
                            string tempProgram = File.ReadAllText(filePath);
                            strPrograms.Append(tempProgram.Replace('%', ' '));
                            strPrograms.Append("%");
                            UploadProgram(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), strPrograms.ToString(), ViewState["MachineData_TreeViewMachine"].ToString(), (bool)ViewState["MachineData_ProgramFoldersEnabled"]);
                        }
                    }
                    if (programsHold.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT3", "HideLoader();", true);
                        HelperClassGeneric.openModal(this, "ReplaceConfirmationModal", true);
                        lvLocalFilesModal.DataSource = programsHold;
                        lvLocalFilesModal.DataBind();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT3", "HideLoader();", true);
                        if (count > 0)
                            HelperClassGeneric.openSuccessModal(this, "Upload Operation Completed");
                        else
                            HelperClassGeneric.openWarningToastrModal(this, "No Program Selected");
                        List<ProgramTransferModel> listProgramFromCNC = ReadAllPrograms(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), 2, ViewState["MachineData_TreeViewMachine"].ToString(), (bool)ViewState["MachineData_ProgramFoldersEnabled"]);
                        Session["ProgramsFromCNC"] = listProgramFromCNC;
                        mainProgram = ReadRunningProgramNumber(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), out subProgram);
                        foreach (ProgramTransferModel prg in listProgramFromCNC)
                        {
                            if (prg.ProgNo.Equals($"O{mainProgram}"))
                                prg.RowColor = "#90ee90";
                            else
                                prg.RowColor = "#f5f5f5";
                        }
                        lvMachinePrograms.DataSource = listProgramFromCNC;
                        lvMachinePrograms.DataBind();

                        if (UploadProgramLst.Count > 0)
                        {
                            foreach (string ProgramNo in UploadProgramLst)
                            {
                                DataBaseAccess.SaveProgramUploadDetails(ddlMachineID.SelectedValue.ToString(), HttpContext.Current.Session["UserName"].ToString(), ProgramNo, "Upload");
                            }

                        }
                    }
                }               
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT6", "HideLoader();", true);
            }
        }
        protected void btnDownloadProgs_Click(object sender, EventArgs e)
        {
            try
            {
                int ckcount = 0;
                for (int i = 0; i < lvMachinePrograms.Items.Count; i++)
                {
                    if (!(lvMachinePrograms.Items[i].FindControl("cbxProgs") as CheckBox).Checked)
                    {
                        continue;
                    }
                    string progNo = (lvMachinePrograms.Items[i].FindControl("lbProgNo") as LinkButton).Text;
                    string progComments = (lvMachinePrograms.Items[i].FindControl("lblComments") as Label).Text;
                    DownloadProgram(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), progNo, ViewState["LocalData_TempFilePath"].ToString(), false, progComments, ViewState["MachineData_TreeViewMachine"].ToString(), Convert.ToBoolean(ViewState["MachineData_ProgramFoldersEnabled"]), false, ViewState["Setting_FilePathExtension"].ToString(), ViewState["LocalData_TempFilePath"].ToString());
                    ckcount++;
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT2", "HideLoader();", true);
                if (ckcount > 0)
                    HelperClassGeneric.openSuccessModal(this, "Program saved locally");
                else
                    HelperClassGeneric.openWarningToastrModal(this, "No Program Selected");

                if (ViewState["LocalData_TempFilePath"] != null)
                {
                    string[] files = Directory.GetFiles(ViewState["LocalData_TempFilePath"].ToString());
                    List<ProgramTransferModel> list = new List<ProgramTransferModel>();
                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileName(file);
                        ProgramTransferModel programTransfer = new ProgramTransferModel()
                        {
                            FileNames = fileName
                        };
                        list.Add(programTransfer);
                    }
                    lvLocalFiles.DataSource = list;
                    lvLocalFiles.DataBind();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "HideloadPT2", "HideLoader();", true);
            }
        }
        protected void btnDeleteProg_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag = true;
                bool run = false;
                string prog = string.Empty;
                for (int i = 0; i < lvMachinePrograms.Items.Count; i++)
                {
                    if ((lvMachinePrograms.Items[i].FindControl("cbxProgs") as CheckBox).Checked)
                    {
                        flag = false;
                        break;
                    }
                }
                for (int i = 0; i < lvMachinePrograms.Items.Count; i++)
                {
                    if ((lvMachinePrograms.Items[i].FindControl("cbxProgs") as CheckBox).Checked)
                    {
                        prog = (lvMachinePrograms.Items[i].FindControl("lbProgNo") as LinkButton).Text;
                        short mainProgram = ReadRunningProgramNumber(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), out short subProgram);
                        if (CurrentlyRunning(mainProgram, subProgram, prog))
                        {
                            run = true;
                            break;
                        }
                    }
                }
                if (!flag)
                {
                    if (run)
                        HelperClassGeneric.openErrorModal(this, $"{prog} is currently running. Please Uncheck {prog} and try again");
                    else
                        HelperClassGeneric.openModal(this, "DeleteConfirmationModal", true);
                }
                else
                    HelperClassGeneric.openWarningToastrModal(this, "No Program Selected");
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Other Methods
        private void TraverseTreeViewNodes(TreeNodeCollection nodes)
        {
            try
            {
                foreach (TreeNode node in nodes)
                {
                    if(node.Value!= "CNC_MEM")
                    {
                        List<ProgramTransferModel> listProgramFromCNC = ReadAllPrograms(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), 2, node.Value, (bool)ViewState["MachineData_ProgramFoldersEnabled"]);
                        if (listProgramFromCNC.Count > 0)
                        {
                            Session["ProgramsFromCNC"] = listProgramFromCNC;
                            short mainProgram = ReadRunningProgramNumber(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), out short subProgram);
                            foreach (ProgramTransferModel prg in listProgramFromCNC)
                            {
                                if (prg.ProgNo.Equals($"O{mainProgram}"))
                                    prg.RowColor = "#90ee90";
                                else
                                    prg.RowColor = "#f5f5f5";
                            }
                            lvMachinePrograms.DataSource = listProgramFromCNC;
                            lvMachinePrograms.DataBind();
                            ViewState["MachineData_TreeViewMachine"] = node.Value;
                            if ((lvMachinePrograms.FindControl("lblMachinePath") as Label) != null)
                                (lvMachinePrograms.FindControl("lblMachinePath") as Label).Text = node.Value;
                            break;
                        }
                        else
                        {
                            TraverseTreeViewNodes(node.ChildNodes);
                        }
                    }
                    else
                    {
                        TraverseTreeViewNodes(node.ChildNodes);
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("TraverseTreeViewNodes: " + ex.Message);
            }
        }
        private void PreUploadMethod()
        {
            try
            {
                string filePath = ViewState["LocalData_TempFilePath"].ToString();
                filePath = Path.Combine(filePath, ViewState["tempSelectedProg"].ToString() + ViewState["Setting_FilePathExtension"].ToString()); 
                StringBuilder strPrograms = new StringBuilder();
                strPrograms.Append("%");
                if (!File.Exists(filePath))
                {
                    HelperClassGeneric.openErrorModal(this, "File do not exists");
                    return;
                }
                string tempProgram = File.ReadAllText(filePath);
                strPrograms.Append(tempProgram.Replace('%', ' '));
                List<string> programs = new List<string>
                {
                     ViewState["MachineData_SelectedProgForUpdateDelete"].ToString()
                };
                strPrograms.Append("%");
                if (programs.Count > 0)
                {
                    if (UploadProgram(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), strPrograms.ToString(), ViewState["MachineData_TreeViewMachine"].ToString(), (bool)ViewState["MachineData_ProgramFoldersEnabled"]))
                    {
                        HelperClassGeneric.clearModal(this);
                        HelperClassGeneric.openSuccessModal(this, "Program Uploaded Successfully");

                        List<ProgramTransferModel> listProgramFromCNC = ReadAllPrograms(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), 2, ViewState["MachineData_TreeViewMachine"].ToString(), (bool)ViewState["MachineData_ProgramFoldersEnabled"]);
                        Session["ProgramsFromCNC"] = listProgramFromCNC;
                        short mainProgram = ReadRunningProgramNumber(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), out short subProgram);
                        foreach (ProgramTransferModel prg in listProgramFromCNC)
                        {
                            if (prg.ProgNo.Equals($"O{mainProgram}"))
                                prg.RowColor = "#90ee90";
                            else
                                prg.RowColor = "#f5f5f5";
                        }
                        lvMachinePrograms.DataSource = listProgramFromCNC;
                        lvMachinePrograms.DataBind();
                    }
                    else
                    {
                        HelperClassGeneric.openErrorModal(this, "Failed to Upload Program");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("PreUploadMethod: " + ex.ToString());
            }
        }
        public static bool UploadProgram(string ipAddress, ushort portNo, string program, string folderPath, bool isProgramFolderSupports)
        {
            int len, n;
            short ret = FocasLib.cnc_allclibhndl3(ipAddress, portNo, 10, out ushort focasLibHandle);
            if (ret != 0)
            {
                Logger.WriteErrorLog("cnc_allclibhndl3() failed. return value is = " + ret);
                return false;
            }

            int count = 0;
            ret = short.MinValue;
            while (ret != 0 && count < 20)
            {
                if (isProgramFolderSupports == false)
                {
                    ret = FocasLib.cnc_dwnstart3(focasLibHandle, 0);
                }
                else
                {
                    ret = FocasLib.cnc_dwnstart4(focasLibHandle, 0, folderPath);
                }
                count++;
            }
            if (ret == -1)
            {
                if (isProgramFolderSupports == false)
                {
                    FocasLib.cnc_dwnend(focasLibHandle);
                }
                else
                {
                    FocasLib.cnc_dwnend4(focasLibHandle);
                }
                FocasLib.cnc_freelibhndl(focasLibHandle);
                return false;
            }
            if (ret != 0)
            {
                Logger.WriteErrorLog("cnc_dwnstart3() failed. return value is = " + ret);
                FocasLib.cnc_freelibhndl(focasLibHandle);
                return false;
            }
            if (ret == FocasLibBase.EW_OK)
            {
                len = program.Length;
                while (len > 0)
                {
                    n = program.Length;
                    if (isProgramFolderSupports == false)
                    {
                        ret = FocasLib.cnc_download3(focasLibHandle, ref n, program);
                    }
                    else
                    {
                        ret = FocasLib.cnc_download4(focasLibHandle, ref n, program);
                    }

                    if (ret == 10) //if buffer is empty
                    {
                        continue;
                    }
                    if (ret == -2) // if buffer in reset mode
                    {
                        break;
                    }
                    if (ret != FocasLibBase.EW_OK)
                    {
                        Logger.WriteErrorLog("cnc_download3() failed. return value is = " + ret);
                        break;
                    }

                    if (ret == FocasLibBase.EW_OK)
                    {
                        program = program.Substring(n, len - n);
                        len -= n;
                    }
                }

                if (isProgramFolderSupports == false)
                {
                    ret = FocasLib.cnc_dwnend(focasLibHandle);
                }
                else
                {
                    ret = FocasLib.cnc_dwnend4(focasLibHandle);
                }

                if (ret != FocasLib.EW_OK)
                {
                    FocasLibBase.ODBERR a = new FocasLibBase.ODBERR();
                    FocasLib.cnc_getdtailerr(focasLibHandle, a);
                    Logger.WriteErrorLog("cnc_dwnend() failed. return value is = " + ret + " ; Error code : " + a.err_no);
                    FocasLib.cnc_freelibhndl(focasLibHandle);
                    if (ret != 7) Logger.WriteDebugLog("cnc_dwnend() failed. return value is = " + ret + " ; Error code : " + a.err_no);
                    if (ret == 7)
                    {
                        Logger.WriteDebugLog("Upload program failed because write protected on CNC side.");
                    }
                    return false;
                }
                FocasLib.cnc_freelibhndl(focasLibHandle);
            }
            return true;
        }
        private void PopulateTreeView(string dir, TreeNode parentNode)
        {
            try
            {
                string[] subDirectories = Directory.GetDirectories(dir);
                foreach (string subdirectory in subDirectories)
                {
                    TreeNode childNode = new TreeNode(Path.GetFileName(subdirectory), subdirectory)
                    {
                        ImageUrl = "~/Images/foldericon.png"
                    };
                    parentNode.ChildNodes.Add(childNode);
                    PopulateTreeView(subdirectory, childNode);
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("PopulateTreeView: " + ex.ToString());
            }
        }
        public static List<string> GetCNCFolders(ushort focasLibHandle, string folderName)
        {
            List<string> folders = new List<string>();
            try
            {
                FocasLibBase.ODBPDFNFIL NoOfSubFolder = new FocasLibBase.ODBPDFNFIL();
                short ret = FocasLib.cnc_rdpdf_subdirn(focasLibHandle, folderName, NoOfSubFolder);

                short num_prog = 1;
                if (NoOfSubFolder.dir_num > 0)
                {
                    for (short i = 0; i < NoOfSubFolder.dir_num; i++)
                    {
                        FocasLibBase.IDBPDFADIR pdf_adir_in = new FocasLibBase.IDBPDFADIR
                        {
                            path = folderName,
                            req_num = i,
                            type = 0,
                            size_kind = 1
                        };

                        num_prog = 1;
                        FocasLibBase.ODBPDFADIR1 pdf_adir_out1 = new FocasLibBase.ODBPDFADIR1();
                        ret = FocasLib.cnc_rdpdf_alldir(focasLibHandle, ref num_prog, pdf_adir_in, pdf_adir_out1);
                        if (pdf_adir_out1.info1.data_kind == 0)
                        {
                            folders.Add(pdf_adir_out1.info1.d_f);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("GetCNCFolders: " + ex.ToString());
            }
            return folders;
        }
        private void PopulateTreeViewMachine(string firstDriveName, TreeNode parentNode, ushort focusHandle)
        {
            try
            {
                List<string> subDirectories = GetCNCFolders(focusHandle, firstDriveName);

                foreach (string subdirectory in subDirectories)
                {
                    if ((firstDriveName + subdirectory).Equals("//CNC_MEM/SYSTEM", StringComparison.OrdinalIgnoreCase) || (firstDriveName + subdirectory).Equals("//CNC_MEM/MTB1", StringComparison.OrdinalIgnoreCase) || (firstDriveName + subdirectory).Equals("//CNC_MEM/MTB2", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    TreeNode childNode = new TreeNode(subdirectory, firstDriveName + subdirectory + "/")
                    {
                        ImageUrl = "~/Images/foldericon.png"
                    };
                    parentNode.ChildNodes.Add(childNode);
                    PopulateTreeViewMachine(firstDriveName + subdirectory + "/", childNode, focusHandle);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("PopulateTreeViewMachine: " + ex.ToString());
            }
        }
        private ushort ConnectToCNC(string dNCIP, string dNCIPPort)
        {
            ushort value = 0;
            try
            {
                short ret = -20;
                ret = FocasLib.cnc_allclibhndl3(dNCIP, Convert.ToUInt16(dNCIPPort), 10, out value);
                if (ret != 0)
                {
                    Logger.WriteErrorLog("cnc_allclibhndl3() failed. return value is = " + ret + "Not able to connect to CNC machine. Please check the network connection and try again");
                    return ushort.MinValue;
                }
                return value;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ConnectToCNC: " + ex.ToString());
            }
            return value;
        }
        public static List<ProgramTransferModel> ReadAllPrograms(string ipAddress, ushort portNo, short programDetailType, string folderPath, bool isProgramFolderSupports)
        {
            List<ProgramTransferModel> programs = new List<ProgramTransferModel>();
            try
            {
                int topProgram = 0;
                short prgromsToRead = 10;

                short ret = FocasLib.cnc_allclibhndl3(ipAddress, portNo, 10, out ushort focasLibHandle);
                if (ret != 0)
                {
                    Logger.WriteErrorLog("cnc_allclibhndl3() failed. return value is = " + ret);
                    return null;
                }
                if (isProgramFolderSupports == false)
                {
                    ret = 0;
                    while (prgromsToRead >= 10)
                    {
                        FocasLibBase.PRGDIR3 d = new FocasLibBase.PRGDIR3();
                        try
                        {
                            ret = FocasLib.cnc_rdprogdir3(focasLibHandle, programDetailType, ref topProgram, ref prgromsToRead, d);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog("ReadAllPrograms1: " + ex.ToString());
                        }
                        if (ret != 0)
                        {
                            Logger.WriteErrorLog("cnc_rdprogdir3() failed. return value is = " + ret);
                            break;
                        }
                        if (prgromsToRead == 0)
                        {
                            Logger.WriteErrorLog("No more programss to read/found.");
                            break;
                        }

                        if (prgromsToRead >= 1) get_program(programs, d.dir1);
                        if (prgromsToRead >= 2) get_program(programs, d.dir2);
                        if (prgromsToRead >= 3) get_program(programs, d.dir3);
                        if (prgromsToRead >= 4) get_program(programs, d.dir4);
                        if (prgromsToRead >= 5) get_program(programs, d.dir5);
                        if (prgromsToRead >= 6) get_program(programs, d.dir6);
                        if (prgromsToRead >= 7) get_program(programs, d.dir7);
                        if (prgromsToRead >= 8) get_program(programs, d.dir8);
                        if (prgromsToRead >= 9) get_program(programs, d.dir9);
                        if (prgromsToRead >= 10) get_program(programs, d.dir10);

                        var lastProgramNo = programs[programs.Count - 1].ProgNo;
                        if (int.TryParse(lastProgramNo, out topProgram))
                        {
                            topProgram++;
                        }
                    }
                }
                else
                {
                    FocasLibBase.ODBPDFNFIL NoOfSubFolder = new FocasLibBase.ODBPDFNFIL();
                    ret = FocasLib.cnc_rdpdf_subdirn(focasLibHandle, folderPath, NoOfSubFolder);
                    short num_prog = 1;
                    if (NoOfSubFolder.file_num > 0)
                    {
                        for (short i = 0; i < NoOfSubFolder.file_num; i++)
                        {
                            FocasLibBase.IDBPDFADIR pdf_adir_in = new FocasLibBase.IDBPDFADIR();
                            pdf_adir_in.path = folderPath;
                            pdf_adir_in.req_num = (short)(NoOfSubFolder.dir_num + i);
                            pdf_adir_in.type = 1;
                            pdf_adir_in.size_kind = 1;

                            num_prog = 1;
                            FocasLibBase.ODBPDFADIR1 pdf_adir_out1 = new FocasLibBase.ODBPDFADIR1();
                            ret = FocasLib.cnc_rdpdf_alldir(focasLibHandle, ref num_prog, pdf_adir_in, pdf_adir_out1);
                            if (pdf_adir_out1.info1.data_kind == 1) get_programOITF(programs, pdf_adir_out1.info1);

                        }
                    }
                }
                FocasLib.cnc_freelibhndl(focasLibHandle);
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("ReadAllPrograms: " + ex.ToString());
            }
            return programs;
        }
        private static void get_program(List<ProgramTransferModel> ls, FocasLibBase.PRGDIR3_data dir)
        {
            try
            {
                ProgramTransferModel pDto = new ProgramTransferModel
                {
                    ProgNo = dir.number.ToString(),
                    ProgLength = dir.length,
                    ProgComments = dir.comment,
                    IsSupportFolder = false
                };
                pDto.ProgModifiedTs = new DateTime(dir.mdate.year < 2000 ? dir.mdate.year + 2000 : dir.mdate.year, dir.mdate.month, dir.mdate.day, dir.mdate.hour, dir.mdate.minute, 00).ToString("dd-MMM-yyyy hh:mm tt");
                ls.Add(pDto);
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("get_program: " + ex.ToString());
            }
        }
        private static void get_programOITF(List<ProgramTransferModel> ls, FocasLibBase.ODBPDFADIR dir)
        {
            try
            {
                ProgramTransferModel pDto = new ProgramTransferModel
                {
                    ProgNo = dir.d_f.Trim(),
                    ProgLength = 0,
                    ProgComments = dir.comment
                };
                pDto.ProgModifiedTs = new DateTime(dir.year < 2000 ? dir.year + 2000 : dir.year, dir.mon, dir.day, dir.hour, dir.min, 00).ToString("dd-MMM-yyyy hh:mm tt");
                ls.Add(pDto);
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("get_programOITF: " + ex.ToString());
            }
        }
        public string CompareTexts()
        {
            //var differ = new Differ();
            //var inlineBuilder = new InlineDiffBuilder(differ);
            //var diffModel = inlineBuilder.BuildDiffModel(File.ReadAllText("D:\\Repos\\ProgramTransfer\\CNC-08\\New folder\\New folder\\dqwdqwd.txt"), File.ReadAllText("D:\\Repos\\ProgramTransfer\\CNC-08\\New folder\\New folder\\New Text Document.txt"));
            //originalText = string.Empty;
            string result = string.Empty;

            //foreach (var line in diffModel.Lines)
            //{
            //    switch (line.Type)
            //    {
            //        case ChangeType.Deleted:
            //            result += $"[-] {line.Text}\n";
            //            break;
            //        case ChangeType.Inserted:
            //            result += "[+] " + line.Text + "\n";
            //            break;
            //        case ChangeType.Unchanged:
            //            result += "" + line.Text + "\n";
            //            break;
            //    }
            //}

            return result;
        }
        public static string DownloadProgram(string ipAddress, ushort portNo, string file, string destFolder, bool compareProgram, string comments, string folderPath, bool isProgramFolderSupports, bool isView, string ext, string localFilePath = "")
        {
            int programNo = 0;
            string programNoStr = string.Empty;
            if (isProgramFolderSupports == false)
                programNo = Convert.ToInt32(file.Replace(" ", "").TrimStart(new char[] { 'O' }));
            else
                programNoStr = file;
            int bufsize = 1280;
            int dataLength = 0;
            string programDownloaded = string.Empty;
            try
            {
                short ret = FocasLib.cnc_allclibhndl3(ipAddress, portNo, 10, out ushort focasLibHandle);
                if (ret != 0)
                {
                    Logger.WriteErrorLog("cnc_allclibhndl3() failed. return value is = " + ret);
                    return "cnc_allclibhndl3() failed. return value is = " + ret;
                }
                if (isProgramFolderSupports)
                    ret = FocasLib.cnc_upstart4(focasLibHandle, 0, folderPath + programNoStr);
                else
                    ret = FocasLib.cnc_upstart3(focasLibHandle, 0, programNo, programNo);
                if (ret == -1)
                {
                    FocasLib.cnc_freelibhndl(focasLibHandle);
                    return "CNC is busy. Please try after some time";
                }

                if (ret != 0)
                {
                    Logger.WriteErrorLog("cnc_upstart3/4() failed. return value is = " + ret);
                    FocasLib.cnc_freelibhndl(focasLibHandle);
                    return "cnc_upstart3/4() failed. return value is = " + ret;
                }
                StringBuilder programStr = new StringBuilder(bufsize * 10);
                bool endFound = false;
                do
                {
                    char[] buf = new char[bufsize + 1];
                    dataLength = bufsize;
                    if (isProgramFolderSupports)
                        ret = FocasLib.cnc_upload4(focasLibHandle, ref dataLength, buf);
                    else
                        ret = FocasLib.cnc_upload3(focasLibHandle, ref dataLength, buf);
                    if (ret == 10)
                        continue;
                    if (ret == -2)
                    {
                        Logger.WriteErrorLog("cnc_upload3() failed. return value is = " + ret);
                        break;
                    }
                    if (ret == 7)
                    {
                        Logger.WriteErrorLog("cnc_upload3() failed. return value is = " + ret);
                        break;
                    }
                    if (ret != 0)
                    {
                        Logger.WriteErrorLog("cnc_upload3() failed. return value is = " + ret);
                        break;
                    }

                    char[] tempBuf = new char[dataLength];
                    Array.Copy(buf, tempBuf, dataLength);
                    programStr.Append(tempBuf);
                    if (buf[dataLength - 1] == '%')
                    {
                        break;
                    }
                } while (endFound == false);
                if (ret == 7)
                {
                    FocasLib.cnc_freelibhndl(focasLibHandle);
                    return string.Empty;
                }
                if (isProgramFolderSupports)
                    ret = FocasLib.cnc_upend4(focasLibHandle);
                else
                    ret = FocasLib.cnc_upend3(focasLibHandle);
                if (ret != 0)
                {
                    Logger.WriteErrorLog("cnc_upend34() failed. return value is = " + ret);
                    return string.Empty;
                }
                if (string.IsNullOrEmpty(Convert.ToString(programStr)))
                {
                    Logger.WriteDebugLog("program is empty.");
                    return string.Empty;
                }
                programStr.Replace("\r", "").Replace("\n", "\r\n");
                if (compareProgram)
                {
                    string tempFile = string.Format("{0}{1}{2}", isProgramFolderSupports ? programNoStr : "O" + programNo.ToString(), isProgramFolderSupports ? "(" + SafeFileName(comments) + ")" : SafeFileName(comments), ext);
                    tempFile = Path.Combine("Temp", tempFile);
                    if (!Directory.Exists(Path.Combine(destFolder, "temp")))
                    {
                        try
                        {
                            Directory.CreateDirectory(Path.Combine(destFolder, "temp"));
                        }
                        catch(Exception ex)
                        {
                            Logger.WriteErrorLog("DownloadProgram2: " + ex.ToString());
                        }
                    }
                    tempFile = Path.Combine(destFolder, tempFile);
                    File.WriteAllText(tempFile, programStr.ToString());
                    return tempFile;
                }
                else
                {
                    file = string.Format("{0}{1}", isProgramFolderSupports ? programNoStr : "O" + programNo.ToString(), isProgramFolderSupports ? "(" + SafeFileName(comments) + ")" : SafeFileName(comments));
                    string version = GetVersion(destFolder, file, programStr,ext, out int status);
                    if (status == 1)
                    {
                        Logger.WriteDebugLog(version);
                        version = string.Empty;
                        return string.Empty;
                    }
                    if (version.ToLower().IndexOf("ERROR") >= 0)
                    {
                        Logger.WriteDebugLog("Code Error");
                        return string.Empty;
                    }
                    try
                    {
                        if (isView)
                            programDownloaded = programStr.ToString();
                        else
                        {
                            File.WriteAllText(Path.Combine(localFilePath, version), programStr.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteErrorLog("DownloadProgram1: " + ex.ToString());
                    }
                }
                FocasLib.cnc_freelibhndl(focasLibHandle);
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("DownloadProgram: " + ex.ToString());
            }
            return programDownloaded;
        }
        public static string SafeFileName(string name)
        {
            StringBuilder str = new StringBuilder(name);
            try
            {
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                {
                    str = str.Replace(c, '_');
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("SafeFileName: " + ex.ToString());
            }
            return str.ToString();
        }
        private static string GetVersion(string folder, string file, StringBuilder program, string fileextension, out int status)
        {
            int i = 1;
            status = 0;
            file += fileextension;
            if (!File.Exists(Path.Combine(folder, file)))
                return Path.Combine(folder, file);
            else
            {
                bool matching = CompareContents(Path.Combine(folder, file), program);
                if (matching == true)
                {
                    status = 1;
                    return Translations.GetString("ProgramTransferPTMessage1") + Path.GetFileNameWithoutExtension(Path.Combine(folder, file));
                }

                string path_wtout_exten = Path.GetFileNameWithoutExtension(Path.Combine(folder, file));
                while (true)
                {
                    string pwithext = Path.Combine(folder, path_wtout_exten + "." + i.ToString() + fileextension);
                    if (!File.Exists(pwithext))
                    {
                        return pwithext;
                    }
                    else
                    {

                        matching = CompareContents(pwithext, program);
                        if (matching == true)
                        {
                            status = 1;
                            return Translations.GetString("ProgramTransferPTMessage1") + Path.GetFileNameWithoutExtension(pwithext);

                        }
                        i++;
                        if (i >= 1000)
                            return "ERROR";
                    }
                }
            }
        }
        private static bool CompareContents(string pwithext, StringBuilder programRecieved)
        {
            string str1 = string.Empty;
            try
            {
                str1 = File.ReadAllText(pwithext);
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("CompareContents: " + ex.ToString());
            }
            if (str1.Equals(programRecieved.ToString(), StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        public static bool CurrentlyRunning(short mainProgram, short subProgram, string ProgramNo)
        {
            if (ProgramNo.TrimStart(new char[] { 'O' }).Equals(mainProgram.ToString(), StringComparison.OrdinalIgnoreCase) ||
                ProgramNo.TrimStart(new char[] { 'O' }).Equals(subProgram.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        private short ReadRunningProgramNumber(string ip, ushort port, out short subProgram)
        {
            subProgram = 0;
            FocasLibBase.ODBPRO odbpro = new FocasLibBase.ODBPRO();
            try
            {
                short ret = FocasLib.cnc_allclibhndl3(ip, port, 10, out ushort focasLibHandle);
                if (ret != 0)
                {
                    Logger.WriteErrorLog("cnc_allclibhndl3() failed. return value is = " + ret);
                    return subProgram;
                }
                FocasLib.cnc_rdprgnum(focasLibHandle, odbpro);
                subProgram = odbpro.mdata;
                FocasLib.cnc_freelibhndl(focasLibHandle);
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("ReadRunningProgramNumber: " + ex.ToString());
            }
            return odbpro.data;
        }
        public static bool CheckProgramExistence(string programNo, List<ProgramTransferModel> ls)
        {
            int index = -1;
            bool status = false;
            if (ls != null && ls.Count > 0)
            {
                try
                {
                    index = ls.FindIndex(delegate (ProgramTransferModel pd1) { return (pd1.ProgNo.TrimStart(new char[] { 'O' }).Equals(programNo.TrimStart(new char[] { 'O' }), StringComparison.OrdinalIgnoreCase)); });
                    if (index >= 0)
                    {
                        status = true;
                    }
                }
                catch (Exception ex)
                {
                    status = false;
                    Logger.WriteDebugLog(ex.ToString());
                }
            }
            else
            {
                status = false;
            }
            return status;
        }
        public static bool DeletePrograms(string ipAddress, ushort portNo, string programNumber, string folderPath, bool isProgramFolderSupports)
        {
            short tempProgramNo = 0;
            string tempProgramNoStr = "";
            if (isProgramFolderSupports == false)
            {
                short.TryParse(programNumber.TrimStart(new char[] { 'O', ' ' }), out tempProgramNo);
                if (tempProgramNo == 0) return false;
            }
            else
            {
                tempProgramNoStr = programNumber;
            }

            bool isdeleted = false;
            int ret1 = -1;
            ret1 = FocasLib.cnc_allclibhndl3(ipAddress, portNo, 10, out ushort focasLibHandle);
            if (ret1 != 0)
            {
                Logger.WriteErrorLog("cnc_allclibhndl3() failed. return value is = " + ret1);
                return false;
            }

            if (isProgramFolderSupports == true)
            {
                string fileName = folderPath + programNumber;
                ret1 = FocasLib.cnc_pdf_del(focasLibHandle, fileName);
            }
            else
            {
                ret1 = FocasLib.cnc_delete(focasLibHandle, tempProgramNo);
            }

            Enum.TryParse(ret1.ToString(), out focas_ret ret);
            switch (ret)
            {
                case focas_ret.EW_OK:
                    Logger.WriteDebugLog("PROGRAM " + programNumber + " has been deleted.");
                    isdeleted = true;
                    break;
                case focas_ret.EW_DATA:
                    Logger.WriteDebugLog("PROGRAM " + programNumber + " doesn't exist.");
                    break;
                case focas_ret.EW_PROT:
                    Logger.WriteDebugLog("Program " + programNumber + " is PROTECTED.");
                    break;
                case focas_ret.EW_BUSY:
                    Logger.WriteDebugLog("CNC is busy. Program " + programNumber + "  delete REJECTED.");
                    break;
                case focas_ret.EW_PASSWD:
                    Logger.WriteDebugLog("Specified program number cannot be deleted because the data is protected.");
                    break;
                case focas_ret.DEL_FAILED:
                    Logger.WriteDebugLog("CNC execution rejected. CNC is on execution or CNC is in the emergency stop. ");
                    break;
            }
            FocasLib.cnc_freelibhndl(focasLibHandle);
            return isdeleted;
        }
        public bool SortTheListView(string type,out string order)
        {
            bool res = false;
            order = string.Empty;
            try
            {
                List<ProgramTransferModel> list = (List<ProgramTransferModel>)Session["ProgramsFromCNC"];
                short mainProgram = ReadRunningProgramNumber(ViewState["MachineData_DNCIP"].ToString(), Convert.ToUInt16(ViewState["MachineData_DNCIPPort"]), out short subProgram);
                foreach (ProgramTransferModel prg in list)
                {
                    if (prg.ProgNo.Equals($"O{mainProgram}"))
                        prg.RowColor = "#90ee90";
                    else
                        prg.RowColor = "#f5f5f5";
                }
                if(Session["ProgramsFromCNC_SortOrder"]==null || Session["ProgramsFromCNC_SortOrder"].ToString() == "Desc")
                {
                    Session["ProgramsFromCNC_SortOrder"] = order = "Asc";
                    if (type.Equals("prg", StringComparison.OrdinalIgnoreCase))
                    {
                        lvMachinePrograms.DataSource = list.OrderBy(x => x.ProgNo).ToList();
                        lvMachinePrograms.DataBind();
                        res = true;
                    }
                    else
                    {
                        lvMachinePrograms.DataSource = list.OrderBy(x => Convert.ToDateTime(x.ProgModifiedTs)).ToList();
                        lvMachinePrograms.DataBind();
                        res = true;
                    }
                }
                else
                {
                    Session["ProgramsFromCNC_SortOrder"] = order = "Desc";
                    if (type.Equals("prg", StringComparison.OrdinalIgnoreCase))
                    {
                        lvMachinePrograms.DataSource = list.OrderByDescending(x => x.ProgNo).ToList();
                        lvMachinePrograms.DataBind();
                        res = true;
                    }
                    else
                    {
                        lvMachinePrograms.DataSource = list.OrderByDescending(x => Convert.ToDateTime(x.ProgModifiedTs)).ToList();
                        lvMachinePrograms.DataBind();
                        res = true;
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return res;
        }
        #endregion
    }
    public class DatabaseResourceManager : ResourceManager
    {
        private string connectionString;
        private Dictionary<string, DatabaseResourceSet> resources;

        public DatabaseResourceManager(string dsn)
        {
            this.connectionString = dsn;
            this.resources = new Dictionary<string, DatabaseResourceSet>();
        }

        protected override ResourceSet InternalGetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
        {
            DatabaseResourceSet rs = null;
            if (this.resources.ContainsKey(culture.Name))
            {
                rs = this.resources[culture.Name];
            }
            else
            {
                rs = new DatabaseResourceSet(connectionString, culture);
                this.resources.Add(culture.Name, rs);
            }
            return rs;
        }
    }
    public class DatabaseResourceReader : IResourceReader
    {
        private string connectionString;
        private string language;

        public DatabaseResourceReader(string dsn, CultureInfo culture)
        {
            this.connectionString = dsn;
            this.language = culture.Name;
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            SqlConnection conn = new SqlConnection(HttpContext.Current.Session["connectionString"].ToString());
            //conn = ConnectionManager.GetConnection();
            SqlCommand command = null;
            int count = 0;
            string query = "SELECT [ClassName]+[Key] as [Key],[" + language + "] " + "FROM MultiLingual_MC ";
            try
            {
                conn.Open();
                command = new SqlCommand(query, conn);
                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetValue(1) != System.DBNull.Value)
                        dict.Add(reader.GetString(0), reader.GetString(1));
                    count++;
                }
                reader.Close();
            }
            catch (Exception exx)   // ignore missing columns in the database
            {
                Logger.WriteErrorLog(exx.ToString());
            }
            finally
            {
                conn.Close();
            }
            return dict.GetEnumerator();
        }
        public void Close()
        {
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void IDisposable.Dispose()
        {
        }
    }
    public class DatabaseResourceSet : ResourceSet
    {
        internal DatabaseResourceSet(string dsn, CultureInfo culture)
            : base(new DatabaseResourceReader(dsn, culture))
        {
        }

        public override Type GetDefaultReader()
        {
            return typeof(DatabaseResourceReader);
        }
    }
    public static class Translations
    {
        public static CultureInfo cultureInfo;
        public static string conString = HttpContext.Current.Session["connectionString"].ToString();
        private static readonly DatabaseResourceManager databaseResourceManager = new DatabaseResourceManager(conString);
        public static string lang = WebConfigurationManager.AppSettings["language"].ToString();
        public static List<string> DBLanguages = new List<string>();

        static Translations()
        {
            lang = WebConfigurationManager.AppSettings["language"].ToString();
            DBLanguages = DataBaseAccess.GetLanguages();
            if (DBLanguages.Contains(lang.ToLower()))
            {
                cultureInfo = new CultureInfo(lang);
            }
            else
            {
                cultureInfo = new CultureInfo("EN");
            }
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

        public static string GetString(string identifier)
        {
            string word = databaseResourceManager.GetString(identifier, cultureInfo);
            return word;
        }
    }
}