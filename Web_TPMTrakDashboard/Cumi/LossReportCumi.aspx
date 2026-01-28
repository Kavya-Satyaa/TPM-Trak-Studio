<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LossReportCumi.aspx.cs" Inherits="Web_TPMTrakDashboard.Cumi.LossReportCumi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
            <div class="bajaj-outer-div-filter-section">
                <div class="bajaj-inner-div-filter-section left-content-filter-section">
                    <table class="bajaj-filter-tbl">
                        <tr>
                            <td>From Date</td>
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
                        </tr>
                        <tr>
                            <td>Down Category</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCategory" CssClass="form-control" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td>Down Code</td>
                            <td>
                                <asp:ListBox ID="lbDownCode" runat="server" SelectionMode="Multiple" CssClass="listBox"></asp:ListBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click" CssClass="bajaj-btn-style" OnClientClick="return showLoader();" />
                            </td>
                            <td>

                                <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="bajaj-btn-style" Style="background-color: #9f0e9f; color: white" OnClick="btnExport_Click" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" CssClass="bajaj-btn-style" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <table class="table table-bordered table-hover headerFixer bajaj-table-style" style="margin: 0px; margin-top: 12px; width: auto; min-width: 550px;">
                <tr>
                    <th>MTTR (Hours)</th>
                    <td>
                        <asp:Label runat="server" ID="lblMTTR" ClientIDMode="Static"></asp:Label>
                    </td>
                    <th>MTBF (Hours)</th>
                    <td>
                        <asp:Label runat="server" ID="lblMTBF" ClientIDMode="Static"></asp:Label>
                    </td>
                </tr>
            </table>
            <div id="scrollMaintainDiv" style="height: 70vh; overflow: auto; margin-top: 5px; width: 100%; display: inline-block">

                <asp:ListView runat="server" ID="lvLossReport" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                    <LayoutTemplate>
                        <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblLossDetails">
                            <tr>
                                <th>Machine</th>
                                <th>Start Date</th>
                                <th>End Date</th>
                                <th>StartTime</th>
                                <th>EndTime</th>
                                <th>Total Time (min.)</th>
                                <th>PDT (min.)</th>
                                <th>Loss Type</th>
                                <th>Sub Loss</th>
                                <th>Loss ID</th>
                                <th>Operator Emop ID</th>
                                <th>Mntc Emp ID</th>
                                <th>Why-Why Document</th>
                                <th>Other Doc</th>
                                <th>Remarks</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                <asp:HiddenField runat="server" ID="hdnMachineInterfaceID" ClientIDMode="Static" Value='<%# Eval("MachineInterfaceID") %>' />
                                <asp:Label runat="server" ID="lblMachine" ClientIDMode="Static" Text='<%# Eval("Machine") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblStartDate" ClientIDMode="Static" Text='<%# Eval("StartDate") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblEndDate" ClientIDMode="Static" Text='<%# Eval("EndDate") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblStartTime" ClientIDMode="Static" Text='<%# Eval("StartTime") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblEndTime" ClientIDMode="Static" Text='<%# Eval("EndTime") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblTotalTime" ClientIDMode="Static" Text='<%# Eval("TotalTime") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label1" ClientIDMode="Static" Text='<%# Eval("PDT") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblLossType" ClientIDMode="Static" Text='<%# Eval("LossType") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSubLoss" ClientIDMode="Static" Text='<%# Eval("SubLoss") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblLossID" ClientIDMode="Static" Text='<%# Eval("LossID") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblOperatorIDWithName" ClientIDMode="Static" Text='<%# Eval("OperatorID") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblMntcEmpIDWithName" ClientIDMode="Static" Text='<%# Eval("MntcEmpID") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:FileUpload runat="server" ID="fuWhyWhy" CssClass="form-control entryControl" ClientIDMode="Static" onchange="FUChange(this,'hdnWhyWhyFileInBase64','hdnWhyWhyFileName');" />
                                <asp:LinkButton runat="server" ID="lnkWhyWhyFileName" ClientIDMode="Static" Text='<%# Eval("WhyWhyDocFileName") %>' OnClientClick="return showFileDetails(this,'whywhy');"></asp:LinkButton>
                                <asp:HiddenField ID="hdnWhyWhyFileName" runat="server" ClientIDMode="Static" Value='<%# Eval("WhyWhyDocFileName") %>' />
                                <asp:HiddenField ID="hdnWhyWhyFileExist" ClientIDMode="Static" runat="server" Value='<%# Eval("WhyWhyDocID") %>' />
                                <asp:HiddenField ID="hdnWhyWhyFileNameExist" runat="server" Value='<%# Eval("WhyWhyDocFileName") %>' />
                                <asp:HiddenField ID="hdnWhyWhyFileInBase64" runat="server" ClientIDMode="Static" />
                            </td>
                            <td>
                                <asp:FileUpload runat="server" ID="fuOtherDoc" CssClass="form-control entryControl" ClientIDMode="Static" onchange="FUChange(this,'hdnOtherDocFileInBase64','hdnOtherDocFileName');" />
                                <asp:LinkButton runat="server" ID="lnkOtherFilename" ClientIDMode="Static" Text='<%# Eval("OtherDocFileName") %>' OnClientClick="return showFileDetails(this,'other');"></asp:LinkButton>
                                <asp:HiddenField ID="hdnOtherDocFileName" runat="server" Value='<%# Eval("OtherDocFileName") %>' />
                                <asp:HiddenField ID="hdnOtherDocFileExist" runat="server" Value='<%# Eval("OtherDocID") %>' />
                                <asp:HiddenField ID="hdnOtherDocFileNameExist" runat="server" Value='<%# Eval("OtherDocFileName") %>' />
                                <asp:HiddenField ID="hdnOtherDocFileInBase64" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRemarks" ClientIDMode="Static" CssClass="form-control entryControl" Text='<%# Eval("Remarks") %>'></asp:TextBox>
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
        var bigDiv = document.getElementById('scrollMaintainDiv');

        $(document).ready(function () {
            DateTimeSetter();
            $.unblockUI({});
        });
        function showLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        $("#tblLossDetails").on("click", "td .entryControl", function () {
            $(this).closest('tr').find("#hdnUpdate").val("update");
        });
        async function FUChange(element, fileInBase64ControlId, fileNameControlId) {            debugger;            const filePathsPromises = [];            const fileName = [];            if (element.files[0] != null) {
                var fileSize = 1048576 * 1;
                if (element.files[0].size > fileSize) {
                    alert("File should be less than 1MB.");
                    console.log("File should be less than 1MB.");
                    $(element).val("");
                    return;
                }            }            for (var i = 0; i < $(element).get(0).files.length; ++i) {                filePathsPromises.push(ToBase64($(element).get(0).files[i]));                fileName.push($(element).get(0).files[i].name);            }            const filePaths = await Promise.all(filePathsPromises);            var mappedFiles = filePaths.map((base64File) => ({ file: base64File }));            let document = "", documentName = "";            debugger;            for (let i = 0; i < mappedFiles.length; i++) {                //  if (document == "") {                document = mappedFiles[i].file;                documentName = fileName[i];                //} else {                //    document += ";;;" + mappedFiles[i].file;                //    documentName += ";;;" + fileName[i];                //}            }            $(element).closest('tr').find("[id$=" + fileInBase64ControlId + "]").val(document);            $(element).closest('tr').find("[id$=" + fileNameControlId + "]").val(documentName);        }        function ToBase64(file) {            debugger;            return new Promise((resolve, reject) => {                const reader = new FileReader();                reader.readAsDataURL(file);                reader.onload = () => resolve(reader.result);                reader.onerror = error => reject(error);            });        };
        function showFileDetails(evt, param) {
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
            if (param == "whywhy") {
                fileName = $(evt).closest('tr').find('#lnkWhyWhyFileName').text();
                fileSource += "WhyWhyDocuments/" + $(evt).closest('tr').find('#hdnWhyWhyFileExist').val();
            }
            else if (param == "other") {
                fileName = $(evt).closest('tr').find('#lnkOtherFilename').text();
                fileSource += "OtherDocuments/" + $(evt).closest('tr').find('#hdnOtherDocFileExist').val();
            }
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
            $("#tblLossDetails").on("click", "td .entryControl", function () {
                $(this).closest('tr').find("#hdnUpdate").val("update");
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
