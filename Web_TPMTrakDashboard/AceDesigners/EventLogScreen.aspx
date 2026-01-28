<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EventLogScreen.aspx.cs" Inherits="Web_TPMTrakDashboard.AceDesigners.EventLogScreen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>

    <style>
        .tblFilter {
            width: 100%;
            box-shadow: 0px 0px 4px white;
        }

            .tblFilter > tbody > tr > td {
                color: white;
                border: 0px solid #9c9c9c;
                padding: 10px 5px;
            }

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

        .cssclass {
            min-width: 250px;
            max-width: 250px;
        }

        .Nametd {
            font-size: 16px;
            text-align: center;
        }
        .outerTable{
            /*box-shadow: 0px 0px 4px white;*/
            width: 100%;
        }
        .outerTable > tbody > tr >td {
            color: white;
            border-collapse: collapse;
        }
        .InnerTable{
            width:100%;
            max-height: 100%;
            min-height: 100%;
        }
        .InnerTable > tbody > tr > td{
            color: black;
            border: 1px solid #ddd;
            padding: 10px;
        }
            .InnerTable > tbody > tr:nth-child(odd) > td {
                background-color: white;
            } 
            .InnerTable > tbody > tr:nth-child(even) > td {
                background-color: #ddd;
            }
        .tdEventID{
            background-color: #c2c2c2;
            font-size: 18px;
            color: white !important;
            font-weight: bold;
        }
        .InnerTable > tbody > tr > th{
            border: 1px solid #ddd;
            height: 38px;
            text-align: center;
                padding: 10px;
        }
        #lblEventID{
            padding: 15px;
        }
        .Date{
            position:relative;
        }
        .bootstrap-datetimepicker-widget{
            color: black;
        }
    </style>
    <table class="tblFilter">
        <tr>
            <td class="Nametd">From Date</td>
            <td style="position: relative;">
                <asp:TextBox runat="server" ID="txtFromDate" CssClass="Date form-control"></asp:TextBox>
            </td>
            <td class="Nametd">To Date</td>
            <td style="position: relative;">
                <asp:TextBox runat="server" ID="txtToDate" CssClass="form-control Date"></asp:TextBox>
            </td>
            <td class="Nametd">Machine ID</td>
            <td>
                <asp:ListBox runat="server" ID="lbMachineID" SelectionMode="Multiple" CssClass="form-control" ClientIDMode="Static"></asp:ListBox>
            </td>
            <td class="Nametd">Event</td>
            <td>
                <asp:ListBox runat="server" ID="lbEventID" SelectionMode="Multiple" CssClass="form-control" ClientIDMode="Static"></asp:ListBox>
                <%--<asp:DropDownList runat="server" ID="ddlEvent" CssClass="form-control"></asp:DropDownList>--%>
            </td>
            <td class="text-center">
                <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click" CssClass="btn btn-primary" />
                <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-primary" OnClick="btnExport_Click" />
            </td>
        </tr>
    </table>

    <div style="margin-top: 20px;">
        <asp:ListView runat="server" ID="lvEventGrid">
            <LayoutTemplate>
                <div runat="server" id="itemplaceholder"></div>
            </LayoutTemplate>
            <ItemTemplate>
                    <div class="col-lg-4"  style="max-height: 80vh; overflow: auto; margin: 10px 0px;">
                        <table runat="server" class="outerTable">
                            <tr>
                                <td class="tdEventID text-center" style="padding: 5px 10px 0px 10px; width: 25%; background-color: #34546b; border: 1px solid #ddd; border-bottom: 0px !important;" colspan="4">
                                    <asp:Label runat="server" ID="lblEventID" ClientIDMode="Static" Text='<%# Eval("EventID") %>'></asp:Label>
                                </td>
                                </tr>
                            <tr>
                                <td style="padding: 0px 0px 0px 0px !important;">
                                    <asp:ListView runat="server" ID="lvInnerData" DataSource='<%# Eval("EventDetails") %>'>
                                        <LayoutTemplate>
                                            <table runat="server" class="InnerTable headerFixer">
                                                <tr runat="server" id="itemplaceholder"></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr class="text=center" runat="server" visible='<%# Eval("HeaderVisibility") %>'>
                                                <th>
                                                    <asp:Label runat="server" Text="Sl No."></asp:Label>
                                                </th>
                                                <th runat="server" visible='<%# Eval("MachineColumnVisibility") %>'>
                                                    <asp:Label runat="server" Text="Machine ID"></asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label runat="server" Text="Time"></asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label runat="server" Text="Enable/ Disable"></asp:Label>
                                                </th>
                                            </tr>
                                            <tr class="ContentRow" runat="server" visible='<%# Eval("ContentVisibility") %>'>
                                                <td style="width: 10%;">
                                                    <asp:Label runat="server" Text='<%# Eval("SlNo") %>'></asp:Label>
                                                </td>
                                                <td runat="server" style="width: 38%;" visible='<%# Eval("MachineColumnVisibility") %>'>
                                                    <asp:Label runat="server" Text='<%# Eval("MachineID") %>'></asp:Label>
                                                </td>
                                                <td style="width: 402%;">
                                                    <asp:Label runat="server" Text='<%# Eval("AlarmTime") %>'></asp:Label>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:Label runat="server" Text='<%# Eval("EnableDisable") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ItemTemplate>
        </asp:ListView>
    </div>

    <script>
        $(document).ready(function () {
            $(".Date").datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $("#lbMachineID").multiselect({
                includeSelectAllOption: true
            });
            $("#lbEventID").multiselect({
                includeSelectAllOption: true
            });
        });

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function (){
            $(".Date").datetimepicker({
                    format: 'DD-MM-YYYY HH:mm:ss',
                    locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
                });
                $("#lbMachineID").multiselect({
                    includeSelectAllOption: true
                });
            $("#lbEventID").multiselect({
                    includeSelectAllOption: true
                });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
