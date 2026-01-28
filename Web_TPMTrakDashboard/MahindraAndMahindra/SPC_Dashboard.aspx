<%@ Page Title="Process Parameter Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SPC_Dashboard.aspx.cs" Inherits="Web_TPMTrakDashboard.MahindraAndMahindra.SPC_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
  <%--  <script src="https://code.highcharts.com/stock/highstock.js"></script>
    <script src="https://code.highcharts.com/stock/modules/data.js"></script>
    <script src="https://code.highcharts.com/stock/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/stock/modules/export-data.js"></script>--%>
     <script src="../MyCssAndJS/Highchart10.3.2/highstock.js"></script>
    <link href="../css/elegant-icons-style.css" rel="stylesheet" />

    <style>
        .multiselect-container {
            height: 300px;
            overflow-x: auto;
        }

        .multiselect-selected-text {
            padding-right: 181px;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
        }

        .card {
            background-color: #fff;
            border-radius: 10px;
            position: relative;
            margin-bottom: 30px;
            border: 1px solid #deebfd;
            position: relative;
            display: flex;
            flex-direction: column;
            min-width: 0;
            word-wrap: break-word;
            background-color: #fff;
            background-clip: border-box;
            border: 1px solid rgba(0,0,0,.125);
            border-radius: .25rem;
            border-radius: 24px;
            margin: 10px;
        }

        .card-head {
            border-radius: 2px 2px 0 0;
            border-bottom: 1px dotted rgba(0, 0, 0, 0.2);
            padding: 2px;
            text-transform: uppercase;
            color: #3a405b;
            font-size: 14px;
            font-weight: 600;
            line-height: 40px;
            min-height: 40px;
        }

            .card-head .tools {
                padding-right: 16px;
                float: right;
                margin-top: 7px;
                margin-bottom: 7px;
                margin-left: 24px;
                line-height: normal;
                vertical-align: middle;
            }

                .card-head .tools .btn-color {
                    color: #97a0b3;
                    margin-right: 3px;
                    font-size: 12px;
                }

        .row {
            --bs-gutter-x: 1.5rem;
            --bs-gutter-y: 0;
            display: flex;
            flex-wrap: wrap;
            margin-top: calc(var(--bs-gutter-y) * -1);
            margin-right: calc(var(--bs-gutter-x)/ -2);
            margin-left: calc(var(--bs-gutter-x)/ -2);
        }

        .card-head header {
            display: inline-block;
            padding: 11px 20px;
            vertical-align: middle;
            line-height: 17px;
            font-size: 20px;
        }

        #tdLstParameters .multiselect.btn {
            overflow: hidden;
            width: 200px;
        }
    </style>
    <div>
        <%--<asp:ScriptManager runat="server" />--%>


        <asp:Timer runat="server" ID="UpdateTimer" OnTick="UpdateTimer_Tick" Enabled="false" />
        <div class="container-fluid row">
            <div style="display: flex">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <table class="table table-bordered" style="display: inline-block; text-align: center; vertical-align: initial; display: inline-grid;">
                            <tr>
                                <td style="vertical-align: inherit; color: white;">
                                    <b><span style="vertical-align: central">Machine-ID</span></b>
                                </td>
                                <td style="vertical-align: inherit">
                                    <asp:DropDownList runat="server" ID="ddlMachineID" CssClass="form-control" ClientIDMode="Static"  />
                                </td>
                                <td id="tdDashboardViewBtn" runat="server" style="vertical-align: inherit" class="DashboardView">
                                    <asp:Button runat="server" ID="btnView" CssClass="btn btn-danger" Width="120" Font-Bold="true" Font-Size="14" Height="45" Text="View" ClientIDMode="Static" Style="margin: 10px;" OnClick="btnView_Click" />
                                </td>
                                <td id="tdAutoRefreshBtn" runat="server" style="vertical-align: inherit; color: white;" class="DashboardView">
                                    <asp:CheckBox runat="server" ID="AutoRefresh" OnCheckedChanged="AutoRefresh_CheckedChanged" Text="Auto Refresh" AutoPostBack="true" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="container-fluid" id="SPCDashboard" runat="server" style="display: inline-block">
            <asp:UpdatePanel ID="updatePanelProcessParam" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div id="divProcessParams">
                            <asp:ListView runat="server" ItemPlaceholderID="placeHolderProcessParams" ID="listViewProcessParams">
                                <LayoutTemplate>
                                    <asp:PlaceHolder runat="server" ID="placeHolderProcessParams" />
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <div class="myItem" style="margin: 15px; min-width: 340px; max-width: 500px; min-height: 210px; display: inline-block; background-color: <%# Eval("BackgroundColor")%>; border-radius: 8px;">

                                        <table style='width: 100%; border: none; border-collapse: collapse; height: 200px' class="outerbox">
                                            <tr style="background-color: #e5e1e15e;">
                                                <td style="text-align: center; vertical-align: middle; font-weight: bold; padding-bottom: 5px; border: none; height: 50px; border-radius: 5px 5px 0px 0px">
                                                    <asp:Label runat="server" ID="lblParameter" ClientIDMode="Static" Style="font-size: 22px; overflow: hidden; width: 327px; min-width: 327px; text-overflow: ellipsis; white-space: nowrap" Font-Size="Medium" Font-Names="Segoe UI" ForeColor='<%# Eval("HeaderForeColor") %>'> <%# Eval("DisplayText")%></asp:Label>
                                                    <%--display: inline-block;--%> &nbsp;&nbsp;
                                                    <i class="glyphicon glyphicon-stats" style="font-size: 19px;cursor:pointer;" onclick="return goToGraphViewChart(this);"></i>
                                                </td>
                                            </tr>
                                            <tr style="border: none;">
                                                <%--background-color: <%# Eval("BackgroundColor")%>;--%>
                                                <td style="text-align: center; vertical-align: middle; font-weight: bold; padding-bottom: 5px; border: none;">
                                                    <asp:TextBox ID="txtParamval" Height="40" runat="server" Enabled="false" Style="text-align: center; margin-top: 6px; width: 153px; font-size: 26px; height: 50px;" Text='<%# Bind("ParameterValue") %>' BackColor="White" ForeColor="#2C639B" Font-Size="18px" Font-Names="Segoe UI" Font-Bold="true" ToolTip='<%# Bind("ParameterValue") %>' />
                                                </td>
                                            </tr>
                                            <tr style="border: none;">
                                                <%--background-color: <%# Eval("BackgroundColor")%>;--%>
                                                <td style="text-align: center; vertical-align: middle; font-weight: bold; padding-bottom: 5px; border: none;">
                                                    <asp:Label runat="server" Height="40" ID="lblUom" Font-Size="18" ForeColor="Black" Font-Names="Segoe UI" Font-Bold="true"><%# Eval("Unit")%></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="border: none; display: <%# Eval("Visibility")%>">
                                                <%--background-color: <%# Eval("BackgroundColor")%>;--%>
                                                <td style="text-align: center; vertical-align: middle; font-weight: bold; padding-left: 20px; padding-bottom: 5px; border: none; font-size: 20px;">
                                                    <div class="row">
                                                        <asp:Label Height="40" runat="server" ForeColor="Black" Font-Names="Segoe UI" Font-Bold="true" Style="padding-top: 5px;">Low</asp:Label>
                                                        <asp:TextBox Height="40" ID="txtLow" runat="server" Enabled="false" Style="text-align: center; width: 100px; height: 50px; margin-left: 10px; font-size: 20px;" Text='<%# Bind("MinValue") %>' BackColor="White" ForeColor="#2C639B" Font-Size="18px" Font-Names="Segoe UI" Font-Bold="true" />
                                                        <asp:Label Height="40" runat="server" ForeColor="Black" Font-Names="Segoe UI" Font-Bold="true" Style="margin-left: 10px; padding-top: 5px; height: 50px; font-size: 20px;">Hi</asp:Label>
                                                        <asp:TextBox Height="40" ID="txtHi" runat="server" Enabled="false" Style="text-align: center; width: 100px; height: 50px; margin-left: 10px; font-size: 20px;" Text='<%# Bind("MaxValue") %>' BackColor="White" ForeColor="#2C639B" Font-Size="18px" Font-Names="Segoe UI" Font-Bold="true" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="UpdateTimer" EventName="Tick" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <script>

        $(document).ready(function () {

        });
        function goToGraphViewChart(ctrl) {
            let machineID = $('#ddlMachineID').val();
            let paramID = $(ctrl).closest('tr').find('#lblParameter').text();
            window.open("SPC_PPGraphView.aspx?MachineID=" + machineID + "&ParameterID=" + paramID, "PP Graph View");
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {


        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
