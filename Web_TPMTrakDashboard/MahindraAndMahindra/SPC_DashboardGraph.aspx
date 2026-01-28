<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SPC_DashboardGraph.aspx.cs" Inherits="Web_TPMTrakDashboard.MahindraAndMahindra.SPC_DashboardGraph" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
               <asp:Timer runat="server" ID="UpdateTimer" OnTick="UpdateTimer_Tick" Enabled="false" />
              <div class="container-fluid row">
            <div style="display: flex">
                <table class="table table-bordered" style="display: inline-block; text-align: center; vertical-align: initial; display: inline-grid;">
                    <tr>
                        <td>
                            <asp:DropDownList runat ="server" ID="cmbtype">
                                <asp:ListItem Text="Dashboard Type" Value="Dashboard" />
                                <asp:ListItem Text="Graph Type" Value="Graph" />
                            </asp:DropDownList>
                        </td>
                        <td id="tdDashboard">
                            <table>
                                <tr>
                                    <td>

                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="vertical-align: inherit; color: white;">
                            <b><span style="width: 100px; margin: 10px;">parameters</span></b>
                        </td>
                        <td style="vertical-align: inherit">
                            <asp:listbox runat="server" id="lstparameters" width="200" style="margin: 10px;" />
                        </td>
                        <td style="vertical-align: inherit; font: bold 20px">
                            <asp:button runat="server" id="btngraphview" cssclass="btn btn-success" width="150" font-bold="true" font-size="14" onclick="btngraphview_click" height="45" text="graph view" clientidmode="static" style="margin: 10px;" />
                        </td>
                        <%--<td style="vertical-align: inherit; color: white;">
                            <asp:CheckBox runat="server" ID="GraphView" OnCheckedChanged="GraphView_CheckedChanged" Text="Graph View" />
                        </td>--%>
                        <td style="vertical-align: inherit; color: white;">
                            <asp:CheckBox runat="server" ID="AutoRefresh" OnCheckedChanged="AutoRefresh_CheckedChanged" Text="Auto Refresh" />
                        </td>
                    </tr>
                   

                </table>
            </div>
                    <div class="row clearfix" id="Charts" runat="server" visible="false">
            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6">
                <div class=" card">
                    <div class="card-head">
                        <header id="FirstChartHeader"><b></b></header>
                        <div class="tools Year">
                            <a><span class="icon_refresh" onclick="RefreshData('FirstChart')"></span></a>
                            <a><span class="arrow_carrot-down Year" onclick="HideShowMenu('FirstChart')" style="font-size: 20px;"></span></a>
                        </div>
                    </div>
                    <div id="divFirstChart" style="margin: 10px">
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6">
                <div class=" card">
                    <div class="card-head">
                        <header id="SecondChartHeader"><b></b></header>
                        <div class="tools Year">
                            <a><span class="icon_refresh" onclick="RefreshData('SecondChart')"></span></a>
                            <a><span class="arrow_carrot-down Year" onclick="HideShowMenu('SecondChart')" style="font-size: 20px;"></span></a>
                        </div>
                    </div>
                    <div id="divSecondChart" style="margin: 10px">
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6">
                <div class=" card">
                    <div class="card-head">
                        <header id="ThirdChartHeader"><b></b></header>
                        <div class="tools Year">
                            <a><span class="icon_refresh" onclick="RefreshData('ThirdChart')"></span></a>
                            <a><span class="arrow_carrot-down Year" onclick="HideShowMenu('ThirdChart')" style="font-size: 20px;"></span></a>
                        </div>
                    </div>
                    <div id="divThirdChart" style="margin: 10px">
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 col-6">
                <div class=" card">
                    <div class="card-head">
                        <header id="FourthChartHeader"><b></b></header>
                        <div class="tools Year">
                            <a><span class="icon_refresh" onclick="RefreshData('FourthChart')"></span></a>
                            <a><span class="arrow_carrot-down Year" onclick="HideShowMenu('FourthChart')" style="font-size: 20px;"></span></a>
                        </div>
                    </div>
                    <div id="divFourthChart" style="margin: 10px">
                    </div>
                </div>
            </div>
        </div>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
