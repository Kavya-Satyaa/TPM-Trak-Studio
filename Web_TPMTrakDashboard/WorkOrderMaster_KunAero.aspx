<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WorkOrderMaster_KunAero.aspx.cs" Inherits="Web_TPMTrakDashboard.WorkOrderMaster_KunAero" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <link href="Scripts/Toastr/toastr.min.css" rel="stylesheet" />
    <script src="Scripts/Toastr/toastr.min.js"></script>
    <style>
        #gvGrid tr td {
            background-color: white;
            text-align: center;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <div class="col-lg-8 col-md-8 col-sm-8">
                        <table class="filter-table">
                            <tr>
                                <td class="filter-table-header" style="vertical-align: middle; color: white;">From Date</td>
                                <td>
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar"></i>
                                        </div>
                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled"></asp:TextBox>
                                    </div>
                                </td>
                                <td class="filter-table-header" style="vertical-align: middle; color: white;">To Date</td>
                                <td>
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar"></i>
                                        </div>
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled"></asp:TextBox>
                                    </div>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtWorkOrder" placeholder="WorkOrderNumber Contains.." CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnSearch" ClientIDMode="Static" OnClick="btnView_Click" Text="Search" CssClass="bajaj-btn-style" />
                                </td>
                               <%-- <td style="color:white;font-weight:bold;font-size:15px;padding:3px;">Financial Year</td>
                                <td>
                                    <asp:ListBox runat="server" ClientIDMode="Static" SelectionMode="Multiple" CssClass="form-control" ID="lbFyear"></asp:ListBox>
                                </td>
                                <td style="color:white;font-weight:bold;font-size:15px;padding:3px;">Work Order ID</td>
                                <td>
                                    <asp:DropDownList runat="server" ClientIDMode="Static" ID="ddlWorkOrderID" CssClass="form-control"></asp:DropDownList>
                                </td>
                                <td style="color:white;font-weight:bold;font-size:15px;padding:3px;">Work Order No</td>
                                <td>
                                    <asp:DropDownList runat="server" ClientIDMode="Static" ID="ddlWorkOrderNo" CssClass="form-control"></asp:DropDownList>
                                </td>
                                <td style="color:white;font-weight:bold;font-size:15px;padding:3px;">Work Order Date</td>
                                <td>
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar"></i>
                                        </div>
                                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled"></asp:TextBox>
                                    </div>
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnView" ClientIDMode="Static" CssClass="btn btn-info" Text="VIEW" OnClick="btnView_Click" />
                                </td>--%>
                            </tr>
                        </table>
                    </div>
                    <div class="col-lg-4 col-md-4 col-sm-4" style="float: right; text-align: right;">
                        <table class="filter-table">
                            <tr>
                                 <td>
                                    <asp:FileUpload runat="server" ID="fileUploader" CssClass="form-control" />
                                </td>
                                <td>
                                    <asp:Button runat="server" ClientIDMode="Static" ID="btnImport" Text="IMPORT" OnClick="btnImport_Click" CssClass="btn btn-info" />
                                </td>
                                <td>
                                    <asp:LinkButton runat="server" ID="btnTemplate" Text="Download Sample Template" CssClass="glyphicon glyphicon-download-alt btn btn-info" ClientIDMode="Static" OnClick="btnTemplate_Click"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div style="margin-top: 10px; height: 80vh; overflow: auto;">
                <asp:ListView runat="server" ID="lvGrid" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder" InsertItemPosition="FirstItem">
                    <LayoutTemplate>
                        <table class="table table-bordered headerFixer">
                            <tr>
                                <th>Sl No</th>
                                <th>Work Order FY</th>
                                <th>WOrk Order ID</th>
                                <th>Work Order Number</th>
                                <th>Work Order Date</th>
                                <th>Work Order Qty</th>
                                <th>Action</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <EmptyDataTemplate>
                        <table class="table table-bordered headerFixer">
                            <tr>
                                <th>Sl No</th>
                                <th>Work Order FY</th>
                                <th>WOrk Order ID</th>
                                <th>Work Order Number</th>
                                <th>Work Order Date</th>
                                <th>Work Order Qty</th>
                                <th>Action</th>
                            </tr>
                            <tr>
                                <td colspan="4">No data Found</td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <InsertItemTemplate>
                        <tr style="position: sticky; top: 36px; z-index: 4; background-color: #202648; text-align: center;">
                            <td></td>
                            <td>
                                <asp:TextBox runat="server" ValidateRequestMode="Enabled" ID="txtFY" CssClass="form-control" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtID" CssClass="form-control" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtWorkOrderNo" CssClass="form-control" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtWorkOrderDate" CssClass="form-control date" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtWorkOrderQty" CssClass="form-control allowNumber" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>
                                <asp:LinkButton ID="linkAddRow" runat="server" CommandName="AddRow" OnClick="linkAddRow_Click" CssClass="glyphicon glyphicon-plus-sign" ToolTip="New" Style="font-size: 20px;" />
                            </td>
                        </tr>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <tr style="background-color: white; text-align: center;">
                            <td>
                                <asp:Label runat="server" ID="lblSlNo" ClientIDMode="Static" Text='<%# Eval("SlNo") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblFY" ClientIDMode="Static" Text='<%# Eval("WorkOrderFY") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblID" ClientIDMode="Static" Text='<%# Eval("WorkOrderID") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblWorkOrder" ClientIDMode="Static" Text='<%# Eval("WorkOrderNumber") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblWorkOrderDate" ClientIDMode="Static" Text='<%# Eval("WorkOrderDate") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblWorkOrderQty" ClientIDMode="Static" Text='<%# Eval("WorkOrderQty") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:LinkButton ID="linkDeleteRow" runat="server" OnClick="linkDeleteRow_Click" CommandName="DeleteRow" CssClass="glyphicon glyphicon-trash "
                                    ToolTip="Delete" Style="font-size: 20px;" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <%--  <asp:GridView runat="server" ID="gvGrid" ClientIDMode="Static" CssClass="table table-bordered headerFixer" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Work Order Number">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblWorkOrder" ClientIDMode="Static" Text='<%# Eval("WorkOrderNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtWorkOrderNo" CssClass="form-control" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Work Order Date">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblWorkOrderDate" ClientIDMode="Static" Text='<%# Eval("WorkOrderDate") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtWorkOrderDate" CssClass="form-control date" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Work Order Qty">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblWorkOrderQty" ClientIDMode="Static" Text='<%# Eval("WorkOrderQty") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtWorkOrderQty" CssClass="form-control" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkDeleteRow" runat="server" OnClick="linkDeleteRow_Click" CommandName="DeleteRow" CssClass="glyphicon glyphicon-trash "
                                    ToolTip="Delete" Style="font-size: 20px;" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="linkAddRow" runat="server" CommandName="AddRow" OnClick="linkAddRow_Click" CssClass="glyphicon glyphicon-plus-sign" ToolTip="New" Style="font-size: 20px;" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>--%>
            </div>
            <div class="modal fade" id="DeleteConfirmModal" role="dialog">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content modalContent confirm-modal-content">
                        <div class="modal-header modalHeader confirm-modal-header">
                            <i class="glyphicon glyphicon glyphicon glyphicon-question-sign modal-icons"></i>
                            <br />
                            <h4 class="confirm-modal-title">Confirmation!</h4>
                            <br />
                            <span id="DeleteMsg" class="confirm-modal-msg">Are you sure you want to delete Record?</span>
                        </div>
                        <div class="modal-footer modalFooter modal-footer">
                            <asp:Button runat="server" Text="Yes" ID="btnDeleteConfirm" CssClass="confirm-modal-btn" OnClick="btnDeleteConfirm_Click" ClientIDMode="Static" />
                            <input type="button" value="No" data-dismiss="modal" class="confirm-modal-btn" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnTemplate" />
            <asp:PostBackTrigger ControlID="btnImport" />
        </Triggers>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            setControls();
        });
        function openDeleteConfirmModal(msg) {
            $("#DeleteMsg").text(msg);
            openDeleteModal('DeleteConfirmModal');
        }
        function setControls() {
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=lbFyear]').multiselect({
                includeSelectAllOption: true
            });
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            setControls();
            function openDeleteConfirmModal(msg) {
                $("#DeleteMsg").text(msg);
                openDeleteModal('DeleteConfirmModal');
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
