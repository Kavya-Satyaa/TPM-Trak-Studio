using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.MachineConnect
{
    public partial class OperationHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindPlant();
                BindData();
            }
        }
        private void BindPlant()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlantsForPlantInfo();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();
                ddlPlant_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachine();
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = DataBaseAccess.getmachineidfromplant(ddlPlant.SelectedValue);
                if (list.Count > 0)
                {
                    list.Remove("All");
                }
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private string getMachineID()
        {
            string machineID = ddlMachine.SelectedValue;
            try
            {
                var spliData = machineID.Split('/');
                if (spliData.Length > 1)
                {
                    machineID = spliData[1];
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return machineID;
        }
        private void BindData()
        {
            try
            {
                btnReset_Click(null, null);

                TreeNode machineNode = new TreeNode
                {
                    Text = ddlMachine.SelectedValue,
                    Value = ddlMachine.SelectedValue
                };
                tvOperationHistoryDetails.Nodes.Add(machineNode);
                string folderPath = Path.Combine(WebConfigurationManager.AppSettings["OperationHistoryFilePath"].ToString(), getMachineID());
                DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
                if (directoryInfo.Exists)
                {
                    DateTime fromDateInDate = Util.GetDateTime(txtFromDate.Text);
                    DateTime toDateInDate = Util.GetDateTime(txtToDate.Text);
                    while (fromDateInDate <= toDateInDate)
                    {
                        string dateInStr = fromDateInDate.ToString("yyyy-MM-dd");
                        if (Directory.Exists(Path.Combine(directoryInfo.ToString(), dateInStr)))
                        {

                            TreeNode dateNode = new TreeNode
                            {
                                Text = dateInStr,
                                Value = dateInStr
                            };
                            machineNode.ChildNodes.Add(dateNode);

                            //fileexists = true;
                            DirectoryInfo nerDirInfo = new DirectoryInfo(directoryInfo + "\\" + dateInStr);

                            foreach (FileInfo file in nerDirInfo.GetFiles())
                            {
                                TreeNode fileNode = new TreeNode
                                {
                                    Text = file.Name,
                                    Value = file.FullName
                                };
                                dateNode.ChildNodes.Add(fileNode);
                            }
                        }
                        fromDateInDate = fromDateInDate.AddDays(1);
                    }
                }
                else
                {
                    HelperClassGeneric.openWarningModal(this, "Folder does not exists.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                tvOperationHistoryDetails.Nodes.Clear();
                opnFileHeader.InnerText = "";
                opnFileData.InnerHtml = "";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void tvOperationHistoryDetails_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                if (tvOperationHistoryDetails.SelectedNode.Parent != null)
                {
                    string pgm = Path.Combine(WebConfigurationManager.AppSettings["OperationHistoryFilePath"].ToString(), getMachineID(), tvOperationHistoryDetails.SelectedNode.Parent.Text);
                    DirectoryInfo directoryInfo = new DirectoryInfo(pgm);
                    if (directoryInfo.Exists)
                    {
                        string readText = File.ReadAllText(Path.Combine(pgm, tvOperationHistoryDetails.SelectedNode.Text));
                        readText = readText.Replace("<INPUT>", "&lt;INPUT&gt;");
                        readText = readText.Replace("\n", "<br />");
                        opnFileData.InnerHtml = readText;
                        opnFileHeader.InnerText = tvOperationHistoryDetails.SelectedNode.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void tvOperationHistoryDetails_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {

        }

    }
}