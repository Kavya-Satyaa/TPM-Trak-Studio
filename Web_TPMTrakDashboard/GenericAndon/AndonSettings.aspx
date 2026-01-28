<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AndonSettings.aspx.cs" Inherits="Web_TPMTrakDashboard.Andon_settings.AndonSettings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="../AndonContent/bootstrap.min.css" rel="stylesheet" />
    <link href="../Scripts/ColorPickerJs/css/pick-a-color-1.2.2.min.css" rel="stylesheet" />
    <script src="../Scripts/ColorPickerJs/dependencies/jquery-1.9.1.min.js"></script>
    <script src="../Scripts/ColorPickerJs/dependencies/tinycolor-0.9.15.min.js"></script>
    <script src="../Scripts/ColorPickerJs/js/pick-a-color-1.2.2.min.js"></script>



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
        .tblSetting{
            margin-bottom: 0px;
        }
        .centerText{
            text-align: center;
        }
    </style>


    <%-- <script src="../AndonScripts/jquery-3.1.0.min.js"></script>
     <script src="../AndonScripts/bootstrap.min.js"></script>--%>
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
                                            <asp:Image ID="companyLogo" runat="server" CssClass="companyicon" ImageUrl="/Image/UI_Settings.png" Style="width: 200px; height: 56px" />
                                        </div>
                                        <span id="headerName" style="color: white; font-weight: bold; font-size: 30px; margin: auto; line-height: 60px;">Andon Settings</span>

                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6" style="margin-top: 62px;">
                                <fieldset style="border-color: black">
                                    <legend class="legendStyle">App Settings</legend>
                                    <table class="tableStyle table">
                                        <tr>
                                            <td><span>Font Settings</span></td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlFontName" Style="height: 28px; padding: 3px 12px;" CssClass="form-control">
                                                    <asp:ListItem Value="Aharoni">Aharoni</asp:ListItem>
                                                    <asp:ListItem Value="Arial">Arial</asp:ListItem>
                                                    <asp:ListItem Value="Arial Black">Arial Black</asp:ListItem>
                                                    <asp:ListItem Value="Baskerville Old Face">Baskerville Old Face</asp:ListItem>
                                                    <asp:ListItem Value="Bodoni MT Black">Bodoni MT Black</asp:ListItem>
                                                    <asp:ListItem Value="Bookman Old Style">Bookman Old Style</asp:ListItem>
                                                    <asp:ListItem Value="Calibri">Calibri</asp:ListItem>
                                                    <asp:ListItem Value="Cooper Black">Cooper Black</asp:ListItem>
                                                    <asp:ListItem Value="Californian FB">Californian FB</asp:ListItem>
                                                    <asp:ListItem Value="Constantia">Constantia</asp:ListItem>
                                                    <asp:ListItem Value="Elephant">Elephant</asp:ListItem>
                                                    <asp:ListItem Value="Georgia">Georgia</asp:ListItem>
                                                    <asp:ListItem Value="Goudy Old Style">Goudy Old Style</asp:ListItem>
                                                    <asp:ListItem Value="High Tower Text">High Tower Text</asp:ListItem>
                                                    <asp:ListItem Value="Segoe UI">Segoe UI</asp:ListItem>
                                                    <asp:ListItem Value="Segoe UI Semibold">Segoe UI Semibold</asp:ListItem>
                                                    <asp:ListItem Value="Segoe WP Black">Segoe WP Black</asp:ListItem>
                                                    <asp:ListItem Value="Times New Roman">Times New Roman</asp:ListItem>
                                                    <asp:ListItem Value="Tw Cen MT">Tw Cen MT</asp:ListItem>
                                                    <asp:ListItem Value="Verdana">Verdana</asp:ListItem>
                                                    <asp:ListItem Value="Vijaya">Vijaya</asp:ListItem>
                                                    <asp:ListItem Value="Vrinda">Vrinda</asp:ListItem>
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
                                            <td style="width: 200px;">
                                                <span>Andon Title</span>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox runat="server" ID="txtAndonTitle" CssClass="form-control" Style="height: 28px; padding: 3px 12px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Show Curved Boxes</td>
                                            <td colspan="2">
                                                <asp:CheckBox runat="server" ID="chkcurvedboxes" Text="" />
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
                                                    <asp:ListItem Value="5">5</asp:ListItem>
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

                                        <tr runat="server" id="trEmoji">
                                            <td>
                                                <span>Emoji Settings</span>
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkShowEmoji" Text="Show Emoji" Style="font-weight: unset" OnCheckedChanged="chkShowEmoji_CheckedChanged" AutoPostBack="true" />
                                            </td>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td><span style="margin-right: 7px;">Emoji Size</span></td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddlEmojiSize" CssClass="form-control" Style="height: 28px; padding: 3px 12px;">
                                                                <asp:ListItem Value="40">40</asp:ListItem>
                                                                <asp:ListItem Value="45">45</asp:ListItem>
                                                                <asp:ListItem Value="50">50</asp:ListItem>
                                                                <asp:ListItem Value="55">55</asp:ListItem>
                                                                <asp:ListItem Value="60">60</asp:ListItem>
                                                                <asp:ListItem Value="65">65</asp:ListItem>
                                                                <asp:ListItem Value="70">70</asp:ListItem>
                                                                <asp:ListItem Value="75">75</asp:ListItem>
                                                                <asp:ListItem Value="80">80</asp:ListItem>
                                                                <asp:ListItem Value="85">85</asp:ListItem>
                                                                <asp:ListItem Value="90">90</asp:ListItem>
                                                                <asp:ListItem Value="95">95</asp:ListItem>
                                                                <asp:ListItem Value="100">100</asp:ListItem>
                                                                <asp:ListItem Value="105">105</asp:ListItem>
                                                                <asp:ListItem Value="110">110</asp:ListItem>
                                                                <asp:ListItem Value="115">115</asp:ListItem>
                                                                <asp:ListItem Value="120">120</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
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
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><span>Sort By</span></td>
                                            <td>
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
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlSortOrder" CssClass="form-control" Style="height: 28px; padding: 3px 12px;">
                                                    <asp:ListItem Value="Ascending">Ascending</asp:ListItem>
                                                    <asp:ListItem Value="Descending">Descending</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <%-- <tr>
                             <td><span>Show On Monitor</span></td>
                             <td colspan="2">
                                  <asp:DropDownList runat="server" ID="ddlShowOnMonitor"  CssClass="form-control" style="height: 28px;padding: 3px 12px;">
                                       <asp:ListItem  Value="Primary">Primary</asp:ListItem>
                                       <asp:ListItem  Value="Secondary">Secondary</asp:ListItem>
                                   </asp:DropDownList>
                             </td>
                         </tr>--%>
                                        <tr>
                                            <td style="padding-top: 15px;">
                                                <span>Footer Settings</span>
                                            </td>
                                            <td style="padding-top: 15px;">
                                                <asp:CheckBox runat="server" ID="chkMsgBlock" Text="Show Msg Block" />
                                            </td>
                                            <td style="padding-top: 15px;">
                                                <asp:CheckBox runat="server" ID="chkFooterBlock" Text="Show Footer Block" Style="font-weight: unset" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><span>ScrollingText</span></td>
                                            <td colspan="2">
                                                <asp:TextBox runat="server" ID="txtScrollingText" TextMode="MultiLine" MaxLength="500" Width="400" CssClass="form-control" Style="height: 28px; padding: 3px 12px;" />
                                            </td>
                                        </tr>
                                        <tr id="trPoojaViewType" runat="server">
                                            <td><span>Pooja View Type</span></td>
                                            <td colspan="2">
                                                <asp:DropDownList runat="server" ID="ddlViewType" CssClass="form-control" Style="height: 28px; padding: 3px 12px;">
                                                    <asp:ListItem Value="Table" Text="Table"></asp:ListItem>
                                                    <asp:ListItem Value="Cockpit" Text="Cockpit"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Button runat="server" CssClass="btn btn-success" Text="Save" ID="btnSave" Style="height: 30px; padding: 3px 15px;" OnClick="btnSave_Click" />
                                                <asp:Button runat="server" CssClass="btn btn-success" Text="Return to Andon" ID="btnAndon" Style="height: 30px; padding: 3px 15px;" OnClick="btnAndon_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <div runat="server" id="computerRunOptionDiv">
                                    <fieldset style="border-color: black;">
                                        <legend class="legendStyleSetting">Run Option Setting</legend>
                                        <table class="tableStyle table">
                                            <tr>
                                                <td style="width: 85px; vertical-align: middle;"><span style="font-weight: bold;">Computer Name</span></td>
                                                <td style="width: 80px;">
                                                    <asp:Label runat="server" Text="" ID="lblComputerName" CssClass="form-control"></asp:Label></td>
                                                <td style="width: 85px; vertical-align: middle; padding-left: 30px;"><span>Run Option</span></td>
                                                <td style="width: 125px;">
                                                    <asp:DropDownList runat="server" ID="ddlRunOptions" CssClass="form-control" ClientIDMode="Static" AutoPostBack="true">
                                                        <asp:ListItem Text="Run By Cell" Value="RunByCell"></asp:ListItem>
                                                        <asp:ListItem Text="Run By Machine" Value="RunByMachine"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trFrequency">
                                                <td>Andon Frequency</td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control" ClientIDMode="Static">
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
                                                                <%--<td style="padding-left: 10px; width: 150px; font-weight: bold;">Box Margins</td>--%>
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
                                                <td colspan="4" class="text-center" style="padding-top: 10px;">
                                                    <asp:Button ID="btnSaveComputerName" ClientIDMode="Static" runat="server" Text="Save" OnClick="btnSaveComputerName_Click" CssClass="btn btn-success" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6" style="margin-top: 62px;">
                                <fieldset style="border-color: black">
                                    <legend class="legendStyleSetting">Cockpit Andon View Settings</legend>
                                    <table style="margin-bottom: 10px;">
                                        <tr>
                                            <td><span style="margin-right: 10px;">Cell ID</span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlCellID" runat="server" CssClass="select form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlCellID_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><span style="margin-right: 10px;">Font size for MachineID</span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="lblProductionMachineFontsize" Text='<%# Bind("LabelFontSize") %>' CssClass="form-control" Style="margin-top: 5px;" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="max-height: 60vh; overflow: auto;">

                                        <asp:GridView runat="server" ID="gridviewSetting" AutoGenerateColumns="false" ShowFooter="false" CssClass="table table-bordered  tbl tblSetting headerFixer" OnRowDataBound="gridviewSetting_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Column">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblColumn" Text='<%# Bind("ValueInText") %>'/>
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

                                    <fieldset style="border-color: black; margin-top: 20px;">
                                        <legend style="width: 43%; font-size: 18px; color: #333; margin-bottom: 0px;">Machine Efficiency Color Settings</legend>
                                        <div class="col-lg-12">
                                            <div class="col-lg-4">
                                                <table>
                                                    <tr>
                                                        <td><span style="font-weight: 700">Good</span></td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtGood" data-toggle="tooltip" name="border-color" runat="server" CssClass="pick-a-color form-control" Style="height: 34px;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="col-lg-4">
                                                <table>
                                                    <tr>
                                                        <td><span style="font-weight: 700">Moderate</span></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtModerate" data-toggle="tooltip" name="border-color" runat="server" CssClass="pick-a-color form-control" Style="height: 34px;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="col-lg-4">
                                                <table>
                                                    <tr>
                                                        <td><span style="font-weight: 700">Bad</span></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtBad" data-toggle="tooltip" name="border-color" runat="server" CssClass="pick-a-color form-control" Style="height: 34px;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </fieldset>

                                    <div style="margin-top: 20px; float: right;">
                                        <asp:Button runat="server" CssClass="btn btn-success" Text="Save" ID="btnSavesetting" Style="height: 30px; padding: 3px 15px;" OnClick="btnSavesetting_Click" />
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <script type="text/javascript">
            $(document).ready(function () {
                $(".pick-a-color").pickAColor();

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

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
                $(".pick-a-color").pickAColor();

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
                })

            });
        </script>
    </form>
</body>
</html>
