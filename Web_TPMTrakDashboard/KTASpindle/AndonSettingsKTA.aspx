<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AndonSettingsKTA.aspx.cs" Inherits="Web_TPMTrakDashboard.KTASpindle.AndonSettingsKTA" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Andon Settings</title>
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
            width: 20%;
            padding: 1px;
            padding-left: 5px;
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

        .headerFixer tr td {
            padding: 4px 6px;
            font-size: 14px;
            border: none;
            border-bottom: 1px solid #d6d7df;
        }

        .headerFixer tr th {
            background-color: #2e6886;
            position: sticky;
            top: -1px;
            z-index: 20;
            color: white;
        }

        label {
            font-weight: unset;
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
                                    <div class="navbar navbar-fixed-top text-center" style="background-color: #3777bc;" id="menu">
                                        <div class="HeaderImage">
                                            <asp:Image ID="CompanyLogo" runat="server" CssClass="companyicon" ImageUrl="/Image/UI_Settings.png" Style="width: 200px; height: 56px;" />
                                        </div>
                                        <span id="headername" style="color: white; font-weight: bold; font-size: 30px; margin: auto; line-height: 60px;">Andon Settings</span>
                                    </div>
                                </div>
                                <div class="col-lg-4"></div>
                                <div class="col-lg-6" style="margin-top: 100px; width: 700px;">
                                    <fieldset style="border-color: black">
                                        <legend class="legendStyleSetting">App Settings</legend>
                                        <table class="tableStyle table">
                                            
                                            <tr>
                                                <td style="width: 200px;">
                                                    <span>Store  Andon Title</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox runat="server" ID="txtAndonTitle" CssClass="form-control" Style="height: 28px; padding: 3px 12px;" />
                                                </td>
                                            </tr>
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
                                                <td><span>Data Refresh Interval (ss)</span></td>
                                                <td colspan="2">
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
                                                <td><span>Screen Flip Interval (ss)</span></td>
                                                <td colspan="2">
                                                    <asp:DropDownList runat="server" ID="ddlScreenFlipInterval" CssClass="form-control" Style="height: 28px; padding: 3px 12px;">
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
                                                <td><span>DateTime Format</span></td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlDate" Style="height: 28px; padding: 3px 12px;" CssClass="form-control">
                                                        <asp:ListItem Text="dd/MMM/yyyy" Value="dd/MMM/yyyy"></asp:ListItem>
                                                        <asp:ListItem Text="dd-MMM-yy" Value="dd-MMM-yy"></asp:ListItem>
                                                        <asp:ListItem Text="MM/dd/yy" Value="MM/dd/yy"></asp:ListItem>
                                                        <asp:ListItem Text="MM/dd/yyyy" Value="MM/dd/yyyy"></asp:ListItem>
                                                        <asp:ListItem Text="yyyy-MM-dd" Value="yyyy-MM-dd"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlTime" CssClass="form-control" Style="height: 28px; padding: 3px 12px;">
                                                        <asp:ListItem Text="hh:mm tt" Value="hh:mm tt"></asp:ListItem>
                                                        <asp:ListItem Text="HH:mm" Value="HH:mm"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:Button runat="server" ID="btnSave" CssClass="btn btn-success" Text="Save" Style="height: 30px; padding: 3px 15px;" OnClick="btnSave_Click" />
                                                
                                                </td>
                                            </tr>
                                            <tr style="background-color: #3777bc;color: white;font-weight:bold;font-size:16px;">
                                                <td colspan="3">Andon View Settings</td>
                                            </tr>
                                            <tr style="display:none;">
                                                <td style="width: 200px;">
                                                    <span>Plant</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" Style="height: 28px; padding: 3px 12px;" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged" AutoPostBack="true" />
                                                </td>
                                            </tr>
                                             <tr>
                                                <td>Cell ID</td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="ddlCellID" runat="server" CssClass="select form-control" OnSelectedIndexChanged="ddlCellID_SelectedIndexChanged" AutoPostBack="True" Style="height: 28px; padding: 3px 12px;">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td><span style="margin-right: 10px;">Store Machine Font Size</span></td>
                                                <td colspan="2">
                                                    <asp:TextBox runat="server" ID="txtProductionMachineFontSize" Text='<%# Bind("LabelFontSize") %>' CssClass="form-control" Style="height: 28px; padding: 3px 12px;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trHeaderFontSize">
                                                <td><span>Header Font Size</span></td>
                                                <td colspan="2">
                                                    <asp:DropDownList runat="server" ID="ddlHeaderFontSize" Style="height: 28px; padding: 3px 12px;" CssClass="form-control">
                                                        <asp:ListItem Value="8">8</asp:ListItem>
                                                        <asp:ListItem Value="9">9</asp:ListItem>
                                                        <asp:ListItem Value="10">10</asp:ListItem>
                                                        <asp:ListItem Value="11">11</asp:ListItem>
                                                        <asp:ListItem Value="12">12</asp:ListItem>
                                                        <asp:ListItem Value="13">13</asp:ListItem>
                                                        <asp:ListItem Value="14">14</asp:ListItem>
                                                        <asp:ListItem Value="15">15</asp:ListItem>
                                                        <asp:ListItem Value="16">16</asp:ListItem>
                                                        <asp:ListItem Value="17">17</asp:ListItem>
                                                        <asp:ListItem Value="18">18</asp:ListItem>
                                                        <asp:ListItem Value="19">19</asp:ListItem>
                                                        <asp:ListItem Value="20">20</asp:ListItem>
                                                        <asp:ListItem Value="21">21</asp:ListItem>
                                                        <asp:ListItem Value="22">22</asp:ListItem>
                                                        <asp:ListItem Value="23">23</asp:ListItem>
                                                        <asp:ListItem Value="24">24</asp:ListItem>
                                                        <asp:ListItem Value="25">25</asp:ListItem>
                                                        <asp:ListItem Value="26">26</asp:ListItem>
                                                        <asp:ListItem Value="27">27</asp:ListItem>
                                                        <asp:ListItem Value="28">28</asp:ListItem>
                                                        <asp:ListItem Value="29">29</asp:ListItem>
                                                        <asp:ListItem Value="30">30</asp:ListItem>
                                                        <asp:ListItem Value="31">31</asp:ListItem>
                                                        <asp:ListItem Value="32">32</asp:ListItem>
                                                        <asp:ListItem Value="33">33</asp:ListItem>
                                                        <asp:ListItem Value="34">34</asp:ListItem>
                                                        <asp:ListItem Value="35">35</asp:ListItem>
                                                        <asp:ListItem Value="36">36</asp:ListItem>
                                                        <asp:ListItem Value="37">37</asp:ListItem>
                                                        <asp:ListItem Value="38">38</asp:ListItem>
                                                        <asp:ListItem Value="39">39</asp:ListItem>
                                                        <asp:ListItem Value="40">40</asp:ListItem>
                                                        <asp:ListItem Value="41">41</asp:ListItem>
                                                        <asp:ListItem Value="42">42</asp:ListItem>
                                                        <asp:ListItem Value="43">43</asp:ListItem>
                                                        <asp:ListItem Value="44">44</asp:ListItem>
                                                        <asp:ListItem Value="45">45</asp:ListItem>
                                                        <asp:ListItem Value="46">46</asp:ListItem>
                                                        <asp:ListItem Value="47">47</asp:ListItem>
                                                        <asp:ListItem Value="48">48</asp:ListItem>
                                                        <asp:ListItem Value="49">49</asp:ListItem>
                                                        <asp:ListItem Value="50">50</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                             <tr runat="server" id="trContentFontSize">
                                                <td><span>Content Font Size</span></td>
                                                <td colspan="2">
                                                    <asp:DropDownList runat="server" ID="ddlContentFontSize" Style="height: 28px; padding: 3px 12px;" CssClass="form-control">
                                                        <asp:ListItem Value="8">8</asp:ListItem>
                                                        <asp:ListItem Value="9">9</asp:ListItem>
                                                        <asp:ListItem Value="10">10</asp:ListItem>
                                                        <asp:ListItem Value="11">11</asp:ListItem>
                                                        <asp:ListItem Value="12">12</asp:ListItem>
                                                        <asp:ListItem Value="13">13</asp:ListItem>
                                                        <asp:ListItem Value="14">14</asp:ListItem>
                                                        <asp:ListItem Value="15">15</asp:ListItem>
                                                        <asp:ListItem Value="16">16</asp:ListItem>
                                                        <asp:ListItem Value="17">17</asp:ListItem>
                                                        <asp:ListItem Value="18">18</asp:ListItem>
                                                        <asp:ListItem Value="19">19</asp:ListItem>
                                                        <asp:ListItem Value="20">20</asp:ListItem>
                                                        <asp:ListItem Value="21">21</asp:ListItem>
                                                        <asp:ListItem Value="22">22</asp:ListItem>
                                                        <asp:ListItem Value="23">23</asp:ListItem>
                                                        <asp:ListItem Value="24">24</asp:ListItem>
                                                        <asp:ListItem Value="25">25</asp:ListItem>
                                                        <asp:ListItem Value="26">26</asp:ListItem>
                                                        <asp:ListItem Value="27">27</asp:ListItem>
                                                        <asp:ListItem Value="28">28</asp:ListItem>
                                                        <asp:ListItem Value="29">29</asp:ListItem>
                                                        <asp:ListItem Value="30">30</asp:ListItem>
                                                        <asp:ListItem Value="31">31</asp:ListItem>
                                                        <asp:ListItem Value="32">32</asp:ListItem>
                                                        <asp:ListItem Value="33">33</asp:ListItem>
                                                        <asp:ListItem Value="34">34</asp:ListItem>
                                                        <asp:ListItem Value="35">35</asp:ListItem>
                                                        <asp:ListItem Value="36">36</asp:ListItem>
                                                        <asp:ListItem Value="37">37</asp:ListItem>
                                                        <asp:ListItem Value="38">38</asp:ListItem>
                                                        <asp:ListItem Value="39">39</asp:ListItem>
                                                        <asp:ListItem Value="40">40</asp:ListItem>
                                                        <asp:ListItem Value="41">41</asp:ListItem>
                                                        <asp:ListItem Value="42">42</asp:ListItem>
                                                        <asp:ListItem Value="43">43</asp:ListItem>
                                                        <asp:ListItem Value="44">44</asp:ListItem>
                                                        <asp:ListItem Value="45">45</asp:ListItem>
                                                        <asp:ListItem Value="46">46</asp:ListItem>
                                                        <asp:ListItem Value="47">47</asp:ListItem>
                                                        <asp:ListItem Value="48">48</asp:ListItem>
                                                        <asp:ListItem Value="49">49</asp:ListItem>
                                                        <asp:ListItem Value="50">50</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td colspan="2">
                                                    <asp:Button runat="server" CssClass="btn btn-success" Text="Save" ID="btnSettingsSave" Style="height: 30px; padding: 3px 15px;" OnClick="btnSettingsSave_Click" />
                                                        <asp:Button runat="server" ID="btnAddon" CssClass="btn btn-success" Text="Return to Andon" Style="height: 30px; padding: 3px 15px;margin-left:10px;" OnClick="btnAddon_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                                <div class="col-lg-2"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
