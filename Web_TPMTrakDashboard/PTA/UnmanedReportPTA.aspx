<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UnmanedReportPTA.aspx.cs" Inherits="Web_TPMTrakDashboard.PTA.UnmanedReportPTA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
            <table id="tblFilter" class="table table-bordered" style="width: auto;">
                <tr>

                    <td class="commanTd" style="vertical-align: middle;">Plant</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPlant" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Machine</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMachine" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Month-Year</td>
                    <td>

                        <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control date date-control" placeholder="MM" AutoCompleteType="Disabled" ClientIDMode="Static" Style="display: inline-block; width: 100px;"></asp:TextBox>
                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control date date-control" placeholder="YYYY" AutoCompleteType="Disabled" ClientIDMode="Static" Style="display: inline-block; width: 100px;"></asp:TextBox>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Shift</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlShift" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click" OnClientClick="return callLoader();" />
                    </td>
                    <td id="tdExport" runat="server">
                        <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-primary" OnClick="btnExport_Click" />
                    </td>
                    <td id="tdBack" runat="server">
                        <asp:Button runat="server" ID="btnBack" Text="Back" CssClass="btn btn-primary" OnClick="btnBack_Click" />
                    </td>
                </tr>
            </table>
            <div id="summaryContainer" runat="server" style="height: 80vh; overflow: auto;" clientidmode="static">
                <asp:ListView runat="server" ID="lvSummary" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                    <LayoutTemplate>
                        <table class="table table-bordered headerFixer alternate-table-style">
                            <tr>
                                <th>Machine ID</th>
                                <th>Utilised Time (min)</th>
                                <th>Down Time (min)</th>
                                <th>Components</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:LinkButton runat="server" ID="lnkMachine" ClientIDMode="Static" Text='<%# Eval("MachineID") %>' OnClick="lnkMachine_Click" OnClientClick="return callLoader();"></asp:LinkButton>

                            </td>
                            <td>
                                <%# Eval("UtilisedTime") %>
                            </td>
                            <td>
                                <%# Eval("DownTime") %>
                            </td>
                            <td>
                                <%# Eval("NoOfComponents") %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <div id="detailContainer" runat="server" style="height: 80vh; overflow: auto;">
                <asp:ListView runat="server" ID="lvDetails" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                    <LayoutTemplate>
                        <table class="table table-bordered headerFixer alternate-table-style">
                            <tr>
                                <th>Date</th>
                                <th>Machine ID</th>
                                <th>Component</th>
                                <th>Operation</th>
                                <th>Operator</th>
                                <th>Batch Start</th>
                                <th>Batch End</th>
                                <th>No. of Components</th>
                                <th>Utilised Time</th>
                                <th>Down Time (min)</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("Date") %>
                            </td>
                            <td>
                                <%# Eval("MachineID") %>
                            </td>
                            <td>
                                <%# Eval("Component") %>
                            </td>
                            <td>
                                <%# Eval("Operation") %>
                            </td>
                            <td>
                                <%# Eval("Operator") %>
                            </td>
                            <td>
                                <%# Eval("BatchStart") %>
                            </td>
                            <td>
                                <%# Eval("BatchEnd") %>
                            </td>
                            <td>
                                <%# Eval("NoOfComponents") %>
                            </td>
                            <td>
                                <%# Eval("UtilisedTime") %>
                            </td>
                            <td>
                                <%# Eval("DownTime") %>
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
    <script>
        var bigDiv = document.getElementById('summaryContainer');
        $(document).ready(function () {
            setDateTimePicker();
            callUnLoader();
        });
        function setDateTimePicker() {
            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

        }
        function callLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        }
        function callUnLoader() {
            $.unblockUI({});
        }
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
        }
        window.onload = function () {

            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            var bigDiv = document.getElementById('summaryContainer');
            $(document).ready(function () {
                setDateTimePicker();
                callUnLoader();
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            });
            bigDiv.onscroll = function () {
                $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
            }
            window.onload = function () {

                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
