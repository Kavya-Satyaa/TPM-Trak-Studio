<%@ Page Title="Operator Message" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OperatorMessage.aspx.cs" Inherits="Web_TPMTrakDashboard.Bajaj.OperatorMessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <div class="content-div">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="bajaj-outer-div-filter-section">
                    <div class="bajaj-inner-div-filter-section left-content-filter-section">
                        <table class="bajaj-filter-tbl">
                            <tr>
                                <td>From Date</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtFromDate" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td>To Date</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtToDate" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td>Plant</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Cell</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlCell" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCell_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>Machine</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlMachine" CssClass="form-control"></asp:DropDownList>
                                </td>
                                <td colspan="4">
                                    <asp:Button runat="server" ID="btnView" OnClick="btnView_Click" CssClass="bajaj-btn-style" Text="View" />
                                    <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" CssClass="bajaj-btn-style" Text="Export" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="scrollMaintainDiv" style="height: 76vh; overflow: auto; margin-top: 12px">
                    <asp:ListView runat="server" ID="lvOprMsgDetails" ClientIDMode="Static">
                        <EmptyDataTemplate>
                            <table class="table table-bordered table-hover headerFixer" id="tblOprMsgDetails">
                                <tr>
                                    <th>Alarm No.</th>
                                    <th>Alarm Date</th>
                                    <th>Alarm Message</th>
                                    <th>Group Number</th>
                                </tr>
                                <tr>
                                    <td colspan="30" class="no-data-found-td"><span class="no-data-found">No Data Found</span></td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblOprMsgDetails">
                                <tr>
                                    <th>Alarm No.</th>
                                    <th>Alarm Date</th>
                                    <th>Alarm Message</th>
                                    <th>Group Number</th>
                                </tr>
                                <tr id="itemplaceholder" runat="server"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Eval("AlarmNo") %>
                                </td>
                                <td>
                                    <%# Eval("AlarmDate") %>
                                </td>
                                <td>
                                    <%# Eval("AlarmMessage") %>
                                </td>
                                <td>
                                    <%# Eval("GroupNo") %>
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
            DateTimeSetter();
        });
        function DateTimeSetter() {
            $('[id$=txtFromDate]').datepicker({
                viewMode: "date",
                minViewMode: "date",
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
                //startDate: '-1d'
            });
            $('[id$=txtToDate]').datepicker({
                viewMode: "date",
                minViewMode: "date",
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
                //startDate: '-1d'
            });
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                DateTimeSetter();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
