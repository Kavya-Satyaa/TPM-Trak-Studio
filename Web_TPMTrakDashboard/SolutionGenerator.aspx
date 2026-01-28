<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolutionGenerator.aspx.cs"
    Inherits="Web_TPMTrakDashboard.SolutionGenerator" %>

    <!DOCTYPE html>
    <html xmlns="http://www.w3.org/1999/xhtml">

    <head runat="server">
        <title>TPM-Trak | Solution Generator</title>
        <link href="https://fonts.googleapis.com/css2?family=Outfit:wght@300;400;600&display=swap" rel="stylesheet" />
        <style>
            :root {
                --primary: #6366f1;
                --primary-hover: #4f46e5;
                --bg: #0f172a;
                --glass-bg: rgba(30, 41, 59, 0.7);
                --glass-border: rgba(255, 255, 255, 0.1);
                --text: #f8fafc;
                --text-dim: #94a3b8;
            }

            body {
                margin: 0;
                padding: 0;
                font-family: 'Outfit', sans-serif;
                background: radial-gradient(circle at top right, #1e1b4b, #0f172a);
                color: var(--text);
                min-height: 100vh;
                display: flex;
                align-items: center;
                justify-content: center;
            }

            .container {
                width: 100%;
                max-width: 800px;
                padding: 2rem;
                background: var(--glass-bg);
                backdrop-filter: blur(12px);
                border-radius: 24px;
                border: 1px solid var(--glass-border);
                box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.5);
                animation: fadeIn 0.6s ease-out;
            }

            @keyframes fadeIn {
                from {
                    opacity: 0;
                    transform: translateY(20px);
                }

                to {
                    opacity: 1;
                    transform: translateY(0);
                }
            }

            h1 {
                font-size: 2rem;
                font-weight: 600;
                margin-bottom: 0.5rem;
                background: linear-gradient(to right, #818cf8, #c084fc);
                -webkit-background-clip: text;
                background-clip: text;
                -webkit-text-fill-color: transparent;
            }

            p.subtitle {
                color: var(--text-dim);
                margin-bottom: 2rem;
                font-size: 0.95rem;
            }

            .form-group {
                margin-bottom: 1.5rem;
            }

            label {
                display: block;
                margin-bottom: 0.5rem;
                font-weight: 500;
                font-size: 0.9rem;
                color: var(--text-dim);
            }

            .input-control {
                width: 100%;
                padding: 0.8rem 1rem;
                background: rgba(15, 23, 42, 0.5);
                border: 1px solid var(--glass-border);
                border-radius: 12px;
                color: var(--text);
                font-family: inherit;
                font-size: 1rem;
                transition: all 0.2s;
                box-sizing: border-box;
            }

            .input-control:focus {
                outline: none;
                border-color: var(--primary);
                box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.2);
            }

            .btn-container {
                display: grid;
                grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
                gap: 1rem;
                margin-top: 1.5rem;
            }

            .btn-action {
                padding: 1rem;
                border: none;
                border-radius: 12px;
                color: white;
                font-weight: 600;
                font-size: 0.95rem;
                cursor: pointer;
                transition: all 0.2s;
                text-align: center;
                text-decoration: none;
                display: flex;
                align-items: center;
                justify-content: center;
                gap: 8px;
            }

            .btn-preview {
                background: rgba(255, 255, 255, 0.1);
                border: 1px solid var(--glass-border);
            }

            .btn-preview:hover {
                background: rgba(255, 255, 255, 0.2);
                border-color: var(--primary);
            }

            .btn-generate {
                background: var(--primary);
                box-shadow: 0 10px 15px -3px rgba(99, 102, 241, 0.3);
            }

            .btn-generate:hover {
                background: var(--primary-hover);
                transform: translateY(-2px);
                box-shadow: 0 20px 25px -5px rgba(99, 102, 241, 0.4);
            }

            .btn-generate:active {
                transform: translateY(0);
            }

            .status-msg {
                margin-top: 1.5rem;
                padding: 1rem;
                border-radius: 12px;
                font-size: 0.9rem;
                display: none;
            }

            .status-success {
                background: rgba(34, 197, 94, 0.1);
                border: 1px solid rgba(34, 197, 94, 0.2);
                color: #4ade80;
            }

            .status-error {
                background: rgba(239, 68, 68, 0.1);
                border: 1px solid rgba(239, 68, 68, 0.2);
                color: #f87171;
            }

            .loader {
                display: inline-block;
                width: 20px;
                height: 20px;
                border: 3px solid rgba(255, 255, 255, .3);
                border-radius: 50%;
                border-top-color: #fff;
                animation: spin 1s ease-in-out infinite;
                margin-right: 10px;
                vertical-align: middle;
                display: none;
            }

            @keyframes spin {
                to {
                    transform: rotate(360deg);
                }
            }

            .radio-group label {
                display: inline-block;
                margin-right: 15px;
                color: var(--text);
                font-weight: 400;
                cursor: pointer;
            }

            .radio-group input {
                margin-right: 5px;
            }
        </style>
    </head>

    <body>
        <form id="form1" runat="server">
            <div class="container">
                <h1>Solution Generator</h1>
                <p class="subtitle">Generate a published artifact with a custom page.</p>

                <div class="form-group">
                    <label>Page Name</label>
                    <asp:TextBox ID="txtPageName" runat="server" CssClass="input-control"
                        placeholder="e.g. MyNewDashboard" />
                </div>

                <div class="form-group">
                    <label>Page Title</label>
                    <asp:TextBox ID="txtPageTitle" runat="server" CssClass="input-control"
                        placeholder="e.g. Analytics Overview" />
                </div>

                <div class="form-group">
                    <label>External Publish Output Path (Optional)</label>
                    <asp:TextBox ID="txtPublishPath" runat="server" CssClass="input-control"
                        placeholder="C:\Temp\PublishOutput" />
                </div>

                <div class="form-group"
                    style="margin-top: 2rem; border-top: 1px solid var(--glass-border); padding-top: 2rem;">
                    <label>Filters Selection (Configure Generated Screen Filters)</label>
                    <p style="font-size: 0.8rem; color: var(--text-dim); margin-bottom: 1rem;">
                        Enable filters and choose selection modes (Single/Multi-select or Date Range).
                    </p>
                    <div style="display: grid; grid-template-columns: 1fr; gap: 1rem; margin-top: 0.5rem;"
                        class="filters-config-grid">

                        <!-- Plant Filter -->
                        <div
                            style="display: flex; align-items: flex-start; gap: 1rem; background: rgba(255,255,255,0.02); padding: 12px; border-radius: 12px; border: 1px solid var(--glass-border);">
                            <div style="width: 200px;">
                                <asp:CheckBox ID="chkDdl1" runat="server" Text="Plant Filter" Checked="true"
                                    CssClass="chk-toggle" data-target="optsDdl1" />
                            </div>
                            <div id="optsDdl1" style="display: block;">
                                <asp:RadioButtonList ID="rblMode1" runat="server" RepeatDirection="Horizontal"
                                    CssClass="radio-group-sm">
                                    <asp:ListItem Text="Single Select" Value="Single" Selected="True" />
                                    <asp:ListItem Text="Multi Select" Value="Multi" />
                                </asp:RadioButtonList>
                            </div>
                        </div>

                        <!-- Machine Filter -->
                        <div
                            style="display: flex; align-items: flex-start; gap: 1rem; background: rgba(255,255,255,0.02); padding: 12px; border-radius: 12px; border: 1px solid var(--glass-border);">
                            <div style="width: 200px;">
                                <asp:CheckBox ID="chkDdl2" runat="server" Text="Machine Filter" Checked="true"
                                    CssClass="chk-toggle" data-target="optsDdl2" />
                            </div>
                            <div id="optsDdl2" style="display: block;">
                                <asp:RadioButtonList ID="rblMode2" runat="server" RepeatDirection="Horizontal"
                                    CssClass="radio-group-sm">
                                    <asp:ListItem Text="Single Select" Value="Single" Selected="True" />
                                    <asp:ListItem Text="Multi Select" Value="Multi" />
                                </asp:RadioButtonList>
                            </div>
                        </div>

                        <!-- Component Filter -->
                        <div
                            style="display: flex; align-items: flex-start; gap: 1rem; background: rgba(255,255,255,0.02); padding: 12px; border-radius: 12px; border: 1px solid var(--glass-border);">
                            <div style="width: 200px;">
                                <asp:CheckBox ID="chkDdl3" runat="server" Text="Component Filter" Checked="true"
                                    CssClass="chk-toggle" data-target="optsDdl3" />
                            </div>
                            <div id="optsDdl3" style="display: block;">
                                <asp:RadioButtonList ID="rblMode3" runat="server" RepeatDirection="Horizontal"
                                    CssClass="radio-group-sm">
                                    <asp:ListItem Text="Single Select" Value="Single" Selected="True" />
                                    <asp:ListItem Text="Multi Select" Value="Multi" />
                                </asp:RadioButtonList>
                            </div>
                        </div>

                        <!-- Operation Filter -->
                        <div
                            style="display: flex; align-items: flex-start; gap: 1rem; background: rgba(255,255,255,0.02); padding: 12px; border-radius: 12px; border: 1px solid var(--glass-border);">
                            <div style="width: 200px;">
                                <asp:CheckBox ID="chkDdl4" runat="server" Text="Operation Filter" Checked="true"
                                    CssClass="chk-toggle" data-target="optsDdl4" />
                            </div>
                            <div id="optsDdl4" style="display: block;">
                                <asp:RadioButtonList ID="rblMode4" runat="server" RepeatDirection="Horizontal"
                                    CssClass="radio-group-sm">
                                    <asp:ListItem Text="Single Select" Value="Single" Selected="True" />
                                    <asp:ListItem Text="Multi Select" Value="Multi" />
                                </asp:RadioButtonList>
                            </div>
                        </div>

                        <!-- Shift Filter -->
                        <div
                            style="display: flex; align-items: flex-start; gap: 1rem; background: rgba(255,255,255,0.02); padding: 12px; border-radius: 12px; border: 1px solid var(--glass-border);">
                            <div style="width: 200px;">
                                <asp:CheckBox ID="chkDdl5" runat="server" Text="Shift Filter" Checked="true"
                                    CssClass="chk-toggle" data-target="optsDdl5" />
                            </div>
                            <div id="optsDdl5" style="display: block;">
                                <asp:RadioButtonList ID="rblMode5" runat="server" RepeatDirection="Horizontal"
                                    CssClass="radio-group-sm">
                                    <asp:ListItem Text="Single Select" Value="Single" Selected="True" />
                                    <asp:ListItem Text="Multi Select" Value="Multi" />
                                </asp:RadioButtonList>
                            </div>
                        </div>

                        <!-- Date Filter -->
                        <div
                            style="display: flex; align-items: flex-start; gap: 1rem; background: rgba(255,255,255,0.02); padding: 12px; border-radius: 12px; border: 1px solid var(--glass-border);">
                            <div style="width: 200px;">
                                <asp:CheckBox ID="chkDate" runat="server" Text="Date Filter" Checked="true"
                                    CssClass="chk-toggle" data-target="optsDate" />
                            </div>
                            <div id="optsDate" style="display: block;">
                                <asp:RadioButtonList ID="rblModeDate" runat="server" RepeatDirection="Horizontal"
                                    CssClass="radio-group-sm">
                                    <asp:ListItem Text="Single Date" Value="SingleDate" Selected="True" />
                                    <asp:ListItem Text="From & To Date" Value="DateRange" />
                                </asp:RadioButtonList>
                            </div>
                        </div>

                    </div>
                </div>

                <style>
                    .radio-group-sm label {
                        margin-right: 20px;
                        font-size: 0.85rem;
                        color: var(--text-dim);
                        cursor: pointer;
                        display: flex;
                        align-items: center;
                    }

                    .filters-config-grid>div:hover {
                        background: rgba(255, 255, 255, 0.04) !important;
                        border-color: var(--primary) !important;
                    }
                </style>

                <div class="form-group"
                    style="margin-top: 2rem; border-top: 1px solid var(--glass-border); padding-top: 2rem;">
                    <label>Grid Column Configuration (Model Definition)</label>
                    <p style="font-size: 0.8rem; color: var(--text-dim); margin-bottom: 1rem;">
                        Define the columns for the dynamic GridView. Ensure Column Names match your Data Source.
                    </p>

                    <div id="gridConfigContainer">
                        <table id="tblGridConfig" style="width: 100%; border-collapse: collapse;">
                            <thead>
                                <tr style="text-align: left; border-bottom: 1px solid var(--glass-border);">
                                    <th style="padding: 0.5rem; font-size: 0.85rem; color: var(--text-dim);">Column Name
                                    </th>
                                    <th style="padding: 0.5rem; font-size: 0.85rem; color: var(--text-dim);">Data Type
                                    </th>
                                    <th style="padding: 0.5rem; font-size: 0.85rem; color: var(--text-dim);">Control
                                        Type</th>
                                    <th
                                        style="padding: 0.5rem; font-size: 0.85rem; color: var(--text-dim); text-align: center;">
                                        Is Key</th>
                                    <th style="padding: 0.5rem; width: 40px;"></th>
                                </tr>
                            </thead>
                            <tbody id="gridBody">
                                <!-- Dynamic rows will be added here -->
                            </tbody>
                        </table>
                        <button type="button" onclick="addRow()"
                            style="margin-top: 1rem; background: rgba(255,255,255,0.05); border: 1px dashed var(--glass-border); color: var(--text-dim); padding: 0.5rem 1rem; border-radius: 8px; cursor: pointer; width: 100%; font-family: inherit;">
                            + Add Another Column
                        </button>
                    </div>
                </div>

                <div class="form-group"
                    style="margin-top: 2rem; border-top: 1px solid var(--glass-border); padding-top: 2rem;">
                    <label>Other Action Button Configuration</label>
                    <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 1rem; margin-top: 1rem;">
                        <div
                            style="background: rgba(255,255,255,0.03); padding: 1rem; border-radius: 12px; border: 1px solid var(--glass-border);">
                            <div style="display: flex; align-items: center; gap: 0.5rem; margin-bottom: 0.5rem;">
                                <asp:CheckBox ID="chkExport" runat="server" Text="Enable Export" CssClass="chk-toggle"
                                    data-target="exportOptions" />
                            </div>
                            <div id="exportOptions" style="display: none; padding-left: 1.5rem;">
                                <label style="font-size: 0.75rem; margin-bottom: 0.25rem;">Export Format:</label>
                                <asp:RadioButtonList ID="rblExportFormat" runat="server" RepeatDirection="Horizontal"
                                    CssClass="radio-group">
                                    <asp:ListItem Text="PDF" Value="PDF" Selected="True" />
                                    <asp:ListItem Text="Excel" Value="Excel" />
                                </asp:RadioButtonList>
                            </div>
                        </div>

                        <div
                            style="background: rgba(255,255,255,0.03); padding: 1rem; border-radius: 12px; border: 1px solid var(--glass-border);">
                            <div style="display: flex; align-items: center; gap: 0.5rem; margin-bottom: 0.5rem;">
                                <asp:CheckBox ID="chkImport" runat="server" Text="Enable Import" CssClass="chk-toggle"
                                    data-target="importOptions" />
                            </div>
                            <div id="importOptions" style="display: none; padding-left: 1.5rem;">
                                <label style="font-size: 0.75rem; margin-bottom: 0.25rem;">Upload Template
                                    (Excel):</label>
                                <asp:FileUpload ID="fuImportTemplate" runat="server" CssClass="input-control"
                                    style="font-size: 0.8rem; padding: 0.5rem;" />
                            </div>
                        </div>
                    </div>
                </div>

                <asp:HiddenField ID="hdnGridConfig" runat="server" />

                <div class="btn-container">
                    <asp:LinkButton ID="btnPreview" runat="server" CssClass="btn-action btn-preview"
                        OnClick="btnPreview_Click">
                        <span>Preview Screen</span>
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnDownloadAspx" runat="server" CssClass="btn-action btn-preview"
                        style="background: rgba(30, 41, 59, 0.7);" OnClick="btnDownloadAspx_Click">
                        <span>Download ASPX</span>
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnGenerate" runat="server" CssClass="btn-action btn-generate"
                        OnClick="btnGenerate_Click" OnClientClick="showLoading()">
                        <span>Download ZIP</span>
                    </asp:LinkButton>
                </div>

                <div id="statusBox" runat="server" class="status-msg">
                    <asp:Literal ID="litStatus" runat="server" />
                </div>
            </div>
        </form>

        <script>
            function addRow(data) {
                const tbody = document.getElementById('gridBody');
                const row = document.createElement('tr');
                row.style.borderBottom = "1px solid rgba(255,255,255,0.05)";

                const colName = data ? data.Name : '';
                const dataType = data ? data.DataType : 'System.String';
                const ctrlType = data ? data.ControlType : 'Label';
                const isKey = data ? data.IsKey : false;

                row.innerHTML = `
                    <td style="padding: 0.5rem;">
                        <input type="text" class="input-control col-name" value="${colName}" placeholder="Column ID" style="padding: 0.5rem;" />
                    </td>
                    <td style="padding: 0.5rem;">
                        <select class="input-control col-type" style="padding: 0.4rem;">
                            <option value="System.String" ${dataType === 'System.String' ? 'selected' : ''}>String</option>
                            <option value="System.Int32" ${dataType === 'System.Int32' ? 'selected' : ''}>Integer</option>
                            <option value="System.Boolean" ${dataType === 'System.Boolean' ? 'selected' : ''}>Boolean</option>
                        </select>
                    </td>
                    <td style="padding: 0.5rem;">
                        <select class="input-control col-ctrl" style="padding: 0.4rem;">
                            <option value="Label" ${ctrlType === 'Label' ? 'selected' : ''}>Label</option>
                            <option value="TextField" ${ctrlType === 'TextField' ? 'selected' : ''}>Text Field</option>
                            <option value="Checkbox" ${ctrlType === 'Checkbox' ? 'selected' : ''}>Checkbox</option>
                        </select>
                    </td>
                    <td style="padding: 0.5rem; text-align: center;">
                        <input type="checkbox" class="col-key" ${isKey ? 'checked' : ''} style="width: 18px; height: 18px; cursor: pointer;" />
                    </td>
                    <td style="padding: 0.5rem; text-align: center;">
                        <button type="button" onclick="this.closest('tr').remove()" style="background: none; border: none; color: #f87171; cursor: pointer; font-size: 1.2rem;">&times;</button>
                    </td>
                `;
                tbody.appendChild(row);
            }

            function syncGridConfig() {
                const rows = document.querySelectorAll('#gridBody tr');
                const config = [];
                rows.forEach(row => {
                    const name = row.querySelector('.col-name').value.trim();
                    if (name) {
                        config.push({
                            Name: name,
                            DataType: row.querySelector('.col-type').value,
                            ControlType: row.querySelector('.col-ctrl').value,
                            IsKey: row.querySelector('.col-key').checked
                        });
                    }
                });
                document.getElementById('<%= hdnGridConfig.ClientID %>').value = JSON.stringify(config);
            }

            function showLoading() {
                syncGridConfig();
                var btn = document.getElementById('<%= btnGenerate.ClientID %>');
                btn.value = 'Processing...';
                setTimeout(function () { btn.disabled = true; }, 10);
                return true;
            }

            // Also sync for preview
            document.getElementById('<%= btnPreview.ClientID %>').addEventListener('click', syncGridConfig);

            window.onload = function () {
                const hdn = document.getElementById('<%= hdnGridConfig.ClientID %>');
                if (hdn.value) {
                    const config = JSON.parse(hdn.value);
                    config.forEach(addRow);
                } else {
                    // Default rows
                    addRow({ Name: 'Module', DataType: 'System.String', ControlType: 'Label', IsKey: true });
                    addRow({ Name: 'Machine', DataType: 'System.String', ControlType: 'Label', IsKey: true });
                    addRow({ Name: 'Status', DataType: 'System.Boolean', ControlType: 'Checkbox', IsKey: false });
                    addRow({ Name: 'Value', DataType: 'System.Int32', ControlType: 'TextField', IsKey: false });
                }

                var statusBox = document.getElementById('<%= statusBox.ClientID %>');
                if (statusBox && statusBox.innerText.trim() !== "") {
                    statusBox.style.display = "block";
                }

                // Toggle logic for Export/Import
                document.querySelectorAll('.chk-toggle input').forEach(chk => {
                    const targetId = chk.closest('.chk-toggle').getAttribute('data-target');
                    const target = document.getElementById(targetId);

                    const updateVisibility = () => {
                        target.style.display = chk.checked ? 'block' : 'none';
                    };

                    chk.addEventListener('change', updateVisibility);
                    updateVisibility(); // Initial state
                });
            }
        </script>
    </body>

    </html>