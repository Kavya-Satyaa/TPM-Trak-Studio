<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AMMaster_Allied.aspx.cs" Inherits="Web_TPMTrakDashboard.Allied.AMMaster_Allied" %>

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
    </style>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div>
                    <table class="tblSettings">
                        <tr>
                            <td>Machine ID</td>
                            <td>
                                <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlMachineID" OnSelectedIndexChanged="ddlMachineID_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td>Frequency</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control" OnSelectedIndexChanged="ddlFrequency_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
                                    <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem>
                                    <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Revision no</td>
                            <td>
                                <%--<asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlRevNo" AutoPostBack="true"></asp:DropDownList>--%>
                                <asp:Label runat="server" ClientIDMode="Static" CssClass="form-control" id="lblRevisionNo"></asp:Label>
                                <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnrevID" />
                            </td>
                            <td>Rev Date</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRevDate" CssClass="form-control Date"></asp:TextBox>
                            </td>
                            <td>Ref No.</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRefNo" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ClientIDMode="Static" CssClass="bajaj-btn-style btnclass" Text="View" ID="btnView" OnClick="btnView_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="btnCreateRevisionNo" Visible="true" Text="New Rev. No." CssClass="bajaj-btn-style btnclass" ClientIDMode="Static" OnClientClick="return openRevNoModal();" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnNew" CssClass="bajaj-btn-style btnclass" Text="New" OnClick="btnNew_Click" ClientIDMode="Static" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnCopy" CssClass="bajaj-btn-style btnclass" Text="Copy" OnClientClick="return openCopyModal();" ClientIDMode="Static" />
                            </td>
                            <td style="visibility:hidden">
                                <asp:FileUpload runat="server" ID="fileUploader" CssClass="form-control" />
                            </td>
                            <td style="visibility:hidden">
                                <asp:Button runat="server" ClientIDMode="Static" ID="btnImport" Text="Import" OnClick="btnImport_Click" CssClass="bajaj-btn-style btnclass" />
                            </td>
                            <td style="visibility:hidden">
                                <asp:LinkButton runat="server" ID="btnTemplate" Text="Download Sample Template" CssClass="glyphicon glyphicon-download-alt bajaj-btn-style" ForeColor="Black" Width="300px" ClientIDMode="Static" OnClick="btnTemplate_Click"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="height: 80vh; overflow: auto; margin-top: 10px;">
                    <asp:ListView runat="server" ID="lvGrid" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                        <LayoutTemplate>
                            <table class="table table-bordered headerFixer">
                                <tr>
                                    <th>Checkpoint ID</th>
                                    <th>Description</th>
                                    <th>Category ID</th>
                                    <th>Category Desc</th>
                                    <th>Action</th>
                                </tr>
                                <tr runat="server" id="itemplaceholder"></tr>
                            </table>
                        </LayoutTemplate>
                        <EmptyDataTemplate>
                            <table class="table table-bordered headerFixer">
                                <tr>
                                    <th>Checkpoint ID</th>
                                    <th>Description</th>
                                    <th>Category ID</th>
                                    <th>Category Desc</th>
                                    <th>Action</th>
                                </tr>
                                <tr style="background-color: white; text-align: center;">
                                    <td colspan="6">No Data Found</td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <ItemTemplate>
                            <tr style="background-color: white; text-align: center;">
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblCheckpointID" Text='<%# Eval("CheckpointID") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblDesc" Text='<%# Eval("CheckpointDesc") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblCategoryID" Text='<%# Eval("CategoryID") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblCategoryDesc" Text='<%# Eval("CategoryDesc") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="linkeditRow" runat="server" CommandName="EditRow" CssClass="glyphicon glyphicon-edit" ToolTip="Edit" Style="font-size: 20px;" OnClick="linkeditRow_Click" />
                                            <asp:LinkButton ID="linkDeleteRow" runat="server" CommandName="DeleteRow" CssClass="glyphicon glyphicon-trash" ToolTip="Delete" Style="font-size: 20px;" OnClick="linkDeleteRow_Click" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="linkeditRow" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="linkDeleteRow" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
                <div class="modal infoModal" id="newEditModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog" style="width: 950px; padding-top: 7%;">
                        <div class="modal-content modalThemeCss">
                            <div class="modal-header" style="background-color: #00b5ff !important;">
                                <h4 class="modal-title" id="newEditModalTitle" runat="server" style="color: black !important"></h4>
                                <i class="glyphicon glyphicon-remove modal-close-icon" onclick="storeModalDataBeforeChange('newEditModal','compare');"></i>
                                <asp:HiddenField runat="server" ID="hfNewEdit" ClientIDMode="Static" />
                            </div>
                            <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                                <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                    <table class="modal-tbl" style="width: 100%; margin: auto; color: black;">
                                        <tr>
                                            <td>Checkpoint ID</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtCheckpointID" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control allowNumber"></asp:TextBox>
                                            </td>
                                            <td>Description</td>
                                            <td>
                                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtDescription" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td>Category ID</td>
                                            <td>
                                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtCategoryID" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td>Category Desc</td>
                                            <td>
                                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtCategoryDesc" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="modal-footer" style="text-align: right !important">
                                <button type="button" data-dismiss="modal" onclick="storeModalDataBeforeChange('newEditModal','compare');" class="error-modal-btn">CANCEL</button>
                                <asp:Button runat="server" ID="btnNewEditSave" ClientIDMode="Static" Text="SAVE" CssClass="confirm-modal-btn" OnClick="btnNewEditSave_Click" />
                            </div>
                        </div>
                    </div>
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
                                <input type="button" value="No" data-dismiss="modal" class="error-modal-btn" />
                                 <asp:Button runat="server" Text="Yes" ID="btnDeleteConfirm" CssClass="confirm-modal-btn" OnClick="btnDeleteConfirm_Click" ClientIDMode="Static" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal infoModal" id="NewRevNoModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-dialog-centered " style="width: 850px">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color: #00b5ff !important;">
                                <h4 class="modal-title" style="color:black !important" id="H1" runat="server">Create Revision No.</h4>
                            </div>
                            <div class="modal-body" style="padding: 1px 15px !important">
                                <table class="innertbl" style="display: flex; justify-content: center; margin: 20px 0px;">
                                    <tr>
                                        <td>Revision No.</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtRevNoNew" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <div class="modal-footer" style="border: 0px !important; border-top: 1px solid black !important;text-align:right !important">
                                    <input type="button" value="CANCEL" data-dismiss="modal" class="error-modal-btn" />
                                    <asp:Button runat="server" ID="btnSaveRevisionNo" OnClick="btnSaveRevisionNo_Click" Text="ADD" ClientIDMode="Static" CssClass="confirm-modal-btn" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal infoModal bajaj-info-modal" id="copyModal" role="dialog" style="min-width: 500px;" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-dialog-centered" style="width: 850px;">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color:#00b5ff !important">
                                <h4 class="modal-title" style="color:black !important">Copy</h4>
                                <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal"></i>
                            </div>
                            <div class="modal-body">
                                <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                    <table style="width: 100%; margin: auto" class="modal-tbl addUpdateTbl">
                                        <tr>
                                            <td>Source MachineID</td>
                                            <td>
                                                <asp:Label runat="server" ID="lblCopySourceMAchineID" ClientIDMode="Static"></asp:Label>
                                            </td>
                                            <td>Source Frequency</td>
                                            <td>
                                                <asp:Label runat="server" ID="lblCopyFrequency" ClientIDMode="Static"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 180px;">Destination MachineID</td>
                                            <td>
                                                <asp:ListBox ID="lbCopyDestMachineID" runat="server" SelectionMode="Multiple" CssClass="form-control" ClientIDMode="Static"></asp:ListBox>
                                            </td>
                                            <td style="width: 180px;">Destination Frequency</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlCopyFrequency" CssClass="form-control" Enabled="false">
                                                    <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
                                                    <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem>
                                                    <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="modal-footer" style="text-align:right !important">
                                <input type="button" value="Close" class="error-modal-btn" data-dismiss="modal" />
                                 <asp:Button runat="server" ID="btnCopyConfirm" Text="Save" CssClass="confirm-modal-btn" OnClick="btnCopyConfirm_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                 <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnNew" EventName="Click" />
                <asp:PostBackTrigger ControlID="btnTemplate" />
                <asp:PostBackTrigger ControlID="btnImport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {
            $('.Date').datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            setControls();
        });
        function openDeleteConfirmModal() {
            openDeleteModal('DeleteConfirmModal');
        }
        function setControls() {
            $('[id$=lbCopyDestMachineID]').multiselect({
                includeSelectAllOption: true,
                buttonWidth: '250px',
            });
        }
        function openCopyModal() {
            $('#copyModal').modal('show');
            return false;
        }
        function copyValidation() {
            
        }
        function openRevNoModal() {
            $("#txtRevNoNew").val("");
            $("#NewRevNoModal").modal('show');
            return false;
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $('.Date').datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            setControls();
            function openDeleteConfirmModal() {
                openDeleteModal('DeleteConfirmModal');
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
