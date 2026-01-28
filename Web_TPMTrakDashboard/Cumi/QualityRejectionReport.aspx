<%@ Page Title="QualityRejectionReport" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QualityRejectionReport.aspx.cs" Inherits="Web_TPMTrakDashboard.Cumi.QualityRejectionReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <style>
        .multiselect-container {
            height: 35vh;
            overflow: auto;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
            <div class="bajaj-outer-div-filter-section">
                <div class="bajaj-inner-div-filter-section left-content-filter-section">
                    <table class="bajaj-filter-tbl" style="box-shadow: 0 4px 8px 0 rgb(0 0 0 / 25%), 0 6px 20px 0 rgb(0 0 0 / 19%);">
                        <tr>
                            <td style="width: 110px;">From Date</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFromDate" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>To Date</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtToDate" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>Plant</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td>Machine</td>
                            <td>
                                <asp:ListBox ID="lbMachine" runat="server" SelectionMode="Multiple" CssClass="listBox"></asp:ListBox>
                            </td>
                            <td>Operator</td>
                            <td>
                                <asp:ListBox ID="lbEmployee" runat="server" CssClass="listBox" SelectionMode="Multiple" AutoPostBack="true"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Rejection Category</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCategory" CssClass="form-control" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td>Rejection Code</td>
                            <td>
                                <asp:ListBox ID="lbRejectionCode" runat="server" SelectionMode="Multiple" CssClass="listBox"></asp:ListBox>
                            </td>
                            <td colspan="6">
                                <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click" CssClass="bajaj-btn-style" OnClientClick="return showLoader();" />
                                <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="bajaj-btn-style" Style="background-color: #9f0e9f; color: white; margin-left: 10px; margin-right: 10px;" OnClick="btnExport_Click" />
                                <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" CssClass="bajaj-btn-style" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="scrollMaintainDiv" style="height: 75vh; overflow: auto; margin-top: 5px; width: 100%; display: inline-block">

                <asp:ListView runat="server" ID="lvQualityReport" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                    <EmptyDataTemplate>
                        <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblQualityReportDetails">
                            <tr>
                                <th>Machine</th>
                                <th>Date Time</th>
                                <th>PO Number</th>
                                <th>Item Code</th>
                                <th>Item Description</th>
                                <th>Operator</th>
                                <th>Rejection Reason</th>
                                <th>Rejection Qty</th>
                                <th>Total Weight</th>
                                <th>Document</th>
                            </tr>
                            <tr>
                                <td colspan="100" class="no-data-found-td"><span class="no-data-found" style="color: black">No Data Found</span></td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblQualityReportDetails">
                            <tr>
                                <th>Machine</th>
                                <th>Date Time</th>
                                <th>PO Number</th>
                                <th>Item Code</th>
                                <th>Item Description</th>
                                <th>Operator</th>
                                <th>Rejection Reason</th>
                                <th>Rejection Qty</th>
                                <th>Total Weight</th>
                                <th>Document</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                <asp:Label runat="server" ID="lblMachine" ClientIDMode="Static" Text='<%# Eval("MachineId") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblDate" ClientIDMode="Static" Text='<%# Eval("Date") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblPONumber" ClientIDMode="Static" Text='<%# Eval("PONumber") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblItemCode" ClientIDMode="Static" Text='<%# Eval("ItemCode") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblItemDesc" ClientIDMode="Static" Text='<%# Eval("ItemDesc") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblOperator" ClientIDMode="Static" Text='<%# Eval("Operator") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblRejReason" ClientIDMode="Static" Text='<%# Eval("RejReason") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblRejQty" ClientIDMode="Static" Text='<%# Eval("RejQty") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblTotalWeight" ClientIDMode="Static" Text='<%# Eval("TotalWeight") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:FileUpload runat="server" ID="fuWhyWhy" CssClass="form-control entryControl" ClientIDMode="Static" onchange="FUChange(this,'hdnDocFileInBase64','hdnDocFileName');" />
                                <asp:LinkButton runat="server" ID="lnkDocFileName" ClientIDMode="Static" Text='<%# Eval("Document") %>' OnClientClick="return showFileDetails(this);"></asp:LinkButton>
                                <asp:HiddenField ID="hdnDocFileName" runat="server" ClientIDMode="Static" Value='<%# Eval("Document") %>' />
                                <asp:HiddenField ID="hdnDocFileExist" ClientIDMode="Static" runat="server" Value='<%# Eval("DocumentID") %>' />
                                <asp:HiddenField ID="hdnDocFileNameExist" runat="server" Value='<%# Eval("Document") %>' />
                                <asp:HiddenField ID="hdnDocFileInBase64" runat="server" ClientIDMode="Static" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <div class="modal infoModal" id="showDocumentModal" role="dialog" style="min-width: 300px;">
                <div class="modal-dialog modal-dialog-centered" style="width: 90%;">
                    <div class="modal-content">
                        <div class="modal-header">

                            <h4 class="modal-title">Document</h4>
                            <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal" onclick="closeLargeDocumentModal()"></i>
                        </div>
                        <div class="modal-body">
                            <div style="height: 70vh">
                                <iframe id="docIframe" style="width: 100%; height: 100%"></iframe>
                                <video id="docVideo" autoplay='autoplay' controls style="width: 100%; height: 100%; display: none">
                                    <source />
                                </video>
                                <img id="docImage" src="" style="width: 100%; height: 100%; display: none" />
                            </div>

                        </div>
                        <div class="modal-footer">

                            <input type="button" value="Close" class="Btns" data-dismiss="modal" onclick="closeLargeDocumentModal()" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            DateTimeSetter();
            $.unblockUI({});
        });
        function showLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        $("#tblQualityReportDetails").on("click", "td .entryControl", function () {
            $(this).closest('tr').find("#hdnUpdate").val("update");
        });

        function showFileDetails(evt) {
            debugger;
            $('#docIframe').css("display", "none");
            $('#docVideo').css("display", "none");
            $('#docImage').css("display", "none");
            $('#docVideo source').attr('src', '');
            $("#docVideo")[0].load();
            $('#docImage').attr('src', '');
            $('#docIframe').attr("src", "");
            src = "../Images/logo/AMIT_logo.png"
            var fileSource = "../CumiDocuments/";
            var fileName = "";
            fileName = $(evt).closest('tr').find('#lnkDocFileName').text();
            fileSource += "QualityRejectionDocument/" + $(evt).closest('tr').find('#hdnDocFileExist').val();
            $('#docIframe').css("display", "block");
            $('#docIframe').attr("src", fileSource);
            var extension = fileName.split('.')[1];
            debugger;
            if (extension == "xlsx" || extension == "xls" || extension == "xlx") {
                $('#showDocumentModal').modal("hide");
            }
            else {
                $('#showDocumentModal').modal("show");
            }
            return false;
        }

        function closeLargeDocumentModal() {
            $('#showDocumentModal').modal("hide");
        }

        async function FUChange(element, fileInBase64ControlId, fileNameControlId) {            debugger;            const filePathsPromises = [];            const fileName = [];            if (element.files[0] != null) {
                var fileSize = 1048576 * 1;
                if (element.files[0].size > fileSize) {
                    alert("File should be less than 1MB.");
                    console.log("File should be less than 1MB.");
                    $(element).val("");
                    return;
                }            }            for (var i = 0; i < $(element).get(0).files.length; ++i) {                filePathsPromises.push(ToBase64($(element).get(0).files[i]));                fileName.push($(element).get(0).files[i].name);            }            const filePaths = await Promise.all(filePathsPromises);            var mappedFiles = filePaths.map((base64File) => ({ file: base64File }));            let document = "", documentName = "";            debugger;            for (let i = 0; i < mappedFiles.length; i++) {                document = mappedFiles[i].file;                documentName = fileName[i];            }            $(element).closest('tr').find("[id$=" + fileInBase64ControlId + "]").val(document);            $(element).closest('tr').find("[id$=" + fileNameControlId + "]").val(documentName);        }
        function ToBase64(file) {            debugger;            return new Promise((resolve, reject) => {                const reader = new FileReader();                reader.readAsDataURL(file);                reader.onload = () => resolve(reader.result);                reader.onerror = error => reject(error);            });        };

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
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            var bigDiv = document.getElementById('scrollMaintainDiv');
            $(document).ready(function () {
                DateTimeSetter();
                $.unblockUI({});
            });
            $("#tblQualityReportDetails").on("click", "td .entryControl", function () {
                $(this).closest('tr').find("#hdnUpdate").val("update");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
