<%@ Page Title="Daily Checklist Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyChecklistReport.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.DailyChecklistReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
          <style type="text/css">
        .header-center {
            text-align: center;
        }

        table tr th {
            text-align: center !important;
        }

        .headerFixerhere tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #5391CA;
            color: white;
        }

        .DataOperations {
            bottom: 0;
            right: 0;
            float: right;
            margin-right: 5px;
            min-height: 40px;
            position: fixed;
        }

        .WeekScheduler {
            vertical-align: middle;
        }

        .InspectionStatus {
            vertical-align: middle;
        }

        .Row {
            display: table;
            border-spacing: 5pt;
            width: 100%;
        }

        .Col {
            display: table-cell;
            height: 50px;
            width: 100%;
            border: 1pt solid black;
            background-color: #DBDBDB;
        }

        .MiddleLeft {
            text-align: left;
            align-items: normal;
            vertical-align: middle;
        }

        .form-control {
            width: 98%;
        }
    </style>

    <div class="container-fluid" style="margin-left: 5px;">
        <asp:UpdatePanel ID="UpdatePanelMaintChklistReport" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
            <ContentTemplate>
                <div class="row" style="text-align: center; color: red;">
                    <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                </div>

                <div class="row" style="height: 60px;">
                    <table id="tblFilter" class="table table-bordered">
                        <tr>
                            <td class="commanTd" style="width: 80px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","LineID") %></td>
                            <td style="width: 160px;">
                                <asp:DropDownList ID="ddlLineID" runat="server" CssClass="form-control" meta:resourcekey="ddlLineIdResource1" AutoPostBack="true" OnSelectedIndexChanged="ddlLineID_SelectedIndexChanged" />
                            </td>

                            <td class="commanTd" style="width: 100px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","MachineId") %></td>
                            <td style="width: 160px;">
                                <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control" meta:resourcekey="ddlMachineIdResource1" AutoPostBack="false" />
                            </td>
                             <td class="commanTd" style="width: 60px; vertical-align: middle;">Month</td>
                            <td style="width: 140px;">
                                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control" meta:resourcekey="ddlMonthResource1" AutoPostBack="false" />
                            </td>

                            <td class="commanTd" style="width: 60px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","Year") %></td>
                            <td style="width: 140px;">
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control" meta:resourcekey="ddlYearResource1" AutoPostBack="false" />
                            </td>

                            <td style="width: 130px;">
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnView" Text="View" Style="min-width: 120px;"  OnClick="btnView_Click" />
                            </td>
                            <td>
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnExport" Text="Export" Style="min-width: 120px;" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                    </table>
                </div>

                <div style="overflow-x: auto; overflow-y: auto;height:80vh">
                    <asp:GridView ID="GridDailyChecklistReport" ClientIDMode="Static" runat="server" CssClass="table table-bordered headerFixerhere" RowStyle-BackColor="#BACADE" AlternatingRowStyle-BackColor="#DCE7F5" RowStyle-Font-Size="Medium" RowStyle-ForeColor="#383838" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true"  Height="100%"  OnDataBound="GridDailyChecklistReport_DataBound">
                        <AlternatingRowStyle BackColor="#DCE7F5" />
                        <EmptyDataTemplate>
                            No data available for selected Machine and date time period.
                        </EmptyDataTemplate>
                        <EmptyDataRowStyle BackColor="#6699ff" />
                        <HeaderStyle BackColor="#5391CA" Font-Bold="True" Font-Size="Medium" ForeColor="White" />
                        <RowStyle BackColor="#BACADE" Font-Size="Medium" ForeColor="#383838" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            debugger;
            var winHeight = $(window).height();
            winHeight = screen.availHeight;
            $("#GridDailyChecklistReport").css('height', winHeight);
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>