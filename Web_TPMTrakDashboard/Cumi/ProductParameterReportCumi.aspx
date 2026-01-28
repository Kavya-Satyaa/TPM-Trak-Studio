<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductParameterReportCumi.aspx.cs" Inherits="Web_TPMTrakDashboard.Cumi.ProductParameterReportCumi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <style>
        .dateControl {
            width: 120px;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
            <div class="bajaj-outer-div-filter-section">
                <div class="bajaj-inner-div-filter-section left-content-filter-section">
                    <table class="bajaj-filter-tbl">
                        <tr>
                            <td>From Date</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFromDate" CssClass="form-control dateControl" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>To Date</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtToDate" CssClass="form-control dateControl" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>Plant</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td>Machine</td>
                            <td>
                                <asp:ListBox ID="lbMachine" runat="server" SelectionMode="Multiple" CssClass="listBox"></asp:ListBox>
                            </td>
                            <td>Shift</td>
                            <td>
                                <asp:ListBox ID="lbShiftID" runat="server" SelectionMode="Multiple" CssClass="listBox"></asp:ListBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click" CssClass="bajaj-btn-style" OnClientClick="return showLoader();" />
                            </td>
                            <td>

                                <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="bajaj-btn-style" Style="background-color: #9f0e9f; color: white" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="scrollMaintainDiv" style="height: 70vh; overflow: auto; margin-top: 5px; width: 100%; display: inline-block">

                <asp:ListView runat="server" ID="lvProductReport" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                    <EmptyDataTemplate>
                        <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblLossDetails">
                            <tr>
                                <th>Date</th>
                                <th>Shift</th>
                                <th>Machine ID</th>
                                <th>PO Number</th>
                                <th>Item Code</th>
                                <th>Employee</th>
                                <th>Weight</th>
                                <th>OOB</th>
                                <th>THK</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblLossDetails">
                            <tr>
                                <th>Date</th>
                                <th>Shift</th>
                                <th>Machine ID</th>
                                <th>PO Number</th>
                                <th>Item Code</th>
                                <th>Employee</th>
                                <th>Weight</th>
                                <th>OOB</th>
                                <th>THK</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>

                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("DateInString") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("Shift") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("MachineID") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("PONumber") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("ProductCode") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("EmployeeName") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("Weight") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("OOB") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("Thickness") %>'></asp:Label>
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
        var bigDiv = document.getElementById('scrollMaintainDiv');

        $(document).ready(function () {
            DateTimeSetter();
            $.unblockUI({});
        });
        function showLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }

        function DateTimeSetter() {
            $('.listBox').multiselect({
                includeSelectAllOption: true
            });
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
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
        }
        window.onload = function () {

            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            var bigDiv = document.getElementById('scrollMaintainDiv');
            $(document).ready(function () {
                DateTimeSetter();
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
                $.unblockUI({});
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
