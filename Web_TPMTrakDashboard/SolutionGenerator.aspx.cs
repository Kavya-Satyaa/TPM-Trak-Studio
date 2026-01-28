using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Web_TPMTrakDashboard
{
    public partial class SolutionGenerator : System.Web.UI.Page
    {
        [Serializable]
        public class GridColumnDefinition
        {
            public string Name { get; set; }
            public string DataType { get; set; }
            public string ControlType { get; set; }
            public bool IsKey { get; set; }
        }

        public class ActionButtonSettings
        {
            public bool EnableExport { get; set; }
            public string ExportFormat { get; set; }
            public bool EnableImport { get; set; }
        }

        [Serializable]
        public class FilterConfiguration
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public bool Enabled { get; set; }
            public string Mode { get; set; } // Single, Multi, SingleDate, DateRange
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            string pageTitle = txtPageTitle.Text.Trim();
            if (string.IsNullOrEmpty(pageTitle)) pageTitle = "New Screen Preview";

            Session["Preview_Title"] = pageTitle;
            Session["Preview_Visibility"] = GetFilterConfig(); // Kept key name for compatibility
            Session["Preview_GridColumns"] = GetGridSettings();
            Session["Preview_ActionSettings"] = new ActionButtonSettings
            {
                EnableExport = chkExport.Checked,
                ExportFormat = rblExportFormat.SelectedValue,
                EnableImport = chkImport.Checked
            };

            // Open in new tab
            string script = "window.open('SolutionPreview.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, GetType(), "OpenPreview", script, true);
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            string pageName = txtPageName.Text.Trim();
            string pageTitle = txtPageTitle.Text.Trim();
            string customPublishPath = txtPublishPath.Text.Trim();

            if (string.IsNullOrEmpty(pageName))
            {
                LogError("Page Name is required.");
                return;
            }

            if (!Regex.IsMatch(pageName, @"^[a-zA-Z][a-zA-Z0-9_]*$"))
            {
                LogError("Invalid Page Name. Use alphanumeric characters starting with a letter.");
                return;
            }

            try
            {
                List<FilterConfiguration> filters = GetFilterConfig();
                List<GridColumnDefinition> columns = GetGridSettings();
                ActionButtonSettings actionSettings = new ActionButtonSettings
                {
                    EnableExport = chkExport.Checked,
                    ExportFormat = rblExportFormat.SelectedValue,
                    EnableImport = chkImport.Checked
                };

                GenerateSolution(pageName, pageTitle, customPublishPath, filters, columns, actionSettings);
            }
            catch (Exception ex)
            {
                LogError("Error during generation: " + ex.Message + (ex.InnerException != null ? " (" + ex.InnerException.Message + ")" : ""));
            }
        }

        protected void btnDownloadAspx_Click(object sender, EventArgs e)
        {
            string pageName = txtPageName.Text.Trim();
            string pageTitle = txtPageTitle.Text.Trim();

            if (string.IsNullOrEmpty(pageName))
            {
                LogError("Page Name is required.");
                return;
            }

            try
            {
                List<FilterConfiguration> filters = GetFilterConfig();
                List<GridColumnDefinition> columns = GetGridSettings();
                ActionButtonSettings actionSettings = new ActionButtonSettings
                {
                    EnableExport = chkExport.Checked,
                    ExportFormat = rblExportFormat.SelectedValue,
                    EnableImport = chkImport.Checked
                };

                DownloadAspxPackage(pageName, pageTitle, filters, columns, actionSettings);
            }
            catch (Exception ex)
            {
                LogError("Error during ASPX download: " + ex.Message);
            }
        }

        private void DownloadAspxPackage(string pageName, string pageTitle, List<FilterConfiguration> filters, List<GridColumnDefinition> columns, ActionButtonSettings actionSettings)
        {
            string aspxContent = GetAspxTemplate(pageName, pageTitle, actionSettings, filters);
            string csContent = GetCsTemplate(pageName, filters, columns, actionSettings);
            string designerContent = GetDesignerTemplate(pageName);

            string solutionRoot = @"C:\TPM-Trak Studio";
            string dbAccessPath = Path.Combine(solutionRoot, "Web_TPMTrakDashboard", "Models", "TPMStudioDBAccess.cs");
            string dbAccessContent = File.Exists(dbAccessPath) ? File.ReadAllText(dbAccessPath) : "";

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var aspxFile = archive.CreateEntry(pageName + ".aspx");
                    using (var entryStream = aspxFile.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        streamWriter.Write(aspxContent);
                    }

                    var csFile = archive.CreateEntry(pageName + ".aspx.cs");
                    using (var entryStream = csFile.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        streamWriter.Write(csContent);
                    }

                    var designerFile = archive.CreateEntry(pageName + ".aspx.designer.cs");
                    using (var entryStream = designerFile.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        streamWriter.Write(designerContent);
                    }

                    if (!string.IsNullOrEmpty(dbAccessContent))
                    {
                        var dbFile = archive.CreateEntry("TPMStudioDBAccess.cs");
                        using (var entryStream = dbFile.Open())
                        using (var streamWriter = new StreamWriter(entryStream))
                        {
                            streamWriter.Write(dbAccessContent);
                        }
                    }
                }

                Response.Clear();
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + pageName + "_ASPX_Only.zip");
                Response.BinaryWrite(memoryStream.ToArray());
                Response.End();
            }
        }

        private List<GridColumnDefinition> GetGridSettings()
        {
            var configJson = hdnGridConfig.Value;
            if (string.IsNullOrEmpty(configJson)) return new List<GridColumnDefinition>();

            try
            {
                // Simple JSON parsing if no library is available, 
                // but since this is ASP.NET, JavaScriptSerializer or Json.NET might be used.
                // For safety in this environment, I'll use a basic approach.
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                return serializer.Deserialize<List<GridColumnDefinition>>(configJson);
            }
            catch
            {
                return new List<GridColumnDefinition>();
            }
        }

        private List<FilterConfiguration> GetFilterConfig()
        {
            return new List<FilterConfiguration>
            {
                new FilterConfiguration { ID = "Ddl1", Name = "Plant", Enabled = chkDdl1.Checked, Mode = rblMode1.SelectedValue },
                new FilterConfiguration { ID = "Ddl2", Name = "Machine", Enabled = chkDdl2.Checked, Mode = rblMode2.SelectedValue },
                new FilterConfiguration { ID = "Ddl3", Name = "Component", Enabled = chkDdl3.Checked, Mode = rblMode3.SelectedValue },
                new FilterConfiguration { ID = "Ddl4", Name = "Operation", Enabled = chkDdl4.Checked, Mode = rblMode4.SelectedValue },
                new FilterConfiguration { ID = "Ddl5", Name = "Shift", Enabled = chkDdl5.Checked, Mode = rblMode5.SelectedValue },
                new FilterConfiguration { ID = "Date", Name = "Date", Enabled = chkDate.Checked, Mode = rblModeDate.SelectedValue }
            };
        }

        private void GenerateSolution(string pageName, string pageTitle, string customPublishPath, List<FilterConfiguration> filters, List<GridColumnDefinition> columns, ActionButtonSettings actionSettings)
        {
            // 1. Identify Paths
            string solutionRoot = @"C:\TPM-Trak Studio";
            string projectDir = Path.Combine(solutionRoot, "Web_TPMTrakDashboard");

            // Handle Import Template
            if (actionSettings.EnableImport && fuImportTemplate.HasFile)
            {
                string tempDir = Path.Combine(projectDir, "Temp", "ImportTemplate");
                if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);
                string filePath = Path.Combine(tempDir, fuImportTemplate.FileName);
                fuImportTemplate.SaveAs(filePath);
            }

            // Paths for build and artifact generation
            string tempBase = Path.Combine(solutionRoot, "GeneratedSolutions", Guid.NewGuid().ToString("N"));
            string publishOutput = Path.Combine(tempBase, "Publish");
            string zipFile = Path.Combine(tempBase, pageName + "_Published.zip");

            Directory.CreateDirectory(tempBase);
            Directory.CreateDirectory(publishOutput);

            // 2. Prepare Temp/Output Directory for Source (for generation) - Site it inside the Source Project
            string tempProjectDir = Path.Combine(projectDir, "GeneratedSolutions", pageName);
            if (Directory.Exists(tempProjectDir)) Directory.Delete(tempProjectDir, true);
            Directory.CreateDirectory(tempProjectDir);

            // 3. Generate Files
            string aspxPath = Path.Combine(tempProjectDir, pageName + ".aspx");
            string csPath = Path.Combine(tempProjectDir, pageName + ".aspx.cs");
            string designerPath = Path.Combine(tempProjectDir, pageName + ".aspx.designer.cs");

            File.WriteAllText(aspxPath, GetAspxTemplate(pageName, pageTitle, actionSettings, filters));
            File.WriteAllText(csPath, GetCsTemplate(pageName, filters, columns, actionSettings));
            File.WriteAllText(designerPath, GetDesignerTemplate(pageName));

            // 4. Update .csproj
            string csprojPath = Path.Combine(projectDir, "Web_TPMTrakDashboard.csproj");
            string relativePath = "GeneratedSolutions\\" + pageName + "\\" + pageName + ".aspx";
            UpdateCsproj(csprojPath, relativePath);

            // 5. Build & Publish
            BuildProject(csprojPath, publishOutput);

            // 6. Optional: Copy to custom path
            if (!string.IsNullOrEmpty(customPublishPath))
            {
                try
                {
                    if (!Directory.Exists(customPublishPath)) Directory.CreateDirectory(customPublishPath);
                    CopyDirectory(publishOutput, customPublishPath);
                }
                catch (Exception ex)
                {
                    litStatus.Text += "<br/>Warning: Failed to copy to custom path: " + ex.Message;
                }
            }

            // 7. Zip Output
            ZipFile.CreateFromDirectory(publishOutput, zipFile);

            // 8. Download
            DownloadZip(zipFile, pageName + "_Published.zip");
        }

        private void UpdateCsproj(string csprojPath, string relativePath)
        {
            string pageName = Path.GetFileNameWithoutExtension(relativePath);
            string dir = Path.GetDirectoryName(relativePath);
            string csRel = Path.Combine(dir, pageName + ".aspx.cs");
            string designerRel = Path.Combine(dir, pageName + ".aspx.designer.cs");

            XmlDocument doc = new XmlDocument();
            doc.Load(csprojPath);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ms", "http://schemas.microsoft.com/developer/msbuild/2003");

            // Check if already exists to avoid duplicates
            if (doc.SelectSingleNode($"//ms:Content[@Include='{relativePath}']", nsmgr) != null) return;

            XmlNode itemGroup = doc.SelectSingleNode("//ms:ItemGroup[ms:Content]", nsmgr);
            if (itemGroup == null)
            {
                itemGroup = doc.CreateElement("ItemGroup", "http://schemas.microsoft.com/developer/msbuild/2003");
                doc.DocumentElement.AppendChild(itemGroup);
            }

            // Add Content
            XmlElement contentNode = doc.CreateElement("Content", "http://schemas.microsoft.com/developer/msbuild/2003");
            contentNode.SetAttribute("Include", relativePath);
            itemGroup.AppendChild(contentNode);

            // Add Compile
            XmlNode compileGroup = doc.SelectSingleNode("//ms:ItemGroup[ms:Compile]", nsmgr);

            // Codebehind
            XmlElement compileNode = doc.CreateElement("Compile", "http://schemas.microsoft.com/developer/msbuild/2003");
            compileNode.SetAttribute("Include", csRel);
            XmlElement depNode = doc.CreateElement("DependentUpon", "http://schemas.microsoft.com/developer/msbuild/2003");
            depNode.InnerText = pageName + ".aspx";
            compileNode.AppendChild(depNode);
            XmlElement subTypeNode = doc.CreateElement("SubType", "http://schemas.microsoft.com/developer/msbuild/2003");
            subTypeNode.InnerText = "ASPXCodeBehind";
            compileNode.AppendChild(subTypeNode);
            compileGroup.AppendChild(compileNode);

            // Designer
            XmlElement designerNode = doc.CreateElement("Compile", "http://schemas.microsoft.com/developer/msbuild/2003");
            designerNode.SetAttribute("Include", designerRel);
            XmlElement depNode2 = doc.CreateElement("DependentUpon", "http://schemas.microsoft.com/developer/msbuild/2003");
            depNode2.InnerText = pageName + ".aspx";
            designerNode.AppendChild(depNode2);
            compileGroup.AppendChild(designerNode);

            doc.Save(csprojPath);
        }

        private void BuildProject(string csprojPath, string publishOutput)
        {
            string[] searchPaths = {
                @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe",
                @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe",
                @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
                @"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
                @"C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe",
                @"C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
            };

            string msbuildPath = searchPaths.FirstOrDefault(p => File.Exists(p));

            if (!File.Exists(msbuildPath)) throw new Exception("MSBuild.exe not found. Please ensure Microsoft Visual Studio 2019 or 2022 is installed.");

            string vsVersion = msbuildPath.Contains("2019") ? "16.0" : "17.0";

            // Generate temp paths for build artifacts
            string tempBase = Path.GetDirectoryName(publishOutput);
            string tempObj = Path.Combine(tempBase, "obj");
            string tempBin = Path.Combine(tempBase, "bin");

            // Calculate relative paths to avoid issues with single quotes in the absolute path (MSB4023)
            string projectFolder = Path.GetDirectoryName(csprojPath);
            if (!projectFolder.EndsWith("\\")) projectFolder += "\\";

            Uri fromUri = new Uri(projectFolder);
            string relPublish = Uri.UnescapeDataString(fromUri.MakeRelativeUri(new Uri(publishOutput.TrimEnd('\\') + "\\")).ToString()).Replace("/", "\\").TrimEnd('\\');
            string relObj = Uri.UnescapeDataString(fromUri.MakeRelativeUri(new Uri(tempObj.TrimEnd('\\') + "\\")).ToString()).Replace("/", "\\").TrimEnd('\\');
            string relBin = Uri.UnescapeDataString(fromUri.MakeRelativeUri(new Uri(tempBin.TrimEnd('\\') + "\\")).ToString()).Replace("/", "\\").TrimEnd('\\');

            string args = $"\"{csprojPath}\" /p:Configuration=Release /p:DeployOnBuild=true /p:PublishUrl=\"{relPublish}\\\\\" /p:WebPublishMethod=FileSystem /p:DeployTarget=WebPublish /p:VisualStudioVersion={vsVersion} /p:BaseIntermediateOutputPath=\"{relObj}\\\\\" /p:OutputPath=\"{relBin}\\\\\" /v:m /nologo";

            ProcessStartInfo psi = new ProcessStartInfo(msbuildPath, args)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process p = Process.Start(psi))
            {
                string output = p.StandardOutput.ReadToEnd();
                string error = p.StandardError.ReadToEnd();
                p.WaitForExit();

                if (p.ExitCode != 0)
                {
                    string relevantError = "";
                    var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    var errorLines = lines.Where(l => l.Contains("error ") || l.Contains("Error ")).ToList();
                    relevantError = errorLines.Count > 0 ? string.Join("\n", errorLines.Take(5)) : error;

                    throw new Exception("Build failed. " + relevantError + "\n\nLog Summary:\n" + (output.Length > 1000 ? output.Substring(output.Length - 1000) : output));
                }
            }
        }

        private void DownloadZip(string zipPath, string fileName)
        {
            Response.Clear();
            Response.ContentType = "application/zip";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.WriteFile(zipPath);
            Response.Flush();
            Response.End();
        }

        private void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                string target = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, target);
            }
            foreach (var directory in Directory.GetDirectories(sourceDir))
            {
                string dirName = Path.GetFileName(directory).ToLower();
                if (dirName == ".git" || dirName == ".vs" || dirName == "obj" || dirName == "bin" || dirName == "node_modules") continue;
                CopyDirectory(directory, Path.Combine(destDir, Path.GetFileName(directory)));
            }
        }

        private void LogError(string msg)
        {
            statusBox.Style["display"] = "block";
            statusBox.Attributes["class"] = "status-msg status-error";
            litStatus.Text = msg;
        }

        private string GetAspxTemplate(string className, string pageTitle, ActionButtonSettings actionSettings, List<FilterConfiguration> filters)
        {
            string actionButtons = "";
            if (actionSettings.EnableExport)
            {
                actionButtons += $@"                <asp:Button ID=""btnExport"" runat=""server"" Text=""Export to {actionSettings.ExportFormat}"" CssClass=""btn-view"" style=""background: #10b981; margin-left:10px;"" OnClick=""btnExport_Click"" />" + Environment.NewLine;
            }
            if (actionSettings.EnableImport)
            {
                actionButtons += $@"                <asp:Button ID=""btnImport"" runat=""server"" Text=""Import Excel"" CssClass=""btn-view"" style=""background: #f59e0b; margin-left:10px;"" OnClick=""btnImport_Click"" />" + Environment.NewLine;
            }

            StringBuilder filtersHtml = new StringBuilder();
            foreach (var filter in filters)
            {
                if (!filter.Enabled) continue;

                filtersHtml.AppendLine($@"                <div id=""pnl{filter.ID}"" runat=""server"" class=""dropdown-container{(filter.Mode == "DateRange" ? " filter-span-2" : "")}"">");
                filtersHtml.AppendLine($@"                    <label>{filter.Name}</label>");

                if (filter.Mode == "Single")
                {
                    filtersHtml.AppendLine($@"                    <asp:DropDownList ID=""ddl{filter.ID}"" runat=""server"" CssClass=""dropdown-control"" AutoPostBack=""true"" OnSelectedIndexChanged=""Filter_Changed"" />");
                }
                else if (filter.Mode == "Multi")
                {
                    filtersHtml.AppendLine($@"                    <asp:ListBox ID=""lb{filter.ID}"" runat=""server"" SelectionMode=""Multiple"" CssClass=""dropdown-control"" AutoPostBack=""true"" OnSelectedIndexChanged=""Filter_Changed"" />");
                }
                else if (filter.ID == "Date")
                {
                    if (filter.Mode == "SingleDate")
                    {
                        filtersHtml.AppendLine($@"                    <asp:TextBox ID=""txtDate"" runat=""server"" TextMode=""Date"" CssClass=""dropdown-control"" />");
                    }
                    else if (filter.Mode == "DateRange")
                    {
                        filtersHtml.AppendLine($@"                    <div class=""date-range-container"">");
                        filtersHtml.AppendLine($@"                        <asp:TextBox ID=""txtFromDate"" runat=""server"" TextMode=""Date"" CssClass=""dropdown-control"" />");
                        filtersHtml.AppendLine($@"                        <asp:TextBox ID=""txtToDate"" runat=""server"" TextMode=""Date"" CssClass=""dropdown-control"" />");
                        filtersHtml.AppendLine($@"                    </div>");
                    }
                }

                filtersHtml.AppendLine($@"                </div>");
            }

            return $@"<%@ Page Language=""C#"" AutoEventWireup=""true"" CodeBehind=""{className}.aspx.cs"" Inherits=""Web_TPMTrakDashboard.{className}"" %>
<!DOCTYPE html>
<html>
<head runat=""server"">
    <title>{pageTitle}</title>
    <link href=""https://fonts.googleapis.com/css2?family=Outfit:wght@300;400;600&display=swap"" rel=""stylesheet"" />
    <style>
        :root {{
            --primary: #6366f1;
            --primary-hover: #4f46e5;
            --bg: #0f172a;
            --glass-bg: rgba(30, 41, 59, 0.7);
            --glass-border: rgba(255, 255, 255, 0.1);
            --text: #f8fafc;
            --text-dim: #94a3b8;
        }}

        body {{ 
            font-family: 'Outfit', sans-serif; 
            padding: 50px; 
            background: radial-gradient(circle at top right, #1e1b4b, #0f172a);
            color: var(--text);
            min-height: 100vh;
            margin: 0;
        }}

        .card {{ 
            background: var(--glass-bg); 
            backdrop-filter: blur(12px);
            padding: 40px; 
            border-radius: 24px; 
            border: 1px solid var(--glass-border);
            box-shadow: 0 25px 50px -12px rgba(0,0,0,0.5); 
            max-width: 1000px; 
            width: 100%;
            margin: auto; 
            animation: slideUp 0.6s ease-out;
        }}

        @keyframes slideUp {{
            from {{ opacity: 0; transform: translateY(20px); }}
            to {{ opacity: 1; transform: translateY(0); }}
        }}

        h1 {{ 
            margin-top: 0;
            background: linear-gradient(to right, #818cf8, #c084fc);
            -webkit-background-clip: text;
            background-clip: text;
            -webkit-text-fill-color: transparent;
            font-size: 2.2rem;
            font-weight: 600;
        }}

        .subtitle {{ color: var(--text-dim); margin-bottom: 2rem; }}

        .controls-grid {{
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
            gap: 1.25rem;
            margin-bottom: 2rem;
            align-items: flex-end;
        }}

        .dropdown-container {{
            display: flex;
            flex-direction: column;
            gap: 0.5rem;
        }}

        label {{
            font-size: 0.85rem;
            color: var(--text-dim);
            font-weight: 500;
            text-transform: uppercase;
            letter-spacing: 0.05em;
        }}

        .dropdown-control {{
            width: 100%;
            padding: 0.75rem 1rem;
            background: rgba(15, 23, 42, 0.4);
            border: 1px solid var(--glass-border);
            border-radius: 12px;
            color: var(--text);
            font-family: inherit;
            font-size: 1rem;
            outline: none;
            transition: all 0.2s;
            cursor: pointer;
        }}

        .dropdown-control:focus {{
            border-color: var(--primary);
            box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.2);
            background-color: rgba(15, 23, 42, 0.6);
        }}

        .btn-view {{
            background: var(--primary);
            color: white;
            border: none;
            padding: 0.85rem 2rem;
            border-radius: 12px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s;
        }}

        .btn-view:hover {{ background: var(--primary-hover); transform: translateY(-2px); }}

        .grid-container {{
            margin-top: 2rem;
            overflow-x: auto;
            border-radius: 12px;
            background: rgba(15, 23, 42, 0.2);
        }}

        .mGrid {{ width: 100%; border-collapse: collapse; }}
        .mGrid th {{ background: rgba(255,255,255,0.05); color: var(--text-dim); text-align: left; padding: 1rem; font-size: 0.9rem; }}
        .mGrid td {{ padding: 1rem; border-top: 1px solid var(--glass-border); color: var(--text); }}
        
        input[type=""date""]::-webkit-calendar-picker-indicator {{
            filter: invert(1);
            cursor: pointer;
        }}

        .date-range-container {{
            display: flex;
            gap: 10px;
            width: 100%;
        }}

        .date-range-container .dropdown-control {{
            flex: 1;
        }}

        .filter-span-2 {{
            grid-column: span 2;
        }}
        
            .time-info {{
                margin-top: 2.5rem;
                font-size: 0.8rem;
                color: var(--text-dim);
                border-top: 1px solid var(--glass-border);
                padding-top: 1.25rem;
            }}

            /* Multiselect Styling */
            .btn-group {{
                width: 100% !important;
                position: relative !important;
            }}
            .multiselect.dropdown-toggle {{
                width: 100% !important;
                background: rgba(15, 23, 42, 0.4) !important;
                border: 1px solid var(--glass-border) !important;
                color: var(--text) !important;
                border-radius: 12px !important;
                padding: 0.75rem 1rem !important;
                text-align: left !important;
                display: flex !important;
                align-items: center !important;
                justify-content: space-between !important;
                cursor: pointer;
            }}
            .multiselect-container {{
                position: absolute !important;
                top: 100% !important;
                left: 0 !important;
                z-index: 1000 !important;
                min-width: 250px !important;
                width: max-content !important;
                max-width: 400px !important;
                background: #1e293b !important;
                border: 1px solid var(--glass-border) !important;
                border-radius: 12px !important;
                box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.5) !important;
                padding: 6px !important;
                max-height: 300px !important;
                overflow-y: auto !important;
                list-style: none !important;
                margin: 5px 0 0 0 !important;
                display: none !important;
            }}
            .btn-group.open .multiselect-container {{
                display: block !important;
            }}
            .multiselect-container li {{
                margin: 2px 0;
            }}
            .multiselect-container li a {{
                padding: 0 !important;
                display: block !important;
                color: inherit !important;
                text-decoration: none !important;
            }}
            .multiselect-container label.checkbox {{
                display: flex !important;
                align-items: center !important;
                padding: 8px 12px !important;
                margin: 0 !important;
                color: var(--text) !important;
                font-weight: 400 !important;
                cursor: pointer !important;
                white-space: nowrap !important;
                border-radius: 8px;
                transition: background 0.2s;
            }}
            .multiselect-container li.active label.checkbox,
            .multiselect-container label.checkbox:hover {{
                background: rgba(99, 102, 241, 0.2) !important;
                color: #818cf8 !important;
            }}
            .multiselect-container input[type=""checkbox""] {{
                margin: 0 12px 0 0 !important;
                width: 16px !important;
                height: 16px !important;
                position: static !important;
                cursor: pointer !important;
                flex-shrink: 0;
            }}
            /* Dark Scrollbar */
            .multiselect-container::-webkit-scrollbar {{
                width: 6px;
            }}
            .multiselect-container::-webkit-scrollbar-track {{
                background: transparent;
            }}
            .multiselect-container::-webkit-scrollbar-thumb {{
                background: rgba(255, 255, 255, 0.1);
                border-radius: 10px;
            }}
            .multiselect-container::-webkit-scrollbar-thumb:hover {{
                background: rgba(255, 255, 255, 0.2);
            }}
            .multiselect-selected-text {{
                overflow: hidden !important;
                text-overflow: ellipsis !important;
                white-space: nowrap !important;
                max-width: 180px !important;
            }}

        </style>
        <link href=""Scripts/MultiCheckBox/bootstrap-multiselect.css"" rel=""stylesheet"" />
    </head>
<body>
    <form id=""form1"" runat=""server"">
        <asp:ScriptManager runat=""server"" />
        <asp:UpdatePanel ID=""upMain"" runat=""server"">
            <ContentTemplate>
                <div class=""card"">
                    <h1>{pageTitle}</h1>
                    <p class=""subtitle"">Dynamic Data Explorer</p>
                    
                    <div class=""controls-grid"">
        {filtersHtml.ToString()}
                        <div class=""dropdown-container"">
                            <asp:Button ID=""btnView"" runat=""server"" Text=""View Report"" CssClass=""btn-view"" OnClick=""btnView_Click"" />
                        </div>
                        <div class=""dropdown-container"" style=""flex-direction: row; align-items: flex-end;"">
        {actionButtons}
                        </div>
                    </div>

                    <div class=""grid-container"">
                        <asp:GridView ID=""gvReport"" runat=""server"" CssClass=""mGrid"" AutoGenerateColumns=""false"" 
                            OnRowDeleting=""gvReport_RowDeleting"" />
                    </div>

                    <div class=""time-info"">
                        <span>Last Updated: <asp:Label ID=""lblTime"" runat=""server"" /></span>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                {(actionSettings.EnableExport ? "<asp:PostBackTrigger ControlID=\"btnExport\" />" : "")}
            </Triggers>
        </asp:UpdatePanel>
    </form>

    <script src=""Scripts/Checkbox/jquery-3.3.1.min.js""></script>
    <script src=""Scripts/Checkbox/bootstrap.min.js""></script>
    <script src=""Scripts/MultiCheckBox/bootstrap-multiselect.js""></script>
    <script type=""text/javascript"">
        var lastOpenDropdownId = null;

        function initMultiselect() {{
            $('select[multiple]').each(function() {{
                var $lb = $(this);
                var id = $lb.attr('id');

                $lb.multiselect({{
                    includeSelectAllOption: true,
                    buttonClass: 'multiselect dropdown-toggle',
                    nonSelectedText: '-- Select --',
                    numberDisplayed: 1,
                    onDropdownShown: function() {{
                        lastOpenDropdownId = id;
                    }},
                    onDropdownHidden: function() {{
                        if (lastOpenDropdownId === id) {{
                            lastOpenDropdownId = null;
                        }}
                    }}
                }});

                if (lastOpenDropdownId === id) {{
                    $lb.parent().find('.btn-group').addClass('open');
                }}
            }});
        }}

        $(document).ready(function() {{
            initMultiselect();
        }});

        if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {{
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {{
                initMultiselect();
            }});
        }}
    </script>
