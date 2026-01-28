<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InspectionReportOfShaftMaster_Highway.aspx.cs" Inherits="Web_TPMTrakDashboard.HighWay.InspectionReportOfShaftMaster_Highway" %>

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

       
        .lblUnits{
            padding-top:5px;
        }
    </style>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <table class="tblSettings">
                                <tr>
                                    <%--<td style="color: white;">Machine ID</td>
                            <td>
                                <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlMachineID"></asp:DropDownList>
                            </td>--%>
                                    <td>Component</td>
                                    <td>
                                        <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlComponent" OnSelectedIndexChanged="ddlComponent_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                    <td>Operation</td>
                                    <td>
                                        <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlOperation" AutoPostBack="true" OnSelectedIndexChanged="ddlOperation_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td>Revision ID</td>
                                    <td>
                                        <%--<asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlRevNo" OnSelectedIndexChanged="ddlRevNo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                                        <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnRevisionID" />
                                        <asp:Label runat="server" ClientIDMode="Static" ID="lblRevisionID" CssClass="form-control" ></asp:Label>
                                    </td>
                                    <td>Rev Date</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtRevDate" CssClass="form-control Date"></asp:TextBox>
                                    </td>
                                    <td>Doc No.</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtDocNo" CssClass="form-control"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button runat="server" ClientIDMode="Static" CssClass="bajaj-btn-style btnclass" Text="View" ID="btnView" OnClientClick="return ViewClick();" OnClick="btnView_Click" />
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ID="btnCreateRevisionNo" Visible="true" Text="New Rev. ID." CssClass="bajaj-btn-style btnclass" ClientIDMode="Static" OnClientClick="return openRevNoModal();" />
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ID="btnNew" CssClass="bajaj-btn-style btnclass" Text="New" OnClick="btnNew_Click" ClientIDMode="Static" />
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ID="btnCopy" CssClass="bajaj-btn-style btnclass" Text="Copy" OnClientClick="return openCopyModal();" ClientIDMode="Static" />
                                    </td>
                                    <%-- </tr>
                            </table>
                        </div>
                         <div class="col-lg-4 col-md-4 col-sm-4" style="float: right; text-align: right;">
                            <table class="tblSettings">
                                <tr>--%>
                                    <td>
                                        <asp:FileUpload runat="server" ID="fileUploader" CssClass="form-control" />
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ClientIDMode="Static" ID="btnImport" Text="Import" OnClick="btnImport_Click" CssClass="bajaj-btn-style btnclass" />
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="btnTemplate" Text="Download Sample Template" CssClass="glyphicon glyphicon-download-alt bajaj-btn-style" ForeColor="Black" Width="300px" ClientIDMode="Static" OnClick="btnTemplate_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div style="height: 80vh; overflow: auto; margin-top: 10px;">
                    <asp:ListView runat="server" ID="lvGrid" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                        <LayoutTemplate>
                            <table class="table table-bordered headerFixer">
                                <tr>
                                    <th>Sl.No</th>
                                    <th>Characteristic ID</th>
                                    <th>Description</th>
                                    <th>Bal No</th>
                                    <th>Specification</th>
                                    <th>Inspection Method</th>
                                    <th>Data Type</th>
                                    <th colspan="2">Frequency</th>
                                    <th>Sort Order</th>
                                    <th>Action</th>
                                </tr>
                                <tr runat="server" id="itemplaceholder"></tr>
                            </table>
                        </LayoutTemplate>
                        <EmptyDataTemplate>
                            <table class="table table-bordered headerFixer">
                                <tr>
                                    <th>Sl.No</th>
                                    <th>Characteristic ID</th>
                                    <th>Description</th>
                                    <th>Bal No</th>
                                    <th>Specification</th>
                                    <th>Inspection Method</th>
                                    <th>Data type</th>
                                    <th colspan="2">Frequency</th>
                                    <th>Sort Order</th>
                                    <th>Action</th>
                                </tr>
                                <tr style="background-color: white; text-align: center;">
                                    <td colspan="10">No Data Found</td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <ItemTemplate>
                            <tr style="background-color: white; text-align: center;">
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblSlNo" Text='<%# Eval("SlNo") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblCharacteristicID" Text='<%# Eval("CharacteristicID") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblDesc" Text='<%# Eval("Description") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblBalNo" Text='<%# Eval("BalNo") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblSpec" Text='<%# Eval("Specification") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblInspectionMethod" Text='<%# Eval("InspectionMethod") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lbldataType" ClientIDMode="Static" Text='<%# Eval("DataType") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblFrequency" Text='<%# Eval("Frequency") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblQty" Text='<%# Eval("FrequencyQty") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblSortOrder" Text='<%# Eval("SortOrder") %>'></asp:Label>
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
                                            <td>Characteristic ID</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtCharacteristicID" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td>Description</td>
                                            <td>
                                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtDescription" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Bal No</td>
                                            <td>
                                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtBalNo" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td>Specification</td>
                                            <td>
                                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtSpecification" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>InspectionMethod</td>
                                            <td>
                                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtInspectionMethod" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td>Data Type</td>
                                            <td>
                                                <asp:DropDownList runat="server" ClientIDMode="Static" ID="ddlDataType" CssClass="form-control">
                                                    <asp:ListItem Text="Text" Value="Text"></asp:ListItem>
                                                    <asp:ListItem Text="Ok/NotOk" Value="Ok/NotOk"></asp:ListItem>
                                                    <asp:ListItem Text="Numeric" Value="Numeric"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Frequency</td>
                                            <td style="display: flex;text-align:center">
                                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtFrequency" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>&nbsp;&nbsp;
                                                <asp:Label runat="server" ClientIDMode="Static" Text="pcs" CssClass="lblUnits"></asp:Label>
                                            </td>
                                            <td>Frequency Qty</td>
                                            <td style="display: flex;text-align:center">
                                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtFrequencyQty" AutoCompleteType="Disabled" CssClass="form-control allowNumber"></asp:TextBox>&nbsp;&nbsp;
                                                 <asp:Label runat="server" ClientIDMode="Static" Text="/hr" CssClass="lblUnits"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Sort Order</td>
                                            <td>
                                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtSortOrder" AutoCompleteType="Disabled" CssClass="form-control allowNumber"></asp:TextBox>
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
                            <div class="modal-footer modalFooter modal-footer" style="text-align: right !important">
                                <asp:Button runat="server" Text="Yes" ID="btnDeleteConfirm" CssClass="confirm-modal-btn" OnClick="btnDeleteConfirm_Click" ClientIDMode="Static" />
                                <input type="button" value="No" data-dismiss="modal" class="error-modal-btn" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal infoModal" id="NewRevNoModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-dialog-centered " style="width: 650px">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color: #00b5ff !important;color:black !important;">
                                <h4 class="modal-title" id="H1" runat="server" style="color:black !important">Create Revision No.</h4>
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
                                    <asp:Button runat="server" ID="btnSaveRevisionNo" OnClick="btnSaveRevisionNo_Click" Text="ADD" ClientIDMode="Static" CssClass="confirm-modal-btn" />
                                    <input type="button" value="CANCEL" data-dismiss="modal" class="error-modal-btn" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal infoModal bajaj-info-modal" id="copyModal" role="dialog" style="min-width: 500px;" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-dialog-centered" style="width: 850px;">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color:#00b5ff !important;color:black !important;">
                                <h4 class="modal-title" style="color:black !important">Copy</h4>
                                <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal"></i>
                            </div>
                            <div class="modal-body">
                                <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                    <table style="width: 100%; margin: auto" class="modal-tbl addUpdateTbl">
                                        <tr>
                                            <td>Source Component</td>
                                            <td>
                                                <asp:Label runat="server" ID="lblCopySourceComponent" ClientIDMode="Static"></asp:Label>
                                            </td>
                                            <td>Source Operation</td>
                                            <td>
                                                <asp:Label runat="server" ID="lblCopyOperation" ClientIDMode="Static"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 180px;">Destination Component</td>
                                            <td>
                                                <%-- <asp:ListBox ID="lbCopyDestComponent" runat="server" SelectionMode="Multiple" CssClass="form-control" ClientIDMode="Static"></asp:ListBox>--%>
                                                <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="lbCopyDestComponent" OnSelectedIndexChanged="lbCopyDestComponent_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </td>
                                            <td style="width: 180px;">Destination Operation</td>
                                            <td>
                                                <%--<asp:ListBox ID="lbCopyOperation" runat="server" SelectionMode="Multiple" CssClass="form-control" ClientIDMode="Static"></asp:ListBox>--%>
                                                <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="lbCopyOperation"></asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="modal-footer" style="text-align: right !important">
                                <asp:Button runat="server" ID="btnCopyConfirm" Text="Save" CssClass="confirm-modal-btn" OnClick="btnCopyConfirm_Click" />
                                <input type="button" value="Close" class="error-modal-btn" data-dismiss="modal" />
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
        function ViewClick() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        function setControls() {
            //$('[id$=lbCopyOperation]').multiselect({
            //    includeSelectAllOption: true,
            //    buttonWidth: '250px',
            //});
            //$('[id$=lbCopyDestMachine]').multiselect({
            //    includeSelectAllOption: true,
            //    buttonWidth: '250px',
            //});
        }
        function openCopyModal() {
            $('#copyModal').modal('show');
            return false;
        }
        function copyValidation() {
            //if ($('#lbCopyDestMachine').val() == "" || $('#lbCopyDestMachine').val() == null) {
            //    openWarningModal_1("Please select destination machine.")
            //    return false;
            //}
            //return true;
        }
        function openRevNoModal() {
            $("#txtRevNoNew").val("");
            $("#NewRevNoModal").modal('show');
            return false;
        }
        function CloseNewEditModal() {
            $("#NewRevNoModal").modal('hide');
        }
        function openDeleteConfirmModal() {
            openDeleteModal('DeleteConfirmModal');
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
            $.unblockUI({});
            function openDeleteConfirmModal() {
                openDeleteModal('DeleteConfirmModal');
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
