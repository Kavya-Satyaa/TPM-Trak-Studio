<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="First.aspx.cs" Inherits="Web_TPMTrakDashboard.First" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Second Attempt</title>
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
            font-family: 'Outfit', sans-serif; 
            padding: 50px; 
            background: radial-gradient(circle at top right, #1e1b4b, #0f172a);
            color: var(--text);
            min-height: 100vh;
            margin: 0;
        }

        .card { 
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
        }

        @keyframes slideUp {
            from { opacity: 0; transform: translateY(20px); }
            to { opacity: 1; transform: translateY(0); }
        }

        h1 { 
            margin-top: 0;
            background: linear-gradient(to right, #818cf8, #c084fc);
            -webkit-background-clip: text;
            background-clip: text;
            -webkit-text-fill-color: transparent;
            font-size: 2.2rem;
            font-weight: 600;
        }

        .subtitle { color: var(--text-dim); margin-bottom: 2rem; }

        .controls-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
            gap: 1.25rem;
            margin-bottom: 2rem;
            align-items: flex-end;
        }

        .dropdown-container {
            display: flex;
            flex-direction: column;
            gap: 0.5rem;
        }

        label {
            font-size: 0.85rem;
            color: var(--text-dim);
            font-weight: 500;
            text-transform: uppercase;
            letter-spacing: 0.05em;
        }

        .dropdown-control {
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
        }

        .dropdown-control:focus {
            border-color: var(--primary);
            box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.2);
            background-color: rgba(15, 23, 42, 0.6);
        }

        .btn-view {
            background: var(--primary);
            color: white;
            border: none;
            padding: 0.85rem 2rem;
            border-radius: 12px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s;
        }

        .btn-view:hover { background: var(--primary-hover); transform: translateY(-2px); }

        .grid-container {
            margin-top: 2rem;
            overflow-x: auto;
            border-radius: 12px;
            background: rgba(15, 23, 42, 0.2);
        }

        .mGrid { width: 100%; border-collapse: collapse; }
        .mGrid th { background: rgba(255,255,255,0.05); color: var(--text-dim); text-align: left; padding: 1rem; font-size: 0.9rem; }
        .mGrid td { padding: 1rem; border-top: 1px solid var(--glass-border); color: var(--text); }
        
        input[type="date"]::-webkit-calendar-picker-indicator {
            filter: invert(1);
            cursor: pointer;
        }

        .date-range-container {
            display: flex;
            gap: 10px;
            width: 100%;
        }

        .date-range-container .dropdown-control {
            flex: 1;
        }

        .filter-span-2 {
            grid-column: span 2;
        }
        
            .time-info {
                margin-top: 2.5rem;
                font-size: 0.8rem;
                color: var(--text-dim);
                border-top: 1px solid var(--glass-border);
                padding-top: 1.25rem;
            }

            /* Multiselect Styling */
            .btn-group {
                width: 100% !important;
                position: relative !important;
            }
            .multiselect.dropdown-toggle {
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
            }
            .multiselect-container {
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
            }
            .btn-group.open .multiselect-container {
                display: block !important;
            }
            .multiselect-container li {
                margin: 2px 0;
            }
            .multiselect-container li a {
                padding: 0 !important;
                display: block !important;
                color: inherit !important;
                text-decoration: none !important;
            }
            .multiselect-container label.checkbox {
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
            }
            .multiselect-container li.active label.checkbox,
            .multiselect-container label.checkbox:hover {
                background: rgba(99, 102, 241, 0.2) !important;
                color: #818cf8 !important;
            }
            .multiselect-container input[type="checkbox"] {
                margin: 0 12px 0 0 !important;
                width: 16px !important;
                height: 16px !important;
                position: static !important;
                cursor: pointer !important;
                flex-shrink: 0;
            }
            /* Dark Scrollbar */
            .multiselect-container::-webkit-scrollbar {
                width: 6px;
            }
            .multiselect-container::-webkit-scrollbar-track {
                background: transparent;
            }
            .multiselect-container::-webkit-scrollbar-thumb {
                background: rgba(255, 255, 255, 0.1);
                border-radius: 10px;
            }
            .multiselect-container::-webkit-scrollbar-thumb:hover {
                background: rgba(255, 255, 255, 0.2);
            }
            .multiselect-selected-text {
                overflow: hidden !important;
                text-overflow: ellipsis !important;
                white-space: nowrap !important;
                max-width: 180px !important;
            }

        </style>
        <link href="Scripts/MultiCheckBox/bootstrap-multiselect.css" rel="stylesheet" />
    </head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />
        <asp:UpdatePanel ID="upMain" runat="server">
            <ContentTemplate>
                <div class="card">
                    <h1>Second Attempt</h1>
                    <p class="subtitle">Dynamic Data Explorer</p>
                    
                    <div class="controls-grid">
                        <div id="pnlDdl1" runat="server" class="dropdown-container">
                    <label>Plant</label>
                    <asp:DropDownList ID="ddlDdl1" runat="server" CssClass="dropdown-control" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed" />
                </div>
                <div id="pnlDdl2" runat="server" class="dropdown-container">
                    <label>Machine</label>
                    <asp:DropDownList ID="ddlDdl2" runat="server" CssClass="dropdown-control" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed" />
                </div>
                <div id="pnlDdl3" runat="server" class="dropdown-container">
                    <label>Component</label>
                    <asp:DropDownList ID="ddlDdl3" runat="server" CssClass="dropdown-control" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed" />
                </div>
                <div id="pnlDdl4" runat="server" class="dropdown-container">
                    <label>Operation</label>
                    <asp:DropDownList ID="ddlDdl4" runat="server" CssClass="dropdown-control" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed" />
                </div>
                <div id="pnlDdl5" runat="server" class="dropdown-container">
                    <label>Shift</label>
                    <asp:DropDownList ID="ddlDdl5" runat="server" CssClass="dropdown-control" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed" />
                </div>
                <div id="pnlDate" runat="server" class="dropdown-container">
                    <label>Date</label>
                    <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="dropdown-control" />
                </div>

                        <div class="dropdown-container">
                            <asp:Button ID="btnView" runat="server" Text="View Report" CssClass="btn-view" OnClick="btnView_Click" />
                        </div>
                        <div class="dropdown-container" style="flex-direction: row; align-items: flex-end;">
                        <asp:Button ID="btnExport" runat="server" Text="Export to PDF" CssClass="btn-view" style="background: #10b981; margin-left:10px;" OnClick="btnExport_Click" />

                        </div>
                    </div>

                    <div class="grid-container">
                        <asp:GridView ID="gvReport" runat="server" CssClass="mGrid" AutoGenerateColumns="false" 
                            OnRowDeleting="gvReport_RowDeleting" />
                    </div>

                    <div class="time-info">
                        <span>Last Updated: <asp:Label ID="lblTime" runat="server" /></span>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
        </asp:UpdatePanel>
    </form>

    <script src="Scripts/Checkbox/jquery-3.3.1.min.js"></script>
    <script src="Scripts/Checkbox/bootstrap.min.js"></script>
    <script src="Scripts/MultiCheckBox/bootstrap-multiselect.js"></script>
    <script type="text/javascript">
        var lastOpenDropdownId = null;

        function initMultiselect() {
            $('select[multiple]').each(function() {
                var $lb = $(this);
                var id = $lb.attr('id');

                $lb.multiselect({
                    includeSelectAllOption: true,
                    buttonClass: 'multiselect dropdown-toggle',
                    nonSelectedText: '-- Select --',
                    numberDisplayed: 1,
                    onDropdownShown: function() {
                        lastOpenDropdownId = id;
                    },
                    onDropdownHidden: function() {
                        if (lastOpenDropdownId === id) {
                            lastOpenDropdownId = null;
                        }
                    }
                });

                if (lastOpenDropdownId === id) {
                    $lb.parent().find('.btn-group').addClass('open');
                }
            });
        }

        $(document).ready(function() {
            initMultiselect();
        });

        if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
                initMultiselect();
            });
        }
    </script>
</body>
</html>"