using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Nippon.DataBaseAccess;
using Web_TPMTrakDashboard.Nippon.Model;

namespace Web_TPMTrakDashboard.Nippon
{
    public partial class AndonSetting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
                setCompanyLogo();
            }
        }
        private void setCompanyLogo()
        {
            //const string imagesPath = "~/CompanyLogo/";// "~/Image/Slideshow/";
            //var dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(imagesPath));

            ////filtering to jpgs, but ideally not required
            //List<string> fileNames = (from flInfo in dir.GetFiles() select flInfo.Name).ToList();
            //if (fileNames.Count > 0)
            //{
            //    Image2.ImageUrl = imagesPath + fileNames[0];
            //}
            //else
            //{
            //    Image2.ImageUrl = "Image/companyIcon.png";
            //}
            Image2.ImageUrl = Web_TPMTrakDashboard.Models.Util.getCompanyLogoPath();
        }

        private void BindData()
        {
            try
            {
                List<AndonSettingData> listMachineDetails = new List<AndonSettingData>();
                listMachineDetails = DBAccess.getAndonMachineSettingDetails();
                List<AndonSettingData> listBackImgDetails = new List<AndonSettingData>();
                //listBackImgDetails = DBAccess.getAndonSettingDetails();
                List<AndonSettingData> listAndonDetails = DBAccess.getAndonSettingDetails();
                if (listAndonDetails.Count > 0)
                {
                    //listMachineDetails = listAndonDetails.Where(k => k.Parameter == "NipponAndonMachineSetting").ToList();
                    listBackImgDetails = listAndonDetails.Where(k => k.Parameter == "NipponAndonSetting" && k.ValueInText == "BackgroundImage").ToList();
                }
                lvMachineDetails.DataSource = listMachineDetails;
                lvMachineDetails.DataBind();
                if (listBackImgDetails.Count > 0)
                {
                    backGroundImg.ImageUrl = listBackImgDetails[0].ImagePath;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void applyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                List<AndonSettingData> listMachineDetails = new List<AndonSettingData>();
                AndonSettingData data = null;
                //Insert Machine Details
                for (int i = 0; i < lvMachineDetails.Items.Count; i++)
                {
                    data = new AndonSettingData();
                    data.MachineID = (lvMachineDetails.Items[i].FindControl("lblMachineID") as Label).Text;
                    
                    // data.SortOrder = (lvMachineDetails.Items[i].FindControl("txtSortOrder") as TextBox).Text;
                    FileUpload fileUpload = (lvMachineDetails.Items[i].FindControl("fileUpload") as FileUpload);
                    if (fileUpload.HasFile)
                    {
                        DirectoryInfo diInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Nippon/AndonMachineImages/"));
                        FileInfo[] files = diInfo.GetFiles();
                        for (int j = 0; j < files.Length; j++)
                        {
                            if (files[j].Name.Contains(data.MachineID + "_"))
                            {
                                string filePath = HttpContext.Current.Server.MapPath("~/Nippon/AndonMachineImages/" + files[j].ToString());
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                }
                                break;
                            }
                        }
                        foreach (HttpPostedFile postedFile in fileUpload.PostedFiles)
                        {
                            string fileName = Path.GetFileName(postedFile.FileName);
                            data.ImageName = data.MachineID + "_" + fileName;
                            postedFile.SaveAs(Server.MapPath("~/Nippon/AndonMachineImages/") + data.ImageName);
                        }
                    }
                    DBAccess.saveAndonMachineDetails(data);
                }

                //Insert background image
                data = new AndonSettingData();
                if (bgFileUpload.HasFile)
                {
                    DirectoryInfo diInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Nippon/AndonMachineImages/"));
                    FileInfo[] files = diInfo.GetFiles();
                    for (int j = 0; j < files.Length; j++)
                    {
                        if (files[j].Name.Contains("BackgroundImg_"))
                        {
                            string filePath = HttpContext.Current.Server.MapPath("~/Nippon/AndonMachineImages/" + files[j].ToString());
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                        }
                        break;
                    }
                    foreach (HttpPostedFile postedFile in bgFileUpload.PostedFiles)
                    {
                        string fileName = Path.GetFileName(postedFile.FileName);
                        data.ImageName = "BackgroundImg" + "_" + fileName;
                        postedFile.SaveAs(Server.MapPath("~/Nippon/AndonMachineImages/") + data.ImageName);
                        DBAccess.saveAndonBackgroungImageDetails(data);
                    }
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "bindNoOfRows", "successMsg('Data Saved Successfully','');", true);
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void homeBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("NipponEffeciencyAndon.aspx");
        }
    }
}