</body>
</html>""";
        }

        private string GetDataSourceMethod(string id)
        {
            switch (id)
            {
                case "Ddl1": return "GetPlants";
                case "Ddl2": return "GetMachines";
                case "Ddl3": return "GetComponents";
                case "Ddl4": return "GetOperations";
                case "Ddl5": return "GetShifts";
                default: return "GetDummyData";
            }
        }

        private string GetCsTemplate(string className, List<FilterConfiguration> filters, List<GridColumnDefinition> columns, ActionButtonSettings actionSettings)
        {
            string pageTitle = txtPageTitle.Text.Trim();

            // Build dynamic columns creation code
            StringBuilder colCode = new StringBuilder();
            var keys = columns.Where(c => c.IsKey).Select(c => c.Name).ToList();
            string keyNames = keys.Count > 0 ? string.Join(", ", keys.Select(k => "\"" + k + "\"")) : "";

            colCode.AppendLine($@"                gvReport.DataKeyNames = new string[] {{ {keyNames} }};");

            int colIdx = 1;
            foreach (var col in columns)
            {
                if (col.ControlType == "Label")
                {
                    colCode.AppendLine($@"                var boundCol{colIdx} = new BoundField();
                boundCol{colIdx}.HeaderText = ""{col.Name}"";
                boundCol{colIdx}.DataField = ""{col.Name}"";
                gvReport.Columns.Add(boundCol{colIdx});");
                }
                else
                {
                    colCode.AppendLine($@"                var templateCol{colIdx} = new TemplateField();
                templateCol{colIdx}.HeaderText = ""{col.Name}"";
                templateCol{colIdx}.ItemTemplate = new DynamicTemplate_{className}(""{col.Name}"", ""{col.ControlType}"");
                gvReport.Columns.Add(templateCol{colIdx});");
                }
                colIdx++;
            }
            colCode.AppendLine($@"                var actionCol = new TemplateField();
                actionCol.HeaderText = ""Action"";
                actionCol.ItemTemplate = new DynamicTemplate_{className}(""Action"", ""DeleteButton"");
                actionCol.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                gvReport.Columns.Add(actionCol);");

            // Build dynamic Bind methods
            StringBuilder bindMethods = new StringBuilder();
            foreach (var f in filters.Where(f => f.Enabled && f.ID != "Date"))
            {
                string modePrefix = f.Mode == "Single" ? "ddl" : "lb";
                bindMethods.AppendLine($@"        private void Bind{f.ID}(string parent1 = null, string parent2 = null)
        {{
            DataTable dt;
            switch (""{f.ID}"")
            {{
                case ""Ddl1"": dt = TPMStudioDBAccess.GetPlants(); break;
                case ""Ddl2"": dt = TPMStudioDBAccess.GetMachines(parent1); break;
                case ""Ddl3"": dt = TPMStudioDBAccess.GetComponents(parent1); break;
                case ""Ddl4"": dt = TPMStudioDBAccess.GetOperations(parent1, parent2); break;
                case ""Ddl5"": dt = TPMStudioDBAccess.GetShifts(); break;
                default: dt = TPMStudioDBAccess.GetDummyData(); break;
            }}

            var ctrl = FindControlRecursive(Page, ""{modePrefix}{f.ID}"");
            if (ctrl is ListControl lc)
            {{
                lc.DataSource = dt;
                if (dt != null && dt.Columns.Count > 0)
                {{
                    lc.DataValueField = dt.Columns[0].ColumnName;
                    lc.DataTextField = dt.Columns.Count > 1 ? dt.Columns[1].ColumnName : dt.Columns[0].ColumnName;
                }}
                lc.DataBind();
                if (lc is DropDownList) lc.Items.Insert(0, new System.Web.UI.WebControls.ListItem(""-- Select {f.Name} --"", """"));
            }}
        }}");
            }

            // Build Filter_Changed body
            string plantValExpr = filters.Any(f => f.ID == "Ddl1" && f.Enabled) ? "GetSelectedValues(\"Ddl1\")" : "\"\"";
            string machineValExpr = filters.Any(f => f.ID == "Ddl2" && f.Enabled) ? "GetSelectedValues(\"Ddl2\")" : "\"\"";
            string componentValExpr = filters.Any(f => f.ID == "Ddl3" && f.Enabled) ? "GetSelectedValues(\"Ddl3\")" : "\"\"";

            StringBuilder filterChangedBody = new StringBuilder();
            filterChangedBody.AppendLine($@"            var lbSender = sender as ListControl;");
            filterChangedBody.AppendLine($@"            if (lbSender == null) return;");
            filterChangedBody.AppendLine($@"            string senderID = lbSender.ID;");
            filterChangedBody.AppendLine();
            filterChangedBody.AppendLine($@"            string plantVal = GetSelectedValues(""Ddl1"");");
            filterChangedBody.AppendLine($@"            string machineVal = GetSelectedValues(""Ddl2"");");
            filterChangedBody.AppendLine($@"            string componentVal = GetSelectedValues(""Ddl3"");");
            filterChangedBody.AppendLine();

            bool hasD2 = filters.Any(f => f.ID == "Ddl2" && f.Enabled);
            bool hasD3 = filters.Any(f => f.ID == "Ddl3" && f.Enabled);
            bool hasD4 = filters.Any(f => f.ID == "Ddl4" && f.Enabled);

            filterChangedBody.AppendLine($@"            if (senderID.EndsWith(""Ddl1"")) {{");
            if (hasD2) filterChangedBody.AppendLine($@"                BindDdl2(plantVal); RestoreSelection(""Ddl2"");");
            if (hasD3) filterChangedBody.AppendLine($@"                BindDdl3(GetSelectedValues(""Ddl2"")); RestoreSelection(""Ddl3"");");
            if (hasD4) filterChangedBody.AppendLine($@"                BindDdl4(GetSelectedValues(""Ddl2""), GetSelectedValues(""Ddl3"")); RestoreSelection(""Ddl4"");");
            filterChangedBody.AppendLine($@"            }}");

            if (hasD2)
            {
                filterChangedBody.AppendLine($@"            else if (senderID.EndsWith(""Ddl2"")) {{");
                if (hasD3) filterChangedBody.AppendLine($@"                BindDdl3(machineVal); RestoreSelection(""Ddl3"");");
                if (hasD4) filterChangedBody.AppendLine($@"                BindDdl4(machineVal, GetSelectedValues(""Ddl3"")); RestoreSelection(""Ddl4"");");
                filterChangedBody.AppendLine($@"            }}");
            }

            if (hasD3)
            {
                filterChangedBody.AppendLine($@"            else if (senderID.EndsWith(""Ddl3"")) {{");
                if (hasD4) filterChangedBody.AppendLine($@"                BindDdl4(machineVal, componentVal); RestoreSelection(""Ddl4"");");
                filterChangedBody.AppendLine($@"            }}");
            }
            filterChangedBody.AppendLine();
            filterChangedBody.AppendLine($@"            BindGrid();");

            // Build the final template string
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Web.UI;");
            sb.AppendLine("using System.Web.UI.WebControls;");
            sb.AppendLine("using Web_TPMTrakDashboard.Models;");
            sb.AppendLine("using System.IO;");
            sb.AppendLine("using iTextSharp.text;");
            sb.AppendLine("using iTextSharp.text.pdf;");
            sb.AppendLine("using System.Web;");
            sb.AppendLine("using OfficeOpenXml;");
            sb.AppendLine("using OfficeOpenXml.Style;");
            sb.AppendLine("using System.Drawing;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine();
            sb.AppendLine("namespace Web_TPMTrakDashboard");
            sb.AppendLine("{");
            sb.AppendLine($@"    public partial class {className} : System.Web.UI.Page");
            sb.AppendLine("    {");
            sb.AppendLine("        protected void Page_Load(object sender, EventArgs e)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (!IsPostBack)");
            sb.AppendLine("            {");
            sb.AppendLine($@"                lblTime.Text = DateTime.Now.ToString(""MMM dd, yyyy HH:mm:ss"");");
            sb.AppendLine("                BindGrid();");
            sb.AppendLine("            }");
            sb.AppendLine("            CreateDynamicColumns();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        protected override void OnInit(EventArgs e)");
            sb.AppendLine("        {");
            sb.AppendLine("            base.OnInit(e);");
            sb.AppendLine("            if (!IsPostBack) {");
            sb.AppendLine("                foreach (string id in new[] { \"Ddl1\", \"Ddl2\", \"Ddl3\", \"Ddl4\", \"Ddl5\" }) {");
            sb.AppendLine("                    Control ctrl = FindControlRecursive(Page, \"ddl\" + id) ?? FindControlRecursive(Page, \"lb\" + id);");
            sb.AppendLine("                    if (ctrl is ListControl lc) {");
            sb.AppendLine("                        if (id == \"Ddl1\" || id == \"Ddl5\") BindFilterControl(lc, id, \"Filter\", true);");
            sb.AppendLine("                        else BindFilterControl(lc, id, \"Filter\", false, \"\");");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            } else {");
            sb.AppendLine("                foreach (string id in new[] { \"Ddl1\", \"Ddl2\", \"Ddl3\", \"Ddl4\", \"Ddl5\" }) {");
            sb.AppendLine("                    Control ctrl = FindControlRecursive(Page, \"ddl\" + id) ?? FindControlRecursive(Page, \"lb\" + id);");
            sb.AppendLine("                    if (ctrl is ListControl lc) {");
            sb.AppendLine("                        DataTable dt = Session[\"Items_\" + id] as DataTable;");
            sb.AppendLine("                        if (dt != null) {");
            sb.AppendLine("                            lc.DataSource = dt; lc.DataBind();");
            sb.AppendLine("                            if (lc is DropDownList) lc.Items.Insert(0, new System.Web.UI.WebControls.ListItem(\"-- Select --\", \"\"));");
            sb.AppendLine("                        }");
            sb.AppendLine("                        RestoreSelection(lc, id);");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        private Control FindControlRecursive(Control root, string id)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (root != null && root.ID == id) return root;");
            sb.AppendLine("            foreach (Control child in root.Controls)");
            sb.AppendLine("            {");
            sb.AppendLine("                Control res = FindControlRecursive(child, id);");
            sb.AppendLine("                if (res != null) return res;");
            sb.AppendLine("            }");
            sb.AppendLine("            return null;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        protected void Filter_Changed(object sender, EventArgs e)");
            sb.AppendLine("        {");
            sb.AppendLine("            var lbSender = sender as ListControl;");
            sb.AppendLine("            if (lbSender == null) return;");
            sb.AppendLine("            string id = lbSender.ID;");
            sb.AppendLine("            string p = GetSelectedValues(\"Ddl1\");");
            sb.AppendLine("            string m = GetSelectedValues(\"Ddl2\");");
            sb.AppendLine("            string c = GetSelectedValues(\"Ddl3\");");
            sb.AppendLine();
            sb.AppendLine("            if (id.EndsWith(\"Ddl1\")) { ");
            sb.AppendLine("                UpdateChild(\"Ddl2\", p); ");
            sb.AppendLine("                UpdateChild(\"Ddl3\", GetSelectedValues(\"Ddl2\"));");
            sb.AppendLine("                UpdateChild(\"Ddl4\", GetSelectedValues(\"Ddl2\"), GetSelectedValues(\"Ddl3\"));");
            sb.AppendLine("            }");
            sb.AppendLine("            else if (id.EndsWith(\"Ddl2\")) { ");
            sb.AppendLine("                UpdateChild(\"Ddl3\", m);");
            sb.AppendLine("                UpdateChild(\"Ddl4\", m, GetSelectedValues(\"Ddl3\"));");
            sb.AppendLine("            }");
            sb.AppendLine("            else if (id.EndsWith(\"Ddl3\")) { ");
            sb.AppendLine("                UpdateChild(\"Ddl4\", m, c);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        private void UpdateChild(string id, string p1, string p2 = null)");
            sb.AppendLine("        {");
            sb.AppendLine("            Control ctrl = FindControlRecursive(Page, \"ddl\" + id) ?? FindControlRecursive(Page, \"lb\" + id);");
            sb.AppendLine("            if (ctrl is ListControl lc) BindFilterControl(lc, id, \"Filter\", false, p1, p2);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        private void BindFilterControl(ListControl lc, string id, string name, bool d, string p1 = null, string p2 = null)");
            sb.AppendLine("        {");
            sb.AppendLine("            DataTable dt;");
            sb.AppendLine("            try {");
            sb.AppendLine("                switch (id) {");
            sb.AppendLine("                    case \"Ddl1\": dt = TPMStudioDBAccess.GetPlants(); break;");
            sb.AppendLine("                    case \"Ddl2\": dt = TPMStudioDBAccess.GetMachines(p1); break;");
            sb.AppendLine("                    case \"Ddl3\": dt = TPMStudioDBAccess.GetComponents(p1); break;");
            sb.AppendLine("                    case \"Ddl4\": dt = TPMStudioDBAccess.GetOperations(p1, p2); break;");
            sb.AppendLine("                    case \"Ddl5\": dt = TPMStudioDBAccess.GetShifts(); break;");
            sb.AppendLine("                    default: dt = TPMStudioDBAccess.GetDummyData(); break;");
            sb.AppendLine("                }");
            sb.AppendLine("            } catch { dt = null; }");
            sb.AppendLine("            if (dt == null || dt.Rows.Count == 0) {");
            sb.AppendLine("                dt = new DataTable(); dt.Columns.Add(\"ID\"); dt.Columns.Add(\"Name\");");
            sb.AppendLine("                for (int i = 1; i <= 3; i++) dt.Rows.Add(id + \"_\" + i, id + \" Item \" + i);");
            sb.AppendLine("            }");
            sb.AppendLine("            lc.DataSource = dt;");
            sb.AppendLine("            if (dt.Columns.Count > 0) {");
            sb.AppendLine("                lc.DataValueField = dt.Columns[0].ColumnName;");
            sb.AppendLine("                lc.DataTextField = dt.Columns.Count > 1 ? dt.Columns[1].ColumnName : dt.Columns[0].ColumnName;");
            sb.AppendLine("            }");
            sb.AppendLine("            lc.DataBind();");
            sb.AppendLine("            if (d && lc is DropDownList) lc.Items.Insert(0, new System.Web.UI.WebControls.ListItem(\"-- Select --\", \"\"));");
            sb.AppendLine("            Session[\"Items_\" + id] = dt;");
            sb.AppendLine("            RestoreSelection(lc, id);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        private void RestoreSelection(ListControl lc, string id)");
            sb.AppendLine("        {");
            sb.AppendLine("            string val = GetPostBackValue(id);");
            sb.AppendLine("            if (string.IsNullOrEmpty(val)) return;");
            sb.AppendLine("            foreach (string s in val.Split(',')) {");
            sb.AppendLine("                var itm = lc.Items.FindByValue(s);");
            sb.AppendLine("                if (itm != null) itm.Selected = true;");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        private string GetSelectedValues(string filterID)");
            sb.AppendLine("        {");
            sb.AppendLine(@"            Control ctrl = FindControlRecursive(Page, ""ddl"" + filterID) ?? FindControlRecursive(Page, ""lb"" + filterID);");
            sb.AppendLine("            if (ctrl is ListControl lc)");
            sb.AppendLine("            {");
            sb.AppendLine(@"                var selected = lc.Items.Cast<System.Web.UI.WebControls.ListItem>().Where(i => i.Selected && !string.IsNullOrEmpty(i.Value)).Select(i => i.Value).ToList();");
            sb.AppendLine("                if (selected.Count > 0) return string.Join(\",\", selected);");
            sb.AppendLine("            }");
            sb.AppendLine("            return GetPostBackValue(filterID);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        private string GetPostBackValue(string id)");
            sb.AppendLine("        {");
            sb.AppendLine(@"            if (Request.Form == null) return """";");
            sb.AppendLine(@"            string key = Request.Form.AllKeys.FirstOrDefault(k => k != null && (k.EndsWith(""$lb"" + id) || k.EndsWith(""$ddl"" + id) || k.EndsWith(""$"" + id) || k == id));");
            sb.AppendLine(@"            if (string.IsNullOrEmpty(key)) return """";");
            sb.AppendLine("            var values = Request.Form.GetValues(key);");
            sb.AppendLine(@"            if (values == null) return """";");
            sb.AppendLine(@"            return string.Join("","", values.Where(v => !string.IsNullOrEmpty(v)));");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        private void CreateDynamicColumns()");
            sb.AppendLine("        {");
            sb.AppendLine("            if (gvReport.Columns.Count == 0)");
            sb.AppendLine("            {");
            sb.Append(colCode.ToString());
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        protected void btnView_Click(object sender, EventArgs e)");
            sb.AppendLine("        {");
            sb.AppendLine("            BindGrid();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        protected void btnExport_Click(object sender, EventArgs e)");
            sb.AppendLine("        {");
            sb.AppendLine($@"            string title = ""{pageTitle}"";");
            sb.AppendLine($@"            string format = ""{actionSettings.ExportFormat}"";");
            sb.AppendLine("            BindGrid();");
            sb.AppendLine($@"            if (format == ""Excel"") {{ ExportToExcel(title); }} else {{ ExportToPDF(title); }}");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        private void ExportToExcel(string title)");
            sb.AppendLine("        {");
            sb.AppendLine("            using (OfficeOpenXml.ExcelPackage pck = new OfficeOpenXml.ExcelPackage())");
            sb.AppendLine("            {");
            sb.AppendLine($@"                OfficeOpenXml.ExcelWorksheet ws = pck.Workbook.Worksheets.Add(""Report"");");
            sb.AppendLine("                int colCountVal = 0; List<int> visibleColIndices = new List<int>();");
            sb.AppendLine("                for (int i = 0; i < gvReport.Columns.Count; i++)");
            sb.AppendLine("                {");
            sb.AppendLine($@"                    if (gvReport.Columns[i].Visible && gvReport.Columns[i].HeaderText != ""Action"")");
            sb.AppendLine("                    { colCountVal++; visibleColIndices.Add(i); }");
            sb.AppendLine("                }");
            sb.AppendLine("                if (colCountVal > 0)");
            sb.AppendLine("                {");
            sb.AppendLine("                    ws.Cells[1, 1, 2, colCountVal].Merge = true;");
            sb.AppendLine($@"                    ws.Cells[1, 1].Value = title + "" REPORT"";");
            sb.AppendLine("                    ws.Cells[1, 1].Style.Font.Size = 16; ws.Cells[1, 1].Style.Font.Bold = true;");
            sb.AppendLine($@"                    ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;");
            sb.AppendLine("                }");
            sb.AppendLine("                int excelCol = 1;");
            sb.AppendLine("                foreach (int idx in visibleColIndices)");
            sb.AppendLine("                {");
            sb.AppendLine("                    ws.Cells[4, excelCol].Value = gvReport.Columns[idx].HeaderText;");
            sb.AppendLine("                    ws.Cells[4, excelCol].Style.Font.Bold = true;");
            sb.AppendLine("                    excelCol++;");
            sb.AppendLine("                }");
            sb.AppendLine("                int excelRow = 5;");
            sb.AppendLine("                foreach (GridViewRow row in gvReport.Rows)");
            sb.AppendLine("                {");
            sb.AppendLine("                    excelCol = 1;");
            sb.AppendLine("                    foreach (int colIdx in visibleColIndices)");
            sb.AppendLine("                    {");
            sb.AppendLine($@"                        string cellText = """"; Control ctrl = row.Cells[colIdx].Controls.Count > 0 ? row.Cells[colIdx].Controls[0] : null;");
            sb.AppendLine($@"                        if (ctrl is Label) cellText = ((Label)ctrl).Text;");
            sb.AppendLine($@"                        else if (ctrl is TextBox) cellText = ((TextBox)ctrl).Text;");
            sb.AppendLine($@"                        else if (ctrl is CheckBox) cellText = ((CheckBox)ctrl).Checked ? ""Yes"" : ""No"";");
            sb.AppendLine($@"                        else cellText = row.Cells[colIdx].Text.Replace(""&nbsp;"", """");");
            sb.AppendLine("                        ws.Cells[excelRow, excelCol].Value = cellText; excelCol++;");
            sb.AppendLine("                    }");
            sb.AppendLine("                    excelRow++;");
            sb.AppendLine("                }");
            sb.AppendLine("                ws.Cells.AutoFitColumns();");
            sb.AppendLine("                Response.Clear();");
            sb.AppendLine($@"                Response.ContentType = ""application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"";");
            sb.AppendLine($@"                Response.AddHeader(""content-disposition"", ""attachment;filename="" + title.Replace("" "", ""_"") + ""_Report.xlsx"");");
            sb.AppendLine("                Response.BinaryWrite(pck.GetAsByteArray()); Response.End();");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        private void ExportToPDF(string title)");
            sb.AppendLine("        {");
            sb.AppendLine("            int colCountVal = 0; foreach(DataControlField col in gvReport.Columns) { if(col.Visible && col.HeaderText != \"Action\") colCountVal++; }");
            sb.AppendLine($@"            iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 10f, 10f, 10f, 0f);");
            sb.AppendLine("            using (MemoryStream ms = new MemoryStream())");
            sb.AppendLine("            {");
            sb.AppendLine("                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, ms); pdfDoc.Open();");
            sb.AppendLine("                iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(colCountVal); table.WidthPercentage = 100;");
            sb.AppendLine($@"                iTextSharp.text.pdf.PdfPCell headerCell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(title + "" REPORT"", iTextSharp.text.FontFactory.GetFont(""Arial"", 16, iTextSharp.text.Font.BOLD)));");
            sb.AppendLine("                headerCell.Colspan = colCountVal; headerCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER; headerCell.Border = iTextSharp.text.Rectangle.NO_BORDER; table.AddCell(headerCell);");
            sb.AppendLine("                foreach (DataControlField col in gvReport.Columns) { if (col.Visible && col.HeaderText != \"Action\") { table.AddCell(new iTextSharp.text.Phrase(col.HeaderText, iTextSharp.text.FontFactory.GetFont(\"Arial\", 10, iTextSharp.text.Font.BOLD))); } }");
            sb.AppendLine("                foreach (GridViewRow row in gvReport.Rows) { for (int i = 0; i < gvReport.Columns.Count; i++) { if (gvReport.Columns[i].Visible && gvReport.Columns[i].HeaderText != \"Action\") { ");
            sb.AppendLine($@"                    string cellText = """"; Control ctrl = row.Cells[i].Controls.Count > 0 ? row.Cells[i].Controls[0] : null;");
            sb.AppendLine($@"                    if (ctrl is Label) cellText = ((Label)ctrl).Text; else if (ctrl is TextBox) cellText = ((TextBox)ctrl).Text; else if (ctrl is CheckBox) cellText = ((CheckBox)ctrl).Checked ? ""Yes"" : ""No""; else cellText = row.Cells[i].Text.Replace(""&nbsp;"", """");");
            sb.AppendLine($@"                    table.AddCell(new iTextSharp.text.Phrase(cellText, iTextSharp.text.FontFactory.GetFont(""Arial"", 9)));");
            sb.AppendLine("                } } }");
            sb.AppendLine("                pdfDoc.Add(table); pdfDoc.Close();");
            sb.AppendLine("                Response.Clear(); Response.ContentType = \"application/pdf\";");
            sb.AppendLine($@"                Response.AddHeader(""Content-Disposition"", ""attachment;filename="" + title.Replace("" "", ""_"") + ""_Report.pdf"");");
            sb.AppendLine($@"                Response.BinaryWrite(ms.ToArray()); Response.End();");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public override void VerifyRenderingInServerForm(Control control) { }");
            sb.AppendLine();
            sb.AppendLine("        protected void btnImport_Click(object sender, EventArgs e)");
            sb.AppendLine("        {");
            sb.AppendLine($@"            ScriptManager.RegisterStartupScript(this, GetType(), ""imp"", ""alert('Importing data...');"", true);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        protected void gvReport_RowDeleting(object sender, GridViewDeleteEventArgs e)");
            sb.AppendLine("        {");
            sb.AppendLine("            // Bind context again to handle deletion logic or refresh");
            sb.AppendLine("            BindGrid();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        private void BindGrid()");
            sb.AppendLine("        {");
            sb.AppendLine(@"            DataTable dt = TPMStudioDBAccess.GetDummyData(); gvReport.DataSource = dt; gvReport.DataBind();");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine($@"    public class DynamicTemplate_{className} : ITemplate");
            sb.AppendLine("    {");
            sb.AppendLine("        private string _colName; private string _type;");
            sb.AppendLine($@"        public DynamicTemplate_{className}(string colName, string type) {{ _colName = colName; _type = type; }}");
            sb.AppendLine("        public void InstantiateIn(Control container)");
            sb.AppendLine("        {");
            sb.AppendLine($@"            if (_type == ""TextField"") {{");
            sb.AppendLine($@"                var txt = new TextBox(); txt.ID = ""txt_"" + _colName; txt.CssClass = ""input-control"";");
            sb.AppendLine($@"                txt.DataBinding += (s, e) => {{ var t = (TextBox)s; var row = (GridViewRow)t.NamingContainer; if (row != null && row.DataItem != null) t.Text = DataBinder.Eval(row.DataItem, _colName)?.ToString() ?? """"; }};");
            sb.AppendLine("                container.Controls.Add(txt);");
            sb.AppendLine($@"            }} else if (_type == ""Checkbox"") {{");
            sb.AppendLine($@"                var chk = new CheckBox(); chk.ID = ""chk_"" + _colName;");
            sb.AppendLine($@"                chk.DataBinding += (s, e) => {{ var c = (CheckBox)s; var row = (GridViewRow)c.NamingContainer; if (row != null && row.DataItem != null) c.Checked = Convert.ToBoolean(DataBinder.Eval(row.DataItem, _colName) ?? false); }};");
            sb.AppendLine("                container.Controls.Add(chk);");
            sb.AppendLine($@"            }} else if (_type == ""DeleteButton"") {{");
            sb.AppendLine($@"                var btn = new LinkButton(); btn.ID = ""btnDelete""; btn.Text = ""&#128465;""; btn.CommandName = ""Delete"";");
            sb.AppendLine($@"                btn.OnClientClick = ""return confirm('Are you sure?');"";");
            sb.AppendLine("                container.Controls.Add(btn);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private string GetDesignerTemplate(string className)
        {
            return $@"namespace Web_TPMTrakDashboard {{
    public partial class {className} {{
        protected global::System.Web.UI.HtmlControls.HtmlForm form1;
        protected global::System.Web.UI.WebControls.Label lblTime;
        protected global::System.Web.UI.WebControls.GridView gvReport;
        protected global::System.Web.UI.WebControls.Button btnView;
        protected global::System.Web.UI.WebControls.Button btnExport;
        protected global::System.Web.UI.WebControls.TextBox txtDate;
        protected global::System.Web.UI.UpdatePanel upMain;
        protected global::System.Web.UI.ScriptManager ScriptManager1;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl pnlDdl1;
        protected global::System.Web.UI.WebControls.ListControl ddlSelection1;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl pnlDdl2;
        protected global::System.Web.UI.WebControls.ListControl ddlSelection2;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl pnlDdl3;
        protected global::System.Web.UI.WebControls.ListControl ddlSelection3;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl pnlDdl4;
        protected global::System.Web.UI.WebControls.ListControl ddlSelection4;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl pnlDdl5;
        protected global::System.Web.UI.WebControls.ListControl ddlSelection5;
    }}
}}";
        }
    }
}
