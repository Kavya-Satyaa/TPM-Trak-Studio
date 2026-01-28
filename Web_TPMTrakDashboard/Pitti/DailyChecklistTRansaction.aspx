<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyChecklistTRansaction.aspx.cs" Inherits="Web_TPMTrakDashboard.Pitti.DailyChecklistTRansaction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    
    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>
    <style>
        .lvCheckpointReportGrid > tbody > tr:first-child > td {
            background-color: #2e6886 !important;
            Color: white;
            font-size: 12px;
            font-weight: bold;
            text-align: center;
        }

        .lvCheckpointReportGrid > tbody > tr:nth-child(odd) > td {
            background-color: #ddd;
        }

        .lvCheckpointReportGrid > tbody > tr:nth-child(even) > td {
            background-color: white;
        }

        .lvInnerView > tbody > tr > td {
            min-width: 80px;
            max-width: 80px;
            min-height: 100%;
            background-color: transparent;
        }

        .cssclass {
            min-width: 300px;
        }
        .Year{
            position: relative;
        }
    </style>
    <table>
        <tr>
            <td>Machine ID</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlMachineID" CssClass="form-control"></asp:DropDownList>
            </td>
            <td>Year</td>
            <td style="position: relative;">
                <asp:TextBox runat="server" ID="txtYear" CssClass="form-control Year"></asp:TextBox>
            </td>
            <td>Month</td>
            <td>
                <asp:TextBox runat="server" ID="txtMonth" CssClass="form-control Month"></asp:TextBox>
            </td>
        </tr>
    </table>
        <div style="overflow: auto; width: 100%;">
        <asp:ListView runat="server" ID="lvCheckpointReportGrid">
            <LayoutTemplate>
                <table class="table table-bordered lvCheckpointReportGrid" id="lvCheckpointReportGrid" clientidmode="static" style="margin: 0px !important;">
                    <tr runat="server" id="itemplaceholder"></tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr runat="server" id="HeaderVisibility">
                    <td style="min-width: 120px;">
                        <asp:Label runat="server" Text="Machine ID" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" Text='<%# Eval("MachineID") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td style="min-width: 120px;">
                        <asp:Label runat="server" Text="Checkpoint ID" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" Text='<%# Eval("CheckPointID") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td style="min-width: 400px;">
                        <asp:Label runat="server" Text="Checkpoint Desc" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" Text='<%# Eval("CheckPointDesc") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td style="min-width: 120px;">
                        <asp:Label runat="server" Text="Standard" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" Text='<%# Eval("Standard") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td style="min-width: 80px;">
                        <asp:Label runat="server" Text="Frequency" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" Text='<%# Eval("Frequency") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td style="padding: 0px;">
                        <asp:ListView runat="server" ID="lvInnerView" DataSource='<%# Eval("InnerListViewData") %>'>
                            <LayoutTemplate>
                                <table class="table table-bordered lvInnerView" style="margin: 0px !important;">
                                    <tr>
                                        <td runat="server" id="itemplaceholder"></td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                    <td class="text-center" runat="server" Visible='<%# Eval("HeaderVisibility") %>' style="background-color: #2e6886; color: white;">
                                        <asp:Label runat="server" Text='<%# Eval("DateValue") %>'></asp:Label>
                                    </td>
                                    <td class="text-center" Visible='<%# Eval("ContentVisibility") %>' runat="server">
                                        <asp:Label runat="server" Text='<%# Eval("DateValue") %>'></asp:Label>
                                    </td>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>
<script>
    $(document).ready(function () {
        freezeColumnFromLeft("lvCheckpointReportGrid", 5);
        $(".Year").datepicker({
            format: 'yyyy',
            viewMode: "years",
            minViewMode: "years",
            autoclose: true,
            orientation: "top",
            autocomplete: "off",
            language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
        });
        $(".Month").datepicker({
            format: 'mm',
            viewMode: "months",
            minViewMode: "months",
            autoclose: true,
            orientation: "top",
            autocomplete: "off",
            language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
        });
    });



    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
        freezeColumnFromLeft(lvCheckpointReportGrid, 5);
        $(".Year").datepicker({
            format: 'yyyy',
            viewMode: "years",
            minViewMode:"years",
            autoclose: true,
            orientation: "top",
            autocomplete: "off",
            language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
        });
        $(".Month").datepicker({
            format: 'mm',
            viewMode: "months",
            minViewMode: "months",
            autoclose: true,
            orientation: "top",
            autocomplete: "off",
            language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
