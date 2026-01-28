<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AndonSettingTableView.aspx.cs" Inherits="Web_TPMTrakDashboard.GenericAndon.AndonSettingTableView" %>

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

        .legendstylesettings {
            display: block;
            width: 25%;
            padding: 0;
            margin-bottom: 0px;
            font-size: 18px;
            color: #333;
            border: 0px;
            /*font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;*/
        }

        .AndonColumnSettings > tbody > tr > th {
            background-color: #263958db;
            text-align: center;
            color: white;
            vertical-align: middle
        }

        .tblGeneral > tbody > tr > td {
            padding: 5px;
        }

        .legendStyle {
            display: block;
            width: 40%;
            padding: 0;
            margin-bottom: 0px;
            font-size: 18px;
            color: #333;
            border: 0px;
        }
    </style>


    <%-- <script src="../AndonScripts/jquery-3.1.0.min.js"></script>
     <script src="../AndonScripts/bootstrap.min.js"></script>--%>
    <link href="../AndonContent/bootstrap-glyphicons.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <div>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-lg-12" style="background: #1a2732; height: 6vh; vertical-align: middle; box-shadow: 2px 2px 5px grey;">
                                <div style="float: left">
                                    <asp:Image runat="server" ID="ImgLogo" ImageUrl="/Image/menuIcon1.jpg" />
                                </div>
                                <div style="text-align: center; padding-top: 9px;">
                                    <span style="color: white; font-size: xx-large">Andon Settings</span>
                                </div>
                            </div>

                            <div class="col-lg-6">
                                <fieldset runat="server" style="border-color: black; border: 2px solid black;">
                                    <legend class="legendstylesettings" style="font-size: 20px; margin-left: 15px; font-weight: bold;">Andon Table Settings</legend>
                                    <div style="margin: 10px 0 5px 10px;">
                                        <div>
                                            <table>
                                                <tr>
                                                    <td style="width: 100px; font-weight: bold;">Cell ID</td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlCellID" CssClass="form-control" OnSelectedIndexChanged="ddlCellID_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="height: 60vh; overflow: auto; border: 1px solid silver; margin: 10px 10px 10px 0px;">
                                            <asp:ListView runat="server" ID="lvParameterDetails" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                                                <LayoutTemplate>

                                                    <table class="table table-bordered  tbl tblSetting headerFixer" id="AndonColumnSettings" clientidmode="static" style="border-color: black !important; width: 98%;">
                                                        <tr>
                                                            <th>Column Name</th>
                                                            <th>Display Text</th>
                                                            <th>Sort Order</th>
                                                            <th>Visible</th>
                                                            <th>Text Align</th>
                                                            <th>Label Font Size</th>
                                                            <th>Data Font Size</th>
                                                        </tr>
                                                        <tr id="itemplaceholder" runat="server"></tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                                            <asp:Label runat="server" ID="txtColumnName" Text='<%# Eval("ValueInText") %>'></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDisplayText" Text='<%# Eval("ValueInText2") %>' CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 20px;">
                                                            <asp:TextBox runat="server" ID="txtSortOrder" Text='<%# Eval("ValueInInt") %>' CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:CheckBox runat="server" ID="chkVisibility" Checked='<%# Eval("ValueInBool") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddlTextAlign" CssClass="form-control">
                                                                <asp:ListItem Text="Left" Value="left" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Center" Value="center"></asp:ListItem>
                                                                <asp:ListItem Text="Right" Value="right"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtLabelFontSize" Text='<%# Eval("LabelFontSize") %>' CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDataFontSize" Text='<%# Eval("DataFontSize") %>' CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                    </div>
                                    <div>
                                        <table style="width: 98%; margin: 10px;">
                                            <tr>
                                                <td style="text-align: center">
                                                    <asp:Button runat="server" ID="BtnSave" Text="Save" CssClass="btn btn-success" OnClick="BtnSave_Click" />
                                                    <asp:Button runat="server" ID="BtnReturn" Text="Return To Andon" CssClass="btn btn-success" OnClick="BtnReturn_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </fieldset>
                            </div>

                            <div class="col-lg-5">
                                <fieldset runat="server" style="border-color: black; border: 1px solid black;">
                                    <legend class="legendStyle" style="font-size: 20px; margin-left: 15px; font-weight: bold;">Andon General Settings</legend>
                                    <table class="tblGeneral" style="width: 98%; margin: 10px; padding: 10px;">
                                        <tr>
                                            <td>Computer Name</td>
                                            <td colspan="2">
                                                <asp:Label runat="server" ID="txtDeviceName" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Andon Page Title</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPageTitle" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Font Settings</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlFontFamily" CssClass="form-control" ClientIDMode="Static">
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
                                                <asp:DropDownList runat="server" ID="ddlFontStyle" CssClass="form-control" ClientIDMode="Static">
                                                    <asp:ListItem Text="Regular" Value="regular"></asp:ListItem>
                                                    <asp:ListItem Text="Bold" Value="bold"></asp:ListItem>
                                                    <asp:ListItem Text="Italic" Value="italic"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Data Refresh Interval (ss)</td>
                                            <td colspan="2">
                                                <asp:DropDownList runat="server" ID="ddlDataRefreshInterval" CssClass="form-control" ClientIDMode="Static">
                                                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                    <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                                    <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                    <asp:ListItem Text="60" Value="60"></asp:ListItem>
                                                    <asp:ListItem Text="70" Value="70"></asp:ListItem>
                                                    <asp:ListItem Text="80" Value="80"></asp:ListItem>
                                                    <asp:ListItem Text="90" Value="90"></asp:ListItem>
                                                    <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                                    <asp:ListItem Text="110" Value="110"></asp:ListItem>
                                                    <asp:ListItem Text="120" Value="120"></asp:ListItem>
                                                    <asp:ListItem Text="130" Value="130"></asp:ListItem>
                                                    <asp:ListItem Text="140" Value="140"></asp:ListItem>
                                                    <asp:ListItem Text="150" Value="150"></asp:ListItem>
                                                    <asp:ListItem Text="160" Value="160"></asp:ListItem>
                                                    <asp:ListItem Text="170" Value="170"></asp:ListItem>
                                                    <asp:ListItem Text="180" Value="180"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Screen Flip Interval (ss)</td>
                                            <td colspan="2">
                                                <asp:DropDownList runat="server" ID="ddlScreenFlipInterval" CssClass="form-control" ClientIDMode="Static">
                                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                    <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                                    <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                    <asp:ListItem Text="60" Value="60"></asp:ListItem>
                                                    <asp:ListItem Text="70" Value="70"></asp:ListItem>
                                                    <asp:ListItem Text="80" Value="80"></asp:ListItem>
                                                    <asp:ListItem Text="90" Value="90"></asp:ListItem>
                                                    <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                                    <asp:ListItem Text="110" Value="110"></asp:ListItem>
                                                    <asp:ListItem Text="120" Value="120"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trAndonDisplayFrequency">
                                            <td>Andon Data Display Frequency</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control"></asp:DropDownList>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Run Option Setting
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList runat="server" ID="ddlRunOption" CssClass="form-control" ClientIDMode="Static" AutoPostBack="true">
                                                    <asp:ListItem Text="Run By Cell" Value="RunByCell"></asp:ListItem>
                                                    <asp:ListItem Text="Run By Machine" Value="RunByMachine"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Date Time Format</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlDateFormat" ClientIDMode="Static" CssClass="form-control">
                                                    <asp:ListItem Text="dd/MM/yyyy" Value="dd/MM/yyyy"></asp:ListItem>
                                                    <asp:ListItem Text="dd-MMM-yyyy" Value="dd-MM-yyyy"></asp:ListItem>
                                                    <asp:ListItem Text="yyyy-MM-dd" Value="yyyy-MM-dd"></asp:ListItem>
                                                    <asp:ListItem Text="MM/dd/yy" Value="MM/dd/yy"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlTimeFormat" ClientIDMode="Static" CssClass="form-control">
                                                    <asp:ListItem Text="HH:mm:ss" Value="HH:mm:ss"></asp:ListItem>
                                                    <asp:ListItem Text="HH:mm" Value="HH:mm"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Sort By</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlSortBy" ClientIDMode="Static" CssClass="form-control">
                                                    <asp:ListItem Value="Machineid">Machine ID</asp:ListItem>
                                                    <asp:ListItem Value="OverAllEfficiency">OEE</asp:ListItem>
                                                    <asp:ListItem Value="ProductionEfficiency">PE</asp:ListItem>
                                                    <asp:ListItem Value="AvailabilityEfficiency">AE</asp:ListItem>
                                                    <asp:ListItem Value="DownTime">Down Time</asp:ListItem>
                                                    <asp:ListItem Value="UtilisedTime">Utilised Time</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlSortOrder" CssClass="form-control">
                                                    <asp:ListItem Text="ASC" Value="ASC"></asp:ListItem>
                                                    <asp:ListItem Text="DESC" Value="DESC"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Footer Settings</td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkEnableFooter" Text="Enable Footer" ClientIDMode="Static" />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkShowMsg" Text="Show Message" ClientIDMode="Static" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Scrolling Text</td>
                                            <td colspan="2">
                                                <asp:TextBox runat="server" ID="txtScrollingText" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" class="text-center">
                                                <asp:Button Text="Save" ID="BtnSaveGenric" runat="server" CssClass="btn btn-success" OnClick="BtnSaveGenric_Click" />
                                            </td>
                                        </tr>

                                    </table>
                                </fieldset>
                            </div>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
