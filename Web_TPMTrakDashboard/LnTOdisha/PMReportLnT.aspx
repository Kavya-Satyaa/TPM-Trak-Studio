<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PMReportLnT.aspx.cs" Inherits="Web_TPMTrakDashboard.LnTOdisha.PMReportLnT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table id="tblFilter" class="table table-bordered" style="width: auto;">
                <tr>

                    <td class="commanTd" style="vertical-align: middle;">Year</td>
                    <td>
                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" Style="width: 70px; display: inline;"></asp:TextBox>

                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Month</td>
                    <td>
                        <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Month !" placeholder="Month" Style="width: 70px; display: inline;"></asp:TextBox>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Machine</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMachine" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click" OnClientClick="return callLoader();" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnExport" Text="Save" CssClass="btn btn-primary" OnClick="btnExport_Click" />
                    </td>
                </tr>
            </table>
            <div style="height: 80vh; overflow: auto;">
                <asp:ListView runat="server" ID="lvChecklist" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                    <LayoutTemplate>
                        <table class="checklist-tbl">

                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label runat="server" Text='<%# Eval("Activity") %>'></asp:Label>
                            </td>
                            <td rowspan='<%# Eval("RowSpan") %>'>
                                <asp:Label runat="server" Text='<%# Eval("AllotedTime") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" Text='<%# Eval("Frequency") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" Text='<%# Eval("LastChecked") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" Text='<%# Eval("TodayPlan") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:ListView runat="server" ID="lvMonthDara" DataSource='<%# Eval("MonthData") %>' ItemPlaceholderID="itemplaceholder2">
                                    <LayoutTemplate>
                                        <table class="inner-table">
                                            <tr>
                                                <td runat="server" id="itemplaceholder2"></td>
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <td>
                                            <asp:Label runat="server" Text='<%# Eval("MonhValue") %>'></asp:Label>
                                        </td>
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            setDateTimePicker();
            callUnLoader();
        });
        function setDateTimePicker() {
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtPlanDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
        }
        function callLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        }
        function callUnLoader() {
            $.unblockUI({});
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

            $(document).ready(function () {
                setDateTimePicker();
                callUnLoader();

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
