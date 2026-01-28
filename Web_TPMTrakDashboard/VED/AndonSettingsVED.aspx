<%@ Page Title="VED Industries" Language="C#" AutoEventWireup="true" CodeBehind="AndonSettingsVED.aspx.cs" Inherits="Web_TPMTrakDashboard.Andon.AndonSettingsVED" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Settings</title>
    <%--<link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />--%>

    <link href="../AndonContent/bootstrap.min.css" rel="stylesheet" />
    <script src="../AndonScripts/jquery-3.1.0.min.js"></script>
    <script src="../AndonScripts/bootstrap.min.js"></script>

    <style>
        #menu {
            position: fixed;
            top: 0;
            background: #1a2732;
            box-shadow: 0 0 10px black;
        }

        .HeaderImage {
            flex: 1;
            float: left;
            width: 0px;
        }

        .legendStyle {
            display: block;
            width: 20%;
            padding: 0;
            margin-bottom: 0px;
            font-size: 18px;
            color: #333;
        }

        .legendStyleSetting {
            display: block;
            width: 29%;
            padding: 0;
            margin-bottom: 0px;
            font-size: 18px;
            color: #333;
        }

        .tableStyle {
            width: 100%;
            max-width: 100%;
            font-size: 14px;
            margin-bottom: 0px;
        }

        .table > tbody > tr > td {
            padding: 4px;
            vertical-align: top;
            border-top: 1px solid white;
        }

        .headerFixer tr th {
            padding: 4px 6px;
            font-size: 14px;
            background-color: #2e6886;
            border: none;
            color: white;
            position: sticky;
            top: 0px;
            z-index: 20;
        }

        .headerFixer tr td {
            padding: 4px 6px;
            font-size: 14px;
            border: none;
            border-bottom: 1px solid #d6d7df;
        }

        .pick-a-color-markup .pick-a-color {
            height: 40px;
        }

        label {
            font-weight: unset;
        }

        .tblSetting {
            margin-bottom: 0px;
        }

        .centerText {
            text-align: center;
        }

        .gvScreenList tbody tr td {
            background: white;
        }

        .tableStyle tbody tr td {
            font-weight: bold;
        }

        .tblfontSettings tbody tr td {
            vertical-align: middle;
            border: 1px solid #ddd;
        }
    </style>

    <link href="../AndonContent/bootstrap-glyphicons.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>


                <div>
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-lg-12">
                                <div>
                                    <div class="navbar navbar-fixed-top text-center" style="background-color: #1a2732;" id="menu">
                                        <div class="HeaderImage">
                                            <asp:Image ID="companyLogo" runat="server" CssClass="companyicon" Style="width: 200px; height: 56px" />
                                        </div>
                                        <span id="headerName" style="color: white; font-weight: bold; font-size: 30px; margin: auto; line-height: 60px;">Andon Settings</span>
                                        <div class="HeaderImageRight" style="display: inline;">
                                            <asp:Image ID="Image1" runat="server" CssClass="companyicon" ImageUrl="/Image/UI_Settings.png" Style="width: 70px; height: 50px; margin-top: 7px;" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">

                            <div class="col-lg-5 col-md-5 col-sm-5" style="margin-top: 62px; display: flex; flex-wrap: wrap; align-content: center; justify-content: center; align-items: center; flex-direction: row;">
                                <fieldset style="border: 2px solid black; width: 95%; background: aliceblue; font-weight: bold;">
                                    <legend class="legendStyle" style="font-weight: bold; width: 20% !important;">Andon Settings</legend>
                                    <table class="tableStyle table">
                                        <tr>
                                            <td>Device Name</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDeviceName" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><span>Font Settings</span></td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlFontName" Style="height: 28px; padding: 3px 12px;" CssClass="form-control">
                                                    <asp:ListItem Value="Arial">Arial</asp:ListItem>
                                                    <asp:ListItem Value="Arial Black">Arial Black</asp:ListItem>
                                                    <asp:ListItem Value="Calibri">Calibri</asp:ListItem>
                                                    <asp:ListItem Value="Segoe UI">Segoe UI</asp:ListItem>
                                                    <asp:ListItem Value="Segoe UI Semibold">Segoe UI Semibold</asp:ListItem>
                                                    <asp:ListItem Value="Segoe WP Black">Segoe WP Black</asp:ListItem>
                                                    <asp:ListItem Value="Times New Roman">Times New Roman</asp:ListItem>
                                                    <asp:ListItem Value="Verdana">Verdana</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlFontStyle" CssClass="form-control" Style="height: 28px; padding: 3px 12px;">
                                                    <asp:ListItem Value="Regular">Regular</asp:ListItem>
                                                    <asp:ListItem Value="Bold">Bold</asp:ListItem>
                                                    <asp:ListItem Value="Italic">Italic</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="padding-top: 15px;">
                                                <span>Data Refresh Interval (in Sec.)</span>
                                            </td>
                                            <td colspan="2" style="padding-top: 15px;">
                                                <asp:DropDownList runat="server" ID="ddlDataRefreshInterval" CssClass="form-control" Style="height: 28px; padding: 3px 12px;">
                                                    <asp:ListItem Value="30">30</asp:ListItem>
                                                    <asp:ListItem Value="40">40</asp:ListItem>
                                                    <asp:ListItem Value="50">50</asp:ListItem>
                                                    <asp:ListItem Value="60">60</asp:ListItem>
                                                    <asp:ListItem Value="70">70</asp:ListItem>
                                                    <asp:ListItem Value="80">80</asp:ListItem>
                                                    <asp:ListItem Value="90">90</asp:ListItem>
                                                    <asp:ListItem Value="110">110</asp:ListItem>
                                                    <asp:ListItem Value="120">120</asp:ListItem>
                                                    <asp:ListItem Value="130">130</asp:ListItem>
                                                    <asp:ListItem Value="140">140</asp:ListItem>
                                                    <asp:ListItem Value="150">150</asp:ListItem>
                                                    <asp:ListItem Value="160">160</asp:ListItem>
                                                    <asp:ListItem Value="170">170</asp:ListItem>
                                                    <asp:ListItem Value="180">180</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>Screen Flip Interval (in Sec.)</span>
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList runat="server" ID="ddlScreenFlipInterval" CssClass="form-control" Style="height: 28px; padding: 3px 12px;">
                                                    <asp:ListItem Value="10">10</asp:ListItem>
                                                    <asp:ListItem Value="20">20</asp:ListItem>
                                                    <asp:ListItem Value="30">30</asp:ListItem>
                                                    <asp:ListItem Value="40">40</asp:ListItem>
                                                    <asp:ListItem Value="50">50</asp:ListItem>
                                                    <asp:ListItem Value="60">60</asp:ListItem>
                                                    <asp:ListItem Value="90">90</asp:ListItem>
                                                    <asp:ListItem Value="120">120</asp:ListItem>
                                                    <asp:ListItem Value="150">150</asp:ListItem>
                                                    <asp:ListItem Value="180">180</asp:ListItem>
                                                    <asp:ListItem Value="210">210</asp:ListItem>
                                                    <asp:ListItem Value="240">240</asp:ListItem>
                                                    <asp:ListItem Value="270">270</asp:ListItem>
                                                    <asp:ListItem Value="300">300</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="padding-top: 15px;">
                                                <span>Enable Slide Show For</span>
                                            </td>
                                            <td style="padding-top: 15px;">
                                                <asp:CheckBox runat="server" ID="chkImage" Text="Image" AutoPostBack="true" OnCheckedChanged="chkImage_CheckedChanged" />
                                            </td>
                                            <td style="padding-top: 15px;">
                                                <asp:CheckBox runat="server" ID="chkVideo" Text="Video" AutoPostBack="true" OnCheckedChanged="chkVideo_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><span>Image Path (JPEG,PNG,JPG)</span></td>
                                            <td colspan="2">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 100%;">
                                                            <asp:TextBox runat="server" ID="txtPathImg" CssClass="form-control" Style="height: 28px; padding: 3px 12px;" />
                                                        </td>
                                                        <%--<td>
                                                    <asp:Image ID="imgfolder" runat="server" AlternateText="Browse Image Path" ImageUrl="/Image/FolderBrowse.png" Width="30" Style="margin-left: 10px; float: right" /></td>--%>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><span>Video Path</span></td>
                                            <td colspan="2">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 100%">
                                                            <asp:TextBox runat="server" ID="txtPathVideo" CssClass="form-control" Style="height: 28px; padding: 3px 12px;" />
                                                        </td>
                                                        <%-- <td>
                                                    <asp:Image ID="imgvideofolder" runat="server" AlternateText="Browse Image Path" ImageUrl="/Image/FolderBrowse.png" Width="30" Style="margin-left: 10px; float: right;" /></td>--%>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><span>Enable Slide Show After Sec.</span></td>
                                            <td colspan="2">
                                                <asp:DropDownList runat="server" ID="ddlSlideShow" CssClass="form-control" Style="height: 28px; padding: 3px 12px;">
                                                    <asp:ListItem Value="10">10</asp:ListItem>
                                                    <asp:ListItem Value="20">20</asp:ListItem>
                                                    <asp:ListItem Value="30">30</asp:ListItem>
                                                    <asp:ListItem Value="40">40</asp:ListItem>
                                                    <asp:ListItem Value="50">50</asp:ListItem>
                                                    <asp:ListItem Value="60">60</asp:ListItem>
                                                    <asp:ListItem Value="70">70</asp:ListItem>
                                                    <asp:ListItem Value="80">80</asp:ListItem>
                                                    <asp:ListItem Value="90">90</asp:ListItem>
                                                    <asp:ListItem Value="100">100</asp:ListItem>
                                                    <asp:ListItem Value="110">110</asp:ListItem>
                                                    <asp:ListItem Value="120">120</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>

                                        <tr style="display: none;">
                                            <td style="padding-top: 15px">Show Curved Boxes</td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkShowCurvedBoxes" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="padding-top: 15px;">Emoji Settings</td>
                                            <td style="padding-top: 15px;">
                                                <asp:CheckBox runat="server" ID="chkShowEmoji" ClientIDMode="Static" Text="Show Emoji" />
                                            </td>
                                            <td style="padding-top: 15px;">
                                                <asp:DropDownList runat="server" ID="ddlEmojiSize" ClientIDMode="Static" Style="height: 28px; padding: 3px 12px;" ToolTip="Emoji Size" CssClass="form-control">
                                                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                    <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                                    <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                    <asp:ListItem Text="60" Value="60"></asp:ListItem>
                                                    <asp:ListItem Text="70" Value="70"></asp:ListItem>
                                                    <asp:ListItem Text="80" Value="80"></asp:ListItem>
                                                    <asp:ListItem Text="90" Value="90"></asp:ListItem>
                                                    <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td style="padding-top: 15px">Footer Setting</td>
                                            <td style="padding-top: 15px;">
                                                <asp:CheckBox runat="server" ID="chkshowFooter" Text="Show Footer" ClientIDMode="Static" />
                                            </td>
                                            <td style="padding-top: 15px;">
                                                <asp:CheckBox runat="server" ID="chkShowMessage" Text="Show Message" ClientIDMode="Static" />
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>Scrolling Message</td>
                                            <td colspan="2">
                                                <asp:TextBox runat="server" ID="txtScrollingMessage" ClientIDMode="Static" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="padding-top: 15px;">(Andon)Sort By</td>
                                            <td style="padding-top: 15px;">
                                                <asp:DropDownList runat="server" ID="ddlsortByName" Style="height: 28px; padding: 3px 12px;" CssClass="form-control">
                                                    <asp:ListItem Value="OverAllEfficiency">OEE</asp:ListItem>
                                                    <asp:ListItem Value="ProductionEfficiency">PE</asp:ListItem>
                                                    <asp:ListItem Value="AvailabilityEfficiency">AE</asp:ListItem>
                                                    <asp:ListItem Value="DownTime">Down Time</asp:ListItem>
                                                    <asp:ListItem Value="UtilisedTime">Utilised Time</asp:ListItem>
                                                    <asp:ListItem Value="Components">Component</asp:ListItem>
                                                    <asp:ListItem Value="Machineid">Machine</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="padding-top: 15px;">
                                                <asp:DropDownList runat="server" ID="ddlSortOrder" CssClass="form-control" Style="height: 28px; padding: 3px 12px;">
                                                    <asp:ListItem Value="Asc">Ascending</asp:ListItem>
                                                    <asp:ListItem Value="Desc">Descending</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-top: 15px;"><span>DateTime Format</span></td>
                                            <td style="padding-top: 15px;">
                                                <asp:DropDownList runat="server" ID="ddlDate" Style="height: 28px; padding: 3px 12px;" CssClass="form-control">
                                                    <asp:ListItem Text="dd/MMM/yyyy" Value="dd/MMM/yyyy"></asp:ListItem>
                                                    <asp:ListItem Text="dd-MMM-yy" Value="dd-MMM-yy"></asp:ListItem>
                                                    <asp:ListItem Text="MM/dd/yy" Value="MM/dd/yy"></asp:ListItem>
                                                    <asp:ListItem Text="MM/dd/yyyy" Value="MM/dd/yyyy"></asp:ListItem>
                                                    <asp:ListItem Text="yyyy-MM-dd" Value="yyyy-MM-dd"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="padding-top: 15px;">
                                                <asp:DropDownList runat="server" ID="ddlTime" CssClass="form-control" Style="height: 28px; padding: 3px 12px;">
                                                    <asp:ListItem Text="hh:mm tt" Value="hh:mm tt"></asp:ListItem>
                                                    <asp:ListItem Text="HH:mm" Value="HH:mm"></asp:ListItem>
                                                    <asp:ListItem Text="HH:mm:ss" Value="HH:mm:ss"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Andon Data Display Frequency(Andon)
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td colspan="2" style="text-align: right;">
                                                <asp:Button runat="server" CssClass="btn btn-success" Text="Save" ID="btnSave" Style="font-weight: bold; height: 30px; padding: 3px 15px; background: #108000" OnClick="btnSave_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset style="border: 2px solid black; width: 95%; background: aliceblue; font-weight: bold;">
                                    <legend class="legendStyle" style="font-weight: bold; width: 20% !important;">Device Settings</legend>
                                    <table class="tableStyle table">
                                        <tr>
                                            <td>Run Option</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlRunOptions" CssClass="form-control" ClientIDMode="Static" AutoPostBack="true">
                                                    <asp:ListItem Text="Run By Cell" Value="RunByCell"></asp:ListItem>
                                                    <asp:ListItem Text="Run By Machine" Value="RunByMachine"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <span style="font-weight: bold;">Use Custom Width for Cockpit Screen&nbsp;&nbsp;&nbsp;
                                                        <asp:CheckBox runat="server" ID="chkUseCustomWidth" ClientIDMode="Static" />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Button runat="server" ID="btnRestoretodefaults" ClientIDMode="Static" CssClass="btn btn-Defualt" Text="Restore To Defaults" OnClientClick="return RestoreDefaultCockpitValues();" />
                                                </span>
                                                <div class="DivDimension" style="display: inline-flex; padding-top: 15px;">
                                                    <table runat="server" id="tblDimension" clientidmode="static">
                                                        <tr>
                                                            <td style="width: 125px; font-weight: bold;">Box Width
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtBoxWidth" ClientIDMode="Static" Width="80" CssClass="form-control allow-numbers"></asp:TextBox>
                                                            </td>
                                                            <td colspan="3">
                                                                <div style="display: inline-flex">
                                                                    <table>
                                                                        <tr>
                                                                            <td style="padding-left: 10px; width: 150px; font-weight: bold;">Box Margins:</td>
                                                                            <td style="width: 150px; font-weight: bold;">Top & Bottom</td>
                                                                            <td title="Adjust Vertical Space">
                                                                                <asp:TextBox runat="server" ID="txtTopMargin" ClientIDMode="Static" CssClass="form-control allow-numbers" Width="50"></asp:TextBox>
                                                                            </td>
                                                                            <td style="width: 150px; padding-left: 10px; font-weight: bold;">Left & Right</td>
                                                                            <td title="Adjust Horizontal Space">
                                                                                <asp:TextBox runat="server" ID="txtLeftMargin" CssClass="form-control allow-numbers" ClientIDMode="Static" Width="50"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>

                                                            <td colspan="5" style="font-size: small; padding-top: 10px; font-style: italic">Note: <b>Total Box Width</b> = (<b>Box Width</b> + 2 * <b>Left&Right</b> Margin)
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" class="" style="padding-top: 10px;text-align: right;">
                                                <asp:Button ID="btnSaveComputerName" ClientIDMode="Static" runat="server" Style="font-weight: bold; height: 30px; padding: 3px 15px; background: #108000" Text="Save" OnClick="btnSaveComputerName_Click" CssClass="btn btn-success" />
                                            </td>
                                        </tr>
                                    </table>

                                </fieldset>
                            </div>
                            <div class="col-lg-7 col-md-7 col-sm-7" style="margin-top: 62px; display: flex; flex-wrap: wrap; align-content: center; justify-content: center; align-items: center; flex-direction: row;">
                                <fieldset style="border: 2px solid black; width: 95%; background: aliceblue;">
                                    <legend class="legendStyle" style="font-weight: bold; width: 8% !important;">Settings</legend>

                                    <div>
                                        <table class="tableStyle table">
                                            <tr>
                                                <td>Screen Type</td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlSettingType" CssClass="form-control" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlSettingType_SelectedIndexChanged">
                                                        <asp:ListItem Text="Andon Screens Settings" Value="ScreenSetting"></asp:ListItem>
                                                        <asp:ListItem Text="Cockpit Andon Parameter Settings" Value="CockpitAndonParameters"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trCellID">
                                                <td>Cell ID</td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlCellID" OnSelectedIndexChanged="ddlCellID_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" Width="200">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trMachineFontSize">
                                                <td>Machine Font Size</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtMachineFontSize" CssClass="form-control" TextMode="Number" Width="100"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                    <div style="max-height: 57vh; overflow: auto;">
                                        <asp:GridView runat="server" ID="gvScreenList" ClientIDMode="Static" AutoGenerateColumns="false" CssClass="table table-bordered headerFixer gvScreenList">
                                            <Columns>
                                                <asp:BoundField HeaderText="Key" DataField="ValueInText" />
                                                <asp:TemplateField HeaderText="Screen Name">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtScreenName" ClientIDMode="Static" Text='<%# Eval("ScreenName") %>' CssClass="form-control screenName"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Is Visible?" ItemStyle-CssClass="text-center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkIsVisible" Checked='<%# Eval("IsVisible") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView runat="server" ID="gridviewSetting" AutoGenerateColumns="false" ShowFooter="false" CssClass="table table-bordered  tbl tblSetting headerFixer" OnRowDataBound="gridviewSetting_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Column">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblColumn" Text='<%# Bind("ValueInText") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Custom Column Name" ItemStyle-Width="24%">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="lblCustomColumnName" Text='<%# Bind("ValueInText2") %>' CssClass="form-control" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sort Order">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="lblSortOrder" Text='<%# Bind("ValueInInt") %>' CssClass="form-control" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Visibility" ItemStyle-CssClass="centerText">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkVisibility" Checked='<%# Bind("ValueInBool") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Text Align" HeaderStyle-Width="100">
                                                    <ItemTemplate>
                                                        <asp:HiddenField runat="server" ID="hdnTextAlign" ClientIDMode="Static" Value='<%# Bind("TextAlign") %>' />
                                                        <asp:DropDownList runat="server" ID="ddlTextAlign" CssClass="form-control">
                                                            <asp:ListItem Text="Left" Value="Left"></asp:ListItem>
                                                            <asp:ListItem Text="Right" Value="Right"></asp:ListItem>
                                                            <asp:ListItem Text="Center" Value="Center"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Label FontSize">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="lblLabelFontSize" Text='<%# Bind("LabelFontSize") %>' CssClass="form-control" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Data FontSize">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="lblDataFontSize" Text='<%# Bind("DataFontSize") %>' CssClass="form-control" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                    <div id="divScreenFontSettings" runat="server">
                                        <div style="text-align: center; background: #e9d5d5;">
                                            <asp:Label runat="server" ID="lblScreenName" Text="Andon Font Settings" ClientIDMode="Static" Style="font-weight: bold; font-size: 18px;"></asp:Label>
                                        </div>
                                        <div style="margin-top: 10px;">
                                            <table class="tableStyle table tblfontSettings" style="width: 100%; background: white;">
                                                <tr>
                                                    <td>OEE Chart font size</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtDonutChartFontSize" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </td>
                                                    <td>Pie Chart font size</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtPieChartFontSize" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="text-align: center !important; font-size: 18px; background: #ddd;">Column chart</td>
                                                </tr>
                                                <tr>
                                                    <td>X-Axis labels</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtColumnxAxisFontSize" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </td>
                                                    <td>Y-Axis labels</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtColumnyAxisFontSize" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Data labels</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtColumnDatalabelFontSize" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="text-align: center !important; font-size: 18px; background: #ddd;">Pareto chart</td>
                                                </tr>
                                                <tr>
                                                    <td>X-Axis labels</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtParetoxAxisFontSize" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </td>
                                                    <td>Y-Axis labels</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtParetoyAxisFontSize" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Data labels(Column)</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtParetoColumnDataLabelFontSize" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </td>
                                                    <td>Data labels(Pareto)</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtParetoDataLabelFontSize" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="text-align: center !important; font-size: 18px; background: #ddd;">Table Data</td>
                                                </tr>
                                                <tr>
                                                    <td>Header</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txttabledataHeaderFontSize" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </td>
                                                    <td>Content</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txttabledatacontentfontsize" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div style="margin-top: 10px;">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td>
                                                    <asp:Button runat="server" CssClass="btn btn-success" Text="Return to Andon" ID="btnAndon" Style="height: 40px; padding: 3px 15px; background: navy; font-weight: bold;" OnClick="btnAndon_Click" />
                                                </td>
                                                <td style="text-align: right;">
                                                    <asp:Button runat="server" CssClass="btn btn-success" Text="Save" ID="btnScreenSave" Style="font-weight: bold; height: 30px; padding: 3px 15px; background: #108000" OnClick="btnScreenSave_Click" OnClientClick="return ValidateScreen();" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlSettingType" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlCellID" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
        <script type="text/javascript">

            $(document).ready(function () {
                $('.allowNumber').keypress(function (evt) {
                    debugger;
                    var charCode = (evt.which) ? evt.which : evt.keyCode;
                    var pos = evt.target.selectionStart;
                    if (charCode == 48 && pos == 0) {
                        return false
                    } else if ((charCode < 48 || charCode > 57)) {
                        return false;
                    }
                    return true;
                });
                if($("#chkUseCustomWidth").is(":checked")) {
                    $(".DivDimension").show();
                }
                else
                $(".DivDimension").hide();

                //$.unblockUI({});
            });
            $("#chkUseCustomWidth").on("change", function () {
                if ($("#chkUseCustomWidth").is(":checked")) {
                    $(".DivDimension").show();
                }
                else
                    $(".DivDimension").hide();
            });



            function ValidateScreen() {

                //showloader();
                $("#gvScreenList tr:gt(0)").each(function () {
                    if ($(this).find(".screenName").val().trim() == "" || $(this).find(".screenName").val().trim() == undefined) {
                        $(this).find(".screenName").focus();
                        //$.unblockUI({});
                        alert("Screen Name is mandatory.");
                        return false;
                    }
                });
                return true;
            }

            function showloader() {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                return true;
            }


            function RestoreDefaultCockpitValues() {
                $("#txtBoxWidth").val("200");
                $("#txtTopMargin").val("3");
                $("#txtLeftMargin").val("3");
                return false;
            }

            $("#btnSaveComputerName").on("click", function () {
                if ($("#chkUseCustomWidth").is(":checked")) {
                    if ($("#txtBoxWidth").val() == "" || $("#txtBoxWidth").val() == "0") {
                        alert("Box Width Can not be Empty/Zero.");
                        $("#txtBoxWidth").focus();
                        return false;
                    }
                    else if ($("#txtTopMargin").val() == "" || $("#txtTopMargin").val() == "0") {
                        alert("Margin Value Can not be Empty/Zero.");
                        $("#txtTopMargin").focus();
                        return false;
                    }
                    else if ($("#txtLeftMargin").val() == "" || $("#txtLeftMargin").val() == "0") {
                        alert("Margin Value Can not be Empty/Zero.");
                        $("#txtLeftMargin").focus();
                        return false;
                    }
                    else {
                        if (Number($("#txtBoxWidth").val()) < 200) {
                            alert("Box Width Can not be less than 200.");
                            $("#txtBoxWidth").focus();
                            return false;
                        }
                        else if (Number($("#txtTopMargin").val() < 3) && Number($("#txtTopMargin").val() > 50)) {
                            alert("Margin should be in range 3 - 50.");
                            $("#txtTopMargin").focus();
                            return false;
                        }
                        else if (Number($("#txtLeftMargin").val() < 3) && Number($("#txtLeftMargin").val() > 50)) {
                            alert("Margin should be in range 3 - 50.");
                            $("#txtLeftMargin").focus();
                            return false;
                        }
                    }
                }
                return true;
            });

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                $('.allowNumber').keypress(function (evt) {
                    debugger;
                    var charCode = (evt.which) ? evt.which : evt.keyCode;
                    var pos = evt.target.selectionStart;
                    if (charCode == 48 && pos == 0) {
                        return false
                    } else if ((charCode < 48 || charCode > 57)) {
                        return false;
                    }
                    return true;
                });

                $(document).ready(function () {
                    if ($("#chkUseCustomWidth").is(":checked")) {
                        $(".DivDimension").show();
                    }
                    else
                        $(".DivDimension").hide();
                });

                $("#chkUseCustomWidth").on("change", function () {
                    if ($("#chkUseCustomWidth").is(":checked")) {
                        $(".DivDimension").show();
                    }
                    else
                        $(".DivDimension").hide();
                });

                $("#btnSaveComputerName").on("click", function () {
                    debugger;
                    if ($("#chkUseCustomWidth").is(":checked")) {
                        if ($("#txtBoxWidth").val() == "" || $("#txtBoxWidth").val() == "0") {
                            alert("Box Width Can not be Empty/Zero.");
                            $("#txtBoxWidth").focus();
                            return false;
                        }
                        else if ($("#txtTopMargin").val() == "" || $("#txtTopMargin").val() == "0") {
                            alert("Margin Value Can not be Empty/Zero.");
                            $("#txtTopMargin").focus();
                            return false;
                        }
                        else if ($("#txtLeftMargin").val() == "" || $("#txtLeftMargin").val() == "0") {
                            alert("Margin Value Can not be Empty/Zero.");
                            $("#txtLeftMargin").focus();
                            return false;
                        }
                        else {
                            if (Number($("#txtBoxWidth").val()) < 200) {
                                alert("Box Width Can not be less than 200.");
                                $("#txtBoxWidth").focus();
                                return false;
                            }
                            else if (Number($("#txtTopMargin").val() < 3) && Number($("#txtTopMargin").val() > 50)) {
                                alert("Margin should be in range 3 - 50.");
                                $("#txtTopMargin").focus();
                                return false;
                            }
                            else if (Number($("#txtLeftMargin").val() < 3) && Number($("#txtLeftMargin").val() > 50)) {
                                alert("Margin should be in range 3 - 50.");
                                $("#txtLeftMargin").focus();
                                return false;
                            }
                        }
                    }
                    return true;
                });
                //$.unblockUI({});
            });
        </script>
    </form>
</body>
</html>
