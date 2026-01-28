<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyChecklistReportPitti.aspx.cs" Inherits="Web_TPMTrakDashboard.Pitti.DailyChecklistReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>
    <style>
        .tblfilter {
            width: 70%;
            box-shadow: 0px 0px 2px black;
        }

            .tblfilter > tbody > tr > td {
                color: white;
                vertical-align: middle;
                font-size: 15px;
                border: 1px solid #4e5166 !important;
            }

        .multiselect-container {
            height: 200px;
            overflow-x: auto;
        }

        .multiselect-selected-text {
            padding-right: 181px;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
        }

        .cssclass {
            min-width: 400px;
        }
    </style>
    <table class="table table-bordered tblfilter">
        <tr>
            <td class="text-center">Machine ID</td>
            <td>
                <asp:ListBox runat="server" ID="ddlMachineID" ClientIDMode="Static" SelectionMode="Multiple" CssClass="form-control cssclass"></asp:ListBox>
            </td>
            <td class="text-center">Year</td>
            <td>
                <%--<asp:DropDownList runat="server" ID="ddlYear" CssClass="form-control"></asp:DropDownList>--%>
                <asp:TextBox runat="server" ID="txtYear" CssClass="form-control Year"></asp:TextBox>
            </td>
            <td class="text-center">Month</td>
            <td>
                <%-- <asp:DropDownList runat="server" ID="ddlMonth" CssClass="form-control">
                    <asp:ListItem Text="Jan" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Feb" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Mar" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Apr" Value="4"></asp:ListItem>
                    <asp:ListItem Text="May" Value="5"></asp:ListItem>
                    <asp:ListItem Text="Jun" Value="6"></asp:ListItem>
                    <asp:ListItem Text="Jul" Value="7"></asp:ListItem>
                    <asp:ListItem Text="Aug" Value="8"></asp:ListItem>
                    <asp:ListItem Text="Sep" Value="9"></asp:ListItem>
                    <asp:ListItem Text="Oct" Value="10"></asp:ListItem>
                    <asp:ListItem Text="Nov" Value="11"></asp:ListItem>
                    <asp:ListItem Text="Dec" Value="12"></asp:ListItem>
                </asp:DropDownList>--%>
                <asp:TextBox runat="server" ID="txtMonth" CssClass="form-control Month"></asp:TextBox>
            </td>
            <td class="text-center" style="display: none;">
                <asp:Button runat="server" ID="btnView" Text="View" ClientIDMode="Static" CssClass="btn btn-primary" />
            </td>
            <td class="text-center">
                <asp:Button runat="server" ID="btnExport" Text="Export" ClientIDMode="Static" OnClick="btnExport_Click" CssClass="btn btn-primary" />
            </td>
        </tr>
    </table>


    <script>
        $(document).ready(function () {
            $("#ddlMachineID").multiselect({
                includeSelectAllOption: true
            });
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
        })
        });

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $("#ddlMachineID").multiselect({
                includeSelectAllOption: true
            });
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
        })
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
