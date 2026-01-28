<%@ Page Title="POStatus" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="POStatus.aspx.cs" Inherits="Web_TPMTrakDashboard.Cumi.POStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <link href="Scripts/SearchableDropDown/select2.min.css" rel="stylesheet" />
    <script src="Scripts/SearchableDropDown/select2.min.js"></script>
    <style>
        .btn-group, .multiselect-native-select .btn-group button {
            width: 100% !important;
        }

        .multiselect-native-select .btn-group button {
            text-align: left;
        }

        .select2-container {
            width: 100% !important;
        }

            .select2-container .select2-selection--single {
                height: 35px !important;
            }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnViewType" ClientIDMode="Static" />
            <div class="bajaj-outer-div-filter-section">
                <div class="bajaj-inner-div-filter-section left-content-filter-section">
                    <table class="bajaj-filter-tbl" style="box-shadow: 0 4px 8px 0 rgb(0 0 0 / 25%), 0 6px 20px 0 rgb(0 0 0 / 19%);">
                        <tr>
                            <td>From Date</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFromDate" CssClass="form-control" AutoCompleteType="Disabled" OnTextChanged="txtFromDate_TextChanged1" AutoPostBack="true"></asp:TextBox>
                            </td>
                            <td>To Date</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtToDate" CssClass="form-control" AutoCompleteType="Disabled" OnTextChanged="txtToDate_TextChanged1" AutoPostBack="true"></asp:TextBox>
                            </td>
                            <td>
                                 <asp:Button runat="server" ID="btndateView" Text="View"  CssClass="bajaj-btn-style" OnClick="btnDateView_Click" OnClientClick="return showLoader();" style="margin-right:10px;"/>
                            </td>
                         <%--   <td>Shift</td>
                            <td>
                                <asp:ListBox ID="lbShift" runat="server" SelectionMode="Multiple" AutoPostBack="true"></asp:ListBox>
                            </td>
                            <td>Plant</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>--%>
                        </tr>
                        <tr>
                           <%-- <td>Machine</td>
                            <td>
                                <asp:ListBox ID="lbMachine" runat="server" SelectionMode="Multiple" AutoPostBack="true"></asp:ListBox>
                            </td>--%>
                            <td>PO Number
                            </td>
                            <td colspan="3">
                                <div runat="server" id="divProdOrderSearch" style="display: inline-block;">
                                    <asp:TextBox runat="server" ID="txtPONumberSearch" ClientIDMode="Static" CssClass="form-control" Style="display: inline-block; width: 150px"></asp:TextBox>
                                    <asp:Button runat="server" ID="btnPOSearch" Text="Search" CssClass="bajaj-btn-style" OnClick="btnPOSearch_Click" Style="margin-right: 0px;background-color:lightsteelblue" />
                                     <asp:Button runat="server" ID="btnPoClear" Text="Clear" CssClass="bajaj-btn-style" OnClick="btnPoClear_Click" Style="margin-right: 0px;background-color:lightsteelblue" />
                                </div>
                                <asp:DropDownList ID="ddlPONumber" runat="server" CssClass="select form-control loadData cssclass" Style="display: inline-block; width: 150px; min-width: 150px;">
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
                                <asp:Button runat="server" ID="btnView" Text="View" CssClass="bajaj-btn-style" OnClick="btnView_Click" OnClientClick="return showLoader();" Style="margin-right: 10px;" />
                                <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="bajaj-btn-style" Style="background-color: #9f0e9f; color: white" OnClick="btnExport_Click" />
                            </td>

                        </tr>
                    </table>
                </div>
            </div>

            <div id="scrollMaintainDiv" style="height: 75vh; overflow: auto; margin-top: 5px; width: 60%; display: inline-block">
                <asp:ListView runat="server" ID="lvPOStatus" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                    <EmptyDataTemplate>
                        <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblPOStatus">
                            <tr>
                               <%-- <th>Date</th>
                                <th>Shift</th>
                                <th>Machine ID</th>--%>
                                <th>PO Number</th>
                                <th>Item Code</th>
                                <%--<th>Employee ID</th>--%>
                                <th>PO Qty</th>
                                <th>Produced Qty</th>
                                <th>Balance Qty</th>
                            </tr>
                            <tr>
                                <td colspan="100" class="no-data-found-td"><span class="no-data-found" style="color: black">No Data Found</span></td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblProcessParameterDetails">
                            <tr>
                               <%-- <th>Date</th>
                                <th>Shift</th>
                                <th>Machine ID</th>--%>
                                <th>PO Number</th>
                                <th>Item Code</th>
                               <%-- <th>Employee ID</th>--%>
                                <th>PO Qty</th>
                                <th>Produced Qty</th>
                                <th>Balance Qty</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                           <%-- <td>
                                <asp:Label runat="server" ID="lblDate" ClientIDMode="Static" Text='<%# Eval("Date") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblShift" ClientIDMode="Static" Text='<%# Eval("Shift") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblHydraulicPressureTop" ClientIDMode="Static" Text='<%# Eval("MachineID") %>'></asp:Label>
                            </td>--%>
                            <td>
                                <asp:Label runat="server" ID="lblPPONo" ClientIDMode="Static" Text='<%# Eval("PONumber") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblItemCode" ClientIDMode="Static" Text='<%# Eval("ItemCode") %>'></asp:Label>
                            </td>
                           <%-- <td>
                                <asp:Label runat="server" ID="lblHydraulicPressureBottom" ClientIDMode="Static" Text='<%# Eval("EmployeeID") %>'></asp:Label>
                            </td>--%>
                            <td>
                                <asp:Label runat="server" ID="lblPOQty" ClientIDMode="Static" Text='<%# Eval("POQty") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblProducedQty" ClientIDMode="Static" Text='<%# Eval("ProducedQty") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblBalanceQty" ClientIDMode="Static" Text='<%# Eval("BalanceQty") %>'></asp:Label>
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
        $(document).ready(function () {
            $("#ddlPONumber").select2({
                placeholder: "PO Number",
                allowClear: true
            });
            $('[id$=lbMachine]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbShift]').multiselect({
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
                //endDate: '1d'
            });
            $('[id$=txtToDate]').datepicker({
                viewMode: "date",
                minViewMode: "date",
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
                //endDate: '1d'
            });
            $.unblockUI({});
        });

        function showLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                $("#ddlPONumber").select2({
                    placeholder: "PO Number",
                    allowClear: true
                });
                $('[id$=lbMachine]').multiselect({
                    includeSelectAllOption: true
                });
                $('[id$=lbShift]').multiselect({
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
                    //endDate: '1d'
                });
                $('[id$=txtToDate]').datepicker({
                    viewMode: "date",
                    minViewMode: "date",
                    format: 'dd-mm-yyyy',
                    todayHighlight: true,
                    autoclose: true,
                    language: 'en-US',
                    //endDate: '1d'
                });
                $.unblockUI({});
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
