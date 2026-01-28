<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MachineStartUpChecksheetMaster_Highway.aspx.cs" Inherits="Web_TPMTrakDashboard.HighWay.MachineStartUpChecksheetMaster_Highway" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
      <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
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
                /*box-shadow: 2px 2px 2px black;*/
            }

        .limitText {
            max-width: 200px;
            /*display: block;*/
            word-wrap: break-word;
        }

        .btnclass {
            box-shadow: 1px 1px 10px #515151;
            font-size: 16px;
            font-family: Verdana;
        }

        .textboxcommon {
            border: none;
            background: transparent;
        }
    </style>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <table class="tblSettings">
                                <tr>
                                    <td>Machine ID</td>
                                    <td>
                                        <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlMachineID"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ClientIDMode="Static" CssClass="bajaj-btn-style btnclass" Text="View" ID="btnView" OnClientClick="return ViewClick();" OnClick="btnView_Click" />
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ID="btnNew" CssClass="bajaj-btn-style btnclass" Text="New" OnClick="btnNew_Click" ClientIDMode="Static" />
                                        <asp:Button runat="server" ID="btnCancel" CssClass="bajaj-btn-style btnclass" Text="CANCEL" OnClick="btnCancel_Click" ClientIDMode="Static" />
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ID="btnSave" CssClass="bajaj-btn-style btnclass" Text="Save" OnClick="btnSave_Click" ClientIDMode="Static" />
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ID="btnCopy" CssClass="bajaj-btn-style btnclass" Text="Copy" OnClientClick="return openCopyModal();" ClientIDMode="Static" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6" style="float: right; text-align: right;">
                            <table class="tblSettings">
                                <tr>
                                    <td>
                                        <asp:FileUpload runat="server" ID="fileUploader" CssClass="form-control" />
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ClientIDMode="Static" ID="btnImport" Text="Import" OnClick="btnImport_Click" CssClass="bajaj-btn-style btnclass" />
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="btnTemplate" Text="Download Sample Template" CssClass="glyphicon glyphicon-download-alt bajaj-btn-style" ForeColor="Black" ClientIDMode="Static" OnClick="btnTemplate_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div style="width: 100%; height: 80vh; overflow: auto; margin-top: 10px;">
                    <asp:ListView runat="server" ClientIDMode="Static" ID="lvGrid" ItemPlaceholderID="itemplaceholder">
                        <LayoutTemplate>
                            <table class="table table-bordered headerFixer">
                                <tr>
                                    <%--<th>Characteristic ID</th>--%>
                                    <th>Characteristics</th>
                                    <th class="limitText">Points To Be Checked</th>
                                    <th>Data Type</th>
                                    <th>Sort Order</th>
                                    <th>Action</th>
                                </tr>
                                <tr runat="server" id="itemplaceholder"></tr>
                            </table>
                        </LayoutTemplate>
                        <EmptyDataTemplate>
                            <table class="table table-bordered headerFixer">
                                <tr>
                                    <%--<th>Characteristic ID</th>--%>
                                    <th>Characteristics</th>
                                    <th class="limitText">Points To Be Checked</th>
                                    <th>Data Type</th>
                                    <th>Sort Order</th>
                                    <th>Action</th>
                                </tr>
                                <tr style="background-color: white; text-align: center;">
                                    <td colspan="5">No data found</td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <ItemTemplate>
                            <tr style="background-color: white; text-align: center;">
                                <%-- <td>
                                    <asp:Label runat="server" ID="lblCharacteristicID" ClientIDMode="Static" Text='<%# Eval("CharacteristicID") %>'></asp:Label>
                                </td>--%>
                                <td>
                                    <asp:Label runat="server" ID="lblDescription" ClientIDMode="Static" Text='<%# Eval("Description") %>'></asp:Label>
                                </td>
                                <td class="limitText">
                                    <asp:Label runat="server" ID="lblPointsToBeChecked" ClientIDMode="Static" Text='<%# Eval("PointsToBeChecked") %>' CssClass="limitText"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lbldataType" ClientIDMode="Static" Text='<%# Eval("DataType") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:HiddenField runat="server" ID="hdnTextOrder" ClientIDMode="Static" Value='<%# Eval("SortOrder") %>' />
                                    <asp:TextBox runat="server" ID="txSortOrder" ClientIDMode="Static" Text='<%# Eval("SortOrder") %>' CssClass="txtupdate textboxcommon form-control"></asp:TextBox>
                                    <%-- <asp:Label runat="server" ID="lblSortOrder" ClientIDMode="Static" Text='<%# Eval("SortOrder") %>'></asp:Label>--%>
                                </td>
                                <td>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <%--  <asp:LinkButton ID="linkeditRow" runat="server" CommandName="EditRow" Text="Edit" CssClass="glyphicon glyphicon-edit" ToolTip="Edit" Style="font-size: 20px;" OnClick="linkeditRow_Click" />--%>
                                            <asp:LinkButton ID="linkDeleteRow" runat="server" CommandName="DeleteRow" CssClass="glyphicon glyphicon-trash" ToolTip="Delete" Style="font-size: 20px;" OnClick="linkDeleteRow_Click" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <%--<asp:AsyncPostBackTrigger ControlID="linkeditRow" EventName="Click" />--%>
                                            <asp:AsyncPostBackTrigger ControlID="linkDeleteRow" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
                <div class="modal infoModal" id="newEditModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog" style="min-width: 950px; padding-top: 7%;">
                        <div class="modal-content modalThemeCss">
                            <div class="modal-header" style="background-color: #00b5ff !important;">
                                <h4 class="modal-title" id="newEditModalTitle" runat="server" style="color: black !important">Add Packing</h4>
                                <i class="glyphicon glyphicon-remove modal-close-icon" onclick="storeModalDataBeforeChange('newEditModal','compare');"></i>
                                <asp:HiddenField runat="server" ID="hfNewEdit" ClientIDMode="Static" />
                            </div>
                            <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                                <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                    <table class="modal-tbl addUpdateTbl" style="width: 100%; margin: auto; color: black;">
                                        <tr>
                                            <%-- <td>Characteristic ID</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtCharacteristicID" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                                            </td>--%>
                                            <td>Description</td>
                                            <td>
                                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtDescription" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td>Points To Be Checked</td>
                                            <td>
                                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtPointsToBeChecked" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Data Type</td>
                                            <td>
                                                <asp:DropDownList runat="server" ClientIDMode="Static" ID="ddlDataType" CssClass="form-control">
                                                    <asp:ListItem Text="Text" Value="Text"></asp:ListItem>
                                                    <asp:ListItem Text="Ok/NotOk" Value="Ok/NotOk"></asp:ListItem>
                                                    <asp:ListItem Text="Numeric" Value="Numeric"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>Sort Order</td>
                                            <td>
                                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtSortorder" CssClass="form-control allowNumber" AutoCompleteType="Disabled"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="modal-footer" style="text-align: right !important;">
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
                            <div class="modal-footer modalFooter modal-footer" style="text-align: right !important">
                                <asp:Button runat="server" Text="Yes" ID="btnDeleteConfirm" CssClass="confirm-modal-btn" OnClick="btnDeleteConfirm_Click" ClientIDMode="Static" />
                                <input type="button" value="No" data-dismiss="modal" class="confirm-modal-btn" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal infoModal bajaj-info-modal" id="copyModal" role="dialog" style="min-width: 500px;" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog" style="width: 600px;">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color: #00b5ff !important;">
                                <h4 class="modal-title" style="color: black !important">Copy</h4>
                                <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal"></i>
                            </div>
                            <div class="modal-body">
                                <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                    <table style="width: 100%; margin: auto" class="modal-tbl addUpdateTbl">
                                        <tr>
                                            <td>Source Machine</td>
                                            <td>
                                                <asp:Label runat="server" ID="lblCopySourceMachine" ClientIDMode="Static"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 180px;">Destination Machine</td>
                                            <td>
                                                <asp:ListBox ID="lbCopyDestMachine" runat="server" SelectionMode="Multiple" CssClass="form-control" ClientIDMode="Static"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="modal-footer" style="text-align: right !important">
                                <asp:Button runat="server" ID="btnCopyConfirm" Text="Save" CssClass="confirm-modal-btn" OnClientClick="return copyValidation();" OnClick="btnCopyConfirm_Click" />
                                <input type="button" value="Close" class="error-modal-btn" data-dismiss="modal" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnNew" EventName="Click" />
                <asp:PostBackTrigger ControlID="btnTemplate" />
                <asp:PostBackTrigger ControlID="btnImport" />
            </Triggers>
        </asp:UpdatePanel>

    </div>
    <script>
        $(document).ready(function () {
            setControls();
        });
        function ViewClick() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        function setControls() {
            $('[id$=lbCopyDestMachine]').multiselect({
                includeSelectAllOption: true,
                buttonWidth: '250px',
            });
        }
        function openCopyModal() {
            $('#copyModal').modal('show');
            return false;
        }
        function copyValidation() {
            if ($('#lbCopyDestMachine').val() == "" || $('#lbCopyDestMachine').val() == null) {
                openWarningModal_1("Please select destination machine.")
                return false;
            }
            return true;
        }
        $("[id$=lvGrid]").on("click", ".txtupdate", function () {
            debugger;
            $("[id$=lvGrid] tr td").find('.txtupdate').removeClass("form-control");
            $("[id$=lvGrid] tr td").find('.txtupdate').addClass("textboxcommon");
            $(this).closest('td').find('input').removeClass("textboxcommon");
            $(this).closest('td').find('input').addClass("form-control");
        });

        $("[id$=btnUpdate]").click(function () {
            var count = 0;
            $("[id$=lvGrid] tr:gt(0)").each(function (src, i) {
                if (($(this, i).closest("tr").find('.txtupdate').val() == "")) {
                    count++;
                    //alert('Please enter Sub Loss Description !!');
                    //$(this, i).closest("tr").find('.txtupdate').focus();
                    return false;
                }
            });
            if (count != 0) {
                return false;
            }
        });
        function openDeleteConfirmModal() {
            openDeleteModal('DeleteConfirmModal');
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            setControls();
            $.unblockUI({});
            function openDeleteConfirmModal() {
                openDeleteModal('DeleteConfirmModal');
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
