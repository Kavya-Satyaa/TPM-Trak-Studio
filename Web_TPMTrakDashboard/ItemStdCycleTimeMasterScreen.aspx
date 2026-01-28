<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ItemStdCycleTimeMasterScreen.aspx.cs" Inherits="Web_TPMTrakDashboard.ItemStdCycleTimeMasterScreen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>

    <div class="content-div">
        <asp:UpdatePanel runat="server">
            <triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
            </triggers>
            <contenttemplate>
                <div class="bajaj-outer-div-filter-section">
                    <div class="bajaj-inner-div-filter-section left-content-filter-section">
                        <table class="bajaj-filter-tbl">
                            <tr>
                                <td>From Date

                                <%--</td>
                                <td>--%>
                                    <asp:TextBox runat="server" ID="txtFromDate" CssClass="form-control" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td>To Date

                                <%--</td>
                                <td>--%>
                                    <asp:TextBox runat="server" ID="txtToDate" CssClass="form-control" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td>Machine Id

                              <%--  </td>
                                <td>--%>
                                    <asp:DropDownList runat="server" ID="ddlMachine" CssClass="form-control"></asp:DropDownList>
                                </td>

                                <td>Updated By

                              <%--  </td>
                                <td>--%>
                                    <asp:DropDownList runat="server" ID="ddlUpdatedBy" CssClass="form-control"></asp:DropDownList>
                                </td>
                                <td>Item Code

                               <%-- </td>
                                <td>--%>
                                    <asp:TextBox runat="server" ID="txtItemCode" CssClass="form-control" AutoCompleteType="Disabled" ClientIDMode="Static" placeholder="Type Item Code"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click" CssClass="bajaj-btn-style" />
                                    <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="bajaj-btn-style bajaj-add-edit-btn-style" OnClick="btnSave_Click" />
                                    <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="bajaj-btn-style" style="background-color: #9f0e9f; color: white" OnClick="btnExport_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="scrollMaintainDiv" style="height: 76vh; overflow: auto; margin-top: 12px">
                    <asp:ListView runat="server" ID="lvDetails" ClientIDMode="Static">
                        <emptydatatemplate>
                            <table class="table table-bordered  headerFixer" id="tblDetails">
                                <tr>
                                    <th>Item Code</th>
                                    <th>Item Description</th>
                                    <th>Updated By</th>
                                    <th>Updated TS</th>
                                    <th>Std. Cycle Time</th>
                                    <th>Std. Machining Time</th>
                                    <th>Std. Load Unload Time</th>
                                    <th>Machine Id</th>
                                    <th runat="server" id="thAction">Action</th>
                                </tr>
                                <tr>
                                    <td colspan="30" class="no-data-found-td"><span class="no-data-found">No Data Found</span></td>
                                </tr>
                            </table>
                        </emptydatatemplate>
                        <layouttemplate>
                            <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblJHMasterDetails">
                                <tr>
                                    <th>Item Code</th>
                                    <th>Item Description</th>
                                    <th>Updated By</th>
                                    <th>Updated TS</th>
                                        <th>Std. Cycle Time</th>
                                    <th>Std. Machining Time</th>
                                    <th>Std. Load Unload Time</th>
                                    <th>Machine Id</th>
                                    <th runat="server" id="thAction">Action</th>
                                </tr>
                                <tr id="itemplaceholder" runat="server"></tr>
                            </table>
                        </layouttemplate>
                        <itemtemplate>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblItemCode" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hfOperationNo" Value='<%# Eval("OperationNo") %>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblItemDescription" Text='<%# Eval("ItemDescription") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label1" Text='<%# Eval("UpdatedBy") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label2" Text='<%# Eval("UpdatedTS") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label3" Text='<%# Eval("StdCycleTime") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtStdMachiningTime" Text='<%# Eval("StdMachiningTime") %>' AutoCompleteType="Disabled" CssClass="form-control allowDecimal"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hfStdMachiningTime" Value='<%# Eval("StdMachiningTime") %>' />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtStdLoadUnloadTime" Text='<%# Eval("StdLoadUnloadTime") %>' AutoCompleteType="Disabled" CssClass="form-control allowDecimal"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hfStdLoadUnloadTime" Value='<%# Eval("StdLoadUnloadTime") %>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblMachineId" Text='<%# Eval("MachineId") %>'></asp:Label>
                                </td>
                                <td style="white-space: nowrap">
                                    <asp:UpdatePanel runat="server">
                                        <contenttemplate>
                                            <asp:LinkButton runat="server" ID="lbDelete" CssClass="bajaj-action-icons bajaj-delete-icons" ToolTip="Delete" OnClick="lbDelete_Click">
                                                <i class="glyphicon glyphicon-trash "></i>
                                                <span>DELETE</span>
                                            </asp:LinkButton>
                                        </contenttemplate>
                                        <triggers>
                                            <asp:AsyncPostBackTrigger ControlID="lbDelete" EventName="Click" />
                                        </triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </itemtemplate>
                    </asp:ListView>
                </div>


                <div class="modal fade" id="deleteConfirmModal" role="dialog">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content modalContent confirm-modal-content">
                            <div class="modal-header modalHeader confirm-modal-header">
                                <i class="glyphicon glyphicon glyphicon glyphicon-question-sign modal-icons"></i>
                                <br />
                                <h4 class="confirm-modal-title">Confirmation!</h4>
                                <br />
                                <span id="DeleteMsg" class="confirm-modal-msg">Are you sure you want to delete Record?</span>
                            </div>
                            <div class="modal-footer modalFooter modal-footer" style="margin-top: 0px">
                                <asp:Button runat="server" Text="Yes" ID="btnDeleteConfirm" CssClass="confirm-modal-btn" OnClick="btnDeleteConfirm_Click" ClientIDMode="Static" />
                                <input type="button" value="No" data-dismiss="modal" class="confirm-modal-btn" />
                            </div>
                        </div>
                    </div>
                </div>
            </contenttemplate>
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
        function openDeleteConfirmModal(msg) {
            $("#DeleteMsg").text(msg);
            openDeleteModal('deleteConfirmModal');
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                DateTimeSetter();
            });
            function openDeleteConfirmModal(msg) {
                $("#DeleteMsg").text(msg);
                openDeleteModal('deleteConfirmModal');
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
