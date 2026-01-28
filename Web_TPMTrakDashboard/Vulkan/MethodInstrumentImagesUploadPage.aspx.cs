using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Vulkan.Models;

namespace Web_TPMTrakDashboard.Vulkan
{
    public partial class MethodInstrumentImagesUploadPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDataGrids();
            }
        }

        private void BindDataGrids()
        {
            List<MetInsEntity> list = new List<MetInsEntity>();
            try
            {
                list = DataBaseAccessVulkan.GetMetInsData("Method");

                gvMethod.DataSource = list;
                gvMethod.DataBind();

                list = DataBaseAccessVulkan.GetMetInsData("Instrument");
                gvInstrument.DataSource = list;
                gvInstrument.DataBind();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string path = Server.MapPath("~/Vulkan/Images");
            try
            {
                if ((sender as Button).ID.ToString().Equals("btnMethodSave", StringComparison.OrdinalIgnoreCase))
                {
                    if (fuMethodImage.HasFiles)
                    {
                        foreach (HttpPostedFile file in fuMethodImage.PostedFiles) 
                        {
                          if(Path.GetExtension(file.FileName.ToString().Trim()) == ".jpg" ||Path.GetExtension(file.FileName.ToString().Trim()) == ".png" ||Path.GetExtension(file.FileName.ToString().Trim()) == ".jpeg")
                            {
                                if (!Directory.Exists(path + "/Method"))
                                {
                                    Directory.CreateDirectory(path + "/Method");
                                }
                                file.SaveAs(path + "/Method/" + file.FileName.ToString());
                                if (file.ContentLength > 512000)
                                {
                                    HelperClassGeneric.openWarningModal(this, "File size cannot be more than 5kb");
                                    return;
                                }

                                byte[] imgBytes = new byte[file.ContentLength];
                               
                                string fileName = System.IO.Path.GetFileName(file.FileName);
                                Stream fs = file.InputStream;
                                BinaryReader br = new BinaryReader(fs); //reads the binary files  
                                imgBytes = br.ReadBytes((Int32)fs.Length);

                                Guid guid = Guid.NewGuid();
                                DataBaseAccessVulkan.SaveMethodAndInstrumentMasterData("Method " + file.FileName.ToString(), imgBytes, guid.ToString());
                            }
                        }

                    }
                }
                else if ((sender as Button).ID.ToString().Equals("btnSaveInstrument", StringComparison.OrdinalIgnoreCase))
                {
                    if (fuInstrumentImage.HasFiles)
                    {
                        foreach (HttpPostedFile file in fuInstrumentImage.PostedFiles)
                        {
                            string extension = Path.GetExtension(file.FileName.ToString().Trim());
                            if (Path.GetExtension(file.FileName.ToString().Trim()) == ".jpg" || Path.GetExtension(file.FileName.ToString().Trim()) == ".png" || Path.GetExtension(file.FileName.ToString().Trim()) == ".jpeg")
                            {
                                if (!Directory.Exists(path + "/Instrument"))
                                {
                                    Directory.CreateDirectory(path + "/Instrument");
                                }
                                file.SaveAs(path + "/Instrument/" + file.FileName.ToString());

                                byte[] imgBytes = new byte[file.ContentLength];

                                string fileName = System.IO.Path.GetFileName(file.FileName);
                                Stream fs = file.InputStream;
                                BinaryReader br = new BinaryReader(fs); //reads the binary files  
                                imgBytes = br.ReadBytes((Int32)fs.Length);

                                DataBaseAccessVulkan.SaveMethodAndInstrumentMasterData("Instrument "+ file.FileName.ToString(), imgBytes, Guid.NewGuid().ToString());
                            }
                        }

                    }
                }

                BindDataGrids();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string RefID = ((sender as GridViewRow).FindControl("hdnRefID") as HiddenField).Value.ToString().Trim();

                //DataBaseAccessVulkan.DeleteImageData()
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

    }
}