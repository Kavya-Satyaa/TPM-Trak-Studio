<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SPCTargetMasterCumi.aspx.cs" Inherits="Web_TPMTrakDashboard.Cumi.SPCTargetMasterCumi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <%: Styles.Render("~/bundles/datecss") %>
            <%: Scripts.Render("~/bundles/datejs") %>
            <%: Styles.Render("~/bundles/toastrCss") %>
            <%: Scripts.Render("~/bundles/toastrJs") %>
            <div class="bajaj-outer-div-filter-section">
                <div class="bajaj-inner-div-filter-section left-content-filter-section">
                    <table class="bajaj-filter-tbl">
                        <tr>
                            <td style="font-weight: bold;">Year</td>
                            <td>
                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtYear" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnView" OnClick="btnView_Click" Text="VIEW" ClientIDMode="Static" CssClass="bajaj-btn-style" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSave" Text="SAVE" ClientIDMode="Static" CssClass="bajaj-btn-style" OnClick="btnSave_Click" />
                            </td>
                        </tr>
                    </table>


                </div>
            </div>
            <div style="width: 100%">
                <div id="scrollMaintainDiv" style="height: 80vh; overflow: auto; margin-top: 12px; min-width: 40%; display: inline-block">
                    <asp:ListView runat="server" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder" ID="lvData">
                        <LayoutTemplate>
                            <table class="table table-bordered table-hover headerFixer bajaj-table-style">
                                <tr>
                                    <th>Month</th>
                                    <th>Target Value  [KWH/Tonnage]</th>
                                </tr>
                                <tr runat="server" id="itemplaceholder"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>

                                <td>
                                    <asp:HiddenField runat="server" ID="hdnMonthInInt" ClientIDMode="Static" Value='<%# Eval("MonthInInt") %>' />
                                    <asp:Label runat="server" Text='<%# Eval("Month") %>' ClientIDMode="Static" ID="lblMonth"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" Text='<%# Eval("TargetValue") %>' ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control" ID="txtTargetValue"></asp:TextBox>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            setControls();
        });
        function setControls() {
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: 'en-US',
            });
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                setControls();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
