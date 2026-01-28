<%@ Page Title="Setting Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApplicationSettingPage.aspx.cs" Inherits="Web_TPMTrakDashboard.ApplicationSettingPage" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Scripts/ColorPickerJs/css/pick-a-color-1.2.2.min.css" rel="stylesheet" />
    <script src="Scripts/ColorPickerJs/dependencies/tinycolor-0.9.15.min.js"></script>
    <script src="Scripts/ColorPickerJs/js/pick-a-color-1.2.2.min.js"></script>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

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
            background-color: white;
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

        .td-Style {
            padding: 5px;
            color: white;
        }

        .content-style {
            text-align: center;
        }

        .panel-heading {
            background-color: lightblue;
            border: 1px solid white;
            display: inline-flex;
            border-radius: 3px;
            text-align: center;
            padding: 5px;
        }

        .Header-style {
            text-align: center;
            color: white;
            font-weight: bold;
            text-align: center;
        }

        #MainContent_gridViewResource tr th {
            color: white;
            background-color: #2E6886 !important;
        }

        .comanStyle {
            color: black;
        }
    </style>

    <div class="container-fluid">
        <table>
            <tr>
                <td class="td-Style">Settings</td>
                <td>
                    <asp:DropDownList ID="ddlSettingLst" CssClass="form-control" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlSettingLst_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Dashboard Column Display" Value="ColumnView" meta:resourcekey="ListItemResource12"></asp:ListItem>
                        <asp:ListItem Text="Live Analytics Cockpit Default View" Value="LiveAnalyticsCockpitDefaultView" meta:resourcekey="ListItemResource20"></asp:ListItem>
                        <asp:ListItem Text="Historical Analytics Cockpit Default View" Value="HistoricalAnalyticsCockpitDefaultView" meta:resourcekey="ListItemResource19"></asp:ListItem>
                        <asp:ListItem Text="OEE Dashboard Default View" Value="DashboardDefaultView" meta:resourcekey="ListItemResource18"></asp:ListItem>
                        <asp:ListItem Text="Cockpit Background Color" Value="CockpitBackColorSetting" meta:resourcekey="ListItemResource23"></asp:ListItem>
                        <asp:ListItem Text="Application Setting" Value="ApplicationSetting" meta:resourcekey="ListItemResource15"></asp:ListItem>
                        <asp:ListItem Text="Machine Efficiency Color" Value="MachineEfficiencyColor" meta:resourcekey="ListItemResource11"></asp:ListItem>
                        <asp:ListItem Text="Machine Status Color" Value="MachineStatusColor" meta:resourcekey="ListItemResource14"></asp:ListItem>
                        <asp:ListItem Text="Machine Custom Sort Order" Value="MachineCustomSortOrder" meta:resourcekey="ListItemResource17"></asp:ListItem>
                        <asp:ListItem Text="Efficiency Color" Value="EfficiencyColor" meta:resourcekey="ListItemResource24"></asp:ListItem>
                        <asp:ListItem Text="Company Logo Upload" Value="CompanyLogoUpload" meta:resourcekey="ListItemResource16"></asp:ListItem>
                        <asp:ListItem Text="Update Page Statistics (Multilingual)" Value="UpdatePageStatics" meta:resourcekey="ListItemResource13"></asp:ListItem>
                        <%-- <asp:ListItem Text="Program Transfer" Value="ProgramTransferSetting" meta:resourcekey="ListItemResource21"></asp:ListItem>--%>
                        <asp:ListItem Text="Eshopx Setting" Value="EshopxSetting" meta:resourcekey="ListItemResource22"></asp:ListItem>
                        <asp:ListItem Text="Data Collection" Value="DataCollection" meta:resourcekey="ListItemResource25"></asp:ListItem>
                        <asp:ListItem Text="Modified Data Settings" Value="ModifiedDataSettings"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <%-- <td class="td-Style">
                    <asp:Button data-toggle="tooltip" ID="BtnView" runat="server" Text="<%$Resources:CommanResource, View %>" CssClass="btn btn-primary toolTip" OnClick="BtnView_Click" />
                </td>--%>
                <td class="td-Style">
                    <asp:Button data-toggle="tooltip" ID="btnSave" runat="server" Text="<%$Resources:CommanResource, Save %>" CssClass="btn btn-primary toolTip" OnClick="btnSave_Click" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="update" runat="server">
            <ContentTemplate>
                <div id="divMachineEfficiencyColor" runat="server" style="margin-top: 20px;" visible="false">
                    <table class="table table-bordered" style="width: 50%">
                        <tr style="background: #2E6886; height: 40px;">
                            <td class="Header-style">Status</td>
                            <td class="Header-style">Color Code</td>
                        </tr>
                        <tr>
                            <td class="content-style"><%=GetLocalResourceObject("Good") %></td>
                            <td>
                                <asp:TextBox ID="txtGood" data-toggle="tooltip" name="border-color" runat="server" class="pick-a-color form-control" meta:resourcekey="txtGoodResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="content-style"><%=GetLocalResourceObject("Bad") %></td>
                            <td>
                                <asp:TextBox ID="txtBad" data-toggle="tooltip" name="border-color" runat="server" class="pick-a-color form-control" meta:resourcekey="txtBadResource1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="content-style"><%=GetLocalResourceObject("Moderate") %></td>
                            <td>
                                <asp:TextBox ID="txtModerate" data-toggle="tooltip" name="border-color" runat="server" class="pick-a-color form-control" meta:resourcekey="txtModerateResource1"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divColumnViewSetting" runat="server" style="margin-top: 20px;" visible="false">
                    <div class="panel-heading text-center" style="width: 60%; justify-content: center; color: black;">
                        <div style="margin-right: 10px;">
                            <asp:DropDownList ID="ddlGridPages" runat="server" CssClass="form-control input-md" Width="400px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlGridPages_SelectedIndexChanged" Style="display: inline;" meta:resourcekey="ddlGridPagesResource1">
                                <asp:ListItem Value="WebTPMTrak" meta:resourcekey="ListItemResource3">OEE Dashboard</asp:ListItem>
                                <asp:ListItem Value="CockpitGridColumn" meta:resourcekey="ListItemResource4">Live Analytics - Iconic View</asp:ListItem>
                                <asp:ListItem Value="WebTPMTrakTableView" meta:resourcekey="ListItemResource5">Live Analytics - Table View</asp:ListItem>
                                <asp:ListItem Value="WebCockpitGridColumnAggregate" meta:resourcekey="ListItemResource8">Historical Analytics(Iconic And Table View)</asp:ListItem>
                                <asp:ListItem Value="WebTPMTrakVDGProduction" meta:resourcekey="ListItemResource6">Cycle Analytics - Production Data</asp:ListItem>
                                <asp:ListItem Value="WebTPMTrakVDGDownTime">Cycle Analytics - Down Data</asp:ListItem>
                                <%--<asp:ListItem Value="TPMTrakWebEnergyViewColumnSettings">Energy Dashboard</asp:ListItem>
                            <asp:ListItem Value="WebIonicViewAndon" meta:resourcekey="ListItemResource9">Andon Iconic View</asp:ListItem>
                            <asp:ListItem Value="WebTableViewAndon" meta:resourcekey="ListItemResource10">Andon Table View</asp:ListItem>
                            <asp:ListItem Value="ComponentInformation" meta:resourcekey="ListItemResource24">Component Information</asp:ListItem>--%>
                                <%-- <asp:ListItem Value="EnergyLiveData" meta:resourcekey="ListItemResource25">Energy Live Data</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                        <div>
                            <%=GetLocalResourceObject("Language") %>
                            <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="form-control input-md" Width="250px" Style="display: inline;" AutoPostBack="true" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged">
                                <asp:ListItem Value="en">English (United Kingdom)</asp:ListItem>
                                <asp:ListItem Value="zh">中文（简体，PRC）</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="width: 60%; overflow-y: auto;" class="gvColumnViewDiv">
                        <asp:GridView ID="grdViewAndonView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered headerFixer" meta:resourcekey="grdViewAndonViewResource1" ClientIDMode="Static">
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
                            <EmptyDataTemplate>
                                <div style="width: 100%; text-align: center; color: red;">No Data Available.</div>
                            </EmptyDataTemplate>
                            <HeaderStyle BackColor="#2E6886" Font-Bold="True" ForeColor="White" Height="40" />
                        </asp:GridView>
                    </div>
                </div>
                <div id="divMachineStatusColor" runat="server" style="margin-top: 20px;">
                    <div style="width: 50%;" class="gvColumnViewDiv">
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
                    </div>
                </div>
                <div id="divApplicationSetting" runat="server" style="margin-top: 20px;">
                    <div style="overflow: auto; width: 50%" class="gvColumnViewDiv">
                        <table class="table table-bordered">
                            <tr style="background: #2E6886; height: 40px;">
                                <td class="Header-style"><%=GetLocalResourceObject("Parameter") %></td>
                                <td class="Header-style"><%=GetLocalResourceObject("Value") %></td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="control-label"><%=GetLocalResourceObject("Downtime/Stoppage") %></label></td>
                                <td>
                                    <asp:DropDownList ID="ddlDowntime" runat="server" CssClass="form-control input-md" meta:resourcekey="ddlDowntimeResource1">
                                        <asp:ListItem Value="hh" meta:resourcekey="ListItemResource1">hh</asp:ListItem>
                                        <asp:ListItem Value="mm" meta:resourcekey="ListItemResource2">min.</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
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
                                <td>
                                    <label class="control-label">Exclude TPM Trak Standard DownCodes</label>
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="checkboxtoHide16losses" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="divCompanyLogo" runat="server" style="margin-top: 20px;">
                    <table class="table table-bordered" style="width: 50%">
                        <tr style="background: #2E6886; height: 40px;">
                            <td colspan="2" class="Header-style">File Upload</td>
                        </tr>
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
                <div id="divMachineSortOrder" runat="server" style="margin-top: 20px;">
                    <div style="overflow: auto; width: 50%" class="gvColumnViewDiv">
                        <asp:GridView runat="server" ID="gvMachineSortOrder" AutoGenerateColumns="false" CssClass="table table-bordered" ShowHeaderWhenEmpty="true" Style="color: black;" ShowHeader="true  ">
                            <Columns>
                                <asp:TemplateField HeaderText="Machine Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMachine" runat="server" Text='<%# Eval("machineid") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="text-center" Height="40" ForeColor="White" BackColor="#2E6886" />

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sort Order">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtSortOrder" runat="server" CssClass="form-control" TextMode="Number" min="0" step="1" Text='<%# Bind("SortOrder") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="text-center" ForeColor="White" BackColor="#2E6886" />
                                    <ItemStyle CssClass="text-center" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div style="width: 100%; text-align: center; color: red;">No Machines Available.</div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
                <div id="divDashboardDefaultView" runat="server" style="margin-top: 20px;">
                    <table class="table table-bordered" style="background-color: white; width: 50%">
                        <tr style="background: #2E6886; height: 40px;">
                            <td class="Header-style"><%=GetLocalResourceObject("Parameter") %></td>
                            <td class="Header-style"><%=GetLocalResourceObject("Value") %></td>
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
                    </table>
                </div>
                <div id="divHistAnalyticsCockpitDefView" runat="server" style="margin-top: 20px;">
                    <table class="tblHAView table-bordered" style="width: 50%">
                        <tr style="background: #2E6886; height: 40px;">
                            <th class="Header-style"><%=GetLocalResourceObject("Parameter") %></th>
                            <th class="Header-style"><%=GetLocalResourceObject("Value") %></th>
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
                    </table>
                </div>
                <div id="divLiveAnalyticsCockpitDefView" runat="server" style="margin-top: 20px;">
                    <table class="tblHAView table-bordered" style="width: 50%">
                        <tr style="background: #2E6886; height: 40px;">
                            <th class="Header-style"><%=GetLocalResourceObject("Parameter") %></th>
                            <th class="Header-style"><%=GetLocalResourceObject("Value") %></th>
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

                    </table>
                </div>

                <div id="divProgramTransferSetting" runat="server" style="margin-top: 20px;">
                    <table class="table table-bordered" style="width: 50%">
                        <tr style="background: #2E6886; height: 40px;">
                            <td class="Header-style"><%=GetLocalResourceObject("Parameter") %></td>
                            <td class="Header-style"><%=GetLocalResourceObject("Value") %></td>
                        </tr>
                        <tr>
                            <td><%=GetLocalResourceObject("ProgramFolderPath") %></td>
                            <td>
                                <asp:TextBox ID="txtProgramFolderPath" CssClass="form-control" runat="server" Text='<%# Bind("Parameter") %>' meta:resourcekey="txtProgramFolderPathResource1" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td><%=GetLocalResourceObject("ProgramFileExtension") %></td>
                            <td>
                                <asp:DropDownList ID="ddlProgramFileExtension" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divEshopxSetting" runat="server" style="margin-top: 20px;">
                    <table class="table table-bordered" style="width: 50%">
                        <tr style="background: #2E6886; height: 40px;">
                            <td class="Header-style"><%=GetLocalResourceObject("Parameter") %></td>
                            <td class="Header-style"><%=GetLocalResourceObject("Value") %></td>
                        </tr>
                        <tr>
                            <td>
                                <label class="settingLbl" id="lblRootPath" runat="server">Root Path</label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPathVal" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divCockpitBackColor" runat="server" style="margin-top: 20px;">
                    <table class="table table-bordered" style="width: 50%">
                        <tr style="background: #2E6886; height: 40px;">
                            <td class="Header-style"><%=GetLocalResourceObject("Parameter") %></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButton CssClass="form-control" runat="server" Text="AE" ID="chkAE" GroupName="backColorParam" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButton CssClass="form-control" runat="server" Text="PE" ID="chkPE" GroupName="backColorParam" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButton CssClass="form-control" runat="server" Text="OEE" ID="chkOEE" GroupName="backColorParam" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButton CssClass="form-control" runat="server" Text="OperatorPE" ID="ChkOperatorPE" GroupName="backColorParam" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divDataCollection" runat="server" style="margin-top: 20px;">
                    <table class="table table-bordered" style="width: 50%">
                        <tr style="background: #2E6886; height: 40px;">
                            <td class="Header-style"><%=GetLocalResourceObject("Parameter") %></td>
                            <td class="Header-style"><%=GetLocalResourceObject("Value") %></td>
                        </tr>
                        <tr>
                            <td>
                                <label class="settingLbl" id="lblSerialNo" runat="server">Serial No</label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSerialNo" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="settingLbl" id="lblHeatCode" runat="server">Heat Code</label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlHeatCode" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="settingLbl" id="lblWorkOrder" runat="server">Work Order</label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlWorkOrder" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divModifiedDataSettings" runat="server" style="margin-top: 20px;" visible="false">
                    <table class="table table-bordered" style="width: 50%">
                        <tr style="background: #2E6886; height: 40px;">
                            <td class="Header-style"><%=GetLocalResourceObject("Parameter") %></td>
                            <td class="Header-style"><%=GetLocalResourceObject("Value") %></td>
                        </tr>
                        <tr>
                            <td>
                                <label class="settingLbl" id="lblChartType" runat="server">Historical Dashboard Chart Type</label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlChartType" runat="server" CssClass="form-control" ClientIDMode="Static" OnSelectedIndexChanged="ddlChartType_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="Separate " Value="Separate"></asp:ListItem>
                                    <asp:ListItem Text="Combined" Value="Combined"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--<label class="settingLbl" id="Label1" runat="server">Historical Dashboard Charts</label>--%>
                                <label class="settingLbl" id="Label1" runat="server">KPI's To Combine</label>
                            </td>
                            <td>
                                <asp:ListBox runat="server" ID="lbCharts" CssClass="form-control" SelectionMode="Multiple" ClientIDMode="Static">
                                    <asp:ListItem Text="AE" Value="AE"></asp:ListItem>
                                    <asp:ListItem Text="PE" Value="PE"></asp:ListItem>
                                    <asp:ListItem Text="QE" Value="QE"></asp:ListItem>
                                    <asp:ListItem Text="OEE" Value="OEE"></asp:ListItem>
                                </asp:ListBox>
                            </td>
                        </tr>
                        <%--<tr>
                            <td>
                                <label class="settingLbl" id="lblisModifieData" runat="server">is Modified?</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlisModified" CssClass="form-control"></asp:DropDownList>
                            </td>
                        </tr>--%>
                        <tr>
                            <td>
                                <label class="settingLbl" id="lblProductionLog" runat="server">Production Audit Log</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlProductionLog" CssClass="form-control"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="settingLbl" id="lblDownLog" runat="server">Down Audit Log</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlDownLog" CssClass="form-control"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="settingLbl" id="lblModifieDataBackColor" runat="server">Modified Data Back Color</label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtColorPicker" data-toggle="tooltip" name="border-color" runat="server" class="pick-a-color form-control" />
                            </td>
                        </tr>
                       <%-- <tr>
                            <td>
                                <label class="settingLbl" id="lblIsModified" runat="server">Is Modified</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlIsModified" CssClass="form-control"></asp:DropDownList>
                            </td>
                        </tr>--%>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="btnUploadeHide" runat="server" OnClick="btnUploadeHide_Click" Style="display: none;" meta:resourcekey="btnUploadeHideResource1" />
        <div id="MultilingualSetting" runat="server" style="margin-top: 20px;">
            <div class="row">
                <table class="table" style="width: 60%; background-color: lightblue; border-radius: 3px; text-align: center; height: 50px;">
                    <tr>
                        <td style="vertical-align: middle; color: black"><b><%=GetLocalResourceObject("Language") %></b></td>
                        <td>
                            <asp:DropDownList ID="ddlmultlang" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlLanguage1_SelectedIndexChanged" meta:resourcekey="ddlLanguageResource1">
                                <asp:ListItem Value="en" meta:resourcekey="ListItemResource26">English (United Kingdom)</asp:ListItem>
                                <asp:ListItem Value="zh" meta:resourcekey="ListItemResource27">中文（简体，PRC）</asp:ListItem>
                            </asp:DropDownList></td>
                        <td style="vertical-align: middle; color: black"><b><%=GetLocalResourceObject("PageName") %></b></td>
                        <td style="margin-right: 20px;">
                            <asp:DropDownList ID="ddlPageName" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlPageName_SelectedIndexChanged" meta:resourcekey="ddlPageNameResource1"></asp:DropDownList></td>
                    </tr>
                </table>
            </div>
            <div class="row">
                <div style="overflow-x: hidden; overflow-y: auto; height: 700px; width: 60%;" class="gvColumnViewDiv">
                    <asp:GridView ID="gridViewResource" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" Width="100%" meta:resourcekey="gridViewResourceResource1">
                        <Columns>
                            <asp:TemplateField HeaderText="Key" meta:resourcekey="TemplateFieldResource4">
                                <ItemTemplate>
                                    <asp:Label ID="lblKey" runat="server" CssClass="comanStyle" Text='<%# Bind("Key") %>' meta:resourcekey="lblKeyResource1"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Height="40" CssClass="text-center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Value" meta:resourcekey="TemplateFieldResource5">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtValue" runat="server" Text='<%# Eval("Value") %>' Width="100%" CssClass="form-control txtupdate" meta:resourcekey="txtValueResource1"></asp:TextBox>
                                    <asp:HiddenField ID="hdfCondition" runat="server" ClientIDMode="Static" />
                                </ItemTemplate>
                                <HeaderStyle Height="40" CssClass="text-center" />
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <script type='text/javascript'>

        $(document).ready(function () {
            var winHeight = $(window).height();
            console.log(winHeight);
            $('[id$=lbCharts]').multiselect({
                includeSelectAllOption: true
            });
            $(".gvColumnViewDiv").height(winHeight - 200);

            $(".pick-a-color").pickAColor();

            $("#btnUpload").click(function () {
                alert();
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
            $("[id$=btnSave]").click(function () {
                if ($("#ddlSettingLst").val() == "ApplicationSetting") {
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
                }
                else
                    $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            });

            $("[id$=GridViewColorCodes]").on("click", "td", function () {
                $(this).closest('tr').find('input[type=hidden]').val("Updated");
            });

            var gridHeader = $('#<%=gvMachineSortOrder.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=gvMachineSortOrder.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);

        });

        $("#MainContent_gridViewResource").on("change", ".txtupdate", function () {
            alert();
            $("[id$=hdfCondition]").val("update");
            $(this).closest('tr').find('input[type = hidden]').val("update");
        });

        function OpenEfficiencyColorView(URL) {
            window.open(URL, "EfficiencyColorSetting");
        }

        function messageNotOk() {
            Command: toastr["error"]("Data Not Saved!")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "2000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"

            }
        }
        function messageOk() {
            Command: toastr["success"]("Saved Successfully")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }
        function ResourceMsgok() {
            Command: toastr["success"]("Resource File Updated...")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }


        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI({});
            var winHeight = $(window).height();
            console.log(winHeight);
            $('[id$=lbCharts]').multiselect({
                includeSelectAllOption: true
            });
            $(".gvColumnViewDiv").height(winHeight - 200);

            $(".pick-a-color").pickAColor();

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

            $("[id$=GridViewColorCodes]").on("click", "td", function () {
                $(this).closest('tr').find('input[type=hidden]').val("Updated");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
