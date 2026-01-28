<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PartFamilyMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.PartFamilyMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>

    <style>
        #tblPartFamilyMasterDetails tr:nth-child(odd) td:nth-child(2) {
            position: sticky;
            left: 0px;
            z-index: 2;
            background-color: #DCDCDC;
        }

        #tblPartFamilyMasterDetails tr:nth-child(even) td:nth-child(2) {
            position: sticky;
            left: 0px;
            z-index: 2;
            background-color: #FFFFFF;
        }
     </style>
    <div class="content-div">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hdnModalValues" ClientIDMode="Static" />
                <div class="bajaj-outer-div-filter-section">
                    <div class="bajaj-inner-div-filter-section left-content-filter-section">
                        <table class="bajaj-filter-tbl">
                            <tr>
                                <td>Part Family</td>
                                <td>
                                   <asp:TextBox ID="txtPartFamilysSearch" runat="server" Style="float: left" AutoPostBack="True" CssClass=" form-control" OnTextChanged="txtPartFamilysSearch_TextChanged" placeholder="search Part Family here ......" ></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnAdd" Text="Add Details" CssClass="btn btn-info" OnClick="btnAdd_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="scrollMaintainDiv" style="height: 80vh; overflow: auto; margin-top: 12px;width:60%">
                    <asp:ListView runat="server" ID="lvPartFamilyMasterDetails" ClientIDMode="Static">
                        <EmptyDataTemplate>
                            <table class="table table-bordered table-hover headerFixer" id="tblPartFamilyMasterDetails">
                                <tr>
                                    <th>PartFamily ID</th>
                                    <th>PartFamily Description</th>
                                    <th runat="server" id="thAction">Action</th>
                                </tr>
                                <tr>
                                    <td colspan="30" class="no-data-found-td"><span class="no-data-found">No Data Found</span></td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblPartFamilyMasterDetails">
                                <tr>
                                    <th>PartFamily ID</th>
                                    <th>PartFamily Description</th>
                                    <th style="width:208px" runat="server" id="thAction">Action</th>
                                </tr>
                                <tr id="itemplaceholder" runat="server"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblPartFamID" Text='<%# Eval("PartFamily") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblPartFamDesc" Text='<%# Eval("Description") %>'></asp:Label>
                                </td>
                                <td style="white-space: nowrap">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:LinkButton runat="server" ID="lbEdit" CssClass=" bajaj-action-icons bajaj-add-edit-btn-style" ToolTip="Edit" OnClick="lbEdit_Click">
                                            <i class="glyphicon glyphicon-pencil"></i>
                                            <span>EDIT</span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="lbDelete" CssClass="bajaj-action-icons bajaj-delete-icons" ToolTip="Delete" OnClick="lbDelete_Click">
                                            <i class="glyphicon glyphicon-trash "></i>
                                           <span>DELETE</span> 
                                            </asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="lbEdit" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="lbDelete" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>

                
                <div class="modal infoModal bajaj-info-modal" id="neweditPartFamModal" role="dialog" style="min-width: 1131px;" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-dialog-centered " style="width: 1131px">
                        <div class="modal-content modalThemeCss">
                            <div class="modal-header">
                                <h4 class="modal-title" id="modalTitle" runat="server"></h4>
                                <i class="glyphicon glyphicon-remove modal-close-icon" onclick="storeModalDataBeforeChange('neweditPartFamModal','compare');"></i>
                                <asp:HiddenField runat="server" ID="hfPartFamNewEdit" ClientIDMode="Static" />
                            </div>
                            <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                                <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                    <table style="width: 100%; margin: auto" class="modal-tbl addUpdateTbl">
                                        <tr>
                                            <td>Part Family ID*</td>
                                            <td>
                                                 <asp:TextBox runat="server" ID="txtPartFamID" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                            <td>Part Family Desc</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPartDesc" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" onclick="storeModalDataBeforeChange('neweditPartFamModal','compare');" class="bajaj-btn-style cancel-btn">CANCEL</button>
                                <asp:Button runat="server" ID="btnPartFamDetailsSave" ClientIDMode="Static" Text="Save" CssClass="bajaj-btn-style   bajaj-add-edit-btn-style" OnClientClick="return PartFamilyValidation();" OnClick="btnPartFamDetailsSave_Click" />
                            </div>
                        </div>
                    </div>
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
                            <div class="modal-footer modalFooter modal-footer">
                                <asp:Button runat="server" Text="Yes" ID="btnDeleteConfirm" CssClass="confirm-modal-btn" OnClick="btnDeleteConfirm_Click" ClientIDMode="Static" />
                                <input type="button" value="No" data-dismiss="modal" class="confirm-modal-btn" />
                            </div>
                        </div>
                    </div>
                </div>
                </ContentTemplate>
         </asp:UpdatePanel>
     </div>
    <script>
        function PartFamilyValidation() {
            if ($('#txtPartFamID').val() == "" || $('#txtPartFamID').val() == null) {
                    toasterWarningMsg("Part Family ID is required.", "");
                    return false;
                }
        }
        function openDeleteConfirmModal(msg) {
            $("#DeleteMsg").text(msg);
            openDeleteModal('deleteConfirmModal');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
