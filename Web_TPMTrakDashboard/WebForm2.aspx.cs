using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindAllResourceFile();
            }
        }

        private void BindAllResourceFile()
        {
            string resourcespath =
                 Request.PhysicalApplicationPath + "App_LocalResources";
            DirectoryInfo dirInfo = new DirectoryInfo(resourcespath);
            foreach (FileInfo filInfo in dirInfo.GetFiles())
            {
                if (filInfo.Extension == ".resx")
                {
                    string filename = filInfo.Name;
                    LanguageDropdownlist.Items.Add(filename);
                }
            }
            LanguageDropdownlist.Items.Insert(0, new ListItem("Select a Resource File"));
        }

        protected void LanguageDropdownlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LanguageDropdownlist.SelectedIndex != 0)
            {
                ShowData();
            }
        }


        private void ShowData()
        {
            string filename = Request.PhysicalApplicationPath +
                   "App_LocalResources\\" + LanguageDropdownlist.SelectedItem.Text;
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
            gridView1.DataSource = slist;
            gridView1.DataBind();
        }

        #region "Frist Wave Save Update Resource----------------------------"
        protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            //NewEditIndex property used to determine the index of the row being edited.  
            gridView1.EditIndex = e.NewEditIndex;
            ShowData();
        }
        protected void GridView1_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            //Finding the controls from Gridview for the row which is going to update  
            TextBox txtKey = gridView1.Rows[e.RowIndex].FindControl("txtKey") as TextBox;
            TextBox txtValue = gridView1.Rows[e.RowIndex].FindControl("txtValue") as TextBox;

            string filename = LanguageDropdownlist.SelectedValue.ToString();
            filename = Request.PhysicalApplicationPath + "App_LocalResources\\" + filename;

            Hashtable data = new Hashtable();
            data.Add(txtKey.Text, txtValue.Text);
            UpdateResourceFile(data, filename);

            Label2.Text = "Resource File Updated...";
            //Setting the EditIndex property to -1 to cancel the Edit mode in Gridview  
            gridView1.EditIndex = -1;
            //Call ShowData method for displaying updated data  
            ShowData();

        }
        protected void GridView1_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            //Setting the EditIndex property to -1 to cancel the Edit mode in Gridview  
            gridView1.EditIndex = -1;
            ShowData();
        }
        #endregion


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

        public static void UpdateResourceFile1(Hashtable data, String path)
        {
            Hashtable resourceEntries = new Hashtable();

            //Get existing resources
            ResXResourceReader reader = new ResXResourceReader(path);
            reader.UseResXDataNodes = true;
            ResXResourceWriter resourceWriter = new ResXResourceWriter(path);
            System.ComponentModel.Design.ITypeResolutionService typeres = null;
            if (reader != null)
            {
                IDictionaryEnumerator id = reader.GetEnumerator();
                foreach (DictionaryEntry d in reader)
                {
                    //Read from file:
                    string val = "";
                    if (d.Value == null)
                        resourceEntries.Add(d.Key.ToString(), "");
                    else
                    {
                        val = ((ResXDataNode)d.Value).GetValue(typeres).ToString();
                        resourceEntries.Add(d.Key.ToString(), val);

                    }

                    //Write (with read to keep xml file order)
                    ResXDataNode dataNode = (ResXDataNode)d.Value;

                    //resourceWriter.AddResource(d.Key.ToString(), val);
                    resourceWriter.AddResource(dataNode);

                }
                reader.Close();
            }

            //Add new data (at the end of the file):
            Hashtable newRes = new Hashtable();
            foreach (String key in data.Keys)
            {
                if (!resourceEntries.ContainsKey(key))
                {

                    String value = data[key].ToString();
                    if (value == null) value = "";

                    resourceWriter.AddResource(key, value);
                }
            }

            //Write to file
            resourceWriter.Generate();
            resourceWriter.Close();

        }

        public static void AddOrUpdateResource(string key, string value, string resourceFilepath, string ResxPathEn)
        {

            var resx = new List<DictionaryEntry>();
            using (var reader = new ResXResourceReader(resourceFilepath))
            {
                resx = reader.Cast<DictionaryEntry>().ToList();
                var existingResource = resx.Where(r => r.Key.ToString() == key).FirstOrDefault();
                if (existingResource.Key == null && existingResource.Value == null) // NEW!
                {
                    resx.Add(new DictionaryEntry() { Key = key, Value = value });
                }
                else // MODIFIED RESOURCE!
                {
                    var modifiedResx = new DictionaryEntry() { Key = existingResource.Key, Value = value };
                    resx.Remove(existingResource);  // REMOVING RESOURCE!
                    resx.Add(modifiedResx);  // AND THEN ADDING RESOURCE!
                }
            }
            using (var writer = new ResXResourceWriter(ResxPathEn))
            {
                resx.ForEach(r =>
                {
                    // Again Adding all resource to generate with final items
                    writer.AddResource(r.Key.ToString(), r.Value.ToString());
                });
                writer.Generate();
            }
        }

        //----------------------------Another Method-------------------------------//

        #region "Second Wave Save Update Resource Value----------"
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Hashtable data = new Hashtable();
            for (int i = 0; i < gridView1.Rows.Count; i++)
            {
                Label lblKey = (Label)gridView1.Rows[i].FindControl("lblKey");
                TextBox txtValue = ((TextBox)gridView1.Rows[i].FindControl("txtValue"));
                data.Add(lblKey.Text, txtValue.Text);
            }
            string filename = LanguageDropdownlist.SelectedValue.ToString();
            filename = Request.PhysicalApplicationPath + "App_LocalResources\\" + filename;
            UpdateResourceFile(data, filename);
            ShowData();
            Label2.Text = "Resource File Updated...";
        }
        #endregion
      
    }
}