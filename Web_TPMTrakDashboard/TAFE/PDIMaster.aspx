<%@ Page Title="PDI Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PDIMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.TAFE.PDIMaster" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>

    <style>
        #tblPDIMasterDetails tr th:nth-child(2) {
            position: sticky;
            left: 0px;
            z-index: 5;
            background-color: #2e6886;
            color: white;
        }

        #tblPDIMasterDetails tr:nth-child(odd) td:nth-child(2) {
            position: sticky;
            left: 0px;
            z-index: 2;
            background-color: #DCDCDC;
        }

        #tblPDIMasterDetails tr:nth-child(even) td:nth-child(2) {
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
                                <td>Machine</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlMachine" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click" CssClass="bajaj-btn-style" />
                                    <asp:Button runat="server" ID="btnAddPDIMasterDetails" Text="Add Details" CssClass="bajaj-btn-style bajaj-add-edit-btn-style" OnClick="btnAddPDIMasterDetails_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="scrollMaintainDiv" style="height: 80vh; overflow: auto; margin-top: 12px">
                    <asp:ListView runat="server" ID="lvPDIMasterDetails" ClientIDMode="Static">
                        <EmptyDataTemplate>
                            <table class="table table-bordered table-hover headerFixer" id="tblPDIMasterDetails">
                                <tr>
                                    <th>Part No.</th>
                                    <th>Part Description</th>
                                    <th>Part Name</th>
                                    <th>Part Type</th>
                                    <th>Operation No.</th>
                                    <th>Material</th>
                                    <th>Version</th>
                                    <th>Issued No.</th>
                                    <th>Doc No.</th>
                                    <th>Type</th>
                                    <th>Images</th>
                                    <th runat="server" id="thAction">Action</th>
                                </tr>
                                <tr>
                                    <td colspan="30" class="no-data-found-td"><span class="no-data-found">No Data Found</span></td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblPDIMasterDetails">
                                <tr>
                                    <th>Part No.</th>
                                    <th>Part Description</th>
                                    <th>Part Name</th>
                                    <th>Part Type</th>
                                    <th>Operation No.</th>
                                    <th>Material</th>
                                    <th>Version</th>
                                    <th>Issued No.</th>
                                    <th>Doc No.</th>
                                    <th>Type</th>
                                    <th>Images</th>
                                    <th runat="server" id="thAction">Action</th>
                                </tr>
                                <tr id="itemplaceholder" runat="server"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblPartNo" Text='<%# Eval("PartNo") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblPartDesc" Text='<%# Eval("PartDesc") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblPartName" Text='<%# Eval("PartName") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblPartType" Text='<%# Eval("PartType") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblOperationNo" Text='<%# Eval("OperationNo") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblMaterial" Text='<%# Eval("Material") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblVersion" Text='<%# Eval("Version") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblIssuedNo" Text='<%# Eval("IssuedNo") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblDocNo" Text='<%# Eval("DocNo") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblType" Text='<%# Eval("Type") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:LinkButton runat="server" ID="lbImage" Text='<%# Eval("ImageName") %>' CssClass="anchor-highlight-color" ClientIDMode="Static" OnClientClick="return showDrawing(this);"></asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="hfImageInBase64" ClientIDMode="Static" Value='<%# Eval("ImageInBase64") %>' />
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

                <div class="modal infoModal bajaj-info-modal" id="neweditPDIModal" role="dialog" style="min-width: 1131px;" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-dialog-centered " style="width: 1131px">
                        <div class="modal-content modalThemeCss">
                            <div class="modal-header">
                                <h4 class="modal-title" id="modalTitle" runat="server"></h4>
                                <i class="glyphicon glyphicon-remove modal-close-icon" onclick="storeModalDataBeforeChange('neweditPDIModal','compare');"></i>
                                <asp:HiddenField runat="server" ID="hfPDINewEdit" ClientIDMode="Static" />
                            </div>
                            <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                                <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                    <table style="width: 100%; margin: auto" class="modal-tbl addUpdateTbl">
                                        <tr>
                                            <td>Part No. *</td>
                                            <td>
                                                <%--    <asp:TextBox runat="server" ID="txtPartNo" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>--%>
                                                <asp:DropDownList runat="server" ID="ddlPartNo" ClientIDMode="Static" CssClass="form-control txtstyle" AutoPostBack="true" OnSelectedIndexChanged="ddlPartNo_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>Part Desc</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPartDesc" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Part Name</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPartName" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                            <td>Part Type</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPartType" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Operation No. *</td>
                                            <td>
                                                <%--  <asp:TextBox runat="server" ID="txtOperationNo" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>--%>
                                                <asp:DropDownList runat="server" ID="ddlOperationNo" ClientIDMode="Static" CssClass="form-control txtstyle">
                                                </asp:DropDownList>
                                            </td>
                                            <td>Material</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtMaterial" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Version *</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtVersion" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                            <td>Issued No.</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtIssuedNo" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Doc No.</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDocNo" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                            <td>Type</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlType" ClientIDMode="Static" CssClass="form-control txtstyle">
                                                    <asp:ListItem Text="Pinion" Value="Pinion"></asp:ListItem>
                                                    <asp:ListItem Text="Crown" Value="Crown"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Drawing</td>
                                            <td>
                                                <asp:FileUpload runat="server" ID="fuImage" ClientIDMode="Static" CssClass="form-control txtstyle" />
                                                <asp:Label runat="server" ID="lblImageName" ClientIDMode="Static"></asp:Label>
                                                <i id="clearImage" runat="server" title="Remove Image" clientidmode="static" class="glyphicon glyphicon-remove" style="color: red; display: inline-block; margin-left: 10px; vertical-align: middle;" onclick="clearUploadedImage()"></i>

                                                <asp:HiddenField runat="server" ID="hfImage" ClientIDMode="Static" />
                                                <asp:HiddenField runat="server" ID="hfImageName" ClientIDMode="Static" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" onclick="storeModalDataBeforeChange('neweditPDIModal','compare');" class="bajaj-btn-style cancel-btn">CANCEL</button>
                                <asp:Button runat="server" ID="btnPDIDetailsSave" ClientIDMode="Static" Text="Save" CssClass="bajaj-btn-style   bajaj-add-edit-btn-style" OnClientClick="return PDIValidation();" OnClick="btnPDIDetailsSave_Click" />
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

                <div class="modal infoModal bajaj-info-modal" id="showDrawingModal" role="dialog" style="min-width: 300px;">
                    <div class="modal-dialog modal-dialog-centered" style="width: 50%;">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4 class="modal-title">Image</h4>
                                <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal" onclick="closeLargeDocumentModal()"></i>
                            </div>
                            <div class="modal-body">
                                <div style="height: 70vh">
                                    <iframe id="iframeDocument" style="width: 100%; height: 100%"></iframe>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <input type="button" value="Close" class="bajaj-btn-style" data-dismiss="modal" onclick="closeLargeDocumentModal()" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {

        });
        function PDIValidation() {
            if ($('#ddlMachine').val() == "" || $('#ddlMachine').val() == null) {
                toasterWarningMsg("Machine is required.", "");
                return false;
            } else
                if ($('#ddlPartNo').val() == "" || $('#ddlPartNo').val() == null) {
                    toasterWarningMsg("Part No. is required.", "");
                    return false;
                } else if ($('#ddlOperationNo').val() == "" || $('#ddlOperationNo').val() == "") {
                    toasterWarningMsg("Operation No. is required.", "");
                    return false;
                } else if ($('#txtVersion').val() == "" || $('#txtVersion').val() == null) {
                    toasterWarningMsg("Version is required.", "");
                    return false;
                }
            var fileInput = document.getElementById('fuImage');
            if (fileInput.files[0] != undefined) {
                var reader = new FileReader();
                reader.readAsDataURL(fileInput.files[0]);
                reader.onload = function () {
                    $("#hfImage").val(reader.result);
                    $("#hfImageName").val($("#fuImage").val().split('\\').pop());
                    __doPostBack('<%= btnPDIDetailsSave.UniqueID%>', '');
                }
            } else {
                __doPostBack('<%= btnPDIDetailsSave.UniqueID%>', '');
            }
            return false;
        }
        $("#fuImage").change(function () {
            var fileExtension = ['png', 'jpeg', 'img'];

            var fileSize = 1048576 * 3;
            var fileInput = document.getElementById('fuImage');
            if (fileInput.files[0].size > fileSize) {
                toasterWarningMsg("File should be less than 3MB.", "");
                $("#fuImage").val("");
                return;
            }
            if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
                toasterWarningMsg("Only images can upload", "");
                $("#fuImage").val("");
            }
        });
        function closeLargeDocumentModal() {
            $('#iframeDocument').attr("src", "");
        }
        function showDrawing(element) {
            let name = $(element).closest('tr').find("#lbImage").text();
            let extension = name.substring(name.lastIndexOf(".") + 1);
            var arrrayBuffer = base64ToArrayBuffer($(element).closest('tr').find("#hfImageInBase64").val()); //data is the base64 encoded 
            var blob = new Blob([arrrayBuffer]);
            $('#iframeDocument').attr("src", "data:image/png;base64," + $(element).closest('tr').find("#hfImageInBase64").val());
            $("#showDrawingModal").modal('show');
            return false;
        }
        function base64ToArrayBuffer(base64) {
            var binaryString = window.atob(base64);
            var binaryLen = binaryString.length;
            var bytes = new Uint8Array(binaryLen);
            for (var i = 0; i < binaryLen; i++) {
                var ascii = binaryString.charCodeAt(i);
                bytes[i] = ascii;
            }
            return bytes;
        }
        function clearUploadedImage() {
            $("#hfImage").val("");
            $("#hfImageName").val("");
            //$('#fuDrawing').val("");
            $("#lblImageName").text("");
            $("#clearImage").css('display', 'none');
        }
        function openDeleteConfirmModal(msg) {
            $("#DeleteMsg").text(msg);
            openDeleteModal('deleteConfirmModal');
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {

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
