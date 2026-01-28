<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserAccess.aspx.cs" Inherits="Web_TPMTrakDashboard.UserAccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #chklabel {
            margin: 10px;
        }

        .scrollbox:hover,
        .scrollbox:focus {
            visibility: visible;
        }

        label {
            margin-left: 7px;
        }

        body ::-webkit-scrollbar-track {
            background: rgb(128 166 207) !important;
             border-radius: 5px !important;
        }

        body ::-webkit-scrollbar-thumb {
            cursor: pointer;
            border-radius: 5px;
            background: rgb(55 78 99) !important;
            -webkit-transition: color 0.2s ease;
            transition: color 0.2s ease;
        }
    </style>
    <link href="Scripts/Sematic/semantic.css" rel="stylesheet" />
    <script src="Scripts/Sematic/semantic.js"></script>
    <link href="Scripts/Sematic/semantic.min.css" rel="stylesheet" />
    <script src="Scripts/Sematic/semantic.min.js"></script>
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
            <div>
                <%--      <asp:UpdatePanel runat="server">
            <ContentTemplate>--%>
                <div class="ui segment">
                    <table>
                        <tr>
                            <td style="width: 220px;">Plant-ID
                            </td>

                            <td style="width: 220px;">User ID
                            </td>
                            <td style="width: 220px;">User Name
                            </td>
                            <td style="width: 120px">Password
                            </td>

                            <td style="width: 120px;" rowspan="2">
                                <asp:CheckBox runat="server" ID="chkSelectAll" OnCheckedChanged="chkSelectAll_CheckedChanged" AutoPostBack="true" />
                                <span>Select All</span>
                            </td>
                            <td rowspan="2">
                                <asp:Button runat="server" ID="btnSave" CssClass="ui violet button" Text="Save" OnClick="btnSave_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: auto; display: inline-block">
                                <table>
                                    <tr style="margin: 3px">
                                        <td>
                                            <asp:DropDownList runat="server" ID="cmbPlantID" CssClass="form-control" Style="width: auto" />
                                        </td>
                                        <td style="white-space: nowrap">
                                            <asp:CheckBox runat="server" ID="chkPlantAll" Text="Plant ID" Style="margin-top: 10px; padding: 10px" CssClass="chklabel" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 220px;">
                                <table>
                                    <tr style="margin: 3px">
                                        <td>
                                            <asp:DropDownList runat="server" ID="cmbUserID" OnSelectedIndexChanged="cmbUserID_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" Width="100" />

                                        </td>

                                        <td>
                                            <asp:CheckBox runat="server" ID="chkAdmin" Text="Admin" Style="margin-top: 10px; padding: 10px" CssClass="chklabel" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblUserName"></asp:Label>
                            </td>
                            <td style="width: 100px;">
                                <asp:TextBox ID="txtPassword" Width="100" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            </td>

                        </tr>
                    </table>
                </div>
                <div class="ui segment">
                    <table>
                        <tr>
                            <td style="width: 220px;">Default Launch Screen
                            </td>
                        </tr>
                        <tr>
                            <td style="width: auto; display: inline-block">
                                <table>
                                    <tr style="margin: 3px">
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlPages" CssClass="form-control" Style="width: auto">
                                                <asp:ListItem Value="Dashboard.aspx">Dashboard</asp:ListItem>
                                                <asp:ListItem Value="IonicView.aspx">Live Iconic View</asp:ListItem>
                                                <asp:ListItem Value="tableView.aspx">Live Table View</asp:ListItem>
                                                <asp:ListItem Value="IonicViewAggregated.aspx">Agg. Iconic View</asp:ListItem>
                                                <asp:ListItem Value="TableViewAggregate.aspx">Agg. Table View</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td rowspan="2">
                                            <asp:Button runat="server" ID="btnPageSave" CssClass="ui violet button" Text="Save" OnClick="btnPageSave_Click" OnClientClick="return ViewClick();" />
                                        </td>
                                    </tr>
                                </table>
                            </td>

                        </tr>
                    </table>
                </div>
                <div class="row">
                    <div style="margin: 10px">
                        <asp:ListView runat="server" ID="lstUserAccess" ItemPlaceholderID="PlaceHolder">
                            <LayoutTemplate>
                                <asp:PlaceHolder runat="server" ID="PlaceHolder" />
                            </LayoutTemplate>
                            <ItemTemplate>
                                <%--style="min-width: 320px; display: inline-block; height: 400px; overflow-y: auto; overflow-x: hidden;"--%>
                                <div runat="server" id="DomainId" visible='<%# Bind("Visibility") %>' class="myItem abc" style="min-width: 320px; display: inline-block; vertical-align: top">
                                    <%--  <div class="border" style="padding-right: 30px; padding-bottom: 20px; border-radius: 55px; background-color: black; margin: 20px;width:310px">--%>
                                    <div class="border" style="padding-right: 10px; padding-bottom: 20px; border-radius: 10px; border: 1px solid white; padding-left: 10px; width: 310px">
                                        <div>
                                            <table style='width: 100%;' class="outercockpit">
                                                <tr>
                                                    <td style="text-align: center; vertical-align: middle; padding: 10px">
                                                        <asp:CheckBox runat="server" Checked='<%# Bind ("MenuCheck")%>' Text='<%# Bind("DomainName") %>' ID="chkmenu" OnCheckedChanged="chkmenu_CheckedChanged" AutoPostBack="true" CssClass="headercheck headerFixer" ForeColor="White"></asp:CheckBox>
                                                        <%--<asp:Label ID="lblDomainName" Text='<%# Bind("DomainName") %>' runat="server" ForeColor="White"/>--%>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div style="overflow-y: auto; height: 50vh;">
                                                <table class="table table-bordered cssNonAdmin cockpit" style='background-color: white; height: 200px; text-align: left'>
                                                    <asp:ListView ID="lstview1" runat="server" DataSource='<%# Eval("Values") %>' ItemPlaceholderID="addressPlaceHolder">
                                                        <LayoutTemplate>
                                                            <asp:PlaceHolder runat="server" ID="addressPlaceHolder" />
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="text-align: left; min-width: 100px; background-color: white; text-align: left">
                                                                    <asp:CheckBox runat="server" ID="chkTable" Text='<%# Bind("TableValueName") %>' Checked='<%# Bind("TableValueChecked") %>' OnCheckedChanged="chkTable_CheckedChanged" AutoPostBack="false" />
                                                                    <asp:HiddenField Value='<%# Bind("MenuName") %>' runat="server" ID="hidmenuname" />
                                                                    <asp:HiddenField Value='<%# Bind("MenuCodeName") %>' runat="server" ID="HiddenField2" />
                                                                    <asp:HiddenField Value='<%# Bind("TextValueCode") %>' runat="server" ID="HiddenField1" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:ListView>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
                <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script>
        function ViewClick() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                $.unblockUI({});
            });
        });
        $(document).on("click", ".headercheck", function () {
            debugger;

        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
