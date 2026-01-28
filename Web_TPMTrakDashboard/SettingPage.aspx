<%@ Page Title="Setting Information" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SettingPage.aspx.cs" Inherits="Web_TPMTrakDashboard.SettingPage" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <link href="Scripts/ColorPickerJs/css/pick-a-color-1.2.2.min.css" rel="stylesheet" />

    <script src="Scripts/ColorPickerJs/dependencies/tinycolor-0.9.15.min.js"></script>
    <script src="Scripts/ColorPickerJs/js/pick-a-color-1.2.2.min.js"></script>



    <%--<%: Scripts.Render("~/bundles/colorcss") %>
    <%: Styles.Render("~/bundles/colorjs") %>--%>
    <style>
        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            border-top: 0px solid #dddddd;
            padding: 2px;
        }

        .center {
            position: absolute;
            left: 50%;
            top: 50%;
            transform: translate(-50%, -50%);
        }

        .table {
            margin-bottom: 0px;
        }

        .HeaderCss {
            font-weight: bold;
            min-width: 100px;
        }

        .close:hover {
            color: red;
        }

        .tabHeader {
            background-color: #2E6886;
            color: white;
            font-size: 18px;
            font-weight: bold;
        }

        .modal {
            border-radius: 20px;
        }

        .downcodes {
            border: white;
            border-color: white;
            border-style: double;
            white-space: nowrap;
            border-width: 100%;
            width: 100%;
            padding: 10px;
            margin-right: 5px;
            text-align: -webkit-center;
        }
    </style>

    <style>
        .btnCss {
            font-family: Calibri;
            height: 25px;
            width: 25px;
            background-color: #428bca;
            color: white;
            font-size: medium;
            border-style: none;
        }

        .headerStyle {
            position: sticky;
            width: 100%;
            top: -15px;
        }

        .row {
            margin-left: 0px;
        }

        .HeaderCss {
            color: white;
            background-color: #428bca;
            font-weight: bold;
            min-width: 100px;
        }

        .tabHeader {
            font-size: 1.2em;
        }

        .pick-a-color-markup .pick-a-color {
            height: 40px;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            vertical-align: middle;
        }

        .tblHAView {
            width: 100%;
        }

            .tblHAView tr th {
                color: white;
                background-color: #2E6886;
                padding: 3px;
            }

            .tblHAView tr td {
                color: black;
                background-color: white;
                border: 1px solid #dddddd;
                padding: 2px 3px;
            }
    </style>

    <div class="container-fluid">

        <%--style='font-family: <%= settings.AppUISettings.FontFamily %>; font-style: <%= settings.AppUISettings.FontStyle %>; font-weight: <%= settings.AppUISettings.FontStyle %>; margin-left: 3px; margin-right: 3px;'>--%>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="row text-center">
                    <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; color: white" meta:resourcekey="lblMessagesResource1"></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:Button ID="btnUploadeHide" runat="server" OnClick="btnUploadeHide_Click" Style="display: none;" meta:resourcekey="btnUploadeHideResource1" />
        <asp:UpdatePanel ID="update" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-lg-6" id="tdGeneralHide" runat="server">
                        <div class="panel panel-primary">
                            <div class="panel-heading tabHeader"><%=GetLocalResourceObject("Machine Efficiency Color Setting") %> </div>
                            <table class="table table-bordered">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtGood" data-toggle="tooltip" name="border-color" runat="server" class="pick-a-color form-control" meta:resourcekey="txtGoodResource1"></asp:TextBox>
                                    </td>
                                    <td><%=GetLocalResourceObject("Good") %></td>
                                    <td>
                                        <asp:TextBox ID="txtBad" data-toggle="tooltip" name="border-color" runat="server" class="pick-a-color form-control" meta:resourcekey="txtBadResource1"></asp:TextBox>
                                    </td>
                                    <td><%=GetLocalResourceObject("Bad") %></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtModerate" data-toggle="tooltip" name="border-color" runat="server" class="pick-a-color form-control" meta:resourcekey="txtModerateResource1"></asp:TextBox>
                                    </td>
                                    <td><%=GetLocalResourceObject("Moderate") %></td>
                                    <td colspan="2">
                                        <asp:Button data-toggle="tooltip" ID="btnSaveColor" runat="server" Text="<%$Resources:CommanResource, Save %>" CssClass="btn btn-primary toolTip" OnClick="btnSaveColor_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="panel panel-primary">
                            <div class="panel-heading tabHeader">Efficiency Color Settings&nbsp;&nbsp;&nbsp;<a href="OEQEPEColor.aspx" target="_blank" class="fa fa-arrow-right" style="font-size: 24px; color: wheat" role="button"><%=GetLocalResourceObject("GoTo") %></a></div>
                            <br />
                            <div class="panel-heading tabHeader"><b><%=GetLocalResourceObject("ResourceLink") %> </b>&nbsp;<a href="UpdateResourceFiles.aspx" target="_blank" class="fa fa-arrow-right" style="font-size: 24px; color: wheat" role="button"><%=GetLocalResourceObject("GoTo") %></a></div>
                        </div>
                        <div class="panel panel-primary">
                            <div class="panel-heading text-center">
                                <div class="col-lg-6">
                                    <%=GetLocalResourceObject("Column View Setting") %>
                                    <asp:DropDownList ID="ddlGridPages" runat="server" CssClass="form-control input-md" Width="180px"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlGridPages_SelectedIndexChanged" Style="display: inline;" meta:resourcekey="ddlGridPagesResource1">
                                        <asp:ListItem Value="WebTPMTrak" meta:resourcekey="ListItemResource3">Dashboard</asp:ListItem>
                                        <asp:ListItem Value="CockpitGridColumn" meta:resourcekey="ListItemResource4">Iconic View</asp:ListItem>
                                        <asp:ListItem Value="WebTPMTrakTableView" meta:resourcekey="ListItemResource5">Table View</asp:ListItem>
                                        <asp:ListItem Value="WebTPMTrakVDGProduction" meta:resourcekey="ListItemResource6">Cycle Analytics-Production View</asp:ListItem>
                                        <asp:ListItem Value="WebTPMTrakVDGDownTime">Cycle Analytics-DownData View</asp:ListItem>
                                        <%--meta:resourcekey="ListItemResource7"--%>
                                        <asp:ListItem Value="TPMTrakWebEnergyViewColumnSettings" meta:resourcekey="ListItemResource8">Energy Dashboard</asp:ListItem>
                                        <asp:ListItem Value="WebCockpitGridColumnAggregate" meta:resourcekey="ListItemResource8">Historical Analytics(Iconic And Table View)</asp:ListItem>
                                        <asp:ListItem Value="WebIonicViewAndon" meta:resourcekey="ListItemResource9">Andon Iconic View</asp:ListItem>
                                        <asp:ListItem Value="WebTableViewAndon" meta:resourcekey="ListItemResource10">Andon Table View</asp:ListItem>
                                        <asp:ListItem Value="ComponentInformation" meta:resourcekey="ListItemResource11">Component Information</asp:ListItem>
                                        <asp:ListItem Value="EnergyLiveData" meta:resourcekey="ListItemResource12">Energy Live Data</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-lg-6">
                                    <%=GetLocalResourceObject("Language") %>
                                    <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="form-control input-md" Width="250px" Style="display: inline;" AutoPostBack="true" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged">
                                        <asp:ListItem Value="en">English (United Kingdom)</asp:ListItem>
                                        <asp:ListItem Value="zh">中文（简体，PRC）</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <asp:GridView ID="grdViewAndonView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered " meta:resourcekey="grdViewAndonViewResource1">
                                <Columns>
                                    <asp:TemplateField HeaderText="Field Name" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:Label ID="lblValueInText" CssClass="control-label  ValiDate" runat="server" Text='<%# Bind("ValueInText") %>' meta:resourcekey="lblValueInTextResource1"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="text-center" ForeColor="White" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Display Name" meta:resourcekey="TemplateFieldResource2">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtValueInText2" CssClass="form-control input-sm " runat="server" Text='<%# Bind("ValueInText2") %>' meta:resourcekey="txtValueInText2Resource1" AutoCompleteType="Disabled"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="text-center" ForeColor="White" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sort Order" meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtValueInInt" CssClass="control-label ValiDate" runat="server" Text='<%# Bind("ValueInInt") %>' meta:resourcekey="lblsortorderTextResource1" AutoCompleteType="Disabled"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="text-center" ForeColor="White" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Visibility" meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("ValueInBool") %>' meta:resourcekey="chkSelectResource1" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="text-center" ForeColor="White" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#004080" Font-Bold="True" ForeColor="White" />
                            </asp:GridView>
                            <div style="margin-bottom: 5px; margin-top: 5px; margin-left: 5px; text-align: center">
                                <asp:Button ID="btnSaveColumnSetting" runat="server" Text="Save Column Setting" CssClass="btn btn-primary" OnClick="btnSaveColumnSetting_Click" meta:resourcekey="btnSaveColumnSettingResource1" />
                            </div>
                        </div>

                        <div class="panel panel-primary">
                            <div class="panel-heading tabHeader"><%=GetLocalResourceObject("MachineStatusColorSetting") %> </div>
                            <div style="overflow: visible;">
                                <asp:GridView runat="server" ID="GridViewColorCodes" AutoGenerateColumns="false" CssClass="table table-bordered" ShowHeaderWhenEmpty="true" Style="color: black;" ShowHeader="true" ClientIDMode="Static">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Machine Status">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfUpdate" runat="server" />
                                                <asp:Label runat="server" ID="lblStatus" Text='<%# Bind("Status")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="text-center" Height="40" ForeColor="White" BackColor="#2E6886" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Color Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtColorPicker" data-toggle="tooltip" name="border-color" runat="server" class="pick-a-color form-control" Text='<%# Bind("Colorcode") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="text-center" ForeColor="White" BackColor="#2E6886" />
                                            <ItemStyle CssClass="text-center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div style="width: 100%; text-align: center; color: red;">No Data Available.</div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div style="margin-bottom: 2px; margin-top: 2px; text-align: center">
                                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnSaveColorCodes" Text="Save Color Codes" OnClick="btnSaveColorCodes_Click" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-6" id="tdApplication" runat="server">
                        <div class="panel panel-primary">
                            <div class="panel-heading tabHeader"><%=GetLocalResourceObject("Application Setting") %>  </div>
                            <table class="table table-bordered">
                                <tr>
                                    <td>
                                        <label class="control-label"><%=GetLocalResourceObject("Downtime/Stoppage") %></label></td>
                                    <td>
                                        <asp:DropDownList ID="ddlDowntime" runat="server" CssClass="form-control input-md" meta:resourcekey="ddlDowntimeResource1">
                                            <asp:ListItem Value="hh" meta:resourcekey="ListItemResource1">hh</asp:ListItem>
                                            <asp:ListItem Value="mm" meta:resourcekey="ListItemResource2">min.</asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label"><%=GetLocalResourceObject("Font Size (px)") %></label></td>
                                    <td>
                                        <asp:DropDownList ID="ddlFontSize" runat="server" CssClass="form-control input-md" meta:resourcekey="ddlFontSizeResource1">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label"><%=GetLocalResourceObject("Cockpit Font Size (px)") %></label></td>
                                    <td>
                                        <asp:DropDownList ID="ddlCockpitFontSize" runat="server" CssClass="form-control input-md" meta:resourcekey="ddlCockpitFontSizeResource1">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label"><%=GetLocalResourceObject("Font Family") %></label></td>
                                    <td>
                                        <asp:DropDownList ID="ddlFontFamily" runat="server" CssClass="form-control input-md" meta:resourcekey="ddlFontFamilyResource1">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label"><%=GetLocalResourceObject("Font Style") %></label></td>
                                    <td>
                                        <asp:DropDownList ID="ddlFontStyle" runat="server" CssClass="form-control input-md" meta:resourcekey="ddlFontStyleResource1">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label"><%=GetLocalResourceObject("Show Smiley Image") %></label></td>
                                    <td>
                                        <asp:CheckBox ID="chkShowSmileyImg" runat="server" meta:resourcekey="chkShowSmileyImgResource1" />
                                    </td>
                                </tr>
                                <tr id="imgHide" style="display: none;">
                                    <td>
                                        <label class="control-label"><%=GetLocalResourceObject("Smiley Image Size (px)") %></label></td>
                                    <td>
                                        <asp:DropDownList ID="ddlSmileImageSize" runat="server" CssClass="form-control input-md" meta:resourcekey="ddlSmileImageSizeResource1"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Button ID="btnSaveGeneralPageSetting" runat="server" OnClick="btnSaveGeneralPageSetting_Click" Text="<%$Resources:CommanResource, Save %>" CssClass="btn btn-primary" /></td>
                                </tr>
                            </table>
                        </div>
                        <div class="panel panel-primary">
                            <div class="panel-heading tabHeader"><%=GetLocalResourceObject("Company Logo") %> </div>
                            <br />
                            <table class="table table-bordered">
                                <tr>
                                    <td>
                                        <asp:FileUpload data-toggle="tooltip" CssClass="toolTip" ID="FileUpload1" runat="server" meta:resourcekey="FileUpload1Resource1" />
                                    </td>
                                    <td>
                                        <button type="button" data-toggle="tooltip" id="btnUpload" class="btn btn-sm btn-primary toolTip"><%=GetLocalResourceObject("Upload files") %></button>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table>
                            <tr>
                                <td>
                                    <div class="downcodes">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBox runat="server" ID="checkboxtoHide16losses" OnCheckedChanged="checkboxtoHide16losses_CheckedChanged" AutoPostBack="true" />
                                                <span style="color: burlywood; margin-left: 20px"><b>Exclude TPM Trak Standard DownCodes</b></span>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Button runat="server" ID="btnMachineSortOrder" Text="Machine Custom Sort Order" CssClass="btn btn-primary" Style="width: auto; margin: 10px; margin-left: 0;" OnClick="btnMachineSortOrder_Click" ClientIDMode="Static" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table class="table table-bordered" style="background-color: white">
                            <tr class="panel-heading tabHeader">
                                <td colspan="2">
                                    <span style="color: white">Dashboard Default View</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="control-label">Default Year</label></td>
                                <td>
                                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control input-md" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                        <asp:ListItem Value="LastYear">Last Year</asp:ListItem>
                                        <asp:ListItem Value="ThisYear">This Year</asp:ListItem>
                                        <asp:ListItem Value="None"> </asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="control-label">Default Month</label></td>
                                <td>
                                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control input-md" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                                        <asp:ListItem Value="LastMonth">Last Month</asp:ListItem>
                                        <asp:ListItem Value="ThisMonth">This Month</asp:ListItem>
                                        <asp:ListItem Value="None"> </asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="control-label">Default Day</label></td>
                                <td>
                                    <asp:DropDownList ID="ddlDate" runat="server" CssClass="form-control input-md" AutoPostBack="true" OnSelectedIndexChanged="ddlDate_SelectedIndexChanged1">
                                        <asp:ListItem Value="Today">Today</asp:ListItem>
                                        <asp:ListItem Value="Yesterday">Yesterday</asp:ListItem>
                                        <asp:ListItem Value="Daybeforeyesterday">Day Before Yesterday</asp:ListItem>
                                        <asp:ListItem Value="None"> </asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;">
                                    <asp:Button ID="btnSave" runat="server" OnClick="btncolorsave_Click" Text="<%$Resources:CommanResource, Save %>" CssClass="btn btn-primary" /></td>
                            </tr>
                        </table>
                        <br />
                        <div class="col-lg-6" style="padding: 0px 5px; display: none;">
                            <table class="tblHAView">
                                <tr class="panel-heading tabHeader">
                                    <th colspan="2">
                                        <span style="color: white">Andon Default View</span>
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label">Production Title</label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtAndonTitle" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label">Andon Type</label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAndonType" runat="server" CssClass="form-control input-md">
                                            <asp:ListItem Value="DayWise">Day Wise</asp:ListItem>
                                            <asp:ListItem Value="ShiftWise">Shift Wise</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label">Data Refresh Interval(Sec.)</label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAndonDataRefreshInterval" runat="server" CssClass="form-control input-md">
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
                                    <td>
                                        <label class="control-label">Flip Interval(Sec.)</label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAndonFlipInterval" runat="server" CssClass="form-control input-md">
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
                                    <td colspan="2" style="text-align: center;">
                                        <asp:Button ID="btnAndonSave" runat="server" OnClick="btnAndonSave_Click" Text="<%$Resources:CommanResource, Save %>" CssClass="btn btn-primary" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label">Page Size for Table View</label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAndonPageSize" runat="server" CssClass="form-control input-md">
                                            <asp:ListItem Value="5">5</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="15">15</asp:ListItem>
                                            <asp:ListItem Value="20">20</asp:ListItem>
                                            <asp:ListItem Value="25">25</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center;">
                                        <asp:Button ID="Button1" runat="server" OnClick="btnAndonSave_Click" Text="<%$Resources:CommanResource, Save %>" CssClass="btn btn-primary" /></td>
                                </tr>
                            </table>
                            <br />
                            <%--<table class="tblHAView">
                                <tr class="panel-heading tabHeader">
                                    <th colspan="2">
                                        <span style="color: white">Default View</span>
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label">Landing Page</label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlLandingPage" runat="server" CssClass="form-control input-md">
                                            <asp:ListItem Value="Dashboard.aspx">Dashboard</asp:ListItem>
                                            <asp:ListItem Value="IonicView.aspx">Live Iconic View</asp:ListItem>
                                            <asp:ListItem Value="tableView.aspx">Live Table View</asp:ListItem>
                                            <asp:ListItem Value="IonicViewAggregated.aspx">Agg. Iconic View</asp:ListItem>
                                            <asp:ListItem Value="TableViewAggregate.aspx">Agg. Table View</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center">

                                        <asp:Button ID="btnLandingPageSave" runat="server" OnClick="btnLandingPageSave_Click" Text="<%$Resources:CommanResource, Save %>" CssClass="btn btn-primary" Style="margin-top: 4px" />
                                    </td>
                                </tr>
                            </table>--%>
                        </div>

                        <div class="col-lg-6" style="padding: 0px 5px">
                            <table class="tblHAView">
                                <tr class="panel-heading tabHeader">
                                    <th colspan="2">
                                        <span style="color: white">Historical Analytics Cockpit Default View</span>
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label">Ionic Default View</label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlHAIonicView" runat="server" CssClass="form-control input-md">
                                            <asp:ListItem Value="Plantwise">Plant View</asp:ListItem>
                                            <asp:ListItem Value="cellwise">Cell View</asp:ListItem>
                                            <asp:ListItem Value="Machinewise">Machine View</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label">Table Default View</label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlHATableView" runat="server" CssClass="form-control input-md">
                                            <asp:ListItem Value="Plantwise">Plant View</asp:ListItem>
                                            <asp:ListItem Value="cellwise">Cell View</asp:ListItem>
                                            <asp:ListItem Value="Machinewise">Machine View</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center">

                                        <asp:Button ID="btnHAViewSave" runat="server" OnClick="btnHAViewSave_Click" Text="<%$Resources:CommanResource, Save %>" CssClass="btn btn-primary" Style="margin-top: 4px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-lg-6" style="padding: 0px 5px;">
                            <table class="tblHAView">
                                <tr class="panel-heading tabHeader">
                                    <th colspan="2">
                                        <span style="color: white">Live Analytics Cockpit Default View</span>
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label">Iconic Default View</label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlLAIonicView" runat="server" CssClass="form-control input-md">
                                            <asp:ListItem Value="Plantwise">Plant View</asp:ListItem>
                                            <asp:ListItem Value="cellwise">Cell View</asp:ListItem>
                                            <asp:ListItem Value="Machinewise">Machine View</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="control-label">Table Default View</label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlLATableView" runat="server" CssClass="form-control input-md">
                                            <asp:ListItem Value="Plantwise">Plant View</asp:ListItem>
                                            <asp:ListItem Value="cellwise">Cell View</asp:ListItem>
                                            <asp:ListItem Value="Machinewise">Machine View</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center">

                                        <asp:Button ID="btnLAViewSave" runat="server" OnClick="btnLAViewSave_Click" Text="<%$Resources:CommanResource, Save %>" CssClass="btn btn-primary" Style="margin-top: 4px" />
                                    </td>
                                </tr>
                            </table>

                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="modal fade model " id="myModal" role="dialog" style="width: auto;">
            <div class="modal-dialog" style="width: 355px;">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <!-- Modal content-->
                        <div class="modal-content" style="width: 355px;">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title" style="color: blue; font-weight: bold">MachineSortOrder</h4>
                            </div>

                            <div class="modal-body" style="height: 400px; z-index: 9999; overflow: auto; width: 350px; position: relative;">
                                <div class="headerStyle">
                                    <table style="width: 100%; background-color: midnightblue; color: white; text-align: center" class="table table-bordered">
                                        <tr style="color: white;">
                                            <td style="color: white; height: 40px; width: 151px;">Machine Name</td>
                                            <td style="color: white; height: 40px;">Sort Order</td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="overflow: auto;">
                                    <asp:GridView runat="server" ID="gvMachineSortOrder" AutoGenerateColumns="false" CssClass="table table-bordered" ShowHeaderWhenEmpty="true" Style="color: black;" ShowHeader="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Machine Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMachine" runat="server" Width="120" Text='<%# Eval("machineid") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="text-center" Height="40" ForeColor="White" BackColor="MidnightBlue" />

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sort Order">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtSortOrder" runat="server" Width="120" TextMode="Number" min="0" step="1" Text='<%# Bind("SortOrder") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="text-center" ForeColor="White" BackColor="MidnightBlue" />
                                                <ItemStyle CssClass="text-center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div style="width: 100%; text-align: center; color: red;">No Machines Available.</div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>


                            </div>
                            <div class="modal-footer" style="text-align: right">
                                <asp:Button runat="server" ID="save" OnClick="save_Click" CssClass="btn btn-primary" Text="Save" />
                            </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <script type='text/javascript'>
            function openModal() {
                $('[id*=myModal]').modal('show');
            }
            $("#btnMachineSortOrder").click(function () {
                $('[id*=myModal]').modal('show');
            });
        </script>
    </div>
    <script>
        $(document).ready(function () {
            $(".pick-a-color").pickAColor();
            $("#btnUpload").click(function () {
                if ($("[id$=FileUpload1]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseSelectFileforUploading") %>");
                    $("[id$=FileUpload1]").focus();
                    return false;
                }
                else {
                    var file = $("[id$=FileUpload1]").val();
                    var exts = ['jpg', 'jpeg', 'gif', 'png'];
                    // first check if file field has any value
                    if (file) {
                        // split file name at dot
                        var get_ext = file.split('.');
                        // reverse name to check extension
                        get_ext = get_ext.reverse();
                        // check file type is valid as given in 'exts' array
                        if ($.inArray(get_ext[0].toLowerCase(), exts) > -1) {
                            $("[id$=btnUploadeHide]").trigger("click");
                        } else {
                            alert('<%=GetLocalResourceObject("InvalidfileUploading") %>');
                        }
                    }
                    return false;
                }
                $("#btnMachineSortOrder").click(function () {
                    $('[id*=myModal]').modal('show');
                });
            });
            $(document).ready(function () {

                function alignModal() {
                    var modalDialog = $(this).find(".modal-dialog");
                    /* Applying the top margin on modal dialog to align it vertically center */
                    modalDialog.css("margin-top", Math.max(0, ($(window).height() - modalDialog.height()) / 2));
                }
                // Align modal when it is displayed
                $(".modal").on("shown.bs.modal", alignModal);

                // Align modal when user resize the window
                $(window).on("resize", function () {
                    $(".modal:visible").each(alignModal);
                });
            });
            $("[id$=chkShowSmileyImg]").click(function () {
                if ($("[id$=chkShowSmileyImg]").is(':checked'))
                    $("#imgHide").show();
                else {
                    $("#imgHide").hide();
                    $("[id$=ddlSmileImageSize]").val('0');
                }
            });
            checkBoxManage();
            function checkBoxManage() {
                if ($("[id$=chkShowSmileyImg]").is(':checked'))
                    $("#imgHide").show();
                else {
                    $("#imgHide").hide();
                    $("[id$=ddlSmileImageSize]").val('0');
                }
            }
            $("[id$=btnSaveGeneralPageSetting]").click(function () {
                if ($("[id$=ddlFontSize]").val() == "0") {
                    alert("<%=GetLocalResourceObject("PleaseEntertheFontSize") %>");
                    $("[id$=ddlFontSize]").focus();
                    return false;
                }
                if ($("[id$=ddlFontFamily]").val() == "0") {
                    alert("<%=GetLocalResourceObject("PleaseSelecttheFontFamily") %>");
                    $("[id$=ddlFontFamily]").focus();
                    return false;
                }
                if ($("[id$=ddlFontStyle]").val() == "0") {
                    alert("<%=GetLocalResourceObject("PleaseSelecttheFontStyle") %>");
                    $("[id$=ddlFontStyle]").focus();
                    return false;
                }
                if ($("[id$=ddlCockpitFontSize]").val() == "0") {
                    alert("<%=GetLocalResourceObject("PleaseSelecttheCockpitFontSize") %>");
                    $("[id$=ddlCockpitFontSize]").focus();
                    return false;
                }
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                checkBoxManage();
            });

            $("[id$=btnSaveColor]").click(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            })

            $("[id$=GridViewColorCodes]").on("click", "td", function () {
                $(this).closest('tr').find('input[type=hidden]').val("Updated");
            });

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
                $.unblockUI({});
                $(".pick-a-color").pickAColor();
                $("[id$=btnSaveColumnSetting]").click(function () {
                    $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                });

                $("[id$=chkShowSmileyImg]").click(function () {
                    if ($("[id$=chkShowSmileyImg]").is(':checked'))
                        $("#imgHide").show();
                    else {
                        $("#imgHide").hide();
                        $("[id$=ddlSmileImageSize]").val('0');
                    }
                });
                checkBoxManage();
                function checkBoxManage() {
                    if ($("[id$=chkShowSmileyImg]").is(':checked'))
                        $("#imgHide").show();
                    else {
                        $("#imgHide").hide();
                        $("[id$=ddlSmileImageSize]").val('0');
                    }
                }
                $("#btnMachineSortOrder").click(function () {
                    $('[id*=myModal]').modal('show');
                });

                $("[id$=GridViewColorCodes]").on("click", "td", function () {
                    $(this).closest('tr').find('input[type=hidden]').val("Updated");
                });

                function openModal() {

                    $('[id*=myModal]').modal('show');
                }

                $(document).ready(function () {

                    function alignModal() {
                        var modalDialog = $(this).find(".modal-dialog");
                        /* Applying the top margin on modal dialog to align it vertically center */
                        modalDialog.css("margin-top", Math.max(0, ($(window).height() - modalDialog.height()) / 2));
                    }
                    // Align modal when it is displayed
                    $(".modal").on("shown.bs.modal", alignModal);

                    // Align modal when user resize the window
                    $(window).on("resize", function () {
                        $(".modal:visible").each(alignModal);
                    });
                });
                //$("[id$=ddlGridPages]").click(function () {
                //    $.blockUI({ message: '<img src="./img/loadIcon/ajax-loader%20(6).gif" style="height: 160px"  />' });
                //});

            });
        });
        $(document).ready(function () {

            var gridHeader = $('#<%=gvMachineSortOrder.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=gvMachineSortOrder.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            //$('#GHead').css('position', 'absolute');
            //$('#GHead').css('top', $('#<%=gvMachineSortOrder.ClientID%>').offset().top);

        });

    </script>
</asp:Content>
