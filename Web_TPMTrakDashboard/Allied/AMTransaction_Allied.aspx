<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AMTransaction_Allied.aspx.cs" Inherits="Web_TPMTrakDashboard.Allied.AMTransaction_Allied" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>
    <style>
        .tblSettings {
            width: 97%;
            box-shadow: 0px 0px 4px #afafaf;
            border-radius: 10px;
        }

            .tblSettings > tbody > tr > td {
                color: white;
                padding: 5px 10px;
                /*border: 1px solid black;*/
                border-collapse: collapse;
                text-align: center;
                font-size: large;
                max-width: 150px;
                /*box-shadow: 2px 2px 2px black;*/
            }

        .btnclass {
            box-shadow: 1px 1px 10px #515151;
            font-size: 16px;
        }

        .lvDetails {
            width: 97%;
            margin: 10px;
        }

            .lvDetails > tbody > tr > td {
                border: 1px solid #b6b6b6;
                padding: 5px;
            }

            .lvDetails > tbody > tr:nth-child(even) > td {
                background-color: white;
            }

            .lvDetails > tbody > tr:nth-child(odd) > td {
                background-color: #ddd;
            }

            .lvDetails > tbody > tr > th {
                border: 1px solid #b6b6b6;
                height: 40px;
                text-align: center;
            }

        .inner-tbl {
            width: 100%;
            height: 100%;
        }

            .inner-tbl > tbody > tr > td {
                border-right: 1px solid #b6b6b6;
                height: 40px;
                min-width: 200px;
                max-width: 200px;
                padding: 5px;
                text-align: center;
                max-width: 100px;
                overflow-wrap: break-word;
            }

            .inner-tbl > tbody > tr > th {
                border-right: 1px solid #b6b6b6;
                height: 40px;
                min-width: 200px;
                max-width: 200px;
                text-align: center;
                max-height: 100px;
            }
              .approveRowtr td {
            position: sticky;
            bottom: -1px;
        }

    </style>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div>
                    <table class="tblSettings">
                        <tr>
                            <td>Machine ID</td>
                            <td>
                                <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlMachineID"></asp:DropDownList>
                            </td>
                            <td>Frequency</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control" OnSelectedIndexChanged="ddlFrequency_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
                                    <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem>
                                    <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Year</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtYear" CssClass="form-control Year"></asp:TextBox>
                            </td>
                            <td runat="server" id="tdMonth">Month</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMonth" CssClass="form-control Month"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ClientIDMode="Static" CssClass="bajaj-btn-style btnclass" Text="View" ID="btnView" OnClick="btnView_Click" />
                            </td>
                            <td>
                                <asp:Button runat="server" ClientIDMode="Static" CssClass="bajaj-btn-style btnclass" Text="Export" ID="btnExport" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="height: 80vh; overflow: auto;">
                    <asp:ListView runat="server" ItemPlaceholderID="itemplaceholder" ClientIDMode="Static" ID="lvDetails">
                        <LayoutTemplate>
                            <table class="lvDetails headerFixer" id="lvDetails">
                                <tr runat="server" id="itemplaceholder"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server" visible='<%# Eval("HeaderVisibility") %>'>
                                <th style="min-width:70px;text-align:center;">
                                    <asp:Label runat="server" Text="Sl No"></asp:Label>
                                </th>
                                <th style="min-width:230px;text-align:center;">
                                    <asp:Label runat="server" Text="Checkpoint"></asp:Label>
                                </th>
                                <th style="padding: 0px !important;" runat="server">
                                    <asp:ListView runat="server" DataSource='<%# Eval("transactionData") %>'>
                                        <LayoutTemplate>
                                            <table class="inner-tbl">
                                                <tr>
                                                    <th runat="server" id="itemplaceholder"></th>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <th>
                                                <asp:Label runat="server" Text='<%# Eval("HearderValue") %>'></asp:Label>
                                            </th>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </th>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ContentVisibility") %>'>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("CheckpointID") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("CheckpointDesc") %>'></asp:Label>
                                </td>
                                <td style="padding: 0px !important;">
                                    <asp:ListView runat="server" DataSource='<%# Eval("transactionData") %>'>
                                        <LayoutTemplate>
                                            <table class="inner-tbl">
                                                <tr>
                                                    <td runat="server" id="itemplaceholder"></td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <td>
                                                <asp:Label runat="server" Text='<%# Eval("HearderValue") %>'></asp:Label>
                                            </td>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                               <tr runat="server" class="approveRowtr" visible='<%# Eval("ApproveVisibility") %>'>
                                <td style="border: 0px !important;"></td>
                                <td style="text-align: right; font-weight: bold; padding: 5px;">
                                    <asp:Label runat="server" Text='Operator' Font-Bold="true"></asp:Label>
                                </td>
                                <td style="padding: 0px !important;">
                                    <asp:ListView runat="server" ID="lvFootergrid" ClientIDMode="Static" DataSource='<%# Eval("TransactionData") %>'>
                                        <LayoutTemplate>
                                            <table style="height: 100%; width: 100%" class="inner-tbl">
                                                <tr>
                                                    <td runat="server" id="itemplaceholder"></td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <td runat="server" style="min-width: 180px; padding: 5px 0px !important; text-align: center; border-right: 1px solid grey; border-bottom: 1px solid grey;">
                                                <asp:Label runat="server" Text='<%# Eval("ContentValue") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                                            </td>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </ContentTemplate>
            <Triggers>
                  <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {
            freezeColumnFromLeft("lvDetails", "2");
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
            freezeColumnFromLeft("lvDetails", "2");
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
