using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard
{
    public partial class UpdateResourceFiles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindResourceFile();
            }
        }
        private void BindResourceFile()
        {
            ddlPageName.Items.Clear();
            string resourcespath = string.Empty;
            List<string> data = new List<string>();
            //data.Add(Request.PhysicalApplicationPath + "App_GlobalResources");
            data.Add(Request.PhysicalApplicationPath + "App_LocalResources");

            foreach (string item in data)
            {
                resourcespath = item;
                string[] result;
                DirectoryInfo dirInfo = new DirectoryInfo(resourcespath);
                foreach (FileInfo filInfo in dirInfo.GetFiles())
                {
                    if (filInfo.Extension == ".resx")
                    {
                        string filename = filInfo.Name;
                        result = filename.Split(new string[] { "." }, StringSplitOptions.None);
                        if (ddlLanguage.SelectedValue.ToString() == "zh")
                        {
                            if (result.Contains("zh"))
                            {
                                //if (result[2].Equals(ddlLanguage.SelectedValue.ToString(), StringComparison.OrdinalIgnoreCase))
                                ddlPageName.Items.Add(filename);
                            }
                        } if (ddlLanguage.SelectedValue.ToString() == "en")
                        {
                            if (!result.Contains("zh"))
                            {
                                ddlPageName.Items.Add(filename);
                            }
                        }
                    }
                }
            }

            ddlPageName.Items.Insert(0, GetGlobalResourceObject("CommanResource", "ALL").ToString());
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Hashtable data = new Hashtable();
            for (int i = 0; i < gridViewResource.Rows.Count; i++)
            {
                Label lblKey = (Label)gridViewResource.Rows[i].FindControl("lblKey");
                TextBox txtValue = ((TextBox)gridViewResource.Rows[i].FindControl("txtValue"));
                HiddenField hdfCondition = ((HiddenField)gridViewResource.Rows[i].FindControl("hdfCondition"));
                if (hdfCondition.Value == "update")
                    data.Add(lblKey.Text, txtValue.Text);
            }
            string filename = ddlPageName.SelectedValue.ToString();
            string[] result = ddlPageName.SelectedValue.Split(new string[] { "." }, StringSplitOptions.None);
            //if (!result.Contains("CommanResource"))
            //{
            //    filename = Request.PhysicalApplicationPath + "App_LocalResources\\" + filename;
            //}
            //else
            //{
            //filename = Request.PhysicalApplicationPath + "App_GlobalResources\\" + filename;
            //}
            filename = Request.PhysicalApplicationPath + "App_LocalResources\\" + filename;
            UpdateResourceFile(data, filename);
           // ShowData();
            lblMessages.Text = "Resource File Updated...";
        }

        protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindResourceFile();
        }

        protected void ddlPageName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPageName.SelectedIndex != 0)
            {
                ShowData();
            }
        }

        private void ShowData()
        {
            string filename = string.Empty;
            string[] result = ddlPageName.SelectedValue.Split(new string[] { "." }, StringSplitOptions.None);
            //if (!result.Contains("CommanResource"))
            //{
            //    filename = Request.PhysicalApplicationPath +
            //     "App_LocalResources\\" + ddlPageName.SelectedValue.ToString();
            //}
            //else
            //{
            //    filename = Request.PhysicalApplicationPath +
            //     "App_GlobalResources\\" + ddlPageName.SelectedValue.ToString();
            //}
            filename = Request.PhysicalApplicationPath +
                "App_LocalResources\\" + ddlPageName.SelectedValue.ToString();
            Stream stream = new FileStream(filename, FileMode.Open,
                FileAccess.Read, FileShare.Read);
            ResXResourceReader RrX = new ResXResourceReader(stream);
            IDictionaryEnumerator RrEn = RrX.GetEnumerator();
            SortedList slist = new SortedList();
            while (RrEn.MoveNext())
            {
                slist.Add(RrEn.Key, RrEn.Value);
            }
            RrX.Close();
            stream.Dispose();
            gridViewResource.DataSource = slist;
            gridViewResource.DataBind();
        }


        public static void UpdateResourceFile(Hashtable data, String path)
        {
            Hashtable resourceEntries = new Hashtable();
            //Get existing resources //UpdatableResXResourceProvider
            ResXResourceReader reader = new ResXResourceReader(path);
            if (reader != null)
            {
                IDictionaryEnumerator id = reader.GetEnumerator();
                foreach (DictionaryEntry d in reader)
                {
                    if (d.Value == null)
                        resourceEntries.Add(d.Key.ToString(), "");
                    else
                        resourceEntries.Add(d.Key.ToString(), d.Value.ToString());
                } reader.Close();
            }
            //Modify resources here...
            foreach (String key in data.Keys)
            {
                if (!resourceEntries.ContainsKey(key))
                {
                    String value = data[key].ToString();
                    if (value == null)
                        value = "";
                    resourceEntries.Add(key, value);
                }
                else
                {
                    String value = data[key].ToString();
                    if (value == null)
                        value = "";
                    resourceEntries.Remove(key);
                    resourceEntries.Add(key, data[key].ToString());
                }
            }
            //Write the combined resource file
            ResXResourceWriter resourceWriter = new ResXResourceWriter(path);
            foreach (String key in resourceEntries.Keys)
            {
                resourceWriter.AddResource(key, resourceEntries[key]);
            }
            resourceWriter.Generate();
            resourceWriter.Close();
        }


    }
}