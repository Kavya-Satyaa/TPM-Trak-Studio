<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PMActivityTransactionData.aspx.cs" Inherits="Web_TPMTrakDashboard.PMActivityTransactionData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <style>
        .table-style tr td {
            color: white;
            padding: 4px;
            white-space: nowrap;
        }

        .table-style tr th {
            background-color: #5391CA;
            color: white;
            font-weight: bold;
            padding: 5px;
            white-space: nowrap;
        }

        .lbls {
            padding: 2px 10px;
            color: white !important;
        }

        .headerFixerhere tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #5391CA;
            color: white;
        }

        .headerFixerhere tr:nth-child(2n+1) td:nth-child(2), .headerFixerhere tr:nth-child(2n) td:nth-child(2) {
            position: sticky;
            left: 0px;
            z-index: 1;
            background-color: #202648;
        }

        .headerFixerhere tr:first-child th:nth-child(2) {
            position: sticky;
            left: 0px;
            z-index: 6;
            background-color: #5391CA;
        }

        .headerFixer tr td {
            background-color: #FFFFFF;
        }

        #lblPMStatus {
            font-size: 16px;
        }

        .table-style tr td:nth-of-type(n+5) {
            text-align: center;
        }

        .fileUploader {
            width: 90px;
        }

        .tblExtraFields td {
            padding: 5px;
        }

        .hiddenlbl {
            display: none;
        }

        .date {
            width: 120px !important;
        }

        #gvPMTransactionDetails tr:last-child td {
            background-color: #ffffab;
        }

        #gvPMTransactionDetails tr td:nth-child(n+8) {
            cursor: pointer;
        }

        .lnkFileName {
            max-width: 90px;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            display: block;
        }
    </style>
    <div>
        <asp:HiddenField runat="server" ID="hdnLoginDisplayStatus" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnLoggedInUserID" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnCurrentDateTime" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnCurrentPrevNextParam" ClientIDMode="Static" />
        <div>
            <table style="display: inline-block;" class="table-style">
                <tr>
                    <td style="text-align: center; vertical-align: central; align-content: center;">Machine ID:
                    </td>
                    <td style="width: 150px">
                        <asp:DropDownList runat="server" ID="ddlMachineId" CssClass="form-control" Width="140px" ClientIDMode="Static" />
                    </td>
                    <td style="text-align: center; vertical-align: central; align-content: center;">Frequency:
                    </td>
                    <td style="width: 150px">
                        <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control" Width="140px" ClientIDMode="Static" onchange="frequencyChange();" />
                    </td>
                    <td style="text-align: center; vertical-align: central; align-content: center;" class="tdYear">Year:
                    </td>
                    <td class="tdYear">
                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" placeholder="Year" Style="width: 70px; display: inline;"></asp:TextBox>
                    </td>
                    <td style="text-align: center; vertical-align: central; align-content: center;" class="tdDate">From Date:
                    </td>
                    <td class="tdDate">
                        <div class="input-group">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" MaxLength="15" AutoCompleteType="Disabled" AutoPostBack="false"></asp:TextBox>
                        </div>

                    </td>
                    <td style="text-align: center; vertical-align: central; align-content: center;" class="tdDate">To Date:
                    </td>
                    <td class="tdDate">
                        <div class="input-group">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" MaxLength="15" AutoCompleteType="Disabled" AutoPostBack="false"></asp:TextBox>
                        </div>

                    </td>
                    <td style="text-align: center; vertical-align: central; align-content: center;">Category:
                    </td>
                    <td style="width: 150px">
                        <asp:DropDownList runat="server" ID="ddlCategory" ClientIDMode="Static" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" OnClick="btnView_Click" CssClass="btn btn-info" Text="View" Width="80" />
                    </td>

                </tr>
                <tr>
                    <td colspan="5">
                        <div style="display: inline-block; margin-right: 7px; margin-top: 10px;">
                            <label style="background-color: red" class="lbls">Activity Pending</label>
                            <label style="background-color: #30ce30;" class="lbls">Completed</label>
                            <label style="background-color: #ffa302;" class="lbls">Under Plan</label>
                        </div>
                        &nbsp;<asp:LinkButton runat="server" ID="lnkLogin" ClientIDMode="Static" CssClass="glyphicon glyphicon-log-in" Style="color: white; font-size: 20px;" OnClientClick="return loginClick();" Text="-LogIn"></asp:LinkButton>
                    </td>
                </tr>
            </table>

        </div>
        <div style="height: 75vh; width: 100%; overflow: auto">
            <asp:GridView ID="gvPMTransactionDetails" runat="server" AutoGenerateColumns="false" Width="100%" EmptyDataText="No Data Found." ShowHeaderWhenEmpty="true" ShowHeader="true" ShowFooter="true" ClientIDMode="Static" CssClass="table table-bordered headerFixer" OnRowDataBound="gvPMTransactionDetails_RowDataBound">
            </asp:GridView>
        </div>
        <div style="margin-top: 5px;">
            <div style="width: 80%; display: inline-block; text-align: center; display: none;">
                <asp:Button runat="server" ID="btnPrev" OnClick="btnPrev_Click" CssClass="btn btn-info" Text="Previous" Width="80" />
                <asp:Button runat="server" ID="btnNext" OnClick="btnNext_Click" CssClass="btn btn-info" Text="Next" Width="80" />
            </div>
            <div style="display: inline-block; float: right; margin-right: 10%">
                <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" OnClientClick="return btnActivityTransactionSaveClick();" CssClass="btn btn-info" Text="Save" Width="80" />
                <asp:Button runat="server" ID="btnActivityTransactionExport" ClientIDMode="Static" Text="Export" CssClass="btn btn-info" OnClientClick="return ExportSelection();" />
            </div>
        </div>
    </div>

    <div class="modal infoModal" id="loginConfirmationModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered " style="width: 22%">
            <div class="modal-content modalThemeCss">
                <div class="modal-header">
                    <h4 class="modal-title">Credentials Required</h4>
                    <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal" style="float: right;"></i>
                    <%--position: relative;top: -22px;--%>
                </div>
                <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                    <span style="color: red; margin-left: 20px;" class="mandatory-message"></span>
                    <div style="padding-left: 15px; padding-right: 15px; padding-bottom: 8px;">
                        <table style="width: 100%; margin: auto" id="tblUserLoginInfo">
                            <tr>
                                <td style="width: 160px">User ID *</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtUserid" CssClass="form-control" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top: 32px; width: 160px">Password *</td>
                                <td style="padding-top: 32px">
                                    <asp:TextBox runat="server" ID="txtPassword" CssClass="form-control" TextMode="Password" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                        </table>

                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnLoginConfirmation" ClientIDMode="Static" Text="OK" CssClass="btn btn-info" OnClientClick="return btnLoginConfirmationClick();" />
                    <button type="button" data-dismiss="modal" class="btn btn-info">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal infoModal" id="extraFieldEntryModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered " style="width: 35%">
            <div class="modal-content modalThemeCss">
                <div class="modal-header">
                    <h4 class="modal-title">Please enter</h4>
                    <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal" style="float: right;"></i>
                    <%--position: relative; top: -22px;--%>
                </div>
                <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                    <span style="color: red; margin-left: 20px;" class="mandatory-message"></span>
                    <div style="padding-left: 15px; padding-right: 15px; padding-bottom: 8px;">
                        <table style="width: 100%; margin: auto" class="tblExtraFields">
                            <tr>
                                <td>Status</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlStatus" ClientIDMode="Static" CssClass="form-control">
                                        <asp:ListItem Text="Checked" Value="Checked"></asp:ListItem>
                                        <asp:ListItem Text="Not Checked" Value="Not Checked"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Action</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlAction" ClientIDMode="Static" CssClass="form-control">
                                        <asp:ListItem Text="Corrected" Value="Corrected"></asp:ListItem>
                                        <asp:ListItem Text="Not Corrected" Value="Not Corrected"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Remarks</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtRemarks" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnExtraFieldEntrySave" ClientIDMode="Static" Text="OK" CssClass="btn btn-info" OnClientClick="return btnExtraFieldEntrySaveClick();" />
                    <button type="button" data-dismiss="modal" class="btn btn-info">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal infoModal" id="exportMenuOptionModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered " style="width: 35%">
            <div class="modal-content modalThemeCss">
                <div class="modal-header">
                    <h4 class="modal-title">Export Menu</h4>
                    <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal" style="float: right;"></i>
                    <%--position: relative; top: -22px;--%>
                </div>
                <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                    <span style="color: red; margin-left: 20px;" class="mandatory-message"></span>
                    <div style="padding-left: 15px; padding-right: 15px; padding-bottom: 8px;">
                        <table style="width: 100%; margin: auto">
                            <tr>
                                <td>From Date</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtFromDateExport" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle" Style="position: relative" placeholder="DD-MM-YYYY"></asp:TextBox>
                                </td>
                                <td>To Date</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtToDateExport" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle" Style="position: relative" placeholder="DD-MM-YYYY"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top: 32px">Frequency</td>
                                <td style="padding-top: 32px">
                                    <asp:CheckBoxList runat="server" ID="cbFrequency" CssClass="checkbox-list" ClientIDMode="Static" RepeatDirection="Vertical" RepeatColumns="2" Style="width: 100%"></asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnExportConfirm" ClientIDMode="Static" Text="Export" CssClass="btn btn-info" OnClientClick="return exportMenuDataValidation();" OnClick="btnExportConfirm_Click" />
                    <button type="button" data-dismiss="modal" class="btn btn-info">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal infoModal" id="FilesModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog modal-dialog-centered" style="width: 90%;">
            <div class="modal-content">
                <div class="modal-header">

                    <h4 class="modal-title">Document</h4>
                    <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal" style="float: right"></i>
                    <%--position: relative; top: -22px;--%>
                </div>
                <div class="modal-body">
                    <div style="height: 70vh">
                        <iframe id="iframeDocument" style="width: 100%; height: 100%"></iframe>
                    </div>

                </div>
                <div class="modal-footer">

                    <input type="button" value="Close" class="btn btn-info" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <div class="modal infoModal" id="showActivityFile" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog modal-dialog-centered" style="width: 90%;">
            <div class="modal-content">
                <div class="modal-header">

                    <h4 class="modal-title" style="color: white; font-size: 25px">Document</h4>
                    <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal"></i>
                </div>
                <div class="modal-body">
                    <div style="height: 75vh">
                        <iframe id="iframeActFile" style="width: 100%; height: 100%"></iframe>
                        <video id="videoActFile" autoplay='autoplay' controls style="width: 100%; height: 100%">
                            <source />
                        </video>
                        <img id="imgActFile" src="" style="width: 100%; height: 100%" />
                    </div>

                </div>
                <div class="modal-footer">

                    <input type="button" value="Close" class="Btns" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <script>
        var SelectedRowIndex = 0, SelectedColIndex = 0;
        $(document).ready(function () {
            displayLoginBtn();
            if ($('#ddlFrequency').val() != "Shift") {
                $('#gvPMTransactionDetails td:nth-child(7)').hide();
                $('#gvPMTransactionDetails th:nth-child(7)').hide();
            }
            if ($('#ddlFrequency').val() == "3 Month" || $('#ddlFrequency').val() == "6 Month") {
                $('#gvPMTransactionDetails tr:last-child').show();
            }
            else {
                $('#gvPMTransactionDetails tr:last-child').hide();
            }
            freezeColumnFromLeft('gvPMTransactionDetails', 7);
            frequencyChange();
        });
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
        function loginClick() {
            $(".mandatory-message").text("");
            $("#txtUserid").val("");
            $("#txtPassword").val("");
            $("#loginConfirmationModal").modal('show');
            $("#txtUserid").focus();
            return false;
        }
        function displayLoginBtn() {
            if ($('#hdnLoginDisplayStatus').val() == "show") {
                $('#lnkLogin').show();
            }
            else {
                $('#lnkLogin').hide();
            }
        }
        function frequencyChange() {
            $('.tdYear').hide();
            $('.tdDate').hide();
            if ($('#ddlFrequency').val() == "3 Month" || $('#ddlFrequency').val() == "6 Month") {
                $('.tdYear').show();
            }
            else {
                $('.tdDate').show();
            }
        }
        $("#gvPMTransactionDetails tr td").click(function () {
            if ($('#hdnLoginDisplayStatus').val().toLowerCase() == "show") {
                toasterWarningMsg('Please login', '');
                return;
            }
            let tdValue = $(this).find('#lblPMStatus').text();
            SelectedColIndex = $(this).index();
            SelectedRowIndex = $(this).closest('tr').index();
            if (SelectedColIndex == 1) {
                let hasFile = $(this).find("#hfHasFile").val();
                if (hasFile == "True") {
                    let activityid = $(this).find('#hdnActivityID').val();
                    let machineid = $("#ddlMachineId").val();
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: '<%= ResolveUrl("PMActivityTransactionData.aspx/getFile") %>',
                        contentType: "application/json; charset=utf-8",
                        crossDomain: true,
                        data: '{activityid: "' + activityid + '",machineid:"' + machineid + '"}',
                        dataType: "json",
                        success: function (response) {
                            let pdfUrl = response.d;
                            $("#iframeDocument").attr("src", pdfUrl);
                            $("#FilesModal").modal('show');
                        },
                        error: function (Result) {
                            alert("Error" + Result);
                        }
                    });
                }
            }

            if ($('#hdnLoginDisplayStatus').val() == "hide") {
                if (tdValue.trim() == "P") {
                    if ($('#ddlFrequency').val() == "3 Month" || $('#ddlFrequency').val() == "6 Month") {
                        $('#ddlStatus').val("Checked");
                        $('#ddlAction').val("Corrected");
                        $('#txtRemarks').val("");
                        $('#btnExtraFieldEntrySave').show();
                        $('#extraFieldEntryModal').modal('show');
                    }
                    else {
                        setStatusPendingToComplete();
                    }
                }
            }
            if ($('#ddlFrequency').val() == "3 Month" || $('#ddlFrequency').val() == "6 Month") {
                if (tdValue.trim() == "C" && $(this).find('.lblEntryStatus').text() == "FieldsEntered") {
                    setFieldsEntryValueToControls(true);
                }
                else if (tdValue.trim() == "C" && $(this).find('.lblEntryStatus').text() == "") {
                    setFieldsEntryValueToControls(false);
                }
            }
        });
        function btnExtraFieldEntrySaveClick() {
            let td = $($("#gvPMTransactionDetails tr")[SelectedRowIndex]).find('td')[SelectedColIndex];
            $(td).find('.lblAction').text($('#ddlAction').val());
            $(td).find('.lblActionStatus').text($('#ddlStatus').val());
            $(td).find('.lblRemarks').text($('#txtRemarks').val());
            debugger;
            setStatusPendingToComplete();
            $('#extraFieldEntryModal').modal('hide');
            return false;
        }
        function setFieldsEntryValueToControls(isSaveBtnRequired) {
            let td = $($("#gvPMTransactionDetails tr")[SelectedRowIndex]).find('td')[SelectedColIndex];
            $('#ddlAction').val($(td).find('.lblAction').text());
            $('#ddlStatus').val($(td).find('.lblActionStatus').text());
            $('#txtRemarks').val($(td).find('.lblRemarks').text());
            if (isSaveBtnRequired) {
                $('#btnExtraFieldEntrySave').show();
            }
            else {
                $('#btnExtraFieldEntrySave').hide();
            }
            $('#extraFieldEntryModal').modal('show');
        }
        function setStatusPendingToComplete() {
            debugger;
            let tdindex = $("#gvPMTransactionDetails tr td").index($(this));
            let td = $($("#gvPMTransactionDetails tr")[SelectedRowIndex]).find('td')[SelectedColIndex];
            $(td).find('#lblPMStatus')[0].textContent = "C";
            $(td).find('#lblPMStatus').css("color", "#05f5dd");
            $(td).find('.lblEntryStatus').text("FieldsEntered");
        }
        function showActivityFile(ctrl) {
            debugger;
            $('#iframeActFile').css("display", "none");
            $('#videoActFile').css("display", "none");
            $('#imgActFile').css("display", "none");
            $('#videoActFile source').attr('src', '');
            $("#videoActFile")[0].load();
            $('#imgActFile').attr('src', '');
            $('#iframeActFile').attr("src", "");
            var fileBase64 = $(ctrl).closest('td').find('#hdnFileNameInBase64Old').val();
            var fileName = $(ctrl).closest('td').find('#hdnFileNameOld').val();
            var extension = fileName.split('.');
            extension = extension[extension.length - 1];
            var fileType = "";
            if ((extension == "mp4") || (extension == "wmv") || (extension == "avi") || (extension == "mov") || (extension == "qt") || (extension == "yuv") || (extension == "mkv") || (extension == "webm") || (extension == "flv") || (extension == "ogg") || (extension == "gif")) {
                fileType = "video";
            }
            else if (extension == "png" || extension == "jpg" || extension == "tif" || extension == "tiff" || extension == "bmp" || extension == "jpeg" || extension == "gif" || extension == "eps" || extension == "tif" || extension == "tif" || extension == "tif") {
                fileType = "image";
            }
            else if (extension == "pdf") {
                fileType = "pdf";
            }
            if (fileBase64 != "") {
                if (fileType == "video") {
                    if (extension == "gif") {
                        $('#imgActFile').css("display", "block");
                        $('#imgActFile').attr('src', "data:image/png;base64," + fileBase64);
                    }
                    else {
                        $('#videoActFile').css("display", "block");
                        $('#videoActFile source').attr('src', "data:video/mp4;base64," + fileBase64);
                        $("#videoActFile")[0].load();
                    }
                    $('#showActivityFile').modal('show');
                }
                else if (fileType == "image") {
                    const linkSource = `data:image/png;base64,${fileBase64}`;
                    const downloadLink = document.createElement("a");
                    downloadLink.href = linkSource;
                    downloadLink.download = fileName;
                    downloadLink.click();

                    $('#imgActFile').css("display", "block");
                    $('#imgActFile').attr('src', "data:image/png;base64," + fileBase64);
                    $('#showActivityFile').modal('show');
                    // $('#iframeActFile').css("display", "block");
                    //var arrrayBuffer = base64ToArrayBuffer(fileBase64); //data is the base64 encoded string
                    //var blob = new Blob([arrrayBuffer], { type: "data:image/jpeg;base64," });
                    //var link = window.URL.createObjectURL(blob);
                    ////$('#iframeActFile').attr("src", link);
                    //$('#iframeActFile').attr("src", "data:image/jpeg;base64," + fileBase64);
                    //$('#showActivityFile').modal('show');
                }
                else if (fileType == "pdf") {
                    $('#iframeActFile').css("display", "block");
                    var arrrayBuffer = base64ToArrayBuffer(fileBase64); //data is the base64 encoded string
                    var blob = new Blob([arrrayBuffer], { type: "application/pdf" });
                    var link = window.URL.createObjectURL(blob);
                    $('#iframeActFile').attr("src", link);
                    $('#showActivityFile').modal('show');
                }
                else {
                    flag = 1;
                }
            }
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
        function btnLoginConfirmationClick() {
            var isAuthorized = false;
            let userid = "";
            let password = "";

            userid = $("#txtUserid").val();
            password = $("#txtPassword").val();
            if (userid == "") {
                alert("User ID required.");
                return false;
            }
            if (password == "") {
                alert("Password required.");
                return false;
            }
            $.ajax({
                async: false,
                type: "POST",
                url: '<%= ResolveUrl("PMActivityTransactionData.aspx/getActvityTransactionAuthorization") %>',
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                data: '{username: "' + userid + '",password: "' + password + '"}',
                dataType: "json",
                success: function (response) {
                    debugger;
                    isAuthorized = response.d;
                },
                error: function (Result) {
                    alert("Error" + Result);
                }
            });

            if (isAuthorized) {
                $('#hdnLoggedInUserID').val(userid);
                $('#hdnLoginDisplayStatus').val("hide");
                $("#loginConfirmationModal").modal('hide');
                displayLoginBtn();
            }
            else {
                alert("Authorization Failed.");
            }
            return false;
        }

        function showSuccessMsg(msg, title) {
            debugger;
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "4000",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut",
                "toastClass": "toaster-position"
            }

            toastr['success'](msg, title);
            return false;
        }
        function toasterWarningMsg(msg, title) {
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "10000",
                "hideDuration": "10000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut",
                "toastClass": "toaster-position"
            }
            var d = Date();
            toastr.warning(msg, title);
            return false;
        }

        function btnActivityTransactionSaveClick() {
            debugger;
            var tblRows = $("#gvPMTransactionDetails tr");
            var isFalied = false;
            let machineid = $("#ddlMachineId").val();
            for (var rowCount = 1; rowCount < tblRows.length - 1; rowCount++) {
                var tblColumns = $(tblRows[rowCount]).find("td");
                let activity = $(tblRows[rowCount]).find("#hdnActivityID")[0].value;
                let frequency = $(tblRows[rowCount]).find("#hdnFrequency")[0].value;
                let shiftID = $(tblRows[rowCount]).find("#hdnShiftID").val();
                let shiftName = $(tblRows[rowCount]).find("#lblShift").text();
                let criteria = $(tblRows[rowCount]).find("#hdnCriteria").val();
                let category = $(tblRows[rowCount]).find("#hdnCategory").val();
                for (var colCount = 7; colCount < tblColumns.length; colCount++) {
                    let status = $(tblColumns[colCount]).find('#lblPMStatus')[0].textContent;
                    let entryStatus = $(tblColumns[colCount]).find('.lblEntryStatus')[0].textContent;
                    //let PMRemarks = $(tblColumns[colCount]).find('.lblPMRemarks')[0].textContent;
                    //let PMTargetTime = $(tblColumns[colCount]).find('.lblPMTargetTime')[0].textContent;
                    let actionStatus = "", action = "", remarks = "";
                    if (frequency == "3 Month" || frequency == "6 Month") {
                        actionStatus = $(tblColumns[colCount]).find(".lblActionStatus").text();
                        action = $(tblColumns[colCount]).find(".lblAction").text();
                        remarks = $(tblColumns[colCount]).find(".lblRemarks").text();
                    }
                    if (status == "C" && entryStatus == "FieldsEntered") {
                        let header = $("#gvPMTransactionDetails tr th")[colCount].textContent;
                        $.ajax({
                            async: false,
                            type: "POST",
                            url: '<%= ResolveUrl("PMActivityTransactionData.aspx/saveActvityTransactionData") %>',
                            contentType: "application/json; charset=utf-8",
                            crossDomain: true,
                            data: '{activity: "' + activity + '",frequency: "' + frequency + '",header: "' + header + '",machineid: "' + machineid + '",updatedBy: "' + $('#hdnLoggedInUserID').val() + '",shiftId: "' + shiftID + '",criteria: "' + criteria + '",status: "' + actionStatus + '",action: "' + action + '",remarks: "' + remarks + '",category: "' + category + '",shiftName: "' + shiftName + '"}',
                            dataType: "json",
                            success: function (response) {
                                debugger;
                                var isUpdated = response.d;
                                if (isUpdated) {
                                     showSuccessMsg('Record updated Successfully.', '');
                                    //-- $("#btnActivityTransactionSave").css("display", "none");
                                }
                                else {
                                    toasterWarningMsg('Failed to update record.','');
                                //isFalied = true;
                                //openErrorModal('Failed to update record.')
                                // $("#btnActivityTransactionSave").css("display", "inline-block");
                                }
                            },
                            error: function (Result) {
                                alert("Error" + Result);
                            }
                        });
                        //if (isFalied) {
                        //    break;
                        //    return false;
                        //}
                    }
                }
            }
            debugger;
            var footerRow = $(tblRows[tblRows.length - 1]);
            if ($(footerRow).css('display') != "none") {
                var footerColumns = $(footerRow).find('td');
                for (var colCount = 6; colCount < tblColumns.length; colCount++) {
                    let frequency = $('#ddlFrequency').val();
                    let header = $("#gvPMTransactionDetails tr th")[colCount].textContent;
                    let fileInBase64 = $(footerColumns[colCount]).find("#hdnFileNameInBase64").val();
                    if (fileInBase64 != "") {
                        fileName = $(footerColumns[colCount]).find("#hdnFileName").val();
                    }
                    else {
                        fileInBase64 = $(footerColumns[colCount]).find("#hdnFileNameInBase64Old").val();
                        fileName = $(footerColumns[colCount]).find("#hdnFileNameOld").val();
                    }
                    if (fileInBase64 == "" || fileInBase64 == undefined) {
                        continue;
                    }
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: '<%= ResolveUrl("PMActivityTransactionData.aspx/saveActivityFileDetails") %>',
                        contentType: "application/json; charset=utf-8",
                        crossDomain: true,
                        data: '{machineID: "' + machineid + '",frequency: "' + frequency + '",header: "' + header + '",fileName: "' + fileName + '",fileInBase64: "' + fileInBase64 + '"}',
                        dataType: "json",
                        success: function (response) {
                            debugger;
                            var isUpdated = response.d;

                        },
                        error: function (Result) {
                            alert("Error" + Result);
                        }
                    });
                }
            }
            return true;
        }
        async function FUChange(ctrl) {
            const filePathsPromises = [];
            const fileName = [];
            debugger;
            var fileExtension = ['png', 'jpg', 'jpeg', 'pdf'];
            debugger;
            for (var i = 0; i < $(ctrl).get(0).files.length; ++i) {
                filePathsPromises.push(ToBase64($(ctrl).get(0).files[i]));
                fileName.push($(ctrl).get(0).files[i].name);

                if ($.inArray($(ctrl).get(0).files[i].name.split('.').pop().toLowerCase(), fileExtension) == -1) {
                    openWarningModal_1("Allowed file types are png, jpg, jpeg and pdf only!", "");
                    $(ctrl).val("");
                    return;
                }
            }

            debugger;
            const filePaths = await Promise.all(filePathsPromises);
            var mappedFiles = filePaths.map((base64File) => ({ file: base64File }));
            let document = "", documentName = "";
            if (mappedFiles.length > 0) {
                document = mappedFiles[0].file;
                documentName = fileName[0];
            }
            else {
                document = $(ctrl).closest('td').find("#hdnFileNameInBase64Old").val();
                documentName = $(ctrl).closest('td').find("#hdnFileNameOld").val();
            }
            $(ctrl).closest('td').find("#hdnFileNameInBase64").val(document);
            $(ctrl).closest('td').find("#hdnFileName").val(documentName);
        }

        function ToBase64(file) {
            return new Promise((resolve, reject) => {
                const reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = () => resolve(reader.result);
                reader.onerror = error => reject(error);
            });
        };
        function ExportSelection() {
            $(".mandatory-message").text("");
            $('#exportMenuOptionModal').modal('show');
            return false;
        }
        function exportMenuDataValidation() {
            if ($('#txtFromDateExport').val() == "") {
                alert("Please select From Date.");
                return false;
            }
            if ($('#txtToDateExport').val() == "") {
                alert("Please select To Date.");
                return false;
            }
            var chk = $('#cbFrequency input');
            var chkFlag = false;
            for (var i = 0; i < chk.length; i++) {
                let inputChk = $('#cbFrequency input')[i];
                if (inputChk.checked) {
                    chkFlag = true;
                    break;
                }
            }
            if (chkFlag == false) {
                alert("Please select Frequency.");
                return false;
            }
            $('#exportMenuOptionModal').modal('hide');
            return true;
        }
        $('[id$=txtFromDateExport]').datepicker({
            //minViewMode: 2,
            format: 'dd-mm-yyyy',
            todayHighlight: true,
            autoclose: true,
            language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
        });
        $('[id$=txtToDateExport]').datepicker({
            //minViewMode: 2,
            format: 'dd-mm-yyyy',
            todayHighlight: true,
            autoclose: true,
            language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
        });

        $(document).keypress(function (e) {
            if ($("#loginConfirmationModal").hasClass('in') && (e.keycode == 13 || e.which == 13)) {
                $("#btnLoginConfirmation").trigger("click");
                return false;
            }
            if ($("#exportMenuOptionModal").hasClass('in') && (e.keycode == 13 || e.which == 13)) {
                $("#btnExportConfirm").trigger("click");
                return false;
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
